using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BalAccess;
using DalConnection;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;
using Gios.Pdf;
using System.IO;
using InsproDataAccess;

public partial class LibraryMod_Card_list_and_holder : System.Web.UI.Page
{
    #region initialization

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    DataTable dtCommon = new DataTable();
    SqlCommand cmd = new SqlCommand();
    static Hashtable Has_Stage = new Hashtable();
    ReuasableMethods ru = new ReuasableMethods();
    string Sql = string.Empty;
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataTable card = new DataTable();
    DataRow drlist;
    public void Connection()
    {
        con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
        con.Open();
    }
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DAccess2 dacces2 = new DAccess2();
    DataSet ds;
    DataSet ds2 = new DataSet();
    string usercode = "", singleuser = "", group_user = "";
    string collegecode = "";
    string course_id = string.Empty;
    static int batchcnt = 0;
    static int degreecnt = 0;
    static int branchcnt = 0;
    string dept = "";
    string Category = "";
    string colcode1 = "";
    string Batch1 = "";
    string Degree1 = "";
    string Branch1 = "";
    static int selectedMode = 0;
    static string collegeCode1 = string.Empty;
    string collegeCode = string.Empty;
    string stud_name = string.Empty;
    string appno = string.Empty;
    string curr_Sem = string.Empty;
    string rollNo = string.Empty;
    string degCode = string.Empty;
    string batchYear = string.Empty;
    string sec = string.Empty;
    static string colcode2 = "";
    string str = string.Empty;
    string userCollegeCode = string.Empty;
    string userCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        else
        {
            userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
        }
        Rblreturn_CheckedChange(sender, e);
        chklstbatch_SelectedIndexChanged(sender, e);

