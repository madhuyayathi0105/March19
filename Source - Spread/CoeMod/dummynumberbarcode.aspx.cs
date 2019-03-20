/*
 * Page reconstructed by Idhris 16-02-2017
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using InsproDataAccess;
using System.Web.UI.WebControls;
using System.Text;
using System.Linq;
using System.Configuration;

public partial class dummynumberbarcode : System.Web.UI.Page
{
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    ReuasableMethods reUse = new ReuasableMethods();

    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    //Added by Idhris 16-02-2017
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
            collegeCode = Session["collegecode"].ToString();
            userCode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (!Page.IsPostBack)
            {
                try
                {
                    bindCollege();
                    cb_College_CheckedChanged(sender, e);
                }
                catch
                {
                }
            }
            else
            {
                clearGrid();
            }
            collegeCode = reUse.GetSelectedItemsValue(cbl_College);
        }
        catch (Exception ex)
        {
        }
    }
    public void bindCollege()
    {
        try
        {
            txt_College.Text = "College";
            cb_College.Checked = true;
            cbl_College.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + userCode + " and cp.college_code=cf.college_code";
            DataTable dtCollege = dirAccess.selectDataTable(selectQuery);
            if (dtCollege.Rows.Count > 0)
            {
                cbl_College.DataSource = dtCollege;
                cbl_College.DataTextField = "collname";
                cbl_College.DataValueField = "college_code";
                cbl_College.DataBind();
            }
            reUse.CallCheckBoxChangedEvent(cbl_College, cb_College, txt_College, "College");
        }
        catch { }
    }
    protected void cb_College_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            reUse.CallCheckBoxChangedEvent(cbl_College, cb_College, txt_College, "College");
            collegeCode = reUse.GetSelectedItemsValue(cbl_College);
            bindyear();
            bindmonth();
            ddlYear_SelectedIndexChanged(sender, e);
            //typeChange();
        }
        catch { }
    }
    protected void cbl_College_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            reUse.CallCheckBoxListChangedEvent(cbl_College, cb_College, txt_College, "College");
            collegeCode = reUse.GetSelectedItemsValue(cbl_College);
            bindyear();
            bindmonth();
            ddlYear_SelectedIndexChanged(sender, e);
            //typeChange();
        }
        catch { }
    }
    public void bindyear()
    {
        try
        {
            ddlYear.Items.Clear();
            if (string.IsNullOrEmpty(collegeCode))
            {
                return;
            }

            DataSet ds = reUse.Examyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
            }
        }
        catch { }
    }
    public void bindmonth()
    {
        try
        {
            ddlMonth.Items.Clear();

            string year = ddlYear.SelectedItem.Text;
            DataTable dtMon = dirAccess.selectDataTable("select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year + "'");
            if (dtMon.Rows.Count > 0)
            {
                ddlMonth.DataSource = dtMon;
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataBind();
            }
        }
        catch { }
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadExamDate();
        loadSubject();
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindmonth();
        loadExamDate();
        loadSubject();
    }
    protected void ddlGenType_IndexChange(object sender, EventArgs e)
    {
        if (ddlGenType.SelectedIndex == 0)
        {
            trSubjectDet.Visible = false;
        }
        else
        {
            trSubjectDet.Visible = true;
            loadExamDate();
            loadSubject();
        }
    }
    private void loadExamDate()
    {
        try
        {
            ddlExDate.Items.Clear();
            string selQ = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date,exdt.Exam_date from exmtt_det as exdt,exmtt as exm where exm.exam_code=exdt.exam_code and exm.exam_month=" + ddlMonth.SelectedItem.Value.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + "  and exdt.coll_code in (" + collegeCode + ")  order by exdt.Exam_date";
            DataTable dtExDate = dirAccess.selectDataTable(selQ);
            if (dtExDate.Rows.Count > 0)
            {
                ddlExDate.DataSource = dtExDate;
                ddlExDate.DataTextField = "Exam_date";
                ddlExDate.DataValueField = "Exam_date";
                ddlExDate.DataBind();
            }
        }
        catch { }
    }
    protected void ddlExdate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadSubject();
        }
        catch { }
    }
    protected void ddlsession_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadSubject();
        }
        catch { }
    }
    private void loadSubject()
    {
        ddlsubject.Items.Clear();
        try
        {
            string examMonth = ddlMonth.SelectedValue.Trim();
            string examYear = ddlYear.SelectedValue.Trim();

            //string subjectQ = "select distinct subject_name+'-'+subject_code as subjectNameCode,subject_code from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s where s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "'";
            string session = string.Empty;
            if (ddlsession.SelectedIndex != 0)
            {
                session = " and exam_session ='" + ddlsession.SelectedItem.Text + " ' ";
            }
            string subjectQ = "select distinct s.subject_Name+'-'+s.subject_code as subjectNameCode ,s.Subject_code  from subject s,exmtt e,exmtt_det ex,sub_sem where sub_sem.subtype_no=s.subtype_no  and s.subject_no=ex.subject_no and ex.coll_code in (" + collegeCode + ") and ex.exam_Date=convert(datetime,'" + ddlExDate.SelectedValue.ToString() + "',103)and ex.exam_code=e.exam_code and e.Exam_Month=" + ddlMonth.SelectedValue.ToString() + " and e.Exam_Year=" + ddlYear.SelectedValue.ToString() + " and e.exam_type='Univ' " + session;
            DataTable dtSubject = dirAccess.selectDataTable(subjectQ);
            if (dtSubject.Rows.Count > 0)
            {
                ddlsubject.DataSource = dtSubject;
                ddlsubject.DataTextField = "subjectNameCode";
                ddlsubject.DataValueField = "subject_code";
                ddlsubject.DataBind();

                ddlsubject.Items.Insert(0, "All");
            }
        }
        catch { }
    }
    //Dummy Numbers Generation
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            string examMonth = ddlMonth.SelectedValue.Trim();
            string examYear = ddlYear.SelectedValue.Trim();

            string selectSetValQ = "select value from master_settings where settings='Starting DummyNo' and usercode='" + userCode + "'";
            int startDummyNo = dirAccess.selectScalarInt(selectSetValQ);
            if (startDummyNo > 0 && examMonth != string.Empty && examYear != string.Empty)
            {
                if (ddlGenType.SelectedIndex == 0)
                {
                    //Common Number Generation
                    string noOfStudentsQ = "select COUNT(distinct(r.roll_no)) from exam_application ea, Exam_Details ed,Registration r where r.Roll_No=ea.roll_no and  ea.exam_code = ed.exam_code and   ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and r.college_code in (" + collegeCode + ") ";
                    int noOfStudents = dirAccess.selectScalarInt(noOfStudentsQ);

                    string noOfStudentsColQ = "select COUNT(distinct(r.roll_no)) as cnt,r.college_code as coll_code  from exam_application ea, Exam_Details ed,Registration r where r.Roll_No=ea.roll_no and  ea.exam_code = ed.exam_code  and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and r.college_code in (" + collegeCode + ")  group by college_code ";
                    DataTable dtColStudCount = dirAccess.selectDataTable(noOfStudentsColQ);
                    if (noOfStudents > 0 && dtColStudCount.Rows.Count > 0)
                    {
                        Dictionary<string, int> dicColStudCnt = new Dictionary<string, int>();
                        for (int rowI = 0; rowI < dtColStudCount.Rows.Count; rowI++)
                        {
                            string colCode = Convert.ToString(dtColStudCount.Rows[rowI]["coll_code"]);
                            if (dicColStudCnt.ContainsKey(colCode))
                            {
                                dicColStudCnt[colCode] += Convert.ToInt32(dtColStudCount.Rows[rowI]["cnt"]);
                            }
                            else
                            {
                                dicColStudCnt.Add(colCode, Convert.ToInt32(dtColStudCount.Rows[rowI]["cnt"]));
                            }
                        }
                        generateDummyNumber(examMonth, examYear, startDummyNo, noOfStudents, dicColStudCnt);
                    }
                    else
                    {
                       // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Students Available')", true);

                        lblAlertMsg.Text = "No Students Available";
                        divPopAlert.Visible = true;
                    }
                }
                else
                {
                    //Subject Wise Number generation
                    if (ddlsubject.Items.Count > 0)
                    {
                        string[] examDateArr = ddlExDate.SelectedItem.Text.Split('-');
                        string examDate = examDateArr[1] + "/" + examDateArr[0] + "/" + examDateArr[2];
                        string subject_code;

                        List<string> lstSubjecCodes = new List<string>();
                        if (ddlsubject.SelectedIndex == 0)
                        {
                            StringBuilder sbSubjectCodes = new StringBuilder();
                            for (int itemI = 1; itemI < ddlsubject.Items.Count; itemI++)
                            {
                                sbSubjectCodes.Append(ddlsubject.Items[itemI].Value + "','");
                                lstSubjecCodes.Add(ddlsubject.Items[itemI].Value);
                            }
                            if (sbSubjectCodes.Length > 3)
                                sbSubjectCodes.Remove(sbSubjectCodes.Length - 3, 3);
                            subject_code = sbSubjectCodes.ToString();
                        }
                        else
                        {
                            subject_code = ddlsubject.SelectedValue.ToString();
                            lstSubjecCodes.Add(subject_code);
                        }

                        string session = string.Empty;
                        if (ddlsession.SelectedIndex != 0)
                        {
                            session = " and etd.exam_session ='" + ddlsession.SelectedItem.Text + " ' ";
                        }

                        string noOfStudentsQ = "select count(distinct roll_no) from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,exmtt et,exmtt_det etd  where s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code  and et.exam_code=etd.exam_code and et.degree_code=ed.degree_code and et.batchTo=ed.batch_year and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and etd.subject_no= ead.subject_no and s.subject_no=etd.subject_no  and ead.ExAttendance=1 and etd.coll_code in (" + collegeCode + ") and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and subject_code in ('" + subject_code + "') and etd.exam_date='" + examDate + "' " + session;
                        int noOfStudents = dirAccess.selectScalarInt(noOfStudentsQ);

                        string noOfStudentsSubQ = "select count(distinct roll_no) as cnt,subject_code,etd.coll_code  from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,exmtt et,exmtt_det etd  where s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code  and et.exam_code=etd.exam_code and et.degree_code=ed.degree_code and et.batchTo=ed.batch_year and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and etd.subject_no= ead.subject_no and s.subject_no=etd.subject_no  and ead.ExAttendance=1  and etd.coll_code in (" + collegeCode + ") and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and subject_code in ('" + subject_code + "') and etd.exam_date='" + examDate + "' " + session + "  group by subject_code,etd.coll_code ";
                        DataTable dtStudSubCount = dirAccess.selectDataTable(noOfStudentsSubQ);

                        if (noOfStudents > 0 && dtStudSubCount.Rows.Count > 0)
                        {
                            Dictionary<string, int> dicSubjecCodes = new Dictionary<string, int>();
                            foreach (string subCode in lstSubjecCodes)
                            {
                                dtStudSubCount.DefaultView.RowFilter = "subject_code='" + subCode + "'";
                                DataView dvSubCnt = dtStudSubCount.DefaultView;
                                if (dvSubCnt.Count > 0)
                                {
                                    dicSubjecCodes.Add(subCode, Convert.ToInt32(dvSubCnt[0]["cnt"].ToString()));
                                }
                            }
                            generateDummyNumber(examMonth, examYear, startDummyNo, noOfStudents, subject_code, examDate, dicSubjecCodes, dtStudSubCount);
                        }
                        else
                        {
                          //  ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Students Available')", true);
                            lblAlertMsg.Text = "No Students Available";
                            divPopAlert.Visible = true;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    //For Common Generation
    private void generateDummyNumber(string examMonth, string examYear, int startDummyNo, int noOfStudents, Dictionary<string, int> dicColStudCnt)
    {
        try
        {
            List<string> lstDummys = new List<string>();
            int maxRange = startDummyNo + noOfStudents;
            byte serialOrRandom = 0;

            if (ddlGenMethod.SelectedIndex == 0)
            {
                //Serial
                for (int studCnt = 0; studCnt < noOfStudents; studCnt++)
                {
                    lstDummys.Add((startDummyNo + studCnt).ToString());

                }
                serialOrRandom = 0;
            }
            else
            {
                //Random
                for (int studCnt = 0; studCnt < noOfStudents; studCnt++)
                {
                    lstDummys.Add(getRandomNumber(startDummyNo, maxRange, ref lstDummys));

                }
                serialOrRandom = 1;
            }

            if (lstDummys.Count > 0)
            {
                SaveCommonDummyNumber(lstDummys, examMonth, examYear, serialOrRandom, dicColStudCnt);
            }
        }
        catch { 
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later')", true); 

            lblAlertMsg.Text = "Please Try Later";
            divPopAlert.Visible = true;
        }
    }
    private string getRandomNumber(int startNo, int rangeLimit, ref List<string> lstDummyNos)
    {
        Random rand = new Random();
        string rNumber = "0";
        do
        {
            rNumber = rand.Next(startNo, rangeLimit).ToString();
        } while (lstDummyNos.Contains(rNumber));
        return rNumber;
    }
    private void SaveCommonDummyNumber(List<string> lstDummys, string examMonth, string examYear, byte serialORrandom, Dictionary<string, int> dicColStudCnt)
    {
        try
        {
            string delQ = "delete from dummynumbernew where exam_month='" + examMonth + "' and exam_year='" + examYear + "' and dummy_type='" + serialORrandom + "' and isnull(subject,'')='' and DCollegeCode in (" + collegeCode + ") ";
            dirAccess.deleteData(delQ);

            int loopCnt = 0;
            foreach (KeyValuePair<string, int> colStud in dicColStudCnt)
            {
                int studCnt = colStud.Value;
                string colCode = colStud.Key;
                for (int dummyI = loopCnt, iniiCnt = 0; loopCnt < lstDummys.Count; loopCnt++, iniiCnt++)
                {
                    string insQ = "insert into dummynumbernew (exam_month, exam_year, dummy_no, dummy_type,DCollegeCode) values('" + examMonth + "', '" + examYear + "', '" + lstDummys[loopCnt] + "', '" + serialORrandom + "','" + colCode + "')";
                    dirAccess.insertData(insQ);

                    if (iniiCnt == studCnt)
                    {
                        break;
                    }
                }

            }

            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Generated Successfully')", true);
            lblAlertMsg.Text = "Generated Successfully";
            divPopAlert.Visible = true;
        }
        catch {
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later')", true); 
            lblAlertMsg.Text = "Please Try Later";
            divPopAlert.Visible = true;
        }
    }
    //For Subject wise generation
    private void generateDummyNumber(string examMonth, string examYear, int startDummyNo, int noOfStudents, string subjectCode, string examDate, Dictionary<string, int> dicSubjecCodes, DataTable dtStudSubCount)
    {
        try
        {

            List<string> lstDummys = new List<string>();
            int maxRange = startDummyNo + noOfStudents + 20000;
            byte serialOrRandom = 0;

            if (ddlGenMethod.SelectedIndex == 0)
            {
                //Serial
                serialOrRandom = 0;
                string delQ = "delete from dummynumbernew where exam_month='" + examMonth + "' and exam_year='" + examYear + "' and dummy_type='" + serialOrRandom + "' and subject in ('" + subjectCode + "') and exam_date='" + examDate + "'  and DCollegeCode in (" + collegeCode + ") ";
                dirAccess.deleteData(delQ);

                int savedDummyNo = getStartDummyNumber(examMonth, examYear, serialOrRandom);
                if (savedDummyNo > 0)
                {
                    startDummyNo = savedDummyNo + 1;
                    //maxRange = startDummyNo + noOfStudents;
                }

                for (int studCnt = 0; studCnt < noOfStudents; studCnt++)
                {
                    lstDummys.Add((startDummyNo + studCnt).ToString());
                }
            }
            else
            {
                //Random
                serialOrRandom = 1;
                string delQ = "delete from dummynumbernew where exam_month='" + examMonth + "' and exam_year='" + examYear + "' and dummy_type='" + serialOrRandom + "' and subject in ('" + subjectCode + "') and exam_date='" + examDate + "'  and DCollegeCode in (" + collegeCode + ") ";
                dirAccess.deleteData(delQ);

                //int savedDummyNo = getStartDummyNumber(examMonth, examYear, serialOrRandom);
                //if (savedDummyNo > 0)
                //{
                //startDummyNo = savedDummyNo+1;
                maxRange = startDummyNo + noOfStudents + 20000;
                //}

                List<string> lstgenNumbers = getSavedDummyNumbers(examMonth, examYear, serialOrRandom);

                for (int studCnt = 0; studCnt < noOfStudents; studCnt++)
                {
                    lstDummys.Add(getRandomNumber(startDummyNo, maxRange, ref lstDummys, lstgenNumbers));
                }
            }

            if (lstDummys.Count > 0)
            {
                SaveSubjectWiseDummyNumber(lstDummys, examMonth, examYear, serialOrRandom, subjectCode, examDate, dicSubjecCodes, dtStudSubCount);
            }


        }
        catch {
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later')", true); 
            lblAlertMsg.Text = "Please Try Later";
            divPopAlert.Visible = true;
        }
    }
    private void SaveSubjectWiseDummyNumber(List<string> lstDummys, string examMonth, string examYear, byte serialORrandom, string subjectCode, string examDate, Dictionary<string, int> dicSubjecCodes, DataTable dtStudSubCount)
    {
        try
        {
            int dummyIndex = 0;
            //For Every Subject
            foreach (KeyValuePair<string, int> subStudcnt in dicSubjecCodes)
            {
                //For every student
                for (int subI = 0; subI < subStudcnt.Value; subI++)
                {
                    DataTable dtNew = dtStudSubCount.Copy();
                    dtNew.DefaultView.RowFilter = "subject_code='" + subStudcnt.Key + "'";
                    DataView dvNew = dtNew.DefaultView;
                    string collCode = "13";
                    if (dvNew.Count > 0)
                    {
                        collCode = dvNew[0]["coll_code"].ToString();
                    }
                    string insQ = "insert into dummynumbernew (exam_month, exam_year, dummy_no, dummy_type,subject, exam_date,DCollegeCode) values('" + examMonth + "', '" + examYear + "', '" + lstDummys[dummyIndex] + "', '" + serialORrandom + "','" + subStudcnt.Key + "','" + examDate + "','" + collCode + "')";
                    dirAccess.insertData(insQ);
                    dummyIndex++;
                }
            }

           // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Generated Successfully')", true);
            lblAlertMsg.Text = "Generated Successfully";
            divPopAlert.Visible = true;
        }
        catch
        {
            // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later')", true); }
            lblAlertMsg.Text = "Please Try Later";
            divPopAlert.Visible = true;
        }
    }
    //Dynamic Number Generation for Subject wise
    private string getRandomNumber(int startNo, int rangeLimit, ref List<string> lstDummyNos, List<string> lstgenNumbers)
    {
        Random rand = new Random();
        string rNumber = "0";
        do
        {
            rNumber = rand.Next(startNo, rangeLimit).ToString();
        } while (lstDummyNos.Contains(rNumber) || lstgenNumbers.Contains(rNumber));
        return rNumber;
    }
    private List<string> getSavedDummyNumbers(string examMonth, string examYear, byte serialOrRandom)
    {
        List<string> lstgenNumbers = new List<string>();
        try
        {
            string selQ = "select convert(varchar(30),dummy_no) as dummy_no from dummynumbernew where dummy_type='" + serialOrRandom + "' and exam_month='" + examMonth + "' and exam_year='" + examYear + "'  and isnull(subject,'')<>''  and DCollegeCode in (" + collegeCode + ") ";
            DataTable dtDummyNos = dirAccess.selectDataTable(selQ);

            if (dtDummyNos.Rows.Count > 0)
            {
                lstgenNumbers = dtDummyNos.AsEnumerable()
                           .Select(r => r.Field<string>("dummy_no"))
                           .ToList();
            }
        }
        catch { }
        return lstgenNumbers;
    }
    private int getStartDummyNumber(string examMonth, string examYear, byte serialOrRandom)
    {
        int startNo = 0;
        try
        {
            string selQ = "select isnull(max(dummy_no),0) from dummynumbernew where dummy_type='" + serialOrRandom + "' and exam_month='" + examMonth + "' and exam_year='" + examYear + "'  and isnull(subject,'')<>''  and DCollegeCode in (" + collegeCode + ") ";
            startNo = dirAccess.selectScalarInt(selQ);
        }
        catch { }
        return startNo;
    }
    //View Generated Dummy Numbers
    protected void Viewbtn_Click(object sender, EventArgs e)
    {
        //Added by idhris 16-02-2017

        gridDummy.DataSource = null;
        gridDummy.DataBind();
        gridDummy.Visible = false;
        lblnorec.Visible = false;


        string examMonth = ddlMonth.SelectedValue.Trim();
        string examYear = ddlYear.SelectedValue.Trim();

        if (examMonth != string.Empty && examYear != string.Empty)
        {
            DataTable dtStudents = new DataTable();
            string subjectfilter = string.Empty;
            if (ddlGenType.SelectedIndex == 0)
            {
                //Common Number Generation
                subjectfilter = " and isnull(subject,'')='' ";

                string studQ = "select distinct(r.reg_no),r.batch_year,r.degree_code,r.roll_no,r.Current_Semester,r.college_code from exam_application ea, Exam_Details ed,Registration r where r.Roll_No=ea.roll_no  and ea.exam_code = ed.exam_code  and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and r.college_code in (" + collegeCode + ")  order by  r.degree_code,r.batch_year desc,r.reg_no asc";
                dtStudents = dirAccess.selectDataTable(studQ);
            }
            else
            {
                //Subject Wise Number generation

                string[] examDateArr = ddlExDate.SelectedItem.Text.Split('-');
                string examDate = examDateArr[1] + "/" + examDateArr[0] + "/" + examDateArr[2];
                string subject_code;

                if (ddlsubject.SelectedIndex == 0)
                {
                    StringBuilder sbSubjectCodes = new StringBuilder();
                    for (int itemI = 1; itemI < ddlsubject.Items.Count; itemI++)
                    {
                        sbSubjectCodes.Append(ddlsubject.Items[itemI].Value + "','");
                    }
                    if (sbSubjectCodes.Length > 3)
                        sbSubjectCodes.Remove(sbSubjectCodes.Length - 3, 3);
                    subject_code = sbSubjectCodes.ToString();
                }
                else
                {
                    subject_code = ddlsubject.SelectedValue.ToString();
                }

                string session = string.Empty;
                if (ddlsession.SelectedIndex != 0)
                {
                    session = " and etd.exam_session ='" + ddlsession.SelectedItem.Text + " ' ";
                }

                subjectfilter = " and subject in ('" + subject_code + "') ";



                string studQ = "select distinct (r.reg_no),r.batch_year,r.degree_code,r.roll_no,r.Current_Semester, r.college_code from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,exmtt et,exmtt_det etd  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code  and et.exam_code=etd.exam_code and et.degree_code=ed.degree_code and et.batchTo=ed.batch_year and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and etd.subject_no= ead.subject_no and s.subject_no=etd.subject_no and ead.ExAttendance=1 and r.college_code in (" + collegeCode + ") and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and subject_code in ('" + subject_code + "')  and etd.exam_date='" + examDate + "' " + session + " order by r.batch_year,r.degree_code,r.roll_no,r.Current_Semester, r.college_code,reg_no asc ";
                dtStudents = dirAccess.selectDataTable(studQ);
            }
            string order = ddlGenMethod.SelectedIndex == 0 ? " order by dummy_no asc " : string.Empty;
            string selectQ = "select dummy_no as [DummyNumber]  from dummynumbernew where exam_month='" + examMonth + "' and exam_year='" + examYear + "'  and dummy_type='" + ddlGenMethod.SelectedIndex + "'  and DCollegeCode in (" + collegeCode + ") " + subjectfilter + order;
            DataTable dtDummyNos = dirAccess.selectDataTable(selectQ);

            string selectStudQ = "select *,dummy_no as [DummyNumber]  from dummynumber where exam_month='" + examMonth + "' and exam_year='" + examYear + "'  and dummy_type='" + ddlGenMethod.SelectedIndex + "'  and DNCollegeCode in (" + collegeCode + ") " + subjectfilter + order;
            DataTable dtStudentDummyNos = dirAccess.selectDataTable(selectStudQ);
            dtDummyNos.Columns.Add("BatchYear");
            dtDummyNos.Columns.Add("DegreeCode");
            dtDummyNos.Columns.Add("CurSem");
            dtDummyNos.Columns.Add("RollNumber");
            dtDummyNos.Columns.Add("RegNumber");
            dtDummyNos.Columns.Add("CollegeCode");
            if (dtDummyNos.Rows.Count > 0 && dtStudents.Rows.Count <= dtDummyNos.Rows.Count)//&& 
            {
                //dtDummyNos.Columns.Add("BatchYear");
                //dtDummyNos.Columns.Add("DegreeCode");
                //dtDummyNos.Columns.Add("CurSem");
                //dtDummyNos.Columns.Add("RollNumber");
                //dtDummyNos.Columns.Add("RegNumber");
                //dtDummyNos.Columns.Add("CollegeCode");
                if (dtStudents.Rows.Count == dtDummyNos.Rows.Count)
                {
                    for (int rowI = 0; rowI < dtDummyNos.Rows.Count; rowI++)
                    {
                        dtDummyNos.Rows[rowI]["BatchYear"] = Convert.ToString(dtStudents.Rows[rowI]["batch_year"]);
                        dtDummyNos.Rows[rowI]["DegreeCode"] = Convert.ToString(dtStudents.Rows[rowI]["degree_code"]);
                        dtDummyNos.Rows[rowI]["CurSem"] = Convert.ToString(dtStudents.Rows[rowI]["Current_Semester"]);
                        dtDummyNos.Rows[rowI]["RollNumber"] = Convert.ToString(dtStudents.Rows[rowI]["roll_no"]);
                        dtDummyNos.Rows[rowI]["RegNumber"] = Convert.ToString(dtStudents.Rows[rowI]["reg_no"]);
                        dtDummyNos.Rows[rowI]["CollegeCode"] = Convert.ToString(dtStudents.Rows[rowI]["college_code"]);
                    }
                    
                }
                viewDummyNumber(dtDummyNos);
        
            }
            if (dtDummyNos.Rows.Count > 0 && dtStudentDummyNos.Rows.Count == dtDummyNos.Rows.Count)
            {
                //dtDummyNos.Columns.Add("BatchYear");
                //dtDummyNos.Columns.Add("DegreeCode");
                //dtDummyNos.Columns.Add("CurSem");
                //dtDummyNos.Columns.Add("RollNumber");
                //dtDummyNos.Columns.Add("RegNumber");
                //dtDummyNos.Columns.Add("CollegeCode");

                if (dtStudentDummyNos.Rows.Count == dtDummyNos.Rows.Count)
                {
                    for (int rowI = 0; rowI < dtDummyNos.Rows.Count; rowI++)
                    {
                        dtDummyNos.Rows[rowI]["BatchYear"] = Convert.ToString(dtStudentDummyNos.Rows[rowI]["batch"]);
                        dtDummyNos.Rows[rowI]["DegreeCode"] = Convert.ToString(dtStudentDummyNos.Rows[rowI]["degreecode"]);
                        dtDummyNos.Rows[rowI]["CurSem"] = Convert.ToString(dtStudentDummyNos.Rows[rowI]["semester"]);
                        dtDummyNos.Rows[rowI]["RollNumber"] = Convert.ToString(dtStudentDummyNos.Rows[rowI]["roll_no"]);
                        dtDummyNos.Rows[rowI]["RegNumber"] = Convert.ToString(dtStudentDummyNos.Rows[rowI]["regno"]);
                        dtDummyNos.Rows[rowI]["CollegeCode"] = Convert.ToString(dtStudentDummyNos.Rows[rowI]["DNCollegeCode"]);
                    }
                }
                viewDummyNumber(dtDummyNos);
            }
            else
            {
                if (dtStudents.Rows.Count == dtDummyNos.Rows.Count)//Rajkumar
                    lblnorec.Visible = false;
                else
                { // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Students Count does not match Dummy Numbers')", true);
                    lblAlertMsg.Text = "Students Count does not match Dummy Numbers";
                    divPopAlert.Visible = true;
                }
            }
        }

    }
    private void viewDummyNumber(DataTable dtDummy)
    {
        gridDummy.DataSource = dtDummy;
        gridDummy.DataBind();
        gridDummy.Visible = true;
    }
    private void clearGrid()
    {
        if (gridDummy.Visible)
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string btnid = Convert.ToString(ctrlid.ClientID);

            if (btnid != "MainContent_Viewbtn" && btnid != "MainContent_btnDummyMap")
            {
                gridDummy.DataSource = null;
                gridDummy.DataBind();
                gridDummy.Visible = false;
                lblnorec.Visible = false;
            }
        }
    }
    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    //Dummy Numbers Mapping
    protected void btnDummyMap_Click(object sender, EventArgs e)
    {
        string examMonth = ddlMonth.SelectedValue.Trim();
        string examYear = ddlYear.SelectedValue.Trim();
        byte dummyType = (byte)ddlGenMethod.SelectedIndex;// 0 - Serial , 1 - Random

        if (ddlGenType.SelectedIndex == 0)
        {
            //Common
            saveCommonMap(examMonth, examYear, dummyType);
        }
        else
        {
            //SubjectWise
            string subjectCode = ddlsubject.SelectedValue.ToString();
            saveSubjectwiseMap(examMonth, examYear, dummyType, subjectCode);
        }

    }
    private void saveCommonMap(string examMonth, string examYear, byte dummyType)
    {
        try
        {
            string delQ = "delete from dummynumber where dummy_type='" + dummyType + "' and exam_month='" + examMonth + "' and exam_year='" + examYear + "'  and isnull(subject,'')='' and ISNULL(subject_no,'')='' and DNCollegeCode in (" + collegeCode + ") ";
            dirAccess.deleteData(delQ);

            foreach (GridViewRow gRow in gridDummy.Rows)
            {
                Label lblBatchYear = (Label)gRow.FindControl("lblBatchYear");
                Label lblDegCode = (Label)gRow.FindControl("lblDegCode");
                Label lblCurSem = (Label)gRow.FindControl("lblCurSem");
                Label lblDummyNo = (Label)gRow.FindControl("lblDummyNo");
                Label lblRegNo = (Label)gRow.FindControl("lblRegNo");
                Label lblRollNo = (Label)gRow.FindControl("lblRollNo");
                Label lblClgCode = (Label)gRow.FindControl("lblClgCode");

                string insQ = "insert into dummynumber (batch, degreecode, semester, roll_no, regno, dummy_no, dummy_type, exam_month, exam_year,DNCollegeCode) values ('" + lblBatchYear.Text + "', '" + lblDegCode.Text + "', '" + lblCurSem.Text + "', '" + lblRollNo.Text + "', '" + lblRegNo.Text + "', '" + lblDummyNo.Text + "', '" + dummyType + "', '" + examMonth + "', '" + examYear + "','" + lblClgCode.Text + "')";
                dirAccess.insertData(insQ);
            }

            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            lblAlertMsg.Text = "Saved Successfully";
            divPopAlert.Visible = true;
        }
        catch {
        
           //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later')", true);
            lblAlertMsg.Text = "Please Try Later";
            divPopAlert.Visible = true;
        }
    }
    private void saveSubjectwiseMap(string examMonth, string examYear, byte dummyType, string subjectCode)
    {
        try
        {
            string[] examDateArr = ddlExDate.SelectedItem.Text.Split('-');
            string examDate = examDateArr[1] + "/" + examDateArr[0] + "/" + examDateArr[2];
            string subject_code;

            if (ddlsubject.SelectedIndex == 0)
            {
                StringBuilder sbSubjectCodes = new StringBuilder();
                for (int itemI = 1; itemI < ddlsubject.Items.Count; itemI++)
                {
                    sbSubjectCodes.Append(ddlsubject.Items[itemI].Value + "','");
                }
                if (sbSubjectCodes.Length > 3)
                    sbSubjectCodes.Remove(sbSubjectCodes.Length - 3, 3);
                subject_code = sbSubjectCodes.ToString();
            }
            else
            {
                subject_code = ddlsubject.SelectedValue.ToString();
            }

            string session = string.Empty;
            if (ddlsession.SelectedIndex != 0)
            {
                session = " and etd.exam_session ='" + ddlsession.SelectedItem.Text + " ' ";
            }

            string delQ = "delete from dummynumber where dummy_type='" + dummyType + "' and exam_month='" + examMonth + "' and exam_year='" + examYear + "'  and subject in ('" + subjectCode + "') and  exam_date='" + examDate + "'  and DNCollegeCode in (" + collegeCode + ")  -- and ISNULL(subject_no,'')=''";
            dirAccess.deleteData(delQ);

            //DataTable dtSubjectDet = dirAccess.selectDataTable("select r.reg_no,s.subject_no,s.subject_code from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code and ead.ExAttendance=1 and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and subject_code='" + subjectCode + "'");

            string studQ = "select r.reg_no,s.subject_no,s.subject_code from exam_application ea, Exam_Details ed,exam_appl_details ead,subject s,Registration r,exmtt et,exmtt_det etd  where ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ead.appl_no=ea.appl_no and  ea.exam_code = ed.exam_code  and et.exam_code=etd.exam_code and et.degree_code=ed.degree_code and et.batchTo=ed.batch_year and et.Exam_month=ed.Exam_Month and et.Exam_year=ed.Exam_year and etd.subject_no= ead.subject_no and s.subject_no=etd.subject_no  and  ead.ExAttendance=1 and r.college_code in (" + collegeCode + ") and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "' and subject_code in ('" + subjectCode + "')  and etd.exam_date='" + examDate + "' " + session + " -- order by degree_code,batch_year desc,reg_no asc ";

            DataTable dtSubjectDet = dirAccess.selectDataTable(studQ);

            foreach (GridViewRow gRow in gridDummy.Rows)
            {
                Label lblBatchYear = (Label)gRow.FindControl("lblBatchYear");
                Label lblDegCode = (Label)gRow.FindControl("lblDegCode");
                Label lblCurSem = (Label)gRow.FindControl("lblCurSem");
                Label lblDummyNo = (Label)gRow.FindControl("lblDummyNo");
                Label lblRegNo = (Label)gRow.FindControl("lblRegNo");
                Label lblRollNo = (Label)gRow.FindControl("lblRollNo");
                Label lblClgCode = (Label)gRow.FindControl("lblClgCode");

                string subject_no = string.Empty;
                dtSubjectDet.DefaultView.RowFilter = "reg_no='" + lblRegNo.Text + "' and subject_code='" + subjectCode + "'";
                DataView dvSubView = dtSubjectDet.DefaultView;
                if (dvSubView.Count > 0)
                {
                    subject_no = Convert.ToString(dvSubView[0]["subject_no"]);
                }

                string insQ = "insert into dummynumber (batch, degreecode, semester, roll_no, regno, dummy_no, dummy_type, exam_month, exam_year, subject, subject_no,exam_date, DNCollegeCode ) values ('" + lblBatchYear.Text + "', '" + lblDegCode.Text + "', '" + lblCurSem.Text + "', '" + lblRollNo.Text + "', '" + lblRegNo.Text + "', '" + lblDummyNo.Text + "', '" + dummyType + "', '" + examMonth + "', '" + examYear + "','" + subjectCode + "','" + subject_no + "','" + examDate + "','" + lblClgCode.Text + "')";
                dirAccess.insertData(insQ);
            }

           // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            lblAlertMsg.Text = "Saved Successfully";
            divPopAlert.Visible = true;
        }
        catch {
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later')", true); 
            lblAlertMsg.Text = "Saved Successfully";
            divPopAlert.Visible = true;
        }
    }
    //Code Ended by Idhris -- Last Modified by idhris 23-02-2017
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

}