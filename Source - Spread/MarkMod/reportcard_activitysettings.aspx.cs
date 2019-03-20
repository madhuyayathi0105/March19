using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;
using FarPoint.Web.Spread;
using System.Configuration;


public partial class reportcard_activitysettings : System.Web.UI.Page
{
    string grade_ids = "";
    string activity_ids = "";
    FpSpread fpspreadsample;
    DataSet ds = new DataSet();
    static Boolean forschoolsetting = false;
    DAccess2 dacc = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable ht = new Hashtable();
    string term = "";
    Boolean cellclick = false;
    static ArrayList arr = new ArrayList();
    string grouporusercode = "";
    string fpbatch_year = "";
    string fpdegreecode = "";
    string fpbranch = "";
    string fpsem = "";
    string fpsec = "";
    DataSet dsbindv = new DataSet();

    FarPoint.Web.Spread.ComboBoxCellType combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolactivity = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocoldesc = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();

    DAccess2 da = new DAccess2();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!Request.FilePath.Contains("CAMHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/MarkMod/CAMHome.aspx");
                return;
            }
        }
        if (!IsPostBack)
        {
            cbClass.Checked = false;
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            // loadcollege();
            // fullscreen.Visible = false;

            //DataSet schoolds = new DataSet();
            //  string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
            //  schoolds.Clear();
            //  schoolds.Dispose();
            //  schoolds = dacc.select_method_wo_parameter(sqlschool, "Text");
            //  if (schoolds.Tables[0].Rows.Count > 0)
            //  {
            //      string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]);
            //      if (schoolvalue.Trim() == "0")
            //      {
            forschoolsetting = true;
            lblBatch.Text = "Year";
            lblDegree.Text = "School Type";
            lblBranch.Text = "Standard";
            //lblSemYr.Text = "Term";
            //    }
            //}
            BindBatch();
            BindDegree();
            if (ddlDegree.Items.Count > 0)
            {
                bindbranch();
                bindsem();
                BindSectionDetail();
                lblErrorMsg.Text = "";
            }
            else
            {
                lblErrorMsg.Text = "Give degree rights to staff";
            }
            fp1.Sheets[0].RowHeader.Visible = false;
            fp1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fp1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fp1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fp1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            fp1.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.White;
            fp1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fp1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fp1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fp1.Sheets[0].DefaultStyle.Font.Bold = false;
            //fp1.Sheets[0].AutoPostBack = true;
            fp1.CommandBar.Visible = false;

            fp1.Sheets[0].RowCount = 0;
            fp1.Sheets[0].ColumnCount = 4;
            fp1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            fp1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Activity";
            fp1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Description";
            fp1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Grade";
            fp1.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fp1.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fp1.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fp1.Sheets[0].ColumnHeader.Columns[3].HorizontalAlign = HorizontalAlign.Center;
            fp1.Sheets[0].Columns[0].Width = 80;
            fp1.Sheets[0].Columns[0].Locked = true;
            fp1.Sheets[0].Columns[1].Width = 315;
            fp1.Sheets[0].Columns[2].Width = 260;
            fp1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fp1.Height = 288;
            fp1.Width = 398;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            darkstyle.ForeColor = System.Drawing.Color.White;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Border.BorderSize = 0;
            darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
            fp1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            for (int i = 0; i < 4; i++)
            {
                fp1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                fp1.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
                fp1.Sheets[0].ColumnHeader.Cells[0, i].Font.Size = FontUnit.Medium;
                fp1.Sheets[0].ColumnHeader.Cells[0, i].Font.Name = "Book Antiqua";
                fp1.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
                fp1.Sheets[0].ColumnHeader.Cells[0, i].ForeColor = Color.White;
            }
            fp1.Visible = true;

            fp1.Sheets[0].Columns[2].Visible = false;
            fp1.Sheets[0].Columns[3].Visible = false;

            //for (int i = 0; i < arr.Count; i++)
            //{


            //}
            btnGo_Click(sender, e);
            ddlactivity.Visible = false;
            hideaddactivity();
            hideactivitygrade();
            cbClass_CheckedChanged(sender, e);
        }
        term = Convert.ToString(ddlSemYr.SelectedItem.Text);
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void ddltolparts_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            fp1.Visible = false;
            btnrowadd.Visible = false;
            btnsaveparts.Visible = false;
            btnremove.Visible = false;
            string sqlsubtt = "select * from CoCurr_Activitie where PartName like '" + Convert.ToString(ddltolparts.SelectedItem.Text.Trim()) + "' and Batch_Year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(sqlsubtt, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubtt.DataSource = ds;
                ddlsubtt.DataTextField = "SubTitle";
                ddlsubtt.DataValueField = "CoCurr_ID";
                ddlsubtt.DataBind();
            }
            ddlformate_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnrowadd_Click(Object sender, EventArgs e)
    {
        try
        {
            string currentsem = Convert.ToString(ddlSemYr.SelectedItem.Text);
            string degreecode = Convert.ToString(ddlBranch.SelectedItem.Value);
            string batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);

            string queryactivity = " select * from textvaltable where TextCriteria='RActv' and college_code='" + Convert.ToString(Session["collegecode"]) + "' order by TextVal";

            DataSet newact = new DataSet();
            newact.Clear();
            newact = da.select_method_wo_parameter(queryactivity, "Text");

            if (newact.Tables[0].Rows.Count > 0)
            {
                combocolactivity.DataSource = newact;
                combocolactivity.DataTextField = "TextVal";
                combocolactivity.DataValueField = "TextCode";
            }
            combocolactivity.ShowButton = false;
            combocolactivity.AutoPostBack = true;
            combocolactivity.UseValue = true;

            fp1.Sheets[0].RowCount++;
            fp1.Sheets[0].Cells[fp1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(fp1.Sheets[0].RowCount);
            fp1.Sheets[0].Cells[fp1.Sheets[0].RowCount - 1, 1].CellType = combocolactivity;
            fp1.SaveChanges();
            fp1.Sheets[0].PageSize = fp1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnsaveparts_Click(Object sender, EventArgs e)
    {
        try
        {
            fp1.SaveChanges();

            int desccount = 0;

            string Roll_No = "";
            int a = 0;
            string Activity = "";
            string Act_Desc = "";
            string Grade = "";
            string Degree_Code = "";
            string Batch_Year = "";
            string CoCurr_ID = "";
            fp1.SaveChanges();

            ArrayList acitivitydumb = new ArrayList();
            Degree_Code = Convert.ToString(ddlBranch.SelectedItem.Value);
            Batch_Year = Convert.ToString(ddlBatch.SelectedItem.Text);
            CoCurr_ID = Convert.ToString(ddlsubtt.SelectedItem.Value);
            acitivitydumb.Clear();
            if (fp1.Sheets[0].RowCount > 0)
            {
                string savesql = " delete from activity_entry where Batch_Year='" + Batch_Year + "' and term='" + term + "' and Degree_Code='" + Degree_Code + "' and CoCurr_ID='" + CoCurr_ID + "'";

                a = da.update_method_wo_parameter(savesql, "Text");
                for (int i = 0; i < fp1.Sheets[0].RowCount; i++)
                {
                    if (fp1.Sheets[0].Columns[1].Visible == true)
                    {
                        //Activity = Convert.ToString(fp1.Sheets[0].Cells[i, 1].Text);
                        // Activity = combocolactivity.Items.GetValue; 
                        //string value =Convert.ToString( combocolactivity.Items.GetValue(i, 1));
                        Activity = Convert.ToString(fp1.Sheets[0].GetValue(i, 1));

                        if (Activity.Trim() == "")
                        {
                            Activity = "0";
                        }
                    }

                    if (!acitivitydumb.Contains(Activity))
                    {
                        savesql = " insert into activity_entry values ('" + CoCurr_ID + "','" + Activity + "','" + Degree_Code + "','" + Batch_Year + "','" + term + "')";

                        a = da.update_method_wo_parameter(savesql, "Text");
                        acitivitydumb.Add(Activity);
                    }
                }
                if (a > 0)
                {
                    btngofp_Click(sender, e);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                }
                else
                {
                    btngofp_Click(sender, e);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnremove_Click(Object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(ddlsubtt.SelectedItem.Value) != "")
            {
                string Degree_Code = "";
                string Batch_Year = "";
                string CoCurr_ID = "";
                string Roll_No = "";

                Degree_Code = Convert.ToString(ddlBranch.SelectedItem.Value);
                Batch_Year = Convert.ToString(ddlBatch.SelectedItem.Text);
                CoCurr_ID = Convert.ToString(ddlsubtt.SelectedItem.Value);
                string sqlselect = "delete from activity_entry  where  CoCurr_ID='" + CoCurr_ID + "' and Degree_Code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' and term='" + term + "'";
                int a = dacc.update_method_wo_parameter(sqlselect, "Text");
                if (a > 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Removed Successfully')", true);
                    fp1.Sheets[0].RowCount = 0;
                    fp1.SaveChanges();
                }
            }
        }
        catch
        {
        }
    }

    protected void btngofp_Click(Object sender, EventArgs e)
    {
        try
        {
            cbClass.Checked = false;
            lblparterr.Visible = false;
            lblparterr.Text = "";
            if (ddlformate.SelectedIndex == 0)
            {
                hideactivitygrade();
                int cccount = 0;
                string partname = Convert.ToString(ddltolparts.SelectedItem.Text).Trim();
                string ttsubttitle = Convert.ToString(ddlsubtt.SelectedItem.Text).Trim();
                string fopsql = "select * from CoCurr_Activitie where CoCurr_ID='" + Convert.ToString(ddlsubtt.SelectedItem.Value) + "' and  Batch_Year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "' ";
                string IsDirectEntry = "";
                string IsActivity = "";
                string IsActDesc = "";
                string IsGrade = "";
                fp1.Sheets[0].RowCount = 0;
                DataSet dsfpful = new DataSet();
                dsfpful.Clear();
                dsfpful = da.select_method_wo_parameter(fopsql, "Text");
                if (dsfpful.Tables[0].Rows.Count > 0)
                {
                    IsDirectEntry = Convert.ToString(dsfpful.Tables[0].Rows[0]["IsDirectEntry"]);
                    IsActivity = "True";
                    IsActDesc = "False";
                    IsGrade = "False";

                }
                if (IsDirectEntry.Trim() == "True")
                {
                    lblparterr.Text = "Sorry it is Direct Entry";
                    lblparterr.Visible = true;
                    fp1.Visible = false;
                    btnrowadd.Visible = false;
                    btnsaveparts.Visible = false;
                    btnremove.Visible = false;
                    return;
                }
                else
                {
                    fp1.Visible = true;
                    lblparterr.Text = "";
                    lblparterr.Visible = false;
                    //for (int i = 0; i < dsfpful.Tables[0].Rows.Count; i++)
                    //{
                    if (IsActivity.Trim() == "True")
                    {
                        fp1.Sheets[0].Columns[1].Visible = true;
                        cccount++;
                    }
                    else
                    {
                        fp1.Sheets[0].Columns[1].Visible = false;
                    }

                    if (IsActDesc.Trim() == "True")
                    {
                        fp1.Sheets[0].Columns[2].Visible = true;
                        cccount++;
                    }
                    else
                    {
                        fp1.Sheets[0].Columns[2].Visible = false;
                    }

                    if (IsGrade.Trim() == "True")
                    {
                        fp1.Sheets[0].Columns[3].Visible = true;
                        cccount++;
                    }
                    else
                    {
                        fp1.Sheets[0].Columns[3].Visible = false;
                    }
                    //}

                }
                if (cccount == 0)
                {
                    lblparterr.Text = "Please Give Rights";
                    lblparterr.Visible = true;
                    fp1.Visible = false;
                    btnrowadd.Visible = false;
                    btnsaveparts.Visible = false;
                    btnremove.Visible = false;
                    return;
                }
                string Roll_No = "";


                string Degree_Code = "";
                string Batch_Year = "";
                string CoCurr_ID = "";

                Degree_Code = Convert.ToString(ddlBranch.SelectedItem.Value);
                Batch_Year = Convert.ToString(ddlBatch.SelectedItem.Text);
                CoCurr_ID = Convert.ToString(ddlsubtt.SelectedItem.Value);
                string sqlselect = "select * from  activity_entry where CoCurr_ID='" + CoCurr_ID + "' and Degree_Code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' and term='" + term + "'";
                DataSet dsselect = new DataSet();
                dsselect.Clear();
                dsselect = da.select_method_wo_parameter(sqlselect, "Text");
                if (dsselect.Tables[0].Rows.Count > 0)
                {
                    fp1.Sheets[0].RowCount = dsselect.Tables[0].Rows.Count;
                    string currentsem = Convert.ToString(ddlSemYr.SelectedItem.Text);
                    string degreecode = Convert.ToString(ddlBranch.SelectedItem.Value);
                    string batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
                    string strtit_acitivity = "";

                    for (int ij = 0; ij < dsselect.Tables[0].Rows.Count; ij++)
                    {
                        if (strtit_acitivity == "")
                        {
                            strtit_acitivity = Convert.ToString(dsselect.Tables[0].Rows[ij][1]);
                        }
                        else
                        {
                            strtit_acitivity = strtit_acitivity + "','" + Convert.ToString(dsselect.Tables[0].Rows[ij][1]);
                        }
                    }


                    string queryactivity = " select * from textvaltable where TextCriteria='RActv' and college_code='" + Convert.ToString(Session["collegecode"]) + "' and TextCode in ('" + strtit_acitivity + "') order by TextVal";

                    DataSet newact = new DataSet();
                    newact.Clear();


                    newact = da.select_method_wo_parameter(queryactivity, "Text");

                    if (newact.Tables[0].Rows.Count > 0)
                    {
                        combocolactivity.DataSource = newact;
                        combocolactivity.DataTextField = "TextVal";
                        combocolactivity.DataValueField = "TextCode";

                    }
                    // combocolactivity = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1acitivity);
                    combocolactivity.ShowButton = false;
                    combocolactivity.AutoPostBack = true;
                    combocolactivity.UseValue = true;
                }
                for (int i = 0; i < dsselect.Tables[0].Rows.Count; i++)
                {
                    fp1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                    fp1.Sheets[0].Cells[i, 1].CellType = combocolactivity;
                    fp1.Sheets[0].Cells[i, 1].Value = Convert.ToString(dsselect.Tables[0].Rows[i]["ActivityTextVal"]);
                }
                fp1.SaveChanges();
                fp1.Sheets[0].PageSize = fp1.Sheets[0].RowCount;
                btnrowadd.Visible = true;
                btnsaveparts.Visible = true;
                btnremove.Visible = true;
            }
            else if (ddlformate.SelectedIndex == 1)
            {
                FpSpread1.Visible = true;
                Button1.Visible = true;
                btnfpspread1save.Visible = true;
                btnfpspread1delete.Visible = true;
                lblerrvel.Visible = false;

                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                //darkstyle.ForeColor = System.Drawing.Color.Black;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Large;
                darkstyle.Border.BorderSize = 0;
                darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                string partname = Convert.ToString(ddltolparts.SelectedItem.Text).Trim();
                string ttsubttitle = Convert.ToString(ddlsubtt.SelectedItem.Text).Trim();
                string fopsql = "select * from CoCurr_Activitie where CoCurr_ID='" + Convert.ToString(ddlsubtt.SelectedItem.Value) + "' and  Batch_Year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "' ";
                string IsDirectEntry = "";
                string IsActivity = "";
                string IsActDesc = "";
                string IsGrade = "";
                fp1.Sheets[0].RowCount = 0;
                DataSet dsfpful = new DataSet();
                dsfpful.Clear();
                dsfpful = da.select_method_wo_parameter(fopsql, "Text");
                if (dsfpful.Tables[0].Rows.Count > 0)
                {
                    IsDirectEntry = Convert.ToString(dsfpful.Tables[0].Rows[0]["IsDirectEntry"]);
                    IsActivity = Convert.ToString(dsfpful.Tables[0].Rows[0]["IsActivity"]);
                    IsActDesc = "False";
                    IsGrade = "False";

                }
                if (IsDirectEntry.Trim() == "True")
                {
                    lblparterr.Text = "Sorry it is Direct Entry";
                    lblparterr.Visible = true;
                    hideaddactivity();
                    hideactivitygrade();
                    return;
                }
                string Degree_Code = "";
                string Batch_Year = "";
                string CoCurr_ID = "";

                Degree_Code = Convert.ToString(ddlBranch.SelectedItem.Value);
                Batch_Year = Convert.ToString(ddlBatch.SelectedItem.Text);
                CoCurr_ID = Convert.ToString(ddlsubtt.SelectedItem.Value);
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 4;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "From";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "To";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Description";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Grade";
                FpSpread1.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Columns[3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].Width = 70;
                FpSpread1.Sheets[0].Columns[1].Width = 70;
                FpSpread1.Sheets[0].Columns[2].Width = 171;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.White;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.CommandBar.Visible = false;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;

                dsbindv.Dispose();
                dsbindv.Reset();

                string strq3 = "select frompoint,topoint,description,grade from activity_gd where collegecode='" + Convert.ToString(Session["collegecode"]) + "' and   ActivityTextVal='" + Convert.ToString(ddlactivity.SelectedItem.Value) + "' and term='" + term + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "' and batch_year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' ";
                dsbindv = da.select_method_wo_parameter(strq3, "Text");
                if (dsbindv != null && dsbindv.Tables[0].Rows.Count > 0)
                {
                    FpSpread1.DataSource = dsbindv;
                    FpSpread1.DataBind();
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        catch
        {
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            bindsem();
            btnGo_Click(sender, e);
            fp1.Visible = false;
            btnrowadd.Visible = false;
            btnsaveparts.Visible = false;
            btnremove.Visible = false;

            FpSpread1.Visible = false;
            Button1.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
        }
        else
        {
            btnGo_Click(sender, e);
            fp1.Visible = false;
            btnrowadd.Visible = false;
            btnsaveparts.Visible = false;
            btnremove.Visible = false;

            FpSpread1.Visible = false;
            Button1.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
        }
        cbClass.Checked = false;
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        string course_id = Convert.ToString(ddlDegree.SelectedValue);
        cbClass.Checked = false;
        string collegecode = Convert.ToString(Session["collegecode"]);
        string usercode = Convert.ToString(Session["UserCode"]);

        string sqlnew = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + course_id + " and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "";
        DataSet ds = new DataSet();
        ds.Clear();
        ds = dacc.select_method_wo_parameter(sqlnew, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }

        bindsem();
        BindSectionDetail();

    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbClass.Checked = false;
        bindsem();
        BindSectionDetail();
        btnGo_Click(sender, e);
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            hideactivitygrade();
            hideaddactivity();
            string fopsql = "select * from CoCurr_Activitie where  Batch_Year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "'";
            DataSet dsfpful = new DataSet();
            dsfpful.Clear();
            dsfpful = dacc.select_method_wo_parameter(fopsql, "Text");
            DataView dv_demand_data = new DataView();
            arr.Clear();
            if (dsfpful.Tables[0].Rows.Count == 0)
            {
                parttable.Visible = false;
                lblparterr.Text = "Please Update Report Card Master Settings";
                lblparterr.Visible = true;
            }
            else
            {
                parttable.Visible = true;
                lblparterr.Text = "";
                lblparterr.Visible = false;
            }
            for (int i = 0; i < dsfpful.Tables[0].Rows.Count; i++)
            {
                string ttpartname = Convert.ToString(dsfpful.Tables[0].Rows[0][1]);
                string[] splitttpartname = ttpartname.Split('-');
                if (splitttpartname.GetUpperBound(0) >= 1)
                {
                    lbltitlepart.Text = "Select " + Convert.ToString(splitttpartname[0]);
                }

                if (!arr.Contains(Convert.ToString(dsfpful.Tables[0].Rows[i][1])))
                {
                    dsfpful.Tables[0].DefaultView.RowFilter = "PartName='" + Convert.ToString(dsfpful.Tables[0].Rows[i][1]) + "'";
                    arr.Add(Convert.ToString(dsfpful.Tables[0].Rows[i][1]));
                }
            }
            ddltolparts.DataSource = arr;
            ddltolparts.DataBind();
            if (ddltolparts.Items.Count > 0)
            {
                string sqlsubtt = "select * from CoCurr_Activitie where PartName like '" + Convert.ToString(ddltolparts.SelectedItem.Text).Trim() + "' and Batch_Year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlsubtt, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsubtt.DataSource = ds;
                    ddlsubtt.DataTextField = "SubTitle";
                    ddlsubtt.DataValueField = "CoCurr_ID";
                    ddlsubtt.DataBind();
                }
            }
        }
        catch
        {

        }
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbClass.Checked = false;
    }

    public void bindsem()
    {

        //--------------------semester load
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;

        string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + Convert.ToString(ddlBranch.SelectedValue) + " and batch_year=" + Convert.ToString(ddlBatch.Text) + " and college_code=" + Convert.ToString(Session["collegecode"]) + "";
        DataSet ds = new DataSet();
        ds.Clear();
        ds = dacc.select_method_wo_parameter(sqlnew, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            //first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]));
            bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]), out first_year);
            int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]), out duration);
            //duration = Convert.ToInt16(Convert.ToString(ds.Tables[0].Rows[0][0]));
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(Convert.ToString(i));
                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(Convert.ToString(i));
                }
            }
        }
        else
        {


            sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + Convert.ToString(ddlBranch.SelectedValue) + " and college_code=" + Convert.ToString(Session["collegecode"]) + "";

            ds.Clear();
            ds = dacc.select_method_wo_parameter(sqlnew, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                //first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]));
                //duration = Convert.ToInt16(Convert.ToString(ds.Tables[0].Rows[0][0]));

                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(Convert.ToString(i));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(Convert.ToString(i));
                    }
                }
            }
        }
        if (ddlSemYr.Items.Count > 0)
        {
            ddlSemYr.SelectedIndex = 0;
            BindSectionDetail();
        }


    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbClass.Checked = false;
        BindSectionDetail();
        btnGo_Click(sender, e);
    }

    public void BindSectionDetail()
    {

        string branch = Convert.ToString(ddlBranch.SelectedValue);
        string batch = Convert.ToString(ddlBatch.SelectedValue);

        string sqlnew = "select distinct sections from registration where batch_year=" + Convert.ToString(ddlBatch.SelectedValue) + " and degree_code=" + Convert.ToString(ddlBranch.SelectedValue) + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
        DataSet ds = new DataSet();
        ds.Clear();
        ds = dacc.select_method_wo_parameter(sqlnew, "Text");

        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));

        if (ds.Tables[0].Rows.Count > 0)
        {

            ddlSec.Enabled = true;

        }
        else
        {
            ddlSec.Enabled = false;

        }

    }

    public void BindBatch()
    {

        try
        {
            string Master1 = "";
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {

                string group = Convert.ToString(Session["group_code"]);
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = Convert.ToString(group_semi[0]);
                }
            }
            else
            {
                Master1 = Convert.ToString(Session["usercode"]);
            }
            string collegecode = Convert.ToString(Session["collegecode"]);
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "'";

            DataSet ds = dacc.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = ddlBatch.Items.Count - 1;
            }
        }
        catch
        {
        }
    }

    public void bindbranch() // added by sridhar 06 sep 2014
    {
        try
        {
            DataSet ds = new DataSet();

            ds.Clear();
            ddlBranch.Items.Clear();
            hat.Clear();
            string usercode = Convert.ToString(Session["usercode"]);
            string collegecode = Convert.ToString(Session["collegecode"]);
            string singleuser = Convert.ToString(Session["single_user"]);
            string group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            string course_id = Convert.ToString(ddlDegree.SelectedValue);

            string query = "";
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
            {
                query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + course_id + " and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + "";
            }
            else
            {
                query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + course_id + " and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "";
            }
            ds = dacc.select_method_wo_parameter(query, "Text");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
        }
        catch
        {
        }
    }

    public void BindDegree()
    {
        string college_code = Convert.ToString(Session["collegecode"]);
        string query = "";

        string usercode = Convert.ToString(Session["usercode"]);

        string singleuser = Convert.ToString(Session["single_user"]);
        string group_user = Convert.ToString(Session["group_code"]);
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = Convert.ToString(group_semi[0]);
        }


        if ((Convert.ToString(group_user).Trim() != "") && (group_user.Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + "";
        }
        else
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "";
        }


        DataSet ds = new DataSet();
        ds.Clear();
        ds = dacc.select_method_wo_parameter(query, "Text");
        // DataSet ds = ClsAttendanceAccess.GetDegreeDetail(Convert.ToString(collegecode));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();
            // ddlDegree.Items.Insert(0, new ListItem("--Select--", "-1"));
        }

    }

    protected void ddlformate_SelectedIndexChanged(object sender, EventArgs e)
    {
        hideactivitygrade();
        if (ddlformate.SelectedIndex == 1)
        {
            fp1.Visible = false;
            btnrowadd.Visible = false;
            btnsaveparts.Visible = false;
            btnremove.Visible = false;
            int cccount = 0;
            string partname = Convert.ToString(ddltolparts.SelectedItem.Text).Trim();
            string ttsubttitle = Convert.ToString(ddlsubtt.SelectedItem.Text).Trim();
            string fopsql = "select * from CoCurr_Activitie where CoCurr_ID='" + Convert.ToString(ddlsubtt.SelectedItem.Value) + "' and  Batch_Year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "'";
            string IsDirectEntry = "";
            string IsActivity = "";
            string IsActDesc = "";
            string IsGrade = "";
            fp1.Sheets[0].RowCount = 0;
            DataSet dsfpful = new DataSet();
            dsfpful.Clear();
            dsfpful = da.select_method_wo_parameter(fopsql, "Text");
            if (dsfpful.Tables[0].Rows.Count > 0)
            {
                IsDirectEntry = Convert.ToString(dsfpful.Tables[0].Rows[0]["IsDirectEntry"]);
                IsActivity = Convert.ToString(dsfpful.Tables[0].Rows[0]["IsActivity"]);
                IsActDesc = "False";
                IsGrade = "False";

            }
            if (IsDirectEntry.Trim() == "True")
            {
                lblparterr.Text = "Sorry it is Direct Entry";
                fp1.Visible = false;
                btnrowadd.Visible = false;
                btnsaveparts.Visible = false;
                btnremove.Visible = false;
                ddlactivity.Visible = false;
                return;
            }
            else
            {
                fp1.Visible = false;
                lblparterr.Text = "";
            }
            string Degree_Code = "";
            string Batch_Year = "";
            string CoCurr_ID = "";

            Degree_Code = Convert.ToString(ddlBranch.SelectedItem.Value);
            Batch_Year = Convert.ToString(ddlBatch.SelectedItem.Text);
            CoCurr_ID = Convert.ToString(ddlsubtt.SelectedItem.Value);
            string sqlselect = "select * from  activity_entry where CoCurr_ID='" + CoCurr_ID + "' and Degree_Code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' and term='" + term + "'";
            DataSet dsselect = new DataSet();
            dsselect.Clear();
            dsselect = da.select_method_wo_parameter(sqlselect, "Text");
            if (dsselect.Tables[0].Rows.Count > 0)
            {
                fp1.Sheets[0].RowCount = dsselect.Tables[0].Rows.Count;
                string currentsem = Convert.ToString(ddlSemYr.SelectedItem.Text);
                string degreecode = Convert.ToString(ddlBranch.SelectedItem.Value);
                string batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
                string strtit_acitivity = "";

                for (int ij = 0; ij < dsselect.Tables[0].Rows.Count; ij++)
                {
                    if (strtit_acitivity == "")
                    {
                        strtit_acitivity = Convert.ToString(dsselect.Tables[0].Rows[ij][1]);
                    }
                    else
                    {
                        strtit_acitivity = strtit_acitivity + "','" + Convert.ToString(dsselect.Tables[0].Rows[ij][1]);
                    }
                }
                string queryactivity = " select * from textvaltable where TextCriteria='RActv' and college_code='" + Convert.ToString(Session["collegecode"]) + "' and TextCode in ('" + strtit_acitivity + "') ";

                DataSet newact = new DataSet();
                newact.Clear();
                newact = da.select_method_wo_parameter(queryactivity, "Text");

                if (newact.Tables[0].Rows.Count > 0)
                {
                    ddlactivity.DataSource = newact;
                    ddlactivity.DataTextField = "TextVal";
                    ddlactivity.DataValueField = "TextCode";
                    ddlactivity.DataBind();
                    ddlactivity.Visible = true;
                }
                else
                {
                    lblparterr.Visible = false;
                }
                // combocolactivity = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1acitivity);
            }
            else
            {
                hideactivitygrade();
                ddlactivity.Visible = false;
                lblparterr.Text = "No Activity are Created";
                lblparterr.Visible = true;
                FpSpread1.Visible = false;
            }
        }
        else
        {
            string Degree_Code = "";
            string Batch_Year = "";
            string CoCurr_ID = "";

            Degree_Code = Convert.ToString(ddlBranch.SelectedItem.Value);
            Batch_Year = Convert.ToString(ddlBatch.SelectedItem.Text);
            CoCurr_ID = Convert.ToString(ddlsubtt.SelectedItem.Value);
            string sqlselect = "select * from  activity_entry where CoCurr_ID='" + CoCurr_ID + "' and Degree_Code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' and term='" + term + "'";
            DataSet dsselect = new DataSet();
            dsselect.Clear();
            dsselect = da.select_method_wo_parameter(sqlselect, "Text");
            if (dsselect.Tables[0].Rows.Count == 0)
            {
                hideactivitygrade();
                ddlactivity.Visible = false;
                lblparterr.Text = "No Activity are Created";
                lblparterr.Visible = true;
                FpSpread1.Visible = false;
            }
            else
            {
                hideactivitygrade();
                ddlactivity.Visible = false;
                lblparterr.Visible = false;
            }
            //hideactivitygrade();
            //FpSpread1.Visible = false;


        }
    }

    protected void ddlactivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlformate.SelectedIndex == 1)
        {
            btngofp_Click(sender, e);
        }
    }

    protected void ddlsubtt_SelectedIndexChanged(object sender, EventArgs e)
    {
        hideactivitygrade();
        if (ddlactivity.Visible == true)
        {
            //FpSpread1.Visible = false;
            //Button1.Visible = false;
            //btnfpspread1save.Visible = false;
            //btnfpspread1delete.Visible = false;

            if (ddlformate.SelectedIndex == 1)
            {
                fp1.Visible = false;
                btnrowadd.Visible = false;
                btnsaveparts.Visible = false;
                btnremove.Visible = false;
                int cccount = 0;
                string partname = Convert.ToString(ddltolparts.SelectedItem.Text).Trim();
                string ttsubttitle = Convert.ToString(ddlsubtt.SelectedItem.Text).Trim();
                string fopsql = "select * from CoCurr_Activitie where CoCurr_ID='" + Convert.ToString(ddlsubtt.SelectedItem.Value) + "' and  Batch_Year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "'";
                string IsDirectEntry = "";
                string IsActivity = "";
                string IsActDesc = "";
                string IsGrade = "";
                fp1.Sheets[0].RowCount = 0;
                DataSet dsfpful = new DataSet();
                dsfpful.Clear();
                dsfpful = da.select_method_wo_parameter(fopsql, "Text");
                if (dsfpful.Tables[0].Rows.Count > 0)
                {
                    IsDirectEntry = Convert.ToString(dsfpful.Tables[0].Rows[0]["IsDirectEntry"]);
                    IsActivity = Convert.ToString(dsfpful.Tables[0].Rows[0]["IsActivity"]);
                    IsActDesc = "False";
                    IsGrade = "False";
                }
                if (IsDirectEntry.Trim() == "True")
                {
                    lblparterr.Text = "Sorry it is Direct Entry";
                    fp1.Visible = false;
                    btnrowadd.Visible = false;
                    btnsaveparts.Visible = false;
                    btnremove.Visible = false;
                    ddlactivity.Visible = false;
                    return;
                }
                else
                {
                    fp1.Visible = false;
                    lblparterr.Text = "";
                }
                string Degree_Code = "";
                string Batch_Year = "";
                string CoCurr_ID = "";

                Degree_Code = Convert.ToString(ddlBranch.SelectedItem.Value);
                Batch_Year = Convert.ToString(ddlBatch.SelectedItem.Text);
                CoCurr_ID = Convert.ToString(ddlsubtt.SelectedItem.Value);
                string sqlselect = "select * from  activity_entry where CoCurr_ID='" + CoCurr_ID + "' and Degree_Code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' and term='" + term + "' ";
                DataSet dsselect = new DataSet();
                dsselect.Clear();
                dsselect = da.select_method_wo_parameter(sqlselect, "Text");
                if (dsselect.Tables[0].Rows.Count > 0)
                {
                    fp1.Sheets[0].RowCount = dsselect.Tables[0].Rows.Count;
                    string currentsem = Convert.ToString(ddlSemYr.SelectedItem.Text);
                    string degreecode = Convert.ToString(ddlBranch.SelectedItem.Value);
                    string batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
                    string strtit_acitivity = "";

                    for (int ij = 0; ij < dsselect.Tables[0].Rows.Count; ij++)
                    {
                        if (strtit_acitivity == "")
                        {
                            strtit_acitivity = Convert.ToString(dsselect.Tables[0].Rows[ij][1]);
                        }
                        else
                        {
                            strtit_acitivity = strtit_acitivity + "','" + Convert.ToString(dsselect.Tables[0].Rows[ij][1]);
                        }
                    }

                    string queryactivity = " select * from textvaltable where TextCriteria='RActv' and college_code='" + Convert.ToString(Session["collegecode"]) + "' and TextCode in ('" + strtit_acitivity + "') ";

                    DataSet newact = new DataSet();
                    newact.Clear();
                    newact = da.select_method_wo_parameter(queryactivity, "Text");

                    if (newact.Tables[0].Rows.Count > 0)
                    {
                        ddlactivity.DataSource = newact;
                        ddlactivity.DataTextField = "TextVal";
                        ddlactivity.DataValueField = "TextCode";
                        ddlactivity.DataBind();
                        ddlactivity.Visible = true;
                        btngofp_Click(sender, e);
                    }
                    else
                    {
                        lblparterr.Visible = false;
                    }
                    // combocolactivity = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1acitivity);
                }
                else
                {
                    ddlactivity.Visible = false;
                    lblparterr.Text = "No Activity are Created";
                    lblparterr.Visible = true;
                }
            }
            else
            {
                string Degree_Code = "";
                string Batch_Year = "";
                string CoCurr_ID = "";
                Degree_Code = Convert.ToString(ddlBranch.SelectedItem.Value);
                Batch_Year = Convert.ToString(ddlBatch.SelectedItem.Text);
                CoCurr_ID = Convert.ToString(ddlsubtt.SelectedItem.Value);
                string sqlselect = "select * from  activity_entry where CoCurr_ID='" + CoCurr_ID + "' and Degree_Code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' and term='" + term + "'";
                DataSet dsselect = new DataSet();
                dsselect.Clear();
                dsselect = da.select_method_wo_parameter(sqlselect, "Text");
                if (dsselect.Tables[0].Rows.Count == 0)
                {

                    ddlactivity.Visible = false;
                    lblparterr.Text = "No Activity are Created";
                    lblparterr.Visible = true;
                    FpSpread1.Visible = false;
                }

                ddlactivity.Visible = false;
            }
        }
        else
        {
            btngofp_Click(sender, e);
        }

    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        int rcount = FpSpread1.Sheets[0].RowCount++;
        FpSpread1.Sheets[0].ColumnCount = 4;
        lblerrvel.Visible = false;
        FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.White;

        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[0].Locked = true;
        FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
        FarPoint.Web.Spread.DoubleCellType intgrcell1 = new FarPoint.Web.Spread.DoubleCellType();
        FarPoint.Web.Spread.RegExpCellType rgex = new FarPoint.Web.Spread.RegExpCellType();
        intgrcell.FormatString = Convert.ToString(System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals);
        intgrcell.MaximumValue = Convert.ToInt32(100);
        intgrcell.MinimumValue = 0;
        intgrcell.ErrorMessage = "Enter valid Number";
        FpSpread1.Sheets[0].Columns[0].CellType = intgrcell;
        FpSpread1.Sheets[0].Columns[1].CellType = intgrcell1;
        //rgex.ValidationExpression = @"^[A-Z]$";// @"/^[A-Z]{1}$/";
        //rgex.ErrorMessage = "Must Enter One Capital Letter only";
        //FpSpread1.Sheets[0].Columns[3].CellType = rgex;
        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;

        FpSpread1.Sheets[0].Columns[1].Locked = false;
        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        int minustwo = FpSpread1.Sheets[0].RowCount - 2;

        if (rcount != 0)
        {

        }
        if (rcount != 0)
        {
            if (Convert.ToDouble(Convert.ToString(FpSpread1.Sheets[0].Cells[minustwo, 1].Text)) != 100)
            {
                if (Convert.ToString(FpSpread1.Sheets[0].Cells[minustwo, 1].Text) != "" && Convert.ToString(FpSpread1.Sheets[0].Cells[minustwo, 0].Text) != "" && Convert.ToString(FpSpread1.Sheets[0].Cells[minustwo, 2].Text) != "")
                {
                    double temp = Convert.ToDouble(Convert.ToString(FpSpread1.Sheets[0].Cells[minustwo, 1].Text));

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = intgrcell;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(temp + 0.01);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = intgrcell1;
                    intgrcell1.MinimumValue = temp + 0.01;
                    intgrcell1.MaximumValue = 100;
                    intgrcell1.ErrorMessage = "Enter Value Between " + (temp + 0.01) + " and 100.";                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    FpSpread1.Sheets[0].RowCount--;
                    lblerrvel.Text = "Please Enter From & To Range";
                    lblerrvel.Visible = true;
                }
            }
            else
            {
                FpSpread1.Sheets[0].RowCount--;
                lblerrvel.Text = "You Are Reached Maximum Range 100";
                lblerrvel.Visible = true;
            }
        }
        else
        {
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = intgrcell;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "00.00";
            intgrcell1.MaximumValue = 100;
            intgrcell1.MinimumValue = 0;
            intgrcell1.ErrorMessage = "Enter Value Between 0 and 100";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = intgrcell1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "00.00";
        }
    }

    //protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}

    //public void loadcollege()
    //{
    //  string  group_user = Convert.ToString(Session["group_code"]);
    //  string columnfield = "";
    //    if (group_user.Contains(';'))
    //    {
    //        string[] group_semi = group_user.Split(';');
    //        group_user = Convert.ToString(group_semi[0]);
    //    }
    //    if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
    //    {
    //        columnfield = " and group_code='" + group_user + "'";
    //    }
    //    else
    //    {
    //        columnfield = " and user_code='" + Convert.ToString(Session["usercode"]) + "'";
    //    }
    //    hat.Clear();
    //    hat.Add("column_field", Convert.ToString(columnfield));
    //    ds.Dispose();
    //    ds.Reset();
    //    ds = da.select_method("bind_college", hat, "sp");
    //    ddlcollege.Items.Clear();
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        ddlcollege.DataSource = ds;
    //        ddlcollege.DataTextField = "collname";
    //        ddlcollege.DataValueField = "college_code";
    //        ddlcollege.DataBind();

    //    }
    //}

    protected void btnfpspread1save_Click1(object sender, EventArgs e)
    {
        string batchyear = "", degreecode = "", collegecode = "", term = "", CoCurr_ID = "";
        batchyear = Convert.ToString(ddlBatch.SelectedValue);
        collegecode = Convert.ToString(Session["collegecode"]);
        degreecode = Convert.ToString(ddlBranch.SelectedValue);
        term = Convert.ToString(ddlSemYr.SelectedValue);
        lblerrvel.Visible = false;
        bool issuc = false;
        int ires = 0;
        if (cbClass.Checked == true)
        {
            CoCurr_ID = collegecode + batchyear + degreecode + term;
        }
        else
        {
            CoCurr_ID = Convert.ToString(ddlactivity.SelectedItem.Value);
        }

        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].ColumnCount = 4;
            ht.Clear();
            string querystr = "";
            querystr = "Delete from activity_gd where collegecode=" + Convert.ToString(Session["collegecode"]) + " and ActivityTextVal='" + CoCurr_ID + "' and term='" + term + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "' and batch_year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' ";
            da.select_method(querystr, ht, "");

            double from = 0;
            double to = 0;
            string classification = "";
            string grade = "";
            for (int v = 0; v < FpSpread1.Sheets[0].RowCount; v++)
            {
                if (Convert.ToString(FpSpread1.Sheets[0].Cells[v, 0].Text) != "" && Convert.ToString(FpSpread1.Sheets[0].Cells[v, 1].Text) != "" && Convert.ToString(FpSpread1.Sheets[0].Cells[v, 2].Text) != "")
                {
                    from = Math.Round((Convert.ToDouble(Convert.ToString(FpSpread1.Sheets[0].Cells[v, 0].Text).Trim())), 2);
                    to = Math.Round((Convert.ToDouble(Convert.ToString(FpSpread1.Sheets[0].Cells[v, 1].Text).Trim())), 2);
                    classification = Convert.ToString(Convert.ToString(FpSpread1.Sheets[0].Cells[v, 2].Text).Trim());
                    grade = Convert.ToString(Convert.ToString(FpSpread1.Sheets[0].Cells[v, 3].Text).Trim()).Trim();
                    //FpSpread1.Sheets[0].Cells[v, 3].HorizontalAlign = HorizontalAlign.Center;
                    if (from != null && to != null && classification != "" && grade != "")
                    {
                        string strinsert = "insert into activity_gd(ActivityTextVal,frompoint,topoint,description,collegecode,grade,term,Degree_Code,batch_year) values('" + CoCurr_ID + "','" + from + "','" + to + "','" + classification + "','" + Convert.ToString(Session["collegecode"]) + "','" + grade + "','" + term + "','" + Convert.ToString(ddlBranch.SelectedItem.Value) + "','" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' )";
                        ires = da.insert_method(strinsert, ht, "");
                        if (ires > 0)
                        {
                            issuc = true;
                        }
                    }
                    else
                    {
                        lblerrvel.Visible = true;
                    }
                }
                else
                {
                    lblerrvel.Visible = true;
                }
            }
            if (issuc)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
            }
        }
    }

    protected void btnfpspread1delete_Click1(object sender, EventArgs e)
    {
        string batchyear = "", degreecode = "", collegecode = "", term = "", CoCurr_ID = "";
        batchyear = Convert.ToString(ddlBatch.SelectedValue);
        collegecode = Convert.ToString(Session["collegecode"]);
        degreecode = Convert.ToString(ddlBranch.SelectedValue);
        term = Convert.ToString(ddlSemYr.SelectedValue);
        lblerrvel.Visible = false;
        if (cbClass.Checked == true)
        {
            CoCurr_ID = collegecode + batchyear + degreecode + term;
        }
        else
        {
            CoCurr_ID = Convert.ToString(ddlactivity.SelectedItem.Value);
        }
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            ht.Clear();
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].ColumnCount = 4;
            string querystr = "";
            querystr = "Delete from activity_gd where collegecode=" + Convert.ToString(Session["collegecode"]) + " and ActivityTextVal='" + CoCurr_ID + "' and term='" + term + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "' ";
            da.select_method(querystr, ht, "");
            FpSpread1.Sheets[0].RowCount = 0;
        }
        else
        {
            lblerrvel.Text = "No Record Found";
            lblerrvel.Visible = true;
        }
    }

    public void hideactivitygrade()
    {
        FpSpread1.Visible = false;
        Button1.Visible = false;
        btnfpspread1save.Visible = false;
        btnfpspread1delete.Visible = false;
        lblerrvel.Visible = false;
    }

    public void hideaddactivity()
    {
        fp1.Visible = false;
        btnrowadd.Visible = false;
        btnsaveparts.Visible = false;
        btnremove.Visible = false;
    }

    protected void cbClass_CheckedChanged(object sender, EventArgs e)
    {
        lblErrorMsg.Text = "";
        lblErrorMsg.Visible = false;
        lblparterr.Visible = false;
        lblparterr.Text = "";
        string batchyear = "", degreecode = "", collegecode = "", term = "", CoCurr_ID = "";
        batchyear = Convert.ToString(ddlBatch.SelectedValue);
        collegecode = Convert.ToString(Session["collegecode"]);
        degreecode = Convert.ToString(ddlBranch.SelectedValue);
        term = Convert.ToString(ddlSemYr.SelectedValue);
        CoCurr_ID = collegecode + batchyear + degreecode + term;
        fp1.Visible = false;
        btnremove.Visible = false;
        btnrowadd.Visible = false;
        btnsaveparts.Visible = false;
        lblerrvel.Visible = false;
        if (cbClass.Checked == true)
        {
            FpSpread1.Visible = true;
            Button1.Visible = true;
            btnfpspread1save.Visible = true;
            btnfpspread1delete.Visible = true;
            lblerrvel.Visible = false;
            parttable.Visible = false;

            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Large;
            darkstyle.Border.BorderSize = 0;
            darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 4;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "From";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "To";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Description";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Grade";
            FpSpread1.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].Width = 70;
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Width = 70;
            FpSpread1.Sheets[0].Columns[2].Width = 171;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.White;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.CommandBar.Visible = false;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;

            dsbindv.Dispose();
            dsbindv.Reset();

            string strq3 = "select frompoint,topoint,description,grade from activity_gd where collegecode='" + Convert.ToString(Session["collegecode"]) + "' and   ActivityTextVal='" + CoCurr_ID + "' and term='" + term + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "' and batch_year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' ";
            dsbindv = da.select_method_wo_parameter(strq3, "Text");
            if (dsbindv != null && dsbindv.Tables[0].Rows.Count > 0)
            {
                FpSpread1.DataSource = dsbindv;
                FpSpread1.DataBind();
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        else
        {
            FpSpread1.Visible = false;
            Button1.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
            //parttable.Visible = false;
            string fopsql = "select * from CoCurr_Activitie where  Batch_Year='" + Convert.ToString(ddlBatch.SelectedItem.Text) + "' and Degree_Code='" + Convert.ToString(ddlBranch.SelectedItem.Value) + "'";
            DataSet dsfpful = new DataSet();
            dsfpful.Clear();
            dsfpful = dacc.select_method_wo_parameter(fopsql, "Text");
            DataView dv_demand_data = new DataView();
            arr.Clear();
            if (dsfpful.Tables[0].Rows.Count == 0)
            {
                parttable.Visible = false;
                lblparterr.Text = "Please Update Report Card Master Settings";
                lblparterr.Visible = true;
            }
            else
            {
                parttable.Visible = true;
                lblparterr.Text = "";
                lblparterr.Visible = false;
            }
        }
    }

}