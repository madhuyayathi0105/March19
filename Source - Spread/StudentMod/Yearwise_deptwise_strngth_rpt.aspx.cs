using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using Gios.Pdf;
using System.Text.RegularExpressions;

public partial class Yearwise_deptwise_strngth_rpt : System.Web.UI.Page
{

    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;
    int count = 0;
    int spancount = 0;
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    Hashtable hat = new Hashtable();
    Hashtable hasyears = new Hashtable();
    DataTable dtshow = new DataTable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    static int rowadd = 0;
    static Hashtable hatsrow = new Hashtable();
    DataSet studgradeds = new DataSet();
    ArrayList avoidcol = new ArrayList();
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
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
                setLabelText();
                cklgender.Items.Add("Male");
                cklgender.Items.Add("Female");
                cklgender.Items.Add("Transgender");
                final.Visible = false;
                loadtype();
                Bindcollege();
                collegecode = ddlcollege.SelectedValue.ToString();
                BindBatch();
                // BindDegree(singleuser, group_user, collegecode, usercode);
                bindgroup();
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                FpSpread1.Visible = false;
            }
        }
        catch (Exception ex) { }
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

        lbl.Add(Label1);

        lbl.Add(lblbranch);

        fields.Add(0);


        fields.Add(3);


        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }
    //protected void btngo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //DataRow dtr = new DataRow();
    //        hide();
    //        lblerrormsg.Visible = true;
    //        DataView dv = new DataView();
    //        ArrayList arrgender = new ArrayList();
    //        DataTable dtshow = new DataTable();
    //        dtshow.Columns.Add("Stream");
    //        string sql = "select distinct Edu_Level from  course order by Edu_Level desc";
    //        ds.Clear();
    //        ds = da.select_method_wo_parameter(sql, "Text");
    //        for (int i = 0; i < cklgender.Items.Count; i++)
    //        {
    //            if (cklgender.Items[i].Selected == true)
    //            {
    //                arrgender.Add(i);
    //            }
    //        }
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {

    //                //if (cklgender.Items[0].Selected == true)
    //                //{
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-M");
    //                //}
    //                //else if (cklgender.Items[1].Selected == true)
    //                //{
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-W");
    //                //}
    //                //else if (cklgender.Items[2].Selected == true)
    //                //{
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-TR");
    //                //}
    //                //else if (cklgender.Items[0].Selected == true && cklgender.Items[1].Selected == true)
    //                //{
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-M");
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-W");

    //                //}
    //                //else if (cklgender.Items[1].Selected == true && cklgender.Items[2].Selected == true)
    //                //{
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-W");
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-TR");
    //                //}
    //                //else if (cklgender.Items[0].Selected == true && cklgender.Items[2].Selected == true)
    //                //{
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-M");
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-TR");
    //                //}
    //                //else if (cklgender.Items[0].Selected == true && cklgender.Items[1].Selected == true && cklgender.Items[2].Selected == true)
    //                //{
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-M");
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-W");
    //                //    dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-TR");
    //                //}

    //                for (int ii = 0; ii < arrgender.Count; ii++)
    //                {
    //                    if (arrgender[ii].ToString() == "0")
    //                    {
    //                        dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-M");
    //                    }
    //                    if (arrgender[ii].ToString() == "1")
    //                    {
    //                        dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-W");
    //                    }
    //                    if (arrgender[ii].ToString() == "2")
    //                    {
    //                        dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-TR");
    //                    }



    //                }
    //                dtshow.Columns.Add("" + ds.Tables[0].Rows[i][0].ToString() + "-Total");
    //            }
    //        }
    //        ArrayList stream11 = new ArrayList();
    //        DataTable tblstream11 = new DataTable();
    //        tblstream11.Columns.Add("Deptname");
    //        tblstream11.Columns.Add("row");
    //      string edullev="";
    //        if (dtshow.Columns.Count > 0)
    //        {
    //            for (int ii = 1; ii < dtshow.Columns.Count; ii++)
    //            {

    //                string edulevel = dtshow.Columns[ii].ToString();

    //                string[] spitedulevel = edulevel.Split('-');
    //                if (spitedulevel.Length == 2)
    //                {
    //                    edulevel = spitedulevel[0].ToString();
    //                    if (ii == 1)
    //                    {
    //                        edullev = edulevel;
    //                    }
    //                    string sex = spitedulevel[1].ToString();
    //                    if(sex=="M")
    //                    {
    //                        sex="0";
    //                    }
    //                    else if(sex=="W")
    //                    {
    //                        sex="1";
    //                    }
    //                    else
    //                    {
    //                        sex="2";
    //                    }
    //                    sql = "SELECT dept_name,Edu_Level,case when sex = 0 THEN 'M' when sex = 1 Then 'W' ELSE 'TR' END Sex,COUNT(*) Tot FROM Registration R,applyn A,Degree G,Course C,Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and Edu_Level='" + edulevel + "' and sex='" + sex + "' and type='"+ddltype.SelectedItem.Text.ToString()+"'  group by G.dept_code,dept_name,Edu_Level,sex  order by Edu_Level desc,Dept_Name,sex";
    //                    ds.Clear();
    //                    ds = da.select_method_wo_parameter(sql, "Text");


    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {

    //                        //ds.Tables[0].DefaultView.RowFilter = "sex='" + sex + "'";
    //                        //dv = ds.Tables[0].DefaultView;
    //                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //                        {
    //                            if (!stream11.Contains(ds.Tables[0].Rows[i]["dept_name"].ToString()))
    //                            {
    //                                stream11.Add(ds.Tables[0].Rows[i]["dept_name"].ToString());
    //                                DataRow dtr = dtshow.NewRow();
    //                                dtr[0] = ds.Tables[0].Rows[i]["dept_name"].ToString();
    //                                dtr[ii] = ds.Tables[0].Rows[i]["Tot"].ToString();
    //                                dtshow.Rows.Add(dtr);
    //                                tblstream11.Rows.Add(ds.Tables[0].Rows[i]["dept_name"].ToString(),i);
    //                            }
    //                            else
    //                            {
    //                                tblstream11.DefaultView.RowFilter = "Deptname='" + ds.Tables[0].Rows[i]["dept_name"].ToString() + "'";
    //                                dv = tblstream11.DefaultView;
    //                                if (dv.Count>0)
    //                                {
    //                                    string row = dv[0][1].ToString();
    //                                    dtshow.Rows[Convert.ToInt32(row)][ii] = ds.Tables[0].Rows[i]["Tot"].ToString();
    //                                }
    //                                //string row = stream11[ds.Tables[0].Rows[i]["dept_name"].ToString()].ToString();
    //                                //dtshow.Rows[Convert.ToInt32(row)][ii] = ds.Tables[0].Rows[i]["Tot"].ToString();
    //                                //dtshow.Rows[Convert.ToInt32()][
    //                                //dtr[0] = ds.Tables[0].Rows[i]["Dept_Code"].ToString();
    //                                //dtr[ii] = ds.Tables[0].Rows[i]["Tot"].ToString();
    //                                //dtshow.Rows.Add(dtr);
    //                            }
    //                        }
    //                    }
    //                }
    //                if (edullev != edulevel)
    //                {
    //                    DataRow dtr = dtshow.NewRow();
    //                                dtr[0] = "Total";
    //                                dtshow.Rows.Add(dtr);
    //                }
    //            }
    //        }


    //    }


    //    catch
    //    {
    //    }
    //}



    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Text = "";
            dtshow.Columns.Add("UG/PG");
            dtshow.Columns.Add("M/W/TR");
            dtshow.Columns.Add("colno");


            hide();
            lblerrormsg.Visible = true;
            int count = 0;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Batch";
                hide();
                lblerrormsg.Visible = true;
                return;


            }
            else
            {
                lblerrormsg.Text = "";

            }
            count = 0;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Group";
                hide();
                lblerrormsg.Visible = true;
                return;


            }
            else
            {
                lblerrormsg.Text = "";

            }
            count = 0;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Branch";
                hide();
                lblerrormsg.Visible = true;
                return;


            }
            else
            {
                lblerrormsg.Text = "";

            }
            count = 0;
            for (int i = 0; i < cklgender.Items.Count; i++)
            {
                if (cklgender.Items[i].Selected == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Gender";
                hide();
                lblerrormsg.Visible = true;
                return;


            }
            else
            {
                lblerrormsg.Text = "";

            }

            if (ddlrpttype.SelectedIndex == 0)
            {
                bindheader();
                bindvalue();
            }
            else if (ddlrpttype.SelectedIndex == 1)
            {
                bindheaderyearwise();
                bindvalueyearwise();
            }

            // bindvalue();
            if (FpSpread1.Sheets[0].Rows.Count > 0)
            {
                final.Visible = true;
            }

            for (int i = 0; i < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; i++)
            {
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].ForeColor = Color.Black;
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].VerticalAlign = VerticalAlign.Middle;

                FpSpread1.Sheets[0].Columns[i].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[i].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[i].Font.Bold = true;
                FpSpread1.Sheets[0].Columns[i].ForeColor = Color.Black;
                if (i >= 4)
                {
                    FpSpread1.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[i].VerticalAlign = VerticalAlign.Middle;
                }
                FpSpread1.Sheets[0].Columns[i].Locked = true;

            }


        }


        catch
        {
        }
    }
    public void bindvalue()
    {
        DataView dv = new DataView();
        string type = "";
        if (ddltype.Items.Count > 0)
        {
            type = ddltype.SelectedItem.Text.ToString();
        }
        string sex = "";

        ArrayList avoirows = new ArrayList();
        //Hashtable avoirows = new Hashtable();
        string edulevelid = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                if (edulevelid == "")
                {

                    edulevelid = ds.Tables[0].Rows[i][0].ToString();
                }
                else
                {

                    edulevelid = edulevelid + "','" + ds.Tables[0].Rows[i][0].ToString();
                }

            }
        }

        string courseid = "";
        string deptid = "";
        string courid = "";
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                string spl = chklstbranch.Items[i].Value.ToString();
                string[] cours = spl.Split('-');

                if (deptid == "")
                {
                    // deptid = chklstbranch.Items[i].Value.ToString();
                    deptid = cours[0];
                    courid = cours[1];
                }
                else
                {
                    //  deptid = deptid + "','" + chklstbranch.Items[i].Value.ToString();
                    deptid = deptid + "','" + cours[0];
                    courid = courid + "','" + cours[1];
                }
            }
        }
        for (int i = 0; i < chklstdegree.Items.Count; i++)
        {
            if (chklstdegree.Items[i].Selected == true)
            {
                courseid = chklstdegree.Items[i].Value.ToString();
                //magesh 25.8.18
                //string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in ('" + deptid + "')";//(select Dept_Code from Department where dept_name in ('" + deptid + "'))";
                string sql = "  select distinct  c.course_name+'-'+ dt.dept_name as dept_name,dt.dept_acronym,dt.dept_name as dept_name1,c.Course_Id from tbl_DeptGrouping tb,course c,Degree d,Department dt where d.Dept_Code=dt.Dept_Code  and  d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'  and Groupcode in ('" + courseid + "') and d.Dept_Code in ('" + deptid + "') and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.Course_Id in('" + courid + "')"; //magesh 25.8.18
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        if (cbacr.Checked)
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_acronym"].ToString();
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_name"].ToString();
                        }
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["Deptcode"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["dept_name1"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[ii]["Course_Id"].ToString();

                    }
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;
                    //avoirows.Add(FpSpread1.Sheets[0].RowCount - 1, courseid);
                    avoirows.Add(FpSpread1.Sheets[0].RowCount - 1);
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
                }
            }
        }
        string batchyearselected = "";
        for (int i = 0; i < chklsbatch.Items.Count; i++)
        {
            if (chklsbatch.Items[i].Selected == true)
            {
                if (batchyearselected == "")
                {
                    batchyearselected = chklsbatch.Items[i].Value.ToString();
                }
                else
                {
                    batchyearselected = batchyearselected + "','" + chklsbatch.Items[i].Value.ToString();
                }

            }
        }
        ArrayList totgt = new ArrayList();
        double ugpgtotal = 0;
        double endugpgtotal = 0;
        double overallugpgtotal = 0;
        //magesh 25.8.18
        // string sqlnew = "SELECT d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and  Course_Name not like '%M.phil%' group by d.Dept_Code,dept_name,Edu_Level,sex order by d.Dept_Code   ";


        string sqlnew = "SELECT d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,c.course_id ,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and  Course_Name not like '%M.phil%' group by d.Dept_Code,dept_name,Edu_Level,sex,c.course_id  order by d.Dept_Code   ";
        //magesh 25.8.18
        ds.Clear();
        int countd = 0;

        ds = da.select_method_wo_parameter(sqlnew, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                if (!avoirows.Contains(i))
                {
                    for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
                    {
                        if (!avoidcol.Contains(j))
                        {
                            edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                            sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
                            deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();

                            //magesh 25.8.18  
                            string cor = FpSpread1.Sheets[0].Cells[i, 0].Note.ToString(); //magesh 25.8.18
                            //  string filter = "dept_name='" + deptid + "' and sex='" + sex + "' and Edu_Level='" + edulevelid + "'";
                            string filter = "dept_name='" + deptid + "' and sex='" + sex + "' and Edu_Level='" + edulevelid + "' and  course_id='" + cor + "'";
                            if (sex.ToUpper().Trim() != "TOTAL")
                            {
                                ds.Tables[0].DefaultView.RowFilter = filter;
                                dv = ds.Tables[0].DefaultView;
                                int coun = 0;
                                if (dv.Count > 0)
                                {
                                    for (int m = 0; m < dv.Count; m++)
                                    {
                                        coun = coun + Convert.ToInt32(dv[m]["total"]);
                                        FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(coun);
                                        ugpgtotal = ugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());
                                        // overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

                                    }


                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = "--";

                                }
                            }
                            else
                            {
                                if (!totgt.Contains(j))
                                {
                                    totgt.Add(j);
                                }
                                FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
                                ugpgtotal = 0;

                            }
                            FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                        }
                    }
                    //FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
                    //overallugpgtotal = 0;
                }


            }

        }


        sqlnew = "SELECT d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and  Course_Name  like '%M.phil%' group by d.Dept_Code,dept_name,Edu_Level,sex order by d.Dept_Code   ";
        ds.Clear();
        int countds = 0;
        ds = da.select_method_wo_parameter(sqlnew, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                if (!avoirows.Contains(i))
                {


                    for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
                    {
                        if (avoidcol.Contains(j))
                        {
                            edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                            sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
                            deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                            string filter = "dept_name='" + deptid + "' and sex='" + sex + "' and Edu_Level='" + edulevelid + "'";
                            if (sex.ToUpper().Trim() != "TOTAL")
                            {
                                ds.Tables[0].DefaultView.RowFilter = filter;
                                dv = ds.Tables[0].DefaultView;
                                int coun = 0;
                                if (dv.Count > 0)
                                {
                                    for (int m = 0; m < dv.Count; m++)
                                    {
                                        coun = coun + Convert.ToInt32(dv[m]["total"]);
                                        FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(coun);
                                        ugpgtotal = ugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());
                                        // overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

                                    }


                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = "--";

                                }
                            }
                            else
                            {
                                if (!totgt.Contains(j))
                                {
                                    totgt.Add(j);
                                }
                                FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
                                ugpgtotal = 0;

                            }
                            FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                        }
                    }
                    //FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
                    //overallugpgtotal = 0;
                }


            }

        }
        int outnum = 0;
        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
        {
            for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
            {
                if (totgt.Contains(j))
                {
                    string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
                    if (Int32.TryParse(tt, out outnum))
                    {

                        endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

                    }
                }
            }
            FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(endugpgtotal);
            //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
            endugpgtotal = 0;
            FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
        }

        overallugpgtotal = 0;
        FpSpread1.Sheets[0].Rows.Count++;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
        FpSpread1.SaveChanges();
        for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
            {
                if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "TOTAL")
                {


                    string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
                    if (Int32.TryParse(tt, out outnum))
                    {
                        overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
                        endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

                    }
                }
                else
                {
                    FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
                    //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
                    endugpgtotal = 0;
                    FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                }

            }
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
            overallugpgtotal = 0;

        }


        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        FpSpread1.SaveChanges();

    }

    //Jaya Kumar 30.8.18
    //public void bindvalueyearwise()
    //{
    //    DataView dv = new DataView();
    //    string type = "";
    //    if (ddltype.Items.Count > 0)
    //    {
    //        type = ddltype.SelectedItem.Text.ToString();
    //    }
    //    string sex = "";

    //    ArrayList avoirows = new ArrayList();
    //    //Hashtable avoirows = new Hashtable();
    //    string edulevelid = "";
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {

    //            if (edulevelid == "")
    //            {

    //                edulevelid = ds.Tables[0].Rows[i][0].ToString();
    //            }
    //            else
    //            {

    //                edulevelid = edulevelid + "','" + ds.Tables[0].Rows[i][0].ToString();
    //            }

    //        }
    //    }

    //    string courseid = "";
    //    string deptid = "";
    //    string courid = "";
    //    for (int i = 0; i < chklstbranch.Items.Count; i++)
    //    {
    //        if (chklstbranch.Items[i].Selected == true)
    //        {
    //            string spl = chklstbranch.Items[i].Value.ToString();
    //            string[] cours=spl.Split('-');

    //            if (deptid == "")
    //            {
    //               // deptid = chklstbranch.Items[i].Value.ToString();
    //                deptid = cours[0];
    //                courid = cours[1];
    //            }
    //            else
    //            {
    //              //  deptid = deptid + "','" + chklstbranch.Items[i].Value.ToString();
    //                deptid = deptid + "','" + cours[0];
    //                courid = courid + "','" + cours[1];
    //            }
    //        }



    //    }
    //    for (int i = 0; i < chklstdegree.Items.Count; i++)
    //    {
    //        if (chklstdegree.Items[i].Selected == true)
    //        {
    //            courseid = chklstdegree.Items[i].Value.ToString();

    //            string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.Edu_Level='UG' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "')  and Dept_Code in (select Dept_Code from Department where Dept_Code in ('" + deptid + "'))";
    //            ds.Clear();
    //            ds = da.select_method_wo_parameter(sql, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
    //                {
    //                    FpSpread1.Sheets[0].Rows.Count++;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_name"].ToString();
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["Deptcode"].ToString();
    //                }
    //                FpSpread1.Sheets[0].Rows.Count++;
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
    //                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;
    //                //avoirows.Add(FpSpread1.Sheets[0].RowCount - 1, courseid);
    //                avoirows.Add(FpSpread1.Sheets[0].RowCount - 1);
    //                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
    //            }
    //        }
    //    }



    //    string batchyearselected = "";
    //    for (int i = 0; i < chklsbatch.Items.Count; i++)
    //    {
    //        if (chklsbatch.Items[i].Selected == true)
    //        {
    //            if (batchyearselected == "")
    //            {
    //                batchyearselected = chklsbatch.Items[i].Value.ToString();
    //            }
    //            else
    //            {
    //                batchyearselected = batchyearselected + "','" + chklsbatch.Items[i].Value.ToString();
    //            }

    //        }
    //    }


    //    //string edulevelid = "";
    //    //if (ds.Tables[0].Rows.Count > 0)
    //    //{
    //    //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //    //    {
    //    //        if (edulevelid == "")
    //    //        {

    //    //            edulevelid = ds.Tables[0].Rows[i][0].ToString();
    //    //        }
    //    //        else
    //    //        {

    //    //            edulevelid = edulevelid + "','" + ds.Tables[0].Rows[i][0].ToString();
    //    //        }

    //    //    }
    //    //}

    //    //string sql = " select distinct d.Dept_Code,dd.Dept_Name from Degree d,course c,Department dd where d.Course_Id=c.Course_Id and d.Dept_Code=dd.Dept_Code and   c.type='" + type + "' and d.Dept_Code in ('" + deptid + "')   and d.Course_Id in ('" + courseid + "') order by dd.Dept_Name,Dept_Code ";
    //    //ds.Clear();
    //    //ds = da.select_method_wo_parameter(sql, "Text");
    //    //if (ds.Tables[0].Rows.Count > 0)
    //    //{

    //    //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //    //    {
    //    //        // string ugpg = FpSpread1.Sheets[0].ColumnHeader.Columns.Count;

    //    //        FpSpread1.Sheets[0].Rows.Count++;
    //    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[i]["dept_name"].ToString();
    //    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["Dept_Code"].ToString();



    //    //    }

    //    //}

    //    double ugpgtotal = 0;
    //    double endugpgtotal = 0;
    //    double overallugpgtotal = 0;
    //    string sqlnew = "SELECT r.Current_Semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and c.Edu_Level='UG' group by r.Current_Semester,d.Dept_Code,dept_name,Edu_Level,sex order by d.Dept_Code ";
    //    ds.Clear();
    //    ds = da.select_method_wo_parameter(sqlnew, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //        {
    //            if (!avoirows.Contains(i))
    //            {

    //                for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
    //                {
    //                    //edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
    //                    string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
    //                    if (chckyear.Trim() == "I")
    //                    {
    //                        chckyear = "  and Current_Semester >=1 and Current_Semester <=2";
    //                    }
    //                    if (chckyear.Trim() == "II")
    //                    {
    //                        chckyear = "  and Current_Semester >=3 and Current_Semester <=4";
    //                    }
    //                    if (chckyear.Trim() == "III")
    //                    {
    //                        chckyear = "  and Current_Semester >=5 and Current_Semester <=6";
    //                    }
    //                    if (chckyear.Trim() == "IV")
    //                    {
    //                        chckyear = "  and Current_Semester >=7 and Current_Semester <=8";
    //                    }
    //                    sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
    //                    string deptidnew = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
    //                    string filter = "Dept_Code='" + deptidnew + "' and sex='" + sex + "' and Edu_Level='UG' " + chckyear + "";
    //                    if (sex.ToUpper().Trim() != "TOTAL")
    //                    {

    //                        ds.Tables[0].DefaultView.RowFilter = filter;
    //                        dv = ds.Tables[0].DefaultView;
    //                        double cun = 0.00;
    //                        if (dv.Count > 0)
    //                        {
    //                            for (int m = 0; m < dv.Count; m++)
    //                            {
    //                                cun = cun + Convert.ToDouble(dv[m]["total"].ToString());
    //                                FpSpread1.Sheets[0].Cells[i, j].Text =Convert.ToString(cun);
    //                               // FpSpread1.Sheets[0].Cells[i, j].Text = dv[m]["total"].ToString();
    //                                ugpgtotal = ugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());
    //                                // overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

    //                            }


    //                        }
    //                        else
    //                        {
    //                            FpSpread1.Sheets[0].Cells[i, j].Text = "--";

    //                        }
    //                    }
    //                    else
    //                    {
    //                        FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
    //                        ugpgtotal = 0;

    //                    }
    //                    FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
    //                    FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
    //                }
    //                // FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
    //                // overallugpgtotal = 0;
    //            }


    //        }

    //    }
    //    // avoirows.Clear();
    //    int outnum = 0;
    //    overallugpgtotal = 0;
    //    FpSpread1.Sheets[0].Rows.Count++;
    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
    //    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

    //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
    //    FpSpread1.SaveChanges();
    //    for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
    //    {
    //        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
    //        {
    //            if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "TOTAL")
    //            {


    //                string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
    //                if (Int32.TryParse(tt, out outnum))
    //                {
    //                    overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
    //                    endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

    //                }
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
    //                //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
    //                endugpgtotal = 0;
    //                FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
    //                FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
    //            }

    //        }
    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
    //        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
    //        overallugpgtotal = 0;

    //    }

    //    FpSpread1.Sheets[0].Rows.Count++;
    //    ArrayList mphilcol = new ArrayList();
    //    int lastrowcount = FpSpread1.Sheets[0].Rows.Count;
    //    FpSpread1.Sheets[0].Cells[lastrowcount - 1, 0].Text = ddltype.SelectedItem.Text.ToString() + " / PG";
    //    FpSpread1.Sheets[0].Cells[lastrowcount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //    int mphilcolstart = 0;
    //    if (dtshow.Rows.Count > 0)
    //    {
    //        for (int cc = 0; cc < dtshow.Rows.Count; cc++)
    //        {
    //            FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].Text = dtshow.Rows[cc][1].ToString();
    //            if (cc == 2)
    //            {
    //                if (!mphilcol.Contains(dtshow.Rows[cc][0].ToString()))
    //                {
    //                    mphilcol.Add(dtshow.Rows[cc][0].ToString());
    //                    mphilcolstart = Convert.ToInt32(dtshow.Rows[cc][0].ToString());
    //                    for (int j = 0; j < cklgender.Items.Count; j++)
    //                    {
    //                        if (cklgender.Items[j].Selected == true)
    //                        {

    //                            mphilcol.Add(mphilcolstart);
    //                            mphilcolstart++;

    //                        }
    //                    }
    //                    mphilcol.Add(mphilcolstart);
    //                }


    //                FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].Text = "M.Phil";
    //            }
    //            FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].SpanModel.Add(lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString()), 1, spancount);
    //        }
    //    }
    //    FpSpread1.Sheets[0].Rows[lastrowcount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
    //    FpSpread1.SaveChanges();
    //    for (int i = 0; i < chklstdegree.Items.Count; i++)
    //    {
    //        if (chklstdegree.Items[i].Selected == true)
    //        {
    //            courseid = chklstdegree.Items[i].Value.ToString();

    //            // string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Deptcode in ('" + deptid + "')";

    //            string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.Edu_Level='PG' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_name in ('" + deptid + "'))";
    //            ds.Clear();
    //            ds = da.select_method_wo_parameter(sql, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
    //                {
    //                    FpSpread1.Sheets[0].Rows.Count++;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_name"].ToString();
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["Deptcode"].ToString();
    //                }
    //                FpSpread1.Sheets[0].Rows.Count++;
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
    //                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;
    //                //avoirows.Add(FpSpread1.Sheets[0].RowCount - 1, courseid);
    //                avoirows.Add(FpSpread1.Sheets[0].RowCount - 1);
    //                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
    //            }
    //        }
    //    }
    //    ugpgtotal = 0;
    //    endugpgtotal = 0;
    //    overallugpgtotal = 0;
    //    sqlnew = "SELECT r.Current_Semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and c.Edu_Level='PG'  and  c.Course_Name not like '%M.Phil%' group by r.Current_Semester,d.Dept_Code,dept_name,Edu_Level,sex order by d.Dept_Code ";
    //    ds.Clear();
    //    ds = da.select_method_wo_parameter(sqlnew, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = lastrowcount; i < FpSpread1.Sheets[0].RowCount; i++)
    //        {
    //            if (!avoirows.Contains(i))
    //            {

    //                for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
    //                {
    //                    if (!mphilcol.Contains(j))
    //                    {

    //                        //edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
    //                        string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
    //                        if (chckyear.Trim() == "I")
    //                        {
    //                            chckyear = "  and Current_Semester >=1 and Current_Semester <=2";
    //                        }
    //                        if (chckyear.Trim() == "II")
    //                        {
    //                            chckyear = "  and Current_Semester >=3 and Current_Semester <=4";
    //                        }
    //                        if (chckyear.Trim() == "III")
    //                        {
    //                            chckyear = "  and Current_Semester >=5 and Current_Semester <=6";
    //                        }
    //                        if (chckyear.Trim() == "IV")
    //                        {
    //                            chckyear = "  and Current_Semester >=7 and Current_Semester <=8";
    //                        }
    //                        sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
    //                        deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
    //                        string filter = "Dept_Code='" + deptid + "' and sex='" + sex + "' and Edu_Level='PG' " + chckyear + "";
    //                        if (sex.ToUpper().Trim() != "TOTAL")
    //                        {

    //                            ds.Tables[0].DefaultView.RowFilter = filter;
    //                            dv = ds.Tables[0].DefaultView;
    //                            if (dv.Count > 0)
    //                            {
    //                                for (int m = 0; m < dv.Count; m++)
    //                                {
    //                                    FpSpread1.Sheets[0].Cells[i, j].Text = dv[0]["total"].ToString();
    //                                    ugpgtotal = ugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());
    //                                    // overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

    //                                }


    //                            }
    //                            else
    //                            {
    //                                FpSpread1.Sheets[0].Cells[i, j].Text = "--";

    //                            }
    //                        }
    //                        else
    //                        {
    //                            FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
    //                            ugpgtotal = 0;

    //                        }
    //                        FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
    //                        FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
    //                    }
    //                }
    //                //FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
    //                overallugpgtotal = 0;
    //            }


    //        }

    //    }
    //    ugpgtotal = 0;
    //    endugpgtotal = 0;
    //    overallugpgtotal = 0;
    //    sqlnew = "SELECT r.Current_Semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and c.Edu_Level='PG'  and  c.Course_Name  like '%M.Phil%' group by r.Current_Semester,d.Dept_Code,dept_name,Edu_Level,sex order by d.Dept_Code ";
    //    ds.Clear();
    //    ds = da.select_method_wo_parameter(sqlnew, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = lastrowcount; i < FpSpread1.Sheets[0].RowCount; i++)
    //        {
    //            if (!avoirows.Contains(i))
    //            {

    //                for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
    //                {
    //                    if (mphilcol.Contains(j))
    //                    {

    //                        //edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
    //                        string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
    //                        chckyear = "  and Current_Semester >=1 and Current_Semester <=2";
    //                        sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
    //                        deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
    //                        string filter = "Dept_Code='" + deptid + "' and sex='" + sex + "' and Edu_Level='PG' " + chckyear + "";
    //                        if (sex.ToUpper().Trim() != "TOTAL")
    //                        {

    //                            ds.Tables[0].DefaultView.RowFilter = filter;
    //                            dv = ds.Tables[0].DefaultView;
    //                            if (dv.Count > 0)
    //                            {
    //                                for (int m = 0; m < dv.Count; m++)
    //                                {
    //                                    FpSpread1.Sheets[0].Cells[i, j].Text = dv[0]["total"].ToString();
    //                                    ugpgtotal = ugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());
    //                                    //overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

    //                                }


    //                            }
    //                            else
    //                            {
    //                                FpSpread1.Sheets[0].Cells[i, j].Text = "--";

    //                            }
    //                        }
    //                        else
    //                        {
    //                            FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
    //                            ugpgtotal = 0;

    //                        }
    //                        FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
    //                        FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
    //                    }
    //                }
    //                //FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
    //                overallugpgtotal = 0;
    //            }


    //        }

    //    }

    //    outnum = 0;
    //    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //    {
    //        for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
    //        {
    //            sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();

    //            if (sex.ToUpper().Trim() == "TOTAL" && (lastrowcount - 1) != i)
    //            {
    //                string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
    //                if (Int32.TryParse(tt, out outnum))
    //                {

    //                    endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

    //                }
    //            }

    //        }
    //        if (lastrowcount - 1 != i)
    //        {
    //            FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(endugpgtotal);
    //        }
    //        //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
    //        endugpgtotal = 0;
    //        FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
    //    }
    //    outnum = 0;
    //    overallugpgtotal = 0;
    //    endugpgtotal = 0;
    //    FpSpread1.Sheets[0].Rows.Count++;
    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
    //    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

    //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
    //    FpSpread1.SaveChanges();
    //    for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
    //    {
    //        for (int i = lastrowcount; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
    //        {
    //            if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "TOTAL")
    //            {


    //                string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
    //                if (Int32.TryParse(tt, out outnum))
    //                {
    //                    overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
    //                    endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

    //                }
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
    //                //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
    //                endugpgtotal = 0;
    //                FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
    //                FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
    //            }

    //        }
    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
    //        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
    //        overallugpgtotal = 0;

    //    }

    //    outnum = 0;
    //    overallugpgtotal = 0;
    //    endugpgtotal = 0;
    //    FpSpread1.Sheets[0].Rows.Count++;
    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Overall Grand Total";
    //    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

    //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
    //    FpSpread1.SaveChanges();
    //    for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
    //    {
    //        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
    //        {
    //            if (avoirows.Contains(i))
    //            {
    //                if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() == "TOTAL")
    //                {


    //                    string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
    //                    if (Int32.TryParse(tt, out outnum))
    //                    {
    //                        overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
    //                        //endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

    //                    }
    //                }
    //                else
    //                {
    //                    // FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
    //                    //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
    //                    endugpgtotal = 0;
    //                    FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
    //                    FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
    //                }
    //            }
    //        }
    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
    //        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
    //        overallugpgtotal = 0;

    //    }


    //    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
    //    FpSpread1.SaveChanges();



    //}
    //public void bindvalueold()
    //{
    //    DataView dv = new DataView();
    //    string type = "";
    //    if (ddltype.Items.Count > 0)
    //    {
    //        type = ddltype.SelectedItem.Text.ToString();
    //    }
    //    string sex = "";
    //    string edulevelsa = "";
    //    Hashtable hatgender = new Hashtable();
    //    ArrayList arrgender = new ArrayList();
    //    ArrayList arredulevel = new ArrayList();
    //    string courseid = "";
    //    for (int i = 0; i < chklstdegree.Items.Count; i++)
    //    {
    //        if (chklstdegree.Items[i].Selected == true)
    //        {
    //            if (courseid == "")
    //            {
    //                courseid = chklstdegree.Items[i].Value.ToString();
    //            }
    //            else
    //            {
    //                courseid = courseid + "','" + chklstdegree.Items[i].Value.ToString();
    //            }

    //        }
    //    }
    //    string deptid = "";
    //    for (int i = 0; i < chklstbranch.Items.Count; i++)
    //    {
    //        if (chklstbranch.Items[i].Selected == true)
    //        {
    //            if (deptid == "")
    //            {
    //                deptid = chklstbranch.Items[i].Value.ToString();
    //            }
    //            else
    //            {
    //                deptid = deptid + "','" + chklstbranch.Items[i].Value.ToString();
    //            }

    //        }
    //    }

    //    string batchyearselected = "";
    //    for (int i = 0; i < chklsbatch.Items.Count; i++)
    //    {
    //        if (chklsbatch.Items[i].Selected == true)
    //        {
    //            if (batchyearselected == "")
    //            {
    //                batchyearselected = chklsbatch.Items[i].Value.ToString();
    //            }
    //            else
    //            {
    //                batchyearselected = batchyearselected + "','" + chklsbatch.Items[i].Value.ToString();
    //            }

    //        }
    //    }

    //    for (int i = 0; i < cklgender.Items.Count; i++)
    //    {
    //        if (cklgender.Items[i].Selected == true)
    //        {
    //            if (i == 0)
    //            {

    //                hatgender.Add("M", 0);
    //            }
    //            else if (i == 1)
    //            {

    //                hatgender.Add("W", 1);
    //            }
    //            else
    //            {
    //                hatgender.Add("TR", 2);
    //            }

    //            arrgender.Add(i);
    //            if (sex == "")
    //            {
    //                sex = "" + i + "";
    //            }
    //            else
    //            {
    //                sex = sex + "','" + "" + i + "";
    //            }
    //        }
    //    }
    //    string edulevelid = "";
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            arredulevel.Add("[" + ds.Tables[0].Rows[i][0].ToString() + "]");
    //            if (edulevelsa == "")
    //            {
    //                edulevelsa = "[" + ds.Tables[0].Rows[i][0].ToString() + "]";
    //                edulevelid = ds.Tables[0].Rows[i][0].ToString();
    //            }
    //            else
    //            {
    //                edulevelsa = edulevelsa + "," + "[" + ds.Tables[0].Rows[i][0].ToString() + "]";
    //                edulevelid = edulevelid + "','" + ds.Tables[0].Rows[i][0].ToString();
    //            }

    //        }
    //    }

    //    string sql = " select distinct d.Dept_Code,dd.Dept_Name from Degree d,course c,Department dd where d.Course_Id=c.Course_Id and d.Dept_Code=dd.Dept_Code and   c.type='" + type + "' and d.Dept_Code in ('" + deptid + "')   and d.Course_Id in ('" + courseid + "') order by dd.Dept_Name,Dept_Code ";
    //    ds.Clear();
    //    ds = da.select_method_wo_parameter(sql, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {

    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            // string ugpg = FpSpread1.Sheets[0].ColumnHeader.Columns.Count;

    //            FpSpread1.Sheets[0].Rows.Count++;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[i]["dept_name"].ToString();
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["Dept_Code"].ToString();



    //        }

    //    }

    //    double ugpgtotal = 0;
    //    double overallugpgtotal = 0;
    //    sql = "SELECT d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') group by d.Dept_Code,dept_name,Edu_Level,sex order by d.Dept_Code ;SELECT upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code    and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "')   group by Edu_Level,sex   ";
    //    ds.Clear();
    //    ds = da.select_method_wo_parameter(sql, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //        {
    //            for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
    //            {
    //                edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
    //                sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
    //                deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
    //                string filter = "Dept_Code='" + deptid + "' and sex='" + sex + "' and Edu_Level='" + edulevelid + "'";
    //                if (sex.ToUpper().Trim() != "TOTAL")
    //                {

    //                    ds.Tables[0].DefaultView.RowFilter = filter;
    //                    dv = ds.Tables[0].DefaultView;
    //                    if (dv.Count > 0)
    //                    {
    //                        for (int m = 0; m < dv.Count; m++)
    //                        {
    //                            FpSpread1.Sheets[0].Cells[i, j].Text = dv[0]["total"].ToString();
    //                            ugpgtotal = ugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());
    //                            overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

    //                        }


    //                    }
    //                    else
    //                    {
    //                        FpSpread1.Sheets[0].Cells[i, j].Text = "0";

    //                    }
    //                }
    //                else
    //                {
    //                    FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
    //                    ugpgtotal = 0;

    //                }
    //                FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
    //                FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
    //            }
    //            FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
    //            overallugpgtotal = 0;
    //        }
    //    }
    //    if (FpSpread1.Sheets[0].Rows.Count > 0)
    //    {
    //        FpSpread1.Sheets[0].Rows.Count++;
    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
    //        FpSpread1.SaveChanges();
    //        FpSpread1.Visible = true;
    //        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

    //        overallugpgtotal = 0;
    //        ugpgtotal = 0;
    //        if (ds.Tables[1].Rows.Count > 0)
    //        {

    //            for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
    //            {
    //                edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
    //                sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
    //                string filter = "sex='" + sex + "' and Edu_Level='" + edulevelid + "'";
    //                if (sex.ToUpper().Trim() != "TOTAL")
    //                {

    //                    ds.Tables[1].DefaultView.RowFilter = filter;
    //                    dv = ds.Tables[1].DefaultView;
    //                    if (dv.Count > 0)
    //                    {
    //                        for (int m = 0; m < dv.Count; m++)
    //                        {
    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j].Text = dv[0]["total"].ToString();
    //                            ugpgtotal = ugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());
    //                            overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

    //                        }


    //                    }
    //                    else
    //                    {
    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j].Text = "0";

    //                    }
    //                }
    //                else
    //                {
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j].Text = Convert.ToString(ugpgtotal);
    //                    ugpgtotal = 0;

    //                }
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Center;
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j].VerticalAlign = VerticalAlign.Middle;
    //            }
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);


    //        }
    //    }

    //    //ArrayList arr = new ArrayList();

    //    //string sql = "SELECT * FROM ( SELECT d.Dept_Code,dept_name,Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,r.App_No FROM Registration R,applyn A,Degree G,Course C,Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and c.Course_Id in ('" + courseid + "') and CC=0 and DelFlag=0 and Exam_Flag='OK'  and r.Batch_Year in ('" + batchyearselected + "')) as s    PIVOT ( COUNT(App_No)  FOR [Edu_Level] IN (" + edulevelsa + "))AS PVTTable order by Dept_Name ,sex ";
    //    //sql = sql + "SELECT * FROM ( SELECT d.Dept_Code,dept_name,Edu_Level,r.App_No FROM Registration R,applyn A,Degree G,Course C,Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and c.Course_Id in ('" + courseid + "') and CC=0 and DelFlag=0 and Exam_Flag='OK'  and r.Batch_Year in ('" + batchyearselected + "')) as s    PIVOT ( COUNT(App_No)  FOR [Edu_Level] IN (" + edulevelsa + "))AS PVTTable order by Dept_Name";
    //    //ds.Clear();
    //    //ds = da.select_method_wo_parameter(sql, "Text");
    //    //if (ds.Tables[0].Rows.Count > 0)
    //    //{

    //    //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //    //    {
    //    //       // string ugpg = FpSpread1.Sheets[0].ColumnHeader.Columns.Count;
    //    //        if (!arr.Contains(ds.Tables[0].Rows[i]["Dept_Code"].ToString()))
    //    //        {
    //    //            arr.Add(ds.Tables[0].Rows[i]["Dept_Code"].ToString());
    //    //            FpSpread1.Sheets[0].Rows.Count++;
    //    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[i]["dept_name"].ToString();
    //    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["Dept_Code"].ToString();

    //    //        }

    //    //    }
    //    //    FpSpread1.SaveChanges();
    //    //    FpSpread1.Visible = true;
    //    //    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
    //    //}


    //}


    public void bindheader()
    {
        //dtshow.Columns.Add("UG/PG");
        //dtshow.Columns.Add("M/W/TR");
        //dtshow.Columns.Add("colno");
        int genderselcount = 0;
        for (int j = 0; j < cklgender.Items.Count; j++)
        {
            if (cklgender.Items[j].Selected == true)
            {
                genderselcount++;

            }
        }
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = System.Drawing.Color.Black;
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Border.BorderSize = 1;
        darkstyle.Border.BorderColor = System.Drawing.Color.Black;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;



        FpSpread1.Sheets[0].RowHeader.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
        FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "DEPARTMENT";
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = ddltype.SelectedItem.Text.ToString();
        string sql = "select distinct Edu_Level from  course order by Edu_Level desc";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        int col = 1;
        int stracol = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            //ds.Tables[0].Rows.Add("M.Phil");//magesh 27.8.18
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + genderselcount;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text = ds.Tables[0].Rows[i][0].ToString();
                //if(ds.Tables[0].Rows[i][0].ToString()=="M.Phil")
                //{
                //    avoidcol.Add(col);
                //}
                stracol = col;
                for (int j = 0; j < cklgender.Items.Count; j++)
                {
                    if (cklgender.Items[j].Selected == true)
                    {
                        string sex = "";
                        if (j == 0)
                        {
                            sex = "M";
                        }
                        else if (j == 1)
                        {
                            sex = "W";
                        }
                        else
                        {
                            sex = "TR";
                        }
                        if (ds.Tables[0].Rows[i][0].ToString() == "M.Phil")
                        {
                            avoidcol.Add(col);
                        }
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text = sex;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = ds.Tables[0].Rows[i][0].ToString();
                        if (ds.Tables[0].Rows[i][0].ToString() == "M.Phil")
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = "PG";
                        }
                        dtshow.Rows.Add(ds.Tables[0].Rows[i][0].ToString(), sex, col);
                        col++;
                    }
                }
                FpSpread1.Sheets[0].Columns.Count++;
                if (ds.Tables[0].Rows[i][0].ToString() == "M.Phil")
                {
                    avoidcol.Add(FpSpread1.Sheets[0].Columns.Count - 1);
                }
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Text = "Total";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Tag = ds.Tables[0].Rows[i][0].ToString();
                dtshow.Rows.Add("Total", "", FpSpread1.Sheets[0].Columns.Count - 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, stracol, 1, genderselcount + 1);

                col++;
            }
        }

        if (FpSpread1.Sheets[0].Columns.Count > 1)
        {
            FpSpread1.Sheets[0].Columns.Count++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].Columns.Count - 1].Text = "G.T";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].Columns.Count - 1, 2, 1);
        }
        FpSpread1.Visible = true;

    }

    public void bindheaderyearwise()
    {
        string batch = "";
        for (int i = 0; i < chklsbatch.Items.Count; i++)
        {
            if (chklsbatch.Items[i].Selected == true)
            {
                if (batch == "")
                {
                    batch = chklsbatch.Items[i].Text.ToString();
                }
                else
                {
                    batch = batch + "," + chklsbatch.Items[i].Text.ToString();
                }

            }
        }
        string year = "";
        dtshow.Columns.Clear();
        dtshow.Rows.Clear();
        dtshow.Columns.Add("startcol");
        dtshow.Columns.Add("Name");
        dtshow.Columns.Add("count");
        int genderselcount = 0;
        for (int j = 0; j < cklgender.Items.Count; j++)
        {
            if (cklgender.Items[j].Selected == true)
            {
                spancount++;
                genderselcount++;

            }
        }
        spancount++;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = System.Drawing.Color.Black;
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Border.BorderSize = 1;
        darkstyle.Border.BorderColor = System.Drawing.Color.Black;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        hatsrow.Clear();

        FpSpread1.Sheets[0].RowHeader.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
        FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "DEPARTMENT";
        string sqls = "select distinct Edu_Level from  course where college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' order by Edu_Level desc";
        ds.Clear();
        int coun = 0;
        ds = da.select_method_wo_parameter(sqls, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                coun++;
                // Convert.ToString(ds.Tables[0].Rows[i]["Course_Id"])
                hatsrow.Add(coun, Convert.ToString(ds.Tables[0].Rows[i]["Edu_Level"]));
            }
        }
        if (hatsrow.Count > 0)
        {
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = ddltype.SelectedItem.Text.ToString() + " /" + hatsrow[1];
            rowadd = 1;
        }
        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = ddltype.SelectedItem.Text.ToString() + " / UG";
        string sql = "select distinct r.Current_Semester from Registration r,applyn a where r.app_no=a.app_no and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'  and r.batch_year in (" + batch + ") and r.Current_Semester<=8 order by r.Current_Semester ";
        //string sql = "select distinct r.Current_Semester,c.Edu_Level from Registration r,applyn a, course c where r.app_no=a.app_no and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'  and r.batch_year in (2018,2017,2016) and r.Current_Semester<=8 order by r.Current_Semester, c.Edu_Level asc";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = ddltype.SelectedItem.Text.ToString() + " / " + ds.Tables[0].Rows[0][1].ToString();
        int col = 1;
        int stracol = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                // FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + genderselcount;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text = ds.Tables[0].Rows[i][0].ToString();

                if (ds.Tables[0].Rows[i][0].ToString() == "1" || ds.Tables[0].Rows[i][0].ToString() == "2")
                {
                    year = "I";
                }
                if (ds.Tables[0].Rows[i][0].ToString() == "3" || ds.Tables[0].Rows[i][0].ToString() == "4")
                {
                    year = "II";
                }
                if (ds.Tables[0].Rows[i][0].ToString() == "5" || ds.Tables[0].Rows[i][0].ToString() == "6")
                {
                    year = "III";
                }
                if (ds.Tables[0].Rows[i][0].ToString() == "7" || ds.Tables[0].Rows[i][0].ToString() == "8")
                {
                    year = "IV";
                }
                if (!hasyears.ContainsValue(year))
                {
                    hasyears.Add(i, year);
                    FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + genderselcount;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text = year + " Year ";
                    //  if (col == 1)
                    // {
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text = year + " Year ";
                    dtshow.Rows.Add(col, year + " Year ", col - stracol);
                    stracol = col;
                    for (int j = 0; j < cklgender.Items.Count; j++)
                    {
                        if (cklgender.Items[j].Selected == true)
                        {
                            string sex = "";
                            if (j == 0)
                            {
                                sex = "M";
                            }
                            else if (j == 1)
                            {
                                sex = "W";
                            }
                            else
                            {
                                sex = "TR";
                            }
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text = sex;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = year;
                            // dtshow.Rows.Add(ds.Tables[0].Rows[i][0].ToString(), sex, col);
                            col++;

                        }
                    }
                    FpSpread1.Sheets[0].Columns.Count++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text = "Total";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = ds.Tables[0].Rows[i][0].ToString();
                    //FpSpread1.Sheets[0].Columns.Count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Text = "Total";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Tag = ds.Tables[0].Rows[i][0].ToString();
                    ////  dtshow.Rows.Add("Total", "", FpSpread1.Sheets[0].Columns.Count - 1);
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, stracol, 1, genderselcount + 1);

                    col++;
                }
                // }
                //// else if (FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text != FpSpread1.Sheets[0].ColumnHeader.Cells[0, col -4].Text)
                //// {

                //     dtshow.Rows.Add(col, year + " Year ", col - stracol);
                //     stracol = col;
                //     for (int j = 0; j < cklgender.Items.Count; j++)
                //     {
                //         if (cklgender.Items[j].Selected == true)
                //         {
                //             string sex = "";
                //             if (j == 0)
                //             {
                //                 sex = "M";
                //             }
                //             else if (j == 1)
                //             {
                //                 sex = "W";
                //             }
                //             else
                //             {
                //                 sex = "TR";
                //             }
                //             FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text = sex;
                //             FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = year;
                //             // dtshow.Rows.Add(ds.Tables[0].Rows[i][0].ToString(), sex, col);
                //             col++;

                //         }
                //     }
                //     FpSpread1.Sheets[0].Columns.Count++;
                //     FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Text = "Total";
                //     FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Tag = ds.Tables[0].Rows[i][0].ToString();
                //     //  dtshow.Rows.Add("Total", "", FpSpread1.Sheets[0].Columns.Count - 1);
                //     FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, stracol, 1, genderselcount + 1);

                //     col++;
                // //}

            }
        }

        if (FpSpread1.Sheets[0].Columns.Count > 1)
        {
            FpSpread1.Sheets[0].Columns.Count++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].Columns.Count - 1].Text = "G.T";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].Columns.Count - 1, 2, 1);
        }
        FpSpread1.Visible = true;

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = txtexcelname.Text;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;
                    //FpEntry.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
                    //Aruna on 26feb2013============================
                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                    FpSpread1.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
                    //=============================================
                }
                else
                {
                    lblnorec.Text = "Please Enter Your Report Name";
                    lblnorec.Visible = true;
                    txtexcelname.Focus();
                }


            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = true;
            lblnorec.Text = "";

            string degreedetails = string.Empty;

            degreedetails = "STUDENTS STRENGTH " + ddlrpttype.SelectedItem.Text.ToString().ToUpper() + "";
            string pagename = "Yearwise_deptwise_strngth_rpt.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }

    }

    public void loadtype()
    {
        try
        {
            collegecode = Session["collegecode"].ToString();
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {


                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();

                //ddltype.Items.Insert(0, "Select");
                ddltype.Enabled = true;


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

    public void Bindcollege()
    {
        try
        {
            string columnfield = "";
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {

                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                loadtype();
            }
            else
            {
                lblerrormsg.Text = "Set college rights to the staff";
                lblerrormsg.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            //ds2 = BindBatch11();
            string strsql = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''  and delflag=0 and exam_flag<>'debar'  order by batch_year desc";
            ds2 = da.select_method_wo_parameter(strsql, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                chklsbatch.SelectedIndex = chklsbatch.Items.Count - 1;
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklsbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                    }
                }
                if (chkbatch.Checked == true)
                {
                    for (int i = 0; i < chklsbatch.Items.Count; i++)
                    {
                        chklsbatch.Items[i].Selected = true;
                        txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklsbatch.Items.Count; i++)
                    {
                        chklsbatch.Items[i].Selected = false;
                        txtbatch.Text = "---Select---";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    //public void BindBatch()
    //{
    //    try
    //    {
    //        ds2.Dispose();
    //        ds2.Reset();
    //        ds2 = d2.BindBatch();
    //        if (ds2.Tables[0].Rows.Count > 0)
    //        {
    //            chklsbatch.DataSource = ds2;
    //            chklsbatch.DataTextField = "Batch_year";
    //            chklsbatch.DataValueField = "Batch_year";
    //            chklsbatch.DataBind();
    //            chklsbatch.SelectedIndex = chklsbatch.Items.Count - 1;
    //            for (int i = 0; i < chklsbatch.Items.Count; i++)
    //            {
    //                chklsbatch.Items[i].Selected = true;
    //                if (chklsbatch.Items[i].Selected == true)
    //                {
    //                    count += 1;
    //                }
    //                if (chklsbatch.Items.Count == count)
    //                {
    //                    chkbatch.Checked = true;
    //                }
    //            }
    //            if (chkbatch.Checked == true)
    //            {
    //                for (int i = 0; i < chklsbatch.Items.Count; i++)
    //                {
    //                    chklsbatch.Items[i].Selected = true;
    //                    txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
    //                }
    //            }
    //            else
    //            {
    //                for (int i = 0; i < chklsbatch.Items.Count; i++)
    //                {
    //                    chklsbatch.Items[i].Selected = false;
    //                    txtbatch.Text = "---Select---";
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerrormsg.Text = ex.ToString();
    //        lblerrormsg.Visible = true;
    //    }
    //}

    //public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    //{
    //    try
    //    {
    //        ArrayList arrnew = new ArrayList();
    //        collegecode = Session["collegecode"].ToString();
    //        //ddltype.Items.Clear();
    //        string strquery = "     select distinct course_id from course where  college_code='" + collegecode + "' and type='" + ddltype.SelectedItem.Text.ToString() + "' and  type is not null and type<>''";
    //        ds = da.select_method_wo_parameter(strquery, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {


    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                arrnew.Add(ds.Tables[0].Rows[i][0].ToString());

    //            }

    //            lblerrormsg.Visible = false;
    //            count = 0;
    //            chklstdegree.Items.Clear();
    //            if (group_user.Contains(';'))
    //            {
    //                string[] group_semi = group_user.Split(';');
    //                group_user = group_semi[0].ToString();
    //            }
    //            ds2.Dispose();
    //            ds2.Reset();
    //            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
    //            if (ds2.Tables[0].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
    //                {
    //                    if (arrnew.Contains(ds2.Tables[0].Rows[i]["course_id"].ToString()))
    //                    {


    //                        ListItem li1 = new ListItem(ds2.Tables[0].Rows[i]["course_name"].ToString(), ds2.Tables[0].Rows[i]["course_id"].ToString());
    //                        chklstdegree.Items.Add(li1);


    //                    }
    //                }

    //                for (int i = 0; i < chklstdegree.Items.Count; i++)
    //                {
    //                    chklstdegree.Items[i].Selected = true;
    //                    if (chklstdegree.Items[i].Selected == true)
    //                    {
    //                        count += 1;
    //                    }
    //                    if (chklstdegree.Items.Count == count)
    //                    {
    //                        chkdegree.Checked = true;
    //                    }
    //                }
    //                if (chkdegree.Checked == true)
    //                {
    //                    for (int i = 0; i < chklstdegree.Items.Count; i++)
    //                    {
    //                        chklstdegree.Items[i].Selected = true;
    //                        txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
    //                    }
    //                }
    //                else
    //                {
    //                    for (int i = 0; i < chklstdegree.Items.Count; i++)
    //                    {
    //                        chklstdegree.Items[i].Selected = false;
    //                        txtdegree.Text = "---Select---";
    //                    }
    //                }
    //                txtdegree.Enabled = true;
    //            }
    //            else
    //            {
    //                txtdegree.Enabled = false;
    //            }

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerrormsg.Text = ex.ToString();
    //        lblerrormsg.Visible = true;
    //    }

    //}
    public void bindgroup()
    {
        if (ddltype.Items.Count > 0)
        {
            string sql = "select distinct (select  textval from textvaltable t where t.TextCode=tb.Groupcode ) as groupn,Groupcode from tbl_DeptGrouping tb where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(sql, "Text");
            chklstdegree.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {



                    ListItem li1 = new ListItem(ds.Tables[0].Rows[i]["groupn"].ToString(), ds.Tables[0].Rows[i]["Groupcode"].ToString());
                    chklstdegree.Items.Add(li1);



                }
                //chklstdegree.DataSource = ds;
                //chklstdegree.DataBind();

                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                if (chkdegree.Checked == true)
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = true;
                        txtdegree.Text = "Group(" + (chklstdegree.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = false;
                        txtdegree.Text = "---Select---";
                    }
                }
                txtdegree.Enabled = true;
            }
        }
        else
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = "No type were found";
        }

    }
    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;
            Hashtable avoiddegree = new Hashtable();
            collegecode = ddlcollege.SelectedValue.ToString();
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "','" + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            if (course_id.Trim() != "")
            {
                if (course_id.ToString().Trim() != "")
                {
                    //if (singleuser == "True")
                    //{
                    //    ds2.Dispose();
                    //    ds2.Reset();
                    //    string strquery = "select distinct department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " ";
                    //    ds2 = da.select_method_wo_parameter(strquery, "Text");
                    //}
                    //else
                    //{
                    //    ds2.Dispose();
                    //    ds2.Reset();
                    //    string strquery1 = "select distinct department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + "";
                    //    ds2 = da.select_method_wo_parameter(strquery1, "Text");
                    //}
                    ds2.Dispose();
                    ds2.Reset();
                    //string strquery = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept from tbl_DeptGrouping tb where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + course_id + "')";
                    string strquery = "select distinct c.Course_Name+'-'+ dt.Dept_Name as dept,CONVERT(varchar(10), dt.dept_code)+'-'+CONVERT(varchar(10), c.Course_Id)as dept_code from degree d,department dt,course c,tbl_DeptGrouping tb  where c.course_id=d.course_id and dt.dept_code=d.dept_code and c.college_code = d.college_code and dt.college_code = d.college_code and d.Dept_Code=tb.Deptcode and tb.type='" + ddltype.SelectedItem.Text.ToString() + "' and d.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and tb.Groupcode in ('" + course_id + "') and c.type='" + ddltype.SelectedItem.Text.ToString() + "'";
                    ds2 = da.select_method_wo_parameter(strquery, "Text");
                }
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds2;
                    chklstbranch.DataTextField = "dept";
                    chklstbranch.DataValueField = "dept_code";


                    chklstbranch.DataBind();
                    chklstbranch.Items[0].Selected = true;
                    for (int i = 0; i < chklstbranch.Items.Count; i++)
                    {
                        chklstbranch.Items[i].Selected = true;
                        if (chklstbranch.Items[i].Selected == true)
                        {
                            count += 1;
                        }
                        if (chklstbranch.Items.Count == count)
                        {
                            chkbranch.Checked = true;
                        }
                    }
                    if (chkbranch.Checked == true)
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chklstbranch.Items[i].Selected = true;
                            if (checkSchoolSetting() == 0)//abarna
                            {
                                txtbranch.Text = "Standard(" + (chklstbranch.Items.Count) + ")";
                            }
                            else
                            {
                                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chkbranch.Checked = false;
                            chklstbranch.Items[i].Selected = false;
                            txtbranch.Text = "---Select---";
                        }
                    }
                }
            }
            else
            {
                txtbranch.Text = "---Select---";
                chklstbranch.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {

            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                }
                txtbatch.Text = "---Select---";
            }
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {

        }
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txtbatch.Text = "--Select--";
            chkbatch.Checked = false;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == chklsbatch.Items.Count)
                {
                    chkbatch.Checked = true;
                }
            }
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {

        }
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            collegecode = ddlcollege.SelectedValue.ToString();
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "---Select---";
                txtbranch.Text = "---Select---";
            }
            collegecode = ddlcollege.SelectedValue.ToString();
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {

        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            collegecode = ddlcollege.SelectedValue.ToString();
            int commcount = 0;
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                if (commcount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }

            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                if (checkSchoolSetting() == 0)
                {
                    txtbranch.Text = "Standard(" + (chklstbranch.Items.Count) + ")";
                }
                else
                {
                    txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                chkbranch.Checked = false;
                txtbranch.Text = "---Select---";
            }
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            lblerrormsg.Visible = false;
            string clg = "";
            int commcount = 0;
            txtbranch.Text = "--Select--";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (checkSchoolSetting() == 0)
                {

                    txtbranch.Text = "Standard(" + commcount.ToString() + ")";
                }
                else
                {
                    txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                }
                if (commcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }

            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            lblerrormsg.Visible = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            // BindDegree(singleuser, group_user, collegecode, usercode);
            bindgroup();
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlrpttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindgroup();
            collegecode = ddlcollege.SelectedValue.ToString();
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    protected void ckgender_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            txtgender.Text = "---Select---";
            if (ckgender.Checked == true)
            {
                for (int i = 0; i < cklgender.Items.Count; i++)
                {
                    cklgender.Items[i].Selected = true;
                }
                txtgender.Text = "Gender(" + (cklgender.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cklgender.Items.Count; i++)
                {
                    cklgender.Items[i].Selected = false;
                }
                txtgender.Text = "---Select---";

            }

            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {

        }
    }

    protected void cklgender_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            int commcount = 0;
            ckgender.Checked = false;
            txtgender.Text = "---Select---";
            for (int i = 0; i < cklgender.Items.Count; i++)
            {
                if (cklgender.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtgender.Text = "Gender(" + commcount.ToString() + ")";
                if (commcount == cklgender.Items.Count)
                {
                    ckgender.Checked = true;
                }
            }


            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    public void hide()
    {
        Printcontrol.Visible = false;
        FpSpread1.Visible = false;
        final.Visible = false;

    }

    public void bindvalueyearwise()
    {
        try
        {
            DataView dv = new DataView();
            string type = "";
            if (ddltype.Items.Count > 0)
            {
                type = ddltype.SelectedItem.Text.ToString();
            }
            string sex = "";

            ArrayList avoirows = new ArrayList();
            //Hashtable avoirows = new Hashtable();
            string edulevelid = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (edulevelid == "")
                    {

                        edulevelid = ds.Tables[0].Rows[i][0].ToString();
                    }
                    else
                    {

                        edulevelid = edulevelid + "','" + ds.Tables[0].Rows[i][0].ToString();
                    }

                }
            }

            string courseid = "";
            string courid = "";
            string deptid = "";
            int cont = 0;
            int cont1 = 0;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    string spl = chklstbranch.Items[i].Value.ToString();
                    string[] cours = spl.Split('-');
                    if (deptid == "")
                    {
                        //deptid = chklstbranch.Items[i].Value.ToString();
                        deptid = cours[0];
                        courid = cours[1];
                    }
                    else
                    {
                        //deptid = deptid + "','" + chklstbranch.Items[i].Value.ToString();
                        deptid = deptid + "','" + cours[0];
                        courid = courid + "','" + cours[1];
                    }

                }
            }
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    courseid = chklstdegree.Items[i].Value.ToString();

                    //string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.Edu_Level='UG' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_name in ('" + deptid + "'))";
                    string sql = " select distinct  c.course_name+'-'+ dt.dept_name as dept_name,dt.dept_name as dept_name1,dt.dept_acronym,d.Dept_Code[Deptcode],c.Course_Id from tbl_DeptGrouping tb,course c,Degree d,Department dt where d.Dept_Code=dt.Dept_Code  and  d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'  and Groupcode in ('" + courseid + "') and d.Dept_Code in ('" + deptid + "') and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.Course_Id in('" + courid + "')";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sql, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                        {
                            FpSpread1.Sheets[0].Rows.Count++;
                            if (cbacr.Checked)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_acronym"].ToString();
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_name"].ToString();
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["Deptcode"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[ii]["Course_Id"].ToString();
                            cont++;
                        }
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;
                        //avoirows.Add(FpSpread1.Sheets[0].RowCount - 1, courseid);
                        avoirows.Add(FpSpread1.Sheets[0].RowCount - 1);
                        cont++;
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
                    }
                }
            }



            string batchyearselected = "";
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (batchyearselected == "")
                    {
                        batchyearselected = chklsbatch.Items[i].Value.ToString();
                    }
                    else
                    {
                        batchyearselected = batchyearselected + "','" + chklsbatch.Items[i].Value.ToString();
                    }

                }
            }


            //string edulevelid = "";
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        if (edulevelid == "")
            //        {

            //            edulevelid = ds.Tables[0].Rows[i][0].ToString();
            //        }
            //        else
            //        {

            //            edulevelid = edulevelid + "','" + ds.Tables[0].Rows[i][0].ToString();
            //        }

            //    }
            //}

            //string sql = " select distinct d.Dept_Code,dd.Dept_Name from Degree d,course c,Department dd where d.Course_Id=c.Course_Id and d.Dept_Code=dd.Dept_Code and   c.type='" + type + "' and d.Dept_Code in ('" + deptid + "')   and d.Course_Id in ('" + courseid + "') order by dd.Dept_Name,Dept_Code ";
            //ds.Clear();
            //ds = da.select_method_wo_parameter(sql, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        // string ugpg = FpSpread1.Sheets[0].ColumnHeader.Columns.Count;

            //        FpSpread1.Sheets[0].Rows.Count++;
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[i]["dept_name"].ToString();
            //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["Dept_Code"].ToString();



            //    }

            //}

            double ugpgtotal = 0;
            double endugpgtotal = 0;
            double overallugpgtotal = 0;
            string sqlnew = "SELECT r.Current_Semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total,c.Course_Id FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and c.Edu_Level='" + hatsrow[rowadd] + "' group by r.Current_Semester,d.Dept_Code,dept_name,Edu_Level,sex,C.Course_Id order by d.Dept_Code ";//magesh 30
            ds.Clear();
            ds = da.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    if (!avoirows.Contains(i))
                    {

                        for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
                        {
                            //edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                            string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                            if (chckyear.Trim() == "I")
                            {
                                chckyear = "  and Current_Semester >=1 and Current_Semester <=2";
                            }
                            if (chckyear.Trim() == "II")
                            {
                                chckyear = "  and Current_Semester >=3 and Current_Semester <=4";
                            }
                            if (chckyear.Trim() == "III")
                            {
                                chckyear = "  and Current_Semester >=5 and Current_Semester <=6";
                            }
                            if (chckyear.Trim() == "IV")
                            {
                                chckyear = "  and Current_Semester >=7 and Current_Semester <=8";
                            }
                            sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
                            string deptidnew = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                            string cor = FpSpread1.Sheets[0].Cells[i, 0].Note.ToString();
                            string filter = "Dept_Code='" + deptidnew + "' and sex='" + sex + "' and Edu_Level='" + hatsrow[rowadd] + "' " + chckyear + " and  course_id='" + cor + "'";
                            if (sex.ToUpper().Trim() != "TOTAL")
                            {

                                ds.Tables[0].DefaultView.RowFilter = filter;
                                dv = ds.Tables[0].DefaultView;
                                int coun = 0;
                                if (dv.Count > 0)
                                {
                                    for (int m = 0; m < dv.Count; m++)
                                    {
                                        coun = coun + Convert.ToInt32(dv[m]["total"]);
                                        FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(coun);
                                        ugpgtotal = ugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());
                                        // overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

                                    }


                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = "--";

                                }
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
                                ugpgtotal = 0;

                            }
                            FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                        }
                        // FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
                        // overallugpgtotal = 0;
                    }


                }

            }
            // avoirows.Clear();
            int outnum = 0;
            overallugpgtotal = 0;
            FpSpread1.Sheets[0].Rows.Count++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            cont++;
            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
            FpSpread1.SaveChanges();
            for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
                {
                    if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "TOTAL")
                    {


                        string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
                        if (Int32.TryParse(tt, out outnum))
                        {
                            overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
                            endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

                        }
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
                        //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
                        endugpgtotal = 0;
                        FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                    }

                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
                overallugpgtotal = 0;

            }

            FpSpread1.Sheets[0].Rows.Count++;
            ArrayList mphilcol = new ArrayList();
            int lastrowcount = FpSpread1.Sheets[0].Rows.Count;
            rowadd = 0;
            if (hatsrow.Count > 0)
            {
                for (int cun = 2; cun < hatsrow.Count; cun++)
                {
                    lastrowcount = FpSpread1.Sheets[0].Rows.Count;
                    //FpSpread1.Sheets[0].Cells[lastrowcount - 1, 0].Text = ddltype.SelectedItem.Text.ToString() + " / PG";
                    rowadd = cun;
                    FpSpread1.Sheets[0].Cells[lastrowcount - 1, 0].Text = ddltype.SelectedItem.Text.ToString() + " / " + hatsrow[cun];
                    FpSpread1.Sheets[0].Cells[lastrowcount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    cont++;
                    int mphilcolstart = 0;
                    if (dtshow.Rows.Count > 0)
                    {
                        for (int cc = 0; cc < dtshow.Rows.Count; cc++)
                        {
                            FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].Text = dtshow.Rows[cc][1].ToString();
                            if (cc == 2 && Convert.ToString(hatsrow[cc]) == "PG")
                            {
                                if (!mphilcol.Contains(dtshow.Rows[cc][0].ToString()))
                                {
                                    mphilcol.Add(dtshow.Rows[cc][0].ToString());
                                    mphilcolstart = Convert.ToInt32(dtshow.Rows[cc][0].ToString());
                                    for (int j = 0; j < cklgender.Items.Count; j++)
                                    {
                                        if (cklgender.Items[j].Selected == true)
                                        {

                                            mphilcol.Add(mphilcolstart);
                                            mphilcolstart++;

                                        }
                                    }
                                    mphilcol.Add(mphilcolstart);
                                }


                                FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].Text = "M.Phil";
                            }
                            FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].SpanModel.Add(lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString()), 1, spancount);
                        }
                    }

                    //FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Rows[lastrowcount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
                    FpSpread1.SaveChanges();
                }
            }
            #region
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    courseid = chklstdegree.Items[i].Value.ToString();

                    // string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Deptcode in ('" + deptid + "')";

                    //string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.Edu_Level='" + hatsrow[rowadd] + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_name in ('" + deptid + "'))";
                    string sql = "select distinct  c.course_name+'-'+ dt.dept_name as dept_name,dt.dept_name as dept_name1,dt.dept_acronym,d.Dept_Code[Deptcode],c.Course_Id from tbl_DeptGrouping tb,course c,Degree d,Department dt where d.Dept_Code=dt.Dept_Code  and  d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'  and Groupcode in ('" + courseid + "') and d.Dept_Code in ('" + deptid + "') and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.Course_Id in('" + courid + "')";
                    // string sql = " select distinct (select c.course_name+'-'+  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode,c.course_id from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_code in ('" + deptid + "'))";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sql, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                        {
                            FpSpread1.Sheets[0].Rows.Count++;
                            if (cbacr.Checked)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_acronym"].ToString();
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_name"].ToString();
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["Deptcode"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[ii]["Course_Id"].ToString();
                            cont1++;
                        }
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                        cont1++;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;
                        //avoirows.Add(FpSpread1.Sheets[0].RowCount - 1, courseid);
                        avoirows.Add(FpSpread1.Sheets[0].RowCount - 1);
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
                    }
                }
            }
            ugpgtotal = 0;
            endugpgtotal = 0;
            overallugpgtotal = 0;
            sqlnew = "SELECT G.degree_code,r.Current_Semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total,c.Course_Id FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and c.Edu_Level='" + hatsrow[rowadd] + "' and  c.Course_Name not like '%M.Phil%' group by r.Current_Semester,d.Dept_Code,dept_name,Edu_Level,sex,G.degree_code,c.Course_Id order by d.Dept_Code ";
            ds.Clear();
            //   lastrowcount = FpSpread1.Sheets[0].RowCount;//am
            ds = da.select_method_wo_parameter(sqlnew, "Text");
            //cont++;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = cont; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    if (!avoirows.Contains(i))
                    {

                        for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
                        {
                            if (!mphilcol.Contains(j))
                            {

                                //edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                                string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                                if (chckyear.Trim() == "I")
                                {
                                    chckyear = "  and Current_Semester >=1 and Current_Semester <=2";
                                }
                                if (chckyear.Trim() == "II")
                                {
                                    chckyear = "  and Current_Semester >=3 and Current_Semester <=4";
                                }
                                if (chckyear.Trim() == "III")
                                {
                                    chckyear = "  and Current_Semester >=5 and Current_Semester <=6";
                                }
                                if (chckyear.Trim() == "IV")
                                {
                                    chckyear = "  and Current_Semester >=7 and Current_Semester <=8";
                                }
                                sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
                                deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                                string cor = FpSpread1.Sheets[0].Cells[i, 0].Note.ToString();
                                string filter = "Dept_Code='" + deptid + "' and sex='" + sex + "' and Edu_Level='" + hatsrow[rowadd] + "' " + chckyear + " and  course_id='" + cor + "'";
                                if (sex.ToUpper().Trim() != "TOTAL")
                                {

                                    ds.Tables[0].DefaultView.RowFilter = filter;
                                    dv = ds.Tables[0].DefaultView;
                                    int coun = 0;
                                    if (dv.Count > 0)
                                    {
                                        for (int m = 0; m < dv.Count; m++)
                                        {
                                            coun = coun + Convert.ToInt32(dv[m]["total"]);
                                            FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(coun);
                                            ugpgtotal = ugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());
                                            // overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

                                        }


                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[i, j].Text = "--";

                                    }
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
                                    ugpgtotal = 0;

                                }
                                FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                            }
                        }
                        //FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
                        overallugpgtotal = 0;
                    }


                }

            }
            ugpgtotal = 0;
            endugpgtotal = 0;
            overallugpgtotal = 0;
            sqlnew = "SELECT G.degree_code,r.Current_Semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total,c.Course_Id FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and c.Edu_Level='" + hatsrow[rowadd] + "'  and  c.Course_Name  like '%M.Phil%' group by r.Current_Semester,d.Dept_Code,dept_name,Edu_Level,sex,G.degree_code,c.Course_Id order by d.Dept_Code ";
            ds.Clear();
            ds = da.select_method_wo_parameter(sqlnew, "Text");
            int cuoun = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = lastrowcount; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    if (!avoirows.Contains(i))
                    {

                        for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
                        {
                            if (mphilcol.Contains(j))
                            {

                                //edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                                string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                                chckyear = "  and Current_Semester >=1 and Current_Semester <=2";
                                sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
                                deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                                string filter = "dept_code='" + deptid + "' and sex='" + sex + "' and Edu_Level='" + hatsrow[rowadd] + "' " + chckyear + "";
                                if (sex.ToUpper().Trim() != "TOTAL")
                                {

                                    ds.Tables[0].DefaultView.RowFilter = filter;
                                    dv = ds.Tables[0].DefaultView;
                                    int coun = 0;
                                    if (dv.Count > 0)
                                    {
                                        for (int m = 0; m < dv.Count; m++)
                                        {
                                            coun = coun + Convert.ToInt32(dv[m]["total"]);
                                            FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(coun);
                                            ugpgtotal = ugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());
                                            // overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

                                        }


                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[i, j].Text = "--";

                                    }
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
                                    ugpgtotal = 0;

                                }
                                FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                            }
                        }
                        //FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
                        overallugpgtotal = 0;
                    }


                }

            }

            outnum = 0;
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
                {
                    sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();

                    if (sex.ToUpper().Trim() == "TOTAL" && (lastrowcount - 1) != i)
                    {
                        string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
                        if (Int32.TryParse(tt, out outnum))
                        {

                            endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

                        }
                    }

                }
                if (lastrowcount - 1 != i)
                {
                    FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(endugpgtotal);
                }
                //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
                endugpgtotal = 0;
                FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            }
            outnum = 0;
            overallugpgtotal = 0;
            endugpgtotal = 0;
            FpSpread1.Sheets[0].Rows.Count++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            cont1++;
            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
            FpSpread1.SaveChanges();
            for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
            {
                for (int i = lastrowcount; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
                {
                    if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "TOTAL")
                    {


                        string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
                        if (Int32.TryParse(tt, out outnum))
                        {
                            overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
                            endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

                        }
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
                        //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
                        endugpgtotal = 0;
                        FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                    }

                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
                overallugpgtotal = 0;

            }


            #endregion
            //        #region hos
            //        for (int i = 0; i < chklstdegree.Items.Count; i++)
            //        {
            //            if (chklstdegree.Items[i].Selected == true)
            //            {
            //                courseid = chklstdegree.Items[i].Value.ToString();

            //                // string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Deptcode in ('" + deptid + "')";

            //                //string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.Edu_Level='" + hatsrow[rowadd] + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_name in ('" + deptid + "'))";
            //                string sql = "select distinct  c.course_name+'-'+ dt.dept_name as dept_name,dt.dept_name as dept_name1,d.Dept_Code[Deptcode],c.Course_Id from tbl_DeptGrouping tb,course c,Degree d,Department dt where d.Dept_Code=dt.Dept_Code  and  d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'  and Groupcode in ('" + courseid + "') and d.Dept_Code in ('" + deptid + "') and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.Course_Id in('" + courid + "')";
            //                // string sql = " select distinct (select c.course_name+'-'+  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode,c.course_id from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_code in ('" + deptid + "'))";
            //                ds.Clear();
            //                ds = da.select_method_wo_parameter(sql, "Text");
            //                if (ds.Tables[0].Rows.Count > 0)
            //                {
            //                    for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
            //                    {
            //                        FpSpread1.Sheets[0].Rows.Count++;
            //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_name"].ToString();
            //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["Deptcode"].ToString();
            //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[ii]["Course_Id"].ToString();
            //                        cont1++;
            //                    }
            //                    FpSpread1.Sheets[0].Rows.Count++;
            //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
            //                    cont1++;
            //                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;
            //                    //avoirows.Add(FpSpread1.Sheets[0].RowCount - 1, courseid);
            //                    avoirows.Add(FpSpread1.Sheets[0].RowCount - 1);
            //                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
            //                }
            //            }
            //        }
            //        ugpgtotal = 0;
            //        endugpgtotal = 0;
            //        double endugpgtotal1 = 0;
            //        overallugpgtotal = 0;
            //        FpSpread1.Sheets[0].Rows.Count++;
            //        lastrowcount = FpSpread1.Sheets[0].Rows.Count;

            //        if (hatsrow.Count > 0)
            //        {
            //            for (int cun = rowadd + 1; cun <= hatsrow.Count + (rowadd); cun++)
            //            {
            //                if (hatsrow.Count >= cun)
            //                {
            //                    FpSpread1.Sheets[0].Cells[lastrowcount - 1, 0].Text = ddltype.SelectedItem.Text.ToString() + " / " + hatsrow[cun];
            //                  //  FpSpread1.Sheets[0].Cells[lastrowcount - 1, 0].Tag = "45";
            //                     lastrowcount = FpSpread1.Sheets[0].Rows.Count;
            //#region my
            //                    int mphilcolstart = 0;
            //                    if (dtshow.Rows.Count > 0)
            //                    {
            //                        for (int cc = 0; cc < dtshow.Rows.Count; cc++)
            //                        {
            //                            FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].Text = dtshow.Rows[cc][1].ToString();
            //                            if (cc == 2 && Convert.ToString(hatsrow[cc]) != "PG")
            //                            {
            //                                if (!mphilcol.Contains(dtshow.Rows[cc][0].ToString()))
            //                                {
            //                                    mphilcol.Add(dtshow.Rows[cc][0].ToString());
            //                                    mphilcolstart = Convert.ToInt32(dtshow.Rows[cc][0].ToString());
            //                                    for (int j = 0; j < cklgender.Items.Count; j++)
            //                                    {
            //                                        if (cklgender.Items[j].Selected == true)
            //                                        {

            //                                            mphilcol.Add(mphilcolstart);
            //                                            mphilcolstart++;

            //                                        }
            //                                    }
            //                                    mphilcol.Add(mphilcolstart);
            //                                }


            //                                FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].Text = "M.Phil";
            //                            }
            //                            FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].HorizontalAlign = HorizontalAlign.Center;
            //                            FpSpread1.Sheets[0].SpanModel.Add(lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString()), 1, spancount);
            //                        }
            //                    }
            //                    #endregion
            //                    FpSpread1.Sheets[0].RowCount++;
            //                    sqlnew = "SELECT r.Current_Semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and c.Edu_Level='" + hatsrow[cun] + "' group by r.Current_Semester,d.Dept_Code,dept_name,Edu_Level,sex order by d.Dept_Code ";
            //                    ds.Clear();
            //                    ds = da.select_method_wo_parameter(sqlnew, "Text");
            //                    if (ds.Tables[0].Rows.Count > 0)
            //                    {
            //                        for (int i = cont + cont1; i < FpSpread1.Sheets[0].RowCount; i++)
            //                        {
            //                            if (!avoirows.Contains(i))
            //                            {

            //                                for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
            //                                {
            //                                    //edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
            //                                    string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
            //                                    if (chckyear.Trim() == "I")
            //                                    {
            //                                        chckyear = "  and Current_Semester >=1 and Current_Semester <=2";
            //                                    }
            //                                    if (chckyear.Trim() == "II")
            //                                    {
            //                                        chckyear = "  and Current_Semester >=3 and Current_Semester <=4";
            //                                    }
            //                                    if (chckyear.Trim() == "III")
            //                                    {
            //                                        chckyear = "  and Current_Semester >=5 and Current_Semester <=6";
            //                                    }
            //                                    if (chckyear.Trim() == "IV")
            //                                    {
            //                                        chckyear = "  and Current_Semester >=7 and Current_Semester <=8";
            //                                    }
            //                                    sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
            //                                    string deptidnew = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
            //                                    string filter = "Dept_Code='" + deptidnew + "' and sex='" + sex + "' and Edu_Level='" + hatsrow[cun] + "' " + chckyear + "";
            //                                    if (sex.ToUpper().Trim() != "TOTAL")
            //                                    {

            //                                        ds.Tables[0].DefaultView.RowFilter = filter;
            //                                        dv = ds.Tables[0].DefaultView;
            //                                        if (dv.Count > 0)
            //                                        {
            //                                            for (int m = 0; m < dv.Count; m++)
            //                                            {
            //                                                FpSpread1.Sheets[0].Cells[i, j].Text = dv[0]["total"].ToString();
            //                                                ugpgtotal = ugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());
            //                                                // overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

            //                                            }


            //                                        }
            //                                        else
            //                                        {
            //                                            FpSpread1.Sheets[0].Cells[i, j].Text = "--";

            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
            //                                        ugpgtotal = 0;

            //                                    }
            //                                    FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
            //                                    FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
            //                                }
            //                                // FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
            //                                // overallugpgtotal = 0;
            //                            }


            //                        }

            //                    }
            //                    // avoirows.Clear();


            //#region
            //for (int i = 0; i < chklstdegree.Items.Count; i++)
            //{
            //    if (chklstdegree.Items[i].Selected == true)
            //    {
            //        courseid = chklstdegree.Items[i].Value.ToString();

            //        string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Deptcode in ('" + deptid + "')";

            //        string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.Edu_Level='" + hatsrow[rowadd] + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_name in ('" + deptid + "'))";
            //        string sql = "select distinct  c.course_name+'-'+ dt.dept_name as dept_name,dt.dept_name as dept_name1,dt.dept_acronym,d.Dept_Code[Deptcode],c.Course_Id from tbl_DeptGrouping tb,course c,Degree d,Department dt where d.Dept_Code=dt.Dept_Code  and  d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'  and Groupcode in ('" + courseid + "') and d.Dept_Code in ('" + deptid + "') and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.Course_Id in('" + courid + "')";
            //        string sql = " select distinct (select c.course_name+'-'+  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode,c.course_id from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_code in ('" + deptid + "'))";
            //        ds.Clear();
            //        ds = da.select_method_wo_parameter(sql, "Text");
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
            //            {
            //                FpSpread1.Sheets[0].Rows.Count++;
            //                if (cbacr.Checked)
            //                {
            //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_acronym"].ToString();
            //                }
            //                else
            //                {
            //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_name"].ToString();
            //                }
            //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["Deptcode"].ToString();
            //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[ii]["Course_Id"].ToString();
            //                cont1++;
            //            }
            //            FpSpread1.Sheets[0].Rows.Count++;
            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
            //            cont1++;
            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;
            //            avoirows.Add(FpSpread1.Sheets[0].RowCount - 1, courseid);
            //            avoirows.Add(FpSpread1.Sheets[0].RowCount - 1);
            //            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
            //        }
            //    }
            //}
            //ugpgtotal = 0;
            //endugpgtotal = 0;
            //overallugpgtotal = 0;
            //sqlnew = "SELECT G.degree_code,r.Current_Semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total,c.Course_Id FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and c.Edu_Level='" + hatsrow[rowadd] + "' and  c.Course_Name not like '%M.Phil%' group by r.Current_Semester,d.Dept_Code,dept_name,Edu_Level,sex,G.degree_code,c.Course_Id order by d.Dept_Code ";
            //ds.Clear();
            //lastrowcount = FpSpread1.Sheets[0].RowCount;
            //ds = da.select_method_wo_parameter(sqlnew, "Text");
            //cont++;
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = cont; i < FpSpread1.Sheets[0].RowCount; i++)
            //    {
            //        if (!avoirows.Contains(i))
            //        {

            //            for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
            //            {
            //                if (!mphilcol.Contains(j))
            //                {

            //                    edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
            //                    string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
            //                    if (chckyear.Trim() == "I")
            //                    {
            //                        chckyear = "  and Current_Semester >=1 and Current_Semester <=2";
            //                    }
            //                    if (chckyear.Trim() == "II")
            //                    {
            //                        chckyear = "  and Current_Semester >=3 and Current_Semester <=4";
            //                    }
            //                    if (chckyear.Trim() == "III")
            //                    {
            //                        chckyear = "  and Current_Semester >=5 and Current_Semester <=6";
            //                    }
            //                    if (chckyear.Trim() == "IV")
            //                    {
            //                        chckyear = "  and Current_Semester >=7 and Current_Semester <=8";
            //                    }
            //                    sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
            //                    deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
            //                    string cor = FpSpread1.Sheets[0].Cells[i, 0].Note.ToString();
            //                    string filter = "Dept_Code='" + deptid + "' and sex='" + sex + "' and Edu_Level='" + hatsrow[rowadd] + "' " + chckyear + " and  course_id='" + cor + "'";
            //                    if (sex.ToUpper().Trim() != "TOTAL")
            //                    {

            //                        ds.Tables[0].DefaultView.RowFilter = filter;
            //                        dv = ds.Tables[0].DefaultView;
            //                        int coun = 0;
            //                        if (dv.Count > 0)
            //                        {
            //                            for (int m = 0; m < dv.Count; m++)
            //                            {
            //                                coun = coun + Convert.ToInt32(dv[m]["total"]);
            //                                FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(coun);
            //                                ugpgtotal = ugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());
            //                                overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

            //                            }


            //                        }
            //                        else
            //                        {
            //                            FpSpread1.Sheets[0].Cells[i, j].Text = "--";

            //                        }
            //                    }
            //                    else
            //                    {
            //                        FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
            //                        ugpgtotal = 0;

            //                    }
            //                    FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
            //                    FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
            //                }
            //            }
            //            FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
            //            overallugpgtotal = 0;
            //        }


            //    }

            //}
            //ugpgtotal = 0;
            //endugpgtotal = 0;
            //overallugpgtotal = 0;
            //sqlnew = "SELECT G.degree_code,r.Current_Semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total,c.Course_Id FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and c.Edu_Level='" + hatsrow[rowadd] + "'  and  c.Course_Name  like '%M.Phil%' group by r.Current_Semester,d.Dept_Code,dept_name,Edu_Level,sex,G.degree_code,c.Course_Id order by d.Dept_Code ";
            //ds.Clear();
            //ds = da.select_method_wo_parameter(sqlnew, "Text");
            //cuoun = 0;
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = lastrowcount; i < FpSpread1.Sheets[0].RowCount; i++)
            //    {
            //        if (!avoirows.Contains(i))
            //        {

            //            for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
            //            {
            //                if (mphilcol.Contains(j))
            //                {

            //                    edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
            //                    string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
            //                    chckyear = "  and Current_Semester >=1 and Current_Semester <=2";
            //                    sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
            //                    deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
            //                    string filter = "dept_code='" + deptid + "' and sex='" + sex + "' and Edu_Level='" + hatsrow[rowadd] + "' " + chckyear + "";
            //                    if (sex.ToUpper().Trim() != "TOTAL")
            //                    {

            //                        ds.Tables[0].DefaultView.RowFilter = filter;
            //                        dv = ds.Tables[0].DefaultView;
            //                        int coun = 0;
            //                        if (dv.Count > 0)
            //                        {
            //                            for (int m = 0; m < dv.Count; m++)
            //                            {
            //                                coun = coun + Convert.ToInt32(dv[m]["total"]);
            //                                FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(coun);
            //                                ugpgtotal = ugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());
            //                                overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

            //                            }


            //                        }
            //                        else
            //                        {
            //                            FpSpread1.Sheets[0].Cells[i, j].Text = "--";

            //                        }
            //                    }
            //                    else
            //                    {
            //                        FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
            //                        ugpgtotal = 0;

            //                    }
            //                    FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
            //                    FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
            //                }
            //            }
            //            FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
            //            overallugpgtotal = 0;
            //        }


            //    }

            //}

            //outnum = 0;
            //for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            //{
            //    for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
            //    {
            //        sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();

            //        if (sex.ToUpper().Trim() == "TOTAL" && (lastrowcount - 1) != i)
            //        {
            //            string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
            //            if (Int32.TryParse(tt, out outnum))
            //            {

            //                endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

            //            }
            //        }

            //    }
            //    if (lastrowcount - 1 != i)
            //    {
            //        FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(endugpgtotal);
            //    }
            //    FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
            //    endugpgtotal = 0;
            //    FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            //    FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            //}
            //outnum = 0;
            //overallugpgtotal = 0;
            //endugpgtotal = 0;
            //FpSpread1.Sheets[0].Rows.Count++;
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            //cont1++;
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

            //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
            //FpSpread1.SaveChanges();
            //for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
            //{
            //    for (int i = lastrowcount; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
            //    {
            //        if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "TOTAL")
            //        {


            //            string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
            //            if (Int32.TryParse(tt, out outnum))
            //            {
            //                overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
            //                endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

            //            }
            //        }
            //        else
            //        {
            //            FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
            //            FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
            //            endugpgtotal = 0;
            //            FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
            //            FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
            //        }

            //    }
            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
            //    overallugpgtotal = 0;

            //}


            //#endregion










            #region hos

            ugpgtotal = 0;
            endugpgtotal = 0;
            overallugpgtotal = 0;
            FpSpread1.Sheets[0].Rows.Count++;
            lastrowcount = FpSpread1.Sheets[0].Rows.Count;

            if (hatsrow.Count > 0)
            {
                for (int cun = rowadd + 1; cun <= hatsrow.Count + (rowadd); cun++)
                {
                    if (hatsrow.Count >= cun)
                    {
                        FpSpread1.Sheets[0].Cells[lastrowcount - 1, 0].Text = ddltype.SelectedItem.Text.ToString() + " / " + hatsrow[cun];
                        if (dtshow.Rows.Count > 0)
                        {
                            for (int cc = 0; cc < dtshow.Rows.Count; cc++)
                            {
                                FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].Text = dtshow.Rows[cc][1].ToString();
                                FpSpread1.Sheets[0].Cells[lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString())].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].SpanModel.Add(lastrowcount - 1, Convert.ToInt32(dtshow.Rows[cc][0].ToString()), 1, spancount);
                                FpSpread1.Sheets[0].Rows[lastrowcount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
                            }
                        }
                        for (int i = 0; i < chklstdegree.Items.Count; i++)
                        {
                            if (chklstdegree.Items[i].Selected == true)
                            {
                                courseid = chklstdegree.Items[i].Value.ToString();

                                // string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Deptcode in ('" + deptid + "')";

                                //string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.Edu_Level='" + hatsrow[rowadd] + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_name in ('" + deptid + "'))";
                                string sql = "select distinct  c.course_name+'-'+ dt.dept_name as dept_name,dt.dept_name as dept_name1,dt.dept_acronym,d.Dept_Code[Deptcode],c.Course_Id from tbl_DeptGrouping tb,course c,Degree d,Department dt where d.Dept_Code=dt.Dept_Code  and  d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'  and Groupcode in ('" + courseid + "') and d.Dept_Code in ('" + deptid + "') and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.Course_Id in('" + courid + "')";
                                // string sql = " select distinct (select c.course_name+'-'+  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode,c.course_id from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_code in ('" + deptid + "'))";
                                ds.Clear();
                                ds = da.select_method_wo_parameter(sql, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                                    {
                                        FpSpread1.Sheets[0].Rows.Count++;
                                        if (cbacr.Checked)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_acronym"].ToString();
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_name"].ToString();
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["Deptcode"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[ii]["Course_Id"].ToString();
                                        cont1++;
                                    }
                                    FpSpread1.Sheets[0].Rows.Count++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                    cont1++;
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;
                                    //avoirows.Add(FpSpread1.Sheets[0].RowCount - 1, courseid);
                                    avoirows.Add(FpSpread1.Sheets[0].RowCount - 1);
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
                                }
                            }
                        }
                        //  FpSpread1.Sheets[0].Cells[lastrowcount - 1, 0].Tag = "45";
                        // lastrowcount = FpSpread1.Sheets[0].Rows.Count;
                        // FpSpread1.Sheets[0].Rows.Count++;
                        sqlnew = "SELECT r.Current_Semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total,c.Course_Id FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') and c.Edu_Level='" + hatsrow[cun] + "' and c.Course_Id in('" + courid + "') group by r.Current_Semester,d.Dept_Code,dept_name,Edu_Level,sex,c.Course_Id order by d.Dept_Code ";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(sqlnew, "Text");

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = lastrowcount; i <= FpSpread1.Sheets[0].RowCount - 1; i++)
                            {
                                if (!avoirows.Contains(i))
                                {

                                    for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
                                    {
                                        //edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                                        string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                                        if (chckyear.Trim() == "I")
                                        {
                                            chckyear = "  and Current_Semester >=1 and Current_Semester <=2";
                                        }
                                        if (chckyear.Trim() == "II")
                                        {
                                            chckyear = "  and Current_Semester >=3 and Current_Semester <=4";
                                        }
                                        if (chckyear.Trim() == "III")
                                        {
                                            chckyear = "  and Current_Semester >=5 and Current_Semester <=6";
                                        }
                                        if (chckyear.Trim() == "IV")
                                        {
                                            chckyear = "  and Current_Semester >=7 and Current_Semester <=8";
                                        }
                                        sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
                                        string deptidnew = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();//am
                                        string cor = FpSpread1.Sheets[0].Cells[i, 0].Note.ToString();
                                        string filter = "Dept_Code='" + deptidnew + "' and sex='" + sex + "' and Edu_Level='" + hatsrow[cun] + "' " + chckyear + " and  course_id='" + cor + "'";
                                        if (sex.ToUpper().Trim() != "TOTAL")
                                        {

                                            ds.Tables[0].DefaultView.RowFilter = filter;
                                            dv = ds.Tables[0].DefaultView;
                                            int coun = 0;
                                            if (dv.Count > 0)
                                            {
                                                for (int m = 0; m < dv.Count; m++)
                                                {
                                                    coun = coun + Convert.ToInt32(dv[m]["total"]);
                                                    FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(coun);
                                                    ugpgtotal = ugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());
                                                    // overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

                                                }


                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[i, j].Text = "--";

                                            }
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
                                            ugpgtotal = 0;

                                        }
                                        FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                                    }
                                    // FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
                                    // overallugpgtotal = 0;
                                }


                            }

                        }


                        outnum = 0;
                        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
                            {
                                sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();

                                if (sex.ToUpper().Trim() == "TOTAL" && (lastrowcount - 1) != i)
                                {
                                    string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
                                    if (Int32.TryParse(tt, out outnum))
                                    {

                                        endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

                                    }
                                }

                            }
                            if (lastrowcount - 1 != i)
                            {
                                FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(endugpgtotal);
                            }
                            //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
                            endugpgtotal = 0;
                            FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        }

                        // avoirows.Clear();
                        outnum = 0;
                        overallugpgtotal = 0;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
                        FpSpread1.SaveChanges();
                        for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
                        {
                            for (int i = lastrowcount; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
                            {
                                if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "TOTAL")
                                {


                                    string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
                                    if (Int32.TryParse(tt, out outnum))
                                    {
                                        overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
                                        endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

                                    }
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
                                    //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
                                    endugpgtotal = 0;
                                    FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                                }

                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
                            overallugpgtotal = 0;

                        }
                    }
                }
            }

            #endregion









            //outnum = 0;
            //overallugpgtotal = 0;
            //double endugpgtotal1 = 0;
            //FpSpread1.Sheets[0].Rows.Count++;
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            //// FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

            //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
            //FpSpread1.SaveChanges();
            //for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
            //{
            //    for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
            //    {
            //        if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "TOTAL" && FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "GRAND TOTAL")
            //        {


            //            string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
            //            if (Int32.TryParse(tt, out outnum))
            //            {
            //                overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
            //                endugpgtotal = endugpgtotal + Convert.ToDouble(tt);
            //                endugpgtotal1 = endugpgtotal1 + Convert.ToDouble(tt);

            //            }
            //        }
            //        else if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "GRAND TOTAL")
            //        {
            //            FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
            //            //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
            //            endugpgtotal = 0;
            //            FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
            //            FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
            //        }
            //        else if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() == "GRAND TOTAL")
            //        {
            //            FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal1);
            //            endugpgtotal1 = 0;
            //        }
            //    }
            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
            //    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
            //    overallugpgtotal = 0;

            //}
            //    }
            //}
            //        }

            //        #endregion


            outnum = 0;
            overallugpgtotal = 0;
            endugpgtotal = 0;
            FpSpread1.Sheets[0].Rows.Count++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Overall Grand Total";
            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
            FpSpread1.SaveChanges();
            for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
                {
                    if (avoirows.Contains(i))
                    {
                        if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() == "TOTAL")
                        {


                            string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
                            if (Int32.TryParse(tt, out outnum))
                            {
                                overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
                                //endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

                            }
                        }
                        else
                        {
                            // FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
                            //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
                            endugpgtotal = 0;
                            FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                        }
                    }
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
                overallugpgtotal = 0;

            }


            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();




        }
        catch
        {
        }
    }
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
}