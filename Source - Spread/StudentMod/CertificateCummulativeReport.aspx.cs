using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class CertificateCummulativeReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string selq = string.Empty;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();

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
                txtfrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttoDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtfrmDt.Attributes.Add("readonly", "readonly");
                txttoDt.Attributes.Add("readonly", "readonly");
                rdb_cummulate.Checked = true;
                rdb_individual.Checked = false;
                UpdatePanel1.Visible = true;
                UpdatePanel3.Visible = false;
                rprint.Visible = false;
                myTab1.Visible = false;
                myTab2.Visible = false;
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                    collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
                bind_batch();
                binddegree();
                bindbranch();
                bindsem();
            }
            if (ddlcollege.Items.Count > 0)
                collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
            lblmainerr.Visible = false;
            lblsmserror.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void rdb_cummulate_Change(object sender, EventArgs e)
    {
        bindcollege();
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        bind_batch();
        binddegree();
        bindbranch();
        bindsem();
        lblmainerr.Visible = false;
        Fpspreadpop.Visible = false;
        UpdatePanel1.Visible = true;
        UpdatePanel3.Visible = false;
        rprint.Visible = false;
        txtexcel.Text = "";
        myTab1.Visible = false;
        myTab2.Visible = false;
    }

    protected void rdb_individual_Change(object sender, EventArgs e)
    {
        bindcollege();
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        bind_batch();
        binddegree();
        bindbranch();
        bindsem();
        bindCertificate();
        lblmainerr.Visible = false;
        Fpspreadpop.Visible = false;
        UpdatePanel1.Visible = false;
        UpdatePanel3.Visible = true;
        rprint.Visible = false;
        txtexcel.Text = "";
        myTab1.Visible = true;
        myTab2.Visible = true;
    }

    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ddlcollege.Items.Clear();
            selq = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"].ToString() + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch { }
    }

    protected void ddlcollege_Change(object sender, EventArgs e)
    {
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        bind_batch();
        binddegree();
        bindbranch();
        bindsem();
        bindCertificate();
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            Fpspreadpop.SaveChanges();
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                txtexcel.Text = "";
                d2.printexcelreport(Fpspreadpop, reportname);
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
            string degreedetails = "";
            if (rdb_cummulate.Checked)
                degreedetails = "Student Cummulative Certificate Report";
            else if (rdb_individual.Checked)
                degreedetails = "Individual Student Certificate Report";
            string pagename = "CertificateCummulativeReport.aspx";
            Printcontrol.loadspreaddetails(Fpspreadpop, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }

    protected void ddl_batch_change(object sender, EventArgs e)
    {
        bindsem();
    }

    protected void ddl_degree_change(object sender, EventArgs e)
    {
        bindbranch();
        bindCertificate();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string degree = "";
            string branch = "";
            string myDegree = "";
            string myBranch = "";
            string CertName = "";
            string myCertName = "";

            degree = GetSelectedItemsValueAsString(cbl_degree);
            branch = GetSelectedItemsValueAsString(cbl_branch);
            CertName = GetSelectedItemsValueAsString(cblCertName);

            myDegree = "'" + degree + "'";
            myBranch = "'" + branch + "'";
            myCertName = "'" + CertName + "'";

            if (ddl_batch.SelectedItem.Text.Trim() == "Select")
            {
                lblmainerr.Visible = true;
                lblmainerr.Text = "Please Select Batch Year!";
                Fpspreadpop.Visible = false;
                rprint.Visible = false;
                return;
            }
            if (rdb_cummulate.Checked && String.IsNullOrEmpty(degree))
            {
                lblmainerr.Visible = true;
                lblmainerr.Text = "Please Select Any Degree!";
                Fpspreadpop.Visible = false;
                rprint.Visible = false;
                return;
            }
            if (rdb_individual.Checked)
            {
                if (Convert.ToString(ddl_degree.SelectedItem.Text).Trim() == "Select")
                {
                    lblmainerr.Visible = true;
                    lblmainerr.Text = "Please Select Any Degree!";
                    Fpspreadpop.Visible = false;
                    rprint.Visible = false;
                    return;
                }
                if (String.IsNullOrEmpty(CertName))
                {
                    lblmainerr.Visible = true;
                    lblmainerr.Text = "Please Select Any Certificate!";
                    Fpspreadpop.Visible = false;
                    rprint.Visible = false;
                    return;
                }
            }
            if (String.IsNullOrEmpty(branch))
            {
                lblmainerr.Visible = true;
                lblmainerr.Text = "Please Select Any Branch!";
                Fpspreadpop.Visible = false;
                rprint.Visible = false;
                return;
            }
            if (ddl_sem.SelectedItem.Text.Trim() == "Select")
            {
                lblmainerr.Visible = true;
                lblmainerr.Text = "Please Select Semester!";
                Fpspreadpop.Visible = false;
                rprint.Visible = false;
                return;
            }
            bindGrid(myBranch, myCertName, Convert.ToString(ddl_sem.SelectedItem.Text), collegecode1);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CertificateCummulativeReport.aspx"); }
    }

    private void LoadHeader()
    {
        Fpspreadpop.Visible = true;
        rprint.Visible = true;
        Fpspreadpop.Sheets[0].ColumnCount = 0;
        Fpspreadpop.Sheets[0].RowCount = 0;
        Fpspreadpop.CommandBar.Visible = false;
        Fpspreadpop.RowHeader.Visible = false;
        Fpspreadpop.Sheets[0].AutoPostBack = true;
        ArrayList arr1 = new ArrayList();
        ArrayList arr2 = new ArrayList();

        arr1.Clear();
        arr2.Clear();

        arr1.Add("Original");
        arr1.Add("Duplicate");
        arr2.Add("Received");
        arr2.Add("Pending");

        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.Black;
        darkstyle.HorizontalAlign = HorizontalAlign.Center;
        darkstyle.Font.Bold = true;
        darkstyle.Font.Name = "Book Antiqua";

        Fpspreadpop.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;
        Fpspreadpop.Sheets[0].RowCount = 0;
        if (rdb_cummulate.Checked)
        {
            Fpspreadpop.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspreadpop.Sheets[0].ColumnCount = 6;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Admitted";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Received";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Pending";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 5].Text = "May be Cancelled";

            Fpspreadpop.Columns[0].Width = 75;
            Fpspreadpop.Columns[0].Locked = true;
            Fpspreadpop.Columns[1].Width = 200;
            Fpspreadpop.Columns[1].Locked = true;
            Fpspreadpop.Columns[2].Width = 100;
            Fpspreadpop.Columns[2].Locked = true;
            Fpspreadpop.Columns[3].Width = 100;
            Fpspreadpop.Columns[3].Locked = true;
            Fpspreadpop.Columns[4].Width = 100;
            Fpspreadpop.Columns[4].Locked = true;
            Fpspreadpop.Columns[5].Width = 125;
            Fpspreadpop.Columns[5].Locked = true;
        }
        else
        {
            Fpspreadpop.Sheets[0].ColumnHeader.RowCount = 3;
            Fpspreadpop.Sheets[0].ColumnCount = 0;

            Fpspreadpop.Sheets[0].ColumnCount++;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, Fpspreadpop.Sheets[0].ColumnCount - 1].Text = "S.No";
            Fpspreadpop.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspreadpop.Sheets[0].ColumnCount - 1, 3, 1);
            Fpspreadpop.Sheets[0].ColumnCount++;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, Fpspreadpop.Sheets[0].ColumnCount - 1].Text = "Department";
            Fpspreadpop.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspreadpop.Sheets[0].ColumnCount - 1, 3, 1);
            Fpspreadpop.Sheets[0].ColumnCount++;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, Fpspreadpop.Sheets[0].ColumnCount - 1].Text = "Admitted";
            Fpspreadpop.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspreadpop.Sheets[0].ColumnCount - 1, 3, 1);

            for (int co = 0; co < cblCertName.Items.Count; co++)
            {
                if (cblCertName.Items[co].Selected == true)
                {
                    int colspan = 0;
                    for (int or1 = 0; or1 < arr2.Count; or1++)
                    {
                        for (int or = 0; or < arr1.Count; or++)
                        {
                            colspan++;
                            Fpspreadpop.Sheets[0].ColumnCount++;
                            Fpspreadpop.Sheets[0].ColumnHeader.Cells[2, Fpspreadpop.Sheets[0].ColumnCount - 1].Text = Convert.ToString(arr1[or]);
                        }
                        Fpspreadpop.Sheets[0].ColumnHeader.Cells[1, Fpspreadpop.Sheets[0].ColumnCount - 2].Text = Convert.ToString(arr2[or1]);

                        Fpspreadpop.Sheets[0].ColumnHeaderSpanModel.Add(1, Fpspreadpop.Sheets[0].ColumnCount - 2, 1, 2);
                        if (or1 == 0)
                            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, Fpspreadpop.Sheets[0].ColumnCount - 2].Text = Convert.ToString(cblCertName.Items[co].Text);
                    }
                    Fpspreadpop.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspreadpop.Sheets[0].ColumnCount - colspan, 1, colspan);
                }
            }
        }
    }

    private void bindGrid(string Degree, string CertName, string currsem, string collcode)
    {
        try
        {
            selq = "";
            int ColIdx = 0;
            Dictionary<int, int> dictTot = new Dictionary<int, int>();
            dictTot.Clear();
            int AdmitAmnt = 0;
            int RecAmnt = 0;
            int DicAmnt = 0;
            int OrgAmnt = 0;
            int DupAmnt = 0;
            DateTime DtFrm = new DateTime();
            DateTime DtTo = new DateTime();
            string[] spldt = new string[2];
            spldt = Convert.ToString(txtfrmDt.Text).Split('/');
            if (spldt.Length == 3)
                DtFrm = Convert.ToDateTime(spldt[1] + "/" + spldt[0] + "/" + spldt[2]);
            spldt = Convert.ToString(txttoDt.Text).Split('/');
            if (spldt.Length == 3)
                DtTo = Convert.ToDateTime(spldt[1] + "/" + spldt[0] + "/" + spldt[2]);
            ds.Clear();
            DataView dvnew = new DataView();
            LoadHeader();
            selq = " select count(distinct r.App_no) as Admitted,r.degree_code,dt.dept_acronym,c.Course_Name,r.Current_Semester from registration r,Degree d,Department dt,Course c where r.degree_code=d.degree_code and c.course_id=d.course_id and d.dept_code=dt.dept_code and r.degree_code in (" + Degree + ") and r.Current_semester ='" + currsem + "' and r.college_code ='" + collcode + "' group by r.degree_code,dt.dept_acronym,r.Current_Semester,c.Course_Name order by dt.dept_acronym";

            selq = selq + " select count(distinct Cer.App_no) as Received,r.degree_code,Cer.CertificateId,dt.dept_acronym,r.Current_Semester from registration r,Degree d,Department dt,Course c,StudCertDetails_New Cer where Cer.app_no=r.app_no and r.degree_code=d.degree_code and c.course_id=d.course_id and d.dept_code=dt.dept_code and r.college_code='" + collcode + "' and Cert_RecDate between '" + DtFrm.ToString("MM/dd/yyyy") + "' and '" + DtTo.ToString("MM/dd/yyyy") + "'";

            if (rdb_individual.Checked)
                selq = selq + " and IsOrginal='1' and CertificateId in (" + CertName + ")";

            selq = selq + " group by r.degree_code,dt.dept_acronym,r.Current_Semester,Cer.CertificateId order by dt.dept_acronym";

            if (rdb_individual.Checked)
            {
                selq = selq + " select count(distinct Cer.App_no) as Received,r.degree_code,Cer.CertificateId,dt.dept_acronym,r.Current_Semester from registration r,Degree d,Department dt,Course c,StudCertDetails_New Cer where Cer.app_no=r.app_no and r.degree_code=d.degree_code and c.course_id=d.course_id and d.dept_code=dt.dept_code and r.college_code='" + collcode + "' and Cert_RecDate between '" + DtFrm.ToString("MM/dd/yyyy") + "' and '" + DtTo.ToString("MM/dd/yyyy") + "' and IsDuplicate='1' and CertificateId in (" + CertName + ") group by r.degree_code,dt.dept_acronym,r.Current_Semester,Cer.CertificateId order by dt.dept_acronym";
            }

            ds = d2.select_method_wo_parameter(selq, "Text");

            if (rdb_cummulate.Checked)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                    {
                        DicAmnt = 0;
                        Fpspreadpop.Sheets[0].RowCount++;

                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ik + 1);
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[ik]["Course_Name"] + " - " + ds.Tables[0].Rows[ik]["dept_acronym"]);
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[ik]["Admitted"]);
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                        Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[ik]["Admitted"]), out AdmitAmnt);
                        if (!dictTot.ContainsKey(2))
                            dictTot.Add(2, AdmitAmnt);
                        else
                        {
                            Int32.TryParse(Convert.ToString(dictTot[2]), out DicAmnt);
                            DicAmnt = DicAmnt + AdmitAmnt;
                            dictTot.Remove(2);
                            dictTot.Add(2, DicAmnt);
                        }

                        ds.Tables[1].DefaultView.RowFilter = " Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[ik]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[ik]["degree_code"]) + "'";
                        dvnew = ds.Tables[1].DefaultView;
                        if (dvnew.Count > 0)
                        {
                            Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvnew[0]["Received"]);
                            Int32.TryParse(Convert.ToString(dvnew[0]["Received"]), out RecAmnt);
                        }
                        else
                        {
                            Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("0");
                            RecAmnt = 0;
                        }
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                        if (!dictTot.ContainsKey(3))
                            dictTot.Add(3, RecAmnt);
                        else
                        {
                            Int32.TryParse(Convert.ToString(dictTot[3]), out DicAmnt);
                            DicAmnt = DicAmnt + RecAmnt;
                            dictTot.Remove(3);
                            dictTot.Add(3, DicAmnt);
                        }

                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(AdmitAmnt - RecAmnt);
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                        if (!dictTot.ContainsKey(4))
                            dictTot.Add(4, (AdmitAmnt - RecAmnt));
                        else
                        {
                            Int32.TryParse(Convert.ToString(dictTot[4]), out DicAmnt);
                            DicAmnt = DicAmnt + (AdmitAmnt - RecAmnt);
                            dictTot.Remove(4);
                            dictTot.Add(4, DicAmnt);
                        }

                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("0");
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    }

                    Fpspreadpop.Sheets[0].RowCount++;
                    Fpspreadpop.Sheets[0].Rows[Fpspreadpop.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Text = Convert.ToString("Total");
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Bold = true;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dictTot[2]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Bold = true;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dictTot[3]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Bold = true;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dictTot[4]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Font.Bold = true;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("0");
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].Font.Bold = true;

                    Fpspreadpop.Sheets[0].PageSize = Fpspreadpop.Sheets[0].RowCount;
                    lblmainerr.Visible = false;
                }
                else
                {
                    rprint.Visible = false;
                    Fpspreadpop.Visible = false;
                    lblmainerr.Visible = true;
                    lblmainerr.Text = "No Records Found!";
                }
            }
            else if (rdb_individual.Checked)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                    {
                        DicAmnt = 0;
                        ColIdx = 0;
                        AdmitAmnt = 0;
                        Fpspreadpop.Sheets[0].RowCount++;
                        ColIdx++;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Text = Convert.ToString(ik + 1);
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Size = FontUnit.Medium;

                        ColIdx++;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Text = Convert.ToString(ds.Tables[0].Rows[ik]["Course_Name"] + " - " + ds.Tables[0].Rows[ik]["dept_acronym"]);
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Size = FontUnit.Medium;

                        ColIdx++;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Text = Convert.ToString(ds.Tables[0].Rows[ik]["Admitted"]);
                        Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[ik]["Admitted"]), out AdmitAmnt);
                        if (!dictTot.ContainsKey(ColIdx - 1))
                            dictTot.Add(ColIdx - 1, AdmitAmnt);
                        else
                        {
                            Int32.TryParse(Convert.ToString(dictTot[ColIdx - 1]), out DicAmnt);
                            DicAmnt = DicAmnt + AdmitAmnt;
                            dictTot.Remove(ColIdx - 1);
                            dictTot.Add(ColIdx - 1, DicAmnt);
                        }
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Size = FontUnit.Medium;

                        for (int ro = 0; ro < cblCertName.Items.Count; ro++)
                        {
                            OrgAmnt = 0;
                            DupAmnt = 0;
                            if (cblCertName.Items[ro].Selected == true)
                            {
                                ColIdx++;
                                ds.Tables[1].DefaultView.RowFilter = " CertificateId ='" + Convert.ToString(cblCertName.Items[ro].Value) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[ik]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[ik]["degree_code"]) + "'";
                                dvnew = ds.Tables[1].DefaultView;
                                if (dvnew.Count > 0)
                                {
                                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Text = Convert.ToString(dvnew[0]["Received"]);
                                    Int32.TryParse(Convert.ToString(dvnew[0]["Received"]), out OrgAmnt);
                                }
                                else
                                {
                                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Text = Convert.ToString("0");
                                    OrgAmnt = 0;
                                }
                                if (!dictTot.ContainsKey(ColIdx - 1))
                                    dictTot.Add(ColIdx - 1, OrgAmnt);
                                else
                                {
                                    Int32.TryParse(Convert.ToString(dictTot[ColIdx - 1]), out DicAmnt);
                                    DicAmnt = DicAmnt + OrgAmnt;
                                    dictTot.Remove(ColIdx - 1);
                                    dictTot.Add(ColIdx - 1, DicAmnt);
                                }
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Name = "Book Antiqua";
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Size = FontUnit.Medium;

                                ColIdx++;
                                ds.Tables[2].DefaultView.RowFilter = " CertificateId ='" + Convert.ToString(cblCertName.Items[ro].Value) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[ik]["Current_Semester"]) + "' and degree_code='" + Convert.ToString(ds.Tables[0].Rows[ik]["degree_code"]) + "'";
                                dvnew = ds.Tables[2].DefaultView;
                                if (dvnew.Count > 0)
                                {
                                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Text = Convert.ToString(dvnew[0]["Received"]);
                                    Int32.TryParse(Convert.ToString(dvnew[0]["Received"]), out DupAmnt);
                                }
                                else
                                {
                                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Text = Convert.ToString("0");
                                    DupAmnt = 0;
                                }
                                if (!dictTot.ContainsKey(ColIdx - 1))
                                    dictTot.Add(ColIdx - 1, DupAmnt);
                                else
                                {
                                    Int32.TryParse(Convert.ToString(dictTot[ColIdx - 1]), out DicAmnt);
                                    DicAmnt = DicAmnt + DupAmnt;
                                    dictTot.Remove(ColIdx - 1);
                                    dictTot.Add(ColIdx - 1, DicAmnt);
                                }
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Name = "Book Antiqua";
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Size = FontUnit.Medium;

                                ColIdx++;
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Text = Convert.ToString(AdmitAmnt - OrgAmnt);
                                if (!dictTot.ContainsKey(ColIdx - 1))
                                    dictTot.Add(ColIdx - 1, (AdmitAmnt - OrgAmnt));
                                else
                                {
                                    Int32.TryParse(Convert.ToString(dictTot[ColIdx - 1]), out DicAmnt);
                                    DicAmnt = DicAmnt + (AdmitAmnt - OrgAmnt);
                                    dictTot.Remove(ColIdx - 1);
                                    dictTot.Add(ColIdx - 1, DicAmnt);
                                }
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Name = "Book Antiqua";
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Size = FontUnit.Medium;

                                ColIdx++;
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Text = Convert.ToString(AdmitAmnt - DupAmnt);
                                if (!dictTot.ContainsKey(ColIdx - 1))
                                    dictTot.Add(ColIdx - 1, (AdmitAmnt - DupAmnt));
                                else
                                {
                                    Int32.TryParse(Convert.ToString(dictTot[ColIdx - 1]), out DicAmnt);
                                    DicAmnt = DicAmnt + (AdmitAmnt - DupAmnt);
                                    dictTot.Remove(ColIdx - 1);
                                    dictTot.Add(ColIdx - 1, DicAmnt);
                                }
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Name = "Book Antiqua";
                                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, ColIdx - 1].Font.Size = FontUnit.Medium;
                            }
                        }
                    }

                    Fpspreadpop.Sheets[0].RowCount++;
                    Fpspreadpop.Sheets[0].Rows[Fpspreadpop.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Text = Convert.ToString("Total");
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    foreach (KeyValuePair<int, int> dr in dictTot)
                    {
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, dr.Key].Text = Convert.ToString(dr.Value);
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, dr.Key].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, dr.Key].Font.Bold = true;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, dr.Key].Font.Name = "Book Antiqua";
                    }

                    Fpspreadpop.Sheets[0].PageSize = Fpspreadpop.Sheets[0].RowCount;
                    lblmainerr.Visible = false;
                }
                else
                {
                    rprint.Visible = false;
                    Fpspreadpop.Visible = false;
                    lblmainerr.Visible = true;
                    lblmainerr.Text = "No Records Found!";
                }
            }
        }
        catch { }
    }

    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_degree, cbl_degree, txt_degree, "Degree");
        bindbranch();
        bindsem();
    }

    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_degree, cbl_degree, txt_degree, "Degree");
        bindbranch();
        bindsem();
    }

    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_branch, cbl_branch, txt_branch, "Branch");
        bindsem();
    }

    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_branch, cbl_branch, txt_branch, "Branch");
        bindsem();
    }

    protected void cbCertName_checkedchange(object sender, EventArgs e)
    {
        chkchange(cbCertName, cblCertName, txtCertName, "Certificate");
    }

    protected void cblCertName_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cbCertName, cblCertName, txtCertName, "Certificate");
    }

    public void bind_batch()
    {
        try
        {
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct batch_year from tbl_attendance_rights order by batch_year desc", "text");
            ddl_batch.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
            bindsem();
        }
        catch { }
    }

    protected void binddegree()
    {
        try
        {
            ds.Clear();
            selq = "";
            if (usercode != "")
            {
                selq = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + Convert.ToString(collegecode1) + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + " order by course.course_name";
            }
            else
            {
                selq = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + Convert.ToString(collegecode1) + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + " order by course.course_name";
            }
            ds = d2.select_method_wo_parameter(selq, "Text");
            cbl_degree.Items.Clear();
            ddl_degree.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();

                ddl_degree.DataSource = ds;
                ddl_degree.DataTextField = "course_name";
                ddl_degree.DataValueField = "course_id";
                ddl_degree.DataBind();
                ddl_degree.Items.Insert(0, "Select");
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = lbl_degree.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }
            else
            {
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
            }
            bindbranch();
            bindsem();
            bindCertificate();
        }
        catch { }
    }

    public void bindbranch()
    {
        try
        {
            selq = "";
            string buildvalue1 = "";
            if (cbl_degree.Items.Count > 0)
            {
                if (rdb_cummulate.Checked == true)
                    buildvalue1 = GetSelectedItemsValueAsString(cbl_degree);
                else if (rdb_individual.Checked)
                    buildvalue1 = Convert.ToString(ddl_degree.SelectedItem.Value);

                if (rdb_cummulate.Checked == true && String.IsNullOrEmpty(buildvalue1))
                    buildvalue1 = "0";
                if (rdb_individual.Checked && Convert.ToString(ddl_degree.SelectedItem.Text) == "Select")
                    buildvalue1 = "0";
                selq = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "'";
                ds = d2.select_method_wo_parameter(selq, "Text");
                cbl_branch.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            cbl_branch.Items[i].Selected = true;
                        }
                        txt_branch.Text = lbl_branch.Text + "(" + cbl_branch.Items.Count + ")";
                        cb_branch.Checked = true;
                    }
                }
                else
                {
                    txt_branch.Text = "--Select--";
                    cb_branch.Checked = false;
                }
            }
            bindsem();
        }
        catch (Exception ex) { }
    }

    private void bindsem()
    {
        try
        {
            string branch = "";
            string batch = "";
            int duration = 0;
            selq = "";
            ds.Clear();
            ddl_sem.Items.Clear();
            branch = GetSelectedItemsValueAsString(cbl_branch);
            batch = Convert.ToString(ddl_batch.SelectedItem.Text);
            if (!String.IsNullOrEmpty(branch) && batch.Trim() != "Select")
            {
                ds = d2.BindSem("'" + branch + "'", "'" + batch + "'", collegecode1);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    Int32.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]), out duration);
                if (duration > 0)
                {
                    for (int s = 0; s < duration; s++)
                    {
                        ddl_sem.Items.Add(new ListItem(Convert.ToString(s + 1), Convert.ToString(s + 1)));
                    }
                    ddl_sem.Items.Insert(0, "Select");
                }
                else
                    ddl_sem.Items.Insert(0, "Select");
            }
            else
                ddl_sem.Items.Insert(0, "Select");
        }
        catch { }
    }

    private void bindCertificate()
    {
        try
        {
            ds.Clear();
            cblCertName.Items.Clear();
            selq = "";
            if (Convert.ToString(ddl_degree.SelectedItem.Text).Trim() != "Select")
            {
                selq = " select distinct mas.MasterValue,c.CertName from CertMasterDet c,CO_MasterValues mas where mas.MasterCode=c.CertName and mas.MasterCriteria='CertificateName' and c.CourseID='" + Convert.ToString(ddl_degree.SelectedItem.Value) + "' order by MasterValue";
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblCertName.DataSource = ds;
                    cblCertName.DataTextField = "MasterValue";
                    cblCertName.DataValueField = "CertName";
                    cblCertName.DataBind();

                    for (int my = 0; my < cblCertName.Items.Count; my++)
                    {
                        cblCertName.Items[my].Selected = true;
                    }
                    txtCertName.Text = "Certificate(" + cblCertName.Items.Count + ")";
                    cbCertName.Checked = true;
                }
                else
                {
                    txtCertName.Text = "--Select--";
                    cbCertName.Checked = false;
                }
            }
            else
            {
                cblCertName.Items.Clear();
                txtCertName.Text = "--Select--";
                cbCertName.Checked = false;
            }
        }
        catch { }
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
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
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
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Text));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsTextnew(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    else
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[j].Text));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }

    private void setLabelText()
    {
        Label lblstr = new Label();
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

        lbl.Add(lblcoll);
        lbl.Add(lblstr);
        lbl.Add(lbl_degree);
        lbl.Add(lbl_branch);
        lbl.Add(lbl_sem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }
}