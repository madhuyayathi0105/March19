using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Configuration;

public partial class TransportAllotReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataSet dsstud = new DataSet();
    ArrayList colord = new ArrayList();
    ArrayList colsndord = new ArrayList();
    ArrayList sendcol = new ArrayList();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    bool fpcellclick = false;
    static byte roll = 0;
    ArrayList ItemList = new ArrayList();
    ArrayList Itemindex = new ArrayList();
    Hashtable htcol = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                loadstrm();
                bindBtch();
                binddeg();
                binddept();
                //  bindsem();
                binddesg();
                bindstafdept();
                bindroute();
                bindvechileid();
                loadvechilestage();
                loadheader();
                //loadledger();
                Month();
                year();
                rbstud_Changed(sender, e);
                bindsem();
                RollAndRegSettings();
                rbsem_Changed(sender, e);
                loadcolorder();
                //loadsecondcolorder();
                vehicleType();
                getSchoolDetails(sender, e);//check school or college settings

            }
            if (ddlcollege.Items.Count > 0)
            {
                if (ddlcollege.SelectedItem.Text.Trim() == "ALL")
                {
                    string sel = "select college_code from collinfo";
                    ds = d2.select_method_wo_parameter(sel, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int clg = 0; clg < ds.Tables[0].Rows.Count; clg++)
                        {
                            if (collegecode == "")
                                collegecode = Convert.ToString(ds.Tables[0].Rows[clg]["college_code"]);
                            else
                                collegecode = collegecode + "'" + "," + "'" + Convert.ToString(ds.Tables[0].Rows[clg]["college_code"]);
                        }
                    }
                }
                else
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
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
        Response.Redirect("default.aspx", false);

    }



    #region college

    public void loadcollege()
    {
        try
        {
            ddlcollege.Items.Clear();
            ds.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                if (ddlcollege.Items.Count > 0)
                {
                    //  ddlcollege.Items.Add(new ListItem("ALL", "1"));
                }
            }
        }
        catch
        { }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = "";
            if (ddlcollege.Items.Count > 0)
            {
                if (ddlcollege.SelectedItem.Text.Trim() == "ALL")
                {
                    string sel = "select college_code from collinfo";
                    ds = d2.select_method_wo_parameter(sel, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int clg = 0; clg < ds.Tables[0].Rows.Count; clg++)
                        {
                            if (collegecode == "")
                                collegecode = Convert.ToString(ds.Tables[0].Rows[clg]["college_code"]);
                            else
                                collegecode = collegecode + "'" + "," + "'" + Convert.ToString(ds.Tables[0].Rows[clg]["college_code"]);
                        }
                    }
                }
                else
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            loadstrm();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            binddeg();
            bindstafdept();
            bindroute();
            bindvechileid();
            loadvechilestage();
            loadheader();
            loadledger();
            vehicleType();
        }
        catch
        {
        }
    }
    #endregion

    #region stream

    public void loadstrm()
    {
        try
        {
            ddlstream.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + ddlcollege.SelectedItem.Value + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
                ddlstream.Enabled = true;
            }
            else
            {
                ddlstream.Enabled = false;
            }
            binddeg();
        }
        catch
        { }
    }
    protected void ddlstream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //  string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            string stream = ddlstream.SelectedItem.Text.ToString();
            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code in('" + collegecode + "')";
            if (stream != "")
            {
                selqry = selqry + "and type  in('" + stream + "')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "Course_Name";
                cbl_degree.DataValueField = "Course_Id";
                cbl_degree.DataBind();
            }
            for (int j = 0; j < cbl_degree.Items.Count; j++)
            {
                cbl_degree.Items[j].Selected = true;
                cb_degree.Checked = true;
            }

            txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
            binddept();
        }
        catch { }
    }
    #endregion

    //student
    #region batch
    public void bindBtch()
    {
        try
        {

            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            binddeg();
            binddept();
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            binddeg();
            binddept();
        }
        catch { }
    }
    #endregion

    #region degree
    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            string stream = "";
            if (ddlstream.Items.Count > 0)
            {
                if (ddlstream.SelectedItem.Text != "")
                {
                    stream = ddlstream.SelectedItem.Text.ToString();
                }
            }

            cbl_degree.Items.Clear();
            //  string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code in('" + collegecode + "')";
            if (stream != "")
            {
                selqry = selqry + " and type  in('" + stream + "')";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    #endregion

    #region dept
    public void binddept()
    {
        try
        {
            string batch2 = "";
            string degree = "";
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            batch2 = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch2 == "")
                    {
                        batch2 = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch2 += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }

            degree = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree = degree + "'" + "," + "'" + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            // string collegecode = ddlcollege.SelectedItem.Value.ToString();
            if (batch2 != "" && degree != "")
            {
                // ds.Clear();
                //  ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                //string sel = "  select dt.Dept_Name,d.degree_code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.Course_Id in('" + degree + "') and d.college_code in('" + collegecode + "')";
                string strquery1 = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + degree + "') and degree.college_code in('" + collegecode + "')  and deptprivilages.Degree_code=degree.Degree_code ";
                if (singleuser == "True")
                    strquery1 += " ";
                else
                    strquery1 += " and group_code=" + group_user + "";
                strquery1 += "   order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc";
                ds = d2.select_method_wo_parameter(strquery1, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }

        }
        catch { }
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            //bindsem();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            //bindsem();
        }
        catch { }
    }
    #endregion

    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
        }
        catch (Exception ex)
        { }

    }

    protected void bindsem()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = d2.loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "TextVal";
                cbl_sem.DataValueField = "TextCode";
                cbl_sem.DataBind();

                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                        cbltext = Convert.ToString(cbl_sem.Items[i].Text);
                    }
                    if (cbl_sem.Items.Count == 1)
                        txt_sem.Text = "" + linkName + "(" + cbltext + ")";
                    else
                        txt_sem.Text = "" + linkName + "(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }
    }

    //protected void bindsem()
    //{
    //    try
    //    {
    //        string sem = "";
    //        string clgvalue = ddlcollege.SelectedItem.Value.ToString();
    //        if (rball.Checked == true)
    //        {
    //            string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code in('" + collegecode + "') order by len(textval),textval asc";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(SelectQ, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                cbl_sem.DataSource = ds;
    //                cbl_sem.DataTextField = "TextVal";
    //                cbl_sem.DataValueField = "TextCode";
    //                cbl_sem.DataBind();
    //            }
    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                {
    //                    cbl_sem.Items[i].Selected = true;
    //                    sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                }
    //                if (cbl_sem.Items.Count == 1)
    //                {
    //                    txt_sem.Text = "SemesterandYear(" + sem + ")";
    //                }
    //                else
    //                {
    //                    txt_sem.Text = "SemesterandYear(" + cbl_sem.Items.Count + ")";
    //                }
    //                cb_sem.Checked = true;
    //            }
    //        }
    //        else if (rbsem.Checked == true)
    //        {
    //            string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code in('" + collegecode + "') order by len(textval),textval asc";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                cbl_sem.DataSource = ds;
    //                cbl_sem.DataTextField = "TextVal";
    //                cbl_sem.DataValueField = "TextCode";
    //                cbl_sem.DataBind();
    //            }
    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                {
    //                    cbl_sem.Items[i].Selected = true;
    //                    sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                }
    //                if (cbl_sem.Items.Count == 1)
    //                {
    //                    txt_sem.Text = "Semester(" + sem + ")";
    //                }
    //                else
    //                {
    //                    txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
    //                }
    //                cb_sem.Checked = true;
    //                spsem.Text = "Semester";
    //            }
    //        }
    //        else if (rbyear.Checked == true)
    //        {
    //            string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code in('" + collegecode + "') order by len(textval),textval asc";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                cbl_sem.DataSource = ds;
    //                cbl_sem.DataTextField = "TextVal";
    //                cbl_sem.DataValueField = "TextCode";
    //                cbl_sem.DataBind();
    //            }
    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                {
    //                    cbl_sem.Items[i].Selected = true;
    //                    sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                }
    //                if (cbl_sem.Items.Count == 1)
    //                {
    //                    txt_sem.Text = "Year(" + sem + ")";
    //                }
    //                else
    //                {
    //                    txt_sem.Text = "Year(" + cbl_sem.Items.Count + ")";
    //                }
    //                cb_sem.Checked = true;
    //                spsem.Text = "Year";
    //            }
    //        }
    //    }
    //    catch { }
    //}
    #endregion

    //staff
    #region desgination

    public void binddesg()
    {
        try
        {
            cbldesg.Items.Clear();
            //string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select desig_code,desig_name from desig_master where collegeCode in('" + collegecode + "')";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbldesg.DataSource = ds;
                cbldesg.DataTextField = "desig_name";
                cbldesg.DataValueField = "desig_code";
                cbldesg.DataBind();
                if (cbldesg.Items.Count > 0)
                {
                    for (int i = 0; i < cbldesg.Items.Count; i++)
                    {
                        cbldesg.Items[i].Selected = true;
                    }
                    txtdesg.Text = "Desgination(" + cbldesg.Items.Count + ")";
                    cbdesg.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cbdesg_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbdesg, cbldesg, txtdesg, "Desgination", "--Select--");
            //  binddept();
        }
        catch { }
    }
    protected void cbldesg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbdesg, cbldesg, txtdesg, "Desgination", "--Select--");
            // binddept();
        }
        catch { }
    }
    #endregion

    #region departemnt

    public void bindstafdept()
    {
        try
        {
            cblstafdept.Items.Clear();
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct dept_code,dept_name from hrdept_master where college_code='" + clgvalue + "'";

            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstafdept.DataSource = ds;
                cblstafdept.DataTextField = "dept_name";
                cblstafdept.DataValueField = "dept_code";
                cblstafdept.DataBind();
                if (cblstafdept.Items.Count > 0)
                {
                    for (int i = 0; i < cblstafdept.Items.Count; i++)
                    {
                        cblstafdept.Items[i].Selected = true;
                    }
                    txtstafdept.Text = "Departemnt(" + cblstafdept.Items.Count + ")";
                    cbstafdept.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cbstafdept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbstafdept, cblstafdept, txtstafdept, "Department", "--Select--");
            //  binddept();
        }
        catch { }
    }
    protected void cblstafdept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbstafdept, cblstafdept, txtstafdept, "Department", "--Select--");
            // binddept();
        }
        catch { }
    }
    #endregion

    //header and ledger
    #region headerandledger
    public void loadheader()
    {
        try
        {
            string header = "";
            chkl_studhed.Items.Clear();
            string hedaderid = "";
            string ledgerid = "";
            // string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string ledgersett = "select LinkValue from New_InsSettings where LinkName='TransportLedgerValue' and user_code='" + usercode + "' and college_code in('" + collegecode + "') ";
            ds = d2.select_method_wo_parameter(ledgersett, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                for (int hed = 0; hed < ds.Tables[0].Rows.Count; hed++)
                {
                    string value = Convert.ToString(ds.Tables[0].Rows[hed]["LinkValue"]);
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        if (hedaderid == "" && ledgerid == "")
                        {
                            hedaderid = valuesplit[0];
                            ledgerid = valuesplit[1];
                        }
                        else
                        {
                            hedaderid = hedaderid + "'" + "," + "'" + valuesplit[0];
                            ledgerid = ledgerid + "'" + "," + "'" + valuesplit[1];
                        }
                    }
                }
                if (hedaderid != "" && ledgerid != "")
                {
                    //   string headname = "select HeaderName,HeaderPK from FM_HeaderMaster where headerPK in('" + hedaderid + "')";
                    string headname = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode + " and h.headerPK in('" + hedaderid + "')  ";
                    DataSet dshed = new DataSet();
                    dshed = d2.select_method_wo_parameter(headname, "Text");
                    if (dshed.Tables[0].Rows.Count > 0)
                    {
                        chkl_studhed.DataSource = dshed;
                        chkl_studhed.DataValueField = "HeaderPK";
                        chkl_studhed.DataTextField = "HeaderName";
                        chkl_studhed.DataBind();

                        for (int i = 0; i < chkl_studhed.Items.Count; i++)
                        {
                            chkl_studhed.Items[i].Selected = true;
                        }
                        txt_studhed.Text = "Header(" + chkl_studhed.Items.Count + ")";
                        chk_studhed.Checked = true; ;
                    }


                    string hed = "";
                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                    {
                        if (chkl_studhed.Items[i].Selected == true)
                        {
                            if (hed == "")
                            {
                                hed = chkl_studhed.Items[i].Value.ToString();
                            }
                            else
                            {
                                hed = hed + "','" + "" + chkl_studhed.Items[i].Value.ToString() + "";
                            }
                        }
                    }

                    // string ledgname = " select distinct LedgerPK,isnull(priority,1000),ledgerName from FM_LedgerMaster where (LedgerName not like 'Cash' and LedgerName not like 'Income & Expenditure' and LedgerName not like 'Misc') and LedgerName not in(select distinct BankName from FM_FinBankMaster) and HeaderFK in('" + hed + "') and LedgerPK in('" + ledgerid + "')  order by isnull(priority,1000),ledgerName asc ";
                    string ledgname = " SELECT distinct LedgerPK,isnull(priority,1000),ledgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode + " and (LedgerName not like 'Cash' and LedgerName not like 'Income & Expenditure' and LedgerName not like 'Misc') and LedgerName not in(select distinct BankName from FM_FinBankMaster) and L.HeaderFK in('" + hed + "') and l.LedgerPK in('" + ledgerid + "')  order by isnull(l.priority,1000), l.ledgerName asc ";

                    DataSet dsled = new DataSet();
                    dsled = d2.select_method_wo_parameter(ledgname, "Text");
                    if (dsled.Tables[0].Rows.Count > 0)
                    {
                        chkl_studled.DataSource = dsled;
                        chkl_studled.DataValueField = "LedgerPK";
                        chkl_studled.DataTextField = "LedgerName";
                        chkl_studled.DataBind();
                        for (int i = 0; i < chkl_studled.Items.Count; i++)
                        {
                            chkl_studled.Items[i].Selected = true;
                        }
                        txt_studled.Text = "Ledger(" + chkl_studled.Items.Count + ")";
                        chk_studled.Checked = true;
                    }

                }
            }
        }
        catch
        {
        }
    }
    public void loadledger()
    {
        try
        {
            string hedaderid = "";
            string ledgerid = "";
            string ledgersett = "select LinkValue from New_InsSettings where LinkName='TransportLedgerValue' and user_code='" + usercode + "' and college_code in('" + collegecode + "') ";
            ds = d2.select_method_wo_parameter(ledgersett, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                for (int hed = 0; hed < ds.Tables[0].Rows.Count; hed++)
                {
                    string value = Convert.ToString(ds.Tables[0].Rows[hed]["LinkValue"]);
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        if (hedaderid == "" && ledgerid == "")
                        {
                            hedaderid = valuesplit[0];
                            ledgerid = valuesplit[1];
                        }
                        else
                        {
                            hedaderid = hedaderid + "'" + "," + "'" + valuesplit[0];
                            ledgerid = ledgerid + "'" + "," + "'" + valuesplit[1];
                        }
                    }
                }
            }


            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            chkl_studled.Items.Clear();

            string hedd = "";
            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    if (hedd == "")
                    {
                        hedd = chkl_studhed.Items[i].Value.ToString();
                    }
                    else
                    {
                        hedd = hedd + "','" + "" + chkl_studhed.Items[i].Value.ToString() + "";
                    }
                }
            }

            // string query1 = " select distinct LedgerPK,isnull(priority,1000),ledgerName from FM_LedgerMaster where (LedgerName not like 'Cash' and LedgerName not like 'Income & Expenditure' and LedgerName not like 'Misc') and LedgerName not in(select distinct BankName from FM_FinBankMaster) and HeaderFK in('" + hedd + "') and ledgerPK in('" + ledgerid + "')  order by isnull(priority,1000), ledgerName asc ";
            string query1 = " SELECT distinct LedgerPK,isnull(priority,1000),ledgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode + " and (LedgerName not like 'Cash' and LedgerName not like 'Income & Expenditure' and LedgerName not like 'Misc') and LedgerName not in(select distinct BankName from FM_FinBankMaster) and L.HeaderFK in('" + hedd + "') and l.LedgerPK in('" + ledgerid + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studled.DataSource = ds;
                chkl_studled.DataTextField = "LedgerName";
                chkl_studled.DataValueField = "LedgerPK";
                chkl_studled.DataBind();
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = true;
                }
                txt_studled.Text = "Ledger(" + chkl_studled.Items.Count + ")";
                chk_studled.Checked = true;

            }
            else
            {
                chkl_studled.Items.Clear();
            }

        }
        catch
        {
        }
    }


    public void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, "Header", "--Select--");
            loadledger();
        }
        catch (Exception ex)
        { }
    }

    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, "Header", "--Select--");
            loadledger();
        }
        catch (Exception ex)
        {

        }
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");

        }
        catch (Exception ex)
        { }
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
        }
        catch (Exception ex)
        { }
    }

    protected void ddlheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadledger();
    }
    //public void cbheader_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxChange(cbheader, cblheader, txtheader, "Header", "--Select--");
    //        loadledger();
    //    }
    //    catch (Exception ex)
    //    { }
    //}

    //public void cblheader_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxListChange(cbheader, cblheader, txtheader, "Header", "--Select--");
    //        loadledger();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    //public void cbledger_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxChange(cbledger, cblledger, txtledger, "Ledger", "--Select--");

    //    }
    //    catch (Exception ex)
    //    { }
    //}
    //public void cblledger_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxListChange(cbledger, cblledger, txtledger, "Ledger", "--Select--");
    //    }
    //    catch (Exception ex)
    //    { }
    //}
    #endregion

    //route and stage

    #region Route

    public void bindroute()
    {
        try
        {
            cblroute.Items.Clear();
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct Route_ID from routemaster order by Route_ID";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblroute.DataSource = ds;
                cblroute.DataTextField = "Route_ID";
                cblroute.DataValueField = "Route_ID";
                cblroute.DataBind();
                if (cblroute.Items.Count > 0)
                {
                    for (int i = 0; i < cblroute.Items.Count; i++)
                    {
                        cblroute.Items[i].Selected = true;
                    }
                    txtroute.Text = "Route ID(" + cblroute.Items.Count + ")";
                    cbroute.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cbroute_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbroute, cblroute, txtroute, "Route", "--Select--");
            //  binddept();
            bindvechileid();
            loadvechilestage();
        }
        catch { }
    }
    protected void cblroute_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbroute, cblroute, txtroute, "Route", "--Select--");
            // binddept();
            loadvechilestage();
            bindvechileid();
        }
        catch { }
    }
    #endregion

    #region vechile id

    public void bindvechileid()
    {
        try
        {
            cblvechile.Items.Clear();
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            string route = "";
            for (int i = 0; i < cblroute.Items.Count; i++)
            {
                if (cblroute.Items[i].Selected == true)
                {
                    if (route == "")
                    {
                        route = Convert.ToString(cblroute.Items[i].Value);
                    }
                    else
                    {
                        route += "','" + Convert.ToString(cblroute.Items[i].Value);
                    }
                }
            }
            ds.Clear();
            string selqry = "select distinct Veh_ID from vehicle_master where route in('" + route + "')  order by Veh_ID";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblvechile.DataSource = ds;
                cblvechile.DataTextField = "Veh_ID";
                cblvechile.DataValueField = "Veh_ID";
                cblvechile.DataBind();
                if (cblvechile.Items.Count > 0)
                {
                    for (int i = 0; i < cblvechile.Items.Count; i++)
                    {
                        cblvechile.Items[i].Selected = true;
                    }
                    txtvechile.Text = "Vechile ID(" + cblvechile.Items.Count + ")";
                    cbvechile.Checked = true;
                }
                loadvechilestage();
            }
            else
            {
                txtvechile.Text = "Select";
                cbvechile.Checked = false;
            }

        }
        catch { }
    }
    protected void cbvechile_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbvechile, cblvechile, txtvechile, "Vechile ID", "--Select--");
            //  binddept();
            loadvechilestage();
        }
        catch { }
    }
    protected void cblvechile_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbvechile, cblvechile, txtvechile, "Vechile ID", "--Select--");
            // binddept();
            loadvechilestage();
        }
        catch { }
    }
    #endregion

    #region Stage

    //public void bindstage()
    //{
    //    try
    //    {
    //        cblstage.Items.Clear();
    //        string clgvalue = ddlcollege.SelectedItem.Value.ToString();
    //        ds.Clear();
    //        string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
    //        //if (stream != "")
    //        //{
    //        //    selqry = selqry + " and type  in('" + stream + "')";
    //        //}
    //        ds = d2.select_method_wo_parameter(selqry, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cblstage.DataSource = ds;
    //            cblstage.DataTextField = "course_name";
    //            cblstage.DataValueField = "course_id";
    //            cblstage.DataBind();
    //            if (cblstage.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cblstage.Items.Count; i++)
    //                {
    //                    cblstage.Items[i].Selected = true;
    //                }
    //                txtstage.Text = "Stage(" + cblstage.Items.Count + ")";
    //                cbstage.Checked = true;
    //            }
    //        }

    //    }
    //    catch { }
    //}
    public void loadvechilestage()
    {
        string sqlquery = string.Empty;
        string filter = "";
        cblstage.Items.Clear();
        //   cblstage.Items.Insert(0, new ListItem("All", "-1"));
        string route = "";
        for (int i = 0; i < cblroute.Items.Count; i++)
        {
            if (cblroute.Items[i].Selected == true)
            {
                if (route == "")
                {
                    route = Convert.ToString(cblroute.Items[i].Value);
                }
                else
                {
                    route += "','" + Convert.ToString(cblroute.Items[i].Value);
                }
            }
        }
        string vechile = "";
        for (int i = 0; i < cblvechile.Items.Count; i++)
        {
            if (cblvechile.Items[i].Selected == true)
            {
                if (vechile == "")
                {
                    vechile = Convert.ToString(cblvechile.Items[i].Value);
                }
                else
                {
                    vechile += "','" + Convert.ToString(cblvechile.Items[i].Value);
                }
            }
        }

        if (route != "-1")
        {
            filter = " and v.Route in('" + route + "')";
        }
        if (vechile != "-1")
        {
            filter = filter + ' ' + "and r.Veh_ID in('" + vechile + "')";
        }

        sqlquery = "select distinct Stage_Name from routemaster r,vehicle_master v where Stage_Name is not null and Stage_Name<>'' and v.Veh_ID=r.Veh_ID " + filter + "";

        ds = d2.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Boolean e1 = isNumeric(ds.Tables[0].Rows[i]["Stage_Name"].ToString(), System.Globalization.NumberStyles.Integer);
                if (e1)
                {
                    string Get_Stage = d2.GetFunction("select distinct Stage_Name from stage_master where Stage_id = '" + ds.Tables[0].Rows[i]["Stage_Name"].ToString() + "'");
                    string Get_Stage_id = d2.GetFunction("select distinct Stage_id from stage_master where Stage_id = '" + ds.Tables[0].Rows[i]["Stage_Name"].ToString() + "'");
                    cblstage.Items.Add(new ListItem(Get_Stage, Get_Stage_id));//Added By SRinath 8/10/2013
                }
                else
                {
                    cblstage.Items.Add(ds.Tables[0].Rows[i]["Stage_Name"].ToString());
                }

            }
            if (cblstage.Items.Count > 0)
            {
                for (int i = 0; i < cblstage.Items.Count; i++)
                {
                    cblstage.Items[i].Selected = true;
                }
                txtstage.Text = "Stage(" + cblstage.Items.Count + ")";
                cbstage.Checked = true;
            }
        }
        else
        {
            txtstage.Text = "Select";
            cbstage.Checked = false;
        }
        // cblstage.SelectedIndex = 0;
    }
    protected void cbstage_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbstage, cblstage, txtstage, "Stage", "--Select--");
            //  binddept();
        }
        catch { }
    }
    protected void cblstage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbstage, cblstage, txtstage, "Stage", "--Select--");
            // binddept();
        }
        catch { }
    }
    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    #endregion

    #region vehicle type

    public void vehicleType()
    {
        try
        {
            cblvehtype.Items.Clear();
            cbvehtype.Checked = false;
            txtvehtype.Text = "--Select--";
            cblvehtype.Items.Add(new ListItem("Own", "0"));
            cblvehtype.Items.Add(new ListItem("Dealer", "1"));
            if (cblroute.Items.Count > 0)
            {
                for (int i = 0; i < cblvehtype.Items.Count; i++)
                {
                    cblvehtype.Items[i].Selected = true;
                }
                txtvehtype.Text = "Vehicle Type(" + cblvehtype.Items.Count + ")";
                cbvehtype.Checked = true;
            }
        }
        catch { }
    }
    protected void cbvehtype_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbvehtype, cblvehtype, txtvehtype, "Vehicle Type", "--Select--");
    }
    protected void cblvehtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbvehtype, cblvehtype, txtvehtype, "Vehicle Type", "--Select--");
    }
    #endregion

    //all rb events
    #region all radion button event

    protected void rbstage_Changed(object sender, EventArgs e)
    {

        rbstage.Checked = true;
        rbroute.Checked = false;

        divspread.Visible = false;
        FpSpread1.Visible = false;
        gnlcolorder.Visible = false;
        studdet.Visible = false;
        fpstud.Visible = false;
        lblvalidation1.Text = "";
        print.Visible = false;
        subprint.Visible = false;

    }
    protected void rbroute_Changed(object sender, EventArgs e)
    {

        rbstage.Checked = false;
        rbroute.Checked = true;

        divspread.Visible = false;
        FpSpread1.Visible = false;
        gnlcolorder.Visible = false;
        studdet.Visible = false;
        fpstud.Visible = false;
        lblvalidation1.Text = "";
        print.Visible = false;
        subprint.Visible = false;
    }
    protected void rbstud_Changed(object sender, EventArgs e)
    {
        try
        {
            trstud.Visible = true;
            tdstaf.Visible = false;
            rbstud.Checked = true;
            rbstaff.Checked = false;
            rbboth.Checked = false;

            rbstage.Checked = true;
            rbroute.Checked = false;

            rbsem.Checked = true;
            rbyear.Checked = false;
            rbmonth.Checked = false;

            divspread.Visible = false;
            FpSpread1.Visible = false;
            gnlcolorder.Visible = false;
            studdet.Visible = false;
            fpstud.Visible = false;
            lblvalidation1.Text = "";
            print.Visible = false;
            subprint.Visible = false;
        }
        catch { }
    }
    protected void rbstaff_Changed(object sender, EventArgs e)
    {
        try
        {
            trstud.Visible = false;
            tdstaf.Visible = true;
            rbstud.Checked = false;
            rbstaff.Checked = true;
            rbboth.Checked = false;

            rbstage.Checked = true;
            rbroute.Checked = false;

            rbsem.Checked = false;
            rbyear.Checked = false;
            rbmonth.Checked = false;

            divspread.Visible = false;
            FpSpread1.Visible = false;
            gnlcolorder.Visible = false;
            studdet.Visible = false;
            fpstud.Visible = false;
            lblvalidation1.Text = "";
            print.Visible = false;
            subprint.Visible = false;
        }
        catch { }
    }

    protected void rbboth_Changed(object sender, EventArgs e)
    {
        try
        {
            trstud.Visible = true;
            tdstaf.Visible = true;
            rbstud.Checked = false;
            rbstaff.Checked = false;
            rbboth.Checked = true;

            rbstage.Checked = true;
            rbroute.Checked = false;

            rbsem.Checked = false;
            rbyear.Checked = false;
            rbmonth.Checked = false;

            divspread.Visible = false;
            FpSpread1.Visible = false;
            gnlcolorder.Visible = false;
            studdet.Visible = false;
            fpstud.Visible = false;
            lblvalidation1.Text = "";
            print.Visible = false;
            subprint.Visible = false;
        }
        catch { }
    }
    protected void rball_Changed(object sender, EventArgs e)
    {
        tdsemyear.Visible = true;
        tdmonth.Visible = false;
        bindsem();

        divspread.Visible = false;
        FpSpread1.Visible = false;
        gnlcolorder.Visible = false;
        studdet.Visible = false;
        fpstud.Visible = false;
        lblvalidation1.Text = "";
        print.Visible = false;
        subprint.Visible = false;
    }
    protected void rbsem_Changed(object sender, EventArgs e)
    {
        tdsemyear.Visible = true;
        tdmonth.Visible = false;
        bindsem();

        divspread.Visible = false;
        FpSpread1.Visible = false;
        gnlcolorder.Visible = false;
        studdet.Visible = false;
        fpstud.Visible = false;
        lblvalidation1.Text = "";
        print.Visible = false;
        subprint.Visible = false;
    }
    //added by sudhagar 24.05.2017
    protected void rbterm_Changed(object sender, EventArgs e)
    {
        tdsemyear.Visible = true;
        tdmonth.Visible = false;
        bindsem();

        divspread.Visible = false;
        FpSpread1.Visible = false;
        gnlcolorder.Visible = false;
        studdet.Visible = false;
        fpstud.Visible = false;
        lblvalidation1.Text = "";
        print.Visible = false;
        subprint.Visible = false;
    }
    protected void rbyear_Changed(object sender, EventArgs e)
    {
        tdsemyear.Visible = true;
        tdmonth.Visible = false;
        bindsem();

        divspread.Visible = false;
        FpSpread1.Visible = false;
        gnlcolorder.Visible = false;
        studdet.Visible = false;
        fpstud.Visible = false;
        lblvalidation1.Text = "";
        print.Visible = false;
        subprint.Visible = false;
    }
    protected void rbmonthChanged(object sender, EventArgs e)
    {
        year();
        tdmonth.Visible = true;
        tdsemyear.Visible = false;

        divspread.Visible = false;
        FpSpread1.Visible = false;
        gnlcolorder.Visible = false;
        studdet.Visible = false;
        fpstud.Visible = false;
        lblvalidation1.Text = "";
        print.Visible = false;
        subprint.Visible = false;
    }

    protected void year()
    {
        try
        {
            string year = System.DateTime.Now.ToString("yyyy");
            int a1 = 0;
            for (int y = Convert.ToInt32(year); y >= 2005; y--)
            {
                a1++;
                ddlyear.Items.Add(new ListItem(Convert.ToString(y), Convert.ToString(y)));
            }
        }
        catch { }
    }
    protected void Month()
    {
        try
        {
            cblmonth.Items.Clear();
            cblmonth.Items.Add(new ListItem("JAN", "1"));
            cblmonth.Items.Add(new ListItem("FEB", "2"));
            cblmonth.Items.Add(new ListItem("MAR", "3"));
            cblmonth.Items.Add(new ListItem("APR", "4"));
            cblmonth.Items.Add(new ListItem("MAY", "5"));
            cblmonth.Items.Add(new ListItem("JUN", "6"));
            cblmonth.Items.Add(new ListItem("JUL", "7"));
            cblmonth.Items.Add(new ListItem("AUG", "8"));
            cblmonth.Items.Add(new ListItem("SEP", "9"));
            cblmonth.Items.Add(new ListItem("OCT", "10"));
            cblmonth.Items.Add(new ListItem("NOV", "11"));
            cblmonth.Items.Add(new ListItem("DEC", "12"));
            if (cblmonth.Items.Count > 0)
            {
                for (int i = 0; i < cblmonth.Items.Count; i++)
                {
                    cblmonth.Items[i].Selected = true;
                }
                cbmonth.Checked = true;
                txtmonth.Text = "Month(" + cblmonth.Items.Count + ")";
            }
        }
        catch { }
    }
    protected void cbmonth_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbmonth, cblmonth, txtmonth, "Month", "--Select--");
        }
        catch { }
    }
    protected void cblmonth_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbmonth, cblmonth, txtmonth, "Month", "--Select--");
        }
        catch { }
    }

    #endregion

    //button search
    #region button go

    protected DataSet DsValues()
    {
        DataSet dsload = new DataSet();
        try
        {
            int method = 0;
            int typename = 0;
            string type = "";
            string SelQ = "";
            // string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            //   degcourseid = Convert.ToString(getCblSelectedValue(cbl_degree));
            string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            string sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            //string header = Convert.ToString(ddlheader.SelectedItem.Value);
            //string ledger = Convert.ToString(ddlledger.SelectedItem.Value);
            string header = Convert.ToString(getCblSelectedValue(chkl_studhed));
            string ledger = Convert.ToString(getCblSelectedValue(chkl_studled));
            string routeid = Convert.ToString(getCblSelectedValue(cblroute));
            string vechileid = Convert.ToString(getCblSelectedValue(cblvechile));
            string stageid = Convert.ToString(getCblSelectedValue(cblstage));
            string stafdesg = Convert.ToString(getCblSelectedValue(cbldesg));
            string stafdept = Convert.ToString(getCblSelectedValue(cblstafdept));
            //type
            string feecat = Convert.ToString(getCblSelectedValue(cbl_sem));
            //string ddlmnth = Convert.ToString(ddlmonth.SelectedItem.Value);
            string ddlmnth = Convert.ToString(getCblSelectedValue(cblmonth));
            string ddlyr = Convert.ToString(ddlyear.SelectedItem.Value);
            string vehiType = Convert.ToString(getCblSelectedValue(cblvehtype));
            if (rbstage.Checked == true)
                typename = 5;
            else
                typename = 6;

            if (rbsem.Checked == true)
            {
                type = "Semester";
                ddlmnth = "";
                ddlyr = "";
            }
            else if (rbyear.Checked == true)
            {
                type = "Yearly";
                ddlmnth = "";
                ddlyr = "";
            }
            else if (rball.Checked == true)
            {
                type = "Semester','Yearly";
                ddlmnth = "";
                ddlyr = "";
            }
            else
            {
                type = "Monthly";
                feecat = "";
            }
            string cancelStr = string.Empty;
            if (!cbcancel.Checked)
                cancelStr = " and isnull(IsCanceledStage,0)<>'1'";
            if (rbstud.Checked == true)
            {
                #region stud
                if (typename == 5)
                {
                    #region stage
                    if (type == "Semester" || type == "Yearly" || type == "Semester','Yearly")
                    {
                        #region sem or year
                        SelQ = "select boarding,stage_name,COUNT(distinct r.app_no) as totstud,sum(feeamount)as feeamt,sum(DeductAmout)as concession,sum(TotalAmount)as totamt,sum(PaidAmount)as paidamt,sum(BalAmount)as balamt,ledgerFK,a.FeeCategory from FT_FeeAllot a,Registration r,Stage_Master s where a.App_No = r.App_No and str(r.Boarding) = str(s.Stage_id )  " + cancelStr + " ";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += " and a.FeeCategory in('" + feecat + "')";
                        else
                            SelQ += "";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";

                        SelQ += "group by Boarding,Stage_Name ,ledgerFK,a.FeeCategory order by Stage_name ";

                        //fully paid
                        #region fully paid
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f where a.App_No = r.App_No and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace   and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += " and a.FeeCategory in('" + feecat + "')";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";
                        SelQ += "group by a.App_No,Boarding,a.FeeCategory having sum(TotalAmount) > 0 and sum(BalAmount) =0 ";

                        #endregion

                        #region partially paid
                        //partially paid
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f where a.App_No = r.App_No and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += " and a.FeeCategory in('" + feecat + "')";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";
                        SelQ += "group by a.App_No,Boarding,a.FeeCategory having sum(TotalAmount) <> sum(BalAmount) and sum(BalAmount) > 0 ";
                        #endregion

                        #region not paid

                        //not paid
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f where a.App_No = r.App_No and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += " and a.FeeCategory in('" + feecat + "')";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";
                        SelQ += "group by a.App_No,Boarding,a.FeeCategory having sum(TotalAmount) = sum(BalAmount)";
                        #endregion



                        #endregion
                    }
                    else
                    {
                        #region month
                        SelQ = "select boarding,stage_name,COUNT(distinct r.app_no) as totstud,sum(feeamount)as feeamt,sum(DeductAmout)as concession,sum(TotalAmount)as totamt,sum(a.PaidAmount)as paidamt,sum(a.BalAmount)as balamt,ledgerFK,a.FeeCategory from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";

                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";

                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";

                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";

                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";

                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";

                        SelQ += "group by Boarding,Stage_Name,ledgerFK,a.FeeCategory order by Stage_name ";

                        //fully paid
                        #region fully paid
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";

                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";

                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";

                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";

                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";

                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";

                        SelQ += "group by a.App_No,Boarding,a.FeeCategory having sum(TotalAmount) > 0 and sum(a.BalAmount) =0 ";
                        #endregion

                        #region partially paid
                        //partially paid
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";

                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";

                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";

                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";

                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";

                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";

                        SelQ += "group by a.App_No,Boarding,a.FeeCategory having sum(TotalAmount) <> sum(a.BalAmount) and sum(a.BalAmount) > 0 ";
                        #endregion

                        #region not paid
                        //not paid                       
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";

                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";

                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";

                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";

                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";

                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";

                        SelQ += "group by a.App_No,Boarding,a.FeeCategory having sum(TotalAmount) = sum(a.BalAmount) ";
                        #endregion

                        #endregion
                    }

                    #endregion
                }
                else
                {
                    #region route
                    if (type == "Semester" || type == "Yearly" || type == "Semester','Yearly")
                    {
                        #region sem or year
                        SelQ = " select bus_routeid,veh_id, COUNT(distinct a.App_No) as totstud,sum(feeamount) as feeamt,sum(DeductAmout) as concession,sum(totalAmount) as totamt,sum(PaidAmount) as paidamt, sum(BalAmount) as balamt,ledgerFK,a.FeeCategory from FT_FeeAllot a,Registration r,Vehicle_Master v where a.App_No = r.App_No  and r.vehid = v.Veh_ID  " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += "  and FeeCategory in( '" + feecat + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";

                        SelQ += "group by Bus_RouteID ,v.veh_id,ledgerFK,a.FeeCategory ";

                        //fully paid
                        #region fully paid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f where a.App_No = r.App_No  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace   and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += "  and FeeCategory in( '" + feecat + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        SelQ += "group by a.App_No,veh_id,a.FeeCategory having sum(TotalAmount) > 0 and sum(BalAmount) =0 ";
                        #endregion

                        //partially paid
                        #region partially paid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f where a.App_No = r.App_No  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += "  and FeeCategory in( '" + feecat + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        SelQ += "group by a.App_No,veh_id,a.FeeCategory having sum(TotalAmount) <> sum(BalAmount) and sum(BalAmount) > 0";

                        #endregion

                        #region not paid
                        //not paid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f where a.App_No = r.App_No  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += "  and FeeCategory in( '" + feecat + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        SelQ += "group by a.App_No,veh_id,a.FeeCategory having sum(TotalAmount) = sum(BalAmount)";
                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region month
                        SelQ = " select bus_routeid,veh_id, COUNT(distinct a.App_No) as totstud,sum(feeamount) as feeamt,sum(DeductAmout) as concession,sum(totalAmount) as totamt,sum(a.PaidAmount) as paidamt, sum(a.BalAmount) as balamt,ledgerFK,a.FeeCategory from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        SelQ += "group by Bus_RouteID ,v.veh_id,ledgerFK,a.FeeCategory ";

                        //fully paid
                        #region fully paid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";

                        SelQ += "group by a.App_No,veh_id,a.FeeCategory having sum(TotalAmount) > 0 and sum(a.BalAmount) =0 ";

                        #endregion

                        #region partially paid
                        //partially paid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";

                        SelQ += "group by a.App_No,veh_id,a.FeeCategory having sum(TotalAmount) <> sum(a.BalAmount) and sum(a.BalAmount) > 0 ";
                        #endregion

                        #region not paid
                        //notpaid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace   and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";

                        SelQ += "group by a.App_No,veh_id,a.FeeCategory having sum(TotalAmount) = sum(a.BalAmount) ";

                        #endregion

                        #endregion
                    }
                    #endregion
                }
                //               
                SelQ = SelQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                #endregion
            }
            else if (rbstaff.Checked == true)
            {

            }
            else
            {
            }

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        bool boolCheck = false;
        if (checkSchoolSetting() != 0)
        {
            ds.Clear();
            ds = DsValues();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (rbstage.Checked == true)
                    loadStudValues();
                else if (rbroute.Checked == true)
                    loadStudRouteValues();
                boolCheck = true;
            }
        }
        else
        {
            ds.Clear();
            ds = DsValuesSchool();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (rbstage.Checked == true)
                    loadStudValuesSchool();
                else if (rbroute.Checked == true)
                    loadStudRouteValuesSchool();
                boolCheck = true;
            }
        }


        if (!boolCheck)
        {
            divspread.Visible = false;
            FpSpread1.Visible = false;
            studdet.Visible = false;
            fpstud.Visible = false;
            print.Visible = false;
            subprint.Visible = false;
            gnlcolorder.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found')", true);
            //imgdiv2.Visible = true;
            //lbl_alert.Visible = true;
            //lbl_alert.Text = "No Record Found";
        }
        //loadSpreadValues();
    }

    protected void loadStudValues()
    {
        try
        {
            #region design
            loadcolumns();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 12;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].SelectionBackColor = Color.White;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Stage";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[1].Visible = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total No Stud";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].Visible = true;
            if (!colord.Contains("1"))
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = spsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            if (!colord.Contains("2"))
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Amt";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[4].Visible = true;
            if (!colord.Contains("3"))
            {
                FpSpread1.Sheets[0].Columns[4].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[4].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Concession";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[5].Visible = true;
            if (!colord.Contains("4"))
            {
                FpSpread1.Sheets[0].Columns[5].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[5].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Allot Amt";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[6].Visible = true;
            if (!colord.Contains("5"))
            {
                FpSpread1.Sheets[0].Columns[6].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[6].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[7].Visible = true;
            if (!colord.Contains("6"))
            {
                FpSpread1.Sheets[0].Columns[7].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[7].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Balance";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[8].Visible = true;
            if (!colord.Contains("7"))
            {
                FpSpread1.Sheets[0].Columns[8].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[8].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Fully Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Tag = "-1";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[9].Visible = true;
            if (!colord.Contains("8"))
            {
                FpSpread1.Sheets[0].Columns[9].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[9].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Partially Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Tag = "-2";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[10].Visible = true;
            if (!colord.Contains("9"))
            {
                FpSpread1.Sheets[0].Columns[10].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[10].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Not Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Tag = "-3";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[11].Visible = true;
            if (!colord.Contains("10"))
            {
                FpSpread1.Sheets[0].Columns[11].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[11].Visible = true;
            }

            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            #endregion

            #region value
            Hashtable gdtot = new Hashtable();
            double feemat = 0;
            double totamt = 0;
            double consamt = 0;
            double totcnt = 0;
            double paidamt = 0;
            double balamt = 0;
            double fullypaid = 0;
            double partpaid = 0;
            double notpaid = 0;
            DataView dvpaid = new DataView();
            DataView dvpart = new DataView();
            DataView dvnot = new DataView();
            DataView Dview = new DataView();
            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[sel]["stage_name"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["boarding"]);

                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["totstud"]), out totcnt);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totcnt);
                if (!gdtot.Contains(2))
                    gdtot.Add(2, Convert.ToString(totcnt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[2]), out total);
                    total += totcnt;
                    gdtot.Remove(2);
                    gdtot.Add(2, Convert.ToString(total));
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["LedgerFK"]);
                //feecat
                string TextName = "";
                string textcode = "";
                if (ds.Tables[4].Rows.Count > 0)
                {
                    ds.Tables[4].DefaultView.RowFilter = "TextCode='" + Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    Dview = ds.Tables[4].DefaultView;
                    if (Dview.Count > 0)
                    {
                        TextName = Convert.ToString(Dview[0]["TextVal"]);
                        textcode = Convert.ToString(Dview[0]["Textcode"]);
                    }
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = TextName;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = textcode;
                //fee amount
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["feeamt"]), out feemat);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(feemat);
                if (!gdtot.Contains(4))
                    gdtot.Add(4, Convert.ToString(feemat));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[4]), out total);
                    total += feemat;
                    gdtot.Remove(4);
                    gdtot.Add(4, Convert.ToString(total));
                }

                //concession
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["concession"]), out consamt);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(consamt);
                if (!gdtot.Contains(5))
                    gdtot.Add(5, Convert.ToString(consamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[5]), out total);
                    total += consamt;
                    gdtot.Remove(5);
                    gdtot.Add(5, Convert.ToString(total));
                }

                //total amount
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["totamt"]), out totamt);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(totamt);
                if (!gdtot.Contains(6))
                    gdtot.Add(6, Convert.ToString(totamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[6]), out total);
                    total += totamt;
                    gdtot.Remove(6);
                    gdtot.Add(6, Convert.ToString(total));
                }

                //paid amount
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["paidamt"]), out paidamt);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(paidamt);
                if (!gdtot.Contains(7))
                    gdtot.Add(7, Convert.ToString(paidamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[7]), out total);
                    total += paidamt;
                    gdtot.Remove(7);
                    gdtot.Add(7, Convert.ToString(total));
                }

                //balance
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["balamt"]), out balamt);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(balamt);
                if (!gdtot.Contains(8))
                    gdtot.Add(8, Convert.ToString(balamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[8]), out total);
                    total += balamt;
                    gdtot.Remove(8);
                    gdtot.Add(8, Convert.ToString(total));
                }


                //fully paid   
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ds.Tables[1].DefaultView.RowFilter = "boarding='" + Convert.ToString(ds.Tables[0].Rows[sel]["boarding"]) + "' and FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    dvpaid = ds.Tables[1].DefaultView;
                    if (dvpaid.Count > 0)
                    {
                        double totcount = 0;
                        for (int i = 0; i < dvpaid.Count; i++)
                        {
                            double.TryParse(Convert.ToString(dvpaid[i]["cnt"]), out totcount);
                            fullypaid += totcount;
                        }

                    }
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(fullypaid);
                // FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 8].t
                if (!gdtot.Contains(9))
                    gdtot.Add(9, Convert.ToString(fullypaid));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[9]), out total);
                    total += fullypaid;
                    gdtot.Remove(9);
                    gdtot.Add(9, Convert.ToString(total));
                }
                fullypaid = 0;
                //partially paid
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ds.Tables[2].DefaultView.RowFilter = "boarding='" + Convert.ToString(ds.Tables[0].Rows[sel]["boarding"]) + "'  and FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    dvpart = ds.Tables[2].DefaultView;
                    if (dvpart.Count > 0)
                    {
                        double totcount = 0;
                        for (int i = 0; i < dvpart.Count; i++)
                        {
                            double.TryParse(Convert.ToString(dvpart[i]["cnt"]), out totcount);
                            partpaid += totcount;
                        }
                    }
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(partpaid);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Tag = -2;
                if (!gdtot.Contains(10))
                    gdtot.Add(10, Convert.ToString(partpaid));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[10]), out total);
                    total += partpaid;
                    gdtot.Remove(10);
                    gdtot.Add(10, Convert.ToString(total));
                }
                partpaid = 0;
                //not paid
                if (ds.Tables[3].Rows.Count > 0)
                {
                    ds.Tables[3].DefaultView.RowFilter = "boarding='" + Convert.ToString(ds.Tables[0].Rows[sel]["boarding"]) + "'  and FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    dvnot = ds.Tables[3].DefaultView;
                    if (dvnot.Count > 0)
                    {
                        double totcount = 0;
                        for (int i = 0; i < dvnot.Count; i++)
                        {
                            double.TryParse(Convert.ToString(dvnot[i]["cnt"]), out totcount);
                            notpaid += totcount;
                        }

                    }
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(notpaid);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Tag = -3;
                if (!gdtot.Contains(11))
                    gdtot.Add(11, Convert.ToString(notpaid));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[11]), out total);
                    total += notpaid;
                    gdtot.Remove(11);
                    gdtot.Add(11, Convert.ToString(total));
                }
                notpaid = 0;
            }

            #endregion

            #region Grandtotal
            double grandTotal = 0;
            FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.GreenYellow;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
            for (int i = 2; i < FpSpread1.Sheets[0].ColumnCount; i++)
            {
                double.TryParse(Convert.ToString(gdtot[i]), out grandTotal);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandTotal);
            }

            #endregion
            FpSpread1.Sheets[0].SelectionBackColor = Color.Green;
            // FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            gnlcolorder.Visible = true;
            divspread.Visible = true;
            FpSpread1.Visible = true;
            studdet.Visible = false;
            fpstud.Visible = false;
            lblvalidation1.Text = "";
            print.Visible = true;
            subprint.Visible = false;
        }
        catch { }
    }

    protected void loadStudRouteValues()
    {
        try
        {
            #region design
            loadcolumns();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 13;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            int check = 0;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Route Id";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vehile ID";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total No Stud";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            if (!colord.Contains("1"))
            {
                FpSpread1.Sheets[0].Columns[3].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = spsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].Visible = true;
            if (!colord.Contains("2"))
            {
                FpSpread1.Sheets[0].Columns[4].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[4].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Amt";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[5].Visible = true;
            if (!colord.Contains("3"))
            {
                FpSpread1.Sheets[0].Columns[5].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[5].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Concession";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[6].Visible = true;
            if (!colord.Contains("4"))
            {
                FpSpread1.Sheets[0].Columns[6].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[6].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Allot Amt";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[7].Visible = true;
            if (!colord.Contains("5"))
            {
                FpSpread1.Sheets[0].Columns[7].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[7].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[8].Visible = true;
            if (!colord.Contains("6"))
            {
                FpSpread1.Sheets[0].Columns[8].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[8].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Balance";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[9].Visible = true;
            if (!colord.Contains("7"))
            {
                FpSpread1.Sheets[0].Columns[9].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[9].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Fully Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Tag = "-1";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[10].Visible = true;
            if (!colord.Contains("8"))
            {
                FpSpread1.Sheets[0].Columns[10].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[10].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Partially Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Tag = "-2";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[11].Visible = true;
            if (!colord.Contains("9"))
            {
                FpSpread1.Sheets[0].Columns[11].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[11].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Not Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Tag = "-3";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[12].Visible = true;
            if (!colord.Contains("10"))
            {
                FpSpread1.Sheets[0].Columns[12].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[12].Visible = true;
            }

            #endregion

            #region value
            double feemat = 0;
            double totamt = 0;
            double consamt = 0;
            double totcnt = 0;
            double paidamt = 0;
            double balamt = 0;
            double fullypaid = 0;
            double partpaid = 0;
            double notpaid = 0;
            Hashtable gdtot = new Hashtable();
            DataView dvpaid = new DataView();
            DataView dvpart = new DataView();
            DataView dvnot = new DataView();
            DataView Dview = new DataView();
            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[sel]["bus_routeid"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["bus_routeid"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[sel]["veh_id"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["LedgerFK"]);
                //total student count
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["totstud"]), out totcnt);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(totcnt);
                if (!gdtot.Contains(3))
                    gdtot.Add(3, Convert.ToString(totcnt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[3]), out total);
                    total += totcnt;
                    gdtot.Remove(3);
                    gdtot.Add(3, Convert.ToString(total));
                }
                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]);
                //feecat
                string TextName = "";
                string textcode = "";
                if (ds.Tables[4].Rows.Count > 0)
                {
                    ds.Tables[4].DefaultView.RowFilter = "TextCode='" + Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    Dview = ds.Tables[4].DefaultView;
                    if (Dview.Count > 0)
                    {
                        TextName = Convert.ToString(Dview[0]["TextVal"]);
                        textcode = Convert.ToString(Dview[0]["Textcode"]);
                    }
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = TextName;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = textcode;

                //fee amount
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["feeamt"]), out feemat);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(feemat);
                if (!gdtot.Contains(5))
                    gdtot.Add(5, Convert.ToString(feemat));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[5]), out total);
                    total += feemat;
                    gdtot.Remove(5);
                    gdtot.Add(5, Convert.ToString(total));
                }

                //concession amount
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["concession"]), out consamt);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(consamt);
                if (!gdtot.Contains(6))
                    gdtot.Add(6, Convert.ToString(consamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[6]), out total);
                    total += consamt;
                    gdtot.Remove(6);
                    gdtot.Add(6, Convert.ToString(total));
                }

                //total amount
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["totamt"]), out totamt);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(totamt);
                if (!gdtot.Contains(7))
                    gdtot.Add(7, Convert.ToString(totamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[7]), out total);
                    total += totamt;
                    gdtot.Remove(7);
                    gdtot.Add(7, Convert.ToString(total));
                }

                //paidamt
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["paidamt"]), out paidamt);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(paidamt);
                if (!gdtot.Contains(8))
                    gdtot.Add(8, Convert.ToString(paidamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[8]), out total);
                    total += paidamt;
                    gdtot.Remove(8);
                    gdtot.Add(8, Convert.ToString(total));
                }
                //bal amt
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[sel]["balamt"]), out balamt);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(balamt);
                if (!gdtot.Contains(9))
                    gdtot.Add(9, Convert.ToString(balamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[9]), out total);
                    total += balamt;
                    gdtot.Remove(9);
                    gdtot.Add(9, Convert.ToString(total));
                }

                //fully paid   
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ds.Tables[1].DefaultView.RowFilter = "veh_id='" + Convert.ToString(ds.Tables[0].Rows[sel]["veh_id"]) + "' and FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    dvpaid = ds.Tables[1].DefaultView;
                    if (dvpaid.Count > 0)
                    {
                        double totcount = 0;
                        for (int i = 0; i < dvpaid.Count; i++)
                        {
                            double.TryParse(Convert.ToString(dvpaid[i]["cnt"]), out totcount);
                            fullypaid += totcount;
                        }
                    }
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(fullypaid);
                if (!gdtot.Contains(10))
                    gdtot.Add(10, Convert.ToString(fullypaid));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[10]), out total);
                    total += fullypaid;
                    gdtot.Remove(10);
                    gdtot.Add(10, Convert.ToString(total));
                }
                fullypaid = 0;
                //partially paid
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ds.Tables[2].DefaultView.RowFilter = "veh_id='" + Convert.ToString(ds.Tables[0].Rows[sel]["veh_id"]) + "' and FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    dvpart = ds.Tables[2].DefaultView;
                    if (dvpart.Count > 0)
                    {
                        double totcount = 0;
                        for (int i = 0; i < dvpart.Count; i++)
                        {
                            double.TryParse(Convert.ToString(dvpart[i]["cnt"]), out totcount);
                            partpaid += totcount;
                        }
                    }
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(partpaid);
                if (!gdtot.Contains(11))
                    gdtot.Add(11, Convert.ToString(partpaid));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[11]), out total);
                    total += partpaid;
                    gdtot.Remove(11);
                    gdtot.Add(11, Convert.ToString(total));
                }
                partpaid = 0;
                //not paid
                if (ds.Tables[3].Rows.Count > 0)
                {
                    ds.Tables[3].DefaultView.RowFilter = "veh_id='" + Convert.ToString(ds.Tables[0].Rows[sel]["veh_id"]) + "' and FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    dvnot = ds.Tables[3].DefaultView;
                    if (dvnot.Count > 0)
                    {
                        double totcount = 0;
                        for (int i = 0; i < dvnot.Count; i++)
                        {
                            double.TryParse(Convert.ToString(dvnot[i]["cnt"]), out totcount);
                            notpaid += totcount;
                        }
                    }
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(notpaid);
                if (!gdtot.Contains(12))
                    gdtot.Add(12, Convert.ToString(notpaid));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(gdtot[12]), out total);
                    total += notpaid;
                    gdtot.Remove(12);
                    gdtot.Add(12, Convert.ToString(total));
                }
                notpaid = 0;

            }

            #endregion
            #region Grandtotal
            double grandTotal = 0;
            FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.GreenYellow;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
            for (int i = 3; i < FpSpread1.Sheets[0].ColumnCount; i++)
            {
                double.TryParse(Convert.ToString(gdtot[i]), out grandTotal);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandTotal);
            }

            #endregion

            //  FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            gnlcolorder.Visible = true;
            divspread.Visible = true;
            FpSpread1.Visible = true;
            studdet.Visible = false;
            fpstud.Visible = false;
            lblvalidation1.Text = "";
            print.Visible = true;
            subprint.Visible = false;
        }
        catch { }
    }

    protected void FpSpread1_OnCellClick(object sender, EventArgs e)
    {
        fpcellclick = true;
    }
    protected void FpSpread1_Selectedindexchanged(object sender, EventArgs e)
    {
        if (checkSchoolSetting() != 0)
        {
            getStudentCollegeDataset();
        }
        else
        {
            getStudentSchoolDataset();
        }
    }

    protected void getStudentCollegeDataset()
    {
        try
        {
            string Id = "";
            string totalid = "";
            string totledg = "";
            string totfeecat = "";
            bool clickval = false;
            string Paiddt = "";
            string paidcolvalue = "";
            if (fpcellclick == true)
            {
                #region get value
                string feecat = "";
                string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
                string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
                //string feecat = Convert.ToString(getCblSelectedValue(cbl_sem));
                string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
                //   degcourseid = Convert.ToString(getCblSelectedValue(cbl_degree));
                string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
                string sem = Convert.ToString(getCblSelectedValue(cbl_sem));
                string stageid = Convert.ToString(getCblSelectedValue(cblstage));
                if (actrow != "" && actcol != "")
                {
                    int row = Convert.ToInt32(actrow);
                    int col = Convert.ToInt32(actcol);
                    Id = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
                    feecat = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
                    string ledger = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
                    // paidcolvalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[row, col].Tag);


                    if (row == FpSpread1.Sheets[0].Rows.Count - 1)
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
                        {
                            if (totalid == "" && totledg == "")
                            {
                                totalid = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                totledg = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                                totfeecat = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Tag);
                            }
                            else
                            {
                                totalid = totalid + "','" + "" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag) + "";
                                totledg = totledg + "','" + "" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag) + "";
                                totfeecat = totfeecat + "','" + "" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Tag) + "";
                            }
                        }
                    }
                    if (Id != "" && Id != null || totalid != "" && totalid != null)
                    {
                        if (rbstage.Checked == true)
                        {
                            if (col == 9 || paidcolvalue == "-1")
                                Paiddt = " and TotalAmount > 0 and BalAmount =0";
                            else if (col == 10 || paidcolvalue == "-2")
                                Paiddt = " and TotalAmount <> BalAmount and BalAmount > 0";
                            else if (col == 11 || paidcolvalue == "-3")
                                Paiddt = " and TotalAmount = BalAmount";

                            if (rbsem.Checked == true || rbyear.Checked == true || rball.Checked == true)
                            {
                                #region

                                string Selq = " select roll_no,roll_admit,reg_no,stud_name,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt,PaidAmount as paidamt,BalAmount balamt,FeeCategory,r.degree_code from FT_FeeAllot a,Registration r where a.App_No = r.App_No and isnull(IsCanceledStage,0)<>'1'";
                                if (Paiddt != "")
                                    Selq += Paiddt;
                                if (batch != "")
                                    Selq += " and r.Batch_year in('" + batch + "')";

                                if (degree != "")
                                    Selq += " and r.degree_code in('" + degree + "')";

                                if (totalid != "")
                                    Selq += " and Boarding in('" + totalid + "')";
                                else
                                    Selq += " and Boarding = '" + Id + "'";

                                if (totledg != "")
                                    Selq += " and LedgerFK in('" + totledg + "')";
                                else
                                    Selq += " and LedgerFK = '" + ledger + "'";

                                if (feecat != "")
                                    Selq += "and FeeCategory in( '" + feecat + "')";
                                else
                                    Selq += "and FeeCategory in( '" + totfeecat + "')";

                                Selq += " order by Roll_No,FeeCategory";
                                Selq = Selq + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(dt.dept_acronym) as Dept_Name from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in('" + collegecode + "')";
                                Selq = Selq + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                                dsstud.Clear();
                                dsstud = d2.select_method_wo_parameter(Selq, "Text");
                                if (dsstud.Tables.Count > 0 && dsstud.Tables[0].Rows.Count > 0)
                                {
                                    loadStudDetails();
                                    clickval = true;
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                if (col == 9)
                                    Paiddt = " and TotalAmount > 0 and a.BalAmount =0";
                                else if (col == 10)
                                    Paiddt = " and TotalAmount <> a.BalAmount anda.BalAmount > 0";
                                else if (col == 11)
                                    Paiddt = " and TotalAmount = a.BalAmount";

                                string ddlmnth = Convert.ToString(getCblSelectedValue(cblmonth));
                                string ddlyr = Convert.ToString(ddlyear.SelectedItem.Value);
                                // string ledger = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);

                                string Selq = " select roll_no,roll_admit,reg_no,stud_name,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt,fm.AllotAmount,fm.PaidAmount as paidamt,fm.BalAmount balamt,FeeCategory,r.degree_code,AllotMonth,AllotYear from FT_FeeAllot a,Registration r,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK   and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')  and isnull(IsCanceledStage,0)<>'1'";
                                if (Paiddt != "")
                                    Selq += Paiddt;

                                if (batch != "")
                                    Selq += " and r.Batch_year in('" + batch + "')";

                                if (degree != "")
                                    Selq += " and r.degree_code in('" + degree + "')";

                                if (totalid != "")
                                    Selq += " and Boarding in( '" + totalid + "')";
                                else
                                    Selq += " and Boarding = '" + Id + "'";

                                if (totledg != "")
                                    Selq += " and LedgerFK in( '" + totledg + "')";
                                else
                                    Selq += " and LedgerFK = '" + ledger + "'";

                                Selq += " order by AllotMonth, Roll_No,FeeCategory";
                                Selq = Selq + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(dt.dept_acronym) as Dept_Name from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in('" + collegecode + "')";
                                Selq = Selq + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                                dsstud.Clear();
                                dsstud = d2.select_method_wo_parameter(Selq, "Text");
                                if (dsstud.Tables.Count > 0 && dsstud.Tables[0].Rows.Count > 0)
                                {
                                    loadStudDetails();
                                    clickval = true;
                                }

                                #endregion
                            }
                        }
                        else if (rbroute.Checked == true)
                        {
                            if (col == 10 || paidcolvalue == "-1")
                                Paiddt = " and TotalAmount > 0 and BalAmount =0";
                            else if (col == 11 || paidcolvalue == "-2")
                                Paiddt = " and TotalAmount <> BalAmount and BalAmount > 0";
                            else if (col == 12 || paidcolvalue == "-3")
                                Paiddt = " and TotalAmount = BalAmount";
                            // feecat = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].Tag);
                            if (rbsem.Checked == true || rbyear.Checked == true || rball.Checked == true)
                            {
                                #region
                                //string ledger = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
                                string Selq = " select roll_no,roll_admit,reg_no,stud_name,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt ,PaidAmount as paidamt ,BalAmount as balamt,FeeCategory,r.degree_code from FT_FeeAllot a,Registration r where a.App_No = r.App_No and isnull(IsCanceledStage,0)<>'1' ";
                                if (Paiddt != "")
                                    Selq += Paiddt;

                                if (batch != "")
                                    Selq += " and r.Batch_year in('" + batch + "')";

                                if (degree != "")
                                    Selq += " and r.degree_code in('" + degree + "')";

                                if (totalid != "")
                                    Selq += " and Bus_RouteID in( '" + totalid + "')";
                                else
                                    Selq += " and Bus_RouteID = '" + Id + "'";

                                if (totledg != "")
                                    Selq += " and LedgerFK in( '" + totledg + "')";
                                else
                                    Selq += " and LedgerFK = '" + ledger + "'";

                                if (feecat != "")
                                    Selq += "and FeeCategory in( '" + feecat + "')";
                                else
                                    Selq += "and FeeCategory in( '" + totfeecat + "')";

                                if (stageid != "")
                                    Selq += " and r.Boarding in('" + stageid + "')";

                                Selq += " order by Roll_No,FeeCategory";

                                Selq = Selq + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(dt.dept_acronym) as Dept_Name from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in('" + collegecode + "')";
                                Selq = Selq + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                                dsstud.Clear();
                                dsstud = d2.select_method_wo_parameter(Selq, "Text");
                                if (dsstud.Tables.Count > 0 && dsstud.Tables[0].Rows.Count > 0)
                                {
                                    loadStudDetails();
                                    clickval = true;
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                if (col == 10)
                                    Paiddt = " and TotalAmount > 0 and a.BalAmount =0";
                                else if (col == 11)
                                    Paiddt = " and TotalAmount <> a.BalAmount anda.BalAmount > 0";
                                else if (col == 12)
                                    Paiddt = " and TotalAmount = a.BalAmount";
                                string ddlmnth = Convert.ToString(getCblSelectedValue(cblmonth));
                                string ddlyr = Convert.ToString(ddlyear.SelectedItem.Value);
                                //string ledger = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
                                string Selq = " select roll_no,roll_admit,reg_no,stud_name,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt ,fm.PaidAmount as paidamt ,fm.BalAmount as balamt,FeeCategory,r.degree_code,AllotMonth,AllotYear,fm.AllotAmount from FT_FeeAllot a,Registration r,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK  and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "') and isnull(IsCanceledStage,0)<>'1'";
                                if (Paiddt != "")
                                    Selq += Paiddt;

                                if (batch != "")
                                    Selq += " and r.Batch_year in('" + batch + "')";

                                if (degree != "")
                                    Selq += " and r.degree_code in('" + degree + "')";

                                if (totalid != "")
                                    Selq += " and Bus_RouteID = '" + totalid + "'";
                                else
                                    Selq += " and Bus_RouteID = '" + Id + "'";

                                if (totledg != "")
                                    Selq += " and LedgerFK in( '" + totledg + "')";
                                else
                                    Selq += " and LedgerFK = '" + ledger + "'";

                                if (stageid != "")
                                    Selq += " and r.Boarding in('" + stageid + "')";

                                Selq += " order by AllotMonth,Roll_No,a.FeeCategory";
                                Selq = Selq + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(dt.dept_acronym) as Dept_Name from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in('" + collegecode + "')";
                                Selq = Selq + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                                dsstud.Clear();
                                dsstud = d2.select_method_wo_parameter(Selq, "Text");
                                if (dsstud.Tables.Count > 0 && dsstud.Tables[0].Rows.Count > 0)
                                {
                                    loadStudDetails();
                                    clickval = true;
                                }
                                #endregion
                            }
                        }
                        if (clickval == false)
                        {

                            studdet.Visible = false;
                            fpstud.Visible = false;
                            subprint.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "No Record Found";
                        }
                    }
                }
                else
                {

                    studdet.Visible = false;
                    fpstud.Visible = false;
                    subprint.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "No Record Found";
                }
                #endregion
            }
        }
        catch { }
    }

    protected void loadStudDetails()
    {
        try
        {
            #region design
            RollAndRegSettings();
            loadcolumns();
            fpstud.Sheets[0].RowCount = 0;
            fpstud.Sheets[0].ColumnCount = 0;
            fpstud.CommandBar.Visible = false;
            fpstud.Sheets[0].AutoPostBack = true;
            fpstud.Sheets[0].ColumnHeader.RowCount = 1;
            fpstud.Sheets[0].RowHeader.Visible = false;
            fpstud.Sheets[0].ColumnCount = 14;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            fpstud.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
            //if (ViewState["colord"] != null)
            //{
            //    colsndord = (ArrayList)ViewState["sendcol"];
            //    loadsndcolumns();
            //}

            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            fpstud.Sheets[0].Columns[1].Visible = true;
            //if (!colsndord.Contains("1"))
            //    fpstud.Sheets[0].Columns[1].Visible = false;

            //if (colsndord.Count == 0)
            //    fpstud.Sheets[0].Columns[1].Visible = true;


            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            fpstud.Sheets[0].Columns[2].Visible = true;
            //if (!colsndord.Contains("2"))
            //    fpstud.Sheets[0].Columns[2].Visible = false;

            //if (colsndord.Count == 0)
            //    fpstud.Sheets[0].Columns[2].Visible = true;


            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            fpstud.Sheets[0].Columns[3].Visible = true;


            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            fpstud.Sheets[0].Columns[4].Visible = true;
            //if (!colsndord.Contains("3"))
            //    fpstud.Sheets[0].Columns[3].Visible = false;

            //if (colsndord.Count == 0)
            //    fpstud.Sheets[0].Columns[3].Visible = true;


            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Department";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            fpstud.Sheets[0].Columns[5].Visible = true;
            //if (!colsndord.Contains("4"))
            //    fpstud.Sheets[0].Columns[4].Visible = false;

            //if (colsndord.Count == 0)
            //    fpstud.Sheets[0].Columns[4].Visible = true;

            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Semester";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[6].Visible = true;
            //if (!colsndord.Contains("5"))
            //    fpstud.Sheets[0].Columns[5].Visible = false;

            //if (colsndord.Count == 0)
            //    fpstud.Sheets[0].Columns[5].Visible = true;

            if (rbmonth.Checked == true)
            {
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Month";
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                fpstud.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                fpstud.Sheets[0].Columns[7].Visible = true;
                //if (!colsndord.Contains("6"))
                //    fpstud.Sheets[0].Columns[6].Visible = false;

                //if (colsndord.Count == 0)
                //    fpstud.Sheets[0].Columns[6].Visible = true;


                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Year";
                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                fpstud.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
                fpstud.Sheets[0].Columns[8].Visible = true;
                //if (!colsndord.Contains("7"))
                //    fpstud.Sheets[0].Columns[7].Visible = false;

                //if (colsndord.Count == 0)
                //    fpstud.Sheets[0].Columns[7].Visible = true;
            }

            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Total Amt";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[9].Visible = true;
            if (!colord.Contains("3"))
                fpstud.Sheets[0].Columns[9].Visible = false;

            if (colord.Count == 0)
                fpstud.Sheets[0].Columns[9].Visible = true;


            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Concession";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
            fpstud.Sheets[0].Columns[10].Visible = true;
            if (!colord.Contains("4"))
                fpstud.Sheets[0].Columns[10].Visible = false;

            if (colord.Count == 0)
                fpstud.Sheets[0].Columns[10].Visible = true;


            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Allot Amt";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
            fpstud.Sheets[0].Columns[11].Visible = true;
            if (!colord.Contains("5"))
                fpstud.Sheets[0].Columns[11].Visible = false;

            if (colord.Count == 0)
                fpstud.Sheets[0].Columns[11].Visible = true;



            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Paid";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Right;
            fpstud.Sheets[0].Columns[12].Visible = true;
            if (!colord.Contains("6"))
                fpstud.Sheets[0].Columns[12].Visible = false;

            if (colord.Count == 0)
                fpstud.Sheets[0].Columns[12].Visible = true;

            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Balance";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Right;
            fpstud.Sheets[0].Columns[13].Visible = true;
            if (!colord.Contains("7"))
                fpstud.Sheets[0].Columns[13].Visible = false;

            if (colord.Count == 0)
                fpstud.Sheets[0].Columns[13].Visible = true;

            //if (roll == 0)
            //{
            //    fpstud.Sheets[0].Columns[1].Visible = true;
            //    fpstud.Sheets[0].Columns[2].Visible = true;
            //}
            //else if (roll == 1)
            //{
            //    fpstud.Sheets[0].Columns[1].Visible = true;
            //    fpstud.Sheets[0].Columns[2].Visible = true;
            //}
            //else if (roll == 2)
            //{
            //    fpstud.Sheets[0].Columns[1].Visible = true;
            //    fpstud.Sheets[0].Columns[2].Visible = false;
            //}
            //else if (roll == 3)
            //{
            //    fpstud.Sheets[0].Columns[1].Visible = false;
            //    fpstud.Sheets[0].Columns[2].Visible = true;
            //}
            fpstud.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpstud.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpstud.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpstud.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpstud.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpstud.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            // fpstud.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            // FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            spreadColumnVisible();
            #endregion

            #region value
            DataView Dview = new DataView();
            DataView fee = new DataView();
            double totamt = 0;
            double feemat = 0;
            double consamt = 0;
            double totcnt = 0;
            double paidamt = 0;
            double balamt = 0;
            ArrayList arsno = new ArrayList();
            int sno = 0;
            Hashtable grandtot = new Hashtable();
            for (int sel = 0; sel < dsstud.Tables[0].Rows.Count; sel++)
            {
                fpstud.Sheets[0].RowCount++;
                if (!arsno.Contains(Convert.ToString(dsstud.Tables[0].Rows[sel]["roll_no"])))
                {
                    arsno.Add(Convert.ToString(dsstud.Tables[0].Rows[sel]["roll_no"]));
                    sno++;
                }
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsstud.Tables[0].Rows[sel]["roll_no"]);
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsstud.Tables[0].Rows[sel]["reg_no"]);
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsstud.Tables[0].Rows[sel]["reg_no"]);

                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 1].CellType = txtroll;
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 2].CellType = txtreg;


                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsstud.Tables[0].Rows[sel]["stud_name"]);
                string Degreename = "";
                if (dsstud.Tables[1].Rows.Count > 0)
                {
                    dsstud.Tables[1].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dsstud.Tables[0].Rows[sel]["Degree_code"]) + "'";
                    Dview = dsstud.Tables[1].DefaultView;
                    if (Dview.Count > 0)
                    {
                        if (cbdeptacr.Checked == true)
                            Degreename = Convert.ToString(Dview[0]["Dept_Name"]);
                        else
                            Degreename = Convert.ToString(Dview[0]["degreename"]);
                    }
                }
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 5].Text = Degreename;
                string TextName = "";
                if (dsstud.Tables[2].Rows.Count > 0)
                {
                    dsstud.Tables[2].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dsstud.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                    fee = dsstud.Tables[2].DefaultView;
                    if (fee.Count > 0)
                    {
                        TextName = Convert.ToString(fee[0]["TextVal"]);
                    }
                }
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 6].Text = TextName;
                if (rbmonth.Checked == true)
                {
                    string mnthname = textMonth(Convert.ToString(dsstud.Tables[0].Rows[sel]["AllotMonth"]));
                    fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 7].Text = mnthname;
                    fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dsstud.Tables[0].Rows[sel]["AllotYear"]);
                    double.TryParse(Convert.ToString(dsstud.Tables[0].Rows[sel]["AllotAmount"]), out totamt);
                    fpstud.Sheets[0].Columns[7].Visible = true;
                    fpstud.Sheets[0].Columns[8].Visible = true;
                }
                else
                {
                    fpstud.Sheets[0].Columns[7].Visible = false;
                    fpstud.Sheets[0].Columns[8].Visible = false;
                    double.TryParse(Convert.ToString(dsstud.Tables[0].Rows[sel]["totamt"]), out totamt);
                }

                //fee amount
                double.TryParse(Convert.ToString(dsstud.Tables[0].Rows[sel]["feeamt"]), out feemat);
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(feemat);
                if (!grandtot.Contains(9))
                    grandtot.Add(9, Convert.ToString(feemat));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(grandtot[9]), out total);
                    total += feemat;
                    grandtot.Remove(9);
                    grandtot.Add(9, Convert.ToString(total));
                }

                //concession
                double.TryParse(Convert.ToString(dsstud.Tables[0].Rows[sel]["concession"]), out consamt);
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(consamt);
                if (!grandtot.Contains(10))
                    grandtot.Add(10, Convert.ToString(consamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(grandtot[10]), out total);
                    total += consamt;
                    grandtot.Remove(10);
                    grandtot.Add(10, Convert.ToString(total));
                }

                //paidamt               
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(totamt);
                if (!grandtot.Contains(11))
                    grandtot.Add(11, Convert.ToString(totamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(grandtot[11]), out total);
                    total += totamt;
                    grandtot.Remove(11);
                    grandtot.Add(11, Convert.ToString(total));
                }

                //paid amount
                double.TryParse(Convert.ToString(dsstud.Tables[0].Rows[sel]["paidamt"]), out paidamt);
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(paidamt);
                if (!grandtot.Contains(12))
                    grandtot.Add(12, Convert.ToString(paidamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(grandtot[12]), out total);
                    total += paidamt;
                    grandtot.Remove(12);
                    grandtot.Add(12, Convert.ToString(total));
                }

                //balance amt
                double.TryParse(Convert.ToString(dsstud.Tables[0].Rows[sel]["balamt"]), out balamt);
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(balamt);
                if (!grandtot.Contains(13))
                    grandtot.Add(13, Convert.ToString(balamt));
                else
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(grandtot[13]), out total);
                    total += balamt;
                    grandtot.Remove(13);
                    grandtot.Add(13, Convert.ToString(total));
                }
            }

            #endregion
            #region Grand total
            double grandTotal = 0;
            fpstud.Sheets[0].PageSize = dsstud.Tables[0].Rows.Count + 1;
            fpstud.Sheets[0].RowCount++;
            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            fpstud.Sheets[0].Rows[fpstud.Sheets[0].RowCount - 1].BackColor = Color.Gold;
            fpstud.Sheets[0].SpanModel.Add(fpstud.Sheets[0].RowCount - 1, 0, 1, 2);
            for (int i = 9; i < fpstud.Sheets[0].ColumnCount; i++)
            {
                double.TryParse(Convert.ToString(grandtot[i]), out grandTotal);
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandTotal);
            }

            #endregion

            //fpstud.Sheets[0].PageSize = fpstud.Sheets[0].RowCount;
            fpstud.SaveChanges();
            studdet.Visible = true;
            fpstud.Visible = true;
            subprint.Visible = true;
        }
        catch { }
    }
    #endregion

    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                if (rbstage.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your StageWise Report Name";
                }
                else if (rbroute.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your RouteWise Report Name";
                }
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Travel Allot Report";
            pagename = "TravelAllotReport.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    protected void btnsubexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtsub.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(fpstud, reportname);
                lblprint.Visible = false;
            }
            else
            {
                if (rbstage.Checked == true)
                {
                    lblprint.Text = "Please Enter Your StageWise Student Report Name";
                }
                else if (rbroute.Checked == true)
                {
                    lblprint.Text = "Please Enter Your RouteWise Student Report Name";
                }
                lblprint.Visible = true;
                txtsub.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintm_Click(object sender, EventArgs e)
    {
        try
        {
            lblprint.Text = "";
            txtsub.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Student Details";
            pagename = "TravelAllotReport.aspx";
            Printmaster1.loadspreaddetails(fpstud, pagename, degreedetails);
            Printmaster1.Visible = true;
        }
        catch { }
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

    protected string textMonth(string mon)
    {
        string txt = "";
        try
        {
            switch (mon)
            {
                case "1":
                    txt = "JAN";
                    break;
                case "2":
                    txt = "FEB";
                    break;
                case "3":
                    txt = "MAR";
                    break;
                case "4":
                    txt = "APR";
                    break;
                case "5":
                    txt = "MAY";
                    break;
                case "6":
                    txt = "JUN";
                    break;
                case "7":
                    txt = "JUL";
                    break;
                case "8":
                    txt = "AUG";
                    break;
                case "9":
                    txt = "SEP";
                    break;
                case "10":
                    txt = "OCT";
                    break;
                case "11":
                    txt = "NOV";
                    break;
                case "12":
                    txt = "DEC";
                    break;
                default:
                    txt = "Month Not Available";
                    break;
            }
        }
        catch { }
        return txt;
    }

    #region Column order

    protected void loadcolorder()
    {
        cblcolorder.Items.Clear();
        //if (rbstage.Checked == true)
        //{
        cblcolorder.Items.Add(new ListItem("Total No Stud", "1"));
        cblcolorder.Items.Add(new ListItem(spsem.Text, "2"));
        cblcolorder.Items.Add(new ListItem("Total Amount", "3"));
        cblcolorder.Items.Add(new ListItem("Concession ", "4"));
        cblcolorder.Items.Add(new ListItem("Allot", "5"));
        cblcolorder.Items.Add(new ListItem("Paid", "6"));
        cblcolorder.Items.Add(new ListItem("Balance", "7"));
        cblcolorder.Items.Add(new ListItem("Fully Paid", "8"));
        cblcolorder.Items.Add(new ListItem("partially Paid", "9"));
        cblcolorder.Items.Add(new ListItem("Not Paid", "10"));
        //}
        //else
        //{

        //}
    }

    protected void cbcolorder_Changed(object sender, EventArgs e)
    {
        if (cbcolorder.Checked == true)
        {
            for (int i = 0; i < cblcolorder.Items.Count; i++)
            {
                cblcolorder.Items[i].Selected = true;
            }
        }
        else
        {

            for (int i = 0; i < cblcolorder.Items.Count; i++)
            {
                cblcolorder.Items[i].Selected = false;
            }
        }
    }

    public bool columncount()
    {
        bool colorder = false;
        try
        {
            for (int i = 0; i < cblcolorder.Items.Count; i++)
            {
                if (cblcolorder.Items[i].Selected == true)
                {
                    colorder = true;
                }
            }
        }
        catch { }
        return colorder;
    }

    public void loadcolumns()
    {
        try
        {
            string linkname = "Travel Allot Report column order settings";
            string columnvalue = "";
            int clsupdate = 0;
            //  string collegecode1 = ddlcollege.SelectedItem.Value.ToString();
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code in('" + collegecode + "') ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (columncount() == true)
            {
                if (cblcolorder.Items.Count > 0)
                {
                    colord.Clear();
                    for (int i = 0; i < cblcolorder.Items.Count; i++)
                    {
                        if (cblcolorder.Items[i].Selected == true)
                        {
                            colord.Add(Convert.ToString(cblcolorder.Items[i].Value));
                            if (columnvalue == "")
                            {
                                columnvalue = Convert.ToString(cblcolorder.Items[i].Value);
                            }
                            else
                            {
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolorder.Items[i].Value);
                            }
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    colord.Clear();
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colord.Add(Convert.ToString(valuesplit[k]));
                                if (columnvalue == "")
                                {
                                    columnvalue = Convert.ToString(valuesplit[k]);
                                }
                                else
                                {
                                    columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                colord.Clear();
                for (int i = 0; i < cblcolorder.Items.Count; i++)
                {
                    cblcolorder.Items[i].Selected = true;
                    colord.Add(Convert.ToString(cblcolorder.Items[i].Value));
                    if (columnvalue == "")
                    {
                        columnvalue = Convert.ToString(cblcolorder.Items[i].Value);
                    }
                    else
                    {
                        columnvalue = columnvalue + ',' + Convert.ToString(cblcolorder.Items[i].Value);
                    }
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code in('" + collegecode + "') else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code in('" + collegecode + "') ";
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = d2.select_method_wo_parameter(sel, "Text");
                if (dscolor.Tables.Count > 0)
                {
                    int count = 0;
                    if (dscolor.Tables[0].Rows.Count > 0)
                    {
                        string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
                        string[] value1 = value.Split(',');
                        if (value1.Length > 0)
                        {
                            for (int i = 0; i < value1.Length; i++)
                            {
                                string val = value1[i].ToString();
                                for (int k = 0; k < cblcolorder.Items.Count; k++)
                                {
                                    if (val == cblcolorder.Items[k].Value)
                                    {
                                        cblcolorder.Items[k].Selected = true;
                                        count++;
                                    }
                                    if (count == cblcolorder.Items.Count)
                                    {
                                        cbcolorder.Checked = true;
                                    }
                                    else
                                    {
                                        cbcolorder.Checked = false;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }
        catch { }
    }

    #region old
    //protected void loadsecondcolorder()
    //{
    //    cblsndcolorder.Items.Clear();
    //    //if (rbstage.Checked == true)
    //    //{
    //    cblsndcolorder.Items.Add(new ListItem("Roll No", "1"));
    //    cblsndcolorder.Items.Add(new ListItem("Reg No", "2"));
    //    cblsndcolorder.Items.Add(new ListItem("Student Name ", "3"));
    //    cblsndcolorder.Items.Add(new ListItem("Department", "4"));
    //    cblsndcolorder.Items.Add(new ListItem("Semester", "5"));
    //    if (rbmonth.Checked == true)
    //    {
    //        cblsndcolorder.Items.Add(new ListItem("Department", "6"));
    //        cblsndcolorder.Items.Add(new ListItem("Semester", "7"));
    //    }
    //    cblsndcolorder.Items.Add(new ListItem("Total Amount", "8"));
    //    cblsndcolorder.Items.Add(new ListItem("Concession ", "9"));
    //    cblsndcolorder.Items.Add(new ListItem("Allot", "10"));
    //    cblsndcolorder.Items.Add(new ListItem("Paid", "11"));
    //    cblsndcolorder.Items.Add(new ListItem("Balance", "12"));
    //    //}
    //    //else
    //    //{

    //    //}
    //}

    //protected void cbsndcolorder_Changed(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string colname12 = "";
    //        if (ViewState["ItemList"] != null)
    //            ItemList = (ArrayList)ViewState["ItemList"];

    //        if (ViewState["Itemindex"] != null)
    //            Itemindex = (ArrayList)ViewState["Itemindex"];

    //        if (ViewState["sendcol"] != null)
    //            sendcol = (ArrayList)ViewState["sendcol"];


    //        if (cbsndcolorder.Checked == true)
    //        {
    //            for (int sel = 0; sel < cblsndcolorder.Items.Count; sel++)
    //            {
    //                cblsndcolorder.Items[sel].Selected = true;
    //                if (!sendcol.Contains(Convert.ToString(cblsndcolorder.Items[sel].Value)))
    //                {
    //                    sendcol.Add(Convert.ToString(cblsndcolorder.Items[sel].Value));
    //                }
    //                if (!Itemindex.Contains(sel))
    //                {
    //                    ItemList.Add(cblsndcolorder.Items[sel].Text.ToString());
    //                    Itemindex.Add(sel);
    //                }
    //            }
    //            for (int i = 0; i < ItemList.Count; i++)
    //            {
    //                if (colname12 == "")
    //                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";

    //                else
    //                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
    //            }
    //        }
    //        else
    //        {
    //            for (int sel = 0; sel < cblsndcolorder.Items.Count; sel++)
    //            {
    //                cblsndcolorder.Items[sel].Selected = false;
    //                ItemList.Remove(cblsndcolorder.Items[sel].Value.ToString());
    //                Itemindex.Remove(sel);
    //            }
    //            for (int i = 0; i < ItemList.Count; i++)
    //            {
    //                if (colname12 == "")
    //                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";

    //                else
    //                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
    //            }
    //        }
    //        txtsndcolorder.Text = colname12;
    //        ViewState["sendcol"] = sendcol;
    //        ViewState["ItemList"] = ItemList;
    //        ViewState["Itemindex"] = Itemindex;
    //    }
    //    catch { }

    //}
    //protected void cblsndcolorder_Selected(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ViewState["ItemList"] != null)
    //            ItemList = (ArrayList)ViewState["ItemList"];

    //        if (ViewState["Itemindex"] != null)
    //            Itemindex = (ArrayList)ViewState["Itemindex"];

    //        if (ViewState["sendcol"] != null)
    //            sendcol = (ArrayList)ViewState["sendcol"];


    //        string colname12 = "";
    //        string result = Request.Form["__EVENTTARGET"];
    //        string[] checkedBox = result.Split('$');
    //        int index = int.Parse(checkedBox[checkedBox.Length - 1]);
    //        string sindex = Convert.ToString(index);
    //        // int count = DictC.Count;
    //        for (int sel = 0; sel < cblsndcolorder.Items.Count; sel++)
    //        {
    //            // count = sel;            
    //            if (cblsndcolorder.Items[sel].Selected == true)
    //            {
    //                if (!sendcol.Contains(Convert.ToString(cblsndcolorder.Items[sel].Value)))
    //                {
    //                    sendcol.Add(Convert.ToString(cblsndcolorder.Items[sel].Value));
    //                }
    //            }
    //            if (cblsndcolorder.Items[index].Selected)
    //            {
    //                if (!Itemindex.Contains(sindex))
    //                {
    //                    ItemList.Add(cblsndcolorder.Items[index].Text.ToString());
    //                    Itemindex.Add(sindex);
    //                }
    //            }
    //            else
    //            {
    //                ItemList.Remove(cblsndcolorder.Items[index].Text.ToString());
    //                Itemindex.Remove(sindex);
    //            }
    //        }
    //        for (int i = 0; i < cblsndcolorder.Items.Count; i++)
    //        {
    //            if (cblsndcolorder.Items[i].Selected == false)
    //            {
    //                sindex = Convert.ToString(i);
    //                ItemList.Remove(cblsndcolorder.Items[i].Text.ToString());
    //                Itemindex.Remove(sindex);
    //            }
    //        }

    //        for (int i = 0; i < ItemList.Count; i++)
    //        {
    //            if (colname12 == "")
    //                colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";

    //            else
    //                colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
    //        }
    //        txtsndcolorder.Text = colname12;
    //        //// ViewState["dict"] = DictC;
    //        ViewState["ItemList"] = ItemList;
    //        ViewState["Itemindex"] = Itemindex;
    //        ViewState["sendcol"] = sendcol;
    //    }
    //    catch { }
    //}

    //public bool columnsndcount()
    //{
    //    bool colorder = false;
    //    try
    //    {
    //        for (int i = 0; i < cblsndcolorder.Items.Count; i++)
    //        {
    //            if (cblsndcolorder.Items[i].Selected == true)
    //            {
    //                colorder = true;
    //            }
    //        }
    //    }
    //    catch { }
    //    return colorder;
    //}

    //public void loadsndcolumns()
    //{
    //    try
    //    {
    //        string linkname = "Travel Allot Report Student Details column order settings";
    //        string columnvalue = "";
    //        int clsupdate = 0;
    //        string collegecode1 = ddlcollege.SelectedItem.Value.ToString();
    //        DataSet dscol = new DataSet();
    //        string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "' ";
    //        dscol.Clear();
    //        dscol = d2.select_method_wo_parameter(selcol, "Text");
    //        if (columnsndcount() == true)
    //        {
    //            if (cblsndcolorder.Items.Count > 0)
    //            {
    //                colsndord.Clear();
    //                for (int i = 0; i < cblsndcolorder.Items.Count; i++)
    //                {
    //                    if (cblsndcolorder.Items[i].Selected == true)
    //                    {
    //                        colsndord.Add(Convert.ToString(cblsndcolorder.Items[i].Value));
    //                        if (columnvalue == "")
    //                        {
    //                            columnvalue = Convert.ToString(cblsndcolorder.Items[i].Value);
    //                        }
    //                        else
    //                        {
    //                            columnvalue = columnvalue + ',' + Convert.ToString(cblsndcolorder.Items[i].Value);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        else if (dscol.Tables.Count > 0)
    //        {
    //            if (dscol.Tables[0].Rows.Count > 0)
    //            {
    //                colsndord.Clear();
    //                for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
    //                {
    //                    string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
    //                    string[] valuesplit = value.Split(',');
    //                    if (valuesplit.Length > 0)
    //                    {
    //                        for (int k = 0; k < valuesplit.Length; k++)
    //                        {
    //                            colsndord.Add(Convert.ToString(valuesplit[k]));
    //                            if (columnvalue == "")
    //                            {
    //                                columnvalue = Convert.ToString(valuesplit[k]);
    //                            }
    //                            else
    //                            {
    //                                columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
    //                            }
    //                        }
    //                    }
    //                }
    //            }

    //        }
    //        else
    //        {
    //            colsndord.Clear();
    //            for (int i = 0; i < cblsndcolorder.Items.Count; i++)
    //            {
    //                cblsndcolorder.Items[i].Selected = true;
    //                colsndord.Add(Convert.ToString(cblsndcolorder.Items[i].Value));
    //                if (columnvalue == "")
    //                {
    //                    columnvalue = Convert.ToString(cblsndcolorder.Items[i].Value);
    //                }
    //                else
    //                {
    //                    columnvalue = columnvalue + ',' + Convert.ToString(cblsndcolorder.Items[i].Value);
    //                }
    //            }
    //        }
    //        if (columnvalue != "" && columnvalue != null)
    //        {
    //            string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode1 + "')";
    //            clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
    //        }
    //        if (clsupdate == 1)
    //        {
    //            string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "' ";
    //            DataSet dscolor = new DataSet();
    //            dscolor.Clear();
    //            dscolor = d2.select_method_wo_parameter(sel, "Text");
    //            if (dscolor.Tables.Count > 0)
    //            {
    //                int count = 0;
    //                if (dscolor.Tables[0].Rows.Count > 0)
    //                {
    //                    string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
    //                    string[] value1 = value.Split(',');
    //                    if (value1.Length > 0)
    //                    {
    //                        for (int i = 0; i < value1.Length; i++)
    //                        {
    //                            string val = value1[i].ToString();
    //                            for (int k = 0; k < cblsndcolorder.Items.Count; k++)
    //                            {
    //                                if (val == cblsndcolorder.Items[k].Value)
    //                                {
    //                                    cblsndcolorder.Items[k].Selected = true;
    //                                    count++;
    //                                }
    //                                if (count == cblsndcolorder.Items.Count)
    //                                {
    //                                    cbsndcolorder.Checked = true;
    //                                }
    //                                else
    //                                {
    //                                    cbsndcolorder.Checked = false;
    //                                }
    //                            }
    //                        }
    //                    }

    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}

    //protected void btnsndgo_Click(object sender, EventArgs e)
    //{
    //   // loadStudDetails();
    //}
    #endregion
    #endregion

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

        lbl.Add(lblclg);
        lbl.Add(lblstr);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(spsem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    #region roll,reg,admission setting
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }

    protected void spreadColumnVisible()
    {
        try
        {
            if (roll == 0)
            {
                fpstud.Columns[1].Visible = true;
                fpstud.Columns[2].Visible = true;
                fpstud.Columns[3].Visible = true;
            }
            else if (roll == 1)
            {
                fpstud.Columns[1].Visible = true;
                fpstud.Columns[2].Visible = true;
                fpstud.Columns[3].Visible = true;
            }
            else if (roll == 2)
            {
                fpstud.Columns[1].Visible = true;
                fpstud.Columns[2].Visible = false;
                fpstud.Columns[3].Visible = false;

            }
            else if (roll == 3)
            {
                fpstud.Columns[1].Visible = false;
                fpstud.Columns[2].Visible = true;
                fpstud.Columns[3].Visible = false;
            }
            else if (roll == 4)
            {
                fpstud.Columns[1].Visible = false;
                fpstud.Columns[2].Visible = false;
                fpstud.Columns[3].Visible = true;
            }
            else if (roll == 5)
            {
                fpstud.Columns[1].Visible = true;
                fpstud.Columns[2].Visible = true;
                fpstud.Columns[3].Visible = false;
            }
            else if (roll == 6)
            {
                fpstud.Columns[1].Visible = false;
                fpstud.Columns[2].Visible = true;
                fpstud.Columns[3].Visible = true;
            }
            else if (roll == 7)
            {
                fpstud.Columns[1].Visible = true;
                fpstud.Columns[2].Visible = false;
                fpstud.Columns[3].Visible = true;
            }
        }
        catch { }
    }

    #endregion

    // last modified 24.052017 sudhagar for velammal school
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
    protected void getSchoolDetails(object sender, EventArgs e)
    {
        if (checkSchoolSetting() == 0)
        {
            rbterm.Visible = true;
            rbterm.Checked = true;
            rbsem.Visible = false;
            rbsem.Checked = false;
            rbyear.Visible = false;
            rball.Visible = false;
            fldmain.Attributes.Add("Style", "height: 10px; width: 249px;");
            fldtype.Attributes.Add("Style", "height: 10px; width: 59px;");
            //tblmain.Attributes.Add("Style", "top: 231px; position: absolute;");
            tdlblfin.Visible = true;
            tdfltfin.Visible = true;
            loadfinanceyear();
        }
        else
        {
            rbterm.Visible = false;
            rbterm.Checked = false;
            rbsem.Checked = true;
            rbsem.Visible = true;
            rbyear.Visible = true;
            rball.Visible = true;
            rbsem_Changed(sender, e);
            tdlblfin.Visible = false;
            tdfltfin.Visible = false;
        }
    }
    #region financial year
    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode + "'  order by FinYearPK desc";
            ds.Dispose();
            ds.Reset();
            chkfyear.Checked = false;
            chklsfyear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }

                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = true;
                    fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
                }
                if (chklsfyear.Items.Count == 1)
                {
                    txtfyear.Text = "" + fnalyr + "";
                }
                else
                {
                    txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
                }
                // txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                chkfyear.Checked = true;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void chklsfyear_selected(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");

    }
    protected void chkfyear_changed(object sender, EventArgs e)
    {
        CallCheckboxChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
    }
    #endregion

    protected DataSet DsValuesSchool()
    {
        DataSet dsload = new DataSet();
        try
        {
            int method = 0;
            int typename = 0;
            string type = "";
            string SelQ = "";
            // string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            //   degcourseid = Convert.ToString(getCblSelectedValue(cbl_degree));
            string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            string sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            //string header = Convert.ToString(ddlheader.SelectedItem.Value);
            //string ledger = Convert.ToString(ddlledger.SelectedItem.Value);
            string header = Convert.ToString(getCblSelectedValue(chkl_studhed));
            string ledger = Convert.ToString(getCblSelectedValue(chkl_studled));
            string fnlYr = Convert.ToString(getCblSelectedValue(chklsfyear));
            string routeid = Convert.ToString(getCblSelectedValue(cblroute));
            string vechileid = Convert.ToString(getCblSelectedValue(cblvechile));
            string stageid = Convert.ToString(getCblSelectedValue(cblstage));
            string stafdesg = Convert.ToString(getCblSelectedValue(cbldesg));
            string stafdept = Convert.ToString(getCblSelectedValue(cblstafdept));
            //type
            string feecat = Convert.ToString(getCblSelectedValue(cbl_sem));
            //string ddlmnth = Convert.ToString(ddlmonth.SelectedItem.Value);
            string ddlmnth = Convert.ToString(getCblSelectedValue(cblmonth));
            string ddlyr = Convert.ToString(ddlyear.SelectedItem.Value);
            string vehiType = Convert.ToString(getCblSelectedValue(cblvehtype));
            if (rbstage.Checked == true)
                typename = 5;
            else
                typename = 6;
            if (rbterm.Checked == true)
            {
                type = "Term";
                ddlmnth = "";
                ddlyr = "";
            }
            else
            {
                type = "Monthly";
                feecat = "";
            }
            string cancelStr = string.Empty;
            if (!cbcancel.Checked)
                cancelStr = " and isnull(IsCanceledStage,0)<>'1'";
            if (rbstud.Checked == true)
            {
                #region stud
                if (typename == 5)
                {
                    #region stage
                    if (type == "Term")
                    {
                        #region Term
                        SelQ = "select boarding,stage_name,COUNT(distinct r.app_no) as totstud,sum(feeamount)as feeamt,sum(DeductAmout)as concession,sum(TotalAmount)as totamt,sum(PaidAmount)as paidamt,sum(BalAmount)as balamt,ledgerFK,a.FeeCategory,a.finyearfk from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f where a.App_No = r.App_No and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace " + cancelStr + " and payType in('" + type + "')";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += " and a.FeeCategory in('" + feecat + "')";
                        else
                            SelQ += "";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";

                        SelQ += "group by Boarding,Stage_Name ,ledgerFK,a.FeeCategory,a.finyearfk order by Stage_name ";

                        //fully paid
                        #region fully paid
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory,a.finyearfk from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f where a.App_No = r.App_No and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace   and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += " and a.FeeCategory in('" + feecat + "')";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";
                        SelQ += "group by a.App_No,Boarding,a.FeeCategory,a.finyearfk having sum(TotalAmount) > 0 and sum(BalAmount) =0 ";

                        #endregion

                        #region partially paid
                        //partially paid
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory,a.finyearfk from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f where a.App_No = r.App_No and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += " and a.FeeCategory in('" + feecat + "')";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";
                        SelQ += "group by a.App_No,Boarding,a.FeeCategory,a.finyearfk having sum(TotalAmount) <> sum(BalAmount) and sum(BalAmount) > 0 ";
                        #endregion

                        #region not paid

                        //not paid
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory,a.finyearfk from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f where a.App_No = r.App_No and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += " and a.FeeCategory in('" + feecat + "')";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";
                        SelQ += "group by a.App_No,Boarding,a.FeeCategory,a.finyearfk having sum(TotalAmount) = sum(BalAmount)";
                        #endregion



                        #endregion
                    }
                    else
                    {
                        #region month
                        SelQ = "select boarding,stage_name,COUNT(distinct r.app_no) as totstud,sum(feeamount)as feeamt,sum(DeductAmout)as concession,sum(TotalAmount)as totamt,sum(a.PaidAmount)as paidamt,sum(a.BalAmount)as balamt,ledgerFK,a.FeeCategory,a.finyearfk from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by Boarding,Stage_Name,ledgerFK,a.FeeCategory,a.finyearfk order by Stage_name ";

                        //fully paid
                        #region fully paid
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory,a.finyearfk from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by a.App_No,Boarding,a.FeeCategory,a.finyearfk having sum(TotalAmount) > 0 and sum(a.BalAmount) =0 ";
                        #endregion

                        #region partially paid
                        //partially paid
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory,a.finyearfk from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by a.App_No,Boarding,a.FeeCategory,a.finyearfk having sum(TotalAmount) <> sum(a.BalAmount) and sum(a.BalAmount) > 0 ";
                        #endregion

                        #region not paid
                        //not paid                       
                        SelQ += "select Count(distinct a.app_no) as cnt,Boarding,a.FeeCategory,a.finyearfk from FT_FeeAllot a,Registration r,Stage_Master s,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK and str(r.Boarding) = str(s.Stage_id ) and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (stageid != "")
                            SelQ += " and Stage_id in('" + stageid + "')";
                        if (header != "")
                            SelQ += " and a.HeaderFK in('" + header + "')";
                        if (ledger != "")
                            SelQ += " and a.LedgerFK in('" + ledger + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by a.App_No,Boarding,a.FeeCategory,a.finyearfk having sum(TotalAmount) = sum(a.BalAmount) ";
                        #endregion

                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region route
                    if (type == "Term")
                    {
                        #region Term
                        SelQ = " select bus_routeid,veh_id, COUNT(distinct a.App_No) as totstud,sum(feeamount) as feeamt,sum(DeductAmout) as concession,sum(totalAmount) as totamt,sum(PaidAmount) as paidamt, sum(BalAmount) as balamt,ledgerFK,a.FeeCategory,a.finyearfk  from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f where a.App_No = r.App_No  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += "  and FeeCategory in( '" + feecat + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by Bus_RouteID ,v.veh_id,ledgerFK,a.FeeCategory,a.finyearfk  ";

                        //fully paid
                        #region fully paid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory,a.finyearfk  from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f where a.App_No = r.App_No  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace   and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += "  and FeeCategory in( '" + feecat + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by a.App_No,veh_id,a.FeeCategory,a.finyearfk  having sum(TotalAmount) > 0 and sum(BalAmount) =0 ";
                        #endregion

                        //partially paid
                        #region partially paid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory,a.finyearfk  from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f where a.App_No = r.App_No  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += "  and FeeCategory in( '" + feecat + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by a.App_No,veh_id,a.FeeCategory,a.finyearfk  having sum(TotalAmount) <> sum(BalAmount) and sum(BalAmount) > 0";

                        #endregion

                        #region not paid
                        //not paid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory,a.finyearfk  from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f where a.App_No = r.App_No  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (feecat != "")
                            SelQ += "  and FeeCategory in( '" + feecat + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by a.App_No,veh_id,a.FeeCategory,a.finyearfk  having sum(TotalAmount) = sum(BalAmount)";
                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region month
                        SelQ = " select bus_routeid,veh_id, COUNT(distinct a.App_No) as totstud,sum(feeamount) as feeamt,sum(DeductAmout) as concession,sum(totalAmount) as totamt,sum(a.PaidAmount) as paidamt, sum(a.BalAmount) as balamt,ledgerFK,a.FeeCategory,a.finyearfk  from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by Bus_RouteID ,v.veh_id,ledgerFK,a.FeeCategory,a.finyearfk  ";

                        //fully paid
                        #region fully paid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory,a.finyearfk  from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by a.App_No,veh_id,a.FeeCategory,a.finyearfk  having sum(TotalAmount) > 0 and sum(a.BalAmount) =0 ";

                        #endregion

                        #region partially paid
                        //partially paid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory,a.finyearfk  from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace  and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by a.App_No,veh_id,a.FeeCategory,a.finyearfk  having sum(TotalAmount) <> sum(a.BalAmount) and sum(a.BalAmount) > 0 ";
                        #endregion

                        #region not paid
                        //notpaid
                        SelQ += " select Count(distinct a.app_no) as cnt,veh_id,a.FeeCategory,a.finyearfk  from FT_FeeAllot a,Registration r,Vehicle_Master v,FeeInfo f,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK  and r.vehid = v.Veh_ID and r.Boarding=StrtPlace   and payType in('" + type + "') " + cancelStr + "";
                        // and v.Route=f.StrtPlace
                        if (batch != "")
                            SelQ += " and r.Batch_year in('" + batch + "')";
                        if (degree != "")
                            SelQ += " and r.degree_code in('" + degree + "')";
                        if (ddlmnth != "" && ddlyr != "")
                            SelQ += " and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')";
                        if (vechileid != "")
                            SelQ += " and Veh_ID in('" + vechileid + "')";
                        if (ledger != "")
                            SelQ += " and LedgerFK in('" + ledger + "')";
                        if (stageid != "")
                            SelQ += " and r.Boarding in('" + stageid + "')";
                        if (vehiType != "")
                            SelQ += " and v.Purchased_From in('" + vehiType + "')";
                        if (!string.IsNullOrEmpty(fnlYr))
                            SelQ += " and a.finyearfk in('" + fnlYr + "')";
                        SelQ += "group by a.App_No,veh_id,a.FeeCategory,a.finyearfk  having sum(TotalAmount) = sum(a.BalAmount) ";

                        #endregion

                        #endregion
                    }
                    #endregion
                }
                //               
                SelQ = SelQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                #endregion
            }


            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
        }
        catch { }
        return dsload;
    }
    //stage
    protected void loadStudValuesSchool()
    {
        try
        {
            #region design
            loadcolumns();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 12;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].SelectionBackColor = Color.White;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Stage";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[1].Visible = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total No Stud";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].Visible = true;
            if (!colord.Contains("1"))
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = spsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            if (!colord.Contains("2"))
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Amt";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[4].Visible = true;
            if (!colord.Contains("3"))
            {
                FpSpread1.Sheets[0].Columns[4].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[4].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Concession";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[5].Visible = true;
            if (!colord.Contains("4"))
            {
                FpSpread1.Sheets[0].Columns[5].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[5].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Allot Amt";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[6].Visible = true;
            if (!colord.Contains("5"))
            {
                FpSpread1.Sheets[0].Columns[6].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[6].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[7].Visible = true;
            if (!colord.Contains("6"))
            {
                FpSpread1.Sheets[0].Columns[7].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[7].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Balance";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[8].Visible = true;
            if (!colord.Contains("7"))
            {
                FpSpread1.Sheets[0].Columns[8].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[8].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Fully Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Tag = "-1";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[9].Visible = true;
            if (!colord.Contains("8"))
            {
                FpSpread1.Sheets[0].Columns[9].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[9].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Partially Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Tag = "-2";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[10].Visible = true;
            if (!colord.Contains("9"))
            {
                FpSpread1.Sheets[0].Columns[10].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[10].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Not Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Tag = "-3";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[11].Visible = true;
            if (!colord.Contains("10"))
            {
                FpSpread1.Sheets[0].Columns[11].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[11].Visible = true;
            }

            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            #endregion

            #region value
            Hashtable gdtot = new Hashtable();
            Hashtable fnlgdtot = new Hashtable();
            double feemat = 0;
            double totamt = 0;
            double consamt = 0;
            double totcnt = 0;
            double paidamt = 0;
            double balamt = 0;
            double fullypaid = 0;
            double partpaid = 0;
            double notpaid = 0;
            DataView dvpaid = new DataView();
            DataView dvpart = new DataView();
            DataView dvnot = new DataView();
            DataView Dview = new DataView();
            Hashtable htfin = getFinYear();
            for (int fnl = 0; fnl < chklsfyear.Items.Count; fnl++)
            {
                bool boolfnlYr = false;
                if (chklsfyear.Items[fnl].Selected)
                {
                    string finYrPk = Convert.ToString(chklsfyear.Items[fnl].Value);
                    ds.Tables[0].DefaultView.RowFilter = "finyearfk='" + finYrPk + "'";
                    DataTable dvfnlYr = ds.Tables[0].DefaultView.ToTable();
                    if (dvfnlYr.Rows.Count > 0)
                    {
                        #region
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(htfin[finYrPk.Trim()]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = finYrPk;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = "-1";
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Green;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                        for (int sel = 0; sel < dvfnlYr.Rows.Count; sel++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvfnlYr.Rows[sel]["stage_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dvfnlYr.Rows[sel]["boarding"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dvfnlYr.Rows[sel]["finyearfk"]);

                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["totstud"]), out totcnt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totcnt);
                            if (!gdtot.Contains(2))
                                gdtot.Add(2, Convert.ToString(totcnt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[2]), out total);
                                total += totcnt;
                                gdtot.Remove(2);
                                gdtot.Add(2, Convert.ToString(total));
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dvfnlYr.Rows[sel]["LedgerFK"]);
                            //feecat
                            string TextName = "";
                            string textcode = "";
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                ds.Tables[4].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dvfnlYr.Rows[sel]["FeeCategory"]) + "'";
                                Dview = ds.Tables[4].DefaultView;
                                if (Dview.Count > 0)
                                {
                                    TextName = Convert.ToString(Dview[0]["TextVal"]);
                                    textcode = Convert.ToString(Dview[0]["Textcode"]);
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = TextName;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = textcode;
                            //fee amount
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["feeamt"]), out feemat);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(feemat);
                            if (!gdtot.Contains(4))
                                gdtot.Add(4, Convert.ToString(feemat));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[4]), out total);
                                total += feemat;
                                gdtot.Remove(4);
                                gdtot.Add(4, Convert.ToString(total));
                            }

                            //concession
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["concession"]), out consamt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(consamt);
                            if (!gdtot.Contains(5))
                                gdtot.Add(5, Convert.ToString(consamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[5]), out total);
                                total += consamt;
                                gdtot.Remove(5);
                                gdtot.Add(5, Convert.ToString(total));
                            }

                            //total amount
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["totamt"]), out totamt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(totamt);
                            if (!gdtot.Contains(6))
                                gdtot.Add(6, Convert.ToString(totamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[6]), out total);
                                total += totamt;
                                gdtot.Remove(6);
                                gdtot.Add(6, Convert.ToString(total));
                            }

                            //paid amount
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["paidamt"]), out paidamt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(paidamt);
                            if (!gdtot.Contains(7))
                                gdtot.Add(7, Convert.ToString(paidamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[7]), out total);
                                total += paidamt;
                                gdtot.Remove(7);
                                gdtot.Add(7, Convert.ToString(total));
                            }

                            //balance
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["balamt"]), out balamt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(balamt);
                            if (!gdtot.Contains(8))
                                gdtot.Add(8, Convert.ToString(balamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[8]), out total);
                                total += balamt;
                                gdtot.Remove(8);
                                gdtot.Add(8, Convert.ToString(total));
                            }
                            //fully paid   
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = "boarding='" + Convert.ToString(dvfnlYr.Rows[sel]["boarding"]) + "' and FeeCategory='" + Convert.ToString(dvfnlYr.Rows[sel]["FeeCategory"]) + "' and finyearfk='" + finYrPk + "'";
                                dvpaid = ds.Tables[1].DefaultView;
                                if (dvpaid.Count > 0)
                                {
                                    double totcount = 0;
                                    for (int i = 0; i < dvpaid.Count; i++)
                                    {
                                        double.TryParse(Convert.ToString(dvpaid[i]["cnt"]), out totcount);
                                        fullypaid += totcount;
                                    }

                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(fullypaid);
                            // FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 8].t
                            if (!gdtot.Contains(9))
                                gdtot.Add(9, Convert.ToString(fullypaid));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[9]), out total);
                                total += fullypaid;
                                gdtot.Remove(9);
                                gdtot.Add(9, Convert.ToString(total));
                            }
                            fullypaid = 0;
                            //partially paid
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "boarding='" + Convert.ToString(dvfnlYr.Rows[sel]["boarding"]) + "'  and FeeCategory='" + Convert.ToString(dvfnlYr.Rows[sel]["FeeCategory"]) + "' and finyearfk='" + finYrPk + "'";
                                dvpart = ds.Tables[2].DefaultView;
                                if (dvpart.Count > 0)
                                {
                                    double totcount = 0;
                                    for (int i = 0; i < dvpart.Count; i++)
                                    {
                                        double.TryParse(Convert.ToString(dvpart[i]["cnt"]), out totcount);
                                        partpaid += totcount;
                                    }
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(partpaid);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Tag = -2;
                            if (!gdtot.Contains(10))
                                gdtot.Add(10, Convert.ToString(partpaid));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[10]), out total);
                                total += partpaid;
                                gdtot.Remove(10);
                                gdtot.Add(10, Convert.ToString(total));
                            }
                            partpaid = 0;
                            //not paid
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                ds.Tables[3].DefaultView.RowFilter = "boarding='" + Convert.ToString(dvfnlYr.Rows[sel]["boarding"]) + "'  and FeeCategory='" + Convert.ToString(dvfnlYr.Rows[sel]["FeeCategory"]) + "' and finyearfk='" + finYrPk + "'";
                                dvnot = ds.Tables[3].DefaultView;
                                if (dvnot.Count > 0)
                                {
                                    double totcount = 0;
                                    for (int i = 0; i < dvnot.Count; i++)
                                    {
                                        double.TryParse(Convert.ToString(dvnot[i]["cnt"]), out totcount);
                                        notpaid += totcount;
                                    }

                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(notpaid);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Tag = -3;
                            if (!gdtot.Contains(11))
                                gdtot.Add(11, Convert.ToString(notpaid));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[11]), out total);
                                total += notpaid;
                                gdtot.Remove(11);
                                gdtot.Add(11, Convert.ToString(total));
                            }
                            notpaid = 0;
                            boolfnlYr = true;
                        }
                        #endregion
                        if (boolfnlYr)
                        {
                            #region every financial year total added
                            double grandSubTotal = 0;
                            // FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = finYrPk;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = "-1";
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.GreenYellow;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                            for (int i = 2; i < FpSpread1.Sheets[0].ColumnCount; i++)
                            {
                                double.TryParse(Convert.ToString(gdtot[i]), out grandSubTotal);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandSubTotal);
                                if (!fnlgdtot.Contains(i))//grand total add
                                    fnlgdtot.Add(i, Convert.ToString(grandSubTotal));
                                else
                                {
                                    double total = 0;
                                    double.TryParse(Convert.ToString(fnlgdtot[i]), out total);
                                    total += grandSubTotal;
                                    fnlgdtot.Remove(i);
                                    fnlgdtot.Add(i, Convert.ToString(total));
                                }
                            }
                            gdtot.Clear();
                            #endregion
                        }
                    }
                }
            }

            #endregion

            #region Grandtotal
            double grandTotal = 0;
            // FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Gold;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
            for (int i = 2; i < FpSpread1.Sheets[0].ColumnCount; i++)
            {
                double.TryParse(Convert.ToString(fnlgdtot[i]), out grandTotal);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandTotal);
            }

            #endregion
            FpSpread1.Sheets[0].SelectionBackColor = Color.Green;
            // FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            gnlcolorder.Visible = true;
            divspread.Visible = true;
            FpSpread1.Visible = true;
            studdet.Visible = false;
            fpstud.Visible = false;
            lblvalidation1.Text = "";
            print.Visible = true;
            subprint.Visible = false;
        }
        catch { }
    }
    //route
    protected void loadStudRouteValuesSchool()
    {
        try
        {
            #region design
            loadcolumns();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 13;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            int check = 0;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Route Id";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vehile ID";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total No Stud";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            if (!colord.Contains("1"))
            {
                FpSpread1.Sheets[0].Columns[3].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = spsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].Visible = true;
            if (!colord.Contains("2"))
            {
                FpSpread1.Sheets[0].Columns[4].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[4].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Amt";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[5].Visible = true;
            if (!colord.Contains("3"))
            {
                FpSpread1.Sheets[0].Columns[5].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[5].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Concession";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[6].Visible = true;
            if (!colord.Contains("4"))
            {
                FpSpread1.Sheets[0].Columns[6].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[6].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Allot Amt";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[7].Visible = true;
            if (!colord.Contains("5"))
            {
                FpSpread1.Sheets[0].Columns[7].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[7].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[8].Visible = true;
            if (!colord.Contains("6"))
            {
                FpSpread1.Sheets[0].Columns[8].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[8].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Balance";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[9].Visible = true;
            if (!colord.Contains("7"))
            {
                FpSpread1.Sheets[0].Columns[9].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[9].Visible = true;
            }


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Fully Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Tag = "-1";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[10].Visible = true;
            if (!colord.Contains("8"))
            {
                FpSpread1.Sheets[0].Columns[10].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[10].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Partially Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Tag = "-2";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[11].Visible = true;
            if (!colord.Contains("9"))
            {
                FpSpread1.Sheets[0].Columns[11].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[11].Visible = true;
            }

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Not Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Tag = "-3";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[12].Visible = true;
            if (!colord.Contains("10"))
            {
                FpSpread1.Sheets[0].Columns[12].Visible = false;
            }
            if (colord.Count == 0)
            {
                FpSpread1.Sheets[0].Columns[12].Visible = true;
            }

            #endregion

            #region value
            double feemat = 0;
            double totamt = 0;
            double consamt = 0;
            double totcnt = 0;
            double paidamt = 0;
            double balamt = 0;
            double fullypaid = 0;
            double partpaid = 0;
            double notpaid = 0;
            Hashtable gdtot = new Hashtable();
            DataView dvpaid = new DataView();
            DataView dvpart = new DataView();
            DataView dvnot = new DataView();
            DataView Dview = new DataView();
            Hashtable fnlgdtot = new Hashtable();
            Hashtable htfin = getFinYear();
            for (int fnl = 0; fnl < chklsfyear.Items.Count; fnl++)
            {
                bool boolfnlYr = false;
                if (chklsfyear.Items[fnl].Selected)
                {
                    string finYrPk = Convert.ToString(chklsfyear.Items[fnl].Value);
                    ds.Tables[0].DefaultView.RowFilter = "finyearfk='" + finYrPk + "'";
                    DataTable dvfnlYr = ds.Tables[0].DefaultView.ToTable();
                    if (dvfnlYr.Rows.Count > 0)
                    {
                        #region
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(htfin[finYrPk.Trim()]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = finYrPk;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = "-1";
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Green;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                        for (int sel = 0; sel < dvfnlYr.Rows.Count; sel++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dvfnlYr.Rows[sel]["finyearfk"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvfnlYr.Rows[sel]["bus_routeid"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dvfnlYr.Rows[sel]["bus_routeid"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvfnlYr.Rows[sel]["veh_id"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dvfnlYr.Rows[sel]["LedgerFK"]);
                            //total student count
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["totstud"]), out totcnt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(totcnt);
                            if (!gdtot.Contains(3))
                                gdtot.Add(3, Convert.ToString(totcnt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[3]), out total);
                                total += totcnt;
                                gdtot.Remove(3);
                                gdtot.Add(3, Convert.ToString(total));
                            }
                            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]);
                            //feecat
                            string TextName = "";
                            string textcode = "";
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                ds.Tables[4].DefaultView.RowFilter = "TextCode='" + Convert.ToString(ds.Tables[0].Rows[sel]["FeeCategory"]) + "'";
                                Dview = ds.Tables[4].DefaultView;
                                if (Dview.Count > 0)
                                {
                                    TextName = Convert.ToString(Dview[0]["TextVal"]);
                                    textcode = Convert.ToString(Dview[0]["Textcode"]);
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = TextName;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = textcode;

                            //fee amount
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["feeamt"]), out feemat);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(feemat);
                            if (!gdtot.Contains(5))
                                gdtot.Add(5, Convert.ToString(feemat));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[5]), out total);
                                total += feemat;
                                gdtot.Remove(5);
                                gdtot.Add(5, Convert.ToString(total));
                            }

                            //concession amount
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["concession"]), out consamt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(consamt);
                            if (!gdtot.Contains(6))
                                gdtot.Add(6, Convert.ToString(consamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[6]), out total);
                                total += consamt;
                                gdtot.Remove(6);
                                gdtot.Add(6, Convert.ToString(total));
                            }

                            //total amount
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["totamt"]), out totamt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(totamt);
                            if (!gdtot.Contains(7))
                                gdtot.Add(7, Convert.ToString(totamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[7]), out total);
                                total += totamt;
                                gdtot.Remove(7);
                                gdtot.Add(7, Convert.ToString(total));
                            }

                            //paidamt
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["paidamt"]), out paidamt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(paidamt);
                            if (!gdtot.Contains(8))
                                gdtot.Add(8, Convert.ToString(paidamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[8]), out total);
                                total += paidamt;
                                gdtot.Remove(8);
                                gdtot.Add(8, Convert.ToString(total));
                            }
                            //bal amt
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["balamt"]), out balamt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(balamt);
                            if (!gdtot.Contains(9))
                                gdtot.Add(9, Convert.ToString(balamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[9]), out total);
                                total += balamt;
                                gdtot.Remove(9);
                                gdtot.Add(9, Convert.ToString(total));
                            }

                            //fully paid   
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = "veh_id='" + Convert.ToString(dvfnlYr.Rows[sel]["veh_id"]) + "' and FeeCategory='" + Convert.ToString(dvfnlYr.Rows[sel]["FeeCategory"]) + "'";
                                dvpaid = ds.Tables[1].DefaultView;
                                if (dvpaid.Count > 0)
                                {
                                    double totcount = 0;
                                    for (int i = 0; i < dvpaid.Count; i++)
                                    {
                                        double.TryParse(Convert.ToString(dvpaid[i]["cnt"]), out totcount);
                                        fullypaid += totcount;
                                    }
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(fullypaid);
                            if (!gdtot.Contains(10))
                                gdtot.Add(10, Convert.ToString(fullypaid));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[10]), out total);
                                total += fullypaid;
                                gdtot.Remove(10);
                                gdtot.Add(10, Convert.ToString(total));
                            }
                            fullypaid = 0;
                            //partially paid
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "veh_id='" + Convert.ToString(dvfnlYr.Rows[sel]["veh_id"]) + "' and FeeCategory='" + Convert.ToString(dvfnlYr.Rows[sel]["FeeCategory"]) + "'";
                                dvpart = ds.Tables[2].DefaultView;
                                if (dvpart.Count > 0)
                                {
                                    double totcount = 0;
                                    for (int i = 0; i < dvpart.Count; i++)
                                    {
                                        double.TryParse(Convert.ToString(dvpart[i]["cnt"]), out totcount);
                                        partpaid += totcount;
                                    }
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(partpaid);
                            if (!gdtot.Contains(11))
                                gdtot.Add(11, Convert.ToString(partpaid));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[11]), out total);
                                total += partpaid;
                                gdtot.Remove(11);
                                gdtot.Add(11, Convert.ToString(total));
                            }
                            partpaid = 0;
                            //not paid
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                ds.Tables[3].DefaultView.RowFilter = "veh_id='" + Convert.ToString(dvfnlYr.Rows[sel]["veh_id"]) + "' and FeeCategory='" + Convert.ToString(dvfnlYr.Rows[sel]["FeeCategory"]) + "'";
                                dvnot = ds.Tables[3].DefaultView;
                                if (dvnot.Count > 0)
                                {
                                    double totcount = 0;
                                    for (int i = 0; i < dvnot.Count; i++)
                                    {
                                        double.TryParse(Convert.ToString(dvnot[i]["cnt"]), out totcount);
                                        notpaid += totcount;
                                    }
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(notpaid);
                            if (!gdtot.Contains(12))
                                gdtot.Add(12, Convert.ToString(notpaid));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(gdtot[12]), out total);
                                total += notpaid;
                                gdtot.Remove(12);
                                gdtot.Add(12, Convert.ToString(total));
                            }
                            notpaid = 0;
                            boolfnlYr = true;
                        }
                        #endregion
                        if (boolfnlYr)
                        {
                            #region every financial year total added
                            double grandSubTotal = 0;
                            // FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = finYrPk;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = "-1";
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.GreenYellow;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                            for (int i = 3; i < FpSpread1.Sheets[0].ColumnCount; i++)
                            {
                                double.TryParse(Convert.ToString(gdtot[i]), out grandSubTotal);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandSubTotal);
                                if (!fnlgdtot.Contains(i))//grand total add
                                    fnlgdtot.Add(i, Convert.ToString(grandSubTotal));
                                else
                                {
                                    double total = 0;
                                    double.TryParse(Convert.ToString(fnlgdtot[i]), out total);
                                    total += grandSubTotal;
                                    fnlgdtot.Remove(i);
                                    fnlgdtot.Add(i, Convert.ToString(total));
                                }
                            }
                            gdtot.Clear();
                            #endregion
                        }
                    }
                }
            }

            #endregion

            #region Grandtotal
            double grandTotal = 0;
            // FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Gold;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
            for (int i = 3; i < FpSpread1.Sheets[0].ColumnCount; i++)
            {
                double.TryParse(Convert.ToString(fnlgdtot[i]), out grandTotal);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandTotal);
            }

            #endregion

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            gnlcolorder.Visible = true;
            divspread.Visible = true;
            FpSpread1.Visible = true;
            studdet.Visible = false;
            fpstud.Visible = false;
            lblvalidation1.Text = "";
            print.Visible = true;
            subprint.Visible = false;
        }
        catch { }
    }
    //cellclick
    protected void getStudentSchoolDataset()
    {
        try
        {
            StringBuilder sbId = new StringBuilder();
            StringBuilder sbLedger = new StringBuilder();
            StringBuilder sbFeecat = new StringBuilder();
            StringBuilder sbFinFk = new StringBuilder();
            StringBuilder sbVehId = new StringBuilder();
            string ledger = string.Empty;
            bool clickval = false;
            string Paiddt = "";
            string paidcolvalue = "";
            string finYearPk = string.Empty;
            string getText = string.Empty;
            string tempfinYearPk = string.Empty;
            ArrayList arfinFk = new ArrayList();
            if (fpcellclick == true)
            {
                #region get value
                string feecat = "";
                string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
                string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
                string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
                string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
                string sem = Convert.ToString(getCblSelectedValue(cbl_sem));
                string stageid = Convert.ToString(getCblSelectedValue(cblstage));
                int row = Convert.ToInt32(actrow);
                int col = Convert.ToInt32(actcol);
                if (row != -1 && col != -1)
                {
                    sbId.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag));
                    sbLedger.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[row, 2].Tag));
                    sbFeecat.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[row, 3].Tag));
                    sbFinFk.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[row, 0].Tag));
                    if (rbroute.Checked)
                        sbVehId.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[row, 2].Text));
                    getText = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 0].Text);
                    finYearPk = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 0].Tag);

                    if (getText.Trim() == "Total")
                    {
                        string preFinFk = finYearPk;
                        string curFinfk = finYearPk;
                        sbId.Clear();
                        sbLedger.Clear();
                        sbFeecat.Clear();
                        sbFinFk.Clear();
                        for (int i = row; i >= 0; i--)
                        {
                            string fYearPk = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag);
                            curFinfk = fYearPk;
                            string strText = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Note);
                            getText = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Text);
                            if (strText.Trim() != "-1" && getText.Trim() != "Total")
                            {
                                sbId.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag) + "','");
                                sbLedger.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag) + "','");
                                sbFeecat.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Tag) + "','");
                                if (rbroute.Checked)
                                    sbVehId.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text) + "','");
                                if (!arfinFk.Contains(fYearPk))
                                {
                                    arfinFk.Add(fYearPk);
                                    sbFinFk.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag) + "','");
                                }
                            }
                            else if (preFinFk != curFinfk)
                                break;
                        }
                        if (sbId.Length > 0)
                            sbId.Remove(sbId.Length - 3, 3);
                        if (sbLedger.Length > 0)
                            sbLedger.Remove(sbLedger.Length - 3, 3);
                        if (sbFeecat.Length > 0)
                            sbFeecat.Remove(sbFeecat.Length - 3, 3);
                        if (sbFinFk.Length > 0)
                            sbFinFk.Remove(sbFinFk.Length - 3, 3);
                        if (rbroute.Checked)
                        {
                            if (sbVehId.Length > 0)
                                sbVehId.Remove(sbVehId.Length - 3, 3);
                        }
                    }

                    if (getText.Trim() == "Grand Total" && row == FpSpread1.Sheets[0].Rows.Count - 1)
                    {
                        sbId.Clear();
                        sbLedger.Clear();
                        sbFeecat.Clear();
                        sbFinFk.Clear();
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
                        {
                            getText = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Text);
                            string strText = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Note);
                            string fYearPk = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag);
                            if (getText.Trim() != "Total" && strText.Trim() != "-1")
                            {
                                sbId.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag) + "','");
                                sbLedger.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag) + "','");
                                sbFeecat.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Tag) + "','");
                                if (rbroute.Checked)
                                    sbVehId.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text) + "','");
                                if (!arfinFk.Contains(fYearPk))
                                {
                                    arfinFk.Add(fYearPk);
                                    sbFinFk.Append(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag) + "','");
                                }
                            }
                        }
                        if (sbId.Length > 0)
                            sbId.Remove(sbId.Length - 3, 3);
                        if (sbLedger.Length > 0)
                            sbLedger.Remove(sbLedger.Length - 3, 3);
                        if (sbFeecat.Length > 0)
                            sbFeecat.Remove(sbFeecat.Length - 3, 3);
                        if (sbFinFk.Length > 0)
                            sbFinFk.Remove(sbFinFk.Length - 3, 3);
                        if (rbroute.Checked)
                        {
                            if (sbVehId.Length > 0)
                                sbVehId.Remove(sbVehId.Length - 3, 3);
                        }
                    }
                    if (sbId.Length > 0 && sbLedger.Length > 0)
                    {
                        if (rbstage.Checked == true)
                        {
                            if (col == 9 || paidcolvalue == "-1")
                                Paiddt = " and TotalAmount > 0 and BalAmount =0";
                            else if (col == 10 || paidcolvalue == "-2")
                                Paiddt = " and TotalAmount <> BalAmount and BalAmount > 0";
                            else if (col == 11 || paidcolvalue == "-3")
                                Paiddt = " and TotalAmount = BalAmount";

                            if (rbterm.Checked)
                            {
                                #region

                                string Selq = " select roll_no,roll_admit,reg_no,(stud_name+'-'+case when mode='1' then '(O)' when mode='2' then '(T)' when mode='3' then'(N)' end)as stud_name,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt,PaidAmount as paidamt,BalAmount balamt,FeeCategory,r.degree_code,finyearfk from FT_FeeAllot a,Registration r where a.App_No = r.App_No and isnull(IsCanceledStage,0)<>'1'";
                                if (Paiddt != "")
                                    Selq += Paiddt;
                                if (batch != "")
                                    Selq += " and r.Batch_year in('" + batch + "')";
                                if (degree != "")
                                    Selq += " and r.degree_code in('" + degree + "')";
                                if (sbId.Length > 0)
                                    Selq += " and Boarding in('" + Convert.ToString(sbId) + "')";
                                if (sbLedger.Length > 0)
                                    Selq += " and LedgerFK in('" + Convert.ToString(sbLedger) + "')";
                                if (sbFeecat.Length > 0)
                                    Selq += "and FeeCategory in( '" + Convert.ToString(sbFeecat) + "')";
                                if (sbFinFk.Length > 0)
                                    Selq += " and a.finyearfk in('" + Convert.ToString(sbFinFk) + "')";
                                Selq += " order by Roll_No,FeeCategory";
                                Selq = Selq + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(dt.dept_acronym) as Dept_Name from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in('" + collegecode + "')";
                                Selq = Selq + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                                dsstud.Clear();
                                dsstud = d2.select_method_wo_parameter(Selq, "Text");
                                if (dsstud.Tables.Count > 0 && dsstud.Tables[0].Rows.Count > 0)
                                {
                                    loadStudDetailsSchool();
                                    clickval = true;
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                if (col == 9)
                                    Paiddt = " and TotalAmount > 0 and a.BalAmount =0";
                                else if (col == 10)
                                    Paiddt = " and TotalAmount <> a.BalAmount anda.BalAmount > 0";
                                else if (col == 11)
                                    Paiddt = " and TotalAmount = a.BalAmount";

                                string ddlmnth = Convert.ToString(getCblSelectedValue(cblmonth));
                                string ddlyr = Convert.ToString(ddlyear.SelectedItem.Value);
                                string Selq = " select roll_no,roll_admit,reg_no,(stud_name+'-'+case when mode='1' then '(O)' when mode='2' then '(T)' when mode='3' then'(N)' end)as stud_name,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt,fm.AllotAmount,fm.PaidAmount as paidamt,fm.BalAmount balamt,FeeCategory,r.degree_code,AllotMonth,AllotYear,a.finyearfk from FT_FeeAllot a,Registration r,FT_FeeallotMonthly fm where a.App_No = r.App_No and a.FeeAllotPK=fm.FeeAllotPK   and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "')  and isnull(IsCanceledStage,0)<>'1'";
                                if (Paiddt != "")
                                    Selq += Paiddt;
                                if (batch != "")
                                    Selq += " and r.Batch_year in('" + batch + "')";
                                if (degree != "")
                                    Selq += " and r.degree_code in('" + degree + "')";
                                if (sbId.Length > 0)
                                    Selq += " and Boarding in('" + Convert.ToString(sbId) + "')";
                                if (sbLedger.Length > 0)
                                    Selq += " and LedgerFK in('" + Convert.ToString(sbLedger) + "')";
                                if (sbFinFk.Length > 0)
                                    Selq += " and a.finyearfk in('" + Convert.ToString(sbFinFk) + "')";
                                Selq += " order by AllotMonth, Roll_No,FeeCategory";
                                Selq = Selq + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(dt.dept_acronym) as Dept_Name from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in('" + collegecode + "')";
                                Selq = Selq + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                                dsstud.Clear();
                                dsstud = d2.select_method_wo_parameter(Selq, "Text");
                                if (dsstud.Tables.Count > 0 && dsstud.Tables[0].Rows.Count > 0)
                                {
                                    loadStudDetailsSchool();
                                    clickval = true;
                                }

                                #endregion
                            }
                        }
                        else if (rbroute.Checked == true)
                        {
                            if (col == 10 || paidcolvalue == "-1")
                                Paiddt = " and TotalAmount > 0 and BalAmount =0";
                            else if (col == 11 || paidcolvalue == "-2")
                                Paiddt = " and TotalAmount <> BalAmount and BalAmount > 0";
                            else if (col == 12 || paidcolvalue == "-3")
                                Paiddt = " and TotalAmount = BalAmount";
                            // feecat = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].Tag);
                            if (rbterm.Checked)
                            {
                                #region
                                //string ledger = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
                                string Selq = " select roll_no,roll_admit,r.reg_no,(stud_name+'-'+case when mode='1' then '(O)' when mode='2' then '(T)' when mode='3' then'(N)' end)as stud_name,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt ,PaidAmount as paidamt ,BalAmount as balamt,FeeCategory,r.degree_code,a.finyearfk from FT_FeeAllot a,Registration r,Vehicle_Master v where a.App_No = r.App_No and r.vehid = v.Veh_ID and isnull(IsCanceledStage,0)<>'1' ";
                                if (Paiddt != "")
                                    Selq += Paiddt;
                                if (batch != "")
                                    Selq += " and r.Batch_year in('" + batch + "')";
                                if (degree != "")
                                    Selq += " and r.degree_code in('" + degree + "')";
                                if (sbId.Length > 0)
                                    Selq += " and Bus_RouteID in('" + Convert.ToString(sbId) + "')";
                                if (sbLedger.Length > 0)
                                    Selq += " and LedgerFK in('" + Convert.ToString(sbLedger) + "')";
                                if (sbFeecat.Length > 0)
                                    Selq += "and FeeCategory in( '" + Convert.ToString(sbFeecat) + "')";
                                if (sbFinFk.Length > 0)
                                    Selq += " and a.finyearfk in('" + Convert.ToString(sbFinFk) + "')";
                                if (stageid != "")
                                    Selq += " and r.Boarding in('" + stageid + "')";
                                if (sbVehId.Length > 0)
                                    Selq += " and r.vehid in('" + Convert.ToString(sbVehId) + "')";

                                Selq += " order by Roll_No,FeeCategory";

                                Selq = Selq + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(dt.dept_acronym) as Dept_Name from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in('" + collegecode + "')";
                                Selq = Selq + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                                dsstud.Clear();
                                dsstud = d2.select_method_wo_parameter(Selq, "Text");
                                if (dsstud.Tables.Count > 0 && dsstud.Tables[0].Rows.Count > 0)
                                {
                                    loadStudDetailsSchool();
                                    clickval = true;
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                if (col == 10)
                                    Paiddt = " and TotalAmount > 0 and a.BalAmount =0";
                                else if (col == 11)
                                    Paiddt = " and TotalAmount <> a.BalAmount anda.BalAmount > 0";
                                else if (col == 12)
                                    Paiddt = " and TotalAmount = a.BalAmount";
                                string ddlmnth = Convert.ToString(getCblSelectedValue(cblmonth));
                                string ddlyr = Convert.ToString(ddlyear.SelectedItem.Value);
                                //string ledger = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
                                string Selq = " select roll_no,roll_admit,r.reg_no,(stud_name+'-'+case when mode='1' then '(O)' when mode='2' then '(T)' when mode='3' then'(N)' end)as stud_name,FeeAmount as feeamt,DeductAmout as concession ,TotalAmount as totamt ,fm.PaidAmount as paidamt ,fm.BalAmount as balamt,FeeCategory,r.degree_code,AllotMonth,AllotYear,fm.AllotAmount,a.finyearfk from FT_FeeAllot a,Registration r,FT_FeeallotMonthly fm,Vehicle_Master v where a.App_No = r.App_No and r.vehid = v.Veh_ID and a.FeeAllotPK=fm.FeeAllotPK  and AllotYear ='" + ddlyr + "' and AllotMonth in('" + ddlmnth + "') and isnull(IsCanceledStage,0)<>'1'";
                                if (Paiddt != "")
                                    Selq += Paiddt;
                                if (batch != "")
                                    Selq += " and r.Batch_year in('" + batch + "')";
                                if (degree != "")
                                    Selq += " and r.degree_code in('" + degree + "')";
                                if (sbId.Length > 0)
                                    Selq += " and Bus_RouteID in('" + Convert.ToString(sbId) + "')";
                                if (sbLedger.Length > 0)
                                    Selq += " and LedgerFK in('" + Convert.ToString(sbLedger) + "')";
                                if (sbFinFk.Length > 0)
                                    Selq += " and a.finyearfk in('" + Convert.ToString(sbFinFk) + "')";
                                if (stageid != "")
                                    Selq += " and r.Boarding in('" + stageid + "')";
                                if (sbVehId.Length > 0)
                                    Selq += " and r.vehid in('" + Convert.ToString(sbVehId) + "')";
                                Selq += " order by AllotMonth,Roll_No,a.FeeCategory";
                                Selq = Selq + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(dt.dept_acronym) as Dept_Name from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in('" + collegecode + "')";
                                Selq = Selq + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code in('" + collegecode + "')";
                                dsstud.Clear();
                                dsstud = d2.select_method_wo_parameter(Selq, "Text");
                                if (dsstud.Tables.Count > 0 && dsstud.Tables[0].Rows.Count > 0)
                                {
                                    loadStudDetailsSchool();
                                    clickval = true;
                                }
                                #endregion
                            }
                        }
                        if (clickval == false)
                        {

                            studdet.Visible = false;
                            fpstud.Visible = false;
                            subprint.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "No Record Found";
                        }
                    }
                }
                else
                {

                    studdet.Visible = false;
                    fpstud.Visible = false;
                    subprint.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "No Record Found";
                }
                #endregion
            }
        }
        catch { }
    }
    //view student details
    protected void loadStudDetailsSchool()
    {
        try
        {
            #region design
            RollAndRegSettings();
            loadcolumns();
            fpstud.Sheets[0].RowCount = 0;
            fpstud.Sheets[0].ColumnCount = 0;
            fpstud.CommandBar.Visible = false;
            fpstud.Sheets[0].AutoPostBack = true;
            fpstud.Sheets[0].ColumnHeader.RowCount = 1;
            fpstud.Sheets[0].RowHeader.Visible = false;
            fpstud.Sheets[0].ColumnCount = 14;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            fpstud.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
            //if (ViewState["colord"] != null)
            //{
            //    colsndord = (ArrayList)ViewState["sendcol"];
            //    loadsndcolumns();
            //}

            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            fpstud.Sheets[0].Columns[1].Visible = true;

            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            fpstud.Sheets[0].Columns[2].Visible = true;

            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            fpstud.Sheets[0].Columns[3].Visible = true;


            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            fpstud.Sheets[0].Columns[4].Visible = true;

            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Department";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            fpstud.Sheets[0].Columns[5].Visible = true;

            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Semester";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[6].Visible = true;

            if (rbmonth.Checked == true)
            {
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Month";
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                fpstud.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                fpstud.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                fpstud.Sheets[0].Columns[7].Visible = true;

                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Year";
                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                fpstud.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                fpstud.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
                fpstud.Sheets[0].Columns[8].Visible = true;

            }

            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Total Amt";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[9].Visible = true;
            if (!colord.Contains("3"))
                fpstud.Sheets[0].Columns[9].Visible = false;

            if (colord.Count == 0)
                fpstud.Sheets[0].Columns[9].Visible = true;


            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Concession";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
            fpstud.Sheets[0].Columns[10].Visible = true;
            if (!colord.Contains("4"))
                fpstud.Sheets[0].Columns[10].Visible = false;

            if (colord.Count == 0)
                fpstud.Sheets[0].Columns[10].Visible = true;


            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Allot Amt";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
            fpstud.Sheets[0].Columns[11].Visible = true;
            if (!colord.Contains("5"))
                fpstud.Sheets[0].Columns[11].Visible = false;

            if (colord.Count == 0)
                fpstud.Sheets[0].Columns[11].Visible = true;



            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Paid";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Right;
            fpstud.Sheets[0].Columns[12].Visible = true;
            if (!colord.Contains("6"))
                fpstud.Sheets[0].Columns[12].Visible = false;

            if (colord.Count == 0)
                fpstud.Sheets[0].Columns[12].Visible = true;

            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Balance";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].ForeColor = ColorTranslator.FromHtml("#000000");
            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].Font.Bold = true;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].Font.Name = "Book Antiqua";
            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].Font.Size = FontUnit.Medium;
            fpstud.Sheets[0].ColumnHeader.Cells[0, 13].HorizontalAlign = HorizontalAlign.Center;
            fpstud.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Right;
            fpstud.Sheets[0].Columns[13].Visible = true;
            if (!colord.Contains("7"))
                fpstud.Sheets[0].Columns[13].Visible = false;

            if (colord.Count == 0)
                fpstud.Sheets[0].Columns[13].Visible = true;

            fpstud.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpstud.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpstud.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpstud.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpstud.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            fpstud.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            spreadColumnVisible();
            #endregion

            #region value
            DataView Dview = new DataView();
            DataView fee = new DataView();
            double totamt = 0;
            double feemat = 0;
            double consamt = 0;
            double totcnt = 0;
            double paidamt = 0;
            double balamt = 0;
            ArrayList arsno = new ArrayList();
            int sno = 0;
            Hashtable grandtot = new Hashtable();
            Hashtable fnlgdtot = new Hashtable();
            Hashtable htfin = getFinYear();
            for (int row = 0; row < chklsfyear.Items.Count; row++)
            {
                bool boolfnlYr = false;
                if (chklsfyear.Items[row].Selected)
                {
                    string finYrPk = Convert.ToString(chklsfyear.Items[row].Value);
                    dsstud.Tables[0].DefaultView.RowFilter = "finyearfk='" + finYrPk + "'";
                    DataTable dvfnlYr = dsstud.Tables[0].DefaultView.ToTable();
                    if (dvfnlYr.Rows.Count > 0)
                    {
                        fpstud.Sheets[0].RowCount++;
                        fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(htfin[finYrPk.Trim()]);
                        fpstud.Sheets[0].Rows[fpstud.Sheets[0].RowCount - 1].BackColor = Color.Green;
                        fpstud.Sheets[0].SpanModel.Add(fpstud.Sheets[0].RowCount - 1, 0, 1, fpstud.Sheets[0].ColumnCount - 1);
                        #region
                        for (int sel = 0; sel < dvfnlYr.Rows.Count; sel++)
                        {
                            fpstud.Sheets[0].RowCount++;
                            sno++;
                            //if (!arsno.Contains(Convert.ToString(dvfnlYr.Rows[sel]["roll_no"])))
                            //{
                            //    arsno.Add(Convert.ToString(dvfnlYr.Rows[sel]["roll_no"]));
                            //    sno++;
                            //}
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvfnlYr.Rows[sel]["roll_no"]);
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvfnlYr.Rows[sel]["reg_no"]);
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvfnlYr.Rows[sel]["reg_no"]);

                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 1].CellType = txtroll;
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 2].CellType = txtreg;


                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvfnlYr.Rows[sel]["stud_name"]);
                            string Degreename = "";
                            if (dsstud.Tables[1].Rows.Count > 0)
                            {
                                dsstud.Tables[1].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dvfnlYr.Rows[sel]["Degree_code"]) + "'";
                                Dview = dsstud.Tables[1].DefaultView;
                                if (Dview.Count > 0)
                                {
                                    if (cbdeptacr.Checked == true)
                                        Degreename = Convert.ToString(Dview[0]["Dept_Name"]);
                                    else
                                        Degreename = Convert.ToString(Dview[0]["degreename"]);
                                }
                            }
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 5].Text = Degreename;
                            string TextName = "";
                            if (dsstud.Tables[2].Rows.Count > 0)
                            {
                                dsstud.Tables[2].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dvfnlYr.Rows[sel]["FeeCategory"]) + "'";
                                fee = dsstud.Tables[2].DefaultView;
                                if (fee.Count > 0)
                                {
                                    TextName = Convert.ToString(fee[0]["TextVal"]);
                                }
                            }
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 6].Text = TextName;
                            if (rbmonth.Checked == true)
                            {
                                string mnthname = textMonth(Convert.ToString(dvfnlYr.Rows[sel]["AllotMonth"]));
                                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 7].Text = mnthname;
                                fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dvfnlYr.Rows[sel]["AllotYear"]);
                                double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["AllotAmount"]), out totamt);
                                fpstud.Sheets[0].Columns[7].Visible = true;
                                fpstud.Sheets[0].Columns[8].Visible = true;
                            }
                            else
                            {
                                fpstud.Sheets[0].Columns[7].Visible = false;
                                fpstud.Sheets[0].Columns[8].Visible = false;
                                double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["totamt"]), out totamt);
                            }

                            //fee amount
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["feeamt"]), out feemat);
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(feemat);
                            if (!grandtot.Contains(9))
                                grandtot.Add(9, Convert.ToString(feemat));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(grandtot[9]), out total);
                                total += feemat;
                                grandtot.Remove(9);
                                grandtot.Add(9, Convert.ToString(total));
                            }

                            //concession
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["concession"]), out consamt);
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(consamt);
                            if (!grandtot.Contains(10))
                                grandtot.Add(10, Convert.ToString(consamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(grandtot[10]), out total);
                                total += consamt;
                                grandtot.Remove(10);
                                grandtot.Add(10, Convert.ToString(total));
                            }

                            //paidamt               
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(totamt);
                            if (!grandtot.Contains(11))
                                grandtot.Add(11, Convert.ToString(totamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(grandtot[11]), out total);
                                total += totamt;
                                grandtot.Remove(11);
                                grandtot.Add(11, Convert.ToString(total));
                            }

                            //paid amount
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["paidamt"]), out paidamt);
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(paidamt);
                            if (!grandtot.Contains(12))
                                grandtot.Add(12, Convert.ToString(paidamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(grandtot[12]), out total);
                                total += paidamt;
                                grandtot.Remove(12);
                                grandtot.Add(12, Convert.ToString(total));
                            }

                            //balance amt
                            double.TryParse(Convert.ToString(dvfnlYr.Rows[sel]["balamt"]), out balamt);
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(balamt);
                            if (!grandtot.Contains(13))
                                grandtot.Add(13, Convert.ToString(balamt));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(grandtot[13]), out total);
                                total += balamt;
                                grandtot.Remove(13);
                                grandtot.Add(13, Convert.ToString(total));
                            }
                            boolfnlYr = true;
                        }
                        #endregion
                        if (boolfnlYr)
                        {
                            #region every financial year total added
                            double grandSubTotal = 0;
                            //FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                            fpstud.Sheets[0].RowCount++;
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 0].Text = "Total";
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 0].Tag = finYrPk;
                            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 0].Note = "-1";
                            fpstud.Sheets[0].Rows[fpstud.Sheets[0].RowCount - 1].BackColor = Color.GreenYellow;
                            fpstud.Sheets[0].SpanModel.Add(fpstud.Sheets[0].RowCount - 1, 0, 1, 4);
                            for (int i = 9; i < fpstud.Sheets[0].ColumnCount; i++)
                            {
                                double.TryParse(Convert.ToString(grandtot[i]), out grandSubTotal);
                                fpstud.Sheets[0].Cells[fpstud.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandSubTotal);
                                if (!fnlgdtot.Contains(i))//grand total add
                                    fnlgdtot.Add(i, Convert.ToString(grandSubTotal));
                                else
                                {
                                    double total = 0;
                                    double.TryParse(Convert.ToString(fnlgdtot[i]), out total);
                                    total += grandSubTotal;
                                    fnlgdtot.Remove(i);
                                    fnlgdtot.Add(i, Convert.ToString(total));
                                }
                            }
                            grandtot.Clear();
                            #endregion
                        }
                    }
                }
            }

            #endregion

            #region Grand total
            double grandTotal = 0;
            //fpstud.Sheets[0].PageSize = dsstud.Tables[0].Rows.Count + 1;
            fpstud.Sheets[0].RowCount++;
            fpstud.Sheets[0].Cells[fpstud.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            fpstud.Sheets[0].Rows[fpstud.Sheets[0].RowCount - 1].BackColor = Color.Gold;
            fpstud.Sheets[0].SpanModel.Add(fpstud.Sheets[0].RowCount - 1, 0, 1, 4);
            for (int i = 9; i < fpstud.Sheets[0].ColumnCount; i++)
            {
                double.TryParse(Convert.ToString(fnlgdtot[i]), out grandTotal);
                fpstud.Sheets[0].Cells[fpstud.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(grandTotal);
            }

            #endregion

            fpstud.Sheets[0].PageSize = fpstud.Sheets[0].RowCount;
            fpstud.SaveChanges();
            studdet.Visible = true;
            fpstud.Visible = true;
            subprint.Visible = true;
        }
        catch { }
    }

    protected Hashtable getFinYear()
    {
        Hashtable htfnlYr = new Hashtable();
        string SelQ = "  select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)))as finyear,finyearpk,collegecode from fm_finyearmaster where collegecode='" + collegecode + "'";
        DataSet dsval = d2.select_method_wo_parameter(SelQ, "Text");
        if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
        {
            for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
            {
                string finYearPk = Convert.ToString(dsval.Tables[0].Rows[row]["finyearpk"]);
                string finYear = Convert.ToString(dsval.Tables[0].Rows[row]["finyear"]);
                if (!htfnlYr.ContainsKey(finYearPk))
                    htfnlYr.Add(finYearPk, finYear);
            }
        }
        return htfnlYr;
    }
}