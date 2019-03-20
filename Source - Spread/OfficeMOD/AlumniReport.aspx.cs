using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using Gios.Pdf;
using System.IO;
using System.Configuration;

public partial class AlumniReport : System.Web.UI.Page
{

    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    List<string> collist = new List<string>();
    ListItem li = new ListItem();
    ArrayList ItemList = new ArrayList();
    ArrayList Itemindex = new ArrayList();

    static string name = "";
    Dictionary<string, string> DictC = new Dictionary<string, string>();
    Dictionary<string, string> ColText = new Dictionary<string, string>();
    int i = 0;
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("OffM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
        //            Response.Redirect(redrURI, false);
        //            return;
        //        }
        //    }
        //}
        //if (Session["collegecode"] == null)
        //{
        //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
        //    Response.Redirect(redrURI, false);
        //    return;
        //}
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("OfficeHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            loadcollege();
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            // bindBtch();
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Attributes.Add("readonly", "readonly");
            loadColOrder();
            loaddicti();
            loadType();
            ViewState["dict"] = null;
            ViewState["ItemList"] = null;
            ViewState["Itemindex"] = null;
        }
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    #region columnorder Values

    protected void loadColOrder()
    {
        try
        {
            cblcolorder.Items.Clear();
            cblcolorder.Items.Add(new ListItem("Student Name", "studName"));
            cblcolorder.Items.Add(new ListItem("Reference No", "Bank_Id"));
            cblcolorder.Items.Add(new ListItem("YearofStudy From", "Batch_year"));
            cblcolorder.Items.Add(new ListItem("YearofStudy To", "Batch_yearto"));
            cblcolorder.Items.Add(new ListItem("Highest Course To", "edu_level"));
            cblcolorder.Items.Add(new ListItem("Department", "Dept_Code"));
            cblcolorder.Items.Add(new ListItem("Gender", "Gender"));
            cblcolorder.Items.Add(new ListItem("DateOfapplied", "dateofapplied"));
            cblcolorder.Items.Add(new ListItem("PaymentDate", "PaymentDate"));
            cblcolorder.Items.Add(new ListItem("Amount", "totalamt"));
            cblcolorder.Items.Add(new ListItem("Occupation", "CurrentOccup"));
            cblcolorder.Items.Add(new ListItem("Organization", "Organization"));
            cblcolorder.Items.Add(new ListItem("MobileNo", "MobileNo"));
            cblcolorder.Items.Add(new ListItem("Email", "Email"));
            cblcolorder.Items.Add(new ListItem("Address", "address"));
            cblcolorder.Items.Add(new ListItem("State", "State"));
            cblcolorder.Items.Add(new ListItem("Country", "Country"));
            cblcolorder.Items.Add(new ListItem("Pincode", "Pincode"));
            cblcolorder.Items.Add(new ListItem("Resident When you Studied in MCC", "IsResident"));
            cblcolorder.Items.Add(new ListItem("Hall Name When Studied at MCC", "Hoste_Code"));
            cblcolorder.Items.Add(new ListItem("Like to Stay on Hall", "IsStayOnCampus"));
            cblcolorder.Items.Add(new ListItem("Hall Name", "stayhostelcode"));
            cblcolorder.Items.Add(new ListItem(" Accompanied by Spouse", "accompainedBy"));
            cblcolorder.Items.Add(new ListItem(" Male Children", "AccompainedMale"));
            cblcolorder.Items.Add(new ListItem(" FeMale Children", "AccompainedFemale"));
            cblcolorder.Items.Add(new ListItem("Food", "isveg"));
            //cblcolorder.Items.Add(new ListItem("Hall Required", "IsStayOnCampus"));
            //cblcolorder.Items.Add(new ListItem("Hall Name", "StayHostelCode"));
            for (int sel = 0; sel < cblcolorder.Items.Count; sel++)
            {
                ColText.Add(Convert.ToString(cblcolorder.Items[sel].Value), Convert.ToString(cblcolorder.Items[sel].Text));
            }


        }
        catch { }
    }
    protected void loaddicti()
    {
        try
        {
            ColText.Clear();
            ColText.Add("studName", "Student Name");
            ColText.Add("Bank_Id", "Reference No");
            ColText.Add("Batch_year", "YearofStudy From");
            ColText.Add("Batch_yearto", "YearofStudy To");
            ColText.Add("edu_level", "Highest Course To");
            ColText.Add("Dept_Code", "Dept Name");
            ColText.Add("Gender", "Gender");
            ColText.Add("dateofapplied", "DateOfapplied");
            ColText.Add("PaymentDate", "PaymentDate");
            ColText.Add("totalamt", "Amount");
            ColText.Add("CurrentOccup", "Occupation");
            ColText.Add("Organization", "Organization");
            ColText.Add("MobileNo", "MobileNo");
            ColText.Add("Email", "Email");
            ColText.Add("address", "Address");
            ColText.Add("State", "State");
            ColText.Add("Country", "Country");
            ColText.Add("Pincode", "Pincode");
            ColText.Add("IsResident", "Resident When you Studied in MCC");
            ColText.Add("stayhostelcode", "Hall Name");
            ColText.Add("Hoste_Code", "Hall Name When Studied at MCC");
            ColText.Add("IsStayOnCampus", "Like to Stay on Hall");
            ColText.Add("accompainedBy", "Accompanied by Spouse");
            ColText.Add("AccompainedMale", "Male Children");
            ColText.Add("AccompainedFemale", "FeMale Children");
            ColText.Add("isveg", "Food");

        }
        catch { }
    }

    protected void cbcolorder_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string colname12 = "";
            if (ViewState["dict"] != null)
            {
                DictC = (Dictionary<string, string>)ViewState["dict"];
            }
            if (ViewState["dict"] != null)
            {
                ItemList = (ArrayList)ViewState["ItemList"];
            }
            if (ViewState["dict"] != null)
            {
                Itemindex = (ArrayList)ViewState["Itemindex"];
            }
            // cblcolorder.Items.Clear();
            if (cbcolorder.Checked == true)
            {
                int count = DictC.Count;
                for (int sel = 0; sel < cblcolorder.Items.Count; sel++)
                {
                    cblcolorder.Items[sel].Selected = true;
                    //li = new ListItem(Convert.ToString(cblcolorder.Items[sel].Text), Convert.ToString(sel));
                    //collist.Add(Convert.ToString(li));
                    if (!DictC.ContainsValue(Convert.ToString(cblcolorder.Items[sel].Value)))
                    {
                        DictC.Add(Convert.ToString(count), Convert.ToString(cblcolorder.Items[sel].Value));
                        count++;
                    }
                    if (!Itemindex.Contains(sel))
                    {
                        ItemList.Add(cblcolorder.Items[sel].Value.ToString());
                        Itemindex.Add(sel);
                    }
                }
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
            }
            else
            {
                for (int sel = 0; sel < cblcolorder.Items.Count; sel++)
                {
                    cblcolorder.Items[sel].Selected = false;
                    //  collist.Clear();
                    DictC.Clear();
                    name = "";
                    ItemList.Remove(cblcolorder.Items[sel].Value.ToString());
                    Itemindex.Remove(sel);
                }
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
            }
            tborder.Text = colname12;
            ViewState["dict"] = DictC;
            ViewState["ItemList"] = ItemList;
            ViewState["Itemindex"] = Itemindex;

        }
        catch { }
    }

    protected void cblcolorder_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            if (ViewState["dict"] != null)
            {
                DictC = (Dictionary<string, string>)ViewState["dict"];
            }
            if (ViewState["dict"] != null)
            {
                ItemList = (ArrayList)ViewState["ItemList"];
            }
            if (ViewState["dict"] != null)
            {
                Itemindex = (ArrayList)ViewState["Itemindex"];
            }



            string colname12 = "";
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            int index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            int count = DictC.Count;
            for (int sel = 0; sel < cblcolorder.Items.Count; sel++)
            {
                count = sel;
                if (cblcolorder.Items[sel].Selected == true)
                {
                    if (!DictC.ContainsValue(Convert.ToString(cblcolorder.Items[sel].Value)))
                    {
                        DictC.Add(Convert.ToString(count), Convert.ToString(cblcolorder.Items[sel].Value));

                    }
                }
                else
                {
                    DictC.Remove(Convert.ToString(sel));
                }
                if (cblcolorder.Items[index].Selected)
                {
                    if (!Itemindex.Contains(sindex))
                    {
                        ItemList.Add(cblcolorder.Items[index].Value.ToString());
                        Itemindex.Add(sindex);
                    }
                }
                else
                {
                    ItemList.Remove(cblcolorder.Items[index].Value.ToString());
                    Itemindex.Remove(sindex);
                }

            }
            for (int i = 0; i < cblcolorder.Items.Count; i++)
            {
                if (cblcolorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);

                }
            }

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
            ViewState["dict"] = DictC;
            ViewState["ItemList"] = ItemList;
            ViewState["Itemindex"] = Itemindex;


        }
        catch { }
    }

    #endregion

    #region college
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        { }
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddl_collegename.SelectedItem.Value.ToString();
            // bindBtch();
        }
        catch
        {
        }
    }

    #endregion

    #region batch
    //public void bindBtch()
    //{
    //    try
    //    {

    //        cbl_batch.Items.Clear();
    //        cb_batch.Checked = false;
    //        txt_batch.Text = "---Select---";
    //        ds.Clear();
    //        ds = d2.BindBatch();
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_batch.DataSource = ds;
    //            cbl_batch.DataTextField = "batch_year";
    //            cbl_batch.DataValueField = "batch_year";
    //            cbl_batch.DataBind();
    //            if (cbl_batch.Items.Count > 0)
    //            {
    //                for (i = 0; i < cbl_batch.Items.Count; i++)
    //                {
    //                    cbl_batch.Items[i].Selected = true;
    //                }
    //                txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
    //                cb_batch.Checked = true;
    //            }
    //        }
    //    }
    //    catch { }
    //}
    //protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
    //    }
    //    catch { }
    //}
    //protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
    //    }
    //    catch { }
    //}
    #endregion

    #region type

    protected void loadType()
    {
        try
        {
            cbltype.Items.Clear();
            cbltype.Items.Add(new ListItem("Online", "1"));
            cbltype.Items.Add(new ListItem("Offline", "0"));
            for (int i = 0; i < cbltype.Items.Count; i++)
            {
                cbltype.Items[i].Selected = true;
            }
            cbtype.Checked = true;
            txttype.Text = "Type(" + cbltype.Items.Count + ")";
        }
        catch { }
    }

    protected void cbtype_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbtype, cbltype, txttype, "Type", "--Select--");
        }
        catch { }
    }
    protected void cbltype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbtype, cbltype, txttype, "Type", "--Select--");
        }
        catch { }
    }
    #endregion


    #region Button Go

    protected DataSet loadDataset()
    {
        DataSet dsload = new DataSet();
        try
        {
            //string batch1 = Convert.ToString(getCblSelectedValue(cbl_batch));
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string type = Convert.ToString(getCblSelectedValue(cbltype));
            string fromdate = Convert.ToString(txt_fromdate.Text);
            string todate = Convert.ToString(txt_todate.Text);
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            }
            String SelQ = "";
            SelQ = " select AlumniStudDet,studName,(address+'-'+city) as address,State,Country,Pincode,Organization,CurrentOccup,Email,MobileNo,Dept_Code,Batch_year,Batch_yearto,edu_level,Course_Id,Gender,CONVERT(varchar(10),dateofapplied,103) as dateofapplied,Bank_Id,CONVERT(varchar(10),PaymentDate,103) as PaymentDate,totalamt,totalamtcount,case when IsResident=1 then 'Resident' when IsResident='0' then 'NonResident' end IsResident,case when IsStayOnCampus=1 then 'Yes' else 'No' end IsStayOnCampus,stayhostelcode ,stayreason,stayhostelcode,case when isveg='0' then 'Non Vegeterian' when isveg='1' then 'Vegeterian' end isveg, case when  AccompainedBy='0' then 'NO' when AccompainedBy='1' then 'YES' end AccompainedBy, AccompainedMale,AccompainedFemale,IsStayOnCampus,StayHostelCode,Hoste_Code from alumnistuddet where Isonline in('" + type + "') and dateofapplied between '" + fromdate + "' and '" + todate + "'";
            SelQ = SelQ + " select COUNT(Bank_Id) as totreg from AlumniStudDet where  dateofapplied between '" + fromdate + "' and '" + todate + "' and College_Code='" + collegecode + "'";


            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = loadDataset();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadGoValues();
        }
        else
        {
            FpSpread1.Visible = false;
            print.Visible = false;
            lblvalidation1.Visible = false;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            lblalert.Text = "No Record Found";
            Errpopup.Visible = true;
        }
    }

    protected void loadGoValues()
    {
        try
        {
            #region Design
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 2;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            cball.AutoPostBack = true;
            cb.AutoPostBack = false;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            loaddicti();
            Dictionary<string, string> dictcol = new Dictionary<string, string>();
            if (ViewState["dict"] != null)
            {
                DictC = (Dictionary<string, string>)ViewState["dict"];
            }
            if (ViewState["dict"] != null)
            {
                ItemList = (ArrayList)ViewState["ItemList"];
            }
            if (ViewState["dict"] != null)
            {
                Itemindex = (ArrayList)ViewState["Itemindex"];
            }
            if (DictC.Count > 0)
            {
                foreach (KeyValuePair<string, string> colname in DictC)
                {
                    FpSpread1.Sheets[0].ColumnCount++;
                    string col = ColText[colname.Value.ToString()].ToString();
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = col;
                    dictcol.Add(Convert.ToString(FpSpread1.Sheets[0].ColumnCount - 1), Convert.ToString(colname.Value.ToString()));
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                }
            #endregion

                #region
                string code = "";
                int height = 0;

                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cball;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                for (int col = 0; col < ds.Tables[0].Rows.Count; col++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    height += 30;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(col + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cb;
                    string alumniname = Convert.ToString(ds.Tables[0].Rows[col]["AlumniStudDet"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = alumniname;

                    foreach (KeyValuePair<String, string> value in dictcol)
                    {
                        string colname = value.Value.ToString();
                        string colcnt = value.Key.ToString();
                        if (colname.Trim() == "stayhostelcode" || colname.Trim() == "Hoste_Code")
                        {
                            code = Convert.ToString(ds.Tables[0].Rows[col][colname]);
                            string hostelname = d2.GetFunction("select distinct Hostel_Name from Hostel_Details where Hostel_code='" + code + "'");
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Convert.ToInt32(colcnt)].Text = hostelname;
                        }
                        else if (colname.Trim() == "State")
                        {
                            code = Convert.ToString(ds.Tables[0].Rows[col][colname]);
                            string state = d2.GetFunction("select MasterValue from CO_MasterValues where MasterCriteria='state' and MasterValue is not null and MasterValue<>''  and MasterCode='" + code + "'");
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Convert.ToInt32(colcnt)].Text = state;
                        }
                        else if (colname.Trim() == "Country")
                        {
                            code = Convert.ToString(ds.Tables[0].Rows[col][colname]);
                            string country = d2.GetFunction("select MasterValue,MasterCode from CO_MasterValues where MasterCriteria='Country' and MasterValue is not null and MasterValue<>'' and MasterCode='" + code + "' ");
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Convert.ToInt32(colcnt)].Text = country;
                        }
                        else if (colname.Trim() == "CurrentOccup")
                        {
                            code = Convert.ToString(ds.Tables[0].Rows[col][colname]);
                            string Occupation = d2.GetFunction("select TextVal  from TextValTable where TextCode =" + code + "");
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Convert.ToInt32(colcnt)].Text = Occupation;
                        }
                        else if (colname.Trim() == "Gender")
                        {
                            code = Convert.ToString(ds.Tables[0].Rows[col][colname]);
                            string Gname = "";
                            if (Convert.ToBoolean(code) == true)
                                Gname = "Male";
                            else
                                Gname = "Female";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Convert.ToInt32(colcnt)].Text = Gname;
                        }
                        else if (colname.Trim() != "stayhostelcode" && colname.Trim() != "State" && colname.Trim() != "Country" && colname.Trim() != "Gender")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Convert.ToInt32(colcnt)].Text = Convert.ToString(ds.Tables[0].Rows[col][colname]);
                        }
                    }
                }
                #region visible

                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                height += 100;
                FpSpread1.Height = height;
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                print.Visible = true;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                FpSpread1.ShowHeaderSelection = false;
                #endregion
            }
            else
            {
                FpSpread1.Visible = false;
                print.Visible = false;
                lblvalidation1.Visible = false;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                lblalert.Text = "Please Select Any One column in the Column Order";
                Errpopup.Visible = true;
            }

                #endregion


        }
        catch { }
    }

    protected void FpSpread1_OnButtonCommand(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            string value = "";
            if (actrow != "")
            {
                int arow = Convert.ToInt32(actrow);
                if (arow == 0)
                {
                    value = Convert.ToString(FpSpread1.Sheets[0].Cells[0, 1].Value);
                    if (value == "1")
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                    }
                    else
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                    }
                }
            }
        }
        catch { }
    }

    #region old

    //protected void btnprint_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FpSpread1.SaveChanges();
    //        string value = "";
    //        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
    //        {
    //            value = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Value);
    //            if (value == "1")
    //            {
    //                string alumnval = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
    //                if (alumnval != "")
    //                {
    //                    AlumniRegForm(alumnval);
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}
    //public void AlumniRegForm(string alumniDet)
    //{
    //    try
    //    {
    //        DataSet dsbindv_new = new DataSet();
    //        DataSet dsbindv = new DataSet();
    //        Font fontColName = new Font("Times New Roman", 17, FontStyle.Bold);
    //        Font fontColaddr = new Font("Times New Roman", 12, FontStyle.Bold);
    //        Font fontColwebsiteemail = new Font("Times New Roman", 10, FontStyle.Regular);
    //        Font fontColwebsiteemailbold = new Font("Times New Roman", 10, FontStyle.Bold);
    //        Font fontAlumniHead = new Font("Times New Roman", 15, FontStyle.Bold);
    //        Font fontReg = new Font("Times New Roman", 16, FontStyle.Bold);
    //        Font fontContent = new Font("Times New Roman", 14, FontStyle.Regular);
    //        Font fontColwebsiteemailsmall = new Font("Times New Roman", 7, FontStyle.Regular);

    //        Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
    //        Gios.Pdf.PdfPage mypdfpage;
    //        string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='13'; select * from AlumniStudDet where AlumniStudDet=" + alumniDet + "";
    //        ds.Dispose();
    //        ds.Reset();
    //        ds = d2.select_method_wo_parameter(strquery, "Text");
    //        string Collegename = "";
    //        string aff = "";
    //        string collacr = "";
    //        string dispin = "";
    //        string clgaddress = "";
    //        string univ = "";
    //        string pincode = "";
    //        string clgwebsite = "";
    //        string clgemail = "";
    //        string clgphno = "";
    //        string clgmobno = "";
    //        bool status = false;
    //        string stuName = "", stuAddr = "", stucity = "", stupincode = "", stuCountry = "", stucur_occupa = "", stuOrganize = "", studept_year = "", stuemail = "", stumob = "", stuaddr = "";
    //        int coltop = 0;
    //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            Collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]) + "(" + Convert.ToString(ds.Tables[0].Rows[0]["category"]) + ")";
    //            string tempaddr = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
    //            dispin = Convert.ToString(ds.Tables[0].Rows[0]["districtpin"]);
    //            if (tempaddr != "")
    //            {
    //                clgaddress = tempaddr.Trim(',');
    //            }
    //            if (clgaddress != "")
    //            {

    //                clgaddress += ", " + dispin;
    //            }
    //            else
    //            {
    //                clgaddress = dispin;
    //            }
    //            coltop = 0;
    //            clgwebsite = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
    //            clgemail = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
    //            clgphno = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
    //            clgmobno = "+91 72990 73125";

    //            if (ds.Tables[1].Rows.Count > 0)
    //            {

    //                stuName = Convert.ToString(ds.Tables[1].Rows[0]["StudName"]);
    //                stuAddr = Convert.ToString(ds.Tables[1].Rows[0]["Address"]);
    //                stucity = Convert.ToString(ds.Tables[1].Rows[0]["City"]);
    //                stupincode = Convert.ToString(ds.Tables[1].Rows[0]["Pincode"]);
    //                stuCountry = d2.GetFunctionv("select MasterValue from CO_MasterValues  where MasterCriteria='Country' and MasterValue is not null and MasterValue<>'' and MasterCode ='" + Convert.ToString(ds.Tables[1].Rows[0]["Country"]) + "'   order by MasterValue");//
    //                stucur_occupa = d2.GetFunctionv("select distinct textval,textcode from textvaltable where TextCriteria='foccu'  and textval is not null and textval<>'' and textcode='" + Convert.ToString(ds.Tables[1].Rows[0]["CurrentOccup"]) + "'");
    //                stuOrganize = Convert.ToString(ds.Tables[1].Rows[0]["Organization"]);
    //                stuemail = Convert.ToString(ds.Tables[1].Rows[0]["Email"]);
    //                stumob = Convert.ToString(ds.Tables[1].Rows[0]["MobileNo"]);
    //                // studept_year = da.GetFunctionv("select Dept_Name from Department where Dept_Code='" + + "'") + "   " + Convert.ToString(ds.Tables[1].Rows[0]["Batch_year"]);
    //                studept_year = Convert.ToString(ds.Tables[1].Rows[0]["Dept_code"]);
    //                studept_year = studept_year + "-" + Convert.ToString(ds.Tables[1].Rows[0]["Batch_yearto"]);
    //                // select Dept_Name from Department where Dept_Code=
    //                stuAddr = stuAddr.Replace("~", ",");
    //                string Stu_Addr = "";
    //                String[] tempstuAddr = stuAddr.Split(',');
    //                if (tempstuAddr.Length > 0)
    //                {
    //                    for (int s = 0; s < tempstuAddr.Length; s++)
    //                    {
    //                        if (tempaddr[s].ToString() != "")
    //                        {
    //                            if (Stu_Addr == "")
    //                            {
    //                                Stu_Addr = tempaddr[s].ToString();
    //                            }
    //                            else
    //                            {
    //                                Stu_Addr += " , " + tempaddr[s].ToString();
    //                            }
    //                        }
    //                    }
    //                }

    //                status = true;
    //                mypdfpage = mydoc.NewPage();

    //                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
    //                {
    //                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
    //                    mypdfpage.Add(LogoImage, 25, 25, 350);
    //                }
    //                PdfRectangle pr1 = new PdfRectangle(mydoc, new PdfArea(mydoc, 15, 25, 565, 800), Color.Black);
    //                mypdfpage.Add(pr1);
    //                coltop = 30;
    //                PdfTextArea pdfHeader = new PdfTextArea(fontColName, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 50), ContentAlignment.MiddleCenter, Collegename);
    //                mypdfpage.Add(pdfHeader);

    //                coltop += 20;
    //                pdfHeader = new PdfTextArea(fontColaddr, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, clgaddress);
    //                mypdfpage.Add(pdfHeader);

    //                coltop += 10;
    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 30), ContentAlignment.MiddleCenter, "Website : http:\\" + clgwebsite + " / Email Id : " + clgemail);
    //                mypdfpage.Add(pdfHeader);

    //                coltop += 15;
    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, "Phone No. : " + clgmobno + " / " + clgphno);
    //                mypdfpage.Add(pdfHeader);

    //                coltop += 15;
    //                pdfHeader = new PdfTextArea(fontAlumniHead, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, "MCC Alumni Association - Celebrating 125 Years ");
    //                mypdfpage.Add(pdfHeader);

    //                coltop += 20;
    //                pdfHeader = new PdfTextArea(fontAlumniHead, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, "Global Alumni Reunion  2016");
    //                mypdfpage.Add(pdfHeader);

    //                coltop += 20;
    //                pdfHeader = new PdfTextArea(fontAlumniHead, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, "REGISTRATION FORM");
    //                mypdfpage.Add(pdfHeader);

    //                //coltop += 15;
    //                //pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, "(Please use capital letters & tick the appropriate box)");
    //                //mypdfpage.Add(pdfHeader);

    //                coltop += 35;

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 30), ContentAlignment.MiddleLeft, "Name  :  " + stuName.ToUpper());
    //                mypdfpage.Add(pdfHeader);

    //                //fontContent
    //                stuaddr = "Address  : " + stuAddr;
    //                coltop += 30;
    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 40), ContentAlignment.MiddleLeft, stuaddr.ToUpper());
    //                mypdfpage.Add(pdfHeader);

    //                coltop += 30;
    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 40), ContentAlignment.MiddleLeft, "City/Town  : " + stucity.ToUpper() + "                                 Pin Code : " + stupincode + "                                       Country : " + stuCountry.ToUpper());
    //                mypdfpage.Add(pdfHeader);

    //                coltop += 30;
    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Current Occupation  : " + stucur_occupa.ToUpper());
    //                mypdfpage.Add(pdfHeader);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 300, coltop, 595, 50), ContentAlignment.MiddleLeft, "  Organization : " + stuOrganize.ToUpper());
    //                mypdfpage.Add(pdfHeader);

    //                coltop += 30;
    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Email  : " + stuemail);
    //                mypdfpage.Add(pdfHeader);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 300, coltop, 595, 50), ContentAlignment.MiddleLeft, " Mobile No. : " + stumob);
    //                mypdfpage.Add(pdfHeader);

    //                coltop += 30;
    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Department & Year Of Study  : " + studept_year);
    //                mypdfpage.Add(pdfHeader);

    //                string IsResident = Convert.ToString(ds.Tables[1].Rows[0]["IsResident"]);

    //                if (IsResident == "True" || IsResident == "1")
    //                {
    //                    string hostelcode = Convert.ToString(ds.Tables[1].Rows[0]["Hoste_Code"]);
    //                    hostelcode = d2.GetFunctionv("select distinct Hostel_Name from Hostel_Details where Hostel_code='" + hostelcode + "' ");
    //                    coltop += 30;
    //                    pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "If Resident Hall  : " + hostelcode);
    //                    mypdfpage.Add(pdfHeader);


    //                }
    //                string IsStayOnCampus = Convert.ToString(ds.Tables[1].Rows[0]["IsStayOnCampus"]);
    //                if (IsStayOnCampus == "True" || IsStayOnCampus == "1")
    //                {
    //                    string hostelcode = Convert.ToString(ds.Tables[1].Rows[0]["stayreason"]);
    //                    hostelcode = d2.GetFunctionv("select distinct Hostel_Name from Hostel_Details where Hostel_code='" + hostelcode + "' ");
    //                    coltop += 30;
    //                    pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Would like to stay on Campus (Aug 6th night) : Yes   If Yes, prefferred Hall for Stay  " + hostelcode);
    //                    mypdfpage.Add(pdfHeader);
    //                    coltop += 10;
    //                    pdfHeader = new PdfTextArea(fontColwebsiteemailsmall, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "(Campus accommodation is limited and accommodation will be subject to availability)");
    //                    mypdfpage.Add(pdfHeader);


    //                }

    //                string AccompainedBy = Convert.ToString(ds.Tables[1].Rows[0]["AccompainedBy"]);
    //                if (AccompainedBy == "True" || AccompainedBy == "1")
    //                {

    //                    coltop += 30;
    //                    pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Will be accompanied by  Spouse | Children  ");
    //                    mypdfpage.Add(pdfHeader);



    //                }

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 300, coltop, 595, 50), ContentAlignment.MiddleLeft, "Male No. : " + Convert.ToString(ds.Tables[1].Rows[0]["AccompainedMale"]) + "  ");
    //                mypdfpage.Add(pdfHeader);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 400, coltop, 595, 50), ContentAlignment.MiddleLeft, "Female No. : " + Convert.ToString(ds.Tables[1].Rows[0]["AccompainedFemale"]) + "  ");
    //                mypdfpage.Add(pdfHeader);

    //                //string query = "select * from AlumniLunchDet where AlumniStudDet=" + AlumniStudDet + " and JoinDate=" + JoinDate + " ";

    //                DateTime dtf = new DateTime();
    //                dsbindv_new.Clear();
    //                dsbindv_new = d2.select_method_wo_parameter("select * from AlumniLunchSettings where  AlumniType='0' order by AlumniDate", "Text");
    //                if (dsbindv_new.Tables[0].Rows.Count > 0)
    //                {

    //                    for (int ii = 0; ii < dsbindv_new.Tables[0].Rows.Count; ii++)
    //                    {



    //                        dtf = Convert.ToDateTime(dsbindv_new.Tables[0].Rows[ii]["AlumniDate"].ToString());
    //                        string AlumniDate = "'" + dtf.Date.ToString("MM/dd/yyy") + "'";

    //                        string query = "select * from AlumniLunchDet where AlumniStudDet=" + alumniDet + " and JoinDate=" + AlumniDate + " ";

    //                        dsbindv.Clear();
    //                        dsbindv = d2.select_method_wo_parameter(query, "Text");
    //                        if (dsbindv.Tables[0].Rows.Count > 0)
    //                        {


    //                            coltop += 30;
    //                            pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Will be Joining for " + dtf.Date.ToString("dd/MM/yyy") + "  ");
    //                            mypdfpage.Add(pdfHeader);

    //                            string AlumnuLunch = dsbindv.Tables[0].Rows[0]["joinsession"].ToString();
    //                            string[] splitAlumnuLunch = AlumnuLunch.Split(';');
    //                            AlumnuLunch = "";
    //                            for (int i = 0; i < splitAlumnuLunch.Length; i++)
    //                            {
    //                                if (AlumnuLunch.Trim() == "")
    //                                {
    //                                    AlumnuLunch = splitAlumnuLunch[i];
    //                                }
    //                                else
    //                                {
    //                                    AlumnuLunch = AlumnuLunch + "','" + splitAlumnuLunch[i];
    //                                }

    //                            }

    //                            query = "select  MasterValue,Mastercode from CO_MasterValues where MasterCode in ('" + AlumnuLunch + "')";
    //                            dsbindv.Clear();
    //                            dsbindv = d2.select_method_wo_parameter(query, "Text");


    //                            int left = 180;
    //                            for (int i = 0; i < dsbindv.Tables[0].Rows.Count; i++)
    //                            {
    //                                string line = "|";
    //                                if (i == dsbindv.Tables[0].Rows.Count - 1)
    //                                {
    //                                    line = "";
    //                                }
    //                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, left, coltop, 595, 50), ContentAlignment.MiddleLeft, "" + dsbindv.Tables[0].Rows[i]["MasterValue"].ToString() + "         " + line + " ");
    //                                mypdfpage.Add(pdfHeader);


    //                                left = left + 80;

    //                            }



    //                        }

    //                    }



    //                }


    //                string IsVeg = Convert.ToString(ds.Tables[1].Rows[0]["IsVeg"]);
    //                if (IsVeg == "True" || IsVeg == "1")
    //                {

    //                    coltop += 30;
    //                    pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Vegiterian");
    //                    mypdfpage.Add(pdfHeader);



    //                }
    //                else
    //                {
    //                    coltop += 30;
    //                    pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Non Vegiterian");
    //                    mypdfpage.Add(pdfHeader);
    //                }


    //                int minus = 50;
    //                pdfHeader = new PdfTextArea(fontColwebsiteemailbold, Color.Black, new PdfArea(mydoc, 30, 600 - minus, 595, 30), ContentAlignment.MiddleLeft, "REGISTRATION FEE : Rs.2500 Per Person (exclude accomodation)");
    //                mypdfpage.Add(pdfHeader);
    //                pdfHeader = new PdfTextArea(fontColwebsiteemailbold, Color.Black, new PdfArea(mydoc, 30, 620 - minus, 595, 30), ContentAlignment.MiddleLeft, "Early Bird Registration");
    //                mypdfpage.Add(pdfHeader);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 640 - minus, 595, 30), ContentAlignment.MiddleLeft, "Rs. 2000 if Registered & Payment received on or Before 31st May 2016");
    //                mypdfpage.Add(pdfHeader);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 660 - minus, 595, 30), ContentAlignment.MiddleLeft, "Rs. 2250 if Registered & Payment received between 1st June to 15 July 2016");
    //                mypdfpage.Add(pdfHeader);

    //                //pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 50, 680, 595, 30), ContentAlignment.MiddleLeft, "For Payment instructions,Please See Annexure A / For Accomodation , Please See Annexure B");
    //                //mypdfpage.Add(pdfHeader);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemailbold, Color.Black, new PdfArea(mydoc, 30, 695 - minus, 595, 30), ContentAlignment.MiddleLeft, "Details Of Payment Toward Registration : Online Payment");
    //                mypdfpage.Add(pdfHeader);


    //                //pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 735, 595, 25), ContentAlignment.MiddleLeft, "Details Of Payment Toward Registration :");
    //                //mypdfpage.Add(pdfHeader);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 750, 200, 25), ContentAlignment.MiddleLeft, "Date :" + DateTime.Now.ToString("dd/MM/yyyy"));
    //                mypdfpage.Add(pdfHeader);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 350, 750, 200, 25), ContentAlignment.MiddleRight, "Signature :----------------------------");
    //                mypdfpage.Add(pdfHeader);

    //                PdfLine pdflin = new PdfLine(mydoc, new Point(30, 767), new Point(565, 767), Color.Black, 1);
    //                mypdfpage.Add(pdflin);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemailbold, Color.Black, new PdfArea(mydoc, 30, 765, 595, 25), ContentAlignment.MiddleCenter, "For Alumni Office Use Only");
    //                mypdfpage.Add(pdfHeader);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 785, 595, 20), ContentAlignment.MiddleLeft, "Received With Payment on :----------------------------------------");
    //                mypdfpage.Add(pdfHeader);

    //                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 800, 595, 20), ContentAlignment.MiddleLeft, "VIDE -- Online Payment/Credit/Debit Card/Cash");
    //                mypdfpage.Add(pdfHeader);
    //                //if (status == true)
    //                //{
    //                //    mypdfpage.SaveToDocument();
    //                //}
    //            }


    //        }

    //        if (status == true)
    //        {
    //            string appPath = HttpContext.Current.Server.MapPath("~");
    //            if (appPath != "")
    //            {
    //                string szPath = appPath + "/Report/";
    //                string szFile = "" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
    //                mydoc.SaveToFile(szPath + szFile);
    //                //Response.ClearHeaders();
    //                //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
    //                //Response.ContentType = "application/pdf";
    //                //Response.WriteFile(szPath + szFile);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    #endregion


    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            bool print = false;
            FpSpread1.SaveChanges();
            string value = "";
            bool status = false;
            DataSet dsbindv_new = new DataSet();
            DataSet dsbindv = new DataSet();
            Font fontColName = new Font("Times New Roman", 17, FontStyle.Bold);
            Font fontColaddr = new Font("Times New Roman", 12, FontStyle.Bold);
            Font fontColwebsiteemail = new Font("Times New Roman", 10, FontStyle.Regular);
            Font fontColwebsiteemailbold = new Font("Times New Roman", 10, FontStyle.Bold);
            Font fontAlumniHead = new Font("Times New Roman", 15, FontStyle.Bold);
            Font fontReg = new Font("Times New Roman", 16, FontStyle.Bold);
            Font fontContent = new Font("Times New Roman", 14, FontStyle.Regular);
            Font fontColwebsiteemailsmall = new Font("Times New Roman", 7, FontStyle.Regular);

            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            for (int sel = 1; sel < FpSpread1.Sheets[0].Rows.Count; sel++)
            {
                value = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 1].Value);
                if (value == "1")
                {
                    string alumniDet = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 1].Tag);
                    if (alumniDet != "")
                    {
                        print = true;
                        string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='13'; select * from AlumniStudDet where AlumniStudDet=" + alumniDet + "";
                        ds.Dispose();
                        ds.Reset();
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        string Collegename = "";
                        string aff = "";
                        string collacr = "";
                        string dispin = "";
                        string clgaddress = "";
                        string univ = "";
                        string pincode = "";
                        string clgwebsite = "";
                        string clgemail = "";
                        string clgphno = "";
                        string clgmobno = "";
                        string stuName = "", stuAddr = "", stucity = "", stupincode = "", stuCountry = "", stucur_occupa = "", stuOrganize = "", studept_year = "", stuemail = "", stumob = "", stuaddr = "";
                        int coltop = 0;
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            Collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]) + "(" + Convert.ToString(ds.Tables[0].Rows[0]["category"]) + ")";
                            string tempaddr = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                            dispin = Convert.ToString(ds.Tables[0].Rows[0]["districtpin"]);
                            if (tempaddr != "")
                            {
                                clgaddress = tempaddr.Trim(',');
                            }
                            if (clgaddress != "")
                            {

                                clgaddress += ", " + dispin;
                            }
                            else
                            {
                                clgaddress = dispin;
                            }
                            coltop = 0;
                            clgwebsite = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
                            clgemail = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                            clgphno = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                            clgmobno = "+91 72990 73125";

                            if (ds.Tables[1].Rows.Count > 0)
                            {

                                stuName = Convert.ToString(ds.Tables[1].Rows[0]["StudName"]);
                                stuAddr = Convert.ToString(ds.Tables[1].Rows[0]["Address"]);
                                stucity = Convert.ToString(ds.Tables[1].Rows[0]["City"]);
                                stupincode = Convert.ToString(ds.Tables[1].Rows[0]["Pincode"]);
                                stuCountry = d2.GetFunctionv("select MasterValue from CO_MasterValues  where MasterCriteria='Country' and MasterValue is not null and MasterValue<>'' and MasterCode ='" + Convert.ToString(ds.Tables[1].Rows[0]["Country"]) + "'   order by MasterValue");//
                                stucur_occupa = d2.GetFunctionv("select distinct textval,textcode from textvaltable where TextCriteria='foccu'  and textval is not null and textval<>'' and textcode='" + Convert.ToString(ds.Tables[1].Rows[0]["CurrentOccup"]) + "'");
                                stuOrganize = Convert.ToString(ds.Tables[1].Rows[0]["Organization"]);
                                stuemail = Convert.ToString(ds.Tables[1].Rows[0]["Email"]);
                                stumob = Convert.ToString(ds.Tables[1].Rows[0]["MobileNo"]);
                                // studept_year = da.GetFunctionv("select Dept_Name from Department where Dept_Code='" + + "'") + "   " + Convert.ToString(ds.Tables[1].Rows[0]["Batch_year"]);
                                studept_year = Convert.ToString(ds.Tables[1].Rows[0]["Dept_code"]);
                                studept_year = studept_year + "-" + Convert.ToString(ds.Tables[1].Rows[0]["Batch_yearto"]);
                                // select Dept_Name from Department where Dept_Code=
                                stuAddr = stuAddr.Replace("~", ",");
                                string Stu_Addr = "";
                                String[] tempstuAddr = stuAddr.Split(',');
                                if (tempstuAddr.Length > 0)
                                {
                                    for (int s = 0; s < tempstuAddr.Length; s++)
                                    {
                                        if (tempaddr[s].ToString() != "")
                                        {
                                            if (Stu_Addr == "")
                                            {
                                                Stu_Addr = tempaddr[s].ToString();
                                            }
                                            else
                                            {
                                                Stu_Addr += " , " + tempaddr[s].ToString();
                                            }
                                        }
                                    }
                                }

                                status = true;
                                mypdfpage = mydoc.NewPage();

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 25, 25, 350);
                                }
                                PdfRectangle pr1 = new PdfRectangle(mydoc, new PdfArea(mydoc, 15, 25, 565, 800), Color.Black);
                                mypdfpage.Add(pr1);
                                coltop = 30;
                                PdfTextArea pdfHeader = new PdfTextArea(fontColName, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 50), ContentAlignment.MiddleCenter, Collegename);
                                mypdfpage.Add(pdfHeader);

                                coltop += 20;
                                pdfHeader = new PdfTextArea(fontColaddr, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, clgaddress);
                                mypdfpage.Add(pdfHeader);

                                coltop += 10;
                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 30), ContentAlignment.MiddleCenter, "Website : http:\\" + clgwebsite + " / Email Id : " + clgemail);
                                mypdfpage.Add(pdfHeader);

                                coltop += 15;
                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, "Phone No. : " + clgmobno + " / " + clgphno);
                                mypdfpage.Add(pdfHeader);

                                coltop += 15;
                                pdfHeader = new PdfTextArea(fontAlumniHead, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, "MCC Alumni Association - Celebrating 125 Years ");
                                mypdfpage.Add(pdfHeader);

                                coltop += 20;
                                pdfHeader = new PdfTextArea(fontAlumniHead, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, "Global Alumni Reunion  2016");
                                mypdfpage.Add(pdfHeader);

                                coltop += 20;
                                pdfHeader = new PdfTextArea(fontAlumniHead, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, "REGISTRATION FORM");
                                mypdfpage.Add(pdfHeader);

                                //coltop += 15;
                                //pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 5, coltop, 595, 35), ContentAlignment.MiddleCenter, "(Please use capital letters & tick the appropriate box)");
                                //mypdfpage.Add(pdfHeader);

                                coltop += 35;

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 30), ContentAlignment.MiddleLeft, "Name  :  " + stuName.ToUpper());
                                mypdfpage.Add(pdfHeader);

                                //fontContent
                                stuaddr = "Address  : " + stuAddr;
                                coltop += 30;
                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 40), ContentAlignment.MiddleLeft, stuaddr.ToUpper());
                                mypdfpage.Add(pdfHeader);

                                coltop += 30;
                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 40), ContentAlignment.MiddleLeft, "City/Town  : " + stucity.ToUpper() + "                                 Pin Code : " + stupincode + "                                       Country : " + stuCountry.ToUpper());
                                mypdfpage.Add(pdfHeader);

                                coltop += 30;
                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Current Occupation  : " + stucur_occupa.ToUpper());
                                mypdfpage.Add(pdfHeader);

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 300, coltop, 595, 50), ContentAlignment.MiddleLeft, "  Organization : " + stuOrganize.ToUpper());
                                mypdfpage.Add(pdfHeader);

                                coltop += 30;
                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Email  : " + stuemail);
                                mypdfpage.Add(pdfHeader);

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 300, coltop, 595, 50), ContentAlignment.MiddleLeft, " Mobile No. : " + stumob);
                                mypdfpage.Add(pdfHeader);

                                coltop += 30;
                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Department & Year Of Study  : " + studept_year);
                                mypdfpage.Add(pdfHeader);

                                string IsResident = Convert.ToString(ds.Tables[1].Rows[0]["IsResident"]);

                                if (IsResident == "True" || IsResident == "1")
                                {
                                    string hostelcode = Convert.ToString(ds.Tables[1].Rows[0]["Hoste_Code"]);
                                    hostelcode = d2.GetFunctionv("select distinct Hostel_Name from Hostel_Details where Hostel_code='" + hostelcode + "' ");
                                    coltop += 30;
                                    pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "If Resident Hall  : " + hostelcode);
                                    mypdfpage.Add(pdfHeader);


                                }
                                string IsStayOnCampus = Convert.ToString(ds.Tables[1].Rows[0]["IsStayOnCampus"]);
                                if (IsStayOnCampus == "True" || IsStayOnCampus == "1")
                                {
                                    string hostelcode = Convert.ToString(ds.Tables[1].Rows[0]["stayreason"]);
                                    hostelcode = d2.GetFunctionv("select distinct Hostel_Name from Hostel_Details where Hostel_code='" + hostelcode + "' ");
                                    coltop += 30;
                                    pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Would like to stay on Campus (Aug 6th night) : Yes   If Yes, prefferred Hall for Stay  " + hostelcode);
                                    mypdfpage.Add(pdfHeader);
                                    coltop += 10;
                                    pdfHeader = new PdfTextArea(fontColwebsiteemailsmall, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "(Campus accommodation is limited and accommodation will be subject to availability)");
                                    mypdfpage.Add(pdfHeader);


                                }

                                string AccompainedBy = Convert.ToString(ds.Tables[1].Rows[0]["AccompainedBy"]);
                                if (AccompainedBy == "True" || AccompainedBy == "1")
                                {

                                    coltop += 30;
                                    pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Will be accompanied by  Spouse | Children  ");
                                    mypdfpage.Add(pdfHeader);



                                }

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 300, coltop, 595, 50), ContentAlignment.MiddleLeft, "Male No. : " + Convert.ToString(ds.Tables[1].Rows[0]["AccompainedMale"]) + "  ");
                                mypdfpage.Add(pdfHeader);

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 400, coltop, 595, 50), ContentAlignment.MiddleLeft, "Female No. : " + Convert.ToString(ds.Tables[1].Rows[0]["AccompainedFemale"]) + "  ");
                                mypdfpage.Add(pdfHeader);

                                //string query = "select * from AlumniLunchDet where AlumniStudDet=" + AlumniStudDet + " and JoinDate=" + JoinDate + " ";

                                DateTime dtf = new DateTime();
                                dsbindv_new.Clear();
                                dsbindv_new = d2.select_method_wo_parameter("select * from AlumniLunchSettings where  AlumniType='0' order by AlumniDate", "Text");
                                if (dsbindv_new.Tables[0].Rows.Count > 0)
                                {

                                    for (int ii = 0; ii < dsbindv_new.Tables[0].Rows.Count; ii++)
                                    {



                                        dtf = Convert.ToDateTime(dsbindv_new.Tables[0].Rows[ii]["AlumniDate"].ToString());
                                        string AlumniDate = "'" + dtf.Date.ToString("MM/dd/yyy") + "'";

                                        string query = "select * from AlumniLunchDet where AlumniStudDet=" + alumniDet + " and JoinDate=" + AlumniDate + " ";

                                        dsbindv.Clear();
                                        dsbindv = d2.select_method_wo_parameter(query, "Text");
                                        if (dsbindv.Tables[0].Rows.Count > 0)
                                        {


                                            coltop += 30;
                                            pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Will be Joining for " + dtf.Date.ToString("dd/MM/yyy") + "  ");
                                            mypdfpage.Add(pdfHeader);

                                            string AlumnuLunch = dsbindv.Tables[0].Rows[0]["joinsession"].ToString();
                                            string[] splitAlumnuLunch = AlumnuLunch.Split(';');
                                            AlumnuLunch = "";
                                            for (int i = 0; i < splitAlumnuLunch.Length; i++)
                                            {
                                                if (AlumnuLunch.Trim() == "")
                                                {
                                                    AlumnuLunch = splitAlumnuLunch[i];
                                                }
                                                else
                                                {
                                                    AlumnuLunch = AlumnuLunch + "','" + splitAlumnuLunch[i];
                                                }

                                            }

                                            query = "select  MasterValue,Mastercode from CO_MasterValues where MasterCode in ('" + AlumnuLunch + "')";
                                            dsbindv.Clear();
                                            dsbindv = d2.select_method_wo_parameter(query, "Text");


                                            int left = 180;
                                            for (int i = 0; i < dsbindv.Tables[0].Rows.Count; i++)
                                            {
                                                string line = "|";
                                                if (i == dsbindv.Tables[0].Rows.Count - 1)
                                                {
                                                    line = "";
                                                }
                                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, left, coltop, 595, 50), ContentAlignment.MiddleLeft, "" + dsbindv.Tables[0].Rows[i]["MasterValue"].ToString() + "         " + line + " ");
                                                mypdfpage.Add(pdfHeader);


                                                left = left + 80;

                                            }



                                        }

                                    }



                                }


                                string IsVeg = Convert.ToString(ds.Tables[1].Rows[0]["IsVeg"]);
                                if (IsVeg == "True" || IsVeg == "1")
                                {

                                    coltop += 30;
                                    pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Vegiterian");
                                    mypdfpage.Add(pdfHeader);



                                }
                                else
                                {
                                    coltop += 30;
                                    pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, coltop, 595, 50), ContentAlignment.MiddleLeft, "Non Vegiterian");
                                    mypdfpage.Add(pdfHeader);
                                }


                                int minus = 50;
                                pdfHeader = new PdfTextArea(fontColwebsiteemailbold, Color.Black, new PdfArea(mydoc, 30, 600 - minus, 595, 30), ContentAlignment.MiddleLeft, "REGISTRATION FEE : Rs.2500 Per Person (exclude accomodation)");
                                mypdfpage.Add(pdfHeader);
                                pdfHeader = new PdfTextArea(fontColwebsiteemailbold, Color.Black, new PdfArea(mydoc, 30, 620 - minus, 595, 30), ContentAlignment.MiddleLeft, "Early Bird Registration");
                                mypdfpage.Add(pdfHeader);

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 640 - minus, 595, 30), ContentAlignment.MiddleLeft, "Rs. 2000 if Registered & Payment received on or Before 31st May 2016");
                                mypdfpage.Add(pdfHeader);

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 660 - minus, 595, 30), ContentAlignment.MiddleLeft, "Rs. 2250 if Registered & Payment received between 1st June to 15 July 2016");
                                mypdfpage.Add(pdfHeader);

                                //pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 50, 680, 595, 30), ContentAlignment.MiddleLeft, "For Payment instructions,Please See Annexure A / For Accomodation , Please See Annexure B");
                                //mypdfpage.Add(pdfHeader);

                                pdfHeader = new PdfTextArea(fontColwebsiteemailbold, Color.Black, new PdfArea(mydoc, 30, 695 - minus, 595, 30), ContentAlignment.MiddleLeft, "Details Of Payment Toward Registration : Online Payment");
                                mypdfpage.Add(pdfHeader);


                                //pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 735, 595, 25), ContentAlignment.MiddleLeft, "Details Of Payment Toward Registration :");
                                //mypdfpage.Add(pdfHeader);

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 750, 200, 25), ContentAlignment.MiddleLeft, "Date :" + DateTime.Now.ToString("dd/MM/yyyy"));
                                mypdfpage.Add(pdfHeader);

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 350, 750, 200, 25), ContentAlignment.MiddleRight, "Signature :----------------------------");
                                mypdfpage.Add(pdfHeader);

                                PdfLine pdflin = new PdfLine(mydoc, new Point(30, 767), new Point(565, 767), Color.Black, 1);
                                mypdfpage.Add(pdflin);

                                pdfHeader = new PdfTextArea(fontColwebsiteemailbold, Color.Black, new PdfArea(mydoc, 30, 765, 595, 25), ContentAlignment.MiddleCenter, "For Alumni Office Use Only");
                                mypdfpage.Add(pdfHeader);

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 785, 595, 20), ContentAlignment.MiddleLeft, "Received With Payment on :----------------------------------------");
                                mypdfpage.Add(pdfHeader);

                                pdfHeader = new PdfTextArea(fontColwebsiteemail, Color.Black, new PdfArea(mydoc, 30, 800, 595, 20), ContentAlignment.MiddleLeft, "VIDE -- Online Payment/Credit/Debit Card/Cash");
                                mypdfpage.Add(pdfHeader);
                                if (status == true)
                                {
                                    mypdfpage.SaveToDocument();
                                }
                            }
                        }
                    }
                }
            }
            if (status == true)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            if (print == false)
            {
                lblalert.Text = "Please Select Any One Student";
                Errpopup.Visible = true;
            }
        }
        catch { }
    }




    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        Errpopup.Visible = false;
    }

    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Alumini Report Name";
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
            degreedetails = "Alumini Report";
            pagename = "AluminiReport.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
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
        catch { }
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
        catch { }
    }

    #endregion
}