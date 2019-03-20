using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using FarPoint.Web.Spread.Design;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.Net;
using System.Net.Mail;
using System.Configuration;

public partial class StudentMod_PQ : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataSet ddummy = new DataSet();
    DataTable data = new DataTable();
    string user_code = "";
    string college_code = "";
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
            user_code = Session["usercode"].ToString();
            college_code = Session["collegecode"].ToString();
            panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px; position: absolute; top: -9px; width: 101%; display:none;");
            if (!IsPostBack)
            {
                rdbug.Checked = true;
            }
        }
        catch (Exception ex) { }
    }

    protected void lnk_logout(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch
        {

        }
    }

    protected void Btn_click(object sender, EventArgs e)
    {
        try
        {
            Dictionary<int, double> dicsubcol = new Dictionary<int, double>();
            string textboxvalue = Convert.ToString(txt_appno.Text);
            if (textboxvalue.Trim() != "")
            {
                string[] splitvalue = textboxvalue.Split(',');
                if (splitvalue.Length > 0)
                {
                    FpSpread3.SaveChanges();
                    FpSpread3.Sheets[0].ColumnCount = 10;
                    FpSpread3.Sheets[0].RowHeader.Visible = false;
                    FpSpread3.Sheets[0].AutoPostBack = false;
                    FpSpread3.Height = 450;
                    FpSpread3.Width = 930;
                    FpSpread3.CommandBar.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = Color.Brown;
                    darkstyle.ForeColor = Color.White;
                    FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    FarPoint.Web.Spread.CheckBoxCellType chkcel1 = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkcel1.AutoPostBack = false;
                    FarPoint.Web.Spread.ButtonCellType but = new FarPoint.Web.Spread.ButtonCellType("MyCommand", FarPoint.Web.Spread.ButtonType.ImageButton, "images/view11.png");
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    // FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].BackColor = Color.MistyRose;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[0].Locked = true;


                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    // FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].BackColor = Color.MistyRose;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "app";
                    //  FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].BackColor = Color.MistyRose;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "View";
                    // FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].BackColor = Color.MistyRose;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Application No";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Sex";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Percentage";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Community";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;

                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Mobile Number";
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;

                    FpSpread3.Sheets[0].Columns[2].Visible = false;
                    FpSpread3.Sheets[0].Columns[0].Visible = true;
                    FpSpread3.Sheets[0].Columns[0].Width = 40;
                    FpSpread3.Sheets[0].Columns[1].Width = 50;
                    FpSpread3.Sheets[0].RowCount = 0;
                    FpSpread3.Sheets[0].Columns[0].Locked = true;

                    FpSpread3.Sheets[0].Columns[4].Locked = true;
                    FpSpread3.Sheets[0].Columns[5].Locked = true;
                    FpSpread3.Sheets[0].Columns[6].Locked = true;
                    FpSpread3.Sheets[0].Columns[7].Locked = true;
                    FpSpread3.Sheets[0].Columns[8].Locked = true;
                    FpSpread3.Sheets[0].Columns[9].Locked = true;

                    int sno = 0;
                    for (int val = 0; val <= splitvalue.GetUpperBound(0); val++)
                    {
                        string com = "select a.degree_code ,batch_year,a.college_code,Dept_Name  from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.college_code =a.college_code  and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and app_formno='" + Convert.ToString(splitvalue[val]).Trim() + "'  and isconfirm ='1'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(com, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string degree_code = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]);
                            string college_code = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                            string batch_year = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                            string dept_name = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);
                            string com1 = "";

                            com = "select a.app_no,app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport ,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP ,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,a.degree_code,batch_year,a.college_code,SubCaste,isdisable ,isdisabledisc,islearningdis ,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet,Institute_name,instaddress,medium,isgrade,Part1Language,Part2Language,percentage ,university_code,uni_state,psubjectno,registerno,acual_marks,max_marks,pass_month,pass_year,noofattempt,ph.grade,sd.course_code,sd.branch_code ,sd.tancet_mark,Vocational_stream  from applyn a,Stud_prev_details sd,perv_marks_history ph,selectcriteria s  where  a.app_no =sd.app_no and s.app_no =a.app_no and s.degree_code =a.degree_code and isapprove <>4  and sd.course_entno =ph.course_entno and a.batch_year ='" + batch_year + "'  and a.college_code ='" + college_code + "' and a.degree_code ='" + degree_code + "' and current_semester =1 and isconfirm ='1' and app_formno='" + Convert.ToString(splitvalue[val]).Trim() + "'";

                            if (rdbug.Checked == true)
                            {
                                com = com + "  and psubjectno not in(sd.Part1Language,sd.Part2Language)";
                            }

                            ds = d2.select_method_wo_parameter(com, "text");
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                com = "select a.app_no,app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport ,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP ,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,a.degree_code,batch_year,a.college_code,SubCaste,isdisable ,isdisabledisc,islearningdis ,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet,Institute_name,instaddress,medium,isgrade,Part1Language,Part2Language,percentage ,university_code,uni_state,psubjectno,registerno,acual_marks,max_marks,pass_month,pass_year,noofattempt,p.grade,s.course_code,s.branch_code ,s.tancet_mark,Vocational_stream from applyn a,Course c,Degree d,Stud_prev_details s,perv_marks_history p,textvaltable t where c.Course_Id=d.Course_Id and c.college_code=d.college_code and c.college_code=a.college_code and d.Degree_Code=a.degree_code and d.college_code=a.college_code  and s.app_no=a.app_no and p.course_entno=s.course_entno and t.TextCode=p.psubjectno and t.college_code=a.college_code  and t.college_code=c.college_code and t.college_code=d.college_code and (selection_status='0' or selection_status is null)and (admission_status='0' or admission_status is null ) and  a.batch_year ='" + batch_year + "'  and a.college_code ='" + college_code + "' and a.degree_code ='" + degree_code + "' and current_semester =1 and isconfirm ='1' and app_formno='" + Convert.ToString(splitvalue[val]).Trim() + "'";
                                if (rdbug.Checked == true)
                                {
                                    com = com + "  and psubjectno not in(s.Part1Language,s.Part2Language)";
                                }
                                ds = d2.select_method_wo_parameter(com, "text");
                            }
                            DataTable firstTable = ds.Tables[0];
                            DataSet ddummy = new DataSet();
                            DataView view = new DataView(firstTable);
                            DataTable distinctValues = new DataTable();
                            distinctValues = view.ToTable(true, "app_no");
                            ddummy.Clear();
                            ddummy.Tables.Add(distinctValues.Copy());

                            DataSet gradeset = new DataSet();
                            string gradequery = "select Frange,Trange,Mark_Grade  from Grade_Master where College_Code =" + college_code + " and batch_year =" + batch_year + "";
                            gradeset = d2.select_method_wo_parameter(gradequery, "Text");
                            DataTable dnew;
                            // ddummy = dt.select_method_wo_parameter(com1, "Text");

                            DataView dvcheck = new DataView();
                            DataTable data = new DataTable();
                            DataView dummyview;
                            dicsubcol.Clear();
                            if (ddummy.Tables[0].Rows.Count > 0)
                            {
                                for (int du = 0; du < ddummy.Tables[0].Rows.Count; du++)
                                {
                                    int total = 0;
                                    int maxtotal = 0;
                                    string app_no = Convert.ToString(ddummy.Tables[0].Rows[du]["app_no"]);
                                    ds.Tables[0].DefaultView.RowFilter = "app_no='" + app_no + "'";
                                    dvcheck = ds.Tables[0].DefaultView;
                                    if (dvcheck.Count > 0)
                                    {
                                        dnew = dvcheck.ToTable();
                                        if (Convert.ToString(dvcheck[0]["isgrade"]) == "False")
                                        {
                                            //  total = Convert.ToInt32(dvcheck[cn]["acual_marks"].ToString());
                                            total = Convert.ToInt32(dnew.Compute("Sum(acual_marks)", ""));
                                            maxtotal = Convert.ToInt32(dnew.Compute("Sum(max_marks)", ""));
                                        }
                                        else
                                        {
                                            if (gradeset.Tables[0].Rows.Count > 0)
                                            {
                                                DataView gradview = new DataView();
                                                if (dnew.Rows.Count > 0)
                                                {
                                                    for (int jk = 0; jk < dnew.Rows.Count; jk++)
                                                    {
                                                        string grade = Convert.ToString(dnew.Rows[jk]["grade"]);
                                                        string max = Convert.ToString(dnew.Rows[jk]["max_marks"]);
                                                        gradeset.Tables[0].DefaultView.RowFilter = "Mark_Grade='" + grade + "'";
                                                        gradview = gradeset.Tables[0].DefaultView;
                                                        if (gradview.Count > 0)
                                                        {
                                                            string fromrange = Convert.ToString(gradview[0]["Frange"]);
                                                            string torange = Convert.ToString(gradview[0]["Trange"]);
                                                            if (fromrange.Trim() != "" && torange.Trim() != "")
                                                            {
                                                                double midpoint = (Convert.ToDouble(fromrange) + Convert.ToDouble(torange)) / Convert.ToDouble(2);
                                                                total = total + Convert.ToInt32(midpoint);
                                                                maxtotal = maxtotal + Convert.ToInt32(max);
                                                            }
                                                            else
                                                            {
                                                                total = total + 0;
                                                                maxtotal = maxtotal + 0;
                                                            }

                                                        }
                                                        else
                                                        {
                                                            total = total + 0;
                                                            maxtotal = maxtotal + 0;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                total = 0;
                                                maxtotal = 0;
                                            }
                                        }
                                        double mark = Convert.ToDouble(total) / Convert.ToDouble(maxtotal) * 100;
                                        if (mark != 0 && Convert.ToString(mark) != "NaN")
                                        {
                                            dicsubcol.Add(Convert.ToInt32(app_no), Convert.ToDouble(Math.Round(mark, 2)));
                                        }
                                    }
                                }
                                dicsubcol = dicsubcol.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                            }

                            if (dicsubcol.Count > 0)
                            {

                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    sno++;
                                    FpSpread3.Sheets[0].RowCount++;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(ds.Tables[0].Rows[0]["seattype"]);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = chkcel1;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Value = 0;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[0]["app_no"]);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[0]["app_formno"]);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].CellType = but;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[0]["app_formno"]);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                                    string gender = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
                                    if (gender.Trim() == "0")
                                    {
                                        gender = "Male";
                                    }
                                    else if (gender.Trim() == "1")
                                    {
                                        gender = "Female";
                                    }
                                    else if (gender.Trim() == "2")
                                    {
                                        gender = "Transgender";
                                    }

                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(gender);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dicsubcol[Convert.ToInt32(ds.Tables[0].Rows[0]["app_no"])]);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

                                    string community = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["community"]));
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(community);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;

                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;

                                }
                                FpSpread3.Visible = true;
                                cbselect.Visible = true;
                                button.Visible = true;
                                print.Visible = true;
                                lblerror.Visible = false;
                            }
                            else
                            {
                                FpSpread3.Visible = false;
                                lblerror.Visible = true;
                                lblerror.Text = "No Records Found";
                                cbselect.Visible = false;
                                button.Visible = false;
                                print.Visible = false;
                            }
                            distinctValues.Dispose();
                        }
                    }

                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Enter the Value";
            }

        }
        catch
        {
        }

    }

    public string subjectcode(string textcri)
    {
        string subjec_no = "";
        try
        {
            DataSet ds23 = new DataSet();
            string select_subno = "select TextVal from textvaltable where TextCode ='" + textcri + "' and college_code ='" + Session["collegecode"].ToString() + "' ";
            ds23.Clear();
            ds23 = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds23.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds23.Tables[0].Rows[0]["TextVal"]);
            }

        }
        catch
        {

        }
        return subjec_no;
    }

    public void sendmail(string mail, string name, string app)
    {
        try
        {
            string send_mail = "";
            string send_pw = "";
            string to_mail = Convert.ToString(mail);
            // string bodytext = "Hi Boy";
            string subtext = "MCC Admission-Regarding";
            string strstuname = Convert.ToString(name);

            string strquery = "select massemail,masspwd from collinfo where college_code = " + Session["collegecode"] + " ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                send_mail = Convert.ToString(ds.Tables[0].Rows[0]["massemail"]);
                send_pw = Convert.ToString(ds.Tables[0].Rows[0]["masspwd"]);
            }
            if (send_mail.Trim() != "" && send_pw.Trim() != "" && to_mail.Trim() != "")
            {
                SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mailmsg = new MailMessage();
                MailAddress mfrom = new MailAddress(send_mail);
                mailmsg.From = mfrom;
                mailmsg.To.Add(to_mail);
                mailmsg.Subject = subtext;
                mailmsg.IsBodyHtml = true;
                // mailmsg.Body = "Hi";
                mailmsg.Body = mailmsg.Body + strstuname;
                mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = app + ".pdf";
                    string attachementpath = szPath + szFile;
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/Report/" + szFile + "")))
                    {
                        Attachment data = new Attachment(attachementpath);
                        mailmsg.Attachments.Add(data);
                    }
                }
                Mail.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                Mail.UseDefaultCredentials = false;
                Mail.Credentials = credentials;
                Mail.Send(mailmsg);

            }

        }
        catch
        {

        }
    }
    public void sendsms(string number, string app)
    {
        try
        {
            int ik = 1;
            DateTime dt_date = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
            while (ik <= 2)
            {
                dt_date = dt_date.AddDays(1);
                if (dt_date.ToString("dddd") == "Sunday")
                {
                    dt_date = dt_date.AddDays(1);
                }
                ik++;
            }
            //string Msg = "MCC ID (Application Number) :  " + app + "; You are provisionally admitted. Last date for fee payment " + dt_date.ToString("dd/MM/yyyy") + " Refer email for Admission Card.";
            string Msg = "MCC ID (Application Number) :  " + app + "; You are provisionally admitted. Last date for payment of fees is " + dt_date.ToString("dd/MM/yyyy") + " Admission Card is sent to your E-mail.";
            string Mobile_no = Convert.ToString(number);
            string user_id = "";
            string SenderID = "";
            string Password = "";
            string todaydate = System.DateTime.Now.ToString("dd/MM/yyyy");
            string[] splitdate = todaydate.Split('/');
            DateTime dt1 = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            string ssr = "select * from Track_Value where college_code='" + Session["collegecode"] + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(ssr, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
            }

            if (user_id.Trim() != "")
            {
                string getval = d2.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {
                    SenderID = spret[0].ToString();
                    Password = spret[0].ToString();
                }
                string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + Mobile_no + "&text=" + Msg + "&priority=ndnd&stype=normal";
                string isst = "0";

                smsreport(strpath, isst, dt1, Mobile_no, Msg);
            }

        }
        catch
        {

        }
    }

    public void smsreport(string uril, string isstaff, DateTime dt1, string phone, string msg)
    {
        try
        {
            string phoneno = phone;
            string message = msg;
            string date = dt1.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = "";
            groupmsgid = strvel;
            int sms = 0;
            string smsreportinsert = "";

            smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date)values( '" + phoneno + "','" + groupmsgid + "','" + message + "','" + Session["collegecode"].ToString() + "','" + isstaff + "','" + date + "')";
            sms = d2.update_method_wo_parameter(smsreportinsert, "Text");

        }
        catch (Exception ex)
        {

        }

    }


    protected void FpSpread3_command(object sender, EventArgs e)
    {
        try
        {

            string activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
            if (activecol == "3")
            {
                DataSet dsnew1 = new DataSet();
                string app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                Session["pdfapp_no"] = Convert.ToString(app_no);
                dsnew1.Clear();
                string jg = "select type,Dept_Name,Edu_Level ,Course_Name  from applyn a,Degree d, Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and a.app_no ='" + app_no + "'";
                dsnew1 = d2.select_method_wo_parameter(jg, "text");
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");

                string type = "";
                string edulevel = "";
                string grduation = "";
                string course = "";

                if (dsnew1.Tables[0].Rows.Count > 0)
                {
                    type = Convert.ToString(dsnew1.Tables[0].Rows[0]["type"]);
                    edulevel = Convert.ToString(dsnew1.Tables[0].Rows[0]["Edu_Level"]);
                    grduation = Convert.ToString(dsnew1.Tables[0].Rows[0]["Course_Name"]);
                    course = Convert.ToString(dsnew1.Tables[0].Rows[0]["Dept_Name"]);
                }

                if (edulevel.ToString().ToUpper() == "PG")
                {
                    pgdiv_verification.Visible = true;
                    ugdiv_verification.Visible = false;
                }
                else if (edulevel.ToString().ToUpper() == "UG")
                {
                    pgdiv_verification.Visible = false;
                    ugdiv_verification.Visible = true;
                }

                string query = "select app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,degree_code,batch_year,college_code,SubCaste,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet from applyn a where a.app_no='" + app_no + "'";
                query = query + " select course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark from Stud_prev_details where app_no ='" + app_no + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    college_span.InnerHtml = ":  " + Convert.ToString(type);
                    degree_Span.InnerHtml = ":  " + Convert.ToString(edulevel);
                    graduation_span.InnerHtml = ":  " + Convert.ToString(grduation);
                    course_span.InnerHtml = ":  " + Convert.ToString(course);

                    applicantname_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]);

                    dob_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["dob"]);
                    if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "0")
                    {
                        gender_span.InnerHtml = ":  Male";
                    }
                    else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "1")
                    {
                        gender_span.InnerHtml = ":  Female";
                    }
                    else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "2")
                    {
                        gender_span.InnerHtml = ":  Transgender";
                    }
                    parent_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]);

                    string occupation = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["parent_occu"]));
                    occupation_span.InnerHtml = ":  " + occupation.ToString();

                    string mothertonge = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["mother_tongue"]));
                    mothertongue_span.InnerHtml = ":  " + Convert.ToString(mothertonge);


                    string relisgion = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["religion"]));
                    religion_span.InnerHtml = ":  " + Convert.ToString(relisgion);

                    string city = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["citizen"]));
                    nationality_span.InnerHtml = ":  " + Convert.ToString(city);

                    string coummnity = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["community"]));
                    commuity_span.InnerHtml = ":  " + Convert.ToString(coummnity);

                    if (Convert.ToString(ds.Tables[0].Rows[0]["caste"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["caste"]) != "0")
                    {
                        string scas = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["caste"]));
                        Caste_span.InnerHtml = ":  " + Convert.ToString(scas);
                    }
                    else
                    {
                        Caste_span.InnerHtml = ":  -";
                    }

                    if (Convert.ToString(ds.Tables[0].Rows[0]["TamilOrginFromAndaman"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["TamilOrginFromAndaman"]) != "False")
                    {
                        tamilorigin_span.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        tamilorigin_span.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]) != "False")
                    {
                        Ex_service_span.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        Ex_service_span.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["isdisable"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["isdisable"]) != "False")
                    {
                        Differentlyable_Span.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        Differentlyable_Span.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["first_graduate"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["first_graduate"]) != "False")
                    {
                        first_generation_Span.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        first_generation_Span.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["CampusReq"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["CampusReq"]) != "False")
                    {
                        residancerequired_span.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        residancerequired_span.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["DistinctSport"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["DistinctSport"]) != "0")
                    {
                        string disy = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["DistinctSport"]));
                        sport_span.InnerHtml = ":  " + Convert.ToString(disy);
                    }
                    else
                    {
                        sport_span.InnerHtml = ":  -";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["co_curricular"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["co_curricular"]) != "0")
                    {
                        string cocour = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["co_curricular"]));
                        Co_Curricular_span.InnerHtml = ":  " + Convert.ToString(cocour);
                    }
                    else
                    {
                        Co_Curricular_span.InnerHtml = ":  -";
                    }

                    if (Convert.ToString(ds.Tables[0].Rows[0]["ncccadet"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["ncccadet"]) != "False")
                    {
                        ncccadetspan.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        ncccadetspan.InnerHtml = ":  No";
                    }

                    if (Convert.ToString(ds.Tables[1].Rows[0]["Vocational_stream"]).Trim() != "" && Convert.ToString(ds.Tables[1].Rows[0]["Vocational_stream"]) != "False")
                    {
                        Vocationalspan.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        Vocationalspan.InnerHtml = ":  No";
                    }


                    caddressline1_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_addressC"]);
                    string address = Convert.ToString(ds.Tables[0].Rows[0]["Streetc"]);
                    if (ds.Tables[0].Rows[0]["Streetc"].ToString().Trim() != "")
                    {

                        string[] split = address.Split('/');
                        if (split.Length > 1)
                        {
                            if (Convert.ToString(split[0]).Trim() != "")
                            {
                                Addressline2_span.InnerHtml = ":  " + Convert.ToString(split[0]);
                            }
                            else
                            {
                                Addressline2_span.InnerHtml = ":  -";
                            }
                            if (Convert.ToString(split[1]).Trim() != "")
                            {
                                Addressline3_span.InnerHtml = ":  " + Convert.ToString(split[1]);
                            }
                            else
                            {
                                Addressline3_span.InnerHtml = ":  -";
                            }
                        }
                        else
                        {
                            Addressline2_span.InnerHtml = ":  " + Convert.ToString(split[0]);
                        }

                    }
                    else
                    {
                        Addressline2_span.InnerHtml = ":  -";
                        Addressline3_span.InnerHtml = ":  -";
                    }

                    if (ds.Tables[0].Rows[0]["Cityc"].ToString().Trim() != "")
                    {
                        city_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Cityc"]);
                    }
                    else
                    {
                        city_span.InnerHtml = "-";
                    }

                    if (ds.Tables[0].Rows[0]["parent_statec"].ToString().Trim() != "")
                    {
                        string state = subjectcode(ds.Tables[0].Rows[0]["parent_statec"].ToString());
                        state_span.InnerHtml = ":  " + Convert.ToString(state);
                    }
                    else
                    {
                        state_span.InnerHtml = ":  -";
                    }

                    if (ds.Tables[0].Rows[0]["Countryc"].ToString().Trim() != "")
                    {
                        string country = subjectcode(ds.Tables[0].Rows[0]["Countryc"].ToString());
                        Country_span.InnerHtml = ":  " + Convert.ToString(country);
                    }
                    else
                    {
                        Country_span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["parent_pincodec"].ToString().Trim() != "")
                    {
                        Postelcode_Span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodec"]);
                    }
                    else
                    {
                        Postelcode_Span.InnerHtml = "-";
                    }

                    if (ds.Tables[0].Rows[0]["Student_Mobile"].ToString().Trim() != "")
                    {
                        Mobilenumber_Span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]);
                    }
                    else
                    {
                        Mobilenumber_Span.InnerHtml = "-";
                    }

                    if (ds.Tables[0].Rows[0]["alter_mobileno"].ToString().Trim() != "")
                    {
                        Alternatephone_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["alter_mobileno"]);
                    }
                    else
                    {
                        Alternatephone_span.InnerHtml = "-";
                    }

                    if (ds.Tables[0].Rows[0]["StuPer_Id"].ToString().Trim() != "")
                    {
                        emailid_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]);
                    }
                    else
                    {
                        emailid_span.InnerHtml = "-";
                    }

                    if (ds.Tables[0].Rows[0]["parent_phnoc"].ToString().Trim() != "")
                    {
                        std_ist_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_phnoc"]);
                    }
                    else
                    {
                        std_ist_span.InnerHtml = "-";
                    }

                    // permnant

                    paddressline1_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]);
                    if (ds.Tables[0].Rows[0]["Streetp"].ToString().Trim() != "")
                    {
                        string streat = Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]);
                        if (streat.Trim() != "")
                        {
                            string[] splitstreat = streat.Split('/');
                            if (splitstreat.Length > 1)
                            {
                                if (Convert.ToString(splitstreat[0]).Trim() != "")
                                {
                                    paddressline2_span.InnerHtml = ":  " + Convert.ToString(splitstreat[0]);
                                }
                                else
                                {
                                    paddressline2_span.InnerHtml = ":  -";
                                }
                                if (Convert.ToString(splitstreat[0]).Trim() != "")
                                {
                                    paddressline3_span.InnerHtml = ":  " + Convert.ToString(splitstreat[1]);
                                }
                                else
                                {
                                    paddressline3_span.InnerHtml = ":  -";
                                }
                            }
                            else
                            {
                                paddressline2_span.InnerHtml = ":  " + Convert.ToString(splitstreat[0]);
                            }
                        }
                        paddressline2_span.InnerHtml = ":  -";
                        paddressline3_span.InnerHtml = ":  -";
                    }
                    else
                    {
                        paddressline2_span.InnerHtml = ":  -";
                        paddressline3_span.InnerHtml = ":  -";
                    }


                    if (ds.Tables[0].Rows[0]["Cityp"].ToString().Trim() != "")
                    {
                        pcity_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Cityp"]);
                    }
                    else
                    {
                        pcity_span.InnerHtml = "-";
                    }

                    if (ds.Tables[0].Rows[0]["parent_statep"].ToString().Trim() != "")
                    {
                        string state = subjectcode(ds.Tables[0].Rows[0]["parent_statep"].ToString());
                        pstate_span.InnerHtml = ":  " + Convert.ToString(state);
                    }
                    else
                    {
                        pstate_span.InnerHtml = ":  -";
                    }

                    if (ds.Tables[0].Rows[0]["Countryp"].ToString().Trim() != "")
                    {
                        string country = subjectcode(ds.Tables[0].Rows[0]["Countryp"].ToString());
                        pcountry_span.InnerHtml = ":  " + Convert.ToString(country);
                    }
                    else
                    {
                        pcountry_span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["parent_pincodep"].ToString().Trim() != "")
                    {
                        ppostelcode_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]);
                    }
                    else
                    {
                        ppostelcode_span.InnerHtml = "-";
                    }


                    if (ds.Tables[0].Rows[0]["parent_phnop"].ToString().Trim() != "")
                    {
                        pstdisd_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]);
                    }
                    else
                    {
                        pstdisd_span.InnerHtml = "-";
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (edulevel == "UG")
                    {
                        ugtotaldiv.Visible = true;
                        pgtotaldiv.Visible = false;
                        string courseentronumber = Convert.ToString(ds.Tables[1].Rows[0]["course_entno"]);
                        string coursecode = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);
                        string university_code = Convert.ToString(ds.Tables[1].Rows[0]["university_code"]);
                        string institutename = Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]);
                        string percentage = Convert.ToString(ds.Tables[1].Rows[0]["percentage"]);
                        string institueaddress = Convert.ToString(ds.Tables[1].Rows[0]["instaddress"]);
                        string medium = Convert.ToString(ds.Tables[1].Rows[0]["medium"]);
                        string part1language = Convert.ToString(ds.Tables[1].Rows[0]["Part1Language"]);
                        string part2language = Convert.ToString(ds.Tables[1].Rows[0]["Part2Language"]);
                        string isgrade = Convert.ToString(ds.Tables[1].Rows[0]["isgrade"]);
                        string university_state = Convert.ToString(ds.Tables[1].Rows[0]["uni_state"]);
                        // string part1language = Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]);

                        if (coursecode.Trim() != "")
                        {
                            string course1 = subjectcode(coursecode);
                            qualifyingexam_span.InnerHtml = ":  " + Convert.ToString(course1);
                        }
                        else
                        {
                            qualifyingexam_span.InnerHtml = ":  -";
                        }


                        if (institutename.Trim() != "")
                        {
                            Nameofschool_span.InnerHtml = ":  " + Convert.ToString(institutename);
                        }
                        else
                        {
                            Nameofschool_span.InnerHtml = "";
                        }
                        if (institueaddress.Trim() != "")
                        {
                            locationofschool_Span.InnerHtml = ":  " + Convert.ToString(institueaddress);
                        }
                        else
                        {
                            locationofschool_Span.InnerHtml = "";
                        }
                        if (medium.Trim() != "")
                        {
                            string m = subjectcode(medium);
                            mediumofstudy_span.InnerHtml = ":  " + Convert.ToString(m);
                        }
                        else
                        {
                            mediumofstudy_span.InnerHtml = ":  -";
                        }
                        if (university_code.Trim() != "")
                        {
                            string univ = subjectcode(university_code);
                            qualifyingboard_span.InnerHtml = ":  " + Convert.ToString(univ);

                        }
                        else
                        {
                            qualifyingboard_span.InnerHtml = ":  -";
                        }
                        if (isgrade.Trim() != "")
                        {
                            if (isgrade == "True")
                            {
                                marksgrade_span.InnerHtml = ":  Grade";
                            }
                            else
                            {
                                marksgrade_span.InnerHtml = ":  Marks";
                            }
                        }

                        string markquery = "select psubjectno,registerno,acual_marks,max_marks,noofattempt,pass_month,pass_year,semyear ,grade from perv_marks_history  where course_entno ='" + courseentronumber + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(markquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable data = new DataTable();
                            DataRow dr = null;
                            Hashtable hash = new Hashtable();
                            data.Columns.Add("Language", typeof(string));
                            data.Columns.Add("Subject", typeof(string));
                            data.Columns.Add("Marks Obtained", typeof(string));
                            data.Columns.Add("Month", typeof(string));
                            data.Columns.Add("Year", typeof(string));
                            data.Columns.Add("Register No / Roll No", typeof(string));
                            data.Columns.Add("No of Attempts", typeof(string));
                            data.Columns.Add("Maximum Marks", typeof(string));

                            hash.Add(0, "Language1");
                            hash.Add(1, "Language2");
                            hash.Add(2, " Subject1");
                            hash.Add(3, " Subject2");
                            hash.Add(4, " Subject3");
                            hash.Add(5, " Subject4");
                            hash.Add(6, " Subject5");
                            hash.Add(7, " Subject6");
                            hash.Add(8, " Subject7");
                            hash.Add(9, " Subject8");
                            hash.Add(10, " Subject9");
                            hash.Add(11, " Subject10");
                            hash.Add(12, " Subject11");

                            int totalmark = 0;
                            int maxtotal = 0;

                            for (int mark = 0; mark < ds.Tables[0].Rows.Count; mark++)
                            {
                                string subjectno = Convert.ToString(ds.Tables[0].Rows[mark]["psubjectno"]);
                                string actualmark = "";
                                if (isgrade == "True")
                                {
                                    actualmark = Convert.ToString(ds.Tables[0].Rows[mark]["grade"]);
                                }
                                else
                                {
                                    actualmark = Convert.ToString(ds.Tables[0].Rows[mark]["acual_marks"]);
                                }
                                string month = Convert.ToString(ds.Tables[0].Rows[mark]["pass_month"]);
                                string year = Convert.ToString(ds.Tables[0].Rows[mark]["pass_year"]);
                                string regno = Convert.ToString(ds.Tables[0].Rows[mark]["registerno"]);
                                string noofattenm = Convert.ToString(ds.Tables[0].Rows[mark]["noofattempt"]);
                                string maxmark = Convert.ToString(ds.Tables[0].Rows[mark]["max_marks"]);
                                dr = data.NewRow();
                                string lang = Convert.ToString(hash[mark]);
                                dr[0] = Convert.ToString(lang);
                                string sub = subjectcode(subjectno);
                                dr[1] = Convert.ToString(sub);
                                dr[2] = Convert.ToString(actualmark);
                                dr[3] = Convert.ToString(month);
                                dr[4] = Convert.ToString(year);
                                dr[5] = Convert.ToString(regno);
                                dr[6] = Convert.ToString(noofattenm);
                                dr[7] = Convert.ToString(maxmark);
                                data.Rows.Add(dr);
                                if (isgrade != "True")
                                {
                                    totalmark = totalmark + Convert.ToInt32(actualmark);
                                    maxtotal = maxtotal + Convert.ToInt32(maxmark);
                                }
                            }
                            if (isgrade != "True")
                            {
                                total_marks_secured.InnerHtml = ":  " + Convert.ToString(totalmark);
                                maximum_marks.InnerHtml = ":  " + Convert.ToString(maxtotal);
                                percentage_span.InnerHtml = ":  " + percentage;
                            }

                            VerificationGridug.DataSource = data;
                            VerificationGridug.DataBind();
                            if (VerificationGridug.Rows.Count > 0)
                            {
                                for (int v = 0; v < VerificationGridug.Rows.Count; v++)
                                {
                                    VerificationGridug.Rows[v].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                                    VerificationGridug.Rows[v].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                                    VerificationGridug.Rows[v].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                    VerificationGridug.Rows[v].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                    VerificationGridug.Rows[v].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                                    VerificationGridug.Rows[v].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                    }
                    else if (edulevel == "PG")
                    {
                        ugtotaldiv.Visible = false;
                        pgtotaldiv.Visible = true;
                        string courseentronumber = Convert.ToString(ds.Tables[1].Rows[0]["course_entno"]);
                        string coursecode = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);
                        string university_code = Convert.ToString(ds.Tables[1].Rows[0]["university_code"]);
                        string institutename = Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]);
                        string percentage = Convert.ToString(ds.Tables[1].Rows[0]["percentage"]);
                        string institueaddress = Convert.ToString(ds.Tables[1].Rows[0]["instaddress"]);
                        string medium = Convert.ToString(ds.Tables[1].Rows[0]["medium"]);
                        string part1language = Convert.ToString(ds.Tables[1].Rows[0]["Part1Language"]);
                        string part2language = Convert.ToString(ds.Tables[1].Rows[0]["Part2Language"]);
                        string isgrade = Convert.ToString(ds.Tables[1].Rows[0]["isgrade"]);
                        string university_state = Convert.ToString(ds.Tables[1].Rows[0]["uni_state"]);
                        string typeofsubject = Convert.ToString(ds.Tables[1].Rows[0]["type_major"]);
                        string typeofsemester = Convert.ToString(ds.Tables[1].Rows[0]["type_semester"]);
                        string regno = Convert.ToString(ds.Tables[1].Rows[0]["registration_no"]);
                        string major = Convert.ToString(ds.Tables[1].Rows[0]["branch_code"]);
                        string majorpercentage = Convert.ToString(ds.Tables[1].Rows[0]["major_percent"]);
                        string majorallidepercentage = Convert.ToString(ds.Tables[1].Rows[0]["majorallied_percent"]);

                        percentagemajorspan.InnerHtml = ":  " + Convert.ToString(percentage);
                        majorsubjectspan.InnerHtml = ":  " + Convert.ToString(majorpercentage);
                        alliedmajorspan.InnerHtml = ":  " + Convert.ToString(majorallidepercentage);

                        if (coursecode.Trim() != "")
                        {
                            string course1 = subjectcode(coursecode);
                            ugqualifyingexam_span.InnerHtml = ":  " + Convert.ToString(course1);
                        }
                        else
                        {
                            ugqualifyingexam_span.InnerHtml = ":  -";
                        }
                        if (institutename.Trim() != "")
                        {
                            nameofcollege_Sapn.InnerHtml = ":  " + Convert.ToString(institutename);
                        }
                        else
                        {
                            nameofcollege_Sapn.InnerHtml = "";
                        }
                        if (institueaddress.Trim() != "")
                        {
                            locationofcollege_sapn.InnerHtml = ":  " + Convert.ToString(institueaddress);
                        }
                        else
                        {
                            locationofcollege_sapn.InnerHtml = "";
                        }
                        if (major.Trim() != "")
                        {
                            string major1 = subjectcode(major);
                            major_span.InnerHtml = ":  " + Convert.ToString(major1);
                        }
                        else
                        {
                            major_span.InnerHtml = "";
                        }
                        if (typeofsubject.Trim() != "")
                        {
                            if (typeofsubject == "1")
                            {
                                typeofsubject = "Single";
                            }
                            else if (typeofsubject == "2")
                            {
                                typeofsubject = "Double";
                            }
                            else if (typeofsubject == "3")
                            {
                                typeofsubject = "Triple";
                            }
                            typeofmajor_span.InnerHtml = ":  " + Convert.ToString(typeofsubject);
                        }
                        if (typeofsemester.Trim() != "")
                        {
                            if (typeofsemester == "True")
                            {
                                typeofsemester = "Semester";
                            }
                            else
                            {
                                typeofsemester = "Non Semester";
                            }
                            typeofsemester_span.InnerHtml = ":  " + Convert.ToString(typeofsemester);
                        }
                        if (medium.Trim() != "")
                        {
                            string lang = subjectcode(medium);
                            mediumofstudy_spanug.InnerHtml = ":  " + Convert.ToString(lang);
                        }

                        if (isgrade.Trim() != "")
                        {
                            if (isgrade == "True")
                            {
                                marksorgradeug_span.InnerHtml = ":  Grade";
                            }
                            else
                            {
                                marksorgradeug_span.InnerHtml = ":  Marks";
                            }
                        }

                        //if (isgrade.Trim() != "")
                        //{
                        //    marksorgradeug_span.InnerHtml = ":  " + Convert.ToString(isgrade);
                        //}
                        if (regno.Trim() != "")
                        {
                            reg_no_span.InnerHtml = ":  " + Convert.ToString(regno);
                        }

                        string pgquery = "select psubjectno,subject_typeno,acual_marks,max_marks,pass_month,pass_year,semyear ,grade from perv_marks_history where course_entno ='" + courseentronumber + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(pgquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable data = new DataTable();
                            DataRow dr = null;
                            Hashtable hash = new Hashtable();
                            data.Columns.Add("S.No", typeof(string));
                            //  data.Columns.Add("Sem/Year", typeof(string));
                            data.Columns.Add("Subject", typeof(string));
                            data.Columns.Add("Subject type", typeof(string));
                            data.Columns.Add("Marks", typeof(string));
                            data.Columns.Add("Month", typeof(string));
                            data.Columns.Add("Year", typeof(string));
                            data.Columns.Add("Maximum Marks", typeof(string));
                            int sno = 0;
                            for (int pg = 0; pg < ds.Tables[0].Rows.Count; pg++)
                            {
                                sno++;
                                string subjectno = Convert.ToString(ds.Tables[0].Rows[pg]["psubjectno"]);
                                string subjecttypeno = Convert.ToString(ds.Tables[0].Rows[pg]["subject_typeno"]);
                                string actualmark = "";
                                if (isgrade == "True")
                                {
                                    actualmark = Convert.ToString(ds.Tables[0].Rows[pg]["grade"]);
                                }
                                else
                                {
                                    actualmark = Convert.ToString(ds.Tables[0].Rows[pg]["acual_marks"]);
                                }

                                string month = Convert.ToString(ds.Tables[0].Rows[pg]["pass_month"]);
                                string year = Convert.ToString(ds.Tables[0].Rows[pg]["pass_year"]);
                                // string noofattenm = Convert.ToString(ds.Tables[0].Rows[pg]["noofattempt"]);
                                string maxmark = Convert.ToString(ds.Tables[0].Rows[pg]["max_marks"]);
                                dr = data.NewRow();
                                dr[0] = Convert.ToString(sno);
                                string subject = subjectcode(subjectno);
                                dr[1] = Convert.ToString(subject);
                                string typesub = subjectcode(subjecttypeno);
                                dr[2] = Convert.ToString(typesub);
                                dr[3] = Convert.ToString(actualmark);
                                dr[4] = Convert.ToString(month);
                                dr[5] = Convert.ToString(year);
                                dr[6] = Convert.ToString(maxmark);
                                data.Rows.Add(dr);
                            }
                            Verificationgridpg.DataSource = data;
                            Verificationgridpg.DataBind();
                            if (VerificationGridug.Rows.Count > 0)
                            {
                                for (int v = 0; v < Verificationgridpg.Rows.Count; v++)
                                {
                                    Verificationgridpg.Rows[v].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                                    Verificationgridpg.Rows[v].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                    Verificationgridpg.Rows[v].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                    Verificationgridpg.Rows[v].Cells[6].HorizontalAlign = HorizontalAlign.Center;

                                }
                            }
                        }

                    }

                }

            }
            if (activecol == "1")
            {
                int isval1 = Convert.ToInt32(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Value);

                if (isval1 == 1)
                {
                    FpSpread3.Sheets[0].Rows[Convert.ToInt32(activerow)].BackColor = Color.LightYellow;
                }
                else if (isval1 == 0)
                {
                    int vll = Convert.ToInt32(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text);
                    if (vll % 2 == 0)
                    {
                        FpSpread3.Sheets[0].Rows[Convert.ToInt32(activerow)].BackColor = Color.Lavender;
                    }
                    else
                    {
                        FpSpread3.Sheets[0].Rows[Convert.ToInt32(activerow)].BackColor = Color.MintCream;
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void cbselect_Change(object sender, EventArgs e)
    {
        try
        {
            if (FpSpread3.Sheets[0].Rows.Count > 0)
            {
                if (cbselect.Checked == true)
                {
                    for (int kl = 0; kl < FpSpread3.Sheets[0].Rows.Count; kl++)
                    {
                        FpSpread3.Sheets[0].Cells[kl, 1].Value = 1;
                    }
                }
                if (cbselect.Checked == false)
                {
                    for (int kl = 0; kl < FpSpread3.Sheets[0].Rows.Count; kl++)
                    {
                        FpSpread3.Sheets[0].Cells[kl, 1].Value = 0;
                    }
                }
            }
            button.Focus();
            FpSpread3.SaveChanges();
        }
        catch
        {

        }
    }

    protected void generate_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {

        }

    }

    public void loadprint()
    {
        try
        {
            Gios.Pdf.PdfDocument mydoc;
            Gios.Pdf.PdfDocument mydocnew = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Font Fontbold = new Font("Book Antiqua", 18, FontStyle.Regular);
            Font fbold = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 11, FontStyle.Regular);
            Font fontname = new Font("Book Antiqua", 11, FontStyle.Bold);
            Font fontmedium = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font fontmediumb = new Font("Book Antiqua", 13, FontStyle.Bold);
            Boolean saveflag = false;
            //string sign = "principal" + ddlcollege.SelectedValue.ToString() + "";
            DataSet d_value = new DataSet();
            string strquery = "select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            string Collegename = "";
            string aff = "";
            string address = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                aff = "(Affiliated to the " + ds.Tables[0].Rows[0]["university"].ToString() + ")";
                address = ds.Tables[0].Rows[0]["address1"].ToString() + " " + ds.Tables[0].Rows[0]["district"].ToString() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
            }

            for (int i = 0; i < FpSpread3.Sheets[0].Rows.Count; i++)
            {
                int isval = 0;
                isval = Convert.ToInt32(FpSpread3.Sheets[0].Cells[i, 1].Value);
                if (isval == 1)
                {
                    try
                    {
                        mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                        saveflag = true;
                        string deprt = "";
                        string course = "";
                        string rollno = FpSpread3.Sheets[0].Cells[i, 2].Tag.ToString();
                        string app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(i), 2].Text);
                        string name = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 4].Text);
                        string jg = "select type,Dept_Name,Edu_Level ,Course_Name,a.degree_code,c.course_id  from applyn a,Degree d, Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and a.app_no ='" + app_no + "'";
                        ds = d2.select_method_wo_parameter(jg, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            deprt = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);
                            course = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);
                        }

                        Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                        Gios.Pdf.PdfPage mypdfpage1 = mydocnew.NewPage();
                        int ik = 1;
                        DateTime dt_date = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                        string updatequery = "update applyn set admitcard_date ='" + dt_date.ToString("MM/dd/yyyy") + "' where app_formno ='" + rollno + "'";
                        int d = d2.update_method_wo_parameter(updatequery, "Text");
                        while (ik <= 2)
                        {
                            dt_date = dt_date.AddDays(1);
                            if (dt_date.ToString("dddd") == "Sunday")
                            {
                                dt_date = dt_date.AddDays(1);
                            }
                            ik++;
                        }

                        string sign = "principal" + Convert.ToString(Session["collegecode"]) + "";

                        string mail_id = "";
                        string stud_phoneno = "";
                        string mailidquery = "select StuPer_Id,Student_Mobile  from applyn where app_formno ='" + rollno + "'";
                        d_value.Clear();
                        d_value = d2.select_method_wo_parameter(mailidquery, "Text");
                        if (d_value.Tables[0].Rows.Count > 0)
                        {
                            mail_id = Convert.ToString(d_value.Tables[0].Rows[0]["StuPer_Id"]);
                            stud_phoneno = Convert.ToString(d_value.Tables[0].Rows[0]["Student_Mobile"]);
                        }
                        //string upadte = "update applyn set enroll_date='" + dten + "',feedate='" + dtfee + "',Is_Enroll='1' where app_formno='" + rollno + "'";
                        //int a = d2.update_method_wo_parameter(upadte, "Text");

                        int xvlaue = 40;

                        PdfArea tete = new PdfArea(mydoc, 10, 10, 570, 820);

                        PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                        mypdfpage.Add(pr1);
                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                      new PdfArea(mydoc, 150, 20, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, Collegename + " (Autonomous)");
                        mypdfpage.Add(ptc);

                        PdfTextArea ptc01 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                      new PdfArea(mydoc, 190, 80, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, address);
                        mypdfpage.Add(ptc01);
                        PdfTextArea ptc02 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, 180, 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, aff);
                        mypdfpage.Add(ptc02);

                        PdfTextArea ptc0265 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 100, 120, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Admission Card");
                        mypdfpage.Add(ptc0265);

                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 25, 25, 300);
                        }

                        PdfTextArea ptc07 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, xvlaue, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "MCC ID");
                        mypdfpage.Add(ptc07);

                        PdfTextArea ptc07ap = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 100, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + rollno.ToString() + "");
                        mypdfpage.Add(ptc07ap);

                        //string[] spdeg = lbldegree.Text.ToString().Split('-');

                        PdfTextArea ptc071 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, xvlaue, 200, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class");
                        mypdfpage.Add(ptc071);

                        PdfTextArea ptc071a = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 100, 200, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + course.ToString() + "");
                        mypdfpage.Add(ptc071a);
                        PdfTextArea ptc08 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, xvlaue, 180, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name");
                        mypdfpage.Add(ptc08);
                        PdfTextArea ptc08na = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 100, 180, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + name.ToString() + "");
                        mypdfpage.Add(ptc08na);
                        PdfTextArea ptc081 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, xvlaue, 220, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Group");
                        mypdfpage.Add(ptc081);
                        PdfTextArea ptc081a = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 100, 220, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + deprt.ToString() + "");
                        mypdfpage.Add(ptc081a);

                        PdfTextArea ptc09 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, xvlaue, 260, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "You are provisionally admitted to the course specified above.");
                        mypdfpage.Add(ptc09);

                        PdfTextArea ptc1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, xvlaue, 280, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Fees should be paid on or before  " + dt_date.ToString("dd/MM/yyyy") + " by 3 pm.");

                        mypdfpage.Add(ptc1);
                        PdfTextArea ptc2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, xvlaue, 310, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Fees may be paid online to the IOB through the college website.");


                        //PdfTextArea ptc3 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                             new PdfArea(mydoc, xvlaue, 340, 500, 40), System.Drawing.ContentAlignment.MiddleLeft, "Candidates paying fees at the IOB cash counter should collect their fee challan from the Admissions");
                        //PdfTextArea ptc80 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                             new PdfArea(mydocnew, xvlaue, 355, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Office and submit a copy of the fee paid challan in the Bursar Office on or before " + dt_date.ToString("dd/MM/yyyy") + " within 3 pm.");

                        PdfTextArea ptc4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, xvlaue, 340, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Your Admission will be automatically cancelled if fee is not remitted by the date specified above.");


                        PdfTextArea ptc5 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, xvlaue, 380, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Enrolment details will be sent to your mail id after payment of fees.");

                        PdfTextArea ptc6 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, xvlaue, 400, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Note :");
                        PdfTextArea ptc61 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, xvlaue + 50, 420, 450, 40), System.Drawing.ContentAlignment.MiddleLeft, "No change of date for the payment of fees will be granted.");

                        PdfTextArea ptc7 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, xvlaue + 50, 440, 450, 40), System.Drawing.ContentAlignment.MiddleLeft, "Fees once paid will not be refunded.");


                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            ds.Dispose();
                            ds.Reset();
                            ds = d2.select_method_wo_parameter("select principal_sign from collinfo where college_code='" + Session["collegecode"] + "' and principal_sign is not null", "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])ds.Tables[0].Rows[0]["principal_sign"];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                }
                                memoryStream.Dispose();
                                memoryStream.Close();
                            }
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                            mypdfpage.Add(LogoImage, 400, 650, 200);
                        }

                        PdfTextArea ptc82 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 400, 760, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "PRINCIPAL & SECRETARY");

                        mypdfpage.Add(ptc82);
                        mypdfpage.Add(ptc2);
                        //mypdfpage.Add(ptc3);
                        //mypdfpage.Add(ptc80);
                        mypdfpage.Add(ptc4);
                        mypdfpage.Add(ptc5);
                        mypdfpage.Add(ptc6);
                        mypdfpage.Add(ptc61);
                        mypdfpage.Add(ptc7);

                        mypdfpage.SaveToDocument();



                        PdfArea tete1 = new PdfArea(mydocnew, 10, 10, 570, 820);

                        PdfRectangle pr11 = new PdfRectangle(mydocnew, tete, Color.Black);
                        mypdfpage1.Add(pr11);
                        PdfTextArea ptc11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocnew, 150, 20, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, Collegename + " (Autonomous)");
                        mypdfpage1.Add(ptc11);

                        PdfTextArea ptc011 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocnew, 190, 80, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, address);
                        mypdfpage1.Add(ptc011);
                        PdfTextArea ptc021 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocnew, 180, 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, aff);
                        mypdfpage1.Add(ptc021);

                        PdfTextArea ptc02651 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocnew, 100, 120, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Admission Card");
                        mypdfpage1.Add(ptc02651);

                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                        {
                            PdfImage LogoImage = mydocnew.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage1.Add(LogoImage, 25, 25, 300);
                        }

                        PdfTextArea ptc0718 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocnew, xvlaue, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "MCC ID");
                        mypdfpage1.Add(ptc0718);

                        PdfTextArea ptc07ap1 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocnew, 100, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + rollno.ToString() + "");
                        mypdfpage1.Add(ptc07ap1);

                        //string[] spdeg = lbldegree.Text.ToString().Split('-');

                        PdfTextArea ptc0711 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocnew, xvlaue, 200, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class");
                        mypdfpage1.Add(ptc071);

                        PdfTextArea ptc071a1 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocnew, 100, 200, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + course.ToString() + "");
                        mypdfpage1.Add(ptc071a1);
                        PdfTextArea ptc0811 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocnew, xvlaue, 180, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name");
                        mypdfpage1.Add(ptc0811);
                        PdfTextArea ptc08111 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocnew, 100, 180, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + name.ToString() + "");
                        mypdfpage1.Add(ptc08111);
                        PdfTextArea ptc08114 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocnew, xvlaue, 220, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Group");
                        mypdfpage1.Add(ptc08114);
                        PdfTextArea ptc081a5 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocnew, 100, 220, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + deprt.ToString() + "");
                        mypdfpage1.Add(ptc081a5);

                        PdfTextArea ptc0989 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocnew, xvlaue, 260, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "You are provisionally admitted to the course specified above.");
                        mypdfpage1.Add(ptc0989);

                        PdfTextArea ptc185 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocnew, xvlaue, 280, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Fees should be paid on or before  " + dt_date.ToString("dd/MM/yyyy") + " by 3 pm.");

                        mypdfpage1.Add(ptc185);
                        PdfTextArea ptc28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocnew, xvlaue, 310, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Fees may be paid online to the IOB through the college website.");


                        //PdfTextArea ptc38 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                             new PdfArea(mydocnew, xvlaue, 340, 500, 40), System.Drawing.ContentAlignment.MiddleLeft, "Candidates paying fees at the IOB cash counter should collect their fee challan from the Admissions");
                        //PdfTextArea ptc801 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                            new PdfArea(mydocnew, xvlaue, 355, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Office and submit a copy of the fee paid challan in the Bursar Office on or before " + dt_date.ToString("dd/MM/yyyy") + " within 3 pm.");

                        PdfTextArea ptc48 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocnew, xvlaue, 340, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Your Admission will be automatically cancelled if fee is not remitted by the date specified above.");


                        PdfTextArea ptc58 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocnew, xvlaue, 380, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Enrolment details will be sent to your mail id after payment of fees.");

                        PdfTextArea ptc68 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocnew, xvlaue, 400, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Note :");
                        PdfTextArea ptc618 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocnew, xvlaue + 50, 420, 450, 40), System.Drawing.ContentAlignment.MiddleLeft, "No change of date for the payment of fees will be granted.");

                        PdfTextArea ptc78 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocnew, xvlaue + 50, 440, 450, 40), System.Drawing.ContentAlignment.MiddleLeft, "Fees once paid will not be refunded.");


                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            ds.Dispose();
                            ds.Reset();
                            ds = d2.select_method_wo_parameter("select principal_sign from collinfo where college_code='" + Session["collegecode"] + "' and principal_sign is not null", "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])ds.Tables[0].Rows[0]["principal_sign"];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                }
                                memoryStream.Dispose();
                                memoryStream.Close();
                            }
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                        {
                            PdfImage LogoImage = mydocnew.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                            mypdfpage1.Add(LogoImage, 400, 650, 200);
                        }

                        PdfTextArea ptc828 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocnew, 400, 760, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "PRINCIPAL & SECRETARY");

                        mypdfpage1.Add(ptc828);
                        mypdfpage1.Add(ptc28);
                        //mypdfpage1.Add(ptc38);
                        //mypdfpage1.Add(ptc801);
                        mypdfpage1.Add(ptc48);
                        mypdfpage1.Add(ptc58);
                        mypdfpage1.Add(ptc68);
                        mypdfpage1.Add(ptc618);
                        mypdfpage1.Add(ptc78);

                        mypdfpage1.SaveToDocument();
                        string appPath = HttpContext.Current.Server.MapPath("~");
                        if (appPath != "")
                        {
                            Response.Buffer = true;
                            Response.Clear();
                            string szPath = appPath + "/Report/";
                            string szFile = "" + rollno + ".pdf";
                            mydoc.SaveToFile(szPath + szFile);
                            //  mydocnew.SaveToFile(szPath + szFile);
                            //Response.ClearHeaders();
                            //Response.ClearHeaders();
                            //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                            //Response.ContentType = "application/pdf";
                            //Response.WriteFile(szPath + szFile);
                        }
                        string appPath1 = HttpContext.Current.Server.MapPath("~");
                        if (appPath1 != "")
                        {
                            Response.Buffer = true;
                            Response.Clear();
                            string szPath = appPath + "/Report/";
                            string szFile = "";
                            if (cbsports.Checked == false)
                            {
                                szFile = "PQ.pdf";
                            }
                            if (cbsports.Checked == true)
                            {
                                szFile = "Sports.pdf";
                            }
                            // mydoc.SaveToFile(szPath + szFile);
                            mydocnew.SaveToFile(szPath + szFile);
                            Response.ClearHeaders();
                            Response.ClearHeaders();
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                            Response.ContentType = "application/pdf";
                            Response.WriteFile(szPath + szFile);
                        }
                        sendmail(mail_id, name, rollno);
                        sendsms(stud_phoneno, rollno);


                        // FpSpread4.Sheets[0].Cells[i, 3].Locked = true;
                        //Div2.Visible = false; 
                    }
                    catch
                    {

                    }
                }
            }
            FpSpread3.SaveChanges();
            if (saveflag == true)
            {
                //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Admit Card Generate Generate Successfully\");", true);
                //errorspan.InnerHtml = "Admit Card Generate Generate Successfully";
                //poperrjs.Visible = true;
            }

        }
        catch
        {
        }
    }

    public void admit()
    {
        try
        {
            int isval1 = 0;
            int fllg = 0;
            FpSpread3.SaveChanges();

            bool flage = false;
            bool checkflage = false;
            bool testflage = false;
            DataSet dsnew = new DataSet();
            string upd = "update admitcolumnset set allot=0 where allot is null";
            int fa = d2.update_method_wo_parameter(upd, "text");
            if (flage == false)
            {
                for (int i = 0; i < FpSpread3.Sheets[0].Rows.Count; i++)
                {
                    isval1 = Convert.ToInt32(FpSpread3.Sheets[0].Cells[i, 1].Value);
                    string finYearid = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code='" + college_code + "'");
                    if (cbsports.Checked == false)
                    {
                        if (isval1 == 1)
                        {
                            checkflage = true;
                            string type = "";
                            string edu = "";
                            string concat = "";
                            string deg_code = "";
                            string degree_code = "";
                            DataSet dsnew1 = new DataSet();
                            string app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(i), 2].Text);
                            Session["pdfapp_no"] = Convert.ToString(app_no);
                            dsnew1.Clear();
                            string jg = "select type,Dept_Name,Edu_Level ,Course_Name,a.degree_code,c.course_id  from applyn a,Degree d, Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and a.app_no ='" + app_no + "'";
                            dsnew1 = d2.select_method_wo_parameter(jg, "text");
                            if (dsnew1.Tables[0].Rows.Count > 0)
                            {
                                type = Convert.ToString(dsnew1.Tables[0].Rows[0]["type"]);
                                edu = Convert.ToString(dsnew1.Tables[0].Rows[0]["Edu_Level"]);
                                concat = type + "-" + edu;
                                deg_code = Convert.ToString(dsnew1.Tables[0].Rows[0]["course_id"]);
                                degree_code = Convert.ToString(dsnew1.Tables[0].Rows[0]["degree_code"]);
                            }
                            string dept_Code = d2.GetFunction("select Dept_Code  from Degree where Course_Id='" + deg_code + "' and Degree_Code ='" + degree_code + "' and college_code ='" + Session["collegecode"] + "'");

                            string text_circode = "";
                            string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + user_code + "' and college_code ='" + Session["collegecode"] + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(settingquery, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                                if (linkvalue == "0")
                                {
                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '1 Semester' and textval not like '-1%'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(semesterquery, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                                    }
                                }
                                else
                                {
                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '1 Year' and textval not like '-1%'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(semesterquery, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                                    }
                                }
                            }

                            fllg = 1;
                            string typecode = d2.GetFunction("select column_name  from admitcolumnset where setcolumn='" + degree_code + "' and textcriteria ='Management'");
                            string relig = "select * from admitcolumnset where TextCriteria='Management'  and priority!='0'  and  setcolumn='" + degree_code + "' and college_code='" + college_code + "' and column_name='" + typecode + "'";
                            DataSet ds2 = new DataSet();
                            ds2 = d2.select_method_wo_parameter(relig, "text");
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                flage = true;
                                int inc = 0;

                                string alot = Convert.ToString(ds2.Tables[0].Rows[0]["allot"]);
                                string clm = Convert.ToString(ds2.Tables[0].Rows[0]["column_name"]);
                                int seatcount = 0;
                                string seatconfirm = Convert.ToString(ds2.Tables[0].Rows[0]["allot_Confirm"]);
                                if (seatconfirm.Trim() != "" && seatconfirm != null)
                                {
                                    seatcount = Convert.ToInt32(seatconfirm);
                                }
                                else
                                {
                                    seatcount = 0;
                                }
                                //if (Convert.ToInt32(ds2.Tables[0].Rows[0]["priority"].ToString()) > Convert.ToInt32(alot))
                                //{
                                if (alot.Trim() != "")
                                {
                                    inc = Convert.ToInt32(alot);
                                    inc++;
                                }
                                else
                                {
                                    inc = 1;
                                }
                                string rel = "update admitcolumnset set allot='" + inc + "' where setcolumn='" + degree_code + "' and column_name='" + clm + "' and textcriteria='Management' and college_code='" + college_code + "' and column_name='" + typecode + "'";
                                rel = rel + " update applyn set selection_status='1' where app_no ='" + FpSpread3.Sheets[0].Cells[i, 2].Text + "'";
                                int f = d2.update_method_wo_parameter(rel, "text");
                                string approve1 = "if not exists (select * from selectcriteria where app_no ='" + FpSpread3.Sheets[0].Cells[i, 2].Text + "') insert into selectcriteria(app_no,usercode,degree_code,college_code,isapprove,select_date,isview,criteria_Code) values('" + FpSpread3.Sheets[0].Cells[i, 2].Text + "','" + user_code + "','" + degree_code + "','" + college_code + "','4','" + System.DateTime.Now.ToString("yyy/MM/dd") + "','0','" + typecode + "') else update selectcriteria set isapprove ='4',usercode='" + user_code + "',degree_code='" + degree_code + "',college_code='" + college_code + "',select_date='" + System.DateTime.Now.ToString("yyy/MM/dd") + "',criteria_Code='" + typecode + "' where app_no ='" + FpSpread3.Sheets[0].Cells[i, 2].Text + "'";
                                int a1 = d2.update_method_wo_parameter(approve1, "text");
                                bool feededuct = false;
                                bool feededuct1 = false;
                                string insertquery = "";
                                int insert = 0;
                                string batch_year = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Note);
                                string seattype = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note);
                                string today = System.DateTime.Now.ToString("MM/dd/yyyy");

                                //string selectquery = "select Headid,feecode,FeeAmount,Deduct,Duedate,DueExt1,fineamt1,DueExt2,FineAmt2,DueExt3,FineAmt3,Total,modeofpay,dedect_reason  from feedefine where batch = " + batch_year + " and DegreeCode = " + deg_code + " and depcode = " + dept_Code + " and FeeCat = " + text_circode + " and CollCode ='" + Session["collegecode"] + "'";
                                //selectquery = selectquery + "  select (SUM(FeeAmount)- SUM(Deduct)) as fee,SUM(Deduct) det, Headid  from feedefine where batch = " + batch_year + " and DegreeCode = " + deg_code + " and depcode = " + dept_Code + " and FeeCat = " + text_circode + " and CollCode ='" + Session["collegecode"] + "' group by Headid";
                                //selectquery = selectquery + " select s.university_code,s.uni_state,s.course_code from applyn a,Stud_prev_details s,TextValTable t where a.app_no =s.app_no   and a.app_formno  ='" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "' and batch_year =" + batch_year + " and degree_code =" + degree_code + " and current_semester =1";
                                //selectquery = selectquery + " select community,parent_statep  from applyn where app_formno  ='" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "' and batch_year =" + batch_year + " and degree_code =" + degree_code + " and current_semester =1";

                                string selectquery = " select LedgerFK,HeaderFK,PayMode,FeeAmount,deductAmout,DeductReason,TotalAmount,RefundAmount,FeeCategory,FineAmount from FT_FeeAllotDegree where DegreeCode='" + degree_code + "' and BatchYear ='" + batch_year + "' and SeatType ='" + seattype + "' and FeeCategory ='" + text_circode + "' and FinYearFK ='" + finYearid + "' ";
                                selectquery = selectquery + "  select top 1 * from collinfo ";
                                selectquery = selectquery + " select s.university_code,s.uni_state,s.course_code from applyn a,Stud_prev_details s,TextValTable t where a.app_no =s.app_no   and a.app_formno  ='" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "' and batch_year =" + batch_year + " and degree_code =" + degree_code + " and current_semester =1";
                                selectquery = selectquery + " select community,parent_statep  from applyn where app_formno  ='" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "' and batch_year =" + batch_year + " and degree_code =" + degree_code + " and current_semester =1";

                                dsnew.Clear();
                                dsnew = d2.select_method_wo_parameter(selectquery, "Text");
                                if (edu == "UG")
                                {
                                    if (dsnew.Tables[2].Rows.Count > 0)
                                    {
                                        string coursecode = Convert.ToString(dsnew.Tables[2].Rows[0]["course_code"]);
                                        string universitystate = Convert.ToString(dsnew.Tables[2].Rows[0]["uni_state"]);
                                        string coursevalue = subjectcode(Convert.ToString(dsnew.Tables[2].Rows[0]["course_code"]));
                                        string statevlaue = subjectcode(Convert.ToString(dsnew.Tables[2].Rows[0]["uni_state"]));
                                        if (Convert.ToString(coursevalue) == "HSC" && Convert.ToString(statevlaue) == "Tamil Nadu")
                                        {
                                            feededuct = true;
                                        }
                                    }
                                }
                                if (edu == "PG")
                                {
                                    if (dsnew.Tables[2].Rows.Count > 0)
                                    {
                                        string coursevalue = subjectcode(Convert.ToString(dsnew.Tables[2].Rows[0]["university_code"]));
                                        if (Convert.ToString(coursevalue) == "Madras University")
                                        {
                                            feededuct1 = true;
                                        }
                                    }
                                    if (type == "DAY")
                                    {

                                        if (dsnew.Tables[3].Rows.Count > 0)
                                        {
                                            string community = subjectcode(Convert.ToString(dsnew.Tables[3].Rows[0]["community"]));
                                            string state = subjectcode(Convert.ToString(dsnew.Tables[3].Rows[0]["parent_statep"]));
                                            if (Convert.ToString(community) == "SC" || Convert.ToString(community) == "SC(Arunthathiyar)" || Convert.ToString(community) == "ST" && Convert.ToString(state) == "Tamil Nadu")
                                            {
                                                feededuct = true;
                                            }
                                        }
                                    }
                                }
                                if (edu != "PG")
                                {
                                    if (dsnew.Tables[0].Rows.Count > 0)
                                    {
                                        for (int fee = 0; fee < dsnew.Tables[0].Rows.Count; fee++)
                                        {
                                            double total = 0;
                                            double deduct = 0;

                                            if (feededuct == false)
                                            {
                                                string getfeetype = d2.GetFunction("select LedgerName  from FM_LedgerMaster where LedgerPK  ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["ledgerfk"]) + "' and HeaderFK ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["headerfk"]) + "'");
                                                if (Convert.ToString(getfeetype).Trim() == "University Registration Fee")
                                                {
                                                    double feeamount = 0;
                                                    string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                                    if (feeamount1.Trim() != "")
                                                    {
                                                        feeamount = Convert.ToDouble(feeamount1);
                                                        deduct = 0;
                                                        total = feeamount + 200;
                                                    }
                                                    else
                                                    {
                                                        deduct = 0;
                                                        total = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    deduct = Convert.ToDouble(dsnew.Tables[0].Rows[fee]["deductAmout"]);
                                                    total = (Convert.ToDouble(dsnew.Tables[0].Rows[fee]["FeeAmount"]) - Convert.ToDouble(dsnew.Tables[0].Rows[fee]["deductAmout"]));
                                                }
                                            }
                                            else
                                            {
                                                deduct = Convert.ToDouble(dsnew.Tables[0].Rows[fee]["deductAmout"]);
                                                total = Convert.ToDouble(dsnew.Tables[0].Rows[fee]["TotalAmount"]);
                                            }

                                            string headerfk = Convert.ToString(dsnew.Tables[0].Rows[fee]["HeaderFK"]);
                                            string leadgerfk = Convert.ToString(dsnew.Tables[0].Rows[fee]["LedgerFK"]);
                                            string feeAmount = Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]);
                                            string deductrea = Convert.ToString(dsnew.Tables[0].Rows[fee]["DeductReason"]);
                                            string finamount = Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmount"]);
                                            string refund = Convert.ToString(dsnew.Tables[0].Rows[fee]["RefundAmount"]);
                                            string feecatg = Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeCategory"]);
                                            string paymode = Convert.ToString(dsnew.Tables[0].Rows[fee]["PayMode"]);
                                            if (feeAmount.Trim() == "")
                                            {
                                                feeAmount = "0";
                                            }
                                            if (deduct == 0)
                                            {
                                                feeAmount = total.ToString();
                                            }
                                            if (finamount.Trim() == "")
                                            {
                                                finamount = "0";
                                            }

                                            #region for Unwanted Queries
                                            //insertquery = "insert into fee_allot(app_formno,Header_ID,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,roll_admit,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt,college_code)values('" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Headid"]) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "','" + today + "','false','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]) + "','" + text_circode + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Duedate"]) + "',0,0," + deduct + ",'" + total + "','N',0,'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt1"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["fineamt1"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt2"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt2"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt3"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt3"]) + ",'',0,1,'" + batch_year + "',0,0," + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["modeofpay"]) + "','','','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["dedect_reason"]) + "','','','','','','" + Convert.ToString(Session["collegecode"]) + "')";
                                            #endregion

                                            insertquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + app_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeAmount + "',PaidAmount='0' ,DeductAmout='" + deduct + "',DeductReason='" + deductrea + "',FromGovtAmt='0',TotalAmount='" + total + "',RefundAmount='" + refund + "',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='" + paymode + "',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='" + finamount + "',BalAmount='" + total + "' where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "') and App_No in('" + app_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no + ",'" + leadgerfk + "','" + headerfk + "','" + feeAmount + "','" + deduct + "'," + deductrea + ",'0','" + total + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + total + "','" + finYearid + "')";
                                            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                            checkflage = true;
                                            #region for Unwanted Queries
                                            //insertquery = "insert into fee_allot(app_formno,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,roll_admit,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt,college_code)values('" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "','" + today + "','false','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]) + "','" + text_circode + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Duedate"]) + "',0,0," + deduct + ",'" + total + "','N',0,'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt1"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["fineamt1"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt2"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt2"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt3"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt3"]) + ",'',0,1,'" + batch_year + "',0,0," + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["modeofpay"]) + "','','','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["dedect_reason"]) + "','','','','','','" + Convert.ToString(Session["collegecode"]) + "')";
                                            //insert = dt.update_method_wo_parameter(insertquery, "Text");
                                            //}
                                            //else
                                            //{
                                            //insertquery = "insert into FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK)values('" + today + "','1','" + app_no + "','" + leadgerfk + "','" + headerfk + "','" + feeAmount + "','" + deduct + "','" + deductrea + "','0','" + total + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + total + "','" + finYearid + "')";
                                            //insert = d2.update_method_wo_parameter(insertquery, "Text");

                                            //insertquery = "insert into FT_FeeAllot(App_No,HeaderFK,LedgerFK,AllotDate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,roll_admit,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt,college_code)values('" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Headid"]) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "','" + today + "','false','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]) + "','" + text_circode + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Duedate"]) + "',0,0," + Convert.ToString(dsnew.Tables[0].Rows[fee]["Deduct"]) + ",'" + (Convert.ToInt32(dsnew.Tables[0].Rows[fee]["FeeAmount"]) - Convert.ToInt32(dsnew.Tables[0].Rows[fee]["Deduct"])) + "','N',0,'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt1"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["fineamt1"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt2"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt2"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt3"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt3"]) + ",'',0,1,'" + batch_year + "',0,0," + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["modeofpay"]) + "','','','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["dedect_reason"]) + "','','','','','','" + Convert.ToString(Session["collegecode"]) + "')";
                                            //}
                                            //}
                                            #endregion

                                            #region for Unwanted Codings
                                            //if (dsnew.Tables[0].Rows.Count > 0)
                                            //{
                                            //    for (int stat = 0; stat < dsnew.Tables[0].Rows.Count; stat++)
                                            //    {
                                            //        if (feededuct == true)
                                            //        {
                                            //            double total = 0;
                                            //            double deduct = 0;
                                            //            string getfeetype = d2.GetFunction("select HeaderName from FM_HeaderMaster HeaderPK='" + Convert.ToString(dsnew.Tables[0].Rows[stat]["HeaderFK"]) + "'");
                                            //            if (Convert.ToString(getfeetype).Trim() == "NON SALARY" || Convert.ToString(getfeetype).Trim() == "NON SALARY")
                                            //            {
                                            //                double feeamount = 0;
                                            //                string feeamount1 = Convert.ToString(dsnew.Tables[0].Rows[stat]["FeeAmount"]);
                                            //                if (feeamount1.Trim() != "")
                                            //                {
                                            //                    feeamount = Convert.ToDouble(feeamount1);
                                            //                    deduct = 200;
                                            //                    total = feeamount - deduct;
                                            //                }
                                            //                else
                                            //                {
                                            //                    deduct = 0;
                                            //                    total = 0;
                                            //                }
                                            //            }
                                            //            else
                                            //            {
                                            //                total = Convert.ToDouble(Convert.ToString(dsnew.Tables[0].Rows[stat]["FeeAmount"]));
                                            //            }

                                            //            insertquery = "insert into fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno,college_code)values('','" + total + "',0,'" + total + "','false','" + text_circode + "','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "',0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(Session["collegecode"]) + "')";
                                            //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                            //        }
                                            //        else
                                            //        {
                                            //            insertquery = "insert into fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno,college_code)values('','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]) + "',0,'" + Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]) + "','false','" + text_circode + "','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "',0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(Session["collegecode"]) + "')";
                                            //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                            //        }
                                            //    }
                                            //}
                                            #endregion
                                        }
                                    }
                                }
                                #region for PG
                                else
                                {
                                    //if (dsnew.Tables[0].Rows.Count > 0)
                                    //{
                                    //    for (int fee = 0; fee < dsnew.Tables[0].Rows.Count; fee++)
                                    //    {
                                    //        bool tempflage = false;
                                    //        if (feededuct == true || feededuct1 == true)
                                    //        {
                                    //            double total = 0;
                                    //            double deduct = 0;
                                    //            string getfeetype = d2.GetFunction("select LedgerName from FM_LedgerMaster LedgerPK='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["LedgerFK"]) + "' and HeaderFK ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["HeaderFK"]) + "'");
                                    //            if (feededuct == true)
                                    //            {
                                    //                if (Convert.ToString(getfeetype).Trim() == "Tuition Fee")
                                    //                {
                                    //                    tempflage = true;
                                    //                    double feeamount = 0;
                                    //                    string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                    //                    if (feeamount1.Trim() != "")
                                    //                    {
                                    //                        feeamount = Convert.ToDouble(feeamount1);
                                    //                        deduct = feeamount;
                                    //                        total = feeamount - feeamount;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        deduct = 0;
                                    //                        total = 0;
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (feededuct1 == true)
                                    //            {
                                    //                if (Convert.ToString(getfeetype).Trim() == "University Registration Fee")
                                    //                {
                                    //                    tempflage = true;
                                    //                    double feeamount = 0;
                                    //                    string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                    //                    if (feeamount1.Trim() != "")
                                    //                    {
                                    //                        feeamount = Convert.ToDouble(feeamount1);
                                    //                        deduct = 20;
                                    //                        total = feeamount - feeamount;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        deduct = 0;
                                    //                        total = 0;
                                    //                    }
                                    //                }
                                    //                else if (Convert.ToString(getfeetype).Trim() == "Matric And Recognition Fee")
                                    //                {
                                    //                    tempflage = true;
                                    //                    double feeamount = 0;
                                    //                    string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                    //                    if (feeamount1.Trim() != "")
                                    //                    {
                                    //                        feeamount = Convert.ToDouble(feeamount1);
                                    //                        deduct = feeamount;
                                    //                        total = feeamount - feeamount;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        deduct = 0;
                                    //                        total = 0;
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (tempflage == false)
                                    //            {
                                    //                deduct = Convert.ToInt32(dsnew.Tables[0].Rows[fee]["Deduct"]);
                                    //                total = (Convert.ToInt32(dsnew.Tables[0].Rows[fee]["FeeAmount"]) - Convert.ToInt32(dsnew.Tables[0].Rows[fee]["Deduct"]));
                                    //            }
                                    //            insertquery = "insert into fee_allot(app_formno,Header_ID,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,roll_admit,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt,college_code)values('" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Headid"]) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "','" + today + "','false','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]) + "','" + text_circode + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Duedate"]) + "',0,0," + deduct + ",'" + total + "','N',0,'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt1"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["fineamt1"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt2"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt2"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt3"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt3"]) + ",'',0,1,'" + batch_year + "',0,0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["modeofpay"]) + "','','','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["dedect_reason"]) + "','','','','','','" + Convert.ToString(Session["collegecode"]) + "')";
                                    //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                    //        }
                                    //        else
                                    //        {
                                    //            insertquery = "insert into fee_allot(app_formno,Header_ID,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,roll_admit,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt,college_code)values('" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Headid"]) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "','" + today + "','false','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]) + "','" + text_circode + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Duedate"]) + "',0,0," + Convert.ToString(dsnew.Tables[0].Rows[fee]["Deduct"]) + ",'" + (Convert.ToInt32(dsnew.Tables[0].Rows[fee]["FeeAmount"]) - Convert.ToInt32(dsnew.Tables[0].Rows[fee]["Deduct"])) + "','N',0,'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt1"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["fineamt1"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt2"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt2"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt3"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt3"]) + ",'',0,1,'" + batch_year + "',0,0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["modeofpay"]) + "','','','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["dedect_reason"]) + "','','','','','','" + Convert.ToString(Session["collegecode"]) + "')";
                                    //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                    //        }
                                    //        //}
                                    //    }
                                    //}

                                    if (dsnew.Tables[0].Rows.Count > 0)
                                    {
                                        for (int fee = 0; fee < dsnew.Tables[0].Rows.Count; fee++)
                                        {
                                            bool tempflage = false;
                                            double total = 0;
                                            double deduct = 0;
                                            if (feededuct == true || feededuct1 == false)
                                            {

                                                string getfeetype = d2.GetFunction("select LedgerName  from FM_LedgerMaster where LedgerPK  ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["ledgerfk"]) + "' and HeaderFK ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["headerfk"]) + "'");
                                                if (feededuct == true)
                                                {
                                                    if (Convert.ToString(getfeetype).Trim() == "Tuition Fee")
                                                    {
                                                        tempflage = true;
                                                        double feeamount = 0;
                                                        string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                                        if (feeamount1.Trim() != "")
                                                        {
                                                            feeamount = Convert.ToDouble(feeamount1);
                                                            deduct = feeamount;
                                                            total = feeamount - feeamount;
                                                        }
                                                        else
                                                        {
                                                            deduct = 0;
                                                            total = 0;
                                                        }
                                                    }
                                                }
                                                if (feededuct1 == false)
                                                {
                                                    if (Convert.ToString(getfeetype).Trim() == "University Registration Fee")
                                                    {
                                                        tempflage = true;
                                                        double feeamount = 0;
                                                        string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                                        if (feeamount1.Trim() != "")
                                                        {
                                                            feeamount = Convert.ToDouble(feeamount1);
                                                            //deduct = 20;
                                                            total = feeamount + 20;
                                                        }
                                                        else
                                                        {
                                                            //deduct = 0;
                                                            total = 20;
                                                        }
                                                    }
                                                    else if (Convert.ToString(getfeetype).Trim() == "Matric And Recognition Fee")
                                                    {
                                                        tempflage = true;
                                                        double feeamount = 0;
                                                        string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                                        if (feeamount1.Trim() != "")
                                                        {
                                                            feeamount = Convert.ToDouble(feeamount1);
                                                            //deduct = feeamount;
                                                            total = feeamount + 30;//feeamount - feeamount;
                                                        }
                                                        else
                                                        {
                                                            //deduct = 0;
                                                            total = 30;
                                                        }
                                                    }
                                                }
                                                if (tempflage == false)
                                                {
                                                    double.TryParse(dsnew.Tables[0].Rows[fee]["deductAmout"].ToString(), out deduct);
                                                    double feeamt = 0;
                                                    double.TryParse(dsnew.Tables[0].Rows[fee]["FeeAmount"].ToString(), out feeamt);
                                                    total = feeamt - deduct;
                                                }
                                            }
                                            else
                                            {
                                                double.TryParse(Convert.ToString(dsnew.Tables[0].Rows[fee]["deductAmout"]), out deduct);
                                                double.TryParse(Convert.ToString(dsnew.Tables[0].Rows[fee]["TotalAmount"]), out total);
                                            }

                                            string headerfk = Convert.ToString(dsnew.Tables[0].Rows[fee]["HeaderFK"]);
                                            string leadgerfk = Convert.ToString(dsnew.Tables[0].Rows[fee]["LedgerFK"]);
                                            string feeAmount = Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]);
                                            //deduct = Convert.ToString(dsnew.Tables[0].Rows[fee]["deductAmout"]);
                                            string deductrea = Convert.ToString(dsnew.Tables[0].Rows[fee]["DeductReason"]);
                                            //totalamount = Convert.ToString(dsnew.Tables[0].Rows[fee]["TotalAmount"]);
                                            string finamount = Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmount"]);
                                            string refund = Convert.ToString(dsnew.Tables[0].Rows[fee]["RefundAmount"]);
                                            string feecatg = Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeCategory"]);
                                            string paymode = Convert.ToString(dsnew.Tables[0].Rows[fee]["PayMode"]);
                                            if (feeAmount.Trim() == "")
                                            {
                                                feeAmount = "0";
                                            }
                                            if (total > Convert.ToDouble(feeAmount))
                                            {
                                                feeAmount = total.ToString();
                                            }
                                            if (finamount.Trim() == "")
                                            {
                                                finamount = "0";
                                            }
                                            string app_no1 = d2.GetFunction("select app_no from applyn where app_formno='" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "'").Trim();
                                            string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "')  and App_No in('" + app_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeAmount + "',PaidAmount='0' ,DeductAmout='" + deduct + "',DeductReason='" + deductrea + "',FromGovtAmt='0',TotalAmount='" + total + "',RefundAmount='" + refund + "',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='" + paymode + "',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='" + finamount + "',BalAmount='" + total + "' where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "') and App_No in('" + app_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no + ",'" + leadgerfk + "','" + headerfk + "','" + feeAmount + "','" + deduct + "'," + deductrea + ",'0','" + total + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + total + "','" + finYearid + "')";

                                            insert = d2.update_method_wo_parameter(insupdquery, "Text");

                                        }
                                    }

                                    #region for Unwanted Codings
                                    //if (dsnew.Tables[1].Rows.Count > 0)
                                    //{
                                    //    for (int stat = 0; stat < dsnew.Tables[1].Rows.Count; stat++)
                                    //    {
                                    //        if (feededuct == true || feededuct1 == true)
                                    //        {
                                    //            double total = 0;
                                    //            double deduct = 0;
                                    //            bool dummyflage = false;
                                    //            string getfeetype = d2.GetFunction("select header_name from acctheader where  header_id ='" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "'");
                                    //            if (feededuct1 == true)
                                    //            {
                                    //                if (Convert.ToString(getfeetype).Trim() == "NON SALARY" || Convert.ToString(getfeetype).Trim() == "NON SALARY")
                                    //                {
                                    //                    dummyflage = true;
                                    //                    double feeamount = 0;
                                    //                    string feeamount1 = Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]);
                                    //                    if (feeamount1.Trim() != "")
                                    //                    {
                                    //                        feeamount = Convert.ToDouble(feeamount1);
                                    //                        deduct = 50;
                                    //                        total = feeamount - deduct;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        deduct = 0;
                                    //                        total = 0;
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (feededuct == true)
                                    //            {
                                    //                if (Convert.ToString(getfeetype).Trim() == "TUITION FEE")
                                    //                {
                                    //                    dummyflage = true;
                                    //                    double feeamount = 0;
                                    //                    string feeamount1 = Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]);
                                    //                    if (feeamount1.Trim() != "")
                                    //                    {
                                    //                        feeamount = Convert.ToDouble(feeamount1);
                                    //                        deduct = feeamount;
                                    //                        total = feeamount - deduct;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        deduct = 0;
                                    //                        total = 0;
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (dummyflage == false)
                                    //            {
                                    //                total = Convert.ToDouble(Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]));
                                    //            }
                                    //            insertquery = "insert into fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno,college_code)values('','" + total + "',0,'" + total + "','false','" + text_circode + "','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "',0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(Session["collegecode"]) + "')";
                                    //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                    //        }
                                    //        else
                                    //        {
                                    //            insertquery = "insert into fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno,college_code)values('','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]) + "',0,'" + Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]) + "','false','" + text_circode + "','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "',0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(Session["collegecode"]) + "')";
                                    //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                    //        }

                                    //    }
                                    //}
                                    //}
                                    //else
                                    //{
                                    //    string approveaa = "delete from selectcriteria  where app_no='" + FpSpread3.Sheets[0].Cells[i, 2].Text + "' and usercode='" + user_code + "' and degree_code='" + degree_code + "' and college_code='" + college_code + "' and criteria_Code='" + typecode + "'";
                                    //    int asd = d2.update_method_wo_parameter(approveaa, "text");
                                    //    // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Seat Avilable In This Department Please Admit Some Other Department')", true);
                                    //    //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Seat Avilable In This Category Please Admit Some Other Category\");", true);
                                    //    return;
                                    //}
                                    #endregion
                                }
                                #endregion
                            }
                        }
                    }
                    if (cbsports.Checked == true)
                    {
                        if (isval1 == 1)
                        {
                            string type = "";
                            string edu = "";
                            string concat = "";
                            string deg_code = "";
                            string degree_code = "";
                            DataSet dsnew1 = new DataSet();
                            string app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(i), 2].Text);
                            Session["pdfapp_no"] = Convert.ToString(app_no);
                            dsnew1.Clear();
                            string jg = "select type,Dept_Name,Edu_Level ,Course_Name,a.degree_code,c.course_id  from applyn a,Degree d, Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and a.app_no ='" + app_no + "'";
                            dsnew1 = d2.select_method_wo_parameter(jg, "text");
                            if (dsnew1.Tables[0].Rows.Count > 0)
                            {
                                type = Convert.ToString(dsnew1.Tables[0].Rows[0]["type"]);
                                edu = Convert.ToString(dsnew1.Tables[0].Rows[0]["Edu_Level"]);
                                concat = type + "-" + edu;
                                deg_code = Convert.ToString(dsnew1.Tables[0].Rows[0]["course_id"]);
                                degree_code = Convert.ToString(dsnew1.Tables[0].Rows[0]["degree_code"]);
                            }
                            string dept_Code = d2.GetFunction("select Dept_Code  from Degree where Course_Id='" + deg_code + "' and Degree_Code ='" + degree_code + "' and college_code ='" + Session["collegecode"] + "'");

                            string text_circode = "";
                            string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + user_code + "' and college_code ='" + Session["collegecode"] + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(settingquery, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                                if (linkvalue == "0")
                                {
                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '1 Semester' and textval not like '-1%'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(semesterquery, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                                    }
                                }
                                else
                                {
                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '1 Year' and textval not like '-1%'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(semesterquery, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                                    }
                                }
                            }

                            fllg = 1;
                            string typecode = d2.GetFunction("select community from applyn where app_no ='" + app_no + "'");
                            string relig = "select * from admitcolumnset where TextCriteria='community'  and priority!='0'  and  setcolumn='" + degree_code + "' and college_code='" + college_code + "' and column_name='" + typecode + "'";
                            DataSet ds2 = new DataSet();
                            ds2 = d2.select_method_wo_parameter(relig, "text");
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                flage = true;
                                int inc = 0;

                                string alot = Convert.ToString(ds2.Tables[0].Rows[0]["allot"]);
                                string clm = Convert.ToString(ds2.Tables[0].Rows[0]["column_name"]);
                                int seatcount = 0;
                                string seatconfirm = Convert.ToString(ds2.Tables[0].Rows[0]["allot_Confirm"]);
                                if (seatconfirm.Trim() != "" && seatconfirm != null)
                                {
                                    seatcount = Convert.ToInt32(seatconfirm);
                                }
                                else
                                {
                                    seatcount = 0;
                                }
                                //if (Convert.ToInt32(ds2.Tables[0].Rows[0]["priority"].ToString()) > Convert.ToInt32(alot))
                                //{
                                if (alot.Trim() != "")
                                {
                                    inc = Convert.ToInt32(alot);
                                    inc++;
                                }
                                else
                                {
                                    inc = 1;
                                }
                                string rel = "update admitcolumnset set allot='" + inc + "' where setcolumn='" + degree_code + "' and column_name='" + clm + "' and textcriteria='community' and college_code='" + college_code + "' and column_name='" + typecode + "'";
                                rel = rel + " update applyn set selection_status='1' where app_no ='" + FpSpread3.Sheets[0].Cells[i, 2].Text + "'";
                                int f = d2.update_method_wo_parameter(rel, "text");
                                string approve1 = "if not exists (select * from selectcriteria where app_no ='" + FpSpread3.Sheets[0].Cells[i, 2].Text + "') insert into selectcriteria(app_no,usercode,degree_code,college_code,isapprove,select_date,isview,criteria_Code) values('" + FpSpread3.Sheets[0].Cells[i, 2].Text + "','" + user_code + "','" + degree_code + "','" + college_code + "','4','" + System.DateTime.Now.ToString("yyy/MM/dd") + "','0','" + typecode + "') else update selectcriteria set isapprove ='4',usercode='" + user_code + "',degree_code='" + degree_code + "',college_code='" + college_code + "',select_date='" + System.DateTime.Now.ToString("yyy/MM/dd") + "',criteria_Code='" + typecode + "' where app_no ='" + FpSpread3.Sheets[0].Cells[i, 2].Text + "'";
                                int a1 = d2.update_method_wo_parameter(approve1, "text");
                                bool feededuct = false;
                                bool feededuct1 = false;
                                string insertquery = "";
                                int insert = 0;
                                string batch_year = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Note);
                                string seattype = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note);
                                string today = System.DateTime.Now.ToString("MM/dd/yyyy");

                                //string selectquery = "select Headid,feecode,FeeAmount,Deduct,Duedate,DueExt1,fineamt1,DueExt2,FineAmt2,DueExt3,FineAmt3,Total,modeofpay,dedect_reason  from feedefine where batch = " + batch_year + " and DegreeCode = " + deg_code + " and depcode = " + dept_Code + " and FeeCat = " + text_circode + " and CollCode ='" + Session["collegecode"] + "'";
                                //selectquery = selectquery + "  select (SUM(FeeAmount)- SUM(Deduct)) as fee,SUM(Deduct) det, Headid  from feedefine where batch = " + batch_year + " and DegreeCode = " + deg_code + " and depcode = " + dept_Code + " and FeeCat = " + text_circode + " and CollCode ='" + Session["collegecode"] + "' group by Headid";
                                //selectquery = selectquery + " select s.university_code,s.uni_state,s.course_code from applyn a,Stud_prev_details s,TextValTable t where a.app_no =s.app_no   and a.app_formno  ='" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "' and batch_year =" + batch_year + " and degree_code =" + degree_code + " and current_semester =1";
                                string selectquery = " select LedgerFK,HeaderFK,PayMode,FeeAmount,deductAmout,DeductReason,TotalAmount,RefundAmount,FeeCategory,FineAmount from FT_FeeAllotDegree where DegreeCode='" + degree_code + "' and BatchYear ='" + batch_year + "' and SeatType ='" + seattype + "' and FeeCategory ='" + text_circode + "' and FinYearFK ='" + finYearid + "' ";

                                selectquery = selectquery + "  select top 1 * from collinfo ";

                                selectquery = selectquery + " select s.university_code,s.uni_state,s.course_code from applyn a,Stud_prev_details s,TextValTable t where a.app_no =s.app_no   and a.app_formno  ='" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "' and batch_year =" + batch_year + " and degree_code =" + degree_code + " and current_semester =1";

                                selectquery = selectquery + " select community,parent_statep  from applyn where app_formno  ='" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "' and batch_year =" + batch_year + " and degree_code =" + degree_code + " and current_semester =1";
                                dsnew.Clear();
                                dsnew = d2.select_method_wo_parameter(selectquery, "Text");
                                if (edu == "UG")
                                {
                                    if (dsnew.Tables[2].Rows.Count > 0)
                                    {
                                        string coursecode = Convert.ToString(dsnew.Tables[2].Rows[0]["course_code"]);
                                        string universitystate = Convert.ToString(dsnew.Tables[2].Rows[0]["uni_state"]);
                                        string coursevalue = subjectcode(Convert.ToString(dsnew.Tables[2].Rows[0]["course_code"]));
                                        string statevlaue = subjectcode(Convert.ToString(dsnew.Tables[2].Rows[0]["uni_state"]));
                                        if (Convert.ToString(coursevalue) == "HSC" && Convert.ToString(statevlaue) == "Tamil Nadu")
                                        {
                                            feededuct = true;
                                        }

                                    }
                                }
                                if (edu == "PG")
                                {
                                    if (dsnew.Tables[2].Rows.Count > 0)
                                    {
                                        string coursevalue = subjectcode(Convert.ToString(dsnew.Tables[2].Rows[0]["university_code"]));
                                        if (Convert.ToString(coursevalue) == "Madras University")
                                        {
                                            feededuct1 = true;
                                        }
                                    }
                                    if (type == "DAY")
                                    {

                                        if (dsnew.Tables[3].Rows.Count > 0)
                                        {
                                            string community = subjectcode(Convert.ToString(dsnew.Tables[3].Rows[0]["community"]));
                                            string state = subjectcode(Convert.ToString(dsnew.Tables[3].Rows[0]["parent_statep"]));
                                            if (Convert.ToString(community) == "SC" || Convert.ToString(community) == "SC(Arunthathiyar)" || Convert.ToString(community) == "ST" && Convert.ToString(state) == "Tamil Nadu")
                                            {
                                                feededuct = true;
                                            }
                                        }
                                    }
                                }
                                if (edu != "PG")
                                {
                                    if (dsnew.Tables[0].Rows.Count > 0)
                                    {
                                        for (int fee = 0; fee < dsnew.Tables[0].Rows.Count; fee++)
                                        {
                                            double total = 0;
                                            double deduct = 0;
                                            if (feededuct == false)
                                            {
                                                string getfeetype = d2.GetFunction("select LedgerName  from FM_LedgerMaster where LedgerPK  ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["ledgerfk"]) + "' and HeaderFK ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["headerfk"]) + "'");
                                                if (Convert.ToString(getfeetype).Trim() == "University Registration Fee")
                                                {
                                                    double feeamount = 0;
                                                    string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                                    if (feeamount1.Trim() != "")
                                                    {
                                                        feeamount = Convert.ToDouble(feeamount1);
                                                        deduct = 0;
                                                        total = feeamount + 200;
                                                    }
                                                    else
                                                    {
                                                        deduct = 0;
                                                        total = 0;
                                                    }

                                                }
                                                else
                                                {
                                                    deduct = Convert.ToDouble(dsnew.Tables[0].Rows[fee]["deductAmout"]);
                                                    total = (Convert.ToDouble(dsnew.Tables[0].Rows[fee]["FeeAmount"]) - Convert.ToDouble(dsnew.Tables[0].Rows[fee]["deductAmout"]));
                                                }
                                            }
                                            else
                                            {
                                                deduct = Convert.ToDouble(dsnew.Tables[0].Rows[fee]["deductAmout"]);
                                                total = Convert.ToDouble(dsnew.Tables[0].Rows[fee]["FeeAmount"]);
                                            }

                                            string headerfk = Convert.ToString(dsnew.Tables[0].Rows[fee]["HeaderFK"]);
                                            string leadgerfk = Convert.ToString(dsnew.Tables[0].Rows[fee]["LedgerFK"]);
                                            string feeAmount = Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]);
                                            string deductrea = Convert.ToString(dsnew.Tables[0].Rows[fee]["DeductReason"]);
                                            string finamount = Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmount"]);
                                            string refund = Convert.ToString(dsnew.Tables[0].Rows[fee]["RefundAmount"]);
                                            string feecatg = Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeCategory"]);
                                            string paymode = Convert.ToString(dsnew.Tables[0].Rows[fee]["PayMode"]);
                                            if (feeAmount.Trim() == "")
                                            {
                                                feeAmount = "0";
                                            }
                                            if (deduct == 0)
                                            {
                                                feeAmount = total.ToString();
                                            }
                                            if (finamount.Trim() == "")
                                            {
                                                finamount = "0";
                                            }

                                            string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + app_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeAmount + "',PaidAmount='0' ,DeductAmout='" + deduct + "',DeductReason='" + deductrea + "',FromGovtAmt='0',TotalAmount='" + total + "',RefundAmount='" + refund + "',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='" + paymode + "',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='" + finamount + "',BalAmount='" + total + "' where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "') and App_No in('" + app_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no + ",'" + leadgerfk + "','" + headerfk + "','" + feeAmount + "','" + deduct + "'," + deductrea + ",'0','" + total + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + total + "','" + finYearid + "')";

                                            insert = d2.update_method_wo_parameter(insupdquery, "Text");
                                            checkflage = true;
                                            #region for Unwanted Queries
                                            //insertquery = "insert into fee_allot(app_formno,Header_ID,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,roll_admit,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt,college_code)values('" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Headid"]) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "','" + today + "','false','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]) + "','" + text_circode + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Duedate"]) + "',0,0," + deduct + ",'" + total + "','N',0,'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt1"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["fineamt1"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt2"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt2"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt3"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt3"]) + ",'',0,1,'" + batch_year + "',0,0," + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["modeofpay"]) + "','','','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["dedect_reason"]) + "','','','','','','" + Convert.ToString(Session["collegecode"]) + "')";

                                            //insertquery = "insert into fee_allot(app_formno,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,roll_admit,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt,college_code)values('" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "','" + today + "','false','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]) + "','" + text_circode + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Duedate"]) + "',0,0," + deduct + ",'" + total + "','N',0,'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt1"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["fineamt1"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt2"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt2"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt3"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt3"]) + ",'',0,1,'" + batch_year + "',0,0," + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["modeofpay"]) + "','','','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["dedect_reason"]) + "','','','','','','" + Convert.ToString(Session["collegecode"]) + "')";
                                            //insert = dt.update_method_wo_parameter(insertquery, "Text");
                                            //}
                                            //else
                                            //{
                                            //    insertquery = "insert into fee_allot(app_formno,Header_ID,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,roll_admit,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt,college_code)values('" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Headid"]) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "','" + today + "','false','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]) + "','" + text_circode + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Duedate"]) + "',0,0," + Convert.ToString(dsnew.Tables[0].Rows[fee]["Deduct"]) + ",'" + (Convert.ToInt32(dsnew.Tables[0].Rows[fee]["FeeAmount"]) - Convert.ToInt32(dsnew.Tables[0].Rows[fee]["Deduct"])) + "','N',0,'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt1"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["fineamt1"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt2"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt2"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt3"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt3"]) + ",'',0,1,'" + batch_year + "',0,0," + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["modeofpay"]) + "','','','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["dedect_reason"]) + "','','','','','','" + Convert.ToString(Session["collegecode"]) + "')";
                                            //    insert = d2.update_method_wo_parameter(insertquery, "Text");
                                            //}
                                            //}
                                            #endregion

                                            #region for Unwanted Codings
                                            //if (dsnew.Tables[1].Rows.Count > 0)
                                            //{
                                            //    for (int stat = 0; stat < dsnew.Tables[1].Rows.Count; stat++)
                                            //    {
                                            //        if (feededuct == true)
                                            //        {
                                            //            double total = 0;
                                            //            double deduct = 0;
                                            //            string getfeetype = d2.GetFunction("select header_name from acctheader where   header_id ='" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "'");
                                            //            if (Convert.ToString(getfeetype).Trim() == "NON SALARY" || Convert.ToString(getfeetype).Trim() == "NON SALARY")
                                            //            {
                                            //                double feeamount = 0;
                                            //                string feeamount1 = Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]);
                                            //                if (feeamount1.Trim() != "")
                                            //                {
                                            //                    feeamount = Convert.ToDouble(feeamount1);
                                            //                    deduct = 200;
                                            //                    total = feeamount - deduct;
                                            //                }
                                            //                else
                                            //                {
                                            //                    deduct = 0;
                                            //                    total = 0;
                                            //                }
                                            //            }
                                            //            else
                                            //            {
                                            //                total = Convert.ToDouble(Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]));
                                            //            }

                                            //            insertquery = "insert into fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno,college_code)values('','" + total + "',0,'" + total + "','false','" + text_circode + "','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "',0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(Session["collegecode"]) + "')";
                                            //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                            //        }
                                            //        else
                                            //        {
                                            //            insertquery = "insert into fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno,college_code)values('','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]) + "',0,'" + Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]) + "','false','" + text_circode + "','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "',0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(Session["collegecode"]) + "')";
                                            //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                            //        }
                                            //    }
                                            //}
                                            #endregion
                                        }
                                    }
                                }
                                #region for PG
                                else
                                {
                                    if (dsnew.Tables[0].Rows.Count > 0)
                                        //{
                                        //    for (int fee = 0; fee < dsnew.Tables[0].Rows.Count; fee++)
                                        //    {
                                        //        bool tempflage = false;
                                        //        if (feededuct == true || feededuct1 == true)
                                        //        {
                                        //            double total = 0;
                                        //            double deduct = 0;
                                        //            string getfeetype = d2.GetFunction("select fee_type  from fee_info where fee_code  ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "' and header_id ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Headid"]) + "'");
                                        //            if (feededuct == true)
                                        //            {
                                        //                if (Convert.ToString(getfeetype).Trim() == "Tuition Fee")
                                        //                {
                                        //                    tempflage = true;
                                        //                    double feeamount = 0;
                                        //                    string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                        //                    if (feeamount1.Trim() != "")
                                        //                    {
                                        //                        feeamount = Convert.ToDouble(feeamount1);
                                        //                        deduct = feeamount;
                                        //                        total = feeamount - feeamount;
                                        //                    }
                                        //                    else
                                        //                    {
                                        //                        deduct = 0;
                                        //                        total = 0;
                                        //                    }
                                        //                }
                                        //            }
                                        //            if (feededuct1 == true)
                                        //            {
                                        //                if (Convert.ToString(getfeetype).Trim() == "University Registration Fee")
                                        //                {
                                        //                    tempflage = true;
                                        //                    double feeamount = 0;
                                        //                    string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                        //                    if (feeamount1.Trim() != "")
                                        //                    {
                                        //                        feeamount = Convert.ToDouble(feeamount1);
                                        //                        deduct = 20;
                                        //                        total = feeamount - feeamount;
                                        //                    }
                                        //                    else
                                        //                    {
                                        //                        deduct = 0;
                                        //                        total = 0;
                                        //                    }
                                        //                }
                                        //                else if (Convert.ToString(getfeetype).Trim() == "Matric And Recognition Fee")
                                        //                {
                                        //                    tempflage = true;
                                        //                    double feeamount = 0;
                                        //                    string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                        //                    if (feeamount1.Trim() != "")
                                        //                    {
                                        //                        feeamount = Convert.ToDouble(feeamount1);
                                        //                        deduct = feeamount;
                                        //                        total = feeamount - feeamount;
                                        //                    }
                                        //                    else
                                        //                    {
                                        //                        deduct = 0;
                                        //                        total = 0;
                                        //                    }
                                        //                }
                                        //            }
                                        //            if (tempflage == false)
                                        //            {
                                        //                deduct = Convert.ToInt32(dsnew.Tables[0].Rows[fee]["Deduct"]);
                                        //                total = (Convert.ToInt32(dsnew.Tables[0].Rows[fee]["FeeAmount"]) - Convert.ToInt32(dsnew.Tables[0].Rows[fee]["Deduct"]));
                                        //            }
                                        //            insertquery = "insert into fee_allot(app_formno,Header_ID,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,roll_admit,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt,college_code)values('" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Headid"]) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "','" + today + "','false','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]) + "','" + text_circode + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Duedate"]) + "',0,0," + deduct + ",'" + total + "','N',0,'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt1"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["fineamt1"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt2"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt2"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt3"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt3"]) + ",'',0,1,'" + batch_year + "',0,0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["modeofpay"]) + "','','','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["dedect_reason"]) + "','','','','','','" + Convert.ToString(Session["collegecode"]) + "')";
                                        //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                        //        }
                                        //        else
                                        //        {
                                        //            insertquery = "insert into fee_allot(app_formno,Header_ID,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,roll_admit,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt,college_code)values('" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Headid"]) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["feecode"]) + "','" + today + "','false','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]) + "','" + text_circode + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["Duedate"]) + "',0,0," + Convert.ToString(dsnew.Tables[0].Rows[fee]["Deduct"]) + ",'" + (Convert.ToInt32(dsnew.Tables[0].Rows[fee]["FeeAmount"]) - Convert.ToInt32(dsnew.Tables[0].Rows[fee]["Deduct"])) + "','N',0,'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt1"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["fineamt1"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt2"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt2"]) + ",'" + Convert.ToString(dsnew.Tables[0].Rows[fee]["DueExt3"]) + "'," + Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmt3"]) + ",'',0,1,'" + batch_year + "',0,0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Note) + "','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["modeofpay"]) + "','','','" + Convert.ToString(dsnew.Tables[0].Rows[fee]["dedect_reason"]) + "','','','','','','" + Convert.ToString(Session["collegecode"]) + "')";
                                        //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                        //        }
                                        //        //}
                                        //    }
                                        //}
                                        if (dsnew.Tables[0].Rows.Count > 0)
                                        {
                                            for (int fee = 0; fee < dsnew.Tables[0].Rows.Count; fee++)
                                            {
                                                bool tempflage = false;
                                                double total = 0;
                                                double deduct = 0;
                                                if (feededuct == true || feededuct1 == false)
                                                {

                                                    string getfeetype = d2.GetFunction("select LedgerName  from FM_LedgerMaster where LedgerPK  ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["ledgerfk"]) + "' and HeaderFK ='" + Convert.ToString(dsnew.Tables[0].Rows[fee]["headerfk"]) + "'");
                                                    if (feededuct == true)
                                                    {
                                                        if (Convert.ToString(getfeetype).Trim() == "Tuition Fee")
                                                        {
                                                            tempflage = true;
                                                            double feeamount = 0;
                                                            string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                                            if (feeamount1.Trim() != "")
                                                            {
                                                                feeamount = Convert.ToDouble(feeamount1);
                                                                deduct = feeamount;
                                                                total = feeamount - feeamount;
                                                            }
                                                            else
                                                            {
                                                                deduct = 0;
                                                                total = 0;
                                                            }
                                                        }
                                                    }
                                                    if (feededuct1 == false)
                                                    {
                                                        if (Convert.ToString(getfeetype).Trim() == "University Registration Fee")
                                                        {
                                                            tempflage = true;
                                                            double feeamount = 0;
                                                            string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                                            if (feeamount1.Trim() != "")
                                                            {
                                                                feeamount = Convert.ToDouble(feeamount1);
                                                                //deduct = 20;
                                                                total = feeamount + 20;
                                                            }
                                                            else
                                                            {
                                                                //deduct = 0;
                                                                total = 20;
                                                            }
                                                        }
                                                        else if (Convert.ToString(getfeetype).Trim() == "Matric And Recognition Fee")
                                                        {
                                                            tempflage = true;
                                                            double feeamount = 0;
                                                            string feeamount1 = Convert.ToString(Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]));
                                                            if (feeamount1.Trim() != "")
                                                            {
                                                                feeamount = Convert.ToDouble(feeamount1);
                                                                //deduct = feeamount;
                                                                total = feeamount + 30;//feeamount - feeamount;
                                                            }
                                                            else
                                                            {
                                                                //deduct = 0;
                                                                total = 30;
                                                            }
                                                        }
                                                    }
                                                    if (tempflage == false)
                                                    {
                                                        double.TryParse(dsnew.Tables[0].Rows[fee]["deductAmout"].ToString(), out deduct);
                                                        double feeamt = 0;
                                                        double.TryParse(dsnew.Tables[0].Rows[fee]["FeeAmount"].ToString(), out feeamt);
                                                        total = feeamt - deduct;
                                                    }
                                                }
                                                else
                                                {
                                                    double.TryParse(Convert.ToString(dsnew.Tables[0].Rows[fee]["deductAmout"]), out deduct);
                                                    double.TryParse(Convert.ToString(dsnew.Tables[0].Rows[fee]["TotalAmount"]), out total);
                                                }

                                                string headerfk = Convert.ToString(dsnew.Tables[0].Rows[fee]["HeaderFK"]);
                                                string leadgerfk = Convert.ToString(dsnew.Tables[0].Rows[fee]["LedgerFK"]);
                                                string feeAmount = Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeAmount"]);
                                                //deduct = Convert.ToString(dsnew.Tables[0].Rows[fee]["deductAmout"]);
                                                string deductrea = Convert.ToString(dsnew.Tables[0].Rows[fee]["DeductReason"]);
                                                //totalamount = Convert.ToString(dsnew.Tables[0].Rows[fee]["TotalAmount"]);
                                                string finamount = Convert.ToString(dsnew.Tables[0].Rows[fee]["FineAmount"]);
                                                string refund = Convert.ToString(dsnew.Tables[0].Rows[fee]["RefundAmount"]);
                                                string feecatg = Convert.ToString(dsnew.Tables[0].Rows[fee]["FeeCategory"]);
                                                string paymode = Convert.ToString(dsnew.Tables[0].Rows[fee]["PayMode"]);
                                                if (feeAmount.Trim() == "")
                                                {
                                                    feeAmount = "0";
                                                }
                                                if (total > Convert.ToDouble(feeAmount))
                                                {
                                                    feeAmount = total.ToString();
                                                }
                                                if (finamount.Trim() == "")
                                                {
                                                    finamount = "0";
                                                }
                                                string app_no1 = d2.GetFunction("select app_no from applyn where app_formno='" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "'").Trim();
                                                string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "')  and App_No in('" + app_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeAmount + "',PaidAmount='0' ,DeductAmout='" + deduct + "',DeductReason='" + deductrea + "',FromGovtAmt='0',TotalAmount='" + total + "',RefundAmount='" + refund + "',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='" + paymode + "',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='" + finamount + "',BalAmount='" + total + "' where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "') and App_No in('" + app_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no + ",'" + leadgerfk + "','" + headerfk + "','" + feeAmount + "','" + deduct + "'," + deductrea + ",'0','" + total + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + total + "','" + finYearid + "')";

                                                insert = d2.update_method_wo_parameter(insupdquery, "Text");

                                            }
                                        }
                                    #region for Unwanted Codings
                                    //if (dsnew.Tables[1].Rows.Count > 0)
                                    //{
                                    //    for (int stat = 0; stat < dsnew.Tables[1].Rows.Count; stat++)
                                    //    {
                                    //        if (feededuct == true || feededuct1 == true)
                                    //        {
                                    //            double total = 0;
                                    //            double deduct = 0;
                                    //            bool dummyflage = false;
                                    //            string getfeetype = d2.GetFunction("select header_name from acctheader where  header_id ='" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "'");
                                    //            if (feededuct1 == true)
                                    //            {
                                    //                if (Convert.ToString(getfeetype).Trim() == "NON SALARY" || Convert.ToString(getfeetype).Trim() == "NON SALARY")
                                    //                {
                                    //                    dummyflage = true;
                                    //                    double feeamount = 0;
                                    //                    string feeamount1 = Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]);
                                    //                    if (feeamount1.Trim() != "")
                                    //                    {
                                    //                        feeamount = Convert.ToDouble(feeamount1);
                                    //                        deduct = 50;
                                    //                        total = feeamount - deduct;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        deduct = 0;
                                    //                        total = 0;
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (feededuct == true)
                                    //            {
                                    //                if (Convert.ToString(getfeetype).Trim() == "TUITION FEE")
                                    //                {
                                    //                    dummyflage = true;
                                    //                    double feeamount = 0;
                                    //                    string feeamount1 = Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]);
                                    //                    if (feeamount1.Trim() != "")
                                    //                    {
                                    //                        feeamount = Convert.ToDouble(feeamount1);
                                    //                        deduct = feeamount;
                                    //                        total = feeamount - deduct;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        deduct = 0;
                                    //                        total = 0;
                                    //                    }
                                    //                }
                                    //            }
                                    //            if (dummyflage == false)
                                    //            {
                                    //                total = Convert.ToDouble(Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]));
                                    //            }
                                    //            insertquery = "insert into fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno,college_code)values('','" + total + "',0,'" + total + "','false','" + text_circode + "','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "',0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(Session["collegecode"]) + "')";
                                    //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                    //        }
                                    //        else
                                    //        {
                                    //            insertquery = "insert into fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno,college_code)values('','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]) + "',0,'" + Convert.ToString(dsnew.Tables[1].Rows[stat]["fee"]) + "','false','" + text_circode + "','" + Convert.ToString(dsnew.Tables[1].Rows[stat]["Headid"]) + "',0,'" + Convert.ToString(FpSpread3.Sheets[0].Cells[i, 2].Tag) + "','" + Convert.ToString(Session["collegecode"]) + "')";
                                    //            insert = d2.update_method_wo_parameter(insertquery, "Text");
                                    //        }
                                    //    }
                                    //}
                                    #endregion

                                    //}
                                    //else
                                    //{
                                    //    string approveaa = "delete from selectcriteria  where app_no='" + FpSpread3.Sheets[0].Cells[i, 2].Text + "' and usercode='" + user_code + "' and degree_code='" + degree_code + "' and college_code='" + college_code + "' and criteria_Code='" + typecode + "'";
                                    //    int asd = d2.update_method_wo_parameter(approveaa, "text");
                                    //    // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Seat Avilable In This Department Please Admit Some Other Department')", true);
                                    //    //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Seat Avilable In This Category Please Admit Some Other Category\");", true);
                                    //    return;
                                    //}
                                }
                                #endregion
                            }
                        }
                    }
                }
                if (checkflage == false)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select Any one Students";
                }


            }
        }
        catch (Exception ex)
        {

        }
    }

    //public void pdf()
    //{
    //    try
    //    {
    //        Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
    //        Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
    //        Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
    //        Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
    //        Gios.Pdf.PdfPage mypage = mydoc.NewPage();
    //        Gios.Pdf.PdfPage mypage1 = mydoc.NewPage();
    //        Gios.Pdf.PdfPage mypage2 = mydoc.NewPage();
    //        bool dummyflage = false;
    //        if (File.Exists(HttpContext.Current.Server.MapPath("~/image/logo.jpg")))//Aruna
    //        {
    //            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/image/logo.jpg"));
    //            mypage.Add(LogoImage, 20, 20, 200);
    //        }
    //        if (File.Exists(HttpContext.Current.Server.MapPath("~/image/logo1.jpg")))//Aruna
    //        {
    //            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/image/logo1.jpg"));
    //            mypage.Add(LogoImage, 500, 20, 200);
    //        }

    //        string collquery = "";
    //        collquery = "select collname,category,university,address1,address2,address3,phoneno,faxno,email,website,district,state,pincode  from collinfo where college_Code=" + college_code + "";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(collquery, "Text");
    //        string collegename = "";
    //        string collegeaddress = "";
    //        string collegedistrict = "";
    //        string phonenumber = "";
    //        string fax = "";
    //        string email = "";
    //        string website = "";
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
    //            collegeaddress = Convert.ToString(ds.Tables[0].Rows[0]["address1"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address2"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
    //            collegedistrict = Convert.ToString(ds.Tables[0].Rows[0]["district"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["state"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
    //            phonenumber = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
    //            fax = Convert.ToString(ds.Tables[0].Rows[0]["faxno"]); ;
    //            email = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
    //            website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
    //        }

    //        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, 10, 10, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
    //        mypage.Add(ptc);
    //        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, 110, 25, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegeaddress);
    //        mypage.Add(ptc);
    //        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, 110, 35, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegedistrict);
    //        mypage.Add(ptc);
    //        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, 110, 45, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phone No: " + phonenumber + ", Fax:" + fax);
    //        mypage.Add(ptc);
    //        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, 110, 55, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, email);
    //        mypage.Add(ptc);
    //        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, 110, 65, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, website);

    //        mypage.Add(ptc);

    //        int y = 60;
    //        int line1 = 50;
    //        int line2 = 400;

    //        string query = "select app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,degree_code,batch_year,college_code,SubCaste,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet from applyn a where a.app_no='" + Convert.ToString(Session["pdfapp_no"]) + "'";
    //        query = query + " select course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark from Stud_prev_details where app_no ='" + Convert.ToString(Session["pdfapp_no"]) + "'";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(query, "text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 50, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Course Details");
    //            mypage.Add(ptc);


    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Stream");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ddltype.SelectedItem.Text));
    //            mypage.Add(ptc);

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Graduation");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ddledu.SelectedItem.Text));
    //            mypage.Add(ptc);

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 110, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 110, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ddldegree.SelectedItem.Text));
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Course");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ddldept.SelectedItem.Text));
    //            mypage.Add(ptc);




    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                              new PdfArea(mydoc, line1, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Application No");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["app_formno"]));
    //            mypage.Add(ptc);

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Applicant Name");
    //            mypage.Add(ptc);

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                            new PdfArea(mydoc, line2, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
    //            mypage.Add(ptc);

    //            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //            //                                                new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Applicant Last  Name");
    //            //mypage.Add(ptc);

    //            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //            //                                              new PdfArea(mydoc, line2, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Session["lastname"]));
    //            //mypage.Add(ptc);



    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, line1, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date of Birth");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, line2, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["dob"]));
    //            mypage.Add(ptc);

    //            string gender = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
    //            if (gender == "0")
    //            {
    //                gender = "Male";
    //            }
    //            else if (gender == "1")
    //            {
    //                gender = "Female";
    //            }
    //            else if (gender == "2")
    //            {
    //                gender = "Transgender";
    //            }

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Gender");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line2, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(gender));
    //            mypage.Add(ptc);


    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, line1, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Parent's Name/Guardian Name");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line2, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]));
    //            mypage.Add(ptc);


    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, line1, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Relationship");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line2, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Relationship"]));
    //            mypage.Add(ptc);

    //            string occupation = Convert.ToString(ds.Tables[0].Rows[0]["parent_occu"]);
    //            if (occupation.Trim() != "")
    //            {
    //                occupation = subjectcode(occupation);
    //            }

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, line1, y + 290, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Occupation");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line2, y + 290, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Session["occupation"]));
    //            mypage.Add(ptc);

    //            string mothertounge = Convert.ToString(ds.Tables[0].Rows[0]["mother_tongue"]);
    //            if (mothertounge.Trim() != "")
    //            {
    //                mothertounge = subjectcode(mothertounge);
    //            }
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, line1, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Mother Tounge ");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line2, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(mothertounge));
    //            mypage.Add(ptc);


    //            string Religion = Convert.ToString(ds.Tables[0].Rows[0]["religion"]);
    //            if (Religion.Trim() != "")
    //            {
    //                Religion = subjectcode(Religion);
    //            }

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, line1, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Religion");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line2, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Religion));
    //            mypage.Add(ptc);

    //            string Nationality = Convert.ToString(ds.Tables[0].Rows[0]["citizen"]);

    //            if (Nationality.Trim() != "")
    //            {
    //                Nationality = subjectcode(Nationality);
    //            }

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Nationality");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line2, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Nationality));
    //            mypage.Add(ptc);

    //            string coummunity = Convert.ToString(ds.Tables[0].Rows[0]["community"]);

    //            if (coummunity.Trim() != "")
    //            {
    //                coummunity = subjectcode(coummunity);
    //            }

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Coummunity(Foriegn Students Select OC)");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                       new PdfArea(mydoc, line2, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(coummunity));
    //            mypage.Add(ptc);

    //            string caste = Convert.ToString(ds.Tables[0].Rows[0]["caste"]);

    //            if (caste.Trim() != "")
    //            {
    //                caste = subjectcode(caste);
    //            }

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Caste");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                        new PdfArea(mydoc, line2, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(caste));
    //            mypage.Add(ptc);

    //            string subreligion = Convert.ToString(ds.Tables[0].Rows[0]["caste"]);

    //            if (subreligion.Trim() != "")
    //            {
    //                subreligion = subjectcode(subreligion);
    //            }

    //            int col = y + 390;
    //            if (Convert.ToString(subreligion).ToUpper() == "PROTESTANT")
    //            {
    //                string missionarychild = Convert.ToString(ds.Tables[0].Rows[0]["MissionaryChild"]);
    //                if (missionarychild == "0" || missionarychild == "False")
    //                {
    //                    missionarychild = "No";
    //                }
    //                else
    //                {
    //                    missionarychild = "Yes";
    //                }
    //                col += 20;
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are You a missionary child ?");
    //                mypage.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                            new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(missionarychild));
    //                mypage.Add(ptc);
    //            }

    //            string tamilorgion = Convert.ToString(ds.Tables[0].Rows[0]["TamilOrginFromAndaman"]);
    //            if (tamilorgion.Trim() == "0" || tamilorgion.Trim() == "False")
    //            {
    //                tamilorgion = "No";
    //            }
    //            else
    //            {
    //                tamilorgion = "Yes";
    //            }

    //            col += 20;
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are You of Tamil Origin From Andaman and Nicobar Islands ? ");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(tamilorgion));
    //            mypage.Add(ptc);
    //            string xserviceman = Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]);
    //            if (xserviceman.Trim() == "0" || xserviceman.Trim() == "False")
    //            {
    //                xserviceman = "No";
    //            }
    //            else
    //            {
    //                xserviceman = "Yes";
    //            }

    //            col += 20;
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are You a Child of an Ex-serviceman of Tamil Nadu origin ?");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(xserviceman));
    //            mypage.Add(ptc);

    //            string differentlyabled = Convert.ToString(ds.Tables[0].Rows[0]["isdisable"]);
    //            if (differentlyabled.Trim() == "0" || differentlyabled.Trim() == "False")
    //            {
    //                differentlyabled = "No";
    //            }
    //            else
    //            {
    //                differentlyabled = "Yes";
    //            }

    //            col += 20;
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are you a Differently abled");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(differentlyabled));
    //            mypage.Add(ptc);

    //            string firstgeneration = Convert.ToString(ds.Tables[0].Rows[0]["first_graduate"]);
    //            if (firstgeneration.Trim() == "0" || firstgeneration.Trim() == "False")
    //            {
    //                firstgeneration = "No";
    //            }
    //            else
    //            {
    //                firstgeneration = "Yes";
    //            }

    //            col += 20;
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are you a first genaration learner ?");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(firstgeneration));
    //            mypage.Add(ptc);

    //            string oncampus = Convert.ToString(ds.Tables[0].Rows[0]["CampusReq"]);
    //            if (oncampus.Trim() == "0" || oncampus.Trim() == "False")
    //            {
    //                oncampus = "No";
    //            }
    //            else
    //            {
    //                oncampus = "Yes";
    //            }

    //            col += 20;
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Is Residence on Campus Required ?");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(oncampus));
    //            mypage.Add(ptc);


    //            string sports = Convert.ToString(ds.Tables[0].Rows[0]["DistinctSport"]);

    //            if (sports.Trim() != "")
    //            {
    //                sports = subjectcode(sports);
    //            }
    //            col += 20;
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Distinction in Sports");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                      new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(sports));
    //            mypage.Add(ptc);

    //            string cocucuricular = Convert.ToString(ds.Tables[0].Rows[0]["co_curricular"]);

    //            if (cocucuricular.Trim() != "")
    //            {
    //                cocucuricular = subjectcode(cocucuricular);
    //            }

    //            col += 20;
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                      new PdfArea(mydoc, line1, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "Extra Curricular Activites/Co-Curricular Activites");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                      new PdfArea(mydoc, line2, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(cocucuricular));
    //            mypage.Add(ptc);

    //            col += 20;
    //            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Communication Address");
    //            mypage.Add(ptc);


    //            string addressline1 = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressC"]);

    //            col += 20;
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line1");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addressline1));
    //            mypage.Add(ptc);

    //            string addressline2 = Convert.ToString(ds.Tables[0].Rows[0]["Streetc"]);
    //            string addressline3 = "";
    //            if (addressline2.Contains('/') == true)
    //            {
    //                string[] splitaddress = addressline2.Split('/');
    //                if (splitaddress.Length > 1)
    //                {
    //                    addressline2 = Convert.ToString(splitaddress[0]);
    //                    addressline3 = Convert.ToString(splitaddress[1]);
    //                }
    //                else
    //                {
    //                    addressline2 = Convert.ToString(splitaddress[0]);
    //                }
    //            }


    //            col += 20;
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line2");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addressline2));
    //            mypage.Add(ptc);

    //            col += 20;
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line3");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addressline3));
    //            mypage.Add(ptc);
    //            col += 20;



    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "City");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Cityc"]));
    //            mypage.Add(ptc);

    //            string pstate = Convert.ToString(ds.Tables[0].Rows[0]["parent_statec"]);

    //            if (pstate.Trim() != "")
    //            {
    //                pstate = subjectcode(pstate);
    //            }

    //            col += 20;

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "State");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(pstate));
    //            mypage.Add(ptc);

    //            col += 20;

    //            string country = Convert.ToString(ds.Tables[0].Rows[0]["Countryc"]);

    //            if (country.Trim() != "")
    //            {
    //                country = subjectcode(country);
    //            }

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Country");
    //            mypage.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(country));
    //            mypage.Add(ptc);



    //            y = 40;

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 10, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "PIN code");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 10, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodec"]));
    //            mypage1.Add(ptc);



    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                new PdfArea(mydoc, line1, y + 30, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 30, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]));
    //            mypage1.Add(ptc);

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                              new PdfArea(mydoc, line1, y + 50, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Alternate Number");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 50, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["alter_mobileno"]));
    //            mypage1.Add(ptc);





    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Email ID");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]));
    //            mypage1.Add(ptc);


    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Phone Number With STD Code");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_phnoc"]));
    //            mypage1.Add(ptc);


    //            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 110, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Permanent Address");
    //            mypage1.Add(ptc);

    //            string addresslinec1 = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]);

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                   new PdfArea(mydoc, line1, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line1");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addresslinec1));
    //            mypage1.Add(ptc);

    //            string addresslinec2 = Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]);
    //            string addresslinec3 = "";
    //            if (addressline2.Contains('/') == true)
    //            {
    //                string[] splitaddress = addressline2.Split('/');
    //                if (splitaddress.Length > 1)
    //                {
    //                    addresslinec2 = Convert.ToString(splitaddress[0]);
    //                    addresslinec3 = Convert.ToString(splitaddress[1]);
    //                }
    //                else
    //                {
    //                    addresslinec2 = Convert.ToString(splitaddress[0]);
    //                }
    //            }

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line2");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addresslinec2));
    //            mypage1.Add(ptc);


    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line3");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addresslinec3));
    //            mypage1.Add(ptc);

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "City");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["cityp"]));
    //            mypage1.Add(ptc);

    //            string cstate = Convert.ToString(ds.Tables[0].Rows[0]["parent_statep"]);

    //            if (cstate.Trim() != "")
    //            {
    //                cstate = subjectcode(cstate);
    //            }


    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "State");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(cstate));
    //            mypage1.Add(ptc);

    //            string ccournty = Convert.ToString(ds.Tables[0].Rows[0]["Countryp"]);

    //            if (ccournty.Trim() != "")
    //            {
    //                ccournty = subjectcode(ccournty);
    //            }

    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Country");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ccournty));
    //            mypage1.Add(ptc);


    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "PIN code");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]));
    //            mypage1.Add(ptc);



    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Phone Number With STD Code");
    //            mypage1.Add(ptc);
    //            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                     new PdfArea(mydoc, line2, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]));
    //            mypage1.Add(ptc);





    //            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 290, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Academic Details");
    //            mypage1.Add(ptc);

    //            if (ddledu.SelectedItem.Text.ToUpper() == "UG")
    //            {

    //                string qualifyingexam = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);

    //                if (qualifyingexam.Trim() != "")
    //                {
    //                    qualifyingexam = subjectcode(qualifyingexam);
    //                }

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydoc, line1, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Qualifying Examination Passed");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(qualifyingexam));
    //                mypage1.Add(ptc);


    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydoc, line1, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name of School");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]));
    //                mypage1.Add(ptc);


    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydoc, line1, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Location of School");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["instaddress"]));
    //                mypage1.Add(ptc);

    //                string mediumofstudy = Convert.ToString(ds.Tables[1].Rows[0]["medium"]);

    //                if (mediumofstudy.Trim() != "")
    //                {
    //                    mediumofstudy = subjectcode(mediumofstudy);
    //                }

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                   new PdfArea(mydoc, line1, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Medium of Study of Qualifying Examination");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(mediumofstudy));
    //                mypage1.Add(ptc);

    //                string qulifyboard = Convert.ToString(ds.Tables[1].Rows[0]["university_code"]);

    //                if (qulifyboard.Trim() != "")
    //                {
    //                    qulifyboard = subjectcode(qulifyboard);
    //                }

    //                string qulifystate = Convert.ToString(ds.Tables[1].Rows[0]["uni_state"]);

    //                if (qulifystate.Trim() != "")
    //                {
    //                    qulifystate = subjectcode(qulifystate);
    //                }

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydoc, line1, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Qualifying Board & State");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(qulifyboard) + " " + Convert.ToString(qulifystate));
    //                mypage1.Add(ptc);

    //                string vocationalstream = Convert.ToString(ds.Tables[1].Rows[0]["Vocational_stream"]);
    //                if (vocationalstream.Trim() == "0" || vocationalstream.Trim() == "False")
    //                {
    //                    vocationalstream = "No";
    //                }
    //                else
    //                {
    //                    vocationalstream = "Yes";
    //                }
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                   new PdfArea(mydoc, line1, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are you Vocational stream");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(vocationalstream));
    //                mypage1.Add(ptc);

    //                string markgrade = Convert.ToString(ds.Tables[1].Rows[0]["isgrade"]);
    //                if (markgrade.Trim() == "False")
    //                {
    //                    markgrade = "Mark";
    //                }
    //                else
    //                {
    //                    markgrade = "Grade";
    //                }

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                   new PdfArea(mydoc, line1, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Marks/Grade");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(markgrade));
    //                mypage1.Add(ptc);

    //                string percentage = Convert.ToString(ds.Tables[1].Rows[0]["percentage"]);
    //                int totalmark = 0;
    //                int maxtotal = 0;
    //                DataTable data = new DataTable();
    //                DataRow dr = null;
    //                Hashtable hash = new Hashtable();
    //                string markquery = "select psubjectno,registerno,acual_marks,grade,max_marks,noofattempt,pass_month,pass_year from perv_marks_history  where course_entno ='" + Convert.ToString(ds.Tables[1].Rows[0]["course_entno"]) + "'";
    //                ds.Clear();
    //                ds = dt.select_method_wo_parameter(markquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {

    //                    data.Columns.Add("Language", typeof(string));
    //                    data.Columns.Add("Subject", typeof(string));
    //                    data.Columns.Add("Marks Obtained", typeof(string));
    //                    data.Columns.Add("Month", typeof(string));
    //                    data.Columns.Add("Year", typeof(string));
    //                    data.Columns.Add("Register No / Roll No", typeof(string));
    //                    data.Columns.Add("No of Attempts", typeof(string));
    //                    data.Columns.Add("Maximum Marks", typeof(string));

    //                    hash.Add(0, "Language1");
    //                    hash.Add(1, "Language2");
    //                    hash.Add(2, " Subject1");
    //                    hash.Add(3, " Subject2");
    //                    hash.Add(4, " Subject3");
    //                    hash.Add(5, " Subject4");
    //                    hash.Add(6, " Subject5");
    //                    hash.Add(7, " Subject6");
    //                    hash.Add(8, " Subject7");
    //                    hash.Add(9, " Subject8");
    //                    hash.Add(10, " Subject9");
    //                    hash.Add(11, " Subject10");
    //                    hash.Add(12, " Subject11");
    //                    for (int mark = 0; mark < ds.Tables[0].Rows.Count; mark++)
    //                    {
    //                        string subjectno = Convert.ToString(ds.Tables[0].Rows[mark]["psubjectno"]);
    //                        string actualmark = "";
    //                        if (markgrade.Trim() == "Mark")
    //                        {
    //                            actualmark = Convert.ToString(ds.Tables[0].Rows[mark]["acual_marks"]);
    //                        }
    //                        if (markgrade.Trim() == "Grade")
    //                        {
    //                            actualmark = Convert.ToString(ds.Tables[0].Rows[mark]["grade"]);
    //                        }
    //                        string month = Convert.ToString(ds.Tables[0].Rows[mark]["pass_month"]);
    //                        string year = Convert.ToString(ds.Tables[0].Rows[mark]["pass_year"]);
    //                        string regno = Convert.ToString(ds.Tables[0].Rows[mark]["registerno"]);
    //                        string noofattenm = Convert.ToString(ds.Tables[0].Rows[mark]["noofattempt"]);
    //                        string maxmark = Convert.ToString(ds.Tables[0].Rows[mark]["max_marks"]);
    //                        dr = data.NewRow();
    //                        string lang = Convert.ToString(hash[mark]);
    //                        dr[0] = Convert.ToString(lang);
    //                        string sub = subjectcode(subjectno);
    //                        dr[1] = Convert.ToString(sub);
    //                        dr[2] = Convert.ToString(actualmark);
    //                        dr[3] = Convert.ToString(month);
    //                        dr[4] = Convert.ToString(year);
    //                        dr[5] = Convert.ToString(regno);
    //                        dr[6] = Convert.ToString(noofattenm);
    //                        dr[7] = Convert.ToString(maxmark);
    //                        data.Rows.Add(dr);
    //                        if (markgrade.Trim() != "Grade")
    //                        {
    //                            totalmark = totalmark + Convert.ToInt32(actualmark);
    //                            maxtotal = maxtotal + Convert.ToInt32(maxmark);
    //                        }
    //                    }
    //                }
    //                int count = 0;
    //                count = data.Rows.Count;
    //                Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
    //                table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
    //                table2.VisibleHeaders = false;
    //                table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //                table2.Columns[0].SetWidth(100);
    //                table2.Columns[1].SetWidth(100);
    //                table2.Columns[2].SetWidth(100);
    //                table2.Columns[3].SetWidth(100);
    //                table2.Columns[4].SetWidth(100);
    //                table2.Columns[5].SetWidth(100);
    //                table2.Columns[6].SetWidth(100);
    //                table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
    //                table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table2.Cell(0, 0).SetContent("Subjects");

    //                if (markgrade.Trim() == "Mark")
    //                {
    //                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 1).SetContent("Mark");
    //                }
    //                if (markgrade.Trim() == "Grade")
    //                {
    //                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 1).SetContent("Grade");
    //                }

    //                table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table2.Cell(0, 2).SetContent("Month");
    //                table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table2.Cell(0, 3).SetContent("Year");
    //                table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table2.Cell(0, 4).SetContent("Register No");
    //                table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table2.Cell(0, 5).SetContent("No.of Attempts");
    //                table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table2.Cell(0, 6).SetContent("Maximun Marks");

    //                for (int add = 0; add < data.Rows.Count; add++)
    //                {

    //                    table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                    table2.Cell(add + 1, 0).SetContent(Convert.ToString(data.Rows[add]["Subject"]));


    //                    table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(add + 1, 1).SetContent(Convert.ToString(data.Rows[add]["Marks Obtained"]));


    //                    table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(add + 1, 2).SetContent(Convert.ToString(data.Rows[add]["Month"]));
    //                    // Month.First().ToString().ToUpper() + Month.Substring(1)

    //                    table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(add + 1, 3).SetContent(Convert.ToString(data.Rows[add]["Year"]));


    //                    table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(add + 1, 4).SetContent(Convert.ToString(data.Rows[add]["Register No / Roll No"]));


    //                    table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(add + 1, 5).SetContent(Convert.ToString(data.Rows[add]["No of Attempts"]));

    //                    table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(add + 1, 6).SetContent(Convert.ToString(data.Rows[add]["Maximum Marks"]));


    //                }

    //                Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 550, 550, 550));
    //                mypage1.Add(myprov_pdfpage1);
    //                if (Convert.ToString(markgrade).Trim() == "Mark")
    //                {

    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, 40, y + 650, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total Marks Obtained :  " + Convert.ToString(totalmark));
    //                    mypage1.Add(ptc);
    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                            new PdfArea(mydoc, 250, y + 650, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Maximum Marks :  " + Convert.ToString(maxtotal));
    //                    mypage1.Add(ptc);
    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                            new PdfArea(mydoc, 480, y + 650, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Percentage :  " + Convert.ToString(percentage));
    //                    mypage1.Add(ptc);
    //                }
    //            }
    //            if (ddledu.SelectedItem.Text.ToUpper() == "PG")
    //            {
    //                string qualifyingexam = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);

    //                if (qualifyingexam.Trim() != "")
    //                {
    //                    qualifyingexam = subjectcode(qualifyingexam);
    //                }

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                   new PdfArea(mydoc, line1, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Qualifying Examination Passed");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(qualifyingexam));
    //                mypage1.Add(ptc);

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydoc, line1, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name of the College");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]));
    //                mypage1.Add(ptc);


    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydoc, line1, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Location of the College");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["instaddress"]));
    //                mypage1.Add(ptc);

    //                string branchcode = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);

    //                if (branchcode.Trim() != "")
    //                {
    //                    branchcode = subjectcode(branchcode);
    //                }

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydoc, line1, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Mention Major");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(branchcode));
    //                mypage1.Add(ptc);

    //                string typeofmajor = Convert.ToString(ds.Tables[1].Rows[0]["type_major"]);
    //                if (typeofmajor.Trim() == "1")
    //                {
    //                    typeofmajor = "Single";
    //                }
    //                else if (typeofmajor.Trim() == "2")
    //                {
    //                    typeofmajor = "Double";
    //                }
    //                else if (typeofmajor.Trim() == "3")
    //                {
    //                    typeofmajor = "Triple";
    //                }

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                    new PdfArea(mydoc, line1, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Type of Major");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(typeofmajor));
    //                mypage1.Add(ptc);

    //                string typeofsemester = Convert.ToString(ds.Tables[1].Rows[0]["type_semester"]);
    //                if (typeofsemester.Trim() == "True")
    //                {
    //                    typeofsemester = "Semester";
    //                }
    //                else
    //                {
    //                    typeofsemester = "Non Semester";
    //                }

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                   new PdfArea(mydoc, line1, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Type of Semester");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(typeofsemester));
    //                mypage1.Add(ptc);

    //                string mediumofstudy = Convert.ToString(ds.Tables[1].Rows[0]["medium"]);

    //                if (mediumofstudy.Trim() != "")
    //                {
    //                    mediumofstudy = subjectcode(mediumofstudy);
    //                }

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                 new PdfArea(mydoc, line1, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Medium of Study at UG level");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(mediumofstudy));
    //                mypage1.Add(ptc);

    //                string markgrade = Convert.ToString(ds.Tables[1].Rows[0]["isgrade"]);
    //                if (markgrade.Trim() == "False")
    //                {
    //                    markgrade = "Mark";
    //                }
    //                else
    //                {
    //                    markgrade = "Grade";
    //                }

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 450, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Marks/Grade");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 450, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(markgrade));
    //                mypage1.Add(ptc);

    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                  new PdfArea(mydoc, line1, y + 470, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Registration No as Mentioned on your Mark Sheet");
    //                mypage1.Add(ptc);
    //                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydoc, line2, y + 470, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["registration_no"]));
    //                mypage1.Add(ptc);

    //                string majorpercentage = Convert.ToString(ds.Tables[1].Rows[0]["major_percent"]);
    //                string majoralliedpercentage = Convert.ToString(ds.Tables[1].Rows[0]["majorallied_percent"]);
    //                string majoralliedpracticalspercentage = Convert.ToString(ds.Tables[1].Rows[0]["percentage"]);


    //                DataTable data = new DataTable();
    //                DataRow dr = null;
    //                Hashtable hash = new Hashtable();
    //                int count = 0;
    //                string pgquery = "select psubjectno,subject_typeno,acual_marks,max_marks,pass_month,pass_year,semyear ,grade  from perv_marks_history where course_entno ='" + Convert.ToString(ds.Tables[1].Rows[0]["course_entno"]) + "'";
    //                ds.Clear();
    //                ds = dt.select_method_wo_parameter(pgquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {

    //                    data.Columns.Add("Sem", typeof(string));
    //                    //  data.Columns.Add("Sem/Year", typeof(string));
    //                    data.Columns.Add("Subject", typeof(string));
    //                    data.Columns.Add("Subject type", typeof(string));
    //                    data.Columns.Add("Marks", typeof(string));
    //                    data.Columns.Add("Month", typeof(string));
    //                    data.Columns.Add("Year", typeof(string));
    //                    data.Columns.Add("Maximum Marks", typeof(string));
    //                    int sno = 0;
    //                    for (int pg = 0; pg < ds.Tables[0].Rows.Count; pg++)
    //                    {
    //                        sno++;
    //                        string semyear = Convert.ToString(ds.Tables[0].Rows[pg]["semyear"]);
    //                        string subjectno = Convert.ToString(ds.Tables[0].Rows[pg]["psubjectno"]);
    //                        string subjecttypeno = Convert.ToString(ds.Tables[0].Rows[pg]["subject_typeno"]);
    //                        string actualmark = "";
    //                        if (markgrade.Trim() == "Mark")
    //                        {
    //                            actualmark = Convert.ToString(ds.Tables[0].Rows[pg]["acual_marks"]);
    //                        }
    //                        else if (markgrade.Trim() == "Grade")
    //                        {
    //                            actualmark = Convert.ToString(ds.Tables[0].Rows[pg]["grade"]);
    //                        }
    //                        string month = Convert.ToString(ds.Tables[0].Rows[pg]["pass_month"]);
    //                        string year = Convert.ToString(ds.Tables[0].Rows[pg]["pass_year"]);
    //                        // string noofattenm = Convert.ToString(ds.Tables[0].Rows[pg]["noofattempt"]);
    //                        string maxmark = Convert.ToString(ds.Tables[0].Rows[pg]["max_marks"]);
    //                        dr = data.NewRow();
    //                        dr[0] = Convert.ToString(semyear);
    //                        string subject = subjectcode(subjectno);
    //                        dr[1] = Convert.ToString(subject);
    //                        string typesub = subjectcode(subjecttypeno);
    //                        dr[2] = Convert.ToString(typesub);
    //                        dr[3] = Convert.ToString(actualmark);
    //                        dr[4] = Convert.ToString(month);
    //                        dr[5] = Convert.ToString(year);
    //                        dr[6] = Convert.ToString(maxmark);
    //                        data.Rows.Add(dr);
    //                    }
    //                }
    //                count = data.Rows.Count;
    //                if (count < 8)
    //                {
    //                    Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
    //                    table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
    //                    table2.VisibleHeaders = false;
    //                    table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //                    table2.Columns[0].SetWidth(100);
    //                    table2.Columns[1].SetWidth(100);
    //                    table2.Columns[2].SetWidth(100);
    //                    table2.Columns[3].SetWidth(100);
    //                    table2.Columns[4].SetWidth(100);
    //                    table2.Columns[5].SetWidth(100);
    //                    table2.Columns[6].SetWidth(100);
    //                    table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
    //                    table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 0).SetContent("Sem/Year");

    //                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 1).SetContent("Subject");

    //                    table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 2).SetContent("Type of Subject");
    //                    if (markgrade.Trim() == "Mark")
    //                    {
    //                        table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(0, 3).SetContent("Mark");
    //                    }
    //                    if (markgrade.Trim() == "Grade")
    //                    {
    //                        table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(0, 3).SetContent("Grade");
    //                    }
    //                    table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 4).SetContent("Month");
    //                    table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 5).SetContent("Year");
    //                    table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 6).SetContent("Maximun Marks");


    //                    for (int add = 0; add < data.Rows.Count; add++)
    //                    {
    //                        table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table2.Cell(add + 1, 0).SetContent(Convert.ToString(data.Rows[add]["Sem"]));


    //                        table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 1).SetContent(Convert.ToString(Convert.ToString(data.Rows[add]["Subject"])));


    //                        table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 2).SetContent(Convert.ToString(data.Rows[add]["Subject type"]));
    //                        // Month.First().ToString().ToUpper() + Month.Substring(1)

    //                        table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 3).SetContent(Convert.ToString(data.Rows[add]["Marks"]));


    //                        table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 4).SetContent(Convert.ToString(data.Rows[add]["Month"]));


    //                        table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 5).SetContent(Convert.ToString(data.Rows[add]["Year"]));

    //                        table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 6).SetContent(Convert.ToString(data.Rows[add]["Maximum Marks"]));


    //                    }

    //                    Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 600, 550, 550));
    //                    mypage1.Add(myprov_pdfpage1);
    //                    if (markgrade.Trim() == "Mark")
    //                    {
    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                new PdfArea(mydoc, line1, 750, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage of marks in all subjects (Language/major/Allied/Ancillary/Elective inclusive of Theory and Practical  : " + Convert.ToString(majoralliedpracticalspercentage) + "");
    //                        mypage1.Add(ptc);
    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                 new PdfArea(mydoc, line1, 770, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total % of Marks in Major subjects alone (Including theory & Practicals)  : " + Convert.ToString(majorpercentage) + "");
    //                        mypage1.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, 790, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals  : " + Convert.ToString(majoralliedpercentage) + "");
    //                        mypage1.Add(ptc);
    //                    }
    //                }
    //                else
    //                {
    //                    dummyflage = true;
    //                    Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
    //                    table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
    //                    table2.VisibleHeaders = false;
    //                    table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //                    table2.Columns[0].SetWidth(100);
    //                    table2.Columns[1].SetWidth(100);
    //                    table2.Columns[2].SetWidth(100);
    //                    table2.Columns[3].SetWidth(100);
    //                    table2.Columns[4].SetWidth(100);
    //                    table2.Columns[5].SetWidth(100);
    //                    table2.Columns[6].SetWidth(100);
    //                    table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
    //                    table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 0).SetContent("Sem/Year");

    //                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 1).SetContent("Subject");

    //                    table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 2).SetContent("Type of Subject");
    //                    if (markgrade.Trim() == "Mark")
    //                    {
    //                        table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(0, 3).SetContent("Mark");
    //                    }
    //                    if (markgrade.Trim() == "Grade")
    //                    {
    //                        table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(0, 3).SetContent("Grade");
    //                    }
    //                    table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 4).SetContent("Month");
    //                    table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 5).SetContent("Year");
    //                    table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table2.Cell(0, 6).SetContent("Maximun Marks");


    //                    for (int add = 0; add < data.Rows.Count; add++)
    //                    {

    //                        table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table2.Cell(add + 1, 0).SetContent(Convert.ToString(data.Rows[add]["Sem"]));


    //                        table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 1).SetContent(Convert.ToString(Convert.ToString(data.Rows[add]["Subject"])));


    //                        table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 2).SetContent(Convert.ToString(data.Rows[add]["Subject type"]));
    //                        // Month.First().ToString().ToUpper() + Month.Substring(1)

    //                        table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 3).SetContent(Convert.ToString(data.Rows[add]["Marks"]));


    //                        table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 4).SetContent(Convert.ToString(data.Rows[add]["Month"]));


    //                        table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 5).SetContent(Convert.ToString(data.Rows[add]["Year"]));

    //                        table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(add + 1, 6).SetContent(Convert.ToString(data.Rows[add]["Maximum Marks"]));

    //                    }


    //                    Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 40, 550, 700));
    //                    mypage2.Add(myprov_pdfpage1);
    //                    if (markgrade.Trim() == "Mark")
    //                    {
    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                new PdfArea(mydoc, line1, 750, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage of marks in all subjects (Language/major/Allied/Ancillary/Elective inclusive of Theory and Practical  : " + Convert.ToString(majoralliedpracticalspercentage) + "");
    //                        mypage2.Add(ptc);
    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                 new PdfArea(mydoc, line1, 770, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total % of Marks in Major subjects alone (Including theory & Practicals)  : " + Convert.ToString(majorpercentage) + "");
    //                        mypage2.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydoc, line1, 790, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals  : " + Convert.ToString(majoralliedpercentage) + "");
    //                        mypage2.Add(ptc);
    //                    }

    //                }


    //            }
    //            mypage.SaveToDocument();
    //            mypage1.SaveToDocument();
    //            if (dummyflage == true)
    //            {
    //                mypage2.SaveToDocument();
    //            }

    //            string appPath = HttpContext.Current.Server.MapPath("~");
    //            if (appPath != "")
    //            {

    //                string szPath = appPath + "/Report/";
    //                string szFile = "Application.pdf";

    //                mydoc.SaveToFile(szPath + szFile);
    //                Response.ClearHeaders();
    //                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
    //                Response.ContentType = "application/pdf";
    //                Response.WriteFile(szPath + szFile);


    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}

    protected void btnadmitprint_click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt_date = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
            int ik = 1;
            while (ik <= 2)
            {
                dt_date = dt_date.AddDays(1);
                if (dt_date.ToString("dddd") == "Sunday")
                {
                    dt_date = dt_date.AddDays(1);
                }
                ik++;
            }
            string degreedetails = "";
            if (cbsports.Checked == false)
            {
                degreedetails = "PQ list";
            }
            if (cbsports.Checked == true)
            {
                degreedetails = "Sports Quota / NRI / Foreign list";
            }
            // string degreedetails = "Office of the Controller of Examinations $Passing Board Report For Examination  - " + ddlmonth.SelectedItem.ToString() + "-" + ddlyear.SelectedItem.ToString() + " " + rename + "@" + sthe1 + sthe2 + "@" + sthe3 + sthe4 + sthe5 + "";
            // string degreedetails = "Department of " + ddldept.SelectedItem.Text + " $Selection list for admission 2015 - 16" + '@' + "Stream:   " + ddltype.SelectedItem.Text + "" + '@' + "Education Level:   " + ddledu.SelectedItem.Text + "" + '@' + "Course:   " + ddldegree.SelectedItem.Text + "" + '@' + "Date: " + System.DateTime.Now.ToString("dd/MM/yyyy") + "                                                                                                               Last Date To Fee Paid: " + dt_date.ToString("dd/MM/yyyy") + "";
            string pagename = "PQ.aspx";
            Printcontrol.loadspreaddetails(FpSpread3, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }
    }
    protected void btn_confirm_clcik(object sender, EventArgs e)
    {
        try
        {
            admit();
            loadprint();
        }
        catch
        {

        }
    }
}
