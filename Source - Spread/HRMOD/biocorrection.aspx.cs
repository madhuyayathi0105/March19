using System;
using System.Collections;
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


public partial class biocorrection : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection mycon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection mysql1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection myatt = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());

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
    SqlDataAdapter da = new SqlDataAdapter();
    int rowstr;
    int getrow2;
    string strdept = "";
    string strcategory;
    int getrow;
    DataSet ds = new DataSet();
    DataSet dsbind = new DataSet();
    DataSet dset = new DataSet();
    string sql3 = "";
    int count = 0;
    Boolean flag_true = false;

    string staffdept = string.Empty;

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    int i = 0;
    string[] bloodvalue = new string[55];
    string[] bloodcode = new string[55];

    string[] seatvalue = new string[55];
    string[] seatcode = new string[50];
    int[] seatindex = new int[44];
    int[] bloodindex = new int[44];
    int checkseat = 0;
    int checkblood = 0;
    int j = 0;
    int countood = 0;
    static int bloodcnt = 0;
    static int seatcnt = 0;
    string sql;
    string sql1;
    string mysql;
    string strdate;
    string strdate1;
    //Added By Srinath 1/4/2013
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    DAccess2 d2 = new DAccess2();
    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            if (IsPostBack)
            {
                //PlaceHolderseattype.Controls.Clear();//Hided by Manikandan 09/08/2013

                checkseat = 0;
                j = 0;
                for (i = 0; i < cbldepttype.Items.Count; i++)
                {
                    if (cbldepttype.Items[i].Selected == true)
                    {
                        seatvalue[j] = cbldepttype.Items[i].Text;
                        checkseat = checkseat + 1;
                        seatcode[j] = cbldepttype.Items[i].Value.ToString();
                        seatindex[j] = i;
                        j++;
                    }
                }

                if ((checkseat + 1) == seatcnt)
                {
                    seatcnt = seatcnt - 2;
                }
                //Start====================Hided by Manikandan 09/08/2013==================
                //if ((ViewState["iseatcontrol"] != null))
                //{
                //    for (int i = 0; i < seatcnt; i++)
                //    {
                //        if (ViewState["lseatcontrol"] != null)
                //        {
                //            Label lbl = seatlabel();
                //            lbl.Text = " " + seatvalue[i] + " ";
                //            lbl.ID = "lbl1-" + seatcode[i].ToString();
                //        }
                //        ImageButton ib = seatimage();
                //        ib.ID = "imgbut1_" + seatcode[i].ToString();
                //        ib.CommandArgument = seatindex[i].ToString();
                //        ib.Click += new ImageClickEventHandler(seatimg_Click);
                //    }
                //}
                //PlaceHolderblood.Controls.Clear();
                //===================================End====================================
                checkblood = 0;
                j = 0;
                for (int i = 0; i < cblcategory.Items.Count; i++)
                {
                    if (cblcategory.Items[i].Selected == true)
                    {
                        bloodvalue[j] = cblcategory.Items[i].Text;
                        checkblood = checkblood + 1;
                        bloodcode[j] = cblcategory.Items[i].Value.ToString();
                        bloodindex[j] = i;
                        j++;
                    }
                }
                if ((checkblood + 1) == bloodcnt)
                {
                    bloodcnt = bloodcnt - 2;
                }
                //Start=====================Hided by Manikandan 09/08/2013===============
                //if ((ViewState["ibloodcontrol"] != null))
                //{
                //    for (int i = 0; i < bloodcnt; i++)
                //    {
                //        if (ViewState["lbloodcontrol"] != null)
                //        {
                //            Label lbl = bloodlabel();
                //            lbl.Text = " " + bloodvalue[i] + " ";
                //            lbl.ID = "lbl2-" + bloodcode[i].ToString();
                //        }
                //        ImageButton ib = bloodimage();
                //        ib.ID = "imgbut2_" + bloodcode[i].ToString();
                //        ib.CommandArgument = bloodindex[i].ToString();
                //        ib.Click += new ImageClickEventHandler(bloodimg_Click);
                //    }
                //}
                //========================End============================================
            }
            if (!IsPostBack)
            {
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
                fpbiomatric.Visible = false;
                btnprintmaster.Visible = false;
                fpbiomatric.Sheets[0].AutoPostBack = false;
                // load_dept();
                //load_category();
                // load_staffname();

                load_college();
                btnsave.Visible = false;//Added by Manikandan 08/08/2013
            }
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Bold = true;
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            // select distinct roll_no from bio_attendance where bio_attendance.access_date between '08/02/2011' and '08/02/2011'//
        }
        catch (Exception ex) { }
    }

    void load_college()
    {
        cblcollege.Visible = true;
        cblcollege.Items.Clear();
        ds.Clear();
        // ListItem lsitem = new ListItem();
        con.Open();
        SqlCommand cmd = new SqlCommand("select distinct college_code,collname from collinfo", con);
        da.SelectCommand = cmd;
        da.Fill(ds);
        cblcollege.DataSource = ds.Tables[0];
        cblcollege.DataTextField = "collname";
        cblcollege.DataValueField = "college_code";
        cblcollege.DataBind();


        con.Close();
        load_dept();
        load_category();
        load_staffname(staffdept);


    }
    void load_dept()
    {


        cbldepttype.Visible = true;
        cbldepttype.Items.Clear();
        ds.Clear();
        // ListItem lsitem = new ListItem();
        //Modified By Srinath 1/4/2013
        //con.Open();
        //SqlCommand cmd = new SqlCommand("select distinct dept_code,dept_name from hrdept_master where college_code='" + cblcollege.SelectedItem.Value.ToString() + "'", con);
        //da.SelectCommand = cmd;
        //da.Fill(ds);
        string deptquery = "";
        Hashtable hat = new Hashtable();
        string singleuser = Session["single_user"].ToString();
        if (singleuser == "True")
        {
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
        }
        else
        {
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
        }
        if (deptquery != "")
        {
            ds = d2.select_method(deptquery, hat, "Text");
            cbldepttype.DataSource = ds.Tables[0];
            cbldepttype.DataTextField = "dept_name";
            cbldepttype.DataValueField = "dept_code";
            cbldepttype.DataBind();
            //con.Close();
        }
    }
    void load_staffname(string staffdept)
    {
        cbostaffname.Items.Clear();
        string staff_cat = "";
        string staffcat_selected = "";
        string derpatment = "";
        cbostaffname.Items.Clear();
        for (int staf_cat = 0; staf_cat < cblcategory.Items.Count; staf_cat++)
        {
            if (cblcategory.Items[staf_cat].Selected == true)
            {
                if (staffcat_selected == "")
                {
                    staffcat_selected = "'" + cblcategory.Items[staf_cat].Value.ToString() + "'";
                }
                else
                {
                    staffcat_selected = staffcat_selected + "," + "'" + cblcategory.Items[staf_cat].Value.ToString() + "'";
                }
            }
        }

        if (staffdept != "")
        {
            derpatment = "and dept_code in (" + staffdept + ")";
        }

        if (staffdept == "")
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                if (cbldepttype.Items[i].Selected == true)
                {

                    if ((staffdept == "")) //Added by Manikandan
                    {
                        staffdept = "'" + cbldepttype.Items[i].Value.ToString() + "'";

                    }
                    else
                    {

                        staffdept = staffdept + "," + "'" + cbldepttype.Items[i].Value.ToString() + "'";
                    }
                    //-------------
                }
            }
        }
        if (staffdept != "")
        {
            derpatment = "and dept_code in (" + staffdept + ")";
        }

        if (staffcat_selected != "" && staffdept != "")
        {
            staff_cat = "and t.category_code in(" + staffcat_selected + ")";
        }

        ds.Clear();
        //ListItem lsitem = new ListItem();
        con.Close();
        con.Open();
        SqlCommand cmdstaff = new SqlCommand("Select distinct m.Staff_code,Staff_name from staffmaster m,stafftrans t where resign=0 and settled=0 and m.staff_code = t.staff_code " + staff_cat + " and t.latestrec = 1 " + derpatment + " and college_code='" + Session["collegecode"] + "' order by staff_name ", con);
        da.SelectCommand = cmdstaff;
        da.Fill(ds);
        cbostaffname.DataSource = ds.Tables[0];
        cbostaffname.DataTextField = "Staff_name";
        cbostaffname.DataValueField = "Staff_name";
        cbostaffname.DataBind();
        // lsitem.Text = "All";
        cbostaffname.Items.Insert(0, "All");
        con.Close();


    }
    void load_category()
    {

        cblcategory.Visible = true;
        cblcategory.Items.Clear();
        ds.Clear();
        // ListItem lsitem = new ListItem();
        con.Open();
        SqlCommand cmd = new SqlCommand("select distinct category_code,category_name from staffcategorizer where  college_code='" + cblcollege.SelectedItem.Value.ToString() + "'", con);
        da.SelectCommand = cmd;
        da.Fill(ds);
        cblcategory.DataSource = ds.Tables[0];
        cblcategory.DataTextField = "category_name";
        cblcategory.DataValueField = "category_code";
        cblcategory.DataBind();

        for (int i = 0; i < cblcategory.Items.Count; i++)
        {
            cblcategory.Items[i].Selected = true;
            tbblood.Text = "Category(" + i.ToString() + ")";
        }

        load_staffname(staffdept);
        con.Close();
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







    protected void btn_go_Click(object sender, EventArgs e)
    {
        //fpbiomatric.CommandBar.Visible = false;
        // Image8.Visible = false;
        //lblne.Visible = false;
        fpbiomatric.Sheets[0].PageSize = 11;
        fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
        fpbiomatric.Pager.Align = HorizontalAlign.Right;
        fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
        fpbiomatric.Pager.Font.Bold = true;
        fpbiomatric.Pager.Font.Name = "Arial";
        fpbiomatric.Pager.ForeColor = Color.DarkGreen;
        fpbiomatric.Pager.BackColor = Color.AliceBlue;
        fpbiomatric.Pager.PageCount = 5;
        fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        load_btnclick();
        fpbiomatric.Sheets[0].RowHeader.Cells[0, 0].Text = " ";//Added by Manikandan 09/08/2013
    }
    void load_btnclick()
    {
        lblnorec.Visible = false;
        fpbiomatric.Visible = true;
        btnprintmaster.Visible = true;
        fpbiomatric.Sheets[0].ColumnCount = 0;
        fpbiomatric.Sheets[0].RowCount = 0;
        fpbiomatric.Sheets[0].ColumnCount = 4;
        //fpbiomatric.Sheets[0].SheetCorner.RowCount = 9;//Hided by Manikandan 08/08/2013
        fpbiomatric.Sheets[0].SheetCorner.RowCount = 3;
        fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
        fpbiomatric.Sheets[0].RowCount = 1;

        //////////////////////////////////////////////////////
        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        fpbiomatric.Sheets[0].Columns[0, 0].Width = 50;
        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
        fpbiomatric.ActiveSheetView.Cells[0, 0].CellType = chkcell;

        //  chkcell = new FarPoint.Web.Spread.CheckBoxCellType(strsesfrom);


        chkcell.AutoPostBack = true;


        fpbiomatric.Sheets[0].Columns[0].CellType = chkcell;
        fpbiomatric.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        fpbiomatric.Sheets[0].SetColumnWidth(0, 50);
        /////////////////////////////////////////////////////////////////////////////
        fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        fpbiomatric.Sheets[0].SetColumnWidth(1, 250);
        //  fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Dept Acronym";
        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        fpbiomatric.Sheets[0].SetColumnWidth(1, 150);
        //fpbiomatric.Sheets[0].FrozenColumnCount = 4;
        // fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
        //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Desig Acronym";
        // fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Date Of Joinng";
        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Category";

        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        //Start======Hided by Manikandan 09/08/2013 
        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
        //==================End===================

        fpbiomatric.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
        fpbiomatric.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
        fpbiomatric.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
        fpbiomatric.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
        fpbiomatric.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
        fpbiomatric.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
        fpbiomatric.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;
        fpbiomatric.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";


        string datefrom;
        string date1;
        string dateto;
        string date2;
        string tempstaffcode = "";

        // fpbiomatric.Sheets[0].RowCount = 0;
        //fpbiomatric.Sheets[0].ColumnCount = 4;
        date1 = Txtentryfrom.Text.ToString();
        string[] split = date1.Split(new Char[] { '/' });
        datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
        date2 = Txtentryto.Text.ToString();
        string[] split1 = date2.Split(new Char[] { '/' });
        dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
        DateTime dt2 = Convert.ToDateTime(dateto.ToString());
        TimeSpan t = dt2.Subtract(dt1);
        long days = t.Days;

        fpbiomatric.Sheets[0].FrozenRowCount = 1;
        //fpbiomatric.Sheets[0].FrozenColumnCount = 4;

        //////////////////////////////////////////////////////////////////

        ArrayList al_reason = new ArrayList();
        con.Close();
        con.Open();

        SqlCommand cmd24 = new SqlCommand("select shortname from leave_category where college_code='" + cblcollege.SelectedItem.Value.ToString() + "'", con);
        SqlDataReader dr24 = cmd24.ExecuteReader();

        if (dr24.HasRows)
        {
            while (dr24.Read())
            {
                al_reason.Add(dr24.GetValue(0).ToString());
            }
        }
        dr24.Close();

        int h = 0;
        string[] reason = new string[al_reason.Count + 4];
        string[] reason1 = new string[al_reason.Count + 3];
        reason[0] = "Select for All";
        reason[1] = "";
        reason[2] = "P";
        reason[3] = "A";

        // reason1[0] = "";
        for (int r = 3; r <= al_reason.Count + 2; r++)
        {
            reason[r + 1] = al_reason[r - 3].ToString();
            //reason1[r] = al_reason[r - 1].ToString();
            h = r;
        }

        // mysql.Close();

        FarPoint.Web.Spread.ComboBoxCellType ddlleavetype = new FarPoint.Web.Spread.ComboBoxCellType(reason);

        ddlleavetype.ShowButton = true;
        ddlleavetype.AutoPostBack = true;
        ddlleavetype.UseValue = true;
        con.Close();

        ArrayList al_reason2 = new ArrayList();
        con.Close();
        con.Open();

        SqlCommand cmd25 = new SqlCommand("select shortname from leave_category where college_code='" + cblcollege.SelectedItem.Value.ToString() + "'", con);
        SqlDataReader dr25 = cmd25.ExecuteReader();

        if (dr25.HasRows)
        {
            while (dr25.Read())
            {
                al_reason2.Add(dr25.GetValue(0).ToString());
            }
        }
        dr24.Close();


        string[] reason3 = new string[al_reason.Count + 4];
        string[] reason4 = new string[al_reason.Count + 1];
        //reason3[0] = "";
        reason3[0] = "";


        reason3[1] = "P";
        reason3[2] = "A";

        // reason4[0] = "";
        for (int r = 2; r <= al_reason2.Count + 1; r++)
        {
            reason3[r + 1] = al_reason2[r - 2].ToString();
            //reason4[r] = al_reason2[r - 1].ToString();
        }
        // mysql.Close();

        FarPoint.Web.Spread.ComboBoxCellType ddlcell = new FarPoint.Web.Spread.ComboBoxCellType(reason3);

        /////////////////////////////////////////////////////

        //////////////////

        FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
        string[] strcomo1 = new string[] { "Select for All ", " ", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
        string[] strminfrom = new string[] { "Select for All", "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };
        string[] strsesfrom = new string[] { "Select For All", "AM", "PM" };


        FarPoint.Web.Spread.ComboBoxCellType objintcellsesfrm = new FarPoint.Web.Spread.ComboBoxCellType();
        objintcellsesfrm = new FarPoint.Web.Spread.ComboBoxCellType(strsesfrom);
        objintcellsesfrm.ShowButton = true;
        objintcellsesfrm.AutoPostBack = true;
        objintcellsesfrm.UseValue = true;

        FarPoint.Web.Spread.ComboBoxCellType objintcell1 = new FarPoint.Web.Spread.ComboBoxCellType();
        objintcell1 = new FarPoint.Web.Spread.ComboBoxCellType(strminfrom);
        objintcell1.ShowButton = true;
        objintcell1.AutoPostBack = true;
        objintcell1.UseValue = true;


        objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
        objintcell.ShowButton = true;
        objintcell.AutoPostBack = true;
        objintcell.UseValue = true;


        string[] strcomo2 = new string[] { "Select for All ", " ", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
        string[] strminto = new string[] { "Select for All", "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };
        string[] strsesto = new string[] { "Select For All", "AM", "PM" };


        FarPoint.Web.Spread.ComboBoxCellType objintcellsesto = new FarPoint.Web.Spread.ComboBoxCellType();
        objintcellsesto = new FarPoint.Web.Spread.ComboBoxCellType(strsesto);
        objintcellsesto.ShowButton = true;
        objintcellsesto.AutoPostBack = true;
        objintcellsesto.UseValue = true;



        FarPoint.Web.Spread.ComboBoxCellType objintcell2 = new FarPoint.Web.Spread.ComboBoxCellType();
        objintcell2 = new FarPoint.Web.Spread.ComboBoxCellType(strminto);
        objintcell2.ShowButton = true;
        objintcell2.AutoPostBack = true;
        objintcell2.UseValue = true;


        FarPoint.Web.Spread.ComboBoxCellType objintcell3 = new FarPoint.Web.Spread.ComboBoxCellType();

        objintcell3 = new FarPoint.Web.Spread.ComboBoxCellType(strcomo2);
        objintcell3.ShowButton = true;
        objintcell3.AutoPostBack = true;
        objintcell3.UseValue = true;

        fpbiomatric.SaveChanges();
        ///////////////
        // day3 = Convert.ToInt32(days);
        // day3 = day3 + 1;

        if ((datefrom != null) && (dateto != null))
        {
            if (rdoall.Checked == true)
            {
                strdate1 = "  bio_attendance.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];


                    lbldate.Visible = false;

                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 8;



                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Text = Txtentryfrom.Text.ToString();

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].HorizontalAlign = HorizontalAlign.Center;

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Tag = datefrom.ToString();

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].HorizontalAlign = HorizontalAlign.Center;
                    // fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].Text = "Time In";
                    //  fpbiomatric.Sheets[0].ColumnHeader.Cells[2, fpbiomatric.Sheets[0].ColumnCount - 2].Text = "Hours";
                    fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 2, 33);
                    fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    ///fpbiomatric.Sheets[0].ColumnHeader.Cells[7, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Time Out";
                    fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount 1, 33);


                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next Date
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 8;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Text = datevar;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].HorizontalAlign = HorizontalAlign.Center;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Tag = split12[0].ToString() + "/" + split12[1].ToString() + "/" + split12[2].ToString(); ;
                        // fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].Text = "Time Out";
                        //  fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;


                        // fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 33);
                        // fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Time In";
                        // fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 33);

                    }



                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }
                //////////////////////////////////////////////////////////////////////////////

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                fpbiomatric.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                fpbiomatric.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(darkstyle);
                fpbiomatric.Sheets[0].AllowTableCorner = true;
                //fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";Hided by Manikandan 08/08/2013


                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 7, 1);
                //Start=================Hided by Manikandan 08/08/2013============
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, fpbiomatric.Sheets[0].ColumnCount - 1, 6, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 9, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 8, 1);
                //=================End============================================



                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);//Hided by Manikandan 08/08/2013
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColor = Color.Black;
                fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 3, 1);




                ///////////////////////

                //Start=================Hided by Manikandan 08/08/2013=====================================
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 6].Border.BorderColor = Color.White;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColor = Color.White;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColor = Color.White;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColor = Color.White;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColor = Color.White;


                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 7].Border.BorderColor = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 7].Border.BorderColor = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 3].Border.BorderColor = Color.White;



                //string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + cblcollege.SelectedItem.Value.ToString() + "'";
                //con.Close();
                //con.Open();
                //SqlCommand comm = new SqlCommand(str, con);
                //SqlDataReader drr = comm.ExecuteReader();
                //drr.Read();
                //string coll_name = Convert.ToString(drr["collname"]);
                //string coll_address1 = Convert.ToString(drr["address1"]);
                //string coll_address2 = Convert.ToString(drr["address2"]);
                //string coll_address3 = Convert.ToString(drr["address3"]);
                //string pin_code = Convert.ToString(drr["pincode"]);

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = coll_name;


                //////////////////////////////////////////////////
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);

                ///////////////////////////////////////////////////////
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 1].Text = coll_address1;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, 1].Text = coll_address2;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, 1].Text = coll_address3 + "-" + pin_code;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Daily Attendance Report";
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);


                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].ForeColor = Color.FromArgb(64, 64, 255);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;


                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 1);

                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, 3);

                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(4, 4, 1, 5);

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Left;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;


                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].HorizontalAlign = HorizontalAlign.Right;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].Font.Size = FontUnit.Medium;

                //MyImg mi = new MyImg();
                //mi.ImageUrl = "../images/10BIT001.jpeg";
                //mi.ImageUrl = "Handler/Handler2.ashx?";
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 1].CellType = mi;
                //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 50);


                //fpbiomatric.Sheets[0].SetColumnWidth(0, 50);

                //fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[2].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[2].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[3].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[3].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[4].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[4].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[5].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[5].Font.Size = FontUnit.Medium;

                //==============================End=========================================

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);

                //Start==================Hided by Manikandan 08/08/2013===========================================
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(5, 4, 1, fpbiomatric.Sheets[0].ColumnCount - 5);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Text = "Date-From" + date1 + "To:" + date2 + "";
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Left;

                // fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 2].Text = date1+"To:"+date2+"";
                // fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 3].Text = "Category";
                //========================================End=====================================================

                string categry4 = "";
                for (int g = 0; g < cblcategory.Items.Count; g++)
                {
                    if (cblcategory.Items[g].Selected == true)
                    {
                        categry4 = categry4 + cblcategory.Items[g].Text + ",";
                    }
                }
                if (categry4 != "")
                {
                    categry4 = categry4.Substring(0, categry4.Length - 1);
                }
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 5].Text = categry4.ToString();
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].Text = "Category:" + categry4.ToString();//Hided by Manikandan 08/08/2013
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].HorizontalAlign = HorizontalAlign.Right;//Hided by Manikandan 08/08/2013

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].ColumnHeader.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].ColumnHeader.Rows[2].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //////////////////////////////////



                sql = "SELECT distinct staffmaster.staff_code , desig_master.desig_acronym, hrdept_master.dept_acronym,CONVERT(VARCHAR(10),staffmaster.join_date,103) as join_date,in_out_time.category_name,staffmaster.staff_name ,desig_name ,hrdept_master.dept_acronym as 'Dept Acronym',hrdept_master.dept_name ,desig_acronym,access_date as 'Entry Date',right(CONVERT(nvarchar(100),time_in ,100),7) as time_in ,right(CONVERT(nvarchar(100),time_out ,100),7) as time_out,convert(char(8),(cast(time_out as datetime) - cast(time_in as datetime)),108) as TotalHours, att  FROM staffmaster,stafftrans,hrdept_master, desig_master,bio_attendance,In_Out_Time,staff_attnd where  (staffmaster.Fingerprint1 Is Not Null or staffmaster.Fingerprint1 Is Not Null) and hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and   staffmaster.staff_code=bio_attendance.roll_no and is_staff=1 and hrdept_master.dept_code=stafftrans.dept_code and desig_master.desig_code=stafftrans.desig_code and staffmaster.settled = 0 And staffmaster.resign = 0  And In_Out_Time.Category_Code = Stafftrans.Category_Code And staffmaster.staff_code = stafftrans.staff_code And stafftrans.latestrec = 1 and in_out_time.shift = stafftrans.shift and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code=desig_master.collegecode and staffmaster.college_code='" + cblcollege.SelectedItem.Value.ToString() + "' and  " + strdate1 + " ";//this (in_out_time.shift = stafftrans.shift) condition added by Manikandan 21/08/2013

                if (tbseattype.Text != "---Select---")
                {
                    int itemcount = 0;


                    for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
                    {
                        if (cbldepttype.Items[itemcount].Selected == true)
                        {
                            if (strdept == "")
                                strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                            else
                                strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                        }
                    }


                    if (strdept != "")
                    {
                        strdept = " in(" + strdept + ")";
                        sql = sql + " and hrdept_master.dept_code " + strdept + "";
                    }
                }
                if (tbblood.Text != "---Select---")
                {

                    strcategory = "";
                    int itemcount1 = 0;

                    for (itemcount1 = 0; itemcount1 < cblcategory.Items.Count; itemcount1++)
                    {
                        if (cblcategory.Items[itemcount1].Selected == true)
                        {
                            if (strcategory == "")
                                strcategory = "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                            else
                                strcategory = strcategory + "," + "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                        }
                    }


                    if (strcategory != "")
                    {
                        strcategory = " in (" + strcategory + ")";
                        sql = sql + "  and stafftrans.category_code" + strcategory + "";
                    }
                }
                if (cbostaffname.SelectedItem.Value.ToString() != "All")
                {
                    sql = sql + " and staffmaster.staff_name='" + cbostaffname.SelectedItem.Value.ToString() + "'";
                }
                sql = sql + " order by staffmaster.staff_code";
                con1.Close();
                con1.Open();
                SqlDataReader drname;
                SqlCommand cmd2 = new SqlCommand(sql, con1);
                drname = cmd2.ExecuteReader();

                if (drname.HasRows == true)
                {
                    fpbiomatric.Visible = true;
                    btnprintmaster.Visible = true;
                    while (drname.Read())
                    {
                        btnsave.Visible = true;//Added by Manikandan 08/08/2013
                        btnsave.Enabled = true;

                        sql = "";
                        // Str = "";
                        string staffcode1;
                        string category8 = "";
                        string timein8 = "";

                        staffcode1 = drname["staff_code"].ToString();
                        category8 = drname["category_name"].ToString();
                        timein8 = drname["time_in"].ToString();


                        int countcolumn;
                        countcolumn = fpbiomatric.Sheets[0].ColumnCount;
                        for (int colcount = 4; colcount <= countcolumn - 1; colcount = colcount + 8)
                        {
                            //////////////////select cell

                            fpbiomatric.ActiveSheetView.Cells[0, colcount].CellType = ddlleavetype;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 1].CellType = ddlleavetype;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 2].CellType = objintcell;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 3].CellType = objintcell1;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 4].CellType = objintcellsesfrm;

                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 5].CellType = objintcell3;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 6].CellType = objintcell2;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 7].CellType = objintcellsesto;

                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 8);
                            fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";


                            //////////////////////////////////////////////////////////////////////////

                            //Start================Hided by Manikandan 08/08/2013=======================================
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 4].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;

                            //===================================End=====================================================
                            //////////////////////////////////////////////////////////////////////
                            string[] cbstrhrsin;
                            cbstrhrsin = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                            string[] cbstrminin;
                            cbstrminin = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };
                            string[] cbstrses;
                            cbstrses = new string[] { "AM", "PM" };
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel1 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrhrsin);
                            fpbiomatric.Sheets[0].Columns[colcount + 2].CellType = cmbcel1;
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel2 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrminin);
                            fpbiomatric.Sheets[0].Columns[colcount + 3].CellType = cmbcel2;
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel3 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrses);
                            fpbiomatric.Sheets[0].Columns[colcount + 4].CellType = cmbcel3;
                            fpbiomatric.Sheets[0].Columns[colcount + 5].CellType = cmbcel1;
                            fpbiomatric.Sheets[0].Columns[colcount + 6].CellType = cmbcel2;
                            fpbiomatric.Sheets[0].Columns[colcount + 7].CellType = cmbcel3;

                            fpbiomatric.ActiveSheetView.Columns[colcount].HorizontalAlign = HorizontalAlign.Center;

                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 4, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 7, 50);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 5, 40);

                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].Font.Name = "Book Antiqua";


                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "UnReg";

                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].Columns[colcount].CellType = ddlcell;
                            fpbiomatric.Sheets[0].Columns[colcount + 1].CellType = ddlcell;

                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount, 1, 2);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].Text = "Mor";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].Text = "Eve";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].HorizontalAlign = HorizontalAlign.Center;

                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "In";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount + 2, 1, 3);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].Text = "Hrs";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].Text = "Minu";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].Text = "Ses";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].HorizontalAlign = HorizontalAlign.Center;

                            //fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                            fpbiomatric.Sheets[0].Columns[colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Text = "Out";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount + 5, 1, 3);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].Text = "Hrs";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].Text = "Min";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].Text = "Ses";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].HorizontalAlign = HorizontalAlign.Center;

                            // fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);
                            string datetagvalue;
                            datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                            strdate = " and bio_attendance.access_date='" + datetagvalue + "'";

                            sql = "SELECT distinct staffmaster.staff_code ,desig_master.desig_acronym, hrdept_master.dept_acronym,CONVERT(VARCHAR(10),staffmaster.join_date,103) as join_date,in_out_time.category_name,staffmaster.staff_name ,desig_name ,hrdept_master.dept_acronym as 'Dept Acronym',hrdept_master.dept_name ,desig_acronym,access_date as 'Entry Date',right(CONVERT(nvarchar(100),time_in ,100),7) as time_in ,right(CONVERT(nvarchar(100),time_out ,100),7) as time_out,convert(char(8),(cast(time_out as datetime) - cast(time_in as datetime)),108) as TotalHours,att  FROM staffmaster,stafftrans,hrdept_master, desig_master,bio_attendance,In_Out_Time,staff_attnd where  (staffmaster.Fingerprint1 Is Not Null or staffmaster.Fingerprint1 Is Not Null) and hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and   staffmaster.staff_code=bio_attendance.roll_no and is_staff=1 and hrdept_master.dept_code=stafftrans.dept_code and desig_master.desig_code=stafftrans.desig_code and staffmaster.settled = 0 And staffmaster.resign = 0 and staffmaster.staff_code='" + staffcode1 + "' And In_Out_Time.Category_Code = Stafftrans.Category_Code And staffmaster.staff_code = stafftrans.staff_code And stafftrans.latestrec = 1 and in_out_time.shift = stafftrans.shift and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code=desig_master.collegecode  and staffmaster.college_code='" + cblcollege.SelectedItem.Value.ToString() + "' " + strdate + " ";//this (in_out_time.shift = stafftrans.shift) condition added by Manikandan 21/08/2013
                            con.Close();
                            con.Open();
                            SqlCommand cmd7 = new SqlCommand(sql, con);


                            SqlDataReader drcount14;
                            fpbiomatric.Width = 750;

                            drcount14 = cmd7.ExecuteReader();

                            while (drcount14.Read())
                            {
                                if (drcount14.HasRows == true)
                                {

                                    if (tempstaffcode == "")
                                    {

                                        fpbiomatric.Sheets[0].RowCount += 1;
                                        tempstaffcode = drcount14["staff_code"].ToString();
                                    }

                                    else if ((tempstaffcode != "") && (tempstaffcode != drcount14["staff_code"].ToString()))
                                    {
                                        fpbiomatric.Sheets[0].RowCount += 1;
                                        tempstaffcode = drcount14["staff_code"].ToString();
                                    }


                                    rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);
                                    Session["getrow"] = rowstr;
                                    string staffcode = "";
                                    string staffname = "";
                                    string dept_acronym = "";
                                    string category;
                                    string intime = "";
                                    string outtime = "";
                                    string horsin = "";
                                    string minin = "";
                                    string minin1 = "";
                                    string sesin = "";
                                    string horsto = "";
                                    string minto1 = "";
                                    string minto = "";
                                    string sesto = "";
                                    int hrint = 0;
                                    int htoutint = 0;
                                    string att = "";
                                    string mrng5 = "";
                                    string eveng5 = "";
                                    att = drcount14["att"].ToString();
                                    if (att != "")
                                    {

                                        string[] tmpdate = att.ToString().Split(new char[] { '-' });


                                        mrng5 = tmpdate[0].ToString();
                                        eveng5 = tmpdate[1].ToString();
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = mrng5.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = eveng5;

                                    staffname = drcount14["staff_name"].ToString();
                                    staffcode = drcount14["staff_code"].ToString();
                                    dept_acronym = drcount14["dept_acronym"].ToString();
                                    category = drcount14["category_name"].ToString();
                                    intime = drcount14["time_in"].ToString();
                                    if (intime != "")
                                    {
                                        // string[] split = date1.Split(new Char[] { '/' });
                                        string[] split50 = intime.Split(new char[] { ':' });
                                        horsin = split50[0];
                                        hrint = Convert.ToInt16(horsin);
                                        minin = split50[1];
                                        minin1 = minin.Substring(0, 2);
                                        sesin = minin.Substring(2);
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = hrint.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = minin1;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = sesin;
                                    }

                                    outtime = drcount14["time_out"].ToString();
                                    if (outtime != "")
                                    {
                                        string[] split51 = outtime.Split(new char[] { ':' });
                                        horsto = split51[0];
                                        htoutint = Convert.ToInt16(horsto);
                                        minto = split51[1];
                                        minto1 = minto.Substring(0, 2);
                                        sesto = minto.Substring(2);
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = htoutint.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 6].Text = minto1.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 7].Text = sesto.ToString();
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = staffname;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Tag = staffcode;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = dept_acronym;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = category;
                                    //  fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = outtime;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = hrint.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = minin1;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = sesin;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = htoutint.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 6].Text = minto1.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 7].Text = sesto.ToString();

                                    fpbiomatric.Sheets[0].RowHeader.Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(fpbiomatric.Sheets[0].RowCount - 1);//Added by Manikandan 09/08/2013

                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].CellType = cmbcel1;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].CellType = cmbcel2;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].CellType = cmbcel3;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].CellType = cmbcel1;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].CellType = cmbcel2;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].CellType = cmbcel3;


                                    Double totalRows = 0;
                                    totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                                    if (totalRows >= 10)
                                    {
                                        fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                                        fpbiomatric.Height = 600;
                                        fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                        fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                                    }
                                    else if (totalRows == 0)
                                    {

                                        fpbiomatric.Height = 600;
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                                        fpbiomatric.Height = 100 + (100 * Convert.ToInt32(totalRows));
                                    }


                                    Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);


                                }
                            }

                        }
                    }
                }

                sql1 = " SELECT distinct staffmaster.staff_code , desig_master.desig_acronym, hrdept_master.dept_acronym,CONVERT(VARCHAR(10),staffmaster.join_date,103) as join_date,in_out_time.category_name,staffmaster.staff_name ,desig_name ,hrdept_master.dept_acronym as 'Dept Acronym',hrdept_master.dept_name ,desig_acronym";
                sql1 = sql1 + "  FROM staffmaster,stafftrans,hrdept_master, desig_master,In_Out_Time,staff_attnd ";
                sql1 = sql1 + " where  hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code ";
                sql1 = sql1 + " and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 ";
                sql1 = sql1 + " and  stafftrans.latestrec<>0   and hrdept_master.dept_code=stafftrans.dept_code and desig_master.desig_code=stafftrans.desig_code ";
                sql1 = sql1 + "and staffmaster.settled = 0 And staffmaster.resign = 0 And In_Out_Time.Category_Code = Stafftrans.Category_Code";  //   -- modified By Jeyaprakash on July 27th
                sql1 = sql1 + " And staffmaster.staff_code = stafftrans.staff_code And stafftrans.latestrec = 1 and in_out_time.shift = stafftrans.shift ";//This (in_out_time.shift = stafftrans.shift) condition added by Manikandan 21/08/2013
                sql1 = sql1 + " and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code=desig_master.collegecode ";
                sql1 = sql1 + " and staffmaster.college_code='" + cblcollege.SelectedItem.Value.ToString() + "' and staffmaster.staff_code not in(select distinct roll_no from bio_attendance where " + strdate1 + " )";


                strdept = "";
                if (tbseattype.Text != "---Select---")
                {
                    int itemcount = 0;


                    for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
                    {
                        if (cbldepttype.Items[itemcount].Selected == true)
                        {
                            if (strdept == "")
                                strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                            else
                                strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                        }
                    }


                    if (strdept != "")
                    {
                        strdept = " in(" + strdept + ")";
                        sql1 = sql1 + " and hrdept_master.dept_code " + strdept + "";
                    }
                }
                strcategory = "";
                if (tbblood.Text != "---Select---")
                {
                    int itemcount1 = 0;
                    for (itemcount1 = 0; itemcount1 < cblcategory.Items.Count; itemcount1++)
                    {
                        if (cblcategory.Items[itemcount1].Selected == true)
                        {
                            if (strcategory == "")
                                strcategory = "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                            else
                                strcategory = strcategory + "," + "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                        }
                    }


                    if (strcategory != "")
                    {
                        strcategory = " in (" + strcategory + ")";
                        sql1 = sql1 + "  and stafftrans.category_code" + strcategory + "";
                    }
                }
                if (cbostaffname.SelectedItem.Value.ToString() != "All")
                {
                    sql1 = sql1 + " and staffmaster.staff_name='" + cbostaffname.SelectedItem.Value.ToString() + "'";
                }
                mycon1.Close();
                mycon1.Open();
                SqlCommand cmd56 = new SqlCommand(sql1, mycon1);
                SqlDataReader dr30;
                dr30 = cmd56.ExecuteReader();
                fpbiomatric.Visible = true;
                btnprintmaster.Visible = true;
                while (dr30.Read())
                {
                    if (dr30.HasRows == true)
                    {

                        sql1 = "";
                        // Str = "";
                        string staffcode1;
                        string category8 = "";
                        string timein8 = "";

                        staffcode1 = dr30["staff_code"].ToString();
                        category8 = dr30["category_name"].ToString();
                        int countcolumn;
                        countcolumn = fpbiomatric.Sheets[0].ColumnCount;

                        for (int colcount = 4; colcount <= countcolumn - 1; colcount = colcount + 8)
                        {

                            fpbiomatric.ActiveSheetView.Cells[0, colcount].CellType = ddlleavetype;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 1].CellType = ddlleavetype;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 2].CellType = objintcell;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 3].CellType = objintcell1;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 4].CellType = objintcellsesfrm;

                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 5].CellType = objintcell3;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 6].CellType = objintcell2;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 7].CellType = objintcellsesto;

                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 8);
                            fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";


                            //////////////////////////////////////////////////////////////////////////
                            //Start=======================Hided by Manikandan 08/08/2013=============================
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 4].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 5].Border.BorderColor = Color.White;



                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;

                            //=============================End===========================================================
                            //////////////////////////////////////////////////////////////////////
                            string[] cbstrhrsin;
                            cbstrhrsin = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                            string[] cbstrminin;
                            cbstrminin = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };
                            string[] cbstrses;
                            cbstrses = new string[] { "AM", "PM" };
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel1 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrhrsin);
                            fpbiomatric.Sheets[0].Columns[colcount + 2].CellType = cmbcel1;
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel2 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrminin);
                            fpbiomatric.Sheets[0].Columns[colcount + 3].CellType = cmbcel2;
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel3 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrses);
                            fpbiomatric.Sheets[0].Columns[colcount + 4].CellType = cmbcel3;
                            fpbiomatric.Sheets[0].Columns[colcount + 5].CellType = cmbcel1;
                            fpbiomatric.Sheets[0].Columns[colcount + 6].CellType = cmbcel2;
                            fpbiomatric.Sheets[0].Columns[colcount + 7].CellType = cmbcel3;

                            fpbiomatric.ActiveSheetView.Columns[colcount].HorizontalAlign = HorizontalAlign.Center;

                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 4, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 7, 50);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 5, 40);

                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].Font.Name = "Book Antiqua";


                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "UnReg";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].Columns[colcount].CellType = ddlcell;
                            fpbiomatric.Sheets[0].Columns[colcount + 1].CellType = ddlcell;

                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount, 1, 2);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].Text = "Mor";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].Text = "Eve";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].HorizontalAlign = HorizontalAlign.Center;

                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "In";

                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;

                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount + 2, 1, 3);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].Text = "Hrs";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].Text = "Minu";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].Text = "Ses";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].HorizontalAlign = HorizontalAlign.Center;

                            //fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                            fpbiomatric.Sheets[0].Columns[colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Text = "Out";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;

                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount + 5, 1, 3);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].Text = "Hrs";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].Text = "Min";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].Text = "Ses";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].HorizontalAlign = HorizontalAlign.Center;


                            string datetagvalue;
                            datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                            strdate = "  bio_attendance.access_date='" + datetagvalue + "'";
                            sql1 = " SELECT distinct staffmaster.staff_code , desig_master.desig_acronym, hrdept_master.dept_acronym,CONVERT(VARCHAR(10),staffmaster.join_date,103) as join_date,in_out_time.category_name,staffmaster.staff_name ,desig_name ,hrdept_master.dept_acronym as 'Dept Acronym',hrdept_master.dept_name ,desig_acronym";
                            sql1 = sql1 + "  FROM staffmaster,stafftrans,hrdept_master, desig_master,In_Out_Time,staff_attnd ";
                            sql1 = sql1 + " where  hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code ";
                            sql1 = sql1 + " and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 ";
                            sql1 = sql1 + " and  stafftrans.latestrec<>0   and hrdept_master.dept_code=stafftrans.dept_code and desig_master.desig_code=stafftrans.desig_code ";
                            sql1 = sql1 + "and staffmaster.settled = 0 And staffmaster.resign = 0  And In_Out_Time.Category_Code = Stafftrans.Category_Code ";
                            sql1 = sql1 + " And staffmaster.staff_code = stafftrans.staff_code And stafftrans.latestrec = 1 and in_out_time.shift = stafftrans.shift  and staffmaster.staff_code='" + staffcode1 + "'";//This (in_out_time.shift = stafftrans.shift) condition added in this Query by Manikandan 21/08/2013
                            sql1 = sql1 + " and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code=desig_master.collegecode ";
                            sql1 = sql1 + " and staffmaster.college_code='" + cblcollege.SelectedItem.Value.ToString() + "' and staffmaster.staff_code not in(select distinct roll_no from bio_attendance where " + strdate + " )";
                            // sql = "SELECT distinct staffmaster.staff_code ,desig_master.desig_acronym, hrdept_master.dept_acronym,CONVERT(VARCHAR(10),staffmaster.join_date,103) as join_date,in_out_time.category_name,staffmaster.staff_name ,desig_name ,hrdept_master.dept_acronym as 'Dept Acronym',hrdept_master.dept_name ,desig_acronym,access_date as 'Entry Date',right(CONVERT(nvarchar(100),time_in ,100),6) as time_in ,right(CONVERT(nvarchar(100),time_out ,100),6) as time_out,convert(char(8),(cast(time_out as datetime) - cast(time_in as datetime)),108) as TotalHours,att  FROM staffmaster,stafftrans,hrdept_master, desig_master,bio_attendance,In_Out_Time,staff_attnd where  (staffmaster.Fingerprint1 Is Not Null or staffmaster.Fingerprint1 Is Not Null) and hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and   staffmaster.staff_code=bio_attendance.roll_no and is_staff=1 and hrdept_master.dept_code=stafftrans.dept_code and desig_master.desig_code=stafftrans.desig_code and staffmaster.settled = 0 And staffmaster.resign = 0 and staffmaster.staff_code='" + staffcode1 + "' And In_Out_Time.Category_Code = Stafftrans.Category_Code And staffmaster.staff_code = stafftrans.staff_code And stafftrans.latestrec = 1 and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code=desig_master.collegecode and att<>'' and staffmaster.college_code=" + Session["collegecode"] + "  " + strdate + " ";
                            con.Close();
                            con.Open();
                            SqlCommand cmd7 = new SqlCommand(sql1, con);
                            SqlDataReader drcount14;
                            fpbiomatric.Width = 750;

                            drcount14 = cmd7.ExecuteReader();
                            getrow = rowstr;

                            //  Session["getrow"] = getrow;
                            while (drcount14.Read())
                            {
                                if (drcount14.HasRows == true)
                                {
                                    btnsave.Visible = true;//Added by Manikandan 08/08/2013
                                    btnsave.Enabled = true;
                                    if (tempstaffcode == "")
                                    {

                                        fpbiomatric.Sheets[0].RowCount += 1;
                                        tempstaffcode = drcount14["staff_code"].ToString();
                                    }

                                    else if ((tempstaffcode != "") && (tempstaffcode != drcount14["staff_code"].ToString()))
                                    {
                                        fpbiomatric.Sheets[0].RowCount += 1;
                                        tempstaffcode = drcount14["staff_code"].ToString();
                                    }
                                    rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                                    string staffcode = "";
                                    string staffname = "";
                                    string dept_acronym = "";
                                    string category;
                                    string intime = "";
                                    string outtime = "";


                                    staffname = drcount14["staff_name"].ToString();
                                    staffcode = drcount14["staff_code"].ToString();
                                    dept_acronym = drcount14["dept_acronym"].ToString();
                                    category = drcount14["category_name"].ToString();

                                    // intime = drcount14["time_in"].ToString();
                                    //outtime = drcount14["time_out"].ToString();
                                    fpbiomatric.Sheets[0].RowHeader.Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(fpbiomatric.Sheets[0].RowCount - 1);//Added by Manikandan 09/08/2013
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = staffname;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Tag = staffcode;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = dept_acronym;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = category;
                                    // fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = outtime;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = intime;
                                    string dateval3 = "";
                                    int da4 = 0;

                                    int d58 = 0;
                                    int mon59 = 0;
                                    string[] spl58 = datetagvalue.Split('/');
                                    string daa58 = spl58[1];
                                    d58 = Convert.ToInt16(daa58);
                                    //da4 = d58 + 3 ;
                                    string mon58 = spl58[0];
                                    mon59 = Convert.ToInt16(mon58);
                                    string motyear5 = mon59 + "/" + spl58[2];
                                    sql3 = "select [" + d58 + "],staff_code,mon_year from staff_attnd where mon_year='" + motyear5 + "' and staff_code='" + staffcode + "'";
                                    SqlCommand cmd100 = new SqlCommand(sql3, myatt);
                                    myatt.Close();
                                    myatt.Open();
                                    SqlDataReader dr100 = cmd100.ExecuteReader();
                                    while (dr100.Read())
                                    {

                                        if (dr30.HasRows == true)
                                        {
                                            string attenda1 = "";
                                            string mor30 = "";
                                            string eve30 = "";
                                            attenda1 = dr100[0].ToString();
                                            if (attenda1 != "")
                                            {
                                                string[] spatt = attenda1.Split('-');
                                                mor30 = spatt[0];
                                                eve30 = spatt[1];
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = mor30.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = eve30.ToString();
                                            }
                                            else
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = mor30.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = eve30.ToString();
                                            }

                                        }
                                    }
                                }
                            }
                        }

                        Double totalRows = 0;
                        totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                        if (totalRows >= 10)
                        {
                            fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                            fpbiomatric.Height = 600;
                            fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                            fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                        }
                        else if (totalRows == 0)
                        {

                            fpbiomatric.Height = 600;
                        }
                        else
                        {
                            fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                            fpbiomatric.Height = 100 + (125 * Convert.ToInt32(totalRows));
                        }
                        Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);

                        //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);//Hided by Manikandan 09/08/2013
                    }
                }
                if (dr30.HasRows == false)
                {
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    btnsave.Visible = false;//Added by Manikandan 08/08/2013
                    btnsave.Enabled = false;
                    lblnorec.Visible = true;
                }

                //if (fpbiomatric.Sheets[0].RowCount == 1)
                //{
                //    Buttontotal.Visible = true;
                //    DropDownListpage.Visible = true;
                //    TextBoxpage.Visible = true;
                //    lblrecord.Visible = true;
                //    lblrecord.Visible = true;
                //    lblpage.Visible = true;
                //    fpbiomatric.Visible = false;
                //}

            }
            else if (rdounreg.Checked == true)
            {
                strdate1 = "  bio_attendance.access_date between '" + datefrom + "' and '" + dateto + "'";


                if (days >= 0)
                {
                    string[] differdays = new string[days];


                    lbldate.Visible = false;


                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 8;

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Text = Txtentryfrom.Text.ToString();

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].HorizontalAlign = HorizontalAlign.Center;

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].HorizontalAlign = HorizontalAlign.Center;
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Tag = datefrom.ToString();
                    // fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].Text = "Time In";
                    //  fpbiomatric.Sheets[0].ColumnHeader.Cells[2, fpbiomatric.Sheets[0].ColumnCount - 2].Text = "Hours";
                    fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 2, 33);
                    fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].ColumnHeader.Cells[7, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Time Out";
                    fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount 1, 33);

                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next Date
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 8;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Tag = split12[0].ToString() + "/" + split12[1].ToString() + "/" + split12[2].ToString(); ;
                        // fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].Text = "Time Out";
                        //  fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                        // fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 33);
                        // fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Time In";
                        // fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 33);
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
                fpbiomatric.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(darkstyle);
                fpbiomatric.Sheets[0].AllowTableCorner = true;
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";


                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 7, 1);

                //Start============Hided by Manikandan 08/08/2013====================
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, fpbiomatric.Sheets[0].ColumnCount - 1, 6, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 9, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 8, 1);
                //===========================End=====================================
                fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 3, 1);
                // fpbiomatric.Sheets[0].ColumnHeader.Rows[6].BackColor = Color.FromArgb(214, 235, 255);
                //fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);//Hided by Manikandan 08/08/2013
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColor = Color.Black;
                fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 3, 1);

                //Start================Hided by Manikandan 08/08/2013======================
                //string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + cblcollege.SelectedItem.Value.ToString() + "'";
                //con.Close();
                //con.Open();
                //SqlCommand comm = new SqlCommand(str, con);
                //SqlDataReader drr = comm.ExecuteReader();
                //drr.Read();
                //string coll_name = Convert.ToString(drr["collname"]);
                //string coll_address1 = Convert.ToString(drr["address1"]);
                //string coll_address2 = Convert.ToString(drr["address2"]);
                //string coll_address3 = Convert.ToString(drr["address3"]);
                //string pin_code = Convert.ToString(drr["pincode"]);

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = coll_name;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 1].Text = coll_address1;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, 1].Text = coll_address2;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, 1].Text = coll_address3 + "-" + pin_code;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Daily Attendance Report";
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                ////fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                ////fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                ////fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);



                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].ForeColor = Color.FromArgb(64, 64, 255);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;

                //// fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 6, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 1);

                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, 3);
                ////  fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(5, 4, 1, 5);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(4, 4, 1, 5);

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Left;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;


                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].HorizontalAlign = HorizontalAlign.Right;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].Font.Size = FontUnit.Medium;

                //MyImg mi = new MyImg();
                //mi.ImageUrl = "../images/10BIT001.jpeg";
                //mi.ImageUrl = "Handler/Handler2.ashx?";
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 1].CellType = mi;
                //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 50);


                //fpbiomatric.Sheets[0].SetColumnWidth(0, 50);

                //fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[2].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[2].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[3].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[3].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[4].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[4].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[5].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[5].Font.Size = FontUnit.Medium;

                //==============================End=================================================

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);

                //======Hided by Manikandan 08/08/2013========
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(5, 4, 1, fpbiomatric.Sheets[0].ColumnCount - 5);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Text = "Date-From" + date1 + "To:" + date2 + "";
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Left;
                //============End=============

                string categry4 = "";
                for (int g = 0; g < cblcategory.Items.Count; g++)
                {
                    if (cblcategory.Items[g].Selected == true)
                    {
                        categry4 = categry4 + cblcategory.Items[g].Text + ",";
                    }
                }

                if (categry4 != "")
                {
                    categry4 = categry4.Substring(0, categry4.Length - 1);
                }
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 5].Text = categry4.ToString();
                //=============hided by manikandan===============
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].Text = "Category:" + categry4.ToString();
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].HorizontalAlign = HorizontalAlign.Right;
                //====================End========================
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].ColumnHeader.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].ColumnHeader.Rows[2].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                ////////////////////////////////////////////////////////////

                sql1 = " SELECT distinct staffmaster.staff_code , desig_master.desig_acronym, hrdept_master.dept_acronym,CONVERT(VARCHAR(10),staffmaster.join_date,103) as join_date,in_out_time.category_name,staffmaster.staff_name ,desig_name ,hrdept_master.dept_acronym as 'Dept Acronym',hrdept_master.dept_name ,desig_acronym";
                sql1 = sql1 + "  FROM staffmaster,stafftrans,hrdept_master, desig_master,In_Out_Time,staff_attnd ";
                sql1 = sql1 + " where  hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code ";
                sql1 = sql1 + " and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 ";
                sql1 = sql1 + " and  stafftrans.latestrec<>0   and hrdept_master.dept_code=stafftrans.dept_code and desig_master.desig_code=stafftrans.desig_code ";
                sql1 = sql1 + "and staffmaster.settled = 0 And staffmaster.resign = 0  And In_Out_Time.Category_Code = Stafftrans.Category_Code ";
                sql1 = sql1 + " And staffmaster.staff_code = stafftrans.staff_code And stafftrans.latestrec = 1 and in_out_time.shift = stafftrans.shift ";//This (in_out_time.shift = stafftrans.shift) condition Added in this query by Manikandan 21/08/2013
                sql1 = sql1 + " and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code=desig_master.collegecode ";
                sql1 = sql1 + " and staffmaster.college_code='" + cblcollege.SelectedItem.Value.ToString() + "' and staffmaster.staff_code not in(select distinct roll_no from bio_attendance where " + strdate1 + " )";


                strdept = "";
                if (tbseattype.Text != "---Select---")
                {
                    int itemcount = 0;


                    for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
                    {
                        if (cbldepttype.Items[itemcount].Selected == true)
                        {
                            if (strdept == "")
                                strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                            else
                                strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                        }
                    }


                    if (strdept != "")
                    {
                        strdept = " in(" + strdept + ")";
                        sql1 = sql1 + " and hrdept_master.dept_code " + strdept + "";
                    }
                }
                strcategory = "";
                if (tbblood.Text != "---Select---")
                {


                    int itemcount1 = 0;

                    for (itemcount1 = 0; itemcount1 < cblcategory.Items.Count; itemcount1++)
                    {
                        if (cblcategory.Items[itemcount1].Selected == true)
                        {
                            if (strcategory == "")
                                strcategory = "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                            else
                                strcategory = strcategory + "," + "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                        }
                    }


                    if (strcategory != "")
                    {
                        strcategory = " in (" + strcategory + ")";
                        sql1 = sql1 + "  and stafftrans.category_code" + strcategory + "";
                    }
                }
                if (cbostaffname.SelectedItem.Value.ToString() != "All")
                {
                    sql1 = sql1 + " and staffmaster.staff_name='" + cbostaffname.SelectedItem.Value.ToString() + "'";
                }
                mycon1.Close();
                mycon1.Open();
                SqlCommand cmd56 = new SqlCommand(sql1, mycon1);
                SqlDataReader dr30;
                dr30 = cmd56.ExecuteReader();
                fpbiomatric.Visible = true;
                btnprintmaster.Visible = true;
                while (dr30.Read())
                {
                    if (dr30.HasRows == true)
                    {

                        sql1 = "";
                        // Str = "";
                        string staffcode1;
                        string category8 = "";
                        string timein8 = "";

                        staffcode1 = dr30["staff_code"].ToString();
                        category8 = dr30["category_name"].ToString();
                        int countcolumn;
                        countcolumn = fpbiomatric.Sheets[0].ColumnCount;

                        for (int colcount = 4; colcount <= countcolumn - 1; colcount = colcount + 8)
                        {
                            ////////////////////////
                            fpbiomatric.ActiveSheetView.Cells[0, colcount].CellType = ddlleavetype;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 1].CellType = ddlleavetype;

                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 2].CellType = objintcell;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 3].CellType = objintcell1;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 4].CellType = objintcellsesfrm;

                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 5].CellType = objintcell3;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 6].CellType = objintcell2;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 7].CellType = objintcellsesto;

                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 8);
                            fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";


                            //////////////////////////////////////////////////////////////////////////
                            //Start====================Hided by Manikandan 08/08/2013==============================
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 4].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 5].Border.BorderColor = Color.White;



                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;

                            //===============================End=========================================================

                            //////////////////////////////////////////////////////////////////////
                            string[] cbstrhrsin;
                            cbstrhrsin = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                            string[] cbstrminin;
                            cbstrminin = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };
                            string[] cbstrses;
                            cbstrses = new string[] { "AM", "PM" };
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel1 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrhrsin);
                            fpbiomatric.Sheets[0].Columns[colcount + 2].CellType = cmbcel1;
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel2 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrminin);
                            fpbiomatric.Sheets[0].Columns[colcount + 3].CellType = cmbcel2;
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel3 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrses);
                            fpbiomatric.Sheets[0].Columns[colcount + 4].CellType = cmbcel3;
                            fpbiomatric.Sheets[0].Columns[colcount + 5].CellType = cmbcel1;
                            fpbiomatric.Sheets[0].Columns[colcount + 6].CellType = cmbcel2;
                            fpbiomatric.Sheets[0].Columns[colcount + 7].CellType = cmbcel3;

                            fpbiomatric.ActiveSheetView.Columns[colcount].HorizontalAlign = HorizontalAlign.Center;

                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 4, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 7, 50);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 5, 40);

                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].Font.Name = "Book Antiqua";


                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "UnReg";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].Columns[colcount].CellType = ddlcell;
                            fpbiomatric.Sheets[0].Columns[colcount + 1].CellType = ddlcell;

                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount, 1, 2);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].Text = "Mor";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].Text = "Eve";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].HorizontalAlign = HorizontalAlign.Center;

                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "In";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount + 2, 1, 3);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].Text = "Hrs";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].Text = "Minu";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].Text = "Ses";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].HorizontalAlign = HorizontalAlign.Center;

                            //fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                            fpbiomatric.Sheets[0].Columns[colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Text = "Out";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount + 5, 1, 3);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].Text = "Hrs";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].Text = "Min";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].Text = "Ses";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].HorizontalAlign = HorizontalAlign.Center;


                            string datetagvalue;
                            datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                            strdate = "  bio_attendance.access_date='" + datetagvalue + "'";

                            sql1 = " SELECT distinct staffmaster.staff_code , desig_master.desig_acronym, hrdept_master.dept_acronym,CONVERT(VARCHAR(10),staffmaster.join_date,103) as join_date,in_out_time.category_name,staffmaster.staff_name ,desig_name ,hrdept_master.dept_acronym as 'Dept Acronym',hrdept_master.dept_name ,desig_acronym";
                            sql1 = sql1 + "  FROM staffmaster,stafftrans,hrdept_master, desig_master,In_Out_Time,staff_attnd ";
                            sql1 = sql1 + " where  hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code ";
                            sql1 = sql1 + " and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 ";
                            sql1 = sql1 + " and  stafftrans.latestrec<>0   and hrdept_master.dept_code=stafftrans.dept_code and desig_master.desig_code=stafftrans.desig_code ";
                            sql1 = sql1 + "and staffmaster.settled = 0 And staffmaster.resign = 0  And In_Out_Time.Category_Code = Stafftrans.Category_Code ";
                            sql1 = sql1 + " And staffmaster.staff_code = stafftrans.staff_code And stafftrans.latestrec = 1 and in_out_time.shift = stafftrans.shift  and staffmaster.staff_code='" + staffcode1 + "'";//this (in_out_time.shift = stafftrans.shift) condition added in this Query by Manikandan 21/08/2013
                            sql1 = sql1 + " and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code=desig_master.collegecode ";
                            sql1 = sql1 + " and staffmaster.college_code='" + cblcollege.SelectedItem.Value.ToString() + "' and staffmaster.staff_code not in(select distinct roll_no from bio_attendance where " + strdate + " )";
                            // sql = "SELECT distinct staffmaster.staff_code ,desig_master.desig_acronym, hrdept_master.dept_acronym,CONVERT(VARCHAR(10),staffmaster.join_date,103) as join_date,in_out_time.category_name,staffmaster.staff_name ,desig_name ,hrdept_master.dept_acronym as 'Dept Acronym',hrdept_master.dept_name ,desig_acronym,access_date as 'Entry Date',right(CONVERT(nvarchar(100),time_in ,100),6) as time_in ,right(CONVERT(nvarchar(100),time_out ,100),6) as time_out,convert(char(8),(cast(time_out as datetime) - cast(time_in as datetime)),108) as TotalHours,att  FROM staffmaster,stafftrans,hrdept_master, desig_master,bio_attendance,In_Out_Time,staff_attnd where  (staffmaster.Fingerprint1 Is Not Null or staffmaster.Fingerprint1 Is Not Null) and hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and   staffmaster.staff_code=bio_attendance.roll_no and is_staff=1 and hrdept_master.dept_code=stafftrans.dept_code and desig_master.desig_code=stafftrans.desig_code and staffmaster.settled = 0 And staffmaster.resign = 0 and staffmaster.staff_code='" + staffcode1 + "' And In_Out_Time.Category_Code = Stafftrans.Category_Code And staffmaster.staff_code = stafftrans.staff_code And stafftrans.latestrec = 1 and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code=desig_master.collegecode and att<>'' and staffmaster.college_code=" + Session["collegecode"] + "  " + strdate + " ";
                            con.Close();
                            con.Open();
                            SqlCommand cmd7 = new SqlCommand(sql1, con);


                            SqlDataReader drcount14;
                            fpbiomatric.Width = 750;

                            drcount14 = cmd7.ExecuteReader();
                            getrow = rowstr;

                            //  Session["getrow"] = getrow;
                            while (drcount14.Read())
                            {
                                if (drcount14.HasRows == true)
                                {
                                    btnsave.Visible = true;//Added by Manikandan 08/08/2013
                                    btnsave.Enabled = true;
                                    if (tempstaffcode == "")
                                    {

                                        fpbiomatric.Sheets[0].RowCount += 1;
                                        tempstaffcode = drcount14["staff_code"].ToString();
                                    }

                                    else if ((tempstaffcode != "") && (tempstaffcode != drcount14["staff_code"].ToString()))
                                    {
                                        fpbiomatric.Sheets[0].RowCount += 1;
                                        tempstaffcode = drcount14["staff_code"].ToString();
                                    }


                                    rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                                    string staffcode = "";
                                    string staffname = "";
                                    string dept_acronym = "";
                                    string category;
                                    string intime = "";
                                    string outtime = "";

                                    staffname = drcount14["staff_name"].ToString();
                                    staffcode = drcount14["staff_code"].ToString();
                                    dept_acronym = drcount14["dept_acronym"].ToString();
                                    category = drcount14["category_name"].ToString();

                                    fpbiomatric.Sheets[0].RowHeader.Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(fpbiomatric.Sheets[0].RowCount - 1);//Added by Manikandan 09/08/2013

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = staffname;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Tag = staffcode;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = dept_acronym;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = category;


                                    string dateval3 = "";
                                    int da4 = 0;

                                    int d58 = 0;
                                    int mon59 = 0;
                                    string[] spl58 = datetagvalue.Split('/');
                                    string daa58 = spl58[1];
                                    d58 = Convert.ToInt16(daa58);
                                    //da4 = d58 + 3 ;
                                    string mon58 = spl58[0];
                                    mon59 = Convert.ToInt16(mon58);
                                    string motyear5 = mon59 + "/" + spl58[2];
                                    sql3 = "select [" + d58 + "],staff_code,mon_year from staff_attnd where mon_year='" + motyear5 + "' and staff_code='" + staffcode + "'";
                                    SqlCommand cmd100 = new SqlCommand(sql3, myatt);
                                    myatt.Close();
                                    myatt.Open();
                                    SqlDataReader dr100 = cmd100.ExecuteReader();
                                    while (dr100.Read())
                                    {
                                        if (dr30.HasRows == true)
                                        {
                                            string attenda1 = "";
                                            string mor30 = "";
                                            string eve30 = "";
                                            attenda1 = dr100[0].ToString();

                                            if (attenda1 != "")
                                            {
                                                string[] spatt = attenda1.Split('-');
                                                mor30 = spatt[0];
                                                eve30 = spatt[1];
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = mor30.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = eve30.ToString();
                                            }
                                            else
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = mor30.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = eve30.ToString();
                                            }
                                        }

                                    }

                                }
                            }
                        }

                        Double totalRows = 0;
                        totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                        if (totalRows >= 10)
                        {
                            fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                            fpbiomatric.Height = 600;
                            fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                            fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                        }
                        else if (totalRows == 0)
                        {
                            fpbiomatric.Height = 600;
                        }
                        else
                        {
                            fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                            fpbiomatric.Height = 125 + (125 * Convert.ToInt32(totalRows));
                        }


                        Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);

                    }
                }
                if (dr30.HasRows == false)
                {
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    btnsave.Visible = false;//Added by Manikandan 08/08/2013
                    btnsave.Enabled = false;
                    lblnorec.Visible = true;
                }

            }
            else if (rdoreg.Checked == true)
            {
                strdate1 = "  bio_attendance.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];


                    lbldate.Visible = false;


                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 8;

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Text = Txtentryfrom.Text.ToString();

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].HorizontalAlign = HorizontalAlign.Center;
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Tag = datefrom.ToString();
                    // fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].Text = "Time In";
                    //  fpbiomatric.Sheets[0].ColumnHeader.Cells[2, fpbiomatric.Sheets[0].ColumnCount - 2].Text = "Hours";
                    fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 2, 33);
                    fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].ColumnHeader.Cells[7, fpbiomatric.Sheets[0].ColumnCount - 8].Text = "Time Out";
                    fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount 1, 33);

                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next Date
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 8;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 8].Tag = split12[0].ToString() + "/" + split12[1].ToString() + "/" + split12[2].ToString(); ;
                        // fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].Text = "Time Out";
                        //  fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;


                        // fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 33);
                        // fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Time In";
                        // fpbiomatric.Sheets[0].Columns[fpbiomatric.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 33);
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
                fpbiomatric.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(darkstyle);
                fpbiomatric.Sheets[0].AllowTableCorner = true;
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";


                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 7, 1);
                //Start============Hided by Manikandan 08/08/2013===================
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, fpbiomatric.Sheets[0].ColumnCount - 1, 6, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 9, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 8, 1);
                //======================End=========================================
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);Hided by Manikandan 08/08/2013
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].SheetCornerSpanModel.Add(0, 0, 3, 1);
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColor = Color.Black;

                //Start================Hided by Manikandan 08/08/2013========================
                //string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + cblcollege.SelectedItem.Value.ToString() + "'";
                //con.Close();
                //con.Open();
                //SqlCommand comm = new SqlCommand(str, con);
                //SqlDataReader drr = comm.ExecuteReader();
                //drr.Read();
                //string coll_name = Convert.ToString(drr["collname"]);
                //string coll_address1 = Convert.ToString(drr["address1"]);
                //string coll_address2 = Convert.ToString(drr["address2"]);
                //string coll_address3 = Convert.ToString(drr["address3"]);
                //string pin_code = Convert.ToString(drr["pincode"]);

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = coll_name;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 1].Text = coll_address1;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, 1].Text = coll_address2;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, 1].Text = coll_address3 + "-" + pin_code;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Daily Attendance Report";
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                ////fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                ////fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                ////fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 2);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].ForeColor = Color.FromArgb(64, 64, 255);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;

                //// fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 6, 1);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 1);

                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, 3);
                ////  fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(5, 4, 1, 5);
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(4, 4, 1, 5);

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Left;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;


                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].HorizontalAlign = HorizontalAlign.Right;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].Font.Size = FontUnit.Medium;

                //MyImg mi = new MyImg();
                //mi.ImageUrl = "../images/10BIT001.jpeg";
                //mi.ImageUrl = "Handler/Handler2.ashx?";
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 1].CellType = mi;
                //fpbiomatric.Sheets[0].SetColumnWidth(fpbiomatric.Sheets[0].ColumnCount - 1, 50);


                //fpbiomatric.Sheets[0].SetColumnWidth(0, 50);

                //fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[2].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[2].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[3].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[3].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[4].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[4].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[5].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeader.Rows[5].Font.Size = FontUnit.Medium;

                //============================End==========================================

                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);

                //Start==========Hided by Manikandan 08/08/2013=====================
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(5, 4, 1, fpbiomatric.Sheets[0].ColumnCount - 5);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].Text = "Date-From" + date1 + "To:" + date2 + "";
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Left;
                //====================End===========================================
                string categry4 = "";
                for (int g = 0; g < cblcategory.Items.Count; g++)
                {
                    if (cblcategory.Items[g].Selected == true)
                    {
                        categry4 = categry4 + cblcategory.Items[g].Text + ",";
                    }
                }
                if (categry4 != "")
                {
                    categry4 = categry4.Substring(0, categry4.Length - 1);
                }

                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 5].Text = categry4.ToString();
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].Text = "Category:" + categry4.ToString();Hided by Manikandan 08/08/2013
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[5, 4].HorizontalAlign = HorizontalAlign.Right;Hided by Manikandan 08/08/2013


                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].ColumnHeader.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpbiomatric.Sheets[0].ColumnHeader.Rows[2].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                ////////////////////////////////////////////////
                sql = "SELECT distinct staffmaster.staff_code , desig_master.desig_acronym, hrdept_master.dept_acronym,CONVERT(VARCHAR(10),staffmaster.join_date,103) as join_date,in_out_time.category_name,staffmaster.staff_name ,desig_name ,hrdept_master.dept_acronym as 'Dept Acronym',hrdept_master.dept_name ,desig_acronym,access_date as 'Entry Date',right(CONVERT(nvarchar(100),time_in ,100),7) as time_in ,right(CONVERT(nvarchar(100),time_out ,100),7) as time_out,convert(char(8),(cast(time_out as datetime) - cast(time_in as datetime)),108) as TotalHours, att  FROM staffmaster,stafftrans,hrdept_master, desig_master,bio_attendance,In_Out_Time,staff_attnd where  (staffmaster.Fingerprint1 Is Not Null or staffmaster.Fingerprint1 Is Not Null) and hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and   staffmaster.staff_code=bio_attendance.roll_no and is_staff=1 and hrdept_master.dept_code=stafftrans.dept_code and desig_master.desig_code=stafftrans.desig_code and staffmaster.settled = 0 And staffmaster.resign = 0  And In_Out_Time.Category_Code = Stafftrans.Category_Code And staffmaster.staff_code = stafftrans.staff_code And stafftrans.latestrec = 1 and in_out_time.shift = stafftrans.shift and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code=desig_master.collegecode  and staffmaster.college_code='" + cblcollege.SelectedItem.Value.ToString() + "' and  " + strdate1 + " ";//this (in_out_time.shift = stafftrans.shift) condition added in this Query by Manikandan 21/08/2013

                if (tbseattype.Text != "---Select---")
                {
                    int itemcount = 0;


                    for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
                    {
                        if (cbldepttype.Items[itemcount].Selected == true)
                        {
                            if (strdept == "")
                                strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                            else
                                strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                        }
                    }


                    if (strdept != "")
                    {
                        strdept = " in(" + strdept + ")";
                        sql = sql + " and hrdept_master.dept_code " + strdept + "";
                    }
                }
                if (tbblood.Text != "---Select---")
                {

                    strcategory = "";
                    int itemcount1 = 0;

                    for (itemcount1 = 0; itemcount1 < cblcategory.Items.Count; itemcount1++)
                    {
                        if (cblcategory.Items[itemcount1].Selected == true)
                        {
                            if (strcategory == "")
                                strcategory = "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                            else
                                strcategory = strcategory + "," + "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                        }
                    }


                    if (strcategory != "")
                    {
                        strcategory = " in (" + strcategory + ")";
                        sql = sql + "  and stafftrans.category_code" + strcategory + "";
                    }
                }
                if (cbostaffname.SelectedItem.Value.ToString() != "All")
                {
                    sql = sql + " and staffmaster.staff_name='" + cbostaffname.SelectedItem.Value.ToString() + "'";
                }
                sql = sql + " order by staffmaster.staff_code";
                con1.Close();
                con1.Open();
                SqlDataReader drname;
                SqlCommand cmd2 = new SqlCommand(sql, con1);
                drname = cmd2.ExecuteReader();

                if (drname.HasRows == true)
                {
                    fpbiomatric.Visible = true;
                    btnprintmaster.Visible = true;
                    while (drname.Read())
                    {
                        btnsave.Visible = true;//Added by Manikandan 08/08/2013
                        btnsave.Enabled = true;

                        sql = "";
                        // Str = "";
                        string staffcode1;
                        string category8 = "";
                        string timein8 = "";

                        staffcode1 = drname["staff_code"].ToString();
                        category8 = drname["category_name"].ToString();
                        timein8 = drname["time_in"].ToString();


                        int countcolumn;
                        countcolumn = fpbiomatric.Sheets[0].ColumnCount;

                        for (int colcount = 4; colcount <= countcolumn - 1; colcount = colcount + 8)
                        {
                            //////////////////////////////////////


                            fpbiomatric.ActiveSheetView.Cells[0, colcount].CellType = ddlleavetype;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 1].CellType = ddlleavetype;



                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 2].CellType = objintcell;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 3].CellType = objintcell1;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 4].CellType = objintcellsesfrm;

                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 5].CellType = objintcell3;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 6].CellType = objintcell2;
                            fpbiomatric.ActiveSheetView.Cells[0, colcount + 7].CellType = objintcellsesto;

                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 8);
                            fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";


                            //////////////////////////////////////////////////////////////////////////

                            //================Hided by Manikandan 08/08/2013=======================
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 4].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount + 5].Border.BorderColor = Color.White;



                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 1].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 2].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 3].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 4].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 5].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;

                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 6].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 6].Border.BorderColor = Color.White;


                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[3, colcount + 7].Border.BorderColor = Color.White;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[4, colcount + 7].Border.BorderColor = Color.White;

                            //==============================End===========================================

                            //////////////////////////////////////////////////////////////////////
                            string[] cbstrhrsin;
                            cbstrhrsin = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                            string[] cbstrminin;
                            cbstrminin = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };
                            string[] cbstrses;
                            cbstrses = new string[] { "AM", "PM" };
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel1 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrhrsin);
                            fpbiomatric.Sheets[0].Columns[colcount + 2].CellType = cmbcel1;
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel2 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrminin);
                            fpbiomatric.Sheets[0].Columns[colcount + 3].CellType = cmbcel2;
                            FarPoint.Web.Spread.ComboBoxCellType cmbcel3 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrses);
                            fpbiomatric.Sheets[0].Columns[colcount + 4].CellType = cmbcel3;
                            fpbiomatric.Sheets[0].Columns[colcount + 5].CellType = cmbcel1;
                            fpbiomatric.Sheets[0].Columns[colcount + 6].CellType = cmbcel2;
                            fpbiomatric.Sheets[0].Columns[colcount + 7].CellType = cmbcel3;

                            fpbiomatric.ActiveSheetView.Columns[colcount].HorizontalAlign = HorizontalAlign.Center;

                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 4, 40);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 7, 50);
                            fpbiomatric.Sheets[0].SetColumnWidth(colcount + 5, 40);

                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 3].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 4].Font.Name = "Book Antiqua";
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].Font.Size = FontUnit.Medium;
                            fpbiomatric.ActiveSheetView.Columns[colcount + 5].Font.Name = "Book Antiqua";


                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "UnReg";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].Columns[colcount].CellType = ddlcell;
                            fpbiomatric.Sheets[0].Columns[colcount + 1].CellType = ddlcell;

                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount, 1, 2);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].Text = "Mor";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].Text = "Eve";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 1].HorizontalAlign = HorizontalAlign.Center;

                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "In";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount + 2, 1, 3);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].Text = "Hrs";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].Text = "Minu";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].Text = "Ses";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 4].HorizontalAlign = HorizontalAlign.Center;

                            //fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                            fpbiomatric.Sheets[0].Columns[colcount].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Text = "Out";

                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount + 5, 1, 3);
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].Text = "Hrs";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].Text = "Min";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 6].HorizontalAlign = HorizontalAlign.Center;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].Text = "Ses";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[2, colcount + 7].HorizontalAlign = HorizontalAlign.Center;

                            // fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);
                            string datetagvalue;
                            datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                            strdate = " and bio_attendance.access_date='" + datetagvalue + "'";

                            sql = "SELECT distinct staffmaster.staff_code ,desig_master.desig_acronym, hrdept_master.dept_acronym,CONVERT(VARCHAR(10),staffmaster.join_date,103) as join_date,in_out_time.category_name,staffmaster.staff_name ,desig_name ,hrdept_master.dept_acronym as 'Dept Acronym',hrdept_master.dept_name ,desig_acronym,access_date as 'Entry Date',right(CONVERT(nvarchar(100),time_in ,100),7) as time_in ,right(CONVERT(nvarchar(100),time_out ,100),7) as time_out,convert(char(8),(cast(time_out as datetime) - cast(time_in as datetime)),108) as TotalHours,att  FROM staffmaster,stafftrans,hrdept_master, desig_master,bio_attendance,In_Out_Time,staff_attnd where  (staffmaster.Fingerprint1 Is Not Null or staffmaster.Fingerprint1 Is Not Null) and hrdept_master.college_code=staffmaster.college_code and hrdept_master.dept_code=stafftrans.dept_code and staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled <>1 and staffmaster.resign <>1 and  stafftrans.latestrec<>0 and   staffmaster.staff_code=bio_attendance.roll_no and is_staff=1 and hrdept_master.dept_code=stafftrans.dept_code and desig_master.desig_code=stafftrans.desig_code and staffmaster.settled = 0 And staffmaster.resign = 0 and staffmaster.staff_code='" + staffcode1 + "' And In_Out_Time.Category_Code = Stafftrans.Category_Code And staffmaster.staff_code = stafftrans.staff_code And stafftrans.latestrec = 1 and in_out_time.shift = stafftrans.shift and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code=desig_master.collegecode  and staffmaster.college_code='" + cblcollege.SelectedItem.Value.ToString() + "'  " + strdate + " ";//this (in_out_time.shift = stafftrans.shift) condition added in this query by Manikandan 21/08/2013
                            con.Close();
                            con.Open();
                            SqlCommand cmd7 = new SqlCommand(sql, con);


                            SqlDataReader drcount14;
                            fpbiomatric.Width = 750;

                            drcount14 = cmd7.ExecuteReader();

                            while (drcount14.Read())
                            {
                                if (drcount14.HasRows == true)
                                {

                                    if (tempstaffcode == "")
                                    {

                                        fpbiomatric.Sheets[0].RowCount += 1;
                                        tempstaffcode = drcount14["staff_code"].ToString();
                                    }

                                    else if ((tempstaffcode != "") && (tempstaffcode != drcount14["staff_code"].ToString()))
                                    {



                                        fpbiomatric.Sheets[0].RowCount += 1;
                                        tempstaffcode = drcount14["staff_code"].ToString();
                                    }


                                    rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);
                                    Session["getrow"] = rowstr;
                                    string staffcode = "";
                                    string staffname = "";
                                    string dept_acronym = "";
                                    string category;
                                    string intime = "";
                                    string outtime = "";
                                    string horsin = "";
                                    string minin = "";
                                    string minin1 = "";
                                    string sesin = "";
                                    string horsto = "";
                                    string minto1 = "";
                                    string minto = "";
                                    string sesto = "";
                                    int hrint = 0;
                                    int htoutint = 0;
                                    staffname = drcount14["staff_name"].ToString();
                                    staffcode = drcount14["staff_code"].ToString();
                                    dept_acronym = drcount14["dept_acronym"].ToString();
                                    category = drcount14["category_name"].ToString();
                                    intime = drcount14["time_in"].ToString();
                                    if (intime != "")
                                    {
                                        // string[] split = date1.Split(new Char[] { '/' });
                                        string[] split50 = intime.Split(new char[] { ':' });
                                        horsin = split50[0];
                                        hrint = Convert.ToInt16(horsin);
                                        minin = split50[1];
                                        minin1 = minin.Substring(0, 2);
                                        sesin = minin.Substring(2);
                                    }

                                    outtime = drcount14["time_out"].ToString();
                                    if (outtime != "")
                                    {
                                        string[] split51 = outtime.Split(new char[] { ':' });
                                        horsto = split51[0];
                                        minto = split51[1];
                                        minto1 = minto.Substring(0, 2);
                                        sesto = minto.Substring(2);
                                        htoutint = Convert.ToInt16(horsto);
                                    }

                                    // int htoutint = 0;
                                    string att = "";
                                    string mrng5 = "";
                                    string eveng5 = "";
                                    att = drcount14["att"].ToString();
                                    if (att != "")
                                    {

                                        string[] tmpdate = att.ToString().Split(new char[] { '-' });


                                        mrng5 = tmpdate[0].ToString();
                                        eveng5 = tmpdate[1].ToString();
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = mrng5.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = eveng5;
                                    fpbiomatric.Sheets[0].RowHeader.Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(fpbiomatric.Sheets[0].RowCount - 1);//Added by Manikandan 09/08/2013
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = staffname;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Tag = staffcode;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = dept_acronym;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = category;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = outtime;

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = hrint.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = minin1;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = sesin;
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = htoutint.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 6].Text = minto1.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 7].Text = sesto.ToString();


                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].CellType = cmbcel1;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].CellType = cmbcel2;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].CellType = cmbcel3;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].CellType = cmbcel1;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].CellType = cmbcel2;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].CellType = cmbcel3;





                                    Double totalRows = 0;
                                    totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                                    if (totalRows >= 10)
                                    {
                                        fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                                        fpbiomatric.Height = 600;
                                        fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                        fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                                    }
                                    else if (totalRows == 0)
                                    {

                                        fpbiomatric.Height = 600;
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                                        fpbiomatric.Height = 125 + (125 * Convert.ToInt32(totalRows));
                                    }


                                    Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);

                                }
                            }

                        }

                    }

                }

            }

        }
        if (fpbiomatric.Sheets[0].RowCount == 1)
        {

            fpbiomatric.Visible = false;
            btnprintmaster.Visible = false;
            btnsave.Visible = false;//Added by Manikandan 08/08/2013
            btnsave.Enabled = false;
            lblnorec.Visible = true;
            btngo.Visible = false;
        }
    }
    void load_save()
    {

        lblselect.Visible = true;
        btngo.Visible = true;

        string staffcode;
        string category5;
        DateTime timein3;
        string hrins;
        string minins = "";
        string sesin = "";
        string timein5 = "";
        string hrouts = "";
        string minouts = "";
        string sesouts = "";
        string timeout5 = "";
        string datevalue1 = "";
        string moratt = "";
        string eveatt;
        string attendance;
        string leavetype = "";
        string leavetype2 = "";
        fpbiomatric.SaveChanges();
        //  getrow=int.Parse( Session["getrow"].ToString());
        for (int k = 4; k <= fpbiomatric.Sheets[0].ColumnCount - 1; k = k + 8)
        {
            for (int getrow2 = 1; getrow2 <= fpbiomatric.Sheets[0].RowCount - 1; getrow2++)
            {
                int isval = 0;
                isval = Convert.ToInt32(fpbiomatric.Sheets[0].Cells[getrow2, 0].Value);

                if (isval == 1)
                {
                    lblselect.Visible = false;
                    btngo.Visible = true;

                    count = count + 1;
                    timeout5 = "";

                    staffcode = Convert.ToString(fpbiomatric.Sheets[0].Cells[getrow2, 1].Tag);
                    category5 = Convert.ToString(fpbiomatric.Sheets[0].Cells[getrow2, 3].Text);
                    datevalue1 = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, k].Tag.ToString();
                    fpbiomatric.SaveChanges();
                    hrins = fpbiomatric.Sheets[0].Cells[getrow2, k + 2].Text.ToString();
                    minins = fpbiomatric.Sheets[0].GetText(getrow2, k + 3);
                    sesin = fpbiomatric.Sheets[0].GetText(getrow2, k + 4);
                    if (hrins != "")
                    {
                        timein5 = hrins + ":" + minins + "" + sesin;
                        string morpresent = getontime("Select right(CONVERT(nvarchar(100),intime ,100),7) as intime from in_out_time where category_name='" + category5 + "'");
                        string latetime = getlatetime("Select right(CONVERT(nvarchar(100),latetime ,100),7) as latetime from in_out_time where category_name='" + category5 + "'");
                        string gracetime = getgracetime("Select right(CONVERT(nvarchar(100),gracetime ,100),7) as gracetime from in_out_time where category_name='" + category5 + "'");
                        string outime3 = getouttime("Select right(CONVERT(nvarchar(100),outtime ,100),7) as outtime from in_out_time where category_name='" + category5 + "'");


                        string extndgracetime = getextendtime("Select right(CONVERT(nvarchar(100),extend_gracetime ,100),7) as extendgracetime from in_out_time where category_name='" + category5 + "'");

                        if (Convert.ToDateTime(timein5) <= Convert.ToDateTime(morpresent))
                        {
                            moratt = "P";
                        }
                        else if ((Convert.ToDateTime(timein5) >= Convert.ToDateTime(gracetime)) && (Convert.ToDateTime(timein5) <= (Convert.ToDateTime(latetime))))
                        {
                            moratt = "LA";
                        }
                        else if ((Convert.ToDateTime(timein5) >= Convert.ToDateTime(gracetime)) && (Convert.ToDateTime(timein5) <= (Convert.ToDateTime(extndgracetime))))
                        {
                            moratt = "PER";

                        }
                        else if (Convert.ToDateTime(timein5) >= Convert.ToDateTime(latetime))
                        {
                            moratt = "A";
                        }


                        hrouts = fpbiomatric.Sheets[0].Cells[getrow2, k + 5].Text.ToString();
                        minouts = fpbiomatric.Sheets[0].Cells[getrow2, k + 6].Text.ToString();
                        sesouts = fpbiomatric.Sheets[0].Cells[getrow2, k + 7].Text.ToString();
                        eveatt = "";

                        timeout5 = hrouts + ":" + minouts + "" + sesouts;
                        if (timeout5 != ":")
                        {
                            if ((Convert.ToDateTime(timeout5) <= Convert.ToDateTime(outime3)))
                            {
                                eveatt = "PER";
                            }
                            else
                            {
                                eveatt = "P";
                            }
                        }
                        else
                        {
                            timeout5 = null;
                        }
                        attendance = moratt + "-" + eveatt;
                        mysql = "select * from bio_attendance where Roll_no='" + staffcode + "' and access_date='" + datevalue1 + "'";
                        SqlCommand cmd60 = new SqlCommand(mysql, con);
                        con.Close();
                        con.Open();
                        SqlDataReader dr60 = cmd60.ExecuteReader();
                        dr60.Read();
                        if (dr60.HasRows == true)
                        {
                            mysql1.Close();
                            mysql1.Open();
                            SqlCommand cmd44 = new SqlCommand("update bio_attendance set time_in='" + timein5 + "', time_out='" + timeout5 + "',att='" + attendance + "' where Roll_no='" + staffcode + "' and access_date='" + datevalue1 + "'", mysql1);
                            SqlDataReader dr44;
                            dr44 = cmd44.ExecuteReader();
                            dr44.Read();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Update successfully')", true);
                        }
                        else
                        {

                            mysql1.Close();
                            mysql1.Open();
                            if (timeout5 != null)
                            {
                                SqlCommand cmd44 = new SqlCommand("Insert Into bio_attendance(roll_no,time_in,time_out,is_staff,access_date,att) values('" + staffcode + "','" + timein5 + "','" + timeout5 + "','1','" + datevalue1 + "','" + attendance + "')", mysql1);

                                SqlDataReader dr44;
                                dr44 = cmd44.ExecuteReader();
                                dr44.Read();
                            }
                            else
                            {
                                SqlCommand cmd44 = new SqlCommand("Insert Into bio_attendance(roll_no,time_in,is_staff,access_date,att) values('" + staffcode + "','" + timein5 + "','1','" + datevalue1 + "','" + attendance + "')", mysql1);

                                SqlDataReader dr44;
                                dr44 = cmd44.ExecuteReader();
                                dr44.Read();
                            }
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                        }


                        int day59 = 0;
                        int month59 = 0;
                        string[] split58 = datevalue1.Split('/');
                        string day58 = split58[1];
                        day59 = Convert.ToInt16(day58);
                        string month58 = split58[0];
                        month59 = Convert.ToInt16(month58);

                        //day57 = day57 + 3;

                        string motyear4 = month59 + "/" + split58[2];
                        sql1 = "select * from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + motyear4 + "'";
                        myatt.Close();
                        myatt.Open();
                        SqlCommand cmd90 = new SqlCommand(sql1, myatt);
                        SqlDataReader dr90 = cmd90.ExecuteReader();
                        dr90.Read();
                        if (dr90.HasRows == true)
                        {
                            mysql1.Close();
                            mysql1.Open();
                            SqlCommand cmd44 = new SqlCommand("update staff_attnd set[" + day59 + "]='" + attendance + "'where staff_code='" + staffcode + "' and  mon_year='" + motyear4 + "'", mysql1);
                            SqlDataReader dr44;
                            dr44 = cmd44.ExecuteReader();
                            dr44.Read();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Update successfully')", true);
                        }
                        else
                        {
                            mysql1.Close();
                            mysql1.Open();
                            SqlCommand cmd44 = new SqlCommand("Insert Into staff_attnd(staff_code,mon_year,[" + day59 + "]) values('" + staffcode + "','" + motyear4 + "','" + attendance + "')", mysql1);
                            SqlDataReader dr44;
                            dr44 = cmd44.ExecuteReader();
                            dr44.Read();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                        }

                    }
                    else
                    {
                        int day57 = 0;
                        int month57;
                        leavetype = fpbiomatric.Sheets[0].Cells[getrow2, k].Text.ToString();
                        leavetype2 = fpbiomatric.Sheets[0].Cells[getrow2, k + 1].Text.ToString();
                        string attend = "";
                        attend = leavetype + "-" + leavetype2;
                        string[] split56 = datevalue1.Split('/');
                        string day56 = split56[1];
                        day57 = Convert.ToInt16(day56);
                        string month56 = split56[0];
                        month57 = Convert.ToInt16(month56);

                        //day57 = day57 + 3;

                        string motyear = month57 + "/" + split56[2];

                        mysql = "select * from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + motyear + "'";
                        SqlCommand cmd60 = new SqlCommand(mysql, con);
                        con.Close();
                        con.Open();
                        SqlDataReader dr60 = cmd60.ExecuteReader();
                        dr60.Read();
                        if (dr60.HasRows == true)
                        {

                            mysql1.Close();
                            mysql1.Open();
                            SqlCommand cmd44 = new SqlCommand("update staff_attnd set[" + day57 + "]='" + attend + "'where staff_code='" + staffcode + "' and  mon_year='" + motyear + "'", mysql1);
                            SqlDataReader dr44;
                            dr44 = cmd44.ExecuteReader();
                            dr44.Read();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Update successfully')", true);

                        }
                        else
                        {
                            mysql1.Close();
                            mysql1.Open();
                            SqlCommand cmd44 = new SqlCommand("Insert Into staff_attnd(staff_code,mon_year,[" + day57 + "]) values('" + staffcode + "','" + motyear + "','" + attend + "')", mysql1);
                            SqlDataReader dr44;
                            dr44 = cmd44.ExecuteReader();
                            dr44.Read();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                        }
                    }
                }
                else
                {
                    if (count == 0)
                    {

                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please  Select The Staff')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                    }

                }
            }
        }


    }
    public string getontime(string sql4)
    {
        string sqlon;
        sqlon = sql4;
        mycon.Close();
        mycon.Open();
        string ontime2 = "";
        SqlCommand cmd50 = new SqlCommand(sqlon, mycon);
        SqlDataReader dr50;
        dr50 = cmd50.ExecuteReader();
        while (dr50.Read())
            if (dr50.HasRows == true)
            {
                ontime2 = dr50["intime"].ToString();
            }
        return ontime2;
    }


    public string getlatetime(string sql4)
    {
        string sqlon;
        sqlon = sql4;
        mycon.Close();
        mycon.Open();
        string latetime = "";
        SqlCommand cmd50 = new SqlCommand(sqlon, mycon);
        SqlDataReader dr50;
        dr50 = cmd50.ExecuteReader();
        while (dr50.Read())
            if (dr50.HasRows == true)
            {
                latetime = dr50["latetime"].ToString();



            }
        return latetime;
    }
    public string getgracetime(string sql4)
    {
        string sqlon;
        sqlon = sql4;
        mycon.Close();
        mycon.Open();
        string gracetime2 = "";
        SqlCommand cmd50 = new SqlCommand(sqlon, mycon);
        SqlDataReader dr50;
        dr50 = cmd50.ExecuteReader();
        while (dr50.Read())
            if (dr50.HasRows == true)
            {
                gracetime2 = dr50["gracetime"].ToString();



            }
        return gracetime2;
    }

    public string getouttime(string sql4)
    {
        string sqlon;
        sqlon = sql4;
        mycon.Close();
        mycon.Open();
        string outtime5 = "";
        SqlCommand cmd50 = new SqlCommand(sqlon, mycon);
        SqlDataReader dr50;
        dr50 = cmd50.ExecuteReader();
        while (dr50.Read())
            if (dr50.HasRows == true)
            {
                outtime5 = dr50["outtime"].ToString();



            }
        return outtime5;
    }


    //extendgracetime
    //getextendtime

    public string getextendtime(string sql4)
    {
        string sqlon;
        sqlon = sql4;
        mycon.Close();
        mycon.Open();
        string extentime = "";
        SqlCommand cmd50 = new SqlCommand(sqlon, mycon);
        SqlDataReader dr50;
        dr50 = cmd50.ExecuteReader();
        while (dr50.Read())
            if (dr50.HasRows == true)
            {
                extentime = dr50["extendgracetime"].ToString();



            }
        return extentime;
    }

    protected void cbldepttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        pseattype.Focus();
        // cbldepttype.Focus();
        int seatcount = 0;
        string value = "";
        string code = "";
        //LinkButtonseattype.Visible = true;//Hided by Manikandan 09/08/2013

        for (int i = 0; i < cbldepttype.Items.Count; i++)
        {
            if (cbldepttype.Items[i].Selected == true)
            {
                value = cbldepttype.Items[i].Text;
                code = cbldepttype.Items[i].Value.ToString();
                seatcount = seatcount + 1;
                tbseattype.Text = "Department(" + seatcount.ToString() + ")";

                if ((staffdept == "")) //Added by Manikandan
                {
                    staffdept = cbldepttype.Items[i].Value.ToString();

                }
                else
                {

                    staffdept = staffdept + "," + cbldepttype.Items[i].Value.ToString();
                }
                //-------------
            }



        }

        if (seatcount == 0)
        {
            tbseattype.Text = "---Select---";
        }
        //Start============Hided by Manikandan 09/08/2013=================
        //else
        //{
        //    Label lbl = seatlabel();
        //    lbl.Text = " " + value + " ";
        //    lbl.ID = "lbl1-" + code.ToString();
        //    ImageButton ib = seatimage();
        //    ib.ID = "imgbut1_" + code.ToString();
        //    ib.Click += new ImageClickEventHandler(seatimg_Click);
        //}
        seatcnt = seatcount;
        load_staffname(staffdept);//Modified by Manikandan
    }

    //public ImageButton seatimage()
    //{
    //    ImageButton imc = new ImageButton();
    //    imc.ImageUrl = "xb.jpeg";
    //    imc.Height = 9;
    //    imc.Width = 9;

    //    PlaceHolderseattype.Controls.Add(imc);
    //    ViewState["iseatcontrol"] = true;
    //    return (imc);
    //}

    //public Label seatlabel()
    //{
    //    Label lbc = new Label();
    //    PlaceHolderseattype.Controls.Add(lbc);
    //    ViewState["lseatcontrol"] = true;
    //    return (lbc);
    //}
    //public void seatimg_Click(object sender, ImageClickEventArgs e)
    //{
    //    seatcnt = seatcnt - 1;
    //    ImageButton b = sender as ImageButton;
    //    int r = Convert.ToInt32(b.CommandArgument);
    //    cbldepttype.Items[r].Selected = false;

    //    tbseattype.Text = "Department(" + seatcnt.ToString() + ")";
    //    if (tbseattype.Text == "Department(0)")
    //    {
    //        tbseattype.Text = "---Select---";
    //        LinkButtonseattype.Visible = false;
    //    }
    //    int p = PlaceHolderseattype.Controls.IndexOf(b);
    //    PlaceHolderseattype.Controls.RemoveAt(p - 1);
    //    PlaceHolderseattype.Controls.Remove(b);
    //}

    //================================End=============================


    protected void cblcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        int bloodcount = 0;
        string value = "";
        string code = "";

        //LinkButtonblood.Visible = true;//Hided by Manikandan 09/08/2013
        for (int i = 0; i < cblcategory.Items.Count; i++)
        {
            if (cblcategory.Items[i].Selected == true)
            {
                value = cblcategory.Items[i].Text;

                code = cblcategory.Items[i].Value.ToString();
                bloodcount = bloodcount + 1;
                tbblood.Text = "Category(" + bloodcount.ToString() + ")";
            }
        }
        if (bloodcount == 0)
        {
            tbblood.Text = "---Select---";
        }
        //Start======================Hided by Manikandan 09/08/2013
        //else
        //{
        //    Label lbl = bloodlabel();
        //    lbl.Text = " " + value + " ";
        //    lbl.ID = "lbl2-" + code.ToString();
        //    ImageButton ib = bloodimage();
        //    ib.ID = "imgbut2_" + code.ToString();
        //    ib.Click += new ImageClickEventHandler(bloodimg_Click);
        //}
        bloodcnt = bloodcount;
        load_staffname(staffdept);
    }


    //public ImageButton bloodimage()
    //{
    //    ImageButton imc = new ImageButton();
    //    imc.ImageUrl = "xb.jpeg";
    //    imc.Height = 9;
    //    imc.Width = 9;

    //    PlaceHolderblood.Controls.Add(imc);
    //    ViewState["ibloodcontrol"] = true;
    //    return (imc);
    //}
    //public Label bloodlabel()
    //{
    //    Label lbc = new Label();
    //    PlaceHolderblood.Controls.Add(lbc);
    //    ViewState["lbloodcontrol"] = true;
    //    return (lbc);
    //}

    //public void bloodimg_Click(object sender, ImageClickEventArgs e)
    //{
    //    bloodcnt = bloodcnt - 1;
    //    ImageButton b = sender as ImageButton;
    //    int r = Convert.ToInt32(b.CommandArgument);
    //    cblcategory.Items[r].Selected = false;
    //    tbblood.Text = "Blood Group(" + bloodcnt.ToString() + ")";
    //    if (tbblood.Text == "Blood Group(0)")
    //    {
    //        LinkButtonblood.Visible = false;
    //        tbblood.Text = "---Select---";
    //    }
    //    int p = PlaceHolderblood.Controls.IndexOf(b);
    //    PlaceHolderblood.Controls.RemoveAt(p - 1);
    //    PlaceHolderblood.Controls.Remove(b);

    //}

    //====================================End========================================

    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        if (chkselect.Checked == true)
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                cbldepttype.Items[i].Selected = true;
                tbseattype.Text = "Department(" + (cbldepttype.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                cbldepttype.Items[i].Selected = false;
                tbseattype.Text = "--Select--";
            }
        }
        cbldepttype_SelectedIndexChanged(sender, e);//Added by Manikandan 21/08/2013
    }
    protected void chkcategory_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcategory.Checked == true)
        {
            for (int i = 0; i < cblcategory.Items.Count; i++)
            {
                cblcategory.Items[i].Selected = true;
                tbblood.Text = "Category(" + (cblcategory.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cblcategory.Items.Count; i++)
            {
                //cblcategory.Items[i].Selected = true;//Hided by Manikandan 09/08/2013
                cblcategory.Items[i].Selected = false;//Modified by Manikandan 09/08/2013
                tbblood.Text = "--Select--";
            }
        }

    }
    protected void Txtentryto_TextChanged(object sender, EventArgs e)
    {
        //load_btnclick();
    }
    //Start================Hided by Manikandan 09/08/2013====================
    //protected void LinkButtonseattype_Click(object sender, EventArgs e)
    //{
    //    cbldepttype.ClearSelection();
    //    PlaceHolderseattype.Controls.Clear();
    //    seatcnt = 0;
    //    tbseattype.Text = "---Select---";
    //    LinkButtonseattype.Visible = false;
    //}
    //protected void LinkButtonblood_Click(object sender, EventArgs e)
    //{
    //    cblcategory.ClearSelection();
    //    PlaceHolderblood.Controls.Clear();
    //    bloodcnt = 0;
    //    tbblood.Text = "---Select---";
    //    LinkButtonblood.Visible = false;
    //}
    //================================End====================================
    protected void tbblood_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        load_save();
    }

    protected void fpbiomatric_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();
        //int ar = Convert.ToInt32(actrow);
        //string seltext1 = fpbiomatric.GetEditValue(ar, 5).ToString();
        //if (seltext1 == "CL")
        //{
        //    fpbiomatric.Sheets[0].Cells[ar, 7].Locked = true;
        //    fpbiomatric.Sheets[0].Cells[ar, 8].Locked = true;
        //}

        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(fpbiomatric.Sheets[0].RowCount); j++)
            {
                string actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "System.Object")
                    fpbiomatric.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
            }
            flag_true = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        load_go();
    }

    void load_go()
    {

        Session["datefrom"] = Txtentryfrom.Text;
        Session["dateto"] = Txtentryto.Text;
        Response.Redirect("biomatric.aspx");
    }
    protected void cblcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_dept();
        load_category();
        load_staffname(staffdept);
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;
        string coll = string.Empty;
        string cat = string.Empty;

        if (rdoall.Checked == true)
        {
            cat = " for Register & Unregister Staff";
        }
        else if (rdounreg.Checked == true)
        {
            cat = " for Unregistered Staff";
        }
        else if (rdoreg.Checked == true)
        {
            cat = " for Registered Staff";
        }

        Session["column_header_row_count"] = fpbiomatric.Sheets[0].ColumnHeader.RowCount;

        degreedetails = "Biocorrection Report" + cat + "@Date: " + Txtentryfrom.Text.ToString() + " To " + Txtentryto.Text.ToString();
        string pagename = "biocorrection.aspx";
        Printcontrol.loadspreaddetails(fpbiomatric, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
}