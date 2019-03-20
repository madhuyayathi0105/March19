using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Windows.Forms;
using System.Drawing;
using System.Configuration;

public partial class Subjectprioritysettings : System.Web.UI.Page
{
    static string[] ss;
    static string p = "";
    static string[] ss1;
    string ss2 = "";
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", grouporusercode = "";
    Boolean flag_true = false;
    ArrayList alv = new ArrayList();
    Hashtable hashmark = new Hashtable();
    ArrayList rights = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
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
            collegecode = Session["collegecode"].ToString();

            if (!IsPostBack)
            {

                ss2 = "";
                p = "";
                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Trebuchet MS";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.White;
                style2.BackColor = Color.Teal;

                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

                style2 = new FarPoint.Web.Spread.StyleInfo();


                style2.VerticalAlign = VerticalAlign.Middle;

                FpSpread2.Sheets[0].DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);


                // ddlsubtype.Items.Add(new ListItem(""));


                bindbatch();
                binddegree();
                bindbranch();
                bindsem();

                clear();




                clear();

            }
        }
        catch(Exception ex)
        {
        }
    }
    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = da.BindBatch();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void binddegree()
    {
        try
        {
            ddldegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindsem()
    {
        try
        {

            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"].ToString() + "";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                        //ddlSemYr.Enabled = false;
                    }
                    else if (first_year == true && i == 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }

                }
            }
            else
            {


                sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"].ToString() + "";

                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlnew, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }
                    }
                }


            }
            bindsubjtype();
        }
        catch (Exception ex)
        {

        }
    }
    public void bindsubjtype()
    {
        try
        {

            ddlsubtype.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            //string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"].ToString() + "";
            string sqlnew = "select subject_type,subtype_no from syllabus_master sm,sub_sem s where sm.syll_code=s.syll_code  and batch_year='" + ddlbatch.Text.ToString() + "' and semester='" + ddlsem.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubtype.DataSource = ds;
                ddlsubtype.DataTextField = "subject_type";
                ddlsubtype.DataValueField = "subtype_no";
                ddlsubtype.DataBind();
            }


        }
        catch (Exception ex)
        {

        }
    }
    public void bindbranch()
    {
        try
        {
            has.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_branch", has, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
            bindsem();

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }


    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    public void clear()
    {
        FpSpread2.Visible = false; btnreset.Visible = false;
        btnsave.Visible = false;

    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddlsubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            bindbranch();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsubjtype();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void Buttongo_Click(object sender, EventArgs e)
    {
        try
        {
            ss2 = "";
            p = "";
            FpSpread2.Visible = false; btnreset.Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 5;
            FpSpread2.Sheets[0].Columns[0].Width = 40;
            FpSpread2.Sheets[0].Columns[1].Width = 113;
            FpSpread2.Sheets[0].Columns[2].Width = 342;
            FpSpread2.Sheets[0].Columns[3].Width = 83;
            FpSpread2.Sheets[0].Columns[4].Width = 90;

            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].Columns[2].Locked = true;
            FpSpread2.Sheets[0].Columns[4].Locked = true;

            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].SheetCorner.RowCount = 2;

            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Code";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread2.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = chkcell1;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].FrozenRowCount = 1;
            chkcell1.AutoPostBack = true;
            chkcell.AutoPostBack = true;
            FpSpread2.Sheets[0].AutoPostBack = false;


            string batchyear = ddlbatch.SelectedValue.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string year = ddlbatch.SelectedValue.ToString();
            string degree = ddldegree.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string depart_code = ddlbranch.SelectedValue.ToString();
            string batchyearatt = ddlbatch.SelectedValue.ToString();
            string studinfo = "";
            //studinfo = "select distinct s.subject_no,subject_code,subject_name,subject_type,ss.subtype_no,subjectpriority from subject s,syllabus_master sm ,sub_sem ss where ss.syll_code=sm.syll_code and sm.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subtype_no=s.subtype_no and batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "' and semester='" + ddlsem.SelectedItem.Text.ToString() + "' and degree_code='" + depart_code + "' and ss.subtype_no='" + ddlsubtype.SelectedItem.Value.ToString() + "'";
            studinfo = "select distinct s.subject_no,subject_code,subject_name,subject_type,ss.subtype_no,subjectpriority from subject s,syllabus_master sm ,sub_sem ss where ss.syll_code=sm.syll_code and sm.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subtype_no=s.subtype_no and batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "' and semester='" + ddlsem.SelectedItem.Text.ToString() + "' and degree_code='" + depart_code + "' order by  subject_code ";

            DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
            if (dsstudinfo.Tables[0].Rows.Count > 0)
            {
                btnsave.Visible = true;

                int sno = 0;
                for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
                {
                    string regno = "";
                    string studname = "";
                    string rollno = "";
                    FpSpread2.Visible = true;
                    sno++;


                    FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno + "";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = dsstudinfo.Tables[0].Rows[studcount]["subject_code"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = dsstudinfo.Tables[0].Rows[studcount]["subject_no"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsstudinfo.Tables[0].Rows[studcount]["subject_name"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsstudinfo.Tables[0].Rows[studcount]["subject_name"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = dsstudinfo.Tables[0].Rows[studcount]["subjectpriority"].ToString(); ;
                    if (FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text.Trim() != "")
                    {
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Value = 1;
                        btnreset.Visible = true;
                    }
                    else
                    {
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = false;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Value = 0;
                        btnreset.Visible = false;
                    }
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = chkcell;


                }
            }
            else
            {
                clear();
                lblerror.Text = "No Records Found";
                lblerror.Visible = true;
            }
            string totalrows = FpSpread2.Sheets[0].RowCount.ToString();
            FpSpread2.Sheets[0].PageSize = (Convert.ToInt32(totalrows) * 20) + 40;
            FpSpread2.Height = (Convert.ToInt32(totalrows) * 50) + 40;
            FpSpread2.SaveChanges();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }




    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            int a = 0;
            FpSpread2.SaveChanges();
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 3].Value);
                if (isval == 1)
                {
                    string sql = " update subject set subjectpriority='" + FpSpread2.Sheets[0].Cells[res, 4].Text.ToString() + "' where subject_no='" + FpSpread2.Sheets[0].Cells[res, 1].Note.ToString() + "'";
                    a = da.update_method_wo_parameter(sql, "Text");
                }
            }
            if (a == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved successfully')", true);
                btnreset.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void btnresetclick(object sender, EventArgs e)
    {
        try
        {
            ss2 = "";
            p = "";
            int a = 0; 
            //added by Mullai
            string resetqry = "update subject set subjectpriority='' where subject_no in(select subject_no from subject s,syllabus_master sm ,sub_sem ss where ss.syll_code=sm.syll_code and sm.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subtype_no=s.subtype_no and batch_year='"+Convert.ToString(ddlbatch.SelectedItem.Text)+"' and semester='"+ddlsem.SelectedItem.Text.ToString()+"' and degree_code='"+ddlbranch.SelectedValue.ToString()+"')";          
            int updateqry = d2.update_method_wo_parameter(resetqry, "text");
            //**
            FpSpread2.SaveChanges();
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 3].Value);
                if (isval == 1)
                {
                    FpSpread2.Sheets[0].Cells[res, 4].Text = "";
                    FpSpread2.Sheets[0].Cells[res, 3].Value = 0;
                    FpSpread2.Sheets[0].Cells[res, 3].Locked = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow1;
            actrow1 = e.SheetView.ActiveRow.ToString();


            if (flag_true == false && actrow1 == "0")
            {
                for (int j = 1; j < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); j++)
                {
                    string actcol1 = e.SheetView.ActiveColumn.ToString();
                    string seltext = e.EditValues[Convert.ToInt16(actcol1)].ToString();
                    if (seltext != "System.Object")
                        FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol1)].Text = seltext.ToString();
                }
                flag_true = true;
            }
            else if (actrow1 != "0")
            {


                string number = "True";

                int actcol = Convert.ToInt16(e.SheetView.ActiveColumn.ToString());
                int actrow = Convert.ToInt16(e.SheetView.ActiveRow.ToString());


                string st1;
                string st;
                st = FpSpread2.GetEditValue(actrow, actcol).ToString();
                //  string sssshhs = sprdHallMaster.GetEditValue(1, 7).ToString();
                st1 = e.EditValues[actcol].ToString();
                if (st == number)
                {


                    if (p == "")
                    {
                        p = actrow.ToString();
                    }
                    else
                    {
                        p = p + "-" + actrow.ToString();
                    }
                    ss = p.Split(new char[] { '-' });
                    int cnt12 = 0;
                    for (int i = 0; i < ss.Length; i++)
                    {
                        if (ss[i] != "")
                        {
                            cnt12 = cnt12 + 1;
                            FpSpread2.Sheets[0].Cells[Convert.ToInt16(ss[i]), 4].Text = cnt12.ToString();

                        }
                    }

                }
                else
                {

                    for (int j = 0; j < ss.Length; j++)
                    {
                        int n;
                        if (ss[j] == "")
                        {
                            n = 0;
                        }
                        else
                        {
                            n = Convert.ToInt16(ss[j]);

                        }

                        if (n == actrow)
                        {
                            FpSpread2.Sheets[0].Cells[n, 4].Text = "";
                            ss[j] = "";

                        }
                        else
                        {


                            if (ss2 == "")
                            {
                                ss2 = ss[j].ToString();
                            }
                            else
                            {
                                ss2 = ss2 + "-" + ss[j].ToString();
                            }
                        }
                    }
                    int ccnt = 0;
                    ss1 = ss2.Split(new char[] { '-' });
                    for (int s = 0; s < ss1.Length; s++)
                    {
                        if (ss1[s] != "")
                        {
                            ccnt = ccnt + 1;
                            FpSpread2.Sheets[0].Cells[Convert.ToInt16(ss1[s]), 4].Text = ccnt.ToString();

                        }
                    }

                    p = ss2;
                }

            }
        }
        catch (Exception ex)
        {

        }

    }


}