using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class ExamHallMaster : System.Web.UI.Page
{
    SqlConnection con;
    int totlselectedseats = 0;
    Boolean flag_true = false;
    string CollegeCode;
    static string[] ss;
    static string p = string.Empty;
    static string[] ss1;
    string ss2 = string.Empty;
    DAccess2 da = new DAccess2();
    Boolean newroomseats = false;
    string building_name = "", floor_name = "", hall_name = "", default_view = "", arranged_view = "", mode = string.Empty;
    FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.IntegerCellType intgrcel = new FarPoint.Web.Spread.IntegerCellType();

    public void Connection()
    {
        con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        con.Open();

    }
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
            CollegeCode = Session["collegecode"].ToString();
            chk.AutoPostBack = true;
            errmsg.Visible = false;

            if (!IsPostBack)
            {
                p = string.Empty;
                Connection();
                sprdHallMaster.Visible = false;
                sprdHallMaster.Sheets[0].ColumnHeader.RowCount = 2;
                sprdHallMaster.Sheets[0].ColumnCount = 9;
                sprdHallMaster.Width = 870;
                loadtype();// added by sridhar 26/oct/2014

                sprdHallMaster.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                sprdHallMaster.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                sprdHallMaster.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                sprdHallMaster.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                sprdHallMaster.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                sprdHallMaster.Sheets[0].DefaultStyle.Font.Bold = false;
                sprdHallMaster.CommandBar.Visible = false;

                sprdHallMaster.RowHeader.Visible = false;
                sprdHallMaster.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                sprdHallMaster.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                sprdHallMaster.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                sprdHallMaster.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                sprdHallMaster.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                sprdHallMaster.Sheets[0].Columns[0].Locked = true;
                sprdHallMaster.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                sprdHallMaster.Sheets[0].Columns[1].Width = 150;
                sprdHallMaster.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                sprdHallMaster.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Building Name";
                sprdHallMaster.Sheets[0].Columns[1].Locked = true;
                sprdHallMaster.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                sprdHallMaster.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Floor No";
                sprdHallMaster.Sheets[0].Columns[2].Locked = true;
                sprdHallMaster.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                sprdHallMaster.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hall No";
                sprdHallMaster.Sheets[0].Columns[3].Locked = true;
                sprdHallMaster.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 2);
                sprdHallMaster.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Exam Seating";
                sprdHallMaster.Sheets[0].Columns[4].Locked = true;
                sprdHallMaster.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Rows";
                sprdHallMaster.Sheets[0].Columns[5].Locked = true;
                sprdHallMaster.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Columns";
                sprdHallMaster.Sheets[0].Columns[6].Locked = true;
                sprdHallMaster.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                sprdHallMaster.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Max Students";
                sprdHallMaster.Sheets[0].Columns[7].Locked = true;

                sprdHallMaster.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Priority";

                sprdHallMaster.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                sprdHallMaster.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
                sprdHallMaster.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                sprdHallMaster.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                sprdHallMaster.Sheets[0].Columns[7].CellType = chk;
                sprdHallMaster.Sheets[0].AddSpanCell(0, 0, 1, 7);
                sprdHallMaster.Sheets[0].Cells[0, 7].CellType = chk;

                sprdHallMaster.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                sprdHallMaster.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                sprdHallMaster.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

                DataSet ds = new DataSet();
                string strmode = string.Empty;
                if (ddltype.Enabled == true && ddltype.Items.Count > 0)
                {
                    strmode = "and (c.mode='" + ddltype.SelectedItem.Text.ToString() + "' or c.mode='BOTH')";
                }
                string strsql = "select distinct b.Building_Name as BuildingName,F.Floor_Name as FloorNo,R.Room_Name as HallNo,R.no_of_rows as Rows,R.no_of_columns as Columns,R.students_allowed as MaxStudents,c.priority from Building_Master B,Floor_Master F,Room_Detail R,class_master c where c.rno= r.room_name and c.floorid=r.floor_name and  c.block=r.Building_Name  and c.coll_code=b.College_Code and R.Floor_Name=F.Floor_Name and  F.Building_Name=B.Building_Name and r.building_name=b.Building_Name  and r.floor_name=f.floor_name and r.College_Code=B.College_Code " + strmode + " and B.College_Code='" + CollegeCode + "' order by priority,BuildingName,FloorNo";

                //string strsql = "select distinct b.Building_Name as BuildingName,F.Floor_Name as FloorNo,R.Room_Name as HallNo,R.no_of_rows as Rows,R.no_of_columns as Columns,R.students_allowed as MaxStudents,c.priority from Building_Master B,Floor_Master F,Room_Detail R left join class_master c on c.rno= r.room_name and c.floorid=r.floor_name and  c.rno=r.room_name and  c.block=r.Building_Name " + strmode + " where R.Floor_Name=F.Floor_Name and  F.Building_Name=B.Building_Name and r.building_name=b.Building_Name  and r.floor_name=f.floor_name and r.College_Code=B.College_Code and B.College_Code='" + CollegeCode + "' order by priority,BuildingName,FloorNo";
                ds = da.select_method_wo_parameter(strsql, "Text");
                int count = ds.Tables[0].Rows.Count;

                sprdHallMaster.ActiveSheetView.PageSize = ds.Tables[0].Rows.Count + 1;
                sprdHallMaster.Sheets[0].RowCount = ds.Tables[0].Rows.Count + 1;
                sprdHallMaster.Height = 1000;
                string SNo = string.Empty;
                if (count > 0)
                {
                    sprdHallMaster.Sheets[0].Columns[7].Locked = true;
                    sprdHallMaster.Sheets[0].Columns[8].Locked = true;
                    int n = 0;
                    sprdHallMaster.Visible = true;
                    for (int i = 0; i < count; i++)
                    {
                        if (SNo != Convert.ToString(ds.Tables[0].Rows[i]["BuildingName"]).Trim())
                        {
                            n = n + 1;
                        }
                        sprdHallMaster.Sheets[0].Cells[i + 1, 0].Text = n.ToString();
                        sprdHallMaster.Sheets[0].Cells[i + 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["BuildingName"]).Trim();
                        sprdHallMaster.Sheets[0].Cells[i + 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["FloorNo"]).Trim();
                        sprdHallMaster.Sheets[0].Cells[i + 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["HallNo"]).Trim();
                        sprdHallMaster.Sheets[0].Cells[i + 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Rows"]).Trim();
                        sprdHallMaster.Sheets[0].Cells[i + 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Columns"]).Trim();
                        sprdHallMaster.Sheets[0].Cells[i + 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["MaxStudents"]).Trim();

                        sprdHallMaster.Sheets[0].Cells[i + 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["priority"]).Trim();
                        string strpri = Convert.ToString(ds.Tables[0].Rows[i]["priority"]).Trim();
                        if (strpri != "" && strpri != null && strpri != "-1")
                        {
                            sprdHallMaster.Sheets[0].Cells[i + 1, 7].Value = "1";
                        }
                        SNo = Convert.ToString(ds.Tables[0].Rows[i]["BuildingName"]).Trim();
                    }
                }
                else
                {
                    loadrest();
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void loadtype()
    {
        try
        {
            string collegecode = Session["collegecode"].ToString();
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            DataSet dstype = da.select_method_wo_parameter(strquery, "Text");
            if (dstype.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = dstype;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
            }
            else
            {
                ddltype.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)// added by sridhar 26/oct/2014
    {
        loadset();
    }

    public void loadset()
    {
        try
        {
            DataSet ds = new DataSet();
            string strsql = string.Empty;
            string collegecode = Session["collegecode"].ToString();

            string strmode = string.Empty;
            if (ddltype.Enabled == true && ddltype.Items.Count > 0)
            {
                strmode = "and class_master.mode='" + ddltype.SelectedItem.Text.ToString() + "'";
            }
            strsql = "select distinct b.Building_Name as BuildingName,F.Floor_Name as FloorNo,R.Room_Name as HallNo,R.no_of_rows as Rows,R.no_of_columns as Columns,R.students_allowed as MaxStudents,class_master.priority from Building_Master B,Floor_Master F,Room_Detail R right join class_master  on class_master.rno= r.room_name and class_master.floorid=r.floor_name and  class_master.rno=r.room_name and  class_master.block=r.Building_Name " + strmode + " where R.Floor_Name=F.Floor_Name and  F.Building_Name=B.Building_Name and r.building_name=b.Building_Name  and r.floor_name=f.floor_name and r.College_Code=B.College_Code  and B.College_Code='" + collegecode + "' order by class_master.priority,BuildingName,FloorNo";
            ds = da.select_method_wo_parameter(strsql, "Text");
            int count = ds.Tables[0].Rows.Count;
            sprdHallMaster.ActiveSheetView.PageSize = ds.Tables[0].Rows.Count + 1;
            sprdHallMaster.Sheets[0].RowCount = ds.Tables[0].Rows.Count + 1;
            sprdHallMaster.Height = 1000;
            string SNo = string.Empty;
            if (count > 0)
            {
                sprdHallMaster.Visible = true;
                btnSave.Visible = true;
                int n = 0;
                sprdHallMaster.Visible = true;
                for (int i = 0; i < count; i++)
                {
                    if (SNo != ds.Tables[0].Rows[i]["BuildingName"].ToString())
                    {
                        n = n + 1;
                    }
                    sprdHallMaster.Sheets[0].Cells[i + 1, 0].Text = n.ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 1].Text = ds.Tables[0].Rows[i]["BuildingName"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 2].Text = ds.Tables[0].Rows[i]["FloorNo"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 3].Text = ds.Tables[0].Rows[i]["HallNo"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 4].Text = ds.Tables[0].Rows[i]["Rows"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 5].Text = ds.Tables[0].Rows[i]["Columns"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 6].Text = ds.Tables[0].Rows[i]["MaxStudents"].ToString();
                    if (ds.Tables[0].Rows[i]["MaxStudents"].ToString().Trim() != "" && ds.Tables[0].Rows[i]["MaxStudents"].ToString() != null)
                    {
                        totlselectedseats = totlselectedseats + Convert.ToInt32(ds.Tables[0].Rows[i]["MaxStudents"]);
                    }

                    sprdHallMaster.Sheets[0].Cells[i + 1, 8].Text = string.Empty;
                    sprdHallMaster.Sheets[0].Cells[i + 1, 7].Value = 0;
                    sprdHallMaster.Sheets[0].Columns[8].Locked = false;
                    sprdHallMaster.Sheets[0].Columns[7].Locked = false;
                    sprdHallMaster.Sheets[0].Cells[i + 1, 8].Text = ds.Tables[0].Rows[i]["priority"].ToString();
                    sprdHallMaster.Sheets[0].Columns[8].Locked = true;
                    string chkprirty = ds.Tables[0].Rows[i]["priority"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 7].Value = 1;
                    sprdHallMaster.Sheets[0].Columns[7].Locked = true;

                    SNo = ds.Tables[0].Rows[i]["BuildingName"].ToString();
                }
                lbltotseats.Text = Convert.ToString(totlselectedseats);
            }
            else
            {
                loadrest();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void loadrest()
    {
        try
        {
            lbltotseats.Text = "0";
            p = string.Empty;
            DataSet ds = new DataSet();
            string strsql = string.Empty;
            string collegecode = Session["collegecode"].ToString();
            string strmode = string.Empty;
            if (ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.Text.ToString().Trim().ToLower() != "all")
                {
                    strmode = " and b.BuildType='" + ddltype.SelectedItem.Text.ToString() + "'";
                }
            }

            strsql = "select distinct b.Building_Name as BuildingName,F.Floor_Name as FloorNo,R.Room_Name as HallNo,R.no_of_rows as Rows,R.no_of_columns as Columns,R.students_allowed as MaxStudents from Building_Master B,Floor_Master F,Room_Detail R  where R.Floor_Name=F.Floor_Name and  F.Building_Name=B.Building_Name and r.building_name=b.Building_Name and r.College_Code=b.College_Code and B.College_Code='" + collegecode + "' " + strmode + "  and isnull(no_of_rows,'0')<>'0' and isnull(no_of_columns,'0')<>'0'  order by BuildingName,FloorNo";
            ds = da.select_method_wo_parameter(strsql, "Text");

            int count = ds.Tables[0].Rows.Count;

            sprdHallMaster.ActiveSheetView.PageSize = ds.Tables[0].Rows.Count + 1;
            sprdHallMaster.Sheets[0].RowCount = 0;
            sprdHallMaster.Sheets[0].RowCount = ds.Tables[0].Rows.Count + 1;
            sprdHallMaster.Height = 1000;
            string SNo = string.Empty;
            if (count > 0)
            {
                sprdHallMaster.Visible = true;
                btnSave.Visible = true;
                int n = 0;
                sprdHallMaster.Visible = true;
                for (int i = 0; i < count; i++)
                {

                    if (SNo != ds.Tables[0].Rows[i]["BuildingName"].ToString())
                    {
                        n = n + 1;
                    }
                    sprdHallMaster.Sheets[0].Cells[i + 1, 0].Text = n.ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 1].Text = ds.Tables[0].Rows[i]["BuildingName"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 2].Text = ds.Tables[0].Rows[i]["FloorNo"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 3].Text = ds.Tables[0].Rows[i]["HallNo"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 4].Text = ds.Tables[0].Rows[i]["Rows"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 5].Text = ds.Tables[0].Rows[i]["Columns"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 6].Text = ds.Tables[0].Rows[i]["MaxStudents"].ToString();
                    sprdHallMaster.Sheets[0].Cells[i + 1, 7].Value = 0;
                    sprdHallMaster.Sheets[0].Cells[i + 1, 8].Text = string.Empty;
                    sprdHallMaster.Sheets[0].Columns[7].Locked = false;
                    sprdHallMaster.Sheets[0].Columns[8].Locked = false;

                    SNo = ds.Tables[0].Rows[i]["BuildingName"].ToString();
                }
            }
            else
            {
                sprdHallMaster.Visible = true;
                btnSave.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnreset_Click(object sender, EventArgs e)  // added by sridhar 28 oct 2014
    {
        string strmode = string.Empty;
        if (ddltype.Enabled == true)
        {
            if (ddltype.SelectedItem.Text.ToString().Trim().ToLower() != "all")
            {
                strmode = " and Mode='" + ddltype.SelectedItem.Text.ToString() + "'";
            }
        }
        string strsql = "delete from  class_master where coll_code='" + CollegeCode + "' " + strmode + " ";
        int res = da.update_method_wo_parameter(strsql, "text");
        loadrest();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // added by sridhar 03/Nov/2014 start
            int checkselectedrows = 0;
            for (int ss = 0; ss < sprdHallMaster.Sheets[0].RowCount; ss++)
            {
                if (sprdHallMaster.Sheets[0].Cells[ss, 7].Text == "True")
                {
                    checkselectedrows++;
                }
            }
            if (checkselectedrows == 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select Any One')", true);
                return;
            }
            // added by sridhar 03/Nov/2014 End
            Connection();
            string Block;
            string RoomNo;
            string NoRows;
            string NoCols;
            string CollegeCode = string.Empty;
            string FloorId;
            string Priority;
            string MaxStudents;

            try
            {
                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                {
                    CollegeCode = Session["collegecode"].ToString();

                }
                string mode = string.Empty;
                if (ddltype.Items.Count > 0)// added by sridhar 26/oct/2014
                {
                    mode = ddltype.SelectedItem.Text.ToString();
                    mode = mode.Trim();
                }

                sprdHallMaster.SaveChanges();
                for (int r = 1; r < sprdHallMaster.Sheets[0].RowCount; r++)
                {
                    if (Convert.ToInt16(sprdHallMaster.Sheets[0].Cells[r, 7].Value) == 1)
                    {
                        Block = sprdHallMaster.Sheets[0].Cells[r, 1].Text;
                        FloorId = sprdHallMaster.Sheets[0].Cells[r, 2].Text;
                        RoomNo = sprdHallMaster.Sheets[0].Cells[r, 3].Text;
                        NoRows = sprdHallMaster.Sheets[0].Cells[r, 4].Text;
                        NoCols = sprdHallMaster.Sheets[0].Cells[r, 5].Text;
                        Priority = sprdHallMaster.Sheets[0].Cells[r, 8].Text;
                        MaxStudents = sprdHallMaster.Sheets[0].Cells[r, 6].Text;
                        con.Close();
                        con.Open();
                        SqlCommand cmd = new SqlCommand("ProcClassMasterAdd", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Block", Block);
                        cmd.Parameters.AddWithValue("@RoomNo", RoomNo);
                        cmd.Parameters.AddWithValue("@NoRows", NoRows);
                        cmd.Parameters.AddWithValue("@NoCols", NoCols);
                        cmd.Parameters.AddWithValue("@NoExam", MaxStudents);
                        cmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
                        cmd.Parameters.AddWithValue("@FloorId", FloorId);
                        cmd.Parameters.AddWithValue("@Priority", Priority);
                        cmd.Parameters.AddWithValue("@Mode", mode); // added by sridhar 26/oct/2014
                        cmd.ExecuteNonQuery();
                    }
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                p = string.Empty;  // changed by sridhar 28 oct 2014
                loadset();
                //if (ddltype.Items.Count > 0)// added by sridhar 26/oct/2014
                //{
                //    ddltype_SelectedIndexChanged(sender, e);
                //}
            }
            catch (Exception ex)
            {
                errmsg.Visible = true;
                errmsg.Text = ex.ToString();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
        con.Dispose();
    }
    protected void sprdHallMaster_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        for (int mm = 1; mm < sprdHallMaster.Sheets[0].RowCount; mm++)
        {
            int selectvalue = Convert.ToInt32(sprdHallMaster.Sheets[0].Cells[mm, 7].Value.ToString());
            if (selectvalue == 1)
            {
                totlselectedseats++;
            }
        }
        lbltotseats.Text = Convert.ToString(totlselectedseats);
    }


    protected void sprdHallMaster_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow1;
            actrow1 = e.SheetView.ActiveRow.ToString();


            if (flag_true == false && actrow1 == "0")
            {
                for (int j = 1; j < Convert.ToInt16(sprdHallMaster.Sheets[0].RowCount); j++)
                {
                    string actcol1 = e.SheetView.ActiveColumn.ToString();
                    string seltext = e.EditValues[Convert.ToInt16(actcol1)].ToString();
                    if (seltext != "System.Object")
                        sprdHallMaster.Sheets[0].Cells[j, Convert.ToInt16(actcol1)].Text = seltext.ToString();
                }
                flag_true = true;
            }
            else if (actrow1 != "0")
            {


                string number = "True";

                int actcol = Convert.ToInt16(e.SheetView.ActiveColumn.ToString());
                int actrow = Convert.ToInt16(e.SheetView.ActiveRow.ToString());


                string st1;
                string st;
                st = sprdHallMaster.GetEditValue(actrow, actcol).ToString();
                //  string sssshhs = sprdHallMaster.GetEditValue(1, 7).ToString();
                st1 = e.EditValues[actcol].ToString();
                if (st == number)
                {


                    if (p == "")
                    {
                        p = actrow.ToString();
                    }
                    else
                    {
                        p = p + "-" + actrow.ToString();
                    }
                    ss = p.Split(new char[] { '-' });
                    int cnt12 = 0;
                    for (int i = 0; i < ss.Length; i++)
                    {
                        if (ss[i] != "")
                        {
                            cnt12 = cnt12 + 1;
                            sprdHallMaster.Sheets[0].Cells[Convert.ToInt16(ss[i]), 8].Text = cnt12.ToString();

                        }
                    }

                }
                else
                {

                    for (int j = 0; j < ss.Length; j++)
                    {
                        int n;
                        if (ss[j] == "")
                        {
                            n = 0;
                        }
                        else
                        {
                            n = Convert.ToInt16(ss[j]);

                        }

                        if (n == actrow)
                        {
                            sprdHallMaster.Sheets[0].Cells[n, 8].Text = string.Empty;
                            ss[j] = string.Empty;

                        }
                        else
                        {


                            if (ss2 == "")
                            {
                                ss2 = ss[j].ToString();
                            }
                            else
                            {
                                ss2 = ss2 + "-" + ss[j].ToString();
                            }
                        }
                    }
                    int ccnt = 0;
                    ss1 = ss2.Split(new char[] { '-' });
                    for (int s = 0; s < ss1.Length; s++)
                    {
                        if (ss1[s] != "")
                        {
                            ccnt = ccnt + 1;
                            sprdHallMaster.Sheets[0].Cells[Convert.ToInt16(ss1[s]), 8].Text = ccnt.ToString();

                        }
                    }

                    p = ss2;
                }

            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }

    }
}
