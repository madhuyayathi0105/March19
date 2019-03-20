using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Collections;

public partial class Cost_Master_1 : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DAccess2 dacces2 = new DAccess2();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static int batchcnt = 0;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("TransportIndex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/TransportMod/TransportIndex.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (!IsPostBack)
        {
            loadcollege();
            bindroute();
            loadstage();
            SettingRights();
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
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        Control cntUpdateBtn = Fp_Route.FindControl("Update");
        Control cntcancelBtn = Fp_Route.FindControl("Cancel");
        Control cntCopyBtn = Fp_Route.FindControl("Copy");
        Control cntCutBtn = Fp_Route.FindControl("Clear");
        Control cntPasteBtn = Fp_Route.FindControl("Paste");
        Control cntPageNextBtn = Fp_Route.FindControl("Next");
        Control cntPagePreviousBtn = Fp_Route.FindControl("Prev");
        Control cntprintBtn = Fp_Route.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntcancelBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPageNextBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntprintBtn.Parent;
            tr.Cells.Remove(tc);
        }

        base.Render(writer);
    }

    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Fp_Route.Visible = false;
    }
    bool Cellclick;
    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    public string GetFunction(string Att_strqueryst)
    {

        string sqlstr;
        sqlstr = Att_strqueryst;

        con.Close();
        con.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = con;
        drnew = cmd.ExecuteReader();
        drnew.Read();

        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        if (Fp_Route.Sheets[0].RowCount > 0)
        {
            Session["column_header_row_count"] = 1;
            string degreedetails = "Cost Details";
            string pagename = "Cost_Master.aspx";
            Printcontrol.loadspreaddetails(Fp_Route, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
    }
    protected void chkrouteid_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chkrouteid, chklstrouteid, txtrouteid, lblrouteid.Text, "--Select--");
        loadstage();
    }
    protected void chklstrouteid_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkrouteid, chklstrouteid, txtrouteid, lblrouteid.Text, "--Select--");
        loadstage();
    }
    public void bindroute()
    {
        DataSet ds = new DataSet();
        chklstrouteid.Items.Clear();
        chklstrouteid.Items.Insert(0, new ListItem("All", "-1"));
        string sql;
        sql = "select distinct Route_ID from routemaster order by Route_ID";
        ds = dacces2.select_method_wo_parameter(sql, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            chklstrouteid.DataSource = ds;
            chklstrouteid.DataTextField = "Route_ID";
            chklstrouteid.DataValueField = "Route_ID";
            chklstrouteid.DataBind();
            for (int i = 0; i < chklstrouteid.Items.Count; i++)
            {
                chklstrouteid.Items[i].Selected = true;
            }
            chkrouteid.Checked = true;
            txtrouteid.Text = "Route(" + chklstrouteid.Items.Count + ")";
            // chklstrouteid.SelectedIndex = chklstrouteid.Items.Count - 1;
        }
        con.Close();
    }
    protected void SettingRights()
    {
        try
        {
            string feeSetgCode = dacces2.GetFunction("select value from Master_Settings where settings='TransportFeeAllotmentSettings'  and usercode='" + usercode + "'");
            string[] splitval = feeSetgCode.Split('-');
            if (splitval[0] == "1")
            {
                ddl_pattern.SelectedItem.Text = "Semester";
                ddlpatternadd.SelectedItem.Text = "Semester";
            }
            else if (splitval[0] == "2")
            {
                ddl_pattern.SelectedItem.Text = "Yearly";
                ddlpatternadd.SelectedItem.Text = "Yearly";
            }
            else if (splitval[0] == "3")
            {
                ddl_pattern.SelectedItem.Text = "Monthly";
                ddlpatternadd.SelectedItem.Text = "Monthly";
            }
            else if (splitval[0] == "4")
            {
                ddl_pattern.SelectedItem.Text = "Term";
                ddlpatternadd.SelectedItem.Text = "Term";
            }
        }
        catch { }
    }
    protected void cbstage_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbstage, cblstage, txtstage, "Stage", "--Select--");
    }
    protected void cblstage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbstage, cblstage, txtstage, "Stage", "--Select--");
    }
    protected void loadstage()
    {
        try
        {
            string route = "";
            for (int i = 0; i < chklstrouteid.Items.Count; i++)
            {
                if (chklstrouteid.Items[i].Selected == true)
                {
                    if (route == "")
                    {
                        route = Convert.ToString(chklstrouteid.Items[i].Value);
                    }
                    else
                    {
                        route += "','" + Convert.ToString(chklstrouteid.Items[i].Value);
                    }
                }
            }
            //   select distinct Stage_Name from RouteMaster where Route_ID in('1')
            //string selQ = "select distinct Stage_Name,address from RouteMaster where Route_ID in('" + route + "')";
            string selQ = "select distinct rm.Stage_Name,sm.Stage_Name as address from Stage_Master sm,RouteMaster rm where convert(nvarchar,sm.Stage_id)=rm.Stage_Name and rm.Route_ID in('" + route + "')";   //modified by prabha on 21 dec 2017//modified on 01 feb 2018 by prabha  convert(nvarchar,sm.Stage_id) added
            ds = d2.select_method_wo_parameter(selQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblstage.DataSource = ds;
                cblstage.DataTextField = "address";
                cblstage.DataValueField = "Stage_Name";
                cblstage.DataBind();
                if (cblstage.Items.Count > 0)
                {
                    for (int i = 0; i < cblstage.Items.Count; i++)
                    {
                        cblstage.Items[i].Selected = true;
                    }
                    txtstage.Text = "Stages(" + cblstage.Items.Count + ")";
                    cbstage.Checked = true;
                }

            }
            else
            {
                txtstage.Text = "Select";
                cbstage.Checked = false;
            }
        }
        catch { }
    }

    //added by sudhagar 14.02.2017
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            cbclg.Checked = false;
            txtclg.Text = "--Select--";
            cblclg.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblclg.DataSource = ds;
                cblclg.DataTextField = "collname";
                cblclg.DataValueField = "college_code";
                cblclg.DataBind();
                for (int i = 0; i < cblclg.Items.Count; i++)
                    cblclg.Items[i].Selected = true;
                txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
                cbclg.Checked = true;
            }
        }
        catch
        { }
    }
    protected void ddl_pattern_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void cbclg_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
    }
    protected void cblclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
    }

    #region button go
    protected void Btn_go_Click(object sender, EventArgs e)
    {
        getAllotedDetails();

    }
    protected void getAllotedDetails()
    {
        try
        {
            #region
            Fp_Route.Sheets[0].RowCount = 0;
            Fp_Route.Sheets[0].ColumnCount = 0;
            Fp_Route.CommandBar.Visible = false;
            Fp_Route.Sheets[0].AutoPostBack = true;
            Fp_Route.Sheets[0].ColumnHeader.RowCount = 1;
            Fp_Route.Sheets[0].RowHeader.Visible = false;
            Fp_Route.Sheets[0].ColumnCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fp_Route.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Fp_Route.Sheets[0].AutoPostBack = false;
            FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            cball.AutoPostBack = true;
            cb.AutoPostBack = false;
            FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
            intgrcel.MinimumValue = 0;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[0].Width = 40;

            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[1].Width = 50;

            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Pattern";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            Fp_Route.Sheets[0].Columns[2].Width = 80;

            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Stage Name";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            Fp_Route.Sheets[0].Columns[3].Width = 460;

            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Cost";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
            Fp_Route.Sheets[0].Columns[4].Width = 120;
            Fp_Route.Sheets[0].Columns[4].CellType = intgrcel;
            #endregion

            string routeid = Convert.ToString(getCblSelectedValue(chklstrouteid));
            string stageid = Convert.ToString(getCblSelectedValue(cblstage));
            string pattern = Convert.ToString(ddl_pattern.SelectedItem.Text);
            string clgCode = Convert.ToString(getCblSelectedValue(cblclg));
            ArrayList arstageId = new ArrayList();
            if (!string.IsNullOrEmpty(routeid) && !string.IsNullOrEmpty(stageid) && !string.IsNullOrEmpty(pattern))
            {
                string selQ = " select distinct sm.stage_id,sm.Stage_Name,f.cost,f.paytype,f.college_code,rm.route_id,f.Month_Value from stage_master sm,routemaster rm ,feeinfo F where cast(rm.stage_name as varchar(100))=cast(sm.stage_id as varchar(100)) and f.StrtPlace=cast(sm.stage_id as varchar(100)) and f.StrtPlace= cast(rm.stage_name as varchar(100)) and f.college_code in('" + clgCode + "') and rm.route_id in ('" + routeid + "') and rm.stage_name in('" + stageid + "') and paytype='" + pattern + "'";
                DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                {
                    int rowCnt = 0;
                    bool boolcheck = false;
                    for (int crow = 0; crow < cblclg.Items.Count; crow++)
                    {
                        bool clgName = false;
                        arstageId.Clear();
                        if (cblclg.Items[crow].Selected)
                        {
                            collegecode = Convert.ToString(cblclg.Items[crow].Value);
                            dsval.Tables[0].DefaultView.RowFilter = "college_code='" + collegecode + "'";
                            DataView dv = dsval.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                for (int row = 0; row < dv.Count; row++)
                                {
                                    string stageId = Convert.ToString(dv[row]["stage_id"]);
                                    if (!arstageId.Contains(stageId))//purpose for dont repeat same stage multiple route id
                                    {
                                        if (!boolcheck)
                                        {
                                            Fp_Route.Sheets[0].RowCount++;
                                            Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 1].CellType = cball;
                                        }
                                        if (!clgName)
                                        {
                                            Fp_Route.Sheets[0].RowCount++;
                                            Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cblclg.Items[crow].Text);
                                            Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].BackColor = Color.Green;
                                            Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].ForeColor = Color.White;
                                            Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                            Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            Fp_Route.Sheets[0].SpanModel.Add(Fp_Route.Sheets[0].RowCount - 1, 0, 1, 4);
                                            clgName = true;
                                        }
                                        rowCnt++;
                                        Fp_Route.Sheets[0].RowCount++;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Tag = collegecode;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 1].CellType = cb;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[row]["paytype"]);
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[row]["Stage_Name"]);
                                        string routeId = Convert.ToString(dv[row]["route_id"]);
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 3].Tag = stageId;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 3].Note = routeId;
                                        double costAmount = 0;
                                        double.TryParse(Convert.ToString(dv[row]["cost"]), out costAmount);
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(costAmount);
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Locked = true;
                                        //  Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 1].Locked = true;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 2].Locked = true;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 3].Locked = true;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 4].Locked = true;
                                        boolcheck = true;
                                        arstageId.Add(stageId);
                                    }
                                }
                                Fp_Route.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            }
                        }
                    }
                    if (boolcheck)
                    {
                        Fp_Route.Sheets[0].PageSize = Fp_Route.Sheets[0].RowCount;
                        Fp_Route.ShowHeaderSelection = false;
                        Fp_Route.SaveChanges();
                        Fp_Route.Visible = true;
                        divspread.Visible = true;
                        btndetails.Visible = true;
                    }
                }
                else
                {
                    Fp_Route.Visible = false;
                    divspread.Visible = false;
                    btndetails.Visible = false;
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No Record Found!";
                }
            }
            else
            {
                Fp_Route.Visible = false;
                divspread.Visible = false;
                btndetails.Visible = false;
                imgAlert.Visible = true;
                lbl_alert.Text = "No Record Found!";
            }
        }
        catch { }
    }
    protected void Fp_Route_OnButtonCommand(object sendr, EventArgs e)
    {
        Fp_Route.SaveChanges();
        int value = 0;
        int.TryParse(Convert.ToString(Fp_Route.Sheets[0].Cells[0, 1].Value), out value);
        if (value == 1)
        {
            for (int row = 0; row < Fp_Route.Sheets[0].Rows.Count; row++)
                Fp_Route.Sheets[0].Cells[row, 1].Value = 1;
        }
        else
        {
            for (int row = 0; row < Fp_Route.Sheets[0].Rows.Count; row++)
                Fp_Route.Sheets[0].Cells[row, 1].Value = 0;
        }

    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkspread())
            {
                bool check = false;
                Fp_Route.SaveChanges();
                int value = 0;
                string routeid = string.Empty;
                string stageid = string.Empty;
                string pattern = string.Empty;
                string collegecode = string.Empty;
                ArrayList ardet = new ArrayList();
                ArrayList ardetrte = new ArrayList();
                for (int row = 0; row < Fp_Route.Sheets[0].Rows.Count; row++)
                {
                    int.TryParse(Convert.ToString(Fp_Route.Sheets[0].Cells[row, 1].Value), out value);
                    if (value == 1)
                    {
                        string stageids = Convert.ToString(Fp_Route.Sheets[0].Cells[row, 3].Tag);
                        string routeids = Convert.ToString(Fp_Route.Sheets[0].Cells[row, 3].Note);
                        string patterns = Convert.ToString(Fp_Route.Sheets[0].Cells[row, 2].Text);
                        string collegecodes = Convert.ToString(Fp_Route.Sheets[0].Cells[row, 0].Tag);
                        if (!string.IsNullOrEmpty(routeids) && !string.IsNullOrEmpty(collegecodes) && !string.IsNullOrEmpty(stageids) && !string.IsNullOrEmpty(patterns))
                        {
                            if (!ardetrte.Contains(routeids))
                            {
                                if (routeid == string.Empty)
                                    routeid = routeids;
                                else
                                    routeid += "'" + "," + "'" + routeids;
                                ardetrte.Add(routeids);
                            }
                            if (!ardet.Contains(stageids))
                            {
                                if (stageid == string.Empty)
                                    stageid = stageids;
                                else
                                    stageid += "'" + "," + "'" + stageids;
                                ardet.Add(stageids);
                            }
                            if (!ardet.Contains(patterns))
                            {
                                if (pattern == string.Empty)
                                    pattern = patterns;
                                else
                                    pattern += "'" + "," + "'" + patterns;
                                ardet.Add(patterns);
                            }
                            if (!ardet.Contains(collegecodes))
                            {
                                if (collegecode == string.Empty)
                                    collegecode = collegecodes;
                                else
                                    collegecode += "'" + "," + "'" + collegecodes;
                                ardet.Add(collegecodes);
                            }
                            check = true;
                        }
                    }
                }
                if (check)
                {
                    if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(stageid) && !string.IsNullOrEmpty(pattern))
                    {
                        divadd.Visible = true;
                        fpfeesadd.Visible = false;
                        btnsaveadd.Visible = false;
                        btnclearadd.Visible = false;
                        tbldet.Visible = false;
                        getAddDetails(routeid, stageid, pattern, collegecode, "Update");

                    }
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Any One!";
            }
        }
        catch { }
    }

    protected void btn_delete_click(object sender, EventArgs e)
    {
        try
        {
            if (checkspread())
            {
                bool check = false;
                Fp_Route.SaveChanges();
                int value = 0;
                for (int row = 0; row < Fp_Route.Sheets[0].Rows.Count; row++)
                {
                    int.TryParse(Convert.ToString(Fp_Route.Sheets[0].Cells[row, 1].Value), out value);
                    if (value == 1)
                    {
                        string DelQ = " delete from feeinfo where paytype='" + Fp_Route.Sheets[0].Cells[row, 2].Text + "' and StrtPlace in('" + Fp_Route.Sheets[0].Cells[row, 3].Tag + "') and college_code ='" + Fp_Route.Sheets[0].Cells[row, 0].Tag + "'";
                        int del = d2.update_method_wo_parameter(DelQ, "Text");
                        check = true;
                    }
                }
                if (check)
                {
                    Btn_go_Click(sender, e);
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Deleted Successfully!";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Any One!";
            }
        }
        catch { }
    }

    protected bool checkspread()
    {
        bool check = false;
        Fp_Route.SaveChanges();
        int value = 0;
        for (int row = 0; row < Fp_Route.Sheets[0].Rows.Count; row++)
        {
            int.TryParse(Convert.ToString(Fp_Route.Sheets[0].Cells[row, 1].Value), out value);
            if (value == 1)
                check = true;
        }
        return check;
    }
    #endregion


    #region Add

    protected void btn_add_Click(object sender, EventArgs e)
    {
        loadcollegeadd();
        bindrouteadd();
        loadstageadd();
        tbldet.Visible = true;
        divadd.Visible = true;
        fpfeesadd.Visible = false;
        btnsaveadd.Visible = false;
        btnclearadd.Visible = false;
    }
    protected void btn_goadd_Click(object sender, EventArgs e)
    {
        string routeid = Convert.ToString(getCblSelectedValue(cblrouteadd));
        string stageid = Convert.ToString(getCblSelectedValue(cblstageadd));
        string pattern = Convert.ToString(ddlpatternadd.SelectedItem.Text);
        string collegecode = string.Empty;
        if (!string.IsNullOrEmpty(routeid) && !string.IsNullOrEmpty(stageid) && !string.IsNullOrEmpty(pattern))
        {
            getAddDetails(routeid, stageid, pattern, collegecode, "Go");

        }
        else
        {
            btnsaveadd.Visible = false;
            btnclearadd.Visible = false;
            fpfeesadd.Visible = false;
            imgAlert.Visible = true;
            lbl_alert.Text = "Kindly select Route/Stage Id!";
        }
    }
    protected void btnsaveadd_click(object sender, EventArgs e)
    {
        saveAddDetails();
    }
    protected void btnclearadd_Click(object sender, EventArgs e)
    {
        btn_goadd_Click(sender, e);
    }

    protected void imgclose_Click(object sender, EventArgs e)
    {
        divadd.Visible = false;
    }

    #endregion

    protected void getAddDetails(string routeid, string stageid, string pattern, string collegecode, string checkName)
    {
        try
        {
            #region design
            fpfeesadd.Sheets[0].RowCount = 0;
            fpfeesadd.Sheets[0].ColumnCount = 0;
            fpfeesadd.CommandBar.Visible = false;
            fpfeesadd.Sheets[0].AutoPostBack = true;
            fpfeesadd.Sheets[0].ColumnHeader.RowCount = 1;
            fpfeesadd.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            fpfeesadd.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            fpfeesadd.Sheets[0].AutoPostBack = false;
            FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
            intgrcel.MinimumValue = 0;
            if (ddlpatternadd.SelectedItem.Text.Trim() != "Monthly")
                fpfeesadd.Sheets[0].ColumnCount = 5;
            else
                fpfeesadd.Sheets[0].ColumnCount = 6;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpfeesadd.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpfeesadd.Sheets[0].Columns[0].Width = 40;

            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Pattern";
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpfeesadd.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            fpfeesadd.Sheets[0].Columns[1].Width = 70;

            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Stage Name";
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            fpfeesadd.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            fpfeesadd.Sheets[0].Columns[2].Width = 520;

            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Cost";
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            fpfeesadd.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            fpfeesadd.Sheets[0].Columns[3].CellType = intgrcel;
            fpfeesadd.Sheets[0].Columns[3].Width = 100;

            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 4].Text = "College Acronym";
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            fpfeesadd.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            fpfeesadd.Sheets[0].Columns[4].CellType = intgrcel;
            //fpfeesadd.Sheets[0].Columns[4].Width = 140;
            fpfeesadd.Sheets[0].Columns[4].Visible = false;
            if (ddlpatternadd.SelectedItem.Text.Trim() == "Monthly")
            {
                fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Monthly";
                fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                fpfeesadd.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpfeesadd.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                fpfeesadd.Sheets[0].Columns[5].Width = 70;
            }
            FarPoint.Web.Spread.ButtonCellType lnkMonth = new FarPoint.Web.Spread.ButtonCellType();
            lnkMonth.Text = "Monthly";
            #endregion

            if (checkName == "Go")
            {
                #region go

                fpfeesadd.SaveChanges();
                string selQ = " select distinct sm.stage_id,sm.Stage_Name from stage_master sm,routemaster rm where cast(rm.stage_name as varchar(100))=cast(sm.stage_id as varchar(100)) and rm.route_id in ('" + routeid + "') and rm.stage_name in('" + stageid + "')";
                DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                    {
                        fpfeesadd.Sheets[0].RowCount++;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 0].Text = (row + 1).ToString();
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 1].Text = pattern;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsval.Tables[0].Rows[row]["Stage_Name"]);
                        string stageId = Convert.ToString(dsval.Tables[0].Rows[row]["stage_id"]);
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 2].Tag = stageId;
                        double costAmount = 0;
                        //  double.TryParse(Convert.ToString(GetFunction("select isnull(cost,0) as cost from feeinfo where cast(strtplace as varchar(100))='" + stageId + "' and paytype='" + pattern + "'")), out costAmount);
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(costAmount);

                        //if monthly setting available
                        if (ddlpatternadd.SelectedItem.Text.Trim() == "Monthly")
                        {
                            //fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 5].Text = "Monthly";
                            fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 5].CellType = lnkMonth;
                            fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 5].Tag = "0";
                            //fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 5].ForeColor = Color.Blue;
                            //fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 5].Font.Underline = true;
                            fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 5].Locked = false;
                        }
                        fpfeesadd.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 0].Locked = true;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 1].Locked = true;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 2].Locked = true;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 3].Locked = false;
                    }
                    fpfeesadd.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    fpfeesadd.Sheets[0].PageSize = fpfeesadd.Sheets[0].RowCount;
                    fpfeesadd.ShowHeaderSelection = false;
                    fpfeesadd.SaveChanges();
                    fpfeesadd.Visible = true;
                    btnsaveadd.Visible = true;
                    btnclearadd.Visible = true;
                    btnsaveadd.Text = "Save";
                }
                else
                {
                    btnsaveadd.Visible = false;
                    btnclearadd.Visible = false;
                    fpfeesadd.Visible = false;
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No Record Found!";
                }
                #endregion
            }
            else if (checkName == "Update")
            {
                #region update
                fpfeesadd.SaveChanges();
                string selQ = " select distinct sm.stage_id,sm.Stage_Name,f.cost,f.paytype,f.college_code,rm.route_id,f.Month_Value from stage_master sm,routemaster rm ,feeinfo F where cast(rm.stage_name as varchar(100))=cast(sm.stage_id as varchar(100)) and f.StrtPlace=cast(sm.stage_id as varchar(100)) and f.StrtPlace= cast(rm.stage_name as varchar(100)) and f.college_code in('" + collegecode + "') and rm.route_id in ('" + routeid + "') and rm.stage_name in('" + stageid + "') and paytype='" + pattern + "'";
                selQ += " select collname,coll_acronymn as acr,college_code from collinfo";
                DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                {
                    bool boolcheck = true;
                    int rowCnt = 0;
                    string Year = string.Empty;
                    Dictionary<int, string> dtMonth = new Dictionary<int, string>();
                    if (ddlpatternadd.SelectedItem.Text.Trim() == "Monthly")
                        getMonthSettings(out  Year, out  dtMonth);
                    for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                    {
                        rowCnt++;
                        fpfeesadd.Sheets[0].RowCount++;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                        collegecode = Convert.ToString(dsval.Tables[0].Rows[row]["college_code"]);
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 0].Tag = collegecode;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsval.Tables[0].Rows[row]["paytype"]);
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsval.Tables[0].Rows[row]["Stage_Name"]);
                        string stageId = Convert.ToString(dsval.Tables[0].Rows[row]["stage_id"]);
                        string routeId = Convert.ToString(dsval.Tables[0].Rows[row]["route_id"]);
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 2].Tag = stageId;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 3].Note = routeId;
                        double costAmount = 0;
                        double.TryParse(Convert.ToString(dsval.Tables[0].Rows[row]["cost"]), out costAmount);
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(costAmount);
                        if (dsval.Tables[1].Rows.Count > 0)
                        {
                            dsval.Tables[1].DefaultView.RowFilter = "college_code='" + collegecode + "'";
                            DataView dvclg = dsval.Tables[1].DefaultView;
                            string colAcr = string.Empty;
                            if (dvclg.Count > 0)
                                colAcr = Convert.ToString(dvclg[0]["acr"]);
                            fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 4].Text = colAcr;
                            fpfeesadd.Sheets[0].Columns[4].Visible = true;
                        }
                        if (ddlpatternadd.SelectedItem.Text.Trim() == "Monthly")
                        {
                            string monthValue = Convert.ToString(dsval.Tables[0].Rows[row]["Month_Value"]);
                            fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 5].CellType = lnkMonth;
                            fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 5].Tag = "1";
                            fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 5].Note = monthValue;
                            fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 5].Locked = false;
                            monthwiseUpdate(monthValue, fpfeesadd.Sheets[0].RowCount - 1, 5, Year, dtMonth);
                        }

                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 0].Locked = true;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 1].Locked = true;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 2].Locked = true;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 3].Locked = false;
                        fpfeesadd.Sheets[0].Cells[fpfeesadd.Sheets[0].RowCount - 1, 4].Locked = false;
                        boolcheck = true;
                    }
                    fpfeesadd.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    if (boolcheck)
                    {
                        fpfeesadd.Sheets[0].PageSize = fpfeesadd.Sheets[0].RowCount;
                        fpfeesadd.ShowHeaderSelection = false;
                        fpfeesadd.SaveChanges();
                        fpfeesadd.Visible = true;
                        btnsaveadd.Visible = true;
                        btnclearadd.Visible = false;
                        btnsaveadd.Text = "Update";
                    }
                }
                else
                {
                    btnsaveadd.Visible = false;
                    btnclearadd.Visible = false;
                    fpfeesadd.Visible = false;
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No Record Found!";
                }
                #endregion
            }
        }
        catch { }
    }

    protected void saveAddDetails()
    {
        try
        {
            fpfeesadd.SaveChanges();
            string collegecode = string.Empty;
            int ans = 0;
            string Year = string.Empty;
            Dictionary<int, string> dtMonth = new Dictionary<int, string>();
            if (ddlpatternadd.SelectedItem.Text.Trim() == "Monthly")
            {
                getMonthSettings(out  Year, out  dtMonth);
            }

            if (btnsaveadd.Text == "Save")
            {
                for (int crow = 0; crow < cblclgadd.Items.Count; crow++)
                {
                    if (cblclgadd.Items[crow].Selected)
                    {
                        collegecode = Convert.ToString(cblclgadd.Items[crow].Value);
                        for (int row = 0; row < fpfeesadd.Sheets[0].Rows.Count; row++)
                        {
                            string pattern = Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 1].Text);
                            string stageId = Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 2].Tag);
                            string stagename = Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 2].Text);
                            double costAmount = 0;
                            double.TryParse(Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 3].Text), out  costAmount);
                            if (ddlpatternadd.SelectedItem.Text.Trim() != "Monthly" && costAmount != 0)//if not monthwise
                            {
                                string endplace = "";//and 
                                string InsQ = "if exists (select * from FeeInfo where  StrtPlace='" + stageId + "'  and payType='" + pattern + "'  and college_code='" + collegecode + "') update FeeInfo set cost='" + costAmount + "',EndPlace='" + endplace + "' where StrtPlace='" + stageId + "' and payType='" + pattern + "'  and college_code='" + collegecode + "' else insert into FeeInfo (Route_ID,StrtPlace,EndPlace,cost,payType,college_code) values('','" + stageId + "','" + endplace + "','" + costAmount + "','" + pattern + "','" + collegecode + "')";
                                ans = dacces2.update_method_wo_parameter(InsQ, "text");
                            }
                            else//month wise
                            {
                                if (costAmount != 0)
                                {
                                    string monthValue = Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 5].Note);
                                    if (string.IsNullOrEmpty(monthValue))
                                    {
                                        int totMonthCnt = 0;
                                        int.TryParse(Convert.ToString(dtMonth.Count), out totMonthCnt);
                                        double tempAmt = Math.Round(costAmount / totMonthCnt, 1, MidpointRounding.AwayFromZero);
                                        foreach (KeyValuePair<int, string> getMonth in dtMonth)
                                        {

                                            if (monthValue == "")
                                                monthValue = "" + Convert.ToString(getMonth.Key) + ":" + Year + ":" + tempAmt + "";
                                            else
                                                monthValue = monthValue + "," + Convert.ToString(getMonth.Key) + ":" + Year + ":" + tempAmt + "";
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(monthValue))
                                    {
                                        string endplace = "";//and 
                                        string InsQ = "if exists (select * from FeeInfo where  StrtPlace='" + stageId + "'  and payType='" + pattern + "'  and college_code='" + collegecode + "') update FeeInfo set cost='" + costAmount + "',EndPlace='" + endplace + "',Month_Value='" + monthValue + "' where StrtPlace='" + stageId + "' and payType='" + pattern + "'  and college_code='" + collegecode + "' else insert into FeeInfo (Route_ID,StrtPlace,EndPlace,cost,payType,college_code,Month_Value) values('','" + stageId + "','" + endplace + "','" + costAmount + "','" + pattern + "','" + collegecode + "','" + monthValue + "')";
                                        ans = dacces2.update_method_wo_parameter(InsQ, "text");
                                    }
                                }
                            }
                        }
                    }
                }
                if (ans > 0)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Enter The Amount!";
                }
            }
            else if (btnsaveadd.Text == "Update")
            {
                for (int row = 0; row < fpfeesadd.Sheets[0].Rows.Count; row++)
                {
                    collegecode = Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 0].Tag);
                    string pattern = Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 1].Text);
                    string stageId = Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 2].Tag);
                    string stagename = Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 2].Text);
                    double costAmount = 0;
                    double.TryParse(Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 3].Text), out  costAmount);
                    if (costAmount != 0)
                    {
                        if (ddlpatternadd.SelectedItem.Text.Trim() != "Monthly")//if not monthwise
                        {
                            string endplace = "";
                            string InsQ = "if exists (select * from FeeInfo where  StrtPlace='" + stageId + "'  and payType='" + pattern + "'  and college_code='" + collegecode + "') update FeeInfo set cost='" + costAmount + "',EndPlace='" + endplace + "' where StrtPlace='" + stageId + "' and payType='" + pattern + "' and college_code='" + collegecode + "' else insert into FeeInfo (Route_ID,StrtPlace,EndPlace,cost,payType,college_code) values('','" + stageId + "','" + endplace + "','" + costAmount + "','" + pattern + "','" + collegecode + "')";
                            ans = dacces2.update_method_wo_parameter(InsQ, "text");
                        }
                        else
                        {
                            string monthValue = Convert.ToString(fpfeesadd.Sheets[0].Cells[row, 5].Note);
                            if (!string.IsNullOrEmpty(monthValue))
                            {
                                string endplace = "";//and 
                                string InsQ = "if exists (select * from FeeInfo where  StrtPlace='" + stageId + "'  and payType='" + pattern + "'  and college_code='" + collegecode + "') update FeeInfo set cost='" + costAmount + "',EndPlace='" + endplace + "',Month_Value='" + monthValue + "' where StrtPlace='" + stageId + "' and payType='" + pattern + "'  and college_code='" + collegecode + "' else insert into FeeInfo (Route_ID,StrtPlace,EndPlace,cost,payType,college_code,Month_Value) values('','" + stageId + "','" + endplace + "','" + costAmount + "','" + pattern + "','" + collegecode + "','" + monthValue + "')";
                                ans = dacces2.update_method_wo_parameter(InsQ, "text");
                            }
                        }
                    }
                }
                if (ans > 0)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Enter The Amount!";
                }
            }
        }
        catch { }
    }

    #region add filter values

    public void loadcollegeadd()
    {
        try
        {
            ds.Clear();
            cbclgadd.Checked = false;
            txtclgadd.Text = "--Select--";
            cblclgadd.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblclgadd.DataSource = ds;
                cblclgadd.DataTextField = "collname";
                cblclgadd.DataValueField = "college_code";
                cblclgadd.DataBind();
                for (int i = 0; i < cblclgadd.Items.Count; i++)
                    cblclgadd.Items[i].Selected = true;
                txtclgadd.Text = lblclg.Text + "(" + cblclgadd.Items.Count + ")";
                cbclgadd.Checked = true;
            }
        }
        catch
        { }
    }
    protected void ddlpatternadd_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void cbclgadd_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclgadd, cblclgadd, txtclgadd, lblclgadd.Text, "--Select--");
    }
    protected void cblclgadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclgadd, cblclgadd, txtclgadd, lblclgadd.Text, "--Select--");
    }

    protected void cbrouteadd_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbrouteadd, cblrouteadd, txtrouteadd, "Stage", "--Select--");
        loadstageadd();
    }
    protected void cblrouteadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbrouteadd, cblrouteadd, txtrouteadd, "Stage", "--Select--");

        loadstageadd();
    }
    public void bindrouteadd()
    {
        cblrouteadd.Items.Clear();
        cbrouteadd.Checked = false;
        txtrouteadd.Text = "--Select--";
        string selQ = "select distinct Route_ID from routemaster order by Route_ID";
        ds = d2.select_method_wo_parameter(selQ, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblrouteadd.DataSource = ds;
            cblrouteadd.DataTextField = "Route_ID";
            cblrouteadd.DataValueField = "Route_ID";
            cblrouteadd.DataBind();
            if (cblrouteadd.Items.Count > 0)
            {
                for (int i = 0; i < cblrouteadd.Items.Count; i++)
                {
                    cblrouteadd.Items[i].Selected = true;
                }
                txtrouteadd.Text = "Route(" + cblrouteadd.Items.Count + ")";
                cbrouteadd.Checked = true;
            }
        }



    }

    protected void cbstageadd_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbstageadd, cblstageadd, txtstageadd, "Route", "--Select--");

    }
    protected void cblstageadd_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbstageadd, cblstageadd, txtstageadd, "Route", "--Select--");
    }
    protected void loadstageadd()
    {
        try
        {
            string route = Convert.ToString(getCblSelectedValue(cblrouteadd));
            DataSet ds = new DataSet();
            cblstageadd.Items.Clear();
            cbstageadd.Checked = false;
            txtstageadd.Text = "--Select--";
            cblstageadd.Items.Insert(0, new ListItem("All", "-1"));
            //string selQ = "select distinct Stage_Name,address from RouteMaster where Route_ID in('" + route + "')";

            string selQ = "select distinct rm.Stage_Name,sm.Stage_Name as address from Stage_Master sm,RouteMaster rm where convert(nvarchar,sm.Stage_id)=rm.Stage_Name and rm.Route_ID in('" + route + "')";   //modified by rajasekar on 13/08/2018
            ds = dacces2.select_method_wo_parameter(selQ, "txt");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstageadd.DataSource = ds;
                cblstageadd.DataTextField = "address";
                cblstageadd.DataValueField = "Stage_Name";
                cblstageadd.DataBind();
                for (int i = 0; i < cblstageadd.Items.Count; i++)
                {
                    cblstageadd.Items[i].Selected = true;
                }
                cbstageadd.Checked = true;
                txtstageadd.Text = "Stage(" + cblstageadd.Items.Count + ")";
            }
            con.Close();
        }
        catch { }
    }

    #endregion

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

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }

    //ADDED BY SUDHAGAR 26.06.2017

    protected void fpfeesadd_OnButtonCommand(object sender, EventArgs e)
    {
        if (ddlpatternadd.SelectedItem.Text.Trim() == "Monthly")
        {
            int actrow = 0;
            int actCol = 0;
            int.TryParse(Convert.ToString(fpfeesadd.ActiveSheetView.ActiveRow), out actrow);
            int.TryParse(Convert.ToString(fpfeesadd.ActiveSheetView.ActiveColumn), out actCol);
            if (actrow != -1)
            {
                int checkVal = 0;
                int.TryParse(Convert.ToString(fpfeesadd.Sheets[0].Cells[actrow, 5].Tag), out checkVal);
                if (checkVal == 0)//go process
                {
                    double totAmt = 0;
                    string stageID = Convert.ToString(fpfeesadd.Sheets[0].Cells[actrow, 2].Tag);
                    double.TryParse(Convert.ToString(fpfeesadd.Sheets[0].Cells[actrow, 3].Text), out totAmt);
                    string Year = string.Empty;
                    Dictionary<int, string> dtMonth = new Dictionary<int, string>();
                    getMonthSettings(out  Year, out  dtMonth);
                    if (totAmt != 0)
                    {
                        monthwise(actrow, actCol, stageID, totAmt, Year, dtMonth);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter The Cost')", true);
                    }
                }
                else//update
                {
                    string Year = string.Empty;
                    Dictionary<int, string> dtMonth = new Dictionary<int, string>();
                    getMonthSettings(out  Year, out  dtMonth);
                    string monthValue = Convert.ToString(fpfeesadd.Sheets[0].Cells[actrow, 5].Note);
                    if (!string.IsNullOrEmpty(monthValue))
                    {
                        monthwiseUpdate(monthValue, actrow, actCol, Year, dtMonth);
                        lblErrorMsg.Visible = false;
                        pnlupdate.Visible = true;
                        divpnlupdate.Visible = true;
                    }
                    else
                    {
                        lblErrorMsg.Visible = false;
                        pnlupdate.Visible = false;
                        divpnlupdate.Visible = false;
                    }
                }
            }
        }
    }


    protected void getMonthSettings(out string Year, out Dictionary<int, string> dtMonth)
    {
        Year = string.Empty;
        dtMonth = new Dictionary<int, string>();
        string getMonth = d2.GetFunction("select value from Master_Settings where settings='TransportFeeAllotmentSettings' and usercode='" + usercode + "'");
        if (getMonth != "0")
        {
            string[] splitval = getMonth.Split('-');
            if (splitval[1].Contains(";") == true)
            {
                string[] year1 = splitval[1].Split(';');
                if (year1.Length > 1)
                    Year = Convert.ToString(year1[1]);
                if (year1[0].Contains(",") == true)
                {
                    string[] year2 = year1[0].Split(',');
                    if (year2.Length > 0)
                    {
                        for (int row = 0; row < year2.Length; row++)
                            dtMonth.Add(Convert.ToInt32(year2[row]), getMonthName(year2[row]));
                    }
                }
            }
        }
    }
    protected void FpSpread3_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            //int a1 = (FpSpread3.Sheets[0].RowCount) - 2;
            // FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Formula = "SUM(D1:D" + a1 + ")";
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnexi_Click(object sender, EventArgs e)
    {
        divpnlupdate.Visible = false;
    }
    protected void btnok_Click(object sender, EventArgs e)
    {
        try
        {
            string monthwise = string.Empty;
            FpSpread3.SaveChanges();
            int actrow = 0;
            int actCol = 0;
            int.TryParse(Convert.ToString(fpfeesadd.ActiveSheetView.ActiveRow), out actrow);
            int.TryParse(Convert.ToString(fpfeesadd.ActiveSheetView.ActiveColumn), out actCol);
            bool boolCheck = false;
            double GetTotalAmount = 0;
            if (FpSpread3.Sheets[0].Rows.Count > 0)
            {
                double.TryParse(lblTotalAmount.Text, out GetTotalAmount);
                double TotalAmount = 0;
                for (int i = 0; i < FpSpread3.Sheets[0].Rows.Count; i++)
                {
                    double Amount = 0;
                    double.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[i, 3].Text), out Amount);
                    if (Amount != 0)
                    {
                        TotalAmount += Amount;
                        if (monthwise == "")
                            monthwise = "" + FpSpread3.Sheets[0].Cells[i, 1].Tag + ":" + FpSpread3.Sheets[0].Cells[i, 2].Text + ":" + Amount + "";
                        else
                            monthwise = monthwise + "," + FpSpread3.Sheets[0].Cells[i, 1].Tag + ":" + FpSpread3.Sheets[0].Cells[i, 2].Text + ":" + Amount + "";
                        boolCheck = true;
                    }
                }
                double tempAmt = 0;
                if (TotalAmount != 0)
                    tempAmt = Math.Round(TotalAmount, 0, MidpointRounding.AwayFromZero);
                if (GetTotalAmount != tempAmt)
                {
                    lblErrorMsg.Text = "Allot Amount Must Match With Total Amount";
                    lblErrorMsg.Visible = true;
                }
                else
                {
                    if (boolCheck && !string.IsNullOrEmpty(monthwise))
                    {
                        fpfeesadd.Sheets[0].Cells[actrow, 5].Note = monthwise;
                    }
                    //if (monthwise.Trim() != "")
                    //    Session["MonthValue"] = monthwise.ToString();
                    pnlupdate.Visible = false;
                    divpnlupdate.Visible = false;
                    lblErrorMsg.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void monthwise(int actrow, int actCol, string StgeID, double Cost, string Year, Dictionary<int, string> dtMonth)
    {
        try
        {
            #region Montwise Retrieve
            string[] prevYear = new string[13];
            string[] prevAmt = new string[13];
            #endregion
            string monthwise = string.Empty;
            string type = ddlpatternadd.SelectedItem.Text;
            lblTotalAmount.Text = Cost.ToString();
            int totMonthCnt = 0;
            int.TryParse(Convert.ToString(dtMonth.Count), out totMonthCnt);
            //double[] monthSplAmt = new double[totMonthCnt];
            double tempAmt = Math.Round(Cost / totMonthCnt, 1, MidpointRounding.AwayFromZero);
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.CommandBar.Visible = false;
            FpSpread3.Sheets[0].AutoPostBack = false;
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 4;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Column.Width = 50;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Month";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Column.Width = 100;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;

            string[] droparray = new string[1];
            droparray[0] = Year;
            FarPoint.Web.Spread.ComboBoxCellType cbYear = new FarPoint.Web.Spread.ComboBoxCellType(droparray);
            cbYear.UseValue = true;
            cbYear.ShowButton = true;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Year";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Column.Width = 80;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Amount";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Column.Width = 80;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
            intgrcell.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            //  intgrcell.MaximumValue = Convert.ToInt32(100);
            intgrcell.MinimumValue = 0;
            intgrcell.ErrorMessage = "Enter valid Number";
            FpSpread3.Sheets[0].Columns[2].CellType = intgrcell;
            FpSpread3.Sheets[0].Columns[2].Font.Bold = false;
            FpSpread3.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
            DropDownList ddlmonth = new DropDownList();
            //for (int i = 0; i < ddlmonth.Items.Count; i++)
            //{
            int rowCnt = 0;
            bool boolCheck = false;
            foreach (KeyValuePair<int, string> getMonth in dtMonth)
            {
                FpSpread3.Sheets[0].Rows.Count++;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(++rowCnt);
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(getMonth.Value);
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(getMonth.Key);
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].Rows.Count - 1, 2].CellType = cbYear;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(tempAmt);
                boolCheck = true;
                if (monthwise == "")
                    monthwise = "" + Convert.ToString(getMonth.Key) + ":" + Year + ":" + tempAmt + "";
                else
                    monthwise = monthwise + "," + Convert.ToString(getMonth.Key) + ":" + Year + ":" + tempAmt + "";
            }
            if (boolCheck && !string.IsNullOrEmpty(monthwise))
            {
                fpfeesadd.Sheets[0].Cells[actrow, 5].Note = monthwise;
            }
            //}
            FpSpread3.Height = 350;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].Rows.Count;
            lblErrorMsg.Visible = false;
            pnlupdate.Visible = true;
            divpnlupdate.Visible = true;
        }
        catch (Exception ex)
        {

        }
    }

    public void monthwiseUpdate(string MonthVal, int actrow, int actCol, string Year, Dictionary<int, string> dtMonth)
    {
        try
        {
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.CommandBar.Visible = false;
            FpSpread3.Sheets[0].AutoPostBack = false;
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 4;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Column.Width = 50;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Month";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Column.Width = 100;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Year";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Column.Width = 80;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Amount";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Column.Width = 80;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
            intgrcell.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            //  intgrcell.MaximumValue = Convert.ToInt32(100);
            intgrcell.MinimumValue = 0;
            intgrcell.ErrorMessage = "Enter valid Number";
            FpSpread3.Sheets[0].Columns[2].CellType = intgrcell;
            FpSpread3.Sheets[0].Columns[2].Font.Bold = false;
            FpSpread3.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
            //ArrayList arRemainMon = new ArrayList();
            //string[] splVal = MonthVal.Split(',');
            //foreach (KeyValuePair<int, string> getMonth in dtMonth)
            //{
            //    string mncode = Convert.ToString(getMonth.Key);
            //    string mnName = Convert.ToString(getMonth.Key);
            //    for (int i = 0; i < splVal.Length; i++)
            //    {
            //        string[] mnthval = splVal[i].Split(':');
            //        if (!mnthval[0].Contains(mncode))
            //        {
            //            if (!arRemainMon.Contains(mncode))
            //                arRemainMon.Add(mncode);
            //        }
            //        else
            //        {
            //            if (arRemainMon.Contains(mncode))
            //                arRemainMon.Remove(mncode);
            //        }
            //    }
          




            bool boolCheck = false;
            double totalAmount = 0;
            int rowCnt = 0;
             string[] splVal = MonthVal.Split(',');
            foreach (KeyValuePair<int, string> getMonth in dtMonth)
            {
                string mncode = Convert.ToString(getMonth.Key);
                string mnName = Convert.ToString(getMonth.Key);
                for (int i = 0; i < splVal.Length; i++)
                {
                    string[] mnthval = splVal[i].Split(':');
                    if (mnthval[0].Contains(mncode))
                    {
                        FpSpread3.Sheets[0].Rows.Count++;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(++rowCnt);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(getMonthName(mnthval[0]));
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(mnthval[0]);
                        string[] droparray = new string[1];
                        droparray[0] = Convert.ToString(mnthval[1]); ;
                        FarPoint.Web.Spread.ComboBoxCellType cbYear = new FarPoint.Web.Spread.ComboBoxCellType(droparray);
                        cbYear.UseValue = true;
                        cbYear.ShowButton = true;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].Rows.Count - 1, 2].CellType = cbYear;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(mnthval[2]);
                        boolCheck = true;
                        double tempAmt = 0;
                        double.TryParse(Convert.ToString(mnthval[2]), out tempAmt);
                        totalAmount += tempAmt;
                    }
                }
            }
            //if (boolCheck)
            //{
            //    fpfeesadd.Sheets[0].Cells[actrow, 5].Note = monthwise;
            //}
            string type = ddlpatternadd.SelectedItem.Text;
            if (totalAmount != 0)
                totalAmount = Math.Round(totalAmount, 0, MidpointRounding.AwayFromZero);
            lblTotalAmount.Text = Convert.ToString(totalAmount);
            FpSpread3.Height = 350;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].Rows.Count;

        }
        catch (Exception ex)
        {

        }
    }
    protected string getMonthName(string MonthCode)
    {
        string monthName = string.Empty;
        switch (MonthCode)
        {
            case "1":
                monthName = "JAN";
                break;
            case "2":
                monthName = "FEB";
                break;
            case "3":
                monthName = "MAR";
                break;
            case "4":
                monthName = "APR";
                break;
            case "5":
                monthName = "MAY";
                break;
            case "6":
                monthName = "JUN";
                break;
            case "7":
                monthName = "JUL";
                break;
            case "8":
                monthName = "AUG";
                break;
            case "9":
                monthName = "SEP";
                break;
            case "10":
                monthName = "OCT";
                break;
            case "11":
                monthName = "NOV";
                break;
            case "12":
                monthName = "DEC";
                break;
        }
        return monthName;
    }


}