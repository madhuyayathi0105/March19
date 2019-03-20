using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Configuration;

public partial class Grademastersettings : System.Web.UI.Page
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
                style2.HorizontalAlign = HorizontalAlign.Center;

                FpSpread2.Sheets[0].DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);


                // ddlsubtype.Items.Add(new ListItem(""));


                bindbatch();
                binddegree();
                bindbranch();
                //bindsem();

                clear();




                clear();

            }
        }
        catch (Exception ex)
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
            //bindsem();

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
        FpSpread2.Visible = false;//btnreset.Visible = false;
        btnsave.Visible = false;
        showdata.Visible = false;
        Button1.Visible = false;

    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
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
            //bindsem();
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

    protected void Buttongo_Click(object sender, EventArgs e)
    {
        try
        {
            ss2 = "";
            p = "";
            FpSpread2.Visible = false;//btnreset.Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 4;
            FpSpread2.Sheets[0].Columns[0].Width = 100;
            FpSpread2.Sheets[0].Columns[1].Width = 100;
            FpSpread2.Sheets[0].Columns[2].Width = 100;
            FpSpread2.Sheets[0].Columns[3].Width = 100;
            // FpSpread2.Sheets[0].Columns[4].Width = 90;

            //FpSpread2.Sheets[0].Columns[0].Locked = true;
            //FpSpread2.Sheets[0].Columns[1].Locked = true;
            //FpSpread2.Sheets[0].Columns[2].Locked = true;
            //FpSpread2.Sheets[0].Columns[4].Locked = true;



            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].SheetCorner.RowCount = 1;



            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "From Point";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "To Point";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Credit Points";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Grade";
            // FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";

            FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
            intgrcell.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            intgrcell.MaximumValue = Convert.ToInt32(100);
            intgrcell.MinimumValue = 0;
            intgrcell.ErrorMessage = "Enter valid Mark";

            FarPoint.Web.Spread.DoubleCellType intgrcell_cp = new FarPoint.Web.Spread.DoubleCellType();
            intgrcell_cp.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            intgrcell_cp.MaximumValue = Convert.ToInt32(10);
            intgrcell_cp.MinimumValue = 0;
            intgrcell_cp.ErrorMessage = "Enter valid Credit Points";

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            FpSpread2.Sheets[0].Columns[0].CellType = intgrcell;
            FpSpread2.Sheets[0].Columns[1].CellType = intgrcell;
            FpSpread2.Sheets[0].Columns[2].CellType = intgrcell_cp;
            FpSpread2.Sheets[0].Columns[3].CellType = txt;






            FpSpread2.Sheets[0].AutoPostBack = false;


            string batchyear = ddlbatch.SelectedValue.ToString();

            string degree = ddldegree.SelectedItem.Value.ToString();

            string depart_code = ddlbranch.SelectedValue.ToString();

            string studinfo = "";

            studinfo = "select * from Grade_Master where   College_Code='" + Session["collegecode"].ToString() + "' and  batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "'  and degree_code='" + depart_code + "'";

            DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
            if (dsstudinfo.Tables[0].Rows.Count > 0)
            {
                btnsave.Visible = true;

                int sno = 0;
                for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
                {

                    FpSpread2.Visible = true;
                    sno++;

                    FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = dsstudinfo.Tables[0].Rows[studcount]["Frange"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = dsstudinfo.Tables[0].Rows[studcount]["Trange"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsstudinfo.Tables[0].Rows[studcount]["Credit_Points"].ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = dsstudinfo.Tables[0].Rows[studcount]["Mark_Grade"].ToString(); ;


                }
            }
            else
            {
                //clear();
                //lblerror.Text = "No Records Found";
                //lblerror.Visible = true;
                FpSpread2.Visible = true;
                btnsave.Visible = true;
                Button1.Visible = true;
                showdata.Visible = true;
            }
            string totalrows = FpSpread2.Sheets[0].RowCount.ToString();
            FpSpread2.Sheets[0].PageSize = (Convert.ToInt32(totalrows) * 20) + 40;
            FpSpread2.Height = 350;
            FpSpread2.SaveChanges();
            FpSpread2.Visible = true;
            btnsave.Visible = true;
            Button1.Visible = true;
            showdata.Visible = true;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }


    protected void Button1_Click1(object sender, EventArgs e)
    {
        FpSpread2.SaveChanges();
        int rcount = FpSpread2.Sheets[0].RowCount++;
        FpSpread2.Sheets[0].ColumnCount = 4;


        FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread2.Sheets[0].DefaultStyle.Font.Bold = false;
        FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.White;

        FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

        FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
        intgrcell.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
        intgrcell.MaximumValue = Convert.ToInt32(100);
        intgrcell.MinimumValue = 0;
        intgrcell.ErrorMessage = "Enter valid Number";
        FpSpread2.Sheets[0].Columns[0].CellType = intgrcell;
        FpSpread2.Sheets[0].Columns[1].CellType = intgrcell;
        FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        int minustwo = FpSpread2.Sheets[0].RowCount - 2;

        if (rcount != 0)
        {
            if (FpSpread2.Sheets[0].Cells[minustwo, 1].Text.ToString() != "" && FpSpread2.Sheets[0].Cells[minustwo, 0].Text.ToString() != "" && FpSpread2.Sheets[0].Cells[minustwo, 2].Text.ToString() != "")
            {
                double temp = Convert.ToDouble(FpSpread2.Sheets[0].Cells[minustwo, 1].Text.ToString());
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].CellType = intgrcell;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(temp);
                //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                FpSpread2.Sheets[0].RowCount--;

            }

        }
        else
        {

            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].CellType = intgrcell;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "00.00";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = intgrcell;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = "00.00";
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            int a = 0;
            FpSpread2.SaveChanges();
            string sql = " delete from Grade_Master where Degree_Code='" + ddlbranch.SelectedItem.Value.ToString() + "' and batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "' and College_Code='" + Session["collegecode"].ToString() + "'";
            a = da.update_method_wo_parameter(sql, "Text");
            string Mark_Grade = "", Frange = "", Trange = "", Credit_Points = "", batch_year = "", Degree_Code = "", College_Code = "";
            for (int res = 0; res < FpSpread2.Sheets[0].RowCount; res++)
            {
                if (FpSpread2.Sheets[0].Cells[res, 0].Text.ToString() != "" && FpSpread2.Sheets[0].Cells[res, 1].Text.ToString() != "" && FpSpread2.Sheets[0].Cells[res, 2].Text.ToString() != "" && FpSpread2.Sheets[0].Cells[res, 3].Text.ToString() != "")
                {

                    Frange = FpSpread2.Sheets[0].Cells[res, 0].Text.ToString();
                    Trange = FpSpread2.Sheets[0].Cells[res, 1].Text.ToString();
                    Credit_Points = FpSpread2.Sheets[0].Cells[res, 2].Text.ToString();
                    Mark_Grade = FpSpread2.Sheets[0].Cells[res, 3].Text.ToString();

                    batch_year = ddlbatch.SelectedItem.Text.ToString();
                    Degree_Code = ddlbranch.SelectedItem.Value.ToString();
                    College_Code = Session["collegecode"].ToString();
                    sql = " insert into Grade_Master (Mark_Grade,Frange,Trange,Credit_Points,batch_year,Degree_Code,College_Code) values ('" + Mark_Grade + "','" + Frange + "','" + Trange + "','" + Credit_Points + "','" + batch_year + "','" + Degree_Code + "','" + College_Code + "')";
                    a = da.update_method_wo_parameter(sql, "Text");
                }
            }

            if (a == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved successfully')", true);
            }


        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Grade Master";
            string pagename = "GradeMaster.aspx";
            Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
            Printcontrol.Visible = true;

        }
        catch
        {

        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            d2.printexcelreport(FpSpread2, reportname);
        }
        catch
        {

        }
    }


    protected void btn_importex(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;

            Boolean rollflag = false;
            Boolean stro = false;
            string errorroll = "";
            int getstuco = 0;
            FpSpread2.SaveChanges();
            if (fpmarkexcel.FileName != "" && fpmarkexcel.FileName != null)
            {
                if (fpmarkexcel.FileName.EndsWith(".xls") || fpmarkexcel.FileName.EndsWith(".xlsx"))
                {
                    using (Stream stream = this.fpmarkexcel.FileContent as Stream)
                    {
                        stream.Position = 0;
                        this.fpmarkimport.OpenExcel(stream);
                        fpmarkimport.OpenExcel(stream);
                        fpmarkimport.SaveChanges();
                    }
                    FpSpread2.Sheets[0].Rows.Count = fpmarkimport.Sheets[0].RowCount - 1;
                    for (int c = 0; c < fpmarkimport.Sheets[0].ColumnCount; c++)
                    {
                        string gettest = fpmarkimport.Sheets[0].Cells[0, c].Text.ToString().Trim().ToLower();
                        for (int g = 0; g < FpSpread2.Sheets[0].ColumnCount; g++)
                        {
                            string settest = FpSpread2.Sheets[0].ColumnHeader.Cells[0, g].Text.ToString().Trim().ToLower();
                            if (settest == gettest)
                            {
                                for (int i = 1; i < fpmarkimport.Sheets[0].RowCount; i++)
                                {

                                    string markval = fpmarkimport.Sheets[0].Cells[i, g].Text.ToString().Trim().ToLower();
                                    FpSpread2.Sheets[0].Cells[i - 1, g].Text = markval.ToUpper();


                                }
                                stro = true;
                            }
                        }
                    }
                    if (stro == true)
                    {
                        if (errorroll == "")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Imported Successfully')", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Test Not Exists')", true);
                    }
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select The File and Then Proceed";
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The File and Then Proceed";
            }
            fpmarkimport.Visible = false;
            FpSpread2.SaveChanges();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void btnfpspread1delete_Click1(object sender, EventArgs e)
    {
        int a = 0;
        string sql = " delete from Grade_Master where Degree_Code='" + ddlbranch.SelectedItem.Value.ToString() + "' and batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "' and College_Code='" + Session["collegecode"].ToString() + "'";
        a = da.update_method_wo_parameter(sql, "Text");

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Deleted successfully')", true);
        FpSpread2.Sheets[0].RowCount = 0;
        FpSpread2.SaveChanges();
    }


}