        if (!Page.IsPostBack)
        {
            loadcollege();
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            bindstaffCategory();
            bindstaffdept1();
            load_ddlrollno();
            colcode2 = Convert.ToString(Session["collegecode"]);
        }
    }

    protected void ddlrollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_rollno.Text = "";
        switch (Convert.ToUInt32(ddlrollno.SelectedItem.Value))
        {
            case 0:
                txt_rollno.Attributes.Add("Placeholder", "Roll No");
                selectedMode = 0;
                break;
            case 1:
                txt_rollno.Attributes.Add("Placeholder", "Reg No");
                selectedMode = 1;
                break;
            case 2:
                txt_rollno.Attributes.Add("Placeholder", "Admission No");
                selectedMode = 2;
                break;
            case 3:
                txt_rollno.Attributes.Add("Placeholder", "App No");
                selectedMode = 3;
                break;
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {

            string query = "";
            WebService ws = new WebService();


            if (selectedMode == 0)
            {
                query = "select top 100 Roll_No from Registration r where  r.cc=0 and r.Exam_Flag<>'debar'  and  r.DelFlag=0 and Roll_No like '" + prefixText + "%' and r.college_code='" + colcode2 + "'  order by  Roll_No asc";
            }
            else if (selectedMode == 1)
            {
                query = "select  top 100 Reg_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Reg_No like '" + prefixText + "%' and r.college_code='" + colcode2 + "'  order by  Reg_No asc";
            }
            else if (selectedMode == 2)
            {
                query = "select  top 100 Roll_admit from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_admit like '" + prefixText + "%' and r.college_code='" + colcode2 + "'  order by  Roll_admit asc";
            }
            else if (selectedMode == 3)
            {
                query = "select  top 100 app_formno from applyn where isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code='" + colcode2 + "'  order by  app_formno asc";

            }


            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstfcode(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {

            string query = "";
            WebService ws = new WebService();



            query = "select top 100 staff_code from staffmaster r where staff_code like '" + prefixText + "%' and r.college_code='" + colcode2 + "'  order by  staff_code asc";



            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    protected void txt_rollno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GetStudentDetails();

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }
    public void GetStudentDetails()
    {
        try
        {

            string txtValue = Convert.ToString(txt_rollno.Text);
            switch (selectedMode)
            {
                case 0:
                    str = " and r.Roll_No='" + txtValue + "'";
                    break;
                case 1:
                    str = " and r.reg_no='" + txtValue + "'";
                    break;
                case 2:
                    str = " and r.Roll_Admit='" + txtValue + "'";
                    break;
                case 3:
                    str = " and a.app_no='" + txtValue + "'";
                    break;
            }


            string qry = "select r.Roll_No,r.reg_no,r.Roll_Admit,a.app_no,r.degree_code,r.Batch_Year,r.Current_Semester,r.stud_name from applyn a,Registration r where a.app_no=r.App_No " + str; //and a.app_formno=r.Roll_No and r.degree_code=a.degree_code and a.batch_year=r.Batch_Year
            DataSet dsStudDetails = d2.select_method_wo_parameter(qry, "Text");

            if (dsStudDetails.Tables.Count > 0 && dsStudDetails.Tables[0].Rows.Count > 0)
            {
                stud_name = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["stud_name"]).Trim();
                rollNo = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["roll_no"]).Trim();
                appno = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["app_no"]).Trim();
                curr_Sem = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["current_semester"]).Trim();
                degCode = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["degree_code"]).Trim();
                batchYear = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["batch_year"]).Trim();
                sec = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["Sections"]).Trim();
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }

    protected void txt_staffcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Getstaffcode();


        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }
    public void Getstaffcode()
    {
        try
        {



        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }
    //protected void rblreporttype_CheckedChange(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        if (rblreporttype.SelectedIndex == 0)
    //        {

    //            Fpspread6.Visible = false;
    //            rptprint1.Visible = false;


    //        }
    //        if (rblreporttype.SelectedIndex == 1)
    //        {

    //            Fpspread6.Visible = false;
    //            rptprint1.Visible = false;

    //        }





    //    }

    //    catch (Exception ex)
    //    {
    //        d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
    //    }

    //}
    public void load_ddlrollno()
    {
        try
        {
            System.Web.UI.WebControls.ListItem lstItem1 = new System.Web.UI.WebControls.ListItem("Roll No", "0");
            System.Web.UI.WebControls.ListItem lstItem2 = new System.Web.UI.WebControls.ListItem("Reg No", "1");
            System.Web.UI.WebControls.ListItem lstItem3 = new System.Web.UI.WebControls.ListItem("Admission No", "2");
            System.Web.UI.WebControls.ListItem lstItem4 = new System.Web.UI.WebControls.ListItem("App No", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            ddlrollno.Items.Clear();
            string insqry1 = "select value from Master_Settings where settings='Roll No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                ddlrollno.Items.Add(lstItem1);
            }


            insqry1 = "select value from Master_Settings where settings='Register No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                ddlrollno.Items.Add(lstItem2);
            }

            insqry1 = "select value from Master_Settings where settings='Admission No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                ddlrollno.Items.Add(lstItem3);
            }

            insqry1 = "select value from Master_Settings where settings='Application No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                ddlrollno.Items.Add(lstItem4);
            }

            if (ddlrollno.Items.Count == 0)
            {
                ddlrollno.Items.Add(lstItem1);
            }
            switch (Convert.ToUInt32(ddlrollno.SelectedItem.Value))
            {
                case 0:
                    txt_rollno.Attributes.Add("placeholder", "Roll No");
                    selectedMode = 0;
                    break;
                case 1:
                    txt_rollno.Attributes.Add("placeholder", "Reg No");
                    selectedMode = 1;
                    break;
                case 2:
                    txt_rollno.Attributes.Add("placeholder", "Admin No");
                    selectedMode = 2;
                    break;
                case 3:
                    txt_rollno.Attributes.Add("placeholder", "App No");
                    selectedMode = 3;
                    break;
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }

    protected void Rblreturn_CheckedChange(object sender, EventArgs e)
    {
        try
        {

            if (Rblreturn.SelectedIndex == 0)
            {
                ddlrollno.Visible = true;
                txt_rollno.Visible = true;
                txt_staffcode.Visible = false;
                txtbatch.Enabled = true;
                txtdegree.Enabled = true;
                txtbranch.Enabled = true;
                txtstaffDept.Enabled = false;
                txt_StaffCatogery.Enabled = false;
                //Fpspread6.Visible = false;
                //rptprint1.Visible = false;


            }
            if (Rblreturn.SelectedIndex == 1)
            {
                ddlrollno.Visible = false;
                txt_rollno.Visible = false;
                txt_staffcode.Visible = true;
                txtbatch.Enabled = false;
                txtdegree.Enabled = false;
                txtbranch.Enabled = false;
                txtstaffDept.Enabled = true;
                txt_StaffCatogery.Enabled = true;
                //Fpspread6.Visible = false;
                //rptprint1.Visible = false;

            }





        }

        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }

    }


    public void loadcollege()
    {
        try
        {
            cblclg.Items.Clear();
            dtCommon.Clear();
            cblclg.Enabled = false;
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
                cblclg.DataSource = dtCommon;
                cblclg.DataTextField = "collname";
                cblclg.DataValueField = "college_code";
                cblclg.DataBind();
                cblclg.SelectedIndex = 0;
                cblclg.Enabled = true;

                if (cblclg.Items.Count > 0)
                {
                    for (int i = 0; i < cblclg.Items.Count; i++)
                    {
                        cblclg.Items[i].Selected = true;
                    }
                    txtclg.Text = Label16.Text + "(" + cblclg.Items.Count + ")";
                    cbclg.Checked = true;
                }
            }




        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
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
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }
    public void BindBatch()
    {


        try
        {
            string qryUserOrGroupCode = string.Empty;
            string groupUserCode = string.Empty;

            string userCode = string.Empty;
            if (chklstbatch.Items.Count > 0)
                chklstbatch.Items.Clear();
            //ds.Clear();
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                else
                {
                    groupUserCode = group;
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (cblclg.Items.Count > 0)
            {
                string clgcode = "";
                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                {
                    if (cblclg.Items[clg].Selected == true)
                    {
                        if (clgcode == "")
                            clgcode = cblclg.Items[clg].Value;
                        else
                            clgcode = clgcode + "," + cblclg.Items[clg].Value;
                    }
                }
                //   collegecode = Session["collegecode"].ToString();
                collegeCode = clgcode;
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string strbinddegree = "select distinct batch_year from Registration where batch_year<>'' and batch_year<>'0' and batch_year<>'-1'   order by batch_year desc"; //college_code in(" + collegeCode + ") and modified by prabha on feb 06 2018
                ds = dacces2.select_method_wo_parameter(strbinddegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklstbatch.DataSource = ds;
                chklstbatch.DataTextField = "Batch_year";
                chklstbatch.DataValueField = "Batch_year";
                chklstbatch.DataBind();
                //chklstbatch.SelectedIndex = 0;


                if (chklstbatch.Items.Count > 0)
                {
                    for (int i = 0; i < chklstbatch.Items.Count; i++)
                    {
                        chklstbatch.Items[i].Selected = true;
                    }
                    txtbatch.Text = Label16.Text + "(" + chklstbatch.Items.Count + ")";
                    chkbatch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            dacces2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }

    }
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            if (cblclg.Items.Count > 0)
            {
                string clgcode = "";
                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                {
                    if (cblclg.Items[clg].Selected == true)
                    {
                        if (clgcode == "")
                            clgcode = cblclg.Items[clg].Value;
                        else
                            clgcode = clgcode + "," + cblclg.Items[clg].Value;
                    }
                }
                //   collegecode = Session["collegecode"].ToString();
                collegecode = clgcode;
            }
            chklstdegree.Items.Clear();
            txtdegree.Text = "--Select--";
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string selqry = string.Empty;
            ds2.Clear();

            if (!string.IsNullOrEmpty(collegecode))
            {

                #region modified on 11/12/2017 User Rights based Reg or Roll No added by prabha

                string columnfield = string.Empty;
                string group_userNEW = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = Convert.ToString(group_semi[0]);
                }
                if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
                {
                    columnfield = " group_code='" + group_userNEW + "'";
                }
                else if (Session["usercode"] != null)
                {
                    columnfield = " user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                }
                string user_code = Convert.ToString(Session["usercode"]).Trim();

                string degreerights = "select degree_code from DeptPrivilages where " + columnfield + " ";

                #endregion

                selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code in(" + collegecode + ")";

                ds2 = dacces2.select_method_wo_parameter(selqry, "Text");
            }

            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
                txtdegree.Text = Label8.Text + "(" + 1 + ")";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }
    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = chklstdegree.Items[i].Value.ToString();
                    }
                    else
                    {
                        course_id = course_id + "'" + "," + "'" + chklstdegree.Items[i].Value.ToString();
                    }
                }
            }

            if (cblclg.Items.Count > 0)
            {
                string clgcode = "";
                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                {
                    if (cblclg.Items[clg].Selected == true)
                    {
                        if (clgcode == "")
                            clgcode = cblclg.Items[clg].Value;
                        else
                            clgcode = clgcode + "," + cblclg.Items[clg].Value;
                    }
                }
                //   collegecode = Session["collegecode"].ToString();
                collegecode = clgcode;
            }
            //course_id = chklstdegree.SelectedValue.ToString();
            chklstbranch.Items.Clear();
            txtbranch.Text = "--Select--";
            //if (group_user.Contains(';'))
            //{
            //    string[] group_semi = group_user.Split(';');
            //    group_user = group_semi[0].ToString();
            //}
            //ds2.Dispose();
            //ds2.Reset();
            //ds2 = dacces2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            // string sel = " select * from Department dt,Degree d where d.Dept_Code=dt.Dept_Code and d.Degree_Code in('" + course_id + "') and  d.college_code in(" + collegecode + ")";
            if (!string.IsNullOrEmpty(collegecode))
            {
                string sel = "  select dt.Dept_Name,dt.dept_code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.Course_Id in('" + course_id + "') and d.college_code in(" + collegecode + ")";
                ds2 = dacces2.select_method_wo_parameter(sel, "Text");
            }
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "dept_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                txtbranch.Text = Label9.Text + "(" + 1 + ")";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
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
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }
    protected void cbclg_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cblclg.Items.Count > 0)
            {
                string clgcode = "";
                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                {
                    if (cblclg.Items[clg].Selected == true)
                    {
                        if (clgcode == "")
                            clgcode = cblclg.Items[clg].Value;
                        else
                            clgcode = clgcode + "," + cblclg.Items[clg].Value;
                    }
                }
                //   collegecode = Session["collegecode"].ToString();
                collegecode = clgcode;
            }
            CallCheckboxChange(cbclg, cblclg, txtclg, Label16.Text, "--Select--");
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }
    protected void cblclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cblclg.Items.Count > 0)
            {
                string clgcode = "";
                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                {
                    if (cblclg.Items[clg].Selected == true)
                    {
                        if (clgcode == "")
                            clgcode = cblclg.Items[clg].Value;
                        else
                            clgcode = clgcode + "," + cblclg.Items[clg].Value;
                    }
                }
                //   collegecode = Session["collegecode"].ToString();
                collegecode = clgcode;

            }
            CallCheckboxListChange(cbclg, cblclg, txtclg, Label16.Text, "--Select--");
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }
    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklstbatch.Items.Count; i++)
                {
                    chklstbatch.Items[i].Selected = true;
                    txtbatch.Text = "Batch(" + (chklstbatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstbatch.Items.Count; i++)
                {
                    chklstbatch.Items[i].Selected = false;
                    txtbatch.Text = "---Select---";
                }
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            pbatch.Focus();

            int batchcount = 0;
            string value = "";
            string code = "";


            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                if (chklstbatch.Items[i].Selected == true)
                {

                    value = chklstbatch.Items[i].Text;
                    code = chklstbatch.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                    txtbatch.Text = "Batch(" + batchcount.ToString() + ")";
                }

            }

            if (batchcount == 0)
                txtbatch.Text = "---Select---";
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

            //BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }

    }
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    txtdegree.Text = Label8.Text + "(" + (chklstdegree.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                    txtdegree.Text = "---Select---";
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //pdegree.Focus();
            pdegree1.Focus();

            int degreecount = 0;
            string value = "";
            string code = "";


            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {

                    value = chklstdegree.Items[i].Text;
                    code = chklstdegree.Items[i].Value.ToString();
                    degreecount = degreecount + 1;
                    txtdegree.Text = Label8.Text + "(" + degreecount.ToString() + ")";
                }

            }

            if (degreecount == 0)
                txtdegree.Text = "---Select---";
            else
            {
                Label lbl = degreelabel();
                lbl.Text = " " + value + " ";
                lbl.ID = "lbl1-" + code.ToString();
                ImageButton ib = degreeimage();
                ib.ID = "imgbut1_" + code.ToString();
                ib.Click += new ImageClickEventHandler(degreeimg_Click);
            }
            degreecnt = degreecount;
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }

    }
    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    txtbranch.Text = Label9.Text + "(" + (chklstbranch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                    txtbranch.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            pbranch.Focus();

            int branchcount = 0;
            string value = "";
            string code = "";


            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {

                    value = chklstbranch.Items[i].Text;
                    code = chklstbranch.Items[i].Value.ToString();
                    branchcount = branchcount + 1;
                    txtbranch.Text = Label9.Text + "(" + branchcount.ToString() + ")";
                }

            }

            if (branchcount == 0)
                txtbranch.Text = "---Select---";
            else
            {
                Label lbl = branchlabel();
                lbl.Text = " " + value + " ";
                lbl.ID = "lbl1-" + code.ToString();
                ImageButton ib = branchimage();
                ib.ID = "imgbut1_" + code.ToString();
                ib.Click += new ImageClickEventHandler(branchimg_Click);
            }
            branchcnt = branchcount;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }

    }
    public void branchimg_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            branchcnt = branchcnt - 1;
            ImageButton b = sender as ImageButton;
            int r = Convert.ToInt32(b.CommandArgument);
            chklstbranch.Items[r].Selected = false;

            txtdegree.Text = Label9.Text + "(" + branchcnt.ToString() + ")";
            if (txtdegree.Text == Label9.Text + "(0)")
            {
                txtdegree.Text = "---Select---";

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }

    }

    public Label branchlabel()
    {

        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton branchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }
    public Label degreelabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton degreeimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }
    public void degreeimg_Click(object sender, ImageClickEventArgs e)
    {
        degreecnt = degreecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstdegree.Items[r].Selected = false;

        txtdegree.Text = Label8.Text + "(" + degreecnt.ToString() + ")";
        if (txtdegree.Text == Label8.Text + "(0)")
        {
            txtdegree.Text = "---Select---";

        }

    }

    public void batchimg_Click(object sender, ImageClickEventArgs e)
    {
        batchcnt = batchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbatch.Items[r].Selected = false;

        txtbatch.Text = "Batch(" + batchcnt.ToString() + ")";
        if (txtbatch.Text == "Batch(0)")
        {
            txtbatch.Text = "---Select---";

        }

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
    public void bindstaffdept1()
    {
        try
        {
            string collegecode = getCblSelectedValue(cblclg);
            SqlDataAdapter dadept = new SqlDataAdapter("select distinct dept_code,dept_name from   hrdept_master where college_code in('" + collegecode + "') order by dept_name", con);
            DataSet dsdept = new DataSet();
            dadept.Fill(dsdept);
            if (dsdept.Tables[0].Rows.Count > 0)
            {
                chklststaffDept.Items.Clear();
                if (dsdept.Tables[0].Rows.Count > 0)
                {
                    chklststaffDept.Items.Clear();
                    chklststaffDept.DataSource = dsdept.Tables[0];
                    chklststaffDept.DataTextField = "dept_name";
                    chklststaffDept.DataValueField = "dept_code";
                    chklststaffDept.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }

    }
    public void bindstaffCategory()
    {
        try
        {
            string collegecode = getCblSelectedValue(cblclg);
            SqlDataAdapter dadept = new SqlDataAdapter("select distinct category_code,category_name from   staffCategorizer where college_code in('" + collegecode + "') order by category_name", con);
            DataSet dscate = new DataSet();
            dadept.Fill(dscate);
            if (dscate.Tables[0].Rows.Count > 0)
            {
                chklststaffDept.Items.Clear();
                if (dscate.Tables[0].Rows.Count > 0)
                {
                    cbl_StaffCatogery.Items.Clear();
                    cbl_StaffCatogery.DataSource = dscate.Tables[0];
                    cbl_StaffCatogery.DataTextField = "category_name";
                    cbl_StaffCatogery.DataValueField = "category_code";
                    cbl_StaffCatogery.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }

    }
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

    protected void chksatffDept_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chksatffDept.Checked == true)
            {
                for (int i = 0; i < chklststaffDept.Items.Count; i++)
                {
                    chklststaffDept.Items[i].Selected = true;
                    txtstaffDept.Text = "Staff(" + (chklststaffDept.Items.Count) + ")";

                }

            }
            else
            {
                for (int i = 0; i < chklststaffDept.Items.Count; i++)
                {
                    chklststaffDept.Items[i].Selected = false;
                    txtstaffDept.Text = "---Select---";
                }


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }
    protected void chklststaffDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            string value = "";
            string code = "";


            for (int i = 0; i < chklststaffDept.Items.Count; i++)
            {
                if (chklststaffDept.Items[i].Selected == true)
                {
                    value = chklststaffDept.Items[i].Text;
                    code = chklststaffDept.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                    txtstaffDept.Text = "Staff(" + batchcount.ToString() + ")";

                }


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }

    }
    protected void cb_StaffCatogery_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_StaffCatogery.Checked == true)
            {
                for (int i = 0; i < cbl_StaffCatogery.Items.Count; i++)
                {
                    cbl_StaffCatogery.Items[i].Selected = true;
                    txt_StaffCatogery.Text = "Staff Catogery(" + (cbl_StaffCatogery.Items.Count) + ")";

                }

            }
            else
            {
                for (int i = 0; i < cbl_StaffCatogery.Items.Count; i++)
                {
                    cbl_StaffCatogery.Items[i].Selected = false;
                    txt_StaffCatogery.Text = "---Select---";
                }


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }
    protected void cbl_StaffCatogery_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            string value = "";
            string code = "";


            for (int i = 0; i < cbl_StaffCatogery.Items.Count; i++)
            {
                if (cbl_StaffCatogery.Items[i].Selected == true)
                {
                    value = cbl_StaffCatogery.Items[i].Text;
                    code = cbl_StaffCatogery.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                    txt_StaffCatogery.Text = "Staff Catogery(" + batchcount.ToString() + ")";

                }


            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }

    }
    #region go
    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        try
        {
            dept = "''";
            Category = "''";
            colcode1 = "''";
            Batch1 = "''";

            if (cblclg.Items.Count > 0)
            {
                string clgcode = "";
                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                {
                    if (cblclg.Items[clg].Selected == true)
                    {
                        if (clgcode == "")
                            clgcode = "'" + cblclg.Items[clg].Value + "'";
                        else
                            clgcode = ",'" + clgcode + "," + cblclg.Items[clg].Value + "'";
                    }
                }
                //   collegecode = Session["collegecode"].ToString();
                colcode1 = clgcode;
            }

            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                if (chklstbatch.Items[i].Selected == true)
                {


                    if (Batch1 == "")
                    {
                        Batch1 = "'" + chklstbatch.Items[i].Value + "'";
                    }
                    else
                    {
                        Batch1 += ",'" + chklstbatch.Items[i].Value + "'";
                    }
                }



            }

            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {


                    if (Degree1 == "")
                    {
                        Degree1 = chklstdegree.Items[i].Value;
                    }
                    else
                    {
                        Degree1 += "," + chklstdegree.Items[i].Value;
                    }
                }

            }
            if (Degree1 == "")
                Degree1 = "0";
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {


                    if (Branch1 == "")
                    {
                        Branch1 = chklstbranch.Items[i].Value;
                    }
                    else
                    {
                        Branch1 += "," + chklstbranch.Items[i].Value;
                    }
                }


            }
            if (Branch1 == "")
                Branch1 = "0";

            for (int i = 0; i < chklststaffDept.Items.Count; i++)
            {
                if (chklststaffDept.Items[i].Selected == true)
                {


                    if (dept == "")
                    {
                        dept = "'" + chklststaffDept.Items[i].Value + "'";
                    }
                    else
                    {
                        dept += ",'" + chklststaffDept.Items[i].Value + "'";
                    }
                }


            }
            for (int i = 0; i < cbl_StaffCatogery.Items.Count; i++)
            {
                if (cbl_StaffCatogery.Items[i].Selected == true)
                {


                    if (Category == "")
                    {
                        Category = "'" + cbl_StaffCatogery.Items[i].Value + "'";
                    }
                    else
                    {
                        Category += ",'" + cbl_StaffCatogery.Items[i].Value + "'";
                    }
                }


            }
            if (rblreporttype.SelectedIndex == 0)
            {
                if (Rblreturn.SelectedIndex == 0)
                {
                    if (txt_rollno.Text.ToString().Trim() != "")
                    {
                        GetStudentDetails();
                        Sql = "SELECT T.Roll_No,R.Stud_Name,C.Course_Name+'-'+D.Dept_Name Dept_Name,COUNT(*) AS TotCard FROM TokenDetails T,Registration R,Degree G,Course C,Department D,applyn a WHERE (T.Roll_No = R.Roll_No OR T.Roll_No = R.Lib_ID) AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND T.Is_Staff = 0 " + str + " and G.College_Code in(" + colcode1 + ") GROUP BY T.Roll_No,R.Stud_Name,C.Course_Name,D.Dept_Name";
                    }
                    else
                    {

                        Sql = "SELECT T.Roll_No,R.Stud_Name,C.Course_Name+'-'+D.Dept_Name Dept_Name,COUNT(*) AS TotCard FROM TokenDetails T,Registration R,Degree G,Course C,Department D WHERE (T.Roll_No = R.Roll_No OR T.Roll_No = R.Lib_ID) AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND T.Is_Staff = 0 and R.Batch_Year in(" + Batch1 + ") and G.Course_Id in(" + Degree1 + ") and D.dept_code in(" + Branch1 + ") and G.College_Code in(" + colcode1 + ") GROUP BY T.Roll_No,R.Stud_Name,C.Course_Name,D.Dept_Name";
                    }
                    ds = d2.select_method_wo_parameter(Sql, "text");


                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        card.Columns.Add("SNo", typeof(string));
                        card.Columns.Add("Department Name", typeof(string));
                        card.Columns.Add("Roll No", typeof(string));
                        card.Columns.Add("Student Name", typeof(string));
                        card.Columns.Add("Total Card", typeof(string));


                      

                        int sno = 0;
                        for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                        {
                            sno++;
                            drlist = card.NewRow();
                            drlist["SNo"] = sno.ToString();

                            drlist["Department Name"] = ds.Tables[0].Rows[r]["Dept_Name"].ToString();

                            drlist["Roll No"] = ds.Tables[0].Rows[r]["Roll_No"].ToString();

                            drlist["Student Name"] = ds.Tables[0].Rows[r]["Stud_Name"].ToString();
                            drlist["Total Card"] = ds.Tables[0].Rows[r]["TotCard"].ToString();
                            card.Rows.Add(drlist);

                        }


                        grdManualExit.DataSource = card;

                        grdManualExit.DataBind();
                       
                        grdManualExit.Visible = true;
                        rptprint1.Visible = true;
                    }

                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                        grdManualExit.Visible = false;
                        rptprint1.Visible = false;
                    }
                    if (grdManualExit.Rows.Count > 0)
                    {

                        CheckBox selectall = grdManualExit.Rows[0].FindControl("selectall") as CheckBox;
                        selectall.Visible = true;
                        CheckBox select = grdManualExit.Rows[0].FindControl("select") as CheckBox;
                        select.Visible = false;

                    }

                }
                else if (Rblreturn.SelectedIndex == 1)
                {
                    if (txt_staffcode.Text.ToString().Trim() != "")
                    {

                        Sql = "SELECT  T.Roll_No as 'Staff Code',sm.staff_name as 'Staff Name',sc.category_name,d.Dept_Name as 'Department Name',COUNT(*) AS TotCard FROM tokendetails T, stafftrans st,staffmaster sm,hrdept_master d,staffCategorizer sc WHERE is_staff = 1 and T.roll_no=st.staff_code and st.staff_code=sm.staff_code and T.roll_no=sm.staff_code and st.dept_code=d.dept_code and st.category_code =sc.category_code and sm.college_code=sc.college_code and sm.college_code=d.college_code and sc.college_code=d.college_code and T.roll_no in('" + txt_staffcode.Text.ToString().Trim() + "') and sm.college_code in(" + colcode1 + ") GROUP BY T.Roll_No,sm.staff_name,sc.category_name,d.Dept_Name";

                    }
                    else
                    {


                        Sql = "SELECT  T.Roll_No as 'Staff Code',sm.staff_name as 'Staff Name',sc.category_name,d.Dept_Name as 'Department Name',COUNT(*) AS TotCard FROM tokendetails T, stafftrans st,staffmaster sm,hrdept_master d,staffCategorizer sc WHERE is_staff = 1 and T.roll_no=st.staff_code and st.staff_code=sm.staff_code and T.roll_no=sm.staff_code and st.dept_code=d.dept_code and st.category_code =sc.category_code and sm.college_code=sc.college_code and sm.college_code=d.college_code and sc.college_code=d.college_code and st.dept_code in(" + dept + ")  and st.category_code in(" + Category + ") and sm.college_code in(" + colcode1 + ") GROUP BY T.Roll_No,sm.staff_name,sc.category_name,d.Dept_Name";

                    }

                    ds = d2.select_method_wo_parameter(Sql, "text");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        card.Columns.Add("SNo", typeof(string));
                        card.Columns.Add("Staff Name", typeof(string));
                        card.Columns.Add("Category Name", typeof(string));
                        card.Columns.Add("Department Name", typeof(string));
                        card.Columns.Add("Staff Code", typeof(string));
                        card.Columns.Add("Total Code", typeof(string));

                     

                        int sno = 0;
                        for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                        {
                            sno++;
                            drlist = card.NewRow();
                            drlist["SNo"] = sno.ToString();

                            drlist["Staff Name"] = ds.Tables[0].Rows[r]["Staff Name"].ToString();
                            drlist["Category Name"] = ds.Tables[0].Rows[r]["category_name"].ToString();
                            drlist["Department Name"] = ds.Tables[0].Rows[r]["Department Name"].ToString();
                            drlist["Staff Code"] = ds.Tables[0].Rows[r]["Staff Code"].ToString();
                            drlist["Total Card"] = ds.Tables[0].Rows[r]["TotCard"].ToString();
                            card.Rows.Add(drlist);
                        }


                        grdManualExit.DataSource = card;
                        grdManualExit.DataBind();
                      
                        grdManualExit.Visible = true;
                        rptprint1.Visible = true;
                    }

                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                        grdManualExit.Visible = false;
                        rptprint1.Visible = false;
                    }

                }
                if (grdManualExit.Rows.Count > 0)
                {

                    CheckBox selectall = grdManualExit.Rows[0].FindControl("selectall") as CheckBox;
                    selectall.Visible = true;
                    CheckBox select = grdManualExit.Rows[0].FindControl("select") as CheckBox;
                    select.Visible = false;

                }

            }

            if (rblreporttype.SelectedIndex == 1)
            {
                if (Rblreturn.SelectedIndex == 0)
                {
                    if (txt_rollno.Text.ToString().Trim() != "")
                    {
                        GetStudentDetails();
                        Sql = "Select distinct token_no as 'Token No',T.roll_no as 'Roll No',T.stud_name as 'Student Name',T.dept_name as 'Department Name' from tokendetails T, Registration R,Degree G,Course C,Department D,applyn a WHERE (T.Roll_No = R.Roll_No OR T.Roll_No = R.Lib_ID) AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND T.Is_Staff = 0 " + str + " and G.College_Code in(" + colcode1 + ")  order by T.roll_no";

                    }
                    else
                    {
                        Sql = "Select distinct token_no as 'Token No',T.roll_no as 'Roll No',T.stud_name as 'Student Name',T.dept_name as 'Department Name' from tokendetails T, Registration R,Degree G,Course C,Department D WHERE (T.Roll_No = R.Roll_No OR T.Roll_No = R.Lib_ID) AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND T.Is_Staff = 0 and R.Batch_Year in(" + Batch1 + ") and G.Course_Id in(" + Degree1 + ") and D.dept_code in(" + Branch1 + ") and G.College_Code in(" + colcode1 + ")   order by T.roll_no";
                    }

                    ds = d2.select_method_wo_parameter(Sql, "text");


                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        card.Columns.Add("SNo", typeof(string));
                        card.Columns.Add("Department Name", typeof(string));
                        card.Columns.Add("Roll No", typeof(string));
                        card.Columns.Add("Student Name", typeof(string));
                        card.Columns.Add("Token No", typeof(string));
                        int sno = 0;


                        for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                        {
                            sno++;
                            drlist = card.NewRow();
                            drlist["SNo"] = sno.ToString();
                            drlist["Department Name"] = ds.Tables[0].Rows[r]["Department Name"].ToString();
                            drlist["Roll No"] = ds.Tables[0].Rows[r]["Roll No"].ToString();
                            drlist["Student Name"] = ds.Tables[0].Rows[r]["Student Name"].ToString();
                            drlist["Token No"] = ds.Tables[0].Rows[r]["Token No"].ToString();
                            card.Rows.Add(drlist);
                        }


                        grdManualExit.DataSource = card;
                        grdManualExit.DataBind();
                      
                        grdManualExit.Visible = true;
                        rptprint1.Visible = true;
                    }

                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                        grdManualExit.Visible = false;
                        rptprint1.Visible = false;
                    }


                }
                else if (Rblreturn.SelectedIndex == 1)
                {

                    if (txt_staffcode.Text.ToString().Trim() != "")
                    {

                        Sql = "Select distinct token_no as 'Token No',td.roll_no as 'Staff Code',sm.staff_name as 'Staff Name',d.Dept_Name as 'Department Name',sc.category_name from tokendetails td, stafftrans st,staffmaster sm,hrdept_master d,staffCategorizer sc where is_staff = 1 and td.roll_no=st.staff_code and st.staff_code=sm.staff_code and td.roll_no=sm.staff_code and st.dept_code=d.dept_code and st.category_code =sc.category_code and sm.college_code=sc.college_code and sm.college_code=d.college_code and sc.college_code=d.college_code and td.roll_no in('" + txt_staffcode.Text.ToString().Trim() + "') and sm.college_code in(" + colcode1 + ") order by roll_no";

                    }
                    else
                    {


                        Sql = "Select distinct token_no as 'Token No',td.roll_no as 'Staff Code',sm.staff_name as 'Staff Name',d.Dept_Name as 'Department Name',sc.category_name from tokendetails td, stafftrans st,staffmaster sm,hrdept_master d,staffCategorizer sc where is_staff = 1 and td.roll_no=st.staff_code and st.staff_code=sm.staff_code and td.roll_no=sm.staff_code and st.dept_code=d.dept_code and st.category_code =sc.category_code and sm.college_code=sc.college_code and sm.college_code=d.college_code and sc.college_code=d.college_code and st.dept_code in(" + dept + ")  and st.category_code in(" + Category + ") and sm.college_code in(" + colcode1 + ") order by roll_no";

                    }
                    ds = d2.select_method_wo_parameter(Sql, "text");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        card.Columns.Add("SNo", typeof(string));
                        card.Columns.Add("Staff Name", typeof(string));
                        card.Columns.Add("Category Name", typeof(string));
                        card.Columns.Add("Department Name", typeof(string));
                        card.Columns.Add("Staff Code", typeof(string));
                        card.Columns.Add("Token No", typeof(string));


                        int sno = 0;
                        for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                        {
                            sno++;
                            drlist = card.NewRow();
                            drlist["SNo"] = sno.ToString();
                            drlist["Staff Name"] = ds.Tables[0].Rows[r]["Staff Name"].ToString();
                            drlist["Category Name"] = ds.Tables[0].Rows[r]["category_name"].ToString();
                            drlist["Department Name"] = ds.Tables[0].Rows[r]["Department Name"].ToString();
                            drlist["Staff Code"] = ds.Tables[0].Rows[r]["Staff Code"].ToString();
                            drlist["Token No"] = ds.Tables[0].Rows[r]["Token No"].ToString();

                            card.Rows.Add(drlist);
                        }


                        grdManualExit.DataSource = card;
                        grdManualExit.DataBind();
                      
                        grdManualExit.Visible = true;
                        rptprint1.Visible = true;
                    }

                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found";
                        grdManualExit.Visible = false;
                        rptprint1.Visible = false;
                    }

                }

            }

            if (grdManualExit.Rows.Count > 0)
            {

                CheckBox selectall = grdManualExit.Rows[0].FindControl("selectall") as CheckBox;
                selectall.Visible = true;
                CheckBox select = grdManualExit.Rows[0].FindControl("select") as CheckBox;
                select.Visible = false;

            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }

    }

  
    #endregion

    protected void grdManualExit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void grdManualExit_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdManualExit.PageIndex = e.NewPageIndex;
        btnMainGo_Click(sender, e);
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexcelname1.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(grdManualExit, report);
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
            }
            btnExcel1.Focus();
        }
        catch
        {

        }
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "LibraryMod_Card_list_and_holder";
            string pagename = "Card_list_and_holder.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(grdManualExit, pagename, attendance, 0, ss,"1");
            Printcontrol.Visible = true;
        }
        catch { }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Card_list_and_holder");
        }
    }


    #endregion
}