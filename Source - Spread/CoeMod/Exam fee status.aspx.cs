using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

public partial class Exam_fee_status : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", collegecode1 = "", user_code = "";
    Boolean flag_true = false;
    ArrayList alv = new ArrayList();
    Hashtable hashmark = new Hashtable();
    static int isHeaderwise = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
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
                loadcollege();
                collegecode = ddlcollege.SelectedValue.ToString();
                bindexamyear();
                bindexammonth();
                bindbatch();
                binddegree();
                bindbranch();
                clear();
                lblreport.Visible = false;
                ddlreporttype.Visible = false;
                rbentry.Checked = true;

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";

                string grouporusercode = "";

                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                user_code = Session["usercode"].ToString().Trim();

                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
            }
            collegecode1 = ddlcollege.Items.Count > 0 ? ddlcollege.SelectedValue : "13";
            if (Session["usercode"] != null)
            {
                user_code = Session["usercode"].ToString().Trim();
                usercode = user_code;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void loadcollege()
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
        ds = da.select_method("bind_college", hat, "sp");
        ddlcollege.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.Enabled = true;
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
    }
    public void bindexamyear()
    {
        try
        {
            ddlYear.Items.Clear();
            ds = da.select_method_wo_parameter("select distinct exam_year from exam_details order by exam_year desc", "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "exam_year";
                ddlYear.DataValueField = "exam_year";
                ddlYear.DataBind();
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void bindexammonth()
    {
        try
        {
            ddlMonth.Items.Clear();
            ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
            ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
            ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            ddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
            clear();

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = da.BindBatch();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void binddegree()
    {
        try
        {
            ddldegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", ddlcollege.SelectedValue.ToString());
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void bindbranch()
    {
        try
        {
            ddlbranch.Items.Clear();
            has.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", ddlcollege.SelectedValue.ToString());
            has.Add("user_code", usercode);
            ds = da.select_method("bind_branch", has, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        bindbranch();
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindexammonth();
        bindbatch();
        binddegree();
        bindbranch();
    }
    public void clear()
    {
        FpSpread1.Visible = false;
        btnsave.Visible = false;
        btnChallan.Visible = false;
        btnChallanConf.Visible = false;
        btnChallanDel.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        txtexcelname.Text = "";
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbatch();
            binddegree();
            bindbranch();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void Buttongo_Click(object sender, EventArgs e)
    {
        if (rbentry.Checked == true)
        {
            loadexamdetails();
        }
        else
        {
            loadfeepaidreport();
        }
    }
    public void loadexamdetails()
    {
        try
        {

            FpSpread1.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 6;
            FpSpread1.Sheets[0].Columns[0].Width = 40;
            FpSpread1.Sheets[0].Columns[1].Width = 80;
            FpSpread1.Sheets[0].Columns[2].Width = 130;
            FpSpread1.Sheets[0].Columns[3].Width = 180;
            FpSpread1.Sheets[0].Columns[4].Width = 90;
            FpSpread1.Width = 810;

            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[4].Locked = true;

            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.Black;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].SheetCorner.RowCount = 1;


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Fees";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";

            if (Session["Rollflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[1].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[1].Visible = false;
            }

            if (Session["Regflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.Black;
            style2.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);


            style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = false;
            style2.ForeColor = System.Drawing.Color.Black;
            style2.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

            style2.ForeColor = System.Drawing.Color.Black;

            FpSpread1.Sheets[0].DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FpSpread1.Sheets[0].SheetName = " ";
            FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

            FpSpread1.Sheets[0].AutoPostBack = false;

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = chkcell1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].FrozenRowCount = 1;
            chkcell1.AutoPostBack = true;

            FpSpread1.Sheets[0].AutoPostBack = false;

            string examyear = ddlYear.SelectedValue.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();

            if (examyear == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }

            string year = ddlbatch.SelectedValue.ToString();
            string degree = ddldegree.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string depart_code = ddlbranch.SelectedValue.ToString();
            string batchyearatt = ddlbatch.SelectedValue.ToString();


            string studinfo = "select  len(r.reg_no),r.Roll_No,r.Reg_No,r.Stud_Name,ed.exam_code,ea.appl_no,ea.total_fee,r.app_no,isnull(ea.is_confirm,'0') as is_confirm from Exam_Details ed,exam_application ea,Registration r where ed.exam_code=ea.exam_code and ea.roll_no=r.Roll_No and r.degree_code='" + depart_code + "' and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and ed.exam_month='" + exammonth + "' and exam_year='" + examyear + "' order by len(r.reg_no),r.reg_no,r.stud_name";
            DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
            if (dsstudinfo.Tables[0].Rows.Count > 0)
            {
                btnsave.Visible = true;
                btnChallan.Visible = true;
                btnChallanConf.Visible = true;
                btnChallanDel.Visible = true;
                int sno = 0;
                for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
                {
                    string regno = "";
                    string studname = "";
                    string rollno = "";
                    FpSpread1.Visible = true;
                    sno++;
                    regno = dsstudinfo.Tables[0].Rows[studcount]["reg_no"].ToString();
                    string appNo = dsstudinfo.Tables[0].Rows[studcount]["app_no"].ToString();
                    studname = dsstudinfo.Tables[0].Rows[studcount]["stud_name"].ToString();
                    rollno = dsstudinfo.Tables[0].Rows[studcount]["roll_no"].ToString();
                    string feeamount = dsstudinfo.Tables[0].Rows[studcount]["total_fee"].ToString();
                    if (feeamount.Trim() != "" && feeamount.Trim() != "-")
                    {
                        Double feeval = Convert.ToDouble(feeamount);
                        feeval = Math.Round(feeval, 0, MidpointRounding.AwayFromZero);
                        feeamount = feeval.ToString();
                    }

                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = appNo;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dsstudinfo.Tables[0].Rows[studcount]["exam_code"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = rollno;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = dsstudinfo.Tables[0].Rows[studcount]["appl_no"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = regno;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = studname;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = feeamount;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = chkcell;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    string isconfirmed = dsstudinfo.Tables[0].Rows[studcount]["is_confirm"].ToString().Trim().ToUpper();
                    //if (isconfirmed == "1" || isconfirmed == "TRUE")
                    //{
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Value = 1;
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
                    //}
                }
                CheckFiance();
            }
            else
            {
                clear();
                lblerror.Text = "No Records Found";
                lblerror.Visible = true;
            }
            string totalrows = FpSpread1.Sheets[0].RowCount.ToString();
            FpSpread1.Sheets[0].PageSize = (Convert.ToInt32(totalrows) * 20) + 40;
            FpSpread1.Height = (Convert.ToInt32(totalrows) * 20) + 40;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void loadfeepaidreport()
    {
        try
        {
            FpSpread1.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 6;
            FpSpread1.Sheets[0].Columns[0].Width = 40;
            FpSpread1.Sheets[0].Columns[1].Width = 80;
            FpSpread1.Sheets[0].Columns[2].Width = 130;
            FpSpread1.Sheets[0].Columns[3].Width = 180;
            FpSpread1.Sheets[0].Columns[4].Width = 90;
            FpSpread1.Width = 810;

            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[4].Locked = true;

            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.Black;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].SheetCorner.RowCount = 1;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Fees";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Status";

            if (Session["Rollflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[1].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[1].Visible = false;
            }

            if (Session["Regflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.Black;
            style2.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = false;
            style2.ForeColor = System.Drawing.Color.Black;
            style2.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

            FpSpread1.Sheets[0].DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FpSpread1.Sheets[0].SheetName = " ";
            FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

            FpSpread1.Sheets[0].AutoPostBack = false;

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            string examyear = ddlYear.SelectedValue.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();

            if (examyear == "0")
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Year And Then Proceed";
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() == "0"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Exam Month And Then Proceed";
                return;
            }

            string year = ddlbatch.SelectedValue.ToString();
            string degree = ddldegree.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string depart_code = ddlbranch.SelectedValue.ToString();
            string batchyearatt = ddlbatch.SelectedValue.ToString();
            string exam_code = d2.GetFunction("select exam_code from exam_details where degree_code ='" + degreecode + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "' and batch_year='" + batchyear + "'").Trim();


            string studinfo = "select len(r.reg_no),r.Roll_No,r.Reg_No,r.Stud_Name,ea.exam_code,ea.total_fee,'Paid' status from exam_application ea,Registration r where ea.roll_no=r.Roll_No and isnull(ea.is_confirm,'0')='1' and r.degree_code='" + depart_code + "' and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and ea.exam_code='" + exam_code + "' order by len(r.reg_no),r.reg_no,r.stud_name";
            if (ddlreporttype.SelectedItem.ToString() == "UnPaid")
            {
                studinfo = "select len(r.reg_no),r.Roll_No,r.Reg_No,r.Stud_Name,ea.exam_code,ea.total_fee,'Unpaid' status from exam_application ea,Registration r where ea.roll_no=r.Roll_No and isnull(ea.is_confirm,'0')='0' and r.degree_code='" + depart_code + "' and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and ea.exam_code='" + exam_code + "' order by len(r.reg_no),r.reg_no,r.stud_name";
            }
            else if (ddlreporttype.SelectedItem.ToString() == "Both")
            {
                studinfo = "select len(r.reg_no),r.Roll_No,r.Reg_No,r.Stud_Name,ea.exam_code,ea.total_fee,CASE WHEN isnull(ea.is_confirm,'0')='1' THEN 'Paid' ELSE 'Unpaid' END status from exam_application ea,Registration r where ea.roll_no=r.Roll_No and r.degree_code='" + depart_code + "' and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and ea.exam_code='" + exam_code + "' order by len(r.reg_no),r.reg_no,r.stud_name";
            }
            DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
            if (dsstudinfo.Tables[0].Rows.Count > 0)
            {
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                FpSpread1.Visible = true;
                int sno = 0;
                for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
                {
                    string regno = "";
                    string studname = "";
                    string rollno = "";
                    sno++;
                    regno = dsstudinfo.Tables[0].Rows[studcount]["reg_no"].ToString();
                    studname = dsstudinfo.Tables[0].Rows[studcount]["stud_name"].ToString();
                    rollno = dsstudinfo.Tables[0].Rows[studcount]["roll_no"].ToString();
                    string status = dsstudinfo.Tables[0].Rows[studcount]["status"].ToString();
                    string feeamount = dsstudinfo.Tables[0].Rows[studcount]["total_fee"].ToString();
                    if (feeamount.Trim() != "" && feeamount.Trim() != "-")
                    {
                        Double feeval = Convert.ToDouble(feeamount);
                        feeval = Math.Round(feeval, 0, MidpointRounding.AwayFromZero);
                        feeamount = feeval.ToString();
                    }

                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    if ((sno % 2) == 0)
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightGray;
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dsstudinfo.Tables[0].Rows[studcount]["exam_code"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = rollno;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = regno;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = studname;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = feeamount.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = status;
                    if (status == "UnPaid")
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.Blue;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "-";
                    }
                }
            }
            else
            {
                clear();
                lblerror.Text = "No Records Found";
                lblerror.Visible = true;
            }
            string totalrows = FpSpread1.Sheets[0].RowCount.ToString();
            FpSpread1.Sheets[0].PageSize = (Convert.ToInt32(totalrows) * 20) + 40;
            FpSpread1.Height = (Convert.ToInt32(totalrows) * 20) + 40;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                string[] spiltspreadname = ctrlname.Split('$');
                if (spiltspreadname.GetUpperBound(0) > 1)
                {
                    string getrowxol = spiltspreadname[3].ToString().Trim();
                    string[] spr = getrowxol.Split(',');
                    if (spr.GetUpperBound(0) == 1)
                    {
                        int arow = Convert.ToInt32(spr[0]);
                        int acol = Convert.ToInt32(spr[1]);
                        if (arow == 0 && acol > 4)
                        {
                            string setval = e.EditValues[acol].ToString();
                            int setvalcel = 0;
                            if (setval.Trim().ToLower() == "true" || setval.Trim() == "1")
                            {
                                setvalcel = 1;
                            }
                            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
                            {
                                FpSpread1.Sheets[0].Cells[r, acol].Value = setvalcel;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet sdn = new DataSet();
            FpSpread1.SaveChanges();
            Boolean setflag = false;
            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 5].Value);
                if (stva == 1)
                {
                    setflag = true;
                }
            }
            if (setflag == false)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Student And Then Proceed";
                return;
            }

            string strinsupdatequery = "";
            int upddelval = 0;

            for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                string examcode = FpSpread1.Sheets[0].Cells[r, 1].Tag.ToString();
                string applno = FpSpread1.Sheets[0].Cells[r, 2].Tag.ToString();
                int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 5].Value);
                if (stva == 1)
                {
                    strinsupdatequery = "update Exam_application set is_confirm='1' where roll_no='" + rollno + "' and exam_code='" + examcode + "'";
                    upddelval = da.update_method_wo_parameter(strinsupdatequery, "Text");
                }
                else
                {
                    strinsupdatequery = "update Exam_application set is_confirm='0' where roll_no='" + rollno + "' and exam_code='" + examcode + "'";
                    upddelval = da.update_method_wo_parameter(strinsupdatequery, "Text");
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved Sucessfully.')", true);
            loadexamdetails();

        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void Radiochange(object sender, EventArgs e)
    {
        clear();
        if (rbentry.Checked == true)
        {
            lblreport.Visible = false;
            ddlreporttype.Visible = false;
        }
        else
        {
            lblreport.Visible = true;
            ddlreporttype.Visible = true;
        }
    }

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }
    public string generateReceiptNo(out string rcpracr, out string hdrSetPK, string hdrs)
    {
        hdrSetPK = string.Empty;
        rcpracr = string.Empty;
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + Session["collegecode"].ToString() + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
        }
        catch { isHeaderwise = 0; }
        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + Session["collegecode"].ToString() + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 5)
                return string.Empty;
        }
        catch { return string.Empty; }
        if (isHeaderwise == 0 || isHeaderwise == 2)
        {
            return getCommonReceiptNo(out rcpracr, out hdrSetPK);
        }
        else
        {
            return getHeaderwiseReceiptNo(out rcpracr, out hdrSetPK, hdrs);
        }
    }
    private string getCommonReceiptNo(out string rcpracr, out string hdrSetPK)
    {
        hdrSetPK = string.Empty;
        rcpracr = string.Empty;
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, Session["collegecode"].ToString());

            string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + Session["collegecode"].ToString() + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + Session["collegecode"].ToString() + ")";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }

                string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + Session["collegecode"].ToString() + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + Session["collegecode"].ToString() + ")");
                recacr = acronymquery;
                rcpracr = recacr;

                int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + Session["collegecode"].ToString() + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + Session["collegecode"].ToString() + ")"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
            }
            return recno;
        }
        catch (Exception ex)
        {// d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); 
            return recno;
        }
    }
    private string getHeaderwiseReceiptNo(out string rcpracr, out string hdrSetPK, string hdrs)
    {
        hdrSetPK = string.Empty;
        rcpracr = string.Empty;
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;
            string isheaderFk = hdrs;

            string finYearid = d2.getCurrentFinanceYear(usercode, Session["collegecode"].ToString());

            DataSet dsFinHedDet = d2.select_method_wo_parameter("select distinct HeaderSettingFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + Session["collegecode"].ToString() + " and FinyearFK=" + finYearid + "", "Text");

            if (dsFinHedDet.Tables.Count > 0 && dsFinHedDet.Tables[0].Rows.Count == 1)
            {
                hdrSetPK = Convert.ToString(dsFinHedDet.Tables[0].Rows[0][0]).Trim();
                string secondreciptqurey = "select * from FM_HeaderFinCodeSettings where HeaderSettingPK =" + Convert.ToString(dsFinHedDet.Tables[0].Rows[0][0]) + " and FinyearFK=" + finYearid + " and CollegeCode='" + Session["collegecode"].ToString() + "' ";
                DataSet dsrecYr = new DataSet();
                dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
                if (dsrecYr.Tables.Count > 0 && dsrecYr.Tables[0].Rows.Count > 0)
                {
                    recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0]["RcptStNo"]);
                    if (recnoprev != "")
                    {
                        int recno_cur = Convert.ToInt32(recnoprev);
                        receno = recno_cur;
                    }
                    recacr = Convert.ToString(dsrecYr.Tables[0].Rows[0]["RcptAcr"]);
                    rcpracr = recacr;

                    int size = Convert.ToInt32(dsrecYr.Tables[0].Rows[0]["Rcptsize"]);

                    string recenoString = receno.ToString();

                    if (size != recenoString.Length && size > recenoString.Length)
                    {
                        while (size != recenoString.Length)
                        {
                            recenoString = "0" + recenoString;
                        }
                    }
                    recno = recacr + recenoString;
                }
            }
            return recno;
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); 
            return recno;
        }
    }
    protected void ddlreporttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "Office of the Controller of Examinations $Exam Fees Paid Status Report  For Examination - " + ddlMonth.SelectedItem.ToString() + " " + ddlYear.Text.ToString() + "@ Degree : " + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString();
        if (ddlreporttype.SelectedItem.ToString() == "UnPaid")
        {
            degreedetails = "Office of the Controller of Examinations $Exam Fees UnPaid Student Report For Examination - " + ddlMonth.SelectedItem.ToString() + " " + ddlYear.Text.ToString() + "@ Degree : " + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString();
        }
        string pagename = "Exam Fee Status.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    //Code Added by Mohamed Idhris  -- 21-09-2016
    private ArrayList MandatoryFees(out ArrayList arrHeaderFk, out ArrayList arrLedgerFk)
    {
        DataTable dtMandFee = new DataTable();
        dtMandFee.Columns.Add("Application Form");
        dtMandFee.Columns.Add("Semester Mark Sheet");
        dtMandFee.Columns.Add("Theory");
        dtMandFee.Columns.Add("Practical");
        dtMandFee.Columns.Add("Project");
        dtMandFee.Columns.Add("Field Work");
        dtMandFee.Columns.Add("Viva Voice");
        dtMandFee.Columns.Add("Disseration");
        dtMandFee.Columns.Add("Consolidate Mark Sheet");
        dtMandFee.Columns.Add("Course Completaion");
        dtMandFee.Columns.Add("Online Application Fee");
        dtMandFee.Columns.Add("Arrear Theory");
        dtMandFee.Columns.Add("Arrear Practical");
        dtMandFee.Columns.Add("Central Valuation");

        dtMandFee.Rows.Add("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
        ArrayList arrMandFees = new ArrayList();
        arrHeaderFk = new ArrayList();
        arrLedgerFk = new ArrayList();

        for (int dCol = 0; dCol < dtMandFee.Columns.Count; dCol++)
        {
            string linkVal = Convert.ToString(dtMandFee.Columns[dCol].ColumnName.Trim()) + "@#MandatoryFee";
            byte prevVal = Convert.ToByte(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'").Trim());
            if (prevVal == 1)
            {
                dtMandFee.Rows[0][dCol] = prevVal;
            }
        }

        for (int dRow = 0; dRow < dtMandFee.Columns.Count; dRow++)
        {

            string colName = Convert.ToString(dtMandFee.Columns[dRow].ColumnName).Trim();
            if (Convert.ToString(dtMandFee.Rows[0][colName]) == "1")
            {
                arrMandFees.Add(colName);

                DataSet settingValue = d2.select_method_wo_parameter(" select settings,value from Master_Settings where settings='" + colName + "' --and usercode='" + Session["usercode"].ToString() + "'", "Text");
                string feeValue = string.Empty;
                string headerCode = string.Empty;
                string ledgerCode = string.Empty;

                if (settingValue.Tables.Count > 0 && settingValue.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        feeValue = Convert.ToString(settingValue.Tables[0].Rows[0][0]);
                        headerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[0];
                        ledgerCode = Convert.ToString(settingValue.Tables[0].Rows[0][1]).Split(';')[1];
                        if (!arrHeaderFk.Contains(headerCode))
                        {
                            arrHeaderFk.Add(headerCode);
                        }
                        if (!arrLedgerFk.Contains(ledgerCode))
                        {
                            arrLedgerFk.Add(ledgerCode);
                        }
                    }
                    catch { }
                }
            }
        }
        return arrMandFees;
    }
    protected void btnChallanConf_Click(object sender, EventArgs e)
    {
        List<string> appNoList = new List<string>();
        checkedOKSpread(out appNoList);
        if (appNoList.Count > 0)
        {
            challanConfirm(appNoList);
        }
        else
        {
            imgAlert.Visible = true;

            lbl_alert.Text = "Please Select Any Student";
        }
    }
    private void challanConfirm(List<string> appNoList)
    {
        #region Mandatory Fees Values
        ArrayList arrHeaderFk = new ArrayList();
        ArrayList arrLedgerFk = new ArrayList();
        ArrayList arrMandFees = MandatoryFees(out arrHeaderFk, out arrLedgerFk);
        StringBuilder headerCodes = new StringBuilder();
        StringBuilder ledgerCodes = new StringBuilder();

        foreach (string hdr in arrHeaderFk)
        {
            headerCodes.Append(hdr + "','");
        }
        if (headerCodes.Length > 2)
        {
            headerCodes.Remove(headerCodes.Length - 3, 3);
        }

        foreach (string lgr in arrLedgerFk)
        {
            ledgerCodes.Append(lgr + "','");
        }
        if (ledgerCodes.Length > 2)
        {
            ledgerCodes.Remove(ledgerCodes.Length - 3, 3);
        }
        #endregion
        int count = 0;
        try
        {
            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string accountid = d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            for (int row = 0; row < appNoList.Count; row++)
            {
                string appNo = appNoList[row];
                string selectQuery = "SELECT ChallanNo,convert(varchar(10), ChallanDate,103) as ChallanDate,app_formno,'' smart_serial_no,''Reg_No,''Roll_Admit,''Roll_No,Stud_Name,Course_Name+'-'+Dept_Name Degree,Course_Name+'-'+dept_acronym DegreeAcr,SUM(TakenAmt) as TakenAmt,ChallanDate  as cldate,A.App_No FROM FT_ChallanDet C,applyn A,Degree G,Course U,Department D WHERE C.App_No = A.app_no AND A.degree_code = G.Degree_Code AND G.Course_Id = u.Course_Id and g.college_code = u.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and a.college_code='" + collegecode1 + "' and a.app_no='" + appNo + "'  and  C.headerfk in('" + headerCodes.ToString() + "') and c.ledgerfk in('" + ledgerCodes.ToString() + "')  GROUP BY ChallanNo, ChallanDate, app_formno, Stud_Name, Course_Name, Dept_Name,ChallanDate,a.App_No,dept_acronym order by  cldate  ";
                DataSet dsChallDet = d2.select_method_wo_parameter(selectQuery, "Text");

                if (dsChallDet.Tables.Count > 0 && dsChallDet.Tables[0].Rows.Count > 0)
                {
                    string acronym = string.Empty;
                    string hdrSetPK = string.Empty;
                    bool confvalue = false;

                    for (int i = 0; i < dsChallDet.Tables[0].Rows.Count; i++)
                    {
                        string chlnNo = Convert.ToString(dsChallDet.Tables[0].Rows[i]["ChallanNo"]);
                        string chlnDt = Convert.ToString(dsChallDet.Tables[0].Rows[i]["ChallanDate"]);
                        string AppFormNo = Convert.ToString(dsChallDet.Tables[0].Rows[i]["app_formno"]);
                        string studname = Convert.ToString(dsChallDet.Tables[0].Rows[i]["Stud_Name"]);
                        string dept = Convert.ToString(dsChallDet.Tables[0].Rows[i]["Degree"]);
                        string total = Convert.ToString(dsChallDet.Tables[0].Rows[i]["TakenAmt"]);
                        string AppNo = Convert.ToString(dsChallDet.Tables[0].Rows[i]["App_No"]);

                        string trasdate = DateTime.Now.ToString("MM/dd/yyyy");
                        chlnDt = chlnDt.Split('/')[1] + "/" + chlnDt.Split('/')[0] + "/" + chlnDt.Split('/')[2];
                        string transtime = DateTime.Now.ToLongTimeString();


                        DataSet sdn = d2.select_method_wo_parameter(" select ChallanNo from FT_ChallanDet WHERE ChallanNo = '" + chlnNo.Trim() + "' AND App_No = " + AppNo + " and isnull( IsConfirmed,'0') = '0'", "Text");

                        if (sdn.Tables.Count > 0 && sdn.Tables[0].Rows.Count > 0)
                        {

                            string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,bankFK,TakenAmt,FInyearFk from FT_ChallanDet where challanNo='" + chlnNo + "' AND App_No = " + AppNo + " and isnull( IsConfirmed,'0') = '0' select distinct HeaderFk from FT_ChallanDet where challanNo='" + chlnNo + "' AND App_No = " + AppNo + " and isnull( IsConfirmed,'0') = '0'";
                            DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                            bool challanOk = true;
                            if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
                                {
                                    string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
                                    string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
                                    string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
                                    string finFk = Convert.ToString(dsDet.Tables[0].Rows[j]["FInyearFk"]);
                                    string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);
                                    double amount = 0;
                                    double.TryParse(taknAmt, out amount);

                                    double balamount = 0;
                                    string balAmtStr = d2.GetFunction("select ISNULL(totalamount,0)-ISNULL(paidamount,0) as balamount from FT_FeeAllot where LedgerFK=" + ledger + " and HeaderFK=" + header + " and FeeCategory=" + FeeCategory + "  and App_No=" + AppNo + ""); //and FinYearFK=" + finFk + "
                                    double.TryParse(balAmtStr, out balamount);
                                    if (balamount < amount)
                                    {
                                        challanOk = false;
                                    }
                                }
                            }
                            if (challanOk)
                            {
                                if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                                {
                                    string hdrs = string.Empty;
                                    for (int hdr = 0; hdr < dsDet.Tables[1].Rows.Count; hdr++)
                                    {
                                        if (hdrs == string.Empty)
                                        {
                                            hdrs = Convert.ToString(dsDet.Tables[1].Rows[hdr][0]);
                                        }
                                        else
                                        {
                                            hdrs += "," + Convert.ToString(dsDet.Tables[1].Rows[hdr][0]);
                                        }
                                    }
                                    int save1 = 0;
                                    try
                                    {
                                        string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));

                                    }
                                    catch { save1 = 0; }

                                    string transcode = generateReceiptNo(out acronym, out hdrSetPK, hdrs);
                                    if (save1 == 5 || (transcode != "" && (hdrSetPK != "" || (isHeaderwise == 0 || isHeaderwise == 2))))
                                    {
                                        int insOk = 0;

                                        for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
                                        {
                                            string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
                                            string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
                                            string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
                                            string bankPk = Convert.ToString(dsDet.Tables[0].Rows[j]["bankFk"]);
                                            string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);

                                            string bankDet = "SELECT DISTINCT BankCode,City FROM FM_FinBankMaster  where CollegeCode=" + collegecode1 + " and BankPk=" + bankPk + "";
                                            DataSet dsBnk = d2.select_method_wo_parameter(bankDet, "Text");

                                            if (dsBnk.Tables.Count > 0)
                                            {
                                                if (dsBnk.Tables[0].Rows.Count > 0)
                                                {
                                                    string iscollected = "0";
                                                    string collecteddate = "";

                                                    iscollected = "1";
                                                    collecteddate = (Convert.ToDateTime(trasdate).ToString("MM/dd/yyyy")).ToString();
                                                    string bnkCode = Convert.ToString(dsBnk.Tables[0].Rows[0]["BankCode"]);
                                                    string bnkCity = Convert.ToString(dsBnk.Tables[0].Rows[0]["City"]);

                                                    string insQuery = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,IsCollected,CollectedDate) VALUES('" + Convert.ToDateTime(trasdate).ToString("MM/dd/yyyy") + "','" + transtime + "','" + transcode + "', 1, " + AppNo + ", " + ledger + ", " + header + ", " + FeeCategory + ", 0, " + taknAmt + ", 4, '" + chlnNo + "', '" + Convert.ToDateTime(chlnDt).ToString("MM/dd/yyyy") + "', " + bankPk + ",'" + bnkCity + "', 1, '0', 0, '', '0', '0', '0', 0, " + usercode + ", " + finYearid + ",'" + iscollected + "','" + collecteddate + "')";

                                                    insOk = d2.update_method_wo_parameter(insQuery, "Text");

                                                    string updateFee = "UPDATE FT_FeeAllot SET PaidAmount = isnull(PaidAmount,0) + " + taknAmt + ",BalAmount = BalAmount-  " + taknAmt + ",ChlTaken = ChlTaken-  " + taknAmt + " WHERE App_No = " + AppNo + " AND FeeCategory = " + FeeCategory + " AND LedgerFK = " + ledger + " and HeaderFk=" + header + "";
                                                    d2.update_method_wo_parameter(updateFee, "Text");

                                                }
                                            }
                                        }

                                        //imgAlert.Visible = true;
                                        if (insOk > 0)
                                        {
                                            #region Update  Challan
                                            string updateChln = "UPDATE FT_ChallanDet SET RcptTransCode= '" + transcode + "',RcptTransDate= '" + trasdate + "',IsConfirmed = '1' WHERE ChallanNo = '" + chlnNo + "' AND App_No = " + AppNo + "";
                                            d2.update_method_wo_parameter(updateChln, "Text");

                                            #endregion

                                            #region Update Paid Status
                                            string Roll_no = d2.GetFunction("Select Roll_no from Registration where app_no='" + AppNo + "'");
                                            string Exam_code_New = d2.GetFunction("select Exam_code  from Exam_details where degree_code ='" + ddlbranch.SelectedValue + "' and Exam_Month='" + ddlMonth.SelectedValue + "' and Exam_year='" + ddlYear.SelectedItem.Text + "' and batch_year='" + ddlbatch.SelectedItem.Text + "' and college_code ='" + ddlcollege.SelectedValue + "'");
                                            string strinsupdatequery = "update Exam_application set is_confirm='1' where roll_no='" + Roll_no + "' and exam_code='" + Exam_code_New + "'";
                                            //d2.update_method_wo_parameter(strinsupdatequery, "Text");
                                            #endregion

                                            #region Update Receipt No
                                            transcode = transcode.Remove(0, acronym.Length);

                                            if (save1 != 5)
                                            {
                                                string updateRecpt = string.Empty;
                                                if (isHeaderwise == 0 || isHeaderwise == 2)
                                                {
                                                    updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + transcode + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + ")";
                                                }
                                                else
                                                {
                                                    updateRecpt = "update FM_HeaderFinCodeSettings set RcptStNo=" + transcode + "+1 where HeaderSettingPK=" + hdrSetPK + " and FinyearFK=" + finYearid + " and CollegeCode=" + collegecode1 + "";
                                                }
                                                d2.update_method_wo_parameter(updateRecpt, "Text");
                                            }
                                            #endregion

                                            count++;
                                            confvalue = true;
                                            //alertmsg = "Confirmed Sucessfully";
                                            //FpSpread1.Rows[i].BackColor = Color.LightGreen;
                                            //FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                                        }
                                        //else
                                        //{
                                        //    alertmsg = "Not Saved";
                                        //}
                                    }
                                    //else
                                    //{
                                    //    alertmsg = "Please Select Particualar Header";// "Receipt No Not Assigned For Selected Headers";
                                    //}

                                }
                                //else
                                //{
                                //    imgAlert.Visible = true;
                                //    alertmsg = "Not Saved";
                                //}
                            }
                            //else
                            //{
                            //imgAlert.Visible = true;
                            //alertmsg = "Challan Cannot Be Confirmed. Balance Not Available";
                            //}
                        }
                        //else
                        //{
                        //imgAlert.Visible = true;
                        //alertmsg = "Challan Already Confirmed";
                        //}
                    }
                }
            }
        }
        catch { }
        imgAlert.Visible = true;
        lbl_alert.Text = "Confirmed : " + count + " Not Confirmed : " + (appNoList.Count - count);
    }
    protected void btnChallanDel_Click(object sender, EventArgs e)
    {
        surediv.Visible = true;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        List<string> appNoList = new List<string>();
        checkedOKSpread(out appNoList);
        if (appNoList.Count > 0)
        {
            challanDelete(appNoList);
        }
        else
        {
            imgAlert.Visible = true;

            lbl_alert.Text = "Please Select Any Student";
        }
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
    }
    private void challanDelete(List<string> appNoList)
    {
        #region Mandatory Fees Values
        ArrayList arrHeaderFk = new ArrayList();
        ArrayList arrLedgerFk = new ArrayList();
        ArrayList arrMandFees = MandatoryFees(out arrHeaderFk, out arrLedgerFk);
        StringBuilder headerCodes = new StringBuilder();
        StringBuilder ledgerCodes = new StringBuilder();

        foreach (string hdr in arrHeaderFk)
        {
            headerCodes.Append(hdr + "','");
        }
        if (headerCodes.Length > 2)
        {
            headerCodes.Remove(headerCodes.Length - 3, 3);
        }

        foreach (string lgr in arrLedgerFk)
        {
            ledgerCodes.Append(lgr + "','");
        }
        if (ledgerCodes.Length > 2)
        {
            ledgerCodes.Remove(ledgerCodes.Length - 3, 3);
        }
        #endregion
        int count = 0;
        int checkCount = 0;
        try
        {
            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string accountid = d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            for (int row = 0; row < appNoList.Count; row++)
            {
                string appNo = appNoList[row];
                string selectQuery = "SELECT ChallanNo,convert(varchar(10), ChallanDate,103) as ChallanDate,app_formno,'' smart_serial_no,''Reg_No,''Roll_Admit,''Roll_No,Stud_Name,Course_Name+'-'+Dept_Name Degree,Course_Name+'-'+dept_acronym DegreeAcr,SUM(TakenAmt) as TakenAmt,ChallanDate  as cldate,A.App_No FROM FT_ChallanDet C,applyn A,Degree G,Course U,Department D WHERE C.App_No = A.app_no AND A.degree_code = G.Degree_Code AND G.Course_Id = u.Course_Id and g.college_code = u.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and a.college_code='" + collegecode1 + "' and a.app_no='" + appNo + "'  and  C.headerfk in('" + headerCodes.ToString() + "') and c.ledgerfk in('" + ledgerCodes.ToString() + "')  GROUP BY ChallanNo, ChallanDate, app_formno, Stud_Name, Course_Name, Dept_Name,ChallanDate,a.App_No,dept_acronym order by  cldate  ";
                DataSet dsChallDet = d2.select_method_wo_parameter(selectQuery, "Text");

                string RollNo = d2.GetFunction("select roll_no from registration where app_no='" + appNo + "'");
                string examyear = ddlYear.SelectedValue.ToString();
                string exammonth = ddlMonth.SelectedValue.ToString();
                string batchyear = ddlbatch.SelectedValue.ToString();
                string degreecode = ddlbranch.SelectedValue.ToString();
                if (chkExamApp.Checked)
                {
                    string deletequery = "delete ead from Exam_Details ed,exam_application ea,exam_appl_details ead where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no='" + RollNo + "' and ed.exam_year='" + examyear + "' and ed.exam_month='" + exammonth + "'";
                    int delva = d2.update_method_wo_parameter(deletequery, "text");
                    deletequery = "delete ea from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code  and ea.roll_no='" + RollNo + "' and ed.exam_year='" + examyear + "' and ed.exam_month='" + exammonth + "'";
                    delva = d2.update_method_wo_parameter(deletequery, "text");
                }

               
                if (dsChallDet.Tables.Count > 0 && dsChallDet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsChallDet.Tables[0].Rows.Count; i++)
                    {
                        count = 0;
                       
                 
                        string chlnNo = Convert.ToString(dsChallDet.Tables[0].Rows[i]["ChallanNo"]);//
                        string chlnDt = Convert.ToString(dsChallDet.Tables[0].Rows[i]["ChallanDate"]);
                        string AppFormNo = Convert.ToString(dsChallDet.Tables[0].Rows[i]["app_formno"]);
                        string studname = Convert.ToString(dsChallDet.Tables[0].Rows[i]["Stud_Name"]);
                        string dept = Convert.ToString(dsChallDet.Tables[0].Rows[i]["Degree"]);
                        string total = Convert.ToString(dsChallDet.Tables[0].Rows[i]["TakenAmt"]);
                        string AppNo = Convert.ToString(dsChallDet.Tables[0].Rows[i]["App_No"]);

                        string trasdate = DateTime.Now.ToString("MM/dd/yyyy");
                        chlnDt = chlnDt.Split('/')[1] + "/" + chlnDt.Split('/')[0] + "/" + chlnDt.Split('/')[2];
                        string transtime = DateTime.Now.ToLongTimeString();

                      
                        string confirmChk = d2.GetFunction(" select ChallanNo from FT_ChallanDet WHERE ChallanNo = '" + chlnNo.Trim() + "' AND App_No = " + AppNo + " and isnull(IsConfirmed,0) = '0'");
                        if (confirmChk != null && confirmChk != "")
                        {
                            string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,isnull(TakenAmt,0) as TakenAmt  from FT_ChallanDet where challanNo='" + chlnNo + "'  AND App_No = " + AppNo + " and isnull(IsConfirmed,0) = '0'";
                            DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                            if (dsDet.Tables.Count > 0)
                            {
                                if (dsDet.Tables[0].Rows.Count > 0)
                                {
                                    for (int n = 0; n < dsDet.Tables[0].Rows.Count; n++)
                                    {

                                        string ledger = Convert.ToString(dsDet.Tables[0].Rows[n]["LedgerFK"]);
                                        string header = Convert.ToString(dsDet.Tables[0].Rows[n]["HeaderFk"]);
                                        string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[n]["FeeCategory"]);
                                        string creditamt = Convert.ToString(dsDet.Tables[0].Rows[n]["TakenAmt"]);

                                        string delQuery = "delete from FT_ChallanDet WHERE ChallanNo = '" + chlnNo + "' AND App_No = " + AppNo + " AND FeeCategory = " + FeeCategory + " and HeaderFk=" + header + " and LedgerFk=" + ledger + " and (IsConfirmed = '0' or IsConfirmed is Null)";

                                        string updateCHlTkn = " update FT_FeeAllot set ChlTaken =ISNULL( ChlTaken,0)-" + creditamt + "  where FeeCategory ='" + FeeCategory + "' and HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "' and App_No='" + AppNo + "' ";
                                        d2.update_method_wo_parameter(updateCHlTkn, "Text");

                                        int delOK = d2.update_method_wo_parameter(delQuery, "Text");

                                        imgAlert.Visible = true;

                                        if (delOK > 0)
                                        {
                                            count++;
                                            //delValue = true;
                                            //alertmsg = "Deleted Sucessfully";
                                        }
                                        else
                                        {
                                            //alertmsg = "Please Cancel The Challan To Delete";
                                        }
                                    }
                                }
                                //else
                                //{
                                //    imgAlert.Visible = true;
                                //    alertmsg = "Not Deleted";
                                //}
                            }
                            //else
                            //{
                            //    imgAlert.Visible = true;
                            //    alertmsg = "Not Deleted";
                            //}
                        }
                        if (count > 0)
                        {
                            checkCount++;
                        }
                    }
                }
            }
        }
        catch { }
        imgAlert.Visible = true;
        lbl_alert.Text = "Deleted : " + checkCount + " Not Deleted : " + (appNoList.Count - checkCount);
    }
    protected void btnChallan_Click(object sender, EventArgs e)
    {
        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 0)
            {
                imgAlert.Visible = true;

                lbl_alert.Text = "Please Add Challan Format Setting";
            }
            else if (save1 == 1)
            {
                //For Mcc and Others
                List<string> appNoList = new List<string>();
                checkedOKSpread(out appNoList);
                if (appNoList.Count > 0)
                {
                    challanPrintMCC(appNoList);
                }
                else
                {
                    imgAlert.Visible = true;

                    lbl_alert.Text = "Please Select Any Student";
                }
            }
            else if (save1 == 2)
            {
                //For NEC
                // challanPrintNew();
            }
            else if (save1 == 3)
            {
                //For UIT
                //  challanPrintUIT();
            }
            else if (save1 == 4)
            {
                //For New College
                //challanPrintNewCollege();
            }
        }
        catch (Exception ex) { }
    }
    private void challanPrintMCC(List<string> appNoList)
    {
        //Mcc and Others
        try
        {
            #region Mandatory Fees Values
            ArrayList arrHeaderFk = new ArrayList();
            ArrayList arrLedgerFk = new ArrayList();
            ArrayList arrMandFees = MandatoryFees(out arrHeaderFk, out arrLedgerFk);
            StringBuilder headerCodes = new StringBuilder();
            StringBuilder ledgerCodes = new StringBuilder();

            foreach (string hdr in arrHeaderFk)
            {
                headerCodes.Append(hdr + "','");
            }
            if (headerCodes.Length > 2)
            {
                headerCodes.Remove(headerCodes.Length - 3, 3);
            }

            foreach (string lgr in arrLedgerFk)
            {
                ledgerCodes.Append(lgr + "','");
            }
            if (ledgerCodes.Length > 2)
            {
                ledgerCodes.Remove(ledgerCodes.Length - 3, 3);
            }
            #endregion

            int challanType = 1;

            string roll_admit = string.Empty;
            string curChlnNo = generateChallanNo();
            string lastRecptNo = Convert.ToString(Session["lastCHlNO"]);
            string accidRecpt = Convert.ToString(Session["lastAccId"]);
            Font FontBarcode = new Font("IDAutomationHC39M", 10, FontStyle.Regular);
            string finYeaid = d2.getCurrentFinanceYear(usercode, collegecode1);

            int count = 0;
            bool createPDFOK = false;
            Font Fontbold = new Font("Arial", 8, FontStyle.Bold);
            Font Fontsmall = new Font("Arial", 8, FontStyle.Bold);
            Font Fontsmall1 = new Font("Arial", 10, FontStyle.Bold);
            Font Fontbold1 = new Font("Arial", 10, FontStyle.Bold);
            Gios.Pdf.PdfDocument mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(13.8, 8.5));
            Gios.Pdf.PdfPage myprov_pdfpage = null;

            #region Bank Details
            string bankName = "";
            string bankPK = "";
            string bankCity = "";
            string bankAddress = "";

            bankPK = getCollegeBankPK();
            bankName = d2.GetFunction("select bankname from FM_FinBankMaster where BankPK='" + bankPK + "'").Trim();
            bankAddress = d2.GetFunction("select Street+', '+(select MasterValue from CO_MasterValues where MasterCode=District)+'-'+PinCode as address from FM_FinBankMaster where BankPK=" + bankPK + "");
            bankAddress = "(" + bankAddress + ")";
            bankCity = d2.GetFunction("select Upper(BankBranch) as city from FM_FinBankMaster where BankPK=" + bankPK + "") + "";
            #endregion

            for (int row = 0; row < appNoList.Count; row++)
            {
                count++;
                #region Inside Students For loop
                try
                {
                    #region Basic Data
                    string recptNo = curChlnNo;
                    string recptDt = DateTime.Now.ToString("MM/dd/yyyy");
                    string studname = string.Empty;
                    // string course = txt_dept.Text.Trim();
                    string batchYrSem = string.Empty;
                    string Regno = string.Empty;
                    string rollno = string.Empty;
                    string appnoNew = appNoList[row];
                    string regno = string.Empty;
                    string degreeCode = string.Empty;
                    string stream = string.Empty;
                    string feeCategory = string.Empty;
                    string app_formno = string.Empty;
                    string smartno = string.Empty;
                    string curSem = string.Empty;
                    string queryRollApp = "select r.Roll_No,a.app_formno,r.smart_serial_no,a.app_no,r.Reg_No,r.stud_name,r.current_semester,r.roll_admit  from Registration r,applyn a where r.App_No=a.app_no  and r.college_code='" + collegecode1 + "'  and r.app_no='" + appnoNew + "'";
                    DataSet dsRollApp = new DataSet();
                    dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                    if (dsRollApp.Tables.Count > 0 && dsRollApp.Tables[0].Rows.Count > 0)
                    {
                        rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                        app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                        appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                        Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                        smartno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["smart_serial_no"]);
                        roll_admit = Convert.ToString(dsRollApp.Tables[0].Rows[0]["roll_admit"]);
                        studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["stud_name"]);
                        curSem = Convert.ToString(dsRollApp.Tables[0].Rows[0]["current_semester"]);
                        feeCategory = getFeecategoryNEW(curSem).Value;
                    }

                    string rolldisplay = "Admission No :";
                    string rollvalue = roll_admit;
                    //if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                    //{
                    //    rolldisplay = "Roll No :";
                    //    rollvalue = rollno;
                    //}
                    //else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                    //{
                    rolldisplay = "Reg No :";
                    rollvalue = Regno;
                    //}
                    //else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                    //{
                    //    rolldisplay = "Admission No :";
                    //    rollvalue = roll_admit;
                    //}
                    //else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                    //{
                    //    rolldisplay = "Smartcard No :";
                    //    rollvalue = smartno;
                    //}
                    //else
                    //{
                    //    appnoNew = getAppNoFromApplyn(roll_admit);
                    //    rolldisplay = "App No :";
                    //    rollvalue = app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + appnoNew + "'").Trim();
                    //}

                    string colquery = "";
                    if (rolldisplay != "App No :")
                    {
                        colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                    }
                    else
                    {
                        colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                    }
                    string collegename = "";
                    string add1 = "";
                    string add2 = "";
                    string univ = "";
                    string deg = "";
                    string cursem = "";
                    string batyr = "";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(colquery, "Text");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                            add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                            add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                            univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            //if (degACR == 0)
                            //{
                            deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                            //}
                            //else
                            //{
                            // deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                            //}
                            degreeCode = Convert.ToString(ds.Tables[1].Rows[0]["Degree_code"]);
                            cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                            batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                            stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);

                            cursem = "Semester : " + romanLetter(Convert.ToString(cursem));
                        }
                    }
                    #endregion
                    #region PDF Generation
                    // New Code
                    string groupHdr;
                    string[] hdrInGrp0;
                    List<string> hdrInGrp = new List<string>();

                    bool checkedHeaderOK = false;

                    #region For Overall
                    string QHdrForGroup = "	SELECT ChlGroupHeader FROM FM_ChlBankPrintSettings WHERE DegreeCode = '" + degreeCode + "' AND SettingType = 1 and CollegeCode=" + collegecode1 + " ";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(QHdrForGroup, "Text");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string bnkAcc = "";
                                checkedHeaderOK = false;
                                groupHdr = Convert.ToString(ds.Tables[0].Rows[i][0]);
                                hdrInGrp0 = groupHdr.Split(',');
                                hdrInGrp.Clear();
                                foreach (string item in hdrInGrp0)
                                {
                                    hdrInGrp.Add(item);
                                    checkedHeaderOK = true;
                                }

                                if (!checkedHeaderOK)
                                {
                                    continue;
                                }

                                //Add new challan Page in this loop
                                bool addpageOK = false;
                                #region TOp portion

                                int y = 0;

                                myprov_pdfpage = mychallan.NewPage();

                                PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                PdfTextArea ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 90, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + DateTime.Now.ToString("dd/MM/yyyy"));
                                PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                //PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                //                                                 new PdfArea(mychallan, 250, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                double ovrallcredit = 0;
                                double grandtotal = 0.00;


                                myprov_pdfpage.Add(FC17);
                                string text = "";

                                //First Ends

                                PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                PdfTextArea UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                //PdfTextArea UC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                //                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + DateTime.Now.ToString("dd/MM/yyyy"));

                                PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                //second End
                                y = 0;


                                PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                PdfTextArea TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                //PdfTextArea TC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                //                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 670, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + DateTime.Now.ToString("dd/MM/yyyy"));
                                PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                            new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                myprov_pdfpage.Add(FC10);
                                myprov_pdfpage.Add(UC10);
                                myprov_pdfpage.Add(TC10);
                                y = 0;

                                #endregion

                                //End of  New CHallan Top Portion

                                //Middle portion of the challan
                                #region Middle Portion challan
                                int chk = 0;
                                for (int indx = 0; indx < hdrInGrp.Count; indx++)
                                {
                                    //string QhdrId = "SELECT HeaderFK  FROM FS_ChlGroupHeaderSettings where ChlGroupHeader in ('" + hdrInGrp[indx] + "') and Stream='" + stream + "'";
                                    string QhdrId = "SELECT HeaderFK  FROM FS_ChlGroupHeaderSettings where ChlGroupHeader in ('" + hdrInGrp[indx] + "') and Stream='" + stream + "' and HeaderFk in ('" + headerCodes.ToString() + "') ";
                                    string HdrId = "";
                                    string dispHdr = "";

                                    DataSet ds1 = new DataSet();
                                    ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                    if (ds1.Tables.Count > 0)
                                    {
                                        if (ds1.Tables[0].Rows.Count > 0)
                                        {

                                            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                            {
                                                if (HdrId == "")
                                                {
                                                    HdrId = Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);

                                                }
                                                else
                                                {
                                                    HdrId += "," + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);
                                                }
                                            }

                                            string QFinTot = "	SELECT App_FormNo,Stud_Name,Course_Name+'-'+Dept_Name Degree,TextVal,DispStream,ChlGroupHeader,SUM(TotalAmount) as Totamount	FROM FT_FeeAllot A,applyn P,Degree G,Course U,Department D,FS_ChlGroupHeaderSettings S,TextValTable T	WHERE A.app_no = P.app_no AND P.degree_code = G.Degree_Code AND G.Course_Id = U.Course_Id AND G.college_code = U.college_code	AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code AND A.HeaderFK = S.HeaderFK AND A.FeeCategory = T.TextCode AND Stream = '" + stream + "' AND A.HeaderFK IN (" + HdrId + ")  and a.ledgerfk in ('" + ledgerCodes.ToString() + "')   AND A.FeeCategory in(" + feeCategory + ") and P.app_no=" + appnoNew + "	GROUP BY App_FormNo,Stud_Name,Course_Name,Dept_Name,TextVal,DispStream,ChlGroupHeader           	    SELECT isnull(SUM(TakenAmt),0) as TakenAmt	FROM FT_ChallanDet C,FS_ChlGroupHeaderSettings S,TextValTable T 	WHERE C.HeaderFK = S.HeaderFK AND C.FeeCategory = T.TextCode	AND Stream = '" + stream + "' AND C.HeaderFK IN (" + HdrId + ")  and C.ledgerfk in ('" + ledgerCodes.ToString() + "')  AND C.FeeCategory in (" + feeCategory + ") and C.app_no=" + appnoNew + "  GROUP BY ChlGroupHeader ";
                                            DataSet ds2 = new DataSet();
                                            ds2 = d2.select_method_wo_parameter(QFinTot, "Text");
                                            if (ds2.Tables.Count > 0)
                                            {
                                                if (ds2.Tables[0].Rows.Count > 0)
                                                {
                                                    dispHdr = Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]);
                                                    string hdrNme = Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]).Trim().ToUpper();
                                                    double totalAmt = Convert.ToDouble(ds2.Tables[0].Rows[0]["Totamount"]);
                                                    if (ds2.Tables[1].Rows.Count > 0)
                                                    {
                                                        totalAmt -= Convert.ToDouble(ds2.Tables[1].Rows[0]["TakenAmt"]);
                                                    }
                                                    // bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                                    bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                    dispHdr += " (" + bnkAcc + ")";
                                                    grandtotal = grandtotal + totalAmt;

                                                    if (grandtotal > 0 || hdrNme == "TUITION FEE")
                                                    {
                                                        addpageOK = true;
                                                        createPDFOK = true;
                                                        if (totalAmt > 0 || hdrNme == "TUITION FEE")
                                                        {
                                                            if (chk == 0)
                                                            {
                                                                // chk++;
                                                                #region Update Challan No
                                                                recptNo = generateChallanNo();

                                                                lastRecptNo = Convert.ToString(Session["lastCHlNO"]);
                                                                accidRecpt = Convert.ToString(Session["lastAccId"]);
                                                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                                {
                                                                    string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]) + "  and ledgerfk in ('" + ledgerCodes.ToString() + "')   and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";

                                                                    DataSet dsEachHdr = new DataSet();
                                                                    dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                    if (dsEachHdr.Tables.Count > 0)
                                                                    {
                                                                        if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot f,FM_LedgerMaster l WHERE l.Ledgerpk=f.ledgerfk  and l.headerfk=f.headerfk  and f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "  and f.ledgerfk in ('" + ledgerCodes.ToString() + "')  and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')  order by case when priority is null then 1 else 0 end, priority ";
                                                                            DataSet dsLedge = new DataSet();
                                                                            dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                            if (dsLedge.Tables.Count > 0)
                                                                            {
                                                                                if (dsLedge.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                                    {
                                                                                        double remainAmt = 0;
                                                                                        remainAmt = Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]));
                                                                                        if (remainAmt > 0 || hdrNme == "TUITION FEE")
                                                                                        {
                                                                                            string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "," + challanType + ")";
                                                                                            d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                                            string updateCHlTkn = " update FT_FeeAllot set ChlTaken = +" + remainAmt + "  where FeeCategory ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "' and HeaderFK ='" + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "' and LedgerFK ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "' and App_No='" + appnoNew + "' ";
                                                                                            d2.update_method_wo_parameter(updateCHlTkn, "Text");
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                    }

                                                    PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                    PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                    PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                    myprov_pdfpage.Add(FC18);
                                                    myprov_pdfpage.Add(FC171);
                                                    myprov_pdfpage.Add(FC19);


                                                    PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                    PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                    PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                    myprov_pdfpage.Add(UC18);
                                                    myprov_pdfpage.Add(UC19);
                                                    myprov_pdfpage.Add(UC171);

                                                    PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                    PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                    PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                    myprov_pdfpage.Add(TC18);
                                                    myprov_pdfpage.Add(TC19);
                                                    myprov_pdfpage.Add(TC171);
                                                    y = y + 15;

                                                }
                                            }

                                        }
                                    }

                                }
                                #endregion
                                //Middle portion of challan End

                                //Bottom portion of the challan
                                if (addpageOK)
                                {
                                    string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                    d2.update_method_wo_parameter(updateRecpt, "Text");
                                    PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                    PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                    PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                    PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                    PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                    PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);

                                    myprov_pdfpage.Add(FC4);
                                    myprov_pdfpage.Add(UC4);
                                    myprov_pdfpage.Add(TC4);
                                    //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                    //myprov_pdfpage.Add(FC08, 250, 125);
                                    //myprov_pdfpage.Add(FC08, 550, 125);
                                    //myprov_pdfpage.Add(FC08, 900, 125);
                                    #region Bottom Portion of Challan

                                    text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                    PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                    PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                    PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                    PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                    PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                    PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                    PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");
                                    PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                    PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                    myprov_pdfpage.Add(pr1);

                                    PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                    PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                    myprov_pdfpage.Add(pr2);

                                    PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                    PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                    myprov_pdfpage.Add(pr3);

                                    Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                    table.VisibleHeaders = false;
                                    table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table.Columns[0].SetWidth(100);
                                    table.Columns[1].SetWidth(60);
                                    table.Columns[2].SetWidth(60);

                                    table.Cell(0, 0).SetContent("Cheque/DD No");
                                    table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, 0).SetFont(Fontbold1);
                                    table.Cell(0, 1).SetContent("Date");
                                    table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, 1).SetFont(Fontbold1);
                                    table.Cell(0, 2).SetContent("Amount");
                                    table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, 2).SetFont(Fontbold1);
                                    table.Cell(1, 0).SetContent("\n");
                                    table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(1, 0).SetFont(Fontbold1);
                                    table.Cell(1, 1).SetContent("\n");
                                    table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(1, 1).SetFont(Fontbold1);
                                    table.Cell(1, 2).SetContent("\n");
                                    table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(1, 2).SetFont(Fontbold1);
                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                    myprov_pdfpage.Add(myprov_pdfpagetable);

                                    Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                    table1.VisibleHeaders = false;
                                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table1.Columns[0].SetWidth(100);
                                    table1.Columns[1].SetWidth(60);
                                    table1.Cell(0, 0).SetContent("2000x");
                                    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(0, 0).SetFont(Fontbold1);
                                    table1.Cell(1, 0).SetContent("500x");
                                    table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(1, 0).SetFont(Fontbold1);
                                    table1.Cell(2, 0).SetContent("100x");
                                    table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(2, 0).SetFont(Fontbold1);
                                    table1.Cell(3, 0).SetContent("50x");
                                    table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(3, 0).SetFont(Fontbold1);
                                    table1.Cell(4, 0).SetContent("20x");
                                    table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(4, 0).SetFont(Fontbold1);
                                    table1.Cell(5, 0).SetContent("10x");
                                    table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(5, 0).SetFont(Fontbold1);
                                    table1.Cell(6, 0).SetContent("5x");
                                    table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(6, 0).SetFont(Fontbold1);
                                    table1.Cell(7, 0).SetContent("Coinsx");
                                    table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(7, 0).SetFont(Fontbold1);
                                    table1.Cell(8, 0).SetContent("Total");
                                    table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(8, 0).SetFont(Fontbold1);



                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                    myprov_pdfpage.Add(myprov_pdfpagetable1);

                                    myprov_pdfpage.Add(FC);
                                    myprov_pdfpage.Add(ORGI);
                                    myprov_pdfpage.Add(IOB);
                                    //myprov_pdfpage.Add(FC4);
                                    myprov_pdfpage.Add(FC5);
                                    myprov_pdfpage.Add(FC6);
                                    myprov_pdfpage.Add(FC7);
                                    myprov_pdfpage.Add(FC8);
                                    myprov_pdfpage.Add(FC9);
                                    //myprov_pdfpage.Add(FC10);
                                    myprov_pdfpage.Add(FC11);
                                    myprov_pdfpage.Add(FC12);
                                    myprov_pdfpage.Add(FC13);
                                    myprov_pdfpage.Add(FC14);
                                    myprov_pdfpage.Add(FC15);
                                    myprov_pdfpage.Add(FC16);

                                    myprov_pdfpage.Add(FC24);
                                    myprov_pdfpage.Add(FC25);
                                    myprov_pdfpage.Add(FC26);
                                    myprov_pdfpage.Add(FC27);
                                    myprov_pdfpage.Add(FC28);
                                    myprov_pdfpage.Add(FC29);
                                    myprov_pdfpage.Add(FC30);
                                    myprov_pdfpage.Add(FC31);

                                    myprov_pdfpage.Add(FC32);
                                    //myprov_pdfpage.Add(FC33);

                                    //First End
                                    myprov_pdfpage.Add(UC17);

                                    PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                    PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                    PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                    PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                    PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                    PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                    PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                    Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                    table3.VisibleHeaders = false;
                                    table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table3.Columns[0].SetWidth(100);
                                    table3.Columns[1].SetWidth(60);
                                    table3.Columns[2].SetWidth(60);

                                    table3.Cell(0, 0).SetContent("Cheque/DD No");
                                    table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(0, 0).SetFont(Fontbold1);
                                    table3.Cell(0, 1).SetContent("Date");
                                    table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(0, 1).SetFont(Fontbold1);
                                    table3.Cell(0, 2).SetContent("Amount");
                                    table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(0, 2).SetFont(Fontbold1);
                                    table3.Cell(1, 0).SetContent("\n");
                                    table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(1, 0).SetFont(Fontbold1);
                                    table3.Cell(1, 1).SetContent("\n");
                                    table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(1, 1).SetFont(Fontbold1);
                                    table3.Cell(1, 2).SetContent("\n");
                                    table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(1, 2).SetFont(Fontbold1);
                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                    myprov_pdfpage.Add(myprov_pdfpagetable3);

                                    Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                    table14.VisibleHeaders = false;
                                    table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table14.Columns[0].SetWidth(100);
                                    table14.Columns[1].SetWidth(60);
                                    table14.Cell(0, 0).SetContent("2000x");
                                    table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table14.Cell(0, 0).SetFont(Fontbold1);
                                    table14.Cell(1, 0).SetContent("500x");
                                    table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table14.Cell(1, 0).SetFont(Fontbold1);
                                    table14.Cell(2, 0).SetContent("100x");
                                    table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table14.Cell(2, 0).SetFont(Fontbold1);
                                    table14.Cell(3, 0).SetContent("50x");
                                    table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table14.Cell(3, 0).SetFont(Fontbold1);
                                    table14.Cell(4, 0).SetContent("20x");
                                    table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table14.Cell(4, 0).SetFont(Fontbold1);
                                    table14.Cell(5, 0).SetContent("10x");
                                    table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table14.Cell(5, 0).SetFont(Fontbold1);
                                    table14.Cell(6, 0).SetContent("5x");
                                    table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table14.Cell(6, 0).SetFont(Fontbold1);
                                    table14.Cell(7, 0).SetContent("Coinsx");
                                    table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table14.Cell(7, 0).SetFont(Fontbold1);
                                    table14.Cell(8, 0).SetContent("Total");
                                    table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table14.Cell(8, 0).SetFont(Fontbold1);

                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                    myprov_pdfpage.Add(myprov_pdfpagetable4);

                                    myprov_pdfpage.Add(UC);
                                    myprov_pdfpage.Add(UC1);
                                    myprov_pdfpage.Add(UC2);
                                    //myprov_pdfpage.Add(UC4);
                                    myprov_pdfpage.Add(UC5);
                                    myprov_pdfpage.Add(UC6);
                                    myprov_pdfpage.Add(UC7);
                                    myprov_pdfpage.Add(UC8);
                                    myprov_pdfpage.Add(UC9);
                                    //myprov_pdfpage.Add(UC10);
                                    myprov_pdfpage.Add(UC11);
                                    myprov_pdfpage.Add(UC12);
                                    myprov_pdfpage.Add(UC13);
                                    myprov_pdfpage.Add(UC14);
                                    myprov_pdfpage.Add(UC15);
                                    myprov_pdfpage.Add(UC16);


                                    myprov_pdfpage.Add(UC24);
                                    myprov_pdfpage.Add(UC25);
                                    myprov_pdfpage.Add(UC26);
                                    myprov_pdfpage.Add(UC27);
                                    myprov_pdfpage.Add(UC28);
                                    myprov_pdfpage.Add(UC29);
                                    myprov_pdfpage.Add(UC30);
                                    myprov_pdfpage.Add(UC31);
                                    myprov_pdfpage.Add(UC32);
                                    //second End


                                    myprov_pdfpage.Add(TC17);

                                    PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                    PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                    PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                    PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                    PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                    PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                    PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                    Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                    table5.VisibleHeaders = false;
                                    table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table5.Columns[0].SetWidth(100);
                                    table5.Columns[1].SetWidth(60);
                                    table5.Columns[2].SetWidth(60);

                                    table5.Cell(0, 0).SetContent("Cheque/DD No");
                                    table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table5.Cell(0, 0).SetFont(Fontbold1);
                                    table5.Cell(0, 1).SetContent("Date");
                                    table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table5.Cell(0, 1).SetFont(Fontbold1);
                                    table5.Cell(0, 2).SetContent("Amount");
                                    table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table5.Cell(0, 2).SetFont(Fontbold1);
                                    table5.Cell(1, 0).SetContent("\n");
                                    table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table5.Cell(1, 0).SetFont(Fontbold1);
                                    table5.Cell(1, 1).SetContent("\n");
                                    table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table5.Cell(1, 1).SetFont(Fontbold1);
                                    table5.Cell(1, 2).SetContent("\n");
                                    table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table5.Cell(1, 2).SetFont(Fontbold1);
                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 250));
                                    myprov_pdfpage.Add(myprov_pdfpagetable31);

                                    Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                    table15.VisibleHeaders = false;
                                    table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table15.Columns[0].SetWidth(100);
                                    table15.Columns[1].SetWidth(60);
                                    table15.Cell(0, 0).SetContent("2000x");
                                    table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table15.Cell(0, 0).SetFont(Fontbold1);
                                    table15.Cell(1, 0).SetContent("500x");
                                    table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table15.Cell(1, 0).SetFont(Fontbold1);
                                    table15.Cell(2, 0).SetContent("100x");
                                    table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table15.Cell(2, 0).SetFont(Fontbold1);
                                    table15.Cell(3, 0).SetContent("50x");
                                    table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table15.Cell(3, 0).SetFont(Fontbold1);
                                    table15.Cell(4, 0).SetContent("20x");
                                    table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table15.Cell(4, 0).SetFont(Fontbold1);
                                    table15.Cell(5, 0).SetContent("10x");
                                    table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table15.Cell(5, 0).SetFont(Fontbold1);
                                    table15.Cell(6, 0).SetContent("5x");
                                    table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table15.Cell(6, 0).SetFont(Fontbold1);
                                    table15.Cell(7, 0).SetContent("Coinsx");
                                    table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table15.Cell(7, 0).SetFont(Fontbold1);
                                    table15.Cell(8, 0).SetContent("Total");
                                    table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table15.Cell(8, 0).SetFont(Fontbold1);

                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                    myprov_pdfpage.Add(myprov_pdfpagetable5);

                                    myprov_pdfpage.Add(TC);
                                    myprov_pdfpage.Add(TC1);
                                    myprov_pdfpage.Add(TC2);
                                    //myprov_pdfpage.Add(TC4);
                                    myprov_pdfpage.Add(TC5);
                                    myprov_pdfpage.Add(TC6);
                                    myprov_pdfpage.Add(TC7);
                                    myprov_pdfpage.Add(TC8);
                                    myprov_pdfpage.Add(TC9);
                                    //myprov_pdfpage.Add(TC10);
                                    myprov_pdfpage.Add(TC11);
                                    myprov_pdfpage.Add(TC12);
                                    myprov_pdfpage.Add(TC13);
                                    myprov_pdfpage.Add(TC14);
                                    myprov_pdfpage.Add(TC15);
                                    myprov_pdfpage.Add(TC16);
                                    myprov_pdfpage.Add(TC17);
                                    myprov_pdfpage.Add(TC24);
                                    myprov_pdfpage.Add(TC25);
                                    myprov_pdfpage.Add(TC26);
                                    myprov_pdfpage.Add(TC27);
                                    myprov_pdfpage.Add(TC28);
                                    myprov_pdfpage.Add(TC29);
                                    myprov_pdfpage.Add(TC30);
                                    myprov_pdfpage.Add(TC31);
                                    myprov_pdfpage.Add(TC32);

                                    myprov_pdfpage.SaveToDocument();
                                    #endregion
                                }
                                //Bottom portion of the challan End
                            }
                        }
                    }

                    //New COde END
                    #endregion

                    #endregion
                }
                catch (Exception ex) { }

                #endregion
            }

            #region Reponse Area
            if (createPDFOK && count > 0)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Challan" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                    mychallan.SaveToFile(szPath + szFile);
                    //Response.ClearHeaders();
                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    //Response.ContentType = "application/pdf";
                    //Response.WriteFile(szPath + szFile);
                    //Response.AddHeader("Refresh", "1; url=receiptPrint.aspx");
                    Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");

                    imgAlert.Visible = true;

                    lbl_alert.Text = "Challan Generated";
                }
            }
            else
            {
                imgAlert.Visible = true;

                lbl_alert.Text = "Challan Cannot Be Generated";
            }
            #endregion
        }
        catch (Exception ex) { }
    }
    public bool checkedOKSpread(out List<string> appNoList)
    {
        appNoList = new List<string>();
        bool Ok = false;

        FpSpread1.SaveChanges();
        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 5].Value);
            if (check == 1)
            {
                Ok = true;
                appNoList.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag));
            }
        }
        return Ok;
    }
    public string generateChallanNo()
    {
        string recno = string.Empty;

        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");

            string secondreciptqurey = "SELECT ChallanStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + ")";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }

                string acronymquery = d2.GetFunction("SELECT ChallanAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + ")");
                recacr = acronymquery;

                int size = Convert.ToInt32(d2.GetFunction("SELECT  ChallanSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + ")"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;

                Session["lastAccId"] = accountid;
                Session["lastCHlNO"] = receno;

            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    private ListItem getFeecategoryNEW(string Sem)
    {
        string college_code = "13";
        if (ddlcollege.Items.Count > 0)
        {
            college_code = Convert.ToString(ddlcollege.SelectedItem.Value);
        }

        ListItem feeCategory = new ListItem();
        string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + user_code + "' and college_code ='" + college_code + "'");
        DataSet dsFeecat = new DataSet();
        if (linkvalue == "0")
        {
            dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + Sem + " Semester' and college_code=" + college_code + "", "Text");
        }
        else
        {
            string year = newfunction(Sem);
            dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + year + " Year' and college_code=" + college_code + "", "Text");
        }
        if (dsFeecat.Tables.Count > 0 && dsFeecat.Tables[0].Rows.Count > 0)
        {
            feeCategory.Text = Convert.ToString(dsFeecat.Tables[0].Rows[0]["textval"]);
            feeCategory.Value = Convert.ToString(dsFeecat.Tables[0].Rows[0]["TextCode"]);
        }
        else
        {
            feeCategory.Text = " ";
            feeCategory.Value = "-1";
        }
        return feeCategory;
    }
    public string newfunction(string val)
    {
        string value = "";
        if (val.Trim() == "1" || val.Trim() == "2")
        {
            value = "1";
        }
        if (val.Trim() == "3" || val.Trim() == "4")
        {
            value = "2";
        }
        if (val.Trim() == "5" || val.Trim() == "6")
        {
            value = "3";
        }
        if (val.Trim() == "7" || val.Trim() == "8")
        {
            value = "4";
        }
        if (val.Trim() == "9" || val.Trim() == "10")
        {
            value = "5";
        }
        return value;
    }
    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            switch (numeral)
            {
                case "1":
                    romanLettervalue = "I";
                    break;
                case "2":
                    romanLettervalue = "II";
                    break;
                case "3":
                    romanLettervalue = "III";
                    break;
                case "4":
                    romanLettervalue = "IV";
                    break;
                case "5":
                    romanLettervalue = "V";
                    break;
                case "6":
                    romanLettervalue = "VI";
                    break;
                case "7":
                    romanLettervalue = "VII";
                    break;
                case "8":
                    romanLettervalue = "VIII";
                    break;
                case "9":
                    romanLettervalue = "IX";
                    break;
                case "10":
                    romanLettervalue = "X";
                    break;
            }
        }
        return romanLettervalue;
    }
    public string currentFinYear()
    {
        string college_code = "13";
        if (ddlcollege.Items.Count > 0)
        {
            college_code = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
        string finYearid = d2.getCurrentFinanceYear(usercode, college_code);
        return finYearid;
    }
    public string getCollegeBankPK()
    {
        string college_code = "13";
        if (ddlcollege.Items.Count > 0)
        {
            college_code = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
        string bankQ = " select LinkValue from New_InsSettings where LinkName='AdmissionBankForChallan'  and user_code ='" + user_code + "' and college_code ='" + college_code + "'";
        string res = Convert.ToString(d2.GetFunction(bankQ));
        return res;
    }
    public string returnIntegerPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 0)
        {
            strVal = strvalArr[0];
        }
        return strVal;
    }
    public string returnDecimalPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 1)
        {
            strVal = strvalArr[1];
            if (strVal.Length >= 2)
            {
                strVal = strVal.Substring(0, 2);
            }
            else
            {
                while (2 != strVal.Length)
                {
                    strVal = strVal + "0";
                }
            }
        }
        else
        {
            strVal = "00";
        }
        return strVal;
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " Lakhs";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + "  Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    public string DecimalToWords(decimal number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + DecimalToWords(Math.Abs(number));

        string words = "";

        int intPortion = (int)number;
        decimal fraction = (number - intPortion) * 100;
        int decPortion = (int)fraction;

        words = ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
        if (decPortion > 0)
        {
            words += " And ";
            words += ConvertNumbertoWords(decPortion);//NumberToWords(intPortion)
            words += " Paise ";
        }
        return words;
    }
    public string NumberToWords(int number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " Million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }

        return words;
    }
    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
    }
    //Last modified by Idhris  -- 16-12-2016
    public void CheckFiance()
    {
        string CheckFiannce = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IncludeFinance' and user_code ='" + user_code + "' and college_code ='" + ddlcollege.SelectedValue + "' ");
        if (CheckFiannce.Trim() != "0")
        {
            FinanceDiv.Visible = true;
            NotFinanceDiv.Visible = false;
        }
        else
        {
            FinanceDiv.Visible = false;
            NotFinanceDiv.Visible = true;
        }
    }
}


