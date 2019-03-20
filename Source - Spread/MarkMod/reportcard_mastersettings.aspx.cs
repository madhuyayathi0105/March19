using System;
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class reportcard_mastersettings : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    static Boolean forschoolsetting = false;
    string collegecode = "";
    string usercode = "";
    string columnfield = string.Empty;
    string singleuser = "";
    string group_user = "";
    string course_id = string.Empty;

    FarPoint.Web.Spread.ComboBoxCellType combocol = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            chkboxsel_all.AutoPostBack = true;
            fpspread.SaveChanges();
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Request.FilePath.Contains("CAMHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MarkMod/CAMHome.aspx");
                    return;
                }
            }
            usercode = Convert.ToString(Session["usercode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            collegecode = Convert.ToString(Session["collegecode"]);

            if (!IsPostBack)
            {
                chkboxsel_all.AutoPostBack = true;
                lblerrmsg1.Visible = false;
                lblerrmsg.Visible = false;
                lblerrmsg2.Visible = false;
                checktable();
                loadcollege();
                batch();
                BindDegree();
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                Titlename();
                Activityname();
                descname();
                SubRemark();
                Panel1addsub.Visible = false;

                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;
                pnldesc.Visible = false;

                fpspread.Visible = false;
                fpspread.Sheets[0].RowHeader.Visible = false;
                fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.White;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
                fpspread.Sheets[0].AutoPostBack = false;
                fpspread.CommandBar.Visible = false;
                fpspread.Sheets[0].RowCount = 0;
                int fpcol = 9;
                int stcol = 3;
                if (chkPartname.Checked)
                {
                    fpcol = 10;
                    stcol = 4;
                }
                fpspread.Sheets[0].ColumnCount = fpcol;
                fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 40;

                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";

                fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 53;
                //fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 100;
                fpspread.Sheets[0].ColumnHeader.Columns[stcol].Width = 153;
                fpspread.Sheets[0].ColumnHeader.Columns[stcol + 1].Width = 250;
                fpspread.Sheets[0].Columns[0].Locked = true;
                fpspread.Sheets[0].Columns[2].Locked = true;
                fpspread.Sheets[0].Columns[stcol].Locked = true;

                for (int i = 0; i < fpcol; i++)
                {
                    fpspread.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                }

                fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = " ";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Part";
                if (chkPartname.Checked)
                {
                    fpspread.Sheets[0].ColumnHeader.Cells[0, stcol - 1].Text = "Part Name";
                }
                fpspread.Sheets[0].ColumnHeader.Cells[0, stcol].Text = "Sub Title";
                fpspread.Sheets[0].ColumnHeader.Cells[0, stcol + 1].Text = "Title Name";
                fpspread.Sheets[0].ColumnHeader.Cells[0, stcol + 2].Text = "Direct";
                fpspread.Sheets[0].ColumnHeader.Cells[0, stcol + 3].Text = "Activity";
                fpspread.Sheets[0].ColumnHeader.Cells[0, stcol + 4].Text = "Description";
                fpspread.Sheets[0].ColumnHeader.Cells[0, stcol + 5].Text = "Grade";
                fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 120;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                darkstyle.ForeColor = System.Drawing.Color.White;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Border.BorderSize = 0;
                darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                //fpspread.Sheets[0].ColumnHeader.Cells[0, 1].CellType = chkboxsel_all;

                for (int g = 0; g < fpspread.Sheets[0].ColumnHeader.Columns.Count; g++)
                {
                    fpspread.Sheets[0].ColumnHeader.Cells[0, g].Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, g].Font.Name = "Book Antiqua";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, g].Font.Bold = true;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, g].ForeColor = Color.White;
                }

                fpspread.SaveChanges();

                fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;

                string grouporusercodeschool = "";
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercodeschool = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                }
                else
                {
                    grouporusercodeschool = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }

                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercodeschool + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = da.select_method_wo_parameter(sqlschool, "Text");
                //if (schoolds.Tables[0].Rows.Count > 0)
                //{
                //    string schoolvalue =Convert.ToString( schoolds.Tables[0].Rows[0]["value"]);
                //    if (schoolvalue.Trim() == "0")
                //    {
                forschoolsetting = true;
                lblcollege.Text = "School";
                lblbatch.Text = "Year";
                lbldeg.Text = "School Type";
                lblbranch.Text = "Standard";
                //lblDuration.Text = "Term";
                //Label1.Text = "Test Mark R11-Continuous Assessment Report";
                //lbldeg.Attributes.Add("Style", "font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 229px;    position: absolute;    top: 187px;");
                //tbdeg.Attributes.Add("Style", "   font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 20px;    margin-left: 207px;    position: absolute;    top: 187px;    width: 100px;");
                //lblbranch.Attributes.Add("Style", " color: Black;    display: inline-block;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    margin-left: 314px;    margin-top: -10px;    position: absolute;    width: 90px;");
                //txtbranch.Attributes.Add("Style", "font-family: 'Book Antiqua';    font-size: medium;    font-weight: bold;    height: 20px;    margin-left: 386px;    position: absolute;    top: 187px;    width: 100px;");

                //    }
                //    else
                //    {
                //        forschoolsetting = false;
                //    }
                //}
                //else
                //{
                //    forschoolsetting = false;
                //}
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    #endregion Page Load

    #region Bind Header

    public void loadcollege()
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]) + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            ds.Dispose();
            ds.Reset();
            ds = da.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    public void batch()
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            Chkbat.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = da.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Chkbat.DataSource = ds;
                Chkbat.DataTextField = "Batch_year";
                Chkbat.DataValueField = "Batch_year";
                Chkbat.DataBind();
            }
            //for (int i = 0; i < Chkbat.Items.Count; i++)
            //{
            //    Chkbatsel.Checked = true;
            //    Chkbat.Items[i].Selected = true;
            //}
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    public void BindDegree()
    {
        try
        {
            lblerrmsg.Visible = false;
            lblerrmsg.Text = "";
            string colcodenew = Convert.ToString(ddlcollege.SelectedValue);

            Chkdeg.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = da.BindDegree(singleuser, group_user, colcodenew, usercode);
            //ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Chkdeg.DataSource = ds;
                Chkdeg.DataTextField = "course_name";
                Chkdeg.DataValueField = "course_id";
                Chkdeg.DataBind();

                //for (int i = 0; i < Chkdeg.Items.Count; i++)
                //{
                //    Chkdeg.Items[i].Selected = true;
                //    if (Chkdeg.Items[i].Selected == true)
                //    {
                //        count2 += 1;
                //    }
                //    if (Chkdeg.Items.Count == count2)
                //    {
                //        Chkdegsel.Checked = true;
                //    }
                //}

            }

            //for (int i = 0; i < Chkdeg.Items.Count;i++ )
            //{

            //    Chkdeg.Items[i].Selected = true;

            //}
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            lblerrmsg.Visible = false;
            lblerrmsg.Text = "";
            for (int i = 0; i < Chkdeg.Items.Count; i++)
            {
                if (Chkdeg.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + Chkdeg.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + Chkdeg.Items[i].Value.ToString() + "";
                    }
                }
            }

            chklstbranch.Items.Clear();
            if (course_id.ToString() != "")
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                ds.Dispose();
                ds.Reset();
                ds = da.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds;
                    chklstbranch.DataTextField = "dept_name";
                    chklstbranch.DataValueField = "degree_code";
                    chklstbranch.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    public void Titlename()
    {
        try
        {
            lblerrmsg.Visible = false;
            lblerrmsg.Text = "";

            ddltitlename.Items.Clear();
            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'RTnam' and college_code = '" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            string strtit = "";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltitlename.DataSource = ds;
                ddltitlename.DataTextField = "TextVal";
                ddltitlename.DataValueField = "TextCode";
                ddltitlename.DataBind();

                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    if (strtit == "")
                //    {
                //        strtit = ds.Tables[0].Rows[i]["TextVal"].ToString();
                //    }
                //    else
                //    {
                //        strtit = strtit + "-" + ds.Tables[0].Rows[i]["TextVal"].ToString();
                //    }
                //}
                //string[] strcomo1 = strtit.Split('-');
                //combocol = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
                combocol.DataSource = ds;
                combocol.DataTextField = "TextVal";
                combocol.DataValueField = "TextCode";
                combocol.ShowButton = false;
                combocol.AutoPostBack = true;
                combocol.UseValue = true;
                int stcol = 3;
                if (chkPartname.Checked)
                {
                    stcol = 4;
                }
                for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                {
                    for (int j = 0; j < fpspread.Sheets[0].ColumnCount; j++)
                    {
                        fpspread.Sheets[0].Cells[i, stcol + 1].CellType = combocol;
                    }
                }

            }

        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    public void Activityname()
    {
        try
        {
            lblerrmsg.Visible = false;
            lblerrmsg.Text = "";

            ddlactivity.Items.Clear();
            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'RActv' and college_code = '" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlactivity.DataSource = ds;
                ddlactivity.DataTextField = "TextVal";
                ddlactivity.DataValueField = "TextCode";
                ddlactivity.DataBind();
            }

        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    public void descname()
    {
        try
        {
            lblerrmsg.Visible = false;
            lblerrmsg.Text = "";

            ddldescrip.Items.Clear();
            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'RAdes' and college_code = '" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldescrip.DataSource = ds;
                ddldescrip.DataTextField = "TextVal";
                ddldescrip.DataValueField = "TextCode";
                ddldescrip.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    public void SubRemark()
    {
        try
        {
            lblerrmsg.Visible = false;
            lblerrmsg.Text = "";
            ddlsubrmrk.Items.Clear();
            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'Rmrk' and college_code = '" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubrmrk.DataSource = ds;
                ddlsubrmrk.DataTextField = "TextVal";
                ddlsubrmrk.DataValueField = "TextCode";
                ddlsubrmrk.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    #endregion Bind Header

    public bool bindfp(string batch, string degree)
    {
        fpspread.Sheets[0].RowCount = 0;
        InitSpread();
        fpspread.SaveChanges();
        string fopsql = "select * from CoCurr_Activitie where Batch_Year in ('" + batch + "') and Degree_Code in ('" + degree + "') ";
        string temptitlename = "";
        int rowwcount = 0;
        int spancount = 1;
        string spanrowsfp = "";
        Hashtable hat = new Hashtable();
        ArrayList arr = new ArrayList();
        string startrow = "";
        Boolean spantrue = false;
        DataSet dsfpful = new DataSet();
        dsfpful.Clear();
        dsfpful = da.select_method_wo_parameter(fopsql, "Text");

        int fptotcol = fpspread.Sheets[0].ColumnCount;
        int stcol = 3;
        if (chkPartname.Checked)
        {
            stcol = 4;
            fpspread.Sheets[0].SetColumnMerge(stcol - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //fptotcol = 10;
        }

        for (int col = 0; col < fptotcol; col++)
        {
            fpspread.Sheets[0].Columns[col].VerticalAlign = VerticalAlign.Middle;
        }

        //fpspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
        //fpspread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
        //fpspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
        //fpspread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
        //fpspread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
        //fpspread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
        //fpspread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
        //fpspread.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;

        if (dsfpful.Tables[0].Rows.Count > 0)
        {

            fpspread.Sheets[0].RowCount = dsfpful.Tables[0].Rows.Count + 1;
            fpspread.Sheets[0].Cells[0, 1].CellType = chkboxsel_all;
            fpspread.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Cells[0, 2].Locked = true;
            if (chkPartname.Checked)
            {
                fpspread.Sheets[0].Cells[0, stcol - 1].Locked = false;
                fpspread.Sheets[0].Cells[0, stcol - 1].CellType = new FarPoint.Web.Spread.TextCellType();
            }
            fpspread.Sheets[0].Cells[0, stcol].Locked = true;
            fpspread.Sheets[0].Cells[0, stcol + 1].Locked = true;
            fpspread.Sheets[0].Cells[0, stcol + 2].Locked = true;
            fpspread.Sheets[0].Cells[0, stcol + 3].Locked = true;
            fpspread.Sheets[0].Cells[0, stcol + 4].Locked = true;
            fpspread.Sheets[0].Cells[0, stcol + 5].Locked = true;

            fpspread.Visible = true;
            btnsubti.Visible = true;
            btnfinalsave.Visible = true;
            btndelete.Visible = true;
            fpspread.Height = 550;
            fpspread.Width = 975;
            if (chkPartname.Checked)
            {
                fpspread.Width = 985;
            }
            ddltitlename.Items.Clear();
            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'RTnam' and college_code = '" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            string strtit = "";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltitlename.DataSource = ds;
                ddltitlename.DataTextField = "TextVal";
                ddltitlename.DataValueField = "TextCode";
                ddltitlename.DataBind();

                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    if (strtit == "")
                //    {
                //        strtit = Convert.ToString(ds.Tables[0].Rows[i]["TextVal"]);
                //    }
                //    else
                //    {
                //        strtit = strtit + "-" + Convert.ToString(ds.Tables[0].Rows[i]["TextVal"]);
                //    }
                //}
                //string[] strcomo1 = strtit.Split('-');
                //combocol = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
                combocol.DataSource = ds;
                combocol.DataTextField = "TextVal";
                combocol.DataValueField = "TextCode";

                combocol.ShowButton = false;
                combocol.AutoPostBack = true;
                combocol.UseValue = true;

                for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
                {
                    for (int j = 0; j < fpspread.Sheets[0].ColumnCount; j++)
                    {
                        fpspread.Sheets[0].Cells[i, stcol + 1].CellType = combocol;
                    }
                }
            }
        }
        //else
        //{
        //    lblerrmsg.Text = "No Records Found";
        //    lblerrmsg.Visible = true;
        //}

        for (int i = 0; i < dsfpful.Tables[0].Rows.Count; i++)
        {
            fpspread.Sheets[0].Cells[i + 1, 1].CellType = chkboxcol;
            fpspread.Sheets[0].Cells[i + 1, 1].HorizontalAlign = HorizontalAlign.Center;
            // fpspread.Sheets[0].Cells[i, 8].CellType = chkboxcol;
            string cocurid = Convert.ToString(dsfpful.Tables[0].Rows[i]["CoCurr_ID"]);//CoCurr_ID
            string usrpartname = "";
            string partno = "";
            if (chkPartname.Checked)
            {
                partnamepartNo(dsfpful.Tables[0], i, cocurid, out usrpartname, out partno);
                fpspread.Sheets[0].Cells[i + 1, stcol - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                fpspread.Sheets[0].Cells[i + 1, stcol - 1].Text = usrpartname;
            }
            fpspread.Sheets[0].Cells[i + 1, stcol + 1].CellType = combocol;

            fpspread.Sheets[0].Cells[i + 1, stcol + 2].CellType = chkboxcol;
            fpspread.Sheets[0].Cells[i + 1, stcol + 2].HorizontalAlign = HorizontalAlign.Center;

            // fpspread.Sheets[0].Cells[i, 8].HorizontalAlign = HorizontalAlign.Center;

            fpspread.Sheets[0].Cells[i + 1, stcol + 3].CellType = chkboxcol;
            fpspread.Sheets[0].Cells[i + 1, stcol + 3].HorizontalAlign = HorizontalAlign.Center;

            fpspread.Sheets[0].Cells[i + 1, stcol + 4].CellType = chkboxcol;
            fpspread.Sheets[0].Cells[i + 1, stcol + 4].HorizontalAlign = HorizontalAlign.Center;

            fpspread.Sheets[0].Cells[i + 1, stcol + 5].CellType = chkboxcol;
            fpspread.Sheets[0].Cells[i + 1, stcol + 5].HorizontalAlign = HorizontalAlign.Center;

            string partname = Convert.ToString(dsfpful.Tables[0].Rows[i][1]);
            string[] spitpartno = partname.Split('-');

            if (Convert.ToString(dsfpful.Tables[0].Rows[i][5]).ToLower() == "true")
            {
                fpspread.Sheets[0].Cells[i + 1, stcol + 3].Value = 1;
            }
            else
            {
                fpspread.Sheets[0].Cells[i + 1, stcol + 3].Value = 0;
            }

            if (Convert.ToString(dsfpful.Tables[0].Rows[i][6]).ToLower() == "true")
            {
                fpspread.Sheets[0].Cells[i + 1, stcol + 4].Value = 1;
            }
            else
            {
                fpspread.Sheets[0].Cells[i + 1, stcol + 4].Value = 0;
            }
            if (Convert.ToString(dsfpful.Tables[0].Rows[i][7]).ToLower() == "true")
            {
                fpspread.Sheets[0].Cells[i + 1, stcol + 5].Value = 1;
            }
            else
            {
                fpspread.Sheets[0].Cells[i + 1, stcol + 5].Value = 0;
            }

            fpspread.Sheets[0].Cells[i + 1, stcol].Text = Convert.ToString(dsfpful.Tables[0].Rows[i][2]);
            fpspread.Sheets[0].Cells[i + 1, stcol + 1].Text = Convert.ToString(dsfpful.Tables[0].Rows[i][3]);
            fpspread.Sheets[0].Cells[i + 1, stcol].Tag = Convert.ToString(dsfpful.Tables[0].Rows[i][0]);
            //fpspread.Sheets[0].Cells[i, 5].Value =Convert.ToString( dsfpful.Tables[0].Rows[i][4]);
            if (Convert.ToString(dsfpful.Tables[0].Rows[i][4]).ToLower() == "true")
            {
                fpspread.Sheets[0].Cells[i + 1, stcol + 2].Value = 1;
            }
            else
            {
                fpspread.Sheets[0].Cells[i + 1, stcol + 2].Value = 0;
            }

            if (temptitlename != Convert.ToString(spitpartno[1]))
            {
                startrow = Convert.ToString(rowwcount - 1);
                rowwcount++;

                //fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = Convert.ToString(spitpartno[0]);

                temptitlename = Convert.ToString(spitpartno[1]);
            }
            else
            {
                spancount++;
                fpspread.Sheets[0].Cells[i + 1, 2].Text = temptitlename;
            }
            fpspread.Sheets[0].Cells[i + 1, 0].Text = Convert.ToString(rowwcount);
            fpspread.Sheets[0].Cells[i + 1, 2].Text = Convert.ToString(spitpartno[1]);
        }

        int rowsnewcount = 1;
        DataView dv_demand_data = new DataView();
        for (int i = 0; i < dsfpful.Tables[0].Rows.Count; i++)
        {
            if (!arr.Contains(Convert.ToString(dsfpful.Tables[0].Rows[i][1])))
            {
                dsfpful.Tables[0].DefaultView.RowFilter = "PartName='" + Convert.ToString(dsfpful.Tables[0].Rows[i][1]) + "'";

                dv_demand_data = dsfpful.Tables[0].DefaultView;
                int count4 = 0;
                count4 = dv_demand_data.Count;
                if (spanrowsfp == "")
                {
                    spanrowsfp = Convert.ToString(i) + "-" + Convert.ToString(count4);
                }
                else
                {
                    spanrowsfp = spanrowsfp + ";" + Convert.ToString(i) + "-" + Convert.ToString(count4);
                }
                arr.Add(Convert.ToString(dsfpful.Tables[0].Rows[i][1]));
            }
        }

        //string[] spiltspanrowsfp = spanrowsfp.Split(';');
        //if (spiltspanrowsfp.GetUpperBound(0) >= 0)
        //{
        //    for (int i = 0; i <= spiltspanrowsfp.GetUpperBound(0); i++)
        //    {
        //        string spanrow1 = spiltspanrowsfp[i].ToString();
        //        string[] spiltspanrow1 = spanrow1.Split('-');

        //        if (spiltspanrow1.GetUpperBound(0) > 0)
        //        {
        //            int rowcount = Convert.ToInt32(spiltspanrow1[0]);
        //            int totrowcount = Convert.ToInt32(spiltspanrow1[1]);
        //            string spancol = Convert.ToString(rowcount) + "-" + Convert.ToString(totrowcount);
        //            fpspread.Sheets[0].SpanModel.Add(rowcount, 0, totrowcount, 1);
        //            //fpspread.Sheets[0].SpanModel.Add(rowcount, 1, totrowcount, 1);
        //            fpspread.Sheets[0].SpanModel.Add(rowcount, 2, totrowcount, 1);
        //            //fpspread.Sheets[0].SpanModel.Add(rowcount, 5, totrowcount, 1);
        //            //fpspread.Sheets[0].SpanModel.Add(rowcount, 6, totrowcount, 1);
        //            //fpspread.Sheets[0].SpanModel.Add(rowcount, 7, totrowcount, 1);
        //            //fpspread.Sheets[0].SpanModel.Add(rowcount, 8, totrowcount, 1);
        //            fpspread.Sheets[0].Cells[rowcount, 0].Note = spancol;
        //        }

        //    }
        //}
        if (arr.Count > 0)
        {
            txt_totparts.Text = Convert.ToString(arr.Count);
            txt_partname.Text = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text);
        }

        fpspread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fpspread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        fpspread.SaveChanges();

        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
        if (fpspread.Sheets[0].RowCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void Chkbatsel_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkbatsel.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkbat.Items)
            {
                li.Selected = true;
                tbbat.Text = "Year(" + (Chkbat.Items.Count) + ")";

            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkbat.Items)
            {
                li.Selected = false;
                tbbat.Text = "- - Select - -";
            }
        }
        string collegecode = Convert.ToString(ddlcollege.SelectedValue);
        //BindDegree();
        //BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

    }

    protected void Chkbat_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        string value = "";
        string code = "";

        for (int i = 0; i < Chkbat.Items.Count; i++)
        {
            if (Chkbat.Items[i].Selected == true)
            {
                value = Chkbat.Items[i].Text;
                code = Convert.ToString(Chkbat.Items[i].Value);
                batchcount = batchcount + 1;
                tbbat.Text = "Year(" + Convert.ToString(batchcount) + ")";
            }
        }
        if (batchcount == 0)
        {
            tbbat.Text = "---Select---";
        }
    }

    protected void Chkdegsel_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkdegsel.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkdeg.Items)
            {
                li.Selected = true;
                tbdeg.Text = "Type(" + (Chkdeg.Items.Count) + ")";
            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in Chkdeg.Items)
            {
                li.Selected = false;
                tbdeg.Text = "- - Select - -";
            }
        }
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }

    protected void Chkdeg_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
        string value = "";
        string code = "";

        for (int i = 0; i < Chkdeg.Items.Count; i++)
        {
            if (Chkdeg.Items[i].Selected == true)
            {
                value = Chkdeg.Items[i].Text;
                code = Convert.ToString(Chkdeg.Items[i].Value);
                commcount = commcount + 1;
                tbdeg.Text = "Type(" + Convert.ToString(commcount) + ")";
            }
        }
        if (commcount == 0)
        {
            tbdeg.Text = "---Select---";
        }
        //for (int i = 0; i < Chkdeg.Items.Count; i++)
        //{
        //    Chkdeg.Items[i].Selected = true;
        //    if (Chkdeg.Items[i].Selected == true)
        //    {
        //        count2 += 1;
        //    }
        //    if (Chkdeg.Items.Count == count2)
        //    {
        //        Chkdegsel.Checked = true;
        //    }
        //}

        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        //  BindSectionDetail(strbatch, strbranch);
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbranch.Checked == true)
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                chklstbranch.Items[i].Selected = true;
                txtbranch.Text = "Standard(" + (chklstbranch.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                chklstbranch.Items[i].Selected = false;
                txtbranch.Text = "---Select---";
            }
        }
        // BindDegree(singleuser, group_user, collegecode, usercode);
        //BindSectionDetail(strbatch, strbranch);
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbranch.Focus();
        int branchcount = 0;
        string value = "";
        string code = "";
        lblFpNewErr.Visible = false;
        lblFpNewErr.Text = "";
        lblerrmsg.Text = "";
        lblerrmsg.Visible = false;
        fpspread.Visible = false;
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {

                value = chklstbranch.Items[i].Text;
                code = Convert.ToString(chklstbranch.Items[i].Value);
                branchcount = branchcount + 1;
                txtbranch.Text = "Standard(" + Convert.ToString(branchcount) + ")";
            }
        }

        string batch = "";
        string degree = "";
        string sem = "";
        int batchcount = 0;
        for (int i = 0; i < Chkbat.Items.Count; i++)
        {
            if (Chkbat.Items[i].Selected == true)
            {
                batchcount++;
                if (batch == "")
                {
                    batch = Convert.ToString(Chkbat.Items[i]);
                }
                else
                {
                    batch = batch + "','" + Convert.ToString(Chkbat.Items[i]);
                }
            }
        }
        //for (int i = 0; i < Chkdeg.Items.Count; i++)
        //{


        //    if (Chkdeg.Items[i].Selected == true)
        //    {
        //        if (degree == "")
        //        {
        //            degree = Chkdeg.Items[i].Value.ToString();
        //        }
        //        else
        //        {
        //            degree = degree + "','" + Chkdeg.Items[i].Value.ToString();
        //        }
        //    }
        //}
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                if (degree == "")
                {
                    degree = Convert.ToString(chklstbranch.Items[i].Value);
                }
                else
                {
                    degree = degree + "','" + Convert.ToString(chklstbranch.Items[i].Value);
                }
            }
        }
        lblerrmsg.Text = "";
        if (branchcount == 0)
        {
            lblerrmsg.Text = "Please Select Atleast One Standard";
            lblerrmsg.Visible = true;
            txtbranch.Text = "--Select--";
            txt_totparts.Text = "";
            txt_partname.Text = "";
            fpspread.Visible = false;
            btnsubti.Visible = false;
            btnfinalsave.Visible = false;
            btndelete.Visible = false;
            return;
        }
        else if (branchcount == 1)
        {
            if (batchcount == 1)
            {
                bindfp(batch, degree);
            }
            else
            {
                lblerrmsg.Text = "Please Only One Year";
                lblerrmsg.Visible = true;
                txt_totparts.Text = "";
                txt_partname.Text = "";
                fpspread.Visible = false;
                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;
                return;
            }
        }
        else
        {
            string fopsql = "select * from CoCurr_Activitie where Batch_Year in ('" + batch + "') and Degree_Code in ('" + degree + "')";
            ds.Clear();
            ds = da.select_method_wo_parameter(fopsql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblerrmsg.Text = "Please Select any one Standard to Proceed";
                lblerrmsg.Visible = true;
                txt_totparts.Text = "";
                txt_partname.Text = "";
                fpspread.Visible = false;
                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;
                return;
            }
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void titleminus_OnClick(object sener, EventArgs e)
    {
        try
        {
            // ------------------ start
            string strdset = "SELECT * FROM CoCurr_Activitie where Title_Name='" + ddltitlename.SelectedValue + "'";
            DataSet dsetdlt = da.select_method_wo_parameter(strdset, "text");
            // ------------------ end
            if (dsetdlt.Tables[0].Rows.Count > 0)
            {
                lblerrmsg2.Text = "This Title Name Already Used so Can't Be Deleted";
                lblerrmsg2.Visible = true;
            }
            else
            {
                string add = "delete from textvaltable where TextCode='" + ddltitlename.SelectedValue + "'and TextCriteria='RTnam' and  college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' ";
                int a = da.update_method_wo_parameter(add, "text");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('TitleName Deleted Successfully')", true);
                Titlename();
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btnset_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            lblFpNewErr.Text = "";
            lblFpNewErr.Visible = false;
            string setno = "";
            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'RTnam' and college_code = '" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            fpspread.Height = 550;
            fpspread.Width = 975;

            string strtit = "";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    if (strtit == "")
                //    {
                //        strtit = ds.Tables[0].Rows[i]["TextVal"].ToString();
                //    }
                //    else
                //    {
                //        strtit = strtit + "-" + ds.Tables[0].Rows[i]["TextVal"].ToString();
                //    }
                //}
                combocol.DataSource = ds;
                combocol.DataTextField = "TextVal";
                combocol.DataValueField = "TextCode";

                combocol.ShowButton = false;

                combocol.AutoPostBack = true;
                combocol.UseValue = true;
            }
            //string[] strcomo1 = strtit.Split('-');
            //combocol = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
            //combocol.ShowButton = true;

            //combocol.AutoPostBack = true;
            //combocol.UseValue = true;


            //string querygrade = "select distinct Mark_Grade from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='0'";
            //string strtitgrade = "";
            //ds.Clear();


            //ds = da.select_method_wo_parameter(querygrade, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        if (strtitgrade == "")
            //        {
            //            strtitgrade = ds.Tables[0].Rows[i]["Mark_Grade"].ToString();
            //        }
            //        else
            //        {
            //            strtitgrade = strtitgrade + "-" + ds.Tables[0].Rows[i]["Mark_Grade"].ToString();
            //        }
            //    }
            //}
            //string[] strcomo1grade = strtitgrade.Split('-');
            //combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1grade);
            //combocolgrade.ShowButton = true;

            //combocolgrade.AutoPostBack = true;
            //combocolgrade.UseValue = true;

            DataTable dt = new DataTable();
            ArrayList addarray = new ArrayList();
            dt.Columns.Add("");
            dt.Columns.Add("");
            if (chkPartname.Checked)
            {
                dt.Columns.Add("");
            }
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            DataRow dr = null;
            fpspread.SaveChanges();
            int stcol = 3;
            if (chkPartname.Checked)
            {
                stcol = 4;
            }
            //Panel1addsub.Visible = false;
            if (fpspread.Sheets[0].RowCount > 0)
            {
                for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
                {
                    if (fpspread.Sheets[0].Cells[i, stcol].Text.Trim() == "")
                    {
                        string sdr = Convert.ToString(fpspread.Sheets[0].Cells[i, 1].Value);
                        if (sdr == "")
                        {
                            sdr = "0";
                        }
                        int isval = Convert.ToInt32(sdr);
                        if (isval == 1)
                        {
                            addarray.Add(i);
                        }
                    }
                }
                for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
                {
                    if (addarray.Count == 0)
                    {
                        //lblerrmsg.Text = "";
                        txttotnosubt.Text = "";
                        Panel1addsub.Visible = false;
                        return;
                    }

                    dr = dt.NewRow();
                    dr[0] = Convert.ToString(fpspread.Sheets[0].Cells[i, 0].Text);
                    dr[1] = Convert.ToString(fpspread.Sheets[0].Cells[i, 1].Value);
                    dr[2] = Convert.ToString(fpspread.Sheets[0].Cells[i, 2].Text);
                    if (chkPartname.Checked)
                    {
                        dr[stcol - 1] = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol - 1].Text);
                    }
                    dr[stcol] = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol].Text);
                    dr[stcol + 1] = Convert.ToString(fpspread.Sheets[0].GetValue(i, stcol + 1));
                    dr[stcol + 2] = Convert.ToString(fpspread.Sheets[0].GetValue(i, stcol + 2));
                    dr[stcol + 3] = Convert.ToString(fpspread.Sheets[0].GetValue(i, stcol + 3));
                    dr[stcol + 4] = Convert.ToString(fpspread.Sheets[0].GetValue(i, stcol + 4));
                    dr[stcol + 5] = Convert.ToString(fpspread.Sheets[0].GetValue(i, stcol + 2));
                    dr[stcol + 6] = Convert.ToString(fpspread.Sheets[0].Cells[i, 0].Note);
                    dt.Rows.Add(dr);

                }
                if (dt.Rows.Count > 0)
                {
                    int txtvalue = 0;
                    txtvalue = Convert.ToInt32(txttotnosubt.Text);
                    fpspread.Sheets[0].RowCount = 1;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = chkboxsel_all;
                    int rowsccsno = 0;
                    string userPartName = "";
                    string selpart = "";
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        fpspread.Sheets[0].RowCount++;
                        //rowsccsno++;
                        if (Convert.ToInt16(fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Value) == 1)
                        {
                            selpart = Convert.ToString(dt.Rows[row][0]);
                        }
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dt.Rows[row][0]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Value = Convert.ToString(dt.Rows[row][1]);

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dt.Rows[row][0]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        // fpspread.Sheets[0].Cells[i, 8].CellType = chkboxcol;
                        if (chkPartname.Checked)
                        {
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].CellType = new FarPoint.Web.Spread.TextCellType();

                            if (fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text.ToString() != fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 2].Text.ToString() || row == 0)
                            {
                                userPartName = Convert.ToString(fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text);
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text = ((Convert.ToString(dt.Rows[row][stcol - 1]) != "") ? Convert.ToString(dt.Rows[row][stcol - 1]) : userPartName);
                            }
                            else
                            {
                                userPartName = userPartName;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text = userPartName;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text = ((Convert.ToString(dt.Rows[row][stcol - 1]) != "") ? Convert.ToString(dt.Rows[row][stcol - 1]) : userPartName);
                            }

                            fpspread.Sheets[0].SetColumnMerge(stcol - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].HorizontalAlign = HorizontalAlign.Left;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].VerticalAlign = VerticalAlign.Middle;
                            //if (selpart == fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text )
                            //{

                            //}
                        }

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Tag = "no";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(dt.Rows[row][stcol]);

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 1].CellType = combocol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 1].Value = Convert.ToString(dt.Rows[row][stcol + 1]);

                        string value5 = Convert.ToString(dt.Rows[row][stcol + 2]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].Value = Convert.ToString(dt.Rows[row][stcol + 2]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].VerticalAlign = VerticalAlign.Middle;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].Value = Convert.ToString(dt.Rows[row][stcol + 3]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].VerticalAlign = VerticalAlign.Middle;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].Value = Convert.ToString(dt.Rows[row][stcol + 4]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].VerticalAlign = VerticalAlign.Middle;
                        // fpspread.Sheets[0].Cells[i, 8].HorizontalAlign = HorizontalAlign.Center;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].Text = Convert.ToString(dt.Rows[row][stcol + 5]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].VerticalAlign = VerticalAlign.Middle;

                        if (Convert.ToString(dt.Rows[row][0]).Trim() != "" && Convert.ToString(dt.Rows[row][0]).Trim() != null)
                        {
                            setno = Convert.ToString(dt.Rows[row][0]);
                            //fpspread.Sheets[0].Cells[row, 0].Note = dt.Rows[row][9].ToString();
                            string spancol = Convert.ToString(dt.Rows[row][stcol + 6]);

                            if (Convert.ToString(spancol.Trim()) != "" && Convert.ToString(spancol.Trim()) != null)
                            {
                                string[] spitbothrowcount = spancol.Split('-');
                                if (spitbothrowcount.GetUpperBound(0) > 0)
                                {
                                    string rowcount = Convert.ToString(fpspread.Sheets[0].RowCount - 1);
                                    string totrowcount = Convert.ToString(spitbothrowcount[1].ToString());
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Note = rowcount + "-" + totrowcount;
                                }
                            }
                        }

                        if (addarray.Contains(row + 1))
                        {
                            //if (fpspread.Sheets[0].Cells[row, 3].Text.Trim() == "")
                            //{
                            int lastspanrowcount = fpspread.Sheets[0].RowCount - 1;
                            //fpspread.Sheets[0].RowCount += txtvalue;
                            //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, txtvalue, 1);
                            int autochar = 97;
                            for (int k = 0; k < txtvalue; k++)
                            {
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dt.Rows[row][0]);
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = chkboxcol;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dt.Rows[row][0]);
                                if (chkPartname.Checked)
                                {
                                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text = Convert.ToString(dt.Rows[row][stcol - 1]);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text = ((Convert.ToString(dt.Rows[row][stcol - 1]) != "") ? Convert.ToString(dt.Rows[row][stcol - 1]) : "Part-" + Convert.ToString(dt.Rows[row][0]));
                                    if (fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text.ToString() != fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 2].Text.ToString() || row == 0)
                                    {
                                        userPartName = Convert.ToString(fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text);
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text = ((Convert.ToString(dt.Rows[row][stcol - 1]) != "") ? Convert.ToString(dt.Rows[row][stcol - 1]) : userPartName);
                                    }
                                    else
                                    {
                                        userPartName = userPartName;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text = userPartName;
                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text = ((Convert.ToString(dt.Rows[row][stcol - 1]) != "") ? Convert.ToString(dt.Rows[row][stcol - 1]) : userPartName);
                                    }
                                    fpspread.Sheets[0].SetColumnMerge(stcol - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].HorizontalAlign = HorizontalAlign.Left;
                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].VerticalAlign = VerticalAlign.Middle;
                                    //if (Convert.ToInt16(fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Value) == 1 && row < txtvalue - 1)
                                    //    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, stcol - 1, txtvalue, 1);
                                }

                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Tag = "no";

                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 1].CellType = combocol;

                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].CellType = chkboxcol;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].VerticalAlign = VerticalAlign.Middle;


                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].CellType = chkboxcol;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].VerticalAlign = VerticalAlign.Middle;


                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].CellType = chkboxcol;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].VerticalAlign = VerticalAlign.Middle;

                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].CellType = chkboxcol;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].VerticalAlign = VerticalAlign.Middle;

                                //Added By Malang Raja

                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + (char)(autochar + k));

                                //---------------------------------------Start Commented By Malang Raja-------------------------------------------

                                //if (k == 0)
                                //{
                                //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + "a");
                                //}
                                //if (k == 1)
                                //{
                                //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + "b");
                                //}
                                //if (k == 2)
                                //{
                                //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + "c");

                                //}
                                //if (k == 3)
                                //{
                                //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + "d");
                                //}
                                //if (k == 4)
                                //{
                                //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + "e");
                                //}
                                //if (k == 5)
                                //{
                                //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + "f");
                                //}
                                //if (k == 6)
                                //{
                                //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + "g");
                                //}
                                //if (k == 7)
                                //{
                                //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + "h");
                                //}
                                //if (k == 8)
                                //{
                                //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + "i");
                                //}
                                //if (k == 9)
                                //{
                                //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(setno + "j");
                                //}

                                //---------------------------------------END Commented By Malang Raja-------------------------------------------

                                if (k != txtvalue - 1)
                                {
                                    fpspread.Sheets[0].RowCount++;
                                }
                            }
                            ////fpspread.Sheets[0].SpanModel.Add(Convert.ToInt32(setno), 0, txtvalue, 1);
                            //fpspread.Sheets[0].SpanModel.Add(lastspanrowcount, 0, txtvalue, 1);
                            //fpspread.Sheets[0].SpanModel.Add(lastspanrowcount, 1, txtvalue, 1);
                            //fpspread.Sheets[0].SpanModel.Add(lastspanrowcount, 2, txtvalue, 1);
                            //fpspread.Sheets[0].SpanModel.Add(lastspanrowcount, 5, txtvalue, 1);
                            //fpspread.Sheets[0].SpanModel.Add(lastspanrowcount, 6, txtvalue, 1);
                            //fpspread.Sheets[0].SpanModel.Add(lastspanrowcount, 7, txtvalue, 1);
                            //fpspread.Sheets[0].SpanModel.Add(lastspanrowcount, 8, txtvalue, 1);
                            string bothrow_count = Convert.ToString(lastspanrowcount) + "-" + Convert.ToString(txtvalue);
                            fpspread.Sheets[0].Cells[lastspanrowcount, 0].Note = bothrow_count;
                            //}
                        }
                    }
                    fpspread.SaveChanges();
                    fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                }

                txttotnosubt.Text = "";

                fpspread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                fpspread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Panel1addsub.Visible = false;

                //for (int row = 0; row < fpspread.Sheets[0].RowCount; row++)
                //{
                //    string spancol = fpspread.Sheets[0].Cells[row, 0].Note.ToString();
                //    if (spancol.Trim().ToString() != "" && spancol.Trim().ToString() != null)
                //    {
                //        string[] spitbothrowcount = spancol.Split('-');
                //        if (spitbothrowcount.GetUpperBound(0) > 0)
                //        {
                //            int rowcount = Convert.ToInt32(spitbothrowcount[0].ToString());
                //            int totrowcount = Convert.ToInt32(spitbothrowcount[1].ToString());
                //            fpspread.Sheets[0].SpanModel.Add(rowcount, 0, totrowcount, 1);
                //            //fpspread.Sheets[0].SpanModel.Add(rowcount, 1, totrowcount, 1);
                //            fpspread.Sheets[0].SpanModel.Add(rowcount, 2, totrowcount, 1);
                //            //fpspread.Sheets[0].SpanModel.Add(rowcount, 5, totrowcount, 1);
                //            //fpspread.Sheets[0].SpanModel.Add(rowcount, 6, totrowcount, 1);
                //            //fpspread.Sheets[0].SpanModel.Add(rowcount, 7, totrowcount, 1);
                //            //fpspread.Sheets[0].SpanModel.Add(rowcount, 8, totrowcount, 1);
                //            fpspread.Sheets[0].Cells[rowcount, 0].Note = spancol;
                //        }
                //    }
                //}

            }

        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btndelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            lblFpNewErr.Text = "";
            lblFpNewErr.Visible = false;
            int deleterow_tabcount = 0;
            fpspread.SaveChanges();
            string chkCoCurr_ID = "";
            int stcol = 3;
            if (chkPartname.Checked)
            {
                stcol = 4;
            }
            int count = 0;
            for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
            {
                string sdrchkCoCurr_ID = Convert.ToString(fpspread.Sheets[0].Cells[i, 1].Value);

                if (sdrchkCoCurr_ID == "" || sdrchkCoCurr_ID == "False" || sdrchkCoCurr_ID == "0")
                {
                    sdrchkCoCurr_ID = "0";
                }

                if (sdrchkCoCurr_ID == "True" || sdrchkCoCurr_ID == "1")
                {
                    count++;
                    sdrchkCoCurr_ID = "1";
                }

                int isvalsdrchkCoCurr_ID = Convert.ToInt32(sdrchkCoCurr_ID);
                if (isvalsdrchkCoCurr_ID == 1)
                {
                    if (chkCoCurr_ID == "")
                    {
                        if (fpspread.Sheets[0].Cells[i, stcol].Tag.ToString() != "no")
                        {
                            chkCoCurr_ID = fpspread.Sheets[0].Cells[i, stcol].Tag.ToString();
                        }
                        //if (fpspread.Sheets[0].Cells[i, 3].Tag.ToString() == "null")
                        //{
                        //    chkCoCurr_ID = Convert.ToString(i);
                        //}

                    }
                    else
                    {
                        if (fpspread.Sheets[0].Cells[i, stcol].Tag.ToString() != "no")
                        {
                            chkCoCurr_ID = chkCoCurr_ID + "','" + fpspread.Sheets[0].Cells[i, stcol].Tag.ToString();
                        }
                        //if (fpspread.Sheets[0].Cells[i, 3].Tag.ToString() == "null")
                        //{
                        //    chkCoCurr_ID = chkCoCurr_ID + "','" + Convert.ToString(i);
                        //}
                    }

                }
            }
            if (count == 0 || chkCoCurr_ID.Trim() == "")
            {
                lblFpNewErr.Text = "Please Select Any One Record To Delete";
                lblFpNewErr.Visible = true;
                return;
            }

            if (chkCoCurr_ID.Trim() != "")
            {
                chkCoCurr_ID = " where CoCurr_ID in ('" + chkCoCurr_ID + "')";
            }
            if (chkCoCurr_ID != "")
            {
                string beforedelete = " select * from activity_gd where ActivityTextVal in ( select ActivityTextVal from activity_entry " + chkCoCurr_ID + ") ";
                DataSet dsbeforedelete = new DataSet();
                dsbeforedelete.Clear();
                dsbeforedelete = da.select_method_wo_parameter(beforedelete, "Text");
                if (dsbeforedelete.Tables[0].Rows.Count > 0)
                {
                    deleterow_tabcount = dsbeforedelete.Tables[0].Rows.Count;
                    //string dlet = "delete from CoCurr_Activitie " + chkCoCurr_ID + "";
                }
                if (deleterow_tabcount > 0)
                {
                    lblFpNewErr.Text = "Already Entered . So please Delete the Record and then Proceed";
                    lblFpNewErr.Visible = true;
                    return;
                }
            }

            string setno = "";
            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'RTnam' and college_code = '" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            fpspread.Height = 550;
            fpspread.Width = 975;
            if (chkPartname.Checked)
            {
                fpspread.Width = 985;
            }

            string strtit = "";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    if (strtit == "")
                //    {
                //        strtit = ds.Tables[0].Rows[i]["TextVal"].ToString();
                //    }
                //    else
                //    {
                //        strtit = strtit + "-" + ds.Tables[0].Rows[i]["TextVal"].ToString();
                //    }
                //}
                combocol.DataSource = ds;
                combocol.DataTextField = "TextVal";
                combocol.DataValueField = "TextCode";
                combocol.ShowButton = false;
                combocol.AutoPostBack = true;
                combocol.UseValue = true;
            }
            //string[] strcomo1 = strtit.Split('-');
            //combocol = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
            //combocol.ShowButton = true;

            //combocol.AutoPostBack = true;
            //combocol.UseValue = true;


            //string querygrade = "select distinct Mark_Grade from Grade_Master where College_Code='" + Session["collegecode"].ToString() + "'and semester='0'";
            //string strtitgrade = "";
            //ds.Clear();


            //ds = da.select_method_wo_parameter(querygrade, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        if (strtitgrade == "")
            //        {
            //            strtitgrade = ds.Tables[0].Rows[i]["Mark_Grade"].ToString();
            //        }
            //        else
            //        {
            //            strtitgrade = strtitgrade + "-" + ds.Tables[0].Rows[i]["Mark_Grade"].ToString();
            //        }
            //    }
            //}
            //string[] strcomo1grade = strtitgrade.Split('-');
            //combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1grade);
            //combocolgrade.ShowButton = true;

            //combocolgrade.AutoPostBack = true;
            //combocolgrade.UseValue = true;

            DataTable dt = new DataTable();
            ArrayList addarray = new ArrayList();
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            if (chkPartname.Checked)
            {
                fpspread.Width = 985;
                dt.Columns.Add("");
            }
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");
            DataRow dr = null;
            fpspread.SaveChanges();
            Panel1addsub.Visible = false;
            if (fpspread.Sheets[0].RowCount > 0)
            {
                for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
                {
                    string sdr = Convert.ToString(fpspread.Sheets[0].Cells[i, 1].Value);

                    if (sdr == "")
                    {
                        sdr = "0";
                    }
                    int isval = Convert.ToInt32(sdr);
                    if (isval == 1)
                    {
                        addarray.Add(i);
                    }
                }
                int ccrow = 0;
                for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
                {
                    if (!addarray.Contains(i))
                    {

                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(fpspread.Sheets[0].Cells[i, 0].Text);
                        dr[1] = Convert.ToString(fpspread.Sheets[0].Cells[i, 1].Value);
                        dr[2] = Convert.ToString(fpspread.Sheets[0].Cells[i, 2].Text);
                        if (chkPartname.Checked)
                        {
                            dr[stcol - 1] = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol - 1].Text);
                        }
                        dr[stcol] = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol].Text);
                        dr[stcol + 1] = Convert.ToString(fpspread.Sheets[0].GetValue(i, stcol + 1).ToString());
                        dr[stcol + 2] = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol + 2].Value);
                        dr[stcol + 3] = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol + 3].Value);
                        dr[stcol + 4] = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol + 4].Value);
                        dr[stcol + 5] = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol + 5].Text);
                        dr[stcol + 7] = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol].Tag);
                        string spancol = fpspread.Sheets[0].Cells[i, 0].Note.ToString();
                        if (spancol.Trim().ToString() != "" && spancol.Trim().ToString() != null)
                        {
                            string[] spitbothrowcount = spancol.Split('-');
                            if (spitbothrowcount.GetUpperBound(0) > 0)
                            {
                                string rowcount = Convert.ToString(ccrow);
                                string totrowcount = Convert.ToString(spitbothrowcount[1].ToString());
                                dr[stcol + 6] = rowcount + "-" + totrowcount;
                            }
                        }
                        dt.Rows.Add(dr);
                        ccrow++;
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    int rowsno = 0;
                    fpspread.Sheets[0].RowCount = 1;
                    fpspread.Sheets[0].Cells[0, 1].CellType = chkboxsel_all;
                    fpspread.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
                    int rowsccsno = 0;
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        fpspread.Sheets[0].RowCount++;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dt.Rows[row][stcol + 6]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dt.Rows[row][0]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Value = Convert.ToString(dt.Rows[row][1]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dt.Rows[row][2]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        if (chkPartname.Checked)
                        {
                            fpspread.Sheets[0].SetColumnMerge(stcol - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].Text = Convert.ToString(dt.Rows[row][stcol - 1]);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol - 1].VerticalAlign = VerticalAlign.Middle;
                        }

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Text = Convert.ToString(dt.Rows[row][stcol]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol].Tag = Convert.ToString(dt.Rows[row][stcol + 7]);

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 1].CellType = combocol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 1].Value = Convert.ToString(dt.Rows[row][stcol + 1]);

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].Value = Convert.ToString(dt.Rows[row][stcol + 2]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 2].VerticalAlign = VerticalAlign.Middle;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].Value = Convert.ToString(dt.Rows[row][stcol + 3]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 3].VerticalAlign = VerticalAlign.Middle;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].Value = Convert.ToString(dt.Rows[row][stcol + 4]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 4].VerticalAlign = VerticalAlign.Middle;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].Text = Convert.ToString(dt.Rows[row][stcol + 5]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, stcol + 5].VerticalAlign = VerticalAlign.Middle;
                        fpspread.SaveChanges();
                    }
                    btnfinalsave_OnClick(sender, e);
                    fpspread.SaveChanges();
                    fpspread.Visible = true;
                    fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                }
                else
                {
                    fpspread.Sheets[0].RowCount = 0;
                    fpspread.SaveChanges();
                    fpspread.Visible = true;
                    fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                }
                fpspread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                fpspread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btnfinalsave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string batch = "";
            lblerrmsg.Text = "";
            //lblerrmsg2.Text = "";
            //lblerrmsg2.Visible = false;
            lblFpNewErr.Text = "";
            lblerrmsg.Visible = false;
            lblFpNewErr.Visible = false;
            string degree = "";
            string sem = "";

            for (int i = 0; i < Chkbat.Items.Count; i++)
            {
                if (Chkbat.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(Chkbat.Items[i]);
                    }
                    else
                    {
                        batch = batch + "','" + Convert.ToString(Chkbat.Items[i]);
                    }

                }
            }

            //for (int i = 0; i < Chkdeg.Items.Count; i++)
            //{


            //    if (Chkdeg.Items[i].Selected == true)
            //    {
            //        if (degree == "")
            //        {
            //            degree = Chkdeg.Items[i].Value.ToString();
            //        }
            //        else
            //        {
            //            degree = degree + "','" + Chkdeg.Items[i].Value.ToString();
            //        }
            //    }

            //}
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = chklstbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        degree = degree + "','" + chklstbranch.Items[i].Value.ToString();
                    }
                }
            }

            string co_ids = "";
            int deleterow_tabcount = 0;
            fpspread.SaveChanges();
            string beforedelete = " select * from  CoCurrActivitie_Det where Batch_Year in ('" + batch + "') and Degree_Code in ('" + degree + "') and ActivityTextVal is not null and (istype is null  or (istype<>'Att' and istype<>'Remks'))";
            DataSet dsbeforedelete = new DataSet();
            dsbeforedelete.Clear();
            dsbeforedelete = da.select_method_wo_parameter(beforedelete, "Text");
            if (dsbeforedelete.Tables[0].Rows.Count > 0)
            {
                deleterow_tabcount = dsbeforedelete.Tables[0].Rows.Count;
            }
            if (deleterow_tabcount > 0)
            {
                lblFpNewErr.Text = "Already Entered . So Please Delete the Record and then Proceed ";
                lblFpNewErr.Visible = true;
                return;
            }
            int stcol = 3;
            if (chkPartname.Checked)
            {
                stcol = 4;
            }
            for (int i = 1; i < fpspread.Sheets[0].Rows.Count; i++)
            {
                if (fpspread.Sheets[0].Cells[i, stcol].Text.ToString().Trim() == "" || fpspread.Sheets[0].Cells[i, stcol].Text.ToString().Trim() == null)
                {
                    lblFpNewErr.Text = "Please Fill All Sub Title";
                    lblFpNewErr.Visible = true;
                    return;
                }
                // fpspread.Sheets[0].Cells[i, 3].Tag
                if (deleterow_tabcount > 0)
                {
                    if (co_ids == "")
                    {
                        co_ids = fpspread.Sheets[0].Cells[i, stcol].Tag.ToString();
                    }
                    else
                    {
                        co_ids = co_ids + "," + fpspread.Sheets[0].Cells[i, stcol].Tag.ToString();
                    }
                }
            }

            if (deleterow_tabcount > 0)
            {
                beforedelete = " select * from  CoCurrActivitie_Det where CoCurr_ID in ('" + co_ids + "')";
                dsbeforedelete.Clear();
                dsbeforedelete = da.select_method_wo_parameter(beforedelete, "Text");
                if (dsbeforedelete.Tables[0].Rows.Count > 0)
                {
                    lblFpNewErr.Text = "Can't delete referanced data ";
                    lblFpNewErr.Visible = true;
                    return;
                }
                else
                {
                    lblFpNewErr.Text = "";
                    lblFpNewErr.Visible = false;
                }
            }

            //string deletesql = "drop table CoCurr_Activitie;Create Table CoCurr_Activitie(CoCurr_ID numeric identity(1,1),PartName nvarchar(100),SubTitle nvarchar(10),Title_Name numeric,IsDirectEntry Bit,IsActivity Bit,IsActDesc Bit,IsGrade Bit,Degree_Code numeric,Batch_Year numeric)";
            string deletesql = "delete from CoCurr_Activitie where Batch_Year in ('" + batch + "') and Degree_Code in ('" + degree + "')";
            int a = da.update_method_wo_parameter(deletesql, "Text");
            string partNo = "";
            string userPartName = "";
            string partname = "";
            string SubTitle = "";
            string Title_Name = "";
            string IsDirectEntry = "";
            string IsActivity = "";
            string IsActDesc = "";
            string IsGrade = "";
            string temppartname = "";
            Boolean istrue = false;

            string tempIsDirectEntry = "";
            string tempIsActivity = "";
            string tempIsActDesc = "";
            string tempIsGrade = "";
            bool isresult = false;
            stcol = 3;
            if (chkPartname.Checked)
            {
                stcol = 4;
            }

            for (int i = 1; i < fpspread.Sheets[0].Rows.Count; i++)
            {
                istrue = false;

                IsDirectEntry = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol + 2].Value);
                IsActivity = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol + 3].Value);
                IsActDesc = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol + 4].Value);
                IsGrade = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol + 5].Value);
                if (IsDirectEntry.Trim() == "")
                {
                    IsDirectEntry = "0";
                }
                if (IsActivity.Trim() == "")
                {
                    IsActivity = "0";
                }
                if (IsActDesc.Trim() == "")
                {
                    IsActDesc = "0";
                }
                if (IsGrade.Trim() == "")
                {
                    IsGrade = "0";
                }

                if (IsDirectEntry.Trim().ToLower() == "true")
                {
                    IsDirectEntry = "1";
                }
                else if (IsDirectEntry.Trim().ToLower() == "false")
                {
                    IsDirectEntry = "0";
                }

                if (IsActivity.Trim().ToLower() == "true")
                {
                    IsActivity = "1";
                }
                else if (IsActivity.Trim().ToLower() == "false")
                {
                    IsActivity = "0";
                }

                if (IsActDesc.Trim().ToLower() == "true")
                {
                    IsActDesc = "1";
                }
                else if (IsActDesc.Trim().ToLower() == "false")
                {
                    IsActDesc = "0";
                }

                if (IsGrade.Trim().ToLower() == "true")
                {
                    IsGrade = "1";
                }
                else if (IsGrade.Trim().ToLower() == "false")
                {
                    IsGrade = "0";
                }

                tempIsDirectEntry = IsDirectEntry;
                tempIsActivity = IsActivity;
                tempIsActDesc = IsActDesc;
                tempIsGrade = IsGrade;

                if (fpspread.Sheets[0].Cells[i, 0].Text.ToString().Trim() != "" && fpspread.Sheets[0].Cells[i, 0].Text.ToString().Trim() != null)
                {
                    istrue = false;
                    partname = fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text.ToString() + "-" + fpspread.Sheets[0].Cells[i, 2].Text.ToString();
                    if (chkPartname.Checked)
                    {
                        if (fpspread.Sheets[0].Cells[i, 2].Text.ToString() != fpspread.Sheets[0].Cells[i - 1, 2].Text.ToString() || i == 1)
                            userPartName = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol - 1].Text);
                        else
                        {
                            userPartName = userPartName;
                            fpspread.Sheets[0].Cells[i, stcol - 1].Text = userPartName;
                        }
                        partNo = Convert.ToString(fpspread.Sheets[0].Cells[i, 2].Text);//fpspread.Sheets[0].Cells[i, 0].Text);
                    }
                    else
                    {
                        userPartName = Convert.ToString(partname);
                        partNo = Convert.ToString(fpspread.Sheets[0].Cells[i, 2].Text);
                    }
                }
                else
                {
                    temppartname = partname;
                    if (chkPartname.Checked)
                    {
                        if (fpspread.Sheets[0].Cells[i, 2].Text.ToString() != fpspread.Sheets[0].Cells[i - 1, 2].Text.ToString() || i == 1)
                        {
                            userPartName = Convert.ToString(fpspread.Sheets[0].Cells[i, stcol - 1].Text);
                        }
                        else
                        {
                            userPartName = userPartName;
                            fpspread.Sheets[0].Cells[i, stcol - 1].Text = userPartName;
                        }
                        partNo = Convert.ToString(fpspread.Sheets[0].Cells[i, 2].Text);//fpspread.Sheets[0].Cells[i, 0].Text);
                    }
                    else
                    {
                        userPartName = Convert.ToString(temppartname);
                        partNo = Convert.ToString(fpspread.Sheets[0].Cells[i, 2].Text);
                    }
                }

                SubTitle = fpspread.Sheets[0].Cells[i, stcol].Text.ToString();
                Title_Name = fpspread.Sheets[0].GetValue(i, stcol + 1).ToString();

                //for (int l = 0; l < Chkdeg.Items.Count; l++ )
                //{
                //    if (Chkdeg.Items[l].Selected==true)
                //    {
                for (int m = 0; m < Chkbat.Items.Count; m++)
                {
                    if (Chkbat.Items[m].Selected == true)
                    {
                        for (int n = 0; n < chklstbranch.Items.Count; n++)
                        {
                            if (chklstbranch.Items[n].Selected == true)
                            {
                                if (istrue == true)
                                {
                                    string insertsql = "insert into CoCurr_Activitie(PartName,SubTitle,Title_Name,IsDirectEntry,IsActivity,IsActDesc,IsGrade,Degree_Code,Batch_Year,Part_No,UserPartName) values ('" + temppartname + "','" + SubTitle + "','" + Title_Name + "'," + tempIsDirectEntry + "," + tempIsActivity + "," + tempIsActDesc + "," + tempIsGrade + "," + chklstbranch.Items[n].Value.ToString() + "," + Chkbat.Items[m].ToString() + ",'" + partNo + "','" + userPartName + "')";
                                    a = da.update_method_wo_parameter(insertsql, "Text");
                                }
                                else
                                {
                                    string insertsql = "insert into CoCurr_Activitie(PartName,SubTitle,Title_Name,IsDirectEntry,IsActivity,IsActDesc,IsGrade,Degree_Code,Batch_Year,Part_No,UserPartName) values ('" + partname + "','" + SubTitle + "','" + Title_Name + "'," + IsDirectEntry + "," + IsActivity + "," + IsActDesc + "," + IsGrade + "," + chklstbranch.Items[n].Value.ToString() + "," + Chkbat.Items[m].Value.ToString() + ",'" + partNo + "','" + userPartName + "')";
                                    a = da.update_method_wo_parameter(insertsql, "Text");
                                }
                                if (a > 0)
                                {
                                    isresult = true;
                                }
                            }
                        }
                    }
                }
            }
            if (isresult)
            {
                btn_go_OnClick(sender, e);
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record(s) Not Saved')", true);
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btnsubti_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            lblFpNewErr.Text = "";
            lblFpNewErr.Visible = false;
            txttotnosubt.Text = "";
            if (fpspread.Sheets[0].RowCount > 0)
            {
                int selcount = 0;
                for (int r = 0; r < fpspread.Sheets[0].RowCount; r++)
                {
                    int val = 0;
                    int.TryParse(Convert.ToString(fpspread.Sheets[0].Cells[r, 1].Value), out val);
                    if (val == 1)
                    {
                        selcount++;
                    }
                }
                if (selcount == 0)
                {
                    lblFpNewErr.Text = "Please Select One Record.";
                    lblFpNewErr.Visible = true;
                }
                else if (selcount == 1)
                {
                    Panel1addsub.Visible = true;
                }
                else
                {
                    lblFpNewErr.Text = "Please Select Only One Record.";
                    lblFpNewErr.Visible = true;
                }
            }
            else
            {
                lblFpNewErr.Text = "No Record(s) Found.";
                lblFpNewErr.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btnsetexit_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            Panel1addsub.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btn_go_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            fpspread.Visible = false;
            lblFpNewErr.Text = "";
            lblFpNewErr.Visible = false;
            string collegecode = "";
            int checkselcount = 0;
            int branchcount = 0;
            int batchcount = 0;
            string batch = "";
            string degree = "";
            if (ddlcollege.Items.Count == 0)
            {
                lblerrmsg.Text = "There Is No School Found";
                lblerrmsg.Visible = true;
                fpspread.Visible = false;
                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddlcollege.SelectedValue);
            }
            if (Chkbat.Items.Count == 0)
            {
                lblerrmsg.Text = "There Is No Year Found";
                lblerrmsg.Visible = true;
                fpspread.Visible = false;
                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;
                return;
            }
            for (int i = 0; i < Chkbat.Items.Count; i++)
            {
                if (Chkbat.Items[i].Selected == true)
                {
                    checkselcount++;
                    batchcount++;
                    if (batch == "")
                    {
                        batch = Convert.ToString(Chkbat.Items[i]);
                    }
                    else
                    {
                        batch = batch + "','" + Convert.ToString(Chkbat.Items[i]);
                    }
                }
            }
            if (checkselcount == 0)
            {
                lblerrmsg.Text = "Please Select Any One Year";
                lblerrmsg.Visible = true;
                fpspread.Visible = false;
                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;
                return;
            }

            checkselcount = 0;
            if (Chkdeg.Items.Count == 0)
            {
                lblerrmsg.Text = "There Is No School Type Found";
                lblerrmsg.Visible = true;
                fpspread.Visible = false;
                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;
                return;
            }
            for (int i = 0; i < Chkdeg.Items.Count; i++)
            {
                if (Chkdeg.Items[i].Selected == true)
                {
                    checkselcount++;
                }
            }
            if (checkselcount == 0)
            {
                lblerrmsg.Text = "Please Select Any One School Type";
                lblerrmsg.Visible = true;
                fpspread.Visible = false;
                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;
                return;
            }

            checkselcount = 0;
            if (chklstbranch.Items.Count == 0)
            {
                lblerrmsg.Text = "There Is No Standard Found";
                lblerrmsg.Visible = true;
                fpspread.Visible = false;
                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;
                return;
            }
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    checkselcount++;
                    branchcount++;
                    if (degree == "")
                    {
                        degree = Convert.ToString(chklstbranch.Items[i].Value);
                    }
                    else
                    {
                        degree = degree + "','" + Convert.ToString(chklstbranch.Items[i].Value);
                    }
                }
            }
            if (checkselcount == 0)
            {
                lblerrmsg.Text = "Please Select Any One Standard";
                lblerrmsg.Visible = true;
                fpspread.Visible = false;
                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;
                return;
            }

            //InitSpread();
            //FarPoint.Web.Spread.FpSpread ds1 = (FarPoint.Web.Spread.FpSpread)ViewState["ljkfdsaklf"];
            bool isSucc = false;
            if (branchcount == 1)
            {
                if (batchcount == 1)
                {
                    isSucc = bindfp(batch, degree);
                }
            }

            if (!isSucc)
            {
                InitSpread();

                int fpcol = 9;
                int stcol = 3;
                if (chkPartname.Checked)
                {
                    fpcol = 10;
                    stcol = 4;
                }
                fpspread.Sheets[0].RowCount = 0;
                fpspread.SaveChanges();
                lblerrmsg.Text = "";
                if (txt_totparts.Text.Trim() == "" || txt_totparts.Text.Trim() == null)
                {
                    lblerrmsg.Text = "Please Enter Total Parts/Section";
                    lblerrmsg.Visible = true;
                    fpspread.Visible = false;
                    btnsubti.Visible = false;
                    btnfinalsave.Visible = false;
                    btndelete.Visible = false;
                    return;
                }
                if (txt_partname.Text.Trim() == "" || txt_partname.Text.Trim() == null)
                {
                    lblerrmsg.Text = "Please Enter Part Name";
                    lblerrmsg.Visible = true;
                    fpspread.Visible = false;
                    btnsubti.Visible = false;
                    btnfinalsave.Visible = false;
                    btndelete.Visible = false;
                    return;
                }

                string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'RTnam' and college_code = '" + collegecode + "'";
                string strtit = "";
                ds.Clear();
                fpspread.Height = 550;
                fpspread.Width = 975;
                if (chkPartname.Checked)
                {
                    fpspread.Width = 985;
                }


                ds = da.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    combocol.DataSource = ds;
                    combocol.DataTextField = "TextVal";
                    combocol.DataValueField = "TextCode";
                    combocol.ShowButton = false;
                    combocol.AutoPostBack = true;
                    combocol.UseValue = true;
                }


                fpspread.Visible = false;
                btnsubti.Visible = false;
                btnfinalsave.Visible = false;
                btndelete.Visible = false;

                fpspread.SaveChanges();

                int rowcount = Convert.ToInt32(txt_totparts.Text);
                fpspread.Sheets[0].RowCount = rowcount + 1;

                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = txt_partname.Text;
                fpspread.Sheets[0].Cells[0, 1].CellType = chkboxsel_all;
                fpspread.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
                for (int i = 1; i < fpspread.Sheets[0].RowCount; i++)
                {
                    fpspread.Visible = true;
                    btnsubti.Visible = true;
                    btnfinalsave.Visible = true;
                    btndelete.Visible = true;
                    fpspread.Sheets[0].Cells[i, 0].Text = Convert.ToString(i);

                    fpspread.Sheets[0].Cells[i, 2].Text = Convert.ToString(i);

                    fpspread.Sheets[0].Cells[i, stcol].Tag = "no";

                    for (int j = 0; j < fpspread.Sheets[0].ColumnCount; j++)
                    {
                        fpspread.Sheets[0].Cells[i, 1].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Center;
                        if (chkPartname.Checked)
                        {
                            fpspread.Sheets[0].Cells[i, stcol - 1].Text = "";
                            fpspread.Sheets[0].Cells[i, stcol - 1].HorizontalAlign = HorizontalAlign.Left;
                            fpspread.Sheets[0].Cells[i, stcol - 1].VerticalAlign = VerticalAlign.Middle;
                        }

                        fpspread.Sheets[0].Cells[i, stcol + 1].CellType = combocol;
                        fpspread.Sheets[0].Cells[i, stcol + 2].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[i, stcol + 3].CellType = chkboxcol;
                        // fpspread.Sheets[0].Cells[i, 8].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[i, stcol + 4].CellType = chkboxcol;
                        fpspread.Sheets[0].Cells[i, stcol + 5].CellType = chkboxcol;

                        fpspread.Sheets[0].Cells[i, stcol + 2].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[i, stcol + 3].HorizontalAlign = HorizontalAlign.Center;
                        // fpspread.Sheets[0].Cells[i, 8].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[i, stcol + 4].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[i, stcol + 5].HorizontalAlign = HorizontalAlign.Center;

                    }
                }

                fpspread.Sheets[0].Cells[0, stcol + 1].Locked = true;
                fpspread.Sheets[0].Cells[0, stcol + 2].Locked = true;
                fpspread.Sheets[0].Cells[0, stcol + 3].Locked = true;
                fpspread.Sheets[0].Cells[0, stcol + 4].Locked = true;
                fpspread.Sheets[0].Cells[0, stcol + 5].Locked = true;
                fpspread.SaveChanges();
                fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            }

        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    //protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{

    //    //string selectallvalue = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Value);
    //    //if (selectallvalue == "1" || selectallvalue == "True")
    //    //{

    //    //}

    //}

    protected void Fpspread1_Command(object sender, EventArgs e)
    {
        if (Convert.ToInt32(fpspread.Sheets[0].Cells[0, 1].Value) == 1)
        {
            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
            {
                fpspread.Sheets[0].Cells[i, 1].Value = 1;
                //btncheckadd.Focus();
                //FpSpreadcheck.SaveChanges();
            }
        }
        else if (Convert.ToInt32(fpspread.Sheets[0].Cells[0, 1].Value) == 0)
        {
            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
            {
                fpspread.Sheets[0].Cells[i, 1].Value = 0;
                // btncheckadd.Focus();
                //FpSpreadcheck.SaveChanges();
            }
        }
    }

    //protected void HAllSpread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{

    //    Cellclick = true;

    //    //Backbtn.Visible = true;
    //}
    //protected void HAllSpread_SelectedIndexChanged(Object sender, EventArgs e)
    //{

    //    if (Cellclick == true)
    //    {
    //    }
    //}

    protected void btnadd2_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            if (txttitle.Text.Trim() != "")
            {
                string add = " if exists(select * from textvaltable where TextVal='" + txttitle.Text + "' and TextCriteria='RTnam' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' ) update textvaltable set TextVal='" + txttitle.Text + "',TextCriteria='RTnam',college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' where TextVal='" + txttitle.Text + "' and TextCriteria='RTnam' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' else insert into textvaltable (TextVal,TextCriteria,college_code) values ('" + txttitle.Text + "', 'RTnam','" + Convert.ToString(ddlcollege.SelectedValue) + "')";
                int a = da.update_method_wo_parameter(add, "text");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Subtitle Master Name Added Successfully')", true);
                Titlename();
                txttitle.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Subtitle Master Name')", true);
            }

        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btnexit2_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            imgdiv1.Visible = false;
            pnltitle.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void titleplus_OnClick(object sener, EventArgs e)
    {
        imgdiv1.Visible = true;
        pnltitle.Visible = true;
        pnlactive.Visible = false;
        imgdiv2.Visible = false;
        divSubrmrk.Visible = false;
        pnlsubrmrk.Visible = false;
    }

    protected void actplus_OnClick(object sender, EventArgs e)
    {
        imgdiv2.Visible = true;
        pnlactive.Visible = true;
        imgdiv1.Visible = false;
        pnltitle.Visible = false;
        divSubrmrk.Visible = false;
        pnlsubrmrk.Visible = false;
    }

    protected void btnexit3_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            pnlactive.Visible = false;
            imgdiv2.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void ddlactivity_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg1.Visible = false;
    }

    protected void ddltitlename_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg2.Visible = false;
    }

    protected void actminus_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            // -----------------3.-6**- start
            String sdlete = "select * from activity_entry where ActivityTextVal='" + ddlactivity.SelectedValue + "'";
            DataSet dsdlete = da.select_method_wo_parameter(sdlete, "text");
            // ------------------ end
            if (dsdlete.Tables[0].Rows.Count > 0)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('This Activity Already Used so Can't Be Deleted')", true);
                lblerrmsg1.Text = "This Activity Already Used so Can't Be Deleted";
                lblerrmsg1.Visible = true;
                Activityname();
            }
            else
            {
                string add = "delete from textvaltable where TextCode='" + ddlactivity.SelectedValue + "'and TextCriteria='RActv' and  college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' ";
                int a = da.update_method_wo_parameter(add, "text");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Activity Deleted Successfully')", true);
                Activityname();
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btndescplus_OnClick(object sender, EventArgs e)
    {
        pnldesc.Visible = true;
    }

    protected void btndescminus_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            string add = "delete from textvaltable where TextCode='" + ddldescrip.SelectedValue + "'and TextCriteria='RAdes' and  college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' ";
            int a = da.update_method_wo_parameter(add, "text");
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Description Deleted Successfully')", true);
            descname();
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    // ------------------ add start

    protected void btnacsave_OnClick(object sender, EventArgs e)
    {
        string savquery = "update textvaltable set TextVal='" + txtpnlac.Text + "' where TextCode='" + ddltitlename.SelectedValue + "' and TextCriteria='RTnam' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "'";
        DataSet dsetsave = da.select_method_wo_parameter(savquery, "text");
        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Subtitle Master Name Updated Successfully')", true);
        Titlename();
    }

    protected void btnacexit_OnClick(object sender, EventArgs e)
    {
        divpnac.Visible = false;
        pnlac.Visible = false;
    }

    protected void btnacedit_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            divpnac.Visible = true;
            pnlac.Visible = true;
            txtpnlac.Text = ddltitlename.SelectedItem.Text;
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btnacsave1_OnClick(object sender, EventArgs e)
    {
        string savquery1 = "update textvaltable set TextVal='" + txtpnlac1.Text + "' where TextCode='" + ddlactivity.SelectedValue + "' and TextCriteria='RActv' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "'";
        DataSet dsetsave1 = da.select_method_wo_parameter(savquery1, "text");
        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Activity Master Updated Successfully')", true);
        Activityname();
    }

    protected void btnacexit1_OnClick(object sender, EventArgs e)
    {
        divpnac1.Visible = false;
        pnlac1.Visible = false;
    }

    protected void btnacedit1_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            divpnac1.Visible = true;
            pnlac1.Visible = true;

            txtpnlac1.Text = ddlactivity.SelectedItem.Text;
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    // ----------------- add end

    protected void btndescadd_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            if (txtdescrip.Text.Trim() != "")
            {
                string add = " if exists(select * from textvaltable where TextVal='" + txtdescrip.Text + "' and TextCriteria='RAdes' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' ) update textvaltable set TextVal='" + txtdescrip.Text + "',TextCriteria='RAdes',college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' where TextVal='" + txtdescrip.Text + "' and TextCriteria='RAdes' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' else insert into textvaltable (TextVal,TextCriteria,college_code) values ('" + txtdescrip.Text + "', 'RAdes','" + Convert.ToString(ddlcollege.SelectedValue) + "')";
                int a = da.update_method_wo_parameter(add, "text");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Description Added Successfully')", true);
                descname();
                txtdescrip.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Description')", true);
            }

        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btndescexit_OnClick(object sender, EventArgs e)
    {
        try
        {
            pnldesc.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnadd3_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (txtactive.Text.Trim() != "")
            {
                string add = " if exists(select * from textvaltable where TextVal='" + txtactive.Text + "' and TextCriteria='RActv' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' ) update textvaltable set TextVal='" + txtactive.Text + "',TextCriteria='RActv',college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' where TextVal='" + txtactive.Text + "' and TextCriteria='RActv' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' else insert into textvaltable (TextVal,TextCriteria,college_code) values ('" + txtactive.Text + "', 'RActv','" + Convert.ToString(ddlcollege.SelectedValue) + "')";
                int a = da.update_method_wo_parameter(add, "text");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Activity Added Successfully')", true);
                Activityname();
                txtactive.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Activity')", true);
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            usercode = Convert.ToString(Session["usercode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            collegecode = Convert.ToString(ddlcollege.SelectedValue);
            batch();
            BindDegree();
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            Titlename();
            Activityname();
            descname();
            SubRemark();
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btnrmrkplus_OnClick(object sender, EventArgs e)
    {
        imgdiv1.Visible = false;
        pnltitle.Visible = false;
        pnlactive.Visible = false;
        imgdiv2.Visible = false;
        divSubrmrk.Visible = true;
        pnlsubrmrk.Visible = true;
    }

    protected void btnrmrkminus_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            // -----------------3.-6**- start
            String sdlete = "select * from result where remarks='" + ddlsubrmrk.SelectedValue.ToString() + "'";
            DataSet dsdlete = da.select_method_wo_parameter(sdlete, "text");
            // ------------------ end
            if (dsdlete.Tables[0].Rows.Count > 0)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('This Activity Already Used so Can't Be Deleted')", true);
                lblerrmsg1.Text = "This Subject Remarks Already Used so Can't Be Deleted";
                lblerrmsg1.Visible = true;
                SubRemark();
            }
            else
            {
                string add = "delete from textvaltable where TextCode='" + ddlsubrmrk.SelectedValue + "'and TextCriteria='Rmrk' and  college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' ";
                int a = da.update_method_wo_parameter(add, "text");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Subject Remarks Deleted Successfully')", true);
                SubRemark();
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    protected void btnrmrkedit_OnClick(object sender, EventArgs e)
    {
        divpedtsubrmrk.Visible = true;
        pnledtrmrk.Visible = true;
        if (ddlsubrmrk.Items.Count > 0)
        {
            txt_edtrmrk.Text = ddlsubrmrk.SelectedItem.Text.ToString();
        }
    }

    protected void ddlsubrmrk_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg1.Visible = false;
    }

    protected void btnaddsubrmrk_OnClick(object sender, EventArgs e)
    {
        if (txt_subrmrk.Text.Trim() != "")
        {
            string add = " if exists(select * from textvaltable where TextVal='" + txt_subrmrk.Text + "' and TextCriteria='Rmrk' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' ) update textvaltable set TextVal='" + txt_subrmrk.Text + "',TextCriteria='Rmrk',college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' where TextVal='" + txt_subrmrk.Text + "' and TextCriteria='Rmrk' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' else insert into textvaltable (TextVal,TextCriteria,college_code) values ('" + txt_subrmrk.Text + "', 'Rmrk','" + Convert.ToString(ddlcollege.SelectedValue) + "')";
            int a = da.update_method_wo_parameter(add, "text");
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Subject Remarks Added Successfully')", true);
            SubRemark();
            txt_subrmrk.Text = "";
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Subjects Remarks')", true);
        }
    }

    protected void btnexit_subrmrk_OnClick(object sender, EventArgs e)
    {
        divSubrmrk.Visible = false;
        pnlsubrmrk.Visible = false;
    }

    protected void btnedsavermrk_OnClick(object sender, EventArgs e)
    {
        string savquery1 = "update textvaltable set TextVal='" + txt_edtrmrk.Text + "' where TextCode='" + ddlsubrmrk.SelectedValue + "' and TextCriteria='Rmrk' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "'";
        DataSet dsetsave1 = da.select_method_wo_parameter(savquery1, "text");
        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Subject Remarks Updated Successfully')", true);
        SubRemark();
    }

    protected void btnedtexitrmrk_OnClick(object sender, EventArgs e)
    {
        divpedtsubrmrk.Visible = false;
        pnledtrmrk.Visible = false;
    }

    private void InitSpread()
    {
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            fpspread.Visible = false;
            fpspread.Sheets[0].RowHeader.Visible = false;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.White;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
            fpspread.Sheets[0].AutoPostBack = false;
            fpspread.CommandBar.Visible = false;
            fpspread.Sheets[0].RowCount = 0;
            int fpcol = 9;
            int stcol = 3;
            if (chkPartname.Checked)
            {
                fpcol = 10;
                stcol = 4;
            }
            fpspread.Sheets[0].ColumnCount = fpcol;
            fpspread.Sheets[0].ColumnHeader.RowCount = 1;
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 40;

            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";

            fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 53;
            //fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 100;
            fpspread.Sheets[0].ColumnHeader.Columns[stcol].Width = 153;
            fpspread.Sheets[0].ColumnHeader.Columns[stcol + 1].Width = 250;
            fpspread.Sheets[0].Columns[0].Locked = true;
            fpspread.Sheets[0].Columns[2].Locked = true;
            fpspread.Sheets[0].Columns[stcol].Locked = true;



            for (int i = 0; i < fpcol; i++)
            {
                fpspread.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
            }

            fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = " ";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Part";
            if (chkPartname.Checked)
            {
                fpspread.Sheets[0].ColumnHeader.Cells[0, stcol - 1].Text = "Part Name";
                fpspread.Sheets[0].Columns[stcol - 1].Width = 90;
                fpspread.Sheets[0].Columns[0].Width = 46;
                fpspread.Sheets[0].Columns[1].Width = 48;
                fpspread.Sheets[0].Columns[2].Width = 75;

                fpspread.Sheets[0].Columns[stcol].Width = 90;
                fpspread.Sheets[0].Columns[stcol + 1].Width = 250;
                fpspread.Sheets[0].Columns[stcol + 2].Width = 80;
                fpspread.Sheets[0].Columns[stcol + 3].Width = 80;
                fpspread.Sheets[0].Columns[stcol + 4].Width = 98;
                fpspread.Sheets[0].Columns[stcol + 5].Width = 90;
                fpspread.Sheets[0].Columns[stcol - 1].Locked = false;
                fpspread.Sheets[0].SetColumnMerge(stcol - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                fpspread.Sheets[0].Columns[stcol - 1].CellType = new FarPoint.Web.Spread.TextCellType();
            }
            fpspread.Sheets[0].ColumnHeader.Cells[0, stcol].Text = "Sub Title";
            fpspread.Sheets[0].ColumnHeader.Cells[0, stcol + 1].Text = "Title Name";
            fpspread.Sheets[0].ColumnHeader.Cells[0, stcol + 2].Text = "Direct";
            fpspread.Sheets[0].ColumnHeader.Cells[0, stcol + 3].Text = "Activity";
            fpspread.Sheets[0].ColumnHeader.Cells[0, stcol + 4].Text = "Description";
            fpspread.Sheets[0].ColumnHeader.Cells[0, stcol + 5].Text = "Grade";
            fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 120;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            darkstyle.ForeColor = System.Drawing.Color.White;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Border.BorderSize = 0;
            darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
            fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            //fpspread.Sheets[0].ColumnHeader.Cells[0, 1].CellType = chkboxsel_all;

            for (int g = 0; g < fpspread.Sheets[0].ColumnHeader.Columns.Count; g++)
            {
                fpspread.Sheets[0].ColumnHeader.Cells[0, g].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, g].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, g].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, g].ForeColor = Color.White;
            }

            fpspread.SaveChanges();
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

    private bool partnamepartNo(DataTable dtPart, int row, string cocurid, out string partname, out string partno)
    {
        bool result = false;
        string qry = "";
        partno = "";
        partname = "";
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            if (dtPart.Rows.Count > 0)
            {
                if (dtPart.Rows.Count >= row)
                {
                    if (Convert.ToString(dtPart.Rows[row]["UserPartName"]).Trim() == "" || Convert.ToString(dtPart.Rows[row]["Part_No"]).Trim() == "")
                    {
                        if (Convert.ToString(dtPart.Rows[row]["PartName"]).Trim() != "")
                        {
                            if (Convert.ToString(dtPart.Rows[row]["UserPartName"]).Trim() == "")
                                partname = Convert.ToString(dtPart.Rows[row]["PartName"]).Trim();
                            if (Convert.ToString(dtPart.Rows[row]["Part_No"]).Trim() == "")
                                partno = Convert.ToString(dtPart.Rows[row]["PartName"]).Trim().Split('-')[1].Trim();
                        }
                        else
                        {
                            partname = "";
                            partno = "";
                        }
                        //if (partname.Trim() != "" && partno.Trim() != "" && cocurid.Trim() != "")
                        //{
                        //    qry = "update CoCurr_Activitie set Part_No='" + partno.Trim() + "',UserPartName='" + partname.Trim() + "' where CoCurr_ID='" + cocurid.Trim() + "'";
                        //    int res = da.update_method_wo_parameter(qry, "txet");
                        //    if (res != 0)
                        //    {
                        //        result = true;
                        //    }
                        //}
                    }
                    else
                    {
                        result = true;
                        partname = Convert.ToString(dtPart.Rows[row]["UserPartName"]).Trim();
                        partno = Convert.ToString(dtPart.Rows[row]["Part_No"]).Trim();
                    }
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
            return false;
        }
    }

    private void checktable()
    {
        bool result = false;
        try
        {
            lblerrmsg.Text = "";
            lblerrmsg.Visible = false;
            string q = "";
            int res = 0;
            q = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME='CoCurr_Activitie'";
            string s = da.GetFunctionv(q);
            if (s.Trim() == "")
            {
                q = "CREATE TABLE CoCurr_Activitie(CoCurr_ID numeric(18, 0) IDENTITY(1,1) primary key NOT NULL,PartName nvarchar(100) NULL,SubTitle nvarchar(10) NULL,Title_Name numeric(18, 0) NULL,IsDirectEntry bit NULL,IsActivity bit NULL,IsActDesc bit NULL,IsGrade bit NULL,Degree_Code numeric(18, 0) NULL,Batch_Year numeric(18, 0) NULL,Part_No int NULL,UserPartName nvarchar(max) NULL)";
                res = da.update_method_wo_parameter(q, "text");
                if (res > 0)
                {
                    result = true;
                }
            }
            else
            {
                q = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CoCurr_Activitie' AND COLUMN_NAME = 'Part_No'";
                s = da.GetFunctionv(q);
                string alterqry = "";
                if (s == "")
                {
                    alterqry = "alter table  CoCurr_Activitie add Part_No int";
                }
                q = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CoCurr_Activitie' AND COLUMN_NAME = 'UserPartName'";
                s = da.GetFunctionv(q);
                if (s == "")
                {
                    alterqry += " ; alter table  CoCurr_Activitie add UserPartName nvarchar(Max)";
                }
                if (alterqry.Trim() != "")
                {
                    res = da.update_method_wo_parameter(alterqry, "text");
                    if (res > 0)
                    {
                        result = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerrmsg.Text = Convert.ToString(ex);
            lblerrmsg.Visible = true;
        }
    }

}