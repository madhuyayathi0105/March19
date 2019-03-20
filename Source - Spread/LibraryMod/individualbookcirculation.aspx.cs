using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class LibraryMod_individualbookcirculation : System.Web.UI.Page
{
    # region fielddeclaration
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataRow dr;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    DataTable dtCommon = new DataTable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable columnhash = new Hashtable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    static int selected_name_sub_wise = 0;
    static int selected_roll_sub_wise = 0;
    static int selected_reg_sub_wise = 0;
    string status = string.Empty;
    DataTable bookcir = new DataTable();
    DataTable bookdet = new DataTable();
    DataSet bookcirculation = new DataSet();
    static Dictionary<int, string> dicRowColor = new Dictionary<int, string>();

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
                BindBatchYear();
                binddeg();
                binddept();
                searchby();
            }
        }
        catch
        {
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
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }
    #endregion

    #region Library

    public void BindLibrary(string LibCodeCollection)
    {
        try
        {
            ddlLibrary.Items.Clear();
            ds.Clear();
            // string College = ddlCollege.SelectedValue.ToString();

            string strquery = "SELECT Lib_Code,Lib_Name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) FROM Library " + LibCodeCollection + " and College_Code ='" + userCollegeCode + "' ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlLibrary.DataSource = ds;
                ddlLibrary.DataTextField = "Lib_Name";
                ddlLibrary.DataValueField = "Lib_Code";
                ddlLibrary.DataBind();
                ddlLibrary.Items.Insert(0, "All");
            }
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    #endregion

    protected void BindBatchYear()
    {
        string qry = " select distinct Batch_Year from Registration order by batch_year desc";
        DataTable dtbatchyr = dirAcc.selectDataTable(qry);
        ddlBatch.Items.Clear();
        if (dtbatchyr.Rows.Count > 0)
        {
            ddlBatch.DataSource = dtbatchyr;
            ddlBatch.DataTextField = "Batch_Year";
            ddlBatch.DataValueField = "Batch_Year";
            ddlBatch.DataBind();

            //cbl_BatchYearFine.DataSource = dtbatchyr;
            //cbl_BatchYearFine.DataTextField = "Batch_Year";
            //cbl_BatchYearFine.DataValueField = "Batch_Year";
            //cbl_BatchYearFine.DataBind();
        }
    }

    public void binddeg()
    {
        try
        {
            ddldegree.Items.Clear();
            string collegecode = ddlCollege.SelectedValue.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collegecode + "'";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch { }
    }

    public void binddept()
    {
        try
        {
            ddlbranch.Items.Clear();
            string batch2 = "";
            string degree = "";
            string course_id = ddldegree.SelectedItem.Value;
            string collcode = ddlCollege.SelectedValue;
            string strquery = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code='" + collcode + "'  and deptprivilages.Degree_code=degree.Degree_code and user_code='" + userCode + "' order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
            // string strquery = " SELECT Course_Name+'-'+Dept_Name Degree,Degree_Code FROM Degree G,Course C,Department D WHERE G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code AND G.College_Code ='" + collcode + "' and c.course_id in(" + course_id + ")  ORDER BY Course_Name,Dept_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");


            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }

        }
        catch { }
    }

    #region searchby
    public void searchby()
    {
        try
        {

            ddlsearchby.Items.Add("Roll No");
            ddlsearchby.Items.Add("Reg No");

        }
        catch (Exception ex)
        { }
    }
    #endregion

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
        }
        catch (Exception ex)
        { }

    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        { }

    }

    protected void cbdate_OnCheckedChanged(object sender, EventArgs e)
    {

        try
        {
            if (cbdate.Checked)
            {
                txt_fromdate.Enabled = true;
                txt_todate.Enabled = true;

            }
            else
            {
                txt_fromdate.Enabled = false;
                txt_todate.Enabled = false;
            }

        }
        catch (Exception ex) { }
        {
        }

    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddept();
    }

    protected void ddlsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlsearchby.SelectedIndex == 0)
        {
            txtsearchroll.Visible = true;
        }
        if (ddlsearchby.SelectedIndex == 1)
        {
            txtsearchroll.Visible = true;
        }
    }

    protected void txtsearch1_TextChanged(object sender, EventArgs e)
    {

    }

    # region Getrno1
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno1(string prefixText)
    {
        List<string> name = new List<string>();

        try
        {

            string query = "";

            WebService ws = new WebService();

            {
                string txtval = string.Empty;

                if (selected_name_sub_wise == 0)
                {

                    query = "select distinct Stud_Name from Registration where Stud_Name like '" + prefixText + "%'  order by Stud_Name";
                }


            }
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    # endregion

    # region Getrno
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> rollreg = new List<string>();

        try
        {

            string qry = "";

            WebService ws = new WebService();

            {
                string txtval = string.Empty;

                if (selected_roll_sub_wise == 0)
                {

                    qry = "select distinct Roll_No from Registration where Roll_No like '" + prefixText + "%'  order by Roll_No";
                }
                else if (selected_reg_sub_wise == 1)
                {


                    qry = "select distinct Reg_No from Registration where Reg_No like '" + prefixText + "%' order by Reg_No";
                }

            }
            rollreg = ws.Getname(qry);
            return rollreg;
        }
        catch { return rollreg; }
    }
    # endregion

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
            DataRow dr1;
            colour.Visible = true;
            string datebok = string.Empty;
            string typebok = string.Empty;
            string Var_Status = "";
            string bokname = string.Empty;
            string bokaccno = string.Empty;
            string modebok = string.Empty;
            string datedubok = string.Empty;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string rolln = Convert.ToString(gridview1.Rows[Convert.ToInt32(rowIndex)].Cells[2].Text);
            string editQ = "SELECT distinct bk.Acc_No,bk.Title,return_flag,isnull(br.mode,'0') as mode,CONVERT(varchar(20),borrow_date,103)borrow_date,CONVERT(varchar(20),return_date,103)return_date,CONVERT(varchar(20),due_date,103)due_date FROM bookdetails bk,borrow br,Registration r WHERE  br.roll_no ='" + rolln + "' and br.acc_no=bk.Acc_No and r.Roll_No=br.roll_no order by borrow_date desc";
            DataSet edit = new DataSet();
            edit = da.select_method_wo_parameter(editQ, "Text");
            colour.Visible = true;
            bookdet.Columns.Add("S.No", typeof(string));
            bookdet.Columns.Add("Acc No", typeof(string));
            bookdet.Columns.Add("Book Name", typeof(string));
            bookdet.Columns.Add("Issue Date", typeof(string));
            bookdet.Columns.Add("Due Date", typeof(string));
            bookdet.Columns.Add("Return Date", typeof(string));
            bookdet.Columns.Add("Transaction Detail", typeof(string));
            bookdet.Columns.Add("Fine Amount", typeof(string));
            bookdet.Columns.Add("Fine Status", typeof(string));
            int rowCnt = 0;
            dicRowColor.Clear();

            if (edit.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < edit.Tables[0].Rows.Count; k++)
                {
                    double TotFinAmt = 0;
                    sno++;
                    dr1 = bookdet.NewRow();
                    dr1["S.No"] = Convert.ToString(sno);
                    bokaccno = Convert.ToString(edit.Tables[0].Rows[k]["Acc_No"]).Trim();
                    dr1["Acc No"] = bokaccno;
                    bokname = Convert.ToString(edit.Tables[0].Rows[k]["Title"]).Trim();
                    dr1["Book Name"] = bokname;
                    datebok = Convert.ToString(edit.Tables[0].Rows[k]["borrow_date"]).Trim();
                    string[] datbor = datebok.Split('/');
                    if (datbor.Length == 3)
                        datebok = datbor[1] + '/' + datbor[0] + '/' + datbor[2];
                    editQ = "select fineamt,case when ((fineamt - paidamt) =0 )then 'Paid' else 'UnPaid/Returned' end as 'Status' from Fine_Details where roll_no='" + rolln + "' and acc_no='" + bokaccno + "' and iss_date='" + datebok + "' group by fineamt,paidamt";
                    DataSet dsFine = new DataSet();
                    dsFine = d2.select_method_wo_parameter(editQ, "text");
                    if (dsFine.Tables[0].Rows.Count > 0)
                    {
                        for (int fin = 0; fin < dsFine.Tables[0].Rows.Count; fin++)
                        {
                            double fineamt = Convert.ToDouble(dsFine.Tables[0].Rows[fin]["fineamt"]);
                            TotFinAmt = TotFinAmt + fineamt;
                        }
                        dr1["Fine Amount"] = TotFinAmt;
                        dr1["Fine Status"] = Convert.ToString(dsFine.Tables[0].Rows[0]["Status"]);
                    }
                    else
                    {
                        dr1["Fine Amount"] = "0";
                        dr1["Fine Status"] = "";
                    }
                    datebok = Convert.ToString(edit.Tables[0].Rows[k]["borrow_date"]).Trim();
                    dr1["Issue Date"] = datebok;                   

                    datedubok = Convert.ToString(edit.Tables[0].Rows[k]["due_date"]).Trim();                   
                    dr1["Due Date"] = datedubok;

                    typebok = Convert.ToString(edit.Tables[0].Rows[k]["return_flag"]).Trim();
                    modebok = Convert.ToString(edit.Tables[0].Rows[k]["mode"]).Trim();

                    if (typebok == "1" && modebok.ToLower() == "false")
                    {
                       datebok = Convert.ToString(edit.Tables[0].Rows[k]["return_date"]).Trim();
                       string[] dat1 = datebok.Split('/');
                       if (dat1.Length == 3)
                           datebok = dat1[1] + '/' + dat1[0] + '/' + dat1[2];
                        datedubok = Convert.ToString(edit.Tables[0].Rows[k]["due_date"]).Trim();
                        string[] dat2 = datedubok.Split('/');
                        if (dat1.Length == 3)
                            datedubok = dat2[1] + '/' + dat2[0] + '/' + dat2[2];
                        DateTime dtreturndate = Convert.ToDateTime(datebok);
                        DateTime dtDuedate = Convert.ToDateTime(datedubok);
                        if (dtreturndate > dtDuedate)
                        {
                            dr1["Transaction Detail"] = "Late Return";
                            dr1["Return Date"] = Convert.ToString(edit.Tables[0].Rows[k]["return_date"]).Trim();
                            dicRowColor.Add(rowCnt, "Late Return");
                        }
                        else
                        {
                            dr1["Transaction Detail"] = "Returned";
                            dr1["Return Date"] = Convert.ToString(edit.Tables[0].Rows[k]["return_date"]).Trim();
                            dicRowColor.Add(rowCnt, "Returned");
                        }
                    }
                    else
                    {
                        dr1["Return Date"] = "";
                    }
                    if (typebok == "0" && modebok.ToLower() == "false")
                    {
                        dr1["Transaction Detail"] = "Issued";
                        dicRowColor.Add(rowCnt, "Issued");
                    }
                    if (typebok == "1" && modebok.ToLower() == "true")
                    {
                        dr1["Transaction Detail"] = "Lost";
                        datebok = Convert.ToString(edit.Tables[0].Rows[k]["return_date"]).Trim();
                        dr1["Return Date"] = datebok;
                        dicRowColor.Add(rowCnt, "Lost");
                    }
                   
                    bookdet.Rows.Add(dr1);
                    rowCnt++;
                }
            }
            popwindowdetails.Visible = true;
            gridview2.DataSource = bookdet;
            gridview2.DataBind();
            gridview2.Visible = true;
            gridview1.Visible = false;
            foreach (KeyValuePair<int, string> dr in dicRowColor)
            {
                int RowVal = dr.Key;
                string DicValue = dr.Value;
                if (DicValue == "Returned")
                {
                    gridview2.Rows[RowVal].BackColor = Color.LightYellow;
                }
                if (DicValue == "Issued")
                {
                    gridview2.Rows[RowVal].BackColor = Color.LightGreen;
                }
                if (DicValue == "Lost")
                {
                    gridview2.Rows[RowVal].BackColor = Color.Red;
                }
                if (DicValue == "Late Return")
                {
                    gridview2.Rows[RowVal].BackColor = Color.Orange;
                }
            }
        }
        catch
        {
        }
    }

  

    protected void btn_go_Click(object sender, EventArgs e)
    {
        string lib = string.Empty;
        string dept = string.Empty;
        string degree = string.Empty;
        string search = string.Empty;
        string batch = string.Empty;
        string Sql = string.Empty;
        string Sql1 = string.Empty;
        string roll = string.Empty;
        string select = string.Empty;
        string name = string.Empty;
        string booktaken = string.Empty;
        string bokreturn = string.Empty;
        string onhand = string.Empty;
        string infromdate = string.Empty;
        string intodate = string.Empty;

        int sno = 0;
        DataSet dsTotbok = new DataSet();
        DataSet dsTotbokreturn = new DataSet();
        DataSet dsTotbokonhand = new DataSet();

        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddlLibrary.Items.Count > 0)
                lib = Convert.ToString(ddlLibrary.SelectedValue);
            if (ddlBatch.Items.Count > 0)
                batch = Convert.ToString(ddlBatch.SelectedValue);
            if (ddldegree.Items.Count > 0)
                degree = Convert.ToString(ddldegree.SelectedValue);
            if (ddlbranch.Items.Count > 0)
                dept = Convert.ToString(ddlbranch.SelectedValue);
            if (ddlsearchby.Items.Count > 0)
                search = Convert.ToString(ddlsearchby.SelectedValue);

            string typ1 = string.Empty;
            if (ddlLibrary.Items.Count > 0)
            {
                for (int i = 0; i < ddlLibrary.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddlLibrary.SelectedItem) == "All")
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + ddlLibrary.Items[i + 1].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + ddlLibrary.Items[i + 1].Value + "";
                        }
                    }
                    else
                        typ1 = ddlLibrary.SelectedValue;
                }
            }
            string fromDate = txt_fromdate.Text;
            string toDate = txt_todate.Text;
            string[] fromdate = fromDate.Split('/');
            string[] todate = toDate.Split('/');
            if (fromdate.Length == 3)
                infromdate = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();
            if (todate.Length == 3)
                intodate = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(lib) && !string.IsNullOrEmpty(dept))
            {
                Sql = "SELECT distinct Roll_No,Stud_Name,Reg_No FROM Registration WHERE  DelFlag = 0 AND Exam_Flag = 'OK' AND Batch_Year='" + batch + "' AND Degree_Code='" + dept + "'";
                if (txtsearchroll.Text != "")
                {
                    if (ddlsearchby.SelectedItem.Text == "Roll No")
                    {
                        Sql = Sql + " AND Roll_No='" + txtsearchroll.Text + "'";
                    }
                    if (ddlsearchby.SelectedItem.Text == "Reg No")
                    {
                        Sql = Sql + " AND Reg_No='" + txtsearchroll.Text + "'";
                    }
                }
                if (txtsearchname.Text != "")
                {
                    Sql = Sql + " AND Stud_Name='" + txtsearchname.Text + "'";
                }
                Sql = Sql + " ORDER BY Roll_No";
                bookcirculation.Clear();
                bookcirculation = d2.select_method_wo_parameter(Sql, "Text");
                bookcir.Columns.Add("SNo", typeof(string));
                bookcir.Columns.Add("Select", typeof(string));
                if (ddlsearchby.SelectedItem.Text == "Roll No")
                {
                    bookcir.Columns.Add("Roll No", typeof(string));
                }
                else
                {
                    bookcir.Columns.Add("Reg No", typeof(string));
                }
                bookcir.Columns.Add("Name", typeof(string));
                bookcir.Columns.Add("Total BookTaken", typeof(string));
                bookcir.Columns.Add("Total BookReturn", typeof(string));
                bookcir.Columns.Add("Total OnHand", typeof(string));
                if (bookcirculation.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < bookcirculation.Tables[0].Rows.Count; j++)
                    {
                        string rollno = Convert.ToString(bookcirculation.Tables[0].Rows[j]["Roll_No"]).Trim();
                        string studname = Convert.ToString(bookcirculation.Tables[0].Rows[j]["Stud_Name"]).Trim();
                        Sql1 = " SELECT distinct COUNT(acc_no)as totcount FROM borrow WHERE  Roll_No ='" + rollno + "' and lib_code in ('" + typ1 + "') ";
                        if (cbdate.Checked)
                        {
                            Sql1 = Sql1 + " and borrow_date between '" + infromdate + "' and '" + intodate + "'";
                        }
                        dsTotbok.Clear();
                        dsTotbok = d2.select_method_wo_parameter(Sql1, "Text");
                        if (dsTotbok.Tables[0].Rows.Count > 0)
                        {
                            sno++;
                            dr = bookcir.NewRow();
                            dr["SNo"] = Convert.ToString(sno);
                            dr["Select"] = "View";
                            if (ddlsearchby.SelectedItem.Text == "Roll No")
                            {
                                rollno = Convert.ToString(bookcirculation.Tables[0].Rows[j]["Roll_No"]).Trim();
                                dr["Roll No"] = rollno;
                            }
                            else if (ddlsearchby.SelectedItem.Text == "Reg No")
                            {
                                string Regno = Convert.ToString(bookcirculation.Tables[0].Rows[j]["Reg_No"]).Trim();
                                dr["Reg No"] = Regno;
                            }
                            dr["Name"] = studname;
                            booktaken = Convert.ToString(dsTotbok.Tables[0].Rows[0]["totcount"]).Trim();
                            dr["Total BookTaken"] = booktaken;
                        }
                        Sql1 = " SELECT distinct COUNT(acc_no)as returncount FROM borrow WHERE  Roll_No ='" + rollno + "'  and return_flag=1  and lib_code in ('" + typ1 + "') ";

                        if (cbdate.Checked)
                        {
                            Sql1 = Sql1 + " and borrow_date between '" + infromdate + "' and '" + intodate + "'";
                        }
                        dsTotbokreturn.Clear();
                        dsTotbokreturn = d2.select_method_wo_parameter(Sql1, "Text");
                        if (dsTotbokreturn.Tables[0].Rows.Count > 0)
                        {
                            bokreturn = Convert.ToString(dsTotbokreturn.Tables[0].Rows[0]["returncount"]).Trim();
                            dr["Total BookReturn"] = bokreturn;
                        }
                        Sql1 = " SELECT distinct COUNT(acc_no)as onhand FROM borrow WHERE  Roll_No ='" + rollno + "'  and return_flag=0  and  lib_code in ('" + typ1 + "')  ";

                        if (cbdate.Checked)
                        {
                            Sql1 = Sql1 + " and borrow_date between '" + infromdate + "' and '" + intodate + "'";
                        }
                        dsTotbokonhand.Clear();
                        dsTotbokonhand = d2.select_method_wo_parameter(Sql1, "Text");
                        if (dsTotbokreturn.Tables[0].Rows.Count > 0)
                        {

                            onhand = Convert.ToString(dsTotbokonhand.Tables[0].Rows[0]["onhand"]).Trim();
                            dr["Total OnHand"] = onhand;
                        }
                        bookcir.Rows.Add(dr);
                    }
                    gridview1.DataSource = bookcir;
                    gridview1.DataBind();
                    gridview1.Visible = true;
                    print2.Visible = true;
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found!";
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch (Exception ex)
        { }
    }

    #region Print
    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(gridview1, reportname);

            }
            else
            {
                txtexcelname2.Focus();
            }
        }
        catch (Exception ex)
        {
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    public void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            lblvalidation3.Text = "";
            txtexcelname2.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "individualbookcirculation Report";
            //+'@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "individualbookcirculation.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(gridview1, pagename, degreedetails, 0, ss, "1");

            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "individualbookcirculation"); }
    }

    protected void getPrintSettings2()
    {
        try
        {

            #region Excel print settings
            string usertype = "";
            if (userCode.Trim() != "")
                usertype = " and usercode='" + userCode + "'";
            else if (groupUserCode.Trim() != "")
                usertype = " and group_code='" + groupUserCode + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed2.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                    btnprintmasterhed2.Visible = true;

                }
            }
            #endregion
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "individualbookcirculation"); }
    }

    #endregion

    protected void imagebtnpop2close_Click(object sender, EventArgs e)
    {
        try
        {
            popwindowdetails.Visible = false;
            gridview1.Visible = true;
        }
        catch
        {
        }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;

        }
        catch (Exception ex)
        { }
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

    protected void gridview2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
        }

    }
}
