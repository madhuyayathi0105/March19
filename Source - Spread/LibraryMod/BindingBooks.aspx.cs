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
using System.Configuration;

public partial class LibraryMod_BindingBooks : System.Web.UI.Page
{
    string usercollegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string groupusercode = string.Empty;
    string college_code = string.Empty;
    string librcode = string.Empty;
    int insertqry1;
    bool flag_true = false;
    DataTable dtserialno = new DataTable();
    DataRow drserail;
    Hashtable ht = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    DateTime dat;
    DateTime dat1;
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    int selectedCellIndex = 0;
    DataTable bindbok = new DataTable();
    DataRow drbind;

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
                bindserialnum();
                txtbindorderdt.Attributes.Add("readonly", "readonly");
                txtbindorderdt.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtexpdate.Attributes.Add("readonly", "readonly");
                txtexpdate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                if (rbltype.SelectedIndex == 0)
                {
                    BtnPeriodical.Visible = true;
                    btnBook.Visible = false;
                }
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

    public void bindserialnum()
    {
        try
        {
            int max_value = 0;
            string maxserial = string.Empty;
            string serno = string.Empty;
            int ser_no;
            string serno1 = string.Empty;
            string serial = string.Empty;
            string colgcode = Convert.ToString(Session["collegecode"]);
            string libraryname = Convert.ToString(ddllibrary.SelectedValue).Trim();
            string libcode = "select lib_name,lib_code from library where college_code='" + colgcode + "' and lib_name='" + libraryname + "'";

            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                librcode = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }
            // string qry = "select distinct isnull(serial_no,0) as ser_no from binding where lib_code = '" + librcode + "'";
            string qry = "select distinct(serial_no) as ser_no,CAST(RIGHT(serial_no, LEN(serial_no) - PATINDEX('%[0-9]%', serial_no)+1) AS INT), LEFT(serial_no, PATINDEX('%[0-9]%', serial_no)-1) FROM binding where lib_code = '" + librcode + "'   ORDER BY CAST(RIGHT(serial_no, LEN(serial_no) - PATINDEX('%[0-9]%', serial_no)+1) AS INT), LEFT(serial_no, PATINDEX('%[0-9]%', serial_no)-1)";
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    serial = Convert.ToString(ds.Tables[0].Rows[i]["ser_no"]);

                }
                maxserial = serial;
                serno = maxserial.Substring(3);
                ser_no = Convert.ToInt32(serno) + 1;
                serno1 = "BIN" + Convert.ToString(ser_no);

            }
            else
            {
                serno1 = "BIN" + (max_value + 1);

            }
            txtserialno.Text = serno1;



        }
        catch
        {
        }
    }

    #endregion

    protected void rbl_typeSelectedindex(object sender, EventArgs e)
    {
        if (rbltype.SelectedIndex == 1)
        {
            btnBook.Visible = true;
            BtnPeriodical.Visible = false;
        }
        else
        {
            btnBook.Visible = false;
            BtnPeriodical.Visible = true;
        }
    }

    protected void ddlclg_selectedindexchange(object sender, EventArgs e)
    {
        getLibPrivil();
    }

    protected void ddllib_selectedindexchange(object sender, EventArgs e)
    {
    }

    public void serialnobtn()
    {
        try
        {
            string serialno = string.Empty;

            int sno = 0;
            string colgcode = Convert.ToString(Session["collegecode"]);
            string libraryname = Convert.ToString(ddllibrary.SelectedValue).Trim();
            string libcode = "select lib_name,lib_code from library where college_code='" + colgcode + "' and lib_name='" + libraryname + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                librcode = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }

            if (rbltype.SelectedIndex == 0)
            {
                serialno = "select distinct(binding.serial_no),binding.binding_date FROM binding  where binding.lib_code ='" + librcode + "'";
                // INNER JOIN journal ON (binding.access_code = journal.access_code) AND (binding.lib_code = journal.lib_code)
            }
            else
            {
                serialno = "SELECT distinct(binding.serial_no),binding.binding_date FROM binding   where binding.lib_code ='" + librcode + "'";
                // INNER JOIN bookdetails ON (binding.access_code =bookdetails.acc_no AND binding.lib_code = bookdetails.lib_code)
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(serialno, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dtserialno.Columns.Add("Serial No", typeof(string));
                dtserialno.Columns.Add("Binding Order Date", typeof(string));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drserail = dtserialno.NewRow();
                    string serialn = Convert.ToString(ds.Tables[0].Rows[i]["serial_no"]).Trim();
                    string bindingdate = Convert.ToString(ds.Tables[0].Rows[i]["binding_date"]).Trim();

                    drserail["Serial No"] = serialn;
                    drserail["Binding Order Date"] = bindingdate;
                    dtserialno.Rows.Add(drserail);


                }
                gridview1.DataSource = dtserialno;
                gridview1.DataBind();
                gridview1.Visible = true;
                div1.Visible = true;

            }
            else
            {
                divserialno.Visible = false;
                gridview1.Visible = false;
                div1.Visible = false;
                divtable.Visible = false;
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";

            }

        }
        catch
        {
        }
    }

    public void serialnobn()
    {
        try
        {
            string serial = string.Empty;
            string maxserial = string.Empty;
            int ser_no;
            string serno = string.Empty;
            string serno1 = string.Empty;
            int max_value = 0;

            string qry = "select distinct(serial_no) as ser_no,CAST(RIGHT(serial_no, LEN(serial_no) - PATINDEX('%[0-9]%', serial_no)+1) AS INT), LEFT(serial_no, PATINDEX('%[0-9]%', serial_no)-1) FROM binding where lib_code = '" + librcode + "'   ORDER BY CAST(RIGHT(serial_no, LEN(serial_no) - PATINDEX('%[0-9]%', serial_no)+1) AS INT), LEFT(serial_no, PATINDEX('%[0-9]%', serial_no)-1)";
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    serial = Convert.ToString(ds.Tables[0].Rows[i]["ser_no"]);

                }
                maxserial = serial;
                serno = maxserial.Substring(3);
                ser_no = Convert.ToInt32(serno) + 1;
                serno1 = "BIN" + Convert.ToString(ser_no);

            }
            else
            {
                serno1 = "BIN" + (max_value + 1);

            }
            txtserialno.Text = serno1;
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
        catch { }

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(gridview2, report);
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
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Binding Books";
            string pagename = "BindingBooks.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(gridview2, pagename, attendance, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch { }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }


    protected void gridview1_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void gridview1_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            int sno = 0;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string serlno = gridview1.Rows[rowIndex].Cells[1].Text;
            txtserialno.Text = serlno;
            divserialn.Visible = false;
            bindbok.Columns.Add("Access No", typeof(string));
            bindbok.Columns.Add("Book Title", typeof(string));
            bindbok.Columns.Add("Author", typeof(string));
            bindbok.Columns.Add("Edition", typeof(string));

            DataTable perdt = new DataTable();
            DataRow drper;
            perdt.Columns.Add("Access Code", typeof(string));
            perdt.Columns.Add("Title", typeof(string));
            perdt.Columns.Add("Volumn No", typeof(string));
            perdt.Columns.Add("Issue No", typeof(string));
            perdt.Columns.Add("Department", typeof(string));
            string sel = "  select * from binding where serial_no='" + serlno + "'";
            ds = da.select_method_wo_parameter(sel, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string select = "select acc_no,title,author,edition from bookdetails where acc_no='" + Convert.ToString(ds.Tables[0].Rows[i]["access_code"]) + "'";
                    DataSet dss = new DataSet();
                    dss = da.select_method_wo_parameter(select, "text");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        sno++;
                        drbind = bindbok.NewRow();
                        drbind["Access No"] = Convert.ToString(dss.Tables[0].Rows[0]["acc_no"]);
                        drbind["Book Title"] = Convert.ToString(dss.Tables[0].Rows[0]["title"]);
                        drbind["Author"] = Convert.ToString(dss.Tables[0].Rows[0]["author"]);
                        drbind["Edition"] = Convert.ToString(dss.Tables[0].Rows[0]["edition"]);
                        bindbok.Rows.Add(drbind);
                    }
                    else
                    {
                        select = "select access_code,title,volume_no,issue_no,dept_name,journal_code from journal where access_code='" + Convert.ToString(ds.Tables[0].Rows[i]["access_code"]) + "'";
                        dss = da.select_method_wo_parameter(select, "text");
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            sno++;
                            drper = perdt.NewRow();
                            string accescode = Convert.ToString(dss.Tables[0].Rows[0]["access_code"]).Trim();
                            string title1 = Convert.ToString(dss.Tables[0].Rows[0]["title"]).Trim();
                            string volno = Convert.ToString(dss.Tables[0].Rows[0]["volume_no"]).Trim();
                            string issueno = Convert.ToString(dss.Tables[0].Rows[0]["issue_no"]).Trim();
                            string dept = Convert.ToString(dss.Tables[0].Rows[0]["dept_name"]).Trim();

                            drper["Access Code"] = accescode;
                            drper["Title"] = title1;
                            drper["Volumn No"] = volno;
                            drper["Issue No"] = issueno;
                            drper["Department"] = dept;

                            perdt.Rows.Add(drper);
                        }
                    }
                }
                if (bindbok.Rows.Count > 0)
                {
                    gridview2.DataSource = bindbok;
                    gridview2.DataBind();
                    gridview2.Visible = true;
                    divtable.Visible = true;
                }
                else
                {
                    gridview4.DataSource = perdt;
                    gridview4.DataBind();
                    gridview4.Visible = true;
                    divtable.Visible = true;
                }

            }

        }
        catch
        {
        }
    }

    protected void gridview4_OnPageIndexChanged(object sender, GridViewRowEventArgs e)
    {
        // gridview4.PageIndex = e.NewPageIndex;
        btnpergo_click(sender, e);
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        lblAlertMsg.Visible = false;
        divPopupAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    protected void btnbindreturn_onclick(object sender, EventArgs e)
    {
        try
        {
            btnreturnbind.Enabled = true;
            int returnqry1 = 0;
            string returnqry = string.Empty;
            string colgcode = Convert.ToString(Session["collegecode"]);
            string libraryname = Convert.ToString(ddllibrary.SelectedValue).Trim();
            string libcode = "select lib_name,lib_code from library where college_code='" + colgcode + "' and lib_name='" + libraryname + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                librcode = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }
            string acc_code = string.Empty;
            string dateorder = string.Empty;
            foreach (GridViewRow row in gridview2.Rows)
            {
                CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                int RowCnt = Convert.ToInt32(row.RowIndex);
                if (cbsel.Checked == true)
                {
                    if (rbltype.SelectedIndex == 0)
                    {
                        acc_code = Convert.ToString(gridview2.Rows[RowCnt].Cells[2].Text);
                        returnqry = "update journal set bind_flag = 'No',issue_flag='Available' where access_code in (select access_code from binding where lib_code = '" + librcode + "' and serial_no = '" + txtserialno.Text + "')  and lib_code = '" + librcode + "' and access_code ='" + acc_code + "'";
                        returnqry = returnqry + " delete from binding where serial_no = '" + txtserialno.Text + "' and lib_code = '" + librcode + "' and access_code='" + acc_code + "' and  binding_date='" + txtbindorderdt.Text + "'";
                    }
                    else
                    {
                        acc_code = Convert.ToString(gridview2.Rows[RowCnt].Cells[2].Text);
                       

                        returnqry = "update bookdetails set book_status = 'Available' where bookdetails.acc_no  ='" + acc_code + "' and lib_code = '" + librcode + "'";
                        returnqry = returnqry + " delete from binding where serial_no = '" + txtserialno.Text + "' and lib_code = '" + librcode + "' and access_code='" + acc_code + "' and binding_date='" + txtbindorderdt.Text + "'";

                    }
                    returnqry1 = da.update_method_wo_parameter(returnqry, "text");
                }
            }
            if (returnqry1 == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Binding Return Information Not Saved";
            }
            else
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Binding Return information saved sucessfully";
                gridview2.Visible = false;
                divtable.Visible = false;
                txtcompany.Text = string.Empty;
                txtemailid.Text = string.Empty;
                txtphone.Text = string.Empty;
                txtaddress.Text = string.Empty;
                btnreturnbind.Enabled = false;
                serialnobn();
            }
        }
        catch
        {
        }
    }

    protected void btnbind_onclick(object sender, EventArgs e)
    {
        try
        {
            string insertqry = string.Empty;

            string colgcode = Convert.ToString(Session["collegecode"]);
            string libraryname = Convert.ToString(ddllibrary.SelectedValue).Trim();
            string libcode = "select lib_name,lib_code from library where college_code='" + colgcode + "' and lib_name='" + libraryname + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                librcode = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }

            if (txtcompany.Text == null || txtcompany.Text == "")
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please enter the binding company name";
                return;
            }
            else if (ddllibrary.SelectedIndex < 0)
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Select Library";
            }
            else if (txtserialno.Text == "")
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Serial No Not Generated";
            }
            else if (rbltype.SelectedIndex < 0)
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Select Periodicals / Books";
            }
            else
            {
                int count = 0;

                foreach (GridViewRow row in gridview2.Rows)
                {
                    CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                    int RowCnt = Convert.ToInt32(row.RowIndex);

                    if (cbsel.Checked == true)
                    {
                        count = count + 1;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                if (count == 0)
                {
                    divPopupAlert.Visible = true;
                    divAlertContent.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Select atleast one entry";
                    return;
                }


            }
            string currdate = string.Empty;
            string currtime = string.Empty;

            currdate = DateTime.Now.ToString("yyyy/MM/dd");
            currtime = DateTime.Now.ToString("hh:mm tt");


            string acc_code = string.Empty;
            string dateorder = string.Empty;


            foreach (GridViewRow row in gridview2.Rows)
            {
                CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                int RowCnt = Convert.ToInt32(row.RowIndex);
                if (cbsel.Checked == true)
                {
                    if (rbltype.SelectedIndex == 0)
                    {
                        acc_code = Convert.ToString(gridview2.Rows[RowCnt].Cells[2].Text);
                        dateorder = Convert.ToString(gridview2.Rows[RowCnt].Cells[7].Text);
                        insertqry = "if not exists(select * from binding where access_date='" + currdate + "' and access_time='" + currtime + "' and serial_no='" + txtserialno.Text + "' and binding_date='" + dateorder + "' and access_code='" + acc_code + "' and flag1='No' and flag2='No' and lib_code='" + librcode + "' and cname='" + txtcompany.Text + "' and caddress='" + txtaddress.Text + "' and Expected_date='" + txtexpdate.Text + "' and cphone='" + txtphone.Text + "' and cemail='" + txtemailid.Text + "') insert into binding values('" + currdate + "','" + currtime + "','" + txtserialno.Text + "','" + dateorder + "','" + acc_code + "','No','No','" + librcode + "','" + txtcompany.Text + "','" + txtaddress.Text + "','" + txtexpdate.Text + "','" + txtphone.Text + "','" + txtemailid.Text + "') else update binding set access_date='" + currdate + "' , access_time='" + currtime + "' , serial_no='" + txtserialno.Text + "' , binding_date='" + dateorder + "' , access_code='" + acc_code + "' , flag1='No' , flag2='No' , lib_code='" + librcode + "' , cname='" + txtcompany.Text + "' , caddress='" + txtaddress.Text + "' , Expected_date='" + txtexpdate.Text + "' , cphone='" + txtphone.Text + "' , cemail='" + txtemailid.Text + "'";
                        insertqry = insertqry + " update journal set bind_flag = 'Yes',issue_flag='Binding' where access_code = '" + acc_code + "' and lib_code = '" + librcode + "'";

                    }
                    else
                    {
                        acc_code = Convert.ToString(gridview2.Rows[RowCnt].Cells[2].Text);
                        dateorder = txtbindorderdt.Text;

                        insertqry = "if not exists(select * from binding where access_date='" + currdate + "' and access_time='" + currtime + "' and serial_no='" + txtserialno.Text + "' and binding_date='" + dateorder + "' and access_code='" + acc_code + "' and flag1='No' and flag2='No' and lib_code='" + librcode + "' and cname='" + txtcompany.Text + "' and caddress='" + txtaddress.Text + "' and Expected_date='" + txtexpdate.Text + "' and cphone='" + txtphone.Text + "' and cemail='" + txtemailid.Text + "') insert into binding values('" + currdate + "','" + currtime + "','" + txtserialno.Text + "','" + dateorder + "','" + acc_code + "','No','No','" + librcode + "','" + txtcompany.Text + "','" + txtaddress.Text + "','" + txtexpdate.Text + "','" + txtphone.Text + "','" + txtemailid.Text + "') else update binding set access_date='" + currdate + "' , access_time='" + currtime + "' , serial_no='" + txtserialno.Text + "' , binding_date='" + dateorder + "' , access_code='" + acc_code + "' , flag1='No' , flag2='No' , lib_code='" + librcode + "' , cname='" + txtcompany.Text + "' , caddress='" + txtaddress.Text + "' , Expected_date='" + txtexpdate.Text + "' , cphone='" + txtphone.Text + "' , cemail='" + txtemailid.Text + "'";
                        insertqry = insertqry + " update bookdetails set book_status = 'Binding' where acc_no = '" + acc_code + "' and lib_code = '" + librcode + "'";

                    }
                    insertqry1 = da.update_method_wo_parameter(insertqry, "text");
                }
            }

            if (insertqry1 == 0)
            {

                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Binding Information Not Saved";
            }
            else
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Binding Information Saved Successfully";
                txtcompany.Enabled = false;
                txtphone.Enabled = false;
                txtemailid.Enabled = false;
                txtaddress.Enabled = false;
                btnreturnbind.Enabled = false;
                btnbind.Enabled = false;
                divtable.Visible = false;
                txtcompany.Text = string.Empty;
                serialnobtn();
                serialnobn();
                btnreturnbind.Enabled = true;
            }


        }
        catch
        {
        }
    }

    protected void btnBook_onclick(object sender, EventArgs e)
    {
        try
        {
            divselectbook.Visible = true;
            divselectbook1.Visible = true;
            binddept();
            if (ddlsearch.SelectedIndex == 0)
            {
                txtsearch.Visible = false;
            }
            else
            {
                txtsearch.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void BtnPeriodical_onclick(object sender, EventArgs e)
    {
        try
        {
            divselectperiodicals.Visible = true;
            divselperiodicals.Visible = true;
            if (ddlpersearch.SelectedIndex == 0)
            {
                txtpersearch.Visible = false;
            }
            else
            {
                txtpersearch.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void btnexit3_click(object sender, EventArgs e)
    {
        divselectperiodicals.Visible = false;
        divselperiodicals.Visible = false;

    }

    protected void gridview4_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void gridview4_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }

    protected void gridview3_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void gridview3_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }

    #region Serialno ButtonClicks
    protected void btnserialno_onclick(object sender, EventArgs e)
    {
        try
        {
            divserialno.Visible = true;
            divserialn.Visible = true;
            serialnobtn();
            btnreturnbind.Enabled = true;
        }
        catch
        {
        }
    }
    protected void btngoserial_click(object sender, EventArgs e)
    {
        try
        {
            int sno = 0;
            string serialno = string.Empty;
            if (rbltype.SelectedIndex == 0)
            {
                serialno = "select distinct(binding.serial_no),binding.binding_date FROM binding INNER JOIN journal ON (binding.access_code = journal.access_code) AND (binding.lib_code = journal.lib_code) where binding.serial_no like '" + txtserialno1.Text + "%' and binding.lib_code ='" + librcode + "'";
            }
            else
            {
                serialno = "SELECT distinct(binding.serial_no),binding.binding_date FROM binding INNER JOIN bookdetails ON (binding.access_code =bookdetails.acc_no AND binding.lib_code = bookdetails.lib_code) where binding.serial_no like '" + txtserialno1.Text + "%' and binding.lib_code ='" + librcode + "'";
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(serialno, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dtserialno.Columns.Add("Serial No", typeof(string));
                dtserialno.Columns.Add("Binding Order Date", typeof(string));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drserail = dtserialno.NewRow();
                    string serialn = Convert.ToString(ds.Tables[0].Rows[i]["serial_no"]).Trim();
                    string bindingdate = Convert.ToString(ds.Tables[0].Rows[i]["binding_date"]).Trim();

                    drserail["Serial No"] = serialn;
                    drserail["Binding Order Date"] = bindingdate;
                    dtserialno.Rows.Add(drserail);


                }
                gridview1.DataSource = dtserialno;
                gridview1.DataBind();
                gridview1.Visible = true;
                div1.Visible = true;
                div_report.Visible = true;

            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";

            }
        }
        catch
        {
        }
    }

    protected void btnexit_click(object sender, EventArgs e)
    {
        divserialno.Visible = false;
        divserialn.Visible = false;
    }
    protected void btnOk_click(object sender, EventArgs e)
    {
        try
        {
            btnok.Visible = true;
            string compyname = string.Empty;
            string phoneno = string.Empty;
            string emailid = string.Empty;
            string address = string.Empty;
            string activerow = "";
            string activecol = "";
            string qry1 = string.Empty;
            string bindorderdate = string.Empty;
            string sernum = string.Empty;
            string colgcode = Convert.ToString(Session["collegecode"]);
            string libraryname = Convert.ToString(ddllibrary.SelectedValue).Trim();
            string libcode = "select lib_name,lib_code from library where college_code='" + colgcode + "' and lib_name='" + libraryname + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                librcode = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }

            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            if (Convert.ToString(rowIndex) != "" && Convert.ToString(rowIndex) != "-1")
            {
                bindorderdate = Convert.ToString(gridview1.Rows[rowIndex].Cells[2].Text);
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(bindorderdate);
                bindorderdate = dt.ToShortDateString();
                sernum = Convert.ToString(gridview1.Rows[rowIndex].Cells[1].Text);
            }

            if (rbltype.SelectedIndex == 0)
            {
                qry1 = "SELECT DISTINCT binding.serial_no,binding.binding_date,journal.access_code,journal.title,journal.volume_no,journal.issue_no,journal.dept_name,binding.cname,binding.caddress FROM binding INNER JOIN journal ON (binding.access_code = journal.access_code) AND (binding.lib_code = journal.lib_code) where binding.serial_no = '" + sernum + "' and binding.lib_code = '" + librcode + "' and binding.binding_date='" + bindorderdate + "'";
            }
            else
            {
                qry1 = "SELECT distinct binding.serial_no,binding.binding_date,bookdetails.acc_no,bookdetails.title,bookdetails.author,bookdetails.edition,binding.cname,binding.caddress,cphone,cemail FROM bookdetails INNER JOIN binding ON (binding.access_code =bookdetails.acc_no ) AND (binding.lib_code = bookdetails.lib_code) where binding.serial_no = '" + sernum + "' and binding.lib_code = '" + librcode + "'and binding.binding_date='" + bindorderdate + "'";
            }

            ds.Clear();
            ds = da.select_method_wo_parameter(qry1, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {


                if (rbltype.SelectedIndex == 1)
                {
                    bindbok.Columns.Add("Access No", typeof(string));
                    bindbok.Columns.Add("Book Title", typeof(string));
                    bindbok.Columns.Add("Author", typeof(string));
                    bindbok.Columns.Add("Edition", typeof(string));
                    bindbok.Columns.Add("Binding Order Date", typeof(string));
                    int sno = 0;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sno++;
                        drbind = bindbok.NewRow();
                        string accesno = Convert.ToString(ds.Tables[0].Rows[i]["acc_no"]).Trim();
                        string booktit = Convert.ToString(ds.Tables[0].Rows[i]["title"]).Trim();
                        string author = Convert.ToString(ds.Tables[0].Rows[i]["author"]).Trim();
                        string edition = Convert.ToString(ds.Tables[0].Rows[i]["edition"]).Trim();
                        string bindorderdt = Convert.ToString(ds.Tables[0].Rows[i]["binding_date"]).Trim();
                        dat = Convert.ToDateTime(bindorderdt);
                        bindorderdt = dat.ToString("dd/MM/yyyy");
                        compyname = Convert.ToString(ds.Tables[0].Rows[i]["cname"]).Trim();
                        address = Convert.ToString(ds.Tables[0].Rows[i]["caddress"]).Trim();
                        phoneno = Convert.ToString(ds.Tables[0].Rows[i]["cphone"]).Trim();
                        emailid = Convert.ToString(ds.Tables[0].Rows[i]["cemail"]).Trim();

                        drbind["Access No"] = accesno;
                        drbind["Book Title"] = booktit;
                        drbind["Author"] = author;
                        drbind["Edition"] = edition;
                        drbind["Binding Order Date"] = bindorderdt;
                        bindbok.Rows.Add(drbind);
                    }
                    gridview2.DataSource = bindbok;
                    gridview2.DataBind();
                    gridview2.Visible = true;
                }

                else
                {
                    bindbok.Columns.Add("Access Code", typeof(string));
                    bindbok.Columns.Add("Title", typeof(string));
                    bindbok.Columns.Add("Volumn No", typeof(string));
                    bindbok.Columns.Add("Issue No", typeof(string));
                    bindbok.Columns.Add("Department", typeof(string));
                    bindbok.Columns.Add("Binding Order Date", typeof(string));


                    int sno = 0;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sno++;
                        drbind = bindbok.NewRow();
                        string accescode = Convert.ToString(ds.Tables[0].Rows[i]["access_code"]).Trim();
                        string title1 = Convert.ToString(ds.Tables[0].Rows[i]["title"]).Trim();
                        string volno = Convert.ToString(ds.Tables[0].Rows[i]["volume_no"]).Trim();
                        string issueno = Convert.ToString(ds.Tables[0].Rows[i]["issue_no"]).Trim();
                        string dept = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]).Trim();
                        string bindorderdt = Convert.ToString(ds.Tables[0].Rows[i]["binding_date"]).Trim();
                        dat = Convert.ToDateTime(bindorderdt);
                        bindorderdt = dat.ToString("dd/MM/yyyy");
                        compyname = Convert.ToString(ds.Tables[0].Rows[i]["cname"]).Trim();
                        address = Convert.ToString(ds.Tables[0].Rows[i]["caddress"]).Trim();

                        drbind["Access Code"] = accescode;
                        drbind["Title"] = title1;
                        drbind["Volumn No"] = volno;
                        drbind["Issue No"] = issueno;
                        drbind["Department"] = dept;
                        drbind["Binding Order Date"] = bindorderdt;
                        bindbok.Rows.Add(drbind);
                    }
                }
                gridview2.DataSource = bindbok;
                gridview2.DataBind();
                gridview2.Visible = true;

            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                divtable.Visible = true;
                gridview2.Visible = true;

                //btn_printmaster.Visible = true;
                //btn_Excel.Visible = true;
                //txt_excelname.Visible = true;
                div_report.Visible = true;
                //lbl_reportname.Visible = true;
                divserialno.Visible = false;
                divserialn.Visible = false;
                txtbindorderdt.Text = bindorderdate;
                txtbindorderdt.Enabled = false;
                txtcompany.Text = compyname;
                txtcompany.Enabled = false;
                txtemailid.Text = emailid;
                txtemailid.Enabled = false;
                txtphone.Text = phoneno;
                txtphone.Enabled = false;
                txtaddress.Text = address;
                txtaddress.Enabled = false;
                txtserialno.Text = sernum;
                btnreturnbind.Enabled = true;


            }
            else
            {
                divtable.Visible = false;
                gridview2.Visible = false;
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                //btn_printmaster.Visible = false;
                //btn_Excel.Visible = false;
                //txt_excelname.Visible = false;
                div_report.Visible = false;
                // lbl_reportname.Visible = false;
                divserialno.Visible = false;
                divserialn.Visible = false;
            }

        }

        catch
        {
        }

    }

    #endregion

    #region Select Book

    public void binddept()
    {
        try
        {
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
                string dept = " select distinct(dept_name) from journal_dept where college_code =" + college_code + "";
                ds = da.select_method_wo_parameter(dept, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataTextField = "dept_name";
                    ddldept.DataValueField = "dept_name";
                    ddldept.DataBind();
                    ddldept.Items.Insert(0, "All");
                }
            }

        }
        catch
        {
        }

    }

    protected void btnselectbooks_click(object sender, EventArgs e)
    {
        try
        {
            string selqry = string.Empty;
            string colgcode = Convert.ToString(Session["collegecode"]);
            string libraryname = Convert.ToString(ddllibrary.SelectedValue).Trim();
            string libcode = "select lib_name,lib_code from library where college_code='" + colgcode + "' and lib_name='" + libraryname + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                librcode = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }

            if (ddlsearch.SelectedIndex == 0)
            {
                selqry = "select acc_no,title,author,edition from bookdetails where lib_code = '" + librcode + "' and book_status = 'Available'";

            }
            else if (ddlsearch.SelectedIndex == 1)
            {
                selqry = "select acc_no,title,author,edition from bookdetails where title like '" + txtsearch.Text + "%'  and book_status = 'Available' and lib_code = '" + librcode + "'";
            }
            else if (ddlsearch.SelectedIndex == 2)
            {
                selqry = "select acc_no,title,author,edition from bookdetails where author like '" + txtsearch.Text + "%'  and book_status = 'Available' and lib_code = '" + librcode + "'";
            }
            else
            {
                selqry = "select acc_no,title,author,edition from bookdetails where acc_no = '" + txtsearch.Text + "'  and book_status = 'Available' and lib_code = '" + librcode + "'";
            }
            if (ddldept.SelectedIndex == 0)
            {
                selqry = selqry;
            }
            else
            {
                selqry = selqry + " and dept_code = '" + Convert.ToString(ddldept.SelectedItem).Trim() + "'";
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(selqry, "text");
            DataTable selbok = new DataTable();
            DataRow drsel;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                selbok.Columns.Add("Access", typeof(string));
                selbok.Columns.Add("Title", typeof(string));
                selbok.Columns.Add("Author", typeof(string));
                selbok.Columns.Add("Edition", typeof(string));



                int sno = 0;
                int ii = 0;
                for (int i = ii; i < ds.Tables[0].Rows.Count; i++)
                {

                    sno++;
                    drsel = selbok.NewRow();
                    string accesno = Convert.ToString(ds.Tables[0].Rows[i]["acc_no"]).Trim();
                    string booktit = Convert.ToString(ds.Tables[0].Rows[i]["title"]).Trim();
                    string author = Convert.ToString(ds.Tables[0].Rows[i]["author"]).Trim();
                    string edition = Convert.ToString(ds.Tables[0].Rows[i]["edition"]).Trim();


                    drsel["Access"] = accesno;
                    drsel["Title"] = booktit;
                    drsel["Author"] = author;
                    drsel["Edition"] = edition;

                    selbok.Rows.Add(drsel);
                }
                gridview3.DataSource = selbok;
                gridview3.DataBind();
                gridview3.Visible = true;




                if (ds.Tables[0].Rows.Count > 0)
                {

                    gridview3.Visible = true;
                    div2.Visible = true;
                    btnselbokexit.Visible = true;
                    btnselbokok.Visible = true;

                }
                else
                {


                    div2.Visible = false;
                    gridview3.Visible = false;
                    btnselbokok.Visible = false;
                    btnselbokexit.Visible = false;
                    divPopupAlert.Visible = true;
                    divAlertContent.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Records Found";
                }

            }

        }
        catch
        {
        }
    }
    protected void btnex_click(object sender, EventArgs e)
    {
        divselectbook.Visible = false;
        divselectbook1.Visible = false;
    }
    protected void ddlsearch_selectedindex(object sender, EventArgs e)
    {
        if (ddlsearch.SelectedIndex == 0)
        {
            txtsearch.Visible = false;
        }
        else
        {
            txtsearch.Visible = true;
        }
    }
    protected void ddldept_selectedindex(object sender, EventArgs e)
    {
    }

    protected void btnselbokexit_click(object sender, EventArgs e)
    {
        divselectbook.Visible = false;
        divselectbook1.Visible = false;
        clearselbok();
    }
    public void clearselbok()
    {
        div2.Visible = false;
        txtsearch.Visible = false;

    }
    protected void btnselbokok_click(object sender, EventArgs e)
    {
        try
        {

            int sno = 0;
            string access = string.Empty;
            string title1 = string.Empty;
            string author1 = string.Empty;
            string edition = string.Empty;


            bindbok.Columns.Add("Access No", typeof(string));
            bindbok.Columns.Add("Book Title", typeof(string));
            bindbok.Columns.Add("Author", typeof(string));
            bindbok.Columns.Add("Edition", typeof(string));

            foreach (GridViewRow row in gridview3.Rows)
            {
                CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                int RowCnt = Convert.ToInt32(row.RowIndex);
                if (cbsel.Checked == true)
                {
                    access = Convert.ToString(gridview3.Rows[RowCnt].Cells[2].Text);
                    title1 = Convert.ToString(gridview3.Rows[RowCnt].Cells[3].Text);
                    author1 = Convert.ToString(gridview3.Rows[RowCnt].Cells[4].Text);
                    edition = Convert.ToString(gridview3.Rows[RowCnt].Cells[5].Text);
                    if (!string.IsNullOrEmpty(access) || !string.IsNullOrEmpty(title1) || !string.IsNullOrEmpty(author1) || !string.IsNullOrEmpty(edition))
                    {
                        sno++;
                        drbind = bindbok.NewRow();
                        drbind["Access No"] = access;
                        drbind["Book Title"] = title1;
                        drbind["Author"] = author1;
                        drbind["Edition"] = edition;
                        bindbok.Rows.Add(drbind);
                    }
                }
            }
            gridview2.DataSource = bindbok;
            gridview2.DataBind();
            gridview2.Visible = true;

            divtable.Visible = true;
            gridview2.Visible = true;

            //btn_printmaster.Visible = true;
            //btn_Excel.Visible = true;
            //txt_excelname.Visible = true;
            div_report.Visible = true;
            //lbl_reportname.Visible = true;
            divselectbook.Visible = false;
            divselectbook1.Visible = false;
            txtcompany.Enabled = true;
            txtemailid.Enabled = true;
            txtphone.Enabled = true;
            txtaddress.Enabled = true;
            btnbind.Enabled = true;
            clearselbok();

        }
        catch
        {
        }
    }

    #endregion

    #region Select Periodicals
    protected void btnperexit_click(object sender, EventArgs e)
    {
        clearselbok1();
        divselectperiodicals.Visible = false;
        divselperiodicals.Visible = false;
    }
    protected void btnperok_click(object sender, EventArgs e)
    {
        try
        {

            int sno = 0;
            string access = string.Empty;
            string title1 = string.Empty;
            string volno = string.Empty;
            string issueno = string.Empty;
            string deptartment = string.Empty;


            bindbok.Columns.Add("Access Code", typeof(string));
            bindbok.Columns.Add("Title", typeof(string));
            bindbok.Columns.Add("Volumn No", typeof(string));
            bindbok.Columns.Add("Issue No", typeof(string));
            bindbok.Columns.Add("Department", typeof(string));
            bindbok.Columns.Add("Binding Order Date", typeof(string));

            int selectedrow = 30;
            int selected = 0;
            //var grid = (GridView)sender;
            //GridViewRow selectedRow = grid.SelectedRow;
            //int rowIndex = grid.SelectedIndex;
            //int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

           

            foreach (GridViewRow row in gridview4.Rows)
            {
                CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                int RowCnt = Convert.ToInt32(row.RowIndex);
                if (cbsel.Checked == true)
                {

                    access = Convert.ToString(gridview4.Rows[RowCnt].Cells[2].Text);
                    title1 = Convert.ToString(gridview4.Rows[RowCnt].Cells[3].Text);
                    volno = Convert.ToString(gridview4.Rows[RowCnt].Cells[4].Text);
                    issueno = Convert.ToString(gridview4.Rows[RowCnt].Cells[5].Text);
                    deptartment = Convert.ToString(gridview4.Rows[RowCnt].Cells[6].Text);
                    if (deptartment == "&nbsp;")
                    {
                        deptartment = "";
                    }

                    if (!string.IsNullOrEmpty(access) || !string.IsNullOrEmpty(title1) || !string.IsNullOrEmpty(volno) || !string.IsNullOrEmpty(deptartment) || !string.IsNullOrEmpty(deptartment))
                    {
                        sno++;
                        drbind = bindbok.NewRow();
                        //selectedrow++;
                        drbind["Access Code"] = access;
                        drbind["Title"] = title1;
                        drbind["Volumn No"] = volno;
                        drbind["Issue No"] = issueno;
                        drbind["Department"] = deptartment;
                        drbind["Binding Order Date"] = Convert.ToString(txtbindorderdt.Text); ;
                        bindbok.Rows.Add(drbind);
                    }

                    gridview2.DataSource = bindbok;
                    gridview2.DataBind();
                    gridview2.Visible = true;

                }


            }

            divtable.Visible = true;
            gridview2.Visible = true;

            //FpSpread1.Height = selectedrow + totalrows;

            //btn_printmaster.Visible = true;
            //btn_Excel.Visible = true;
            //txt_excelname.Visible = true;
            div_report.Visible = true;
            //lbl_reportname.Visible = true;
            divselectperiodicals.Visible = false;
            divselperiodicals.Visible = false;
            txtcompany.Enabled = true;
            txtemailid.Enabled = true;
            txtphone.Enabled = true;
            txtaddress.Enabled = true;
            btnbind.Enabled = true;
            btnreturnbind.Enabled = false;
            clearselbok1();
        }
        catch
        {
        }
    }
    protected void btnpergo_click(object sender, EventArgs e)
    {
        try
        {
            string perqry = string.Empty;
            string colgcode = Convert.ToString(Session["collegecode"]);
            string libraryname = Convert.ToString(ddllibrary.SelectedValue).Trim();
            string libcode = "select lib_name,lib_code from library where college_code='" + colgcode + "' and lib_name='" + libraryname + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                librcode = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }
            if (ddlpersearch.SelectedIndex == 0)
            {
                perqry = "select access_code,title,volume_no,issue_no,dept_name,journal_code from journal where bind_flag = 'No' and issue_flag <> 'Binding' and lib_code = '" + librcode + "'";
            }
            else if (ddlpersearch.SelectedIndex == 1)
            {
                perqry = "select access_code,title,volume_no,issue_no,dept_name,journal_code from journal where bind_flag = 'No' and issue_flag <> 'Binding' and lib_code = '" + librcode + "' and  access_code = '" + txtpersearch.Text + "'";
            }
            else if (ddlpersearch.SelectedIndex == 2)
            {
                perqry = "select access_code,title,volume_no,issue_no,dept_name,journal_code from journal where bind_flag = 'No' and issue_flag <> 'Binding' and lib_code = '" + librcode + "' and title like '" + txtpersearch.Text + "%'";
            }
            else
            {
                perqry = "select access_code,title,volume_no,issue_no,dept_name,journal_code from journal where bind_flag = 'No' and issue_flag <> 'Binding' and lib_code = '" + librcode + "' and dept_name like  '" + txtpersearch.Text + "%'";
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(perqry, "text");
            DataTable perdt = new DataTable();
            DataRow drper;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                perdt.Columns.Add("Access Code", typeof(string));
                perdt.Columns.Add("Title", typeof(string));
                perdt.Columns.Add("Volumn No", typeof(string));
                perdt.Columns.Add("Issue No", typeof(string));
                perdt.Columns.Add("Department", typeof(string));


                int sno = 0;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drper = perdt.NewRow();
                    string accescode = Convert.ToString(ds.Tables[0].Rows[i]["access_code"]).Trim();
                    string title1 = Convert.ToString(ds.Tables[0].Rows[i]["title"]).Trim();
                    string volno = Convert.ToString(ds.Tables[0].Rows[i]["volume_no"]).Trim();
                    string issueno = Convert.ToString(ds.Tables[0].Rows[i]["issue_no"]).Trim();
                    string dept = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]).Trim();

                    drper["Access Code"] = accescode;
                    drper["Title"] = title1;
                    drper["Volumn No"] = volno;
                    drper["Issue No"] = issueno;
                    drper["Department"] = dept;

                    perdt.Rows.Add(drper);
                }
                gridview4.DataSource = perdt;
                gridview4.DataBind();
                gridview4.Visible = true;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {

                gridview4.Visible = true;
                divperspread.Visible = true;
                btnperok.Visible = true;
                btnperexit.Visible = true;

            }
            else
            {
                divperspread.Visible = false;
                gridview4.Visible = false;
                btnperok.Visible = false;
                btnperexit.Visible = false;
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
            }


        }
        catch
        {
        }
    }
    protected void ddlpersearch_selectedindex(object sender, EventArgs e)
    {
        if (ddlpersearch.SelectedIndex == 0)
        {
            txtpersearch.Visible = false;
        }
        else
        {
            txtpersearch.Visible = true;
        }
    }

    public void clearselbok1()
    {
        divperspread.Visible = false;
        txtpersearch.Visible = false;

    }
    #endregion

}