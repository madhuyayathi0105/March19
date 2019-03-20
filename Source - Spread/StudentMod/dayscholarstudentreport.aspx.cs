using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Drawing;
using FarPoint.Web.Spread;
using System.Collections;

public partial class StudentMod_dayscholarstudentreport : System.Web.UI.Page
{
    double percentage;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlDataAdapter da = new SqlDataAdapter();
    SqlCommand cmd = new SqlCommand();
    SqlDataReader dr;
    SqlDataReader drcount25;
    SqlDataReader drcount26;
    SqlDataReader drcount27;
    SqlDataReader drcount28;
    string attendance = "";
    string hostelattend1 = "";
    string hostelattend2 = "";
    string hostelattend3 = "";
    DataSet ds = new DataSet();
    DataSet dsbind = new DataSet();
    DataSet dset = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    string Str;
    string sql;
    int day3;
    bool gracetimeflag = false;
    bool ontimeflag = false;
    bool Generalflag = true;
    int ontime1 = 0;
    string strdate;
    //string strdate;
    string enddate;
    string partdate;
    string date = "";
    // string date2="";
    string date1;
    string dateparti;
    string dateformat;
    string datefrom;
    string dateto;
    string Att_changedate;
    string date2;
    string date3;
    string dateupto;
    string Att_dateformate;
    string Att_changryear;
    string today;
    string datetoday;
    string strTime;
    bool presentclick = false;
    bool absentclick = false;
    int countpresent = 0;
    int countabsent = 0;
    int countpermission = 0;
    int countlate = 0;
    int countpresenteve = 0;
    int countabsenteve = 0;
    int countpresenteve2 = 0;
    int counttotalmornpresent = 0;
    int counttotalevennpresent = 0;
    int counttotalabsentmorn = 0;
    int counttotalabsenteven = 0;
    int totalmornlate = 0;
    int totalevenlate = 0;
    int totalpermorn = 0;
    int totalpereven = 0;
    static int batchcnt = 0;
    double totalperesent = 0;
    double totalabsent = 0;
    int countlateeve = 0;
    double totallate = 0;
    double totalpermission = 0;
    int countpermissioneve = 0;
    int totalcountevennpermission = 0;
    int totallatecount = 0;
    // Att_changryear
    int countpresent2;
    int countabsent2;
    int countpermission2;
    int countlate2;
    double totalpresent;
    double g;
    int counttotalpresenr = 0;
    int counttotalabsent = 0;
    double c;
    int d;
    static string order_by_var = "";
    DAccess2 d2 = new DAccess2();
    [Serializable()]

