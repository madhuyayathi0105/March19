using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Services;
using System.Web.UI;
using System.Configuration;


public partial class Type_Master : System.Web.UI.Page
{
    bool cellclick = false;
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    static string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;
    static string typ = string.Empty;
    static string clgcode1 = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    static int gridview1_selectedrow = -1, gridview1_selectedcol = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["collegecode"] == null)
        //{
        //    Response.Redirect("~/Default.aspx");
        //}
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("Feedbackhome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FeedBackMOD/Feedbackhome.aspx");
                    return;
                }
            }

        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        lbl_norec1.Visible = false;
        if (!IsPostBack)
        {


            bindclg();
            bindclg1();
            BindType();
            btnSearch_Click(sender, e);
        }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }


    public void Cb_college_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            Txt_college.Text = "--Select--";
            if (Cb_college.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    Cbl_college.Items[i].Selected = true;
                }
                Txt_college.Text = "College(" + (Cbl_college.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    Cbl_college.Items[i].Selected = false;
                }
                Txt_college.Text = "--Select--";
            }

            string college = "";
            for (int row = 0; row < Cbl_college.Items.Count; row++)
            {
                if (Cbl_college.Items[row].Selected == true)
                {
                    if (college == "")
                    {
                        college = Cbl_college.Items[row].Value;
                    }
                    else
                    {
                        college = college + "','" + Cbl_college.Items[row].Value;
                    }
                }
            }
            clgcode1 = college;
            //  BindType();

        }

        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }

    }
    public void Cbl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            Txt_college.Text = "--Select--";
            Cb_college.Checked = false;

            for (int i = 0; i < Cbl_college.Items.Count; i++)
            {
                if (Cbl_college.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_college.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_college.Items.Count)
                {

                    Cb_college.Checked = true;
                }
                Txt_college.Text = "College(" + commcount.ToString() + ")";

            }
            string college = "";
            for (int row = 0; row < Cbl_college.Items.Count; row++)
            {
                if (Cbl_college.Items[row].Selected == true)
                {
                    if (college == "")
                    {
                        college = Cbl_college.Items[row].Value;
                    }
                    else
                    {
                        college = college + "','" + Cbl_college.Items[row].Value;
                    }
                }
            }
            clgcode1 = college;

            //bindhostelname();
            //  BindType();

        }

        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }

    }
    public void bindclg()
    {
        try
        {
            ds.Clear();
            Cbl_college.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_college.DataSource = ds;
                Cbl_college.DataTextField = "collname";
                Cbl_college.DataValueField = "college_code";
                Cbl_college.DataBind();
            }
            if (Cbl_college.Items.Count > 0)
            {
                for (int row = 0; row < Cbl_college.Items.Count; row++)
                {
                    Cbl_college.Items[row].Selected = true;
                    Cb_college.Checked = true;
                }
                Txt_college.Text = "College(" + Cbl_college.Items.Count + ")";

                string college = "";
                for (int row = 0; row < Cbl_college.Items.Count; row++)
                {
                    if (Cbl_college.Items[row].Selected == true)
                    {
                        if (college == "")
                        {
                            college = Cbl_college.Items[row].Value;
                        }
                        else
                        {
                            college = college + "'" + "," + "'" + Cbl_college.Items[row].Value;
                        }
                    }
                }
                clgcode1 = college;
            }

            else
            {

                Txt_college.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }



    public void Cb_college1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            Txt_college1.Text = "--Select--";
            if (Cb_college1.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_college1.Items.Count; i++)
                {
                    Cbl_college1.Items[i].Selected = true;
                }
                Txt_college1.Text = "College(" + (Cbl_college1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_college1.Items.Count; i++)
                {
                    Cbl_college1.Items[i].Selected = false;
                }
                Txt_college1.Text = "--Select--";
            }
            string college = "";
            for (int row = 0; row < Cbl_college1.Items.Count; row++)
            {
                if (Cbl_college1.Items[row].Selected == true)
                {
                    if (college == "")
                    {
                        college = Cbl_college1.Items[row].Value;
                    }
                    else
                    {

                        college = college + "'" + "," + "'" + Cbl_college1.Items[row].Value;
                    }
                }
            }

            clgcode = college;
            BindType();
        }


        catch (Exception ex)
        {

            d2.sendErrorMail(ex, collegecode1, "Options_Creation");

        }
    }
    public void Cbl_college1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            Txt_college1.Text = "--Select--";
            Cb_college1.Checked = false;

            for (int i = 0; i < Cbl_college1.Items.Count; i++)
            {
                if (Cbl_college1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_college1.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_college1.Items.Count)
                {

                    Cb_college1.Checked = true;
                }
                Txt_college1.Text = "College(" + commcount.ToString() + ")";
                string college = "";
                for (int row = 0; row < Cbl_college1.Items.Count; row++)
                {
                    if (Cbl_college1.Items[row].Selected == true)
                    {
                        if (college == "")
                        {
                            college = Cbl_college1.Items[row].Value;
                        }
                        else
                        {
                            college = college + "'" + "," + "'" + Cbl_college1.Items[row].Value;
                        }
                    }
                }
                clgcode = college;
            }
            //bindhostelname();
            BindType();

        }

        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }
    public void bindclg1()
    {
        try
        {
            ds.Clear();
            Cbl_college1.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_college1.DataSource = ds;
                Cbl_college1.DataTextField = "collname";
                Cbl_college1.DataValueField = "college_code";
                Cbl_college1.DataBind();
            }
            if (Cbl_college1.Items.Count > 0)
            {
                for (int row = 0; row < Cbl_college1.Items.Count; row++)
                {
                    Cbl_college1.Items[row].Selected = true;
                    Cb_college1.Checked = true;
                }
                Txt_college1.Text = "College(" + Cbl_college1.Items.Count + ")";

                string college = "";
                for (int row = 0; row < Cbl_college1.Items.Count; row++)
                {
                    if (Cbl_college1.Items[row].Selected == true)
                    {
                        if (college == "")
                        {
                            college = Cbl_college1.Items[row].Value;
                        }
                        else
                        {

                            college = college + "'" + "," + "'" + Cbl_college1.Items[row].Value;
                        }
                    }
                }

                clgcode = college;
            }

            else
            {

                Txt_college1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }
    public void mark()
    {
        try
        {
            string type = "";
            int typecode = 0;
            DataTable dtl = new DataTable();
            DataRow dtrow = null;
            string tagval = "";
            for (int i = 0; i < cbl_Type.Items.Count; i++)
            {
                if (cbl_Type.Items[i].Selected == true)
                {
                    if (type == "")
                    {
                        type = "" + cbl_Type.Items[i].Value.ToString() + "";
                        typecode++;
                    }
                    else
                    {
                        type = type + "','" + cbl_Type.Items[i].Value.ToString() + "";
                    }
                }
            }


            string college = "";
            if (Cbl_college1.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college1.Items.Count; i++)
                {
                    if (Cbl_college1.Items[i].Selected == true)
                    {
                        if (college == "")
                        {
                            college = Convert.ToString(Cbl_college1.Items[i].Value);
                        }
                        else
                        {
                            college = college + "','" + Convert.ToString(Cbl_college1.Items[i].Value);
                        }
                    }
                }



            }



            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);



            for (int col = 0; col < 6; col++)
            {
                dtl.Columns.Add("", typeof(string));

            }





            dtl.Rows[0][0] = "S.No";




            dtl.Rows[0][1] = "Type";




            dtl.Rows[0][2] = "Abbreviation";



            dtl.Rows[0][3] = "Score";



            dtl.Rows[0][4] = "Stars";




            dtl.Rows[0][5] = "Tag values";
            ////ds.Clear();

            string selqry = "";
            string txttype = "";
            txttype = txt_search.Text.ToString();
            if (txt_search.Text != "")
            {
                selqry = " SELECT MarkMasterPK,MarkTypeAcr, MarkType , Point, CollegeCode,NO_Of_Stars,is_remark FROM CO_MarkMaster WHERE  MarkType= ('" + txttype + "') and CollegeCode in ('" + college + "')  order by NO_Of_Stars desc";
            }
            else if (typecode > 0)
            {
                selqry = " SELECT MarkMasterPK,MarkTypeAcr, MarkType ,CollegeCode, Point,NO_Of_Stars,is_remark FROM CO_MarkMaster WHERE   MarkType in ('" + type + "') and CollegeCode in ( '" + college + "')  order by NO_Of_Stars desc";
            }
            if (selqry != "")
            {
                ds = d2.select_method_wo_parameter(selqry, "Text");
            }
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    //FpSpread1.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count; 
                    DataView dv = new DataView();

                    for (int i = 0; i < Cbl_college1.Items.Count; i++)
                    {
                        if (Cbl_college1.Items[i].Selected == true)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "CollegeCode='" + Cbl_college1.Items[i].Value + "'";
                            dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {


                                dtrow = dtl.NewRow();
                                dtl.Rows.Add(dtrow);


                                dtl.Rows[dtl.Rows.Count - 1][0] = Convert.ToString(Cbl_college.Items[i].Text);

                                int sno = 1;
                                for (int ik = 0; ik < dv.Count; ik++)
                                {


                                    dtrow = dtl.NewRow();
                                    dtl.Rows.Add(dtrow);



                                    dtl.Rows[dtl.Rows.Count - 1][0] = Convert.ToString(sno++);
                                    tagval = "";




                                    dtl.Rows[dtl.Rows.Count - 1][1] = dv[ik]["MarkType"].ToString();
                                    tagval += "^" + dv[ik]["MarkMasterPK"].ToString();


                                    dtl.Rows[dtl.Rows.Count - 1][2] = dv[ik]["MarkTypeAcr"].ToString();
                                    tagval += "^";


                                    dtl.Rows[dtl.Rows.Count - 1][3] = dv[ik]["Point"].ToString();
                                    tagval += "^" + dv[ik]["CollegeCode"].ToString();



                                    dtl.Rows[dtl.Rows.Count - 1][4] = dv[ik]["NO_Of_Stars"].ToString();
                                    tagval += "^" + dv[ik]["is_remark"].ToString();

                                    dtl.Rows[dtl.Rows.Count - 1][5] = tagval;

                                }
                            }
                        }
                    }


                    txt_search.Text = "";

                    if (dtl.Rows.Count > 1)
                    {
                        gridview1.DataSource = dtl;
                        gridview1.DataBind();
                        gridview1.Visible = true;




                        for (int i = 0; i < gridview1.Rows.Count; i++)
                        {
                            gridview1.Rows[i].Cells[0].Width = 60;

                            gridview1.Rows[i].Cells[1].Width = 200;

                            gridview1.Rows[i].Cells[2].Width = 200;

                            gridview1.Rows[i].Cells[3].Width = 100;

                            gridview1.Rows[i].Cells[4].Width = 100;



                            for (int j = 0; j < gridview1.HeaderRow.Cells.Count; j++)
                            {

                                if (j == 0 || j == 3 || j == 4)
                                {
                                    gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;


                                }
                                else if (j == 5)
                                {
                                    gridview1.Rows[i].Cells[j].Visible = false;
                                }
                                gridview1.Rows[i].Cells[j].Font.Name = "Book Antiqua";
                                gridview1.Rows[i].Cells[j].Font.Size = FontUnit.Medium;

                                if (i == 0)
                                {
                                    gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                    gridview1.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                    gridview1.Rows[i].Cells[j].BorderColor = Color.Black;
                                    gridview1.Rows[i].Cells[j].Font.Bold = true;
                                    gridview1.Rows[i].Cells[j].ForeColor = Color.White;

                                }
                                else
                                {
                                    int colspan = 1;
                                    if (j == 0)
                                    {

                                        while (gridview1.Rows[i].Cells[j].Text != "&nbsp;" && gridview1.Rows[i].Cells[j + colspan].Text == "&nbsp;")
                                        {
                                            colspan++;
                                            if (gridview1.HeaderRow.Cells.Count - 1 == j + colspan)
                                                break;

                                        }
                                    }

                                    if (colspan != 1)
                                    {
                                        gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                        gridview1.Rows[i].Cells[j].ColumnSpan = colspan;
                                        for (int a = j + 1; a < j + colspan; a++)
                                            gridview1.Rows[i].Cells[a].Visible = false;
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    imgdiv3.Visible = true;
                    lbl_alert.Text = "No Records Found";
                    gridview1.Visible = false;
                    div1.Visible = false;
                    rptprint1.Visible = false;
                }

            }
            else
            {
                lbl_alert.Text = "No Records Found";
                imgdiv3.Visible = true;
                div1.Visible = false;
                rptprint1.Visible = false;
            }
        }
        catch (Exception ex)
        {

            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            rptprint1.Visible = true;
            div1.Visible = true;
            mark();
        }
        catch (Exception ex)
        {

            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }

    protected void btn_AddNew_Click(object sender, EventArgs e)
    {
        try
        {
            is_remark.Checked = false;
            lbl_noofstars_limit.Text = "";
            lbl_point_limit.Text = "";
            btn_Save.Text = "Save";
            btndel.Visible = false;
            Addmark.Visible = true;
            Txt_college.Enabled = true;
            txt_type2.Text = "";
            txt_abtn.Text = "";
            txt_point.Text = "";
            txt_noofstars.Text = "";
            bindclg();
        }
        catch (Exception ex)
        {

            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }

    }

    public void cb_Type_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            Txt_Type.Text = "--Select--";
            if (cb_Type.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_Type.Items.Count; i++)
                {
                    cbl_Type.Items[i].Selected = true;
                }
                Txt_Type.Text = "Type(" + (cbl_Type.Items.Count) + ")";


            }
            else
            {
                for (int i = 0; i < cbl_Type.Items.Count; i++)
                {
                    cbl_Type.Items[i].Selected = false;
                }
                Txt_Type.Text = "--Select--";
            }


        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }

    }
    public void cbl_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string buildvalue = "";
            string build = "";
            cb_Type.Checked = false;
            Txt_Type.Text = "--Select--";


            for (int i = 0; i < cbl_Type.Items.Count; i++)
            {
                if (cbl_Type.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    //cb_Type.Checked = false;
                    build = cbl_Type.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }

                }

            }


            if (commcount > 0)
            {
                Txt_Type.Text = "Type(" + commcount.ToString() + ")";
                if (commcount == cbl_Type.Items.Count)
                {
                    cb_Type.Checked = true;
                }
                Txt_Type.Text = "Type(" + commcount.ToString() + ")";
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }
    public void BindType()
    {
        try
        {
            string college = "";
            for (int row = 0; row < Cbl_college1.Items.Count; row++)
            {
                if (Cbl_college1.Items[row].Selected == true)
                {
                    if (college == "")
                    {
                        college = Cbl_college1.Items[row].Value;
                    }
                    else
                    {
                        college = college + "," + Cbl_college1.Items[row].Value;
                    }
                }
            }

            Txt_Type.Text = "--Select--";
            cbl_Type.Items.Clear();
            cb_Type.Checked = false;
            if (college.Trim() != "")
            {

                string selqry = " SELECT  distinct (MarkType) FROM CO_MarkMaster WHERE  CollegeCode in (" + college + ")";
                ds = d2.select_method_wo_parameter(selqry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_Type.DataSource = ds;
                    cbl_Type.DataTextField = "MarkType";
                    cbl_Type.DataValueField = "MarkType";
                    cbl_Type.DataBind();
                }
                if (cbl_Type.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_Type.Items.Count; row++)
                    {
                        cbl_Type.Items[row].Selected = true;
                        cb_Type.Checked = true;
                    }
                    Txt_Type.Text = "Type(" + cbl_Type.Items.Count + ")";

                }

                else
                {

                    Txt_Type.Text = "--Select--";
                }
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }

    }



    protected void btn_errorclose1_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = Convert.ToString(Cbl_college.Items[i].Value);
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            if (college_cd != "")
            {
                string abbreviation = txt_abtn.Text.ToUpper();

                string type = txt_type2.Text;
                string point = txt_point.Text;
                string stars = txt_noofstars.Text;
                if (stars != "")
                {
                    type = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(type);
                    abbreviation = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(abbreviation);
                    int isremark = 0;
                    if (is_remark.Checked)
                        isremark = 1;
                    if (btn_Save.Text.Trim().ToLower() == "save")
                    {
                        for (int a = 0; a < Cbl_college.Items.Count; a++)
                        {
                            if (Cbl_college.Items[a].Selected == true)
                            {
                                string insert2 = "insert into CO_MarkMaster (MarkTypeAcr, MarkType , Point, CollegeCode,No_Of_Stars,is_remark) values ('" + abbreviation + "','" + type + "','" + point + "','" + Cbl_college.Items[a].Value + "','" + stars + "','" + isremark + "')";
                                int insertvalue2 = d2.update_method_wo_parameter(insert2, "Text");
                            }
                        }

                        imgdiv3.Visible = true;
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Saved Successfully";

                    }
                    else if (btn_Save.Text.Trim().ToLower() == "update")
                    { // update CO_MarkMaster set MarkType='type',MarkTypeAcr='abbreviation',Point='point' where MarkMasterpk=''"+value+"'' and collegecode='" + collegecode1 + "'

                        string activerow = gridview1_selectedrow.ToString();
                        string activecol = gridview1_selectedcol.ToString();




                        string[] tagvalues = gridview1.Rows[Convert.ToInt32(activerow)].Cells[gridview1.HeaderRow.Cells.Count - 1].Text.Split('^');
                        int value = Convert.ToInt32(tagvalues[1]);


                        int college = Convert.ToInt32(tagvalues[3]);


                        string sql = " update CO_MarkMaster set MarkType='" + type + "',MarkTypeAcr='" + abbreviation + "',Point='" + point + "',No_Of_Stars='" + stars + "',is_remark='" + isremark + "' where MarkMasterpk='" + value + "' and collegecode='" + college_cd + "'";
                        int insertvalue2 = d2.update_method_wo_parameter(sql, "Text");

                        imgdiv3.Visible = true;
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Updated Successfully";
                        Addmark.Visible = false;
                    }


                    else
                    {
                        imgdiv3.Visible = true;
                        lbl_alert.Text = "Please Enter value";
                        lbl_alert.ForeColor = Color.Red;

                    }

                    BindType();
                    mark();
                    txt_type2.Text = "";
                    txt_abtn.Text = "";
                    txt_point.Text = "";
                    txt_noofstars.Text = "";
                    is_remark.Checked = false;
                    btnSearch_Click(sender, e);
                    bindclg();
                }

                // update CO_MarkMaster set MarkType='',MarkTypeAcr='',Point='' where MarkMasterpk='' and collegecode=''
            }
            else
            {
                imgdiv3.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select College";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        Addmark.Visible = false;
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }
    protected void gridview1OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex != 0)
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
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }
    protected void gridview1_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            int cnt = gridview1.HeaderRow.Cells.Count;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

            gridview1_selectedrow = rowIndex;
            gridview1_selectedcol = selectedCellIndex;

            if (rowIndex != -1 && selectedCellIndex != -1)
            {



                string activerow = rowIndex.ToString();
                string activecol = selectedCellIndex.ToString();

                string[] tagvalues = gridview1.Rows[Convert.ToInt32(activerow)].Cells[gridview1.HeaderRow.Cells.Count - 1].Text.Split('^');
                string type = gridview1.Rows[Convert.ToInt32(activerow)].Cells[1].Text.ToString();

                if (type.Trim() != "")
                {
                    string Abbreviation = gridview1.Rows[Convert.ToInt32(activerow)].Cells[2].Text.ToString();
                    string Points = gridview1.Rows[Convert.ToInt32(activerow)].Cells[3].Text.ToString();
                    string stars = gridview1.Rows[Convert.ToInt32(activerow)].Cells[4].Text.ToString();
                    if (tagvalues[0] != "&nbsp;")
                    {
                        string value = tagvalues[1];
                        string college = tagvalues[3];
                        string is_remark2 = tagvalues[4];


                        if (is_remark2 == "1")
                            is_remark.Checked = true;
                        else
                            is_remark.Checked = false;
                        txt_type2.Text = type;
                        txt_abtn.Text = Abbreviation;
                        txt_point.Text = Points;
                        txt_noofstars.Text = stars;
                        Txt_college.Enabled = false;
                        int count2 = 0;
                        for (int i = 0; i < Cbl_college.Items.Count; i++)
                        {
                            Cbl_college.Items[i].Selected = false;
                        }
                        for (int i = 0; i < Cbl_college.Items.Count; i++)
                        {
                            if (Cbl_college.Items[i].Value.ToString() == college)
                            {
                                Cbl_college.Items[i].Selected = true;
                                Cb_college.Checked = false;
                                count2 = count2 + 1;
                            }
                            Txt_college.Text = "College(" + count2.ToString() + ")";
                        }
                        Cbl_college_SelectedIndexChanged(sender, e);
                        btn_Save.Text = "Update";
                        btndel.Visible = true;
                        Addmark.Visible = true;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
            
    }



    public void btndel_Click(object sender, EventArgs e)
    {
        try
        {
            img4.Visible = true;
            lbl_warning_alert.Visible = true;
            lbl_warning_alert.Text = "Are You Sure You Want Delete?";

            //int savecc = 0;
            //FpSpread1.SaveChanges();
            //string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            //string sql = "delete  from CO_MarkMaster where  MarkMasterpk = '" + FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString() + "' and collegecode='" + collegecode1 + "'";
            //int qry = d2.update_method_wo_parameter(sql, "Text");
            //savecc++;
            //if (savecc > 0)
            //{
            //    bindclg1();
            //    BindType();
            //    lbl_alert.Text = "Deleted Successfully";
            //    lbl_alert.Visible = true;
            //    imgdiv3.Visible = true;
            //    mark();
            //    txt_type2.Text = "";
            //    txt_abtn.Text = "";
            //    txt_point.Text = "";
            //}
            //Addmark.Visible = false;

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }

    public void btn_warningmsg_Click(object sender, EventArgs e)
    {
        try
        {
            img4.Visible = false;
            lbl_warning_alert.Text = "Are you sure you want delete";

            int savecc = 0;

            string activerow = gridview1_selectedrow.ToString();


            string[] tagvalues = gridview1.Rows[Convert.ToInt32(activerow)].Cells[gridview1.HeaderRow.Cells.Count - 1].Text.Split('^');
            string sql = "delete  from CO_MarkMaster where  MarkMasterpk = '" + tagvalues[1] + "' ";
            int qry = d2.update_method_wo_parameter(sql, "Text");
            savecc++;
            if (savecc > 0)
            {
                bindclg1();
                BindType();
                lbl_alert.Text = "Deleted Successfully";
                lbl_alert.Visible = true;
                imgdiv3.Visible = true;
                mark();
                txt_type2.Text = "";
                txt_abtn.Text = "";
                txt_point.Text = "";
                txt_noofstars.Text = "";
            }
            Addmark.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }

    }
    protected void btn_warning_exit_Click(object sender, EventArgs e)
    {
        img4.Visible = false;

    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }


    [WebMethod]
    public static string CheckUserName(string StoreName)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string user_name = StoreName;
            if (user_name.Trim() != "" && user_name != null)
            {
                string query = dd.GetFunction("select distinct MarkType,MarkMasterpk from CO_MarkMaster  where CollegeCode in('" + clgcode1 + "') and MarkType ='" + user_name + "'");
                if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                {
                    returnValue = "0";
                }

            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }


    [WebMethod]
    public static string CheckAbbreviation(string StoreName)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string user_name = StoreName;
            if (user_name.Trim() != "" && user_name != null)
            {
                string query = dd.GetFunction("select distinct MarkTypeAcr from CO_MarkMaster  where CollegeCode in('" + clgcode1 + "') and MarkTypeAcr ='" + user_name + "'");
                if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                {
                    returnValue = "0";
                }

            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Type(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "SELECT  distinct (MarkType), MarkMasterPK  FROM CO_MarkMaster where CollegeCode in ('" + clgcode + "') and MarkType like '" + prefixText + "%'";

        name = ws.Getname(query);

        return name;
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (gridview1.Visible == true)
                {
                    
                    d2.printexcelreportgrid(gridview1, reportname);

                }
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }
    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string dptname = "TypeMaster";
            string pagename = "Type_Master.aspx";

            if (gridview1.Visible == true)
            {
                
                string ss = null;
                string degreedetails = "";
                Printcontrol.loadspreaddetails(gridview1, pagename, degreedetails, 0, ss);
                Printcontrol.Visible = true;

            }
            
            
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }


    protected void txt_noofstars_textchange(object sender, EventArgs e)
    {
        try
        {


            string returnValue = "1";

            DAccess2 dd = new DAccess2();
            string user_name = txt_noofstars.Text;
            lbl_noofstars_limit.Text="";
            //string college = "";
            //for (int row = 0; row < Cbl_college.Items.Count; row++)
            //{
            //    if (Cbl_college.Items[row].Selected == true)
            //    {
            //        if (college == "")
            //        {
            //            college = Cbl_college.Items[row].Value;
            //        }
            //        else
            //        {
            //            college = college + "','" + Cbl_college.Items[row].Value;
            //        }
            //    }
            //}
            //clgcode1 = college;
            
                if (Convert.ToInt32(txt_noofstars.Text) <= 5)
                {
                    if (user_name.Trim() != "" && user_name != null)
                    {
                        string query = dd.GetFunction("select distinct No_Of_Stars from CO_MarkMaster  where CollegeCode in('" + clgcode1 + "') and No_Of_Stars ='" + user_name + "'");
                        if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                        {
                            returnValue = "0";
                        }

                    }
                    else
                    {
                        returnValue = "2";
                    }


                    switch (returnValue)
                    {
                        case "0":
                            lbl_noofstars_limit.Text = "Star value Does Not Exist";
                            lbl_noofstars_limit.ForeColor = Color.Green;

                            break;
                        case "1":
                            lbl_noofstars_limit.Text = "Star value Available";
                            lbl_noofstars_limit.ForeColor = Color.Red;
                            txt_noofstars.Text = "";
                            break;
                        case "2":
                            lbl_noofstars_limit.Text = "Please Enter Star value";
                            lbl_noofstars_limit.ForeColor = Color.Red;
                            txt_noofstars.Text = "";
                            break;
                        case "error":
                            lbl_noofstars_limit.Text = "Error Occurred";
                            lbl_noofstars_limit.ForeColor = Color.Red;
                            txt_noofstars.Text = "";
                            break;
                    }




                }
                else
                {
                    //lbl_noofstars_limit.Visible = true;
                    lbl_noofstars_limit.Text = "Enter the Maximum value of 5";
                    lbl_noofstars_limit.ForeColor = Color.Red;
                    txt_noofstars.Text = "";
                }
           

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }

    protected void txt_point_textchange(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(txt_point.Text) <= 100)
            {
                lbl_point_limit.Visible = false;

            }
            else
            {
                lbl_point_limit.Text = "Enter the Maximum value of 100";
                lbl_point_limit.Visible = true;
                txt_point.Text = "";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Options_Creation");
        }
    }


    protected void btn_exit_Click(object sender, EventArgs e)
    {

    }


    public override void VerifyRenderingInServerForm(Control control)
    { }
}