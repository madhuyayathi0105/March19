using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using System.Configuration;

public partial class seatingarrange : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DAccess2 dt = new DAccess2();
    Hashtable hat = new Hashtable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();

    string college_code = string.Empty;
    string norow = string.Empty;
    string nocol = string.Empty;
    string allotseat = string.Empty;
    string collegeCode = string.Empty;
    string[] arrang;
    string[] spcel;
    static int btngen = 0;
    int hss = 0;
    string rollNoStu = string.Empty;
    string regNum = string.Empty;
    bool isBundledNoWise = false;
    string strBlock = string.Empty;
    string examDate = string.Empty;
    DataTable dtBlock = new DataTable();

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
            lblmsg.Visible = false;
            college_code = Convert.ToString(Session["collegecode"]).Trim();
            if (!IsPostBack)
            {
                div1.Visible = false;
                divMainContents.Visible = false;
                chkForSeating.Checked = false;
                divBlock.Visible = false;
                lblBlock.Visible = false;
                Fspread3.Visible = false;
                pnlContents.Visible = false;
                Fspread3.Sheets[0].ColumnCount = 14;
                Fspread3.Sheets[0].RowHeader.Visible = false;
                Fspread3.Sheets[0].AutoPostBack = true;
                Fspread3.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                MyStyle.Font.Size = FontUnit.Medium;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.Font.Bold = true;
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Fspread3.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                Fspread3.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                Fspread3.Sheets[0].DefaultStyle.Font.Bold = false;
                Fspread3.CommandBar.Visible = false;
                Radioformat1.Checked = true;
                Chksetting.Checked = false;
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = Color.Black;
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColor = Color.Black;
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.Black;
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColor = Color.Black;
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColor = Color.Black;
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColor = Color.Black;
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 6].Border.BorderColor = Color.Black;
                Fspread3.RowHeader.Visible = false;
                Fspread3.CommandBar.Visible = false;
                Bindcollege();
                loadYear();
                loadmonth();
                mode();
                loaddatesession();
                hss = 0;
                txtCollege.Enabled = false;
                chkmergrecol.Checked = true;
                loadhall();
                divShowBundleNo.Visible = false;
                chkShowBundleNo.Checked = false;
                loadBlock();
                string sml = dt.GetFunction("select value from COE_Master_Settings where settings='Bundle Per Student'");
                if (sml.Trim() != "" && sml.Trim() != "0")
                {
                    isBundledNoWise = true;
                    divShowBundleNo.Visible = true;
                }
                else
                {
                    isBundledNoWise = false;
                    divShowBundleNo.Visible = false;
                }
            }
            lblmsg.Visible = false;
            btnView.Enabled = ExamSeatingArrangementLock();
        }
        catch (Exception ex) { }
    }

    public void loadBlock()
    {
        collegeCode = string.Empty;
        if (chkmergrecol.Checked)
        {
            collegeCode = Convert.ToString(Session["collegecode"]).Trim(); ;
        }
        else
        {
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
        }
        if (ddlDate.Items.Count > 0 && ddlSession.Items.Count > 0 && !string.IsNullOrEmpty(collegeCode))
        {
            if (ddlDate.SelectedItem.Text.ToLower().Trim() != "all" && ddlDate.SelectedItem.Text.ToLower().Trim() != "")
            {
                string edate = ddlDate.SelectedItem.Text.ToString().Trim();
                string[] spd = edate.Split('-');
                examDate = spd[1] + "/" + spd[0] + "/" + spd[2];
            }

            strBlock = "select distinct cs.block from exam_seating es,class_master cs where cs.rno=es.roomno and es.edate='" + examDate + "' and es.ses_sion='" + ddlSession.SelectedItem.ToString().Trim() + "' and cs.coll_code in(" + collegeCode + ") order by cs.block";
            dtBlock = dirAcc.selectDataTable(strBlock);
        }
        cblBlock.Items.Clear();
        chkBlock.Checked = false;
        txtBlock.Text = "--Select--";
        txtBlock.Enabled = false;
        if (dtBlock.Rows.Count > 0)
        {
            cblBlock.DataSource = dtBlock;
            cblBlock.DataTextField = "block";
            cblBlock.DataValueField = "block";
            cblBlock.DataBind();
            txtBlock.Enabled = true;
            foreach (wc.ListItem li in cblBlock.Items)
            {
                li.Selected = true;
            }
        }
        CallCheckboxListChange(chkBlock, cblBlock, txtBlock, lblBlock.Text, "--Select--");
        if (Radioformat2.Checked)
        {
            divBlock.Visible = chkIncludeBlock.Checked;
            lblBlock.Visible = chkIncludeBlock.Checked;
        }
    }

    public void loadYear()
    {
        try
        {
            ddlYear.Items.Clear();
            ds = dt.Examyear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void loadmonth()
    {
        try
        {
            ds.Clear();
            ddlMonth.Items.Clear();
            string year = ddlYear.SelectedValue;
            ds = dt.Exammonth(year);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthname";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void mode()
    {
        try
        {
            collegeCode = string.Empty;
            ddltype.Items.Clear();
            if (chkmergrecol.Checked)
            {
                collegeCode = Convert.ToString(Session["collegecode"]).Trim(); ;
            }
            else
            {
                if (cblCollege.Items.Count > 0)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                string mode = "select distinct type from course where college_code in (" + collegeCode + ") and type is not null and type<>''";
                ds = dt.select_method_wo_parameter(mode, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataValueField = "type";
                ddltype.DataBind();
            }
            else
            {
                ddltype.Items.Insert(0, "");
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void hallbind()
    {
        try
        {
            divMainContents.Visible = false;
            bool serialwise = false;
            bool QpaperWise = false;
            string columnfield = string.Empty;
            string group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string SerialNoorder = dirAcc.selectScalarString("select value from Master_Settings where settings='Seating Arrangement Serial no order' " + columnfield + "");
            string QpaperOrder = dirAcc.selectScalarString("select value from Master_Settings where settings='Seating Arrangement Qpaper order' " + columnfield + "");
            if (SerialNoorder == "1" || SerialNoorder.ToLower() == "true")
            {
                serialwise = true;
            }
            if (QpaperOrder == "1" || SerialNoorder.ToLower() == "true")
            {
                QpaperWise = true;
            }
            if (ddlhall.SelectedIndex == 0)
            {
                //lblmsg.Visible = true;
                //lblmsg.Text = "Please Select Hall No";
            }
            string rl = string.Empty;
            int p = 0;
            int l1 = 0;
            int t = 0;
            string nrow = string.Empty;
            int vl = 0;
            int v = 0;
            int flag = 0;
            string dat = string.Empty;
            string arrangeview1 = string.Empty;
            string arrangeviewNew = string.Empty;
            string allotSeat = string.Empty;
            int allotedSeats = 0;
            int allotedSeatsNew = 0;
            DataSet dsCollege = new DataSet();
            collegeCode = string.Empty;
            if (chkmergrecol.Checked)
            {
                collegeCode = Convert.ToString(Session["collegecode"]).Trim(); ;
            }
            else
            {
                if (cblCollege.Items.Count > 0)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                string qrynew = "select *,district+' - '+pincode  as districtpin from collinfo where college_code in (" + collegeCode + ")";
                dsCollege = dt.select_method_wo_parameter(qrynew, "Text");
            }
            Fpspread.Sheets[0].AutoPostBack = true;
            Fpspread.Sheets[0].RowHeader.Visible = false;
            Fpspread.Sheets[0].ColumnHeader.RowCount = 2;
            Fpspread.Sheets[0].ColumnHeader.Visible = false;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Fpspread.CommandBar.Visible = false;
            if (ddlhall.Items.Count > 0)
            {
                string strmode = string.Empty;
                if (chkmergrecol.Checked == false)//Modifed by Srinath 25 Oct 2016
                {
                    if (ddltype.Items.Count > 0)
                    {
                        if (ddltype.SelectedItem.Text.Trim().ToLower() != "all")
                        {
                            if (ddltype.SelectedItem.Text != "")
                            {
                                strmode = " and Mode='" + ddltype.SelectedItem.Text.Trim() + "'";
                            }
                        }
                    }
                }
                else
                {
                    if (collegeCode.Contains(","))
                    {
                        strmode = string.Empty;
                    }
                }
                if (ddlhall.Items.Count > 0)
                {
                    if (ddlhall.SelectedItem.Text != "")
                    {
                        // rl = "select * from tbl_room_seats  where Hall_No='" + ddlhall.SelectedItem.Text + "' and exm_month='" + ddlMonth.SelectedValue + "' and exm_year='" + ddlYear.SelectedItem.Text + "' " + strmode + "";
                        rl = "select * from tbl_room_seats where Hall_No='" + ddlhall.SelectedItem.Text + "' " + strmode + "";
                    }
                    ds = dt.select_method_wo_parameter(rl, "text");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    flag = 1;
                    nrow = Convert.ToString(ds.Tables[0].Rows[0]["no_of_rows"]).Trim();
                    arrangeview1 = Convert.ToString(ds.Tables[0].Rows[0]["arranged_view"]).Trim();
                    arrangeviewNew = Convert.ToString(ds.Tables[0].Rows[0]["arrangedViewNew"]).Trim();
                    allotseat = Convert.ToString(ds.Tables[0].Rows[0]["allocted_seats"]).Trim();
                    allotSeat = Convert.ToString(ds.Tables[0].Rows[0]["allotedSeatsNew"]).Trim();
                    int.TryParse(allotseat, out allotedSeats);
                    int.TryParse(allotSeat, out allotedSeatsNew);
                    Fpspread.Sheets[0].RowCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["no_of_rows"]).Trim());
                }
                Fpspread.Sheets[0].ColumnCount = 0;
                if (ddlDate.Items.Count > 0)
                {
                    if (ddlDate.SelectedItem.Text.Trim().ToLower() != "all")
                    {
                        dat = ddlDate.SelectedItem.Text;
                        string[] datt = dat.Split('-');
                        dat = datt[2].ToString() + "-" + datt[1].ToString() + "-" + datt[0].ToString();
                    }
                    else if (ddlDate.SelectedItem.Text.Trim().ToLower() == "all")
                    {
                        dat = ddlDate.Items[1].Text;
                        string[] datt = dat.Split('-');
                        dat = datt[2].ToString() + "-" + datt[1].ToString() + "-" + datt[0].ToString();
                    }
                }
                string sql = "select * from exam_seating es,subject s where es.subject_no=s.subject_no and es.roomno='" + ddlhall.SelectedItem.Text + "' and  es.edate='" + dat + "' and  es.ses_sion='" + ddlSession.SelectedItem.Text + "' order by seat_no";
                ds1 = dt.select_method_wo_parameter(sql, "text");
                if (flag == 1)
                {
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        Fspread3.Visible = false;
                        pnlContents.Visible = false;
                        Exportexcel.Visible = false;
                        Printfspread3.Visible = false;
                        btn_directprint.Visible = false;
                        Fpspread.Visible = true;
                        pnlContent1.Visible = true;
                        txtexcelname.Visible = true;
                        txtexcelname.Text = string.Empty;
                        btnxl.Visible = true;
                        btnprintmaster.Visible = true;
                        btnDirectPrint.Visible = true;
                        lblrptname.Visible = true;
                        if (allotedSeats < ds1.Tables[0].Rows.Count)
                        {
                            arrang = arrangeviewNew.Split(';');
                        }
                        else //if (allotedSeatsNew <= ds1.Tables[0].Rows.Count)
                        {
                            arrang = arrangeview1.Split(';');
                        }
                        Dictionary<int, int> dicsubcol = new Dictionary<int, int>();
                        Dictionary<string, int> dicsubcolcount = new Dictionary<string, int>();
                        if (dsCollege.Tables.Count > 0 && dsCollege.Tables[0].Rows.Count > 0)
                        {
                            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(Convert.ToString(ddlMonth.SelectedItem.Value).Trim()));
                            string[] strpa = Convert.ToString(dsCollege.Tables[0].Rows[0]["affliatedby"]).Trim().Split(',');
                            spF1College.InnerText = Convert.ToString(dsCollege.Tables[0].Rows[0]["Collname"]).Trim();
                            spF1Controller.InnerText = "OFFICE OF THE CONTROLLER OF EXAMINATIONS";
                            spF1Seating.InnerText = "SEATING ARRANGEMENT";
                            spF1Aff.InnerText = (strpa.Length > 0) ? Convert.ToString(strpa[0]).Trim() : Convert.ToString(dsCollege.Tables[0].Rows[0]["affliatedby"]).Trim();
                            spF1Date.InnerText = "Date & Session : " + Convert.ToString(ddlDate.SelectedItem.Text).Trim() + " & " + Convert.ToString(ddlSession.SelectedItem.Text).Trim();
                            spExamination.InnerText = "Examination - " + strMonthName.ToUpper() + " " + Convert.ToString(ddlYear.SelectedItem.Text);
                            spHallNo.InnerText = "Hall No : " + Convert.ToString(ddlhall.SelectedItem.Text).Trim();
                        }
                        for (int spr = 0; spr <= arrang.GetUpperBound(0); spr++)
                        {
                            string colsp = arrang[spr].ToString();
                            if (colsp.Trim() != "" && colsp != null)
                            {
                                spcel = colsp.Split('-');
                                for (int spc = 0; spc <= spcel.GetUpperBound(0); spc++)
                                {
                                    int colsn = Convert.ToInt32(spcel[spc]);
                                    string strrow = "C" + spc + "R" + spr;
                                    if (!dicsubcolcount.ContainsKey(strrow))
                                    {
                                        dicsubcolcount.Add(strrow, colsn);
                                    }
                                    if (dicsubcol.ContainsKey(spc))
                                    {
                                        int valc = dicsubcol[spc];
                                        if (valc < colsn)
                                        {
                                            dicsubcol[spc] = colsn;
                                        }
                                    }
                                    else
                                    {
                                        dicsubcol.Add(spc, colsn);
                                    }
                                }
                            }
                        }
                        int count = 0;
                        int add = 0;
                        int getcouv = 0;
                        int col_cnt = 0;

                        ArrayList addarr = new ArrayList();
                        //TableRow trRow1 = new TableRow();
                        TableCell tcell = new TableCell();
                        int autoChar = 97;
                        for (int h1 = 0; h1 < dicsubcol.Count; h1++)
                        {
                            t++;
                            int sucol = dicsubcol[h1];
                            for (int l = sucol - 1; l < sucol; l++)
                            {
                                l1++;
                                Fpspread.Sheets[0].Columns.Count = Fpspread.Sheets[0].Columns.Count + sucol;

                                //=========================================================
                                col_cnt = Convert.ToInt16(Fpspread.Sheets[0].Columns.Count);
                                bool even = false;


                                if (h1 == dicsubcol.Count - 1)
                                {
                                    even = true;
                                    if (col_cnt % 2 == 0)
                                    {
                                        even = false;
                                    }
                                }

                                bool colflag = false;
                                bool resetflag = false;
                                //==========================================================

                                //tblHeader1.Cells.Count = Fpspread.Sheets[0].Columns.Count + sucol;
                                //tblHeader2.Cells.Count = Fpspread.Sheets[0].Columns.Count + sucol;
                                TableCell tcellnew = new TableCell();
                                addarr.Clear();

                                for (int j = 0; j < Convert.ToInt32(nrow); j++)
                                {
                                    vl++;
                                    string strrow = "C" + h1 + "R" + j;
                                    string seatValue = string.Empty;
                                    if (dicsubcolcount.ContainsKey(strrow))
                                    {
                                        getcouv = dicsubcolcount[strrow];
                                        count = add;
                                        addarr.Add(getcouv);
                                        for (int g = 0; g < getcouv; g++)
                                        {
                                            hss++;
                                            //seatValue = Convert.ToString("");
                                            seatValue = Convert.ToString((j + 1) + (Convert.ToInt32(nrow) * g)) + Convert.ToString((char)autoChar);
                                            tcellnew = new TableCell();
                                            tcellnew.Width = 86;

                                            //========================================
                                            //if (even == true)
                                            //{
                                            if (serialwise == true)
                                            {
                                                if (colflag == false && resetflag == false)
                                                {
                                                    tcellnew.Text = Convert.ToString(col_cnt - 1);
                                                    colflag = true;
                                                }
                                                else if (colflag == true && resetflag == false)
                                                {
                                                    tcellnew.Text = Convert.ToString(col_cnt);
                                                    resetflag = true;
                                                }
                                                if (even == true)
                                                {
                                                    tcellnew.Text = Convert.ToString(col_cnt);
                                                }
                                            }
                                            else
                                            {
                                                tcellnew.Text = Convert.ToString(g + 1);
                                            }
                                            //if (even == false)
                                            //{
                                            //    tcellnew.Text = Convert.ToString(col_cnt);
                                            //}

                                            //}
                                            //else
                                            //{
                                            //    tcellnew.Text = Convert.ToString(col_cnt);
                                            //}

                                            //========================================


                                            //tcellnew.BorderWidth = 1;
                                            //tcellnew.BorderColor = Color.Black;
                                            if (Fpspread.Sheets[0].Columns.Count - 1 >= tblHeader2.Cells.Count || even == false)
                                            {
                                                if (tblHeader2.Cells.Count == count)
                                                    tblHeader2.Cells.AddAt(count, tcellnew);
                                                else
                                                    tblHeader2.Cells.Add(tcellnew);

                                                even = true;
                                            }

                                            Fpspread.Sheets[0].ColumnHeader.Cells[1, count].Text = Convert.ToString(g + 1);
                                            Fpspread.Sheets[0].ColumnHeader.Cells[1, count].Font.Bold = true;
                                            Fpspread.Sheets[0].ColumnHeader.Cells[1, count].Font.Size = FontUnit.Medium;
                                            Fpspread.Sheets[0].ColumnHeader.Cells[1, count].Font.Name = "Book Antiqua";
                                            Fpspread.Sheets[0].ColumnHeader.Cells[1, count].HorizontalAlign = HorizontalAlign.Center;
                                            //if (p < Convert.ToInt32(allotseat))
                                            //{
                                            //    if (p < ds1.Tables[0].Rows.Count)
                                            //    {
                                            //        if (ds1.Tables[0].Rows[p]["seatCode"].ToString().Trim() == Convert.ToString(seatValue))
                                            //        {
                                            //            Fpspread.Sheets[0].Cells[j, count].Text = ds1.Tables[0].Rows[p]["regno"].ToString() + "  -[" + ds1.Tables[0].Rows[p]["seat_no"].ToString() + "]- " + ds1.Tables[0].Rows[p]["subject_code"].ToString();
                                            //            //Fpspread.Sheets[0].Cells[j, count].Text = "[" + ds1.Tables[0].Rows[p]["seat_no"].ToString() + "] " + ds1.Tables[0].Rows[p]["regno"].ToString();
                                            //            Fpspread.Sheets[0].Cells[j, count].Font.Bold = true;
                                            //            Fpspread.Sheets[0].Cells[j, count].Font.Size = FontUnit.Medium;
                                            //            Fpspread.Sheets[0].Cells[j, count].Font.Name = "Book Antiqua";
                                            //            Fpspread.Sheets[0].Cells[j, count].HorizontalAlign = HorizontalAlign.Center;
                                            //            Fpspread.Sheets[0].Cells[j, count].VerticalAlign = VerticalAlign.Middle;
                                            //            p++;
                                            //        }
                                            //        else
                                            //        {
                                            //            Fpspread.Sheets[0].Cells[j, count].Text = "[" + hss + "]";
                                            //            Fpspread.Sheets[0].Cells[j, count].Font.Bold = true;
                                            //            Fpspread.Sheets[0].Cells[j, count].Font.Size = FontUnit.Medium;
                                            //            Fpspread.Sheets[0].Cells[j, count].Font.Name = "Book Antiqua";
                                            //            Fpspread.Sheets[0].Cells[j, count].HorizontalAlign = HorizontalAlign.Left;
                                            //        }
                                            //    }
                                            //}
                                            DataView dvStudent = new DataView();
                                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                                            {
                                                //ds1.Tables[0].DefaultView.RowFilter = " seatCode='" + seatValue + "'";// ((chkNewSeating.Checked) ? " seatCode='" + seatValue + "'" : " seat_no='" + hss + "' ");
                                                ds1.Tables[0].DefaultView.RowFilter = " seat_no='" + hss + "'";
                                                dvStudent = ds1.Tables[0].DefaultView;
                                            }
                                            if (dvStudent.Count > 0)
                                            {
                                                Fpspread.Sheets[0].Cells[j, count].Text = dvStudent[0]["regno"].ToString() + "  -[" + hss + "]- " + dvStudent[0]["subject_code"].ToString();
                                                //Fpspread.Sheets[0].Cells[j, count].Text = "[" + ds1.Tables[0].Rows[p]["seat_no"].ToString() + "] " + ds1.Tables[0].Rows[p]["regno"].ToString();
                                                Fpspread.Sheets[0].Cells[j, count].Font.Bold = true;
                                                Fpspread.Sheets[0].Cells[j, count].Font.Size = FontUnit.Medium;
                                                Fpspread.Sheets[0].Cells[j, count].Font.Name = "Book Antiqua";
                                                Fpspread.Sheets[0].Cells[j, count].HorizontalAlign = HorizontalAlign.Center;
                                                Fpspread.Sheets[0].Cells[j, count].VerticalAlign = VerticalAlign.Middle;
                                            }
                                            else
                                            {
                                                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                                                {
                                                    ds1.Tables[0].DefaultView.RowFilter = " seat_no='" + hss + "'";// ((chkNewSeating.Checked) ? " seatCode='" + seatValue + "'" : " seat_no='" + hss + "' ");
                                                    dvStudent = ds1.Tables[0].DefaultView;
                                                }
                                                if (dvStudent.Count > 0)
                                                {
                                                    Fpspread.Sheets[0].Cells[j, count].Text = dvStudent[0]["regno"].ToString() + "  -[" + hss + "]- " + dvStudent[0]["subject_code"].ToString();
                                                    //Fpspread.Sheets[0].Cells[j, count].Text = "[" + ds1.Tables[0].Rows[p]["seat_no"].ToString() + "] " + ds1.Tables[0].Rows[p]["regno"].ToString();
                                                    Fpspread.Sheets[0].Cells[j, count].Font.Bold = true;
                                                    Fpspread.Sheets[0].Cells[j, count].Font.Size = FontUnit.Medium;
                                                    Fpspread.Sheets[0].Cells[j, count].Font.Name = "Book Antiqua";
                                                    Fpspread.Sheets[0].Cells[j, count].HorizontalAlign = HorizontalAlign.Center;
                                                    Fpspread.Sheets[0].Cells[j, count].VerticalAlign = VerticalAlign.Middle;
                                                }
                                                else
                                                {
                                                    Fpspread.Sheets[0].Cells[j, count].Text = "[" + hss + "]";
                                                    Fpspread.Sheets[0].Cells[j, count].Font.Bold = true;
                                                    Fpspread.Sheets[0].Cells[j, count].Font.Size = FontUnit.Medium;
                                                    Fpspread.Sheets[0].Cells[j, count].Font.Name = "Book Antiqua";
                                                    Fpspread.Sheets[0].Cells[j, count].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                            }
                                            count++;
                                        }
                                        //if (getcouv < sucol)
                                        //{
                                        //    for (int y = getcouv; y < sucol; y++)
                                        //    {
                                        //        if (p < Convert.ToInt32(allotseat))
                                        //        {
                                        //            if (p < ds1.Tables[0].Rows.Count)
                                        //            {
                                        //                Fpspread.Sheets[0].Cells[y, count].Text = ds1.Tables[0].Rows[p]["regno"].ToString() + "[" + ds1.Tables[0].Rows[p]["seat_no"].ToString() + "]";
                                        //                Fpspread.Sheets[0].Cells[y, count].Font.Bold = true;
                                        //                Fpspread.Sheets[0].Cells[y, count].Font.Size = FontUnit.Medium;
                                        //                Fpspread.Sheets[0].Cells[y, count].Font.Name = "Book Antiqua";
                                        //                Fpspread.Sheets[0].Cells[y, count].HorizontalAlign = HorizontalAlign.Left;
                                        //                p++;
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                    }
                                }
                                if (addarr.Count > 0)
                                {
                                    addarr.Sort();
                                }
                                add = add + Convert.ToInt32(addarr[addarr.Count - 1]);
                                //h++;
                                if (v < Fpspread.Sheets[0].ColumnCount)
                                {
                                    tcellnew = new TableCell();
                                    //tcellnew.Width = 90;                                   
                                    tcellnew.Text = "Column" + (t);
                                    tcellnew.BorderWidth = 0;
                                    tcellnew.ColumnSpan = sucol;
                                    tblHeader1.Cells.Add(tcellnew);
                                    Fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, v, 1, sucol);
                                    Fpspread.Sheets[0].ColumnHeader.Cells[0, v].Text = "Column" + (t);
                                    Fpspread.Sheets[0].ColumnHeader.Cells[0, v].Font.Bold = true;
                                    Fpspread.Sheets[0].ColumnHeader.Cells[0, v].Font.Size = FontUnit.Medium;
                                    Fpspread.Sheets[0].ColumnHeader.Cells[0, v].Font.Name = "Book Antiqua";
                                    Fpspread.Sheets[0].ColumnHeader.Cells[0, v].HorizontalAlign = HorizontalAlign.Center;
                                    v = v + sucol;
                                }
                            }
                            autoChar++;
                        }
                        //tblFormat1.Width = tblHeader1.Cells.Count * 80;
                    }
                    else
                    {
                        lblmsg.Text = "No Records Found";
                        lblmsg.Visible = true;
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        btnDirectPrint.Visible = false;
                    }
                    int totalcount = 0;
                    string sal = string.Empty;
                    if (QpaperWise == false)
                    {
                        sal = " select distinct s.subject_name,s.subject_code,COUNT(e.subject_no) as num from exam_seating e,subject s where e.subject_no=s.subject_no and e.roomno='" + ddlhall.SelectedItem.Text + "' and e.edate='" + dat + "' and ses_sion='" + ddlSession.SelectedItem.Text + "' group by s.subject_name,s.subject_code order by COUNT(e.subject_no) desc,s.subject_code";
                    }
                    else
                    {
                        sal = "select distinct ISNULL(d.QpaperType,'A') as Qpaper,sy.degree_code,d.Acronym,s.subject_no,s.subject_name,s.subject_code,COUNT(e.subject_no) as num   from exam_seating e,syllabus_master sy,subject s,Degree d where s.syll_code=sy.syll_code  and d.Degree_Code=sy.degree_code and e.subject_no=s.subject_no and e.roomno='" + ddlhall.SelectedItem.Text + "' and e.edate='" + dat + "' and ses_sion='" + ddlSession.SelectedItem.Text + "' group by s.subject_name,s.subject_code, s.subject_no,sy.degree_code,d.Acronym,ISNULL(d.QpaperType,'A') order by COUNT(e.subject_no) desc,s.subject_code,ISNULL(d.QpaperType,'A')";
                    }
                    ds = dt.select_method_wo_parameter(sal, "text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            //string sal1 = "    select * from exam_seating where roomno='" + ddlhall.SelectedItem.Text + "' and edate='" + dat + "' and subject_no='" + ds.Tables[0].Rows[j]["subject_no"].ToString() + "'";
                            //ds2 = dt.select_method_wo_parameter(sal1, "text");
                            //if (ds2.Tables[0].Rows.Count > 0)
                            //{
                            //    string ll = "select regno,e.seat_no,Course_Name,dept_acronym,Batch_Year from exam_seating e,Registration r,Degree d,Department dt,Course c where r.Reg_No =e.regno and e.degree_code = r.degree_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and roomno='" + ddlhall.SelectedItem.Text + "' and subject_no='" + ds.Tables[0].Rows[j]["subject_no"].ToString() + "' and edate='" + dat + "' and regno!='' order by e.seat_no";
                            //    ds1 = dt.select_method_wo_parameter(ll, "text");
                            //    if (ds1.Tables[0].Rows.Count > 0)
                            //    {
                            //        Fpspread.Sheets[0].RowCount++;
                            //        Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - 1, 0, 1, Fpspread.Sheets[0].Columns.Count - 1);
                            //        string stucount = ds2.Tables[0].Rows.Count.ToString();
                            //        if (stucount == "1")
                            //        {
                            //            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]) + " - " + Convert.ToString(ds1.Tables[0].Rows[0]["dept_acronym"]) + "  (" + ds1.Tables[0].Rows[0]["Batch_Year"] + ")  " + ds.Tables[0].Rows[j]["subject_name"].ToString() + " ( " + ds1.Tables[0].Rows[0]["regno"].ToString() + " [" + ds1.Tables[0].Rows[0]["seat_no"].ToString() + "] )";
                            //        }
                            //        else
                            //        {
                            //            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]) + " - " + Convert.ToString(ds1.Tables[0].Rows[0]["dept_acronym"]) + "  (" + ds1.Tables[0].Rows[0]["Batch_Year"] + ")  " + ds.Tables[0].Rows[j]["subject_name"].ToString() + " ( " + ds1.Tables[0].Rows[0]["regno"].ToString() + " [" + ds1.Tables[0].Rows[0]["seat_no"].ToString() + "] - " + ds1.Tables[0].Rows[ds1.Tables[0].Rows.Count - 1]["regno"].ToString() + " [" + ds1.Tables[0].Rows[ds1.Tables[0].Rows.Count - 1]["seat_no"].ToString() + "] )";
                            //        }
                            //        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            //        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            //        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                            //        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, Fpspread.Sheets[0].Columns.Count - 1].Text = Convert.ToString(" Count : " + ds2.Tables[0].Rows.Count + "");
                            //        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, Fpspread.Sheets[0].Columns.Count - 1].Font.Bold = true;
                            //        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, Fpspread.Sheets[0].Columns.Count - 1].Font.Size = FontUnit.Medium;
                            //        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, Fpspread.Sheets[0].Columns.Count - 1].Font.Name = "Book Antiqua";
                            //        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, Fpspread.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                            //        totalcount = totalcount + Convert.ToInt32(ds2.Tables[0].Rows.Count);
                            //    }
                            //}
                            Fpspread.Sheets[0].RowCount++;

                            Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - 1, 0, 1, Fpspread.Sheets[0].Columns.Count - 1);
                            if (QpaperWise == true)
                            {
                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[j]["subject_code"]) + " - " + Convert.ToString(ds.Tables[0].Rows[j]["subject_name"]) + "-" + Convert.ToString(ds.Tables[0].Rows[j]["Qpaper"]);
                            }
                            else
                            {
                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[j]["subject_code"]) + " - " + Convert.ToString(ds.Tables[0].Rows[j]["subject_name"]);
                            }
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, Fpspread.Sheets[0].Columns.Count - 1].Text = Convert.ToString(" Count : " + ds.Tables[0].Rows[j]["num"].ToString() + "");
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, Fpspread.Sheets[0].Columns.Count - 1].Font.Bold = true;
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, Fpspread.Sheets[0].Columns.Count - 1].Font.Size = FontUnit.Medium;
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, Fpspread.Sheets[0].Columns.Count - 1].Font.Name = "Book Antiqua";
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, Fpspread.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                            totalcount = totalcount + Convert.ToInt32(ds.Tables[0].Rows[j]["num"].ToString());
                        }
                        Fpspread.Sheets[0].RowCount++;
                        Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - 1, 0, 1, Fpspread.Sheets[0].Columns.Count);
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = "Total     " + Convert.ToString(totalcount) + "";
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    }
                    Fpspread.Sheets[0].PageSize = Fpspread.Sheets[0].RowCount;
                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "No Records Found";
                    Fpspread.Visible = false;
                    pnlContent1.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    btnDirectPrint.Visible = false;
                    lblrptname.Visible = false;
                    Fspread3.Visible = false;
                    pnlContents.Visible = false;
                    Exportexcel.Visible = false;
                    Printfspread3.Visible = false;
                    btn_directprint.Visible = false;
                }
            }
            else
            {
                lblmsg.Visible = true;
                lblmsg.Text = "No Seat Alloted For This Hall";
                Fpspread.Visible = false;
                pnlContent1.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                btnDirectPrint.Visible = false;
                lblrptname.Visible = false;
                Fspread3.Visible = false;
                pnlContents.Visible = false;
                Exportexcel.Visible = false;
                Printfspread3.Visible = false;
                btn_directprint.Visible = false;
            }
            if (Radioformat2.Checked == true)
            {
                lblmsg.Visible = false;
                Fpspread.Visible = false;
                pnlContent1.Visible = false;
                btnprintmaster.Visible = false;
                btnDirectPrint.Visible = false;
                Printcontrol.Visible = false;
                btnxl.Visible = false;
                txtexcelname.Text = string.Empty;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
            }
            if (Radioformat3.Checked == true)
            {
                Fpseating.Visible = false;
                Fpspread.Visible = false;
                pnlContent1.Visible = false;
                Fspread3.Visible = false;
                pnlContents.Visible = false;
                lblmsg.Visible = false;
                lblmessage1.Visible = false;
                lblrptname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                btnDirectPrint.Visible = false;
                Printcontrol.Visible = false;
                txtexcelname.Visible = false;
                Exportexcel.Visible = false;
                Printfspread3.Visible = false;
                btn_directprint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void loaddatesession()
    {
        try
        {
            ddlSession.Items.Clear();
            ddlDate.Items.Clear();
            ds.Clear();
            ds.Reset();
            int da11 = 0;


            if (ddlMonth.Items.Count > 0 && ddlYear.Items.Count > 0 && ddlMonth.SelectedIndex != -1)
            {
                int.TryParse(ddlMonth.SelectedValue.ToString(), out da11);
                da11 = da11 - 1;
                string s = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,et.exam_date from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' and DATEPART(month, et.exam_date)>='" + da11 + "' and DATEPART(YEAR, et.exam_date)>='" + ddlYear.SelectedItem.Text.ToString() + "' order by et.exam_date";
                ds.Clear();
                ds.Reset();
                ds = dt.select_method_wo_parameter(s, "txt");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlDate.Enabled = true;
                    ddlSession.Enabled = true;
                    ddlDate.Items.Clear();
                    ddlDate.DataSource = ds;
                    ddlDate.DataTextField = "ExamDate";
                    ddlDate.DataValueField = "ExamDate";
                    ddlDate.DataBind();
                    // ddlDate.Items.Insert(0, "All");
                }
                string s1 = "select distinct et.exam_session from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' and DATEPART(month, et.exam_date)>='" + da11 + "' and DATEPART(YEAR, et.exam_date)>='" + ddlYear.SelectedItem.Text.ToString() + "'";
                ds.Clear();
                ds.Reset();
                ds = dt.select_method_wo_parameter(s1, "txt");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlDate.Enabled = true;
                    ddlSession.Enabled = true;
                    ddlSession.Items.Clear();
                    ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                    ddlSession.DataSource = ds;
                    ddlSession.DataTextField = "exam_session";
                    ddlSession.DataValueField = "exam_session";
                    ddlSession.DataBind();
                }
                else
                {
                    ddlDate.Items.Clear();
                    ddlSession.Items.Clear();
                    ddlDate.Enabled = false;
                    ddlSession.Enabled = false;
                }
                ddlhall.Items.Clear();
            }
            if (ddlMonth.Items.Count > 0 && Convert.ToInt16(ddlMonth.SelectedValue) == -1)
            {
                btnView.Enabled = false;
            }
            else if (ddlDate.Enabled == false || ddlSession.Enabled == false)
            {
                btnView.Enabled = false;
            }
            else
            {
                if (ExamSeatingArrangementLock())
                    btnView.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void loadhall()
    {
        try
        {
            ddlhall.Items.Clear();
            if (ddlDate.Items.Count > 0 && ddlYear.Items.Count > 0 && ddlMonth.Items.Count > 0)
            {
                if (Convert.ToString(ddlDate.SelectedItem).Trim().ToLower() != "all" && Convert.ToString(ddlDate.SelectedItem).Trim().ToLower() != "")
                {
                    string[] spd = Convert.ToString(ddlDate.SelectedItem).Trim().Split('-');
                    string typequery = string.Empty;
                    if (ddltype.Items.Count > 0)
                    {
                        if (Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddltype.SelectedItem.Text).Trim().ToLower() != "")
                        {
                            if (ddltype.SelectedItem.Text != "")
                            {
                                typequery = " and c.type='" + Convert.ToString(ddltype.SelectedItem.Text).Trim() + "'";
                            }
                        }
                    }
                    if (cblCollege.Items.Count > 0)
                    {
                        collegeCode = getCblSelectedValue(cblCollege);
                    }
                    if (chkmergrecol.Checked == true)
                    {
                        typequery = string.Empty;
                        collegeCode = Convert.ToString(Session["collegecode"]).Trim();
                    }
                    else
                    {
                        if (collegeCode.Contains(","))
                        {
                            typequery = string.Empty;
                        }
                    }
                    //string hl = "select distinct es.roomno from exmtt e,exmtt_det et,exam_seating es,course c,Degree d where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id " + typequery + " and e.Exam_year='" + ddlYear.SelectedItem.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and es.edate='" + spd[1] + '/' + spd[0] + '/' + spd[2] + "' and es.ses_sion='" + ddlSession.SelectedItem.ToString() + "'";
                    string hl = " select distinct es.roomno,Cm.Priority from exmtt e,exmtt_det et,exam_seating es,course c,Degree d,class_master CM where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id  and Cm.rno=es.roomno ";
                    if (collegeCode.Contains(','))
                    {
                        hl = hl + " and d.college_code in (" + collegeCode + ")";
                    }
                    else
                    {
                        hl = hl + typequery;
                    }
                    hl = hl + " and e.Exam_year='" + ddlYear.SelectedItem.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and es.edate='" + spd[1] + '/' + spd[0] + '/' + spd[2] + "' and es.ses_sion='" + ddlSession.SelectedItem.ToString() + "' order by Cm.Priority";
                    ds = dt.select_method_wo_parameter(hl, "text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlhall.Enabled = true;
                        ddlhall.DataSource = ds;
                        ddlhall.DataTextField = "roomno";
                        ddlhall.DataValueField = "roomno";
                        ddlhall.DataBind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void clear()
    {
        divMainContents.Visible = false;
        chkForSeating.Checked = false;
        Fpspread.Visible = false;
        pnlContent1.Visible = false;
        btnprintmaster.Visible = false;
        btnDirectPrint.Visible = false;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Text = string.Empty;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        lblmsg.Visible = false;
        Fpseating.Visible = false;
        Print_seating.Visible = false;
        Excel_seating.Visible = false;
        Print_seating.Visible = false;
        Excel_seating.Visible = false;
        Fpseating.Visible = false;
        Fpspread.Visible = false;
        Fspread3.Visible = false;
        pnlContents.Visible = false;
        lblmsg.Visible = false;
        lblmessage1.Visible = false;
        lblrptname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        btnDirectPrint.Visible = false;
        Printcontrol.Visible = false;
        txtexcelname.Visible = false;
        Exportexcel.Visible = false;
        Printfspread3.Visible = false;
        btn_directprint.Visible = false;
        Print_seating.Visible = false;
        Excel_seating.Visible = false;
        Fpseating.Visible = false;
        lblmsg.Visible = false;
        Fspread3.Visible = false;
        Exportexcel.Visible = false;
        Printfspread3.Visible = false;
        btn_directprint.Visible = false;
        Fpspread.Visible = false;
        btnprintmaster.Visible = false;
        btnDirectPrint.Visible = false;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Text = string.Empty;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        if (ExamSeatingArrangementLock())
            btnView.Enabled = true;
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loaddatesession();
        loadhall();
        loadBlock();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadmonth();
        loaddatesession();
        loadhall();
        loadBlock();
    }

    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadhall();
        loadBlock();
    }

    protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadhall();
        loadBlock();
    }

    protected void ddlhall_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        btngo_Click(sender, e);
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadhall();
        loadBlock();
        btngo_Click(sender, e);
    }

    protected void Radioformat1_CheckedChanged(object sender, EventArgs e)
    {
        lblreportname2.Visible = false;
        txtreportname2.Visible = false;
        lblexcsea.Visible = false;
        txtexseat.Visible = false;
        ddlhall.Enabled = true;
        Fspread3.Visible = false;
        pnlContents.Visible = false;
        Exportexcel.Visible = false;
        Printfspread3.Visible = false;
        btn_directprint.Visible = false;
        Chksetting.Enabled = false;
        Fpspread.Visible = false;
        pnlContent1.Visible = false;
        Fspread3.Visible = false;
        pnlContents.Visible = false;
        lblmsg.Visible = false;
        lblmessage1.Visible = false;
        lblrptname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        btnDirectPrint.Visible = false;
        Printcontrol.Visible = false;
        txtexcelname.Visible = false;
        Exportexcel.Visible = false;
        Printfspread3.Visible = false;
        btn_directprint.Visible = false;
        Print_seating.Visible = false;
        Excel_seating.Visible = false;
        Fpseating.Visible = false;
        Chksetting.Enabled = false;
        cbfooter.Enabled = false;
        divBlock.Visible = false;
        lblBlock.Visible = false;

    }

    protected void Radioformat2_CheckedChanged(object sender, EventArgs e)
    {
        divBlock.Visible = true;
        lblBlock.Visible = true;
        lblreportname2.Visible = false;
        txtreportname2.Visible = false;
        hallbind();
        loadBlock();
        lblexcsea.Visible = false;
        txtexseat.Visible = false;
        Fpspread.Visible = false;
        pnlContent1.Visible = false;
        btnprintmaster.Visible = false;
        btnDirectPrint.Visible = false;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Text = string.Empty;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        ddlhall.Enabled = false;
        Fspread3.Visible = false;
        pnlContents.Visible = false;
        Exportexcel.Visible = false;
        Printfspread3.Visible = false;
        btn_directprint.Visible = false;
        Fpseating.Visible = false;
        Chksetting.Enabled = true;
        Excel_seating.Visible = false;
        Print_seating.Visible = false;
        cbfooter.Enabled = true;
    }

    protected void Radioformat3_CheckedChanged(object sender, EventArgs e)
    {
        lblreportname2.Visible = false;
        txtreportname2.Visible = false;
        lblexcsea.Visible = false;
        txtexseat.Visible = false;
        hallbind();
        ddlhall.Enabled = true;
        Fpseating.Visible = false;
        Fpspread.Visible = false;
        pnlContent1.Visible = false;
        Fspread3.Visible = false;
        pnlContents.Visible = false;
        lblmsg.Visible = false;
        lblmessage1.Visible = false;
        lblrptname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        btnDirectPrint.Visible = false;
        Printcontrol.Visible = false;
        txtexcelname.Visible = false;
        Exportexcel.Visible = false;
        Printfspread3.Visible = false;
        btn_directprint.Visible = false;
        Print_seating.Visible = false;
        Excel_seating.Visible = false;
        Chksetting.Enabled = false;
        cbfooter.Enabled = false;
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkNewSeating.Checked)
            {
                Fpseating.Visible = false;
                Print_seating.Visible = false;
                Excel_seating.Visible = false;
                ddlhall.Enabled = true;
                Fspread3.Visible = false;
                pnlContents.Visible = false;
                Exportexcel.Visible = false;
                Printfspread3.Visible = false;
                btn_directprint.Visible = false;
                collegeCode = string.Empty;
                string qryCollege = string.Empty;
                if (chkmergrecol.Checked)
                {
                    collegeCode = Convert.ToString(Session["collegecode"]).Trim();
                }
                else
                {
                    if (cblCollege.Items.Count > 0)
                    {
                        collegeCode = getCblSelectedValue(cblCollege);
                    }
                }
                if (Radioformat3.Checked == true)
                {
                    formatthree();
                }
                else
                {
                    if (ddlYear.Items.Count == 0)
                    {
                        Fspread3.Visible = false;
                        pnlContents.Visible = false;
                        Exportexcel.Visible = false;
                        Printfspread3.Visible = false; btn_directprint.Visible = false;
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Select The Exam Year and Then Proceed";
                        Fpspread.Visible = false;
                        pnlContent1.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        btnDirectPrint.Visible = false;
                        lblrptname.Visible = false;
                        return;
                    }
                    if (ddlMonth.Items.Count == 0)
                    {
                        Fspread3.Visible = false;
                        pnlContents.Visible = false;
                        Exportexcel.Visible = false;
                        Printfspread3.Visible = false; btn_directprint.Visible = false;
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Select The Exam Month and Then Proceed";
                        Fpspread.Visible = false;
                        pnlContent1.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        btnDirectPrint.Visible = false;
                        lblrptname.Visible = false;
                        return;
                    }
                    hss = 0;
                    Hashtable ht1 = new Hashtable();
                    int seatno = 0;
                    int rowcn = 0;
                    int roww = 0;
                    int spcn = 0;
                    string strmode = string.Empty;
                    string typaeva = string.Empty;
                    if (ddltype.Items.Count > 0)
                    {
                        if (ddltype.SelectedItem.Text.ToLower().Trim() != "all")
                        {
                            if (ddltype.SelectedItem.Text.Trim() != "")
                            {
                                if (ddltype.SelectedItem.Text.Trim().ToLower() != "mca" && ddltype.SelectedItem.Text.Trim().ToLower() != "day")
                                {
                                    strmode = " and Mode='" + ddltype.SelectedItem.Text.Trim() + "'";
                                    typaeva = "and c.type='" + ddltype.SelectedItem.Text.Trim() + "'";
                                }
                                else
                                {
                                    strmode = " and Mode in('Day','MCA')";
                                    typaeva = " and c.type in('Day','MCA')";
                                }
                            }
                        }
                    }
                    if (chkmergrecol.Checked == true)
                    {
                        typaeva = string.Empty;
                        strmode = string.Empty;
                        qryCollege = string.Empty;
                    }
                    else
                    {
                        qryCollege = " and d.college_code in(" + collegeCode + ")";
                        if (collegeCode.Contains(","))
                        {
                            typaeva = string.Empty;
                            strmode = string.Empty;
                        }
                    }
                    Hashtable htCompleted = new Hashtable();
                    Dictionary<string, int> dicAllsubjects = new Dictionary<string, int>();
                    string strdateval = string.Empty;
                    string getrommdate = string.Empty;
                    string getrommdate1 = string.Empty;
                    string dateval = string.Empty;
                    if (ddlDate.Items.Count > 0)
                    {
                        if (ddlDate.SelectedItem.Text.Trim().ToLower() != "all" && ddlDate.SelectedItem.Text.Trim().ToLower() != "")
                        {
                            string edate = ddlDate.SelectedItem.Text.ToString().Trim();
                            string[] spd = edate.Split('-');
                            strdateval = strdateval + " and et.exam_date='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                            strdateval = strdateval + " and et.exam_session='" + ddlSession.SelectedItem.ToString().Trim() + "'";
                            getrommdate = getrommdate + " and e.edate='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                            getrommdate = getrommdate + " and e.ses_sion='" + ddlSession.SelectedItem.ToString().Trim() + "'";

                            getrommdate1 = getrommdate1 + " and es.edate='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                            getrommdate1 = getrommdate1 + " and es.ses_sion='" + ddlSession.SelectedItem.ToString().Trim() + "'";
                            dateval = spd[1] + '-' + spd[0] + '-' + spd[2];
                        }
                    }
                    if (!string.IsNullOrEmpty(collegeCode.Trim()))
                    {
                        string prior = "select * from class_master where coll_code in (" + collegeCode + ") " + strmode + " order by Mode,priority";
                        ds = dt.select_method_wo_parameter(prior, "text");
                    }
                    if (chkmergrecol.Checked == true)
                    {
                        //strmode = " ";
                        //typaeva = " ";
                        typaeva = string.Empty;
                        strmode = string.Empty;
                    }
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string strdelquery = "delete es from exam_seating es,Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=et.subject_no and et.subject_no=es.subject_no and ed.degree_code=es.degree_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and ed.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.ToString() + "' " + strdateval + " " + typaeva + qryCollege + " ";
                        //strdelquery = "delete es from exam_seating es,Registration r,course c,degree d,department dt,exmtt etd,exmtt_det et where c.Course_Id=d.Course_Id and d.Dept_Code=dt.Dept_Code and d.Degree_Code=r.degree_code and r.degree_code=es.degree_code and r.degree_code=etd.degree_code and es.degree_code=etd.degree_code and etd.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and r.Batch_Year=etd.batchFrom and r.Reg_No=es.regno and et.exam_code=etd.exam_code and et.exam_date=es.edate and es.ses_sion=et.exam_session and et.subject_no=es.subject_no and r.college_code=c.college_code and c.college_code=d.college_code and dt.college_code=r.college_code and c.college_code=dt.college_code and d.college_code=r.college_code  and etd.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and etd.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' " + strdateval + " " + typaeva + qryCollege;//and et.exam_date='2016-04-21' and et.exam_session='A.N'
                        strdelquery = "delete es from exam_seating es,course c,degree d,exmtt etd,exmtt_det et where c.Course_Id=d.Course_Id  and es.degree_code=etd.degree_code and etd.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and et.exam_code=etd.exam_code and et.exam_date=es.edate and es.ses_sion=et.exam_session and et.subject_no=es.subject_no and c.college_code=d.college_code and etd.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and etd.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' " + strdateval + " " + typaeva + qryCollege;
                        if (chkmergrecol.Checked == true)
                        {
                            typaeva = string.Empty;
                            //strdelquery = "delete es from exam_seating es,Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=et.subject_no and et.subject_no=es.subject_no and ed.degree_code=es.degree_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and ed.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.ToString() + "' " + strdateval + " " + typaeva + qryCollege + " ";
                            //strdelquery = "delete es from exam_seating es,Registration r,course c,degree d,department dt,exmtt etd,exmtt_det et where c.Course_Id=d.Course_Id and d.Dept_Code=dt.Dept_Code and d.Degree_Code=r.degree_code and r.degree_code=es.degree_code and r.degree_code=etd.degree_code and es.degree_code=etd.degree_code and etd.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and r.Batch_Year=etd.batchFrom and r.Reg_No=es.regno and et.exam_code=etd.exam_code and et.exam_date=es.edate and es.ses_sion=et.exam_session and et.subject_no=es.subject_no and r.college_code=c.college_code and c.college_code=d.college_code and dt.college_code=r.college_code and c.college_code=dt.college_code and d.college_code=r.college_code  and etd.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and etd.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' " + strdateval + " " + typaeva + qryCollege;


                            strdelquery = "delete es from exam_seating es,course c,degree d,exmtt etd,exmtt_det et where c.Course_Id=d.Course_Id  and es.degree_code=etd.degree_code and etd.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and et.exam_code=etd.exam_code and et.exam_date=es.edate and es.ses_sion=et.exam_session and et.subject_no=es.subject_no and c.college_code=d.college_code and etd.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and etd.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' " + strdateval + " " + typaeva + qryCollege;
                        }

                        strdelquery = "delete es from exam_seating es,course c,degree d where c.Course_Id=d.Course_Id   and es.degree_code=d.Degree_Code  and c.college_code=d.college_code   " + getrommdate1 + " " + typaeva + qryCollege;

                        int delq = dt.update_method_wo_parameter(strdelquery, "text");
                        Dictionary<string, int> dicdubcount = new Dictionary<string, int>();
                        DataSet ds3 = new DataSet();
                        string typeval = string.Empty;
                        if (ddltype.Items.Count > 0)
                        {
                            if (ddltype.SelectedItem.Text.Trim().ToLower() != "all")
                            {
                                if (ddltype.SelectedItem.Text.Trim().ToLower() != "")
                                {
                                    typeval = " and c.type='" + ddltype.SelectedItem.Text.Trim() + "'";
                                    if (ddltype.SelectedItem.Text.Trim().ToLower() != "mca" && ddltype.SelectedItem.Text.Trim().ToLower() != "day")
                                    {
                                        //strmode = " and Mode='" + ddltype.SelectedItem.Text.Trim() + "'";
                                        typeval = "and c.type='" + ddltype.SelectedItem.Text.Trim() + "'";
                                    }
                                    else
                                    {
                                        //strmode = " and Mode in('Day','MCA')";
                                        typeval = " and c.type in('Day','MCA')";
                                    }
                                }
                            }
                        }
                        if (chkmergrecol.Checked == true)
                        {
                            typaeva = string.Empty;
                            strmode = string.Empty;
                            typeval = string.Empty;
                        }
                        else
                        {
                            if (collegeCode.Contains(","))
                            {
                                typaeva = string.Empty;
                                strmode = string.Empty;
                                typeval = string.Empty;
                            }
                        }
                        string strexamdate = " and et.exam_date='" + dateval + "'";
                        string strexamsession = string.Empty;
                        if (ddlSession.Items.Count > 0)
                        {
                            if (ddlSession.SelectedItem.Text.Trim().ToLower() != "all" && ddlSession.SelectedItem.Text.Trim() != "")
                            {
                                strexamsession = " and et.exam_session='" + ddlSession.SelectedItem.Text.Trim() + "'";
                            }
                        }
                        //string roll = "select ed.subject_no,s.subject_code,s.subject_name,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code " + typeval + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + qryCollege + " order by et.exam_date,et.exam_session,c.type,s.subject_code,e.degree_code,e.batch_year desc,r.Reg_No asc";
                        //rajkumar ===========================
                        //string roll = "select ed.subject_no,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym end subject_codeAcr,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et, Registration r,Degree d,course c,Department de,subject s left join tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code " + typeval + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + qryCollege + " order by et.exam_date,et.exam_session,c.type,s.subject_code,e.degree_code,e.batch_year desc,r.Reg_No asc";
                        //==================================================
                        string roll = "select ed.subject_no,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym end subject_codeAcr,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end QpaperType,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et, Registration r,Degree d,course c,Department de,subject s left join  tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code   " + typeval + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + qryCollege + " order by et.exam_date,et.exam_session,c.type,s.subject_code,e.degree_code,e.batch_year desc,r.Reg_No asc";
                        if (chkmergrecol.Checked == true)
                        {
                            typeval = string.Empty;
                            //roll = "select ed.subject_no,s.subject_code,s.subject_name,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et,Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code " + typeval + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + qryCollege + " order by et.exam_date,et.exam_session,c.type,s.subject_code,e.degree_code,e.batch_year desc,r.Reg_No asc";

                            //rajkumar=========================
                            //roll = "select ed.subject_no,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym end subject_codeAcr,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et,Registration r,Degree d,course c,Department de,subject s left join tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code " + typeval + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + qryCollege + " order by et.exam_date,et.exam_session,c.type,s.subject_code,e.degree_code,e.batch_year desc,r.Reg_No asc";
                            //=============================

                            roll = "select ed.subject_no,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym end subject_codeAcr,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end QpaperType,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et, Registration r,Degree d,course c,Department de,subject s left join  tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code   " + typeval + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + qryCollege + " order by et.exam_date,et.exam_session,c.type,s.subject_code,e.degree_code,e.batch_year desc,r.Reg_No asc";
                        }
                        ds2 = dt.select_method_wo_parameter(roll, "text");
                        DataView dv = new DataView();
                        //string exam = "select s.subject_code,s.subject_name,et.exam_date,et.exam_session,COUNT(ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + strexamdate + qryCollege + " " + strexamsession + " ";

                        string exam = "select case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym end subject_codeAcr,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end QpaperType ,et.exam_date,et.exam_session,COUNT(distinct ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et, Registration r,Degree d,course c,Department de,subject s left join  tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id  and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + strexamdate + qryCollege + " " + strexamsession + " ";

                        exam = exam + typeval;
                        //exam = exam + " group by s.subject_code,s.subject_name,et.exam_date,et.exam_session,c.type order by et.exam_date,et.exam_session,c.type,stucount desc";
                        exam = exam + " group by case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym end,et.exam_date,et.exam_session,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end order by et.exam_date,et.exam_session,stucount desc";
                        if (chkmergrecol.Checked == true)
                        {
                            typeval = string.Empty;
                            //exam = "select s.subject_code,s.subject_name,et.exam_date,et.exam_session,COUNT(ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + strexamdate + qryCollege + " " + strexamsession + " ";
                            //exam = exam + typeval;
                            //exam = exam + " group by s.subject_code,s.subject_name,et.exam_date,et.exam_session order by et.exam_date,et.exam_session,stucount desc";
                            exam = "select case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym end subject_codeAcr,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end QpaperType ,et.exam_date,et.exam_session,COUNT(distinct ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et, Registration r,Degree d,course c,Department de,subject s left join  tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id  and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + strexamdate + qryCollege + " " + strexamsession + " ";
                            exam = exam + typeval;
                            exam = exam + " group by case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym end,et.exam_date,et.exam_session,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end order by et.exam_date,et.exam_session,stucount desc";
                        }
                        ds1.Clear();
                        ds1.Reset();
                        ds1 = dt.select_method_wo_parameter(exam, "text");
                        DataSet dsRoomSeatingsArrange = new DataSet();
                        string room = "select * from tbl_room_seats where coll_code in (" + collegeCode + ") " + strmode;
                        dsRoomSeatingsArrange = dt.select_method_wo_parameter(room, "text");
                        if (dsRoomSeatingsArrange.Tables.Count > 0 && dsRoomSeatingsArrange.Tables[0].Rows.Count == 0)
                        {
                            Fspread3.Visible = false;
                            pnlContents.Visible = false;
                            Exportexcel.Visible = false;
                            Printfspread3.Visible = false; btn_directprint.Visible = false;
                            lblmsg.Visible = true;
                            lblmsg.Text = "No Hall Definition Were Found.";
                            Fpspread.Visible = false;
                            pnlContent1.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            btnprintmaster.Visible = false;
                            btnDirectPrint.Visible = false;
                            lblrptname.Visible = false;
                            return;
                        }
                        string columnNameVal = "subject_code";
                        if (chkForSeating.Checked)
                            columnNameVal = "subject_codeAcr";//subject_codeAcr
                        if (chkForSeating.Checked && CheckBox1.Checked)
                            columnNameVal = "QpaperType";//rajkumar 02/02/2018
                        //,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym end subject_codeAcr
                        DataTable dtAllDistinctSubjectsCommon = new DataTable();
                        dtAllDistinctSubjectsCommon.Columns.Add(columnNameVal);
                        dtAllDistinctSubjectsCommon.Columns.Add("exam_date");
                        dtAllDistinctSubjectsCommon.Columns.Add("exam_session");
                        dtAllDistinctSubjectsCommon.Columns.Add("studentCount", typeof(int));

                        DataTable dtAllDistinctSubjects = new DataTable();
                        DataTable dtAllDistinctSubjectsList = new DataTable();
                        Dictionary<string, int> dicInCompleteSubjects = new Dictionary<string, int>();
                        Dictionary<string, int> dicTotalStudentsForSubjects = new Dictionary<string, int>();
                        int totalStudent = 0;
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            dicAllsubjects.Clear();
                            dicInCompleteSubjects.Clear();
                            dicTotalStudentsForSubjects.Clear();
                            dtAllDistinctSubjects = ds1.Tables[0].DefaultView.ToTable(true, columnNameVal, "exam_date", "exam_session");
                            //dtAllDistinctSubjectsList = ds1.Tables[0].DefaultView.ToTable(true, "subject_code", "subject_name", "exam_date", "exam_session");
                            int index = 0;
                            foreach (DataRow dr in ds1.Tables[0].Rows)
                            {
                                string subjectCode = Convert.ToString(dr[columnNameVal]).Trim();
                                string subjectName = Convert.ToString(dr["subject_name"]).Trim();
                                string examDate = Convert.ToString(dr["exam_date"]).Trim();
                                string examSessions = Convert.ToString(dr["exam_session"]).Trim();
                                string studentCounts = Convert.ToString(dr["stucount"]).Trim();
                                int studentsCount = 0;
                                int.TryParse(studentCounts, out studentsCount);
                                totalStudent += studentsCount;
                                if (!dicAllsubjects.ContainsKey(Convert.ToString(subjectCode).Trim().ToLower()))
                                {
                                    dicAllsubjects.Add(Convert.ToString(subjectCode).Trim().ToLower(), index);
                                }
                                if (!dicInCompleteSubjects.ContainsKey(Convert.ToString(subjectCode).Trim().ToLower()))
                                {
                                    dicInCompleteSubjects.Add(Convert.ToString(subjectCode).Trim().ToLower(), 0);
                                }
                                if (!dicTotalStudentsForSubjects.ContainsKey(Convert.ToString(subjectCode).Trim().ToLower()))
                                {
                                    dicTotalStudentsForSubjects.Add(Convert.ToString(subjectCode).Trim().ToLower(), studentsCount);
                                }
                                else
                                {
                                    int countValue = dicTotalStudentsForSubjects[subjectCode.Trim().ToLower()];
                                    dicTotalStudentsForSubjects[subjectCode.Trim().ToLower()] = countValue + studentsCount;
                                }
                                index++;
                            }
                            DataRow drStudent;
                            foreach (KeyValuePair<string, int> item in dicTotalStudentsForSubjects)
                            {
                                drStudent = dtAllDistinctSubjectsCommon.NewRow();
                                string subjectCode = Convert.ToString(item.Key).Trim();
                                int studentCount = item.Value;
                                dtAllDistinctSubjects.DefaultView.RowFilter = columnNameVal + "='" + subjectCode + "'";
                                DataView dvSubject = dtAllDistinctSubjects.DefaultView;
                                drStudent[columnNameVal] = Convert.ToString(dvSubject[0][columnNameVal]).Trim();
                                drStudent["exam_date"] = Convert.ToString(dvSubject[0]["exam_date"]).Trim();
                                drStudent["exam_session"] = Convert.ToString(dvSubject[0]["exam_session"]).Trim();
                                drStudent["studentCount"] = Convert.ToString(studentCount);
                                dtAllDistinctSubjectsCommon.Rows.Add(drStudent);
                            }
                            if (dtAllDistinctSubjectsCommon.Rows.Count > 0)
                            {
                                dtAllDistinctSubjects = new DataTable();
                                dtAllDistinctSubjectsCommon.DefaultView.Sort = "studentCount desc";
                                dtAllDistinctSubjects = dtAllDistinctSubjectsCommon.DefaultView.ToTable();
                            }
                        }
                        int OddIndex = 0;
                        int EvenIndex = 1;
                        bool flag = false;
                        bool Evenflag = false;
                        bool Oddflag = false;
                        string strgetdate = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,convert(varchar(20),et.exam_date,110) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' and et.coll_code in (" + collegeCode + ") " + strdateval + " order by  ExamDate";
                        ds3 = dt.select_method_wo_parameter(strgetdate, "text");
                        int totAllotedStudents = 0;
                        int totAllotedStudentsNew = 0;
                        int totActualStudents = 0;
                        int totActualStudentsNew = 0;
                        if (dsRoomSeatingsArrange.Tables.Count > 0 && dsRoomSeatingsArrange.Tables[0].Rows.Count > 0)
                        {
                            object total = dsRoomSeatingsArrange.Tables[0].Compute("sum(allocted_seats)", string.Empty);
                            int.TryParse(Convert.ToString(total).Trim(), out totAllotedStudents);

                            total = dsRoomSeatingsArrange.Tables[0].Compute("sum(allotedSeatsNew)", string.Empty);
                            int.TryParse(Convert.ToString(total).Trim(), out totAllotedStudentsNew);

                            total = dsRoomSeatingsArrange.Tables[0].Compute("sum(actual_seats)", string.Empty);
                            int.TryParse(Convert.ToString(total).Trim(), out totActualStudents);

                            total = dsRoomSeatingsArrange.Tables[0].Compute("sum(actualSeatsNew)", string.Empty);
                            int.TryParse(Convert.ToString(total).Trim(), out totActualStudentsNew);

                        }
                        if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                        {
                            spcn = ds3.Tables[0].Rows.Count - 1;
                            for (int sp = 0; sp < ds3.Tables[0].Rows.Count; sp++)
                            {
                                Dictionary<string, string> dicStudentsHall = new Dictionary<string, string>();
                                Dictionary<string, int> dicHallMaxSeatNo = new Dictionary<string, int>();
                                Dictionary<string, int> dicStudentsAlloted = new Dictionary<string, int>();
                                bool isAlternate = false;
                                bool isOne = false;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    seatno = 0;
                                    int seatingNo = 0;
                                    int oddSeatNo = 0;
                                    int evenSeatNo = 0;
                                    int newSeatNo = 0;
                                    string halno = Convert.ToString(ds.Tables[0].Rows[i]["rno"]).Trim();
                                    //string room = "select * from tbl_room_seats where Hall_No='" + halno + "' and exm_month='" + ddlMonth.SelectedValue + "' and exm_year='" + ddlYear.SelectedItem.Text + "' and coll_code='" + college_code + "' " + strmode + "";
                                    room = "select * from tbl_room_seats where Hall_No='" + halno + "' and coll_code in (" + collegeCode + ") " + strmode + "";
                                    //Hashtable htHallSubject = new Hashtable();
                                    DataTable dtRoomSeating = new DataTable();
                                    if (dsRoomSeatingsArrange.Tables.Count > 0 && dsRoomSeatingsArrange.Tables[0].Rows.Count > 0)
                                    {
                                        dsRoomSeatingsArrange.Tables[0].DefaultView.RowFilter = "Hall_No='" + halno + "' " + ((isAlternate) ? " and hasAlternate ='1'" : "");
                                        dtRoomSeating = dsRoomSeatingsArrange.Tables[0].DefaultView.ToTable();
                                    }
                                    int tempOdd = OddIndex;
                                    bool tempOddFlag = Oddflag;
                                    //int tempEven = EvenIndex;
                                    if (dicInCompleteSubjects.Count == 1)
                                    {
                                        if (OddIndex > EvenIndex)
                                        {
                                            OddIndex = EvenIndex;
                                            Oddflag = Evenflag;
                                            Evenflag = tempOddFlag;
                                            EvenIndex = tempOdd;
                                        }
                                    }
                                    Dictionary<string, int> dicHallSubject = new Dictionary<string, int>();
                                    //DataSet dsrommdet = dt.select_method_wo_parameter(room, "text");
                                    DataSet dsrommdet = new DataSet();
                                    dsrommdet.Clear();
                                    dsrommdet.Tables.Add(dtRoomSeating);
                                    if (dsrommdet.Tables.Count > 0 && dsrommdet.Tables[0].Rows.Count > 0)
                                    {
                                        //if (dtRoomSeating.Rows.Count > 0)
                                        //{
                                        //    string floor1 = Convert.ToString(dtRoomSeating.Rows[0]["Floor_Name"]).Trim();
                                        //    string norow1 = Convert.ToString(dtRoomSeating.Rows[0]["no_of_rows"]).Trim();
                                        //    string arrangeview1 = Convert.ToString(dtRoomSeating.Rows[0]["arranged_view"]).Trim();
                                        //    string nocol1 = Convert.ToString(dtRoomSeating.Rows[0]["no_of_columns"]).Trim();
                                        //    string mode1 = Convert.ToString(dtRoomSeating.Rows[0]["mode"]).Trim();
                                        //    string acseat1 = Convert.ToString(dtRoomSeating.Rows[0]["actual_seats"]).Trim();
                                        //    string allotseat1 = Convert.ToString(dtRoomSeating.Rows[0]["allocted_seats"]).Trim();
                                        //    string seattype1 = Convert.ToString(dtRoomSeating.Rows[0]["is_single"]).Trim();
                                        //    string month1 = Convert.ToString(dtRoomSeating.Rows[0]["exm_month"]).Trim();
                                        //    string year1 = Convert.ToString(dtRoomSeating.Rows[0]["exm_year"]).Trim();
                                        //    string[] arrang1 = arrangeview1.Split(';');
                                        //}
                                        string floor = Convert.ToString(dsrommdet.Tables[0].Rows[0]["Floor_Name"]).Trim();
                                        norow = Convert.ToString(dsrommdet.Tables[0].Rows[0]["no_of_rows"]).Trim();
                                        string arrangeview = Convert.ToString(dsrommdet.Tables[0].Rows[0]["arranged_view"]).Trim();
                                        nocol = Convert.ToString(dsrommdet.Tables[0].Rows[0]["no_of_columns"]).Trim();
                                        string mode = Convert.ToString(dsrommdet.Tables[0].Rows[0]["mode"]).Trim();
                                        string acseat = Convert.ToString(dsrommdet.Tables[0].Rows[0]["actual_seats"]).Trim();
                                        allotseat = Convert.ToString(dsrommdet.Tables[0].Rows[0]["allocted_seats"]).Trim();
                                        //string seattype = Convert.ToString(dsrommdet.Tables[0].Rows[0]["is_single"]).Trim();
                                        //string month = Convert.ToString(dsrommdet.Tables[0].Rows[0]["exm_month"]).Trim();
                                        //string year = Convert.ToString(dsrommdet.Tables[0].Rows[0]["exm_year"]).Trim();
                                        arrang = arrangeview.Split(';');

                                        string arrangeViewNew = Convert.ToString(dsrommdet.Tables[0].Rows[0]["arrangedViewNew"]).Trim();
                                        string actualSeats = Convert.ToString(dsrommdet.Tables[0].Rows[0]["actualSeatsNew"]).Trim();
                                        string allotedSeats = Convert.ToString(dsrommdet.Tables[0].Rows[0]["allotedSeatsNew"]).Trim();
                                        string defaultViewNew = Convert.ToString(dsrommdet.Tables[0].Rows[0]["defaultViewNew"]).Trim();
                                        if (isAlternate)
                                        {
                                            arrang = arrangeViewNew.Split(';');
                                            if (dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                            {
                                                int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                if (seatno >= seatVal)
                                                {
                                                    dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                }
                                                else
                                                {
                                                    dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                }
                                                seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                            }
                                        }
                                        Dictionary<int, int> dicsubcol = new Dictionary<int, int>();
                                        Dictionary<string, int> dicsubcolcount = new Dictionary<string, int>();
                                        for (int spr = 0; spr <= arrang.GetUpperBound(0); spr++)
                                        {
                                            string colsp = arrang[spr].ToString();
                                            if (colsp.Trim() != "" && colsp != null)
                                            {
                                                spcel = colsp.Split('-');
                                                for (int spc = 0; spc <= spcel.GetUpperBound(0); spc++)
                                                {
                                                    int colsn = Convert.ToInt32(spcel[spc]);
                                                    string strrow = "C" + spc + "R" + spr;
                                                    if (!dicsubcolcount.ContainsKey(strrow))
                                                    {
                                                        dicsubcolcount.Add(strrow, colsn);
                                                    }
                                                    if (dicsubcol.ContainsKey(spc))
                                                    {
                                                        int valc = dicsubcol[spc];
                                                        if (valc < colsn)
                                                        {
                                                            dicsubcol[spc] = colsn;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dicsubcol.Add(spc, colsn);
                                                    }
                                                }
                                            }
                                        }
                                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && dtAllDistinctSubjects.Rows.Count > 0)
                                        {
                                            string sub = string.Empty;
                                            string ses = string.Empty;
                                            string emd = string.Empty;
                                            string degcd = string.Empty;
                                            string subcode = string.Empty;
                                            string seatValue = string.Empty;
                                            int autoChar = 97;
                                            newSeatNo = 0;
                                            for (int col = 0; col < Convert.ToInt32(nocol); col++)
                                            {
                                                tempOdd = OddIndex;
                                                tempOddFlag = Oddflag;
                                                //tempEven = EvenIndex;
                                                seatValue = string.Empty;
                                                if (dicInCompleteSubjects.Count == 1)
                                                {
                                                    if (OddIndex > EvenIndex)
                                                    {
                                                        OddIndex = EvenIndex;
                                                        Oddflag = Evenflag;
                                                        Evenflag = tempOddFlag;
                                                        EvenIndex = tempOdd;
                                                    }
                                                }
                                                int rowSeat = 0;
                                                for (int row = 0; row < Convert.ToInt32(norow); row++)
                                                {
                                                    string strrow = "C" + col + "R" + row;
                                                    if (dicsubcolcount.ContainsKey(strrow))
                                                    {
                                                        int getcouv = dicsubcolcount[strrow];
                                                        int sucol = dicsubcol[col];
                                                        int recaldept = 0;
                                                        string subjectCodeOld = subcode;
                                                        for (int subcol = 0; subcol < Convert.ToInt32(sucol); subcol++)
                                                        {
                                                            seatValue = Convert.ToString((row + 1) + (Convert.ToInt32(norow) * subcol)) + Convert.ToString((char)autoChar);
                                                            seatingNo = (row + 1) + (Convert.ToInt32(norow) * subcol);
                                                            string keyValue1 = Convert.ToString(halno.Trim() + "@" + seatValue.Trim()).Trim().ToLower();
                                                            //rowSeat++;
                                                            newSeatNo++;
                                                            if (!dicStudentsHall.ContainsKey(keyValue1))
                                                            {
                                                                int scl = 0;
                                                                int oldscl = subcol;
                                                                subjectCodeOld = subcode;
                                                                DataView dvSub = new DataView();
                                                                subcode = string.Empty;
                                                                tempOdd = OddIndex;
                                                                tempOddFlag = Oddflag;
                                                                //tempEven = EvenIndex;
                                                                if (dicInCompleteSubjects.Count == 1)
                                                                {
                                                                    if (OddIndex > EvenIndex)
                                                                    {
                                                                        OddIndex = EvenIndex;
                                                                        Oddflag = Evenflag;
                                                                        Evenflag = tempOddFlag;
                                                                        EvenIndex = tempOdd;
                                                                    }
                                                                }
                                                            raja: if (subcol % 2 != 0)
                                                                {
                                                                    if (Evenflag == true)
                                                                    {
                                                                        if (EvenIndex >= dtAllDistinctSubjects.Rows.Count)
                                                                        {
                                                                            if (subcol < getcouv)
                                                                            {
                                                                                seatno++;
                                                                                if (!dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                                                                {
                                                                                    dicHallMaxSeatNo.Add(halno.Trim().ToLower(), seatno);
                                                                                }
                                                                                else
                                                                                {
                                                                                    int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                    if (seatno >= seatVal)
                                                                                    {
                                                                                        dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                                                    }
                                                                                    seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                }
                                                                            }
                                                                        }
                                                                        continue;
                                                                    }
                                                                    if (dtAllDistinctSubjects.Rows.Count > EvenIndex)
                                                                    {
                                                                        subcode = Convert.ToString(dtAllDistinctSubjects.Rows[EvenIndex][columnNameVal]).Trim();
                                                                        flag = true;
                                                                        scl = EvenIndex;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (Oddflag == true)
                                                                    {
                                                                        if (OddIndex >= dtAllDistinctSubjects.Rows.Count)
                                                                        {
                                                                            if (subcol < getcouv)
                                                                            {
                                                                                seatno++;
                                                                                if (!dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                                                                {
                                                                                    dicHallMaxSeatNo.Add(halno.Trim().ToLower(), seatno);
                                                                                }
                                                                                else
                                                                                {
                                                                                    int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                    if (seatno >= seatVal)
                                                                                    {
                                                                                        dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                                                    }
                                                                                    seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                }
                                                                            }
                                                                        }
                                                                        continue;
                                                                    }
                                                                    if (dtAllDistinctSubjects.Rows.Count > OddIndex)
                                                                    {
                                                                        flag = false;
                                                                        subcode = Convert.ToString(dtAllDistinctSubjects.Rows[OddIndex][columnNameVal]).Trim();
                                                                        scl = OddIndex;
                                                                    }
                                                                }
                                                                emd = Convert.ToString(dtAllDistinctSubjects.Rows[scl]["exam_date"]).Trim();
                                                                ses = Convert.ToString(dtAllDistinctSubjects.Rows[scl]["exam_session"]).Trim();
                                                                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                                                {
                                                                    ds2.Tables[0].DefaultView.RowFilter = columnNameVal + "='" + subcode + "' and exam_date='" + emd + "' and exam_session='" + ses + "' ";
                                                                    ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,subject_code,Course_Name,Dept_Name,batch_year desc,Reg_No asc";
                                                                    dvSub = ds2.Tables[0].DefaultView;
                                                                }
                                                                if (dvSub.Count > 0)
                                                                {
                                                                    if (subcol < getcouv)
                                                                    {
                                                                        int stuco = 0;
                                                                        if (dicdubcount.ContainsKey(subcode.ToString().Trim().ToLower()))
                                                                        {
                                                                            stuco = dicdubcount[subcode.ToString().Trim().ToLower()];
                                                                        }
                                                                        else
                                                                        {
                                                                            dicdubcount.Add(subcode.ToString().Trim().ToLower(), 0);
                                                                        }
                                                                        if (stuco != dvSub.Count)
                                                                        {
                                                                            if (dvSub.Count > stuco)
                                                                            {
                                                                                seatno++;
                                                                                if (!dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                                                                {
                                                                                    dicHallMaxSeatNo.Add(halno.Trim().ToLower(), seatno);
                                                                                }
                                                                                else
                                                                                {
                                                                                    int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                    if (seatno >= seatVal)
                                                                                    {
                                                                                        dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                                                    }
                                                                                    seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                }
                                                                                // btngen = 1;
                                                                                string roll1 = Convert.ToString(dvSub[stuco]["Reg_No"]).Trim();
                                                                                degcd = Convert.ToString(dvSub[stuco]["Degree_Code"]).Trim();
                                                                                sub = Convert.ToString(dvSub[stuco]["subject_no"]).Trim();
                                                                                string keyValue = Convert.ToString(halno.Trim() + "@" + seatValue.Trim()).Trim().ToLower();
                                                                                if (!dicStudentsHall.ContainsKey(keyValue))
                                                                                {
                                                                                    dicStudentsHall.Add(keyValue, roll1);
                                                                                }

                                                                                if (!dicStudentsAlloted.ContainsKey(roll1.Trim().ToLower()))
                                                                                {
                                                                                    dicStudentsAlloted.Add(roll1.Trim().ToLower(), 1);

                                                                                    //to be changed 
                                                                                    int sss = newSeatNo;
                                                                                    string seatarrange = "if exists(select * from exam_seating where edate='" + emd + "' and ses_sion='" + ses + "' and subject_no='" + sub + "' and roomno='" + halno + "' and seat_no='" + newSeatNo + "')delete from exam_seating where edate='" + emd + "' and ses_sion='" + ses + "' and subject_no='" + sub + "' and roomno='" + halno + "' and seat_no='" + newSeatNo + "' insert into exam_seating (roomno,regno,subject_no,edate,ses_sion,block,seat_no,degree_code,ArrangementType,Floorid,seatCode)values('" + halno + "','" + roll1 + "','" + sub + "','" + emd + "','" + ses + "','" + floor + "','" + newSeatNo + "','" + degcd + "',0,'" + floor + "','" + seatValue + "')";
                                                                                    int a = dt.update_method_wo_parameter(seatarrange, "text");
                                                                                    stuco++;
                                                                                    dicdubcount[subcode.ToString().Trim().ToLower()] = stuco;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (dicInCompleteSubjects.ContainsKey(subcode.ToString().Trim().ToLower()))
                                                                            {
                                                                                dicInCompleteSubjects.Remove(subcode.ToString().Trim().ToLower());
                                                                            }
                                                                            if (flag == true)
                                                                            {
                                                                                if (OddIndex > EvenIndex)
                                                                                {
                                                                                    EvenIndex = OddIndex + 1;
                                                                                }
                                                                                else if (OddIndex < EvenIndex)
                                                                                {
                                                                                    EvenIndex = EvenIndex + 1;
                                                                                }
                                                                                if (EvenIndex >= dtAllDistinctSubjects.Rows.Count)
                                                                                {
                                                                                    //EvenIndex--;
                                                                                    Evenflag = true;
                                                                                }
                                                                                goto raja;
                                                                                //seatno-=1;
                                                                            }
                                                                            else if (flag == false)
                                                                            {
                                                                                if (OddIndex > EvenIndex)
                                                                                {
                                                                                    OddIndex = OddIndex + 1;
                                                                                }
                                                                                else if (OddIndex < EvenIndex)
                                                                                {
                                                                                    OddIndex = EvenIndex + 1;
                                                                                }
                                                                                if (OddIndex >= dtAllDistinctSubjects.Rows.Count)
                                                                                {
                                                                                    //OddIndex--;
                                                                                    Oddflag = true;
                                                                                }
                                                                                goto raja;
                                                                                //seatno -= 1;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                autoChar++;
                                            }
                                        }
                                        else
                                        {
                                            lblmsg.Visible = true;
                                            lblmsg.Text = ddltype.SelectedItem.Text + " " + "Type Student Not Write Exam This Date";
                                            Fpspread.Visible = false;
                                            pnlContent1.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnxl.Visible = false;
                                            btnprintmaster.Visible = false;
                                            btnDirectPrint.Visible = false;
                                            lblrptname.Visible = false;
                                            Fspread3.Visible = false;
                                            pnlContents.Visible = false;
                                            Exportexcel.Visible = false;
                                            Printfspread3.Visible = false;
                                            btn_directprint.Visible = false;
                                        }
                                    }
                                    if (ds.Tables[0].Rows.Count - 1 == i)
                                    {
                                        if (!isOne)
                                        {
                                            if (totalStudent > totAllotedStudents)
                                            {
                                                if (dicStudentsAlloted.Count < totalStudent)
                                                {
                                                    i = -1;
                                                    isAlternate = true;
                                                    isOne = true;
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                            {
                                ddlhall.Items.Clear();
                                if (chkmergrecol.Checked == true)
                                {
                                    typaeva = string.Empty;
                                }
                                else
                                {
                                    if (collegeCode.Contains(","))
                                    {
                                        typaeva = string.Empty;
                                    }
                                }
                                string hl = "select distinct e.roomno,cm.priority from exam_seating e, tbl_room_seats t,class_master cm,Registration r,Degree d,Course c where e.roomno=t.Hall_No and cm.rno=e.roomno and cm.rno=t.Hall_No and e.regno=r.Reg_No and r.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id " + typaeva + " " + getrommdate + " order by cm.priority ";
                                ds = dt.select_method_wo_parameter(hl, "text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    ddlhall.Enabled = true;
                                    ddlhall.DataSource = ds;
                                    ddlhall.DataTextField = "roomno";
                                    ddlhall.DataValueField = "roomno";
                                    ddlhall.DataBind();
                                    hallbind();
                                }
                            }
                            else
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "Please Set Hall Defination";
                                Fpspread.Visible = false;
                                pnlContent1.Visible = false;
                                txtexcelname.Visible = false;
                                btnxl.Visible = false;
                                btnprintmaster.Visible = false;
                                btnDirectPrint.Visible = false;
                                lblrptname.Visible = false;
                                Fspread3.Visible = false;
                                pnlContents.Visible = false;
                                Exportexcel.Visible = false;
                                Printfspread3.Visible = false;
                                btn_directprint.Visible = false;
                            }
                        }
                        else
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "Please Set Time Table";
                            Fpspread.Visible = false;
                            pnlContent1.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            btnprintmaster.Visible = false;
                            btnDirectPrint.Visible = false;
                            lblrptname.Visible = false;
                            Fspread3.Visible = false;
                            pnlContents.Visible = false;
                            Exportexcel.Visible = false;
                            Printfspread3.Visible = false;
                            btn_directprint.Visible = false;
                        }
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Set Hall Priority";
                        Fpspread.Visible = false;
                        pnlContent1.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        btnDirectPrint.Visible = false;
                        lblrptname.Visible = false;
                        Fspread3.Visible = false;
                        pnlContents.Visible = false;
                        Exportexcel.Visible = false;
                        Printfspread3.Visible = false;
                        btn_directprint.Visible = false;
                    }
                }
                if (Radioformat2.Checked == true)
                {
                    ddlhall.Enabled = false;
                    if (ddlhall.Items.Count == 0)
                    {
                        DataSet dn = new DataSet();
                        if (!string.IsNullOrEmpty(collegeCode.Trim()))
                        {
                            string query = "select * from class_master where coll_code in (" + collegeCode + ") order by priority";
                            dn = dt.select_method_wo_parameter(query, "text");
                        }
                    }
                    go();
                    Fpspread.Visible = false;
                    pnlContent1.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    btnDirectPrint.Visible = false;
                    lblrptname.Visible = false;
                }
            }
            else
            {
                Fpseating.Visible = false;
                Print_seating.Visible = false;
                Excel_seating.Visible = false;
                ddlhall.Enabled = true;
                Fspread3.Visible = false;
                pnlContents.Visible = false;
                Exportexcel.Visible = false;
                Printfspread3.Visible = false; btn_directprint.Visible = false;
                collegeCode = string.Empty;
                if (chkmergrecol.Checked)
                {
                    collegeCode = Convert.ToString(Session["collegecode"]).Trim(); ;
                }
                else
                {
                    if (cblCollege.Items.Count > 0)
                    {
                        collegeCode = getCblSelectedValue(cblCollege);
                    }
                }
                if (Radioformat3.Checked == true)
                {
                    formatthree();
                }
                else
                {
                    if (ddlYear.Items.Count == 0)
                    {
                        Fspread3.Visible = false;
                        pnlContents.Visible = false;
                        Exportexcel.Visible = false;
                        Printfspread3.Visible = false; btn_directprint.Visible = false;
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Select The Exam Year and Then Proceed";
                        Fpspread.Visible = false;
                        pnlContent1.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        btnDirectPrint.Visible = false;
                        lblrptname.Visible = false;
                        return;
                    }
                    if (ddlMonth.Items.Count == 0)
                    {
                        Fspread3.Visible = false;
                        pnlContents.Visible = false;
                        Exportexcel.Visible = false;
                        Printfspread3.Visible = false; btn_directprint.Visible = false;
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Select The Exam Month and Then Proceed";
                        Fpspread.Visible = false;
                        pnlContent1.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        btnDirectPrint.Visible = false;
                        lblrptname.Visible = false;
                        return;
                    }
                    hss = 0;
                    Hashtable ht1 = new Hashtable();
                    int seatno = 0;
                    int rowcn = 0;
                    int roww = 0;
                    int spcn = 0;
                    string strmode = string.Empty;
                    string typaeva = string.Empty;
                    string qryCollege = string.Empty;
                    if (ddltype.Items.Count > 0)
                    {
                        if (ddltype.SelectedItem.Text.Trim().ToLower() != "all")
                        {
                            //if (ddltype.SelectedItem.Text.Trim().ToLower() != "")
                            //{
                            //    strmode = " and Mode='" + ddltype.SelectedItem.Text.Trim() + "'";
                            //    typaeva = " and c.type='" + ddltype.SelectedItem.Text.Trim() + "'";
                            //}
                            if (ddltype.SelectedItem.Text.Trim().ToLower() != "mca" && ddltype.SelectedItem.Text.Trim().ToLower() != "day")
                            {
                                strmode = " and Mode='" + ddltype.SelectedItem.Text.Trim() + "'";
                                typaeva = "and c.type='" + ddltype.SelectedItem.Text.Trim() + "'";
                            }
                            else
                            {
                                strmode = " and Mode in('Day','MCA')";
                                typaeva = " and c.type in('Day','MCA')";
                            }

                        }
                    }
                    if (chkmergrecol.Checked == true)
                    {
                        typaeva = string.Empty;
                        strmode = string.Empty;
                        qryCollege = string.Empty;
                    }
                    else
                    {
                        qryCollege = " and d.college_code in(" + collegeCode + ")";
                        if (collegeCode.Contains(","))
                        {
                            typaeva = string.Empty;
                            strmode = string.Empty;
                        }
                    }
                    string strdateval = string.Empty;
                    string getrommdate = string.Empty;
                    string getrommdate1 = string.Empty;
                    string dateval = string.Empty;
                    if (ddlDate.Items.Count > 0)
                    {
                        if (ddlDate.SelectedItem.Text.ToLower().Trim() != "all")
                        {
                            string edate = ddlDate.SelectedItem.Text.ToString().Trim();
                            string[] spd = edate.Split('-');
                            strdateval = strdateval + " and et.exam_date='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                            strdateval = strdateval + " and et.exam_session='" + ddlSession.SelectedItem.ToString() + "'";
                            getrommdate = getrommdate + " and e.edate='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                            getrommdate = getrommdate + " and e.ses_sion='" + ddlSession.SelectedItem.ToString() + "'";

                            getrommdate1 = getrommdate1 + " and es.edate='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                            getrommdate1 = getrommdate1 + " and es.ses_sion='" + ddlSession.SelectedItem.ToString() + "'";
                            dateval = spd[1] + '-' + spd[0] + '-' + spd[2];
                        }
                    }
                    if (!string.IsNullOrEmpty(collegeCode.Trim()))
                    {
                        string prior = "select * from class_master where coll_code in (" + collegeCode + ") " + strmode + " order by Mode,priority";
                        ds = dt.select_method_wo_parameter(prior, "text");
                    }
                    if (chkmergrecol.Checked == true)
                    {
                        strmode = " ";
                        typaeva = " ";
                    }
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //string strdelquery = "delete es from exam_seating es,Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=et.subject_no and et.subject_no=es.subject_no and ed.degree_code=es.degree_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and ed.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.ToString() + "' " + strdateval + " ";

                        //string strdelquery = "delete es from exam_seating es,Registration r,course c,degree d,department dt,exmtt etd,exmtt_det et where c.Course_Id=d.Course_Id and d.Dept_Code=dt.Dept_Code and d.Degree_Code=r.degree_code and r.degree_code=es.degree_code and r.degree_code=etd.degree_code and es.degree_code=etd.degree_code and etd.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and r.Batch_Year=etd.batchFrom and r.Reg_No=es.regno and et.exam_code=etd.exam_code and et.exam_date=es.edate and es.ses_sion=et.exam_session and et.subject_no=es.subject_no and r.college_code=c.college_code and c.college_code=d.college_code and dt.college_code=r.college_code and c.college_code=dt.college_code and d.college_code=r.college_code  and etd.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and etd.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' " + strdateval + " " + typaeva + qryCollege;//and et.exam_date='2016-04-21' and et.exam_session='A.N'
                        //to be changed 
                        string strdelquery = "delete es from exam_seating es,course c,degree d,exmtt etd,exmtt_det et where c.Course_Id=d.Course_Id  and es.degree_code=etd.degree_code and etd.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and et.exam_code=etd.exam_code and et.exam_date=es.edate and es.ses_sion=et.exam_session and et.subject_no=es.subject_no and c.college_code=d.college_code and etd.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and etd.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' " + strdateval + " " + typaeva + qryCollege;
                        if (chkmergrecol.Checked == true)
                        {
                            typaeva = string.Empty;
                            //strdelquery = "delete es from exam_seating es,Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=et.subject_no and et.subject_no=es.subject_no and ed.degree_code=es.degree_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and ed.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.ToString() + "' " + strdateval + " " + typaeva + qryCollege + " ";
                            //strdelquery = "delete es from exam_seating es,Registration r,course c,degree d,department dt,exmtt etd,exmtt_det et where c.Course_Id=d.Course_Id and d.Dept_Code=dt.Dept_Code and d.Degree_Code=r.degree_code and r.degree_code=es.degree_code and r.degree_code=etd.degree_code and es.degree_code=etd.degree_code and etd.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and r.Batch_Year=etd.batchFrom and r.Reg_No=es.regno and et.exam_code=etd.exam_code and et.exam_date=es.edate and es.ses_sion=et.exam_session and et.subject_no=es.subject_no and r.college_code=c.college_code and c.college_code=d.college_code and dt.college_code=r.college_code and c.college_code=dt.college_code and d.college_code=r.college_code  and etd.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and etd.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' " + strdateval + " " + typaeva + qryCollege;

                            strdelquery = "delete es from exam_seating es,course c,degree d,exmtt etd,exmtt_det et where c.Course_Id=d.Course_Id  and es.degree_code=etd.degree_code and etd.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and et.exam_code=etd.exam_code and et.exam_date=es.edate and es.ses_sion=et.exam_session and et.subject_no=es.subject_no and c.college_code=d.college_code and etd.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and etd.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' " + strdateval + " " + typaeva + qryCollege;
                        }

                        //string strdelquery = "delete es from exam_seating es,Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=et.subject_no and et.subject_no=es.subject_no and ed.degree_code=es.degree_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and ed.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.ToString() + "' " + strdateval + " " + typaeva + qryCollege + " ";
                        //strdelquery = "delete es from exam_seating es,Registration r,course c,degree d,department dt,exmtt etd,exmtt_det et where c.Course_Id=d.Course_Id and d.Dept_Code=dt.Dept_Code and d.Degree_Code=r.degree_code and r.degree_code=es.degree_code and r.degree_code=etd.degree_code and es.degree_code=etd.degree_code and etd.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and r.Batch_Year=etd.batchFrom and r.Reg_No=es.regno and et.exam_code=etd.exam_code and et.exam_date=es.edate and es.ses_sion=et.exam_session and et.subject_no=es.subject_no and r.college_code=c.college_code and c.college_code=d.college_code and dt.college_code=r.college_code and c.college_code=dt.college_code and d.college_code=r.college_code  and etd.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and etd.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' " + strdateval + " " + typaeva + qryCollege;//and et.exam_date='2016-04-21' and et.exam_session='A.N'
                        //if (chkmergrecol.Checked == true)
                        //{
                        //    typaeva = string.Empty;
                        //    //strdelquery = "delete es from exam_seating es,Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=et.subject_no and et.subject_no=es.subject_no and ed.degree_code=es.degree_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and ed.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.ToString() + "' " + strdateval + " " + typaeva + qryCollege + " ";
                        //    strdelquery = "delete es from exam_seating es,Registration r,course c,degree d,department dt,exmtt etd,exmtt_det et where c.Course_Id=d.Course_Id and d.Dept_Code=dt.Dept_Code and d.Degree_Code=r.degree_code and r.degree_code=es.degree_code and r.degree_code=etd.degree_code and es.degree_code=etd.degree_code and etd.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and r.Batch_Year=etd.batchFrom and r.Reg_No=es.regno and et.exam_code=etd.exam_code and et.exam_date=es.edate and es.ses_sion=et.exam_session and et.subject_no=es.subject_no and r.college_code=c.college_code and c.college_code=d.college_code and dt.college_code=r.college_code and c.college_code=dt.college_code and d.college_code=r.college_code  and etd.Exam_month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and etd.Exam_year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' " + strdateval + " " + typaeva + qryCollege;
                        //}

                        //modified by Prabha 27/09/2017
                        strdelquery = "delete es from exam_seating es,course c,degree d where c.Course_Id=d.Course_Id   and es.degree_code=d.Degree_Code  and c.college_code=d.college_code   " + getrommdate1 + " " + typaeva + qryCollege;

                        int delq = dt.update_method_wo_parameter(strdelquery, "text");
                        Dictionary<string, int> dicdubcount = new Dictionary<string, int>();
                        DataSet ds3 = new DataSet();
                        string typeval = string.Empty;
                        if (ddltype.Items.Count > 0)
                        {
                            if (ddltype.SelectedItem.Text.ToLower() != "all")
                            {
                                if (ddltype.SelectedItem.Text != "")
                                {
                                    //typeval = "  and c.type='" + ddltype.SelectedItem.Text + "'";
                                    if (ddltype.SelectedItem.Text.Trim().ToLower() != "mca" && ddltype.SelectedItem.Text.Trim().ToLower() != "day")
                                    {
                                        //strmode = " and Mode='" + ddltype.SelectedItem.Text.Trim() + "'";
                                        typeval = "and c.type='" + ddltype.SelectedItem.Text.Trim() + "'";
                                    }
                                    else
                                    {
                                        //strmode = " and Mode in('Day','MCA')";
                                        typeval = " and c.type in('Day','MCA')";
                                    }
                                }
                            }
                        }
                        if (chkmergrecol.Checked == true)
                        {
                            typaeva = string.Empty;
                            strmode = string.Empty;
                            typeval = string.Empty;
                            qryCollege = string.Empty;
                        }
                        else
                        {
                            qryCollege = " and d.college_code in(" + collegeCode + ")";
                            if (collegeCode.Contains(","))
                            {
                                typaeva = string.Empty;
                                strmode = string.Empty;
                                typeval = string.Empty;
                            }
                        }
                        string strexamdate = " and et.exam_date='" + dateval + "'";
                        string strexamsession = string.Empty;
                        if (ddlSession.Items.Count > 0)
                        {
                            if (ddlSession.SelectedItem.Text.ToLower() != "all")
                            {
                                strexamsession = " and et.exam_session='" + ddlSession.SelectedItem.Text + "'";
                            }
                        }
                        //string roll = "select ed.subject_no,s.subject_code,s.subject_name,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code";
                        string roll = "select ed.subject_no,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym++'@mr@'+isnull(d.qpapertype,'A') end subject_codeAcr,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end QpaperType,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et, Registration r,Degree d,course c,Department de,subject s left join  tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code  ";
                        if (collegeCode.Contains(','))
                        {
                            roll = roll + " and r.College_code in (" + collegeCode + ")";
                        }
                        else
                        {
                            roll = roll + " " + typeval + "";
                        }
                        roll = roll + " and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + "  order by et.exam_date,et.exam_session,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym+'@mr@'+isnull(d.qpapertype,'A') end,e.degree_code,e.batch_year desc,r.Reg_No asc";
                        if (chkmergrecol.Checked == true)
                        {
                            typeval = string.Empty;
                            //roll = "select ed.subject_no,s.subject_code,s.subject_name,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code " + typeval + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + " order by et.exam_date,et.exam_session,c.type,s.subject_code,e.degree_code,e.batch_year desc,r.Reg_No asc";
                            roll = "select ed.subject_no,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym++'@mr@'+isnull(d.qpapertype,'A') end subject_codeAcr,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end QpaperType,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et, Registration r,Degree d,course c,Department de,subject s left join  tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code   " + typeval + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + "  order by et.exam_date,et.exam_session,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym+'@mr@'+isnull(d.qpapertype,'A') end,e.degree_code,e.batch_year desc,r.Reg_No asc";
                        }
                        DataSet dsNewStudentList = new DataSet();
                        dsNewStudentList = dt.select_method_wo_parameter(roll, "text");
                        if (dsNewStudentList.Tables.Count > 0 && dsNewStudentList.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtStudent = new DataTable();
                            dtStudent = dsNewStudentList.Tables[0].DefaultView.ToTable(true);
                            ds2.Clear();
                            ds2.Tables.Add(dtStudent);
                        }
                        else
                        {
                            ds2 = dt.select_method_wo_parameter(roll, "text");
                        }
                        DataView dv = new DataView();
                        //string exam = "select s.subject_code,s.subject_name,et.exam_date,et.exam_session,c.type,COUNT(ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + strexamdate + " " + strexamsession + " ";
                        string exam = "select case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym+'@mr@'+isnull(d.qpapertype,'A') end subject_codeAcr,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end QpaperType ,et.exam_date,et.exam_session,COUNT(distinct ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et, Registration r,Degree d,course c,Department de,subject s left join  tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id  and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + strexamdate + " " + strexamsession + " ";
                        if (collegeCode.Contains(','))
                        {
                            exam = exam + " and r.College_code in (" + collegeCode + ")";
                        }
                        else
                        {
                            exam = exam + typeval;
                        }
                        //exam = exam + typeval;
                        //exam = exam + " group by s.subject_code,s.subject_name,et.exam_date,et.exam_session,c.type order by et.exam_date,et.exam_session,c.type,stucount desc";
                        exam = exam + " group by case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym+'@mr@'+isnull(d.qpapertype,'A') end,et.exam_date,et.exam_session,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end order by et.exam_date,et.exam_session,stucount desc";
                        if (chkmergrecol.Checked == true)
                        {
                            typeval = string.Empty;
                            //exam = "select s.subject_code,s.subject_name,et.exam_date,et.exam_session,COUNT(ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + strexamdate + " " + strexamsession + " ";
                            exam = "select case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym+'@mr@'+isnull(d.qpapertype,'A') end subject_codeAcr,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end QpaperType ,et.exam_date,et.exam_session,COUNT(distinct ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et, Registration r,Degree d,course c,Department de,subject s left join  tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id  and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + strexamdate + " " + strexamsession + " ";
                            exam = exam + typeval;
                            //exam = exam + " group by s.subject_code,s.subject_name,et.exam_date,et.exam_session order by et.exam_date,et.exam_session,stucount desc";
                            exam = exam + " group by case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end,s.subject_name,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym+'@mr@'+isnull(d.qpapertype,'A') end,et.exam_date,et.exam_session,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+isnull(d.qpapertype,'A') end order by et.exam_date,et.exam_session,stucount desc";
                        }
                        ds1.Clear();
                        ds1.Reset();
                        ds1 = dt.select_method_wo_parameter(exam, "text");
                        string strgetdate = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,convert(varchar(20),et.exam_date,110) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' and et.coll_code in (" + collegeCode + ") " + strdateval + " order by  ExamDate";
                        ds3 = dt.select_method_wo_parameter(strgetdate, "text");
                        DataSet dsRoomSeatingsArrange = new DataSet();
                        string room = "select * from tbl_room_seats where coll_code in (" + collegeCode + ") " + strmode;
                        dsRoomSeatingsArrange = dt.select_method_wo_parameter(room, "text");
                        string columnName = "subject_code";
                        if (chkForSeating.Checked)
                            columnName = "subject_codeAcr";//subject_codeAcr//QpaperType
                        if (chkForSeating.Checked && CheckBox1.Checked)
                            columnName = "QpaperType";//rajkumar 02/02/2018
                        if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                        {
                            spcn = ds3.Tables[0].Rows.Count - 1;
                            DataTable dtAllDistinctSubjects = new DataTable();

                            DataTable dtAllDistinctSubjectsCommon = new DataTable();
                            dtAllDistinctSubjectsCommon.Columns.Add(columnName);
                            //dtAllDistinctSubjectsCommon.Columns.Add("subject_codeAcr");
                            dtAllDistinctSubjectsCommon.Columns.Add("DeptAcr");
                            dtAllDistinctSubjectsCommon.Columns.Add("qpaper");
                            dtAllDistinctSubjectsCommon.Columns.Add("exam_date");
                            dtAllDistinctSubjectsCommon.Columns.Add("exam_session");
                            dtAllDistinctSubjectsCommon.Columns.Add("studentCount", typeof(int));
                            dtAllDistinctSubjectsCommon.Columns.Add("subjectWiseCount", typeof(int));

                            //,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code+'@mr@'+de.dept_acronym end subject_codeAcr

                            Dictionary<string, int> dicAllsubjects = new Dictionary<string, int>();
                            Dictionary<string, int> dicInCompleteSubjects = new Dictionary<string, int>();
                            Dictionary<string, int> dicTotalStudentsForSubjects = new Dictionary<string, int>();
                            Dictionary<string, int> dicSubjectWiseTotalStudents = new Dictionary<string, int>();
                            int totalStudent = 0;
                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                            {
                                dicAllsubjects.Clear();
                                dicInCompleteSubjects.Clear();
                                dicTotalStudentsForSubjects.Clear();

                                dtAllDistinctSubjects = ds1.Tables[0].DefaultView.ToTable(true, columnName, "exam_date", "exam_session");
                                //dtAllDistinctSubjects.Columns.Add("studentCount");
                                int index = 0;
                                foreach (DataRow dr in ds1.Tables[0].Rows)
                                {
                                    string subjectCode = Convert.ToString(dr[columnName]).Trim();

                                    string subjectCodeVal = Convert.ToString(dr["subject_code"]).Trim();
                                    string subjectName = Convert.ToString(dr["subject_name"]).Trim();
                                    string examDate = Convert.ToString(dr["exam_date"]).Trim();
                                    string examSessions = Convert.ToString(dr["exam_session"]).Trim();
                                    string studentCounts = Convert.ToString(dr["stucount"]).Trim();
                                    int studentsCount = 0;
                                    int.TryParse(studentCounts, out studentsCount);
                                    totalStudent += studentsCount;
                                    if (!dicAllsubjects.ContainsKey(Convert.ToString(subjectCode).Trim().ToLower()))
                                    {
                                        dicAllsubjects.Add(Convert.ToString(subjectCode).Trim().ToLower(), index);
                                    }
                                    if (!dicInCompleteSubjects.ContainsKey(Convert.ToString(subjectCode).Trim().ToLower()))
                                    {
                                        dicInCompleteSubjects.Add(Convert.ToString(subjectCode).Trim().ToLower(), 0);
                                    }
                                    if (!dicTotalStudentsForSubjects.ContainsKey(Convert.ToString(subjectCode).Trim().ToLower()))
                                    {
                                        dicTotalStudentsForSubjects.Add(Convert.ToString(subjectCode).Trim().ToLower(), studentsCount);
                                    }
                                    else
                                    {
                                        int countValue = dicTotalStudentsForSubjects[subjectCode.Trim().ToLower()];
                                        dicTotalStudentsForSubjects[subjectCode.Trim().ToLower()] = countValue + studentsCount;
                                    }

                                    if (!dicSubjectWiseTotalStudents.ContainsKey(Convert.ToString(subjectCodeVal).Trim().ToLower()))
                                    {
                                        dicSubjectWiseTotalStudents.Add(Convert.ToString(subjectCodeVal).Trim().ToLower(), studentsCount);
                                    }
                                    else
                                    {
                                        int countValue = dicSubjectWiseTotalStudents[subjectCodeVal.Trim().ToLower()];
                                        dicSubjectWiseTotalStudents[subjectCodeVal.Trim().ToLower()] = countValue + studentsCount;
                                    }
                                    index++;
                                }
                                DataRow drStudent;
                                foreach (KeyValuePair<string, int> item in dicTotalStudentsForSubjects)
                                {
                                    drStudent = dtAllDistinctSubjectsCommon.NewRow();
                                    string subjectCode = Convert.ToString(item.Key).Trim();
                                    int studentCount = item.Value;
                                    dtAllDistinctSubjects.DefaultView.RowFilter = columnName + "='" + subjectCode + "'";
                                    DataView dvSubject = dtAllDistinctSubjects.DefaultView;
                                    //drStudent["subject_code"] = Convert.ToString(dvSubject[0]["subject_code"]).Trim();

                                    string subjectCodeValue = Convert.ToString(dvSubject[0][columnName]).Trim();
                                    string[] subDeptAcr = subjectCodeValue.Split(new string[] { "@mr@" }, StringSplitOptions.RemoveEmptyEntries);
                                    string departmentAcr = string.Empty;
                                    string qpaper = string.Empty;
                                    string subjectCode1 = subDeptAcr[0];
                                    if (subDeptAcr.Length > 1)
                                    {
                                        departmentAcr = subDeptAcr[1];
                                    }
                                    if (subDeptAcr.Length > 2)
                                    {
                                        qpaper = subDeptAcr[2];
                                    }
                                    int totalCount = studentCount;
                                    if (dicSubjectWiseTotalStudents.ContainsKey(subjectCode1.ToLower().Trim()))
                                    {
                                        totalCount = dicSubjectWiseTotalStudents[subjectCode1.ToLower().Trim()];
                                    }
                                    drStudent[columnName] = Convert.ToString(dvSubject[0][columnName]).Trim();
                                    drStudent["exam_date"] = Convert.ToString(dvSubject[0]["exam_date"]).Trim();
                                    drStudent["exam_session"] = Convert.ToString(dvSubject[0]["exam_session"]).Trim();
                                    drStudent["DeptAcr"] = departmentAcr;
                                    drStudent["qpaper"] = qpaper;
                                    drStudent["studentCount"] = Convert.ToString(studentCount);
                                    drStudent["subjectWiseCount"] = Convert.ToString(totalCount);
                                    dtAllDistinctSubjectsCommon.Rows.Add(drStudent);
                                }
                                if (dtAllDistinctSubjectsCommon.Rows.Count > 0)
                                {
                                    dtAllDistinctSubjects = new DataTable();
                                    dtAllDistinctSubjectsCommon.DefaultView.Sort = "subjectWiseCount desc,DeptAcr,qpaper,studentCount desc";
                                    dtAllDistinctSubjects = dtAllDistinctSubjectsCommon.DefaultView.ToTable();
                                }
                            }
                            int totAllotedStudents = 0;
                            int totAllotedStudentsNew = 0;
                            int totActualStudents = 0;
                            int totActualStudentsNew = 0;

                            int totalSeatingToStudent = 0;
                            if (dsRoomSeatingsArrange.Tables.Count > 0 && dsRoomSeatingsArrange.Tables[0].Rows.Count > 0)
                            {
                                object total = dsRoomSeatingsArrange.Tables[0].Compute("sum(allocted_seats)", string.Empty);
                                int.TryParse(Convert.ToString(total).Trim(), out totAllotedStudents);

                                total = dsRoomSeatingsArrange.Tables[0].Compute("sum(allotedSeatsNew)", string.Empty);
                                int.TryParse(Convert.ToString(total).Trim(), out totAllotedStudentsNew);

                                total = dsRoomSeatingsArrange.Tables[0].Compute("sum(actual_seats)", string.Empty);
                                int.TryParse(Convert.ToString(total).Trim(), out totActualStudents);

                                total = dsRoomSeatingsArrange.Tables[0].Compute("sum(actualSeatsNew)", string.Empty);
                                int.TryParse(Convert.ToString(total).Trim(), out totActualStudentsNew);
                            }
                            for (int sp = 0; sp < ds3.Tables[0].Rows.Count; sp++)
                            {
                                Dictionary<string, string> dicStudentsHall = new Dictionary<string, string>();
                                Dictionary<string, int> dicHallMaxSeatNo = new Dictionary<string, int>();
                                Dictionary<string, int> dicStudentsAlloted = new Dictionary<string, int>();
                                bool isAlternate = false;
                                bool isOne = false;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    seatno = 0;
                                    int seatingNo = 0;
                                    string halno = ds.Tables[0].Rows[i]["rno"].ToString();
                                    //string room = "select * from tbl_room_seats where Hall_No='" + halno + "' and exm_month='" + ddlMonth.SelectedValue + "' and exm_year='" + ddlYear.SelectedItem.Text + "' and coll_code='" + college_code + "' " + strmode + "";
                                    room = "select * from tbl_room_seats where Hall_No='" + halno + "' and coll_code in (" + collegeCode + ") " + strmode + "";
                                    //DataSet dsrommdet = dt.select_method_wo_parameter(room, "text");
                                    DataSet dsrommdet = new DataSet();
                                    DataTable dtRoomSeating = new DataTable();
                                    if (dsRoomSeatingsArrange.Tables.Count > 0 && dsRoomSeatingsArrange.Tables[0].Rows.Count > 0)
                                    {
                                        dsRoomSeatingsArrange.Tables[0].DefaultView.RowFilter = "Hall_No='" + halno + "' " + ((isAlternate) ? " and hasAlternate ='1'" : "");
                                        dtRoomSeating = dsRoomSeatingsArrange.Tables[0].DefaultView.ToTable();
                                    }
                                    dsrommdet.Clear();
                                    dsrommdet.Reset();
                                    dsrommdet.Tables.Add(dtRoomSeating);
                                    if (dsrommdet.Tables.Count > 0 && dsrommdet.Tables[0].Rows.Count > 0)
                                    {
                                        string floor = Convert.ToString(dsrommdet.Tables[0].Rows[0]["Floor_Name"]).Trim();
                                        norow = Convert.ToString(dsrommdet.Tables[0].Rows[0]["no_of_rows"]).Trim();
                                        string arrangeview = Convert.ToString(dsrommdet.Tables[0].Rows[0]["arranged_view"]).Trim();
                                        nocol = Convert.ToString(dsrommdet.Tables[0].Rows[0]["no_of_columns"]).Trim();
                                        string mode = Convert.ToString(dsrommdet.Tables[0].Rows[0]["mode"]).Trim();
                                        string acseat = Convert.ToString(dsrommdet.Tables[0].Rows[0]["actual_seats"]).Trim();
                                        allotseat = Convert.ToString(dsrommdet.Tables[0].Rows[0]["allocted_seats"]).Trim();
                                        string seattype = Convert.ToString(dsrommdet.Tables[0].Rows[0]["is_single"]).Trim();
                                        string month = Convert.ToString(dsrommdet.Tables[0].Rows[0]["exm_month"]).Trim();
                                        string year = Convert.ToString(dsrommdet.Tables[0].Rows[0]["exm_year"]).Trim();
                                        arrang = arrangeview.Split(';');
                                        string arrangeViewNew = Convert.ToString(dsrommdet.Tables[0].Rows[0]["arrangedViewNew"]).Trim();
                                        string actualSeats = Convert.ToString(dsrommdet.Tables[0].Rows[0]["actualSeatsNew"]).Trim();
                                        string allotedSeats = Convert.ToString(dsrommdet.Tables[0].Rows[0]["allotedSeatsNew"]).Trim();
                                        string defaultViewNew = Convert.ToString(dsrommdet.Tables[0].Rows[0]["defaultViewNew"]).Trim();
                                        if (isAlternate)
                                        {
                                            arrang = arrangeViewNew.Split(';');
                                        }
                                        Dictionary<int, int> dicsubcol = new Dictionary<int, int>();
                                        Dictionary<string, int> dicsubcolcount = new Dictionary<string, int>();
                                        for (int spr = 0; spr <= arrang.GetUpperBound(0); spr++)
                                        {
                                            string colsp = arrang[spr].ToString();
                                            if (colsp.Trim() != "" && colsp != null)
                                            {
                                                spcel = colsp.Split('-');
                                                for (int spc = 0; spc <= spcel.GetUpperBound(0); spc++)
                                                {
                                                    int colsn = Convert.ToInt32(spcel[spc]);
                                                    string strrow = "C" + spc + "R" + spr;
                                                    if (!dicsubcolcount.ContainsKey(strrow))
                                                    {
                                                        dicsubcolcount.Add(strrow, colsn);
                                                    }
                                                    if (dicsubcol.ContainsKey(spc))
                                                    {
                                                        int valc = dicsubcol[spc];
                                                        if (valc < colsn)
                                                        {
                                                            dicsubcol[spc] = colsn;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dicsubcol.Add(spc, colsn);
                                                    }
                                                }
                                            }
                                        }
                                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && dtAllDistinctSubjects.Rows.Count > 0)
                                        {
                                            string sub = string.Empty;
                                            string ses = string.Empty;
                                            string emd = string.Empty;
                                            string degcd = string.Empty;
                                            string seatValue = string.Empty;
                                            int autoChar = 97;
                                            for (int col = 0; col < Convert.ToInt32(nocol); col++)//change here 
                                            {
                                                seatValue = string.Empty;
                                                for (int row = 0; row < Convert.ToInt32(norow); row++)
                                                {

                                                    string strrow = "C" + col + "R" + row;
                                                    if (dicsubcolcount.ContainsKey(strrow))
                                                    {
                                                        int getcouv = dicsubcolcount[strrow];
                                                        int sucol = dicsubcol[col];
                                                        int recaldept = 0;
                                                        for (int subcol = 0; subcol < Convert.ToInt32(sucol); subcol++)
                                                        {
                                                            int scl = subcol;
                                                            seatValue = Convert.ToString((row + 1) + (Convert.ToInt32(norow) * subcol)) + Convert.ToString((char)autoChar);
                                                            string keyValue1 = Convert.ToString(halno.Trim() + "@" + seatValue.Trim()).Trim().ToLower();
                                                            //rowSeat++;

                                                            if (sucol > 1 && subcol > 0)//Modifed By Srinath 22 Oct 2016 
                                                            {
                                                                scl = recaldept + 1;
                                                            }

                                                            if (Radioformat2.Checked == true)//Modified by Rajkumar on 22-10-2018
                                                            {
                                                                if ((row % 2) == 1 && col % 2 == 1)
                                                                {
                                                                    scl = recaldept + sucol;
                                                                }
                                                                else if (col % 2 == 0 && (row % 2) == 0)
                                                                    scl = recaldept + sucol;//sucol//sucol
                                                            }

                                                            roww = rowcn;
                                                            int noofcheck = 0;
                                                            if (!dicStudentsHall.ContainsKey(keyValue1))
                                                            {
                                                                if (scl < dtAllDistinctSubjects.Rows.Count)
                                                                {
                                                                    string subcode = dtAllDistinctSubjects.Rows[scl][columnName].ToString();
                                                                    emd = dtAllDistinctSubjects.Rows[scl]["exam_date"].ToString();
                                                                    ses = dtAllDistinctSubjects.Rows[scl]["exam_session"].ToString();
                                                                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        ds2.Tables[0].DefaultView.RowFilter = columnName + "='" + subcode + "' and exam_date='" + emd + "' and exam_session='" + ses + "' ";
                                                                        //et.exam_date,et.exam_session,c.type,s.subject_code,e.degree_code,e.batch_year desc,r.Reg_No asc
                                                                        ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,subject_code,Course_Name,Dept_Name,batch_year desc,Reg_No asc";
                                                                        dv = ds2.Tables[0].DefaultView;
                                                                    }
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        if (subcol < getcouv)
                                                                        {
                                                                            seatno++;
                                                                            if (!dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                                                            {
                                                                                dicHallMaxSeatNo.Add(halno.Trim().ToLower(), seatno);
                                                                            }
                                                                            else
                                                                            {
                                                                                int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                                if (seatno >= seatVal)
                                                                                {
                                                                                    dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                                                }
                                                                                else
                                                                                {
                                                                                    dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                                                }
                                                                                seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                            }
                                                                        l3: if (scl < dtAllDistinctSubjects.Rows.Count)
                                                                            {
                                                                                int stuco = 0;
                                                                                if (dicdubcount.ContainsKey(subcode.ToString().Trim().ToLower()))
                                                                                {
                                                                                    stuco = dicdubcount[subcode.ToString().Trim().ToLower()];
                                                                                }
                                                                                else
                                                                                {
                                                                                    dicdubcount.Add(subcode.ToString().Trim().ToLower(), 0);
                                                                                }
                                                                                if (dv.Count > stuco)
                                                                                {
                                                                                    btngen = 1;
                                                                                    string roll1 = dv[stuco]["Reg_No"].ToString();
                                                                                    degcd = dv[stuco]["Degree_Code"].ToString();
                                                                                    sub = dv[stuco]["subject_no"].ToString();
                                                                                    // seatno++;
                                                                                    string keyValue = Convert.ToString(halno.Trim() + "@" + seatValue.Trim()).Trim().ToLower();

                                                                                    if (!dicStudentsAlloted.ContainsKey(roll1.Trim().ToLower()))
                                                                                    {
                                                                                        if (!dicStudentsHall.ContainsKey(keyValue))
                                                                                        {
                                                                                            dicStudentsHall.Add(keyValue, roll1);
                                                                                        }
                                                                                        dicStudentsAlloted.Add(roll1.Trim().ToLower(), 1);

                                                                                        string seatarrange = "if exists(select * from exam_seating where edate='" + emd + "' and ses_sion='" + ses + "' and subject_no='" + sub + "' and roomno='" + halno + "' and seat_no='" + seatno + "')delete from exam_seating where edate='" + emd + "' and ses_sion='" + ses + "' and subject_no='" + sub + "' and roomno='" + halno + "' and seat_no='" + seatno + "' insert into exam_seating (roomno,regno,subject_no,edate,ses_sion,block,seat_no,degree_code,ArrangementType,Floorid,seatCode)values('" + halno + "','" + roll1 + "','" + sub + "','" + emd + "','" + ses + "','" + floor + "','" + seatno + "','" + degcd + "',0,'" + floor + "','" + seatValue + "')  ";
                                                                                        int a = dt.update_method_wo_parameter(seatarrange, "text");
                                                                                    }
                                                                                    stuco++;
                                                                                    dicdubcount[subcode.ToString().Trim().ToLower()] = stuco;
                                                                                }
                                                                                else
                                                                                {
                                                                                  
                                                                                    if (Radioformat2.Checked)
                                                                                    {
                                                                                        recaldept++;
                                                                                        scl = recaldept + scl;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        scl++;
                                                                                    }

                                                                                    if (scl < dtAllDistinctSubjects.Rows.Count)
                                                                                    {
                                                                                        emd = dtAllDistinctSubjects.Rows[scl]["exam_date"].ToString();
                                                                                        ses = dtAllDistinctSubjects.Rows[scl]["exam_session"].ToString();
                                                                                        subcode = dtAllDistinctSubjects.Rows[scl][columnName].ToString();
                                                                                        if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                                                                        {
                                                                                            ds2.Tables[0].DefaultView.RowFilter = columnName + "='" + subcode + "' and exam_date='" + emd + "' and exam_session='" + ses + "' ";
                                                                                            ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,subject_code,Course_Name,Dept_Name,batch_year desc,Reg_No asc";
                                                                                            dv = ds2.Tables[0].DefaultView;
                                                                                        }
                                                                                        goto l3;
                                                                                    }
                                                                                    if (scl == dtAllDistinctSubjects.Rows.Count)
                                                                                    {
                                                                                        if (noofcheck == 0)
                                                                                        {
                                                                                            noofcheck = 1;
                                                                                            scl = 0;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    recaldept = scl;
                                                                }
                                                                else
                                                                {
                                                                    seatno++;
                                                                    if (!dicHallMaxSeatNo.ContainsKey(halno.Trim().ToLower()))
                                                                    {
                                                                        dicHallMaxSeatNo.Add(halno.Trim().ToLower(), seatno);
                                                                    }
                                                                    else
                                                                    {
                                                                        int seatVal = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                        if (seatno >= seatVal)
                                                                        {
                                                                            dicHallMaxSeatNo[halno.Trim().ToLower()] = seatno;
                                                                        }
                                                                        else
                                                                        {
                                                                            dicHallMaxSeatNo[halno.Trim().ToLower()] = seatVal;
                                                                        }
                                                                        seatno = dicHallMaxSeatNo[halno.Trim().ToLower()];
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                autoChar++;
                                            }
                                        }
                                        else
                                        {
                                            lblmsg.Visible = true;
                                            lblmsg.Text = ddltype.SelectedItem.Text + " " + "Type Student Not Write Exam This Date";
                                            Fpspread.Visible = false;
                                            pnlContent1.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnxl.Visible = false;
                                            btnprintmaster.Visible = false;
                                            btnDirectPrint.Visible = false;
                                            lblrptname.Visible = false;
                                            Fspread3.Visible = false;
                                            pnlContents.Visible = false;
                                            Exportexcel.Visible = false;
                                            Printfspread3.Visible = false;
                                            btn_directprint.Visible = false;
                                        }
                                    }
                                    if (ds.Tables[0].Rows.Count - 1 == i)
                                    {
                                        if (!isOne)
                                        {
                                            if (dicStudentsAlloted.Count < totalStudent)
                                            {

                                            }
                                            else
                                            {
                                            }
                                            if (totalStudent > totAllotedStudents)
                                            {
                                                if (dicStudentsAlloted.Count < totalStudent)
                                                {
                                                    i = -1;
                                                    isAlternate = true;
                                                    isOne = true;
                                                    continue;
                                                }
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                            {
                                ddlhall.Items.Clear();
                                if (chkmergrecol.Checked == true)
                                {
                                    typaeva = string.Empty;
                                    qryCollege = string.Empty;
                                }
                                else
                                {
                                    qryCollege = " and d.college_code in(" + collegeCode + ")";
                                    if (collegeCode.Contains(","))
                                    {
                                        typaeva = string.Empty;
                                    }
                                }
                                string hl = "select distinct e.roomno,cm.priority from exam_seating e,tbl_room_seats t,class_master cm,Registration r,Degree d,Course c where e.roomno=t.Hall_No and cm.rno=e.roomno and cm.rno=t.Hall_No and e.regno=r.Reg_No and r.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id " + typaeva + qryCollege + " " + getrommdate + " order by cm.priority ";
                                ds = dt.select_method_wo_parameter(hl, "text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    ddlhall.Enabled = true;
                                    ddlhall.DataSource = ds;
                                    ddlhall.DataTextField = "roomno";
                                    ddlhall.DataValueField = "roomno";
                                    ddlhall.DataBind();
                                    hallbind();
                                }
                            }
                            else
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "Please Set Hall Defination";
                                Fpspread.Visible = false;
                                pnlContent1.Visible = false;
                                txtexcelname.Visible = false;
                                btnxl.Visible = false;
                                btnprintmaster.Visible = false;
                                btnDirectPrint.Visible = false;
                                lblrptname.Visible = false;
                                Fspread3.Visible = false;
                                pnlContents.Visible = false;
                                Exportexcel.Visible = false;
                                Printfspread3.Visible = false;
                                btn_directprint.Visible = false;
                            }
                        }
                        else
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "Please Set Time Table";
                            Fpspread.Visible = false;
                            pnlContent1.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            btnprintmaster.Visible = false;
                            btnDirectPrint.Visible = false;
                            lblrptname.Visible = false;
                            Fspread3.Visible = false;
                            pnlContents.Visible = false;
                            Exportexcel.Visible = false;
                            Printfspread3.Visible = false;
                            btn_directprint.Visible = false;
                        }
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Set Hall Priority";
                        Fpspread.Visible = false;
                        pnlContent1.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        btnDirectPrint.Visible = false;
                        lblrptname.Visible = false;
                        Fspread3.Visible = false;
                        pnlContents.Visible = false;
                        Exportexcel.Visible = false;
                        Printfspread3.Visible = false;
                        btn_directprint.Visible = false;
                    }
                }
                if (Radioformat2.Checked == true)
                {
                    ddlhall.Enabled = false;
                    if (ddlhall.Items.Count == 0)
                    {
                        DataSet dn = new DataSet();
                        if (!string.IsNullOrEmpty(collegeCode.Trim()))
                        {
                            string query = "select * from class_master where coll_code in (" + collegeCode + ") order by priority";
                            dn = dt.select_method_wo_parameter(query, "text");
                        }
                    }
                    go();
                    Fpspread.Visible = false;
                    pnlContent1.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    btnDirectPrint.Visible = false;
                    lblrptname.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void btnView_Click1(object sender, EventArgs e)
    {
        try
        {
            Fpseating.Visible = false;
            Print_seating.Visible = false;
            Excel_seating.Visible = false;
            ddlhall.Enabled = true;
            Fspread3.Visible = false;
            pnlContents.Visible = false;
            Exportexcel.Visible = false;
            Printfspread3.Visible = false; btn_directprint.Visible = false;
            collegeCode = string.Empty;
            if (chkmergrecol.Checked)
            {
                collegeCode = Convert.ToString(Session["collegecode"]).Trim(); ;
            }
            else
            {
                if (cblCollege.Items.Count > 0)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
            }
            if (Radioformat3.Checked == true)
            {
                formatthree();
            }
            else
            {
                if (ddlYear.Items.Count == 0)
                {
                    Fspread3.Visible = false;
                    pnlContents.Visible = false;
                    Exportexcel.Visible = false;
                    Printfspread3.Visible = false; btn_directprint.Visible = false;
                    lblmsg.Visible = true;
                    lblmsg.Text = "Please Select The Exam Year and Then Proceed";
                    Fpspread.Visible = false;
                    pnlContent1.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    btnDirectPrint.Visible = false;
                    lblrptname.Visible = false;
                    return;
                }
                if (ddlMonth.Items.Count == 0)
                {
                    Fspread3.Visible = false;
                    pnlContents.Visible = false;
                    Exportexcel.Visible = false;
                    Printfspread3.Visible = false; btn_directprint.Visible = false;
                    lblmsg.Visible = true;
                    lblmsg.Text = "Please Select The Exam Month and Then Proceed";
                    Fpspread.Visible = false;
                    pnlContent1.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    btnDirectPrint.Visible = false;
                    lblrptname.Visible = false;
                    return;
                }
                hss = 0;
                Hashtable ht1 = new Hashtable();
                int seatno = 0;
                int rowcn = 0;
                int roww = 0;
                int spcn = 0;
                string strmode = string.Empty;
                string typaeva = string.Empty;
                if (ddltype.Items.Count > 0)
                {
                    if (ddltype.SelectedItem.Text.ToLower() != "all")
                    {
                        if (ddltype.SelectedItem.Text != "")
                        {
                            strmode = " and Mode='" + ddltype.SelectedItem.Text + "'";
                            typaeva = "and c.type='" + ddltype.SelectedItem.Text + "'";
                        }
                    }
                }
                string strdateval = string.Empty;
                string getrommdate = string.Empty;
                string dateval = string.Empty;
                if (ddlDate.Items.Count > 0)
                {
                    if (ddlDate.SelectedItem.Text.ToLower() != "all")
                    {
                        string edate = ddlDate.SelectedItem.Text.ToString();
                        string[] spd = edate.Split('-');
                        strdateval = strdateval + " and et.exam_date='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                        strdateval = strdateval + " and et.exam_session='" + ddlSession.SelectedItem.ToString() + "'";
                        getrommdate = getrommdate + " and e.edate='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                        getrommdate = getrommdate + " and e.ses_sion='" + ddlSession.SelectedItem.ToString() + "'";
                        dateval = spd[1] + '-' + spd[0] + '-' + spd[2];
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode.Trim()))
                {
                    string prior = "select * from class_master where coll_code in (" + collegeCode + ") " + strmode + " order by priority,Mode";
                    ds = dt.select_method_wo_parameter(prior, "text");
                }
                if (chkmergrecol.Checked == true)
                {
                    strmode = " ";
                    typaeva = " ";
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string strdelquery = "delete es from exam_seating es,Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=et.subject_no and et.subject_no=es.subject_no and ed.degree_code=es.degree_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and ed.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.ToString() + "' " + strdateval + " " + typaeva + " ";
                    if (chkmergrecol.Checked == true)
                    {
                        typaeva = string.Empty;
                        strdelquery = "delete es from exam_seating es,Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,Degree d,Course c where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=et.subject_no and et.subject_no=es.subject_no and ed.degree_code=es.degree_code and et.exam_date=es.edate and et.exam_session=es.ses_sion and ed.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.ToString() + "' " + strdateval + " " + typaeva + " ";
                    }
                    int delq = dt.update_method_wo_parameter(strdelquery, "text");
                    Dictionary<string, int> dicdubcount = new Dictionary<string, int>();
                    DataSet ds3 = new DataSet();
                    string typeval = string.Empty;
                    if (ddltype.Items.Count > 0)
                    {
                        if (ddltype.SelectedItem.Text.ToLower() != "all")
                        {
                            if (ddltype.SelectedItem.Text != "")
                            {
                                typeval = "  and c.type='" + ddltype.SelectedItem.Text + "'";
                            }
                        }
                    }
                    string strexamdate = " and et.exam_date='" + dateval + "'";
                    string strexamsession = string.Empty;
                    if (ddlSession.Items.Count > 0)
                    {
                        if (ddlSession.SelectedItem.Text.ToLower() != "all")
                        {
                            strexamsession = " and et.exam_session='" + ddlSession.SelectedItem.Text + "'";
                        }
                    }
                    string roll = "select ed.subject_no,s.subject_code,s.subject_name,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code " + typeval + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + " order by et.exam_date,et.exam_session,c.type,s.subject_code,e.degree_code,e.batch_year desc,r.Reg_No asc";
                    if (chkmergrecol.Checked == true)
                    {
                        typeval = string.Empty;
                        roll = "select ed.subject_no,s.subject_code,s.subject_name,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code " + typeval + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + strexamdate + " " + strexamsession + " order by et.exam_date,et.exam_session,c.type,s.subject_code,e.degree_code,e.batch_year desc,r.Reg_No asc";
                    }
                    ds2 = dt.select_method_wo_parameter(roll, "text");
                    DataView dv = new DataView();
                    string exam = "select s.subject_code,s.subject_name,et.exam_date,et.exam_session,c.type,COUNT(ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + strexamdate + " " + strexamsession + " ";
                    exam = exam + typeval;
                    exam = exam + " group by s.subject_code,s.subject_name,et.exam_date,et.exam_session,c.type order by et.exam_date,et.exam_session,c.type,stucount desc";
                    if (chkmergrecol.Checked == true)
                    {
                        typeval = string.Empty;
                        exam = "select s.subject_code,s.subject_name,et.exam_date,et.exam_session,COUNT(ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et, Registration r,Degree d,course c,Department de where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and e.Exam_Month='" + ddlMonth.SelectedValue + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + strexamdate + " " + strexamsession + " ";
                        exam = exam + typeval;
                        exam = exam + " group by s.subject_code,s.subject_name,et.exam_date,et.exam_session order by et.exam_date,et.exam_session,stucount desc";
                    }
                    ds1.Clear();
                    ds1.Reset();
                    ds1 = dt.select_method_wo_parameter(exam, "text");
                    string strgetdate = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,convert(varchar(20),et.exam_date,110) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' and et.coll_code in (" + collegeCode + ") " + strdateval + " order by  ExamDate";
                    ds3 = dt.select_method_wo_parameter(strgetdate, "text");
                    spcn = ds3.Tables[0].Rows.Count - 1;
                    if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                    {
                        for (int sp = 0; sp < ds3.Tables[0].Rows.Count; sp++)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                seatno = 0;
                                string halno = ds.Tables[0].Rows[i]["rno"].ToString();
                                //string room = "select * from tbl_room_seats where Hall_No='" + halno + "' and exm_month='" + ddlMonth.SelectedValue + "' and exm_year='" + ddlYear.SelectedItem.Text + "' and coll_code='" + college_code + "' " + strmode + "";
                                string room = "select * from tbl_room_seats where Hall_No='" + halno + "' and coll_code in (" + collegeCode + ") " + strmode + "";
                                DataSet dsrommdet = dt.select_method_wo_parameter(room, "text");
                                if (dsrommdet.Tables.Count > 0 && dsrommdet.Tables[0].Rows.Count > 0)
                                {
                                    string floor = dsrommdet.Tables[0].Rows[0]["Floor_Name"].ToString();
                                    norow = dsrommdet.Tables[0].Rows[0]["no_of_rows"].ToString();
                                    string arrangeview = dsrommdet.Tables[0].Rows[0]["arranged_view"].ToString();
                                    nocol = dsrommdet.Tables[0].Rows[0]["no_of_columns"].ToString();
                                    string mode = dsrommdet.Tables[0].Rows[0]["mode"].ToString();
                                    string acseat = dsrommdet.Tables[0].Rows[0]["actual_seats"].ToString();
                                    allotseat = dsrommdet.Tables[0].Rows[0]["allocted_seats"].ToString();
                                    string seattype = dsrommdet.Tables[0].Rows[0]["is_single"].ToString();
                                    string month = dsrommdet.Tables[0].Rows[0]["exm_month"].ToString();
                                    string year = dsrommdet.Tables[0].Rows[0]["exm_year"].ToString();
                                    arrang = arrangeview.Split(';');
                                    Dictionary<int, int> dicsubcol = new Dictionary<int, int>();
                                    Dictionary<string, int> dicsubcolcount = new Dictionary<string, int>();
                                    for (int spr = 0; spr <= arrang.GetUpperBound(0); spr++)
                                    {
                                        string colsp = arrang[spr].ToString();
                                        if (colsp.Trim() != "" && colsp != null)
                                        {
                                            spcel = colsp.Split('-');
                                            for (int spc = 0; spc <= spcel.GetUpperBound(0); spc++)
                                            {
                                                int colsn = Convert.ToInt32(spcel[spc]);
                                                string strrow = "C" + spc + "R" + spr;
                                                if (!dicsubcolcount.ContainsKey(strrow))
                                                {
                                                    dicsubcolcount.Add(strrow, colsn);
                                                }
                                                if (dicsubcol.ContainsKey(spc))
                                                {
                                                    int valc = dicsubcol[spc];
                                                    if (valc < colsn)
                                                    {
                                                        dicsubcol[spc] = colsn;
                                                    }
                                                }
                                                else
                                                {
                                                    dicsubcol.Add(spc, colsn);
                                                }
                                            }
                                        }
                                    }
                                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                                    {
                                        string sub = string.Empty;
                                        string ses = string.Empty;
                                        string emd = string.Empty;
                                        string degcd = string.Empty;
                                        for (int col = 0; col < Convert.ToInt32(nocol); col++)
                                        {
                                            for (int row = 0; row < Convert.ToInt32(norow); row++)
                                            {
                                                string strrow = "C" + col + "R" + row;
                                                if (dicsubcolcount.ContainsKey(strrow))
                                                {
                                                    int getcouv = dicsubcolcount[strrow];
                                                    int sucol = dicsubcol[col];
                                                    int recaldept = 0;
                                                    for (int subcol = 0; subcol < Convert.ToInt32(sucol); subcol++)
                                                    {
                                                        int scl = subcol;
                                                        if (subcol % 2 != 0)//Modifed By Srinath 22 Oct 2016 
                                                        //if (sucol > 1)
                                                        {
                                                            scl = 1;
                                                            DataView dvSub = new DataView();
                                                            string subcode = ds1.Tables[0].Rows[scl]["subject_code"].ToString();
                                                            emd = ds1.Tables[0].Rows[scl]["exam_date"].ToString();
                                                            ses = ds1.Tables[0].Rows[scl]["exam_session"].ToString();
                                                            ds2.Tables[0].DefaultView.RowFilter = "subject_code='" + subcode + "' and exam_date='" + emd + "' and exam_session='" + ses + "' ";
                                                            //ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,type,subject_code,degree_code,batch_year desc,Reg_No asc";
                                                            ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,subject_code,Course_Name,Dept_Name,batch_year desc,Reg_No asc";
                                                            dvSub = ds2.Tables[0].DefaultView;
                                                            if (dvSub.Count > 0)
                                                            {
                                                                if (subcol < getcouv)
                                                                {
                                                                    if (dicdubcount.ContainsKey(subcode.ToString().Trim().ToLower()))
                                                                    {
                                                                        int newcount = dicdubcount[subcode.ToString().Trim().ToLower()];
                                                                        if (newcount == dvSub.Count)
                                                                        {
                                                                            scl += 2;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        dicdubcount.Add(subcode.ToString().Trim().ToLower(), 0);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            scl = 0;
                                                            DataView dvSub = new DataView();
                                                            string subcode = ds1.Tables[0].Rows[scl]["subject_code"].ToString();
                                                            emd = ds1.Tables[0].Rows[scl]["exam_date"].ToString();
                                                            ses = ds1.Tables[0].Rows[scl]["exam_session"].ToString();
                                                            ds2.Tables[0].DefaultView.RowFilter = "subject_code='" + subcode + "' and exam_date='" + emd + "' and exam_session='" + ses + "' ";
                                                            //ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,type,subject_code,degree_code,batch_year desc,Reg_No asc";
                                                            ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,subject_code,Course_Name,Dept_Name,batch_year desc,Reg_No asc";
                                                            dvSub = ds2.Tables[0].DefaultView;
                                                            if (dvSub.Count > 0)
                                                            {
                                                                if (subcol < getcouv)
                                                                {
                                                                    if (dicdubcount.ContainsKey(subcode.ToString().Trim().ToLower()))
                                                                    {
                                                                        int newcount = dicdubcount[subcode.ToString().Trim().ToLower()];
                                                                        if (newcount == dvSub.Count)
                                                                        {
                                                                            scl += 2;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        dicdubcount.Add(subcode.ToString().Trim().ToLower(), 0);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (Radioformat2.Checked == true)
                                                        {
                                                            if ((row % 2) == 1)
                                                            {
                                                                scl = recaldept + sucol;
                                                            }
                                                        }
                                                        roww = rowcn;
                                                        int noofcheck = 0;
                                                        if (scl < ds1.Tables[0].Rows.Count)
                                                        {
                                                            string subcode = ds1.Tables[0].Rows[scl]["subject_code"].ToString();
                                                            emd = ds1.Tables[0].Rows[scl]["exam_date"].ToString();
                                                            ses = ds1.Tables[0].Rows[scl]["exam_session"].ToString();
                                                            ds2.Tables[0].DefaultView.RowFilter = "subject_code='" + subcode + "' and exam_date='" + emd + "' and exam_session='" + ses + "' ";
                                                            //ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,type,subject_code,degree_code,batch_year desc,Reg_No asc";
                                                            ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,subject_code,Course_Name,Dept_Name,batch_year desc,Reg_No asc";
                                                            dv = ds2.Tables[0].DefaultView;
                                                            if (dv.Count > 0)
                                                            {
                                                                if (subcol < getcouv)
                                                                {
                                                                    seatno++;
                                                                l3: if (scl < ds1.Tables[0].Rows.Count)
                                                                    {
                                                                        int stuco = 0;
                                                                        if (dicdubcount.ContainsKey(subcode.ToString().Trim().ToLower()))
                                                                        {
                                                                            stuco = dicdubcount[subcode.ToString().Trim().ToLower()];
                                                                        }
                                                                        else
                                                                        {
                                                                            dicdubcount.Add(subcode.ToString().Trim().ToLower(), 0);
                                                                        }
                                                                        if (dv.Count > stuco)
                                                                        {
                                                                            btngen = 1;
                                                                            string roll1 = dv[stuco]["Reg_No"].ToString();
                                                                            degcd = dv[stuco]["Degree_Code"].ToString();
                                                                            sub = dv[stuco]["subject_no"].ToString();
                                                                            // seatno++;
                                                                            string seatarrange = "if exists(select * from exam_seating where edate='" + emd + "' and ses_sion='" + ses + "' and subject_no='" + sub + "' and roomno='" + halno + "' and seat_no='" + seatno + "')delete from exam_seating where edate='" + emd + "' and ses_sion='" + ses + "' and subject_no='" + sub + "' and roomno='" + halno + "' and seat_no='" + seatno + "' insert into exam_seating (roomno,regno,subject_no,edate,ses_sion,block,seat_no,degree_code,ArrangementType,Floorid)values('" + halno + "','" + roll1 + "','" + sub + "','" + emd + "','" + ses + "','" + floor + "','" + seatno + "','" + degcd + "',0,'" + floor + "')  ";
                                                                            int a = dt.update_method_wo_parameter(seatarrange, "text");
                                                                            stuco++;
                                                                            dicdubcount[subcode.ToString().Trim().ToLower()] = stuco;
                                                                        }
                                                                        else
                                                                        {
                                                                            scl++;
                                                                            if (scl < ds1.Tables[0].Rows.Count)
                                                                            {
                                                                                emd = ds1.Tables[0].Rows[scl]["exam_date"].ToString();
                                                                                ses = ds1.Tables[0].Rows[scl]["exam_session"].ToString();
                                                                                subcode = ds1.Tables[0].Rows[scl]["subject_code"].ToString();
                                                                                ds2.Tables[0].DefaultView.RowFilter = "subject_code='" + subcode + "' and exam_date='" + emd + "' and exam_session='" + ses + "' ";
                                                                                //ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,type,subject_code,degree_code,batch_year desc,Reg_No asc";
                                                                                ds2.Tables[0].DefaultView.Sort = "exam_date,exam_session,subject_code,Course_Name,Dept_Name,batch_year desc,Reg_No asc";
                                                                                dv = ds2.Tables[0].DefaultView;
                                                                                goto l3;
                                                                            }
                                                                            if (scl == ds1.Tables[0].Rows.Count)
                                                                            {
                                                                                if (noofcheck == 0)
                                                                                {
                                                                                    noofcheck = 1;
                                                                                    scl = 0;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            recaldept = scl;
                                                        }
                                                        else
                                                        {
                                                            seatno++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lblmsg.Visible = true;
                                        lblmsg.Text = ddltype.SelectedItem.Text + " " + "Type Student Not Write Exam This Date";
                                        Fpspread.Visible = false;
                                        pnlContent1.Visible = false;
                                        txtexcelname.Visible = false;
                                        btnxl.Visible = false;
                                        btnprintmaster.Visible = false;
                                        btnDirectPrint.Visible = false;
                                        lblrptname.Visible = false;
                                        Fspread3.Visible = false;
                                        pnlContents.Visible = false;
                                        Exportexcel.Visible = false;
                                        Printfspread3.Visible = false;
                                        btn_directprint.Visible = false;
                                    }
                                }
                            }
                        }
                        if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                        {
                            ddlhall.Items.Clear();
                            if (chkmergrecol.Checked == true)
                            {
                                typaeva = string.Empty;
                            }
                            string hl = "select distinct e.roomno,cm.priority from exam_seating e, tbl_room_seats t,class_master cm,Registration r,Degree d,Course c where e.roomno=t.Hall_No and cm.rno=e.roomno and cm.rno=t.Hall_No and e.regno=r.Reg_No and r.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id " + typaeva + " " + getrommdate + " order by cm.priority ";
                            ds = dt.select_method_wo_parameter(hl, "text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                ddlhall.Enabled = true;
                                ddlhall.DataSource = ds;
                                ddlhall.DataTextField = "roomno";
                                ddlhall.DataValueField = "roomno";
                                ddlhall.DataBind();
                                hallbind();
                            }
                        }
                        else
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "Please Set Hall Defination";
                            Fpspread.Visible = false;
                            pnlContent1.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            btnprintmaster.Visible = false;
                            btnDirectPrint.Visible = false;
                            lblrptname.Visible = false;
                            Fspread3.Visible = false;
                            pnlContents.Visible = false;
                            Exportexcel.Visible = false;
                            Printfspread3.Visible = false;
                            btn_directprint.Visible = false;
                        }
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Set Time Table";
                        Fpspread.Visible = false;
                        pnlContent1.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        btnDirectPrint.Visible = false;
                        lblrptname.Visible = false;
                        Fspread3.Visible = false;
                        pnlContents.Visible = false;
                        Exportexcel.Visible = false;
                        Printfspread3.Visible = false; btn_directprint.Visible = false;
                    }
                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Please Set Hall Priority";
                    Fpspread.Visible = false;
                    pnlContent1.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    btnDirectPrint.Visible = false;
                    lblrptname.Visible = false;
                    Fspread3.Visible = false;
                    pnlContents.Visible = false;
                    Exportexcel.Visible = false;
                    Printfspread3.Visible = false; btn_directprint.Visible = false;
                }
            }
            if (Radioformat2.Checked == true)
            {
                ddlhall.Enabled = false;
                if (ddlhall.Items.Count == 0)
                {
                    DataSet dn = new DataSet();
                    if (!string.IsNullOrEmpty(collegeCode.Trim()))
                    {
                        string query = "select * from class_master where coll_code in (" + collegeCode + ") order by priority";
                        dn = dt.select_method_wo_parameter(query, "text");
                    }
                }
                go();
                Fpspread.Visible = false;
                pnlContent1.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                btnDirectPrint.Visible = false;
                lblrptname.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexcelname.Text;
            if (report.ToString().Trim() != "")
            {
                dt.printexcelreport(Fpspread, report);
                lblmessage1.Visible = false;
            }
            else
            {
                lblmessage1.Text = "Please Enter Your Report Name";
                lblmessage1.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            string date = ddlDate.SelectedItem.Text;
            string pagename = "seatingarrange.aspx";
            string degreedetails = "Office of the Controller of Examinations $SEATING ARRANGEMENT";
            degreedetails = degreedetails + "$Examination " + ddlMonth.SelectedItem.ToString() + " " + ddlYear.SelectedItem.ToString();
            if (ddlDate.SelectedItem.Text != "All")
            {
                degreedetails = degreedetails + "@Date & Session : " + ddlDate.SelectedItem.ToString() + " & " + ddlSession.SelectedItem.ToString() + "";
            }
            //if (Radioformat3.Checked == true)
            //{
            degreedetails = degreedetails + "@Hall No : " + ddlhall.SelectedItem.ToString() + "";
            // }
            Printcontrol.loadspreaddetails(Fpspread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void go()
    {
        try
        {
            DataSet dsCollege = new DataSet();
            collegeCode = string.Empty;
            if (chkmergrecol.Checked)
            {
                collegeCode = Convert.ToString(Session["collegecode"]).Trim(); ;
            }
            else
            {
                if (cblCollege.Items.Count > 0)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
            }
            if (!string.IsNullOrEmpty(collegeCode.Trim()))
            {
                string qrynew = "select *,district+' - '+pincode  as districtpin from collinfo where college_code in(" + collegeCode + ")";
                dsCollege = dt.select_method_wo_parameter(qrynew, "Text");
            }
            if (Radioformat2.Checked == true)
            {
                hallbind();
                DataSet dhall = new DataSet();
                Hashtable hat = new Hashtable();
                Fpspread.Visible = false;
                pnlContent1.Visible = false;
                Fpseating.Visible = false;
                btnprintmaster.Visible = false;
                btnDirectPrint.Visible = false;
                btnxl.Visible = false;
                int b = 0;
                string regno = string.Empty;
                int add = 0;
                int totstu = 0;
                string halldate = string.Empty;
                string strdateval = string.Empty;
                string typequery = string.Empty;
                string acroymn = string.Empty;
                string deg = string.Empty;
                string answer = string.Empty;
                string booklet = string.Empty;
                string roomno = string.Empty;
                string blockNo = string.Empty;
                string subjectcode = string.Empty;
                DataSet ds3 = new DataSet();
                Fpspread.Visible = false;
                pnlContent1.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                btnDirectPrint.Visible = false;
                TableRow tr = new TableRow();
                string rollStart = string.Empty;
                string rollEnd = string.Empty;
                //TableCell tcell = new TableCell();
                //tcell.Text = "S.No";
                //tcell.Width = 30;
                //tr.Cells.Add(tcell);
                //tcell = new TableCell();
                //tcell.Text = "Invigilator Name";
                //tcell.Width = 69;
                //tr.Cells.Add(tcell);
                //tcell = new TableCell();
                //tcell.Text = "Hall No";
                //tcell.Width = 65;
                //tr.Cells.Add(tcell);
                //tcell = new TableCell();
                //tcell.Text = "Initials of the Invigilator";
                //tcell.Width = 65;
                //tr.Cells.Add(tcell);
                //tcell = new TableCell();
                //tcell.Text = "Degree/Branch ";
                //tcell.Width = 105;
                //tr.Cells.Add(tcell);
                //tcell = new TableCell();
                //tcell.Text = "Subject Code";
                //tcell.Width = 80;
                //tr.Cells.Add(tcell);
                //tcell = new TableCell();
                //tcell.Text = "Reg. No of the Candidate";
                //tcell.Width = 380;
                //tr.Cells.Add(tcell);
                //tcell = new TableCell();
                //tcell.Text = "Total No of Student";
                //tcell.Width = 70;
                //tr.Cells.Add(tcell);
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                MyStyle.Font.Size = FontUnit.Medium;
                MyStyle.Font.Name = "Book Antiqua";
                MyStyle.Font.Bold = true;
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Fspread3.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Invigilator Name";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hall No";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Initials of the Invigilator";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree/Branch ";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Code";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Reg. No of the Candidate";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total No of Student";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Answer Booklet Numbers";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Signature of the Hall Superintendents";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Present";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Absent";
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Bundle No";
                Fspread3.Sheets[0].Columns[12].Visible = false;
                tcBundleNo.Visible = false;
                if (chkShowBundleNo.Checked)
                {
                    Fspread3.Sheets[0].Columns[12].Visible = true;
                    tcBundleNo.Visible = true;
                }
                Fspread3.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Initials of the Invigilator";
                Fspread3.Sheets[0].ColumnHeader.Visible = false;
                Fspread3.Sheets[0].Columns[0].Width = 50;
                Fspread3.Sheets[0].Columns[1].Width = 100;
                Fspread3.Sheets[0].Columns[2].Width = 100;
                Fspread3.Sheets[0].Columns[3].Width = 100;
                Fspread3.Sheets[0].Columns[4].Width = 100;
                Fspread3.Sheets[0].Columns[5].Width = 110;
                Fspread3.Sheets[0].Columns[6].Width = 550;
                Fspread3.Sheets[0].Columns[7].Width = 100;
                ArrayList regstuduniq = new ArrayList();
                //  Fspread3.Width = 1024;
                if (Chksetting.Checked == true)
                {
                    Fspread3.Sheets[0].Columns[8].Visible = true;
                    Fspread3.Sheets[0].Columns[9].Visible = true;
                    tcBooletNo.Visible = true;
                    tcHallSuperend.Visible = true;
                    tblcellsno.Width = 30;
                    tblcellInvName.Width = 80;
                    tblcellHallNo.Width = 99;
                    tcInvSign.Width = 85;
                    TableCell4.Width = 95;
                    TableCell6.Width = 105;
                    TableCell7.Width = 550;
                    TableCell8.Width = 44;
                    tcBooletNo.Width = 40;
                    tcHallSuperend.Width = 24;
                    tcBundleNo.Width = 48;
                    TableCell11.Width = 83;
                    TableCell12.Width = 55;
                    TableCell13.Width = 65;
                    //tcBooletNo.Attributes.Add("style", "display:block;");
                    //tcHallSuperend.Attributes.Add("style", "display:block;");
                    //tcell = new TableCell();
                    //tcell.Text = "Answer Booklet Numbers";
                    //tcell.Width = 40;
                    //tr.Cells.Add(tcell);
                    //tcell = new TableCell();
                    //tcell.Text = "Signature of the Hall Superintendents";
                    //tcell.Width = 40;
                    //tr.Cells.Add(tcell);
                }
                else
                {
                    Fspread3.Sheets[0].Columns[8].Visible = false;
                    Fspread3.Sheets[0].Columns[9].Visible = false;
                    tcBooletNo.Visible = false;
                    tcHallSuperend.Visible = false;
                    //tcBooletNo.Attributes.Add("style", "display:none;");
                    //tcHallSuperend.Attributes.Add("style", "display:none;");
                }
                //tcell = new TableCell();
                //tcell.Text = "Present";
                //tcell.Width = 55;
                //tr.Cells.Add(tcell);
                //tcell = new TableCell();
                //tcell.Text = "Absent";
                //tcell.Width = 55;
                //tr.Cells.Add(tcell);
                //tcell = new TableCell();
                //tcell.Text = "Initials of the Invigilator";
                //tcell.Width = 65;
                //tr.Cells.Add(tcell);
                Fspread3.Sheets[0].RowCount = 0;
                //tblFormat2.Rows.Add(tr);
                if (ddlDate.Items.Count > 0)
                {
                    if (ddlDate.SelectedItem.Text.ToLower().Trim() != "all")
                    {
                        string edate = ddlDate.SelectedItem.Text.ToString().Trim();
                        string[] spd = edate.Split('-');
                        strdateval = strdateval + " and et.exam_date='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                    }
                }
                if (ddlSession.Items.Count > 0)
                {
                    if (ddlSession.SelectedItem.Text.ToLower().Trim() != "all" && ddlSession.SelectedItem.Text.ToLower().Trim() != "both" && ddlSession.SelectedItem.Text.ToLower().Trim() != "")
                    {
                        strdateval = strdateval + " and et.exam_session='" + ddlSession.SelectedItem.ToString() + "'";
                    }
                }
                string strgetdate = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,convert(varchar(20),et.exam_date,110) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' and et.coll_code in(" + collegeCode + ") " + strdateval + " order by  ExamDate";
                ds3 = dt.select_method_wo_parameter(strgetdate, "text");
                if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                {
                    string strexamdate = " and et.exam_date='" + Convert.ToString(ds3.Tables[0].Rows[0]["edate"]).Trim() + "'";
                    if (ddltype.SelectedItem.Text.ToLower().Trim() != "all")
                    {
                        if (ddltype.SelectedItem.Text.Trim().ToLower() != "")
                        {
                            typequery = "and c.type='" + ddltype.SelectedItem.Text.Trim() + "'";
                        }
                    }
                    if (chkmergrecol.Checked == true)
                    {
                        typequery = string.Empty;
                    }
                    if (ddlDate.SelectedItem.Text.ToLower().Trim() != "all" && ddlDate.SelectedItem.Text.ToLower().Trim() != "")
                    {
                        string edate = ddlDate.SelectedItem.Text.ToString().Trim();
                        string[] spd = edate.Split('-');
                        halldate = spd[1] + "/" + spd[0] + "/" + spd[2];
                    }
                }
                else
                {
                    lblmsg.Text = "Please Set Time Table";
                    lblmsg.Visible = true;
                }
                string totstu1 = string.Empty;
                string block = string.Empty;

                //string hl = "select distinct es.roomno, cm.priority from exmtt e,exmtt_det et,exam_seating es,course c,Degree d,class_master cm where cm.rno=es.roomno and c.type=cm.Mode and e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id " + typequery + " and e.Exam_year='" + ddlYear.SelectedItem.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.ToString() + "'  order by  cm.priority";
                string selBlock = string.Empty;
                string qryBlock = string.Empty;
                if (Radioformat2.Checked)
                {
                    if (cblBlock.Items.Count > 0)
                    {
                        selBlock = getCblSelectedValue(cblBlock);
                    }
                    if (!string.IsNullOrEmpty(selBlock))
                        qryBlock = " and cs.block in(" + selBlock + ")";
                }
                string hl = "select distinct es.roomno,cs.block,cs.priority from exam_seating es,class_master cs where cs.rno=es.roomno and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.ToString().Trim() + "' and cs.coll_code in(" + collegeCode + ")" + qryBlock + "   order by  cs.priority";
                ds2 = dt.select_method_wo_parameter(hl, "text");

                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    for (int f = 0; f < ds2.Tables[0].Rows.Count; f++)
                    {
                        totstu = 0;
                        int rowcount = 0;
                        roomno = ds2.Tables[0].Rows[f]["roomno"].ToString();
                        block = ds2.Tables[0].Rows[f]["block"].ToString();
                        if (chkIncludeBlock.Checked == true)
                        {
                            blockNo = "(" + block + ")";
                        }
                        if (ddltype.SelectedItem.Text.Trim().ToLower() != "all")
                        {
                            if (ddltype.SelectedItem.Text.Trim() != "")
                            {
                                typequery = "and c.type='" + ddltype.SelectedItem.Text.Trim() + "'";
                            }
                        }

                        string qryCollege = string.Empty;
                        if (chkmergrecol.Checked == true)
                        {
                            typequery = string.Empty;
                            qryCollege = string.Empty;
                        }
                        else
                        {
                            qryCollege = " and d.college_code in(" + collegeCode + ") ";
                            if (collegeCode.Contains(","))
                            {
                                typequery = string.Empty;
                            }
                        }
                        string query = "select distinct es.roomno,c.Course_Name,de.Dept_Name,d.Degree_Code,s.subject_code,d.Acronym,es.edate,es.ses_sion from exmtt e,exmtt_det et,exam_seating es,course c,Degree d,Department de,subject s where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and d.Dept_Code=de.Dept_Code and es.subject_no=s.subject_no  and et.subject_no=s.subject_no  and e.Exam_year='" + ddlYear.SelectedItem.Text + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and es.edate='" + halldate + "' and  es.ses_sion='" + ddlSession.SelectedItem.Text.Trim() + "'  and es.roomno='" + roomno + "' " + qryCollege + typequery + " order by es.edate,es.ses_sion,es.roomno,s.subject_code,c.Course_Name,de.Dept_Name";
                        //string query = "select distinct es.roomno,c.Course_Name,de.Dept_Name,s.subject_code,es.edate,es.ses_sion,COUNT(es.seat_no) from exmtt e,exmtt_det et,exam_seating es,course c,Degree d,Department de,subject s where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and d.Dept_Code=de.Dept_Code and es.subject_no=s.subject_no  and et.subject_no=s.subject_no  and e.Exam_year='" + ddlYear.SelectedItem.Text + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and es.edate='" + halldate + "' and  es.ses_sion='" + ddlSession.SelectedItem.Text.Trim() + "'  and es.roomno='" + roomno + "' " + qryCollege + typequery + " group by es.roomno,c.Course_Name,de.Dept_Name,s.subject_code,es.edate,es.ses_sion  order by COUNT(es.seat_no) desc, es.edate,es.ses_sion,es.roomno,s.subject_code,c.Course_Name,de.Dept_Name";

                        qryCollege = string.Empty;
                        if (chkmergrecol.Checked == true)
                        {
                            typequery = string.Empty;
                            qryCollege = string.Empty;
                        }
                        else
                        {
                            qryCollege = " and r.college_code in(" + collegeCode + ") ";
                            if (collegeCode.Contains(","))
                            {
                                typequery = string.Empty;
                            }
                        }
                        int total = 1;
                        DataSet ds = new DataSet();
                        ds = dt.select_method_wo_parameter(query, "Text");
                        rowcount = ds.Tables[0].Rows.Count;
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            total = 1;
                            if (dsCollege.Tables.Count > 0 && dsCollege.Tables[0].Rows.Count > 0)
                            {
                                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(Convert.ToString(ddlMonth.SelectedItem.Value).Trim()));
                                string[] strpa = Convert.ToString(dsCollege.Tables[0].Rows[0]["affliatedby"]).Trim().Split(',');
                                //aff = strpa[0];
                                spCollege.InnerText = Convert.ToString(dsCollege.Tables[0].Rows[0]["Collname"]).Trim();
                                spController.InnerText = "OFFICE OF THE CONTROLLER OF EXAMINATIONS";
                                spSeating.InnerText = "SEATING ARRANGEMENT  -  " + strMonthName.ToUpper() + " " + Convert.ToString(ddlYear.SelectedItem.Text);
                                spAffBy.InnerText = (strpa.Length > 0) ? Convert.ToString(strpa[0]).Trim() : Convert.ToString(dsCollege.Tables[0].Rows[0]["affliatedby"]).Trim();
                                spDateSession.InnerText = "Date & Session : " + Convert.ToString(ddlDate.SelectedItem.Text).Trim() + " & " + Convert.ToString(ddlSession.SelectedItem.Text).Trim();
                            }
                            Hashtable hatDicroomsub = new Hashtable();
                            int sno = 0;
                            int row = 0;
                            for (int room = 0; room < ds.Tables[0].Rows.Count; room++)
                            {
                                regno = string.Empty;
                                ArrayList arrBundleNo = new ArrayList();
                                deg = ds.Tables[0].Rows[room]["Course_Name"].ToString();
                                string degreecode = ds.Tables[0].Rows[room]["Degree_Code"].ToString();
                                acroymn = ds.Tables[0].Rows[room]["Dept_Name"].ToString();
                                subjectcode = ds.Tables[0].Rows[room]["subject_code"].ToString();

                                string deptName = roomno + subjectcode + acroymn;
                                if (!hatDicroomsub.ContainsKey(deptName))
                                {
                                    sno = sno + 1;
                                    hatDicroomsub.Add(deptName, room);
                                    //string h1 = "select distinct r.Reg_No, es.seat_no,es.bundle_no from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and es.subject_no=s.subject_no and ead.subject_no=s.subject_no and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.Text + "' and ed.degree_code='" + degreecode + "' and es.roomno='" + roomno + "' and s.subject_code='" + subjectcode + "' " + qryCollege + " order by r.Reg_No"; ,len(Reg_No) regLen,SUBSTRING(Reg_No,1,len(Reg_No)-3) AS Prefix,SUBSTRING(Reg_No,len(Reg_No)-2,3) as Suffix
                                    string h1 = "select distinct r.Reg_No,'' regLen,'' AS Prefix,'' as Suffix,r.Batch_Year,r.degree_code,r.Current_Semester,case when r.mode='1' then 'Regular' when r.mode='2' then 'Transfer' when r.mode='3' then 'Lateral' end as Mode,r.mode as ModeVal,r.isRedo,Convert(int,DATEPART(year,r.Adm_Date)) AS tempBatch, es.seat_no,es.bundle_no,Course_Name,Dept_Name from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,subject s,Registration r,Degree d,Department de,course c where d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and es.subject_no=s.subject_no and ead.subject_no=s.subject_no and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.Text + "'  and es.roomno='" + roomno + "' and s.subject_code='" + subjectcode + "' and Dept_Name='" + acroymn + "'  " + qryCollege + " order by r.Reg_No";//and ed.degree_code='" + degreecode + "'  

                                    h1 = h1 + "    select count( r.Reg_No) as totstu from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,subject s,Registration r,Degree d,Department de,course c where d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and r.degree_code=d.Degree_Code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and es.subject_no=s.subject_no and ead.subject_no=s.subject_no and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.Text + "' and  es.roomno='" + roomno + "' and s.subject_code='" + subjectcode + "' and Dept_Name='" + acroymn + "' " + qryCollege + "";

                                    ////ed.degree_code='" + degreecode + "'  and
                                    dhall = dt.select_method_wo_parameter(h1, "Text");
                                    //string deptNameAcr = " select distinct (Course_Name+'-'+Dept_Name) as cname from exam_seating es,Degree d,Department de,course c,subject s where  s.subject_no=es.subject_no  and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and d.Degree_Code=es.degree_code and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.Text + "' and  es.roomno='" + roomno + "' and s.subject_code='" + subjectcode + "'";
                                    //DataTable dtdicdept = dirAcc.selectDataTable(deptNameAcr);
                                    //string deptName1 = string.Empty;
                                    //if (dtdicdept.Rows.Count > 0)
                                    //{
                                    //    foreach (DataRow dt1 in dtdicdept.Rows)
                                    //    {
                                    //        string DEPT =Convert.ToString(dt1["cname"]);
                                    //        if (string.IsNullOrEmpty(deptName1))
                                    //            deptName1 = DEPT;
                                    //        else
                                    //            deptName1 = deptName1 + " & " + DEPT;
                                    //    }
                                    //}
                                    Dictionary<string, Dictionary<string, string>> dicBatchRegNo = new Dictionary<string, Dictionary<string, string>>();
                                    Dictionary<string, Dictionary<string, int[]>> dicBatchRegNoWithPreSuff = new Dictionary<string, Dictionary<string, int[]>>();
                                    Hashtable hatdicstu = new Hashtable();
                                    bool isRows = false;
                                   
                                    if (dhall.Tables.Count > 0 && dhall.Tables[0].Rows.Count > 0)
                                    {
                                        row++;
                                        Fspread3.Sheets[0].RowCount++;
                                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row);
                                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 2].Text = roomno.ToString() + blockNo.ToString();
                                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = deg + " - " + acroymn.ToString();
                                        // Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = deptName1;
                                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 5].Text = subjectcode.ToString();
                                        Dictionary<string, string> dicRegNoList = new Dictionary<string, string>();
                                        Dictionary<string, int[]> dicRegNoWithPreSuff = new Dictionary<string, int[]>();
                                        for (b = 0; b < dhall.Tables[0].Rows.Count; b++)
                                        {
                                            isRows = true;
                                           
                                            string bundleNo = Convert.ToString(dhall.Tables[0].Rows[b]["bundle_no"]).Trim();
                                            string regNos = Convert.ToString(dhall.Tables[0].Rows[b]["Reg_No"]).Trim().ToLower();
                                            string batchYear = Convert.ToString(dhall.Tables[0].Rows[b]["Batch_Year"]).Trim().ToLower();
                                            string lastNo = Convert.ToString(dhall.Tables[0].Rows[b]["Suffix"]).Trim().ToLower();
                                            string firstNo = Convert.ToString(dhall.Tables[0].Rows[b]["Prefix"]).Trim().ToLower();
                                            int studentLastNo = 0;
                                            lastNo = lastNo.TrimStart('0');
                                            int.TryParse(lastNo, out studentLastNo);
                                            //int studentFirstNo = 0;
                                            //int.TryParse(firstNo, out studentFirstNo);
                                            if (dicRegNoList.ContainsKey(regNos))
                                                dicRegNoList[regNos] = lastNo;
                                            else
                                                dicRegNoList.Add(regNos, lastNo);

                                            int[] last = new int[0];
                                            Array.Resize(ref last, last.Length + 1);
                                            last[last.Length - 1] = studentLastNo;
                                            if (dicRegNoWithPreSuff.ContainsKey(firstNo))
                                            {
                                                int[] last1 = new int[0];
                                                last1 = dicRegNoWithPreSuff[firstNo];
                                                Array.Resize(ref last1, last1.Length + 1);
                                                last1[last1.Length - 1] = studentLastNo;
                                                dicRegNoWithPreSuff[firstNo] = last1;
                                            }
                                            else
                                            {
                                                dicRegNoWithPreSuff.Add(firstNo, last);
                                            }

                                            if (dicBatchRegNo.ContainsKey(batchYear))
                                                dicBatchRegNo[batchYear] = dicRegNoList;
                                            else
                                                dicBatchRegNo.Add(batchYear, dicRegNoList);

                                            if (dicBatchRegNoWithPreSuff.ContainsKey(batchYear))
                                                dicBatchRegNoWithPreSuff[batchYear] = dicRegNoWithPreSuff;
                                            else
                                                dicBatchRegNoWithPreSuff.Add(batchYear, dicRegNoWithPreSuff);

                                            if (!string.IsNullOrEmpty(bundleNo))
                                            {
                                                if (!arrBundleNo.Contains(bundleNo.Trim()))
                                                {
                                                    arrBundleNo.Add(bundleNo);
                                                }
                                            }
                                            if (!regstuduniq.Contains(dhall.Tables[0].Rows[b]["Reg_No"].ToString()))
                                            {
                                                if (regno == "")
                                                {
                                                    regno = dhall.Tables[0].Rows[b]["Reg_No"].ToString() + "  " + " [ " + dhall.Tables[0].Rows[b]["seat_no"].ToString() + " ] ";
                                                }
                                                else
                                                {
                                                    regno = regno + ",  " + dhall.Tables[0].Rows[b]["Reg_No"].ToString() + "  " + " [ " + dhall.Tables[0].Rows[b]["seat_no"].ToString() + " ] ";
                                                    total++;
                                                }
                                                regstuduniq.Add(dhall.Tables[0].Rows[b]["Reg_No"].ToString());
                                            }
                                        }
                                        //Fspread3.SaveChanges();
                                    }
                                    totstu1 = dhall.Tables[1].Rows[0]["totstu"].ToString();
                                    //string DicSub = roomno + "-" + subjectcode;
                                    //if (!hatdicstu.ContainsKey(DicSub))
                                    //{
                                    //    totstu = totstu + Convert.ToInt32(totstu1);
                                    //}
                                    //

                                    //Command by rajkumar for jamal
                                    
                                    if (!hat.ContainsKey(roomno))
                                    {
                                        hat.Add(roomno, totstu1);
                                        add = 0;
                                        add = add + Convert.ToInt32(totstu1);
                                    }
                                    else
                                    {
                                        add = add + Convert.ToInt32(totstu1);
                                    }
                                    if (chkIncludeBlock.Checked)
                                    {
                                        DataTable dicBatchwiseStudent = new DataTable();
                                        DataTable dtStroll = new DataTable();
                                        //bool listOrComma=false;
                                        //DataTable dtEndroll = new DataTable();
                                        string startNo = string.Empty;
                                        string endNo = string.Empty;

                                        string prefix = string.Empty;
                                        string sufix = string.Empty;

                                        foreach (KeyValuePair<string, Dictionary<string, int[]>> item in dicBatchRegNoWithPreSuff)
                                        {
                                            string batchYear = item.Key;
                                            Dictionary<string, int[]> dicStud = item.Value;
                                            foreach (KeyValuePair<string, int[]> studItem in dicStud)
                                            {
                                                //string 
                                                //int min=studItem.
                                            }
                                        }

                                        if (dhall.Tables.Count > 0 && dhall.Tables[0].Rows.Count > 0)
                                        {

                                        }


                                        dtStroll = dhall.Tables[0].DefaultView.ToTable(true, "tempBatch");
                                        //dhall.Tables[0].DefaultView.RowFilter = "batch_year<>''";
                                        DataTable dtBatch = dhall.Tables[0].DefaultView.ToTable(true, "batch_year");
                                        if (dtStroll.Rows.Count > 0)
                                        {
                                            int max = 0;
                                            List<object> lstBatch = dtBatch.AsEnumerable().Select(r => r.Field<object>("batch_year")).ToList();
                                            dhall.Tables[0].DefaultView.RowFilter = "ModeVal='1' " + ((lstBatch.Count > 0) ? " and tempBatch in('" + string.Join("','", lstBatch.ToArray()) + "')" : "");
                                            DataTable dtMaxBatch = dhall.Tables[0].DefaultView.ToTable();

                                            if (dtMaxBatch.Rows.Count == 0)
                                            {
                                                dhall.Tables[0].DefaultView.RowFilter = "ModeVal='1' ";
                                                dtMaxBatch = dhall.Tables[0].DefaultView.ToTable();
                                            }
                                            if (dtMaxBatch.Rows.Count > 0)
                                            {
                                                List<int> studentList = dtMaxBatch.AsEnumerable().Select(r => r.Field<int>("tempBatch")).ToList();
                                                //int min = studentList.Min();
                                                max = studentList.Max();
                                            }
                                            //else
                                            //{

                                            //}
                                            DataTable dtStrollNew = new DataTable();
                                            bool currentBatch = false;
                                            if (dtStroll.Rows.Count > 0)
                                            {
                                                string finalRegNo = string.Empty;
                                                for (int i = 0; i < dtStroll.Rows.Count; i++)
                                                {
                                                    string listBatch = Convert.ToString(dtStroll.Rows[i]["tempBatch"]).Trim();
                                                    dhall.Tables[0].DefaultView.RowFilter = "tempBatch='" + listBatch + "'";
                                                    dhall.Tables[0].DefaultView.Sort = "Reg_No asc";
                                                    dicBatchwiseStudent = dhall.Tables[0].DefaultView.ToTable();
                                                    //for (int j = 0; j < dicBatchwiseStudent.Rows.Count; j++)
                                                    if (dicBatchwiseStudent.Rows.Count > 0)
                                                    {
                                                        int batch = Convert.ToInt32(dicBatchwiseStudent.Rows[0]["tempBatch"]);
                                                        regNum = dicBatchwiseStudent.Rows[0]["Reg_No"].ToString();
                                                        if (batch == max)
                                                        {
                                                            if (currentBatch == false)
                                                            {
                                                                string rollCount = string.Empty;
                                                                dicBatchwiseStudent.DefaultView.RowFilter = "Batch_Year='" + batch + "' ";

                                                                currentBatch = true;
                                                                if (currentBatch == true)
                                                                {
                                                                    DataRow dr = (DataRow)dicBatchwiseStudent.Rows[dicBatchwiseStudent.Rows.Count - 1];
                                                                    string latReg = dr["Reg_No"].ToString();
                                                                    if (dicBatchwiseStudent.Rows.Count > 1)
                                                                        //  rollCount = rollStart + "-" + rollEnd;
                                                                        finalRegNo += "  " + regNum.ToString() + "-" + latReg;
                                                                    else
                                                                        finalRegNo += "  " + regNum.ToString();
                                                                    // Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].Text = "" + "" + regNum.ToString() + "-" + latReg;
                                                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].CellType = txtcell;
                                                                    //regNum = string.Empty;
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            for (int j = 0; j < dicBatchwiseStudent.Rows.Count; j++)
                                                            {
                                                                regNum = dicBatchwiseStudent.Rows[j]["Reg_No"].ToString();
                                                                finalRegNo += "  " + regNum.ToString() + ",";
                                                                // Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].Text =""+""+regNum.ToString() + ",";
                                                                Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].CellType = txtcell;
                                                                //regNum = string.Empty;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(finalRegNo))
                                                {
                                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].Text = finalRegNo;
                                                }
                                            }
                                        }
                                    }
                                    else if(!string.IsNullOrEmpty(regno))
                                    {
                                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].Text = regno.ToString();
                                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].CellType = txtcell;
                                        regno = string.Empty;
                                    }

                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 7].Text = b.ToString();
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 8].Text = Convert.ToString("");
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 9].Text = Convert.ToString("");
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 8].ForeColor = Color.White;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 9].ForeColor = Color.White;
                                    string bundleNoList = string.Join(",", arrBundleNo.ToArray());
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(bundleNoList);
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 12].Locked = true;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 12].VerticalAlign = VerticalAlign.Middle;
                                    rowcount = 0;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                    Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;

                                    if (string.IsNullOrEmpty(Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].Text) && isRows)
                                    {
                                        Fspread3.Sheets[0].RowCount = Fspread3.Sheets[0].RowCount - 1;
                                        add = add - Convert.ToInt32(totstu1);
                                        row = row - 1;
                                    }
                                }
                            }
                            Fspread3.Sheets[0].RowCount++;
                            Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 7);
                            // Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 6, 1, 1);
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "    Total ";
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].ForeColor = Color.Red;
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 7].Text = add.ToString();
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 7].ForeColor = Color.Red;
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            total = 0;
                            hat.Clear();
                        }
                        else
                        {
                            lblmsg.Text = "No Records Found";
                            lblmsg.Visible = true;
                            Fspread3.Visible = false;
                            pnlContents.Visible = false;
                            Exportexcel.Visible = false;
                            Printfspread3.Visible = false; btn_directprint.Visible = false;
                            lblreportname2.Visible = false;
                            txtreportname2.Visible = false;
                        }
                        //Fspread3.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        //Fspread3.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        //Fspread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        //Fspread3.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        //Fspread3.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        //Fspread3.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        //Fspread3.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                        Fspread3.Sheets[0].Columns[0].Locked = true;
                        Fspread3.Sheets[0].Columns[1].Locked = true;
                        Fspread3.Sheets[0].Columns[2].Locked = true;
                        Fspread3.Sheets[0].Columns[3].Locked = true;
                        Fspread3.Sheets[0].Columns[4].Locked = true;
                        Fspread3.Sheets[0].Columns[5].Locked = true;
                        Fspread3.Sheets[0].Columns[6].Locked = true;
                        Fspread3.Sheets[0].PageSize = Fspread3.Sheets[0].RowCount;
                        Fspread3.Visible = true;
                        pnlContents.Visible = true;
                        Exportexcel.Visible = true;
                        Printfspread3.Visible = true; btn_directprint.Visible = true;
                        lblreportname2.Visible = true;
                        txtreportname2.Visible = true;
                    }
                    if (cbfooter.Checked == true)
                    {
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "Chief Superintendent:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 6, 1, Fspread3.Sheets[0].ColumnCount - 6);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].Text = "Absentees";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "University Representative:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "Reserve Hall Superintendent:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "No. of Halls:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds2.Tables[0].Rows.Count);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "No. of Q. Paper Covers opened:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "No. of A. Paper Covers packed:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "No. of candidates present:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "No. of candidates Absent:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "Unopened Question paper covers:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "Supporting Staff:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "Clerk:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "Programmer:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "Attenders:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].RowCount++;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 0, 1, 4);
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 1, 4, 1, 2);
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].Text = "Waterboy:";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].Text = ".";
                        Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                        Fspread3.Sheets[0].SpanModel.Add(Fspread3.Sheets[0].RowCount - 13, 6, 13, Fspread3.Sheets[0].ColumnCount - 6);
                        //Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 13, 6].Text = ".";
                        //Fspread3.Sheets[0].Cells[Fspread3.Sheets[0].RowCount - 13, 6].ForeColor = Color.White;
                    }
                }
                else
                {
                    lblmsg.Text = "No Records Found";
                    lblmsg.Visible = true;
                    Fspread3.Visible = false;
                    pnlContents.Visible = false;
                    Exportexcel.Visible = false;
                    Printfspread3.Visible = false;
                    btn_directprint.Visible = false;
                    lblreportname2.Visible = false;
                    txtreportname2.Visible = false;
                }
            }
            else
            {
                lblmsg.Text = "No Records Found";
                lblmsg.Visible = true;
                Fspread3.Visible = false;
                pnlContents.Visible = false;
                Printfspread3.Visible = false; btn_directprint.Visible = false;
                lblreportname2.Visible = false;
                txtreportname2.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    public void formatthree()
    {
        try
        {
            Fspread3.Visible = false;
            pnlContents.Visible = false;
            Exportexcel.Visible = false;
            Print_seating.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            hallbind();
            hss = 0;
            int p = 0;
            string nrow = string.Empty;
            int flag = 0;
            string dat = string.Empty;
            string arrangeview1 = string.Empty;
            FarPoint.Web.Spread.TextCellType txtcell1 = new FarPoint.Web.Spread.TextCellType();
            Fpseating.Sheets[0].SheetName = " ";
            Fpseating.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            Fpseating.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            Fpseating.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            Fpseating.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fpseating.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Fpseating.Sheets[0].DefaultStyle.Font.Bold = false;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpseating.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Fpseating.Sheets[0].AllowTableCorner = true;
            Fpseating.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            Fpseating.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            Fpseating.Pager.Align = HorizontalAlign.Right;
            Fpseating.Pager.Font.Bold = true;
            Fpseating.Pager.Font.Name = "Book Antiqua";
            Fpseating.Pager.PageCount = 5;
            Fpseating.CommandBar.Visible = false;
            Fpseating.Sheets[0].AutoPostBack = true;
            Fpseating.Sheets[0].RowHeader.Visible = false;
            Fpseating.Sheets[0].ColumnHeader.RowCount = 1;
            Fpseating.Sheets[0].Columns.Count = 0;
            if (ddlhall.Items.Count > 0)
            {
                string strmode = string.Empty;
                if (ddltype.Items.Count > 0)
                {
                    if (ddltype.SelectedItem.Text.Trim().ToLower() != "all")
                    {
                        if (ddltype.SelectedItem.Text.Trim().ToLower() != "")
                        {
                            strmode = " and Mode='" + ddltype.SelectedItem.Text.Trim() + "'";
                        }
                    }
                }
                string qryCollege = string.Empty;
                if (chkmergrecol.Checked)
                {
                    strmode = string.Empty;
                    qryCollege = string.Empty;
                }
                else
                {
                    qryCollege = " and college_code in(" + collegeCode + ")";
                    if (collegeCode.Contains(","))
                    {
                        strmode = string.Empty;
                    }
                }
                if (ddlhall.SelectedItem.Text.Trim() != "")
                {
                    //string rl = "select * from tbl_room_seats  where Hall_No='" + ddlhall.SelectedItem.Text + "' and exm_month='" + ddlMonth.SelectedValue + "' and exm_year='" + ddlYear.SelectedItem.Text + "' " + strmode + "";
                    string rl = "select * from tbl_room_seats where Hall_No='" + ddlhall.SelectedItem.Text + "' " + strmode + "";
                    ds = dt.select_method_wo_parameter(rl, "text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        flag = 1;
                        nrow = ds.Tables[0].Rows[0]["no_of_rows"].ToString();
                        arrangeview1 = ds.Tables[0].Rows[0]["arranged_view"].ToString();
                        allotseat = ds.Tables[0].Rows[0]["allocted_seats"].ToString();
                        Fpseating.Sheets[0].RowCount = Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_rows"].ToString());
                    }
                    Fpseating.Sheets[0].ColumnCount = 0;
                    if (ddlDate.Items.Count > 0)
                    {
                        if (ddlDate.SelectedItem.Text.Trim().ToLower() != "all")
                        {
                            dat = ddlDate.SelectedItem.Text;
                            string[] datt = dat.Split('-');
                            dat = datt[2].ToString() + "-" + datt[1].ToString() + "-" + datt[0].ToString();
                        }
                        else if (ddlDate.SelectedItem.Text.Trim().ToLower() == "all")
                        {
                            dat = ddlDate.Items[1].Text;
                            string[] datt = dat.Split('-');
                            dat = datt[2].ToString() + "-" + datt[1].ToString() + "-" + datt[0].ToString();
                        }
                    }
                    Dictionary<string, string> dicstude = new Dictionary<string, string>();
                    string sql = "select e.regno,e.seat_no,s.subject_code,c.Course_Name,r.Current_Semester,s.subject_no from exam_seating e,subject s,course c,Degree d,Registration r where r.Reg_No=e.regno and r.degree_code=d.Degree_Code and e.subject_no=s.subject_no and c.Course_Id=d.Course_Id and e.degree_code=d.Degree_Code and e.roomno='" + ddlhall.SelectedItem.Text + "' and e.edate='" + dat + "' and e.ses_sion='" + ddlSession.SelectedItem.Text + "' order by e.seat_no";
                    ds1 = dt.select_method_wo_parameter(sql, "text");
                    if (flag == 1)
                    {
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            Fpseating.Visible = true;
                            lblexcsea.Visible = true;
                            txtexseat.Text = string.Empty;
                            Excel_seating.Visible = true;
                            Print_seating.Visible = true;
                            txtexseat.Visible = true;
                            arrang = arrangeview1.Split(';');
                            Dictionary<int, int> dicsubcol = new Dictionary<int, int>();
                            Dictionary<string, int> dicsubcolcount = new Dictionary<string, int>();
                            for (int spr = 0; spr <= arrang.GetUpperBound(0); spr++)
                            {
                                string colsp = arrang[spr].ToString();
                                if (colsp.Trim() != "" && colsp != null)
                                {
                                    spcel = colsp.Split('-');
                                    for (int spc = 0; spc <= spcel.GetUpperBound(0); spc++)
                                    {
                                        int colsn = Convert.ToInt32(spcel[spc]);
                                        string strrow = "C" + spc + "R" + spr;
                                        if (!dicsubcolcount.ContainsKey(strrow))
                                        {
                                            dicsubcolcount.Add(strrow, colsn);
                                        }
                                        if (dicsubcol.ContainsKey(spc))
                                        {
                                            int valc = dicsubcol[spc];
                                            if (valc < colsn)
                                            {
                                                dicsubcol[spc] = colsn;
                                            }
                                        }
                                        else
                                        {
                                            dicsubcol.Add(spc, colsn);
                                        }
                                    }
                                }
                            }
                            int count = 0;
                            int getcouv = 0;
                            for (int h1 = 0; h1 < dicsubcol.Count; h1++)
                            {
                                int sucol = dicsubcol[h1];
                                sucol = sucol * 4;
                                for (int l = sucol - 1; l < sucol; l++)
                                {
                                    Fpseating.Sheets[0].Columns.Count = Fpseating.Sheets[0].Columns.Count + sucol;
                                    count = Fpseating.Sheets[0].Columns.Count - sucol;
                                    for (int g = 0; g < dicsubcol[h1]; g++)
                                    {
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count].Text = "Reg No";
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count].Font.Bold = true;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count].Font.Size = FontUnit.Medium;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count].Font.Name = "Book Antiqua";
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count].HorizontalAlign = HorizontalAlign.Center;
                                        Fpseating.Sheets[0].Columns[count].CellType = txtcell1;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 1].Text = "Course and Semester";
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 1].Font.Bold = true;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 1].Font.Size = FontUnit.Medium;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 1].Font.Name = "Book Antiqua";
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 1].HorizontalAlign = HorizontalAlign.Center;
                                        Fpseating.Sheets[0].Columns[count + 1].CellType = txtcell1;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 2].Text = "Paper Code";
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 2].Font.Bold = true;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 2].Font.Size = FontUnit.Medium;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 2].Font.Name = "Book Antiqua";
                                        Fpseating.Sheets[0].Columns[count + 2].CellType = txtcell1;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 3].Text = "Seat No";
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 3].Font.Bold = true;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 3].Font.Size = FontUnit.Medium;
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 3].Font.Name = "Book Antiqua";
                                        Fpseating.Sheets[0].ColumnHeader.Cells[0, count + 3].HorizontalAlign = HorizontalAlign.Center;
                                        Fpseating.Sheets[0].Columns[count + 3].CellType = txtcell1;
                                        count++;
                                        count = count + 3;
                                    }
                                    for (int j = 0; j < Convert.ToInt32(nrow); j++)
                                    {
                                        string strrow = "C" + h1 + "R" + j;
                                        if (dicsubcolcount.ContainsKey(strrow))
                                        {
                                            getcouv = dicsubcolcount[strrow];
                                            count = Fpseating.Sheets[0].Columns.Count - sucol;
                                            for (int g = 0; g < getcouv; g++)
                                            {
                                                hss++;
                                                if (p < Convert.ToInt32(allotseat))
                                                {
                                                    if (p < ds1.Tables[0].Rows.Count)
                                                    {
                                                        if (ds1.Tables[0].Rows[p]["seat_no"].ToString() == Convert.ToString(hss))
                                                        {
                                                            Fpseating.Sheets[0].Cells[j, count].Text = ds1.Tables[0].Rows[p]["regno"].ToString();
                                                            if (dicstude.ContainsKey(ds1.Tables[0].Rows[p]["subject_no"].ToString()))
                                                            {
                                                                string getreg = dicstude[ds1.Tables[0].Rows[p]["subject_no"].ToString()];
                                                                getreg = getreg + ", " + ds1.Tables[0].Rows[p]["regno"].ToString();
                                                                dicstude[ds1.Tables[0].Rows[p]["subject_no"].ToString()] = getreg;
                                                            }
                                                            else
                                                            {
                                                                dicstude.Add(ds1.Tables[0].Rows[p]["subject_no"].ToString(), ds1.Tables[0].Rows[p]["regno"].ToString());
                                                            }
                                                            // Fpseating.Sheets[0].Cells[j, count].Font.Bold = true;
                                                            Fpseating.Sheets[0].Cells[j, count].Font.Size = FontUnit.Medium;
                                                            Fpseating.Sheets[0].Cells[j, count].Font.Name = "Book Antiqua";
                                                            Fpseating.Sheets[0].Cells[j, count].HorizontalAlign = HorizontalAlign.Left;
                                                            Fpseating.Sheets[0].Cells[j, count + 1].Text = "S" + ds1.Tables[0].Rows[p]["Current_Semester"].ToString() + " - " + ds1.Tables[0].Rows[p]["Course_Name"].ToString();
                                                            //Fpseating.Sheets[0].Cells[j, count + 1].Font.Bold = true;
                                                            Fpseating.Sheets[0].Cells[j, count + 1].Font.Size = FontUnit.Medium;
                                                            Fpseating.Sheets[0].Cells[j, count + 1].Font.Name = "Book Antiqua";
                                                            Fpseating.Sheets[0].Cells[j, count + 1].HorizontalAlign = HorizontalAlign.Left;
                                                            Fpseating.Sheets[0].Cells[j, count + 2].Text = ds1.Tables[0].Rows[p]["subject_code"].ToString();
                                                            //Fpseating.Sheets[0].Cells[j, count + 2].Font.Bold = true;
                                                            Fpseating.Sheets[0].Cells[j, count + 2].Font.Size = FontUnit.Medium;
                                                            Fpseating.Sheets[0].Cells[j, count + 2].Font.Name = "Book Antiqua";
                                                            Fpseating.Sheets[0].Cells[j, count + 2].HorizontalAlign = HorizontalAlign.Left;
                                                            Fpseating.Sheets[0].Cells[j, count + 3].Text = "[" + ds1.Tables[0].Rows[p]["seat_no"].ToString() + "]";
                                                            //Fpseating.Sheets[0].Cells[j, count + 3].Font.Bold = true;
                                                            Fpseating.Sheets[0].Cells[j, count + 3].Font.Size = FontUnit.Medium;
                                                            Fpseating.Sheets[0].Cells[j, count + 3].Font.Name = "Book Antiqua";
                                                            Fpseating.Sheets[0].Cells[j, count + 3].HorizontalAlign = HorizontalAlign.Center;
                                                            p++;
                                                        }
                                                        else
                                                        {
                                                            Fpseating.Sheets[0].Cells[j, count + 3].Text = "[" + hss + "]";
                                                            //Fpseating.Sheets[0].Cells[j, count + 3].Font.Bold = true;
                                                            Fpseating.Sheets[0].Cells[j, count + 3].Font.Size = FontUnit.Medium;
                                                            Fpseating.Sheets[0].Cells[j, count + 3].Font.Name = "Book Antiqua";
                                                            Fpseating.Sheets[0].Cells[j, count + 3].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }
                                                }
                                                count++;
                                                count = count + 3;
                                            }
                                        }
                                    }
                                }
                            }
                            string sal = "select distinct e.subject_no,s.subject_code, s.subject_name,c.Course_Name,de.dept_acronym,COUNT(e.subject_no) as num  from exam_seating e,subject s,Degree d,course c,Department de where e.subject_no=s.subject_no and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and e.roomno='" + ddlhall.SelectedItem.Text + "' and e.edate='" + dat + "' group by e.subject_no,s.subject_name,s.subject_code,c.Course_Name,de.dept_acronym order by COUNT(e.subject_no) desc";
                            ds = dt.select_method_wo_parameter(sal, "text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                Fpseating.Sheets[0].RowCount++;
                                Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 0, 1, Fpseating.Sheets[0].Columns.Count - 1);
                                Fpseating.Sheets[0].RowCount++;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Text = "Hall No";
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Text = "Course Code";
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Text = "Subject Code";
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Text = "Reg no";
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 3, 1, Fpseating.Sheets[0].ColumnCount - 4);
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, Fpseating.Sheets[0].ColumnCount - 1].Text = "No of Candidates";
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, Fpseating.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, Fpseating.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, Fpseating.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, Fpseating.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                for (int se = 0; se < ds.Tables[0].Rows.Count; se++)
                                {
                                    Fpseating.Sheets[0].RowCount++;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Text = ddlhall.SelectedItem.Text.ToString();
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[se]["Course_Name"].ToString() + " - " + ds.Tables[0].Rows[se]["dept_acronym"].ToString();
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[se]["subject_code"].ToString();
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    if (dicstude.ContainsKey(ds.Tables[0].Rows[se]["subject_no"].ToString()))
                                    {
                                        string getreg = dicstude[ds.Tables[0].Rows[se]["subject_no"].ToString()];
                                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Text = getreg;
                                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 3, 1, Fpseating.Sheets[0].ColumnCount - 4);
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, Fpseating.Sheets[0].ColumnCount - 1].Text = ds.Tables[0].Rows[se]["num"].ToString();
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, Fpseating.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, Fpseating.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                                }
                                Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - ds.Tables[0].Rows.Count, 0, ds.Tables[0].Rows.Count, 1);
                            }
                        }
                        else
                        {
                            lblmsg.Text = "No Records Found";
                            lblmsg.Visible = true;
                            txtexseat.Visible = false;
                            lblexcsea.Visible = false;
                            Excel_seating.Visible = false;
                            Print_seating.Visible = false;
                        }
                    }
                }
            }
            //Fpseating.Sheets[0].ColumnCount = 8;
            //Fpseating.Sheets[0].RowHeader.Visible = false;
            //Fpseating.Sheets[0].AutoPostBack = true;
            //Fpseating.Sheets[0].ColumnHeader.RowCount = 2;
            //Fpseating.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            //Fpseating.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            //Fpseating.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            //Fpseating.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            //Fpseating.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            //Fpseating.Sheets[0].DefaultStyle.Font.Bold = false;
            //Fpseating.CommandBar.Visible = false;
            //FarPoint.Web.Spread.TextCellType txtcell1 = new FarPoint.Web.Spread.TextCellType();
            //FarPoint.Web.Spread.TextCellType regnotxtcell = new FarPoint.Web.Spread.TextCellType();
            //Fpseating.Sheets[0].Columns[0].Width = 200;
            //Fpseating.Sheets[0].Columns[1].Width = 350;
            //Fpseating.Sheets[0].Columns[2].Width = 150;
            //Fpseating.Sheets[0].Columns[3].Width = 200;
            //Fpseating.Sheets[0].Columns[4].Width = 200;
            //Fpseating.Sheets[0].Columns[5].Width = 350;
            //Fpseating.Sheets[0].Columns[6].Width = 150;
            //Fpseating.Sheets[0].Columns[7].Width = 200;
            //Fpseating.Sheets[0].RowCount = 0;
            //Fpseating.Sheets[0].AutoPostBack = true;
            //DataView dv = new DataView();
            //DataSet dsformat = new DataSet();
            //DataSet dhallbind = new DataSet();
            //string strdateval =string.Empty;
            //string typequery =string.Empty;
            //string roomno =string.Empty;
            //string hallcode =string.Empty;
            //string halldate =string.Empty;
            //string deg =string.Empty;
            //string subjectcode =string.Empty;
            //string acroymn =string.Empty;
            //string regno =string.Empty;
            //string degreecode =string.Empty;
            //int bcount = 0;
            //int seatno = 1;
            //string hl =string.Empty;
            //if (ddlhall.SelectedItem.Text == "")
            //{
            //    lblmsg.Text = "No Records Found";
            //    lblmsg.Visible = true;
            //}
            //else
            //{
            //    hallcode = ddlhall.SelectedItem.Text;
            //}
            //if (hallcode != "")
            //{
            //    Fpseating.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Hall Code :" + "  " + hallcode;
            //    Fpseating.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 8);
            //    Fpseating.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Left;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Reg No";
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 1].Text = "Course and Semester";
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Paper Code";
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Seat No";
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Reg No";
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Course and Semester";
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Paper Code";
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Seat No";
            //    Fpseating.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[0, 6].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[0, 7].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 3].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 5].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 6].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].ColumnHeader.Cells[1, 7].Border.BorderColor = Color.Black;
            //    Fpseating.Sheets[0].RowCount = 0;
            //    if (ddlDate.SelectedItem.Text != "All")
            //    {
            //        string edate = ddlDate.SelectedItem.Text.ToString();
            //        string[] spd = edate.Split('-');
            //        strdateval = strdateval + " and et.exam_date='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
            //    }
            //    strdateval = strdateval + " and et.exam_session='" + ddlSession.SelectedItem.ToString() + "'";
            //    string strgetdate = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,convert(varchar(20),et.exam_date,110) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' and et.coll_code='" + college_code + "' " + strdateval + " order by  ExamDate";
            //    dsformat = dt.select_method_wo_parameter(strgetdate, "text");
            //    if (dsformat.Tables[0].Rows.Count > 0)
            //    {
            //        string strexamdate = " and et.exam_date='" + dsformat.Tables[0].Rows[0]["edate"].ToString() + "'";
            //        if (ddltype.SelectedItem.Text != "All")
            //        {
            //            if (ddltype.SelectedItem.Text != "")
            //            {
            //                typequery = "and c.type='" + ddltype.SelectedItem.Text + "'";
            //            }
            //        }
            //        if (ddlDate.SelectedItem.Text != "All")
            //        {
            //            string edate = ddlDate.SelectedItem.Text.ToString();
            //            string[] spd = edate.Split('-');
            //            halldate = spd[1] + "/" + spd[0] + "/" + spd[2];
            //        }
            //    }
            //    else
            //    {
            //        lblmsg.Text = "Please Set Time Table";
            //        lblmsg.Visible = true;
            //    }
            //    hl = "select distinct es.roomno ,c.Course_Name,de.Dept_Name,d.Degree_Code,s.subject_name,s.subject_code,d.Acronym,es.edate,es.ses_sion from exmtt e,exmtt_det et,exam_seating es,course c,Degree d,Department de,subject s where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and  d.Dept_Code=de.Dept_Code and es.subject_no=s.subject_no  and et.subject_no=s.subject_no and e.Exam_year='" + ddlYear.SelectedItem.Text + "' and e.Exam_month='" + ddlMonth.SelectedValue + "' and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.Text + "'  ";
            //    ds2 = dt.select_method_wo_parameter(hl, "text");
            //    if (ds2.Tables[0].Rows.Count > 0)
            //    {
            //        if (ddltype.SelectedItem.Text != "All")
            //        {
            //            if (ddltype.SelectedItem.Text != "")
            //            {
            //                hl = hl + " and c.type='" + ddltype.SelectedItem.Text + "'";
            //            }
            //        }
            //        if (ddlhall.Items.Count > 0)
            //        {
            //            if (ddlhall.SelectedItem.Text != "All")
            //            {
            //                if (ddlhall.SelectedItem.Text != "")
            //                {
            //                    hl = hl + " and es.roomno='" + ddlhall.SelectedItem.Text + "'";
            //                }
            //            }
            //        }
            //        ds2 = dt.select_method_wo_parameter(hl, "text");
            //        if (ds2.Tables[0].Rows.Count > 0)
            //        {
            //            ds2.Tables[0].DefaultView.RowFilter = " roomno='" + ddlhall.SelectedItem.Text + "'";
            //            dv = ds2.Tables[0].DefaultView;
            //            if (dv.Count > 0)
            //            {
            //                for (int room = 0; room < ds2.Tables[0].Rows.Count; room++)
            //                {
            //                    roomno = ds2.Tables[0].Rows[room]["roomno"].ToString();
            //                    deg = ds2.Tables[0].Rows[room]["Course_Name"].ToString();
            //                    degreecode = ds2.Tables[0].Rows[room]["Degree_Code"].ToString();
            //                    acroymn = ds2.Tables[0].Rows[room]["Acronym"].ToString();
            //                    subjectcode = ds2.Tables[0].Rows[room]["subject_code"].ToString();
            //                    string register = "select r.Reg_No from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and es.subject_no=s.subject_no and ead.subject_no=s.subject_no and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.Text + "' and ed.degree_code='" + degreecode + "' and es.roomno='" + roomno + "' and s.subject_code='" + subjectcode + "'order by r.Reg_No";
            //                    dhallbind = dt.select_method_wo_parameter(register, "Text");
            //                    int tempq = 0;
            //                    if (dhallbind.Tables[0].Rows.Count > 0)
            //                    {
            //                        for (bcount = 0; bcount < dhallbind.Tables[0].Rows.Count; bcount++)
            //                        {
            //                            if (seatno % 2 == 0)
            //                            {
            //                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].Text = seatno.ToString();
            //                            }
            //                            else
            //                            {
            //                                Fpseating.Sheets[0].RowCount++;
            //                                Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Text = seatno.ToString();
            //                                tempq++;
            //                            }
            //                            seatno++;
            //                        }
            //                    }
            //                }
            //            }
            //            int rowcountw = seatno;
            //            int div = seatno / 2;
            //            int temp = 0;
            //            int cond = 0;
            //            int counting = 0;
            //            for (int room = 0; room < ds2.Tables[0].Rows.Count; room++)
            //            {
            //                roomno = ds2.Tables[0].Rows[room]["roomno"].ToString();
            //                deg = ds2.Tables[0].Rows[room]["Course_Name"].ToString();
            //                degreecode = ds2.Tables[0].Rows[room]["Degree_Code"].ToString();
            //                acroymn = ds2.Tables[0].Rows[room]["Acronym"].ToString();
            //                subjectcode = ds2.Tables[0].Rows[room]["subject_code"].ToString();
            //                DataSet dhallbinding = new DataSet();
            //                string regcs = "select r.Reg_No from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and es.subject_no=s.subject_no and ead.subject_no=s.subject_no and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.Text + "' and ed.degree_code='" + degreecode + "' and es.roomno='" + roomno + "' and s.subject_code='" + subjectcode + "'order by r.Reg_No";
            //                dhallbinding = dt.select_method_wo_parameter(regcs, "Text");
            //                if (dhallbinding.Tables[0].Rows.Count > 0)
            //                {
            //                    for (bcount = 0; bcount < dhallbinding.Tables[0].Rows.Count; bcount++)
            //                    {
            //                        //if (temp % 2 == 0)
            //                        //{
            //                        //    regno = dhallbinding.Tables[0].Rows[bcount]["Reg_No"].ToString();
            //                        //    Fpseating.Sheets[0].Cells[cond, 0].CellType = regnotxtcell;
            //                        //    Fpseating.Sheets[0].Cells[cond, 0].Text = regno.ToString();
            //                        //    Fpseating.Sheets[0].Cells[cond, 1].Text = deg.ToString() + " - " + acroymn.ToString();
            //                        //    Fpseating.Sheets[0].Cells[cond, 2].Text = subjectcode.ToString();
            //                        //    Fpseating.Sheets[0].RowCount++;
            //                        //    regno =string.Empty;
            //                        //    cond = 0;
            //                        //}
            //                        //else
            //                        //{
            //                        //    if (dhallbinding.Tables[0].Rows.Count > 0)
            //                            {
            //                                for (bcount = 0; bcount < dhallbinding.Tables[0].Rows.Count; bcount++)
            //                                {
            //                                    regno = dhallbinding.Tables[0].Rows[bcount]["Reg_No"].ToString();
            //                                    if (cond < div)
            //                                    {
            //                                        Fpseating.Sheets[0].Cells[cond, 0].CellType = regnotxtcell;
            //                                        Fpseating.Sheets[0].Cells[cond, 0].Text = regno.ToString();
            //                                        Fpseating.Sheets[0].Cells[cond, 1].Text = deg.ToString() + " - " + acroymn.ToString();
            //                                        Fpseating.Sheets[0].Cells[cond, 2].Text = subjectcode.ToString();
            //                                        Fpseating.Sheets[0].RowCount++;
            //                                    }
            //                                    cond++;
            //                                    if (cond > div)
            //                                    {
            //                                        Fpseating.Sheets[0].Cells[0 + counting, 4].CellType = regnotxtcell;
            //                                        Fpseating.Sheets[0].Cells[0 + counting, 4].Text = regno.ToString();
            //                                        Fpseating.Sheets[0].Cells[0 + counting, 5].Text = deg.ToString() + " - " + acroymn.ToString();
            //                                        Fpseating.Sheets[0].Cells[0 + counting, 6].Text = subjectcode.ToString();
            //                                        Fpseating.Sheets[0].RowCount++;
            //                                        counting++;
            //                                    }
            //                                }
            //                            }
            //                        //}
            //                        temp++;
            //                    }
            //                }
            //            }
            //            Fpseating.Sheets[0].RowCount = div;
            //            Fpseating.Sheets[0].RowCount++;
            //            Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 0, 1, 8);
            //            Fpseating.Sheets[0].RowCount++;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Text = "Hall No";
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Text = "Course Code";
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Text = "Subject Code";
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Text = "Reg no";
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
            //            Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 3, 1, 4);
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].Text = "No of Candidates";
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Bold = true;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Font.Bold = true;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].Font.Bold = true;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antique";
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antique";
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antique";
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antique";
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antique";
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            //            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
            //            int add1 = 0;
            //            DataSet ds3 = new DataSet();
            //            if (ddlDate.SelectedItem.Text != "All")
            //            {
            //                string edate = ddlDate.SelectedItem.Text.ToString();
            //                string[] spd = edate.Split('-');
            //                strdateval = strdateval + " and et.exam_date='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
            //            }
            //            strdateval = strdateval + " and et.exam_session='" + ddlSession.SelectedItem.ToString() + "'";
            //            string strgetdate1 = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,convert(varchar(20),et.exam_date,110) as edate from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' and et.coll_code='" + college_code + "' " + strdateval + " order by  ExamDate";
            //            ds3 = dt.select_method_wo_parameter(strgetdate, "text");
            //            if (ds3.Tables[0].Rows.Count > 0)
            //            {
            //                string strexamdate = " and et.exam_date='" + ds3.Tables[0].Rows[0]["edate"].ToString() + "'";
            //                if (ddltype.SelectedItem.Text != "All")
            //                {
            //                    if (ddltype.SelectedItem.Text != "")
            //                    {
            //                        typequery = "and c.type='" + ddltype.SelectedItem.Text + "'";
            //                    }
            //                }
            //                if (ddlDate.SelectedItem.Text != "All")
            //                {
            //                    string edate = ddlDate.SelectedItem.Text.ToString();
            //                    string[] spd = edate.Split('-');
            //                    halldate = spd[1] + "/" + spd[0] + "/" + spd[2];
            //                }
            //            }
            //            else
            //            {
            //                lblmsg.Text = "Please Set Time Table";
            //                lblmsg.Visible = true;
            //            }
            //            string totstu1 =string.Empty;
            //            string hl1 = "select distinct es.roomno from exmtt e,exmtt_det et,exam_seating es,course c,Degree d where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id " + typequery + " and e.Exam_year='" + ddlYear.SelectedItem.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.ToString() + "'";
            //            ds2 = dt.select_method_wo_parameter(hl, "text");
            //            if (ds2.Tables[0].Rows.Count > 0)
            //            {
            //                int rowcount = 0;
            //                roomno = ddlhall.SelectedItem.Text;
            //                string query = "select distinct es.roomno ,c.Course_Name,de.Dept_Name,d.Degree_Code,s.subject_name,s.subject_code,d.Acronym,es.edate,es.ses_sion from exmtt e,exmtt_det et,exam_seating es,course c,Degree d,Department de,subject s where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and d.Dept_Code=de.Dept_Code and es.subject_no=s.subject_no  and et.subject_no=s.subject_no  and e.Exam_year='" + ddlYear.SelectedItem.Text + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and es.edate='" + halldate + "' and  es.ses_sion='" + ddlSession.SelectedItem.Text + "'  and es.roomno='" + ddlhall.SelectedItem.Text + "'";
            //                if (ddltype.SelectedItem.Text != "All")
            //                {
            //                    if (ddltype.SelectedItem.Text != "")
            //                    {
            //                        typequery = "and c.type='" + ddltype.SelectedItem.Text + "'";
            //                    }
            //                }
            //                int total = 1;
            //                DataSet ds = new DataSet();
            //                DataSet dhall1 = new DataSet();
            //                int b1 = 0;
            //                Hashtable hat1 = new Hashtable();
            //                ds = dt.select_method_wo_parameter(query, "Text");
            //                rowcount = ds.Tables[0].Rows.Count;
            //                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //                {
            //                    total = 1;
            //                    for (int roomss = 0; roomss < ds.Tables[0].Rows.Count; roomss++)
            //                    {
            //                        deg = ds.Tables[0].Rows[roomss]["Course_Name"].ToString();
            //                        string degreecode1 = ds.Tables[0].Rows[roomss]["Degree_Code"].ToString();
            //                        acroymn = ds.Tables[0].Rows[roomss]["Acronym"].ToString();
            //                        subjectcode = ds.Tables[0].Rows[roomss]["subject_code"].ToString();
            //                        Fpseating.Sheets[0].RowCount++;
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Text = roomno.ToString();
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Text = deg + " - " + acroymn.ToString();
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Text = subjectcode.ToString();
            //                        string h1 = "select r.Reg_No from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and es.subject_no=s.subject_no and ead.subject_no=s.subject_no and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.Text + "' and ed.degree_code='" + degreecode1 + "' and es.roomno='" + ddlhall.SelectedItem.Text + "' and s.subject_code='" + subjectcode + "'order by r.Reg_No";
            //                        h1 = h1 + "       select count( r.Reg_No) as totstu from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text + "' and es.subject_no=s.subject_no and ead.subject_no=s.subject_no and es.edate='" + halldate + "' and es.ses_sion='" + ddlSession.SelectedItem.Text + "' and ed.degree_code='" + degreecode1 + "' and es.roomno='" + roomno + "' and s.subject_code='" + subjectcode + "'";
            //                        string regno1 =string.Empty;
            //                        dhall1 = dt.select_method_wo_parameter(h1, "Text");
            //                        if (dhall1.Tables[0].Rows.Count > 0)
            //                        {
            //                            for (b1 = 0; b1 < dhall1.Tables[0].Rows.Count; b1++)
            //                            {
            //                                if (regno1 == "")
            //                                {
            //                                    regno1 = dhall1.Tables[0].Rows[b1]["Reg_No"].ToString();
            //                                }
            //                                else
            //                                {
            //                                    regno1 = regno1 + ",  " + dhall1.Tables[0].Rows[b1]["Reg_No"].ToString();
            //                                    total++;
            //                                }
            //                            }
            //                        }
            //                        totstu1 = dhall1.Tables[1].Rows[0]["totstu"].ToString();
            //                        if (!hat1.ContainsKey(roomno))
            //                        {
            //                            hat1.Add(roomno, totstu1);
            //                            add1 = 0;
            //                            add1 = add1 + Convert.ToInt32(totstu1);
            //                        }
            //                        else
            //                        {
            //                            add1 = add1 + Convert.ToInt32(totstu1);
            //                        }
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].CellType = txtcell1;
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Text = regno1.ToString();
            //                        Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 3, 1, 4);
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].CellType = txtcell1;
            //                        regno =string.Empty;
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].Text = b1.ToString();
            //                        rowcount = 0;
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            //                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
            //                    }
            //                    Fpseating.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //                    Fpseating.Sheets[0].RowCount++;
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Font.Bold = true;
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].Text = "    Total ";
            //                    Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 3, 1, 4);
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].ForeColor = Color.Red;
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].Text = add1.ToString();
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].ForeColor = Color.Red;
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].Font.Bold = true;
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
            //                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
            //                    total = 0;
            //                    hat1.Clear();
            //                    add1 = 0;
            //                }
            //                else
            //                {
            //                    lblmsg.Text = "No Records Found";
            //                    lblmsg.Visible = true;
            //                    Fspread3.Visible = false;
            //                    Exportexcel.Visible = false;
            //                    Printfspread3.Visible = false; btn_directprint.Visible = false;
            //                }
            //                Fpseating.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //                Fpseating.Sheets[0].PageSize = Fpseating.Sheets[0].RowCount;
            //                Fpseating.Visible = true;
            //            }
            //            Fpseating.Sheets[0].PageSize = Fpseating.Sheets[0].RowCount;
            //            Fpseating.Visible = true;
            //            //fpseatingreport();
            //            Print_seating.Visible = true;
            //            Excel_seating.Visible = true;
            //            Fpspread.Visible = false;
            //            Fspread3.Visible = false;
            //            lblmsg.Visible = false;
            //            lblmessage1.Visible = false;
            //            lblrptname.Visible = false;
            //            btnxl.Visible = false;
            //            btnprintmaster.Visible = false;
            //            Printcontrol.Visible = false;
            //            txtexcelname.Visible = false;
            //            Exportexcel.Visible = false;
            //            Printfspread3.Visible = false; btn_directprint.Visible = false;
            //        }
            //        else
            //        {
            //            lblmsg.Text = "No Records Found";
            //            lblmsg.Visible = true;
            //            Print_seating.Visible = false;
            //            Excel_seating.Visible = false;
            //            Fpseating.Visible = false;
            //        }
            //    }
            //    else
            //    {
            //        lblmsg.Text = "No Records Found";
            //        lblmsg.Visible = true;
            //        Print_seating.Visible = false;
            //        Excel_seating.Visible = false;
            //        Fpseating.Visible = false;
            //    }
            //}
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    protected void btn_directprint_Click(object sender, EventArgs e)
    {
        Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
        Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
        Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
        Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
        Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
        Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
        Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
        Boolean flag = true;
        System.Drawing.Font Fontboldhead = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
        System.Drawing.Font Fontmediumv = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font Fontmedium1V = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font f1 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Regular);
        System.Drawing.Font f2 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Regular);
        System.Drawing.Font f3 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
        System.Drawing.Font f4 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
        System.Drawing.Font f5 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Regular);
        System.Drawing.Font f6 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
        System.Drawing.Font f7 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Bold);
        System.Drawing.Font f8 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Bold);
        System.Drawing.Font f9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);
        System.Drawing.Font f10 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
        System.Drawing.Font f11 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold);
        System.Drawing.Font f12 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
        string coename = string.Empty;
        string strquery = "select *,district+' - '+pincode  as districtpin from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
        ds.Dispose();
        ds.Reset();
        ds = dt.select_method_wo_parameter(strquery, "Text");
        string Collegename = string.Empty;
        string aff = string.Empty;
        string collacr = string.Empty;
        string dispin = string.Empty;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
            aff = ds.Tables[0].Rows[0]["affliatedby"].ToString();
            string[] strpa = aff.Split(',');
            aff = strpa[0];
            coename = ds.Tables[0].Rows[0]["coe"].ToString();
            collacr = ds.Tables[0].Rows[0]["acr"].ToString();
            dispin = ds.Tables[0].Rows[0]["districtpin"].ToString();
        }
        if (Radioformat2.Checked == true)
        {
            #region format2
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            mypdfpage = mydoc.NewPage();
            PdfArea tete = new PdfArea(mydoc, 15, 15, 565, 810);
            PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
            mypdfpage.Add(pr1);
            int coltop = 20;
            #region Left Logo
            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            {
                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                mypdfpage.Add(LogoImage, 20, 20, 400);
            }
            #endregion
            #region TOP DETAILS
            PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename);
            mypdfpage.Add(ptc);
            coltop = coltop + 25;
            ptc = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, aff);
            mypdfpage.Add(ptc);
            coltop = coltop + 20;
            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");
            mypdfpage.Add(ptc);
            coltop = coltop + 20;
            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString()));
            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "SEATING ARRANGEMENT  -  " + strMonthName.ToUpper() + " " + ddlYear.SelectedItem.Text.ToString() + "");
            mypdfpage.Add(ptc);
            coltop = coltop + 20;
            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "Date & Session : " + ddlDate.SelectedItem.Text.ToString() + " & " + ddlSession.SelectedItem.Text.ToString() + " ");
            mypdfpage.Add(ptc);
            coltop = coltop + 20;
            //PdfArea tlinerect = new PdfArea(mydoc, 15, coltop + 25, 565, 0.01);
            //PdfRectangle plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
            //mypdfpage.Add(plimerecyt);
            #endregion
            Gios.Pdf.PdfTable table1forpage2a = mydoc.NewTable(Fontboldhead, 1, 6, 5);
            table1forpage2a.VisibleHeaders = false;
            table1forpage2a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            table1forpage2a.Columns[0].SetWidth(40);
            table1forpage2a.Columns[1].SetWidth(60);
            table1forpage2a.Columns[2].SetWidth(130);
            table1forpage2a.Columns[3].SetWidth(90);
            table1forpage2a.Columns[4].SetWidth(120);
            table1forpage2a.Columns[5].SetWidth(100);
            table1forpage2a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage2a.Cell(0, 0).SetContent("S.No.");
            table1forpage2a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage2a.Cell(0, 1).SetContent("Hall No.");
            table1forpage2a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage2a.Cell(0, 2).SetContent("Degree / Branch");
            table1forpage2a.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage2a.Cell(0, 3).SetContent("Subject Code");
            table1forpage2a.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage2a.Cell(0, 4).SetContent("Reg. No / Seat No.");
            table1forpage2a.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
            table1forpage2a.Cell(0, 5).SetContent("Total No of Student");
            Gios.Pdf.PdfTablePage addtabletopage = table1forpage2a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 125, 565, 50));
            mypdfpage.Add(addtabletopage);
            PdfArea tlinerect = new PdfArea(mydoc, 15, coltop + 25, 565, 0.01);
            PdfRectangle plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
            Gios.Pdf.PdfTable tableparts;
            Gios.Pdf.PdfTable tablepartsduplicate;
            int snoo = 0;
            double page2col = 140;
            int adjustvalue = 6;
            Boolean addedcc = false;
            if (Fspread3.Sheets[0].RowCount > 0)
            {
                for (int i = 0; i < Fspread3.Sheets[0].RowCount; i++)
                {
                    snoo++;
                    tableparts = mydoc.NewTable(Fontsmall1, 1, 6, 2);
                    tablepartsduplicate = mydoc.NewTable(Fontsmall1, 1, 6, 2);
                    tableparts.VisibleHeaders = false;
                    tablepartsduplicate.VisibleHeaders = false;
                    tablepartsduplicate.SetBorders(Color.Black, 1, BorderType.None);
                    tableparts.SetBorders(Color.Black, 1, BorderType.None);
                    tableparts.Columns[0].SetWidth(40);
                    tableparts.Columns[1].SetWidth(60);
                    tableparts.Columns[2].SetWidth(130);
                    tableparts.Columns[3].SetWidth(90);
                    tableparts.Columns[4].SetWidth(120);
                    tableparts.Columns[5].SetWidth(100);
                    tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts.Cell(0, 1).SetContent(Fspread3.Sheets[0].Cells[i, 2].Text.ToString());
                    tableparts.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                    tableparts.Cell(0, 2).SetContent(Fspread3.Sheets[0].Cells[i, 4].Text.ToString());
                    tableparts.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts.Cell(0, 3).SetContent(Fspread3.Sheets[0].Cells[i, 5].Text.ToString());
                    tableparts.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                    tableparts.Cell(0, 4).SetContent(Fspread3.Sheets[0].Cells[i, 6].Text.ToString());
                    tableparts.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts.Cell(0, 5).SetContent(Fspread3.Sheets[0].Cells[i, 7].Text.ToString());
                    if (Fspread3.Sheets[0].Cells[i, 0].Text.ToString().Trim().ToLower() == "total")
                    {
                        tableparts.Rows[0].SetColors(Color.Black, Color.DarkGray);
                        tableparts.Rows[0].SetFont(Fontbold);
                        tableparts.Cell(0, 4).SetCellPadding(5);
                        // tableparts.Cell(0, 4).SetColors(Color.Black, Color.AliceBlue);
                        tableparts.Cell(0, 4).SetContent(Fspread3.Sheets[0].Cells[i, 0].Text.ToString() + " : ");
                        tableparts.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleRight);
                    }
                    else
                    {
                        tableparts.Cell(0, 0).SetContent(Fspread3.Sheets[0].Cells[i, 0].Text.ToString());
                    }
                    tablepartsduplicate.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepartsduplicate.Cell(0, 0).SetContent(Fspread3.Sheets[0].Cells[i, 0].Text.ToString());
                    tablepartsduplicate.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepartsduplicate.Cell(0, 1).SetContent(Fspread3.Sheets[0].Cells[i, 2].Text.ToString());
                    tablepartsduplicate.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                    tablepartsduplicate.Cell(0, 2).SetContent(Fspread3.Sheets[0].Cells[i, 4].Text.ToString());
                    tablepartsduplicate.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepartsduplicate.Cell(0, 3).SetContent(Fspread3.Sheets[0].Cells[i, 5].Text.ToString());
                    tablepartsduplicate.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                    tablepartsduplicate.Cell(0, 4).SetContent(Fspread3.Sheets[0].Cells[i, 6].Text.ToString());
                    tablepartsduplicate.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepartsduplicate.Cell(0, 5).SetContent(Fspread3.Sheets[0].Cells[i, 7].Text.ToString());
                    page2col = page2col + 5;
                    addtabletopage = tablepartsduplicate.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, page2col, 553, 600));
                    //mypdfpage.Add(addtabletopage);
                    double getheigh = addtabletopage.Area.Height;
                    getheigh = Math.Round(getheigh, 2);
                    double dummycolval = page2col + getheigh;
                    if (813 > dummycolval && flag == true)
                    {
                        if (Fspread3.Sheets[0].Cells[i, 0].Text.ToString().Trim().ToLower() == "total")
                        {
                            addtabletopage = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, page2col, 565, 600));
                            mypdfpage.Add(addtabletopage);
                            addedcc = true;
                            tlinerect = new PdfArea(mydoc, 15, dummycolval + 7, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                        }
                        else
                        {
                            addtabletopage = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, page2col + adjustvalue, 565, 600));
                            mypdfpage.Add(addtabletopage);
                            addedcc = true;
                            tlinerect = new PdfArea(mydoc, 15, dummycolval + 7, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                        }
                        dummycolval = dummycolval - 120;
                        tlinerect = new PdfArea(mydoc, 57, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 120, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 256, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 350, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 476, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        page2col = page2col + getheigh;
                        addedcc = false;
                    }
                    else if (813 > dummycolval)
                    {
                        addedcc = true;
                        dummycolval = dummycolval - 120;
                        addedcc = false;
                        if (Fspread3.Sheets[0].Cells[i, 0].Text.ToString().Trim().ToLower() == "total")
                        {
                            addtabletopage = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, page2col, 565, 600));
                            mypdfpage.Add(addtabletopage);
                            addedcc = true;
                            tlinerect = new PdfArea(mydoc, 15, dummycolval + 127, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                        }
                        else
                        {
                            addtabletopage = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, page2col + adjustvalue, 565, 600));
                            mypdfpage.Add(addtabletopage);
                            addedcc = true;
                            tlinerect = new PdfArea(mydoc, 15, dummycolval + 7, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                        }
                        tlinerect = new PdfArea(mydoc, 57, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 120, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 256, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 350, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 476, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        page2col = page2col + getheigh;
                    }
                    else
                    {
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydoc.NewPage();
                        mypdfpage.Add(pr1);
                        page2col = 160;
                        if (addedcc == false)
                        {
                            Gios.Pdf.PdfTablePage addtabletopagenew = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, page2col + adjustvalue, 565, 600));
                            mypdfpage.Add(addtabletopagenew);
                        }
                        flag = false;
                        coltop = 20;
                        #region Left Logo
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 20, 20, 400);
                        }
                        #endregion
                        #region TOP DETAILS
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename);
                        mypdfpage.Add(ptc);
                        coltop = coltop + 25;
                        ptc = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, aff);
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString()));
                        ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "SEATING ARRANGEMENT  -  " + strMonthName.ToUpper() + " " + ddlYear.SelectedItem.Text.ToString() + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "Date & Session : " + ddlDate.SelectedItem.Text.ToString() + " & " + ddlSession.SelectedItem.Text.ToString() + " ");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        #endregion
                        table1forpage2a = mydoc.NewTable(Fontboldhead, 1, 6, 5);
                        table1forpage2a.VisibleHeaders = false;
                        table1forpage2a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table1forpage2a.Columns[0].SetWidth(40);
                        table1forpage2a.Columns[1].SetWidth(60);
                        table1forpage2a.Columns[2].SetWidth(130);
                        table1forpage2a.Columns[3].SetWidth(90);
                        table1forpage2a.Columns[4].SetWidth(120);
                        table1forpage2a.Columns[5].SetWidth(100);
                        table1forpage2a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 0).SetContent("S.No.");
                        table1forpage2a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 1).SetContent("Hall No.");
                        table1forpage2a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 2).SetContent("Degree / Branch");
                        table1forpage2a.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 3).SetContent("Subject Code");
                        table1forpage2a.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 4).SetContent("Reg. No / Seat No.");
                        table1forpage2a.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 5).SetContent("Total No of Student");
                        addtabletopage = table1forpage2a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 125, 565, 50));
                        mypdfpage.Add(addtabletopage);
                        page2col = page2col + getheigh;
                    }
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////
                mypdfpage.SaveToDocument();
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Seatingarrange_Format2" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            #endregion
        }
        if (Radioformat1.Checked == true)
        {
            #region format3
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage;
            mypdfpage = mydoc.NewPage();
            PdfArea tete = new PdfArea(mydoc, 15, 15, 565, 810);
            PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
            mypdfpage.Add(pr1);
            int coltop = 20;
            #region Left Logo
            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            {
                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                mypdfpage.Add(LogoImage, 20, 20, 400);
            }
            #endregion
            #region TOP DETAILS
            PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename);
            mypdfpage.Add(ptc);
            coltop = coltop + 25;
            ptc = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, aff);
            mypdfpage.Add(ptc);
            coltop = coltop + 20;
            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");
            mypdfpage.Add(ptc);
            coltop = coltop + 20;
            string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString()));
            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "SEATING ARRANGEMENT  -  " + strMonthName.ToUpper() + " " + ddlYear.SelectedItem.Text.ToString() + "");
            mypdfpage.Add(ptc);
            coltop = coltop + 20;
            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "Date & Session : " + ddlDate.SelectedItem.Text.ToString() + " & " + ddlSession.SelectedItem.Text.ToString() + " ");
            mypdfpage.Add(ptc);
            coltop = coltop + 20;
            //PdfArea tlinerect = new PdfArea(mydoc, 15, coltop + 25, 565, 0.01);
            //PdfRectangle plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
            //mypdfpage.Add(plimerecyt);
            #endregion
            #region header
            Gios.Pdf.PdfTable table1forpage2a = mydoc.NewTable(Fontboldhead, 2, 9, 5);
            table1forpage2a.VisibleHeaders = false;
            table1forpage2a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            for (int j = 0; j < Fpspread.Sheets[0].Columns.Count; j++)
            {
                table1forpage2a.Columns[j].SetWidth(100);
                for (int i = 0; i < Fpspread.Sheets[0].ColumnHeader.RowCount; i++)
                {
                    table1forpage2a.Cell(j, i).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table1forpage2a.Cell(j, i).SetContent(Fpspread.Sheets[0].ColumnHeader.Cells[j, i].Text.ToString());
                }
            }
            foreach (PdfCell pr in table1forpage2a.CellRange(0, 0, 0, 0).Cells)
            {
                pr.ColSpan = 3;
            }
            foreach (PdfCell pr in table1forpage2a.CellRange(0, 3, 0, 3).Cells)
            {
                pr.ColSpan = 3;
            }
            foreach (PdfCell pr in table1forpage2a.CellRange(0, 6, 0, 6).Cells)
            {
                pr.ColSpan = 3;
            }
            Gios.Pdf.PdfTablePage addtabletopage = table1forpage2a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 125, 565, 50));
            mypdfpage.Add(addtabletopage);
            #endregion
            PdfArea tlinerect = new PdfArea(mydoc, 15, coltop + 25, 565, 0.01);
            PdfRectangle plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
            Gios.Pdf.PdfTable tableparts;
            Gios.Pdf.PdfTable tablepartsduplicate;
            int snoo = 0;
            double page2col = 140;
            int adjustvalue = 6;
            Boolean addedcc = false;
            if (Fspread3.Sheets[0].RowCount > 0)
            {
                for (int i = 0; i < Fspread3.Sheets[0].RowCount; i++)
                {
                    snoo++;
                    tableparts = mydoc.NewTable(Fontsmall1, 1, 6, 2);
                    tablepartsduplicate = mydoc.NewTable(Fontsmall1, 1, 6, 2);
                    tableparts.VisibleHeaders = false;
                    tablepartsduplicate.VisibleHeaders = false;
                    tablepartsduplicate.SetBorders(Color.Black, 1, BorderType.None);
                    tableparts.SetBorders(Color.Black, 1, BorderType.None);
                    tableparts.Columns[0].SetWidth(40);
                    tableparts.Columns[1].SetWidth(60);
                    tableparts.Columns[2].SetWidth(130);
                    tableparts.Columns[3].SetWidth(90);
                    tableparts.Columns[4].SetWidth(120);
                    tableparts.Columns[5].SetWidth(100);
                    tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts.Cell(0, 1).SetContent(Fspread3.Sheets[0].Cells[i, 2].Text.ToString());
                    tableparts.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                    tableparts.Cell(0, 2).SetContent(Fspread3.Sheets[0].Cells[i, 4].Text.ToString());
                    tableparts.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts.Cell(0, 3).SetContent(Fspread3.Sheets[0].Cells[i, 5].Text.ToString());
                    tableparts.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                    tableparts.Cell(0, 4).SetContent(Fspread3.Sheets[0].Cells[i, 6].Text.ToString());
                    tableparts.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts.Cell(0, 5).SetContent(Fspread3.Sheets[0].Cells[i, 7].Text.ToString());
                    if (Fspread3.Sheets[0].Cells[i, 0].Text.ToString().Trim().ToLower() == "total")
                    {
                        tableparts.Rows[0].SetColors(Color.Black, Color.DarkGray);
                        tableparts.Rows[0].SetFont(Fontbold);
                        tableparts.Cell(0, 4).SetCellPadding(5);
                        // tableparts.Cell(0, 4).SetColors(Color.Black, Color.AliceBlue);
                        tableparts.Cell(0, 4).SetContent(Fspread3.Sheets[0].Cells[i, 0].Text.ToString() + " : ");
                        tableparts.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleRight);
                    }
                    else
                    {
                        tableparts.Cell(0, 0).SetContent(Fspread3.Sheets[0].Cells[i, 0].Text.ToString());
                    }
                    tablepartsduplicate.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepartsduplicate.Cell(0, 0).SetContent(Fspread3.Sheets[0].Cells[i, 0].Text.ToString());
                    tablepartsduplicate.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepartsduplicate.Cell(0, 1).SetContent(Fspread3.Sheets[0].Cells[i, 2].Text.ToString());
                    tablepartsduplicate.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                    tablepartsduplicate.Cell(0, 2).SetContent(Fspread3.Sheets[0].Cells[i, 4].Text.ToString());
                    tablepartsduplicate.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepartsduplicate.Cell(0, 3).SetContent(Fspread3.Sheets[0].Cells[i, 5].Text.ToString());
                    tablepartsduplicate.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                    tablepartsduplicate.Cell(0, 4).SetContent(Fspread3.Sheets[0].Cells[i, 6].Text.ToString());
                    tablepartsduplicate.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepartsduplicate.Cell(0, 5).SetContent(Fspread3.Sheets[0].Cells[i, 7].Text.ToString());
                    page2col = page2col + 5;
                    addtabletopage = tablepartsduplicate.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, page2col, 553, 600));
                    double getheigh = addtabletopage.Area.Height;
                    getheigh = Math.Round(getheigh, 2);
                    double dummycolval = page2col + getheigh;
                    if (813 > dummycolval && flag == true)
                    {
                        if (Fspread3.Sheets[0].Cells[i, 0].Text.ToString().Trim().ToLower() == "total")
                        {
                            addtabletopage = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, page2col, 565, 600));
                            mypdfpage.Add(addtabletopage);
                            addedcc = true;
                            tlinerect = new PdfArea(mydoc, 15, dummycolval + 7, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                        }
                        else
                        {
                            addtabletopage = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, page2col + adjustvalue, 565, 600));
                            mypdfpage.Add(addtabletopage);
                            addedcc = true;
                            tlinerect = new PdfArea(mydoc, 15, dummycolval + 7, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                        }
                        dummycolval = dummycolval - 120;
                        tlinerect = new PdfArea(mydoc, 57, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 120, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 256, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 350, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 476, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        page2col = page2col + getheigh;
                        addedcc = false;
                    }
                    else if (813 > dummycolval)
                    {
                        addedcc = true;
                        dummycolval = dummycolval - 120;
                        addedcc = false;
                        if (Fspread3.Sheets[0].Cells[i, 0].Text.ToString().Trim().ToLower() == "total")
                        {
                            addtabletopage = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, page2col, 565, 600));
                            mypdfpage.Add(addtabletopage);
                            addedcc = true;
                            tlinerect = new PdfArea(mydoc, 15, dummycolval + 127, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                        }
                        else
                        {
                            addtabletopage = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, page2col + adjustvalue, 565, 600));
                            mypdfpage.Add(addtabletopage);
                            addedcc = true;
                            tlinerect = new PdfArea(mydoc, 15, dummycolval + 7, 565, 0.01);
                            plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                            mypdfpage.Add(plimerecyt);
                        }
                        tlinerect = new PdfArea(mydoc, 57, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 120, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 256, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 350, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        tlinerect = new PdfArea(mydoc, 476, 125, 0.01, dummycolval);
                        plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        page2col = page2col + getheigh;
                    }
                    else
                    {
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydoc.NewPage();
                        mypdfpage.Add(pr1);
                        page2col = 160;
                        if (addedcc == false)
                        {
                            Gios.Pdf.PdfTablePage addtabletopagenew = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, page2col + adjustvalue, 565, 600));
                            mypdfpage.Add(addtabletopagenew);
                        }
                        flag = false;
                        coltop = 20;
                        #region Left Logo
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 20, 20, 400);
                        }
                        #endregion
                        #region TOP DETAILS
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, Collegename);
                        mypdfpage.Add(ptc);
                        coltop = coltop + 25;
                        ptc = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, aff);
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "OFFICE OF THE CONTROLLER OF EXAMINATIONS");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(ddlMonth.SelectedItem.Value.ToString()));
                        ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "SEATING ARRANGEMENT  -  " + strMonthName.ToUpper() + " " + ddlYear.SelectedItem.Text.ToString() + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 0, coltop, 595, 30), System.Drawing.ContentAlignment.TopCenter, "Date & Session : " + ddlDate.SelectedItem.Text.ToString() + " & " + ddlSession.SelectedItem.Text.ToString() + " ");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        #endregion
                        table1forpage2a = mydoc.NewTable(Fontboldhead, 1, 6, 5);
                        table1forpage2a.VisibleHeaders = false;
                        table1forpage2a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table1forpage2a.Columns[0].SetWidth(40);
                        table1forpage2a.Columns[1].SetWidth(60);
                        table1forpage2a.Columns[2].SetWidth(130);
                        table1forpage2a.Columns[3].SetWidth(90);
                        table1forpage2a.Columns[4].SetWidth(120);
                        table1forpage2a.Columns[5].SetWidth(100);
                        table1forpage2a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 0).SetContent("S.No.");
                        table1forpage2a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 1).SetContent("Hall No.");
                        table1forpage2a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 2).SetContent("Degree / Branch");
                        table1forpage2a.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 3).SetContent("Subject Code");
                        table1forpage2a.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 4).SetContent("Reg. No / Seat No.");
                        table1forpage2a.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1forpage2a.Cell(0, 5).SetContent("Total No of Student");
                        addtabletopage = table1forpage2a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 125, 565, 50));
                        mypdfpage.Add(addtabletopage);
                        page2col = page2col + getheigh;
                    }
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////
                mypdfpage.SaveToDocument();
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Seatingarrange_Format2" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            #endregion
        }
    }

    protected void btn1_print(object sender, EventArgs e)
    {
        try
        {
            string date = ddlDate.SelectedItem.Text;
            string pagename = "seatingarrange.aspx";
            string degreedetails = "Exam Seating Arrangment";
            // degreedetails = degreedetails + "@Date : " + ddlDate.SelectedItem.Text + "";
            if (ddlDate.SelectedItem.Text != "All")
            {
                degreedetails = degreedetails + "@Date & Session : " + ddlDate.SelectedItem.ToString() + " & " + ddlSession.SelectedItem.ToString() + "";
            }
            //if (Radioformat3.Checked == true)
            //{
            //string date = "@" + "Date       :" + ddlDate.SelectedItem.Text + "   ";
            //string pagename = "seatingarrange.aspx";
            //string degreedetails = date + "                                                                 Exam Seating Arrangment" + "                                                                                      Session :" + ddlSession.SelectedItem.Text;
            Fspread3.Sheets[0].ColumnHeader.Visible = true;
            Printcontrol.loadspreaddetails(Fspread3, pagename, degreedetails);
            Printcontrol.Visible = true;
            Fspread3.Sheets[0].ColumnHeader.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    protected void printseating_click(object sender, EventArgs e)
    {
        try
        {
            string pagename = "seatingarrange.aspx";
            string degreedetails = "Office of the Controller of Examinations$SEATING ARRANGEMENT @Date & Session : " + ddlDate.SelectedItem.Text + " & " + ddlSession.SelectedItem.Text;
            Printcontrol.loadspreaddetails(Fpseating, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void Excelseating_click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexseat.Text;
            if (report.ToString().Trim() != "")
            {
                dt.printexcelreport(Fpseating, report);
                lblmessage1.Visible = false;
            }
            else
            {
                lblmessage1.Text = "Please Enter Your Report Name";
                lblmessage1.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        clear();
        if (Radioformat3.Checked == true)
        {
            formatthree();
        }
        else if (Radioformat2.Checked == true)
        {
            ddlhall.Enabled = false;
            if (ddlhall.Items.Count == 0)
            {
                string query = "select * from class_master where coll_code='" + college_code + "'  order by priority";
                DataSet dn = new DataSet();
                dn = dt.select_method_wo_parameter(query, "text");
            }
            go();
            Fpspread.Visible = false;
            pnlContent1.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            btnDirectPrint.Visible = false;
            lblrptname.Visible = false;
        }
        else
        {
            hallbind();
        }
    }

    protected void btn_excel(object sender, EventArgs e)
    {
        try
        {
            string report = txtreportname2.Text;
            if (report.ToString().Trim() != "")
            {
                Fspread3.Sheets[0].ColumnHeader.Visible = true;
                dt.printexcelreport(Fspread3, report);
                lblmessage1.Visible = false;
                Fspread3.Sheets[0].ColumnHeader.Visible = false;
            }
            else
            {
                lblmessage1.Text = "Please Enter Your Report Name";
                lblmessage1.Visible = true;
            }
        }
        catch (Exception ex)
        { }
    }

    #region Added By Malang Raja On Nov 04 2016

    public void Bindcollege()
    {
        try
        {
            string columnfield = string.Empty;
            string group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = dt.select_method("bind_college", hat, "sp");
            cblCollege.Items.Clear();
            chkCollege.Checked = false;
            txtCollege.Text = "--Select--";
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                cblCollege.DataSource = dsprint;
                cblCollege.DataTextField = "collname";
                cblCollege.DataValueField = "college_code";
                cblCollege.DataBind();
                foreach (wc.ListItem li in cblCollege.Items)
                {
                    li.Selected = true;
                }
            }
            else
            {
                //errmsg.Text = "Set college rights to the staff";
                //errmsg.Visible = true;
                //return;
            }
            CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkCollege_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            CallCheckboxChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            mode();
            loadhall();
        }
        catch
        {
        }
    }

    protected void cblCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            mode();
            loadhall();
        }
        catch
        {
        }
    }

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
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
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
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
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
            string name = string.Empty;
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
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
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

    protected void chkmergrecol_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtCollege.Enabled = false;
            if (!chkmergrecol.Checked)
            {
                txtCollege.Enabled = true;
            }
            Bindcollege();
            mode();
            loadhall();
            loadBlock();
        }
        catch
        {
        }
    }

    #endregion

    private bool ExamSeatingArrangementLock()
    {
        bool isLock = false;
        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " and group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "' ";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "' ";
            }
            ds.Clear();
            string lockTimeTableGeneration = string.Empty;
            //if (!string.IsNullOrEmpty(grouporusercode))
            //{
            string Master1 = "select value from Master_Settings where settings='COE Exam Seating Arrangement Generation Lock' and  value='1' --" + grouporusercode + "";
            lockTimeTableGeneration = dt.GetFunction(Master1);
            //}
            if (!string.IsNullOrEmpty(lockTimeTableGeneration) && lockTimeTableGeneration.Trim() == "1")
            {
                isLock = false;
            }
            else
            {
                isLock = true;
            }
            return isLock;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    protected void chkIncludeBlock_CheckedChanged(object sender, EventArgs e)
    {
        divBlock.Visible = chkIncludeBlock.Checked;
        lblBlock.Visible = chkIncludeBlock.Checked;
        loadBlock();
    }

    protected void chkBlock_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            CallCheckboxChange(chkBlock, cblBlock, txtBlock, lblBlock.Text, "--Select--");
            //loadhall();
        }
        catch
        {
        }
    }

    protected void cblBlock_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            CallCheckboxListChange(chkBlock, cblBlock, txtBlock, lblBlock.Text, "--Select--");
            //loadhall();
        }
        catch
        {
        }
    }

    #region Generate Excel

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;

            printCommonPdf.Visible = false;
            string reportname = txtExcelNameMissing.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpStudentList.Visible == true)
                {
                    dt.printexcelreport(FpStudentList, reportname);
                }
                lblExcelErr.Visible = false;
            }
            else
            {
                lblExcelErr.Text = "Please Enter Your Report Name";
                lblExcelErr.Visible = true;
                txtExcelNameMissing.Focus();
            }
        }
        catch (Exception ex)
        {

        }
    }

    #endregion Generate Excel

    #region Alert Popup Close

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    #region Print PDF

    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;

            string rptheadname = string.Empty;
            rptheadname = "Missing Student of Seating Arrangement";
            string pagename = "seatingarrange.aspx";
            //string Course_Name = Convert.ToString(ddlDegree.SelectedItem).Trim();
            //rptheadname += "$" + ((ddlTest.Items.Count > 0) ? ddlTest.SelectedItem.Text : "") + "@ " + Course_Name + " - " + Convert.ToString(ddlBranch.SelectedItem).Trim() + ((ddlSec.Items.Count == 0) ? "" : (ddlSec.Items.Count > 0 && !string.IsNullOrEmpty(ddlSec.SelectedItem.Text.Trim()) && ddlSec.SelectedItem.Text.Trim().ToLower() != "all") ? " - " + ddlSec.SelectedItem.Text.Trim() : "") + "@ " + " Year of Admission : " + Convert.ToString(ddlBatch.SelectedItem).Trim() + "@ " + " " + lblSem.Text.Trim() + " : " + Convert.ToString(ddlSem.SelectedItem).Trim();
            if (FpStudentList.Visible == true)
            {
                printCommonPdf.loadspreaddetails(FpStudentList, pagename, rptheadname);
            }
            printCommonPdf.Visible = true;
            lblExcelErr.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    #endregion Print PDF

    private string orderByStudents(string collegeCode, string aliasName = null, string tableName = null, byte includeOrderBy = 0)
    {
        string orderBy = string.Empty;
        try
        {
            string orderBySetting = dirAcc.selectScalarString("select value from master_Settings where settings='order_by' ");//and value<>''
            orderBySetting = orderBySetting.Trim();

            string serialNo = dirAcc.selectScalarString("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");

            string aliasOrTableName = ((string.IsNullOrEmpty(aliasName) && string.IsNullOrEmpty(tableName)) ? "" : ((!string.IsNullOrEmpty(tableName)) ? tableName.Trim() + "." : ((!string.IsNullOrEmpty(aliasName)) ? aliasName.Trim() + "." : "")));

            orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
            if (serialNo.Trim().ToLower() == "1" || serialNo.ToLower().Trim() == "true")
                orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "serialno";
            else
                switch (orderBySetting)
                {
                    case "0":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
                        break;
                    case "1":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Reg_No";
                        break;
                    case "2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,1,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No," + aliasOrTableName + "stud_name";
                        break;
                    case "0,1":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No";
                        break;
                    case "1,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Reg_No," + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Stud_Name";
                        break;
                    default:
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
                        break;
                }
        }
        catch (Exception ex)
        {

        }
        return orderBy;
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type , 4 For Application No</param>
    /// <param name="dsSettingsOptional">it is Optional Parameter</param>
    /// <returns>true or false</returns>
    private bool ColumnHeaderVisiblity(int type, DataSet dsSettingsOptional = null)
    {
        bool hasValues = false;
        try
        {
            DataSet dsSettings = new DataSet();
            if (dsSettingsOptional == null)
            {
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    string groupCode = Convert.ToString(Session["group_code"]).Trim();
                    string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                    if (groupUser.Length > 0)
                    {
                        groupCode = groupUser[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(groupCode.Trim()))
                    {
                        grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                    }
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type','Application No') and value='1' " + grouporusercode + "";
                    dsSettings = dirAcc.selectDataSet(Master1);
                }
            }
            else
            {
                dsSettings = dsSettingsOptional;
            }
            if (dsSettings.Tables.Count > 0 && dsSettings.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSettings in dsSettings.Tables[0].Rows)
                {
                    switch (type)
                    {
                        case 0:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "roll no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 1:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "register no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 2:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "admission no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 3:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "student_type")
                            {
                                hasValues = true;
                            }
                            break;
                        case 4:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "application no")
                            {
                                hasValues = true;
                            }
                            break;
                    }
                    if (hasValues)
                        break;
                }
            }
            return hasValues;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public void Init_Spread(Farpoint.FpSpread FpSpread1, int type = 0)
    {
        try
        {
            #region FpSpread Style

            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            #endregion FpSpread Style

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;
            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Center;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.HorizontalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.VerticalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            Dictionary<string, byte> dicColumnVisiblity = new Dictionary<string, byte>();
            //columnVisibility(ref dicColumnVisiblity);
            bool isRollNoVisible = ColumnHeaderVisiblity(0);
            bool isRegNoVisible = ColumnHeaderVisiblity(1);
            bool isAdmissionNoVisible = ColumnHeaderVisiblity(2);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3);
            bool isVisibleColumn = false;
            if (type == 0)
            {
                FpSpread1.Sheets[0].ColumnCount = 8;

                byte value = 0;
                FpSpread1.Sheets[0].Columns[0].Width = 35;
                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].Columns[1].Width = 100;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = isRollNoVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = isRegNoVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = isAdmissionNoVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].Columns[4].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = isStudentTypeVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].Columns[5].Width = 85;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].Columns[6].Width = 220;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject Code";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

                FpSpread1.Sheets[0].Columns[7].Width = 220;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Subject Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);

            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 3;

                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 150;
                FpSpread1.Sheets[0].Columns[2].Width = 250;

                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;

                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;

                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnMissingStudent_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            string qryCollegeCode = string.Empty;
            string qryDate = string.Empty;
            string qrySession = string.Empty;
            string qryType = string.Empty;
            string qryExamYear = string.Empty;
            string qryExamMonth = string.Empty;
            string examYear = string.Empty;
            string examMonth = string.Empty;

            string qryExamYear1 = string.Empty;
            string qryExamMonth1 = string.Empty;
            string qryDate1 = string.Empty;
            string qrySession1 = string.Empty;

            DataTable dtStudentList = new DataTable();
            DataTable dtAllStudentInfo = new DataTable();
            DataTable dtNotAllotedStudents = new DataTable();
            if (chkmergrecol.Checked)
            {
                collegeCode = Convert.ToString(Session["collegecode"]).Trim();
            }
            else if (cblCollege.Items.Count > 0)
            {
                collegeCode = getCblSelectedValue(cblCollege);
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and r.College_code in(" + collegeCode + ")";
            }
            if (ddlYear.Items.Count > 0)
            {
                if (ddlYear.SelectedValue.Trim().ToLower() != "" && ddlYear.SelectedValue.Trim().ToLower() != "0" && ddlYear.SelectedValue.Trim().ToLower() != "all")
                {
                    examYear = Convert.ToString(ddlYear.SelectedValue).Trim();
                }
                if (!string.IsNullOrEmpty(examYear))
                {
                    qryExamYear = " and ed.Exam_year='" + examYear + "'";
                    qryExamYear1 = " and et.Exam_year='" + examYear + "'";
                }
            }
            else
            {
                lblAlertMsg.Text = "No Exam Year Found";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlMonth.Items.Count > 0)
            {
                if (ddlMonth.SelectedValue.Trim().ToLower() != "" && ddlMonth.SelectedValue.Trim().ToLower() != "0" && ddlMonth.SelectedValue.Trim().ToLower() != "all")
                {
                    examMonth = Convert.ToString(ddlMonth.SelectedValue).Trim();
                }
                if (!string.IsNullOrEmpty(examMonth))
                {
                    qryExamMonth = " and ed.Exam_Month='" + examMonth + "' ";
                    qryExamMonth1 = "  and et.Exam_month='" + examMonth + "'";
                }
            }
            else
            {
                lblAlertMsg.Text = "No Exam Month Found";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlDate.Items.Count > 0)
            {
                if (ddlDate.SelectedItem.Text.ToLower().Trim() != "all")
                {
                    string edate = ddlDate.SelectedItem.Text.ToString().Trim();
                    string[] spd = edate.Split('-');
                    qryDate = " and etd.exam_date='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                    qryDate1 = " and es.edate='" + spd[1] + '-' + spd[0] + '-' + spd[2] + "'";
                }
            }
            else
            {
                lblAlertMsg.Text = "No Exam Date Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlSession.Items.Count > 0)
            {
                if (ddlSession.SelectedItem.Text.ToLower().Trim() != "all" && ddlSession.SelectedItem.Text.ToLower().Trim() != "both" && ddlSession.SelectedItem.Text.ToLower().Trim() != "")
                {
                    qrySession = " and etd.exam_session='" + ddlSession.SelectedItem.ToString() + "'";
                    qrySession1 = " and es.ses_sion='" + ddlSession.SelectedItem.ToString() + "'";
                }
            }
            else
            {
                lblAlertMsg.Text = "No Exam Session Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (ddltype.Items.Count > 0)
            {
                if (ddltype.SelectedItem.Text.ToLower().Trim() != "all" && ddltype.SelectedItem.Text.ToLower().Trim() != "both" && ddltype.SelectedItem.Text.Trim().ToLower() != "")
                {
                    qryType = "and c.type='" + ddltype.SelectedItem.Text.Trim() + "'";
                }
                if (!string.IsNullOrEmpty(collegeCode) && collegeCode.Contains(','))
                {
                    qryType = string.Empty;
                }
            }
            if (chkmergrecol.Checked == true)
            {
                qryType = string.Empty;
            }

            string qry = " select ed.subject_no,case when ISNULL(tq.Com_Subject_Code,'')<>'' then tq.Com_Subject_Code else s.subject_code end subject_code,s.subject_name,et.exam_date,et.exam_session,de.Dept_Name,de.Dept_Code,c.Course_Name,c.type,r.Reg_No,e.degree_code,e.batch_year from Exam_Details e,exam_application ea,exam_appl_details ed,exmtt_det et, Registration r,Degree d,course c,Department de,subject s left join tbl_equal_paper_Matching tq on tq.Equal_Subject_Code=s.subject_code where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and ea.roll_no =r.Roll_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and et.subject_no=ed.subject_no and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code " + qryType + "  and e.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' " + qryDate + " " + qrySession + qryCollegeCode + " order by et.exam_date,et.exam_session,c.type,e.degree_code,e.batch_year desc,r.Reg_No asc,s.subject_code";
            //dtStudentList = dirAcc.selectDataTable(qry);
            //if()
            qry = " select  s.subject_no,s.subject_code,s.subject_name,etd.exam_date,etd.exam_session,dt.Dept_Name,dt.Dept_Code,c.Course_Name,c.type,r.App_No,r.Reg_No,r.Roll_No,r.Stud_Name,r.Stud_Type,r.Roll_Admit from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,exmtt et,exmtt_det etd,Course c,Degree dg,Department dt where c.Course_Id=dg.Course_Id and dg.Degree_Code=r.degree_code and dg.Degree_Code=ed.degree_code and dg.Degree_Code=et.degree_code and dg.Dept_Code=dt.Dept_Code and et.exam_code=etd.exam_code and etd.subject_no=s.subject_no and ead.subject_no=etd.subject_no and et.degree_code=r.degree_code and et.degree_code=dg.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and ead.subject_no=s.subject_no  " + qryExamMonth + qryExamYear + qryType + qryDate + qrySession + qryCollegeCode + " ";
            //--order by s.subject_code,c.Course_Name,dt.Dept_Name,r.Reg_no
            dtAllStudentInfo = dirAcc.selectDataTable(qry);

            //and etd.exam_date='11/10/2017' and etd.exam_session='F.N' and r.college_code in(13,14) and ed.Exam_Month='11' and et.Exam_month='11' and ed.Exam_year='2017' and et.Exam_year='2017'
            //qry = "select distinct r.App_No from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,exmtt et,exmtt_det etd where et.exam_code=etd.exam_code and etd.subject_no=s.subject_no and ead.subject_no=etd.subject_no and et.degree_code=r.degree_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and ead.subject_no=s.subject_no  " + qryExamMonth + qryExamYear + qryExamYear1 + qryExamMonth1 + qryCollegeCode + qryType + qryDate + qrySession + " except select distinct r.App_No from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,subject s,Registration r where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no  and es.subject_no=s.subject_no and ead.subject_no=s.subject_no and es.regno=r.Reg_No and ea.roll_no=r.Roll_No and ed.degree_code=r.degree_code and ed.degree_code=es.degree_code and r.degree_code=es.degree_code " + qryCollegeCode + qryType + qryDate1 + qrySession1 + qryExamYear + qryExamMonth + "";
            //and Exam_Month='11' and ed.Exam_year='2017'

            DataTable dtAppliedStudents = new DataTable();
            DataTable dtAllotedStudents = new DataTable();

            qry = "select r.App_No from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,Registration r,exmtt et,exmtt_det etd,Course c,Degree dg where c.Course_Id=dg.Course_Id and r.degree_code=dg.Degree_Code and et.degree_code=dg.Degree_Code and dg.Degree_Code=ed.degree_code and ed.degree_code=r.degree_code and r.Batch_Year=ed.batch_year and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ea.roll_no=r.Roll_No and et.degree_code=r.degree_code and ed.degree_code=et.degree_code and ead.subject_no=s.subject_no and et.exam_code=etd.exam_code and etd.subject_no=ead.subject_no and ead.subject_no=s.subject_no " + qryExamMonth + qryExamYear + qryExamYear1 + qryExamMonth1 + qryCollegeCode + qryDate + qryType + qrySession + "";
            dtAppliedStudents = dirAcc.selectDataTable(qry);

            //qry = "select r.App_No from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,subject s,Registration r where r.Batch_Year=ed.batch_year and ed.degree_code=r.degree_code and r.Reg_No=es.regno and ed.exam_code=ea.exam_code and ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and s.subject_no=es.subject_no   " + qryCollegeCode + qryDate1 + qrySession1 + qryExamYear + qryType + qryExamMonth + "";

            qry = " select r.App_No from Exam_Details ed,exam_application ea,exam_appl_details ead,exam_seating es,subject s,Registration r,Course c,Degree d where d.Degree_Code=r.degree_code and d.Course_Id=c.Course_Id and ed.degree_code=d.Degree_Code and r.Batch_Year=ed.batch_year and ed.degree_code=r.degree_code and r.Reg_No=es.regno and ed.exam_code=ea.exam_code and ea.roll_no=r.Roll_No and s.subject_no=ead.subject_no and ea.appl_no=ead.appl_no and ead.subject_no=es.subject_no and s.subject_no=es.subject_no  " + qryCollegeCode + qryDate1 + qrySession1 + qryExamYear + qryType + qryExamMonth + "";

            dtAllotedStudents = dirAcc.selectDataTable(qry);

            List<decimal> lstAllotedStudents = new List<decimal>();
            List<decimal> lstAppliedStudents = new List<decimal>();
            List<string> lstNotAllotedStudents = new List<string>();

            if (dtAppliedStudents.Rows.Count > 0)
            {
                lstAppliedStudents = dtAppliedStudents.AsEnumerable().Select(r => r.Field<decimal>("App_No")).Distinct().ToList();
            }
            else
            {
                //lblAlertMsg.Text = "No Student Were Applied";
                //divPopAlert.Visible = true;
                //return;
            }
            if (dtAllotedStudents.Rows.Count > 0)
            {
                lstAllotedStudents = dtAllotedStudents.AsEnumerable().Select(r => r.Field<decimal>("App_No")).Distinct().ToList();
            }
            else
            {
                //lblAlertMsg.Text = "No Seating Generated";
                //divPopAlert.Visible = true;
                //return;
            }
            dtNotAllotedStudents.Rows.Clear();
            dtNotAllotedStudents.Columns.Clear();
            dtNotAllotedStudents.Columns.Add("app_No");
            if (lstAppliedStudents.Count > 0)
            {
                foreach (decimal item in lstAppliedStudents)
                {
                    DataRow drNotAlloted;
                    if (lstAllotedStudents.Count > 0)
                    {
                        if (!lstAllotedStudents.Contains(item))
                        {
                            drNotAlloted = dtNotAllotedStudents.NewRow();
                            drNotAlloted["app_No"] = item;
                            dtNotAllotedStudents.Rows.Add(drNotAlloted);
                        }
                    }
                }
            }
            if (dtNotAllotedStudents.Rows.Count > 0)
            {
                lstNotAllotedStudents = dtNotAllotedStudents.AsEnumerable().Select(r => r.Field<string>("App_No")).Distinct().ToList();
                string qryAppNo = string.Empty;
                string appNoList = string.Join("','", lstNotAllotedStudents.ToArray());

                int sno = 0;
                DataView dvStudent = new DataView();
                if (!string.IsNullOrEmpty(appNoList))
                {
                    qryAppNo = " app_no in('" + appNoList + "')";
                    dtAllStudentInfo.DefaultView.RowFilter = "app_no in('" + appNoList + "')";
                    dtAllStudentInfo.DefaultView.Sort = "Reg_No asc,subject_code";
                    dvStudent = dtAllStudentInfo.DefaultView;
                }
                if (dvStudent.Count > 0)
                {
                    Init_Spread(FpStudentList);
                    foreach (DataRowView drStudent in dvStudent)
                    {
                        string appNo = Convert.ToString(drStudent["App_No"]).Trim();

                        FpStudentList.Sheets[0].RowCount++;
                        sno++;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(drStudent["Roll_No"]).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].CellType = new Farpoint.TextCellType();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;


                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(drStudent["Reg_No"]).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].CellType = new Farpoint.TextCellType();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(drStudent["Roll_Admit"]).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].CellType = new Farpoint.TextCellType();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(drStudent["Stud_Type"]).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].CellType = new Farpoint.TextCellType();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(drStudent["Stud_Name"]).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].CellType = new Farpoint.TextCellType();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(drStudent["subject_code"]).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].CellType = new Farpoint.TextCellType();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(drStudent["subject_name"]).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].CellType = new Farpoint.TextCellType();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;

                    }
                }
                FpStudentList.SaveChanges();
                FpStudentList.Sheets[0].PageSize = FpStudentList.Sheets[0].RowCount;
                FpStudentList.Height = 500;
                FpStudentList.SaveChanges();
                FpStudentList.Visible = true;
                divMainContents.Visible = true;
            }
            else
            {
                lblAlertMsg.Text = "No Student Found";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch
        {
        }
    }

    protected void chkForSeating_CheckedChanged(object sender, EventArgs e)
    {
        if (chkForSeating.Checked == true)
        {
            div1.Visible = true;
        }
        else
        {
            div1.Visible = false;
        }
    }
}