    public class MyImg : ImageCellType
    {
        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(50);
            return img;
        }
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
            if (!IsPostBack)
            {
                ViewState["unreg"] = null;
                ViewState["Bothpresent"] = null;
                ViewState["BothAbsent"] = null;
                ViewState["Bothod"] = null;
                ViewState["Bothper"] = null;

                Txtentryfrom.Attributes.Add("readonly", "readonly");
                Txtentryto.Attributes.Add("readonly", "readonly");
                order_by_var = "";
                filteration();
                fpbiomatric.Visible = false;
                gracetimeflag = false;
                ontimeflag = false;
                Generalflag = true;
                //Display current Date In the Text Box
                string today = System.DateTime.Now.ToString();
                string today1;
                string[] split13 = today.Split(new char[] { ' ' });
                string[] split14 = split13[0].Split(new Char[] { '/' });
                today1 = split14[1].ToString() + "/" + split14[0].ToString() + "/" + split14[2].ToString();
                Txtentryfrom.Text = today1;
                string today2 = System.DateTime.Now.ToString();
                string today3;
                string[] split15 = today.Split(new char[] { ' ' });
                string[] split16 = split13[0].Split(new Char[] { '/' });
                today3 = split16[1].ToString() + "/" + split16[0].ToString() + "/" + split16[2].ToString();
                Txtentryto.Text = today3;
                //fpbiomatric.Visible = false;
                // load_hostelname();
                bindcollege();
                //ddlBranch.Items.Insert(0, "Select");
                ddlSemYr.Items.Insert(0, "Select");
                ddlSec.Items.Insert(0, "Select");
                cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''order by batch_year", con);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                //ddlBatch.DataSource = ds1;
                //ddlBatch.DataValueField = "batch_year";
                //ddlBatch.DataBind();
                //ddlBatch.Items.Insert(0, "Select");
                cbl_batchyear.DataSource = ds1;
                cbl_batchyear.DataValueField = "batch_year";
                cbl_batchyear.DataBind();
                //course
                con.Open();
                cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + Session["collegecode"] + " order by course.course_name ", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cbl_degree.DataSource = ds;
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataBind();
                //ddlDegree.DataSource = ds;
                //ddlDegree.DataValueField = "course_id";
                //ddlDegree.DataTextField = "course_name";
                //ddlDegree.DataBind();
                //ddlDegree.Items.Insert(0, "Select");
                con.Close();
                cblsearch.Items[0].Selected = true;
                cblsearch.Items[1].Selected = true;
                cblsearch.Items[2].Selected = true;
                //cblsearch.Items[5].Selected = true;
                //cblsearch.Items[6].Selected = true;
                //cblsearch.Items[7].Selected = true;
                fpbiomatric.CommandBar.Visible = false;
                rdb_deptname.Checked = true;
                rdoboth1.Checked = true;
            }
        }
        catch (Exception ex) { }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = fpbiomatric.FindControl("Update");
        Control cntCancelBtn = fpbiomatric.FindControl("Cancel");
        Control cntCopyBtn = fpbiomatric.FindControl("Copy");
        Control cntCutBtn = fpbiomatric.FindControl("Clear");
        Control cntPasteBtn = fpbiomatric.FindControl("Paste");
        Control cntPageNextBtn = fpbiomatric.FindControl("Next");
        Control cntPagePreviousBtn = fpbiomatric.FindControl("Prev");
        Control cntPagePrintBtn = fpbiomatric.FindControl("Print");

        Control cntPagePrintpdfBtn = fpbiomatric.FindControl("PrintPDF");

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

            tc = (TableCell)cntPagePrintpdfBtn.Parent;
            tr.Cells.Remove(tc);
        }
        base.Render(writer);
    }

    protected override void OnLoad(EventArgs e)
    {
        try
        {
            string cacheKey = "BBcacheKey";
            object cache = HttpContext.Current.Cache[cacheKey];
            if (cache == null)
                HttpContext.Current.Cache[cacheKey] = DateTime.UtcNow.ToString();
            Response.AddCacheItemDependency(cacheKey);
        }
        catch (Exception ex)
        {
            throw new SystemException(ex.Message);
        }
        base.OnLoad(e);
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        fpbiomatric.Visible = true;
        fpbiomatric.Visible = true;

        lblnorec.Visible = false;
        imgabsent.Visible = true;
        lblheaderabsent1.Visible = true;
        lblheaderabsent2.Visible = true;
        imgper.Visible = true;
        lblmornper.Visible = true;
        lblevenper.Visible = true;
        lblpermission.Visible = true;
        lblpresent1.Visible = true;
        lblpresent2.Visible = true;
        lbl_headermorn.Visible = true;
        lbl_headereven.Visible = true;
        lbllate.Visible = true;
        lbllate1.Visible = true;
        imglate.Visible = true;
        lblmornlate.Visible = true;
        lblevenlate.Visible = true;
        imgabsent.Visible = true;
        lblheaderabsent1.Visible = true;
        lblheaderabsent2.Visible = true;
        lblabsent1.Visible = true;
        lblabsent2.Visible = true;
        imgpresent.Visible = true;
        // Panel5.Visible = true;
        imglate.Visible = true;
        lbllate.Visible = true;
        imgper.Visible = true;
        lblmornper.Visible = true;
        lblevenper.Visible = true;
        lblpermission.Visible = true;
        lblpermission1.Visible = true;
        //imgontime.Visible = true;
        //lblontime.Visible = true;

        load_click();
    }

    void load_click()
    {
        try
        {
            Fieldset1.Visible = false;
            Fieldset2.Visible = false;
            btnprintmaster.Visible = true;
            lblexcel.Visible = true;
            txtexcel.Visible = true;
            btnexcel.Visible = true;
            txtexcel.Text = "";
            lblerror.Visible = false;
            lblerror.Text = "";
            lblpresent1.Text = ":" + "0";
            lblpresent2.Text = ":" + "0";
            lblabsent1.Text = ":" + "0";
            lblabsent2.Text = ":" + "0";
            //lbllate.Text = ":" + "0";
            //lbllate1.Text = ":" + "0";
            //lblpermission.Text = ":" + "0";
            //lblpermission1.Text = ":" + "0";
            //lblontime.Text = ":" + "0";
            lblnorec.Visible = false;
            countpresent = 0;
            countabsent = 0;
            countlate = 0;
            countpermission = 0;
            ontime1 = 0;
            Hashtable hat = new Hashtable();
            fpbiomatric.Sheets[0].AutoPostBack = true;
            // Panel5.Visible = true;
            string tempstaffcode = "";
            //  CheckBoxselect.Checked = true;
            string dept = "";
            string attmark_CL = "";
            string lbl_leave_msg = "";
            bool flagall = false;
            if (cbo_att.Items[0].Selected == true)
            {
                lbl_leave_msg = "Present Report";
                flagall = true;
            }
            else
            {
                flagall = false;
            }

            if (cbo_att.Items[1].Selected == true)
            {
                lbl_leave_msg = "Absent Report";
                flagall = true;
            }
            else
            {
                flagall = false;
            }

            //if (ddlBatch.SelectedIndex == 0)
            //{
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "Please Select The Batch Year!";
            //    return;
            //}
            //if (ddlDegree.SelectedIndex == 0)
            //{
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "Please Select The Degree!";
            //    return;
            //}
            //if (ddlBranch.SelectedIndex == 0)//delsi
            //{
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "Please Select The Branch!";
            //    return;
            //}


            if (cbo_att.Items.Count != null)
            {
                int itemcount = 0;
                for (itemcount = 0; itemcount < cbo_att.Items.Count; itemcount++)
                {
                    if (cbo_att.Items[itemcount].Selected == true)
                    {
                        if (rdoinonly.Checked == true && rdb_morn.Checked == true)
                        {
                            if (attmark_CL.Trim() == "")
                                attmark_CL = " att like '" + cbo_att.Items[itemcount].Text.Trim().ToString() + "-%'";
                            else
                                attmark_CL = attmark_CL + " or att like '" + cbo_att.Items[itemcount].Text.Trim().ToString() + "-%'";
                        }
                        else if (rdooutonly.Checked == true && rdb_even.Checked == true)
                        {
                            if (attmark_CL.Trim() == "")
                                attmark_CL = " att like '%-" + cbo_att.Items[itemcount].Text.Trim().ToString() + "'";
                            else
                                attmark_CL = attmark_CL + " or att like '%-" + cbo_att.Items[itemcount].Text.Trim().ToString() + "'";
                        }
                        else if (rdoinandout.Checked == true)
                        {
                            if (attmark_CL.Trim() == "")
                                attmark_CL = " (att like '" + cbo_att.Items[itemcount].Text.Trim().ToString() + "-" + cbo_att.Items[itemcount].Text.Trim().ToString() + "')";
                            else
                                attmark_CL = attmark_CL + " or (att like '" + cbo_att.Items[itemcount].Text.Trim().ToString() + "-" + cbo_att.Items[itemcount].Text.Trim().ToString() + "')";
                        }
                    }
                }
            }

            if (attmark_CL.TrimEnd().ToString() != "")
            {
                Str = " and (" + attmark_CL + ")";
            }
            if (rdb_deptname.Checked == true)
            {
                dept = "Dept_Name";
            }
            else if (rdb_deptacr.Checked == true)
            {
                dept = "Dept_acronym";
            }
            if (rdoinandout.Checked == true)
            {
                #region In and Out

                attfiltertype.Visible = true;
                lbllatetext.Visible = false;
                //lbllatetext.Text = "";
                imgabsent.Visible = true;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;
                imgpresent.Visible = true;
                imglate.Visible = false;
                lblmornlate.Visible = false;
                lblevenlate.Visible = false;

                imgper.Visible = false;
                lblmornper.Visible = false;
                lblevenper.Visible = false;
                lblpermission.Visible = false;
                lblpermission1.Visible = false;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lbllate.Visible = false;
                lbllate1.Visible = false;

                //imgontime.Visible = false;
                //lblontime.Visible = false;
                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                //  CheckBoxselect.Visible = true;
                string[] search = new string[50];
                cblsearch.Items[3].Selected = true;
                cblsearch.Items[4].Selected = true;
                cblsearch.Items[5].Selected = true;


                string wsearch = "";
                int count = 0;
                for (int i = 0; i < cblsearch.Items.Count; i++)
                {
                    if (cblsearch.Items[i].Selected == true)
                    {
                        count = count + 1;

                    }
                }

                fpbiomatric.Sheets[0].RowCount = 0;
                fpbiomatric.Sheets[0].ColumnCount = 0;
                // fpbiomatric.CommandBar.Visible = false;
                //  fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].SheetCorner.Columns[0].Visible = false;//delsi2710
                fpbiomatric.Sheets[0].PageSize = 10;
                fpbiomatric.RowHeader.Visible = true;
                fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpbiomatric.Pager.Align = HorizontalAlign.Right;
                fpbiomatric.Pager.Font.Bold = true;
                fpbiomatric.Pager.Font.Name = "Book Antiqua";
                fpbiomatric.Pager.ForeColor = Color.DarkGreen;
                fpbiomatric.Pager.BackColor = Color.AliceBlue;
                fpbiomatric.Pager.PageCount = 5;
                fpbiomatric.Sheets[0].SheetCorner.RowCount = 2;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnCount = 11;

                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                fpbiomatric.Sheets[0].Columns[0].CellType = txtcell;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Room No";
                fpbiomatric.Columns[4].Visible = false;
                fpbiomatric.Columns[5].Visible = false;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Att  %";
                fpbiomatric.Sheets[0].SetColumnWidth(6, 70);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 7].Text = "P";
                fpbiomatric.Sheets[0].SetColumnWidth(7, 30);


                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 8].Text = "A";
                fpbiomatric.Sheets[0].SetColumnWidth(8, 30);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 9].Text = "LA";
                fpbiomatric.Sheets[0].SetColumnWidth(9, 30);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 10].Text = "PER";
                fpbiomatric.Sheets[0].SetColumnWidth(10, 30);


                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);

                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                fpbiomatric.Sheets[0].SetColumnWidth(0, 50);
                fpbiomatric.Sheets[0].SetColumnWidth(1, 200);
                fpbiomatric.Sheets[0].SetColumnWidth(2, 200);
                fpbiomatric.Sheets[0].SetColumnWidth(3, 250);
                fpbiomatric.Sheets[0].SetColumnWidth(4, 100);
                fpbiomatric.Sheets[0].SetColumnWidth(5, 100);

                fpbiomatric.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[4].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[5].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[6].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[7].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[8].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[9].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[10].Font.Name = "Book Antiqua";

                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;


                fpbiomatric.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[4].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[5].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[6].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[7].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[8].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[9].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[10].Font.Size = FontUnit.Medium;

                //fpbiomatric.ActiveSheetView.Columns[10].Font.Size = FontUnit.Medium;
                ///  fpbiomatric.ActiveSheetView.Columns[11].Font.Size = FontUnit.Medium;
                //fpbiomatric.ActiveSheetView.Columns[12].Font.Size = FontUnit.Medium;

                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[2].ToString() + "-" + split[1].ToString() + "-" + split[0].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[2].ToString() + "-" + split1[1].ToString() + "-" + split1[0].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = "and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];
                    lbldate.Visible = false;
                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = Txtentryfrom.Text.ToString();
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = datefrom.ToString();
                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;

                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = split12[2].ToString() + "-" + split12[0].ToString() + "-" + split12[1].ToString();
                        fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;

                    }
                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                fpbiomatric.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].AllowTableCorner = true;


                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.FromArgb(214, 235, 255);
                fpbiomatric.Sheets[0].ColumnHeader.Rows[1].BackColor = Color.FromArgb(214, 235, 255);


                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }

                sql = "select distinct  t.App_No,T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree, b.Access_Date , right(CONVERT(nvarchar(100),time_in ,100),7) as Time_In, right(CONVERT(nvarchar(100),time_Out ,100),7) as Time_Out,Att  from Registration t,Degree G,Course C,Department D,Bio_Attendance B,attendance a where a.roll_no = t.Roll_No  And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and T.Roll_No = B.Roll_No AND B.Is_Staff=0 AND B.Latestrec ='1'  " + strdate + " and t.college_code in('" + colegecode + "') AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' " + Str + "";
                string courseid = string.Empty;
                string batchyr = string.Empty;
                string branch = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseid + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyr = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyr + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branch = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branch + "')";

                }
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}
                if (Chktimein.Checked == true)
                {
                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                    sql = sql + " " + strTime + "";
                }
                else if (Chktimeout.Checked == true)
                {
                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                    sql = sql + " " + strTime + "";
                }
                string order = "order by T.Roll_No,t.app_no,T.Stud_Name";

                sql = sql;
                int reg = 0;
                int ureg = 0;
                con1.Open();
                DataSet ds2 = new DataSet();
                SqlDataAdapter da2 = new SqlDataAdapter(sql, con1);
                da2.Fill(ds2);
                int hpresent = 0;
                int habsent = 0;
                int totalpresent = 0;

                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                totallatecount = 0;
                totalcountevennpermission = 0;
                int sno = 0;
                int cont = ds2.Tables[0].Rows.Count;
                if (cont > 0)
                {
                    for (int h = 0; h < ds2.Tables[0].Rows.Count; h++)
                    {

                        countpresent2 = 0;
                        countabsent2 = 0;
                        countlate2 = 0;
                        countpermission2 = 0;
                        countabsenteve = 0;
                        countpresenteve2 = 0;
                        countlateeve = 0;
                        countpermissioneve = 0;
                        tempstaffcode = "";


                        // fpbiomatric.Sheets[0].RowCount++;

                        string rollno; double totalp = 0; double totalA = 0; double totalLA = 0; double totalPER = 0;
                        rollno = ds2.Tables[0].Rows[h]["Roll_No"].ToString();
                        string appno = ds2.Tables[0].Rows[h]["app_no"].ToString();
                        string stud_name = ds2.Tables[0].Rows[h]["stud_name"].ToString();
                        string degree = ds2.Tables[0].Rows[h]["degree"].ToString();


                        if (!hat.Contains(rollno))
                        {

                            hat.Add(rollno, rollno);
                            fpbiomatric.Sheets[0].RowCount++;
                            for (int colcount = 11; colcount <= Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount) - 1; colcount = colcount + 4)
                            {
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;

                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;

                                fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";

                                fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;


                                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 4);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "In";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Out";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "Mor";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Eve";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 60);



                                int rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);
                                sno = rowstr;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = Convert.ToString(sno);
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = rollno.ToString();
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = stud_name.ToString();

                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = degree.ToString();


                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Left;


                                string datetagvalue;

                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                                strdate = " and  access_date='" + datetagvalue + "'";
                                string[] monyeararr = datetagvalue.Split('-');
                                int year = Convert.ToInt16(monyeararr[0]);
                                int month = Convert.ToInt32(monyeararr[1]);
                                int monyear = Convert.ToInt16(monyeararr[1]) + year * 12;
                                int day10 = Convert.ToInt16(monyeararr[2]);

                                string sql5 = "select att,roll_no,right(CONVERT(nvarchar(100),time_in ,100),7) as time_in,right(CONVERT(nvarchar(100),time_out ,100),7) as time_out from bio_attendance where roll_no='" + rollno + "' " + strdate + " and is_staff=0  " + Str + " ";
                                if (Chktimein.Checked == true)
                                {

                                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                                    sql5 = sql5 + " " + strTime + "";
                                }
                                else if (Chktimeout.Checked == true)
                                {

                                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                                    sql5 = sql5 + " " + strTime + "";
                                }

                                DataSet dsbio = new DataSet();
                                dsbio.Clear();
                                dsbio = d2.select_method_wo_parameter(sql5, "Text");

                                if (dsbio.Tables[0].Rows.Count > 0)
                                {
                                    string timein = dsbio.Tables[0].Rows[0]["time_in"].ToString();
                                    string timeout = dsbio.Tables[0].Rows[0]["time_out"].ToString();

                                    //Added By Saranyadevi 19.4.2018
                                    if (timein == timeout)
                                    {

                                        if ("12:00AM" == timein && "12:00AM" == timeout)
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = "";
                                        }
                                        else
                                        {

                                            string time = timein.Contains("AM") ? "AM" : "PM";
                                            if (time == "PM")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = timeout.ToString();
                                            }
                                            else
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = timeout.ToString(); ;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = "";

                                            }
                                        }
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = timein.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = timeout.ToString();
                                    }
                                    string att = ds2.Tables[0].Rows[h]["att"].ToString();
                                    string mrng = ""; string evng = "";
                                    string[] tmpdate;
                                    tmpdate = att.Split(new char[] { '-' });
                                    if (tmpdate.Length == 2)
                                    {
                                        mrng = tmpdate[0].ToString();
                                        evng = tmpdate[1].ToString();
                                    }


                                    if (mrng.Trim() != "")
                                    {
                                        if (mrng.ToString() == "P")
                                        {
                                            countpresent2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Green;
                                            counttotalmornpresent++;//= countpresent2;
                                        }
                                        if (mrng.ToString() == "A")
                                        {
                                            countabsent2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Red;
                                            counttotalabsentmorn++;
                                        }
                                        if (mrng.ToString() == "LA")
                                        {
                                            totallatecount++;
                                            countlate2++; totalmornlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.DarkRed;
                                        }
                                        if (mrng.ToString() == "PER")
                                        {
                                            countpermission2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                    }

                                    if (evng.Trim() != "")
                                    {
                                        if (evng.ToString() == "P")
                                        {
                                            countpresenteve2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Green;
                                            counttotalevennpresent++;// countpresenteve2++;
                                        }
                                        if (evng.ToString() == "A")
                                        {
                                            countabsenteve++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                            counttotalabsenteven++;
                                        }
                                        if (evng.ToString() == "LA")
                                        {
                                            totallatecount++;
                                            countlateeve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.DarkRed;
                                        }
                                        if (evng.ToString() == "PER")
                                        {
                                            countpermissioneve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                    }


                                    #region present
                                    totalperesent = countpresent2 + countpresenteve2;
                                    totalperesent = totalperesent / 2;
                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text.Trim() != "")
                                    {
                                        totalp = totalp + totalperesent;
                                    }
                                    else
                                    {
                                        totalp = totalperesent;
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text = Convert.ToString(totalp);
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, 7).ToString());
                                    c = g * 100;
                                    d = day3;
                                    // fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = att.ToString();
                                    if (c != 0)
                                    {
                                        percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = percentage.ToString();
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = "0";
                                    }
                                    #endregion

                                    #region Absent
                                    totalabsent = countabsent2 + countabsenteve;
                                    totalabsent = totalabsent / 2;

                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text.Trim() != "")
                                    {
                                        totalA = totalA + totalabsent;
                                    }
                                    else
                                    {
                                        totalA = totalabsent;
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text = Convert.ToString(totalA);
                                    //totalabsent.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                    #endregion

                                }



                            }

                            // habsent++;

                            fpbiomatric.Sheets[0].Columns[0].Visible = true;
                            fpbiomatric.Sheets[0].Columns[1].Visible = false;
                            fpbiomatric.Sheets[0].Columns[2].Visible = false;
                            fpbiomatric.Sheets[0].Columns[3].Visible = false;
                            fpbiomatric.Sheets[0].Columns[4].Visible = false;
                            //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;
                            //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;
                            //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;


                            fpbiomatric.Sheets[0].Columns[4].Visible = false;
                            fpbiomatric.Sheets[0].Columns[5].Visible = false;
                            if (count != 0)
                            {
                                if (cblsearch.Items[0].Selected == true)
                                {
                                    fpbiomatric.Sheets[0].Columns[1].Visible = true;
                                }
                                if (cblsearch.Items[1].Selected == true)
                                {
                                    fpbiomatric.Sheets[0].Columns[2].Visible = true;
                                }
                                if (cblsearch.Items[2].Selected == true)
                                {
                                    fpbiomatric.Sheets[0].Columns[3].Visible = true;
                                }
                                //if (cblsearch.Items[3].Selected == true)
                                //{
                                //    fpbiomatric.Sheets[0].Columns[3].Visible = true;
                                //}
                                //if (cblsearch.Items[4].Selected == true)
                                //{
                                //    fpbiomatric.Sheets[0].Columns[4].Visible = true;
                                //}
                                //if (cblsearch.Items[5].Selected == true)
                                //{
                                //    fpbiomatric.Sheets[0].Columns[colcount].Visible = true;
                                //}
                                //if (cblsearch.Items[6].Selected == true)
                                //{
                                //    fpbiomatric.Sheets[0].Columns[colcount].Visible = true;
                                //}
                                //if (cblsearch.Items[7].Selected == true)
                                //{
                                //    fpbiomatric.Sheets[0].Columns[colcount].Visible = true;
                                //}
                            }
                        }

                    }

                }

                lblpresent1.Text = ":" + counttotalmornpresent;
                lblpresent2.Text = ":" + counttotalevennpresent;
                lblabsent1.Text = ":" + counttotalabsentmorn;
                lblabsent2.Text = ":" + counttotalabsenteven;
                //lblpermission.Text = ":" + totalpermorn;//totalcountevennpermission;
                //lblpermission1.Text = ":" + totalpereven;
                //lbllate.Text = ":" + totalmornlate;//totallatecount;
                //lbllate1.Text = ":" + totalevenlate;
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    txtexcel.Text = "";
                    lblerror.Visible = false;
                    lblerror.Text = "";
                    lblexcel.Visible = false;
                    txtexcel.Visible = false;
                    btnexcel.Visible = false;
                    lblpresent1.Text = "0";
                    lblpresent2.Text = "0";
                    lblabsent1.Text = "0";
                    lblabsent2.Text = "0";
                    //lbllate.Text = "0";
                    //lbllate1.Text = "0";
                    return;
                }
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Present:" + Convert.ToString(counttotalmornpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Present:" + Convert.ToString(counttotalevennpresent);


                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Absent:" + counttotalabsentmorn.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Absent:" + counttotalabsenteven.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                //22.04.16
                //int counttotal = counttotalmornpresent + counttotalevennpresent + counttotalabsentmorn + counttotalabsenteven + totalcountevennpermission + totallatecount;// +countabsent;
                //fpbiomatric.Sheets[0].RowCount++;

                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                //fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of  Present in hostel:" + counttotal.ToString();
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                //int noofabsent = ureg - reg - hpresent;

                //fpbiomatric.Sheets[0].RowCount++;

                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                //fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of  Absent in hostel:" + noofabsent.ToString();
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                //end

                Double totalRows = 0;
                totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                if (totalRows >= 10)
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                    fpbiomatric.Height = 350;
                    fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows == 0)
                {

                    fpbiomatric.Height = 300;
                }
                else
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                    fpbiomatric.Height = 75 + (75 * Convert.ToInt32(totalRows));
                }

                //fpbiomatric.Height = 150 + (fpbiomatric.Sheets[0].RowCount * 10);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);

                fpbiomatric.Visible = true;
                lblnorec.Visible = false;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                ViewState["Bothpresent"] = null;
                ViewState["BothAbsent"] = null;
                ViewState["Bothod"] = null;
                ViewState["Bothper"] = null;
                Str = "";
                #endregion

            }
            else if (rdoinonly.Checked == true)
            {
                #region InOnly

                attfiltertype.Visible = true;
                lbllatetext.Visible = false;
                //lbllatetext.Text = "";
                imgabsent.Visible = true;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;
                imgpresent.Visible = true;
                imglate.Visible = false;
                lblmornlate.Visible = false;
                lblevenlate.Visible = false;

                imgper.Visible = false;
                lblmornper.Visible = false;
                lblevenper.Visible = false;
                //lblpermission.Visible = true;
                //lblpermission1.Visible = true;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lbllate.Visible = false;
                lbllate1.Visible = false;

                //imgontime.Visible = false;
                //lblontime.Visible = false;
                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                //  CheckBoxselect.Visible = true;
                string[] search = new string[50];
                cblsearch.Items[3].Selected = true;
                cblsearch.Items[4].Selected = true;
                cblsearch.Items[5].Selected = true;


                string wsearch = "";
                int count = 0;
                for (int i = 0; i < cblsearch.Items.Count; i++)
                {
                    if (cblsearch.Items[i].Selected == true)
                    {
                        count = count + 1;

                    }
                }

                fpbiomatric.Sheets[0].RowCount = 0;
                fpbiomatric.Sheets[0].ColumnCount = 0;
                // fpbiomatric.CommandBar.Visible = false;
                //    fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].PageSize = 10;
                fpbiomatric.RowHeader.Visible = true;
                fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpbiomatric.Pager.Align = HorizontalAlign.Right;
                fpbiomatric.Pager.Font.Bold = true;
                fpbiomatric.Pager.Font.Name = "Book Antiqua";
                fpbiomatric.Pager.ForeColor = Color.DarkGreen;
                fpbiomatric.Pager.BackColor = Color.AliceBlue;
                fpbiomatric.Pager.PageCount = 5;
                fpbiomatric.Sheets[0].SheetCorner.RowCount = 2;
                fpbiomatric.Sheets[0].SheetCorner.Columns[0].Visible = false;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnCount = 11;

                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                fpbiomatric.Sheets[0].Columns[0].CellType = txtcell;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Room No";
                fpbiomatric.Columns[4].Visible = false;
                fpbiomatric.Columns[5].Visible = false;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Att  %";
                fpbiomatric.Sheets[0].SetColumnWidth(6, 70);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 7].Text = "P";
                fpbiomatric.Sheets[0].SetColumnWidth(7, 30);


                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 8].Text = "A";
                fpbiomatric.Sheets[0].SetColumnWidth(8, 30);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 9].Text = "LA";
                fpbiomatric.Sheets[0].SetColumnWidth(9, 30);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 10].Text = "PER";
                fpbiomatric.Sheets[0].SetColumnWidth(10, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);

                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                fpbiomatric.Sheets[0].SetColumnWidth(0, 50);
                fpbiomatric.Sheets[0].SetColumnWidth(1, 200);
                fpbiomatric.Sheets[0].SetColumnWidth(2, 200);
                fpbiomatric.Sheets[0].SetColumnWidth(3, 250);
                fpbiomatric.Sheets[0].SetColumnWidth(4, 100);
                fpbiomatric.Sheets[0].SetColumnWidth(5, 100);

                fpbiomatric.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[4].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[5].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[6].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[7].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[8].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[9].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[10].Font.Name = "Book Antiqua";


                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;

                fpbiomatric.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[4].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[5].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[6].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[7].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[8].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[9].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[10].Font.Size = FontUnit.Medium;

                //fpbiomatric.ActiveSheetView.Columns[10].Font.Size = FontUnit.Medium;
                ///  fpbiomatric.ActiveSheetView.Columns[11].Font.Size = FontUnit.Medium;
                //fpbiomatric.ActiveSheetView.Columns[12].Font.Size = FontUnit.Medium;

                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[2].ToString() + "-" + split[1].ToString() + "-" + split[0].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[2].ToString() + "-" + split1[1].ToString() + "-" + split1[0].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = "and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];
                    lbldate.Visible = false;
                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = Txtentryfrom.Text.ToString();
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = datefrom.ToString();
                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Att";
                    //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 30);
                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = split12[2].ToString() + "-" + split12[0].ToString() + "-" + split12[1].ToString();
                        fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                        //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Att";
                        //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 30);
                    }
                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                fpbiomatric.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].AllowTableCorner = true;


                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.FromArgb(214, 235, 255);
                fpbiomatric.Sheets[0].ColumnHeader.Rows[1].BackColor = Color.FromArgb(214, 235, 255);


                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }

                sql = "select distinct  t.App_No,T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree, b.Access_Date , right(CONVERT(nvarchar(100),time_in ,100),7) as Time_In, right(CONVERT(nvarchar(100),time_Out ,100),7) as Time_Out,Att  from Registration t,Degree G,Course C,Department D,Bio_Attendance B,attendance a where a.roll_no = t.Roll_No  And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and T.Roll_No = B.Roll_No AND B.Is_Staff=0 AND B.Latestrec ='1'  " + strdate + " and t.college_code in('" + colegecode + "') AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' " + Str + "";

                string courseid = string.Empty;
                string batchyr = string.Empty;
                string branch = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseid + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyr = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyr + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branch = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branch + "')";

                }
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlBranch.SelectedItem.ToString() != "Select")
                //{
                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}
                if (Chktimein.Checked == true)
                {
                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                    sql = sql + " " + strTime + "";
                }
                else if (Chktimeout.Checked == true)
                {
                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                    sql = sql + " " + strTime + "";
                }
                string order = "order by T.Roll_No,t.app_no,T.Stud_Name";

                sql = sql;
                int reg = 0;
                int ureg = 0;
                con1.Open();
                DataSet ds2 = new DataSet();
                SqlDataAdapter da2 = new SqlDataAdapter(sql, con1);
                da2.Fill(ds2);
                int hpresent = 0;
                int habsent = 0;
                int totalpresent = 0;

                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                totallatecount = 0;
                totalcountevennpermission = 0;

                int cont = ds2.Tables[0].Rows.Count;
                if (cont > 0)
                {
                    for (int h = 0; h < ds2.Tables[0].Rows.Count; h++)
                    {

                        countpresent2 = 0;
                        countabsent2 = 0;
                        countlate2 = 0;
                        countpermission2 = 0;
                        countabsenteve = 0;
                        countpresenteve2 = 0;
                        countlateeve = 0;
                        countpermissioneve = 0;
                        tempstaffcode = "";


                        // fpbiomatric.Sheets[0].RowCount++;
                        string rollno; double totalp = 0; double totalA = 0; double totalLA = 0; double totalPER = 0;
                        rollno = ds2.Tables[0].Rows[h]["Roll_No"].ToString();
                        string appno = ds2.Tables[0].Rows[h]["app_no"].ToString();
                        string stud_name = ds2.Tables[0].Rows[h]["stud_name"].ToString();
                        string degree = ds2.Tables[0].Rows[h]["degree"].ToString();


                        if (!hat.Contains(rollno))
                        {
                            fpbiomatric.Sheets[0].RowCount++;
                            hat.Add(rollno, rollno);
                            for (int colcount = 11; colcount <= Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount) - 1; colcount = colcount + 4)
                            {
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;

                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;

                                fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";

                                fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;


                                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 4);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "In";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Out";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "Mor";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Eve";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 60);


                                int rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);
                                int str = rowstr;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = Convert.ToString(str);
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = rollno.ToString();
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = stud_name.ToString();

                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = degree.ToString();

                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Left;

                                string datetagvalue;

                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                                strdate = " and  access_date='" + datetagvalue + "'";
                                string[] monyeararr = datetagvalue.Split('-');
                                int year = Convert.ToInt16(monyeararr[0]);
                                int month = Convert.ToInt32(monyeararr[1]);
                                int monyear = Convert.ToInt16(monyeararr[1]) + year * 12;
                                int day10 = Convert.ToInt16(monyeararr[2]);

                                string sql5 = "select att,roll_no,right(CONVERT(nvarchar(100),time_in ,100),7) as time_in,right(CONVERT(nvarchar(100),time_out ,100),7) as time_out from bio_attendance where roll_no='" + rollno + "' " + strdate + " and is_staff=0  " + Str + " ";
                                if (Chktimein.Checked == true)
                                {

                                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                                    sql5 = sql5 + " " + strTime + "";
                                }
                                else if (Chktimeout.Checked == true)
                                {

                                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                                    sql5 = sql5 + " " + strTime + "";
                                }

                                DataSet dsbio = new DataSet();
                                dsbio.Clear();
                                dsbio = d2.select_method_wo_parameter(sql5, "Text");

                                if (dsbio.Tables[0].Rows.Count > 0)
                                {

                                    string timein = dsbio.Tables[0].Rows[0]["time_in"].ToString();

                                    if (timein == "12:00AM")
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = timein.ToString();
                                    }


                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = "";

                                    string att = ds2.Tables[0].Rows[h]["att"].ToString();
                                    string mrng = ""; string evng = "";
                                    string[] tmpdate;
                                    tmpdate = att.Split(new char[] { '-' });
                                    if (tmpdate.Length == 2)
                                    {
                                        mrng = tmpdate[0].ToString();
                                        evng = tmpdate[1].ToString();
                                    }


                                    if (mrng.Trim() != "")
                                    {
                                        if (mrng.ToString() == "P")
                                        {
                                            countpresent2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Green;
                                            counttotalmornpresent++;//= countpresent2;
                                        }
                                        if (mrng.ToString() == "A")
                                        {
                                            countabsent2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Red;
                                            counttotalabsentmorn++;
                                        }
                                        if (mrng.ToString() == "LA")
                                        {
                                            totallatecount++;
                                            countlate2++; totalmornlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.DarkRed;
                                        }
                                        if (mrng.ToString() == "PER")
                                        {
                                            countpermission2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                    }
                                    #region present
                                    totalperesent = countpresent2 + countpresenteve2;
                                    totalperesent = totalperesent / 2;
                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text.Trim() != "")
                                    {
                                        totalp = totalp + totalperesent;
                                    }
                                    else
                                    {
                                        totalp = totalperesent;
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text = Convert.ToString(totalp);
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, 7).ToString());
                                    c = g * 100;
                                    d = day3;
                                    // fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = att.ToString();
                                    if (c != 0)
                                    {
                                        percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = percentage.ToString();
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = "0";
                                    }
                                    #endregion

                                    #region Absent
                                    totalabsent = countabsent2 + countabsenteve;
                                    totalabsent = totalabsent / 2;

                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text.Trim() != "")
                                    {
                                        totalA = totalA + totalabsent;
                                    }
                                    else
                                    {
                                        totalA = totalabsent;
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text = Convert.ToString(totalA);
                                    //totalabsent.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                    #endregion
                                }
                            }

                            // habsent++;

                            fpbiomatric.Sheets[0].Columns[0].Visible = true;
                            fpbiomatric.Sheets[0].Columns[1].Visible = false;
                            fpbiomatric.Sheets[0].Columns[2].Visible = false;
                            fpbiomatric.Sheets[0].Columns[3].Visible = false;
                            fpbiomatric.Sheets[0].Columns[4].Visible = false;
                            fpbiomatric.Sheets[0].Columns[5].Visible = false;



                            fpbiomatric.Sheets[0].Columns[5].Visible = false;
                            if (count != 0)
                            {
                                if (cblsearch.Items[0].Selected == true)
                                {
                                    fpbiomatric.Sheets[0].Columns[1].Visible = true;
                                }
                                if (cblsearch.Items[1].Selected == true)
                                {
                                    fpbiomatric.Sheets[0].Columns[2].Visible = true;
                                }
                                if (cblsearch.Items[2].Selected == true)
                                {
                                    fpbiomatric.Sheets[0].Columns[3].Visible = true;
                                }

                            }
                        }


                    }

                }

                lblpresent1.Text = ":" + counttotalmornpresent;
                lblpresent2.Text = ":" + counttotalevennpresent;
                lblabsent1.Text = ":" + counttotalabsentmorn;
                lblabsent2.Text = ":" + counttotalabsenteven;
                //lblpermission.Text = ":" + totalpermorn;//totalcountevennpermission;
                //lblpermission1.Text = ":" + totalpereven;
                //lbllate.Text = ":" + totalmornlate;//totallatecount;
                //lbllate1.Text = ":" + totalevenlate;
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    txtexcel.Text = "";
                    lblerror.Visible = false;
                    lblerror.Text = "";
                    lblexcel.Visible = false;
                    txtexcel.Visible = false;
                    btnexcel.Visible = false;
                    lblpresent1.Text = "0";
                    lblpresent2.Text = "0";
                    lblabsent1.Text = "0";
                    lblabsent2.Text = "0";
                    lbllate.Text = "0";
                    lbllate1.Text = "0";
                    return;
                }
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Present:" + Convert.ToString(counttotalmornpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Present:" + Convert.ToString(counttotalevennpresent);


                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Absent:" + counttotalabsentmorn.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Absent:" + counttotalabsenteven.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                //22.04.16
                //int counttotal = counttotalmornpresent + counttotalevennpresent + counttotalabsentmorn + counttotalabsenteven + totalcountevennpermission + totallatecount;// +countabsent;
                //fpbiomatric.Sheets[0].RowCount++;

                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                //fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of  Present in hostel:" + counttotal.ToString();
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                //int noofabsent = ureg - reg - hpresent;

                //fpbiomatric.Sheets[0].RowCount++;

                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                //fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of  Absent in hostel:" + noofabsent.ToString();
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                //end

                Double totalRows = 0;
                totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                if (totalRows >= 10)
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                    fpbiomatric.Height = 350;
                    fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows == 0)
                {

                    fpbiomatric.Height = 300;
                }
                else
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                    fpbiomatric.Height = 75 + (75 * Convert.ToInt32(totalRows));
                }

                //fpbiomatric.Height = 150 + (fpbiomatric.Sheets[0].RowCount * 10);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);

                fpbiomatric.Visible = true;
                lblnorec.Visible = false;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                ViewState["Bothpresent"] = null;
                ViewState["BothAbsent"] = null;
                ViewState["Bothod"] = null;
                ViewState["Bothper"] = null;
                Str = "";
                #endregion
            }
            else if (rdooutonly.Checked == true)
            {
                #region OutOnly

                attfiltertype.Visible = true;
                lbllatetext.Visible = false;
                //lbllatetext.Text = "";
                imgabsent.Visible = true;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;
                imgpresent.Visible = true;


                imgper.Visible = false;
                lblmornper.Visible = false;
                lblevenper.Visible = false;
                //lblpermission.Visible = true;
                //lblpermission1.Visible = true;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lbllate.Visible = false;
                lbllate1.Visible = false;
                imglate.Visible = false;
                lblmornlate.Visible = false;
                lblevenlate.Visible = false;
                //imgontime.Visible = false;
                //lblontime.Visible = false;
                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                //  CheckBoxselect.Visible = true;
                string[] search = new string[50];
                cblsearch.Items[3].Selected = true;
                cblsearch.Items[4].Selected = true;
                cblsearch.Items[5].Selected = true;


                string wsearch = "";
                int count = 0;
                for (int i = 0; i < cblsearch.Items.Count; i++)
                {
                    if (cblsearch.Items[i].Selected == true)
                    {
                        count = count + 1;

                    }
                }

                fpbiomatric.Sheets[0].RowCount = 0;
                fpbiomatric.Sheets[0].ColumnCount = 0;
                // fpbiomatric.CommandBar.Visible = false;
                //     fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].PageSize = 10;
                fpbiomatric.RowHeader.Visible = true;
                fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpbiomatric.Pager.Align = HorizontalAlign.Right;
                fpbiomatric.Pager.Font.Bold = true;
                fpbiomatric.Pager.Font.Name = "Book Antiqua";
                fpbiomatric.Pager.ForeColor = Color.DarkGreen;
                fpbiomatric.Pager.BackColor = Color.AliceBlue;
                fpbiomatric.Pager.PageCount = 5;
                fpbiomatric.Sheets[0].SheetCorner.RowCount = 2;
                fpbiomatric.Sheets[0].SheetCorner.Columns[0].Visible = false;//delsi2710
                fpbiomatric.Sheets[0].SheetCorner.Columns[0].Visible = false;//delsi2710
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnCount = 11;

                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                fpbiomatric.Sheets[0].Columns[1].CellType = txtcell;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Room No";
                fpbiomatric.Columns[4].Visible = false;
                fpbiomatric.Columns[5].Visible = false;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Att  %";
                fpbiomatric.Sheets[0].SetColumnWidth(6, 70);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 7].Text = "P";
                fpbiomatric.Sheets[0].SetColumnWidth(7, 30);


                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 8].Text = "A";
                fpbiomatric.Sheets[0].SetColumnWidth(8, 30);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 9].Text = "LA";
                fpbiomatric.Sheets[0].SetColumnWidth(9, 30);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 10].Text = "PER";
                fpbiomatric.Sheets[0].SetColumnWidth(10, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].SetColumnWidth(1, 50);
                fpbiomatric.Sheets[0].SetColumnWidth(2, 200);
                fpbiomatric.Sheets[0].SetColumnWidth(2, 200);
                fpbiomatric.Sheets[0].SetColumnWidth(3, 250);
                fpbiomatric.Sheets[0].SetColumnWidth(4, 100);
                fpbiomatric.Sheets[0].SetColumnWidth(5, 100);

                fpbiomatric.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[4].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[5].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[6].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[7].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[8].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[9].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[10].Font.Name = "Book Antiqua";


                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;

                fpbiomatric.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[4].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[5].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[6].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[7].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[8].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[9].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[10].Font.Size = FontUnit.Medium;

                //fpbiomatric.ActiveSheetView.Columns[10].Font.Size = FontUnit.Medium;
                ///  fpbiomatric.ActiveSheetView.Columns[11].Font.Size = FontUnit.Medium;
                //fpbiomatric.ActiveSheetView.Columns[12].Font.Size = FontUnit.Medium;

                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[2].ToString() + "-" + split[1].ToString() + "-" + split[0].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[2].ToString() + "-" + split1[1].ToString() + "-" + split1[0].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = "and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];
                    lbldate.Visible = false;
                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = Txtentryfrom.Text.ToString();
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = datefrom.ToString();
                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Att";
                    //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 30);
                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = split12[2].ToString() + "-" + split12[0].ToString() + "-" + split12[1].ToString();
                        fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                        //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Att";
                        //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 30);
                    }
                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                fpbiomatric.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].AllowTableCorner = true;
                //   fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";

                //    fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 7, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, fpbiomatric.Sheets[0].ColumnCount - 1, 6, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 9, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 7, 1);
                // fpbiomatric.Sheets[0].ColumnHeader.Rows[6].BackColor = Color.FromArgb(214, 235, 255);
                //fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColorRight = Color.White;
                //fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 3, 1);

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.FromArgb(214, 235, 255);
                fpbiomatric.Sheets[0].ColumnHeader.Rows[1].BackColor = Color.FromArgb(214, 235, 255);


                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }

                sql = "select distinct  t.App_No,T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree, b.Access_Date , right(CONVERT(nvarchar(100),time_in ,100),7) as Time_In, right(CONVERT(nvarchar(100),time_Out ,100),7) as Time_Out,Att  from Registration t,Degree G,Course C,Department D,Bio_Attendance B,attendance a where a.roll_no = t.Roll_No  And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and T.Roll_No = B.Roll_No AND B.Is_Staff=0 AND B.Latestrec ='1'  " + strdate + " and t.college_code in('" + colegecode + "') AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' " + Str + "";
                string courseid = string.Empty;
                string batchyr = string.Empty;
                string branch = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseid + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyr = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyr + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branch = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branch + "')";

                }
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlBranch.SelectedItem.ToString() != "Select")
                //{
                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}
                if (Chktimein.Checked == true)
                {
                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                    sql = sql + " " + strTime + "";
                }
                else if (Chktimeout.Checked == true)
                {
                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                    sql = sql + " " + strTime + "";
                }
                string order = "order by T.Roll_No,t.app_no,T.Stud_Name";

                sql = sql;
                int reg = 0;
                int ureg = 0;
                con1.Open();
                DataSet ds2 = new DataSet();
                SqlDataAdapter da2 = new SqlDataAdapter(sql, con1);
                da2.Fill(ds2);
                int hpresent = 0;
                int habsent = 0;
                int totalpresent = 0;

                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                totallatecount = 0;
                totalcountevennpermission = 0;

                int cont = ds2.Tables[0].Rows.Count;
                if (cont > 0)
                {
                    for (int h = 0; h < ds2.Tables[0].Rows.Count; h++)
                    {

                        countpresent2 = 0;
                        countabsent2 = 0;
                        countlate2 = 0;
                        countpermission2 = 0;
                        countabsenteve = 0;
                        countpresenteve2 = 0;
                        countlateeve = 0;
                        countpermissioneve = 0;
                        tempstaffcode = "";


                        // fpbiomatric.Sheets[0].RowCount++;
                        string rollno; double totalp = 0; double totalA = 0; double totalLA = 0; double totalPER = 0;
                        rollno = ds2.Tables[0].Rows[h]["Roll_No"].ToString();
                        string appno = ds2.Tables[0].Rows[h]["app_no"].ToString();
                        string stud_name = ds2.Tables[0].Rows[h]["stud_name"].ToString();
                        string degree = ds2.Tables[0].Rows[h]["degree"].ToString();
                        string timein = ds2.Tables[0].Rows[h]["Time_In"].ToString();

                        int str = 0;

                        if (!hat.Contains(rollno))
                        {
                            fpbiomatric.Sheets[0].RowCount++;
                            hat.Add(rollno, rollno);
                            for (int colcount = 11; colcount <= Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount) - 1; colcount = colcount + 4)
                            {
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;

                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;

                                fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";

                                fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;


                                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 4);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "In";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Out";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "Mor";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Eve";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 60);

                                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(6, colcount, 1, 3);
                                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "Att";
                                //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;

                                string datetagvalue;
                                int rowstr = 0;
                                rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);
                                str = rowstr;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = Convert.ToString(str);
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = rollno.ToString();
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = stud_name.ToString();

                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = degree.ToString();
                                // fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = hostelname.ToString();

                                // fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].Text = roomname.ToString();

                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                                strdate = " and  access_date='" + datetagvalue + "'";
                                string[] monyeararr = datetagvalue.Split('-');
                                int year = Convert.ToInt16(monyeararr[0]);
                                int month = Convert.ToInt32(monyeararr[1]);
                                int monyear = Convert.ToInt16(monyeararr[1]) + year * 12;
                                int day10 = Convert.ToInt16(monyeararr[2]);

                                string sql5 = "select att,roll_no,right(CONVERT(nvarchar(100),time_in ,100),7) as time_in,right(CONVERT(nvarchar(100),time_out ,100),7) as time_out from bio_attendance where roll_no='" + rollno + "' " + strdate + " and is_staff=0  " + Str + " ";
                                if (Chktimein.Checked == true)
                                {

                                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                                    sql5 = sql5 + " " + strTime + "";
                                }
                                else if (Chktimeout.Checked == true)
                                {

                                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                                    sql5 = sql5 + " " + strTime + "";
                                }

                                DataSet dsbio = new DataSet();
                                dsbio.Clear();
                                dsbio = d2.select_method_wo_parameter(sql5, "Text");

                                if (dsbio.Tables[0].Rows.Count > 0)
                                {
                                    string timeout = dsbio.Tables[0].Rows[0]["time_out"].ToString();
                                    // fpbiomatric.Sheets[0].RowCount++;

                                    if (timeout == "12:00AM")
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = "";
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = timeout.ToString();
                                    }
                                    string att = ds2.Tables[0].Rows[h]["att"].ToString();
                                    string mrng = ""; string evng = "";
                                    string[] tmpdate;
                                    tmpdate = att.Split(new char[] { '-' });
                                    if (tmpdate.Length == 2)
                                    {
                                        mrng = tmpdate[0].ToString();
                                        evng = tmpdate[1].ToString();
                                    }
                                    if (evng.Trim() != "")
                                    {
                                        if (evng.ToString() == "P")
                                        {
                                            countpresenteve2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Green;
                                            counttotalevennpresent++;// countpresenteve2++;
                                        }
                                        if (evng.ToString() == "A")
                                        {
                                            countabsenteve++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                            counttotalabsenteven++;
                                        }
                                        if (evng.ToString() == "LA")
                                        {
                                            totallatecount++;
                                            countlateeve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.DarkRed;
                                        }
                                        if (evng.ToString() == "PER")
                                        {
                                            countpermissioneve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                    }



                                    #region present
                                    totalperesent = countpresent2 + countpresenteve2;
                                    totalperesent = totalperesent / 2;
                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text.Trim() != "")
                                    {
                                        totalp = totalp + totalperesent;
                                    }
                                    else
                                    {
                                        totalp = totalperesent;
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text = Convert.ToString(totalp);
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, 7).ToString());
                                    c = g * 100;
                                    d = day3;
                                    // fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = att.ToString();
                                    if (c != 0)
                                    {
                                        percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = percentage.ToString();
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = "0";
                                    }
                                    #endregion

                                    #region Absent
                                    totalabsent = countabsent2 + countabsenteve;
                                    totalabsent = totalabsent / 2;

                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text.Trim() != "")
                                    {
                                        totalA = totalA + totalabsent;
                                    }
                                    else
                                    {
                                        totalA = totalabsent;
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text = Convert.ToString(totalA);
                                    //totalabsent.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                    #endregion
                                }
                            }

                            // habsent++;

                            fpbiomatric.Sheets[0].Columns[0].Visible = true;
                            fpbiomatric.Sheets[0].Columns[1].Visible = false;
                            fpbiomatric.Sheets[0].Columns[2].Visible = false;
                            fpbiomatric.Sheets[0].Columns[3].Visible = false;
                            fpbiomatric.Sheets[0].Columns[4].Visible = false;
                            fpbiomatric.Sheets[0].Columns[5].Visible = false;
                            //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;
                            //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;
                            //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;


                            fpbiomatric.Sheets[0].Columns[5].Visible = false;
                            if (count != 0)
                            {
                                if (cblsearch.Items[0].Selected == true)
                                {
                                    fpbiomatric.Sheets[0].Columns[1].Visible = true;
                                }
                                if (cblsearch.Items[1].Selected == true)
                                {
                                    fpbiomatric.Sheets[0].Columns[2].Visible = true;
                                }
                                if (cblsearch.Items[2].Selected == true)
                                {
                                    fpbiomatric.Sheets[0].Columns[3].Visible = true;
                                }
                                //if (cblsearch.Items[3].Selected == true)
                                //{
                                //    fpbiomatric.Sheets[0].Columns[3].Visible = true;
                                //}
                                //if (cblsearch.Items[4].Selected == true)
                                //{
                                //    fpbiomatric.Sheets[0].Columns[4].Visible = true;
                                //}
                                //if (cblsearch.Items[5].Selected == true)
                                //{
                                //    fpbiomatric.Sheets[0].Columns[colcount].Visible = true;
                                //}
                                //if (cblsearch.Items[6].Selected == true)
                                //{
                                //    fpbiomatric.Sheets[0].Columns[colcount].Visible = true;
                                //}
                                //if (cblsearch.Items[7].Selected == true)
                                //{
                                //    fpbiomatric.Sheets[0].Columns[colcount].Visible = true;
                                //}
                            }
                        }


                    }

                }

                lblpresent1.Text = ":" + counttotalmornpresent;
                lblpresent2.Text = ":" + counttotalevennpresent;
                lblabsent1.Text = ":" + counttotalabsentmorn;
                lblabsent2.Text = ":" + counttotalabsenteven;
                //lblpermission.Text = ":" + totalpermorn;//totalcountevennpermission;
                //lblpermission1.Text = ":" + totalpereven;
                //lbllate.Text = ":" + totalmornlate;//totallatecount;
                //lbllate1.Text = ":" + totalevenlate;
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    txtexcel.Text = "";
                    lblerror.Visible = false;
                    lblerror.Text = "";
                    lblexcel.Visible = false;
                    txtexcel.Visible = false;
                    btnexcel.Visible = false;
                    lblpresent1.Text = "0";
                    lblpresent2.Text = "0";
                    lblabsent1.Text = "0";
                    lblabsent2.Text = "0";
                    lbllate.Text = "0";
                    lbllate1.Text = "0";
                    return;
                }
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Present:" + Convert.ToString(counttotalmornpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Present:" + Convert.ToString(counttotalevennpresent);


                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Absent:" + counttotalabsentmorn.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Absent:" + counttotalabsenteven.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                //22.04.16
                //int counttotal = counttotalmornpresent + counttotalevennpresent + counttotalabsentmorn + counttotalabsenteven + totalcountevennpermission + totallatecount;// +countabsent;
                //fpbiomatric.Sheets[0].RowCount++;

                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                //fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of  Present in hostel:" + counttotal.ToString();
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                //int noofabsent = ureg - reg - hpresent;

                //fpbiomatric.Sheets[0].RowCount++;

                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                //fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of  Absent in hostel:" + noofabsent.ToString();
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                //end

                Double totalRows = 0;
                totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                if (totalRows >= 10)
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                    fpbiomatric.Height = 350;
                    fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows == 0)
                {

                    fpbiomatric.Height = 300;
                }
                else
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                    fpbiomatric.Height = 75 + (75 * Convert.ToInt32(totalRows));
                }

                //fpbiomatric.Height = 150 + (fpbiomatric.Sheets[0].RowCount * 10);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);

                fpbiomatric.Visible = true;
                lblnorec.Visible = false;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                ViewState["Bothpresent"] = null;
                ViewState["BothAbsent"] = null;
                ViewState["Bothod"] = null;
                ViewState["Bothper"] = null;
                Str = "";
                #endregion
            }
            else if (rdounreg.Checked == true)
            {
                #region Un reg

                attfiltertype.Visible = false;
                imgabsent.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;

                lblmornlate.Visible = false;
                lblevenlate.Visible = false;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                imgpresent.Visible = false;
                imgper.Visible = false;
                lblmornper.Visible = false;
                lblevenper.Visible = false;
                lblpermission.Visible = false;
                lblpermission1.Visible = false;
                lblpresent1.Visible = false;
                lblpresent2.Visible = false;
                lbl_headermorn.Visible = false;
                lbl_headereven.Visible = false;
                lbllate.Visible = false;
                lbllate1.Visible = false;
                imglate.Visible = false;
                //lblmornlate.Visible = false;
                //lblevenlate.Visible = false;
                lbllatetext.Visible = false;
                //imgontime.Visible = false;
                //lblontime.Visible = false;
                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                //cblsearch.Items[5].Selected = true;
                //cblsearch.Items[6].Selected = true;
                //cblsearch.Items[7].Selected = true;
                //cblsearch.Items[5].Attributes.Add("style", "display:none;");
                //cblsearch.Items[6].Attributes.Add("style", "display:none;");
                //cblsearch.Items[7].Attributes.Add("style", "display:none;");

                //  CheckBoxselect.Visible = true;
                string[] search = new string[50];
                //if (cblsearch.Items[0].Selected == true)
                //{
                //    search[0] = "staffmaster.staff_code";
                //}
                //if (cblsearch.Items[1].Selected == true)
                //{
                //    search[1] = "staffmaster.staff_name";
                //}
                //if (cblsearch.Items[2].Selected == true)
                //{
                //    search[2] = "hrdept_master.dept_name";
                //}
                //if (cblsearch.Items[3].Selected == true)
                //{
                //    search[3] = "dept_acronym";
                //}
                //if (cblsearch.Items[4].Selected == true)
                //{
                //    search[4] = "desig_master.desig_name";
                //}
                //if (cblsearch.Items[5].Selected == true)
                //{
                //    search[5] = " desig_master.desig_acronym";
                //}
                //if (cblsearch.Items[6].Selected == true)
                //{
                //    search[6] = "CONVERT(VARCHAR(10),staffmaster.join_date,103)";
                //}
                //if (cblsearch.Items[7].Selected == true)
                //{
                //    search[7] = "in_out_time.category_name";
                //}

                string wsearch = "";
                int count = 0;
                for (int i = 0; i < cblsearch.Items.Count; i++)
                {
                    if (cblsearch.Items[i].Selected == true)
                    {
                        count = count + 1;
                    }
                }

                fpbiomatric.Sheets[0].RowCount = 0;
                fpbiomatric.Sheets[0].ColumnCount = 0;
                // fpbiomatric.CommandBar.Visible = false;
                //  fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].PageSize = 8;
                fpbiomatric.RowHeader.Visible = true;
                fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpbiomatric.Pager.Align = HorizontalAlign.Right;
                fpbiomatric.Pager.Font.Bold = true;
                fpbiomatric.Pager.Font.Name = "Book Antiqua";
                fpbiomatric.Pager.ForeColor = Color.DarkGreen;
                fpbiomatric.Pager.BackColor = Color.AliceBlue;
                fpbiomatric.Pager.PageCount = 5;
                fpbiomatric.Sheets[0].SheetCorner.RowCount = 2;
                fpbiomatric.Sheets[0].SheetCorner.Columns[0].Visible = false;//delsi2710
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnCount = 11;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Room No";
                fpbiomatric.Columns[4].Visible = false;
                fpbiomatric.Columns[5].Visible = false;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Att  %";
                fpbiomatric.Sheets[0].SetColumnWidth(6, 70);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 7].Text = "P";
                fpbiomatric.Sheets[0].SetColumnWidth(7, 30);


                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 8].Text = "A";
                fpbiomatric.Sheets[0].SetColumnWidth(8, 30);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 9].Text = "OD";
                fpbiomatric.Sheets[0].SetColumnWidth(9, 30);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 10].Text = "PER";
                fpbiomatric.Sheets[0].SetColumnWidth(10, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].SetColumnWidth(1, 200);
                fpbiomatric.Sheets[0].SetColumnWidth(2, 250);
                fpbiomatric.Sheets[0].SetColumnWidth(3, 100);
                fpbiomatric.Sheets[0].SetColumnWidth(4, 100);

                fpbiomatric.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[4].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[5].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[6].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[7].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[8].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[9].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[10].Font.Name = "Book Antiqua";

                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;




                fpbiomatric.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[4].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[5].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[6].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[7].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[8].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[9].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[10].Font.Size = FontUnit.Medium;

                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[2].ToString() + "-" + split[1].ToString() + "-" + split[0].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[2].ToString() + "-" + split1[1].ToString() + "-" + split1[0].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = " where B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];
                    lbldate.Visible = false;
                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].Text = Txtentryfrom.Text.ToString();
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].Tag = datefrom.ToString();
                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Att";
                    //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 30);

                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].Tag = split12[2].ToString() + "-" + split12[0].ToString() + "-" + split12[1].ToString(); ;
                        fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;


                        //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Att";
                        //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 30);
                    }
                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                fpbiomatric.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].AllowTableCorner = true;
                //  fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 7, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, fpbiomatric.Sheets[0].ColumnCount - 1, 6, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 9, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 7, 1);
                // fpbiomatric.Sheets[0].ColumnHeader.Rows[6].BackColor = Color.FromArgb(214, 235, 255);
                //fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColorRight = Color.White;
                //fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 3, 1);

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.FromArgb(214, 235, 255);
                fpbiomatric.Sheets[0].ColumnHeader.Rows[1].BackColor = Color.FromArgb(214, 235, 255);

                //sql = "SELECT r.room_name, h.hostel_name, T.Roll_No,T.Stud_Name,Course_Name+'-'+Dept_Name as Degree";
                //sql = sql + " FROM hostel_details h, Hostel_StudentDetails R,Registration T,Degree G,Course C,Department D ";
                //sql = sql + " Where r.Roll_Admit = T.Roll_Admit And T.degree_code = G.degree_code and isnull(R.vacated,0)=0 and isnull(r.relived,0)=0 ";
                //sql = sql + " AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and h.hostel_code=r.hostel_code  ";

                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }
                //change
                sql = "SELECT distinct t.app_no, T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree FROM attendance a,Registration T,Degree G,Course C,Department D Where a.Att_App_no  = T.App_No And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and t.college_code in('" + colegecode + "')  AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'";
                string courseid = string.Empty;
                string batchyr = string.Empty;
                string branch = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseid + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyr = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyr + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branch = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branch + "')";

                }

                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}
                //if (cboroll.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND T.App_No ='" + cboroll.SelectedItem.Value + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}

                sql = sql;
                con1.Open();
                DataSet ds2 = new DataSet();
                SqlDataAdapter da2 = new SqlDataAdapter(sql, con1);
                da2.Fill(ds2);
                int cont = ds2.Tables[0].Rows.Count;
                if (cont > 0)
                {
                    for (int h = 0; h < ds2.Tables[0].Rows.Count; h++)
                    {
                        countpresent2 = 0;
                        countabsent2 = 0;
                        countlate2 = 0;
                        countpermission2 = 0;
                        tempstaffcode = "";
                        countabsenteve = 0;
                        countpresenteve2 = 0;
                        countlateeve = 0;
                        countpermissioneve = 0;

                        string rollno;
                        rollno = Convert.ToString(ds2.Tables[0].Rows[h]["Roll_no"]);
                        string stud_name = ds2.Tables[0].Rows[h]["stud_name"].ToString();
                        string degree = ds2.Tables[0].Rows[h]["degree"].ToString();
                        //fpbiomatric.Sheets[0].RowCount++;
                        if (!hat.Contains(rollno))
                        {
                            hat.Add(rollno, rollno);
                            fpbiomatric.Sheets[0].RowCount++;
                            Boolean rowvis = false;
                            for (int colcount = 11; colcount <= Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount) - 1; colcount = colcount + 2)
                            {

                                //fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";
                                //fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";
                                //fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                                //fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;

                                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 2);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Mor";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Eve";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);

                                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 1);
                                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Att";
                                //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                string datetagvalue;
                                string startsem_date = string.Empty;
                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();
                                strdate = " and  access_date='" + datetagvalue + "'";
                                string[] monyeararr = datetagvalue.Split('-');
                                int year = Convert.ToInt16(monyeararr[0]);
                                int month = Convert.ToInt32(monyeararr[1]);
                                int monyear = Convert.ToInt16(monyeararr[1]) + year * 12;
                                int day10 = Convert.ToInt16(monyeararr[2]);

                                fpbiomatric.Width = 1000;
                                string sql2 = " Select * From bio_attendance Where Roll_No ='" + rollno + "' and Is_Staff = 0 " + strdate + "  ";//" + Str + "
                                SqlCommand cmd90 = new SqlCommand(sql2, con);
                                con.Close();
                                con.Open();
                                SqlDataReader dr52;
                                dr52 = cmd90.ExecuteReader();

                                if (dr52.HasRows == false)
                                {
                                    rowvis = true;
                                    int datval = 0;
                                    int rowcnt = 0;
                                    int rowstr = 0;
                                    int str = 0;
                                    if (tempstaffcode == "")
                                    {
                                        //fpbiomatric.Sheets[0].RowCount++;//10.05.16
                                        tempstaffcode = ds2.Tables[0].Rows[h]["App_no"].ToString();
                                    }
                                    else if ((tempstaffcode != "") && (tempstaffcode != rollno))
                                    {
                                        //fpbiomatric.Sheets[0].RowCount++;
                                        tempstaffcode = ds2.Tables[0].Rows[h]["App_no"].ToString();
                                    }
                                    if (Convert.ToString(ViewState["unreg"]).Trim() != "1")
                                    {
                                        str = 0;
                                        rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);
                                        str = rowstr;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = Convert.ToString(str);
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = Convert.ToString(ds2.Tables[0].Rows[h]["roll_no"]);
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = stud_name.ToString();

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = degree.ToString();

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;


                                        //string sqlll2 = "select * from hattendance where month_year='" + monyear + "' and roll_no='" + rollno + "'";
                                    }
                                    if (Convert.ToString(ViewState["unreg"]) == "1")
                                    {
                                        attendance = " and D" + day10 + "=" + "'2'" + " and D" + day10 + "E=" + "'2'";
                                    }
                                    else { attendance = ""; }

                                    string sqlll2 = " select * from attendance where month_year='" + monyear + "' and Att_App_no='" + tempstaffcode + "' " + attendance + "";
                                    SqlDataAdapter da34 = new SqlDataAdapter(sqlll2, con1);
                                    DataSet ds34 = new DataSet();
                                    da34.Fill(ds34);

                                    int catt = ds34.Tables[0].Rows.Count;
                                    if (catt > 0)
                                    {
                                        if (tempstaffcode == "")
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            countabsenteve = 0;
                                            countpresenteve2 = 0;
                                            countlateeve = 0;
                                            countpermissioneve = 0;

                                            // tempstaffcode = ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                        }
                                        else if ((tempstaffcode != "") && (tempstaffcode != ds34.Tables[0].Rows[0]["Att_App_no"].ToString()))
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            countabsenteve = 0;
                                            countpresenteve2 = 0;
                                            countlateeve = 0;
                                            countpermissioneve = 0;
                                            //fpbiomatric.Sheets[0].RowCount += 1;
                                            // tempstaffcode =  ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                        }
                                        fpbiomatric.Visible = true;
                                        if (Convert.ToString(ViewState["unreg"]) == "1")
                                        {

                                            rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);
                                            str = rowstr;

                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = Convert.ToString(str);
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = Convert.ToString(ds2.Tables[0].Rows[h]["roll_no"]);
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = stud_name.ToString();

                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = degree.ToString();

                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;


                                        }
                                        string no_of_hrs = "";
                                        int no_hrs = 0;
                                        string strquery = " select distinct No_of_hrs_per_day FROM registration r, Department d ,PeriodAttndSchedule p  ,seminfo s WHERE r.degree_code=p.degree_code and r.Batch_Year in('" + batchyr + "') and r.degree_code in('" + branch + "')";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(strquery, "Text");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {

                                            no_of_hrs = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                                        }
                                        if (no_of_hrs.Trim() != "")
                                        {
                                            no_hrs = Convert.ToInt16(no_of_hrs);
                                        }
                                        else
                                        {
                                            no_hrs = 0;
                                        }

                                        string str1 = "";
                                        string lasthr = "d" + no_hrs;
                                        string attmark = ""; string attmarkeve = ""; string atteve = "";
                                        attmark = ds34.Tables[0].Rows[0]["d" + day10 + "d1"].ToString();
                                        attmarkeve = ds34.Tables[0].Rows[0]["d" + day10 + lasthr].ToString();
                                        atteve = Attmark(attmarkeve);

                                        string[] splitatt = attmark.Split('-');
                                        attmark = splitatt[0];
                                        str1 = Attmark(attmark);

                                        //if (str1 == "P")
                                        //{
                                        //    countpresent2++;
                                        //    countpresent++;
                                        //}
                                        //totalpresent = countpresent2;

                                        if (str1.ToString() == "P")
                                        {
                                            countpresent2++;// countpresent++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].BackColor = Color.Green;
                                            counttotalmornpresent++;
                                        }
                                        if (atteve.ToString() == "P")
                                        {
                                            countpresenteve2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.Green;
                                            counttotalevennpresent++;// countpresenteve2++;
                                        }
                                        totalperesent = countpresent2 + countpresenteve2;
                                        totalperesent = totalperesent / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text = Convert.ToDouble(totalperesent).ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                        g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, 7).ToString());
                                        c = g * 100;
                                        d = day3;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = str1.ToString();

                                        if (c != 0)
                                        {
                                            percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = percentage.ToString();

                                        }
                                        else
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = "0";

                                        }
                                        if (str1.ToString() == "A")
                                        {
                                            countabsent2++; countabsent++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].BackColor = Color.Red;
                                            counttotalabsentmorn++;
                                        }
                                        if (atteve.ToString() == "A")
                                        {
                                            countabsenteve++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.Red;
                                            counttotalabsenteven++;
                                        }
                                        totalabsent = countabsent2 + countabsenteve;
                                        totalabsent = totalabsent / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text = totalabsent.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].HorizontalAlign = HorizontalAlign.Center;


                                        if (str1.ToString() == "OD")
                                        {
                                            totallatecount++;
                                            countlate2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].BackColor = Color.DarkRed;
                                        }
                                        if (atteve.ToString() == "OD")
                                        {
                                            totallatecount++;
                                            countlateeve++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.DarkRed;
                                        }
                                        totallate = countlate2 + countlateeve;
                                        totallate = totallate / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 9].Text = totallate.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 9].HorizontalAlign = HorizontalAlign.Center;

                                        if (str1.ToString() == "PER")
                                        {
                                            countpermission2++; totalmornlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                        if (atteve.ToString() == "PER")
                                        {
                                            countpermissioneve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                        totalpermission = countpermission2 + countpermissioneve;
                                        totalpermission = totalpermission / 2;



                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 10].Text = totalpermission.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 10].HorizontalAlign = HorizontalAlign.Center;

                                        //fpbiomatric.Sheets[0].Columns[0].Visible = false;
                                        //fpbiomatric.Sheets[0].Columns[1].Visible = false;
                                        //fpbiomatric.Sheets[0].Columns[2].Visible = false;
                                        //fpbiomatric.Sheets[0].Columns[3].Visible = false;
                                        //fpbiomatric.Sheets[0].Columns[4].Visible = false;
                                        //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;
                                        //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;
                                        //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;
                                        fpbiomatric.Sheets[0].Columns[1].Visible = true;
                                        fpbiomatric.Sheets[0].Columns[4].Visible = false;
                                        if (count != 0)
                                        {
                                            if (cblsearch.Items[0].Selected == true)
                                            {
                                                fpbiomatric.Sheets[0].Columns[1].Visible = true;
                                            }
                                            if (cblsearch.Items[1].Selected == true)
                                            {
                                                fpbiomatric.Sheets[0].Columns[2].Visible = true;
                                            }
                                            if (cblsearch.Items[2].Selected == true)
                                            {
                                                fpbiomatric.Sheets[0].Columns[3].Visible = true;
                                            }

                                        }
                                    }

                                }


                            }
                            if (!rowvis)
                                fpbiomatric.Sheets[0].RowCount--;
                        }
                    }
                }
                lblpresent1.Text = ":" + counttotalmornpresent;
                lblpresent2.Text = ":" + counttotalevennpresent;
                lblabsent1.Text = ":" + counttotalabsentmorn;
                lblabsent2.Text = ":" + counttotalabsenteven;
                //lblpermission.Text = ":" + totalpermorn;//totalcountevennpermission;
                //lblpermission1.Text = ":" + totalpereven;
                //lbllate.Text = ":" + totalmornlate;//totallatecount;
                //lbllate1.Text = ":" + totalevenlate;
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    txtexcel.Text = "";
                    lblerror.Visible = false;
                    lblerror.Text = "";
                    lblexcel.Visible = false;
                    txtexcel.Visible = false;
                    btnexcel.Visible = false;
                    lblpresent1.Text = "0";
                    lblpresent2.Text = "0";
                    lblabsent1.Text = "0";
                    lblabsent2.Text = "0";
                    return;
                }

                fpbiomatric.Sheets[0].RowCount++;

                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Absent:" + counttotalabsentmorn.ToString();

                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Absent:" + counttotalabsenteven.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                Double totalRows = 0;
                totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                if (totalRows >= 10)
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                    fpbiomatric.Height = 350;
                    fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows == 0)
                {

                    fpbiomatric.Height = 300;
                }
                else
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                    fpbiomatric.Height = 75 + (75 * Convert.ToInt32(totalRows));
                }

                fpbiomatric.Height = 150 + (fpbiomatric.Sheets[0].RowCount * 10);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);

                fpbiomatric.Visible = true;
                lblnorec.Visible = false;
                ViewState["unreg"] = null;
                Str = "";

                #endregion

            }
            else if (rdoboth.Checked == true)
            {
                #region Both
                attfiltertype.Visible = true;
                lbllatetext.Visible = false;
                //lbllatetext.Text = "";
                imgabsent.Visible = true;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;
                imgpresent.Visible = true;
                imglate.Visible = false;
                lblmornlate.Visible = false;
                lblevenlate.Visible = false;

                imgper.Visible = false;
                lblmornper.Visible = false;
                lblevenper.Visible = false;
                lblpermission.Visible = false;
                lblpermission1.Visible = false;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lbllate.Visible = false;
                lbllate1.Visible = false;

                //imgontime.Visible = false;
                //lblontime.Visible = false;
                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                //  CheckBoxselect.Visible = true;
                string[] search = new string[50];
                cblsearch.Items[3].Selected = true;
                cblsearch.Items[4].Selected = true;
                cblsearch.Items[5].Selected = true;
                //if (cblsearch.Items[0].Selected == true)
                //{
                //    search[0] = "staffmaster.staff_code";
                //}
                //if (cblsearch.Items[1].Selected == true)
                //{
                //    search[1] = "staffmaster.staff_name";
                //}
                //if (cblsearch.Items[2].Selected == true)
                //{
                //    search[2] = "hrdept_master.dept_name";
                //}
                //if (cblsearch.Items[3].Selected == true)
                //{
                //    search[3] = "dept_acronym";
                //}
                //if (cblsearch.Items[4].Selected == true)
                //{
                //    search[4] = "desig_master.desig_name";
                //}
                //if (cblsearch.Items[5].Selected == true)
                //{
                //    search[5] = " desig_master.desig_acronym";
                //}
                //if (cblsearch.Items[6].Selected == true)
                //{
                //    search[6] = "CONVERT(VARCHAR(10),staffmaster.join_date,103)";
                //}
                //if (cblsearch.Items[7].Selected == true)
                //{
                //    search[7] = "in_out_time.category_name";
                //}

                string wsearch = "";
                int count = 0;
                for (int i = 0; i < cblsearch.Items.Count; i++)
                {
                    if (cblsearch.Items[i].Selected == true)
                    {
                        count = count + 1;

                    }
                }

                fpbiomatric.Sheets[0].RowCount = 0;
                fpbiomatric.Sheets[0].ColumnCount = 0;
                // fpbiomatric.CommandBar.Visible = false;
                //    fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].PageSize = 10;
                fpbiomatric.RowHeader.Visible = true;
                fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpbiomatric.Pager.Align = HorizontalAlign.Right;
                fpbiomatric.Pager.Font.Bold = true;
                fpbiomatric.Pager.Font.Name = "Book Antiqua";
                fpbiomatric.Pager.ForeColor = Color.DarkGreen;
                fpbiomatric.Pager.BackColor = Color.AliceBlue;
                fpbiomatric.Pager.PageCount = 5;
                fpbiomatric.Sheets[0].SheetCorner.RowCount = 2;
                fpbiomatric.Sheets[0].SheetCorner.Columns[0].Visible = false;//delsi2710
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnCount = 11;

                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                fpbiomatric.Sheets[0].Columns[1].CellType = txtcell;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Room No";
                fpbiomatric.Columns[4].Visible = false;
                fpbiomatric.Columns[5].Visible = false;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Att  %";
                fpbiomatric.Sheets[0].SetColumnWidth(6, 70);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 7].Text = "P";
                fpbiomatric.Sheets[0].SetColumnWidth(7, 30);


                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 8].Text = "A";
                fpbiomatric.Sheets[0].SetColumnWidth(8, 30);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 9].Text = "LA";
                fpbiomatric.Sheets[0].SetColumnWidth(9, 30);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 10].Text = "PER";
                fpbiomatric.Sheets[0].SetColumnWidth(10, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);

                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].SetColumnWidth(0, 50);
                fpbiomatric.Sheets[0].SetColumnWidth(1, 200);
                fpbiomatric.Sheets[0].SetColumnWidth(2, 200);
                fpbiomatric.Sheets[0].SetColumnWidth(3, 250);
                fpbiomatric.Sheets[0].SetColumnWidth(4, 100);
                fpbiomatric.Sheets[0].SetColumnWidth(5, 100);

                fpbiomatric.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[4].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[5].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[6].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[7].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[8].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[9].Font.Name = "Book Antiqua";
                fpbiomatric.ActiveSheetView.Columns[10].Font.Name = "Book Antiqua";


                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;

                fpbiomatric.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[4].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[5].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[6].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[7].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[8].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[9].Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.Columns[10].Font.Size = FontUnit.Medium;

                //fpbiomatric.ActiveSheetView.Columns[10].Font.Size = FontUnit.Medium;
                ///  fpbiomatric.ActiveSheetView.Columns[11].Font.Size = FontUnit.Medium;
                //fpbiomatric.ActiveSheetView.Columns[12].Font.Size = FontUnit.Medium;

                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[2].ToString() + "-" + split[1].ToString() + "-" + split[0].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[2].ToString() + "-" + split1[1].ToString() + "-" + split1[0].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = " where B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];
                    lbldate.Visible = false;
                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = Txtentryfrom.Text.ToString();
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = datefrom.ToString();
                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Att";
                    //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 30);
                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = split12[2].ToString() + "-" + split12[0].ToString() + "-" + split12[1].ToString();
                        fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;




                    }
                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                fpbiomatric.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].AllowTableCorner = true;
                //   fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";

                //   fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 7, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, fpbiomatric.Sheets[0].ColumnCount - 1, 6, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 9, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 7, 1);
                // fpbiomatric.Sheets[0].ColumnHeader.Rows[6].BackColor = Color.FromArgb(214, 235, 255);
                //fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColorRight = Color.White;
                //fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 3, 1);

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.FromArgb(214, 235, 255);
                fpbiomatric.Sheets[0].ColumnHeader.Rows[1].BackColor = Color.FromArgb(214, 235, 255);

                //sql = "SELECT r.room_name, h.hostel_name, T.Roll_No,T.Stud_Name,Course_Name+'-'+Dept_Name as Degree";
                //sql = sql + ";
                //sql = sql + " FROM hostel_details h, Hostel_StudentDetails R,Registration T,Degree G,Course C,Department D ";
                //sql = sql + " Where r.Roll_Admit = T.Roll_Admit And T.degree_code = G.degree_code and isnull(R.vacated,0)=0 and isnull(R.relived,0)=0 ";
                //sql = sql + " AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and h.hostel_code=r.hostel_code  ";
                //// sql = sql + "  AND T.Roll_No = B.Roll_No AND B.Is_Staff=0 AND B.Latestrec ='1'" + strdate + " ";

                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }

                sql = "SELECT distinct  t.app_no, T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree FROM attendance a,Registration T,Degree G,Course C,Department D  Where a.Att_App_no  = T.App_No And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and t.college_code in('" + colegecode + "') AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'";

                string courseid = string.Empty;
                string batchyr = string.Empty;
                string branch = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseid + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyr = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyr + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branch = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branch + "')";

                }
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}
                //if (cboroll.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND T.App_No ='" + cboroll.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi0201
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}
                string order = "order by T.Roll_No,t.app_no,T.Stud_Name";

                sql = sql + "order by  T.Roll_No";
                int reg = 0;
                int ureg = 0;
                con1.Open();
                DataSet ds2 = new DataSet();
                SqlDataAdapter da2 = new SqlDataAdapter(sql, con1);
                da2.Fill(ds2);
                int hpresent = 0;
                int habsent = 0;
                int totalpresent = 0;

                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                totallatecount = 0;
                totalcountevennpermission = 0;

                int cont = ds2.Tables[0].Rows.Count;
                if (cont > 0)
                {
                    for (int h = 0; h < ds2.Tables[0].Rows.Count; h++)
                    {
                        //bool avail = false;

                        //if (presentclick == true || absentclick == true)
                        //{

                        //    string qryc = "select att,roll_no,right(CONVERT(nvarchar(100),time_in ,100),7) as time_in,right(CONVERT(nvarchar(100),time_out ,100),7) as time_out from bio_attendance b  " + strdate + " and is_staff=0  " + Str + " and roll_no='" + rollno + " '";
                        //    ds2.Clear();
                        //    ds2 = d2.select_method_wo_parameter(qryc, "Text");
                        //    if (ds2.Tables.Count>0 && ds2.Tables[0].Rows.Count > 0)
                        //    {
                        //        avail = true;
                        //    }
                        //    else
                        //    {
                        //        avail = false;
                        //    }
                        //}
                        //else
                        //{
                        //    avail = true;
                        //}



                        // sql = "";
                        //Str = "";
                        countpresent2 = 0;
                        countabsent2 = 0;
                        countlate2 = 0;
                        countpermission2 = 0;
                        countabsenteve = 0;
                        countpresenteve2 = 0;
                        countlateeve = 0;
                        countpermissioneve = 0;
                        tempstaffcode = "";

                        //string hostelname = ds2.Tables[0].Rows[h]["hostelname"].ToString();
                        // string roomname = ds2.Tables[0].Rows[h]["room_name"].ToString();
                        //if (avail == true)
                        //{
                        fpbiomatric.Sheets[0].RowCount++;
                        string rollno; double totalp = 0; double totalA = 0; double totalLA = 0; double totalPER = 0;
                        rollno = ds2.Tables[0].Rows[h]["Roll_No"].ToString();
                        string appno = ds2.Tables[0].Rows[h]["app_no"].ToString();
                        string stud_name = ds2.Tables[0].Rows[h]["stud_name"].ToString();
                        string degree = ds2.Tables[0].Rows[h]["degree"].ToString();
                        if (!hat.Contains(rollno))
                        {
                            hat.Add(rollno, rollno);
                            for (int colcount = 11; colcount <= Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount) - 1; colcount = colcount + 4)
                            {
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;

                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;

                                fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";

                                fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;


                                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 4);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "In";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Out";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "Mor";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Eve";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 60);

                                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(6, colcount, 1, 3);
                                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "Att";
                                //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;

                                string datetagvalue;
                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                                strdate = " and  access_date='" + datetagvalue + "'";


                                string startsem_date = string.Empty;
                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();
                                strdate = " and  access_date='" + datetagvalue + "'";
                                string[] monyeararr = datetagvalue.Split('-');
                                int year = Convert.ToInt16(monyeararr[0]);
                                int month = Convert.ToInt32(monyeararr[1]);
                                int monyear = Convert.ToInt16(monyeararr[1]) + year * 12;
                                int day10 = Convert.ToInt16(monyeararr[2]);
                                ureg++;
                                fpbiomatric.Width = 1000;
                                int datval = 0;
                                int rowcnt = 0;
                                int rowstr = 0;

                                string no_of_hrs = "";
                                int no_hrs = 0;

                                string strquery = " select distinct No_of_hrs_per_day FROM registration r, Department d ,PeriodAttndSchedule p  ,seminfo s WHERE r.degree_code=p.degree_code and r.Batch_Year in('" + batchyr + "') and r.degree_code in('" + branch + "')";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(strquery, "Text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {

                                    no_of_hrs = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                                }
                                if (no_of_hrs.Trim() != "")
                                {
                                    no_hrs = Convert.ToInt16(no_of_hrs);
                                }
                                else
                                {
                                    no_hrs = 0;
                                }

                                string str1 = "";
                                string lasthr = "d" + no_hrs;
                                if (tempstaffcode == "")
                                {
                                    //fpbiomatric.Sheets[0].RowCount++;
                                    tempstaffcode = ds2.Tables[0].Rows[h]["app_no"].ToString();
                                }
                                else if ((tempstaffcode != "") && (tempstaffcode != rollno))
                                {
                                    //fpbiomatric.Sheets[0].RowCount++;
                                    tempstaffcode = ds2.Tables[0].Rows[h]["app_no"].ToString();
                                }
                                int str = 0;
                                if ((Convert.ToString(ViewState["Bothpresent"]) != "1") && (Convert.ToString(ViewState["BothAbsent"]) != "2") && (Convert.ToString(ViewState["Bothod"]) != "3") && (Convert.ToString(ViewState["Bothper"]) != "4"))
                                {

                                    //fpbiomatric.Sheets[0].RowCount++;
                                    rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);
                                    str = rowstr;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = "";

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = Convert.ToString(str);
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = rollno.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = stud_name.ToString();

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = degree.ToString();
                                    // fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = hostelname.ToString();

                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].Text = roomname.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                }

                                //attmarkeve = dsbio.Tables[1].Rows[0]["d" + day10 + lasthr].ToString();
                                if (Convert.ToString(ViewState["Bothpresent"]) == "1")
                                {
                                    if (rdb_morn.Checked == true)
                                    {
                                        hostelattend1 = " and d" + day10 + "d1=" + "'1'";
                                    }
                                    else if (rdb_even.Checked == true)
                                    {
                                        hostelattend1 = " and d" + day10 + lasthr + "=" + "'1'";
                                    }
                                    else
                                    {
                                        hostelattend1 = " and d" + day10 + "d1=" + "'1'" + " and d" + day10 + lasthr + "=" + "'1'";
                                    }
                                }
                                else if (Convert.ToString(ViewState["BothAbsent"]) == "2")
                                {
                                    if (rdb_morn.Checked == true)
                                    {
                                        hostelattend1 = " and d" + day10 + "d1=" + "'2'";
                                    }
                                    else if (rdb_even.Checked == true)
                                    {
                                        hostelattend1 = " and d" + day10 + lasthr + "=" + "'2'";
                                    }
                                    else
                                    {
                                        hostelattend1 = " and d" + day10 + "d1=" + "'2'" + " and d" + day10 + lasthr + "=" + "'2'";
                                    }
                                }
                                else if (Convert.ToString(ViewState["Bothod"]) == "3")
                                {
                                    if (rdb_morn.Checked == true)
                                    {
                                        hostelattend1 = " and d" + day10 + "d1=" + "'3'";
                                    }
                                    else if (rdb_even.Checked == true)
                                    {
                                        hostelattend1 = "and d" + day10 + lasthr + "=" + "'3'";
                                    }
                                    else
                                    {
                                        hostelattend1 = " and d" + day10 + "d1=" + "'3'" + " and d" + day10 + lasthr + "=" + "'3'";
                                    }
                                }
                                else if (Convert.ToString(ViewState["Bothper"]) == "4")
                                {
                                    if (rdb_morn.Checked == true)
                                    {
                                        hostelattend1 = " and D" + day10 + "=" + "'4'";
                                    }
                                    else if (rdb_even.Checked == true)
                                    {
                                        hostelattend1 = " and D" + day10 + "E=" + "'4'";
                                    }
                                    else
                                    {
                                        hostelattend1 = " and D" + day10 + "=" + "'4'" + " and D" + day10 + "E=" + "'4'";
                                    }
                                }
                                else
                                {
                                    hostelattend1 = "";
                                }

                                string sql5 = "select att,roll_no,right(CONVERT(nvarchar(100),time_in ,100),7) as time_in,right(CONVERT(nvarchar(100),time_out ,100),7) as time_out from bio_attendance where roll_no='" + rollno + "' " + strdate + " and is_staff=0  " + Str + " ";
                                if (Chktimein.Checked == true)
                                {

                                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                                    sql5 = sql5 + " " + strTime + "";
                                }
                                else if (Chktimeout.Checked == true)
                                {

                                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                                    sql5 = sql5 + " " + strTime + "";
                                }
                                sql5 = sql5 + "select * from attendance where month_year='" + monyear + "' and Att_App_no='" + appno + "' " + hostelattend1 + "";
                                DataSet dsbio = new DataSet();
                                dsbio.Clear();
                                dsbio = d2.select_method_wo_parameter(sql5, "Text");

                                //SqlDataAdapter dabio = new SqlDataAdapter(sql5, mycon);
                                //mycon.Close();
                                //mycon.Open();
                                //dabio.Fill(dsbio);
                                //int cntbio = dsbio.Tables[0].Rows.Count;
                                if (dsbio.Tables[0].Rows.Count > 0)
                                {
                                    reg++;
                                    if (tempstaffcode == "")
                                    {
                                        countpresent2 = 0;
                                        countabsent2 = 0;
                                        countlate2 = 0;
                                        countpermission2 = 0;
                                        countabsenteve = 0;
                                        countpresenteve2 = 0;
                                        countlateeve = 0;
                                        countpermissioneve = 0;
                                        // tempstaffcode = ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                    }
                                    else if ((tempstaffcode != "") && (tempstaffcode != dsbio.Tables[0].Rows[0]["Roll_No"].ToString()))
                                    {
                                        countpresent2 = 0;
                                        countabsent2 = 0;
                                        countlate2 = 0;
                                        countpermission2 = 0;

                                        countabsenteve = 0;
                                        countpresenteve2 = 0;
                                        countlateeve = 0;
                                        countpermissioneve = 0;
                                        //fpbiomatric.Sheets[0].RowCount += 1;
                                        // tempstaffcode =  ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                    }

                                    if ((Convert.ToString(ViewState["Bothpresent"]) == "1") || (Convert.ToString(ViewState["BothAbsent"]) == "2") || (Convert.ToString(ViewState["Bothod"]) == "3") || (Convert.ToString(ViewState["Bothper"]) == "4"))
                                    {
                                        fpbiomatric.Sheets[0].RowCount++;
                                        rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);
                                        str = rowstr;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = Convert.ToString(str);
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = rollno.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = stud_name.ToString();

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = degree.ToString();
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = hostelname.ToString();

                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].Text = roomname.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    }


                                    string timein = dsbio.Tables[0].Rows[0]["time_in"].ToString();
                                    string timeout = dsbio.Tables[0].Rows[0]["time_out"].ToString();

                                    //Added By Saranyadevi 19.4.2018
                                    if (timein == timeout)
                                    {

                                        if ("12:00AM" == timein && "12:00AM" == timeout)
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = "";
                                        }
                                        else
                                        {

                                            string time = timein.Contains("AM") ? "AM" : "PM";
                                            if (time == "PM")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = timeout.ToString();
                                            }
                                            else
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = timeout.ToString(); ;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = "";

                                            }
                                        }
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = timein.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = timeout.ToString();
                                    }
                                    string att = dsbio.Tables[0].Rows[0]["att"].ToString();
                                    string mrng = ""; string evng = "";
                                    string[] tmpdate;
                                    tmpdate = att.Split(new char[] { '-' });
                                    if (tmpdate.Length == 2)
                                    {
                                        mrng = tmpdate[0].ToString();
                                        evng = tmpdate[1].ToString();
                                    }



                                    //if (tmpdate.Length == 1)
                                    //{
                                    //    mrng = tmpdate[0].ToString();
                                    //    evng = "";
                                    //}
                                    //05.05.16
                                    // string sql2 = " select*from HT_Attendance where AttnMonth='" + month + "' and AttnYear='" + year + "' and App_No='" + appno + "'";

                                    if (mrng.Trim() != "")
                                    {
                                        if (mrng.ToString() == "P")
                                        {
                                            countpresent2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Green;
                                            counttotalmornpresent++;//= countpresent2;
                                        }
                                        if (mrng.ToString() == "A")
                                        {
                                            countabsent2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Red;
                                            counttotalabsentmorn++;
                                        }
                                        if (mrng.ToString() == "LA")
                                        {
                                            totallatecount++;
                                            countlate2++; totalmornlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.DarkRed;
                                        }
                                        if (mrng.ToString() == "PER")
                                        {
                                            countpermission2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                    }
                                    else
                                    {
                                        if (dsbio.Tables[1].Rows.Count > 0)
                                        {


                                            string atteve = "";
                                            string attmark = ""; string attmarkeve = "";
                                            attmark = dsbio.Tables[1].Rows[0]["d" + day10 + "d1"].ToString();
                                            //attmarkeve = dsbio.Tables[1].Rows[0]["d" + day10 + lasthr].ToString();
                                            //atteve = Attmark(attmarkeve);

                                            string[] splitatt = attmark.Split('-');
                                            attmark = splitatt[0];
                                            str1 = Attmark(attmark);
                                            if (str1 == "P")
                                            {
                                                countpresent2++; counttotalmornpresent++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = str1.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Green;
                                            }
                                            if (str1 == "A")
                                            {
                                                countabsent2++; countabsent++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = str1.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Red;
                                                counttotalabsentmorn++;
                                            }
                                            if (str1 == "LA")
                                            {
                                                countlate2++;
                                                countlate++; totalevenlate++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = str1.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.DarkRed;
                                            }
                                            if (str1 == "PER")
                                            {
                                                countpermission2++;
                                                countpermission++; totalpermorn++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = str1.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                    }
                                    if (evng.Trim() != "")
                                    {
                                        if (evng.ToString() == "P")
                                        {
                                            countpresenteve2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Green;
                                            counttotalevennpresent++;// countpresenteve2++;
                                        }
                                        if (evng.ToString() == "A")
                                        {
                                            countabsenteve++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                            counttotalabsenteven++;
                                        }
                                        if (evng.ToString() == "LA")
                                        {
                                            totallatecount++;
                                            countlateeve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.DarkRed;
                                        }
                                        if (evng.ToString() == "PER")
                                        {
                                            countpermissioneve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                    }
                                    else
                                    {
                                        if (dsbio.Tables[1].Rows.Count > 0)
                                        {
                                            string atteve = "";
                                            string attmark = ""; string attmarkeve = "";
                                            //attmark = dsbio.Tables[1].Rows[0]["d" + day10 + ""].ToString();
                                            attmarkeve = dsbio.Tables[1].Rows[0]["d" + day10 + lasthr].ToString();
                                            atteve = Attmark(attmarkeve);
                                            //string[] splitatt = attmark.Split('-');
                                            //attmark = splitatt[0];
                                            //str1 = Attmark(attmark);
                                            if (atteve == "P")
                                            {
                                                countpresenteve2++; counttotalevennpresent++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Green;
                                            }
                                            if (atteve.ToString() == "A")
                                            {
                                                countabsenteve++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                                counttotalabsenteven++;
                                            }
                                            if (atteve.ToString() == "OD")
                                            {
                                                //countabsenteve++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                                //counttotalabsenteven++;
                                            }

                                        }
                                    }
                                    #region present
                                    totalperesent = countpresent2 + countpresenteve2;
                                    totalperesent = totalperesent / 2;
                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text.Trim() != "")
                                    {
                                        totalp = totalp + totalperesent;
                                    }
                                    else
                                    {
                                        totalp = totalperesent;
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text = Convert.ToString(totalp);
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, 7).ToString());
                                    c = g * 100;
                                    d = day3;
                                    // fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = att.ToString();
                                    if (c != 0)
                                    {
                                        percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = percentage.ToString();
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = "0";
                                    }
                                    #endregion

                                    #region Absent
                                    totalabsent = countabsent2 + countabsenteve;
                                    totalabsent = totalabsent / 2;

                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text.Trim() != "")
                                    {
                                        totalA = totalA + totalabsent;
                                    }
                                    else
                                    {
                                        totalA = totalabsent;
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text = Convert.ToString(totalA);
                                    //totalabsent.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                    #endregion

                                    #region Late
                                    totallate = countlate2 + countlateeve;
                                    totallate = totallate / 2;

                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 9].Text.Trim() != "")
                                    {
                                        totalLA = totalLA + totallate;
                                    }
                                    else
                                    {
                                        totalLA = totallate;
                                    }

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 9].Text = totalLA.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                    #endregion

                                    #region permission
                                    totalpermission = countpermission2 + countpermissioneve;
                                    totalpermission = totalpermission / 2;

                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 10].Text.Trim() != "")
                                    {
                                        totalPER = totalPER + totalpermission;
                                    }
                                    else
                                    {
                                        totalPER = totalabsent;
                                    }

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 10].Text = totalPER.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                                    #endregion


                                }
                                else
                                {
                                    //string sql2 = "select * from hattendance where month_year='" + monyear + "' and roll_no='" + rollno + "'";
                                    string sql2 = " select * from Attendance where month_year='" + monyear + "' and roll_no='" + rollno + "'";

                                    SqlDataAdapter da34 = new SqlDataAdapter(sql2, con1);
                                    DataSet ds34 = new DataSet();
                                    da34.Fill(ds34);

                                    int catt = ds34.Tables[0].Rows.Count;
                                    if (catt > 0)
                                    {
                                        if (tempstaffcode == "")
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            //countabsenteve = 0;
                                            //countpresenteve2 = 0;
                                            //countlateeve = 0;
                                            //countpermissioneve = 0;
                                            // tempstaffcode = ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                        }
                                        else if ((tempstaffcode != "") && (tempstaffcode != ds34.Tables[0].Rows[0]["roll_no"].ToString()))
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            //countabsenteve = 0;
                                            //countpresenteve2 = 0;
                                            //countlateeve = 0;
                                            //countpermissioneve = 0;
                                            //fpbiomatric.Sheets[0].RowCount += 1;
                                            //tempstaffcode =  ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                        }
                                        fpbiomatric.Visible = true;

                                        if ((Convert.ToString(ViewState["Bothpresent"]) == "1") || (Convert.ToString(ViewState["BothAbsent"]) == "2") || (Convert.ToString(ViewState["Bothod"]) == "3") || (Convert.ToString(ViewState["Bothper"]) == "4"))
                                        {
                                            fpbiomatric.Sheets[0].RowCount++;
                                            rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);
                                            str = rowstr;

                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = Convert.ToString(str);
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = rollno.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = stud_name.ToString();

                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = degree.ToString();
                                            // fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = hostelname.ToString();

                                            //  fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].Text = roomname.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        string atteve = "";
                                        string attmark = ""; string attmarkeve = "";
                                        attmark = ds34.Tables[0].Rows[0]["d" + day10 + "d1"].ToString();
                                        attmarkeve = ds34.Tables[0].Rows[0]["d" + day10 + lasthr].ToString();
                                        atteve = Attmark(attmarkeve);

                                        string[] splitatt = attmark.Split('-');
                                        attmark = splitatt[0];
                                        str1 = Attmark(attmark);
                                        if (str1 == "P")
                                        {
                                            countpresent2++; counttotalmornpresent++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Green;
                                        }
                                        if (atteve == "P")
                                        {
                                            countpresenteve2++; counttotalevennpresent++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Green;
                                        }
                                        totalperesent = countpresent2 + countpresenteve2;
                                        totalperesent = totalperesent / 2;

                                        //totalpresent = countpresent2;//22.04.16 barath
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text = Convert.ToDouble(totalperesent).ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                        g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, 7).ToString());
                                        c = g * 100;

                                        d = day3;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = str1.ToString();
                                        if (c != 0)
                                        {
                                            percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = percentage.ToString();
                                        }
                                        else
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = "0";

                                        }
                                        if (str1 == "")
                                        {
                                            str1 = "";
                                        }

                                        if (str1 == "A")
                                        {
                                            countabsent2++; countabsent++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Red;
                                            counttotalabsentmorn++;
                                        }
                                        if (atteve.ToString() == "A")
                                        {
                                            countabsenteve++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                            counttotalabsenteven++;
                                        }
                                        totalabsent = countabsent2 + countabsenteve;
                                        totalabsent = totalabsent / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text = totalabsent.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].HorizontalAlign = HorizontalAlign.Center;

                                        if (str1 == "LA")
                                        {
                                            countlate2++;
                                            countlate++; totalmornlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.DarkRed;
                                        }
                                        if (atteve.ToString() == "LA")
                                        {
                                            totallatecount++;
                                            countlateeve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.DarkRed;
                                        }
                                        totallate = countlate2 + countlateeve;
                                        totallate = totallate / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 9].Text = totallate.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 9].HorizontalAlign = HorizontalAlign.Center;

                                        if (str1 == "PER")
                                        {
                                            countpermission2++;
                                            countpermission++; totalpermorn++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        if (atteve.ToString() == "PER")
                                        {
                                            countpermissioneve++; totalpereven++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                        totalpermission = countpermission2 + countpermissioneve;
                                        totalpermission = totalpermission / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 10].Text = totalpermission.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                                // habsent++;

                                //fpbiomatric.Sheets[0].Columns[0].Visible = false;
                                //fpbiomatric.Sheets[0].Columns[1].Visible = false;
                                //fpbiomatric.Sheets[0].Columns[2].Visible = false;
                                //fpbiomatric.Sheets[0].Columns[3].Visible = false;
                                //fpbiomatric.Sheets[0].Columns[4].Visible = false;
                                //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;
                                //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;
                                //fpbiomatric.Sheets[0].Columns[colcount].Visible = false;


                                fpbiomatric.Sheets[0].Columns[4].Visible = false;
                                fpbiomatric.Sheets[0].Columns[0].Visible = true;
                                if (count != 0)
                                {
                                    if (cblsearch.Items[0].Selected == true)
                                    {
                                        fpbiomatric.Sheets[0].Columns[1].Visible = true;
                                    }
                                    if (cblsearch.Items[1].Selected == true)
                                    {
                                        fpbiomatric.Sheets[0].Columns[2].Visible = true;
                                    }
                                    if (cblsearch.Items[2].Selected == true)
                                    {
                                        fpbiomatric.Sheets[0].Columns[3].Visible = true;
                                    }

                                }
                            }
                        }

                    }
                }
                lblpresent1.Text = ":" + counttotalmornpresent;
                lblpresent2.Text = ":" + counttotalevennpresent;
                lblabsent1.Text = ":" + counttotalabsentmorn;
                lblabsent2.Text = ":" + counttotalabsenteven;
                //lblpermission.Text = ":" + totalpermorn;//totalcountevennpermission;
                //lblpermission1.Text = ":" + totalpereven;
                //lbllate.Text = ":" + totalmornlate;//totallatecount;
                //lbllate1.Text = ":" + totalevenlate;
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    txtexcel.Text = "";
                    lblerror.Visible = false;
                    lblerror.Text = "";
                    lblexcel.Visible = false;
                    txtexcel.Visible = false;
                    btnexcel.Visible = false;
                    lblpresent1.Text = "0";
                    lblpresent2.Text = "0";
                    lblabsent1.Text = "0";
                    lblabsent2.Text = "0";
                    lbllate.Text = "0";
                    lbllate1.Text = "0";
                    return;
                }
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Present:" + Convert.ToString(counttotalmornpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Present:" + Convert.ToString(counttotalevennpresent);


                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Absent:" + counttotalabsentmorn.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Absent:" + counttotalabsenteven.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                //22.04.16
                //int counttotal = counttotalmornpresent + counttotalevennpresent + counttotalabsentmorn + counttotalabsenteven + totalcountevennpermission + totallatecount;// +countabsent;
                //fpbiomatric.Sheets[0].RowCount++;

                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                //fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of  Present in hostel:" + counttotal.ToString();
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                //int noofabsent = ureg - reg - hpresent;

                //fpbiomatric.Sheets[0].RowCount++;

                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                //fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of  Absent in hostel:" + noofabsent.ToString();
                //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                //end

                Double totalRows = 0;
                totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                if (totalRows >= 10)
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                    fpbiomatric.Height = 350;
                    fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows == 0)
                {

                    fpbiomatric.Height = 300;
                }
                else
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                    fpbiomatric.Height = 75 + (75 * Convert.ToInt32(totalRows));
                }

                //fpbiomatric.Height = 150 + (fpbiomatric.Sheets[0].RowCount * 10);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);

                fpbiomatric.Visible = true;
                lblnorec.Visible = false;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                ViewState["Bothpresent"] = null;
                ViewState["BothAbsent"] = null;
                ViewState["Bothod"] = null;
                ViewState["Bothper"] = null;
                Str = "";
                #endregion
            }
            else if (rbdailylog.Checked == true)
            {
                #region Daily logs Student
                attfiltertype.Visible = false;
                lbllate1.Visible = false;
                lblmornper.Visible = false;
                lblevenper.Visible = false;

                lbllatetext.Visible = false;
                lblheaderabsent1.Visible = false;
                lblheaderabsent2.Visible = false;
                imgabsent.Visible = false;
                lblabsent1.Visible = false;
                lblabsent2.Visible = false;
                imgpresent.Visible = false;
                imglate.Visible = false;
                lblmornlate.Visible = false;
                lblevenlate.Visible = false;
                imgper.Visible = false;
                //lblmornper.Visible = true;
                //lblevenper.Visible = true;
                //imgontime.Visible = false;
                //lblontime.Visible = false;

                lblpermission.Visible = false;
                lblpermission1.Visible = false;
                lblpresent1.Visible = false;
                lblpresent2.Visible = false;
                lbl_headermorn.Visible = false;
                lbl_headereven.Visible = false;
                lbllate.Visible = false;
                Hashtable date_count_01 = new Hashtable();
                ArrayList binddate = new ArrayList();

                fpbiomatric.Sheets[0].RowCount = 0;
                fpbiomatric.Sheets[0].RowHeader.Visible = false;
                fpbiomatric.Sheets[0].AutoPostBack = false;
                fpbiomatric.CommandBar.Visible = false;
                fpbiomatric.Visible = false;
                fpbiomatric.Sheets[0].ColumnHeader.RowCount = 2;
                fpbiomatric.Sheets[0].ColumnCount = 6;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].DefaultStyle.Locked = true;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;


                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                fpbiomatric.Columns[4].Visible = false;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Room No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                fpbiomatric.Columns[5].Visible = false;

                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                fpbiomatric.Sheets[0].Columns[0].Width = 40;
                fpbiomatric.Sheets[0].Columns[1].Width = 120;

                if (cblsearch.Items[0].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[1].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[1].Visible = false;
                }

                if (cblsearch.Items[1].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[2].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[2].Visible = false;
                }


                if (cblsearch.Items[2].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[3].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[3].Visible = false;
                }
                fpbiomatric.Sheets[0].GridLineColor = Color.Black;
                fpbiomatric.Height = 600;
                fpbiomatric.Width = 1000;
                fpbiomatric.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].DefaultStyle.Font.Bold = false;
                fpbiomatric.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].DefaultStyle.Border.BorderSizeBottom = 0;
                fpbiomatric.Sheets[0].DefaultStyle.Border.BorderSizeRight = 0;

                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[2].ToString() + "-" + split[1].ToString() + "-" + split[0].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[2].ToString() + "-" + split1[1].ToString() + "-" + split1[0].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = " and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {

                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }
                ArrayList columnhide = new ArrayList();

                string addtionam = "";

                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }

                //sql = "SELECT distinct rm.room_name,h.HostelMasterPK , h.hostelname, T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree,CONVERT(VARCHAR, T.Fingerprint1 ) as Fingerprint1,T.Roll_No FROM HT_HostelRegistration R,  Registration T,Degree G,Course C, HM_HostelMaster  h,Department D,bio..Daily_Logs B,room_detail rm Where r.APP_No = T.App_No And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and   h.HostelMasterPK =r.HostelMasterFK  and convert(nvarchar(100),b.FINGERPRINTDETAILS)=convert(nvarchar(100),t.Fingerprint1)   and  B.DATE between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' and isnull(r.IsDiscontinued ,0)=0 and isnull(r.IsVacated ,0)=0 ";


                //Cmd Saranyadevi 4.2.2019
                //sql = " SELECT distinct  T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree,CONVERT(VARCHAR, T.finger_id ) as Fingerprint1,T.Roll_No FROM  Registration T,Degree G,Course C,Department D,Daily_Logs B,attendance a Where  T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID  AND G.Dept_Code = D.Dept_Code  and a.Att_App_no=t.App_No and  convert(nvarchar(100), b.FingerID )=convert(nvarchar(100),t.finger_id)  and  B.Log_Date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "'  and t.college_code in('" + colegecode + "') AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'";

                sql = " SELECT distinct  T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree,CONVERT(VARCHAR, T.finger_id ) as Fingerprint1,T.Roll_No,isnull(T.Smart_BiouserID,'')Smart_BiouserID FROM  Registration T,Degree G,Course C,Department D,Daily_Logs B,attendance a Where  T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID  AND G.Dept_Code = D.Dept_Code  and a.Att_App_no=t.App_No and  (convert(nvarchar(100), b.FingerID )=convert(nvarchar(100),t.finger_id) or T.Smart_BiouserID=b.FingerID )  and  B.Log_Date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "'  and t.college_code in('" + colegecode + "') AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'";
                string courseid = string.Empty;
                string batchyr = string.Empty;
                string branch = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseid + "')";
                    addtionam = addtionam + " AND C.Course_id in('" + courseid + "')";
                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyr = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyr + "')";
                    addtionam = addtionam + " AND T.Batch_Year in('" + batchyr + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branch = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branch + "')";
                    addtionam = addtionam + " AND G.Degree_code in('" + branch + "')";
                }
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}
                //if (cboroll.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND T.App_No ='" + cboroll.SelectedItem.Value + "'";
                //    addtionam = addtionam + "  AND T.App_No ='" + cboroll.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //    addtionam = addtionam + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND G.Degree_code='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //    addtionam = addtionam + " AND T.Degree_code='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}


                //if (Chktimein.Checked == true)
                //{

                //    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                //    sql = sql + " " + strTime + "";
                //}
                //else if (Chktimeout.Checked == true)
                //{

                //    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                //    sql = sql + " " + strTime + "";
                //}

                fpbiomatric.Sheets[0].RowCount = 0;
                DataSet bioattend = new DataSet();
                DataSet biofinger = new DataSet();
                DataSet bioattcount = new DataSet();
                bioattend = d2.select_method_wo_parameter(sql, "Test");
                string access_date = "";
                int sno = 0;
                string empty_date = "";
                string org_date = "";
                int columndatecount = 0;
                int findbigrepeatcount = 0;
                int biorepeatcount = 0;

                for (int i = 0; i < bioattend.Tables[0].Rows.Count; i++)
                {
                    fpbiomatric.Visible = true;
                    findbigrepeatcount = 0;
                    sno++;
                    fpbiomatric.Sheets[0].RowCount++;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = bioattend.Tables[0].Rows[i]["Roll_No"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Tag = bioattend.Tables[0].Rows[i]["Fingerprint1"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Note = bioattend.Tables[0].Rows[i]["Roll_No"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;


                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].Text = bioattend.Tables[0].Rows[i]["Stud_Name"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].Tag = bioattend.Tables[0].Rows[i]["Smart_BiouserID"].ToString();

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 3].Text = bioattend.Tables[0].Rows[i]["Degree"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                    //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 4].Text = bioattend.Tables[0].Rows[i]["HostelName"].ToString();
                    //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    //fpbiomatric.Columns[4].Visible = false;

                    //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 5].Text = bioattend.Tables[0].Rows[i]["room_name"].ToString();
                    //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    //fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                    //fpbiomatric.Columns[5].Visible = false;
                }

                for (DateTime dt = dt1; dt <= dt2; dt = dt.AddDays(1))
                {
                    //string strnum = d2.GetFunction(" SELECT distinct  COUNT(*) as count1,t.Roll_No,t.Stud_Name,b.FINGERPRINTDETAILS,t.Roll_Admit FROM HT_HostelRegistration R, Registration T,bio..Daily_Logs B Where r.APP_No = T.App_No and convert(nvarchar(100),b.FINGERPRINTDETAILS)=convert(nvarchar(100),t.Fingerprint1) and B.DATE ='" + dt.ToString("MM/dd/yyyy") + "' and isnull(r.IsVacated ,0)=0 and isnull(r.IsDiscontinued ,0)=0 and r.BuildingFK<>'' and MemType=1 " + addtionam + " group by t.Roll_No,t.Stud_Name,b.FINGERPRINTDETAILS,t.Roll_Admit order by count1 desc");

                    //string strnum = d2.GetFunction("SELECT distinct  COUNT(*) as count1,t.Roll_No,t.Stud_Name,b.FingerID  as FINGERPRINTDETAILS,t.Roll_Admit FROM Registration T,Daily_Logs B,attendance a Where   a.Att_App_no=t.App_No and convert(nvarchar(100),b.FingerID)=convert(nvarchar(100),t.finger_id) and B.Log_Date ='" + dt.ToString("MM/dd/yyyy") + "' and t.college_code in('"+colegecode+"') " + addtionam + "  group by t.Roll_No,t.Stud_Name,b.FingerID,t.Roll_Admit order by count1 desc");

                    string strnum1 = d2.GetFunction("SELECT distinct  COUNT(distinct t.Roll_No) as count1,t.Roll_No,t.Stud_Name,b.FingerID  as FINGERPRINTDETAILS,t.Roll_Admit FROM Registration T,Degree G,Course C,Department D,Daily_Logs B,attendance a Where  T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID  AND G.Dept_Code = D.Dept_Code  and a.Att_App_no=t.App_No and  convert(nvarchar(100), b.FingerID )=convert(nvarchar(100),t.finger_id) and B.Log_Date ='" + dt.ToString("MM/dd/yyyy") + "' and t.college_code in('" + colegecode + "') " + addtionam + "  group by t.Roll_No,t.Stud_Name,b.FingerID,t.Roll_Admit order by count1 desc");





                    //string f = "SELECT distinct  COUNT(distinct t.Roll_No) as count1,t.Roll_No,t.Stud_Name,b.FingerID  as FINGERPRINTDETAILS,t.Roll_Admit FROM Registration T,Degree G,Course C,Department D,Daily_Logs B,attendance a Where  T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID  AND G.Dept_Code = D.Dept_Code  and a.Att_App_no=t.App_No and  convert(nvarchar(100), b.FingerID )=convert(nvarchar(100),t.finger_id) and B.Log_Date ='" + dt.ToString("MM/dd/yyyy") + "' and t.college_code in('" + colegecode + "') " + addtionam + "  group by t.Roll_No,t.Stud_Name,b.FingerID,t.Roll_Admit order by count1 desc";

                    string strnum2 = d2.GetFunction("SELECT distinct  COUNT(distinct t.Roll_No) as count1,t.Roll_No,t.Stud_Name,isnull(T.Smart_BiouserID,'')Smart_BiouserID,t.Roll_Admit FROM Registration T,Degree G,Course C,Department D,Daily_Logs B,attendance a Where  T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID  AND G.Dept_Code = D.Dept_Code  and a.Att_App_no=t.App_No and T.Smart_BiouserID=b.FingerID and B.Log_Date ='" + dt.ToString("MM/dd/yyyy") + "' and t.college_code in('" + colegecode + "') " + addtionam + "  group by t.Roll_No,t.Stud_Name,Smart_BiouserID,t.Roll_Admit order by count1 desc");


                  
                    string strnum = "";

                    int num1 = 0; int num2 = 0;
                    int.TryParse(strnum1, out num1);
                    int.TryParse(strnum2, out num2);
                    int punum = num1 + num2;
                    strnum = Convert.ToString(punum);


                    int k = 0;
                    if (strnum.Trim() != "" && strnum != null && strnum.Trim() != "0")
                    {
                        k++;
                        int colcou = Convert.ToInt32(strnum);
                        int stcol = fpbiomatric.Sheets[0].ColumnCount++;
                        //fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 1;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Text = dt.ToString("dd/MM/yyyy");
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Font.Size = FontUnit.Medium;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Font.Name = "Book Antiqua";
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Font.Bold = true;
                        string dat = Convert.ToString(dt.ToString("MM/dd/yyyy"));
                        binddate.Add(dat);
                        //date_count_01.Add(dat, colcou);
                        int pounch = 0;
                        for (int hst = 0; hst < colcou; hst++)
                        {
                            pounch = pounch + 1;
                            //if (hst > 0)
                            //{
                            //    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 1;
                            //}
                            //else
                            //{
                            //    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount++;
                            //}

                            if (!date_count_01.ContainsKey(dat))
                            {
                                date_count_01.Add(dat, fpbiomatric.Sheets[0].ColumnCount);
                            }
                            else
                            {
                                fpbiomatric.Sheets[0].ColumnCount++;
                            }
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Punch " + (pounch).ToString();
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Punch " + (pounch - 1).ToString();
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        }

                        //string strbioattedetails = "SELECT distinct CONVERT(VARCHAR, b.DATE, 103) as Access_Date, right(CONVERT(nvarchar(100),INTIME ,100),7) as Time_In,right(CONVERT(nvarchar(100),OUTTIME ,100),7) as Time_Out,T.Roll_No FROM HT_HostelRegistration R,Registration T,HM_HostelMaster h,bio..Daily_Logs B Where r.APP_No = T.App_No and h.HostelMasterPK =r.HostelMasterFK   and convert(nvarchar(100),b.FINGERPRINTDETAILS)=convert(nvarchar(100),t.Fingerprint1) and  B.DATE='" + dt.ToString("MM/dd/yyyy") + "' and isnull(r.IsVacated ,0)=0 and isnull(r.IsSuspend ,0)=0  " + addtionam + " order by t.Roll_No,Time_In";
                        //string strbioattedetails = "SELECT distinct CONVERT(VARCHAR, b.Log_Date , 103) as Access_Date, T.Roll_No FROM HT_HostelRegistration R,Registration T,HM_HostelMaster h,Daily_Logs B Where r.APP_No = T.App_No and h.HostelMasterPK =r.HostelMasterFK   and convert(nvarchar(100),b.FingerID )=convert(nvarchar(100),t.Fingerprint1) and  B.Log_Date='" + dt.ToString("MM/dd/yyyy") + "' and isnull(r.IsVacated ,0)=0 and isnull(r.IsSuspend ,0)=0  " + addtionam + " order by t.Roll_No";

                        //DataSet dsbioattdetails = d2.select_method_wo_parameter(strbioattedetails, "Text");
                        //for (int i = 0; i < fpbiomatric.Sheets[0].Rows.Count; i++)
                        //{
                        //    string roll_no = fpbiomatric.Sheets[0].Cells[i, 1].Note.ToString();
                        //    dsbioattdetails.Tables[0].DefaultView.RowFilter = "Roll_No='" + roll_no + "'";
                        //    DataView dvstu = dsbioattdetails.Tables[0].DefaultView;
                        //    int setva = stcol - 1;
                        //    for (int d = 0; d < dvstu.Count; d++)
                        //    {

                        //        setva = setva + 2;
                        //        //02.05.16
                        //        //string getintime= dvstu[d]["Time_In"].ToString();
                        //        //string getouttime = dvstu[d]["Time_Out"].ToString();

                        //        string getintime = dvstu[d]["Time_Out"].ToString();
                        //        string getouttime = dvstu[d]["Time_In"].ToString();

                        //        fpbiomatric.Sheets[0].Cells[i, setva - 1].Text = getintime;
                        //        fpbiomatric.Sheets[0].Cells[i, setva].Text = getouttime;
                        //    }
                        //}
                        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, stcol, 1, (colcou));//* 2
                    }
                }

                biofinger.Clear();
                string fpdate = ""; int fbcl = 0;
                for (int k = 0; k < fpbiomatric.Sheets[0].RowCount; k++)
                {

                    int fpcount = Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount);
                    string fingerid = fpbiomatric.Sheets[0].Cells[k, 1].Tag.ToString();
                    string biouserid = fpbiomatric.Sheets[0].Cells[k, 2].Tag.ToString();
                    ArrayList logtime = new ArrayList(); int fbcol = 0;
                    for (int s = 0; s < binddate.Count; s++)
                    {
                        fpdate = binddate[s].ToString();
                        string[] convert_fpdate = fpdate.Split('-');

                        sql = "  select * from Daily_Logs where FingerID='" + fingerid + "'  and Log_Date='" + binddate[s].ToString() + "' order by cast(LogTime as datetime)";// desc";
                        biofinger = d2.select_method_wo_parameter(sql, "Text");
                        int bind_columcount = Convert.ToInt32(date_count_01[fpdate]);
                        Boolean column = false;
                        for (int z = 0; z < biofinger.Tables[0].Rows.Count; z++)
                        {
                            string logss = biofinger.Tables[0].Rows[z]["LogTime"].ToString();
                            if (z != 0)
                            {
                                bind_columcount++;
                            }
                            fpbiomatric.Sheets[0].Cells[k, bind_columcount - 1].Text = logss.ToString();
                            //  logtime.Add(logss);
                            column = true;
                        }

                        //Mess Smart Card
                        int bind_columcount1 = 0;
                        if (!column)
                            bind_columcount1 = Convert.ToInt32(bind_columcount);
                        else
                            bind_columcount1 = Convert.ToInt32(bind_columcount + 1);
                        sql = "  select * from Daily_Logs where FingerID='" + biouserid + "'  and Log_Date='" + binddate[s].ToString() + "'  " + strTime + " order by cast(LogTime as datetime)";// desc";
                        biofinger = d2.select_method_wo_parameter(sql, "Text");

                        for (int z = 0; z < biofinger.Tables[0].Rows.Count; z++)
                        {
                            string logss = biofinger.Tables[0].Rows[z]["LogTime"].ToString();
                            if (z != 0)
                            {
                                bind_columcount1++;
                            }
                            fpbiomatric.Sheets[0].Cells[k, bind_columcount1 - 1].Text = logss.ToString();
                            fpbiomatric.Sheets[0].Cells[k, bind_columcount1 - 1].BackColor = Color.Chocolate;
                            //  logtime.Add(logss);
                        }
                    }
                }
                fpbiomatric.Sheets[0].PageSize = fpbiomatric.Sheets[0].RowCount;
                fpbiomatric.Sheets[0].FrozenColumnCount = 4;
                for (int k = 0; k < columnhide.Count; k++)
                {
                    int val_hide = Convert.ToInt32(columnhide[k].ToString());
                    fpbiomatric.Sheets[0].Columns[val_hide].Visible = false;
                }
                ArrayList abspersent = new ArrayList();
                biorepeatcount = fpbiomatric.Sheets[0].ColumnCount;
                fpbiomatric.Sheets[0].ColumnCount++;
                abspersent.Add(biorepeatcount);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Text = "Absent";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, biorepeatcount, 2, 1);
                biorepeatcount = fpbiomatric.Sheets[0].ColumnCount;
                fpbiomatric.Sheets[0].ColumnCount++;
                abspersent.Add(biorepeatcount);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Text = "Present";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, biorepeatcount, 2, 1);
                fpbiomatric.Sheets[0].PageSize = fpbiomatric.Sheets[0].RowCount;
                int totalpreeesent = 0;
                int totalabbsent = 0;
                countpermission2 = 0;
                countlate2 = 0;
                for (int mm = 0; mm < fpbiomatric.Sheets[0].RowCount; mm++)
                {
                    string rollno = fpbiomatric.Sheets[0].Cells[mm, 1].Note.ToString();
                    for (int s = 0; s < binddate.Count; s++)
                    {
                        countpresent2 = 0;
                        countabsent2 = 0;
                        string datetagvalue;
                        string startsem_date = string.Empty;
                        datetagvalue = binddate[s].ToString();
                        strdate = " and  access_date='" + datetagvalue + "'";
                        string[] monyeararr = datetagvalue.Split('/');
                        int year = Convert.ToInt16(monyeararr[2]);
                        int month = Convert.ToInt32(monyeararr[0]);
                        int monyear = Convert.ToInt16(monyeararr[0]) + year * 12;
                        int day10 = Convert.ToInt16(monyeararr[1]);

                        bioattcount.Clear();
                        string sql2 = "select * from attendance where month_year='" + monyear + "' and roll_no='" + rollno + "'";
                        bioattcount = d2.select_method_wo_parameter(sql2, "Text");
                        int catt = bioattcount.Tables[0].Rows.Count;
                        if (catt > 0)
                        {

                            string no_of_hrs = "";
                            int no_hrs = 0;
                            string strquery = " select distinct No_of_hrs_per_day FROM registration r, Department d ,PeriodAttndSchedule p  ,seminfo s WHERE r.degree_code=p.degree_code and r.Batch_Year in('" + batchyr + "') and r.degree_code in('" + branch + "')";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(strquery, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {

                                no_of_hrs = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                            }
                            if (no_of_hrs.Trim() != "")
                            {
                                no_hrs = Convert.ToInt16(no_of_hrs);
                            }
                            else
                            {
                                no_hrs = 0;
                            }

                            string str1 = "";
                            string lasthr = "d" + no_hrs;
                            string attmark = ""; string attmarkeve = ""; string atteve = "";
                            attmark = bioattcount.Tables[0].Rows[0]["d" + day10 + "d1"].ToString();
                            attmarkeve = bioattcount.Tables[0].Rows[0]["d" + day10 + lasthr].ToString();
                            atteve = Attmark(attmarkeve);

                            string[] splitatt = attmark.Split('-');
                            attmark = splitatt[0];
                            str1 = Attmark(attmark);


                            if (str1 == "P" || atteve == "P")
                            {
                                countpresent2++;
                                totalpreeesent++;
                            }
                            if (str1 == "A" || atteve == "A")
                            {
                                countabsent2++;
                                totalabbsent++;
                            }
                            if (str1 == "LA")
                            {
                                countlate2++;
                            }
                            if (str1 == "PER")
                            {
                                countpermission2++;
                            }
                        }
                    }
                    fpbiomatric.Sheets[0].Cells[mm, Convert.ToInt32(abspersent[0].ToString())].Text = Convert.ToString(countpresent2);
                    fpbiomatric.Sheets[0].Cells[mm, Convert.ToInt32(abspersent[1].ToString())].Text = Convert.ToString(countabsent2);
                }

                lbllate.Visible = false;
                lblpermission.Visible = false;
                lblpermission1.Visible = false;
                //lblontime.Visible = false;
                imglate.Visible = false;
                lblmornlate.Visible = false;
                lblevenlate.Visible = false;
                imgper.Visible = false;
                lblmornper.Visible = false;
                lblevenper.Visible = false;
                Fieldset1.Visible = true;
                Fieldset2.Visible = true;
                //imgontime.Visible = false;
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    return;
                }
                #endregion
            }

        }
        catch
        {
        }

    }

    public string Attmark(string Attstr_mark)
    {
        string Att_mark;
        Att_mark = "";
        if (Attstr_mark == "1")
        {
            Att_mark = "P";
        }
        else if (Attstr_mark == "2")
        {
            Att_mark = "A";
        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";
        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "L";
        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "S";
        }
        else if (Attstr_mark == "6")
        {
            Att_mark = "PER";
        }
        else if (Attstr_mark == "7")
        {
            Att_mark = "LA";
        }
        return Att_mark;
    }
    public string getfunction3(string sql)
    {
        string sqlstr1;
        sqlstr1 = sql;
        mycon.Close();
        mycon.Open();
        SqlCommand cmd10 = new SqlCommand(sqlstr1, mycon);
        SqlDataReader dr10;
        dr10 = cmd10.ExecuteReader();
        while (dr10.Read())
        {
            if (dr10.HasRows == true)
            {
                string att = "";
                att = dr10["att"].ToString();
                if (att == "P")
                {
                    countpresent = countpresent + 1;
                }
                if (att == "A")
                {
                    countabsent = countabsent + 1;
                }
                lblpresent1.Text = ":" + countpresent;
                lblpresent2.Text = ":" + "0";

                //lblpresent.Visible = true;
                lblabsent1.Text = ":" + countabsent;
                lblabsent2.Text = ":" + "0";
                if (att == "LA")
                {
                    countlate = countlate + 1;
                }
                lbllate.Text = ":" + countlate;
                if (att == "PER")
                {
                    countpermission = countpermission + 1;
                }
                lblpermission.Text = ":" + countpermission;
            }
        }
        return "";
        dr10.Close();
        mycon.Close();
    }
    public string getfunction(string sql1)
    {
        string sqlstr;
        sqlstr = sql1;
        mycon.Close();
        mycon.Open();
        SqlCommand cmd10 = new SqlCommand(sqlstr, mycon);
        SqlDataReader dr11;
        dr11 = cmd10.ExecuteReader();
        dr11.Read();
        if (dr11.HasRows == true)
        {
            string gettimein = "";
            gettimein = dr11["intime"].ToString();
            return gettimein;
        }
        else
        {
            return "";
        }
    }

    protected void rdotoday_CheckedChanged(object sender, EventArgs e)
    {
        //rdoparticular.Checked = false;
        //rdodatebetween.Checked = false;
    }

    protected void Cbo_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        //load_branch();
    }
    protected void Cbo_Branch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void chktimebwt_CheckedChanged(object sender, EventArgs e)
    {
        Chktimein.Visible = true;
        Chktimeout.Visible = true;

        Chktimein.Visible = false;
        Chktimeout.Visible = false;
        cbo_hour2.Visible = false;
        cbo_min2.Visible = false;
        cbo_hours.Visible = false;
        cbo_min.Visible = false;
        cbo_hrtin.Visible = false;
        cbo_hrinto.Visible = false;
        cbo_mintimein.Visible = false;
        cbo_mininto.Visible = false;
        cbointo.Visible = false;
        cbo_in.Visible = false;
        cbo_sec.Visible = false;
        cbo_sec2.Visible = false;
        lbltoutto.Visible = false;
        lblto.Visible = false;
    }
    protected void Chktimein_CheckedChanged(object sender, EventArgs e)
    {
        if (Chktimein.Checked == true)
        {
            cbo_hrtin.Enabled = true;
            cbo_hrinto.Enabled = true;
            cbo_mintimein.Enabled = true;
            cbo_mininto.Enabled = true;
            cbointo.Enabled = true;
            cbo_in.Enabled = true;
            lblto.Enabled = true;
        }
        else
        {
            cbo_hrtin.Enabled = false;
            cbo_hrinto.Enabled = false;
            cbo_mintimein.Enabled = false;
            cbo_mininto.Enabled = false;
            cbointo.Enabled = false;
            cbo_in.Enabled = false;
            lblto.Enabled = false;
        }
    }
    protected void Chktimeout_CheckedChanged(object sender, EventArgs e)
    {
        if (Chktimeout.Checked == true)
        {
            cbo_hour2.Enabled = true;
            cbo_min2.Enabled = true;
            cbo_hours.Enabled = true;
            cbo_min.Enabled = true;
            cbo_sec.Enabled = true;
            cbo_sec2.Enabled = true;
            lbltoutto.Enabled = true;
        }
        else
        {
            cbo_hour2.Enabled = false;
            cbo_min2.Enabled = false;
            cbo_hours.Enabled = false;
            cbo_min.Enabled = false;
            cbo_sec.Enabled = false;
            cbo_sec2.Enabled = false;
            lbltoutto.Enabled = false;
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  load_rollno();
    }
    protected void ddlDegree_SelectedIndexChanged1(object sender, EventArgs e)
    {
        //ddlBranch.Items.Clear();
        //con.Open();

        ////  cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddlDegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
        //string sqldegree = "";
        //sqldegree = "select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code= " + Session["collegecode"] + "";

        //if (ddlDegree.SelectedItem.Text != "Select")
        //{
        //    sqldegree = sqldegree + " and degree.course_id= " + ddlDegree.SelectedValue.ToString() + "";
        //}
        //SqlDataAdapter da = new SqlDataAdapter(sqldegree, con);
        //con.Close();
        //con.Open();
        //DataSet ds = new DataSet();
        //da.Fill(ds);
        //ddlBranch.DataSource = ds;
        //ddlBranch.DataValueField = "degree_code";
        //ddlBranch.DataTextField = "dept_name";
        //ddlBranch.DataBind();
        //ddlBranch.Items.Insert(0, "Select");
        //con.Close();
        // load_rollno();

    }
    protected void ddlBranch_SelectedIndexChanged1(object sender, EventArgs e)
    {
        // load_rollno();

        if (!Page.IsPostBack == false)
        {
            ddlSemYr.Items.Clear();
        }
        try
        {
            //if ((ddlBranch.SelectedIndex != 0) && (ddlBranch.SelectedIndex > 0))
            //{

            //}
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }
    protected void Txtentryto_TextChanged(object sender, EventArgs e)
    {

    }
    protected void cbo_mininto_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #region Search
    protected void btnsearch_Click(object sender, ImageClickEventArgs e)
    {
        load_click();
    }
    #endregion

    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (SelectAll.Checked == true)
        {
            for (int i = 0; i < cbo_att.Items.Count; i++)
            {
                cbo_att.Items[i].Selected = true;
                TextBox1.Text = "Leave(" + (cbo_att.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbo_att.Items.Count; i++)
            {
                cbo_att.Items[i].Selected = false;
                //cbo_att.Items[i + 3].Selected = true;
                TextBox1.Text = "---Select---";
            }
        }

    }

    protected void cbo_att_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < cbo_att.Items.Count; i++)
        {
            if (cbo_att.Items[i].Selected == true)
            {

                value = cbo_att.Items[i].Text;
                code = cbo_att.Items[i].Value.ToString();
                batchcount = batchcount + 1;
                TextBox1.Text = "Leave(" + batchcount.ToString() + ")";
            }

        }

        if (batchcount == 0)
            TextBox1.Text = "---Select---";
        else
        {
            Label lbl = batchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = batchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(batchimg_Click);
        }
        batchcnt = batchcount;
    }

    public void batchimg_Click(object sender, ImageClickEventArgs e)
    {
        batchcnt = batchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cbo_att.Items[r].Selected = false;
        TextBox1.Text = "Leave(" + batchcnt.ToString() + ")";
        if (TextBox1.Text == "Leave(0)")
        {
            TextBox1.Text = "---Select---";
        }
    }

    public Label batchlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton batchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    protected void imgpresent_Click(object sender, ImageClickEventArgs e)
    {
        //presentclick = true;
        //absentclick = false;
        //if (rdoinandout.Checked == true)
        //{
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'P-%'";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-P'";
        //    }
        //    else
        //    {
        //        Str = " and att like 'P-P'";
        //    }
        //}
        //else if (rdoinonly.Checked == true)
        //{
        //    Str = " and att like '%-P'";
        //}
        //else if (rdooutonly.Checked == true)
        //{
        //    Str = " and att like 'P-%'";
        //}
        //else if (rdoboth.Checked == true)
        //{
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'P-%'";
        //        ViewState["Bothpresent"] = "1";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-P'";
        //        ViewState["Bothpresent"] = "1";
        //    }
        //    else
        //    {
        //        Str = " and att like 'P-P'";
        //        ViewState["Bothpresent"] = "1";
        //    }
        //}
        //load_click();
    }
    protected void imgabsent_Click(object sender, ImageClickEventArgs e)
    {
        //absentclick = true;
        //presentclick = false;
        //if (rdoinandout.Checked == true)
        //{
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'A-%'";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-A'";
        //    }
        //    else
        //    {
        //        Str = " and att like 'A-A'";
        //    }
        //}
        //else if (rdoinonly.Checked == true)
        //{
        //    Str = " and att like '%-A'";
        //}
        //else if (rdooutonly.Checked == true)
        //{
        //    Str = " and att like 'A-%'";
        //}
        //else if (rdounreg.Checked == true)
        //{
        //    Str = " and att like 'A-A'";
        //    ViewState["unreg"] = 1;
        //}
        //else if (rdoboth.Checked == true)
        //{
        //    ViewState["BothAbsent"] = "2";
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'A-%'";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-A'";
        //    }
        //    else
        //    {
        //        Str = " and att like 'A-A'";
        //    }
        //}

    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {


    }
    protected void imglate_Click(object sender, ImageClickEventArgs e)
    {
        if (rdoinandout.Checked == true)
        {
            if (rdb_morn.Checked == true)
            {
                Str = " and att like 'LA-%'";
            }
            else if (rdb_even.Checked == true)
            {
                Str = " and att like '%-LA'";
            }
            else
            {
                Str = " and att like 'LA-LA'";
            }
        }
        else if (rdoinonly.Checked == true)
        {
            Str = " and att like '%-LA'";
        }
        else if (rdooutonly.Checked == true)
        {
            Str = " and att like 'LA-%'";
        }
        else if (rdoboth.Checked == true)
        {
            ViewState["Bothod"] = "3";
            if (rdb_morn.Checked == true)
            {
                Str = " and att like 'LA-%'";
            }
            else if (rdb_even.Checked == true)
            {
                Str = " and att like '%-LA'";
            }
            else
            {
                Str = " and att like 'LA-LA'";
            }
        }

    }
    protected void imgpermission_Click(object sender, ImageClickEventArgs e)
    {
        if (rdoinandout.Checked == true)
        {
            if (rdb_morn.Checked == true)
            {
                Str = " and att like 'PER-%'";
            }
            else if (rdb_even.Checked == true)
            {
                Str = " and att like '%-PER'";
            }
            else
            {
                Str = " and att like 'PER-PER'";
            }
        }
        else if (rdoinonly.Checked == true)
        {
            Str = " and att like '%-PER'";
        }
        else if (rdooutonly.Checked == true)
        {
            Str = " and att like 'PER-%'";
        }
        else if (rdoboth.Checked == true)
        {
            ViewState["Bothper"] = "4";
            if (rdb_morn.Checked == true)
            {
                Str = " and att like 'PER-%'";
            }
            else if (rdb_even.Checked == true)
            {
                Str = " and att like '%-PER'";
            }
            else
            {
                Str = " and att like 'PER-PER'";
            }
        }
        load_click();
    }

    protected void CheckBoxselect_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void imgontime_Click(object sender, ImageClickEventArgs e)
    {
        Generalflag = false;
        ontimeflag = true;
        load_click();
        // load_btnclick();
    }

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    public void filteration()
    {
        string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");

        if (orderby_Setting == "")
        {
            order_by_var = "";
        }
        else
        {

            if (orderby_Setting == "0")
            {
                order_by_var = " order by len(T.Roll_No),T.Roll_No";
            }

            else if (orderby_Setting == "2")
            {
                order_by_var = " ORDER BY T.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                order_by_var = " order by len(T.Roll_No),T.Roll_No,T.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                order_by_var = " order by len(T.Roll_No),T.Roll_No";
            }
            else if (orderby_Setting == "1,2")
            {
                order_by_var = " ORDER BY T.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                order_by_var = " order by len(T.Roll_No),T.Roll_No";
            }
        }

        if (order_by_var.Trim().ToString() == "")
        {
            order_by_var = " order by len(T.Roll_No),T.Roll_No";
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 2;
        string degreedetails = "Daily Attendance Report";
        if (rdoinandout.Checked == true)
        {
            degreedetails = "Daily In And Out Attendance Report";
        }
        else if (rdoinonly.Checked == true)
        {
            degreedetails = "Daily In Only Attendance Report";
        }
        else if (rdooutonly.Checked == true)
        {
            degreedetails = "Daily Out Only Attendance Report";
        }
        else if (rdounreg.Checked == true)
        {
            degreedetails = "Daily Registered Attendance Report";
        }
        else if (rbdailylog.Checked == true)
        {
            degreedetails = "Daily Logs Attendance Report";
        }

        degreedetails = degreedetails + "@ Date-From:" + Txtentryfrom.Text + "To:" + Txtentryto.Text + ""; ;

        string pagename = "Biohostel_new.aspx";
        Printcontrol.loadspreaddetails(fpbiomatric, pagename, degreedetails);
        Printcontrol.Visible = true;


    }
    protected void rdchecked(object sender, EventArgs e)
    {
        fpbiomatric.Visible = false;
        lblpresent1.Text = "";
        lblpresent2.Text = "";
        lblabsent1.Text = "";
        lblabsent2.Text = "";
        imgabsent.Visible = false;
        lblheaderabsent1.Visible = false;
        lblheaderabsent2.Visible = false;
        lblabsent1.Visible = false;
        lblabsent2.Visible = false;
        imgpresent.Visible = false;
        imglate.Visible = false;
        lblmornlate.Visible = false;
        lblevenlate.Visible = false;

        imgper.Visible = false;
        lblmornper.Visible = false;
        lblevenper.Visible = false;
        lblpermission.Visible = false;
        lblpermission1.Visible = false;
        lblpresent1.Visible = false;
        lblpresent2.Visible = false;
        lbl_headermorn.Visible = false;
        lbl_headereven.Visible = false;
        lbllate.Visible = false;
        lbllate1.Visible = false;

        if (rdoinandout.Checked == true)
        {
            rdb_morn.Enabled = false;
            rdb_morn.Checked = false;
            rdb_even.Enabled = false;
            rdb_even.Checked = false;
            rdoboth1.Enabled = true;
            rdoboth1.Checked = true;
            TextBox1.Enabled = true;
        }
        else if (rdoinonly.Checked == true)
        {
            rdb_morn.Enabled = true;
            rdb_morn.Checked = true;
            rdb_even.Enabled = false;
            rdb_even.Checked = false;
            rdoboth1.Enabled = false;
            rdoboth1.Checked = false;
            TextBox1.Enabled = true;
        }
        else if (rdooutonly.Checked == true)
        {
            rdb_morn.Enabled = false;
            rdb_morn.Checked = false;
            rdb_even.Enabled = true;
            rdb_even.Checked = true;
            rdoboth1.Enabled = false;
            rdoboth1.Checked = false;
            TextBox1.Enabled = true;
        }
        else if (rdounreg.Checked == true)
        {
            cblsearch.Items[3].Attributes.Add("style", "display:none;");
            cblsearch.Items[4].Attributes.Add("style", "display:none;");
            cblsearch.Items[5].Attributes.Add("style", "display:none;");
            //lblmornlate.Visible = false;
            //lblevenlate.Visible = false;

            //imgpresent.Visible = false;
            //imgper.Visible = false;
            //lblmornper.Visible = false;
            //lblevenper.Visible = false;
            //lblpermission.Visible = false;
            //lblpermission1.Visible = false;
            //lblpresent1.Visible = false;
            //lblpresent2.Visible = false;
            //lbl_headermorn.Visible = false;
            //lbl_headereven.Visible = false;
            //lbllate.Visible = false;
            //lbllate1.Visible = false;
            //imglate.Visible = false;
            ////imgontime.Visible = false;
            ////lblontime.Visible = false;
            //imgabsent.Visible = true;
            //lblheaderabsent1.Visible = true;
            //lblheaderabsent2.Visible = true;
            rdb_morn.Enabled = false;
            rdb_morn.Checked = false;
            rdb_even.Enabled = false;
            rdb_even.Checked = false;
            rdoboth1.Enabled = false;
            rdoboth1.Checked = false;
            TextBox1.Enabled = false;
        }
        else
        {
            rdb_morn.Enabled = false;
            rdb_morn.Checked = false;
            rdb_even.Enabled = false;
            rdb_even.Checked = false;
            rdoboth1.Enabled = false;
            rdoboth1.Checked = false;
            TextBox1.Enabled = false;
        }
    }
    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";


            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_clg.DataSource = ds;
                cbl_clg.DataTextField = "collname";
                cbl_clg.DataValueField = "college_code";
                cbl_clg.DataBind();

                int count = 0;
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    cbl_clg.Items[i].Selected = true;
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        txt_college.Text = "College(" + Convert.ToString(cbl_clg.Items.Count) + ")";
                        cb_clg.Checked = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void cbl_clg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_college.Text = "--Select--";
            cb_clg.Checked = false;

            for (int i = 0; i < cbl_clg.Items.Count; i++)
            {
                if (cbl_clg.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_college.Text = "College(" + commcount.ToString() + ")";
                if (commcount == cbl_clg.Items.Count)
                {
                    cb_clg.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_clg_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_clg.Checked == true)
            {
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    cbl_clg.Items[i].Selected = true;
                }
                txt_college.Text = "College(" + (cbl_clg.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    cbl_clg.Items[i].Selected = false;
                }
                txt_college.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {

        alertpopwindow.Visible = false;

    }
    protected void btnexcel_Click(object sender, EventArgs e)
    {

        try
        {
            string reportname = txtexcel.Text;
            lblerror.Text = "";
            lblerror.Visible = false;

            if (reportname.ToString().Trim() != "")
            {

                d2.printexcelreport(fpbiomatric, reportname);

            }
            else
            {
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
        }
        catch
        {
        }
    }
    protected void cb_batchyear_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batchyear, cbl_batchyear, txt_batchyr, Lblbatch.Text, "--Select--");


    }
    protected void cbl_batchyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batchyear, cbl_batchyear, txt_batchyr, Lblbatch.Text);


    }
    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, Lbldegree.Text, "--Select--");
        bindbranch();

    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, Lbldegree.Text);
        bindbranch();

    }
    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_branch, cbl_branch, txtbranch, LblBranch.Text, "--Select--");
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_branch, cbl_branch, txtbranch, LblBranch.Text);
    }
    public void bindbranch()
    {
        cbl_branch.Items.Clear();
        con.Open();
        string degree = string.Empty;
        if (cbl_degree.Items.Count > 0)
            degree = rs.GetSelectedItemsValueAsString(cbl_degree);
        //  cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddlDegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
        string sqldegree = "";
        sqldegree = "select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code= " + Session["collegecode"] + "";

        if (txtbranch.Text != "--Select--")
        {
            sqldegree = sqldegree + " and degree.course_id in('" + degree + "')";
        }
        SqlDataAdapter da = new SqlDataAdapter(sqldegree, con);
        con.Close();
        con.Open();
        DataSet ds = new DataSet();
        da.Fill(ds);
        cbl_branch.DataSource = ds;
        cbl_branch.DataValueField = "degree_code";
        cbl_branch.DataTextField = "dept_name";
        cbl_branch.DataBind();

        con.Close();

    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
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
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst)
    {
        try
        {
            int sel = 0;
            int count = 0;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
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
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }

}
