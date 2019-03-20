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
using System.Configuration;
public partial class LibraryMod_LibraryAddScreen : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    string UserCode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    DataTable dtCommon = new DataTable();
    DataSet dsprint = new DataSet();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    bool BlnAllowMulColStud = false;
    int Memtype = 0;
    bool BlnUseClientSysDate = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("LibraryHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/LibraryMod/LibraryHome.aspx");
                    return;
                }
            }
            UserCode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            divWelcome.Visible = false;
            LblName.Text = "";
            LblDept.Text = "";
            img_stud1.ImageUrl = "";
            Page.Form.DefaultFocus = Txt_UserID.ClientID;
            if (!IsPostBack)
            {
                Session["LibraryAddScreen"] = "EntryScreen";
                LblLibrarianName.Text = "";
                Bindcollege();
                getLibPrivil();
                string SqlQry = "SELECT LinkValue FROM InsSettings WHERE LinkName = 'Use Client System Date for User In Out' AND College_Code =" + ddl_collegename.SelectedValue + "";
                ds = d2.select_method_wo_parameter(SqlQry, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]) == "1")
                        BlnUseClientSysDate = true;
                    else
                        BlnUseClientSysDate = false;
                }
                else
                    BlnUseClientSysDate = false;
                if (BlnUseClientSysDate == true)
                {
                    ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:fundate(); ", true);                      
                }
                else
                {
                    string dtDate = d2.ServerDate();
                    string[] dat = dtDate.Split('/');
                    if (dat.Length == 3)
                        dtDate = dat[1] + '/' + dat[0] + '/' + dat[2];
                    LblDate.Text = dtDate.Split(' ')[0];
                }
                DisplayStatus(sender, e);
                Txt_UserID.Text = "";
                getLibPrivil();
                displayCollegeName();
                string StrSmartNo = d2.GetFunction("SELECT RFIDNew1 FROM InsInstaller");

                //if (StrSmartNo == "1")
                //{
                //    Txt_UserID.Visible = false;
                //    Txt_SmartCardID.Visible = true;
                //    //If Txt_SmartCardID.Visible Then Txt_SmartCardID.ZOrder
                //    //If Txt_SmartCardID.Visible Then Txt_SmartCardID.SetFocus
                //}
                //else
                //{
                //    Txt_SmartCardID.Visible = false;
                //    Txt_UserID.Visible = true;
                //    //Txt_UserID.Left = Txt_SmartCardID.Left
                //    //Txt_UserID.ZOrder
                //    //Txt_UserID.SetFocus
                //}
            }
        }
        catch (Exception ex) { }
    }

    #region college

    public void Bindcollege()
    {
        try
        {
            //ddl_library.Items.Clear();
            dtCommon.Clear();
            ddl_collegename.Enabled = false;
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddl_collegename.DataSource = dtCommon;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
                ddl_collegename.SelectedIndex = 0;
                ddl_collegename.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "LibraryRackAllocation");
        }
    }

    public void displayCollegeName()
    {
        string sql = string.Empty;
        string colcode = Convert.ToString(ddl_collegename.SelectedValue);
        sql = "select com_name from collinfo where com_name<>'' and college_code='" + colcode + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(sql, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            LblCollegeName.Text = Convert.ToString(ds.Tables[0].Rows[0]["com_name"]);
        }
        else
        {
            sql = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + colcode + "' ";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(sql, "Text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                LblCollegeName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["collname"]);
            }
        }
    }
    #endregion

    public void bindLibrary(string LibCollection)
    {
        ddl_LibName.Items.Clear();
        ds.Clear();
        string collegecode = Convert.ToString(ddl_collegename.SelectedValue);
        string SelectQ = string.Empty;

        SelectQ = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND  college_code in('" + collegecode + "') ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
        ds = d2.select_method_wo_parameter(SelectQ, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_LibName.DataSource = ds;
            ddl_LibName.DataTextField = "lib_name";
            ddl_LibName.DataValueField = "lib_code";
            ddl_LibName.DataBind();

            ddlLib_ManualExit.DataSource = ds;
            ddlLib_ManualExit.DataTextField = "lib_name";
            ddlLib_ManualExit.DataValueField = "lib_code";
            ddlLib_ManualExit.DataBind();
            ddlLib_ManualExit.Items.Add("All");
        }
    }

    protected void ddl_collegename_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
        displayCollegeName();
    }

    protected void ddlLibName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DisplayStatus(sender, e);
    }

    protected void DisplayStatus(object sender, EventArgs e)
    {
        try
        {            
            string Sql = string.Empty;
            string fromdate = LblDate.Text;
            string Libcode = Convert.ToString(ddl_LibName.SelectedItem.Value);
            string college = Convert.ToString(ddl_collegename.SelectedItem.Value);

            //**************Library Settings*************

            Sql = "SELECT ISNULL(AllowAllCollStud,0) AllowAllCollStud FROM Library WHERE Lib_Code ='" + Libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string AllowStud = Convert.ToString(ds.Tables[0].Rows[0]["AllowAllCollStud"]);
                if (AllowStud == "True")
                    BlnAllowMulColStud = true;
                else
                    BlnAllowMulColStud = false;
            }
            else
            {
                BlnAllowMulColStud = false;
            }
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();

            //For Best Studnet
            Sql = "select query1.* from (select roll_no,stud_name,dept_name,count(*) as total from libusers where usercat = 'Student' and year(entry_date) ='" + frdate[2].ToString() + "' and month(entry_date) ='" + frdate[1].ToString() + "' and lib_code ='" + Libcode + "' group by roll_no,stud_name,dept_name) query1,(select max(query2.total) as highest from (select roll_no,stud_name,count(*) as total from libusers where usercat = 'Student' and year(entry_date) ='" + frdate[2].ToString() + "' and month(entry_date) ='" + frdate[1].ToString() + "'and lib_code ='" + Libcode + "' group by roll_no,stud_name) query2) query3 Where query1.total = query3.Highest";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");

            if (dsload.Tables[0].Rows.Count > 0)
            {
                string name = Convert.ToString(dsload.Tables[0].Rows[0]["Stud_Name"]);
                string dept = Convert.ToString(dsload.Tables[0].Rows[0]["dept_name"]);
                LblStuName.Text = name + '-' + dept;
            }
            else
            {
                LblStuName.Text = "";
            }
            //For Best Staff
            Sql = "select query1.* from (select roll_no,stud_name,dept_name,count(*) as total from libusers where usercat = 'Staff' and year(entry_date) ='" + frdate[2].ToString() + "' and month(entry_date) ='" + frdate[1].ToString() + "' and lib_code ='" + Libcode + "' group by roll_no,stud_name,dept_name) query1,(select max(query2.total) as highest from (select roll_no,stud_name,count(*) as total from libusers where usercat = 'Staff' and year(entry_date) ='" + frdate[2].ToString() + "' and month(entry_date) ='" + frdate[1].ToString() + "' and lib_code ='" + Libcode + "' group by roll_no,stud_name,dept_name) query2) query3 Where query1.total = query3.Highest";

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");

            if (dsload.Tables[0].Rows.Count > 0)
            {
                string name = Convert.ToString(dsload.Tables[0].Rows[0]["Stud_Name"]);
                string dept = Convert.ToString(dsload.Tables[0].Rows[0]["dept_name"]);
                LblStaffName.Text = name + '-' + dept;
            }
            else
            {
                LblStaffName.Text = "";
            }
            //Student Total
            Sql = "select count(*) as total from libusers where usercat = 'Student' and year(entry_date) ='" + frdate[2].ToString() + "' and month(entry_date) ='" + frdate[1].ToString() + "' and lib_code ='" + Libcode + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");

            if (dsload.Tables[0].Rows.Count > 0)
            {
                string total = Convert.ToString(dsload.Tables[0].Rows[0]["total"]);

                LblStuMthTot.Text = total;
            }
            else
            {
                LblStuMthTot.Text = "";
            }
            //Staff Total
            Sql = "select count(*) as total from libusers where usercat = 'Staff' and year(entry_date) ='" + frdate[2].ToString() + "' and month(entry_date) ='" + frdate[1].ToString() + "' and lib_code ='" + Libcode + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                string total = Convert.ToString(dsload.Tables[0].Rows[0]["total"]);

                LblStaffMthTot.Text = total;
            }
            else
            {
                LblStaffMthTot.Text = "";
            }

            //Visitor Total
            Sql = "select count(*) as total from libusers where usercat = 'Visitor' and year(entry_date) ='" + frdate[2].ToString() + "' and month(entry_date) ='" + frdate[1].ToString() + "' and lib_code ='" + Libcode + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                string total = Convert.ToString(dsload.Tables[0].Rows[0]["total"]);

                LblVisitMthTot.Text = total;
            }
            else
            {
                LblVisitMthTot.Text = "";
            }

            //Total User
            Sql = "select count(*) as total from libusers where year(entry_date) ='" + frdate[2].ToString() + "' and month(entry_date) ='" + frdate[1].ToString() + "' and lib_code ='" + Libcode + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                string total = Convert.ToString(dsload.Tables[0].Rows[0]["total"]);

                LblMthTot.Text = total;
            }
            else
            {
                LblMthTot.Text = "";
            }

            Sql = "SELECT COUNT(*) TotCount FROM LibUsers WHERE UserCat ='Student' AND Exit_Time ='' AND Lib_Code ='" + Libcode + "' AND Entry_Date ='" + fromdate + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                string total = Convert.ToString(dsload.Tables[0].Rows[0]["TotCount"]);

                LblStuIn.Text = total;
            }
            else
            {
                LblStuIn.Text = "";
            }

            Sql = "SELECT COUNT(*) TotCount FROM LibUsers WHERE UserCat ='Staff' AND Exit_Time ='' AND Lib_Code ='" + Libcode + "' AND Entry_Date ='" + fromdate + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                string total = Convert.ToString(dsload.Tables[0].Rows[0]["TotCount"]);
                LblStaffIn.Text = total;
            }
            else
            {
                LblStaffIn.Text = "";
            }

            Sql = "SELECT COUNT(*) TotCount FROM LibUsers WHERE UserCat ='Visitor' AND Exit_Time ='' AND Lib_Code ='" + Libcode + "' AND Entry_Date ='" + fromdate + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                string total = Convert.ToString(dsload.Tables[0].Rows[0]["TotCount"]);
                LblVisitIn.Text = total;
            }
            else
            {
                LblVisitIn.Text = "";
            }

            Sql = "SELECT COUNT(*) TotCount FROM LibUsers WHERE UserCat ='Student' AND Exit_Time <>'' AND Lib_Code ='" + Libcode + "' AND Entry_Date ='" + fromdate + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                string total = Convert.ToString(dsload.Tables[0].Rows[0]["TotCount"]);
                LblStuOut.Text = total;
            }
            else
            {
                LblStuOut.Text = "";
            }

            Sql = "SELECT COUNT(*) TotCount FROM LibUsers WHERE UserCat ='Staff' AND Exit_Time <>'' AND Lib_Code ='" + Libcode + "' AND Entry_Date ='" + fromdate + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                string total = Convert.ToString(dsload.Tables[0].Rows[0]["TotCount"]);
                LblStaffOut.Text = total;
            }
            else
            {
                LblStaffOut.Text = "";
            }

            Sql = "SELECT COUNT(*) TotCount FROM LibUsers WHERE UserCat ='Visitor' AND Exit_Time <>'' AND Lib_Code ='" + Libcode + "' AND Entry_Date ='" + fromdate + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                string total = Convert.ToString(dsload.Tables[0].Rows[0]["TotCount"]);
                LblVisitOut.Text = total;
            }
            else
            {
                LblVisitOut.Text = "";
            }
            double StuHitTotIn = Convert.ToDouble(LblStuIn.Text);
            double StuHitTotOut = Convert.ToDouble(LblStuOut.Text);
            double StuHitTotal = StuHitTotIn + StuHitTotOut;
            LblStuTotal.Text = Convert.ToString(StuHitTotal);

            double StaffHitTotIn = Convert.ToDouble(LblStaffIn.Text);
            double StaffHitTotOut = Convert.ToDouble(LblStaffOut.Text);
            double StaffHitTotal = StaffHitTotIn + StaffHitTotOut;
            LblStaffTotal.Text = Convert.ToString(StaffHitTotal);

            double VisitHitTotIn = Convert.ToDouble(LblVisitIn.Text);
            double VisitHitTotOut = Convert.ToDouble(LblVisitOut.Text);
            double VisitHitTotal = VisitHitTotIn + VisitHitTotOut;
            LblVisitTotal.Text = Convert.ToString(VisitHitTotal);

            double TotalIn = StuHitTotIn + StaffHitTotIn + VisitHitTotIn;
            Lbl_TotIn.Text = Convert.ToString(TotalIn);

            double TotalOut = StuHitTotOut + StaffHitTotOut + VisitHitTotOut;
            Lbl_TotOut.Text = Convert.ToString(TotalOut);

            double Total = StuHitTotal + StaffHitTotal + VisitHitTotal;
            Lbl_TotStrength.Text = Convert.ToString(Total);
        }
        catch (Exception ex)
        {
        }
    }

    protected void Txt_SmartCardID_OnTextChanged(object sender, EventArgs e)
    {
        if (Txt_SmartCardID.Text != "")
        {
            Txt_UserID.Text = d2.GetFunction("SELECT Roll_No from registration where smart_serial_no ='" + Txt_SmartCardID.Text + "' ");
            if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
            {
                Txt_UserID.Text = d2.GetFunction("SELECT Staff_Code from StaffMaster where smart_seriel_no ='" + Txt_SmartCardID.Text + "' ");
                if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                {
                    Txt_UserID.Text = d2.GetFunction("SELECT Roll_No from registration where roll_no ='" + Txt_SmartCardID.Text + "' ");
                    if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                    {
                        Txt_UserID.Text = d2.GetFunction("SELECT Roll_No from registration where lib_id ='" + Txt_SmartCardID.Text + "' ");
                        if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                        {
                            Txt_UserID.Text = d2.GetFunction("SELECT Staff_Code from StaffMaster where staff_code ='" + Txt_SmartCardID.Text + "' ");
                            if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                            {
                                Txt_UserID.Text = d2.GetFunction("SELECT Staff_Code from StaffMaster where lib_id ='" + Txt_SmartCardID.Text + "' ");
                                if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                                {
                                    Txt_UserID.Text = d2.GetFunction("SELECT User_ID from User_Master where User_ID ='" + Txt_SmartCardID.Text + "' ");
                                }
                            }
                        }
                    }
                }
            }
            if (Txt_UserID.Text != "")
            {
                Txt_UserID_OnTextChanged(sender, e);
                Txt_SmartCardID.Text = "";
            }
            else
            {
                // InfoMsg "No Member"
                if (Txt_SmartCardID.Text != "")
                {
                    Txt_SmartCardID.Text = "";
                    //Txt_SmartCardID.SetFocus
                }
            }
        }
    }

    protected void VisibleAll(int IntMemType)
    {
        img_stud1.Visible = true;
        if (IntMemType == 1)
        {
            Txt_UserID.Visible = true;
            LblName.Visible = true;
            LblDept.Visible = true;
        }
        else if (IntMemType == 2)
        {
            Txt_UserID.Visible = true;
            LblName.Visible = true;
            LblDept.Visible = true;
        }
        else if (IntMemType == 3)
        {
            Txt_UserID.Visible = true;
            LblName.Visible = true;
            LblDept.Visible = true;
        }
    }

    protected void DisplayHead(string Libcode)
    {
        divWelcome.Visible = true;

        string Sql = "SELECT * FROM LibUsers WHERE Roll_No ='" + Txt_UserID.Text + "' AND Exit_Time = '' AND Lib_Code ='" + Libcode + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(Sql, "text");
        if (ds.Tables[0].Rows.Count == 0)
        {
            LblWel.Text = "Welcome";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HidePop();", true);

        }
        else
        {
            LblWel.Text = "Thank You";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HidePop();", true);
        }

        Txt_UserID.Text = "";
        Page.Form.DefaultFocus = Txt_UserID.ClientID;

    }

    protected void InVisibleAll()
    {
        LblName.Visible = false;
        LblDept.Visible = false;
        img_stud1.Visible = false;
        imgdiv2.Visible = false;
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        LblName.Text = "";
        Txt_UserID.Text = "";
        LblDept.Text = "";
        img_stud1.Visible = false;
        img_stud1.ImageUrl = "";
    }

    protected void Txt_UserID_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string UserId = Txt_UserID.Text;
            if (UserId != "")
            {
                Txt_UserID.Text = d2.GetFunction("SELECT Roll_No from registration where smart_serial_no ='" + UserId + "' ");
                if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                {
                    Txt_UserID.Text = d2.GetFunction("SELECT Staff_Code from StaffMaster where smart_seriel_no ='" + UserId + "' ");
                    if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                    {
                        Txt_UserID.Text = d2.GetFunction("SELECT Roll_No from registration where roll_no ='" + UserId + "' ");
                        if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                        {
                            Txt_UserID.Text = d2.GetFunction("SELECT Roll_No from registration where lib_id ='" + UserId + "' ");
                            if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                            {
                                Txt_UserID.Text = d2.GetFunction("SELECT Staff_Code from StaffMaster where staff_code ='" + UserId + "' ");
                                if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                                {
                                    Txt_UserID.Text = d2.GetFunction("SELECT Staff_Code from StaffMaster where lib_id ='" + UserId + "' ");
                                    if (Txt_UserID.Text == "" || Txt_UserID.Text == "0")
                                    {
                                        Txt_UserID.Text = d2.GetFunction("SELECT User_ID from User_Master where User_ID ='" + UserId + "' ");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            string Sql = string.Empty;
            string col_Code = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string Libcode = Convert.ToString(ddl_LibName.SelectedValue);
            if (Txt_UserID.Text != "")
            {

            }

            int IntMemType = 0;
            int IntIs_Staff = 0;
            if (Txt_UserID.Text != "" && Txt_UserID.Text != "0")
            {
                Sql = "SELECT R.Roll_No,App_No,R.Stud_Name,Course_Name+' - '+Dept_Name Course_Name,Current_Semester FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code AND (R.Roll_No ='" + Txt_UserID.Text + "' OR R.Reg_No ='" + Txt_UserID.Text + "' OR R.Lib_ID ='" + Txt_UserID.Text + "') ";

                if (BlnAllowMulColStud == false)
                {
                    Sql += "AND G.College_Code =" + col_Code + "";
                }
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(Sql, "text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    IntMemType = 1;
                    string rollNo = Convert.ToString(dsload.Tables[0].Rows[0]["Roll_No"]);
                    string Stud_Name = Convert.ToString(dsload.Tables[0].Rows[0]["Stud_Name"]);
                    string Current_Semester = Convert.ToString(dsload.Tables[0].Rows[0]["Current_Semester"]);
                    string Department = Convert.ToString(dsload.Tables[0].Rows[0]["Course_Name"]);

                    Txt_RollNo.Text = rollNo;
                    LblName.Text = Stud_Name;
                    Lbl_Semester.Text = Current_Semester;
                    LblDept.Text = Department;
                    img_stud1.Visible = true;
                    img_stud1.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollNo + " ";

                    DisplayHead(Libcode);
                    BtnUserSave_OnClick(IntMemType);
                    DisplayStatus(sender, e);
                    return;
                }
                else
                {
                    Sql = "SELECT M.Staff_Code,Appl_No,M.Staff_Name,Dept_Name FROM StaffMaster M,StaffTrans T,HrDept_Master D WHERE M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 AND (M.Staff_Code ='" + Txt_UserID.Text + "' OR M.Lib_ID ='" + Txt_UserID.Text + "') ";
                    if (BlnAllowMulColStud == false)
                    {
                        Sql += "AND M.College_Code =" + col_Code + "";
                    }

                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(Sql, "text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        IntMemType = 2;
                        VisibleAll(IntMemType);
                        string StaffCode = Convert.ToString(dsload.Tables[0].Rows[0]["Staff_Code"]);
                        string Staff_Name = Convert.ToString(dsload.Tables[0].Rows[0]["Staff_Name"]);
                        string Department = Convert.ToString(dsload.Tables[0].Rows[0]["Dept_Name"]);
                        Txt_RollNo.Text = StaffCode;
                        LblName.Text = Staff_Name;
                        Lbl_Semester.Text = "0";
                        LblDept.Text = Department;
                        img_stud1.Visible = true;
                        img_stud1.ImageUrl = "~/Handler/staffphoto.ashx?Staff_code=" + StaffCode + " ";
                        DisplayHead(Libcode);
                        BtnUserSave_OnClick(IntMemType);
                        DisplayStatus(sender, e);
                        return;
                    }
                    else
                    {
                        Sql = "SELECT User_ID,Name,Department,Is_Staff,membertype FROM User_Master WHERE User_ID ='" + Txt_UserID.Text + "' AND ISNULL(Status,0) = 1 ";
                        if (BlnAllowMulColStud == false)
                        {
                            Sql += "AND College_Code =" + col_Code + "";
                        }
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(Sql, "text");
                        if (dsload.Tables[0].Rows.Count > 0)
                        {
                            IntIs_Staff = Convert.ToInt32(dsload.Tables[0].Rows[0]["Is_Staff"]);
                            string memberType = Convert.ToString(dsload.Tables[0].Rows[0]["membertype"]);

                            if (memberType == "student")
                            {
                                IntMemType = 3;
                                LblName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Name"]);
                                string Department = Convert.ToString(dsload.Tables[0].Rows[0]["Department"]);
                                string userId = Convert.ToString(dsload.Tables[0].Rows[0]["User_ID"]);
                                Lbl_Semester.Text = "0";
                                Txt_RollNo.Text = userId;
                                LblDept.Text = Department;
                                img_stud1.Visible = true;
                                img_stud1.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + userId + " ";
                                DisplayHead(Libcode);
                                BtnUserSave_OnClick(IntMemType);
                                DisplayStatus(sender, e);
                                return;
                            }
                            if (memberType == "staff")
                            {
                                IntMemType = 3;
                                LblName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Name"]);
                                string Department = Convert.ToString(dsload.Tables[0].Rows[0]["Department"]);
                                string userId = Convert.ToString(dsload.Tables[0].Rows[0]["User_ID"]);
                                Lbl_Semester.Text = "0";
                                Txt_RollNo.Text = userId;
                                LblDept.Text = Department;
                                img_stud1.Visible = true;
                                img_stud1.ImageUrl = "~/Handler/staffphoto.ashx?Staff_code=" + userId + " ";
                                DisplayHead(Libcode);
                                BtnUserSave_OnClick(IntMemType);
                                DisplayStatus(sender, e);
                                return;
                            }
                            if (memberType == "visitor")
                            {
                                IntMemType = 3;
                                LblName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Name"]);
                                string Department = Convert.ToString(dsload.Tables[0].Rows[0]["Department"]);
                                string userId = Convert.ToString(dsload.Tables[0].Rows[0]["User_ID"]);
                                Lbl_Semester.Text = "0";
                                Txt_RollNo.Text = userId;
                                LblDept.Text = Department;
                                img_stud1.Visible = true;
                                img_stud1.ImageUrl = "~/Handler/VisitorPhoto.ashx?VisitorID=" + userId + " ";
                                DisplayHead(Libcode);
                                BtnUserSave_OnClick(IntMemType);
                                DisplayStatus(sender, e);
                                return;
                            }
                            VisibleAll(IntMemType);
                        }
                    }
                }
            }
            if (Txt_UserID.Text == "0")
            {
                Txt_UserID.Text = "";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_Ok_Click(object sender, EventArgs e)
    {

        if (Txt_Password.Text == "1947")
        {
            //Unload Me
        }
        else
        {
            lbl_alertMsg.Text = "You are not Authorized to Exit";
            //Txt_Password.SetFocus
        }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        DivExit.Visible = false;
    }

    protected void BtnUserSave_OnClick(int IntMemType)
    {
        try
        {
            string StrMemType = "";
            string Sql = "";
            int update = 0;
            int insert = 0;
            if (IntMemType == 1)
            {
                StrMemType = "Student";
            }
            if (IntMemType == 2)
            {
                StrMemType = "Staff";
            }
            if (IntMemType == 3)
            {
                StrMemType = "Visitor";
            }

            string libCode = Convert.ToString(ddl_LibName.SelectedValue);
            string libName = Convert.ToString(ddl_LibName.SelectedItem.Text);
            string Date = LblDate.Text;
            string[] frdate = Date.Split('/');
            if (frdate.Length == 3)
                Date = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();

            if (IntMemType == 1 || IntMemType == 2)
            {
                Sql = "SELECT * FROM LibUsers WHERE Roll_No ='" + Txt_RollNo.Text + "' AND Exit_Time ='' AND Lib_Code ='" + libCode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Sql = "UPDATE LibUsers SET Exit_Time ='" + txt_time.Text + "' WHERE Roll_No ='" + Txt_RollNo.Text + "' AND Exit_Time ='' AND Lib_Code ='" + libCode + "'";
                    update = d2.update_method_wo_parameter(Sql, "TEXT");
                }
                else
                {
                    Sql = "INSERT INTO LibUsers(roll_no,stud_name,dept_name,current_semester,entry_date,entry_time,exit_time,usercat,lib_code,visitor_details,IsManual) Values('" + Txt_RollNo.Text + "','" + LblName.Text + "','" + LblDept.Text + "'," + Lbl_Semester.Text + ",'" + Date + "','" + txt_time.Text + "','','" + StrMemType + "','" + libCode + "','" + Txt_VisitorName.Text + "',0)";
                    insert = d2.update_method_wo_parameter(Sql, "TEXT");
                }
            }
            if (IntMemType == 3)
            {
                Sql = "SELECT * FROM LibUsers WHERE Roll_No ='" + Txt_RollNo.Text + "' AND Exit_Time ='' AND Lib_Code ='" + libCode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Sql = "UPDATE LibUsers SET Exit_Time ='" + txt_time.Text + "' WHERE Roll_No ='" + Txt_RollNo.Text + "' AND Exit_Time ='' AND Lib_Code ='" + libCode + "'";
                    update = d2.update_method_wo_parameter(Sql, "TEXT");
                }
                else
                {
                    Sql = "INSERT INTO LibUsers(roll_no,stud_name,dept_name,current_semester,entry_date,entry_time,exit_time,usercat,lib_code,visitor_details,IsManual) Values('" + Txt_RollNo.Text + "','" + LblName.Text + "','" + LblDept.Text + "'," + Lbl_Semester.Text + ",'" + Date + "','" + txt_time.Text + "','','" + StrMemType + "','" + libCode + "','" + Txt_VisitorName.Text + "',0)";
                    insert = d2.update_method_wo_parameter(Sql, "TEXT");
                }
            }


        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_Manual_OnClick(object sender, EventArgs e)
    {
        DivManualExit.Visible = true;
        divExitSpread.Visible = true;

        DateTime FromTime = DateTime.Now;
        MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
        if (FromTime.ToString("tt") == "AM")
        {
            am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
        }
        else
        {
            am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
        }
        TimeSelector1.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
        BtnSearch_Click(sender, e);
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int ChkCount = 0;
            string Sql = "";
            int update = 0;
            string libName = Convert.ToString(ddlLib_ManualExit.SelectedValue);
            DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
            foreach (GridViewRow gvrow in grdManualExit.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    ChkCount = 1;
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    string rollNo = Convert.ToString(grdManualExit.Rows[RowCnt].Cells[2].Text);
                    Sql = "UPDATE LibUsers SET Exit_Time ='" + F_time + "' WHERE Roll_No ='" + rollNo + "' AND Exit_Time = ''";
                    if (libName != "All")
                    {
                        Sql += "AND Lib_Code ='" + libName + "'";
                    }
                    update = d2.update_method_wo_parameter(Sql, "TEXT");
                }
            }
            if (ChkCount > 0)
            {
                DivManualExitok.Visible = true;
                LblManualOk.Text = "Selected Students are Updated Sucessfully";
            }
            else
            {
                lbl_alertMsg.Text = "Select Student";
                return;
            }
            DisplayStatus(sender, e);
        }

        catch (Exception ex)
        { }

    }

    protected void BtnManualOk_Click(object sender, EventArgs e)
    {
        BtnSearch_Click(sender, e);
        DivManualExitok.Visible = false;
    }

    //protected void grdManualExit_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    //{
    //    grdManualExit.PageIndex = e.NewPageIndex;
    //    BtnSearch_Click(sender, e);
    //}

    protected void grdManualExit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ((CheckBox)e.Row.FindControl("allchk")).Attributes.Add("onclick",
                "javascript:SelectAll('" +
                ((CheckBox)e.Row.FindControl("allchk")).ClientID + "')");
        }

    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql = "";
            Sql = "SELECT Roll_No,Stud_Name,Dept_Name,Entry_Date,Entry_Time FROM Libusers WHERE Exit_Time = ''";
            if (ddlLib_ManualExit.SelectedItem.Text != "All")
            {
                Sql += " AND Lib_Code ='" + ddlLib_ManualExit.SelectedValue + "'";
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                grdManualExit.DataSource = dsload;
                grdManualExit.DataBind();
                grdManualExit.Visible = true;
                for (int l = 0; l < grdManualExit.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdManualExit.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdManualExit.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdManualExit.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            grdManualExit.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            grdManualExit.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                            grdManualExit.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Left;
                        }
                    }
                }
            }
            else
            {
                grdManualExit.DataSource = null;
                grdManualExit.DataBind();
                grdManualExit.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void imagebtnPospopclose_Click(object sender, EventArgs e)
    {
        DivManualExit.Visible = false;
    }

    public void getLibPrivil()
    {
        try
        {
            Hashtable hsLibcode = new Hashtable();
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddl_collegename.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            if (singleuser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + UserCode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = group_user.Split(';');
                if (groupUser.Length > 0)
                {
                    if (groupUser.Length == 1)
                    {
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code=" + groupUser[0] + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
                    }
                    if (groupUser.Length > 1)
                    {
                        for (int i = 0; i < groupUser.Length; i++)
                        {
                            GrpUserVal = groupUser[i];
                            if (!GrpCode.Contains(GrpUserVal))
                            {
                                if (GrpCode == "")
                                    GrpCode = GrpUserVal;
                                else
                                    GrpCode = GrpCode + "','" + GrpUserVal;
                            }
                        }
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code in ('" + GrpCode + "')";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
                    }
                }

            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                libcodecollection = "WHERE lib_code IN (-1)";
                goto aa;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string codeCollection = Convert.ToString(ds.Tables[0].Rows[i]["lib_code"]);
                    if (!hsLibcode.Contains(codeCollection))
                    {
                        hsLibcode.Add(codeCollection, "LibCode");
                        if (libcodecollection == "")
                            libcodecollection = codeCollection;
                        else
                            libcodecollection = libcodecollection + "','" + codeCollection;
                    }
                }
            }
            //libcodecollection = Left(libcodecollection, Len(libcodecollection) - 1);
            libcodecollection = "WHERE lib_code IN ('" + libcodecollection + "')";
        aa:
            LibCollection = libcodecollection;
            LoadList(LibCollection);
            bindLibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    protected void LoadList(string LibCollection)
    {
        try
        {
            string coll_Code = Convert.ToString(ddl_collegename.SelectedValue);
            string Sql = "SELECT Lib_Code,Lib_Name,ISNULL(Librarian,'') Librarian FROM Library " + LibCollection + " AND College_Code =" + coll_Code + "";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblLibrarianName.Text = Convert.ToString(dsload.Tables[0].Rows[0]["Librarian"]);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_Exit_OnClick(object sender, EventArgs e)
    {
        imgPassWord.Visible = true;
        Page.Form.DefaultFocus = txtPassword.ClientID;
    }

    protected void btn_ExitScreenOk_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtPassword.Text == "1947")
            {
                Response.Redirect("~/LibraryMod/LibraryHome.aspx");      
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alertMsg.Text = "You are not Authorized to Exit";
            }
        }
        catch (Exception ex)
        {
        }

    }

    protected void btn_ExitScreen_Click(object sender, EventArgs e)
    {
        imgPassWord.Visible = false;
    }

}