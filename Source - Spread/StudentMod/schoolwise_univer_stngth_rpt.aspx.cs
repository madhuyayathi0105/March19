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

public partial class schoolwise_univer_stngth_rpt : System.Web.UI.Page
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

    DataTable dtshow = new DataTable();
    DataSet ds = new DataSet();

    DAccess2 da = new DAccess2();


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
            //count = 0;
            //for (int i = 0; i < cklgender.Items.Count; i++)
            //{
            //    if (cklgender.Items[i].Selected == true)
            //    {
            //        count++;
            //    }
            //}

            //if (count == 0)
            //{
            //    lblerrormsg.Text = "Please Select Atleast One Gender";
            //    hide();
            //    lblerrormsg.Visible = true;
            //    return;


            //}
            //else
            //{
            //    lblerrormsg.Text = "";

            //}


            bindheader();
            bindvalue();



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
        string univcoode = "";

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
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                if (deptid == "")
                {
                    deptid = chklstbranch.Items[i].Value.ToString();
                }
                else
                {
                    deptid = deptid + "','" + chklstbranch.Items[i].Value.ToString();
                }

            }
        }
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                courseid = chklstbranch.Items[i].Text.ToString();

                //string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Dept_Code in (select Dept_Code from Department where dept_name in ('" + deptid + "'))";         
                //ds.Clear();
                //ds = da.select_method_wo_parameter(sql, "Text");
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                //{
                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = courseid;
                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["Deptcode"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = courseid;
                //}
                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;
                //avoirows.Add(FpSpread1.Sheets[0].RowCount - 1, courseid);
                avoirows.Add(FpSpread1.Sheets[0].RowCount - 1);
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
                // }
            }
        }



        string batchyearselected = "0";
        //for (int i = 0; i < chklsbatch.Items.Count; i++)
        //{
        //    if (chklsbatch.Items[i].Selected == true)
        //    {
        //        if (batchyearselected == "")
        //        {
        //            batchyearselected = chklsbatch.Items[i].Value.ToString();
        //        }
        //        else
        //        {
        //            batchyearselected = batchyearselected + "','" + chklsbatch.Items[i].Value.ToString();
        //        }

        //    }
        //}
        if (ddlBatch.Items.Count > 0)
        {
            batchyearselected = ddlBatch.SelectedItem.Text.ToString();
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

        ArrayList totgt = new ArrayList();
        double ugpgtotal = 0;
        double endugpgtotal = 0;
        double overallugpgtotal = 0;
        string sqlnew = "SELECT distinct Edu_Level,Course_Name,U.Univ_code,d.Dept_Name,count( distinct p.App_No) total FROM Applyn A,Registration R,Degree G,Course C,Department D,Stud_prev_details P,TextValTable T,tbl_UnivGrouping U where a.app_no = r.App_No and a.app_no = p.app_no and p.course_code = t.TextCode and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and p.course_code = U.UnivText_code  and  Course_Name not like '%M.phil%'   and r.Batch_Year='" + batchyearselected + "' and CC=0 and DelFlag=0 and Exam_Flag='OK' group by Edu_Level,Course_Name,U.Univ_code,d.Dept_Name   ";
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
                        if (!avoidcol.Contains(j))
                        {
                            edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                            univcoode = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Note.ToString();
                            deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                            string filter = "dept_name='" + deptid + "' and Univ_code='" + univcoode + "' and Edu_Level='" + edulevelid + "'";
                            if (FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString().ToUpper().Trim() != "TOTAL")
                            {
                                ds.Tables[0].DefaultView.RowFilter = filter;
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    for (int m = 0; m < dv.Count; m++)
                                    {
                                        FpSpread1.Sheets[0].Cells[i, j].Text = dv[0]["total"].ToString();
                                        ugpgtotal = ugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());
                                        //overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

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
                else
                {
                    FpSpread1.Sheets[0].Rows[i].Visible = false;
                }


            }

        }


        sqlnew = "SELECT distinct Edu_Level,Course_Name,U.Univ_code,d.Dept_Name,count( distinct p.App_No) total FROM Applyn A,Registration R,Degree G,Course C,Department D,Stud_prev_details P,TextValTable T,tbl_UnivGrouping U where a.app_no = r.App_No and a.app_no = p.app_no and p.course_code = t.TextCode and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and p.course_code = U.UnivText_code  and  Course_Name  like '%M.phil%'   and r.Batch_Year='" + batchyearselected + "' and CC=0 and DelFlag=0 and Exam_Flag='OK' group by Edu_Level,Course_Name,U.Univ_code,d.Dept_Name   ";
        ds.Clear();
        ds = da.select_method_wo_parameter(sqlnew, "Text");

        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
        {
            if (!avoirows.Contains(i))
            {


                for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
                {
                    if (avoidcol.Contains(j))
                    {
                        edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                        univcoode = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Note.ToString();
                        deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                        string filter = "dept_name='" + deptid + "' and Univ_code='" + univcoode + "' and Edu_Level='" + edulevelid + "'";
                        if (FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString().ToUpper().Trim() != "TOTAL")
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ds.Tables[0].DefaultView.RowFilter = filter;
                                dv = ds.Tables[0].DefaultView;
                            }
                            if (dv.Count > 0)
                            {
                                for (int m = 0; m < dv.Count; m++)
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = dv[0]["total"].ToString();
                                    ugpgtotal = ugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());
                                    //overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

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
            else
            {
                FpSpread1.Sheets[0].Rows[i].Visible = false;
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




    public void bindheader()
    {
        //dtshow.Columns.Add("UG/PG");
        //dtshow.Columns.Add("M/W/TR");
        //dtshow.Columns.Add("colno");
        DataTable ugpgdt = new DataTable();
        ugpgdt.Columns.Add("UGPG");
        ugpgdt.Columns.Add("Values");
        ugpgdt.Columns.Add("code");
        string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'UnvGp' and college_code = '" + Session["collegecode"].ToString() + "' ";

        ds.Clear();

        ds = da.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string ss = ds.Tables[0].Rows[i][1].ToString();
                string[] splitstr = ss.Split('-');
                ListItem li = new ListItem();
                li.Text = splitstr[1];
                li.Value = ds.Tables[0].Rows[i][0].ToString();
                ugpgdt.Rows.Add(splitstr[0], splitstr[1], ds.Tables[0].Rows[i][0].ToString());

            }
            //ddltitlename.DataSource = ds;
            //ddltitlename.DataTextField = "TextVal";
            //ddltitlename.DataValueField = "TextCode";
            //ddltitlename.DataBind();

        }
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
        DataView dvsections = new DataView();


        FpSpread1.Sheets[0].RowHeader.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
        FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "DEPARTMENT";
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = ddltype.SelectedItem.Text.ToString();
        string sql = "select distinct course.Edu_Level,course.Edu_Level from  course order by course.Edu_Level desc";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        int col = 1;
        int stracol = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].Rows.Add("M.Phil", "PG");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ugpgdt.DefaultView.RowFilter = "UGPG ='" + ds.Tables[0].Rows[i][1].ToString() + "'";
                dvsections = ugpgdt.DefaultView;

                FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + dvsections.Count;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, col - 1].Text = ds.Tables[0].Rows[i][0].ToString();
                //if(ds.Tables[0].Rows[i][0].ToString()=="M.Phil")
                //{
                //    avoidcol.Add(col);
                //}
                stracol = col;
                for (int j = 0; j < dvsections.Count; j++)
                {


                    if (ds.Tables[0].Rows[i][0].ToString() == "M.Phil")
                    {
                        avoidcol.Add(col);
                    }
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text = dvsections[j][1].ToString();
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Note = dvsections[j][2].ToString();
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = ds.Tables[0].Rows[i][0].ToString();
                    if (ds.Tables[0].Rows[i][0].ToString() == "M.Phil")
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = "PG";
                    }
                    dtshow.Rows.Add(ds.Tables[0].Rows[i][0].ToString(), dvsections[j][1].ToString(), col);
                    col++;

                }
                FpSpread1.Sheets[0].Columns.Count++;
                if (ds.Tables[0].Rows[i][0].ToString() == "M.Phil")
                {
                    avoidcol.Add(FpSpread1.Sheets[0].Columns.Count - 1);
                }
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Text = "Total";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Tag = ds.Tables[0].Rows[i][0].ToString();
                dtshow.Rows.Add("Total", "", FpSpread1.Sheets[0].Columns.Count - 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, stracol, 1, dvsections.Count + 1);

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
            string strsql = "select distinct top 3 batch_year from Registration where batch_year<>'-1' and batch_year<>''  and delflag=0 and exam_flag<>'debar'  order by batch_year desc";
            ds2 = da.select_method_wo_parameter(strsql, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {

                ddlBatch.DataSource = ds2;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();

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
        if (ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0)
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
        }}
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
                    string strquery = "          select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept from tbl_DeptGrouping tb where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + course_id + "')";
                    ds2 = da.select_method_wo_parameter(strquery, "Text");

                }
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds2;
                    chklstbranch.DataTextField = "dept";
                    chklstbranch.DataValueField = "dept";
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
                            txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
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
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
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
                txtbranch.Text = "Branch(" + commcount.ToString() + ")";
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
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Text = "";
        hide();
        //BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        lblerrormsg.Visible = true;
        return;
    }


}