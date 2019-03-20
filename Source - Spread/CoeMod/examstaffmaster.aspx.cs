using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Configuration;

public partial class examstaffmaster : System.Web.UI.Page
{
    #region declaration
    string usercode = "";
    string collegecode = "";
    string singleuser = "";
    string groupuser = "";
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 da = new DAccess2();
    DataSet ds2 = new DataSet();
    DataSet dsdear = new DataSet();
    Hashtable ht = new Hashtable();
    string name_active = "";
    string des_active = "";
    string perexp = "";
    DateTime da1;
    Boolean Cellclick = true;
    Boolean cellclicktext = false;
    Boolean flag_true = false;
    string SenderID = string.Empty;
    string Password = string.Empty;
    string user_id = string.Empty;

    string message = string.Empty;
    string message1 = string.Empty;
    string strmobileno = string.Empty;
    string mobilenos = "";

    string mailid = string.Empty;
    string mailpwd = string.Empty;
    string to_mail = string.Empty;
    string strstuname = string.Empty;
    Boolean fpupdate = false;
    #endregion
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
            txtyear.Enabled = false;
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            groupuser = Session["group_code"].ToString();
            lblmessage1.Visible = false;
            lblerrormsg.Visible = false;
            if (!IsPostBack)
            {
                txttravelallowance.Text = string.Empty;
                txtdailyallowance.Text = string.Empty;
                txtyear.Attributes.Add("readonly", "readonly");
                panelinvilation.Visible = false;
                purpose();
                spread1();
                panel7.Visible = false;
                panel9.Visible = false;
                txtsettingtextbox.Visible = false;
                txtvalutationtextbox.Visible = false;
                bindexternalyear();
                stream();
                staffsteram();
                subjectload();
                lblerror1.Visible = false;
                txtmessage.Visible = false;
                btnsms.Visible = false;
                lblmessage1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                btndelete.Visible = false;
                fsstaff.Sheets[0].AutoPostBack = true;
                fsstaff.CommandBar.Visible = false;

                FarPoint.Web.Spread.StyleInfo styles = new FarPoint.Web.Spread.StyleInfo();
                styles.Font.Size = 10;
                styles.Font.Bold = true;
                fsstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(styles);
                fsstaff.Sheets[0].AllowTableCorner = true;
                fsstaff.Sheets[0].RowHeader.Visible = false;

                fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

                fsstaff.Sheets[0].DefaultColumnWidth = 50;
                fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                fsstaff.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.Black;
                fsstaff.Sheets[0].ColumnHeader.DefaultStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

                fsstaff.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fsstaff.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fsstaff.Sheets[0].DefaultStyle.Font.Bold = false;
                fsstaff.SheetCorner.Cells[0, 0].Font.Bold = true;

                fsstaff.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                fsstaff.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;


                fsstaff.Sheets[0].ColumnCount = 3;
                fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Code";
                fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Name";

                fsstaff.Sheets[0].Columns[0].Width = 80;
                fsstaff.Sheets[0].Columns[1].Width = 103;
                fsstaff.Sheets[0].Columns[2].Width = 300;

                fsstaff.Sheets[0].Columns[0].Locked = true;
                fsstaff.Sheets[0].Columns[1].Locked = true;
                fsstaff.Sheets[0].Columns[2].Locked = true;
                main();
                scheme();
                ddlexdept.Attributes.Add("onfocus", "depart()");
                ddlextcity.Attributes.Add("onfocus", "city()");
                ddlextuniv.Attributes.Add("onfocus", "instition()");
                ddlexterdesign.Attributes.Add("onfocus", "design()");
                checktype.Items[0].Selected = true;
                designation();
                department();
                city();
                instition();
                checkedtype();
                ddltoexp.SelectedIndex = ddltoexp.Items.Count - 1;

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void scheme()
    {
        try
        {
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            ddlscheme.Items.Clear();
            string sqlquery = "select distinct type from course";
            ds = da.select_method_wo_parameter(sqlquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlscheme.DataSource = ds;
                ddlscheme.DataTextField = "type";
                ddlscheme.DataValueField = "type";
                ddlscheme.DataBind();
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void ddlemptype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            main();
            Btnedit.Focus();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }

    }

    public void stream()
    {
        try
        {
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            ddlstreamview.Items.Clear();
            ddlstreamadd.Items.Clear();
            string typequery = "select distinct UPPER(LTRIM(RTRIM(isnull(type,'')))) as type from course where LTRIM(RTRIM(isnull(type,'')))<>'' order by type";
            ds = da.select_method_wo_parameter(typequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlstreamview.DataSource = ds;
                ddlstreamview.DataTextField = "type";
                ddlstreamview.DataValueField = "type";
                ddlstreamview.DataBind();
                ddlstreamview.Items.Insert(0, "All");

                ddlstreamadd.DataSource = ds;
                ddlstreamadd.DataValueField = "type";
                ddlstreamadd.Items.Insert(0, "All");
                ddlstreamadd.DataBind();
                ddlstreamadd.DataTextField = "type";



            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void ddlmonthadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        subjectload();
        txtsettingtextbox.Text = "--Select--";
        txtvalutationtextbox.Text = "--Select--";
        Btnedit.Focus();
    }

    //protected void month()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        ds.Dispose();
    //        ds.Reset();
    //        ddlmonthview.Items.Clear();
    //        string year1 = ddlstreamview.SelectedValue;
    //        ds = da.Exammonth(year1);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlmonthview.DataSource = ds;
    //            ddlmonthview.DataTextField = "monthName";
    //            ddlmonthview.DataValueField = "Exam_month";
    //            ddlmonthview.DataBind();


    //        }
    //    }
    //    catch (SqlException ex)
    //    {
    //        lblerrormsg.Visible = true;
    //        lblerrormsg.Text = ex.ToString();
    //    }

    //}
    //protected void month1()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        ds.Dispose();
    //        ds.Reset();
    //        ddlmonthadd.Items.Clear();
    //        string year1 = ddlstreamadd.SelectedValue;
    //        ds = da.Exammonth(year1);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {

    //            ddlmonthadd.DataSource = ds;
    //            ddlmonthadd.DataTextField = "monthName";
    //            ddlmonthadd.DataValueField = "Exam_month";
    //            ddlmonthadd.DataBind();



    //        }
    //    }
    //    catch (SqlException ex)
    //    {
    //        lblerrormsg.Visible = true;
    //        lblerrormsg.Text = ex.ToString();
    //    }

    //}

    protected void main()
    {
        if (ddlemptype.Text == "1")
        {
            panelinvilation.Visible = false;
            ddlempno1.Visible = false;
            ddlmrs.Visible = true;
            ddlempno.Visible = true;
            lblstaffname.Visible = false;
            txtissueper.Visible = false;
            btnstaff.Visible = false;
            lblempno.Text = "Name";
            lblscheme.Visible = false;
            ddlscheme.Visible = false;
            lbluniversity.Visible = false;
            txtuniversity.Visible = false;
            ddlscheme.Enabled = true;
            txtissueper.Enabled = true;
            ddlempno.Enabled = true;
            ddldept.Enabled = true;
            ddlgender.Enabled = true;
            txtdesign.Enabled = true;
            txtinstition.Visible = false;
            txtuniversity.Enabled = true;
            txtaddress2.Enabled = true;
            txtaddress3.Enabled = true;
            ddlyear.Enabled = true;
            txtaddress1.Enabled = true;
            txtcity.Visible = false;
            txtpincode.Enabled = true;
            txtmobile.Enabled = true;
            txtphone.Enabled = true;
            txtemil.Enabled = true;
            ddlstatedyear.Enabled = true;
            ddlempno.Text = "";
            ddldept.Text = "";
            ddldept.Visible = false;
            ddlgender.SelectedIndex = 0;
            txtdesign.Text = "";
            txtinstition.Text = "";
            txtuniversity.Text = "";
            ddlyear.SelectedIndex = 0;
            txtaddress1.Text = "";
            txtcity.Text = "";
            txtpincode.Text = "";
            txtmobile.Text = "";
            txtphone.Text = "";
            txtemil.Text = "";
            ddlexdept.Visible = true;
            ddlexterdesign.Visible = true;
            ddlextuniv.Visible = true;
            ddlextcity.Visible = true;
            txtdesign.Visible = false;
            txtsettingtextbox.Text = "--Select--";
            txtvalutationtextbox.Text = "--Select--";
            lblsetting.Checked = false;
            lblvalution.Checked = false;
            panel9.Visible = false;
            txtsettingtextbox.Visible = false;
            panel7.Visible = false;
            txtvalutationtextbox.Visible = false;
            trBank.Visible = true;
            trBankIfsc.Visible = true;
            trBankAcc.Visible = true;
        }
        else if (ddlemptype.Text == "0")
        {
            panelinvilation.Visible = true;
            ddlempno1.Visible = true;
            txtissueper.Text = "";
            txtinstition.Enabled = true;
            txtcity.Enabled = true;
            ddlmrs.Visible = false;
            ddlexdept.Visible = false;
            ddlexterdesign.Visible = false;
            ddlextuniv.Visible = false;
            ddlextcity.Visible = false;
            ddldept.Visible = true;
            txtcity.Visible = true;
            txtinstition.Visible = true;
            txtdesign.Visible = true;
            lblstaffname.Visible = true;
            txtissueper.Visible = true;
            btnstaff.Visible = true;
            lblempno.Text = "Staff Code";
            lblscheme.Visible = true;
            ddlscheme.Visible = true;
            lbluniversity.Visible = true;
            txtuniversity.Visible = true;
            ddlempno.Visible = false;
            ddlempno.Text = "";
            ddldept.Text = "";
            ddlgender.SelectedIndex = 0;
            txtdesign.Text = "";
            txtinstition.Text = "";
            txtuniversity.Text = "";
            ddlyear.SelectedIndex = 0;
            txtaddress1.Text = "";
            txtcity.Text = "";
            txtpincode.Text = "";
            txtmobile.Text = "";
            txtphone.Text = "";
            txtemil.Text = "";
            txtsettingtextbox.Text = "--Select--";
            txtvalutationtextbox.Text = "--Select--";
            lblsetting.Checked = false;
            lblvalution.Checked = false;
            panel9.Visible = false;
            txtsettingtextbox.Visible = false;
            panel7.Visible = false;
            txtvalutationtextbox.Visible = false;
            trBank.Visible = false;
            trBankIfsc.Visible = false;
            trBankAcc.Visible = false;
        }
    }

    public void bind()
    {
        try
        {
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            lblerror1.Visible = false;
            string sqlquery = "";
            if (ddlempno1.Text != "")
            {
                sqlquery = "select * from staff_appl_master sa,staffmaster sm,desig_master dm where sa.appl_no=sm.appl_no and sm.staff_code='" + ddlempno1.Text + "'  and sa.college_code=dm.collegeCode and sa.desig_code=dm.desig_code select * from collinfo where college_code='" + collegecode + "'";
            }
            else
            {
                sqlquery = "select * from staff_appl_master sa,staffmaster sm,desig_master dm where sa.appl_no=sm.appl_no and sm.staff_code='" + des_active.ToString() + "'  and sa.college_code=dm.collegeCode and sa.desig_code=dm.desig_code select * from collinfo where college_code='" + ddlcollege.SelectedValue + "'";

            }
            ds = da.select_method_wo_parameter(sqlquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtissueper.Text = Convert.ToString(ds.Tables[0].Rows[0]["Staff_Name"]);
                ddlempno1.Text = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"]);
                ddldept.Text = Convert.ToString(ds.Tables[0].Rows[0]["dept_name"]);
                ddlgender.Text = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
                txtdesign.Text = Convert.ToString(ds.Tables[0].Rows[0]["desig_name"]);
                txtinstition.Text = Convert.ToString(ds.Tables[1].Rows[0]["collname"]);
                txtuniversity.Text = Convert.ToString(ds.Tables[1].Rows[0]["university"]);
                da1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["join_date"].ToString());
                if (ddlyear.Items.Count > 0)
                {
                    if (ddlyear.Items.FindByValue("" + da1.ToString("yyyy") + "") != null)
                    {
                        ddlyear.SelectedValue = da1.ToString("yyyy");
                    }
                    else
                    {
                        ddlyear.Items.Insert(ddlyear.Items.Count - 1, da1.ToString("yyyy"));
                        ddlyear.SelectedValue = da1.ToString("yyyy");
                    }
                }
                // scheme();
                //  ddlscheme.Text = ds.Tables[0].Rows[0]["Stream"].ToString();
                ddlscheme.SelectedIndex = ddlscheme.Items.IndexOf(ddlscheme.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["Stream"])));
                txtaddress1.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_address1"]);
                txtcity.Text = Convert.ToString(ds.Tables[0].Rows[0]["pcity"]);
                txtpincode.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_pincode"]);
                txtmobile.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_mobileno"]);
                txtphone.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_phone"]);
                txtemil.Text = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                perexp = Convert.ToString(ds.Tables[0].Rows[0]["experience_info"]);
                exper();
                ddlempno.Visible = true;
                txtissueper.Enabled = true;
                ddlstatedyear.Enabled = false;
                ddlempno.Visible = false;
                ddldept.Enabled = false;
                ddlgender.Enabled = false;
                txtdesign.Enabled = false;
                txtinstition.Enabled = false;
                txtuniversity.Enabled = false;
                txtaddress2.Enabled = false;
                txtaddress3.Enabled = false;
                ddlyear.Enabled = false;
                txtaddress1.Enabled = false;
                txtcity.Enabled = false;
                txtpincode.Enabled = false;
                txtmobile.Enabled = false;
                txtphone.Enabled = false;
                txtemil.Enabled = false;
                txtyear.Enabled = false;
                ddlscheme.Enabled = false;
                perexp = ds.Tables[0].Rows[0]["experience_info"].ToString();
            }
            else
            {
                txtissueper.Text = "";
                ddldept.Text = "";
                ddlgender.SelectedIndex = 0;
                txtdesign.Text = "";
                txtinstition.Text = "";
                txtuniversity.Text = "";
                ddlyear.SelectedIndex = 0;
                ddlscheme.SelectedIndex = 0;
                txtaddress1.Text = "";
                txtcity.Text = "";
                txtpincode.Text = "";
                txtmobile.Text = "";
                txtphone.Text = "";
                txtemil.Text = "";
                ddlstatedyear.SelectedIndex = 0;
                ddlempno.Visible = true;
                txtissueper.Enabled = true;
                ddlstatedyear.Enabled = false;
                txtyear.Text = "";
                ddlempno.Visible = false;
                ddldept.Enabled = false;
                ddlgender.Enabled = false;
                txtdesign.Enabled = false;
                txtinstition.Enabled = false;
                txtuniversity.Enabled = false;
                txtaddress2.Enabled = false;
                txtaddress3.Enabled = false;
                ddlyear.Enabled = false;
                txtaddress1.Enabled = false;
                txtcity.Enabled = false;
                txtpincode.Enabled = false;
                txtmobile.Enabled = false;
                txtphone.Enabled = false;
                txtemil.Enabled = false;
                txtyear.Enabled = false;
                ddlscheme.Enabled = false;
                lblerror1.Text = "Staff Code Is Not Registered";
                lblerror1.Visible = true;
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    public void exper()
    {
        try
        {
            int cureyear = 0;
            int curemonth = 0;
            string joindatestaff = "-";
            if (da1.ToString() != "" && da1 != null)
            {
                DateTime dtexp = Convert.ToDateTime(da1);
                joindatestaff = dtexp.ToString("dd/MM/yyyy");
            }
            if (da1.ToString() != "" && da1 != null)
            {
                DateTime dt = DateTime.Now;
                DateTime dtexp = Convert.ToDateTime(da1);
                int cury = Convert.ToInt32(dt.ToString("yyyy"));
                int jyear = Convert.ToInt32(dtexp.ToString("yyyy"));
                cureyear = cury - jyear;
                int curmon = Convert.ToInt32(dt.ToString("MM"));
                int jmon = Convert.ToInt32(dtexp.ToString("MM"));
                if (curmon < jmon)
                {
                    curemonth = (curmon + 12) - jmon;
                    cureyear--;
                }
                else
                {
                    curemonth = curmon - jmon;
                }
            }
            txtyear.Text = cureyear.ToString();
            if (ddlstatedyear.Items.Count > 0)
            {
                if (ddlstatedyear.Items.FindByValue("" + da1.ToString("yyyy") + "") != null)
                {
                    ddlstatedyear.SelectedValue = da1.ToString("yyyy");
                }
                else
                {
                    ddlstatedyear.Items.Insert(ddlstatedyear.Items.Count - 1, da1.ToString("yyyy"));
                    ddlstatedyear.SelectedValue = da1.ToString("yyyy");
                }
            }
            if (perexp != "")
            {
                int expyear = 0;
                int expmon = 0;
                string[] spit = perexp.Split('\\');
                for (int s = 0; s <= spit.GetUpperBound(0); s++)
                {
                    if (spit[s].Trim().ToString() != "" && spit[s] != "")
                    {
                        string[] sporg = spit[s].Split(';');
                        if (sporg.GetUpperBound(0) > 10)
                        {
                            string yer = sporg[6].ToString();
                            if (yer.ToString().Trim() != "" && yer != null)
                            {
                                expyear = expyear + Convert.ToInt32(yer);
                            }
                            ddlstatedyear.Text = expyear.ToString();
                            string mon = sporg[7].ToString();
                            if (mon.ToString().Trim() != "" && mon != null)
                            {
                                expmon = expmon + Convert.ToInt32(mon);
                            }
                        }
                    }
                }
                int exy = 0;
                int exaxcm = 0;
                if (expmon.ToString().Trim() != "" && expmon != null)
                {
                    if (expmon > 11)
                    {
                        exy = expmon / 12;
                        exaxcm = expmon % 12;
                    }
                    else
                    {
                        exaxcm = expmon;
                    }
                }
                expyear = expyear + exy;
                int totalexpyear = cureyear + expyear;
                int totalexpmonth = curemonth + exaxcm;
                if (totalexpmonth > 11)
                {
                    totalexpmonth = totalexpmonth - 12;
                    totalexpyear++;
                }
                txtyear.Text = totalexpyear.ToString();

            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }

    }

    protected void btnnew_click(object sender, EventArgs e)
    {
        try
        {
            txtdailyallowance.Text = string.Empty;
            txttravelallowance.Text = string.Empty;

            if (ddlemptype.Text == "1")
            {
                panelinvilation.Visible = false;
                ddlempno.Visible = true;
                lblscred.Checked = false;
                ddlmrs.Visible = true;
                AddPageModify.Text = "Add";
                btnsave.Text = "Save";
                lblerror1.Visible = false;
                lblstaffname.Visible = false;
                txtissueper.Visible = false;
                btnstaff.Visible = false;
                lblempno.Text = "Name";
                lblscheme.Visible = false;
                ddlscheme.Visible = false;
                lbluniversity.Visible = false;
                txtuniversity.Visible = false;
                ddlscheme.Enabled = true;
                txtissueper.Enabled = true;
                ddlempno.Visible = true;
                ddlempno1.Visible = false;
                ddldept.Enabled = true;
                ddlgender.Enabled = true;
                txtdesign.Enabled = true;
                txtinstition.Visible = false;
                txtuniversity.Enabled = true;
                txtaddress2.Enabled = true;
                txtaddress3.Enabled = true;
                ddlyear.Enabled = true;
                txtaddress1.Enabled = true;
                txtcity.Visible = false;
                txtpincode.Enabled = true;
                txtmobile.Enabled = true;
                txtphone.Enabled = true;
                txtemil.Enabled = true;
                txtyear.Enabled = true;
                ddlempno.Text = "";
                ddldept.Text = "";
                ddldept.Visible = false;
                ddlgender.SelectedIndex = 0;
                txtdesign.Text = "";
                txtinstition.Text = "";
                txtuniversity.Text = "";
                ddlyear.SelectedIndex = 0;
                txtaddress1.Text = "";
                txtaddress2.Text = "";
                txtaddress3.Text = "";
                txtcity.Text = "";
                txtpincode.Text = "";
                txtmobile.Text = "";
                txtphone.Text = "";
                txtemil.Text = "";
                ddlexdept.Visible = true;
                ddlexterdesign.Visible = true;
                ddlextuniv.Visible = true;
                ddlextcity.Visible = true;
                txtdesign.Visible = false;
                ddlstreamadd.SelectedIndex = 0;
                ddlFnAn.Text = "F.N/A.N";
                lblsetting.Checked = false;
                lblvalution.Checked = false;
                lblinvi.Checked = false;
                ddlexdept.Text = "---Select---";
                ddlextcity.Text = "---Select---";
                ddlexterdesign.Text = "---Select---";
                ddlextuniv.Text = "---Select---";
                txtyear.Text = "";
                ddlemptype.Enabled = true;
                txtsettingtextbox.Text = "--Select--";
                txtvalutationtextbox.Text = "--Select--";
                panel9.Visible = false;
                txtsettingtextbox.Visible = false;
                panel7.Visible = false;
                txtvalutationtextbox.Visible = false;
                ddlstatedyear.SelectedIndex = 0;


            }
            else if (ddlemptype.Text == "0")
            {
                panelinvilation.Visible = true;
                ddlempno1.Enabled = true;
                ddlempno1.Visible = true;
                ddlempno1.Text = "";
                lblscred.Checked = false;
                ddlmrs.Visible = false;
                txtissueper.Enabled = false;
                AddPageModify.Text = "Add";
                btnsave.Text = "Save";
                lblerror1.Visible = false;
                txtissueper.Enabled = true;
                ddlexdept.Visible = false;
                ddlexterdesign.Visible = false;
                ddlextuniv.Visible = false;
                ddlextcity.Visible = false;
                ddldept.Visible = true;
                txtcity.Visible = true;
                txtinstition.Visible = true;
                txtdesign.Visible = true;
                lblstaffname.Visible = true;
                txtissueper.Visible = true;
                btnstaff.Visible = true;
                lblempno.Text = "Staff Code";
                txtissueper.Text = "";
                lblscheme.Visible = true;
                ddlscheme.Visible = true;
                lbluniversity.Visible = true;
                txtuniversity.Visible = true;
                ddlempno.Text = "";
                ddldept.Text = "";
                ddlgender.SelectedIndex = 0;
                txtdesign.Text = "";
                txtinstition.Text = "";
                txtuniversity.Text = "";
                ddlyear.SelectedIndex = 0;
                txtaddress1.Text = "";
                txtaddress2.Text = "";
                txtaddress3.Text = "";
                txtcity.Text = "";
                txtpincode.Text = "";
                txtmobile.Text = "";
                txtphone.Text = "";
                txtemil.Text = "";
                ddlstreamadd.SelectedIndex = 0;
                ddlFnAn.Text = "F.N/A.N";
                lblsetting.Checked = false;
                lblvalution.Checked = false;
                lblinvi.Checked = false;
                ddlemptype.Enabled = true;
                txtsettingtextbox.Text = "--Select--";
                txtvalutationtextbox.Text = "--Select--";
                panel9.Visible = false;
                txtsettingtextbox.Visible = false;
                panel7.Visible = false;
                txtvalutationtextbox.Visible = false;
                txtyear.Text = "";
                ddlstatedyear.SelectedIndex = 0;

            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void btnsave_click(object sender, EventArgs e)
    {
        try
        {
            string setting = "";
            string invi = "";
            string valution = "";
            string session = "";
            string text = "";
            string scrab = "";
            string settingsubject = "";
            string setting1 = "";
            string valusubject = "";
            string valudation1 = "";
            string set = "";
            Boolean r = false;
            string cityselect = "";
            if (lblsetting.Checked == true || lblinvi.Checked == true || lblvalution.Checked == true || lblscred.Checked == true)
            {
                r = true;
            }
            if (r != true)
            {
                lblerror1.Text = "Please Select Any One Examiner Type";
                lblerror1.Visible = true;
                fpcammarkstaff.Visible = false;
                lblerrormsg.Visible = true;
                lblmessage1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                txtmessage.Visible = false;
                btnsms.Visible = false;
                btnprintmaster.Visible = false;
                btndelete.Visible = false;
                return;
            }
            if (lblsetting.Checked == true)
            {
                setting = "1";
            }
            else
            {
                setting = "0";
            }
            if (lblinvi.Checked == true)
            {
                invi = "1";

            }
            else
            {
                invi = "0";
            }
            if (lblvalution.Checked == true)
            {
                valution = "1";

            }
            else
            {
                valution = "0";
            }
            if (lblscred.Checked == true)
            {
                scrab = "1";
            }
            else
            {
                scrab = "0";
            }
            if (setting == "1")
            {
                if (txtsettingtextbox.Text != "--Select--")
                {
                    for (int i = 0; i < ddlsetting.Items.Count; i++)
                    {
                        if (ddlsetting.Items[i].Selected == true)
                        {
                            setting1 = ddlsetting.Items[i].Value.ToString();
                            if (settingsubject == "")
                            {
                                settingsubject = setting1;
                            }
                            else
                            {
                                settingsubject = settingsubject + "'" + "," + "'" + setting1;
                            }
                        }
                    }
                    //string sqlquery = "select distinct s.subject_no,s.subject_name,s.subject_code from Exam_Details ed,exam_appl_details ead,exam_application ea,subject s where ed.exam_code=ea.exam_code and ead.appl_no=ea.appl_no and ead.subject_no=s.subject_no  and ed.Exam_year=" + ddlstreamadd.SelectedValue + " and subject_code in ('" + settingsubject.ToString() + "')  order by subject_name";
                    string sqlquery = "select distinct s.subject_no,s.subject_name,s.subject_code from Exam_Details ed,exam_appl_details ead,exam_application ea,subject s where ed.exam_code=ea.exam_code and ead.appl_no=ea.appl_no and ead.subject_no=s.subject_no  and subject_code in ('" + settingsubject.ToString() + "')  order by subject_name";
                    DataSet davalutatiom = da.select_method_wo_parameter(sqlquery, "text");
                    settingsubject = "";
                    setting1 = "";
                    for (int i = 0; i < davalutatiom.Tables[0].Rows.Count; i++)
                    {
                        setting1 = Convert.ToString(davalutatiom.Tables[0].Rows[i]["subject_no"]);
                        if (settingsubject == "")
                        {
                            settingsubject = setting1;
                        }
                        else
                        {
                            settingsubject = settingsubject + "," + setting1;
                        }
                    }
                }
                if (settingsubject == "")
                {
                    //lblerror1.Text = "Please Select Any Subject For Setting";
                    //lblerror1.Visible = true;
                    //return;
                }
            }
            if (valution == "1")
            {
                if (txtvalutationtextbox.Text != "--Select--")
                {
                    for (int i = 0; i < ddlvalution.Items.Count; i++)
                    {

                        if (ddlvalution.Items[i].Selected == true)
                        {
                            valudation1 = ddlvalution.Items[i].Value.ToString();
                            if (valusubject == "")
                            {
                                valusubject = valudation1;
                            }
                            else
                            {
                                valusubject = valusubject + "," + valudation1;
                            }
                        }
                    }
                    //string sqlquery = "select distinct s.subject_no,s.subject_name,s.subject_code from subject s where subject_code in ('" + valusubject.ToString() + "') order by subject_name";
                    //DataSet davalutatiom = da.select_method_wo_parameter(sqlquery, "text");
                    //valudation1 = "";
                    //valusubject = "";
                    //for (int i = 0; i < davalutatiom.Tables[0].Rows.Count; i++)
                    //{
                    //    valudation1 = Convert.ToString(davalutatiom.Tables[0].Rows[i]["subject_no"]);
                    //    if (valusubject == "")
                    //    {
                    //        valusubject = valudation1;
                    //    }
                    //    else
                    //    {
                    //        valusubject = valusubject + "," + valudation1;

                    //    }
                    //}
                }
                if (valusubject == "")
                {
                    //lblerror1.Text = "Please Select Any Subject for Valutation";
                    //lblerror1.Visible = true;
                    //return;
                }
            }

            if (ddlFnAn.Text == "F.N")
            {
                session = "F.N";
            }
            else if (ddlFnAn.Text == "A.N")
            {
                session = "A.N";
            }
            else if (ddlFnAn.Text == "F.N/A.N")
            {
                session = "F.N/A.N";
            }
            if (ddlemptype.Text == "0")
            {
                if (txtissueper.Text != "")
                {
                    if (ddlempno1.Text != "")
                    {
                        //for internal

                        string sqlquery = "if exists(select * from examstaffmaster where  staff_code='" + ddlempno1.Text + "' and type='" + ddlstreamadd.Text + "' )update examstaffmaster set isexternal=" + ddlemptype.SelectedValue + " , setting=" + setting + ",Inivigition=" + invi + ",Valuation=" + valution + ",sectio='" + session + "',type='" + ddlstreamadd.Text + "',Set_subject_no = '" + settingsubject + "',Val_subject_no='" + valusubject + "',scrab='" + scrab + "' where staff_code='" + ddlempno1.Text + "' and type='" + ddlstreamadd.Text + "'  else INSERT INTO examstaffmaster (staff_code,isexternal,setting,Valuation, Inivigition,sectio,yearofexp,type,Set_subject_no,Val_subject_no,scrab) VALUES ('" + ddlempno1.Text + "'," + ddlemptype.SelectedValue + "," + setting + "," + valution + "," + invi + ",'" + session + "','" + txtyear.Text + "','" + ddlstreamadd.Text + "','" + settingsubject + "','" + valusubject + "','" + scrab + "')";
                        //modiifed
                        string taallowance = txttravelallowance.Text.ToString().Trim();
                        string daallowance = txtdailyallowance.Text.ToString().Trim();

                        if (!string.IsNullOrEmpty(taallowance) && !string.IsNullOrEmpty(daallowance))
                            sqlquery = sqlquery + " if exists(select * from staffmaster where  staff_code='" + ddlempno1.Text + "') update staffmaster set daAmount='" + daallowance + "', taAmount='" + taallowance + "' where staff_code='" + ddlempno1.Text + "'";
                        int save = da.insert_method(sqlquery, ht, "Text");

                        if (save != 0)
                        {
                            if (btnsave.Text == "Save")
                            {
                                lblerror1.Visible = false;
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Saved Successfully')", true);
                                btnnew_click(sender, e);
                            }
                            else if (btnsave.Text == "Update")
                            {
                                lblerror1.Visible = false;
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Updated Successfully')", true);
                                btnnew_click(sender, e);
                            }
                            btnsave.Text = "Save";
                        }
                    }
                    else
                    {
                        lblerror1.Text = "Please Select Staff Code";
                        lblerror1.Visible = true;
                    }
                }
                else
                {
                    lblerror1.Text = "Please Select Staff Name";
                    lblerror1.Visible = true;
                }
            }

            else if (ddlemptype.Text == "1")
            {
                if (ddlempno.Text != "")
                {
                    if (ddlyear.Text != "--Select--")
                    {
                        if (ddlstatedyear.Text != "--Select--")
                        {
                            if (ddlexdept.Text != "" && ddlexdept.Text != "---Select---")
                            {
                                if (ddlexterdesign.Text != "" && ddlexterdesign.Text != "---Select---")
                                {
                                    if (ddlextuniv.Text != "" && ddlextuniv.Text != "---Select---")
                                    {
                                        if (ddlextcity.Text == "" || ddlextcity.Text == "---Select---")
                                        {
                                            cityselect = "";
                                        }
                                        else
                                        {
                                            cityselect = ddlextcity.Text;
                                        }
                                        //for updating into external_staff table
                                        string taallowance = txttravelallowance.Text.ToString().Trim();
                                        string daallowance = txtdailyallowance.Text.ToString().Trim();
                                        string BankName = TxtBkName.Text.ToString().Trim();
                                        string ifscCode = TxtIfsc.Text.ToString().Trim();
                                        string BKAccNo = TxtAccNo.Text.ToString().Trim();
                                        if (ddlemptype.Text == "1")
                                        {
                                            string name = ddlempno.Text;
                                            string[] dateg = name.Split('(');
                                            name = dateg[0].ToString();
                                            lblerror1.Visible = false;
                                            if (string.IsNullOrEmpty(taallowance) || string.IsNullOrEmpty(daallowance))
                                            {
                                                taallowance = "0";
                                                daallowance = "0";
                                            }
                                            string sqlqueryinsert = "if exists(select * from external_staff where title='" + ddlmrs.Text + " ' and staff_name='" + name.ToString() + "' and designation='" + ddlexterdesign.SelectedItem + "'and college_name='" + ddlextuniv.SelectedItem + "'and dept_code='" + ddlexdept.SelectedValue + "' and design_code='" + ddlexterdesign.SelectedValue + "' and coll_code='" + ddlextuniv.SelectedValue + "' and sex='" + ddlgender.SelectedValue + "') update external_staff set designation='" + ddlexterdesign.SelectedItem + "',college_name='" + ddlextuniv.SelectedItem + "',dept_code='" + ddlexdept.SelectedValue + "',design_code='" + ddlexterdesign.SelectedValue + "',coll_code='" + ddlextuniv.SelectedValue + "',per_address='" + txtaddress1.Text + "," + txtaddress2.Text + "," + txtaddress3.Text + "',sex='" + ddlgender.SelectedValue + "',experience_info_carrer='" + ddlstatedyear.SelectedValue + "',email='" + txtemil.Text + "',per_phone='" + txtphone.Text + "',dept_name='" + ddlexdept.SelectedItem + "',per_mobileno='" + txtmobile.Text + "',per_pincode='" + txtpincode.Text + "',experience_info='" + ddlyear.Text + "',totalexp='" + txtyear.Text + "',pcity_code='" + cityselect.ToString() + "',daAmount='" + daallowance + "',taAmount='" + taallowance + "',ifsc_code='" + ifscCode + "',acc_no='" + BKAccNo + "',bank_name='" + BankName + "' where staff_name='" + name.ToString() + "' and designation='" + ddlexterdesign.SelectedItem + "'and college_name='" + ddlextuniv.SelectedItem + "'and dept_code='" + ddlexdept.SelectedValue + "' and design_code='" + ddlexterdesign.SelectedValue + "' and coll_code='" + ddlextuniv.SelectedValue + "' and sex='" + ddlgender.SelectedValue + "' else Insert into external_staff(title,staff_name,designation,college_name,dept_code,design_code,coll_code,per_address,sex,email,per_phone,dept_name,per_mobileno,per_pincode,experience_info,experience_info_carrer,totalexp,pcity_code,daAmount,taAmount,ifsc_code,acc_no,bank_name)values('" + ddlmrs.Text + "', '" + name.ToString() + "','" + ddlexterdesign.SelectedItem + "','" + ddlextuniv.SelectedItem + "','" + ddlexdept.SelectedValue + "','" + ddlexterdesign.SelectedValue + "','" + ddlextuniv.SelectedValue + "','" + txtaddress1.Text + "," + txtaddress2.Text + "," + txtaddress3.Text + "','" + ddlgender.SelectedValue + "','" + txtemil.Text + "','" + txtphone.Text + "','" + ddlexdept.SelectedItem + "','" + txtmobile.Text + "','" + txtpincode.Text + "','" + ddlyear.Text + "','" + ddlstatedyear.SelectedValue + "','" + txtyear.Text + "','" + cityselect.ToString() + "','" + daallowance + "','" + taallowance + "','" + ifscCode + "','" + BKAccNo + "','" + BankName + "') select staff_code from external_staff where title='" + ddlmrs.Text + "' and staff_name='" + name.ToString() + "' and designation='" + ddlexterdesign.SelectedItem + "'and college_name='" + ddlextuniv.SelectedItem + "'and dept_code='" + ddlexdept.SelectedValue + "' and design_code='" + ddlexterdesign.SelectedValue + "' and coll_code='" + ddlextuniv.SelectedValue + "' and sex='" + ddlgender.SelectedValue + "' ";

                                            ds = da.select_method_wo_parameter(sqlqueryinsert, "Text");
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                sqlqueryinsert = " if exists(select * from examstaffmaster where  staff_code='" + ds.Tables[0].Rows[0]["staff_code"].ToString() + "'  and type='" + ddlstreamadd.Text + "' ) update examstaffmaster set isexternal=" + ddlemptype.SelectedValue + " , setting=" + setting + ",Valuation=" + valution + ",sectio='" + session + "',yearofexp='" + txtyear.Text + "',Set_subject_no = '" + settingsubject + "',Val_subject_no = '" + valusubject + "' where staff_code='" + ds.Tables[0].Rows[0]["staff_code"].ToString() + "'  and type='" + ddlstreamadd.Text + "'  else INSERT INTO examstaffmaster (staff_code,isexternal,setting,Valuation,sectio,yearofexp,type,Set_subject_no,Val_subject_no) VALUES ('" + ds.Tables[0].Rows[0]["staff_code"].ToString() + "'," + ddlemptype.SelectedValue + "," + setting + "," + valution + ",'" + session + "','" + txtyear.Text + "','" + ddlstreamadd.Text + "','" + settingsubject + "','" + valusubject + "') ";
                                                int save = da.insert_method(sqlqueryinsert, ht, "Text");
                                                if (save != 0)
                                                {
                                                    if (btnsave.Text == "Save")
                                                    {
                                                        lblerror1.Visible = false;
                                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Saved Successfully')", true);
                                                        btnnew_click(sender, e);
                                                    }
                                                    else if (btnsave.Text == "Update")
                                                    {

                                                        lblerror1.Visible = false;
                                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Updated Successfully')", true);
                                                        btnnew_click(sender, e);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        lblerror1.Text = "Please Select Institution";
                                        lblerror1.Visible = true;
                                    }
                                }
                                else
                                {
                                    lblerror1.Text = "Please Select Designation";
                                    lblerror1.Visible = true;
                                }
                            }
                            else
                            {
                                lblerror1.Text = "Please Select Department";
                                lblerror1.Visible = true;
                            }
                        }
                        else
                        {
                            lblerror1.Text = "Please Select Any Carrer Start Yaer";
                            lblerror1.Visible = true;
                        }
                    }
                    else
                    {
                        lblerror1.Text = "Please Select Any Join Year";
                        lblerror1.Visible = true;
                    }
                }
                else
                {
                    lblerror1.Text = "Please Select Name";
                    lblerror1.Visible = true;
                }
            }

        }
        catch (SqlException ex)
        {
            lblerror1.Visible = true;
            lblerror1.Text = ex.ToString();
        }
        txttravelallowance.Text = string.Empty;
        txtdailyallowance.Text = string.Empty;
    }

    protected void btndelete_click(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            Boolean tr = false;
            lblerrormsg.Visible = false;
            fpcammarkstaff.SaveChanges();
            for (int j = 1; j < fpcammarkstaff.Sheets[0].RowCount; j++)
            {
                int gam = Convert.ToInt32(fpcammarkstaff.Sheets[0].Cells[j, 1].Value);
                if (gam == 1)
                {
                    string staffcoce = fpcammarkstaff.Sheets[0].Cells[j, 4].Tag.ToString();
                    string del = "Delete  from examstaffmaster where staff_code='" + staffcoce + "'";
                    ds = da.select_method_wo_parameter(del, "text");
                    tr = true;


                }
            }
            if (tr == true)
            {
                spread();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Deleted Successfully')", true);
            }
            if (tr != true)
            {
                lblerrormsg.Text = "Please Select Any One Staff";
                lblerrormsg.Visible = true;
            }

        }
        catch (SqlException ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void Btnedit_click(object sender, EventArgs e)
    {
        Accordion1.SelectedIndex = 0;
       // Response.Redirect("Default_login.aspx");
    }

    protected void spread()
    {
        try
        {
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            labpurpose.Visible = true;
            ddlpurpose.Visible = true;
            fpspreadpurpose.Visible = true;
            btnaddtemplate.Visible = true;
            btndeletetemplate.Visible = true;
            lblerrormsg.Visible = false;
            fpcammarkstaff.Visible = true;
            lblmessage1.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnxl.Visible = true;
            btnprintmaster.Visible = true;
            btndelete.Visible = true;
            fpcammarkstaff.Sheets[0].AutoPostBack = false;
            fpcammarkstaff.Sheets[0].RowCount = 0;
            fpcammarkstaff.Sheets[0].ColumnCount = 18;
            fpcammarkstaff.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            fpcammarkstaff.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpcammarkstaff.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpcammarkstaff.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = Color.Black;
            style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpcammarkstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpcammarkstaff.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpcammarkstaff.Sheets[0].AllowTableCorner = true;
            fpcammarkstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpcammarkstaff.Sheets[0].AllowTableCorner = true;
            fpcammarkstaff.Sheets[0].AllowTableCorner = true;
            fpcammarkstaff.Sheets[0].Columns[2].Width = 60;
            fpcammarkstaff.Sheets[0].Columns[3].Width = 70;
            fpcammarkstaff.Sheets[0].Columns[4].Width = 120;
            fpcammarkstaff.Sheets[0].Columns[5].Width = 80;
            fpcammarkstaff.Sheets[0].Columns[6].Width = 90;
            fpcammarkstaff.Sheets[0].Columns[7].Width = 70;
            fpcammarkstaff.Sheets[0].Columns[8].Width = 70;
            fpcammarkstaff.Sheets[0].Columns[1].Width = 70;
            fpcammarkstaff.Sheets[0].Columns[0].Width = 50;
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Type";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Update";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Department";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Designation";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Experience";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Setting";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 9].Text = " Valuation";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Inivigilation";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Session";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Institution";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Scraps";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 14].Text = "City";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Email";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Phone No";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Moblie No";
            string department1 = "";
            string department = "";
            string designation = "";
            string designation1 = "";
            string college = "";
            string college1 = "";
            string city = "";
            string city1 = "";
            string text = "";
            string setting1 = "0";
            string valutation1 = "0";
            string invaluation1 = "0";
            string sqlquery = "";
            string session = "";
            string DEPART = "";
            string DESI = "";
            string CIY = "";
            string COLL = "";
            string EXTERDEPART = "";
            string EXTERDESI = "";
            string EXTERCITY = "";
            string EXTERCOLL = "";
            string screb = "0";

            if (check_examtype.Text != "")
            {
                for (int i = 0; i < check_examtype.Items.Count; i++)
                {

                    if (check_examtype.Items[i].Selected == true)
                    {
                        text = check_examtype.Items[i].Value.ToString();
                        if (text == "1")
                        {
                            setting1 = "1";
                        }
                        else if (text == "2")
                        {
                            valutation1 = "1";
                        }
                        else if (text == "3")
                        {
                            invaluation1 = "1";
                        }
                        else if (text == "4")
                        {
                            screb = "1";
                        }
                    }
                }
            }
            else
            {

            }
            if (ddldept11.Text != "")
            {
                for (int i = 0; i < ddldept11.Items.Count; i++)
                {

                    if (ddldept11.Items[i].Selected == true)
                    {
                        department1 = ddldept11.Items[i].Value.ToString();
                        if (department == "")
                        {
                            department = department1;
                        }
                        else
                        {
                            department = department + "'" + "," + "'" + department1;

                        }
                    }
                }
                if (department != "")
                {
                    DEPART = "and st.dept_code in('" + department + "') ";
                    EXTERDEPART = "and exs.dept_code in('" + department + "') ";
                }

            }

            if (ddldept1.Text != "")
            {
                for (int i = 0; i < ddldept1.Items.Count; i++)
                {

                    if (ddldept1.Items[i].Selected == true)
                    {
                        designation1 = ddldept1.Items[i].Value.ToString();
                        if (designation == "")
                        {
                            designation = designation1;
                        }
                        else
                        {
                            designation = designation + "'" + "," + "'" + designation1;

                        }
                    }
                }
                if (designation != "")
                {
                    DESI = "and st.desig_code in('" + designation + "')";
                    EXTERDESI = "and exs.design_code in('" + designation + "') ";
                }
            }

            if (ddlcity.Text != "")
            {
                for (int i = 0; i < ddlcity.Items.Count; i++)
                {

                    if (ddlcity.Items[i].Selected == true)
                    {
                        city1 = ddlcity.Items[i].Value.ToString();
                        if (city == "")
                        {
                            city = city1;
                        }
                        else
                        {
                            city = city + "'" + "," + "'" + city1;

                        }
                    }
                }
                if (city != "")
                {
                    CIY = " and spm.pcity in(select textval from textvaltable t where t.TextCode in('" + city + "')) ";
                    EXTERCITY = "and exs.pcity_code in('" + city + "')";
                }
            }

            if (ddlinstition.Text != "")
            {
                for (int i = 0; i < ddlinstition.Items.Count; i++)
                {

                    if (ddlinstition.Items[i].Selected == true)
                    {
                        college1 = ddlinstition.Items[i].Value.ToString();
                        if (college == "")
                        {
                            college = college1;
                        }
                        else
                        {
                            college = college + "'" + "," + "'" + college1;

                        }
                    }

                }
                if (college != "")
                {
                    COLL = "and sm.college_code in('" + college + "') ";
                    EXTERCOLL = "and exs.coll_code  in('" + college + "')";
                }
            }
            string session1 = "";
            if (ddlsession.Text == "Both")
            {
                session1 = " ";

            }
            else
            {
                session1 = "and sectio ='" + ddlsession.SelectedValue + "'";
            }

            string streamtype = string.Empty;
            if (ddlstreamview.Text == "All")
            {
                if (ddlstreamview.Items.Count > 0)
                {
                    for (int i = 0; i < ddlstreamview.Items.Count; i++)
                    {
                        if (string.IsNullOrEmpty(streamtype))
                            streamtype = ddlstreamview.Items[i].Text.Trim();
                        else
                            streamtype = streamtype + "','" + ddlstreamview.Items[i].Text.Trim();
                    }
                }
            }
            else
                streamtype = ddlstreamview.Text.Trim();
            if (checktype.Text == "")
            {
                if (check_examtype.Text == "")
                {
                    sqlquery = "select distinct sm.staff_code as staffcode,sm.staff_name as staffname,st.dept_code as deptcode,st.desig_code as desigcode,c.college_code as collcode,hm.dept_name as department,spm.pcity as city,dm.desig_name as design,c.collname  as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,spm.email,yearofexp,scrab,Val_subject_no,Set_subject_no from examstaffmaster es,staffmaster sm,stafftrans st,staff_appl_master spm, hrdept_master hm,desig_master dm,collinfo c where es.staff_code=st.staff_code and sm.staff_code=st.staff_code and sm.appl_no=spm.appl_no and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and c.college_code=sm.college_code and sm.settled=0 and sm.resign=0 and st.latestrec=1 " + DEPART + " " + DESI + " " + COLL + " " + CIY + "  and isexternal=0 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + " and type='" + ddlstreamview.Text + "' union  select distinct es.staff_code as staffcode,exs.staff_name as staffname,exs.dept_code as deptcode,exs.design_code as desigcode,exs.coll_code as collcode,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.dept_code) ) as department,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.pcity_code) ) as city,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.design_code) ) as design,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.coll_code) ) as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,email,yearofexp,scrab,Val_subject_no,Set_subject_no from external_staff exs,examstaffmaster es where convert(nvarchar(15),es.staff_code)=convert(nvarchar(15),exs.staff_code)  and isexternal=1 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + " " + EXTERCITY + " " + EXTERCOLL + " " + EXTERDEPART + " " + EXTERDESI + " and type in('" + streamtype + "')";
                }
                else
                {
                    sqlquery = "select distinct sm.staff_code as staffcode,sm.staff_name as staffname,st.dept_code as deptcode,st.desig_code as desigcode,c.college_code as collcode,hm.dept_name as department,spm.pcity as city,dm.desig_name as design,c.collname  as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,spm.email,yearofexp,scrab,Val_subject_no,Set_subject_no from examstaffmaster es,staffmaster sm,stafftrans st,staff_appl_master spm, hrdept_master hm,desig_master dm,collinfo c where es.staff_code=st.staff_code and sm.staff_code=st.staff_code and sm.appl_no=spm.appl_no and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and c.college_code=sm.college_code and sm.settled=0 and sm.resign=0 and st.latestrec=1 " + DEPART + " " + DESI + " " + COLL + " " + CIY + " and   es.setting=" + setting1 + " and es.Valuation=" + valutation1 + " and es.scrab=" + screb + "  and es.Inivigition=" + invaluation1 + " and isexternal=0 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + " and type='" + ddlstreamview.Text + "' union  select distinct es.staff_code as staffcode,exs.staff_name as staffname,exs.dept_code as deptcode,exs.design_code as desigcode,exs.coll_code as collcode,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.dept_code) ) as department,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.pcity_code) ) as city,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.design_code) ) as design,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.coll_code) ) as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,email,yearofexp,scrab,Val_subject_no,Set_subject_no from external_staff exs,examstaffmaster es where convert(nvarchar(15),es.staff_code)=convert(nvarchar(15),exs.staff_code) and es.setting=" + setting1 + "  and  es.Valuation=" + valutation1 + "  and isexternal=1 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + "  " + EXTERCITY + " " + EXTERCOLL + " " + EXTERDEPART + " " + EXTERDESI + " and type in('" + streamtype + "')";
                }

            }
            else
            {
                if (checktype.Items[0].Selected == true && checktype.Items[1].Selected == true)
                {
                    if (check_examtype.Text == "")
                    {
                        sqlquery = "select distinct sm.staff_code as staffcode,sm.staff_name as staffname,st.dept_code as deptcode,st.desig_code as desigcode,c.college_code as collcode,hm.dept_name as department,spm.pcity as city,dm.desig_name as design,c.collname  as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,spm.email,yearofexp,scrab,Val_subject_no,Set_subject_no from examstaffmaster es,staffmaster sm,stafftrans st,staff_appl_master spm, hrdept_master hm,desig_master dm,collinfo c where es.staff_code=st.staff_code and sm.staff_code=st.staff_code and sm.appl_no=spm.appl_no and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and c.college_code=sm.college_code and sm.settled=0 and sm.resign=0 and st.latestrec=1 " + DEPART + " " + DESI + " " + COLL + " " + CIY + "  and isexternal=0 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + " and type='" + ddlstreamview.Text + "' union  select distinct es.staff_code as staffcode,exs.staff_name as staffname,exs.dept_code as deptcode,exs.design_code as desigcode,exs.coll_code as collcode,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.dept_code) ) as department,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.pcity_code) ) as city,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.design_code) ) as design,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.coll_code) ) as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,email,yearofexp,scrab,Val_subject_no,Set_subject_no from external_staff exs,examstaffmaster es where convert(nvarchar(15),es.staff_code)=convert(nvarchar(15),exs.staff_code)  and isexternal=1 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + " " + EXTERCITY + " " + EXTERCOLL + " " + EXTERDEPART + " " + EXTERDESI + " and type in('" + streamtype + "')";
                    }
                    else
                    {
                        sqlquery = "select distinct sm.staff_code as staffcode,sm.staff_name as staffname,st.dept_code as deptcode,st.desig_code as desigcode,c.college_code as collcode,hm.dept_name as department,spm.pcity as city,dm.desig_name as design,c.collname  as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,spm.email,yearofexp,scrab,Val_subject_no,Set_subject_no from examstaffmaster es,staffmaster sm,stafftrans st,staff_appl_master spm, hrdept_master hm,desig_master dm,collinfo c where es.staff_code=st.staff_code and sm.staff_code=st.staff_code and sm.appl_no=spm.appl_no and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and c.college_code=sm.college_code and sm.settled=0 and sm.resign=0 and st.latestrec=1 " + DEPART + " " + DESI + " " + COLL + " " + CIY + " and   es.setting=" + setting1 + " and es.Valuation=" + valutation1 + " and es.scrab=" + screb + "  and es.Inivigition=" + invaluation1 + " and isexternal=0 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + "  and type='" + ddlstreamview.Text + "' union  select distinct es.staff_code as staffcode,exs.staff_name as staffname,exs.dept_code as deptcode,exs.design_code as desigcode,exs.coll_code as collcode,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.dept_code) ) as department,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.pcity_code) ) as city,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.design_code) ) as design,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.coll_code) ) as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,email,yearofexp,scrab,Val_subject_no,Set_subject_no from external_staff exs,examstaffmaster es where convert(nvarchar(15),es.staff_code)=convert(nvarchar(15),exs.staff_code) and es.setting=" + setting1 + "  and  es.Valuation=" + valutation1 + "  and isexternal=1 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + "  " + EXTERCITY + " " + EXTERCOLL + " " + EXTERDEPART + " " + EXTERDESI + " and type in('" + streamtype + "')";
                    }
                }
                else if (checktype.Text == "Internal")
                {
                    if (check_examtype.Text == "")
                    {
                        sqlquery = "select distinct sm.staff_code as staffcode,sm.staff_name as staffname,st.dept_code as deptcode,st.desig_code as desigcode,c.college_code as collcode,hm.dept_name as department,spm.pcity as city,dm.desig_name as design,c.collname  as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,spm.email,yearofexp,scrab,Val_subject_no,Set_subject_no from examstaffmaster es,staffmaster sm,stafftrans st,staff_appl_master spm, hrdept_master hm,desig_master dm,collinfo c where es.staff_code=st.staff_code and sm.staff_code=st.staff_code and sm.appl_no=spm.appl_no and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and c.college_code=sm.college_code and sm.settled=0 and sm.resign=0 and st.latestrec=1 " + DEPART + " " + DESI + " " + COLL + " " + CIY + "  and isexternal=0 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + "  and type in('" + streamtype + "')";
                    }
                    else
                    {
                        sqlquery = "select distinct sm.staff_code as staffcode,sm.staff_name as staffname,st.dept_code as deptcode,st.desig_code as desigcode,c.college_code as collcode,hm.dept_name as department,spm.pcity as city,dm.desig_name as design,c.collname  as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,spm.email,yearofexp,scrab,Val_subject_no,Set_subject_no from examstaffmaster es,staffmaster sm,stafftrans st,staff_appl_master spm, hrdept_master hm,desig_master dm,collinfo c where es.staff_code=st.staff_code and sm.staff_code=st.staff_code and sm.appl_no=spm.appl_no and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and c.college_code=sm.college_code and sm.settled=0 and sm.resign=0 and st.latestrec=1 " + DEPART + " " + DESI + " " + COLL + " " + CIY + " and   es.setting=" + setting1 + " and  es.Valuation=" + valutation1 + " and es.scrab=" + screb + "  and es.Inivigition=" + invaluation1 + " and isexternal=0 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + "  and type in('" + streamtype + "')";
                    }
                }
                else if (checktype.Text == "External")
                {
                    if (check_examtype.Text == "")
                    {
                        sqlquery = "select distinct es.staff_code as staffcode,exs.staff_name as staffname,exs.dept_code as deptcode,exs.design_code as desigcode,exs.coll_code as collcode,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.dept_code) ) as department,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.pcity_code) ) as city,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.design_code) ) as design,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.coll_code) ) as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,email,yearofexp,scrab,Val_subject_no,Set_subject_no from external_staff exs,examstaffmaster es where convert(nvarchar(15),es.staff_code)=convert(nvarchar(15),exs.staff_code)  and isexternal=1 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + "  " + EXTERCITY + " " + EXTERCOLL + " " + EXTERDEPART + " " + EXTERDESI + " and type in('" + streamtype + "')";
                    }
                    else
                    {
                        sqlquery = "select distinct es.staff_code as staffcode,exs.staff_name as staffname,exs.dept_code as deptcode,exs.design_code as desigcode,exs.coll_code as collcode,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.dept_code) ) as department,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.pcity_code) ) as city,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.design_code) ) as design,(select textval from textvaltable t where convert(nvarchar(15),t.TextCode)=convert(nvarchar(15),exs.coll_code) ) as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,email,yearofexp,scrab,Val_subject_no,Set_subject_no from external_staff exs,examstaffmaster es where convert(nvarchar(15),es.staff_code)=convert(nvarchar(15),exs.staff_code) and es.setting=" + setting1 + " and es.Valuation=" + valutation1 + "  and isexternal=1 " + session1 + " and yearofexp between " + ddlfromexp.Text + " and " + ddltoexp.Text + "" + EXTERCITY + " " + EXTERCOLL + " " + EXTERDEPART + " " + EXTERDESI + "  and type in('" + streamtype + "')";
                    }
                }
            }
            ds = da.select_method_wo_parameter(sqlquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int cn = 0;
                fpcammarkstaff.Sheets[0].RowCount++;
                fpcammarkstaff.Sheets[0].SpanModel.Add(0, 0, 1, 1);
                FarPoint.Web.Spread.CheckBoxCellType chtbox1 = new FarPoint.Web.Spread.CheckBoxCellType();
                fpcammarkstaff.Sheets[0].Cells[0, 1].CellType = chtbox1;
                chtbox1.AutoPostBack = true;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    fpcammarkstaff.Sheets[0].RowCount++;
                    cn++;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 0].Text = cn.ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 13].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 14].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 14].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 15].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 15].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 16].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 16].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 17].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 17].Font.Size = FontUnit.Medium;
                    FarPoint.Web.Spread.CheckBoxCellType chtbox = new FarPoint.Web.Spread.CheckBoxCellType();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 1].CellType = chtbox;
                    fpcammarkstaff.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    FarPoint.Web.Spread.ButtonCellType buttype = new FarPoint.Web.Spread.ButtonCellType();
                    buttype.Text = "Update";
                    // buttype.CommandName = "fpcammarkstaff_ButtonCommand";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 2].CellType = buttype;
                    fpcammarkstaff.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    chtbox.AutoPostBack = false;
                    string external = Convert.ToString(ds.Tables[0].Rows[i]["isexternal"]);
                    if (external.ToString() == "True")
                    {
                        external = "External";
                    }
                    else
                    {
                        external = "Internal";
                    }
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 3].Text = external;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["staffname"]);
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["staffcode"]);
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["department"]);
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["design"]);
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["yearofexp"]);
                    string setting = Convert.ToString(ds.Tables[0].Rows[i]["setting"]);
                    if (setting.ToString() == "True")
                    {
                        setting = "Y";
                    }
                    else
                    {
                        setting = "N";
                    }
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 8].Text = setting;
                    string Inivigition = Convert.ToString(ds.Tables[0].Rows[i]["Inivigition"]);
                    if (Inivigition.ToString() == "True")
                    {
                        Inivigition = "Y";
                    }
                    else if (Inivigition.ToString() == "False")
                    {
                        Inivigition = "N";
                    }
                    else
                    {
                        Inivigition = "";
                    }
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 10].Text = Inivigition;
                    string Valuation = Convert.ToString(ds.Tables[0].Rows[i]["Valuation"]);
                    if (Valuation.ToString() == "True")
                    {
                        Valuation = "Y";
                    }
                    else
                    {
                        Valuation = "N";
                    }

                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 9].Text = Valuation;
                    string scarb = Convert.ToString(ds.Tables[0].Rows[i]["Scrab"]);
                    if (scarb.ToString() == "True")
                    {
                        scarb = "Y";
                    }
                    else if (scarb.ToString() == "False")
                    {
                        scarb = "N";
                    }
                    else
                    {
                        scarb = "";
                    }
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 11].Text = scarb;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(ds.Tables[0].Rows[i]["sectio"]);
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(ds.Tables[0].Rows[i]["collname"]);
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(ds.Tables[0].Rows[i]["per_mobileno"]);
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 15].Text = Convert.ToString(ds.Tables[0].Rows[i]["email"]);
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 16].Text = Convert.ToString(ds.Tables[0].Rows[i]["per_phone"]);
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(ds.Tables[0].Rows[i]["city"]);
                }
                fpcammarkstaff.Sheets[0].PageSize = fpcammarkstaff.Sheets[0].RowCount;
                fpcammarkstaff.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fpcammarkstaff.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                fpcammarkstaff.Sheets[0].Columns[0].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[3].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[4].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[5].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[6].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[7].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[8].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[9].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[10].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[11].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[12].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[13].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[14].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[15].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[16].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[17].Locked = true;
            }
            else
            {
                lblerrormsg.Text = "No Records Found";
                fpcammarkstaff.Visible = false;
                lblerrormsg.Visible = true;
                lblmessage1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                txtmessage.Visible = false;
                btnsms.Visible = false;
                btnprintmaster.Visible = false;
                btndelete.Visible = false;
                labpurpose.Visible = false;
                ddlpurpose.Visible = false;
                fpspreadpurpose.Visible = false;
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            txttravelallowance.Text = string.Empty;
            txtdailyallowance.Text = string.Empty;
            if (chkboxsms.Checked == true)
            {
                txtmessage.Visible = true;
                btnsms.Visible = true;

            }
            else if (chkboxmail.Checked == true)
            {
                txtmessage.Visible = true;
                btnsms.Visible = true;
            }
            spread();
            //  btnnew_click(sender, e);
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btnstaff_Click(object sender, EventArgs e)
    {
        try
        {
            panel8.Visible = true;
            fsstaff.Visible = true;
            btnstaffadd.Text = "Ok";
            fsstaff.Sheets[0].RowCount = 0;
            BindCollege();
            loadstaffdep();
            bind_stafType();
            bind_design();
            stafftypecateg();
            loadfsstaff();
            Btnedit.Focus();

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    public void BindCollege()
    {
        try
        {
            ds.Clear();
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
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
            ht.Clear();
            ht.Add("column_field", columnfield.ToString());
            ds = da.select_method("bind_college", ht, "sp");
            ddlcollege.Items.Clear();
            ddlinstition.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();

                ddlinstition.DataSource = ds;
                ddlinstition.DataTextField = "collname";
                ddlinstition.DataValueField = "college_code";
                ddlinstition.DataBind();
            }

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void loadstaffdep()
    {
        try
        {
            ddldepratstaff.Items.Clear();
            ddldept11.Items.Clear();
            ds.Clear();
            string college = "";
            if (checktype.Text == "Internal")
            {
                college = collegecode;
            }
            else
            {
                college = ddlcollege.SelectedValue;
            }
            ds = da.loaddepartment(college);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldepratstaff.DataSource = ds;
                ddldepratstaff.DataTextField = "dept_name";
                ddldepratstaff.DataValueField = "dept_code";
                ddldepratstaff.DataBind();
                ddldepratstaff.Items.Insert(0, "All");



                ddldept11.DataSource = ds;
                ddldept11.DataTextField = "dept_name";
                ddldept11.DataValueField = "Dept_Code";
                ddldept11.DataBind();
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bind_stafType()
    {
        try
        {
            ds.Clear();
            string college = "";
            college = ddlcollege.SelectedValue;
            ds = da.loadstafftype(college);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_stftype.DataSource = ds;
                ddl_stftype.DataTextField = "StfType";
                ddl_stftype.DataValueField = "StfType";
                ddl_stftype.DataBind();
                ddl_stftype.Items.Insert(0, "All");
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlstreamadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            subjectload();
            txtsettingtextbox.Text = "--Select--";
            txtvalutationtextbox.Text = "--Select--";
            Btnedit.Focus();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlstreamview_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Btnedit.Focus();

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bind_design()
    {
        try
        {
            ddldept1.Items.Clear();
            ddl_design.Items.Clear();
            ds1.Clear();
            ds1.Reset();
            ds1.Dispose();
            string college = "";
            if (checktype.Text == "Internal")
            {
                college = collegecode;
            }
            else
            {
                college = ddlcollege.SelectedValue;
            }
            ds1 = da.loaddesignation(college);
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddl_design.DataSource = ds1;
                ddl_design.DataTextField = "Desig_Name";
                ddl_design.DataValueField = "Desig_Name";
                ddl_design.DataBind();
                ddl_design.Items.Insert(0, "All");

                ddldept1.DataSource = ds1;
                ddldept1.DataTextField = "Desig_Name";
                ddldept1.DataValueField = "desig_code";
                ddldept1.DataBind();

            }
        }
        catch
        {
        }
    }

    public void stafftypecateg()
    {
        try
        {
            ddl_design.Items.Clear();
            ddl_design.Items.Clear();
            ds1.Clear();
            ds1.Reset();
            ds1.Dispose();
            string college = "";
            if (checktype.Text == "Internal")
            {
                college = collegecode;
            }
            else
            {
                college = ddlcollege.SelectedValue;
            }
            string addstera = "select category_code,category_name from staffcategorizer";
            ds1 = da.select_method_wo_parameter(addstera, "text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_design.DataSource = ds1;
                ddl_design.DataTextField = "category_name";
                ddl_design.DataValueField = "category_code";
                ddl_design.DataBind();
                ddl_design.Items.Insert(0, "All");
            }
        }
        catch
        {
        }
    }

    public void staffsteram()
    {
        try
        {
            ddldept1.Items.Clear();
            ddl_design.Items.Clear();
            ds1.Clear();
            ds1.Reset();
            ds1.Dispose();
            string college = "";
            if (checktype.Text == "Internal")
            {
                college = collegecode;
            }
            else
            {
                college = ddlcollege.SelectedValue;
            }
            string addstera = "select distinct stream from staffmaster where college_code='13' and stream is not null";
            ds1 = da.select_method_wo_parameter(addstera, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstaffstream.DataSource = ds1;
                ddlstaffstream.DataTextField = "stream";
                ddlstaffstream.DataValueField = "stream";
                ddlstaffstream.DataBind();
                ddlstaffstream.Items.Insert(0, "All");
            }
        }
        catch
        {
        }
    }

    public void loadfsstaff()
    {
        try
        {
            string sql = "";
            if (ddldepratstaff.SelectedIndex != 0)
            {
                if (txt_search.Text != "")
                {
                    if (ddlstaff.SelectedIndex == 0)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0)and  (staffmaster.settled = 0)  and (staff_name like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                    }
                    else if (ddlstaff.SelectedIndex == 1)
                    {
                        sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                    }
                }
                else
                {

                    sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";

                }
            }
            else if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code)";
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code)";
                }
                else if (ddlcollege.SelectedIndex != -1)
                {
                    sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
                }

                else
                {
                    sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster,hrdept_master.dept_name where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0";

                }
            }
            else
                if (ddldepratstaff.SelectedValue.ToString() == "All")
                {
                    sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";

                }
            fsstaff.Sheets[0].ColumnCount = 0;
            fsstaff.Sheets[0].ColumnCount = 4;
            fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
            fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Code";
            fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Name";
            fsstaff.Sheets[0].ColumnHeader.Columns[3].Label = "Select";

            fsstaff.Sheets[0].Columns[0].Width = 80;
            fsstaff.Sheets[0].Columns[1].Width = 200;
            fsstaff.Sheets[0].Columns[2].Width = 400;
            fsstaff.Sheets[0].Columns[3].Width = 50;

            fsstaff.Sheets[0].Columns[0].Locked = true;
            fsstaff.Sheets[0].Columns[1].Locked = true;
            fsstaff.Sheets[0].Columns[2].Locked = true;

            fsstaff.Sheets[0].RowCount = 0;
            fsstaff.SaveChanges();

            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();


            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();


            fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
            chkcell.AutoPostBack = true;
            fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].CellType = chkcell;
            fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
            fsstaff.Sheets[0].SpanModel.Add(fsstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            string bindspread = sql;

            string design_name = string.Empty;
            string dept_all = string.Empty;
            string design_all = string.Empty;

            if (ddl_design.Items.Count > 0)
            {
                design_name = ddl_design.SelectedItem.ToString();

            }

            for (int cnt = 1; cnt < ddldepratstaff.Items.Count; cnt++)
            {
                if (dept_all == "")
                {
                    dept_all = ddldepratstaff.Items[cnt].Value;
                }
                else
                {
                    dept_all = dept_all + "','" + ddldepratstaff.Items[cnt].Value;
                }

            }

            for (int cnt = 1; cnt < ddl_design.Items.Count; cnt++)
            {
                if (dept_all == "")
                {
                    design_all = ddl_design.Items[cnt].Value;
                }
                else
                {
                    design_all = design_all + "','" + ddl_design.Items[cnt].Value;
                }
            }

            //string Sql_Query = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and h.dept_code = '" + ddldepratstaff.SelectedValue.ToString() + "' and d.desig_name='" + design_name + "' and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";

            //if (ddldepratstaff.SelectedItem.ToString() == "All" && ddl_design.SelectedItem.ToString() == "All")
            //{
            //    Sql_Query = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code  and h.dept_code in ('" + dept_all + "') and d.desig_name in ('" + design_all + "') and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";
            //}
            //else if (ddldepratstaff.SelectedItem.ToString() == "All")
            //{
            //    Sql_Query = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code in ('" + dept_all + "') and d.desig_name='" + design_name + "' and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";
            //}
            //else if (ddl_design.SelectedItem.ToString() == "All")
            //{

            //    Sql_Query = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code = '" + ddldepratstaff.SelectedValue.ToString() + "' and d.desig_name in ('" + design_all + "') and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";
            //}

            //if (ddl_stftype.SelectedItem.ToString() != "All")
            //{
            //    Sql_Query = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code = '" + ddldepratstaff.SelectedValue.ToString() + "' and d.desig_name in ('" + design_all + "') and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and stftype = '" + ddl_stftype.SelectedItem.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";
            //}
            string dept = "";
            if (ddldepratstaff.Text != "All" && ddldepratstaff.Text.Trim() != "")
            {
                dept = " and st.dept_code='" + ddldepratstaff.SelectedValue.ToString() + "' ";
            }

            string type = "";
            if (ddl_stftype.Text != "All" && ddl_stftype.Text.Trim() != "")
            {
                type = " and st.stftype='" + ddl_stftype.SelectedValue.ToString() + "' ";
            }

            string design = "";
            if (ddl_design.Text != "All" && ddl_design.Text.Trim() != "")
            {
                design = " and st.category_code='" + ddl_design.SelectedValue.ToString() + "' ";
            }

            string stram = "";
            if (ddlstaffstream.Text != "All" && ddlstaffstream.Text.Trim() != "")
            {
                stram = " and s.Stream='" + ddlstaffstream.SelectedValue.ToString() + "' ";
            }

            string Sql_Query = "select distinct s.staff_name,st.staff_code from staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and (st.latestrec <> 0) AND (s.resign = 0) and (s.settled = 0) and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' " + dept + " " + type + " " + design + " " + stram + "";


            DataSet dsbindspread = new DataSet();
            dsbindspread.Clear();
            dsbindspread = da.select_method_wo_parameter(Sql_Query, "Text");
            panel8.Visible = true;

            if (dsbindspread.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();


                    fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
                    fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].CellType = txt;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = code;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = name;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].CellType = chkcell1;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    fsstaff.Sheets[0].AutoPostBack = false;
                }
                int rowcount = fsstaff.Sheets[0].RowCount;

                fsstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                fsstaff.Width = 750;
                fsstaff.SaveChanges();
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void fsstaff_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string activerow = fsstaff.ActiveSheetView.ActiveRow.ToString();
            string activecol = fsstaff.ActiveSheetView.ActiveColumn.ToString();
            Cellclick = true;
            panel8.Visible = true;
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkinivigilation.Checked == false)
            {
                fsstaff.SaveChanges();
                for (int i = 1; i < fsstaff.Sheets[0].RowCount; i++)
                {
                    int s = Convert.ToInt32(fsstaff.Sheets[0].Cells[i, 3].Value);
                    if (s == 1)
                    {
                        ddlempno1.Text = "";
                        name_active = fsstaff.Sheets[0].Cells[Convert.ToInt32(i), 2].Text;
                        des_active = fsstaff.Sheets[0].Cells[Convert.ToInt32(i), 1].Text;
                        txtissueper.Text = name_active.ToString();
                        txtstaff_co.Text = fsstaff.Sheets[0].Cells[Convert.ToInt32(i), 1].Text;
                        bind();
                        s = fsstaff.Sheets[0].RowCount;
                    }
                }
                panel8.Visible = false;
            }
            else
            {
                string collcode = ddlcollege.SelectedValue.ToString();
                string Stream = ddlstaffstream.SelectedItem.Text;
                if (Stream.Trim() != "")
                {
                    if (Stream.Trim() == "Management")
                    {
                        Stream = "Day";
                    }
                }

                Boolean saveflag = false;
                fsstaff.SaveChanges();
                for (int i = 1; i < fsstaff.Sheets[0].RowCount; i++)
                {
                    int s = Convert.ToInt32(fsstaff.Sheets[0].Cells[i, 3].Value);
                    if (s == 1)
                    {
                        saveflag = true;
                        string staffcode = fsstaff.Sheets[0].Cells[i, 1].Text.ToString();
                        string staffname = fsstaff.Sheets[0].Cells[i, 2].Text.ToString();

                        string addstaffquery = "if not exists (select * from examstaffmaster where staff_code='" + staffcode + "' and Type='" + Stream + "') insert into examstaffmaster(staff_code,isexternal,setting,Valuation,Inivigition,sectio,yearofexp,scrab,type) values ('" + staffcode + "',0,0,0,1,'F.N/A.N',0,0,'" + Stream + "') else update examstaffmaster set Inivigition='1' where staff_code='" + staffcode + "' and Type='" + Stream + "'";
                        int savupdate = da.update_method_wo_parameter(addstaffquery, "text");
                    }
                }
                if (saveflag == false)
                {
                    lblerrormsg.Text = "Please Select Staff And Then Procee";
                    lblerrormsg.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Saved Successfully')", true);
                }
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btnexitpop_Click(object sender, EventArgs e)
    {
        try
        {
            panel8.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fsstaff.Sheets[0].RowCount = 0;
            loadfsstaff();
            panel8.Visible = true;

        }
        catch
        {
        }
    }

    protected void ddl_stftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fsstaff.Sheets[0].RowCount = 0;
            bind_design();
            loadfsstaff();
            panel8.Visible = true;
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddl_design_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fsstaff.Sheets[0].RowCount = 0;
            loadfsstaff();
            panel8.Visible = true;
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void ddlstaffstream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fsstaff.Sheets[0].RowCount = 0;
            loadfsstaff();
            panel8.Visible = true;
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btncityplus_Click(object sender, EventArgs e)
    {
        try
        {
            pnlcity.Visible = true;
            txt_city.Text = "";
            capcity.InnerHtml = "City";
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btncitymins_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlextcity.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string reason = ddlextcity.SelectedItem.ToString();
                string reason1 = ddlextcity.SelectedValue.ToString();
                if (reason != "---Select---")
                {
                    if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                    {
                        string strquery = "if exists(select * from external_staff where pcity_code='" + reason1 + "')select * from external_staff where pcity_code='" + reason1 + "'";
                        int d = da.update_method_wo_parameter(strquery, "Text");
                        if (d == -1)
                        {

                            lblerror1.Text = "Can't Be Deleted";
                            lblerror1.Visible = true;


                        }
                        city();
                    }
                    //  lblerror.Visible = false;
                }
                else
                {
                    lblerror1.Text = "Select City Then Delete";
                    lblerror1.Visible = true;
                }
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btnadd1_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            ArrayList testarray = new ArrayList();
            ht.Clear();
            if (txt_dept.Text != "")
            {
                if (ddlexdept.Items.Count == 0)
                {

                    string value = txt_dept.Text;
                    string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + value + "','exdep','" + collegecode + "')";
                    int a = da.insert_method(strquery, ht, "Text");
                    txt_dept.Text = "";
                    department();
                }
                else
                {
                    if (ddlexdept.Items.Count > 0)
                    {
                        for (int i = 0; i < ddlexdept.Items.Count; i++)
                        {
                            testarray.Add(ddlexdept.Items[i].ToString());
                        }
                        string typevalue = txt_dept.Text;
                        if (testarray.Contains(typevalue) == false)
                        {
                            string value = txt_dept.Text;
                            string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + value + "','exdep','" + collegecode + "')";
                            int a = da.insert_method(strquery, ht, "Text");
                            txt_dept.Text = "";
                            department();
                        }
                        else
                        {
                            paneldept.Visible = true;
                        }

                    }
                }
            }
            else
            {
                paneldept.Visible = true;
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btndesignplus_Click(object sender, EventArgs e)
    {
        try
        {
            panel1_sedign.Visible = true;
            txt_design.Text = "";

            Capdegina.InnerHtml = "Designation";
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btndesignmins_Click(object sender, EventArgs e)
    {

        try
        {
            if (ddlexterdesign.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string reason = ddlexterdesign.SelectedItem.ToString();
                string reason1 = ddlexterdesign.SelectedValue.ToString();
                if (reason != "---Select---")
                {
                    if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                    {
                        string strquery = "if exists(select * from external_staff where design_code='" + reason1 + "')select * from external_staff where design_code='" + reason1 + "' else delete textvaltable where TextVal='" + reason + "' and TextCriteria='exdsi' and college_code='" + collegecode + "'";
                        int d = da.update_method_wo_parameter(strquery, "Text");
                        if (d == -1)
                        {
                            lblerror1.Text = "Can't Be Deleted";
                            lblerror1.Visible = true;


                        }
                        designation();
                    }
                    //lblerror1.Visible = false;
                }
                else
                {
                    lblerror1.Text = "Select Designation Then Delete";
                    lblerror1.Visible = true;
                }
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btnplus_Click(object sender, EventArgs e)
    {
        try
        {
            paneldept.Visible = true;
            txt_dept.Text = "";

            capdepart.InnerHtml = "Department";
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btnmins_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlexdept.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string reason = ddlexdept.SelectedItem.ToString();
                string reason1 = ddlexdept.SelectedValue.ToString();
                if (reason != "---Select---")
                {
                    if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                    {
                        string strquery = "if exists(select * from external_staff where dept_code='" + reason1 + "')select * from external_staff where dept_code='" + reason1 + "' else delete textvaltable where TextVal='" + reason + "' and TextCriteria='exdep' and college_code='" + collegecode + "'";
                        int d = da.update_method_wo_parameter(strquery, "Text");
                        if (d == -1)
                        {

                            lblerror1.Text = "Can't Be Deleted";
                            lblerror1.Visible = true;



                        }
                        department();
                    }

                }
                else
                {
                    lblerror1.Text = "Select Department Then Delete";
                    lblerror1.Visible = true;
                }
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btnuniplus_Click(object sender, EventArgs e)
    {
        try
        {
            pan_instition.Visible = true;
            txt_instition.Text = "";
            Capins.Text = "Institution";
            Text_km.Text = "";


        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btnunimins_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlextuniv.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string reason = ddlextuniv.SelectedItem.ToString();
                string reason1 = ddlextuniv.SelectedValue.ToString();
                if (reason != "---Select---")
                {
                    if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                    {
                        string strquery = "if exists(select * from external_staff where coll_code='" + reason1 + "')select * from external_staff where coll_code='" + reason1 + "' else delete textvaltable where TextVal='" + reason + "' and TextCriteria='exins' and college_code='" + collegecode + "'";
                        int d = da.update_method_wo_parameter(strquery, "Text");
                        if (d == -1)
                        {

                            lblerror1.Text = "Can't Be Deleted";
                            lblerror1.Visible = true;



                        }
                        instition();
                    }
                    lblerror1.Visible = false;
                }
                else
                {
                    lblerror.Text = "Select Institution Then Delete";
                    lblerror.Visible = true;
                }
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }



    }

    protected void btnexit1_Click(object sender, EventArgs e)
    {
        try
        {

            paneldept.Visible = false;
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btn_designadd_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            ArrayList testarray = new ArrayList();
            if (txt_design.Text != "")
            {
                if (ddlexterdesign.Items.Count == 0)
                {

                    string value = txt_design.Text;
                    string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + value + "','exdsi','" + collegecode + "')";
                    int a = da.insert_method(strquery, ht, "Text");
                    txt_design.Text = "";
                    department();
                }
                else
                {
                    if (ddlexterdesign.Items.Count > 0)
                    {
                        for (int i = 0; i < ddlexterdesign.Items.Count; i++)
                        {
                            testarray.Add(ddlexterdesign.Items[i].ToString());
                        }
                        string typevalue = txt_design.Text;
                        if (testarray.Contains(typevalue) == false)
                        {
                            string value = txt_design.Text;
                            string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + value + "','exdsi','" + collegecode + "')";
                            int a = da.insert_method(strquery, ht, "Text");
                            txt_design.Text = "";
                            designation();
                        }
                        else
                        {
                            panel1_sedign.Visible = true;
                        }

                    }
                }
            }
            else
            {
                panel1_sedign.Visible = true;
            }

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btn_designexit_Click(object sender, EventArgs e)
    {
        try
        {
            panel1_sedign.Visible = false;

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btn_cityadd_Click(object sender, EventArgs e)
    {
        try
        {

            ht.Clear();
            ArrayList testarray = new ArrayList();
            if (txt_city.Text != "")
            {
                if (ddlextcity.Items.Count == 0)
                {

                    string value = txt_city.Text;
                    string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + value + "','city','" + collegecode + "')";
                    int a = da.insert_method(strquery, ht, "Text");
                    txt_city.Text = "";
                    city();
                }
                else
                {
                    if (ddlextcity.Items.Count > 0)
                    {
                        for (int i = 0; i < ddlextcity.Items.Count; i++)
                        {
                            testarray.Add(ddlextcity.Items[i].ToString());
                        }
                        string typevalue = txt_city.Text;
                        if (testarray.Contains(typevalue) == false)
                        {
                            string value = txt_city.Text;
                            string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + value + "','city','" + collegecode + "')";
                            int a = da.insert_method(strquery, ht, "Text");
                            txt_city.Text = "";
                            city();
                        }
                        else
                        {
                            pnlcity.Visible = true;
                        }
                    }
                }
            }
            else
            {
                pnlcity.Visible = true;
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btn_cityexit_Click(object sender, EventArgs e)
    {
        try
        {
            pnlcity.Visible = false;

        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_institionadd_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            ArrayList testarray = new ArrayList();
            if (txt_instition.Text != "" && Text_km.Text != "")
            {
                if (ddlextuniv.Items.Count == 0)
                {

                    string value = txt_instition.Text;
                    string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code,Institution_km) values('" + value + "','exins','" + collegecode + "','" + Text_km.Text + "')";
                    int a = da.insert_method(strquery, ht, "Text");
                    txt_instition.Text = "";
                    Text_km.Text = "";
                    instition();
                }
                else
                {
                    if (ddlextuniv.Items.Count > 0)
                    {
                        for (int i = 0; i < ddlextuniv.Items.Count; i++)
                        {
                            testarray.Add(ddlextuniv.Items[i].ToString());
                        }
                        string typevalue = txt_instition.Text;
                        if (testarray.Contains(typevalue) == false)
                        {
                            string value = txt_instition.Text;
                            string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code,Institution_km) values('" + value + "','exins','" + collegecode + "','" + Text_km.Text + "')";
                            int a = da.insert_method(strquery, ht, "Text");
                            txt_instition.Text = "";
                            Text_km.Text = "";
                            instition();
                        }
                        else
                        {
                            pan_instition.Visible = true;
                        }

                    }
                }
            }
            else
            {
                pan_instition.Visible = true;
            }

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btn_institionexit_Click(object sender, EventArgs e)
    {
        try
        {
            pan_instition.Visible = false;

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void department()
    {
        try
        {
            ddlexdept.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string query = "select TextCode ,Textval  from textvaltable where TextCriteria='exdep' and college_code=" + collegecode + "";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlexdept.DataSource = ds;
                ddlexdept.DataTextField = "Textval";
                ddlexdept.DataValueField = "TextCode";
                ddlexdept.DataBind();
            }
            if (checktype.Items[0].Selected == true && checktype.Items[1].Selected == true)
            {
                string strquery = "";

                ddldept11.Items.Clear();
                ds.Clear();
                string singleuser = Session["single_user"].ToString();
                if (singleuser == "True")
                {
                    strquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "')  union select TextCode as dept_code,Textval as dept_name from textvaltable where TextCriteria='exdep' and college_code=" + collegecode + "";
                }
                else
                {
                    groupuser = Session["group_code"].ToString();
                    if (groupuser.Contains(';'))
                    {
                        string[] group_semi = groupuser.Split(';');
                        groupuser = group_semi[0].ToString();
                    }
                    strquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + groupuser + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "')  union select TextCode as dept_code,Textval as dept_name from textvaltable where TextCriteria='exdep' and college_code=" + collegecode + "";
                }
                if (strquery != "")
                {
                    ds = da.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddldept11.DataSource = ds;
                        ddldept11.DataTextField = "dept_name";
                        ddldept11.DataValueField = "Dept_Code";
                        ddldept11.DataBind();
                    }
                }
            }
            else if (checktype.Text == "Internal")
            {
                loadstaffdep();
            }
            else if (checktype.Text == "External")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldept11.DataSource = ds;
                    ddldept11.DataTextField = "Textval";
                    ddldept11.DataValueField = "TextCode";
                    ddldept11.DataBind();
                }

            }


            ddlexdept.Items.Insert(0, "---Select---");
            paneldept.Visible = false;
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void designation()
    {
        try
        {
            ddlexterdesign.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string query = "select TextCode,Textval from textvaltable where TextCriteria='exdsi' and college_code=" + collegecode + "";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlexterdesign.DataSource = ds;
                ddlexterdesign.DataTextField = "Textval";
                ddlexterdesign.DataValueField = "TextCode";
                ddlexterdesign.DataBind();
            }
            if (checktype.Items[0].Selected == true && checktype.Items[1].Selected == true)
            {
                string sqlquery = "select convert(varchar(100),TextCode) as TextCode ,Textval  from textvaltable where TextCriteria='exdsi' and college_code=" + collegecode + " union SELECT distinct t.desig_code as TextCode ,Desig_Name as Textval FROM StaffTrans T,staffmaster m,Desig_Master G WHERE t.staff_code = m.staff_code and T.Desig_Code = G.Desig_Code AND Latestrec = 1 and G.collegecode=" + collegecode + "";
                ds = da.select_method_wo_parameter(sqlquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldept1.DataSource = ds;
                    ddldept1.DataTextField = "Textval";
                    ddldept1.DataValueField = "TextCode";
                    ddldept1.DataBind();
                }

            }
            else if (checktype.Text == "Internal")
            {
                bind_design();
            }
            else if (checktype.Text == "External")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldept1.DataSource = ds;
                    ddldept1.DataTextField = "Textval";
                    ddldept1.DataValueField = "TextCode";
                    ddldept1.DataBind();
                }

            }


            ddlexterdesign.Items.Insert(0, "---Select---");
            panel1_sedign.Visible = false;
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void city()
    {
        try
        {
            ddlextcity.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string query = "select TextCode,Textval from textvaltable where TextCriteria='city' and college_code=" + collegecode + "";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlextcity.DataSource = ds;
                ddlextcity.DataTextField = "Textval";
                ddlextcity.DataValueField = "TextCode";
                ddlextcity.DataBind();

                ddlcity.DataSource = ds;
                ddlcity.DataTextField = "Textval";
                ddlcity.DataValueField = "TextCode";
                ddlcity.DataBind();
            }

            ddlextcity.Items.Insert(0, "---Select---");

            pnlcity.Visible = false;
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void instition()
    {
        try
        {
            ddlextuniv.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string query = "select TextCode,Textval from textvaltable where TextCriteria='exins' and college_code=" + collegecode + "";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlextuniv.DataSource = ds;
                ddlextuniv.DataTextField = "Textval";
                ddlextuniv.DataValueField = "TextCode";
                ddlextuniv.DataBind();
            }
            if (checktype.Items[0].Selected == true && checktype.Items[1].Selected == true)
            {
                string sqlquery = "select Textval,TextCode  from textvaltable where TextCriteria='exins' and college_code=" + collegecode + " union select collname as Textval,college_code as TextCode from collinfo";
                ds = da.select_method_wo_parameter(sqlquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlinstition.DataSource = ds;
                    ddlinstition.DataTextField = "Textval";
                    ddlinstition.DataValueField = "TextCode";
                    ddlinstition.DataBind();
                }

            }
            else if (checktype.Text == "Internal")
            {
                BindCollege();
            }
            else if (checktype.Text == "External")
            {
                ddlinstition.DataSource = ds;
                ddlinstition.DataTextField = "Textval";
                ddlinstition.DataValueField = "TextCode";
                ddlinstition.DataBind();
            }

            ddlextuniv.Items.Insert(0, "---Select---");
            pan_instition.Visible = false;
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chktype_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            txttype.Text = "--Select--";
            if (chktype.Checked == true)
            {
                for (int i = 0; i < checktype.Items.Count; i++)
                {
                    checktype.Items[i].Selected = true;
                    txttype.Text = "Type(" + (checktype.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < checktype.Items.Count; i++)
                {
                    checktype.Items[i].Selected = false;
                    txttype.Text = "--Select--";
                }
            }
            checkedtype();
            designation();
            department();
            city();
            instition();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void chk_examtpe_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            txtexaminer.Text = "--Select--";
            if (chk_examtpe.Checked == true)
            {

                for (int i = 0; i < check_examtype.Items.Count; i++)
                {
                    check_examtype.Items[i].Selected = true;
                    txtexaminer.Text = "Examiner Type(" + (check_examtype.Items.Count) + ")";
                }


            }
            else
            {
                for (int i = 0; i < check_examtype.Items.Count; i++)
                {
                    check_examtype.Items[i].Selected = false;
                    txtexaminer.Text = "--Select--";
                }
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void check_examtype_selectedchanged(object sender, EventArgs e)
    {

        int ddlcount = 0;
        try
        {
            txtexaminer.Text = "--Select--";

            string value = "";
            string code = "";
            for (int i = 0; i < check_examtype.Items.Count; i++)
            {

                if (check_examtype.Items[i].Selected == true)
                {

                    value = check_examtype.Items[i].Text;
                    code = check_examtype.Items[i].Value.ToString();
                    ddlcount = ddlcount + 1;
                    txtexaminer.Text = "Examiner Type(" + ddlcount.ToString() + ")";
                }
            }
            if (ddlcount == 0)
                txtexaminer.Text = "---Select---";

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void chkvalution_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            txtvalutationtextbox.Text = "--Select--";
            if (chkvalution.Checked == true)
            {

                for (int i = 0; i < ddlvalution.Items.Count; i++)
                {
                    ddlvalution.Items[i].Selected = true;
                    txtvalutationtextbox.Text = "Subject(" + (ddlvalution.Items.Count) + ")";
                }


            }
            else
            {
                for (int i = 0; i < ddlvalution.Items.Count; i++)
                {
                    ddlvalution.Items[i].Selected = false;
                    txtvalutationtextbox.Text = "--Select--";
                }
            }
            btnfoucs.Focus();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void checkvalution_selectedchanged(object sender, EventArgs e)
    {

        int ddlcount = 0;
        try
        {
            txtvalutationtextbox.Text = "--Select--";

            string value = "";
            string code = "";
            for (int i = 0; i < ddlvalution.Items.Count; i++)
            {

                if (ddlvalution.Items[i].Selected == true)
                {

                    value = ddlvalution.Items[i].Text;
                    code = ddlvalution.Items[i].Value.ToString();
                    ddlcount = ddlcount + 1;
                    txtvalutationtextbox.Text = "Subject(" + ddlcount.ToString() + ")";
                }
            }
            if (ddlcount == 0)
                txtvalutationtextbox.Text = "---Select---";
            btnfoucs.Focus();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void chksetting_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            txtsettingtextbox.Text = "--Select--";
            if (chksetting.Checked == true)
            {

                for (int i = 0; i < ddlsetting.Items.Count; i++)
                {
                    ddlsetting.Items[i].Selected = true;
                    txtsettingtextbox.Text = "Subject(" + (ddlsetting.Items.Count) + ")";
                }


            }
            else
            {
                for (int i = 0; i < ddlsetting.Items.Count; i++)
                {
                    ddlsetting.Items[i].Selected = false;
                    txtsettingtextbox.Text = "--Select--";
                }
            }
            btnfoucs.Focus();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void checksetting_selectedchanged(object sender, EventArgs e)
    {

        int ddlcount = 0;
        try
        {
            txtsettingtextbox.Text = "--Select--";

            string value = "";
            string code = "";
            for (int i = 0; i < ddlsetting.Items.Count; i++)
            {

                if (ddlsetting.Items[i].Selected == true)
                {

                    value = ddlsetting.Items[i].Text;
                    code = ddlsetting.Items[i].Value.ToString();
                    ddlcount = ddlcount + 1;
                    txtsettingtextbox.Text = "Subject(" + ddlcount.ToString() + ")";
                }
            }
            if (ddlcount == 0)
                txtsettingtextbox.Text = "---Select---";
            btnfoucs.Focus();

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    public void checkedtype()
    {
        if (checktype.Items[0].Selected == true && checktype.Items[1].Selected == true)
        {
            check_examtype.Items.Clear();
            check_examtype.Items.Insert(0, "Setting");
            check_examtype.Items.Insert(1, "Valuation");
            check_examtype.Items.Insert(2, "Invigilation");
            check_examtype.Items.Insert(3, "Scraps");
            check_examtype.Items[0].Value = "1";
            check_examtype.Items[1].Value = "2";
            check_examtype.Items[2].Value = "3";
            check_examtype.Items[3].Value = "4";
        }
        else if (checktype.Text == "Internal")
        {
            check_examtype.Items.Clear();
            check_examtype.Items.Insert(0, "Setting");
            check_examtype.Items.Insert(1, "Valuation");
            check_examtype.Items.Insert(2, "Invigilation");
            check_examtype.Items.Insert(3, "Scraps");
            check_examtype.Items[0].Value = "1";
            check_examtype.Items[1].Value = "2";
            check_examtype.Items[2].Value = "3";
            check_examtype.Items[3].Value = "4";
        }
        else if (checktype.Text == "External")
        {
            check_examtype.Items.Clear();
            check_examtype.Items.Insert(0, "Setting");
            check_examtype.Items.Insert(1, "Valuation");
            check_examtype.Items[0].Value = "1";
            check_examtype.Items[1].Value = "2";
        }
    }

    protected void checktype_selectedchanged(object sender, EventArgs e)
    {
        int ddlcount = 0;
        try
        {
            txttravelallowance.Text = string.Empty;
            txtdailyallowance.Text = string.Empty;

            txttype.Text = "--Select--";
            string value = "";
            string code = "";
            for (int i = 0; i < checktype.Items.Count; i++)
            {
                if (checktype.Items[i].Selected == true)
                {

                    value = checktype.Items[i].Text;
                    code = checktype.Items[i].Value.ToString();
                    ddlcount = ddlcount + 1;
                    txttype.Text = "Type(" + ddlcount.ToString() + ")";
                }
            }
            if (ddlcount == 0)
                txttype.Text = "---Select---";

            checkedtype();
            designation();
            department();
            city();
            instition();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void ddldept11_selectedchanged(object sender, EventArgs e)
    {
        int ddlcount = 0;
        try
        {
            txt_deptview.Text = "--Select--";
            string value = "";
            string code = "";
            for (int i = 0; i < ddldept11.Items.Count; i++)
            {
                if (ddldept11.Items[i].Selected == true)
                {

                    value = ddldept11.Items[i].Text;
                    code = ddldept11.Items[i].Value.ToString();
                    ddlcount = ddlcount + 1;
                    txt_deptview.Text = "Depart(" + ddlcount.ToString() + ")";
                }

            }
            if (ddlcount == 0)
                txt_deptview.Text = "---Select---";
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void Checkdept_checkedchanged(object sender, EventArgs e)
    {
        txt_deptview.Text = "--Select--";
        if (Checkdept.Checked == true)
        {

            for (int i = 0; i < ddldept11.Items.Count; i++)
            {
                ddldept11.Items[i].Selected = true;
                txt_deptview.Text = "Depart(" + (ddldept11.Items.Count) + ")";
            }


        }
        else
        {
            for (int i = 0; i < ddldept11.Items.Count; i++)
            {
                ddldept11.Items[i].Selected = false;
                txt_deptview.Text = "--Select--";
            }
        }

    }

    protected void ddldept1_selectedchanged(object sender, EventArgs e)
    {
        int ddlcount = 0;
        try
        {
            txt_designview.Text = "--Select--";

            string value = "";
            string code = "";
            for (int i = 0; i < ddldept1.Items.Count; i++)
            {
                if (ddldept1.Items[i].Selected == true)
                {

                    value = ddldept1.Items[i].Text;
                    code = ddldept1.Items[i].Value.ToString();
                    ddlcount = ddlcount + 1;
                    txt_designview.Text = "Design(" + ddlcount.ToString() + ")";
                }
            }
            if (ddlcount == 0)
                txt_designview.Text = "---Select---";
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void Checkdesign_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            txt_designview.Text = "--Select--";
            if (Checkdesign.Checked == true)
            {

                for (int i = 0; i < ddldept1.Items.Count; i++)
                {
                    ddldept1.Items[i].Selected = true;
                    txt_designview.Text = "Design(" + (ddldept1.Items.Count) + ")";
                }


            }
            else
            {
                for (int i = 0; i < ddldept1.Items.Count; i++)
                {
                    ddldept1.Items[i].Selected = false;
                    txt_designview.Text = "--Select--";
                }
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlcity_selectedchanged(object sender, EventArgs e)
    {
        int ddlcount = 0;
        try
        {
            txt_cityview.Text = "--Select--";

            string value = "";
            string code = "";
            for (int i = 0; i < ddlcity.Items.Count; i++)
            {
                if (ddlcity.Items[i].Selected == true)
                {

                    value = ddlcity.Items[i].Text;
                    code = ddlcity.Items[i].Value.ToString();
                    ddlcount = ddlcount + 1;
                    txt_cityview.Text = "City(" + ddlcount.ToString() + ")";
                }
            }
            if (ddlcount == 0)
                txt_cityview.Text = "---Select---";
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void CheckBox1_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            txt_cityview.Text = "--Select--";
            if (CheckBox1.Checked == true)
            {

                for (int i = 0; i < ddlcity.Items.Count; i++)
                {
                    ddlcity.Items[i].Selected = true;
                    txt_cityview.Text = "City(" + (ddlcity.Items.Count) + ")";
                }


            }
            else
            {
                for (int i = 0; i < ddlcity.Items.Count; i++)
                {
                    ddlcity.Items[i].Selected = false;
                    txt_cityview.Text = "--Select--";
                }
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void ddlinstition_selectedchanged(object sender, EventArgs e)
    {
        int ddlcount = 0;
        try
        {
            txt_viewinstition.Text = "--Select--";

            string value = "";
            string code = "";
            for (int i = 0; i < ddlinstition.Items.Count; i++)
            {

                if (ddlinstition.Items[i].Selected == true)
                {

                    value = ddlinstition.Items[i].Text;
                    code = ddlinstition.Items[i].Value.ToString();
                    ddlcount = ddlcount + 1;
                    txt_viewinstition.Text = "College(" + ddlcount.ToString() + ")";
                }



            }
            if (ddlcount == 0)
                txt_viewinstition.Text = "---Select---";
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void CheckBox2_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            txt_viewinstition.Text = "--Select--";
            if (CheckBox2.Checked == true)
            {
                for (int i = 0; i < ddlinstition.Items.Count; i++)
                {
                    ddlinstition.Items[i].Selected = true;
                    txt_viewinstition.Text = "College(" + (ddlinstition.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < ddlinstition.Items.Count; i++)
                {
                    ddlinstition.Items[i].Selected = false;
                    txt_viewinstition.Text = "--Select--";
                }
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void fpcammarkstaff_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string activerow = fpcammarkstaff.Sheets[0].ActiveRow.ToString();
            if (activerow == "0")
            {
                string selecttext = "";
                string actcol = "1";
                selecttext = e.EditValues[Convert.ToInt32(actcol)].ToString();
                for (int i = 1; i < fpcammarkstaff.Sheets[0].RowCount; i++)
                {
                    if (selecttext != "System Object")
                    {
                        fpcammarkstaff.Sheets[0].Cells[i, Convert.ToInt32(actcol)].Text = selecttext.ToString();
                    }

                }
            }
            fpupdate = true;

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void fpcammarkstaff_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            if (fpupdate == false)
            {
                Cellclick = true;
                int actrow1 = 0;
                int actcol1 = 0;
                string sqlquery = "";
                AddPageModify.Text = "Modify";
                btnsave.Text = "Update";
                ds.Clear();
                actrow1 = fpcammarkstaff.Sheets[0].ActiveRow;
                actcol1 = fpcammarkstaff.Sheets[0].ActiveColumn;
                string chkimgtag1 = Convert.ToString(fpcammarkstaff.Sheets[0].GetNote(Convert.ToInt16(actrow1), Convert.ToInt16(actcol1)));
                string external = fpcammarkstaff.Sheets[0].Cells[actrow1, 3].Text;
                string staffcode = fpcammarkstaff.Sheets[0].Cells[actrow1, 4].Tag.ToString();
                txttravelallowance.Text = string.Empty;
                txtdailyallowance.Text = string.Empty;
                string daamount = string.Empty;
                string taamount = string.Empty;

                Accordion1.SelectedIndex = 1;
                if (external == "Internal")
                {
                    sqlquery = "select sm.Taamount,sm.daAmount,UPPER(LTRIM(RTRIM(isnull(type,'')))) as type ,*  from examstaffmaster es,staffmaster sm,stafftrans st,staff_appl_master spm, hrdept_master hm,desig_master dm,collinfo c where es.staff_code=st.staff_code and sm.staff_code=st.staff_code and sm.appl_no=spm.appl_no and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and c.college_code=sm.college_code and sm.settled=0 and sm.resign=0 and st.latestrec=1 and sm.staff_code='" + staffcode + "'  and es.type='" + ddlstreamview.SelectedValue + "' ";
                }
                else if (external == "External")
                {
                    sqlquery = "select distinct es.staff_code as staffcode,exs.staff_name as staffname,exs.dept_code as deptcode,exs.design_code as desigcode,exs.coll_code as collcode,(select textval from textvaltable t where t.TextCode=exs.dept_code ) as department,(select textval from textvaltable t where t.TextCode=exs.design_code ) as design,(select textval from textvaltable t where t.TextCode=exs.coll_code ) as collname,setting,Inivigition,isexternal,sectio,Valuation,per_phone,per_mobileno,email,yearofexp,per_address,sex,per_pincode,experience_info,experience_info_carrer,dept_code,pcity_code,UPPER(LTRIM(RTRIM(isnull(type,'')))) as type,Val_subject_no,Set_subject_no,scrab,title,exs.daAmount,exs.taAmount  from external_staff exs,examstaffmaster es where convert(nvarchar(15),es.staff_code)=convert(nvarchar(15),exs.staff_code) and es.staff_code='" + staffcode + "'  and es.type='" + ddlstreamview.SelectedValue + "'";
                }
                ds = da.select_method_wo_parameter(sqlquery, "text");

                if (external == "Internal")
                {
                    panelinvilation.Visible = true;
                    ddlempno.Visible = false;
                    ddlempno1.Visible = true;
                    ddlempno1.Enabled = false;
                    ddlmrs.Visible = false;
                    ddlstatedyear.Enabled = false;
                    ddlscheme.Enabled = false;
                    ddlemptype.Text = "0";
                    ddlexdept.Visible = false;
                    ddlexterdesign.Visible = false;
                    ddlextuniv.Visible = false;
                    ddlextcity.Visible = false;
                    ddldept.Visible = true;
                    txtcity.Visible = true;
                    txtinstition.Visible = true;
                    txtdesign.Visible = true;
                    lblstaffname.Visible = true;
                    txtissueper.Visible = true;
                    btnstaff.Visible = true;
                    lblempno.Text = "Staff Code";
                    lblscheme.Visible = true;
                    ddlscheme.Visible = true;
                    lbluniversity.Visible = true;
                    txtuniversity.Visible = true;
                    txtissueper.Text = Convert.ToString(ds.Tables[0].Rows[0]["staff_name"]);
                    ddlempno1.Text = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"]);
                    ddldept.Text = Convert.ToString(ds.Tables[0].Rows[0]["dept_name"]);
                    ddlgender.Text = Convert.ToString(ds.Tables[0].Rows[0]["sex"]).Trim().ToUpper();
                    txtdesign.Text = Convert.ToString(ds.Tables[0].Rows[0]["desig_name"]);
                    txtinstition.Text = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                    txtuniversity.Text = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                    da1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["join_date"].ToString());
                    //modified
                    daamount = Convert.ToString(ds.Tables[0].Rows[0]["daAmount"]);
                    taamount = Convert.ToString(ds.Tables[0].Rows[0]["taAmount"]);

                    if (daamount == "0" || taamount == "0")
                    {
                        txtdailyallowance.Text = string.Empty;
                        txttravelallowance.Text = string.Empty;
                    }
                    else
                    {
                        txtdailyallowance.Text = daamount.ToString().Trim();
                        txttravelallowance.Text = taamount.ToString().Trim();
                    }


                    if (ddlyear.Items.Count > 0)
                    {
                        if (ddlyear.Items.FindByValue("" + da1.ToString("yyyy") + "") != null)
                        {
                            ddlyear.SelectedValue = da1.ToString("yyyy");
                        }
                        else
                        {
                            ddlyear.Items.Insert(ddlyear.Items.Count - 1, da1.ToString("yyyy"));
                            ddlyear.SelectedValue = da1.ToString("yyyy");
                        }
                    }
                    // scheme();
                    //  ddlscheme.Text = ds.Tables[0].Rows[0]["Stream"].ToString();
                    ddlscheme.SelectedIndex = ddlscheme.Items.IndexOf(ddlscheme.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["Stream"])));
                    txtaddress1.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_address1"]);
                    txtcity.Text = Convert.ToString(ds.Tables[0].Rows[0]["pcity"]);
                    txtpincode.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_pincode"]);
                    txtmobile.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_mobileno"]);
                    txtphone.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_phone"]);
                    txtemil.Text = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                    perexp = Convert.ToString(ds.Tables[0].Rows[0]["experience_info"]);
                    exper();
                    txtyear.Text = Convert.ToString(ds.Tables[0].Rows[0]["yearofexp"]);
                    txtissueper.Enabled = false;
                    ddlemptype.Enabled = false;
                    ddlstaff.Enabled = false;
                    ddlempno.Enabled = false;
                    ddldept.Enabled = false;
                    ddlgender.Enabled = false;
                    txtdesign.Enabled = false;
                    txtinstition.Enabled = false;
                    txtuniversity.Enabled = false;
                    ddlyear.Enabled = false;
                    txtaddress1.Enabled = false;
                    txtcity.Enabled = false;
                    txtpincode.Enabled = false;
                    txtmobile.Enabled = false;
                    txtphone.Enabled = false;
                    txtemil.Enabled = false;
                    txtyear.Enabled = false;

                    ddlstreamadd.Text = Convert.ToString(ds.Tables[0].Rows[0]["type"]);
                    ddlFnAn.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["sectio"]);


                }
                else if (external == "External")
                {
                    panelinvilation.Visible = false;
                    ddldept.Visible = false;
                    ddlmrs.Visible = true;
                    ddlempno.Visible = true;
                    ddlempno1.Visible = false;
                    ddlemptype.Enabled = false;
                    ddlemptype.Text = "1";
                    lblstaffname.Visible = false;
                    txtissueper.Visible = false;
                    btnstaff.Visible = false;
                    lblempno.Text = "Name";
                    lblscheme.Visible = false;
                    ddlscheme.Visible = false;
                    lbluniversity.Visible = false;
                    txtuniversity.Visible = false;
                    ddlscheme.Enabled = true;
                    txtissueper.Enabled = true;
                    ddlempno.Enabled = true;
                    ddldept.Enabled = true;
                    ddlgender.Enabled = true;
                    txtdesign.Enabled = true;
                    txtinstition.Visible = false;
                    txtuniversity.Enabled = true;
                    txtaddress2.Enabled = true;
                    txtaddress3.Enabled = true;
                    ddlyear.Enabled = true;
                    txtaddress1.Enabled = true;
                    txtcity.Visible = false;
                    txtpincode.Enabled = true;
                    txtmobile.Enabled = true;
                    txtphone.Enabled = true;
                    txtemil.Enabled = true;
                    txtyear.Enabled = true;
                    ddlgender.Text = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
                    ddlmrs.Text = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                    ddlempno.Text = Convert.ToString(ds.Tables[0].Rows[0]["staffname"]);
                    ddlexdept.Text = Convert.ToString(ds.Tables[0].Rows[0]["deptcode"]);
                    txtyear.Text = Convert.ToString(ds.Tables[0].Rows[0]["yearofexp"]);
                    ddlexterdesign.Text = Convert.ToString(ds.Tables[0].Rows[0]["desigcode"]);
                    ddlextuniv.Text = Convert.ToString(ds.Tables[0].Rows[0]["collcode"]);
                    string addval = Convert.ToString(ds.Tables[0].Rows[0]["per_address"]); ;
                    string[] spval = addval.Split(',');


                    txtdailyallowance.Text = Convert.ToString(ds.Tables[0].Rows[0]["daAmount"]);
                    txttravelallowance.Text = Convert.ToString(ds.Tables[0].Rows[0]["taAmount"]);



                    if (spval.GetUpperBound(0) > 0)
                    {
                        for (int spa = 0; spa <= spval.GetUpperBound(0); spa++)
                        {
                            if (spa == 0)
                            {
                                txtaddress1.Text = spval[spa].ToString();
                            }
                            else if (spa == 1)
                            {
                                txtaddress2.Text = spval[spa].ToString();
                            }
                            else
                            {
                                txtaddress3.Text = spval[spa].ToString();
                            }
                        }
                    }
                    else
                    {
                        txtaddress1.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_address"]);
                    }

                    string cityer = Convert.ToString(ds.Tables[0].Rows[0]["pcity_code"]);
                    if (cityer.ToString() == "")
                    {
                        ddlextcity.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlextcity.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["pcity_code"]);

                    }
                    txtpincode.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_pincode"]);
                    txtmobile.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_mobileno"]);
                    txtphone.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_phone"]);
                    txtemil.Text = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                    ddlyear.Text = Convert.ToString(ds.Tables[0].Rows[0]["experience_info"]);
                    ddlstatedyear.Text = Convert.ToString(ds.Tables[0].Rows[0]["experience_info_carrer"]);
                    ddlstatedyear.Enabled = true;
                    ddlexdept.Visible = true;
                    ddlexterdesign.Visible = true;
                    ddlextuniv.Visible = true;
                    ddlextcity.Visible = true;
                    txtdesign.Visible = false;
                    ddlFnAn.Text = Convert.ToString(ds.Tables[0].Rows[0]["sectio"]);
                    //ddlstreamadd.Text = Convert.ToString(ds.Tables[0].Rows[0]["year"]);
                    ddlstreamadd.Text = Convert.ToString(ds.Tables[0].Rows[0]["type"]);
                }
                string setting = Convert.ToString(ds.Tables[0].Rows[0]["setting"]);
                if (setting.ToString() == "True")
                {
                    lblsetting.Checked = true;
                }
                else
                {
                    lblsetting.Checked = false;
                }
                string Valuation = Convert.ToString(ds.Tables[0].Rows[0]["Valuation"]);
                if (Valuation.ToString() == "True")
                {
                    lblvalution.Checked = true;
                }
                else
                {
                    lblvalution.Checked = false;
                }
                string invalation = Convert.ToString(ds.Tables[0].Rows[0]["Inivigition"]);
                if (invalation.ToString() == "True")
                {
                    lblinvi.Checked = true;
                }
                else
                {
                    lblinvi.Checked = false;
                }
                string scrab = Convert.ToString(ds.Tables[0].Rows[0]["scrab"]);
                if (scrab.ToString() == "True")
                {
                    lblscred.Checked = true;
                }
                else
                {
                    lblscred.Checked = false;
                }
                subjectload();
                if (setting == "True")
                {
                    int jcount = 0;
                    int k = 0;
                    for (int h = 0; h < ddlsetting.Items.Count; h++)
                    {

                        ddlsetting.Items[h].Selected = false;

                    }
                    lblsetting.Checked = true;
                    txtsettingtextbox.Visible = true;
                    panel9.Visible = true;
                    string strsetval = "";
                    string subjectnosetting = Convert.ToString(ds.Tables[0].Rows[0]["Set_subject_no"]);
                    if (subjectnosetting.Trim() != "")
                    {
                        strsetval = "and s.subject_no in(" + subjectnosetting.ToString() + ")";

                        //  string sqlqueryqw = "select distinct s.subject_name,s.subject_code from Exam_Details ed,exam_appl_details ead,exam_application ea,subject s where ed.exam_code=ea.exam_code and ead.appl_no=ea.appl_no and ead.subject_no=s.subject_no  and ed.Exam_year=" + ddlstreamadd.SelectedValue + " and s.subject_no in(" + subjectnosetting.ToString() + ")";
                        string sqlqueryqw = "select distinct s.subject_name,s.subject_code from Exam_Details ed,exam_appl_details ead,exam_application ea,subject s where ed.exam_code=ea.exam_code and ead.appl_no=ea.appl_no and ead.subject_no=s.subject_no  " + strsetval + "";
                        DataSet datasde = da.select_method_wo_parameter(sqlqueryqw, "text");
                        subjectnosetting = "";
                        for (int j = 0; j < datasde.Tables[0].Rows.Count; j++)
                        {
                            if (subjectnosetting.ToString() == "")
                            {
                                subjectnosetting = datasde.Tables[0].Rows[j]["subject_code"].ToString();
                            }
                            else
                            {
                                subjectnosetting = subjectnosetting + "," + datasde.Tables[0].Rows[j]["subject_code"].ToString();
                            }
                        }
                    }
                    else
                    {
                        subjectnosetting = "";
                    }
                    string[] subject = subjectnosetting.Split(',');
                    for (int j = 0; j <= subject.GetUpperBound(0); j++)
                    {
                        for (int h = 0; h < ddlsetting.Items.Count; h++)
                        {
                            if (subject[j].ToString() == ddlsetting.Items[h].Value.ToString())
                            {
                                ddlsetting.Items[h].Selected = true;
                                jcount++;
                            }
                        }
                    }

                    if (jcount > 0)
                    {
                        txtsettingtextbox.Text = "Subject(" + jcount + ")";
                    }
                    else
                    {
                        txtsettingtextbox.Text = "--Select--";
                    }
                }
                else
                {
                    lblsetting.Checked = false;
                }

                if (Valuation == "True")
                {
                    int jcount = 0;
                    for (int h = 0; h < ddlvalution.Items.Count; h++)
                    {

                        ddlvalution.Items[h].Selected = false;

                    }
                    lblvalution.Checked = true;
                    txtvalutationtextbox.Visible = true;
                    panel7.Visible = true;
                    string subjectnosetting = Convert.ToString(ds.Tables[0].Rows[0]["Val_subject_no"]);
                    //string sqlqueryqw = "select distinct s.subject_name,s.subject_code from Exam_Details ed,exam_appl_details ead,exam_application ea,subject s where ed.exam_code=ea.exam_code and ead.appl_no=ea.appl_no and ead.subject_no=s.subject_no and ed.Exam_year='" + ddlstreamadd.SelectedValue + "' and s.subject_no in('" + subjectnosetting.ToString() + "')";
                    //DataSet datasde = da.select_method_wo_parameter(sqlqueryqw, "text");
                    //subjectnosetting = "";
                    //for (int j = 0; j < datasde.Tables[0].Rows.Count; j++)
                    //{
                    //    if (subjectnosetting.ToString() == "")
                    //    {
                    //        subjectnosetting = datasde.Tables[0].Rows[j]["subject_code"].ToString();
                    //    }
                    //    else
                    //    {
                    //        subjectnosetting = subjectnosetting + "," + datasde.Tables[0].Rows[j]["subject_code"].ToString();
                    //    }
                    //}
                    string[] subject = subjectnosetting.Split(',');
                    for (int j = 0; j <= subject.GetUpperBound(0); j++)
                    {
                        for (int h = 0; h < ddlvalution.Items.Count; h++)
                        {
                            if (subject[j].ToString().Trim().ToLower() == ddlvalution.Items[h].Value.ToString().Trim().ToLower())
                            {
                                ddlvalution.Items[h].Selected = true;
                                jcount++;
                            }
                        }
                    }
                    if (jcount > 0)
                    {
                        txtvalutationtextbox.Text = "Subject(" + jcount + ")";
                    }
                    else
                    {
                        txtvalutationtextbox.Text = "--Select--";
                    }
                }
                else
                {
                    lblvalution.Checked = false;
                }
            }

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void fpcammarkstaff_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Cellclick = true;

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {

            string degreedetails = "Exam Staff Master " + '@' + "Date :" + DateTime.Now.ToString();
            string pagename = "examstaffmaster.aspx";
            Printcontrol.loadspreaddetails(fpcammarkstaff, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexcelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreport(fpcammarkstaff, report);

            }
            else
            {
                lblmessage1.Text = "Please Enter Your Report Name";
                lblmessage1.Visible = true;
            }

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chkboxsms_CheckedChangeds(object sender, EventArgs e)
    {
        fpcammarkstaff.Visible = false;
        txtmessage.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        txtmessage.Visible = false;
        btnprintmaster.Visible = false;
        btndelete.Visible = false;
        btnsms.Visible = false;
        lblrptname.Visible = false;
        labpurpose.Visible = false;
        ddlpurpose.Visible = false;
        fpspreadpurpose.Visible = false;
        btnaddtemplate.Visible = false;
        btndeletetemplate.Visible = false;


    }

    protected void chkboxmail_CheckedChanged(object sender, EventArgs e)
    {
        fpcammarkstaff.Visible = false;
        txtmessage.Visible = false;
        btnsms.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        txtmessage.Visible = false;
        btnprintmaster.Visible = false;
        btndelete.Visible = false;
        lblrptname.Visible = false;
        labpurpose.Visible = false;
        ddlpurpose.Visible = false;
        fpspreadpurpose.Visible = false;
        btnaddtemplate.Visible = false;
        btndeletetemplate.Visible = false;
    }

    public void smsreport(string uril, string mobilenos)
    {
        try
        {
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();

            string groupmsgid = "";
            groupmsgid = strvel.Trim().ToString(); //aruna 02oct2013 strvel;       

            int sms = 0;
            string smsreportinsert = "";

            string[] split_mobileno = mobilenos.Split(new Char[] { ',' });

            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + message + "','" + Session["collegecode"].ToString() + "','1','" + date + "')";
                sms = da.update_method_wo_parameter(smsreportinsert, "Text");
            }

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btnsms_Click(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == false)
            {
                Cellclick = true;
            }
            Boolean check_flag = false;
            Boolean send = false;
            ds.Clear();

            if (chkboxsms.Checked == true)
            {
                string collegeusercode = string.Empty;

                string sqlcollege = "select SMS_User_ID,college_code from track_value where college_code='" + collegecode + "'";
                ds = da.select_method_wo_parameter(sqlcollege, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    user_id = ds.Tables[0].Rows[0]["SMS_User_ID"].ToString();
                }
                string getval = da.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {

                    SenderID = spret[0].ToString();
                    Password = spret[0].ToString();
                    Session["api"] = user_id;
                    Session["senderid"] = SenderID;
                }
                message = txtmessage.Text;
                int k = 0;
                fpcammarkstaff.SaveChanges();
                for (int j = 1; j < fpcammarkstaff.Sheets[0].RowCount; j++)
                {
                    int gam = Convert.ToInt32(fpcammarkstaff.Sheets[0].Cells[j, 1].Value);
                    if (gam == 1)
                    {
                        check_flag = true;
                        strmobileno = fpcammarkstaff.Sheets[0].Cells[j, 16].Text;
                        if (strmobileno != "Nil" && strmobileno != "")
                        {
                            mobilenos = strmobileno.ToString();
                            //string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + message + " &priority=ndnd&stype=normal";
                            //string isstf = mobilenos;
                            //smsreport(strpath1, isstf);
                            int nofosmssend = da.send_sms(user_id, ddlcollege.SelectedValue.ToString(), usercode, mobilenos, message, "1");
                            send = true;

                        }
                        else
                        {
                            k++;
                        }
                    }
                }
                if (k == 0)
                {

                }
                else
                {
                    lblerrormsg.Text = "Phone no is not avaliable for " + k + " Staffs";
                    lblerrormsg.Visible = true;

                }
                if (chkboxmail.Checked == false)
                {
                    txtmessage.Text = "";
                }

            }
            if (chkboxmail.Checked == true)
            {
                message = txtmessage.Text;
                string strquery = "select massemail,masspwd from collinfo where college_code = " + collegecode + " ";
                ds = da.select_method_wo_parameter(strquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    mailid = ds.Tables[0].Rows[0]["massemail"].ToString();
                    mailpwd = ds.Tables[0].Rows[0]["masspwd"].ToString();
                }
                int j = 0;
                fpcammarkstaff.SaveChanges();
                for (int l = 1; l < fpcammarkstaff.Sheets[0].RowCount; l++)
                {
                    int isval = Convert.ToInt32(fpcammarkstaff.Sheets[0].Cells[l, 1].Value);
                    if (isval == 1)
                    {
                        check_flag = true;
                        strmobileno = fpcammarkstaff.Sheets[0].Cells[l, 16].Text;
                        strstuname = (fpcammarkstaff.Sheets[0].Cells[l, 4].Text);
                        to_mail = (fpcammarkstaff.Sheets[0].Cells[l, 14].Text);
                        if (to_mail.ToString() != "")
                        {
                            SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                            MailMessage mailmsg = new MailMessage();
                            MailAddress mfrom = new MailAddress(mailid);
                            mailmsg.From = mfrom;
                            mailmsg.To.Add(to_mail);
                            mailmsg.Subject = "Report";
                            mailmsg.IsBodyHtml = true;
                            mailmsg.Body = "Hi  ";
                            mailmsg.Body = mailmsg.Body + strstuname;
                            mailmsg.Body = mailmsg.Body + "<br>";
                            mailmsg.Body = mailmsg.Body + message;
                            mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                            Mail.EnableSsl = true;
                            NetworkCredential credentials = new NetworkCredential(mailid, mailpwd);
                            Mail.UseDefaultCredentials = false;
                            Mail.Credentials = credentials;
                            Mail.Send(mailmsg);
                            send = true;
                        }
                        else
                        {
                            j++;
                        }
                    }

                }
                if (j == 0)
                {
                }
                else
                {
                    lblerrormsg.Text = "Mail ID is not avaliable for " + j + " Staffs";
                    lblerrormsg.Visible = true;
                }
            }
            if (send == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Send Successfully')", true);
            }
            if (check_flag == true)
            {
            }
            else
            {
                lblerrormsg.Text = "Please Select Any One Staff and Then Proceed";
                lblerrormsg.Visible = true;

            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void bindexternalyear()
    {
        try
        {
            int dateyear = DateTime.Now.Year;

            for (int g = dateyear - 45; g <= dateyear; g++)
            {
                ddlstatedyear.Items.Add((g.ToString()));

                ddlyear.Items.Add((g.ToString()));
            }
            ddlyear.Items.Insert(0, "--Select--");
            ddlstatedyear.Items.Insert(0, "--Select--");
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlyear.Text != "--Select--" && ddlstatedyear.Text != "--Select--")
            {
                if (Convert.ToInt32(ddlyear.Text) >= Convert.ToInt32(ddlstatedyear.Text))
                {
                    int exp = (Convert.ToInt32(ddlyear.Text) - Convert.ToInt32(ddlstatedyear.Text)) + (Convert.ToInt32(DateTime.Now.Year) - Convert.ToInt32(ddlyear.Text));
                    txtyear.Text = exp.ToString();
                    lblerror1.Visible = false;
                }
                else
                {
                    lblerror1.Visible = true;
                    lblerror1.Text = "Started Should be Greater than Join Year";

                }
            }
            else
            {
                lblerror1.Visible = true;
                lblerror1.Text = "Select Any Join Year And Started Year";

            }
            btnfoucs.Focus();

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlstatedyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlyear.Text != "--Select--" && ddlstatedyear.Text != "--Select--")
            {
                if (Convert.ToInt32(ddlyear.Text) >= Convert.ToInt32(ddlstatedyear.Text))
                {
                    int exp = (Convert.ToInt32(ddlyear.Text) - Convert.ToInt32(ddlstatedyear.Text)) + (Convert.ToInt32(DateTime.Now.Year) - Convert.ToInt32(ddlyear.Text));
                    txtyear.Text = exp.ToString();
                    lblerror1.Visible = false;
                }
                else
                {
                    lblerror1.Visible = true;
                    lblerror1.Text = "Started Should be Greater than Join Year";

                }

            }
            else
            {
                lblerror1.Visible = true;
                lblerror1.Text = "Select Any Started Year And Join Year";

            }
            btnfoucs.Focus();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void lblsetting_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (lblsetting.Checked == true)
            {
                for (int h = 0; h < ddlsetting.Items.Count; h++)
                {
                    ddlsetting.Items[h].Selected = false;
                }
                panel9.Visible = true;
                txtsettingtextbox.Visible = true;

            }
            else
            {
                panel9.Visible = false;
                txtsettingtextbox.Visible = false;

            }
            btnfoucs.Focus();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void lblvalution_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (lblvalution.Checked == true)
            {
                for (int h = 0; h < ddlvalution.Items.Count; h++)
                {
                    ddlvalution.Items[h].Selected = false;
                }
                panel7.Visible = true;
                txtvalutationtextbox.Visible = true;
            }
            else
            {
                panel7.Visible = false;
                txtvalutationtextbox.Visible = false;
            }
            btnfoucs.Focus();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCity(string prefixText)
    {

        DataSet dt = new DataSet();
        DAccess2 dsa = new DAccess2();

        string strsql = "with  asha as(select substring(staff_name, CHARINDEX(' ', staff_name)+1, len(staff_name)-(CHARINDEX(' ', staff_name)-1))as staff,staff_code from external_staff ) select substring(staff_name, CHARINDEX(' ', staff_name)+1, len(staff_name)-(CHARINDEX(' ', staff_name)-1)) as staff_name,a.staff_code from external_staff,asha a where a.staff like '" + prefixText + "%' and a.staff_code=external_staff.staff_code";
        dt = dsa.select_method_wo_parameter(strsql, "text");

        List<string> CityNames = new List<string>();

        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
        {
            CityNames.Add(Convert.ToString(dt.Tables[0].Rows[i]["Staff_name"]) + "(" + Convert.ToString(dt.Tables[0].Rows[i]["Staff_code"]) + ")");

        }

        return CityNames;
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCity1(string prefixText)
    {

        DataSet dt = new DataSet();
        DAccess2 dsa = new DAccess2();

        string strsql = "select Staff_name,Staff_code from staffmaster where staff_code like '" + prefixText + "%'";
        dt = dsa.select_method_wo_parameter(strsql, "text");

        List<string> CityNames = new List<string>();

        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
        {
            CityNames.Add(dt.Tables[0].Rows[i]["Staff_code"].ToString());

        }

        return CityNames;
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCity12(string prefixText)
    {

        DataSet dt = new DataSet();
        DAccess2 dsa = new DAccess2();

        string strsql = "select Staff_name,Staff_code from staffmaster where Staff_Name like '" + prefixText + "%'";
        dt = dsa.select_method_wo_parameter(strsql, "text");

        List<string> CityNames = new List<string>();

        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
        {
            CityNames.Add(Convert.ToString(dt.Tables[0].Rows[i]["Staff_name"]) + "(" + Convert.ToString(dt.Tables[0].Rows[i]["Staff_code"]) + ")");

        }

        return CityNames;
    }

    protected void txtissueper_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            string staff_code = txtissueper.Text;
            string[] data = staff_code.Split('(', ')');
            string sqlquery = "";
            if (data.Length != 1)
            {
                staff_code = data[1].ToString();
                sqlquery = "select * from staff_appl_master sa,staffmaster sm,desig_master dm where sa.appl_no=sm.appl_no and sm.staff_code='" + staff_code + "'  and sa.college_code=dm.collegeCode and sa.desig_code=dm.desig_code select * from collinfo where college_code='" + collegecode + "'";
                ds = da.select_method_wo_parameter(sqlquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtissueper.Text = Convert.ToString(ds.Tables[0].Rows[0]["Staff_Name"]);
                    ddlempno1.Text = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"]);
                    ddldept.Text = Convert.ToString(ds.Tables[0].Rows[0]["dept_name"]);
                    ddlgender.Text = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
                    txtdesign.Text = Convert.ToString(ds.Tables[0].Rows[0]["desig_name"]);
                    txtinstition.Text = Convert.ToString(ds.Tables[1].Rows[0]["collname"]);
                    txtuniversity.Text = Convert.ToString(ds.Tables[1].Rows[0]["university"]);
                    da1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["join_date"].ToString());
                    if (ddlyear.Items.Count > 0)
                    {
                        if (ddlyear.Items.FindByValue("" + da1.ToString("yyyy") + "") != null)
                        {
                            ddlyear.SelectedValue = da1.ToString("yyyy");
                        }
                        else
                        {
                            ddlyear.Items.Insert(ddlyear.Items.Count - 1, da1.ToString("yyyy"));
                            ddlyear.SelectedValue = da1.ToString("yyyy");
                        }
                    }
                    // scheme();
                    //  ddlscheme.Text = ds.Tables[0].Rows[0]["Stream"].ToString();
                    ddlscheme.SelectedIndex = ddlscheme.Items.IndexOf(ddlscheme.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["Stream"])));
                    txtaddress1.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_address1"]);
                    txtcity.Text = Convert.ToString(ds.Tables[0].Rows[0]["pcity"]);
                    txtpincode.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_pincode"]);
                    txtmobile.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_mobileno"]);
                    txtphone.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_phone"]);
                    txtemil.Text = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                    perexp = Convert.ToString(ds.Tables[0].Rows[0]["experience_info"]);
                    exper();
                    ddlempno.Visible = true;
                    txtissueper.Enabled = true;
                    ddlstatedyear.Enabled = false;
                    ddlempno.Visible = false;
                    ddldept.Enabled = false;
                    ddlgender.Enabled = false;
                    txtdesign.Enabled = false;
                    txtinstition.Enabled = false;
                    txtuniversity.Enabled = false;
                    txtaddress2.Enabled = false;
                    txtaddress3.Enabled = false;
                    ddlyear.Enabled = false;
                    txtaddress1.Enabled = false;
                    txtcity.Enabled = false;
                    txtpincode.Enabled = false;
                    txtmobile.Enabled = false;
                    txtphone.Enabled = false;
                    txtemil.Enabled = false;
                    txtyear.Enabled = false;
                    ddlscheme.Enabled = false;
                    perexp = Convert.ToString(ds.Tables[0].Rows[0]["experience_info"]);
                }
                else
                {

                    txtissueper.Text = "";

                    ddldept.Text = "";
                    ddlgender.SelectedIndex = 0;
                    txtdesign.Text = "";
                    txtinstition.Text = "";
                    txtuniversity.Text = "";
                    ddlyear.SelectedIndex = 0;
                    ddlscheme.SelectedIndex = 0;
                    txtaddress1.Text = "";
                    txtcity.Text = "";
                    txtpincode.Text = "";
                    txtmobile.Text = "";
                    txtphone.Text = "";
                    txtemil.Text = "";
                    ddlstatedyear.SelectedIndex = 0;
                    ddlempno.Visible = true;
                    txtissueper.Enabled = true;
                    ddlstatedyear.Enabled = false;
                    ddlempno.Visible = false;
                    ddldept.Enabled = false;
                    ddlgender.Enabled = false;
                    txtdesign.Enabled = false;
                    txtinstition.Enabled = false;
                    txtuniversity.Enabled = false;
                    txtaddress2.Enabled = false;
                    txtaddress3.Enabled = false;
                    ddlyear.Enabled = false;
                    txtaddress1.Enabled = false;
                    txtcity.Enabled = false;
                    txtpincode.Enabled = false;
                    txtmobile.Enabled = false;
                    txtphone.Enabled = false;
                    txtemil.Enabled = false;
                    txtyear.Enabled = false;
                    ddlscheme.Enabled = false;
                    lblerror1.Text = "Staff Name Is Not Registered";
                    lblerror1.Visible = true;

                }

            }
            else
            {
                lblerror1.Text = "Staff Name Is Not Registered";
                lblerror1.Visible = true;
            }


        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlempno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            if (ddlemptype.SelectedValue == "1")
            {

                string staff_code = ddlempno.Text;
                string[] data = staff_code.Split('(', ')');
                if (data.Length != 1)
                {
                    staff_code = data[1].ToString();

                    string sqlquery = "select * from external_staff exs where staff_code='" + staff_code + "'";
                    ds = da.select_method_wo_parameter(sqlquery, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlmrs.Text = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                        ddlgender.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
                        ddlyear.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["experience_info"]);
                        ddlstatedyear.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["experience_info_carrer"]);
                        txtaddress1.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_address"]);
                        txtaddress2.Text = "";
                        txtaddress3.Text = "";
                        txtpincode.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_pincode"]);
                        txtmobile.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_mobileno"]);
                        txtphone.Text = Convert.ToString(ds.Tables[0].Rows[0]["per_phone"]);
                        txtemil.Text = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                        ddlexdept.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["dept_code"]);
                        string cityer = Convert.ToString(ds.Tables[0].Rows[0]["pcity_code"]);
                        if (cityer.ToString() == "")
                        {
                            ddlextcity.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlextcity.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["pcity_code"]);

                        }
                        ddlexterdesign.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["design_code"]);
                        ddlextuniv.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["coll_code"]);
                        txtyear.Text = Convert.ToString(ds.Tables[0].Rows[0]["totalexp"]);

                    }
                }
            }
            btnfoucs.Focus();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void ddlempno1_TextChanged(object sender, EventArgs e)
    {
        bind();
        btnfoucs.Focus();
    }

    protected void subjectload()
    {
        try
        {
            ds2.Clear();
            string sqlquery = "select distinct subject_name,subject_code,subject_code+'-'+subject_name as subcodename from subject s order by subject_name,subject_code desc";
            ds2 = da.select_method_wo_parameter(sqlquery, "text");
            if (ds2.Tables[0].Rows.Count > 0)
            {

                ddlsetting.DataSource = ds2;
                ddlsetting.DataTextField = "subcodename";
                ddlsetting.DataValueField = "subject_code";
                ddlsetting.DataBind();

                ddlvalution.DataSource = ds2;
                ddlvalution.DataTextField = "subcodename";
                ddlvalution.DataValueField = "subject_code";
                ddlvalution.DataBind();

            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void purpose()
    {

        ddlpurpose.Items.Clear();
        string sqlquery = "select purpose,temp_code from sms_purpose where college_code = '" + collegecode + "'";
        DataSet purpo = da.select_method_wo_parameter(sqlquery, "text");

        if (purpo.Tables[0].Rows.Count > 0)
        {

            ddlpurpose.DataSource = purpo;
            ddlpurpose.DataTextField = "purpose";
            ddlpurpose.DataValueField = "temp_code";
            ddlpurpose.DataBind();
            ddlpurpose.Items.Insert(0, "");


            ddlpurposemsg.DataSource = purpo;
            ddlpurposemsg.DataTextField = "purpose";
            ddlpurposemsg.DataValueField = "temp_code";
            ddlpurposemsg.DataBind();
            ddlpurposemsg.Items.Insert(0, "");

        }
    }

    protected void bindpurpose()
    {
        try
        {
            fpspreadpurpose.Sheets[0].ColumnHeaderVisible = false;
            fpspreadpurpose.Sheets[0].SheetCorner.Columns[0].Visible = false;
            fpspreadpurpose.Visible = true;

            //lblpurpose1.Visible = true;
            ddlpurpose.Visible = true;
            fpspreadpurpose.Sheets[0].RowCount = 1;
            fpspreadpurpose.Sheets[0].ColumnCount = 2;
            fpspreadpurpose.Columns[1].Width = 900;
            fpspreadpurpose.Height = 200;
            fpspreadpurpose.Sheets[0].AutoPostBack = true;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Text = "S.No";
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Locked = true;

            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Text = "Template";
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Locked = true;
            string gfg = ddlpurpose.SelectedValue.ToString();
            //string gfvgj = ddlpurposemsg.Text;


            if (gfg == "")
            {
                ds.Dispose();
                ds.Reset();

                string spread2query = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template";
                ds = da.select_method_wo_parameter(spread2query, "Text");
            }
            else
            {
                string spread2query1 = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template where temp_code = " + ddlpurpose.SelectedValue + "";
                ds = da.select_method_wo_parameter(spread2query1, "Text");
            }


            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    fpspreadpurpose.Sheets[0].RowCount++;
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["SrNo"]);

                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
                }
            }
            fpspreadpurpose.Sheets[0].PageSize = fpspreadpurpose.Sheets[0].RowCount;
            fpspreadpurpose.SaveChanges();
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }


    }

    protected void ddlpurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindpurpose();
    }

    protected void spread1()
    {
        try
        {
            ds.Clear();
            fpspreadpurpose.Sheets[0].ColumnHeaderVisible = false;

            fpspreadpurpose.Sheets[0].SheetCorner.Columns[0].Visible = false;
            //FpSpread2.Visible = true;

            //lblpurpose1.Visible = true;
            //ddlpurpose.Visible = true;
            fpspreadpurpose.Sheets[0].RowCount = 1;
            fpspreadpurpose.Sheets[0].ColumnCount = 2;
            fpspreadpurpose.Columns[1].Width = 900;
            fpspreadpurpose.Height = 100;
            fpspreadpurpose.Sheets[0].AutoPostBack = true;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Text = "S.No";
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Locked = true;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Text = "Template";
            fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Locked = true;


            string spread2query1 = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template";
            ds = da.select_method_wo_parameter(spread2query1, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    fpspreadpurpose.Sheets[0].RowCount++;
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["SrNo"]);

                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    fpspreadpurpose.Sheets[0].Cells[fpspreadpurpose.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
                }
            }
            fpspreadpurpose.Sheets[0].PageSize = fpspreadpurpose.Sheets[0].RowCount;
            fpspreadpurpose.SaveChanges();
            fpspreadpurpose.Sheets[0].Columns[0].Locked = true;
            fpspreadpurpose.Sheets[0].Columns[1].Locked = true;

        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btnaddtemplate_Click(object sender, EventArgs e)
    {

        templatepanel.Visible = true;
    }

    protected void btndeletetemplate_Click(object sender, EventArgs e)
    {
        try
        {

            Cellclick = true;

            if (Cellclick == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = fpspreadpurpose.ActiveSheetView.ActiveRow.ToString();
                activecol = fpspreadpurpose.ActiveSheetView.ActiveColumn.ToString();
                int ar;
                int ac;
                ar = Convert.ToInt32(activerow.ToString());
                ac = Convert.ToInt32(activecol.ToString());
                if (ar != -1)
                {
                    string msg = fpspreadpurpose.Sheets[0].GetText(ar, 1);
                    string strdeletequery = "delete   sms_template where Template='" + msg + "'";
                    int vvv = da.insert_method(strdeletequery, ht, "text");

                    if (vvv == 1)
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Delete Template Succefully";
                    }
                    else
                    {
                        lblerror.Text = "Delete Template  failed";
                    }
                }
                spread1();
                Cellclick = false;
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void fpspreadpurpose_CellClick(object sender, EventArgs e)
    {
        Cellclick = true;
    }

    protected void fpspreadpurpose_SelectedIndexChanged(Object sender, EventArgs e)
    {
        Cellclick = true;

        if (Cellclick == true)
        {
            string activerow = "";
            string activecol = "";
            activerow = fpspreadpurpose.ActiveSheetView.ActiveRow.ToString();
            activecol = fpspreadpurpose.ActiveSheetView.ActiveColumn.ToString();
            int ar;
            int ac;
            ar = Convert.ToInt32(activerow.ToString());
            ac = Convert.ToInt32(activecol.ToString());
            if (ar != -1)
            {
                txtmessage.Text = fpspreadpurpose.Sheets[0].GetText(ar, 1);
            }
            Cellclick = false;
        }
    }

    protected void btnpurposeadd_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            int i = 0;
            string purposemessage = txtpurposecaption.Text;
            if (purposemessage != "")
            {
                string sqlquery = "insert into sms_purpose (Purpose,college_code) values ( '" + purposemessage + "','" + collegecode + "') ";
                i = da.insert_method(sqlquery, ht, "text");
                if (i != 0)
                {
                    purpose();
                    ddlpurposemsg.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(txtpurposecaption.Text.Trim()));
                    ddlpurpose.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(txtpurposecaption.Text.Trim()));
                }
            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btnpurposeexit_Click(object sender, EventArgs e)
    {

        purposepanel.Visible = false;
        templatepanel.Visible = false;

    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        templatepanel.Visible = false;
    }

    protected void btnsave1_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            int i = 0;
            string content = txtpurposemsg.Text;
            string sqlquery = "insert into sms_template (temp_code,Template,college_code) values ('" + ddlpurposemsg.SelectedItem.Value + "','" + content + "','" + collegecode + "') ";
            i = da.insert_method(sqlquery, ht, "text");
            if (i != 0)
            {
                purpose();

            }
        }
        catch (SqlException ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }

    protected void btnminus1_Click(object sender, EventArgs e)
    {

        try
        {

            ht.Clear();
            int i = 0;

            string strdelpurpose = "Delete from sms_purpose where temp_code = '" + ddlpurposemsg.SelectedValue + "'";
            i = da.insert_method(strdelpurpose, ht, "Text");
            if (i == 1)
            {
                lblerror.Text = "Purpose deleted Successfully";
                lblerror.Visible = true;
                purpose();
            }
            else
            {
                lblerror.Text = "Purpose deleted Failed";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void btnplus1_Click(object sender, EventArgs e)
    {
        purposepanel.Visible = true;
    }

    protected void fsstaff_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = e.SheetView.ActiveRow.ToString();
            if (flag_true == false && actrow == "0")
            {
                int s = Convert.ToInt16(fsstaff.Sheets[0].Cells[0, 3].Value);

                for (int j = 1; j < Convert.ToInt16(fsstaff.Sheets[0].RowCount); j++)
                {
                    fsstaff.Sheets[0].Cells[j, 3].Value = s;
                }
                flag_true = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

}