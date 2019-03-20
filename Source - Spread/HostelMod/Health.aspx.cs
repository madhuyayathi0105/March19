using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class HostelMod_Health : System.Web.UI.Page
{
    string college = "";
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Boolean Cellclick = false;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    static string buttonvalue = "";
    int i = 0;
    int j = 0;
    string getday = "";
    string gettoday = "";
    string sql = "";
    int commcount;
    string buildvalue = "";
    string build = "";
    string itemheader = "";
    string commname = "";
    string hostel = "";
    string hostelcode = "";
    static string hostel_name_code = "";
    string rollno = "";
    string regno = "";
    string name = "";
    string degree = "";
    string hostlnm = "";
    string date = "";
    string amount = "";
    string activerow = "";
    string activecol = "";
    int sno = 0;
    int selected = 0;
    ReuasableMethods rs = new ReuasableMethods();
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
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
            bindhostelname();
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            description();
            mainspread.Visible = false;
            printdiv.Visible = false;
            rptprint.Visible = false;
            #region popupStaff
            loadcollegestaffpopup();
            bindstaffdepartmentpopup();
            #endregion
        }
    }


    #region BindHostel
    public void bindhostelname()
    {
        try
        {
            ds.Clear();
            cbl_hostelname.Items.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_hostelname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname.Checked == true)
            {
                for (i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                }
                txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
            }
            else
            {
                for (i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                }
                txt_hostelname.Text = "--Select--";
            }
            mainspread.Visible = false;
            printdiv.Visible = false;
            rptprint.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_hostlname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            commcount = 0;
            txt_hostelname.Text = "--Select--";
            cb_hostelname.Checked = false;
            for (i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
                if (commcount == cbl_hostelname.Items.Count)
                {
                    cb_hostelname.Checked = true;
                }
            }
            mainspread.Visible = false;
            printdiv.Visible = false;
            rptprint.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region FromDate
    protected void txt_fromdate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                mainspread.Visible = false;
                printdiv.Visible = false;
                rptprint.Visible = false;
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Enter FromDate less than or equal to the ToDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region ToDate
    protected void txt_todate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_todate.Text != "" && txt_fromdate.Text != "")
            {
                mainspread.Visible = false;
                printdiv.Visible = false;
                rptprint.Visible = false;
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Enter ToDate greater than or equal to the FromDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region Go
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string hostename = string.Empty;
            string from = "";
            string to = "";
            int index;
            DataSet dsstud = new DataSet();

            if (cbl_hostelname.Items.Count > 0)
                hostename = rs.GetSelectedItemsValueAsString(cbl_hostelname);
            from = Convert.ToString(txt_fromdate.Text);
            string[] splitdate = from.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            getday = dt.ToString("MM/dd/yyyy");
            to = Convert.ToString(txt_todate.Text);
            string[] splitdate1 = to.Split('-');
            splitdate1 = splitdate1[0].Split('/');
            DateTime dt1 = new DateTime();
            if (splitdate1.Length > 0)
            {
                dt1 = Convert.ToDateTime(splitdate1[1] + "/" + splitdate1[0] + "/" + splitdate1[2]);
            }
            gettoday = dt1.ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(hostename) && !string.IsNullOrEmpty(getday) && !string.IsNullOrEmpty(gettoday))
            {
                sql = "select sd.HealthAdditionalAmt as Amount,hr.APP_No,r.Roll_No,r.Stud_Name ,r.Reg_No,hm.HostelMasterPK,hm.HostelName ,(c.Course_Name+' - '+dt.Dept_Name)as Degree,CONVERT(varchar(10),TransDate ,103)as Transdate,m.MasterValue  from HT_HealthCheckup sd,Registration r,Degree d,Department dt,Course c,HM_HostelMaster hm,HT_HostelRegistration hr,CO_MasterValues m where m.MasterCode=sd.HealthDesc and m.MasterCriteria='Expense' and sd.App_No =r.App_No and sd.App_No =hr.APP_No and hr.HostelMasterFK =hm.HostelMasterPK and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and C.Course_Id =d.Course_Id and TransDate between '" + getday + "' and '" + gettoday + "' and hm.HostelMasterPK in ('" + hostename + "') group by hr.APP_No, r.Roll_No,r.Stud_Name,r.Reg_No ,hm.HostelMasterPK,hm.HostelName ,(c.Course_Name+' - '+dt.Dept_Name),Transdate,MasterValue,HealthAdditionalAmt;";


                sql += "select hsd.APP_No,hc.HealthAdditionalAmt as Amount,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,hd.HostelName,CONVERT(varchar(10),TransDate ,103)as Transdate,m.MasterValue  from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st,HT_HealthCheckup hc,CO_MasterValues m  where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code and hc.App_No=hsd.APP_No and m.MasterCriteria='Expense' and m.MasterCode=hc.HealthDesc   and TransDate between '" + getday + "' and '" + gettoday + "' and hsd.HostelMasterFK in ('" + hostename + "') group by hsd.APP_No, hd.HostelName ,Transdate,MasterValue,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,HealthAdditionalAmt;";

                sql += "select hc.HealthAdditionalAmt as Amount,HM.HostelName as Hostel_Name,Vi.VenContactName as Guest_Name,Vi.VendorContactPK as GuestCode,CONVERT(varchar(10),TransDate ,103)as Transdate,m.MasterValue, HM.HostelMasterPK as Hostel_Code from HT_HostelRegistration H,CO_VendorMaster V,IM_VendorContactMaster Vi,HM_HostelMaster HM,HT_HealthCheckup hc,CO_MasterValues m   where hm.HostelMasterPK =h.HostelMasterFK and v.VendorPK=vi.VendorFK  and H.GuestVendorFK=v.VendorPK and vi.VendorContactPK=h.APP_No and hc.App_no=vi.VendorContactPK and hc.App_no=h.APP_No and m.MasterCriteria='Expense' and m.MasterCode=hc.HealthDesc  and TransDate between '" + getday + "' and '" + gettoday + "' and H.HostelMasterFK in ('" + hostename + "')  group by VendorContactPK, HostelName ,Transdate,MasterValue,VenContactName,HostelMasterPK,HealthAdditionalAmt";

                dsstud.Clear();
                dsstud = d2.select_method_wo_parameter(sql, "Text");
            }

            if ((dsstud.Tables.Count > 0 && dsstud.Tables[0].Rows.Count > 0) || (dsstud.Tables.Count > 0 && dsstud.Tables[1].Rows.Count > 0) || (dsstud.Tables.Count > 0 && dsstud.Tables[2].Rows.Count > 0))
            {
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.SaveChanges();
                Fpspread1.SheetCorner.ColumnCount = 0;
                Fpspread1.CommandBar.Visible = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                Fpspread1.Sheets[0].AutoPostBack = true;

                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 7;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Columns[0].Width = 80;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "App No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[1].Locked = true;
                Fpspread1.Columns[1].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[2].Locked = true;
                Fpspread1.Columns[2].Width = 200;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hostel Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[3].Locked = true;
                Fpspread1.Columns[3].Width = 200;


                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Date";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[4].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Description";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[4].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Amount";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[4].Width = 150;
                if (dsstud.Tables[0].Rows.Count > 0)
                {
                    for (int gorow = 0; gorow < dsstud.Tables[0].Rows.Count; gorow++)
                    {
                        sno++;
                        Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsstud.Tables[0].Rows[gorow]["APP_No"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsstud.Tables[0].Rows[gorow]["Stud_Name"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsstud.Tables[0].Rows[gorow]["HostelName"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsstud.Tables[0].Rows[gorow]["Transdate"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsstud.Tables[0].Rows[gorow]["MasterValue"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dsstud.Tables[0].Rows[gorow]["Amount"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    }
                }
                if (dsstud.Tables[1].Rows.Count > 0)
                {
                    for (int gorow1 = 0; gorow1 < dsstud.Tables[1].Rows.Count; gorow1++)
                    {
                        sno++;
                        Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsstud.Tables[1].Rows[gorow1]["APP_No"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsstud.Tables[1].Rows[gorow1]["staff_name"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsstud.Tables[1].Rows[gorow1]["HostelName"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsstud.Tables[1].Rows[gorow1]["Transdate"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsstud.Tables[1].Rows[gorow1]["MasterValue"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dsstud.Tables[1].Rows[gorow1]["Amount"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    }
                }

                if (dsstud.Tables[2].Rows.Count > 0)
                {
                    for (int gorow2 = 0; gorow2 < dsstud.Tables[2].Rows.Count; gorow2++)
                    {
                        sno++;
                        Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsstud.Tables[2].Rows[gorow2]["GuestCode"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsstud.Tables[2].Rows[gorow2]["Guest_Name"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsstud.Tables[2].Rows[gorow2]["Hostel_Name"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsstud.Tables[2].Rows[gorow2]["Transdate"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsstud.Tables[2].Rows[gorow2]["MasterValue"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dsstud.Tables[2].Rows[gorow2]["Amount"]);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    }
                }

                Fpspread1.Height = 345;
                Fpspread1.Width = 900;
                Fpspread1.Visible = true;
                mainspread.Visible = true;
                Fpspread1.SaveChanges();
                printdiv.Visible = true;
                rptprint.Visible = true;
            }
            else
            {
                Fpspread1.Visible = false;
                mainspread.Visible = false;
                printdiv.Visible = false;
                rptprint.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Records Found";

            }


        }


        catch
        {

        }
    }
    #endregion

    #region Fpspread1CellClick
    protected void Fpspread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
            popupstudaddinl.Visible = true;
            btn_save.Text = "Update";
            btn_save.Visible = true;
            btn_save2.Visible = false;
            btn_save1.Visible = false;
            btn_delete.Visible = true;
            lbl_stu.Visible = false;
            txt_stu.Visible = false;

        }
        catch
        {

        }
    }

    protected void Fpspread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {

                Fpspread1.SaveChanges();
                txt_degree.Enabled = false;
                txt_rollno.Enabled = false;
                txt_regno.Enabled = false;
                txt_name.Enabled = false;
                txt_guestname.Enabled = false;
                txt_gustCode.Enabled = false;
                string activerow = "";
                string qry = string.Empty;
                DataSet dsdetails = new DataSet();
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                if (activerow.Trim() != "")
                {

                    string Appno = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    if (Appno != "")
                    {
                        qry = "select sd.HealthAdditionalAmt as Amount,hr.APP_No,r.Roll_No,r.Stud_Name ,r.Reg_No,hm.HostelMasterPK,hm.HostelName ,(c.Course_Name+' - '+dt.Dept_Name)as Degree,CONVERT(varchar(10),TransDate ,103)as Transdate,m.MasterValue  from HT_HealthCheckup sd,Registration r,Degree d,Department dt,Course c,HM_HostelMaster hm,HT_HostelRegistration hr,CO_MasterValues m where m.MasterCode=sd.HealthDesc and m.MasterCriteria='Expense' and sd.App_No =r.App_No and sd.App_No =hr.APP_No and hr.HostelMasterFK =hm.HostelMasterPK and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and C.Course_Id =d.Course_Id and sd.App_No='" + Appno + "' group by hr.APP_No, r.Roll_No,r.Stud_Name,r.Reg_No ,hm.HostelMasterPK,hm.HostelName ,(c.Course_Name+' - '+dt.Dept_Name),Transdate,MasterValue,HealthAdditionalAmt;";
                        qry += "select hsd.APP_No,hc.HealthAdditionalAmt as Amount,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,hd.HostelName,CONVERT(varchar(10),TransDate ,103)as Transdate,m.MasterValue  from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st,HT_HealthCheckup hc,CO_MasterValues m  where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code and hc.App_No=hsd.APP_No and m.MasterCriteria='Expense' and m.MasterCode=hc.HealthDesc  and  hc.App_No='" + Appno + "' group by hsd.APP_No, hd.HostelName ,Transdate,MasterValue,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,HealthAdditionalAmt;";
                        qry += "select hc.HealthAdditionalAmt as Amount,HM.HostelName as Hostel_Name,Vi.VenContactName as Guest_Name,Vi.VendorContactPK as GuestCode,CONVERT(varchar(10),TransDate ,103)as Transdate,m.MasterValue, HM.HostelMasterPK as Hostel_Code from HT_HostelRegistration H,CO_VendorMaster V,IM_VendorContactMaster Vi,HM_HostelMaster HM,HT_HealthCheckup hc,CO_MasterValues m   where hm.HostelMasterPK =h.HostelMasterFK and v.VendorPK=vi.VendorFK  and H.GuestVendorFK=v.VendorPK and vi.VendorContactPK=h.APP_No and hc.App_no=vi.VendorContactPK and hc.App_no=h.APP_No and m.MasterCriteria='Expense' and m.MasterCode=hc.HealthDesc  and  hc.App_No='" + Appno + "'  group by VendorContactPK, HostelName ,Transdate,MasterValue,VenContactName,HostelMasterPK,HealthAdditionalAmt";
                        dsdetails.Clear();
                        dsdetails = d2.select_method_wo_parameter(qry, "Text");

                    }

                    if ((dsdetails.Tables.Count > 0 && dsdetails.Tables[0].Rows.Count > 0) || (dsdetails.Tables.Count > 0 && dsdetails.Tables[1].Rows.Count > 0) || (dsdetails.Tables.Count > 0 && dsdetails.Tables[2].Rows.Count > 0))
                    {
                        if (dsdetails.Tables[0].Rows.Count > 0)
                        {
                            rblstustaffguest.Items[0].Selected = true;
                            rblstustaffguest.Items[1].Enabled = false;
                            rblstustaffguest.Items[2].Enabled = false;
                            lbl_rollno.Visible = true;
                            txt_rollno.Visible = true;
                            btn_rollno.Visible = true;
                            lbl_regno.Visible = true;
                            txt_regno.Visible = true;
                            lbl_name.Visible = true;
                            txt_name.Visible = true;
                            lbl_degree.Visible = true;
                            txt_degree.Visible = true;
                            dept.Visible = true;
                            design.Visible = true;
                            txt_guestname.Visible = false;
                            btn_guestname.Visible = false;
                            lbl_guCode.Visible = false;
                            txt_gustCode.Visible = false;
                            lbl_guestname.Visible = false;
                            lbl_pop1staffname.Visible = false;
                            txt_pop1staffname.Visible = false;
                            btnstaffname.Visible = false;
                            lbl_staffcode.Visible = false;
                            txt_staffcode.Visible = false;
                            lbl_dept.Visible = false;
                            txt_dept.Visible = false;
                            lbl_design.Visible = false;
                            txt_design.Visible = false;

                            txt_rollno.Text = Convert.ToString(dsdetails.Tables[0].Rows[0]["Roll_No"]);
                            txt_regno.Text = Convert.ToString(dsdetails.Tables[0].Rows[0]["Reg_No"]);
                            txt_name.Text = Convert.ToString(dsdetails.Tables[0].Rows[0]["Stud_Name"]);
                            txt_degree.Text = Convert.ToString(dsdetails.Tables[0].Rows[0]["Degree"]);
                            txt_hostelname1.Text = Convert.ToString(dsdetails.Tables[0].Rows[0]["HostelName"]);
                            txt_date.Text = Convert.ToString(dsdetails.Tables[0].Rows[0]["Transdate"]);
                            //ddl_description.SelectedIndex = ddl_description.Items.IndexOf(ddl_description.Items.FindByText(Convert.ToString(dsdetails.Tables[0].Rows[0]["MasterValue"])));
                            ddl_description.SelectedItem.Text = Convert.ToString(dsdetails.Tables[0].Rows[0]["MasterValue"]);
                            txt_amount.Text = Convert.ToString(dsdetails.Tables[0].Rows[0]["Amount"]);
                        }
                        if (dsdetails.Tables[1].Rows.Count > 0)
                        {
                            rblstustaffguest.Items[0].Enabled = false;
                            rblstustaffguest.Items[1].Selected = true;
                            rblstustaffguest.Items[2].Enabled = false;
                            lbl_pop1staffname.Visible = true;
                            txt_pop1staffname.Visible = true;
                            btnstaffname.Visible = true;
                            lbl_staffcode.Visible = true;
                            txt_staffcode.Visible = true;
                            lbl_dept.Visible = true;
                            txt_dept.Visible = true;
                            lbl_design.Visible = true;
                            txt_design.Visible = true;
                            dept.Visible = true;
                            design.Visible = true;
                            lbl_rollno.Visible = false;
                            txt_rollno.Visible = false;
                            btn_rollno.Visible = false;
                            lbl_regno.Visible = false;
                            txt_regno.Visible = false;
                            lbl_name.Visible = false;
                            txt_name.Visible = false;
                            lbl_degree.Visible = false;
                            txt_degree.Visible = false;
                            dept.Visible = false;
                            design.Visible = false;
                            txt_guestname.Visible = false;
                            btn_guestname.Visible = false;
                            lbl_guCode.Visible = false;
                            txt_gustCode.Visible = false;
                            lbl_guestname.Visible = false;
                            txt_pop1staffname.Text = Convert.ToString(dsdetails.Tables[1].Rows[0]["staff_name"]);
                            txt_staffcode.Text = Convert.ToString(dsdetails.Tables[1].Rows[0]["staff_code"]);
                            txt_dept.Text = Convert.ToString(dsdetails.Tables[1].Rows[0]["dept_name"]);
                            txt_design.Text = Convert.ToString(dsdetails.Tables[1].Rows[0]["desig_name"]);
                            txt_hostelname1.Text = Convert.ToString(dsdetails.Tables[1].Rows[0]["HostelName"]);
                            txt_date.Text = Convert.ToString(dsdetails.Tables[1].Rows[0]["Transdate"]);
                            //ddl_description.SelectedIndex = ddl_description.Items.IndexOf(ddl_description.Items.FindByText(Convert.ToString(dsdetails.Tables[1].Rows[0]["MasterValue"])));
                            ddl_description.SelectedItem.Text = Convert.ToString(dsdetails.Tables[1].Rows[0]["MasterValue"]);
                            txt_amount.Text = Convert.ToString(dsdetails.Tables[1].Rows[0]["Amount"]);
                        }
                        if (dsdetails.Tables[2].Rows.Count > 0)
                        {
                            rblstustaffguest.Items[0].Enabled = false;
                            rblstustaffguest.Items[1].Enabled = false;
                            rblstustaffguest.Items[2].Selected = true;
                            txt_guestname.Visible = true;
                            btn_guestname.Visible = true;
                            lbl_guCode.Visible = true;
                            txt_gustCode.Visible = true;
                            lbl_guestname.Visible = true;
                            lbl_pop1staffname.Visible = false;
                            txt_pop1staffname.Visible = false;
                            btnstaffname.Visible = false;
                            lbl_staffcode.Visible = false;
                            txt_staffcode.Visible = false;
                            lbl_dept.Visible = false;
                            txt_dept.Visible = false;
                            lbl_design.Visible = false;
                            txt_design.Visible = false;
                            lbl_rollno.Visible = false;
                            txt_rollno.Visible = false;
                            btn_rollno.Visible = false;
                            lbl_regno.Visible = false;
                            txt_regno.Visible = false;
                            lbl_name.Visible = false;
                            txt_name.Visible = false;
                            lbl_degree.Visible = false;
                            txt_degree.Visible = false;
                            dept.Visible = false;
                            design.Visible = false;
                            txt_guestname.Text = Convert.ToString(dsdetails.Tables[2].Rows[0]["Guest_Name"]);
                            txt_gustCode.Text = Convert.ToString(dsdetails.Tables[2].Rows[0]["GuestCode"]);
                            txt_hostelname1.Text = Convert.ToString(dsdetails.Tables[2].Rows[0]["Hostel_Name"]);
                            txt_date.Text = Convert.ToString(dsdetails.Tables[2].Rows[0]["Transdate"]);
                            //ddl_description.SelectedIndex = ddl_description.Items.IndexOf(ddl_description.Items.FindByText(Convert.ToString(dsdetails.Tables[2].Rows[0]["MasterValue"])));
                            ddl_description.SelectedItem.Text = Convert.ToString(dsdetails.Tables[2].Rows[0]["MasterValue"]);
                            txt_amount.Text = Convert.ToString(dsdetails.Tables[2].Rows[0]["Amount"]);
                        }
                    }

                }
            }

        }
        catch
        {

        }
    }

    #endregion

    #region Delete
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to Delete this Record?";
        }
        catch
        {

        }

    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {
            string hosstudstaffrollno = string.Empty;
            string stuapp_no = string.Empty;
            string staffapp_no = string.Empty;
            string qryhostudgymdelete = string.Empty;
            string HeaderFK = string.Empty;
            string LedgerFK = string.Empty;
            string paid = string.Empty;
            double amt = 0;
            string qry = string.Empty;
            DataSet dshealth = new DataSet();
            DataSet dsfee = new DataSet();
            int query = 0;
            //header and ledger
            string healthfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Health' and collegecode='" + ddl_collegestaff.SelectedValue + "'";
            dshealth.Clear();
            dshealth = d2.select_method_wo_parameter(healthfeeset, "Text");
            if (dshealth.Tables.Count > 0 && dshealth.Tables[0].Rows.Count > 0)
            {
                HeaderFK = Convert.ToString(dshealth.Tables[0].Rows[0]["header"]);
                LedgerFK = Convert.ToString(dshealth.Tables[0].Rows[0]["ledger"]);
                //exincludemessbill = Convert.ToString(dshealth.Tables[0].Rows[0]["Text_value"]);
            }
            if (rblstustaffguest.SelectedIndex == 0)
            {
                if (txt_rollno.Text != "")
                {
                    hosstudstaffrollno = txt_rollno.Text;
                    stuapp_no = d2.GetFunction("select app_no from Registration where Roll_No='" + hosstudstaffrollno + "'");
                    if (stuapp_no.Trim() != "0" && stuapp_no.Trim() != "")
                    {
                        qryhostudgymdelete = "  delete HT_HealthCheckup where APP_No='" + stuapp_no + "' and MemType='1'";
                        query = d2.update_method_wo_parameter(qryhostudgymdelete, "Text");
                    }
                    paid = "select paidamount from ft_feeallot where app_no='" + stuapp_no + "'";
                    dsfee.Clear();
                    dsfee = d2.select_method_wo_parameter(paid, "text");
                    if (dsfee.Tables.Count > 0)
                    {
                        if (dsfee.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsfee.Tables[0].Rows.Count; i++)
                            {
                                amt = Convert.ToDouble(dsfee.Tables[0].Rows[i]["paidamount"]);
                                if (amt == 0.00)
                                {
                                    qry = "delete from ft_Feeallot where app_no='" + stuapp_no + "' and paidamount='0' and headerfk in('" + HeaderFK + "') and ledgerfk in('" + LedgerFK + "') and MemType='1'";
                                    query = d2.update_method_wo_parameter(qry, "Text");
                                }
                            }
                        }
                    }
                }
            }
            else if (rblstustaffguest.SelectedIndex == 1)
            {
                if (txt_staffcode.Text != "")
                {
                    hosstudstaffrollno = txt_staffcode.Text;
                    staffapp_no = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + hosstudstaffrollno + "' and sam.appl_no = sm.appl_no");

                    if (staffapp_no.Trim() != "0" && staffapp_no.Trim() != "")
                    {
                        qryhostudgymdelete = "  delete HT_HealthCheckup where APP_No='" + staffapp_no + "' and MemType='2'";
                        query = d2.update_method_wo_parameter(qryhostudgymdelete, "Text");
                    }
                    paid = "select paidamount from ft_feeallot where app_no='" + staffapp_no + "'";
                    dsfee.Clear();
                    dsfee = d2.select_method_wo_parameter(paid, "text");
                    if (dsfee.Tables.Count > 0)
                    {
                        if (dsfee.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsfee.Tables[0].Rows.Count; j++)
                            {
                                amt = Convert.ToDouble(dsfee.Tables[0].Rows[j]["paidamount"]);
                                if (amt == 0.00)
                                {
                                    qry = "delete from ft_Feeallot where app_no='" + staffapp_no + "' and paidamount='0' and headerfk in('" + HeaderFK + "') and ledgerfk in('" + LedgerFK + "') and MemType='2'";
                                    query = d2.update_method_wo_parameter(qry, "Text");
                                }
                            }
                        }
                    }

                }

            }
            else
            {
                if (txt_gustCode.Text != "")
                {
                    hosstudstaffrollno = txt_gustCode.Text;
                    if (hosstudstaffrollno.Trim() != "0" && hosstudstaffrollno.Trim() != "")
                    {
                        qryhostudgymdelete = "  delete HT_HealthCheckup where APP_No='" + hosstudstaffrollno + "' and MemType='3'";
                        query = d2.update_method_wo_parameter(qryhostudgymdelete, "Text");
                    }
                    paid = "select paidamount from ft_feeallot where app_no='" + hosstudstaffrollno + "'";
                    dsfee.Clear();
                    dsfee = d2.select_method_wo_parameter(paid, "text");
                    if (dsfee.Tables.Count > 0)
                    {
                        if (dsfee.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < dsfee.Tables[0].Rows.Count; k++)
                            {
                                amt = Convert.ToDouble(dsfee.Tables[0].Rows[k]["paidamount"]);
                                if (amt == 0.00)
                                {
                                    qry = "delete from ft_Feeallot where app_no='" + hosstudstaffrollno + "' and paidamount='0' and headerfk in('" + HeaderFK + "') and ledgerfk in('" + LedgerFK + "') and MemType='3'";
                                    query = d2.update_method_wo_parameter(qry, "Text");
                                }
                            }
                        }
                    }

                }

            }
            if (query != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                surediv.Visible = false;
                lblstudent.Visible = false;
                lblstudentcount.Visible = false;
                txt_rollno.Text = "";
                txt_regno.Text = "";
                txt_name.Text = "";
                txt_degree.Text = "";
                txt_hostelname1.Text = "";
                txt_amount.Text = "";
                txt_stu.Text = "";
                txt_staffcode.Text = "";
                txt_dept.Text = "";
                txt_design.Text = "";
                txt_guestname.Text = "";
                txt_gustCode.Text = "";
                lblstudent.Visible = false;
                lblstudentcount.Visible = false;
                description();
                popupstudaddinl.Visible = true;
            }

        }
        catch
        {
        }

    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        try
        {

            surediv.Visible = false;
            imgdiv2.Visible = false;
            popupstudaddinl.Visible = true;
        }
        catch
        {
        }

    }
    #endregion

    #region AddNew

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        try
        {
            rblstustaffguest.Items[0].Selected = true;
            rblstustaffguest.Items[1].Enabled = true;
            rblstustaffguest.Items[2].Enabled = true;
            rblstustaffguest.Items[0].Enabled = true;
            rblstustaffguest.Items[1].Selected = false;
            rblstustaffguest.Items[2].Selected = false;
            txt_rollno.Enabled = true;
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            popupstudaddinl.Visible = true;
            txt_regno.Text = "";
            txt_rollno.Text = "";
            txt_name.Text = "";
            txt_degree.Text = "";
            txt_hostelname1.Text = "";
            description();
            txt_description.Text = "";
            txt_amount.Text = "";
            btn_save.Visible = true;
            lbl_pop1staffname.Visible = false;
            txt_pop1staffname.Visible = false;
            txt_degree.Enabled = true;
            lbl_degree.Enabled = true;
            lbl_rollno.Visible = true;
            txt_rollno.Visible = true;
            lbl_regno.Enabled = true;
            txt_regno.Enabled = true;
            lbl_name.Enabled = true;
            txt_name.Enabled = true;
            lbl_rollno.Visible = true;
            txt_rollno.Visible = true;
            btn_rollno.Visible = true;
            lbl_regno.Visible = true;
            txt_regno.Visible = true;
            lbl_name.Visible = true;
            txt_name.Visible = true;
            lbl_degree.Visible = true;
            txt_degree.Visible = true;
            dept.Visible = true;
            design.Visible = true;
            txt_guestname.Visible = false;
            btn_guestname.Visible = false;
            lbl_guCode.Visible = false;
            txt_gustCode.Visible = false;
            lbl_guestname.Visible = false;
            lbl_pop1staffname.Visible = false;
            txt_pop1staffname.Visible = false;
            btnstaffname.Visible = false;
            lbl_staffcode.Visible = false;
            txt_staffcode.Visible = false;
            lbl_dept.Visible = false;
            txt_dept.Visible = false;
            lbl_design.Visible = false;
            txt_design.Visible = false;
            btn_save.Text = "Save";
            btn_delete.Visible = false;
            lbl_stu.Visible = false;
            txt_stu.Visible = false;
            btn_save1.Visible = false;
            btn_save2.Visible = false;
        }
        catch
        {
        }

    }
    #endregion

    #region PopUpforAddNew

    #region rbforstustaffguest
    protected void rblstustaffguest_Selected(object sender, EventArgs e)
    {
        try
        {

            if (rblstustaffguest.SelectedIndex == 0)
            {
                lbl_rollno.Visible = true;
                lbl_pop1staffname.Visible = false;
                txt_rollno.Visible = true;
                btn_rollno.Visible = true;
                txt_pop1staffname.Visible = false;
                btnstaffname.Visible = false;
                lbl_regno.Visible = true;
                lbl_staffcode.Visible = false;
                txt_regno.Visible = true;
                txt_staffcode.Visible = false;
                lbl_name.Visible = true;
                lbl_dept.Visible = false;
                txt_name.Visible = true;
                txt_dept.Visible = false;
                lbl_degree.Visible = true;
                txt_degree.Visible = true;
                lbl_design.Visible = false;
                txt_design.Visible = false;
                txt_rollno.Text = "";
                txt_regno.Text = "";
                txt_hostelname1.Text = "";
                txt_name.Text = "";
                txt_degree.Text = "";
                lbl_stu.Visible = false;
                lblstudent.Visible = false;
                txt_stu.Visible = false;
                lblstudentcount.Visible = false;
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbl_guest.Visible = false;
                txt_guestname.Visible = false;
                btn_guestname.Visible = false;
                lbl_guCode.Visible = false;
                txt_gustCode.Visible = false;
                dept.Visible = true;
                design.Visible = true;
                lbl_staff.Visible = false;
                lbl_guestname.Visible = false;
                txt_guestname.Visible = false;
                btn_save.Visible = true;
                btn_save1.Visible = false;
                btn_save2.Visible = false;

            }
            else if (rblstustaffguest.SelectedIndex == 1)
            {
                lbl_rollno.Visible = false;
                lbl_pop1staffname.Visible = true;
                txt_rollno.Visible = false;
                btn_rollno.Visible = false;
                txt_pop1staffname.Visible = true;
                btnstaffname.Visible = true;
                lbl_regno.Visible = false;
                lbl_staffcode.Visible = true;
                txt_regno.Visible = false;
                txt_staffcode.Visible = true;
                lbl_name.Visible = false;
                lbl_dept.Visible = true;
                txt_name.Visible = false;
                txt_dept.Visible = true;
                lbl_degree.Visible = false;
                txt_degree.Visible = false;
                lbl_design.Visible = true;
                txt_design.Visible = true;
                txt_pop1staffname.Text = "";
                txt_hostelname1.Text = "";
                txt_staffcode.Text = "";
                txt_dept.Text = "";
                txt_design.Text = "";
                lbl_stu.Visible = false;
                lblstudent.Visible = false;
                txt_stu.Visible = false;
                lblstudentcount.Visible = false;
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbl_guest.Visible = false;
                lbl_guestname.Visible = false;
                txt_guestname.Visible = false;
                btn_guestname.Visible = false;
                lbl_guCode.Visible = false;
                txt_gustCode.Visible = false;
                dept.Visible = true;
                design.Visible = true;
                btn_save.Visible = false;
                btn_save1.Visible = true;
                btn_save2.Visible = false;
            }
            else
            {
                lbl_rollno.Visible = false;
                lbl_pop1staffname.Visible = false;
                txt_rollno.Visible = false;
                btn_rollno.Visible = false;
                txt_pop1staffname.Visible = false;
                btnstaffname.Visible = false;
                lbl_regno.Visible = false;
                lbl_staffcode.Visible = false;
                txt_regno.Visible = false;
                txt_staffcode.Visible = false;
                lbl_name.Visible = false;
                lbl_dept.Visible = false;
                txt_name.Visible = false;
                txt_dept.Visible = false;
                lbl_degree.Visible = false;
                txt_degree.Visible = false;
                lbl_design.Visible = false;
                txt_design.Visible = false;
                txt_pop1staffname.Text = "";
                txt_hostelname1.Text = "";
                txt_staffcode.Text = "";
                txt_dept.Text = "";
                txt_design.Text = "";
                lbl_stu.Visible = false;
                lblstudent.Visible = false;
                txt_stu.Visible = false;
                lblstudentcount.Visible = false;
                dept.Visible = false;
                design.Visible = false;
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbl_guest.Visible = false;
                txt_guestname.Visible = true;
                btn_guestname.Visible = true;
                lbl_guCode.Visible = true;
                txt_gustCode.Visible = true;
                lbl_staff.Visible = false;
                lbl_guestname.Visible = true;
                btn_save.Visible = false;
                btn_save1.Visible = false;
                btn_save2.Visible = true;



            }
        }
        catch
        {

        }

    }
    #endregion

    #region StudentDetails
    protected void btn_rollno_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_errormsg1.Visible = false;
            popupselectstd.Visible = true;
            bindhostelname1();
            bindbatch();
            binddegree();
            bindbranch(college);
            Fpspread2.Visible = false;
            btn_ok.Visible = false;
            btn_exit1.Visible = false;
            txt_rollno1.Text = "";
            lbl_count.Visible = false;
        }
        catch
        {

        }
    }
    #endregion

    #region StaffDetails

    #region staffnamesearch
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select staff_name  from staffmaster where resign =0 and settled =0  and staff_code not in (select Roll_No from Hostel_StudentDetails )  and staff_name like  '" + prefixText + "%' ";
        string query = "select staff_name  from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0 and s.appl_no = a.appl_no  and a.appl_id not in (select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 )   and staff_name like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    protected void Staffname_txtchange(object sender, EventArgs e)
    {


    }
    #endregion


    #region Staffcodesearch
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCodepopup(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0  and s.appl_no = a.appl_no  and a.appl_id in(select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 )  and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    #endregion

    protected void btnstaffname_Click(object sender, EventArgs e)
    {
        popupwindowstaff.Visible = true;
        Fpstaff.Visible = false;
        div1.Visible = false;
        btn_staffok.Visible = false;
        btn_staffexit.Visible = false;
        txt_staffcodesearch.Text = "";
        txt_staffnamesearch.Text = "";
        bindhostelname3();
        bindstaffdepartmentpopup();
        lbl_errorsearch.Visible = false;
        lbl_errormsg1.Visible = false;
    }
    #endregion


    #region GuestDetails
    protected void Guestname_txtchange(object sendre, EventArgs e)
    {


    }


    protected void btn_guestname_Click(object sendre, EventArgs e)
    {
        try
        {
            DivGuestpopupwindow.Visible = true;
            FpSpreadguest.Visible = false;
            divGuest.Visible = false;
            btn_guestok.Visible = false;
            btn_guestexit.Visible = false;
            bindhostelhostel();
            lbl_errorsearch.Visible = false;
            lbl_errormsg1.Visible = false;
        }
        catch
        {


        }

    }
    #endregion

    #region Description
    public void description()
    {
        try
        {
            string headerquery = "";
            ddl_description.Items.Clear();
            headerquery = "select distinct MasterValue,MasterCode from HT_StudAdditionalDet h,CO_MasterValues m where h.AdditionalDesc=m.MasterCode and CollegeCode='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_description.DataSource = ds;
                ddl_description.DataTextField = "MasterValue";
                ddl_description.DataValueField = "MasterCode";
                ddl_description.DataBind();
                ddl_description.Items.Insert(0, "Select");
                ddl_description.Items.Insert(ddl_description.Items.Count, "Others");
            }
            else
            {
                ddl_description.Items.Insert(0, "Select");
                ddl_description.Items.Insert(ddl_description.Items.Count, "Others");
            }
        }
        catch
        {
        }
    }
    #endregion


    public string subjectcodenew(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }


    #region Save
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string expnc = "";
            int ins = 0;
            string sql = "";
            string staffcode = string.Empty;
            string Guestcode = string.Empty;
            DataSet dshealthfees = new DataSet();
            string header_id = string.Empty;
            string ledgPK = string.Empty;
            string exincludemessbill = string.Empty;
            string tcode = string.Empty;
            if (ddl_description.SelectedItem.Value != "Select")
            {
                if (ddl_description.SelectedItem.Value != "Others")
                {
                    expnc = Convert.ToString(ddl_description.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_description.Text);
                    expnc = subjectcodenew("Expense", doc_prty1);
                }
            }

            date = Convert.ToString(txt_date.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            if (txt_hostelname1.Text != "" && txt_amount.Text != "")
            {
                hostlnm = Convert.ToString(txt_hostelname1.Text);
                amount = Convert.ToString(txt_amount.Text);
            }


            //header and ledger
            string healthfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Health' and collegecode='" + ddl_collegestaff.SelectedValue + "'";
            dshealthfees.Clear();
            dshealthfees = d2.select_method_wo_parameter(healthfeeset, "Text");
            if (dshealthfees.Tables.Count > 0 && dshealthfees.Tables[0].Rows.Count > 0)
            {
                header_id = Convert.ToString(dshealthfees.Tables[0].Rows[0]["header"]);
                ledgPK = Convert.ToString(dshealthfees.Tables[0].Rows[0]["ledger"]);
                exincludemessbill = Convert.ToString(dshealthfees.Tables[0].Rows[0]["Text_value"]);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Set Fees setting";
                return;
            }
            string finYeaid = d2.getCurrentFinanceYear(usercode, collegecode1);
            if (rblstustaffguest.SelectedIndex == 0)
            {
                regno = Convert.ToString(txt_regno.Text);
                if (txt_rollno.Visible == true)
                {
                    rollno = Convert.ToString(txt_rollno.Text);
                }
                if (txt_stu.Visible == true)
                {
                    rollno = Convert.ToString(lbl_Sturollno.Text);
                }
                name = Convert.ToString(txt_name.Text);
                degree = Convert.ToString(txt_degree.Text);
                string rollnum = string.Empty;
                string[] split = rollno.Split(';');
                if (split.Length > 0)
                {
                    for (int i = 0; i < split.Length; i++)
                    {
                        rollnum = Convert.ToString(split[i]);
                        if (!string.IsNullOrEmpty(rollnum) && !string.IsNullOrEmpty(expnc) && !string.IsNullOrEmpty(amount))
                        {
                            string app_no = d2.GetFunction("select app_no from Registration where Roll_No='" + rollnum + "'");
                            if (app_no != "")
                            {
                                if (exincludemessbill == "1")
                                {
                                    sql = "if exists (select * from HT_HealthCheckup where App_No='" + app_no + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "') update HT_HealthCheckup set MemType='1',App_No='" + app_no + "',TransDate='" + dt.ToString("MM/dd/yyyy") + "',HealthAdditionalAmt='" + amount + "',HealthDesc='" + expnc + "' where App_No='" + app_no + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "' else insert into HT_HealthCheckup(MemType,App_No,TransDate,HealthAdditionalAmt,HealthDesc) values('1','" + app_no + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + expnc + "')";
                                }
                                else
                                {
                                    if (finYeaid.Trim() != "" && finYeaid.Trim() != "0")
                                    {
                                        tcode = "0";
                                        sql = "if exists (select * from HT_HealthCheckup where App_No='" + app_no + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "') update HT_HealthCheckup set MemType='1',App_No='" + app_no + "',TransDate='" + dt.ToString("MM/dd/yyyy") + "',HealthAdditionalAmt='" + amount + "',HealthDesc='" + expnc + "' where App_No='" + app_no + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "' else insert into HT_HealthCheckup(MemType,App_No,TransDate,HealthAdditionalAmt,HealthDesc) values('1','" + app_no + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + expnc + "')";
                                        sql += "if exists (select * from FT_FeeAllot where App_No ='" + app_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinYearFK='" + finYeaid + "' ) update FT_FeeAllot set AllotDate='" + dt.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + amount + "',TotalAmount ='" + amount + "' ,BalAmount ='" + amount + "'-isnull(PaidAmount,'0')   where App_No ='" + app_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinYearFK='" + finYeaid + "' else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt)  values ('" + app_no + "','" + ledgPK + "','" + header_id + "','" + finYeaid + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + tcode + "','',0,0,'" + amount + "','" + amount + "','1','1',0,0)";
                                    }

                                }
                                ins = d2.update_method_wo_parameter(sql, "Text");
                            }
                        }
                    }
                }
            }
            else if (rblstustaffguest.SelectedIndex == 1)
            {
                if (txt_pop1staffname.Visible == true)
                {
                    staffcode = Convert.ToString(txt_staffcode.Text);
                }
                if (txt_stu.Visible == true)
                {
                    staffcode = Convert.ToString(lbl_Sturollno.Text);
                }

                string staffcod = string.Empty;
                string[] split1 = staffcode.Split(';');
                if (split1.Length > 0)
                {
                    for (int j = 0; j < split1.Length; j++)
                    {
                        staffcod = Convert.ToString(split1[j]);
                        if (!string.IsNullOrEmpty(staffcod) && !string.IsNullOrEmpty(expnc) && !string.IsNullOrEmpty(amount))
                        {
                            string staffapp_no = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + staffcod + "' and sam.appl_no = sm.appl_no");

                            if (staffapp_no != "")
                            {
                                if (exincludemessbill == "1")
                                {
                                    sql = "if exists (select * from HT_HealthCheckup where App_No='" + staffapp_no + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "') update HT_HealthCheckup set MemType='2',App_No='" + staffapp_no + "',TransDate='" + dt.ToString("MM/dd/yyyy") + "',HealthAdditionalAmt='" + amount + "',HealthDesc='" + expnc + "' where App_No='" + staffapp_no + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "' else insert into HT_HealthCheckup(MemType,App_No,TransDate,HealthAdditionalAmt,HealthDesc) values('2','" + staffapp_no + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + expnc + "')";
                                }
                                else
                                {
                                    if (finYeaid.Trim() != "" && finYeaid.Trim() != "0")
                                    {
                                        tcode = "0";
                                        sql = "if exists (select * from HT_HealthCheckup where App_No='" + staffapp_no + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "') update HT_HealthCheckup set MemType='2',App_No='" + staffapp_no + "',TransDate='" + dt.ToString("MM/dd/yyyy") + "',HealthAdditionalAmt='" + amount + "',HealthDesc='" + expnc + "' where App_No='" + staffapp_no + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "' else insert into HT_HealthCheckup(MemType,App_No,TransDate,HealthAdditionalAmt,HealthDesc) values('2','" + staffapp_no + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + expnc + "')";
                                        sql += "if exists (select * from FT_FeeAllot where App_No ='" + staffapp_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinYearFK='" + finYeaid + "' ) update FT_FeeAllot set AllotDate='" + dt.ToString("MM/dd/yyyy") + "',MemType='2',FeeAmount='" + amount + "',TotalAmount ='" + amount + "' ,BalAmount ='" + amount + "'-isnull(PaidAmount,'0')   where App_No ='" + staffapp_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinYearFK='" + finYeaid + "' else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt)  values ('" + staffapp_no + "','" + ledgPK + "','" + header_id + "','" + finYeaid + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + tcode + "','',0,0,'" + amount + "','" + amount + "','2','1',0,0)";
                                    }

                                }
                                ins = d2.update_method_wo_parameter(sql, "Text");
                            }
                        }
                    }
                }

            }

            else
            {
                if (txt_guestname.Visible == true)
                {
                    Guestcode = Convert.ToString(txt_gustCode.Text);
                }
                if (txt_stu.Visible == true)
                {
                    Guestcode = Convert.ToString(lbl_Sturollno.Text);
                }

                string guestcod = string.Empty;
                string[] split2 = Guestcode.Split(';');
                if (split2.Length > 0)
                {
                    for (int j = 0; j < split2.Length; j++)
                    {
                        guestcod = Convert.ToString(split2[j]);
                        if (!string.IsNullOrEmpty(guestcod) && !string.IsNullOrEmpty(expnc) && !string.IsNullOrEmpty(amount))
                        {
                            if (exincludemessbill == "1")
                            {
                                sql = "if exists (select * from HT_HealthCheckup where App_No='" + guestcod + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "') update HT_HealthCheckup set MemType='3',App_No='" + guestcod + "',TransDate='" + dt.ToString("MM/dd/yyyy") + "',HealthAdditionalAmt='" + amount + "',HealthDesc='" + expnc + "' where App_No='" + guestcod + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "' else insert into HT_HealthCheckup(MemType,App_No,TransDate,HealthAdditionalAmt,HealthDesc) values('3','" + guestcod + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + expnc + "')";
                            }
                            else
                            {
                                if (finYeaid.Trim() != "" && finYeaid.Trim() != "0")
                                {
                                    tcode = "0";
                                    sql += "if exists (select * from HT_HealthCheckup where App_No='" + guestcod + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "') update HT_HealthCheckup set MemType='3',App_No='" + guestcod + "',TransDate='" + dt.ToString("MM/dd/yyyy") + "',HealthAdditionalAmt='" + amount + "',HealthDesc='" + expnc + "' where App_No='" + guestcod + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and HealthDesc='" + expnc + "' else insert into HT_HealthCheckup(MemType,App_No,TransDate,HealthAdditionalAmt,HealthDesc) values('3','" + guestcod + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + expnc + "')";
                                    sql = "if exists (select * from FT_FeeAllot where App_No ='" + guestcod + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinYearFK='" + finYeaid + "' ) update FT_FeeAllot set AllotDate='" + dt.ToString("MM/dd/yyyy") + "',MemType='3',FeeAmount='" + amount + "',TotalAmount ='" + amount + "' ,BalAmount ='" + amount + "'-isnull(PaidAmount,'0')   where App_No ='" + guestcod + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinYearFK='" + finYeaid + "' else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt)  values ('" + guestcod + "','" + ledgPK + "','" + header_id + "','" + finYeaid + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + tcode + "','',0,0,'" + amount + "','" + amount + "','3','1',0,0)";
                                }

                            }
                            ins = d2.update_method_wo_parameter(sql, "Text");

                        }
                    }
                }

            }
            if (ins > 0)
            {
                if (btn_save.Text == "Save")
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Updated Successfully";

                }
                lblstudent.Visible = false;
                lblstudentcount.Visible = false;
                txt_rollno.Text = "";
                txt_regno.Text = "";
                txt_name.Text = "";
                txt_degree.Text = "";
                txt_hostelname1.Text = "";
                txt_amount.Text = "";
                txt_stu.Text = "";
                txt_staffcode.Text = "";
                txt_dept.Text = "";
                txt_design.Text = "";
                txt_guestname.Text = "";
                txt_gustCode.Text = "";
                lblstudent.Visible = false;
                lblstudentcount.Visible = false;
                description();
            }


        }
        catch
        {



        }

    }


    protected void btn_save1_Click(object sender, EventArgs e)
    {
        try
        {
            btn_save_Click(sender, e);

        }
        catch
        {
        }

    }

    protected void btn_save2_Click(object sender, EventArgs e)
    {
        try
        {
            btn_save_Click(sender, e);

        }
        catch
        {
        }


    }
    #endregion
    #endregion

    #region QusforStudentPopupwindow

    #region SearchRollNo
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        if (hostel_name_code.Trim() != "")
        {
            query = "select distinct top 10 r.Roll_No from Registration as r join HT_HostelRegistration as hs on r.app_no=hs.APP_No join HM_HostelMaster  as hd on hs.HostelMasterFK=hd.HostelMasterPK where r.Delflag=0 and r.cc=0 and HostelMasterPK in ('" + hostel_name_code + "') and r.roll_no like '" + prefixText + "%' order by r.Roll_No desc";

        }
        else
        {
            query = "select distinct top 10 r.Roll_No from Registration as r join HT_HostelRegistration as hs on r.app_no=hs.APP_No join HM_HostelMaster  as hd on hs.HostelMasterFK=hd.HostelMasterPK where r.Delflag=0 and r.cc=0 and r.roll_no like '" + prefixText + "%' order by r.Roll_No desc ";
        }
        name = ws.Getname(query);
        return name;
    }
    #endregion

    protected void txt_rollno_txtchange(object sender, EventArgs e)
    {
        try
        {
            string rollno = Convert.ToString(txt_rollno.Text);
            string selectquery = " select r.Roll_No,r.Reg_No,r.Roll_Admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.HostelMasterPK ,hd.HostelName  from Registration r,HM_HostelMaster hd,HT_HostelRegistration hs,Degree d,Department dt,Course c where r.App_No =hs.App_No and hs.HostelMasterFK =hd.HostelMasterPK and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No = '" + txt_rollno.Text + "'";

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                rollno = Convert.ToString(txt_rollno.Text);
                string regno = Convert.ToString(ds.Tables[0].Rows[0]["Reg_No"]);
                string stuname = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);
                string deg = Convert.ToString(ds.Tables[0].Rows[0]["Degree"]);
                string hostelname = Convert.ToString(ds.Tables[0].Rows[0]["HostelName"]);
                txt_regno.Text = regno;
                txt_name.Text = stuname;
                txt_degree.Text = deg;
                txt_hostelname1.Text = hostelname;
            }
        }
        catch
        {
        }
    }

    #region HostelName

    public void bindhostelname1()
    {
        try
        {
            ds.Clear();
            cbl_hostelname2.Items.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname2.DataSource = ds;
                cbl_hostelname2.DataTextField = "HostelName";
                cbl_hostelname2.DataValueField = "HostelMasterPK";
                cbl_hostelname2.DataBind();
                if (cbl_hostelname2.Items.Count > 0)
                {
                    for (i = 0; i < cbl_hostelname2.Items.Count; i++)
                    {
                        cbl_hostelname2.Items[i].Selected = true;
                        if (hostel_name_code == "")
                        {
                            hostel_name_code = Convert.ToString(cbl_hostelname2.Items[i].Value);
                        }
                        else
                        {
                            hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname2.Items[i].Value);
                        }
                    }
                    txt_hostelname2.Text = "Hostel(" + cbl_hostelname2.Items.Count + ")";
                }
            }
            else
            {
                txt_hostelname2.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_hostelname2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_hostelname2.Text = "--Select--";
            cb_hostelname2.Checked = false;
            commcount = 0;
            for (i = 0; i < cbl_hostelname2.Items.Count; i++)
            {
                if (cbl_hostelname2.Items[i].Selected == true)
                {
                    if (hostel_name_code == "")
                    {
                        hostel_name_code = Convert.ToString(cbl_hostelname2.Items[i].Value);
                    }
                    else
                    {
                        hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname2.Items[i].Value);
                    }
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_hostelname2.Text = "Hostel(" + commcount.ToString() + ")";
                if (commcount == cbl_hostelname2.Items.Count)
                {
                    cb_hostelname2.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    protected void cb_hostelname2_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname2.Checked == true)
            {
                for (i = 0; i < cbl_hostelname2.Items.Count; i++)
                {
                    cbl_hostelname2.Items[i].Selected = true;
                    if (hostel_name_code == "")
                    {
                        hostel_name_code = Convert.ToString(cbl_hostelname2.Items[i].Value);
                    }
                    else
                    {
                        hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname2.Items[i].Value);
                    }
                }
                txt_hostelname2.Text = "Hostel(" + (cbl_hostelname2.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_hostelname2.Items.Count; i++)
                {
                    cbl_hostelname2.Items[i].Selected = false;
                }
                txt_hostelname2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region Batch
    public void bindbatch()
    {
        try
        {
            ddl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region Degree
    public void binddegree()
    {
        try
        {
            ds.Clear();
            cbl_degree.Items.Clear();
            //string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            //ds = d2.select_method_wo_parameter(query, "Text");
            ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree1.Text = "Degree(" + cbl_degree.Items.Count + ")";
                }
                else
                {
                    txt_degree1.Text = "--Select--";
                }
            }
            else
            {
                txt_degree1.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree.Checked = false;
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch.Text = "--Select--";
                    build = cbl_degree.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            bindbranch(buildvalue);
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree1.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree1.Text = "--Select--";
                txt_degree1.Text = "--Select--";
            }
            else
            {
                txt_degree1.Text = "Degree(" + seatcount.ToString() + ")";
            }
            // bindbranch(college);
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_degree_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";
            if (cb_degree.Checked == true)
            {
                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree1.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
                        build1 = cbl_degree.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                bindbranch(buildvalue1);
            }
            else
            {
                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree1.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    cbl_branch.ClearSelection();
                    cb_branch.Checked = false;
                }
            }
            bindbranch(college);
            // Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region Branch
    public void bindbranch(string branch)
    {
        try
        {
            cbl_branch.Items.Clear();
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_degree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_degree.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            if (itemheader.Trim() != "")
            {
                ds = d2.select_method(commname, hat, "Text");
                //ds = d2.BindBranch(singleuser, group_user, cbl_degree.SelectedValue, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            cbl_branch.Items[i].Selected = true;
                        }
                        txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
                    }
                }
                else
                {
                    txt_branch.Text = "--Select--";
                }
            }
            else
            {
                txt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_branch.Items.Count)
                {
                    cb_branch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_branch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch.Checked == true)
            {
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region Go
    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_hostelname2.Items.Count; i++)
            {
                if (cbl_hostelname2.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        hostel = "" + cbl_hostelname2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostel = hostel + "'" + "," + "'" + cbl_hostelname2.Items[i].Value.ToString() + "";
                    }
                }
            }
            lbl_errormsg1.Visible = false;
            Fpspread2.SaveChanges();
            Fpspread2.DataBind();
            Fpspread2.CommandBar.Visible = false;
            // Fpspread2.Sheets[0].FrozenColumnCount = 2;
            Fpspread2.SheetCorner.ColumnCount = 0;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 6;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            ds.Clear();
            if (txt_rollno1.Text != "")
            {
                sql = " select r.APP_No,r.Reg_No, r.Roll_No,r.Roll_admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.HostelMasterPK  ,hd.HostelName  from Registration r,HM_HostelMaster hd,HT_HostelRegistration hs,Degree d,Department dt,Course c where r.App_No =hs.APP_No and hs.HostelMasterfK =hd.HostelMasterPK and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and isnull(IsDiscontinued,'0')=0 and isnull(IsSuspend,'0')=0  and isnull(IsVacated ,'0')=0   and r.Roll_No like  '" + txt_rollno1.Text + "' order by r.Roll_No";
            }
            else
            {
                sql = "select r.APP_No,r.Reg_No, r.Roll_No,r.Roll_admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.HostelMasterPK  ,hd.HostelName  from Registration r,HM_HostelMaster hd,HT_HostelRegistration hs,Degree d,Department dt,Course c where r.App_No =hs.APP_No and hs.HostelMasterfK =hd.HostelMasterPK and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and isnull(IsDiscontinued,'0')=0 and isnull(IsSuspend,'0')=0  and isnull(IsVacated ,'0')=0   and d.Degree_Code in('" + itemheader + "') and hs.HostelMasterFK in('" + hostel + "') order by Roll_No";
            }
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpspread2.Sheets[0].AutoPostBack = false;
                //Fpspread2.Sheets[0].RowHeader.Visible = false;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[0].Locked = true;
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[1].Locked = true;
                Fpspread2.Columns[1].Width = 130;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[2].Locked = true;
                Fpspread2.Columns[2].Width = 130;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[3].Locked = true;
                Fpspread2.Columns[3].Width = 170;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].Columns[4].Locked = true;
                //Fpspread2.Columns[4].Width = 400;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel Name";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[4].Locked = true;
                Fpspread2.Columns[4].Width = 150;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[4].Width = 150;
                FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                Fpspread2.Width = 636;
                int studcount = 0;

                for (int row1 = 0; row1 < cbl_branch.Items.Count; row1++)
                {
                    if (cbl_branch.Items[row1].Selected)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "Degree_Code='" + Convert.ToSingle(cbl_branch.Items[row1].Value) + "'";
                        DataView dv = ds.Tables[0].DefaultView;
                        if (dv.Count > 0)
                        {
                            Fpspread2.Sheets[0].RowCount = Fpspread2.Sheets[0].RowCount + 1;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["Degree_Code"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[0]["Degree"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].AddSpanCell(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 6);
                            sno++;
                            for (int row = 0; row < dv.Count; row++)
                            {
                                studcount++;
                                // sno++;
                                Fpspread2.Sheets[0].RowCount++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[row]["Roll_No"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv[row]["Degree"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[row]["Reg_No"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[row]["Stud_Name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["department"]);
                                ////Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[row]["HostelName"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dv[row]["HostelMasterPK"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].CellType = chk;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            }
                        }
                    }
                }
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                lbl_count.Visible = true;
                lbl_count.Text = "No of Student :" + studcount.ToString();
                Fpspread2.SaveChanges();
                Fpspread2.Width = 750;
                Fpspread2.Visible = true;
                btn_ok.Visible = true;
                btn_exit1.Visible = true;
            }
            else
            {

                Fpspread2.Visible = false;
                lbl_errormsg1.Visible = true;
                lbl_count.Visible = false;
                lbl_errormsg1.Text = "No Records Found";
                btn_ok.Visible = false;
                btn_exit1.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region OK

    protected void btn_ok_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            string activecol = "";
            string name = "";
            string degree = "";
            string degreecode = "";
            string selectStud = "";
            Fpspread2.SaveChanges();
            activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
            //Added By Saranyadevi 13.2.2018
            string rollno = string.Empty;
            string StudentName = string.Empty;
            string hostelname = string.Empty;
            string selectrollno = string.Empty;
            string selectStudentName = string.Empty;
            string selecthostelname = string.Empty;
            ArrayList hostelnameArr = new ArrayList();
            int studCount = 0;
            Fpspread2.SaveChanges();
            for (int row = 0; row < Fpspread2.Sheets[0].RowCount; row++)
            {
                selected = 0;
                int.TryParse(Convert.ToString(Fpspread2.Sheets[0].Cells[row, 5].Value), out selected);
                if (selected == 1)
                {
                    rollno = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 1].Text).Trim();
                    StudentName = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 3].Text).Trim();
                    hostelname = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 4].Text).Trim();
                    if (String.IsNullOrEmpty(selectrollno))
                        selectrollno = rollno;
                    else
                        selectrollno += ";" + rollno;
                    if (String.IsNullOrEmpty(selectStudentName))
                        selectStudentName = StudentName;
                    else
                        selectStudentName += ";" + StudentName;

                    if (String.IsNullOrEmpty(selecthostelname))
                    {
                        selecthostelname = hostelname;
                        hostelnameArr.Add(hostelname);
                    }
                    else
                        if (!hostelnameArr.Contains(hostelname))
                        {
                            selecthostelname += ";" + hostelname;
                            hostelnameArr.Add(hostelname);
                        }
                    studCount++;
                }
            }
            if (studCount == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Any Student";
                return;
            }
            if (studCount > 1)
            {
                txt_hostelname1.Text = selecthostelname;
                lbl_Sturollno.Text = selectrollno;
                ViewState["NoOfStudents"] = studCount;
                lblstudent.Visible = true;
                lblstudentcount.Visible = true;
                string stucnt = Convert.ToString(ViewState["NoOfStudents"]);
                lblstudentcount.Text = stucnt;
                //clearpopup();
                txt_degree.Enabled = false;
                lbl_degree.Enabled = false;
                lbl_rollno.Visible = false;
                txt_rollno.Visible = false;
                lbl_regno.Enabled = false;
                txt_regno.Enabled = false;
                lbl_name.Enabled = false;
                txt_name.Enabled = false;
                lbl_stu.Visible = true;
                lbl_Sturollno.Visible = false;
                txt_stu.Visible = true;
                txt_stu.Text = stucnt;
                lbl_staff.Visible = false;
                lbl_guest.Visible = false;
                lbl_pop1staffname.Visible = false;
                txt_pop1staffname.Visible = false;
                lbl_guestname.Visible = false;
                popupselectstd.Visible = false;
            }
            else
            {
                lblstudent.Visible = false;
                lblstudentcount.Visible = false;
                rollno = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                txt_rollno.Text = rollno;
                regno = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                txt_regno.Text = regno;
                name = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                txt_name.Text = name;
                degree = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                degreecode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                Session["degreecode1"] = Convert.ToString(degreecode);
                txt_degree.Text = degree;
                hostel = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                hostelcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                Session["hostelcode1"] = Convert.ToString(hostelcode);
                txt_hostelname1.Text = hostel;
                txt_degree.Enabled = true;
                lbl_degree.Enabled = true;
                lbl_rollno.Visible = true;
                txt_rollno.Visible = true;
                lbl_regno.Enabled = true;
                txt_regno.Enabled = true;
                lbl_name.Enabled = true;
                txt_name.Enabled = true;
                lbl_stu.Visible = false;
                lbl_Sturollno.Visible = false;
                txt_stu.Visible = false;
                lbl_staff.Visible = false;
                lbl_guest.Visible = false;
                lbl_pop1staffname.Visible = false;
                txt_pop1staffname.Visible = false;
                lbl_guestname.Visible = false;
                txt_guestname.Visible = false;
                popupselectstd.Visible = false;
                popupstudaddinl.Visible = true;
            }

        }
        catch (Exception ex)
        {
        }
    }
    #endregion
    #endregion

    #region QusforStaffPopupWindow




    #region Hostelname
    public void bindhostelname3()
    {
        try
        {
            ds.Clear();
            cbl_hostelname3.Items.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname3.DataSource = ds;
                cbl_hostelname3.DataTextField = "HostelName";
                cbl_hostelname3.DataValueField = "HostelMasterPK";
                cbl_hostelname3.DataBind();
                if (cbl_hostelname3.Items.Count > 0)
                {
                    for (i = 0; i < cbl_hostelname3.Items.Count; i++)
                    {
                        cbl_hostelname3.Items[i].Selected = true;
                        if (hostel_name_code == "")
                        {
                            hostel_name_code = Convert.ToString(cbl_hostelname3.Items[i].Value);
                        }
                        else
                        {
                            hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname3.Items[i].Value);
                        }
                    }
                    txt_hostelname3.Text = "Hostel(" + cbl_hostelname3.Items.Count + ")";
                }
            }
            else
            {
                txt_hostelname3.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_hostelname3_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_hostelname3.Text = "--Select--";
            cb_hostelname3.Checked = false;
            commcount = 0;
            for (i = 0; i < cbl_hostelname3.Items.Count; i++)
            {
                if (cbl_hostelname3.Items[i].Selected == true)
                {
                    if (hostel_name_code == "")
                    {
                        hostel_name_code = Convert.ToString(cbl_hostelname3.Items[i].Value);
                    }
                    else
                    {
                        hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname3.Items[i].Value);
                    }
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_hostelname3.Text = "Hostel(" + commcount.ToString() + ")";
                if (commcount == cbl_hostelname3.Items.Count)
                {
                    cb_hostelname3.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    protected void cb_hostelname3_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname3.Checked == true)
            {
                for (i = 0; i < cbl_hostelname3.Items.Count; i++)
                {
                    cbl_hostelname3.Items[i].Selected = true;
                    if (hostel_name_code == "")
                    {
                        hostel_name_code = Convert.ToString(cbl_hostelname3.Items[i].Value);
                    }
                    else
                    {
                        hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname3.Items[i].Value);
                    }
                }
                txt_hostelname3.Text = "Hostel(" + (cbl_hostelname3.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_hostelname3.Items.Count; i++)
                {
                    cbl_hostelname3.Items[i].Selected = false;
                }
                txt_hostelname3.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion


    public void loadcollegestaffpopup()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegestaff.DataSource = ds;
                ddl_collegestaff.DataTextField = "collname";
                ddl_collegestaff.DataValueField = "college_code";
                ddl_collegestaff.DataBind();
            }
            //binddept(ddl_collegename.SelectedItem.Value.ToString());
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
        {
        }
    }

    public void bindstaffdepartmentpopup()
    {
        try
        {
            ds.Clear();
            //string query = "";
            //query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + collegecode1 + "'";
            string clgcode = "";
            if (ddl_collegestaff.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegestaff.SelectedItem.Value);
            }
            ds = d2.loaddepartment(clgcode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_staffdepartment.DataSource = ds;
                ddl_staffdepartment.DataTextField = "dept_name";
                ddl_staffdepartment.DataValueField = "dept_code";
                ddl_staffdepartment.DataBind();

                ddl_staffdepartment.Items.Insert(0, "All");
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
    }


    protected void ddl_collegestaff_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            bindstaffdepartmentpopup();
            div1.Visible = false;
            Fpstaff.Visible = false;
            btn_staffok.Visible = false;
            btn_staffexit.Visible = false;
            lbl_errorsearch.Visible = false;
            lbl_errorsearch.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }

    protected void ddl_staffdepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            div1.Visible = false;
            Fpstaff.Visible = false;
            btn_staffok.Visible = false;
            btn_staffexit.Visible = false;
            lbl_errorsearch.Visible = false;
            lbl_errorsearch.Text = " ";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }

    protected void ddl_searchbystaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_searchbystaff.SelectedItem.Text == "Staff Name")
            {
                txt_staffnamesearch.Visible = true;
                txt_staffcodesearch.Visible = false;
                txt_staffnamesearch.Text = "";

            }
            else if (ddl_searchbystaff.SelectedItem.Text == "Staff Code")
            {
                txt_staffcodesearch.Visible = true;
                txt_staffnamesearch.Visible = false;
                txt_staffnamesearch.Text = "";
            }
            div1.Visible = false;
            Fpstaff.Visible = false;
            btn_staffok.Visible = false;
            btn_staffexit.Visible = false;
            lbl_errorsearch.Visible = false;
            lbl_errorsearch.Text = " ";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }

    protected void btn_staffselectgo_Click(object sender, EventArgs e)
    {
        try
        {
            int rolcount = 0;
            string sql = "";
            int rowcount;
            string hostaffcollcode = string.Empty;
            string hostaffdeptcode = string.Empty;
            string qrystaffdept = string.Empty;
            string qrystaffnamesearch = string.Empty;
            string qrystaffcodesearch = string.Empty;
            string hostelstaff = string.Empty;
            //Fpstaff.Visible = true;
            if (ddl_collegestaff.Items.Count > 0)
                hostaffcollcode = Convert.ToString(ddl_collegestaff.SelectedItem.Value);
            for (i = 0; i < cbl_hostelname3.Items.Count; i++)
            {
                if (cbl_hostelname3.Items[i].Selected == true)
                {
                    if (hostelstaff == "")
                    {
                        hostelstaff = "" + cbl_hostelname3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostelstaff = hostelstaff + "'" + "," + "'" + cbl_hostelname3.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (ddl_staffdepartment.SelectedItem.Text != "All")
            {
                if (ddl_staffdepartment.Items.Count > 0)
                    hostaffdeptcode = Convert.ToString(ddl_staffdepartment.SelectedValue);
                qrystaffdept = "and h.dept_code='" + hostaffdeptcode + "' ";

            }
            if (txt_staffnamesearch.Text != "")
            {
                if (ddl_searchbystaff.SelectedIndex == 0)
                {
                    qrystaffnamesearch = "and s.Staff_name ='" + Convert.ToString(txt_staffnamesearch.Text) + "'";
                }
            }
            if (txt_staffcodesearch.Text.Trim() != "")
            {
                if (ddl_searchbystaff.SelectedIndex == 1)
                {
                    qrystaffcodesearch = "and s.staff_code ='" + Convert.ToString(txt_staffcodesearch.Text) + "'";
                }
            }


            if (!string.IsNullOrEmpty(hostaffcollcode))
            {
                sql = "select distinct a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name ,hm.HostelName   from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a,HT_HostelRegistration hr,HM_HostelMaster hm where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and d.collegeCode=a.college_code and hr.MemType='2'  and hr.APP_No=a.appl_id and hm.HostelMasterPK=hr.HostelMasterFK  and  hr.HostelMasterFK in('" + hostelstaff + "') and s.college_Code='" + hostaffcollcode + "' " + qrystaffnamesearch + qrystaffcodesearch + qrystaffdept + " ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "Text");
            }


            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.SaveChanges();
            Fpstaff.SheetCorner.ColumnCount = 0;
            Fpstaff.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
            Fpstaff.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            Fpstaff.Sheets[0].AutoPostBack = false;

            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.Sheets[0].ColumnCount = 7;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[0].Locked = true;
                Fpstaff.Columns[0].Width = 80;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[1].Locked = true;
                Fpstaff.Columns[1].Width = 100;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[2].Locked = true;
                Fpstaff.Columns[2].Width = 200;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[3].Locked = true;
                Fpstaff.Columns[3].Width = 250;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpstaff.Columns[4].Width = 200;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[4].Locked = true;
                Fpstaff.Width = 700;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Hostel Name";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[4].Locked = true;
                Fpstaff.Columns[4].Width = 150;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Columns[4].Width = 150;
                FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();


                for (rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;

                    Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["appl_id"]);

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";


                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["HostelName"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["HostelName"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 6].CellType = chk;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                }

                lbl_errorsearch1.Visible = true;
                lbl_errorsearch1.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 345;
                Fpstaff.Width = 846;
                btn_staffok.Visible = true;
                btn_staffexit.Visible = true;
                Fpstaff.Visible = true;
                div1.Visible = true;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();

            }
            else
            {
                Fpstaff.Visible = false;
                btn_staffok.Visible = false;
                btn_staffexit.Visible = false;
                div1.Visible = false;
                lbl_errorsearch1.Visible = false;
                lbl_errorsearch.Visible = true;
                lbl_errorsearch.Text = "No Records Found";

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }

    protected void btn_staffok_Click(object sender, EventArgs e)
    {
        try
        {
            string StaffName = "";
            string staffcode = "";
            string StaffDepartment = "";
            string StaffDesignation = "";
            string staffhostelname = "";
            Fpstaff.SaveChanges();
            activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
            string staffcod = string.Empty;
            string StafName = string.Empty;
            string stafhostelname = string.Empty;
            string stafdepart = string.Empty;
            string stafdesign = string.Empty;
            string selectstaffcode = string.Empty;
            string selectStaffName = string.Empty;
            string selectStaffdept = string.Empty;
            string selectStaffdesign = string.Empty;
            string selectstaffhostelname = string.Empty;
            ArrayList staffhostelnameArr = new ArrayList();
            int staffCount = 0;
            Fpstaff.SaveChanges();

            for (int strow = 0; strow < Fpstaff.Sheets[0].RowCount; strow++)
            {
                selected = 0;
                int.TryParse(Convert.ToString(Fpstaff.Sheets[0].Cells[strow, 6].Value), out selected);
                if (selected == 1)
                {
                    staffcod = Convert.ToString(Fpstaff.Sheets[0].Cells[strow, 1].Text).Trim();
                    StafName = Convert.ToString(Fpstaff.Sheets[0].Cells[strow, 2].Text).Trim();
                    selectStaffdept = Convert.ToString(Fpstaff.Sheets[0].Cells[strow, 3].Text).Trim();
                    selectStaffdesign = Convert.ToString(Fpstaff.Sheets[0].Cells[strow, 4].Text).Trim();
                    stafhostelname = Convert.ToString(Fpstaff.Sheets[0].Cells[strow, 5].Text).Trim();
                    if (String.IsNullOrEmpty(selectstaffcode))
                        selectstaffcode = staffcod;
                    else
                        selectstaffcode += ";" + staffcod;
                    if (String.IsNullOrEmpty(selectStaffName))
                        selectStaffName = StafName;
                    else
                        selectStaffName += ";" + StafName;
                    if (String.IsNullOrEmpty(selectStaffdept))
                        selectStaffdept = stafdepart;
                    else
                        selectStaffdept += ";" + stafdepart;
                    if (String.IsNullOrEmpty(selectStaffdesign))
                        selectStaffdesign = stafdesign;
                    else
                        selectStaffdesign += ";" + stafdesign;

                    if (String.IsNullOrEmpty(selectstaffhostelname))
                    {
                        selectstaffhostelname = stafhostelname;
                        staffhostelnameArr.Add(stafhostelname);
                    }
                    else
                        if (!staffhostelnameArr.Contains(stafhostelname))
                        {
                            selectstaffhostelname += ";" + stafhostelname;
                            staffhostelnameArr.Add(stafhostelname);
                        }
                    staffCount++;
                }
            }
            if (staffCount == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Any Staff";
                return;
            }
            if (staffCount > 1)
            {
                txt_hostelname1.Text = selectstaffhostelname;
                lbl_Sturollno.Text = selectstaffcode;
                ViewState["NoOfStaff"] = staffCount;
                lblstudent.Visible = true;
                lblstudent.Text = "No Of Staff:";
                lblstudentcount.Visible = true;
                string staffcnt = Convert.ToString(ViewState["NoOfStaff"]);
                lblstudentcount.Text = staffcnt;
                lbl_pop1staffname.Visible = false;
                txt_pop1staffname.Visible = false;
                txt_pop1staffname.Enabled = false;
                txt_staffcode.Enabled = false;
                txt_dept.Enabled = false;
                txt_design.Enabled = false;
                lbl_stu.Visible = false;
                lbl_staff.Visible = true;
                lbl_guest.Visible = false;
                lbl_Sturollno.Visible = false;
                lbl_pop1staffname.Visible = false;
                lbl_guest.Visible = false;
                txt_guestname.Visible = false;
                txt_stu.Visible = true;
                txt_stu.Text = staffcnt;
                popupwindowstaff.Visible = false;

            }
            else
            {

                lblstudent.Visible = false;
                lblstudentcount.Visible = false;
                StaffName = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                txt_pop1staffname.Text = StaffName;
                staffcode = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                txt_staffcode.Text = staffcode;
                StaffDepartment = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                txt_dept.Text = StaffDepartment;
                StaffDesignation = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                //degreecode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                //Session["degreecode1"] = Convert.ToString(degreecode);
                txt_design.Text = StaffDesignation;
                staffhostelname = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text;
                //hostelcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
                //Session["hostelcode1"] = Convert.ToString(hostelcode);
                txt_hostelname1.Text = staffhostelname;
                lbl_pop1staffname.Visible = true;
                txt_pop1staffname.Visible = true;
                txt_pop1staffname.Enabled = true;
                txt_staffcode.Enabled = true;
                txt_dept.Enabled = true;
                txt_design.Enabled = true;
                lbl_stu.Visible = false;
                lbl_staff.Visible = false;
                lbl_guest.Visible = false;
                lbl_Sturollno.Visible = false;
                lbl_guestname.Visible = false;
                txt_guestname.Visible = false;
                txt_stu.Visible = false;
                popupwindowstaff.Visible = false;

            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    protected void btn_staffexit_Click(object sender, EventArgs e)
    {
        popupwindowstaff.Visible = false;

    }

    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {

        popupwindowstaff.Visible = false;
    }
    #endregion

    #region QusforGuestPopupWindow


    #region Hostelname
    protected void cb_hostelname1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname1.Checked == true)
            {
                for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                {
                    cbl_hostelname1.Items[i].Selected = true;
                }
                txt_guesthostelname.Text = "Hostel Name(" + (cbl_hostelname1.Items.Count) + ")";

                cbl_hostelname1_SelectedIndexChanged(sender, e);
            }
            else
            {
                for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                {
                    cbl_hostelname1.Items[i].Selected = false;
                }
                txt_guesthostelname.Text = "--Select--";
                cbl_buildingname.Items.Clear();
                txt_buildingname.Text = "--Select--";
                cb_buildingname.Checked = false;
                cbl_floorname.Items.Clear();
                txt_floorname.Text = "--Select--";
                cb_floorname.Checked = false;
                cbl_roomname.Items.Clear();
                txt_roomname.Text = "--Select--";
                cb_roomname.Checked = false;


            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_hostelname1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_hostelname1.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
            {
                if (cbl_hostelname1.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_guesthostelname.Text = "--Select--";
                    cb_hostelname1.Checked = false;
                    build = cbl_hostelname1.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            clgbuild(buildvalue);
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_hostelname1.Items.Count)
            {
                txt_guesthostelname.Text = "Hostel Name(" + seatcount + ")";
                cb_hostelname1.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_guesthostelname.Text = "--Select--";
            }
            else
            {
                txt_guesthostelname.Text = "Hostel Name(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }

    }
    public void bindhostelhostel()
    {
        try
        {
            //ds = d2.BindHostel_inv(collegecode1);
            //string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelName ";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(itemname, "Text");

            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_hostelname1.DataSource = ds;
                cbl_hostelname1.DataTextField = "HostelName";
                cbl_hostelname1.DataValueField = "HostelMasterPK";
                cbl_hostelname1.DataBind();

                for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                {
                    cbl_hostelname1.Items[i].Selected = true;
                    txt_guesthostelname.Text = "Hostel(" + (cbl_hostelname1.Items.Count) + ")";
                    cb_hostelname1.Checked = true;
                }

                string lochosname = "";
                for (int i = 0; i < cbl_hostelname1.Items.Count; i++)
                {
                    if (cbl_hostelname1.Items[i].Selected == true)
                    {
                        string hosname = cbl_hostelname1.Items[i].Value.ToString();
                        if (lochosname == "")
                        {
                            lochosname = hosname;
                        }
                        else
                        {
                            lochosname = lochosname + "'" + "," + "'" + hosname;
                        }
                    }
                }

                clgbuild(lochosname);

            }
            else
            {
                cbl_hostelname1.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region Buildname
    public void clgbuild(string hostelname)
    {
        try
        {
            cbl_buildingname.Items.Clear();
            string bul = "";
            bul = d2.GetBuildingCode_inv(hostelname);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_buildingname.DataSource = ds;
                cbl_buildingname.DataTextField = "Building_Name";
                cbl_buildingname.DataValueField = "code";
                cbl_buildingname.DataBind();
            }

            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                cbl_buildingname.Items[i].Selected = true;
                txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
                cb_buildingname.Checked = true;
            }

            string locbuild = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    string builname = cbl_buildingname.Items[i].Text;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            clgfloor(locbuild);
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbbuildname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_buildingname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string lochosname = "";
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        string hosname = cbl_hostelname.Items[i].Value.ToString();
                        if (lochosname == "")
                        {
                            lochosname = hosname;
                        }
                        else
                        {
                            lochosname = lochosname + "'" + "," + "'" + hosname;
                        }
                    }
                }
                cbl_buildingname.Items.Clear();
                clgbuild(lochosname);

                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    if (cb_buildingname.Checked == true)
                    {
                        cbl_buildingname.Items[i].Selected = true;
                        txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_buildingname.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;

                        }

                    }
                }
                clgfloor(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    cbl_buildingname.Items[i].Selected = false;
                    txt_buildingname.Text = "--Select--";
                    cbl_floorname.Items.Clear();
                    cb_floorname.Checked = false;
                    txt_floorname.Text = "--Select--";
                    txt_roomname.Text = "--Select--";
                    cb_roomname.Checked = false;
                    cbl_roomname.Items.Clear();
                }
            }
            //  Button2.Focus();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblbuildname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_buildingname.Checked = false;

            string buildvalue = "";
            string build = "";
            string lochosname = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    string hosname = cbl_hostelname.Items[i].Value.ToString();
                    if (lochosname == "")
                    {
                        lochosname = hosname;
                    }
                    else
                    {
                        lochosname = lochosname + "'" + "," + "'" + hosname;
                    }
                }
            }
            //cbl_buildingname.Items.Clear();
            //clgbuild(lochosname);

            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    cb_floorname.Checked = true;
                    build = cbl_buildingname.Items[i].Text.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;

                    }

                }
            }

            clgfloor(buildvalue);

            if (seatcount == cbl_buildingname.Items.Count)
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
                cb_buildingname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_buildingname.Text = "--Select--";
            }
            else
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region Floor
    public void clgfloor(string buildname)
    {
        try
        {
            //chklstfloorpo3.Items.Clear();
            cbl_floorname.Items.Clear();
            ds = d2.BindFloor(buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "FloorPK";
                cbl_floorname.DataBind();

            }
            else
            {
                txt_floorname.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                cbl_floorname.Items[i].Selected = true;
                cb_floorname.Checked = true;
            }

            string locfloor = "";
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                    string flrname = cbl_floorname.Items[i].Text;
                    if (locfloor == "")
                    {
                        locfloor = flrname;
                    }
                    else
                    {
                        locfloor = locfloor + "'" + "," + "'" + flrname;
                    }
                }

            }
            clgroom(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbfloorname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_floorname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";

                if (cb_buildingname.Checked == true)
                {
                    for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                    {
                        build1 = cbl_buildingname.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                if (cb_floorname.Checked == true)
                {
                    for (int j = 0; j < cbl_floorname.Items.Count; j++)
                    {
                        cbl_floorname.Items[j].Selected = true;
                        txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                        build2 = cbl_floorname.Items[j].Text.ToString();
                        if (buildvalue2 == "")
                        {
                            buildvalue2 = build2;
                        }
                        else
                        {
                            buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                        }
                    }
                }
                clgroom(buildvalue2, buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = false;
                    txt_floorname.Text = "--Select--";
                }
                cb_roomname.Checked = false;
                cbl_roomname.Items.Clear();
                txt_roomname.Text = "--Select--";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblfloorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floorname.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    build1 = cbl_buildingname.Items[i].Text.ToString();
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                    }

                }
            }
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build2 = cbl_floorname.Items[i].Text.ToString();
                    if (buildvalue2 == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }
                }
            }
            clgroom(buildvalue2, buildvalue1);

            if (seatcount == cbl_floorname.Items.Count)
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
                cb_floorname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_floorname.Text = "--Select--";
            }
            else
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
            //  clgroom(buildvalue1, buildvalue2);
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region Room
    public void clgroom(string floorname, string buildname)
    {
        try
        {
            cbl_roomname.Items.Clear();
            ds = d2.BindRoom(floorname, buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname.DataSource = ds;
                cbl_roomname.DataTextField = "Room_Name";
                cbl_roomname.DataValueField = "Roompk";
                cbl_roomname.DataBind();
            }
            else
            {
                txt_roomname.Text = "--Select--";
            }

            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                cbl_roomname.Items[i].Selected = true;
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
                cb_roomname.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbroomname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomname.Checked == true)
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = true;
                }
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = false;
                }
                txt_roomname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblroomname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_roomname.Checked = false;
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            if (seatcount == cbl_roomname.Items.Count)
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
                cb_roomname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_roomname.Text = "--Select--";
            }
            else
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region go
    protected void btnguest_go_Click(object sender, EventArgs e)
    {
        try
        {
            string date = "";
            string floorname = "";
            string date1 = "";
            int gurowcount = 0;
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    string floorname1 = cbl_floorname.Items[i].Value.ToString();
                    if (floorname == "")
                    {
                        floorname = floorname1;
                    }
                    else
                    {
                        floorname = floorname + "'" + "," + "'" + floorname1;
                    }
                }
            }
            string buildingname = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    string buildingname1 = cbl_buildingname.Items[i].Value.ToString();
                    if (buildingname == "")
                    {
                        buildingname = buildingname1;
                    }
                    else
                    {
                        buildingname = buildingname + "'" + "," + "'" + buildingname1;
                    }
                }
            }
            string roomname = "";
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    string roomname1 = cbl_roomname.Items[i].Value.ToString();
                    if (roomname == "")
                    {
                        roomname = roomname1;
                    }
                    else
                    {
                        roomname = roomname + "'" + "," + "'" + roomname1;
                    }
                }
            }
            string hoscode = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    string hoscode1 = cbl_hostelname.Items[i].Value.ToString();
                    if (hoscode == "")
                    {
                        hoscode = hoscode1;
                    }
                    else
                    {
                        hoscode = hoscode + "'" + "," + "'" + hoscode1;
                    }
                }
            }
            if (txt_hostelname.Text.Trim() != "--Select--" && txt_buildingname.Text.Trim() != "--Select--" && txt_floorname.Text.Trim() != "--Select--" && txt_roomname.Text.Trim() != "--Select--")
            {
                string q = "select HM.HostelName as Hostel_Name,Vi.VenContactName as Guest_Name,Vi.VendorContactPK as GuestCode,V.VendorAddress as Guest_Address,Vi.VendorMobileNo as MobileNo,V.VendorCompName as From_Company,f.Floor_Name as Floor_Name,r.Room_Name as Room_Name,HM.HostelMasterPK as Hostel_Code,B.Building_Name,B.Code,V.VendorStreet as Guest_Street,V.VendorCity as Guest_City,V.VendorPin as Guest_PinCode from HT_HostelRegistration H,CO_VendorMaster V,IM_VendorContactMaster Vi,Building_Master B,Floor_Master f,Room_Detail r,HM_HostelMaster HM where hm.HostelMasterPK =h.HostelMasterFK and v.VendorPK=vi.VendorFK and b.Code =h.BuildingFK and f.FloorPK=H.FloorFK and r.RoomPk=H.RoomFK and B.Code in('" + buildingname + "') and H.FloorFK in('" + floorname + "') and H.RoomFK in('" + roomname + "') and HM.HostelMasterPK in('" + hoscode + "') and H.GuestVendorFK=v.VendorPK and vi.VendorContactPK=h.APP_No";//and HM.CollegeCode='" + collegecode1 + "' 

                ds.Clear();
                ds = d2.select_method_wo_parameter(q, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpreadguest.Sheets[0].RowCount = 0;
                    FpSpreadguest.SaveChanges();
                    FpSpreadguest.SheetCorner.ColumnCount = 0;
                    FpSpreadguest.CommandBar.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpreadguest.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                    FpSpreadguest.Sheets[0].RowCount = FpSpreadguest.Sheets[0].RowCount + 1;
                    FpSpreadguest.Sheets[0].SpanModel.Add(FpSpreadguest.Sheets[0].RowCount - 1, 0, 1, 3);
                    FpSpreadguest.Sheets[0].AutoPostBack = false;

                    FpSpreadguest.Sheets[0].RowCount = 0;
                    FpSpreadguest.Sheets[0].ColumnCount = 5;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadguest.Sheets[0].Columns[0].Locked = true;
                        FpSpreadguest.Columns[0].Width = 80;

                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Guest Name";
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadguest.Sheets[0].Columns[1].Locked = true;
                        FpSpreadguest.Columns[1].Width = 100;

                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Guest Code";
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadguest.Sheets[0].Columns[2].Locked = true;
                        FpSpreadguest.Columns[2].Width = 200;

                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hostel Name";
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadguest.Sheets[0].Columns[3].Locked = true;
                        FpSpreadguest.Columns[3].Width = 200;


                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Select";
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpreadguest.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpreadguest.Columns[4].Width = 150;
                        FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();


                        for (int gurow = 0; gurow < ds.Tables[0].Rows.Count; gurow++)
                        {
                            sno++;
                            FpSpreadguest.Sheets[0].RowCount = FpSpreadguest.Sheets[0].RowCount + 1;

                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[gurow]["GuestCode"]);

                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[gurow]["Guest_Name"]);
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[gurow]["GuestCode"]);
                            //FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[gurow]["desig_code"]);
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[gurow]["Hostel_Name"]);
                            //FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[gurow]["desig_code"]);
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 4].CellType = chk;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpreadguest.Sheets[0].Cells[FpSpreadguest.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        }

                        lbl_errorsearch3.Visible = true;
                        lbl_errorsearch3.Text = "No of guest :" + sno.ToString();
                        gurowcount = Fpstaff.Sheets[0].RowCount;
                        FpSpreadguest.Height = 345;
                        FpSpreadguest.Width = 846;
                        btn_guestok.Visible = true;
                        btn_guestexit.Visible = true;
                        FpSpreadguest.Visible = true;
                        divGuest.Visible = true;
                        FpSpreadguest.Sheets[0].PageSize = 25 + (gurowcount * 20);
                        FpSpreadguest.SaveChanges();

                    }

                }
                else
                {
                    FpSpreadguest.Visible = false;
                    btn_guestok.Visible = false;
                    btn_guestexit.Visible = false;
                    divGuest.Visible = false;
                    lbl_errorsearch2.Visible = true;
                    lbl_errorsearch2.Text = "No Records Found";
                    lbl_errorsearch3.Visible = false;
                }


            }
        }
        catch
        { }
    }
    #endregion

    protected void btn_guestok_Click(object sender, EventArgs e)
    {
        try
        {
            string guestName = "";
            string guestcode = "";
            string guestaddress = "";
            string guesthostelname = "";
            FpSpreadguest.SaveChanges();
            activerow = FpSpreadguest.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpreadguest.ActiveSheetView.ActiveColumn.ToString();
            string guestcod = string.Empty;
            string guesfName = string.Empty;
            string gueshostelname = string.Empty;
            string selectguescode = string.Empty;
            string selectguesName = string.Empty;
            string selectgueshostelname = string.Empty;
            ArrayList gueshostelnameArr = new ArrayList();
            int guestCount = 0;
            for (int gtrow = 0; gtrow < FpSpreadguest.Sheets[0].RowCount; gtrow++)
            {
                selected = 0;
                int.TryParse(Convert.ToString(FpSpreadguest.Sheets[0].Cells[gtrow, 4].Value), out selected);
                if (selected == 1)
                {
                    guestcod = Convert.ToString(FpSpreadguest.Sheets[0].Cells[gtrow, 2].Text).Trim();
                    guesfName = Convert.ToString(FpSpreadguest.Sheets[0].Cells[gtrow, 1].Text).Trim();
                    gueshostelname = Convert.ToString(FpSpreadguest.Sheets[0].Cells[gtrow, 3].Text).Trim();
                    if (String.IsNullOrEmpty(selectguescode))
                        selectguescode = guestcod;
                    else
                        selectguescode += ";" + guestcod;
                    if (String.IsNullOrEmpty(selectguesName))
                        selectguesName = guesfName;
                    else
                        selectguesName += ";" + guesfName;

                    if (String.IsNullOrEmpty(selectgueshostelname))
                    {
                        selectgueshostelname = gueshostelname;
                        gueshostelnameArr.Add(gueshostelname);
                    }
                    else
                        if (!gueshostelnameArr.Contains(gueshostelname))
                        {
                            selectgueshostelname += ";" + gueshostelname;
                            gueshostelnameArr.Add(gueshostelname);
                        }
                    guestCount++;
                }
            }
            if (guestCount == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Any Guest";
                return;
            }
            if (guestCount > 1)
            {
                txt_hostelname1.Text = selectgueshostelname;
                lbl_Sturollno.Text = selectguescode;
                ViewState["NoOfGuest"] = guestCount;
                lblstudent.Visible = true;
                lblstudent.Text = "No Of Guest:";
                lblstudentcount.Visible = true;
                string Guestcnt = Convert.ToString(ViewState["NoOfGuest"]);
                lblstudentcount.Text = Guestcnt;
                lbl_guest.Visible = false;
                txt_guestname.Visible = false;
                txt_guestname.Enabled = false;
                txt_gustCode.Enabled = false;
                lbl_Sturollno.Visible = false;
                lbl_stu.Visible = false;
                lbl_staff.Visible = false;
                lbl_Sturollno.Visible = false;
                lbl_guestname.Visible = false;
                txt_guestname.Visible = false;
                txt_stu.Visible = true;
                txt_stu.Text = Guestcnt;
                DivGuestpopupwindow.Visible = false;

            }
            else
            {
                lblstudent.Visible = false;
                lblstudentcount.Visible = false;
                guestName = FpSpreadguest.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                txt_guestname.Text = guestName;
                guestaddress = FpSpreadguest.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                txt_gustCode.Text = guestaddress;
                guesthostelname = FpSpreadguest.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                txt_hostelname1.Text = guesthostelname;
                lbl_guest.Visible = true;
                txt_guestname.Visible = true;
                txt_guestname.Enabled = true;
                txt_gustCode.Enabled = true;
                txt_stu.Visible = false;
                lbl_stu.Visible = false;
                lbl_staff.Visible = false;
                lbl_guest.Visible = false;
                lbl_Sturollno.Visible = false;
                lbl_guestname.Visible = true;
                txt_guestname.Visible = true;
                txt_stu.Visible = false;
                DivGuestpopupwindow.Visible = false;

            }


        }
        catch
        {


        }
    }


    protected void btn_guestexit_Click(object sender, EventArgs e)
    {
        DivGuestpopupwindow.Visible = false;
    }
    #endregion

    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread2, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {

            string degreedetails = "Gym Allotment";
            string pagename = "GymAllotment.aspx";
            Printcontrol.loadspreaddetails(Fpspread2, pagename, degreedetails);
            printdiv.Visible = true;
            Printcontrol.Visible = true;
            // 
        }
        catch
        {
        }
    }
    #endregion

    #region Close
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            imgdiv2.Visible = false;
        }
        catch
        {

        }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popupstudaddinl.Visible = false;
    }

    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        DivGuestpopupwindow.Visible = false;
    }



    protected void btn_exit_Click(object sender, EventArgs e)
    {
        popupstudaddinl.Visible = false;
    }

    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
    }
    #endregion
}