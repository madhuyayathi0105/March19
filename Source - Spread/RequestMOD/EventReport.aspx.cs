using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class EventReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    static ArrayList ItemList_Event = new ArrayList();
    static ArrayList Itemindex_Event = new ArrayList();
    static ArrayList ItemList_Event1 = new ArrayList();
    static ArrayList Itemindex_Event1 = new ArrayList();
    static string deptcode = "";
    Boolean cellclick = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("ReqM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Request"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("RequestHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/RequestMOD/RequestHome.aspx");
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
                BindCollege();
                bindbranch();
                EventType();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                deptcode = rs.GetSelectedItemsValueAsString(cbl_branch);
                pheaderfilter0.Visible = false;
                pcolumnorder0.Visible = false;
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
            }
        }
        catch (Exception ex) { }
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_branch.Checked == true)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            deptcode = rs.GetSelectedItemsValueAsString(cbl_branch);
        }
        catch
        {
        }
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_branch.Items.Count)
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }

            else
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
            }
            deptcode = rs.GetSelectedItemsValueAsString(cbl_branch);
        }
        catch
        {
        }
    }
    public void cb_evetype_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_evetype, cb1_evetype, txt_evetype, "Event Type", "--Select--");
    }
    public void cb1_evetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_evetype, cb1_evetype, txt_evetype, "Event Type", "--Select--");
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
    void BindCollege()
    {
        try
        {
            string srisql = "select collname,college_code from collinfo";
            ds.Clear();
            ds = d2.select_method_wo_parameter(srisql, "Text");
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
        catch
        {
        }
    }
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        deptcode = rs.GetSelectedItemsValueAsString(cbl_branch);
    }
    public void bindbranch()
    {
        try
        {
            cb_branch.Checked = false;
            string commname = "";

            commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "'";

            ds.Clear();
            cbl_branch.Items.Clear();
            ds = d2.select_method(commname, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
                if (cbl_branch.Items.Count > 0)
                {
                    //    for (int i = 0; i < cbl_branch.Items.Count; i++)
                    //    {
                    cbl_branch.Items[0].Selected = true;
                }
                txt_branch.Text = "Branch(" + 1 + ")";
                //}

            }


        }
        catch (Exception ex)
        {
        }
    }
    public void EventType()
    {
        try
        {
            cb1_evetype.Items.Clear();
            string q = "select MasterValue,MasterCode from CO_MasterValues where MasterCriteria='EventName' and MasterValue<>''";
            ds = d2.select_method_wo_parameter(q, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                cb1_evetype.DataSource = ds;
                cb1_evetype.DataTextField = "MasterValue";
                cb1_evetype.DataValueField = "MasterCode";
                cb1_evetype.DataBind();
            }
            for (int ii = 0; ii < cb1_evetype.Items.Count; ii++)
            {
                cb1_evetype.Items[ii].Selected = true;
            }
            txt_evetype.Text = "Event Type(" + cb1_evetype.Items.Count + ")";
            cb_evetype.Checked = true;

        }
        catch (Exception ex)
        { }
    }
    public void cb_date_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_date.Checked == true)
        {
            txt_fromdate.Enabled = true;
            txt_todate.Enabled = true;
        }
        else
        {
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
        }
    }
    public void btn_go_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (txt_staffname.Text.Trim() != "" || txt_studname.Text.Trim() != "")
            {

                staffstudgo();
            }
            else
            {

                go();
            }
        }
        catch
        {
        }
    }

    public void staffstudgo()
    {
        try
        {

            int count = 0;
            string Apl_id = "";
            string query = "";
            Fpspread1.Sheets[0].Visible = true;
            pheaderfilter0.Visible = false;
            pcolumnorder0.Visible = false;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].ColumnCount = 1;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            string[] ay = txt_fromdate.Text.Split('/');
            string[] ay1 = txt_todate.Text.Split('/');
            deptcode = rs.GetSelectedItemsValueAsString(cbl_branch);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();

            dt = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
            dt1 = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
            string adddate = "";
            if (cb_date.Checked == true)
            {
                adddate = "  and  rd.eventdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
            }
            string eventname = "";
            eventname = rs.GetSelectedItemsValueAsString(cb1_evetype);
            if (txt_staffname.Text.Trim() != "")
            {
                Apl_id = d2.GetFunction("select appl_id from staff_appl_master where appl_name='" + txt_staffname.Text + "'");
            }
            else
            {
                Apl_id = d2.GetFunction("select app_no from applyn where stud_name='" + txt_staffname.Text + "'");
            }
            query = "select (Select MasterValue FROM CO_MasterValues T WHERE memberaction = T.MasterCode) memberaction, case when ActionType='1' then 'Participant' when ActionType='2' then 'Presented' else 'Organizer' end as ActionType ,(Select MasterValue FROM CO_MasterValues T WHERE ra.ActionName = T.MasterCode) ActionName,ACtionDesc, case when ed.MemType='0' then 'Student' else 'Staff' end as MemType,requestcode,ApplNo,CONVERT(VARCHAR(11),rd.eventdate,103) eventdate,NoOfAction,rd.OutdoorLoc,StartPeriod,EndPeriod,ra.StartTime,ra.EndTime,case when ra.LocationType='0' then'Indoor' else 'Outdoor' end as LocationType,RequisitionPK,(Select MasterValue FROM CO_MasterValues T WHERE R.ReqEventName = T.MasterCode)ReqEventName,CONVERT(VARCHAR(11),requestdate,103) requestdate from RQ_EventMemberDet ed,RQ_Requisition r,RQ_ReqActionDet ra ,RQ_ReqEventDet re,RQ_RequisitionDet rd where ed.RequisitionFK =r.RequisitionPK and ed.ActionFK =ra.ActionPK and ed.RequisitionFK=ra.RequisitionFK and ed.RequisitionFK =rd.RequisitionFK and r.RequisitionPK =ra.RequisitionFK and r.RequisitionPK=rd.RequisitionFK and ra.RequisitionFK =rd.RequisitionFK  and rd.RequisitionFK=ra.RequisitionFK and rd.RequisitionFK=re.RequisitionFK and ApplNo ='" + Apl_id + "' and r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.ReqEventName in('" + eventname + "') " + adddate + "";

            if (query == "")
            {
                Fpspread1.Sheets[0].Visible = false;
                lbl_err_item.Visible = true;
                lbl_err_item.Text = "Kindly Select All List ";
                div_report.Visible = false;
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
            }
            else
            {
                if (query != "")
                {
                    ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread1.Sheets[0].Visible = false;
                        Fpspread1.Visible = false;
                        lbl_err_item.Visible = true;
                        lbl_err_item.Text = "No Records Found";
                        pheaderfilter0.Visible = false;
                        pcolumnorder0.Visible = false;
                        div_report.Visible = false;
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lbl_err_item.Visible = false;
                            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                            {
                                if (cblcolumnorder.Items[i].Selected == true)
                                {
                                    hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);

                                    string colvalue = cblcolumnorder.Items[i].Text;
                                    if (ItemList_Event1.Contains(colvalue) == false)
                                    {
                                        ItemList_Event1.Add(cblcolumnorder.Items[i].Text);

                                    }
                                    tborder.Text = "";
                                    for (int j = 0; j < ItemList_Event1.Count; j++)
                                    {
                                        tborder.Text = tborder.Text + ItemList_Event1[j].ToString();

                                        tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";

                                    }
                                }
                                cblcolumnorder.Items[0].Enabled = false;
                            }


                            if (ItemList_Event1.Count == 0)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    cblcolumnorder.Items[i].Selected = true;
                                    hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                                    string colvalue = cblcolumnorder.Items[i].Text;
                                    if (ItemList_Event1.Contains(colvalue) == false)
                                    {
                                        ItemList_Event1.Add(cblcolumnorder.Items[i].Text);

                                    }
                                    tborder.Text = "";
                                    for (int j = 0; j < ItemList_Event1.Count; j++)
                                    {
                                        tborder.Text = tborder.Text + ItemList_Event1[j].ToString();

                                        tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";

                                    }
                                }
                            }

                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;


                            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                            style2.Font.Size = 14;
                            style2.Font.Name = "Book Antiqua";
                            style2.Font.Bold = true;
                            style2.HorizontalAlign = HorizontalAlign.Center;
                            style2.ForeColor = Color.Black;
                            style2.BackColor = Color.AliceBlue;

                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            for (int i = 0; i < ItemList_Event1.Count; i++)
                            {
                                string value1 = ItemList_Event1[i].ToString();
                                int a = value1.Length;
                                Fpspread1.Sheets[0].ColumnCount++;

                                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = ItemList_Event1[i].ToString();
                                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;


                            }

                            Fpspread1.Sheets[0].RowCount = 0;


                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {


                                Fpspread1.Sheets[0].RowCount++;
                                count++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["RequisitionPK"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                int c = 0;
                                string appstage = "";
                                if (ItemList_Event1.Count > 0 && hat.Count > 0)
                                {
                                    if (ItemList_Event1.Count == hat.Count)
                                    {
                                        for (int j = 0; j < ItemList_Event1.Count; j++)
                                        {
                                            string k = Convert.ToString(ItemList_Event1[j].ToString());
                                            string names = Convert.ToString(hat[k].ToString());
                                            c++;
                                            string val = ds.Tables[0].Rows[i][names].ToString();

                                            if (val != "")
                                            {

                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][names].ToString();
                                                if (names == "StartTime" || names == "EndTime" || names == "StartPeriod" || names == "EndPeriod" || names == "NoOfAction")
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 80;
                                                }
                                                else if (names == "OutdoorLoc")
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 350;
                                                }
                                                else if (names == "ReqEventName")
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 350;
                                                }
                                                else
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 150;
                                                }
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].VerticalAlign = VerticalAlign.Middle;
                                            }




                                        }

                                    }
                                }
                            }

                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            for (int k = 0; k < Fpspread1.Sheets[0].Columns.Count; k++)
                            {
                                Fpspread1.Sheets[0].SetColumnMerge(k, FarPoint.Web.Spread.Model.MergePolicy.Always);

                            }
                            if (CheckBox_column.Checked == true)
                            {
                                Fpspread1.Width = 1000;
                                Fpspread1.Height = 420;
                            }
                            else
                            {
                                Fpspread1.Width = 850;
                                Fpspread1.Height = 420;
                            }


                            pheaderfilter.Visible = true;
                            pcolumnorder.Visible = true;
                            pheaderfilter0.Visible = false;
                            pcolumnorder0.Visible = false;
                            Fpspread1.Visible = true;
                            div_report.Visible = true;

                        }

                    }
                }
            }

        }
        catch
        {
        }
    }
    public void go()
    {
        try
        {
            int count = 0;
            string query = "";
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            Fpspread1.Sheets[0].Visible = true;
            pheaderfilter0.Visible = true;
            pcolumnorder0.Visible = true;

            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;

            Fpspread1.Sheets[0].AutoPostBack = true;
            Fpspread1.Sheets[0].RowCount = 0;

            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].ColumnCount = 1;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            string[] ay = txt_fromdate.Text.Split('/');
            string[] ay1 = txt_todate.Text.Split('/');
            deptcode = rs.GetSelectedItemsValueAsString(cbl_branch);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();

            dt = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
            dt1 = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
            string adddate = "";
            if (cb_date.Checked == true)
            {
                adddate = "  and  eventdate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
            }
            string eventname = "";
            eventname = rs.GetSelectedItemsValueAsString(cb1_evetype);


            if (txt_eventname.Text.Trim() != "")
            {
                string eventnmetxt = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='EventName' and MasterValue='" + txt_eventname.Text + "'");

                query = "select CONVERT(VARCHAR(11),eventdate,103) eventdate,NoOfAction, OutdoorLoc,StartPeriod,EndPeriod,StartTime,EndTime,case when LocationType='0' then'Indoor' else 'Outdoor' end as LocationType,RequisitionPK,(Select MasterValue FROM CO_MasterValues T WHERE R.ReqEventName = T.MasterCode) ReqEventName,CONVERT(VARCHAR(11),requestdate,103) requestdate from RQ_Requisition r ,RQ_RequisitionDet rd,RQ_ReqEventDet ed  where r.RequisitionPK =rd.RequisitionFK and r.RequestType ='7' and ed.RequisitionFK =r.RequisitionPK and ed.RequisitionFK =rd.RequisitionFK  and r.ReqEventName in ('" + eventnmetxt + "')";
            }

            else
            {
                query = "select CONVERT(VARCHAR(11),eventdate,103) eventdate,NoOfAction, OutdoorLoc,StartPeriod,EndPeriod,StartTime,EndTime,case when LocationType='0' then'Indoor' else 'Outdoor' end as LocationType,RequisitionPK,(Select MasterValue FROM CO_MasterValues T WHERE R.ReqEventName = T.MasterCode) ReqEventName,CONVERT(VARCHAR(11),requestdate,103) requestdate from RQ_Requisition r ,RQ_RequisitionDet rd,RQ_ReqEventDet ed  where r.RequisitionPK =rd.RequisitionFK and r.RequestType ='7' and ed.RequisitionFK =r.RequisitionPK and ed.RequisitionFK =rd.RequisitionFK  and r.ReqEventName in ('" + eventname + "') " + adddate + "";
            }

            if (query == "")
            {
                Fpspread1.Sheets[0].Visible = false;
                lbl_err_item.Visible = true;
                lbl_err_item.Text = "Kindly Select All List ";
                div_report.Visible = false;
            }
            else
            {
                if (query != "")
                {
                    ds = d2.select_method(query, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        Fpspread1.Sheets[0].Visible = false;
                        Fpspread1.Visible = false;
                        lbl_err_item.Visible = true;
                        lbl_err_item.Text = "No Records Found";
                        pheaderfilter0.Visible = false;
                        pcolumnorder0.Visible = false;
                        div_report.Visible = false;
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lbl_err_item.Visible = false;
                            for (int i = 0; i < cblcolumnorder0.Items.Count; i++)
                            {
                                if (cblcolumnorder0.Items[i].Selected == true)
                                {
                                    hat.Add(cblcolumnorder0.Items[i].Text, cblcolumnorder0.Items[i].Value);

                                    string colvalue = cblcolumnorder0.Items[i].Text;
                                    if (ItemList_Event.Contains(colvalue) == false)
                                    {
                                        ItemList_Event.Add(cblcolumnorder0.Items[i].Text);

                                    }
                                    tborder0.Text = "";
                                    for (int j = 0; j < ItemList_Event.Count; j++)
                                    {
                                        tborder0.Text = tborder0.Text + ItemList_Event[j].ToString();

                                        tborder0.Text = tborder0.Text + "(" + (j + 1).ToString() + ")  ";

                                    }
                                }
                                cblcolumnorder0.Items[0].Enabled = false;
                            }


                            if (ItemList_Event.Count == 0)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    cblcolumnorder0.Items[i].Selected = true;
                                    hat.Add(cblcolumnorder0.Items[i].Text, cblcolumnorder0.Items[i].Value);
                                    string colvalue = cblcolumnorder0.Items[i].Text;
                                    if (ItemList_Event.Contains(colvalue) == false)
                                    {
                                        ItemList_Event.Add(cblcolumnorder0.Items[i].Text);

                                    }
                                    tborder0.Text = "";
                                    for (int j = 0; j < ItemList_Event.Count; j++)
                                    {
                                        tborder0.Text = tborder0.Text + ItemList_Event[j].ToString();

                                        tborder0.Text = tborder0.Text + "(" + (j + 1).ToString() + ")  ";

                                    }
                                }
                            }

                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;


                            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                            style2.Font.Size = 14;
                            style2.Font.Name = "Book Antiqua";
                            style2.Font.Bold = true;
                            style2.HorizontalAlign = HorizontalAlign.Center;
                            style2.ForeColor = Color.Black;
                            style2.BackColor = Color.AliceBlue;

                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            for (int i = 0; i < ItemList_Event.Count; i++)
                            {
                                string value1 = ItemList_Event[i].ToString();
                                int a = value1.Length;
                                Fpspread1.Sheets[0].ColumnCount++;

                                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = ItemList_Event[i].ToString();
                                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;


                            }

                            Fpspread1.Sheets[0].RowCount = 0;


                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {


                                Fpspread1.Sheets[0].RowCount++;
                                count++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["RequisitionPK"]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                int c = 0;
                                string appstage = "";
                                if (ItemList_Event.Count > 0 && hat.Count > 0)
                                {
                                    if (ItemList_Event.Count == hat.Count)
                                    {
                                        for (int j = 0; j < ItemList_Event.Count; j++)
                                        {
                                            string k = Convert.ToString(ItemList_Event[j].ToString());
                                            string names = Convert.ToString(hat[k].ToString());
                                            c++;
                                            string val = ds.Tables[0].Rows[i][names].ToString();

                                            if (val != "")
                                            {

                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][names].ToString();
                                                if (names == "StartTime" || names == "EndTime" || names == "StartPeriod" || names == "EndPeriod" || names == "NoOfAction")
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 80;
                                                }
                                                else if (names == "OutdoorLoc")
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 350;
                                                }
                                                else if (names == "ReqEventName")
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 350;
                                                }
                                                else
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 150;
                                                }
                                            }




                                        }

                                    }
                                }
                            }
                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;

                            //if (CheckBox_column0.Checked == true)
                            //{
                            //    Fpspread1.Width = 900;
                            //    Fpspread1.Height = 420;
                            //}
                            //else
                            //{
                            //    Fpspread1.Width = 830;
                            //    Fpspread1.Height = 420;
                            //}

                            pheaderfilter0.Visible = true;
                            pcolumnorder0.Visible = true;
                            Fpspread1.Visible = true;
                            div_report.Visible = true;

                        }

                    }
                }
            }
        }


        catch
        {
        }
    }
    public void txt_eventname_TextChanged(object sender, EventArgs e)
    {
        txt_staffname.Text = "";
        txt_studname.Text = "";

    }
    public void txt_staffname_TextChanged(object sender, EventArgs e)
    {
        txt_eventname.Text = "";
        txt_studname.Text = "";
    }
    public void txt_studname_TextChanged(object sender, EventArgs e)
    {
        txt_eventname.Text = "";
        txt_staffname.Text = "";
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetEvent(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "select MasterValue from CO_MasterValues where MasterCriteria='EventName' and MasterValue like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and sa.dept_code in('" + deptcode + "') and  s.staff_name like '" + prefixText + "%'";

        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstudname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "select stud_name from Registration where degree_code in('" + deptcode + "') and  stud_name like '" + prefixText + "%'";

        name = ws.Getname(query);
        return name;
    }
    public void CheckBox_column0_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column0.Checked == true)
            {
                ItemList_Event.Clear();
                for (int i = 0; i < cblcolumnorder0.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder0.Items[i].Selected = true;
                    LinkButton8.Visible = true;
                    ItemList_Event.Add(cblcolumnorder0.Items[i].Text.ToString());
                    Itemindex_Event.Add(si);
                }
                LinkButton8.Visible = true;
                tborder0.Visible = true;
                tborder0.Text = "";
                int j = 0;
                for (int i = 0; i < ItemList_Event.Count; i++)
                {
                    j = j + 1;
                    tborder0.Text = tborder0.Text + ItemList_Event[i].ToString();

                    tborder0.Text = tborder0.Text + "(" + (j).ToString() + ")  ";

                }

            }
            else
            {
                for (int i = 0; i < cblcolumnorder0.Items.Count; i++)
                {
                    cblcolumnorder0.Items[i].Selected = false;
                    LinkButton8.Visible = false;
                    ItemList_Event.Clear();
                    Itemindex_Event.Clear();
                    cblcolumnorder0.Items[0].Enabled = false;
                }

                tborder0.Text = "";
                tborder0.Visible = false;

            }

        }
        catch (Exception ex)
        {

        }
    }
    public void LinkButtonsremove0_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder0.ClearSelection();
            CheckBox_column0.Checked = false;
            LinkButton8.Visible = false;
            ItemList_Event.Clear();
            Itemindex_Event.Clear();
            tborder0.Text = "";
            tborder0.Visible = false;

        }
        catch (Exception ex)
        {
        }
    }
    public void cblcolumnorder0_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column0.Checked = false;
            string value = "";
            int index;
            cblcolumnorder0.Items[0].Selected = true;
            cblcolumnorder0.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder0.Items[index].Selected)
            {

            }
            else
            {
                ItemList_Event.Remove(cblcolumnorder0.Items[index].Text.ToString());
                Itemindex_Event.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder0.Items.Count; i++)
            {

                if (cblcolumnorder0.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList_Event.Remove(cblcolumnorder0.Items[i].Text.ToString());
                    Itemindex_Event.Remove(sindex);

                }
            }

            LinkButton8.Visible = true;
            tborder0.Visible = true;
            tborder0.Text = "";
            for (int i = 0; i < ItemList_Event.Count; i++)
            {
                tborder0.Text = tborder0.Text + ItemList_Event[i].ToString();

                tborder0.Text = tborder0.Text + "(" + (i + 1).ToString() + ")  ";

            }
            if (ItemList_Event.Count == 13)
            {
                CheckBox_column0.Checked = true;
            }
            if (ItemList_Event.Count == 0)
            {
                tborder0.Visible = false;
                LinkButton8.Visible = false;
            }

            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    public void btn_popclose_Click(object sender, EventArgs e)
    {
        popview.Visible = false;
    }
    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                ds.Clear();
                string date = "";
                popview.Visible = true;
                string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                string reqpk = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                string q = "select applno,(Select MasterValue FROM CO_MasterValues T WHERE memberaction = T.MasterCode) memberaction, case when ActionType='1' then 'Participant' when ActionType='2' then 'Presented' else 'Organizer' end as ActionType, CONVERT(VARCHAR(11),eventdate,103) eventdate,(Select MasterValue FROM CO_MasterValues T WHERE ActionName = T.MasterCode) ActionName,ACtionDesc,StartTime,EndTime,OutdoorLoc,buildcode,floorno,roomno, case when MemType='0' then 'Student' else 'Staff' end as MemType from RQ_ReqActionDet At,RQ_EventMemberDet rm where at.ActionPK =rm.ActionFK and at.RequisitionFK =rm.RequisitionFK and at.RequisitionFK ='" + reqpk + "' order by ActionPK,ActionType  ";

                q = q + " select appl_name,dept_name, case when ActionType='1' then 'Participant' when ActionType='2' then 'Presented' else 'Organizer' end as ActionType,  case when MemType='0' then 'Student' else 'Staff' end as MemType from RQ_EventMemberDet rm,staff_appl_master sm where RequisitionFK ='" + reqpk + "' and ActionType='3'  and sm.appl_id=rm.ApplNo";
                ds = d2.select_method_wo_parameter(q, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Dummy");
                    dt.Columns.Add("Dummy1");
                    dt.Columns.Add("Dummy2");
                    dt.Columns.Add("Dummy3");
                    dt.Columns.Add("Dummy4");
                    dt.Columns.Add("Dummay5");
                    dt.Columns.Add("Dummay6");
                    dt.Columns.Add("Dummay7");
                    dt.Columns.Add("Dummay8");
                    dt.Columns.Add("Dummay9");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr;
                        dr = dt.NewRow();
                        date = ds.Tables[0].Rows[i]["EventDate"].ToString();

                        dr[0] = date;
                        dr[1] = ds.Tables[0].Rows[i]["ActionName"].ToString();
                        dr[2] = ds.Tables[0].Rows[i]["ACtionDesc"].ToString();
                        dr[3] = ds.Tables[0].Rows[i]["StartTime"].ToString();
                        dr[4] = ds.Tables[0].Rows[i]["EndTime"].ToString();
                        if (ds.Tables[0].Rows[i]["OutdoorLoc"].ToString() == "")
                        {
                            dr[5] = ds.Tables[0].Rows[i]["BuildCode"].ToString() + "-" + ds.Tables[0].Rows[i]["FloorNo"].ToString() + "-" + ds.Tables[0].Rows[i]["RoomNo"].ToString();
                        }
                        else
                        {
                            dr[5] = ds.Tables[0].Rows[i]["OutdoorLoc"].ToString();
                        }

                        dr[6] = ds.Tables[0].Rows[i]["MemType"].ToString();

                        dr[7] = ds.Tables[0].Rows[i]["ActionType"].ToString();
                        string name = "";
                        if (ds.Tables[0].Rows[i]["MemType"].ToString() == "Staff")
                        {
                            name = d2.GetFunction("select appl_name from staff_appl_master where appl_id='" + ds.Tables[0].Rows[i]["applno"].ToString() + "'");
                        }
                        else
                        {
                            name = d2.GetFunction("select stud_name from applyn where app_no='" + ds.Tables[0].Rows[i]["applno"].ToString() + "'");
                        }

                        dr[8] = name;
                        dr[9] = ds.Tables[0].Rows[i]["memberaction"].ToString();

                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        gridadd.DataSource = dt;
                        gridadd.DataBind();
                    }
                    gridadd.Visible = true;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Dummy");
                    dt.Columns.Add("Dummy1");
                    dt.Columns.Add("Dummy2");

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        DataRow dr;
                        dr = dt.NewRow();

                        dr[0] = ds.Tables[1].Rows[i]["MemType"].ToString();
                        dr[1] = ds.Tables[1].Rows[i]["appl_name"].ToString();
                        dr[2] = ds.Tables[1].Rows[i]["dept_name"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                    GridView1.Visible = true;
                }

            }

        }
        catch
        {
        }
    }
    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            cellclick = true;
        }
        catch
        {
        }
    }
    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
            if (txt_excelname.Text == "")
            {
                lbl_norec.Visible = true;
            }
            else
            {
                lbl_norec.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {

                d2.printexcelreport(Fpspread1, report);


                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }

        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }

    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "";
            string pagename = "EventReport.aspx";
            attendance = "Event Report";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, attendance);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                ItemList_Event1.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList_Event1.Add(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex_Event1.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                for (int i = 0; i < ItemList_Event1.Count; i++)
                {
                    j = j + 1;
                    tborder.Text = tborder.Text + ItemList_Event1[i].ToString();

                    tborder.Text = tborder.Text + "(" + (j).ToString() + ")  ";

                }

            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList_Event1.Clear();
                    Itemindex_Event1.Clear();
                    cblcolumnorder.Items[0].Enabled = false;
                }

                tborder.Text = "";
                tborder.Visible = false;

            }

        }
        catch (Exception ex)
        {

        }
    }
    public void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList_Event1.Clear();
            Itemindex_Event1.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex_Event1.Contains(sindex))
                {


                    ItemList_Event1.Add(cblcolumnorder.Items[index].Text.ToString());
                    Itemindex_Event1.Add(sindex);
                }
            }
            else
            {
                ItemList_Event1.Remove(cblcolumnorder.Items[index].Text.ToString());
                Itemindex_Event1.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {

                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList_Event1.Remove(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex_Event1.Remove(sindex);

                }
            }

            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            for (int i = 0; i < ItemList_Event1.Count; i++)
            {
                tborder.Text = tborder.Text + ItemList_Event1[i].ToString();

                tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";

            }
            if (ItemList_Event1.Count == 22)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList_Event1.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }

            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }
}