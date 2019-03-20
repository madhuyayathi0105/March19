using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;


public partial class Timetablenew : System.Web.UI.Page
{
    string Master1 = "";

    SqlCommand cmd;
    int sn0 = 0;
    int year = 0;
    static int flag = 0;
    static int rowcount = 0;
    Boolean cellclick = false;
    Boolean flag_true = false;
    string strdayflag;
    string regularflag;
    string Stud_Type = "";
    string Stud_Type1 = "";
    string CollegeCode;
    Hashtable hash = new Hashtable();
    Hashtable hashsubject = new Hashtable();
    Hashtable hashsemester = new Hashtable();
    Hashtable hashyear = new Hashtable();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    [Serializable()]
    public class MyImg : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(75);
            return img;


        }
    }
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = individualsubstud.FindControl("Update");
        Control cntCancelBtn = individualsubstud.FindControl("Cancel");
        Control cntCopyBtn = individualsubstud.FindControl("Copy");
        Control cntCutBtn = individualsubstud.FindControl("Clear");
        Control cntPasteBtn = individualsubstud.FindControl("Paste");
        //Control cntPageNextBtn = FpSpread1.FindControl("Next");
        //Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        Control cntPagePrintBtn = individualsubstud.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);

            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePreviousBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePrintBtn.Parent;
            //tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

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

            if (!IsPostBack)
            {
                if (flag == 0)
                {
                    flag = 1;
                    CollegeCode = Session["CollegeCode"].ToString();

                    Session["Stud_Type1"] = "0";
                    Session["Stud_Type"] = "0";
                    Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                    setcon.Close();
                    setcon.Open();
                    SqlDataReader mtrdr;
                    Session["Rollflag"] = "0";
                    Session["Regflag"] = "0";
                    Session["Studflag"] = "0";
                    SqlCommand mtcmd = new SqlCommand(Master1, setcon);
                    mtrdr = mtcmd.ExecuteReader();
                    {
                        if (mtrdr.HasRows)
                        {
                            while (mtrdr.Read())
                            {
                                if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                                {
                                    Session["Rollflag"] = "1";
                                }
                                if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                                {
                                    Session["Regflag"] = "1";
                                }
                                if (mtrdr["settings"].ToString() == "Student_Type" && mtrdr["value"].ToString() == "1")
                                {
                                    Session["Studflag"] = "1";
                                }
                                if (mtrdr["settings"].ToString() == "Day Scholar" && mtrdr["value"].ToString() == "1")
                                {
                                    Session["Stud_Type"] = "Day Scholar";

                                }
                                if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                                {
                                    Session["Stud_Type1"] = "Hostler";

                                }



                            }
                        }
                    }
                    //individualsubstud.Sheets[0].AutoPostBack = true;
                    HAllSpread.Sheets[0].RowHeader.Visible = false;
                    HAllSpread.Sheets[0].ColumnCount = 10;
                    HAllSpread.Sheets[0].Columns[0].Locked = true;
                    HAllSpread.Sheets[0].Columns[1].Locked = true;
                    HAllSpread.Sheets[0].Columns[2].Locked = true;
                    HAllSpread.Sheets[0].Columns[3].Locked = true;
                    HAllSpread.Sheets[0].Columns[4].Locked = true;
                    HAllSpread.Sheets[0].Columns[5].Locked = true;
                    HAllSpread.Sheets[0].Columns[6].Locked = true;
                    HAllSpread.Sheets[0].Columns[7].Locked = true;
                    HAllSpread.Sheets[0].Columns[8].Locked = true;

                    HAllSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    HAllSpread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    HAllSpread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    HAllSpread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                    HAllSpread.Sheets[0].RowCount = 0;
                    HAllSpread.Sheets[0].Columns[0].Width = 40;
                    HAllSpread.Sheets[0].Columns[1].Width = 40;
                    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                    HAllSpread.Sheets[0].Columns[9].CellType = chkcell;
                    HAllSpread.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                    HAllSpread.Sheets[0].RowCount = HAllSpread.Sheets[0].RowCount + 1;
                    HAllSpread.Sheets[0].SpanModel.Add(HAllSpread.Sheets[0].RowCount - 1, 0, 1, 6);
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 9].CellType = chkcell;
                    HAllSpread.Sheets[0].Cells[HAllSpread.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                    HAllSpread.Sheets[0].FrozenRowCount = 1;
                    chkcell.AutoPostBack = true;
                    HAllSpread.Sheets[0].SheetCorner.RowCount = 2;
                    HAllSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    HAllSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    HAllSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                    HAllSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                    HAllSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                    HAllSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                    HAllSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                    HAllSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 3);
                    HAllSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    HAllSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
                    HAllSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                    HAllSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Course";
                    HAllSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sem";
                    HAllSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subjects";
                    HAllSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Students";
                    HAllSpread.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Regular";
                    HAllSpread.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Arrear";
                    HAllSpread.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Total";
                    HAllSpread.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Select";
                    HAllSpread.Sheets[0].Columns[0].Width = 60;
                    HAllSpread.Sheets[0].Columns[1].Width = 60;
                    HAllSpread.Sheets[0].Columns[2].Width = 80;
                    HAllSpread.Sheets[0].Columns[3].Width = 150;
                    HAllSpread.Sheets[0].Columns[4].Width = 60;
                    HAllSpread.Sheets[0].Columns[5].Width = 150;
                    HAllSpread.Sheets[0].Columns[6].Width = 100;
                    HAllSpread.Sheets[0].Columns[7].Width = 100;
                    HAllSpread.Sheets[0].Columns[8].Width = 100;
                    HAllSpread.Sheets[0].Columns[9].Width = 60;

                    HAllSpread.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                    HAllSpread.Sheets[0].Columns[5].Font.Underline = true;
                    HAllSpread.Sheets[0].Columns[5].ForeColor = Color.Blue;
                    HAllSpread.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                    HAllSpread.Sheets[0].AutoPostBack = false;
                    HAllSpread.CommandBar.Visible = false;

                    SqlCommand cmd = new SqlCommand("ProcExamTimeTableSelectSubjectData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet examds = new DataSet();
                    da.Fill(examds);
                    HAllSpread.Sheets[0].RowCount = 2;
                    int count;
                    int countp;
                    int countA;
                    int countAP;
                    int Sno = 0;
                    string Temp = "";
                    string Arear = "0";
                    string Regular = "0";
                    if (examds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < examds.Tables[0].Rows.Count; i++)
                        {

                            SqlCommand cd = new SqlCommand("ProcExamTimeTableCount", con);
                            cd.CommandType = CommandType.StoredProcedure;
                            cd.Parameters.AddWithValue("@DegreeCode", examds.Tables[0].Rows[i]["DegreeCode"].ToString());
                            cd.Parameters.AddWithValue("@BatchYear", examds.Tables[0].Rows[i]["BatchYear"].ToString());
                            cd.Parameters.AddWithValue("@Semester", examds.Tables[0].Rows[i]["Semester"].ToString());
                            cd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
                            SqlDataAdapter da1 = new SqlDataAdapter(cd);
                            DataSet ds1 = new DataSet();
                            da1.Fill(ds1);
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                {
                                    if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
                                    {
                                        Sno = Sno + 1;
                                    }
                                    count = HAllSpread.Sheets[0].RowCount - 1;
                                    HAllSpread.Sheets[0].Cells[count, 0].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[count, 4].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[count, 6].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[count, 7].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[count, 8].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[count, 0].Text = Sno.ToString();
                                    HAllSpread.Sheets[0].Cells[count, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
                                    HAllSpread.Sheets[0].Cells[count, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
                                    HAllSpread.Sheets[0].Cells[count, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
                                    HAllSpread.Sheets[0].Cells[count, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
                                    HAllSpread.Sheets[0].Cells[count, 3].Note = examds.Tables[0].Rows[i]["deptacronym"].ToString();
                                    HAllSpread.Sheets[0].Cells[count, 4].Text = examds.Tables[0].Rows[i]["Semester"].ToString();
                                    HAllSpread.Sheets[0].Cells[count, 5].Text = ds1.Tables[0].Rows[j]["SubjectName"].ToString();
                                    HAllSpread.Sheets[0].Cells[count, 5].Note = ds1.Tables[0].Rows[j]["SubjectNo"].ToString();
                                    HAllSpread.Sheets[0].Cells[count, 5].Tag = 0;
                                    HAllSpread.Sheets[0].Rows[count].ForeColor = Color.Blue;
                                    HAllSpread.Sheets[0].Cells[count, 6].Text = ds1.Tables[0].Rows[j]["RegularTheoryCount"].ToString();
                                    HAllSpread.Sheets[0].Cells[count, 7].Text = Arear.ToString();
                                    HAllSpread.Sheets[0].Cells[count, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[0].Rows[j]["RegularTheoryCount"].ToString()) + Convert.ToInt16(Arear.ToString()));
                                    HAllSpread.Sheets[0].RowCount++;

                                    Temp = examds.Tables[0].Rows[i]["Department"].ToString();
                                }
                            }

                            if (ds1.Tables[1].Rows.Count > 0)
                            {
                                for (int jp = 0; jp < ds1.Tables[1].Rows.Count; jp++)
                                {
                                    if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
                                    {
                                        Sno = Sno + 1;
                                    }
                                    countp = HAllSpread.Sheets[0].RowCount - 1;
                                    HAllSpread.Sheets[0].Cells[countp, 0].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countp, 4].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countp, 6].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countp, 7].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countp, 8].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countp, 0].Text = Sno.ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 3].Note = examds.Tables[0].Rows[i]["deptacronym"].ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 4].Text = examds.Tables[0].Rows[i]["Semester"].ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 5].Text = ds1.Tables[1].Rows[jp]["SubjectName"].ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 5].Note = ds1.Tables[1].Rows[jp]["SubjectNo"].ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 5].Tag = 0;
                                    HAllSpread.Sheets[0].Rows[countp].ForeColor = Color.BlueViolet;
                                    HAllSpread.Sheets[0].Cells[countp, 6].Text = ds1.Tables[1].Rows[jp]["RegularPracticalCount"].ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 7].Text = Arear.ToString();
                                    HAllSpread.Sheets[0].Cells[countp, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[1].Rows[jp]["RegularPracticalCount"].ToString()) + Convert.ToInt16(Arear.ToString()));
                                    HAllSpread.Sheets[0].RowCount++;

                                    Temp = examds.Tables[0].Rows[i]["Department"].ToString();
                                }
                            }


                            if (ds1.Tables[2].Rows.Count > 0)
                            {
                                for (int k = 0; k < ds1.Tables[2].Rows.Count; k++)
                                {
                                    if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
                                    {
                                        Sno = Sno + 1;
                                    }
                                    countA = HAllSpread.Sheets[0].RowCount - 1;
                                    HAllSpread.Sheets[0].Cells[countA, 0].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countA, 4].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countA, 6].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countA, 7].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countA, 8].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countA, 0].Text = Sno.ToString();

                                    HAllSpread.Sheets[0].Cells[countA, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
                                    HAllSpread.Sheets[0].Cells[countA, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
                                    HAllSpread.Sheets[0].Cells[countA, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
                                    HAllSpread.Sheets[0].Cells[countA, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
                                    HAllSpread.Sheets[0].Cells[countA, 3].Note = examds.Tables[0].Rows[i]["deptacronym"].ToString();
                                    HAllSpread.Sheets[0].Cells[countA, 4].Text = ds1.Tables[2].Rows[k]["semester"].ToString();
                                    HAllSpread.Sheets[0].Cells[countA, 5].Text = ds1.Tables[2].Rows[k]["SubjectName"].ToString();
                                    HAllSpread.Sheets[0].Cells[countA, 5].Note = ds1.Tables[2].Rows[k]["SubjectNo"].ToString();
                                    HAllSpread.Sheets[0].Cells[countA, 5].Tag = 1;
                                    HAllSpread.Sheets[0].Rows[countA].ForeColor = Color.Orange;
                                    HAllSpread.Sheets[0].Cells[countA, 6].Text = Regular.ToString();
                                    HAllSpread.Sheets[0].Cells[countA, 7].Text = ds1.Tables[2].Rows[k]["ArearTheoryCount"].ToString();
                                    HAllSpread.Sheets[0].Cells[countA, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[2].Rows[k]["ArearTheoryCount"].ToString()) + Convert.ToInt16(Regular.ToString()));
                                    HAllSpread.Sheets[0].RowCount++;

                                    Temp = examds.Tables[0].Rows[i]["Department"].ToString();


                                }
                            }
                            if (ds1.Tables[3].Rows.Count > 0)
                            {

                                for (int kp = 0; kp < ds1.Tables[3].Rows.Count; kp++)
                                {
                                    if (Temp != examds.Tables[0].Rows[i]["Department"].ToString())
                                    {
                                        Sno = Sno + 1;
                                    }
                                    countAP = HAllSpread.Sheets[0].RowCount - 1;
                                    HAllSpread.Sheets[0].Cells[countAP, 0].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countAP, 4].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countAP, 6].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countAP, 7].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countAP, 8].HorizontalAlign = HorizontalAlign.Center;
                                    HAllSpread.Sheets[0].Cells[countAP, 0].Text = Sno.ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 1].Text = examds.Tables[0].Rows[i]["Year"].ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 1].Note = examds.Tables[0].Rows[i]["BatchYear"].ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 2].Text = examds.Tables[0].Rows[i]["Course"].ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 3].Text = examds.Tables[0].Rows[i]["Department"].ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 3].Note = examds.Tables[0].Rows[i]["deptacronym"].ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 4].Text = ds1.Tables[3].Rows[kp]["Semester"].ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 5].Text = ds1.Tables[3].Rows[kp]["SubjectName"].ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 5].Note = ds1.Tables[3].Rows[kp]["SubjectNo"].ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 5].Tag = 1;
                                    HAllSpread.Sheets[0].Rows[countAP].ForeColor = Color.OrangeRed;
                                    HAllSpread.Sheets[0].Cells[countAP, 6].Text = Regular.ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 7].Text = ds1.Tables[3].Rows[kp]["ArearPracticalCount"].ToString();
                                    HAllSpread.Sheets[0].Cells[countAP, 8].Text = Convert.ToString(Convert.ToInt16(ds1.Tables[3].Rows[kp]["ArearPracticalCount"].ToString()) + Convert.ToInt16(Regular.ToString()));
                                    HAllSpread.Sheets[0].RowCount++;

                                    Temp = examds.Tables[0].Rows[i]["Department"].ToString();


                                }


                            }

                        }


                        HAllSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        HAllSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        HAllSpread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        HAllSpread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        rowcount = HAllSpread.Sheets[0].RowCount * 20;

                    }
                } HAllSpread.Sheets[0].RowCount--;
                HAllSpread.Sheets[0].PageSize = rowcount;
                HAllSpread.Height = rowcount;
            }
        }
        catch
        {
        }
    }
    protected void HAllSpread_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(HAllSpread.Sheets[0].RowCount); j++)
            {
                string actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "System.Object")
                    HAllSpread.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
            }
            flag_true = true;
        }

    }
    protected void HAllSpread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellclick = true;
    }
    protected void HAllSpread_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {


                //============


            }
        }
        catch
        {

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        individualsubstud.Sheets[0].ColumnCount = 6;
        individualsubstud.Sheets[0].Columns[0].Locked = true;
        individualsubstud.Sheets[0].Columns[1].Locked = true;
        individualsubstud.Sheets[0].Columns[2].Locked = true;
        individualsubstud.Sheets[0].Columns[3].Locked = true;
        individualsubstud.Sheets[0].Columns[4].Locked = true;
        individualsubstud.Sheets[0].Columns[5].Locked = true;
        individualsubstud.Visible = false;
        individualsubstud.Sheets[0].SheetCorner.RowCount = 11;
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(6, 1, 1, 5);
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(7, 1, 1, 5);
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(8, 1, 1, 5);
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(9, 1, 1, 5);
        individualsubstud.Sheets[0].Columns[0].Width = 70;
        individualsubstud.Sheets[0].Columns[1].Width = 100;
        individualsubstud.Sheets[0].Columns[2].Width = 150;
        individualsubstud.Sheets[0].Columns[3].Width = 100;
        individualsubstud.Sheets[0].Columns[4].Width = 170;
        individualsubstud.Sheets[0].Columns[5].Width = 70;
        individualsubstud.Sheets[0].AutoPostBack = true;
        //if (Session["Rollflag"] == "0")
        //{
        //    individualsubstud.Width = 700;
        //    individualsubstud.Sheets[0].Columns[1].Visible = false;

        //}
        if (Session["Regflag"] == "0")
        {
            individualsubstud.Width = 510;
            individualsubstud.Sheets[0].Columns[2].Visible = false;

        }
        if (Session["Studflag"].ToString() == "0")
        {
            individualsubstud.Width = 570;
            individualsubstud.Sheets[0].Columns[3].Visible = false;

        }
        if (Session["Regflag"].ToString() == "0" && Session["Rollflag"].ToString() == "0" && Session["Studflag"].ToString() == "0")
        {
            individualsubstud.Width = 510;
            individualsubstud.Sheets[0].Columns[1].Width = 200;
            individualsubstud.Sheets[0].Columns[1].Visible = true;
        }

        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        individualsubstud.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        individualsubstud.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        individualsubstud.Sheets[0].AllowTableCorner = true;
        individualsubstud.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";
        MyImg mi = new MyImg();
        mi.ImageUrl = "~/images/10BIT001.jpeg";
        mi.ImageUrl = "Handler/Handler2.ashx?";
        MyImg mi1 = new MyImg();
        mi1.ImageUrl = "~/images/10BIT001.jpeg";
        mi1.ImageUrl = "Handler/Handler5.ashx?";
        string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
        con1.Close();
        con1.Open();
        SqlCommand comm = new SqlCommand(str, con1);
        SqlDataReader drr = comm.ExecuteReader();
        drr.Read();
        string coll_name = Convert.ToString(drr["collname"]);
        string coll_address1 = Convert.ToString(drr["address1"]);
        string coll_address2 = Convert.ToString(drr["address2"]);
        string coll_address3 = Convert.ToString(drr["address3"]);
        string pin_code = Convert.ToString(drr["pincode"]);
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 1);
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(0, individualsubstud.Sheets[0].ColumnCount - 1, 1, 6);

        individualsubstud.Sheets[0].ColumnHeader.Cells[0, 1].Text = coll_name;
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, individualsubstud.Sheets[0].ColumnCount - 2);
        individualsubstud.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        individualsubstud.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;

        individualsubstud.Sheets[0].ColumnHeader.Cells[1, 1].Text = coll_address1;
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, individualsubstud.Sheets[0].ColumnCount - 2);
        individualsubstud.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
        individualsubstud.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

        individualsubstud.Sheets[0].ColumnHeader.Cells[2, 1].Text = coll_address2;
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, individualsubstud.Sheets[0].ColumnCount - 2);
        individualsubstud.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
        individualsubstud.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

        individualsubstud.Sheets[0].ColumnHeader.Cells[3, 1].Text = coll_address3 + "-" + " " + pin_code + ".";
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, individualsubstud.Sheets[0].ColumnCount - 2);
        individualsubstud.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
        individualsubstud.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

        //individualsubstud.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Salary Summary For-" + monname + "--" + cblbatchyear.Text + "";
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, individualsubstud.Sheets[0].ColumnCount - 2);
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, individualsubstud.Sheets[0].ColumnCount - 2);
        individualsubstud.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;
        individualsubstud.Sheets[0].ColumnHeader.Cells[4, 1].ForeColor = Color.FromArgb(64, 64, 255);
        individualsubstud.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;

        individualsubstud.Sheets[0].ColumnHeader.Rows[10].BackColor = Color.FromArgb(214, 235, 255);
        individualsubstud.Sheets[0].ColumnHeader.Rows[10].Font.Bold = true;
        individualsubstud.Sheets[0].ColumnHeader.Rows[10].Font.Size = FontUnit.Medium;
        individualsubstud.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(0, individualsubstud.Sheets[0].ColumnCount - 1, 6, 1);
        individualsubstud.Sheets[0].ColumnHeader.Cells[0, individualsubstud.Sheets[0].ColumnCount - 1].CellType = mi1;
        individualsubstud.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        individualsubstud.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        individualsubstud.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        individualsubstud.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        individualsubstud.Sheets[0].RowCount = 0;

        individualsubstud.Sheets[0].ColumnHeaderSpanModel.Add(10, 4, 1, 2);
        individualsubstud.Sheets[0].ColumnHeader.Rows[10].HorizontalAlign = HorizontalAlign.Center;
        individualsubstud.Sheets[0].ColumnHeader.Cells[10, 0].Text = "S.No";
        individualsubstud.Sheets[0].ColumnHeader.Cells[10, 1].Text = "Roll No";
        individualsubstud.Sheets[0].ColumnHeader.Cells[10, 2].Text = "Reg No";
        individualsubstud.Sheets[0].ColumnHeader.Cells[10, 3].Text = "Student Type";
        individualsubstud.Sheets[0].ColumnHeader.Cells[10, 4].Text = "Student Name";
        individualsubstud.Sheets[0].RowHeader.Visible = false;
        HAllSpread.SaveChanges();




        int sno = 0;
        int count = 0;
        for (int res = 1; res <= Convert.ToInt32(HAllSpread.Sheets[0].RowCount) - 1; res++)
        {

            int isval = 0;
            string s = HAllSpread.Sheets[0].Cells[res, 9].Text;

            isval = Convert.ToInt32(HAllSpread.Sheets[0].Cells[res, 9].Value);
            if (isval == 1)
            {
                string year1 = "";
                string semester1 = "";
                string degreecourse1 = "";
                string subject1 = "";
                string subject_no = HAllSpread.Sheets[0].Cells[res, 5].Note;
                int semester = Convert.ToInt32(HAllSpread.Sheets[0].Cells[res, 4].Text);

                //individualsubstud.Sheets[0].AutoPostBack = true;
                individualsubstud.CommandBar.Visible = true;
                //string activerow = "";
                //string activecol = "";
                //activerow = HAllSpread.ActiveSheetView.ActiveRow.ToString();
                //activecol = HAllSpread.ActiveSheetView.ActiveColumn.ToString();
                //string subject_no = HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Note;
                //int semester = Convert.ToInt32(HAllSpread.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);

                string year = HAllSpread.Sheets[0].Cells[res, 1].Text;
                string degree = HAllSpread.Sheets[0].Cells[res, 2].Text;
                string course = HAllSpread.Sheets[0].Cells[res, 3].Text;
                string courseacro = HAllSpread.Sheets[0].Cells[res, 3].Note;
                string subject = HAllSpread.Sheets[0].Cells[res, 5].Text;
                string arreartag = Convert.ToString(HAllSpread.Sheets[0].Cells[res, 5].Tag);
                string degreecourse = degree + "-" + courseacro;
                individualsubstud.Sheets[0].ColumnHeader.Cells[6, 0].Text = "Year";
                individualsubstud.Sheets[0].ColumnHeader.Cells[7, 0].Text = "Branch";
                individualsubstud.Sheets[0].ColumnHeader.Cells[8, 0].Text = "Semester";
                individualsubstud.Sheets[0].ColumnHeader.Cells[9, 0].Text = "Subject";
                if (hashyear.Contains(year))
                {
                }
                else
                {
                    hashyear.Add(year, count);
                    count++;
                }
                foreach (DictionaryEntry parameter in hashyear)
                {
                    string selectyear = Convert.ToString(parameter.Key);
                    if (year1 == "")
                    {
                        year1 = selectyear;
                    }
                    else if (year1 != "")
                    {
                        if (year1 != year)
                        {
                            year1 = year1 + "," + " " + selectyear;
                        }
                    }
                }


                if (hash.Contains(degreecourse))
                {
                }
                else
                {
                    hash.Add(degreecourse, count);
                    count++;
                }
                foreach (DictionaryEntry parameter in hash)
                {
                    string selectdegreecourse = Convert.ToString(parameter.Key);
                    if (degreecourse1 == "")
                    {
                        degreecourse1 = selectdegreecourse;
                    }
                    else if (degreecourse1 != "")
                    {
                        if (degreecourse1 != degreecourse)
                        {
                            degreecourse1 = degreecourse1 + "," + " " + selectdegreecourse;
                        }
                    }


                }
                if (hashsubject.Contains(subject))
                {
                }
                else
                {
                    hashsubject.Add(subject, count);
                    count++;
                }
                foreach (DictionaryEntry parameter in hashsubject)
                {
                    string selectsubject = Convert.ToString(parameter.Key);
                    if (subject1 == "")
                    {
                        subject1 = selectsubject;
                    }
                    else if (subject1 != "")
                    {

                        if (subject1 != subject)
                        {
                            subject1 = subject1 + "," + " " + selectsubject;
                        }
                    }
                }

                if (hashsemester.Contains(semester))
                {
                }
                else
                {
                    hashsemester.Add(semester, count);
                    count++;
                }
                foreach (DictionaryEntry parameter in hashsemester)
                {
                    string selectsemester = Convert.ToString(parameter.Key);
                    if (semester1 == "")
                    {
                        semester1 = Convert.ToString(selectsemester);
                    }
                    else if (semester1 != "")
                    {

                        if (semester1 != Convert.ToString(semester))
                        {
                            semester1 = semester1 + "," + Convert.ToString(selectsemester);
                        }
                    }
                }

                individualsubstud.Sheets[0].ColumnHeader.Cells[6, 1].Text = ":" + " " + year1;
                individualsubstud.Sheets[0].ColumnHeader.Cells[7, 1].Text = ":" + " " + degreecourse1;
                individualsubstud.Sheets[0].ColumnHeader.Cells[8, 1].Text = ":" + " " + semester1;
                individualsubstud.Sheets[0].ColumnHeader.Cells[9, 1].Text = ":" + " " + subject1;
                individualsubstud.Sheets[0].ColumnHeader.Rows[6].Border.BorderColorBottom = Color.White;
                individualsubstud.Sheets[0].ColumnHeader.Rows[7].Border.BorderColorBottom = Color.White;
                individualsubstud.Sheets[0].ColumnHeader.Rows[8].Border.BorderColorBottom = Color.White;
                individualsubstud.Sheets[0].ColumnHeader.Rows[6].Border.BorderColorRight = Color.White;
                individualsubstud.Sheets[0].ColumnHeader.Rows[7].Border.BorderColorRight = Color.White;
                individualsubstud.Sheets[0].ColumnHeader.Rows[8].Border.BorderColorRight = Color.White;
                individualsubstud.Sheets[0].ColumnHeader.Rows[9].Border.BorderColorRight = Color.White;


                string type = "0";
                int i = 0;
                int j = 0;
                // string type1 = "";
                if (Session["Stud_Type"] != "0" && Session["Stud_Type1"] != "0")
                {
                    j = 2;
                }
                if (Session["Stud_Type"] != "1" && Session["Stud_Type1"] != "1")
                {
                    j = 2;
                }
                if (j != 2)
                {
                    if (Session["Stud_Type1"] != "Hostler")
                    {
                        i = 1;
                        type = Session["Stud_Type1"].ToString();
                    }

                    else if (Session["Stud_Type"] != "Day Scholar")
                    {
                        i = 1;
                        type = Session["Stud_Type"].ToString();
                    }
                }
                if (arreartag == "0")
                {
                    SqlCommand studentscmd = new SqlCommand("procindividualsubstud", con);
                    studentscmd.CommandType = CommandType.StoredProcedure;
                    studentscmd.Parameters.AddWithValue("@subject_no", subject_no);
                    studentscmd.Parameters.AddWithValue("@semester", semester);
                    if (type != "")
                        studentscmd.Parameters.AddWithValue("@stud_type", type);
                    studentscmd.Parameters.AddWithValue("@i", i);
                    //studentscmd.Parameters.AddWithValue("@stud_type1", type1);
                    SqlDataAdapter studentsda = new SqlDataAdapter(studentscmd);
                    DataSet studentsds = new DataSet();
                    studentsda.Fill(studentsds);
                    if (studentsds.Tables[0].Rows.Count > 0)
                    {
                        individualsubstud.Visible = true;
                        for (int dd = 0; dd < studentsds.Tables[0].Rows.Count; dd++)
                        {
                            sno++;
                            individualsubstud.Sheets[0].RowCount = individualsubstud.Sheets[0].RowCount + 1;
                            individualsubstud.Sheets[0].SpanModel.Add(individualsubstud.Sheets[0].RowCount - 1, 4, 1, 2);
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 0].Text = sno + "";
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 1].Text = studentsds.Tables[0].Rows[dd]["roll_no"].ToString();
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 2].Text = studentsds.Tables[0].Rows[dd]["reg_no"].ToString();
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 3].Text = studentsds.Tables[0].Rows[dd]["Stud_type"].ToString();
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 4].Text = studentsds.Tables[0].Rows[dd]["Stud_name"].ToString();
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                if (arreartag == "1")
                {
                    SqlCommand studentscmdforarrear = new SqlCommand("procindividualsubstudforarrear", con);
                    studentscmdforarrear.CommandType = CommandType.StoredProcedure;
                    studentscmdforarrear.Parameters.AddWithValue("@subject_no", subject_no);
                    if (type != "")
                        studentscmdforarrear.Parameters.AddWithValue("@stud_type", type);
                    studentscmdforarrear.Parameters.AddWithValue("@i", i);
                    SqlDataAdapter studentsdaforarrear = new SqlDataAdapter(studentscmdforarrear);
                    DataSet studentsdsforarrear = new DataSet();
                    studentsdaforarrear.Fill(studentsdsforarrear);
                    if (studentsdsforarrear.Tables[0].Rows.Count > 0)
                    {
                        individualsubstud.Visible = true;
                        for (int dd2 = 0; dd2 < studentsdsforarrear.Tables[0].Rows.Count; dd2++)
                        {
                            sno++;
                            individualsubstud.Sheets[0].RowCount = individualsubstud.Sheets[0].RowCount + 1;
                            individualsubstud.Sheets[0].SpanModel.Add(individualsubstud.Sheets[0].RowCount - 1, 4, 1, 2);
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 0].Text = sno + "";
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 1].Text = studentsdsforarrear.Tables[0].Rows[dd2]["roll_no"].ToString();
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 2].Text = studentsdsforarrear.Tables[0].Rows[dd2]["reg_no"].ToString();
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 3].Text = studentsdsforarrear.Tables[0].Rows[dd2]["Stud_type"].ToString();
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 4].Text = studentsdsforarrear.Tables[0].Rows[dd2]["Stud_name"].ToString();
                            individualsubstud.Sheets[0].Cells[individualsubstud.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }

                }
                int totalrows = individualsubstud.Sheets[0].RowCount;
                individualsubstud.Sheets[0].PageSize = totalrows * 100;
                individualsubstud.Height = totalrows + 50 * 15;
            }
        }
    }
}