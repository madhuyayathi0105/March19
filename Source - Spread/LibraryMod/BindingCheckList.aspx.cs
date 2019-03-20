using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;
using System.IO;
using System.Data.OleDb;
using System.Configuration;


public partial class LibraryMod_BindingCheckList : System.Web.UI.Page
{
    string usercollegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string groupusercode = string.Empty;
    string college_code = string.Empty;
    string lib_code = string.Empty;
    string booktype = string.Empty;
    DataTable bindcheck = new DataTable();
    DataRow drbind;
    Hashtable ht = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproStoreAccess storeAcc = new InsproStoreAccess();

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
           
                usercollegecode = Session["collegecode"] != null ? Convert.ToString(Session["collegecode"]) : "";
                usercode = Session["usercode"] != null ? Convert.ToString(Session["usercode"]) : "";
                singleuser = Session["single_user"] != null ? Convert.ToString(Session["single_user"]) : "";
                groupusercode = Session["group_code"] != null ? Convert.ToString(Session["group_code"]) : "";
          
            if (!IsPostBack)
            {
                bindclg();
                getLibPrivil();
                Txtdate.Attributes.Add("readonly", "readonly");
                Txtdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
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
            dtCommon.Clear();

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
                ddlclg.DataSource = dtCommon;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                ddlclg.SelectedIndex = 0;
                ddlclg.Enabled = true;
            }



