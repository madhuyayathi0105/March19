using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using Gios.Pdf;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Diagnostics;
using InsproDataAccess;
using System.Configuration;

public partial class CoeMod_ExamAttendanceReportNew : System.Web.UI.Page
{
    static Hashtable ht = new Hashtable();
    static Hashtable HashFloor = new Hashtable();
    static Hashtable HashDate = new Hashtable();
    static Hashtable HasSession = new Hashtable();
    static Hashtable Hashhall = new Hashtable();
    static Hashtable boundvl = new Hashtable();
    static Hashtable Hashdenm = new Hashtable();
    static Hashtable Hasdegree = new Hashtable();
    static Hashtable Hasroll = new Hashtable();
    static Hashtable hasbatch = new Hashtable();
    static Hashtable hassubno = new Hashtable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    Hashtable hat = new Hashtable();

    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string session_var = string.Empty;
    static int commcnt = 0;
    string value = string.Empty;
    string code = string.Empty;
    //string barCode = txtCode.Text;
    System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
    Font oFont = new Font("IDAutomationHC39M", 16);
    PointF point = new PointF(2f, 2f);
    SolidBrush blackBrush = new SolidBrush(Color.Black);
    SolidBrush whiteBrush = new SolidBrush(Color.White);
    bool flag_true = false;

    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(50);
            return img;
        }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        Control cntUpdateBtn = Subjectspread.FindControl("Update");
        Control cntCancelBtn = Subjectspread.FindControl("Cancel");
        Control cntCopyBtn = Subjectspread.FindControl("Copy");
        Control cntCutBtn = Subjectspread.FindControl("Clear");
        Control cntPasteBtn = Subjectspread.FindControl("Paste");
        Control cntPagePrintBtn = Subjectspread.FindControl("PrintPDF");
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
            tc = (TableCell)cntPagePrintBtn.Parent;
            tr.Cells.Remove(tc);
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerror1.Visible = false;
            lblerr1.Visible = false;
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
            if (!IsPostBack)
            {
                AttSpread.Visible = false;
                lblDispErr.Visible = false;
                lblDispErr.Text = string.Empty;
                divDummyNoSheets.Visible = false;
                btnDummyNoSheets.Visible = false;
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                //ddlMonth.Items.Clear();
                //ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                //ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                //ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                //ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                //ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                //ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                //ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                //ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                //ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                //ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                //ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                //ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                //ddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
                // BindExamtype();
                ddlsession.Items.Clear();
                ddlsession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddlsession.Items.Insert(1, new System.Web.UI.WebControls.ListItem("F.N", "1"));
                ddlsession.Items.Insert(2, new System.Web.UI.WebControls.ListItem("A.N", "2"));
                ddlsession.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Both", "3"));
                //ddltypes.Items.Clear();
                //ddltypes.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                //ddltypes.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Datewise Attandance Sheet", "1"));
                //ddltypes.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Hallwise Attandance Sheet", "2"));
                //ddltypes.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Departmentwise Attandance Sheet", "3"));
                cblsearch.Items[1].Selected = true;
                cblsearch.Items[3].Selected = true;
                cblsearch.Items[7].Selected = true;
                //month1();
                //year();
                BindExamYear();
                BindExamMonth();
                loadtype();
                fsPrintSetting.Visible = false;//Deepali 12.5.18
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void loadtype()
    {
        try
        {
            ddltype.Items.Clear();
            //string strtypequery = "select distinct isnull(type,'') type from course where isnull(type,'')>''";
            string strtypequery = "select distinct Mode from  Class_master";
            DataSet dstype = d2.select_method_wo_parameter(strtypequery, "text");
            if (dstype.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = dstype;
                ddltype.DataTextField = "Mode";
                ddltype.DataValueField = "Mode";
                ddltype.DataBind();
                ddltype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("ALL", "ALL"));
            }
            else
            {
                ddltype.Enabled = false;
            }
        }
        catch
        {
        }
    }

    protected void paneltxtdept_CheckedChanged(object sender, EventArgs e)
    {
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Button2.Visible = false;
        Subjectspread.Visible = false;
        if (cbbatselectall.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkbat.Items)
            {
                li.Selected = true;
                txtdept.Text = "Dept(" + (Chkbat.Items.Count) + ")";
            }
        }
        else
        {
            btnDisplay.Visible = false;
            btnPhaseSheet.Visible = false;
            btnFoilCard.Visible = false;
            btnDummyNoSheets.Visible = false;
            foreach (System.Web.UI.WebControls.ListItem li in Chkbat.Items)
            {
                li.Selected = false;
                txtdept.Text = "- - Select - -";
            }
        }
    }

    protected void paneltxtdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnDisplay.Visible = false;
        btnDummyNoSheets.Visible = false;
        divDummyNoSheets.Visible = false;
        btnPhaseSheet.Visible = false;
        btnFoilCard.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Button2.Visible = false;
        Subjectspread.Visible = false;
        int commcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < Chkbat.Items.Count; i++)
        {
            if (Chkbat.Items[i].Selected == true)
            {
                value = Chkbat.Items[i].Text;
                code = Chkbat.Items[i].Value.ToString();
                commcount = commcount + 1;
                txtdept.Text = "Dept(" + commcount.ToString() + ")";
            }
        }
        cbbatselectall.Checked = false;
        if (commcount == 0)
            txtdept.Text = "- - All - -";
        else
        {
        }
        commcnt = commcount;
    }

    protected void paneltxthallno_CheckedChanged(object sender, EventArgs e)
    {
        btnrest.Visible = false;
        paneltxtdept.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        Subjectspread.Visible = false;
        lbldepart.Visible = false;
        txtdept.Visible = false;
        paneltxtdept.Visible = false;
        lblerror1.Visible = false;
        Subjectspread.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Button2.Visible = false;
        lblnorecc.Visible = false;
        lblDispErr.Visible = false;
        btngenerate.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        AttSpread.Visible = false;
        g1btnprint.Visible = false;
        lblexportxl.Visible = false;
        g1btnexcel.Visible = false;
        txtexcell.Visible = false;
        lblerror1.Visible = false;
        btnDisplay.Visible = false;
        btnDummyNoSheets.Visible = false;
        divDummyNoSheets.Visible = false;
        btnPhaseSheet.Visible = false;
        btnFoilCard.Visible = false;
        if (cbdepselectall.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkdep.Items)
            {
                li.Selected = true;
                txthallno.Text = "Hall No(" + (Chkdep.Items.Count) + ")";
            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkdep.Items)
            {
                li.Selected = false;
                txthallno.Text = "- - Select - -";
            }
        }
    }

    protected void paneltxthallno_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnDisplay.Visible = false;
        btnDummyNoSheets.Visible = false;
        divDummyNoSheets.Visible = false;
        btnPhaseSheet.Visible = false;
        btnFoilCard.Visible = false;
        btnrest.Visible = false;
        paneltxtdept.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        Subjectspread.Visible = false;
        lbldepart.Visible = false;
        txtdept.Visible = false;
        paneltxtdept.Visible = false;
        lblerror1.Visible = false;
        Subjectspread.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Button2.Visible = false;
        lblnorecc.Visible = false;
        lblDispErr.Visible = false;
        btngenerate.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        AttSpread.Visible = false;
        g1btnprint.Visible = false;
        lblexportxl.Visible = false;
        g1btnexcel.Visible = false;
        txtexcell.Visible = false;
        int commcount = 0;
        for (int i = 0; i < Chkdep.Items.Count; i++)
        {
            if (Chkdep.Items[i].Selected == true)
            {
                value = Chkdep.Items[i].Text;
                code = Chkdep.Items[i].Value.ToString();
                commcount = commcount + 1;
                txthallno.Text = "Hall No(" + commcount.ToString() + ")";
            }
        }
        cbdepselectall.Checked = false;
        if (commcount == 0)
        {
            txthallno.Text = "- - All - -";
        }
        else
        {
        }
        commcnt = commcount;
    }

    public void Bindhallno()
    {
        try
        {
            Chkdep.Items.Clear();
            string months = ddlMonth.SelectedValue.ToString();
            string years = ddlYear.SelectedValue.ToString();
            string datess = ddlfrmdate.SelectedItem.Text;
            string[] fromdatespit99 = datess.ToString().Split('-');
            datess = fromdatespit99[2] + '-' + fromdatespit99[1] + '-' + fromdatespit99[0];
            string sedd = string.Empty;
            if (ddlsession.SelectedItem.Text == "Both")
            {
                sedd = string.Empty;
            }
            else
            {
                sedd = "and ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
            }
            string strtype = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
                {
                    //strtype = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
                    strtype = "and cs.Mode='" + ddltype.SelectedItem.ToString() + "'";
                }
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtype = "and c.type in('Day','MCA')";
                }
            }
            // string getdeteails = "SELECT distinct roomno FROM exam_seating where edate='" + datess + "' " + sedd + "";
            //string getdeteails = "SELECT distinct es.roomno FROM exam_seating es,Registration r,Degree d,course c where es.regno=r.Reg_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + strtype + " and edate='" + datess + "' " + sedd + "";
            string getdeteails = "SELECT distinct es.roomno ,cs.Priority FROM exam_seating es,Registration r,Degree d,course c ,Class_master cs where es.regno=r.Reg_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and cs.rno=es.roomno " + strtype + " and edate='" + datess + "' " + sedd + " order by cs.Priority";
            DataSet dssem = d2.select_method_wo_parameter(getdeteails, "Text");
            int count5 = dssem.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                Chkdep.DataSource = dssem;
                Chkdep.DataTextField = "roomno";
                Chkdep.DataValueField = "roomno";
                Chkdep.DataBind();
            }
            else
            {
                Chkdep.Items.Clear();
                txthallno.Text = "- - All - -";
            }
            if (count5 > 0)
            {
                cbdepselectall.Checked = true;
                for (int i = 0; i < Chkdep.Items.Count; i++)
                {
                    Chkdep.Items[i].Selected = true;
                    txthallno.Text = "Hall No(" + Chkdep.Items.Count + ")";
                }
            }
            // Chkdep.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch
        {
        }
    }

    public void Binddepart()
    {
        try
        {
            btnDisplay.Visible = false;
            btnDummyNoSheets.Visible = false;
            btnPhaseSheet.Visible = false;
            btnFoilCard.Visible = false;
            string departnt = string.Empty;
            int ledgercount = 0;
            string collgr = Session["collegecode"].ToString();
            string examdate = ddlfrmdate.SelectedValue.ToString();
            string[] dsplit = examdate.Split('-');
            examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
            //string sessiond = ddlsession.SelectedItem.Text;
            string sessiond = string.Empty;
            if (ddlsession.SelectedItem.Text == "Both")
            {
                sessiond = string.Empty;
            }
            else
            {
                sessiond = "and ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
            }
            for (int f = 0; f < Chkdep.Items.Count; f++)
            {
                if (Chkdep.Items[f].Selected == true)
                {
                    ledgercount = ledgercount + 1;
                    if (departnt == "")
                    {
                        departnt = Chkdep.Items[f].Value.ToString();
                    }
                    else
                    {
                        departnt = departnt + "','" + Chkdep.Items[f].Value.ToString();
                    }
                }
            }
            if (ledgercount == 0)
            {
                Chkbat.Visible = false;
                cbbatselectall.Visible = false;
                txtdept.Visible = false;
            }
            else
            {
                string getdeteails9 = "select distinct (c.Course_Name +'-'+ d.Acronym) as Grade,d.Degree_Code  from Degree d,course c,Department de,exam_seating es where d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and es.roomno in ('" + departnt + "')  and es.edate='" + examdate + "' " + sessiond + "";
                DataSet dssem = d2.select_method_wo_parameter(getdeteails9, "Text");
                int count5 = dssem.Tables[0].Rows.Count;
                if (count5 > 0)
                {
                    Chkbat.DataSource = dssem;
                    Chkbat.DataTextField = "Grade";
                    Chkbat.DataValueField = "Degree_Code";
                    Chkbat.DataBind();
                }
                else
                {
                    Chkbat.Visible = false;
                    cbbatselectall.Visible = false;
                    txtdept.Visible = false;
                }
                if (count5 > 0)
                {
                    cbbatselectall.Checked = true;
                    for (int i = 0; i < Chkbat.Items.Count; i++)
                    {
                        Chkbat.Items[i].Selected = true;
                        txtdept.Text = "Dept (" + Chkbat.Items.Count + ")";
                    }
                    Chkbat.Visible = true;
                    cbbatselectall.Visible = true;
                    txtdept.Visible = true;
                    btnDisplay.Visible = false;
                    btnDummyNoSheets.Visible = false;
                    btnPhaseSheet.Visible = false;
                    btnFoilCard.Visible = false;
                }
            }
            // Chkdep.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch
        {
        }
    }

    public void Bindexamdate()
    {
        try
        {
            DataSet ds1 = new DataSet();
            ddlfrmdate.Items.Clear();
            string strtype = string.Empty;
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
                {
                    strtype = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtype = "and c.type in('Day','MCA')";
                }
            }
            //string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date,exdt.Exam_date from exmtt_det as exdt,exmtt as exm where exm.exam_code=exdt.exam_code and exm.exam_month=" + ddlMonth.SelectedValue.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + " order by exdt.Exam_date";
            string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date ,datepart(day ,exam_date),datepart(month,exam_date),datepart(year,exam_date) from exmtt_det as exdt,exmtt as exm,degree d,Course c where exm.exam_code=exdt.exam_code and exm.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + strtype + " and exm.exam_month=" + ddlMonth.SelectedValue.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + " ";
            getexamdate = getexamdate + "   union all";
            getexamdate = getexamdate + "   select distinct  convert(varchar(10),e.ExamDate,105) as exam_date,datepart(day ,ExamDate),datepart(month,ExamDate),datepart(year,ExamDate) from examtheorybatch e,subject su where e.subno=su.subject_no  and DATEPART(year,ExamDate)=" + ddlYear.SelectedValue.ToString() + " order by datepart(year,exam_date),datepart(month,exam_date),datepart(day ,exam_date),exam_date";


            ds1 = d2.select_method_wo_parameter(getexamdate, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddlfrmdate.Items.Add("Select");
                ddlfrmdate.DataSource = ds1;
                ddlfrmdate.DataValueField = "exam_date";
                ddlfrmdate.DataBind();
            }
            ddlfrmdate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch
        {
        }
    }

    public void year()
    {
        ddlYear.Items.Clear();
        DataSet ds = d2.Examyear();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlYear.DataSource = ds;
            ddlYear.DataTextField = "Exam_year";
            ddlYear.DataValueField = "Exam_year";
            ddlYear.DataBind();
        }
        ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
    }

    protected void month1()
    {
        try
        {
            ddlMonth.Items.Clear();
            string year1 = ddlYear.SelectedValue;
            DataSet ds = d2.Exammonth(year1);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
            }
            ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch
        {
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    public void BindExamYear()
    {
        try
        {
            ddlYear.Items.Clear();
            string qry = "select distinct ed.Exam_year from exam_details ed where ed.Exam_year<>'0' order by ed.Exam_year desc";
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
            }
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    private void BindExamMonth()
    {
        try
        {
            ddlMonth.Items.Clear();
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
            if (!string.IsNullOrEmpty(ExamYear))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed where  ed.Exam_Month<>'0' " + ExamYear + " order by Exam_Month";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlMonth.DataSource = ds;
                    ddlMonth.DataTextField = "Month_Name";
                    ddlMonth.DataValueField = "Exam_Month";
                    ddlMonth.DataBind();
                }
            }
            ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnDisplay.Visible = false;
        btnDummyNoSheets.Visible = false;
        divDummyNoSheets.Visible = false;
        btnPhaseSheet.Visible = false;
        btnFoilCard.Visible = false;
        btnrest.Visible = false;
        paneltxtdept.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        Subjectspread.Visible = false;
        btngenerate.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Button2.Visible = false;
        lblnorecc.Visible = false;
        lblDispErr.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        lblerror1.Visible = false;
        //ddlexamtype.SelectedIndex = 0;
        lblDate.Visible = false;
        paneltxtdept.Visible = false;
        txtdept.Visible = false;
        lbldepart.Visible = false;
        lbldepart.Visible = false;
        Subjectspread.Visible = false;
        AttSpread.Visible = false;
        g1btnprint.Visible = false;
        lblexportxl.Visible = false;
        g1btnexcel.Visible = false;
        txtexcell.Visible = false;
        ddlfrmdate.Items.Clear();
        Bindexamdate();
        Bindhallno();
    }

    protected void ddlfrmdate_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnDisplay.Visible = false;
        divDummyNoSheets.Visible = false;
        btnDummyNoSheets.Visible = false;
        btnPhaseSheet.Visible = false;
        btnFoilCard.Visible = false;
        btnrest.Visible = false;
        paneltxtdept.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        Subjectspread.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Button2.Visible = false;
        lblnorecc.Visible = false;
        lblDispErr.Visible = false;
        lblDispErr.Text = string.Empty;
        btngenerate.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        lblerror1.Visible = false;
        Subjectspread.Visible = false;
        AttSpread.Visible = false;
        paneltxtdept.Visible = false;
        txtdept.Visible = false;
        lbldepart.Visible = false;
        g1btnprint.Visible = false;
        lblexportxl.Visible = false;
        g1btnexcel.Visible = false;
        txtexcell.Visible = false;
        Bindhallno();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnDisplay.Visible = false;
        btnDummyNoSheets.Visible = false;
        divDummyNoSheets.Visible = false;
        btnPhaseSheet.Visible = false;
        btnFoilCard.Visible = false;
        btnrest.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Button2.Visible = false;
        lblnorecc.Visible = false;
        lblDispErr.Visible = false;
        lblDispErr.Text = string.Empty;
        paneltxtdept.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        Subjectspread.Visible = false;
        btngenerate.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        paneltxtdept.Visible = false;
        txtdept.Visible = false;
        lbldepart.Visible = false;
        lblerror1.Visible = false;
        lblDate.Visible = false;
        Subjectspread.Visible = false;
        AttSpread.Visible = false;
        g1btnprint.Visible = false;
        lblexportxl.Visible = false;
        g1btnexcel.Visible = false;
        txtexcell.Visible = false;
        ddlfrmdate.Items.Clear();
        // ddlexamtype.Items.Clear();
        //BindExamtype();
        //month1();
        BindExamMonth();
        Bindexamdate();
        Bindhallno();
    }

    protected void ddlexamtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnrest.Visible = false;
        paneltxtdept.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        Subjectspread.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Button2.Visible = false;
        lblnorecc.Visible = false;
        lblDispErr.Visible = false;
        lblDispErr.Text = string.Empty;
        btngenerate.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        divDummyNoSheets.Visible = false;
        paneltxtdept.Visible = false;
        txtdept.Visible = false;
        lblerror1.Visible = false;
        Subjectspread.Visible = false;
        AttSpread.Visible = false;
        g1btnprint.Visible = false;
        lblexportxl.Visible = false;
        g1btnexcel.Visible = false;
        txtexcell.Visible = false;
        Bindexamdate();
        Bindhallno();
    }

    protected void checkconsolidate_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            fsPrintSetting.Visible = false;
            if (checkconsolidate.Checked == true)
            {
                btnDisplay.Visible = false;
                divDummyNoSheets.Visible = false;
                btnDummyNoSheets.Visible = false;
                btnPhaseSheet.Visible = false;
                btnFoilCard.Visible = false;
                AttSpread.Visible = false;
                lblerror1.Visible = false;
                Button2.Visible = false;
                lblrptname.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                g1btnprint.Visible = false;
                lblexportxl.Visible = false;
                g1btnexcel.Visible = false;
                txtexcell.Visible = false;
                btngenerate.Visible = false;
                lblPages.Visible = false;
                ddlPageNo.Visible = false;
                btnrest.Visible = false;
                paneltxtdept.Visible = false;
                Subjectspread.Visible = false;
                lblnorecc.Visible = false;
                lblDispErr.Visible = false;
                lblDispErr.Text = string.Empty;
                Subjectspread.Visible = false;
                txtdept.Visible = false;
                lbldepart.Visible = false;
                lblerr1.Visible = false;
            }
            else
            {
                btnDisplay.Visible = false;
                btnDummyNoSheets.Visible = false;
                btnPhaseSheet.Visible = false;
                btnFoilCard.Visible = false;
                AttSpread.Visible = false;
                lblerror1.Visible = false;
                Button2.Visible = false;
                lblrptname.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                g1btnprint.Visible = false;
                lblexportxl.Visible = false;
                g1btnexcel.Visible = false;
                txtexcell.Visible = false;
                btngenerate.Visible = false;
                lblPages.Visible = false;
                ddlPageNo.Visible = false;
                btnrest.Visible = false;
                paneltxtdept.Visible = false;
                Subjectspread.Visible = false;
                lblnorecc.Visible = false;
                lblDispErr.Visible = false;
                lblDispErr.Text = string.Empty;
                Subjectspread.Visible = false;
                txtdept.Visible = false;
                lbldepart.Visible = false;
                lblerr1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror1.Text = ex.ToString();
            lblerror1.Visible = true;
        }
    }

    protected void g1btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcell.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(AttSpread, reportname);
            }
            else
            {
                lblerror1.Text = "Please Enter Your Report Name";
                lblerror1.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror1.Text = ex.ToString();
            lblerror1.Visible = true;
        }
    }

    protected void g1btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Exam Attendance Report" + '@' + "                               " + "Date: " + ddlfrmdate.SelectedItem.ToString() + "                                                                                                                                                                       " + "Session: " + ddlsession.SelectedItem.ToString();
            string pagename = "cumreport.aspx";
            Printcontrol.loadspreaddetails(AttSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lblerror1.Text = ex.ToString();
            lblerror1.Visible = true;
        }
    }

    public void loadhalldetails()
    {
        try
        {
            Hashtable ht = new Hashtable();
            string departnt = string.Empty;
            int ledgercount = 0;
            for (int f = 0; f < Chkdep.Items.Count; f++)
            {
                if (Chkdep.Items[f].Selected == true)
                {
                    ledgercount = ledgercount + 1;
                    if (departnt == "")
                    {
                        departnt = Chkdep.Items[f].Value.ToString();
                    }
                    else
                    {
                        departnt = departnt + "','" + Chkdep.Items[f].Value.ToString();
                    }
                }
            }
            if (ledgercount == 0)
            {
                Chkbat.Visible = false;
                cbbatselectall.Visible = false;
                txtdept.Visible = false;
                AttSpread.Visible = false;
            }
            else
            {
                string strdegrcode = string.Empty;
                string str_degrcode = string.Empty;
                for (int depst = 0; depst < Chkbat.Items.Count; depst++)
                {
                    if (Chkbat.Items[depst].Selected == true)
                    {
                        if (strdegrcode == "")
                        {
                            strdegrcode = Chkbat.Items[depst].Value.ToString();
                        }
                        else
                        {
                            strdegrcode = strdegrcode + ',' + Chkbat.Items[depst].Value.ToString();
                        }
                    }
                }
                if (strdegrcode == "")
                {
                    btnrest.Visible = false;
                    paneltxtdept.Visible = false;
                    lblPages.Visible = false;
                    ddlPageNo.Visible = false;
                    Subjectspread.Visible = false;
                    btnDisplay.Visible = false;
                    btnDummyNoSheets.Visible = false;
                    divDummyNoSheets.Visible = false;
                    btnPhaseSheet.Visible = false;
                    btnFoilCard.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    Button2.Visible = false;
                    lblnorecc.Visible = false;
                    btngenerate.Visible = false;
                    lblPages.Visible = false;
                    ddlPageNo.Visible = false;
                    paneltxtdept.Visible = false;
                    txtdept.Visible = false;
                    lblerror1.Visible = false;
                    Subjectspread.Visible = false;
                    AttSpread.Visible = false;
                    return;
                }
                else
                {
                    str_degrcode = " and ed.degree_code in(" + strdegrcode + ")";  //Hide by aruna 30mar2017
                    strdegrcode = " and r.degree_code in(" + strdegrcode + ")";

                }
                lblDate.Visible = false;
                string fromdate = ddlfrmdate.SelectedValue.ToString();
                session_var = ddlsession.SelectedItem.Text;
                Session["session_var"] = session_var;
                string ff = ddlYear.SelectedValue.ToString();
                AttSpread.Visible = false;
                lblerror1.Visible = false;
                string sml = d2.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
                int bundleperstust = 0;
                if (sml != null && sml.Trim() != "" && sml.Trim() != "0")
                {
                    bundleperstust = Convert.ToInt32(sml);
                }
                if (bundleperstust == 0)
                {
                    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ Modified by Rajesh 06-05-2015 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                    if (checkconsolidate.Checked == true)
                    {
                        string value1 = string.Empty;
                        string code1 = string.Empty;
                        int sno = 1;
                        AttSpread.Sheets[0].RowCount = 0;
                        AttSpread.Sheets[0].ColumnCount = 0;
                        AttSpread.Sheets[0].ColumnHeader.RowCount = 2;
                        AttSpread.Sheets[0].ColumnCount = 10;
                        AttSpread.CommandBar.Visible = false;
                        AttSpread.Sheets[0].SheetCorner.ColumnCount = 0;
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total No of Candidate";
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Hall No";
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree/Branch";
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Code";
                        AttSpread.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Attended";
                        AttSpread.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Present";
                        AttSpread.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Absent";
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Reg No of Absentees";
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Booklet Numbers Returned";
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Signature of the Hall Superintendents";
                        AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                        MyStyle.Font.Size = FontUnit.Medium;
                        MyStyle.Font.Name = "Book Antiqua";
                        MyStyle.Font.Bold = true;
                        MyStyle.HorizontalAlign = HorizontalAlign.Center;
                        MyStyle.ForeColor = Color.Black;
                        MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        AttSpread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                        AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                        AttSpread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                        AttSpread.Sheets[0].DefaultStyle.Font.Bold = false;
                        AttSpread.Sheets[0].Columns[0].Width = 50;
                        AttSpread.Sheets[0].Columns[1].Width = 100;
                        AttSpread.Sheets[0].Columns[2].Width = 500;
                        AttSpread.Sheets[0].Columns[3].Width = 80;
                        AttSpread.Sheets[0].Columns[4].Width = 80;
                        AttSpread.Sheets[0].Columns[5].Width = 80;
                        AttSpread.Sheets[0].Columns[6].Width = 80;
                        AttSpread.Sheets[0].Columns[7].Width = 100;
                        AttSpread.Sheets[0].Columns[8].Width = 100;
                        AttSpread.Sheets[0].Columns[9].Width = 200;
                        AttSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 3);
                        AttSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        AttSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        AttSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        AttSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        AttSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                        AttSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                        AttSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                        Boolean reportflag = false;
                        string collgr = Session["collegecode"].ToString();
                        string examdate = ddlfrmdate.SelectedValue.ToString();
                        string[] dsplit = examdate.Split('-');
                        examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                        string sessiond = string.Empty;
                        if (ddlsession.SelectedItem.Text == "Both")
                        {
                            sessiond = string.Empty;
                        }
                        else
                        {
                            sessiond = "  and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
                        }
                        for (int i = 0; i < Chkdep.Items.Count; i++)
                        {
                            if (Chkdep.Items[i].Selected == true)
                            {
                                value1 = Chkdep.Items[i].Text;
                                code1 = Chkdep.Items[i].Value.ToString();
                                string query1 = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,s.subject_name,s.subject_code,c.Course_Name,de.Dept_Name from registration r,Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating as es,subject s,Degree d,Course c,Department de  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and es.regno=r.Reg_No and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and s.subject_no=ead.subject_no and s.subject_no=es.subject_no and es.degree_code=r.degree_code and ed.degree_code=es.degree_code and d.Degree_Code=r.degree_code and d.Course_Id=c.Course_Id and ed.degree_code=d.Degree_Code and d.Degree_Code=ed.degree_code and exam_flag<>'Debar' and d.Dept_Code=de.Dept_Code and ed.Exam_Month='" + ddlMonth.SelectedValue + "' and ed.Exam_year='" + ddlYear.SelectedItem + "' and es.edate='" + examdate + "' " + sessiond + " and es.roomno='" + value1 + "' group by es.edate,es.ses_sion,s.subject_name,s.subject_code,es.roomno,c.Course_Name,de.Dept_Name order by es.roomno";
                                DataSet dset1 = new DataSet();
                                dset1 = d2.select_method_wo_parameter(query1, "Text");
                                string hallno = string.Empty;
                                string dept = string.Empty;
                                string subcode = string.Empty;
                                string strngth = string.Empty;
                                if (dset1.Tables[0].Rows.Count > 0)
                                {
                                    AttSpread.Width = 1050;
                                    AttSpread.Visible = true;
                                    reportflag = true;
                                    lblerror1.Visible = false;
                                    btnDisplay.Visible = true;
                                    btnDummyNoSheets.Visible = true;
                                    btnPhaseSheet.Visible = true;
                                    for (int mm = 0; mm < dset1.Tables[0].Rows.Count; mm++)
                                    {
                                        AttSpread.Sheets[0].RowCount++;
                                        AttSpread.Sheets[0].AutoPostBack = true;
                                        hallno = dset1.Tables[0].Rows[mm]["roomno"].ToString();
                                        dept = dset1.Tables[0].Rows[mm]["Course_Name"].ToString() + " - " + dset1.Tables[0].Rows[mm]["Dept_Name"].ToString();
                                        subcode = dset1.Tables[0].Rows[mm]["subject_code"].ToString();
                                        strngth = dset1.Tables[0].Rows[mm]["strength"].ToString();
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].Text = hallno;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = dept;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Text = subcode;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = strngth;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                        //sno++;
                                        AttSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        AttSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        AttSpread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                        AttSpread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                                    }
                                    sno++;
                                }
                                else
                                {
                                    AttSpread.Visible = false;
                                    lblerror1.Text = "No Records Found";
                                    lblerror1.Visible = true;
                                    g1btnprint.Visible = false;
                                    lblexportxl.Visible = false;
                                    g1btnexcel.Visible = false;
                                    txtexcell.Visible = false;
                                    btngenerate.Visible = false;
                                    lblPages.Visible = false;
                                    ddlPageNo.Visible = false;
                                    Button2.Visible = false;
                                    lblrptname.Visible = false;
                                    btnExcel.Visible = false;
                                    txtexcelname.Visible = false;
                                    btnrest.Visible = false;
                                    paneltxtdept.Visible = false;
                                    Subjectspread.Visible = false;
                                    lblnorecc.Visible = false;
                                    lblDispErr.Visible = false;
                                    lblDispErr.Text = string.Empty;
                                    Subjectspread.Visible = false;
                                    txtdept.Visible = false;
                                    lbldepart.Visible = false;
                                    lblerr1.Visible = false;
                                }
                                AttSpread.Sheets[0].PageSize = AttSpread.Sheets[0].RowCount;
                            }
                        }
                        if (reportflag == true)
                        {
                            AttSpread.Visible = true;
                            lblerror1.Visible = false;
                            Printcontrol.Visible = false;
                            g1btnprint.Visible = true;
                            lblexportxl.Visible = true;
                            g1btnexcel.Visible = true;
                            txtexcell.Visible = true;
                            btngenerate.Visible = false;
                            lblPages.Visible = false;
                            ddlPageNo.Visible = false;
                            Button2.Visible = false;
                            lblrptname.Visible = false;
                            btnExcel.Visible = false;
                            txtexcelname.Visible = false;
                            btnrest.Visible = false;
                            paneltxtdept.Visible = false;
                            Subjectspread.Visible = false;
                            lblnorecc.Visible = false;
                            lblDispErr.Visible = false;
                            lblDispErr.Text = string.Empty;
                            Subjectspread.Visible = false;
                            txtdept.Visible = false;
                            lbldepart.Visible = false;
                            lblerr1.Visible = false;
                        }
                        else
                        {
                            AttSpread.Visible = false;
                            lblerror1.Text = "No Records Found";
                            lblerror1.Visible = true;
                            g1btnprint.Visible = false;
                            lblexportxl.Visible = false;
                            g1btnexcel.Visible = false;
                            txtexcell.Visible = false;
                            btngenerate.Visible = false;
                            lblPages.Visible = false;
                            ddlPageNo.Visible = false;
                            Button2.Visible = false;
                            lblrptname.Visible = false;
                            btnExcel.Visible = false;
                            txtexcelname.Visible = false;
                            btnrest.Visible = false;
                            paneltxtdept.Visible = false;
                            Subjectspread.Visible = false;
                            lblnorecc.Visible = false;
                            lblDispErr.Visible = false;
                            lblDispErr.Text = string.Empty;
                            Subjectspread.Visible = false;
                            txtdept.Visible = false;
                            lbldepart.Visible = false;
                            lblerr1.Visible = false;
                        }
                    }
                    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ Modified by Rajesh 06-05-2015 ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                    else
                    {
                        AttSpread.Sheets[0].RowCount = 0;
                        AttSpread.Sheets[0].ColumnCount = 4;
                        AttSpread.Sheets[0].RowHeader.Visible = false;
                        AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                        MyStyle.Font.Size = FontUnit.Medium;
                        MyStyle.Font.Name = "Book Antiqua";
                        MyStyle.Font.Bold = true;
                        MyStyle.HorizontalAlign = HorizontalAlign.Center;
                        MyStyle.ForeColor = Color.Black;
                        MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        AttSpread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                        AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                        AttSpread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                        AttSpread.Sheets[0].DefaultStyle.Font.Bold = false;
                        AttSpread.Sheets[0].Columns[0].Width = 100;
                        AttSpread.Sheets[0].Columns[1].Width = 100;
                        AttSpread.Sheets[0].Columns[2].Width = 150;
                        AttSpread.Sheets[0].Columns[3].Width = 150;
                        AttSpread.Sheets[0].Columns[0].Locked = true;
                        AttSpread.Sheets[0].Columns[2].Locked = true;
                        AttSpread.Sheets[0].Columns[3].Locked = true;
                        AttSpread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        AttSpread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                        AttSpread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                        AttSpread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                        AttSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.None);
                        AttSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.None);
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hall No";
                        AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total";
                        AttSpread.Sheets[0].AutoPostBack = false;
                        AttSpread.CommandBar.Visible = false;
                        string collgr = Session["collegecode"].ToString();
                        string examdate = ddlfrmdate.SelectedValue.ToString();
                        string[] dsplit = examdate.Split('-');
                        examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                        string sessiond = string.Empty;
                        if (ddlsession.SelectedItem.Text.Trim().ToLower() == "both" || ddlsession.SelectedItem.Text.Trim().ToLower() == "")
                        {
                            sessiond = string.Empty;
                        }
                        else
                        {
                            sessiond = "  and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
                        }
                        string spreadbind1 = "select es.roomno,COUNT(1) as strength,es.ses_sion,es.edate from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.roomno in ('" + departnt + "') and es.edate='" + examdate + "'  " + sessiond + " " + strdegrcode + " group by es.roomno,es.ses_sion,es.edate ";
                        DataSet ds2 = d2.select_method_wo_parameter(spreadbind1, "Text");
                        FarPoint.Web.Spread.CheckBoxCellType cheall = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.CheckBoxCellType cheselectall = new FarPoint.Web.Spread.CheckBoxCellType();
                        cheselectall.AutoPostBack = true;
                        string strength = string.Empty;
                        string roomno = string.Empty;
                        string sesson = string.Empty;
                        string exdate = string.Empty;
                        int sno = 1;
                        //int totalrows = 0;
                        int height = 45;
                        if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {
                            AttSpread.Width = 500;
                            AttSpread.Visible = true;
                            AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                            AttSpread.Sheets[0].Cells[0, 1].CellType = cheselectall;
                            AttSpread.Sheets[0].SpanModel.Add(AttSpread.Sheets[0].RowCount - 1, 2, 1, 3);
                            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                            {
                                AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                                height = height + AttSpread.Sheets[0].Rows[i].Height;
                                roomno = ds2.Tables[0].Rows[i]["roomno"].ToString();
                                strength = ds2.Tables[0].Rows[i]["strength"].ToString();
                                sesson = ds2.Tables[0].Rows[i]["ses_sion"].ToString();
                                exdate = ds2.Tables[0].Rows[i]["edate"].ToString();
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].CellType = cheall;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = roomno;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = exdate;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Text = strength;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Note = sesson;
                                sno++;
                            }
                            if (height > 600)
                            {
                                AttSpread.Height = 400;
                            }
                            else if (height > 500)
                            {
                                AttSpread.Height = height - 200;
                            }
                            else
                            {
                                AttSpread.Height = 300;
                            }
                            AttSpread.SaveChanges();
                            AttSpread.Sheets[0].PageSize = AttSpread.Sheets[0].RowCount;
                            btngenerate.Visible = true;
                            btnDisplay.Visible = true;
                            btnDummyNoSheets.Visible = true;
                            btnPhaseSheet.Visible = true;
                            lblPages.Visible = true;
                            ddlPageNo.Visible = true;
                            lblerr1.Visible = false;
                            lbldepart.Visible = true;
                            txtdept.Visible = true;
                            paneltxtdept.Visible = true;
                            cbbatselectall.Visible = true;
                            Chkbat.Visible = true;
                        }
                        else
                        {
                            cbbatselectall.Visible = false;
                            Chkbat.Visible = false;
                            lbldepart.Visible = false;
                            txtdept.Visible = false;
                            paneltxtdept.Visible = false;
                            lbldepart.Visible = false;
                            lblerr1.Visible = true;
                            lblerr1.Text = "No Records Found";
                            lblnorecc.Visible = false;
                            lblDispErr.Visible = false;
                            lblDispErr.Text = string.Empty;
                            lblrptname.Visible = false;
                            AttSpread.Visible = false;
                            paneltxtdept.Visible = false;
                            btngenerate.Visible = false;
                            btnDisplay.Visible = false;
                            btnDummyNoSheets.Visible = false;
                            btnPhaseSheet.Visible = false;
                            btnFoilCard.Visible = false;
                            lblPages.Visible = false;
                            ddlPageNo.Visible = false;
                            lblDate.Visible = false;
                        }
                    }
                }
                else
                {
                    AttSpread.Sheets[0].RowCount = 0;
                    AttSpread.Sheets[0].ColumnCount = 9;
                    AttSpread.Sheets[0].RowHeader.Visible = false;
                    AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    MyStyle.Font.Size = FontUnit.Medium;
                    MyStyle.Font.Name = "Book Antiqua";
                    MyStyle.Font.Bold = true;
                    MyStyle.HorizontalAlign = HorizontalAlign.Center;
                    MyStyle.ForeColor = Color.Black;
                    MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    AttSpread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                    AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    AttSpread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                    AttSpread.Sheets[0].DefaultStyle.Font.Bold = false;
                    AttSpread.Sheets[0].Columns[0].Width = 80;
                    AttSpread.Sheets[0].Columns[1].Width = 80;
                    AttSpread.Sheets[0].Columns[2].Width = 150;
                    AttSpread.Sheets[0].Columns[3].Width = 150;
                    AttSpread.Sheets[0].Columns[4].Width = 100;
                    AttSpread.Sheets[0].Columns[5].Width = 100;
                    AttSpread.Sheets[0].Columns[6].Width = 150;
                    AttSpread.Sheets[0].Columns[7].Width = 150;
                    AttSpread.Sheets[0].Columns[0].Locked = true;
                    AttSpread.Sheets[0].Columns[2].Locked = true;
                    AttSpread.Sheets[0].Columns[3].Locked = true;
                    AttSpread.Sheets[0].Columns[4].Locked = true;
                    AttSpread.Sheets[0].Columns[5].Locked = true;
                    AttSpread.Sheets[0].Columns[6].Locked = true;
                    AttSpread.Sheets[0].Columns[7].Locked = true;
                    AttSpread.Sheets[0].Columns[8].Visible = false;
                    AttSpread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    AttSpread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    AttSpread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    AttSpread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                    AttSpread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                    AttSpread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                    AttSpread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                    AttSpread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                    AttSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.None);
                    AttSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.None);
                    AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hall No";
                    AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Dept";
                    AttSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total";
                    AttSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Bundle No";
                    AttSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "From";
                    AttSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "To";
                    AttSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "degreecode";
                    AttSpread.Sheets[0].AutoPostBack = false;
                    AttSpread.CommandBar.Visible = false;
                    string collgr = Session["collegecode"].ToString();
                    string examdate = ddlfrmdate.SelectedValue.ToString();
                    string[] dsplit = examdate.Split('-');
                    examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                    string sessiond = string.Empty;
                    if (ddlsession.SelectedItem.Text == "Both")
                    {
                        sessiond = string.Empty;
                    }
                    else
                    {
                        sessiond = "  and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
                    }
                    //string spreadbind1 = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.degree_code,dp.dept_name,r.batch_year,es.subject_no,es.bundle_no  from registration r,exam_details ed,exam_application ea,exam_appl_details ead,exam_seating as es,degree d,department dp where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.roll_no and r.exam_flag<>'Debar' and es.regno=r.Reg_No and ead.subject_no=es.subject_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=d.Degree_Code and r.degree_code=d.Degree_Code and dp.dept_code=d.dept_code and d.college_code=r.college_code  and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.roomno in ('" + departnt + "') and es.edate='" + examdate + "' " + strdegrcode + " " + sessiond + " and r.college_code='" + collgr + "' group by es.roomno,es.ses_sion,es.edate ,r.degree_code,dp.dept_name,r.batch_year,es.subject_no,es.bundle_no ";
                    Dictionary<string, string> dicDegreeName = new Dictionary<string, string>();
                    string qry = "select dg.Degree_Code,c.Priority,ltrim(rtrim(isnull(c.type,''))) as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails from degree dg,Course c,department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and c.college_code=dt.college_code and dt.college_code=dg.college_code and dg.college_code=c.college_code order by c.type,Edu_Level desc,dg.Degree_Code,c.Priority";
                    DataSet dsDegreeName = new DataSet();
                    DataSet dsStudentDetails = new DataSet();
                    dsDegreeName = d2.select_method_wo_parameter(qry, "text");
                    if (dsDegreeName.Tables.Count > 0 && dsDegreeName.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drDegreeName in dsDegreeName.Tables[0].Rows)
                        {
                            string degreeCode = Convert.ToString(drDegreeName["Degree_Code"]).Trim();
                            string departmentName = Convert.ToString(drDegreeName["Dept_Name"]).Trim();
                            if (!dicDegreeName.ContainsKey(degreeCode.Trim().ToLower()))
                            {
                                dicDegreeName.Add(degreeCode.Trim().ToLower(), departmentName);
                            }
                        }
                    }
                    //Hide by aruna 30mar2017
                    //string spreadbind1 = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.degree_code,r.batch_year,es.subject_no,es.bundle_no  from registration r,exam_details ed,exam_application ea,exam_appl_details ead,exam_seating as es where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.roll_no and r.exam_flag<>'Debar' and es.regno=r.Reg_No and ead.subject_no=es.subject_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.roomno in ('" + departnt + "') and es.edate='" + examdate + "' " + strdegrcode + " " + sessiond + "  group by es.roomno,es.ses_sion,es.edate ,r.degree_code,r.batch_year,es.subject_no,es.bundle_no ";
                    string spreadbind1 = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,ed.degree_code,ed.batch_year,es.subject_no,es.bundle_no,cm.priority,cm.Mode  from registration r,exam_details ed,exam_application ea,exam_appl_details ead,exam_seating as es,class_master cm where cm.rno=es.roomno and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.roll_no and r.exam_flag<>'Debar' and es.regno=r.Reg_No and ead.subject_no=es.subject_no and   ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.roomno in ('" + departnt + "') and es.edate='" + examdate + "' " + str_degrcode + " " + sessiond + "  group by cm.Mode,cm.priority,es.roomno,es.ses_sion,es.edate,ed.degree_code,ed.batch_year,es.subject_no,es.bundle_no order by cm.Mode,cm.priority";
                    DataSet ds2 = d2.select_method_wo_parameter(spreadbind1, "Text");
                    qry = "select es.edate,es.ses_sion,es.roomno,es.subject_no,es.regno,es.degree_code,ed.Exam_Month,ed.Exam_year,r.Batch_Year,r.roll_no,es.seat_no,es.bundle_no from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and es.edate='" + examdate + "' " + strdegrcode + " " + sessiond + " and es.roomno in ('" + departnt + "')  and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' ";
                    dsStudentDetails = d2.select_method_wo_parameter(qry, "text");
                    FarPoint.Web.Spread.CheckBoxCellType cheall = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.CheckBoxCellType cheselectall = new FarPoint.Web.Spread.CheckBoxCellType();
                    cheselectall.AutoPostBack = true;
                    string strength = string.Empty;
                    string roomno = string.Empty;
                    string sesson = string.Empty;
                    string exdate = string.Empty;
                    string dept = string.Empty;
                    string bun = string.Empty;
                    string degrrcode = string.Empty;
                    string batchyr = string.Empty;
                    string sbjno = string.Empty;
                    int sno = 0;
                    string getfinbundleno = d2.GetFunction("select max(e.bundle_no),len(e.bundle_no) from exam_seating e,exmtt_det ed,exmtt et where ed.exam_code=et.exam_code and convert(nvarchar(15),ed.subject_no)=convert(nvarchar(15),e.subject_no )and ed.exam_date=e.edate and ed.exam_session=e.ses_sion and et.exam_month=" + ddlMonth.SelectedValue + " and et.exam_year=" + ddlYear.SelectedItem.Text + " group by len(e.bundle_no) order by len(e.bundle_no) desc");
                    if (getfinbundleno.Trim() == "" || getfinbundleno == "0")
                    {
                        getfinbundleno = d2.GetFunction("select value from COE_Master_Settings where settings='Bundle Number Generation'");
                    }
                    else
                    {
                        int incbun = Convert.ToInt32(getfinbundleno);
                        incbun++;
                        getfinbundleno = incbun.ToString();
                    }
                    if (getfinbundleno == "")
                    {
                        getfinbundleno = "1";
                    }
                    int bundle = Convert.ToInt32(getfinbundleno);
                    int height = 45;
                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        AttSpread.Width = 950;
                        AttSpread.Visible = true;
                        btnrest.Visible = true;
                        AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].CellType = cheselectall;
                        AttSpread.Sheets[0].SpanModel.Add(AttSpread.Sheets[0].RowCount - 1, 2, 1, 3);
                        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                        {
                            string bundleno = ds2.Tables[0].Rows[i]["bundle_no"].ToString();
                            roomno = ds2.Tables[0].Rows[i]["roomno"].ToString();
                            strength = ds2.Tables[0].Rows[i]["strength"].ToString();
                            //dept = ds2.Tables[0].Rows[i]["dept_name"].ToString();
                            sesson = ds2.Tables[0].Rows[i]["ses_sion"].ToString();
                            exdate = ds2.Tables[0].Rows[i]["edate"].ToString();
                            degrrcode = ds2.Tables[0].Rows[i]["degree_code"].ToString();
                            batchyr = ds2.Tables[0].Rows[i]["batch_year"].ToString();
                            sbjno = ds2.Tables[0].Rows[i]["subject_no"].ToString();
                            string departmentName = string.Empty;
                            if (dicDegreeName.ContainsKey(degrrcode.Trim().ToLower()))
                            {
                                departmentName = dicDegreeName[degrrcode.Trim().ToLower()];
                                dept = departmentName;
                            }
                            if (!ht.ContainsKey(sbjno + '-' + roomno))
                            {
                                AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                                sno++;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Note = sbjno;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = exdate;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Note = sesson;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].CellType = cheall;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 8].Text = ds2.Tables[0].Rows[i]["degree_code"].ToString();
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = roomno;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Note = batchyr;
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Text = dept;
                                ds2.Tables[0].DefaultView.RowFilter = "subject_no='" + sbjno + "' and edate='" + exdate + "' and ses_sion='" + sesson + "' and roomno='" + roomno + "'";
                                DataView dvstucount = ds2.Tables[0].DefaultView;
                                int stuco = 0;
                                for (int st = 0; st < dvstucount.Count; st++)
                                {
                                    stuco = stuco + Convert.ToInt32(dvstucount[st]["strength"].ToString());
                                    strength = stuco.ToString();
                                }
                                AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = strength;
                            }
                            int nodtubun = 0;
                            string nofstubundel = d2.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
                            if (nofstubundel != "" && nofstubundel != null)
                            {
                                nodtubun = Convert.ToInt32(nofstubundel);
                            }
                            string[] dtt = exdate.Split(' ');
                            exdate = dtt[0].ToString();
                            //  string cnt = "select * from exam_seating e,registration r where r.reg_no=e.regno and e.edate='" + exdate + "' and e.ses_sion ='" + sesson + "' and e.degree_code='" + degrrcode + "' and r.batch_year='" + batchyr + "' and roomno='" + roomno + "' and e.subject_no  ='" + sbjno + "' order by e.seat_no";
                            string cnt = "select es.edate,es.ses_sion,es.roomno,es.subject_no,es.regno,es.degree_code,r.Batch_Year,r.roll_no from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and es.edate='" + exdate + "' and es.ses_sion ='" + sesson + "'  and es.degree_code='" + degrrcode + "' and r.batch_year='" + batchyr + "' and roomno='" + roomno + "' and es.subject_no  ='" + sbjno + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' order by es.seat_no";
                            DataTable dtStudentsList = new DataTable();
                            if (dsStudentDetails.Tables.Count > 0 && dsStudentDetails.Tables[0].Rows.Count > 0)
                            {
                                DataView dvList = new DataView();
                                dsStudentDetails.Tables[0].DefaultView.RowFilter = "Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' and Exam_Month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and ses_sion ='" + sesson + "' and subject_no  ='" + sbjno + "' and edate='" + exdate + "' and roomno='" + roomno + "' and batch_year='" + batchyr + "' and degree_code='" + degrrcode + "'";
                                dvList = dsStudentDetails.Tables[0].DefaultView;
                                dvList.Sort = "seat_no";
                                dtStudentsList = dvList.ToTable(true, "edate", "ses_sion", "roomno", "subject_no", "regno", "degree_code", "Batch_Year", "roll_no", "seat_no", "bundle_no");
                            }
                            DataSet dsv = new DataSet();
                            dsv.Clear();
                            dsv.Tables.Add(dtStudentsList);
                            //dsv = d2.select_method_wo_parameter(cnt, "text"); by Malang Raja 
                            int kstartno = 0;
                            FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();
                            string ksregno = string.Empty;
                            if (dsv.Tables.Count > 0 && dsv.Tables[0].Rows.Count > 0)
                            {
                                if (!ht.ContainsKey(sbjno + '-' + roomno))
                                {
                                    ht.Add(sbjno + '-' + roomno, sbjno + '-' + roomno);
                                    for (int k = 0; k < dsv.Tables[0].Rows.Count; k++)
                                    {
                                        string newBundleNo = Convert.ToString(dsv.Tables[0].Rows[k]["bundle_no"]).Trim();
                                        if (string.IsNullOrEmpty(newBundleNo))
                                        {
                                            bundleno = d2.GetFunction("select bundle_no from exam_seating where regno='" + dsv.Tables[0].Rows[k]["regno"].ToString() + "' and edate='" + exdate + "' and ses_sion='" + sesson + "' and degree_code='" + degrrcode + "' ");
                                        }
                                        else
                                        {
                                            bundleno = newBundleNo;
                                        }
                                        string degCode = degrrcode;
                                        departmentName = string.Empty;
                                        degCode = Convert.ToString(ds2.Tables[0].Rows[i]["degree_code"]).Trim();
                                        if (dicDegreeName.ContainsKey(degCode.Trim().ToLower()))
                                        {
                                            departmentName = dicDegreeName[degCode.Trim().ToLower()];
                                            dept = departmentName;
                                        }
                                        if (k > 0)
                                        {
                                            sno++;
                                            AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Note = sbjno;
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = exdate;
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Note = sesson;
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].CellType = cheall;
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 8].Text = ds2.Tables[0].Rows[i]["degree_code"].ToString();
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = roomno;
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Note = batchyr;
                                            //departmentName = string.Empty;
                                            //degCode = Convert.ToString(ds2.Tables[0].Rows[i]["degree_code"]).Trim();
                                            //if (dicDegreeName.ContainsKey(degCode.Trim().ToLower()))
                                            //{
                                            //    departmentName = dicDegreeName[degCode.Trim().ToLower()];
                                            //    dept = departmentName;
                                            //}
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Text = dept;
                                        }
                                        if (bundleno.Trim() == "")
                                        {
                                            bun = bundle.ToString();
                                            bundle++;
                                        }
                                        else
                                        {
                                            bun = bundleno.ToString();
                                        }
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].CellType = txtceltype;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Text = bun;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Text = dept;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = dsv.Tables[0].Rows[k]["roomno"].ToString();
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = exdate;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Note = sesson;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].CellType = cheall;
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 8].Text = dsv.Tables[0].Rows[k]["degree_code"].ToString();
                                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = dsv.Tables[0].Rows[k]["regno"].ToString();
                                        k = k + nodtubun - 1;
                                        if (k < dsv.Tables[0].Rows.Count)
                                        {
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Text = dsv.Tables[0].Rows[k]["regno"].ToString();
                                            for (int ks = kstartno; ks <= k; ks++)
                                            {
                                                if (ksregno == "")
                                                {
                                                    ksregno = dsv.Tables[0].Rows[ks]["regno"].ToString();
                                                }
                                                else
                                                {
                                                    ksregno = ksregno + "','" + dsv.Tables[0].Rows[ks]["regno"].ToString();
                                                }
                                            }
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Tag = ksregno;
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].CellType = txtceltype;
                                            string sv = "update exam_seating set bundle_no='" + bun + "' where regno in ( '" + ksregno + "') and subject_no='" + sbjno + "' and edate='" + exdate + "' and ses_sion='" + sesson + "' and roomno='" + dsv.Tables[0].Rows[k]["roomno"].ToString() + "' and degree_code='" + dsv.Tables[0].Rows[k]["degree_code"].ToString() + "'";
                                            int k1 = d2.update_method_wo_parameter(sv, "text");
                                            ksregno = string.Empty;
                                            kstartno = k + 1;
                                        }
                                        else
                                        {
                                            for (int ks = kstartno; ks < dsv.Tables[0].Rows.Count; ks++)
                                            {
                                                if (ksregno == "")
                                                {
                                                    ksregno = dsv.Tables[0].Rows[ks]["regno"].ToString();
                                                }
                                                else
                                                {
                                                    ksregno = ksregno + "','" + dsv.Tables[0].Rows[ks]["regno"].ToString();
                                                }
                                            }
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Tag = ksregno;
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].CellType = txtceltype;
                                            AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dsv.Tables[0].Rows[dsv.Tables[0].Rows.Count - 1]["regno"].ToString());
                                            string sv = "update exam_seating set bundle_no='" + bun + "' where regno in ('" + ksregno + "') and subject_no='" + sbjno + "' and edate='" + exdate + "' and ses_sion='" + sesson + "' and roomno='" + AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text + "' and degree_code='" + AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 8].Text + "'";
                                            int k1 = d2.update_method_wo_parameter(sv, "text");
                                            kstartno = k;
                                            ksregno = string.Empty;
                                        }
                                        height = height + height + AttSpread.Sheets[0].Rows[AttSpread.Sheets[0].RowCount - 1].Height;
                                        //AttSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        //AttSpread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Bottom;
                                    }
                                }
                            }
                        }
                        if (height > 600)
                        {
                            AttSpread.Height = 500;
                        }
                        else if (height > 500)
                        {
                            AttSpread.Height = height - 200;
                        }
                        else if (height > 400)
                        {
                            AttSpread.Height = height - 100;
                        }
                        else
                        {
                            if (height < 0)
                            {
                                height = 500;
                            }
                            AttSpread.Height = height;
                        }
                        AttSpread.SaveChanges();
                        AttSpread.Sheets[0].PageSize = AttSpread.Sheets[0].RowCount;
                        btngenerate.Visible = true;
                        btnDisplay.Visible = true;
                        btnDummyNoSheets.Visible = true;
                        btnPhaseSheet.Visible = true;
                        btnFoilCard.Visible = true;
                        lblPages.Visible = true;
                        ddlPageNo.Visible = true;
                        lblerr1.Visible = false;
                        lbldepart.Visible = true;
                        txtdept.Visible = true;
                        paneltxtdept.Visible = true;
                        cbbatselectall.Visible = true;
                        Chkbat.Visible = true;
                    }
                    else
                    {
                        cbbatselectall.Visible = false;
                        Chkbat.Visible = false;
                        lbldepart.Visible = false;
                        txtdept.Visible = false;
                        paneltxtdept.Visible = false;
                        lbldepart.Visible = false;
                        lblerr1.Visible = true;
                        lblerr1.Text = "No Records Found";
                        lblnorecc.Visible = false;
                        lblDispErr.Visible = false;
                        lblDispErr.Text = string.Empty;
                        lblrptname.Visible = false;
                        AttSpread.Visible = false;
                        paneltxtdept.Visible = false;
                        btngenerate.Visible = false;
                        btnDisplay.Visible = false;
                        btnDummyNoSheets.Visible = false;
                        btnPhaseSheet.Visible = false;
                        btnFoilCard.Visible = false;
                        lblPages.Visible = false;
                        ddlPageNo.Visible = false;
                        lblDate.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        lblnorecc.Visible = false;
        lblDispErr.Visible = false;
        lblDispErr.Text = string.Empty;
        btnDisplay.Visible = true;
        btnDummyNoSheets.Visible = true;
        btnPhaseSheet.Visible = false;
        btnFoilCard.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Button2.Visible = false;
        btnrest.Visible = false;
        divDummyNoSheets.Visible = false;
        btngo.Visible = true;
        if (ddlMonth.SelectedValue.ToString() == "" || ddlMonth.SelectedIndex == 0)
        {
            btnDisplay.Visible = false;
            btnDummyNoSheets.Visible = false;
            btnPhaseSheet.Visible = false;
            btnFoilCard.Visible = false;
            btngenerate.Visible = false;
            AttSpread.Visible = false;
            lblPages.Visible = false;
            ddlPageNo.Visible = false;
            paneltxtdept.Visible = false;
            lbldepart.Visible = false;
            lblPages.Visible = false;
            ddlPageNo.Visible = false;
            lbldepart.Visible = false;
            txtdept.Visible = false;
            paneltxtdept.Visible = false;
            lblerror1.Visible = false;
            lblerr1.Visible = true;
            Subjectspread.Visible = false;
            lblerr1.Text = "Please Select Month";
            return;
        }
        else if (ddlYear.SelectedValue.ToString() == "" || ddlYear.SelectedIndex == 0)
        {
            btnDisplay.Visible = false;
            btnDummyNoSheets.Visible = false;
            btnPhaseSheet.Visible = false;
            btnFoilCard.Visible = false;
            btngenerate.Visible = false;
            AttSpread.Visible = false;
            lblPages.Visible = false;
            ddlPageNo.Visible = false;
            paneltxtdept.Visible = false;
            lbldepart.Visible = false;
            lblPages.Visible = false;
            ddlPageNo.Visible = false;
            lbldepart.Visible = false;
            txtdept.Visible = false;
            paneltxtdept.Visible = false;
            lblerror1.Visible = false;
            lblerr1.Visible = true;
            Subjectspread.Visible = false;
            lblerr1.Text = "Please Select Year";
            return;
        }
        else if (ddlfrmdate.SelectedValue.ToString() == "" || ddlfrmdate.SelectedIndex == 0)
        {
            btnDisplay.Visible = false;
            btnDummyNoSheets.Visible = false;
            btnPhaseSheet.Visible = false;
            btnFoilCard.Visible = false;
            btngenerate.Visible = false;
            AttSpread.Visible = false;
            lblPages.Visible = false;
            ddlPageNo.Visible = false;
            paneltxtdept.Visible = false;
            lbldepart.Visible = false;
            lblPages.Visible = false;
            ddlPageNo.Visible = false;
            lbldepart.Visible = false;
            txtdept.Visible = false;
            paneltxtdept.Visible = false;
            lblerror1.Visible = false;
            lblerr1.Visible = true;
            Subjectspread.Visible = false;
            lblerr1.Text = "Please Select Exam Date";
            return;
        }
        else if (ddlsession.SelectedValue.ToString() == "" || ddlsession.SelectedIndex == 0)
        {
            btnDisplay.Visible = false;
            btnDummyNoSheets.Visible = false;
            btnPhaseSheet.Visible = false;
            btnFoilCard.Visible = false;
            btngenerate.Visible = false;
            AttSpread.Visible = false;
            lblPages.Visible = false;
            ddlPageNo.Visible = false;
            paneltxtdept.Visible = false;
            lbldepart.Visible = false;
            lblPages.Visible = false;
            ddlPageNo.Visible = false;
            lbldepart.Visible = false;
            txtdept.Visible = false;
            paneltxtdept.Visible = false;
            lblerror1.Visible = false;
            lblerr1.Visible = true;
            Subjectspread.Visible = false;
            lblerr1.Text = "Please Select Session";
            return;
        }
        else
            if (chksubwise.Checked == false && txthallno.Text == "- - All - -" || Chkdep.Items.Count < 0)
            {
                btnDisplay.Visible = false;
                btnDummyNoSheets.Visible = false;
                btnPhaseSheet.Visible = false;
                btnFoilCard.Visible = false;
                btngenerate.Visible = false;
                AttSpread.Visible = false;
                lblPages.Visible = false;
                ddlPageNo.Visible = false;
                paneltxtdept.Visible = false;
                lbldepart.Visible = false;
                lblPages.Visible = false;
                ddlPageNo.Visible = false;
                lbldepart.Visible = false;
                txtdept.Visible = false;
                paneltxtdept.Visible = false;
                lblerror1.Visible = false;
                lblerr1.Visible = true;
                Subjectspread.Visible = false;
                lblerr1.Text = "Please Allot Hall No";
                return;
            }
            else
            {
                if (chksubwise.Checked == false)
                {
                    if (paneltxtdept.Visible == false)
                    {
                        btngenerate.Visible = false;
                        Binddepart();
                    }
                    loadhalldetails();
                }
                else
                {
                    if (!string.IsNullOrEmpty(ddlSubject.SelectedValue.ToString()))
                    {
                        loadSubjectwise();
                    }
                }
            }
        if (checkconsolidate.Checked == true)
        {
            btnDisplay.Visible = false;
            btnDummyNoSheets.Visible = false;
            btnPhaseSheet.Visible = false;
            btnFoilCard.Visible = false;
        }
    }

    protected void AttSpread_OnUpdateCommand(object sender, EventArgs e)
    {
        try
        {
            lblerror1.Visible = false;
            lblerror1.Text = string.Empty;
            if (Convert.ToInt32(AttSpread.Sheets[0].Cells[0, 1].Value) == 1)
            {
                for (int i = 0; i < AttSpread.Sheets[0].RowCount; i++)
                {
                    AttSpread.Sheets[0].Cells[i, 1].Value = 1;
                }
            }
            else if (Convert.ToInt32(AttSpread.Sheets[0].Cells[0, 1].Value) == 0)
            {
                for (int i = 0; i < AttSpread.Sheets[0].RowCount; i++)
                {
                    AttSpread.Sheets[0].Cells[i, 1].Value = 0;
                }
            }
            //string actrow = e.CommandArgument.ToString();
            //if (flag_true == false && actrow == "0")
            //{
            //    for (int j = 1; j < Convert.ToInt16(AttSpread.Sheets[0].RowCount); j++)
            //    {
            //        string actcol = e.SheetView.ActiveColumn.ToString();
            //        string seltext = e.EditValues[1].ToString();
            //        if (seltext != "System.Object" && seltext != "Selector For All")
            //        {
            //            AttSpread.Sheets[0].Cells[j, 1].Text = seltext.ToString();
            //        }
            //    }
            //    flag_true = true;
            //}
        }
        catch
        {
        }
    }

    protected void ddlsession_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnDisplay.Visible = false;
        btnDummyNoSheets.Visible = false;
        btnPhaseSheet.Visible = false;
        btnFoilCard.Visible = false;
        btnrest.Visible = false;
        paneltxtdept.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        Subjectspread.Visible = false;
        lblnorecc.Visible = false;
        lblDispErr.Visible = false;
        lblDispErr.Text = string.Empty;
        Subjectspread.Visible = false;
        AttSpread.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Button2.Visible = false;
        btngenerate.Visible = false;
        lblPages.Visible = false;
        ddlPageNo.Visible = false;
        paneltxtdept.Visible = false;
        txtdept.Visible = false;
        lbldepart.Visible = false;
        g1btnprint.Visible = false;
        lblexportxl.Visible = false;
        g1btnexcel.Visible = false;
        txtexcell.Visible = false;
        lblerror1.Visible = false;
        lblerr1.Visible = false;
        Bindhallno();
    }

    protected void btngenerate_click(object sender, EventArgs e)
    {
        try
        {
            bool subjectWise = false;
            if (chksubwise.Checked == true)
            {

                subjectWise = true;
                loadStudent();
                fsPrintSetting.Visible = true;//Deepali 12.5.18
                rblPrintSettingsHeader.SelectedIndex = 0;//Deepali 12.5.18
                rblPrintSettingsFooter.SelectedIndex = 0;//Deepali 12.5.18

            }
            if (subjectWise == false)
            {
                btnDisplay.Visible = true;
                divDummyNoSheets.Visible = false;
                lblDispErr.Visible = false;
                lblDispErr.Text = string.Empty;
                btnDummyNoSheets.Visible = true;
                btnPhaseSheet.Visible = true;
                btnFoilCard.Visible = true;
                //loadhalldetails();
                Subjectspread.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                Button2.Visible = false;
                lblnorecc.Visible = false;
                lblDispErr.Visible = false;
                lblDispErr.Text = string.Empty;
                AttSpread.SaveChanges();
                //loadstidanddetails();
                string hall = string.Empty;
                DateTime dt = new DateTime();
                AttSpread.Visible = true;
                HashDate.Clear();
                HashFloor.Clear();
                Hashhall.Clear();
                boundvl.Clear();
                HasSession.Clear();
                Hasdegree.Clear();
                Hasroll.Clear();
                Hashdenm.Clear();
                hasbatch.Clear();
                hassubno.Clear();
                int y = 0;
                string sml = d2.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
                Boolean chkgenflag = false;
                if (sml == null || sml.Trim() == "" || sml.Trim() == "0")
                {
                    for (int j = 1; j < AttSpread.Sheets[0].RowCount; j++)
                    {
                        if (Convert.ToInt16(AttSpread.Sheets[0].Cells[j, 1].Value) == 1)
                        {
                            if (hall != AttSpread.Sheets[0].Cells[j, 2].Text.ToString() || dt != Convert.ToDateTime(AttSpread.Sheets[0].Cells[j, 2].Note.ToString()))
                            {
                                chkgenflag = true;
                                //HashFloor.Add(y, AttSpread.Sheets[0].Cells[j, 7].Text.ToString());
                                Hashhall.Add(y, AttSpread.Sheets[0].Cells[j, 2].Text.ToString());
                                HashDate.Add(y, AttSpread.Sheets[0].Cells[j, 2].Note.ToString());
                                HasSession.Add(y, AttSpread.Sheets[0].Cells[j, 3].Note.ToString());
                                y = y + 1;
                            }
                            hall = AttSpread.Sheets[0].Cells[j, 2].Text.ToString();
                            dt = Convert.ToDateTime(AttSpread.Sheets[0].Cells[j, 2].Note.ToString());
                        }
                    }
                    if (chkgenflag == false)
                    {
                        lblerror1.Visible = true;
                        lblerror1.Text = "Please Select Subject And Then Proceed";
                        return;
                    }
                    string page = string.Empty;
                    int num = 1;
                    ddlPageNo.Items.Clear();
                    if (Hashhall.Count > 0)
                    {
                        lblPages.Visible = true;
                        ddlPageNo.Visible = true;
                        ddlPageNo.Items.Clear();
                        for (int i = 0; i < Hashhall.Count; i++)
                        {
                            page = Hashhall[i].ToString();
                            ddlPageNo.Items.Insert(i, new System.Web.UI.WebControls.ListItem(page, i.ToString()));
                            num = num + 1;
                        }
                        finalload();
                    }
                }
                else
                {
                    for (int j = 1; j < AttSpread.Sheets[0].RowCount; j++)
                    {
                        if (Convert.ToInt16(AttSpread.Sheets[0].Cells[j, 1].Value) == 1)
                        {
                            //if (hall != AttSpread.Sheets[0].Cells[j, 2].Text.ToString() || dt != Convert.ToDateTime(AttSpread.Sheets[0].Cells[j, 2].Note.ToString()))
                            {
                                //HashFloor.Add(y, AttSpread.Sheets[0].Cells[j, 7].Text.ToString());
                                chkgenflag = true;
                                Hashdenm.Add(y, AttSpread.Sheets[0].Cells[j, 3].Text.ToString());
                                Hashhall.Add(y, AttSpread.Sheets[0].Cells[j, 2].Text.ToString());
                                HashDate.Add(y, AttSpread.Sheets[0].Cells[j, 2].Note.ToString());
                                HasSession.Add(y, AttSpread.Sheets[0].Cells[j, 4].Note.ToString());
                                Hasdegree.Add(y, AttSpread.Sheets[0].Cells[j, 8].Text.ToString());
                                boundvl.Add(y, AttSpread.Sheets[0].Cells[j, 5].Text.ToString());
                                Hasroll.Add(y, "'" + AttSpread.Sheets[0].Cells[j, 6].Text.ToString() + "'" + " and " + "'" + AttSpread.Sheets[0].Cells[j, 7].Text.ToString() + "'");
                                hasbatch.Add(y, AttSpread.Sheets[0].Cells[j, 3].Note.ToString());
                                hassubno.Add(y, AttSpread.Sheets[0].Cells[j, 0].Note.ToString());
                                y = y + 1;
                            }
                            hall = AttSpread.Sheets[0].Cells[j, 2].Text.ToString();
                            dt = Convert.ToDateTime(AttSpread.Sheets[0].Cells[j, 2].Note.ToString());
                        }
                    }
                    if (chkgenflag == false)
                    {
                        lblerror1.Visible = true;
                        lblerror1.Text = "Please Select Subject And Then Proceed";
                        return;
                    }
                    string page = string.Empty;
                    int num = 1;
                    ddlPageNo.Items.Clear();
                    if (Hashhall.Count > 0)
                    {
                        lblPages.Visible = true;
                        ddlPageNo.Visible = true;
                        ddlPageNo.Items.Clear();
                        for (int i = 0; i < Hashhall.Count; i++)
                        {
                            page = Hashhall[i].ToString() + "-" + Hashdenm[i].ToString();
                            ddlPageNo.Items.Insert(i, new System.Web.UI.WebControls.ListItem(page, i.ToString()));
                            num = num + 1;
                        }
                        finalload1();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerror1.Visible = true;
            lblerror1.Text = ex.ToString();
        }
    }

    public void finalload1()
    {
        try
        {
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            AttSpread.SaveChanges();
            lblnorecc.Visible = false;
            lblDispErr.Visible = false;
            lblDispErr.Text = string.Empty;
            lblerror1.Visible = false;
            Subjectspread.Sheets[0].ColumnCount = 0;
            Subjectspread.Sheets[0].RowCount = 0;
            Subjectspread.Sheets[0].AutoPostBack = true;
            Subjectspread.SaveChanges();
            Subjectspread.Visible = false;
            Subjectspread.Sheets[0].ColumnCount = 11;
            Subjectspread.CommandBar.Visible = false;
            Subjectspread.RowHeader.Visible = false;
            Subjectspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Subjectspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Subjectspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].DefaultStyle.Font.Bold = false;
            Subjectspread.Sheets[0].AutoPostBack = true;
            Subjectspread.Sheets[0].Columns[0].Width = 50;
            Subjectspread.Sheets[0].Columns[1].Width = 130;
            Subjectspread.Sheets[0].Columns[2].Width = 850;
            Subjectspread.Sheets[0].Columns[3].Width = 150;
            Subjectspread.Sheets[0].Columns[4].Width = 250;
            Subjectspread.Sheets[0].Columns[5].Width = 100;
            Subjectspread.Sheets[0].Columns[6].Width = 130;
            Subjectspread.Sheets[0].Columns[7].Width = 250;
            Subjectspread.Sheets[0].Columns[9].Width = 100;
            Subjectspread.Sheets[0].Columns[10].Width = 150;
            Subjectspread.Sheets[0].Columns[0].CellType = txt;
            Subjectspread.Sheets[0].Columns[1].CellType = txt;
            Subjectspread.Sheets[0].Columns[2].CellType = txt;
            Subjectspread.Sheets[0].Columns[3].CellType = txt;
            Subjectspread.Sheets[0].Columns[4].CellType = txt;
            Subjectspread.Sheets[0].Columns[5].CellType = txt;
            Subjectspread.Sheets[0].Columns[6].CellType = txt;
            Subjectspread.Sheets[0].Columns[7].CellType = txt;
            Subjectspread.Sheets[0].Columns[9].CellType = txt;
            Subjectspread.Sheets[0].Columns[10].CellType = txt;
            Subjectspread.Sheets[0].RowCount++;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Register Number";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name of the Candidate     ";
            if (cblsearch.Items[5].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[3].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Type";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[3].Visible = false;
            }
            if (cblsearch.Items[6].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[4].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[4].Visible = false;
            }
            if (cblsearch.Items[1].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[5].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Seat No";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[5].Visible = false;
            }
            if (cblsearch.Items[4].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[6].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Hall No";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[6].Visible = false;
            }
            if (cblsearch.Items[7].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[7].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Subject Code And Name";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[7].Visible = false;
            }
            if (cblsearch.Items[0].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[8].Visible = true;
                Subjectspread.Sheets[0].Columns[8].Width = 200;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Answer Booklet No";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[8].Visible = false;
            }
            if (cblsearch.Items[2].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[9].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "HS to Write " + '"' + "AB" + '"' + " for Absentees";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[9].Visible = false;
            }
            if (cblsearch.Items[3].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[10].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Text = "        Signature of Candidate        ";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[10].Visible = false;
            }
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            Subjectspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Subjectspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            Subjectspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            Subjectspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            Subjectspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            string regno = string.Empty;
            string studname = string.Empty;
            string depattd = string.Empty;
            string studtype = string.Empty;
            string hallso = string.Empty;
            string seat = string.Empty;
            string subjectname = string.Empty;
            int sno = 0;
            string Date = string.Empty;
            string session = string.Empty;
            string HallNo = string.Empty;
            string hdeg = string.Empty;
            string hroll = string.Empty;
            string bndlee = string.Empty;
            int mm = 0;
            mm = Convert.ToInt16(ddlPageNo.SelectedValue);
            Date = HashDate[mm].ToString();
            session = HasSession[mm].ToString();
            HallNo = Hashhall[mm].ToString();
            hdeg = Hasdegree[mm].ToString();
            hroll = Hasroll[mm].ToString();
            bndlee = boundvl[mm].ToString();
            string departnt = string.Empty;
            int ledgercount = 0;
            string batch = hasbatch[mm].ToString();
            string subno = hassubno[mm].ToString();
            for (int f = 0; f < Chkbat.Items.Count; f++)
            {
                if (Chkbat.Items[f].Selected == true)
                {
                    ledgercount = ledgercount + 1;
                    if (departnt == "")
                    {
                        departnt = Chkbat.Items[f].Value.ToString();
                    }
                    else
                    {
                        departnt = departnt + "','" + Chkbat.Items[f].Value.ToString();
                    }
                }
            }
            string[] dummy_date_split = Date.Split(' ');
            string[] dsplit = dummy_date_split[0].Split('/');
            Date = dsplit[2].ToString() + "-" + dsplit[0].ToString() + "-" + dsplit[1].ToString();
            string collgr = Session["collegecode"].ToString();
            string qurreys1 = string.Empty;
            if (departnt != "" || ledgercount > 0)
            {
                qurreys1 = " and r.degree_code in ('" + hdeg + "') ";
            }
            string qurreys = "select r.Reg_No,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym)  as Grade,(s.subject_code +'-'+ s.subject_name) as subjectname ,(c.Course_Name +'-'+ de.dept_name) as deptname from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject s,Degree d,course c,Department de where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and r.degree_code=d.Degree_Code and ed.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and es.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.edate='" + Date + "' and es.ses_sion ='" + session + "'  " + qurreys1 + " and r.batch_year='" + batch + "' and roomno='" + HallNo + "' and es.subject_no  ='" + subno + "' and r.roll_no between " + hroll + " order by es.seat_no";
            DataSet daoverall = d2.select_method_wo_parameter(qurreys, "Text");
            if (daoverall.Tables[0].Rows.Count > 0)
            {
                int row = 0;
                for (int i = 0; i < daoverall.Tables[0].Rows.Count; i++)
                {
                    Subjectspread.Sheets[0].RowCount = Subjectspread.Sheets[0].RowCount + 1;
                    regno = daoverall.Tables[0].Rows[i]["Reg_No"].ToString();
                    studname = daoverall.Tables[0].Rows[i]["Stud_Name"].ToString();
                    studtype = daoverall.Tables[0].Rows[i]["Stud_Type"].ToString();
                    depattd = daoverall.Tables[0].Rows[i]["Grade"].ToString();
                    seat = daoverall.Tables[0].Rows[i]["seat_no"].ToString();
                    hallso = daoverall.Tables[0].Rows[i]["roomno"].ToString();
                    subjectname = daoverall.Tables[0].Rows[i]["subjectname"].ToString();
                    string deptpdf = daoverall.Tables[0].Rows[i]["deptname"].ToString();
                    sno++;
                    Subjectspread.Sheets[0].Cells[row, 0].Text = sno.ToString();
                    Subjectspread.Sheets[0].Cells[row, 1].Text = regno;
                    Subjectspread.Sheets[0].Cells[row, 2].Text = studname;
                    Subjectspread.Sheets[0].Cells[row, 3].Text = studtype;
                    Subjectspread.Sheets[0].Cells[row, 4].Text = depattd;
                    Subjectspread.Sheets[0].Cells[row, 4].Tag = deptpdf;
                    Subjectspread.Sheets[0].Cells[row, 5].Text = seat;
                    Subjectspread.Sheets[0].Cells[row, 6].Text = hallso;
                    Subjectspread.Sheets[0].Cells[row, 7].Text = subjectname;
                    Subjectspread.Sheets[0].Cells[row, 10].Text = "                                                     ";
                    row++;
                }
                Subjectspread.Sheets[0].Columns[8].Width = 200;
                Subjectspread.Sheets[0].RowCount = Subjectspread.Sheets[0].RowCount + 5;
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 6, 0, 1, 11);
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 6, 0].Text = string.Empty;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 6, 0].ForeColor = Color.White;
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 4, 0, 1, 11);
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 4, 0].Text = "sakthi";
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 4, 0].ForeColor = Color.White;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 5, 0].Font.Bold = true;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 5, 0].Font.Size = FontUnit.Medium;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Left;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 5, 0].HorizontalAlign = HorizontalAlign.Left;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Left;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 5, 0].Text = "Certified that the following particulars have been verified";
                printcheckvalue.Text = Convert.ToString(Subjectspread.Sheets[0].RowCount - 7);// added by sridhar
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Small;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Small;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Small;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Text = "1.The Register No. in the attendance sheet with that in the hall ticket.";
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Text = "2.The identification of the candidate with the photo pasted in the hall ticket";
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Text = "3.The answer book number entered in the attendance sheet by the candidate";
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 8].Text = "Page Total Present" + " " + ":";
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 8].Text = "Page Total Absent " + " " + ":";
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 1, 0, 1, 11);
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 2, 0, 1, 8);
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 3, 0, 1, 8);
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 5, 0, 1, 8);
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 2, 8, 1, 4);
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 3, 8, 1, 4);
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 4, 8, 1, 4);
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 8].Border.BorderColorBottom = Color.Black;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Border.BorderColorRight = Color.Black;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Border.BorderColorRight = Color.Black;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 8].Border.BorderColorBottom = Color.Black;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 8].Border.BorderColorTop = Color.Black;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 4].Border.BorderColorRight = Color.Black;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 9].Border.BorderColorBottom = Color.Black;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 9].Border.BorderColorBottom = Color.Black;
                Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 9].Border.BorderColorTop = Color.Black;
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 2, 9, 1, 4);
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 3, 9, 1, 4);
                Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 4, 9, 1, 4);
                Subjectspread.Visible = true;
                Subjectspread.Sheets[0].PageSize = Subjectspread.Sheets[0].RowCount;
                btnDisplay.Visible = true;
                btnDummyNoSheets.Visible = true;
                btnPhaseSheet.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnExcel.Visible = true;
                Button2.Visible = true;
                btngenerate.Visible = true;
                lblPages.Visible = true;
                ddlPageNo.Visible = true;
            }
            else
            {
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                Button2.Visible = false;
                lblnorecc.Visible = false;
                lblDispErr.Visible = false;
                lblDispErr.Text = string.Empty;
                lblerror1.Visible = true;
                lblerror1.Text = "No Records Found";
                btngenerate.Visible = true;
                lblPages.Visible = true;
                ddlPageNo.Visible = true;
                Subjectspread.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror1.Visible = true;
            lblerror1.Text = ex.ToString();
        }
    }

    public void finalload()
    {
        //string month =string.Empty;
        //string Year =string.Empty;
        //string MonthandYear =string.Empty;
        //month = ddlMonth.SelectedItem.Text.ToString();
        //Year = ddlYear.SelectedItem.Text.ToString();
        //MonthandYear = month.ToUpper() + "  " + Year.ToUpper();
        //MyImg mi = new MyImg();
        //mi.ImageUrl = "~/images/10BIT001.jpeg";
        //mi.ImageUrl = "Handler/Handler2.ashx?";
        //MyImg1 mi2 = new MyImg1();
        //mi2.ImageUrl = "~/images/10BIT001.jpeg";
        //mi2.ImageUrl = "Handler/Handler5.ashx?";
        //string str = "select isnull(collname, ' ') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode,isnull(acr,' ') as acr from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
        //SqlCommand comm = new SqlCommand(str, con);
        //SqlDataReader drr = comm.ExecuteReader();
        //drr.Read();
        //string coll_name = Convert.ToString(drr["collname"]);
        //string coll_address1 = Convert.ToString(drr["address1"]);
        //string coll_address2 = Convert.ToString(drr["address2"]);
        //string coll_address3 = Convert.ToString(drr["address3"]);
        //string pin_code = Convert.ToString(drr["pincode"]);
        //Session["acr"] = Convert.ToString(drr["acr"]);
        //string catgory = drr["category"].ToString();
        //catgory = "(An " + catgory + " Institution" + "," + "";
        //string affliatedby = drr["affliated"].ToString();
        //string affiliated = catgory + " " + "Affiliated to" + " " + affliatedby + ")";
        //string address = coll_address1 + "," + " " + coll_address2 + "," + " " + coll_address3 + "-" + " " + pin_code + ".";
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        AttSpread.SaveChanges();
        lblnorecc.Visible = false;
        lblDispErr.Visible = false;
        lblDispErr.Text = string.Empty;
        lblerror1.Visible = false;
        Subjectspread.Sheets[0].RowCount = 0;
        Subjectspread.Sheets[0].AutoPostBack = true;
        Subjectspread.SaveChanges();
        Subjectspread.Visible = false;
        Subjectspread.Sheets[0].ColumnCount = 0;
        Subjectspread.Sheets[0].ColumnCount = 12;
        Subjectspread.CommandBar.Visible = false;
        Subjectspread.RowHeader.Visible = false;
        Subjectspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        MyStyle.Font.Size = FontUnit.Medium;
        MyStyle.Font.Name = "Book Antiqua";
        MyStyle.Font.Bold = true;
        MyStyle.HorizontalAlign = HorizontalAlign.Center;
        MyStyle.ForeColor = Color.Black;
        MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        Subjectspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
        Subjectspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].DefaultStyle.Font.Bold = false;
        Subjectspread.Sheets[0].AutoPostBack = true;
        Subjectspread.Sheets[0].Columns[0].Width = 50;
        Subjectspread.Sheets[0].Columns[1].Width = 130;
        Subjectspread.Sheets[0].Columns[2].Width = 130;
        Subjectspread.Sheets[0].Columns[3].Width = 250;
        Subjectspread.Sheets[0].Columns[4].Width = 150;
        Subjectspread.Sheets[0].Columns[5].Width = 150;
        Subjectspread.Sheets[0].Columns[6].Width = 130;
        Subjectspread.Sheets[0].Columns[7].Width = 100;
        Subjectspread.Sheets[0].Columns[9].Width = 100;
        Subjectspread.Sheets[0].Columns[10].Width = 150;
        Subjectspread.Sheets[0].Columns[0].CellType = txt;
        Subjectspread.Sheets[0].Columns[1].CellType = txt;
        Subjectspread.Sheets[0].Columns[2].CellType = txt;
        Subjectspread.Sheets[0].Columns[3].CellType = txt;
        Subjectspread.Sheets[0].Columns[4].CellType = txt;
        Subjectspread.Sheets[0].Columns[5].CellType = txt;
        Subjectspread.Sheets[0].Columns[6].CellType = txt;
        Subjectspread.Sheets[0].Columns[7].CellType = txt;
        Subjectspread.Sheets[0].Columns[9].CellType = txt;
        Subjectspread.Sheets[0].Columns[10].CellType = txt;
        Subjectspread.Sheets[0].RowCount++;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register Number";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Name";
        if (cblsearch.Items[5].Selected == true)
        {
            Subjectspread.Sheets[0].Columns[4].Visible = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
        }
        else
        {
            Subjectspread.Sheets[0].Columns[4].Visible = false;
        }
        if (cblsearch.Items[6].Selected == true)
        {
            Subjectspread.Sheets[0].Columns[5].Visible = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Department";
        }
        else
        {
            Subjectspread.Sheets[0].Columns[5].Visible = false;
        }
        if (cblsearch.Items[1].Selected == true)
        {
            Subjectspread.Sheets[0].Columns[6].Visible = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Seat No";
        }
        else
        {
            Subjectspread.Sheets[0].Columns[6].Visible = false;
        }
        if (cblsearch.Items[4].Selected == true)
        {
            Subjectspread.Sheets[0].Columns[7].Visible = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Hall No";
        }
        else
        {
            Subjectspread.Sheets[0].Columns[7].Visible = false;
        }
        if (cblsearch.Items[7].Selected == true)
        {
            Subjectspread.Sheets[0].Columns[1].Visible = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Code";
        }
        else
        {
            Subjectspread.Sheets[0].Columns[1].Visible = false;
        }
        if (cblsearch.Items[0].Selected == true)
        {
            Subjectspread.Sheets[0].Columns[8].Visible = true;
            Subjectspread.Sheets[0].Columns[8].Width = 200;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Answer Booklet No";
        }
        else
        {
            Subjectspread.Sheets[0].Columns[8].Visible = false;
        }
        if (cblsearch.Items[2].Selected == true)
        {
            Subjectspread.Sheets[0].Columns[9].Visible = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "HS to Write " + '"' + "AB" + '"' + " for Absentees";
        }
        else
        {
            Subjectspread.Sheets[0].Columns[9].Visible = false;
        }
        if (cblsearch.Items[8].Selected == true)
        {
            Subjectspread.Sheets[0].Columns[10].Visible = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Student Photo";
        }
        else
        {
            Subjectspread.Sheets[0].Columns[10].Visible = false;
        }
        if (cblsearch.Items[3].Selected == true)
        {
            Subjectspread.Sheets[0].Columns[11].Visible = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Signature of Candidate  ";
        }
        else
        {
            Subjectspread.Sheets[0].Columns[11].Visible = false;
        }
        Subjectspread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
        Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
        Subjectspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        Subjectspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        Subjectspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
        Subjectspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
        Subjectspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
        Subjectspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
        Subjectspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
        Subjectspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
        Subjectspread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
        Subjectspread.Sheets[0].Columns[10].VerticalAlign = VerticalAlign.Middle;
        string regno = string.Empty;
        string studname = string.Empty;
        string depattd = string.Empty;
        string studtype = string.Empty;
        string hallso = string.Empty;
        string seat = string.Empty;
        string subjectname = string.Empty;
        int sno = 0;
        string Date = string.Empty;
        string session = string.Empty;
        string HallNo = string.Empty;
        int mm = 0;
        mm = Convert.ToInt16(ddlPageNo.SelectedValue);
        Date = HashDate[mm].ToString();
        session = HasSession[mm].ToString();
        HallNo = Hashhall[mm].ToString();
        string departnt = string.Empty;
        int ledgercount = 0;
        for (int f = 0; f < Chkbat.Items.Count; f++)
        {
            if (Chkbat.Items[f].Selected == true)
            {
                ledgercount = ledgercount + 1;
                if (departnt == "")
                {
                    departnt = Chkbat.Items[f].Value.ToString();
                }
                else
                {
                    departnt = departnt + "','" + Chkbat.Items[f].Value.ToString();
                }
            }
        }
        string[] dummy_date_split = Date.Split(' ');
        string[] dsplit = dummy_date_split[0].Split('/');
        Date = dsplit[2].ToString() + "-" + dsplit[0].ToString() + "-" + dsplit[1].ToString();
        string collgr = Session["collegecode"].ToString();
        string qurreys1 = string.Empty;
        if (departnt != "" || ledgercount > 0)
        {
            qurreys1 = " and r.degree_code in ('" + departnt + "') ";
        }
        //string qurreys = " select r.Reg_No,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname  from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + HallNo + "' and es.edate='" + Date + "' and es.ses_sion='" + session + "'  and r.college_code='" + collgr + "' " + qurreys1 + "  order by es.seat_no";
        //string qurreys = " select r.Reg_No,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,sub.subject_code  as subjectname  from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + HallNo + "' and es.edate='" + Date + "' and es.ses_sion='" + session + "'  and r.college_code='" + collgr + "' " + qurreys1 + "  order by es.seat_no";
        string qurreys = " select r.Reg_No,r.roll_no,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'- '+ d.Acronym) as Grade,(s.subject_code +'- '+ s.subject_name) as subjectname from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,Registration r,subject s,Degree d,Course c,Department de where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.roomno='" + HallNo + "' and es.edate='" + Date + "' and es.ses_sion='" + session + "'  and r.college_code='" + collgr + "' " + qurreys1 + "  order by subjectname,es.seat_no";
        DataSet daoverall = d2.select_method_wo_parameter(qurreys, "Text");
        if (daoverall.Tables[0].Rows.Count > 0)
        {
            int row = 0;
            for (int i = 0; i < daoverall.Tables[0].Rows.Count; i++)
            {
                Subjectspread.Sheets[0].RowCount = Subjectspread.Sheets[0].RowCount + 1;
                regno = daoverall.Tables[0].Rows[i]["Reg_No"].ToString();
                studname = daoverall.Tables[0].Rows[i]["Stud_Name"].ToString();
                studtype = daoverall.Tables[0].Rows[i]["Stud_Type"].ToString();
                depattd = daoverall.Tables[0].Rows[i]["Grade"].ToString();
                seat = daoverall.Tables[0].Rows[i]["seat_no"].ToString();
                hallso = daoverall.Tables[0].Rows[i]["roomno"].ToString();
                subjectname = daoverall.Tables[0].Rows[i]["subjectname"].ToString();
                string rollno = daoverall.Tables[0].Rows[i]["roll_no"].ToString();
                sno++;
                Subjectspread.Sheets[0].Cells[row, 0].Text = sno.ToString();
                Subjectspread.Sheets[0].Cells[row, 2].Text = regno;
                Subjectspread.Sheets[0].Cells[row, 3].Text = studname;
                Subjectspread.Sheets[0].Cells[row, 4].Text = studtype;
                Subjectspread.Sheets[0].Cells[row, 5].Text = depattd;
                Subjectspread.Sheets[0].Cells[row, 6].Text = seat;
                Subjectspread.Sheets[0].Cells[row, 7].Text = hallso;
                Subjectspread.Sheets[0].Cells[row, 1].Text = subjectname;
                MyImg mi = new MyImg();
                mi.ImageUrl = "Handler/Handler4.ashx?rollno=" + rollno;
                Subjectspread.Sheets[0].Cells[row, 10].CellType = mi;
                row++;
            }
            Subjectspread.Sheets[0].Columns[8].Width = 200;
            Subjectspread.Sheets[0].RowCount = Subjectspread.Sheets[0].RowCount + 2;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Font.Bold = true;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Text = "Certified that the following particulars have been verified";
            printcheckvalue.Text = Convert.ToString(Subjectspread.Sheets[0].RowCount - 5);// added by sridhar
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Small;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Small;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Text = "1.The Register No. in the attendance sheet with that in the hall ticket.";
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Text = "2.The identification of the candidate with the photo given in the hall ticket";
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 8].Text = "Page Total Present" + " " + ":";
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 8].Text = "Page Total Absent " + " " + ":";
            Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 1, 0, 1, 11);
            Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 2, 0, 1, 8);
            Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 3, 0, 1, 8);
            Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 2, 8, 1, 4);
            Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 3, 8, 1, 4);
            ////Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 4, 0, 1, 8);
            ////Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 5, 0, 1, 8);
            ////Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 6, 0, 1, 8);
            ////Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 7, 0, 1, 8);
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
            //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 4, 0].Border.BorderColorBottom = Color.White;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;
            //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 5, 0].Border.BorderColorBottom = Color.Black;
            //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 5, 0].Border.BorderColorRight = Color.Black;
            //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 6, 0].Border.BorderColorBottom = Color.White;
            //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 6, 0].Border.BorderColorRight = Color.Black;
            //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 7, 0].Border.BorderColorBottom = Color.White;
            //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 7, 0].Border.BorderColorRight = Color.Black;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 8].Border.BorderColorBottom = Color.Black;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Border.BorderColorRight = Color.Black;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Border.BorderColorRight = Color.Black;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 8].Border.BorderColorBottom = Color.Black;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 8].Border.BorderColorTop = Color.Black;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 4].Border.BorderColorRight = Color.Black;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 9].Border.BorderColorBottom = Color.Black;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 9].Border.BorderColorBottom = Color.Black;
            Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 9].Border.BorderColorTop = Color.Black;
            Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 2, 9, 1, 4);
            Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 3, 9, 1, 4);
            //Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 3, 8, 1, 3);
            //Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 4, 8, 1, 3);
            //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Text = "Signature of Hall Superintendent With Name";
            //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 4].Margin.Left = 50;
            //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 4].Text = "Signature of Chief Superintendent";
            Subjectspread.Visible = true;
            Subjectspread.Sheets[0].PageSize = Subjectspread.Sheets[0].RowCount;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnExcel.Visible = true;
            Button2.Visible = true;
            btngenerate.Visible = true;
            lblPages.Visible = true;
            ddlPageNo.Visible = true;
        }
        else
        {
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            Button2.Visible = false;
            lblnorecc.Visible = false;
            lblDispErr.Visible = false;
            lblDispErr.Text = string.Empty;
            lblerror1.Visible = true;
            lblerror1.Text = "No Records Found";
            btngenerate.Visible = true;
            lblPages.Visible = true;
            ddlPageNo.Visible = true;
            Subjectspread.Visible = false;
        }
    }

    protected void ddlPageNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sml = d2.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
        if (sml != null && sml.Trim() != "" && sml.Trim() != "0")
        {
            finalload1();
        }
        else
        {
            finalload();
        }
        //lblrptname.Visible = true;
        // txtexcelname.Visible = true;
        //btnExcel.Visible = true;
        //Button2.Visible = true;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            divDummyNoSheets.Visible = false;
            lblDispErr.Visible = false;
            lblDispErr.Text = string.Empty;
            string strexcelname = txtexcelname.Text;
            if (strexcelname != "")
            {
                d2.printexcelreport(Subjectspread, strexcelname);
            }
            else
            {
                lblnorecc.Text = "Please enter your Report Name";
                lblnorecc.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnrest_click(object sender, EventArgs e)
    {
        try
        {
            string strdate = ddlfrmdate.Text;
            if (strdate.Trim() != "")
            {
                string[] spda = strdate.Split('-');
                string date = spda[1] + '-' + spda[0] + '-' + spda[2];
                string strquery = "update es set es.bundle_no='' from exam_seating es,Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and ed.exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.edate='" + date + "'";
                int upbun = d2.update_method_wo_parameter(strquery, "Text");
                lblnorecc.Visible = false;
                divDummyNoSheets.Visible = false;
                lblDispErr.Visible = false;
                lblDispErr.Text = string.Empty;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                Button2.Visible = false;
                Subjectspread.Visible = false;
                AttSpread.Visible = false;
                btngenerate.Visible = false;
                lblPages.Visible = false;
                ddlPageNo.Visible = false;
                btnrest.Visible = false;
                btnDisplay.Visible = false;
                btnDummyNoSheets.Visible = false;
                btnPhaseSheet.Visible = false;
                btnFoilCard.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Deleted successfully')", true);
            }
        }
        catch (Exception ex)
        {
            lblnorecc.Text = ex.ToString();
            lblnorecc.Visible = true;
        }
    }

    protected void btnPrint_Click1(object sender, EventArgs e)
    {
        try
        {

            divDummyNoSheets.Visible = false;
            if (chksubwise.Checked == true)
            {
                printSubwise();
            }
            else
            {
                string collcode = Session["collegecode"].ToString();
                if (!File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                {
                    DataSet dsstuphoto = d2.select_method_wo_parameter("select fileupload from tbl_notification where viewrs='Printmaster' and College_Code='" + Session["collegecode"].ToString() + "'", "Text");
                    if (dsstuphoto.Tables[0].Rows.Count > 0)
                    {
                        if (dsstuphoto.Tables[0].Rows[0]["fileupload"] != null && dsstuphoto.Tables[0].Rows[0]["fileupload"].ToString().Trim() != "")
                        {
                            byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["fileupload"];
                            MemoryStream memoryStream = new MemoryStream();
                            memoryStream.Write(file, 0, file.Length);
                            if (file.Length > 0)
                            {
                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                System.Drawing.Image thumb = imgx.GetThumbnailImage(2630, 440, null, IntPtr.Zero);
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString().ToString() + ".jpeg")))
                                {
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString().ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                            memoryStream.Dispose();
                            memoryStream.Close();
                        }
                    }
                }
                string sml = d2.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
                if (sml == null || sml.Trim() == "" || sml.Trim() == "0")
                {
                    if (cblsearch.Items[8].Selected == true)
                    {
                        printattendancesheetwithphoto();
                    }
                    else
                    {
                        string examdate = ddlfrmdate.SelectedValue.ToString();
                        string strquery = ddlsession.SelectedItem.Text;
                        if (strquery.Trim() != "Both" && strquery.Trim() != "")
                        {
                            strquery = " and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
                        }
                        else
                        {
                            strquery = string.Empty;
                        }
                        string[] dsplit = examdate.Split('-');
                        examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                        string departnt = string.Empty;
                        int ledgercount = 0;
                        for (int f = 0; f < Chkdep.Items.Count; f++)
                        {
                            if (Chkdep.Items[f].Selected == true)
                            {
                                ledgercount = ledgercount + 1;
                                if (departnt == "")
                                {
                                    departnt = Chkdep.Items[f].Value.ToString();
                                }
                                else
                                {
                                    departnt = departnt + "','" + Chkdep.Items[f].Value.ToString();
                                }
                            }
                        }
                        string dcommt = string.Empty;
                        string mothyer = ddlMonth.SelectedValue.ToString() + "/" + ddlYear.SelectedValue.ToString();
                        string yersessn = ddlfrmdate.SelectedValue.ToString() + "/" + ddlsession.SelectedItem.ToString();
                        if (ddlsession.SelectedItem.ToString().ToLower() == "both")
                        {
                            String fnan = ddlsession.Items[1].ToString() + "/" + ddlsession.Items[2].ToString();
                            yersessn = ddlfrmdate.SelectedValue.ToString() + "/" + fnan;
                        }
                        string print = " select distinct es.roomno,s.subject_code,s.subject_name from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,Registration r,subject s,Degree d,Course c,Department de where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.roomno='" + ddlPageNo.SelectedItem.ToString() + "' and es.edate='" + examdate + "' " + strquery + "  and r.college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dsm = new DataSet();
                        dsm = d2.select_method_wo_parameter(print, "text");
                        string subcode = string.Empty;
                        for (int es = 0; es < dsm.Tables[0].Rows.Count; es++)
                        {
                            if (subcode == "")
                            {
                                subcode = dsm.Tables[0].Rows[es]["subject_code"].ToString() + " - " + dsm.Tables[0].Rows[es]["subject_name"].ToString();
                            }
                            else
                            {
                                subcode = subcode + ", " + dsm.Tables[0].Rows[es]["subject_code"].ToString() + " - " + dsm.Tables[0].Rows[es]["subject_name"].ToString();
                            }
                        }
                        dcommt = " ATTENDANCE FOR END OF SEMESTER EXAMINATIONS " + "@Date of Exam/Session : " + yersessn + "@" + "Room No : " + dsm.Tables[0].Rows[0]["roomno"].ToString() + "@Subject : " + subcode;
                        Printcontrol.loadspreaddetails(Subjectspread, "ExamattendanceReport.aspx", dcommt);
                        Printcontrol.Visible = true;
                    }
                }
                else
                {
                    Subjectspread.SaveChanges();
                    string mothyer = string.Empty;
                    string yersessn = string.Empty;
                    string sessn = string.Empty;
                    if (Hashhall.Count > 0)
                    {
                        ViewState["haltable"] = Hashhall;
                    }
                    if (boundvl.Count > 0)
                    {
                        ViewState["boundvaltable"] = boundvl;
                    }
                    int printmonth = ddlMonth.SelectedIndex;
                    DropDownList pntddlMonth = new DropDownList();
                    pntddlMonth.Items.Clear();
                    pntddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                    pntddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                    pntddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                    pntddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                    pntddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                    pntddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                    pntddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                    pntddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                    pntddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                    pntddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                    pntddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                    pntddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                    pntddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
                    string examstrMonthNameyear = string.Empty;
                    if (printmonth != 12)
                    {
                        examstrMonthNameyear = "  ATTENDANCE FOR END OF SEMESTER EXAMINATIONS - " + pntddlMonth.Items[printmonth].Text.ToString() + " / " + pntddlMonth.Items[printmonth + 1].Text.ToString() + "  " + ddlYear.SelectedValue.ToString();
                    }
                    else
                    {
                        examstrMonthNameyear = "  ATTENDANCE FOR END OF SEMESTER EXAMINATIONS - " + pntddlMonth.Items[printmonth].Text.ToString() + "  " + ddlYear.SelectedValue.ToString();
                    }
                    DAccess2 da = new DAccess2();
                    DataSet ds = new DataSet();
                    mothyer = ddlMonth.SelectedValue.ToString() + "/" + ddlYear.SelectedValue.ToString();
                    yersessn = ddlfrmdate.SelectedValue.ToString() + "/" + ddlsession.SelectedItem.ToString();
                    if (ddlsession.SelectedItem.ToString().ToLower() == "both")
                    {
                        String fnan = ddlsession.Items[1].ToString() + "/" + ddlsession.Items[2].ToString();
                        yersessn = ddlfrmdate.SelectedValue.ToString() + "/" + fnan;
                    }
                    sessn = ddlsession.SelectedItem.ToString();
                    string HallNo = string.Empty;
                    string bunl = string.Empty;
                    int mm1 = 0;
                    mm1 = Convert.ToInt16(ddlPageNo.SelectedValue);
                    Hashtable newhashhall = (Hashtable)ViewState["haltable"];
                    Hashtable newhashboundval = (Hashtable)ViewState["boundvaltable"];
                    if (newhashhall.Count > 0)
                    {
                        HallNo = newhashhall[mm1].ToString();
                    }
                    if (newhashboundval != null)
                    {
                        if (newhashboundval.Count > 0)
                        {
                            bunl = newhashboundval[mm1].ToString();
                        }
                    }
                    string examdate = ddlfrmdate.SelectedValue.ToString();
                    string[] dsplit = examdate.Split('-');
                    examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                    string departnt = string.Empty;
                    int ledgercount = 0;
                    for (int f = 0; f < Chkdep.Items.Count; f++)
                    {
                        if (Chkdep.Items[f].Selected == true)
                        {
                            ledgercount = ledgercount + 1;
                            if (departnt == "")
                            {
                                departnt = Chkdep.Items[f].Value.ToString();
                            }
                            else
                            {
                                departnt = departnt + "','" + Chkdep.Items[f].Value.ToString();
                            }
                        }
                    }
                    string dcommt = string.Empty;
                    Session["column_header_row_count"] = 3;
                    string strquery = ddlsession.SelectedItem.Text;
                    if (strquery.Trim() != "Both" && strquery.Trim() != "")
                    {
                        strquery = " and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
                    }
                    else
                    {
                        strquery = string.Empty;
                    }
                    string print = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.current_semester,r.degree_code,c.course_name + '-' +dp.dept_name as dept, r.batch_year,es.subject_no,sb.subject_name,sb.subject_code,es.bundle_no  from registration r,subjectchooser sc,exam_seating as  es,degree d,department dp,subject sb,course c where sc.roll_no=r.roll_no   and exam_flag<>'Debar' and es.regno=r.Reg_No  and es.subject_no=sc.subject_no and c.course_id=d.course_id and sc.subject_no=sb.subject_no and d.degree_code=r.degree_code and dp.dept_code=d.dept_code and  d.college_code=r.college_code and es.roomno in ('" + departnt + "') and es.edate='" + examdate + "' " + strquery + " and r.college_code='" + Session["collegecode"].ToString() + "' group by es.roomno,es.ses_sion,es.edate ,r.degree_code,  dp.dept_name,r.batch_year,es.subject_no,es.bundle_no,sb.subject_name,sb.subject_code,r.current_semester ,  c.course_name";
                    DataSet dsm = new DataSet();
                    dsm = d2.select_method_wo_parameter(print, "text");
                    if (dsm.Tables[0].Rows.Count > 0)
                    {
                        dcommt = " ATTENDANCE FOR END OF SEMESTER EXAMINATIONS " + '@' + "Degree & Branch : " + dsm.Tables[0].Rows[0]["dept"].ToString() + "                                                                              Semester:" + dsm.Tables[0].Rows[0]["current_semester"].ToString() + '@' + "Subject Code       : " + dsm.Tables[0].Rows[0]["subject_code"].ToString() + "                                                                   Date of Exam/Session : " + yersessn + '@' + " Subject Name     : " + dsm.Tables[0].Rows[0]["subject_name"].ToString() + "                                                                    " + "Room No : " + dsm.Tables[0].Rows[0]["roomno"].ToString() + "" + "/" + bunl;
                    }
                    Font Fontco18 = new Font("Book Antiqua", 18, FontStyle.Bold);
                    Font Fontco12 = new Font("Book Antiqua", 12, FontStyle.Bold);
                    Font Fontco12a = new Font("Book Antiqua", 12, FontStyle.Regular);
                    Font Fontco10 = new Font("Book Antiqua", 10, FontStyle.Bold);
                    Font Fontco10a = new Font("Book Antiqua", 10, FontStyle.Regular);
                    Font Fontco14 = new Font("Book Antiqua", 14, FontStyle.Bold);
                    Font Fontco14a = new Font("Book Antiqua", 14, FontStyle.Regular);
                    Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                    Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                    PdfArea pa1 = new PdfArea(mydoc, 14, 12, 566, 821);
                    PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                    mypdfpage.Add(pr3);
                    PdfTextArea pdf1;
                    string deptanddegree = Subjectspread.Sheets[0].Cells[0, 4].Tag.ToString();
                    string SubjectCodeAndName = Subjectspread.Sheets[0].Cells[0, 7].Text;
                    string[] splitSubjectCodeAndName = SubjectCodeAndName.Split('-');
                    if (chkheadimage.Checked == true)
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                            mypdfpage.Add(LogoImage, 20, 15, 400);
                        }
                    }
                    else
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 21, 25, 450);
                        }
                        string clm = "SELECT *  from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(clm, "text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                pdf1 = new PdfTextArea(Fontco18, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
                                mypdfpage.Add(pdf1);
                                string distpinspp = ds.Tables[0].Rows[0]["district"].ToString() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
                                string distpin = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                                string[] splitdistpin = distpin.Split(',');
                                pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + splitdistpin[0] + "");
                                mypdfpage.Add(pdf1);
                                pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 60, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Office of the Controller of Examinations");
                                mypdfpage.Add(pdf1);
                                pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + distpinspp + "");
                                mypdfpage.Add(pdf1);
                            }
                        }
                    }
                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + examstrMonthNameyear + "");
                    mypdfpage.Add(pdf1);
                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 13, 95, 750, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________");
                    mypdfpage.Add(pdf1);
                    int mm = Convert.ToInt16(ddlPageNo.SelectedValue);
                    string Date = HashDate[mm].ToString();
                    string session = HasSession[mm].ToString();
                    HallNo = Hashhall[mm].ToString();
                    string hdeg = Hasdegree[mm].ToString();
                    string hroll = Hasroll[mm].ToString();
                    string bndlee = boundvl[mm].ToString();
                    Gios.Pdf.PdfTable table1forpage1 = mydoc.NewTable(Fontco10a, 3, 4, 1);
                    table1forpage1.VisibleHeaders = false;
                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                    table1forpage1.SetColumnsWidth(new int[] { 55, 10, 88 });
                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Cell(0, 0).SetContent("Degree & Branch");
                    table1forpage1.Cell(0, 1).SetContent(":");
                    table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Cell(0, 2).SetContent(deptanddegree);
                    table1forpage1.Cell(2, 0).SetContent("Subject Name");
                    table1forpage1.Cell(2, 1).SetContent(":");
                    table1forpage1.Cell(2, 2).SetContent(splitSubjectCodeAndName[1].ToString());
                    table1forpage1.Cell(1, 0).SetContent("Subject Code ");
                    table1forpage1.Cell(1, 1).SetContent(":");
                    table1forpage1.Cell(1, 2).SetContent(splitSubjectCodeAndName[0].ToString());
                    table1forpage1.Cell(0, 3).SetContent("                      ");
                    table1forpage1.Cell(1, 3).SetContent("                        ");
                    table1forpage1.Cell(2, 3).SetContent("                      ");
                    Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, 115, 326, 200));
                    mypdfpage.Add(newpdftabpage2);
                    table1forpage1 = mydoc.NewTable(Fontco10a, 3, 4, 1);
                    table1forpage1.VisibleHeaders = false;
                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                    table1forpage1.SetColumnsWidth(new int[] { 65, 10, 88 });
                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Cell(0, 0).SetContent("Semester");
                    table1forpage1.Cell(0, 1).SetContent(":");
                    table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Cell(0, 2).SetContent(dsm.Tables[0].Rows[0]["current_semester"].ToString());
                    table1forpage1.Cell(1, 0).SetContent("Date of Exam/Session");
                    table1forpage1.Cell(1, 1).SetContent(":");
                    table1forpage1.Cell(1, 2).SetContent(yersessn);
                    table1forpage1.Cell(2, 0).SetContent("Room No / Bundle No ");
                    table1forpage1.Cell(2, 1).SetContent(":");
                    table1forpage1.Cell(2, 2).SetContent(HallNo + " / " + bndlee);
                    table1forpage1.Cell(0, 3).SetContent("                      ");
                    table1forpage1.Cell(1, 3).SetContent("                        ");
                    table1forpage1.Cell(2, 3).SetContent("                      ");
                    newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 340, 115, 326, 200));
                    mypdfpage.Add(newpdftabpage2);
                    table1forpage1 = mydoc.NewTable(Fontco10, 1, 5, 1);
                    table1forpage1.VisibleHeaders = false;
                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                    table1forpage1.SetColumnsWidth(new int[] { 40, 130, 250, 220, 200 });
                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Columns[4].SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Cell(0, 0).SetContent("S.No");
                    table1forpage1.Cell(0, 1).SetContent("Register Number");
                    table1forpage1.Cell(0, 2).SetContent("Name of the Candidate ");
                    table1forpage1.Cell(0, 3).SetContent("Answer Booklet No");
                    table1forpage1.Cell(0, 4).SetContent("Signature of the Candidate");
                    newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 22, 185, 550, 450));
                    mypdfpage.Add(newpdftabpage2);
                    //}
                    int rowscount1 = Convert.ToInt32(printcheckvalue.Text);
                    rowscount1 = rowscount1 + 2;
                    table1forpage1 = mydoc.NewTable(Fontco10a, rowscount1, 5, 1);
                    table1forpage1.VisibleHeaders = false;
                    table1forpage1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    table1forpage1.SetColumnsWidth(new int[] { 40, 130, 250, 220, 200 });
                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Columns[4].SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Cell(0, 0).SetContent("           ");
                    table1forpage1.Cell(0, 1).SetContent("               ");
                    table1forpage1.Cell(0, 2).SetContent("                     ");
                    table1forpage1.Cell(0, 3).SetContent("                       ");
                    table1forpage1.Cell(0, 4).SetContent("                        ");
                    for (int i = 0; i < rowscount1 - 1; i++)
                    {
                        table1forpage1.Cell(i + 1, 0).SetContent(Subjectspread.Sheets[0].Cells[i, 0].Text.ToString());
                        table1forpage1.Cell(i + 1, 1).SetContent(Subjectspread.Sheets[0].Cells[i, 1].Text.ToString());
                        table1forpage1.Cell(i + 1, 2).SetContent(Subjectspread.Sheets[0].Cells[i, 2].Text.ToString());
                        table1forpage1.Cell(i + 1, 3).SetContent(Subjectspread.Sheets[0].Cells[i, 8].Text.ToString());
                        table1forpage1.Cell(i + 1, 4).SetContent("                                                                                                 ");
                    }
                    newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 22, 180, 550, 550));
                    mypdfpage.Add(newpdftabpage2);
                    table1forpage1 = mydoc.NewTable(Fontco10, 1, 1, 1);
                    table1forpage1.VisibleHeaders = false;
                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Cell(0, 0).SetContent("Certified that the following particulars have been verified");
                    newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, 720, 326, 200));
                    mypdfpage.Add(newpdftabpage2);
                    table1forpage1 = mydoc.NewTable(Fontco10a, 3, 1, 1);
                    table1forpage1.VisibleHeaders = false;
                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Cell(0, 0).SetContent("1.The Register No. in the attendance sheet with that in the hall ticket.");
                    table1forpage1.Cell(1, 0).SetContent("2.The identification of the candidate with the photo pasted in the hall ticket");
                    table1forpage1.Cell(2, 0).SetContent("3.The answer book number entered in the attendance sheet by the candidate");
                    newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, 735, 500, 200));
                    mypdfpage.Add(newpdftabpage2);
                    pa1 = new PdfArea(mydoc, 20, 720, 350, 110);
                    pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                    mypdfpage.Add(pr3);
                    table1forpage1 = mydoc.NewTable(Fontco10a, 2, 3, 1);
                    table1forpage1.VisibleHeaders = false;
                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                    table1forpage1.SetColumnsWidth(new int[] { 80, 10, 88 });
                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage1.Cell(0, 0).SetContent("Total Present");
                    table1forpage1.Cell(0, 1).SetContent(" ");
                    table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Cell(0, 2).SetContent("                                             ");
                    table1forpage1.Cell(1, 0).SetContent("Total Absent");
                    table1forpage1.Cell(1, 1).SetContent(" ");
                    table1forpage1.Cell(1, 2).SetContent("                                              ");
                    newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 380, 730, 190, 100));
                    mypdfpage.Add(newpdftabpage2);
                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 26, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
                    mypdfpage.Add(pdf1);
                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 225, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
                    mypdfpage.Add(pdf1);
                    pa1 = new PdfArea(mydoc, 450, 730, 73, 50);
                    pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                    mypdfpage.Add(pr3);
                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 450, 745, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________");
                    mypdfpage.Add(pdf1);
                    mypdfpage.SaveToDocument();
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "Attendance12" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                        mydoc.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblnorecc.Text = ex.ToString();
            lblnorecc.Visible = true;
        }
    }

    public void Display()
    {
        try
        {
            divDummyNoSheets.Visible = false;
            lblDispErr.Visible = false;
            lblDispErr.Text = string.Empty;
            AttSpread.SaveChanges();
            int g = 1;
            string collgr = string.Empty;
            string affilitied = string.Empty;
            string collname = string.Empty;
            string pincode = string.Empty;
            string district = string.Empty;
            string Date = string.Empty;
            int mm = 0;
            int y = 0;
            string HallNo = string.Empty;
            string session = string.Empty;
            string hdeg = string.Empty;
            string hroll = string.Empty;
            string bndlee = string.Empty;
            string batch = string.Empty;
            string subno = string.Empty;
            string hall = string.Empty;
            DataSet dsdisplay = new DataSet();
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);
            Boolean chkgenflag = false;
            DateTime dt = new DateTime();
            int coltop = 10;
            coltop = coltop + 5;
            int coltop1 = coltop;
            int finctop = coltop;
            int yq = 180;
            string strquery = string.Empty;
            int isval = 0;
            int ji = 0;
            int tablepadding = 10;
            strquery = "Select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
            string sml = d2.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
            if (sml.Trim() != "" && sml.Trim() != "0")
            {
                if (Convert.ToInt32(sml) > 15)
                {
                    tablepadding = 6;
                }
                else
                {
                    tablepadding = 10;
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        collname = ds.Tables[0].Rows[0]["collname"].ToString();
                        affilitied = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        district = ds.Tables[0].Rows[0]["district"].ToString();
                        pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                        string[] aff = affilitied.Split(',');
                        affilitied = aff[0].ToString();
                        boundvl.Clear();
                        HasSession.Clear();
                        Hasdegree.Clear();
                        Hashdenm.Clear();
                        Hashhall.Clear();
                        HashDate.Clear();
                        Hasroll.Clear();
                        hassubno.Clear();
                        hasbatch.Clear();
                        int u = 0;
                        for (mm = 0; mm < AttSpread.Sheets[0].Rows.Count; mm++)
                        {
                            isval = Convert.ToInt32(AttSpread.Sheets[0].Cells[u, 1].Value);
                            u = u + 1;
                            if (isval == 1 && u > 1)
                            {
                                y = y + 1;
                                chkgenflag = true;
                                lblerr1.Visible = false;
                                lblerr1.Text = string.Empty;
                                coltop = 10;
                                hall = AttSpread.Sheets[0].Cells[u - 1, 2].Text.ToString();
                                dt = Convert.ToDateTime(AttSpread.Sheets[0].Cells[u - 1, 2].Note.ToString());
                                PdfArea tete = new PdfArea(mydocument, 15, 10, 565, 825);
                                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                Date = AttSpread.Sheets[0].Cells[u - 1, 2].Note.ToString();
                                session = AttSpread.Sheets[0].Cells[u - 1, 4].Note.ToString();
                                HallNo = AttSpread.Sheets[0].Cells[u - 1, 2].Text.ToString();
                                hdeg = AttSpread.Sheets[0].Cells[u - 1, 8].Text.ToString();
                                hroll = AttSpread.Sheets[0].Cells[u - 1, 6].Text.ToString();
                                //hroll = "'" + hroll + "'  and  '" + AttSpread.Sheets[0].Cells[u - 1, 7].Text.ToString() + "'";
                                bndlee = AttSpread.Sheets[0].Cells[u - 1, 5].Text.ToString();
                                batch = AttSpread.Sheets[0].Cells[u - 1, 3].Note.ToString();
                                subno = AttSpread.Sheets[0].Cells[u - 1, 0].Note.ToString();
                                string[] dummy_date_split = Date.Split(' ');
                                string[] dsplit = dummy_date_split[0].Split('/');
                                Date = dsplit[2].ToString() + "-" + dsplit[0].ToString() + "-" + dsplit[1].ToString();
                                collgr = Session["collegecode"].ToString();
                                // query = "select r.Reg_No,r.Stud_Name,r.Stud_Type,r.current_semester,s.subject_code,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym)  as Grade,sc.semester,(s.subject_code +'-'+ s.subject_name) as subjectname ,(c.Course_Name +'-'+ de.dept_name) as deptname from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject s,Degree d,course c,Department de,subjectchooser sc where sc.subject_no=s.subject_no and sc.roll_no=r.roll_no and ea.roll_no=sc.roll_no and ead.subject_no=sc.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and r.degree_code=d.Degree_Code and ed.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and es.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.edate='" + Date + "' and es.ses_sion ='" + session + "' and r.degree_code='" + hdeg + "' and r.batch_year='" + batch + "' and roomno='" + HallNo + "' and es.subject_no  ='" + subno + "' and r.Reg_No in ('" + AttSpread.Sheets[0].Cells[u - 1, 7].Tag.ToString() + "') order by es.seat_no";
                                //string query = "select r.Reg_No,r.Stud_Name,r.Stud_Type,r.current_semester,s.subject_code,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym)  as Grade,(s.subject_code +'-'+ s.subject_name) as subjectname ,(c.Course_Name +'-'+ de.dept_name) as deptname from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject s,Degree d,course c,Department de where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and r.degree_code=d.Degree_Code and ed.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and es.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.edate='" + Date + "' and es.ses_sion ='" + session + "' and r.degree_code='" + hdeg + "' and r.batch_year='" + batch + "' and roomno='" + HallNo + "' and es.subject_no  ='" + subno + "' and r.Reg_No in ('" + AttSpread.Sheets[0].Cells[u - 1, 7].Tag.ToString() + "') order by es.seat_no";
                                string query = "select distinct r.Reg_No,r.Stud_Name,r.Stud_Type,r.current_semester,s.subject_code,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym)  as Grade,sc.semester,(s.subject_code +'-'+ s.subject_name) as subjectname ,(c.Course_Name +'-'+ de.dept_name) as deptname,es.bundle_no from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject s,Degree d,course c,Department de,subjectchooser sc where sc.subject_no=s.subject_no and sc.roll_no=r.roll_no and ea.roll_no=sc.roll_no and ead.subject_no=sc.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and r.degree_code=d.Degree_Code and ed.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and es.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.edate='" + Date + "' and es.ses_sion ='" + session + "' and r.degree_code='" + hdeg + "' and r.batch_year='" + batch + "' and roomno='" + HallNo + "' and es.subject_no  ='" + subno + "' and r.Reg_No in ('" + AttSpread.Sheets[0].Cells[u - 1, 7].Tag.ToString() + "') order by es.seat_no";
                                dsdisplay = d2.select_method_wo_parameter(query, "text");
                                PdfTextArea ptc;
                                if (chkheadimage.Checked == true)
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                                        mypdfpage.Add(LogoImage, 18, 18, 480);
                                    }
                                    coltop = 30;
                                }
                                else
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 35, 25, 700);
                                    }
                                    ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                    mypdfpage.Add(ptc);
                                    coltop = coltop + 15;
                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affilitied);
                                    mypdfpage.Add(ptc);
                                    coltop = coltop + 15;
                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
                                    mypdfpage.Add(ptc);
                                }
                                coltop = coltop + 15;
                                ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE FOR THE END OF SEMESTER EXAMINATIONS" + "-" + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text + "");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 10;
                                ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                mypdfpage.Add(ptc);
                                Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, dsdisplay.Tables[0].Rows.Count + 1, 5, tablepadding);
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetContent("S.No");
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetFont(Fontbold);
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetContent("Register Number");
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetFont(Fontbold);
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetContent("Name of the Candidate");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetFont(Fontbold);
                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 3).SetContent("Answer Booklet No");
                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 3).SetFont(Fontbold);
                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 4).SetContent("Signature of Candidate");
                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 4).SetFont(Fontbold);
                                table1.VisibleHeaders = false;
                                table1.Columns[0].SetWidth(20);
                                table1.Columns[1].SetWidth(40);
                                table1.Columns[2].SetWidth(80);
                                table1.Columns[3].SetWidth(50);
                                table1.Columns[4].SetWidth(60);
                                if (dsdisplay.Tables.Count > 0 && dsdisplay.Tables[0].Rows.Count > 0)
                                {
                                    DataTable dtBundleList = new DataTable();
                                    dtBundleList = dsdisplay.Tables[0].DefaultView.ToTable(true, "deptname", "semester", "subject_code", "subjectname", "bundle_no");
                                    for (ji = 0; ji < dtBundleList.Rows.Count; ji++)
                                    {
                                        string deptname = Convert.ToString(dtBundleList.Rows[ji]["deptname"]).Trim();
                                        string semester = Convert.ToString(dtBundleList.Rows[ji]["semester"]).Trim();
                                        string sub_code = Convert.ToString(dtBundleList.Rows[ji]["subject_code"]).Trim();
                                        string sub_name = Convert.ToString(dtBundleList.Rows[ji]["subjectname"]).Trim();
                                        string bundleNo = Convert.ToString(dtBundleList.Rows[ji]["bundle_no"]).Trim();
                                        string[] sub = sub_name.Split('-');
                                        string subname = sub[1].ToString();
                                        //ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                        //                                             new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
                                        //mypdfpage.Add(ptc);
                                        //ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                        //                                      new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                        //mypdfpage.Add(ptc);
                                        //ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                        //                                      new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, deptname);
                                        //mypdfpage.Add(ptc);
                                        coltop = coltop + 35;
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, ":  " + deptname);
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + semester);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 25;
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + sub_code);
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Exam /Session");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + ddlfrmdate.SelectedItem.Text + "/" + session);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 25;
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 140, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ":  ");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 150, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, subname);
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Hall No\t\t/\t\tBundle No");//"Room No / Bundle No"
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + hall + "\t\t/\t\t" + bundleNo);//":  " + hall + " / " + bndlee
                                        mypdfpage.Add(ptc);
                                        for (ji = 1; ji <= dsdisplay.Tables[0].Rows.Count; ji++)
                                        {
                                            string regno = dsdisplay.Tables[0].Rows[ji - 1]["Reg_No"].ToString();
                                            string name = dsdisplay.Tables[0].Rows[ji - 1]["Stud_Name"].ToString();
                                            string roomno = dsdisplay.Tables[0].Rows[ji - 1]["roomno"].ToString();
                                            string seatno = dsdisplay.Tables[0].Rows[ji - 1]["seat_no"].ToString();
                                            table1.Cell(g, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(g, 0).SetContent(g.ToString());
                                            table1.Cell(g, 0).SetFont(Fontnormal);
                                            table1.Cell(g, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(g, 1).SetContent(regno.ToString());
                                            table1.Cell(g, 1).SetFont(Fontnormal);
                                            table1.Cell(g, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(g, 2).SetContent(name.ToString());
                                            table1.Cell(g, 2).SetFont(Fontnormal);
                                            g = g + 1;
                                        }
                                        if (!CheckBox1.Checked)//raj
                                        {
                                            coltop = 710;
                                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Certified that the following particulars have been verified");
                                            mypdfpage.Add(ptc);
                                            coltop = coltop + 20;
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "1.The Register No. in the attendance sheet with that in the hall ticket.");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 395, coltop - 20, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Present");
                                            mypdfpage.Add(ptc);
                                            coltop = coltop + 12;
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "2.The identification of the candidate with the photo pasted in the hall ticket");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 395, 735, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Absent");
                                            mypdfpage.Add(ptc);
                                            coltop = coltop + 12;
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "3.The answer book number entered in the attendance sheet by the candidate");
                                            mypdfpage.Add(ptc);
                                            coltop = coltop + 40;
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 245, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
                                            mypdfpage.Add(ptc);

                                            PdfArea pa8 = new PdfArea(mydocument, 20, 700, 360, 125);
                                            PdfRectangle pr8 = new PdfRectangle(mydocument, pa8, Color.Black);
                                            mypdfpage.Add(pr8);
                                            PdfArea pa9 = new PdfArea(mydocument, 470, 700, 60, 25);
                                            PdfRectangle pr9 = new PdfRectangle(mydocument, pa9, Color.Black);
                                            mypdfpage.Add(pr9);
                                            PdfArea pa6 = new PdfArea(mydocument, 470, 725, 60, 25);
                                            PdfRectangle pr6 = new PdfRectangle(mydocument, pa6, Color.Black);
                                            mypdfpage.Add(pr6);

                                        }
                                        else
                                        {
                                            coltop = 710;
                                            coltop = coltop + 95;
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 245, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 395, coltop - 30, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Present");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 395, coltop - 5, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Absent");
                                            mypdfpage.Add(ptc);
                                            //PdfArea pa8 = new PdfArea(mydocument, 20, 700, 360, 125);
                                            //PdfRectangle pr8 = new PdfRectangle(mydocument, pa8, Color.Black);
                                            //mypdfpage.Add(pr8);
                                            PdfArea pa9 = new PdfArea(mydocument, 490, 765, 60, 25);
                                            PdfRectangle pr9 = new PdfRectangle(mydocument, pa9, Color.Black);
                                            mypdfpage.Add(pr9);
                                            PdfArea pa6 = new PdfArea(mydocument, 490, 790, 60, 25);
                                            PdfRectangle pr6 = new PdfRectangle(mydocument, pa6, Color.Black);
                                            mypdfpage.Add(pr6);
                                        }
                                        Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, 750));
                                        mypdfpage.Add(newpdftabpage1);
                                        mypdfpage.Add(pr1);

                                        g = 1;
                                        if (yq >= 180)
                                        {
                                            mypdfpage.SaveToDocument();
                                            mypdfpage = mydocument.NewPage();
                                            yq = 180;
                                        }
                                    }
                                    string appPath = HttpContext.Current.Server.MapPath("~");
                                    if (appPath != "")
                                    {
                                        string szPath = appPath + "/Report/";
                                        string szFile = "ExamAttendanceSheet" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                                        mydocument.SaveToFile(szPath + szFile);
                                        Response.ClearHeaders();
                                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                        Response.ContentType = "application/pdf";
                                        Response.WriteFile(szPath + szFile);
                                    }
                                }
                                else
                                {
                                    lblerror1.Visible = true;
                                    lblerror1.Text = "No Records Found";
                                }
                            }
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                }
                if (chkgenflag == false)
                {
                    lblerror1.Visible = true;
                    lblerror1.Text = "Please Select Any One Record";
                }
            }
            else
            {
                ArrayList arr_subjectunique = new ArrayList();
                if (sml.Trim() != "0")
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            collname = ds.Tables[0].Rows[0]["collname"].ToString();
                            affilitied = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                            district = ds.Tables[0].Rows[0]["district"].ToString();
                            pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                            string sessiond1 = string.Empty;
                            if (ddlsession.SelectedItem.Text == "Both")
                            {
                                sessiond1 = string.Empty;
                            }
                            else
                            {
                                sessiond1 = "  and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
                            }
                            string[] aff = affilitied.Split(',');
                            affilitied = aff[0].ToString();
                            string datess = ddlfrmdate.SelectedItem.Text;
                            string[] fromdatespit99 = datess.ToString().Split('-');
                            datess = fromdatespit99[2] + '-' + fromdatespit99[1] + '-' + fromdatespit99[0];
                            //string overall = "select distinct  top 40 es.roomno,COUNT(1) as strength,es.ses_sion,es.edate  from registration r,subjectchooser sc,exam_seating as es where sc.roll_no=r.roll_no  and exam_flag<>'Debar' and es.regno=r.Reg_No and es.subject_no=sc.subject_no " + sessiond1 + "  group by es.roomno,es.ses_sion,es.edate  ";
                            string overall = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate  from registration r,subjectchooser sc,exam_seating as es where sc.roll_no=r.roll_no  and exam_flag<>'Debar' and es.regno=r.Reg_No and es.subject_no=sc.subject_no and es.edate='" + datess + "' " + sessiond1 + " group by es.roomno,es.ses_sion,es.edate  ";
                            //string overall = "select distinct es.roomno ,c.Course_Name,es.edate,s.subject_no,de.Dept_Name,d.Degree_Code,s.subject_name,s.subject_code,d.Acronym,es.edate,es.ses_sion from exmtt e,exmtt_det et,exam_seating es,course c,Degree d,Department de,subject s where e.exam_code=et.exam_code and et.subject_no=es.subject_no and  e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and   d.Dept_Code=de.Dept_Code and es.subject_no=s.subject_no    and et.subject_no=s.subject_no and e.Exam_year='" + ddlYear.SelectedItem.Text + "'   and e.Exam_month='" + ddlMonth.SelectedValue + "' and es.edate='" + datess + "' and es.ses_sion='" + ddlsession.SelectedItem.Text + "'";
                            DataSet dsoverall = new DataSet();
                            dsoverall = d2.select_method_wo_parameter(overall, "text");
                            int u = 0;
                            int startrow = 0;
                            int tablerowscount = 0;
                            for (int sew = 0; sew < AttSpread.Sheets[0].Rows.Count; sew++)
                            {
                                isval = Convert.ToInt16(AttSpread.Sheets[0].Cells[u, 1].Value);
                                u = u + 1;
                                if (isval == 1 && u > 1)
                                {
                                    int we = 1;
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 35, 25, 700);
                                    }
                                    if (dsoverall.Tables.Count > 0 && dsoverall.Tables[0].Rows.Count > 0)
                                    {
                                        coltop = 10;
                                        PdfTextArea ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 15;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affilitied);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 15;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 15;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE FOR THE END OF SEMESTER EXAMINATIONS" + "-" + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text + "");
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 10;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                        mypdfpage.Add(ptc);
                                        //string roomnoco = dsoverall.Tables[0].Rows[u-1]["roomno"].ToString();
                                        string roomnoco = AttSpread.Sheets[0].Cells[u - 1, 2].Text.ToString();
                                        string queryreg = string.Empty;
                                        queryreg = "select distinct  sub.subject_no,r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sc.semester,sub.subject_code,sub.subject_name ,c.Course_Name, (select dept_name from Department where d.dept_code=Dept_Code) as deptname,r.degree_code  from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "' and es.edate='" + datess + "'  and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'  order by es.seat_no";
                                        //  queryreg = "select distinct  top 102 r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sub.subject_code,sub.subject_name   from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "'   and es.ses_sion='" + ddlsession.SelectedItem.Text + "'  order by es.seat_no";
                                        //queryreg = "select distinct  r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sub.subject_code,sub.subject_name   from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "' and es.edate='" + datess + "' and es.ses_sion='" + ddlsession.SelectedItem.Text + "'  order by es.seat_no";
                                        DataSet dschecksubjlist = new DataSet();
                                        dschecksubjlist = d2.select_method_wo_parameter(queryreg, "text");
                                        DataSet dscheck = new DataSet();
                                        //dscheck = d2.select_method_wo_parameter(queryreg, "text");
                                        for (int subjlist = 0; subjlist < dschecksubjlist.Tables[0].Rows.Count; subjlist++)
                                        {
                                            if (!arr_subjectunique.Contains(dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString().Trim().ToLower() + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString().Trim().ToLower()))
                                            {
                                                DataView DVsubjlist = new DataView();
                                                dschecksubjlist.Tables[0].DefaultView.RowFilter = " subject_no='" + dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString() + "'and degree_code='" + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString() + "'";
                                                DVsubjlist = dschecksubjlist.Tables[0].DefaultView;
                                                dscheck.Clear();
                                                dscheck.Tables.Clear();
                                                dscheck.Tables.Add(DVsubjlist.ToTable());
                                                arr_subjectunique.Add(dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString().Trim().ToLower() + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString().Trim().ToLower());
                                                string deptname = dscheck.Tables[0].Rows[0]["Course_Name"].ToString() + " - " + dscheck.Tables[0].Rows[0]["deptname"].ToString();
                                                string sub_code = dscheck.Tables[0].Rows[0]["subject_code"].ToString();
                                                string semester = dscheck.Tables[0].Rows[0]["semester"].ToString();
                                                string subname = dscheck.Tables[0].Rows[0]["subject_name"].ToString();
                                                we = we + 1;
                                                coltop = coltop + 35;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, deptname);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, semester);
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 25;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, sub_code);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Exam /Session");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ddlfrmdate.SelectedItem.Text + "/" + ddlsession.SelectedItem.Text);
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 25;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, subname);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, dsoverall.Tables[0].Rows[sew]["roomno"].ToString());
                                                mypdfpage.Add(ptc);
                                                int tblrocc = 0;
                                                sml = "25";
                                                if (dscheck.Tables.Count > 0 && dscheck.Tables[0].Rows.Count < Convert.ToInt32(sml))
                                                {
                                                    tblrocc = dscheck.Tables[0].Rows.Count;
                                                }
                                                else
                                                {
                                                    tblrocc = Convert.ToInt32(sml);
                                                }
                                                Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, tblrocc + 1, 5, 4);
                                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 0).SetContent("S.No");
                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 0).SetFont(Fontbold);
                                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 1).SetContent("Register Number");
                                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 1).SetFont(Fontbold);
                                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 2).SetContent("Name of the Candidate");
                                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 2).SetFont(Fontbold);
                                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 3).SetContent("Answer Booklet No");
                                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 3).SetFont(Fontbold);
                                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 4).SetContent("Signature of Candidate");
                                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 4).SetFont(Fontbold);
                                                table1.VisibleHeaders = false;
                                                table1.Columns[0].SetWidth(20);
                                                table1.Columns[1].SetWidth(40);
                                                table1.Columns[2].SetWidth(80);
                                                table1.Columns[3].SetWidth(50);
                                                table1.Columns[4].SetWidth(60);
                                                int gwe = 1;
                                                int ast = 0;
                                                tablerowscount = dscheck.Tables[0].Rows.Count;
                                                for (ast = startrow; ast < dscheck.Tables[0].Rows.Count; ast++)
                                                {
                                                    if (ast != 0 && ast % Convert.ToInt32(sml) == 0)
                                                    {
                                                        Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, 650));
                                                        mypdfpage.Add(newpdftabpage1);
                                                        tablerowscount = tablerowscount - 25;
                                                        coltop = 680;
                                                        ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Certified that the following particulars have been verified");
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 30;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "1.The Register No. in the attendance sheet with that in the hall ticket.");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 395, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Present");
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 12;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "2.The identification of the candidate with the photo pasted in the hall ticket");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                     new PdfArea(mydocument, 395, 735, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Absent");
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 12;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "3.The answer book number entered in the attendance sheet by the candidate");
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 75;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 245, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
                                                        mypdfpage.Add(ptc);
                                                        PdfArea pa8 = new PdfArea(mydocument, 20, 700, 360, 125);
                                                        PdfRectangle pr8 = new PdfRectangle(mydocument, pa8, Color.Black);
                                                        mypdfpage.Add(pr8);
                                                        PdfArea pa9 = new PdfArea(mydocument, 470, 700, 60, 25);
                                                        PdfRectangle pr9 = new PdfRectangle(mydocument, pa9, Color.Black);
                                                        mypdfpage.Add(pr9);
                                                        PdfArea pa6 = new PdfArea(mydocument, 470, 725, 60, 25);
                                                        PdfRectangle pr6 = new PdfRectangle(mydocument, pa6, Color.Black);
                                                        mypdfpage.Add(pr6);
                                                        PdfArea tete = new PdfArea(mydocument, 15, 10, 565, 825);
                                                        PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                                        mypdfpage.Add(pr1);
                                                        mypdfpage.SaveToDocument();
                                                        mypdfpage = mydocument.NewPage();
                                                        coltop = 10;
                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                                        {
                                                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                                            mypdfpage.Add(LogoImage, 35, 25, 700);
                                                        }
                                                        ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 15;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affilitied);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 15;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 15;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE FOR THE END OF SEMESTER EXAMINATIONS" + "-" + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text + "");
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 10;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                                        mypdfpage.Add(ptc);
                                                        we = we + 1;
                                                        coltop = coltop + 35;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                     new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, deptname);
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, semester);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 25;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, sub_code);
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Exam /Session");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ddlfrmdate.SelectedItem.Text + "/" + ddlsession.SelectedItem.Text);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 25;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, subname);
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, dsoverall.Tables[0].Rows[sew]["roomno"].ToString());
                                                        mypdfpage.Add(ptc);
                                                        if (tablerowscount > 25)
                                                        {
                                                            tblrocc = 25;
                                                        }
                                                        else
                                                        {
                                                            tblrocc = tablerowscount;
                                                        }
                                                        table1 = mydocument.NewTable(Fontbold, tblrocc + 1, 5, 4);
                                                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 0).SetContent("S.No");
                                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 0).SetFont(Fontbold);
                                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 1).SetContent("Register Number");
                                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 1).SetFont(Fontbold);
                                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 2).SetContent("Name of the Candidate");
                                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 2).SetFont(Fontbold);
                                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 3).SetContent("Answer Booklet No");
                                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 3).SetFont(Fontbold);
                                                        table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 4).SetContent("Signature of Candidate");
                                                        table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 4).SetFont(Fontbold);
                                                        table1.VisibleHeaders = false;
                                                        table1.Columns[0].SetWidth(20);
                                                        table1.Columns[1].SetWidth(40);
                                                        table1.Columns[2].SetWidth(80);
                                                        table1.Columns[3].SetWidth(50);
                                                        table1.Columns[4].SetWidth(60);
                                                        gwe = 1;
                                                    }
                                                    string regno = dscheck.Tables[0].Rows[ast]["Reg_No"].ToString();
                                                    string name = dscheck.Tables[0].Rows[ast]["Stud_Name"].ToString();
                                                    string seat = dscheck.Tables[0].Rows[ast]["seat_no"].ToString();
                                                    string hallno = dscheck.Tables[0].Rows[ast]["roomno"].ToString();
                                                    table1.Cell(gwe, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table1.Cell(gwe, 0).SetContent(gwe.ToString());
                                                    table1.Cell(gwe, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table1.Cell(gwe, 1).SetContent(regno.ToString());
                                                    table1.Cell(gwe, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(gwe, 2).SetContent(name.ToString());
                                                    gwe = gwe + 1;
                                                }
                                                int h = 650;
                                                Gios.Pdf.PdfTablePage newpdftabpage11 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, h));
                                                mypdfpage.Add(newpdftabpage11);
                                                coltop = 680;
                                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Certified that the following particulars have been verified");
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 30;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "1.The Register No. in the attendance sheet with that in the hall ticket.");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 395, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Present");
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 12;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "2.The identification of the candidate with the photo pasted in the hall ticket");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 395, 735, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Absent");
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 12;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "3.The answer book number entered in the attendance sheet by the candidate");
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 75;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 245, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
                                                mypdfpage.Add(ptc);
                                                PdfArea pa81 = new PdfArea(mydocument, 20, 700, 360, 125);
                                                PdfRectangle pr81 = new PdfRectangle(mydocument, pa81, Color.Black);
                                                mypdfpage.Add(pr81);
                                                PdfArea pa91 = new PdfArea(mydocument, 470, 700, 60, 25);
                                                PdfRectangle pr91 = new PdfRectangle(mydocument, pa91, Color.Black);
                                                mypdfpage.Add(pr91);
                                                PdfArea pa61 = new PdfArea(mydocument, 470, 725, 60, 25);
                                                PdfRectangle pr61 = new PdfRectangle(mydocument, pa61, Color.Black);
                                                mypdfpage.Add(pr61);
                                                PdfArea tete1 = new PdfArea(mydocument, 15, 10, 565, 825);
                                                PdfRectangle pr11 = new PdfRectangle(mydocument, tete1, Color.Black);
                                                mypdfpage.Add(pr11);
                                                g = 1;
                                                if (h >= 500)
                                                {
                                                    coltop = 10;
                                                    ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                                    mypdfpage.Add(ptc);
                                                    coltop = coltop + 15;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affilitied);
                                                    mypdfpage.Add(ptc);
                                                    coltop = coltop + 15;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
                                                    mypdfpage.Add(ptc);
                                                    coltop = coltop + 15;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE FOR THE END OF SEMESTER EXAMINATIONS" + "-" + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text + "");
                                                    mypdfpage.Add(ptc);
                                                    coltop = coltop + 10;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                                    mypdfpage.Add(ptc);
                                                    mypdfpage.SaveToDocument();
                                                    mypdfpage = mydocument.NewPage();
                                                    //yq = 190;
                                                }
                                            }
                                        }
                                        string appPath = HttpContext.Current.Server.MapPath("~");
                                        if (appPath != "")
                                        {
                                            string szPath = appPath + "/Report/";
                                            string szFile = "ExamAttendanceSheet" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                                            mydocument.SaveToFile(szPath + szFile);
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
                }
                else
                {
                    lblnorecc.Visible = true;
                    lblnorecc.Text = "Please Allot Bundle No And Then Proceed";
                }
            }
        }
        catch (Exception ex)
        {
            lblnorecc.Text = ex.ToString();
            lblnorecc.Visible = true;
        }
    }

    protected void btndisplay_click(object sender, EventArgs e)
    {
        try
        {
            Display();
        }
        catch (Exception ex)
        {
            lblnorecc.Text = ex.ToString();
            lblnorecc.Visible = true;
        }
    }

    protected void btnPhaseSheet_click(object sender, EventArgs e)
    {
        try
        {
            DisplayPhaseSheet();
        }
        catch (Exception ex)
        {
            lblnorecc.Text = ex.ToString();
            lblnorecc.Visible = true;
        }
    }

    public void DisplayPhaseSheet()
    {
        try
        {
            divDummyNoSheets.Visible = false;
            lblDispErr.Visible = false;
            lblDispErr.Text = string.Empty;
            AttSpread.SaveChanges();
            int g = 0;
            string collgr = string.Empty;
            string affilitied = string.Empty;
            string collname = string.Empty;
            string pincode = string.Empty;
            string district = string.Empty;
            string Date = string.Empty;
            int mm = 0;
            int y = 0;
            string HallNo = string.Empty;
            string session = string.Empty;
            string hdeg = "", hroll = "", bndlee = string.Empty;
            string batch = string.Empty;
            string subno = string.Empty;
            string hall = string.Empty;
            DataSet dsdisplay = new DataSet();
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);
            Font fontBundleNo = new Font("Book Antique", 22, FontStyle.Bold);
            Boolean chkgenflag = false;
            DateTime dt = new DateTime();
            int coltop = 10;
            coltop = coltop + 5;
            int coltop1 = coltop;
            int finctop = coltop;
            int yq = 180;
            string strquery = string.Empty;
            int isval = 0;
            int ji = 0;
            int tablepadding = 10;
            strquery = "Select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
            string sml = d2.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
            if (sml.Trim() != "" && sml.Trim() != "0")
            {
                if (Convert.ToInt32(sml) > 15)
                {
                    tablepadding = 3;
                }
                else
                {
                    tablepadding = 10;
                }
                {
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        collname = ds.Tables[0].Rows[0]["collname"].ToString();
                        affilitied = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        district = ds.Tables[0].Rows[0]["district"].ToString();
                        pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                        string[] aff = affilitied.Split(',');
                        affilitied = aff[0].ToString();
                        boundvl.Clear();
                        HasSession.Clear();
                        Hasdegree.Clear();
                        Hashdenm.Clear();
                        Hashhall.Clear();
                        HashDate.Clear();
                        Hasroll.Clear();
                        hassubno.Clear();
                        hasbatch.Clear();
                        int u = 0;
                        for (mm = 0; mm < AttSpread.Sheets[0].Rows.Count; mm++)
                        {
                            isval = Convert.ToInt32(AttSpread.Sheets[0].Cells[u, 1].Value);
                            u = u + 1;
                            if (isval == 1 && u > 1)
                            {
                                y = y + 1;
                                chkgenflag = true;
                                lblerr1.Visible = false;
                                lblerr1.Text = string.Empty;
                                coltop = 10;
                                hall = AttSpread.Sheets[0].Cells[u - 1, 2].Text.ToString();
                                dt = Convert.ToDateTime(AttSpread.Sheets[0].Cells[u - 1, 2].Note.ToString());
                                PdfArea tete = new PdfArea(mydocument, 15, 10, 825, 565);
                                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                Date = AttSpread.Sheets[0].Cells[u - 1, 2].Note.ToString();
                                session = AttSpread.Sheets[0].Cells[u - 1, 4].Note.ToString();
                                HallNo = AttSpread.Sheets[0].Cells[u - 1, 2].Text.ToString();
                                hdeg = AttSpread.Sheets[0].Cells[u - 1, 8].Text.ToString();
                                hroll = AttSpread.Sheets[0].Cells[u - 1, 6].Text.ToString();
                                //hroll = "'" + hroll + "'  and  '" + AttSpread.Sheets[0].Cells[u - 1, 7].Text.ToString() + "'";
                                bndlee = AttSpread.Sheets[0].Cells[u - 1, 5].Text.ToString();
                                batch = AttSpread.Sheets[0].Cells[u - 1, 3].Note.ToString();
                                subno = AttSpread.Sheets[0].Cells[u - 1, 0].Note.ToString();
                                string[] dummy_date_split = Date.Split(' ');
                                string[] dsplit = dummy_date_split[0].Split('/');
                                Date = dsplit[2].ToString() + "-" + dsplit[0].ToString() + "-" + dsplit[1].ToString();
                                collgr = Session["collegecode"].ToString();
                                string[] rollNoList = Convert.ToString(AttSpread.Sheets[0].Cells[u - 1, 7].Tag).Trim().Split(',');
                                string query = "select distinct r.Reg_No,r.Stud_Name,r.Stud_Type,r.current_semester,s.subject_code,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym)  as Grade,sc.semester,(s.subject_code +'-'+ s.subject_name) as subjectname ,(c.Course_Name +'-'+ de.dept_name) as deptname,es.bundle_no from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject s,Degree d,course c,Department de,subjectchooser sc where sc.subject_no=s.subject_no and sc.roll_no=r.roll_no and ea.roll_no=sc.roll_no and ead.subject_no=sc.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and r.degree_code=d.Degree_Code and ed.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and es.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.edate='" + Date + "' and es.ses_sion ='" + session + "' and r.degree_code='" + hdeg + "' and r.batch_year='" + batch + "' and roomno='" + HallNo + "' and es.subject_no  ='" + subno + "' and r.Reg_No in ('" + AttSpread.Sheets[0].Cells[u - 1, 7].Tag.ToString() + "') order by es.seat_no ";
                                dsdisplay = d2.select_method_wo_parameter(query, "text");
                                PdfTextArea ptc;
                                ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 0, coltop, 820, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + "," + district + "-" + pincode);
                                mypdfpage.Add(ptc);
                                coltop = coltop + 25;
                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 820, 50), System.Drawing.ContentAlignment.MiddleCenter, "Name of the Examinations : END SEMESTER EXAMINATIONS" + "-" + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text + "");
                                mypdfpage.Add(ptc);
                                int rows = 6;
                                rows = (dsdisplay.Tables[0].Rows.Count / 5);
                                if (dsdisplay.Tables[0].Rows.Count % 5 > 0) rows++;
                                rows = 6;
                                Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, rows, 10, 10);
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table1.VisibleHeaders = false;
                                table1.Columns[0].SetWidth(70);
                                table1.Columns[1].SetWidth(70);
                                table1.Columns[2].SetWidth(70);
                                table1.Columns[3].SetWidth(70);
                                table1.Columns[4].SetWidth(70);
                                table1.Columns[5].SetWidth(70);
                                table1.Columns[6].SetWidth(70);
                                table1.Columns[7].SetWidth(70);
                                table1.Columns[8].SetWidth(70);
                                table1.Columns[9].SetWidth(70);
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetContent("REG.No.");
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetContent("P / A");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetContent("REG.No.");
                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 3).SetContent("P / A");
                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 4).SetContent("REG.No.");
                                table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 5).SetContent("P / A");
                                table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 6).SetContent("REG.No.");
                                table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 7).SetContent("P / A");
                                table1.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 8).SetContent("REG.No.");
                                table1.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 9).SetContent("P / A");
                                if (dsdisplay.Tables.Count > 0 && dsdisplay.Tables[0].Rows.Count > 0)
                                {
                                    string deptname = dsdisplay.Tables[0].Rows[0]["deptname"].ToString();
                                    string semester = dsdisplay.Tables[0].Rows[0]["semester"].ToString();
                                    string sub_code = dsdisplay.Tables[0].Rows[0]["subject_code"].ToString();
                                    string sub_name = dsdisplay.Tables[0].Rows[0]["subjectname"].ToString();
                                    string bundleNo = Convert.ToString(dsdisplay.Tables[0].Rows[0]["bundle_no"]).Trim();
                                    string[] sub = sub_name.Split('-');
                                    string subname = sub[1].ToString();
                                    coltop = coltop + 45;
                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No");
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, ":  " + hall);
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 260, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Bundle No\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t:\t\t");
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(fontBundleNo, System.Drawing.Color.Black, new PdfArea(mydocument, 370, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "  " + bundleNo);
                                    mypdfpage.Add(ptc);
                                    coltop = coltop + 25;
                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date");
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + ddlfrmdate.SelectedItem.Text);
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 260, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Session");
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 370, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + ddlsession.SelectedItem.Text);
                                    mypdfpage.Add(ptc);
                                    coltop = coltop + 25;
                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ":  ");
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 150, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, sub_code);
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 260, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Title of the paper");
                                    mypdfpage.Add(ptc);
                                    ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 370, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + subname);
                                    mypdfpage.Add(ptc);
                                    int rOw = 0;
                                    for (int cOl = 0; cOl < 10; cOl += 2)
                                    {
                                        for (int rro = 1; rro < 6; rro++)
                                        {
                                            if (dsdisplay.Tables.Count > 0 && dsdisplay.Tables[0].Rows.Count > rOw)
                                            {
                                                string regno = dsdisplay.Tables[0].Rows[rOw]["Reg_No"].ToString();
                                                string name = dsdisplay.Tables[0].Rows[rOw]["Stud_Name"].ToString();
                                                string roomno = dsdisplay.Tables[0].Rows[rOw]["roomno"].ToString();
                                                string seatno = dsdisplay.Tables[0].Rows[rOw]["seat_no"].ToString();
                                                table1.Cell(rro, cOl).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(rro, cOl).SetContent(regno.ToString());
                                                table1.Cell(rro, cOl).SetFont(Fontnormal);
                                                rOw++;
                                            }
                                            else
                                            {
                                                table1.Cell(rro, cOl).SetContent("\n");
                                            }
                                        }
                                    }
                                    coltop = 500;

                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 35, (coltop - 130), 600, 20), System.Drawing.ContentAlignment.TopLeft, "PLEASE NOTE:");
                                    mypdfpage.Add(ptc);

                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 45, (coltop - 130), 600, 50), System.Drawing.ContentAlignment.TopLeft, "\nI) THIS PACKET IS INTENDED TO HOLD 25 ANSWER BOOKS ONLY\nII) MARK 'P' FOR PRESENT AND 'AAA' FOR ABSENT IN THE BOX PROVIDED");
                                    mypdfpage.Add(ptc);

                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 500, (coltop - 150), 600, 50), System.Drawing.ContentAlignment.TopLeft, "TOTAL NO. OF ANSWER BOOKS IN THE PACKET ");
                                    mypdfpage.Add(ptc);

                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 545, (coltop - 60), 250, 50), System.Drawing.ContentAlignment.MiddleCenter, "PACKED AND SEALED IN MY PRESENCE \n\n\n\nSIGNATURE OF CHEIF SUPDT");
                                    mypdfpage.Add(ptc);

                                    PdfArea tete3 = new PdfArea(mydocument, 745, (coltop - 160), 60, 30);
                                    PdfRectangle pr3 = new PdfRectangle(mydocument, tete3, Color.Black);
                                    mypdfpage.Add(pr3);

                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 245, (coltop - 40), 300, 50), System.Drawing.ContentAlignment.MiddleCenter, "Signature of Invigilator\n\n(Name in Block Letters)");
                                    mypdfpage.Add(ptc);

                                    Gios.Pdf.PdfTable table2 = mydocument.NewTable(Fontbold, 3, 3, 5);
                                    table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table2.VisibleHeaders = false;
                                    table2.Columns[0].SetWidth(70);
                                    table2.Columns[1].SetWidth(150);
                                    table2.Columns[2].SetWidth(70);
                                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 1).SetContent("SIGNATURE");
                                    table2.Cell(0, 1).SetFont(Fontbold);
                                    table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 2).SetContent("DATE");
                                    table2.Cell(0, 2).SetFont(Fontbold);
                                    table2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(1, 0).SetContent("EXTERNAL");
                                    table2.Cell(1, 0).SetFont(Fontbold);
                                    table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(2, 0).SetContent("INTERNAL");
                                    table2.Cell(2, 0).SetFont(Fontbold);
                                    table2.Rows[1].SetCellPadding(10);
                                    table2.Rows[2].SetCellPadding(10);

                                    Gios.Pdf.PdfTablePage newpdftabpage0 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, (coltop - 30), 230, 80));
                                    mypdfpage.Add(newpdftabpage0);
                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 35, (coltop - 65), 250, 50), System.Drawing.ContentAlignment.MiddleLeft, "VALUATION");
                                    mypdfpage.Add(ptc);

                                    ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 150, (coltop + 50), 200, 50), System.Drawing.ContentAlignment.MiddleRight, "CAMP OFFICER");
                                    mypdfpage.Add(ptc);

                                    Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, (yq - 25), 810, 560));
                                    mypdfpage.Add(newpdftabpage1);

                                    coltop += 40;
                                    g = 1;
                                    if (yq >= 180)
                                    {
                                        mypdfpage.SaveToDocument();
                                        mypdfpage = mydocument.NewPage();
                                        yq = 180;
                                    }
                                    string appPath = HttpContext.Current.Server.MapPath("~");
                                    if (appPath != "")
                                    {
                                        string szPath = appPath + "/Report/";
                                        string szFile = "PhasingSheet" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                                        mydocument.SaveToFile(szPath + szFile);
                                        Response.ClearHeaders();
                                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                        Response.ContentType = "application/pdf";
                                        Response.WriteFile(szPath + szFile);
                                    }
                                }
                                else
                                {
                                    lblerror1.Visible = true;
                                    lblerror1.Text = "No Records Found";
                                }
                            }
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                }
                if (chkgenflag == false)
                {
                    lblerror1.Visible = true;
                    lblerror1.Text = "Please Select Any One Record";
                }
            }
            else
            {
                ArrayList arr_subjectunique = new ArrayList();
                if (sml.Trim() != "0")
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            collname = ds.Tables[0].Rows[0]["collname"].ToString();
                            affilitied = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                            district = ds.Tables[0].Rows[0]["district"].ToString();
                            pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                            string sessiond1 = string.Empty;
                            if (ddlsession.SelectedItem.Text == "Both")
                            {
                                sessiond1 = string.Empty;
                            }
                            else
                            {
                                sessiond1 = "  and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
                            }
                            string[] aff = affilitied.Split(',');
                            affilitied = aff[0].ToString();
                            string datess = ddlfrmdate.SelectedItem.Text;
                            string[] fromdatespit99 = datess.ToString().Split('-');
                            datess = fromdatespit99[2] + '-' + fromdatespit99[1] + '-' + fromdatespit99[0];
                            //string overall = "select distinct  top 40 es.roomno,COUNT(1) as strength,es.ses_sion,es.edate  from registration r,subjectchooser sc,exam_seating as es where sc.roll_no=r.roll_no  and exam_flag<>'Debar' and es.regno=r.Reg_No and es.subject_no=sc.subject_no " + sessiond1 + "  group by es.roomno,es.ses_sion,es.edate  ";
                            string overall = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate  from registration r,subjectchooser sc,exam_seating as es where sc.roll_no=r.roll_no  and exam_flag<>'Debar' and es.regno=r.Reg_No and es.subject_no=sc.subject_no and es.edate='" + datess + "' " + sessiond1 + "  group by es.roomno,es.ses_sion,es.edate  ";
                            //string overall = "select distinct es.roomno ,c.Course_Name,es.edate,s.subject_no,de.Dept_Name,d.Degree_Code,s.subject_name,s.subject_code,d.Acronym,es.edate,es.ses_sion from exmtt e,exmtt_det et,exam_seating es,course c,Degree d,Department de,subject s where e.exam_code=et.exam_code and et.subject_no=es.subject_no and  e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and   d.Dept_Code=de.Dept_Code and es.subject_no=s.subject_no    and et.subject_no=s.subject_no and e.Exam_year='" + ddlYear.SelectedItem.Text + "'   and e.Exam_month='" + ddlMonth.SelectedValue + "' and es.edate='" + datess + "' and es.ses_sion='" + ddlsession.SelectedItem.Text + "'";
                            DataSet dsoverall = new DataSet();
                            dsoverall = d2.select_method_wo_parameter(overall, "text");
                            int u = 0;
                            int startrow = 0;
                            int tablerowscount = 0;
                            for (int sew = 0; sew < AttSpread.Sheets[0].Rows.Count; sew++)
                            {
                                isval = Convert.ToInt16(AttSpread.Sheets[0].Cells[u, 1].Value);
                                u = u + 1;
                                if (isval == 1)
                                {
                                    int we = 1;
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 35, 25, 700);
                                    }
                                    if (dsoverall.Tables.Count > 0 && dsoverall.Tables[0].Rows.Count > 0)
                                    {
                                        coltop = 10;
                                        PdfTextArea ptc = new PdfTextArea(head, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 15;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affilitied);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 15;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 15;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE FOR THE END OF SEMESTER EXAMINATIONS" + "-" + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text + "");
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 10;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                        mypdfpage.Add(ptc);
                                        string roomnoco = dsoverall.Tables[0].Rows[sew]["roomno"].ToString();
                                        string queryreg = string.Empty;
                                        queryreg = "select distinct  sub.subject_no,r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sc.semester,sub.subject_code,sub.subject_name ,c.Course_Name, (select dept_name from Department where d.dept_code=Dept_Code) as deptname,r.degree_code  from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "' and es.edate='" + datess + "'  and es.ses_sion like'" + ddlsession.SelectedItem.Text + "%'  order by es.seat_no";
                                        //  queryreg = "select distinct  top 102 r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sub.subject_code,sub.subject_name   from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "'   and es.ses_sion='" + ddlsession.SelectedItem.Text + "'  order by es.seat_no";
                                        //queryreg = "select distinct  r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sub.subject_code,sub.subject_name   from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "' and es.edate='" + datess + "' and es.ses_sion='" + ddlsession.SelectedItem.Text + "'  order by es.seat_no";
                                        DataSet dschecksubjlist = new DataSet();
                                        dschecksubjlist = d2.select_method_wo_parameter(queryreg, "text");
                                        DataSet dscheck = new DataSet();
                                        //dscheck = d2.select_method_wo_parameter(queryreg, "text");
                                        for (int subjlist = 0; subjlist < dschecksubjlist.Tables[0].Rows.Count; subjlist++)
                                        {
                                            if (!arr_subjectunique.Contains(dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString().Trim().ToLower() + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString().Trim().ToLower()))
                                            {
                                                DataView DVsubjlist = new DataView();
                                                dschecksubjlist.Tables[0].DefaultView.RowFilter = " subject_no='" + dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString() + "'and degree_code='" + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString() + "'";
                                                DVsubjlist = dschecksubjlist.Tables[0].DefaultView;
                                                dscheck.Clear();
                                                dscheck.Tables.Clear();
                                                dscheck.Tables.Add(DVsubjlist.ToTable());
                                                arr_subjectunique.Add(dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString().Trim().ToLower() + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString().Trim().ToLower());
                                                string deptname = dscheck.Tables[0].Rows[0]["Course_Name"].ToString() + " - " + dscheck.Tables[0].Rows[0]["deptname"].ToString();
                                                string sub_code = dscheck.Tables[0].Rows[0]["subject_code"].ToString();
                                                string semester = dscheck.Tables[0].Rows[0]["semester"].ToString();
                                                string subname = dscheck.Tables[0].Rows[0]["subject_name"].ToString();
                                                we = we + 1;
                                                coltop = coltop + 35;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, deptname);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, semester);
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 25;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, sub_code);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Exam /Session");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ddlfrmdate.SelectedItem.Text + "/" + ddlsession.SelectedItem.Text);
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 25;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, subname);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, dsoverall.Tables[0].Rows[sew]["roomno"].ToString());
                                                mypdfpage.Add(ptc);
                                                int tblrocc = 0;
                                                sml = "25";
                                                if (dscheck.Tables.Count > 0 && dscheck.Tables[0].Rows.Count < Convert.ToInt32(sml))
                                                {
                                                    tblrocc = dscheck.Tables[0].Rows.Count;
                                                }
                                                else
                                                {
                                                    tblrocc = Convert.ToInt32(sml);
                                                }

                                                Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, tblrocc + 1, 5, 4);
                                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table1.VisibleHeaders = false;

                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 0).SetContent("S.No");
                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 0).SetFont(Fontbold);

                                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 1).SetContent("Register Number");
                                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 1).SetFont(Fontbold);

                                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 2).SetContent("Name of the Candidate");
                                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 2).SetFont(Fontbold);

                                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 3).SetContent("Answer Booklet No");
                                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 3).SetFont(Fontbold);

                                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 4).SetContent("Signature of Candidate");
                                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 4).SetFont(Fontbold);

                                                table1.Columns[0].SetWidth(20);
                                                table1.Columns[1].SetWidth(40);
                                                table1.Columns[2].SetWidth(80);
                                                table1.Columns[3].SetWidth(50);
                                                table1.Columns[4].SetWidth(60);

                                                int gwe = 1;
                                                int ast = 0;
                                                tablerowscount = dscheck.Tables[0].Rows.Count;
                                                for (ast = startrow; ast < dscheck.Tables[0].Rows.Count; ast++)
                                                {
                                                    if (ast != 0 && ast % Convert.ToInt32(sml) == 0)
                                                    {
                                                        Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, 650));
                                                        mypdfpage.Add(newpdftabpage1);
                                                        tablerowscount = tablerowscount - 25;

                                                        coltop = 680;
                                                        ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Certified that the following particulars have been verified");
                                                        mypdfpage.Add(ptc);

                                                        coltop = coltop + 30;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "1.The Register No. in the attendance sheet with that in the hall ticket.");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 395, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Present");
                                                        mypdfpage.Add(ptc);

                                                        coltop = coltop + 12;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "2.The identification of the candidate with the photo pasted in the hall ticket");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 395, 735, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Absent");
                                                        mypdfpage.Add(ptc);

                                                        coltop = coltop + 12;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "3.The answer book number entered in the attendance sheet by the candidate");
                                                        mypdfpage.Add(ptc);

                                                        coltop = coltop + 75;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
                                                        mypdfpage.Add(ptc);

                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 245, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
                                                        mypdfpage.Add(ptc);

                                                        PdfArea pa8 = new PdfArea(mydocument, 20, 700, 360, 125);
                                                        PdfRectangle pr8 = new PdfRectangle(mydocument, pa8, Color.Black);
                                                        mypdfpage.Add(pr8);

                                                        PdfArea pa9 = new PdfArea(mydocument, 470, 700, 60, 25);
                                                        PdfRectangle pr9 = new PdfRectangle(mydocument, pa9, Color.Black);
                                                        mypdfpage.Add(pr9);

                                                        PdfArea pa6 = new PdfArea(mydocument, 470, 725, 60, 25);
                                                        PdfRectangle pr6 = new PdfRectangle(mydocument, pa6, Color.Black);
                                                        mypdfpage.Add(pr6);

                                                        PdfArea tete = new PdfArea(mydocument, 15, 10, 565, 825);
                                                        PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                                        mypdfpage.Add(pr1);

                                                        mypdfpage.SaveToDocument();
                                                        mypdfpage = mydocument.NewPage();
                                                        coltop = 10;
                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                                        {
                                                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                                            mypdfpage.Add(LogoImage, 35, 25, 700);
                                                        }
                                                        ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 15;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affilitied);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 15;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 15;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE FOR THE END OF SEMESTER EXAMINATIONS" + "-" + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text + "");
                                                        mypdfpage.Add(ptc);

                                                        coltop = coltop + 10;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                                        mypdfpage.Add(ptc);
                                                        we = we + 1;
                                                        coltop = coltop + 35;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, deptname);
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, semester);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 25;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, sub_code);
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Exam /Session");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ddlfrmdate.SelectedItem.Text + "/" + ddlsession.SelectedItem.Text);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 25;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 130, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, subname);
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Room No");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 463, coltop, 180, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, dsoverall.Tables[0].Rows[sew]["roomno"].ToString());
                                                        mypdfpage.Add(ptc);

                                                        if (tablerowscount > 25)
                                                        {
                                                            tblrocc = 25;
                                                        }
                                                        else
                                                        {
                                                            tblrocc = tablerowscount;
                                                        }

                                                        table1 = mydocument.NewTable(Fontbold, tblrocc + 1, 5, 4);
                                                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                        table1.VisibleHeaders = false;

                                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 0).SetContent("S.No");
                                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 0).SetFont(Fontbold);

                                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 1).SetContent("Register Number");
                                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 1).SetFont(Fontbold);

                                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 2).SetContent("Name of the Candidate");
                                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 2).SetFont(Fontbold);

                                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 3).SetContent("Answer Booklet No");
                                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 3).SetFont(Fontbold);

                                                        table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 4).SetContent("Signature of Candidate");
                                                        table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 4).SetFont(Fontbold);

                                                        table1.Columns[0].SetWidth(20);
                                                        table1.Columns[1].SetWidth(40);
                                                        table1.Columns[2].SetWidth(80);
                                                        table1.Columns[3].SetWidth(50);
                                                        table1.Columns[4].SetWidth(60);
                                                        gwe = 1;
                                                    }
                                                    string regno = dscheck.Tables[0].Rows[ast]["Reg_No"].ToString();
                                                    string name = dscheck.Tables[0].Rows[ast]["Stud_Name"].ToString();
                                                    string seat = dscheck.Tables[0].Rows[ast]["seat_no"].ToString();
                                                    string hallno = dscheck.Tables[0].Rows[ast]["roomno"].ToString();
                                                    table1.Cell(gwe, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table1.Cell(gwe, 0).SetContent(gwe.ToString());
                                                    table1.Cell(gwe, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table1.Cell(gwe, 1).SetContent(regno.ToString());
                                                    table1.Cell(gwe, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(gwe, 2).SetContent(name.ToString());
                                                    gwe = gwe + 1;
                                                }
                                                int h = 650;
                                                Gios.Pdf.PdfTablePage newpdftabpage11 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, h));
                                                mypdfpage.Add(newpdftabpage11);
                                                coltop = 680;
                                                ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Certified that the following particulars have been verified");
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 30;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "1.The Register No. in the attendance sheet with that in the hall ticket.");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 395, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Present");
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 12;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "2.The identification of the candidate with the photo pasted in the hall ticket");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 395, 735, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total Absent");
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 12;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 30, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "3.The answer book number entered in the attendance sheet by the candidate");
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 75;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black, new PdfArea(mydocument, 245, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
                                                mypdfpage.Add(ptc);

                                                PdfArea pa81 = new PdfArea(mydocument, 20, 700, 360, 125);
                                                PdfRectangle pr81 = new PdfRectangle(mydocument, pa81, Color.Black);
                                                mypdfpage.Add(pr81);

                                                PdfArea pa91 = new PdfArea(mydocument, 470, 700, 60, 25);
                                                PdfRectangle pr91 = new PdfRectangle(mydocument, pa91, Color.Black);
                                                mypdfpage.Add(pr91);

                                                PdfArea pa61 = new PdfArea(mydocument, 470, 725, 60, 25);
                                                PdfRectangle pr61 = new PdfRectangle(mydocument, pa61, Color.Black);
                                                mypdfpage.Add(pr61);

                                                PdfArea tete1 = new PdfArea(mydocument, 15, 10, 565, 825);
                                                PdfRectangle pr11 = new PdfRectangle(mydocument, tete1, Color.Black);
                                                mypdfpage.Add(pr11);

                                                g = 1;
                                                if (h >= 500)
                                                {
                                                    coltop = 10;
                                                    ptc = new PdfTextArea(head, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                                    mypdfpage.Add(ptc);
                                                    coltop = coltop + 15;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affilitied);
                                                    mypdfpage.Add(ptc);
                                                    coltop = coltop + 15;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
                                                    mypdfpage.Add(ptc);
                                                    coltop = coltop + 15;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "ATTENDANCE FOR THE END OF SEMESTER EXAMINATIONS" + "-" + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text + "");
                                                    mypdfpage.Add(ptc);
                                                    coltop = coltop + 10;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                                    mypdfpage.Add(ptc);
                                                    mypdfpage.SaveToDocument();
                                                    mypdfpage = mydocument.NewPage();
                                                    //yq = 190;
                                                }
                                            }
                                        }
                                        string appPath = HttpContext.Current.Server.MapPath("~");
                                        if (appPath != "")
                                        {
                                            string szPath = appPath + "/Report/";
                                            string szFile = "PhasingSheet" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                                            mydocument.SaveToFile(szPath + szFile);
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
                }
                else
                {
                    lblnorecc.Visible = true;
                    lblnorecc.Text = "Please Allot Bundle No And Then Proceed";
                }
            }
        }
        catch (Exception ex)
        {
            lblnorecc.Text = ex.ToString();
            lblnorecc.Visible = true;
        }
    }

    public void printattendancesheetwithphoto()
    {
        try
        {
            divDummyNoSheets.Visible = false;
            Subjectspread.SaveChanges();
            string mothyer = string.Empty;
            string yersessn = string.Empty;
            string sessn = string.Empty;
            int printmonth = ddlMonth.SelectedIndex;
            DropDownList pntddlMonth = new DropDownList();
            pntddlMonth.Items.Clear();
            pntddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
            pntddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            pntddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            pntddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            pntddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            pntddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
            pntddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            pntddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            pntddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            pntddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            pntddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            pntddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            pntddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
            string examstrMonthNameyear = string.Empty;
            if (printmonth != 12)
            {
                examstrMonthNameyear = "  ATTENDANCE FOR END OF SEMESTER EXAMINATIONS - " + pntddlMonth.Items[printmonth].Text.ToString() + " / " + pntddlMonth.Items[printmonth + 1].Text.ToString() + "  " + ddlYear.SelectedValue.ToString();
            }
            else
            {
                examstrMonthNameyear = "  ATTENDANCE FOR END OF SEMESTER EXAMINATIONS - " + pntddlMonth.Items[printmonth].Text.ToString() + "  " + ddlYear.SelectedValue.ToString();
            }
            DAccess2 da = new DAccess2();
            DataSet ds = new DataSet();
            mothyer = ddlMonth.SelectedValue.ToString() + "/" + ddlYear.SelectedValue.ToString();
            yersessn = ddlfrmdate.SelectedValue.ToString() + "/" + ddlsession.SelectedItem.ToString();
            if (ddlsession.SelectedItem.ToString().ToLower() == "both")
            {
                String fnan = ddlsession.Items[1].ToString() + "/" + ddlsession.Items[2].ToString();
                yersessn = ddlfrmdate.SelectedValue.ToString() + "/" + fnan;
            }
            sessn = ddlsession.SelectedItem.ToString();
            string HallNo = string.Empty;
            string bunl = string.Empty;
            int mm1 = 0;
            mm1 = Convert.ToInt16(ddlPageNo.SelectedValue);
            HallNo = ddlPageNo.SelectedItem.ToString();
            string examdate = ddlfrmdate.SelectedValue.ToString();
            string[] dsplit = examdate.Split('-');
            examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
            string departnt = string.Empty;
            int ledgercount = 0;
            for (int f = 0; f < Chkdep.Items.Count; f++)
            {
                if (Chkdep.Items[f].Selected == true)
                {
                    ledgercount = ledgercount + 1;
                    if (departnt == "")
                    {
                        departnt = Chkdep.Items[f].Value.ToString();
                    }
                    else
                    {
                        departnt = departnt + "','" + Chkdep.Items[f].Value.ToString();
                    }
                }
            }
            string dcommt = string.Empty;
            string strquery = ddlsession.SelectedItem.Text;
            if (strquery.Trim() != "Both" && strquery.Trim() != "")
            {
                strquery = " and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
            }
            else
            {
                strquery = string.Empty;
            }
            string print = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.current_semester,r.degree_code,c.course_name + '-' +dp.dept_name as dept, r.batch_year,es.subject_no,sb.subject_name,sb.subject_code,es.bundle_no  from registration r,subjectchooser sc,exam_seating as  es,degree d,department dp,subject sb,course c where sc.roll_no=r.roll_no   and exam_flag<>'Debar' and es.regno=r.Reg_No  and es.subject_no=sc.subject_no and c.course_id=d.course_id and sc.subject_no=sb.subject_no and d.degree_code=r.degree_code and dp.dept_code=d.dept_code and  d.college_code=r.college_code and es.roomno in ('" + departnt + "') and es.edate='" + examdate + "' " + strquery + " and r.college_code='" + Session["collegecode"].ToString() + "' group by es.roomno,es.ses_sion,es.edate ,r.degree_code,  dp.dept_name,r.batch_year,es.subject_no,es.bundle_no,sb.subject_name,sb.subject_code,r.current_semester ,  c.course_name";
            DataSet dsm = new DataSet();
            dsm = d2.select_method_wo_parameter(print, "text");
            if (dsm.Tables.Count > 0 && dsm.Tables[0].Rows.Count > 0)
            {
                dcommt = " ATTENDANCE FOR END OF SEMESTER EXAMINATIONS " + '@' + "Degree & Branch : " + dsm.Tables[0].Rows[0]["dept"].ToString() + "                                                                              Semester:" + dsm.Tables[0].Rows[0]["current_semester"].ToString() + '@' + "Subject Code       : " + dsm.Tables[0].Rows[0]["subject_code"].ToString() + "                                                                   Date of Exam/Session : " + yersessn + '@' + " Subject Name     : " + dsm.Tables[0].Rows[0]["subject_name"].ToString() + "                                                                    " + "Room No : " + dsm.Tables[0].Rows[0]["roomno"].ToString() + "" + "/" + bunl;
            }
            Font Fontco18 = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontco12 = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font Fontco12a = new Font("Book Antiqua", 12, FontStyle.Regular);
            Font Fontco10 = new Font("Book Antiqua", 10, FontStyle.Bold);
            Font Fontco10a = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font Fontco14 = new Font("Book Antiqua", 14, FontStyle.Bold);
            Font Fontco14a = new Font("Book Antiqua", 14, FontStyle.Regular);
            int noofcolumns = 0;
            Dictionary<int, int> diccolvalu = new Dictionary<int, int>();
            for (int c = 0; c < Subjectspread.Sheets[0].ColumnCount; c++)
            {
                if (Subjectspread.Sheets[0].Columns[c].Visible == true)
                {
                    noofcolumns++;
                    diccolvalu.Add(noofcolumns, c);
                }
            }
            int noofpage = (Subjectspread.Sheets[0].Rows.Count) / 10;
            if ((Subjectspread.Sheets[0].Rows.Count % 10) > 0)
            {
                noofpage++;
            }
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
            PdfTable fpspreadtable = mydoc.NewTable(Fontco10a, 11, noofcolumns, 4);
            fpspreadtable.VisibleHeaders = false;
            fpspreadtable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            fpspreadtable.CellRange(0, 0, 0, 4).SetFont(Fontco10a);
            for (int c = 0; c < noofcolumns; c++)
            {
                int colval = diccolvalu[c + 1];
                fpspreadtable.Cell(0, c).SetContent(Subjectspread.Sheets[0].ColumnHeader.Cells[0, colval].Text.ToString());
                fpspreadtable.Cell(0, c).SetCellPadding(15);
                if (colval == 0 || colval == 6 || colval == 7 || colval == 8 || colval == 9)
                {
                    fpspreadtable.Columns[c].SetWidth(40);
                }
                else if (colval == 2 || colval == 4 || colval == 5 || colval == 11)
                {
                    fpspreadtable.Columns[c].SetWidth(80);
                }
                else if (colval == 1 || colval == 3)
                {
                    fpspreadtable.Columns[c].SetWidth(150);
                }
                else if (colval == 10)
                {
                    fpspreadtable.Columns[c].SetWidth(65);
                }
            }
            PdfArea pa1 = new PdfArea(mydoc, 14, 12, 566, 821);
            PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
            mypdfpage.Add(pr3);
            PdfTextArea pdf1;
            string SubjectCodeAndName = Subjectspread.Sheets[0].Cells[0, 7].Text;
            string[] splitSubjectCodeAndName = SubjectCodeAndName.Split('-');
            if (chkheadimage.Checked == true)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                {
                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                    mypdfpage.Add(LogoImage, 20, 15, 400);
                }
            }
            else
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 21, 25, 450);
                }
                string clm = "SELECT *  from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(clm, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        pdf1 = new PdfTextArea(Fontco18, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
                        mypdfpage.Add(pdf1);
                        string distpinspp = ds.Tables[0].Rows[0]["district"].ToString() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
                        string distpin = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        string[] splitdistpin = distpin.Split(',');
                        pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + splitdistpin[0] + "");
                        mypdfpage.Add(pdf1);
                        pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 60, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Office of the Controller of Examinations");
                        mypdfpage.Add(pdf1);
                        pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + distpinspp + "");
                        mypdfpage.Add(pdf1);
                    }
                }
            }
            pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + examstrMonthNameyear + "");
            mypdfpage.Add(pdf1);
            pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 13, 95, 750, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________");
            mypdfpage.Add(pdf1);
            int imgetop = 180;
            int noofrow = 0;
            int nofopage = 1;
            for (int r = 0; r < Subjectspread.Sheets[0].Rows.Count; r++)
            {
                if ((r % 10) == 0 && r > 0)
                {
                    imgetop = 180;
                    noofrow = 0;
                    mypdfpage = mydoc.NewPage();
                    PdfArea pa12 = new PdfArea(mydoc, 14, 12, 566, 821);
                    PdfRectangle pr23 = new PdfRectangle(mydoc, pa12, Color.Black);
                    mypdfpage.Add(pr23);
                    if (chkheadimage.Checked == true)
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                            mypdfpage.Add(LogoImage, 20, 15, 400);
                        }
                    }
                    else
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 21, 25, 450);
                        }
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                pdf1 = new PdfTextArea(Fontco18, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
                                mypdfpage.Add(pdf1);
                                string distpinspp = ds.Tables[0].Rows[0]["district"].ToString() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
                                string distpin = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                                string[] splitdistpin = distpin.Split(',');
                                pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + splitdistpin[0] + "");
                                mypdfpage.Add(pdf1);
                                pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 60, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Office of the Controller of Examinations");
                                mypdfpage.Add(pdf1);
                                pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + distpinspp + "");
                                mypdfpage.Add(pdf1);
                            }
                        }
                    }
                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + examstrMonthNameyear + "");
                    mypdfpage.Add(pdf1);
                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 13, 95, 750, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________");
                    mypdfpage.Add(pdf1);
                    nofopage++;
                    if (noofpage == nofopage)
                    {
                        fpspreadtable = mydoc.NewTable(Fontco10a, (Subjectspread.Sheets[0].Rows.Count % 10) + 1, noofcolumns, 4);
                    }
                    else
                    {
                        fpspreadtable = mydoc.NewTable(Fontco10a, 11, noofcolumns, 4);
                    }
                    fpspreadtable.VisibleHeaders = false;
                    fpspreadtable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    fpspreadtable.CellRange(0, 0, 0, 4).SetFont(Fontco10a);
                    for (int c = 0; c < noofcolumns; c++)
                    {
                        int colval = diccolvalu[c + 1];
                        fpspreadtable.Cell(0, c).SetContent(Subjectspread.Sheets[0].ColumnHeader.Cells[0, colval].Text.ToString());
                        fpspreadtable.Cell(0, c).SetCellPadding(15);
                        if (colval == 0 || colval == 6 || colval == 7 || colval == 8 || colval == 9)
                        {
                            fpspreadtable.Columns[c].SetWidth(40);
                        }
                        else if (colval == 2 || colval == 4 || colval == 5 || colval == 11)
                        {
                            fpspreadtable.Columns[c].SetWidth(80);
                        }
                        else if (colval == 1 || colval == 3)
                        {
                            fpspreadtable.Columns[c].SetWidth(150);
                        }
                        else if (colval == 10)
                        {
                            fpspreadtable.Columns[c].SetWidth(65);
                        }
                    }
                }
                noofrow++;
                string regno = Subjectspread.Sheets[0].Cells[r, 2].Text.ToString();
                for (int c = 0; c < noofcolumns; c++)
                {
                    int colval = diccolvalu[c + 1];
                    if (colval != 10)
                    {
                        fpspreadtable.Cell(noofrow, c).SetContent(Subjectspread.Sheets[0].Cells[r, colval].Text.ToString());
                        fpspreadtable.Cell(noofrow, c).SetCellPadding(18);
                    }
                    else
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        string rollval = d2.GetFunction("select roll_no from registration where reg_no='" + regno + "'");
                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollval + ".jpeg")))
                        {
                            DataSet dsstuphoto = d2.select_method_wo_parameter("select photo from stdphoto where app_no in(select app_no from registration where reg_no='" + regno + "')", "Text");
                            if (dsstuphoto.Tables[0].Rows.Count > 0)
                            {
                                if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                {
                                    byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["photo"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollval + ".jpeg")) == false)
                                        {
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rollval + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                    }
                                }
                                memoryStream.Dispose();
                                memoryStream.Close();
                            }
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollval + ".jpeg")))
                        {
                            fpspreadtable.Cell(noofrow, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                            PdfImage leftimage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rollval + ".jpeg"));
                            mypdfpage.Add(leftimage, 470, imgetop, 50);
                            imgetop = imgetop + 50;
                        }
                    }
                }
                if (r == Subjectspread.Sheets[0].Rows.Count - 1 || r == Subjectspread.Sheets[0].Rows.Count - 2 || r == Subjectspread.Sheets[0].Rows.Count - 3)
                {
                    foreach (PdfCell pr in fpspreadtable.CellRange(noofrow, 0, noofrow, 1).Cells)
                    {
                        pr.ColSpan = noofcolumns - 2;
                    }
                }
                if (((r + 1) % 10) == 0 || r == Subjectspread.Sheets[0].Rows.Count - 1)
                {
                    Gios.Pdf.PdfTablePage newpdftabpage2 = fpspreadtable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 16, 120, 565, 1000));
                    mypdfpage.Add(newpdftabpage2);
                    mypdfpage.SaveToDocument();
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Attendance12" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            lblnorecc.Text = ex.ToString();
            lblnorecc.Visible = true;
        }
    }

    //Rajkumar 20/1/2018===================

    protected void chksubwise_OnCheckedChanged(object sender, EventArgs e)
    {
        fsPrintSetting.Visible = false;
        if (chksubwise.Checked == true)
        {
            ddlSubject.Visible = true;
            txthallno.Enabled = false;
            checkconsolidate.Checked = false;
            AttSpread.Visible = false;
            btngenerate.Visible = false;
            Subjectspread.Visible = false;
            loadSubject();
        }
        else
        {
            ddlSubject.Visible = false;
            txthallno.Enabled = true;
            AttSpread.Visible = false;
            btngenerate.Visible = false;
            Subjectspread.Visible = false;

        }
    }
    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void loadSubject()
    {
        try
        {
            string SelectQ = string.Empty;
            DataTable dtSubject = new DataTable();
            string examdate = ddlfrmdate.SelectedValue.ToString();
            string[] dsplit = examdate.Split('-');
            examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
            string sessiond = string.Empty;
            if (ddlsession.SelectedItem.Text == "Both")
            {
                sessiond = string.Empty;
            }
            else
            {
                sessiond = "  and et.exam_session like'" + ddlsession.SelectedItem.Text + "%'";
            }
            if (!string.IsNullOrEmpty(examdate) && !string.IsNullOrEmpty(ddlYear.SelectedItem.ToString()) && !string.IsNullOrEmpty(ddlMonth.SelectedValue.ToString()))
            {
                SelectQ = "select distinct s.subject_code,s.subject_name from exmtt e,exmtt_det et,subject s where  et.exam_date='" + examdate + "' and e.exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_Year='" + ddlYear.SelectedItem + "' " + sessiond + " and e.exam_code=et.exam_code and et.subject_no=s.subject_no";
                SelectQ = SelectQ + "  union ";
                SelectQ = SelectQ + "  select distinct s.subject_code,s.subject_name from examtheorybatch es,subject s where s.subject_no=es.SubNo and es.ExamDate='" + examdate + "'  order by s.subject_name,s.subject_code";

                dtSubject = dirAcc.selectDataTable(SelectQ);
                if (dtSubject.Rows.Count > 0)
                {
                    ddlSubject.DataSource = dtSubject;
                    ddlSubject.DataTextField = "subject_name";
                    ddlSubject.DataValueField = "subject_code";
                    ddlSubject.DataBind();
                }
            }
        }
        catch
        {

        }
    }
    public void loadSubjectwise()
    {
        try
        {
            string SelectQ = string.Empty;
            DataTable dtStudent = new DataTable();
            AttSpread.Visible = true;
            Subjectspread.Visible = false;
            AttSpread.Sheets[0].RowCount = 0;
            AttSpread.Sheets[0].ColumnCount = 5;
            AttSpread.Sheets[0].RowHeader.Visible = false;
            AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            btnDisplay.Visible = false;
            btnDummyNoSheets.Visible = false;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            AttSpread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            AttSpread.Sheets[0].DefaultStyle.Font.Bold = false;
            AttSpread.Sheets[0].Columns[0].Width = 100;
            AttSpread.Sheets[0].Columns[1].Width = 100;
            AttSpread.Sheets[0].Columns[2].Width = 150;
            AttSpread.Sheets[0].Columns[3].Width = 200;
            AttSpread.Sheets[0].Columns[4].Width = 150;
            AttSpread.Sheets[0].Columns[0].Locked = true;
            AttSpread.Sheets[0].Columns[2].Locked = true;
            AttSpread.Sheets[0].Columns[3].Locked = true;
            AttSpread.Sheets[0].Columns[4].Locked = true;
            AttSpread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.None);
            AttSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.None);
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Dept Name";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total";
            AttSpread.Sheets[0].AutoPostBack = false;
            AttSpread.CommandBar.Visible = false;
            string examdate = ddlfrmdate.SelectedValue.ToString();
            string[] dsplit = examdate.Split('-');
            examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
            string sessiond = string.Empty;
            string sess = string.Empty;
            int sno = 0;
            FarPoint.Web.Spread.CheckBoxCellType cheall = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType cheselectall = new FarPoint.Web.Spread.CheckBoxCellType();
            cheselectall.AutoPostBack = true;
            int height = 45;
            string strSubject = string.Empty;

            if (ddlSubject.Items.Count > 0)
            {
                strSubject = "and s.subject_code='" + ddlSubject.SelectedValue.ToString() + "'";
            }
            if (ddlsession.SelectedItem.Text == "Both")
            {
                sessiond = string.Empty;
            }
            else
            {
                sess = "   and es.examsession like '" + ddlsession.SelectedItem.Text + "%'";
                sessiond = "  and et.exam_session like '" + ddlsession.SelectedItem.Text + "%'";
            }
            if (!string.IsNullOrEmpty(examdate) && !string.IsNullOrEmpty(ddlYear.SelectedItem.ToString()) && !string.IsNullOrEmpty(ddlMonth.SelectedValue.ToString()))
            {
                if (!chkBatch.Checked)
                {
                    SelectQ = "select s.subject_code,(c.Course_Name+'-'+de.Dept_Name) as Cource,d.Degree_Code,s.subject_name,et.exam_date,et.exam_session,d.Acronym,COUNT(ea.roll_no) as stucount,r.Batch_Year from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem + "' and  et.exam_date='" + examdate + "' " + sessiond + " " + strSubject + " group by s.subject_code,de.Dept_Name,c.Course_Name,s.subject_name,et.exam_date,et.exam_session,d.Acronym,c.type,r.Batch_Year,d.Degree_Code order by (c.Course_Name+'-'+de.Dept_Name)";
                }
                else
                {
                   
                    SelectQ = "select s.subject_code,(c.Course_Name+'-'+de.Dept_Name) as Cource,d.Degree_Code,s.subject_name, es.ExamSession ,es.ExamDate,d.Acronym,COUNT(ea.roll_no) as stucount,r.Batch_Year from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,examtheorybatch es, Registration r,Degree d,course c,Department de where r.app_no=es.AppNo and e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=es.SubNo and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem + "' and es.ExamDate ='" + examdate + "'   " + sess + "    " + strSubject + "  and es.Batch='"+Convert.ToString(ddlBatch.SelectedValue)+"'  group by s.subject_code,de.Dept_Name,c.Course_Name,s.subject_name,ExamSession ,ExamDate,d.Acronym,c.type,r.Batch_Year,d.Degree_Code order by  s.subject_code ,(c.Course_Name+'-'+de.Dept_Name)";
                }

                dtStudent = dirAcc.selectDataTable(SelectQ);

                if (dtStudent.Rows.Count > 0)
                {
                    AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].CellType = cheselectall;
                    AttSpread.Sheets[0].SpanModel.Add(AttSpread.Sheets[0].RowCount - 1, 2, 1, 3);
                    for (int i = 0; i < dtStudent.Rows.Count; i++)
                    {
                        sno++;
                        AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].CellType = cheall;
                        height = height + AttSpread.Sheets[0].Rows[i].Height;
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtStudent.Rows[i]["subject_name"]);
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(dtStudent.Rows[i]["subject_code"]);
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dtStudent.Rows[i]["Cource"]);
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(dtStudent.Rows[i]["Degree_Code"]);
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtStudent.Rows[i]["stucount"]);
                        AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(dtStudent.Rows[i]["Batch_Year"]);

                    }
                    if (height > 600)
                    {
                        AttSpread.Height = 400;
                    }
                    else if (height > 500)
                    {
                        AttSpread.Height = height - 200;
                    }
                    else
                    {
                        AttSpread.Height = 300;
                    }
                    AttSpread.Width = 700;
                    AttSpread.SaveChanges();
                    AttSpread.Sheets[0].PageSize = AttSpread.Sheets[0].RowCount;
                    btngenerate.Visible = true;
                }
            }
        }
        catch
        {

        }
    }
    public void loadStudent()
    {
        try
        {
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            AttSpread.SaveChanges();
            lblnorecc.Visible = false;
            lblDispErr.Visible = false;
            lblDispErr.Text = string.Empty;
            lblerror1.Visible = false;
            Subjectspread.Sheets[0].RowCount = 0;
            Subjectspread.Sheets[0].AutoPostBack = true;
            Subjectspread.SaveChanges();
            Subjectspread.Visible = true;
            Subjectspread.Sheets[0].ColumnCount = 12;
            Subjectspread.CommandBar.Visible = false;
            Subjectspread.RowHeader.Visible = false;
            Subjectspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Subjectspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Subjectspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].DefaultStyle.Font.Bold = false;
            Subjectspread.Sheets[0].AutoPostBack = true;

            Subjectspread.Sheets[0].Columns[0].Width = 50;
            Subjectspread.Sheets[0].Columns[1].Width = 130;
            Subjectspread.Sheets[0].Columns[2].Width = 130;
            Subjectspread.Sheets[0].Columns[3].Width = 250;
            Subjectspread.Sheets[0].Columns[4].Width = 150;
            Subjectspread.Sheets[0].Columns[5].Width = 150;
            Subjectspread.Sheets[0].Columns[6].Width = 130;
            Subjectspread.Sheets[0].Columns[7].Width = 100;
            Subjectspread.Sheets[0].Columns[9].Width = 100;
            Subjectspread.Sheets[0].Columns[10].Width = 150;

            Subjectspread.Sheets[0].Columns[0].CellType = txt;
            Subjectspread.Sheets[0].Columns[1].CellType = txt;
            Subjectspread.Sheets[0].Columns[2].CellType = txt;
            Subjectspread.Sheets[0].Columns[3].CellType = txt;
            Subjectspread.Sheets[0].Columns[4].CellType = txt;
            Subjectspread.Sheets[0].Columns[5].CellType = txt;
            Subjectspread.Sheets[0].Columns[6].CellType = txt;
            Subjectspread.Sheets[0].Columns[7].CellType = txt;
            Subjectspread.Sheets[0].Columns[9].CellType = txt;
            Subjectspread.Sheets[0].Columns[10].CellType = txt;

            Subjectspread.Sheets[0].RowCount++;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].VerticalAlign = VerticalAlign.Middle;

            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register Number";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Name";
            if (cblsearch.Items[5].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[4].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[4].Visible = false;
            }
            if (cblsearch.Items[6].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[5].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Department";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[5].Visible = false;
            }
            if (cblsearch.Items[1].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[6].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Seat No";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[6].Visible = false;
            }
            if (cblsearch.Items[4].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[7].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Hall No";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[7].Visible = false;
            }
            if (cblsearch.Items[7].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[1].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Name";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[1].Visible = false;
            }
            if (cblsearch.Items[0].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[8].Visible = true;
                Subjectspread.Sheets[0].Columns[8].Width = 200;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Answer Booklet No";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[8].Visible = false;
            }
            if (cblsearch.Items[2].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[9].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "HS to Write " + '"' + "AB" + '"' + " for Absentees";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[9].Visible = false;
            }
            if (cblsearch.Items[8].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[10].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Student Photo";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[10].Visible = false;
            }
            if (cblsearch.Items[3].Selected == true)
            {
                Subjectspread.Sheets[0].Columns[11].Visible = true;
                Subjectspread.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Signature of Candidate  ";
            }
            else
            {
                Subjectspread.Sheets[0].Columns[11].Visible = false;
            }
            Subjectspread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            Subjectspread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            Subjectspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Subjectspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            Subjectspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            Subjectspread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
            Subjectspread.Sheets[0].Columns[10].VerticalAlign = VerticalAlign.Middle;
            string SelectQ = string.Empty;
            int sno = 0;
            string sessiond = string.Empty;
            string sess = string.Empty;
            if (ddlsession.SelectedItem.Text == "Both")
            {
                sessiond = string.Empty;
            }
            else
            {
                sess = "  and et.examsession like '" + ddlsession.SelectedItem.Text + "%'";
                sessiond = "  and et.exam_session like '" + ddlsession.SelectedItem.Text + "%'";
            }
            string examdate = ddlfrmdate.SelectedValue.ToString();
            string[] dsplit = examdate.Split('-');
            examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
            bool setflag = false;
            if (!string.IsNullOrEmpty(examdate) && !string.IsNullOrEmpty(ddlYear.SelectedValue.ToString()) && !string.IsNullOrEmpty(ddlMonth.SelectedValue.ToString()))
            {
                SelectQ = "select ea.roll_no,r.Reg_No,r.Stud_Name,r.Stud_Type,r.Current_Semester,s.subject_code,s.subject_name,(c.Course_Name+'-'+d.Acronym) as Cource,d.Degree_Code,de.Dept_Name,et.exam_date as examdate,et.exam_session as examsession ,d.Acronym,r.Batch_Year from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and  et.exam_date='" + examdate + "' " + sessiond + "    order by r.Reg_No"; // (c.Course_Name+'-'+de.Dept_Name),ea.roll_no,r.Stud_Type"; (c.Course_Name+'-'+de.Dept_Name),ea.roll_no,r.Stud_Type";

                if (chkBatch.Checked)
                {
                    SelectQ = "select ea.roll_no,r.Reg_No,r.Stud_Name,r.Stud_Type,r.Current_Semester,s.subject_code,s.subject_name,(c.Course_Name+'-'+d.Acronym) as Cource,d.Degree_Code,de.Dept_Name,et.examdate,et.examsession,d.Acronym,r.Batch_Year from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,examtheorybatch et, Registration r,Degree d,course c,Department de where et.AppNo=r.App_No and e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.SubNo and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and  et.examdate='" + examdate + "'    " + sess + "  and et.Batch='B1'   order by r.Reg_No"; // (c.Course_Name+'-'+de.Dept_Name),ea.roll_no,r.Stud_Type"; (c.Course_Name+'-'+de.Dept_Name),ea.roll_no,r.Stud_Type";
                }

                DataTable dtStudentinfo = dirAcc.selectDataTable(SelectQ);
                int row = 0;
                if (dtStudentinfo.Rows.Count > 0)
                {
                    for (int r = 1; r < AttSpread.Sheets[0].RowCount; r++)
                    {
                        int stva = Convert.ToInt32(AttSpread.Sheets[0].Cells[r, 1].Value);
                        if (stva == 1)
                        {
                            DataTable dicStud = new DataTable();
                            setflag = true;
                            string subcode = AttSpread.Sheets[0].Cells[r, 2].Note.ToString();
                            string degCode = AttSpread.Sheets[0].Cells[r, 3].Note.ToString();
                            dtStudentinfo.DefaultView.RowFilter = "subject_code='" + subcode + "' and Degree_Code='" + degCode + "'";
                            dicStud = dtStudentinfo.DefaultView.ToTable();
                            if (dicStud.Rows.Count > 0)
                            {

                                for (int i = 0; i < dicStud.Rows.Count; i++)
                                {
                                    Subjectspread.Sheets[0].RowCount = Subjectspread.Sheets[0].RowCount + 1;
                                    sno++;
                                    Subjectspread.Sheets[0].Cells[row, 0].Text = sno.ToString();
                                    Subjectspread.Sheets[0].Cells[row, 0].Tag = Convert.ToString(dicStud.Rows[i]["Current_Semester"]) + "#" + Convert.ToString(dicStud.Rows[i]["examsession"]);//Deepali 14.5.18
                                    Subjectspread.Sheets[0].Cells[row, 2].Text = Convert.ToString(dicStud.Rows[i]["Reg_No"]);
                                    Subjectspread.Sheets[0].Cells[row, 3].Text = Convert.ToString(dicStud.Rows[i]["Stud_Name"]);
                                    Subjectspread.Sheets[0].Cells[row, 4].Text = Convert.ToString(dicStud.Rows[i]["Stud_Type"]);
                                    Subjectspread.Sheets[0].Cells[row, 5].Text = Convert.ToString(dicStud.Rows[i]["Cource"]);
                                    Subjectspread.Sheets[0].Cells[row, 5].Tag = Convert.ToString(dicStud.Rows[i]["Degree_Code"]);
                                    //Subjectspread.Sheets[0].Cells[row, 6].Text = "";
                                    //Subjectspread.Sheets[0].Cells[row, 7].Text = "";
                                    Subjectspread.Sheets[0].Cells[row, 1].Text = Convert.ToString(dicStud.Rows[i]["subject_name"]);
                                    Subjectspread.Sheets[0].Cells[row, 1].Tag = Convert.ToString(dicStud.Rows[i]["subject_code"]);
                                    MyImg mi = new MyImg();
                                    mi.ImageUrl = "Handler/Handler4.ashx?rollno=" + Convert.ToString(dicStud.Rows[i]["roll_no"]);
                                    Subjectspread.Sheets[0].Cells[row, 10].CellType = mi;
                                    row++;
                                }

                            }
                        }
                    }
                    Subjectspread.Sheets[0].Columns[8].Width = 200;
                    Subjectspread.Sheets[0].RowCount = Subjectspread.Sheets[0].RowCount + 2;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Font.Bold = true;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Left;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Left;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Text = "Certified that the following particulars have been verified";
                    printcheckvalue.Text = Convert.ToString(Subjectspread.Sheets[0].RowCount - 5);// added by sridhar
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Small;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Small;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Text = "1.The Register No. in the attendance sheet with that in the hall ticket.";
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Text = "2.The identification of the candidate with the photo given in the hall ticket";
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 8].Text = "Page Total Present" + " " + ":";
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 8].Text = "Page Total Absent " + " " + ":";
                    Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 1, 0, 1, 11);
                    Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 2, 0, 1, 8);
                    Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 3, 0, 1, 8);
                    Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 2, 8, 1, 4);
                    Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 3, 8, 1, 4);

                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
                    //Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 4, 0].Border.BorderColorBottom = Color.White;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;

                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 8].Border.BorderColorBottom = Color.Black;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 0].Border.BorderColorRight = Color.Black;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 0].Border.BorderColorRight = Color.Black;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 8].Border.BorderColorBottom = Color.Black;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 8].Border.BorderColorTop = Color.Black;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 1, 4].Border.BorderColorRight = Color.Black;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 2, 9].Border.BorderColorBottom = Color.Black;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 9].Border.BorderColorBottom = Color.Black;
                    Subjectspread.Sheets[0].Cells[Subjectspread.Sheets[0].RowCount - 3, 9].Border.BorderColorTop = Color.Black;
                    Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 2, 9, 1, 4);
                    Subjectspread.Sheets[0].SpanModel.Add(Subjectspread.Sheets[0].RowCount - 3, 9, 1, 4);

                    Subjectspread.Visible = true;
                    Subjectspread.Sheets[0].PageSize = Subjectspread.Sheets[0].RowCount;
                    Button2.Visible = true;
                }
            }
        }
        catch
        {
        }
    }
    public void printSubwise()
    {
        try
        {
            Subjectspread.SaveChanges();
            string mothyer = string.Empty;
            string yersessn = string.Empty;
            string sessn = string.Empty;

            //int printmonth = ddlMonth.SelectedIndex;
            int printmonth = 0;
            int.TryParse(Convert.ToString(ddlMonth.SelectedValue), out printmonth);//Deepali 14.5.18
            DropDownList pntddlMonth = new DropDownList();
            pntddlMonth.Items.Clear();
            pntddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
            pntddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            pntddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            pntddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            pntddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            pntddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
            pntddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            pntddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            pntddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            pntddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            pntddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            pntddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            pntddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
            string examstrMonthNameyear = string.Empty;
            //if (printmonth != 12)
            //{
                examstrMonthNameyear = "  ATTENDANCE FOR SEMESTER END EXAMINATIONS - " + pntddlMonth.Items[printmonth].Text.ToString().ToUpper() + "  " + ddlYear.SelectedValue.ToString(); //+ pntddlMonth.Items[printmonth + 1].Text.ToString() + "
            //}
            //else
            //{
            //    examstrMonthNameyear = "  ATTENDANCE FOR SEMESTER END EXAMINATIONS - " + pntddlMonth.Items[printmonth].Text.ToString() + "  " + ddlYear.SelectedValue.ToString();
            //}
            DAccess2 da = new DAccess2();
            DataSet ds = new DataSet();
            mothyer = ddlMonth.SelectedValue.ToString() + "/" + ddlYear.SelectedValue.ToString();
            //yersessn = ddlfrmdate.SelectedValue.ToString() + "/" + ddlsession.SelectedItem.ToString();
            //if (ddlsession.SelectedItem.ToString().ToLower() == "both")
            //{
            //    String fnan = ddlsession.Items[1].ToString() + "/" + ddlsession.Items[2].ToString();
            //    yersessn = ddlfrmdate.SelectedValue.ToString() + "/" + fnan;
            //}
            yersessn = ddlfrmdate.SelectedValue.ToString() + "/" + Convert.ToString(Subjectspread.Sheets[0].Cells[0, 0].Tag).Split('#')[1];//Deepali 14.5.18
            sessn = ddlsession.SelectedItem.ToString();
            string HallNo = string.Empty;
            string bunl = string.Empty;
            int mm1 = 0;
            //mm1 = Convert.ToInt16(ddlPageNo.SelectedValue);
            //Hashtable newhashhall = (Hashtable)ViewState["haltable"];
            //Hashtable newhashboundval = (Hashtable)ViewState["boundvaltable"];
            //if (newhashhall.Count > 0)
            //{
            //    HallNo = newhashhall[mm1].ToString();
            //}
            //if (newhashboundval != null)
            //{
            //    if (newhashboundval.Count > 0)
            //    {
            //        bunl = newhashboundval[mm1].ToString();
            //    }
            //}
            string examdate = ddlfrmdate.SelectedValue.ToString();
            string[] dsplit = examdate.Split('-');
            examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
            string departnt = string.Empty;
            int ledgercount = 0;
            for (int f = 0; f < Chkdep.Items.Count; f++)
            {
                if (Chkdep.Items[f].Selected == true)
                {
                    ledgercount = ledgercount + 1;
                    if (departnt == "")
                    {
                        departnt = Chkdep.Items[f].Value.ToString();
                    }
                    else
                    {
                        departnt = departnt + "','" + Chkdep.Items[f].Value.ToString();
                    }
                }
            }
            string dcommt = string.Empty;
            Session["column_header_row_count"] = 3;
            string strquery = ddlsession.SelectedItem.Text;
            if (strquery.Trim() != "Both" && strquery.Trim() != "")
            {
                strquery = " and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'";
            }
            else
            {
                strquery = string.Empty;
            }

            string print = "select distinct es.roomno,COUNT(1) as strength,es.ses_sion,es.edate,r.current_semester,r.degree_code,c.course_name + '-' +dp.dept_name as dept, r.batch_year,es.subject_no,sb.subject_name,sb.subject_code,es.bundle_no  from registration r,subjectchooser sc,exam_seating as  es,degree d,department dp,subject sb,course c where sc.roll_no=r.roll_no   and exam_flag<>'Debar' and es.regno=r.Reg_No  and es.subject_no=sc.subject_no and c.course_id=d.course_id and sc.subject_no=sb.subject_no and d.degree_code=r.degree_code and dp.dept_code=d.dept_code and  d.college_code=r.college_code and es.roomno in ('" + departnt + "') and es.edate='" + examdate + "' " + strquery + " and r.college_code='" + Session["collegecode"].ToString() + "' group by es.roomno,es.ses_sion,es.edate ,r.degree_code,  dp.dept_name,r.batch_year,es.subject_no,es.bundle_no,sb.subject_name,sb.subject_code,r.current_semester ,  c.course_name";

            DataSet dsm = new DataSet();
            string cur_sem = string.Empty;
            //Deepali 14.5.18==============================
            if (Subjectspread.Sheets[0].RowCount > 0)
                cur_sem = Convert.ToString(Subjectspread.Sheets[0].Cells[0, 0].Tag).Split('#')[0];
            //Deepali 14.5.18============================
            dsm = d2.select_method_wo_parameter(print, "text");
            if (dsm.Tables[0].Rows.Count > 0)
            {
                //  cur_sem = dsm.Tables[0].Rows[0]["current_semester"].ToString(); //Deepali 14.5.18 
                dcommt = " ATTENDANCE FOR END OF SEMESTER EXAMINATIONS " + '@' + "Degree & Branch : " + dsm.Tables[0].Rows[0]["dept"].ToString() + "                                                                              Semester:" + dsm.Tables[0].Rows[0]["current_semester"].ToString() + '@' + "Subject Code       : " + dsm.Tables[0].Rows[0]["subject_code"].ToString() + "                                                                   Date of Exam/Session : " + yersessn + '@' + " Subject Name     : " + dsm.Tables[0].Rows[0]["subject_name"].ToString() + "                                                                    " + "Room No : " + dsm.Tables[0].Rows[0]["roomno"].ToString() + "" + "/" + bunl;
            }
            Font Fontco18 = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontco12 = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font Fontco12a = new Font("Book Antiqua", 12, FontStyle.Regular);
            Font Fontco10 = new Font("Book Antiqua", 10, FontStyle.Bold);
            Font Fontco10a = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font Fontco14 = new Font("Book Antiqua", 14, FontStyle.Bold);
            Font Fontco14a = new Font("Book Antiqua", 14, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
            PdfArea pa1 = new PdfArea(mydoc, 14, 12, 566, 821);
            PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
            mypdfpage.Add(pr3);
            PdfTextArea pdf1;
            string deptanddegree = Subjectspread.Sheets[0].Cells[0, 5].Text.ToString();
            string SubjectCodeAndName = Subjectspread.Sheets[0].Cells[0, 1].Text + "$" + Subjectspread.Sheets[0].Cells[0, 1].Tag;
            string[] splitSubjectCodeAndName = SubjectCodeAndName.Split('$');
            if (chkheadimage.Checked == true)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                {
                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                    mypdfpage.Add(LogoImage, 20, 15, 400);
                }
            }
            else
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 21, 25, 450);
                }
                string clm = "SELECT *  from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(clm, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        pdf1 = new PdfTextArea(Fontco18, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
                        mypdfpage.Add(pdf1);
                        string distpinspp = ds.Tables[0].Rows[0]["district"].ToString() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
                        string distpin =  Convert.ToString(ds.Tables[0].Rows[0]["category"]) +" - "+ ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        string[] splitdistpin = distpin.Split(',');
                        pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + splitdistpin[0] + "");
                        mypdfpage.Add(pdf1);
                        pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 60, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Office of the Controller of Examinations");
                        mypdfpage.Add(pdf1);
                        pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + distpinspp + "");
                        mypdfpage.Add(pdf1);
                    }
                }
            }
            pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + examstrMonthNameyear + "");
            mypdfpage.Add(pdf1);
            pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 13, 95, 750, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________");
            mypdfpage.Add(pdf1);
            //int mm = Convert.ToInt16(ddlPageNo.SelectedValue);
            //string Date = HashDate[mm].ToString();
            //string session = HasSession[mm].ToString();
            //HallNo = Hashhall[mm].ToString();
            //string hdeg = Hasdegree[mm].ToString();
            //string hroll = Hasroll[mm].ToString();
            //string bndlee = boundvl[mm].ToString();
            Gios.Pdf.PdfTable table1forpage1 = mydoc.NewTable(Fontco10a, 3, 5, 7);
            table1forpage1.VisibleHeaders = false;
            table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
            table1forpage1.SetColumnsWidth(new int[] { 21, 2, 40 });
            table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
            table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
            table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage1.Cell(0, 0).SetContent("Degree & Branch");
            table1forpage1.Cell(0, 1).SetContent(":");
            table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
            table1forpage1.Cell(0, 2).SetContent(deptanddegree);
            table1forpage1.Cell(2, 0).SetContent("Subject Name");
            table1forpage1.Cell(2, 1).SetContent(":");
            table1forpage1.Cell(2, 2).SetContent(splitSubjectCodeAndName[0].ToString());
            table1forpage1.Cell(1, 0).SetContent("Subject Code ");
            table1forpage1.Cell(1, 1).SetContent(":");
            table1forpage1.Cell(1, 2).SetContent(splitSubjectCodeAndName[1].ToString());
            //table1forpage1.Cell(0, 3).SetContent("                      ");
            //table1forpage1.Cell(1, 3).SetContent("                        ");
            //table1forpage1.Cell(2, 3).SetContent("                      ");
            Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, 105, 550, 550));
            mypdfpage.Add(newpdftabpage2);
            table1forpage1 = mydoc.NewTable(Fontco10a, 3, 5, 7);
            table1forpage1.VisibleHeaders = false;
            table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
            table1forpage1.SetColumnsWidth(new int[] { 75, 10, 88 });
            table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
            table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
            table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage1.Cell(0, 0).SetContent("Semester");
            table1forpage1.Cell(0, 1).SetContent(":");
            table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
            //table1forpage1.Cell(0, 2).SetContent(dsm.Tables[0].Rows[0]["current_semester"].ToString());
            table1forpage1.Cell(0, 2).SetContent(cur_sem);
            table1forpage1.Cell(1, 0).SetContent("Date of Exam/Session");
            table1forpage1.Cell(1, 1).SetContent(":");
            table1forpage1.Cell(1, 2).SetContent(yersessn);
            table1forpage1.Cell(2, 0).SetContent("Room No / Bundle No ");
            table1forpage1.Cell(2, 1).SetContent(":");
            table1forpage1.Cell(2, 2).SetContent("" + " / " + "");
            //table1forpage1.Cell(0, 3).SetContent("                      ");
            //table1forpage1.Cell(1, 3).SetContent("                        ");
            //table1forpage1.Cell(2, 3).SetContent("                      ");
            newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 340, 105, 326, 200));
            mypdfpage.Add(newpdftabpage2);

            //Rajkumar
            //Gios.Pdf.PdfTablePage newpdftabpage2;
            int tblcount = 0;
            if (Convert.ToInt32(Subjectspread.Sheets[0].RowCount - 3) > 26)
                tblcount = 25;//deepali 12.5.18
            else
                tblcount = Convert.ToInt32(Subjectspread.Sheets[0].RowCount - 3) + 1;
            int clct = 3;
            if (cblsearch.Items[3].Selected == true)
            {
                clct++;
            }
            if (cblsearch.Items[0].Selected == true)
            {
                clct++;
            }
            table1forpage1 = mydoc.NewTable(Fontco10a, tblcount, clct, 1);
            table1forpage1.VisibleHeaders = false;
            table1forpage1.SetBorders(System.Drawing.Color.Black, 1, BorderType.CompleteGrid);
            if (clct == 5)
            {
                table1forpage1.SetColumnsWidth(new int[] { 40, 130, 250, 220, 200 });
                table1forpage1.Columns[4].SetContentAlignment(ContentAlignment.MiddleCenter);
                table1forpage1.Cell(0, 4).SetCellPadding(4);
            }
            else
            {
                table1forpage1.SetColumnsWidth(new int[] { 40, 130, 250, 220 });
            }
            table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
            table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);

            table1forpage1.Columns[3].SetContentAlignment(ContentAlignment.MiddleCenter);//deepali 16.5.18
            table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            int tableheight = 0;
            int tblheight = 0;
            int coltop = 100;
            //deepali 16.5.18=========================
            table1forpage1.Cell(0, 0).SetCellPadding(4);
            table1forpage1.Cell(0, 1).SetCellPadding(4);
            table1forpage1.Cell(0, 2).SetCellPadding(4);
            table1forpage1.Cell(0, 3).SetCellPadding(4);

            //=========================
            if (Convert.ToInt32(Subjectspread.Sheets[0].RowCount - 3) > 0)
            {
                table1forpage1.Cell(0, 0).SetContent("S.No");
                table1forpage1.Cell(0, 1).SetContent("Register Number");
                table1forpage1.Cell(0, 2).SetContent("Name of the Candidate ");
                if (cblsearch.Items[0].Selected == true)
                {
                    table1forpage1.Cell(0, 3).SetContent("Answer Booklet No");
                }
                if (cblsearch.Items[3].Selected == true)
                {
                    if (clct == 5)
                    {
                        table1forpage1.Cell(0, 4).SetContent("Signature of the Candidate");
                    }
                    else
                    {
                        table1forpage1.Cell(0, 3).SetContent("Signature of the Candidate");
                    }
                }
                int pagct = 0;
                int pagct1 = 1;
                coltop += 100;
                int row = 0;
                int modcount = 1;
                bool isFirstPagePrinted = false;
                int check = 0;
                int ct3=Convert.ToInt32(Subjectspread.Sheets[0].RowCount) - 3;
                double pgct = Convert.ToDouble(ct3) / 24;
              
                if (pgct.ToString().Length > 1)
                {
                    pgct = pgct + 1;
                    string[] spt = Convert.ToString(pgct).Split('.');
                    string pgc = spt[0].ToString();
                    pgct = Convert.ToDouble(pgc);
                   // pgct = Math.Round(pgct, 0, MidpointRounding.AwayFromZero);
                }
                //if (pgct == 1)
                //{
                //    pagct = 1;
                //}
                for (int m = 1; m <= Convert.ToInt32(Subjectspread.Sheets[0].RowCount - 3); m++)
                {
                    //if (m % 16 == 0)//Deepali 14.5.18
                    if (isFirstPagePrinted == false ? m % 25 == 0 : m == (modcount * 25) - check)
                    {
                        isFirstPagePrinted = true;
                        pagct++;
                        pagct1++;
                        if (rblPrintSettingsFooter.SelectedIndex.Equals(0))
                        {
                            PdfArea pa66 = new PdfArea(mydoc, 14, 12, 566, 821);
                            PdfRectangle pr77 = new PdfRectangle(mydoc, pa66, Color.Black);
                            mypdfpage.Add(pr77);
                            newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 22, 185, 550, 550));
                            mypdfpage.Add(newpdftabpage2);
                            if (CheckBox1.Checked == false)
                            {
                                if (chkincludenote.Checked == true)
                                {
                                    table1forpage1 = mydoc.NewTable(Fontco10, 1, 1, 1);
                                    table1forpage1.VisibleHeaders = false;
                                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpage1.Cell(0, 0).SetContent("Certified that the following particulars have been verified");
                                    newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, 738, 326, 200));
                                    mypdfpage.Add(newpdftabpage2);
                                    table1forpage1 = mydoc.NewTable(Fontco10a, 3, 1, 1);
                                    table1forpage1.VisibleHeaders = false;
                                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1forpage1.Cell(0, 0).SetContent("1.The Register No. in the attendance sheet with that in the hall ticket.");
                                    table1forpage1.Cell(1, 0).SetContent("2.The identification of the candidate with the photo pasted in the hall ticket");
                                    table1forpage1.Cell(2, 0).SetContent("3.The answer book number entered in the attendance sheet by the candidate");
                                    newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, 750, 500, 200));
                                    mypdfpage.Add(newpdftabpage2);

                                }
                                pa1 = new PdfArea(mydoc, 20, 735, 380, 90);//110
                                pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                                mypdfpage.Add(pr3);
                            }
                            table1forpage1 = mydoc.NewTable(Fontco10a, 2, 3, 1);
                            table1forpage1.VisibleHeaders = false;
                            table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage1.SetColumnsWidth(new int[] { 80, 10, 88 });
                            table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage1.Cell(0, 0).SetContent("Total Present");
                            table1forpage1.Cell(0, 1).SetContent(" ");
                            table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage1.Cell(0, 2).SetContent("                                             ");
                            table1forpage1.Cell(1, 0).SetContent("Total Absent");
                            table1forpage1.Cell(1, 1).SetContent(" ");
                            table1forpage1.Cell(1, 2).SetContent("                                              ");
                            newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 410, 740, 190, 100));
                            mypdfpage.Add(newpdftabpage2);
                            PdfTextArea pdpg = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 520, 795, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Page " + pagct + " of " + pgct + "");
                            mypdfpage.Add(pdpg);
                            PdfTextArea pdpg2 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 473, 815, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Printed On " + DateTime.Now.ToString("dd-MM-yyyy") + "");
                            mypdfpage.Add(pdpg2);
                            string sig = "select template from Master_Settings where settings='Exam Attendance Signature Settings' and usercode='" + Convert.ToString(Session["collegecode"]) + "'";
                            DataSet dssig = d2.select_method_wo_parameter(sig, "text");
                            string signatureleft = string.Empty;
                            string signatureright = string.Empty;
                            if (dssig.Tables.Count > 0 && dssig.Tables[0].Rows.Count > 0)
                            {
                                string sign1 = Convert.ToString(dssig.Tables[0].Rows[0]["template"]);
                                string[] spt = sign1.Split(';');
                                string sig2 = Convert.ToString(spt[0]);
                                signatureright = Convert.ToString(spt[1]);
                                if (sig2.Contains("$"))
                                {
                                    string[] spt1 = sig2.Split('$');
                                    int ct = 780;
                                    for (int j2 = 0; j2 < spt1.Length; j2++)
                                    {

                                        pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 26, ct, 595, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(spt1[j2]));
                                        mypdfpage.Add(pdf1);
                                        ct = ct + 18;
                                    }

                                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 225, 780, 595, 50), System.Drawing.ContentAlignment.TopLeft, signatureright);
                                    mypdfpage.Add(pdf1);
                                }
                                else
                                {
                                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 26, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, sig2);
                                    mypdfpage.Add(pdf1);
                                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 225, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, signatureright);
                                    mypdfpage.Add(pdf1);
                                }


                            }



                            //pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 26, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
                            //mypdfpage.Add(pdf1);
                            //pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 225, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
                            //mypdfpage.Add(pdf1);
                            pa1 = new PdfArea(mydoc, 480, 740, 73, 50);
                            pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                            mypdfpage.Add(pr3);
                            pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 754, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________");
                            mypdfpage.Add(pdf1);
                        }
                        else
                        {
                            PdfArea pa4 = new PdfArea(mydoc, 14, 12, 566, 821);
                            PdfRectangle pr5 = new PdfRectangle(mydoc, pa4, Color.Black);
                            mypdfpage.Add(pr5);
                            newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 22,190, 550, 550));//550, 600
                            mypdfpage.Add(newpdftabpage2);
                        }


                        //PdfArea pa4 = new PdfArea(mydoc, 14, 12, 566, 821);
                        //PdfRectangle pr5 = new PdfRectangle(mydoc, pa4, Color.Black);
                        //mypdfpage.Add(pr5);
                        //newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 22, coltop, 550, 600));//550, 600
                        //mypdfpage.Add(newpdftabpage2);
                        tblheight = (int)newpdftabpage2.Area.Height;
                        coltop += (int)tblheight + 25;
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydoc.NewPage();
                        coltop = 40;


                        if (rblPrintSettingsHeader.SelectedIndex.Equals(0))
                        {
                            PdfArea pa2 = new PdfArea(mydoc, 14, 12, 566, 821);
                            PdfRectangle pr4 = new PdfRectangle(mydoc, pa2, Color.Black);
                            mypdfpage.Add(pr4);
                            PdfTextArea pdf11;
                            string deptanddegree1 = Subjectspread.Sheets[0].Cells[0, 5].Text.ToString();
                            string SubjectCodeAndName1 = Subjectspread.Sheets[0].Cells[0, 1].Text + "$" + Subjectspread.Sheets[0].Cells[0, 1].Tag;
                            string[] splitSubjectCodeAndName1 = SubjectCodeAndName1.Split('$');
                            if (chkheadimage.Checked == true)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 20, 15, 400);
                                }
                            }
                            else
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + Session["collegecode"].ToString() + ".jpeg")))
                                {
                                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + Session["collegecode"].ToString() + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 21, 25, 450);
                                }
                                else if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 21, 25, 450);
                                }

                                string clm = "SELECT *  from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                                ds.Clear();
                                ds = da.select_method_wo_parameter(clm, "text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        pdf11 = new PdfTextArea(Fontco18, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
                                        mypdfpage.Add(pdf11);
                                        string distpinspp = ds.Tables[0].Rows[0]["district"].ToString() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
                                        string distpin =  Convert.ToString(ds.Tables[0].Rows[0]["category"]) +" - "+ ds.Tables[0].Rows[0]["affliatedby"].ToString();
                                        string[] splitdistpin = distpin.Split(',');
                                        pdf11 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + splitdistpin[0] + "");
                                        mypdfpage.Add(pdf11);
                                        pdf11 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 60, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Office of the Controller of Examinations");
                                        mypdfpage.Add(pdf11);
                                        pdf11 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 75, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + distpinspp + "");
                                        mypdfpage.Add(pdf11);
                                    }
                                }
                            }
                            pdf11 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + examstrMonthNameyear + "");
                            mypdfpage.Add(pdf11);
                            pdf11 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 13, 95, 750, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________");
                            mypdfpage.Add(pdf11);


                            Gios.Pdf.PdfTable table1forpage11 = mydoc.NewTable(Fontco10a, 3, 5, 4.5);
                            table1forpage11.VisibleHeaders = false;
                            table1forpage11.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage11.SetColumnsWidth(new int[] { 20, 5, 50 });
                            table1forpage11.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage11.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage11.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage11.Cell(0, 0).SetContent("Degree & Branch");
                            table1forpage11.Cell(0, 1).SetContent(":");
                            table1forpage11.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage11.Cell(0, 2).SetContent(deptanddegree1);
                            table1forpage11.Cell(2, 0).SetContent("Subject Name");
                            table1forpage11.Cell(2, 1).SetContent(":");
                            table1forpage11.Cell(2, 2).SetContent(splitSubjectCodeAndName1[0].ToString());
                            table1forpage11.Cell(1, 0).SetContent("Subject Code ");
                            table1forpage11.Cell(1, 1).SetContent(":");
                            table1forpage11.Cell(1, 2).SetContent(splitSubjectCodeAndName1[1].ToString());
                            table1forpage11.Cell(0, 3).SetContent("                      ");
                            table1forpage11.Cell(1, 3).SetContent("                        ");
                            table1forpage11.Cell(2, 3).SetContent("                      ");
                            Gios.Pdf.PdfTablePage newpdftabpage22 = table1forpage11.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, 105, 550, 270));
                            mypdfpage.Add(newpdftabpage22);
                            table1forpage11 = mydoc.NewTable(Fontco10a, 3, 5, 1);
                            table1forpage11.VisibleHeaders = false;
                            table1forpage11.SetBorders(Color.Black, 1, BorderType.None);
                            table1forpage11.SetColumnsWidth(new int[] { 65, 10, 88 });
                            table1forpage11.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage11.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage11.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage11.Cell(0, 0).SetContent("Semester");
                            table1forpage11.Cell(0, 1).SetContent(":");
                            table1forpage11.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //table1forpage11.Cell(0, 2).SetContent(dsm.Tables[0].Rows[0]["current_semester"].ToString());
                            table1forpage11.Cell(0, 2).SetContent(cur_sem);
                            table1forpage11.Cell(1, 0).SetContent("Date of Exam/Session");
                            table1forpage11.Cell(1, 1).SetContent(":");
                            table1forpage11.Cell(1, 2).SetContent(yersessn);
                            table1forpage11.Cell(2, 0).SetContent("Room No / Bundle No ");
                            table1forpage11.Cell(2, 1).SetContent(":");
                            table1forpage11.Cell(2, 2).SetContent("" + " / " + "");
                            table1forpage11.Cell(0, 3).SetContent("                      ");
                            table1forpage11.Cell(1, 3).SetContent("                        ");
                            table1forpage11.Cell(2, 3).SetContent("                      ");
                            newpdftabpage22 = table1forpage11.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 350, 105, 326, 160));
                            mypdfpage.Add(newpdftabpage22);


                        }
                        //if (Convert.ToInt32(Subjectspread.Sheets[0].RowCount - 3) + 3 - (modcount * 16) > 17)
                        //    tblcount = 18;
                        //else
                        //    tblcount = Convert.ToInt32(Subjectspread.Sheets[0].RowCount - 3) + 3 - (modcount * 16);

                        if (Convert.ToInt32(Subjectspread.Sheets[0].RowCount - 3) - (modcount * 24) > 25)//deepali 12.5.18
                            tblcount = 25;
                        else
                            tblcount = (Convert.ToInt32(Subjectspread.Sheets[0].RowCount - 3) - (modcount * 24)) + 1;
                        int clct1 = 3;
                        if (cblsearch.Items[3].Selected == true)
                        {
                            clct1++;
                        }
                        if (cblsearch.Items[0].Selected == true)
                        {
                            clct1++;
                        }
                        table1forpage1 = mydoc.NewTable(Fontco10a, tblcount, clct1, 1);
                        table1forpage1.VisibleHeaders = false;
                        if (clct == 5)
                        {
                            table1forpage1.SetColumnsWidth(new int[] { 40, 130, 250, 220, 200 });
                            table1forpage1.Columns[4].SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage1.Cell(0, 4).SetCellPadding(4);
                        }
                        else
                        {
                            table1forpage1.SetColumnsWidth(new int[] { 40, 130, 250, 220 });

                        }
                        table1forpage1.SetBorders(System.Drawing.Color.Black, 1, BorderType.CompleteGrid);
                        table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);

                        table1forpage1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage1.Columns[3].SetContentAlignment(ContentAlignment.MiddleCenter);//deepali 16.5.18
                        //table1forpage1.SetColumnsWidth(new int[] { 65, 10, 88 });
                        //table1forpage1.Columns[0].SetWidth(300);
                        //table1forpage1.Columns[1].SetWidth(100);

                        //deepali 16.5.18=========================
                        table1forpage1.Cell(0, 0).SetCellPadding(4);
                        table1forpage1.Cell(0, 1).SetCellPadding(4);
                        table1forpage1.Cell(0, 2).SetCellPadding(4);
                        table1forpage1.Cell(0, 3).SetCellPadding(4);

                        //================================
                        table1forpage1.Cell(0, 0).SetContent("S.No");
                        table1forpage1.Cell(0, 1).SetContent("Register Number");
                        table1forpage1.Cell(0, 2).SetContent("Name of the Candidate ");
                        //table1forpage1.Cell(0, 3).SetContent("Answer Booklet No");
                        //table1forpage1.Cell(0, 4).SetContent("Signature of the Candidate");

                        if (cblsearch.Items[0].Selected == true)
                        {
                            table1forpage1.Cell(0, 3).SetContent("Answer Booklet No");
                        }
                        if (cblsearch.Items[3].Selected == true)
                        {
                            if (clct1 == 5)
                            {
                                table1forpage1.Cell(0, 4).SetContent("Signature of the Candidate");
                            }
                            else
                            {
                                table1forpage1.Cell(0, 3).SetContent("Signature of the Candidate");
                            }
                        }


                        row = 0;
                        modcount++;
                        check++;
                    }

                    table1forpage1.Cell(row + 1, 0).SetContent(Convert.ToString(Subjectspread.Sheets[0].Cells[m - 1, 0].Text));
                    table1forpage1.Cell(row + 1, 1).SetContent(Convert.ToString(Subjectspread.Sheets[0].Cells[m - 1, 2].Text));
                    table1forpage1.Cell(row + 1, 2).SetContent("  " + Convert.ToString(Subjectspread.Sheets[0].Cells[m - 1, 3].Text));
                    if (cblsearch.Items[0].Selected == true)
                    {
                        table1forpage1.Cell(row + 1, 3).SetContent(Convert.ToString(Subjectspread.Sheets[0].Cells[m - 1, 8].Text));
                    }
                    if (cblsearch.Items[3].Selected == true)
                    {
                        if (clct == 5)
                        {
                            table1forpage1.Cell(row + 1, 4).SetContent("                                                                                                 ");
                        }
                        else
                        {
                            table1forpage1.Cell(row + 1, 3).SetContent("                                                                                                 ");
                        }
                    }
                   
                    row++;
                }
                PdfTextArea pdpg1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 520, 795, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Page " + pagct1 + " of " + pgct + "");
                mypdfpage.Add(pdpg1);
                PdfTextArea pdpg21 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 473, 815, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Printed On " + DateTime.Now.ToString("dd-MM-yyyy") + "");
                mypdfpage.Add(pdpg21);

            }

            PdfArea pa6 = new PdfArea(mydoc, 14, 12, 566, 821);
            PdfRectangle pr7 = new PdfRectangle(mydoc, pa6, Color.Black);
            mypdfpage.Add(pr7);

            newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 22, 185, 550, 550));
            mypdfpage.Add(newpdftabpage2);
            if (CheckBox1.Checked == false)
            {
                if (chkincludenote.Checked == true)
                {
                    table1forpage1 = mydoc.NewTable(Fontco10, 1, 1, 1);
                    table1forpage1.VisibleHeaders = false;
                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Cell(0, 0).SetContent("Certified that the following particulars have been verified");
                    newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, 738, 326, 200));
                    mypdfpage.Add(newpdftabpage2);
                    table1forpage1 = mydoc.NewTable(Fontco10a, 3, 1, 1);
                    table1forpage1.VisibleHeaders = false;
                    table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
                    table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                    table1forpage1.Cell(0, 0).SetContent("1.The Register No. in the attendance sheet with that in the hall ticket.");
                    table1forpage1.Cell(1, 0).SetContent("2.The identification of the candidate with the photo pasted in the hall ticket");
                    table1forpage1.Cell(2, 0).SetContent("3.The answer book number entered in the attendance sheet by the candidate");
                    newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, 750, 500, 200));
                    mypdfpage.Add(newpdftabpage2);
                }

                pa1 = new PdfArea(mydoc, 20, 735, 380, 90);//110
                pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                mypdfpage.Add(pr3);
            }
            table1forpage1 = mydoc.NewTable(Fontco10a, 2, 3, 1);
            table1forpage1.VisibleHeaders = false;
            table1forpage1.SetBorders(Color.Black, 1, BorderType.None);
            table1forpage1.SetColumnsWidth(new int[] { 80, 10, 88 });
            table1forpage1.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
            table1forpage1.Columns[2].SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage1.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage1.Cell(0, 0).SetContent("Total Present");
            table1forpage1.Cell(0, 1).SetContent(" ");
            table1forpage1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
            table1forpage1.Cell(0, 2).SetContent("                                             ");
            table1forpage1.Cell(1, 0).SetContent("Total Absent");
            table1forpage1.Cell(1, 1).SetContent(" ");
            table1forpage1.Cell(1, 2).SetContent("                                              ");
            newpdftabpage2 = table1forpage1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 410, 740, 190, 100));
            mypdfpage.Add(newpdftabpage2);
            //if (CheckBox1.Checked == false)
            //{
            string sig3 = "select template from Master_Settings where settings='Exam Attendance Signature Settings' and usercode='" + Convert.ToString(Session["collegecode"]) + "'";
            DataSet dssig3 = d2.select_method_wo_parameter(sig3, "text");
            string signatureleft3 = string.Empty;
            string signatureright3 = string.Empty;
            if (dssig3.Tables.Count > 0 && dssig3.Tables[0].Rows.Count > 0)
            {
                string sign1 = Convert.ToString(dssig3.Tables[0].Rows[0]["template"]);
                string[] spt = sign1.Split(';');
                string sig2 = Convert.ToString(spt[0]);
                signatureright3 = Convert.ToString(spt[1]);
                if (sig2.Contains("$"))
                {
                    string[] spt1 = sig2.Split('$');
                    int ct = 780;
                    for (int j2 = 0; j2 < spt1.Length; j2++)
                    {

                        pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 26, ct, 595, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(spt1[j2]));
                        mypdfpage.Add(pdf1);
                        ct = ct + 18;
                    }

                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 225, 780, 595, 50), System.Drawing.ContentAlignment.TopLeft, signatureright3);
                    mypdfpage.Add(pdf1);
                }
                else
                {
                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 26, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, sig2);
                    mypdfpage.Add(pdf1);
                    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 225, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, signatureright3);
                    mypdfpage.Add(pdf1);
                }


            }
            //pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 26, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
            //mypdfpage.Add(pdf1);
            //pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 225, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
            //mypdfpage.Add(pdf1);
            //  }
            //else
            //{
            //    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 26, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Invigilator");
            //    mypdfpage.Add(pdf1);
            //    pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 435, 810, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of Chief Invigilator");
            //    mypdfpage.Add(pdf1);
            //}
            pa1 = new PdfArea(mydoc, 480, 740, 73, 50);
            pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
            mypdfpage.Add(pr3);
            pdf1 = new PdfTextArea(Fontco10a, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 754, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________");
            mypdfpage.Add(pdf1);
            mypdfpage.SaveToDocument();
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Attendance12" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
            //string degreedetails = "Subjectwise details" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");
            //string pagename = "ExamattendanceReport.aspx";
            //Printcontrol.loadspreaddetails(Subjectspread, pagename, degreedetails);
            //Printcontrol.Visible = true;
        }
        catch
        {

        }
    }

    protected void chkBatch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ddlBatch.Items.Clear();
            if (chkBatch.Checked)
            {
                ddlBatch.Visible = true;
                string examdate = ddlfrmdate.SelectedValue.ToString();
                string[] dsplit = examdate.Split('-');
                examdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();

                string sessiond = string.Empty;
                if (ddlsession.SelectedItem.Text == "Both")
                    sessiond = string.Empty;
                else
                    sessiond = "   and examsession like '" + ddlsession.SelectedItem.Text + "%'";

                string subNo = "select distinct batch from examtheorybatch  where  ExamDate='" + examdate + "' " + sessiond;
                DataTable dtSubject = dirAcc.selectDataTable(subNo);
                if (dtSubject.Rows.Count > 0)
                {
                    ddlBatch.DataSource = dtSubject;
                    ddlBatch.DataTextField = "batch";
                    ddlBatch.DataValueField = "batch";
                    ddlBatch.DataBind();
                }
            }
            else
                ddlBatch.Visible = false;
        }
        catch
        {
        }
    }

    //Rajkumar 20/1/2018===================

    #region Added By Malang Raja on Nov 08 2016

    public void FoilCard()
    {
        try
        {
            divDummyNoSheets.Visible = false;
            lblDispErr.Visible = false;
            lblDispErr.Text = string.Empty;
            AttSpread.SaveChanges();
            int g = 1;
            string collgr = string.Empty;
            string affilitied = string.Empty;
            string collname = string.Empty;
            string pincode = string.Empty;
            string district = string.Empty;
            string Date = string.Empty;
            int mm = 0;
            int y = 0;
            string HallNo = string.Empty;
            string session = string.Empty;
            string hdeg = "", hroll = "", bndlee = string.Empty;
            string batch = string.Empty;
            string subno = string.Empty;
            string hall = string.Empty;
            DataSet dsdisplay = new DataSet();

            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);
            Font fontCoverNo = new Font("IDAutomationHC39M", 10, FontStyle.Bold);
            Boolean chkgenflag = false;
            DateTime dt = new DateTime();
            int coltop = 10;
            coltop = coltop + 5;
            int coltop1 = coltop;
            int finctop = coltop;
            int yq = 180;
            string strquery = string.Empty;
            int isval = 0;
            int ji = 0;
            int tablepadding = 10;
            strquery = "Select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
            string sml = d2.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
            if (sml.Trim() != "" && sml.Trim() != "0")
            {
                if (Convert.ToInt32(sml) > 15)
                {
                    tablepadding = 3;
                }
                else
                {
                    tablepadding = 10;
                }
                {
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        collname = ds.Tables[0].Rows[0]["collname"].ToString();
                        affilitied = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        district = ds.Tables[0].Rows[0]["district"].ToString();
                        pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                        string[] aff = affilitied.Split(',');
                        affilitied = aff[0].ToString();
                        boundvl.Clear();
                        HasSession.Clear();
                        Hasdegree.Clear();
                        Hashdenm.Clear();
                        Hashhall.Clear();
                        HashDate.Clear();
                        Hasroll.Clear();
                        hassubno.Clear();
                        hasbatch.Clear();
                        int u = 0;
                        for (mm = 0; mm < AttSpread.Sheets[0].Rows.Count; mm++)
                        {
                            isval = Convert.ToInt32(AttSpread.Sheets[0].Cells[u, 1].Value);
                            u = u + 1;
                            if (isval == 1 && u > 1)
                            {
                                y = y + 1;
                                chkgenflag = true;
                                lblerr1.Visible = false;
                                lblerr1.Text = string.Empty;
                                coltop = 10;
                                hall = AttSpread.Sheets[0].Cells[u - 1, 2].Text.ToString();
                                dt = Convert.ToDateTime(AttSpread.Sheets[0].Cells[u - 1, 2].Note.ToString());
                                PdfArea tete = new PdfArea(mydocument, 15, 10, 565, 825);
                                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                Date = AttSpread.Sheets[0].Cells[u - 1, 2].Note.ToString();
                                session = AttSpread.Sheets[0].Cells[u - 1, 4].Note.ToString();
                                HallNo = AttSpread.Sheets[0].Cells[u - 1, 2].Text.ToString();
                                hdeg = AttSpread.Sheets[0].Cells[u - 1, 8].Text.ToString();
                                hroll = AttSpread.Sheets[0].Cells[u - 1, 6].Text.ToString();
                                //hroll = "'" + hroll + "'  and  '" + AttSpread.Sheets[0].Cells[u - 1, 7].Text.ToString() + "'";
                                bndlee = AttSpread.Sheets[0].Cells[u - 1, 5].Text.ToString();
                                batch = AttSpread.Sheets[0].Cells[u - 1, 3].Note.ToString();
                                subno = AttSpread.Sheets[0].Cells[u - 1, 0].Note.ToString();
                                string[] dummy_date_split = Date.Split(' ');
                                string[] dsplit = dummy_date_split[0].Split('/');
                                Date = dsplit[2].ToString() + "-" + dsplit[0].ToString() + "-" + dsplit[1].ToString();
                                collgr = Session["collegecode"].ToString();
                                // query = "select r.Reg_No,r.Stud_Name,r.Stud_Type,r.current_semester,s.subject_code,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym)  as Grade,sc.semester,(s.subject_code +'-'+ s.subject_name) as subjectname ,(c.Course_Name +'-'+ de.dept_name) as deptname from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject s,Degree d,course c,Department de,subjectchooser sc where sc.subject_no=s.subject_no and sc.roll_no=r.roll_no and ea.roll_no=sc.roll_no and ead.subject_no=sc.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and r.degree_code=d.Degree_Code and ed.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and es.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.edate='" + Date + "' and es.ses_sion ='" + session + "' and r.degree_code='" + hdeg + "' and r.batch_year='" + batch + "' and roomno='" + HallNo + "' and es.subject_no  ='" + subno + "' and r.Reg_No in ('" + AttSpread.Sheets[0].Cells[u - 1, 7].Tag.ToString() + "') order by es.seat_no";
                                //string query = "select r.Reg_No,r.Stud_Name,r.Stud_Type,r.current_semester,s.subject_code,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym)  as Grade,(s.subject_code +'-'+ s.subject_name) as subjectname ,(c.Course_Name +'-'+ de.dept_name) as deptname from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject s,Degree d,course c,Department de where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and r.degree_code=d.Degree_Code and ed.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and es.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.edate='" + Date + "' and es.ses_sion ='" + session + "' and r.degree_code='" + hdeg + "' and r.batch_year='" + batch + "' and roomno='" + HallNo + "' and es.subject_no  ='" + subno + "' and r.Reg_No in ('" + AttSpread.Sheets[0].Cells[u - 1, 7].Tag.ToString() + "') order by es.seat_no";

                                string query = "select distinct r.Reg_No,r.Stud_Name,r.Stud_Type,r.current_semester,s.subject_code,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym)  as Grade,sc.semester,(s.subject_code +'-'+ s.subject_name) as subjectname ,(c.Course_Name +'-'+ de.dept_name) as deptname,es.bundle_no,r.degree_code,r.batch_year,r.college_code from Exam_Details ed,exam_application ea,exam_appl_details ead ,exam_seating es,Registration r,subject s,Degree d,course c,Department de,subjectchooser sc where sc.subject_no=s.subject_no and sc.roll_no=r.roll_no and ea.roll_no=sc.roll_no and ead.subject_no=sc.subject_no and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and r.degree_code=d.Degree_Code and ed.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and ead.subject_no=s.subject_no and es.subject_no=s.subject_no and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and es.edate='" + Date + "' and es.ses_sion ='" + session + "' and r.degree_code='" + hdeg + "' and r.batch_year='" + batch + "' and roomno='" + HallNo + "' and es.subject_no  ='" + subno + "' and r.Reg_No in ('" + AttSpread.Sheets[0].Cells[u - 1, 7].Tag.ToString() + "') order by es.seat_no";
                                dsdisplay = d2.select_method_wo_parameter(query, "text");
                                PdfTextArea ptc;
                                //Gios.Pdf.PdfTable tbltCover = mydocument.NewTable(Fontbold, 2, 1, 15);
                                //tbltCover.VisibleHeaders = false;
                                //tbltCover.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                //tbltCover.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);s
                                //tbltCover.Cell(0, 0).SetContent("Cover No");
                                //tbltCover.Cell(0, 0).SetCellPadding(5);
                                //tbltCover.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                //tbltCover.Cell(1, 0).SetContent("Cover No");
                                //tbltCover.Cell(1, 0).SetCellPadding(5);
                                //tbltCover.Columns[0].SetWidth(50);
                                //Gios.Pdf.PdfTablePage tblPageCover = tbltCover.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, mydocument.PageWidth - 100, 20, 80, 80));
                                //mypdfpage.Add(tblPageCover);




                                if (chkheadimage.Checked == true)
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                                        mypdfpage.Add(LogoImage, 18, 18, 480);
                                    }
                                    coltop = 30;
                                }
                                else
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 35, 25, 700);
                                    }
                                    ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                    mypdfpage.Add(ptc);
                                    coltop = coltop + 15;
                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("Office of the Controller of Examinations").ToUpper());
                                    mypdfpage.Add(ptc);
                                }
                                coltop = coltop + 15;
                                ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "FOIL CARD FOR THE END SEMESTER EXAMINATIONS" + "-" + Convert.ToString(ddlMonth.SelectedItem.Text).ToUpper() + " " + ddlYear.SelectedItem.Text + "");
                                mypdfpage.Add(ptc);//FOIL CARD FOR THE END OF SEMESTER EXAMINATIONS-
                                coltop = coltop + 10;
                                ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                mypdfpage.Add(ptc);

                                Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, dsdisplay.Tables[0].Rows.Count + 1, 4, 10);
                                table1.VisibleHeaders = false;
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetContent("S.No");
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetFont(Fontbold);
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetContent("Register Number");
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetFont(Fontbold);
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetContent("Marks In Figures");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetFont(Fontbold);
                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 3).SetContent("Marks In Words");
                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 3).SetFont(Fontbold);
                                //table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                //table1.Cell(0, 4).SetContent("Signature of Candidate");
                                //table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                //table1.Cell(0, 4).SetFont(Fontbold);
                                table1.Columns[0].SetWidth(20);
                                table1.Columns[1].SetWidth(40);
                                table1.Columns[2].SetWidth(100);
                                table1.Columns[3].SetWidth(150);
                                //table1.Columns[4].SetWidth(60);
                                if (dsdisplay.Tables[0].Rows.Count > 0)
                                {
                                    DataTable dtBundleList = new DataTable();
                                    dtBundleList = dsdisplay.Tables[0].DefaultView.ToTable(true, "college_code", "batch_year", "degree_code", "deptname", "semester", "subject_code", "subjectname", "bundle_no");//"batch_year", "degree_code"
                                    for (ji = 0; ji < dtBundleList.Rows.Count; ji++)
                                    {
                                        string degcode = Convert.ToString(dtBundleList.Rows[ji]["degree_code"]).Trim();
                                        string batchYear = Convert.ToString(dtBundleList.Rows[ji]["batch_year"]).Trim();
                                        string deptname = Convert.ToString(dtBundleList.Rows[ji]["deptname"]).Trim();
                                        string semester = Convert.ToString(dtBundleList.Rows[ji]["semester"]).Trim();
                                        string sub_code = Convert.ToString(dtBundleList.Rows[ji]["subject_code"]).Trim();
                                        string sub_name = Convert.ToString(dtBundleList.Rows[ji]["subjectname"]).Trim();
                                        string bundleNo = Convert.ToString(dtBundleList.Rows[ji]["bundle_no"]).Trim();
                                        string CollegeCode = Convert.ToString(dtBundleList.Rows[ji]["college_code"]).Trim();

                                        #region Dummy Number Display
                                        byte dummyNumberMode = getDummyNumberMode(CollegeCode);//0-serial , 1-random
                                        string dummyNumberType = string.Empty;

                                        if (DummyNumberType(CollegeCode) == 1)
                                        {
                                            dummyNumberType = " and subject='" + sub_code + "' ";
                                        }
                                        else
                                        {
                                            dummyNumberType = " and subject='" + sub_code + "' ";
                                            //dummyNumberType = " and ltrim(rtrim( isnull(subject,'')))='' ";
                                        }
                                        string selDummyQ = string.Empty;
                                        if (chksubwise.Checked == false)
                                        {
                                            selDummyQ = "select dummy_no,regno,roll_no from dummynumber where exam_month='" + Convert.ToString(ddlMonth.SelectedValue) + "' and exam_year='" + Convert.ToString(ddlYear.SelectedValue) + "' and DNCollegeCode='" + CollegeCode + "'-- and degreecode='" + degcode + "' " + dummyNumberType + "  and dummy_type='" + dummyNumberMode + "' --  and semester='" + semester + "' and exam_date='11/01/2016' ";
                                        }
                                        else if (chksubwise.Checked == true)
                                        {
                                            selDummyQ = "select dummy_no,regno,roll_no from dummynumber where exam_month='" + Convert.ToString(ddlMonth.SelectedValue) + "' and exam_year='" + Convert.ToString(ddlYear.SelectedValue) + "' and DNCollegeCode='" + CollegeCode + "' " + dummyNumberType + "  and dummy_type='" + dummyNumberMode + "' --  and semester='" + semester + "' and exam_date='11/01/2016' and degreecode='" + degcode + "'";

                                        }

                                        DataTable dtMappedNumbers = dirAcc.selectDataTable(selDummyQ);
                                        bool showDummyNumber = ShowDummyNumber(CollegeCode);
                                        if (showDummyNumber)
                                        {
                                            if (dtMappedNumbers.Rows.Count == 0)
                                            {
                                                //lblAlertMsg.Visible = true;
                                                //lblAlertMsg.Text = "No Dummy Numbers Generated";
                                                //divPopAlert.Visible = true;
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Dummy Numbers Generated')", true);
                                                //return;
                                            }
                                        }
                                        #endregion
                                        //Gios.Pdf.PdfTable tbltCover = mydocument.NewTable(Fontbold, 2, 1, 15);
                                        //tbltCover.VisibleHeaders = false;
                                        //tbltCover.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        //tbltCover.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //tbltCover.Cell(0, 0).SetContent("Bundle No");
                                        //tbltCover.Cell(0, 0).SetCellPadding(5);
                                        //tbltCover.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //tbltCover.Cell(1, 0).SetContent(bundleNo);
                                        //tbltCover.Cell(1, 0).SetFont(fontCoverNo);
                                        //tbltCover.Cell(1, 0).SetCellPadding(5);
                                        //tbltCover.Columns[0].SetWidth(50);

                                        string barCode = bundleNo;
                                        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();

                                        using (Bitmap bitMap = new Bitmap(barCode.Length * 40, 80))
                                        {
                                            using (Graphics graphics = Graphics.FromImage(bitMap))
                                            {
                                                Font oFont = new Font("IDAutomationHC39M", 16);
                                                PointF point = new PointF(2f, 2f);
                                                SolidBrush blackBrush = new SolidBrush(Color.Black);
                                                //SolidBrush whiteBrush = new SolidBrush(Color.White);
                                                graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                                                graphics.DrawString("*" + barCode + "*", oFont, blackBrush, point);
                                            }
                                            using (MemoryStream ms = new MemoryStream())
                                            {
                                                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                                byte[] byteImage = ms.ToArray();

                                                ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 490, 30, 70, 10), System.Drawing.ContentAlignment.MiddleCenter, "Cover No");
                                                mypdfpage.Add(ptc);

                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/BarCode/" + bundleNo + ".jpeg")))
                                                {

                                                    PdfImage LogoImage1 = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/BarCode/" + bundleNo + ".jpeg"));
                                                    mypdfpage.Add(LogoImage1, 500, 45, 200);
                                                }
                                                else
                                                {

                                                    File.WriteAllBytes(Server.MapPath("~/BarCode/" + bundleNo + ".jpeg"), byteImage);

                                                    //FileInfo filinfo = new FileInfo();
                                                    //filinfo.Refresh();
                                                    DirectoryInfo dir = new DirectoryInfo("~/BarCode/" + bundleNo + ".jpeg");
                                                    dir.Refresh();
                                                    ms.Dispose();
                                                    ms.Close();
                                                    PdfImage LogoImage1 = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/BarCode/" + bundleNo + ".jpeg"));
                                                    mypdfpage.Add(LogoImage1, 500, 45, 200);
                                                }
                                            }


                                        }

                                        string[] sub = sub_name.Split('-');
                                        string subname = sub[1].ToString();
                                        coltop = coltop + 35;
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, ":  " + deptname);
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + semester);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 25;
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + sub_code);
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Exam /Session");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + ddlfrmdate.SelectedItem.Text + "/" + session);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 25;
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 140, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ":  ");
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 150, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, subname);
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Hall No");//"Room No / Bundle No"
                                        mypdfpage.Add(ptc);
                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + hall);//":  " + hall + " / " + bndlee
                                        mypdfpage.Add(ptc);
                                        for (ji = 1; ji <= dsdisplay.Tables[0].Rows.Count; ji++)
                                        {
                                            string regno = dsdisplay.Tables[0].Rows[ji - 1]["Reg_No"].ToString();
                                            if (!showDummyNumber)
                                            {
                                                dtMappedNumbers.DefaultView.RowFilter = "regno='" + regno + "'";
                                                DataView dvDumNo = dtMappedNumbers.DefaultView;
                                                if (dvDumNo.Count > 0)
                                                {
                                                    regno = string.Empty;
                                                    regno = dvDumNo[0]["dummy_no"].ToString();
                                                }
                                            }

                                            string name = dsdisplay.Tables[0].Rows[ji - 1]["Stud_Name"].ToString();
                                            string roomno = dsdisplay.Tables[0].Rows[ji - 1]["roomno"].ToString();
                                            string seatno = dsdisplay.Tables[0].Rows[ji - 1]["seat_no"].ToString();
                                            table1.Cell(g, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(g, 0).SetContent(g.ToString());
                                            table1.Cell(g, 0).SetFont(Fontnormal);
                                            table1.Cell(g, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(g, 1).SetContent(regno.ToString());
                                            table1.Cell(g, 1).SetFont(Fontnormal);
                                            g = g + 1;
                                        }
                                        Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, 750));
                                        mypdfpage.Add(newpdftabpage1);
                                        mypdfpage.Add(pr1);
                                        PdfTextArea pdfSignExaminer = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 25, 750, 200, 50), ContentAlignment.MiddleLeft, "Signature of the Examiner");
                                        mypdfpage.Add(pdfSignExaminer);
                                        PdfTextArea pdfDate = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 25, 775, 200, 50), ContentAlignment.MiddleLeft, "Date\t\t:\t\t");
                                        mypdfpage.Add(pdfDate);
                                        PdfTextArea pdfSignChairman = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 350, 750, 200, 50), ContentAlignment.MiddleRight, "Signature of the Chairman");
                                        mypdfpage.Add(pdfSignChairman);
                                        g = 1;
                                        if (yq >= 180)
                                        {
                                            mypdfpage.SaveToDocument();
                                            mypdfpage = mydocument.NewPage();
                                            yq = 180;
                                        }
                                    }
                                    string appPath = HttpContext.Current.Server.MapPath("~");
                                    if (appPath != "")
                                    {
                                        string szPath = appPath + "/Report/";
                                        string szFile = "FoilCardSheet_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                                        mydocument.SaveToFile(szPath + szFile);
                                        Response.ClearHeaders();
                                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                        Response.ContentType = "application/pdf";
                                        Response.WriteFile(szPath + szFile);
                                    }
                                }
                                else
                                {
                                    lblerror1.Visible = true;
                                    lblerror1.Text = "No Records Found";
                                }
                            }
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                }
                if (chkgenflag == false)
                {
                    lblerror1.Visible = true;
                    lblerror1.Text = "Please Select Any One Record";
                }
            }
            else
            {
                ArrayList arr_subjectunique = new ArrayList();
                if (sml.Trim() != "0")
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            collname = ds.Tables[0].Rows[0]["collname"].ToString();
                            affilitied = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                            district = ds.Tables[0].Rows[0]["district"].ToString();
                            pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                            string sessiond1 = string.Empty;
                            if (ddlsession.SelectedItem.Text == "Both")
                            {
                                sessiond1 = string.Empty;
                            }
                            else
                            {
                                sessiond1 = "  and es.ses_sion like'" + ddlsession.SelectedItem.Text + "%'";
                            }
                            string[] aff = affilitied.Split(',');
                            affilitied = aff[0].ToString();
                            string datess = ddlfrmdate.SelectedItem.Text;
                            string[] fromdatespit99 = datess.ToString().Split('-');
                            datess = fromdatespit99[2] + '-' + fromdatespit99[1] + '-' + fromdatespit99[0];
                            //string overall = "select distinct  top 40 es.roomno,COUNT(1) as strength,es.ses_sion,es.edate  from registration r,subjectchooser sc,exam_seating as es where sc.roll_no=r.roll_no  and exam_flag<>'Debar' and es.regno=r.Reg_No and es.subject_no=sc.subject_no " + sessiond1 + "  group by es.roomno,es.ses_sion,es.edate  ";
                            string overall = "select distinct   es.roomno,COUNT(1) as strength,es.ses_sion,es.edate  from registration r,subjectchooser sc,exam_seating as es where sc.roll_no=r.roll_no  and exam_flag<>'Debar' and es.regno=r.Reg_No and es.subject_no=sc.subject_no and es.edate='" + datess + "' " + sessiond1 + "  group by es.roomno,es.ses_sion,es.edate  ";
                            DataSet dsoverall = new DataSet();
                            dsoverall = d2.select_method_wo_parameter(overall, "text");
                            int u = 0;
                            int startrow = 0;
                            int tablerowscount = 0;
                            for (int sew = 0; sew < AttSpread.Sheets[0].Rows.Count; sew++)
                            {
                                isval = Convert.ToInt16(AttSpread.Sheets[0].Cells[u, 1].Value);
                                u = u + 1;
                                if (isval == 1)
                                {
                                    int we = 1;
                                    Gios.Pdf.PdfTable tbltCover = mydocument.NewTable(Fontbold, 2, 1, 15);
                                    tbltCover.VisibleHeaders = false;
                                    tbltCover.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    tbltCover.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tbltCover.Cell(0, 0).SetContent("Cover No");
                                    tbltCover.Cell(0, 0).SetCellPadding(5);
                                    tbltCover.Columns[0].SetWidth(50);
                                    Gios.Pdf.PdfTablePage tblPageCover = tbltCover.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, mydocument.PageWidth - 100, 20, 80, 80));
                                    mypdfpage.Add(tblPageCover);
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                    {
                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 35, 25, 700);
                                    }
                                    if (dsoverall.Tables.Count > 0 && dsoverall.Tables[0].Rows.Count > 0)
                                    {
                                        coltop = 10;
                                        PdfTextArea ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 15;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("Office of the Controller of Examinations").ToUpper());
                                        mypdfpage.Add(ptc);
                                        //coltop = coltop + 15;
                                        //ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                        //                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
                                        //mypdfpage.Add(ptc);
                                        coltop = coltop + 15;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "FOIL CARD FOR THE END SEMESTER EXAMINATIONS" + "-" + Convert.ToString(ddlMonth.SelectedItem.Text).ToUpper() + " " + ddlYear.SelectedItem.Text + "");
                                        mypdfpage.Add(ptc);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + 10;
                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                        mypdfpage.Add(ptc);
                                        string roomnoco = dsoverall.Tables[0].Rows[sew]["roomno"].ToString();
                                        string queryreg = string.Empty;
                                        queryreg = "select distinct  sub.subject_no,r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sc.semester,sub.subject_code,sub.subject_name ,c.Course_Name, (select dept_name from Department where d.dept_code=Dept_Code) as deptname,r.degree_code  from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "' and es.edate='" + datess + "'  and es.ses_sion like '" + ddlsession.SelectedItem.Text + "%'  order by es.seat_no";
                                        //  queryreg = "select distinct  top 102 r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sub.subject_code,sub.subject_name   from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "'   and es.ses_sion='" + ddlsession.SelectedItem.Text + "'  order by es.seat_no";
                                        //queryreg = "select distinct  r.Reg_No,r.Current_Semester,r.Stud_Name,r.Stud_Type,es.seat_no,es.roomno,(c.Course_Name +'-'+ d.Acronym) as Grade,(sub.subject_code +'-'+ sub.subject_name) as subjectname,sub.subject_code,sub.subject_name   from registration r,subjectchooser sc,exam_seating as es ,Degree d,course c,Department de,subject sub where sc.subject_no=sub.subject_no and sc.roll_no=r.roll_no  and delflag=0 and exam_flag<>'Debar' and es.regno=r.Reg_No and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and es.subject_no=sc.subject_no and es.roomno='" + roomnoco + "' and es.edate='" + datess + "' and es.ses_sion='" + ddlsession.SelectedItem.Text + "'  order by es.seat_no";
                                        DataSet dschecksubjlist = new DataSet();
                                        dschecksubjlist = d2.select_method_wo_parameter(queryreg, "text");
                                        DataSet dscheck = new DataSet();
                                        //dscheck = d2.select_method_wo_parameter(queryreg, "text");
                                        for (int subjlist = 0; subjlist < dschecksubjlist.Tables[0].Rows.Count; subjlist++)
                                        {
                                            if (!arr_subjectunique.Contains(dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString().Trim().ToLower() + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString().Trim().ToLower()))
                                            {
                                                DataView DVsubjlist = new DataView();
                                                dschecksubjlist.Tables[0].DefaultView.RowFilter = " subject_no='" + dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString() + "'and degree_code='" + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString() + "'";
                                                DVsubjlist = dschecksubjlist.Tables[0].DefaultView;
                                                dscheck.Clear();
                                                dscheck.Tables.Clear();
                                                dscheck.Tables.Add(DVsubjlist.ToTable());
                                                arr_subjectunique.Add(dschecksubjlist.Tables[0].Rows[subjlist]["subject_no"].ToString().Trim().ToLower() + dschecksubjlist.Tables[0].Rows[subjlist]["degree_code"].ToString().Trim().ToLower());
                                                string deptname = dscheck.Tables[0].Rows[0]["Course_Name"].ToString() + " - " + dscheck.Tables[0].Rows[0]["deptname"].ToString();
                                                string sub_code = dscheck.Tables[0].Rows[0]["subject_code"].ToString();
                                                string semester = dscheck.Tables[0].Rows[0]["semester"].ToString();
                                                string subname = dscheck.Tables[0].Rows[0]["subject_name"].ToString();
                                                we = we + 1;
                                                coltop = coltop + 35;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, ":  " + deptname);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + semester);
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 25;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + sub_code);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Exam /Session");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + ddlfrmdate.SelectedItem.Text + "/" + ddlsession.SelectedItem.Text);
                                                mypdfpage.Add(ptc);
                                                coltop = coltop + 25;
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 140, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ":  ");
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 150, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, subname);
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Hall No");//"Room No / Bundle No"
                                                mypdfpage.Add(ptc);
                                                ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                      new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + hall);//":  " 
                                                int tblrocc = 0;
                                                sml = "25";
                                                if (dscheck.Tables.Count > 0 && dscheck.Tables[0].Rows.Count < Convert.ToInt32(sml))
                                                {
                                                    tblrocc = dscheck.Tables[0].Rows.Count;
                                                }
                                                else
                                                {
                                                    tblrocc = Convert.ToInt32(sml);
                                                }
                                                Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, tblrocc + 1, 4, 4);
                                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 0).SetContent("S.No");
                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 0).SetFont(Fontbold);
                                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 1).SetContent("Register Number");
                                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 1).SetFont(Fontbold);
                                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 2).SetContent("Marks In Figures");
                                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 2).SetFont(Fontbold);
                                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 3).SetContent("Marks In Words");
                                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table1.Cell(0, 3).SetFont(Fontbold);
                                                //table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                //table1.Cell(0, 4).SetContent("Signature of Candidate");
                                                //table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                //table1.Cell(0, 4).SetFont(Fontbold);
                                                table1.VisibleHeaders = false;
                                                table1.Columns[0].SetWidth(20);
                                                table1.Columns[1].SetWidth(40);
                                                table1.Columns[2].SetWidth(100);
                                                table1.Columns[3].SetWidth(150);
                                                //table1.Columns[4].SetWidth(150);
                                                int gwe = 1;
                                                int ast = 0;
                                                tablerowscount = dscheck.Tables[0].Rows.Count;
                                                for (ast = startrow; ast < dscheck.Tables[0].Rows.Count; ast++)
                                                {
                                                    if (ast != 0 && ast % Convert.ToInt32(sml) == 0)
                                                    {
                                                        Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, 650));
                                                        mypdfpage.Add(newpdftabpage1);
                                                        tablerowscount = tablerowscount - 25;
                                                        PdfArea tete = new PdfArea(mydocument, 15, 10, 565, 825);
                                                        PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                                        mypdfpage.Add(pr1);
                                                        mypdfpage.SaveToDocument();
                                                        mypdfpage = mydocument.NewPage();
                                                        coltop = 10;
                                                        tbltCover = mydocument.NewTable(Fontbold, 2, 1, 15);
                                                        tbltCover.VisibleHeaders = false;
                                                        tbltCover.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                        tbltCover.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tbltCover.Cell(0, 0).SetContent("Cover No");
                                                        tbltCover.Cell(0, 0).SetCellPadding(5);
                                                        tbltCover.Columns[0].SetWidth(50);
                                                        tblPageCover = tbltCover.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, mydocument.PageWidth - 100, 20, 80, 80));
                                                        mypdfpage.Add(tblPageCover);
                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                                        {
                                                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                                            mypdfpage.Add(LogoImage, 35, 25, 700);
                                                        }
                                                        ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 15;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("Office of the Controller of Examinations").ToUpper());
                                                        mypdfpage.Add(ptc);
                                                        //coltop = coltop + 15;
                                                        //ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                        //                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
                                                        //mypdfpage.Add(ptc);
                                                        coltop = coltop + 15;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "FOIL CARD FOR THE END SEMESTER EXAMINATIONS" + "-" + Convert.ToString(ddlMonth.SelectedItem.Text).ToUpper() + " " + ddlYear.SelectedItem.Text + "");
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 10;
                                                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                                        mypdfpage.Add(ptc);
                                                        we = we + 1;
                                                        coltop = coltop + 35;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Degree & Branch");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 140, coltop, 180, 100), System.Drawing.ContentAlignment.TopLeft, ":  " + deptname);
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Semester");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + semester);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 25;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Code");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 140, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + sub_code);
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Exam /Session");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + ddlfrmdate.SelectedItem.Text + "/" + ddlsession.SelectedItem.Text);
                                                        mypdfpage.Add(ptc);
                                                        coltop = coltop + 25;
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Subject Name");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 140, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, ":  ");
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 150, coltop, 200, 50), System.Drawing.ContentAlignment.TopLeft, subname);
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 360, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Hall No");//"Room No / Bundle No"
                                                        mypdfpage.Add(ptc);
                                                        ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                              new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + hall);//":  " 
                                                        if (tablerowscount > 25)
                                                        {
                                                            tblrocc = 25;
                                                        }
                                                        else
                                                        {
                                                            tblrocc = tablerowscount;
                                                        }
                                                        table1 = mydocument.NewTable(Fontbold, tblrocc + 1, 4, 4);
                                                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 0).SetContent("S.No");
                                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 0).SetFont(Fontbold);
                                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 1).SetContent("Register Number");
                                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 1).SetFont(Fontbold);
                                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 2).SetContent("Marks In Figures");
                                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 2).SetFont(Fontbold);
                                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 3).SetContent("Marks In Words");
                                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(0, 3).SetFont(Fontbold);
                                                        //table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        //table1.Cell(0, 4).SetContent("Signature of Candidate");
                                                        //table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        //table1.Cell(0, 4).SetFont(Fontbold);
                                                        table1.VisibleHeaders = false;
                                                        table1.Columns[0].SetWidth(20);
                                                        table1.Columns[1].SetWidth(40);
                                                        table1.Columns[2].SetWidth(100);
                                                        table1.Columns[3].SetWidth(150);
                                                        gwe = 1;
                                                    }
                                                    string regno = dscheck.Tables[0].Rows[ast]["Reg_No"].ToString();
                                                    string name = dscheck.Tables[0].Rows[ast]["Stud_Name"].ToString();
                                                    string seat = dscheck.Tables[0].Rows[ast]["seat_no"].ToString();
                                                    string hallno = dscheck.Tables[0].Rows[ast]["roomno"].ToString();
                                                    table1.Cell(gwe, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table1.Cell(gwe, 0).SetContent(gwe.ToString());
                                                    table1.Cell(gwe, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table1.Cell(gwe, 1).SetContent(regno.ToString());
                                                    gwe = gwe + 1;
                                                }
                                                int h = 650;
                                                Gios.Pdf.PdfTablePage newpdftabpage11 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, yq, 550, h));
                                                mypdfpage.Add(newpdftabpage11);
                                                PdfArea tete1 = new PdfArea(mydocument, 15, 10, 565, 825);
                                                PdfRectangle pr11 = new PdfRectangle(mydocument, tete1, Color.Black);
                                                mypdfpage.Add(pr11);
                                                g = 1;
                                                if (h >= 500)
                                                {
                                                    coltop = 10;
                                                    ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                                    mypdfpage.Add(ptc);
                                                    coltop = coltop + 15;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("Office of the Controller of Examinations").ToUpper());
                                                    mypdfpage.Add(ptc);
                                                    //coltop = coltop + 15;
                                                    //ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                    //                                        new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, district + "-" + pincode);
                                                    //mypdfpage.Add(ptc);
                                                    coltop = coltop + 15;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "FOIL CARD FOR THE END SEMESTER EXAMINATIONS" + "-" + Convert.ToString(ddlMonth.SelectedItem.Text).ToUpper() + " " + ddlYear.SelectedItem.Text + "");
                                                    mypdfpage.Add(ptc);
                                                    coltop = coltop + 10;
                                                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________________________________________________________________________________");
                                                    mypdfpage.Add(ptc);
                                                    PdfTextArea pdfSignExaminer = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 25, 750, 200, 50), ContentAlignment.MiddleLeft, "Signature of the Examiner");
                                                    mypdfpage.Add(pdfSignExaminer);
                                                    PdfTextArea pdfDate = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 25, 775, 200, 50), ContentAlignment.MiddleLeft, "Date\t\t:\t\t");
                                                    mypdfpage.Add(pdfDate);
                                                    PdfTextArea pdfSignChairman = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 350, 750, 200, 50), ContentAlignment.MiddleRight, "Signature of the Chairman");
                                                    mypdfpage.Add(pdfSignChairman);
                                                    mypdfpage.SaveToDocument();
                                                    mypdfpage = mydocument.NewPage();
                                                    //yq = 190;
                                                }
                                            }
                                        }
                                        string appPath = HttpContext.Current.Server.MapPath("~");
                                        if (appPath != "")
                                        {
                                            string szPath = appPath + "/Report/";
                                            string szFile = "FoilCardSheet_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                                            mydocument.SaveToFile(szPath + szFile);
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
                }
                else
                {
                    lblnorecc.Visible = true;
                    lblnorecc.Text = "Please Allot Bundle No And Then Proceed";
                }
            }
        }
        catch (Exception ex)
        {
            lblnorecc.Text = ex.ToString();
            lblnorecc.Visible = true;
        }
    }

    protected void btnFoilCard_click(object sender, EventArgs e)
    {
        try
        {
            FoilCard();
        }
        catch (Exception ex)
        {
            lblnorecc.Text = ex.ToString();
            lblnorecc.Visible = true;
        }
    }

    #endregion Added By Malang Raja on Nov 08 2016

    #region Added By Malang Raja on Mar 17 2017

    private void dummyNumberSheet()
    {
        try
        {
            AttSpread.SaveChanges();
            divDummyNoSheets.Visible = false;
            lblDispErr.Visible = false;
            lblDispErr.Text = string.Empty;
            DataSet dsCollegeDetails = new DataSet();
            DataSet dsHallDetails = new DataSet();
            DataSet dsDummyNoDetails = new DataSet();
            Dictionary<string, byte> dicHallNo = new Dictionary<string, byte>();

            string qry = string.Empty;
            string examMonth = string.Empty;
            string examYear = string.Empty;
            string stream = string.Empty;
            string examDate = string.Empty;
            string examSessions = string.Empty;
            string hallNos = string.Empty;
            string userCode = string.Empty;

            string qryExamYear = string.Empty;
            string qryExamMonth = string.Empty;
            string qryExamDate = string.Empty;
            string qryExamSession = string.Empty;
            string qryHallNo = string.Empty;
            string hallNo = string.Empty;
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            string rollNo = string.Empty;
            string regNo = string.Empty;
            string appNo = string.Empty;
            string dummyNumber = string.Empty;
            string studentName = string.Empty;
            string studentType = string.Empty;
            string subjectCode = string.Empty;
            string subjectNo = string.Empty;
            string subjectName = string.Empty;
            string seatNo = string.Empty;
            string roomNos = string.Empty;
            string examDates = string.Empty;
            string examSession = string.Empty;
            DateTime dtExamDate = new DateTime();

            bool selHall = false;
            bool isBundledNoWise = false;
            bool isRandom = false;
            bool isSubjectWise = false;
            bool hasDummyNos = false;
            bool hasExamSeating = false;
            int sno = 0;

            DataTable dtDummyNumberDetails = new DataTable();
            dtDummyNumberDetails.Columns.Clear();
            dtDummyNumberDetails.Rows.Clear();
            dtDummyNumberDetails.Clear();

            dtDummyNumberDetails.Columns.Add("SNo");
            dtDummyNumberDetails.Columns.Add("SNos");
            dtDummyNumberDetails.Columns.Add("collegeCode");
            dtDummyNumberDetails.Columns.Add("BatchYear");
            dtDummyNumberDetails.Columns.Add("DegreeCode");
            dtDummyNumberDetails.Columns.Add("Roll_No");
            dtDummyNumberDetails.Columns.Add("Reg_No");
            dtDummyNumberDetails.Columns.Add("AppNo");
            dtDummyNumberDetails.Columns.Add("StudentName");
            dtDummyNumberDetails.Columns.Add("StudentType");
            dtDummyNumberDetails.Columns.Add("DummyNo");
            dtDummyNumberDetails.Columns.Add("Subject_code");
            dtDummyNumberDetails.Columns.Add("Subject_no");
            dtDummyNumberDetails.Columns.Add("Subject_name");
            dtDummyNumberDetails.Columns.Add("seat_no");
            dtDummyNumberDetails.Columns.Add("roomNo");
            dtDummyNumberDetails.Columns.Add("examDate");
            dtDummyNumberDetails.Columns.Add("examSession");
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                qry = "Select * from collinfo where college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'";
                dsCollegeDetails = d2.select_method_wo_parameter(qry, "Text");
            }
            if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
            }
            string dummyType = string.Empty;
            string dummyMode = string.Empty;
            if (!string.IsNullOrEmpty(userCode))
            {
                dummyType = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberTypeOnMarkEntryCOE' and user_code ='" + userCode + "'");//and college_code ='13' 
                dummyMode = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberModeOnMarkEntryCOE' and user_code ='" + userCode + "'");//and college_code ='13'
            }
            //else
            //{
            //    dummyType = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberTypeOnMarkEntryCOE' and LinkValue='1'");//and college_code ='13' 
            //    dummyMode = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberModeOnMarkEntryCOE' and LinkValue='1'");//and 
            //}
            if (string.IsNullOrEmpty(dummyMode.Trim()) || dummyMode.Trim() == "0")
            {
                isRandom = false;
            }
            else
            {
                isRandom = true;
            }
            if (string.IsNullOrEmpty(dummyType.Trim()) || dummyType.Trim() == "0")
            {
                isSubjectWise = false;
            }
            else
            {
                isSubjectWise = true;
            }
            string sml = d2.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
            if (sml.Trim() != "" && sml.Trim() != "0")
            {
                isBundledNoWise = true;
            }
            else
            {
                isBundledNoWise = false;
            }
            if (ddlYear.Items.Count == 0)
            {
                lblDispErr.Visible = true;
                lblDispErr.Text = "No Exam Year Were Found.";
                return;
            }
            else
            {
                examYear = Convert.ToString(ddlYear.SelectedItem.Text).Trim();
                if (string.IsNullOrEmpty(examYear) || examYear.Trim() == "0")
                {
                    lblDispErr.Visible = true;
                    lblDispErr.Text = "Please Select Exam Year And Then Proceed.";
                    return;
                }
                else
                {
                    qryExamYear = " and ed.Exam_year='" + examYear + "' ";
                }
            }
            if (ddlMonth.Items.Count == 0)
            {
                lblDispErr.Visible = true;
                lblDispErr.Text = "No Exam Month Were Found.";
                return;
            }
            else
            {
                examMonth = Convert.ToString(ddlMonth.SelectedItem.Value).Trim();
                if (string.IsNullOrEmpty(examMonth) || examMonth.Trim() == "0")
                {
                    lblDispErr.Visible = true;
                    lblDispErr.Text = "Please Select Exam Month And Then Proceed.";
                    return;
                }
                else
                {
                    qryExamMonth = " and ed.Exam_Month='" + examMonth + "'";
                }
            }
            if (ddltype.Items.Count > 0)
            {
                stream = Convert.ToString(ddltype.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(stream) && stream.Trim() != "0" && stream.Trim().ToLower() != "both" && stream.Trim().ToLower() != "all")
                {
                    stream = Convert.ToString(ddltype.SelectedItem.Text).Trim();
                }
                else
                {
                    stream = string.Empty;
                }
            }
            if (ddlsession.Items.Count > 0)
            {
                examSessions = Convert.ToString(ddlsession.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(examSessions) && examSessions.Trim() != "0" && examSessions.Trim().ToLower() != "both" && examSessions.Trim().ToLower() != "all")
                {
                    examSessions = Convert.ToString(ddlsession.SelectedItem.Text).Trim();
                    qryExamSession = "  and es.ses_sion ='" + examSessions + "'";
                }
                else
                {
                    examSessions = string.Empty;
                    qryExamSession = string.Empty;
                }
            }
            if (ddlfrmdate.Items.Count > 0)
            {
                examDate = Convert.ToString(ddlfrmdate.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(examDate) && examDate.Trim() != "0" && examDate.Trim().ToLower() != "both" && examDate.Trim().ToLower() != "all")
                {
                    examDate = Convert.ToString(ddlfrmdate.SelectedItem.Text).Trim();
                    qryExamDate = string.Empty;
                    if (!string.IsNullOrEmpty(examDate))
                    {
                        if (DateTime.TryParseExact(examDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtExamDate))
                        {
                            qryExamDate = " and es.edate='" + dtExamDate.ToString("MM/dd/yyyy") + "'";
                        }
                    }
                }
                else
                {
                    examDate = string.Empty;
                    qryExamDate = string.Empty;
                }
            }
            if (ddlPageNo.Items.Count == 0 && AttSpread.Sheets[0].RowCount <= 1)
            {
                lblDispErr.Visible = true;
                lblDispErr.Text = "No Hall No Were Found.";
                return;
            }
            else
            {
                string qryRegNo = string.Empty;
                string regNosDe = string.Empty;
                for (int row = 1; row < AttSpread.Sheets[0].RowCount; row++)
                {
                    string selected = Convert.ToString(AttSpread.Sheets[0].Cells[row, 1].Value).Trim();
                    int selVal = 0;
                    int.TryParse(selected.Trim(), out selVal);
                    string hallNumber = string.Empty;
                    hallNumber = Convert.ToString(AttSpread.Sheets[0].Cells[row, 2].Text).Trim();
                    regNosDe = string.Empty;
                    if (isBundledNoWise)
                    {
                        regNosDe = Convert.ToString(AttSpread.Sheets[0].Cells[row, 7].Tag).Trim();
                    }
                    if (selVal == 1)
                    {
                        selHall = true;
                        if (!string.IsNullOrEmpty(hallNumber.Trim()))
                        {
                            if (!dicHallNo.ContainsKey(hallNumber.Trim().ToLower()))
                            {
                                if (string.IsNullOrEmpty(hallNos))
                                {
                                    hallNos = "'" + hallNumber + "'";
                                }
                                else
                                {
                                    hallNos += ",'" + hallNumber + "'";
                                }
                                dicHallNo.Add(hallNumber.ToLower().Trim(), 0);
                            }
                        }
                        if (!string.IsNullOrEmpty(regNosDe.Trim()))
                        {
                            if (string.IsNullOrEmpty(qryRegNo))
                            {
                                qryRegNo = "'" + regNosDe + "'";
                            }
                            else
                            {
                                qryRegNo += ",'" + regNosDe + "'";
                            }
                        }
                    }
                }
                if (isBundledNoWise)
                {
                    if (AttSpread.Sheets[0].RowCount <= 1)
                    {
                        lblDispErr.Visible = true;
                        lblDispErr.Text = "No Hall No Were Found.";
                        return;
                    }
                    if (!selHall)
                    {
                        lblDispErr.Visible = true;
                        lblDispErr.Text = "Please Select Hall No And Then Proceed";
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hallNos))
                        {
                            qryHallNo = "  and roomno in(" + hallNos + ") ";
                        }
                        else
                        {
                            qryHallNo = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(qryRegNo))
                        {
                            qryRegNo = " and r.Reg_No in(" + qryRegNo + ") ";
                        }
                        else
                        {
                            qryRegNo = string.Empty;
                        }
                    }
                }
                else
                {
                    qryRegNo = string.Empty;
                    string hallNumber = string.Empty;
                    //if (!selHall)
                    //{
                    //    lblDispErr.Visible = true;
                    //    lblDispErr.Text = "Please Select Hall No And Then Proceed";
                    //    return;
                    //}
                    if (ddlPageNo.Items.Count == 0 && !selHall)
                    {
                        dicHallNo.Clear();
                        lblDispErr.Visible = true;
                        lblDispErr.Text = "No Hall No Were Found or Please Select Any One Hall And Then Proceed";
                        return;
                    }
                    else
                    {
                        if (ddlPageNo.Items.Count > 0)
                        {
                            dicHallNo.Clear();
                            hallNumber = Convert.ToString(ddlPageNo.SelectedItem.Text).Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(hallNumber))
                    {
                        if (!dicHallNo.ContainsKey(hallNumber.Trim().ToLower()))
                        {
                            if (string.IsNullOrEmpty(hallNos))
                            {
                                hallNos = "'" + hallNumber + "'";
                            }
                            else
                            {
                                hallNos += ",'" + hallNumber + "'";
                            }
                            dicHallNo.Add(hallNumber.ToLower().Trim(), 0);
                        }
                        if (!string.IsNullOrEmpty(hallNos))
                        {
                            qryHallNo = "  and roomno in(" + hallNos + ") ";
                        }
                        else
                        {
                            qryHallNo = string.Empty;
                        }
                    }
                    else
                    {
                        //lblDispErr.Visible = true;
                        //lblDispErr.Text = "Please Select Hall No And Then Proceed";
                        //return;
                        if (!string.IsNullOrEmpty(hallNos))
                        {
                            qryHallNo = "  and roomno in(" + hallNos + ") ";
                        }
                        else
                        {
                            qryHallNo = string.Empty;
                        }
                    }
                }
                string collegeName = string.Empty;
                string collname = string.Empty;
                string affilitied = string.Empty;
                string district = string.Empty;
                string pincode = string.Empty;
                string[] aff = new string[0];
                if (dsCollegeDetails.Tables.Count > 0 && dsCollegeDetails.Tables[0].Rows.Count > 0)
                {
                    collname = Convert.ToString(dsCollegeDetails.Tables[0].Rows[0]["collname"]).Trim();
                    affilitied = Convert.ToString(dsCollegeDetails.Tables[0].Rows[0]["affliatedby"]).Trim();
                    district = Convert.ToString(dsCollegeDetails.Tables[0].Rows[0]["district"]).Trim();
                    pincode = Convert.ToString(dsCollegeDetails.Tables[0].Rows[0]["pincode"]).Trim();
                    aff = affilitied.Split(',');
                    affilitied = ((aff.Length > 0) ? aff[0].ToString() : "");
                    string qryDummyNo = string.Empty;
                    string qryDummyMode = string.Empty;
                    string qryDummyType = string.Empty;
                    if (isRandom)
                    {
                        qryDummyMode = " and du.dummy_type='1'";
                    }
                    else
                    {
                        qryDummyMode = " and du.dummy_type='0'";
                    }
                    if (isSubjectWise)
                    {
                        qryDummyType = " Dummy_type ='1' ";
                    }
                    else
                    {
                        qryDummyType = " Dummy_type ='0' ";
                    }
                    qry = "select r.college_code,r.Batch_Year,r.degree_code,r.Current_Semester,r.App_No,r.Reg_No,r.Roll_No,du.dummy_no,du.dummy_type as dummy_Mode,ISNULL(du.subject,'') subject_code,ISNULL(du.subject_no,'0') as subject_no,du.exam_date,du.exam_month,du.exam_year,du.semester ,'0' as Dummy_type from dummynumber du,Registration r where r.Roll_No=du.roll_no and r.degree_code=du.degreecode and r.Batch_Year=du.batch and r.college_code=du.DNCollegeCode and du.subject_no is null and du.subject is null " + qryDummyMode + " and du.exam_month='" + examMonth + "' and du.exam_year='" + examYear + "' union select r.college_code,r.Batch_Year,r.degree_code,r.Current_Semester,r.App_No,r.Reg_No,r.Roll_No,du.dummy_no,du.dummy_type as dummy_Mode,ISNULL(du.subject,'') subject_code,ISNULL(du.subject_no,'0') as subject_no,du.exam_date,du.exam_month,du.exam_year,du.semester,'1' as Dummy_type from dummynumber du,Registration r,subject s where r.Roll_No=du.roll_no and s.subject_no=du.subject_no and r.degree_code=du.degreecode and r.Batch_Year=du.batch and r.college_code=du.DNCollegeCode  " + qryDummyMode + " and du.exam_month='" + examMonth + "' and du.exam_year='" + examYear + "' order by du.dummy_type,Dummy_type,r.college_code,r.Batch_Year desc,r.degree_code,r.Reg_No";
                    dsDummyNoDetails = d2.select_method_wo_parameter(qry, "text");
                    qry = "select r.Reg_No,r.app_no,r.Roll_No,r.Stud_Name,r.college_code,r.Batch_Year,r.degree_code,r.Stud_Type,r.current_semester,s.subject_code,s.subject_no,es.seat_no,es.roomno,s.subject_name,CONVERT(varchar(20),es.edate,103) edate ,es.ses_sion  from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,Registration r,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and r.Roll_No=ea.roll_no and ed.degree_code=r.degree_code and ed.batch_year=r.Batch_Year and ed.degree_code=es.degree_code and es.degree_code=r.degree_code and es.regno=r.Reg_No and ead.subject_no=s.subject_no and es.subject_no=s.subject_no " + qryExamYear + qryExamMonth + qryExamSession + qryExamDate + qryHallNo + qryRegNo + " order by es.edate,es.ses_sion desc,es.roomno ,es.seat_no";
                    dsHallDetails = d2.select_method_wo_parameter(qry, "text");
                    if (dicHallNo.Count > 0)
                    {
                        Dictionary<string, string> dicAddedStuents = new Dictionary<string, string>();
                        sno = 0;
                        foreach (KeyValuePair<string, byte> dicHall in dicHallNo)
                        {
                            string roomNo = dicHall.Key;
                            DataTable dtHalls = new DataTable();
                            DataTable dtDummyNos = new DataTable();
                            if (dsHallDetails.Tables.Count > 0 && dsHallDetails.Tables[0].Rows.Count > 0)
                            {
                                dsHallDetails.Tables[0].DefaultView.RowFilter = "roomno='" + roomNo.Trim() + "'";
                                dtHalls = dsHallDetails.Tables[0].DefaultView.ToTable();
                            }
                            foreach (DataRow drStud in dtHalls.Rows)
                            {
                                hasExamSeating = true;
                                hallNo = string.Empty;
                                collegeCode = string.Empty;
                                batchYear = string.Empty;
                                degreeCode = string.Empty;
                                rollNo = string.Empty;
                                regNo = string.Empty;
                                appNo = string.Empty;
                                dummyNumber = string.Empty;
                                studentName = string.Empty;
                                studentType = string.Empty;
                                subjectCode = string.Empty;
                                subjectNo = string.Empty;
                                subjectName = string.Empty;
                                seatNo = string.Empty;
                                roomNos = string.Empty;
                                examDates = string.Empty;
                                examSession = string.Empty;

                                hallNo = Convert.ToString(drStud["roomno"]).Trim();
                                collegeCode = Convert.ToString(drStud["college_code"]).Trim();
                                batchYear = Convert.ToString(drStud["Batch_Year"]).Trim();
                                degreeCode = Convert.ToString(drStud["degree_code"]).Trim();
                                rollNo = Convert.ToString(drStud["Roll_No"]).Trim();
                                regNo = Convert.ToString(drStud["Reg_No"]).Trim();
                                appNo = Convert.ToString(drStud["app_no"]).Trim();
                                studentName = Convert.ToString(drStud["Stud_Name"]).Trim();
                                studentType = Convert.ToString(drStud["Stud_Type"]).Trim();
                                subjectCode = Convert.ToString(drStud["subject_code"]).Trim();
                                subjectNo = Convert.ToString(drStud["subject_no"]).Trim();
                                subjectName = Convert.ToString(drStud["subject_name"]).Trim();
                                seatNo = Convert.ToString(drStud["seat_no"]).Trim();
                                roomNos = Convert.ToString(drStud["roomno"]).Trim();
                                examDates = Convert.ToString(drStud["edate"]).Trim();
                                examSession = Convert.ToString(drStud["ses_sion"]).Trim();

                                string key = string.Empty;
                                string qrySubWise = string.Empty;
                                if (isSubjectWise)
                                {
                                    key = regNo + "@" + subjectNo;
                                    qrySubWise = " and subject_no='" + subjectNo + "'";
                                    qryDummyType = " Dummy_type ='1' ";
                                }
                                else
                                {
                                    key = regNo;
                                    qrySubWise = string.Empty;
                                    qryDummyType = " Dummy_type ='0' ";
                                    //roomNos = roomNo;
                                    //examSession = string.Empty;
                                    //seatNo = string.Empty;
                                    //subjectNo = string.Empty;
                                    //subjectCode = string.Empty;
                                    //subjectName = string.Empty;
                                    //examDates = dtExamDate.ToString("dd/MM/yyyy");
                                }
                                key = key.Trim().ToLower();
                                DataRow drDummyNo;
                                if (dsDummyNoDetails.Tables.Count > 0 && dsDummyNoDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsDummyNoDetails.Tables[0].DefaultView.RowFilter = qryDummyType + qrySubWise + " and app_no='" + appNo + "'";
                                    dtDummyNos = dsDummyNoDetails.Tables[0].DefaultView.ToTable();
                                }
                                if (dtDummyNos.Rows.Count > 0)
                                {
                                    hasDummyNos = true;
                                    dummyNumber = Convert.ToString(dtDummyNos.Rows[0]["dummy_no"]).Trim();
                                    if (!dicAddedStuents.ContainsKey(key.Trim().ToLower()))
                                    {
                                        sno++;
                                        drDummyNo = dtDummyNumberDetails.NewRow();
                                        drDummyNo["SNo"] = Convert.ToString(sno).Trim();
                                        drDummyNo["SNos"] = Convert.ToString(sno).Trim();
                                        drDummyNo["collegeCode"] = Convert.ToString(collegeCode).Trim();
                                        drDummyNo["BatchYear"] = Convert.ToString(batchYear).Trim();
                                        drDummyNo["DegreeCode"] = Convert.ToString(degreeCode).Trim();
                                        drDummyNo["Roll_No"] = Convert.ToString(rollNo).Trim();
                                        drDummyNo["Reg_No"] = Convert.ToString(regNo).Trim();
                                        drDummyNo["AppNo"] = Convert.ToString(appNo).Trim();
                                        drDummyNo["StudentName"] = Convert.ToString(studentName).Trim();
                                        drDummyNo["StudentType"] = Convert.ToString(studentType).Trim();
                                        drDummyNo["DummyNo"] = Convert.ToString(dummyNumber).Trim();
                                        drDummyNo["Subject_code"] = Convert.ToString(subjectCode).Trim();
                                        drDummyNo["Subject_no"] = Convert.ToString(subjectNo).Trim();
                                        drDummyNo["Subject_name"] = Convert.ToString(subjectName).Trim();
                                        drDummyNo["seat_no"] = Convert.ToString(seatNo).Trim();
                                        drDummyNo["roomNo"] = Convert.ToString(roomNos).Trim();
                                        drDummyNo["examDate"] = Convert.ToString(examDates).Trim();
                                        drDummyNo["examSession"] = Convert.ToString(examSession).Trim();
                                        dtDummyNumberDetails.Rows.Add(drDummyNo);
                                        dicAddedStuents.Add(key.Trim().ToLower(), "1");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lblDispErr.Visible = true;
                        lblDispErr.Text = "No Hall No Were Found.";
                        return;
                    }
                    if (dtDummyNumberDetails.Rows.Count > 0)
                    {
                        //gvDummyNoSheet.DataSource = dtDummyNumberDetails;
                        //gvDummyNoSheet.DataBind();
                        //divDummyNoSheets.Visible = true;
                        printDummyNumberDetails(dsCollegeDetails.Tables[0], dtDummyNumberDetails, dicHallNo, hallNo, examDate, examSession, examYear);
                    }
                    else
                    {
                        if (!hasExamSeating)
                        {
                            lblDispErr.Text = "Please Generate Exam Seating";
                        }
                        else if (!hasDummyNos)
                        {
                            lblDispErr.Text = "Please Generate Dummy Numbers And Then Proceed";
                        }
                        else
                        {
                            lblDispErr.Text = "Please Generate Exam Seating or Dummy Numbers";
                        }
                        divDummyNoSheets.Visible = false;
                        lblDispErr.Visible = true;
                        return;
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void btnDummyNoSheets_click(object sender, EventArgs e)
    {
        try
        {
            dummyNumberSheet();
        }
        catch (Exception ex)
        {
            lblnorecc.Text = ex.ToString();
            lblnorecc.Visible = true;
        }
    }

    public void printDummyNumberDetails(DataTable dtCollege, DataTable dtDummyNumbersDetails, Dictionary<string, byte> dicHallNo, string hallNo, string dates, string examSessions, string examMonthYear)
    {
        try
        {
            Font fontCol_Name = new Font("Book Antiqua", 16, FontStyle.Bold);
            Font fontclgAddrHeader = new Font("Book Antiqua", 11, FontStyle.Regular);
            Font fontclgReportHeader = new Font("Book Antiqua", 15, FontStyle.Bold);
            Font fontstudClass = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font fontRegNo = new Font("Book Antiqua", 7, FontStyle.Regular);
            Font fontReportContent = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font fontStudentDetailsContent = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font fontReportStudProfileHeader = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font fontReportStudActivityHeader = new Font("Book Antiqua", 10, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            PdfTextArea pdftxt;
            bool status = false;
            PdfTablePage pdfNewTablePage;
            PdfTable pdfSubHeader;
            PdfTable pdfNewTable;
            int coltop = 15;
            string collegeName = string.Empty;
            string collname = string.Empty;
            string affilitied = string.Empty;
            string district = string.Empty;
            string pincode = string.Empty;
            string[] aff = new string[0];
            //string hallNo = string.Empty;
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            string rollNo = string.Empty;
            string regNo = string.Empty;
            string appNo = string.Empty;
            string dummyNumber = string.Empty;
            string studentName = string.Empty;
            string studentType = string.Empty;
            string subjectCode = string.Empty;
            string subjectNo = string.Empty;
            string subjectName = string.Empty;
            string seatNo = string.Empty;
            string roomNos = string.Empty;
            string examDates = string.Empty;
            string examSession = string.Empty;

            if (dtCollege.Rows.Count > 0 && dtDummyNumbersDetails.Rows.Count > 0)
            {
                foreach (KeyValuePair<string, byte> dicHall in dicHallNo)
                {
                    string roomNo = dicHall.Key;
                    mypdfpage = mydoc.NewPage();
                    DataTable dtHalls = new DataTable();
                    DataTable dtDummyNos = new DataTable();
                    if (dtDummyNumbersDetails.Rows.Count > 0)
                    {
                        dtDummyNumbersDetails.DefaultView.RowFilter = "roomno='" + roomNo.Trim() + "'";
                        dtHalls = dtDummyNumbersDetails.DefaultView.ToTable();
                    }
                    collname = Convert.ToString(dtCollege.Rows[0]["collname"]).Trim();
                    affilitied = Convert.ToString(dtCollege.Rows[0]["affliatedby"]).Trim();
                    district = Convert.ToString(dtCollege.Rows[0]["district"]).Trim();
                    pincode = Convert.ToString(dtCollege.Rows[0]["pincode"]).Trim();
                    aff = affilitied.Split(',');
                    affilitied = ((aff.Length > 0) ? aff[0].ToString() : "");
                    coltop = 12;
                    pdftxt = new PdfTextArea(fontCol_Name, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, collname);
                    mypdfpage.Add(pdftxt);
                    coltop += 26;
                    int sno = 0;
                    int tblRow = 1;
                    int hallRows = 20;
                    if (dtHalls.Rows.Count < 20)
                    {
                        hallRows = dtHalls.Rows.Count;
                    }
                    pdfNewTable = mydoc.NewTable(fontStudentDetailsContent, hallRows + 1, 6, 10);
                    pdfNewTable.VisibleHeaders = false;
                    pdfNewTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    pdfNewTable.SetColumnsWidth(new int[] { 50, 100, 200, 200, 200, 200 });
                    pdfNewTable.Cell(0, 0).SetContent("SNo");
                    pdfNewTable.Cell(0, 0).SetFont(fontReportStudActivityHeader);
                    pdfNewTable.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                    pdfNewTable.Cell(0, 1).SetContent("Reg No");
                    pdfNewTable.Cell(0, 1).SetFont(fontReportStudActivityHeader);
                    pdfNewTable.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                    pdfNewTable.Cell(0, 2).SetContent("Dummy No");
                    pdfNewTable.Cell(0, 2).SetFont(fontReportStudActivityHeader);
                    pdfNewTable.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                    pdfNewTable.Cell(0, 3).SetContent("Dummy No");
                    pdfNewTable.Cell(0, 3).SetFont(fontReportStudActivityHeader);
                    pdfNewTable.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);

                    pdfNewTable.Cell(0, 4).SetContent("Dummy No");
                    pdfNewTable.Cell(0, 4).SetFont(fontReportStudActivityHeader);
                    pdfNewTable.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                    pdfNewTable.Cell(0, 5).SetContent("Dummy No");
                    pdfNewTable.Cell(0, 5).SetFont(fontReportStudActivityHeader);
                    pdfNewTable.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    int iteration = 0;
                    foreach (DataRow drDummyNo in dtHalls.Rows)
                    {
                        status = true;
                        hallNo = string.Empty;
                        collegeCode = string.Empty;
                        batchYear = string.Empty;
                        degreeCode = string.Empty;
                        rollNo = string.Empty;
                        regNo = string.Empty;
                        appNo = string.Empty;
                        dummyNumber = string.Empty;
                        studentName = string.Empty;
                        studentType = string.Empty;
                        subjectCode = string.Empty;
                        subjectNo = string.Empty;
                        subjectName = string.Empty;
                        seatNo = string.Empty;
                        roomNos = string.Empty;
                        examDates = string.Empty;
                        examSession = string.Empty;
                        string snos = Convert.ToString(drDummyNo["SNos"]).Trim();
                        string snoOld = Convert.ToString(drDummyNo["SNo"]).Trim();
                        hallNo = Convert.ToString(drDummyNo["roomNo"]).Trim();
                        collegeCode = Convert.ToString(drDummyNo["collegeCode"]).Trim();
                        batchYear = Convert.ToString(drDummyNo["BatchYear"]).Trim();
                        degreeCode = Convert.ToString(drDummyNo["DegreeCode"]).Trim();
                        rollNo = Convert.ToString(drDummyNo["Roll_No"]).Trim();
                        regNo = Convert.ToString(drDummyNo["Reg_No"]).Trim();
                        appNo = Convert.ToString(drDummyNo["AppNo"]).Trim();
                        dummyNumber = Convert.ToString(drDummyNo["DummyNo"]).Trim();
                        studentName = Convert.ToString(drDummyNo["StudentName"]).Trim();
                        studentType = Convert.ToString(drDummyNo["StudentType"]).Trim();
                        subjectCode = Convert.ToString(drDummyNo["Subject_code"]).Trim();
                        subjectNo = Convert.ToString(drDummyNo["Subject_no"]).Trim();
                        subjectName = Convert.ToString(drDummyNo["Subject_name"]).Trim();
                        seatNo = Convert.ToString(drDummyNo["seat_no"]).Trim();
                        roomNos = Convert.ToString(drDummyNo["roomNo"]).Trim();
                        examDates = Convert.ToString(drDummyNo["examDate"]).Trim();
                        examSession = Convert.ToString(drDummyNo["examSession"]).Trim();
                        if (sno == 0)
                        {
                            pdfSubHeader = mydoc.NewTable(fontReportStudActivityHeader, 1, 5, 4);
                            pdfSubHeader.VisibleHeaders = false;
                            pdfSubHeader.SetBorders(Color.Black, 1, BorderType.None);
                            pdfSubHeader.SetColumnsWidth(new int[] { 80, 80, 270, 170, 180 });
                            pdfSubHeader.Cell(0, 0).SetContent("Hall No :");
                            pdfSubHeader.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                            pdfSubHeader.Cell(0, 1).SetContent(roomNos);
                            pdfSubHeader.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                            pdfSubHeader.Cell(0, 3).SetContent("Exam Date & Session :");
                            pdfSubHeader.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                            pdfSubHeader.Cell(0, 4).SetContent(examDates + " & " + examSession);
                            pdfSubHeader.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);

                            pdfNewTablePage = pdfSubHeader.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, mydoc.PageWidth - 50, 50));
                            mypdfpage.Add(pdfNewTablePage);
                            coltop += Convert.ToInt16(pdfNewTablePage.Area.Height) + 14;
                        }
                        if (sno % 20 == 0 && sno != 0)
                        {
                            pdfNewTablePage = pdfNewTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, mydoc.PageWidth - 50, mydoc.PageHeight - coltop - 20));
                            mypdfpage.Add(pdfNewTablePage);

                            mypdfpage.SaveToDocument();
                            mypdfpage = mydoc.NewPage();

                            coltop = 12;
                            pdftxt = new PdfTextArea(fontCol_Name, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, collname);
                            mypdfpage.Add(pdftxt);
                            coltop += 26;

                            pdfSubHeader = mydoc.NewTable(fontReportStudActivityHeader, 1, 5, 4);
                            pdfSubHeader.VisibleHeaders = false;
                            pdfSubHeader.SetBorders(Color.Black, 1, BorderType.None);
                            pdfSubHeader.SetColumnsWidth(new int[] { 80, 80, 270, 170, 180 });
                            pdfSubHeader.Cell(0, 0).SetContent("Hall No :");
                            pdfSubHeader.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                            pdfSubHeader.Cell(0, 1).SetContent(roomNos);
                            pdfSubHeader.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                            pdfSubHeader.Cell(0, 3).SetContent("Exam Date & Session :");
                            pdfSubHeader.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                            pdfSubHeader.Cell(0, 4).SetContent(examDates + " & " + examSession);
                            pdfSubHeader.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);

                            pdfNewTablePage = pdfSubHeader.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, mydoc.PageWidth - 50, 50));
                            mypdfpage.Add(pdfNewTablePage);
                            coltop += Convert.ToInt16(pdfNewTablePage.Area.Height) + 14;
                            iteration++;
                            hallRows = 20;
                            if (dtHalls.Rows.Count - (iteration * 20) < 20)
                            {
                                hallRows = dtHalls.Rows.Count - (iteration * 20);
                            }
                            pdfNewTable = mydoc.NewTable(fontStudentDetailsContent, hallRows + 1, 6, 10);
                            pdfNewTable.VisibleHeaders = false;
                            pdfNewTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            pdfNewTable.SetColumnsWidth(new int[] { 50, 100, 200, 200, 200, 200 });

                            pdfNewTable.Cell(0, 0).SetContent("SNo");
                            pdfNewTable.Cell(0, 0).SetFont(fontReportStudActivityHeader);
                            pdfNewTable.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                            pdfNewTable.Cell(0, 1).SetContent("Reg No");
                            pdfNewTable.Cell(0, 1).SetFont(fontReportStudActivityHeader);
                            pdfNewTable.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            pdfNewTable.Cell(0, 2).SetContent("Dummy No");
                            pdfNewTable.Cell(0, 2).SetFont(fontReportStudActivityHeader);
                            pdfNewTable.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                            pdfNewTable.Cell(0, 3).SetContent("Dummy No");
                            pdfNewTable.Cell(0, 3).SetFont(fontReportStudActivityHeader);
                            pdfNewTable.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);

                            pdfNewTable.Cell(0, 4).SetContent("Dummy No");
                            pdfNewTable.Cell(0, 4).SetFont(fontReportStudActivityHeader);
                            pdfNewTable.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                            pdfNewTable.Cell(0, 5).SetContent("Dummy No");
                            pdfNewTable.Cell(0, 5).SetFont(fontReportStudActivityHeader);
                            pdfNewTable.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tblRow = 1;
                        }

                        pdfNewTable.Cell(tblRow, 0).SetContent(snoOld);
                        pdfNewTable.Cell(tblRow, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                        if (regNo.Length > 8)
                        {
                            pdfNewTable.Cell(tblRow, 1).SetContent(regNo);
                            pdfNewTable.Cell(tblRow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            pdfNewTable.Cell(tblRow, 1).SetFont(fontRegNo);
                        }
                        else
                        {
                            pdfNewTable.Cell(tblRow, 1).SetContent(regNo);
                            pdfNewTable.Cell(tblRow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                        pdfNewTable.Cell(tblRow, 2).SetContent(dummyNumber);
                        pdfNewTable.Cell(tblRow, 2).SetFont(fontclgReportHeader);
                        pdfNewTable.Cell(tblRow, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                        pdfNewTable.Cell(tblRow, 3).SetContent(dummyNumber);
                        pdfNewTable.Cell(tblRow, 3).SetFont(fontclgReportHeader);
                        pdfNewTable.Cell(tblRow, 3).SetContentAlignment(ContentAlignment.MiddleCenter);

                        pdfNewTable.Cell(tblRow, 4).SetContent(dummyNumber);
                        pdfNewTable.Cell(tblRow, 4).SetFont(fontclgReportHeader);
                        pdfNewTable.Cell(tblRow, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                        pdfNewTable.Cell(tblRow, 5).SetContent(dummyNumber);
                        pdfNewTable.Cell(tblRow, 5).SetFont(fontclgReportHeader);
                        pdfNewTable.Cell(tblRow, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                        sno++;
                        tblRow++;
                    }
                    pdfNewTablePage = pdfNewTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, mydoc.PageWidth - 50, mydoc.PageHeight - coltop - 20));
                    mypdfpage.Add(pdfNewTablePage);
                    if (status)
                        mypdfpage.SaveToDocument();
                }
            }
            else
            {

            }
            if (status == true)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "DummyNumberSheetsDetails" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch
        {

        }
    }

    #endregion


    private bool ShowDummyNumber(string CollegeCode)
    {

        bool retval = false;
        string saveDummy = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ShowDummyNumberOnMarkEntryCOE' and college_code ='" + CollegeCode + "' and user_code ='" + usercode + "'  ").Trim();
        if (saveDummy == "1")
        {
            retval = true;
        }
        return retval;
    }

    private byte DummyNumberType(string CollegeCode)
    {
        byte retval = 0;//0-common , 1- subjectwise
        string typeDummy = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberTypeOnMarkEntryCOE' and college_code ='" + CollegeCode + "' and user_code ='" + usercode + "'  ").Trim();
        if (typeDummy == "1")
        {
            retval = 1;
        }
        return retval;
    }

    private byte getDummyNumberMode(string CollegeCode)
    {
        byte retval = 0;//0-Serial , 1- Random
        string modeDummy = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DummyNumberModeOnMarkEntryCOE' and college_code ='" + CollegeCode + "' and user_code ='" + usercode + "'  ").Trim();
        if (modeDummy == "1")
        {
            retval = 1;
        }
        return retval;
    }

    protected void btnsettings_Click(object sender, EventArgs e)
    {

        string qr = "select template from Master_Settings where settings='Exam Attendance Signature Settings' and usercode='" + Convert.ToString(Session["collegecode"]) + "'";
        DataSet dsqr = d2.select_method_wo_parameter(qr, "text");
        string txt1 = string.Empty; string txt2 = string.Empty;
        if (dsqr.Tables.Count > 0 && dsqr.Tables[0].Rows.Count > 0)
        {
            string str = Convert.ToString(dsqr.Tables[0].Rows[0]["template"]);
            string[] splt = str.Split(';');
            txt1 = Convert.ToString(splt[0]);
            txt2 = Convert.ToString(splt[1]);
        }
        txtsignature1.Text = txt1;
        txtsignature2.Text = txt2;
        divsettings.Visible = true;
        divset.Visible = true;
    }

    protected void btnsaveformsetting_Click(object sender, EventArgs e)
    {
        try
        {
            string sign1 = string.Empty;
            string sign2 = string.Empty;

            sign1 = txtsignature1.Text;
            sign2 = txtsignature2.Text;

            string signatures = sign1 + ";" + sign2;

            string updateqry = "if exists(select template from Master_Settings where settings='Exam Attendance Signature Settings' and usercode='" + Convert.ToString(Session["collegecode"]) + "') update Master_Settings set template='" + Convert.ToString(signatures) + "' where usercode='" + Convert.ToString(Session["collegecode"]) + "' and settings='Exam Attendance Signature Settings' else insert into Master_Settings (usercode,settings,template) values('" + Convert.ToString(Session["collegecode"]) + "','Exam Attendance Signature Settings','" + Convert.ToString(signatures) + "')";
            int updqry = d2.update_method_wo_parameter(updateqry, "text");
            if (updqry > 0)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Saved Successfully";

                txtsignature1.Text = string.Empty;
                txtsignature2.Text = string.Empty;


            }


        }
        catch
        {
        }

    }
    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }
    protected void btnexit1_Click(object sender, EventArgs e)
    {

        divsettings.Visible = false;
        divset.Visible = false;

    }



}