#region Before Change this Page Save Funcation

//public void btnsave_Click()
//    {
//        try
//        {
//            DataSet sdn = new DataSet();
//            FpSpread1.SaveChanges();
//            Boolean setflag = false;
//            //for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
//            //{
//            //    int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 5].Value);
//            //    if (stva == 1)
//            //    {
//            //        setflag = true;
//            //    }
//            //}
//            //if (setflag == false)
//            //{
//            //    lblerror.Visible = true;
//            //    lblerror.Text = "Please Select The Student And Then Proceed";
//            //    return;
//            //}

//            string strinsupdatequery = "";
//            int upddelval = 0;

//            //for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
//            //{
//            //    string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
//            //    string examcode = FpSpread1.Sheets[0].Cells[r, 1].Tag.ToString();
//            //    string applno = FpSpread1.Sheets[0].Cells[r, 2].Tag.ToString();
//            //    int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 5].Value);
//            //    if (stva == 1)
//            //    {
//            //        strinsupdatequery = "delete from studexamelig where roll_no='" + rollno + "' and exam_code='" + examcode + "'";
//            //        upddelval = da.update_method_wo_parameter(strinsupdatequery, "Text");
//            //    }
//            //    else
//            //    {
//            //        strinsupdatequery = "delete from exam_appl_details where appl_no='" + applno + "'";
//            //        upddelval = da.update_method_wo_parameter(strinsupdatequery, "Text");

