using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Configuration;
public partial class DepartmentwiseArrearReport : System.Web.UI.Page
{
    static string collegecode = "";
    string usercode = "";
    string singleuser = "", group_user = "";
    string course_id = string.Empty;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    string strbatch = "";
    string strbranch = "";
    string strdegree = "";
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

            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            lblerrormsg.Visible = false;
            lblreportmsg.Visible = false;
            if (!IsPostBack)
            {
                clear();
                collegecode = Session["collegecode"].ToString();

                //Settings for Reg No, Roll No
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";

                if (rblPassorFailSublist.SelectedValue == "0")
                {
                    lblarrearrange.Attributes.Add("style", "display:none");
                    txtarrearrange.Attributes.Add("style", "display:none");
                }
                else
                {
                    lblarrearrange.Attributes.Add("style", "display:block");
                    txtarrearrange.Attributes.Add("style", "display:block");
                }

                string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                DataSet dsmaster = d2.select_method_wo_parameter(Master1, "Text");
                if (dsmaster.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < dsmaster.Tables[0].Rows.Count; k++)
                    {
                        if (dsmaster.Tables[0].Rows[k]["settings"].ToString() == "Roll No" && dsmaster.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (dsmaster.Tables[0].Rows[k]["settings"].ToString() == "Register No" && dsmaster.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                    }
                }
                //End
                BindBatch();
                BindDegree();
                BindBranchMultiple();
            }
            if (rblPassorFailSublist.SelectedValue == "0")
            {
                lblarrearrange.Attributes.Add("style", "display:none");
                txtarrearrange.Attributes.Add("style", "display:none");
            }
            else
            {
                lblarrearrange.Attributes.Add("style", "display:block");
                txtarrearrange.Attributes.Add("style", "display:block");
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void clear()
    {
        ViewSpread.Attributes.Add("style", "display:none; left: 5px; position: absolute; top: 320px;");
        FpSpread1.Visible = false;
        lblerrormsg.Visible = false;
        lblrptname.Visible = false;
        btnexcel.Visible = false;
        btnprint.Visible = false;
        txtexcelname.Visible = false;
        lblreportmsg.Visible = false;
        txtexcelname.Text = "";
    }
    public void BindBatch()
    {
        try
        {
            clear();
            chk_batch.Checked = false;
            txt_batch.Text = "---Select---";
            chklst_batch.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_batch.DataSource = ds;
                chklst_batch.DataTextField = "Batch_year";
                chklst_batch.DataValueField = "Batch_year";
                chklst_batch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }
    public void BindDegree()
    {
        try
        {
            clear();
            chk_degree.Checked = false;
            txt_degree.Text = "---Select---";
            chklst_degree.Items.Clear();
            if (txt_batch.Text != "---Select---")
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                ds.Dispose();
                ds.Reset();
                ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_degree.DataSource = ds;
                    chklst_degree.DataTextField = "course_name";
                    chklst_degree.DataValueField = "course_id";
                    chklst_degree.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void BindBranchMultiple()
    {
        try
        {
            clear();
            for (int i = 0; i < chklst_degree.Items.Count; i++)
            {

                if (chklst_degree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "'" + chklst_degree.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        course_id = course_id + "," + "'" + chklst_degree.Items[i].Value.ToString() + "'";
                    }
                }
            }
            chk_branch.Checked = false;
            txt_branch.Text = "---Select---";
            chklst_branch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            if (txt_degree.Text != "---Select---")
            {
                ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_branch.DataSource = ds;
                    chklst_branch.DataTextField = "dept_name";
                    chklst_branch.DataValueField = "degree_code";
                    chklst_branch.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }
    protected void chk_batch_ChekedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int chkbatchcount = 0;
            if (chk_batch.Checked == true)
            {
                chkbatchcount++;
                for (int i = 0; i < chklst_batch.Items.Count; i++)
                {
                    chklst_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (chklst_batch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklst_batch.Items.Count; i++)
                {
                    chklst_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "---Select---";
            }

            BindDegree();
            BindBranchMultiple();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chklst_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int batchcount = 0;
            chk_batch.Checked = false;
            txt_batch.Text = "---Select---";

            for (int i = 0; i < chklst_batch.Items.Count; i++)
            {
                if (chklst_batch.Items[i].Selected == true)
                {
                    batchcount = batchcount + 1;
                }
            }
            if (batchcount > 0)
            {
                txt_batch.Text = "Batch(" + batchcount.ToString() + ")";
                if (batchcount == chklst_batch.Items.Count)
                {
                    chk_batch.Checked = true;
                }
            }
            BindDegree();
            BindBranchMultiple();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chk_degree_ChekedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chk_degree.Checked == true)
            {
                for (int i = 0; i < chklst_degree.Items.Count; i++)
                {
                    chklst_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (chklst_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklst_degree.Items.Count; i++)
                {
                    chklst_degree.Items[i].Selected = false;
                }
                txt_degree.Text = "---Select---";
            }
            BindBranchMultiple();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chklst_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int degreecount = 0;
            txt_degree.Text = "---Select---";
            chk_degree.Checked = false;
            for (int i = 0; i < chklst_degree.Items.Count; i++)
            {
                if (chklst_degree.Items[i].Selected == true)
                {
                    degreecount = degreecount + 1;
                }
            }
            if (degreecount > 0)
            {
                txt_degree.Text = "Degree(" + degreecount + ")";
                if (degreecount == chklst_degree.Items.Count)
                {
                    chk_degree.Checked = true;
                }
            }
            BindBranchMultiple();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chk_branch_ChekedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txt_branch.Text = "---Select---";
            if (chk_branch.Checked == true)
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = true;
                }
                if (chklst_branch.Items.Count > 0)
                {
                    txt_branch.Text = "Department(" + (chklst_branch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chklst_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int branchcount = 0;
            txt_branch.Text = "---Select---";
            chk_branch.Checked = false;

            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    branchcount = branchcount + 1;
                }
            }
            if (branchcount > 0)
            {
                txt_branch.Text = "Department(" + branchcount.ToString() + ")";
                if (branchcount == chklst_branch.Items.Count)
                {
                    chk_branch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            int arrearrange = 0;
            int.TryParse(Convert.ToString(txtarrearrange.Text), out arrearrange);
            //if (arrearrange >= 0 && arrearrange <= 100)
            //{
            //txtarrearrange.Visible=
            if (rblPassorFailSublist.SelectedValue == "0" || rblPassorFailSublist.SelectedValue == "1")
            {
                FpSpread1.Width = 1000;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Visible = true;
                FpSpread1.Sheets[0].AutoPostBack = false;

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 6;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                //Setting
                if (Session["Rollflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[3].Visible = false;
                }
                else
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[3].Visible = true;
                }

                if (Session["Regflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                }
                else
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                }
                //End

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.ForeColor = Color.Black;
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.Border.BorderColor = ColorTranslator.FromHtml("#FFFFFF");
                darkstyle.Border.BorderSize = 1;

                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpSpread1.ActiveSheetView.Columns.Default.Border.BorderColor = System.Drawing.Color.Black;

                FarPoint.Web.Spread.TextCellType snocell = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType departmentcell = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType regcell = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType rollcell = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType studentnamecell = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType subjectcodecell = new FarPoint.Web.Spread.TextCellType();

                FpSpread1.Sheets[0].AutoPostBack = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.NO";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "DEPARTMENT";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].ForeColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "REG NO";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 2].ForeColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 2].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "ROLL NO";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 3].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 3].ForeColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 3].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "STUDENT NAME";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 4].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 4].ForeColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 4].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 4].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                if (rblPassorFailSublist.SelectedValue == "1")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "SUBJECT CODE (Failed)";
                }
                else if (rblPassorFailSublist.SelectedValue == "0")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "SUBJECT CODE (Passed)";
                }
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 5].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 5].ForeColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 5].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 5].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 5].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 5].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].Columns[0].Width = 50;
                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[2].Width = 150;
                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Width = 150;
                FpSpread1.Sheets[0].Columns[5].Width = 200;

                //Batch, Degree, Department
                //Get Batch
                for (int i = 0; i < chklst_batch.Items.Count; i++)
                {
                    if (chklst_batch.Items[i].Selected == true)
                    {
                        if (strbatch == "")
                        {
                            strbatch = "'" + chklst_batch.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strbatch = strbatch + "," + "'" + chklst_batch.Items[i].Value.ToString() + "'";
                        }
                    }
                }//end
                //Get Degree
                for (int i = 0; i < chklst_degree.Items.Count; i++)
                {
                    if (chklst_degree.Items[i].Selected == true)
                    {
                        if (strdegree == "")
                        {
                            strdegree = "'" + chklst_degree.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strdegree = strdegree + "," + "'" + chklst_degree.Items[i].Value.ToString() + "'";
                        }
                    }
                }//End
                //Get Branch
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    if (chklst_branch.Items[i].Selected == true)
                    {
                        if (strbranch == "")
                        {
                            strbranch = "'" + chklst_branch.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strbranch = strbranch + "," + "'" + chklst_branch.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                //End
                //Bind Arrear Student
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                Boolean recflag = false;
                //   string ArrearCount = " select m.roll_no,count(distinct m.subject_no) as arrearcount,r.Batch_Year from mark_entry m,Registration r where m.roll_no=r.Roll_No and r.degree_code in(" + strbranch + ") and r.Batch_Year in(" + strbatch + ") and  m.subject_no not in(select subject_no from mark_entry m1 where m.exam_code=m1.exam_code and m.subject_no=m1.subject_no and m.roll_no=m1.roll_no and m1.result='pass') group by m.roll_no,r.Batch_Year,r.degree_code, m.roll_no having count(distinct m.subject_no) between 1 and " + txtarrearrange.Text + "  order by arrearcount,r.Batch_Year,r.degree_code, m.roll_no";

                DataSet dsarrearcnt = new DataSet();
                if (rblPassorFailSublist.SelectedValue == "1")
                {
                    if (arrearrange >= 0 && arrearrange <= 100)
                    {
                    }
                    else
                    {
                        lblerrormsg.Text = "The Arrear Range Must Be Between 0 and 100";
                        lblerrormsg.Visible = true;
                        ViewSpread.Attributes.Add("style", "display:none; left: 5px; position: absolute; top: 320px;");
                        FpSpread1.Visible = false;
                        lblreportmsg.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnexcel.Visible = false;
                        btnprint.Visible = false;
                        return;
                    }

                    string ArrearCount = " select r.Batch_Year,r.degree_code,r.reg_no,r.roll_no,count( distinct sc.subject_no) arrearcount from Registration r,subjectChooser sc,subject s,mark_entry m where r.Roll_No=sc.Roll_No and sc.subject_no=s.subject_no and sc.roll_no=m.roll_no and sc.subject_no=m.subject_no and m.roll_no=r.Roll_No and m.subject_no=s.subject_no and r.degree_code in(" + strbranch + ") and r.Batch_Year in(" + strbatch + ")   and sc.subject_no not in(select m.subject_no from mark_entry m where m.roll_no=r.Roll_No and m.subject_no=sc.subject_no and m.result='Pass') group by r.Batch_Year,r.degree_code,r.reg_no,r.roll_no having count( distinct sc.subject_no)  between 1 and " + txtarrearrange.Text + "  order by r.Batch_Year,r.degree_code,r.reg_no,r.roll_no";
                    if (txtarrearrange.Text == "0")
                    {
                        FpSpread1.Sheets[0].Columns[5].Visible = false;
                        ArrearCount = "select r1.roll_no,r1.Reg_No,r1.Stud_Name,r1.Batch_Year,de.dept_acronym,c.Course_Name,de.Dept_Name,r1.Current_Semester from Registration r1,Degree d,Course c,Department de where r1.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r1.Batch_Year in(" + strbatch + ") and r1.degree_code in(" + strbranch + ") and r1.Roll_No not in(select m.roll_no from mark_entry m,Registration r where m.roll_no=r.Roll_No and r.degree_code in(" + strbranch + ") and r.Batch_Year in(" + strbatch + ") and  m.subject_no not in(select subject_no from mark_entry m1 where m.exam_code=m1.exam_code and m.subject_no=m1.subject_no  and m.roll_no=m1.roll_no and m1.result='pass') group by m.roll_no,r.Batch_Year,r.degree_code, m.roll_no having count(distinct m.subject_no) >0 ) order by r1.Batch_Year,r1.degree_code, r1.roll_no";
                    }

                    dsarrearcnt = d2.select_method_wo_parameter(ArrearCount, "text");

                    DataView dvarrearcount = new DataView();
                    DataView dvarreardetails = new DataView();
                    if (dsarrearcnt.Tables.Count > 0 && dsarrearcnt.Tables[0].Rows.Count > 0)
                    {
                        btnexcel.Visible = true;
                        btnprint.Visible = true;
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;

                        int count = Convert.ToInt32(txtarrearrange.Text);
                        int sno = 0;
                        int passcount = 0;
                        int column = 0;
                        if (txtarrearrange.Text == "0")
                        {
                            //FpSpread1.Sheets[0].RowCount++;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Text = "TOTAL ARREAR:" + passcount;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Font.Bold = true;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].BackColor = Color.Wheat;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].ForeColor = Color.Black;
                            //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, column, column + 1, 6);

                            for (int i = 0; i < dsarrearcnt.Tables[0].Rows.Count; i++)
                            {
                                recflag = true;
                                string coursename = dsarrearcnt.Tables[0].Rows[i]["Course_Name"].ToString();
                                string regno = dsarrearcnt.Tables[0].Rows[i]["Reg_No"].ToString();
                                string studname = dsarrearcnt.Tables[0].Rows[i]["Stud_Name"].ToString();
                                string studrollno = dsarrearcnt.Tables[0].Rows[i]["roll_no"].ToString();
                                string batchyear = dsarrearcnt.Tables[0].Rows[i]["Batch_Year"].ToString();
                                string acronym = dsarrearcnt.Tables[0].Rows[i]["dept_acronym"].ToString();
                                string sem = dsarrearcnt.Tables[0].Rows[i]["Current_Semester"].ToString();
                                FpSpread1.Sheets[0].RowCount++;
                                sno++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].CellType = snocell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Text = sno.ToString();

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].CellType = departmentcell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].Text = batchyear + "-" + coursename + "-" + acronym;

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 2].CellType = regcell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 2].Text = regno.ToString();

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 3].CellType = rollcell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 3].Text = studrollno.ToString();

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 4].CellType = studentnamecell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 4].Text = studname.ToString();

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 2].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 3].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 4].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 4].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 5].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 5].Font.Name = "Book Antiqua";
                            }
                        }
                        else
                        {  //Get Details of Arrear Student
                            //string arrearpassing = "select m.roll_no,s.subject_code,r.Reg_No,r.Stud_Name,r.Batch_Year,de.dept_acronym,c.Course_Name,de.Dept_Name,r.Current_Semester from mark_entry m,Registration r,subject s,degree d,course c,Department de where m.roll_no=r.Roll_No and m.subject_no=s.subject_no and r.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and de.Dept_Code=d.Dept_Code and r.degree_code in(" + strbranch + ") and r.Batch_Year in(" + strbatch + ") and  m.subject_no not in(select subject_no from mark_entry m1   where m.exam_code=m1.exam_code and m.subject_no=m1.subject_no and m.roll_no=m1.roll_no and  m1.result='pass')  order by Batch_Year ,m.roll_no ";
                            string arrearpassing = " select r.Batch_Year,r.degree_code,r.reg_no,r.roll_no,r.Stud_Name,s.subject_code,s.subject_name,de.dept_acronym,c.Course_Name,de.Dept_Name,r.Current_Semester,sc.semester  from Registration r,subjectChooser sc,subject s,degree d,course c,Department de,mark_entry m where r.Roll_No=sc.Roll_No and sc.subject_no=s.subject_no and r.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and de.Dept_Code=d.Dept_Code and sc.subject_no=m.subject_no and m.roll_no=r.Roll_No and m.subject_no=s.subject_no  and r.degree_code in(" + strbranch + ") and r.Batch_Year in(" + strbatch + ")  and sc.subject_no not in(select m.subject_no from mark_entry m where m.roll_no=r.Roll_No and m.subject_no=sc.subject_no and m.result='Pass')  order by r.Batch_Year,r.degree_code,r.reg_no,r.roll_no";
                            DataSet dsarrearpassing = d2.select_method_wo_parameter(arrearpassing, "text");
                            int arrearsem = 0;
                            if (ddlsem.SelectedItem.ToString() != "All")
                            {
                                arrearsem = Convert.ToInt32(ddlsem.SelectedItem.ToString());
                            }
                            for (int arrear = 0; arrear < count; arrear++)
                            {
                                passcount++;

                                if (count == passcount || arrearsem == 0)
                                {
                                    dsarrearcnt.Tables[0].DefaultView.RowFilter = "arrearcount=" + passcount + " ";
                                    dvarrearcount = dsarrearcnt.Tables[0].DefaultView;
                                    if (dvarrearcount.Count > 0)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Text = "TOTAL ARREAR:" + passcount;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].BackColor = Color.Wheat;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].ForeColor = Color.Black;
                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, column, column + 1, 6);

                                        for (int dv = 0; dv < dvarrearcount.Count; dv++)
                                        {
                                            string rollno = dvarrearcount[dv]["roll_no"].ToString();
                                            dsarrearpassing.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                                            dvarreardetails = dsarrearpassing.Tables[0].DefaultView;
                                            Boolean allowflag = false;
                                            if (arrearsem > 0)
                                            {
                                                for (int ar = 0; ar < dvarreardetails.Count; ar++)
                                                {
                                                    string semval = dvarreardetails[0]["semester"].ToString();
                                                    if (semval != arrearsem.ToString())
                                                    {
                                                        allowflag = true;
                                                        ar = dvarreardetails.Count;
                                                    }
                                                }
                                            }
                                            if (allowflag == false)
                                            {
                                                if (dvarreardetails.Count > 0)
                                                {
                                                    recflag = true;
                                                    sno++;
                                                    string coursename = dvarreardetails[0]["Course_Name"].ToString();
                                                    string regno = dvarreardetails[0]["Reg_No"].ToString();
                                                    string studname = dvarreardetails[0]["Stud_Name"].ToString();
                                                    string studrollno = dvarreardetails[0]["roll_no"].ToString();
                                                    string batchyear = dvarreardetails[0]["Batch_Year"].ToString();
                                                    string acronym = dvarreardetails[0]["dept_acronym"].ToString();
                                                    string sem = dvarreardetails[0]["Current_Semester"].ToString();
                                                    string subcode = "";
                                                    string scode = "";
                                                    if (dvarreardetails.Count > 1)
                                                    {
                                                        for (int code = 0; code < dvarreardetails.Count; code++)
                                                        {
                                                            if (scode == "")
                                                            {
                                                                scode = dvarreardetails[code]["subject_code"].ToString();
                                                            }
                                                            else
                                                            {
                                                                scode = scode + ", " + dvarreardetails[code]["subject_code"].ToString();
                                                                subcode = scode;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        subcode = dvarreardetails[0]["subject_code"].ToString();
                                                    }

                                                    FpSpread1.Sheets[0].RowCount++;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].CellType = snocell;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Text = sno.ToString();

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].CellType = departmentcell;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].Text = batchyear + "-" + coursename + "-" + acronym;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 2].CellType = regcell;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 2].Text = regno.ToString();

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 3].CellType = rollcell;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 3].Text = studrollno.ToString();

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 4].CellType = studentnamecell;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 4].Text = studname.ToString();

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].CellType = subjectcodecell;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 5].Text = subcode.ToString();

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 1].Font.Name = "Book Antiqua";

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 2].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 2].Font.Name = "Book Antiqua";

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 3].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 3].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 3].Font.Name = "Book Antiqua";

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 4].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 4].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 4].Font.Name = "Book Antiqua";

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 5].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 5].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, column + 5].Font.Name = "Book Antiqua";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        ViewSpread.Attributes.Add("style", "display:block; left: 5px; position: absolute; top: 320px;");
                        FpSpread1.Visible = true;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.SaveChanges();
                    }
                }
                else if (rblPassorFailSublist.SelectedValue == "0")
                {
                    DataSet dspassed = new DataSet();
                    DataSet dsStud = new DataSet();
                    string qry = "select distinct r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,dt.dept_acronym,r.serialno from Registration r,Degree dg,Course c,Department dt where c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=c.college_code and r.college_code=dt.college_code and r.degree_code=dg.Degree_Code and dt.Dept_Code=dg.Dept_Code and c.Course_Id=dg.Course_Id and r.Batch_Year in (" + strbatch + ") and r.degree_code in (" + strbranch + ") and r.DelFlag=0 and r.Exam_Flag<>'debar'  order by r.Batch_Year,r.degree_code,r.Roll_No";
                    dsStud = d2.select_method_wo_parameter(qry, "Text");


                    string qrypass = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,dt.dept_acronym,r.serialno,sc.semester,s.subject_name,s.subject_code from subjectchooser sc,subject s,mark_entry m,Registration r,Degree dg,Course c,Department dt where c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=c.college_code and r.college_code=dt.college_code and r.degree_code=dg.Degree_Code and dt.Dept_Code=dg.Dept_Code and c.Course_Id=dg.Course_Id and sc.subject_no=m.subject_no and sc.subject_no=s.subject_no and sc.roll_no=m.roll_no and m.subject_no=s.subject_no and sc.roll_no=r.roll_no  and m.result='Pass'  and r.Batch_Year in (" + strbatch + ") and r.degree_code in (" + strbranch + ") and r.DelFlag=0 and r.Exam_Flag<>'debar' order by r.Batch_Year,r.degree_code,r.Roll_No,sc.semester,s.subject_code";
                    if (Convert.ToString(ddlsem.SelectedItem).ToLower().Trim() != "all")
                    {
                        qrypass = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,dt.dept_acronym,r.serialno,sc.semester,s.subject_name,s.subject_code from subjectchooser sc,subject s,mark_entry m,Registration r,Degree dg,Course c,Department dt where c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=c.college_code and r.college_code=dt.college_code and r.degree_code=dg.Degree_Code and dt.Dept_Code=dg.Dept_Code and c.Course_Id=dg.Course_Id and  sc.subject_no=m.subject_no and sc.subject_no=s.subject_no and sc.roll_no=m.roll_no and m.subject_no=s.subject_no and sc.roll_no=r.roll_no  and m.result='Pass'  and r.Batch_Year in (" + strbatch + ") and r.degree_code in(" + strbranch + ") and r.DelFlag=0 and r.Exam_Flag<>'debar' and sc.semester='" + Convert.ToString(ddlsem.SelectedItem).Trim() + "' order by r.Batch_Year,r.degree_code,r.Roll_No,sc.semester,s.subject_code";//r.Batch_Year,r.degree_code,r.reg_no,r.roll_no
                    }
                    dspassed = d2.select_method_wo_parameter(qrypass, "text");

                    DataView dvpassedcount = new DataView();
                    DataView dvpasseddetails = new DataView();
                    if (dspassed.Tables.Count > 0 && dspassed.Tables[0].Rows.Count > 0)
                    {
                        btnexcel.Visible = true;
                        btnprint.Visible = true;
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;
                        DataView dvPassed = new DataView();
                        if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
                        {
                            int sno = 0;
                            for (int rows = 0; rows < dsStud.Tables[0].Rows.Count; rows++)
                            {
                                recflag = true;
                                string roll_no = Convert.ToString(dsStud.Tables[0].Rows[rows]["Roll_No"]);
                                string reg_no = Convert.ToString(dsStud.Tables[0].Rows[rows]["Reg_No"]);
                                string stud_name = Convert.ToString(dsStud.Tables[0].Rows[rows]["Stud_Name"]);
                                string batchyr = Convert.ToString(dsStud.Tables[0].Rows[rows]["Batch_Year"]);
                                string course_name = Convert.ToString(dsStud.Tables[0].Rows[rows]["Course_Name"]);
                                string dept_name = Convert.ToString(dsStud.Tables[0].Rows[rows]["Dept_Name"]);
                                string dept_acrname = Convert.ToString(dsStud.Tables[0].Rows[rows]["dept_acronym"]);
                                string subject_code = "";
                                FpSpread1.Sheets[0].RowCount++;
                                sno++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = snocell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = departmentcell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyr + "-" + course_name + "-" + dept_acrname;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regcell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(reg_no);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = rollcell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(roll_no);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = studentnamecell;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(stud_name);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = subjectcodecell;
                                dspassed.Tables[0].DefaultView.RowFilter = "Roll_No='" + roll_no + "'";
                                dvPassed = dspassed.Tables[0].DefaultView;
                                if (dvPassed.Count > 0)
                                {
                                    subject_code = "";
                                    for (int subcode = 0; subcode < dvPassed.Count; subcode++)
                                    {
                                        if (subject_code == "")
                                        {
                                            subject_code = Convert.ToString(dvPassed[subcode]["subject_code"]);
                                        }
                                        else
                                        {
                                            subject_code += ", " + Convert.ToString(dvPassed[subcode]["subject_code"]);
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(subject_code);
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("---");
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            }
                        }
                        ViewSpread.Attributes.Add("style", "display:block; left: 5px; position: absolute; top: 320px;");
                        FpSpread1.Visible = true;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.SaveChanges();
                    }
                    else
                    {
                        lblerrormsg.Text = "No Records Found";
                        lblerrormsg.Visible = true;
                        ViewSpread.Attributes.Add("style", "display:none; left: 5px; position: absolute; top: 320px;");
                        FpSpread1.Visible = false;
                        lblreportmsg.Visible = false;
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;
                        btnexcel.Visible = false;
                        btnprint.Visible = false;
                        return;
                    }
                }
                if (recflag == false)
                {
                    lblerrormsg.Text = "No Records Found";
                    lblerrormsg.Visible = true;
                    ViewSpread.Attributes.Add("style", "display:none; left: 5px; position: absolute; top: 320px;");
                    FpSpread1.Visible = false;
                    lblreportmsg.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    btnexcel.Visible = false;
                    btnprint.Visible = false;
                }
            }
            else
            {
                lblerrormsg.Text = "Please Choose Any One Option Passd Subjects Or Failed Subjects";
                lblerrormsg.Visible = true;
                ViewSpread.Attributes.Add("style", "display:none; left: 5px; position: absolute; top: 320px;");
                FpSpread1.Visible = false;
                lblreportmsg.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnexcel.Visible = false;
                btnprint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblreportmsg.Visible = false;
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                lblreportmsg.Text = "Please Enter Your Report Name";
                lblreportmsg.Visible = true;
            }
        }

        catch (Exception ex)
        {
            lblreportmsg.Text = ex.ToString();
            lblreportmsg.Visible = true;
        }
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < chklst_batch.Items.Count; i++)
            {
                if (chklst_batch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "" + chklst_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "" + chklst_batch.Items[i].Value.ToString() + "";
                    }

                }
            }

            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklst_branch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklst_branch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (strbatch != "" && strbranch != "")
            {
                string degreedetails = "";
                if (rblPassorFailSublist.SelectedValue == "1")
                {
                    string semquery = " select distinct r.Current_Semester from mark_entry m,Registration r where m.roll_no=r.Roll_No and r.degree_code in(" + strbranch + ") and r.Batch_Year in(" + strbatch + ") and  m.subject_no not in(select subject_no from mark_entry m1 where m.exam_code=m1.exam_code and  m.subject_no=m1.subject_no and m.roll_no=m1.roll_no and m1.result='pass') group by m.roll_no,r.Current_Semester having count(m.subject_no) between 1 and " + txtarrearrange.Text + "  ";

                    DataSet dssemester = d2.select_method_wo_parameter(semquery, "text");

                    string year = "";
                    string year1 = "";
                    string year2 = "";
                    string year3 = "";
                    string year4 = "";
                    string semester = "";

                    if (dssemester.Tables[0].Rows.Count > 0)
                    {
                        for (int s = 0; s < dssemester.Tables[0].Rows.Count; s++)
                        {
                            string sem = dssemester.Tables[0].Rows[s]["Current_Semester"].ToString();

                            //Semester
                            if (semester == "")
                            {
                                semester = sem;
                            }
                            else
                            {
                                semester = semester + "," + sem;
                            }
                            //Year

                            if (sem == "1" || sem == "2")
                            {
                                year1 = "I";
                            }
                            else if (sem == "3" || sem == "4")
                            {
                                year2 = "II";
                            }
                            else if (sem == "5" || sem == "6")
                            {
                                year3 = "III";
                            }
                            else if (sem == "7" || sem == "8")
                            {
                                year4 = "IV";
                            }
                        }
                    }

                    if (year1 != "")
                    {
                        year = year1;
                    }
                    if (year2 != "")
                    {
                        year = year + ", " + year2;
                    }
                    if (year3 != "")
                    {
                        year = year + ", " + year3;
                    }
                    if (year4 != "")
                    {
                        year = year + ", " + year4;
                    }

                    degreedetails = "Arrear Count Wise Student List" + "@" + "Batch:" + strbatch + "                                                                                                 Year:" + year + " " + "@" + "Sem:" + semester + "";
                }
                if (rblPassorFailSublist.SelectedValue == "0")
                {
                    degreedetails = "Department Wise Student's Passed Subject Count List" + "@" + "Batch:" + strbatch + "";
                }
                string pagename = "DepartmentwiseArrearReport.aspx";
                FpSpread1.Visible = true;
                Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            else
            {
                lblerrormsg.Text = "Please Select Batch Year, Degree and Department";
                lblerrormsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void rblPassorFailSublist_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }

}