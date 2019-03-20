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

public partial class TransportMod_CostMasterReport : System.Web.UI.Page
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
    string pattern = string.Empty;
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
            //SettingRights();
        }
    }


    public void loadcollege()
    {
        try
        {
            ds.Clear();
            //cbclg.Checked = false;
            //txtclg.Text = "--Select--";
            ddlclg.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                //for (int i = 0; i < ddlclg.Items.Count; i++)
                //    cblclg.Items[i].Selected = true;
                //txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
                //cbclg.Checked = true;
            }
        }
        catch
        { }
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
            
        }
        con.Close();
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
            
            string selQ = "select distinct rm.Stage_Name,sm.Stage_Name as address from Stage_Master sm,RouteMaster rm where convert(nvarchar,sm.Stage_id)=rm.Stage_Name and rm.Route_ID in('" + route + "')";   
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
    protected void SettingRights()
    {
        try
        {
            string feeSetgCode = dacces2.GetFunction("select value from Master_Settings where settings='TransportFeeAllotmentSettings'  and usercode='" + usercode + "'");
            string[] splitval = feeSetgCode.Split('-');
            if (splitval[0] == "1")
            {
                pattern = "Semester";

            }
            else if (splitval[0] == "2")
            {
                pattern = "Yearly";

            }
            else if (splitval[0] == "3")
            {
                pattern = "Monthly";

            }
            else if (splitval[0] == "4")
            {
                pattern = "Term";

            }
        }
        catch { }
    }


    //protected void cbclg_CheckedChanged(object sender, EventArgs e)
    //{
    //    CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
    //}
    //protected void cblclg_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
    //}

    //protected void ddl_pattern_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //}
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
    protected void cbstage_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbstage, cblstage, txtstage, "Stage", "--Select--");
    }
    protected void cblstage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbstage, cblstage, txtstage, "Stage", "--Select--");
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
            Fp_Route.Sheets[0].ColumnCount = 6;
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
            Fp_Route.Sheets[0].Columns[0].Width = 50;

            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Route ID";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[1].Width = 100;



            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Start Place";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            Fp_Route.Sheets[0].Columns[2].Width = 300;

            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].Text = "End Place";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            Fp_Route.Sheets[0].Columns[3].Width = 300;

            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Cost";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
            Fp_Route.Sheets[0].Columns[4].Width = 120;
            //Fp_Route.Sheets[0].Columns[4].CellType = intgrcel;


            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Pay Type";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            Fp_Route.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            Fp_Route.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            Fp_Route.Sheets[0].Columns[5].Width = 80;
            #endregion

            string routeid = Convert.ToString(getCblSelectedValue(chklstrouteid));
            string stageid = Convert.ToString(getCblSelectedValue(cblstage));
            //string pattern = Convert.ToString(ddl_pattern.SelectedItem.Text);
            string clgCode = Convert.ToString(ddlclg.SelectedValue);
            ArrayList arstageId = new ArrayList();

            SettingRights();
            if (!string.IsNullOrEmpty(routeid) && !string.IsNullOrEmpty(stageid))
            {
                string selQ = " select distinct sm.stage_id,sm.Stage_Name,rm.Rou_To,f.cost,f.paytype,f.college_code,rm.route_id,f.Month_Value from stage_master sm,routemaster rm ,feeinfo F where cast(rm.stage_name as varchar(100))=cast(sm.stage_id as varchar(100)) and f.StrtPlace=cast(sm.stage_id as varchar(100)) and f.StrtPlace= cast(rm.stage_name as varchar(100)) and f.college_code in('" + clgCode + "') and rm.route_id in ('" + routeid + "') and rm.stage_name in('" + stageid + "') and paytype='" + pattern + "' order by rm.route_id";
                DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                {
                    int rowCnt = 0;
                    bool boolcheck = false;
                    for (int crow = 0; crow < ddlclg.Items.Count; crow++)
                    {
                        bool clgName = false;
                        arstageId.Clear();
                        if (ddlclg.Items[crow].Selected)
                        {
                            collegecode = Convert.ToString(ddlclg.Items[crow].Value);
                            dsval.Tables[0].DefaultView.RowFilter = "college_code='" + collegecode + "'";
                            DataView dv = dsval.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                for (int row = 0; row < dv.Count; row++)
                                {
                                    string stageId = Convert.ToString(dv[row]["stage_id"]);
                                    //if (!arstageId.Contains(stageId))//purpose for dont repeat same stage multiple route id//
                                    //{
                                        //if (!boolcheck)
                                        //{
                                        //    Fp_Route.Sheets[0].RowCount++;
                                        //    Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 1].CellType = cball;
                                        //}
                                        //if (!clgName)
                                        //{
                                        //    Fp_Route.Sheets[0].RowCount++;
                                        //    Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ddlclg.Items[crow].Text);
                                        //    Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].BackColor = Color.Green;
                                        //    Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].ForeColor = Color.White;
                                        //    Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        //    Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        //    Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        //    Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        //    Fp_Route.Sheets[0].SpanModel.Add(Fp_Route.Sheets[0].RowCount - 1, 0, 1, 4);
                                        //    clgName = true;
                                        //}
                                        rowCnt++;
                                        Fp_Route.Sheets[0].RowCount++;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Tag = collegecode;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[row]["route_id"]);
                                        
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[row]["Stage_Name"]);
                                        string routeId = Convert.ToString(dv[row]["route_id"]);
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 2].Tag = stageId;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 2].Note = routeId;

                                        string endstagename = d2.GetFunction("select Stage_Name from stage_master where Stage_id='" + Convert.ToString(dv[row]["Rou_To"]) + "'");

                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 3].Text = endstagename;
                                        
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 3].Tag = stageId;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 3].Note = routeId;

                                        double costAmount = 0;
                                        double.TryParse(Convert.ToString(dv[row]["cost"]), out costAmount);
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(costAmount);
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 0].Locked = true;
                                        //  Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 1].Locked = true;                         
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[row]["paytype"]);
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 5].Locked = true;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 3].Locked = true;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 2].Locked = true;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 1].Locked = true;
                                        Fp_Route.Sheets[0].Cells[Fp_Route.Sheets[0].RowCount - 1, 4].Locked = true;
                                        boolcheck = true;
                                        arstageId.Add(stageId);
                                    //}
                                }
                                Fp_Route.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            }
                        }
                    }
                    if (boolcheck)
                    {
                        Fp_Route.Width = 970;
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

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fp_Route, reportname);
            }
            else
            {
                lblerror1.Text = "Please Enter Your Report Name";
                lblerror1.Visible = true;
            }

        }
        catch (Exception ex)
        {
            lblerror1.Text = ex.ToString();
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        if (Fp_Route.Sheets[0].RowCount > 0)
        {
            Session["column_header_row_count"] = 1;
            string degreedetails = "Cost Details";
            string pagename = "CostMasterReport.aspx";
            Printcontrol.loadspreaddetails(Fp_Route, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
    }


    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }


}