//            //        strinsupdatequery = "delete from exam_application where appl_no='" + applno + "' and exam_code='" + examcode + "'";
//            //        upddelval = da.update_method_wo_parameter(strinsupdatequery, "Text");

//            //        strinsupdatequery = "if not exists(select * from studexamelig where roll_no='" + rollno + "' and exam_code='" + examcode + "') insert into studexamelig(exam_code,roll_no) values('" + examcode + "','" + rollno + "')";
//            //        upddelval = da.update_method_wo_parameter(strinsupdatequery, "Text");
//            //    }
//            //}
//            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Save Sucessfully.')", true);
//            //loadexamdetails();

//            sdn.Clear();
//            string AppNo = "";
//            string regno = "";
//            string hdrSetPK = string.Empty;
//            string acronym = string.Empty;
//            bool confvalue = false;
//            string chlnDt = "";
//            string alertmsg = "";
//            DateTime trasdate = DateTime.Now;
//            string transtime = DateTime.Now.ToLongTimeString();

//            string finYearid = d2.getCurrentFinanceYear(usercode,Session["collegecode"].ToString() ); 
//            for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
//            {
//                string rollno = FpSpread1.Sheets[0].Cells[i, 1].Text.ToString();
//                string examcode = FpSpread1.Sheets[0].Cells[i, 1].Tag.ToString();
//                string applno = FpSpread1.Sheets[0].Cells[i, 2].Tag.ToString();
//                byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 5].Value);
//                if (check == 1)
//                {
//                    regno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Text);

