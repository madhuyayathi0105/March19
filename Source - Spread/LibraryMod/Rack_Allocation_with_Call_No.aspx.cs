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
using System.Text.RegularExpressions;

public partial class LibraryMod_Rack_Allocation_with_Call_No : System.Web.UI.Page
{
    #region Field Declaration
    DAccess2 d2 = new DAccess2();
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    DAccess2 dacces2 = new DAccess2();

    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;


    #endregion

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
                //BindLibrary();
                Binddept();
                //ddlrackno.Items.Clear(); 
                //ddlshelfno.Items.Clear(); 
                LoadRackNo();
                LoadShelf();
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }

    }

    #region college

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
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }
    }

    #endregion

    #region Library

    public void BindLibrary(string libcode)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            string selectQuery = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " and college_code=" + ddlCollege.SelectedValue + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddllibrary.DataSource = ds;
                ddllibrary.DataTextField = "lib_name";
                ddllibrary.DataValueField = "lib_code";
                ddllibrary.DataBind();
                //ddllibrary.Items.Insert(0, "All");
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }
    }

    #endregion

    #region dept

    public void Binddept()
    {
        try
        {
            ddldept.Items.Clear();
            ds.Clear();
            string selectQuery = "";
            string newcollcode = ddlCollege.SelectedValue;
            selectQuery = "select distinct isnull(dept_name,'') dept_name from Journal_Dept where college_code=" + ddlCollege.SelectedValue + " and Lib_Code=" + ddllibrary.SelectedValue;
            //selectQuery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code='" + userCode + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + newcollcode + "') order by dept_name";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldept.DataSource = ds;
                ddldept.DataTextField = "Dept_Name";
                ddldept.DataValueField = "Dept_Name";
                ddldept.DataBind();
                // ddldept.Items.Insert(0, "All");/
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }

    }

    #endregion

    #region LoadCallNo

    public void LoadCallNo()
    {
        try
        {
            bool callCheck = false;
            bool callToCheck = false;
            chklstcallno.Items.Clear();
            ds.Clear();
            string selectQuery = "select distinct isnull(call_no,'') call_no,len(call_no) from bookdetails ";
            selectQuery = selectQuery + " where 1 =1 and call_no<>''";
            if (txtcallno.Text.Trim() != "")
                selectQuery = selectQuery + " and (call_no >= '" + txtcallno.Text + "' and  call_no <='" + txtcallnoto.Text + "') ";//selectQuery = selectQuery + " and call_no like '" + txtcallno.Text + "%'";
            if (ddllibrary.Text.Trim() != "")
                selectQuery = selectQuery + " and lib_code ='" + ddllibrary.SelectedValue + "'";
            if (ddldept.Text.Trim() != "All")
                selectQuery = selectQuery + " and Dept_Code ='" + ddldept.Text + "'";
            string CallNoToCheck = Convert.ToString(txtcallnoto.Text);
            string strcall = "";
            if (CallNoToCheck.Contains('.'))
            {
                selectQuery = selectQuery + " union all select distinct isnull(call_no,'') call_no,len(call_no) from bookdetails ";
                selectQuery = selectQuery + " where 1 =1 and call_no<>''";
                if (txtcallnoto.Text.Trim() != "")
                    selectQuery = selectQuery + " and call_no like'" + txtcallnoto.Text + "%' ";
                if (ddllibrary.Text.Trim() != "")
                    selectQuery = selectQuery + " and lib_code ='" + ddllibrary.SelectedValue + "'";
                if (ddldept.Text.Trim() != "All")
                    selectQuery = selectQuery + " and Dept_Code ='" + ddldept.Text + "'";
            }            
            selectQuery = selectQuery + " order by call_no,len(call_no) ";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            string CallNoCheck=Convert.ToString(txtcallno.Text);
            
            if(CallNoCheck.Contains('.'))
            {
                string[] splitstr = CallNoCheck.Split('.');
                strcall=splitstr[0];
                callCheck = true;                
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string Call_No = Convert.ToString(ds.Tables[0].Rows[i]["call_no"]);
                    if (!Call_No.Contains(' ') || !Call_No.Contains('.'))
                    {
                        if (callCheck == true)
                        {
                            if (Call_No.Contains(strcall))
                            {
                            }
                            else
                            {
                                chklstcallno.Items.Add(Call_No);
                            }
                        }
                        else
                        {
                            chklstcallno.Items.Add(Call_No);
                        }
                    }
                    else
                    {
                        chklstcallno.Items.Add(Call_No);
                    }                   
                }
                int count = chklstcallno.Items.Count;               
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }
    }

    #endregion

    #region LoadRackNo

    public void LoadRackNo()
    {
        try
        {
            ddlrackno.Items.Clear();
            ds.Clear();
            string selectQuery = "select distinct rack_no,len(rack_no)  from rack_master where 1=1 ";
            if (ddllibrary.Text.Trim() != "All")
                selectQuery = selectQuery + " and lib_code ='" + ddllibrary.SelectedValue + "'";
            selectQuery = selectQuery + " order by len(rack_no),rack_no ";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlrackno.DataSource = ds;
                ddlrackno.DataTextField = "rack_no";
                ddlrackno.DataValueField = "rack_no";
                ddlrackno.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }
    }

    #endregion

    #region LoadShelf

    public void LoadShelf()
    {
        try
        {
            ddlshelfno.Items.Clear();
            ds.Clear();
            string selectQuery = "select distinct row_no,len(row_no)  from rackrow_master where 1=1 ";
            selectQuery = selectQuery + " and rack_no ='" + ddlrackno.Text + "' ";
            if (ddllibrary.Text.Trim() != "All")
                selectQuery = selectQuery + " and lib_code ='" + ddllibrary.SelectedValue + "' ";
            selectQuery = selectQuery + " order by len(row_no),row_no ";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlshelfno.DataSource = ds;
                ddlshelfno.DataTextField = "row_no";
                ddlshelfno.DataValueField = "row_no";
                ddlshelfno.DataBind();



            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }

    }

    #endregion

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {



            getLibPrivil();
            Binddept();


        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }


    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Binddept();
            LoadRackNo();
            LoadShelf();
            chklstcallno.Items.Clear();

        }

        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }


    }

    protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }

        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }


    }

    protected void chkcallno_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkcallno.Checked == true)
            {
                for (int i = 0; i < chklstcallno.Items.Count; i++)
                {
                    chklstcallno.Items[i].Selected = true;
                    txtcallno1.Text = Label1.Text + "(" + (chklstcallno.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstcallno.Items.Count; i++)
                {
                    chklstcallno.Items[i].Selected = false;
                    txtcallno1.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }

    }

    protected void chklstcallno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int count = 0;
            string value = "";
            string code = "";
            for (int i = 0; i < chklstcallno.Items.Count; i++)
            {
                if (chklstcallno.Items[i].Selected == true)
                {

                    value = chklstcallno.Items[i].Text;
                    code = chklstcallno.Items[i].Value.ToString();
                    count = count + 1;
                    txtcallno1.Text = Label1.Text + "(" + count.ToString() + ")";
                }

            }

            if (count == 0)
                txtcallno1.Text = "---Select---";

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }
    }

    protected void ddlrackno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadShelf();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }
    }

    #endregion

    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        try
        {
            LoadCallNo();
            //LoadRackNo();
            //LoadShelf();

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string StrCallNo = "";
            string Sql = "";
            int StrCount;
            int strAvlCount;
            int StrMaxCount;
            int save = 0;
            if (ddlrackno.Text != "" && ddlshelfno.Text != "")
            {
                for (int i = 0; i < chklstcallno.Items.Count; i++)
                {
                    if (chklstcallno.Items[i].Selected == true)
                    {
                        Sql = "select acc_no,lib_code from bookdetails where call_no ='" + chklstcallno.Items[i] + "' ";
                        if (ddllibrary.Text.Trim() != "All")
                            Sql = Sql + " AND lib_code ='" + ddllibrary.SelectedValue + "' ";
                        if (ddldept.Text.Trim() != "All")
                            Sql = Sql + " AND Dept_Code ='" + ddldept.SelectedItem.Text + "' ";
                        ds = dacces2.select_method_wo_parameter(Sql, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {
                                StrCount = Convert.ToInt32(dacces2.GetFunction("select count(*) from rack_allocation where acc_no ='" + ds.Tables[0].Rows[k]["acc_no"].ToString() + "' and lib_code ='" + ds.Tables[0].Rows[k]["lib_code"].ToString() + "'"));
                                strAvlCount = Convert.ToInt32(dacces2.GetFunction("select count(*) from rack_allocation where rack_no ='" + ddlrackno.Text + "' and row_no ='" + ddlshelfno.Text + "' and lib_code ='" + ds.Tables[0].Rows[k]["lib_code"].ToString() + "'"));
                                StrMaxCount = Convert.ToInt32(dacces2.GetFunction("select isnull(Max_Capacity,0) from rackrow_master where rack_no ='" + ddlrackno.Text + "' and row_no ='" + ddlshelfno.Text + "' and lib_code ='" + ds.Tables[0].Rows[k]["lib_code"].ToString() + "'"));

                                if (strAvlCount < StrMaxCount)
                                {
                                    //if (StrCount == 0)
                                    //{
                                    Sql = "if exists(select * from rack_allocation where acc_no='" + ds.Tables[0].Rows[k]["acc_no"].ToString() + "' and lib_code='" + ddllibrary.SelectedValue + "') update rack_allocation set rack_no='" + ddlrackno.Text + "',row_no='" + ddlshelfno.Text + "' where acc_no='" + ds.Tables[0].Rows[k]["acc_no"].ToString() + "' and lib_code='" + ddllibrary.SelectedValue + "' else insert into rack_allocation (lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type) ";
                                    Sql = Sql + " values ('" + ddllibrary.SelectedValue + "','" + ddlrackno.Text + "','" + ddlshelfno.Text + "','" + ds.Tables[0].Rows[k]["acc_no"].ToString() + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToLongTimeString() + "','BOK')";
                                    save = dacces2.update_method_wo_parameter(Sql, "Text");
                                    //}
                                }
                            }
                        }
                    }
                }
            }
            imgAlert.Visible = true;
            lbl_alert.Text = "Saved Sucessfully";

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }
    }

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        try
        {
            imgAlert.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Rack_Allocation_with_Call_No");
        }
    }

    public void getLibPrivil()
    {
        try
        {
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlCollege.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            Hashtable hsLibcode = new Hashtable();
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

}


