using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class DayWise_AdmissionComparison : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    string q1 = "";

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
            lblvalidation1.Visible = false;
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            lblvalidation1.Text = "";
            if (!IsPostBack)
            {
                setLabelText();
                bind_batch();
                bindcollege();
                bind_seattype();
                binddegree();
                bindbranch();
                txtResDt.Attributes.Add("readonly", "readonly");
                txtFrmDt.Attributes.Add("readonly", "readonly");
                txtToDt.Attributes.Add("readonly", "readonly");
                txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtResDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            lbl_error.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch { }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Daywise Admission Comparison Report";
            string pagename = "Daywise_AdmissionComparison.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch { }
    }

    private void LoadHeader(string degree, string batchyear)
    {
        int myBatchYr = 0;
        DataView dvnew = new DataView();
        DateTime PrevDate = new DateTime();
        Int32.TryParse(batchyear, out myBatchYr);
        DateTime GetResDt = new DateTime();
        try
        {
            GetResDt = Convert.ToDateTime(d2.GetFunction("select value from Master_settings where settings='Admission Result Date'"));
        }
        catch
        {
            Fpspread1.Visible = false;
            rptprint.Visible = false;
            lbl_error.Visible = false;
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Text = "Please Set Result Date!";
            return;
        }
        PrevDate = Convert.ToDateTime(GetResDt.Month + "/" + GetResDt.Day + "/" + (myBatchYr - 1));
        Fpspread1.Visible = true;
        Fpspread1.Sheets[0].AutoPostBack = true;
        Fpspread1.CommandBar.Visible = false;
        Fpspread1.Sheets[0].RowHeader.Visible = false;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.Font.Bold = true;
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.HorizontalAlign = HorizontalAlign.Center;
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.Black;

        Fpspread1.Sheets[0].ColumnHeader.RowCount = 3;
        Fpspread1.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;
        Fpspread1.Sheets[0].ColumnCount = 0;
        Fpspread1.Sheets[0].RowCount = 0;

        Fpspread1.Sheets[0].ColumnCount++;
        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Admitted Statistics";
        Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Before Result";
        Fpspread1.Sheets[0].ColumnCount++;
        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 2);
        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 2, 2);

        q1 = "";
        q1 = "select Count(*) as Count,a.seattype,r.batch_year from registration r,applyn a where r.app_no=a.app_no and Adm_Date<='" + PrevDate.ToString("MM/dd/yyyy") + "' and r.degree_code in(" + degree + ") and cc=0 and DelFlag=0 and Exam_Flag<>'debar' group by a.seattype,r.batch_year";

        q1 = q1 + " select Count(*) as Count,a.seattype,r.batch_year from registration r,applyn a where r.app_no=a.app_no and Adm_Date<='" + GetResDt.ToString("MM/dd/yyyy") + "' and r.degree_code in(" + degree + ") and cc=0 and DelFlag=0 and Exam_Flag<>'debar' group by a.seattype,r.batch_year";
        ds.Clear();
        ds = d2.select_method_wo_parameter(q1, "Text");
        for (int ik = 0; ik < cbl_seat.Items.Count; ik++)
        {
            if (cbl_seat.Items[ik].Selected == true)
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_seat.Items[ik].Text);
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_seat.Items[ik].Value);
                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(myBatchYr - 1);
                ds.Tables[0].DefaultView.RowFilter = " seattype='" + Convert.ToString(cbl_seat.Items[ik].Value) + "' and batch_year='" + Convert.ToString(myBatchYr - 1) + "'";
                dvnew = ds.Tables[0].DefaultView;
                if (dvnew.Count > 0)
                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dvnew[0]["Count"]);
                else
                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "-";

                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(myBatchYr);
                ds.Tables[1].DefaultView.RowFilter = " seattype='" + Convert.ToString(cbl_seat.Items[ik].Value) + "' and batch_year='" + Convert.ToString(myBatchYr) + "'";
                dvnew = ds.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dvnew[0]["Count"]);
                else
                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "-";

                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Diff";
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - 3, 1, 3);
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, Fpspread1.Sheets[0].ColumnCount - 1, 2, 1);
            }
        }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            int slno = 0;
            string chkseattype = "";
            string chkbatchyear = "";
            int currAmnt = 0;
            int PrevAmnt = 0;
            DateTime dtFrm = new DateTime();
            DateTime dtTo = new DateTime();
            DateTime dtPrevFrm = new DateTime();
            DateTime dtPrevTo = new DateTime();
            DateTime dtTempFrm = new DateTime();
            DateTime dtTempPrevFrm = new DateTime();
            DataView dvMynew = new DataView();
            dtFrm = getDate(Convert.ToString(txtFrmDt.Text));
            dtTo = getDate(Convert.ToString(txtToDt.Text));
            dtPrevFrm = Convert.ToDateTime(dtFrm.Month + "/" + dtFrm.Day + "/" + (dtFrm.Year - 1));
            dtPrevTo = Convert.ToDateTime(dtTo.Month + "/" + dtTo.Day + "/" + (dtTo.Year - 1));
            dtTempFrm = dtFrm;
            dtTempPrevFrm = dtPrevFrm;
            string seattype = "";
            string mySeatType = "";
            string degree = "";
            string myDegree = "";
            string branch = "";
            string myBranch = "";
            string batchyear = "";
            seattype = returnwithsinglecodevalue(cbl_seat);
            mySeatType = "'" + seattype + "'";
            degree = returnwithsinglecodevalue(cbl_degree);
            myDegree = "'" + degree + "'";
            branch = returnwithsinglecodevalue(cbl_branch);
            myBranch = "'" + branch + "'";
            string GetResDt = d2.GetFunction("select value from Master_settings where settings='Admission Result Date'");
            if (String.IsNullOrEmpty(GetResDt))
            {
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Set Result Date!";
                return;
            }
            if (ddl_batch.Items.Count > 0)
                batchyear = Convert.ToString(ddl_batch.SelectedItem.Text);
            else
            {
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select Batch Year!";
                return;
            }
            if (String.IsNullOrEmpty(degree))
            {
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select Any Degree!";
                return;
            }
            if (String.IsNullOrEmpty(seattype))
            {
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select Any Seat Type!";
                return;
            }
            LoadHeader(myBranch, batchyear);
            q1 = "";
            q1 = " select Count(*) as StudCount,a.seattype,r.batch_year,r.Adm_Date from registration r,applyn a where r.app_no=a.app_no and ((Adm_Date between '" + dtPrevFrm.ToString("MM/dd/yyyy") + "' and '" + dtPrevTo.ToString("MM/dd/yyyy") + "') or (Adm_Date between '" + dtFrm.ToString("MM/dd/yyyy") + "' and '" + dtTo.ToString("MM/dd/yyyy") + "')) and r.degree_code in(" + myBranch + ") and cc=0 and DelFlag=0 and Exam_Flag<>'debar' group by a.seattype,r.batch_year,r.Adm_Date";

            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    lbl_error.Visible = true;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Text = "No Records Found!";
                }
                else
                {
                    while (dtTempFrm <= dtTo)
                    {
                        chkseattype = "";
                        chkbatchyear = "";
                        currAmnt = 0;
                        PrevAmnt = 0;
                        Fpspread1.Sheets[0].RowCount++;
                        slno += 1;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Day " + Convert.ToString(slno) + "\n" + dtTempFrm.ToString("dd/MM/yyyy"));
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = "Per Day";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        for (int ro = 2; ro < Fpspread1.Sheets[0].ColumnCount; ro++)
                        {
                            currAmnt = 0;
                            PrevAmnt = 0;
                            chkseattype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, ro].Tag);
                            chkbatchyear = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, ro].Text);
                            if (!String.IsNullOrEmpty(chkbatchyear) && String.IsNullOrEmpty(chkseattype))
                                chkseattype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, ro - 1].Tag);
                            if (!String.IsNullOrEmpty(chkseattype) && !String.IsNullOrEmpty(chkbatchyear))
                            {
                                if (batchyear != chkbatchyear)
                                    ds.Tables[0].DefaultView.RowFilter = " seattype='" + chkseattype + "' and batch_year='" + chkbatchyear + "' and Adm_Date='" + dtTempPrevFrm.ToString("MM/dd/yyyy") + "'";
                                else
                                    ds.Tables[0].DefaultView.RowFilter = " seattype='" + chkseattype + "' and batch_year='" + chkbatchyear + "' and Adm_Date='" + dtTempFrm.ToString("MM/dd/yyyy") + "'";
                                dvMynew = ds.Tables[0].DefaultView;
                                if (dvMynew.Count > 0)
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Text = Convert.ToString(dvMynew[0]["StudCount"]);
                                else
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Text = "-";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                Int32.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro - 1].Text), out currAmnt);
                                Int32.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro - 2].Text), out PrevAmnt);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Text = Convert.ToString(currAmnt - PrevAmnt);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Font.Bold = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, 0].Text);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = "Total";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        for (int ro = 2; ro < Fpspread1.Sheets[0].ColumnCount; ro++)
                        {
                            if (Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, ro].Text).Trim() != "Diff")
                            {
                                if (slno == 1)
                                {
                                    Int32.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, ro].Text), out currAmnt);
                                    Int32.TryParse(Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, ro].Text), out PrevAmnt);
                                }
                                else
                                {
                                    Int32.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, ro].Text), out currAmnt);
                                    Int32.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 3, ro].Text), out PrevAmnt);
                                }
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Text = Convert.ToString(currAmnt + PrevAmnt);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Font.Bold = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].BackColor = Color.LightGray;
                            }
                            else
                            {
                                Int32.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro - 1].Text), out currAmnt);
                                Int32.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro - 2].Text), out PrevAmnt);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Text = Convert.ToString(currAmnt - PrevAmnt);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Font.Bold = true;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, ro].BackColor = Color.LightGray;
                            }
                        }
                        dtTempFrm = dtTempFrm.AddDays(1);
                        dtTempPrevFrm = dtTempPrevFrm.AddDays(1);
                    }
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Visible = true;
                    rptprint.Visible = true;
                    lbl_error.Visible = false;
                }
            }
        }
        catch { }
    }

    protected void lnkResDt_Click(object sender, EventArgs e)
    {
        popYear.Visible = true;
        string[] splDt = new string[2];
        string selQ = d2.GetFunction("select value from Master_Settings where settings='Admission Result Date'");
        if (!String.IsNullOrEmpty(selQ) && selQ.Trim() != "0")
        {
            splDt = selQ.Split('/');
            txtResDt.Text = Convert.ToString(splDt[1] + "/" + splDt[0] + "/" + splDt[2]);
        }
        else
            txtResDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    protected void btnaddYr_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime ResDt = new DateTime();
            DateTime dtFrm = new DateTime();
            ResDt = getDate(Convert.ToString(txtResDt.Text));
            dtFrm = getDate(Convert.ToString(txtFrmDt.Text));
            if (ResDt > dtFrm)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Result should be less than or Equal to From Date!";
            }
            else
            {
                string settings = "Admission Result Date";

                string insQ = "if exists (Select * from Master_Settings where settings='" + settings + "') Update Master_Settings set value='" + ResDt.ToString("MM/dd/yyyy") + "' where settings='" + settings + "' else insert into Master_Settings (usercode,settings,value) Values ('" + usercode + "','" + settings + "','" + ResDt.ToString("MM/dd/yyyy") + "')";
                int insCount = d2.update_method_wo_parameter(insQ, "Text");
                if (insCount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    popYear.Visible = false;
                    lblalerterr.Text = "Result Year Saved Successfully!";
                }
            }
        }
        catch { }
    }

    protected void btnexitYr_Click(object sender, EventArgs e)
    {
        popYear.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void txtFrmDt_Change(object sender, EventArgs e)
    {
        try
        {
            DateTime ResDt = new DateTime();
            DateTime FrmDt = new DateTime();
            DateTime ToDt = new DateTime();
            string selQ = d2.GetFunction("select value from Master_Settings where settings='Admission Result Date'");
            if (!String.IsNullOrEmpty(selQ))
                ResDt = Convert.ToDateTime(selQ);
            else
                ResDt = Convert.ToDateTime(DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year);
            FrmDt = getDate(Convert.ToString(txtFrmDt.Text));
            ToDt = getDate(Convert.ToString(txtToDt.Text));
            if (FrmDt > ToDt)
            {
                txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbl_error.Visible = true;
                lbl_error.Text = "From Date should be less than or Equal to To Date!";
                return;
            }
            if (ResDt > FrmDt)
            {
                txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbl_error.Visible = true;
                lbl_error.Text = "From Date should be greater than or Equal to Result Date!";
                return;
            }
        }
        catch { }
    }

    protected void txtToDt_Change(object sender, EventArgs e)
    {
        try
        {
            DateTime ResDt = new DateTime();
            DateTime FrmDt = new DateTime();
            DateTime ToDt = new DateTime();
            string selQ = d2.GetFunction("select value from Master_Settings where settings='Admission Result Date'");
            if (!String.IsNullOrEmpty(selQ))
                ResDt = Convert.ToDateTime(selQ);
            else
                ResDt = Convert.ToDateTime(DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year);
            FrmDt = getDate(Convert.ToString(txtFrmDt.Text));
            ToDt = getDate(Convert.ToString(txtToDt.Text));
            if (FrmDt > ToDt)
            {
                txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbl_error.Visible = true;
                lbl_error.Text = "From Date should be less than or Equal to To Date!";
                return;
            }
            if (ResDt > FrmDt)
            {
                txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbl_error.Visible = true;
                lbl_error.Text = "From Date should be greater than or Equal to Result Date!";
                return;
            }
        }
        catch { }
    }

    private DateTime getDate(string date)
    {
        DateTime dt = new DateTime();
        string[] splDt = new string[2];
        splDt = date.Split('/');
        dt = Convert.ToDateTime(splDt[1] + "/" + splDt[0] + "/" + splDt[2]);
        return dt;
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
    }

    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, "--Select--");
        bindbranch();
    }

    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text);
        bindbranch();
    }

    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, "--Select--");
    }

    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text);
    }

    protected void cb_seat_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_seat, cbl_seat, txt_seat, "Seat Type", "--Select--");
    }

    protected void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_seat, cbl_seat, txt_seat, "Seat Type");
    }

    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            q1 = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"].ToString() + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch { }
    }

    protected void binddegree()
    {
        try
        {
            ds.Clear();
            string query = "";
            if (usercode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + " order by course.course_name";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + " order by course.course_name";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            cbl_degree.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
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
        }
        catch { }
    }

    public void bindbranch()
    {
        try
        {
            string query1 = "";
            string buildvalue1 = "";
            if (cbl_degree.Items.Count > 0)
            {
                buildvalue1 = returnwithsinglecodevalue(cbl_degree);
                if (String.IsNullOrEmpty(buildvalue1))
                    buildvalue1 = "0";
                query1 = "select distinct degree.degree_code,(course_name+'-'+department.dept_name) as dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + ddl_college.SelectedValue + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' order by dept_name";
                ds = d2.select_method_wo_parameter(query1, "Text");
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
        }
        catch (Exception ex) { }
    }

    protected void bind_seattype()
    {
        try
        {
            ds.Clear();
            if (ddl_college.Items.Count > 0)
            {
                q1 = " select textcode,textval from textvaltable where TextCriteria='seat' and college_code ='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' order by TextVal";
                ds = d2.select_method_wo_parameter(q1, "text");
                cbl_seat.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_seat.DataSource = ds;
                    cbl_seat.DataTextField = "textval";
                    cbl_seat.DataValueField = "textcode";
                    cbl_seat.DataBind();
                    if (cbl_seat.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_seat.Items.Count; i++)
                        {
                            cbl_seat.Items[i].Selected = true;
                        }
                        txt_seat.Text = "Seat Type(" + cbl_seat.Items.Count + ")";
                        cb_seat.Checked = true;
                    }
                }
                else
                {
                    txt_seat.Text = "--Select--";
                    cb_seat.Checked = false;
                }
            }
        }
        catch { }
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
        }
        catch { }
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        else if (Session["usercode"] != null)
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";

        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lbl_collegename);
        lbl.Add(lbl_degree);
        lbl.Add(lbl_branch);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    protected string returnwithsinglecodevalue(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                    empty = Convert.ToString(cb.Items[i].Value);
                else
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
            }
        }
        return empty;
    }

    protected string returnwithsinglecodetext(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                    empty = Convert.ToString(cb.Items[i].Value);
                else
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
            }
        }
        return empty;
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
                    count++;
            }
            if (count > 0)
            {
                if (count == 1)
                    txt.Text = dipst + "(" + count + ")";
                else
                    txt.Text = dipst + "(" + count + ")";
                if (cbl.Items.Count == count)
                    cb.Checked = true;
            }
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
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
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                else
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
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
}