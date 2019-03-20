using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;


public partial class LibraryMod_No_Dues : System.Web.UI.Page
{
    Hashtable has = new Hashtable();
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    DataTable dtCommon = new DataTable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable columnhash = new Hashtable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    bool flag_true = false;
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    Boolean Cellclick = false;
    DataTable bohdues = new DataTable();
    DataRow drnodues;
    DataRow dradd;
    DataTable bokadd = new DataTable();
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
           
                userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
           

            if (!IsPostBack)
            {
                Bindcollege();
                getLibPrivil();
                bindbatch();
                bindbranch();
                binddegree();
                dues();
                Status();
                userentry();
                grdNoDues.Visible = false;
                //rptprint.Visible = false;
                txt_from.Attributes.Add("readonly", "readonly");
                txt_from.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Txtto.Attributes.Add("readonly", "readonly");
                Txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtissued.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    #region Binding Methods

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
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
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;




            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }

    }

    public void BindLibrary(string LibCodeCollection)
    {
        try
        {
            ddllib.Items.Clear();
            ds.Clear();
            string College = Convert.ToString(ddlCollege.SelectedValue);
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                string lib = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCodeCollection + " AND  college_code='" + College + "' ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(lib, "text");               
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    ddllib.DataSource = ds;
                    ddllib.DataTextField = "lib_name";
                    ddllib.DataValueField = "lib_code";
                    ddllib.DataBind();

                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();

            ds = dirAcc.selectDataSet("select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>'' and college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' order by batch_year desc");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddlbatch.DataSource = ds;
                    ddlbatch.DataTextField = "batch_year";
                    ddlbatch.DataValueField = "batch_year";
                    ddlbatch.DataBind();
                    ddlbatch.SelectedIndex = 0;
                    ddlbatch.Items.Insert(0, "All");


                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    public void binddegree()
    {
        try
        {
            ddlcourse.Items.Clear();
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    ddlcourse.DataSource = ds;
                    ddlcourse.DataTextField = "course_name";
                    ddlcourse.DataValueField = "course_id";
                    ddlcourse.DataBind();
                    // ddlcourse.SelectedIndex = 0;
                    ddlcourse.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    public void bindbranch()
    {
        try
        {
            has.Clear();
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("course_id", ddlcourse.SelectedValue);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_branch", has, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataTextField = "dept_name";
                    ddldept.DataValueField = "degree_code";
                    ddldept.DataBind();
                }
            }
            ddldept.SelectedIndex = 0;
            ddldept.Items.Insert(0, "All");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    public void dues()
    {
        try
        {
            ddlnodues.Items.Add("Issued");
            ddlnodues.Items.Add("Pending");
            ddlnodues.Items.Add("Both");
            ddlnodues.Items.FindByText("Both").Selected = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    public void Status()
    {
        try
        {
            ddlstatus.Items.Add("Admitted Students");
            ddlstatus.Items.Add("Discontinued Students");
            ddlstatus.Items.Add("All");
            ddlstatus.Items.FindByText("All").Selected = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    public void userentry()
    {
        try
        {
            ddluserentry.Items.Add("Roll Number");
            ddluserentry.Items.Add("Library ID");
            ddluserentry.Items.Add("Register Number");
            ddluserentry.Items.Add("Admission Number");
            //ddluserentry.Items.Add("Discontinued Students");
            //ddluserentry.Items.Add("Discontinued Students");
            //ddluserentry.Items.Add("All");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    #endregion

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
            if (Chkbatch.Checked == true)
            {
                //bindbatch();
                //binddegree();
                //bindbranch();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }


    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //binddegree();
            //bindbranch();

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }

    }

    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }


    }

    protected void Cboldsearch_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (Cboldsearch.Checked == true)
            {
                Txtto.Enabled = true;
                txt_from.Enabled = true;
            }
            else
            {
                Txtto.Enabled = false;
                txt_from.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    protected void Chkbatch_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (Chkbatch.Checked == true)
            {
                //ddlbatch.Enabled = true;
                //ddlcourse.Enabled = true;
                //ddldept.Enabled = true;
                //bindbatch();
                //binddegree();
                //bindbranch();
            }
            else
            {
                //ddlbatch.Enabled = false;
                //ddlcourse.Enabled = false;
                //ddldept.Enabled = false;
                //ddlbatch.Items.Clear();
                //ddlcourse.Items.Clear();
                //ddldept.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    protected void Chkissued_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (Chkissued.Checked == true)
            {
                txtissued.Enabled = true;


            }
            else
            {
                txtissued.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    protected void Go_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql = string.Empty;
            DataSet bookallo = new DataSet();
            grdNoDues.Visible = true;



            string dateissu = txt_from.Text;
            string issuedate = string.Empty;
            if (dateissu != "")
            {
                string[] spl = dateissu.Split('/');
                if (spl.Length > 2)
                {
                    string mon = spl[1];
                    string day = spl[0];
                    string yea = spl[2];
                    issuedate = spl[2] + '-' + spl[1] + '-' + spl[0];
                }

            }

            string dateissu1 = Txtto.Text;
            string issuedate1 = string.Empty;
            if (dateissu1 != "")
            {
                string[] spl1 = dateissu1.Split('/');
                if (spl1.Length > 2)
                {
                    string mon1 = spl1[1];
                    string day1 = spl1[0];
                    string yea1 = spl1[2];
                    issuedate1 = spl1[2] + '-' + spl1[1] + '-' + spl1[0];
                }

            }

            if (rdbstudent.Checked == true)
            {
                Sql = "SELECT '' AS 'Select',D.Code,D.Roll_No,Stud_Name as Name,Course_Name + '-' + Dept_Name as Degree,";
                Sql = Sql + "CASE WHEN No_Dues = 1 THEN Issued_Date ELSE '' END Issued_Date,";
                Sql = Sql + "CASE WHEN No_Dues = 0 THEN 'No' ELSE 'YES' END No_Dues,ISNULL(D.Remarks,'') Remarks,0 AS Is_Staff ";
                Sql = Sql + "FROM LibNoDues_Tbl D,Registration R,Degree G,Course c,Department P ";
                Sql = Sql + "Where 1 = 1 ";
                Sql = Sql + "AND D.Roll_No = R.Roll_No AND R.Degree_Code = G.Degree_Code ";
                Sql = Sql + "AND G.Course_ID = C.Course_ID AND G.Dept_Code = P.Dept_Code ";
                Sql = Sql + "AND Is_Staff = 0 ";
                if (Convert.ToString(ddlCollege.SelectedValue) != "")
                    Sql = Sql + "AND G.College_Code ='" + Convert.ToString(ddlCollege.SelectedValue) + "' ";
                if (Convert.ToString(ddllib.SelectedValue) != "")
                    Sql = Sql + "AND D.Lib_Code ='" + Convert.ToString(ddllib.SelectedValue) + "'";
                if (Cboldsearch.Checked == true)
                    Sql = Sql + "AND Issued_Date Between '" + issuedate + "' AND '" + issuedate1 + "' ";
                if (Convert.ToString(ddlnodues.SelectedItem) == "Issued")
                    Sql = Sql + "AND No_Dues ='True' ";
                if (Convert.ToString(ddlnodues.SelectedItem) == "Pending")
                    Sql = Sql + "AND No_Dues ='False' ";

                if (Chkbatch.Checked == true)
                {

                    string typ = string.Empty;
                    if (ddlbatch.Items.Count > 0)
                    {
                        for (int i = 0; i < ddlbatch.Items.Count - 1; i++)
                        {
                            if (Convert.ToString(ddlbatch.SelectedItem) == "All")
                            {
                                if (typ == "")
                                {
                                    typ = "" + ddlbatch.Items[i + 1].Value + "";
                                }
                                else
                                {
                                    typ = typ + "'" + "," + "'" + ddlbatch.Items[i + 1].Value + "";
                                }
                            }
                            else
                                typ = ddlbatch.SelectedValue;
                        }
                    }
                    string typ1 = string.Empty;
                    if (ddlcourse.Items.Count > 0)
                    {
                        for (int i = 0; i < ddlcourse.Items.Count - 1; i++)
                        {
                            if (Convert.ToString(ddlcourse.SelectedItem) == "All")
                            {
                                if (typ1 == "")
                                {
                                    typ1 = "" + ddlcourse.Items[i + 1].Value + "";
                                }
                                else
                                {
                                    typ1 = typ1 + "'" + "," + "'" + ddlcourse.Items[i + 1].Value + "";
                                }
                            }
                            else
                                typ1 = ddlcourse.SelectedValue;
                        }
                    }
                    string typ2 = string.Empty;
                    if (ddldept.Items.Count > 0)
                    {
                        for (int i = 0; i < ddldept.Items.Count - 1; i++)
                        {
                            if (Convert.ToString(ddldept.SelectedItem) == "All")
                            {
                                if (typ2 == "")
                                {
                                    typ2 = "" + ddldept.Items[i + 1].Value + "";
                                }
                                else
                                {
                                    typ2 = typ2 + "'" + "," + "'" + ddldept.Items[i + 1].Value + "";
                                }
                            }
                            else
                                typ2 = ddldept.SelectedValue;
                        }
                    }
                    if (Convert.ToString(ddlbatch.SelectedValue) != "")
                        Sql = Sql + "AND Batch_Year in('" + typ + "')";
                    if (Convert.ToString(ddlcourse.SelectedValue) != "")
                        Sql = Sql + "AND G.Course_ID in('" + typ1 + "')";
                    if (Convert.ToString(ddldept.SelectedValue) != "")
                        Sql = Sql + "AND G.Dept_Code in('" + typ2 + "')";
                }
                if (Convert.ToString(ddlstatus.SelectedItem) == "Admitted Students")
                    Sql = Sql + " AND R.DelFlag = 0 AND Exam_Flag = 'OK' ";
                if (Convert.ToString(ddlstatus.SelectedItem) == "Discontinued Students")
                    Sql = Sql + " AND (R.DelFlag <> 0 OR Exam_Flag = 'DEBAR') ";


            }

            else if (rdbstaff.Checked == true)
            {
                Sql = "SELECT '' AS 'Select',D.Code,D.Roll_No,Staff_Name as Name,Dept_Name as Degree,";
                Sql = Sql + "CASE WHEN No_Dues = 1 THEN Issued_Date ELSE '' END Issued_Date,";
                Sql = Sql + "CASE WHEN No_Dues = 0 THEN 'No' ELSE 'YES' END No_Dues,ISNULL(D.Remarks,'') Remarks,1 AS Is_Staff ";
                Sql = Sql + "FROM LibNoDues_Tbl D,StaffMaster M,StaffTrans T,Department P ";
                Sql = Sql + "Where 1 = 1 ";
                Sql = Sql + "AND D.Roll_No = M.Staff_Code AND M.Staff_Code = T.Staff_Code ";
                Sql = Sql + "AND T.Dept_Code = P.Dept_Code ";
                Sql = Sql + "AND Is_Staff = 1 AND T.Latestrec = 1 ";
                if (Convert.ToString(ddllib.SelectedValue) != "")
                    Sql = Sql + "AND D.Lib_Code ='" + Convert.ToString(ddllib.SelectedValue) + "'";
                if (Cboldsearch.Checked == true)
                    Sql = Sql + "AND Issued_Date Between '" + txt_from.Text + "' AND '" + Txtto.Text + "' ";
                if (Convert.ToString(ddlnodues.SelectedItem) == "Issued")
                    Sql = Sql + "AND No_Dues ='True' ";
                if (Convert.ToString(ddlnodues.SelectedItem) == "Pending")
                    Sql = Sql + "AND No_Dues ='False' ";
            }
            else
            {
                Sql = "SELECT '' AS 'Select',D.Code,D.Roll_No,Stud_Name as Name,Course_Name + '-' + Dept_Name as Degree,";
                Sql = Sql + "CASE WHEN No_Dues = 1 THEN Issued_Date ELSE '' END Issued_Date,";
                Sql = Sql + "CASE WHEN No_Dues = 0 THEN 'No' ELSE 'YES' END No_Dues,ISNULL(D.Remarks,'') Remarks,0 AS Is_Staff ";
                Sql = Sql + "FROM LibNoDues_Tbl D,Registration R,Degree G,Course c,Department P ";
                Sql = Sql + "Where 1 = 1 ";
                Sql = Sql + "AND D.Roll_No = R.Roll_No AND R.Degree_Code = G.Degree_Code ";
                Sql = Sql + "AND G.Course_ID = C.Course_ID AND G.Dept_Code = P.Dept_Code ";
                Sql = Sql + "AND Is_Staff = 0 ";
                if (Convert.ToString(ddlCollege.SelectedValue) != "")
                    Sql = Sql + "AND G.College_Code ='" + Convert.ToString(ddlCollege.SelectedValue) + "' ";
                if (Convert.ToString(ddllib.SelectedValue) != "")
                    Sql = Sql + "AND D.Lib_Code ='" + Convert.ToString(ddllib.SelectedValue) + "'";
                if (Cboldsearch.Checked == true)
                    Sql = Sql + "AND Issued_Date Between '" + txt_from.Text + "' AND '" + Txtto.Text + "' ";
                if (Convert.ToString(ddlnodues.SelectedItem) == "Issued")
                    Sql = Sql + "AND No_Dues ='True' ";
                if (Convert.ToString(ddlnodues.SelectedItem) == "Pending")
                    Sql = Sql + "AND No_Dues ='False' ";

                if (Chkbatch.Checked == true)
                {
                    if (Convert.ToString(ddlbatch.SelectedValue) != "")
                        Sql = Sql + "AND Batch_Year ='" + Convert.ToString(ddlbatch.SelectedValue) + "' ";
                    if (Convert.ToString(ddlcourse.SelectedValue) != "")
                        Sql = Sql + "AND G.Course_ID ='" + Convert.ToString(ddlcourse.SelectedValue) + "'";
                    if (Convert.ToString(ddldept.SelectedValue) != "")
                        Sql = Sql + "AND G.Dept_Code ='" + Convert.ToString(ddldept.SelectedValue) + "'";
                }
                if (Convert.ToString(ddlstatus.SelectedItem) == "Admitted Students")
                    Sql = Sql + " AND R.DelFlag = 0 AND Exam_Flag = 'OK' ";
                if (Convert.ToString(ddlstatus.SelectedItem) == "Discontinued Students")
                    Sql = Sql + " AND (R.DelFlag <> 0 OR Exam_Flag = 'DEBAR') ";

                Sql = Sql + " UNION ALL ";

                Sql = Sql + "SELECT '' AS 'Select',D.Code,D.Roll_No,Staff_Name as Name,Dept_Name as Degree,";
                Sql = Sql + "CASE WHEN No_Dues = 1 THEN Issued_Date ELSE '' END Issued_Date,";
                Sql = Sql + "CASE WHEN No_Dues = 0 THEN 'No' ELSE 'YES' END No_Dues,ISNULL(D.Remarks,'') Remarks,1 AS Is_Staff ";
                Sql = Sql + "FROM LibNoDues_Tbl D,StaffMaster M,StaffTrans T,Department P ";
                Sql = Sql + "Where 1 = 1 ";
                Sql = Sql + "AND D.Roll_No = M.Staff_Code AND M.Staff_Code = T.Staff_Code ";
                Sql = Sql + "AND T.Dept_Code = P.Dept_Code ";
                Sql = Sql + "AND Is_Staff = 1 AND T.Latestrec = 1 ";
                if (Convert.ToString(ddllib.SelectedValue) != "")
                    Sql = Sql + "AND D.Lib_Code ='" + Convert.ToString(ddllib.SelectedValue) + "'";
                if (Cboldsearch.Checked == true)
                    Sql = Sql + "AND Issued_Date Between '" + txt_from.Text + "' AND '" + Txtto.Text + "' ";
                if (Convert.ToString(ddlnodues.SelectedItem) == "Issued")
                    Sql = Sql + "AND No_Dues ='True' ";
                if (Convert.ToString(ddlnodues.SelectedItem) == "Pending")
                    Sql = Sql + "AND No_Dues ='False' ";
            }
            int sno = 0;
            bookallo = d2.select_method_wo_parameter(Sql, "Text");

            bohdues.Columns.Add("SNo", typeof(string));
            bohdues.Columns.Add("Roll No", typeof(string));
            bohdues.Columns.Add("Student Name", typeof(string));
            bohdues.Columns.Add("Department", typeof(string));
            bohdues.Columns.Add("Issued Date", typeof(string));
            bohdues.Columns.Add("Issued", typeof(string));
            bohdues.Columns.Add("Remark", typeof(string));

           

            if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < bookallo.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drnodues = bohdues.NewRow();
                    int m = i;


                    //Fpspread.Sheets[0].AutoPostBack = false;
                    drnodues["SNo"] = Convert.ToString(sno);
                    drnodues["Roll No"] = Convert.ToString(bookallo.Tables[0].Rows[i]
["Roll_No"]);
                    drnodues["Student Name"] = Convert.ToString(bookallo.Tables[0].Rows[i]["Name"]);
                    drnodues["Department"] = Convert.ToString(bookallo.Tables[0].Rows[i]
["Degree"]);
                    if (Convert.ToString(bookallo.Tables[0].Rows[i]["Issued_Date"]) != "1/1/1900 12:00:00 AM")
                    {
                        string date = Convert.ToString(bookallo.Tables[0].Rows[i]["Issued_Date"]);
                        if (date != "")
                        {
                            string[] split = date.Split();
                            if (split.Length > 2)
                            {
                                date = split[0];
                                string[] issue = date.Split('/');
                                date = issue[1] + '/' + issue[0] + '/' + issue[2];
                                drnodues["Issued Date"] = date;
                            }
                        }
                    }
                    else
                        drnodues["Issued Date"] = "";

                    drnodues["Issued"] = Convert.ToString(bookallo.Tables[0].Rows[i]["No_Dues"]);
                    drnodues["Remark"] = Convert.ToString(bookallo.Tables[0].Rows[i]["Remarks"]);
                    bohdues.Rows.Add(drnodues);

                }
                chkGridSelectAll.Visible = true;
                grdNoDues.DataSource = bohdues;
                grdNoDues.DataBind();
               
                grdNoDues.Visible = true;
                print.Visible = true;
                for (int l = 0; l < grdNoDues.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdNoDues.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdNoDues.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdNoDues.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            grdNoDues.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            grdNoDues.Rows[l].Cells[5].HorizontalAlign = HorizontalAlign.Center;

                            grdNoDues.Rows[l].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
              
               
            }
            else
            {
                grdNoDues.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No record found";
                 print.Visible = false;
            }

        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    protected void grdNoDues_RowDataBound(object sender, GridViewRowEventArgs e)
    {

       
    }

 

    #region Spread_cellclick

    protected void grdNoDues_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdNoDues_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetupdatebook = new DataSet();
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            if (Convert.ToString(rowIndex) != "-1" && Convert.ToString(rowIndex) != "")
            {
                string roll = Convert.ToString(grdNoDues.Rows[rowIndex].Cells[2].Text);
                txt_rollno.Text = roll;
                Cellclick = true;
                add_Click(sender, e);
                DivDueList.Visible = true;
                grdNoDuesForm.Visible = true;
            }
        }


        catch
        {
        }
    }

    protected void grdNoDues_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdNoDues.PageIndex = e.NewPageIndex;
        Go_Click(sender, e);
    }

    #endregion

    protected void add_Click(object sender, EventArgs e)
    {
        try
        {
            nodues.Visible = true;
            DivDueList.Visible = true;

            bokadd.Columns.Add("AccessNo", typeof(string));
            bokadd.Columns.Add("CallNo", typeof(string));
            bokadd.Columns.Add("Title", typeof(string));
            bokadd.Columns.Add("Author", typeof(string));
            bokadd.Columns.Add("IssuedStaff", typeof(string));
            bokadd.Columns.Add("DueDays", typeof(string));
            bokadd.Columns.Add("DueDate", typeof(string));
            bokadd.Columns.Add("Fine", typeof(string));
            bokadd.Columns.Add("LibraryName", typeof(string));


            if (Cellclick == true)
            {
                txt_rollno_TextChanged(sender, e);
            }
            else
            {
                txt_rollno.Text = "";
                txtname.Text = "";
                txtdept.Text = "";
                txtsem.Text = "";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    protected void txt_rollno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string Sql = string.Empty;
            string StrBtn_NoDues = "";
            string StrTokCode = "";
            Double DblFine = 0.0;
            DataSet txttable = new DataSet();
            DataSet txttable1 = new DataSet();
            string fine = "";
            string colcode = Convert.ToString(ddlCollege.SelectedValue);

            if (Convert.ToString(ddluserentry.SelectedItem) == "Roll Number")
            {
                if (rdbstu.Checked == true)
                {
                    Sql = "SELECT App_No,Roll_No,Stud_Name,Course_Name+'-'+Dept_Name Dept_Name,Current_Semester,G.Course_ID,G.Dept_Code,DelFlag,Exam_Flag ";
                    Sql = Sql + "FROM Registration R,Degree G,Course C,Department D ";
                    Sql = Sql + "WHERE R.Degree_Code = G.Degree_Code ";
                    Sql = Sql + "AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code ";
                    Sql = Sql + "AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code ";
                    Sql = Sql + "AND Roll_No ='" + txt_rollno.Text + "' ";
                    txttable = d2.select_method_wo_parameter(Sql, "text");
                    txtname.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Stud_Name"]);
                    txtdept.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Name"]);
                    txtsem.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Current_Semester"]);
                    image2.ImageUrl = "Handler/Handler4.ashx?rollno=" + txt_rollno.Text;
                    StrTokCode = Convert.ToString(txttable.Tables[0].Rows[0]["Course_ID"]) + "~" + Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Code"]);
                    
                }
                if (rdbstaff_stu.Checked == true)
                {
                    Sql = "SELECT M.Staff_Code,Staff_Name,Dept_Name ";
                    Sql = Sql + "FROM StaffMaster M,StaffTrans T,HrDept_Master D ";
                    Sql = Sql + "WHERE M.Staff_Code = T.Staff_Code AND T.Latestrec = 1 ";
                    Sql = Sql + "AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code ";
                    Sql = Sql + "AND M.Staff_Code ='" + txt_rollno.Text + "' ";
                    Sql = Sql + " AND resign = 0 AND settled = 0 ";
                    txttable = d2.select_method_wo_parameter(Sql, "text");
                    txtname.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Staff_Name"]);
                    txtdept.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Name"]);
                    image2.ImageUrl = "Handler/Handler4.ashx?rollno=" + txt_rollno.Text;
                }
            }
            else if (Convert.ToString(ddluserentry.SelectedItem) == "Library ID")
            {
                if (rdbstu.Checked == true)
                {
                    Sql = "SELECT App_No,Roll_No,Stud_Name,Course_Name+'-'+Dept_Name Dept_Name,Current_Semester,G.Course_ID,G.Dept_Code ";
                    Sql = Sql + "FROM Registration R,Degree G,Course C,Department D ";
                    Sql = Sql + "WHERE R.Degree_Code = G.Degree_Code ";
                    Sql = Sql + "AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code ";
                    Sql = Sql + "AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code ";
                    Sql = Sql + "AND Lib_ID ='" + txt_rollno.Text + "' ";
                    txttable = d2.select_method_wo_parameter(Sql, "text");
                    txtname.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Stud_Name"]);
                    txtdept.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Name"]);
                    txtsem.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Current_Semester"]);
                    image2.ImageUrl = "Handler/Handler4.ashx?rollno=" + txt_rollno.Text;
                    StrTokCode = Convert.ToString(txttable.Tables[0].Rows[0]["Course_ID"]) + "~" + Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Code"]);
                }
                if (rdbstaff_stu.Checked == true)
                {
                    Sql = "SELECT M.Staff_Code,Staff_Name,Dept_Name ";
                    Sql = Sql + "FROM StaffMaster M,StaffTrans T,HrDept_Master D ";
                    Sql = Sql + "WHERE M.Staff_Code = T.Staff_Code AND T.Latestrec = 1 ";
                    Sql = Sql + "AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code ";
                    Sql = Sql + "AND M.Lib_ID ='" + txt_rollno.Text + "' ";
                    Sql = Sql + " AND resign = 0 AND settled = 0 ";
                    txttable = d2.select_method_wo_parameter(Sql, "text");
                    txtname.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Staff_Name"]);
                    txtdept.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Name"]);
                    image2.ImageUrl = "Handler/Handler4.ashx?rollno=" + txt_rollno.Text;
                }
            }
            else if (Convert.ToString(ddluserentry.SelectedItem) == "Register Number")
            {
                if (rdbstu.Checked == true)
                {
                    Sql = "SELECT App_No,Roll_No,Stud_Name,Course_Name+'-'+Dept_Name Dept_Name,Current_Semester,G.Course_ID,G.Dept_Code ";
                    Sql = Sql + "FROM Registration R,Degree G,Course C,Department D ";
                    Sql = Sql + "WHERE R.Degree_Code = G.Degree_Code ";
                    Sql = Sql + "AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code ";
                    Sql = Sql + "AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code ";
                    Sql = Sql + "AND Reg_No ='" + txt_rollno.Text + "' ";
                    txttable = d2.select_method_wo_parameter(Sql, "text");
                    txtname.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Stud_Name"]);
                    txtdept.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Name"]);
                    txtsem.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Current_Semester"]);
                    image2.ImageUrl = "Handler/Handler4.ashx?rollno=" + txt_rollno.Text;
                    StrTokCode = Convert.ToString(txttable.Tables[0].Rows[0]["Course_ID"]) + "~" + Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Code"]);
                }
            }
            else if (Convert.ToString(ddluserentry.SelectedItem) == "Admission Number")
            {
                if (rdbstu.Checked == true)
                {
                    Sql = "SELECT App_No,Roll_No,Stud_Name,Course_Name+'-'+Dept_Name Dept_Name,Current_Semester,G.Course_ID,G.Dept_Code ";
                    Sql = Sql + "FROM Registration R,Degree G,Course C,Department D ";
                    Sql = Sql + "WHERE R.Degree_Code = G.Degree_Code ";
                    Sql = Sql + "AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code ";
                    Sql = Sql + "AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code ";
                    Sql = Sql + "AND Roll_Admit ='" + txt_rollno.Text + "' ";
                    txttable = d2.select_method_wo_parameter(Sql, "text");
                    txtname.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Stud_Name"]);
                    txtdept.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Name"]);
                    txtsem.Text = Convert.ToString(txttable.Tables[0].Rows[0]["Current_Semester"]);
                    image2.ImageUrl = "Handler/Handler4.ashx?rollno=" + txt_rollno.Text;
                    StrTokCode = Convert.ToString(txttable.Tables[0].Rows[0]["Course_ID"]) + "~" + Convert.ToString(txttable.Tables[0].Rows[0]["Dept_Code"]);
                }
            }
            string typ1 = string.Empty;
            if (ddllibrary.Items.Count > 0)
                typ1 = Convert.ToString(ddllibrary.SelectedValue);
            Sql = "SELECT * FROM LibNoDues_Tbl D,Library L WHERE D.Lib_Code = L.Lib_Code AND Roll_No ='" + txt_rollno.Text + "' AND L.College_Code ='" + colcode + "'";
            if (ddllibrary.Text != "")
                Sql = Sql + " AND D.Lib_Code ='" + typ1 + "'";
            if (rdbstu.Checked == true)
                Sql = Sql + " AND Is_Staff = 0";
            else
                Sql = Sql + " AND Is_Staff = 1";
            txttable1.Clear();
            txttable1 = d2.select_method_wo_parameter(Sql, "Text");
            if (txttable1.Tables[0].Rows.Count > 0)
            {

                StrBtn_NoDues = "M";
                string nodues = Convert.ToString(txttable1.Tables[0].Rows[0]["No_Dues"]);
                if (nodues.ToUpper() == "TRUE")
                {
                    txtissued.Enabled = true;
                    Chkissued.Checked = true;
                    string Date = Convert.ToString(txttable1.Tables[0].Rows[0]["Issued_Date"]);
                    string[] adate1 = Date.Split('/');
                    if (adate1.Length == 3)
                        Date = adate1[1].ToString() + "/" + adate1[0].ToString() + "/" + adate1[2].ToString();
                    txtissued.Text = Date.Split(' ')[0];
                }
                else
                {
                    txtissued.Enabled = false;
                    Chkissued.Checked = false;
                }
                btnprintletter.Enabled = true;
            }
            else
            {
                StrBtn_NoDues = "N";
                btnprintletter.Enabled = false;
            }
            if (rdbstu.Checked == true)
            {
                fine = d2.GetFunction("SELECT Fine FROM Lib_Master WHERE Code ='" + StrTokCode + "' AND Is_Staff = 0 ");
                DblFine = Convert.ToDouble(fine);
            }
            else
            {
                fine = d2.GetFunction("SELECT Fine FROM Lib_Master WHERE Code ='" + txt_rollno.Text + "' AND Is_Staff = 1 ");
                DblFine = Convert.ToDouble(fine);
            }

            //'Due Books'
            Sql = "SELECT Acc_No,Call_No,Title,Author,Borrow_Date,Book_IssuedBy,DateDiff(Day,Borrow_Date,GetDate()) Due_Days,Due_Date,(DateDiff(Day,Borrow_Date,GetDate())) * '" + DblFine + "' as Fine,Lib_Name as LibraryName FROM Borrow B,Library L  WHERE 1=1 AND B.Lib_Code = L.Lib_Code AND Return_Flag = 0  AND Roll_No = '" + txt_rollno.Text + "'";
            if (ddllibrary.Text != "")
                Sql = Sql + " AND  B.Lib_Code ='" + typ1 + "'";
            if (rdbstu.Checked == true)
                Sql = Sql + " AND Is_Staff = 0";
            else
                Sql = Sql + " AND Is_Staff = 1";
            txttable1.Clear();
            txttable1 = d2.select_method_wo_parameter(Sql, "Text");
            if (txttable1.Tables[0].Rows.Count > 0)
            {

                bokadd.Columns.Add("CallNo", typeof(string));
                bokadd.Columns.Add("AccessNo", typeof(string));
                bokadd.Columns.Add("Title", typeof(string));
                bokadd.Columns.Add("Author", typeof(string));
                bokadd.Columns.Add("IssuedStaff", typeof(string));
                bokadd.Columns.Add("DueDays", typeof(string));
                bokadd.Columns.Add("DueDate", typeof(string));
                bokadd.Columns.Add("Fine", typeof(string));
                bokadd.Columns.Add("LibraryName", typeof(string));
                int sno = 0;
                for (int row = 0; row < txttable1.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    dradd = bokadd.NewRow();
                    string tokendetails = Convert.ToString(txttable1.Tables[0].Rows[row]["Borrow_Date"]);
                    string issuestaff = Convert.ToString(txttable1.Tables[0].Rows[row]["Book_IssuedBy"]);
                    DateTime Duedate = Convert.ToDateTime(txttable1.Tables[0].Rows[row]["Due_Date"]);
                    dradd["AccessNo"] = Convert.ToString(txttable1.Tables[0].Rows[row]["Acc_No"]).Trim();
                    dradd["CallNo"] = Convert.ToString(txttable1.Tables[0].Rows[row]["Call_No"]).Trim();
                    dradd["Title"] = Convert.ToString(txttable1.Tables[0].Rows[row]["Title"]).Trim();
                    dradd["Author"] = Convert.ToString(txttable1.Tables[0].Rows[row]["Author"]);
                    dradd["IssuedStaff"] = issuestaff;
                    dradd["DueDays"] = Convert.ToString(txttable1.Tables[0].Rows[row]["Due_Days"]);
                    dradd["DueDate"] = Duedate.ToString("dd/MM/yyyy");
                    dradd["Fine"] = Convert.ToString(txttable1.Tables[0].Rows[row]["Fine"]);
                    dradd["LibraryName"] = Convert.ToString(txttable1.Tables[0].Rows[row]["LibraryName"]);
                    bokadd.Rows.Add(dradd);
                }
                grdNoDuesForm.DataSource = bokadd;
                grdNoDuesForm.DataBind();
                grdNoDuesForm.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    protected void btn_Question_Bank_popup_Click(object sender, EventArgs e)
    {
        nodues.Visible = false;

    }

    protected void rdbstu_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdbstaff_stu.Checked == true)
                Lblroll.Text = "Staff Code";
            else
                Lblroll.Text = "Roll No";
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }   

    protected void save_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql = string.Empty;
            DataSet bookallo = new DataSet();
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            string time = DateTime.Now.ToString("h:mm tt");
            string dateissu = txtissued.Text;
            string issuedate = string.Empty;
           
            if (dateissu != "")
            {
                string[] spl = dateissu.Split('/');
                if (spl.Length > 2)
                {
                    string mon = spl[1];
                    string day = spl[0];
                    string yea = spl[2];
                    issuedate = spl[2] + '-' + spl[1] + '-' + spl[0];
                }
            }
            Sql = "SELECT * FROM LibNoDues_Tbl WHERE Roll_No ='" + txt_rollno.Text + "' AND Lib_Code ='" + Convert.ToString(ddllibrary.SelectedValue) + "' ";
            bookallo = d2.select_method_wo_parameter(Sql, "Text");
            if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
            {
                string noDueVal = Convert.ToString(bookallo.Tables[0].Rows[0]["No_Dues"]);
                if (noDueVal.ToLower() == "true")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No dues already given to this student";
                    return;
                }
            }

            if (grdNoDuesForm.Rows.Count > 0)
            {
                DivBooksDue.Visible = true;
                LblBookDue.Text = "Books was not returned, do you save ";
                //nodues.Visible = false;
                return;
            }
            int memtype = 0;
            int chknodues = 0;
            if (rdbstaff_stu.Checked == true)
                memtype = 1;
            else
                memtype = 0;
            if (Chkissued.Checked == true)
                chknodues = 1;
            else
                chknodues = 0;

            Sql = "INSERT INTO LibNoDues_Tbl(Access_Date,Access_Time,Issued_Date,Is_Staff,Roll_No,No_Dues,Lib_Code,User_Entry,Remarks)";
            Sql = Sql + "VALUES('" + date + "','" + time + "',";
            Sql = Sql + "'" + issuedate + "'," + memtype + ",";
            Sql = Sql + "'" + txt_rollno.Text + "','" + chknodues + "','" + Convert.ToString(ddllibrary.SelectedValue) + "','" + Convert.ToString(ddluserentry.SelectedItem) + "','" + Txt_Remarks.Text + "')";
            int ins1 = d2.update_method_wo_parameter(Sql, "Text");
            if (Chkissued.Checked == true)
            {
                btnprintletter.Enabled = true;
                Sql = "Update TokenDetails SET Is_Locked = 2,Reas_Loc ='No dues issued' ";
                Sql = Sql + "WHERE Roll_No ='" + txt_rollno.Text + "' AND Is_Locked = 0 ";
            }
            int ins = d2.update_method_wo_parameter(Sql, "Text");
            if (Chkissued.Checked == true)
            {
                DivSurePrint.Visible = true;
                LblSurePrint.Text = "No Dues has been saved successfully, Do you want to print letter";
                return;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Dues has been saved successfully";
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    protected void btnBooksDueYes_Click(object sender, EventArgs e)
    {
        string date = DateTime.Now.ToString("yyyy/MM/dd");
        string time = DateTime.Now.ToString("h:mm tt");
        string dateissu = txtissued.Text;
        string issuedate = string.Empty;
      
        if (dateissu != "")
        {
            string[] spl = dateissu.Split('/');
            if (spl.Length > 2)
            {
                string mon = spl[1];
                string day = spl[0];
                string yea = spl[2];
                issuedate = spl[2] + '-' + spl[1] + '-' + spl[0];
            }
        }
        int memtype = 0;
        int chknodues = 0;
        if (rdbstaff_stu.Checked == true)
            memtype = 1;
        else
            memtype = 0;
        if (Chkissued.Checked == true)
            chknodues = 1;
        else
            chknodues = 0;

        string Sql = "INSERT INTO LibNoDues_Tbl(Access_Date,Access_Time,Issued_Date,Is_Staff,Roll_No,No_Dues,Lib_Code,User_Entry,Remarks)";
        Sql = Sql + "VALUES('" + date + "','" + time + "',";
        Sql = Sql + "'" + issuedate + "'," + memtype + ",";
        Sql = Sql + "'" + txt_rollno.Text + "','" + chknodues + "','" + Convert.ToString(ddllibrary.SelectedValue) + "','" + Convert.ToString(ddluserentry.SelectedItem) + "','" + Txt_Remarks.Text + "')";
        int ins1 = d2.update_method_wo_parameter(Sql, "Text");
        if (Chkissued.Checked == true)
        {
            btnprintletter.Enabled = true;
            Sql = "Update TokenDetails SET Is_Locked = 2,Reas_Loc ='No dues issued' ";
            Sql = Sql + "WHERE Roll_No ='" + txt_rollno.Text + "' AND Is_Locked = 0 ";
        }
        int ins = d2.update_method_wo_parameter(Sql, "Text");
        if (Chkissued.Checked == true)
        {
            DivSurePrint.Visible = true;
            LblSurePrint.Text = "No Dues has been saved successfully, Do you want to print letter";
            return;
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "No Dues has been saved successfully";
        }

    }

    protected void btnBooksDueNo_Click(object sender, EventArgs e)
    {
        DivBooksDue.Visible = false;
    }

    protected void btnSurePrintYes_Click(object sender, EventArgs e)
    {
        printletter();
        DivSurePrint.Visible = false;
    }

    protected void btnSurePrintNo_Click(object sender, EventArgs e)
    {
        DivSurePrint.Visible = false;
        alertpopwindow.Visible = true;
        lblalerterr.Text = "No Dues has been saved successfully";

    }

    protected void update_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql = string.Empty;
            DataSet bookallo = new DataSet();
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            string time = DateTime.Now.ToString("h:mm tt");
            int chknodues = 0;
            if (Chkissued.Checked == true)
                chknodues = 1;
            else
                chknodues = 0;
            Sql = "UPDATE LibNoDues_Tbl SET Access_Date='" + date + "',Access_Time='" + time + "',";
            Sql = Sql + "Issued_Date ='" + txtissued.Text + "',No_Dues =" + chknodues + ",Remarks ='" + Txt_Remarks.Text + "' ";
            Sql = Sql + "WHERE Roll_No ='" + txt_rollno.Text + "'";
            Sql = Sql + "AND Lib_Code =" + Convert.ToString(ddllibrary.SelectedValue) + "";
            int ins1 = d2.update_method_wo_parameter(Sql, "Text");
            if (Chkissued.Checked == true)
            {
                btnprintletter.Enabled = true;
                Sql = "Update TokenDetails SET Is_Locked = 2,Reas_Loc ='No dues issued' ";
                Sql = Sql + "WHERE Roll_No ='" + txt_rollno.Text + "' AND Is_Locked = 0 ";
            }
            else
            {
                btnprintletter.Enabled = false;
                Sql = "Update TokenDetails SET Is_Locked = 0,Reas_Loc ='' ";
                Sql = Sql + "WHERE Roll_No ='" + txt_rollno.Text + "' AND Is_Locked = 2 AND Reas_Loc ='No dues issued' ";
            }
            int ins = d2.update_method_wo_parameter(Sql, "Text");
            if (Chkissued.Checked == true)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Dues has been saved successfully, Do you print letter";
                printletter();
            }
            else
            {
                nodues.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Dues has been updated successfully";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            nodues.Visible = false;
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
        nodues.Visible = true;
    }

    protected void printletter()
    {
        try
        {
            StringBuilder SbHtml = new StringBuilder();
            string clgaddress = string.Empty;
            string pincode = string.Empty;
            string collName = string.Empty;
            string VisitorName = "";
            string CompanyName = "";
            string GatePassDate = "";
            string MobileNo = "";
            string gateno = "";
            string intime = string.Empty;
            string outtime = string.Empty;
            string Purpose = string.Empty;
            string add1 = string.Empty;
            string city = string.Empty;
            string state = string.Empty;
            string dis = string.Empty;
            int pin = 0;
            string meet = string.Empty;
            string Deptm = string.Empty;
            string expectedtime = string.Empty;
            string department = txtdept.Text;
            string[] dept = department.Split('-');
            string course = dept[0];
            string degree = dept[1];
            string libName = ddllibrary.SelectedItem.Text;

            string strquery = "select *,district+' - '+pincode as districtpin,collname from collinfo where college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                pincode = Convert.ToString(ds.Tables[0].Rows[0]["pincode"]).Trim();
                collName = Convert.ToString(ds.Tables[0].Rows[0]["collname"]).Trim();
                clgaddress = Convert.ToString(ds.Tables[0].Rows[0]["address3"]) + " , " + Convert.ToString(ds.Tables[0].Rows[0]["district"]) + ((pin != 0) ? (" - " + pin.ToString()) : " - " + pincode);
            }
            string Sql = string.Empty;
            DataSet printds_new = new DataSet();
            string issuedate = string.Empty;
            string designation = "";
            if (rdbstu.Checked == true)
            {
                if (txt_rollno.Text != "")
                {
                    Sql = "SELECT DISTINCT B.Roll_No,Stud_Name,Course_Name,Dept_Name,ISNULL(Adm_Date,'') Adm_Date,ISNULL(End_Date,'') End_Date ";
                    Sql = Sql + "FROM LibNoDues_Tbl B ";
                    Sql = Sql + "INNER JOIN Registration R ON B.Roll_No = R.Roll_No ";
                    Sql = Sql + "INNER JOIN Degree G ON  G.Degree_Code = R.Degree_Code ";
                    Sql = Sql + "INNER JOIN Course C ON G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code ";
                    Sql = Sql + "INNER JOIN Department D ON G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code ";
                    Sql = Sql + "LEFT JOIN SemInfo I ON R.Degree_Code = I.Degree_Code AND R.Batch_Year = I.Batch_Year AND R.Current_Semester = I.Semester ";
                    Sql = Sql + "WHERE B.Roll_No ='" + txt_rollno.Text + "' ";
                    printds_new = da.select_method_wo_parameter(Sql, "Text");
                    {
                        string Date = Convert.ToString(printds_new.Tables[0].Rows[0]["Adm_Date"]).Trim();
                        string[] adate1 = Date.Split('/');
                        if (adate1.Length == 3)
                            Date = adate1[1].ToString() + "/" + adate1[0].ToString() + "/" + adate1[2].ToString();

                        string EndDate = Convert.ToString(printds_new.Tables[0].Rows[0]["End_Date"]).Trim();
                        string[] Joindate1 = EndDate.Split('/');
                        if (Joindate1.Length == 3)
                            EndDate = Joindate1[1].ToString() + "/" + Joindate1[0].ToString() + "/" + Joindate1[2].ToString();
                        issuedate = Date.Split(' ')[0] + " - " + EndDate.Split(' ')[0];
                        designation = "Student";
                    }
                }
            }
            else
            {
                if (txt_rollno.Text != "")
                {
                    Sql = "SELECT B.Roll_No,Staff_Name,Dept_Name,Join_Date ";
                    Sql = Sql + "FROM LibNoDues_Tbl B,StaffMaster M,StaffTrans T,HrDept_Master D ";
                    Sql = Sql + "WHERE B.Roll_No = M.Staff_Code ";
                    Sql = Sql + "AND M.Staff_Code = T.Staff_Code AND T.Latestrec = 1 ";
                    Sql = Sql + "AND T.Dept_Code = D.Dept_Code ";
                    Sql = Sql + "AND B.Roll_No ='" + txt_rollno.Text + "' ";
                    printds_new = da.select_method_wo_parameter(Sql, "Text");
                    {
                        issuedate = Convert.ToString(printds_new.Tables[0].Rows[0]["Join_Date"]).Trim();
                        designation = "Staff";
                    }
                }
            }

            #region I Page
            SbHtml.Append("<html>");
            SbHtml.Append("<body>");
            SbHtml.Append("<div style='height:513px; width: 664px; border:1px solid black; margin:0px; margin-left: 105px;page-break-after: always;'>");

            #region Header

            SbHtml.Append("<div style='width: 910px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
            SbHtml.Append("<font face='IDAutomationHC39M'size='4'>");
            SbHtml.Append("<div style='width: 945px; height: 5px; border: 0px solid black; margin:0px; margin-left: 370px;'>");
            SbHtml.Append("<span style='font-weight:bold;'width: 7px; height:5px; border: 0px solid Red'></span>");
            SbHtml.Append("</div>");
            SbHtml.Append("</font>");
            SbHtml.Append("<br/>");
            SbHtml.Append("<br/>");
            SbHtml.Append("<br/>");
            SbHtml.Append("<table cellspacing='0' cellpadding='5' border='0px' style='width: 645px; height:30px; font-weight: bold;'>");
            SbHtml.Append("<tr style='text-align:right;'>");
            SbHtml.Append("<td>");
            SbHtml.Append("</td>");
            SbHtml.Append("</tr>");
            SbHtml.Append("<tr>");
            SbHtml.Append("<td rowspan='3'><img src='" + "../college/Left_Logo.jpg" + "' style='height:80px; width:80px;'/></td>");
            SbHtml.Append("<td style='text-align:center;'>");
            SbHtml.Append("<span> " + collName + "</span>");
            SbHtml.Append("</td>");
            SbHtml.Append("<td rowspan='3'><img src='" + "../college/right_Logo.jpg" + "' style='height:80px; width:80px;'/></td>");
            SbHtml.Append("</tr>");
            SbHtml.Append("<tr style='text-align:center;'>");
            SbHtml.Append("<td>");
            SbHtml.Append("<span> " + clgaddress + "</span>");
            SbHtml.Append("</td>");
            SbHtml.Append("</tr>");
            SbHtml.Append("<tr><td style='text-align:center;'><span> " + libName.ToUpper() + "</span></td></tr>");
            SbHtml.Append("<tr>");
            SbHtml.Append("<td>");
            SbHtml.Append("</td>");
            SbHtml.Append("<td colspan='5' style='text-align:right;'>");
            SbHtml.Append("<span> DATE: " + DateTime.Now.ToString("dd/MM/yyyy") + " </span>");
            SbHtml.Append("</td>");
            SbHtml.Append("</tr>");
            SbHtml.Append("<tr>");
            SbHtml.Append("<td colspan='5' style='text-align: center;'>");
            SbHtml.Append("<span> NO DUES CERTIFICATE</span>");
            SbHtml.Append("</td>");
            SbHtml.Append("</tr>");
            SbHtml.Append("<tr>");
            SbHtml.Append("<td colspan='5' style='text-align: right;'>");
            SbHtml.Append("<span>Designation: " + designation + "</span>");
            SbHtml.Append("</td>");
            SbHtml.Append("</tr>");
            SbHtml.Append("</table>");
            SbHtml.Append("</div>");

            #endregion

            #region Student Details

            SbHtml.Append("<br>");
            SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
            SbHtml.Append("<center>");
            SbHtml.Append("<p style='font-size:large;text-align:justify'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; This is to certify that Mr./Mrs./Miss. <u>" + txtname.Text + "</u> Roll No. <u>" + txt_rollno.Text + "</u> a student of <u>" + course + "</u> department of <u>" + degree + "</u>  has no dues during the period <u>" + issuedate + "</u> on as on  <u>" + DateTime.Now.ToString("dd/MM/yyyy") + "</u> </p >");
            SbHtml.Append("</center>");
            SbHtml.Append("</div>");
            #endregion

            #region FooterDetails

            SbHtml.Append("<br/>");
            SbHtml.Append("<br/>");
            SbHtml.Append("<br/>");
            SbHtml.Append("<br/>");
            SbHtml.Append("<br/>");
            SbHtml.Append("<br/>");
            SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
            SbHtml.Append("<table border='0px' cellspacing='0' cellpadding='5' style='width: 645px;'>");
            SbHtml.Append("<tr style='text-align:left;'>");
            SbHtml.Append("</tr>");
            SbHtml.Append("<tr>");
            SbHtml.Append("<td colspan='5' style='text-align:right;'>");
            SbHtml.Append("<span>Librarian</span>");
            SbHtml.Append("</td>");
            SbHtml.Append("</tr>");
            SbHtml.Append("</table>");
            SbHtml.Append("</div>");
            SbHtml.Append("</div>");
            SbHtml.Append("</body>");
            SbHtml.Append("</html>");

            contentDiv.InnerHtml = SbHtml.ToString();
            contentDiv.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "btn_erroralert", "PrintDiv();", true);

            #endregion

            #endregion

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    protected void btnprintletter_Click(object sender, EventArgs e)
    {
        try
        {
            printletter();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "No Dues Entry");
        }
    }

    public void getLibPrivil()
    {
        try
        {
            Hashtable hsLibcode = new Hashtable();
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlCollege.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            if (singleUser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + userCode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupUserCode.Split(';');
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

            BindLibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdNoDues, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "No Dues " + '@';
            pagename = "No Dues.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdNoDues, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch { }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
    #endregion
}