//                    strinsupdatequery = "update exam_application set is_confirm='1' where appl_no='" + applno + "' and exam_code='" + examcode + "'";
//                    upddelval = da.update_method_wo_parameter(strinsupdatequery, "Text");
//                    //AppNo = FpSpread1.Sheets[0].Cells[i, 2].Tag.ToString();
//                    AppNo = d2.GetFunction(" select app_no from registration where Reg_No='" + regno + "'");
//                    sdn = d2.select_method_wo_parameter(" select ChallanNo from FT_ChallanDet WHERE  App_No = " + AppNo + " and isnull( IsConfirmed,'0') = '0'", "Text");

//                    if (sdn.Tables.Count > 0 && sdn.Tables[0].Rows.Count > 0)
//                    {
//                        string chlnNo = sdn.Tables[0].Rows[0]["ChallanNo"].ToString();

//                        string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,bankFK,TakenAmt,FInyearFk, Convert(nvarchar(20),ChallanDate,101)as ChallanDate from FT_ChallanDet where challanNo='" + chlnNo + "' AND App_No = " + AppNo + " and isnull( IsConfirmed,'0') = '0' select distinct HeaderFk from FT_ChallanDet where challanNo='" + chlnNo + "' AND App_No = " + AppNo + " and isnull( IsConfirmed,'0') = '0'";
//                        DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
//                        bool challanOk = true;
//                        if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
//                        {
//                            for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
//                            {
//                                string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
//                                string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
//                                string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
//                                string finFk = Convert.ToString(dsDet.Tables[0].Rows[j]["FInyearFk"]);
//                                string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);
//                                chlnDt = Convert.ToString(dsDet.Tables[0].Rows[j]["ChallanDate"]);
//                                double amount = 0;
//                                double.TryParse(taknAmt, out amount);

