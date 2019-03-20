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
using FarPoint.Web.Spread;
using System.Collections;
using Gios.Pdf;
public partial class Biohostel_new : System.Web.UI.Page
{
    double percentage;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlDataAdapter da = new SqlDataAdapter();
    SqlCommand cmd = new SqlCommand();
    SqlDataReader dr;
    SqlDataReader drcount25;
    SqlDataReader drcount26;
    SqlDataReader drcount27;
    SqlDataReader drcount28;
    ReuasableMethods rs = new ReuasableMethods();
    string hostelattend = "";
    string hostelattend1 = "";
    string hostelattend2 = "";
    string hostelattend3 = "";
    DataSet ds = new DataSet();
    DataSet dsbind = new DataSet();
    DataSet dset = new DataSet();
    string Str;
    string sql;
    int day3;
    bool gracetimeflag = false;
    bool ontimeflag = false;
    bool Generalflag = true;
    int ontime1 = 0;
    string strdate;
    //string strdate;
    string enddate;
    string partdate;
    string date = "";
    // string date2="";
    string date1;
    string dateparti;
    string dateformat;
    string datefrom;
    string dateto;
    string Att_changedate;
    string date2;
    string date3;
    string dateupto;
    string Att_dateformate;
    string Att_changryear;
    string today;
    string datetoday;
    string strTime;
    string strbwTime = "";
    int countpresent = 0;
    int countabsent = 0;
    int countpermission = 0;
    int countlate = 0;

    int countpresenteve = 0;
    int countabsenteve = 0;
    int countpresenteve2 = 0;
    int counttotalmornpresent = 0;
    int counttotalevennpresent = 0;
    int counttotalabsentmorn = 0;
    int counttotalabsenteven = 0;
    DataSet dsparentde = new DataSet();
    int totalmornlate = 0;
    int totalevenlate = 0;
    int totalpermorn = 0;
    int totalpereven = 0;

