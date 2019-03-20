using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Drawing;

public partial class TransportReport : System.Web.UI.Page
{
    int count = 0;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strroute = string.Empty;
    string sqlstrroute = string.Empty;
    string strvechile = string.Empty;
    string sqlstrvechile = string.Empty;
    string strplace = string.Empty;
    string sqlstrplace = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    Hashtable hat = new Hashtable();
    string PageLogOut = "";

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

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
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (!IsPostBack)
        {

            Fptransport.Width = 1000;
            Fptransport.Sheets[0].AutoPostBack = true;
            Fptransport.CommandBar.Visible = true;
            Fptransport.Sheets[0].SheetName = " ";
            Fptransport.Sheets[0].SheetCorner.Columns[0].Visible = false;
            Fptransport.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            Fptransport.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
            Fptransport.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            Fptransport.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fptransport.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Fptransport.Sheets[0].DefaultStyle.Font.Bold = false;

            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = System.Drawing.Color.Black;
            Fptransport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fptransport.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fptransport.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            Fptransport.Sheets[0].AllowTableCorner = true;

            //---------------page number

            Fptransport.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            Fptransport.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            Fptransport.Pager.Align = HorizontalAlign.Right;
            Fptransport.Pager.Font.Bold = true;
            Fptransport.Pager.Font.Name = "Book Antiqua";
            Fptransport.Pager.ForeColor = System.Drawing.Color.DarkGreen;
            Fptransport.Pager.BackColor = System.Drawing.Color.Beige;
            Fptransport.Pager.BackColor = System.Drawing.Color.AliceBlue;
            Fptransport.Pager.PageCount = 100;

            Fptransport.Visible = false;
            //btnxl.Visible = false;
            //lblrptname.Visible = false;
            //txtexcelname.Visible = false;
            LabelE.Visible = false;
            lblnorec.Visible = false;
            errmsg.Visible = false;
            bindroute();
            bindvechile();
            bindplace();
            //btnprintmaster.Visible = false;

        }
    }

    protected void bindroute()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.Bindroute();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstroute.DataSource = ds2;
                chklstroute.DataTextField = "Route_ID";
                chklstroute.DataValueField = "Route_ID";
                chklstroute.DataBind();
                chklstroute.SelectedIndex = chklstroute.Items.Count - 1;
                for (int i = 0; i < chklstroute.Items.Count; i++)
                {
                    chklstroute.Items[i].Selected = true;
                    if (chklstroute.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstroute.Items.Count == count)
                    {
                        chkroute.Checked = true;
                    }

                }

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "TransportReport.aspx");
        }
    }

    protected void bindvechile()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindVechile();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstvechile.DataSource = ds2;
                chklstvechile.DataTextField = "Veh_ID";
                chklstvechile.DataValueField = "Veh_ID";
                chklstvechile.DataBind();
                chklstvechile.SelectedIndex = chklstvechile.Items.Count - 1;
                for (int i = 0; i < chklstvechile.Items.Count; i++)
                {
                    chklstvechile.Items[i].Selected = true;
                    if (chklstvechile.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstvechile.Items.Count == count)
                    {
                        chkvechile.Checked = true;
                    }

                }

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "TransportReport.aspx");
        }
    }

    protected void bindplace()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.Bindplace();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstplace.DataSource = ds2;
                chklstplace.DataTextField = "Stage_Name";
                chklstplace.DataValueField = "Stage_id";
                chklstplace.DataBind();
                chklstplace.SelectedIndex = chklstplace.Items.Count - 1;
                for (int i = 0; i < chklstplace.Items.Count; i++)
                {
                    chklstplace.Items[i].Selected = true;
                    if (chklstplace.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstvechile.Items.Count == count)
                    {
                        chkplace.Checked = true;
                    }

                }

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "TransportReport.aspx");
        }
    }

    protected void bindspread()
    {
        try
        {
            Hashtable htTotal = new Hashtable();
            string Allot = string.Empty;
            string Concession = string.Empty;
            string Total = string.Empty;
            string Receipt = string.Empty;
            string Balance = string.Empty;
            Fptransport.Visible = true;


            Fptransport.Sheets[0].RowCount = 0;
            Fptransport.Sheets[0].ColumnCount = 0;
            Fptransport.Sheets[0].ColumnHeader.Visible = true;
            Fptransport.Sheets[0].ColumnCount++;
            Fptransport.CommandBar.Visible = false;
            Fptransport.Sheets[0].AutoPostBack = true;
            Fptransport.Sheets[0].ColumnHeader.RowCount = 2;
            Fptransport.Sheets[0].ColumnCount = 15;
            Fptransport.Sheets[0].RowCount = 0;
            Fptransport.Sheets[0].PageSize = 8000;

            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Route";
            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Starting Place";
            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Bus No";
            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Registration No";
            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "No of Travellers";
            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 2);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "No of Students";
            Fptransport.Sheets[0].SpanModel.Add(0, 6, 1, 3);

            Fptransport.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Male";
            Fptransport.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Female";

            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 2);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 8].Text = "No of Staffs";

            Fptransport.Sheets[0].SpanModel.Add(0, 8, 1, 3);
            Fptransport.Sheets[0].ColumnHeader.Cells[1, 8].Text = "No";
            Fptransport.Sheets[0].ColumnHeader.Cells[1, 9].Text = "College";

            //========Added by Saranya on 09/01/2018====================//

            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Allot";
            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Concession";
            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Total";
            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 13, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Receipt";
            Fptransport.Sheets[0].ColumnHeaderSpanModel.Add(0, 14, 2, 1);
            Fptransport.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Balance";
            //==========================================================//


            Fptransport.Sheets[0].Columns[0].Width = 50;
            Fptransport.Sheets[0].Columns[1].Width = 50;
            Fptransport.Sheets[0].Columns[2].Width = 200;
            Fptransport.Sheets[0].Columns[3].Width = 100;
            Fptransport.Sheets[0].Columns[4].Width = 100;
            Fptransport.Sheets[0].Columns[5].Width = 80;
            Fptransport.Sheets[0].Columns[6].Width = 80;
            Fptransport.Sheets[0].Columns[7].Width = 80;
            Fptransport.Sheets[0].Columns[8].Width = 80;
            Fptransport.Sheets[0].Columns[9].Width = 80;
            //=====Added by Saranya on 09/01/2018========//
            Fptransport.Sheets[0].Columns[10].Width = 80;
            Fptransport.Sheets[0].Columns[11].Width = 80;
            Fptransport.Sheets[0].Columns[12].Width = 80;
            Fptransport.Sheets[0].Columns[13].Width = 80;
            Fptransport.Sheets[0].Columns[14].Width = 80;
            //==========================================//
            string strcmd = string.Empty;

            if (txtroute.Text != "---Select---" || chklstroute.Items.Count != null)
            {
                int itemcount = 0;


                for (itemcount = 0; itemcount < chklstroute.Items.Count; itemcount++)
                {
                    if (chklstroute.Items[itemcount].Selected == true)
                    {
                        if (strroute == "")
                            strroute = "'" + chklstroute.Items[itemcount].Value.ToString() + "'";
                        else
                            strroute = strroute + "," + "'" + chklstroute.Items[itemcount].Value.ToString() + "'";
                    }
                }
                if (strroute != "")
                {
                    strroute = " in(" + strroute + ")";
                    sqlstrroute = " and v.Route  " + strroute + "";

                }
                else
                {
                    sqlstrroute = "";
                }
            }


            if (txtvechile.Text != "---Select---" || chklstvechile.Items.Count != null)
            {
                int itemcount = 0;


                for (itemcount = 0; itemcount < chklstvechile.Items.Count; itemcount++)
                {
                    if (chklstvechile.Items[itemcount].Selected == true)
                    {
                        if (strvechile == "")
                            strvechile = "'" + chklstvechile.Items[itemcount].Value.ToString() + "'";
                        else
                            strvechile = strvechile + "," + "'" + chklstvechile.Items[itemcount].Value.ToString() + "'";
                    }
                }
                if (strvechile != "")
                {
                    strvechile = " in(" + strvechile + ")";
                    sqlstrvechile = " and v.Veh_ID  " + strvechile + "";
                }
                else
                {
                    sqlstrvechile = " ";

                }
            }


            if (txtplace.Text != "---Select---" || chklstplace.Items.Count != null)
            {
                int itemcount = 0;

                for (itemcount = 0; itemcount < chklstplace.Items.Count; itemcount++)
                {
                    if (chklstplace.Items[itemcount].Selected == true)
                    {
                        if (strplace == "")
                            strplace = "'" + chklstplace.Items[itemcount].Value.ToString() + "'";
                        else
                            strplace = strplace + "," + "'" + chklstplace.Items[itemcount].Value.ToString() + "'";
                    }
                }
                if (strplace != "")
                {
                    strplace = " in(" + strplace + ")";
                    sqlstrplace = " and Stage_id " + strplace + "";
                }
                else
                {
                    sqlstrplace = "";
                }

            }

            ds2.Dispose();
            ds2.Reset();
            // ds2 = d2.Bindtransport(sqlstrroute, sqlstrvechileReg_No, sqlstrplace);//Hiiden By Srinath 18/3/2013
            //string transportquery = "SELECT Route_Name,route_id,s.Stage_Name,R.Veh_ID,len(R.Veh_ID) as vech,NofTravrs FROM RouteMaster R,Vehicle_Master V ,stage_master s WHERE R.Route_ID = V.Route and cast(r.stage_name as varchar(100))=cast(s.stage_id as varchar(100)) AND Sess = 'M' AND Arr_Time = 'Halt'  " + sqlstrroute + " " + sqlstrvechile + " " + sqlstrplace + " order by len(R.Veh_ID) asc";//Modified By Srinath 12/6/2014

            //string transportquery = "SELECT distinct Route_Name,route_id,v.Reg_No,s.Stage_Name,R.Veh_ID,len(R.Veh_ID) as vech,NofTravrs FROM RouteMaster R,Vehicle_Master V ,stage_master s WHERE R.Route_ID = V.Route and cast(r.stage_name as varchar(100))=cast(s.stage_id as varchar(100)) AND Sess = 'M' AND Arr_Time = 'Halt'  " + sqlstrroute + " " + sqlstrvechile + " " + sqlstrplace + " order by len(R.Veh_ID) asc";

            string transportquery = "SELECT distinct Route_Name,route_id,v.Reg_No,s.Stage_Name,R.Veh_ID,len(R.Veh_ID) as vech,NofTravrs FROM RouteMaster R,Vehicle_Master V ,stage_master s WHERE R.Route_ID = V.Route and cast(r.stage_name as varchar(100))=cast(s.stage_id as varchar(100)) AND Sess = 'M'   " + sqlstrroute + " " + sqlstrvechile + " " + sqlstrplace + " order by len(R.Veh_ID) asc";//rajasekar4
            ds2 = d2.select_method(transportquery, hat, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                int travellers_TotCount = 0;
                for (int rolcount = 0; rolcount < ds2.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    string Routename = ds2.Tables[0].Rows[rolcount]["Route_Name"].ToString();
                    string stagename = ds2.Tables[0].Rows[rolcount]["Stage_Name"].ToString();
                    string vechileid = ds2.Tables[0].Rows[rolcount]["Veh_ID"].ToString();
                    string regno = ds2.Tables[0].Rows[rolcount]["Reg_No"].ToString();//Added By SRinath 12/6/2014
                    //Modified By SRinath 8/10/2013
                    string route = ds2.Tables[0].Rows[rolcount]["Route_Id"].ToString();
                    //string route = d2.GetFunction("select route_id from routemaster where route_name='" + Routename + "' group by route_id");
                    // string travellers = ds2.Tables[0].Rows[rolcount]["NofTravrs"].ToString();

                    int travellers = 0;
                    int malecount = 0;
                    int femalecount = 0;
                    int staffno = 0;
                    string staffcollege = string.Empty;

                    string strcount = "SELECT M.TotM,F.TotF FROM  (SELECT Count(*) TotM FROM Registration R,Applyn A WHERE R.App_No = A.App_No AND VehID <> '' AND Bus_RouteID <> '' AND Bus_RouteId = '" + route + "' AND A.Sex = 0) M, (SELECT Count(*) TotF FROM Registration R,Applyn A WHERE R.App_No = A.App_No AND VehID <> '' AND Bus_RouteID <> '' AND Bus_RouteId = '" + route + "' AND A.Sex = 1) F";
                    DataSet dsload = d2.select_method(strcount, hat, "Text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        malecount = Convert.ToInt32(dsload.Tables[0].Rows[0]["TotM"].ToString());
                        femalecount = Convert.ToInt32(dsload.Tables[0].Rows[0]["TotF"].ToString());
                    }
                    string strstaff = "SELECT COUNT(*) TotStf,Acr FROM StaffMaster M,CollInfo C WHERE M.College_Code = C.College_Code AND VehID <> '' AND Bus_RouteID <> '' AND Bus_RouteID = '" + route + "' GROUP BY Acr ";
                    dsload.Reset();
                    dsload.Dispose();
                    dsload = d2.select_method(strstaff, hat, "Text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        for (int count = 0; count < dsload.Tables[0].Rows.Count; count++)
                        {
                            int staff = Convert.ToInt32(dsload.Tables[0].Rows[count]["TotStf"].ToString());
                            if (staff != 0)
                            {
                                staffno = staff + staffno;
                            }
                            if (staffcollege == "")
                            {
                                staffcollege = dsload.Tables[0].Rows[count]["Acr"].ToString();
                            }
                            else
                            {
                                staffcollege = staffcollege + ',' + dsload.Tables[0].Rows[count]["Acr"].ToString();
                            }
                        }
                    }
                    travellers = staffno + malecount + femalecount;

                    //==================Added By saranya on 09/01/2018=============================//

                    string HeaderFK = string.Empty;
                    string LedgerFK = string.Empty;
                    string hdFK = string.Empty;
                    string ldFK = string.Empty;
                    string selQ = " select LinkValue from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "'";
                    DataSet dsVal1 = d2.select_method_wo_parameter(selQ, "Text");
                    if (dsVal1.Tables.Count > 0 && dsVal1.Tables[0].Rows.Count > 0)
                    {
                        for (int row1 = 0; row1 < dsVal1.Tables[0].Rows.Count; row1++)
                        {
                            string linkValue = Convert.ToString(dsVal1.Tables[0].Rows[row1]["LinkValue"]);
                            //string clgcode = Convert.ToString(dsVal1.Tables[0].Rows[row1]["college_code"]);
                            string[] leng = linkValue.Split(',');
                            if (leng.Length == 2)
                            {
                                hdFK = Convert.ToString(leng[0]);
                                HeaderFK += "'" + "," + "'" + hdFK;
                                ldFK = Convert.ToString(leng[1]);
                                LedgerFK += "'" + "," + "'" + ldFK;
                            }
                        }
                    }
                    string selq = "select distinct App_No,staff_appl_id from Registration,staff_appl_master where Bus_RouteId = '" + route + "' and VehID='" + vechileid + "'";
                    DataSet ds = d2.select_method_wo_parameter(selq, "Text");
                    string App_No = string.Empty;
                    string appno = string.Empty;
                    string Staffappno = string.Empty;
                    string Staff_applId = string.Empty;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        appno = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);
                        Staffappno = Convert.ToString(ds.Tables[0].Rows[row]["staff_appl_id"]);
                        if (App_No == "")
                        {
                            App_No = appno;
                        }
                        else
                        {
                            App_No += "'" + "," + "'" + appno;
                        }
                        if (Staff_applId == "")
                        {
                            Staff_applId = Staffappno;
                        }
                        else
                        {
                            Staff_applId += "'" + "," + "'" + Staffappno;
                        }


                    }

                    //string selq1 = "select SUM(FeeAmount)as Allot,SUM(isnull(DeductAmout,'0'))as Concession,SUM(TotalAmount)as Total,SUM(PaidAmount)as Receipt,SUM(BalAmount)as Balance  from FT_FeeAllot where App_No in ('" + App_No + "') and HeaderFk in('" + HeaderFK + "') and LedgerFk in('" + LedgerFK + "')";//,'" + Staff_applId + "'
                    string selq1 = "select SUM(FeeAmount)as Allot,SUM(isnull(DeductAmout,'0'))as Concession,SUM(TotalAmount)as Total,SUM(PaidAmount)as Receipt,SUM(BalAmount)as Balance  from FT_FeeAllot where cast (App_No as varchar(100)) in ('') and HeaderFk in('" + HeaderFK + "') and LedgerFk in('" + LedgerFK + "')";//rajasekar4
                    DataSet ds1 = d2.select_method_wo_parameter(selq1, "Text");
                    for (int row = 0; row < ds1.Tables[0].Rows.Count; row++)
                    {
                        Allot = Convert.ToString(ds1.Tables[0].Rows[row]["Allot"]);
                        Concession = Convert.ToString(ds1.Tables[0].Rows[row]["Concession"]);
                        Total = Convert.ToString(ds1.Tables[0].Rows[row]["Total"]);
                        Receipt = Convert.ToString(ds1.Tables[0].Rows[row]["Receipt"]);
                        Balance = Convert.ToString(ds1.Tables[0].Rows[row]["Balance"]);
                    }
                    //==============================================================================================//   

                    Fptransport.Sheets[0].RowCount = Fptransport.Sheets[0].RowCount + 1;
                    Fptransport.Sheets[0].Rows[Fptransport.Sheets[0].RowCount - 1].Font.Bold = false;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 1].Text = route;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 2].Text = stagename;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 3].Text = vechileid;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 4].Text = regno;//Added by srinath 12/6/2014
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 5].Text = travellers.ToString();
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                    //========================Added By saranya on 09/01/2018==============================//

                    //Travellers TotalCount
                    travellers_TotCount = travellers;
                    if (!htTotal.ContainsKey(5))
                        htTotal.Add(5, Convert.ToString(travellers_TotCount));
                    else
                    {
                        double TravelersCount = 0;
                        double.TryParse(Convert.ToString(htTotal[5]), out TravelersCount);
                        TravelersCount += Convert.ToDouble(travellers_TotCount);
                        htTotal.Remove(5);
                        htTotal.Add(5, Convert.ToString(TravelersCount));
                    }

                    //Male TotalCount
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 6].Text = malecount.ToString();
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    if (!htTotal.ContainsKey(6))
                        htTotal.Add(6, Convert.ToString(malecount));
                    else
                    {
                        double MaleCount = 0;
                        double.TryParse(Convert.ToString(htTotal[6]), out MaleCount);
                        MaleCount += Convert.ToDouble(malecount);
                        htTotal.Remove(6);
                        htTotal.Add(6, Convert.ToString(MaleCount));
                    }

                    //Female TotalCount
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 7].Text = femalecount.ToString();
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    if (!htTotal.ContainsKey(7))
                        htTotal.Add(7, Convert.ToString(femalecount));
                    else
                    {
                        double FemaleCount = 0;
                        double.TryParse(Convert.ToString(htTotal[7]), out FemaleCount);
                        FemaleCount += Convert.ToDouble(femalecount);
                        htTotal.Remove(7);
                        htTotal.Add(7, Convert.ToString(FemaleCount));
                    }

                    //Staff TotalCount
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 8].Text = staffno.ToString();
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    if (!htTotal.ContainsKey(8))
                        htTotal.Add(8, Convert.ToString(staffno));
                    else
                    {
                        double StaffCount = 0;
                        double.TryParse(Convert.ToString(htTotal[8]), out StaffCount);
                        StaffCount += Convert.ToDouble(staffno);
                        htTotal.Remove(8);
                        htTotal.Add(8, Convert.ToString(StaffCount));
                    }

                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 9].Text = staffcollege;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;



                    //Allot Total
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 10].Text = Allot;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                    if (!htTotal.ContainsKey(10))
                        htTotal.Add(10, Convert.ToString(Allot));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[10]), out amount);
                        amount += Convert.ToDouble(Allot);
                        htTotal.Remove(10);
                        htTotal.Add(10, Convert.ToString(amount));
                    }

                    //Concession Total
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 11].Text = Concession;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                    if (!htTotal.ContainsKey(11))
                        htTotal.Add(11, Convert.ToString(Concession));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[11]), out amount);
                        amount += Convert.ToDouble(Concession);
                        htTotal.Remove(11);
                        htTotal.Add(11, Convert.ToString(amount));
                    }

                    //Total 
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 12].Text = Total;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                    if (!htTotal.ContainsKey(12))
                        htTotal.Add(12, Convert.ToString(Total));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[12]), out amount);
                        amount += Convert.ToDouble(Total);
                        htTotal.Remove(12);
                        htTotal.Add(12, Convert.ToString(amount));
                    }

                    //Receipt Total
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 13].Text = Receipt;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                    if (!htTotal.ContainsKey(13))
                        htTotal.Add(13, Convert.ToString(Receipt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[13]), out amount);
                        amount += Convert.ToDouble(Receipt);
                        htTotal.Remove(13);
                        htTotal.Add(13, Convert.ToString(amount));
                    }

                    //Balance Total
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 14].Text = Balance;
                    Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;
                    if (!htTotal.ContainsKey(14))
                        htTotal.Add(14, Convert.ToString(Balance));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[14]), out amount);
                        amount += Convert.ToDouble(Balance);
                        htTotal.Remove(14);
                        htTotal.Add(14, Convert.ToString(amount));
                    }

                }

                //Grand Total
                int row_Count = 0;
                Fptransport.Sheets[0].Rows.Count++;
                row_Count = Fptransport.Sheets[0].Rows.Count - 1;
                Fptransport.Sheets[0].Cells[row_Count, 0].Text = "Grand Total";
                Fptransport.Sheets[0].SpanModel.Add(row_Count, 0, 1, 5);
                Fptransport.Sheets[0].Rows[row_Count].BackColor = Color.Green;
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(htTotal[5]);
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(htTotal[6]);
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(htTotal[7]);
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(htTotal[8]);
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(htTotal[10]);
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(htTotal[11]);
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(htTotal[12]);
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(htTotal[13]);
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(htTotal[14]);
                Fptransport.Sheets[0].Cells[Fptransport.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;

                //=====================================================================================================//
            }

        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "TransportReport.aspx");
        }
    }
    #region Added by saranya on 10/01/2018
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            // string degreedetails = "Student Fee Status";
            //string collegeName = Convert.ToString(ddl_collegename.SelectedItem.Text);
            //string degreedetails = collegeName + "\nTransportReport" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");
            string degreedetails = string.Empty;
            string pagename = "TransportReport.aspx";
            Printcontrol.loadspreaddetails(Fptransport, pagename, degreedetails);
            Printcontrol.Visible = true;
            lblsmserror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "TransportReport.aspx");
        }
    }
    #endregion
    protected void btngo_Click(object sender, EventArgs e)
    {
        bindspread();
        //btnxl.Visible = true;
        //lblrptname.Visible = true;
        //txtexcelname.Visible = true;
        //btnprintmaster.Visible = true;
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        string reporname = txtexcelname.Text.Trim();
        if (reporname != "")
        {
            d2.printexcelreport(Fptransport, reporname);
        }
    }
    protected void chklstroute_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Vechile filter
        string clg = "";
        int commcount = 0;
        int commcount2 = 0;
        for (int i = 0; i < chklstroute.Items.Count; i++)
        {
            if (chklstroute.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                txtroute.Text = "Route(" + commcount.ToString() + ")";
                if (clg == "")
                {
                    clg = chklstroute.Items[i].Value.ToString();
                }
                else
                {
                    clg = clg + "','" + chklstroute.Items[i].Value;
                }
            }
        }
        if (clg != "")
        {
            clg = " where route in('" + clg + "')";
        }
        if (commcount == 0)
        {
            txtroute.Text = "--Select--";
        }
        DataSet ds = new DataSet();
        string st = "Select Distinct Veh_ID from Vehicle_Master " + clg;
        ds = d2.select_method(st, hat, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            chklstvechile.DataSource = ds;
            chklstvechile.DataTextField = "Veh_ID";
            chklstvechile.DataValueField = "Veh_ID";
            chklstvechile.DataBind();
        }
        for (int i = 0; i < chklstvechile.Items.Count; i++)
        {
            //if (chklstroute.Items[i].Selected == true)
            //{
            commcount2 = commcount2 + 1;
            txtvechile.Text = "Vechile(" + commcount2.ToString() + ")";
            //}
        }
        if (commcount2 == 0)
        {
            txtvechile.Text = "--Select--";
        }
        //place Filter

        string clplace = "";
        int commcount1 = 0;
        for (int i = 0; i < chklstroute.Items.Count; i++)
        {
            if (chklstroute.Items[i].Selected == true)
            {
                commcount1 = commcount1 + 1;
                txtplace.Text = "Place(" + commcount1.ToString() + ")";
                if (clplace == "")
                {
                    clplace = chklstroute.Items[i].Value.ToString();
                }
                else
                {
                    clplace = clplace + "','" + chklstroute.Items[i].Value;
                }
            }
        }
        if (clplace != "")
        {
            clplace = " and route_id in('" + clplace + "')";
        }
        if (commcount1 == 0)
        {
            txtplace.Text = "--Select--";
        }
        DataSet dsplace = new DataSet();
        //string stplace = "Select Distinct s.Stage_Name,s.stage_id from RouteMaster r ,stage_master s WHERE r.stage_name=s.stage_id and Sess = 'M' AND Arr_Time = 'Halt' " + clplace;
        string stplace = "Select Distinct s.Stage_Name,s.stage_id from RouteMaster r ,stage_master s WHERE Sess = 'M' " + clplace;//rajasekar4
        dsplace = d2.select_method(stplace, hat, "Text");
        if (dsplace.Tables[0].Rows.Count > 0)
        {
            chklstplace.DataSource = dsplace;
            chklstplace.DataTextField = "Stage_Name";
            chklstplace.DataValueField = "stage_id";
            chklstplace.DataBind();
        }
    }
    protected void chkroute_ChekedChange(object sender, EventArgs e)
    {
        if (chkroute.Checked == true)
        {
            for (int i = 0; i < chklstroute.Items.Count; i++)
            {
                chklstroute.Items[i].Selected = true;
                txtroute.Text = "Route(" + (chklstroute.Items.Count) + ")";
            }
            bindvechile();
            for (int i = 0; i < chklstvechile.Items.Count; i++)
            {
                chklstvechile.Items[i].Selected = true;
                txtvechile.Text = "Vechile(" + (chklstvechile.Items.Count) + ")";
            }
            bindplace();
            for (int i = 0; i < chklstplace.Items.Count; i++)
            {
                chklstplace.Items[i].Selected = true;
                txtplace.Text = "Place(" + (chklstplace.Items.Count) + ")";
            }

        }
        else
        {
            for (int i = 0; i < chklstroute.Items.Count; i++)
            {
                chklstroute.Items[i].Selected = false;
                txtroute.Text = "---Select---";
            }
            for (int i = 0; i < chklstvechile.Items.Count; i++)
            {
                chklstvechile.Items[i].Selected = false;
                txtvechile.Text = "---Select---";
            }
            for (int i = 0; i < chklstplace.Items.Count; i++)
            {
                chklstplace.Items[i].Selected = false;
                txtplace.Text = "---Select---";
            }
        }
    }
    protected void chklstvechile_SelectedIndexChanged(object sender, EventArgs e)
    {

        string clg = "";
        int commcount = 0;
        for (int i = 0; i < chklstvechile.Items.Count; i++)
        {
            if (chklstvechile.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                txtvechile.Text = "Vechile(" + commcount.ToString() + ")";
                if (clg == "")
                {
                    clg = chklstvechile.Items[i].Value.ToString();
                }
                else
                {
                    clg = clg + "','" + chklstvechile.Items[i].Value;
                }
            }
        }

    }
    protected void chkvechile_CheckedChanged(object sender, EventArgs e)
    {
        if (chkvechile.Checked == true)
        {
            for (int i = 0; i < chklstvechile.Items.Count; i++)
            {
                chklstvechile.Items[i].Selected = true;
                txtvechile.Text = "Vechile(" + (chklstvechile.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstvechile.Items.Count; i++)
            {
                chklstvechile.Items[i].Selected = false;
                txtvechile.Text = "---Select---";
            }
        }
    }
    protected void chklstplace_SelectedIndexChanged(object sender, EventArgs e)
    {

        string clg = "";
        int commcount = 0;
        for (int i = 0; i < chklstplace.Items.Count; i++)
        {
            if (chklstplace.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                txtplace.Text = "Place(" + commcount.ToString() + ")";
                if (clg == "")
                {
                    clg = chklstplace.Items[i].Value.ToString();
                }
                else
                {
                    clg = clg + "','" + chklstplace.Items[i].Value;
                }
            }
        }
    }
    protected void chkplace_CheckedChanged(object sender, EventArgs e)
    {
        if (chkplace.Checked == true)
        {
            for (int i = 0; i < chklstplace.Items.Count; i++)
            {
                chklstplace.Items[i].Selected = true;
                txtplace.Text = "Place(" + (chklstplace.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstplace.Items.Count; i++)
            {
                chklstplace.Items[i].Selected = false;
                txtplace.Text = "---Select---";
            }
        }
    }
}