//                                double balamount = 0;
//                                string balAmtStr = d2.GetFunction("select ISNULL(totalamount,0)-ISNULL(paidamount,0) as balamount from FT_FeeAllot where LedgerFK=" + ledger + " and HeaderFK=" + header + " and FeeCategory=" + FeeCategory + " and FinYearFK=" + finFk + " and App_No=" + AppNo + "");
//                                double.TryParse(balAmtStr, out balamount);
//                                if (balamount < amount)
//                                {
//                                    challanOk = false;
//                                }
//                            }
//                        }
//                        if (challanOk)
//                        {
//                            if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
//                            {
//                                string hdrs = string.Empty;
//                                for (int hdr = 0; hdr < dsDet.Tables[1].Rows.Count; hdr++)
//                                {
//                                    if (hdrs == string.Empty)
//                                    {
//                                        hdrs = Convert.ToString(dsDet.Tables[1].Rows[hdr][0]);
//                                    }
//                                    else
//                                    {
//                                        hdrs += "," + Convert.ToString(dsDet.Tables[1].Rows[hdr][0]);
//                                    }
//                                }
//                                int save1 = 0;
//                                try
//                                {
//                                    string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + Session["collegecode"].ToString() + "'";
//                                    save1 = Convert.ToInt32(d2.GetFunction(insqry1));