            //ddlclg.Items.Clear();
            //string columnfield = string.Empty;
            //string group_user = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            //if (group_user.Contains(";"))
            //{
            //    string[] group_semi = group_user.Split(';');
            //    group_user = Convert.ToString(group_semi[0]);
            //}
            //if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            //{
            //    columnfield = " and group_code='" + group_user + "'";
            //}
            //else if (Session["usercode"] != null)
            //{
            //    columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            //}
            //ht.Clear();
            //ht.Add("column_field", Convert.ToString(columnfield));
            //ds = da.select_method("bind_college", ht, "sp");
            //ddlclg.Items.Clear();
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlclg.DataSource = ds;
            //    ddlclg.DataValueField = "college_code";
            //    ddlclg.DataTextField = "collname";
            //    ddlclg.DataBind();
            //    ddlclg.SelectedIndex = 0;
            //}

        }
        catch
        {
        }
    }

    public void bindlibrary(string LibCollection)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            if (ddlclg.Items.Count > 0)
            {
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(college_code))
                        {
                            college_code = "'" + li.Value + "'";
                        }
                        else
                        {
                            college_code = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(college_code))
            {

                string lib = "select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND college_code=" + college_code + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataValueField = "lib_name";
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataBind();

                }
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
            //libcodecollection = Left(libcodecollection, Len(libcodecollection) - 1);
            libcodecollection = "WHERE lib_code IN ('" + libcodecollection + "')";
        aa:
            LibCollection = libcodecollection;

            bindlibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    protected void ddllib_OnSelectedChanged(object sender, EventArgs e)
    {
    }

    protected void rblbind_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }

    public static DataSet Excelconvertdataset(string path)
    {
        DataSet ds3 = new DataSet();
        string StrSheetName = string.Empty;

        string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';";
        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
        try
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();

            if (excelConnection.State == ConnectionState.Closed)
                excelConnection.Open();

            DataTable dtSheets = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dtSheets != null && dtSheets.Rows.Count > 0)
            {
                StrSheetName = dtSheets.Rows[0].ItemArray[2].ToString();

            }
            if (!string.IsNullOrEmpty(StrSheetName))
            {
                OleDbCommand cmd = new OleDbCommand("Select * from [" + StrSheetName + "]", excelConnection);
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(ds3, "excelData");
                adapter.Dispose();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (excelConnection.State != ConnectionState.Closed)
                excelConnection.Close();
        }
        return ds3;
    }

    protected void btnimport_OnClick(object sender, EventArgs e)
    {
        try
        {
            string conStr = "";
            DataSet ds1 = new DataSet();
            if (fileupload1.FileName != "" && fileupload1.FileName != null)
            {
                using (Stream stream = this.fileupload1.FileContent as Stream)
                {
                    string extension = Path.GetFileName(fileupload1.PostedFile.FileName);
                    if (extension.ToString() != "")
                    {
                        if (System.IO.Path.GetExtension(fileupload1.FileName) == ".xls" || System.IO.Path.GetExtension(fileupload1.FileName) == ".xlsx")
                        {
                            string path = Server.MapPath("~/ImportFiles/" + System.IO.Path.GetFileName(fileupload1.FileName));
                            textbox.Text = path;
                            stream.Position = 0;
                            fileupload1.SaveAs(path);
                            ds1.Clear();
                            ds1 = Excelconvertdataset(path);
                            DataTable dt = ds1.Tables[0];
                            gridview1.Caption = Path.GetFileName(path);
                            gridview1.DataSource = dt;
                            gridview1.DataBind();
                            divtable.Visible = true;
                            gridview1.Visible = true;
                            btnSave.Visible = true;
                        }
                        else
                        {
                            lblAlertMsg.Visible = true;
                            lblAlertMsg.Text = "File is not an Excel file or is locked and cannot be imported.(Invalid File Type or Locked)";
                            btnPopAlertClose.Visible = true;
                            divPopupAlert.Visible = true;
                            divAlertContent.Visible = true;
                        }
                    }
                }
            }
            else
            {
                //FpSpread1.Visible = false;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Select any Excel file and then proceed  ";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
            }
        }
        catch
        {
        }

    }

    public void import()
    {
        try
        {
            Boolean rollflag = false;
            Boolean stro = false;
            string errorroll = string.Empty;
            DataSet dsimport = new DataSet();
            int getstuco = 0;
            double maxMarks = 0;


            if (fileupload1.FileName != "" && fileupload1.FileName != null)
            {
                if (fileupload1.FileName.EndsWith(".xls") || fileupload1.FileName.EndsWith(".xlsx"))
                {
                    using (Stream stream = this.fileupload1.FileContent as Stream)
                    {
                        string extension = Path.GetFileName(fileupload1.PostedFile.FileName);
                        string filname = System.IO.Path.GetExtension(fileupload1.FileName);
                        // string path = Server.MapPath("~/Import/abc" + System.IO.Path.GetExtension(fpmarkexcel.FileName));
                        OleDbDataAdapter adapter = new OleDbDataAdapter();
                        string path = Server.MapPath("~/Importfiles/" + extension);
                        string appPath = path.Replace("\\", "/");
                        fileupload1.SaveAs(appPath);

                        //string extension = Path.GetFileName(fpmarkexcel.PostedFile.FileName);
                        dsimport.Clear();
                        dsimport = Excelconvertdataset(path);

                        stream.Position = 0;

                    }
                    bool entry = false;
                    for (int c = 1; c < dsimport.Tables[0].Columns.Count; c++)
                    {

                    }
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Please Select The File and Then Proceed";
                }

                btnSave.Visible = true;

            }
        }


        catch (Exception ex)
        {


        }
    }

    protected void gridview1_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        gridview1.PageIndex = e.NewPageIndex;
        btngo_OnClick(sender, e);
    }

    protected void gridview1_onselectedindexchanged(object sender, EventArgs e)
    {


    }

    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            string qry = string.Empty;

            libcode();
            string date = Txtdate.Text;
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(date);
            date = dt.ToString("MM/dd/yyyy");
            if (ddllib.SelectedIndex == 0)
            {
                qry = "SELECT ISNULL(C.AccNo,'') AccNo,ISNULL(B.Title,'') Title,ISNULL(B.Author,'') Author,ISNULL(B.Price,0) Price,ISNULL(B.Call_No,'') Call_No FROM BindingCheckList_Tbl C INNER JOIN BookDetails B ON B.Acc_No = C.AccNo WHERE B.Lib_Code ='" + lib_code + "' AND C.EntryDate ='" + Convert.ToString(date) + "' ORDER BY B.Call_No";
            }
            else if (ddllib.SelectedIndex == 1)
            {
                qry = "SELECT ISNULL(AccNo,'') AccNo,ISNULL(B.Title,'') as Title,'' as Author,0 as Price,'' as Call_No FROM BindingCheckList_Tbl C INNER JOIN Journal B ON B.Journal_Code = C.AccNo WHERE B.Lib_Code ='" + lib_code + "' AND C.EntryDate ='" + Convert.ToString(date) + "' ORDER BY B.Call_No";
            }
            else if (ddllib.SelectedIndex == 2)
            {
                qry = "SELECT ISNULL(C.AccNo,'') AccNo,ISNULL(B.Title,'') Title,'' as Author,0 as Price,'' as Call_No FROM BindingCheckList_Tbl C INNER JOIN Project_Book B ON B.ProBook_AccNo = C.AccNo WHERE B.Lib_Code ='" + lib_code + "' AND C.EntryDate ='" + Convert.ToString(date) + "' ORDER BY B.Title";
            }
            else if (ddllib.SelectedIndex == 3)
            {
                qry = "SELECT ISNULL(C.AccNo,'') AccNo,ISNULL(B.Title,'') Title,ISNULL(B.Author,'') as Author,ISNULL(B.Price,0) as Price,'' as Call_No FROM BindingCheckList_Tbl C INNER JOIN NonBookMat B ON B.NonBookMat_No = C.AccN WHERE B.Lib_Code ='" + lib_code + "' AND C.EntryDate ='" + Convert.ToString(date) + "' ORDER BY B.NonBookMat_No";
            }
            else if (ddllib.SelectedIndex == 4)
            {
                qry = "SELECT ISNULL(AccNo,'') AccNo,ISNULL(B.Title,'') as Title,'' as Author,0 as Price,'' as Call_No FROM BindingCheckList_Tbl C INNER JOIN University_Question B ON B.Access_Code = C.AccNo WHERE B.Lib_Code ='" + lib_code + "' AND C.EntryDate ='" + Convert.ToString(date) + "' ORDER BY B.Call_No";
            }
            else
            {
                qry = "SELECT ISNULL(C.AccNo,'') AccNo,ISNULL(B.Title,'') Title,'' as Author,0 as Price,'' as Call_No FROM BindingCheckList_Tbl C INNER JOIN Back_Volume B ON B.Access_Code = C.AccNo WHERE B.Lib_Code ='" + lib_code + "' AND C.EntryDate ='" + Convert.ToString(date) + "' ORDER BY B.Call_No";
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                bindcheck.Columns.Add("SNo", typeof(string));
                bindcheck.Columns.Add("Access No", typeof(string));
                bindcheck.Columns.Add("Title", typeof(string));
                bindcheck.Columns.Add("Author", typeof(string));
                bindcheck.Columns.Add("Price", typeof(string));
                bindcheck.Columns.Add("Call No", typeof(string));

              

                int sno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drbind = bindcheck.NewRow();
                    string accno = Convert.ToString(ds.Tables[0].Rows[i]["AccNo"]);
                    string title = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                    string author = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                    string price = Convert.ToString(ds.Tables[0].Rows[i]["Price"]);
                    string callno = Convert.ToString(ds.Tables[0].Rows[i]["Call_No"]);

                    drbind["SNo"] = Convert.ToString(sno);
                    drbind["Access No"] = accno;
                    drbind["Title"] = title;
                    drbind["Author"] = author;
                    drbind["Price"] = price;
                    drbind["Call No"] = callno;
                    bindcheck.Rows.Add(drbind);
                }
                gridview1.DataSource = bindcheck;
                gridview1.DataBind();
                gridview1.Visible = true;
                div_report.Visible = true;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                gridview1.Visible = true;
                divPopupAlert.Visible = false;
                divAlertContent.Visible = false;
                btnPopAlertClose.Visible = false;
                lblAlertMsg.Visible = false;
            }
            else
            {
                gridview1.Visible = false;

                divtable.Visible = false;
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                btnPopAlertClose.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
            }

        }
        catch
        {
        }
    }

   

    public void boktype()
    {
        if (ddllib.SelectedIndex == 0)
        {
            booktype = "BOK";
        }
        else if (ddllib.SelectedIndex == 1)
        {
            booktype = "PER";
        }
        else if (ddllib.SelectedIndex == 2)
        {
            booktype = "PRO";
        }
        else if (ddllib.SelectedIndex == 3)
        {
            booktype = "NBM";
        }
        else if (ddllib.SelectedIndex == 4)
        {
            booktype = "QBA";
        }
        else if (ddllib.SelectedIndex == 5)
        {
            booktype = "BVO";
        }
        else if (ddllib.SelectedIndex == 6)
        {
            booktype = "REF";
        }
    }

    public void libcode()
    {
        string libraryname = Convert.ToString(ddllibrary.SelectedValue).Trim();

        if (ddlclg.Items.Count > 0)
        {
            foreach (ListItem li in ddlclg.Items)
            {
                if (li.Selected)
                {
                    if (string.IsNullOrEmpty(college_code))
                    {
                        college_code = "'" + li.Value + "'";
                    }
                    else
                    {
                        college_code = ",'" + li.Value + "'";
                    }
                }
            }
        }

        string libcode = "select lib_name,lib_code from library where college_code=" + college_code + " and lib_name='" + libraryname + "'";
        ds.Clear();
        ds = da.select_method_wo_parameter(libcode, "text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            lib_code = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
        }

    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string insertqry = string.Empty;
            int insertqry1 = 0;

            string date = Txtdate.Text;
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(date);
            date = dt.ToString("MM/dd/yyyy");

            boktype();
            libcode();
            string bindingstatus = string.Empty;
            string bindingstat = string.Empty;
            if (rblbind.SelectedIndex == 0)
            {
                bindingstatus = "1";
                bindingstat = "Binding";
            }
            else if (rblbind.SelectedIndex == 1)
            {
                bindingstatus = "0";
                bindingstat = "Available";
            }
            string autocode = string.Empty;
            int max_value = 0;
            string auto_code = string.Empty;
            string code = "SELECT MAX(Code) as code From BindingCheckList_Tbl Where College_Code =" + college_code + "";
            ds.Clear();
            ds = da.select_method_wo_parameter(code, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    autocode = Convert.ToString(ds.Tables[0].Rows[i]["code"]);

                }
                if (autocode == "")
                {
                    max_value = max_value + 1;
                    auto_code = Convert.ToString(max_value);
                }
                else
                {
                    autocode = autocode.Substring(0);
                    max_value = Convert.ToInt32(autocode) + 1;
                    auto_code = Convert.ToString(max_value);
                }
            }
            for (int j = 1; j <= gridview1.Rows.Count - 1; j++)
            {
                string colname = gridview1.Rows[0].Cells[j].Text.ToString();
            }
            for (int i = 0; i < gridview1.Rows.Count; i++)
            {
                if (rblbind.SelectedIndex == 0)
                {
                    string acc_no = Convert.ToString(gridview1.Rows[i].Cells[1].Text);
                    string tit1 = "";
                    string authors = "";
                    string prices = "";
                    string callno = "";

                    insertqry = "if not exists(select * from BindingCheckList_Tbl where Code='" + auto_code + "' and EntryDate='" + date + "' and AccNo= '" + acc_no + "' and Lib_Code='" + lib_code + "' and College_Code=" + college_code + " and Book_Type='" + booktype + "' and  BindingStatus= '" + bindingstatus + "') INSERT INTO BindingCheckList_Tbl(Code,EntryDate,AccNo,Lib_Code,College_Code,Book_Type,BindingStatus) values('" + auto_code + "','" + date + "','" + acc_no + "','" + lib_code + "'," + college_code + ",'" + booktype + "','" + bindingstatus + "') else update BindingCheckList_Tbl set Code='" + auto_code + "' , EntryDate='" + date + "'   ,  BindingStatus= '" + bindingstatus + "' where  AccNo= '" + acc_no + "' and  Lib_Code='" + lib_code + "' and College_Code=" + college_code + " and Book_Type='" + booktype + "'";

                    insertqry = insertqry + " UPDATE BookDetails SET Book_Status ='" + bindingstat + "' WHERE Acc_No ='" + acc_no + "' AND Lib_Code ='" + lib_code + "'";
                    insertqry1 = da.update_method_wo_parameter(insertqry, "text");
                }
                else
                {
                    string acc_no = Convert.ToString(gridview1.Rows[i].Cells[1].Text);
                    string returnqry = "update bookdetails set book_status = 'Available' where bookdetails.acc_no  ='" + acc_no + "' and lib_code = '" + lib_code + "'";
                    returnqry = returnqry + " delete from BindingCheckList_Tbl where AccNo = '" + acc_no + "'";
                    insertqry1 = da.update_method_wo_parameter(returnqry, "text");
                }
            }
            if (insertqry1 == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Not Saved";

            }
            else
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Saved Sucessfully";
                gridview1.Visible = false;
                textbox.Text = string.Empty;
                btnSave.Visible = false;
            }

        }
        catch
        {
        }
    }

    protected void ddllibrary_OnSelectedChanged(object sender, EventArgs e)
    {
    }

    protected void ddlclg_OnSelectedChanged(object sender, EventArgs e)
    {
        getLibPrivil();
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string bindchecklist = "Binding Check List";
            string pagename = "BindingCheckList.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(gridview1, pagename, bindchecklist, 0, ss,"1");
            Printcontrolhed2.Visible = true;
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
        catch
        {
        }
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
                lbl_norec.Visible = true;
                lbl_norec.Text = "Please Enter Your Report Name";
            }
            btn_Excel.Focus();
        }
        catch
        {
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            lblAlertMsg.Visible = false;
            divPopupAlert.Visible = false;
        }
        catch
        {
        }
    }

}