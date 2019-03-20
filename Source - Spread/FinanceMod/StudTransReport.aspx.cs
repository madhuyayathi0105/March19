using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;


public partial class StudTransReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static int chosedmode = 0;
    static int personmode = 0;
    int userCode = 0;
    static byte roll = 0;
    static int admis = 0;
    static string colgcode = string.Empty;
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds1 = new DataSet();
    DataSet ds = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                bindCollege();
                //if (ddlcollege.Items.Count > 0)
                //{
                //    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                //    colgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
                //}
                setLabelText();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                RollAndRegSettings();
            }
            //if (ddlcollege.Items.Count > 0)
            //{
            //    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            //    colgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
            //}
        }
        catch (Exception ex) { }
    }

    #region college
    protected void bindCollege()
    {
        cblclg.Items.Clear();
        cbclg.Checked = false;
        txtclg.Text = "--Select--";
        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblclg.DataSource = ds;
            cblclg.DataTextField = "collname";
            cblclg.DataValueField = "college_code";
            cblclg.DataBind();
            if (cblclg.Items.Count > 0)
            {
                for (int row = 0; row < cblclg.Items.Count; row++)
                {
                    cblclg.Items[row].Selected = true;
                }
                cbclg.Checked = true;
                txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
            }
        }
    }
    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
    }
    #endregion
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "TransferRefund"); }
    }
    public void bindcollege()
    {
        try
        {
            //ds.Clear();
            //ddlcollege.Items.Clear();
            //string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            //ds = d2.select_method_wo_parameter(selectQuery, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlcollege.DataSource = ds;
            //    ddlcollege.DataTextField = "collname";
            //    ddlcollege.DataValueField = "college_code";
            //    ddlcollege.DataBind();
            //}
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "TransferRefund"); }
    }
    protected void ddlcollege_Selected(object sender, EventArgs e)
    {
        //if (ddlcollege.Items.Count > 0)
        //{
        //    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        //    colgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
        //}
    }
    #region Report


    protected void btnrptgo_Click(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ds = loadDatasetDet();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                RollAndRegSettings();
                loadTransferReport(ds);
            }
            else
            {
                gridReport.Visible = false;
                btnExport.Visible = false;
                pnlContents.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found')", true);
            }
        }
        catch { }
    }

    protected DataSet loadDatasetDet()
    {
        DataSet dsload = new DataSet();
        try
        {
            string collegecode = "";
            //if (ddlcollege.Items.Count > 0)
            //    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();

            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();

            // string SeleQ = " select r.app_no,r.degree_code,r.sections,r.college_code,r.batch_year,r.roll_no,r.reg_no,r.Roll_Admit,r.stud_name,convert(varchar(10),st.TransferDate,103)as TransferDate,st.FromDegree,st.Todegree,st.FromSection,st.ToSection,st.FromCollege,st.Tocollege from ST_Student_Transfer st,registration r where st.appno=r.app_no  and st.TransferDate between '" + fromdate + "' and '" + todate + "' ";
            string SeleQ = string.Empty;
            if (rblType.SelectedIndex == 1)
            {
                SeleQ = " select r.app_no,r.stud_name,r.roll_no,r.reg_no,r.roll_admit,r.batch_year,st.FromDegree,st.Todegree,st.FromSection,st.ToSection,st.FromCollege,st.Tocollege,st.fromseattype,st.ToSeatType,convert(varchar(10),st.TransferDate,103)as TransferDate,convert(varchar(10),r.adm_date,103)as adm_date,Old_RollNo,Old_RegNo,Old_RollAdmit,Old_ReceiptNo,convert(varchar(10),Old_ReceiptDate,103)as Old_ReceiptDate,Old_Amt,New_ReceiptNo,convert(varchar(10),New_ReceiptDate,103)as New_ReceiptDate,New_Amt,New_ExcessAmt from registration r,st_student_transfer st,ST_Student_Transfer_Details std where r.app_no=st.appno and st.StudentTransferPK=std.StudentTransferfK and st.TransferDate between '" + fromdate + "' and '" + todate + "' and r.college_code in('" + collegecode + "') ";
            }
            else
            {
                SeleQ = " select r.app_no,r.stud_name,app_formno as roll_no,app_formno as reg_no,app_formno as roll_admit,r.batch_year,st.FromDegree,st.Todegree,st.FromSection,st.ToSection,st.FromCollege,st.Tocollege,st.fromseattype,st.ToSeatType,convert(varchar(10),st.TransferDate,103)as TransferDate,''adm_date,Old_RollNo,Old_RegNo,Old_RollAdmit,Old_ReceiptNo,convert(varchar(10),Old_ReceiptDate,103)as Old_ReceiptDate,Old_Amt,New_ReceiptNo,convert(varchar(10),New_ReceiptDate,103)as New_ReceiptDate,New_Amt,New_ExcessAmt from applyn r,st_student_transfer st,ST_Student_Transfer_Details std where r.app_no=st.appno and st.StudentTransferPK=std.StudentTransferfK and st.TransferDate between '" + fromdate + "' and '" + todate + "' and r.college_code in('" + collegecode + "') ";
            }
            //and r.college_code='" + collegecode + "'
            //and r.degree_code=st.FromDegree and r.college_code=st.FromCollege
            SeleQ += " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  d.college_code in('" + collegecode + "') ";
            //and d.college_code ='" + collegecode + "'
            SeleQ += " select collname,college_code,acr from collinfo";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SeleQ, "Text");
        }
        catch { }
        return dsload;
    }
    protected void loadTransferReport(DataSet ds)
    {
        try
        {
            DataTable dtrpt = new DataTable();
            dtrpt.Columns.Add("Sno");
            dtrpt.Columns.Add("Roll No");
            dtrpt.Columns.Add("Old Roll No");
            dtrpt.Columns.Add("Reg No");
            dtrpt.Columns.Add("Old Reg No");
            dtrpt.Columns.Add("Admission No");
            dtrpt.Columns.Add("Old Admission No");
            dtrpt.Columns.Add("Name");
            dtrpt.Columns.Add("Admit Date");
            dtrpt.Columns.Add("Transfer Date");
            dtrpt.Columns.Add("From Department");
            dtrpt.Columns.Add("To Department");
            dtrpt.Columns.Add("From Section");
            dtrpt.Columns.Add("To Section");
            dtrpt.Columns.Add("From SeatType");
            dtrpt.Columns.Add("To SeatType");
            dtrpt.Columns.Add("From College");
            dtrpt.Columns.Add("To College");
            dtrpt.Columns.Add("Journal No");
            dtrpt.Columns.Add("Old Receipt No");
            dtrpt.Columns.Add("Journal Date");
            dtrpt.Columns.Add("Old Receipt Date");
            dtrpt.Columns.Add("Amt");
            dtrpt.Columns.Add("Old Amt");
            dtrpt.Columns.Add("Advance Amt");

            DataRow drrpt;
            if (dtrpt.Columns.Count > 0)
            {
                for (int dsrow = 0; dsrow < ds.Tables[0].Rows.Count; dsrow++)
                {
                    drrpt = dtrpt.NewRow();
                    drrpt["Sno"] = Convert.ToString(dsrow + 1);
                    drrpt["Roll No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["roll_no"]);
                    drrpt["Old Roll No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["Old_RollNo"]);
                    drrpt["Reg No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["reg_no"]);
                    drrpt["Old Reg No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["Old_RegNo"]);
                    drrpt["Admission No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["Roll_Admit"]);
                    drrpt["Old Admission No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["Old_RollAdmit"]);
                    drrpt["Name"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["stud_name"]);
                    drrpt["Admit Date"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["adm_date"]);
                    drrpt["Transfer Date"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["TransferDate"]);
                    string frdept = deptName(ds, Convert.ToString(ds.Tables[0].Rows[dsrow]["FromDegree"]));
                    drrpt["From Department"] = frdept;
                    string todept = deptName(ds, Convert.ToString(ds.Tables[0].Rows[dsrow]["Todegree"]));
                    drrpt["To Department"] = todept;
                    drrpt["From Section"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["FromSection"]);
                    drrpt["To Section"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["ToSection"]);
                    drrpt["From SeatType"] = getSeatName(Convert.ToString(ds.Tables[0].Rows[dsrow]["fromseattype"]));
                    drrpt["To SeatType"] = getSeatName(Convert.ToString(ds.Tables[0].Rows[dsrow]["ToSeatType"]));
                    string frclg = collegeName(ds, Convert.ToString(ds.Tables[0].Rows[dsrow]["FromCollege"]));
                    drrpt["From College"] = frclg;
                    string toclg = collegeName(ds, Convert.ToString(ds.Tables[0].Rows[dsrow]["Tocollege"]));
                    drrpt["To College"] = toclg;
                    drrpt["Journal No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["New_ReceiptNo"]);
                    drrpt["Old Receipt No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["Old_ReceiptNo"]);
                    drrpt["Journal Date"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["New_ReceiptDate"]);
                    drrpt["Old Receipt Date"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["Old_ReceiptDate"]);
                    drrpt["Amt"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["New_Amt"]);
                    drrpt["Old Amt"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["Old_Amt"]);
                    drrpt["Advance Amt"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["New_ExcessAmt"]);

                    dtrpt.Rows.Add(drrpt);
                }
            }
            if (dtrpt.Rows.Count > 0)
            {
                gridReport.DataSource = dtrpt;
                gridReport.DataBind();
                columnCount();
                gridReport.Visible = true;
                btnExport.Visible = true;
                pnlContents.Visible = true;
                printCollegeDet();
            }
        }
        catch { }
    }

    protected string getSeatName(string seatCode)
    {
        string seatText = string.Empty;
        seatText = d2.GetFunction("select textval from textvaltable where textcriteria='seat' and textcode='" + seatCode + "'");
        seatText = seatText == "0" ? "" : seatText;
        return seatText;
    }
    protected void columnCount()
    {
        try
        {
            int Cnt = gridReport.Rows[0].Cells.Count;
            if (Cnt > 10)
                btnExport.Text = "Print A3 Format";
            else
                btnExport.Text = "Print A4 Format";
        }
        catch { }
    }
    protected void printCollegeDet()
    {
        try
        {
            string collegecode = string.Empty;
            if (cblclg.Items.Count > 0)
            {
                for (int row = 0; row < cblclg.Items.Count; row++)
                {
                    if (cblclg.Items[row].Selected && string.IsNullOrEmpty(collegecode))
                        collegecode = Convert.ToString(cblclg.Items[row].Value);
                }
            }
            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode + " ";

            string collegename = "";
            string add1 = "";
            string add2 = "";
            string add3 = "";
            string univ = "";
            string feedet = "";
            ds = d2.select_method_wo_parameter(colquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                add1 += " " + add2;
                spCollege.InnerText = collegename;
                spAffBy.InnerText = add1;
                spController.InnerText = add3;
                spSeating.InnerText = univ;
                spDateSession.InnerText = "STUDENT TRANSFER DETAILS-" + DateTime.Now.ToString("dd.MM.yyyy") + "";
            }
        }
        catch { }
    }

    protected string deptName(DataSet ds, string deptcode)
    {
        string Degreename = "";
        try
        {
            DataView Dview = new DataView();
            if (ds.Tables[1].Rows.Count > 0)
            {
                ds.Tables[1].DefaultView.RowFilter = "Degree_code='" + deptcode + "'";//degreename
                Dview = ds.Tables[1].DefaultView;
                if (Dview.Count > 0)
                    Degreename = Convert.ToString(Dview[0]["dept_acronym"]);
            }
        }
        catch { }
        return Degreename;
    }
    protected string collegeName(DataSet ds, string clgcode)
    {
        string collname = "";
        try
        {
            DataView Dview = new DataView();
            if (ds.Tables[2].Rows.Count > 0)
            {
                ds.Tables[2].DefaultView.RowFilter = "college_code='" + clgcode + "'";
                Dview = ds.Tables[2].DefaultView;
                if (Dview.Count > 0)
                    collname = Convert.ToString(Dview[0]["collname"]);//modified by abarna
            }
        }
        catch { }
        return collname;
    }

    protected void gridReport_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            e.Row.Cells[0].Width = 50;
            e.Row.Cells[1].Width = 400;
            e.Row.Cells[2].Width = 500;
            e.Row.Cells[3].Width = 300;
            e.Row.Cells[4].Width = 300;
            e.Row.Cells[5].Width = 300;
            e.Row.Cells[6].Width = 300;
            e.Row.Cells[7].Width = 300;
            e.Row.Cells[8].Width = 300;
            e.Row.Cells[9].Width = 300;
            // e.Row.Cells[10].Width = 250;

            #region
            if (roll == 0)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[5].Visible = true;

                e.Row.Cells[2].Visible = true;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 1)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[5].Visible = true;

                e.Row.Cells[2].Visible = true;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 2)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = false;

                e.Row.Cells[2].Visible = true;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[6].Visible = false;
            }
            else if (roll == 3)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[5].Visible = false;

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[6].Visible = false;
            }
            else if (roll == 4)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = true;

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 5)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[5].Visible = false;

                e.Row.Cells[2].Visible = true;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[6].Visible = false;
            }
            else if (roll == 6)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[5].Visible = true;

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 7)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = true;

                e.Row.Cells[2].Visible = true;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[6].Visible = true;
            }
            #endregion
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

            #region
            if (roll == 0)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[5].Visible = true;

                e.Row.Cells[2].Visible = true;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 1)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[5].Visible = true;

                e.Row.Cells[2].Visible = true;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 2)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = false;

                e.Row.Cells[2].Visible = true;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[6].Visible = false;
            }
            else if (roll == 3)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[5].Visible = false;

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[6].Visible = false;
            }
            else if (roll == 4)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = true;

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 5)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[5].Visible = false;

                e.Row.Cells[2].Visible = true;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[6].Visible = false;
            }
            else if (roll == 6)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[5].Visible = true;

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[6].Visible = true;
            }
            else if (roll == 7)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = true;

                e.Row.Cells[2].Visible = true;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[6].Visible = true;
            }
            #endregion
        }
    }



    #endregion

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
        lbl.Add(lblclg);
        fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }
    //

    protected void btnalert_Click(object sender, EventArgs e)
    {
        divalert.Visible = false;
    }

    // last modified 05.06.2017 sudhagar

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private string getCblSelectedTextwithout(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("," + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
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
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
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
        }
        catch { }
    }

    #endregion
}