using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class Result_Analysis_new : System.Web.UI.Page
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
    string subtype = string.Empty;
    string subname = string.Empty;

    string fromsem = String.Empty;
    string tosem = String.Empty;
    string fromsem1 = String.Empty;
    string tosem1 = String.Empty;

    int i, row, commcount = 0;

    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();

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
                bindSubtype();
                //bindSubname();

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
                divspread.Visible = false;
                rptprint.Visible = false;

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBtch();
            binddeg();
            binddept();
            bindSubtype();
            bindSubname();
        }
        catch { }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {

            //txt_batch.Text = "--Select--";
            //if (cb_batch.Checked == true)
            //{

            //    for (i = 0; i < cbl_batch.Items.Count; i++)
            //    {
            //        cbl_batch.Items[i].Selected = true;
            //    }
            //    txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
            //}
            //else
            //{
            //    for (i = 0; i < cbl_batch.Items.Count; i++)
            //    {
            //        cbl_batch.Items[i].Selected = false;
            //    }
            //}

            //binddeg();
            //binddept();
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //i = 0;
            //cb_batch.Checked = false;
            //commcount = 0;
            //txt_batch.Text = "--Select--";
            //for (i = 0; i < cbl_batch.Items.Count; i++)
            //{
            //    if (cbl_batch.Items[i].Selected == true)
            //    {
            //        commcount = commcount + 1;
            //    }
            //}
            //if (commcount > 0)
            //{
            //    if (commcount == cbl_batch.Items.Count)
            //    {
            //        cb_batch.Checked = true;
            //    }
            //    txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            //}
            //binddeg();
            //binddept();
        }
        catch { }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //i = 0;
            //cb_degree.Checked = false;
            //commcount = 0;
            //txt_degree.Text = "--Select--";
            //for (i = 0; i < cbl_degree.Items.Count; i++)
            //{
            //    if (cbl_degree.Items[i].Selected == true)
            //    {
            //        commcount = commcount + 1;
            //    }
            //}
            //if (commcount > 0)
            //{
            //    if (commcount == cbl_degree.Items.Count)
            //    {
            //        cb_degree.Checked = true;
            //    }
            //    txt_degree.Text = "Degree(" + commcount.ToString() + ")";
            //}
            //binddept();
        }
        catch { }
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //txt_dept.Text = "--Select--";
            //if (cb_dept.Checked == true)
            //{

            //    for (i = 0; i < cbl_dept.Items.Count; i++)
            //    {
            //        cbl_dept.Items[i].Selected = true;
            //    }
            //    txt_dept.Text = "Deartment(" + (cbl_dept.Items.Count) + ")";
            //}
            //else
            //{
            //    for (i = 0; i < cbl_dept.Items.Count; i++)
            //    {
            //        cbl_dept.Items[i].Selected = false;
            //    }
            //}

        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //i = 0;
            //cb_dept.Checked = false;
            //commcount = 0;
            //txt_dept.Text = "--Select--";
            //for (i = 0; i < cbl_dept.Items.Count; i++)
            //{
            //    if (cbl_dept.Items[i].Selected == true)
            //    {
            //        commcount = commcount + 1;
            //    }
            //}
            //if (commcount > 0)
            //{
            //    if (commcount == cbl_dept.Items.Count)
            //    {
            //        cb_dept.Checked = true;
            //    }
            //    txt_dept.Text = "Department(" + commcount.ToString() + ")";
            //}
        }
        catch { }
    }
    protected void cb__subtype_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_subtype.Text = "--Select--";
            if (cb_subtype.Checked == true)
            {

                for (i = 0; i < cbl_subtype.Items.Count; i++)
                {
                    cbl_subtype.Items[i].Selected = true;
                }
                txt_subtype.Text = "Subject Type(" + (cbl_subtype.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_subtype.Items.Count; i++)
                {
                    cbl_subtype.Items[i].Selected = false;
                }
            }
            // bindSubname();
        }
        catch { }
    }
    protected void cbl_subtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_subtype.Checked = false;
            commcount = 0;
            txt_subtype.Text = "--Select--";
            for (i = 0; i < cbl_subtype.Items.Count; i++)
            {
                if (cbl_subtype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_subtype.Items.Count)
                {
                    cb_subtype.Checked = true;
                }
                txt_subtype.Text = "Subject Type(" + commcount.ToString() + ")";
            }
            //  bindSubname();
        }
        catch { }
    }
    protected void cb_subname_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //txt_subname.Text = "--Select--";
            //if (cb_subname.Checked == true)
            //{

            //    for (i = 0; i < cbl_subname.Items.Count; i++)
            //    {
            //        cbl_subname.Items[i].Selected = true;
            //    }
            //    txt_subname.Text = "Subject Name(" + (cbl_subname.Items.Count) + ")";
            //}
            //else
            //{
            //    for (i = 0; i < cbl_subname.Items.Count; i++)
            //    {
            //        cbl_subname.Items[i].Selected = false;
            //    }
            //}
        }
        catch { }
    }
    protected void cbl_subname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //i = 0;
            //cb_subname.Checked = false;
            //commcount = 0;
            //txt_subname.Text = "--Select--";
            //for (i = 0; i < cbl_subname.Items.Count; i++)
            //{
            //    if (cbl_subname.Items[i].Selected == true)
            //    {
            //        commcount = commcount + 1;
            //    }
            //}
            //if (commcount > 0)
            //{
            //    if (commcount == cbl_subname.Items.Count)
            //    {
            //        cb_subname.Checked = true;
            //    }
            //    txt_subname.Text = "Subject Name(" + commcount.ToString() + ")";
            //}
        }
        catch { }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            college = "";
            batch = "";
            degree = "";
            dept = "";
            subtype = "";
            subname = "";
            fromsem = "";
            tosem = "";
            fromsem1 = "";
            tosem1 = "";

            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
            divspread.Visible = false;
            rptprint.Visible = false;
            lbl_error.Text = "No Records Found";
            lbl_error.Visible = true;

            if (ddl_college.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_degree.Items.Count > 0 && ddl_dept.Items.Count > 0)
            {
                college = Convert.ToString(ddl_college.SelectedValue);
                batch = Convert.ToString(ddl_batch.SelectedValue);
                degree = Convert.ToString(ddl_degree.SelectedValue);
                dept = Convert.ToString(ddl_dept.SelectedValue);

                fromsem = Convert.ToString(txt_semfrom.Text.Trim());
                tosem = Convert.ToString(txt_semto.Text.Trim());

                //fromsem1 = Convert.ToString(txt_fromsembot.Text.Trim());
                //tosem1 = Convert.ToString(txt_tosembot.Text.Trim());


                subtype = "";
                if (cbl_subtype.Items.Count > 0)
                {
                    for (i = 0; i < cbl_subtype.Items.Count; i++)
                    {
                        if (cbl_subtype.Items[i].Selected == true)
                        {
                            if (subtype == "")
                            {
                                subtype = Convert.ToString(cbl_subtype.Items[i].Text);
                            }
                            else
                            {
                                subtype = subtype + "','" + Convert.ToString(cbl_subtype.Items[i].Text);
                                
                                //if (subtype == "PRACTICAL" && subtype == "Practical" && subtype == "practical")
                                //{
                                //    string subtypes = "Practicals";
                                //    subtype = subtype + "','" + subtypes;

                                //}

                            }
                        }
                    }
                }
               
               
                //subname = "";
                //if (cbl_subname.Items.Count > 0)
                //{
                //    for (i = 0; i < cbl_subname.Items.Count; i++)
                //    {
                //        if (cbl_subname.Items[i].Selected == true)
                //        {
                //            if (subname == "")
                //            {
                //                subname = Convert.ToString(cbl_subname.Items[i].Value);
                //            }
                //            else
                //            {
                //                subname = subname + "," + Convert.ToString(cbl_subname.Items[i].Value);
                //            }
                //        }
                //    }
                //}

                if (subtype != "")
                {
                    selectQuery = "select subject_type,s.subject_no,subject_code,subject_name,semester  from syllabus_master y,subject s,sub_sem u where  y.syll_code = s.syll_code and y.syll_code = u.syll_code and s.subType_no = u.subType_no and y.degree_code = '" + dept + "' and y.batch_year = '" + batch + "' and subject_type in ( '" + subtype + "') and semester between '" + fromsem + "' and '" + tosem + "' order by subject_type,semester ";

                    selectQuery += " select m.exam_code,Exam_Month,Exam_year,sy.semester,subject_type,m.subject_no,count(distinct r.roll_no) as appear from Registration r,mark_entry m,subject s,syllabus_master sy,sub_sem ss,Exam_Details ed where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and m.subject_no=s.subject_no and m.roll_no=r.Roll_No and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and m.exam_code=ed.exam_code and r.Batch_Year='" + batch + "' and r.degree_code='" + dept + "' and r.DelFlag=0 and m.attempts <=1 and ss.subject_type in( '" + subtype + "') and semester between '" + fromsem + "' and '" + tosem + "' group by m.exam_code,Exam_Month,Exam_year,sy.semester,subject_type,m.subject_no  order by m.exam_code,sy.semester ,m.subject_no ";

                    selectQuery += " select m.exam_code,Exam_Month,Exam_year,sy.semester,subject_type,m.subject_no,count(distinct r.roll_no) as fail from Registration r,mark_entry m,subject s,syllabus_master sy,sub_sem ss,Exam_Details ed where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and m.subject_no=s.subject_no and m.roll_no=r.Roll_No and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and m.exam_code=ed.exam_code and r.Batch_Year='" + batch + "' and r.degree_code='" + dept + "' and r.DelFlag=0 and ss.subject_type in('" + subtype + "') and sy.semester between '" + fromsem + "' and '" + tosem + "' and m.result<>'Pass' group by      m.exam_code,Exam_Month,Exam_year,sy.semester,subject_type,m.subject_no order by    m.exam_code,Exam_Month,Exam_year,sy.semester,subject_type,m.subject_no ";

                    selectQuery += "   select m.exam_code,Exam_Month,Exam_year,sy.semester,subject_type,m.subject_no ,count(distinct r.roll_no) as fail from Registration r,mark_entry m,subject s,syllabus_master sy ,sub_sem ss,Exam_Details ed where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and m.subject_no=s.subject_no and m.roll_no=r.Roll_No and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and m.exam_code=ed.exam_code and r.Batch_Year='" + batch + "' and r.degree_code='" + dept + "' and r.DelFlag=0 and ss.subject_type in('" + subtype + "') and sy.semester between '" + fromsem + "' and '" + tosem + "' and ltrim(rtrim(type))='' and (result ='AA' or result =  'AAA' or result = 'UA')  group by  m.exam_code,Exam_Month,Exam_year,sy.semester,subject_type,m.subject_no order by      m.exam_code,Exam_Month,Exam_year,sy.semester,subject_type,m.subject_no ";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQuery, "Text");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 3;
                        // FpSpread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
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

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Semester";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].Width = 100;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Month & Year";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[2].Width = 150;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);


                        ArrayList listsem = new ArrayList();
                        for (row = Convert.ToInt32(fromsem); row <= Convert.ToInt32(tosem); row++)
                        {
                            listsem.Add(row);
                        }
                        Hashtable counthash = new Hashtable();
                        Hashtable indexcount = new Hashtable();
                        int typececk = 0;
                        int check = 0;
                        for (row = 0; row < cbl_subtype.Items.Count; row++)
                        {
                            typececk = 0;
                           if (cbl_subtype.Items[row].Selected == true)
                            {
                                foreach (int index in listsem)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                    DataView dv = ds.Tables[0].DefaultView;

                                    if (dv.Count > 0)
                                    {
                                        if (typececk < dv.Count)
                                        {
                                            typececk = dv.Count;
                                        }
                                    }

                                }
                                counthash.Add(Convert.ToString(cbl_subtype.Items[row].Text), typececk);
                            }
                        }
                        foreach (int index in listsem)
                        {
                            if (check == 0)
                            {
                                for (row = 0; row < cbl_subtype.Items.Count; row++)
                                {
                                    if (cbl_subtype.Items[row].Selected == true)
                                    {

                                        ds.Tables[0].DefaultView.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                        DataView dv = ds.Tables[0].DefaultView;

                                        if (dv.Count > 0)
                                        {
                                            check++;
                                            FpSpread1.Sheets[0].ColumnCount += 1;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Avg " + cbl_subtype.Items[row].Text;

                                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            int columncount = Convert.ToInt32(counthash[Convert.ToString(cbl_subtype.Items
[row].Text)]);
                                            indexcount.Add(Convert.ToString(cbl_subtype.Items[row].Text), FpSpread1.Sheets[0].ColumnCount);

                                            if (!indexcount.ContainsKey("practicals"))
                                            {
                                                if (indexcount.ContainsKey("Practical") || indexcount.ContainsKey("practical") || indexcount.ContainsKey("PRACTICAL"))
                                                {
                                                    indexcount.Add("practicals", FpSpread1.Sheets[0].ColumnCount);
                                                }
                                            }

                                            FpSpread1.Sheets[0].ColumnCount += Convert.ToInt32(counthash[Convert.ToString(cbl_subtype.Items[row].Text)]);


                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - columncount].Text = cbl_subtype.Items[row].Text;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - columncount].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - columncount].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - columncount].Font.Size = FontUnit.Medium;

                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - columncount].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - columncount, 1, columncount);
                                            int new1 = 0;
                                            for (i = columncount; i > 0; i--)
                                            {
                                                if (new1 < dv.Count)
                                                {
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - i].Text = Convert.ToString(dv[new1]["subject_code"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - i].Tag = Convert.ToString(dv[new1]["subject_no"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - i].Font.Bold = true;
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - i].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - i].Font.Size = FontUnit.Medium;

                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - i].HorizontalAlign = HorizontalAlign.Center;
                                                    new1++;
                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - i].Text = " ";
                                                }
                                            }


                                        }

                                    }

                                }
                                if (ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                                {
                                    for (row = 0; row < cbl_subtype.Items.Count; row++)
                                    {

                                        if (cbl_subtype.Items[row].Selected == true)
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                            DataView dv2 = ds.Tables[1].DefaultView;
                                            ds.Tables[2].DefaultView.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                            DataView dv1 = ds.Tables[2].DefaultView;
                                            DataView dvabs = new DataView();
                                            if (ds.Tables[3].Rows.Count > 0)
                                            {
                                                ds.Tables[3].DefaultView.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                                dvabs = ds.Tables[3].DefaultView;
                                            }


                                            if (dv2.Count > 0)
                                            {
                                                int getindex = Convert.ToInt32(indexcount[Convert.ToString(cbl_subtype.Items[row].Text)]);
                                                int incindex = getindex;
                                                int columncount = Convert.ToInt32(counthash[Convert.ToString(cbl_subtype.Items
[row].Text)]);

                                                FpSpread1.Sheets[0].RowCount = 3;
                                                FpSpread1.Sheets[0].Cells[0, 0].Text = "1";
                                                FpSpread1.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[0, 0].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].AddSpanCell(0, 0, 3, 1);

                                                FpSpread1.Sheets[0].Cells[0, 1].Text = romanLetter(index.ToString()) + " " + returnMonYear(Convert.ToString(dv2[0]["Exam_Month"])) + Convert.ToString(dv2[0]["Exam_Year"]);
                                                FpSpread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[0, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[0, 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].AddSpanCell(0, 1, 3, 1);

                                                FpSpread1.Sheets[0].Cells[0, 2].Text = "Appeared";
                                                FpSpread1.Sheets[0].Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[0, 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[0, 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[0, 2].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[1, 2].Text = "Passed";
                                                FpSpread1.Sheets[0].Cells[1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[1, 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[1, 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[1, 2].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[2, 2].Text = "%Pass";
                                                FpSpread1.Sheets[0].Cells[2, 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[2, 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[2, 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[2, 2].Font.Name = "Book Antiqua";


                                                double appearedAvg = 0;
                                                double passAvg = 0;
                                                double percentAvg = 0;

                                                for (int s = 0; s < columncount; s++)
                                                {
                                                    incindex++;
                                                    string subjectno = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, incindex - 1].Tag);
                                                    if (subjectno.Trim() != "")
                                                    {
                                                        DataTable dnew = dv2.ToTable();
                                                        DataView dv3 = new DataView(dnew);

                                                        DataTable dnew1 = dv1.ToTable();
                                                        DataView dv4 = new DataView(dnew1);

                                                        DataTable dnew2 = new DataTable();
                                                        if (dvabs.Count > 0)
                                                        {
                                                            dnew2 = dvabs.ToTable();
                                                        }
                                                        DataView dv5 = new DataView(dnew2);


                                                        dv3.RowFilter = "subject_no='" + subjectno + "'";

                                                        dv4.RowFilter = "subject_no='" + subjectno + "'";

                                                        double appeared = 0, passed = 0;
                                                        double percent = 0;
                                                        if (dv3.Count > 0)
                                                        {
                                                            string ap = Convert.ToString(dv3[0]["appear"]);
                                                            if (ap.Trim() != "")
                                                            {
                                                                appeared = Convert.ToInt32(ap);

                                                                if (dv5.Count > 0)
                                                                {
                                                                    dv5.RowFilter = "subject_no='" + subjectno + "'";
                                                                    if (dv5.Count > 0)
                                                                    {
                                                                        string abs = Convert.ToString(dv5[0]["fail"]);
                                                                        if (abs.Trim() != "")
                                                                        {
                                                                            appeared -= Convert.ToInt32(abs);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (dv4.Count > 0)
                                                            {
                                                                string pas = Convert.ToString(dv4[0]["fail"]);
                                                                if (pas.Trim() != "")
                                                                {
                                                                    passed = Convert.ToInt32(pas);
                                                                }
                                                            }
                                                            if (appeared != 0)
                                                            {
                                                                percent = ((appeared - passed) * 100) / appeared;
                                                                percent = Math.Round(percent, 2);
                                                            }

                                                            appearedAvg += appeared;
                                                            passAvg += appeared - passed;
                                                            percentAvg += percent;

                                                            FpSpread1.Sheets[0].Cells[0, incindex - 1].Text = Convert.ToString(appeared);
                                                            FpSpread1.Sheets[0].Cells[0, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[0, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[0, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[0, incindex - 1].Font.Name = "Book Antiqua";

                                                            FpSpread1.Sheets[0].Cells[1, incindex - 1].Text = Convert.ToString(appeared - passed);
                                                            FpSpread1.Sheets[0].Cells[1, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[1, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[1, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[1, incindex - 1].Font.Name = "Book Antiqua";

                                                            FpSpread1.Sheets[0].Cells[2, incindex - 1].Text = Convert.ToString(percent);
                                                            FpSpread1.Sheets[0].Cells[2, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[2, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[2, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[2, incindex - 1].Font.Name = "Book Antiqua";
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[0, incindex - 1].Text = Convert.ToString("-");
                                                            FpSpread1.Sheets[0].Cells[0, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[0, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[0, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[0, incindex - 1].Font.Name = "Book Antiqua";

                                                            FpSpread1.Sheets[0].Cells[1, incindex - 1].Text = Convert.ToString("-");
                                                            FpSpread1.Sheets[0].Cells[1, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[1, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[1, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[1, incindex - 1].Font.Name = "Book Antiqua";

                                                            FpSpread1.Sheets[0].Cells[2, incindex - 1].Text = Convert.ToString("-");
                                                            FpSpread1.Sheets[0].Cells[2, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[2, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[2, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[2, incindex - 1].Font.Name = "Book Antiqua";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FpSpread1.Sheets[0].Cells[0, incindex - 1].Text = Convert.ToString("-");
                                                        FpSpread1.Sheets[0].Cells[0, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[0, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                        FpSpread1.Sheets[0].Cells[0, incindex - 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[0, incindex - 1].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[1, incindex - 1].Text = Convert.ToString("-");
                                                        FpSpread1.Sheets[0].Cells[1, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[1, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                        FpSpread1.Sheets[0].Cells[1, incindex - 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[1, incindex - 1].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[2, incindex - 1].Text = Convert.ToString("-");
                                                        FpSpread1.Sheets[0].Cells[2, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[2, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                        FpSpread1.Sheets[0].Cells[2, incindex - 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[2, incindex - 1].Font.Name = "Book Antiqua";
                                                    }
                                                }

                                                appearedAvg /= dv2.Count;
                                                passAvg /= dv2.Count;
                                                percentAvg /= dv2.Count;

                                                FpSpread1.Sheets[0].Cells[0, getindex - 1].Text = Math.Round(appearedAvg, 2).ToString();
                                                FpSpread1.Sheets[0].Cells[0, getindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[0, getindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[0, getindex - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[0, getindex - 1].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[1, getindex - 1].Text = Math.Round(passAvg, 2).ToString();
                                                FpSpread1.Sheets[0].Cells[1, getindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[1, getindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[1, getindex - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[1, getindex - 1].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[2, getindex - 1].Text = Math.Round(percentAvg, 2).ToString();
                                                FpSpread1.Sheets[0].Cells[2, getindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[2, getindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[2, getindex - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[2, getindex - 1].Font.Name = "Book Antiqua";
                                            }
                                        }
                                    }

                                }

                            }
                            else
                            {

                                //Row end
                                bool newcheck = false;
                                for (row = 0; row < cbl_subtype.Items.Count; row++)
                                {
                                    if (cbl_subtype.Items[row].Selected == true)
                                    {

                                        ds.Tables[0].DefaultView.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                        DataView dv = ds.Tables[0].DefaultView;

                                        if (dv.Count > 0)
                                        {
                                            if (newcheck == false)
                                            {
                                                FpSpread1.Sheets[0].RowCount += 2;

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = "S.No";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
                                                // FpSpread1.Sheets[0].Columns[0].Width = 50;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, 0, 2, 1);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Text = "Semester";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].VerticalAlign = VerticalAlign.Middle;
                                                //   FpSpread1.Sheets[0].Columns[1].Width = 100;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].ForeColor = ColorTranslator.FromHtml("#ffffff");
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, 1, 2, 1);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Text = "Month & Year";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Center;
                                                //  FpSpread1.Sheets[0].Columns[2].Width = 150;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].ForeColor = ColorTranslator.FromHtml("#ffffff");
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, 2, 2, 1);
                                                newcheck = true;
                                            }




                                            check++;
                                            int columncount = Convert.ToInt32(indexcount[Convert.ToString(cbl_subtype.Items
[row].Text)]);
                                            if (cbl_subtype.Items[row].Text == "Practicals")
                                            {
                                                columncount = Convert.ToInt32(indexcount["Practical"]);
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount - 1].Text = "Avg " + cbl_subtype.Items[row].Text;

                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, columncount - 1, 2, 1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount - 1].VerticalAlign = VerticalAlign.Middle;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount - 1].ForeColor = ColorTranslator.FromHtml("#ffffff");

                                            int count_column = Convert.ToInt32(counthash[Convert.ToString(cbl_subtype.Items[row].Text)]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount].Text = cbl_subtype.Items[row].Text;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount].Font.Size = FontUnit.Medium;

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount].VerticalAlign = VerticalAlign.Middle;
                                            //   FpSpread1.Sheets[0].Columns[1].Width = 100;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, columncount].ForeColor = ColorTranslator.FromHtml("#ffffff");
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, columncount, 1, count_column);
                                            int new1 = 0;
                                            int n = columncount;
                                            for (i = 0; i < count_column; i++)
                                            {
                                                if (new1 < dv.Count)
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].Text = Convert.ToString(dv[new1]["subject_code"]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].Tag = Convert.ToString(dv[new1]["subject_no"]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].Font.Size = FontUnit.Medium;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].VerticalAlign = VerticalAlign.Middle;
                                                    //   FpSpread1.Sheets[0].Columns[1].Width = 100;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].ForeColor = ColorTranslator.FromHtml("#ffffff");
                                                    new1++;
                                                    n++;
                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].Text = " ";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].Font.Size = FontUnit.Medium;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].VerticalAlign = VerticalAlign.Middle;
                                                    //   FpSpread1.Sheets[0].Columns[1].Width = 100;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, n].ForeColor = ColorTranslator.FromHtml("#ffffff");
                                                }
                                            }


                                        }

                                    }

                                }
                                bool valcheck = false;
                                if (ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                                {

                                    for (row = 0; row < cbl_subtype.Items.Count; row++)
                                    {
                                        if (cbl_subtype.Items[row].Selected == true)
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                            DataView dv2 = ds.Tables[1].DefaultView;
                                            ds.Tables[2].DefaultView.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                            DataView dv1 = ds.Tables[2].DefaultView;

                                            DataView dvabs = new DataView();
                                            if (ds.Tables[3].Rows.Count > 0)
                                            {
                                                ds.Tables[3].DefaultView.RowFilter = "semester='" + index + "' and subject_type='" + cbl_subtype.Items[row].Text + "'";
                                                dvabs = ds.Tables[3].DefaultView;
                                            }
                                            if (dv2.Count > 0)
                                            {
                                                int getindex = Convert.ToInt32(indexcount[Convert.ToString(cbl_subtype.Items[row].Text)]);
                                                if (cbl_subtype.Items[row].Text == "Practicals")
                                                {
                                                    getindex = Convert.ToInt32(indexcount["Practical"]);
                                                }

                                                int incindex = getindex;
                                                int columncount = Convert.ToInt32(counthash[Convert.ToString(cbl_subtype.Items
[row].Text)]);

                                                if (valcheck == false)
                                                {
                                                    FpSpread1.Sheets[0].RowCount += 3;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Text = "1";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 3, 0, 3, 1);

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 1].Text = romanLetter(index.ToString()) + " " + returnMonYear(Convert.ToString(dv2[0]["Exam_Month"])) + Convert.ToString(dv2[0]["Exam_Year"]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 1].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 3, 1, 3, 1);

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 2].Text = "Appeared";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 2].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 2].Font.Name = "Book Antiqua";

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Text = "Passed";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Font.Name = "Book Antiqua";

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "%Pass";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                                    valcheck = true;
                                                }

                                                double appearedAvg = 0;
                                                double passAvg = 0;
                                                double percentAvg = 0;

                                                for (int s = 0; s < columncount; s++)
                                                {
                                                    incindex++;
                                                    string subjectno = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, incindex - 1].Tag);
                                                    if (subjectno.Trim() != "")
                                                    {
                                                        DataTable dnew = dv2.ToTable();
                                                        DataView dv3 = new DataView(dnew);

                                                        DataTable dnew1 = dv1.ToTable();
                                                        DataView dv4 = new DataView(dnew1);

                                                        DataTable dnew2 = new DataTable();
                                                        if (dvabs.Count > 0)
                                                        {
                                                            dnew2 = dvabs.ToTable();
                                                        }
                                                        DataView dv5 = new DataView(dnew2);


                                                        dv3.RowFilter = "subject_no='" + subjectno + "'";

                                                        dv4.RowFilter = "subject_no='" + subjectno + "'";

                                                        double appeared = 0, passed = 0;
                                                        double percent = 0;
                                                        if (dv3.Count > 0)
                                                        {
                                                            string ap = Convert.ToString(dv3[0]["appear"]);
                                                            if (ap.Trim() != "")
                                                            {
                                                                appeared = Convert.ToInt32(ap);
                                                                if (dv5.Count > 0)
                                                                {
                                                                    dv5.RowFilter = "subject_no='" + subjectno + "'";
                                                                    if (dv5.Count > 0)
                                                                    {
                                                                        string abs = Convert.ToString(dv5[0]["fail"]);
                                                                        if (abs.Trim() != "")
                                                                        {
                                                                            appeared -= Convert.ToInt32(abs);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (dv4.Count > 0)
                                                            {
                                                                string pas = Convert.ToString(dv4[0]["fail"]);
                                                                if (pas.Trim() != "")
                                                                {
                                                                    passed = Convert.ToInt32(pas);
                                                                }
                                                            }
                                                            if (appeared != 0)
                                                            {
                                                                percent = ((appeared - passed) * 100) / appeared;
                                                                percent = Math.Round(percent, 2);
                                                            }

                                                            appearedAvg += appeared;
                                                            passAvg += appeared - passed;
                                                            percentAvg += percent;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].Text = Convert.ToString(appeared);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].Font.Name = "Book Antiqua";

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].Text = Convert.ToString(appeared - passed);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].Font.Name = "Book Antiqua";

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].Text = Convert.ToString(percent);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].Font.Name = "Book Antiqua";
                                                        }
                                                        else
                                                        {

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].Text = Convert.ToString("-");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].Font.Name = "Book Antiqua";

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].Text = Convert.ToString("-");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].Font.Name = "Book Antiqua";

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].Text = Convert.ToString("-");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].Font.Name = "Book Antiqua";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].Text = Convert.ToString("-");
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, incindex - 1].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].Text = Convert.ToString("-");
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, incindex - 1].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].Text = Convert.ToString("-");
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, incindex - 1].Font.Name = "Book Antiqua";
                                                    }
                                                }

                                                appearedAvg /= dv2.Count;
                                                passAvg /= dv2.Count;
                                                percentAvg /= dv2.Count;

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, getindex - 1].Text = Math.Round(appearedAvg, 2).ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, getindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, getindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, getindex - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, getindex - 1].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, getindex - 1].Text = Math.Round(passAvg, 2).ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, getindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, getindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, getindex - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, getindex - 1].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, getindex - 1].Text = Math.Round(percentAvg, 2).ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, getindex - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, getindex - 1].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, getindex - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, getindex - 1].Font.Name = "Book Antiqua";
                                            }
                                        }
                                    }

                                }


                            }

                        }

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
        catch { }
    }
    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            switch (numeral)
            {
                case "1":
                    romanLettervalue = "Semester-I";
                    break;
                case "2":
                    romanLettervalue = "Semester-II";
                    break;
                case "3":
                    romanLettervalue = "Semester-III";
                    break;
                case "4":
                    romanLettervalue = "Semester-IV";
                    break;
                case "5":
                    romanLettervalue = "Semester-V";
                    break;
                case "6":
                    romanLettervalue = "Semester-VI";
                    break;
                case "7":
                    romanLettervalue = "Semester-VII";
                    break;
                case "8":
                    romanLettervalue = "Semester-VIII";
                    break;
                case "9":
                    romanLettervalue = "Semester-IX";
                    break;
                case "10":
                    romanLettervalue = "Semester-X";
                    break;

            }
        }
        return romanLettervalue;
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
                ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
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
            ddl_dept.Items.Clear();
            degree = "";
            degree = Convert.ToString(ddl_degree.SelectedValue.ToString());

            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_dept.DataSource = ds;
                    ddl_dept.DataTextField = "dept_name";
                    ddl_dept.DataValueField = "degree_code";
                    ddl_dept.DataBind();
                }
            }
        }
        catch { }
    }
    public void bindSubtype()
    {
        try
        {
            txt_subtype.Text = "---Select---";
            cbl_subtype.Items.Clear();
            cb_subtype.Checked = false;
            subtype = "";

            batch = "";
            if (ddl_batch.Items.Count > 0)
            {
                batch = Convert.ToString(ddl_batch.SelectedValue.ToString());
            }

            dept = "";
            if (ddl_dept.Items.Count > 0)
            {
                dept = Convert.ToString(ddl_dept.SelectedValue.ToString());
            }

            if (batch != "" && dept != "")
            {
                //selectQuery = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and r.current_semester=s.semester and r.degree_code in ( " + dept + ") and r.batch_year in ('" + batch + "')";
                selectQuery = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar'  and r.degree_code in ( " + dept + ") and r.batch_year in ('" + batch + "')";
                 
                ds.Clear();;
                ds = d2.select_method_wo_parameter(selectQuery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (subtype == "")
                        {
                            subtype = Convert.ToString(ds.Tables[0].Rows[i]["syll_code"]);
                        }
                        else
                        {
                            subtype = subtype + "'" + "," + "'" + Convert.ToString(ds.Tables[0].Rows[i]["syll_code"]);
                        }
                    }

                    DataSet ds2 = new DataSet();
                    //string selectQuery1 = "select distinct subject_type from sub_sem where syll_code in ('" + subtype + "')";
                    string selectQuery1 = "select distinct subject_type from sub_sem where syll_code in ('" + subtype + "')";
                    ds2.Clear();
                    ds2 = d2.select_method_wo_parameter(selectQuery1, "Text");

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        cbl_subtype.DataSource = ds2;
                        cbl_subtype.DataTextField = "subject_type";
                        cbl_subtype.DataValueField = "subject_type";
                        cbl_subtype.DataBind();

                        if (cbl_subtype.Items.Count > 0)
                        {
                            for (i = 0; i < cbl_subtype.Items.Count; i++)
                            {
                                cbl_subtype.Items[i].Selected = true;
                              
                            }
                            txt_subtype.Text = "Subject Type(" + cbl_subtype.Items.Count + ")";
                            cb_subtype.Checked = true;
                        }
                    }

                }
            }
        }
        catch { }
    }
    public void bindSubname()
    {
        try
        {
            //txt_subname.Text = "---Select---";
            //cbl_subname.Items.Clear();
            //cb_subname.Checked = false;
            //subname = "";

            //batch = "";
            //if (ddl_batch.Items.Count > 0)
            //{
            //    batch = Convert.ToString(ddl_batch.SelectedValue.ToString());
            //}

            //dept = "";
            //if (ddl_dept.Items.Count > 0)
            //{
            //    dept = Convert.ToString(ddl_dept.SelectedValue.ToString());
            //}


            //subtype = "";
            //if (cbl_subtype.Items.Count > 0)
            //{
            //    for (i = 0; i < cbl_subtype.Items.Count; i++)
            //    {
            //        if (cbl_subtype.Items[i].Selected == true)
            //        {
            //            if (subtype == "")
            //            {
            //                subtype = Convert.ToString(cbl_subtype.Items[i].Value);
            //            }
            //            else
            //            {
            //                subtype = subtype + "," + Convert.ToString(cbl_subtype.Items[i].Value);
            //            }
            //        }
            //    }
            //}

            //if (batch != "" && dept != "" && subtype != "")
            //{

            //    selectQuery = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and r.current_semester=s.semester and r.degree_code in ( " + dept + ") and r.batch_year in ('" + batch + "')";

            //    ds.Clear();
            //    ds = d2.select_method_wo_parameter(selectQuery, "Text");
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {

            //        subname = "";
            //        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            //        {
            //            if (subname == "")
            //            {
            //                subname = Convert.ToString(ds.Tables[0].Rows[i]["syll_code"]);
            //            }
            //            else
            //            {
            //                subname = subname + "'" + "," + "'" + Convert.ToString(ds.Tables[0].Rows[i]["syll_code"]);
            //            }
            //        }

            //        DataSet ds2 = new DataSet();
            //        string selectQuery1 = "select distinct subject_name from subject,subjectchooser,registration,sub_sem where sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no and registration.degree_code in (" + dept + ") and registration.batch_year in ('" + batch + "') and subject.syll_code in ('" + subname + "') and sub_sem.subType_no in (" + subtype + ") order by subject_name";
            //        ds2.Clear();
            //        ds2 = d2.select_method_wo_parameter(selectQuery1, "Text");

            //        if (ds2.Tables[0].Rows.Count > 0)
            //        {
            //            cbl_subname.DataSource = ds2;
            //            cbl_subname.DataTextField = "subject_name";
            //            cbl_subname.DataValueField = "subject_name";
            //            cbl_subname.DataBind();

            //            if (cbl_subname.Items.Count > 0)
            //            {
            //                for (i = 0; i < cbl_subname.Items.Count; i++)
            //                {
            //                    cbl_subname.Items[i].Selected = true;
            //                }
            //                txt_subname.Text = "Subject Name(" + cbl_subname.Items.Count + ")";
            //                cb_subname.Checked = true;
            //            }
            //        }

            //    }

            //}
        }
        catch { }
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
        bindSubtype();
        bindSubname();
    }
    protected void ddl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddept();
        bindSubtype();
        bindSubname();
    }
    protected void ddl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindSubtype();
        bindSubname();
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Result Analysis";
            try
            {
                degreedetails = "Result Analysis for " + ddl_batch.SelectedItem.Text + " (" + ddl_degree.SelectedItem.Text + ") - " + ddl_dept.SelectedItem.Text;
            }
            catch
            {

            }
            string pagename = "ResultAnalysis.aspx";
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