//                                }
//                                catch { save1 = 0; }

//                                string transcode = generateReceiptNo(out acronym, out hdrSetPK, hdrs);
//                                if (save1 == 5 || (transcode != "" && (hdrSetPK != "" || (isHeaderwise == 0 || isHeaderwise == 2))))
//                                {
//                                    int insOk = 0;

//                                    for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
//                                    {
//                                        string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
//                                        string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
//                                        string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
//                                        string bankPk = Convert.ToString(dsDet.Tables[0].Rows[j]["bankFk"]);
//                                        string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);

//                                        string bankDet = "SELECT DISTINCT BankCode,City FROM FM_FinBankMaster  where CollegeCode=" + Session["collegecode"].ToString() + " and BankPk=" + bankPk + "";
//                                        DataSet dsBnk = d2.select_method_wo_parameter(bankDet, "Text");

//                                        if (dsBnk.Tables.Count > 0)
//                                        {
//                                            if (dsBnk.Tables[0].Rows.Count > 0)
//                                            {
//                                                string iscollected = "0";
//                                                string collecteddate = "";

//                                                iscollected = "1";
//                                                //collecteddate = (Convert.ToDateTime(trasdate).ToString("MM/dd/yyyy")).ToString();
//                                                collecteddate = trasdate.ToString("MM/dd/yyyy");
//                                                string bnkCode = Convert.ToString(dsBnk.Tables[0].Rows[0]["BankCode"]);
//                                                string bnkCity = Convert.ToString(dsBnk.Tables[0].Rows[0]["City"]);