    static int batchcnt = 0;
    double totalperesent = 0;
    double totalabsent = 0;
    int countlateeve = 0;
    double totallate = 0;
    double totalpermission = 0;
    int countpermissioneve = 0;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    int totalcountevennpermission = 0;
    int totallatecount = 0;
    // Att_changryear
    int countpresent2;
    int countabsent2;
    int countpermission2;
    int countlate2;
    double totalpresent;
    double g;
    int counttotalpresenr = 0;
    int counttotalabsent = 0;
    double c;
    int d;
    Hashtable htcolumn = new Hashtable();
    static string order_by_var = "";
    DAccess2 d2 = new DAccess2();
    [Serializable()]
    public class MyImg : ImageCellType
    {
        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(50);
            return img;
        }
    }
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
        //if (Session["collegecode"] == null) //Aruna For Back Button
        //{
        //    Response.Redirect("~/Default.aspx");
        //}

        if (!IsPostBack)
        {
            ViewState["unreg"] = null;
            ViewState["Bothpresent"] = null;
            ViewState["BothAbsent"] = null;
            ViewState["Bothod"] = null;
            ViewState["Bothper"] = null;
            rdb_deptacr.Checked = true;
            Txtentryfrom.Attributes.Add("readonly", "readonly");
            Txtentryto.Attributes.Add("readonly", "readonly");
            order_by_var = "";
            filteration();
            fpbiomatric.Visible = false;
            gracetimeflag = false;
            ontimeflag = false;
            Generalflag = true;
            //Display current Date In the Text Box
            string today = System.DateTime.Now.ToString();
            string today1;
            string[] split13 = today.Split(new char[] { ' ' });
            string[] split14 = split13[0].Split(new Char[] { '/' });
            today1 = split14[1].ToString() + "/" + split14[0].ToString() + "/" + split14[2].ToString();
            Txtentryfrom.Text = today1;
            string today2 = System.DateTime.Now.ToString();
            string today3;
            string[] split15 = today.Split(new char[] { ' ' });
            string[] split16 = split13[0].Split(new Char[] { '/' });
            today3 = split16[1].ToString() + "/" + split16[0].ToString() + "/" + split16[2].ToString();
            Txtentryto.Text = today3;
            //fpbiomatric.Visible = false;
            load_hostelname();
            bindcollege();
            ddlstud.Visible = false;

            //ddlBranch.Items.Insert(0, "Select");
            ddlSemYr.Items.Insert(0, "Select");
            ddlSec.Items.Insert(0, "Select");

            cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''order by batch_year", con);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);

            DataSet ds1 = new DataSet();
            da1.Fill(ds1);

            //ddlBatch.DataSource = ds1;
            //ddlBatch.DataValueField = "batch_year";
            //ddlBatch.DataBind();
            //ddlBatch.Items.Insert(0, "Select");
            cbl_batchyear.DataSource = ds1;
            cbl_batchyear.DataValueField = "batch_year";
            cbl_batchyear.DataBind();
            //course
            con.Open();
            cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + Session["collegecode"] + " order by course.course_name ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            //ddlDegree.DataSource = ds;
            //ddlDegree.DataValueField = "course_id";
            //ddlDegree.DataTextField = "course_name";
            //ddlDegree.DataBind();
            //ddlDegree.Items.Insert(0, "Select");
            cbl_degree.DataSource = ds;
            cbl_degree.DataValueField = "course_id";
            cbl_degree.DataTextField = "course_name";
            cbl_degree.DataBind();
            con.Close();

            cblsearch.Items[0].Selected = true;
            cblsearch.Items[1].Selected = true;
            cblsearch.Items[2].Selected = true;
            //cblsearch.Items[5].Selected = true;
            //cblsearch.Items[6].Selected = true;
            //cblsearch.Items[7].Selected = true;

            load_rollno();
            load_studname();
            fpbiomatric.CommandBar.Visible = false;
            rdoboth1.Checked = true;
            ddlstud.Visible = false;
        }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = fpbiomatric.FindControl("Update");
        Control cntCancelBtn = fpbiomatric.FindControl("Cancel");
        Control cntCopyBtn = fpbiomatric.FindControl("Copy");
        Control cntCutBtn = fpbiomatric.FindControl("Clear");
        Control cntPasteBtn = fpbiomatric.FindControl("Paste");
        Control cntPageNextBtn = fpbiomatric.FindControl("Next");
        Control cntPagePreviousBtn = fpbiomatric.FindControl("Prev");
        Control cntPagePrintBtn = fpbiomatric.FindControl("Print");

        Control cntPagePrintpdfBtn = fpbiomatric.FindControl("PrintPDF");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);

            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePreviousBtn.Parent;
            //tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePrintpdfBtn.Parent;
            tr.Cells.Remove(tc);
        }
        base.Render(writer);
    }
    void load_floorname()
    {
        try
        {
            cbl_floorName.Items.Clear();
            string itemname = "select distinct floor_name,floorpk from HT_HostelRegistration h,Floor_Master fm where h.FloorFK=fm.Floorpk  and fm.College_Code='" + Convert.ToString(Session["collegecode"]) + "'";
            //if (Cbo_HostelName.Text != "All")
            //{
            //    itemname = itemname + " and h.HostelMasterFK in('" + Convert.ToString(Cbo_HostelName.SelectedItem.Value) + "')";
            //}
            string hostelnameget = string.Empty;
            if (txthostelname.Text.ToString() != "--Select--")
            {
                if (cbl_hostelname.Items.Count > 0)
                    hostelnameget = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                itemname = itemname + " and h.HostelMasterFK in('" + hostelnameget + "')";

            }
            string buildingname = string.Empty;
            if (txt_building.Text.ToString() != "--Select--")
            {
                if (cbl_building.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_building.Items.Count; i++)
                    {
                        if (cbl_building.Items[i].Selected == true)
                        {
                            if (buildingname == "")
                            {
                                buildingname = Convert.ToString(cbl_building.Items[i].Text);
                            }
                            else
                            {
                                buildingname = buildingname + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                            }
                        }
                    }
                    itemname = itemname + " and  Building_Name in('" + buildingname + "')";
                }

            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorName.DataSource = ds.Tables[0];
                cbl_floorName.DataTextField = "floor_name";
                cbl_floorName.DataValueField = "floorpk";
                cbl_floorName.DataBind();
                //cbofloorname.Items.Insert(0, "All");
            }
        }
        catch
        {

        }
    }
    void load_rollno()
    {
        cbl_rollnum.Items.Clear();
        string sql_query = "";
        try
        {
            sql_query = sql_query + " select r.Roll_No,r.App_No from HT_HostelRegistration h,Registration r,Degree G, Course C ,Department D where r.App_No=h.APP_No  and r.degree_code=g.Degree_Code and g.Course_Id=c.Course_Id and g.Dept_Code =d.Dept_Code and isnull(IsSuspend,0)=0 and isnull(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and r.college_code='" + Convert.ToString(Session["collegecode"]) + "' ";

            string courseid = string.Empty;
            string batchyr = string.Empty;
            string branch = string.Empty;
            if (txt_degree.Text.ToString() != "--Select--")
            {
                if (cbl_degree.Items.Count > 0)
                    courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                sql = sql + " AND C.Course_id in('" + courseid + "')";

            }
            if (txt_batchyr.Text.ToString() != "--Select--")
            {
                if (cbl_batchyear.Items.Count > 0)
                    batchyr = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                sql = sql + " AND T.Batch_Year in('" + batchyr + "')";


            }
            if (txtbranch.Text.ToString() != "--Select--")
            {
                if (cbl_branch.Items.Count > 0)
                    branch = rs.GetSelectedItemsValueAsString(cbl_branch);
                sql = sql + " AND G.Degree_code in('" + branch + "')";

            }
            //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
            //{
            //    sql_query = sql_query + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
            //}
            //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
            //{
            //    sql_query = sql_query + " AND r.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
            //}
            //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
            //{
            //    sql_query = sql_query + " AND G.Degree_code='" + ddlBranch.SelectedItem.Value.ToString() + "'";
            //}
            string hostel_name = string.Empty;
            string flrname = string.Empty;
            if (txthostelname.Text.ToString() != "--Select--")
            {
                if (cbl_hostelname.Items.Count > 0)
                    hostel_name = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                sql_query = sql_query + " And h.HostelMasterFK in('" + hostel_name + "')";

            }
            //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
            //{
            //    sql_query = sql_query + " And h.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
            //}
            string rooms_nums = string.Empty;
            if (txtroom_no.Text.ToString() != "--Select--")
            {
                if (cbl_room_no.Items.Count > 0)
                {
                    rooms_nums = rs.GetSelectedItemsValueAsString(cbl_room_no);
                    sql_query = sql_query + " AND h.RoomFK in('" + rooms_nums + "')";
                }

            }
            //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
            //{
            //    sql_query = sql_query + " AND h.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";
            //}
            if (txtfloorname.Text.ToString() != "--Select--")
            {
                if (cbl_floorName.Items.Count > 0)
                {
                    flrname = rs.GetSelectedItemsValueAsString(cbl_floorName);
                    sql_query = sql_query + " and h.floorfk in('" + flrname + "')";
                }

            }
            //if (cbofloorname.SelectedItem.Value.ToString() != "All")
            //{
            //    sql_query = sql_query + " and h.floorfk='" + cbofloorname.SelectedItem.Value.ToString() + "'";
            //}
            sql_query = sql_query + " order by Roll_No";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql_query, "TExt");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_rollnum.DataSource = ds;
                cbl_rollnum.DataTextField = "roll_no";
                cbl_rollnum.DataValueField = "App_No";
                cbl_rollnum.DataBind();
                // cboroll.Items.Insert(0, "All");
            }
        }
        catch { }
    }
    void load_studname()
    {
        try
        {
            cbl_studeName.Items.Clear();
            string sql_query = " select r.Stud_Name,r.App_No from HT_HostelRegistration h,Registration r,Degree G, Course C ,Department D where r.App_No=h.APP_No  and r.degree_code=g.Degree_Code and g.Course_Id=c.Course_Id and g.Dept_Code =d.Dept_Code and isnull(IsSuspend,0)=0 and isnull(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and r.college_code='" + Convert.ToString(Session["collegecode"]) + "' ";

            string courseid = string.Empty;
            string batchyr = string.Empty;
            string branch = string.Empty;
            if (txt_degree.Text.ToString() != "--Select--")
            {
                if (cbl_degree.Items.Count > 0)
                    courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                sql = sql + " AND C.Course_id in('" + courseid + "')";

            }
            if (txt_batchyr.Text.ToString() != "--Select--")
            {
                if (cbl_batchyear.Items.Count > 0)
                    batchyr = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                sql = sql + " AND T.Batch_Year in('" + batchyr + "')";


            }
            if (txtbranch.Text.ToString() != "--Select--")
            {
                if (cbl_branch.Items.Count > 0)
                    branch = rs.GetSelectedItemsValueAsString(cbl_branch);
                sql = sql + " AND G.Degree_code in('" + branch + "')";

            }
            //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
            //{
            //    sql_query = sql_query + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
            //}
            //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
            //{
            //    sql_query = sql_query + " AND r.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
            //}
            //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
            //{
            //    sql_query = sql_query + " AND G.Degree_code='" + ddlBranch.SelectedItem.Value.ToString() + "'";
            //}
            string hostelnameS = string.Empty;
            string flrname = string.Empty;
            string rmsnum = string.Empty;
            if (txthostelname.Text.ToString() != "--Select--")
            {
                if (cbl_hostelname.Items.Count > 0)
                    hostelnameS = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                sql_query = sql_query + " And h.HostelMasterFK in('" + hostelnameS + "')";


            }
            //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
            //{
            //    sql_query = sql_query + " And h.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
            //}

            if (txtroom_no.Text.ToString() != "--Select--")
            {
                if (cbl_room_no.Items.Count > 0)
                {
                    rmsnum = rs.GetSelectedItemsValueAsString(cbl_room_no);
                    sql_query = sql_query + " AND h.RoomFK in('" + rmsnum + "')";
                }

            }
            //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
            //{
            //    sql_query = sql_query + " AND h.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";
            //}

            if (txtfloorname.Text.ToString() != "--Select--")
            {
                if (cbl_floorName.Items.Count > 0)
                {
                    flrname = rs.GetSelectedItemsValueAsString(cbl_floorName);
                    sql_query = sql_query + " and h.floorfk in('" + flrname + "')";
                }

            }
            //if (cbofloorname.SelectedItem.Value.ToString() != "All")
            //{
            //    sql_query = sql_query + " and h.floorfk='" + cbofloorname.SelectedItem.Value.ToString() + "'";
            //}
            sql_query = sql_query + " order by Roll_No";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql_query, "TExt");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_studeName.DataSource = ds;
                cbl_studeName.DataTextField = "Stud_Name";
                cbl_studeName.DataValueField = "App_No";
                cbl_studeName.DataBind();
                // cbostudentname.Items.Insert(0, "All");
            }
        }
        catch
        {

        }

    }


    void load_hostelname()
    {
        try
        {
            //Cbo_HostelName.Items.Clear();
            //ds.Clear();
            //string q = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelName";//where CollegeCode in ('" + Convert.ToString(Session["collegecode"]) + "')
            //ds = d2.select_method_wo_parameter(q, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    Cbo_HostelName.DataSource = ds;
            //    Cbo_HostelName.DataTextField = "HostelName";
            //    Cbo_HostelName.DataValueField = "HostelMasterPK";
            //    Cbo_HostelName.DataBind();
            //    Cbo_HostelName.Items.Insert(0, "All");
            //}

            cbl_hostelname.Items.Clear();
            ds.Clear();
            string q = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelName";//where CollegeCode in ('" + Convert.ToString(Session["collegecode"]) + "')
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();

            }
            load_floorname();
            load_room();
        }
        catch
        {

        }

    }
    void load_room()
    {
        try
        {
            cbl_room_no.Items.Clear();
            string hostelname = string.Empty;
            //string room = " select distinct Room_Name,Roompk from Room_Detail r,HT_HostelRegistration h where h.RoomFK =r.RoomPk ";
            string room = "select Room_Name,Roompk from Floor_Master f,Building_Master b,Room_Detail r where b.Building_Name=f.Building_Name and  r.Floor_Name=f.Floor_Name and r.Building_Name=f.Building_Name  ";

            //if (txthostelname.Text.ToString() != "--Select--")
            //{
            //    if (cbl_hostelname.Items.Count > 0)
            //        hostelname = rs.GetSelectedItemsValueAsString(cbl_hostelname);
            //    room = room + "  and h.HostelMasterFK in('" + hostelname + "')";
            //}
            string building = "";
            if (txt_building.Text.ToString() != "Select--")
            {
                if (cbl_building.Items.Count > 0)
                    building = rs.GetSelectedItemsValueAsString(cbl_building);
                room = room + "  and b.Code in('" + building + "')";
            
            }
            //if (Cbo_HostelName.SelectedItem.Text != "All")
            //{
            //    room = room + "  and h.HostelMasterFK in('" + Cbo_HostelName.SelectedItem.Value.ToString() + "')";
            //}
            string flrname = string.Empty;
            if (txtfloorname.Text.ToString() != "--Select--")
            {
                if (cbl_floorName.Items.Count > 0)
                {
                    flrname = rs.GetSelectedItemsValueAsString(cbl_floorName);
                    room = room + "  and f.Floorpk in('" + flrname + "')";
                }

            }
            //if (cbofloorname.SelectedItem.Text.ToString() != "All")
            //{
            //    room = room + "  and FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
            //}
            room = room + "  order by Room_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(room, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_room_no.DataSource = ds;
                cbl_room_no.DataTextField = "Room_Name";
                cbl_room_no.DataValueField = "Roompk";
                cbl_room_no.DataBind();
                //Cbo_Room.Items.Insert(0, "All");
            }
        }
        catch { }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        fpbiomatric.Visible = true;
        fpbiomatric.Visible = true;

        lblnorec.Visible = false;
        imgabsent.Visible = true;
        lblheaderabsent1.Visible = true;
        lblheaderabsent2.Visible = true;
        imgper.Visible = true;
        lblmornper.Visible = true;
        lblevenper.Visible = true;
        lblpermission.Visible = true;
        lblpresent1.Visible = true;
        lblpresent2.Visible = true;
        lbl_headermorn.Visible = true;
        lbl_headereven.Visible = true;
        lbllate.Visible = true;
        lbllate1.Visible = true;
        imglate.Visible = true;
        lblmornlate.Visible = true;
        lblevenlate.Visible = true;
        imgabsent.Visible = true;
        lblheaderabsent1.Visible = true;
        lblheaderabsent2.Visible = true;
        lblabsent1.Visible = true;
        lblabsent2.Visible = true;
        imgpresent.Visible = true;
        // Panel5.Visible = true;
        imglate.Visible = true;
        lbllate.Visible = true;
        imgper.Visible = true;
        lblmornper.Visible = true;
        lblevenper.Visible = true;
        lblpermission.Visible = true;
        lblpermission1.Visible = true;
        //imgontime.Visible = true;
        //lblontime.Visible = true;

        load_click();
    }
    void load_click()
    {
        try
        {
            Fieldset1.Visible = false;
            Fieldset2.Visible = false;
            btnprintmaster.Visible = true;
            rptprint.Visible = true;
            lblpresent1.Text = ":" + "0";
            lblpresent2.Text = ":" + "0";
            lblabsent1.Text = ":" + "0";
            lblabsent2.Text = ":" + "0";
            lbllate.Text = ":" + "0";
            lbllate1.Text = ":" + "0";
            lblpermission.Text = ":" + "0";
            lblpermission1.Text = ":" + "0";
            //lblontime.Text = ":" + "0";
            lblnorec.Visible = false;
            countpresent = 0;
            countabsent = 0;
            countlate = 0;
            countpermission = 0;
            ontime1 = 0;
            Hashtable hat = new Hashtable();
            fpbiomatric.Sheets[0].AutoPostBack = true;
            // Panel5.Visible = true;
            string tempstaffcode = "";
            //  CheckBoxselect.Checked = true;
            string attmark_CL = "";
            Boolean filterattnd = false;

            if (cbo_att.Items.Count != null)
            {
                int itemcount = 0;
                for (itemcount = 0; itemcount < cbo_att.Items.Count; itemcount++)
                {
                    string PreorAbs = "";
                    if (cbo_att.Items[itemcount].Selected == true)
                    {
                        if (cbo_att.Items[itemcount].Text == "P")
                            PreorAbs = "1";
                        else
                            PreorAbs = "2";
                        if (rdoinonly.Checked == true && rdb_even.Checked == true)
                        {
                            if (attmark_CL.Trim() == "")
                                attmark_CL = " att like '%-" + PreorAbs + "'";
                            else
                                attmark_CL = attmark_CL + " or att like '%-" + PreorAbs + "'";

                        }
                        else if (rdooutonly.Checked == true && rdb_morn.Checked == true)
                        {

                            if (attmark_CL.Trim() == "")
                                attmark_CL = " att like '" + PreorAbs + "-%'";
                            else
                                attmark_CL = attmark_CL + " or att like '" + PreorAbs + "-%'";
                        }
                        else if (rdoinandout.Checked == true)
                        {
                            if (attmark_CL.Trim() == "")
                                attmark_CL = " (att like '" + PreorAbs + "-" + PreorAbs + "')";
                            else
                                attmark_CL = attmark_CL + " or (att like '" + PreorAbs + "-" + PreorAbs + "')";
                        }
                    }
                }
            }

            if (attmark_CL.TrimEnd().ToString() != "")
            {
                Str = " and (" + attmark_CL + ")";

            }
            string dept = "";
            if (rdb_deptname.Checked == true)
            {
                dept = "Dept_Name";
            }
            else if (rdb_deptacr.Checked == true)
            {
                dept = "Dept_acronym";
            }
            if (cblsearch.Visible == true)
            {
                if (ItemList.Count == 0)
                {
                    ItemList.Add("Roll No");
                    ItemList.Add("Student Name");
                    ItemList.Add("Department");

                }
            }
            htcolumn.Clear();
            htcolumn.Add("0", "Roll No");
            htcolumn.Add("1", "Student Name");
            htcolumn.Add("2", "Department");
            htcolumn.Add("3", "Hostel Name");
            htcolumn.Add("4", "Room No");
            htcolumn.Add("5", "Father Name");
            htcolumn.Add("6", "Father MobileNo");
            htcolumn.Add("7", "Mother Name");
            htcolumn.Add("8", "Mother MobileNo");
            htcolumn.Add("9", "Student MobileNo");
            htcolumn.Add("10", "In Time");
            htcolumn.Add("11", "Out Time");
            htcolumn.Add("12", "Attendance");
            htcolumn.Add("13", "Finger ID");

            if (rdoinandout.Checked == true)
            {
                #region In and Out
                //attfiltertype.Visible = true;

                lbllatetext.Visible = false;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                imgabsent.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;
                imgpresent.Visible = true;
                imglate.Visible = true;
                lblmornlate.Visible = true;
                lblevenlate.Visible = true;
                imgper.Visible = true;
                lblmornper.Visible = true;
                lblevenper.Visible = true;
                //imgontime.Visible = true;
                //lblontime.Visible = true;

                lblpermission.Visible = true;
                lblpermission1.Visible = true;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lbllate.Visible = true;
                lbllate1.Visible = true;
                //  CheckBoxselect.Visible = true;
                string[] search = new string[50];

                cblsearch.Items[0].Selected = true;
                cblsearch.Items[1].Selected = true;
                cblsearch.Items[2].Selected = true;
                tborder.Visible = true;
                tborder.Text = "Roll No(1),Student Name(2),Department(3)";
                if (cblsearch.Items[0].Selected == true)
                {
                    search[0] = "staffmaster.staff_code";
                }
                if (cblsearch.Items[1].Selected == true)
                {
                    search[1] = "staffmaster.staff_name";
                }
                if (cblsearch.Items[2].Selected == true)
                {
                    search[2] = "hrdept_master.dept_name";
                }
                if (cblsearch.Items[3].Selected == true)
                {
                    search[3] = "dept_acronym";
                }
                if (cblsearch.Items[4].Selected == true)
                {
                    search[4] = "desig_master.desig_name";
                }
                if (cblsearch.Items[5].Selected == true)
                {
                    search[5] = " desig_master.desig_acronym";
                }
                if (cblsearch.Items[6].Selected == true)
                {
                    search[6] = "CONVERT(VARCHAR(10),staffmaster.join_date,103)";
                }
                if (cblsearch.Items[7].Selected == true)
                {
                    search[7] = "in_out_time.category_name";
                }
                string wsearch = "";
                int count = 0;
                for (int i = 0; i < cblsearch.Items.Count; i++)
                {
                    if (cblsearch.Items[i].Selected == true)
                    {
                        count = count + 1;
                    }
                }
                #region Header

                fpbiomatric.Sheets[0].RowCount = 1;
                fpbiomatric.Sheets[0].ColumnCount = 0;
                //fpbiomatric.CommandBar.Visible = false;
                fpbiomatric.RowHeader.Visible = true;
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";

                fpbiomatric.Sheets[0].PageSize = 10;
                fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpbiomatric.Pager.Align = HorizontalAlign.Right;
                fpbiomatric.Pager.Font.Bold = true;
                fpbiomatric.Pager.Font.Name = "Book Antiqua";
                fpbiomatric.Pager.ForeColor = Color.DarkGreen;
                fpbiomatric.Pager.BackColor = Color.AliceBlue;
                fpbiomatric.Pager.PageCount = 5;
                fpbiomatric.Sheets[0].SheetCorner.RowCount = 2;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnCount = ItemList.Count + 1;
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();

                string coltext1 = "";
                int insdex = 0;
                int colcount1 = 0;
                foreach (string key in htcolumn.Keys)
                {
                    coltext1 = htcolumn[key].ToString();
                    if (ItemList.Contains(Convert.ToString(coltext1)))
                    {
                        insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                        //FpSpread1.Columns[insdex].Locked = true;
                        fpbiomatric.Columns[insdex + 1].Width = 50;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Text = Convert.ToString(coltext1);
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Bold = true;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Name = "Book Antiqua";
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Size = FontUnit.Medium;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].HorizontalAlign = HorizontalAlign.Center;
                        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, insdex, 2, 1);
                    }
                }


                fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 5;
                int col = fpbiomatric.Sheets[0].ColumnCount - 1;
                int a = fpbiomatric.Sheets[0].ColumnCount - 1;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 5].Text = "Att  %";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 5, 70);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 5, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 4].Text = "P";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 4, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 3].Text = "A";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 3, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 2].Text = "LA";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 2, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 1].Text = "PER";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 1, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 1, 2, 1);

                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = " and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];
                    lbldate.Visible = false;
                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount - 1 + 4;
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = Txtentryfrom.Text.ToString();
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = datefrom.ToString();
                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;

                    if (chk_Time.Checked && Chktimein.Checked == true)
                    {
                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;

                    }
                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = split12[0].ToString() + "/" + split12[1].ToString() + "/" + split12[2].ToString(); ;
                        fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                        if (chk_Time.Checked && Chktimein.Checked == true)
                        {
                            fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;

                        }

                    }
                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }
                #endregion

                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }
                sql = sql + "  select  rm.Room_Name,r.HostelMasterFK, h.hostelname, t.App_No,T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree, b.Access_Date , right(CONVERT(nvarchar(100),time_in ,100),7) as Time_In, right(CONVERT(nvarchar(100),time_Out ,100),7) as Time_Out,Att,t.Finger_id  from HT_HostelRegistration r,Registration t,Degree G,Course C, HM_HostelMaster h,Department D,Bio_Attendance B,Room_Detail rm where r.RoomFK=rm.Roompk and h.HostelMasterPK =r.HostelMasterFK and r.APP_No  = T.App_No And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and T.Roll_No = B.Roll_No AND B.Is_Staff=0 AND B.Latestrec ='1'" + strdate + "   and ISNULL(r.IsDiscontinued,0)=0 and ISNULL(r.IsSuspend,0)=0 and ISNULL(r.IsVacated,0)=0 and  rm.Roompk=r.RoomFK and isnull(Time_Out,'')<>'' and r.CollegeCode in('" + colegecode + "') " + Str + "";

                string hostelnameval = string.Empty;
                if (txthostelname.Text.ToString() != "--Select--")
                {
                    if (cbl_hostelname.Items.Count > 0)
                        hostelnameval = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                    sql = sql + " And R.HostelMasterFK in('" + hostelnameval + "')";

                }

                string buildingname = string.Empty;
                if (txt_building.Text.ToString() != "--Select--")
                {
                    if (cbl_building.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_building.Items.Count; i++)
                        {
                            if (cbl_building.Items[i].Selected == true)
                            {
                                if (buildingname == "")
                                {
                                    buildingname = Convert.ToString(cbl_building.Items[i].Text);
                                }
                                else
                                {
                                    buildingname = buildingname + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                                }
                            }
                        }
                        sql = sql + " and  Building_Name in('" + buildingname + "')";
                    }

                }
                //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " And R.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                //}

                string rm_name = string.Empty;
                if (txtroom_no.Text.ToString() != "--Select--")
                {
                    if (cbl_room_no.Items.Count > 0)
                    {
                        rm_name = rs.GetSelectedItemsValueAsString(cbl_room_no);
                        sql = sql + " AND R.RoomFK in('" + rm_name + "')";
                    }

                }
                //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND R.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";
                //}
                string flrname = string.Empty;
                if (txtfloorname.Text.ToString() != "--Select--")
                {
                    if (cbl_floorName.Items.Count > 0)
                    {
                        flrname = rs.GetSelectedItemsValueAsString(cbl_floorName);
                        sql = sql + " and R.FloorFK in('" + flrname + "')";
                    }

                }
                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}
                string studroll = string.Empty;
                if (txtrollnum.Text.ToString() != "--Select--")
                {
                    if (cbl_rollnum.Items.Count > 0)
                    {
                        studroll = rs.GetSelectedItemsValueAsString(cbl_rollnum);
                        sql = sql + " AND T.App_No in('" + studroll + "')";

                    }

                }
                string courseids = string.Empty;
                string batchyrs = string.Empty;
                string branchs = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseids = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseids + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyrs = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyrs + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branchs = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branchs + "')";

                }
                //if (cboroll.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND T.App_No ='" + cboroll.Text + "'";
                //}
                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND G.Degree_code='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}
                string studName = string.Empty;
                if (txtstudename.Text.ToString() != "--Select--")
                {
                    if (cbl_studeName.Items.Count > 0)
                    {
                        studName = rs.GetSelectedItemsValueAsString(cbl_studeName);
                        sql = sql + " AND T.App_No in('" + studName + "')";

                    }

                }
                //if (cbostudentname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and t.App_No='" + cbostudentname.SelectedItem.Value + "' ";
                //}
                if (Chktimein.Checked == true && !chk_Time.Checked)
                {
                    //strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between ' " + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and ' " + cbo_hrinto.Text + ":" + cbo_mininto.Text +cbointo.Text + "'";
                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                    sql = sql + " " + strTime + "";
                }
                if (Chktimeout.Checked == true && !chk_Time.Checked)
                {
                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between ' " + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + "'  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + cbo_sec2.Text + "'";
                    sql = sql + " " + strTime + "";
                }


                if (ddlorder.SelectedItem.Text == "Select")
                {
                    sql = sql + order_by_var;
                }
                else if (ddlorder.SelectedItem.Text == "Hostel")
                {
                    sql = sql + " order by h.HostelName,rm.Room_Name";
                }
                else if (ddlorder.SelectedItem.Text == "Room")
                {
                    sql = sql + " order by rm.Room_Name";

                }
                //   sql = sql + order_by_var;
                string str2 = getfunction3(sql);
                con1.Open();
                SqlDataReader drname;
                SqlCommand cmd2 = new SqlCommand(sql, con1);
                drname = cmd2.ExecuteReader();
                int colcount;
                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                totallatecount = 0;
                totalcountevennpermission = 0;
                totalevenlate = 0;
                totalmornlate = 0;
                totalpermorn = 0;
                totalpereven = 0;

                while (drname.Read())
                {
                    if (drname.HasRows == true)
                    {
                        countpresent2 = 0;
                        countabsent2 = 0;
                        countpermission2 = 0;
                        countlate2 = 0;
                        sql = "";
                        // Str = "";
                        string rollno;
                        string hostelcode2 = "";
                        string timein3 = "";
                        hostelcode2 = drname["HostelMasterFK"].ToString();
                        rollno = drname["App_No"].ToString();
                        if (!hat.Contains(rollno))
                        {
                            hat.Add(rollno, rollno);
                            timein3 = drname["time_in"].ToString();
                            int countcolumn = fpbiomatric.Sheets[0].ColumnCount;
                            //colcount = colcount + 5;
                            int columncount = 0;

                            if (chk_Time.Checked && Chktimein.Checked == true)
                                columncount = 6;
                            else
                                columncount = 4;

                            for (colcount = col; colcount <= Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount) - 1; colcount = colcount + columncount)
                            {
                                if (chk_Time.Checked && Chktimein.Checked == true)
                                    fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 6);
                                else
                                    fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 4);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Out";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Mor";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);

                                //30.04.16
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "IN";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Eve";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 60);
                                if (chk_Time.Checked && Chktimein.Checked == true)
                                {
                                    fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Text = "Between Time";
                                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                                    fpbiomatric.Sheets[0].SetColumnWidth(colcount + 4, 60);

                                    fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Text = "Att";
                                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                    fpbiomatric.Sheets[0].SetColumnWidth(colcount + 5, 60);

                                }


                                string datetagvalue;
                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();
                                strdate = " and B.access_date='" + datetagvalue + "'";

                                sql = "SELECT  rm.room_name,r.HostelMasterFK , hm.hostelname,t.app_no,  T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree, b.Access_Date , right(CONVERT(nvarchar(100),time_in ,100),7) as Time_In,right(CONVERT(nvarchar(100),time_Out ,100),7) Time_Out,Att,t.Finger_id FROM  HT_HostelRegistration R,Registration T,Degree G,Course C,Department D,Bio_Attendance B,room_detail rm,HM_HostelMaster hm where r.APP_No=t.APP_No and T.degree_code = G.degree_code and r.HostelMasterFK=hm.HostelMasterPK AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and t.app_no='" + rollno + "'   AND T.Roll_No = B.Roll_No AND B.Is_Staff=0 AND rm.Roompk=r.RoomFK and  B.Latestrec ='1' and r.CollegeCode in('" + colegecode + "') and isnull(R.IsVacated ,0)=0 and isnull(r.IsSuspend,0)=0 and isnull(r.IsDiscontinued ,0)=0 " + strdate + " " + Str + " ";
                                string hostelnames = string.Empty;
                                if (txthostelname.Text.ToString() != "--Select--")
                                {
                                    if (cbl_hostelname.Items.Count > 0)
                                        hostelnames = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                                    sql = sql + " And R.HostelMasterFK in('" + hostelnames + "')";

                                }
                                //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " And R.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                                //}
                                //else
                                //{
                                //}
                                string buildingnames = string.Empty;
                                if (txt_building.Text.ToString() != "--Select--")
                                {
                                    if (cbl_building.Items.Count > 0)
                                    {
                                        for (int i = 0; i < cbl_building.Items.Count; i++)
                                        {
                                            if (cbl_building.Items[i].Selected == true)
                                            {
                                                if (buildingnames == "")
                                                {
                                                    buildingnames = Convert.ToString(cbl_building.Items[i].Text);
                                                }
                                                else
                                                {
                                                    buildingnames = buildingnames + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                                                }
                                            }
                                        }
                                        sql = sql + " and  Building_Name in('" + buildingnames + "')";
                                    }

                                }

                                string flrnames = string.Empty;
                                if (txtfloorname.Text.ToString() != "--Select--")
                                {
                                    if (cbl_floorName.Items.Count > 0)
                                    {
                                        flrnames = rs.GetSelectedItemsValueAsString(cbl_floorName);
                                        sql = sql + " and R.FloorFK in('" + flrnames + "')";
                                    }

                                }

                                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                                //}
                                //else
                                //{
                                //}
                                string rooms = string.Empty;
                                if (txtroom_no.Text.ToString() != "--Select--")
                                {
                                    if (cbl_room_no.Items.Count > 0)
                                    {
                                        rooms = rs.GetSelectedItemsValueAsString(cbl_room_no);
                                        sql = sql + " AND R.RoomFK in('" + rooms + "')";
                                    }

                                }
                                //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " AND R.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";
                                //}
                                //else
                                //{
                                //}
                                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                                //{
                                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                                //}
                                string studrolls = string.Empty;
                                if (txtrollnum.Text.ToString() != "--Select--")
                                {
                                    if (cbl_rollnum.Items.Count > 0)
                                    {
                                        studrolls = rs.GetSelectedItemsValueAsString(cbl_rollnum);
                                        sql = sql + " AND T.App_No in('" + studrolls + "')";

                                    }

                                }
                                string courseidget = string.Empty;
                                string batchyrget = string.Empty;
                                string branchget = string.Empty;
                                if (txt_degree.Text.ToString() != "--Select--")
                                {
                                    if (cbl_degree.Items.Count > 0)
                                        courseidget = rs.GetSelectedItemsValueAsString(cbl_degree);
                                    sql = sql + " AND C.Course_id in('" + courseidget + "')";

                                }
                                if (txt_batchyr.Text.ToString() != "--Select--")
                                {
                                    if (cbl_batchyear.Items.Count > 0)
                                        batchyrget = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                                    sql = sql + " AND T.Batch_Year in('" + batchyrget + "')";


                                }
                                if (txtbranch.Text.ToString() != "--Select--")
                                {
                                    if (cbl_branch.Items.Count > 0)
                                        branchget = rs.GetSelectedItemsValueAsString(cbl_branch);
                                    sql = sql + " AND G.Degree_code in('" + branchget + "')";

                                }
                                //if (cboroll.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " AND T.App_No ='" + cboroll.Text + "'";
                                //}
                                //else
                                //{
                                //}

                                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                                //{
                                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                                //}
                                //else
                                //{
                                //}

                                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                                //{
                                //    sql = sql + " AND G.degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                                //}
                                //else
                                //{
                                //}
                                string studNames = string.Empty;
                                if (txtstudename.Text.ToString() != "--Select--")
                                {
                                    if (cbl_studeName.Items.Count > 0)
                                    {
                                        studNames = rs.GetSelectedItemsValueAsString(cbl_studeName);
                                        sql = sql + " AND t.App_No in('" + studNames + "')";

                                    }

                                }
                                //if (cbostudentname.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " and t.App_No ='" + cbostudentname.SelectedItem.Value + "'";
                                //}
                                //else
                                //{

                                //}
                                if (Chktimein.Checked == true && !chk_Time.Checked)
                                {
                                    //strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text +cbointo.Text + "'";
                                    strTime = "and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                                    sql = sql + " " + strTime + "";
                                }
                                if (Chktimeout.Checked == true && !chk_Time.Checked)
                                {
                                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between ' " + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + "'  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + cbo_sec2.Text + "'";
                                    sql = sql + " " + strTime + "";  //  strTime = " and (time_out between '" & Format(dtpStime, "hh:mm AM/PM") & "'  and '" & Format(dtpEtime, "hh:mm AM/PM") & "' 
                                }
                                if (Chktimein.Checked == true && chk_Time.Checked)
                                {
                                    string time = cbo_hrtin.Text.Trim();
                                    string time1 = cbo_hrinto.Text.Trim();
                                    if (time != "")
                                    {
                                        if (time.Length == 1)
                                            time = "0" + time;
                                        else
                                            time = time;
                                    }
                                    if (time1 != "")
                                    {
                                        if (time1.Length == 1)
                                            time1 = "0" + time1;
                                        else
                                            time1 = time1;

                                    }

                                    strbwTime = " and  LogTime between '" + time.Trim() + ":" + cbo_mintimein.Text + " " + cbo_in.Text + "' and '" + time1.Trim() + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";


                                }
                                con.Close();
                                con.Open();
                                SqlCommand cmd7 = new SqlCommand(sql, con);

                                SqlDataReader drcount14;
                                fpbiomatric.Width = 1000;

                                int datval = 0;
                                int rowcnt = 0;
                                int rowstr = 0;
                                string timein2 = getfunction(" select right(CONVERT(nvarchar(100),in_time ,100),7) as intime from hostel_inout_time where hostel_code='" + hostelcode2 + "'");
                                drcount14 = cmd7.ExecuteReader();
                            lbl2:
                                while (drcount14.Read())
                                {
                                    if (drcount14.HasRows == true)
                                    {
                                        if (ontimeflag == true)
                                        {
                                            if (Convert.ToDateTime(timein3) <= Convert.ToDateTime(timein2))
                                            {
                                                goto lbl1;
                                            }
                                            else
                                            {
                                                goto lbl2;
                                            }
                                        }
                                        else if (Generalflag == false)
                                        {
                                            goto lbl1;
                                        }
                                        else
                                        {
                                            goto lbl1;
                                        }
                                    lbl1:
                                        if (tempstaffcode == "")
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;

                                            countabsenteve = 0;
                                            countpresenteve2 = 0;
                                            countlateeve = 0;
                                            countpermissioneve = 0;

                                            // fpbiomatric.Sheets[0].RowCount += 1;
                                            tempstaffcode = drcount14["App_No"].ToString();
                                        }
                                        else if ((tempstaffcode != "") && (tempstaffcode != drcount14["App_No"].ToString()))
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            countabsenteve = 0;
                                            countpresenteve2 = 0;
                                            countlateeve = 0;
                                            countpermissioneve = 0;

                                            //fpbiomatric.Sheets[0].RowCount += 1;
                                            tempstaffcode = drcount14["App_No"].ToString();
                                        }



                                        string rollno1 = "";
                                        string Appno = "";
                                        string studname = "";
                                        string degree = "";
                                        string hostelname = "";
                                        string roomno = "";
                                        string fatnmae = "";
                                        string fatmobno = "";
                                        string monname = "";
                                        string monmobno = "";
                                        string studmobno = "";
                                        string intime = "";
                                        string outtime = "";
                                        string hostelcode;
                                        string fingerid = "";

                                        rollno1 = drcount14["Roll_No"].ToString();
                                        studname = drcount14["stud_name"].ToString();
                                        Appno = drcount14["app_no"].ToString();

                                        degree = drcount14["Degree"].ToString();
                                        hostelname = drcount14["hostelname"].ToString();
                                        roomno = drcount14["room_name"].ToString();
                                        //barath 30/3/2016
                                        //intime = drcount14["time_in"].ToString();
                                        //outtime = drcount14["time_out"].ToString();
                                        intime = drcount14["time_out"].ToString();
                                        outtime = drcount14["time_in"].ToString();
                                        hostelcode = drcount14["HostelMasterFK"].ToString();
                                        fingerid = drcount14["Finger_id"].ToString();
                                        //string ontime=get

                                        string parentdetails = "select isnull(a.parent_name,'') parent_name,isnull(a.parentF_Mobile,'') parentF_Mobile,isnull(a.mother,'') mother,isnull(a.parentM_Mobile,'') parentM_Mobile,isnull(a.Student_Mobile,'') Student_Mobile from applyn  a where a.app_no='" + Appno + "'";
                                        dsparentde.Clear();
                                        dsparentde = d2.select_method_wo_parameter(parentdetails, "Text");
                                        if (dsparentde.Tables.Count > 0 && dsparentde.Tables[0].Rows.Count > 0)
                                        {
                                            fatnmae = Convert.ToString(dsparentde.Tables[0].Rows[0]["parent_name"]).Trim();
                                            fatmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["parentF_Mobile"]).Trim();
                                            monname = Convert.ToString(dsparentde.Tables[0].Rows[0]["mother"]).Trim();
                                            monmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["parentM_Mobile"]).Trim();
                                            studmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["Student_Mobile"]).Trim();
                                        }
                                        string ontime = getfunction(" select right(CONVERT(nvarchar(100),in_time ,100),7) as intime from hostel_inout_time where hostel_code='" + hostelcode + "'");

                                        if (intime.Trim() != "" && outtime.Trim() != "")
                                        {

                                            if (intime != outtime)
                                            {
                                                rowstr = fpbiomatric.Sheets[0].RowCount++;
                                                foreach (string key in htcolumn.Keys)
                                                {
                                                    coltext1 = htcolumn[key].ToString();
                                                    insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                                                    if (ItemList.Contains(Convert.ToString(coltext1)))
                                                    {
                                                        if (coltext1.Trim() == "Roll No")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = rollno1;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].CellType = txtcell;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                        }
                                                        if (coltext1.Trim() == "Student Name")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studname;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                        }
                                                        if (coltext1.Trim() == "Department")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = degree;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                        }
                                                        if (coltext1.Trim() == "Hostel Name")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = hostelname;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                        }
                                                        if (coltext1.Trim() == "Room No")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = roomno;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                        }
                                                        if (coltext1.Trim() == "Father Name")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatnmae;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                        }
                                                        if (coltext1.Trim() == "Father MobileNo")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatmobno;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                        }
                                                        if (coltext1.Trim() == "Mother Name")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monname;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                        }
                                                        if (coltext1.Trim() == "Mother MobileNo")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monmobno;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                        }
                                                        if (coltext1.Trim() == "Student MobileNo")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studmobno;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                        }
                                                        if (coltext1.Trim() == "Finger ID")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fingerid;
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                        }

                                                    }
                                                }


                                                //Added By Saranyadevi 19.4.2018
                                                if (intime == outtime)
                                                {

                                                    if ("12:00AM" == intime && "12:00AM" == outtime)
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = "";
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                                    }
                                                    else
                                                    {

                                                        string time = intime.Contains("AM") ? "AM" : "PM";
                                                        if (time == "PM")
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = outtime.ToString();
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                                        }
                                                        else
                                                        {
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = "";
                                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = intime.ToString();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if ("12:00AM" == intime)
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";

                                                    }
                                                    else
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = intime.ToString();

                                                    }
                                                    if ("12:00AM" == outtime)
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = "";
                                                    }
                                                    else
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = outtime.ToString();

                                                    }
                                                }

                                                string mrng = "";
                                                string evng = "";
                                                string att;
                                                att = drcount14["att"].ToString();
                                                if (att != "")
                                                {
                                                    string[] tmpdate;
                                                    tmpdate = att.Split(new char[] { '-' });
                                                    if (tmpdate.Length == 2)
                                                    {
                                                        mrng = tmpdate[0].ToString();
                                                        evng = tmpdate[1].ToString();
                                                    }
                                                    if (tmpdate.Length == 1)
                                                    {
                                                        mrng = tmpdate[0].ToString();
                                                        evng = "";
                                                    }
                                                    int setcount = 0;
                                                    setcount = colcount;
                                                    if (mrng.ToString() == "1")
                                                    {
                                                        mrng = "P";
                                                        countpresent2++;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].Text = mrng.ToString();
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].BackColor = Color.Green;
                                                        counttotalmornpresent++;//= countpresent2;
                                                    }
                                                    if (evng.ToString() == "1")
                                                    {
                                                        evng = "P";
                                                        countpresenteve2++;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = evng.ToString();
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].BackColor = Color.Green;
                                                        counttotalevennpresent++;// countpresenteve2++;
                                                    }
                                                    totalperesent = countpresent2 + countpresenteve2;
                                                    totalperesent = totalperesent / 2;

                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].Text = Convert.ToDouble(totalperesent).ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].HorizontalAlign = HorizontalAlign.Center;
                                                    g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, col - 4).ToString());
                                                    c = g * 100;

                                                    d = day3;
                                                    if (c != 0)
                                                    {
                                                        percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].HorizontalAlign = HorizontalAlign.Center;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].Text = percentage.ToString();
                                                    }
                                                    if (mrng.ToString() == "2")
                                                    {
                                                        mrng = "A";
                                                        countabsent2++;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].Text = mrng.ToString();
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].BackColor = Color.Red;
                                                        counttotalabsentmorn++;
                                                    }
                                                    if (evng.ToString() == "2")
                                                    {
                                                        evng = "A";
                                                        countabsenteve++;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = evng.ToString();
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].BackColor = Color.Red;
                                                        counttotalabsenteven++;
                                                    }
                                                    totalabsent = countabsent2 + countabsenteve;
                                                    totalabsent = totalabsent / 2;

                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].Text = totalabsent.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].HorizontalAlign = HorizontalAlign.Center;
                                                    if (mrng.ToString() == "4")
                                                    {
                                                        mrng = "LA";
                                                        totallatecount++; totalmornlate++;
                                                        countlate2++;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = intime;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].BackColor = Color.DarkRed;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].Text = mrng.ToString();
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].BackColor = Color.DarkRed;
                                                    }
                                                    if (evng.ToString() == "4")
                                                    {
                                                        evng = "LA";
                                                        totallatecount++; totalevenlate++;
                                                        countlateeve++;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = intime;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].BackColor = Color.DarkRed;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = evng.ToString();
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].BackColor = Color.DarkRed;
                                                    }
                                                    totallate = countlate2 + countlateeve;
                                                    totallate = totallate / 2;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].Text = totallate.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].HorizontalAlign = HorizontalAlign.Center;
                                                    if (mrng.ToString() == "PER")
                                                    {
                                                        countpermission2++; totalpermorn++;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = intime;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].Text = mrng.ToString();
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                        totalcountevennpermission++;
                                                    }
                                                    if (evng.ToString() == "PER")
                                                    {
                                                        countpermissioneve++; totalpereven++;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = intime;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = evng.ToString();
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                        totalcountevennpermission++;
                                                    }
                                                    totalpermission = countpermission2 + countpermissioneve;
                                                    totalpermission = totalpermission / 2;

                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].Text = totalpermission.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                else
                                                {
                                                    int setcount1 = 0;
                                                    setcount1 = colcount;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount1 + 1].Text = mrng.ToString();
                                                }
                                                if (Chktimein.Checked && chk_Time.Checked)
                                                {

                                                    string betime = "select LogTime from Daily_Logs where Log_Date='" + datetagvalue + "' and FingerID='" + fingerid + "' " + strbwTime + "";
                                                    DataSet dstime = d2.select_method_wo_parameter(betime, "Text");
                                                    if (dstime.Tables.Count > 0 && dstime.Tables[0].Rows.Count > 0)
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = dstime.Tables[0].Rows[0]["LogTime"].ToString();
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = "p";
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].BackColor = Color.Green;
                                                    }
                                                    else
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = "";
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = "A";
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].BackColor = Color.Red;
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    rptprint.Visible = false;
                    counttotalabsentmorn = 0;
                    counttotalabsenteven = 0;
                    lblpresent1.Text = "0";
                    lblpresent2.Text = "0";
                    lblabsent1.Text = "0";
                    lblabsent2.Text = "0";
                    //totalcountevennpermission
                    return;
                }
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Present:" + Convert.ToString(counttotalmornpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Present:" + Convert.ToString(counttotalevennpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Absent:" + counttotalabsentmorn.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Absent:" + counttotalabsenteven.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;


                Double totalRows = 0;
                totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                if (totalRows >= 10)
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    fpbiomatric.Height = 350;
                    fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                }
                else if (totalRows == 0)
                {
                    fpbiomatric.Height = 300;
                }
                else
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    fpbiomatric.Height = 75 + (75 * Convert.ToInt32(totalRows));
                }
                fpbiomatric.Sheets[0].SetColumnWidth(0, 75);
                fpbiomatric.Height = 150 + (fpbiomatric.Sheets[0].RowCount * 30);

                Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);
                fpbiomatric.Visible = true;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lblnorec.Visible = false;

                lblpresent1.Text = ":" + counttotalmornpresent;
                lblpresent2.Text = ":" + counttotalevennpresent;
                lblabsent1.Text = ":" + counttotalabsentmorn;
                lblabsent2.Text = ":" + counttotalabsenteven;
                lblpermission.Text = ":" + totalpermorn;// totalcountevennpermission;
                lblpermission1.Text = ":" + totalpereven;
                lbllate.Text = ":" + totalmornlate;//totallatecount;
                lbllate1.Text = ":" + totalevenlate;
                con.Close();

                if (drname.HasRows == false)
                {
                    fpbiomatric.Visible = false;

                    lblnorec.Visible = true;
                    imgabsent.Visible = false;
                    lblheaderabsent1.Visible = false;
                    lblheaderabsent2.Visible = false;
                    lblabsent1.Visible = false;
                    lblabsent2.Visible = false;
                    imgpresent.Visible = false;
                    imglate.Visible = false;
                    lblmornlate.Visible = false;
                    lblevenlate.Visible = false;
                    lbllate.Visible = false;
                    lbllate1.Visible = false;
                    lblpresent1.Visible = false;
                    lblpresent2.Visible = false;
                    lbl_headermorn.Visible = false;
                    lbl_headereven.Visible = false;
                    lbllate.Visible = false;
                    imgper.Visible = false;
                    lblmornper.Visible = false;
                    lblevenper.Visible = false;
                    lblpermission.Visible = false;
                    lblpermission1.Visible = false;
                    lbllate.Visible = false;
                    // attfiltertype.Visible = false;
                    //imgontime.Visible = false;
                    //lblontime.Visible = false;

                }
                //drname.Close();
                con1.Close();
                #endregion
            }
            else if (rdoinonly.Checked == true)
            {
                #region In Only
                //attfiltertype.Visible = false;
                lbllatetext.Visible = false;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                imgabsent.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;
                imgpresent.Visible = true;
                imglate.Visible = true;
                lblmornlate.Visible = true;
                lblevenlate.Visible = true;
                imgper.Visible = true;
                lblmornper.Visible = true;
                lblevenper.Visible = true;
                lblpermission.Visible = true;
                lblpermission1.Visible = true;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lbllate.Visible = true;
                lbllate1.Visible = true;
                imglate.Visible = true;
                lblmornlate.Visible = true;
                lblevenlate.Visible = true;
                //imgontime.Visible = true;
                //lblontime.Visible = true;
                cblsearch.Items[0].Selected = true;
                cblsearch.Items[1].Selected = true;
                cblsearch.Items[2].Selected = true;

                string[] search = new string[50];
                if (cblsearch.Items[0].Selected == true)
                {
                    search[0] = "staffmaster.staff_code";
                }
                if (cblsearch.Items[1].Selected == true)
                {
                    search[1] = "staffmaster.staff_name";
                }
                if (cblsearch.Items[2].Selected == true)
                {
                    search[2] = "hrdept_master.dept_name";
                }
                if (cblsearch.Items[3].Selected == true)
                {
                    search[3] = "dept_acronym";
                }
                if (cblsearch.Items[4].Selected == true)
                {
                    search[4] = "desig_master.desig_name";
                }
                if (cblsearch.Items[5].Selected == true)
                {
                    search[5] = " desig_master.desig_acronym";
                }
                if (cblsearch.Items[6].Selected == true)
                {
                    search[6] = "CONVERT(VARCHAR(10),staffmaster.join_date,103)";
                }
                if (cblsearch.Items[7].Selected == true)
                {
                    search[7] = "in_out_time.category_name";
                }
                string wsearch = "";
                int count = 0;
                for (int i = 0; i < cblsearch.Items.Count; i++)
                {
                    if (cblsearch.Items[i].Selected == true)
                    {
                        count = count + 1;
                    }
                }
                #region Header

                fpbiomatric.Sheets[0].RowCount = 1;
                fpbiomatric.Sheets[0].ColumnCount = 0;
                //fpbiomatric.CommandBar.Visible = false;
                fpbiomatric.RowHeader.Visible = true;
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";

                fpbiomatric.Sheets[0].PageSize = 10;
                fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpbiomatric.Pager.Align = HorizontalAlign.Right;
                fpbiomatric.Pager.Font.Bold = true;
                fpbiomatric.Pager.Font.Name = "Book Antiqua";
                fpbiomatric.Pager.ForeColor = Color.DarkGreen;
                fpbiomatric.Pager.BackColor = Color.AliceBlue;
                fpbiomatric.Pager.PageCount = 5;
                fpbiomatric.Sheets[0].SheetCorner.RowCount = 2;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnCount = ItemList.Count + 1;
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();

                string coltext1 = "";
                int insdex = 0;
                int colcount1 = 0;
                foreach (string key in htcolumn.Keys)
                {
                    coltext1 = htcolumn[key].ToString();
                    if (ItemList.Contains(Convert.ToString(coltext1)))
                    {
                        insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                        //FpSpread1.Columns[insdex].Locked = true;
                        fpbiomatric.Columns[insdex + 1].Width = 50;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Text = Convert.ToString(coltext1);
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Bold = true;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Name = "Book Antiqua";
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Size = FontUnit.Medium;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].HorizontalAlign = HorizontalAlign.Center;
                        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, insdex, 2, 1);
                    }
                }


                fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 5;
                int col = fpbiomatric.Sheets[0].ColumnCount - 1;
                int a = fpbiomatric.Sheets[0].ColumnCount - 1;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 5].Text = "Att  %";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 5, 70);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 5, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 4].Text = "P";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 4, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 3].Text = "A";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 3, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 2].Text = "LA";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 2, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 1].Text = "PER";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 1, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 1, 2, 1);


                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = " and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];

                    lbldate.Visible = false;



                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount - 1 + 4;

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = Txtentryfrom.Text.ToString();
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = datefrom.ToString();
                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                    if (chk_Time.Checked && Chktimein.Checked == true)
                    {
                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;

                    }

                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = split12[0].ToString() + "/" + split12[1].ToString() + "/" + split12[2].ToString(); ;
                        fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                        if (chk_Time.Checked && Chktimein.Checked == true)
                        {
                            fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;

                        }

                    }
                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }


                #endregion

                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }
                sql = "SELECT  rm.room_name,t.app_no, h.hostelname, T.Roll_No,T.Stud_Name,Course_Name+'-'+ " + dept + "  as Degree, b.Access_Date , right(CONVERT(nvarchar(100),time_in ,100),7) as Time_In, right(CONVERT(nvarchar(100),time_Out ,100),7) as Time_Out,Att,t.Finger_id FROM HM_HostelMaster h, HT_HostelRegistration R,Registration T,Degree G,Course C,Department D,Bio_Attendance B,room_detail rm  Where r.APP_No = T.App_No And T.degree_code = G.degree_code and isnull(Time_Out,'')<>'' AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and h.HostelMasterPK =r.HostelMasterFK and rm.Roompk=r.RoomFK AND T.Roll_No  = B.Roll_No AND B.Is_Staff=0 AND B.Latestrec ='1' and r.CollegeCode in('" + colegecode + "') and isnull(R.IsVacated ,0)=0 and isnull(r.IsSuspend,0)=0 and isnull(r.IsDiscontinued ,0)=0 " + strdate + " " + Str + " ";

                string hostelnamess = string.Empty;
                if (txthostelname.Text.ToString() != "--Select--")
                {
                    if (cbl_hostelname.Items.Count > 0)
                        hostelnamess = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                    sql = sql + " And R.HostelMasterFK in('" + hostelnamess + "')";

                }

                string building_name = string.Empty;
                if (txt_building.Text.ToString() != "--Select--")
                {
                    if (cbl_building.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_building.Items.Count; i++)
                        {
                            if (cbl_building.Items[i].Selected == true)
                            {
                                if (building_name == "")
                                {
                                    building_name = Convert.ToString(cbl_building.Items[i].Text);
                                }
                                else
                                {
                                    building_name = building_name + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                                }
                            }
                        }
                        sql = sql + " and  Building_Name in('" + building_name + "')";
                    }

                }
                //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " And R.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                //}
                string rooms = string.Empty;
                if (txtroom_no.Text.ToString() != "--Select--")
                {
                    if (cbl_room_no.Items.Count > 0)
                    {
                        rooms = rs.GetSelectedItemsValueAsString(cbl_room_no);
                        sql = sql + " AND R.RoomFK  in('" + rooms + "')";
                    }

                }
                //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND R.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";

                //}
                string flr_Namess = string.Empty;
                if (txtfloorname.Text.ToString() != "--Select--")
                {
                    if (cbl_floorName.Items.Count > 0)
                    {
                        flr_Namess = rs.GetSelectedItemsValueAsString(cbl_floorName);
                        sql = sql + " and R.FloorFK in('" + flr_Namess + "')";
                    }

                }
                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}

                string studrolls = string.Empty;
                if (txtrollnum.Text.ToString() != "--Select--")
                {
                    if (cbl_rollnum.Items.Count > 0)
                    {
                        studrolls = rs.GetSelectedItemsValueAsString(cbl_rollnum);
                        sql = sql + " AND T.App_No in('" + studrolls + "')";

                    }

                }
                string courseidval = string.Empty;
                string batchyrval = string.Empty;
                string branchval = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseidval = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseidval + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyrval = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyrval + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branchval = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branchval + "')";

                }
                //if (cboroll.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND T.app_no ='" + cboroll.SelectedItem.Value + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}
                string stud_Name = string.Empty;
                if (txtstudename.Text.ToString() != "--Select--")
                {
                    if (cbl_studeName.Items.Count > 0)
                    {
                        stud_Name = rs.GetSelectedItemsValueAsString(cbl_studeName);
                        sql = sql + " AND t.App_No in('" + stud_Name + "')";

                    }

                }
                //if (cbostudentname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and t.App_no ='" + cbostudentname.SelectedItem.Value + "' ";
                //}

                if (Chktimein.Checked == true && !chk_Time.Checked)
                {

                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                    sql = sql + " " + strTime + "";
                }
                else if (Chktimeout.Checked == true && !chk_Time.Checked)
                {

                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";

                    sql = sql + " " + strTime + "";
                }
                //  sql = sql + order_by_var;
                if (ddlorder.SelectedItem.Text == "Select")
                {
                    sql = sql + order_by_var;
                }
                else if (ddlorder.SelectedItem.Text == "Hostel")
                {
                    sql = sql + " order by h.HostelName,rm.Room_Name";
                }
                else if (ddlorder.SelectedItem.Text == "Room")
                {
                    sql = sql + " order by rm.Room_Name";

                }
                string str2 = getfunction3(sql);
                con1.Open();
                SqlDataReader drname;
                SqlCommand cmd2 = new SqlCommand(sql, con1);
                drname = cmd2.ExecuteReader();

                while (drname.Read())
                {
                    if (drname.HasRows == true)
                    {
                        sql = "";
                        //Str = "";
                        countpresent2 = 0;
                        countabsent2 = 0;
                        countlate2 = 0;
                        countpermission2 = 0;
                        //02.05.16
                        countabsenteve = 0;
                        countpresenteve2 = 0;
                        countlateeve = 0;
                        countpermissioneve = 0;

                        string rollno;
                        rollno = drname["Roll_no"].ToString();
                        if (!hat.Contains(rollno))
                        {
                            hat.Add(rollno, rollno);
                            int columncount = 0;
                            if (chk_Time.Checked && Chktimein.Checked == true)
                                columncount = 6;
                            else
                                columncount = 4;
                            for (int colcount = col; colcount <= Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount) - 1; colcount = colcount + columncount)
                            {
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;


                                if (chk_Time.Checked && Chktimein.Checked == true)
                                    fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 6);
                                else
                                    fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 4);


                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Out";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Mor";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "In";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Eve";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 60);

                                if (chk_Time.Checked && Chktimein.Checked == true)
                                {
                                    fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Text = "Between Time";
                                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                                    fpbiomatric.Sheets[0].SetColumnWidth(colcount + 4, 60);

                                    fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Text = "Att";
                                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                    fpbiomatric.Sheets[0].SetColumnWidth(colcount + 5, 60);

                                }


                                string datetagvalue;
                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                                strdate = " and B.access_date='" + datetagvalue + "'";


                                sql = "SELECT distinct rm.room_name,t.app_no, h.hostelname, T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree, b.Access_Date , right(CONVERT(nvarchar(100),time_in ,100),7) as Time_In,right(CONVERT(nvarchar(100),time_Out ,100),7) Time_Out,Att,t.Finger_id FROM HM_HostelMaster h, HT_HostelRegistration R,Registration T,Degree G,Course C,Department D,Bio_Attendance B ,room_detail rm Where r.APP_No = T.App_No And T.degree_code = G.degree_code and isnull(Time_Out,'')<>'' and h.HostelMasterPK =r.HostelMasterFK   AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and t.roll_no='" + rollno + "'  AND T.Roll_No = B.Roll_No AND B.Is_Staff=0 and rm.Roompk=r.RoomFK AND B.Latestrec ='1' and r.CollegeCode in('" + colegecode + "') and isnull(R.IsVacated ,0)=0 and isnull(r.IsSuspend,0)=0 and isnull(r.IsDiscontinued ,0)=0 " + strdate + "" + Str + " ";

                                //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " And R.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                                //}
                                string hostelnamesss = string.Empty;
                                if (txthostelname.Text.ToString() != "--Select--")
                                {
                                    if (cbl_hostelname.Items.Count > 0)
                                        hostelnamesss = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                                    sql = sql + " And R.HostelMasterFK in('" + hostelnamesss + "')";

                                }
                                string building_names = string.Empty;
                                if (txt_building.Text.ToString() != "--Select--")
                                {
                                    if (cbl_building.Items.Count > 0)
                                    {
                                        for (int i = 0; i < cbl_building.Items.Count; i++)
                                        {
                                            if (cbl_building.Items[i].Selected == true)
                                            {
                                                if (building_names == "")
                                                {
                                                    building_names = Convert.ToString(cbl_building.Items[i].Text);
                                                }
                                                else
                                                {
                                                    building_names = building_names + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                                                }
                                            }
                                        }
                                        sql = sql + " and  Building_Name in('" + building_names + "')";
                                    }

                                }
                                string roomsno = string.Empty;
                                if (txtroom_no.Text.ToString() != "--Select--")
                                {
                                    if (cbl_room_no.Items.Count > 0)
                                    {
                                        roomsno = rs.GetSelectedItemsValueAsString(cbl_room_no);
                                        sql = sql + " AND R.RoomFK in('" + roomsno + "')";
                                    }

                                }
                                //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " AND R.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";
                                //}
                                string fflName = string.Empty;
                                if (txtfloorname.Text.ToString() != "--Select--")
                                {
                                    if (cbl_floorName.Items.Count > 0)
                                    {
                                        fflName = rs.GetSelectedItemsValueAsString(cbl_floorName);
                                        sql = sql + " and R.FloorFK in('" + fflName + "')";
                                    }

                                }
                                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                                //}
                                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                                //{
                                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                                //}
                                string stud_roll = string.Empty;
                                if (txtrollnum.Text.ToString() != "--Select--")
                                {
                                    if (cbl_rollnum.Items.Count > 0)
                                    {
                                        stud_roll = rs.GetSelectedItemsValueAsString(cbl_rollnum);
                                        sql = sql + " AND T.app_no in('" + stud_roll + "')";

                                    }

                                }
                                string courseides = string.Empty;
                                string batchyres = string.Empty;
                                string branches = string.Empty;
                                if (txt_degree.Text.ToString() != "--Select--")
                                {
                                    if (cbl_degree.Items.Count > 0)
                                        courseides = rs.GetSelectedItemsValueAsString(cbl_degree);
                                    sql = sql + " AND C.Course_id in('" + courseides + "')";

                                }
                                if (txt_batchyr.Text.ToString() != "--Select--")
                                {
                                    if (cbl_batchyear.Items.Count > 0)
                                        batchyres = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                                    sql = sql + " AND T.Batch_Year in('" + batchyres + "')";


                                }
                                if (txtbranch.Text.ToString() != "--Select--")
                                {
                                    if (cbl_branch.Items.Count > 0)
                                        branches = rs.GetSelectedItemsValueAsString(cbl_branch);
                                    sql = sql + " AND G.Degree_code in('" + branches + "')";

                                }
                                //if (cboroll.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " AND T.app_no ='" + cboroll.SelectedItem.Value + "'";
                                //}

                                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                                //{
                                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                                //}

                                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                                //{
                                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                                //}
                                if (Chktimein.Checked == true && !chk_Time.Checked)
                                {

                                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                                    sql = sql + " " + strTime + "";
                                }
                                string studName = string.Empty;
                                if (txtstudename.Text.ToString() != "--Select--")
                                {
                                    if (cbl_studeName.Items.Count > 0)
                                    {
                                        studName = rs.GetSelectedItemsValueAsString(cbl_studeName);
                                        sql = sql + " AND t.app_no in('" + studName + "')";

                                    }

                                }
                                //if (cbostudentname.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " and t.app_no='" + cbostudentname.SelectedItem.Value + "' ";
                                //}
                                else if (Chktimeout.Checked == true && !chk_Time.Checked)
                                {

                                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                                    sql = sql + " " + strTime + "";  //  strTime = " and (time_out between '" & Format(dtpStime, "hh:mm AM/PM") & "'  and '" & Format(dtpEtime, "hh:mm AM/PM") & "' 
                                }

                                if (Chktimein.Checked == true && chk_Time.Checked)
                                {

                                    string time = cbo_hrtin.Text.Trim();
                                    string time1 = cbo_hrinto.Text.Trim();
                                    if (time != "")
                                    {
                                        if (time.Length == 1)
                                            time = "0" + time;
                                        else
                                            time = time;
                                    }
                                    if (time1 != "")
                                    {
                                        if (time1.Length == 1)
                                            time1 = "0" + time1;
                                        else
                                            time1 = time1;

                                    }

                                    strbwTime = " and  LogTime between '" + time.Trim() + ":" + cbo_mintimein.Text + " " + cbo_in.Text + "' and '" + time1.Trim() + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";


                                }
                                con.Open();
                                SqlCommand cmd7 = new SqlCommand(sql, con);
                                SqlDataReader drcount14;
                                fpbiomatric.Width = 1000;


                                int datval = 0;
                                int rowcnt = 0;
                                int rowstr = 0;

                                drcount14 = cmd7.ExecuteReader();

                                while (drcount14.Read())
                                {
                                    if (drcount14.HasRows == true)
                                    {
                                        if (tempstaffcode == "")
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            //fpbiomatric.Sheets[0].RowCount += 1;
                                            tempstaffcode = drcount14["app_no"].ToString();
                                        }
                                        else if ((tempstaffcode != "") && (tempstaffcode != drcount14["app_no"].ToString()))
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            //fpbiomatric.Sheets[0].RowCount += 1;
                                            tempstaffcode = drcount14["app_no"].ToString();
                                        }
                                        //rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                                        string rollno1 = "";
                                        string studname = "";
                                        string degree = "";
                                        string hostelname = "";
                                        string roomno = "";
                                        string intime = "";
                                        string outtime = "";
                                        string Appno = "";
                                        string fatnmae = "";
                                        string fatmobno = "";
                                        string monname = "";
                                        string monmobno = "";
                                        string studmobno = "";
                                        string fingerid = "";

                                        rollno1 = drcount14["Roll_No"].ToString();
                                        studname = drcount14["stud_name"].ToString();
                                        Appno = drcount14["app_no"].ToString();

                                        degree = drcount14["Degree"].ToString();
                                        hostelname = drcount14["hostelname"].ToString();
                                        roomno = drcount14["room_name"].ToString();

                                        intime = drcount14["time_out"].ToString();
                                        outtime = drcount14["time_in"].ToString();
                                        fingerid = drcount14["Finger_id"].ToString();

                                        string parentdetails = "select isnull(a.parent_name,'') parent_name,isnull(a.parentF_Mobile,'') parentF_Mobile,isnull(a.mother,'') mother,isnull(a.parentM_Mobile,'') parentM_Mobile,isnull(a.Student_Mobile,'') Student_Mobile from applyn  a where a.app_no='" + Appno + "'";
                                        dsparentde.Clear();
                                        dsparentde = d2.select_method_wo_parameter(parentdetails, "Text");
                                        if (dsparentde.Tables.Count > 0 && dsparentde.Tables[0].Rows.Count > 0)
                                        {
                                            fatnmae = Convert.ToString(dsparentde.Tables[0].Rows[0]["parent_name"]).Trim();
                                            fatmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["parentF_Mobile"]).Trim();
                                            monname = Convert.ToString(dsparentde.Tables[0].Rows[0]["mother"]).Trim();
                                            monmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["parentM_Mobile"]).Trim();
                                            studmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["Student_Mobile"]).Trim();
                                        }

                                        if (intime != outtime)
                                        {
                                            rowstr = fpbiomatric.Sheets[0].RowCount++;
                                            foreach (string key in htcolumn.Keys)
                                            {
                                                coltext1 = htcolumn[key].ToString();
                                                insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                                                if (ItemList.Contains(Convert.ToString(coltext1)))
                                                {
                                                    if (coltext1.Trim() == "Roll No")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = rollno1;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].CellType = txtcell;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Student Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studname;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Department")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = degree;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Hostel Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = hostelname;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                    }
                                                    if (coltext1.Trim() == "Room No")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = roomno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Father Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatnmae;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Father MobileNo")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatmobno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Mother Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monname;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Mother MobileNo")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monmobno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Student MobileNo")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studmobno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                    }
                                                    if (coltext1.Trim() == "Finger ID")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fingerid;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                    }

                                                }
                                            }
                                            if ("12:00AM" == outtime)
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = "";

                                            }
                                            else
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = outtime;
                                            }
                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = outtime;
                                            string mrng = "";
                                            string evng = "";
                                            string att;
                                            att = drcount14["att"].ToString();
                                            if (att != "")
                                            {
                                                string[] tmpdate;
                                                tmpdate = att.Split(new char[] { '-' });
                                                if (tmpdate.Length == 2)
                                                {
                                                    mrng = tmpdate[0].ToString();
                                                    evng = tmpdate[1].ToString();
                                                }
                                                if (tmpdate.Length == 1)
                                                {
                                                    mrng = tmpdate[0].ToString();
                                                    evng = "";
                                                }

                                                int setcount = 0;
                                                setcount = colcount;
                                                //if (mrng.ToString() == "1")
                                                //{
                                                //    mrng = "P";
                                                //    // countpresent++;
                                                //    countpresent2++;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = mrng.ToString();
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].BackColor = Color.Green;
                                                //    counttotalmornpresent++;
                                                //}
                                                if (evng.ToString() == "1")
                                                {
                                                    evng = "P";
                                                    countpresenteve2++;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = evng.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].BackColor = Color.Green;
                                                    counttotalevennpresent++;// countpresenteve2++;
                                                }
                                                totalperesent = countpresenteve2;// countpresent2 +
                                                totalperesent = totalperesent / 2;
                                                //totalpresent = countpresent2;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4
    ].Text = Convert.ToDouble(totalperesent).ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].HorizontalAlign = HorizontalAlign.Center;
                                                g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, col - 4).ToString());
                                                c = g * 100;

                                                d = day3;
                                                if (c != 0)
                                                {
                                                    percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].Text = percentage.ToString();

                                                }
                                                //if (mrng.ToString() == "2")
                                                //{
                                                //    mrng = "A";
                                                //    countabsent2++;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = mrng.ToString();
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].BackColor = Color.Red;
                                                //    counttotalabsentmorn++;
                                                //}
                                                if (evng.ToString() == "2")
                                                {
                                                    evng = "A";
                                                    countabsenteve++;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = evng.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].BackColor = Color.Red;
                                                    counttotalabsenteven++;
                                                }
                                                totalabsent = countabsenteve;//countabsent2 +
                                                totalabsent = totalabsent / 2;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].Text = totalabsent.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].HorizontalAlign = HorizontalAlign.Center;


                                                //if (mrng.ToString() == "LA")
                                                //{
                                                //    totallatecount++; totalmornlate++;
                                                //    countlate2++;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].Text = mrng.ToString();
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].BackColor = Color.DarkRed;
                                                //}
                                                if (evng.ToString() == "4")
                                                {
                                                    evng = "LA";
                                                    totallatecount++; totalevenlate++;
                                                    countlateeve++;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = evng.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].BackColor = Color.DarkRed;
                                                }
                                                totallate = countlateeve;//countlate2 +
                                                totallate = totallate / 2;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].Text = totallate.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].HorizontalAlign = HorizontalAlign.Center;

                                                //if (mrng.ToString() == "PER")
                                                //{
                                                //    countpermission2++; totalpermorn++;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].Text = mrng.ToString();
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                                //    totalcountevennpermission++;
                                                //    //fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].BackColor = ;
                                                //}
                                                if (evng.ToString() == "PER")
                                                {
                                                    countpermissioneve++; totalpereven++;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = evng.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                    totalcountevennpermission++;
                                                }
                                                totalpermission = countpermissioneve;//countpermission2 + 
                                                totalpermission = totalpermission / 2;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].Text = totalpermission.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            else
                                            {
                                                int setcount1 = 0;
                                                setcount1 = colcount;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount1 + 3].Text = mrng.ToString();
                                            }

                                            if (Chktimein.Checked && chk_Time.Checked)
                                            {

                                                string betime = "select LogTime from Daily_Logs where Log_Date='" + datetagvalue + "' and FingerID='" + fingerid + "' " + strbwTime + "";
                                                DataSet dstime = d2.select_method_wo_parameter(betime, "Text");
                                                if (dstime.Tables.Count > 0 && dstime.Tables[0].Rows.Count > 0)
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = dstime.Tables[0].Rows[0]["LogTime"].ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = "p";
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].BackColor = Color.Green;
                                                }
                                                else
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = "";
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = "A";
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].BackColor = Color.Red;
                                                }

                                            }

                                        }
                                    }

                                    fpbiomatric.Visible = true;

                                    lblnorec.Visible = false;
                                }

                                drcount14.Close();
                                con.Close();
                            }

                        }
                    }
                }
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    rptprint.Visible = false;
                    return;
                }
                fpbiomatric.Sheets[0].RowCount++;

                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Present:" + Convert.ToString(counttotalmornpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Present:" + Convert.ToString(counttotalevennpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                fpbiomatric.Sheets[0].RowCount++;

                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Absent:" + counttotalabsentmorn.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Absent:" + counttotalabsenteven.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;


                Double totalRows = 0;
                totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                if (totalRows >= 10)
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                    fpbiomatric.Height = 350;
                    fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows == 0)
                {

                    fpbiomatric.Height = 300;
                }
                else
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                    fpbiomatric.Height = 75 + (75 * Convert.ToInt32(totalRows));
                }

                fpbiomatric.Height = 150 + (fpbiomatric.Sheets[0].RowCount * 30);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);


                fpbiomatric.Visible = true;

                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lblnorec.Visible = false;
                lblpresent1.Text = ":" + counttotalmornpresent;
                lblpresent2.Text = ":" + counttotalevennpresent;
                lblabsent1.Text = ":" + counttotalabsentmorn;
                lblabsent2.Text = ":" + counttotalabsenteven;
                lblpermission.Text = ":" + totalpermorn;//totalcountevennpermission;
                lblpermission1.Text = ":" + totalpereven;
                lbllate.Text = ":" + totalmornlate;//totallatecount;
                lbllate1.Text = ":" + totalevenlate;
                if (drname.HasRows == false)
                {
                    fpbiomatric.Visible = false;

                    lblnorec.Visible = true;
                    lblheaderabsent1.Visible = false;
                    lblheaderabsent2.Visible = false;
                    imgabsent.Visible = false;
                    lblabsent1.Visible = false;
                    lblabsent2.Visible = false;
                    imgpresent.Visible = false;
                    imglate.Visible = false;
                    lblmornlate.Visible = false;
                    lblevenlate.Visible = false;
                    lbllate.Visible = false;
                    lbllate1.Visible = false;
                    imgper.Visible = false;
                    lblmornper.Visible = false;
                    lblevenper.Visible = false;
                    lblpermission.Visible = false;
                    lblpermission1.Visible = false;
                    imglate.Visible = false;
                    imglate.Visible = false;
                    lblmornlate.Visible = false;
                    lblevenlate.Visible = false;
                    lblmornlate.Visible = false;
                    lblevenlate.Visible = false;

                    lblpresent1.Visible = false;
                    lblpresent2.Visible = false;
                    lbl_headermorn.Visible = false;
                    lbl_headereven.Visible = false;

                    lbllate.Visible = false;
                    //imgontime.Visible = false;
                    //lblontime.Visible = false;

                }
                drname.Close();
                con1.Close();
                #endregion
            }
            else if (rdooutonly.Checked == true)
            {
                #region Out Only
                // attfiltertype.Visible = false;
                imglate.Visible = true;
                lblmornlate.Visible = true;
                lblevenlate.Visible = true;
                lbllatetext.Visible = false;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                imgabsent.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;
                imgpresent.Visible = true;
                imgper.Visible = true;
                lblmornper.Visible = true;
                lblevenper.Visible = true;
                lblpermission.Visible = true;
                lblpermission1.Visible = true;
                imgper.Visible = true;
                lblmornper.Visible = true;
                lblevenper.Visible = true;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lbllate.Visible = true;
                lbllate1.Visible = true;
                //imgontime.Visible = true;
                //lblontime.Visible = true;

                // CheckBoxselect.Visible = true;
                cblsearch.Items[0].Selected = true;
                cblsearch.Items[1].Selected = true;
                cblsearch.Items[2].Selected = true;

                string[] search = new string[50];

                if (cblsearch.Items[0].Selected == true)
                {
                    search[0] = "staffmaster.staff_code";
                }
                if (cblsearch.Items[1].Selected == true)
                {
                    search[1] = "staffmaster.staff_name";
                }
                if (cblsearch.Items[2].Selected == true)
                {
                    search[2] = "hrdept_master.dept_name";
                }
                if (cblsearch.Items[3].Selected == true)
                {
                    search[3] = "dept_acronym";
                }
                if (cblsearch.Items[4].Selected == true)
                {
                    search[4] = "desig_master.desig_name";
                }
                if (cblsearch.Items[5].Selected == true)
                {
                    search[5] = " desig_master.desig_acronym";
                }
                if (cblsearch.Items[6].Selected == true)
                {
                    search[6] = "CONVERT(VARCHAR(10),staffmaster.join_date,103)";
                }
                if (cblsearch.Items[7].Selected == true)
                {
                    search[7] = "in_out_time.category_name";
                }

                string wsearch = "";
                int count = 0;
                for (int i = 0; i < cblsearch.Items.Count; i++)
                {
                    if (cblsearch.Items[i].Selected == true)
                    {
                        count = count + 1;

                    }
                }


                #region Header

                fpbiomatric.Sheets[0].RowCount = 1;
                fpbiomatric.Sheets[0].ColumnCount = 0;
                //fpbiomatric.CommandBar.Visible = false;
                fpbiomatric.RowHeader.Visible = true;
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";

                fpbiomatric.Sheets[0].PageSize = 10;
                fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpbiomatric.Pager.Align = HorizontalAlign.Right;
                fpbiomatric.Pager.Font.Bold = true;
                fpbiomatric.Pager.Font.Name = "Book Antiqua";
                fpbiomatric.Pager.ForeColor = Color.DarkGreen;
                fpbiomatric.Pager.BackColor = Color.AliceBlue;
                fpbiomatric.Pager.PageCount = 5;
                fpbiomatric.Sheets[0].SheetCorner.RowCount = 2;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnCount = ItemList.Count + 1;
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();

                string coltext1 = "";
                int insdex = 0;
                int colcount1 = 0;
                foreach (string key in htcolumn.Keys)
                {
                    coltext1 = htcolumn[key].ToString();
                    if (ItemList.Contains(Convert.ToString(coltext1)))
                    {
                        insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                        //FpSpread1.Columns[insdex].Locked = true;
                        fpbiomatric.Columns[insdex + 1].Width = 50;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Text = Convert.ToString(coltext1);
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Bold = true;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Name = "Book Antiqua";
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Size = FontUnit.Medium;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].HorizontalAlign = HorizontalAlign.Center;
                        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, insdex, 2, 1);
                    }
                }


                fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 5;
                int col = fpbiomatric.Sheets[0].ColumnCount - 1;
                int a = fpbiomatric.Sheets[0].ColumnCount - 1;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 5].Text = "Att  %";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 5, 70);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 5, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 4].Text = "P";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 4, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 3].Text = "A";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 3, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 2].Text = "LA";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 2, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 1].Text = "PER";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 1, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 1, 2, 1);


                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = " and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];

                    lbldate.Visible = false;



                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount - 1 + 4;

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = Txtentryfrom.Text.ToString();
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = datefrom.ToString();
                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                    if (chk_Time.Checked && Chktimein.Checked == true)
                    {
                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;

                    }
                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = split12[0].ToString() + "/" + split12[1].ToString() + "/" + split12[2].ToString(); ;
                        fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                        if (chk_Time.Checked && Chktimein.Checked == true)
                        {
                            fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;

                        }
                    }
                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }


                #endregion

                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }

                sql = "SELECT  rm.room_name, h.hostelname,t.app_no, T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree, b.Access_Date , right(CONVERT(nvarchar(100),time_in ,100),7) as Time_In, right(CONVERT(nvarchar(100),time_Out ,100),7) as Time_Out,Att,t.Finger_id FROM HM_HostelMaster h, HT_HostelRegistration R,Registration T,Degree G,Course C,Department D,Bio_Attendance B,room_detail rm Where r.APP_No  = T.App_No And T.degree_code = G.degree_code and isnull(Time_In,'')<>'' AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and h.HostelMasterPK=r.HostelMasterFK  and rm.Roompk=r.RoomFK  AND T.Roll_No = B.Roll_No AND B.Is_Staff=0 AND B.Latestrec ='1' and r.CollegeCode in('" + colegecode + "') and isnull(R.IsVacated ,0)=0 and isnull(r.IsSuspend,0)=0 and isnull(r.IsDiscontinued ,0)=0 " + strdate + " " + Str + " ";
                string hostelnames = string.Empty;
                if (txthostelname.Text.ToString() != "--Select--")
                {
                    if (cbl_hostelname.Items.Count > 0)
                        hostelnames = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                    sql = sql + " And R.HostelMasterFK in('" + hostelnames + "')";

                }

                string buildname = string.Empty;
                if (txt_building.Text.ToString() != "--Select--")
                {
                    if (cbl_building.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_building.Items.Count; i++)
                        {
                            if (cbl_building.Items[i].Selected == true)
                            {
                                if (buildname == "")
                                {
                                    buildname = Convert.ToString(cbl_building.Items[i].Text);
                                }
                                else
                                {
                                    buildname = buildname + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                                }
                            }
                        }
                        sql = sql + " and  Building_Name in('" + buildname + "')";
                    }

                }

                //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " And R.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                //}
                string flooor = string.Empty;
                if (txtfloorname.Text.ToString() != "--Select--")
                {
                    if (cbl_floorName.Items.Count > 0)
                    {
                        flooor = rs.GetSelectedItemsValueAsString(cbl_floorName);
                        sql = sql + " and R.FloorFK in('" + flooor + "')";
                    }

                }

                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                //}
                string rooms = string.Empty;
                if (txtroom_no.Text.ToString() != "--Select--")
                {
                    if (cbl_room_no.Items.Count > 0)
                    {
                        rooms = rs.GetSelectedItemsValueAsString(cbl_room_no);
                        sql = sql + " AND R.RoomFK in('" + rooms + "')";
                    }

                }
                //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND R.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";

                //}
                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}
                string studroll = string.Empty;
                if (txtrollnum.Text.ToString() != "--Select--")
                {
                    if (cbl_rollnum.Items.Count > 0)
                    {
                        studroll = rs.GetSelectedItemsValueAsString(cbl_rollnum);
                        sql = sql + " AND T.App_No in('" + studroll + "')";

                    }

                }
                string courseidgets = string.Empty;
                string batchyrgets = string.Empty;
                string branchgets = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseidgets = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseidgets + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyrgets = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyrgets + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branchgets = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branchgets + "')";

                }
                //if (cboroll.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND T.App_no ='" + cboroll.SelectedItem.Value + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}
                string studentName = string.Empty;
                if (txtstudename.Text.ToString() != "--Select--")
                {
                    if (cbl_studeName.Items.Count > 0)
                    {
                        studentName = rs.GetSelectedItemsValueAsString(cbl_studeName);
                        sql = sql + " AND t.App_no in('" + studentName + "')";

                    }

                }
                //if (cbostudentname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and t.App_no ='" + cbostudentname.SelectedItem.Value + "' ";
                //}

                if (Chktimein.Checked == true && !chk_Time.Checked)
                {

                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                    sql = sql + " " + strTime + "";
                }
                else if (Chktimeout.Checked == true && !chk_Time.Checked)
                {
                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between ' " + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + "'  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + cbo_sec2.Text + "'";

                    //strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";

                    sql = sql + " " + strTime + "";
                }
                //  sql = sql + order_by_var;
                if (ddlorder.SelectedItem.Text == "Select")
                {
                    sql = sql + order_by_var;
                }
                else if (ddlorder.SelectedItem.Text == "Hostel")
                {
                    sql = sql + " order by h.HostelName,rm.Room_Name";
                }
                else if (ddlorder.SelectedItem.Text == "Room")
                {
                    sql = sql + " order by rm.Room_Name";

                }
                string str2 = getfunction3(sql);
                con1.Open();
                SqlDataReader drname;
                SqlCommand cmd2 = new SqlCommand(sql, con1);
                drname = cmd2.ExecuteReader();

                while (drname.Read())
                {
                    if (drname.HasRows == true)
                    {
                        countpresent2 = 0;
                        countabsent2 = 0;
                        countlate2 = 0;
                        countpermission2 = 0;

                        countabsenteve = 0;
                        countpresenteve2 = 0;
                        countlateeve = 0;
                        countpermissioneve = 0;

                        sql = "";
                        // Str = "";
                        string rollno;
                        rollno = drname["app_no"].ToString();
                        if (!hat.Contains(rollno))
                        {
                            hat.Add(rollno, rollno);
                            int columncount = 0;
                            if (chk_Time.Checked && Chktimein.Checked == true)
                                columncount = 6;
                            else
                                columncount = 4;
                            for (int colcount = col; colcount <= Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount) - 1; colcount = colcount + columncount)
                            {

                                if (chk_Time.Checked && Chktimein.Checked == true)
                                    fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 6);
                                else
                                    fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 4);


                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Out";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Mor";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "IN";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Eve";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 60);
                                if (chk_Time.Checked && Chktimein.Checked == true)
                                {
                                    fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Text = "Between Time";
                                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                                    fpbiomatric.Sheets[0].SetColumnWidth(colcount + 4, 60);

                                    fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Text = "Att";
                                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                    fpbiomatric.Sheets[0].SetColumnWidth(colcount + 5, 60);

                                }
                                string datetagvalue;
                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                                strdate = " and B.access_date='" + datetagvalue + "'";




                                sql = "SELECT rm.room_name, h.hostelname,t.app_no, T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree, b.Access_Date , right(CONVERT(nvarchar(100),time_in ,100),7) as Time_In,right(CONVERT(nvarchar(100),time_Out ,100),7) Time_Out,Att,t.Finger_id FROM HM_HostelMaster h, HT_HostelRegistration R,Registration T,Degree G,Course C,Department D,Bio_Attendance B,room_detail rm Where r.APP_No = T.App_No And T.degree_code = G.degree_code and  h.HostelMasterPK =r.HostelMasterFK and isnull(Time_In,'')<>'' AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and t.App_no='" + rollno + "'  AND T.Roll_No = B.Roll_No AND B.Is_Staff=0   and rm.Roompk=r.RoomFK AND B.Latestrec ='1' and r.CollegeCode in('" + colegecode + "') and isnull(R.IsVacated ,0)=0 and isnull(r.IsSuspend,0)=0 and isnull(r.IsDiscontinued ,0)=0 " + strdate + " " + Str + " ";


                                string hostelnamess = string.Empty;
                                if (txthostelname.Text.ToString() != "--Select--")
                                {
                                    if (cbl_hostelname.Items.Count > 0)
                                        hostelnamess = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                                    sql = sql + " And R.HostelMasterFK in('" + hostelnamess + "')";

                                }

                                string buildnames = string.Empty;
                                if (txt_building.Text.ToString() != "--Select--")
                                {
                                    if (cbl_building.Items.Count > 0)
                                    {
                                        for (int i = 0; i < cbl_building.Items.Count; i++)
                                        {
                                            if (cbl_building.Items[i].Selected == true)
                                            {
                                                if (buildnames == "")
                                                {
                                                    buildnames = Convert.ToString(cbl_building.Items[i].Text);
                                                }
                                                else
                                                {
                                                    buildnames = buildnames + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                                                }
                                            }
                                        }
                                        sql = sql + " and  Building_Name in('" + buildnames + "')";
                                    }

                                }
                                //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " And R.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                                //}
                                string flrnames = string.Empty;
                                if (txtfloorname.Text.ToString() != "--Select--")
                                {
                                    if (cbl_floorName.Items.Count > 0)
                                    {
                                        flrnames = rs.GetSelectedItemsValueAsString(cbl_floorName);
                                        sql = sql + " and R.FloorFK in('" + flrnames + "')";
                                    }

                                }
                                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                                //}
                                string roomsnum = string.Empty;
                                if (txtroom_no.Text.ToString() != "--Select--")
                                {
                                    if (cbl_room_no.Items.Count > 0)
                                    {
                                        roomsnum = rs.GetSelectedItemsValueAsString(cbl_room_no);
                                        sql = sql + " AND R.RoomFK in('" + roomsnum + "')";
                                    }

                                }
                                //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " AND R.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";

                                //}
                                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                                //{
                                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                                //}
                                string studrollss = string.Empty;
                                if (txtrollnum.Text.ToString() != "--Select--")
                                {
                                    if (cbl_rollnum.Items.Count > 0)
                                    {
                                        studrollss = rs.GetSelectedItemsValueAsString(cbl_rollnum);
                                        sql = sql + " AND T.App_No in('" + studrollss + "')";

                                    }

                                }
                                string courseidvalue = string.Empty;
                                string batchyrvalue = string.Empty;
                                string branchvalue = string.Empty;
                                if (txt_degree.Text.ToString() != "--Select--")
                                {
                                    if (cbl_degree.Items.Count > 0)
                                        courseidvalue = rs.GetSelectedItemsValueAsString(cbl_degree);
                                    sql = sql + " AND C.Course_id in('" + courseidvalue + "')";

                                }
                                if (txt_batchyr.Text.ToString() != "--Select--")
                                {
                                    if (cbl_batchyear.Items.Count > 0)
                                        batchyrvalue = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                                    sql = sql + " AND T.Batch_Year in('" + batchyrvalue + "')";


                                }
                                if (txtbranch.Text.ToString() != "--Select--")
                                {
                                    if (cbl_branch.Items.Count > 0)
                                        branchvalue = rs.GetSelectedItemsValueAsString(cbl_branch);
                                    sql = sql + " AND G.Degree_code in('" + branchvalue + "')";

                                }
                                //if (cboroll.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " AND T.App_no ='" + cboroll.SelectedItem.Value + "'";
                                //}

                                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                                //{
                                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                                //}

                                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                                //{
                                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                                //}
                                string studNamess = string.Empty;
                                if (txtstudename.Text.ToString() != "--Select--")
                                {
                                    if (cbl_studeName.Items.Count > 0)
                                    {
                                        studNamess = rs.GetSelectedItemsValueAsString(cbl_studeName);
                                        sql = sql + " AND t.App_no in('" + studNamess + "')";

                                    }

                                }
                                //if (cbostudentname.SelectedItem.Value.ToString() != "All")
                                //{
                                //    sql = sql + " and t.App_no='" + cbostudentname.SelectedItem.Value + "' ";
                                //}
                                if (Chktimein.Checked == true && !chk_Time.Checked)
                                {
                                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                                    sql = sql + " " + strTime + "";
                                }
                                else if (Chktimeout.Checked == true && !chk_Time.Checked)
                                {

                                    //strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between ' " + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + "'  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + cbo_sec2.Text + "'";

                                    sql = sql + " " + strTime + "";  //  strTime = " and (time_out between '" & Format(dtpStime, "hh:mm AM/PM") & "'  and '" & Format(dtpEtime, "hh:mm AM/PM") & "' 

                                }
                                if (Chktimein.Checked == true && chk_Time.Checked)
                                {
                                    string time = cbo_hrtin.Text.Trim();
                                    string time1 = cbo_hrinto.Text.Trim();
                                    if (time != "")
                                    {
                                        if (time.Length == 1)
                                            time = "0" + time;
                                        else
                                            time = time;
                                    }
                                    if (time1 != "")
                                    {
                                        if (time1.Length == 1)
                                            time1 = "0" + time1;
                                        else
                                            time1 = time1;

                                    }

                                    strbwTime = " and  LogTime between '" + time.Trim() + ":" + cbo_mintimein.Text + " " + cbo_in.Text + "' and '" + time1.Trim() + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";


                                }
                                // sql = sql + order_by_var;
                                if (ddlorder.SelectedItem.Text == "Select")
                                {
                                    sql = sql + order_by_var;
                                }
                                else if (ddlorder.SelectedItem.Text == "Hostel")
                                {
                                    sql = sql + " order by h.HostelName,rm.Room_Name";
                                }
                                else if (ddlorder.SelectedItem.Text == "Room")
                                {
                                    sql = sql + " order by rm.Room_Name";

                                }
                                con.Close();
                                con.Open();
                                SqlCommand cmd7 = new SqlCommand(sql, con);


                                SqlDataReader drcount14;
                                fpbiomatric.Width = 1000;


                                int datval = 0;
                                int rowcnt = 0;
                                int rowstr = 0;

                                drcount14 = cmd7.ExecuteReader();

                                while (drcount14.Read())
                                {
                                    if (drcount14.HasRows == true)
                                    {
                                        if (tempstaffcode == "")
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            countabsenteve = 0;
                                            countpresenteve2 = 0;
                                            countlateeve = 0;
                                            countpermissioneve = 0;

                                            //fpbiomatric.Sheets[0].RowCount += 1;
                                            tempstaffcode = drcount14["app_no"].ToString();
                                        }
                                        else if ((tempstaffcode != "") && (tempstaffcode != drcount14["app_no"].ToString()))
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;

                                            countabsenteve = 0;
                                            countpresenteve2 = 0;
                                            countlateeve = 0;
                                            countpermissioneve = 0;

                                            //fpbiomatric.Sheets[0].RowCount += 1;
                                            tempstaffcode = drcount14["app_no"].ToString();
                                        }
                                        //rowstr = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                                        string rollno1 = "";
                                        string studname = "";
                                        string degree = "";
                                        string hostelname = "";
                                        string roomno = "";
                                        string intime = "";
                                        string outtime = "";
                                        string Appno = "";
                                        string fatnmae = "";
                                        string fatmobno = "";
                                        string monname = "";
                                        string monmobno = "";
                                        string studmobno = "";
                                        string fingerid = "";

                                        rollno1 = drcount14["Roll_No"].ToString();
                                        studname = drcount14["stud_name"].ToString();
                                        Appno = drcount14["app_no"].ToString();
                                        degree = drcount14["Degree"].ToString();
                                        hostelname = drcount14["hostelname"].ToString();
                                        roomno = drcount14["room_name"].ToString();
                                        //modified by barath 02/05/2016
                                        //intime = drcount14["time_in"].ToString();
                                        //outtime = drcount14["time_out"].ToString();
                                        intime = drcount14["time_out"].ToString();
                                        outtime = drcount14["time_in"].ToString();
                                        fingerid = drcount14["Finger_id"].ToString();

                                        string parentdetails = "select isnull(a.parent_name,'') parent_name,isnull(a.parentF_Mobile,'') parentF_Mobile,isnull(a.mother,'') mother,isnull(a.parentM_Mobile,'') parentM_Mobile,isnull(a.Student_Mobile,'') Student_Mobile from applyn  a where a.app_no='" + Appno + "'";
                                        dsparentde.Clear();
                                        dsparentde = d2.select_method_wo_parameter(parentdetails, "Text");
                                        if (dsparentde.Tables.Count > 0 && dsparentde.Tables[0].Rows.Count > 0)
                                        {
                                            fatnmae = Convert.ToString(dsparentde.Tables[0].Rows[0]["parent_name"]).Trim();
                                            fatmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["parentF_Mobile"]).Trim();
                                            monname = Convert.ToString(dsparentde.Tables[0].Rows[0]["mother"]).Trim();
                                            monmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["parentM_Mobile"]).Trim();
                                            studmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["Student_Mobile"]).Trim();
                                        }
                                        if (intime != outtime)
                                        {
                                            rowstr = fpbiomatric.Sheets[0].RowCount++;
                                            foreach (string key in htcolumn.Keys)
                                            {
                                                coltext1 = htcolumn[key].ToString();
                                                insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                                                if (ItemList.Contains(Convert.ToString(coltext1)))
                                                {
                                                    if (coltext1.Trim() == "Roll No")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = rollno1;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].CellType = txtcell;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Student Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studname;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Department")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = degree;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Hostel Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = hostelname;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                    }
                                                    if (coltext1.Trim() == "Room No")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = roomno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Father Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatnmae;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Father MobileNo")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatmobno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Mother Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monname;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Mother MobileNo")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monmobno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Student MobileNo")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studmobno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                    }
                                                    if (coltext1.Trim() == "Finger ID")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fingerid;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                    }

                                                }
                                            }
                                            if (intime != outtime)
                                            {
                                                if ("12:00AM" != intime)
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = intime;


                                                }
                                                else
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                                }

                                            }
                                            else
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                            }

                                            string mrng = "";
                                            string evng = "";
                                            string att;
                                            att = drcount14["att"].ToString();
                                            if (att != "")
                                            {
                                                string[] tmpdate;
                                                tmpdate = att.Split(new char[] { '-' });
                                                if (tmpdate.Length == 2)
                                                {
                                                    mrng = tmpdate[0].ToString();
                                                    evng = tmpdate[1].ToString();
                                                }
                                                if (tmpdate.Length == 1)
                                                {
                                                    mrng = tmpdate[0].ToString();
                                                    evng = "";
                                                }
                                                int setcount = 0;
                                                setcount = colcount;
                                                if (mrng.ToString() == "1")
                                                {
                                                    mrng = "P";
                                                    countpresent2++;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].Text = mrng.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].BackColor = Color.Green;
                                                    counttotalmornpresent++;
                                                }
                                                //if (evng.ToString() == "1")
                                                //{
                                                //    evng = "P";
                                                //    countpresenteve2++;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].Text = evng.ToString();
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].BackColor = Color.Green;
                                                //    counttotalevennpresent++;
                                                //}
                                                totalperesent = countpresent2;// +countpresenteve2;
                                                totalperesent = totalperesent / 2;

                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].Text = Convert.ToDouble(totalperesent).ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].HorizontalAlign = HorizontalAlign.Center;
                                                g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, col - 4).ToString());
                                                c = g * 100;

                                                d = day3;
                                                if (c != 0)
                                                {
                                                    percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].Text = percentage.ToString();

                                                }
                                                if (mrng.ToString() == "2")
                                                {
                                                    mrng = "A";
                                                    countabsent2++;

                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].Text = mrng.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].BackColor = Color.Red;
                                                    counttotalabsentmorn++;
                                                }
                                                //if (evng.ToString() == "2")
                                                //{
                                                //    evng = "A";
                                                //    countabsenteve++;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].Text = evng.ToString();
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 2].BackColor = Color.Red;
                                                //    counttotalabsenteven++;
                                                //}
                                                totalabsent = countabsenteve;//countabsent2 + 
                                                totalabsent = totalabsent / 2;

                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].Text = totalabsent.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].HorizontalAlign = HorizontalAlign.Center;

                                                if (mrng.ToString() == "4")
                                                {
                                                    mrng = "LA";
                                                    totallatecount++; totalmornlate++;
                                                    countlate2++;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].Text = mrng.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].BackColor = Color.DarkRed;
                                                }
                                                //if (evng.ToString() == "LA")
                                                //{
                                                //    totallatecount++; totalevenlate++;
                                                //    countlateeve++;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = evng.ToString();
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].BackColor = Color.DarkRed;
                                                //}
                                                totallate = countlate2;// +countlateeve;
                                                totallate = totallate / 2;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].Text = countlate2.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].HorizontalAlign = HorizontalAlign.Center;

                                                if (mrng.ToString() == "PER")
                                                {
                                                    countpermission2++; totalpermorn++;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].Text = mrng.ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                    totalcountevennpermission++;
                                                }
                                                //if (evng.ToString() == "PER")
                                                //{
                                                //    countpermissioneve++; totalpereven++;
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].Text = evng.ToString();
                                                //    fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                //    totalcountevennpermission++;
                                                //}
                                                totalpermission = countpermission2;//+ countpermissioneve;
                                                totalpermission = totalpermission / 2;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].Text = totalpermission.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].HorizontalAlign = HorizontalAlign.Center;

                                            }
                                            else
                                            {
                                                int setcount1 = 0;
                                                setcount1 = colcount;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, setcount1 + 1].Text = mrng.ToString();
                                            }

                                            if (Chktimein.Checked && chk_Time.Checked)
                                            {

                                                string betime = "select LogTime from Daily_Logs where Log_Date='" + datetagvalue + "' and FingerID='" + fingerid + "' " + strbwTime + "";
                                                DataSet dstime = d2.select_method_wo_parameter(betime, "Text");
                                                if (dstime.Tables.Count > 0 && dstime.Tables[0].Rows.Count > 0)
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = dstime.Tables[0].Rows[0]["LogTime"].ToString();
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = "p";
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].BackColor = Color.Green;
                                                }
                                                else
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = "";
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = "A";
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].BackColor = Color.Red;
                                                }

                                            }
                                        }

                                    }
                                    fpbiomatric.Visible = true;
                                    lblnorec.Visible = false;
                                    lblpresent1.Visible = true;
                                    lblpresent2.Visible = true;
                                    lbl_headermorn.Visible = true;
                                    lbl_headereven.Visible = true;
                                }
                                drcount14.Close();
                                con.Close();
                            }
                        }
                    }
                }

                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    rptprint.Visible = false;
                    counttotalabsentmorn = 0;
                    counttotalabsenteven = 0;
                    lblpresent1.Text = "0";
                    lblpresent2.Text = "0";
                    lblabsent1.Text = "0";
                    lblabsent2.Text = "0";
                    lbllate1.Text = "0";
                    lbllate.Text = "0";
                    return;
                }
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Present:" + Convert.ToString(counttotalmornpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Present:" + Convert.ToString(counttotalevennpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Absent:" + counttotalabsentmorn.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Absent:" + counttotalabsenteven.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                Double totalRows = 0;
                totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                if (totalRows >= 10)
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    fpbiomatric.Height = 350;
                    fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows == 0)
                {

                    fpbiomatric.Height = 300;
                }
                else
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                    fpbiomatric.Height = 75 + (75 * Convert.ToInt32(totalRows));
                }
                fpbiomatric.Height = 150 + (fpbiomatric.Sheets[0].RowCount * 30);

                Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);


                fpbiomatric.Visible = true;

                lblnorec.Visible = false;

                lblpresent1.Text = ":" + counttotalmornpresent;
                lblpresent2.Text = ":" + counttotalevennpresent;
                lblabsent1.Text = ":" + counttotalabsentmorn;
                lblabsent2.Text = ":" + counttotalabsenteven;
                lblpermission.Text = ":" + totalpermorn;//totalcountevennpermission;
                lblpermission1.Text = ":" + totalpereven;
                lbllate.Text = ":" + totalmornlate;//totallatecount;
                lbllate1.Text = ":" + totalevenlate;
                if (drname.HasRows == false)
                {
                    fpbiomatric.Visible = false;

                    lblnorec.Visible = true;
                    lblheaderabsent1.Visible = false;
                    lblheaderabsent2.Visible = false;
                    imgabsent.Visible = false;
                    lblabsent1.Visible = false;
                    lblabsent2.Visible = false;
                    imgpresent.Visible = false;
                    imglate.Visible = false;
                    lblmornlate.Visible = false;
                    lblevenlate.Visible = false;
                    lbllate.Visible = false;
                    imgper.Visible = false;
                    lblmornper.Visible = false;
                    lblevenper.Visible = false;
                    lblpresent1.Visible = false;
                    lblpresent2.Visible = false;
                    lbl_headermorn.Visible = false;
                    lbl_headereven.Visible = false;
                    lbllate.Visible = false;
                    lblpermission.Visible = false;
                    lblpermission1.Visible = false;
                    lbllate.Visible = false;
                    lbllate1.Visible = false;
                    //imgontime.Visible = false;
                    //lblontime.Visible = false;
                }
                drname.Close();
                con1.Close();
                #endregion
            }
            else if (rdounreg.Checked == true)
            {
                #region Un reg

                //attfiltertype.Visible = false;
                imgabsent.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;
                //ViewState["unreg"] = 1;
                lblmornlate.Visible = false;
                lblevenlate.Visible = false;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                imgpresent.Visible = false;
                imgper.Visible = false;
                lblmornper.Visible = false;
                lblevenper.Visible = false;
                lblpermission.Visible = false;
                lblpermission1.Visible = false;
                lblpresent1.Visible = false;
                lblpresent2.Visible = false;
                lbl_headermorn.Visible = false;
                lbl_headereven.Visible = false;
                lbllate.Visible = false;
                lbllate1.Visible = false;
                imglate.Visible = false;
                //lblmornlate.Visible = false;
                //lblevenlate.Visible = false;
                lbllatetext.Visible = false;
                //imgontime.Visible = false;
                //lblontime.Visible = false;
                cblsearch.Items[5].Selected = true;
                cblsearch.Items[6].Selected = true;
                cblsearch.Items[7].Selected = true;
                cblsearch.Items[5].Attributes.Add("style", "display:none;");
                cblsearch.Items[6].Attributes.Add("style", "display:none;");
                cblsearch.Items[7].Attributes.Add("style", "display:none;");

                //  CheckBoxselect.Visible = true;
                string[] search = new string[50];
                if (cblsearch.Items[0].Selected == true)
                {
                    search[0] = "staffmaster.staff_code";
                }
                if (cblsearch.Items[1].Selected == true)
                {
                    search[1] = "staffmaster.staff_name";
                }
                if (cblsearch.Items[2].Selected == true)
                {
                    search[2] = "hrdept_master.dept_name";
                }
                if (cblsearch.Items[3].Selected == true)
                {
                    search[3] = "dept_acronym";
                }
                if (cblsearch.Items[4].Selected == true)
                {
                    search[4] = "desig_master.desig_name";
                }
                if (cblsearch.Items[5].Selected == true)
                {
                    search[5] = " desig_master.desig_acronym";
                }
                if (cblsearch.Items[6].Selected == true)
                {
                    search[6] = "CONVERT(VARCHAR(10),staffmaster.join_date,103)";
                }
                if (cblsearch.Items[7].Selected == true)
                {
                    search[7] = "in_out_time.category_name";
                }

                string wsearch = "";
                int count = 0;
                for (int i = 0; i < cblsearch.Items.Count; i++)
                {
                    if (cblsearch.Items[i].Selected == true)
                    {
                        count = count + 1;
                    }
                }
                #region Header

                fpbiomatric.Sheets[0].RowCount = 1;
                fpbiomatric.Sheets[0].ColumnCount = 0;
                //fpbiomatric.CommandBar.Visible = false;
                fpbiomatric.RowHeader.Visible = true;
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";

                fpbiomatric.Sheets[0].PageSize = 10;
                fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpbiomatric.Pager.Align = HorizontalAlign.Right;
                fpbiomatric.Pager.Font.Bold = true;
                fpbiomatric.Pager.Font.Name = "Book Antiqua";
                fpbiomatric.Pager.ForeColor = Color.DarkGreen;
                fpbiomatric.Pager.BackColor = Color.AliceBlue;
                fpbiomatric.Pager.PageCount = 5;
                fpbiomatric.Sheets[0].SheetCorner.RowCount = 2;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnCount = ItemList.Count + 1;
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();

                string coltext1 = "";
                int insdex = 0;
                int colcount1 = 0;
                foreach (string key in htcolumn.Keys)
                {
                    coltext1 = htcolumn[key].ToString();
                    if (ItemList.Contains(Convert.ToString(coltext1)))
                    {
                        insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                        //FpSpread1.Columns[insdex].Locked = true;
                        fpbiomatric.Columns[insdex + 1].Width = 50;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Text = Convert.ToString(coltext1);
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Bold = true;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Name = "Book Antiqua";
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Size = FontUnit.Medium;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].HorizontalAlign = HorizontalAlign.Center;
                        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, insdex, 2, 1);
                    }
                }


                fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 5;
                int col = fpbiomatric.Sheets[0].ColumnCount - 1;
                int a = fpbiomatric.Sheets[0].ColumnCount - 1;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 5].Text = "Att  %";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 5, 70);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 5, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 4].Text = "P";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 4, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 3].Text = "A";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 3, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 2].Text = "LA";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 2, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 1].Text = "PER";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 1, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 1, 2, 1);


                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = " and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];

                    lbldate.Visible = false;



                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount - 1 + 2;

                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].Text = Txtentryfrom.Text.ToString();
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].Tag = datefrom.ToString();
                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;

                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].Tag = split12[0].ToString() + "/" + split12[1].ToString() + "/" + split12[2].ToString(); ;
                        fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;

                    }
                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }


                #endregion

                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }
                sql = "SELECT rm.room_name,t.app_no, h.hostelname, T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree,T.Finger_id FROM HM_HostelMaster h, HT_HostelRegistration R,Registration T,Degree G,Course C,Department D ,room_detail rm Where r.APP_No = T.App_No And T.degree_code = G.degree_code and isnull(R.IsVacated ,0)=0 and isnull(r.IsSuspend,0)=0 and isnull(r.IsDiscontinued ,0)=0 AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and h.HostelMasterPK=r.HostelMasterFK  and rm.Roompk=r.RoomFK and r.CollegeCode in('" + colegecode + "')";
                string hostelnames = string.Empty;
                if (txthostelname.Text.ToString() != "--Select--")
                {
                    if (cbl_hostelname.Items.Count > 0)
                        hostelnames = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                    sql = sql + " And R.HostelMasterFK in('" + hostelnames + "')";

                }

                string buildnamess = string.Empty;
                if (txt_building.Text.ToString() != "--Select--")
                {
                    if (cbl_building.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_building.Items.Count; i++)
                        {
                            if (cbl_building.Items[i].Selected == true)
                            {
                                if (buildnamess == "")
                                {
                                    buildnamess = Convert.ToString(cbl_building.Items[i].Text);
                                }
                                else
                                {
                                    buildnamess = buildnamess + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                                }
                            }
                        }
                        sql = sql + " and  Building_Name in('" + buildnamess + "')";
                    }

                }
                //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " And R.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                //}
                string rooms = string.Empty;
                if (txtroom_no.Text.ToString() != "--Select--")
                {
                    if (cbl_room_no.Items.Count > 0)
                    {
                        rooms = rs.GetSelectedItemsValueAsString(cbl_room_no);
                        sql = sql + " AND R.RoomFK in('" + rooms + "')";
                    }

                }
                //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND R.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";

                //}
                string floornames = string.Empty;
                if (txtfloorname.Text.ToString() != "--Select--")
                {
                    if (cbl_floorName.Items.Count > 0)
                    {
                        floornames = rs.GetSelectedItemsValueAsString(cbl_floorName);
                        sql = sql + " and R.FloorFK in('" + floornames + "')";
                    }

                }
                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}
                string courseidid = string.Empty;
                string batchyrid = string.Empty;
                string branchid = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseidid = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseidid + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyrid = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyrid + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branchid = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branchid + "')";

                }
                string studroll = string.Empty;
                if (txtrollnum.Text.ToString() != "--Select--")
                {
                    if (cbl_rollnum.Items.Count > 0)
                    {
                        studroll = rs.GetSelectedItemsValueAsString(cbl_rollnum);
                        sql = sql + " AND T.App_No in('" + studroll + "')";

                    }

                }

                //if (cboroll.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND T.App_No ='" + cboroll.SelectedItem.Value + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}
                string studName = string.Empty;
                if (txtstudename.Text.ToString() != "--Select--")
                {
                    if (cbl_studeName.Items.Count > 0)
                    {
                        studName = rs.GetSelectedItemsValueAsString(cbl_studeName);
                        sql = sql + " AND t.App_No in('" + studName + "')";

                    }

                }
                //if (cbostudentname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and t.App_No='" + cbostudentname.SelectedItem.Value + "' ";
                //}
                // sql = sql + order_by_var;
                if (ddlorder.SelectedItem.Text == "Select")
                {
                    sql = sql + order_by_var;
                }
                else if (ddlorder.SelectedItem.Text == "Hostel")
                {
                    sql = sql + " order by h.HostelName,rm.Room_Name";
                }
                else if (ddlorder.SelectedItem.Text == "Room")
                {
                    sql = sql + " order by rm.Room_Name";

                }
                con1.Open();
                DataSet ds2 = new DataSet();
                SqlDataAdapter da2 = new SqlDataAdapter(sql, con1);
                da2.Fill(ds2);
                int cont = ds2.Tables[0].Rows.Count;
                if (cont > 0)
                {
                    for (int h = 0; h < ds2.Tables[0].Rows.Count; h++)
                    {

                        countpresent2 = 0;
                        countabsent2 = 0;
                        countlate2 = 0;
                        countpermission2 = 0;
                        tempstaffcode = "";
                        countabsenteve = 0;
                        countpresenteve2 = 0;
                        countlateeve = 0;
                        countpermissioneve = 0;

                        string rollno;
                        rollno = Convert.ToString(ds2.Tables[0].Rows[h]["Roll_no"]);
                        string stud_name = ds2.Tables[0].Rows[h]["stud_name"].ToString();
                        string degree = ds2.Tables[0].Rows[h]["degree"].ToString();
                        string hostelname = ds2.Tables[0].Rows[h]["hostelname"].ToString();
                        string roomname = ds2.Tables[0].Rows[h]["room_name"].ToString();
                        string Appno = ds2.Tables[0].Rows[h]["app_no"].ToString();
                        string fingerid = ds2.Tables[0].Rows[h]["Finger_id"].ToString();

                        string fatnmae = "";
                        string fatmobno = "";
                        string monname = "";
                        string monmobno = "";
                        string studmobno = "";
                        string parentdetails = "select isnull(a.parent_name,'') parent_name,isnull(a.parentF_Mobile,'') parentF_Mobile,isnull(a.mother,'') mother,isnull(a.parentM_Mobile,'') parentM_Mobile,isnull(a.Student_Mobile,'') Student_Mobile from applyn  a where a.app_no='" + Appno + "'";
                        dsparentde.Clear();
                        dsparentde = d2.select_method_wo_parameter(parentdetails, "Text");
                        if (dsparentde.Tables.Count > 0 && dsparentde.Tables[0].Rows.Count > 0)
                        {
                            fatnmae = Convert.ToString(dsparentde.Tables[0].Rows[0]["parent_name"]).Trim();
                            fatmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["parentF_Mobile"]).Trim();
                            monname = Convert.ToString(dsparentde.Tables[0].Rows[0]["mother"]).Trim();
                            monmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["parentM_Mobile"]).Trim();
                            studmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["Student_Mobile"]).Trim();
                        }
                        if (!hat.Contains(rollno))
                        {
                            hat.Add(rollno, rollno);
                            for (int colcount = col; colcount <= Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount) - 1; colcount = colcount + 2)
                            {
                                //fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";
                                //fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";
                                //fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                                //fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;

                                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 2);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Mor";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Eve";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);

                                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 1);
                                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Att";
                                //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                string datetagvalue;
                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();
                                strdate = " and  access_date='" + datetagvalue + "'";
                                string[] monyeararr = datetagvalue.Split('/');
                                int year = Convert.ToInt16(monyeararr[2]);
                                int month = Convert.ToInt32(monyeararr[0]);
                                int monyear = year * 12 + Convert.ToInt16(monyeararr[0]);
                                int day10 = Convert.ToInt16(monyeararr[1]);
                                fpbiomatric.Width = 1000;
                                string sql2 = " Select * From bio_attendance Where Roll_No ='" + rollno + "' and Is_Staff = 0 and isnull(Time_in,'')<>'' and isnull(Time_Out,'')<>'' " + strdate + " ";//" + Str + "
                                SqlCommand cmd90 = new SqlCommand(sql2, con);
                                con.Close();
                                con.Open();
                                SqlDataReader dr52;
                                dr52 = cmd90.ExecuteReader();

                                if (dr52.HasRows == false)
                                {
                                    int datval = 0;
                                    int rowcnt = 0;
                                    int rowstr = 0;

                                    if (tempstaffcode == "")
                                    {
                                        //fpbiomatric.Sheets[0].RowCount++;//10.05.16
                                        tempstaffcode = ds2.Tables[0].Rows[h]["App_no"].ToString();
                                    }
                                    else if ((tempstaffcode != "") && (tempstaffcode != rollno))
                                    {
                                        //fpbiomatric.Sheets[0].RowCount++;
                                        tempstaffcode = ds2.Tables[0].Rows[h]["App_no"].ToString();
                                    }
                                    //if (Convert.ToString(ViewState["unreg"]).Trim() != "1")
                                    //{

                                    //    rowstr = fpbiomatric.Sheets[0].RowCount++;

                                    //    foreach (string key in htcolumn.Keys)
                                    //    {
                                    //        coltext1 = htcolumn[key].ToString();
                                    //        insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                                    //        if (ItemList.Contains(Convert.ToString(coltext1)))
                                    //        {
                                    //            if (coltext1.Trim() == "Roll No")
                                    //            {
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = Convert.ToString(ds2.Tables[0].Rows[h]["roll_no"]);
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].CellType = txtcell;
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                    //            }
                                    //            if (coltext1.Trim() == "Student Name")
                                    //            {
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = stud_name;
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                    //            }
                                    //            if (coltext1.Trim() == "Department")
                                    //            {
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = degree;
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                    //            }
                                    //            if (coltext1.Trim() == "Hostel Name")
                                    //            {
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = hostelname;
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                    //            }
                                    //            if (coltext1.Trim() == "Room No")
                                    //            {
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = roomname;
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                    //            }
                                    //            if (coltext1.Trim() == "Father Name")
                                    //            {
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatnmae;
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                    //            }
                                    //            if (coltext1.Trim() == "Father MobileNo")
                                    //            {
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatmobno;
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                    //            }
                                    //            if (coltext1.Trim() == "Mother Name")
                                    //            {
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monname;
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                    //            }
                                    //            if (coltext1.Trim() == "Mother MobileNo")
                                    //            {
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monmobno;
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                    //            }
                                    //            if (coltext1.Trim() == "Student MobileNo")
                                    //            {
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studmobno;
                                    //                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                    //            }

                                    //        }
                                    //    }

                                    //}
                                    //if (Convert.ToString(ViewState["unreg"]) == "1")
                                    //{
                                    //    hostelattend = " and D" + day10 + "=" + "'2'" + " and D" + day10 + "E=" + "'2'";
                                    //}
                                    //else { hostelattend = ""; }


                                    string sqlll2 = " select * from HT_Attendance where AttnMonth='" + month + "' and AttnYear='" + year + "' and App_No='" + tempstaffcode + "' and D" + day10 + "=" + "'2'" + " and D" + day10 + "E=" + "'2'";
                                    SqlDataAdapter da34 = new SqlDataAdapter(sqlll2, con1);
                                    DataSet ds34 = new DataSet();
                                    da34.Fill(ds34);

                                    int catt = ds34.Tables[0].Rows.Count;
                                    if (catt > 0)
                                    {
                                        if (tempstaffcode == "")
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            countabsenteve = 0;
                                            countpresenteve2 = 0;
                                            countlateeve = 0;
                                            countpermissioneve = 0;

                                            // tempstaffcode = ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                        }
                                        else if ((tempstaffcode != "") && (tempstaffcode != ds34.Tables[0].Rows[0]["App_no"].ToString()))
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            countabsenteve = 0;
                                            countpresenteve2 = 0;
                                            countlateeve = 0;
                                            countpermissioneve = 0;
                                            //fpbiomatric.Sheets[0].RowCount += 1;
                                            // tempstaffcode =  ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                        }
                                        //fpbiomatric.Visible = true;
                                        //if (Convert.ToString(ViewState["unreg"]) == "1")
                                        //{

                                        rowstr = fpbiomatric.Sheets[0].RowCount++;

                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = Convert.ToString(ds2.Tables[0].Rows[h]["roll_no"]);
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = stud_name.ToString();

                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = degree.ToString();
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = hostelname.ToString();

                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].Text = roomname.ToString();

                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                                        foreach (string key in htcolumn.Keys)
                                        {
                                            coltext1 = htcolumn[key].ToString();
                                            insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                                            if (ItemList.Contains(Convert.ToString(coltext1)))
                                            {
                                                if (coltext1.Trim() == "Roll No")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = Convert.ToString(ds2.Tables[0].Rows[h]["roll_no"]);
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].CellType = txtcell;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Student Name")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = stud_name;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Department")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = degree;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Hostel Name")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = hostelname;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                }
                                                if (coltext1.Trim() == "Room No")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = roomname;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Father Name")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatnmae;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Father MobileNo")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatmobno;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Mother Name")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monname;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Mother MobileNo")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monmobno;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Student MobileNo")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studmobno;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                }
                                                if (coltext1.Trim() == "Finger ID")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fingerid;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                }

                                            }
                                        }



                                        string str1 = "";
                                        string attmark = ""; string attmarkeve = ""; string atteve = "";
                                        attmark = ds34.Tables[0].Rows[0]["d" + day10 + ""].ToString();
                                        attmarkeve = ds34.Tables[0].Rows[0]["d" + day10 + "E"].ToString();
                                        atteve = Attmark(attmarkeve);

                                        string[] splitatt = attmark.Split('-');
                                        attmark = splitatt[0];
                                        str1 = Attmark(attmark);
                                        //if (str1 == "P")
                                        //{
                                        //    countpresent2++;
                                        //    countpresent++;
                                        //}
                                        //totalpresent = countpresent2;

                                        if (str1.ToString() == "P")
                                        {
                                            countpresent2++;// countpresent++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].BackColor = Color.Green;
                                            counttotalmornpresent++;
                                        }
                                        if (atteve.ToString() == "P")
                                        {
                                            countpresenteve2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.Green;
                                            counttotalevennpresent++;// countpresenteve2++;
                                        }
                                        totalperesent = countpresent2 + countpresenteve2;
                                        totalperesent = totalperesent / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].Text = Convert.ToDouble(totalperesent).ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].HorizontalAlign = HorizontalAlign.Center;
                                        g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, col - 4).ToString());
                                        c = g * 100;
                                        d = day3;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = str1.ToString();

                                        if (c != 0)
                                        {
                                            percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].Text = percentage.ToString();

                                        }
                                        else
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].Text = "0";

                                        }
                                        if (str1.ToString() == "A")
                                        {
                                            countabsent2++; countabsent++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].BackColor = Color.Red;
                                            counttotalabsentmorn++;
                                        }
                                        if (atteve.ToString() == "A")
                                        {
                                            countabsenteve++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.Red;
                                            counttotalabsenteven++;
                                        }
                                        totalabsent = countabsent2 + countabsenteve;
                                        totalabsent = totalabsent / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].Text = totalabsent.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].HorizontalAlign = HorizontalAlign.Center;


                                        if (str1.ToString() == "OD")
                                        {
                                            totallatecount++;
                                            countlate2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].BackColor = Color.DarkRed;
                                        }
                                        if (atteve.ToString() == "OD")
                                        {
                                            totallatecount++;
                                            countlateeve++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.DarkRed;
                                        }
                                        totallate = countlate2 + countlateeve;
                                        totallate = totallate / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].Text = totallate.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].HorizontalAlign = HorizontalAlign.Center;

                                        if (str1.ToString() == "PER")
                                        {
                                            countpermission2++; totalmornlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                        if (atteve.ToString() == "PER")
                                        {
                                            countpermissioneve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                        totalpermission = countpermission2 + countpermissioneve;
                                        totalpermission = totalpermission / 2;



                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].Text = totalpermission.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].HorizontalAlign = HorizontalAlign.Center;


                                    }

                                }

                            }
                        }
                    }
                }
                lblpresent1.Text = ":" + counttotalmornpresent;
                lblpresent2.Text = ":" + counttotalevennpresent;
                lblabsent1.Text = ":" + counttotalabsentmorn;
                lblabsent2.Text = ":" + counttotalabsenteven;
                lblpermission.Text = ":" + totalpermorn;//totalcountevennpermission;
                lblpermission1.Text = ":" + totalpereven;
                lbllate.Text = ":" + totalmornlate;//totallatecount;
                lbllate1.Text = ":" + totalevenlate;
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    rptprint.Visible = false;
                    lblpresent1.Text = "0";
                    lblpresent2.Text = "0";
                    lblabsent1.Text = "0";
                    lblabsent2.Text = "0";
                    return;
                }

                fpbiomatric.Sheets[0].RowCount++;

                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Absent:" + counttotalabsentmorn.ToString();

                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Absent:" + counttotalabsenteven.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                Double totalRows = 0;
                totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                if (totalRows >= 10)
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                    fpbiomatric.Height = 350;
                    fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows == 0)
                {

                    fpbiomatric.Height = 300;
                }
                else
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                    fpbiomatric.Height = 75 + (75 * Convert.ToInt32(totalRows));
                }

                fpbiomatric.Height = 150 + (fpbiomatric.Sheets[0].RowCount * 10);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);

                fpbiomatric.Visible = true;
                lblnorec.Visible = false;
                ViewState["unreg"] = null;
                Str = "";
                //lblpresent1.Visible = true;
                //lblpresent2.Visible = true;
                //lbl_headermorn.Visible = true;
                //lbl_headereven.Visible = true;
                #endregion
            }
            else if (rdoboth.Checked == true)
            {
                #region Both
                // attfiltertype.Visible = true;
                lbllatetext.Visible = false;
                //lbllatetext.Text = "";
                imgabsent.Visible = true;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;
                imgpresent.Visible = true;
                imglate.Visible = true;
                lblmornlate.Visible = true;
                lblevenlate.Visible = true;

                imgper.Visible = true;
                lblmornper.Visible = true;
                lblevenper.Visible = true;
                lblpermission.Visible = true;
                lblpermission1.Visible = true;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lbllate.Visible = true;
                lbllate1.Visible = true;
                imglate.Visible = true;
                lblmornlate.Visible = true;
                lblevenlate.Visible = true;
                //imgontime.Visible = false;
                //lblontime.Visible = false;

                //  CheckBoxselect.Visible = true;
                string[] search = new string[50];
                cblsearch.Items[5].Selected = true;
                cblsearch.Items[6].Selected = true;
                cblsearch.Items[7].Selected = true;
                if (cblsearch.Items[0].Selected == true)
                {
                    search[0] = "staffmaster.staff_code";
                }
                if (cblsearch.Items[1].Selected == true)
                {
                    search[1] = "staffmaster.staff_name";
                }
                if (cblsearch.Items[2].Selected == true)
                {
                    search[2] = "hrdept_master.dept_name";
                }
                if (cblsearch.Items[3].Selected == true)
                {
                    search[3] = "dept_acronym";
                }
                if (cblsearch.Items[4].Selected == true)
                {
                    search[4] = "desig_master.desig_name";
                }
                if (cblsearch.Items[5].Selected == true)
                {
                    search[5] = " desig_master.desig_acronym";
                }
                if (cblsearch.Items[6].Selected == true)
                {
                    search[6] = "CONVERT(VARCHAR(10),staffmaster.join_date,103)";
                }
                if (cblsearch.Items[7].Selected == true)
                {
                    search[7] = "in_out_time.category_name";
                }

                string wsearch = "";
                int count = 0;
                for (int i = 0; i < cblsearch.Items.Count; i++)
                {
                    if (cblsearch.Items[i].Selected == true)
                    {
                        count = count + 1;

                    }
                }

                #region Header

                fpbiomatric.Sheets[0].RowCount = 1;
                fpbiomatric.Sheets[0].ColumnCount = 0;
                //fpbiomatric.CommandBar.Visible = false;
                fpbiomatric.RowHeader.Visible = true;
                fpbiomatric.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";

                fpbiomatric.Sheets[0].PageSize = 10;
                fpbiomatric.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpbiomatric.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpbiomatric.Pager.Align = HorizontalAlign.Right;
                fpbiomatric.Pager.Font.Bold = true;
                fpbiomatric.Pager.Font.Name = "Book Antiqua";
                fpbiomatric.Pager.ForeColor = Color.DarkGreen;
                fpbiomatric.Pager.BackColor = Color.AliceBlue;
                fpbiomatric.Pager.PageCount = 5;
                fpbiomatric.Sheets[0].SheetCorner.RowCount = 2;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                fpbiomatric.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnCount = ItemList.Count + 1;
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();

                string coltext1 = "";
                int insdex = 0;
                int colcount1 = 0;
                foreach (string key in htcolumn.Keys)
                {
                    coltext1 = htcolumn[key].ToString();
                    if (ItemList.Contains(Convert.ToString(coltext1)))
                    {
                        insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                        //FpSpread1.Columns[insdex].Locked = true;
                        fpbiomatric.Columns[insdex + 1].Width = 50;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Text = Convert.ToString(coltext1);
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Bold = true;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Name = "Book Antiqua";
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].Font.Size = FontUnit.Medium;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, insdex].HorizontalAlign = HorizontalAlign.Center;
                        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, insdex, 2, 1);
                    }
                }


                fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 5;
                int col = fpbiomatric.Sheets[0].ColumnCount - 1;
                int a = fpbiomatric.Sheets[0].ColumnCount - 1;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 5].Text = "Att  %";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 5, 70);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 5, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 4].Text = "P";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 4, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 3].Text = "A";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 3, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 2].Text = "LA";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 2, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, a - 1].Text = "PER";
                fpbiomatric.Sheets[0].SetColumnWidth(a - 1, 30);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, a - 1, 2, 1);

                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = " and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {
                    string[] differdays = new string[days];
                    lbldate.Visible = false;
                    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount - 1 + 4;
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = Txtentryfrom.Text.ToString();
                    fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = datefrom.ToString();
                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                    if (chk_Time.Checked && Chktimein.Checked == true)
                    {
                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;

                    }
                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next
                    {

                        differdays[date_loop - 1] = dt1.AddDays(date_loop).ToString();
                        string[] split11 = differdays[date_loop - 1].Split(new char[] { ' ' });
                        string[] split12 = split11[0].Split(new Char[] { '/' });
                        string datevar = "";
                        datevar = split12[1].ToString() + "/" + split12[0].ToString() + "/" + split12[2].ToString();

                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 4;

                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Text = datevar;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].Tag = split12[0].ToString() + "/" + split12[1].ToString() + "/" + split12[2].ToString(); ;
                        fpbiomatric.ActiveSheetView.ColumnHeader.Cells[0, fpbiomatric.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                        if (chk_Time.Checked && Chktimein.Checked == true)
                        {
                            fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;

                        }
                    }
                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }
                #endregion

                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }

                sql = "SELECT rm.room_name, h.hostelname,t.app_no, T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree,T.Finger_id FROM HM_HostelMaster h, HT_HostelRegistration R,Registration T,Degree G,Course C,Department D ,room_detail rm Where r.APP_No  = T.App_No And T.degree_code = G.degree_code and isnull(R.IsVacated,0)=0 and isnull(R.IsSuspend,0)=0 and isnull(R.IsDiscontinued,0)=0 AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and h.HostelMasterPK =r.HostelMasterFK and rm.Roompk=r.RoomFK and r.CollegeCode in('" + colegecode + "')  and ISNULL(r.IsDiscontinued,0)=0 and ISNULL(r.IsSuspend,0)=0 and ISNULL(r.IsVacated,0)=0 " + Str + "";
                string hostelnamey = string.Empty;
                if (txthostelname.Text.ToString() != "--Select--")
                {
                    if (cbl_hostelname.Items.Count > 0)
                        hostelnamey = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                    sql = sql + " And R.HostelMasterFK in('" + hostelnamey + "')";

                }

                string buildnam = string.Empty;
                if (txt_building.Text.ToString() != "--Select--")
                {
                    if (cbl_building.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_building.Items.Count; i++)
                        {
                            if (cbl_building.Items[i].Selected == true)
                            {
                                if (buildnam == "")
                                {
                                    buildnam = Convert.ToString(cbl_building.Items[i].Text);
                                }
                                else
                                {
                                    buildnam = buildnam + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                                }
                            }
                        }
                        sql = sql + " and  Building_Name in('" + buildnam + "')";
                    }

                }
                //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " And R.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                //}
                string roomnumb = string.Empty;
                if (txtroom_no.Text.ToString() != "--Select--")
                {
                    if (cbl_room_no.Items.Count > 0)
                    {
                        roomnumb = rs.GetSelectedItemsValueAsString(cbl_room_no);
                        sql = sql + " AND R.RoomFK in('" + roomnumb + "')";
                    }

                }
                //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND R.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";
                //}
                string floornames = string.Empty;
                if (txtfloorname.Text.ToString() != "--Select--")
                {
                    if (cbl_floorName.Items.Count > 0)
                    {
                        floornames = rs.GetSelectedItemsValueAsString(cbl_floorName);
                        sql = sql + " and R.FloorFK in('" + floornames + "')";
                    }

                }
                string courseidbio = string.Empty;
                string batchyrbio = string.Empty;
                string branchbio = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseidbio = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseidbio + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyrbio = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyrbio + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branchbio = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branchbio + "')";

                }
                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}
                string studroll = string.Empty;
                if (txtrollnum.Text.ToString() != "--Select--")
                {
                    if (cbl_rollnum.Items.Count > 0)
                    {
                        studroll = rs.GetSelectedItemsValueAsString(cbl_rollnum);
                        sql = sql + " AND T.App_No in('" + studroll + "')";

                    }

                }
                string courseid = string.Empty;
                string batchyr = string.Empty;
                string branch = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseid = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseid + "')";

                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyr = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyr + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branch = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branch + "')";

                }
                //if (cboroll.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND T.App_No ='" + cboroll.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND G.Degree_code ='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}
                string stuName = string.Empty;
                if (txtstudename.Text.ToString() != "--Select--")
                {
                    if (cbl_studeName.Items.Count > 0)
                    {
                        stuName = rs.GetSelectedItemsValueAsString(cbl_studeName);
                        sql = sql + " AND t.App_No in('" + stuName + "')";

                    }

                }

                //if (cbostudentname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and t.App_No='" + cbostudentname.SelectedItem.Value.ToString() + "' ";
                //}
                if (ddlorder.SelectedItem.Text == "Select")
                {
                    sql = sql + order_by_var;
                }
                else if (ddlorder.SelectedItem.Text == "Hostel")
                {
                    sql = sql + " order by h.HostelName,rm.Room_Name";
                }
                else if (ddlorder.SelectedItem.Text == "Room")
                {
                    sql = sql + " order by rm.Room_Name";

                }
                int reg = 0;
                int ureg = 0;
                con1.Open();
                DataSet ds2 = new DataSet();
                SqlDataAdapter da2 = new SqlDataAdapter(sql, con1);
                da2.Fill(ds2);
                int hpresent = 0;
                int habsent = 0;
                int totalpresent = 0;

                counttotalmornpresent = 0;
                counttotalevennpresent = 0;
                counttotalabsentmorn = 0;
                counttotalabsenteven = 0;
                totallatecount = 0;
                totalcountevennpermission = 0;

                int cont = ds2.Tables[0].Rows.Count;
                if (cont > 0)
                {
                    for (int h = 0; h < ds2.Tables[0].Rows.Count; h++)
                    {
                        // sql = "";
                        //Str = "";
                        countpresent2 = 0;
                        countabsent2 = 0;
                        countlate2 = 0;
                        countpermission2 = 0;
                        countabsenteve = 0;
                        countpresenteve2 = 0;
                        countlateeve = 0;
                        countpermissioneve = 0;
                        tempstaffcode = "";
                        string rollno; double totalp = 0; double totalA = 0; double totalLA = 0; double totalPER = 0;
                        rollno = ds2.Tables[0].Rows[h]["Roll_No"].ToString();
                        string appno = ds2.Tables[0].Rows[h]["app_no"].ToString();
                        string stud_name = ds2.Tables[0].Rows[h]["stud_name"].ToString();
                        string degree = ds2.Tables[0].Rows[h]["degree"].ToString();
                        string hostelname = ds2.Tables[0].Rows[h]["hostelname"].ToString();
                        string roomname = ds2.Tables[0].Rows[h]["room_name"].ToString();
                        string Appno = ds2.Tables[0].Rows[h]["app_no"].ToString();
                        string fingerid = ds2.Tables[0].Rows[h]["Finger_id"].ToString();
                        string fatnmae = "";
                        string fatmobno = "";
                        string monname = "";
                        string monmobno = "";
                        string studmobno = "";
                        string parentdetails = "select isnull(a.parent_name,'') parent_name,isnull(a.parentF_Mobile,'') parentF_Mobile,isnull(a.mother,'') mother,isnull(a.parentM_Mobile,'') parentM_Mobile,isnull(a.Student_Mobile,'') Student_Mobile from applyn  a where a.app_no='" + Appno + "'";
                        dsparentde.Clear();
                        dsparentde = d2.select_method_wo_parameter(parentdetails, "Text");
                        if (dsparentde.Tables.Count > 0 && dsparentde.Tables[0].Rows.Count > 0)
                        {
                            fatnmae = Convert.ToString(dsparentde.Tables[0].Rows[0]["parent_name"]).Trim();
                            fatmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["parentF_Mobile"]).Trim();
                            monname = Convert.ToString(dsparentde.Tables[0].Rows[0]["mother"]).Trim();
                            monmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["parentM_Mobile"]).Trim();
                            studmobno = Convert.ToString(dsparentde.Tables[0].Rows[0]["Student_Mobile"]).Trim();
                        }
                        int rowstr = 0;
                        //fpbiomatric.Sheets[0].RowCount++;
                        if (!hat.Contains(rollno))
                        {
                            hat.Add(rollno, rollno);
                            int columncount = 0;

                            if (chk_Time.Checked && Chktimein.Checked == true)
                                columncount = 6;
                            else
                                columncount = 4;
                            rowstr = fpbiomatric.Sheets[0].RowCount++;
                            for (int colcount = col; colcount <= Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount) - 1; colcount = colcount + columncount)
                            {
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount].Font.Size = FontUnit.Medium;

                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";


                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;

                                fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Name = "Book Antiqua";
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Name = "Book Antiqua";

                                fpbiomatric.ActiveSheetView.Columns[colcount + 2].Font.Size = FontUnit.Medium;
                                fpbiomatric.ActiveSheetView.Columns[colcount + 1].Font.Size = FontUnit.Medium;


                                if (chk_Time.Checked && Chktimein.Checked == true)
                                    fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 6);
                                else
                                    fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, 1, 4);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount].Text = "Out";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount, 60);
                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 1].Text = "Mor";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 1, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "IN";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 2, 60);

                                fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 3].Text = "Eve";
                                fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                fpbiomatric.Sheets[0].SetColumnWidth(colcount + 3, 60);

                                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(6, colcount, 1, 3);
                                //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 2].Text = "Att";
                                //fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                if (chk_Time.Checked && Chktimein.Checked == true)
                                {
                                    fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 4].Text = "Between Time";
                                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                                    fpbiomatric.Sheets[0].SetColumnWidth(colcount + 4, 60);

                                    fpbiomatric.Sheets[0].ColumnHeader.Cells[1, colcount + 5].Text = "Att";
                                    fpbiomatric.ActiveSheetView.ColumnHeader.Cells[1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                    fpbiomatric.Sheets[0].SetColumnWidth(colcount + 5, 60);

                                }

                                string datetagvalue;
                                datetagvalue = fpbiomatric.Sheets[0].ColumnHeader.Cells[0, colcount].Tag.ToString();

                                strdate = " and  access_date='" + datetagvalue + "'";

                                string[] monyeararr = datetagvalue.Split('/');
                                int year = Convert.ToInt16(monyeararr[2]);
                                int month = Convert.ToInt32(monyeararr[0]);
                                int monyear = year * 12 + Convert.ToInt16(monyeararr[0]);
                                int day10 = Convert.ToInt16(monyeararr[1]);
                                ureg++;
                                fpbiomatric.Width = 1000;
                                int datval = 0;
                                int rowcnt = 0;

                                if (tempstaffcode == "")
                                {
                                    //fpbiomatric.Sheets[0].RowCount++;
                                    tempstaffcode = ds2.Tables[0].Rows[h]["app_no"].ToString();
                                }
                                else if ((tempstaffcode != "") && (tempstaffcode != rollno))
                                {
                                    //fpbiomatric.Sheets[0].RowCount++;
                                    tempstaffcode = ds2.Tables[0].Rows[h]["app_no"].ToString();
                                }
                                if ((Convert.ToString(ViewState["Bothpresent"]) != "1") && (Convert.ToString(ViewState["BothAbsent"]) != "2") && (Convert.ToString(ViewState["Bothod"]) != "3") && (Convert.ToString(ViewState["Bothper"]) != "4"))
                                {

                                    //fpbiomatric.Sheets[0].RowCount++;

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = "";

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;

                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = rollno.ToString();
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = stud_name.ToString();

                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = degree.ToString();
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = hostelname.ToString();

                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].Text = roomname.ToString();

                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Left;


                                    foreach (string key in htcolumn.Keys)
                                    {
                                        coltext1 = htcolumn[key].ToString();
                                        insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                                        if (ItemList.Contains(Convert.ToString(coltext1)))
                                        {
                                            if (coltext1.Trim() == "Roll No")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = rollno;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].CellType = txtcell;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                            }
                                            if (coltext1.Trim() == "Student Name")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = stud_name;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                            if (coltext1.Trim() == "Department")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = degree;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                            if (coltext1.Trim() == "Hostel Name")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = hostelname;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                            }
                                            if (coltext1.Trim() == "Room No")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = roomname;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                            if (coltext1.Trim() == "Father Name")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatnmae;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                            if (coltext1.Trim() == "Father MobileNo")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatmobno;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                            if (coltext1.Trim() == "Mother Name")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monname;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                            if (coltext1.Trim() == "Mother MobileNo")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monmobno;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                            if (coltext1.Trim() == "Student MobileNo")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studmobno;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                            }
                                            if (coltext1.Trim() == "Finger ID")
                                            {
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fingerid;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                            }

                                        }
                                    }
                                }
                                if (Convert.ToString(ViewState["Bothpresent"]) == "1")
                                {
                                    if (rdb_morn.Checked == true)
                                    {
                                        hostelattend1 = " and D" + day10 + "=" + "'1'" + " ";
                                    }
                                    else if (rdb_even.Checked == true)
                                    {
                                        hostelattend1 = " and D" + day10 + "E=" + "'1'";
                                    }
                                    else
                                    {
                                        hostelattend1 = " and D" + day10 + "=" + "'1'" + " and D" + day10 + "E=" + "'1'";
                                    }
                                }
                                else if (Convert.ToString(ViewState["BothAbsent"]) == "2")
                                {
                                    if (rdb_morn.Checked == true)
                                    {
                                        hostelattend1 = " and D" + day10 + "=" + "'2'";
                                    }
                                    else if (rdb_even.Checked == true)
                                    {
                                        hostelattend1 = "and D" + day10 + "E=" + "'2'";
                                    }
                                    else
                                    {
                                        hostelattend1 = " and D" + day10 + "=" + "'2'" + " and D" + day10 + "E=" + "'2'";
                                    }
                                }
                                else if (Convert.ToString(ViewState["Bothod"]) == "3")
                                {
                                    if (rdb_morn.Checked == true)
                                    {
                                        hostelattend1 = " and D" + day10 + "=" + "'3'";
                                    }
                                    else if (rdb_even.Checked == true)
                                    {
                                        hostelattend1 = " and D" + day10 + "E=" + "'3'";
                                    }
                                    else
                                    {
                                        hostelattend1 = " and D" + day10 + "=" + "'3'" + " and D" + day10 + "E=" + "'3'";
                                    }
                                }
                                else if (Convert.ToString(ViewState["Bothper"]) == "4")
                                {
                                    if (rdb_morn.Checked == true)
                                    {
                                        hostelattend1 = " and D" + day10 + "=" + "'4'";
                                    }
                                    else if (rdb_even.Checked == true)
                                    {
                                        hostelattend1 = " and D" + day10 + "E=" + "'4'";
                                    }
                                    else
                                    {
                                        hostelattend1 = " and D" + day10 + "=" + "'4'" + " and D" + day10 + "E=" + "'4'";
                                    }
                                }
                                else
                                { hostelattend1 = ""; }

                                string sql5 = "select att,roll_no,right(CONVERT(nvarchar(100),time_in ,100),7) as time_in,right(CONVERT(nvarchar(100),time_out ,100),7) as time_out from bio_attendance where roll_no='" + rollno + "' " + strdate + " and is_staff=0  " + Str + " ";
                                if (Chktimein.Checked == true && !chk_Time.Checked)
                                {

                                    //strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                                    sql5 = sql5 + " " + strTime + "";
                                }
                                else if (Chktimeout.Checked == true && !chk_Time.Checked)
                                {
                                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between ' " + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + "'  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + cbo_sec2.Text + "'";
                                    //strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                                    sql5 = sql5 + " " + strTime + "";
                                }
                                sql5 = sql5 + " select*from HT_Attendance where AttnMonth='" + month + "' and AttnYear='" + year + "' and App_No='" + appno + "' " + hostelattend1 + "";
                                DataSet dsbio = new DataSet();
                                dsbio.Clear();
                                dsbio = d2.select_method_wo_parameter(sql5, "Text");
                                if (Chktimein.Checked == true && chk_Time.Checked)
                                {
                                    string time = cbo_hrtin.Text.Trim();
                                    string time1 = cbo_hrinto.Text.Trim();
                                    if (time != "")
                                    {
                                        if (time.Length == 1)
                                            time = "0" + time;
                                        else
                                            time = time;
                                    }
                                    if (time1 != "")
                                    {
                                        if (time1.Length == 1)
                                            time1 = "0" + time1;
                                        else
                                            time1 = time1;

                                    }

                                    strbwTime = " and  LogTime between '" + time.Trim() + ":" + cbo_mintimein.Text + " " + cbo_in.Text + "' and '" + time1.Trim() + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";


                                }
                                //SqlDataAdapter dabio = new SqlDataAdapter(sql5, mycon);
                                //mycon.Close();
                                //mycon.Open();
                                //dabio.Fill(dsbio);
                                //int cntbio = dsbio.Tables[0].Rows.Count;
                                if (dsbio.Tables[0].Rows.Count > 0)
                                {
                                    reg++;
                                    if (tempstaffcode == "")
                                    {
                                        countpresent2 = 0;
                                        countabsent2 = 0;
                                        countlate2 = 0;
                                        countpermission2 = 0;
                                        countabsenteve = 0;
                                        countpresenteve2 = 0;
                                        countlateeve = 0;
                                        countpermissioneve = 0;
                                        // tempstaffcode = ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                    }
                                    else if ((tempstaffcode != "") && (tempstaffcode != dsbio.Tables[0].Rows[0]["Roll_No"].ToString()))
                                    {
                                        countpresent2 = 0;
                                        countabsent2 = 0;
                                        countlate2 = 0;
                                        countpermission2 = 0;

                                        countabsenteve = 0;
                                        countpresenteve2 = 0;
                                        countlateeve = 0;
                                        countpermissioneve = 0;
                                        //fpbiomatric.Sheets[0].RowCount += 1;
                                        // tempstaffcode =  ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                    }
                                    if ((Convert.ToString(ViewState["Bothpresent"]) == "1") || (Convert.ToString(ViewState["BothAbsent"]) == "2") || (Convert.ToString(ViewState["Bothod"]) == "3") || (Convert.ToString(ViewState["Bothper"]) == "4"))
                                    {
                                        //fpbiomatric.Sheets[0].RowCount++;
                                        //rowstr = fpbiomatric.Sheets[0].RowCount++;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = rollno.ToString();
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = stud_name.ToString();

                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = degree.ToString();
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = hostelname.ToString();

                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].Text = roomname.ToString();

                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        //fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Left;


                                        foreach (string key in htcolumn.Keys)
                                        {
                                            coltext1 = htcolumn[key].ToString();
                                            insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                                            if (ItemList.Contains(Convert.ToString(coltext1)))
                                            {
                                                if (coltext1.Trim() == "Roll No")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = rollno;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].CellType = txtcell;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Student Name")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = stud_name;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Department")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = degree;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Hostel Name")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = hostelname;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                }
                                                if (coltext1.Trim() == "Room No")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = roomname;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Father Name")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatnmae;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Father MobileNo")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatmobno;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Mother Name")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monname;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Mother MobileNo")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monmobno;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                                if (coltext1.Trim() == "Student MobileNo")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studmobno;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                }
                                                if (coltext1.Trim() == "Finger ID")
                                                {
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fingerid;
                                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                }

                                            }
                                        }
                                    }


                                    string timein = dsbio.Tables[0].Rows[0]["time_out"].ToString();
                                    string timeout = dsbio.Tables[0].Rows[0]["time_in"].ToString();

                                    //Added By Saranyadevi 19.4.2018
                                    if (timein == timeout)
                                    {

                                        if ("12:00AM" == timein && "12:00AM" == timeout)
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = "";
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                        }
                                        else
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = timein.ToString();
                                            //string time = timein.Contains("AM") ? "AM" : "PM";
                                            //if (time == "PM")
                                            //{
                                            //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = timeout.ToString(); ;
                                            //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = "";
                                            //}
                                            //else
                                            //{
                                            //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";
                                            //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = timeout.ToString();
                                            //}
                                        }
                                    }
                                    else
                                    {

                                        if ("12:00AM" == timein)
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = "";

                                        }
                                        else
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = timein.ToString();

                                        }
                                        if ("12:00AM" == timeout)
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = "";
                                        }
                                        else
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = timeout.ToString();

                                        }
                                    }
                                    string att = dsbio.Tables[0].Rows[0]["att"].ToString();
                                    string mrng = ""; string evng = "";
                                    string[] tmpdate;
                                    tmpdate = att.Split(new char[] { '-' });
                                    if (tmpdate.Length == 2)
                                    {
                                        mrng = tmpdate[0].ToString();
                                        evng = tmpdate[1].ToString();
                                    }
                                    //if (tmpdate.Length == 1)
                                    //{
                                    //    mrng = tmpdate[0].ToString();
                                    //    evng = "";
                                    //}
                                    //05.05.16
                                    // string sql2 = " select*from HT_Attendance where AttnMonth='" + month + "' and AttnYear='" + year + "' and App_No='" + appno + "'";

                                    if (mrng.Trim() != "")
                                    {
                                        if (mrng == "1")
                                        {
                                            mrng = "P";
                                            countpresent2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.Green;
                                            counttotalmornpresent++;//= countpresent2;
                                        }
                                        if (mrng == "2")
                                        {
                                            mrng = "A";
                                            countabsent2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.Red;
                                            counttotalabsentmorn++;
                                        }
                                        if (mrng.ToString() == "4")
                                        {
                                            mrng = "LA";
                                            totallatecount++;
                                            countlate2++; totalmornlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.DarkRed;
                                        }
                                        if (mrng.ToString() == "PER")
                                        {
                                            countpermission2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = mrng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                    }
                                    else
                                    {
                                        if (dsbio.Tables[1].Rows.Count > 0)
                                        {
                                            string str1 = ""; string atteve = "";
                                            string attmark = ""; string attmarkeve = "";
                                            attmark = dsbio.Tables[1].Rows[0]["d" + day10 + ""].ToString();
                                            //attmarkeve = dsbio.Tables[1].Rows[0]["d" + day10 + "E"].ToString();
                                            //atteve = Attmark(attmarkeve);

                                            string[] splitatt = attmark.Split('-');
                                            attmark = splitatt[0];
                                            str1 = Attmark(attmark);
                                            if (str1 == "P")
                                            {
                                                countpresent2++; counttotalmornpresent++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = str1.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.Green;
                                            }
                                            if (str1 == "A")
                                            {
                                                countabsent2++; countabsent++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = str1.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.Red;
                                                counttotalabsentmorn++;
                                            }
                                            if (str1 == "LA")
                                            {
                                                countlate2++;
                                                countlate++; totalevenlate++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = str1.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.DarkRed;
                                            }
                                            if (str1 == "PER")
                                            {
                                                countpermission2++;
                                                countpermission++; totalpermorn++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = str1.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                    }
                                    if (evng.Trim() != "")
                                    {
                                        if (evng.ToString() == "1")
                                        {
                                            evng = "P";
                                            countpresenteve2++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Green;
                                            counttotalevennpresent++;// countpresenteve2++;
                                        }
                                        if (evng.ToString() == "2")
                                        {
                                            evng = "A";
                                            countabsenteve++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                            counttotalabsenteven++;
                                        }
                                        if (evng.ToString() == "LA")
                                        {

                                            totallatecount++;
                                            countlateeve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.DarkRed;
                                        }
                                        if (evng.ToString() == "PER")
                                        {
                                            countpermissioneve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                    }
                                    else
                                    {
                                        if (dsbio.Tables[1].Rows.Count > 0)
                                        {
                                            string str1 = ""; string atteve = "";
                                            string attmark = ""; string attmarkeve = "";
                                            //attmark = dsbio.Tables[1].Rows[0]["d" + day10 + ""].ToString();
                                            attmarkeve = dsbio.Tables[1].Rows[0]["d" + day10 + "E"].ToString();
                                            atteve = Attmark(attmarkeve);
                                            //string[] splitatt = attmark.Split('-');
                                            //attmark = splitatt[0];
                                            //str1 = Attmark(attmark);
                                            if (atteve == "P")
                                            {
                                                countpresenteve2++; counttotalevennpresent++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Green;
                                            }
                                            if (atteve.ToString() == "A")
                                            {
                                                countabsenteve++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                                counttotalabsenteven++;
                                            }
                                            if (atteve.ToString() == "OD")
                                            {
                                                //countabsenteve++;
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                                fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                                //counttotalabsenteven++;
                                            }

                                        }
                                    }
                                    #region present
                                    totalperesent = countpresent2 + countpresenteve2;
                                    totalperesent = totalperesent / 2;
                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].Text.Trim() != "")
                                    {
                                        totalp = totalp + totalperesent;
                                    }
                                    else
                                    {
                                        totalp = totalperesent;
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].Text = Convert.ToString(totalp);
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].HorizontalAlign = HorizontalAlign.Center;
                                    g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, col - 4).ToString());
                                    c = g * 100;
                                    d = day3;
                                    // fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = att.ToString();
                                    if (c != 0)
                                    {
                                        percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].Text = percentage.ToString();
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].Text = "0";
                                    }
                                    #endregion

                                    #region Absent
                                    totalabsent = countabsent2 + countabsenteve;
                                    totalabsent = totalabsent / 2;

                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].Text.Trim() != "")
                                    {
                                        totalA = totalA + totalabsent;
                                    }
                                    else
                                    {
                                        totalA = totalabsent;
                                    }
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].Text = Convert.ToString(totalA);
                                    //totalabsent.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].HorizontalAlign = HorizontalAlign.Center;
                                    #endregion

                                    #region Late
                                    totallate = countlate2 + countlateeve;
                                    totallate = totallate / 2;

                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].Text.Trim() != "")
                                    {
                                        totalLA = totalLA + totallate;
                                    }
                                    else
                                    {
                                        totalLA = totallate;
                                    }

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].Text = totalLA.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].HorizontalAlign = HorizontalAlign.Center;
                                    #endregion

                                    #region permission
                                    totalpermission = countpermission2 + countpermissioneve;
                                    totalpermission = totalpermission / 2;

                                    if (fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].Text.Trim() != "")
                                    {
                                        totalPER = totalPER + totalpermission;
                                    }
                                    else
                                    {
                                        totalPER = totalpermission;
                                    }

                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].Text = totalPER.ToString();
                                    fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].HorizontalAlign = HorizontalAlign.Center;
                                    #endregion

                                    #region old


                                    //// 02.05.16
                                    ////string timein = dsbio.Tables[0].Rows[0]["time_in"].ToString();
                                    ////string timeout = dsbio.Tables[0].Rows[0]["time_out"].ToString();
                                    //string timein = dsbio.Tables[0].Rows[0]["time_out"].ToString();
                                    //string timeout = dsbio.Tables[0].Rows[0]["time_in"].ToString();

                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount].Text = timein.ToString();
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = timeout.ToString();

                                    //if (mrng.ToString() == "P")
                                    //{
                                    //    countpresent2++;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Green;
                                    //    counttotalmornpresent++;//= countpresent2;
                                    //}
                                    //if (evng.ToString() == "P")
                                    //{
                                    //    countpresenteve2++;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Green;
                                    //    counttotalevennpresent++;// countpresenteve2++;
                                    //}
                                    //totalperesent = countpresent2 + countpresenteve2;
                                    //totalperesent = totalperesent / 2;

                                    //if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text.Trim() != "")
                                    //{
                                    //    totalp = totalp + totalperesent;
                                    //}
                                    //else
                                    //{
                                    //    totalp = totalperesent;
                                    //}
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].Text = Convert.ToString(totalp);
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    //g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, 6).ToString());
                                    //c = g * 100;

                                    //d = day3;
                                    //// fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = att.ToString();

                                    //if (c != 0)
                                    //{
                                    //    percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, 5].Text = percentage.ToString();
                                    //}
                                    //else
                                    //{
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, 5].Text = "0";
                                    //}
                                    //if (mrng.ToString() == "A")
                                    //{
                                    //    countabsent2++;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.Red;
                                    //    counttotalabsentmorn++;
                                    //}
                                    //if (evng.ToString() == "A")
                                    //{
                                    //    countabsenteve++;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                    //    counttotalabsenteven++;
                                    //}
                                    //totalabsent = countabsent2 + countabsenteve;
                                    //totalabsent = totalabsent / 2;

                                    //if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text.Trim() != "")
                                    //{
                                    //    totalA = totalA + totalabsent;
                                    //}
                                    //else
                                    //{
                                    //    totalA = totalabsent;
                                    //}
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].Text = Convert.ToString(totalA);
                                    ////totalabsent.ToString();
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 7].HorizontalAlign = HorizontalAlign.Center;

                                    //if (mrng.ToString() == "LA")
                                    //{
                                    //    totallatecount++;
                                    //    countlate2++;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].BackColor = Color.DarkRed;
                                    //}
                                    //if (evng.ToString() == "LA")
                                    //{
                                    //    totallatecount++;
                                    //    countlateeve++;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.DarkRed;
                                    //}
                                    //totallate = countlate2 + countlateeve;
                                    //totallate = totallate / 2;

                                    //if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text.Trim() != "")
                                    //{
                                    //    totalLA = totalLA + totallate;
                                    //}
                                    //else
                                    //{
                                    //    totalLA = totallate;
                                    //}

                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].Text = totalLA.ToString();
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 8].HorizontalAlign = HorizontalAlign.Center;

                                    //if (mrng.ToString() == "PER")
                                    //{
                                    //    countpermission2++;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].Text = mrng.ToString();
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                    //    totalcountevennpermission++;
                                    //}
                                    //if (evng.ToString() == "PER")
                                    //{
                                    //    countpermissioneve++;
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = evng.ToString();
                                    //    fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                    //    totalcountevennpermission++;
                                    //}
                                    //totalpermission = countpermission2 + countpermissioneve;
                                    //totalpermission = totalpermission / 2;

                                    //if (fpbiomatric.Sheets[0].Cells[rowstr - 1, 9].Text.Trim() != "")
                                    //{
                                    //    totalPER = totalPER + totalpermission;
                                    //}
                                    //else
                                    //{
                                    //    totalPER = totalabsent;
                                    //}

                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 9].Text = totalPER.ToString();
                                    //fpbiomatric.Sheets[0].Cells[rowstr - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                    #endregion
                                }
                                else
                                {
                                    //string sql2 = "select * from hattendance where month_year='" + monyear + "' and roll_no='" + rollno + "'";
                                    string sql2 = " select*from HT_Attendance where AttnMonth='" + month + "' and AttnYear='" + year + "' and App_No='" + appno + "' " + hostelattend1 + " ";

                                    SqlDataAdapter da34 = new SqlDataAdapter(sql2, con1);
                                    DataSet ds34 = new DataSet();
                                    da34.Fill(ds34);

                                    int catt = ds34.Tables[0].Rows.Count;
                                    if (catt > 0)
                                    {
                                        if (tempstaffcode == "")
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            //countabsenteve = 0;
                                            //countpresenteve2 = 0;
                                            //countlateeve = 0;
                                            //countpermissioneve = 0;
                                            // tempstaffcode = ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                        }
                                        else if ((tempstaffcode != "") && (tempstaffcode != ds34.Tables[0].Rows[0]["App_No"].ToString()))
                                        {
                                            countpresent2 = 0;
                                            countabsent2 = 0;
                                            countlate2 = 0;
                                            countpermission2 = 0;
                                            //countabsenteve = 0;
                                            //countpresenteve2 = 0;
                                            //countlateeve = 0;
                                            //countpermissioneve = 0;
                                            //fpbiomatric.Sheets[0].RowCount += 1;
                                            //tempstaffcode =  ds34.Tables[0].Rows[0]["Roll_No"].ToString();
                                        }
                                        fpbiomatric.Visible = true;
                                        if ((Convert.ToString(ViewState["Bothpresent"]) == "1") || (Convert.ToString(ViewState["BothAbsent"]) == "2") || (Convert.ToString(ViewState["Bothod"]) == "3") || (Convert.ToString(ViewState["Bothper"]) == "4"))
                                        {

                                            rowstr = fpbiomatric.Sheets[0].RowCount++;
                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].Text = rollno.ToString();
                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].Text = stud_name.ToString();

                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].Text = degree.ToString();
                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].Text = hostelname.ToString();

                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].Text = roomname.ToString();

                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                            //fpbiomatric.Sheets[0].Cells[rowstr - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                                            foreach (string key in htcolumn.Keys)
                                            {
                                                coltext1 = htcolumn[key].ToString();
                                                insdex = ItemList.IndexOf(Convert.ToString(coltext1));
                                                if (ItemList.Contains(Convert.ToString(coltext1)))
                                                {
                                                    if (coltext1.Trim() == "Roll No")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = rollno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].CellType = txtcell;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Student Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = stud_name;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Department")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = degree;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Hostel Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = hostelname;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                    }
                                                    if (coltext1.Trim() == "Room No")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = roomname;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Father Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatnmae;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Father MobileNo")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fatmobno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Mother Name")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monname;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Mother MobileNo")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = monmobno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;
                                                    }
                                                    if (coltext1.Trim() == "Student MobileNo")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = studmobno;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                    }
                                                    if (coltext1.Trim() == "Finger ID")
                                                    {
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].Text = fingerid;
                                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, insdex].HorizontalAlign = HorizontalAlign.Left;

                                                    }

                                                }
                                            }
                                        }

                                        string str1 = ""; string atteve = "";
                                        string attmark = ""; string attmarkeve = "";
                                        attmark = ds34.Tables[0].Rows[0]["d" + day10 + ""].ToString();
                                        attmarkeve = ds34.Tables[0].Rows[0]["d" + day10 + "E"].ToString();
                                        atteve = Attmark(attmarkeve);

                                        string[] splitatt = attmark.Split('-');
                                        attmark = splitatt[0];
                                        str1 = Attmark(attmark);
                                        if (str1 == "P")
                                        {
                                            countpresent2++; counttotalmornpresent++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.Green;
                                        }
                                        if (atteve == "P")
                                        {
                                            countpresenteve2++; counttotalevennpresent++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Green;
                                        }
                                        totalperesent = countpresent2 + countpresenteve2;
                                        totalperesent = totalperesent / 2;

                                        //totalpresent = countpresent2;//22.04.16 barath
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].Text = Convert.ToDouble(totalperesent).ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].HorizontalAlign = HorizontalAlign.Center;
                                        g = Convert.ToDouble(fpbiomatric.Sheets[0].GetText(rowstr - 1, col - 4).ToString());
                                        c = g * 100;

                                        d = day3;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = str1.ToString();
                                        if (c != 0)
                                        {
                                            percentage = Math.Round((Convert.ToDouble(c) / Convert.ToDouble(d)), 2, MidpointRounding.AwayFromZero);
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 5].Text = percentage.ToString();
                                        }
                                        else
                                        {
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 4].Text = "0";

                                        }
                                        if (str1 == "")
                                        {
                                            str1 = "";
                                        }

                                        if (str1 == "A")
                                        {
                                            countabsent2++; countabsent++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.Red;
                                            counttotalabsentmorn++;
                                        }
                                        if (atteve.ToString() == "A")
                                        {
                                            countabsenteve++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.Red;
                                            counttotalabsenteven++;
                                        }
                                        totalabsent = countabsent2 + countabsenteve;
                                        totalabsent = totalabsent / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].Text = totalabsent.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 3].HorizontalAlign = HorizontalAlign.Center;

                                        if (str1 == "LA")
                                        {
                                            countlate2++;
                                            countlate++; totalmornlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].BackColor = Color.DarkRed;
                                        }
                                        if (atteve.ToString() == "LA")
                                        {
                                            totallatecount++;
                                            countlateeve++; totalevenlate++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].BackColor = Color.DarkRed;
                                        }
                                        totallate = countlate2 + countlateeve;
                                        totallate = totallate / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].Text = totallate.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 2].HorizontalAlign = HorizontalAlign.Center;

                                        if (str1 == "PER")
                                        {
                                            countpermission2++;
                                            countpermission++; totalpermorn++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].Text = str1.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        if (atteve.ToString() == "PER")
                                        {
                                            countpermissioneve++; totalpereven++;
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].Text = atteve.ToString();
                                            fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                            totalcountevennpermission++;
                                        }
                                        totalpermission = countpermission2 + countpermissioneve;
                                        totalpermission = totalpermission / 2;

                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].Text = totalpermission.ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, col - 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                                //Between Time Present Or Absent

                                if (Chktimein.Checked && chk_Time.Checked)
                                {

                                    string betime = "select LogTime from Daily_Logs where Log_Date='" + datetagvalue + "' and FingerID='" + fingerid + "' " + strbwTime + "";
                                    DataSet dstime = d2.select_method_wo_parameter(betime, "Text");
                                    if (dstime.Tables.Count > 0 && dstime.Tables[0].Rows.Count > 0)
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = dstime.Tables[0].Rows[0]["LogTime"].ToString();
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = "p";
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].BackColor = Color.Green;
                                    }
                                    else
                                    {
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 4].Text = "";
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].Text = "A";
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].HorizontalAlign = HorizontalAlign.Center;
                                        fpbiomatric.Sheets[0].Cells[rowstr - 1, colcount + 5].BackColor = Color.Red;
                                    }

                                }



                                // habsent++;

                            }
                        }
                    }
                }
                lblpresent1.Text = ":" + counttotalmornpresent;
                lblpresent2.Text = ":" + counttotalevennpresent;
                lblabsent1.Text = ":" + counttotalabsentmorn;
                lblabsent2.Text = ":" + counttotalabsenteven;
                lblpermission.Text = ":" + totalpermorn;//totalcountevennpermission;
                lblpermission1.Text = ":" + totalpereven;
                lbllate.Text = ":" + totalmornlate;//totallatecount;
                lbllate1.Text = ":" + totalevenlate;
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    rptprint.Visible = false;
                    lblpresent1.Text = "0";
                    lblpresent2.Text = "0";
                    lblabsent1.Text = "0";
                    lblabsent2.Text = "0";
                    lbllate.Text = "0";
                    lbllate1.Text = "0";
                    return;
                }
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Present:" + Convert.ToString(counttotalmornpresent);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Present:" + Convert.ToString(counttotalevennpresent);


                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Morning Absent:" + counttotalabsentmorn.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].RowCount++;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Bold = true;
                fpbiomatric.Sheets[0].Rows[fpbiomatric.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                fpbiomatric.Sheets[0].SpanModel.Add(fpbiomatric.Sheets[0].RowCount - 1, 1, 1, fpbiomatric.Sheets[0].ColumnCount - 1);
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = "Total No of Evening Absent:" + counttotalabsenteven.ToString();
                fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;


                Double totalRows = 0;
                totalRows = Convert.ToInt32(fpbiomatric.Sheets[0].RowCount);

                if (totalRows >= 10)
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);


                    fpbiomatric.Height = 350;
                    fpbiomatric.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    fpbiomatric.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows == 0)
                {

                    fpbiomatric.Height = 300;
                }
                else
                {
                    fpbiomatric.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                    fpbiomatric.Height = 75 + (75 * Convert.ToInt32(totalRows));
                }

                //fpbiomatric.Height = 150 + (fpbiomatric.Sheets[0].RowCount * 10);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / fpbiomatric.Sheets[0].PageSize);

                fpbiomatric.Visible = true;
                lblnorec.Visible = false;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                ViewState["Bothpresent"] = null;
                ViewState["BothAbsent"] = null;
                ViewState["Bothod"] = null;
                ViewState["Bothper"] = null;
                Str = "";
                #endregion
            }
            else if (rbdailylog.Checked == true && ddlstud.SelectedItem.ToString() == "Students")
            {
                #region Daily logs Student
                // attfiltertype.Visible = false;
                lbllate1.Visible = false;
                lblmornper.Visible = false;
                lblevenper.Visible = false;

                lbllatetext.Visible = false;
                lblheaderabsent1.Visible = false;
                lblheaderabsent2.Visible = false;
                imgabsent.Visible = false;
                lblabsent1.Visible = false;
                lblabsent2.Visible = false;
                imgpresent.Visible = false;
                imglate.Visible = false;
                lblmornlate.Visible = false;
                lblevenlate.Visible = false;
                imgper.Visible = false;
                //lblmornper.Visible = true;
                //lblevenper.Visible = true;
                //imgontime.Visible = false;
                //lblontime.Visible = false;

                lblpermission.Visible = false;
                lblpermission1.Visible = false;
                lblpresent1.Visible = false;
                lblpresent2.Visible = false;
                lbl_headermorn.Visible = false;
                lbl_headereven.Visible = false;
                lbllate.Visible = false;
                Hashtable date_count_01 = new Hashtable();
                ArrayList binddate = new ArrayList();

                fpbiomatric.Sheets[0].RowCount = 0;
                fpbiomatric.Sheets[0].RowHeader.Visible = false;
                fpbiomatric.Sheets[0].AutoPostBack = true;
                fpbiomatric.CommandBar.Visible = false;
                fpbiomatric.Visible = false;
                fpbiomatric.Sheets[0].ColumnHeader.RowCount = 2;
                fpbiomatric.Sheets[0].ColumnCount = 6;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].DefaultStyle.Locked = true;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;


                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Room No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                fpbiomatric.Sheets[0].Columns[0].Width = 40;
                fpbiomatric.Sheets[0].Columns[1].Width = 120;

                if (cblsearch.Items[0].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[1].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[1].Visible = false;
                }

                if (cblsearch.Items[1].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[2].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[2].Visible = false;
                }


                if (cblsearch.Items[2].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[3].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[3].Visible = false;
                }

                if (cblsearch.Items[3].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[4].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[4].Visible = false;
                }

                if (cblsearch.Items[4].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[5].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[5].Visible = false;
                }
                fpbiomatric.Sheets[0].GridLineColor = Color.Black;
                fpbiomatric.Height = 600;
                fpbiomatric.Width = 1000;
                fpbiomatric.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].DefaultStyle.Font.Bold = false;
                fpbiomatric.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].DefaultStyle.Border.BorderSizeBottom = 0;
                fpbiomatric.Sheets[0].DefaultStyle.Border.BorderSizeRight = 0;

                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = " and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {

                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }
                ArrayList columnhide = new ArrayList();

                string addtionam = "";

                string colegecode = "";
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        string build1 = cbl_clg.Items[i].Value.ToString();
                        if (colegecode == "")
                        {
                            colegecode = build1;
                        }
                        else
                        {
                            colegecode = colegecode + "'" + "," + "'" + build1;
                        }
                    }
                }

                //sql = "SELECT distinct rm.room_name,h.HostelMasterPK , h.hostelname, T.Roll_No,T.Stud_Name,Course_Name+'-'+" + dept + " as Degree,CONVERT(VARCHAR, T.Fingerprint1 ) as Fingerprint1,T.Roll_No FROM HT_HostelRegistration R,  Registration T,Degree G,Course C, HM_HostelMaster  h,Department D,bio..Daily_Logs B,room_detail rm Where r.APP_No = T.App_No And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code and   h.HostelMasterPK =r.HostelMasterFK  and convert(nvarchar(100),b.FINGERPRINTDETAILS)=convert(nvarchar(100),t.Fingerprint1)   and  B.DATE between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' and isnull(r.IsDiscontinued ,0)=0 and isnull(r.IsVacated ,0)=0 ";

                //Cmd Saranyadevi 4.2.2019
                //sql = " SELECT distinct rm.room_name,h.HostelMasterPK , h.hostelname, T.Roll_No,T.Stud_Name,Course_Name+'-'+Dept_acronym as Degree,CONVERT(VARCHAR, T.finger_id ) as Fingerprint1,T.Roll_No FROM HT_HostelRegistration R,  Registration T,Degree G,Course C, HM_HostelMaster  h,Department D,Daily_Logs B,room_detail rm Where r.APP_No = T.App_No And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID  AND G.Dept_Code = D.Dept_Code and   h.HostelMasterPK =r.HostelMasterFK  and convert(nvarchar(100), b.FingerID )=convert(nvarchar(100),t.finger_id)   and  B.Log_Date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' and isnull(r.IsDiscontinued ,0)=0 and isnull(r.IsVacated ,0)=0 and rm.RoomPK =r.RoomFK  and r.CollegeCode in('" + colegecode + "')";


                sql = " SELECT distinct rm.room_name,h.HostelMasterPK , h.hostelname, T.Roll_No,T.Stud_Name,Course_Name+'-'+Dept_acronym as Degree,CONVERT(VARCHAR, T.finger_id ) as Fingerprint1,T.Roll_No,isnull(T.Smart_BiouserID,'')Smart_BiouserID FROM HT_HostelRegistration R,  Registration T,Degree G,Course C, HM_HostelMaster  h,Department D,Daily_Logs B,room_detail rm Where r.APP_No = T.App_No And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID  AND G.Dept_Code = D.Dept_Code and   h.HostelMasterPK =r.HostelMasterFK  and (convert(nvarchar(100), b.FingerID )=convert(nvarchar(100),t.finger_id) or T.Smart_BiouserID=b.FingerID )  and  B.Log_Date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' and isnull(r.IsDiscontinued ,0)=0 and isnull(r.IsVacated ,0)=0 and rm.RoomPK =r.RoomFK AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and r.CollegeCode in('" + colegecode + "') And isnull(Gatepass,'')<>'1' ";

                //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " And R.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                //    addtionam = addtionam + " And R.HostelMasterFK='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                //}
                string hostelnames = string.Empty;
                if (txthostelname.Text.ToString() != "--Select--")
                {
                    if (cbl_hostelname.Items.Count > 0)
                        hostelnames = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                    sql = sql + " And R.HostelMasterFK in('" + hostelnames + "')";
                    addtionam = addtionam + " And R.HostelMasterFK in('" + hostelnames + "')";
                }
                string buil_nam = string.Empty;
                if (txt_building.Text.ToString() != "--Select--")
                {
                    if (cbl_building.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_building.Items.Count; i++)
                        {
                            if (cbl_building.Items[i].Selected == true)
                            {
                                if (buil_nam == "")
                                {
                                    buil_nam = Convert.ToString(cbl_building.Items[i].Text);
                                }
                                else
                                {
                                    buil_nam = buil_nam + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                                }
                            }
                        }
                        sql = sql + " and  Building_Name in('" + buil_nam + "')";
                    }

                }
                string roomnumber = string.Empty;
                if (txtroom_no.Text.ToString() != "--Select--")
                {
                    if (cbl_room_no.Items.Count > 0)
                    {
                        roomnumber = rs.GetSelectedItemsValueAsString(cbl_room_no);
                        sql = sql + " AND R.RoomFK in('" + roomnumber + "')";
                        addtionam = addtionam + " AND R.RoomFK in('" + roomnumber + "')";
                    }

                }
                //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND R.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";
                //    addtionam = addtionam + " AND R.RoomFK ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";
                //}
                string flrNames = string.Empty;
                if (txtfloorname.Text.ToString() != "--Select--")
                {
                    if (cbl_floorName.Items.Count > 0)
                    {
                        flrNames = rs.GetSelectedItemsValueAsString(cbl_floorName);
                        sql = sql + " and R.FloorFK in('" + flrNames + "')";
                        addtionam = addtionam + " and R.FloorFK in('" + flrNames + "')";
                    }

                }
                string courseidvs = string.Empty;
                string batchyrvs = string.Empty;
                string branchvs = string.Empty;
                if (txt_degree.Text.ToString() != "--Select--")
                {
                    if (cbl_degree.Items.Count > 0)
                        courseidvs = rs.GetSelectedItemsValueAsString(cbl_degree);
                    sql = sql + " AND C.Course_id in('" + courseidvs + "')";
                    addtionam = addtionam + " AND C.Course_id in('" + courseidvs + "')";
                }
                if (txt_batchyr.Text.ToString() != "--Select--")
                {
                    if (cbl_batchyear.Items.Count > 0)
                        batchyrvs = rs.GetSelectedItemsValueAsString(cbl_batchyear);
                    sql = sql + " AND T.Batch_Year in('" + batchyrvs + "')";
                    addtionam = addtionam + "  AND T.Batch_Year in('" + batchyrvs + "')";


                }
                if (txtbranch.Text.ToString() != "--Select--")
                {
                    if (cbl_branch.Items.Count > 0)
                        branchvs = rs.GetSelectedItemsValueAsString(cbl_branch);
                    sql = sql + " AND G.Degree_code in('" + branchvs + "')";
                    addtionam = addtionam + "  AND G.Degree_code in('" + branchvs + "')";
                }
                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                //    addtionam = addtionam + " and R.FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                //}
                //if (ddlDegree.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND C.Course_id ='" + ddlDegree.SelectedItem.Value.ToString() + "'";
                //}
                string studrollsss = string.Empty;
                if (txtrollnum.Text.ToString() != "--Select--")
                {
                    if (cbl_rollnum.Items.Count > 0)
                    {
                        studrollsss = rs.GetSelectedItemsValueAsString(cbl_rollnum);

                        sql = sql + " AND T.App_No in('" + studrollsss + "')";
                        addtionam = addtionam + "  AND T.App_No in('" + studrollsss + "')";
                    }

                }
                //if (cboroll.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND T.App_No ='" + cboroll.SelectedItem.Value + "'";
                //    addtionam = addtionam + "  AND T.App_No ='" + cboroll.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBatch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //    addtionam = addtionam + " AND T.Batch_Year ='" + ddlBatch.SelectedItem.Value.ToString() + "'";
                //}

                //if (ddlBranch.SelectedItem.ToString() != "Select")//delsi
                //{
                //    sql = sql + " AND G.Degree_code='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //    addtionam = addtionam + " AND T.Degree_code='" + ddlBranch.SelectedItem.Value.ToString() + "'";
                //}
                string student = string.Empty;
                if (txtstudename.Text.ToString() != "--Select--")
                {
                    if (cbl_studeName.Items.Count > 0)
                    {
                        student = rs.GetSelectedItemsValueAsString(cbl_studeName);
                        sql = sql + " AND T.App_No in('" + studrollsss + "')";
                        addtionam = addtionam + "  AND T.App_No in('" + studrollsss + "')";

                    }

                }
                //if (cbostudentname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and t.stud_name like '%" + cbostudentname.Text + "%' ";
                //    addtionam = addtionam + " and t.stud_name like '%" + cbostudentname.Text + "%' ";
                //}
                if (Chktimein.Checked == true)
                {
                    //strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                    string time = cbo_hrtin.Text.Trim();
                    string time1 = cbo_hrinto.Text.Trim();
                    if (time != "")
                    {
                        if (time.Length == 1)
                            time = "0" + time;
                        else
                            time = time;
                    }
                    if (time1 != "")
                    {
                        if (time1.Length == 1)
                            time1 = "0" + time1;
                        else
                            time1 = time1;

                    }
                    //string time = "0" + cbo_hrtin.Text;
                    strTime = " and  LogTime between '" + time.Trim() + ":" + cbo_mintimein.Text + " " + cbo_in.Text + "' and '" + time1.Trim() + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                    sql = sql + " " + strTime + "";
                }
                //if (Chktimein.Checked == true)
                //{

                //    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                //    sql = sql + " " + strTime + "";
                //}
                //else if (Chktimeout.Checked == true)
                //{

                //    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";
                //    sql = sql + " " + strTime + "";
                //}
                sql = sql + " order by h.HostelName,rm.Room_Name,t.Roll_No,Degree";
                fpbiomatric.Sheets[0].RowCount = 0;
                DataSet bioattend = new DataSet();
                DataSet biofinger = new DataSet();
                DataSet bioattcount = new DataSet();
                bioattend = d2.select_method_wo_parameter(sql, "Test");
                string access_date = "";
                int sno = 0;
                string empty_date = "";
                string org_date = "";
                int columndatecount = 0;
                int findbigrepeatcount = 0;
                int biorepeatcount = 0;

                for (int i = 0; i < bioattend.Tables[0].Rows.Count; i++)
                {
                    fpbiomatric.Visible = true;
                    findbigrepeatcount = 0;
                    sno++;
                    fpbiomatric.Sheets[0].RowCount++;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = bioattend.Tables[0].Rows[i]["Roll_No"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Tag = bioattend.Tables[0].Rows[i]["Fingerprint1"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Note = bioattend.Tables[0].Rows[i]["Roll_No"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;


                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].Text = bioattend.Tables[0].Rows[i]["Stud_Name"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].Tag = bioattend.Tables[0].Rows[i]["Smart_BiouserID"].ToString();

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 3].Text = bioattend.Tables[0].Rows[i]["Degree"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 4].Text = bioattend.Tables[0].Rows[i]["HostelName"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 5].Text = bioattend.Tables[0].Rows[i]["room_name"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                }

                for (DateTime dt = dt1; dt <= dt2; dt = dt.AddDays(1))
                {

                    //string strnum = d2.GetFunction(" SELECT distinct  COUNT(*) as count1,t.Roll_No,t.Stud_Name,b.FINGERPRINTDETAILS,t.Roll_Admit FROM HT_HostelRegistration R, Registration T,bio..Daily_Logs B Where r.APP_No = T.App_No and convert(nvarchar(100),b.FINGERPRINTDETAILS)=convert(nvarchar(100),t.Fingerprint1) and B.DATE ='" + dt.ToString("MM/dd/yyyy") + "' and isnull(r.IsVacated ,0)=0 and isnull(r.IsDiscontinued ,0)=0 and r.BuildingFK<>'' and MemType=1 " + addtionam + " group by t.Roll_No,t.Stud_Name,b.FINGERPRINTDETAILS,t.Roll_Admit order by count1 desc");

                    //Cmd Saranyadevi 4.2.2019
                    //string strnum = d2.GetFunction("SELECT distinct  COUNT(*) as count1,t.Roll_No,t.Stud_Name,b.FingerID  as FINGERPRINTDETAILS,t.Roll_Admit FROM HT_HostelRegistration R, Registration T,Daily_Logs B Where r.APP_No = T.App_No and convert(nvarchar(100),b.FingerID)=convert(nvarchar(100),t.finger_id) and B.Log_Date ='" + dt.ToString("MM/dd/yyyy") + "' and isnull(r.IsVacated ,0)=0 and isnull(r.IsDiscontinued ,0)=0 and r.BuildingFK<>'' and MemType=1  and r.CollegeCode in('" + colegecode + "')   " + addtionam + "  " + strTime + " group by t.Roll_No,t.Stud_Name,b.FingerID,t.Roll_Admit order by count1 desc");
                    string strnum1 = d2.GetFunction("SELECT distinct  COUNT(*) as count1,t.Roll_No,t.Stud_Name,b.FingerID  as FINGERPRINTDETAILS,t.Roll_Admit  FROM HT_HostelRegistration R, Registration T,Daily_Logs B,Degree G,Course C, HM_HostelMaster  h,Department D,room_detail rm  Where r.APP_No = T.App_No and convert(nvarchar(100),b.FingerID)=convert(nvarchar(100),t.finger_id) and B.Log_Date ='" + dt.ToString("MM/dd/yyyy") + "' and isnull(r.IsVacated ,0)=0 and isnull(r.IsDiscontinued ,0)=0 and r.BuildingFK<>'' and MemType=1 AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and r.APP_No = T.App_No And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID  AND G.Dept_Code = D.Dept_Code and   h.HostelMasterPK =r.HostelMasterFK and isnull(r.IsDiscontinued ,0)=0 and isnull(r.IsVacated ,0)=0 and rm.RoomPK =r.RoomFK   and r.CollegeCode in('" + colegecode + "') And isnull(Gatepass,'')<>'1'    " + addtionam + "  " + strTime + " group by t.Roll_No,t.Stud_Name,b.FingerID,t.Roll_Admit order by count1 desc");


                    string strnum2 = d2.GetFunction("SELECT distinct  COUNT(*) as count1,t.Roll_No,t.Stud_Name,t.Roll_Admit,isnull(T.Smart_BiouserID,'')Smart_BiouserID  FROM HT_HostelRegistration R, Registration T,Daily_Logs B,Degree G,Course C, HM_HostelMaster  h,Department D,room_detail rm Where r.APP_No = T.App_No and T.Smart_BiouserID=b.FingerID and B.Log_Date ='" + dt.ToString("MM/dd/yyyy") + "' and isnull(r.IsVacated ,0)=0 and isnull(r.IsDiscontinued ,0)=0 and r.BuildingFK<>'' and MemType=1 AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and r.APP_No = T.App_No And T.degree_code = G.degree_code AND G.Course_ID = C.Course_ID  AND G.Dept_Code = D.Dept_Code and   h.HostelMasterPK =r.HostelMasterFK and isnull(r.IsDiscontinued ,0)=0 and isnull(r.IsVacated ,0)=0 and rm.RoomPK =r.RoomFK   and r.CollegeCode in('" + colegecode + "') And isnull(Gatepass,'')<>'1'   " + addtionam + "  " + strTime + " group by t.Roll_No,t.Stud_Name,t.Roll_Admit,Smart_BiouserID order by count1 desc");

                    string strnum = "";

                    int num1 = 0; int num2 = 0;
                    int.TryParse(strnum1, out num1);
                    int.TryParse(strnum2, out num2);
                    int punum = num1 + num2;
                    strnum = Convert.ToString(punum);
                    //string g = "SELECT distinct  COUNT(*) as count1,t.Roll_No,t.Stud_Name,b.FingerID  as FINGERPRINTDETAILS,t.Roll_Admit  FROM HT_HostelRegistration R, Registration T,Daily_Logs B Where r.APP_No = T.App_No and convert(nvarchar(100),b.FingerID)=convert(nvarchar(100),t.finger_id) and B.Log_Date ='" + dt.ToString("MM/dd/yyyy") + "' and isnull(r.IsVacated ,0)=0 and isnull(r.IsDiscontinued ,0)=0 and r.BuildingFK<>'' and MemType=1 AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'  and r.CollegeCode in('" + colegecode + "')   " + addtionam + "  " + strTime + " group by t.Roll_No,t.Stud_Name,b.FingerID,t.Roll_Admit order by count1 desc";
                    int k = 0;
                    if (strnum.Trim() != "" && strnum != null && strnum.Trim() != "0")
                    {
                        k++;
                        int colcou = Convert.ToInt32(strnum);
                        int stcol = fpbiomatric.Sheets[0].ColumnCount++;
                        //fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 1;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Text = dt.ToString("dd/MM/yyyy");
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Font.Size = FontUnit.Medium;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Font.Name = "Book Antiqua";
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Font.Bold = true;
                        string dat = Convert.ToString(dt.ToString("MM/dd/yyyy"));
                        binddate.Add(dat);
                        //date_count_01.Add(dat, colcou);
                        int pounch = 0;
                        for (int hst = 0; hst < colcou; hst++)
                        {
                            pounch = pounch + 1;
                            //if (hst > 0)
                            //{
                            //    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 1;
                            //}
                            //else
                            //{
                            //    fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount++;
                            //}

                            if (!date_count_01.ContainsKey(dat))
                            {
                                date_count_01.Add(dat, fpbiomatric.Sheets[0].ColumnCount);
                            }
                            else
                            {
                                fpbiomatric.Sheets[0].ColumnCount++;
                            }
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Punch " + (pounch).ToString();
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Punch " + (pounch - 1).ToString();
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            //fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        }

                        //string strbioattedetails = "SELECT distinct CONVERT(VARCHAR, b.DATE, 103) as Access_Date, right(CONVERT(nvarchar(100),INTIME ,100),7) as Time_In,right(CONVERT(nvarchar(100),OUTTIME ,100),7) as Time_Out,T.Roll_No FROM HT_HostelRegistration R,Registration T,HM_HostelMaster h,bio..Daily_Logs B Where r.APP_No = T.App_No and h.HostelMasterPK =r.HostelMasterFK   and convert(nvarchar(100),b.FINGERPRINTDETAILS)=convert(nvarchar(100),t.Fingerprint1) and  B.DATE='" + dt.ToString("MM/dd/yyyy") + "' and isnull(r.IsVacated ,0)=0 and isnull(r.IsSuspend ,0)=0  " + addtionam + " order by t.Roll_No,Time_In";
                        //string strbioattedetails = "SELECT distinct CONVERT(VARCHAR, b.Log_Date , 103) as Access_Date, T.Roll_No FROM HT_HostelRegistration R,Registration T,HM_HostelMaster h,Daily_Logs B Where r.APP_No = T.App_No and h.HostelMasterPK =r.HostelMasterFK   and convert(nvarchar(100),b.FingerID )=convert(nvarchar(100),t.Fingerprint1) and  B.Log_Date='" + dt.ToString("MM/dd/yyyy") + "' and isnull(r.IsVacated ,0)=0 and isnull(r.IsSuspend ,0)=0  " + addtionam + " order by t.Roll_No";

                        //DataSet dsbioattdetails = d2.select_method_wo_parameter(strbioattedetails, "Text");
                        //for (int i = 0; i < fpbiomatric.Sheets[0].Rows.Count; i++)
                        //{
                        //    string roll_no = fpbiomatric.Sheets[0].Cells[i, 1].Note.ToString();
                        //    dsbioattdetails.Tables[0].DefaultView.RowFilter = "Roll_No='" + roll_no + "'";
                        //    DataView dvstu = dsbioattdetails.Tables[0].DefaultView;
                        //    int setva = stcol - 1;
                        //    for (int d = 0; d < dvstu.Count; d++)
                        //    {

                        //        setva = setva + 2;
                        //        //02.05.16
                        //        //string getintime= dvstu[d]["Time_In"].ToString();
                        //        //string getouttime = dvstu[d]["Time_Out"].ToString();

                        //        string getintime = dvstu[d]["Time_Out"].ToString();
                        //        string getouttime = dvstu[d]["Time_In"].ToString();

                        //        fpbiomatric.Sheets[0].Cells[i, setva - 1].Text = getintime;
                        //        fpbiomatric.Sheets[0].Cells[i, setva].Text = getouttime;
                        //    }
                        //}
                        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, stcol, 1, (colcou));//* 2
                    }
                }

                biofinger.Clear();
                string fpdate = ""; int fbcl = 0;
                for (int k = 0; k < fpbiomatric.Sheets[0].RowCount; k++)
                {

                    int fpcount = Convert.ToInt32(fpbiomatric.Sheets[0].ColumnCount);
                    string fingerid = fpbiomatric.Sheets[0].Cells[k, 1].Tag.ToString();
                    string biouserid = fpbiomatric.Sheets[0].Cells[k, 2].Tag.ToString();
                    ArrayList logtime = new ArrayList(); int fbcol = 0;
                    for (int s = 0; s < binddate.Count; s++)
                    {
                        fpdate = binddate[s].ToString();
                        string[] convert_fpdate = fpdate.Split('/');


                        //Hostel Biometric
                        sql = "  select * from Daily_Logs where FingerID='" + fingerid + "'  and Log_Date='" + binddate[s].ToString() + "'  " + strTime + " And isnull(Gatepass,'')<>'1' order by cast(LogTime as datetime)";// desc";
                        biofinger = d2.select_method_wo_parameter(sql, "Text");
                        int bind_columcount = Convert.ToInt32(date_count_01[fpdate]);
                        Boolean column = false;
                        for (int z = 0; z < biofinger.Tables[0].Rows.Count; z++)
                        {
                            string logss = biofinger.Tables[0].Rows[z]["LogTime"].ToString();
                            if (z != 0)
                            {
                                bind_columcount++;
                            }
                            fpbiomatric.Sheets[0].Cells[k, bind_columcount - 1].Text = logss.ToString();
                            column = true;
                            //  logtime.Add(logss);
                        }
                        //Mess Smart Card
                        int bind_columcount1 = 0;
                        if (!column)
                            bind_columcount1 = Convert.ToInt32(bind_columcount);
                        else
                            bind_columcount1 = Convert.ToInt32(bind_columcount + 1);
                        sql = "  select * from Daily_Logs where FingerID='" + biouserid + "'  and Log_Date='" + binddate[s].ToString() + "' And isnull(Gatepass,'')<>'1'  " + strTime + " order by cast(LogTime as datetime)";// desc";
                        biofinger = d2.select_method_wo_parameter(sql, "Text");

                        for (int z = 0; z < biofinger.Tables[0].Rows.Count; z++)
                        {
                            string logss = biofinger.Tables[0].Rows[z]["LogTime"].ToString();
                            if (z != 0)
                            {
                                bind_columcount1++;
                            }
                            fpbiomatric.Sheets[0].Cells[k, bind_columcount1 - 1].Text = logss.ToString();
                            fpbiomatric.Sheets[0].Cells[k, bind_columcount1 - 1].BackColor = Color.Chocolate;
                            //  logtime.Add(logss);
                        }



                    }
                }
                fpbiomatric.Sheets[0].PageSize = fpbiomatric.Sheets[0].RowCount;

                //fpbiomatric.Sheets[0].FrozenColumnCount = 4;
                //for (int k = 0; k < columnhide.Count; k++)
                //{
                //    int val_hide = Convert.ToInt32(columnhide[k].ToString());
                //    fpbiomatric.Sheets[0].Columns[val_hide].Visible = false;
                //}
                //ArrayList abspersent = new ArrayList();
                //biorepeatcount = fpbiomatric.Sheets[0].ColumnCount;
                //fpbiomatric.Sheets[0].ColumnCount++;
                //abspersent.Add(biorepeatcount);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Text = "Absent";
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Name = "Book Antiqua";
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, biorepeatcount, 2, 1);
                //biorepeatcount = fpbiomatric.Sheets[0].ColumnCount;
                //fpbiomatric.Sheets[0].ColumnCount++;
                //abspersent.Add(biorepeatcount);
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Text = "Present";
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Size = FontUnit.Medium;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Name = "Book Antiqua";
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].HorizontalAlign = HorizontalAlign.Center;
                //fpbiomatric.Sheets[0].ColumnHeader.Cells[0, biorepeatcount].Font.Bold = true;
                //fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, biorepeatcount, 2, 1);
                //fpbiomatric.Sheets[0].PageSize = fpbiomatric.Sheets[0].RowCount;                
                //int totalpreeesent = 0;
                //int totalabbsent = 0;
                //countpermission2 = 0;
                //countlate2 = 0;
                //for (int mm = 0; mm < fpbiomatric.Sheets[0].RowCount; mm++)
                //{
                //    string rollno = fpbiomatric.Sheets[0].Cells[mm, 1].Note.ToString();
                //    for (int s = 0; s < binddate.Count; s++)
                //    {
                //        countpresent2 = 0;
                //        countabsent2 = 0;
                //        string datetagvalue;
                //        datetagvalue = binddate[s].ToString();
                //        string[] monyeararr = datetagvalue.Split('/');
                //        int year = Convert.ToInt16(monyeararr[2]);
                //        int monyear = year * 12 + Convert.ToInt16(monyeararr[0]);
                //        int day10 = Convert.ToInt16(monyeararr[1]);
                //        bioattcount.Clear();
                //        string sql2 = "select * from hattendance where month_year='" + monyear + "' and roll_no='" + rollno + "'";
                //        bioattcount = d2.select_method_wo_parameter(sql, "Text");
                //        int catt = bioattcount.Tables[0].Rows.Count;
                //        if (catt > 0)
                //        {
                //            string str1 = "";
                //            string attmark = "";
                //            attmark = bioattcount.Tables[0].Rows[0]["d" + day10 + ""].ToString();
                //            string[] splitatt = attmark.Split('-');
                //            attmark = splitatt[0];
                //            str1 = Attmark(attmark);
                //            if (str1 == "P")
                //            {
                //                countpresent2++;
                //                totalpreeesent++;
                //            }
                //            if (str1 == "A")
                //            {
                //                countabsent2++;
                //                totalabbsent++;
                //            }
                //            if (str1 == "LA")
                //            {
                //                countlate2++;
                //            }
                //            if (str1 == "PER")
                //            {
                //                countpermission2++;
                //            }
                //        }
                //    }
                //    fpbiomatric.Sheets[0].Cells[mm, Convert.ToInt32(abspersent[0].ToString())].Text = Convert.ToString(countpresent2);
                //    fpbiomatric.Sheets[0].Cells[mm, Convert.ToInt32(abspersent[1].ToString())].Text = Convert.ToString(countabsent2);
                //}
                //lblpresent.Text = Convert.ToString(totalpreeesent);
                //lblabsent.Text = Convert.ToString(totalabbsent);
                lbllate.Visible = false;
                lblpermission.Visible = false;
                lblpermission1.Visible = false;
                //lblontime.Visible = false;
                imglate.Visible = false;
                lblmornlate.Visible = false;
                lblevenlate.Visible = false;
                imgper.Visible = false;
                lblmornper.Visible = false;
                lblevenper.Visible = false;
                Fieldset1.Visible = true;
                Fieldset2.Visible = true;
                //imgontime.Visible = false;
                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    Fieldset1.Visible = false;
                    Fieldset2.Visible = false;
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    rptprint.Visible = false;
                    return;
                }
                #endregion
            }
            else if (rbdailylog.Checked == true && ddlstud.SelectedItem.ToString() == "Guest")
            {
                #region Daily log guest
                //attfiltertype.Visible = false;
                lbllatetext.Visible = false;
                lblheaderabsent1.Visible = true;
                lblheaderabsent2.Visible = true;
                imgabsent.Visible = true;
                lblabsent1.Visible = true;
                lblabsent2.Visible = true;
                imgpresent.Visible = true;
                imglate.Visible = true;
                lblmornlate.Visible = true;
                lblevenlate.Visible = true;
                imgper.Visible = true;
                lblmornper.Visible = true;
                lblevenper.Visible = true;
                //imgontime.Visible = true;
                //lblontime.Visible = true;

                lblpermission.Visible = true;
                lblpermission1.Visible = true;
                lblpresent1.Visible = true;
                lblpresent2.Visible = true;
                lbl_headermorn.Visible = true;
                lbl_headereven.Visible = true;
                lbllate.Visible = true;
                Hashtable date_count_01 = new Hashtable();
                ArrayList binddate = new ArrayList();

                fpbiomatric.Sheets[0].RowCount = 0;
                fpbiomatric.Sheets[0].RowHeader.Visible = false;
                fpbiomatric.Sheets[0].AutoPostBack = false;
                fpbiomatric.CommandBar.Visible = false;
                fpbiomatric.Visible = false;
                fpbiomatric.Sheets[0].ColumnHeader.RowCount = 2;
                fpbiomatric.Sheets[0].ColumnCount = 6;


                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpbiomatric.Sheets[0].DefaultStyle.Locked = true;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Guest Code";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Guest Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;


                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hostel Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Floor Name";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Room No";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpbiomatric.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;

                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                fpbiomatric.Sheets[0].Columns[0].Width = 40;
                fpbiomatric.Sheets[0].Columns[1].Width = 120;

                if (cblsearch.Items[0].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[1].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[1].Visible = false;
                }

                if (cblsearch.Items[1].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[2].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[2].Visible = false;
                }


                if (cblsearch.Items[3].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[3].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[3].Visible = false;
                }
                fpbiomatric.Sheets[0].Columns[4].Visible = false;
                if (cblsearch.Items[4].Selected == true)
                {
                    fpbiomatric.Sheets[0].Columns[5].Visible = true;
                }
                else
                {
                    fpbiomatric.Sheets[0].Columns[5].Visible = false;
                }


                fpbiomatric.Sheets[0].GridLineColor = Color.Black;
                fpbiomatric.Height = 600;
                fpbiomatric.Width = 1000;
                fpbiomatric.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                fpbiomatric.Sheets[0].DefaultStyle.Font.Bold = false;
                fpbiomatric.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                fpbiomatric.Sheets[0].DefaultStyle.Border.BorderSizeBottom = 0;
                fpbiomatric.Sheets[0].DefaultStyle.Border.BorderSizeRight = 0;

                date1 = Txtentryfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                date2 = Txtentryto.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                TimeSpan t = dt2.Subtract(dt1);
                long days = t.Days;
                day3 = Convert.ToInt32(days);
                day3 = day3 + 1;

                strdate = " and B.access_date between '" + datefrom + "' and '" + dateto + "'";
                if (days >= 0)
                {

                }
                else
                {
                    lbldate.Visible = true;
                    lbldate.Text = "Date Must Be Greater Than From Date";
                }

                string addtionam = "";
                sql = "select distinct g.Guest_Name,g.Guest_Code,g.Finger_ID,g.Building_Name,h.Hostel_Name,g.Floor_Name,g.Room_Name from Hostel_GuestReg g,Hostel_Details h,bio..Daily_Logs b,Building_Master bm,Floor_Master fm,Room_Detail rd where g.Hostel_Code = h.Hostel_code and g.Finger_ID = b.FINGERPRINTDETAILS and g.Building_Name=bm.Building_Name and bm.Building_Name=fm.Building_Name and g.Floor_Name=fm.Floor_Name and rd.Building_Name=g.Building_Name and rd.Building_Name=bm.Building_Name and g.Room_Name=rd.Room_Name and B.DATE between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' ";
                //if (Cbo_HostelName.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " And g.Hostel_Code='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                //    addtionam = addtionam + " And g.Hostel_Code='" + Cbo_HostelName.SelectedItem.Value.ToString() + "'";
                //}
                string hostelnames = string.Empty;
                if (txthostelname.Text.ToString() != "--Select--")
                {
                    if (cbl_hostelname.Items.Count > 0)
                        hostelnames = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                    sql = sql + " And g.Hostel_Code in('" + hostelnames + "')";
                    addtionam = addtionam + " And g.Hostel_Code in('" + hostelnames + "')";
                }

                string buil_nams = string.Empty;
                if (txt_building.Text.ToString() != "--Select--")
                {
                    if (cbl_building.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_building.Items.Count; i++)
                        {
                            if (cbl_building.Items[i].Selected == true)
                            {
                                if (buil_nams == "")
                                {
                                    buil_nams = Convert.ToString(cbl_building.Items[i].Text);
                                }
                                else
                                {
                                    buil_nams = buil_nams + "'" + "," + "'" + Convert.ToString(cbl_building.Items[i].Text);
                                }
                            }
                        }
                        sql = sql + " and  Building_Name in('" + buil_nams + "')";
                    }

                }
                string roomnos = string.Empty;
                if (txtroom_no.Text.ToString() != "--Select--")
                {
                    if (cbl_room_no.Items.Count > 0)
                    {
                        roomnos = rs.GetSelectedItemsValueAsString(cbl_room_no);
                        sql = sql + " AND g.Room_Name  in('" + roomnos + "')";
                        addtionam = addtionam + " AND g.Room_Name in('" + roomnos + "')";
                    }

                }
                //if (Cbo_Room.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " AND g.Room_Name ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";
                //    addtionam = addtionam + " AND g.Room_Name ='" + Cbo_Room.SelectedItem.Value.ToString() + "'";
                //}
                string fname = string.Empty;
                if (txtfloorname.Text.ToString() != "--Select--")
                {
                    if (cbl_floorName.Items.Count > 0)
                    {
                        fname = rs.GetSelectedItemsValueAsString(cbl_floorName);
                        sql = sql + " and g.floor_name in('" + fname + "')";
                        addtionam = addtionam + " and g.floor_name in('" + fname + "')";
                    }

                }
                //if (cbofloorname.SelectedItem.Value.ToString() != "All")
                //{
                //    sql = sql + " and g.floor_name='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                //    addtionam = addtionam + " and g.floor_name='" + cbofloorname.SelectedItem.Value.ToString() + "'";
                //}

                if (Chktimein.Checked == true)
                {

                    strTime = " and  Right(CONVERT(nvarchar(100),time_in ,100),7) between '" + cbo_hrtin.Text + ":" + cbo_mintimein.Text + cbo_in.Text + "'  and '" + cbo_hrinto.Text + ":" + cbo_mininto.Text + " " + cbointo.Text + "'";
                    sql = sql + " " + strTime + "";
                }
                else if (Chktimeout.Checked == true)
                {

                    strTime = " AND Right(CONVERT(nvarchar(100),time_out ,100),7) between '" + cbo_hours.Text + ":" + cbo_min.Text + cbo_sec.Text + " '  and '" + cbo_hour2.SelectedItem.Value.ToString() + ":" + cbo_min2.SelectedItem.Value.ToString() + " " + cbo_sec2.Text + " '";

                    sql = sql + " " + strTime + "";
                }
                sql = sql + " order by g.Guest_Code,g.Guest_Name,g.Building_Name,g.Floor_Name,g.Room_Name ";
                fpbiomatric.Sheets[0].RowCount = 0;
                DataSet bioattend = new DataSet();
                DataSet biofinger = new DataSet();
                DataSet bioattcount = new DataSet();
                bioattend = d2.select_method_wo_parameter(sql, "Test");
                string access_date = "";
                int sno = 0;
                string empty_date = "";
                string org_date = "";
                int columndatecount = 0;
                int findbigrepeatcount = 0;
                int biorepeatcount = 0;

                for (int i = 0; i < bioattend.Tables[0].Rows.Count; i++)
                {
                    fpbiomatric.Visible = true;
                    findbigrepeatcount = 0;
                    sno++;
                    fpbiomatric.Sheets[0].RowCount++;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Text = bioattend.Tables[0].Rows[i]["Guest_Code"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].Note = bioattend.Tables[0].Rows[i]["Finger_ID"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;


                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].Text = bioattend.Tables[0].Rows[i]["Guest_Name"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 3].Text = bioattend.Tables[0].Rows[i]["Hostel_Name"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 4].Text = bioattend.Tables[0].Rows[i]["Floor_Name"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 5].Text = bioattend.Tables[0].Rows[i]["Room_Name"].ToString();
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    fpbiomatric.Sheets[0].Cells[fpbiomatric.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                }

                for (DateTime dt = dt1; dt <= dt2; dt = dt.AddDays(1))
                {
                    string strnum = d2.GetFunction("SELECT distinct  COUNT(*) as count1,g.Finger_ID FROM Hostel_GuestReg g,Hostel_Details h,bio..Daily_Logs b   Where  g.Hostel_Code = h.Hostel_code and g.Finger_ID = b.FINGERPRINTDETAILS and  B.DATE = '" + dt.ToString("MM/dd/yyyy") + "' group by g.Finger_ID order by count1 desc");
                    if (strnum.Trim() != "" && strnum != null && strnum.Trim() != "0")
                    {
                        int colcou = Convert.ToInt32(strnum);
                        int stcol = fpbiomatric.Sheets[0].ColumnCount;
                        fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 1;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Text = dt.ToString("dd/MM/yyyy");
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Font.Size = FontUnit.Medium;
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Font.Name = "Book Antiqua";
                        fpbiomatric.Sheets[0].ColumnHeader.Cells[0, stcol].Font.Bold = true;

                        int pounch = 0;
                        for (int hst = 0; hst < colcou; hst++)
                        {
                            pounch = pounch + 2;
                            if (hst > 0)
                            {
                                fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 2;
                            }
                            else
                            {
                                fpbiomatric.Sheets[0].ColumnCount = fpbiomatric.Sheets[0].ColumnCount + 1;
                            }
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].Text = "Punch " + (pounch - 1).ToString();
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Text = "Punch " + (pounch).ToString();

                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                            fpbiomatric.Sheets[0].ColumnHeader.Cells[1, fpbiomatric.Sheets[0].ColumnCount - 2].Font.Bold = true;
                        }

                        string strbioattedetails = "SELECT g.Finger_ID,CONVERT(VARCHAR, b.DATE, 103) as Access_Date, right(CONVERT(nvarchar(100),INTIME ,100),7) as Time_In,right(CONVERT(nvarchar(100),OUTTIME ,100),7) as Time_Out FROM Hostel_GuestReg g,Hostel_Details h,bio..Daily_Logs b  Where  g.Hostel_Code = h.Hostel_code and g.Finger_ID = b.FINGERPRINTDETAILS and  B.DATE = '" + dt.ToString("MM/dd/yyyy") + "' order by g.Finger_ID,b.Access_Date,b.Time_In desc ";
                        DataSet dsbioattdetails = d2.select_method_wo_parameter(strbioattedetails, "Text");
                        for (int i = 0; i < fpbiomatric.Sheets[0].Rows.Count; i++)
                        {
                            string roll_no = fpbiomatric.Sheets[0].Cells[i, 1].Note.ToString();
                            dsbioattdetails.Tables[0].DefaultView.RowFilter = "Finger_ID='" + roll_no + "'";
                            DataView dvstu = dsbioattdetails.Tables[0].DefaultView;
                            int setva = stcol - 1;
                            for (int d = 0; d < dvstu.Count; d++)
                            {

                                setva = setva + 2;
                                //02.05.16
                                //string getintime = dvstu[d]["Time_In"].ToString();
                                //string getouttime = dvstu[d]["Time_Out"].ToString();
                                string getintime = dvstu[d]["Time_Out"].ToString();
                                string getouttime = dvstu[d]["Time_In"].ToString();

                                fpbiomatric.Sheets[0].Cells[i, setva - 1].Text = getintime;
                                fpbiomatric.Sheets[0].Cells[i, setva].Text = getouttime;
                            }
                        }
                        fpbiomatric.Sheets[0].ColumnHeaderSpanModel.Add(0, stcol, 1, (colcou * 2));
                    }
                }

                fpbiomatric.Sheets[0].PageSize = fpbiomatric.Sheets[0].RowCount;

                if (fpbiomatric.Sheets[0].RowCount == 0)
                {
                    lblnorec.Visible = true;
                    fpbiomatric.Visible = false;
                    btnprintmaster.Visible = false;
                    rptprint.Visible = false;
                    return;
                }
                #endregion
            }
        }
        catch
        {
        }

    }

    public string Attmark(string Attstr_mark)
    {
        string Att_mark;
        Att_mark = "";
        if (Attstr_mark == "0")     //Added By Saranyadevi 19.4.2018
        {
            Att_mark = "A";
        }
        else if (Attstr_mark == "1")
        {
            Att_mark = "P";
        }
        else if (Attstr_mark == "2")
        {
            Att_mark = "A";
        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";
        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "LA";
        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "S";
        }
        else if (Attstr_mark == "6")
        {
            Att_mark = "PER";
        }
        else if (Attstr_mark == "7")
        {
            Att_mark = "L";
        }
        return Att_mark;
    }
    public string getfunction3(string sql)
    {
        string sqlstr1;
        sqlstr1 = sql;
        mycon.Close();
        mycon.Open();
        SqlCommand cmd10 = new SqlCommand(sqlstr1, mycon);
        SqlDataReader dr10;
        dr10 = cmd10.ExecuteReader();
        while (dr10.Read())
        {
            if (dr10.HasRows == true)
            {
                string att = "";
                att = dr10["att"].ToString();
                if (att == "P")
                {
                    countpresent = countpresent + 1;
                }
                if (att == "A")
                {
                    countabsent = countabsent + 1;
                }
                lblpresent1.Text = ":" + countpresent;
                lblpresent2.Text = ":" + "0";

                //lblpresent.Visible = true;
                lblabsent1.Text = ":" + countabsent;
                lblabsent2.Text = ":" + "0";
                if (att == "LA")
                {
                    countlate = countlate + 1;
                }
                lbllate.Text = ":" + countlate;
                if (att == "PER")
                {
                    countpermission = countpermission + 1;
                }
                lblpermission.Text = ":" + countpermission;
            }
        }
        return "";
        dr10.Close();
        mycon.Close();
    }
    public string getfunction(string sql1)
    {
        string sqlstr;
        sqlstr = sql1;
        mycon.Close();
        mycon.Open();
        SqlCommand cmd10 = new SqlCommand(sqlstr, mycon);
        SqlDataReader dr11;
        dr11 = cmd10.ExecuteReader();
        dr11.Read();
        if (dr11.HasRows == true)
        {
            string gettimein = "";
            gettimein = dr11["intime"].ToString();
            return gettimein;
        }
        else
        {
            return "";
        }
    }
    protected void Cbo_HostelName_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_floorname();
        load_room();
        load_studname();
        load_rollno();
        load_studname();
        bindcollege();
    }
    protected void rdotoday_CheckedChanged(object sender, EventArgs e)
    {
        //rdoparticular.Checked = false;
        //rdodatebetween.Checked = false;
    }
    protected void Cbo_Room_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_rollno();
        load_studname();
    }
    protected void Cbo_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        //load_branch();
    }
    protected void Cbo_Branch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void Chk_roll_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void chktimebwt_CheckedChanged(object sender, EventArgs e)
    {
        Chktimein.Visible = true;
        Chktimeout.Visible = true;

        Chktimein.Visible = false;
        Chktimeout.Visible = false;
        cbo_hour2.Visible = false;
        cbo_min2.Visible = false;
        cbo_hours.Visible = false;
        cbo_min.Visible = false;
        cbo_hrtin.Visible = false;
        cbo_hrinto.Visible = false;
        cbo_mintimein.Visible = false;
        cbo_mininto.Visible = false;
        cbointo.Visible = false;
        cbo_in.Visible = false;
        cbo_sec.Visible = false;
        cbo_sec2.Visible = false;
        lbltoutto.Visible = false;
        lblto.Visible = false;
    }
    protected void Chktimein_CheckedChanged(object sender, EventArgs e)
    {
        if (Chktimein.Checked == true)
        {
            cbo_hrtin.Enabled = true;
            cbo_hrinto.Enabled = true;
            cbo_mintimein.Enabled = true;
            cbo_mininto.Enabled = true;
            cbointo.Enabled = true;
            cbo_in.Enabled = true;
            lblto.Enabled = true;
        }
        else
        {
            cbo_hrtin.Enabled = false;
            cbo_hrinto.Enabled = false;
            cbo_mintimein.Enabled = false;
            cbo_mininto.Enabled = false;
            cbointo.Enabled = false;
            cbo_in.Enabled = false;
            lblto.Enabled = false;
        }
    }
    protected void Chktimeout_CheckedChanged(object sender, EventArgs e)
    {
        if (Chktimeout.Checked == true)
        {
            cbo_hour2.Enabled = true;
            cbo_min2.Enabled = true;
            cbo_hours.Enabled = true;
            cbo_min.Enabled = true;
            cbo_sec.Enabled = true;
            cbo_sec2.Enabled = true;
            lbltoutto.Enabled = true;
        }
        else
        {
            cbo_hour2.Enabled = false;
            cbo_min2.Enabled = false;
            cbo_hours.Enabled = false;
            cbo_min.Enabled = false;
            cbo_sec.Enabled = false;
            cbo_sec2.Enabled = false;
            lbltoutto.Enabled = false;
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_rollno();
        load_studname();
    }
    protected void ddlDegree_SelectedIndexChanged1(object sender, EventArgs e)
    {
        //ddlBranch.Items.Clear();
        //con.Open();

        ////  cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddlDegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
        //string sqldegree = "";
        //sqldegree = "select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code= " + Session["collegecode"] + "";

        //if (ddlDegree.SelectedItem.Text != "Select")
        //{
        //    sqldegree = sqldegree + " and degree.course_id= " + ddlDegree.SelectedValue.ToString() + "";
        //}
        //SqlDataAdapter da = new SqlDataAdapter(sqldegree, con);
        //con.Close();
        //con.Open();
        //DataSet ds = new DataSet();
        //da.Fill(ds);
        //ddlBranch.DataSource = ds;
        //ddlBranch.DataValueField = "degree_code";
        //ddlBranch.DataTextField = "dept_name";
        //ddlBranch.DataBind();
        //ddlBranch.Items.Insert(0, "Select");
        //con.Close();
        //load_rollno();
        //load_studname();
    }
    protected void ddlBranch_SelectedIndexChanged1(object sender, EventArgs e)
    {
        load_rollno();
        load_studname();
        if (!Page.IsPostBack == false)
        {
            ddlSemYr.Items.Clear();
        }
        try
        {
            //if ((ddlBranch.SelectedIndex != 0) && (ddlBranch.SelectedIndex > 0))
            //{

            //}
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }
    protected void Txtentryto_TextChanged(object sender, EventArgs e)
    {

    }
    protected void cbo_mininto_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnsearch_Click(object sender, ImageClickEventArgs e)
    {
        load_click();
    }
    protected void imgpresent_Click(object sender, ImageClickEventArgs e)
    {
        //if (rdoinandout.Checked == true)
        //{
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'P-%'";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-P'";
        //    }
        //    else
        //    {
        //        Str = " and att like 'P-P'";
        //    }
        //}
        //else if (rdoinonly.Checked == true)
        //{
        //    Str = " and att like '%-P'";
        //}
        //else if (rdooutonly.Checked == true)
        //{
        //    Str = " and att like 'P-%'";
        //}
        //else if (rdoboth.Checked == true)
        //{
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'P-%'";
        //        ViewState["Bothpresent"] = "1";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-P'";
        //        ViewState["Bothpresent"] = "1";
        //    }
        //    else
        //    {
        //        Str = " and att like 'P-P'";
        //        ViewState["Bothpresent"] = "1";
        //    }
        //}
        //load_click();
    }
    protected void imgabsent_Click(object sender, ImageClickEventArgs e)
    {

        //if (rdoinandout.Checked == true)
        //{
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'A-%'";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-A'";
        //    }
        //    else
        //    {
        //        Str = " and att like 'A-A'";
        //    }
        //}
        //else if (rdoinonly.Checked == true)
        //{
        //    Str = " and att like '%-A'";
        //}
        //else if (rdooutonly.Checked == true)
        //{
        //    Str = " and att like 'A-%'";
        //}
        //else if (rdounreg.Checked == true)
        //{
        //    Str = " and att like 'A-A'";
        //    ViewState["unreg"] = 1;
        //}
        //else if (rdoboth.Checked == true)
        //{
        //    ViewState["BothAbsent"] = "2";
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'A-%'";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-A'";
        //    }
        //    else
        //    {
        //        Str = " and att like 'A-A'";
        //    }
        //}
        //load_click();

    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {


    }
    protected void imglate_Click(object sender, ImageClickEventArgs e)
    {
        //if (rdoinandout.Checked == true)
        //{
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'LA-%'";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-LA'";
        //    }
        //    else
        //    {
        //        Str = " and att like 'LA-LA'";
        //    }
        //}
        //else if (rdoinonly.Checked == true)
        //{
        //    Str = " and att like '%-LA'";
        //}
        //else if (rdooutonly.Checked == true)
        //{
        //    Str = " and att like 'LA-%'";
        //}
        //else if (rdoboth.Checked == true)
        //{
        //    ViewState["Bothod"] = "3";
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'LA-%'";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-LA'";
        //    }
        //    else
        //    {
        //        Str = " and att like 'LA-LA'";
        //    }
        //}
        //load_click();
    }
    protected void imgpermission_Click(object sender, ImageClickEventArgs e)
    {
        //if (rdoinandout.Checked == true)
        //{
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'PER-%'";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-PER'";
        //    }
        //    else
        //    {
        //        Str = " and att like 'PER-PER'";
        //    }
        //}
        //else if (rdoinonly.Checked == true)
        //{
        //    Str = " and att like '%-PER'";
        //}
        //else if (rdooutonly.Checked == true)
        //{
        //    Str = " and att like 'PER-%'";
        //}
        //else if (rdoboth.Checked == true)
        //{
        //    ViewState["Bothper"] = "4";
        //    if (rdb_morn.Checked == true)
        //    {
        //        Str = " and att like 'PER-%'";
        //    }
        //    else if (rdb_even.Checked == true)
        //    {
        //        Str = " and att like '%-PER'";
        //    }
        //    else
        //    {
        //        Str = " and att like 'PER-PER'";
        //    }
        //}
        //load_click();
    }

    protected void CheckBoxselect_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void imgontime_Click(object sender, ImageClickEventArgs e)
    {
        Generalflag = false;
        ontimeflag = true;
        load_click();
        // load_btnclick();
    }
    protected void cbofloorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_room();
        load_studname();
        load_rollno();

    }
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    public void filteration()
    {
        string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");

        if (orderby_Setting == "")
        {
            order_by_var = "";
        }
        else
        {
            if (orderby_Setting == "0")
            {
                order_by_var = " order by len(T.Roll_No),T.Roll_No";
            }
            else if (orderby_Setting == "2")
            {
                order_by_var = " ORDER BY T.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                order_by_var = " order by len(T.Roll_No),T.Roll_No,T.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                order_by_var = " order by len(T.Roll_No),T.Roll_No";
            }
            else if (orderby_Setting == "1,2")
            {
                order_by_var = " ORDER BY T.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                order_by_var = " order by len(T.Roll_No),T.Roll_No";
            }
        }

        if (order_by_var.Trim().ToString() == "")
        {
            order_by_var = " order by len(T.Roll_No),T.Roll_No";
        }
    }

    protected void rdchecked(object sender, EventArgs e)
    {
        fpbiomatric.Visible = false;
        lblpresent1.Text = "";
        lblpresent2.Text = "";
        lblabsent1.Text = "";
        lblabsent2.Text = "";
        imgabsent.Visible = false;
        lblheaderabsent1.Visible = false;
        lblheaderabsent2.Visible = false;
        lblabsent1.Visible = false;
        lblabsent2.Visible = false;
        imgpresent.Visible = false;
        imglate.Visible = false;
        lblmornlate.Visible = false;
        lblevenlate.Visible = false;
        lblnorec.Visible = false;
        imgper.Visible = false;
        lblmornper.Visible = false;
        lblevenper.Visible = false;
        lblpermission.Visible = false;
        lblpermission1.Visible = false;
        lblpresent1.Visible = false;
        lblpresent2.Visible = false;
        lbl_headermorn.Visible = false;
        lbl_headereven.Visible = false;
        lbllate.Visible = false;
        lbllate1.Visible = false;
        Chktimeout.Enabled = true;

        if (rdoinandout.Checked == true)
        {
            rdb_morn.Enabled = false;
            rdb_morn.Checked = false;
            rdb_even.Enabled = false;
            rdb_even.Checked = false;
            rdoboth1.Enabled = true;
            rdoboth1.Checked = true;
            TextBox1.Enabled = true;
            Chktimeout.Enabled = true;
        }
        else if (rdoinonly.Checked == true)
        {
            rdb_morn.Enabled = false;
            rdb_morn.Checked = false;
            rdb_even.Enabled = true;
            rdb_even.Checked = true;
            rdoboth1.Enabled = false;
            rdoboth1.Checked = false;
            TextBox1.Enabled = true;
            Chktimeout.Enabled = true;
        }
        else if (rdooutonly.Checked == true)
        {
            rdb_morn.Enabled = true;
            rdb_morn.Checked = true;
            rdb_even.Enabled = false;
            rdb_even.Checked = false;
            rdoboth1.Enabled = false;
            rdoboth1.Checked = false;
            TextBox1.Enabled = true;
            Chktimeout.Enabled = true;
        }
        else if (rdounreg.Checked == true)
        {
            cblsearch.Items[3].Attributes.Add("style", "display:none;");
            cblsearch.Items[4].Attributes.Add("style", "display:none;");
            cblsearch.Items[5].Attributes.Add("style", "display:none;");
            //lblmornlate.Visible = false;
            //lblevenlate.Visible = false;

            //imgpresent.Visible = false;
            //imgper.Visible = false;
            //lblmornper.Visible = false;
            //lblevenper.Visible = false;
            //lblpermission.Visible = false;
            //lblpermission1.Visible = false;
            //lblpresent1.Visible = false;
            //lblpresent2.Visible = false;
            //lbl_headermorn.Visible = false;
            //lbl_headereven.Visible = false;
            //lbllate.Visible = false;
            //lbllate1.Visible = false;
            //imglate.Visible = false;
            ////imgontime.Visible = false;
            ////lblontime.Visible = false;
            //imgabsent.Visible = true;
            //lblheaderabsent1.Visible = true;
            //lblheaderabsent2.Visible = true;
            rdb_morn.Enabled = false;
            rdb_morn.Checked = false;
            rdb_even.Enabled = false;
            rdb_even.Checked = false;
            rdoboth1.Enabled = false;
            rdoboth1.Checked = false;
            TextBox1.Enabled = false;
            Chktimeout.Enabled = true;
        }
        else
        {
            rdb_morn.Enabled = false;
            rdb_morn.Checked = false;
            rdb_even.Enabled = false;
            rdb_even.Checked = false;
            rdoboth1.Enabled = false;
            rdoboth1.Checked = false;
            TextBox1.Enabled = false;
            Chktimeout.Enabled = false;
        }
        //if (rbdailylog.Checked == true)
        //{
        //    ddlstud.Visible = true;
        //    Chktimeout.Enabled = false;
        //}

        //else if (rdounreg.Checked == true)
        //{
        //    cblsearch.Items[5].Attributes.Add("style", "display:none;");
        //    cblsearch.Items[6].Attributes.Add("style", "display:none;");
        //    cblsearch.Items[7].Attributes.Add("style", "display:none;");
        //    lblmornlate.Visible = false;
        //    lblevenlate.Visible = false;

        //    imgpresent.Visible = false;
        //    imgper.Visible = false;
        //    lblmornper.Visible = false;
        //    lblevenper.Visible = false;
        //    lblpermission.Visible = false;
        //    lblpermission1.Visible = false;
        //    lblpresent1.Visible = false;
        //    lblpresent2.Visible = false;

        //    lbl_headermorn.Visible = false;
        //    lbl_headereven.Visible = false;
        //    lbllate.Visible = false;
        //    lbllate1.Visible = false;
        //    imglate.Visible = false;
        //    lblabsent1.Visible = false;
        //    lblabsent2.Visible = false;
        //    imgabsent.Visible = false;
        //    lblheaderabsent1.Visible = false;
        //    lblheaderabsent2.Visible = false;
        //    Chktimeout.Enabled = true;
        //    //attfiltertype.Visible = false;
        //}
        //else
        //    Chktimeout.Enabled = true;
        //imgabsent.Visible = false;
        //lblheaderabsent1.Visible = false;
        //lblheaderabsent2.Visible = false;
        ////attfiltertype.Visible = false;
        //lblabsent1.Visible = false;
        //lblabsent2.Visible = false;
    }
    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            //string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            string query = "";
            string hostel_name = string.Empty;
            if (txthostelname.Text.ToString() != "--Select--")
            {
                if (cbl_hostelname.Items.Count > 0)
                    hostel_name = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                query = "select distinct college_code,collname from HM_HostelMaster hm,collinfo c where  hm.HostelMasterPK in('" + hostel_name + "') order by collname";//hm.CollegeCode=c.college_code and

            }
            //if (Convert.ToString(Cbo_HostelName.SelectedItem.Value) != "All")
            //{
            //    query = "select distinct college_code,collname from HM_HostelMaster hm,collinfo c where  hm.HostelMasterPK in('" + Convert.ToString(Cbo_HostelName.SelectedItem.Value) + "') order by collname";//hm.CollegeCode=c.college_code and
            //}
            else
            {
                query = "select distinct college_code,collname from HM_HostelMaster hm,collinfo c order by collname";// where hm.CollegeCode=c.college_code
            }

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_clg.DataSource = ds;
                cbl_clg.DataTextField = "collname";
                cbl_clg.DataValueField = "college_code";
                cbl_clg.DataBind();

                int count = 0;
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    cbl_clg.Items[i].Selected = true;
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        txt_college.Text = "College(" + Convert.ToString(cbl_clg.Items.Count) + ")";
                        cb_clg.Checked = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void cbl_clg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_college.Text = "--Select--";
            cb_clg.Checked = false;

            for (int i = 0; i < cbl_clg.Items.Count; i++)
            {
                if (cbl_clg.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_college.Text = "College(" + commcount.ToString() + ")";
                if (commcount == cbl_clg.Items.Count)
                {
                    cb_clg.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_clg_checkedchange(object sender, EventArgs e)
    {
        try
        {

            if (cb_clg.Checked == true)
            {
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    cbl_clg.Items[i].Selected = true;
                }
                txt_college.Text = "College(" + (cbl_clg.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    cbl_clg.Items[i].Selected = false;
                }
                txt_college.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }


    #region Column_Order
    protected void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string value = "";
            int index;
            // ItemList.Clear();
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblsearch.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(cblsearch.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblsearch.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblsearch.Items.Count; i++)
            {

                if (cblsearch.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblsearch.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }

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

            }
            tborder.Text = colname12;
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;

            }

        }
        catch (Exception ex)
        {
        }

    }
    #endregion

    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (SelectAll.Checked == true)
        {
            for (int i = 0; i < cbo_att.Items.Count; i++)
            {
                cbo_att.Items[i].Selected = true;
                TextBox1.Text = "Leave(" + (cbo_att.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbo_att.Items.Count; i++)
            {
                cbo_att.Items[i].Selected = false;
                //cbo_att.Items[i + 3].Selected = true;
                TextBox1.Text = "---Select---";
            }
        }

    }

    protected void cbo_att_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < cbo_att.Items.Count; i++)
        {
            if (cbo_att.Items[i].Selected == true)
            {

                value = cbo_att.Items[i].Text;
                code = cbo_att.Items[i].Value.ToString();
                batchcount = batchcount + 1;
                TextBox1.Text = "Leave(" + batchcount.ToString() + ")";
            }

        }

        if (batchcount == 0)
            TextBox1.Text = "---Select---";
        else
        {
            Label lbl = batchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = batchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(batchimg_Click);
        }
        batchcnt = batchcount;
    }


    public Label batchlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton batchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    public void batchimg_Click(object sender, ImageClickEventArgs e)
    {
        batchcnt = batchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cbo_att.Items[r].Selected = false;
        TextBox1.Text = "Leave(" + batchcnt.ToString() + ")";
        if (TextBox1.Text == "Leave(0)")
        {
            TextBox1.Text = "---Select---";
        }
    }
    protected void cb_batchyear_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batchyear, cbl_batchyear, txt_batchyr, Lblbatch.Text, "--Select--");
        load_rollno();
        load_studname();


    }
    protected void cbl_batchyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batchyear, cbl_batchyear, txt_batchyr, Lblbatch.Text);
        load_rollno();
        load_studname();


    }
    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, Lbldegree.Text, "--Select--");
        bindbranch();

    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, Lbldegree.Text);
        bindbranch();

    }
    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_branch, cbl_branch, txtbranch, LblBranch.Text, "--Select--");
        load_rollno();
        load_studname();
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_branch, cbl_branch, txtbranch, LblBranch.Text);
        load_rollno();
        load_studname();
    }
    protected void cb_hostelname_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_hostelname, cbl_hostelname, txthostelname, lblHostelname.Text, "--Select--");
        bindbuild();
        load_floorname();
        load_room();
        load_studname();
        load_rollno();
        load_studname();
        bindcollege();
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_hostelname, cbl_hostelname, txthostelname, lblHostelname.Text);
        bindbuild();
        load_floorname();
        load_room();
        load_studname();
        load_rollno();
        load_studname();
        bindcollege();
    }
    protected void cb_building_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_building, cbl_building, txt_building, lbl_building.Text, "--Select--");
        load_floorname();
        load_room();
        load_studname();
        load_rollno();
        load_studname();
        bindcollege();

    }
    protected void cbl_building_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_building, cbl_building, txt_building, lbl_building.Text);
        load_floorname();
        load_room();
        load_studname();
        load_rollno();
        load_studname();
        bindcollege();
    }
    protected void cb_floorName_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_floorName, cbl_floorName, txtfloorname, Label8.Text, "--Select--");
        load_room();
        load_studname();
        load_rollno();
    }
    protected void cbl_floorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_floorName, cbl_floorName, txtfloorname, Label8.Text);
        load_room();
        load_studname();
        load_rollno();
    }
    protected void cb_roomNo_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_roomNo, cbl_room_no, txtroom_no, lbl_romm.Text, "--Select--");
        load_rollno();
        load_studname();

    }
    protected void cbl_room_no_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_roomNo, cbl_room_no, txtroom_no, lbl_romm.Text);
        load_rollno();
        load_studname();

    }
    protected void cb_roolnum_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_roolnum, cbl_rollnum, txtrollnum, lbl_RollNo.Text, "--Select--");
    }
    protected void cbl_rollnum_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_roolnum, cbl_rollnum, txtrollnum, lbl_RollNo.Text);
    }
    protected void cb_studeName_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_studeName, cbl_studeName, txtstudename, lbl_studname.Text, "--Select--");
    }
    protected void cbl_studeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_studeName, cbl_studeName, txtstudename, lbl_studname.Text);
    }


    public void bindbranch()
    {
        cbl_branch.Items.Clear();
        con.Open();
        string degree = string.Empty;
        if (cbl_degree.Items.Count > 0)
            degree = rs.GetSelectedItemsValueAsString(cbl_degree);
        //  cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddlDegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
        string sqldegree = "";
        sqldegree = "select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code= " + Session["collegecode"] + "";

        if (txt_degree.Text != "--Select--")
        {
            sqldegree = sqldegree + " and degree.course_id in('" + degree + "')";
        }

        SqlDataAdapter da = new SqlDataAdapter(sqldegree, con);
        con.Close();
        con.Open();
        DataSet ds = new DataSet();
        da.Fill(ds);
        cbl_branch.DataSource = ds;
        cbl_branch.DataValueField = "degree_code";
        cbl_branch.DataTextField = "dept_name";
        cbl_branch.DataBind();
        if (cbl_branch.Items.Count > 0)
        {
            for (int row = 0; row < cbl_branch.Items.Count; row++)
            {
                cbl_branch.Items[row].Selected = true;
            }
            txtbranch.Text = "Branch(" + cbl_branch.Items.Count + ")";

        }

        con.Close();
        load_rollno();
        load_studname();

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
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
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
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
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
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }

    public void bindbuild()
    {
        try
        {

            cbl_building.Items.Clear();
            txt_building.Text = "---Select---";
            cb_building.Checked = false;
            string build = "";
            if (cbl_hostelname.Items.Count > 0)//mess code 14.10.15
            {
                for (int val = 0; val < cbl_hostelname.Items.Count; val++)
                {
                    if (cbl_hostelname.Items[val].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_hostelname.Items[val].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_hostelname.Items[val].Value);
                        }
                    }
                }
            }

            string bul = "";
            //

            string buil1 = d2.GetBuildingCode_inv(build);
            ds = d2.BindBuilding(buil1);
            //

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_building.DataSource = ds;
                cbl_building.DataTextField = "Building_Name";
                cbl_building.DataValueField = "code";
                cbl_building.DataBind();
                if (cbl_building.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_building.Items.Count; row++)
                    {
                        cbl_building.Items[row].Selected = true;
                    }
                    txt_building.Text = "Building Name(" + cbl_building.Items.Count + ")";

                }
            }

        }
        catch (Exception ex)
        {
        }
    }



    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(fpbiomatric, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }

    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 2;
        string degreedetails = "Daily Attendance Report";
        if (rdoinandout.Checked == true)
        {
            degreedetails = "Daily In And Out Attendance Report";
        }
        else if (rdoinonly.Checked == true)
        {
            degreedetails = "Daily In Only Attendance Report";
        }
        else if (rdooutonly.Checked == true)
        {
            degreedetails = "Daily Out Only Attendance Report";
        }
        else if (rdounreg.Checked == true)
        {
            degreedetails = "Daily Registered Attendance Report";
        }
        else if (rbdailylog.Checked == true)
        {
            degreedetails = "Daily Logs Attendance Report";
        }
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



        degreedetails = degreedetails + '@' + "Hostel Name: " + hostelname + "@ Date-From:" + Txtentryfrom.Text + "To:" + Txtentryto.Text + "";

        string pagename = "Biohostel_new.aspx";
        Printcontrol.loadspreaddetails(fpbiomatric, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
}

