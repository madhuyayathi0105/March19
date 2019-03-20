/*
 * Author : Mohamed Idhris Sheik Dawood
 * Date Created  : 08-05-2017
 * 
 */

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class StudentMod_StudAdmissionReport : System.Web.UI.Page
{
    int collegeCode = 0;
    int userCode = 0;

    InsproDirectAccess DA = new InsproDirectAccess();

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
            if (!IsPostBack)
            {

                collegeCode = Convert.ToInt32(Convert.ToString(Session["collegecode"]));
                userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
                setLabelText();
                bindCollege();
                bindbatch();
                //bindType();
                bindEduLevel();
                bindSeattype();
                bindsem();

                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");

                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
            }
            lbl_validation.Visible = false;
            lblErr.Text = string.Empty;
        }
        catch { Response.Redirect("~/Default.aspx"); }
    }
    public void bindCollege()
    {
        try
        {
            //DataTable dtClg = new DataTable();
            //ddl_college.Items.Clear();
            //string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + userCode + " and cp.college_code=cf.college_code";
            //dtClg = DA.selectDataTable(selectQuery);
            //if (dtClg.Rows.Count > 0)
            //{
            //    ddl_college.DataSource = dtClg;
            //    ddl_college.DataTextField = "collname";
            //    ddl_college.DataValueField = "college_code";
            //    ddl_college.DataBind();
            //}

            cbl_College.Items.Clear();
            cb_College.Checked = true;
            txtCollege.Text = lblCollege.Text;
            DataTable dtClg = new DataTable();

            dtClg = DA.selectDataTable("select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + userCode + " and cp.college_code=cf.college_code");
            if (dtClg.Rows.Count > 0)
            {
                cbl_College.DataSource = dtClg;
                cbl_College.DataTextField = "collname";
                cbl_College.DataValueField = "college_code";
                cbl_College.DataBind();
                CallCheckBoxChangedEvent(cbl_College, cb_College, txtCollege, lblCollege.Text);
            }
        }
        catch (Exception ex) { ddl_college.Items.Clear(); }
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
        lbl.Add(lblCollege);
        fields.Add(0);

        //lbl.Add(lbl_stream);
        //fields.Add(1);

        lbl.Add(lbl_Sem);
        fields.Add(4);

        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    public void bindType()
    {
        //try
        //{
        //    lbl_stream.Text = useStreamShift();
        //    //ddl_strm.Items.Clear();
        //    //string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>''  and r.college_code='" + collegeCode + "'  order by type asc";

        //    //DataTable dtType = DA.selectDataTable(query);
        //    //if (dtType.Rows.Count > 0)
        //    //{
        //    //    ddl_strm.DataSource = dtType;
        //    //    ddl_strm.DataTextField = "type";
        //    //    ddl_strm.DataValueField = "type";
        //    //    ddl_strm.DataBind();
        //    //    ddl_strm.Enabled = true;
        //    //}
        //    //else
        //    //{
        //    //    ddl_strm.Enabled = false;
        //    //}

        //    cbl_Strm.Items.Clear();
        //    cb_Strm.Checked = true;
        //    txtStrm.Text = lbl_stream.Text;
        //    DataTable dtType = new DataTable();
        //    string collegeCode = GetSelectedItemsValue(cbl_College);
        //    dtType = DA.selectDataTable("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>''  and r.college_code in (" + collegeCode + ")  order by type asc");
        //    if (dtType.Rows.Count > 0)
        //    {
        //        txtStrm.Enabled = true;
        //        cbl_Strm.DataSource = dtType;
        //        cbl_Strm.DataTextField = "type";
        //        cbl_Strm.DataValueField = "type";
        //        cbl_Strm.DataBind();
        //        CallCheckBoxChangedEvent(cbl_Strm, cb_Strm, txtStrm, lbl_stream.Text);
        //    }
        //    else
        //    {
        //        txtStrm.Enabled = false;
        //    }
        //}
        //catch (Exception ex) { }
    }
    private void bindEduLevel()
    {
        try
        {

            cbl_EduLev.Items.Clear();
            cb_EduLev.Checked = true;
            txtEduLev.Text = lblEduLev.Text;
            DataTable dtEduLeve = new DataTable();

            string collegeCode = GetSelectedItemsValue(cbl_College);
            //string type = GetSelectedItemsValueAsString(cbl_Strm);

            //string query = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and type in ('" + type + "') and college_code in (" + collegeCode + ")";

            string query = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and college_code in (" + collegeCode + ")";


            dtEduLeve = DA.selectDataTable(query);
            if (dtEduLeve.Rows.Count > 0)
            {
                cbl_EduLev.DataSource = dtEduLeve;
                cbl_EduLev.DataTextField = "Edu_Level";
                cbl_EduLev.DataValueField = "Edu_Level";
                cbl_EduLev.DataBind();
                CallCheckBoxChangedEvent(cbl_EduLev, cb_EduLev, txtEduLev, lblEduLev.Text);
            }

            //ddlEduLev.Items.Clear();

            //DataTable dtEduLeve = new DataTable();

            //string collegeCode = GetSelectedItemsValue(cbl_College);

            //string query = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>''  and college_code in (" + collegeCode + ")";

            //dtEduLeve = DA.selectDataTable(query);
            //if (dtEduLeve.Rows.Count > 0)
            //{
            //    ddlEduLev.DataSource = dtEduLeve;
            //    ddlEduLev.DataTextField = "Edu_Level";
            //    ddlEduLev.DataValueField = "Edu_Level";
            //    ddlEduLev.DataBind();
            //}
        }
        catch { }
    }
    private string useStreamShift()
    {
        string useStrShft = "Stream";
        string streamcode = DA.selectScalarString("select value from Master_Settings where settings='Stream/Shift Rights' and usercode='" + userCode + "'").Trim();

        if (streamcode == "" || streamcode == "0")
        {
            useStrShft = "Stream";
        }
        if (streamcode.Trim() == "1")
        {
            useStrShft = "Shift";
        }
        if (streamcode.Trim() == "2")
        {
            useStrShft = "Stream";
        }
        return useStrShft;
    }
    public void bindbatch()
    {
        try
        {
            //ddl_batch.Items.Clear();
            //string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            //DataTable dtBatch = DA.selectDataTable(sqlyear);
            //if (dtBatch.Rows.Count > 0)
            //{
            //    ddl_batch.DataSource = dtBatch;
            //    ddl_batch.DataTextField = "batch_year";
            //    ddl_batch.DataValueField = "batch_year";
            //    ddl_batch.DataBind();
            //}

            cbl_Batch.Items.Clear();
            cb_Batch.Checked = true;
            txt_batch.Text = lbl_batch.Text;
            DataTable dtBatch = new DataTable();

            dtBatch = DA.selectDataTable("select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc");
            if (dtBatch.Rows.Count > 0)
            {
                cbl_Batch.DataSource = dtBatch;
                cbl_Batch.DataTextField = "batch_year";
                cbl_Batch.DataValueField = "batch_year";
                cbl_Batch.DataBind();
                CallCheckBoxChangedEvent(cbl_Batch, cb_Batch, txt_batch, lbl_batch.Text);
            }
        }
        catch { }
    }
    public void bindSeattype()
    {
        try
        {

            cbl_SeatType.Items.Clear();
            cb_SeatType.Checked = true;
            txtSeatType.Text = lblSeatType.Text;
            DataTable dtSeatType = new DataTable();
            string collegeCodes = GetSelectedItemsValue(cbl_College);

            dtSeatType = DA.selectDataTable("select distinct TextVal from textvaltable where TextCriteria='seat' and college_code in (" + collegeCodes + ")");
            if (dtSeatType.Rows.Count > 0)
            {
                cbl_SeatType.DataSource = dtSeatType;
                cbl_SeatType.DataTextField = "TextVal";
                cbl_SeatType.DataValueField = "TextVal";
                cbl_SeatType.DataBind();
                CallCheckBoxChangedEvent(cbl_SeatType, cb_SeatType, txtSeatType, lblSeatType.Text);
            }
        }
        catch { }
    }
    public void bindsem()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = true;
            txt_sem.Text = lbl_Sem.Text;
            DataTable dtSem = new DataTable();
            string collegeCode = GetSelectedItemsValue(cbl_College);
            string batch_year = GetSelectedItemsValue(cbl_Batch);
            dtSem = DA.selectDataTable("select distinct  Current_Semester from Registration where Batch_Year in (" + batch_year + ") and college_code in (" + collegeCode + ") and CC=0 and DelFlag='0' and Exam_Flag<>'Debar' order by Current_Semester asc");
            if (dtSem.Rows.Count > 0)
            {
                cbl_sem.DataSource = dtSem;
                cbl_sem.DataTextField = "Current_Semester";
                cbl_sem.DataValueField = "Current_Semester";
                cbl_sem.DataBind();
                CallCheckBoxChangedEvent(cbl_sem, cb_sem, txt_sem, lbl_Sem.Text);
            }
        }
        catch { }

    }
    protected void ddl_college_OnIndexChange(object sender, EventArgs e)
    {
        //bindType();
        bindbatch();
        bindEduLevel();
        bindSeattype();
        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void cb_College_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_College, cb_College, txtCollege, lblCollege.Text);
        //bindType();
        bindbatch();
        bindEduLevel();
        bindSeattype();
        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void cbl_College_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_College, cb_College, txtCollege, lblCollege.Text);
        //bindType();
        bindbatch();
        bindEduLevel();
        bindSeattype();
        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void cb_Strm_CheckedChanged(object sender, EventArgs e)
    {
        //CallCheckBoxChangedEvent(cbl_Strm, cb_Strm, txtStrm, lbl_stream.Text);
        bindEduLevel();
        bindSeattype();
        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void cbl_Strm_SelectedIndexChanged(object sender, EventArgs e)
    {
        //CallCheckBoxListChangedEvent(cbl_Strm, cb_Strm, txtStrm, lbl_stream.Text);
        bindEduLevel();
        bindSeattype();
        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void cb_EduLev_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_EduLev, cb_EduLev, txtEduLev, lblEduLev.Text);

        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void cbl_EduLev_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_EduLev, cb_EduLev, txtEduLev, lblEduLev.Text);

        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void ddl_batch_OnIndexChange(object sender, EventArgs e)
    {
        //bindType();

        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void cb_Batch_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_Batch, cb_Batch, txt_batch, lbl_batch.Text);

        //bindType();
        bindEduLevel();
        bindSeattype();
        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void cbl_Batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_Batch, cb_Batch, txt_batch, lbl_batch.Text);
        //bindType();
        bindEduLevel();
        bindSeattype();
        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void cb_SeatType_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_SeatType, cb_SeatType, txtSeatType, lblSeatType.Text);

        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void cbl_SeatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_SeatType, cb_SeatType, txtSeatType, lblSeatType.Text);

        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void ddl_strm_OnIndexChange(object sender, EventArgs e)
    {

        bindsem();

        //btn_go_Click(sender, e);
    }
    protected void ddlEduLev_OnIndexChange(object sender, EventArgs e)
    {
        bindsem();
    }
    protected void ddl_sem_OnIndexChange(object sender, EventArgs e)
    {

        // btn_go_Click(sender, e);
    }
    protected void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_sem, cb_sem, txt_sem, lbl_Sem.Text);

        //btn_go_Click(sender, e);
    }
    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_sem, cb_sem, txt_sem, lbl_Sem.Text);

        //btn_go_Click(sender, e);
    }
    protected void checkDate(object sender, EventArgs e)
    {
        try
        {
            DateTime fromdate = Convert.ToDateTime(txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2]);
            DateTime todate = Convert.ToDateTime(txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2]);

            if (fromdate <= todate)
            {
            }
            else
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('From Date Should Not Exceed To Date')", true);
                lblErr.Text = "From Date Should Not Exceed To Date";
            }
        }
        catch { }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {

            Printcontrol.Visible = false;
            rptprint.Visible = false;

            string collegeCodes = GetSelectedItemsValue(cbl_College);

            string seattypeval = GetSelectedItemsValueAsString(cbl_SeatType);

            string eduLeve = GetSelectedItemsValueAsString(cbl_EduLev); //ddlEduLev.Items.Count > 0 ? ddlEduLev.SelectedValue.Trim() : string.Empty;

            string batch_year = GetSelectedItemsValue(cbl_Batch);

            string cusem = GetSelectedItemsText(cbl_sem);

            string dateCheck = string.Empty;
            string dateTransCheck = string.Empty;

            string[] fromdate = Convert.ToString(txt_fromdate.Text).Split('/');
            DateTime fromdt = Convert.ToDateTime(fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2]);
            string[] todate = Convert.ToString(txt_todate.Text).Split('/');
            DateTime todt = Convert.ToDateTime(todate[1] + "/" + todate[0] + "/" + todate[2]);
            if (cbDateWise.Checked)
            {
                dateCheck = " and Adm_Date >='" + fromdt.ToString("MM/dd/yyyy") + "' and Adm_Date <='" + todt.ToString("MM/dd/yyyy") + "'  ";
                dateTransCheck = " and Transferdate >='" + fromdt.ToString("MM/dd/yyyy") + "' and Transferdate <='" + todt.ToString("MM/dd/yyyy") + "'  ";
            }
            DataTable dtAdmdata = new DataTable();
            DataTable dtTransdata = new DataTable();
            if (collegeCodes != string.Empty && batch_year != string.Empty && eduLeve != string.Empty && seattypeval != string.Empty && cusem != string.Empty)
            {
                string selectquery = "select a.seattype,(select t.textval from textvaltable t where t.TextCode=a.seattype) as seatval,r.degree_code,r.Batch_Year,r.college_code,c.Edu_Level,convert(varchar(10),r.Adm_Date,103) AdmDate,r.Adm_Date,convert(varchar(10),a.date_applied,103) AppliedDate,r.Current_Semester  from Registration r, applyn a,Course c,Degree d where r.App_No = a.app_no and c.Course_Id=d.Course_Id and r.degree_code=d.degree_code  and r.college_code in (" + collegeCodes + ") and r.Batch_Year in (" + batch_year + ")  and c.Edu_Level in ('" + eduLeve + "') and a.seattype in (select t.textcode from textvaltable t where t.textval in ('" + seattypeval + "') and  t.college_code in (" + collegeCodes + "))";
                string selTransquery = "select a.seattype,(select t.textval from textvaltable t where t.TextCode=a.seattype) as seatval,r.degree_code,r.Batch_Year,r.college_code,c.Edu_Level,convert(varchar(10),r.Adm_Date,103) AdmDate,convert(varchar(10),a.date_applied,103) AppliedDate,convert(varchar(10),st.transferdate,103) transferdate,r.Current_Semester  from Registration r, applyn a,Course c,Degree d,st_student_Transfer st where r.App_No = a.app_no and r.app_no=st.appno and a.app_no=st.appno and c.Course_Id=d.Course_Id and r.degree_code=d.degree_code  and r.college_code in (" + collegeCodes + ") and r.Batch_Year in (" + batch_year + ")  and c.Edu_Level in ('" + eduLeve + "') and a.seattype in (select t.textcode from textvaltable t where t.textval in ('" + seattypeval + "') and  t.college_code in (" + collegeCodes + "))";

                dtAdmdata = DA.selectDataTable(selectquery);
                dtTransdata = DA.selectDataTable(selTransquery);
                if (dtAdmdata.Rows.Count > 0)
                {
                    DataTable dtCurCol = new DataTable();
                    #region Datatable Column generation
                    dtCurCol.Columns.Add("College");
                    dtCurCol.Columns.Add("EduLevel");
                    dtCurCol.Columns.Add("Degree");
                    int cnt = 0;
                    bool Isadded = false;
                    for (int seatI = 0; seatI < cbl_SeatType.Items.Count; seatI++)
                    {
                        if (cbl_SeatType.Items[seatI].Selected)
                        {
                            int Ch = 0;
                            string seatType = cbl_SeatType.Items[seatI].Value;
                            for (int semI = 0; semI < cbl_sem.Items.Count; semI++)
                            {
                                if (cbl_sem.Items[semI].Selected)
                                {
                                    dtCurCol.Columns.Add(seatType + "#" + cbl_sem.Items[semI]);
                                    Ch++;
                                }
                            }
                            dtCurCol.Columns.Add(seatType + "#Total");
                            if (!Isadded)
                            {
                                cnt = Ch + 1;
                                Isadded = true;
                            }
                        }
                    }
                    dtCurCol.Columns.Add("Grand Total#Grand Total");
                    //transfer
                    int TransCol = 0;
                    //   bool IsaddedTrans = false;
                    for (int seatI = 0; seatI < cbl_SeatType.Items.Count; seatI++)
                    {
                        if (cbl_SeatType.Items[seatI].Selected)
                        {
                            string seatType = cbl_SeatType.Items[seatI].Value;
                            dtCurCol.Columns.Add(seatType + "#" + "Transfer");
                            TransCol++;
                        }
                    }
                    if (TransCol > 0)
                    {
                        dtCurCol.Columns.Add("Total#Transfer");
                        TransCol++;
                    }
                    bool headerNotAdded = true;
                    int serialNo = 0;
                    #endregion
                    Dictionary<int, int> dicGrandTotal = new Dictionary<int, int>();
                    for (int clgI = 0; clgI < cbl_College.Items.Count; clgI++)
                    {
                        if (cbl_College.Items[clgI].Selected)
                        {
                            Dictionary<int, int> dicCurColVal = new Dictionary<int, int>();
                            string curCollege = cbl_College.Items[clgI].Value;

                            string colAcr = DA.selectScalarString("select Coll_acronymn,collname from collinfo where college_code='" + curCollege + "'");

                            serialNo++;

                            for (int eduLevI = 0; eduLevI < cbl_EduLev.Items.Count; eduLevI++)
                            {
                                if (cbl_EduLev.Items[eduLevI].Selected)
                                {
                                    string EduLevel = cbl_EduLev.Items[eduLevI].Value;

                                    DataTable dtDegDet = DA.selectDataTable("select distinct d.Degree_Code,dt.Dept_Name,dt.dept_acronym,c.Edu_Level,r.college_code from Degree d, course c, Department dt, Registration r where d.Course_Id=c.Course_Id and d.Degree_Code=r.degree_code and d.Dept_Code=dt.Dept_Code and r.Batch_Year in (" + batch_year + ") and r.college_code in (" + curCollege + ")  ");
                                    if (dtDegDet.Rows.Count > 0)
                                    {

                                        for (int degI = 0; degI < dtDegDet.Rows.Count; degI++)
                                        {
                                            string curDegAcr = Convert.ToString(dtDegDet.Rows[degI]["dept_acronym"]);
                                            string curDegCode = Convert.ToString(dtDegDet.Rows[degI]["Degree_Code"]);

                                            DataRow drNew = dtCurCol.NewRow();
                                            drNew["College"] = colAcr;
                                            drNew["EduLevel"] = EduLevel;
                                            drNew["Degree"] = curDegAcr;

                                            int totalCnt = 0;
                                            int gTotalCnt = 0;
                                            #region
                                            for (int semI = 3; semI < (dtCurCol.Columns.Count - TransCol); semI++)
                                            {
                                                string[] seat_sem = Convert.ToString(dtCurCol.Columns[semI].ColumnName).Split('#');
                                                if (!dtCurCol.Columns[semI].ColumnName.Contains("Total"))
                                                {
                                                    dtAdmdata.DefaultView.RowFilter = " seatval='" + seat_sem[0] + "'  and degree_code ='" + curDegCode + "' and Batch_Year in (" + batch_year + ")  and college_code='" + curCollege + "' and Edu_Level='" + EduLevel + "' and Current_Semester='" + seat_sem[1] + "'" + dateCheck;
                                                    DataTable dtFnlCount = dtAdmdata.DefaultView.ToTable();
                                                    drNew[dtCurCol.Columns[semI].ColumnName] = dtFnlCount.Rows.Count;
                                                    totalCnt += dtFnlCount.Rows.Count;
                                                    gTotalCnt += dtFnlCount.Rows.Count;

                                                    if (!dicCurColVal.ContainsKey(semI))
                                                    {
                                                        dicCurColVal.Add(semI, dtFnlCount.Rows.Count);
                                                    }
                                                    else
                                                    {
                                                        dicCurColVal[semI] += dtFnlCount.Rows.Count;
                                                    }
                                                }
                                                else
                                                {
                                                    drNew[dtCurCol.Columns[semI].ColumnName] = totalCnt;
                                                    if (!dicCurColVal.ContainsKey(semI))
                                                    {
                                                        dicCurColVal.Add(semI, totalCnt);
                                                    }
                                                    else
                                                    {
                                                        dicCurColVal[semI] += totalCnt;
                                                    }
                                                    totalCnt = 0;
                                                }

                                            }
                                            drNew["Grand Total#Grand Total"] = gTotalCnt;
                                            dtCurCol.Rows.Add(drNew);
                                            if (!dicCurColVal.ContainsKey(-1))
                                            {
                                                dicCurColVal.Add(-1, gTotalCnt);
                                            }
                                            else
                                            {
                                                dicCurColVal[-1] += gTotalCnt;
                                            }
                                            #endregion
                                            //transfer
                                            if (TransCol > 0 && dtTransdata.Rows.Count > 0)
                                            {
                                                for (int semI = (dtCurCol.Columns.Count - TransCol); semI < dtCurCol.Columns.Count; semI++)
                                                {
                                                    string[] seat_sem = Convert.ToString(dtCurCol.Columns[semI].ColumnName).Split('#');
                                                    if (!dtCurCol.Columns[semI].ColumnName.Contains("Total"))
                                                    {
                                                        dtTransdata.DefaultView.RowFilter = " seatval='" + seat_sem[0] + "'  and degree_code ='" + curDegCode + "' and Batch_Year in (" + batch_year + ")  and college_code='" + curCollege + "' and Edu_Level='" + EduLevel + "' " + dateTransCheck;
                                                        DataTable dtFnlCount = dtTransdata.DefaultView.ToTable();
                                                        drNew[dtCurCol.Columns[semI].ColumnName] = dtFnlCount.Rows.Count;
                                                        totalCnt += dtFnlCount.Rows.Count;
                                                        gTotalCnt += dtFnlCount.Rows.Count;

                                                        if (!dicCurColVal.ContainsKey(semI))
                                                        {
                                                            dicCurColVal.Add(semI, dtFnlCount.Rows.Count);
                                                        }
                                                        else
                                                        {
                                                            dicCurColVal[semI] += dtFnlCount.Rows.Count;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        drNew[dtCurCol.Columns[semI].ColumnName] = totalCnt;
                                                        if (!dicCurColVal.ContainsKey(semI))
                                                        {
                                                            dicCurColVal.Add(semI, totalCnt);
                                                        }
                                                        else
                                                        {
                                                            dicCurColVal[semI] += totalCnt;
                                                        }
                                                        totalCnt = 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (dtCurCol.Rows.Count > 0)
                            {
                                if (headerNotAdded)
                                {
                                    #region Column Header Creation
                                    spreadStudList.Sheets[0].RowCount = 0;
                                    spreadStudList.Sheets[0].ColumnCount = 0;
                                    spreadStudList.Sheets[0].ColumnHeader.RowCount = 2;
                                    spreadStudList.CommandBar.Visible = false;
                                    spreadStudList.Sheets[0].ColumnCount = (dtCurCol.Columns.Count + 1);

                                    spreadStudList.Sheets[0].RowHeader.Visible = false;
                                    spreadStudList.Sheets[0].AutoPostBack = false;

                                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                    darkstyle.ForeColor = Color.Black;
                                    spreadStudList.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Text = " S.No";
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                    spreadStudList.Sheets[0].Columns[0].Locked = true;
                                    spreadStudList.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                                    spreadStudList.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                                    spreadStudList.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                    spreadStudList.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                    spreadStudList.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    spreadStudList.Columns[0].Width = 50;

                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblCollege.Text;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                                    spreadStudList.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                                    spreadStudList.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                                    spreadStudList.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                                    spreadStudList.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                                    spreadStudList.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    spreadStudList.Sheets[0].Columns[1].Locked = true;
                                    spreadStudList.Columns[1].Width = 150;

                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Text = "EduLevel";
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                                    spreadStudList.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                                    spreadStudList.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                                    spreadStudList.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                    spreadStudList.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                    spreadStudList.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    spreadStudList.Sheets[0].Columns[2].Locked = true;
                                    spreadStudList.Columns[2].Width = 100;

                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                                    spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                                    spreadStudList.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                                    spreadStudList.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                                    spreadStudList.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                    spreadStudList.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                                    spreadStudList.Sheets[0].Columns[3].Locked = true;
                                    spreadStudList.Columns[3].Width = 150;


                                    ArrayList arr = new ArrayList();
                                    ArrayList arrTrans = new ArrayList();
                                    int curColTrans = 0;
                                    for (int semI = 3; semI < dtCurCol.Columns.Count; semI++)
                                    {
                                        int curCol = semI + 1;
                                        string[] seat_sem = Convert.ToString(dtCurCol.Columns[semI].ColumnName).Split('#');

                                        if (seat_sem[0].Contains("Grand Total"))
                                        {
                                            spreadStudList.Sheets[0].ColumnHeader.Cells[0, curCol].Text = seat_sem[0];
                                        }
                                        else
                                        {
                                            if (seat_sem[1].Trim() != "Transfer")
                                                spreadStudList.Sheets[0].ColumnHeader.Cells[0, curCol].Text = seat_sem[0] + " - " + lbl_Sem.Text;
                                            else
                                                spreadStudList.Sheets[0].ColumnHeader.Cells[0, curCol].Text = seat_sem[1];
                                        }
                                        if (seat_sem[1].Trim() != "Transfer")
                                        {
                                            if (!arr.Contains(seat_sem[0]))
                                            {
                                                if (seat_sem[0].Trim() != "Grand Total")
                                                    spreadStudList.Sheets[0].ColumnHeaderSpanModel.Add(0, curCol, 1, cnt);
                                                else
                                                    spreadStudList.Sheets[0].ColumnHeaderSpanModel.Add(0, curCol, 2, 1);
                                                arr.Add(seat_sem[0]);
                                            }
                                        }
                                        else
                                        {
                                            curColTrans = semI + 1;
                                            if (!arrTrans.Contains(seat_sem[0]))
                                            {
                                                spreadStudList.Sheets[0].ColumnHeaderSpanModel.Add(0, curColTrans, 1, TransCol);
                                                arrTrans.Add(seat_sem[0]);
                                            }
                                        }

                                        if (seat_sem[1].Trim() != "Transfer")
                                            spreadStudList.Sheets[0].ColumnHeader.Cells[1, curCol].Text = seat_sem[1];
                                        else
                                            spreadStudList.Sheets[0].ColumnHeader.Cells[1, curCol].Text = seat_sem[0];

                                        spreadStudList.Sheets[0].ColumnHeader.Cells[0, curCol].HorizontalAlign = HorizontalAlign.Center;
                                        spreadStudList.Sheets[0].ColumnHeader.Cells[0, curCol].Font.Size = FontUnit.Medium;
                                        spreadStudList.Sheets[0].ColumnHeader.Cells[0, curCol].Font.Bold = true;
                                        spreadStudList.Sheets[0].ColumnHeader.Cells[0, curCol].Font.Name = "Book Antiqua";


                                        spreadStudList.Sheets[0].ColumnHeader.Cells[1, curCol].HorizontalAlign = HorizontalAlign.Center;
                                        spreadStudList.Sheets[0].ColumnHeader.Cells[1, curCol].Font.Size = FontUnit.Medium;
                                        spreadStudList.Sheets[0].ColumnHeader.Cells[1, curCol].Font.Bold = true;
                                        spreadStudList.Sheets[0].ColumnHeader.Cells[1, curCol].Font.Name = "Book Antiqua";

                                        spreadStudList.Sheets[0].Columns[curCol].Font.Name = "Book Antiqua";
                                        spreadStudList.Sheets[0].Columns[curCol].Font.Size = FontUnit.Medium;
                                        spreadStudList.Sheets[0].Columns[curCol].HorizontalAlign = HorizontalAlign.Center;
                                        spreadStudList.Sheets[0].Columns[curCol].Locked = true;
                                        spreadStudList.Columns[curCol].Width = 80;
                                    }

                                    spreadStudList.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                                    spreadStudList.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                                    spreadStudList.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                                    spreadStudList.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                                    spreadStudList.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadStudList.Sheets[0].ColumnCount - 1, 2, 1);
                                    #endregion
                                    headerNotAdded = false;
                                }

                                for (int fnlRowI = 0; fnlRowI < dtCurCol.Rows.Count; fnlRowI++)
                                {
                                    spreadStudList.Sheets[0].RowCount++;
                                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo);
                                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtCurCol.Rows[0]["College"]);

                                    for (int fnlColI = 1; fnlColI < dtCurCol.Columns.Count; fnlColI++)
                                    {
                                        int curCol = fnlColI + 1;
                                        spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, curCol].Text = Convert.ToString(dtCurCol.Rows[fnlRowI][fnlColI]);
                                    }
                                }

                                spreadStudList.Sheets[0].RowCount++;
                                int lastRow = spreadStudList.Sheets[0].RowCount - 1;
                                spreadStudList.Sheets[0].Cells[lastRow, 1].Text = "TOTAL";

                                for (int fnlColI = 3; fnlColI < (dtCurCol.Columns.Count - TransCol); fnlColI++)
                                {
                                    int curCol = fnlColI + 1;
                                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, curCol].Text = Convert.ToString(dicCurColVal[fnlColI]);
                                    if (!dicGrandTotal.ContainsKey(fnlColI))
                                        dicGrandTotal.Add(fnlColI, dicCurColVal[fnlColI]);
                                    else
                                        dicGrandTotal[fnlColI] += dicCurColVal[fnlColI];
                                }
                                spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, (dtCurCol.Columns.Count - TransCol)].Text = Convert.ToString(dicCurColVal[-1]);
                                if (!dicGrandTotal.ContainsKey(-1))
                                    dicGrandTotal.Add(-1, dicCurColVal[-1]);
                                else
                                    dicGrandTotal[-1] += dicCurColVal[-1];
                                //transfer total 
                                for (int fnlColI = (dtCurCol.Columns.Count - TransCol); fnlColI < dtCurCol.Columns.Count; fnlColI++)
                                {
                                    int curCol = fnlColI + 1;
                                    double grandtotal = 0;//02.06.17 barath
                                    if (dicCurColVal.ContainsKey(fnlColI))
                                        double.TryParse(Convert.ToString(dicCurColVal[fnlColI]), out grandtotal);
                                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, curCol].Text = Convert.ToString(grandtotal);
                                    if (!dicGrandTotal.ContainsKey(fnlColI))
                                        dicGrandTotal.Add(fnlColI, Convert.ToInt32(grandtotal));
                                    else
                                        dicGrandTotal[fnlColI] += Convert.ToInt32(grandtotal);
                                }
                                dtCurCol.Clear();
                            }
                        }
                    }
                    if (dicGrandTotal.Count > 0)
                    {
                        spreadStudList.Sheets[0].RowCount++;
                        int lastRow = spreadStudList.Sheets[0].RowCount - 1;
                        spreadStudList.Sheets[0].Cells[lastRow, 1].Text = "GRAND TOTAL";

                        for (int fnlColI = 3; fnlColI < (dtCurCol.Columns.Count - TransCol); fnlColI++)
                        {
                            int curCol = fnlColI + 1;
                            spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, curCol].Text = Convert.ToString(dicGrandTotal[fnlColI]);

                        }
                        spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, (dtCurCol.Columns.Count - TransCol)].Text = Convert.ToString(dicGrandTotal[-1]);
                        //transfer grandtotal
                        for (int fnlColI = (dtCurCol.Columns.Count - TransCol); fnlColI < dtCurCol.Columns.Count; fnlColI++)
                        {
                            int curCol = fnlColI + 1;
                            spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, curCol].Text = Convert.ToString(dicGrandTotal[fnlColI]);

                        }
                    }
                    spreadStudList.Visible = true;
                    spreadStudList.Sheets[0].PageSize = spreadStudList.Sheets[0].RowCount;
                    spreadStudList.Height = 320;
                    spreadStudList.SaveChanges();
                    rptprint.Visible = true;
                }
                else
                {
                    spreadStudList.Visible = false;
                    lblErr.Text = "No Records Found";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Records Found')", true);
                }
            }
            else
            {
                lblErr.Text = "Please Select  " + lblCollege.Text + ", Education Level, Batch, SeatType and " + lbl_Sem.Text + "";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select  " + lblCollege.Text + ", Education Level, Batch, SeatType and " + lbl_Sem.Text + "')", true);
            }
        }
        catch
        {
            spreadStudList.Visible = false;
            lblErr.Text = "No Records Found";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Records Found')", true);
        }
    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Student Admission Report";
            string pagename = "RegNoAllocation.aspx";
            Printcontrol.loadspreaddetails(spreadStudList, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                new DAccess2().printexcelreport(spreadStudList, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch (Exception ex) { }

    }
    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch (Exception ex) { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private List<string> GetSelectedItemsValueList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Value);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }
    private List<string> GetSelectedItemsTextList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Text);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }
    private List<string> GetItemsValueList(CheckBoxList cblItems)
    {
        System.Collections.Generic.List<string> lsItems = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblItems.Items.Count; list++)
            {
                lsItems.Add(cblItems.Items[list].Value);
            }
        }
        catch { lsItems.Clear(); }
        return lsItems;
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
}