//                                                string insQuery = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,IsCollected,CollectedDate) VALUES('" + trasdate.ToString("MM/dd/yyyy") + "','" + transtime + "','" + transcode + "', 1, " + AppNo + ", " + ledger + ", " + header + ", " + FeeCategory + ", 0, " + taknAmt + ", 4, '" + chlnNo + "', '" + chlnDt + "', " + bankPk + ",'" + bnkCity + "', 1, '0', 0, '', '0', '0', '0', 0, " + Session["usercode"].ToString() + ", " + finYearid + ",'" + iscollected + "','" + collecteddate + "')";

//                                                insOk = d2.update_method_wo_parameter(insQuery, "Text");

//                                                string updateFee = "UPDATE FT_FeeAllot SET PaidAmount = isnull(PaidAmount,0) + " + taknAmt + ",BalAmount = BalAmount-  " + taknAmt + ",ChlTaken = ChlTaken-  " + taknAmt + " WHERE App_No = " + AppNo + " AND FeeCategory = " + FeeCategory + " AND LedgerFK = " + ledger + " and HeaderFk=" + header + "";
//                                                d2.update_method_wo_parameter(updateFee, "Text");

//                                            }
//                                        }
//                                    }

//                                    imgAlert.Visible = true;
//                                    if (insOk > 0)
//                                    {
//                                        #region Update  Challan
//                                        string updateChln = "UPDATE FT_ChallanDet SET RcptTransCode= '" + transcode + "',RcptTransDate= '" + trasdate.ToString("MM/dd/yyyy") + "',IsConfirmed = '1' WHERE ChallanNo = '" + chlnNo + "' AND App_No = " + AppNo + "";
//                                        d2.update_method_wo_parameter(updateChln, "Text");



