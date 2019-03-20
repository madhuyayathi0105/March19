using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Collections;
using System.Drawing;
using FarPoint.Web.Spread;
using System.Configuration;

public partial class LibraryMod_LibraryInformation : System.Web.UI.Page
{
    string usercollegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string groupusercode = string.Empty;
    string college_code = string.Empty;
    string lib_code = string.Empty;
    string dept_code = string.Empty;
    DataTable libinfo = new DataTable();
    Hashtable ht = new Hashtable();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DAccess2 da = new DAccess2();
    Dictionary<int, string> dicRowColor = new Dictionary<int, string>();

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
          
                usercollegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupusercode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
          
            if (!IsPostBack)
            {
                bindclg();
                getLibPrivil();
                department();
                txt_fromdate1.Attributes.Add("readonly", "readonly");
                txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate1.Attributes.Add("readonly", "readonly");
                txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch
        {
        }
    }

    #region BindHeaders

    public void bindclg()
    {
        try
        {
            ddlclg.Items.Clear();
            string columnfield = string.Empty;
            string group_user = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            ds = da.select_method("bind_college", ht, "sp");
            ddlclg.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = ds;
                ddlclg.DataValueField = "college_code";
                ddlclg.DataTextField = "collname";
                ddlclg.DataBind();
                ddlclg.SelectedIndex = 0;
            }
        }
        catch
        {
        }
    }

    public void getLibPrivil()
    {
        try
        {
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlclg.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            Hashtable hsLibcode = new Hashtable();
            if (singleuser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + usercode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupusercode.Split(';');
                if (groupUser.Length > 0)
                {
                    if (groupUser.Length == 1)
                    {
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code=" + groupUser[0] + "";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(sql, "text");
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
                        ds = da.select_method_wo_parameter(sql, "text");
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
            libcodecollection = "WHERE lib_code IN ('" + libcodecollection + "')";
        aa:
            LibCollection = libcodecollection;
            bindlibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void bindlibrary(string libcode)
    {
        try
        {
            ddllibrary.Items.Clear();
            college_code = Convert.ToString(ddlclg.SelectedValue);
            if (!string.IsNullOrEmpty(college_code))
            {
                string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " AND college_code=" + college_code + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds.Clear();
                ds = da.select_method_wo_parameter(lib_name, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_name";
                    ddllibrary.DataBind();
                    ddllibrary.Items.Insert(0, "All");
                }

            }
        }
        catch
        {
        }
    }

    public void department()
    {
        try
        {
            ddldept.Items.Clear();
            string dept = string.Empty;
            string lib = string.Empty;
            college_code = Convert.ToString(Session["collegecode"]);
            string lib_name = ddllibrary.SelectedItem.ToString();
            if (ddllibrary.SelectedIndex == 0)
            {
                lib = "select lib_name,lib_code from library where college_code='" + college_code + "'";
            }
            else
            {
                lib = "select lib_name,lib_code from library where college_code='" + college_code + "' and lib_name='" + lib_name + "'";
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(lib, "text");
            if (ddllibrary.SelectedIndex != 0)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lib_code = ds.Tables[0].Rows[0]["lib_code"].ToString().Trim();
                }
            }
            if (!string.IsNullOrEmpty(college_code))
            {
                if (ddllibrary.SelectedIndex == 0)
                {
                    dept = "SELECT DISTINCT dept_name from journal_dept where college_code ='" + college_code + "' order by dept_name";
                }
                else
                {
                    dept = "SELECT DISTINCT dept_name from journal_dept where college_code ='" + college_code + "' AND Lib_Code ='" + lib_code + "' order by dept_name";
                }
                ds.Clear();
                ds = da.select_method_wo_parameter(dept, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataValueField = "dept_name";
                    ddldept.DataTextField = "dept_name";
                    ddldept.DataBind();
                    ddldept.Items.Insert(0, "All");
                }
            }
        }
        catch
        {
        }
    }

    #endregion

    protected void ddlclg_Selectedindexchanged(object sender, EventArgs e)
    {
        getLibPrivil();
    }

    protected void ddllibrary_selectedindexchanged(object sender, EventArgs e)
    {
    }

    protected void ddldept_selectedindexchanged(object sender, EventArgs e)
    {

    }

    protected void gridview1_onselectedindexchanged(object sender, EventArgs e)
    {
    }

    protected void gridview1_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        gridview1.PageIndex = e.NewPageIndex;
        btnGo_Click(sender, e);
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string qry = string.Empty;
            string dept_code = Convert.ToString(ddldept.SelectedItem);
            string colgcode = Convert.ToString(Session["collegecode"]);
            string libraryname = Convert.ToString(ddllibrary.SelectedValue).Trim();
            DataRow dr2;

            divtable.Visible = true;
            string libcode = "select lib_name,lib_code from library where college_code='" + colgcode + "' and lib_name='" + libraryname + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lib_code = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }

            //**********Reference Books**************
            if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = "select count(*) as refbooks from bookdetails where ref='Yes'";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = "select count(*) as refbooks from bookdetails where ref='Yes' and lib_code='" + lib_code + "' and dept_code ='" + dept_code + "'";
            }
            else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = "select count(*) as refbooks from bookdetails where ref='Yes' and lib_code='" + lib_code + "'";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = "select count(*) as refbooks from bookdetails where ref='Yes' and dept_code ='" + dept_code + "'";
            }
            if (cbdate1.Checked == true)
            {
                qry = qry + " and bill_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "'";
            }
            //***********


            //*************Text Books*********
            if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = qry + " select count(*) as txtbooks from bookdetails where ref='No'";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = qry + " select count(*) as txtbooks from bookdetails where ref='No' and lib_code='" + lib_code + "' and dept_code ='" + dept_code + "'";
            }
            else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = qry + " select count(*) as txtbooks from bookdetails where ref='No' and lib_code='" + lib_code + "'";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = qry + " select count(*) as txtbooks from bookdetails where ref='No' and dept_code ='" + dept_code + "'";
            }

            if (cbdate1.Checked == true)
            {
                qry = qry + " and bill_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "'";
            }
            //**************


            //*********No of Titles(Single)**********
            if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
            {

                qry = qry + " select sum(a.tit) as titsi from (select count(distinct title) tit from bookdetails where 1=1 ";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = qry + " select sum(a.tit) as titsi from (select count(distinct title) tit from bookdetails where lib_code='" + lib_code + "' and dept_code ='" + dept_code + "'";
            }
            else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = qry + " select sum(a.tit) as titsi from (select count(distinct title) tit from bookdetails where lib_code ='" + lib_code + "'";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = qry + " select sum(a.tit) as titsi from (select count(distinct title) tit from bookdetails where dept_code ='" + dept_code + "'";
            }
            if (cbdate1.Checked == true)
            {
                qry = qry + " and bill_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "' ";
            }
            qry = qry + " group by dept_code) a";
            //**********************

            //**********No Of Volumes**********
            if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = qry + " select count(title) as title from bookdetails where 1=1";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = qry + " select count(title) as title from bookdetails where lib_code='" + lib_code + "' and dept_code ='" + dept_code + "'";
            }
            else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = qry + " select count(title) as title from bookdetails where lib_code='" + lib_code + "'";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = qry + " select count(title) as title from bookdetails where dept_code ='" + dept_code + "'";
            }

            if (cbdate1.Checked == true)
            {
                qry = qry + " and bill_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "'";
            }
            //*********

            //****Book Categories*****
            if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = qry + " select distinct category,count(*) as bokcateg from bookdetails where 1=1";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = qry + " select distinct category,count(*) as bokcateg from bookdetails where lib_code='" + lib_code + "' and dept_code ='" + dept_code + "'";
            }
            else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = qry + " select distinct category,count(*) as bokcateg from bookdetails where lib_code='" + lib_code + "'";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = qry + " select distinct category,count(*) as bokcateg from bookdetails where dept_code ='" + dept_code + "'";
            }
            if (cbdate1.Checked == true)
            {
                qry = qry + " and bill_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "' ";
            }
            qry = qry + " group by category";
            //**************

            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");
            libinfo.Columns.Add("Contents", typeof(string));
            libinfo.Columns.Add("Availability", typeof(string));
            dr2 = libinfo.NewRow();
            dr2["Contents"] = "Contents";
            dr2["Availability"] = "Availability";

            libinfo.Rows.Add(dr2);
            dr2 = libinfo.NewRow();
            if (ds.Tables.Count > 0)
            {
                dr2 = libinfo.NewRow();
                dr2["Contents"] = "Books :";
                dicRowColor.Add(libinfo.Rows.Count, "Books :");
                libinfo.Rows.Add(dr2);

                dr2 = libinfo.NewRow();
                dr2["Contents"] = "No. of Volumes";
                dr2["Availability"] = Convert.ToString(ds.Tables[3].Rows[0]["title"]);
                libinfo.Rows.Add(dr2);
                dr2 = libinfo.NewRow();
                dr2["Contents"] = "No. of Titles(Single)";
                dr2["Availability"] = Convert.ToString(ds.Tables[2].Rows[0]["titsi"]);
                libinfo.Rows.Add(dr2);
                dr2 = libinfo.NewRow();
                dr2["Contents"] = "Reference Books";
                dr2["Availability"] = Convert.ToString(ds.Tables[0].Rows[0]["refbooks"]);
                libinfo.Rows.Add(dr2);
                dr2 = libinfo.NewRow();
                dr2["Contents"] = "Text Books";
                dr2["Availability"] = Convert.ToString(ds.Tables[1].Rows[0]["txtbooks"]);
                libinfo.Rows.Add(dr2);
                dr2 = libinfo.NewRow();
                libinfo.Rows.Add(dr2);
            }
            dr2["Contents"] = "Book Categories :";
            dicRowColor.Add(libinfo.Rows.Count - 1, "Book Categories :");
            for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
            {
                dr2 = libinfo.NewRow();
                dr2["Contents"] = Convert.ToString(ds.Tables[4].Rows[i]["category"]);
                dr2["Availability"] = Convert.ToString(ds.Tables[4].Rows[i]["bokcateg"]);
                libinfo.Rows.Add(dr2);
            }

            //*****Non Book Materials*********
            if (ddllibrary.SelectedIndex == 0)
            {

                if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
                {
                    qry = " select count(*) as nonbukmat from nonbookmat where issue_flag <> 'Lost'";
                }
                else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
                {
                    qry = " select count(*) as nonbukmat from nonbookmat where issue_flag <> 'Lost' and lib_code='" + lib_code + "' and dept_code ='" + dept_code + "'";
                }
                else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
                {
                    qry = " select count(*) as nonbukmat from nonbookmat where issue_flag <> 'Lost' and lib_code='" + lib_code + "'";
                }
                else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
                {
                    qry = " select count(*) as nonbukmat from nonbookmat where issue_flag <> 'Lost'  and department='" + dept_code + "'";
                }
                if (cbdate1.Checked == true)
                {
                    qry = qry + " and mon_year between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "'";
                }

                if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
                {
                    qry = qry + " select distinct attachment, count(*) as nonbukmat from nonbookmat where issue_flag <> 'Lost' and attachment<>'Nil' and attachment is not null";
                }
                else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
                {
                    qry = qry + " select distinct attachment, count(*) as nonbukmat from nonbookmat where issue_flag <> 'Lost' and lib_code='" + lib_code + "' and attachment<>'Nil' and attachment is not null and department='" + dept_code + "'";
                }
                else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
                {
                    qry = qry + " select distinct attachment, count(*) as nonbukmat from nonbookmat where issue_flag <> 'Lost' and lib_code='" + lib_code + "'  and attachment<>'Nil' and attachment is not null";
                }
                else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
                {
                    qry = qry + " select distinct attachment, count(*) as nonbukmat from nonbookmat where issue_flag <> 'Lost' and  attachment<>'Nil' and attachment is not null and department='" + dept_code + "'";
                }

                if (cbdate1.Checked == true)
                {
                    qry = qry + " and mon_year between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "' ";
                }
                qry = qry + " group by attachment";

                ds.Clear();
                ds = da.select_method_wo_parameter(qry, "text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i1 = 0; i1 < ds.Tables[0].Rows.Count; i1++)
                    {
                        dr2 = libinfo.NewRow();
                        dr2["Contents"] = "Non Book Materials";
                        dicRowColor.Add(libinfo.Rows.Count, "Non Book Materials");
                        dr2["Availability"] = Convert.ToString(ds.Tables[0].Rows[i1]["nonbukmat"]);
                        libinfo.Rows.Add(dr2);
                    }
                    for (int l1 = 0; l1 < ds.Tables[1].Rows.Count; l1++)
                    {
                        dr2 = libinfo.NewRow();
                        dr2["Contents"] = Convert.ToString(ds.Tables[1].Rows[l1]["attachment"]);
                        dr2["Availability"] = Convert.ToString(ds.Tables[1].Rows[l1]["nonbukmat"]);
                        libinfo.Rows.Add(dr2);
                    }
                }
            }
            //****************

            //*****Thesis*******
            if (ddllibrary.SelectedIndex == 0)
            {
                if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
                {
                    qry = " select count(*) as thesis from project_book where issue_flag <> 'Lost'";
                }
                else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
                {
                    qry = " select count(*) as thesis from project_book where issue_flag <> 'Lost' and lib_code='" + lib_code + "' and dept_code ='" + dept_code + "'";
                }
                else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
                {
                    qry = " select count(*) as thesis from project_book where issue_flag <> 'Lost' and lib_code='" + lib_code + "'";
                }
                else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
                {
                    qry = " select count(*) as thesis from project_book where issue_flag <> 'Lost' and degree_code='" + dept_code + "'";
                }

                //*******

                //*********Journals********

                //****Indian******
                if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
                {
                    qry = qry + " select count(*) as journalct from journal_master where is_national=1";
                }
                else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
                {
                    qry = qry + " select count(*) as journalct from journal_master where is_national=1 and lib_code='" + lib_code + "' and dept_code ='" + dept_code + "'";
                }
                else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
                {
                    qry = qry + " select count(*) as journalct from journal_master where is_national=1 and lib_code='" + lib_code + "'";
                }
                else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
                {
                    qry = qry + " select count(*) as journalct from journal_master where is_national=1 and department='" + dept_code + "'";
                }


                //****Foreign*****
                if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
                {
                    qry = qry + " select count(*) as journalct1 from journal_master where is_national=0";
                }
                else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
                {
                    qry = qry + " select count(*) as journalct1 from journal_master where is_national=0 and lib_code='" + lib_code + "' and dept_code ='" + dept_code + "'";
                }
                else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
                {
                    qry = qry + " select count(*) as journalct1 from journal_master where is_national=0 and lib_code='" + lib_code + "'";
                }
                else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
                {
                    qry = qry + " select count(*) as journalct1 from journal_master where is_national=0 and department='" + dept_code + "'";
                }

                //**********

                //*******Question Bank*******

                qry = qry + " SELECT COUNT(*) as questbank FROM University_Question WHERE 1=1 ";
                if (ddldept.SelectedIndex != 0)
                {
                    qry = qry + " AND Dept ='" + dept_code + "'";
                }
                if (ddllibrary.SelectedIndex != 0)
                {
                    qry = qry + " AND Lib_Code ='" + lib_code + "'";
                }

                ds.Clear();
                ds = da.select_method_wo_parameter(qry, "text");

                if (ds.Tables.Count > 0)
                {
                    for (int i2 = 0; i2 < ds.Tables[0].Rows.Count; i2++)
                    {
                        dr2 = libinfo.NewRow();
                        dr2["Contents"] = "Thesis";
                        dicRowColor.Add(libinfo.Rows.Count, "Thesis");
                        dr2["Availability"] = Convert.ToString(ds.Tables[0].Rows[i2]["thesis"]);
                        libinfo.Rows.Add(dr2);
                    }
                    dr2 = libinfo.NewRow();
                    dr2["Contents"] = "Journals";
                    dicRowColor.Add(libinfo.Rows.Count, "Journals");
                    libinfo.Rows.Add(dr2);
                    for (int i3 = 0; i3 < ds.Tables[1].Rows.Count; i3++)
                    {
                        dr2 = libinfo.NewRow();
                        dr2["Contents"] = "Indian";
                        libinfo.Rows.Add(dr2);
                    }
                    for (int i5 = 0; i5 < ds.Tables[2].Rows.Count; i5++)
                    {
                        dr2 = libinfo.NewRow();
                        dr2["Contents"] = "Foreign";
                        dr2["Availability"] = Convert.ToString(ds.Tables[2].Rows[i5]["journalct1"]);
                        libinfo.Rows.Add(dr2);
                    }
                    for (int j = 0; j < ds.Tables[3].Rows.Count; j++)
                    {
                        dr2 = libinfo.NewRow();
                        dr2["Contents"] = "Question Bank";
                        dicRowColor.Add(libinfo.Rows.Count, "Question Bank");
                        dr2["Availability"] = Convert.ToString(ds.Tables[3].Rows[j]["questbank"]);
                        libinfo.Rows.Add(dr2);
                    }
                }
            }

            //***Competitive Books****
            if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = " select count(*) as competbook from bookdetails  where 1=1 and call_no like '001 %'";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = " select count(*) as competbook from bookdetails where lib_code='" + lib_code + "' and dept_code ='" + dept_code + "' and call_no like '001 %'";
            }
            else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = " select count(*) as competbook from bookdetails where lib_code='" + lib_code + "' and call_no like '001 %' ";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = " select count(*) as competbook from bookdetails where  dept_code ='" + dept_code + "' and call_no like '001 %'";
            }

            if (cbdate1.Checked == true)
            {
                qry = qry + " and bill_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "'";
            }
            //**********

            //*****Book Status*****
            if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = qry + " select distinct book_status,count(*) as bokst from bookdetails  where 1=1";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = qry + " select distinct book_status,count(*) as bokst from bookdetails where lib_code='" + lib_code + "' and dept_code ='" + dept_code + "'";
            }
            else if (ddldept.SelectedIndex == 0 && ddllibrary.SelectedIndex != 0)
            {
                qry = qry + " select distinct book_status,count(*) as bokst from bookdetails where lib_code='" + lib_code + "'";
            }
            else if (ddldept.SelectedIndex != 0 && ddllibrary.SelectedIndex == 0)
            {
                qry = qry + " select distinct book_status,count(*) as bokst from bookdetails where  dept_code ='" + dept_code + "'";
            }

            if (cbdate1.Checked == true)
            {
                qry = qry + " and bill_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "' ";
            }
            qry = qry + " group by book_status";
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0)
            {
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    dr2 = libinfo.NewRow();
                    dr2["Contents"] = "Competitive Books :";
                    dicRowColor.Add(libinfo.Rows.Count, "Competitive Books :");
                    dr2["Availability"] = Convert.ToString(ds.Tables[0].Rows[0]["competbook"]);
                    libinfo.Rows.Add(dr2);
                }
                dr2 = libinfo.NewRow();
                dr2["Contents"] = "Book Status :";
                dicRowColor.Add(libinfo.Rows.Count, "Book Status :");
                libinfo.Rows.Add(dr2);
                for (int i1 = 0; i1 < ds.Tables[1].Rows.Count; i1++)
                {
                    dr2 = libinfo.NewRow();
                    dr2["Contents"] = Convert.ToString(ds.Tables[1].Rows[i1]["book_status"]);
                    dr2["Availability"] = Convert.ToString(ds.Tables[1].Rows[i1]["bokst"]);
                    libinfo.Rows.Add(dr2);
                }
            }

            gridview1.DataSource = libinfo;
            gridview1.DataBind();
            gridview1.Visible = true;
            divtable.Visible = true;
            foreach (KeyValuePair<int, string> dr in dicRowColor)
            {
                int g = dr.Key;
                string DicValue = dr.Value;
                if (DicValue == "Book Status :")
                {
                    gridview1.Rows[g].ForeColor = ColorTranslator.FromHtml("&HFF0000");
                }
                if (DicValue == "Competitive Books :")
                {
                    gridview1.Rows[g].ForeColor = ColorTranslator.FromHtml("&HFF0000");
                }
                if (DicValue == "Question Bank")
                {
                    gridview1.Rows[g].ForeColor = Color.Blue;
                }
                if (DicValue == "Journals")
                {
                    gridview1.Rows[g].ForeColor = Color.Blue;
                }
                if (DicValue == "Thesis")
                {
                    gridview1.Rows[g].ForeColor = Color.Blue;
                }
                if (DicValue == "Non Book Materials")
                {
                    gridview1.Rows[g].ForeColor = Color.Blue;
                }
                if (DicValue == "Book Categories :")
                {
                    gridview1.Rows[g].ForeColor = ColorTranslator.FromHtml("&HFF0000");
                }
                if (DicValue == "Books :")
                {
                    gridview1.Rows[g].ForeColor = Color.Blue;
                }
            }
            div_report.Visible = true;
            div_report.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Visible = true;
            RowHead(gridview1);
        }
        catch (Exception ex)
        {
        }
    }

    protected void RowHead(GridView gridview1)
    {
        for (int head = 0; head < 1; head++)
        {
            gridview1.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gridview1.Rows[head].Font.Bold = true;
            gridview1.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    protected void cbdate1_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbdate1.Checked == true)
        {
            txt_fromdate1.Enabled = true;
            txt_todate1.Enabled = true;
        }
        else
        {
            txt_fromdate1.Enabled = false;
            txt_todate1.Enabled = false;
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
        catch { }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(gridview1, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch
        {

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string libinformation = "Library Information";
            string pagename = "LibraryInformation.aspx";
            Printcontrol1.loadspreaddetails(gridview1, pagename, libinformation);
            Printcontrol1.Visible = true;
        }
        catch { }
    }


}
