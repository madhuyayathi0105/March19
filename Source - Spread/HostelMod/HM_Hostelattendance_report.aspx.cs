using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using System.Configuration;

public partial class HM_Hostelattendance_report : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static ArrayList ItemList1 = new ArrayList();
    static ArrayList Itemindex1 = new ArrayList();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    DAccess2 dt = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    private object sender;
    private EventArgs e;
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        if (!IsPostBack)
        {
            bindhostelname();
            bindbuilding();
            bindfloor();
            rdb_Hostel.Checked = true;
            bindcriteria();

            bind_batch();
            bindcollege();
            binddegree();
            bindbranch();

            pheaderfilter1.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        lblvalidation1.Visible = false;
        lbl_error.Visible = false;
    }
    protected void lnk_btn_logout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {
        }
    }
    public void datevalidate(TextBox txt1, TextBox txt2)
    {
        try
        {
            if (txt1.Text != "" && txt2.Text != "")
            {
                //txt_leavedays.Text = "";
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt1.Text);
                string seconddate = Convert.ToString(txt2.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterror.Text = "Select ToDate greater than or equal to the FromDate ";
                    txt2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    //public void bindhostelname()
    //{
    //    try
    //    {
    //        cbl_hostelname.Items.Clear();
    //        ds.Clear();
    //        ds = d2.BindHostel(collegecode1);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_hostelname.DataSource = ds;
    //            cbl_hostelname.DataTextField = "Hostel_Name";
    //            cbl_hostelname.DataValueField = "Hostel_code";
    //            cbl_hostelname.DataBind();
    //            if (cbl_hostelname.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    //                {
    //                    cbl_hostelname.Items[i].Selected = true;
    //                }
    //                txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
    //                cb_hostelname.Checked = true;
    //            }
    //        }
    //        else
    //        {
    //            txt_hostelname.Text = "--Select--";
    //            cb_hostelname.Checked = false;
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    public void bindhostelname()
    {
        try
        {
            ds.Clear();
            cbl_hostelname.Items.Clear();
            //ds = d2.BindHostel_inv(collegecode1);
            //string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelName";
            //ds = d2.select_method_wo_parameter(itemname, "text");
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                }
                //else
                //{
                //    txt_floorname.Text = "--Select--";
                //    cbl_floorname.Items.Clear();
                //    cb_floorname.Checked = false;
                //}
                bindbuilding();
                bindfloor();
                bindfloor();
            }
            else
            {
                txt_hostelname.Text = "--Select--";
                txt_floor.Text = "--Select--";
                cbl_floor.Items.Clear();
                cb_floor.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_hostelname.Text = "--Select--";
        if (cb_hostelname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = true;
            }
            cb_hostelname.Checked = true;
            txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = false;
            }
            txt_hostelname.Text = "--Select--";
            cb_hostelname.Checked = false;
        }
        bindbuilding();
        bindfloor();
        bindroom();
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_hostelname.Checked = false;
            int commcount = 0;
            //string buildvalue = "";
            //string build = "";
            txt_hostelname.Text = "--Select--";
            for (i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                //    if (cbl_hostelname.Items[i].Selected == true)
                //    {
                //        commcount = commcount + 1;
                //        cb_hostelname.Checked = false;
                //        build = cbl_hostelname.Items[i].Value.ToString();
                //        if (buildvalue == "")
                //        {
                //            buildvalue = build;
                //        }
                //        else
                //        {
                //            buildvalue = buildvalue + "'" + "," + "'" + build;
                //        }
                //    }
                //}
                //if (commcount > 0)
                //{
                //    if (commcount == cbl_hostelname.Items.Count)
                //    {
                //        cb_hostelname.Checked = true;
                //    }
                //    txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
                //}
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_hostelname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_hostelname.Items.Count)
                {
                    cb_hostelname.Checked = true;
                }
                txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
            }
            bindbuilding();
            bindfloor();
            bindroom();
        }
        catch (Exception ex)
        {
        }
    }
    //public void bindfloor()
    //{
    //    try
    //    {
    //        string floorname = "";
    //        cbl_floor.Items.Clear();
    //        txt_floor.Text = "---Select---";
    //        cb_floor.Checked = false;
    //        if (cbl_hostelname.Items.Count > 0)
    //        {
    //            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
    //            {
    //                if (cbl_hostelname.Items[i].Selected == true)
    //                {
    //                    if (floorname == "")
    //                    {
    //                        floorname = Convert.ToString(cbl_hostelname.Items[i].Text);
    //                    }
    //                    else
    //                    {
    //                        floorname = floorname + "'" + "," + "'" + Convert.ToString(cbl_hostelname.Items[i].Text);
    //                    }
    //                }
    //            }
    //        }
    //        if (floorname != "")
    //        {
    //          //  ds = d2.BindFloor(floorname);
    //            string itemname = "select distinct f.Floor_Name  from Floor_Master f,Hostel_Details h where f.College_Code=h.college_code and h.Hostel_Name in('" + floorname + "')";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(itemname, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                cbl_floor.DataSource = ds;
    //                cbl_floor.DataTextField = "Floor_Name";
    //                cbl_floor.DataValueField = "Floor_Name";
    //                cbl_floor.DataBind();
    //                if (cbl_floor.Items.Count > 0)
    //                {
    //                    for (int i = 0; i < cbl_floor.Items.Count; i++)
    //                    {
    //                        cbl_floor.Items[i].Selected = true;
    //                    }
    //                    txt_floor.Text = "Floor Name(" + cbl_floor.Items.Count + ")";
    //                    cb_floor.Checked = true;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            cb_floor.Checked = false;
    //            txt_floor.Text = "--Select--";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void bindfloor()
    {
        try
        {
            cbl_floor.Items.Clear();
            //txt_floor.Text = "---Select---";
            //cb_floor.Checked = false;
            //string hostel = "";
            //for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            //{
            //    if (cbl_hostelname.Items[i].Selected == true)
            //    {
            //        if (hostel == "")
            //        {
            //            hostel = "" + cbl_hostelname.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            hostel = hostel + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}
            //string build = d2.GetBuildingCode_inv(hostel);
            //char[] delimiterChars = { ',' };
            //string[] build1 = build.Split(delimiterChars);
            //string build2 = "";
            //foreach (string b in build1)
            //{
            //    if (build2 == "")
            //    {
            //        build2 = "" + b + "";
            //    }
            //    else
            //    {
            //        build2 = build2 + "'" + "," + "'" + b + "";
            //    }
            //}
            //ds.Clear();
            //string floor = "select code,Building_Name from Building_Master where code in ('" + build2 + "')";
            //ds = d2.select_method_wo_parameter(floor, "Text");
            //string w = "";
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    string q1 = Convert.ToString(ds.Tables[0].Rows[0][1]);
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        string q = Convert.ToString(ds.Tables[0].Rows[i][1]);
            //        if (w == "")
            //        {
            //            w = "" + q + "";
            //        }
            //        else
            //        {
            //            w = w + "'" + "," + "'" + q + "";
            //        }
            //    }
            //}
            ds.Clear();
            // ds = d2.BindFloor(w);
            //cbl_floorname.Items.Clear();
            //string itemname = "select distinct  F.Floor_Name from Hostel_Details h,Building_Master b,Floor_Master f where  b.Building_Name=f.Building_Name and h.college_code =b.College_Code and h.college_code =f.College_Code and f.College_Code =b.College_Code  and h.college_code ='" + collegecode1 + "' and h.Hostel_code in('" + hostel + "')";//h.Building_Code=b.code and
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(itemname, "Text");
            string itemname = "select * from Floor_Master f,Building_Master b where b.Building_Name=f.Building_Name and b.code  ='" + Convert.ToString(drbbuilding.SelectedValue) + "'";

            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floor.DataSource = ds;
                cbl_floor.DataTextField = "Floor_Name";
                cbl_floor.DataValueField = "FloorPK";
                cbl_floor.DataBind();
                if (cbl_floor.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_floor.Items.Count; i++)
                    {
                        cbl_floor.Items[i].Selected = true;
                    }
                    txt_floor.Text = "Floor Name(" + cbl_floor.Items.Count + ")";
                }
            }
            else
            {
                txt_floor.Text = "--Select--";
            }
            bindroom();
            //for (int i = 0; i < cbl_floorname.Items.Count; i++)
            //{
            //    if (cbl_floorname.Items[i].Selected == true)
            //    {
            //    }
            //    else
            //    {
            //        for (i = 0; i < cbl_floorname.Items.Count; i++)
            //        {
            //            cbl_floorname.Items[i].Selected = false;
            //        }
            //        txt_floorname.Text = "--Select--";
            //    }
            //}
            //txt_floorname.Text = "Floor Name (" + cbl_floorname.Items.Count + ")";
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbfloor_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_floor.Text = "--Select--";
            if (cb_floor.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_floor.Items.Count; i++)
                {
                    cbl_floor.Items[i].Selected = true;
                }
                txt_floor.Text = "Floor Name(" + (cbl_floor.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_floor.Items.Count; i++)
                {
                    cbl_floor.Items[i].Selected = false;
                }
                txt_floor.Text = "--Select--";
            }
            bindroom();
        }
        catch
        {
        }
    }
    protected void cblfloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        //    cbl_room.Items.Clear();
        //    txt_room.Text = "--Select--";
        //    cb_room.Checked = false;
        int i = 0;
        cb_floor.Checked = false;
        int commcount = 0;
        txt_floor.Text = "--Select--";
        for (i = 0; i < cbl_floor.Items.Count; i++)
        {
            if (cbl_floor.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                //cb_floor.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_floor.Items.Count)
            {
                cb_floor.Checked = true;
            }
            txt_floor.Text = "Floor Name(" + commcount.ToString() + ")";
        }
        bindroom();
    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate, txt_todate);
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter FromDate less than or equal to the ToDate", cv_fromtodt1);
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate, txt_todate);
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter ToDate greater than or equal to the FromDate", cv_fromtodt2);
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void cb_criteria_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_criteria.Text = "--Select--";
            if (cb_criteria.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_criteria.Items.Count; i++)
                {
                    cbl_criteria.Items[i].Selected = true;
                }
                txt_criteria.Text = "Criteria(" + (cbl_criteria.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_criteria.Items.Count; i++)
                {
                    cbl_criteria.Items[i].Selected = false;
                }
                txt_criteria.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_criteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_criteria.Text = "--Select--";
            cb_criteria.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_criteria.Items.Count; i++)
            {
                if (cbl_criteria.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_criteria.Text = "Criteria(" + commcount.ToString() + ")";
                if (commcount == cbl_criteria.Items.Count)
                {
                    cb_criteria.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindcriteria()
    {
        cbl_criteria.Items.Clear();
        txt_criteria.Text = "---Select---";
        cb_criteria.Checked = false;
        cbl_criteria.Items.Insert(0, "P");
        cbl_criteria.Items.Insert(1, "A");
        if (rdb_Hostel.Checked == true)
        {
            cbl_criteria.Items.Insert(2, "OD");
        }
        if (cbl_criteria.Items.Count > 0)
        {
            for (int i = 0; i < cbl_criteria.Items.Count; i++)
            {
                cbl_criteria.Items[i].Selected = true;
            }
            txt_criteria.Text = "Criteria(" + cbl_criteria.Items.Count + ")";
            //  cb_criteria.Checked = true;
        }
        else
        {
            txt_criteria.Text = "--Select--";
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Fphostelcount.Visible = false;
            FpSpread1.Visible = false;
            div1.Visible = false;
            string hostel = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        hostel = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostel = hostel + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string floor = "";
            for (int i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected == true)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_floor.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + cbl_floor.Items[i].Value.ToString() + "";
                    }
                }
            }
            Hashtable htRecordsOK = new Hashtable(); int absentcounttotal = 0; int evening_absenttotal = 0; int columnheadercount = 0;
            string collegeCode = rs.GetSelectedItemsValueAsString(cblCollege);
            string DegreeCode = rs.GetSelectedItemsValueAsString(cbl_branch);
            string BatchYear = rs.GetSelectedItemsValueAsString(cbl_Batch);
            string HostelMasterFk = rs.GetSelectedItemsValueAsString(cbl_hostelname);
            string Floorfk = rs.GetSelectedItemsValueAsString(cbl_floor);
            string roomfk = rs.GetSelectedItemsValueAsString(cbl_room);
            if (rdbDetails.Checked == true)
            {
                #region Student Details
                #region Hostel
                if (rdb_Hostel.Checked == true)
                {
                    DataTable dt_new = new DataTable();
                    dt_new.Columns.Add(Convert.ToString("App_No"));
                    if (hostel != "" && floor != "" && cbl_criteria.Text.Trim() != "Select")
                    {
                        string orderby = "";
                        if (ddlorder.SelectedItem.Text == "Roll No")
                        {
                            orderby = " order by r.roll_no asc";
                        }
                        else if (ddlorder.SelectedItem.Text == "Room")
                        {
                            orderby = " order by rm.Room_Name";

                        }

                        string q = "select r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,h.HostelMasterPK as Hostel_Code,C.Course_Name,Dt.Dept_Name,r.Current_Semester,r.Sections,hs.BuildingFK,FloorFK,RoomFK,h.HostelName as Hostel_Name,bm.Building_Name,fm.Floor_Name,rm.Room_Name from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c,Building_Master bm,Floor_Master fm,Room_Detail rm where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and ISNULL(hs.IsDiscontinued,0)=0 and ISNULL(hs.IsSuspend,0) =0 and ISNULL(hs.IsVacated,0) =0  and h.HostelMasterPK in('" + hostel + "') and FloorFK in ('" + floor + "') and code='" + Convert.ToString(drbbuilding.SelectedValue) + "' and rm.Roompk in('" + roomfk + "')  and r.Batch_Year in('" + BatchYear + "') and r.degree_code in('" + DegreeCode + "') and r.college_code in('" + collegeCode + "') and MemType='1' and bm.Code=hs.BuildingFK and fm.Floorpk=hs.FloorFK and rm.Roompk=hs.RoomFK " + orderby + "  ";//order by r.batch_year desc, r.degree_code asc,r.roll_no asc,hs.roomfk asc
                        ds = d2.select_method_wo_parameter(q, "Text");
                        //lbl_error.Text = "No of Students :" + ds.Tables[0].Rows.Count.ToString();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].ColumnCount = 0;
                            FpSpread1.Sheets[0].RowCount = 0;
                            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                            FpSpread1.CommandBar.Visible = false;
                            FpSpread1.Sheets[0].ColumnCount = 1;
                            FpSpread1.Sheets[0].AutoPostBack = true;
                            FpSpread1.Sheets[0].RowHeader.Visible = false;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                            FpSpread1.Columns[0].Width = 50;
                            FpSpread1.Columns[0].Locked = true;
                            Hashtable columnhash = new Hashtable();
                            columnhash.Add("Roll_No", "Roll No");
                            columnhash.Add("Reg_No", "Reg No");
                            columnhash.Add("Stud_Name", "Student Name");
                            columnhash.Add("Stud_Type", "Student Type");
                            columnhash.Add("Course_Name", "Degree");
                            columnhash.Add("Dept_Name", "Department");
                            columnhash.Add("Current_Semester", "Semester");
                            columnhash.Add("Sections", "Section");
                            columnhash.Add("Building_Name", "Building Name");
                            columnhash.Add("Floor_Name", "Floor Name");
                            columnhash.Add("Room_Name", "Room No");
                            columnhash.Add("Hostel_Name", "Hostel Name");

                            if (ItemList.Count <= 1)
                            {
                                ItemList.Add("Roll_No");
                                ItemList.Add("Reg_No");
                                ItemList.Add("Stud_Name");
                                ItemList.Add("Stud_Type");
                            }
                            //for (int ks = 0; ks < ItemList.Count; ks++)
                            //{
                            //    FpSpread1.Sheets[0].ColumnCount++;
                            //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                            //}
                            for (int jk = 0; jk < ds.Tables[0].Columns.Count; jk++)
                            {
                                string colno = Convert.ToString(ds.Tables[0].Columns[jk]);
                                if (ItemList.Contains(Convert.ToString(colno)))
                                {
                                    int index = ItemList.IndexOf(Convert.ToString(colno));
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(columnhash[colno]);
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                    columnheadercount++;

                                }
                            }
                            if (cbboth.Checked == true)
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Session";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                            }
                            //  dt_new.Columns.Add(Convert.ToString(dt.ToString("dd/MM/yyyy")));
                            //dt = dt.AddDays(1);
                            string fromdate = Convert.ToString(txt_fromdate.Text);
                            DateTime dt = new DateTime();
                            string[] split = fromdate.Split('/');
                            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                            DateTime Fromgranttotal_date = new DateTime();
                            Fromgranttotal_date = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                            string month1 = Convert.ToString(split[1]);
                            string year1 = Convert.ToString(split[2]);
                            string todate = Convert.ToString(txt_todate.Text);
                            DateTime dt1 = new DateTime();
                            string[] split1 = todate.Split('/');
                            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                            DateTime Togranttotal_date = new DateTime();
                            Togranttotal_date = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                            string month2 = Convert.ToString(split1[1]);
                            string year2 = Convert.ToString(split1[2]);
                            DateTime dnew_time = dt;
                            DateTime temptime = dt;
                            DateTime temptimenew = dt;
                            while (dt <= dt1)
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dt.ToString("dd/MM/yyyy"));
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                dt_new.Columns.Add(Convert.ToString(dt.ToString("dd/MM/yyyy")) + "-M");
                                dt_new.Columns.Add(Convert.ToString(dt.ToString("dd/MM/yyyy")) + "-E");
                                dt = dt.AddDays(1);
                            }
                            if (ItemList.Contains(Convert.ToString("Total Days Count")))
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Total Days Count");
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                            }

                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Absent Count");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                            DataRow dnewrow = null;
                            FarPoint.Web.Spread.TextCellType chkdate = new FarPoint.Web.Spread.TextCellType();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                DataView dv = new DataView();
                                dnew_time = temptime;
                                string roll_no = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                                string selectquery = "select * from HT_Attendance where App_No ='" + roll_no + "' and CAST(CONVERT(varchar(20),AttnMonth)+'/01/'+CONVERT(varchar(20),AttnYear) as Datetime) between CAST(CONVERT(varchar(20),'" + month1.TrimStart('0') + "')+'/01/'+CONVERT(varchar(20),'" + year1 + "') as Datetime) and CAST(CONVERT(varchar(20),'" + month2.TrimStart('0') + "')+'/01/'+CONVERT(varchar(20),'" + year2 + "') as Datetime) ";
                                // AttnMonth between '" + month1.TrimStart('0') + "' and '" + month2.TrimStart('0') + "' and AttnYear between '" + year1 + "' and '" + year2 + "'";
                                DataSet dn = d2.select_method_wo_parameter(selectquery, "Text");
                                if (dn.Tables[0].Rows.Count > 0)
                                {
                                    dnewrow = dt_new.NewRow();
                                    dnewrow[0] = Convert.ToString(roll_no);
                                    int col = 0;
                                    while (dnew_time <= dt1)
                                    {
                                        col++;
                                        string fmdate = dnew_time.ToString("dd/MM/yyyy");
                                        split = fmdate.Split('/');
                                        string Attendancevalue = "";
                                        string Attendance_evening = "";
                                        string Attendancevalue1 = Convert.ToString(split[0]);
                                        Attendancevalue1 = Attendancevalue1.TrimStart('0');
                                        Attendancevalue = "D" + Attendancevalue1 + "";
                                        Attendance_evening = "D" + Attendancevalue1 + "E" + "";
                                        // dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                                        string newquery_Value = "";
                                        string evening_attendance_query = "";
                                        for (int cbl = 0; cbl < cbl_criteria.Items.Count; cbl++)
                                        {
                                            if (cbl_criteria.Items[cbl].Selected == true)
                                            {
                                                if (newquery_Value == "")
                                                {
                                                    if (cbl_criteria.Items[cbl].Text == "P")
                                                    {
                                                        newquery_Value = "D" + Attendancevalue1 + " = 1";
                                                    }
                                                    else if (cbl_criteria.Items[cbl].Text == "A")
                                                    {
                                                        newquery_Value = "D" + Attendancevalue1 + " = 2";
                                                    }
                                                    else if (cbl_criteria.Items[cbl].Text == "OD")
                                                    {
                                                        newquery_Value = "D" + Attendancevalue1 + " = 3";
                                                    }
                                                }
                                                else
                                                {
                                                    if (cbl_criteria.Items[cbl].Text == "P")
                                                    {
                                                        newquery_Value = newquery_Value + " or " + "D" + Attendancevalue1 + " = 1";
                                                    }
                                                    else if (cbl_criteria.Items[cbl].Text == "A")
                                                    {
                                                        newquery_Value = newquery_Value + " or " + "D" + Attendancevalue1 + " = 2";
                                                    }
                                                    else if (cbl_criteria.Items[cbl].Text == "OD")
                                                    {
                                                        newquery_Value = newquery_Value + " or " + "D" + Attendancevalue1 + " = 3";
                                                    }
                                                }
                                            }
                                        }
                                        newquery_Value = newquery_Value + " or isnull( D" + Attendancevalue1 + ",'0')=0";//Added By Saranyadevi14.3.2019
                                        for (int cblcount = 0; cblcount < cbl_criteria.Items.Count; cblcount++)
                                        {
                                            if (cbl_criteria.Items[cblcount].Selected == true)
                                            {
                                                if (evening_attendance_query == "")
                                                {
                                                    if (cbl_criteria.Items[cblcount].Text == "P")
                                                    {
                                                        evening_attendance_query = Attendance_evening + " = 1";
                                                    }
                                                    else if (cbl_criteria.Items[cblcount].Text == "A")
                                                    {
                                                        evening_attendance_query = Attendance_evening + " = 2";
                                                    }
                                                    else if (cbl_criteria.Items[cblcount].Text == "OD")
                                                    {
                                                        evening_attendance_query = Attendance_evening + " = 3";
                                                    }
                                                }
                                                else
                                                {
                                                    if (cbl_criteria.Items[cblcount].Text == "P")
                                                    {
                                                        evening_attendance_query = evening_attendance_query + " or " + Attendance_evening + " = 1";
                                                    }
                                                    else if (cbl_criteria.Items[cblcount].Text == "A")
                                                    {
                                                        evening_attendance_query = evening_attendance_query + " or " + Attendance_evening + " = 2";
                                                    }
                                                    else if (cbl_criteria.Items[cblcount].Text == "OD")
                                                    {
                                                        evening_attendance_query = evening_attendance_query + " or " + Attendance_evening + " = 3";
                                                    }
                                                }
                                            }
                                        }
                                        evening_attendance_query = evening_attendance_query + " or isnull( " + Attendance_evening + ",'0')=0";//Added By Saranyadevi14.3.2019
                                        if (cbboth.Checked == true)
                                            dn.Tables[0].DefaultView.RowFilter = "(" + newquery_Value + " and " + evening_attendance_query + ") and AttnYear='" + Convert.ToString(split[2]) + "' and AttnMonth='" + Convert.ToString(split[1]).TrimStart('0') + "'";
                                        else if (cbmor.Checked == true)
                                            dn.Tables[0].DefaultView.RowFilter = "(" + newquery_Value + ") and AttnYear='" + Convert.ToString(split[2]) + "' and AttnMonth='" + Convert.ToString(split[1]).TrimStart('0') + "'";
                                        else
                                            dn.Tables[0].DefaultView.RowFilter = "(" + evening_attendance_query + ") and AttnYear='" + Convert.ToString(split[2]) + "' and AttnMonth='" + Convert.ToString(split[1]).TrimStart('0') + "'";
                                        dv = dn.Tables[0].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            string Attendance_value = Convert.ToString(dv[0][Attendancevalue]);
                                            string Evening_attendance = Convert.ToString(dv[0][Attendance_evening]);
                                            if (Attendance_value.Trim() == "1")
                                            {
                                                dnewrow[col] = "P";
                                                if (!htRecordsOK.Contains("P"))
                                                    htRecordsOK.Add("P", "P");
                                            }
                                            else if (Attendance_value.Trim() == "2")
                                            {
                                                dnewrow[col] = "A";
                                                if (!htRecordsOK.Contains("A"))
                                                    htRecordsOK.Add("A", "A");
                                            }
                                            else if (Attendance_value.Trim() == "3")
                                            {
                                                dnewrow[col] = "OD";
                                                if (!htRecordsOK.Contains("OD"))
                                                    htRecordsOK.Add("OD", "OD");
                                            }
                                            if (Evening_attendance.Trim() == "1")
                                            {
                                                dnewrow[col + 1] = "P";
                                                if (!htRecordsOK.Contains("P"))
                                                    htRecordsOK.Add("P", "P");
                                            }
                                            else if (Evening_attendance.Trim() == "2")
                                            {
                                                dnewrow[col + 1] = "A";
                                                if (!htRecordsOK.Contains("A"))
                                                    htRecordsOK.Add("A", "A");
                                            }
                                            else if (Evening_attendance.Trim() == "3")
                                            {
                                                dnewrow[col + 1] = "OD";
                                                if (!htRecordsOK.Contains("OD"))
                                                    htRecordsOK.Add("OD", "OD");
                                            }
                                        }
                                        else
                                        {
                                            dnewrow[col] = "-";
                                            dnewrow[col + 1] = "-";
                                        }
                                        dnew_time = dnew_time.AddDays(1);
                                        col++;
                                    }
                                    dt_new.Rows.Add(dnewrow);
                                }
                            }
                            int sno = 0;
                            if (dt_new.Rows.Count > 0)
                            {
                                Hashtable grandtotal = new Hashtable();
                                Hashtable grandtotal_eve = new Hashtable();
                                for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                                {
                                    string rollno = Convert.ToString(ds.Tables[0].Rows[ik]["App_No"]);
                                    DataView dns = new DataView(dt_new);
                                    dns.RowFilter = "App_No='" + rollno + "'";
                                    if (dns.Count > 0)
                                    {
                                        sno++;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        int col = 0;
                                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                        {
                                            if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                                            {
                                                col++;
                                                int index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                                FpSpread1.Sheets[0].Columns[index + 1].Width = 150;
                                                FpSpread1.Sheets[0].Columns[index + 1].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[0].Rows[ik][j].ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            }
                                        }

                                        if (dns.Count > 0)
                                        {
                                            int totalcount = 0;
                                            int evng_totalcount = 0;
                                            temptime = temptimenew;
                                            int absentcount = 0;
                                            int evng_absentcount = 0;
                                            if (cbboth.Checked == true)
                                            {
                                                col++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "Morning";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "Evening";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                            }

                                            while (temptime <= dt1)
                                            {
                                                //col++;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "Morning";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                                //FpSpread1.Sheets[0].RowCount++;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "Evening";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                if (cbboth.Checked == true)
                                                {
                                                    col++;
                                                    totalcount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].CellType = chkdate;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Text = Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-M"]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].HorizontalAlign = HorizontalAlign.Center;
                                                    evng_totalcount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                    if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-E"]) != "")
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-E"]);
                                                    else
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                else if (cbmor.Checked == true)
                                                {
                                                    col++;
                                                    totalcount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                    if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-M"]) != "")
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-M"]);
                                                    else
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                else
                                                {
                                                    col++;
                                                    evng_totalcount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                    if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-E"]) != "")
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-E"]);
                                                    else
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                }

                                                if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-M"]).ToUpper() == "P")
                                                {
                                                    if (grandtotal.Contains(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-P"))
                                                    {
                                                        string val = Convert.ToString(grandtotal[Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-P"]);
                                                        double pcount = 0;
                                                        double.TryParse(val, out pcount);
                                                        pcount++;
                                                        grandtotal.Remove(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-P");
                                                        grandtotal.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-P", pcount);
                                                    }
                                                    else
                                                    {
                                                        grandtotal.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-P", 1);
                                                    }
                                                }

                                                if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-E"]).ToUpper() == "P")
                                                {
                                                    if (grandtotal_eve.Contains(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-P"))
                                                    {
                                                        string val1 = Convert.ToString(grandtotal_eve[Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-P"]);
                                                        double pcount1 = 0;
                                                        double.TryParse(val1, out pcount1);
                                                        pcount1++;
                                                        grandtotal_eve.Remove(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-P");
                                                        grandtotal_eve.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-P", pcount1);
                                                    }
                                                    else
                                                    {
                                                        grandtotal_eve.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-P", 1);
                                                    }
                                                }


                                                if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-M"]).ToUpper() == "A")
                                                {
                                                    //absentcounttotal++;
                                                    if (grandtotal.Contains(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-A"))
                                                    {
                                                        string val = Convert.ToString(grandtotal[Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-A"]);
                                                        double pcount = 0;
                                                        double.TryParse(val, out pcount);
                                                        pcount++;
                                                        grandtotal.Remove(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-A");
                                                        grandtotal.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-A", pcount);
                                                    }
                                                    else
                                                    {
                                                        grandtotal.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-A", 1);
                                                    }
                                                }



                                                if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-E"]).ToUpper() == "A")
                                                {
                                                    //absentcounttotal++;
                                                    if (grandtotal_eve.Contains(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-A"))
                                                    {
                                                        string val1 = Convert.ToString(grandtotal_eve[Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-A"]);
                                                        double pcount1 = 0;
                                                        double.TryParse(val1, out pcount1);
                                                        pcount1++;
                                                        grandtotal_eve.Remove(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-A");
                                                        grandtotal_eve.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-A", pcount1);
                                                    }
                                                    else
                                                    {
                                                        grandtotal_eve.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-A", 1);
                                                    }
                                                }



                                                if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-M"]).ToUpper() == "OD")
                                                {
                                                    //odcounttotal++;
                                                    if (grandtotal.Contains(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-OD"))
                                                    {
                                                        string val = Convert.ToString(grandtotal[Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-OD"]);
                                                        double pcount = 0;
                                                        double.TryParse(val, out pcount);
                                                        pcount++;
                                                        grandtotal.Remove(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-OD");
                                                        grandtotal.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-OD", pcount);
                                                    }
                                                    else
                                                    {
                                                        grandtotal.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-OD", 1);
                                                    }
                                                }

                                                if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-E"]).ToUpper() == "OD")
                                                {
                                                    //odcounttotal++;
                                                    if (grandtotal_eve.Contains(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-OD"))
                                                    {
                                                        string val1 = Convert.ToString(grandtotal_eve[Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-OD"]);
                                                        double pcount1 = 0;
                                                        double.TryParse(val1, out pcount1);
                                                        pcount1++;
                                                        grandtotal_eve.Remove(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-OD");
                                                        grandtotal_eve.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-OD", pcount1);
                                                    }
                                                    else
                                                    {
                                                        grandtotal_eve.Add(Convert.ToString(temptime.ToString("dd/MM/yyyy")) + "-OD", 1);
                                                    }
                                                }


                                                if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-M"]).Trim() == "A" || Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-M"]).Trim() == "OD")
                                                {
                                                    absentcount++;
                                                    absentcounttotal++;
                                                }



                                                if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-E"]).Trim() == "A" || Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-E"]).Trim() == "OD")
                                                {
                                                    evng_absentcount++;
                                                    evening_absenttotal++;
                                                }
                                                //FpSpread1.Sheets[0].RowCount--;
                                                temptime = temptime.AddDays(1);//delsi
                                            }
                                            if (cbboth.Checked == true)
                                            {
                                                if (ItemList.Contains(Convert.ToString("Total Days Count")))
                                                {
                                                    col++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].CellType = chkdate;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Text = Convert.ToString(totalcount);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(evng_totalcount);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                col++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].CellType = chkdate;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Text = Convert.ToString(absentcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].HorizontalAlign = HorizontalAlign.Center;


                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(evng_absentcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            else if (cbmor.Checked == true)
                                            {
                                                if (ItemList.Contains(Convert.ToString("Total Days Count")))
                                                {

                                                    col++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(totalcount);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                col++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(absentcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            else
                                            {
                                                if (ItemList.Contains(Convert.ToString("Total Days Count")))
                                                {
                                                    col++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(evng_totalcount);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                col++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(evng_absentcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                    }
                                }
                                if (cbboth.Checked == true)
                                {
                                    for (int ik = 0; ik < ds.Tables[0].Rows.Count * 2; ik++)
                                    {
                                        for (int c = 0; c < ItemList.Count + 1; c++)//delsis
                                        {
                                            if (ik % 2 == 0)
                                                FpSpread1.Sheets[0].SpanModel.Add(ik, c, 2, 1);
                                        }
                                    }
                                }
                                #region total
                                // FpSpread1.Sheets[0].RowCount++;
                                int colval = 0;
                                if (cbmor.Checked == true)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total Present Count Morning";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount
 + 1);
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total Absent Count Morning";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount + 1);
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total OD Count Morning";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount + 1);

                                }
                                if (cbeve.Checked == true)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total Present Count Evening";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount + 1);
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total Absent Count Evening";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount + 1);
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total OD Count Evening";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount + 1);
                                }
                                if (cbboth.Checked == true)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total Present Count Morning";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount
    + 2);
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total Present Count Evening";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount + 2);
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total Absent Count Morning";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount + 2);


                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total Absent Count Evening";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount + 2);

                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total OD Count Morning";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount + 2);


                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Text = "Total OD Count Evening";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colval].ForeColor = Color.Brown;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, columnheadercount + 2);
                                }
                                columnheadercount++;
                                int cun = 0;
                                while (Fromgranttotal_date <= Togranttotal_date)
                                {

                                    string percent = Convert.ToString(grandtotal[Fromgranttotal_date.ToString("dd/MM/yyyy") + "-P"]);
                                    string absent = Convert.ToString(grandtotal[Fromgranttotal_date.ToString("dd/MM/yyyy") + "-A"]);
                                    string od = Convert.ToString(grandtotal[Fromgranttotal_date.ToString("dd/MM/yyyy") + "-OD"]);
                                    string Evening_present = Convert.ToString(grandtotal_eve[Fromgranttotal_date.ToString("dd/MM/yyyy") + "-P"]);
                                    string Evening_absent = Convert.ToString(grandtotal_eve[Fromgranttotal_date.ToString("dd/MM/yyyy") + "-A"]);
                                    string Evening_od = Convert.ToString(grandtotal_eve[Fromgranttotal_date.ToString("dd/MM/yyyy") + "-OD"]);

                                    if (cbboth.Checked == true)
                                    {
                                        cun = 6;
                                        columnheadercount++;
                                    }
                                    else
                                    {
                                        cun = 3;

                                        columnheadercount++;



                                    }

                                    if (cbboth.Checked == true)
                                    {

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 6, columnheadercount].Text = Convert.ToString(percent);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 6, columnheadercount].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 6, columnheadercount].ForeColor = Color.Brown;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 6, columnheadercount].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 6, columnheadercount].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 6, columnheadercount].HorizontalAlign = HorizontalAlign.Center;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, columnheadercount].Text = Convert.ToString(Evening_present);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, columnheadercount].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, columnheadercount].ForeColor = Color.Brown;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, columnheadercount].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, columnheadercount].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, columnheadercount].HorizontalAlign = HorizontalAlign.Center;



                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, columnheadercount].Text = Convert.ToString(absent);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, columnheadercount].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, columnheadercount].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, columnheadercount].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, columnheadercount].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, columnheadercount].ForeColor = Color.Brown;



                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, columnheadercount].Text = Convert.ToString(Evening_absent);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, columnheadercount].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, columnheadercount].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, columnheadercount].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, columnheadercount].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, columnheadercount].ForeColor = Color.Brown;



                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnheadercount].Text = Convert.ToString(od);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnheadercount].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnheadercount].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnheadercount].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnheadercount].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnheadercount].ForeColor = Color.Brown;


                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnheadercount].Text = Convert.ToString(Evening_od);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnheadercount].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnheadercount].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnheadercount].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnheadercount].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnheadercount].ForeColor = Color.Brown;

                                    }
                                    else if (cbmor.Checked == true)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Text = Convert.ToString(percent);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].ForeColor = Color.Brown;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        cun--;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Text = Convert.ToString(absent);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].ForeColor = Color.Brown;
                                        cun--;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Text = Convert.ToString(od);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].ForeColor = Color.Brown;

                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Text = Convert.ToString(Evening_present);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].ForeColor = Color.Brown;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        cun--;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Text = Convert.ToString(Evening_absent);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].ForeColor = Color.Brown;
                                        cun--;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Text = Convert.ToString(Evening_od);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - cun, columnheadercount - 1].ForeColor = Color.Brown;

                                    }

                                    Fromgranttotal_date = Fromgranttotal_date.AddDays(1);
                                }
                                if (cbboth.Checked == true)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(absentcounttotal);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.Brown;

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(evening_absenttotal);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.Brown;
                                }
                                else if (cbmor.Checked == true)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(absentcounttotal);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.Brown;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(evening_absenttotal);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.Brown;
                                }

                                #endregion
                            }
                            bool Ok = false;
                            for (int i = 0; i < cbl_criteria.Items.Count; i++)
                            {
                                if (cbl_criteria.Items[i].Selected)
                                {
                                    if (htRecordsOK.Contains(cbl_criteria.Items[i].Text.ToUpper()))
                                        Ok = true;
                                }
                            }
                            if (dt_new.Rows.Count > 0 && Ok)
                            {
                                rptprint.Visible = true;
                                FpSpread1.Visible = true;
                                div1.Visible = true;
                                lbl_error.Visible = false;
                                pheaderfilter.Visible = true;
                                pcolumnorder.Visible = true;
                                FpSpread1.SaveChanges();
                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            }
                            else
                            {
                                lbl_error.Visible = true;
                                rptprint.Visible = false;
                                lbl_error.Text = "No records found";
                                pheaderfilter.Visible = false;
                                pcolumnorder.Visible = false;
                                div1.Visible = false;
                                FpSpread1.Visible = false;
                            }
                        }
                        else
                        {
                            //rptprint.Visible = false;
                            //lbl_error.Visible = true;
                            //lbl_error.Text = "Please Select All Field";
                            rptprint.Visible = false;
                            lbl_error.Visible = true;
                            lbl_error.Text = "No records found";
                            pheaderfilter.Visible = false;
                            pcolumnorder.Visible = false;
                            div1.Visible = false;
                            FpSpread1.Visible = false;
                        }
                    }
                    else
                    {
                        rptprint.Visible = false;
                        // imgdiv2.Visible = true;
                        //lbl_alert.Text = "No records found";
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please Select All Field";
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        div1.Visible = false;
                        FpSpread1.Visible = false;
                    }
                }
                #endregion
                #region Guest
                else if (rdo_guest.Checked == true)//24.11.15
                {
                    DataTable dt_new = new DataTable();
                    dt_new.Columns.Add(Convert.ToString("GuestCode"));
                    string fromdate = Convert.ToString(txt_fromdate.Text);
                    DateTime dt = new DateTime();
                    string[] split = fromdate.Split('/');
                    dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    string todate = Convert.ToString(txt_todate.Text);
                    DateTime dt1 = new DateTime();
                    string[] split1 = todate.Split('/');
                    dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                    string current = DateTime.Now.ToString("dd/MM/yyyy");
                    string[] split2 = current.Split('/');
                    DateTime dt3 = Convert.ToDateTime(split2[1] + "/" + split2[0] + "/" + split2[2]);
                    if (txt_hostelname.Text.Trim() != "--Select--" && txt_floor.Text.Trim() != "--Select--")// && txt_buildingname.Text.Trim() != "--Select--" && txt_floorname.Text.Trim() != "--Select--" && txt_roomname.Text.Trim() != "--Select--"
                    {
                        //string q = "select hd.Hostel_Name, Guest_Name,GuestCode,Guest_Address,MobileNo,From_Company ,Floor_Name,Room_Name,gr.Hostel_Code,convert(varchar(10),Admission_Date ,103)as Admission_Date,bm.Building_Name,bm.Code,Guest_Street,Guest_City,Guest_PinCode,Purpose from Hostel_GuestReg gr,Hostel_Details hd,Building_Master bm,Hostel_Details hh where gr.Hostel_Code=hd.Hostel_code and gr.Hostel_Code=hh.Hostel_code and bm.Building_Name=gr.Building_Name   and gr.college_code='" + collegecode1 + "' and Floor_Name in('" + floor + "') and gr.Hostel_Code in('" + hostel + "')";//  and Admission_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
                        //select hd.Hostel_Name, Guest_Name,GuestCode,Guest_Address,MobileNo,From_Company ,Floor_Name,Room_Name,gr.Hostel_Code,convert(varchar(10),Admission_Date ,103)as Admission_Date,bm.Building_Name,bm.Code,Guest_Street,Guest_City,Guest_PinCode,Purpose from Hostel_GuestReg gr,Hostel_Details hd,Building_Master bm,Hostel_Details hh where gr.Hostel_Code=hd.Hostel_code and gr.Hostel_Code=hh.Hostel_code and bm.Building_Name=gr.Building_Name  and bm.Code in('" + buildingname + "') and gr.college_code='" + collegecode1 + "' and Floor_Name in('" + floorname + "') and Room_Name in('" + roomname + "') and gr.Hostel_Code in('" + hoscode + "')";//  and Admission_Date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
                        //string q = "select hd.HostelName as Hostel_Name,cv.VendorName as Guest_Name,vi.VendorContactPK as GuestCode,cv.VendorAddress as Guest_Address,cv.VendorMobileNo as MobileNo,cv.VendorCompName as From_Company,fm.Floor_Name,rm.Room_Name,gr.HostelMasterFK,bm.Building_Name,bm.Code,cv.VendorStreet as Guest_Street,cv.VendorCity as Guest_City,cv.VendorPin as Guest_PinCode from HT_HostelRegistration gr,HM_HostelMaster hd,Building_Master bm,CO_VendorMaster cv,IM_VendorContactMaster vi,Floor_Master fm,Room_Detail rm where gr.HostelMasterFK=hd.HostelMasterPK and bm.Code=gr.BuildingFK and gr.FloorFK in('" + floor + "') and gr.HostelMasterFK in('" + hostel + "') and MemType='3' and gr.GuestVendorFK=cv.VendorPK and gr.FloorFK=fm.Floorpk and gr.RoomFK=rm.Roompk and cv.VendorPK=vi.VendorFK";
                        string q = "select HM.HostelName as Hostel_Name,Vi.VenContactName as Guest_Name,Vi.VendorContactPK as GuestCode,V.VendorAddress as Guest_Address,Vi.VendorMobileNo as MobileNo,V.VendorCompName as From_Company,f.Floor_Name as Floor_Name,r.Room_Name as Room_Name,HM.HostelMasterPK as Hostel_Code,B.Building_Name,B.Code,V.VendorStreet as Guest_Street,V.VendorCity as Guest_City,V.VendorPin as Guest_PinCode from HT_HostelRegistration H,CO_VendorMaster V,IM_VendorContactMaster Vi,Building_Master B,Floor_Master f,Room_Detail r,HM_HostelMaster HM where hm.HostelMasterPK =h.HostelMasterFK and v.VendorPK=vi.VendorFK and b.Code =h.BuildingFK and f.FloorPK=H.FloorFK and r.RoomPk=H.RoomFK and h.CollegeCode='" + collegecode1 + "' and H.FloorFK in('" + floor + "') and HM.HostelMasterPK in('" + hostel + "') and H.GuestVendorFK=v.VendorPK ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(q, "Text");
                        if (dt <= dt1 && dt1 <= dt3)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].ColumnCount = 0;
                                FpSpread1.Sheets[0].RowCount = 0;
                                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                                FpSpread1.CommandBar.Visible = false;
                                FpSpread1.Sheets[0].ColumnCount = 1;
                                FpSpread1.Sheets[0].AutoPostBack = false;
                                FpSpread1.Sheets[0].RowHeader.Visible = false;
                                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                darkstyle.ForeColor = Color.White;
                                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                FpSpread1.Columns[0].Width = 50;
                                FpSpread1.Columns[0].Locked = true;
                                Hashtable columnhash1 = new Hashtable();
                                columnhash1.Add("Hostel_Name", "Hostel Name");
                                columnhash1.Add("Guest_Name", "Guest Name");
                                columnhash1.Add("Guest_Address", "Guest Address");
                                columnhash1.Add("MobileNo", "Mobile No");
                                columnhash1.Add("From_Company", "From Company");
                                columnhash1.Add("Floor_Name", "Floor Name");
                                columnhash1.Add("Room_Name", "Room Name");
                                columnhash1.Add("Admission_Date", "Admission Date");
                                columnhash1.Add("Building_Name", "Building Name");
                                //columnhash.Add("Floor_Name", "Floor Name");
                                //columnhash.Add("Room_Name", "Room No");
                                columnhash1.Add("Guest_Street", "Guest Street");
                                columnhash1.Add("Guest_City", "Guest City");
                                columnhash1.Add("Guest_PinCode", "Guest Pincode");
                                columnhash1.Add("Purpose", "Purpose");
                                if (ItemList1.Count == 0)
                                {
                                    ItemList1.Add("Hostel_Name");
                                    ItemList1.Add("Guest_Name");
                                    ItemList1.Add("MobileNo");
                                    ItemList1.Add("From_Company");
                                }
                                for (int jk = 0; jk < ds.Tables[0].Columns.Count; jk++)
                                {
                                    string colno = Convert.ToString(ds.Tables[0].Columns[jk]);
                                    if (ItemList1.Contains(Convert.ToString(colno)))
                                    {
                                        int index = ItemList1.IndexOf(Convert.ToString(colno));
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(columnhash1[colno]);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                    }
                                }
                                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                                string month1 = Convert.ToString(split[1]);
                                string year1 = Convert.ToString(split[2]);
                                dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                                string month2 = Convert.ToString(split1[1]);
                                string year2 = Convert.ToString(split1[2]);
                                DateTime dnew_time = dt;
                                DateTime temptime = dt;
                                DateTime temptimenew = dt;
                                while (dt <= dt1)
                                {
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dt.ToString("dd/MM/yyyy"));
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                    dt_new.Columns.Add(Convert.ToString(dt.ToString("dd/MM/yyyy")));
                                    dt = dt.AddDays(1);
                                }
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Total Days Count");
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Absent Count");
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                DataRow dnewrow = null;
                                FarPoint.Web.Spread.TextCellType chkdate = new FarPoint.Web.Spread.TextCellType();
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    DataView dv = new DataView();
                                    dnew_time = temptime;
                                    string GuestCode = Convert.ToString(ds.Tables[0].Rows[i]["GuestCode"]);
                                    string selectquery = "select * from HT_Attendance where App_No ='" + GuestCode + "' and AttnMonth between '" + month1 + "' and '" + month2 + "' and AttnYear between '" + year1 + "' and '" + year2 + "'";
                                    DataSet dn = d2.select_method_wo_parameter(selectquery, "Text");
                                    if (dn.Tables[0].Rows.Count > 0)
                                    {
                                        dnewrow = dt_new.NewRow();
                                        dnewrow[0] = Convert.ToString(GuestCode);
                                        int col = 0;
                                        while (dnew_time <= dt1)
                                        {
                                            col++;
                                            string fmdate = dnew_time.ToString("dd/MM/yyyy");
                                            split = fmdate.Split('/');
                                            string Attendancevalue = "";
                                            string Attendancevalue1 = Convert.ToString(split[0]);
                                            Attendancevalue1 = Attendancevalue1.TrimStart('0');
                                            Attendancevalue = "D" + Attendancevalue1 + "";
                                            // dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                                            string newquery_Value = "";
                                            for (int cbl = 0; cbl < cbl_criteria.Items.Count; cbl++)
                                            {
                                                if (cbl_criteria.Items[cbl].Selected == true)
                                                {
                                                    if (newquery_Value == "")
                                                    {
                                                        if (cbl_criteria.Items[cbl].Text == "P")
                                                        {
                                                            newquery_Value = "D" + Attendancevalue1 + " = 1";
                                                        }
                                                        else if (cbl_criteria.Items[cbl].Text == "A")
                                                        {
                                                            newquery_Value = "D" + Attendancevalue1 + " = 2";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (cbl_criteria.Items[cbl].Text == "P")
                                                        {
                                                            newquery_Value = newquery_Value + " or " + "D" + Attendancevalue1 + " = 1";
                                                        }
                                                        else if (cbl_criteria.Items[cbl].Text == "A")
                                                        {
                                                            newquery_Value = newquery_Value + " or " + "D" + Attendancevalue1 + " = 2";
                                                        }
                                                    }
                                                }
                                            }
                                            dn.Tables[0].DefaultView.RowFilter = "" + newquery_Value + " and AttnYear='" + Convert.ToString(split[2]) + "' and AttnMonth='" + Convert.ToString(split[1]) + "'";
                                            dv = dn.Tables[0].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                string Attendance_value = Convert.ToString(dv[0][Attendancevalue]);
                                                if (Attendance_value.Trim() == "1")
                                                {
                                                    dnewrow[col] = "P";
                                                    if (!htRecordsOK.Contains("P"))
                                                        htRecordsOK.Add("P", "P");
                                                }
                                                else if (Attendance_value.Trim() == "2")
                                                {
                                                    dnewrow[col] = "A";
                                                    if (!htRecordsOK.Contains("A"))
                                                        htRecordsOK.Add("A", "A");
                                                }
                                            }
                                            else
                                            {
                                                dnewrow[col] = "-";
                                            }
                                            dnew_time = dnew_time.AddDays(1);
                                        }
                                        dt_new.Rows.Add(dnewrow);
                                    }
                                }
                                int sno = 0;
                                if (dt_new.Rows.Count > 0)
                                {
                                    for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                                    {
                                        string rollno = Convert.ToString(ds.Tables[0].Rows[ik]["GuestCode"]);
                                        DataView dns = new DataView(dt_new);
                                        dns.RowFilter = "GuestCode='" + rollno + "'";
                                        if (dns.Count > 0)
                                        {
                                            sno++;
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            int col = 0;
                                            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                            {
                                                if (ItemList1.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                                                {
                                                    col++;
                                                    int index = ItemList1.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                                    FpSpread1.Sheets[0].Columns[index + 1].Width = 150;
                                                    FpSpread1.Sheets[0].Columns[index + 1].Locked = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[0].Rows[ik][j].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                }
                                            }
                                            if (dns.Count > 0)
                                            {
                                                int totalcount = 0;
                                                temptime = temptimenew;
                                                int absentcount = 0;
                                                while (temptime <= dt1)
                                                {
                                                    col++;
                                                    totalcount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy") + "-M"]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                    if (Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy")]).Trim() == "A" || Convert.ToString(dns[0][temptime.ToString("dd/MM/yyyy")]).Trim() == "OD")
                                                    {
                                                        absentcount++;
                                                    }
                                                    temptime = temptime.AddDays(1);
                                                }
                                                col++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(totalcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                col++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = chkdate;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(absentcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                    }
                                }
                                bool Ok = false;
                                for (int i = 0; i < cbl_criteria.Items.Count; i++)
                                {
                                    if (cbl_criteria.Items[i].Selected)
                                    {
                                        if (htRecordsOK.Contains(cbl_criteria.Items[i].Text.ToUpper()))
                                            Ok = true;
                                    }
                                }
                                if (dt_new.Rows.Count > 0 && Ok)
                                {
                                    rptprint.Visible = true;
                                    FpSpread1.Visible = true;
                                    div1.Visible = true;
                                    lbl_error.Visible = false;
                                    pheaderfilter.Visible = false;
                                    //pcolumnorder.Visible = true;
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                }
                                else
                                {
                                    div1.Visible = false;
                                    rptprint.Visible = false;
                                    lbl_error.Visible = true;
                                    lbl_error.Text = "No Records Found";
                                }
                            }
                            else
                            {
                                div1.Visible = false;
                                rptprint.Visible = false;
                                lbl_error.Visible = true;
                                lbl_error.Text = "No Records Found";
                            }
                        }
                    }
                    else
                    {
                        div1.Visible = false;
                        rptprint.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please select all fields";
                    }
                }
                #endregion
                #endregion
            }
            else if (rdbCumulative.Checked == true)
            {
                #region Cumulative
                if (hostel != "" && Floorfk != "" && cbl_criteria.Text.Trim() != "Select" && !string.IsNullOrEmpty(HostelMasterFk) && !string.IsNullOrEmpty(DegreeCode) && !string.IsNullOrEmpty(collegeCode))
                {
                    DateTime Fromdate = new DateTime();
                    DateTime Todate = new DateTime();
                    Fromdate = rs.GetDatetime(txt_fromdate.Text);
                    string fromday = Convert.ToString(Fromdate.ToString("dd")).TrimStart('0');
                    string frommonth = Convert.ToString(Fromdate.ToString("MM")).TrimStart('0');
                    string fromyear = Convert.ToString(Fromdate.ToString("yyyy"));

                    Todate = rs.GetDatetime(txt_todate.Text);
                    string today = Convert.ToString(Todate.ToString("dd")).TrimStart('0');
                    string tomonth = Convert.ToString(Todate.ToString("MM")).TrimStart('0');
                    string toyear = Convert.ToString(Todate.ToString("yyyy"));

                    Dictionary<string, double> AttendancecountDic = new Dictionary<string, double>();
                    ds.Clear();
                    string StudentCountQry = "select COUNT(h.app_no)StudentCount,h.HostelMasterFK,hm.HostelName,r.Batch_Year from HT_HostelRegistration h,Registration r,HM_HostelMaster hm where h.APP_No=r.App_No and h.HostelMasterFK=hm.HostelMasterPK and r.college_code in('" + collegeCode + "') and r.Batch_Year in('" + BatchYear + "') and r.degree_code in('" + DegreeCode + "') and h.HostelMasterFK in('" + HostelMasterFk + "') and h.FloorFK in('" + Floorfk + "') and h.BuildingFK='" + Convert.ToString(drbbuilding.SelectedValue) + "' and h.RoomFK in('" + roomfk + "') and ISNULL (isvacated,'0')=0 and ISNULL(IsDiscontinued,'0')=0 group by h.HostelMasterFK,hm.HostelName,r.Batch_Year order by h.HostelMasterFK ,r.Batch_Year desc ";
                    StudentCountQry += " select h.app_no,h.HostelMasterFK,hm.HostelName,r.Batch_Year,ha.AttnMonth,ha.AttnYear,ha.App_No,[D1],[D2],[D3],[D4],[D5],[D6],[D7],[D8],[D9],[D10],[D11],[D12],[D13],[D14],[D15],[D16],[D17],[D18],[D19],[D20],[D21],[D22],[D23],[D24],[D25],[D26],[D27],[D28],[D29],[D30],[D31],[D1E],[D2E],[D3E],[D4E],[D5E],[D6E],[D7E],[D8E],[D9E],[D10E],[D11E],[D12E],[D13E],[D14E],[D15E],[D16E],[D17E],[D18E],[D19E],[D20E],[D21E],[D22E],[D23E],[D24E],[D25E],[D26E],[D27E],[D28E],[D29E],[D30E],[D31E] from HT_HostelRegistration h,Registration r,HM_HostelMaster hm,HT_Attendance HA where h.APP_No=r.App_No and h.HostelMasterFK=hm.HostelMasterPK and ha.App_No=h.APP_No and r.college_code in('" + collegeCode + "') and r.Batch_Year in('" + BatchYear + "') and r.degree_code in('" + DegreeCode + "') and h.HostelMasterFK in('" + HostelMasterFk + "')  and h.BuildingFK='" + Convert.ToString(drbbuilding.SelectedValue) + "' and h.RoomFK in('" + roomfk + "') and h.FloorFK in('" + Floorfk + "') and ( (AttnMonth >= '" + frommonth + "' and AttnYear = '" + fromyear + "') or (AttnMonth <='" + tomonth + "' and AttnYear = '" + toyear + "' )) and ISNULL (isvacated,'0')=0 and ISNULL(IsDiscontinued,'0')=0 order by h.HostelMasterFK,h.APP_No";
                    //and CAST(CONVERT(varchar(20),AttnMonth)+'/01/'+CONVERT(varchar(20),AttnYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/" + fromday + "/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/" + today + "/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) 
                    ds = d2.select_method_wo_parameter(StudentCountQry, "text");
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fphostelcount.Sheets[0].RowCount = 0;
                            Fphostelcount.Sheets[0].ColumnCount = 0;
                            Fphostelcount.CommandBar.Visible = false;
                            Fphostelcount.Sheets[0].AutoPostBack = true;
                            Fphostelcount.Sheets[0].ColumnHeader.RowCount = 2;
                            Fphostelcount.Sheets[0].RowHeader.Visible = false;
                            Fphostelcount.Sheets[0].Columns.Count = 4;

                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            Fphostelcount.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fphostelcount.Columns[0].Width = 50;

                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch";
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fphostelcount.Columns[1].Width = 80;

                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hostel name";
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fphostelcount.Columns[2].Width = 180;

                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total strength";
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            Fphostelcount.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fphostelcount.Columns[3].Width = 100;
                            Fphostelcount.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                            Fphostelcount.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                            Fphostelcount.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                            Fphostelcount.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                            if (cbboth.Checked)
                            {

                                Fphostelcount.Sheets[0].ColumnCount++;
                                Fphostelcount.Sheets[0].ColumnHeader.Cells[0, Fphostelcount.Sheets[0].ColumnCount - 1].Text = "Session";
                                Fphostelcount.Sheets[0].ColumnHeader.Cells[0, Fphostelcount.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fphostelcount.Sheets[0].ColumnHeader.Cells[0, Fphostelcount.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fphostelcount.Sheets[0].ColumnHeader.Cells[0, Fphostelcount.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                Fphostelcount.Sheets[0].ColumnHeader.Cells[0, Fphostelcount.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                                Fphostelcount.Columns[Fphostelcount.Sheets[0].ColumnCount - 1].Width = 170;
                                Fphostelcount.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                            }
                            DateTime tempdate = Fromdate;
                            while (tempdate <= Todate)
                            {
                                Fphostelcount.Sheets[0].ColumnCount++;
                                Fphostelcount.Sheets[0].ColumnHeader.Cells[0, Fphostelcount.Sheets[0].ColumnCount - 1].Text = Convert.ToString(tempdate.ToString("dd/MM/yyyy"));
                                Fphostelcount.Sheets[0].ColumnHeader.Cells[0, Fphostelcount.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                Fphostelcount.Sheets[0].ColumnHeader.Cells[0, Fphostelcount.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fphostelcount.Sheets[0].ColumnHeader.Cells[0, Fphostelcount.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                bool firstCol = false;
                                int selectCritria = 0;
                                for (int cbl = 0; cbl < cbl_criteria.Items.Count; cbl++)
                                {
                                    if (cbl_criteria.Items[cbl].Selected == true)
                                    {
                                        selectCritria++;
                                        string Header = string.Empty;
                                        switch (cbl)
                                        {
                                            case 0:
                                                Header = "Present";
                                                break;
                                            case 1:
                                                Header = "Absent";
                                                break;
                                            case 2:
                                                Header = "OD";
                                                break;
                                        }
                                        if (firstCol)
                                            Fphostelcount.Sheets[0].ColumnCount++;
                                        firstCol = true;
                                        Fphostelcount.Sheets[0].ColumnHeader.Cells[0, Fphostelcount.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(tempdate.ToString("dd/MM/yyyy"));
                                        Fphostelcount.Sheets[0].ColumnHeader.Cells[1, Fphostelcount.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_criteria.Items[cbl].Text);// Header;
                                        Fphostelcount.Sheets[0].ColumnHeader.Cells[1, Fphostelcount.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_criteria.Items[cbl].Text);
                                        Fphostelcount.Sheets[0].ColumnHeader.Cells[1, Fphostelcount.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        Fphostelcount.Sheets[0].ColumnHeader.Cells[1, Fphostelcount.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        Fphostelcount.Sheets[0].ColumnHeader.Cells[1, Fphostelcount.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        Fphostelcount.Sheets[0].ColumnHeader.Cells[1, Fphostelcount.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        Fphostelcount.Columns[Fphostelcount.Sheets[0].ColumnCount - 1].Width = 100;
                                    }
                                }
                                Fphostelcount.Sheets[0].ColumnHeaderSpanModel.Add(0, Fphostelcount.Sheets[0].ColumnCount - selectCritria, 1, selectCritria);
                                tempdate = tempdate.AddDays(1);
                            }
                            int r = 0;
                            int TotalHeader = cbboth.Checked ? 4 : 3;

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                r++;
                                Fphostelcount.Sheets[0].RowCount++;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(r);
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["Batch_Year"]);
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dr["HostelName"]);
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dr["HostelMasterFK"]);
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["StudentCount"]);
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                if (cbboth.Checked)
                                {
                                    Fphostelcount.Sheets[0].RowCount++;
                                }
                                tempdate = Fromdate;
                                AttendancecountDic.Clear();

                                while (tempdate <= Todate)
                                {
                                    #region MyRegion
                                    string Attendancevalue1 = Convert.ToString(tempdate.Day);
                                    Attendancevalue1 = Attendancevalue1.TrimStart('0');
                                    string Attendancevalue = "D" + Attendancevalue1 + "";
                                    string Attendance_evening = "D" + Attendancevalue1 + "E" + "";
                                    string newquery_Value = "";
                                    string evening_attendance_query = "";
                                    for (int cbl = 0; cbl < cbl_criteria.Items.Count; cbl++)
                                    {
                                        if (cbl_criteria.Items[cbl].Selected == true)
                                        {
                                            if (newquery_Value == "")
                                            {
                                                if (cbl_criteria.Items[cbl].Text == "P")
                                                    newquery_Value = "D" + Attendancevalue1 + " = 1";
                                                else if (cbl_criteria.Items[cbl].Text == "A")
                                                    newquery_Value = "D" + Attendancevalue1 + " = 2";
                                                else if (cbl_criteria.Items[cbl].Text == "OD")
                                                    newquery_Value = "D" + Attendancevalue1 + " = 3";
                                            }
                                            else
                                            {
                                                if (cbl_criteria.Items[cbl].Text == "P")
                                                    newquery_Value = newquery_Value + " or " + "D" + Attendancevalue1 + " = 1";
                                                else if (cbl_criteria.Items[cbl].Text == "A")
                                                    newquery_Value = newquery_Value + " or " + "D" + Attendancevalue1 + " = 2";
                                                else if (cbl_criteria.Items[cbl].Text == "OD")
                                                    newquery_Value = newquery_Value + " or " + "D" + Attendancevalue1 + " = 3";
                                            }
                                            if (evening_attendance_query == "")
                                            {
                                                if (cbl_criteria.Items[cbl].Text == "P")
                                                    evening_attendance_query = Attendance_evening + " = 1";
                                                else if (cbl_criteria.Items[cbl].Text == "A")
                                                    evening_attendance_query = Attendance_evening + " = 2";
                                                else if (cbl_criteria.Items[cbl].Text == "OD")
                                                    evening_attendance_query = Attendance_evening + " = 3";
                                            }
                                            else
                                            {
                                                if (cbl_criteria.Items[cbl].Text == "P")
                                                    evening_attendance_query = evening_attendance_query + " or " + Attendance_evening + " = 1";
                                                else if (cbl_criteria.Items[cbl].Text == "A")
                                                    evening_attendance_query = evening_attendance_query + " or " + Attendance_evening + " = 2";
                                                else if (cbl_criteria.Items[cbl].Text == "OD")
                                                    evening_attendance_query = evening_attendance_query + " or " + Attendance_evening + " = 3";
                                            }
                                        }
                                    }
                                    #endregion
                                    if (cbboth.Checked == true)
                                        ds.Tables[1].DefaultView.RowFilter = " HostelMasterFK='" + Convert.ToString(dr["HostelMasterFK"]) + "' and Batch_Year='" + Convert.ToString(dr["Batch_Year"]) + "' and AttnMonth='" + tempdate.ToString("MM").TrimStart('0') + "' and AttnYear='" + tempdate.ToString("yyyy") + "' and (" + newquery_Value + " and " + evening_attendance_query + ")";
                                    else if (cbmor.Checked == true)
                                        ds.Tables[1].DefaultView.RowFilter = " HostelMasterFK='" + Convert.ToString(dr["HostelMasterFK"]) + "' and Batch_Year='" + Convert.ToString(dr["Batch_Year"]) + "' and AttnMonth='" + tempdate.ToString("MM").TrimStart('0') + "' and AttnYear='" + tempdate.ToString("yyyy") + "' and (" + newquery_Value + " )";
                                    else
                                        ds.Tables[1].DefaultView.RowFilter = " HostelMasterFK='" + Convert.ToString(dr["HostelMasterFK"]) + "' and Batch_Year='" + Convert.ToString(dr["Batch_Year"]) + "' and AttnMonth='" + tempdate.ToString("MM").TrimStart('0') + "' and AttnYear='" + tempdate.ToString("yyyy") + "' and ( " + evening_attendance_query + ")";
                                    DataView AttendDV = ds.Tables[1].DefaultView;
                                    double val = 0;
                                    if (cbboth.Checked)
                                    {
                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, 4].Text = "Morning";
                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, 4].Tag = "M";
                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, 4].HorizontalAlign = HorizontalAlign.Left;
                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, 4].Font.Size = FontUnit.Medium;
                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, 4].Font.Name = "Book Antiqua";
                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 4].Text = "Evening";
                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 4].Tag = "E";
                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    }
                                    #region Attendance Value Added

                                    foreach (DataRowView AttDR in AttendDV)
                                    {
                                        string Attendance_value = Convert.ToString(AttDR[Attendancevalue]);
                                        string Evening_attendance = Convert.ToString(AttDR[Attendance_evening]);
                                        val = 0;
                                        if (Attendance_value.Trim() == "1")
                                        {
                                            if (!AttendancecountDic.ContainsKey(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MP"))
                                                AttendancecountDic.Add(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MP", 1);
                                            else
                                            {
                                                double.TryParse(Convert.ToString(AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MP"]), out val);
                                                AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MP"] = val + 1;
                                            }
                                        }
                                        else if (Attendance_value.Trim() == "2")
                                        {
                                            if (!AttendancecountDic.ContainsKey(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MA"))
                                                AttendancecountDic.Add(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MA", 1);
                                            else
                                            {
                                                double.TryParse(Convert.ToString(AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MA"]), out val);
                                                AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MA"] = val + 1;
                                            }
                                        }
                                        else if (Attendance_value.Trim() == "3")
                                        {
                                            if (!AttendancecountDic.ContainsKey(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MOD"))
                                                AttendancecountDic.Add(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MOD", 1);
                                            else
                                            {
                                                double.TryParse(Convert.ToString(AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MOD"]), out val);
                                                AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-MOD"] = val + 1;
                                            }
                                        }
                                        if (Evening_attendance.Trim() == "1")
                                        {
                                            if (!AttendancecountDic.ContainsKey(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EP"))
                                                AttendancecountDic.Add(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EP", 1);
                                            else
                                            {
                                                double.TryParse(Convert.ToString(AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EP"]), out val);
                                                AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EP"] = val + 1;
                                            }
                                        }
                                        else if (Evening_attendance.Trim() == "2")
                                        {
                                            if (!AttendancecountDic.ContainsKey(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EA"))
                                                AttendancecountDic.Add(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EA", 1);
                                            else
                                            {
                                                double.TryParse(Convert.ToString(AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EA"]), out val);
                                                AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EA"] = val + 1;
                                            }
                                        }
                                        else if (Evening_attendance.Trim() == "3")
                                        {
                                            if (!AttendancecountDic.ContainsKey(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EOD"))
                                                AttendancecountDic.Add(Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EOD", 1);
                                            else
                                            {
                                                double.TryParse(Convert.ToString(AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EOD"]), out val);
                                                AttendancecountDic[Convert.ToString(tempdate.ToString("dd/MM/yyyy")) + "-EOD"] = val + 1;
                                            }
                                        }
                                    }
                                    #endregion
                                    #region Attendance Value Bind
                                    Fphostelcount.SaveChanges();
                                    if (AttendancecountDic.Count > 0)
                                    {
                                        if (cbmor.Checked)
                                        {
                                            for (int col = TotalHeader + 1; col < Fphostelcount.Sheets[0].ColumnCount; col++)
                                            {
                                                string headerDate = Convert.ToString(Fphostelcount.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                                string headerName = Convert.ToString(Fphostelcount.Sheets[0].ColumnHeader.Cells[1, col].Tag);
                                                if (!string.IsNullOrEmpty(headerName))
                                                {
                                                    if (AttendancecountDic.ContainsKey(headerDate + "-M" + headerName))
                                                    {
                                                        double PresentCount = 0;
                                                        double.TryParse(Convert.ToString(AttendancecountDic[headerDate + "-M" + headerName]), out PresentCount);
                                                        string PresentCnt = (Convert.ToString(PresentCount) == "0") ? " " : Convert.ToString(PresentCount);
                                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Text = Convert.ToString(PresentCnt);
                                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    }
                                                }
                                            }
                                        }
                                        if (cbeve.Checked)
                                        {
                                            for (int col = TotalHeader + 1; col < Fphostelcount.Sheets[0].ColumnCount; col++)
                                            {
                                                string headerDate = Convert.ToString(Fphostelcount.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                                string headerName = Convert.ToString(Fphostelcount.Sheets[0].ColumnHeader.Cells[1, col].Tag);
                                                if (!string.IsNullOrEmpty(headerName))
                                                {
                                                    if (AttendancecountDic.ContainsKey(headerDate + "-E" + headerName))
                                                    {
                                                        double PresentCount = 0;
                                                        double.TryParse(Convert.ToString(AttendancecountDic[headerDate + "-E" + headerName]), out PresentCount);
                                                        string PresentCnt = (Convert.ToString(PresentCount) == "0") ? " " : Convert.ToString(PresentCount);
                                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Text = Convert.ToString(PresentCnt);
                                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                        Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    }
                                                }
                                            }
                                        }
                                        if (cbboth.Checked)
                                        {
                                            for (int col = TotalHeader + 1; col < Fphostelcount.Sheets[0].ColumnCount; col++)
                                            {
                                                string headerDate = Convert.ToString(Fphostelcount.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                                string headerName = Convert.ToString(Fphostelcount.Sheets[0].ColumnHeader.Cells[1, col].Tag);
                                                if (!string.IsNullOrEmpty(headerName))
                                                {
                                                    double MornPresentCount = 0;
                                                    double EvenPresentCount = 0;
                                                    if (AttendancecountDic.ContainsKey(headerDate + "-E" + headerName))
                                                        double.TryParse(Convert.ToString(AttendancecountDic[headerDate + "-E" + headerName]), out EvenPresentCount);
                                                    if (AttendancecountDic.ContainsKey(headerDate + "-M" + headerName))
                                                        double.TryParse(Convert.ToString(AttendancecountDic[headerDate + "-M" + headerName]), out MornPresentCount);

                                                    string EvenPresentCnt = (Convert.ToString(EvenPresentCount) == "0") ? " " : Convert.ToString(EvenPresentCount);
                                                    string MornPresentCnt = (Convert.ToString(MornPresentCount) == "0") ? " " : Convert.ToString(MornPresentCount);

                                                    //   Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, col].Text = Convert.ToString(EvenPresentCnt);
                                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, col].Text = Convert.ToString(MornPresentCnt);
                                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, col].HorizontalAlign = HorizontalAlign.Center;
                                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, col].Font.Size = FontUnit.Medium;
                                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, col].Font.Name = "Book Antiqua";

                                                    //  Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Text = Convert.ToString(MornPresentCnt);
                                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Text = Convert.ToString(EvenPresentCnt);
                                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    tempdate = tempdate.AddDays(1);
                                }
                            }
                            #region Grand Total

                            if (cbboth.Checked)
                            {
                                Fphostelcount.Sheets[0].RowCount++;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].Text = "Morning Total";
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].HorizontalAlign = HorizontalAlign.Center;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].Font.Size = FontUnit.Medium;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].Font.Name = "Book Antiqua";
                                Fphostelcount.Sheets[0].Rows[Fphostelcount.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                                Fphostelcount.Sheets[0].Rows[Fphostelcount.Sheets[0].RowCount - 1].ForeColor = Color.IndianRed;
                                Fphostelcount.Sheets[0].RowCount++;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].Text = "Evening Total";
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].HorizontalAlign = HorizontalAlign.Center;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].Font.Size = FontUnit.Medium;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].Font.Name = "Book Antiqua";
                                Fphostelcount.Sheets[0].Rows[Fphostelcount.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                                Fphostelcount.Sheets[0].Rows[Fphostelcount.Sheets[0].RowCount - 1].ForeColor = Color.IndianRed;
                            }
                            Fphostelcount.Sheets[0].RowCount++;
                            Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].Text = "Total";
                            Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].HorizontalAlign = HorizontalAlign.Center;
                            Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].Font.Size = FontUnit.Medium;
                            Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, TotalHeader].Font.Name = "Book Antiqua";
                            Dictionary<string, double> GrandTotalDic = new Dictionary<string, double>();
                            for (int col = TotalHeader + 1; col < Fphostelcount.Sheets[0].ColumnCount; col++)
                            {
                                double TotalCount = 0;
                                double rowvalue = 0;
                                double MornVal = 0;
                                double EvenVal = 0;
                                string Session = string.Empty;
                                for (int row = 0; row < Fphostelcount.Sheets[0].RowCount; row++)
                                {
                                    rowvalue = 0;
                                    if (cbboth.Checked)
                                    {
                                        Session = Convert.ToString(Fphostelcount.Sheets[0].Cells[row, TotalHeader].Tag);
                                        if (Session.ToUpper() == "M")
                                        {
                                            double.TryParse(Convert.ToString(Fphostelcount.Sheets[0].Cells[row, col].Text), out rowvalue);
                                            MornVal += rowvalue;
                                        }
                                        else if (Session.ToUpper() == "E")
                                        {
                                            double.TryParse(Convert.ToString(Fphostelcount.Sheets[0].Cells[row, col].Text), out rowvalue);
                                            EvenVal += rowvalue;
                                        }
                                    }
                                    double.TryParse(Convert.ToString(Fphostelcount.Sheets[0].Cells[row, col].Text), out rowvalue);
                                    TotalCount += rowvalue;
                                    if (cbboth.Checked)
                                    {
                                        for (int c = 0; c < TotalHeader; c++)
                                            Fphostelcount.Sheets[0].SpanModel.Add(row, c, 2, 1);
                                    }
                                }
                                if (cbboth.Checked)
                                {
                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 3, col].Text = Convert.ToString(MornVal);
                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 3, col].HorizontalAlign = HorizontalAlign.Center;
                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 3, col].Font.Size = FontUnit.Medium;
                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 3, col].Font.Name = "Book Antiqua";
                                    Fphostelcount.Sheets[0].Rows[Fphostelcount.Sheets[0].RowCount - 3].BackColor = Color.Bisque;
                                    Fphostelcount.Sheets[0].Rows[Fphostelcount.Sheets[0].RowCount - 3].ForeColor = Color.IndianRed;

                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, col].Text = Convert.ToString(EvenVal);
                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, col].HorizontalAlign = HorizontalAlign.Center;
                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, col].Font.Size = FontUnit.Medium;
                                    Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 2, col].Font.Name = "Book Antiqua";
                                    Fphostelcount.Sheets[0].Rows[Fphostelcount.Sheets[0].RowCount - 2].BackColor = Color.Bisque;
                                    Fphostelcount.Sheets[0].Rows[Fphostelcount.Sheets[0].RowCount - 2].ForeColor = Color.IndianRed;
                                }
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Text = Convert.ToString(TotalCount);
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                Fphostelcount.Sheets[0].Cells[Fphostelcount.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                Fphostelcount.Sheets[0].Rows[Fphostelcount.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                                Fphostelcount.Sheets[0].Rows[Fphostelcount.Sheets[0].RowCount - 1].ForeColor = Color.IndianRed;
                            }

                            #endregion
                        }
                    }
                    Fphostelcount.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fphostelcount.Sheets[0].PageSize = Fphostelcount.Sheets[0].RowCount;
                    Fphostelcount.Visible = true;
                    rptprint.Visible = true;
                }
                else
                {
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select All Field";
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    div1.Visible = false;
                    Fphostelcount.Visible = false;
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode, "Hostelattendance_report.aspx");
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
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    //if (tborder.Text == "")
                    //{
                    //    ItemList.Add("Company Code");
                    //}
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder.Items[0].Selected = true;
                //    cblcolumnorder.Items[1].Selected = true;
                //    cblcolumnorder.Items[2].Selected = true;
                //}
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                //tborder.Text = tborder.Text + ItemList[i].ToString();
                //tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";
            }
            tborder.Text = colname12;
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
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
    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                tborder.Text = "";
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    // tborder.Text = tborder.Text + ItemList[i].ToString();
                }
                tborder.Text = colname12;
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    //cblcolumnorder.Items[0].Selected = true;
                }
                tborder.Text = "";
                tborder.Visible = false;
            }
            //  Button2.Focus();
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
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string hostelname = "";

            if (cbl_hostelname.Items.Count > 0)
            {

                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        if (hostelname == "")
                        {
                            hostelname = "" + cbl_hostelname.Items[i].Text.ToString() + "";
                        }
                        else
                        {
                            hostelname = hostelname + "," + cbl_hostelname.Items[i].Text.ToString() + "";
                        }
                    }
                }

            }
            if (rdbDetails.Checked)
            {

                if (rdb_Hostel.Checked == true)
                {
                    string degreedetails = "Hostel Absentees  Attendance Report";
                    string pagename = "hostel_attendance_report.aspx";
                    degreedetails = degreedetails + '@' + "Hostel Name: " + hostelname;
                    Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
                    Printcontrol.Visible = true;
                }
                else if (rdo_guest.Checked == true)
                {
                    string degreedetails = "Hostel Guest  Attendance Report";
                    string pagename = "hostel_attendance_report.aspx";
                    degreedetails = degreedetails + '@' + "Hostel Name: " + hostelname;
                    Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
                    Printcontrol.Visible = true;
                }
            }
            else if (rdbCumulative.Checked)
            {
                string degreedetails = "Daily Attendance Report" + '@' + "Hostel Name: " + hostelname + "@Date: " + Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                string pagename = "hostel_attendance_report.aspx";
                Printcontrol.loadspreaddetails(Fphostelcount, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
        }
        catch
        {
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdbDetails.Checked)
            {
                string reportname = txtexcelname.Text;
                if (reportname.ToString().Trim() != "")
                {
                    d2.printexcelreport(FpSpread1, reportname);
                    lblvalidation1.Visible = false;
                }
                else
                {
                    lblvalidation1.Text = "Please Enter Your Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
            }
            else if (rdbCumulative.Checked)
            {
                string reportname = txtexcelname.Text;
                if (reportname.ToString().Trim() != "")
                {
                    d2.printexcelreport(Fphostelcount, reportname);
                    lblvalidation1.Visible = false;
                }
                else
                {
                    lblvalidation1.Text = "Please Enter Your Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
            }
        }
        catch
        {
        }
    }
    //24.11.15   
    protected void rdb_hostel_SelectedIndexchange(object sender, EventArgs e)
    {
        if (rdb_Hostel.Checked == true)
        {
            pheaderfilter.Visible = true;
            pcolumnorder.Visible = true;
            rptprint.Visible = false;
            div1.Visible = false;
            FpSpread1.Visible = false;
            pheaderfilter1.Visible = false;
            pcolumnorder1.Visible = false;
        }
        bindcriteria();
    }
    protected void rdb_guest__SelectedIndexchange(object sender, EventArgs e)
    {
        if (rdo_guest.Checked == true)
        {
            pheaderfilter1.Visible = true;
            pcolumnorder1.Visible = true;
            rptprint.Visible = false;
            div1.Visible = false;
            FpSpread1.Visible = false;
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
        }
        bindcriteria();
    }
    public void cb_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string si = "";
            int j = 0;
            if (cb_column.Checked == true)
            {
                ItemList1.Clear();
                for (i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    //if (rdb_cumulative.Checked == true)
                    //{
                    //    ItemList1.Remove("Date");                    
                    //    Itemindex.Remove(si == "Date");
                    //    ItemList1.Remove("Description");
                    //    Itemindex.Remove(si == "Description");
                    //}
                    //else
                    //{
                    si = Convert.ToString(i);
                    cblcolumnorder1.Items[i].Selected = true;
                    lnk_columnorder1.Visible = true;
                    ItemList1.Add(cblcolumnorder1.Items[i].Value.ToString());
                    Itemindex1.Add(si);
                    //}
                }
                lnk_columnorder1.Visible = true;
                tborder1.Visible = true;
                tborder1.Text = "";
                for (i = 0; i < ItemList1.Count; i++)
                {
                    j = j + 1;
                    tborder1.Text = tborder1.Text + ItemList1[i].ToString();
                    tborder1.Text = tborder1.Text + "(" + (j).ToString() + ")  ";
                }
            }
            else
            {
                for (i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    cblcolumnorder1.Items[i].Selected = false;
                    lnk_columnorder1.Visible = false;
                    ItemList1.Clear();
                    Itemindex1.Clear();
                    cblcolumnorder1.Items[0].Enabled = false;
                }
                tborder1.Text = "";
                tborder1.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void lb_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder1.ClearSelection();
            cb_column.Checked = false;
            lnk_columnorder1.Visible = false;
            //cblcolumnorder1.Items[0].Selected = true;
            ItemList1.Clear();
            Itemindex1.Clear();
            tborder1.Text = "";
            tborder1.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_columnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int index;
            string value = "";
            string result = "";
            string sindex = "";
            cb_column.Checked = false;
            cblcolumnorder1.Items[0].Selected = true;
            cblcolumnorder1.Items[0].Enabled = false;
            value = string.Empty;
            result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            sindex = Convert.ToString(index);
            if (cblcolumnorder1.Items[index].Selected)
            {
                if (!Itemindex1.Contains(sindex))
                {
                    //if (tborder1.Text == "")
                    //{
                    //    ItemList1.Add("Roll No");
                    //}
                    //ItemList1.Add("Admission No");
                    //ItemList1.Add("Name");                   
                    ItemList1.Add(cblcolumnorder1.Items[index].Value.ToString());
                    Itemindex1.Add(sindex);
                }
            }
            else
            {
                ItemList1.Remove(cblcolumnorder1.Items[index].Value.ToString());
                Itemindex1.Remove(sindex);
            }
            for (i = 0; i < cblcolumnorder1.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder1.Items[0].Selected = true;
                //    cblcolumnorder1.Items[1].Selected = true;
                //    cblcolumnorder1.Items[2].Selected = true;
                //}
                if (cblcolumnorder1.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList1.Remove(cblcolumnorder1.Items[i].Value.ToString());
                    Itemindex1.Remove(sindex);
                }
            }
            lnk_columnorder1.Visible = true;
            tborder1.Visible = true;
            tborder1.Text = "";
            for (i = 0; i < ItemList1.Count; i++)
            {
                tborder1.Text = tborder1.Text + ItemList1[i].ToString();
                tborder1.Text = tborder1.Text + "(" + (i + 1).ToString() + ")  ";
            }
            if (ItemList1.Count == 22)
            {
                cb_column.Checked = true;
            }
            if (ItemList1.Count == 0)
            {
                tborder1.Visible = false;
                lnk_columnorder1.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    //cumlative report
    protected void cb_Batch_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_Batch, cb_Batch, txt_batch, lbl_batch.Text);
    }
    protected void cbl_Batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_Batch, cb_Batch, txt_batch, lbl_batch.Text);
    }
    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, lbl_branch.Text);
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_branch, cb_branch, txt_branch, lbl_branch.Text);
    }
    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
        bindbranch();
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
        bindbranch();
    }
    protected void cbCollegeCheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cblCollege, cbCollege, txtCollege, lbl_collegename.Text);
        binddegree();
        bindbranch();
    }
    protected void cblCollegeSelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cblCollege, cbCollege, txtCollege, lbl_collegename.Text);
        binddegree();
        bindbranch();
    }
    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            cblCollege.Items.Clear();
            string q1 = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"].ToString() + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblCollege.DataSource = ds;
                cblCollege.DataTextField = "collname";
                cblCollege.DataValueField = "college_code";
                cblCollege.DataBind();
                if (cblCollege.Items.Count > 0)
                {
                    for (int i = 0; i < cblCollege.Items.Count; i++)
                    {
                        cblCollege.Items[i].Selected = true;
                    }
                    txtCollege.Text = lbl_collegename.Text + "(" + cblCollege.Items.Count + ")";
                }
            }
        }
        catch { }
    }
    protected void bindbranch()
    {
        try
        {
            if (cbl_degree.Items.Count > 0)
            {
                string CourseID = rs.GetSelectedItemsValueAsString(cbl_degree);
                string CollegeCode = rs.GetSelectedItemsValueAsString(cblCollege);
                string query1 = " select distinct degree.degree_code,department.dept_name+'-'+degree.Acronym as dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + CourseID + "') and degree.college_code in('" + CollegeCode + "') and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "'";
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
                    }
                }
                else
                {
                    txt_branch.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void binddegree()
    {
        try
        {
            string query = string.Empty;
            string collegeCode = rs.GetSelectedItemsValueAsString(cblCollege);
            txt_degree.Text = "--Select--";
            if (!string.IsNullOrEmpty(collegeCode))
            {
                if (usercode != "")
                    query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in('" + collegeCode + "') and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
                else
                    query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code  in('" + collegeCode + "') and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + "";
                ds.Clear();
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
                            cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = lbl_degree.Text + "(" + cbl_degree.Items.Count + ")";
                    }
                }
                else
                    txt_degree.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void bind_batch()
    {
        try
        {
            ds.Clear();
            txt_batch.Text = "--Select--";
            ds = d2.select_method_wo_parameter("select distinct batch_year from tbl_attendance_rights order by batch_year desc", "text");
            cbl_Batch.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Batch.DataSource = ds;
                cbl_Batch.DataTextField = "batch_year";
                cbl_Batch.DataValueField = "batch_year";
                cbl_Batch.DataBind();
                if (cbl_Batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_Batch.Items.Count; i++)
                        cbl_Batch.Items[i].Selected = true;
                    txt_batch.Text = lbl_batch.Text + "(" + cbl_Batch.Items.Count + ")";
                }
            }
            else
                txt_batch.Text = "--Select--";
        }
        catch
        {
        }
    }
    protected void rdbDetails_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdbDetails.Checked == true)
            {
                cblcolumnorder.Items.Add("Total Days Count");
            }
            else
            {
                cblcolumnorder.Items.Remove("Total Days Count");
                ItemList.Remove("Total Days Count");

            }
        }
        catch
        {
        }
    }
    public void bindbuilding()
    {
        try
        {
            string hostel = "";
            drbbuilding.Items.Clear();
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        hostel = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostel = hostel + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (hostel != "")
            {
                string building = string.Empty;
                string build = "select HostelBuildingFK From  HM_HostelMaster where HostelMasterPK IN ('" + hostel + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(build, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        if (building == "")
                        {
                            building = "" + Convert.ToString(ds.Tables[0].Rows[i]["HostelBuildingFK"]) + "";
                        }
                        else
                        {
                            building = building + "" + "," + "" + Convert.ToString(ds.Tables[0].Rows[i]["HostelBuildingFK"]) + "";
                        }
                    }
                }

                if (building != "")
                {
                    string itemname = "select * from  Building_Master where code in(" + building + ")";



                    ds.Clear();
                    ds = d2.select_method_wo_parameter(itemname, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        drbbuilding.DataSource = ds;
                        drbbuilding.DataTextField = "Building_name";
                        drbbuilding.DataValueField = "code";
                        drbbuilding.DataBind();
                    }
                    bindfloor();
                    bindroom();
                }
            }
        }
        catch
        {
        }
    }

    public void bindroom()
    {
        try
        {
            string floor = "";

            for (int i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected == true)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_floor.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + cbl_floor.Items[i].Value.ToString() + "";
                    }
                }
            }
            cbl_room.Items.Clear();
            txt_room.Text = "---Select---";
            cb_room.Checked = false;
            string query = "";
            query = "select Room_Name,Roompk from Floor_Master f,Building_Master b,Room_Detail r where b.Building_Name=f.Building_Name and  r.Floor_Name=f.Floor_Name and r.Building_Name=f.Building_Name and f.Floorpk in('" + floor + "') and b.Code='" + Convert.ToString(drbbuilding.SelectedValue) + "'";
            //  query = "select distinct rd.Roompk,rd.Room_Name from Room_Detail rd,Floor_Master hd where rd.Floor_Name=hd.Floor_Name and hd.FloorPK in('" + floor + "')  order by Roompk";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_room.DataSource = ds;
                cbl_room.DataTextField = "Room_Name";
                cbl_room.DataValueField = "Roompk";
                cbl_room.DataBind();

                if (cbl_room.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_room.Items.Count; row++)
                    {
                        cbl_room.Items[row].Selected = true;
                    }
                    txt_room.Text = "Room (" + cbl_room.Items.Count + ")";
                    cb_room.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void drbbuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindfloor();
            bindroom();
        }
        catch
        {
        }
    }
    protected void cb_room_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_room.Text = "--Select--";
            if (cb_room.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = true;
                }
                txt_room.Text = "Room (" + (cbl_room.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = false;
                }
                txt_room.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_room_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_room.Checked = false;
        int commcount = 0;

        txt_room.Text = "--Select--";

        for (int i = 0; i < cbl_room.Items.Count; i++)
        {
            if (cbl_room.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_room.Items.Count)
            {
                cb_room.Checked = true;
            }
            txt_room.Text = "Room (" + commcount.ToString() + ")";
        }
    }
}