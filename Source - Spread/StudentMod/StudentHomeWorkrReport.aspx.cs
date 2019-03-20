using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using InsproDataAccess;
using System.Collections.Generic;

public partial class StudentMod_StudentHomeWorkrReport : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DataSet ds = new DataSet();
    InsproDirectAccess dir = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string grouporusercode = string.Empty;
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    DataTable gviewhmewrkdt = new DataTable();
    DataRow gviewhmewrkdr = null;
    Institution institute;

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
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                NEWPrintMater1.Visible = false;

                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                setLabelText();
                bindcollege();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                bindsubject();
            }
        }
        catch (Exception ex) { }
    }

    private void setLabelText()
    {
        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            institute = new Institution(grouporusercode);
            List<Label> lbl = new List<Label>();
            List<byte> fields = new List<byte>();
            lbl.Add(lblcollege);
            lbl.Add(lbldegree);
            lbl.Add(lblbranch);
            lbl.Add(lblduration);
            fields.Add(0);
            fields.Add(2);
            fields.Add(3);
            fields.Add(4);
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblbatch.Text = "Year";
            }
            else
            {
                lblbatch.Text = "Batch";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
        }
    }

    protected void bindcollege()
    {
        ddlcollege.Items.Clear();
        string grporusercode = string.Empty;
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grporusercode = " and user_code=" + Session["usercode"].ToString().Trim() + "";
        }


        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where cp.college_code=cf.college_code " + grporusercode + "";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
    }

    public void bindbatch()
    {
        ddlbatch.Items.Clear();
        ds = d2.select_method_wo_parameter("bind_batch", "sp");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataTextField = "batch_year";
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();
        }
        int count1 = ds.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            ddlbatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
    }

    public void binddegree()
    {
        ddldegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        has.Clear();
        has.Add("single_user", singleuser);
        has.Add("group_code", group_user);
        has.Add("college_code", collegecode);
        has.Add("user_code", usercode);
        ds = d2.select_method("bind_degree", has, "sp");
        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
    }

    public void bindbranch()
    {
        ddlbranch.Items.Clear();
        has.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        has.Add("single_user", singleuser);
        has.Add("group_code", group_user);
        has.Add("course_id", ddldegree.SelectedValue);
        has.Add("college_code", collegecode);
        has.Add("user_code", usercode);
        ds = d2.select_method("bind_branch", has, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }
    }

    public void bindsem()
    {
        ddlduration.Items.Clear();
        string duration = string.Empty;
        Boolean first_year = false;
        has.Clear();
        collegecode = Session["collegecode"].ToString();
        has.Add("degree_code", ddlbranch.SelectedValue.ToString());
        has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        has.Add("college_code", collegecode);
        ds = d2.select_method("bind_sem", has, "sp");
        int count3 = ds.Tables[0].Rows.Count;
        if (count3 > 0)
        {
            ddlduration.Enabled = true;
            duration = ds.Tables[0].Rows[0][0].ToString();
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
            {
                if (first_year == false)
                {
                    ddlduration.Items.Add(loop_val.ToString());
                }
                else if (first_year == true && loop_val != 2)
                {
                    ddlduration.Items.Add(loop_val.ToString());
                }
            }
        }
        else
        {
            count3 = ds.Tables[1].Rows.Count;
            if (count3 > 0)
            {
                ddlduration.Enabled = true;
                duration = ds.Tables[1].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlduration.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlduration.Items.Add(loop_val.ToString());
                    }
                }
            }
            else
            {
                ddlduration.Enabled = false;
            }
        }
    }

    public void bindsec()
    {
        ddlsec.Items.Clear();
        has.Clear();
        has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        has.Add("degree_code", ddlbranch.SelectedValue);
        ds = d2.select_method("bind_sec", has, "sp");
        int count5 = ds.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlsec.DataSource = ds;
            ddlsec.DataTextField = "sections";
            ddlsec.DataValueField = "sections";
            ddlsec.DataBind();
            ddlsec.Enabled = true;
        }
        else
        {
            ddlsec.Enabled = false;
        }
    }

    public void bindsubject()
    {
        cblsubject.Items.Clear();
        chksubject.Checked = false;
        //string collegecode = Session["collegecode"].ToString();
        string collegecode = ddlcollege.SelectedValue;
        string valDegree = ddlbranch.SelectedValue;
        string valBatch = ddlbatch.SelectedValue;
        string sem = ddlduration.SelectedValue;

        string qrySub = "select distinct subject_name,subject_no,subject_code,CONVERT(nvarchar(max),isnull(subject.subject_code,'')+'-'+isnull(subject.subject_name,'')) as text from subject,sub_sem,syllabus_master where  subject.subtype_no = sub_sem.subtype_no  and subject.syll_code=syllabus_master.syll_code   and  syllabus_master.degree_code in('" + valDegree + "') and syllabus_master.batch_year in('" + valBatch + "') and syllabus_master.semester='" + sem + "' order by subject.subject_name";//and sub_sem.subject_type in(" + subtype + ")
        DataSet ds = new DataSet();
        ds = d2.select_method(qrySub, hat, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cblsubject.DataSource = ds;
            cblsubject.DataTextField = "subject_name";
            cblsubject.DataValueField = "subject_no";
            cblsubject.DataBind();
            if (cblsubject.Items.Count > 0)
            {
                for (int i = 0; i < cblsubject.Items.Count; i++)
                {
                    cblsubject.Items[i].Selected = true;
                }
                txtsubject.Text = "Subject(" + cblsubject.Items.Count + ")";
                chksubject.Checked = true;
            }
        }
        else
        {
            txtsubject.Text = "--Select--";
            chksubject.Checked = false;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        errlbl.Visible = false;
        gview.Visible = false;
        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
        bindsubject();
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        gview.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        NEWPrintMater1.Visible = false;
        errlbl.Visible = false;
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
        bindsubject();
        //binddate();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtexcelname.Text = string.Empty;
        gview.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        NEWPrintMater1.Visible = false;
        errlbl.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        bindsubject();
        //load_subject();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtexcelname.Text = string.Empty;
        gview.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        NEWPrintMater1.Visible = false;
        errlbl.Visible = false;
        bindsem();
        bindsec();
        bindsubject();
        //load_subject();
    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtexcelname.Text = string.Empty;
        gview.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        NEWPrintMater1.Visible = false;
        errlbl.Visible = false;
        bindsec();
        bindsubject();
        //load_subject();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtexcelname.Text = string.Empty;
        gview.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        NEWPrintMater1.Visible = false;
        errlbl.Visible = false;
        bindsubject();
        //load_subject();
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //txtexcelname.Text = string.Empty;
            errmsg.Visible = false;
            gview.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            NEWPrintMater1.Visible = false;

            errlbl.Visible = false;
            if (txtFromDate.Text != "")
            {
                string[] spitfrom = txtFromDate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);
                string[] spilttodate = txtToDate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
                if (dtto < dtfrom)
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    errlbl.Visible = true;
                    errlbl.Text = "To Date Must Be Greater Than From Date";
                }
            }
            else
            {
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception ex)
        {
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            errlbl.Visible = true;
            errlbl.Text = "Please Enter Valid From Date";
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //txtexcelname.Text = string.Empty;
            errmsg.Visible = false;
            gview.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            NEWPrintMater1.Visible = false;

            //frmlbl.Visible = false;
            //tolbl.Visible = false;
            //tofromlbl.Visible = false;
            errlbl.Visible = false;
            if (txtToDate.Text != "")
            {
                string[] spitfrom = txtFromDate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);
                string[] spilttodate = txtToDate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
                if (dtto < dtfrom)
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    errlbl.Visible = true;
                    errlbl.Text = "To Date Must Be Greater Than From Date";
                }
            }
            else
            {
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception ex)
        {
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            errlbl.Visible = true;
            errlbl.Text = "Please Enter Valid From Date";
        }
    }

    protected void chksubject_CheckedChanged(object sender, EventArgs e)
    {
        if (chksubject.Checked)
        {
            //CallCheckboxListChange(chksubject, cblsubject, txtsubject, lblsubject.Text, "--Select--");
            for (int i = 0; i < cblsubject.Items.Count; i++)
            {
                cblsubject.Items[i].Selected = true;
            }
            txtsubject.Text = "Subject(" + cblsubject.Items.Count + ")";
            chksubject.Checked = true;
        }
        else
        {
            for (int i = 0; i < cblsubject.Items.Count; i++)
            {
                cblsubject.Items[i].Selected = false;
            }
            txtsubject.Text = "--Select--";
            chksubject.Checked = false;
        }
    }

    protected void chklstsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        for (int i = 0; i < cblsubject.Items.Count; i++)
        {
            if (cblsubject.Items[i].Selected)
            {
                cblsubject.Items[i].Selected = true;
                count++;
            }
        }
        if (count != 0)
        {
            txtsubject.Text = "Subject(" + count + ")";
            bindsec();
        }
        else
        {
            txtsubject.Text = "--Select--";
        }
        if (count == cblsubject.Items.Count)
        {
            chksubject.Checked = true;
        }
        else
        {
            chksubject.Checked = false;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            errlbl.Visible = false;
            errlbl.Text = "";
            gview.Visible = false;
            int cnt = 0;
            for (int i = 0; i < cblsubject.Items.Count; i++)
            {
                if (cblsubject.Items[i].Selected)
                    cnt++;
            }
            if (cnt == 0)
            {
                errlbl.Text = "Choose atleast one subject";
                errlbl.Visible = true;
                return;
            }
            loadgrid();
        }
        catch
        {
        }


    }

    protected void loadgrid()
    {
        try
        {
            ArrayList row = new ArrayList();
            gviewhmewrkdt.Rows.Clear();
            gviewhmewrkdt.Columns.Clear();

            gviewhmewrkdt.Columns.Add("S.No");
            gviewhmewrkdt.Columns.Add("unid");
            gviewhmewrkdt.Columns.Add("Subject");
            gviewhmewrkdt.Columns.Add("subjectno");
            gviewhmewrkdt.Columns.Add("Heading");
            gviewhmewrkdt.Columns.Add("Description");
            gviewhmewrkdt.Columns.Add("Photo");
            gviewhmewrkdt.Columns.Add("Attachment");
            gviewhmewrkdt.Columns.Add("Delivered");
            gviewhmewrkdt.Columns.Add("Not Delivered");
            gviewhmewrkdt.Columns.Add("Open");
            gviewhmewrkdt.Columns.Add("Not Open");
            gviewhmewrkdt.Columns.Add("Date");

            gviewhmewrkdr = gviewhmewrkdt.NewRow();
            gviewhmewrkdr["S.No"] = "S.No";
            gviewhmewrkdr["unid"] = "unid";
            gviewhmewrkdr["Subject"] = "Subject";
            gviewhmewrkdr["Subjectno"] = "Subjectno";
            gviewhmewrkdr["Heading"] = "Heading";
            gviewhmewrkdr["Description"] = "Description";
            gviewhmewrkdr["Photo"] = "Photo";
            gviewhmewrkdr["Attachment"] = "Attachment";
            gviewhmewrkdr["Delivered"] = "Delivered";
            gviewhmewrkdr["Not Delivered"] = "Not Delivered";
            gviewhmewrkdr["Open"] = "Open";
            gviewhmewrkdr["Not Open"] = "Not Open";
            gviewhmewrkdr["Date"] = "Date";
            gviewhmewrkdt.Rows.Add(gviewhmewrkdr);

            string stufrm = txtFromDate.Text;
            string stuto = txtToDate.Text;
            string[] frmspl = stufrm.Split(new char[] { '/' });
            string[] tospl = stuto.Split(new char[] { '/' });

            string frmdate = frmspl[2].ToString() + "-" + frmspl[1].ToString() + "-" + frmspl[0].ToString();
            string todate = tospl[2].ToString() + "-" + tospl[1].ToString() + "-" + tospl[0].ToString();
            string batchVal = string.Empty;
            string degreVal = string.Empty;
            string branchVal = string.Empty;
            string semVal = string.Empty;
            string secVal = string.Empty;
            string subVal = string.Empty;

            if (!string.IsNullOrEmpty(ddlbatch.SelectedValue))
            {
                batchVal = ddlbatch.SelectedValue;
            }
            if (!string.IsNullOrEmpty(ddldegree.SelectedValue))
            {
                degreVal = ddldegree.SelectedValue;
            }
            if (!string.IsNullOrEmpty(ddlbranch.SelectedValue))
            {
                branchVal = ddlbranch.SelectedValue;
            }
            if (!string.IsNullOrEmpty(ddlduration.SelectedValue))
            {
                semVal = ddlduration.SelectedValue;
            }
            if (!string.IsNullOrEmpty(ddlsec.SelectedValue))
            {
                secVal = " and hw.Section='" + ddlsec.SelectedValue + "'";
            }
            if (cblsubject.Items.Count > 0)
            {
                for (int i = 0; i < cblsubject.Items.Count; i++)
                {
                    if (cblsubject.Items[i].Selected)
                    {
                        if (string.IsNullOrEmpty(subVal))
                        {
                            subVal = cblsubject.Items[i].Value;
                        }
                        else
                        {
                            subVal = subVal + "','" + cblsubject.Items[i].Value;
                        }
                    }
                }
            }

            string qurys = "select distinct hw.idno,hw.subjectno,hw.Date,hw.Homework,hw.Section,hw.PhotoAttachment,hw.FileAttachment,hw.Heading from Registration r,subject s,syllabus_master sy,subjectChooser sc,Home_Work hw where hw.subjectno=sc.subject_no and r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.semester=r.Current_Semester and sc.subject_no=s.subject_no and sy.syll_code=s.syll_code and r.Roll_No=sc.roll_no and r.Current_Semester=sc.semester and hw.subjectno in('" + subVal + "') " + secVal + " and Date between '" + frmdate + "' and '" + todate + "' order by date desc,idno asc,subjectno desc";

            DataSet dsset = d2.select_method_wo_parameter(qurys, "Text");
            if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                string subno = string.Empty;
                string dte = string.Empty;
                string date1 = string.Empty;
                string idno = string.Empty;
                string date = string.Empty;
                string dsqry = "select hs.*,sh.idno,sh.subjectno from Home_Work sh,stud_homework_status hs where sh.idno=hs.homework_id  and hs.date between '" + frmdate + "' and '" + todate + "'";
                DataSet dsstatus = d2.select_method_wo_parameter(dsqry, "Text");

                for (int i = 0; i < dsset.Tables[0].Rows.Count; i++)
                {
                    if (dte != Convert.ToString(dsset.Tables[0].Rows[i]["Date"]))
                    {
                        dte = Convert.ToString(dsset.Tables[0].Rows[i]["Date"]);
                        string[] datesplit = dte.Split(' ');
                        date = datesplit[0];
                        string[] dat = date.Split('/');
                        date1 = dat[1] + "/" + dat[0] + "/" + dat[2];

                        gviewhmewrkdr = gviewhmewrkdt.NewRow();
                        gviewhmewrkdr["S.No"] = date1;
                        gviewhmewrkdt.Rows.Add(gviewhmewrkdr);
                        if (subno != Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]))
                        {
                            subno = Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]);
                            string subjectname = d2.GetFunction("Select Subject_Name from subject where subject_no='" + subno + "'");

                            idno = Convert.ToString(dsset.Tables[0].Rows[i]["idno"]);

                            DataView dvdeliver = new DataView();
                            dsstatus.Tables[0].DefaultView.RowFilter = " idno='" + idno + "' and subjectno='" + subno + "' and Date='" + date + "' and delivered<>0";
                            dvdeliver = dsstatus.Tables[0].DefaultView;

                            int delivercnt = dvdeliver.Count;

                            string stucount = d2.GetFunction("select COUNT(distinct r.roll_no) as countStu from Registration r,subjectChooser sc where sc.roll_no=r.Roll_No and sc.subject_no='" + subno + "'");

                            int cntundeli = Convert.ToInt32(stucount) - Convert.ToInt32(delivercnt);

                            DataView dvOpen = new DataView();
                            dsstatus.Tables[0].DefaultView.RowFilter = " idno='" + idno + "' and subjectno='" + subno + "' and Date='" + date + "' and is_open<>0";
                            dvOpen = dsstatus.Tables[0].DefaultView;

                            int opencnt = dvOpen.Count;

                            int unopen = Convert.ToInt32(stucount) - Convert.ToInt32(opencnt);

                            sno++;
                            gviewhmewrkdr = gviewhmewrkdt.NewRow();
                            string uniq = Convert.ToString(dsset.Tables[0].Rows[i]["idno"]);
                            string subjectno = Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]);
                            string headng = Convert.ToString(dsset.Tables[0].Rows[i]["Heading"]);
                            string topic = Convert.ToString(dsset.Tables[0].Rows[i]["Homework"]);
                            string photo = Convert.ToString(dsset.Tables[0].Rows[i]["PhotoAttachment"]);
                            string file = Convert.ToString(dsset.Tables[0].Rows[i]["FileAttachment"]);

                            gviewhmewrkdr["S.No"] = sno.ToString();
                            gviewhmewrkdr["unid"] = uniq;
                            gviewhmewrkdr["Subject"] = subjectname;
                            gviewhmewrkdr["Subjectno"] = subjectno;
                            gviewhmewrkdr["Heading"] = headng;
                            gviewhmewrkdr["Description"] = topic;
                            gviewhmewrkdr["Photo"] = photo;
                            gviewhmewrkdr["Attachment"] = file;
                            gviewhmewrkdr["Delivered"] = Convert.ToString(delivercnt);
                            gviewhmewrkdr["Not Delivered"] = cntundeli;
                            gviewhmewrkdr["Open"] = Convert.ToString(opencnt);
                            gviewhmewrkdr["Not Open"] = unopen;
                            gviewhmewrkdr["Date"] = date;
                            gviewhmewrkdt.Rows.Add(gviewhmewrkdr);
                        }
                        else
                        {
                            subno = Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]);
                            string subjectname = d2.GetFunction("Select Subject_Name from subject where subject_no='" + subno + "'");

                            idno = Convert.ToString(dsset.Tables[0].Rows[i]["idno"]);

                            DataView dvdeliver = new DataView();
                            dsstatus.Tables[0].DefaultView.RowFilter = " idno='" + idno + "' and subjectno='" + subno + "' and Date='" + date + "' and delivered<>0";
                            dvdeliver = dsstatus.Tables[0].DefaultView;

                            string stucount = d2.GetFunction("select COUNT(distinct r.roll_no) as countStu from Registration r,subjectChooser sc where sc.roll_no=r.Roll_No and sc.subject_no='" + subno + "'");
                            int delivercnt = dvdeliver.Count;

                            int cntundeli = Convert.ToInt32(stucount) - Convert.ToInt32(delivercnt);

                            DataView dvOpen = new DataView();
                            dsstatus.Tables[0].DefaultView.RowFilter = " idno='" + idno + "' and subjectno='" + subno + "' and Date='" + date + "' and is_open<>0";
                            dvOpen = dsstatus.Tables[0].DefaultView;

                            int opencnt = dvOpen.Count;

                            int unopen = Convert.ToInt32(stucount) - Convert.ToInt32(opencnt);

                            sno++;
                            gviewhmewrkdr = gviewhmewrkdt.NewRow();

                            string uniq = Convert.ToString(dsset.Tables[0].Rows[i]["idno"]);
                            string subjectno = Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]);
                            string headng = Convert.ToString(dsset.Tables[0].Rows[i]["Heading"]);
                            string topic = Convert.ToString(dsset.Tables[0].Rows[i]["Homework"]);
                            string photo = Convert.ToString(dsset.Tables[0].Rows[i]["PhotoAttachment"]);
                            string file = Convert.ToString(dsset.Tables[0].Rows[i]["FileAttachment"]);

                            gviewhmewrkdr["S.No"] = sno.ToString();
                            gviewhmewrkdr["unid"] = uniq;
                            gviewhmewrkdr["Subject"] = subjectname;
                            gviewhmewrkdr["Subjectno"] = subjectno;
                            gviewhmewrkdr["Heading"] = headng;
                            gviewhmewrkdr["Description"] = topic;
                            gviewhmewrkdr["Photo"] = photo;
                            gviewhmewrkdr["Attachment"] = file;
                            gviewhmewrkdr["Delivered"] = Convert.ToString(delivercnt);
                            gviewhmewrkdr["Not Delivered"] = cntundeli;
                            gviewhmewrkdr["Open"] = Convert.ToString(opencnt);
                            gviewhmewrkdr["Not Open"] = unopen;
                            gviewhmewrkdr["Date"] = date;

                            gviewhmewrkdt.Rows.Add(gviewhmewrkdr);
                        }
                    }
                    else
                    {
                        if (subno != Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]))
                        {
                            subno = Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]);
                            string subjectname = d2.GetFunction("Select Subject_Name from subject where subject_no='" + subno + "'");

                            idno = Convert.ToString(dsset.Tables[0].Rows[i]["idno"]);

                            DataView dvdeliver = new DataView();
                            dsstatus.Tables[0].DefaultView.RowFilter = " idno='" + idno + "' and subjectno='" + subno + "' and Date='" + date + "' and delivered<>0";
                            dvdeliver = dsstatus.Tables[0].DefaultView;

                            string stucount = d2.GetFunction("select COUNT(distinct r.roll_no) as countStu from Registration r,subjectChooser sc where sc.roll_no=r.Roll_No and sc.subject_no='" + subno + "'");

                            int delivercnt = dvdeliver.Count;

                            int cntundeli = Convert.ToInt32(stucount) - Convert.ToInt32(delivercnt);

                            DataView dvOpen = new DataView();
                            dsstatus.Tables[0].DefaultView.RowFilter = " idno='" + idno + "' and subjectno='" + subno + "' and Date='" + date + "' and is_open<>0";
                            dvOpen = dsstatus.Tables[0].DefaultView;
                            int opencnt = dvOpen.Count;

                            int unopen = Convert.ToInt32(stucount) - Convert.ToInt32(opencnt);

                            sno++;
                            gviewhmewrkdr = gviewhmewrkdt.NewRow();
                            string uniq = Convert.ToString(dsset.Tables[0].Rows[i]["idno"]);
                            string subjectno = Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]);
                            string headng = Convert.ToString(dsset.Tables[0].Rows[i]["Heading"]);
                            string topic = Convert.ToString(dsset.Tables[0].Rows[i]["Homework"]);
                            string photo = Convert.ToString(dsset.Tables[0].Rows[i]["PhotoAttachment"]);
                            string file = Convert.ToString(dsset.Tables[0].Rows[i]["FileAttachment"]);

                            gviewhmewrkdr["S.No"] = sno.ToString();
                            gviewhmewrkdr["unid"] = uniq;
                            gviewhmewrkdr["Subject"] = subjectname;
                            gviewhmewrkdr["Subjectno"] = subjectno;
                            gviewhmewrkdr["Heading"] = headng;
                            gviewhmewrkdr["Description"] = topic;
                            gviewhmewrkdr["Photo"] = photo;
                            gviewhmewrkdr["Attachment"] = file;
                            gviewhmewrkdr["Delivered"] = Convert.ToString(delivercnt);
                            gviewhmewrkdr["Not Delivered"] = cntundeli;
                            gviewhmewrkdr["Open"] = Convert.ToString(opencnt);
                            gviewhmewrkdr["Not Open"] = unopen;
                            gviewhmewrkdr["Date"] = date;
                            gviewhmewrkdt.Rows.Add(gviewhmewrkdr);
                        }
                        else
                        {
                            subno = Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]);
                            string subjectname = d2.GetFunction("Select Subject_Name from subject where subject_no='" + subno + "'");

                            idno = Convert.ToString(dsset.Tables[0].Rows[i]["idno"]);

                            DataView dvdeliver = new DataView();
                            dsstatus.Tables[0].DefaultView.RowFilter = " idno='" + idno + "' and subjectno='" + subno + "' and Date='" + date + "' and delivered<>0";
                            dvdeliver = dsstatus.Tables[0].DefaultView;

                            string stucount = d2.GetFunction("select COUNT(distinct r.roll_no) as countStu from Registration r,subjectChooser sc where sc.roll_no=r.Roll_No and sc.subject_no='" + subno + "'");

                            int delivercnt = dvdeliver.Count;

                            int cntundeli = Convert.ToInt32(stucount) - Convert.ToInt32(delivercnt);

                            DataView dvOpen = new DataView();
                            dsstatus.Tables[0].DefaultView.RowFilter = " idno='" + idno + "' and subjectno='" + subno + "' and Date='" + date + "' and is_open<>0";
                            dvOpen = dsstatus.Tables[0].DefaultView;
                            int opencnt = dvOpen.Count;

                            int unopen = Convert.ToInt32(stucount) - Convert.ToInt32(opencnt);

                            sno++;
                            gviewhmewrkdr = gviewhmewrkdt.NewRow();

                            string uniq = Convert.ToString(dsset.Tables[0].Rows[i]["idno"]);
                            string subjectno = Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]);
                            string headng = Convert.ToString(dsset.Tables[0].Rows[i]["Heading"]);
                            string topic = Convert.ToString(dsset.Tables[0].Rows[i]["Homework"]);
                            string photo = Convert.ToString(dsset.Tables[0].Rows[i]["PhotoAttachment"]);
                            string file = Convert.ToString(dsset.Tables[0].Rows[i]["FileAttachment"]);

                            gviewhmewrkdr["S.No"] = sno.ToString();
                            gviewhmewrkdr["unid"] = uniq;
                            gviewhmewrkdr["Subject"] = subjectname;
                            gviewhmewrkdr["Subjectno"] = subjectno;
                            gviewhmewrkdr["Heading"] = headng;
                            gviewhmewrkdr["Description"] = topic;
                            gviewhmewrkdr["Photo"] = photo;
                            gviewhmewrkdr["Attachment"] = file;
                            gviewhmewrkdr["Delivered"] = Convert.ToString(delivercnt);
                            gviewhmewrkdr["Not Delivered"] = cntundeli;
                            gviewhmewrkdr["Open"] = Convert.ToString(opencnt);
                            gviewhmewrkdr["Not Open"] = unopen;
                            gviewhmewrkdr["Date"] = date;

                            gviewhmewrkdt.Rows.Add(gviewhmewrkdr);
                        }
                    }
                }
                gview.DataSource = gviewhmewrkdt;
                gview.DataBind();
                gview.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                gview.Rows[0].Font.Bold = true;
                gview.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                gview.Rows[0].Font.Name = "Book Antique";
                if (gview.Rows.Count > 1)
                {
                    gview.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;
                    btnprintmaster.Visible = true;
                    //NEWPrintMater1.Visible = true;
                }
                else
                {
                    errlbl.Text = "No Record Found";
                    gview.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    NEWPrintMater1.Visible = false;
                }
                gview.HeaderRow.Cells[1].Visible = false;
                gview.HeaderRow.Cells[3].Visible = false;
                gview.HeaderRow.Cells[12].Visible = false;
                for (int i = 0; i < gview.Rows.Count; i++)
                {
                    gview.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    gview.Rows[i].Cells[8].HorizontalAlign = HorizontalAlign.Center;
                    gview.Rows[i].Cells[9].HorizontalAlign = HorizontalAlign.Center;
                    gview.Rows[i].Cells[10].HorizontalAlign = HorizontalAlign.Center;
                    gview.Rows[i].Cells[11].HorizontalAlign = HorizontalAlign.Center;
                    gview.Rows[i].Cells[1].Visible = false;
                    gview.Rows[i].Cells[3].Visible = false;
                    gview.Rows[i].Cells[12].Visible = false;
                    string datte = gview.Rows[i].Cells[0].Text;
                    if (datte.Contains('/'))
                    {
                        gview.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        gview.Rows[i].Cells[0].ColumnSpan = 11;
                        gview.Rows[i].Cells[0].Font.Bold = true;
                        //gview.Rows[i].Cells[1].Visible = false;
                        //gview.Rows[i].Cells[3].Visible = false;
                        for (int cell = 1; cell < gview.Rows[i].Cells.Count; cell++)
                        {
                            gview.Rows[i].Cells[cell].Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void gviewOnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex != 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //for (int i = 0; i < e.Row.Cells.Count; i++)
                    //{
                    e.Row.Cells[6].ForeColor = Color.Blue;
                    e.Row.Cells[6].BorderColor = Color.Black;
                    //e.Row.Cells[6].Font.Underline = true;
                    TableCell cell = e.Row.Cells[6];
                    cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                    cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                    cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                       , SelectedGridCellIndex.ClientID, 6
                       , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));

                    e.Row.Cells[7].ForeColor = Color.Blue;
                    e.Row.Cells[7].BorderColor = Color.Black;
                    //e.Row.Cells[7].Font.Underline = true;
                    TableCell cell1 = e.Row.Cells[7];
                    cell1.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                    cell1.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                    cell1.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                       , SelectedGridCellIndex.ClientID, 7
                       , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));

                    e.Row.Cells[8].ForeColor = Color.Blue;
                    e.Row.Cells[8].BorderColor = Color.Black;
                    //e.Row.Cells[8].Font.Underline = true;
                    TableCell cell2 = e.Row.Cells[8];
                    cell2.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                    cell2.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                    cell2.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                       , SelectedGridCellIndex.ClientID, 8
                       , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));


                    e.Row.Cells[9].ForeColor = Color.Blue;
                    e.Row.Cells[9].BorderColor = Color.Black;
                    //e.Row.Cells[9].Font.Underline = true;
                    TableCell cell3 = e.Row.Cells[9];
                    cell3.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                    cell3.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                    cell3.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                       , SelectedGridCellIndex.ClientID, 9
                       , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));


                    e.Row.Cells[10].ForeColor = Color.Blue;
                    e.Row.Cells[10].BorderColor = Color.Black;
                    //e.Row.Cells[10].Font.Underline = true;
                    TableCell cell4 = e.Row.Cells[10];
                    cell4.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                    cell4.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                    cell4.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                       , SelectedGridCellIndex.ClientID, 10
                       , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));


                    e.Row.Cells[11].ForeColor = Color.Blue;
                    e.Row.Cells[11].BorderColor = Color.Black;
                    //e.Row.Cells[11].Font.Underline = true;
                    TableCell cell5 = e.Row.Cells[11];
                    cell5.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                    cell5.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                    cell5.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                       , SelectedGridCellIndex.ClientID, 11
                       , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
                    //}
                }
            }
        }
    }

    protected void gview_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            string activerow = string.Empty;
            string activecol = string.Empty;

            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndx = grid.SelectedIndex;
            int colIndx = int.Parse(this.SelectedGridCellIndex.Value);

            //string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[1].Replace("ctl", string.Empty);
            //int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string iduni = gview.Rows[rowIndx].Cells[1].Text; //(gviewhomework.Rows[rowIndx].FindControl("lbluniq") as Label).Text;
            string picname = gview.Rows[rowIndx].Cells[6].Text;
            string filenme = gview.Rows[rowIndx].Cells[7].Text;

            DataSet dspicture = new DataSet();
            DataSet dspicture1 = new DataSet();
            DataSet dsdelivered = new DataSet();

            activerow = rowIndx.ToString();
            activecol = colIndx.ToString();

            if (Convert.ToInt32(activecol) == 6 && picname != "&nbsp;")
            {
                string qrys = "select PhotoAttachment,PhotoContentType,PhotoData from Home_Work where idno='" + iduni + "'";
                dspicture.Clear();
                dspicture = d2.select_method_wo_parameter(qrys, "Text");
                if (dspicture.Tables.Count > 0 && dspicture.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dspicture.Tables[0].Rows.Count; i++)
                    {
                        Response.ContentType = dspicture.Tables[0].Rows[i]["PhotoContentType"].ToString();
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dspicture.Tables[0].Rows[i]["PhotoAttachment"] + "\"");
                        Response.BinaryWrite((byte[])dspicture.Tables[0].Rows[i]["PhotoData"]);
                        Response.End();
                    }
                }
            }
            else if (Convert.ToInt32(activecol) == 7 && filenme != "&nbsp;")
            {
                string qrys = "select FileAttachment,FileContentType,FileData from Home_Work where idno='" + iduni + "'";
                dspicture1.Clear();
                dspicture1 = d2.select_method_wo_parameter(qrys, "Text");
                if (dspicture1.Tables.Count > 0 && dspicture1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dspicture1.Tables[0].Rows.Count; i++)
                    {
                        Response.ContentType = dspicture1.Tables[0].Rows[i]["FileContentType"].ToString();
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dspicture1.Tables[0].Rows[i]["FileAttachment"] + "\"");
                        Response.BinaryWrite((byte[])dspicture1.Tables[0].Rows[i]["FileData"]);
                        Response.End();
                    }
                }
            }
            else if (Convert.ToInt32(activecol) == 8)
            {
                string id = gview.Rows[rowIndx].Cells[1].Text;
                string datee = gview.Rows[rowIndx].Cells[12].Text;
                string subno = gview.Rows[rowIndx].Cells[3].Text;
                string qry = "select r.Roll_No RollNo,r.Stud_Name Name from stud_homework_status hs,Registration r where r.App_No=hs.app_no and hs.homework_id='" + id + "' and hs.date='" + datee + "' and hs.delivered<>0 order by Roll_No";

                //string qry = "select r.Roll_No RollNo,r.Stud_Name Name from subjectChooser sc,Registration r where sc.roll_no=r.Roll_No and r.roll_no not in(select hs.app_no from stud_homework_status hs where hs.homework_id='" + id + "' and hs.date='" + datee + "' and hs.delivered<>0) and sc.subject_no='" + subno + "'";
                dsdelivered = d2.select_method_wo_parameter(qry, "Text");

                if (dsdelivered.Tables.Count > 0 && dsdelivered.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = dsdelivered;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                    divPopSpread.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                }
            }
            else if (Convert.ToInt32(activecol) == 9)
            {
                string id = gview.Rows[rowIndx].Cells[1].Text;
                string datee = gview.Rows[rowIndx].Cells[12].Text;
                string subno = gview.Rows[rowIndx].Cells[3].Text;
                //string qry = "select r.Roll_No RollNo,r.Stud_Name Name from stud_homework_status hs,Registration r where r.App_No=hs.app_no and hs.homework_id='" + id + "' and hs.date='" + datee + "' and hs.delivered<>1 order by Roll_No";
                string qry = "select r.Roll_No RollNo,r.Stud_Name Name from subjectChooser sc,Registration r where sc.roll_no=r.Roll_No and r.App_No in(select hs.app_no from stud_homework_status hs where hs.homework_id='" + id + "' and hs.date='" + datee + "' and hs.delivered<>1) and sc.subject_no='" + subno + "'";
                dsdelivered.Clear();
                dsdelivered = d2.select_method_wo_parameter(qry, "Text");
                if (dsdelivered.Tables.Count > 0 && dsdelivered.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = dsdelivered;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                    divPopSpread.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                }

            }
            else if (Convert.ToInt32(activecol) == 10)
            {
                string id = gview.Rows[rowIndx].Cells[1].Text;
                string datee = gview.Rows[rowIndx].Cells[12].Text;
                string subno = gview.Rows[rowIndx].Cells[3].Text;
                string qry = "select r.Roll_No RollNo,r.Stud_Name Name from stud_homework_status hs,Registration r where r.App_No=hs.app_no and hs.homework_id='" + id + "' and hs.date='" + datee + "' and hs.is_open<>0 order by Roll_No";

                //string qry = "select r.Roll_No RollNo,r.Stud_Name Name from subjectChooser sc,Registration r where sc.roll_no=r.Roll_No and r.roll_no not in(select hs.app_no from stud_homework_status hs where hs.homework_id='" + id + "' and hs.date='" + datee + "' and hs.is_open<>0) and sc.subject_no='" + subno + "'";
                dsdelivered.Clear();
                dsdelivered = d2.select_method_wo_parameter(qry, "Text");

                if (dsdelivered.Tables.Count > 0 && dsdelivered.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = dsdelivered;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                    divPopSpread.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                }
            }
            else if (Convert.ToInt32(activecol) == 11)
            {
                string id = gview.Rows[rowIndx].Cells[1].Text;
                string datee = gview.Rows[rowIndx].Cells[12].Text;
                string subno = gview.Rows[rowIndx].Cells[3].Text;
                //string qry = "select r.Roll_No RollNo,r.Stud_Name Name from stud_homework_status hs,Registration r where r.App_No=hs.app_no and hs.homework_id='" + id + "' and hs.date='" + datee + "' and hs.is_open<>1 order by Roll_No";

                string qry = "select r.Roll_No RollNo,r.Stud_Name Name from subjectChooser sc,Registration r where sc.roll_no=r.Roll_No and r.App_No in(select hs.app_no from stud_homework_status hs where hs.homework_id='" + id + "' and hs.date='" + datee + "' and hs.is_open<>1) and sc.subject_no='" + subno + "'";
                dsdelivered.Clear();
                dsdelivered = d2.select_method_wo_parameter(qry, "Text");
                if (dsdelivered.Tables.Count > 0 && dsdelivered.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = dsdelivered;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                    divPopSpread.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                }
            }
        }
        catch
        {
        }
    }

    protected void btnclosespread_OnClick(object sender, EventArgs e)
    {
        divPopSpread.Visible = false;
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
        catch { }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        string reportname = txtexcelname.Text;
        if (reportname.ToString().Trim() != "")
        {
            d2.printexcelreportgrid(gview, reportname);
        }
        else
        {
            errlbl.Visible = true;
            errlbl.Text = "Please Enter Report Name";
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;
        degreedetails = "Student Home Work Report" + "@Degree: " + ddlbatch.SelectedItem.Text.ToString() + "-" + ddldegree.SelectedItem.Text.ToString() + "- Sem -" + ddlduration.SelectedItem.Text.ToString();
        string pagename = "studenthomeworkreport.aspx";
        string ss = Session["usercode"].ToString();
        NEWPrintMater1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
        NEWPrintMater1.Visible = true;
    }
}