using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using System.Configuration;

public partial class DespatchOfAnswerPackets : System.Web.UI.Page
{
    string college_code = string.Empty;
    string collegeCode = string.Empty;
    string userCollegeCode = string.Empty;
    string userCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 dt = new DAccess2();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
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
                Fpspread.Sheets[0].Visible = false;
                btn_directprint.Visible = false;
                Bindcollege();
                loadmonth();
                loaddatesession();
                loadYear();
              
               
            }

        }
        catch (Exception ex)
        { da.sendErrorMail(ex, collegeCode, "DespatchOfAnswerPackets"); }
    }
    public void Bindcollege()
    {
        try
        {
            cblCollege.Items.Clear();
            chkCollege.Checked = false;
            dtCommon.Clear();
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                cblCollege.DataSource = dtCommon;
                cblCollege.DataTextField = "collname";
                cblCollege.DataValueField = "college_code";
                cblCollege.DataBind();
               
            }
        }
        catch (Exception ex)
        { da.sendErrorMail(ex, collegeCode, "DespatchOfAnswerPackets"); }
    }
    public void loaddatesession()
    {
        try
        {
            ddlSession.Items.Clear();
            ddlDate.Items.Clear();
            ds.Clear();
            ds.Reset();
            if (ddlMonth.Items.Count > 0 && ddlYear.Items.Count > 0 && ddlMonth.SelectedIndex != -1)
            {
                string s = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,et.exam_date from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' and DATEPART(month, et.exam_date)>='" + ddlMonth.SelectedValue.ToString() + "' and DATEPART(YEAR, et.exam_date)>='" + ddlYear.SelectedItem.Text.ToString() + "' order by et.exam_date";
                ds.Clear();
                ds.Reset();
                ds = dt.select_method_wo_parameter(s, "txt");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlDate.Enabled = true;
                    ddlSession.Enabled = true;
                    ddlDate.Items.Clear();
                    ddlDate.DataSource = ds;
                    ddlDate.DataTextField = "ExamDate";
                    ddlDate.DataValueField = "ExamDate";
                    ddlDate.DataBind();
                   
                }
                string s1 = "select distinct et.exam_session from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' and DATEPART(month, et.exam_date)>='" + ddlMonth.SelectedValue.ToString() + "' and DATEPART(YEAR, et.exam_date)>='" + ddlYear.SelectedItem.Text.ToString() + "'";
                ds.Clear();
                ds.Reset();
                ds = dt.select_method_wo_parameter(s1, "txt");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlDate.Enabled = true;
                    ddlSession.Enabled = true;
                    ddlSession.Items.Clear();
                    ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                    ddlSession.DataSource = ds;
                    ddlSession.DataTextField = "exam_session";
                    ddlSession.DataValueField = "exam_session";
                    ddlSession.DataBind();
                }
                else
                {
                    ddlDate.Items.Clear();
                    ddlSession.Items.Clear();
                    ddlDate.Enabled = false;
                    ddlSession.Enabled = false;
                }
                
            }
           
            else
            {
               
            }
        }
        catch (Exception ex)
        { da.sendErrorMail(ex, collegeCode, "DespatchOfAnswerPackets"); }
    }
    public void loadYear()
    {
        try
        {
            ddlYear.Items.Clear();
            ds = dt.Examyear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
            }
        }
        catch (Exception ex)
        { da.sendErrorMail(ex, collegeCode, "DespatchOfAnswerPackets"); }
    }
    public void loadmonth()
    {
        try
        {
            ds.Clear();
            ddlMonth.Items.Clear();
            string year = ddlYear.SelectedValue;
            ds = dt.Exammonth(year);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthname";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
            }
        }
        catch (Exception ex)
        { da.sendErrorMail(ex, collegeCode, "DespatchOfAnswerPackets"); }
    }
    protected void chkCollege_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
           
            CallCheckboxChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            loadYear();

        }
        catch (Exception ex)
        { da.sendErrorMail(ex, collegeCode, "DespatchOfAnswerPackets"); }
    }
    protected void cblCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
           
            loadYear();
            CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");


        }
        catch (Exception ex)
        { da.sendErrorMail(ex, collegeCode, "DespatchOfAnswerPackets"); }
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        loaddatesession();
  
    }
   
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        loadmonth();
        loaddatesession();

    }
    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        //loaddatesession();
    }
    protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        //loaddatesession();
    }

   
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
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
        catch (Exception ex)
        { da.sendErrorMail(ex, collegeCode, "DespatchOfAnswerPackets"); }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
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
        catch (Exception ex)
        { da.sendErrorMail(ex, collegeCode, "DespatchOfAnswerPackets"); }
    }
   

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string valCollege = string.Empty;
            int exMonthName = 0;
            int colspanbranch = 0;
            int colspansubj = 0;
            int coldeg = 0;
            int coldegr = 0;
            int dsrow = 0;
            int inrow = 0;
            string sess = string.Empty;
            string collegeCode1 = string.Empty;
            int total = 0;
            bool colspanBool = false;
            string BranchCodeReplication = string.Empty;
            string degree = string.Empty;
            string subjcode = string.Empty;
            DataView colSpanDV = new DataView();
            DataView colSpansub = new DataView();
            DataSet dsCollege = new DataSet();
            Fpspread.Sheets[0].Visible = true;
            Fpspread.Sheets[0].AutoPostBack = true;
            Fpspread.Sheets[0].RowHeader.Visible = false;
            Fpspread.Sheets[0].ColumnHeader.Visible = true;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.White;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Fpspread.CommandBar.Visible = false;
            Fpspread.Sheets[0].ColumnCount = 8;
            Fpspread.Sheets[0].RowCount = 2;
            Fpspread.BorderWidth = 2;
            valCollege = rs.GetSelectedItemsValueAsString(cblCollege);
            btn_directprint.Visible = true;
            if (valCollege.ToString() != "")
            {
                if (ddlDate.SelectedValue != "" && ddlSession.SelectedValue != "")
                {
                    lblerror.Visible = false;
                    Fpspread.Visible = true;
                    int.TryParse(Convert.ToString(ddlMonth.SelectedValue).Trim(), out exMonthName);
                    string exdate = Convert.ToString(ddlDate.SelectedValue).Trim();
                    string[] spl1 = exdate.Split('-');
                    DateTime dtl1 = Convert.ToDateTime(spl1[1] + '-' + spl1[0] + '-' + spl1[2]);
                    string exmdate = dtl1.ToString("dd");
                    string exmmonth = dtl1.ToString("MM");
                    string exmmonthful = dtl1.ToString("MMMM");
                    string exmyear = dtl1.ToString("yyyy");
                    string examdate = exmyear + '-' + exmmonth + '-' + exmdate;
                    string prmonthyea = exmmonthful + '-' + exmyear;

                    // string sql ="select distinct s.subject_code, de.Dept_Name,c.Course_Name,d.Degree_Code,s.subject_name,d.Acronym,et.exam_date,et.exam_session from exmtt e,exmtt_det et,course c,Degree d,Department de,subject s  where  e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and d.Dept_Code=de.Dept_Code  and et.exam_date='" + examdate + "' and e.exam_Month='" + exMonthName + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text + "' and et.exam_session='" + ddlSession.SelectedItem.Text + "' and et.coll_code in('" + valCollege + "')and e.exam_code=et.exam_code and et.subject_no=s.subject_no group by c.Course_Name, de.Dept_Name,d.Degree_Code,s.subject_name,d.Acronym,et.exam_date,et.exam_session,s.subject_code order by de.Dept_Name";
                    //string sql = "select distinct s.subject_code,de.Dept_Name,c.Course_Name,s.subject_name,et.exam_date,et.exam_session,d.Acronym,COUNT(ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + exMonthName + "'and et.coll_code in('" + valCollege + "') and e.Exam_year='" + ddlYear.SelectedItem.Text + "' and  et.exam_date='" + examdate + "' and et.exam_session='" + ddlSession.SelectedItem.Text + "' group by s.subject_code,de.Dept_Name,c.Course_Name,s.subject_name,et.exam_date,et.exam_session,d.Acronym,c.type order by de.Dept_Name";

                    string sql = "select  s.subject_code,(c.Course_Name+'-'+de.Dept_Name) as course,c.Edu_Level,s.subject_name,et.exam_date,et.exam_session,d.Acronym,COUNT(ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + exMonthName + "' and et.coll_code in('" + valCollege + "') and e.Exam_year='" + ddlYear.SelectedItem.Text + "'  and  et.exam_date='" + examdate + "' and et.exam_session='" + ddlSession.SelectedItem.Text + "'  group by s.subject_code,de.Dept_Name,c.Course_Name,s.subject_name,et.exam_date,et.exam_session,d.Acronym,c.Edu_Level,c.type order by  s.subject_code";
                    ds = dt.select_method_wo_parameter(sql, "text");
                    int tabrow = ds.Tables[0].Rows.Count;
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread.Sheets[0].RowCount = 0;
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            Fpspread.Sheets[0].RowCount++;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Date of Exam";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread.Columns[0].Width = 50;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Session";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread.Columns[1].Width = 50;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Code";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread.Columns[1].Width = 50;
                            string subj = Convert.ToString(ds.Tables[0].Rows[j]["subject_code"]).Trim();
                            string subjname = Convert.ToString(ds.Tables[0].Rows[j]["subject_name"]).Trim();
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Branch";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread.Columns[1].Width = 50;
                            string edulevel = Convert.ToString(ds.Tables[0].Rows[j]["Edu_Level"]).Trim();
                            ds.Tables[0].DefaultView.RowFilter = " Edu_Level='" + Convert.ToString(ds.Tables[0].Rows[j]["Edu_Level"]) + "' ";
                            if (!hat.ContainsKey(edulevel))
                            {
                                hat.Add(edulevel, subjname);
                                Spdegree.InnerHtml = "DEGREE: " + edulevel + "";
                            }
                            string bran = Convert.ToString(ds.Tables[0].Rows[j]["course"]).Trim();
                            ds.Tables[0].DefaultView.RowFilter = " course='" + Convert.ToString(ds.Tables[0].Rows[j]["course"]) + "' ";
                            colSpanDV = ds.Tables[0].DefaultView;
                            if (BranchCodeReplication != Convert.ToString(ds.Tables[0].Rows[j]["course"]))
                            {
                                Fpspread.Sheets[0].Cells[j, 3].Text = Convert.ToString(bran).Trim();
                                Fpspread.Sheets[0].Cells[j, 3].VerticalAlign = VerticalAlign.Middle;
                                Fpspread.Sheets[0].Cells[j, 3].HorizontalAlign = HorizontalAlign.Left;
                                if (BranchCodeReplication != "")
                                    if (colSpanDV.Count > 0)
                                    {
                                        Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - colspanbranch - 1, 3, colspanbranch, 1);
                                    }
                                colspanbranch = 0;
                            }
                            colspanbranch++;
                            BranchCodeReplication = Convert.ToString(ds.Tables[0].Rows[j]["course"]).Trim();
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Q.P Code/Booklet code";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread.Columns[1].Width = 50;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Answer Paper Packet Number Alloted by College";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread.Columns[1].Width = 50;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread.Columns[1].Width = 50;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Answer scripts";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            Fpspread.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread.Columns[1].Width = 50;
                            string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + Convert.ToString(Session["collegecode"]).Trim() +"'";
                            DataSet ds1 = new DataSet();
                            ds1.Dispose();
                            ds1.Reset();
                            ds1 = dt.select_method_wo_parameter(strquery, "Text");
                            spF1College.InnerText = ds1.Tables[0].Rows[0]["Collname"].ToString();
                            string catagor = ds1.Tables[0].Rows[0]["category"].ToString();
                           // string[] strpa = Convert.ToString(ds1.Tables[0].Rows[0]["affliatedby"]).Trim().Split(',');
                            spcategory.InnerHtml = "(" + Convert.ToString(catagor).Trim() + " & " +Convert.ToString(ds1.Tables[0].Rows[0]["university"]).Trim()+ ")";
                            spF1Date.InnerText = "Month & year of Exam: " + Convert.ToString(prmonthyea).Trim() + "";
                            spHead.InnerText = "DESPATCH OF ANSWER PACKETS ";
                            dateoedel.InnerHtml = "Date of Delvery:" + Convert.ToString(ddlDate.SelectedValue).Trim() + "";
                            spsign.InnerHtml = "Signature of the Anna University Representative";
                            spsignchif.InnerHtml = "Signature of the Chief Superintendent";
                            spsig.InnerHtml = "Authorized Signatory office of COE";
                            spnbun.InnerHtml = "Received " + ds.Tables[0].Rows.Count + " bundles from exam cell";
                            ds.Tables[0].DefaultView.RowFilter = " subject_code='" + Convert.ToString(ds.Tables[0].Rows[j]["subject_code"]) + "' ";
                            colSpansub = ds.Tables[0].DefaultView;
                            if (subjcode != Convert.ToString(ds.Tables[0].Rows[j]["subject_code"]))
                            {
                                if (subjcode != "")
                                    if (colSpansub.Count > 0)
                                    {
                                        dsrow = j;
                                        inrow = j;
                                        dsrow = ds.Tables[0].Rows.Count - 1;
                                        Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - colspansubj - 1, 2, colspansubj, 1);
                                        Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - colspansubj - 1, 6, colspansubj, 1);
                                    }
                                colspansubj = 0;
                            }
                            colspansubj++;
                            if (dsrow == 0 && j == ds.Tables[0].Rows.Count - 1)
                            {
                                Fpspread.Sheets[0].SpanModel.Add(0, 2, ds.Tables[0].Rows.Count, 1);
                                Fpspread.Sheets[0].SpanModel.Add(0, 6, ds.Tables[0].Rows.Count, 1);
                            }
                            if (dsrow == j)
                            {
                                Fpspread.Sheets[0].SpanModel.Add(inrow, 2, colspansubj, 1);
                                Fpspread.Sheets[0].SpanModel.Add(inrow, 6, colspansubj, 1);
                            }
                            subjcode = Convert.ToString(ds.Tables[0].Rows[j]["subject_code"]).Trim();
                            if (!hat.ContainsKey(subj))
                            {
                                hat.Add(subj, bran);
                                Fpspread.Sheets[0].Cells[j, 2].Text = Convert.ToString(subj).Trim();
                                Fpspread.Sheets[0].Cells[j, 2].Font.Size = FontUnit.Medium;
                                Fpspread.Sheets[0].Cells[j, 2].VerticalAlign = VerticalAlign.Middle;
                                Fpspread.Sheets[0].Cells[j, 2].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread.Sheets[0].Cells[j, 6].Text = Convert.ToString(subjname).Trim();
                                Fpspread.Sheets[0].Cells[j, 6].Font.Size = FontUnit.Medium;
                                Fpspread.Sheets[0].Cells[j, 6].VerticalAlign = VerticalAlign.Middle;
                                Fpspread.Sheets[0].Cells[j, 6].HorizontalAlign = HorizontalAlign.Left;
                            }
                            Fpspread.Sheets[0].Cells[j, 7].Text = Convert.ToString(ds.Tables[0].Rows[j]["stucount"]).Trim();
                            Fpspread.Sheets[0].Cells[j, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread.Sheets[0].Cells[j, 7].Font.Size = FontUnit.Medium;
                            total += Convert.ToInt32(ds.Tables[0].Rows[j]["stucount"].ToString());
                        }
                        Fpspread.Sheets[0].SpanModel.Add(0, 0, Fpspread.Sheets[0].RowCount, 1);
                        Fpspread.Sheets[0].Cells[0, 0].Text = Convert.ToString(exdate).Trim();
                        Fpspread.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread.Sheets[0].Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
                        Fpspread.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread.Sheets[0].SpanModel.Add(0, 1, Fpspread.Sheets[0].RowCount, 1);
                        Fpspread.Sheets[0].Cells[0, 1].Text = Convert.ToString(ddlSession.SelectedValue).Trim();
                        Fpspread.Sheets[0].Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread.Sheets[0].Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
                        Fpspread.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread.Sheets[0].RowCount++;
                        Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - 1, 0, 1, 5);
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = "Total";
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(total).Trim();
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread.Sheets[0].RowCount++;
                        Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - 1, 0, 1, Fpspread.Sheets[0].Columns.Count);
                        if (ddlSession.SelectedValue.Trim() == "A.N")
                        {
                            sess = "Afternoon";
                        }
                        else
                        {
                            sess = "Forenoon";
                        }
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = "1.Certificate of opening of sessionwise package of Q.P for " + sess + "";
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread.Sheets[0].RowCount++;
                        Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - 1, 0, 1, Fpspread.Sheets[0].Columns.Count);
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = "2.Student Attendance Sheet original copies for " + sess + "";
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread.Sheets[0].PageSize = Fpspread.Sheets[0].RowCount;
                        Fpspread.Width = 900;
                        Fpspread.Height = 900;
                        Fpspread.SaveChanges();
                    }

                    else
                    {
                        lblerror.Text = "No Records Found";
                        lblerror.Visible = true;
                    }
                }

                else
                {
                    lblerror.Text = "Please select the date and session";
                    Fpspread.Visible = false;
                    lblerror.Visible = true;
                    btn_directprint.Visible = false;
                }
            }
            else
            {
                lblerror.Text = "Please select all field";
                Fpspread.Visible = false;
                lblerror.Visible = true;
                btn_directprint.Visible = false;
            }
        }
        catch (Exception ex)
        { da.sendErrorMail(ex, collegeCode, "DespatchOfAnswerPackets"); }
    }
}