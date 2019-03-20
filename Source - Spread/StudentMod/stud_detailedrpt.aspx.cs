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
public partial class stud_detailedrpt : System.Web.UI.Page
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
                lblstatus.Visible = false;
                ddlstatus.Visible = false;
                final.Visible = false;
                loadtype();
                Bindcollege();
                collegecode = ddlcollege.SelectedValue.ToString();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string Master = "select * from Master_Settings where settings in('Roll No','Register No','Admission No') " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Admissionflag"] = "1";
                        }
                    }
                }
                FpSpread1.Visible = false;
            }
            lblerrormsg.Visible = false;
        }
        catch (Exception ex) { }
    }


    //added by abarna for schoolsetting and collegesetting based label displayed on that screen
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

        lbl.Add(lbldegree);
        lbl.Add(lblbranch);

        fields.Add(0);

        fields.Add(2);

        fields.Add(3);


        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }


    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Text = "";
            hide();
            lblerrormsg.Text = "";
            lblerrormsg.Visible = true;
            string batchyear = "";
            string degreecode = "";
            string type = ddltype.SelectedItem.Text.ToString();
            int count = 0;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    count++;
                    if (batchyear == "")
                    {
                        batchyear = chklsbatch.Items[i].Text.ToString();
                    }
                    else
                    {
                        batchyear = batchyear + "','" + chklsbatch.Items[i].Text.ToString();
                    }
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
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    count++;
                    if (degreecode == "")
                    {
                        degreecode = chklstbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        degreecode = degreecode + "','" + chklstbranch.Items[i].Value.ToString();
                    }
                }
            }
            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Degree";
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
            Dictionary<string, double> GenderCountDic = new Dictionary<string, double>();
            if (ddlrpttype.SelectedIndex == 0)
            {
                #region Exservice
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
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
                FpSpread1.Sheets[0].ColumnCount = 11;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                // FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name of the Student";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "C.I & Gr.";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Father Name & Address";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Regiment";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Rank & Number";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Community";
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Width = 40;
                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[2].Width = 150;
                FpSpread1.Sheets[0].Columns[3].Width = 150;
                FpSpread1.Sheets[0].Columns[4].Width = 180;
                FpSpread1.Sheets[0].Columns[5].Width = 180;
                FpSpread1.Sheets[0].Columns[6].Width = 150;
                FpSpread1.Sheets[0].Columns[7].Width = 200;
                FpSpread1.Sheets[0].Columns[8].Width = 85;
                FpSpread1.Sheets[0].Columns[9].Width = 150;
                FpSpread1.Sheets[0].Columns[10].Width = 150;
                //FpSpread1.Width = 1000;
                string sql = " SELECT r.Stud_Name,r.Current_Semester,Course_Name+'-'+Dept_Name Course,Parent_Name+' '+parent_addressP+' '+Streetp+' '+Cityp+' '+parent_pincodep as addressmcc,isnull(Regiment,'') Regiment ,isnull(ExSRank,'') +'-'+isnull(ExSNumber,'') RankNum,(select textval from textvaltable t where t.TextCode = a.community ) community,(select textval from textvaltable t where t.TextCode = a.religion ) religion,r.degree_code,a.sex, case when a.sex=0 then 'Male' when a.sex ='1' then 'Female' when a.sex=2 then 'TransGender' end Gender,1 dummy,r.roll_no,r.Reg_No,r.Roll_Admit from Registration r,applyn a,Degree g,course c,Department d where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and ISNULL(IsExService,0) = 1 and r.Batch_Year in ('" + batchyear + "') and r.degree_code in ('" + degreecode + "') and c.type='" + type + "'  and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "Text");
                int height = 0;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count);
                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                        FpSpread1.Sheets[0].Columns[2].Visible = false;
                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                        if (Convert.ToString(Session["Rollflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[1].Visible = true;
                        if (Convert.ToString(Session["Regflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[2].Visible = true;
                        if (Convert.ToString(Session["Admissionflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[ii]["roll_no"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[ii]["Reg_No"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[ii]["Roll_Admit"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[ii]["Stud_Name"].ToString();
                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[ii]["Stud_Name"].ToString();
                        int sem = Convert.ToInt32(ds.Tables[0].Rows[ii]["Current_Semester"].ToString());
                        string year = "";
                        if (sem >= 1 && sem <= 2)
                        {
                            year = "I";
                        }
                        else if (sem >= 3 && sem <= 4)
                        {
                            year = "II";
                        }
                        else if (sem >= 5 && sem <= 6)
                        {
                            year = "III";
                        }
                        else if (sem >= 7 && sem <= 8)
                        {
                            year = "IV";
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[ii]["Gender"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = year + " " + ds.Tables[0].Rows[ii]["Course"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[ii]["addressmcc"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[ii]["Regiment"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = ds.Tables[0].Rows[ii]["RankNum"].ToString();
                        string comm = "";
                        string f1 = ds.Tables[0].Rows[ii]["community"].ToString();
                        string f2 = ds.Tables[0].Rows[ii]["religion"].ToString();
                        if (f1.Trim() != "" && f1 != null && f1.Length > 1)
                        {
                            comm = f1;
                        }
                        if (f2.Trim() != "" && f2 != null && f2.Length > 1)
                        {
                            if (comm.Trim() != "" && comm != null)
                            {
                                comm = comm + " / " + f2;
                            }
                            else
                            {
                                comm = f2;
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = comm;
                        height = height + FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height;
                    }
                    #region Include Total
                    if (cb_includetotal.Checked)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "S.No";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Branch Name";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Male";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "Female";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Transgender";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = "Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                        double rowTotal = 0;
                        double overallTotal = 0;
                        for (int t = 0; t < chklstbranch.Items.Count; t++)
                        {
                            rowTotal = 0;
                            if (chklstbranch.Items[t].Selected == true)
                            {
                                FpSpread1.Sheets[0].Rows.Count++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(t + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(chklstbranch.Items[t].Text);
                                double countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='0'")), out countval);
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                if (GenderCountDic.ContainsKey("0"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["0"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["0"] = total;
                                }
                                else
                                    GenderCountDic.Add("0", countval);
                                countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='1'")), out countval);
                                }
                                if (GenderCountDic.ContainsKey("1"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["1"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["1"] = total;
                                }
                                else
                                    GenderCountDic.Add("1", countval);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='2'")), out countval);
                                }
                                if (GenderCountDic.ContainsKey("2"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["2"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["2"] = total;
                                }
                                else
                                    GenderCountDic.Add("2", countval);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(rowTotal);
                                overallTotal += rowTotal;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                            }
                        }
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Grand Total";
                        string GenderCount = string.Empty;
                        if (GenderCountDic.ContainsKey("0"))
                            GenderCount = Convert.ToString(GenderCountDic["0"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = GenderCount;
                        if (GenderCountDic.ContainsKey("1"))
                            GenderCount = Convert.ToString(GenderCountDic["1"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = GenderCount;
                        if (GenderCountDic.ContainsKey("2"))
                            GenderCount = Convert.ToString(GenderCountDic["2"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = GenderCount;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(overallTotal);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[8].ForeColor = Color.Black;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[8].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Columns[8].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[8].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[9].ForeColor = Color.Black;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[9].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[9].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Columns[9].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[9].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                    }
                    #endregion
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    FpSpread1.Height = height + 100;
                    FpSpread1.Visible = true;
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
                        FpSpread1.Sheets[0].Columns[i].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Columns[i].Locked = true;
                    }
                    final.Visible = true;
                }
                #endregion
            }
            else if (ddlrpttype.SelectedIndex == 2)
            {
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
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
                FpSpread1.Sheets[0].ColumnCount = 11;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                // FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name of the Student";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Course of Study";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Year of Study";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Native State";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Permanent Address";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Contact No";
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Width = 40;
                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[2].Width = 150;
                FpSpread1.Sheets[0].Columns[3].Width = 150;
                FpSpread1.Sheets[0].Columns[4].Width = 180;
                FpSpread1.Sheets[0].Columns[5].Width = 100;
                FpSpread1.Sheets[0].Columns[6].Width = 200;
                FpSpread1.Sheets[0].Columns[7].Width = 85;
                FpSpread1.Sheets[0].Columns[8].Width = 150;
                FpSpread1.Sheets[0].Columns[9].Width = 150;
                FpSpread1.Sheets[0].Columns[10].Width = 150;
                FpSpread1.Width = 1000;
                string sql = "SELECT parent_phnop,r.Stud_Name,Course_Name+'-'+Dept_Name Course,Str(R.Batch_Year) + '-' + LTRIM(RTRIM(STR(R.Batch_Year + ((select Duration from Degree m where m.Degree_Code = g.degree_code)/2)))) as year,(SELECT TextVal FROM TextValTable T WHERE T.TextCode = A.NativeState) NativeState100,parent_addressP,Streetp,Cityp,parent_pincodep,CASE WHEN ISNUMERIC(DistrictP) = 1 THEN (SELECT TextVal FROM TextValTable M WHERE M.TextCode  = A.DistrictP) ELSE Districtp END District,(SELECT TextVal FROM TextValTable K WHERE K.TextCode  = A.Parent_StateP ) State,(SELECT TextVal FROM TextValTable S WHERE S.TextCode = A.Parent_statep) NativeState,parent_phnop,parentF_Mobile,r.roll_no,r.Reg_No,r.Roll_Admit,r.degree_code,a.sex, case when a.sex=0 then 'Male' when a.sex ='1' then 'Female' when a.sex=2 then 'TransGender' end Gender,1 dummy from Registration r,applyn a,Degree g,course c,Department d where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and r.Batch_Year in ('" + batchyear + "') and r.degree_code in ('" + degreecode + "') and c.type='" + type + "'  and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and A.Parent_StateP not in (SELECT distinct TextCode FROM TextValTable K WHERE K.TextCode  = A.Parent_StateP and k.TextVal like '%tamil nadu%' or k.TextVal like '%tamilnadu%') order by r.Stud_Name ";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "Text");
                int height = 0;
                //FpSpread1.Sheets[0].RowCount = 0;
                //FpSpread1.Sheets[0].ColumnCount = 7;
                //FpSpread1.SaveChanges();
                DataView dv = new DataView();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].DefaultView.RowFilter = "State  not like '%tamil nadu%' and State  not like '%tamilnadu%'";
                    dv = ds.Tables[0].DefaultView;
                    for (int ii = 0; ii < dv.Count; ii++)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count);
                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                        FpSpread1.Sheets[0].Columns[2].Visible = false;
                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                        if (Convert.ToString(Session["Rollflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[1].Visible = true;
                        if (Convert.ToString(Session["Regflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[2].Visible = true;
                        if (Convert.ToString(Session["Admissionflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[ii]["roll_no"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[ii]["Reg_No"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[ii]["Roll_Admit"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dv[ii]["Stud_Name"].ToString();
                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[ii]["Stud_Name"].ToString();
                        //int sem = Convert.ToInt32(ds.Tables[0].Rows[ii]["Current_Semester"].ToString());
                        //string year = "";
                        //if (sem >= 1 && sem <= 2)
                        //{
                        //    year = "I";
                        //}
                        //else if (sem >= 3 && sem <= 4)
                        //{
                        //    year = "II";
                        //}
                        //else if (sem >= 5 && sem <= 6)
                        //{
                        //    year = "III";
                        //}
                        //else if (sem >= 7 && sem <= 8)
                        //{
                        //    year = "IV";
                        //}
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = dv[ii]["Gender"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = dv[ii]["Course"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = dv[ii]["year"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = dv[ii]["NativeState"].ToString();
                        string addresss = dv[ii]["parent_addressP"].ToString() + " " + dv[ii]["Streetp"].ToString() + " " + dv[ii]["Cityp"].ToString() + " " + dv[ii]["parent_pincodep"].ToString() + " " + dv[ii]["District"].ToString() + " " + dv[ii]["State"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = addresss;
                        string phno = "";
                        string f1 = ds.Tables[0].Rows[ii]["parentF_Mobile"].ToString();
                        string f2 = ds.Tables[0].Rows[ii]["parent_phnop"].ToString();
                        if (f1.Trim() != "" && f1 != null && f1.Length > 1)
                        {
                            phno = f1;
                        }
                        if (f2.Trim() != "" && f2 != null && f2.Length > 1)
                        {
                            if (phno.Trim() != "" && phno != null)
                            {
                                phno = phno + " / " + f2;
                            }
                            else
                            {
                                phno = f2;
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].CellType = txtceltype;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = phno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].CellType = txtceltype;
                        height = height + FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height;
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    FpSpread1.Height = height + 100;
                    FpSpread1.Visible = true;
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
                        FpSpread1.Sheets[0].Columns[i].Locked = true;
                        FpSpread1.Sheets[0].Columns[i].VerticalAlign = VerticalAlign.Middle;
                    }
                    #region Include Total
                    if (cb_includetotal.Checked)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "S.No";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Branch Name";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Male";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Female";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "Transgender";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.White;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                        double rowTotal = 0;
                        double overallTotal = 0;
                        for (int t = 0; t < chklstbranch.Items.Count; t++)
                        {
                            rowTotal = 0;
                            if (chklstbranch.Items[t].Selected == true)
                            {
                                FpSpread1.Sheets[0].Rows.Count++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(t + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(chklstbranch.Items[t].Text);
                                double countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='0'")), out countval);
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                if (GenderCountDic.ContainsKey("0"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["0"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["0"] = total;
                                }
                                else
                                    GenderCountDic.Add("0", countval);
                                countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='1'")), out countval);
                                }
                                if (GenderCountDic.ContainsKey("1"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["1"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["1"] = total;
                                }
                                else
                                    GenderCountDic.Add("1", countval);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='2'")), out countval);
                                }
                                if (GenderCountDic.ContainsKey("2"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["2"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["2"] = total;
                                }
                                else
                                    GenderCountDic.Add("2", countval);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(rowTotal);
                                overallTotal += rowTotal;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            }
                        }
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Grand Total";
                        string GenderCount = string.Empty;
                        if (GenderCountDic.ContainsKey("0"))
                            GenderCount = Convert.ToString(GenderCountDic["0"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = GenderCount;
                        if (GenderCountDic.ContainsKey("1"))
                            GenderCount = Convert.ToString(GenderCountDic["1"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = GenderCount;
                        if (GenderCountDic.ContainsKey("2"))
                            GenderCount = Convert.ToString(GenderCountDic["2"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = GenderCount;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(overallTotal);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                    }
                    #endregion
                    final.Visible = true;
                }
            }
            else if (ddlrpttype.SelectedIndex == 3)
            {
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
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
                FpSpread1.Sheets[0].ColumnCount = 18;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                //FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Full Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Course I/II/III Year & Class";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Regular / Part-Time";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Duration of Course";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Nationality";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Gender";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Passport No. & Validity";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Visa No. & Validity";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Local Address";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Permanent Address";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Phone / Mobile & Landline";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 15].Text = "E-mail ID";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Specific Cultural interest / Talent";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Final Year Students only - Do you wish to continue higher studies next year in this institutions";
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                //FpSpread1.Sheets[0].Columns[0].Width = 40;
                //FpSpread1.Sheets[0].Columns[1].Width = 180;
                //FpSpread1.Sheets[0].Columns[2].Width = 150;
                //FpSpread1.Sheets[0].Columns[3].Width = 200;
                //FpSpread1.Sheets[0].Columns[4].Width = 85;
                //FpSpread1.Sheets[0].Columns[5].Width = 150;
                //FpSpread1.Sheets[0].Columns[6].Width = 150;
                // FpSpread1.Width = 1000;
                string sql = "SELECT distinct Countryp,parent_statec,parent_addressc,Streetc,Cityc,parent_pincodec,r.Current_Semester,R.Stud_Name,Roll_No,Reg_No,Course_Name+'-'+Dept_name course,'Regular' Regular,(Select Duration FROM Degree K WHERE K.Degree_Code = G.Degree_Code) Duration,(Select TextVal FROM TextValTable N WHERE N.TextCode = A.citizen ) Nationality,CASE WHEN sex = 0 THEN 'Male' WHEN sex = 1 THEN 'Female' Else 'TransGender' END Gender,ISNULL(PassportNo,'') PassportNo,IsNUll(convert(nvarchar(50),PassToDate,103),'') ExpDate,ISNULL(VisaNo,'') VisaNo,ISNULL(convert(nvarchar(50),VisaToDate,103),'') VisaExpDate,parent_addressP,Streetp,Cityp,CASE When ISNUMERIC(districtp) = 1 then (select textval from textvaltable where TextCode = a.Districtp) else Districtp end district,(select textval from textvaltable k where k.TextCode = a.parent_statep) state,parent_pincodep,parent_phnop ,parentF_Mobile,StuPer_Id,(select textval from textvaltable l where l.TextCode = a.co_curricular) cultural,r.roll_no,r.Roll_Admit,r.degree_code,1 dummy,a.sex FROM Registration R,Applyn A,Degree G,Course C,Department D where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and d.Dept_Code = g.Dept_Code and g.college_code = d.college_code  and r.Batch_Year in ('" + batchyear + "') and r.degree_code in ('" + degreecode + "') and c.type='" + type + "'  and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'  and citizen not in (SELECT distinct CONVERT(varchar(100),TextCode)  FROM TextValTable K WHERE CONVERT(varchar(100),k.TextCode)  = citizen      and isnull(k.TextVal,'') like '%india%' )  and citizen is not null and citizen <>'-1' and(Select TextVal FROM TextValTable N WHERE N.TextCode = A.citizen and (isnull(n.TextVal,'-1')<>'-1' or n.TextVal<>'' )) <>'' order by r.roll_no ,  R.Stud_Name , Duration ";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "Text");
                int height = 0;
                //FpSpread1.Sheets[0].RowCount = 0;
                //FpSpread1.Sheets[0].ColumnCount = 7;
                //FpSpread1.SaveChanges();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count);
                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                        FpSpread1.Sheets[0].Columns[2].Visible = false;
                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                        if (Convert.ToString(Session["Rollflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[1].Visible = true;
                        if (Convert.ToString(Session["Regflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[2].Visible = true;
                        if (Convert.ToString(Session["Admissionflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[ii]["roll_no"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[ii]["Reg_No"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[ii]["Roll_Admit"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[ii]["Stud_Name"].ToString();
                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[ii]["Reg_No"].ToString();
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[ii]["Roll_No"].ToString();
                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[ii]["Stud_Name"].ToString();
                        int sem = Convert.ToInt32(ds.Tables[0].Rows[ii]["Current_Semester"].ToString());
                        string year = "";
                        if (sem >= 1 && sem <= 2)
                        {
                            year = "I";
                        }
                        else if (sem >= 3 && sem <= 4)
                        {
                            year = "II";
                        }
                        else if (sem >= 5 && sem <= 6)
                        {
                            year = "III";
                        }
                        else if (sem >= 7 && sem <= 8)
                        {
                            year = "IV";
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = year + "  " + ds.Tables[0].Rows[ii]["Course"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[ii]["Regular"].ToString();
                        sem = Convert.ToInt32(ds.Tables[0].Rows[ii]["Duration"].ToString());
                        if (sem == 6)
                        {
                            year = "3 Years";
                        }
                        else if (sem == 4)
                        {
                            year = " 2 Years";
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = year;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[ii]["Nationality"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = ds.Tables[0].Rows[ii]["Gender"].ToString();
                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[ii]["Gender"].ToString();
                        string comm = "";
                        string f1 = ds.Tables[0].Rows[ii]["PassportNo"].ToString();
                        string f2 = ds.Tables[0].Rows[ii]["ExpDate"].ToString();
                        if (f1.Trim() != "" && f1 != null && f1.Length > 1)
                        {
                            comm = f1;
                        }
                        if (f2.Trim() != "" && f2 != null && f2.Length > 1)
                        {
                            if (comm.Trim() != "" && comm != null)
                            {
                                comm = comm + " / " + f2;
                            }
                            else
                            {
                                comm = f2;
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = comm;
                        comm = "";
                        f1 = ds.Tables[0].Rows[ii]["VisaNo"].ToString();
                        f2 = ds.Tables[0].Rows[ii]["VisaExpDate"].ToString();
                        if (f1.Trim() != "" && f1 != null && f1.Length > 1)
                        {
                            comm = f1;
                        }
                        if (f2.Trim() != "" && f2 != null && f2.Length > 1)
                        {
                            if (comm.Trim() != "" && comm != null)
                            {
                                comm = comm + " / " + f2;
                            }
                            else
                            {
                                comm = f2;
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = comm;
                        string addresss = ds.Tables[0].Rows[ii]["parent_addressc"].ToString() + " " + ds.Tables[0].Rows[ii]["Streetc"].ToString() + " " + ds.Tables[0].Rows[ii]["Cityc"].ToString() + " " + ds.Tables[0].Rows[ii]["parent_pincodec"].ToString() + "  " + ds.Tables[0].Rows[ii]["parent_statec"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = addresss;
                        addresss = ds.Tables[0].Rows[ii]["parent_addressP"].ToString() + " " + ds.Tables[0].Rows[ii]["Streetp"].ToString() + " " + ds.Tables[0].Rows[ii]["Cityp"].ToString() + " " + ds.Tables[0].Rows[ii]["parent_pincodep"].ToString() + " " + ds.Tables[0].Rows[ii]["District"].ToString() + " " + ds.Tables[0].Rows[ii]["State"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13].Text = addresss;
                        // addresss = ds.Tables[0].Rows[ii]["parent_addressP"].ToString() + " " + ds.Tables[0].Rows[ii]["Streetp"].ToString() + " " + ds.Tables[0].Rows[ii]["Cityp"].ToString() + " " + ds.Tables[0].Rows[ii]["parent_pincodep"].ToString() + " " + ds.Tables[0].Rows[ii]["District"].ToString() + " " + ds.Tables[0].Rows[ii]["State"].ToString();
                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = addresss;
                        comm = "";
                        f1 = ds.Tables[0].Rows[ii]["parent_phnop"].ToString();
                        f2 = ds.Tables[0].Rows[ii]["parentF_Mobile"].ToString();
                        if (f1.Trim() != "" && f1 != null && f1.Length > 1)
                        {
                            comm = f1;
                        }
                        if (f2.Trim() != "" && f2 != null && f2.Length > 1)
                        {
                            if (comm.Trim() != "" && comm != null)
                            {
                                comm = comm + " / " + f2;
                            }
                            else
                            {
                                comm = f2;
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14].CellType = txtceltype;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14].Text = comm;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 15].Text = ds.Tables[0].Rows[ii]["StuPer_Id"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 16].Text = ds.Tables[0].Rows[ii]["cultural"].ToString();
                        //height = height + FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height;
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    //FpSpread1.Height = height + 100;
                    FpSpread1.Visible = true;
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
                        FpSpread1.Sheets[0].Columns[i].Locked = true;
                        FpSpread1.Sheets[0].Columns[i].VerticalAlign = VerticalAlign.Middle;
                    }
                    #region Include Total
                    if (cb_includetotal.Checked)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "S.No";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Branch Name";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Male";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Female";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "Transgender";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.White;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                        double rowTotal = 0;
                        double overallTotal = 0;
                        for (int t = 0; t < chklstbranch.Items.Count; t++)
                        {
                            rowTotal = 0;
                            if (chklstbranch.Items[t].Selected == true)
                            {
                                FpSpread1.Sheets[0].Rows.Count++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(t + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(chklstbranch.Items[t].Text);
                                double countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='0'")), out countval);
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                if (GenderCountDic.ContainsKey("0"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["0"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["0"] = total;
                                }
                                else
                                    GenderCountDic.Add("0", countval);
                                countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='1'")), out countval);
                                }
                                if (GenderCountDic.ContainsKey("1"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["1"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["1"] = total;
                                }
                                else
                                    GenderCountDic.Add("1", countval);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='2'")), out countval);
                                }
                                if (GenderCountDic.ContainsKey("2"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["2"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["2"] = total;
                                }
                                else
                                    GenderCountDic.Add("2", countval);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(rowTotal);
                                overallTotal += rowTotal;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            }
                        }
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Grand Total";
                        string GenderCount = string.Empty;
                        if (GenderCountDic.ContainsKey("0"))
                            GenderCount = Convert.ToString(GenderCountDic["0"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = GenderCount;
                        if (GenderCountDic.ContainsKey("1"))
                            GenderCount = Convert.ToString(GenderCountDic["1"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = GenderCount;
                        if (GenderCountDic.ContainsKey("2"))
                            GenderCount = Convert.ToString(GenderCountDic["2"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = GenderCount;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(overallTotal);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                    }
                    #endregion
                    final.Visible = true;
                }
            }
            if (ddlrpttype.SelectedIndex == 1)
            {
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
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
                FpSpread1.Sheets[0].ColumnCount = 10;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                //FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name of the Candidate";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Name of the Course Applied";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Examination Passed";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Univertity";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "State whether provisional eligibility certificate obtained or fees for recognition paid";
                // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Community";
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;

                string sql = " select distinct r.Stud_Name,Course_Name+'-'+Dept_Name Course,(select TextVal from textvaltable where textcode=convert(varchar(100),isnull(p.course_code,0)))Passed,(select TextVal from textvaltable where textcode=convert(varchar(100),isnull(p.university_code,0)))University,R.roll_no,R.Reg_No,R.Roll_Admit,r.degree_code,a.sex,1 dummy, case when a.sex=0 then 'Male' when a.sex ='1' then 'Female' when a.sex=2 then 'TransGender' end Gender from Registration R inner join applyn a on r.App_No = a.app_no inner join Degree g on r.degree_code = g.Degree_Code inner join course c on g.Course_Id = c.Course_Id and g.college_code = c.college_code inner join Department d on g.Dept_Code = d.Dept_Code and g.college_code = d.college_code left join Stud_prev_details p on p.app_no = r.App_No where isnull(p.course_code,0)<>0 and isnull(p.university_code,0)<>0 and r.batch_year in ('" + batchyear + "')  and r.degree_code in ('" + degreecode + "') and c.type='" + type + "'  and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "Text");
                int height = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count);
                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                        FpSpread1.Sheets[0].Columns[2].Visible = false;
                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                        if (Convert.ToString(Session["Rollflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[1].Visible = true;
                        if (Convert.ToString(Session["Regflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[2].Visible = true;
                        if (Convert.ToString(Session["Admissionflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[ii]["roll_no"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[ii]["Reg_No"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[ii]["Roll_Admit"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[ii]["Stud_Name"].ToString();
                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[ii]["Stud_Name"].ToString();
                        //int sem = Convert.ToInt32(ds.Tables[0].Rows[ii]["Current_Semester"].ToString());
                        //string year = "";
                        //if (sem >= 1 && sem <= 2)
                        //{
                        //    year = "I";
                        //}
                        //else if (sem >= 3 && sem <= 4)
                        //{
                        //    year = "II";
                        //}
                        //else if (sem >= 5 && sem <= 6)
                        //{
                        //    year = "III";
                        //}
                        //else if (sem >= 7 && sem <= 8)
                        //{
                        //    year = "IV";
                        //}
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[ii]["Gender"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[ii]["Course"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[ii]["Passed"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[ii]["University"].ToString();
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[ii]["RankNum"].ToString();
                        //string comm = ds.Tables[0].Rows[ii]["community"].ToString() + " / " + ds.Tables[0].Rows[ii]["religion"].ToString();
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = comm;
                        //height = height + FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height;
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    FpSpread1.Height = height + 100;
                    FpSpread1.Visible = true;
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
                        FpSpread1.Sheets[0].Columns[i].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Columns[i].Locked = true;
                    }
                    #region Include Total
                    if (cb_includetotal.Checked)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "S.No";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Branch Name";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Male";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "Female";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "Transgender";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.White;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = Color.White;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                        double rowTotal = 0;
                        double overallTotal = 0;
                        for (int t = 0; t < chklstbranch.Items.Count; t++)
                        {
                            rowTotal = 0;
                            if (chklstbranch.Items[t].Selected == true)
                            {
                                FpSpread1.Sheets[0].Rows.Count++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(t + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(chklstbranch.Items[t].Text);
                                double countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='0'")), out countval);
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                if (GenderCountDic.ContainsKey("0"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["0"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["0"] = total;
                                }
                                else
                                    GenderCountDic.Add("0", countval);
                                countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='1'")), out countval);
                                }
                                if (GenderCountDic.ContainsKey("1"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["1"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["1"] = total;
                                }
                                else
                                    GenderCountDic.Add("1", countval);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                countval = 0;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(dummy)", "  degree_code='" + Convert.ToString(chklstbranch.Items[t].Value) + "' and sex='2'")), out countval);
                                }
                                if (GenderCountDic.ContainsKey("2"))
                                {
                                    double value = 0;
                                    double.TryParse(Convert.ToString(GenderCountDic["2"]), out value);
                                    double total = value + countval;
                                    GenderCountDic["2"] = total;
                                }
                                else
                                    GenderCountDic.Add("2", countval);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Convert.ToString(countval) == "0" ? " - " : Convert.ToString(countval));
                                rowTotal += countval;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(rowTotal);
                                overallTotal += rowTotal;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            }
                        }
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Grand Total";
                        string GenderCount = string.Empty;
                        if (GenderCountDic.ContainsKey("0"))
                            GenderCount = Convert.ToString(GenderCountDic["0"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = GenderCount;
                        if (GenderCountDic.ContainsKey("1"))
                            GenderCount = Convert.ToString(GenderCountDic["1"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = GenderCount;
                        if (GenderCountDic.ContainsKey("2"))
                            GenderCount = Convert.ToString(GenderCountDic["2"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = GenderCount;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(overallTotal);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                    }
                    #endregion
                    final.Visible = true;
                }
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
        }
        catch
        {
        }
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
            string stream = "";
            if (ddltype.SelectedItem.Text.ToLower().Trim() == "day")
            {
                stream = "AIDED STREAM";
            }
            if (ddltype.SelectedItem.Text.ToLower().Trim() == "evening")
            {
                stream = "SELF FINANCED STREAM";
            }
            degreedetails = "" + ddlrpttype.SelectedItem.Text.ToString().ToUpper() + "      " + stream + "";
            string pagename = "stud_detailedrpt.aspx";
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
            #region magesh 27/1/18
            string strsql = "select distinct  batch_year from Registration where batch_year<>'-1' and batch_year<>''  and delflag=0 and exam_flag<>'debar'  order by batch_year desc";
            //string strsql = "select distinct top 3 batch_year from Registration where batch_year<>'-1' and batch_year<>''  and delflag=0 and exam_flag<>'debar'  order by batch_year desc";
            #endregion
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
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            lblerrormsg.Visible = false;
            count = 0;
            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            // ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (singleuser == "True")
            {
                ds2.Dispose();
                ds2.Reset();
                string strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id  and course.college_code = degree.college_code   and degree.college_code=" + Session["collegecode"].ToString() + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "  and course.type='" + ddltype.SelectedItem.Text.ToString() + "' ";
                ds2 = da.select_method_wo_parameter(strquery, "Text");
            }
            else
            {
                ds2.Dispose();
                ds2.Reset();
                string strquery1 = "select distinct degree.course_id,course.course_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code  and degree.college_code=" + Session["collegecode"].ToString() + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + " and course.type='" + ddltype.SelectedItem.Text.ToString() + "' ";
                ds2 = da.select_method_wo_parameter(strquery1, "Text");
            }
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
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
                        if (checkSchoolSetting() == 0)
                        {
                            txtdegree.Text = "School Type(" + (chklstdegree.Items.Count) + ")";
                        }
                        else
                        {
                            txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                        }
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
                // BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;
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
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
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
                //ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (singleuser == "True")
                {
                    ds2.Dispose();
                    ds2.Reset();
                    string strquery = "select distinct degree.degree_code,course.Course_Name+'-'+ department.dept_name as dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
                    ds2 = d2.select_method_wo_parameter(strquery, "text");
                }
                else
                {
                    ds2.Dispose();
                    ds2.Reset();
                    string strquery1 = "select distinct degree.degree_code,course.Course_Name+'-'+ department.dept_name as dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + " order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc";
                    ds2 = d2.select_method_wo_parameter(strquery1, "text");
                }
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds2;
                    chklstbranch.DataTextField = "dept_name";
                    chklstbranch.DataValueField = "degree_code";
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
    public void hide()
    {
        Printcontrol.Visible = false;
        FpSpread1.Visible = false;
        final.Visible = false;
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
                if (checkSchoolSetting() == 0)
                {
                    txtdegree.Text = "School Type(" + (chklstdegree.Items.Count) + ")";
                }
                else
                {
                    txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                }
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
                if (checkSchoolSetting() == 0)
                {
                    txtdegree.Text = "School Type(" + commcount.ToString() + ")";
                }
                else
                {
                    txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                }
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
            BindDegree(singleuser, group_user, collegecode, usercode);
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
            //if (ddlrpttype.SelectedIndex==2)
            //{
            //lblstatus.Visible = true;
            //ddlstatus.Visible = true;
            //}
            //else
            //{
            //    lblstatus.Visible = false;
            //    ddlstatus.Visible = false;
            //}
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
    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
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
            lblerrormsg.Text = "";
            collegecode = ddlcollege.SelectedValue.ToString();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
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



    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
}