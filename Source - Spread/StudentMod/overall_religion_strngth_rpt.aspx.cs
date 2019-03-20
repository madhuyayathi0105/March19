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

public partial class overall_religion_strngth_rpt : System.Web.UI.Page
{
    DataTable dtshow = new DataTable();
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
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    DataSet studgradeds = new DataSet();
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
                rbreligion.Checked = true;
                cklgender.Items.Add("Male");
                cklgender.Items.Add("Female");
                cklgender.Items.Add("Transgender");
                final.Visible = false;
                loadtype();
                Bindcollege();
                bindedulevel();
                collegecode = ddlcollege.SelectedValue.ToString();
                BindBatch();
                // BindDegree(singleuser, group_user, collegecode, usercode);
                bindgroup();
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                FpSpread1.Visible = false;
                bindreligion();
                // bindcommunity();
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

    public void bindreligion()
    {

        ds.Clear();
        txtregcom.Text = "--Select--";
        string cmd_bind_route = "  select distinct TextCode,textval from applyn , textvaltable where TextCode=religion  and textval<>''";

        ds = d2.select_method_wo_parameter(cmd_bind_route, "Text");
        chklregcom.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            chklregcom.DataSource = ds;
            chklregcom.DataTextField = "textval";
            chklregcom.DataValueField = "TextCode";
            chklregcom.DataBind();

        }
        else
        {
            txtregcom.Text = "--Select--";
            txtregcom.Text = "--Select--";
        }

    }
    public void bindcommunity()
    {

        ds.Clear();
        txtregcom.Text = "--Select--";
        string cmd_bind_route = "select distinct TextCode,textval from applyn , textvaltable where TextCode=community and textval<>''";

        ds = d2.select_method_wo_parameter(cmd_bind_route, "Text");
        chklregcom.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            chklregcom.DataSource = ds;
            chklregcom.DataTextField = "textval";
            chklregcom.DataValueField = "TextCode";
            chklregcom.DataBind();

        }
        else
        {
            txtregcom.Text = "--Select--";
            txtregcom.Text = "--Select--";
        }

    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {

            dtshow.Columns.Add("UG/PG");
            dtshow.Columns.Add("M/W/TR");
            dtshow.Columns.Add("colno");

            lblnorec.Text = "";
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

            count = 0;
            for (int i = 0; i < chklregcom.Items.Count; i++)
            {
                if (chklregcom.Items[i].Selected == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Religion / Community";
                hide();
                lblerrormsg.Visible = true;
                return;


            }
            else
            {
                lblerrormsg.Text = "";

            }

            bindheader();
            bindvalue();
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

    public void bindheader()
    {
        //dtshow.Columns.Add("UG/PG");
        //dtshow.Columns.Add("M/W/TR");
        //dtshow.Columns.Add("colno");
        int spancol1 = 0;

        int genderselcount = 0;
        for (int j = 0; j < cklgender.Items.Count; j++)
        {
            if (cklgender.Items[j].Selected == true)
            {
                genderselcount++;

            }
        }
        int relcommcount = 0;
        for (int j = 0; j < chklregcom.Items.Count; j++)
        {
            if (chklregcom.Items[j].Selected == true)
            {
                relcommcount++;


            }
        }

        relcommcount = ((relcommcount) * genderselcount);
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

        string brancode = "";
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                if (brancode.Trim() == "")
                {
                    brancode = chklstbranch.Items[i].Value.ToString();
                }
                else
                {
                    brancode = brancode + ',' + chklstbranch.Items[i].Value.ToString();
                }
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
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 3;
        FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "BRANCH";
        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = ddltype.SelectedItem.Text.ToString();
        //string sql = "select distinct r.Current_Semester from Registration r,applyn a where r.app_no=a.app_no and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'  and r.batch_year in (" + batch + ") and r.Current_Semester<=6 order by r.Current_Semester ";
        string sql = "";
        if (ddledulevel.SelectedItem.Text.Trim().ToString().ToLower() == "ug")
        {
            string duration = da.GetFunction("select top 1  duration from degree g,course c where g.Course_Id = c.Course_Id  and c.Edu_Level='ug'");
            sql = "select distinct r.Current_Semester from Registration r,applyn a where r.app_no=a.app_no and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'  and r.batch_year in (" + batch + ") and r.degree_code in(" + brancode + ") and r.Current_Semester<='" + duration + "' order by r.Current_Semester ";
        }
        else
        {
            string duration = da.GetFunction("select top 1  duration from degree g,course c where g.Course_Id = c.Course_Id  and c.Edu_Level='" + ddledulevel.SelectedItem.Text.Trim().ToString() + "'");
            sql = "select distinct r.Current_Semester from Registration r,applyn a where r.app_no=a.app_no and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'  and r.batch_year in (" + batch + ") and r.degree_code in(" + brancode + ") and r.Current_Semester<='" + duration + "' order by r.Current_Semester ";
        }

        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        int col = 1;
        int stracol = 0;
        string year = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + relcommcount;
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
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text = year + "  " + ddledulevel.SelectedItem.Text.ToString();
                stracol = col;
                FpSpread1.Sheets[0].Columns.Count++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, stracol].Text = "S.S";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, stracol].Tag = year;
                col++;
                for (int j = 0; j < chklregcom.Items.Count; j++)
                {
                    if (chklregcom.Items[j].Selected == true)
                    {
                        spancol1 = col;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text = chklregcom.Items[j].Text.ToString();
                        // FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = ds.Tables[0].Rows[i][0].ToString();
                        // dtshow.Rows.Add(ds.Tables[0].Rows[i][0].ToString(), sex, col);
                        //col++;
                        for (int ij = 0; ij < cklgender.Items.Count; ij++)
                        {
                            if (cklgender.Items[ij].Selected == true)
                            {
                                string sex = "";
                                if (ij == 0)
                                {
                                    sex = "M";
                                }
                                else if (ij == 1)
                                {
                                    sex = "W";
                                }
                                else
                                {
                                    sex = "TR";
                                }
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, col].Text = sex;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, col].Tag = chklregcom.Items[j].Value.ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, col].Note = year;
                                //   dtshow.Rows.Add(ds.Tables[0].Rows[i][0].ToString(), sex, col);
                                col++;
                            }
                        }
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, spancol1--, 1, genderselcount);
                    }
                }
                spancol1 = col;
                FpSpread1.Sheets[0].Columns.Count++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, spancol1].Text = "T.L";
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
                        FpSpread1.Sheets[0].Columns.Count++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col].Text = sex;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col].Tag = "T.L";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col].Note = year;
                        //   dtshow.Rows.Add(ds.Tables[0].Rows[i][0].ToString(), sex, col);
                        col++;
                    }
                }
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, spancol1--, 1, genderselcount);

                //FpSpread1.Sheets[0].Columns.Count++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text = "G.T";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = ds.Tables[0].Rows[i][0].ToString();
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, col, 2, 1);
                // dtshow.Rows.Add("Total", "", FpSpread1.Sheets[0].Columns.Count - 1);

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, stracol, 1, (col - stracol) + 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, stracol, 2, 1);


                col++;
            }
        }
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
        //if (FpSpread1.Sheets[0].Columns.Count > 1)
        //{
        //    FpSpread1.Sheets[0].Columns.Count++;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].Columns.Count - 1].Text = "G.T";
        //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].Columns.Count - 1, 2, 1);
        //}
        FpSpread1.Visible = true;

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
        string reglicommcodes = "";
        for (int j = 0; j < chklregcom.Items.Count; j++)
        {
            if (chklregcom.Items[j].Selected == true)
            {


                if (reglicommcodes == "")
                {
                    reglicommcodes = chklregcom.Items[j].Value.ToString();
                }
                else
                {
                    reglicommcodes = reglicommcodes + "','" + chklregcom.Items[j].Value.ToString();
                }

            }
        }
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
        string course = string.Empty;
        for (int i = 0; i < chklstdegree.Items.Count; i++)
        {
            if (chklstdegree.Items[i].Selected == true)
            {
                courseid = chklstdegree.Items[i].Value.ToString();

                //   string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Deptcode in ('" + deptid + "')";
                string sql = "select distinct course_name + '-' +(select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode,c.course_id from tbl_DeptGrouping tb,course c,Degree d where tb.type=c.type and tb.Deptcode=d.Dept_Code and c.Course_Id=d.Course_Id and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and tb.type='" + ddltype.SelectedItem.Text.ToString() + "' and tb.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Deptcode in ('" + deptid + "')";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[0].Rows[ii]["dept_name"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[ii]["Deptcode"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[ii]["course_id"].ToString();
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


        string dvfiltrelcomm = "";
        string selectioncommureli = "";
        if (rbreligion.Checked == true)
        {
            selectioncommureli = "a.religion,";
            dvfiltrelcomm = "religion";
        }
        else
        {

            selectioncommureli = "a.community,";
            dvfiltrelcomm = "community";
        }

        double ugpgtotal = 0;
        double endugpgtotal = 0;
        double overallugpgtotal = 0;

        string sqlnew = "SELECT r.current_semester,d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level," + selectioncommureli + " case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total,c.course_id FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') group by c.course_id,r.current_semester,d.Dept_Code,dept_name,Edu_Level," + selectioncommureli + "sex order by d.Dept_Code ;";

        ds.Clear();
        ds = da.select_method_wo_parameter(sqlnew, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                if (!avoirows.Contains(i))
                {

                    for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
                    {
                        course = FpSpread1.Sheets[0].Cells[i, 0].Note.ToString();
                        sex = FpSpread1.Sheets[0].ColumnHeader.Cells[2, j].Text.ToString();
                        if (FpSpread1.Sheets[0].ColumnHeader.Cells[2, j].Text.ToString().Trim().ToUpper() != "")
                        {

                            edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[2, j].Tag.ToString();
                            if (edulevelid.Trim().ToUpper() != "T.L")
                            {
                                deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                                string chckyear = FpSpread1.Sheets[0].ColumnHeader.Cells[2, j].Note.ToString();
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
                                string filter = "Dept_Code='" + deptid + "' and sex='" + sex + "' and course_id='" + course + "' and Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and " + dvfiltrelcomm + "= '" + edulevelid + "' " + chckyear + "";

                                ds.Tables[0].DefaultView.RowFilter = filter;
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    int tot = 0;
                                    for (int m = 0; m < dv.Count; m++)
                                    {
                                        int total = Convert.ToInt32(dv[m]["total"].ToString());

                                        tot = tot + total;
                                        FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(tot);//delsi1603
                                        ugpgtotal = ugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());
                                        overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[m]["total"].ToString());

                                    }


                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = "--";

                                }
                            }
                            else
                            {
                                edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[2, j].Note.ToString();
                                if (edulevelid.Trim() == "I")
                                {
                                    edulevelid = "  and r.Current_Semester between 1 and 2";
                                }
                                if (edulevelid.Trim() == "II")
                                {
                                    edulevelid = "  and r.Current_Semester between 3 and 4";
                                }
                                if (edulevelid.Trim() == "III")
                                {
                                    edulevelid = "  and r.Current_Semester between 5 and 6";
                                }
                                if (edulevelid.Trim() == "IV")
                                {
                                    edulevelid = "  and r.Current_Semester between 7 and 8";
                                }


                                string sqlnewfinal = "SELECT  case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "')  and d.Dept_Code='" + deptid + "' and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.course_id='" + course + "'  and a." + dvfiltrelcomm + " in('" + reglicommcodes + "') " + edulevelid + "   group by sex;";

                                ds2.Clear();
                                ds2 = da.select_method_wo_parameter(sqlnewfinal, "Text");
                                string filter = "sex='" + sex + "'";

                                ds2.Tables[0].DefaultView.RowFilter = filter;
                                dv = ds2.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = dv[0]["total"].ToString();

                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = "--";

                                }
                            }


                        }
                        else if (FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString().Trim().ToUpper() == "G.T")
                        {
                            FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
                            ugpgtotal = 0;

                        }
                        else if (FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString().Trim().ToUpper() == "S.S")
                        {

                            deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                            course = FpSpread1.Sheets[0].Cells[i, 0].Note.ToString();
                            string sqlnewfinal = " select SUM(No_Of_seats) as total from Degree d,course c where d.Course_Id=c.Course_Id and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and d.Dept_Code in ('" + deptid + "') and c.course_id in('" + course + "')";

                            ds2.Clear();
                            ds2 = da.select_method_wo_parameter(sqlnewfinal, "Text");
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[i, j].Text = ds2.Tables[0].Rows[0]["total"].ToString();

                            }

                        }
                        FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;

                    }
                    // FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
                    overallugpgtotal = 0;
                }


            }

        }
        int outnum = 0;
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


            //// string date_filt = "From : " + tbstart_date.Text.ToString() + "   " + "To : " + tbend_date.Text.ToString();
            //string degreeset = da.GetFunction("select (Course_Name+' - '+Acronym) as degreeset from course c, degree d where c.Course_Id=d.Course_Id and Degree_Code='" + ddstandard.SelectedItem.Value.ToString() + "'");
            //degreeset=degreeset+" - "+ ddlSemYr.SelectedItem.Text.ToString();
            //string strsec = "";
            //if (ddlSec.Enabled == true)
            //{
            //    string sections = ddlSec.SelectedItem.Text.ToString();
            //    if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
            //    {
            //        strsec = "";
            //    }
            //    else
            //    {
            //        strsec = " - " + sections.ToString();
            //    }

            //}
            //degreeset = degreeset +  strsec;

            //int batchyear = Convert.ToInt32(dropyear.SelectedItem.Text.ToString());

            //string date_filt = "Batch : ";

            //date_filt = date_filt + "@" + "Degree : ";
            string degreedetails = string.Empty;
            string stream = "";
            if (ddltype.SelectedItem.Text.ToLower().Trim() == "day")
            {
                stream = "AIDED STREAM";
            }
            if (ddltype.SelectedItem.Text.ToLower().Trim() == "evening")
            {
                stream = "SELF FINANCED STREAM";
            }
            degreedetails = "STUDENTS STRENGTH  " + stream + "";
            string pagename = "overall_religion_strngth_rpt.aspx";
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

    //        lblerrormsg.Visible = false;
    //        count = 0;
    //        chklstdegree.Items.Clear();
    //        if (group_user.Contains(';'))
    //        {
    //            string[] group_semi = group_user.Split(';');
    //            group_user = group_semi[0].ToString();
    //        }
    //        ds2.Dispose();
    //        ds2.Reset();
    //        ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
    //        if (ds2.Tables[0].Rows.Count > 0)
    //        {
    //            chklstdegree.DataSource = ds2;
    //            chklstdegree.DataTextField = "course_name";
    //            chklstdegree.DataValueField = "course_id";
    //            chklstdegree.DataBind();
    //            chklstdegree.Items[0].Selected = true;
    //            for (int i = 0; i < chklstdegree.Items.Count; i++)
    //            {
    //                chklstdegree.Items[i].Selected = true;
    //                if (chklstdegree.Items[i].Selected == true)
    //                {
    //                    count += 1;
    //                }
    //                if (chklstdegree.Items.Count == count)
    //                {
    //                    chkdegree.Checked = true;
    //                }
    //            }
    //            if (chkdegree.Checked == true)
    //            {
    //                for (int i = 0; i < chklstdegree.Items.Count; i++)
    //                {
    //                    chklstdegree.Items[i].Selected = true;
    //                    txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
    //                }
    //            }
    //            else
    //            {
    //                for (int i = 0; i < chklstdegree.Items.Count; i++)
    //                {
    //                    chklstdegree.Items[i].Selected = false;
    //                    txtdegree.Text = "---Select---";
    //                }
    //            }
    //            txtdegree.Enabled = true;
    //        }
    //        else
    //        {
    //            txtdegree.Enabled = false;
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
                // BindBranchMultiple();
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
                    string strquery = "          select distinct course_name + '-' +(select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept,Deptcode,c.course_id from tbl_DeptGrouping tb,course c,Degree d where tb.type=c.type and tb.Deptcode=d.Dept_Code and c.Course_Id=d.Course_Id and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and tb.type='" + ddltype.SelectedItem.Text.ToString() + "' and tb.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + course_id + "')";
                    ds2 = da.select_method_wo_parameter(strquery, "Text");

                }
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds2;
                    chklstbranch.DataTextField = "dept";
                    chklstbranch.DataValueField = "Deptcode";
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
                            if (checkSchoolSetting() == 0)
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
                txtdegree.Text = "Group(" + (chklstdegree.Items.Count) + ")";
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
                txtdegree.Text = "Group(" + commcount.ToString() + ")";
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
            //  BindDegree(singleuser, group_user, collegecode, usercode);
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

    protected void rbreligion_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblrpttype.Text = "Religion";
            bindreligion();
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
    protected void rbcommunity_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblrpttype.Text = "Community";
            bindcommunity();
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
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            bindgroup();
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
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
    protected void chkregcom_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            txtregcom.Text = "---Select---";
            if (chkregcom.Checked == true)
            {
                for (int i = 0; i < chklregcom.Items.Count; i++)
                {
                    chklregcom.Items[i].Selected = true;
                }
                if (rbreligion.Checked == true)
                {
                    txtregcom.Text = "Religion(" + (chklregcom.Items.Count) + ")";
                }
                else
                {
                    txtregcom.Text = "Community(" + (chklregcom.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < chklregcom.Items.Count; i++)
                {
                    chklregcom.Items[i].Selected = false;
                }
                txtregcom.Text = "---Select---";

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

    protected void chklregcom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            int commcount = 0;
            chkregcom.Checked = false;
            txtregcom.Text = "---Select---";
            for (int i = 0; i < chklregcom.Items.Count; i++)
            {
                if (chklregcom.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                // txtregcom.Text = "Gender(" + commcount.ToString() + ")"; 
                if (rbreligion.Checked == true)
                {
                    txtregcom.Text = "Religion(" + commcount.ToString() + ")";
                }
                else
                {
                    txtregcom.Text = "Community(" + commcount.ToString() + ")";
                }
                if (commcount == chklregcom.Items.Count)
                {
                    chkregcom.Checked = true;
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
    protected void ddledulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Text = "";
        hide();
        lblerrormsg.Visible = true;
        bindgroup();
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        return;
    }
    public void bindedulevel()
    {
        string sql = "select distinct Edu_Level from course where college_code='" + ddlcollege.SelectedItem.Value.ToString() + "' order by Edu_Level desc";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        ddledulevel.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddledulevel.DataSource = ds;
            ddledulevel.DataTextField = "Edu_Level";
            ddledulevel.DataBind();
        }

    }
    public void hide()
    {
        Printcontrol.Visible = false;
        FpSpread1.Visible = false;
        final.Visible = false;

    }

    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
}