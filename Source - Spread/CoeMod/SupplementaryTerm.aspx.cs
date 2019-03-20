using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class SupplementaryTerm : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string selectQuery = string.Empty;

    string college = string.Empty;
    string batch = string.Empty;
    string degree = string.Empty;
    string dept = string.Empty;
    string exammonth = String.Empty;
    string examyear = String.Empty;
    string exammonYear = String.Empty;
    string examcode = String.Empty;


    int i, row, commcount;

    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    DataTable spreaddata = new DataTable();


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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindclg();
                bindBtch();
                binddeg();
                binddept();
                bindmonyear();

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
                divspread.Visible = false;
                rptprint.Visible = false;
            }
        }
        catch { }
    }

    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBtch();
            binddeg();
            binddept();
            bindmonyear();
        }
        catch { }
    }

    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_dept.Text = "--Select--";
            if (cb_dept.Checked == true)
            {

                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                }
                txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
            }
            bindmonyear();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_dept.Checked = false;
            commcount = 0;
            txt_dept.Text = "--Select--";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_dept.Items.Count)
                {
                    cb_dept.Checked = true;
                }
                txt_dept.Text = "Department(" + commcount.ToString() + ")";
            }
            bindmonyear();
        }
        catch { }
    }
    protected void cb_monyear_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_monyear.Text = "--Select--";
            if (cb_monyear.Checked == true)
            {

                for (i = 0; i < cbl_monyear.Items.Count; i++)
                {
                    cbl_monyear.Items[i].Selected = true;
                }
                txt_monyear.Text = "Month/Year(" + (cbl_monyear.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_monyear.Items.Count; i++)
                {
                    cbl_monyear.Items[i].Selected = false;
                }
            }

        }
        catch { }
    }
    protected void cbl_monyear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_monyear.Checked = false;
            commcount = 0;
            txt_monyear.Text = "--Select--";
            for (i = 0; i < cbl_monyear.Items.Count; i++)
            {
                if (cbl_monyear.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_monyear.Items.Count)
                {
                    cb_monyear.Checked = true;
                }
                txt_monyear.Text = "Month/Year(" + commcount.ToString() + ")";
            }
        }
        catch { }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
            divspread.Visible = false;
            rptprint.Visible = false;
            lbl_error.Text = "No Records Found";
            lbl_error.Visible = true;

            Chart1.Series.Clear();
            Chart1.Visible = false;

            #region Get Input
            college = string.Empty;
            batch = string.Empty;
            degree = string.Empty;
            dept = string.Empty;
            exammonth = string.Empty;
            examyear = string.Empty;
            Hashtable addcolumnindex = new Hashtable();
            examcode = String.Empty;
            Hashtable monthYear = new Hashtable();
            DataRow dr;
            DataTable footerData = new DataTable();
            if (ddl_college.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_degree.Items.Count > 0)
            {
                college = Convert.ToString(ddl_college.SelectedValue);
                batch = Convert.ToString(ddl_batch.SelectedValue);
                degree = Convert.ToString(ddl_degree.SelectedValue);

                int indexnew = 0;
                if (cbl_dept.Items.Count > 0)
                {
                    for (i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        if (cbl_dept.Items[i].Selected == true)
                        {
                            indexnew++;

                            if (dept == "")
                            {
                                dept = Convert.ToString(cbl_dept.Items[i].Value);
                            }
                            else
                            {
                                dept = dept + "','" + Convert.ToString(cbl_dept.Items[i].Value);
                            }
                            spreaddata.Columns.Add(cbl_dept.Items[i].Text);
                            addcolumnindex.Add(Convert.ToString(cbl_dept.Items[i].Text), indexnew - 1);
                        }
                    }
                }

                if (cbl_monyear.Items.Count > 0)
                {
                    for (i = 0; i < cbl_monyear.Items.Count; i++)
                    {
                        if (cbl_monyear.Items[i].Selected == true)
                        {
                            if (exammonth == "")
                            {
                                exammonth = Convert.ToString(cbl_monyear.Items[i].Value);
                                examyear = Convert.ToString(cbl_monyear.Items[i].Text.Split(' ')[2]);
                            }
                            else
                            {
                                exammonth = exammonth + "','" + Convert.ToString(cbl_monyear.Items[i].Value);
                                examyear = examyear + "','" + Convert.ToString(cbl_monyear.Items[i].Text.Split(' ')[2]);
                            }
                            Chart1.Series.Add(Convert.ToString(cbl_monyear.Items[i].Text));
                            Chart1.Series[0].BorderWidth = 2;

                            dr = spreaddata.NewRow();
                            spreaddata.Rows.Add(dr);
                        }
                    }
                }


                selectQuery = "select exam_code,degree_code,batch_year,exam_Month,exam_year from exam_details where degree_code in ('" + dept + "') and batch_year='" + batch + "' and exam_Month in ('" + exammonth + "') and exam_year in ('" + examyear + "')";

                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(selectQuery, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        if (examcode == "")
                        {
                            examcode = Convert.ToString(ds1.Tables[0].Rows[i][0]);
                        }
                        else
                        {
                            examcode = examcode + "','" + Convert.ToString(ds1.Tables[0].Rows[i][0]);
                        }
                    }
                }


            #endregion
                if (exammonth != "" && examyear != "" && examcode != "")
                {
                    selectQuery = "";
                    selectQuery = "select m.exam_code,Exam_Month,exam_year,COUNT(distinct m.roll_no) appeared,r.degree_code from mark_entry m,Exam_Details x,Registration r,Degree g,course c,Department d where m.exam_code = x.exam_code and m.roll_no = r.Roll_No and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.Dept_Code = d.Dept_Code and g.college_code = c.college_code and g.college_code = c.college_code and DelFlag=0 and Exam_Flag <>'debar' and m.exam_code = x.exam_code and m.exam_code in ('" + examcode + "') group by r.degree_code,g.Dept_Code,dept_name,m.exam_code,Exam_Month,exam_year order by Dept_Name,Exam_year,Exam_Month";

                    selectQuery += "  select count(distinct m.roll_no) fail ,exam_code,r.degree_code from mark_entry m,Registration r where m.roll_no=r.Roll_No and r.DelFlag=0 and r.Exam_Flag<>'debar' and  exam_code in ('" + examcode + "') and result<>'pass'  group by exam_code,r.degree_code";

                    selectQuery += " select  count(distinct m.roll_no) Abse, ex.exam_code,ex.degree_code from mark_entry m,Exam_Details ex,Registration r where ex.exam_code =m.exam_code and m.attempts = 1  and r.Roll_No=m.roll_no and CC=0 and DelFlag=0 and Exam_Flag <>'debar'  and ltrim(rtrim(type))='' and (result ='AA' or result =  'AAA' or result = 'UA') and passorfail=0  and ex.exam_code in ('" + examcode + "' ) group by ex.exam_code,ex.degree_code";


                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQuery, "Text");

                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {

                        #region column header
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 2;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].Width = 50;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].Width = 100;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                        Hashtable columncountnew = new Hashtable();
                        //int columncountFoot = 2;
                        //footerData.Columns.Add((columncountFoot - 2).ToString());
                        //footerData.Columns.Add((columncountFoot - 1).ToString());
                        for (i = 0; i < cbl_monyear.Items.Count; i++)
                        {
                            if (cbl_monyear.Items[i].Selected == true)
                            {
                                FpSpread1.Sheets[0].ColumnCount += 2;
                                columncountnew.Add(Convert.ToString(cbl_monyear.Items[i].Text), FpSpread1.Sheets[0].ColumnCount);
                                //columncountFoot += 2;

                                //footerData.Columns.Add((columncountFoot-2).ToString());
                                //footerData.Columns.Add((columncountFoot-1).ToString());

                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Text = cbl_monyear.Items[i].Text;

                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 1, 2);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "Appeared ";

                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Passed";

                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;


                            }
                        }
                        FpSpread1.Sheets[0].ColumnCount += 1;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";

                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                        columncountnew.Add("Total", FpSpread1.Sheets[0].ColumnCount);
                        // columncountFoot++;
                        //footerData.Columns.Add((columncountFoot).ToString());

                        int columnspan = 0;
                        for (i = 0; i < cbl_monyear.Items.Count; i++)
                        {
                            if (cbl_monyear.Items[i].Selected == true)
                            {
                                columnspan++;
                                // columncountFoot++;
                                //footerData.Columns.Add((columncountFoot-1).ToString());

                                FpSpread1.Sheets[0].ColumnCount += 1;
                                columncountnew.Add(Convert.ToString(cbl_monyear.Items[i].Text) + "percentage", FpSpread1.Sheets[0].ColumnCount);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Pass Percentage";


                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = cbl_monyear.Items[i].Text;

                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                            }
                        }
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - columnspan, 1, columnspan);
                        #endregion

                        #region row values
                        int serialno = 0;
                        int totalcolindex = 0;
                        double total = 0;
                        int indexPasspercent1 = 2;

                        if (cbl_monyear.Items.Count > 0)
                        {
                            for (i = 0; i < cbl_monyear.Items.Count; i++)
                            {
                                if (cbl_monyear.Items[i].Selected == true)
                                {
                                    indexPasspercent1 += 2;
                                }
                            }
                        }

                        Hashtable passcount = new Hashtable();
                        Hashtable appeardcount = new Hashtable();
                        double totalcountvalue = 0;
                        Hashtable passpercentagecount = new Hashtable();


                        if (cbl_dept.Items.Count > 0)
                        {
                            for (row = 0; row < cbl_dept.Items.Count; row++)
                            {
                                int indexPasspercent = indexPasspercent1;
                                if (cbl_dept.Items[row].Selected == true)
                                {
                                    serialno++;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = serialno.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = cbl_dept.Items[row].Text;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                    total = 0;
                                    double appeared = 0;
                                    double fail = 0;
                                    double pass = 0;
                                    double passpercent = 0;

                                    totalcolindex = 0;


                                    int getindex = Convert.ToInt32(addcolumnindex[Convert.ToString(cbl_dept.Items[row].Text)]);

                                    int totalrows = 0;
                                    if (cbl_monyear.Items.Count > 0)
                                    {
                                        int columnindex = 0;

                                        for (i = 0; i < cbl_monyear.Items.Count; i++)
                                        {

                                            if (cbl_monyear.Items[i].Selected == true)
                                            {
                                                totalrows++;
                                                columnindex += 2;
                                                totalcolindex = columnindex;

                                                indexPasspercent++;
                                                dept = Convert.ToString(cbl_dept.Items[row].Value);
                                                exammonth = Convert.ToString(cbl_monyear.Items[i].Value);
                                                examyear = Convert.ToString(cbl_monyear.Items[i].Text.Split(' ')[2]);
                                                ds1.Tables[0].DefaultView.RowFilter = "degree_code='" + dept + "' and batch_year='" + batch + "' and exam_Month ='" + exammonth + "' and exam_year='" + examyear + "'";
                                                DataView dvExamcode = ds1.Tables[0].DefaultView;
                                                if (dvExamcode.Count > 0)
                                                {

                                                    ds.Tables[0].DefaultView.RowFilter = "degree_code='" + dept + "' and exam_code='" + dvExamcode[0]["exam_code"] + "'";
                                                    DataView dvAppeared = ds.Tables[0].DefaultView;

                                                    ds.Tables[1].DefaultView.RowFilter = "degree_code='" + dept + "' and exam_code='" + dvExamcode[0]["exam_code"] + "'";
                                                    DataView dvFail = ds.Tables[1].DefaultView;

                                                    ds.Tables[2].DefaultView.RowFilter = "degree_code='" + dept + "' and exam_code='" + dvExamcode[0]["exam_code"] + "'";
                                                    DataView dvabsent = ds.Tables[2].DefaultView;
                                                    appeared = 0;
                                                    fail = 0;
                                                    pass = 0;
                                                    passpercent = 0;

                                                    if (dvAppeared.Count > 0)
                                                    {
                                                        string ap = Convert.ToString(dvAppeared[0]["appeared"]);

                                                        if (ap != "")
                                                        {
                                                            appeared = Convert.ToDouble(ap);
                                                        }
                                                        if (dvabsent.Count > 0)
                                                        {
                                                            string absent = Convert.ToString(dvabsent[0]["Abse"]);
                                                            if (absent.Trim() != "")
                                                            {
                                                                appeared = appeared - Convert.ToDouble(absent);
                                                            }
                                                        }

                                                        if (!appeardcount.Contains(Convert.ToString(cbl_monyear.Items[i].Text)))
                                                        {
                                                            appeardcount.Add(Convert.ToString(cbl_monyear.Items[i].Text), appeared);
                                                        }
                                                        else
                                                        {
                                                            string getvlaue = Convert.ToString(appeardcount[Convert.ToString(cbl_monyear.Items[i].Text)]);
                                                            if (getvlaue.Trim() != "")
                                                            {
                                                                double countvalue = Convert.ToDouble(getvlaue) + Convert.ToDouble(appeared);
                                                                appeardcount.Remove(Convert.ToString(cbl_monyear.Items[i].Text));
                                                                appeardcount.Add(Convert.ToString(cbl_monyear.Items[i].Text), countvalue);
                                                            }
                                                        }
                                                    }
                                                    if (dvFail.Count > 0)
                                                    {
                                                        string fai = Convert.ToString(dvFail[0]["fail"]);
                                                        if (fai != "")
                                                        {
                                                            fail = Convert.ToDouble(fai);
                                                        }
                                                        pass = appeared - fail;
                                                        if (!passcount.Contains(Convert.ToString(cbl_monyear.Items[i].Text)))
                                                        {
                                                            passcount.Add(Convert.ToString(cbl_monyear.Items[i].Text), pass);
                                                        }
                                                        else
                                                        {
                                                            string getvlaue = Convert.ToString(passcount[Convert.ToString(cbl_monyear.Items[i].Text)]);
                                                            if (getvlaue.Trim() != "")
                                                            {
                                                                double countvalue = Convert.ToDouble(getvlaue) + Convert.ToDouble(pass);
                                                                passcount.Remove(Convert.ToString(cbl_monyear.Items[i].Text));
                                                                passcount.Add(Convert.ToString(cbl_monyear.Items[i].Text), countvalue);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        pass = appeared;
                                                        if (!passcount.Contains(Convert.ToString(cbl_monyear.Items[i].Text)))
                                                        {
                                                            passcount.Add(Convert.ToString(cbl_monyear.Items[i].Text), pass);
                                                        }
                                                        else
                                                        {
                                                            string getvlaue = Convert.ToString(passcount[Convert.ToString(cbl_monyear.Items[i].Text)]);
                                                            if (getvlaue.Trim() != "")
                                                            {
                                                                double countvalue = Convert.ToDouble(getvlaue) + Convert.ToDouble(pass);
                                                                passcount.Remove(Convert.ToString(cbl_monyear.Items[i].Text));
                                                                passcount.Add(Convert.ToString(cbl_monyear.Items[i].Text), countvalue);
                                                            }
                                                        }
                                                    }
                                                    if (pass != 0)
                                                    {
                                                        passpercent = Math.Round((pass / appeared) * 100, 2);
                                                    }

                                                    if (!passpercentagecount.Contains(Convert.ToString(cbl_monyear.Items[i].Text)))
                                                    {
                                                        passpercentagecount.Add(Convert.ToString(cbl_monyear.Items[i].Text), passpercent);
                                                    }
                                                    else
                                                    {
                                                        string getvlaue = Convert.ToString(passpercentagecount[Convert.ToString(cbl_monyear.Items[i].Text)]);
                                                        if (getvlaue.Trim() != "")
                                                        {
                                                            double countvalue = Convert.ToDouble(getvlaue) + Convert.ToDouble(passpercent);
                                                            passpercentagecount.Remove(Convert.ToString(cbl_monyear.Items[i].Text));
                                                            passpercentagecount.Add(Convert.ToString(cbl_monyear.Items[i].Text), countvalue);
                                                        }
                                                    }


                                                    total += pass;

                                                    totalcountvalue = totalcountvalue + total;

                                                    spreaddata.Rows[totalrows - 1][getindex] = Convert.ToString(passpercent);

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnindex].Text = appeared.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnindex].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnindex].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnindex].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnindex].Font.Name = "Book Antiqua";

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnindex + 1].Text = pass.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnindex + 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnindex + 1].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnindex + 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnindex + 1].Font.Name = "Book Antiqua";

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indexPasspercent].Text = passpercent.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indexPasspercent].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indexPasspercent].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indexPasspercent].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indexPasspercent].Font.Name = "Book Antiqua";


                                                }

                                            }
                                        }

                                    }


                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalcolindex + 2].Text = total.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalcolindex + 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalcolindex + 2].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalcolindex + 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalcolindex + 2].Font.Name = "Book Antiqua";
                                }

                            }
                            FpSpread1.Sheets[0].RowCount += 2;
                            for (i = 0; i < cbl_monyear.Items.Count; i++)
                            {

                                if (cbl_monyear.Items[i].Selected == true)
                                {
                                    int columnidexnew = Convert.ToInt32(columncountnew[Convert.ToString(cbl_monyear.Items[i].Text)]);
                                    int percentagenew = Convert.ToInt32(columncountnew[Convert.ToString(cbl_monyear.Items[i].Text) + "percentage"]);
                                    double passcount_new = Convert.ToDouble(passcount[Convert.ToString(cbl_monyear.Items[i].Text)]);
                                    double appeard_new = Convert.ToDouble(appeardcount[Convert.ToString(cbl_monyear.Items[i].Text)]);



                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = Convert.ToString("Total");
                                    FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 2, 0, 1, 2);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew - 1].Text = Convert.ToString(passcount_new);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew - 1].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew - 1].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew - 2].Text = Convert.ToString(appeard_new);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew - 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew - 2].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew - 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew - 2].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Overall Pass Percentage");
                                    FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                                    double per = 0;
                                    if (passcount_new != 0)
                                    {
                                        per = Math.Round(Convert.ToDouble(passcount_new) / Convert.ToDouble(appeard_new) * Convert.ToDouble(100), 2);
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percentagenew - 1].Text = Convert.ToString(per);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percentagenew - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percentagenew - 1].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percentagenew - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percentagenew - 1].Font.Name = "Book Antiqua";

                                }
                            }
                            int columnidexnew1 = Convert.ToInt32(columncountnew["Total"]);

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew1 - 1].Text = Convert.ToString(totalcountvalue);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew1 - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew1 - 1].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew1 - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columnidexnew1 - 1].Font.Name = "Book Antiqua";

                            for (i = columnidexnew1; i < FpSpread1.Sheets[0].ColumnCount; i++)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, i].Text = Convert.ToString("-");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, i].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, i].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, i].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, i].Font.Name = "Book Antiqua";
                            }
                            for (i = 2; i < columnidexnew1; i++)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].Text = Convert.ToString("-");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].Font.Name = "Book Antiqua";
                            }

                        }

                        if (spreaddata.Rows.Count > 0)
                        {
                            for (int chart_i = 0; chart_i < spreaddata.Columns.Count; chart_i++)
                            {
                                for (int chart_j = 0; chart_j < spreaddata.Rows.Count; chart_j++)
                                {
                                    string subnncode = Convert.ToString(spreaddata.Columns[chart_i]);
                                    string m1 = spreaddata.Rows[chart_j][chart_i].ToString();
                                    Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                    Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                    Chart1.Series[chart_j].IsXValueIndexed = true;
                                }
                            }
                            Chart1.Visible = true;
                        }
                        #endregion
                    }
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        FpSpread1.Visible = true;
                        divspread.Visible = true;
                        rptprint.Visible = true;
                        lbl_error.Visible = false;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    }

                }

            }
        }
        catch
        {
        }
    }
    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindBtch()
    {
        try
        {
            ddl_batch.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();

            }


        }
        catch { }
    }
    public void binddeg()
    {
        try
        {
            ddl_degree.Items.Clear();

            batch = "";
            batch = Convert.ToString(ddl_batch.SelectedValue.ToString());
            if (batch != "")
            {
                ds.Clear();
                ds = d2.BindDegree(singleuser, group_user, ddl_college.SelectedItem.Value, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_degree.DataSource = ds;
                    ddl_degree.DataTextField = "course_name";
                    ddl_degree.DataValueField = "course_id";
                    ddl_degree.DataBind();

                }
            }
        }
        catch { }
    }
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            degree = "";
            degree = Convert.ToString(ddl_degree.SelectedValue.ToString());

            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, ddl_college.SelectedItem.Value, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }
        }
        catch { }
    }
    public void bindmonyear()
    {
        try
        {
            college = "";
            batch = "";
            degree = "";
            dept = "";

            cbl_monyear.Items.Clear();
            cb_monyear.Checked = false;
            txt_monyear.Text = "---Select---";

            if (ddl_college.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_degree.Items.Count > 0)
            {
                college = Convert.ToString(ddl_college.SelectedValue);
                batch = Convert.ToString(ddl_batch.SelectedValue);
                degree = Convert.ToString(ddl_degree.SelectedValue);


                if (cbl_dept.Items.Count > 0)
                {
                    for (i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        if (cbl_dept.Items[i].Selected == true)
                        {
                            if (dept == "")
                            {
                                dept = Convert.ToString(cbl_dept.Items[i].Value);
                            }
                            else
                            {
                                dept = dept + "','" + Convert.ToString(cbl_dept.Items[i].Value);
                            }
                        }
                    }
                }
                selectQuery = "select distinct Exam_Month , Exam_year  from Exam_Details where batch_year ='" + batch + "' and degree_code in ('" + dept + "') order by Exam_year asc";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQuery, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    exammonth = "";
                    examyear = "";
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        exammonth = Convert.ToString(ds.Tables[0].Rows[i]["Exam_Month"]);
                        examyear = Convert.ToString(ds.Tables[0].Rows[i]["Exam_year"]);
                        string monthyear = returnMonYear(exammonth) + examyear;

                        cbl_monyear.Items.Add(monthyear);
                        cbl_monyear.Items[cbl_monyear.Items.Count - 1].Value = exammonth;
                        //cbl_monyear.DataSource = ds;
                        //cbl_monyear.DataTextField = "Exam_Month";
                        //cbl_monyear.DataValueField = "Exam_year";
                        //cbl_monyear.DataBind();

                    }
                    if (cbl_monyear.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_monyear.Items.Count; i++)
                        {
                            cbl_monyear.Items[i].Selected = true;
                        }
                        txt_monyear.Text = "Month/Year(" + cbl_monyear.Items.Count + ")";
                        cb_monyear.Checked = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    public string returnMonYear(string numeral)
    {
        string monthyear = String.Empty;
        switch (numeral)
        {
            case "1":
                monthyear = "Jan - ";
                break;
            case "2":
                monthyear = "Feb - ";
                break;
            case "3":
                monthyear = "Mar - ";
                break;
            case "4":
                monthyear = "Apr - ";
                break;
            case "5":
                monthyear = "May - ";
                break;
            case "6":
                monthyear = "Jun - ";
                break;
            case "7":
                monthyear = "Jul - ";
                break;
            case "8":
                monthyear = "Aug - ";
                break;
            case "9":
                monthyear = "Sep - ";
                break;
            case "10":
                monthyear = "Oct - ";
                break;
            case "11":
                monthyear = "Nov - ";
                break;
            case "12":
                monthyear = "Dec - ";
                break;
        }
        return monthyear;
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void ddl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddeg();
        binddept();
        bindmonyear();
    }
    protected void ddl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddept();
        bindmonyear();
    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Result Analysis (Supplementary Exam)";
            try
            {
                degreedetails = "Result Analysis for " + ddl_degree.SelectedItem.Text + " - " + ddl_batch.SelectedItem.Text + " " + "(Supplementary Exam)";
            }
            catch
            {

            }
            string pagename = "Supplementaryterm.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch
        {

        }

    }

}