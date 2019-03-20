using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Data.SqlClient;
using iTextSharp.text.pdf;
using FarPoint.Web.Spread;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BalAccess;
using DalConnection;
using System.Configuration;



public partial class HostelMod_StudentSearch : System.Web.UI.Page
{

    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl;
            img.Width = Unit.Percentage(75);
            img.Height = Unit.Percentage(70);
            //img.AlternateText = "No Image Found";
            return img;
        }
    }
    #region Field Declaration

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    string app = string.Empty;
    string usercode = string.Empty;
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string Rollflag1 = string.Empty;
    string Regflag1 = string.Empty;
    string Studflag1 = string.Empty;
    string rollno = string.Empty;
    string sql = "";
    DataSet dsmess = new DataSet();
    DataSet dsload2 = new DataSet();
    static string mm = "";
    string course_id = string.Empty;
    static string Hostelcode = "";
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet dsload = new DataSet();
    DataSet dsload1 = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds1 = new DataSet();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    static string query = "";
    static int chosedmode = 0;
    static int personmode = 0;
    int row = 0;
    string collegeCode = string.Empty;
    string hostelname = string.Empty;
    string buildname = string.Empty;
    string floorname = string.Empty;
    string roomname = string.Empty;

    string admno = string.Empty;
    string appno = string.Empty;
    string name = string.Empty;
    string qryrollnofilter = string.Empty;
    string qryregnofilter = string.Empty;
    string qryadmnofilter = string.Empty;
    string qryappnofilter = string.Empty;
    string qrynamefilter = string.Empty;
    Boolean Cellclick = false;
    bool check = false;
    string Photo = string.Empty;
    string Institutions = string.Empty;
    string collegeins = string.Empty;
    string Roll_No = string.Empty;
    string Name = string.Empty;
    string Hostel_Name = string.Empty;
    string Room = string.Empty;
    string PhoneNo = string.Empty;
    string fathername = string.Empty;
    string sex = string.Empty;
    string regno = string.Empty;
    string mailid = string.Empty;
    string dob = string.Empty;
    string course = string.Empty;
    string qry = string.Empty;
    string address = string.Empty;
    string floor = string.Empty;
    string bulidingname = string.Empty;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                usercode = Session["usercode"].ToString();
                collegecode1 = Session["collegecode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();

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
           
            if (!IsPostBack)
            {
                bindcollege();
                bindhostel();
                load_ddlrollno();
                load_floorname();
                load_hostelname();
                load_room();
                // block();
            }
        }
        catch
        { }
    }

    #region college
    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            //ddl_college.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ddl_college.DataSource = ds;
                //ddl_college.DataTextField = "collname";
                //ddl_college.DataValueField = "college_code";
                //ddl_college.DataBind();
                cbl_clg.DataSource = ds;
                cbl_clg.DataTextField = "collname";
                cbl_clg.DataValueField = "college_code";
                cbl_clg.DataBind();

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
    #endregion

    #region hostel name
    protected void bindhostel()
    {
        try
        {
            cbl_hostelname.Items.Clear();
            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + group_user + "'");
            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in (" + MessmasterFK + ") order by hostelname ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                //for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                //{
                //    cbl_hostelname.Items[i].Selected = true;
                //    if (cbl_hostelname.Items[i].Selected == true)
                //    {
                //        txt_hostelname.Text = "HostelName(" + Convert.ToString(cbl_hostelname.Items.Count) + ")";
                //        cb_hostelname.Checked = true;
                //    }
                //}
                mm = cbl_hostelname.SelectedValue;
            }
            else
            {
                cbl_hostelname.Items.Insert(0, "--Select--");
                txt_hostelname.Text = "--Select--";

            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_hostelname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txt_buildingname.Text = "--Select--";
            txt_floorname.Text = "--Select--";
            txt_roomname.Text = "--Select--";
            if (cb_hostelname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                if (cb_hostelname.Checked == true)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        if (cb_hostelname.Checked == true)
                        {
                            cbl_hostelname.Items[i].Selected = true;
                            txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
                            build1 = cbl_hostelname.Items[i].Value.ToString();
                            if (buildvalue1 == "")
                            {
                                buildvalue1 = build1;
                            }
                            else
                            {
                                buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                            }
                        }
                    }
                    Hostelcode = buildvalue1;
                    clgbuild(buildvalue1);
                }
            }
            else
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                    txt_hostelname.Text = "--Select--";
                    cbl_buildname.ClearSelection();
                    cbl_floorname.ClearSelection();
                    cbl_roomname.ClearSelection();
                    cb_buildname.Checked = false;
                    cb_floorname.Checked = false;
                    cb_roomname.Checked = false;
                }
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;
        cb_hostelname.Checked = false;
        int commcount = 0;
        string buildvalue = "";
        string build = "";
        txt_hostelname.Text = "--Select--";
        for (i = 0; i < cbl_hostelname.Items.Count; i++)
        {
            if (cbl_hostelname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_hostelname.Checked = false;
                ///new 22/08/15
                build = cbl_hostelname.Items[i].Value.ToString();
                if (buildvalue == "")
                {
                    buildvalue = build;
                }
                else
                {
                    buildvalue = buildvalue + "'" + "," + "'" + build;
                }
                clgbuild(buildvalue);
                Hostelcode = buildvalue;
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
    }
    #endregion

    #region buildname
    public void clgbuild(string hostelname)
    {
        try
        {
            cbl_buildname.Items.Clear();
            string bul = "";
            bul = d2.GetBuildingCode_inv(hostelname);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_buildname.DataSource = ds;
                cbl_buildname.DataTextField = "Building_Name";
                cbl_buildname.DataValueField = "code";
                cbl_buildname.DataBind();
            }
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                cbl_buildname.Items[i].Selected = true;
                txt_buildingname.Text = "Building(" + (cbl_buildname.Items.Count) + ")";
                cb_buildname.Checked = true;
            }
            string locbuild = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    string builname = cbl_buildname.Items[i].Text;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            clgfloor(locbuild);
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_buildname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_buildname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_buildname.Items.Count; i++)
                {
                    if (cb_buildname.Checked == true)
                    {
                        cbl_buildname.Items[i].Selected = true;
                        txt_buildingname.Text = "Building(" + (cbl_buildname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_buildname.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                clgfloor(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_buildname.Items.Count; i++)
                {
                    cbl_buildname.Items[i].Selected = false;
                    txt_buildingname.Text = "--Select--";
                    cbl_floorname.Items.Clear();
                    cb_floorname.Checked = false;
                    txt_floorname.Text = "--Select--";
                    txt_roomname.Text = "--Select--";
                    cb_roomname.Checked = false;
                    cbl_roomname.Items.Clear();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_buildname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_buildname.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    // txt_floorname.Text = "--Select--";
                    cb_floorname.Checked = true;
                    build = cbl_buildname.Items[i].Text.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            clgfloor(buildvalue);
            if (seatcount == cbl_buildname.Items.Count)
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
                cb_buildname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_buildingname.Text = "--Select--";
            }
            else
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region floor
    public void clgfloor(string buildname)
    {
        try
        {
            cbl_floorname.Items.Clear();
            //ds = d2.BindFloor_new(buildname);
            string itemname = "select distinct Floor_Name,FloorPK from Floor_Master where Building_Name in('" + buildname + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "FloorPK";
                cbl_floorname.DataBind();
            }
            else
            {
                txt_floorname.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                cbl_floorname.Items[i].Selected = true;
                cb_floorname.Checked = true;
            }
            string locfloor = "";
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                    string flrname = cbl_floorname.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
                    if (locfloor == "")
                    {
                        locfloor = flrname;
                    }
                    else
                    {
                        locfloor = locfloor + "'" + "," + "'" + flrname;
                    }
                }
            }
            clgroom(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_floorname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_floorname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";
                if (cb_buildname.Checked == true)
                {
                    for (int i = 0; i < cbl_buildname.Items.Count; i++)
                    {
                        build1 = cbl_buildname.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                if (cb_floorname.Checked == true)
                {
                    for (int j = 0; j < cbl_floorname.Items.Count; j++)
                    {
                        cbl_floorname.Items[j].Selected = true;
                        txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                        build2 = cbl_floorname.Items[j].Text.ToString();
                        if (buildvalue2 == "")
                        {
                            buildvalue2 = build2;
                        }
                        else
                        {
                            buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                        }
                    }
                }
                clgroom(buildvalue2, buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = false;
                    txt_floorname.Text = "--Select--";
                }
                cb_roomname.Checked = false;
                cbl_roomname.Items.Clear();
                txt_roomname.Text = "--Select--";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_floorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floorname.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    build1 = cbl_buildname.Items[i].Text.ToString();
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                    }
                }
            }
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build2 = cbl_floorname.Items[i].Text.ToString();
                    if (buildvalue2 == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }
                }
            }
            clgroom(buildvalue2, buildvalue1);
            if (seatcount == cbl_floorname.Items.Count)
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
                cb_floorname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_floorname.Text = "--Select--";
            }
            else
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region room
    public void clgroom(string floorname, string buildname)
    {
        try
        {
            cbl_roomname.Items.Clear();
            //ds = d2.BindRoom(floorname, buildname);changed at sairam 29.09.16//11.04.17 barath
            string itemname = "select Room_Name,Roompk from Room_Detail where Building_Name in('" + buildname + "') and floor_name in('" + floorname + "') order by (len(Room_Name)) asc,Room_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname.DataSource = ds;
                cbl_roomname.DataTextField = "Room_Name";
                cbl_roomname.DataValueField = "Roompk";
                cbl_roomname.DataBind();
            }
            else
            {
                txt_roomname.Text = "--Select--";
            }
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                cbl_roomname.Items[i].Selected = true;
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
                cb_roomname.Checked = true;
            }
            string room = "";
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    string flrname = cbl_roomname.Items[i].Text;
                    if (room == "")
                    {
                        room = flrname;
                    }
                    else
                    {
                        room = room + "'" + "," + "'" + flrname;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_roomname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomname.Checked == true)
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = true;
                }
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = false;
                }
                txt_roomname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblroomname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_roomname.Checked = false;
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount == cbl_roomname.Items.Count)
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
                cb_roomname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_roomname.Text = "--Select--";
            }
            else
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    # region rollno
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {

            WebService ws = new WebService();
            if (personmode == 0)
            {
                //student query
                if (chosedmode == 0)
                {

                    query = " select distinct r.Roll_No from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.Roll_No like '" + prefixText + "%' order by Roll_No ";

                }
                else if (chosedmode == 1)
                {

                    query = "select distinct r.Reg_No from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.Reg_No like '" + prefixText + "%' order by Reg_No";
                }

                else if (chosedmode == 2)
                {

                    query = "select distinct r.Roll_Admit from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.Roll_Admit like '" + prefixText + "%' order by Roll_Admit";

                }
                else if (chosedmode == 3)
                {
                    query = "select distinct r.App_No from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.App_No like '" + prefixText + "%'";

                }
                else if (chosedmode == 4)
                {

                    query = "select distinct r.Stud_Name from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.Stud_Name like '" + prefixText + "%'";

                }
                else if (chosedmode == 5)
                {
                    query = "select distinct h.id from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and h.id like '" + prefixText + "%'";
                }
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    protected void ddlrollno_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Error.Visible = false;
        //FpSpread1.Visible = false;
        //btnprintmaster.Visible = false;
        txtno.Text = "";
        lblnum.Text = ddlrollno.SelectedItem.ToString();
        

            switch (Convert.ToUInt32(ddlrollno.SelectedItem.Value))
            {
                case 0:
                    txtno.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txtno.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txtno.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txtno.Attributes.Add("placeholder", "App No");
                    chosedmode = 3;
                    break;
                case 4:
                    txtno.Attributes.Add("placeholder", "Name");
                    chosedmode = 4;
                    break;
                case 5:
                    txtno.Attributes.Add("placeholder", "Hostel Id");
                    chosedmode = 5;
                    break;
            }
     

       

    }

    public void load_ddlrollno()
    {
        try
        {
            System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("Roll No", "0");
            System.Web.UI.WebControls.ListItem lst2 = new System.Web.UI.WebControls.ListItem("Reg No", "1");
            System.Web.UI.WebControls.ListItem lst3 = new System.Web.UI.WebControls.ListItem("Admin No", "2");
            System.Web.UI.WebControls.ListItem lst4 = new System.Web.UI.WebControls.ListItem("App No", "3");
            System.Web.UI.WebControls.ListItem lst5 = new System.Web.UI.WebControls.ListItem("Name", "4");
            System.Web.UI.WebControls.ListItem lst51 = new System.Web.UI.WebControls.ListItem("Hostel Id", "5");

            //Roll Number or Reg Number or Admission No or Application Number
            ddlrollno.Items.Clear();
            string insqry1 = "select value from Master_Settings where settings='Roll No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                ddlrollno.Items.Add(lst1);
            }


            insqry1 = "select value from Master_Settings where settings='Register No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                ddlrollno.Items.Add(lst2);
            }

            insqry1 = "select value from Master_Settings where settings='Admission No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                ddlrollno.Items.Add(lst3);
            }

            insqry1 = "select value from Master_Settings where settings='Application No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //App Form Number - Application Number
                ddlrollno.Items.Add(lst4);

            }
            insqry1 = "select value from Master_Settings where settings='Hostel Id' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //App Form Number - Application Number
                ddlrollno.Items.Add(lst51);

            }

            if (ddlrollno.Items.Count == 0)
            {
                ddlrollno.Items.Add(lst1);
            }
            ddlrollno.Items.Add(lst5);
            switch (Convert.ToUInt32(ddlrollno.SelectedItem.Value))
            {
                case 0:
                    txtno.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txtno.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txtno.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txtno.Attributes.Add("placeholder", "App No");
                    chosedmode = 3;
                    break;
                case 5:
                    txtno.Attributes.Add("placeholder", "Hostel Id");
                    chosedmode = 5;
                    break;
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
    }

    #endregion

    #region date
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtFromDate1_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtToDate1_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtFromDate2_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtToDate2_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtdate_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region delete and update
    protected void delete()
    {
        try
        {
            //int activerow = Fpload1.ActiveSheetView.ActiveRow;
            //int activecol = Fpload1.ActiveSheetView.ActiveColumn;
            //string sqld = "";
            //int query = 0;
            //Gym_Name = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 1].Text);
            //Gym_Acroynm = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 2].Text);
            //Gym_PK = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 3].Text);

            //if (txt_name.Text != "" && txt_acry.Text != "")
            //{
            //    nam = Convert.ToString(txt_name.Text);
            //    acr = Convert.ToString(txt_acry.Text);
            //    sqld = "delete from HM_GymMaster where GymAcr='" + acr + "' and GymName='" + nam + "'";
            //    query = d2.update_method_wo_parameter(sqld, "TEXT");
            //    if (query != 0)
            //    {
            //        Divdelete.Visible = true;
            //        Label4.Visible = true;
            //        Label4.Text = "Deleted Successfully";
            //        Div3.Visible = false;


            //    }
            //}


        }
        catch (Exception ex) { }
    }


    protected void btnupdate_Click(object sender, EventArgs e)
    {

        try
        {
            //string sqlu = string.Empty;

            //if (txt_name.Text != "" && txt_acry.Text != "")
            //{
            //    nam = Convert.ToString(txt_name.Text);
            //    acr = Convert.ToString(txt_acry.Text);

            //    sqlu = "update HM_GymMaster set GymAcr='" + acr + "', GymName='" + nam + "' where GymPk='" + Gym_PK + "'";
            //    query = d2.update_method_wo_parameter(sqlu, "TEXT");
            //    if (query != 0)
            //    {
            //        Div1.Visible = true;
            //        Label3.Visible = true;
            //        Label3.Text = "Updated Successfully";

            //    }
            //}
            //else
            //{

            //    Div1.Visible = false;
            //    imgAlert.Visible = true;
            //    lblalerterr.Text = "No Record Found!";
            //}
        }
        catch
        {
        }
    }


    #endregion

    #region go
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet studentsearch = new DataSet();
            studentsearch = student();

            if (studentsearch.Tables.Count > 0 && studentsearch.Tables[0].Rows.Count > 0)
            {
                loadspreaddetails(studentsearch);

            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch
        {
        }
    }

    protected void btngo1_Click(object sender, EventArgs e)
    {
        try
        {
            //DataSet hostel = new DataSet();

            //{
            //    if (row >= 0)
            //    {
            //        Roll_No = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
            //        Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
            //        Hostel_Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
            //        string Hostel_code = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Tag);
            //        Room = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);



            //        qry = "select hr.APP_No,R.Roll_No,R.Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,case when a.sex='0' then 'male' when a.sex='1' then 'Female'  else 'transder' end as sex ,a.parent_addressP ,a.parent_name as father_name,a.StuPer_Id as mail_id,BuildingFK,(select building_name from Building_Master where BuildingFK=code)as building_name,FloorFK,(select floor_name from Floor_Master where floorpk=FloorFK)as floor_name,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and h.HostelMasterPK in ('" + Hostel_code + "') and  R.Roll_No='" + Convert.ToString(Roll_No) + "'";
            //        hostel.Clear();
            //        hostel = d2.select_method_wo_parameter(qry, "Text");
            //        if (hostel.Tables[0].Rows.Count > 0)
            //        {
            //            course = Convert.ToString(hostel.Tables[0].Rows[row]["Degree"]).Trim();
            //        }
            //        Lblinfo.Visible = true;
            //        lblstud3.Visible = true;
            //        lblstudtxt3.Visible = true;
            //        lblroll3.Visible = true;
            //        lblrolltxt3.Visible = true;
            //        lblcourse3.Visible = true;
            //        lblcoursetxt3.Visible = true;
            //        lblhostel3.Visible = true;
            //        lblhosteltxt3.Visible = true;
            //        lblroom3.Visible = true;
            //        lblroomtxt3.Visible = true;


            //        lblstudtxt3.Text = Name;
            //        lblrolltxt3.Text = Roll_No;
            //        lblhosteltxt3.Text = Hostel_Name;
            //        lblcoursetxt3.Text = course;
            //        lblroomtxt3.Text = Room;
            //    }
            //  DataSet hostelattendance = new DataSet();
            // hostelattendance = Hostel();

            // if (hostelattendance.Tables.Count > 0 && hostelattendance.Tables[0].Rows.Count > 0)
            // {
            // loadhostelspread3(hostelattendance);
            loadhostelspread3();

            //}
            //  else
            // {
           // alertpopwindow.Visible = true;
            //lblalerterr.Text = "No Record Found!";
            // }
            // }
        }
        catch
        {
        }
    }

    protected void btngo2_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsmess = new DataSet();

            //{
            //if (row >= 0)
            //{
            //    Roll_No = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
            //    Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
            //    Hostel_Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
            //    string Hostel_code = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Tag);


            //    qry = "select hr.APP_No,R.Roll_No,R.Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,case when a.sex='0' then 'male' when a.sex='1' then 'Female'  else 'transder' end as sex ,a.parent_addressP ,a.parent_name as father_name,a.StuPer_Id as mail_id,BuildingFK,(select building_name from Building_Master where BuildingFK=code)as building_name,FloorFK,(select floor_name from Floor_Master where floorpk=FloorFK)as floor_name,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and h.HostelMasterPK in ('" + Hostel_code + "') and  R.Roll_No='" + Convert.ToString(Roll_No) + "'";
            //    dsmess.Clear();
            //    dsmess = d2.select_method_wo_parameter(qry, "Text");
            //    if (dsmess.Tables[0].Rows.Count > 0)
            //    {
            //        course = Convert.ToString(dsmess.Tables[0].Rows[row]["Degree"]).Trim();
            //    }
            //    Label15.Visible = true;
            //    lblstud4.Visible = true;
            //    lblstudtxt4.Visible = true;
            //    lblroll4.Visible = true;
            //    lblrolltxt4.Visible = true;
            //    lblcourse4.Visible = true;
            //    lblcoursetxt4.Visible = true;
            //    lblhostel4.Visible = true;
            //    lblhosteltxt4.Visible = true;



            //    lblstudtxt4.Text = Name;
            //    lblrolltxt4.Text = Roll_No;
            //    lblhosteltxt4.Text = Hostel_Name;
            //    lblcoursetxt4.Text = course;

            //}
            DataSet messattendance = new DataSet();
            messattendance = Mess();

            //if (messattendance.Tables.Count > 0 && messattendance.Tables[0].Rows.Count > 0)
            // {
            loadmessdetails();

            //}
            //else
            //{
            //alertpopwindow.Visible = true;
            // lblalerterr.Text = "No Record Found!";
            //}
            //}
        }
        catch
        {
        }
    }

    protected void btngo3_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsout = new DataSet();

           // {
                //if (row >= 0)
                //{
                //    Roll_No = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                //    Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
                //    Hostel_Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
                //    string Hostel_code = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Tag);


                //    qry = "select hr.APP_No,R.Roll_No,R.Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,case when a.sex='0' then 'male' when a.sex='1' then 'Female'  else 'transder' end as sex ,a.parent_addressP ,a.parent_name as father_name,a.StuPer_Id as mail_id,BuildingFK,(select building_name from Building_Master where BuildingFK=code)as building_name,FloorFK,(select floor_name from Floor_Master where floorpk=FloorFK)as floor_name,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and h.HostelMasterPK in ('" + Hostel_code + "') and  R.Roll_No='" + Convert.ToString(Roll_No) + "'";
                //    dsout.Clear();
                //    dsout = d2.select_method_wo_parameter(qry, "Text");
                //    if (dsout.Tables[0].Rows.Count > 0)
                //    {
                //        course = Convert.ToString(dsout.Tables[0].Rows[row]["Degree"]).Trim();
                //    }

                //    lblstud5.Visible = true;
                //    lblstudtxt5.Visible = true;
                //    lblroll5.Visible = true;
                //    lblrolltxt5.Visible = true;
                //    lblcourse5.Visible = true;
                //    lblcoursetxt5.Visible = true;
                //    lblhost5.Visible = true;
                //    lblhosttxt5.Visible = true;



                //    lblstudtxt5.Text = Name;
                //    lblrolltxt5.Text = Roll_No;
                //    lblhosttxt5.Text = Hostel_Name;
                //    lblcoursetxt5.Text = course;

                //}
                DataSet inout = new DataSet();
                inout = inandout();

                //if (inout.Tables.Count > 0 && inout.Tables[0].Rows.Count > 0)
               // {
                    loadindetails(inout);

               // }
               // else
               // {
                  //  alertpopwindow.Visible = true;
                   // lblalerterr.Text = "No Record Found!";
              //  }
           // }
        }
        catch
        {
        }
    }
    #endregion

    #region spread

    protected void Cell_Click1(object sender, EventArgs e)
    {

        try
        {
            check = true;
        }
        catch
        {
        }
    }

    protected void Fpspread_render(object sender, EventArgs e)
    {

        try
        {

            if (check == true)
            {
                DataSet basicdetails = new DataSet();
                DataSet information = new DataSet();
                DataSet transfer = new DataSet();
                string activerow = Fpload1.ActiveSheetView.ActiveRow.ToString();
                string activecol = Fpload1.ActiveSheetView.ActiveColumn.ToString();
                int col = 0;
                int.TryParse(activecol, out col);

                int.TryParse(activerow, out row);

                if (col == 8)
                {
                    popwindow1.Visible = true;
                    if (row >= 0)
                    {
                        Institutions = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 2].Text);
                        Roll_No = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                        Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
                        Hostel_Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
                        string Hostel_code = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Tag);
                        Room = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);
                        PhoneNo = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 7].Text);


                        qry = "select hr.APP_No,R.Roll_No,R.Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,case when a.sex='0' then 'male' when a.sex='1' then 'Female'  else 'transder' end as sex ,a.parent_addressP ,a.parent_name as father_name,a.StuPer_Id as mail_id,BuildingFK,(select building_name from Building_Master where BuildingFK=code)as building_name,FloorFK,(select floor_name from Floor_Master where floorpk=FloorFK)as floor_name,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and h.HostelMasterPK in ('" + Hostel_code + "') and  R.Roll_No='" + Convert.ToString(Roll_No) + "'";
                        basicdetails.Clear();
                        basicdetails = d2.select_method_wo_parameter(qry, "Text");
                        if (basicdetails.Tables[0].Rows.Count > 0)
                        {
                            fathername = Convert.ToString(basicdetails.Tables[0].Rows[row]["father_name"]).Trim();
                            sex = Convert.ToString(basicdetails.Tables[0].Rows[row]["sex"]).Trim();
                            regno = Convert.ToString(basicdetails.Tables[0].Rows[row]["Reg_No"]).Trim();
                            mailid = Convert.ToString(basicdetails.Tables[0].Rows[row]["mail_id"]).Trim();
                            dob = Convert.ToString(basicdetails.Tables[0].Rows[row]["DOB"]).Trim();
                            course = Convert.ToString(basicdetails.Tables[0].Rows[row]["Degree"]).Trim();
                            address = Convert.ToString(basicdetails.Tables[0].Rows[row]["parent_addressP"]).Trim();
                            image3.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + Roll_No;
                            floor = Convert.ToString(basicdetails.Tables[0].Rows[row]["floor_name"]).Trim();
                            bulidingname = Convert.ToString(basicdetails.Tables[0].Rows[row]["building_name"]).Trim();
                        }

                        lbl_name1.Enabled = true;
                        lblnametext1.Enabled = true;
                        lbl_instut1.Enabled = true;
                        lblintitutext1.Enabled = true;
                        lblroll1.Enabled = true;
                        lblrolltxt1.Enabled = true;
                        lblhostel1.Enabled = true;
                        lblhosteltxt1.Enabled = true;
                        lblphne.Enabled = true;
                        lblphnetxt1.Enabled = true;
                        lblroom.Enabled = true;
                        lblroomtxt1.Enabled = true;




                        lblnametext1.Text = Name;
                        lblintitutext1.Text = Institutions;
                        lblrolltxt1.Text = Roll_No;
                        lblhosteltxt1.Text = Hostel_Name;
                        lblphnetxt1.Text = PhoneNo;
                        lblroomtxt1.Text = Room;
                        lblregtxt1.Text = Name;
                        lblfathertxt1.Text = fathername;
                        lblcoursetxt1.Text = course;
                        lblgendertxt.Text = sex;
                        lbldobtxt1.Text = dob;
                        lblemailtxt1.Text = mailid;
                        lbladdress1.Text = address;
                        image3.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + Roll_No;
                        lblfloor1.Text = floor;
                        lblblocktxt1.Text = bulidingname;

                    }
                }
                else if (activecol.Trim() == "9")
                {
                    popwindow2.Visible = true;
                    if (row >= 0)
                    {

                        Roll_No = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                        Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
                        Hostel_Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
                        string Hostel_code = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Tag);
                        PhoneNo = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 7].Text);



                        qry = "select hr.APP_No,R.Roll_No,R.Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,case when a.sex='0' then 'male' when a.sex='1' then 'Female'  else 'transder' end as sex ,a.parent_addressP ,a.parent_name as father_name,a.StuPer_Id as mail_id,BuildingFK,(select building_name from Building_Master where BuildingFK=code)as building_name,FloorFK,(select floor_name from Floor_Master where floorpk=FloorFK)as floor_name,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and h.HostelMasterPK in ('" + Hostel_code + "') and  R.Roll_No='" + Convert.ToString(Roll_No) + "'";
                        information.Clear();
                        information = d2.select_method_wo_parameter(qry, "Text");
                        if (information.Tables[0].Rows.Count > 0)
                        {
                            course = Convert.ToString(information.Tables[0].Rows[row]["Degree"]).Trim();
                        }

                        Lblstud2.Enabled = true;
                        lblstudtxt2.Enabled = true;
                        lblroll2.Enabled = true;
                        lblrolltxt2.Enabled = true;
                        lblcourse2.Enabled = true;
                        lblcoursetxt2.Enabled = true;
                        lblhostel2.Enabled = true;
                        lblhosteltxt2.Enabled = true;
                        lblphne2.Enabled = true;
                        lblphnetxt2.Enabled = true;

                        lblstudtxt2.Text = Name;
                        lblrolltxt2.Text = Roll_No;
                        lblhosteltxt2.Text = Hostel_Name;
                        lblphnetxt2.Text = PhoneNo;
                        lblcoursetxt2.Text = course;

                        DataSet studentDetails = new DataSet();
                        studentDetails = studinfor();

                        if (studentDetails.Tables.Count > 0 && studentDetails.Tables[0].Rows.Count > 0)
                        {
                            loadspread2(studentDetails);

                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Found!";
                        }

                    }
                }
                else if (activecol.Trim() == "10")
                {
                    popwindow3.Visible = true;
                    txtFromDate.Text = "";
                    txtToDate.Text = "";
                    FpSpread1.Visible = false;
                    DataSet hostel = new DataSet();

                    {
                        if (row >= 0)
                        {
                            Roll_No = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                            Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
                            Hostel_Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
                            string Hostel_code = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Tag);
                            Room = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);



                            qry = "select hr.APP_No,R.Roll_No,R.Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,case when a.sex='0' then 'male' when a.sex='1' then 'Female'  else 'transder' end as sex ,a.parent_addressP ,a.parent_name as father_name,a.StuPer_Id as mail_id,BuildingFK,(select building_name from Building_Master where BuildingFK=code)as building_name,FloorFK,(select floor_name from Floor_Master where floorpk=FloorFK)as floor_name,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and h.HostelMasterPK in ('" + Hostel_code + "') and  R.Roll_No='" + Convert.ToString(Roll_No) + "'";
                            hostel.Clear();
                            hostel = d2.select_method_wo_parameter(qry, "Text");
                            if (hostel.Tables[0].Rows.Count > 0)
                            {
                                course = Convert.ToString(hostel.Tables[0].Rows[row]["Degree"]).Trim();
                            }
                            Lblinfo.Visible = true;
                            lblstud3.Visible = true;
                            lblstudtxt3.Visible = true;
                            lblroll3.Visible = true;
                            lblrolltxt3.Visible = true;
                            lblcourse3.Visible = true;
                            lblcoursetxt3.Visible = true;
                            lblhostel3.Visible = true;
                            lblhosteltxt3.Visible = true;
                            lblroom3.Visible = true;
                            lblroomtxt3.Visible = true;


                            lblstudtxt3.Text = Name;
                            lblrolltxt3.Text = Roll_No;
                            lblhosteltxt3.Text = Hostel_Name;
                            lblcoursetxt3.Text = course;
                            lblroomtxt3.Text = Room;
                            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        }
                    }
                }
                else if (activecol.Trim() == "11")
                {
                    popwindow4.Visible = true;
                    txtFromDate1.Text = "";
                    txtToDate1.Text = "";
                    FpSpread2.Visible = false;
                    if (row >= 0)
                    {
                        Roll_No = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                        Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
                        Hostel_Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
                        string Hostel_code = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Tag);


                        qry = "select hr.APP_No,R.Roll_No,R.Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,case when a.sex='0' then 'male' when a.sex='1' then 'Female'  else 'transder' end as sex ,a.parent_addressP ,a.parent_name as father_name,a.StuPer_Id as mail_id,BuildingFK,(select building_name from Building_Master where BuildingFK=code)as building_name,FloorFK,(select floor_name from Floor_Master where floorpk=FloorFK)as floor_name,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and h.HostelMasterPK in ('" + Hostel_code + "') and  R.Roll_No='" + Convert.ToString(Roll_No) + "'";
                        dsmess.Clear();
                        dsmess = d2.select_method_wo_parameter(qry, "Text");
                        if (dsmess.Tables[0].Rows.Count > 0)
                        {
                            course = Convert.ToString(dsmess.Tables[0].Rows[row]["Degree"]).Trim();
                        }
                        Label15.Visible = true;
                        lblstud4.Visible = true;
                        lblstudtxt4.Visible = true;
                        lblroll4.Visible = true;
                        lblrolltxt4.Visible = true;
                        lblcourse4.Visible = true;
                        lblcoursetxt4.Visible = true;
                        lblhostel4.Visible = true;
                        lblhosteltxt4.Visible = true;



                        lblstudtxt4.Text = Name;
                        lblrolltxt4.Text = Roll_No;
                        lblhosteltxt4.Text = Hostel_Name;
                        lblcoursetxt4.Text = course;
                        txtFromDate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtToDate1.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    }

                }
                else if (activecol.Trim() == "12")
                {
                    popwindow5.Visible = true;
                    txtFromDate2.Text = "";
                    txtToDate2.Text = "";
                    FpSpread3.Visible = false;
                    DataSet dsout = new DataSet();
                    if (row >= 0)
                    {
                        Roll_No = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 3].Text);
                        Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
                        Hostel_Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Text);
                        string Hostel_code = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 5].Tag);


                        qry = "select hr.APP_No,R.Roll_No,R.Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,case when a.sex='0' then 'male' when a.sex='1' then 'Female'  else 'transder' end as sex ,a.parent_addressP ,a.parent_name as father_name,a.StuPer_Id as mail_id,BuildingFK,(select building_name from Building_Master where BuildingFK=code)as building_name,FloorFK,(select floor_name from Floor_Master where floorpk=FloorFK)as floor_name,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and h.HostelMasterPK in ('" + Hostel_code + "') and  R.Roll_No='" + Convert.ToString(Roll_No) + "'";
                        dsout.Clear();
                        dsout = d2.select_method_wo_parameter(qry, "Text");
                        if (dsout.Tables[0].Rows.Count > 0)
                        {
                            course = Convert.ToString(dsout.Tables[0].Rows[row]["Degree"]).Trim();
                        }
                        Label20.Visible = true;
                        lblstud5.Visible = true;
                        lblstudtxt5.Visible = true;
                        lblroll5.Visible = true;
                        lblrolltxt5.Visible = true;
                        lblcourse5.Visible = true;
                        lblcoursetxt5.Visible = true;
                        lblhost5.Visible = true;
                        lblhosttxt5.Visible = true;



                        lblstudtxt5.Text = Name;
                        lblrolltxt5.Text = Roll_No;
                        lblhosttxt5.Text = Hostel_Name;
                        lblcoursetxt5.Text = course;
                        txtFromDate2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtToDate2.Text = DateTime.Now.ToString("dd/MM/yyyy");

                    }
                }
                else if (activecol.Trim() == "13")
                {
                    popwindow6.Visible = true;
                    Label9.Text = "Confirm Room Transfer";
                    lblhostel6.Visible = true;
                    Cbo_HostelName.Visible = true;
                    Label14.Visible = true;
                    cbofloorname.Visible = true;
                    lblroom6.Visible = true;
                    Cbo_Room.Visible = true;
                    lblreason.Visible = false;
                    txt_reason.Visible = false;
                    Label13.Text = "Date";
                    Lbl.Visible = false;
                    lblblock6.Visible = true;
                    ddlblock6.Visible = true;
                    btn_save1.Text = "RoomTransfer";
                    txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else if (activecol.Trim() == "14")
                {
                    popwindow6.Visible = true;
                    Label9.Text = "Vacated";
                    lblhostel6.Visible = false;
                    Cbo_HostelName.Visible = false;
                    Label14.Visible = false;
                    cbofloorname.Visible = false;
                    lblroom6.Visible = false;
                    Cbo_Room.Visible = false;
                    Label13.Text = "Date";
                    lblreason.Visible = true;
                    txt_reason.Visible = true;
                    Lbl.Visible = false;
                    lblblock6.Visible = false;
                    ddlblock6.Visible = false;
                    btn_save1.Text = "Yes";
                    txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                }

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Fpload_OnButtonCommand(object sender, EventArgs e)
    {
        try
        {
            student();

        }
        catch
        {
        }
    }
    #endregion

    #region student
    private DataSet student()
    {
        DataSet room = new DataSet();
        string regno = string.Empty;
        try
        {
            if (cbl_clg.Items.Count > 0)
                collegeCode = Convert.ToString(getCblSelectedValue(cbl_clg).ToUpper());
            if (cbl_hostelname.Items.Count > 0)
                hostelname = Convert.ToString(getCblSelectedValue(cbl_hostelname).ToUpper());
            if (cbl_buildname.Items.Count > 0)
                buildname = Convert.ToString(getCblSelectedValue(cbl_buildname).ToUpper());
            if (cbl_floorname.Items.Count > 0)
                floorname = Convert.ToString(getCblSelectedValue(cbl_floorname).ToUpper());
            if (cbl_roomname.Items.Count > 0)
                roomname = Convert.ToString(getCblSelectedValue(cbl_roomname).ToUpper());
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(hostelname) && !string.IsNullOrEmpty(buildname) && !string.IsNullOrEmpty(floorname) && !string.IsNullOrEmpty(roomname))
            {
                if (ddlrollno.SelectedItem.Text == "Roll No")
                {
                    if (ddlrollno.Items.Count > 0)
                        rollno = Convert.ToString(txtno.Text.Trim());
                    qryrollnofilter = "and R.Roll_No='" + rollno + "'";
                }
                if (ddlrollno.SelectedItem.Text =="Reg No")
                {
                    if (ddlrollno.Items.Count > 0)
                        regno = Convert.ToString(txtno.Text.Trim());
                    qryregnofilter = "and r.Reg_no='" + regno + "'";
                }
                if (ddlrollno.SelectedItem.Text == "Admin No")
                {
                    if (ddlrollno.Items.Count > 0)
                        admno = Convert.ToString(txtno.Text.Trim());
                    qryadmnofilter = "and r.Roll_Admit='" + admno + "'";
                }
                if (ddlrollno.SelectedItem.Text == "App No")
                {
                    if (ddlrollno.Items.Count > 0)
                        appno = Convert.ToString(txtno.Text.Trim());
                    qryappnofilter = "and r.App_No='" + appno + "'";
                }
                if (ddlrollno.SelectedItem.Text == "Name")
                {
                    if (ddlrollno.Items.Count > 0)
                        name = Convert.ToString(txtno.Text.Trim());

                    qrynamefilter = "and r.Stud_Name='" + name + "'";
                }
                if (ddlrollno.SelectedItem.Text == "Hostel Id")
                {
                    if (ddlrollno.Items.Count > 0)
                        name = Convert.ToString(txtno.Text.Trim());

                    qrynamefilter = "and hr.id='" + name + "'";
                }

                if (ddlrollno.SelectedItem.Text == "Roll No")
                {
                    if (rollno == "")
                    {
                        sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No  and rd.RoomPK=hr.RoomFK and  a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "') and r.college_code in ('" + collegeCode + "')and h.HostelMasterPK in ('" + hostelname + "') and FloorFK in('" + floorname + "') and BuildingFK in ('" + buildname + "') and ISNULL(IsVacated,'0')=0   order by R.Roll_No";
                    }
                    else if (rollno != "")
                    {
                        sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "')  and FloorFK in('" + floorname + "') and r.college_code in ('" + collegeCode + "') and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "') " + qryrollnofilter + "";
                    }
                    room.Clear();
                    room = d2.select_method_wo_parameter(sql, "Text");
                }
                else if (ddlrollno.SelectedItem.Text == "Reg No")
                {
                    if (regno == "")
                    {
                        sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "') and r.college_code in ('" + collegeCode + "')and h.HostelMasterPK in ('" + hostelname + "') and FloorFK in('" + floorname + "') and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "') order by R.Reg_No";
                    }
                    else if (regno != "")
                    {
                        sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "')  and FloorFK in('" + floorname + "') and r.college_code in ('" + collegeCode + "') and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "') " + qryregnofilter + "";
                    }
                    room.Clear();
                    room = d2.select_method_wo_parameter(sql, "Text");
                }
                else if (ddlrollno.SelectedItem.Text == "Admin No")
                {
                    if (admno == "")
                    {
                        sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No  and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "') and r.college_code in ('" + collegeCode + "')and h.HostelMasterPK in ('" + hostelname + "') and FloorFK in('" + floorname + "') and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "')  order by R.Roll_Admit";
                    }
                    else if (admno != "")
                    {
                        sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "')  and FloorFK in('" + floorname + "') and r.college_code in ('" + collegeCode + "') and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "')" + qryadmnofilter + "";
                    }
                    room.Clear();
                    room = d2.select_method_wo_parameter(sql, "Text");
                }
                else if (ddlrollno.SelectedItem.Text == "App No")
                {
                    if (appno == "")
                    {
                        sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "') and r.college_code in ('" + collegeCode + "')and h.HostelMasterPK in ('" + hostelname + "') and FloorFK in('" + floorname + "')  and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "') order by R.App_No";
                    }
                    else if (appno != "")
                    {
                        sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "')  and FloorFK in('" + floorname + "') and r.college_code in ('" + collegeCode + "') and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "') " + qryappnofilter + "";
                    }
                    room.Clear();
                    room = d2.select_method_wo_parameter(sql, "Text");
                }
                else if (ddlrollno.SelectedItem.Text == "Name")
                {
                    if (name == "")
                    {
                        sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "') and r.college_code in ('" + collegeCode + "')and h.HostelMasterPK in ('" + hostelname + "') and FloorFK in('" + floorname + "')  and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "')  order by R.Stud_Name";
                    }
                    else if (name != "")
                    {
                        sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No  and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "')  and FloorFK in('" + floorname + "') and r.college_code in ('" + collegeCode + "') and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "') " + qrynamefilter + "";
                    }
                }
                    else if (ddlrollno.SelectedItem.Text == "Hostel Id")
                    {
                        if (name == "")
                        {
                            sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,R.Stud_Name,hr.id,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "') and r.college_code in ('" + collegeCode + "')and h.HostelMasterPK in ('" + hostelname + "') and FloorFK in('" + floorname + "')  and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "')  order by R.Stud_Name";
                        }
                        else if (name != "")
                        {
                            sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.college_code,r.Roll_Admit,rd.room_name,hr.id,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,Room_Detail rd,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and rd.RoomPK=hr.RoomFK and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and RoomFK in('" + roomname + "')  and FloorFK in('" + floorname + "') and r.college_code in ('" + collegeCode + "') and ISNULL(IsVacated,'0')=0  and BuildingFK in ('" + buildname + "') " + qrynamefilter + "";//and h.HostelMasterPK in ('" + hostelname + "')
                        }
                    }


                    room.Clear();
                    room = d2.select_method_wo_parameter(sql, "Text");
                
            }
        }
        catch (Exception ex)
        {
        }
        return room;
    }

    private void loadspreaddetails(DataSet ds)
    {
        try
        {


            loadspreadHeader(ds);
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;


            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {


                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {

                    Fpload1.Sheets[0].RowCount++;
                    sno++;


                    MyImg studImage = new MyImg();//img

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        studImage.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]);

                    }


                    collegeins = Convert.ToString(ds.Tables[0].Rows[row]["college_code"]).Trim();
                    Institutions = d2.GetFunction("select collname from collinfo where college_code='" + Convert.ToString(collegeins) + "'");
                    Roll_No = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]).Trim();
                    Name = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]).Trim();
                    Hostel_Name = Convert.ToString(ds.Tables[0].Rows[row]["HostelName"]).Trim();
                  //  Room = Convert.ToString(ds.Tables[0].Rows[row]["RoomFK"]).Trim();
                    Room = Convert.ToString(ds.Tables[0].Rows[row]["Room_Name"]).Trim();
                    PhoneNo = Convert.ToString(ds.Tables[0].Rows[row]["Student_Mobile"]).Trim();
                    FarPoint.Web.Spread.ImageCellType img1 = new FarPoint.Web.Spread.ImageCellType();



                    FarPoint.Web.Spread.ImageCellType imagecell1 = new FarPoint.Web.Spread.ImageCellType();
                    FarPoint.Web.Spread.ImageCellType imagecell2 = new FarPoint.Web.Spread.ImageCellType();
                    FarPoint.Web.Spread.ImageCellType imagecell3 = new FarPoint.Web.Spread.ImageCellType();
                    FarPoint.Web.Spread.ImageCellType imagecell4 = new FarPoint.Web.Spread.ImageCellType();
                    FarPoint.Web.Spread.ImageCellType imagecell5 = new FarPoint.Web.Spread.ImageCellType();
                    FarPoint.Web.Spread.ImageCellType imagecell6 = new FarPoint.Web.Spread.ImageCellType();
                    MyImg studImages = new MyImg();//img
                    studImages.ImageUrl = "~/image/stuinfo.png";
                    MyImg studImageuni = new MyImg();//img
                    studImageuni.ImageUrl = "~/image/university1.jpg";
                    MyImg studImagesatt = new MyImg();//img
                    studImagesatt.ImageUrl = "~/image/hostel attendance1.jpg";
                    MyImg studImagemess = new MyImg();//img
                    studImagemess.ImageUrl = "~/image/stumess.png";
                    MyImg studImageinout = new MyImg();//img
                    studImageinout.ImageUrl = "~/image/inout.png";
                    MyImg studImagetrans = new MyImg();//img
                    studImagetrans.ImageUrl = "~/image/transfer.png";
                    MyImg studImagevec = new MyImg();//img
                    studImagevec.ImageUrl = "~/image/vacate image1.png";



                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].CellType = studImage;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 8].CellType = studImages;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 8].Note = "Basic Details";
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 9].CellType = studImageuni;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 9].Note = "Student information";
                    Fpload1.Sheets[0].Rows[Fpload1.Sheets[0].RowCount - 1].Height = 5;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 10].CellType = studImagesatt;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 11].CellType = studImagemess;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 12].CellType = studImageinout;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 13].CellType = studImagetrans;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 14].CellType = studImagevec;


                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 10].Note = "Hostel Attendance";
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 11].Note = "Mess Attendance";
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 12].Note = "In Out";
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 13].Note = "Room Transfer";
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 14].Note = "Vacated";

                    //Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 15].CellType = imagecell7;
                    //Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 16].CellType = imagecell8;
                    //Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 17].CellType = imagecell7;
                    //Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 18].CellType = imagecell8;

                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);

                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].Text = Institutions;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].Text = Roll_No;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]).Trim();
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 4].Text = Name;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].Text = Hostel_Name;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[row]["HostelMasterFK"]).Trim();
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 6].Text = Room;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 7].Text = PhoneNo;


                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 14].BackColor = Color.White;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 13].BackColor = Color.Blue;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 12].BackColor = Color.LawnGreen;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 11].BackColor = Color.OrangeRed;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 10].BackColor = Color.Orange;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 11].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 12].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 13].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 14].VerticalAlign = VerticalAlign.Middle;

                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].Locked = true;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].Locked = true;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].Locked = true;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].Locked = true;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 4].Locked = true;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].Locked = true;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 6].Locked = true;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 7].Locked = true;
                   // Fpload1.Columns[2].Width = 50;
                }


                Fpload1.Sheets[0].PageSize = Fpload1.Sheets[0].RowCount;
                Fpload1.SaveChanges();
                Fpload1.Sheets[0].PageSize = 100;
                Fpload1.Visible = true;
                //  lbprint.Visible = true;
                // lblrptname.Visible = true;
                // txtexcelname.Visible = true;
                //  btn_excel.Visible = true;
                //  btnprintmaster.Visible = true;

            }
            else
            {
                Fpload1.Visible = false;
            }
        }

        catch
        {
        }
    }

    public void loadspreadHeader(DataSet ds)
    {

        try
        {

            Fpload1.Sheets[0].RowCount = 0;
            Fpload1.Sheets[0].ColumnCount = 15;
            Fpload1.CommandBar.Visible = false;
            Fpload1.Sheets[0].AutoPostBack = true;
            Fpload1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpload1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Fpload1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;


            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
            Fpload1.Sheets[0].Columns[0].Width = 20;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Photo";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
            Fpload1.Sheets[0].Columns[1].Width = 50;


            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Institutions";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
            Fpload1.Sheets[0].Columns[2].Width = 530;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll_No";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
            Fpload1.Sheets[0].Columns[3].Width = 80;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
            Fpload1.Sheets[0].Columns[4].Width = 120;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Hostel_Name";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;
            Fpload1.Sheets[0].Columns[5].Width = 150;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Room";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
            Fpload1.Sheets[0].Columns[6].Width = 10;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "PhoneNo";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 7].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 7].Locked = true;
            Fpload1.Sheets[0].Columns[7].Width = 10;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Details";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 8].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 8].Locked = false;
            Fpload1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 14);
            Fpload1.Sheets[0].Columns[8].Width = 130;
            Fpload1.Sheets[0].Columns[9].Width = 150;
            Fpload1.Sheets[0].Columns[10].Width = 130;
            Fpload1.Sheets[0].Columns[11].Width = 130;
            Fpload1.Sheets[0].Columns[12].Width = 130;
            Fpload1.Sheets[0].Columns[14].Width = 130;
            Fpload1.Sheets[0].Columns[13].Width = 120;
        }
        catch (Exception ex) { }
    }
    #endregion

    # region information

    private DataSet studinfor()
    {

        DataSet dsinfo = new DataSet();
        string sqry = string.Empty;
        try
        {
            #region get Value
            app = d2.GetFunction("select App_No from registration where roll_no='" + lblrolltxt2.Text + "'");
            sqry = "select a.parent_name as father_name,a.Parent_AddressP from applyn a where  a.app_no='" + app + "'";
            sqry += "select a.mother,a.Parent_AddressP from applyn a where  a.app_no='" + app + "'";
            sqry += "select a.guardian_name,a.gur_off_address1  from applyn a where  a.app_no='" + app + "'";

            dsinfo.Clear();
            dsinfo = d2.select_method_wo_parameter(sqry, "Text");
            #endregion
        }
        catch (Exception ex)
        { }

        return dsinfo;
    }

    private void loadspread2(DataSet dsload)
    {
        try
        {


            DataTable dt = new DataTable();
            dt.Columns.Add("SNo");
            dt.Columns.Add("Photo");
            dt.Columns.Add("VisitorName");
            dt.Columns.Add("Relationship");
            dt.Columns.Add("Address");


            Fpuser.Sheets[0].RowCount = 0;
            Fpuser.Sheets[0].ColumnCount = 0;
            Fpuser.CommandBar.Visible = false;
            Fpuser.Sheets[0].AutoPostBack = true;
            Fpuser.Sheets[0].ColumnHeader.RowCount = 1;
            Fpuser.Sheets[0].RowHeader.Visible = false;
            Fpuser.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpuser.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            for (int row = 0; row < dt.Columns.Count; row++)
            {

                Fpuser.Sheets[0].ColumnCount++;
                string col = Convert.ToString(dt.Columns[row].ColumnName);
                Fpuser.Sheets[0].ColumnHeader.Cells[0, row].Text = col;
                Fpuser.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
                Fpuser.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
                Fpuser.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
                Fpuser.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
                Fpuser.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
                Fpuser.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            }
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int rowcount = 0;
            string sqry = string.Empty;
            string Photoimages = string.Empty;

            int sno = 0;
            string VisitorName = string.Empty;
            string Relationship = string.Empty;
            string Address = string.Empty;
            string VisitorName1 = string.Empty;
            string Relationship1 = string.Empty;
            string Address1 = string.Empty;
            string VisitorName2 = string.Empty;
            string Relationship2 = string.Empty;
            string Address2 = string.Empty;
            if (dsload.Tables.Count > 0)
            {
                sno++;
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    Fpuser.Sheets[0].RowCount++;
                    MyImg imgfatp = new MyImg();//img
                    if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
                    {
                        imgfatp.ImageUrl = "~/Handler/Handler7.ashx?id=" + app;

                    }
                    VisitorName = Convert.ToString(dsload.Tables[0].Rows[0]["father_name"]).Trim();
                    Relationship = "Father";
                    Address = Convert.ToString(dsload.Tables[0].Rows[0]["parent_addressP"]).Trim();

                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].CellType = imgfatp;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].CellType = txtCell;

                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].Text = imgfatp.ImageUrl;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].Text = VisitorName;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].Text = Relationship;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].Text = Address;

                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                }
                sno++;
                if (dsload.Tables[1].Rows.Count > 0)
                {
                    Fpuser.Sheets[0].RowCount++;
                    MyImg imgmotp1 = new MyImg();//img


                    if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
                    {
                        imgmotp1.ImageUrl = "~/Handler/Handler8.ashx?id=" + app;
                    }

                    VisitorName1 = Convert.ToString(dsload.Tables[1].Rows[0]["mother"]).Trim();
                    Relationship1 = "Mother";
                    Address1 = Convert.ToString(dsload.Tables[1].Rows[0]["parent_addressP"]).Trim();

                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].CellType = imgmotp1;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].CellType = txtCell;

                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].Text = imgmotp1.ImageUrl;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].Text = VisitorName1;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].Text = Relationship1;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].Text = Address1;

                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                }
                sno++;
                if (dsload.Tables[2].Rows.Count > 0)
                {
                    Fpuser.Sheets[0].RowCount++;
                    MyImg imggurp2 = new MyImg();//img


                    if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
                    {
                        imggurp2.ImageUrl = "~/Handler/Handler9.ashx?id=" + app;

                    }

                    VisitorName2 = Convert.ToString(dsload.Tables[2].Rows[0]["guardian_name"]).Trim();
                    Relationship2 = "Guardian";
                    Address2 = Convert.ToString(dsload.Tables[2].Rows[0]["gur_off_address1"]).Trim();

                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].CellType = imggurp2;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].CellType = txtCell;

                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].Text = imggurp2.ImageUrl;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].Text = VisitorName2;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].Text = Relationship2;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].Text = Address2;

                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpuser.Columns[0].Width = 5;

                }
            }

            Fpuser.Sheets[0].PageSize = Fpuser.Sheets[0].RowCount;
            Fpuser.SaveChanges();
            Fpuser.Visible = true;


        }
        catch { }
    }
    #endregion

    # region hostelattendance

    private DataSet Hostel()
    {

        DataSet dshostel = new DataSet();
        //DataTable dtdetails = new DataTable();
        try
        {
            #region get Value
            string firstdate1 = Convert.ToString(txtFromDate.Text);
            string seconddate1 = Convert.ToString(txtToDate.Text);
            string fdmonth = "";
            string sdmonth = "";
            string fdyear = "";
            string sdyear = "";


            string[] splitt = firstdate1.Split('/');
            string[] splitt1 = seconddate1.Split('/');
            fdmonth = Convert.ToString(splitt[1]);
            sdmonth = Convert.ToString(splitt1[1]);
            fdyear = Convert.ToString(splitt[2]);
            sdyear = Convert.ToString(splitt1[2]);

            string room = "select AttnMonth,AttnYear,([D1]),[D2],[D3] ,[D4],[D5],[D6],[D7],[D8],[D9],[D10],[D11],[D12],[D13],[D14],[D15],[D16],[D17],[D18],[D19],[D20],[D21],[D22],[D23],[D24],[D25],[D26],[D27],[D28],[D29],[D30],[D31],[D1E],[D2E],[D3E],[D4E],[D5E],[D6E],[D7E],[D8E],[D9E],[D10E],[D11E],[D12E],[D13E],[D14E],[D15E],[D16E],[D17E],[D18E],[D19E],[D20E],[D21E],[D22E],[D23E],[D24E],[D25E],[D26E],[D27E],[D28E],[D29E],[D30E],[D31E]  from HT_Attendance where App_No='19038' and AttnMonth between '" + fdmonth + "'and '" + sdmonth + "' and AttnYear between '" + fdyear + "' and '" + sdyear + "'";
            dshostel.Clear();
            dshostel = d2.select_method_wo_parameter(room, "Text");
            #endregion

        }
        catch (Exception ex)
        { }

        return dshostel;
    }
    private void funmonth(string mond, string yeard, ref string mondcal)
    {
        try
        {
            int monthattn = 0;
            int yearattnn = 0;
            int.TryParse(mond, out monthattn);
            int.TryParse(yeard, out yearattnn);
            if (mond == "1")
                mondcal = "31";
            if (mond == "2")
            {
                if (yearattnn % 4 == 0)
                    mondcal = "29";
                else
                    mondcal = "28";
            }
            if (mond == "3")
                mondcal = "31";
            if (mond == "4")
                mondcal = "30";
            if (mond == "5")
                mondcal = "31";
            if (mond == "6")
                mondcal = "30";
            if (mond == "7")
                mondcal = "31";
            if (mond == "8")
                mondcal = "31";
            if (mond == "9")
                mondcal = "30";
            if (mond == "10")
                mondcal = "31";
            if (mond == "11")
                mondcal = "30";
            if (mond == "12")
                mondcal = "31";


        }
        catch
        {

        }
    }

    //private void loadhostelspread3(DataSet dsload1)
    private void loadhostelspread3()
    {
        try
        {


            DataTable dthos = new DataTable();
            dthos.Columns.Add("SNo");
            dthos.Columns.Add("Attendance Date");
            dthos.Columns.Add("Entry Time");
            dthos.Columns.Add("Status");

            DataSet attn = new DataSet();
            string AttndDayvalue = "";
            string AttndDayvalue1 = "";
            string mrnevng_att = "";
            string attnmonth = "";
            string attnyear = "";

            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpuser.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            int stday;
            int stmon;
            int styear;
            int endaday;
            int endmon;
            int endyear;

            string[] split5 = txtFromDate.Text.Split('/');

            string day5 = split5[0];
            string mo5 = split5[1];
            string yea5 = split5[2];
            string[] split6 = txtToDate.Text.Split('/');
            string day6 = split6[0];
            string mo6 = split6[1];
            string yea6 = split6[2];
            int.TryParse(day5, out stday);
            int.TryParse(day6, out endaday);
            int.TryParse(mo5, out stmon);
            int.TryParse(mo6, out endmon);
            int.TryParse(yea5, out styear);
            int.TryParse(yea6, out endyear);
            int dayfind = 0;
            dayfind = stday;

            string count = string.Empty;

            string attnpre = "select * from HT_Attendance where App_No=(select app_no from Registration where Roll_No='" + lblrolltxt3.Text + "') and AttnMonth between '" + mo5 + "' and '" + mo6 + "' and AttnYear between '" + yea5 + "' and '" + yea6 + "'";
            attn = d2.select_method_wo_parameter(attnpre, "text");

            int studcount = 0;
            int leavecount = 0;
            int latecount = 0;
            int present = 0;
            int absent = 0;
            int od = 0;
            int con = 0;
            string months = string.Empty;
            for (int row = 0; row < dthos.Columns.Count; row++)
            {
                studcount++;
                FpSpread1.Sheets[0].ColumnCount++;
                string col = Convert.ToString(dthos.Columns[row].ColumnName);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].Text = col;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            }

            int fprow = 0;
            int motnfr = 0;
            int sno = 0;
            if (attn.Tables[0].Rows.Count > 0 && attn.Tables.Count > 0)
            {
                for (int row = 0; row < attn.Tables[0].Rows.Count; row++)
                {
                    string mondcal = string.Empty;
                    string mond = Convert.ToString(attn.Tables[0].Rows[row]["AttnMonth"]);
                    string yeard = Convert.ToString(attn.Tables[0].Rows[row]["AttnYear"]);
                    funmonth(mond, yeard, ref mondcal);
                    int.TryParse(mondcal, out motnfr);
                    if (row == 0)
                    {

                        for (dayfind = stday; dayfind <= 31; dayfind++)
                        {
                            if (dayfind <= motnfr)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                AttndDayvalue1 = "D" + dayfind + "";
                                count = Convert.ToString(attn.Tables[0].Rows[row][AttndDayvalue1]);
                                int.TryParse(count, out con);
                                if (con == 1)
                                {
                                    present++;
                                    FpSpread1.Sheets[0].Cells[fprow, 3].Text = "present";
                                }
                                else if (con == 2)
                                {
                                    absent++;
                                    FpSpread1.Sheets[0].Cells[fprow, 3].Text = "Absent";
                                }
                                else if (con == 3)
                                {
                                    od++;
                                    FpSpread1.Sheets[0].Cells[fprow, 3].Text = "OD";
                                }
                                sno++;
                                FpSpread1.Sheets[0].Cells[fprow, 0].Text = Convert.ToString(sno);
                                FpSpread1.Sheets[0].Cells[fprow, 0].HorizontalAlign = HorizontalAlign.Center;
                                months = Convert.ToString(dayfind);
                                FpSpread1.Sheets[0].Cells[fprow, 1].Text = months + '-' + mond + '-' + yeard;
                                FpSpread1.Sheets[0].Cells[fprow, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Columns[1].Width = 15;
                                FpSpread1.Columns[0].Width = 15;
                                FpSpread1.Columns[2].Width = 15;
                                FpSpread1.Columns[3].Width = 50;
                                fprow++;
                            }
                        }

                    }
                    else
                    {
                        int tabrow = attn.Tables[0].Rows.Count;
                        int daymot = 0;
                        tabrow -= 1;
                        if (tabrow == row)
                            daymot = endaday;
                        else
                            daymot = motnfr;
                        for (dayfind = 1; dayfind <= daymot; dayfind++)
                        {
                            {
                                if (dayfind <= motnfr)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    AttndDayvalue1 = "D" + dayfind + "";
                                    count = Convert.ToString(attn.Tables[0].Rows[row][AttndDayvalue1]);
                                    int.TryParse(count, out con);
                                    if (con == 1)
                                    {
                                        present++;
                                        FpSpread1.Sheets[0].Cells[fprow, 3].Text = "present";
                                    }
                                    else if (con == 2)
                                    {
                                        absent++;
                                        FpSpread1.Sheets[0].Cells[fprow, 3].Text = "Absent";
                                    }
                                    else if (con == 3)
                                    {
                                        od++;
                                        FpSpread1.Sheets[0].Cells[fprow, 3].Text = "OD";
                                    }
                                    sno++;
                                    FpSpread1.Sheets[0].Cells[fprow, 0].Text = Convert.ToString(sno);
                                    FpSpread1.Sheets[0].Cells[fprow, 0].HorizontalAlign = HorizontalAlign.Center;
                                    months = Convert.ToString(dayfind);
                                    FpSpread1.Sheets[0].Cells[fprow, 1].Text = months + '-' + mond + '-' + yeard;
                                    FpSpread1.Sheets[0].Cells[fprow, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Columns[1].Width = 15;
                                    FpSpread1.Columns[0].Width = 15;
                                    FpSpread1.Columns[2].Width = 15;
                                    FpSpread1.Columns[3].Width = 50;
                                    fprow++;
                                }
                            }

                        }
                    }

                    FpSpread1.Visible = true;
                    FpSpread1.SaveChanges();
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

                }
            }
            else
            {
                FpSpread1.Visible = false;
            }
            att.Visible = true;
            lbl_count1.Visible = true;
            lbl_count1.Text = "Total Days :" + FpSpread1.Sheets[0].RowCount;
            lbl_count2.Visible = true;
            lbl_count2.Text = "Present :" + present;
            lbl_count3.Visible = true;
            lbl_count3.Text = "Absent :" + absent;
            lbl_count4.Visible = true;
            lbl_count4.Text = "Leave :" + leavecount.ToString();
            lbl_count5.Visible = true;
            lbl_count5.Text = "Late Entry :" + latecount.ToString();


        }
        catch
        {
        }
    }
    #endregion

    # region messattendance

    private DataSet Mess()
    {


        string selq = string.Empty;
        try
        {
            string infromdate = string.Empty;
            string intodate = string.Empty;
            string fromDate = txtFromDate1.Text;
            string toDate = txtToDate1.Text;
            string[] fromdate = fromDate.Split('/');
            string[] todate = toDate.Split('/');
            if (fromdate.Length == 3)
                infromdate = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

            if (todate.Length == 3)
                intodate = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
            if (ddlrollno.SelectedIndex == 0)
            {
                if (ddlrollno.Items.Count > 0)
                    rollno = Convert.ToString(txtno.Text.Trim());

                selq = "select distinct CONVERT(varchar(20),Entry_Date,103) Entry_Date,Session_name,Roll_No from HostelMess_Attendance where Entry_Date between'" + infromdate + "' and '" + intodate + "'and Roll_No='" + rollno + "'";
                dsmess = d2.select_method_wo_parameter(selq, "Text");
            }

        }
        catch (Exception ex)
        { }

        return dsmess;
    }

    private void loadmessdetails()
    //     private void loadmessdetails(DataSet dsload2)
    {

        try
        {
            int totdays = 0;
            int precount1 = 0;
            int absentcount1 = 0;

            DataTable dt = new DataTable();
            dt.Columns.Add("SNo");
            dt.Columns.Add("Roll No");
            dt.Columns.Add("Entry Date");
            dt.Columns.Add("Morning");
            dt.Columns.Add("Lunch");
            dt.Columns.Add("Dinner");

            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            for (int row = 0; row < dt.Columns.Count; row++)
            {

                FpSpread2.Sheets[0].ColumnCount++;
                string col = Convert.ToString(dt.Columns[row].ColumnName);
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].Text = col;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            }
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;
            int rowcount = 0;
            string messattendPresent = string.Empty;
            string messattendAbsent = string.Empty;
            string EntryDate = string.Empty;
            string morning = string.Empty;

            DataSet dsroom = new DataSet();
           // string activerow = Fpload1.ActiveSheetView.ActiveRow.ToString();
           // Roll_No = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
           //// Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(row), 4].Text);
           // Hostel_Name = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
           // string Hostel_code = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);


           // qry = "select hr.APP_No,R.Roll_No,R.Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,case when a.sex='0' then 'male' when a.sex='1' then 'Female'  else 'transder' end as sex ,a.parent_addressP ,a.parent_name as father_name,a.StuPer_Id as mail_id,BuildingFK,(select building_name from Building_Master where BuildingFK=code)as building_name,FloorFK,(select floor_name from Floor_Master where floorpk=FloorFK)as floor_name,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and h.HostelMasterPK in ('" + Hostel_code + "') and  R.Roll_No='" + Convert.ToString(Roll_No) + "'";
           // dsmess.Clear();
           // dsmess = d2.select_method_wo_parameter(qry, "Text");

            if (dsmess.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsmess.Tables[0].Rows.Count; row++)
                {

                    FpSpread2.Sheets[0].RowCount++;
                    totdays++;
                    precount1++;
                    absentcount1++;
                    sno++;
                    Roll_No = Convert.ToString(dsmess.Tables[0].Rows[row]["Roll_No"]).Trim();
                    EntryDate = Convert.ToString(dsmess.Tables[0].Rows[row]["Entry_Date"]).Trim();
                    morning = Convert.ToString(dsmess.Tables[0].Rows[row]["Session_name"]).ToLower();


                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = txtCell;

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Roll_No;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = EntryDate;

                    for (int j = 0; j < dsmess.Tables[0].Rows.Count; j++)
                    {

                        dsmess.Tables[0].DefaultView.RowFilter = " Roll_No='" + rollno + "' ";
                        DataTable dtroom = dsmess.Tables[0].DefaultView.ToTable();
                    }
                    messattendPresent = "Present";
                    messattendAbsent = "Absent";
                    if (morning == "breakfast")
                    {
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = messattendPresent;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = messattendAbsent;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = messattendAbsent;

                    }

                    if (morning == "Lunch")
                    {

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = messattendPresent;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = messattendAbsent;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = messattendAbsent;

                    }

                    if (morning == "dinner")
                    {

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = messattendPresent;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = messattendAbsent;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = messattendAbsent;

                    }


                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Locked = true;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Locked = true;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Locked = true;

                }
                messatt.Visible = true;
                Label16.Visible = false;
                Label17.Visible = true;
                Label17.Text = "Total Days :" + totdays.ToString();
                Label18.Visible = true;
                Label18.Text = "Present :" + precount1.ToString();
                Label19.Visible = true;
                Label19.Text = "Absent :" + absentcount1.ToString();
                FpSpread2.SaveChanges();
                FpSpread2.Visible = true;
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            }
            else
            {
                FpSpread2.Visible = false;
            }
           
            //}
        }
        catch 
        { 
        }
    }
    #endregion

    # region inandout

    private DataSet inandout()
    {

        DataSet dsinout = new DataSet();

        string selgry = string.Empty;
        try
        {
            #region get Value
            string infromdate = string.Empty;
            string intodate = string.Empty;
            string fromDate = txtFromDate2.Text;
            string toDate = txtToDate2.Text;
            string[] fromdate = fromDate.Split('/');
            string[] todate = toDate.Split('/');
            if (fromdate.Length == 3)
                infromdate = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

            if (todate.Length == 3)
                intodate = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();

            app = d2.GetFunction("select App_No from registration where roll_no='" + lblrolltxt5.Text + "'");
            selgry = "select distinct GatepassEntrydate,GatepassExitdate from GateEntryExit where app_no='" + app + "' and GatepassEntrydate  between '" + infromdate + "' and '" + intodate + "'  and GatepassExitdate between '" + infromdate + "' and '" + intodate + "' and  IsApproval='1' ";
            dsinout = d2.select_method_wo_parameter(selgry, "Text");


            #endregion
        }
        catch (Exception ex)
        { }

        return dsinout;
    }

    private void loadindetails(DataSet dsload2)
    {
        try
        {

            DataTable dt = new DataTable();




            popwindow4.Visible = false;
            FpSpread2.Visible = false;
            popwindow5.Visible = true;

            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].ScrollingContentVisible = false;
            FpSpread3.Sheets[0].AutoPostBack = false;
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.Sheets[0].ColumnHeader.Visible = true;
            MyStyle.Font.Size = FontUnit.Small;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            MyStyle.Border.BorderColor = Color.Blue;
            FpSpread3.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            FpSpread3.CommandBar.Visible = false;
            FpSpread3.RowHeader.Width = 100;
            FpSpread3.Sheets[0].ColumnCount = 3;

            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.BorderWidth = 0;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderSize = 1;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = Color.White;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderSize = 1;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColor = Color.White;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderSize = 1;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.White;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Entry Date";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Status";
            FpSpread3.Columns[2].Width = 200;
            FpSpread3.Columns[1].Width = 150;
            int sn = 0;

            if (dsload2.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsload2.Tables[0].Rows.Count; i++)
                {
                    sn++;
                    FpSpread3.Visible = true;
                    FpSpread3.Sheets[0].RowCount++;
                    FpSpread3.Sheets[0].Cells[i, 0].Text = Convert.ToString(sn);
                    FpSpread3.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                    string dat = Convert.ToString(dsload2.Tables[0].Rows[i]["GatepassEntrydate"]);
                    DateTime dtm = new DateTime();
                    string[] split = dat.Split();
                    string inoutdtm = string.Empty;
                    if (split.Length > 0)
                    {
                        string damm = split[0].ToString();
                        string[] split1 = damm.Split('/');

                        inoutdtm = Convert.ToString(split1[1] + "/" + split1[0] + "/" + split1[2]);
                    }

                    FpSpread3.Sheets[0].Cells[i, 1].Text = Convert.ToString(inoutdtm);
                    FpSpread3.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Cells[i, 2].Text = "Out";
                    FpSpread3.Sheets[0].Cells[i, 2].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            else
            {
                FpSpread3.Visible = false;
            }


            // dt.Columns.Add("SNo");
            // dt.Columns.Add("Entry Date");
            // dt.Columns.Add("Status");
            // popwindow4.Visible = true;
            // FpSpread2.Visible = true;
            // FpSpread2.Sheets[0].RowCount = 1;
            // FpSpread2.Sheets[0].ColumnCount = 4;
            // FpSpread2.CommandBar.Visible = false;
            // FpSpread2.Sheets[0].AutoPostBack = true;
            // FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
            // FpSpread2.Sheets[0].RowHeader.Visible = false;
            //// FpSpread2.Sheets[0].ColumnCount = 0;
            // FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            // darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            // darkstyle.ForeColor = Color.White;
            // FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            // for (int row = 0; row < dt.Columns.Count; row++)
            // {

            //     //FpSpread2.Sheets[0].ColumnCount++;
            //     string col = Convert.ToString(dt.Columns[row].ColumnName);
            //     FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].Text = col;
            //     FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
            //     FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
            //     FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
            //     FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
            //     FpSpread2.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
            //     FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            // }

            int totdays2 = 0;
            int precount2 = 0;
            int absentcount2 = 0;

            int sno = 0;
            int rowcount = 0;
            string messattendPresent = string.Empty;
            string messattendAbsent = string.Empty;
            string EntryDate = string.Empty;
            string status = string.Empty;

            DataSet dsroom = new DataSet();



            Label10.Visible = true;
            Label10.Text = "Total Days :" + sn;
            Label11.Visible = true;
            Label11.Text = "OUt :" + sn;
            Label12.Visible = true;
            Label12.Text = "IN :" + absentcount2.ToString();


            FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
            FpSpread3.Width = 500;
            FpSpread3.Height = 500;
            FpSpread3.SaveChanges();
           FpSpread3.Visible = true;

        }
        catch { }
    }
    #endregion

    #region roomtransfer

    void load_floorname()
    {
        try
        {
            cbofloorname.Items.Clear();
            string itemname = "select distinct floor_name,floorpk from HT_HostelRegistration h,Floor_Master fm where h.FloorFK=fm.Floorpk  and fm.College_Code='" + Convert.ToString(Session["collegecode"]) + "'";
            if (Cbo_HostelName.Text != "Select")
            {
                itemname = itemname + " and h.HostelMasterFK in('" + Convert.ToString(Cbo_HostelName.SelectedItem.Value) + "')";
            }
            if (ddlblock6.SelectedItem.Text.ToString() != "Select")
            {
                itemname = itemname + "  and Building_Name='" + Convert.ToString(ddlblock6.SelectedItem.Text) + "'";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbofloorname.DataSource = ds.Tables[0];
                cbofloorname.DataTextField = "floor_name";
                cbofloorname.DataValueField = "floorpk";
                cbofloorname.DataBind();
                cbofloorname.Items.Insert(0, "Select");
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
            Cbo_HostelName.Items.Clear();
            ds.Clear();
            string q = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelName";//where CollegeCode in ('" + Convert.ToString(Session["collegecode"]) + "')
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbo_HostelName.DataSource = ds;
                Cbo_HostelName.DataTextField = "HostelName";
                Cbo_HostelName.DataValueField = "HostelMasterPK";
                Cbo_HostelName.DataBind();
                Cbo_HostelName.Items.Insert(0, "Select");
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
            Cbo_Room.Items.Clear();
            string room = " select distinct Room_Name,Roompk from Room_Detail r,HT_HostelRegistration h where h.RoomFK =r.RoomPk and students_allowed<>Avl_Student and students_allowed>Avl_Student";

            if (Cbo_HostelName.SelectedItem.Text != "Select")
            {
                room = room + "  and h.HostelMasterFK in('" + Cbo_HostelName.SelectedItem.Value.ToString() + "')";
            }
            if (ddlblock6.SelectedItem.Text.ToString() != "Select")
            {
                room = room + "  and Building_Name='" + Convert.ToString(ddlblock6.SelectedItem.Text) + "'";
            }
            if (cbofloorname.SelectedItem.Text.ToString() != "Select")
            {
                room = room + "  and FloorFK='" + cbofloorname.SelectedItem.Value.ToString() + "'";
            }
            room = room + "  order by Room_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(room, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbo_Room.DataSource = ds;
                Cbo_Room.DataTextField = "Room_Name";
                Cbo_Room.DataValueField = "Roompk";
                Cbo_Room.DataBind();
                Cbo_Room.Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    void block()
    {
        try
        {
            ddlblock6.Items.Clear();
            string block =d2.GetFunction("select HostelBuildingFK from HM_HostelMaster where HostelMasterPK IN ('" + Convert.ToString(Cbo_HostelName.SelectedItem.Value) + "')");

            ds = d2.BindBuilding(block);
            // block = block + "  order by Building_Name";
            // ds.Clear();
            // ds = d2.select_method_wo_parameter(block, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlblock6.DataSource = ds;
                ddlblock6.DataTextField = "Building_Name";
                ddlblock6.DataValueField = "code";
                ddlblock6.DataBind();
                ddlblock6.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    protected void Cbo_HostelName_SelectedIndexChanged(object sender, EventArgs e)
    {
        block();
        load_floorname();
        load_room();


    }
    protected void ddlblock_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_room();
        load_floorname();

    }
    protected void cbofloorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_room();


    }
    protected void Cbo_Room_SelectedIndexChanged(object sender, EventArgs e)
    {


    }
    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region alertclose
    protected void btnpopsave_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = Fpload1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpload1.ActiveSheetView.ActiveColumn.ToString();
            string appp = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
            string hospk = Convert.ToString(Fpload1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
            Session["app"] = appp;
            string detai = "select * from HT_HostelRegistration where APP_No='" + appp + "'";
            DataSet detds = d2.select_method_wo_parameter(detai, "Text");
            string roomdet = "select f.Floor_Name,r.Room_Name,b.Building_Name from Floor_Master f,Room_Detail r,Building_Master b where f.Building_Name=r.Building_Name and f.Building_Name=b.Building_Name and f.Floor_Name=r.Floor_Name and f.FLOORpk='" + Convert.ToString(detds.Tables[0].Rows[0]["FloorFK"]) + "' and b.Code='" + Convert.ToString(detds.Tables[0].Rows[0]["BuildingFK"]) + "' and r.Roompk='" + Convert.ToString(detds.Tables[0].Rows[0]["RoomFK"]) + "'";
            DataSet detds1 = d2.select_method_wo_parameter(roomdet, "Text");
            if (btn_save1.Text == "RoomTransfer")
            {

                if (detds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < detds.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToString(ddlblock6.SelectedValue) != "Select" && Convert.ToString(cbofloorname.SelectedValue) != "Select" && Convert.ToString(Cbo_HostelName.SelectedValue) != "Select" && Convert.ToString(Cbo_Room.SelectedValue) != "Select")
                        {

                            string up = "update Room_Detail set Avl_Student= Avl_Student - 1 where Floor_Name='" + Convert.ToString(detds1.Tables[0].Rows[0]["Floor_Name"]) + "' and Room_Name='" + Convert.ToString(detds1.Tables[0].Rows[0]["Room_Name"]) + "' and Building_Name='" + Convert.ToString(detds1.Tables[0].Rows[0]["Building_Name"]) + "'";
                            int k = d2.update_method_wo_parameter(up, "text");
                            string up1 = " update Room_Detail set Avl_Student= Avl_Student + 1 where  Floor_Name='" + Convert.ToString(cbofloorname.SelectedItem.Text) + "' and Room_Name='" + Convert.ToString(Cbo_Room.SelectedItem.Text) + "' and Building_Name='" + Convert.ToString(ddlblock6.SelectedItem.Text) + "'";
                            k = d2.update_method_wo_parameter(up1, "text");
                            string query = "update  HT_HostelRegistration set MemType='1',BuildingFK='" + Convert.ToString(ddlblock6.SelectedValue) + "',FloorFK='" + Convert.ToString(cbofloorname.SelectedValue) + "',RoomFK='" + Convert.ToString(Cbo_Room.SelectedValue) + "', HostelMasterFK='" + Convert.ToString(Cbo_HostelName.SelectedValue) + "'   where HostelMasterFK='" + Convert.ToString(hospk) + "'  and APP_No ='" + appp + "'";
                            int h = d2.insert_method(query, hat, "Text");
                            if (h == 1 && k == 1)
                            {
                                Lbl.Visible = true;
                                Lbl.Text = "Transfer Complete";
                            }
                        }

                    }
                }
            }
            else
            {
                string vecated = "";
                vecated = "1";
                string vecated_date = Convert.ToString(txtdate.Text);
                string[] vdate = vecated_date.Split('/');
                DateTime vd = new DateTime();
                vd = Convert.ToDateTime(vdate[1] + "/" + vdate[0] + "/" + vdate[2]);
                vecated_date = Convert.ToString(vd.ToString("MM/dd/yyyy"));
                string reason = Convert.ToString(txt_reason.Text);

                string DC = "HSVAC"; string reasonds = Convert.ToString(txt_reason.Text);
                string reasoncode = subjectcodevac(DC, reasonds);
                int upm = 0;

                if (vecated.Trim() == "1")
                {
                    string up = " update Room_Detail set Avl_Student= Avl_Student - 1 where  Floor_Name='" + Convert.ToString(detds1.Tables[0].Rows[0]["Floor_Name"]) + "' and Room_Name='" + Convert.ToString(detds1.Tables[0].Rows[0]["Room_Name"]) + "' and Building_Name='" + Convert.ToString(detds1.Tables[0].Rows[0]["Building_Name"]) + "'";
                    upm = d2.update_method_wo_parameter(up, "text");
                }

                query = "update  HT_HostelRegistration set MemType='1',Reason='" + reasoncode + "',IsVacated='" + vecated + "',VacatedDate='" + vecated_date + "',collegecode='" + Convert.ToString(detds.Tables[0].Rows[0]["CollegeCode"]) + "'  where HostelMasterFK='" + Convert.ToString(hospk) + "'  and APP_No ='" + appp + "'";
                int h = d2.insert_method(query, hat, "Text");
                string regupdate = " update Registration set Stud_Type='Day Scholar' where App_No='" + appp + "'";
                int s = d2.update_method_wo_parameter(regupdate, "Text");
                if (s == 1 && h == 1 && upm == 1)
                {
                    Lbl.Visible = true;
                    Lbl.Text = " vecated Successfully";
                }
            }
        }
        catch (Exception ex) { }
    }
    public string subjectcodevac(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string detais = "select * from HT_HostelRegistration where APP_No='" + Convert.ToString(Session["app"]) + "'";
            DataSet detdst = d2.select_method_wo_parameter(detais, "Text");
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + Convert.ToString(detdst.Tables[0].Rows[0]["CollegeCode"]) + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + Convert.ToString(detdst.Tables[0].Rows[0]["CollegeCode"]) + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + Convert.ToString(detdst.Tables[0].Rows[0]["CollegeCode"]) + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }

    protected void btnpopexit_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
        popwindow2.Visible = false;
        popwindow3.Visible = false;
        popwindow4.Visible = false;
        popwindow5.Visible = false;
        popwindow6.Visible = false;
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
        }
        catch { }
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
        popwindow2.Visible = false;
        popwindow3.Visible = false;
        popwindow4.Visible = false;
        popwindow5.Visible = false;
        popwindow6.Visible = false;
    }
    #endregion

    # region print

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            //string reportname = txtexcelname.Text;

            //if (reportname.ToString().Trim() != "")
            //{
            //    d2.printexcelreport(Fpload1, reportname);
            //}
            //else
            //{
            //    txtexcelname.Focus();
            //    //  lblerrmainapp.Text = "Please Enter Your Report Name";
            //    // lblerrmainapp.Visible = true;lbprint
            //    lbprint.Text = "Please Enter Your Report Name";
            //    lbprint.Visible = true;
            //}

        }
        catch (Exception ex)
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        //string degreedetails = "Student Search";

        //string pagename = "StudentSearch.aspx";
        //Session["column_header_row_count"] = Fpload1.ColumnHeader.RowCount;

        //Printcontrol.loadspreaddetails(Fpload1, pagename, degreedetails);
        //Printcontrol.Visible = true;

    }
    #endregion


}