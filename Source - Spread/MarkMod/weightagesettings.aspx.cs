using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Drawing;
using InsproDataAccess;
using System.Text;
using wc = System.Web.UI.WebControls;



public partial class MarkMod_weightagesettings : System.Web.UI.Page
{

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string group_code = string.Empty;
    string columnfield = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string strsem = string.Empty;
    string strsec = string.Empty;

    InsproDirectAccess dir = new InsproDirectAccess();
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static Hashtable htb = new Hashtable();
    static Hashtable htsubjcide = new Hashtable();
    System.Text.StringBuilder coname = new System.Text.StringBuilder();



    protected void Page_Load(object sender, EventArgs e)
    {
        try
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

            collegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            usercode = (Session["userco,de"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            group_user = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";


            if (!IsPostBack)
            {
                bindclg();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                //  BindSectionDetail();
                // bindSubject();

            }
        }
        catch
        {
        }
    }

    #region bindheaders

    public void bindclg()
    {
        try
        {

            group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();

            }

        }
        catch
        {
        }
    }

    public void bindbatch()
    {
        try
        {

            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
            }
        }
        catch
        {
        }
    }

    public void binddegree()
    {
        try
        {

            collegecode = ddlcollege.SelectedValue.ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            string typ = ddldegree.SelectedValue.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            hat.Clear();
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", typ);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds.Clear();
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }


        }
        catch (Exception ex)
        {

        }
    }

    public void bindsem()
    {
        try
        {
            ddlsemester.Items.Clear();

            string degreecode = ddlbranch.SelectedValue.ToString();
            string strgetfuncuti = string.Empty;
            if (degreecode.Trim() != "")
            {
                strgetfuncuti = d2.GetFunction("select max(Duration) from Degree where Degree_Code in(" + degreecode + ")");
            }
            if (Convert.ToInt32(strgetfuncuti) > 0)
            {
                for (int loop_val = 1; loop_val <= Convert.ToInt32(strgetfuncuti); loop_val++)
                {
                    ddlsemester.Items.Add(loop_val.ToString());
                }
            }

        }
        catch
        {
        }
    }
    //public void BindSectionDetail()
    //{
    //    try
    //    {
    //        string batch = ddlbatch.SelectedValue.ToString();
    //        string branch = ddlbranch.SelectedValue.ToString();
    //        ddlsection.Items.Clear();
    //        ds.Dispose();
    //        ds.Reset();
    //        ds = d2.BindSectionDetail(batch, branch);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlsection.DataSource = ds;
    //            ddlsection.DataTextField = "sections";
    //            ddlsection.DataBind();
    //            ddlsection.Items.Insert(0, "All");
    //            if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
    //            {
    //                ddlsection.Enabled = false;

    //            }
    //            else
    //            {
    //                ddlsection.Enabled = true;

    //            }
    //        }
    //        else
    //        {
    //            ddlsection.Enabled = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}



    //public void bindSubject()
    //{


    //    string batchYear = Convert.ToString(ddlbatch.SelectedValue);
    //    string degCode = Convert.ToString(ddlbranch.SelectedValue);
    //    string sem = Convert.ToString(ddlsemester.SelectedValue);
    //    string colCode = Convert.ToString(ddlcollege.SelectedValue);

    //    string selectQ = string.Empty;

    //    selectQ = "select distinct subject_name,s.subject_code,CONVERT(nvarchar(max),subject_code)+'-'+CONVERT(nvarchar(max),subject_name) as subName from registration r,subject s,syllabus_master sm,Sub_sem Sem,degree d,course c  where d.degree_code=r.degree_code and d.course_id=c.course_id and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year and r.current_semester=sm.semester and sm.syll_code=s.syll_code  and  s.syll_code=SM.syll_code  and  S.subtype_no = Sem.subtype_no and promote_count=1 and c.college_code=" + colCode + "  and r.degree_code='" + degCode + "' and r.Batch_Year='" + batchYear + "' and r.Current_Semester='" + sem + "'  and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar'  order by subject_name";

    //    DataSet dss = d2.select_method_wo_parameter(selectQ, "text");
    //    if (dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
    //    {
    //        ddl_subject.DataSource = dss;
    //        ddl_subject.DataTextField = "subName";
    //        ddl_subject.DataValueField = "subject_code";
    //        ddl_subject.DataBind();

    //    }

    //}





    #endregion
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }
        return null;
    }


    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
        // BindSectionDetail();
        // bindSubject();
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
        bindsem();
        // BindSectionDetail();
        // bindSubject();

    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        //BindSectionDetail();
        // bindSubject();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        //  BindSectionDetail();
        //bindSubject();
    }
    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        // BindSectionDetail();
        // bindSubject();
    }
    //protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}

    //protected void ddl_subject_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //}
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            // string section = string.Empty;

            //if (ddlsection.Enabled == true)
            //{
            //    if (ddlsection.SelectedIndex.ToString() != "0")
            //    {
            //        section = " and section='" + ddlsection.SelectedItem.Text.ToString() + "'";

            //    }
            //}
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add("CO");
            dt.Columns.Add("Subject");
            dt.Columns.Add("subject_no");
            dt.Columns.Add("Test");
            dt.Columns.Add("CriteriaName");
            dt.Columns.Add("WeightagePercentage");

            string qry = "select distinct isnull(template,'') as co,masterno from Master_Settings where settings='COSettings'";
            qry = qry + " select coname,cono,subject_no,criteria_no,criterianame,weightage_percentage from weightage_setting where batch='" + ddlbatch.SelectedItem.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsemester.SelectedItem.Text.ToString() + "'";


            DataSet ds1 = d2.select_method_wo_parameter(qry, "text");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                if (ds1.Tables[1].Rows.Count > 0)
                {
                    for (int i1 = 0; i1 < ds1.Tables[1].Rows.Count; i1++)
                    {
                        dr = dt.NewRow();
                        dr["CO"] = Convert.ToString(ds1.Tables[1].Rows[i1]["cono"]);
                        dr["Subject"] = Convert.ToString(ds1.Tables[1].Rows[i1]["subject_no"]);
                        dr["Test"] = Convert.ToString(ds1.Tables[1].Rows[i1]["criteria_no"]);
                        dr["CriteriaName"] = Convert.ToString(ds1.Tables[1].Rows[i1]["criterianame"]);
                        dr["WeightagePercentage"] = Convert.ToString(ds1.Tables[1].Rows[i1]["weightage_percentage"]);
                        dt.Rows.Add(dr);
                    }
                }
                int tb1 = ds1.Tables[0].Rows.Count;
                int tb2 = ds1.Tables[1].Rows.Count;
                if (tb1 > tb2)
                {
                    int tb = tb1 - tb2;
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        if (i < tb)
                        {
                            dr = dt.NewRow();
                            dr["CO"] = string.Empty;
                            dr["Subject"] = string.Empty;
                            dr["Test"] = string.Empty;
                            dt.Rows.Add(dr);
                        }
                    }
                }

                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Visible = true;
                btnaddnew.Visible = true;
                btnsave.Visible = true;

            }
            else
            {
                divPopAlert.Visible = true;
                divPopAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Allot COSettings";
            }


        }
        catch
        {
        }
    }

    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlconame = (e.Row.FindControl("ddlconame") as DropDownList);
                DropDownList ddlsub = (e.Row.FindControl("ddlsubject") as DropDownList);

                string qry = "select distinct isnull(template,'') as co,masterno from Master_Settings where settings='COSettings'";

                string logstaffcode = "";
                if (Convert.ToString(Session["Staff_Code"]) != "")
                {
                    logstaffcode = " and st.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "'";
                }

                qry = qry + " select distinct S.subject_no,subject_name,s.subject_code,CONVERT(nvarchar(max),subject_code)+'-'+CONVERT(nvarchar(max),subject_name) as subName from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " and SM.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' " + logstaffcode + " order by S.subject_no ";
             //   qry = qry + " select distinct subject_name,s.subject_code,CONVERT(nvarchar(max),subject_code)+'-'+CONVERT(nvarchar(max),subject_name) as subName,s.subject_no from registration r,subject s,syllabus_master sm,Sub_sem Sem,degree d,course c  where d.degree_code=r.degree_code and d.course_id=c.course_id and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year and r.current_semester=sm.semester and sm.syll_code=s.syll_code  and  s.syll_code=SM.syll_code  and  S.subtype_no = Sem.subtype_no and promote_count=1 and c.college_code=" + ddlcollege.SelectedValue.ToString() + "  and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "'  and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar'  order by subject_name";
                qry = qry + " select c.criteria,c.Criteria_no from syllabus_master sm,CriteriaForInternal c where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and c.syll_code=sm.syll_code";
                DataSet ds2 = d2.select_method_wo_parameter(qry, "text");
                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    ddlconame.DataSource = ds2.Tables[0];
                    ddlconame.DataTextField = "co";
                    ddlconame.DataValueField = "masterno";
                    ddlconame.DataBind();
                    ddlconame.Items.Insert(0, "--Select--");
                }
                if (ds2.Tables[1].Rows.Count > 0)
                {
                    ddlsub.DataSource = ds2.Tables[1];
                    ddlsub.DataTextField = "subName";
                    ddlsub.DataValueField = "subject_no";
                    ddlsub.DataBind();
                    ddlsub.Items.Insert(0, "--Select--");
                }
                if (ds2.Tables[2].Rows.Count > 0)
                {

                    CheckBoxList atttype = (e.Row.FindControl("cbltest1") as CheckBoxList);//e.Row.RowIndex
                    CheckBox chk = (e.Row.FindControl("chktest1") as CheckBox);
                    TextBox txtBox = (e.Row.FindControl("txttest1") as TextBox);
                    atttype.Items.Clear();
                    atttype.DataSource = ds2.Tables[2];
                    atttype.DataTextField = "criteria";
                    atttype.DataValueField = "Criteria_no";
                    atttype.DataBind();
                    //checkBoxListselectOrDeselect(atttype, false);
                    CallCheckboxListChange(chk, atttype, txtBox, "Test", "--Select--");

                }

                string[] spt = null;
                bool sptflag = false;

                Label lbco = e.Row.FindControl("lblconame") as Label;
                string cono1 = lbco.Text;
                if (string.IsNullOrEmpty(cono1))
                    ddlconame.Items[0].Selected = true;
                else
                    ddlconame.Items.FindByValue(cono1).Selected = true;



                Label lblsub = e.Row.FindControl("lblsubject") as Label;
                string sub = lblsub.Text;
                if (string.IsNullOrEmpty(sub))
                    ddlsub.Items[0].Selected = true;
                else
                    ddlsub.Items.FindByValue(sub).Selected = true;

                Label lbtst = e.Row.FindControl("lbltest") as Label;
                string tst = lbtst.Text;
                if (tst.Contains(","))
                {
                    spt = tst.Split(',');
                    sptflag = true;
                }

                CheckBoxList atttype1 = (e.Row.FindControl("cbltest1") as CheckBoxList);
                if (sptflag == true)
                {

                    for (int k = 0; k < spt.Length; k++)
                    {
                        string val = Convert.ToString(spt[k]);
                        atttype1.Items.FindByValue(val).Selected = true;
                    }
                }
                else
                {
                    atttype1.Items.FindByValue(tst).Selected = true;
                }



                CheckBoxList atttype11 = (e.Row.FindControl("cbltest1") as CheckBoxList);//e.Row.RowIndex
                CheckBox chk1 = (e.Row.FindControl("chktest1") as CheckBox);
                TextBox txtBox1 = (e.Row.FindControl("txttest1") as TextBox);
                CallCheckboxListChange(chk1, atttype11, txtBox1, "Test", "--Select--");
                // CallCheckboxChange(chk1, atttype11, txtBox1, "Test", "--Select--");


            }
        }
        catch
        {
        }
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

    protected void chktest_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ddlLabTest = (CheckBox)sender;
            var row = ddlLabTest.NamingContainer;
            string rowIndxS = ddlLabTest.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            //string colIndxS = ddlLabTest.UniqueID.ToString().Split('$')[4].Replace("chktest", string.Empty);
            //int colIndx = Convert.ToInt32(colIndxS);
            CheckBox ddlAddLabTestShortName = GridView1.Rows[rowIndx].FindControl("chktest1") as CheckBox;
            CheckBoxList cbl = GridView1.Rows[rowIndx].FindControl("cbltest1") as CheckBoxList;
            TextBox txtB = GridView1.Rows[rowIndx].FindControl("txttest1") as TextBox;
            CallCheckboxChange(ddlAddLabTestShortName, cbl, txtB, "Test", "--Select--");
        }
        catch
        {
        }
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
        catch { }
    }

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    protected void btnsave_OnClick(object sender, EventArgs e)
    {
        try
        {
            int inst = 0;

            string criteriano = string.Empty;

            foreach (GridViewRow gr in GridView1.Rows)
            {
                criteriano = string.Empty;
                DropDownList ddlconame = gr.FindControl("ddlconame") as DropDownList;
                DropDownList ddlsubject1 = gr.FindControl("ddlsubject") as DropDownList;
                CheckBoxList cbltst = gr.FindControl("cbltest1") as CheckBoxList;
                string subno = ddlsubject1.SelectedValue.ToString();
                string conum = ddlconame.SelectedValue.ToString();
                string coname = ddlconame.SelectedItem.Text.ToString();
                if (cbltst.Items.Count > 0)
                {
                    for (int i = 0; i < cbltst.Items.Count; i++)
                    {
                        if (cbltst.Items[i].Selected == true)
                        {
                            if (string.IsNullOrEmpty(criteriano))
                                criteriano = cbltst.Items[i].Value.ToString();
                            else
                                criteriano = criteriano + "," + cbltst.Items[i].Value.ToString();

                        }
                    }
                }
                TextBox txtcriterianame = gr.FindControl("txtcriterianame") as TextBox;
                TextBox percent = gr.FindControl("txtweightper") as TextBox;

                string criterianame = txtcriterianame.Text;
                string wgpercent = percent.Text;
                string qry = string.Empty;
               
                if (!string.IsNullOrEmpty(wgpercent) && !string.IsNullOrEmpty(criterianame) && !string.IsNullOrEmpty(criteriano))
                {
                    
                   // int count = 0;
                    int del = dir.deleteData("delete weightage_setting where batch='" + ddlbatch.SelectedItem.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and cono='" + conum + "' and criteria_no='" + criteriano + "' and subject_no='" + subno + "'");

                    qry = "if exists(select * from weightage_setting where batch='" + ddlbatch.SelectedItem.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and cono='" + conum + "' and subject_no='" + subno + "' and criteria_no='" + criteriano + "') update weightage_setting set weightage_percentage='" + wgpercent + "',criterianame='" + criterianame + "' where batch='" + ddlbatch.SelectedItem.Text.ToString() + "'  and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and cono='" + conum + "' and subject_no='" + subno + "' and criteria_no='" + criteriano + "' else insert into weightage_setting(batch,degree_code,semester,coname,cono,subject_no,criteria_no,criterianame,weightage_percentage)values('" + ddlbatch.SelectedItem.Text.ToString() + "','" + ddlbranch.SelectedValue.ToString() + "','" + ddlsemester.SelectedItem.Text.ToString() + "' ,'" + coname + "','" + conum + "','" + subno + "','" + criteriano + "','" + criterianame + "','" + wgpercent + "')";
                    inst = d2.update_method_wo_parameter(qry, "text");


                }
                //else
                //{
                //    divPopAlert.Visible = true;
                //    divPopAlertContent.Visible = true;
                //    lblAlertMsg.Visible = true;
                //    lblAlertMsg.Text = "Please Enter All Details";
                //    return;
                //}
            }
            if (inst > 0)
            {
                divPopAlert.Visible = true;
                divPopAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Saved Successfully";
            }
            else
            {
                divPopAlert.Visible = true;
                divPopAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Not Saved";
            }
        }
        catch
        {
        }
    }

    protected void cbltest1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBoxList grids = (CheckBoxList)sender;
            string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxSs) - 2;

            CheckBoxList atttype = (GridView1.Rows[rowIndx].FindControl("cbltest1") as CheckBoxList);//e.Row.RowIndex
            CheckBox chk = (GridView1.Rows[rowIndx].FindControl("chktest1") as CheckBox);
            TextBox txtBox = (GridView1.Rows[rowIndx].FindControl("txttest1") as TextBox);

            CallCheckboxListChange(chk, atttype, txtBox, "Test", "--Select--");
        }
        catch
        {
        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        divPopAlert.Visible = false;
        divPopAlertContent.Visible = false;
    }



    protected void btnaddnewrow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dt_newrow = new DataTable();
            DataRow dr = null;

            dt_newrow.Columns.Add("CO");
            dt_newrow.Columns.Add("Subject");
            dt_newrow.Columns.Add("Test");
            dt_newrow.Columns.Add("CriteriaName");
            dt_newrow.Columns.Add("WeightagePercentage");

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                dr = dt_newrow.NewRow();

                DropDownList conam = (DropDownList)GridView1.Rows[i].Cells[0].FindControl("ddlconame");
                string coname = conam.Text;
                dr["CO"] = coname;

                DropDownList sub = (DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlsubject");
                string subj = sub.Text;
                dr["Subject"] = subj;

                Label tst = (Label)GridView1.Rows[i].Cells[2].FindControl("lbltest");
                string test = tst.Text;
                dr["Test"] = test;
                Label critnam = (Label)GridView1.Rows[i].Cells[2].FindControl("lblcriterianame");
                string criterianame = critnam.Text;
                dr["CriteriaName"] = criterianame;
                Label weitper = (Label)GridView1.Rows[i].Cells[2].FindControl("lblweightper");
                string weightper = weitper.Text;
                dr["WeightagePercentage"] = weightper;

                dt_newrow.Rows.Add(dr);
            }
            ViewState["CurrentTable"] = dt_newrow;


            dr = dt_newrow.NewRow();

            dr["CO"] = "";
            dr["Subject"] = "";
            dr["Test"] = "";
            dr["CriteriaName"] = "";
            dr["WeightagePercentage"] = "";

            dt_newrow.Rows.Add(dr);

            GridView1.DataSource = dt_newrow;
            GridView1.DataBind();
        }
        catch
        {
        }
    }

    //protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        for (int i = 1; i < e.Row.Cells.Count; i++)
    //        {
    //            TableCell cell = e.Row.Cells[i];
    //            cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
    //            cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
    //            cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
    //               , SelectedGridCellIndex.ClientID, i
    //               , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
    //        }
    //    }
    //}

    protected void ddlsubject_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList grids = (DropDownList)sender;
            string rowIndxSs = grids.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxSs) - 2;
            //    int  rowindexs = rowIndx;
            DropDownList ddl = GridView1.Rows[rowIndx].FindControl("ddlsubject") as DropDownList;
            string subno = Convert.ToString(ddl.SelectedValue);

            DropDownList ddlco = GridView1.Rows[rowIndx].FindControl("ddlconame") as DropDownList;
            string cono = Convert.ToString(ddlco.SelectedValue);

            CheckBoxList atttype = (GridView1.Rows[rowIndx].FindControl("cbltest1") as CheckBoxList);//e.Row.RowIndex
            CheckBox chk = (GridView1.Rows[rowIndx].FindControl("chktest1") as CheckBox);
            TextBox txtBox = (GridView1.Rows[rowIndx].FindControl("txttest1") as TextBox);
            atttype.Items.Clear();

            string critno = "select distinct c.Criteria_no,c.criteria from CAQuesSettingsParent ca,criteriaforInternal c  where ca.CourseOutComeNo in('" + cono + "')  and ca.subjectno in('" + subno + "') and c.criteria_no=ca.criteriano ";
            DataSet dss = d2.select_method_wo_parameter(critno, "text");
            if (dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
            {
                atttype.Items.Clear();
                atttype.DataSource = dss.Tables[0];
                atttype.DataTextField = "criteria";
                atttype.DataValueField = "Criteria_no";
                atttype.DataBind();
                checkBoxListselectOrDeselect(atttype, true);
                CallCheckboxListChange(chk, atttype, txtBox, "Test", "--Select--");
            }


        }
        catch
        {
        }
    }
}