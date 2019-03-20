using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;

public partial class Reg_no_wise_Barcode : System.Web.UI.Page
{
    [Serializable()]
    public class MyImg : ImageCellType
    {

        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(75);
            return img;


        }

    }
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_header = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_reg = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    string query_reg = "", query_header = "";
    int sl_no = 0;


    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

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
            if (!Page.IsPostBack)
            {
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                FpSpread1.Visible = false;
                lblnorec.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbatch()
    {
        ////batch
        ddlbatch.Items.Clear();
        string sqlstring = "";
        int max_bat = 0;
        con.Close();
        con.Open();
        SqlCommand cmd;
        cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataSet ds1 = new DataSet();
        da1.Fill(ds1);
        ddlbatch.DataSource = ds1;
        ddlbatch.DataValueField = "batch_year";
        ddlbatch.DataTextField = "batch_year";
        ddlbatch.DataBind();

        //----------------display max year value 
        sqlstring = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
        max_bat = Convert.ToInt32(GetFunction(sqlstring));
        ddlbatch.SelectedValue = max_bat.ToString();
        con.Close();
        //binddegree();

    }
    public void binddegree()
    {
        ////degree
        ddldegree.Items.Clear();
        con.Close();
        con.Open();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();

        DataSet ds = Bind_Degree(collegecode, usercode);
        ddldegree.DataSource = ds;
        ddldegree.DataValueField = "course_id";
        ddldegree.DataTextField = "course_name";
        ddldegree.DataBind();
        //bindbranch();

    }
    public DataSet Bind_Degree(string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public void bindbranch()
    {
        //--------load degree
        ddlbranch.Items.Clear();
        con.Close();
        con.Open();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        string course_id = ddldegree.SelectedValue.ToString();
        DataSet ds = Bind_Dept(course_id, collegecode.ToString(), usercode);
        ddlbranch.DataSource = ds;
        ddlbranch.DataTextField = "dept_name";
        ddlbranch.DataValueField = "degree_code";
        ddlbranch.DataBind();
        con.Close();
    }
    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public void bindsem()
    {

        //--------------------semester load
        ddlduration.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        SqlCommand cmd1;
        cmd1 = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
        dr = cmd1.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            first_year = Convert.ToBoolean(dr[1].ToString());
            duration = Convert.ToInt16(dr[0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlduration.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlduration.Items.Add(i.ToString());
                }

            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd1 = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlduration.Items.Clear();
            dr1 = cmd1.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr1[1].ToString());
                duration = Convert.ToInt16(dr1[0].ToString());

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlduration.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlduration.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }
        con.Close();
        //bindsec();
    }
    public string GetFunction(string sql)
    {
        string s;
        SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        con1.Open();
        SqlCommand com3 = new SqlCommand(sql, con1);
        SqlDataReader dr5;
        dr5 = com3.ExecuteReader();
        dr5.Read();
        if (dr5.HasRows == true)
        {
            if (dr5[0].ToString() == null)
            {
                s = "";
            }
            else
            {
                s = dr5[0].ToString();
            }
        }
        else
        {
            s = "";
        }

        con1.Close();
        return s;
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();

    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
    }
    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 0;
        FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
        FpSpread1.Sheets[0].ColumnCount = 4;
        FpSpread1.Sheets[0].RowCount = 0;
        //FpSpread1.Sheets[0].Columns[0].Width = 30;
        //FpSpread1.Sheets[0].Columns[1].Width = 10;
        //FpSpread1.Sheets[0].Columns[2].Width = 10;
        FpSpread1.Sheets[0].RowCount++;
        FpSpread1.Sheets[0].Cells[0, 0].Text = "Sl. No";
        FpSpread1.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[0].Width = 100;
        FpSpread1.Sheets[0].Cells[0, 0].Font.Bold = true;

        FpSpread1.Sheets[0].Cells[0, 1].Text = "Reg. No";
        FpSpread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[1].Width = 200;
        FpSpread1.Sheets[0].Cells[0, 1].Font.Bold = true;


        FpSpread1.Sheets[0].SpanModel.Add(0, 2, 1, 2);
        FpSpread1.Sheets[0].Cells[0, 2].Text = "Bar Code";
        FpSpread1.Sheets[0].Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[0, 2].Font.Bold = true;
        FpSpread1.Sheets[0].Columns[3].Width = 100;


        string ddlbatch_value = ddlbatch.SelectedValue.ToString();
        string s = ddldegree.SelectedValue.ToString();
        string ddlbranch_value = ddlbranch.SelectedValue.ToString();
        string ddlduration_value = ddlduration.SelectedValue.ToString();
        string reg_number = "";

        FarPoint.Web.Spread.TextCellType ttype = new FarPoint.Web.Spread.TextCellType();
        sl_no = 1;
        con_reg.Close();
        con_reg.Open();
        query_reg = "select reg_no from registration where batch_year=" + ddlbatch_value + " and degree_code=" + ddlbranch_value + " and current_semester=" + ddlduration_value + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR'";
        SqlCommand com_reg = new SqlCommand(query_reg, con_reg);
        SqlDataReader dr_reg;
        dr_reg = com_reg.ExecuteReader();
        if (dr_reg.HasRows == true)
        {
            while (dr_reg.Read())
            {
                reg_number = dr_reg["reg_no"].ToString();

                lblnorec.Visible = false;
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sl_no.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                if (reg_number != "")
                {

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = ttype;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dr_reg["reg_no"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 2);

                    string dummynumber = dr_reg["reg_no"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "*" + dummynumber + "*";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "IDAutomationHC39M";


                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                }
                else
                {
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 2);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "-";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height = 45;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "-";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                }
                sl_no++;
            }
            logo_settings();
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

        }
        else
        {
            lblnorec.Visible = true;
            lblnorec.Text = "Generate Register Number First";
            FpSpread1.Visible = false;
        }

    }
    public void logo_settings()
    {

        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpSpread1.Sheets[0].AllowTableCorner = true;

        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;

        FpSpread1.Sheets[0].ColumnHeader.RowCount = 5;
        FpSpread1.Sheets[0].ColumnCount = 4;
        MyImg mi = new MyImg();
        mi.ImageUrl = "~/images/10BIT001.jpeg";
        mi.ImageUrl = "Handler/Handler2.ashx?";
        MyImg mi2 = new MyImg();
        mi2.ImageUrl = "~/images/10BIT001.jpeg";
        mi2.ImageUrl = "Handler/Handler5.ashx?";

        con_header.Close();
        con_header.Open();
        query_header = "select collname,category,affliatedby,address1,address2,address3,phoneno,faxno,email,website from collinfo where college_code=" + Session["collegecode"] + "";
        SqlCommand com_header = new SqlCommand(query_header, con_header);
        SqlDataReader sdr_header;
        sdr_header = com_header.ExecuteReader();
        while (sdr_header.Read())
        {

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 5, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;



            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 2);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = sdr_header["collname"].ToString();
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = 10;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;



            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, 2);
            string cc =
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Text = sdr_header["category"].ToString() + ", Affliated to" + sdr_header["affliatedby"].ToString();
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = 10;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, 2);
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Text = sdr_header["address1"].ToString() + "-" + sdr_header["address2"].ToString() + "-" + sdr_header["address1"].ToString();
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Font.Size = 10;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, 2);
            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Text = "Phone : " + sdr_header["phoneno"].ToString() + "  Fax : " + sdr_header["faxno"].ToString();
            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Font.Size = 10;
            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, 2);//5th row span
            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Text = "E-Mail : " + sdr_header["email"].ToString() + "  Web Site : " + sdr_header["website"].ToString();
            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Font.Size = 10;
            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;


            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 5, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].CellType = mi2;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;



        }
    }


}