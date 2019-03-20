using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using InsproDataAccess;
using System.Globalization;
using System.Configuration;

public partial class MarkMod_Invigilation : System.Web.UI.Page
{

    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string staff_code = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    DataSet dscol = new DataSet();
    Hashtable grandtotal = new Hashtable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    ReuasableMethods rs = new ReuasableMethods();
    string selQ = string.Empty;
    string seqstaff = string.Empty;
    string CycleTestname = string.Empty;
    string CycleTestno = string.Empty;

    Boolean checkedchk = false;

    bool checkbx = false;
    bool fnoon = false;
    bool anoon = false;
    DataRow dr;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!Request.FilePath.Contains("CAMHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/MarkMod/CAMHome.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        staff_code = (string)Session["Staff_Code"];

        if (!IsPostBack)
        {
            if (staff_code == "" || staff_code == null)
            {
                Response.Write("You Are not a Valid Staff");
                return;
            }

            CycleTest();
            showreport1.Visible = false;
            btn_save.Visible = false;


        }
    }

    #region CycleTest

    public void CycleTest()
    {
        try
        {
            cbl_cycletest.Items.Clear();
            cb_cycletest.Checked = false;
            txt_cycletest.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            string Query = "select distinct ci.criteria,ci.Criteria_no from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no  and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_cycletest.DataSource = ds;
                cbl_cycletest.DataTextField = "criteria";
                cbl_cycletest.DataValueField = "Criteria_no";
                cbl_cycletest.DataBind();
                if (cbl_cycletest.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_cycletest.Items.Count; i++)
                    {
                        cbl_cycletest.Items[i].Selected = true;
                    }
                    txt_cycletest.Text = "CycleTest(" + cbl_cycletest.Items.Count + ")";
                    cb_cycletest.Checked = true;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
    }

    protected void cb_cycletest_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_cycletest, cbl_cycletest, txt_cycletest, "cycletest", "--Select--");
            showreport1.Visible = false;
            btn_save.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
    }

    protected void cbl_cycletest_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_cycletest, cbl_cycletest, txt_cycletest, "cycletest", "--Select--");
            showreport1.Visible = false;
            btn_save.Visible = false;


        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
    }

    #endregion

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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
    }

    #endregion

    #region Go

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            fromtodate();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
    }

    #endregion

    #region fpspread

    private DataSet staffselectsession()
    {
        DataSet dsloaddetails = new DataSet();
        DataSet dtExistStaff = new DataSet();
        try
        {

            #region get Value
            string CycleTestNo = string.Empty;
            if (cbl_cycletest.Items.Count > 0)
                CycleTestNo = Convert.ToString(getCblSelectedValue(cbl_cycletest).ToUpper());
            if (!string.IsNullOrEmpty(CycleTestNo))
            {
                string ExistsStaffCode = string.Empty;
                seqstaff = "select distinct isa.hallNo,isa.criteriaNo,isa.examDate,isa.examSession,isa.examCode,isa.staff_code from internalSeatingArragement isa,CriteriaForInternal ci   where ci.Criteria_no=isa.criteriaNo and ci.Criteria_no in('" + CycleTestNo + "')";
                dtExistStaff.Clear();
                dtExistStaff = d2.select_method_wo_parameter(seqstaff, "Text");
                //ExistsStaffCode = Convert.ToString(dtExistStaff.Tables[0].Rows[existStaff]["staff_code"]).Trim();

                dtExistStaff.Tables[0].DefaultView.RowFilter = "ISNULL(staff_code,'') ='" + staff_code + "'";
                DataTable dtnew = dtExistStaff.Tables[0].DefaultView.ToTable();
                if (dtnew.Rows.Count > 0)
                {
                    selQ = "select distinct CONVERT(varchar(20),e.examFromTime,103) InDate,CONVERT(varchar(8),e.examFromTime,108) ExamInTime,e.examToTime,CONVERT(varchar(8),e.examToTime,103) OutDate,CONVERT(varchar(5),e.examToTime,108) ExamOutTime,e.examToTime,CONVERT(varchar(20),i.examDate,103) ExamDate,CONVERT(varchar(8),i.examDate,108) ExamTime,i.examDate, ci.criteria,i.hallNo,i.criteriaNo  from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,internalSeatingArragement i where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no  and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and i.examCode=e.exam_code  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and ci.Criteria_no in('" + CycleTestNo + "')";
                    dsloaddetails.Clear();
                    dsloaddetails = d2.select_method_wo_parameter(selQ, "Text");
                }
                else
                {
                    dtExistStaff.Tables[0].DefaultView.RowFilter = "staff_code =''";
                    dtnew = dtExistStaff.Tables[0].DefaultView.ToTable();
                    selQ = "select distinct CONVERT(varchar(20),e.examFromTime,103) InDate,CONVERT(varchar(8),e.examFromTime,108) ExamInTime,e.examToTime,CONVERT(varchar(8),e.examToTime,103) OutDate,CONVERT(varchar(5),e.examToTime,108) ExamOutTime,e.examToTime,CONVERT(varchar(20),i.examDate,103) ExamDate,CONVERT(varchar(8),i.examDate,108) ExamTime,i.examDate, ci.criteria,i.hallNo,i.criteriaNo  from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,internalSeatingArragement i where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no  and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and i.examCode=e.exam_code  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and i.staff_code='' and ci.Criteria_no in('" + CycleTestNo + "')";
                    dsloaddetails.Clear();
                    dsloaddetails = d2.select_method_wo_parameter(selQ, "Text");
                }

            }

            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
        return dsloaddetails;
    }

    private void loadspreadCount(DataSet ds)
    {
        try
        {
            int chk = 0;
            bool forenoon = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("date");
            dt.Columns.Add("hall");
            dt.Columns.Add("testno");
            dt.Columns.Add("fn");
            dt.Columns.Add("an");

            int sno = 0;
            DataTable dtnew = new DataTable();
            DateTime examinttime = new DateTime();
            DateTime an = new DateTime();
            DateTime fn = new DateTime();
            string examtime = string.Empty;
            string Antime = string.Empty;
            string Fntime = string.Empty;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                   
                    string date = Convert.ToString(ds.Tables[0].Rows[row]["ExamDate"]).Trim();
                    string hallno = Convert.ToString(ds.Tables[0].Rows[row]["hallNo"]).Trim();
                    string testno = Convert.ToString(ds.Tables[0].Rows[row]["criteriaNo"]).Trim();
                    examinttime = Convert.ToDateTime(ds.Tables[0].Rows[row]["ExamInTime"]);
                    an = Convert.ToDateTime("09:00:00");
                    fn = Convert.ToDateTime("12:00:00");
                    examtime = Convert.ToString(examinttime);
                    Antime = Convert.ToString(an);
                    Fntime = Convert.ToString(fn);
                    dr = dt.NewRow();
                    dr["date"] = date;
                    dr["hall"] = hallno;
                    dr["testno"] = testno;
                  
                    if ((!string.IsNullOrEmpty(examtime) && !string.IsNullOrEmpty(Antime)) || (!string.IsNullOrEmpty(examtime) && !string.IsNullOrEmpty(Fntime)))
                    {
                        string[] split = examtime.Split(' ');
                        string[] split1 = Antime.Split(' ');
                        string[] split2 = Fntime.Split(' ');

                        if ((split.Length > 0 && split1.Length > 0) || (split.Length > 0 && split2.Length > 0))
                        {
                            string ExamInTime = split[1];
                            string AnTime = split1[1];
                            string FnTime = split2[1];
                            if (Convert.ToDateTime(ExamInTime) < Convert.ToDateTime(AnTime))
                            {
                                chk = 1;
                                forenoon = false;
                                anoon = true;

                            }
                            else if (Convert.ToDateTime(ExamInTime) <= Convert.ToDateTime(FnTime))
                            {
                                chk = 1;
                                forenoon = true;
                                fnoon = true;
                              
                            }

                        }
                    }

                    dt.Rows.Add(dr);
                }
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Visible = true;
                DataSet dssave = new DataSet();
                string StaffTestexists = string.Empty;
                string TestNo = string.Empty;
                string CriteriaNumber = string.Empty;
                if (cbl_cycletest.Items.Count > 0)
                    TestNo = Convert.ToString(rs.GetSelectedItemsValue(cbl_cycletest));
                staff_code = (string)Session["Staff_Code"];
                if (!string.IsNullOrEmpty(staff_code) && !string.IsNullOrEmpty(TestNo))
                {
                    StaffTestexists = "select distinct isa.hallNo,isa.criteriaNo,isa.examDate,isa.examSession,isa.examCode,isa.staff_code from internalSeatingArragement isa  where staff_code='" + staff_code + "'";
                    //and criteriaNo in('" + CycleTestno + "')
                    dssave.Clear();
                    dssave = d2.select_method_wo_parameter(StaffTestexists, "Text");
                    if (dssave.Tables.Count > 0 && dssave.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int noofrecord = 0; noofrecord < ds.Tables[0].Rows.Count; noofrecord++)
                            {
                                string totalcriteriaNo = Convert.ToString(ds.Tables[0].Rows[noofrecord]["criteriaNo"]).Trim();

                                for (int saverecord = 0; saverecord < dssave.Tables[0].Rows.Count; saverecord++)
                                {
                                    string savecriteriaNo = Convert.ToString(dssave.Tables[0].Rows[saverecord]["criteriaNo"]).Trim();
                                    if (totalcriteriaNo == savecriteriaNo)
                                    {
                                        for (int k = 0; k < GridView1.Rows.Count; k++)
                                       {
                                           checkbx = true;
                                            if (forenoon == true)
                                            {
                                                dt.Rows[noofrecord]["fn"] = 1;
                                                dt.Rows[noofrecord]["an"] = 0;
                                            }
                                            else
                                            {
                                                dt.Rows[noofrecord]["fn"] = 0;
                                                dt.Rows[noofrecord]["an"] = 1;
                                            }
                                           
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                GridView1.DataSource = dt;
                GridView1.DataBind();
                showreport1.Visible = true;
                btn_save.Visible = true;

            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
    }

    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label fnlbl = (Label)e.Row.FindControl("lblfn");
            string fn = fnlbl.Text;
            Label anlbl = (Label)e.Row.FindControl("lblAn");
            string an = anlbl.Text;
            if (checkbx == true)
            {
                if (fn == "1")
                {
                    (e.Row.FindControl("cb_fn") as CheckBox).Checked = true;

                }
                else
                {
                    e.Row.Cells[3].Enabled = false;
                }
                if (an == "1")
                {
                    (e.Row.FindControl("cb_an") as CheckBox).Checked = true;

                }
                else
                {
                    e.Row.Cells[4].Enabled = false;
                }
            }
            if (fnoon == false)
            {
                e.Row.Cells[3].Enabled = false;
            }
            if (anoon == false)
            {
                e.Row.Cells[4].Enabled = false;
            }
            if (fn == "1" || an == "1")
            {
                e.Row.BackColor = Color.LightBlue;
            }
            else
            {
                e.Row.BackColor = Color.White;
            }
        }
    }

    #endregion

    #region save
    protected void btnattOk_Click(object sender, EventArgs e)
    {
        try
        {
            saveMessage.Visible = false;
            btn_save.Visible = false;
            savestafffortest();

        }
        catch
        {
        }
    }

    protected void btnattCancel_Click(object sender, EventArgs e)
    {
        try
        {
            saveMessage.Visible = false;
        }
        catch
        {
        }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            saveMessage.Visible = true;


        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
    }

    public void savestafffortest()
    {
        try
        {

          //  spreadDet1.SaveChanges();
            bool isSave = false;
           // int activeRow = spreadDet1.ActiveSheetView.ActiveRow;
           // int activeColumn = spreadDet1.ActiveSheetView.ActiveColumn;
            string selecttest = string.Empty;
            string internalseatingSaveqry = string.Empty;
            string internalseatingupdateqry = string.Empty;
            staff_code = (string)Session["Staff_Code"];
            DataTable dsstaffcode = new DataTable();
            DataTable dtdeletestaff = new DataTable();
            internalseatingupdateqry = "update internalSeatingArragement  set staff_code=''  where staff_code='" + staff_code + "'";
            dtdeletestaff.Clear();
            int a = dirAcc.updateData(internalseatingupdateqry);
            //DeleteQry = "delete from qPaperSetterStaff where subjectNo='" + SubjectNo + "' and examYear='" + examyear + "' and examMonth='" + exammonth + "'";
            //int a = dirAcc.deleteData(DeleteQry);
            foreach(GridViewRow row in GridView1.Rows)
            {
                int selected = 0;
                int selected1 = 0;
                CheckBox chkfn = (CheckBox)row.FindControl("cb_fn");
                if (chkfn.Checked == true)
                {
                    selected = 1;
                }              
                CheckBox chan = (CheckBox)row.FindControl("cb_an");
                if (chan.Checked == true)
                {
                    selected1 = 1;
                }
                if (selected == 1 || selected1 == 1)
                {
                    Label tstno = (Label)row.FindControl("lbltestno");
                    string criteriano = tstno.Text;
                    if (String.IsNullOrEmpty(selecttest))
                    {
                        selecttest = criteriano;
                    }
                    else
                    {
                        selecttest += ";" + criteriano;
                    }
                }
            }
            string[] split = selecttest.Split(';');

            if (split.Length > 0)
            {
                string testno = split[0];

                for (int i = 0; i < split.Length; i++)
                {
                    testno = split[i];

                    if (!string.IsNullOrEmpty(staff_code) && !string.IsNullOrEmpty(testno))
                    {
                        internalseatingSaveqry = "if exists(select distinct isa.hallNo,isa.criteriaNo,isa.examDate,isa.examSession,isa.examCode from internalSeatingArragement isa where isa.criteriaNo in('" + testno + "')) update internalSeatingArragement set staff_code='" + staff_code + "' where criteriaNo in('" + testno + "')";
                        //internalseatingSaveqry = "if exists(select distinct isa.hallNo,isa.criteriaNo,isa.examDate,isa.examSession,isa.examCode from internalSeatingArragement isa where isa.criteriaNo in('" + testno + "')) insert into internalSeatingArragement (staff_code)values('" + staff_code + "')";
                        dsstaffcode.Clear();
                        int res = dirAcc.insertData(internalseatingSaveqry);
                        if (res != 0)
                            isSave = true;
                    }

                }
            }


            if (isSave)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Saved SuccessFully";
            }
        }
        catch
        {
        }

    }

    #endregion

    #region Date

    public void fromtodate()
    {
        try
        {
            if (cbl_cycletest.Items.Count > 0)
                CycleTestname = Convert.ToString(getCblSelectedValue(cbl_cycletest).ToUpper());
            DataSet dtdate = new DataSet();
            string qry = "select tv.TextVal from textvaltable tv where TextCriteria='Invig'";
            dtdate.Clear();
            dtdate = d2.select_method_wo_parameter(qry, "Text");

            if (dtdate.Tables.Count > 0 && dtdate.Tables[0].Rows.Count > 0)
            {
                for (int date = 0; date < dtdate.Tables[0].Rows.Count; date++)
                {
                    string fromtodate = Convert.ToString(dtdate.Tables[0].Rows[date]["TextVal"]).Trim();
                    string[] split = fromtodate.Split('$');
                    if (split.Length > 0)
                    {
                        string allowedsession = split[0];
                        string from = split[1];
                        string to = split[2];
                        string testname = split[3];
                        string[] From_Date = from.Split('/');
                        string[] To_Date = to.Split('/');
                        DateTime fromdate = DateTime.Parse(From_Date[1] + "/" + From_Date[0] + "/" + From_Date[2]);
                        DateTime todate = DateTime.Parse(To_Date[1] + "/" + To_Date[0] + "/" + To_Date[2]);
                        DateTime todaydate = DateTime.Now;

                        if ((fromdate <= todaydate && todaydate >= todate) || (fromdate <= todaydate && todaydate <= todate))
                        {
                            DataSet ds = new DataSet();
                            ds = staffselectsession();
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                loadspreadCount(ds);
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "No Record Found!";
                            }
                        }
                        else
                        {
                            showreport1.Visible = false;
                            btn_save.Visible = false;
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Your Invigilation Period Expired";
                        }
                    }
                }
            }
            else
            {
                showreport1.Visible = false;
                btn_save.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Date Not Alloted";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }

    }

    #endregion

    #region alertclose

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        lblalerterr.Text = string.Empty;
        alertpopwindow.Visible = false;
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
       
            lblalerterror.Text = string.Empty;
            alertpopup.Visible = false;
            checkedchk = true;
      
    }

    #endregion

}