using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Configuration;

public partial class AdmissionPrint : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;
    int i = 0;

    Hashtable hat = new Hashtable();

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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                bindcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                }
                loadstream();
                loadedulevel();
                BindBatch();
                Bindcourse();
                binddept();
                txtfrmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtfrmdate.Attributes.Add("readonly", "readonly");
                txttodate.Attributes.Add("readonly", "readonly");
            }
        }
        catch (Exception ex) { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread.SaveChanges();
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                txtexcel.Text = "";
                d2.printexcelreport(FpSpread, reportname);
                lblsmserror.Visible = false;
            }
            else
            {
                lblsmserror.Text = "Please Enter Your Report Name";
                lblsmserror.Visible = true;
            }
            btnprintmaster.Focus();
        }
        catch { }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Admission Print Format";
            string pagename = "AdmissionPrint.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadstream();
            loadedulevel();
            BindBatch();
            Bindcourse();
            binddept();
        }
        catch { }
    }

    protected void type_Change(object sender, EventArgs e)
    {
        try
        {
            loadedulevel();
            Bindcourse();
            binddept();
        }
        catch { }
    }

    protected void edulevel_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            Bindcourse();
            binddept();
        }
        catch { }
    }

    protected void batch_SelectedIndexChange(object sender, EventArgs e)
    {

    }

    protected void cbdegree_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbdegree.Checked == true)
            {
                for (i = 0; i < cbldegree.Items.Count; i++)
                {
                    cbldegree.Items[i].Selected = true;
                }
                txt_degree.Text = lbldeg.Text + "(" + Convert.ToString(cbldegree.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbldegree.Items.Count; i++)
                {
                    cbldegree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
            binddept();
        }
        catch { }
    }

    protected void cbldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_degree.Text = "--Select--";
            cbdegree.Checked = false;
            int count = 0;
            for (i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_degree.Text = lbldeg.Text + "(" + count + ")";
                if (count == cbldegree.Items.Count)
                {
                    cbdegree.Checked = true;
                }
            }
            binddept();
        }
        catch { }
    }

    protected void cbdepartment_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbdepartment1.Checked == true)
            {
                for (i = 0; i < cbldepartment.Items.Count; i++)
                {
                    cbldepartment.Items[i].Selected = true;
                }
                txt_department.Text = lblBran.Text + "(" + Convert.ToString(cbldepartment.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbldepartment.Items.Count; i++)
                {
                    cbldepartment.Items[i].Selected = false;
                }
                txt_department.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void cbldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_department.Text = "--Select--";
            cbdepartment1.Checked = false;
            int count = 0;
            for (i = 0; i < cbldepartment.Items.Count; i++)
            {
                if (cbldepartment.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_department.Text = lblBran.Text + "(" + count + ")";
                if (count == cbldepartment.Items.Count)
                {
                    cbdepartment1.Checked = true;
                }
            }
        }
        catch { }
    }

    public bool checkok()
    {
        bool check = false;
        FpSpread.SaveChanges();
        try
        {
            for (i = 1; i < FpSpread.Sheets[0].Rows.Count; i++)
            {
                byte selval = Convert.ToByte(FpSpread.Sheets[0].Cells[i, 1].Value);
                if (selval == 1)
                {
                    check = true;
                }
            }
        }
        catch { }
        return check;
    }

    protected void Fpspread_command(object sender, EventArgs e)
    {
        try
        {
            FpSpread.SaveChanges();
            string selval = Convert.ToString(FpSpread.Sheets[0].Cells[0, 1].Value);
            if (selval == "1")
            {
                for (i = 1; i < FpSpread.Sheets[0].Rows.Count; i++)
                {
                    FpSpread.Sheets[0].Cells[i, 1].Value = 1;
                }
            }
            else
            {
                for (i = 1; i < FpSpread.Sheets[0].Rows.Count; i++)
                {
                    FpSpread.Sheets[0].Cells[i, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string degreecode = "";
            string batchyear = "";
            string[] spl = new string[2];
            degreecode = GetSelectedItemsValueAsString(cbldepartment);
            batchyear = Convert.ToString(ddlbatch.SelectedItem.Text);
            string frmdate = Convert.ToString(txtfrmdate.Text);
            string todate = Convert.ToString(txttodate.Text);
            spl = frmdate.Split('/');
            DateTime dt = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
            spl = todate.Split('/');
            DateTime dt1 = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);

            string selquery = "select app_formno,stud_name,batch_year,(c.Course_Name+' - '+Dept_Name) as Dept_Name from applyn a,Degree d,Department dt,Course C where isconfirm='1' and admission_status ='1' and selection_status ='1' and is_enroll ='1' and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and batch_year ='" + batchyear + "' and a.degree_code in ('" + degreecode + "') and admitcard_date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                FpSpread.Sheets[0].RowCount = 0;
                FpSpread.Sheets[0].ColumnCount = 6;
                FpSpread.Sheets[0].AutoPostBack = false;
                FpSpread.CommandBar.Visible = false;
                FpSpread.Sheets[0].RowHeader.Visible = false;
                FpSpread.Sheets[0].FrozenRowCount = 1;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread.Columns[0].Locked = true;
                FpSpread.Columns[0].Width = 50;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread.Columns[1].Width = 80;

                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell.AutoPostBack = false;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "App Form No";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread.Columns[2].Locked = true;
                FpSpread.Columns[2].Width = 125;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread.Columns[3].Locked = true;
                FpSpread.Columns[3].Width = 175;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Batch Year";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread.Columns[4].Locked = true;
                FpSpread.Columns[4].Width = 175;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = lbldeg.Text + "/" + lblBran.Text;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread.Columns[5].Locked = true;
                FpSpread.Columns[5].Width = 200;

                FpSpread.Sheets[0].RowCount++;
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[0, 1].CellType = chkall;
                    FpSpread.Sheets[0].Cells[0, 1].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[0, 1].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_formno"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["batch_year"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                }
                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                mainpgeerr.Visible = false;
                FpSpread.Visible = true;
                btncoverprint.Visible = true;
                btninsurprnt.Visible = true;
                rprint.Visible = true;
                FpSpread.Height = 500;
                FpSpread.Width = 820;
            }
            else
            {
                FpSpread.Visible = false;
                btncoverprint.Visible = false;
                btninsurprnt.Visible = false;
                rprint.Visible = false;
                mainpgeerr.Visible = true;
                mainpgeerr.Text = "No Record Found!";
            }
        }
        catch { }
    }

    //protected void btncoverprint_click(object sender, EventArgs ewi)
    //{
    //    try
    //    {
    //        if (checkok() == true)
    //        {
    //            int headalign = 0;
    //            int pdfheight = 0;
    //            Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
    //            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
    //            Font Fontsmalltbl = new Font("Times New Roman", 8, FontStyle.Regular);
    //            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
    //            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);   //InCentimeters(60, 40)
    //            headalign = 1655;
    //            pdfheight = 1000;
    //            Gios.Pdf.PdfPage mypage;

    //            for (i = 1; i < FpSpread.Sheets[0].RowCount; i++)
    //            {
    //                FpSpread.SaveChanges();
    //                string val = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Value);
    //                if (val == "1")
    //                {
    //                    string appformno = Convert.ToString(FpSpread.Sheets[0].Cells[i, 2].Text);
    //                    string app_no = d2.GetFunction("select app_no from applyn where app_formno='" + appformno + "' and college_code='" + collegecode1 + "'");
    //                    mypage = mydoc.NewPage();

    //                    #region for CollegeDetails
    //                    //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/MccLeft_Logo.jpeg")))
    //                    //{
    //                    //    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/MccLeft_Logo.jpeg"));
    //                    //    mypage.Add(LogoImage, 25, 25, 400);
    //                    //}
    //                    //if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/MccLeft_Logo.jpeg")))
    //                    //{
    //                    //    MemoryStream memoryStream = new MemoryStream();
    //                    //    string sellogo = "select logo1,logo2 from collinfo where college_code='" + Session["collegecode"] + "'";
    //                    //    ds.Clear();
    //                    //    ds = d2.select_method_wo_parameter(sellogo, "Text");
    //                    //    if (ds.Tables.Count > 0)
    //                    //    {
    //                    //        if (ds.Tables[0].Rows.Count > 0)
    //                    //        {
    //                    //            byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
    //                    //            memoryStream.Write(file, 0, file.Length);
    //                    //            if (file.Length > 0)
    //                    //            {
    //                    //                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
    //                    //                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
    //                    //                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + file + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                    //            }
    //                    //            memoryStream.Dispose();
    //                    //            memoryStream.Close();
    //                    //        }
    //                    //    }
    //                    //}

    //                    //string collquery = "";
    //                    //collquery = "select collname,category,university,address1,address2,address3,phoneno,faxno,email,website,district,state,pincode  from collinfo where college_Code=" + collegecode1 + "";
    //                    //ds.Clear();
    //                    //ds = d2.select_method_wo_parameter(collquery, "Text");
    //                    //string collegename = "";
    //                    //string collegeaddress = "";
    //                    //string collegedistrict = "";
    //                    //string phonenumber = "";
    //                    //string fax = "";
    //                    //string email = "";
    //                    //string website = "";
    //                    //if (ds.Tables[0].Rows.Count > 0)
    //                    //{
    //                    //    collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]) + "(" + Convert.ToString(ds.Tables[0].Rows[0]["category"]) + ")";
    //                    //    collegeaddress = Convert.ToString(ds.Tables[0].Rows[0]["address1"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address2"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
    //                    //    collegedistrict = Convert.ToString(ds.Tables[0].Rows[0]["district"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["state"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
    //                    //    phonenumber = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
    //                    //    fax = Convert.ToString(ds.Tables[0].Rows[0]["faxno"]); ;
    //                    //    email = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
    //                    //    website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
    //                    //}

    //                    //PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                    //                                       new PdfArea(mydoc, 0, 20, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
    //                    //mypage.Add(ptc);
    //                    //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                    //                                                   new PdfArea(mydoc, 0, 30, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, collegeaddress);
    //                    //mypage.Add(ptc);
    //                    //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                    //                                                   new PdfArea(mydoc, 0, 40, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, collegedistrict);
    //                    //mypage.Add(ptc);
    //                    //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                    //                                                   new PdfArea(mydoc, 0, 50, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phone No: " + phonenumber + ", Fax:" + fax);
    //                    //mypage.Add(ptc);
    //                    //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                    //                                                   new PdfArea(mydoc, 0, 60, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, email);
    //                    //mypage.Add(ptc);
    //                    //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                    //                                                   new PdfArea(mydoc, 0, 70, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, website);

    //                    //mypage.Add(ptc);
    //                    //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
    //                    //                                         new PdfArea(mydoc, 450, 85, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Application Form for Insurance");
    //                    //mypage.Add(ptc);
    //                    #endregion

    //                    int y = 0;
    //                    int line1 = 75;
    //                    int line2 = 150;

    //                    PdfTextArea ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, line1, y + 30, 50, 30), System.Drawing.ContentAlignment.MiddleLeft, "S.No.");
    //                    mypage.Add(ptc);

    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, line1 + 30, y + 30, 75, 30), System.Drawing.ContentAlignment.MiddleLeft, "_________________________");
    //                    mypage.Add(ptc);

    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, line2 + 175, y + 30, 75, 30), System.Drawing.ContentAlignment.MiddleLeft, "Reg.No.");
    //                    mypage.Add(ptc);

    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, line2 + 215, y + 30, 75, 30), System.Drawing.ContentAlignment.MiddleLeft, "_________________________");
    //                    mypage.Add(ptc);

    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, line2 + 450, y + 30, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, "Hall");
    //                    mypage.Add(ptc);

    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, line2 + 475, y + 30, 75, 30), System.Drawing.ContentAlignment.MiddleLeft, "_________________________");
    //                    mypage.Add(ptc);

    //                    string degreecode = GetSelectedItemsValueAsString(cbldepartment);
    //                    string getstudinfn = "select stud_name,c.Course_Name,sex,age,Convert(varchar(10),dob,103) as dob,dob as dob1,bldgrp,idmark,Dept_Name,batch_year,mother,parent_income,motherocc,mIncome,parent_occu,citizen,mother_tongue,StuPer_Id,community,religion,guardian_name,Guardian_income,Guardian_occ,Convert(varchar(10),Guardiandob,103) as Guardiandob,CONVERT(varchar(10),date_applied,103) as admitdate,Student_Mobile,Convert(varchar(10),fatherdob,103) as fatherdob,Convert(varchar(10),motherdob,103) as motherdob,isdisable,parent_name,parent_addressP,Streetp,cityp,IsExService,isdisable,handy,visualhandy,islearningdis,isdisabledisc,SubCaste,parent_addressC,Streetc,parent_statec,Cityc,parent_pincodec,parent_pincodep,parent_statep,visualhandy from applyn a,Degree d,Department dt,Course C where isconfirm='1' and admission_status ='1' and selection_status ='1' and is_enroll ='1' and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and batch_year ='" + Convert.ToString(ddlbatch.SelectedItem.Text) + "' and a.degree_code in ('" + degreecode + "') and app_formno='" + appformno + "'";

    //                    ds.Clear();
    //                    ds = d2.select_method_wo_parameter(getstudinfn, "Text");
    //                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        string gender = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "0")
    //                        {
    //                            gender = "Male";
    //                        }
    //                        else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "1")
    //                        {
    //                            gender = "Female";
    //                        }
    //                        else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "2")
    //                        {
    //                            gender = "TransGender";
    //                        }
    //                        else
    //                        {
    //                            gender = "";
    //                        }
    //                        string nationality = "";
    //                        string getnatio = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["citizen"]), "citi");
    //                        if (getnatio.Trim() != "" && getnatio.Trim() != "0")
    //                        {
    //                            nationality = getnatio;
    //                        }
    //                        string religion = "";
    //                        string getreligion = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["religion"]), "relig");
    //                        if (getreligion.Trim() != "" && getreligion.Trim() != "0")
    //                        {
    //                            religion = getreligion;
    //                        }
    //                        string state = "";
    //                        string getstate = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_statep"]), "state");
    //                        if (getstate.Trim() != "" && getstate.Trim() != "0")
    //                        {
    //                            state = getstate;
    //                        }
    //                        string mothertongue = "";
    //                        string getmotherton = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["mother_tongue"]), "mton");
    //                        if (getmotherton.Trim() != "" && getmotherton.Trim() != "0")
    //                        {
    //                            mothertongue = getmotherton;
    //                        }
    //                        string community = "";
    //                        string getcommunity = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["community"]), "comm");
    //                        if (getcommunity.Trim() != "" && getcommunity.Trim() != "0")
    //                        {
    //                            community = getcommunity;
    //                        }
    //                        string bloodgroup = "";
    //                        string getbloodgrp = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["bldgrp"]), "bgrou");
    //                        if (getbloodgrp.Trim() != "" && getbloodgrp.Trim() != "0")
    //                        {
    //                            bloodgroup = getbloodgrp;
    //                        }
    //                        string email = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]).Trim() != null)
    //                        {
    //                            email = Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]);
    //                        }
    //                        string admitdate = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["admitdate"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["admitdate"]).Trim() != null)
    //                        {
    //                            admitdate = Convert.ToString(ds.Tables[0].Rows[0]["admitdate"]);
    //                        }
    //                        string fathername = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]).Trim() != null)
    //                        {
    //                            fathername = Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]);
    //                        }
    //                        string fatherdob = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["fatherdob"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["fatherdob"]).Trim() != null)
    //                        {
    //                            fatherdob = Convert.ToString(ds.Tables[0].Rows[0]["fatherdob"]);
    //                        }
    //                        string fatherocc = "";
    //                        string getfatherocc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_occu"]), "foccu");
    //                        if (getfatherocc.Trim() != "" && getfatherocc.Trim() != "0")
    //                        {
    //                            fatherocc = getfatherocc;
    //                        }
    //                        string fatherinc = "";
    //                        string getfatherinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_income"]), "fin");
    //                        if (getfatherinc.Trim() != "" && getfatherinc.Trim() != "0")
    //                        {
    //                            fatherinc = getfatherinc;
    //                        }
    //                        string mothername = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["mother"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["mother"]).Trim() != null)
    //                        {
    //                            mothername = Convert.ToString(ds.Tables[0].Rows[0]["mother"]);
    //                        }
    //                        string motherdob = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["motherdob"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["motherdob"]).Trim() != null)
    //                        {
    //                            motherdob = Convert.ToString(ds.Tables[0].Rows[0]["motherdob"]);
    //                        }
    //                        string motherocc = "";
    //                        string getmotherocc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["motherocc"]), "moccu");
    //                        if (getmotherocc.Trim() != "" && getmotherocc.Trim() != "0")
    //                        {
    //                            motherocc = getmotherocc;
    //                        }
    //                        string motherinc = "";
    //                        string getmmotherinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["mIncome"]), "min");
    //                        if (getmmotherinc.Trim() != "" && getmmotherinc.Trim() != "0")
    //                        {
    //                            motherinc = getmmotherinc;
    //                        }
    //                        string guardianname = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]).Trim() != null)
    //                        {
    //                            guardianname = Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]);
    //                        }
    //                        string guardiandob = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["Guardiandob"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["Guardiandob"]).Trim() != null)
    //                        {
    //                            guardiandob = Convert.ToString(ds.Tables[0].Rows[0]["Guardiandob"]);
    //                        }
    //                        string guardianocc = "";
    //                        string getguardocc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["Guardian_occ"]), "moccu");
    //                        if (getguardocc.Trim() != "" && getguardocc.Trim() != "0")
    //                        {
    //                            guardianocc = getguardocc;
    //                        }
    //                        string guardianinc = "";
    //                        string getguardinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["Guardian_income"]), "min");
    //                        if (getguardinc.Trim() != "" && getguardinc.Trim() != "0")
    //                        {
    //                            guardianinc = getguardinc;
    //                        }
    //                        string xservice = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]).Trim() == "0")
    //                        {
    //                            xservice = "No";
    //                        }
    //                        else if (Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]).Trim() == "1")
    //                        {
    //                            xservice = "Yes";
    //                        }
    //                        else
    //                        {
    //                            xservice = "";
    //                        }
    //                        string isdisable = "";
    //                        string handy = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["isdisable"]).Trim() == "0")
    //                        {
    //                            isdisable = "No";
    //                        }
    //                        else if (Convert.ToString(ds.Tables[0].Rows[0]["isdisable"]).Trim() == "1")
    //                        {
    //                            isdisable = "Yes";
    //                            if (Convert.ToString(ds.Tables[0].Rows[0]["handy"]).Trim() == "1")
    //                            {
    //                                handy = "Physically";
    //                            }
    //                            else if (Convert.ToString(ds.Tables[0].Rows[0]["visualhandy"]).Trim() == "1")
    //                            {
    //                                handy = "Visually";
    //                            }
    //                            else if (Convert.ToString(ds.Tables[0].Rows[0]["islearningdis"]).Trim() == "1")
    //                            {
    //                                handy = "Learning Disable";
    //                            }
    //                            else
    //                            {
    //                                handy = Convert.ToString(ds.Tables[0].Rows[0]["isdisabledisc"]);
    //                            }
    //                        }

    //                        string subcaste = "";
    //                        string getsubcaste = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["SubCaste"]), "caste");
    //                        if (getsubcaste.Trim() != "" && getsubcaste.Trim() != "0")
    //                        {
    //                            subcaste = getsubcaste;
    //                        }

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                           new PdfArea(mydoc, line1, y + 50, 150, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                           new PdfArea(mydoc, line2 + 175, y + 50, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                           new PdfArea(mydoc, line2 + 275, y + 50, 150, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                           new PdfArea(mydoc, line2 + 475, y + 50, 150, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]));
    //                        mypage.Add(ptc);

    //                        DataSet dsmarks = new DataSet();
    //                        Dictionary<string, double> dicmarks = new Dictionary<string, double>();
    //                        Double Marks = 0;
    //                        Double totmarks = 0;
    //                        if (ddledulevel.SelectedItem.Text.Trim().ToUpper() == "UG")
    //                        {
    //                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                               new PdfArea(mydoc, line1 + 30, y + 70, 300, 30), System.Drawing.ContentAlignment.MiddleLeft, "MARKS OF LAST EXAMINATION PASSED");
    //                            mypage.Add(ptc);

    //                            string getmarks = "SELECT T.TextVal,acual_marks,T.TextCode from Stud_prev_details s,perv_marks_history p,TextValTable t where s.course_entno =p.course_entno  and t.TextCode =p.psubjectno and s.app_no ='" + app_no + "'";
    //                            dsmarks.Clear();
    //                            dsmarks = d2.select_method_wo_parameter(getmarks, "Text");
    //                            int col = 0;
    //                            if (dsmarks.Tables.Count > 0 && dsmarks.Tables[0].Rows.Count > 0)
    //                            {
    //                                for (int ik = 0; ik < dsmarks.Tables[0].Rows.Count; ik++)
    //                                {
    //                                    if (!dicmarks.ContainsKey(Convert.ToString(dsmarks.Tables[0].Rows[ik]["TextVal"])))
    //                                    {
    //                                        Double.TryParse(Convert.ToString(dsmarks.Tables[0].Rows[ik]["acual_marks"]), out Marks);
    //                                        dicmarks.Add(Convert.ToString(dsmarks.Tables[0].Rows[ik]["TextVal"]), Marks);
    //                                        totmarks = totmarks + Marks;
    //                                    }
    //                                }
    //                                Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, 2, dicmarks.Count + 1, 1);
    //                                table2.VisibleHeaders = false;
    //                                table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

    //                                for (int ro = 0; ro < 2; ro++)
    //                                {
    //                                    col = 0;
    //                                    foreach (var dr in dicmarks)
    //                                    {
    //                                        if (ro == 0)
    //                                        {
    //                                            table2.Columns[col].SetWidth(75);
    //                                            table2.Cell(ro, col).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            table2.Cell(ro, col).SetContent(dr.Key.ToString());
    //                                            col++;
    //                                        }
    //                                        else
    //                                        {
    //                                            table2.Cell(ro, col).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            table2.Cell(ro, col).SetContent(dr.Value.ToString());
    //                                            col++;
    //                                        }
    //                                    }
    //                                }
    //                                table2.Cell(0, dicmarks.Count).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                table2.Cell(1, dicmarks.Count).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                table2.Columns[dicmarks.Count].SetWidth(75);
    //                                table2.Cell(0, dicmarks.Count).SetContent("Total");
    //                                table2.Cell(1, dicmarks.Count).SetContent(Convert.ToString(totmarks));

    //                                table2.CellRange(0, 0, 1, dicmarks.Count).SetFont(Fontsmall);
    //                                Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, line1 + 40, y + 110, 500, 200));
    //                                mypage.Add(myprov_pdfpage1);

    //                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                               new PdfArea(mydoc, line1, y + 108, 75, 30), System.Drawing.ContentAlignment.MiddleLeft, "Subject");
    //                                mypage.Add(ptc);

    //                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                               new PdfArea(mydoc, line1, y + 122, 75, 30), System.Drawing.ContentAlignment.MiddleLeft, "Marks");
    //                                mypage.Add(ptc);
    //                            }
    //                        }

    //                        PdfImage LogoImage2;
    //                        string stdphtsql = "select * from StdPhoto where app_no='" + app_no + "'";
    //                        MemoryStream memoryStream = new MemoryStream();
    //                        DataSet dsstdpho = new DataSet();
    //                        dsstdpho.Clear();
    //                        dsstdpho.Dispose();
    //                        dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
    //                        if (dsstdpho.Tables[0].Rows.Count > 0)
    //                        {
    //                            byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
    //                            memoryStream.Write(file, 0, file.Length);
    //                            if (file.Length > 0)
    //                            {
    //                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
    //                                System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
    //                                if (File.Exists(HttpContext.Current.Server.MapPath("~/StudentImage/" + app_no + ".jpeg")))
    //                                {

    //                                }
    //                                else
    //                                {
    //                                    thumb.Save(HttpContext.Current.Server.MapPath("~/StudentImage/" + app_no + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                }
    //                            }
    //                        }

    //                        if (File.Exists(HttpContext.Current.Server.MapPath("~/StudentImage/" + app_no + ".jpeg")))
    //                        {
    //                            LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/StudentImage/" + app_no + ".jpeg"));
    //                            mypage.Add(LogoImage2, line2 + 550, y + 110, 420);
    //                        }
    //                        else
    //                        {

    //                        }

    //                        Gios.Pdf.PdfTable table3 = mydoc.NewTable(Fontsmalltbl, 18, 7, 4);
    //                        table3.VisibleHeaders = false;
    //                        table3.SetBorders(Color.Black, 1, BorderType.None);

    //                        table3.Columns[0].SetWidth(175);
    //                        table3.Columns[1].SetWidth(100);
    //                        table3.Columns[2].SetWidth(100);
    //                        table3.Columns[3].SetWidth(100);
    //                        table3.Columns[4].SetWidth(175);
    //                        table3.Columns[5].SetWidth(175);
    //                        table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(0, 0).SetContent("Date of Birth in Christian Era:");

    //                        table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(1, 0).SetContent("Gender:");

    //                        table3.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(2, 0).SetContent("Nationality & Religion:");

    //                        table3.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(3, 0).SetContent("State:");

    //                        table3.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(4, 0).SetContent("Mother Tongue:");

    //                        table3.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(5, 0).SetContent("Community:");

    //                        table3.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(6, 0).SetContent("Sub Caste:");

    //                        table3.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(7, 0).SetContent("Physically / Visually Challenged Specify:");

    //                        table3.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(8, 0).SetContent("Ex-Serviceman's Son / Daughter:");

    //                        table3.Cell(9, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(9, 0).SetContent("Blood Group:");

    //                        table3.Cell(10, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(10, 0).SetContent("Email Address:");

    //                        table3.Cell(11, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(11, 0).SetContent("Part I Language in UG:");

    //                        table3.Cell(12, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(12, 0).SetContent("Date Of Admission:");

    //                        table3.Cell(13, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(13, 0).SetContent("Temporary Residential Address & Phone:");

    //                        table3.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(2, 1).SetContent(nationality);

    //                        table3.Cell(10, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(10, 1).SetContent(email);

    //                        table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(0, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["dob"]));

    //                        table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(1, 2).SetContent(gender);

    //                        table3.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(2, 2).SetContent(religion);

    //                        table3.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(3, 2).SetContent(state);

    //                        table3.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(4, 2).SetContent(mothertongue);

    //                        table3.Cell(5, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(5, 2).SetContent(community);

    //                        table3.Cell(6, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(6, 2).SetContent(subcaste);

    //                        table3.Cell(7, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(7, 1).SetContent(isdisable);

    //                        table3.Cell(7, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(7, 2).SetContent(handy);

    //                        table3.Cell(8, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(8, 2).SetContent(xservice);

    //                        table3.Cell(9, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(9, 2).SetContent(bloodgroup);

    //                        table3.Cell(11, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(11, 2).SetContent("");

    //                        table3.Cell(12, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(12, 2).SetContent(admitdate);

    //                        table3.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(0, 3).SetContent("Father's Name");

    //                        table3.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(1, 3).SetContent("Date of Birth & Age");

    //                        table3.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(2, 3).SetContent("Occupation");

    //                        table3.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(3, 3).SetContent("Monthly Income");

    //                        table3.Cell(4, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(4, 3).SetContent("Mother's Name");

    //                        table3.Cell(5, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(5, 3).SetContent("Date of Birth & Age");

    //                        table3.Cell(6, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(6, 3).SetContent("Occupation");

    //                        table3.Cell(7, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(7, 3).SetContent("Monthly Income");

    //                        table3.Cell(8, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(8, 3).SetContent("Guardian's Name");

    //                        table3.Cell(9, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(9, 3).SetContent("Date of Birth & Age");

    //                        table3.Cell(10, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(10, 3).SetContent("Occupation");

    //                        table3.Cell(11, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(11, 3).SetContent("Monthly Income");

    //                        table3.Cell(13, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(13, 3).SetContent("Permanent Residential Address & Phone");

    //                        table3.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(0, 4).SetContent(fathername);

    //                        table3.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(1, 4).SetContent(fatherdob);

    //                        table3.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(2, 4).SetContent(fatherocc);

    //                        table3.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(3, 4).SetContent(fatherinc);

    //                        table3.Cell(4, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(4, 4).SetContent(mothername);

    //                        table3.Cell(5, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(5, 4).SetContent(motherdob);

    //                        table3.Cell(6, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(6, 4).SetContent(motherocc);

    //                        table3.Cell(7, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(7, 4).SetContent(motherinc);

    //                        table3.Cell(8, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(8, 4).SetContent(guardianname);

    //                        table3.Cell(9, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(9, 4).SetContent(guardiandob);

    //                        table3.Cell(10, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(10, 4).SetContent(guardianocc);

    //                        table3.Cell(11, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(11, 4).SetContent(guardianinc);

    //                        table3.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(0, 5).SetContent("Father's Office Address & Phone");

    //                        table3.Cell(4, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(4, 5).SetContent("Mother's Office Address & Phone");

    //                        table3.Cell(8, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(8, 5).SetContent("Guardian's Office Address & Phone");

    //                        table3.Cell(13, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(13, 5).SetContent("Guardian's Residential Address & Phone:");

    //                        table3.Cell(14, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(14, 0).SetContent(ds.Tables[0].Rows[0]["parent_addressC"]);

    //                        table3.Cell(15, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(15, 0).SetContent(ds.Tables[0].Rows[0]["Streetc"]);

    //                        table3.Cell(16, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(16, 0).SetContent(ds.Tables[0].Rows[0]["Cityc"]);

    //                        table3.Cell(16, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(16, 1).SetContent(ds.Tables[0].Rows[0]["parent_pincodec"]);

    //                        table3.Cell(17, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(17, 0).SetContent(ds.Tables[0].Rows[0]["parent_statec"]);

    //                        table3.Cell(14, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(14, 3).SetContent(ds.Tables[0].Rows[0]["parent_addressP"]);

    //                        table3.Cell(15, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(15, 3).SetContent(ds.Tables[0].Rows[0]["Streetp"]);

    //                        table3.Cell(16, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(16, 3).SetContent(ds.Tables[0].Rows[0]["cityp"]);

    //                        table3.Cell(16, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(16, 4).SetContent(ds.Tables[0].Rows[0]["parent_pincodep"]);

    //                        table3.Cell(17, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table3.Cell(17, 3).SetContent(ds.Tables[0].Rows[0]["parent_statep"]);

    //                        PdfRectangle pdfrect = new PdfRectangle(mydoc, new PdfArea(mydoc, line1, y + 180, 700, 325), Color.Black);
    //                        mypage.Add(pdfrect);

    //                        PdfLine pdfcolin1 = new PdfLine(mydoc, new PointF(line1 + 300, y + 180), new PointF(line1 + 300, y + 505), Color.Black, 1);
    //                        mypage.Add(pdfcolin1);

    //                        PdfLine pdfcolin2 = new PdfLine(mydoc, new PointF(line1 + 400, y + 180), new PointF(line1 + 400, y + 408), Color.Black, 1);
    //                        mypage.Add(pdfcolin2);

    //                        PdfLine pdfcolin3 = new PdfLine(mydoc, new PointF(line1 + 540, y + 180), new PointF(line1 + 540, y + 505), Color.Black, 1);
    //                        mypage.Add(pdfcolin3);

    //                        PdfLine pdfroin1 = new PdfLine(mydoc, new PointF(line1, y + 195), new PointF(line1 + 700, y + 195), Color.Black, 1);
    //                        mypage.Add(pdfroin1);

    //                        PdfLine pdfroin2 = new PdfLine(mydoc, new PointF(line1, y + 210), new PointF(line1 + 540, y + 210), Color.Black, 1);
    //                        mypage.Add(pdfroin2);

    //                        PdfLine pdfroin3 = new PdfLine(mydoc, new PointF(line1, y + 227), new PointF(line1 + 540, y + 227), Color.Black, 1);
    //                        mypage.Add(pdfroin3);

    //                        PdfLine pdfroin4 = new PdfLine(mydoc, new PointF(line1, y + 242), new PointF(line1 + 700, y + 242), Color.Black, 1);
    //                        mypage.Add(pdfroin4);

    //                        PdfLine pdfroin5 = new PdfLine(mydoc, new PointF(line1, y + 258), new PointF(line1 + 700, y + 258), Color.Black, 1);
    //                        mypage.Add(pdfroin5);

    //                        PdfLine pdfroin6 = new PdfLine(mydoc, new PointF(line1, y + 274), new PointF(line1 + 540, y + 274), Color.Black, 1);
    //                        mypage.Add(pdfroin6);

    //                        PdfLine pdfroin7 = new PdfLine(mydoc, new PointF(line1, y + 290), new PointF(line1 + 540, y + 290), Color.Black, 1);
    //                        mypage.Add(pdfroin7);

    //                        PdfLine pdfroin8 = new PdfLine(mydoc, new PointF(line1, y + 307), new PointF(line1 + 700, y + 307), Color.Black, 1);
    //                        mypage.Add(pdfroin8);

    //                        PdfLine pdfroin9 = new PdfLine(mydoc, new PointF(line1, y + 323), new PointF(line1 + 700, y + 323), Color.Black, 1);
    //                        mypage.Add(pdfroin9);

    //                        PdfLine pdfroin10 = new PdfLine(mydoc, new PointF(line1, y + 338), new PointF(line1 + 540, y + 338), Color.Black, 1);
    //                        mypage.Add(pdfroin10);

    //                        PdfLine pdfroin11 = new PdfLine(mydoc, new PointF(line1, y + 355), new PointF(line1 + 540, y + 355), Color.Black, 1);
    //                        mypage.Add(pdfroin11);

    //                        PdfLine pdfroin12 = new PdfLine(mydoc, new PointF(line1, y + 372), new PointF(line1 + 540, y + 372), Color.Black, 1);
    //                        mypage.Add(pdfroin12);

    //                        PdfLine pdfroin13 = new PdfLine(mydoc, new PointF(line1, y + 388), new PointF(line1 + 700, y + 388), Color.Black, 1);
    //                        mypage.Add(pdfroin13);

    //                        PdfLine pdfroin14 = new PdfLine(mydoc, new PointF(line1, y + 410), new PointF(line1 + 700, y + 410), Color.Black, 1);
    //                        mypage.Add(pdfroin14);

    //                        Gios.Pdf.PdfTablePage myprov_pdfpage2 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, line1, y + 180, 700, 350));
    //                        mypage.Add(myprov_pdfpage2);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                               new PdfArea(mydoc, line1, y + 530, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Student");
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                               new PdfArea(mydoc, line2 + 215, y + 530, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Parent");
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                               new PdfArea(mydoc, line2 + 475, y + 530, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Guardian");
    //                        mypage.Add(ptc);
    //                    }
    //                    mypage.SaveToDocument();
    //                }
    //            }
    //            string appPath = HttpContext.Current.Server.MapPath("~");
    //            if (appPath != "")
    //            {
    //                string szPath = appPath + "/Report/";
    //                string szFile = "CoverFormat" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
    //                mydoc.SaveToFile(szPath + szFile);

    //                Response.ClearHeaders();
    //                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
    //                Response.ContentType = "application/pdf";
    //                Response.WriteFile(szPath + szFile);
    //                Response.End();
    //            }
    //            mainpgeerr.Visible = false;
    //        }
    //    }
    //    catch { }
    //}


    protected void btncoverprint_click(object sender, EventArgs ewi)
    {
        try
        {
            if (checkok() == true)
            {
                int headalign = 0;
                int pdfheight = 0;
                Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
                Font Fontsmall = new Font("Times New Roman", 12, FontStyle.Regular);
                Font Fontsmalltbl = new Font("Times New Roman", 10, FontStyle.Regular);
                Font Fontsmalltblbold = new Font("Times New Roman", 10, FontStyle.Bold);
                Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);   //InCentimeters(60, 40)
                headalign = 1655;
                pdfheight = 1000;
                Gios.Pdf.PdfPage mypage;

                for (i = 1; i < FpSpread.Sheets[0].RowCount; i++)
                {
                    FpSpread.SaveChanges();
                    string val = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Value);
                    if (val == "1")
                    {
                        string appformno = Convert.ToString(FpSpread.Sheets[0].Cells[i, 2].Text);
                        string app_no = d2.GetFunction("select app_no from applyn where app_formno='" + appformno + "' and college_code='" + collegecode1 + "'");
                        mypage = mydoc.NewPage();

                        #region for CollegeDetails
                        //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/MccLeft_Logo.jpeg")))
                        //{
                        //    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/MccLeft_Logo.jpeg"));
                        //    mypage.Add(LogoImage, 25, 25, 400);
                        //}
                        //if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/MccLeft_Logo.jpeg")))
                        //{
                        //    MemoryStream memoryStream = new MemoryStream();
                        //    string sellogo = "select logo1,logo2 from collinfo where college_code='" + Session["collegecode"] + "'";
                        //    ds.Clear();
                        //    ds = d2.select_method_wo_parameter(sellogo, "Text");
                        //    if (ds.Tables.Count > 0)
                        //    {
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                        //            memoryStream.Write(file, 0, file.Length);
                        //            if (file.Length > 0)
                        //            {
                        //                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                        //                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                        //                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + file + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                        //            }
                        //            memoryStream.Dispose();
                        //            memoryStream.Close();
                        //        }
                        //    }
                        //}

                        //string collquery = "";
                        //collquery = "select collname,category,university,address1,address2,address3,phoneno,faxno,email,website,district,state,pincode  from collinfo where college_Code=" + collegecode1 + "";
                        //ds.Clear();
                        //ds = d2.select_method_wo_parameter(collquery, "Text");
                        //string collegename = "";
                        //string collegeaddress = "";
                        //string collegedistrict = "";
                        //string phonenumber = "";
                        //string fax = "";
                        //string email = "";
                        //string website = "";
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]) + "(" + Convert.ToString(ds.Tables[0].Rows[0]["category"]) + ")";
                        //    collegeaddress = Convert.ToString(ds.Tables[0].Rows[0]["address1"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address2"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                        //    collegedistrict = Convert.ToString(ds.Tables[0].Rows[0]["district"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["state"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
                        //    phonenumber = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                        //    fax = Convert.ToString(ds.Tables[0].Rows[0]["faxno"]); ;
                        //    email = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                        //    website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
                        //}

                        //PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                        //                                       new PdfArea(mydoc, 0, 20, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                        //mypage.Add(ptc);
                        //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 0, 30, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, collegeaddress);
                        //mypage.Add(ptc);
                        //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 0, 40, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, collegedistrict);
                        //mypage.Add(ptc);
                        //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 0, 50, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phone No: " + phonenumber + ", Fax:" + fax);
                        //mypage.Add(ptc);
                        //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 0, 60, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, email);
                        //mypage.Add(ptc);
                        //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 0, 70, 842, 30), System.Drawing.ContentAlignment.MiddleCenter, website);

                        //mypage.Add(ptc);
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                                         new PdfArea(mydoc, 450, 85, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Application Form for Insurance");
                        //mypage.Add(ptc);
                        #endregion

                        int y = 0;
                        int line1 = 25;
                        int line2 = 150;

                        PdfTextArea ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, line1, y + 30, 50, 30), System.Drawing.ContentAlignment.MiddleLeft, "S.No.");
                        mypage.Add(ptc);

                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, line1 + 33, y + 30, 50, 30), System.Drawing.ContentAlignment.MiddleLeft, "____________________");
                        mypage.Add(ptc);

                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, line2 + 175, y + 30, 75, 30), System.Drawing.ContentAlignment.MiddleLeft, "Reg.No.");
                        mypage.Add(ptc);

                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, line2 + 220, y + 30, 50, 30), System.Drawing.ContentAlignment.MiddleLeft, "______________________");
                        mypage.Add(ptc);

                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, line1 + 620, y + 30, 75, 30), System.Drawing.ContentAlignment.MiddleLeft, "Hall");
                        mypage.Add(ptc);

                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, line1 + 648, y + 30, 50, 30), System.Drawing.ContentAlignment.MiddleLeft, "____________________");
                        mypage.Add(ptc);

                        string degreecode = GetSelectedItemsValueAsString(cbldepartment);
                        string getstudinfn = "select stud_name,c.Course_Name,sex,age,Convert(varchar(10),dob,103) as dob,dob as dob1,bldgrp,idmark,Dept_Name,batch_year,mother,caste,parent_income,motherocc,mIncome,parent_occu,citizen,mother_tongue,StuPer_Id,community,religion,guardian_name,Guardian_income,Guardian_occ,Convert(varchar(10),Guardiandob,103) as Guardiandob,CONVERT(varchar(10),date_applied,103) as admitdate,Student_Mobile,Convert(varchar(10),fatherdob,103) as fatherdob,Convert(varchar(10),motherdob,103) as motherdob,isdisable,parent_name,parent_addressP,Streetp,cityp,IsExService,isdisable,handy,visualhandy,islearningdis,isdisabledisc,SubCaste,parent_addressC,Streetc,parent_statec,Cityc,parent_pincodec,parent_pincodep,parent_statep,addressg,Streetg,stateg,Cityg,ping,mot_off_address1,mot_off_address2,mot_off_state,mot_off_country,mot_off_pincode,gur_off_address1,gur_off_address2,gur_off_state,gur_off_country,gur_off_pincode,Fat_off_addressP,Fat_off_street,Fat_off_state,Fat_off_country,Fat_off_pincode,parentF_Mobile,parentM_Mobile,guardian_mobile,visualhandy,d.duration from applyn a,Degree d,Department dt,Course C where isconfirm='1' and admission_status ='1' and selection_status ='1' and is_enroll ='1' and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and batch_year ='" + Convert.ToString(ddlbatch.SelectedItem.Text) + "' and a.degree_code in ('" + degreecode + "') and app_formno='" + appformno + "'";

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(getstudinfn, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string gender = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "0")
                            {
                                gender = "Male";
                            }
                            else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "1")
                            {
                                gender = "Female";
                            }
                            else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "2")
                            {
                                gender = "TransGender";
                            }
                            else
                            {
                                gender = "";
                            }
                            string nationality = "";
                            string getnatio = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["citizen"]), "citi");
                            if (getnatio.Trim() != "" && getnatio.Trim() != "0")
                            {
                                nationality = getnatio;
                            }
                            string religion = "";
                            string getreligion = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["religion"]), "relig");
                            if (getreligion.Trim() != "" && getreligion.Trim() != "0")
                            {
                                religion = getreligion;
                            }
                            string state = "";
                            string getstate = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_statep"]), "state");
                            if (getstate.Trim() != "" && getstate.Trim() != "0")
                            {
                                state = getstate;
                            }
                            string mothertongue = "";
                            string getmotherton = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["mother_tongue"]), "mton");
                            if (getmotherton.Trim() != "" && getmotherton.Trim() != "0")
                            {
                                mothertongue = getmotherton;
                            }
                            string community = "";
                            string getcommunity = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["community"]), "comm");
                            if (getcommunity.Trim() != "" && getcommunity.Trim() != "0")
                            {
                                community = getcommunity;
                            }
                            string bloodgroup = "";
                            string getbloodgrp = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["bldgrp"]), "bgrou");
                            if (getbloodgrp.Trim() != "" && getbloodgrp.Trim() != "0")
                            {
                                bloodgroup = getbloodgrp;
                            }
                            string email = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]).Trim() != null)
                            {
                                email = Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]);
                            }
                            string admitdate = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["admitdate"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["admitdate"]).Trim() != null)
                            {
                                admitdate = Convert.ToString(ds.Tables[0].Rows[0]["admitdate"]);
                            }
                            string fathername = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]).Trim() != null)
                            {
                                fathername = Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]);
                            }
                            string fatherdob = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["fatherdob"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["fatherdob"]).Trim() != null)
                            {
                                fatherdob = Convert.ToString(ds.Tables[0].Rows[0]["fatherdob"]);
                            }
                            string fatherocc = "";
                            string getfatherocc = getnewtxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_occu"]), "foccu");
                            if (getfatherocc.Trim() != "" && getfatherocc.Trim() != "0")
                            {
                                fatherocc = getfatherocc;
                            }
                            string fatherinc = "";
                            string getfatherinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_income"]), "fin");
                            if (getfatherinc.Trim() != "" && getfatherinc.Trim() != "0")
                            {
                                fatherinc = getfatherinc;
                            }
                            string mothername = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["mother"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["mother"]).Trim() != null)
                            {
                                mothername = Convert.ToString(ds.Tables[0].Rows[0]["mother"]);
                            }
                            string motherdob = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["motherdob"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["motherdob"]).Trim() != null)
                            {
                                motherdob = Convert.ToString(ds.Tables[0].Rows[0]["motherdob"]);
                            }
                            string motherocc = "";
                            string getmotherocc = getnewtxtval(Convert.ToString(ds.Tables[0].Rows[0]["motherocc"]), "foccu");
                            if (getmotherocc.Trim() != "" && getmotherocc.Trim() != "0")
                            {
                                motherocc = getmotherocc;
                            }
                            string motherinc = "";
                            string getmmotherinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["mIncome"]), "min");
                            if (getmmotherinc.Trim() != "" && getmmotherinc.Trim() != "0")
                            {
                                motherinc = getmmotherinc;
                            }
                            string guardianname = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]).Trim() != null)
                            {
                                guardianname = Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]);
                            }
                            string guardiandob = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["Guardiandob"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["Guardiandob"]).Trim() != null)
                            {
                                guardiandob = Convert.ToString(ds.Tables[0].Rows[0]["Guardiandob"]);
                            }
                            string guardianocc = "";
                            string getguardocc = getnewtxtval(Convert.ToString(ds.Tables[0].Rows[0]["Guardian_occ"]), "foccu");
                            if (getguardocc.Trim() != "" && getguardocc.Trim() != "0")
                            {
                                guardianocc = getguardocc;
                            }
                            string guardianinc = "";
                            string getguardinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["Guardian_income"]), "fin");
                            if (getguardinc.Trim() != "" && getguardinc.Trim() != "0")
                            {
                                guardianinc = getguardinc;
                            }
                            string xservice = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]).Trim() == "0")
                            {
                                xservice = "No";
                            }
                            else if (Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]).Trim() == "1")
                            {
                                xservice = "Yes";
                            }
                            else
                            {
                                xservice = "";
                            }
                            string isdisable = "";
                            string handy = "";

                            if (Convert.ToString(ds.Tables[0].Rows[0]["isdisable"]).Trim() == "1")
                            {
                                isdisable = "Yes";
                                if (Convert.ToString(ds.Tables[0].Rows[0]["handy"]).Trim() == "1")
                                {
                                    handy = "Physically";
                                }
                                else if (Convert.ToString(ds.Tables[0].Rows[0]["visualhandy"]).Trim() == "1")
                                {
                                    handy = "Visually";
                                }
                                else if (Convert.ToString(ds.Tables[0].Rows[0]["islearningdis"]).Trim() == "1")
                                {
                                    handy = "Learning Disable";
                                }
                                else
                                {
                                    handy = Convert.ToString(ds.Tables[0].Rows[0]["isdisabledisc"]);
                                }
                            }
                            else
                            {
                                isdisable = "N/A";
                            }

                            string subcaste = "";
                            string getsubcaste = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["caste"]), "caste");
                            if (getsubcaste.Trim() != "" && getsubcaste.Trim() != "0")
                            {
                                subcaste = getsubcaste;
                            }
                            else
                            {
                                subcaste = "N/A";
                            }

                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, line1, y + 50, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, line2 + 175, y + 50, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, line2 + 225, y + 50, 150, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]));
                            mypage.Add(ptc);

                            double getduration = 0;
                            double duration = 0;
                            double getfrmyear = 0;
                            double gettoyear = 0;
                            string toyear = "";
                            Double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["duration"]), out getduration);
                            duration = getduration / 2;
                            Double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]), out getfrmyear);
                            gettoyear = getfrmyear + duration;
                            toyear = Convert.ToString(gettoyear).Remove(0, 2);
                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, line1 + 640, y + 50, 150, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(getfrmyear) + " - " + toyear);
                            mypage.Add(ptc);

                            DataSet dsmarks = new DataSet();
                            Dictionary<string, double> dicmarks = new Dictionary<string, double>();
                            Double Marks = 0;
                            Double totmarks = 0;
                            if (ddledulevel.SelectedItem.Text.Trim().ToUpper() == "UG")
                            {
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydoc, line1 + 30, y + 80, 300, 30), System.Drawing.ContentAlignment.MiddleLeft, "MARKS OF LAST EXAMINATION PASSED");
                                mypage.Add(ptc);

                                //string getmarks = "SELECT T.TextVal,acual_marks,T.TextCode from Stud_prev_details s,perv_marks_history p,TextValTable t where s.course_entno =p.course_entno  and t.TextCode =p.psubjectno and s.app_no ='" + app_no + "'";
                                string getmarks = " SELECT T.TextVal,acual_marks,T.TextCode from Stud_prev_details s,perv_marks_history p,TextValTable t where s.course_entno =p.course_entno  and t.TextCode =s.Part1Language and s.Part1Language =p.psubjectno and s.app_no='" + app_no + "' SELECT T.TextVal,acual_marks,T.TextCode from Stud_prev_details s,perv_marks_history p,TextValTable t where s.course_entno =p.course_entno  and t.TextCode =s.Part2Language and s.Part2Language =p.psubjectno and s.app_no='" + app_no + "' SELECT T.TextVal,acual_marks,T.TextCode from Stud_prev_details s,perv_marks_history p,TextValTable t where s.course_entno =p.course_entno  and t.TextCode =p.psubjectno and s.app_no='" + app_no + "' and p.psubjectno not in((SELECT T.TextCode from Stud_prev_details s,perv_marks_history p,TextValTable t where s.course_entno =p.course_entno  and t.TextCode =s.Part1Language and s.Part1Language =p.psubjectno and s.app_no='" + app_no + "'),(SELECT T.TextCode from Stud_prev_details s,perv_marks_history p,TextValTable t where s.course_entno =p.course_entno  and t.TextCode =s.Part2Language and s.Part2Language =p.psubjectno and s.app_no='" + app_no + "'))";
                                dsmarks.Clear();
                                dsmarks = d2.select_method_wo_parameter(getmarks, "Text");
                                int col = 0;
                                if (dsmarks.Tables.Count > 0 && dsmarks.Tables[0].Rows.Count > 0)
                                {
                                    for (int ik = 0; ik < dsmarks.Tables[0].Rows.Count; ik++)
                                    {
                                        if (!dicmarks.ContainsKey(Convert.ToString(dsmarks.Tables[0].Rows[ik]["TextVal"])))
                                        {
                                            Double.TryParse(Convert.ToString(dsmarks.Tables[0].Rows[ik]["acual_marks"]), out Marks);
                                            dicmarks.Add(Convert.ToString(dsmarks.Tables[0].Rows[ik]["TextVal"]), Marks);
                                            totmarks = totmarks + Marks;
                                        }
                                    }
                                    for (int ik = 0; ik < dsmarks.Tables[1].Rows.Count; ik++)
                                    {
                                        if (!dicmarks.ContainsKey(Convert.ToString(dsmarks.Tables[1].Rows[ik]["TextVal"])))
                                        {
                                            Double.TryParse(Convert.ToString(dsmarks.Tables[1].Rows[ik]["acual_marks"]), out Marks);
                                            dicmarks.Add(Convert.ToString(dsmarks.Tables[1].Rows[ik]["TextVal"]), Marks);
                                            totmarks = totmarks + Marks;
                                        }
                                    }
                                    for (int ik = 0; ik < dsmarks.Tables[2].Rows.Count; ik++)
                                    {
                                        if (!dicmarks.ContainsKey(Convert.ToString(dsmarks.Tables[2].Rows[ik]["TextVal"])))
                                        {
                                            Double.TryParse(Convert.ToString(dsmarks.Tables[2].Rows[ik]["acual_marks"]), out Marks);
                                            dicmarks.Add(Convert.ToString(dsmarks.Tables[2].Rows[ik]["TextVal"]), Marks);
                                            totmarks = totmarks + Marks;
                                        }
                                    }
                                    Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, 2, dicmarks.Count + 1, 3);
                                    table2.VisibleHeaders = false;
                                    table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                    for (int ro = 0; ro < 2; ro++)
                                    {
                                        col = 0;
                                        foreach (var dr in dicmarks)
                                        {
                                            if (ro == 0)
                                            {
                                                table2.Columns[col].SetWidth(100);
                                                table2.Cell(ro, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table2.Cell(ro, col).SetContent(dr.Key.ToString());
                                                col++;
                                            }
                                            else
                                            {
                                                table2.Cell(ro, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table2.Cell(ro, col).SetContent(dr.Value.ToString());
                                                col++;
                                            }
                                        }
                                    }
                                    table2.Cell(0, dicmarks.Count).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(1, dicmarks.Count).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Columns[dicmarks.Count].SetWidth(100);
                                    table2.Cell(0, dicmarks.Count).SetContent("Total");
                                    table2.Cell(1, dicmarks.Count).SetContent(Convert.ToString(totmarks));

                                    table2.CellRange(0, 0, 1, dicmarks.Count).SetFont(Fontsmall);
                                    Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, line1 + 40, y + 110, 570, 200));
                                    mypage.Add(myprov_pdfpage1);

                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydoc, line1, y + 108, 75, 30), System.Drawing.ContentAlignment.MiddleLeft, "Subject");
                                    mypage.Add(ptc);

                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydoc, line1, y + 122, 75, 30), System.Drawing.ContentAlignment.MiddleLeft, "Marks");
                                    mypage.Add(ptc);
                                }
                            }

                            PdfImage LogoImage2;
                            string stdphtsql = "select * from StdPhoto where app_no='" + app_no + "'";
                            MemoryStream memoryStream = new MemoryStream();
                            DataSet dsstdpho = new DataSet();
                            dsstdpho.Clear();
                            dsstdpho.Dispose();
                            dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
                            if (dsstdpho.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpeg")))
                                    {

                                    }
                                    else
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                }
                            }

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpeg")))
                            {
                                LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpeg"));
                                mypage.Add(LogoImage2, line2 + 530, y + 80, 300);
                            }
                            else
                            {

                            }

                            Gios.Pdf.PdfTable table3 = mydoc.NewTable(Fontsmalltbl, 18, 6, 3);
                            table3.VisibleHeaders = false;
                            table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                            table3.Columns[0].SetWidth(150);
                            table3.Columns[1].SetWidth(115);
                            table3.Columns[2].SetWidth(100);
                            table3.Columns[3].SetWidth(125);
                            table3.Columns[4].SetWidth(150);
                            table3.Columns[5].SetWidth(150);
                            table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(0, 0).SetContent("Date of Birth in Christian Era:");

                            table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(1, 0).SetContent("Gender:");

                            table3.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(2, 0).SetContent("Nationality & Religion:");

                            table3.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(3, 0).SetContent("State:");

                            table3.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(4, 0).SetContent("Mother Tongue:");

                            table3.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(5, 0).SetContent("Community:");

                            table3.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(6, 0).SetContent("Sub Caste:");

                            table3.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(7, 0).SetContent("Differently Abled? if Specify:");

                            table3.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(8, 0).SetContent("Ex-Serviceman's Son / Daughter:");

                            table3.Cell(9, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(9, 0).SetContent("Blood Group:");

                            table3.Cell(10, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(10, 0).SetContent("Email Address:");

                            table3.Cell(11, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(11, 0).SetContent("Part I Language in UG:");

                            table3.Cell(12, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(12, 0).SetContent("Date of Admission:");

                            table3.Cell(13, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(13, 0).SetContent("Temporary Residential Address & Phone:");

                            table3.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(2, 1).SetFont(Fontsmalltblbold);
                            table3.Cell(2, 1).SetContent(nationality);

                            table3.Cell(10, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(10, 1).SetFont(Fontsmalltblbold);
                            table3.Cell(10, 1).SetContent(email);

                            table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(0, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(0, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["dob"]));

                            table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(1, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(1, 2).SetContent(gender);

                            table3.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(2, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(2, 2).SetContent(religion);

                            table3.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(3, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(3, 2).SetContent(state);

                            table3.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(4, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(4, 2).SetContent(mothertongue);

                            table3.Cell(5, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(5, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(5, 2).SetContent(community);

                            table3.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(6, 1).SetFont(Fontsmalltblbold);
                            table3.Cell(6, 1).SetContent(subcaste);

                            if (isdisable == "Yes")
                            {
                                table3.Cell(7, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(7, 1).SetFont(Fontsmalltblbold);
                                table3.Cell(7, 1).SetContent(isdisable);

                                table3.Cell(7, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(7, 2).SetFont(Fontsmalltblbold);
                                table3.Cell(7, 2).SetContent(handy);
                            }
                            else
                            {
                                table3.Cell(7, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(7, 2).SetFont(Fontsmalltblbold);
                                table3.Cell(7, 2).SetContent(isdisable);
                            }

                            table3.Cell(8, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(8, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(8, 2).SetContent(xservice);

                            table3.Cell(9, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(9, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(9, 2).SetContent(bloodgroup);

                            table3.Cell(11, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(11, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(11, 2).SetContent("");

                            table3.Cell(12, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(12, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(12, 2).SetContent("");

                            table3.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(0, 3).SetContent("Father's Name");

                            table3.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(1, 3).SetContent("Date of Birth & Age");

                            table3.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(2, 3).SetContent("Occupation");

                            table3.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(3, 3).SetContent("Monthly Income");

                            table3.Cell(4, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(4, 3).SetContent("Mother's Name");

                            table3.Cell(5, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(5, 3).SetContent("Date of Birth & Age");

                            table3.Cell(6, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(6, 3).SetContent("Occupation");

                            table3.Cell(7, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(7, 3).SetContent("Monthly Income");

                            table3.Cell(8, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(8, 3).SetContent("Guardian's Name");

                            table3.Cell(9, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(9, 3).SetContent("Date of Birth & Age");

                            table3.Cell(10, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(10, 3).SetContent("Occupation");

                            table3.Cell(11, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(11, 3).SetContent("Monthly Income");

                            table3.Cell(13, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(13, 3).SetContent("Permanent Residential Address & Phone");

                            table3.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(0, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(0, 4).SetContent(" " + fathername);

                            table3.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(1, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(1, 4).SetContent(" " + fatherdob);

                            table3.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(2, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(2, 4).SetContent(" " + fatherocc);

                            table3.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(3, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(3, 4).SetContent(" " + fatherinc);

                            table3.Cell(4, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(4, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(4, 4).SetContent(" " + mothername);

                            table3.Cell(5, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(5, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(5, 4).SetContent(" " + motherdob);

                            table3.Cell(6, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(6, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(6, 4).SetContent(" " + motherocc);

                            table3.Cell(7, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(7, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(7, 4).SetContent(" " + motherinc);

                            table3.Cell(8, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(8, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(8, 4).SetContent(" " + guardianname);

                            table3.Cell(9, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(9, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(9, 4).SetContent(" " + guardiandob);

                            table3.Cell(10, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(10, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(10, 4).SetContent(" " + guardianocc);

                            table3.Cell(11, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(11, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(11, 4).SetContent(" " + guardianinc);

                            table3.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(0, 5).SetContent("Father's Office Address & Phone");

                            table3.Cell(4, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(4, 5).SetContent("Mother's Office Address & Phone");

                            table3.Cell(8, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(8, 5).SetContent("Guardian's Office Address & Phone");

                            table3.Cell(13, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(13, 5).SetContent("Guardian's Residential Address & Phone:");

                            table3.Cell(14, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(14, 0).SetFont(Fontsmalltblbold);
                            table3.Cell(14, 0).SetContent(ds.Tables[0].Rows[0]["parent_addressC"]);

                            table3.Cell(15, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(15, 0).SetFont(Fontsmalltblbold);
                            table3.Cell(15, 0).SetContent(ds.Tables[0].Rows[0]["Streetc"]);

                            table3.Cell(16, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(16, 0).SetFont(Fontsmalltblbold);
                            table3.Cell(16, 0).SetContent(ds.Tables[0].Rows[0]["Cityc"]);

                            table3.Cell(16, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(16, 1).SetFont(Fontsmalltblbold);
                            table3.Cell(16, 1).SetContent(ds.Tables[0].Rows[0]["parent_pincodec"]);

                            string getcomstate = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_statec"]), "state");
                            string comstate = "";
                            if (getcomstate.Trim() != "")
                            {
                                comstate = getcomstate;
                            }
                            else
                            {
                                comstate = "";
                            }
                            table3.Cell(17, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(17, 0).SetFont(Fontsmalltblbold);
                            table3.Cell(17, 0).SetContent(comstate);

                            table3.Cell(14, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(14, 3).SetFont(Fontsmalltblbold);
                            table3.Cell(14, 3).SetContent(ds.Tables[0].Rows[0]["parent_addressP"]);

                            table3.Cell(15, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(15, 3).SetFont(Fontsmalltblbold);
                            table3.Cell(15, 3).SetContent(ds.Tables[0].Rows[0]["Streetp"]);

                            table3.Cell(16, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(16, 3).SetFont(Fontsmalltblbold);
                            table3.Cell(16, 3).SetContent(ds.Tables[0].Rows[0]["cityp"]);

                            table3.Cell(16, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(16, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(16, 4).SetContent(ds.Tables[0].Rows[0]["parent_pincodep"]);

                            table3.Cell(17, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(17, 2).SetFont(Fontsmalltblbold);
                            table3.Cell(17, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]));

                            table3.Cell(17, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(17, 4).SetFont(Fontsmalltblbold);
                            table3.Cell(17, 4).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["parentF_Mobile"]));


                            table3.Cell(17, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(17, 3).SetFont(Fontsmalltblbold);
                            table3.Cell(17, 3).SetContent(state);

                            table3.Cell(14, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(14, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(14, 5).SetContent(ds.Tables[0].Rows[0]["addressg"]);

                            table3.Cell(15, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(15, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(15, 5).SetContent(ds.Tables[0].Rows[0]["Streetg"]);

                            table3.Cell(16, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(16, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(16, 5).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["Cityg"]) + "   " + Convert.ToString(ds.Tables[0].Rows[0]["ping"]));

                            string getguardstate = "";
                            string guardstate = "";
                            getguardstate = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["stateg"]), "state");
                            if (getguardstate.Trim() != "" && getguardstate.Trim() != "0")
                            {
                                guardstate = getguardstate;
                            }
                            else
                            {
                                guardstate = "";
                            }

                            table3.Cell(17, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(17, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(17, 5).SetContent(guardstate + "   " + Convert.ToString(ds.Tables[0].Rows[0]["guardian_mobile"]));

                            table3.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(1, 5).SetFont(Fontsmalltblbold);
                            string ss = Convert.ToString(ds.Tables[0].Rows[0]["Fat_off_addressP"]) + " " + Convert.ToString(ds.Tables[0].Rows[0]["Fat_off_street"]);

                            string[] sss = ss.Split(',');
                            ss = "";
                            foreach (string item in sss)
                            {
                                ss += item + ", ";
                            }
                            string getssval = ss.TrimEnd(' ').TrimEnd(',');
                            table3.Cell(1, 5).SetContent(getssval);

                            string getftoffstate = "";
                            string ftoffstate = "";
                            getftoffstate = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["Fat_off_state"]), "state");
                            if (getftoffstate.Trim() != "" && getftoffstate.Trim() != "0")
                            {
                                ftoffstate = getftoffstate;
                            }
                            else
                            {
                                ftoffstate = "";
                            }

                            table3.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(2, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(2, 5).SetContent(Convert.ToString(ftoffstate) + "   " + Convert.ToString(ds.Tables[0].Rows[0]["Fat_off_pincode"]));

                            table3.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(3, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(3, 5).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["parentF_Mobile"]));

                            table3.Cell(5, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(5, 5).SetFont(Fontsmalltblbold);
                            string contval = Convert.ToString(ds.Tables[0].Rows[0]["mot_off_address1"]) + " " + Convert.ToString(ds.Tables[0].Rows[0]["mot_off_address2"]);

                            string[] splitcont = contval.Split(',');
                            contval = "";
                            foreach (string item in splitcont)
                            {
                                contval += item + ", ";
                            }
                            string getcontval = contval.TrimEnd(' ').TrimEnd(',');
                            table3.Cell(5, 5).SetContent(getcontval);

                            string getmooffstate = "";
                            string mooffstate = "";
                            getmooffstate = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["mot_off_state"]), "state");
                            if (getmooffstate.Trim() != "" && getmooffstate.Trim() != "0")
                            {
                                mooffstate = getmooffstate;
                            }
                            else
                            {
                                mooffstate = "";
                            }

                            table3.Cell(6, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(6, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(6, 5).SetContent(Convert.ToString(mooffstate) + "  " + Convert.ToString(ds.Tables[0].Rows[0]["mot_off_pincode"]));

                            table3.Cell(7, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(7, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(7, 5).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["parentM_Mobile"]));

                            table3.Cell(9, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(9, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(9, 5).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["gur_off_address1"]));

                            table3.Cell(10, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(10, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(10, 5).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["gur_off_address2"]));

                            string getguroffstate = "";
                            string guroffstate = "";
                            getguroffstate = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["gur_off_state"]), "state");
                            if (getguroffstate.Trim() != "" && getguroffstate.Trim() != "0")
                            {
                                guroffstate = getguroffstate;
                            }
                            else
                            {
                                guroffstate = "";
                            }

                            table3.Cell(11, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(11, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(11, 5).SetContent(Convert.ToString(guroffstate) + "  " + Convert.ToString(ds.Tables[0].Rows[0]["gur_off_pincode"]));

                            table3.Cell(12, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table3.Cell(12, 5).SetFont(Fontsmalltblbold);
                            table3.Cell(12, 5).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["guardian_mobile"]));

                            foreach (PdfCell pc in table3.CellRange(6, 1, 6, 1).Cells)
                            {
                                pc.ColSpan = 2;
                            }

                            foreach (PdfCell pc in table3.CellRange(10, 1, 10, 1).Cells)
                            {
                                pc.ColSpan = 2;
                            }

                            foreach (PdfCell pc in table3.CellRange(13, 0, 13, 0).Cells)
                            {
                                pc.ColSpan = 3;
                            }

                            foreach (PdfCell pc in table3.CellRange(13, 3, 13, 3).Cells)
                            {
                                pc.ColSpan = 2;
                            }

                            foreach (PdfCell pc in table3.CellRange(1, 5, 1, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            foreach (PdfCell pc in table3.CellRange(2, 5, 2, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            foreach (PdfCell pc in table3.CellRange(3, 5, 3, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            foreach (PdfCell pc in table3.CellRange(4, 5, 4, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            foreach (PdfCell pc in table3.CellRange(5, 5, 5, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            foreach (PdfCell pc in table3.CellRange(6, 5, 6, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            foreach (PdfCell pc in table3.CellRange(7, 5, 7, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            foreach (PdfCell pc in table3.CellRange(8, 5, 8, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            foreach (PdfCell pc in table3.CellRange(9, 5, 9, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            foreach (PdfCell pc in table3.CellRange(10, 5, 10, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            foreach (PdfCell pc in table3.CellRange(11, 5, 11, 5).Cells)
                            {
                                pc.RowSpan = 1;
                            }

                            //PdfRectangle pdfrect = new PdfRectangle(mydoc, new PdfArea(mydoc, line1, y + 180, 790, 360), Color.Black);
                            //mypage.Add(pdfrect);

                            //PdfLine pdfcolin1 = new PdfLine(mydoc, new PointF(line1 + 350, y + 180), new PointF(line1 + 350, y + 540), Color.Black, 1);
                            //mypage.Add(pdfcolin1);

                            //PdfLine pdfcolin2 = new PdfLine(mydoc, new PointF(line1 + 460, y + 180), new PointF(line1 + 460, y + 425), Color.Black, 1);
                            //mypage.Add(pdfcolin2);

                            //PdfLine pdfcolin3 = new PdfLine(mydoc, new PointF(line1 + 620, y + 180), new PointF(line1 + 620, y + 540), Color.Black, 1);
                            //mypage.Add(pdfcolin3);

                            //PdfLine pdfroin1 = new PdfLine(mydoc, new PointF(line1, y + 198), new PointF(line1 + 790, y + 198), Color.Black, 1);
                            //mypage.Add(pdfroin1);

                            //PdfLine pdfroin2 = new PdfLine(mydoc, new PointF(line1, y + 215), new PointF(line1 + 620, y + 215), Color.Black, 1);
                            //mypage.Add(pdfroin2);

                            //PdfLine pdfroin3 = new PdfLine(mydoc, new PointF(line1, y + 233), new PointF(line1 + 620, y + 233), Color.Black, 1);
                            //mypage.Add(pdfroin3);

                            //PdfLine pdfroin4 = new PdfLine(mydoc, new PointF(line1, y + 253), new PointF(line1 + 350, y + 253), Color.Black, 1);
                            //mypage.Add(pdfroin4);

                            //PdfLine pdfroinnew4 = new PdfLine(mydoc, new PointF(line1 + 350, y + 253), new PointF(line1 + 790, y + 253), Color.Black, 2);
                            //mypage.Add(pdfroinnew4);

                            //PdfLine pdfroin5 = new PdfLine(mydoc, new PointF(line1, y + 272), new PointF(line1 + 790, y + 272), Color.Black, 1);
                            //mypage.Add(pdfroin5);

                            //PdfLine pdfroin6 = new PdfLine(mydoc, new PointF(line1, y + 290), new PointF(line1 + 620, y + 290), Color.Black, 1);
                            //mypage.Add(pdfroin6);

                            //PdfLine pdfroin7 = new PdfLine(mydoc, new PointF(line1, y + 307), new PointF(line1 + 620, y + 307), Color.Black, 1);
                            //mypage.Add(pdfroin7);

                            //PdfLine pdfroin8 = new PdfLine(mydoc, new PointF(line1, y + 325), new PointF(line1 + 350, y + 325), Color.Black, 1);
                            //mypage.Add(pdfroin8);

                            //PdfLine pdfroinnew8 = new PdfLine(mydoc, new PointF(line1 + 350, y + 325), new PointF(line1 + 790, y + 325), Color.Black, 2);
                            //mypage.Add(pdfroinnew8);

                            //PdfLine pdfroin9 = new PdfLine(mydoc, new PointF(line1, y + 350), new PointF(line1 + 790, y + 350), Color.Black, 1);
                            //mypage.Add(pdfroin9);

                            //PdfLine pdfroin10 = new PdfLine(mydoc, new PointF(line1, y + 371), new PointF(line1 + 620, y + 371), Color.Black, 1);
                            //mypage.Add(pdfroin10);

                            //PdfLine pdfroin11 = new PdfLine(mydoc, new PointF(line1, y + 388), new PointF(line1 + 620, y + 388), Color.Black, 1);
                            //mypage.Add(pdfroin11);

                            //PdfLine pdfroin12 = new PdfLine(mydoc, new PointF(line1, y + 408), new PointF(line1 + 620, y + 408), Color.Black, 1);
                            //mypage.Add(pdfroin12);

                            //PdfLine pdfroin13 = new PdfLine(mydoc, new PointF(line1, y + 425), new PointF(line1 + 790, y + 425), Color.Black, 1);
                            //mypage.Add(pdfroin13);

                            //PdfLine pdfroin14 = new PdfLine(mydoc, new PointF(line1, y + 450), new PointF(line1 + 790, y + 450), Color.Black, 1);
                            //mypage.Add(pdfroin14);

                            Gios.Pdf.PdfTablePage myprov_pdfpage2 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, line1, y + 180, 790, 370));
                            mypage.Add(myprov_pdfpage2);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydoc, line1, y + 555, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Student");
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydoc, line2 + 75, y + 555, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Parent");
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydoc, line2 + 275, y + 555, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Guardian");
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydoc, line2 + 475, y + 555, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Enrollment Staff");
                            mypage.Add(ptc);
                        }
                        mypage.SaveToDocument();
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "CoverFormat" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);

                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                    Response.End();
                }
                mainpgeerr.Visible = false;
            }
        }
        catch { }
    }


    //protected void btninsurprnt_click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (checkok() == true)
    //        {
    //            Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
    //            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
    //            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
    //            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
    //            Gios.Pdf.PdfPage mypage;

    //            for (i = 1; i < FpSpread.Sheets[0].RowCount; i++)
    //            {
    //                FpSpread.SaveChanges();
    //                string val = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Value);
    //                if (val == "1")
    //                {
    //                    string appformno = Convert.ToString(FpSpread.Sheets[0].Cells[i, 2].Text);
    //                    string app_no = d2.GetFunction("select app_no from applyn where app_formno='" + appformno + "' and college_code='" + collegecode1 + "'");
    //                    mypage = mydoc.NewPage();

    //                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/MccLeft_Logo.jpeg")))
    //                    {
    //                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/MccLeft_Logo.jpeg"));
    //                        mypage.Add(LogoImage, 25, 25, 400);
    //                    }
    //                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/MccLeft_Logo.jpeg")))
    //                    {
    //                        MemoryStream memoryStream = new MemoryStream();
    //                        string sellogo = "select logo1,logo2 from collinfo where college_code='" + Session["collegecode"] + "'";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(sellogo, "Text");
    //                        if (ds.Tables.Count > 0)
    //                        {
    //                            if (ds.Tables[0].Rows.Count > 0)
    //                            {
    //                                byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
    //                                memoryStream.Write(file, 0, file.Length);
    //                                if (file.Length > 0)
    //                                {
    //                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
    //                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
    //                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + file + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                }
    //                                memoryStream.Dispose();
    //                                memoryStream.Close();
    //                            }
    //                        }
    //                    }

    //                    string collquery = "";
    //                    collquery = "select collname,category,university,address1,address2,address3,phoneno,faxno,email,website,district,state,pincode  from collinfo where college_Code=" + collegecode1 + "";
    //                    ds.Clear();
    //                    ds = d2.select_method_wo_parameter(collquery, "Text");
    //                    string collegename = "";
    //                    string collegeaddress = "";
    //                    string collegedistrict = "";
    //                    string phonenumber = "";
    //                    string fax = "";
    //                    string email = "";
    //                    string website = "";
    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]) + "(" + Convert.ToString(ds.Tables[0].Rows[0]["category"]) + ")";
    //                        collegeaddress = Convert.ToString(ds.Tables[0].Rows[0]["address1"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address2"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
    //                        collegedistrict = Convert.ToString(ds.Tables[0].Rows[0]["district"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["state"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
    //                        phonenumber = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
    //                        fax = Convert.ToString(ds.Tables[0].Rows[0]["faxno"]); ;
    //                        email = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
    //                        website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
    //                    }

    //                    PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydoc, 110, 10, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
    //                    mypage.Add(ptc);
    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, 110, 25, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegeaddress);
    //                    mypage.Add(ptc);
    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, 110, 35, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegedistrict);
    //                    mypage.Add(ptc);
    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, 110, 45, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phone No: " + phonenumber + ", Fax:" + fax);
    //                    mypage.Add(ptc);
    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, 110, 55, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, email);
    //                    mypage.Add(ptc);
    //                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                                       new PdfArea(mydoc, 110, 65, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, website);

    //                    mypage.Add(ptc);
    //                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, 110, 85, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Application Form for Insurance");
    //                    mypage.Add(ptc);

    //                    int y = 65;
    //                    int line1 = 50;
    //                    int line2 = 250;

    //                    string degreecode = GetSelectedItemsValueAsString(cbldepartment);

    //                    string getstudinfn = "select stud_name,c.Course_Name,sex,age,Convert(varchar(10),dob,103) as dob,dob as dob1,bldgrp,idmark,Dept_Name,batch_year,mother,parent_income,motherocc,mIncome,parent_occu,guardian_name,Guardian_income,Guardian_occ,Convert(varchar(10),Guardiandob,103) as Guardiandob,Convert(varchar(10),fatherdob,103) as fatherdob,Convert(varchar(10),motherdob,103) as motherdob,isdisable,parent_name,parent_addressP,Streetp,cityp,parent_pincodep,parent_statep,visualhandy from applyn a,Degree d,Department dt,Course C where isconfirm='1' and admission_status ='1' and selection_status ='1' and is_enroll ='1' and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and batch_year ='" + Convert.ToString(ddlbatch.SelectedItem.Text) + "' and a.degree_code in ('" + degreecode + "') and app_formno='" + appformno + "'";
    //                    ds.Clear();
    //                    ds = d2.select_method_wo_parameter(getstudinfn, "Text");
    //                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "1.Name");
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
    //                        mypage.Add(ptc);

    //                        PdfImage LogoImage2;
    //                        string stdphtsql = "select * from StdPhoto where app_no='" + app_no + "'";
    //                        MemoryStream memoryStream = new MemoryStream();
    //                        DataSet dsstdpho = new DataSet();
    //                        dsstdpho.Clear();
    //                        dsstdpho.Dispose();
    //                        dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
    //                        if (dsstdpho.Tables[0].Rows.Count > 0)
    //                        {
    //                            byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
    //                            memoryStream.Write(file, 0, file.Length);
    //                            if (file.Length > 0)
    //                            {
    //                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
    //                                System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
    //                                if (File.Exists(HttpContext.Current.Server.MapPath("~/StudentImage/" + app_no + ".jpeg")))
    //                                {

    //                                }
    //                                else
    //                                {
    //                                    thumb.Save(HttpContext.Current.Server.MapPath("~/StudentImage/" + app_no + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                }
    //                            }
    //                        }

    //                        if (File.Exists(HttpContext.Current.Server.MapPath("~/StudentImage/" + app_no + ".jpeg")))
    //                        {
    //                            LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/StudentImage/" + app_no + ".jpeg"));
    //                            mypage.Add(LogoImage2, line2 + 200, y + 70, 420);
    //                        }
    //                        else
    //                        {

    //                        }

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 100, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "2.Sex");
    //                        mypage.Add(ptc);

    //                        string gender = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "0")
    //                        {
    //                            gender = "Male";
    //                        }
    //                        else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "1")
    //                        {
    //                            gender = "Female";
    //                        }
    //                        else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "2")
    //                        {
    //                            gender = "TransGender";
    //                        }
    //                        else
    //                        {
    //                            gender = "";
    //                        }
    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 100, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(gender));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "3.Age&Date Of Birth");
    //                        mypage.Add(ptc);

    //                        string age = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["age"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["age"]) != null)
    //                        {
    //                            age = Convert.ToString(ds.Tables[0].Rows[0]["age"]);
    //                        }
    //                        else
    //                        {
    //                            if (Convert.ToString(ds.Tables[0].Rows[0]["dob"]) != null && Convert.ToString(ds.Tables[0].Rows[0]["dob"]) != "")
    //                            {
    //                                int curryear = Convert.ToInt32(DateTime.Now.Year);
    //                                DateTime dt = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["dob1"]));
    //                                int dobyear = dt.Year;
    //                                age = Convert.ToString(curryear - dobyear);
    //                            }
    //                        }
    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(age));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2 + 30, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["dob"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "4.Blood Group");
    //                        mypage.Add(ptc);

    //                        string blood = "";
    //                        string bldgrp = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["bldgrp"]), "bgrou");
    //                        if (bldgrp.Trim() != "" && bldgrp.Trim() != "0")
    //                        {
    //                            blood = bldgrp;
    //                        }
    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(blood));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "5.Identification Marks");
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 190, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["idmark"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 220, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "6.Course&Year Of Study");
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 220, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2 + 60, y + 220, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2 + 150, y + 220, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 250, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "7.Name,Age,Occupation & Monthly Income Details:");
    //                        mypage.Add(ptc);

    //                        Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, 4, 5, 1);
    //                        table2 = mydoc.NewTable(Fontsmall, 4, 5, 1);
    //                        table2.VisibleHeaders = false;
    //                        table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //                        table2.Columns[0].SetWidth(75);
    //                        table2.Columns[1].SetWidth(150);
    //                        table2.Columns[2].SetWidth(75);
    //                        table2.Columns[3].SetWidth(100);
    //                        table2.Columns[4].SetWidth(100);
    //                        table2.CellRange(0, 0, 0, 4).SetFont(Fontsmall);

    //                        table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(0, 0).SetContent("Relation");

    //                        table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(0, 1).SetContent("Name");

    //                        table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(0, 2).SetContent("D.O.B");

    //                        table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(0, 3).SetContent("Occupation");

    //                        table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(0, 4).SetContent("Monthly Income in Rs.");

    //                        table2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(1, 0).SetContent("Father");

    //                        table2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table2.Cell(1, 1).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]));

    //                        table2.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(1, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["fatherdob"]));

    //                        string fatheroccupation = "";
    //                        string getfatherocc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_occu"]), "foccu");
    //                        if (getfatherocc.Trim() != "" && getfatherocc.Trim() != "0")
    //                        {
    //                            fatheroccupation = getfatherocc;
    //                        }
    //                        table2.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(1, 3).SetContent(Convert.ToString(fatheroccupation));

    //                        string fatherinc = "";
    //                        string getfatherinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_income"]), "fin");
    //                        if (getfatherinc.Trim() != "" && getfatherinc.Trim() != "0")
    //                        {
    //                            fatherinc = getfatherinc;
    //                        }
    //                        table2.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(1, 4).SetContent(Convert.ToString(fatherinc));

    //                        table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(2, 0).SetContent("Mother");

    //                        table2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table2.Cell(2, 1).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["mother"]));

    //                        table2.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(2, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["motherdob"]));

    //                        string motheroccupation = "";
    //                        string getmotherocc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["motherocc"]), "moccu");
    //                        if (getmotherocc.Trim() != "" && getmotherocc.Trim() != "0")
    //                        {
    //                            motheroccupation = getmotherocc;
    //                        }
    //                        table2.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(2, 3).SetContent(Convert.ToString(getmotherocc));

    //                        string motherinc = "";
    //                        string getmotherinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["mIncome"]), "min");
    //                        if (getmotherinc.Trim() != "" && getmotherinc.Trim() != "0")
    //                        {
    //                            motherinc = getmotherinc;
    //                        }
    //                        table2.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(2, 4).SetContent(Convert.ToString(motherinc));

    //                        table2.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(3, 0).SetContent("Guardian");

    //                        table2.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        table2.Cell(3, 1).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]));

    //                        table2.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(3, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["Guardiandob"]));

    //                        string guardianoccupation = "";
    //                        string getguardianocc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["motherocc"]), "moccu");
    //                        if (getguardianocc.Trim() != "" && getguardianocc.Trim() != "0")
    //                        {
    //                            guardianoccupation = getguardianocc;
    //                        }
    //                        table2.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(3, 3).SetContent(Convert.ToString(guardianoccupation));

    //                        string guardianinc = "";
    //                        string getguardianinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["mIncome"]), "min");
    //                        if (getguardianinc.Trim() != "" && getguardianinc.Trim() != "0")
    //                        {
    //                            guardianinc = getguardianinc;
    //                        }
    //                        table2.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table2.Cell(3, 4).SetContent(Convert.ToString(guardianinc));

    //                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, y + 280, 500, 550));
    //                        mypage.Add(myprov_pdfpage1);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 340, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "8.Residential Address");
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 340, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 370, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 400, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["cityp"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2 + 90, y + 400, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 430, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["parent_statep"]));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 460, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "9.Any Physical Disability");
    //                        mypage.Add(ptc);

    //                        string visualhandy = "";
    //                        if (Convert.ToString(ds.Tables[0].Rows[0]["visualhandy"]) == "0")
    //                        {
    //                            visualhandy = "No";
    //                        }
    //                        else
    //                        {
    //                            visualhandy = "Yes";
    //                        }
    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line2, y + 460, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(visualhandy));
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                             new PdfArea(mydoc, line1, y + 600, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date:");
    //                        mypage.Add(ptc);

    //                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                            new PdfArea(mydoc, line2 + 100, y + 600, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Student");
    //                        mypage.Add(ptc);
    //                    }
    //                    mypage.SaveToDocument();
    //                }
    //            }

    //            string appPath = HttpContext.Current.Server.MapPath("~");
    //            if (appPath != "")
    //            {
    //                string szPath = appPath + "/Report/";
    //                string szFile = "InsuranceFormat" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
    //                mydoc.SaveToFile(szPath + szFile);

    //                Response.ClearHeaders();
    //                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
    //                Response.ContentType = "application/pdf";
    //                Response.WriteFile(szPath + szFile);
    //                Response.End();
    //            }
    //            mainpgeerr.Visible = false;
    //        }
    //        else
    //        {
    //            mainpgeerr.Visible = true;
    //            mainpgeerr.Text = "Please Select Any one Student!";
    //        }
    //    }
    //    catch { }
    //}


    protected void btninsurprnt_click(object sender, EventArgs e)
    {
        try
        {
            if (checkok() == true)
            {
                Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
                Font Fontsmall = new Font("Times New Roman", 15, FontStyle.Regular);
                Font Fontsmall1 = new Font("Times New Roman", 15, FontStyle.Bold);
                Font Fontsmall2 = new Font("Times New Roman", 12, FontStyle.Bold);
                Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
                Font Fontbold2 = new Font("Times New Roman", 13, FontStyle.Bold);
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                Gios.Pdf.PdfPage mypage;

                for (i = 1; i < FpSpread.Sheets[0].RowCount; i++)
                {
                    FpSpread.SaveChanges();
                    string val = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Value);
                    if (val == "1")
                    {
                        string appformno = Convert.ToString(FpSpread.Sheets[0].Cells[i, 2].Text);
                        string app_no = d2.GetFunction("select app_no from applyn where app_formno='" + appformno + "' and college_code='" + collegecode1 + "'");
                        mypage = mydoc.NewPage();

                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/left_Logo.jpeg"));
                            mypage.Add(LogoImage, 25, 25, 400);
                        }
                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/left_Logo.jpeg")))
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            string sellogo = "select logo1,logo2 from collinfo where college_code='" + Session["collegecode"] + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(sellogo, "Text");
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + file + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                            }
                        }

                        string collquery = "";
                        collquery = "select collname,category,university,address1,address2,address3,phoneno,faxno,email,website,district,state,pincode  from collinfo where college_Code=" + collegecode1 + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(collquery, "Text");
                        string collegename = "";
                        string collegeaddress = "";
                        string collegedistrict = "";
                        string phonenumber = "";
                        string fax = "";
                        string email = "";
                        string website = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]) + " (" + Convert.ToString(ds.Tables[0].Rows[0]["category"]) + ")";
                            collegeaddress = Convert.ToString(ds.Tables[0].Rows[0]["address1"]) + "" + Convert.ToString(ds.Tables[0].Rows[0]["address3"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
                            // collegedistrict = Convert.ToString(ds.Tables[0].Rows[0]["district"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["state"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
                            phonenumber = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                            fax = Convert.ToString(ds.Tables[0].Rows[0]["faxno"]); ;
                            email = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                            website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
                        }

                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 110, 10, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                        mypage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 110, 25, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegeaddress);
                        mypage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 110, 35, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegedistrict);
                        mypage.Add(ptc);
                        //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 110, 45, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phone No: " + phonenumber + ", Fax:" + fax);
                        //mypage.Add(ptc);
                        //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 110, 55, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, email);
                        //mypage.Add(ptc);
                        //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 110, 65, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, website);

                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg"));
                            mypage.Add(LogoImage, 25, 18, 440);

                        }

                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            ds.Dispose();
                            ds.Reset();
                            ds = d2.select_method_wo_parameter("select logo1 from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "' and logo1 is not null", "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                }
                                memoryStream.Dispose();
                                memoryStream.Close();
                            }
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"));
                            mypage.Add(LogoImage, 25, 18, 440);
                        }
                        mypage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, 110, 85, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Application Form for Insurance");
                        mypage.Add(ptc);

                        int y = 65;
                        int line1 = 50;
                        int line2 = 250;

                        string degreecode = GetSelectedItemsValueAsString(cbldepartment);

                        string getstudinfn = "select Student_Mobile,stud_name,c.Course_Name,sex,age,Convert(varchar(10),dob,103) as dob,dob as dob1,bldgrp,idmark,Dept_Name,batch_year,mother,parent_income,motherocc,mIncome,parent_occu,guardian_name,Guardian_income,Guardian_occ,Convert(varchar(10),Guardiandob,103) as Guardiandob,Convert(varchar(10),fatherdob,103) as fatherdob,Convert(varchar(10),motherdob,103) as motherdob,isdisable,parent_name,parent_addressP,Streetp,cityp,parent_pincodep,parent_statep,visualhandy,handy from applyn a,Degree d,Department dt,Course C where isconfirm='1' and admission_status ='1' and selection_status ='1' and is_enroll ='1' and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and batch_year ='" + Convert.ToString(ddlbatch.SelectedItem.Text) + "' and a.degree_code in ('" + degreecode + "') and app_formno='" + appformno + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(getstudinfn, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string[] splid = new string[10];
                            string[] spladd = new string[5];
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "1.Name");
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                            mypage.Add(ptc);

                            PdfImage LogoImage2;
                            string stdphtsql = "select * from StdPhoto where app_no='" + app_no + "'";
                            MemoryStream memoryStream = new MemoryStream();
                            DataSet dsstdpho = new DataSet();
                            dsstdpho.Clear();
                            dsstdpho.Dispose();
                            dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
                            if (dsstdpho.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpeg")))
                                    {

                                    }
                                    else
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                }
                            }

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpeg")))
                            {
                                LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpeg"));
                                mypage.Add(LogoImage2, line2 + 200, y + 70, 250);
                            }
                            else
                            {

                            }

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 100, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "2.Sex");
                            mypage.Add(ptc);

                            string gender = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "0")
                            {
                                gender = "Male";
                            }
                            else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "1")
                            {
                                gender = "Female";
                            }
                            else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "2")
                            {
                                gender = "TransGender";
                            }
                            else
                            {
                                gender = "";
                            }
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 100, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(gender));
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "3.Age&Date Of Birth");
                            mypage.Add(ptc);

                            string age = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["age"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["age"]) != null)
                            {
                                age = Convert.ToString(ds.Tables[0].Rows[0]["age"]);
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[0]["dob"]) != null && Convert.ToString(ds.Tables[0].Rows[0]["dob"]) != "")
                                {
                                    int curryear = Convert.ToInt32(DateTime.Now.Year);
                                    DateTime dt = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["dob1"]));
                                    int dobyear = dt.Year;
                                    age = Convert.ToString(curryear - dobyear);
                                }
                            }
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(age));
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2 + 30, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["dob"]));
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "4.Blood Group");
                            mypage.Add(ptc);

                            string blood = "";
                            string bldgrp = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["bldgrp"]), "bgrou");
                            if (bldgrp.Trim() != "" && bldgrp.Trim() != "0")
                            {
                                blood = bldgrp;
                            }
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(blood));
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "5.Identification Marks");
                            mypage.Add(ptc);

                            int getpos = y + 190;
                            int newpos = 0;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["idmark"]).Trim().Contains(","))
                            {
                                splid = Convert.ToString(ds.Tables[0].Rows[0]["idmark"]).Split(',');
                                if (splid.Length > 0)
                                {
                                    for (int ik = 0; ik < splid.Length; ik++)
                                    {
                                        ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, getpos + (ik * 30), 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ik + 1) + "." + Convert.ToString(splid[ik]));
                                        mypage.Add(ptc);
                                        newpos = getpos + (ik * 30);
                                    }
                                }
                            }
                            else
                            {
                                ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, line2, y + 190, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["idmark"]));
                                mypage.Add(ptc);
                                newpos = y + 190;
                            }

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, newpos + 30, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "6.Course&Year Of Study");
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, newpos + 30, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2 + 60, newpos + 30, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]));
                            mypage.Add(ptc);
                            int batchh = Convert.ToInt32(ds.Tables[0].Rows[0]["batch_year"]) + 1;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2 + 230, newpos + 30, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]) + "-" + batchh);
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, newpos + 60, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "7.Name,Age,Occupation & Monthly Income Details:");
                            mypage.Add(ptc);


                            Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall1, 4, 5, 1);
                            table2 = mydoc.NewTable(Fontsmall, 4, 5, 1);
                            table2.VisibleHeaders = false;
                            table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table2.Columns[0].SetWidth(75);
                            table2.Columns[1].SetWidth(150);
                            table2.Columns[2].SetWidth(75);
                            table2.Columns[3].SetWidth(100);
                            table2.Columns[4].SetWidth(100);
                            table2.CellRange(0, 0, 0, 4).SetFont(Fontsmall);

                            table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 0).SetContent("Relation");

                            table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 1).SetContent("Name");

                            table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 2).SetContent("D.O.B");

                            table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 3).SetContent("Occupation");

                            table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 4).SetContent("Monthly Income in Rs.");

                            table2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(1, 0).SetContent("Father");

                            table2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(1, 1).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]));

                            table2.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(1, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["fatherdob"]));

                            string fatheroccupation = "";
                            string getfatherocc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_occu"]), "foccu");
                            if (getfatherocc.Trim() != "" && getfatherocc.Trim() != "0")
                            {
                                fatheroccupation = getfatherocc;
                            }
                            table2.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(1, 3).SetContent(Convert.ToString(fatheroccupation));

                            string fatherinc = "";
                            string getfatherinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_income"]), "fin");
                            if (getfatherinc.Trim() != "" && getfatherinc.Trim() != "0")
                            {
                                fatherinc = getfatherinc;
                            }
                            table2.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(1, 4).SetContent(Convert.ToString(fatherinc));

                            table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(2, 0).SetContent("Mother");

                            table2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(2, 1).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["mother"]));

                            table2.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(2, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["motherdob"]));

                            string motheroccupation = "";
                            string getmotherocc = getnewtxtval(Convert.ToString(ds.Tables[0].Rows[0]["motherocc"]), "foccu");
                            if (getmotherocc.Trim() != "" && getmotherocc.Trim() != "0")
                            {
                                motheroccupation = getmotherocc;
                            }

                            table2.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(2, 3).SetContent(Convert.ToString(motheroccupation));

                            string motherinc = "";
                            string getmotherinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["mIncome"]), "min");
                            if (getmotherinc.Trim() != "" && getmotherinc.Trim() != "0")
                            {
                                motherinc = getmotherinc;
                            }
                            table2.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(2, 4).SetContent(Convert.ToString(motherinc));

                            table2.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(3, 0).SetContent("Guardian");

                            table2.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(3, 1).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]));

                            table2.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(3, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["Guardiandob"]));

                            string guardianoccupation = "";
                            string getguardianocc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["Guardian_occ"]), "foccu");
                            if (getguardianocc.Trim() != "" && getguardianocc.Trim() != "0")
                            {
                                guardianoccupation = getguardianocc;
                            }
                            table2.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(3, 3).SetContent(Convert.ToString(guardianoccupation));

                            string guardianinc = "";
                            string getguardianinc = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["Guardian_income"]), "fin");
                            if (getguardianinc.Trim() != "" && getguardianinc.Trim() != "0")
                            {
                                guardianinc = getguardianinc;
                            }
                            table2.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(3, 4).SetContent(Convert.ToString(guardianinc));

                            Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 52, newpos + 90, 500, 550));
                            mypage.Add(myprov_pdfpage1);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, newpos + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "8.Residential Address");
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, newpos + 210, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]));
                            mypage.Add(ptc);

                            int getypos = newpos + 230;
                            int newposval = 0;
                            if (Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]).Contains("/"))
                            {
                                spladd = Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]).Split('/');
                                if (spladd.Length > 0)
                                {
                                    for (int jk = 0; jk < spladd.Length; jk++)
                                    {
                                        ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, getypos + (jk * 20), 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(spladd[jk]));
                                        mypage.Add(ptc);
                                        newposval = getypos + (jk * 20);
                                    }
                                }
                            }
                            else
                            {
                                ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, getypos, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]));
                                mypage.Add(ptc);
                                newposval = getypos;
                            }

                            ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, newposval + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["cityp"]));
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2 + 220, newposval + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]));
                            mypage.Add(ptc);

                            string state = gettxtval(Convert.ToString(ds.Tables[0].Rows[0]["parent_statep"]), "state");
                            ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, newposval + 40, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, state);
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, line2, newposval + 60, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No : " + (Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"])));
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, newposval + 100, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "9.Any Physical Disability");
                            mypage.Add(ptc);

                            string visualhandy = "";
                            string handy = "";

                            if (Convert.ToString(ds.Tables[0].Rows[0]["handy"]).Trim() == "1")
                            {
                                visualhandy = "Yes";
                                handy = "Physically";
                            }
                            else
                            {
                                visualhandy = "No";
                            }

                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, newposval + 100, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(visualhandy));
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, newposval + 200, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date:");
                            mypage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, line2 + 100, newposval + 200, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Student");
                            mypage.Add(ptc);
                        }
                        mypage.SaveToDocument();
                    }
                }

                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "InsuranceFormat" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);

                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                    Response.End();
                }
                mainpgeerr.Visible = false;
            }
            else
            {
                mainpgeerr.Visible = true;
                mainpgeerr.Text = "Please Select Any one Student!";
            }
        }
        catch { }
    }

    public string gettxtval(string txtcode, string txtcriteria)
    {
        string val = "";
        try
        {
            val = d2.GetFunction("select TextVal from TextValTable where TextCriteria='" + txtcriteria + "' and TextCode='" + txtcode + "' and college_code='" + collegecode1 + "'");
        }
        catch
        {

        }
        return val;
    }

    public void bindcollege()
    {
        try
        {
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
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddl_collegename.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.Enabled = true;
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch (Exception e) { }
    }

    public void binddept()
    {
        try
        {
            cbldepartment.Items.Clear();
            string build = "";
            string build2 = "";
            build = Convert.ToString(ddledulevel.SelectedItem.Value);
            build2 = GetSelectedItemsValueAsString(cbldegree);
            if (build != "" && build2 != "")
            {
                string deptquery = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and  department .dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + build2 + "') and degree.college_code in ('" + collegecode1 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldepartment.DataSource = ds;
                    cbldepartment.DataTextField = "dept_name";
                    cbldepartment.DataValueField = "degree_code";
                    cbldepartment.DataBind();
                    if (cbldepartment.Items.Count > 0)
                    {
                        for (i = 0; i < cbldepartment.Items.Count; i++)
                        {
                            cbldepartment.Items[i].Selected = true;
                        }
                        cbdepartment1.Checked = true;
                        txt_department.Text = lblBran.Text + "(" + cbldepartment.Items.Count + ")";
                    }
                }
            }
            else
            {
                cbdepartment1.Checked = false;
                txt_department.Text = "--Select--";
            }
        }
        catch (Exception ex) { }
    }

    public void loadstream()
    {
        try
        {
            ddltype.Items.Clear();
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + collegecode1 + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
            }
            else
            {
                ddltype.Enabled = false;
            }
            loadedulevel();
            Bindcourse();
            binddept();
        }
        catch { }
    }

    public void loadedulevel()
    {
        try
        {
            ds.Clear();
            ddledulevel.Items.Clear();
            string itemheader = "";
            string deptquery = "";
            if (ddltype.Enabled)
            {
                itemheader = Convert.ToString(ddltype.SelectedItem.Value);
                deptquery = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and type in ('" + itemheader + "') and college_code in ('" + collegecode1 + "') order by Edu_Level desc";
            }
            else
            {
                deptquery = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and college_code in ('" + collegecode1 + "') order by Edu_Level desc";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddledulevel.DataSource = ds;
                ddledulevel.DataTextField = "Edu_Level";
                ddledulevel.DataBind();
            }
            Bindcourse();
            binddept();
        }
        catch { }
    }

    public void BindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = d2.BindBatch();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch { }
    }

    public void Bindcourse()
    {
        try
        {
            cbldegree.Items.Clear();
            string build = "";
            string build1 = "";
            build = Convert.ToString(ddledulevel.SelectedItem.Value);
            if (build != "")
            {
                string deptquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + collegecode1 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
                if (ddltype.Enabled)
                {
                    build1 = Convert.ToString(ddltype.SelectedItem.Value);
                    deptquery = deptquery + " and type in ('" + build1 + "')";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldegree.DataSource = ds;
                    cbldegree.DataTextField = "course_name";
                    cbldegree.DataValueField = "course_id";
                    cbldegree.DataBind();
                    if (cbldegree.Items.Count > 0)
                    {
                        for (i = 0; i < cbldegree.Items.Count; i++)
                        {
                            cbldegree.Items[i].Selected = true;
                        }
                        cbdegree.Checked = true;
                        txt_degree.Text = lbldeg.Text + "(" + cbldegree.Items.Count + ")";
                    }
                }
            }
            else
            {
                cbdegree.Checked = false;
                txt_degree.Text = "--Select--";
            }
            binddept();
        }
        catch (Exception ex) { }
    }

    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    public string getnewtxtval(string txtcode, string txtcriteria)
    {
        string val = "";
        try
        {
            val = d2.GetFunction("select TextVal from TextValTable where TextCriteria='" + txtcriteria + "' and TextCode='" + txtcode + "' and college_code='" + collegecode1 + "'");
        }
        catch
        {

        }
        return val;
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
        lbl.Add(lbl_collegename);
        fields.Add(0);
        lbl.Add(lblStr);
        fields.Add(1);
        lbl.Add(lbldeg);
        fields.Add(2);
        lbl.Add(lblBran);
        fields.Add(3);
        //lbl.Add(lbl_org_sem);
        //fields.Add(4);



        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
}