//                                        #endregion

//                                        #region Update Receipt No
//                                        transcode = transcode.Remove(0, acronym.Length);

//                                        if (save1 != 5)
//                                        {
//                                            string updateRecpt = string.Empty;
//                                            if (isHeaderwise == 0 || isHeaderwise == 2)
//                                            {
//                                                updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + transcode + "+1 where collegecode =" + Session["collegecode"].ToString() + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + Session["collegecode"].ToString() + ")";
//                                            }
//                                            else
//                                            {
//                                                updateRecpt = "update FM_HeaderFinCodeSettings set RcptStNo=" + transcode + "+1 where HeaderSettingPK=" + hdrSetPK + " and FinyearFK=" + finYearid + " and CollegeCode=" + Session["collegecode"].ToString() + "";
//                                            }
//                                            d2.update_method_wo_parameter(updateRecpt, "Text");

//                                            strinsupdatequery = "delete from studexamelig where roll_no='" + rollno + "' and exam_code='" + examcode + "'";
//                                            upddelval = da.update_method_wo_parameter(strinsupdatequery, "Text");



//                                            // upddelval = da.update_method_wo_parameter(strinsupdatequery, "Text");
//                                        }
//                                        #endregion


//                                        confvalue = true;
//                                        alertmsg = "Confirmed Sucessfully";
//                                        FpSpread1.Rows[i].BackColor = Color.LightGreen;
//                                        FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
//                                    }
//                                    else
//                                    {
//                                        alertmsg = "Not Saved";
//                                    }
//                                }
//                                else
//                                {
//                                    alertmsg = "Receipt No Not Assigned For Selected Headers";
//                                }

//                            }
//                            else
//                            {
//                                imgAlert.Visible = true;
//                                alertmsg = "Not Saved";
//                            }
//                        }
//                        else
//                        {
//                            imgAlert.Visible = true;
//                            alertmsg = "Challan Cannot Be Confirmed. Balance Not Available";
//                        }
//                    }
//                    else
//                    {
//                        imgAlert.Visible = true;
//                        alertmsg = "Challan Already Confirmed";
//                    }
//                }
//                else
//                {


//                    strinsupdatequery = "update exam_application set is_confirm='0' where appl_no='" + applno + "' and exam_code='" + examcode + "'";
//                    upddelval = da.update_method_wo_parameter(strinsupdatequery, "Text");


//                    lblerror.Visible = true;
//                    lblerror.Text = "Please Select The Student And Then Proceed";
//                    return;
//                }

//            }
//            if (confvalue)
//            {
//                imgAlert.Visible = true;
//                lbl_alert.Text = "Confirmed Sucessfully";
//                loadexamdetails();
//            }
//            else
//            {
//                imgAlert.Visible = true;
//                lbl_alert.Text = alertmsg;
//            }
//        }
//        catch (Exception ex)
//        {
//            lblerror.Visible = true;
//            lblerror.Text = ex.ToString();
//        }
//    }
#endregion