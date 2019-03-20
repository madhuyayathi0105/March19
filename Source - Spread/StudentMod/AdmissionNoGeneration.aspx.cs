using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Text;
using System.Configuration;
public partial class StudentMod_AdmissionNoGeneration : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable hat = new Hashtable();
    AdmissionNumberAndApplicationNumberGeneration autogen = new AdmissionNumberAndApplicationNumberGeneration();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
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
                Bindcollege();
                // gridquota_DataBound();
                //string appNo = autogen.AdmissionNoAndApplicationNumberGeneration(0, appno: "23190");
                // AdmissionNoGeneration(0,  appno: "12172");//"", "", "", "", "",
            }
        }
        catch (Exception ex) { }
    }
    public void rdb_applicationno_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_applicationno.Checked == true)
        {
            lblsize.Text = "Application Number Size";
        }
    }
    public void rdb_admissionnoCheckedChanged(object sender, EventArgs e)
    {
        if (rdb_admissionno.Checked == true)
        {
            lblsize.Text = "Admission Number Size";
        }
    }
    public void Bindcollege()
    {
        try
        {
            string columnfield = "";
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = dsprint;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                //chklstclgacr.DataSource = dsprint;
                //chklstclgacr.DataTextField = "collname";
                //chklstclgacr.DataValueField = "college_code";
                //chklstclgacr.DataBind();
                //chklstclgacr.Items[0].Selected = true;
                //for (int i = 0; i < chklstclgacr.Items.Count; i++)
                //{
                //    chklstclgacr.Items[i].Selected = true;
                //    if (chklstclgacr.Items[i].Selected == true)
                //    {
                //        count += 1;
                //    }
                //    if (chklstclgacr.Items.Count == count)
                //    {
                //        chkclgacr.Checked = true;
                //    }
                //}
                //if (chkclgacr.Checked == true)
                //{
                //    for (int i = 0; i < chklstclgacr.Items.Count; i++)
                //    {
                //        chklstclgacr.Items[i].Selected = true;
                //        txtclgacr.Text = "Instution(" + (chklstclgacr.Items.Count) + ")";
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < chklstclgacr.Items.Count; i++)
                //    {
                //        chklstclgacr.Items[i].Selected = false;
                //        txtclgacr.Text = "---Select---";
                //    }
                //}
                //txtclgacr.Enabled = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Generate_Click(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            DataSet ds = new DataSet();
            string clgcode = Session["collegecode"].ToString();
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].Columns.Count = 5;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Columns[0].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "From Digit";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Columns[1].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "To Digit";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Columns[2].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Columns[3].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Prefix/Suffix";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Columns[4].Width = 130;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0BA6CB");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            int val = 0;
            Int32.TryParse(txtsize.Text, out val);
            string[] item = new string[0];
            if (val > 0)
            {
                for (int v = 0; v <= val; v++)
                {
                    Array.Resize(ref item, item.Length + 1);
                    item[item.Length - 1] = Convert.ToString(v);
                }
            }
            FarPoint.Web.Spread.ComboBoxCellType dropdown1 = new FarPoint.Web.Spread.ComboBoxCellType();
            string query = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='ApplicationNumberGeneration' and CollegeCode='" + clgcode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                dropdown1.DataSource = ds;
                dropdown1.DataTextField = "MasterValue";
                dropdown1.DataValueField = "MasterCode";
            }
            dropdown1.AutoPostBack = false;
            FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
            string[] prefix = new string[2];
            prefix[0] = "Prefix";
            prefix[1] = "Suffix";
            FarPoint.Web.Spread.ComboBoxCellType dropdown = new FarPoint.Web.Spread.ComboBoxCellType(item, item);
            FarPoint.Web.Spread.ComboBoxCellType dropdown2 = new FarPoint.Web.Spread.ComboBoxCellType(prefix, prefix);
            FarPoint.Web.Spread.DoubleCellType txtserialNo = new FarPoint.Web.Spread.DoubleCellType();
            btn.CssClass = "textbox btn";
            btn.ButtonType = FarPoint.Web.Spread.ButtonType.LinkButton;
            btn.Text = "+";
            DataSet CodeGenDS = d2.select_method_wo_parameter("   select NumberType,NumberLength,ag.collegeCode,FRange,TRange,DifferentRange,HeaderCode, Case when convert(varchar(max), startNo)!=0 then convert(varchar(max),startNo) else (case when PrefixOrSufix=1 then 'Prefix' when PrefixOrSufix=2 then 'Suffix'  end) end as PrefixOrSufix,GenerationNumber,NumberSize,upper(MasterValue) as type,startNo from AdmissionNoGeneration ag,CO_MasterValues m where ag.collegecode='" + Convert.ToString(ddlclg.SelectedItem.Value) + "' and ag.headercode=m.MasterCode and MasterCriteria='ApplicationNumberGeneration' order by frange,trange ", "text");//ag.collegecode=m.collegecode and
            string Frange = string.Empty;
            string Trange = string.Empty;
            string HeaderCode = string.Empty;
            string PrefixOrSufix = string.Empty;
            string NumberSize = string.Empty;
            string type = string.Empty;
            int rangeDiff = 0;
            int StartNo = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = dropdown;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = dropdown;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                dropdown1.ShowButton = true;
                dropdown1.AutoPostBack = true;
                dropdown1.UseValue = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = dropdown1;
                FpSpread1.Sheets[0].Columns[3].CellType = dropdown1;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = dropdown2;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                if (CodeGenDS.Tables != null)//24.01.18 barath
                {
                    if (i < CodeGenDS.Tables[0].Rows.Count)
                    {
                        if (CodeGenDS.Tables[0].Rows.Count > 0)
                        {
                            Frange = Convert.ToString(CodeGenDS.Tables[0].Rows[i]["FRange"]);
                            Trange = Convert.ToString(CodeGenDS.Tables[0].Rows[i]["TRange"]);
                            HeaderCode = Convert.ToString(CodeGenDS.Tables[0].Rows[i]["HeaderCode"]);
                            PrefixOrSufix = Convert.ToString(CodeGenDS.Tables[0].Rows[i]["PrefixOrSufix"]);
                            int.TryParse(Convert.ToString(CodeGenDS.Tables[0].Rows[i]["DifferentRange"]), out rangeDiff);
                            int.TryParse(Convert.ToString(CodeGenDS.Tables[0].Rows[i]["startNo"]), out StartNo);
                            NumberSize = Convert.ToString(CodeGenDS.Tables[0].Rows[i]["GenerationNumber"]);
                            type = Convert.ToString(CodeGenDS.Tables[0].Rows[i]["type"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Frange;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Trange;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = HeaderCode;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = PrefixOrSufix;
                            if (type == "QUOTA")
                            {
                                btn.Text = "Quota Settings";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = btn;
                            }
                            else if (type == "SERIAL NO")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txtserialNo;
                                //string sd = generateApplicationNumber(StartNo, rangeDiff);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = PrefixOrSufix;
                            }
                        }
                    }
                }
                FpSpread1.SaveChanges();
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            FpSpread1.Height = 270;
            FpSpread1.Width = 568;
            FpSpread1.Visible = true;
            btnSave.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
    public string generateApplicationNumber(int serialStartNo, int size)
    {
        string appNoString = serialStartNo.ToString();
        if (size != appNoString.Length && size > appNoString.Length)
        {
            while (size != appNoString.Length)
                appNoString = "0" + appNoString;
        }
        return appNoString;
    }
    protected void Generate_Go_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        lblerror.Visible = false;
        FpSpread1.SaveChanges();
        StringBuilder sb = new StringBuilder();
        int finalValue = 0;
        bool isValid = true;
        string numberlength = Convert.ToString(txtsize.Text);
        int FromRange = 0; int ToRange = 0;
        int val = 0;
        string FromRangeValue = string.Empty;
        string ToRangeValue = string.Empty;
        string clgdetailsvalue = string.Empty;
        string pre_suffvalue = string.Empty;
        string SerialNo = string.Empty;
        if (rdb_applicationno.Checked == true)
        {
            val = 1;
        }
        if (rdb_admissionno.Checked == true)
        {
            val = 0;
        }
        bool insertBool = false;
        string clgcode = "";
        //for (int chk = 0; chk < chklstclgacr.Items.Count; chk++)
        //{
        isValid = true;
        //if (chklstclgacr.Items[chk].Selected == true)
        //{
        for (int j = 0; j < FpSpread1.Sheets[0].RowCount; j++)
        {
            FromRangeValue = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 1].Text).Trim();
            ToRangeValue = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 2].Text).Trim();
            clgdetailsvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 3].Text).Trim();
            pre_suffvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 4].Text).Trim();
            SerialNo = string.Empty;
            if (FromRangeValue != "" && ToRangeValue != "" && clgdetailsvalue != "")
            {
                int.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[j, 1].Text).Trim(), out FromRange);
                int.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[j, 2].Text).Trim(), out ToRange);
                if (ToRange < FromRange)
                {
                    isValid = false;
                    sb.Append("Enter the Range Greater Than Start Range");
                    popup_alert.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = sb.ToString();
                    return;
                }
                if (j > 0)
                {
                    if (FromRange < finalValue)
                    {
                        isValid = false;
                        sb.Append("Enter the Range Greater than selected Row Value");
                        popup_alert.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = sb.ToString();
                        return;
                    }
                }
                finalValue = ToRange;
            }
        }
        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
        {
            FromRangeValue = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Text).Trim();
            ToRangeValue = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text).Trim();
            clgdetailsvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text).Trim();
            pre_suffvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text).Trim();
            SerialNo = string.Empty;
            if (FromRangeValue != "" && ToRangeValue != "" && clgdetailsvalue != "")
            {
                int.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Text).Trim(), out FromRange);
                int.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text).Trim(), out ToRange);
                if (ToRange < FromRange)
                {
                    isValid = false;
                    sb.Append("Enter the Range Greater Than Start Range");
                    lblerror.Visible = true;
                    lblerror.Text = sb.ToString();
                    return;
                }
                if (i > 0)
                {
                    if (FromRange < finalValue)
                    {
                        isValid = false;
                        sb.Append("Enter the Range Greater than selected Row Value");
                        lblerror.Visible = true;
                        lblerror.Text = sb.ToString();
                        return;
                    }
                }
                finalValue = ToRange;
                string clgdetails = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text).Trim();
                string pre_suff = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text).Trim();
                string HeaderCode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Value).Trim();
                string PrefixOrSufix = string.Empty;
                if (pre_suff.Trim().ToUpper() == "PREFIX")
                    PrefixOrSufix = "1";
                else if (pre_suff.Trim().ToUpper() == "SUFFIX")
                    PrefixOrSufix = "2";
                else
                    PrefixOrSufix = pre_suff;
                if (clgdetails.Trim().ToLower() == "serial no")
                {
                    SerialNo = PrefixOrSufix;
                    PrefixOrSufix = string.Empty;
                }
                if (clgdetails.Trim().ToLower() == "quota")
                {
                    PrefixOrSufix = string.Empty;
                }
                if (FromRange < ToRange)
                {
                    //clgcode = Convert.ToString(chklstclgacr.Items[chk].Value);
                    clgcode = Convert.ToString(ddlclg.SelectedItem.Value);
                    string query = "if exists(select * from AdmissionNoGeneration where collegeCode='" + clgcode + "' and HeaderCode='" + HeaderCode + "')update AdmissionNoGeneration set NumberType='" + val + "',NumberLength='" + numberlength + "',FRange='" + FromRange + "',TRange='" + ToRange + "',HeaderCode='" + HeaderCode + "',PrefixOrSufix='" + PrefixOrSufix + "',StartNo='" + SerialNo + "' where collegeCode='" + clgcode + "' and HeaderCode='" + HeaderCode + "' else insert into AdmissionNoGeneration(NumberType,NumberLength,FRange,TRange,HeaderCode,PrefixOrSufix,collegeCode,StartNo)values('" + val + "','" + numberlength + "','" + FromRange + "','" + ToRange + "', '" + HeaderCode + "','" + PrefixOrSufix + "','" + clgcode + "','" + SerialNo + "')";
                    int insert = d2.update_method_wo_parameter(query, "text");
                    if (insert != 0)
                        insertBool = true;
                }
                popup_alert.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Saved Successfully";
            }
            //else
            //{
            //    lblerror.Visible = false;
            //}
        }
        //  }
        //}
    }
    public void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)//del
    {
        FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
        FarPoint.Web.Spread.DoubleCellType txtserialNo = new FarPoint.Web.Spread.DoubleCellType();
        txtserialNo.ErrorMessage = "Allow only numbers";
        btn.ButtonType = FarPoint.Web.Spread.ButtonType.LinkButton;
        btn.Text = "Quota Settings";
        string clgDetails = string.Empty;
        string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
        string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
        if (actrow.Trim() != "" && actcol.Trim() != "")
        {
            clgDetails = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(actcol)].Text);
            if (clgDetails == "Quota")
            {
                btn.Text = "Quota Settings";
                FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].CellType = btn;
            }
            //else
            //{
            //    string[] prefix = new string[2];
            //    prefix[0] = "Prefix";
            //    prefix[1] = "Suffix";
            //    FarPoint.Web.Spread.ComboBoxCellType dropdown2 = new FarPoint.Web.Spread.ComboBoxCellType(prefix, prefix);
            //    FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].CellType = dropdown2;
            //}
            if (clgDetails == "Serial No")
            {
                string start = FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text;
                string end = FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text;
                int difference = 0, startVal = 0, endVal = 0;
                int.TryParse(start, out startVal);
                int.TryParse(end, out endVal);
                difference = endVal - startVal;
                double maxValue = Math.Pow(10, difference);
                FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].CellType = txtserialNo;
                txtserialNo.MinimumValue = 0;
                txtserialNo.MaximumValue = maxValue - 1;
                txtserialNo.ErrorMessage = "Must Enter Between 0 and " + (maxValue - 1);
                txtserialNo.CssClass = "txtbgColor";
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0BA6CB");
                darkstyle.ForeColor = Color.White;
            }
            if (checkSchoolSetting() == 0)
            {
                if (clgDetails == "College Acr" || clgDetails == "Department Acr" || clgDetails == "College Code" || clgDetails == "Year" || clgDetails == "Batch Year" || clgDetails == "Sem")
                {
                    string[] prefix = new string[2];
                    prefix[0] = "Prefix";
                    prefix[1] = "Suffix";
                    FarPoint.Web.Spread.ComboBoxCellType dropdown2 = new FarPoint.Web.Spread.ComboBoxCellType(prefix, prefix);
                    FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].CellType = dropdown2;
                }
                if (actcol == "4")
                {
                    if (btn.Text == "Quota Settings")
                    {
                        gridquota_DataBound();
                        alertpopwindow.Visible = true;
                    }
                }
            }
            else
            {
                if (clgDetails == "School Acr" || clgDetails == "Department Acr" || clgDetails == "Class Code" || clgDetails == "Year" || clgDetails == "Batch Year" || clgDetails == "Term")
                {
                    string[] prefix = new string[2];
                    prefix[0] = "Prefix";
                    prefix[1] = "Suffix";
                    FarPoint.Web.Spread.ComboBoxCellType dropdown2 = new FarPoint.Web.Spread.ComboBoxCellType(prefix, prefix);
                    FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].CellType = dropdown2;
                }
                if (actcol == "4")
                {
                    if (btn.Text == "Quota Settings")
                    {
                        gridquota_DataBound();
                        alertpopwindow.Visible = true;
                    }
                }
            }

        }

    }
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
    protected void FpSpread2_SelectedIndexChanged(Object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        DataSet data = new DataSet();
        if (gridquota.Rows.Count > 0)
        {
            for (int i = 0; i < gridquota.Rows.Count; i++)
            {
                string seattype = Convert.ToString((gridquota.Rows[i].FindControl("lbltext") as Label).Text);
                string seatcode = Convert.ToString((gridquota.Rows[i].FindControl("lblcode") as Label).Text);
                string clgcode = Convert.ToString((gridquota.Rows[i].FindControl("lblclgcode") as Label).Text);
                string priority = Convert.ToString((gridquota.Rows[i].FindControl("txt_code") as TextBox).Text);
                if (seattype.Trim() != "" && priority.Trim() != "" && priority.Trim() != "0")
                {
                    string updatequery = "update textvaltable set priority2='" + priority + "' where college_code='" + clgcode + "' and textcode='" + seatcode + "' and TextCriteria='seat'";
                    int upd = d2.update_method_wo_parameter(updatequery, "Text");
                    popup_alert.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Saved Successfully";
                }
            }
        }
        else
        {
            alertpopwindow.Visible = false;
            lblalerterr.Visible = true;
            lblerror.Text = "No Record Found";
        }
        alertpopwindow.Visible = false;
        lblalerterr.Visible = false;
    }
    //protected void chkclgacr_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (chkclgacr.Checked == true)
    //        {
    //            for (int i = 0; i < chklstclgacr.Items.Count; i++)
    //            {
    //                chklstclgacr.Items[i].Selected = true;
    //            }
    //            txtclgacr.Text = "Instution(" + (chklstclgacr.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < chklstclgacr.Items.Count; i++)
    //            {
    //                chklstclgacr.Items[i].Selected = false;
    //            }
    //            txtclgacr.Text = "---Select---";
    //        }
    //        return;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void chklstclgacr_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int commcount = 0;
    //        chkclgacr.Checked = false;
    //        txtclgacr.Text = "---Select---";
    //        for (int i = 0; i < chklstclgacr.Items.Count; i++)
    //        {
    //            if (chklstclgacr.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            txtclgacr.Text = "Instution(" + commcount.ToString() + ")";
    //            if (commcount == chklstclgacr.Items.Count)
    //            {
    //                chkclgacr.Checked = true;
    //            }
    //        }
    //        return;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    private void gridquota_DataBound()
    {
        //string collegecode = rs.GetSelectedItemsValueAsString(chklstclgacr);
        string collegecode = ddlclg.SelectedItem.Value;
        DataSet data = new DataSet();
        string query = "select distinct textcode,textval,priority2,college_code as collegecode from textvaltable where TextCriteria='seat ' and college_code in('" + collegecode + "')";
        data = d2.select_method_wo_parameter(query, "Text");
        if (data.Tables[0].Rows.Count > 0)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("textval");
            dt.Columns.Add("priority2");
            dt.Columns.Add("textcode");
            dt.Columns.Add("collegecode");
            for (int j = 0; j < data.Tables[0].Rows.Count; j++)
            {
                dr = dt.NewRow();
                dr[0] = Convert.ToString(data.Tables[0].Rows[j]["textval"]);
                dr[1] = Convert.ToString(data.Tables[0].Rows[j]["priority2"]);
                dr[2] = Convert.ToString(data.Tables[0].Rows[j]["textcode"]);
                dr[3] = Convert.ToString(data.Tables[0].Rows[j]["collegecode"]);
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                gridquota.DataSource = dt;
                gridquota.DataBind();
                gridquota.Visible = true;
            }
            else
            {
                gridquota.Visible = false;
            }
        }
        else
        {
            gridquota.Visible = false;
        }
    }
    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    public string AdmissionNoAndApplicationNumberGeneration(int NumberGenerationType = 0, string Semester = "", string DegreeCode = "", string CollegeCode = "", string BatchYear = "", string SeatType = "", string appno = null)
    {
        try
        {
            dirAccess = new InsproDirectAccess();
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            DataSet autoGenDS = new DataSet();
            string admissionNo = string.Empty;
            string GenerationNo = string.Empty;
            string GenerationNum = string.Empty;
            string query = string.Empty;
            string RunningSeries = string.Empty;
            if (NumberGenerationType == 0)
            {
                DataTable GetStudCollegeCode = dirAccess.selectDataTable("select college_code from applyn where app_no='" + appno + "'");
                if (GetStudCollegeCode.Rows.Count > 0)
                    CollegeCode = Convert.ToString(GetStudCollegeCode.Rows[0]["college_code"]);
                query = "select d.Acronym as degreeAcr,c.Coll_acronymn,a.current_semester,rtrim(ltrim(regcode))DegreeRegcode,a.degree_code,a.batch_year,c.acr,a.college_code,t.textval as SeatType,priority2 as SeatTypeNo from degree d,collinfo c,applyn a left join  textvaltable t on a.college_code=t.college_code and TextCriteria='seat ' and a.seattype=t.TextCode   where a.degree_code=d.Degree_Code and a.college_code=d.college_code and c.college_code=a.college_code and a.app_no='" + appno + "'";//a.degree_code
            }
            if (NumberGenerationType == 1)
            {
                query = "select d.Acronym as degreeAcr,c.Coll_acronymn,'" + Semester + "' current_semester,rtrim(ltrim(regcode))DegreeRegcode,'" + BatchYear + "' batch_year,c.acr'" + CollegeCode + "' college_code,t.textval as SeatType,priority2 as SeatTypeNo,a.degree_code from degree d,collinfo c left join textvaltable t on c.college_code=t.college_code and TextCriteria='seat ' where d.college_code=c.college_code and d.Degree_Code='" + DegreeCode + "' and c.college_code='" + CollegeCode + "' and t.TextCode='" + SeatType + "'";//'" + DegreeCode + "'degree_code
            }
            query += "  select NumberLength,DifferentRange,HeaderCode,MasterValue,PrefixOrSufix,StartNo,t.MasterCriteria1,NumberType,g.collegeCode,MasterCriteria1 from AdmissionNoGeneration G,CO_MasterValues t where g.HeaderCode=t.MasterCode and g.collegecode='" + CollegeCode + "' and NumberType='" + NumberGenerationType + "'  order by frange,trange";
            ds = dirAccess.selectDataSet(query);
            if (ds.Tables[1].Rows.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string CodeValue = string.Empty;
                string Code = string.Empty;
                int StartNo = 0;
                if (NumberGenerationType == 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Semester = Convert.ToString(dr["current_semester"]);
                        DegreeCode = Convert.ToString(dr["degree_code"]);
                        BatchYear = Convert.ToString(dr["batch_year"]);
                        CollegeCode = Convert.ToString(dr["college_code"]);
                        SeatType = Convert.ToString(dr["SeatTypeNo"]);
                    }
                }
                query = " select GenerationNumber, case when isnull(RunningSeries,'')='' then StartSeries+1 else RunningSeries+1 end RunningSeries,len(RunningSeries) as RunningSeriesLength  from AdmissionNoGenerationCode where NumberType='" + NumberGenerationType + "'  and BatchYear ='" + BatchYear + "' and Semester ='" + Semester + "'  and DegreeCode ='" + DegreeCode + "'  and SeatTypeNo ='" + SeatType + "'  and CollegeCode ='" + CollegeCode + "'";
                autoGenDS = dirAccess.selectDataSet(query);
                if (autoGenDS.Tables[0].Rows.Count == 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        int MasterCriteriaValue = 0;
                        int PrefixSuffix = 0;
                        int rangeDiff = 0;
                        int Length = 0;
                        string Value = string.Empty;
                        CodeValue = string.Empty;
                        int.TryParse(Convert.ToString(dr["MasterCriteria1"]), out MasterCriteriaValue);
                        int.TryParse(Convert.ToString(dr["PrefixOrSufix"]), out PrefixSuffix);
                        int.TryParse(Convert.ToString(dr["DifferentRange"]), out rangeDiff);
                        #region Code Generation
                        switch (MasterCriteriaValue)
                        {
                            case 1:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["Coll_acronymn"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                {
                                    if (PrefixSuffix == 1)
                                        CodeValue = Convert.ToString(Value.Substring(0, rangeDiff));
                                    else if (PrefixSuffix == 2)
                                        CodeValue = Convert.ToString(Value.Substring(Length - rangeDiff, rangeDiff));
                                }
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 2:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["acr"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                {
                                    if (PrefixSuffix == 1)
                                        CodeValue = Convert.ToString(Value.Substring(0, rangeDiff));
                                    else if (PrefixSuffix == 2)
                                        CodeValue = Convert.ToString(Value.Substring(Length - rangeDiff, rangeDiff));
                                }
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 3:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["SeatTypeNo"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                    CodeValue = Convert.ToString(ds.Tables[0].Rows[0]["SeatTypeNo"]);
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 4:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["degreeAcr"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                {
                                    if (PrefixSuffix == 1)
                                        CodeValue = Convert.ToString(Value.Substring(0, rangeDiff));
                                    else if (PrefixSuffix == 2)
                                        CodeValue = Convert.ToString(Value.Substring(Length - rangeDiff, rangeDiff));
                                }
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 5:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["DegreeRegcode"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                {
                                    if (PrefixSuffix == 1)
                                        CodeValue = Convert.ToString(Value.Substring(0, rangeDiff));
                                    else if (PrefixSuffix == 2)
                                        CodeValue = Convert.ToString(Value.Substring(Length - rangeDiff, rangeDiff));
                                }
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 6:
                                string year = Convert.ToString(ds.Tables[0].Rows[0]["current_semester"]);
                                CodeValue = returnYearforSem(year);
                                break;
                            case 7:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                {
                                    if (PrefixSuffix == 1)
                                        CodeValue = Convert.ToString(Value.Substring(0, rangeDiff));
                                    else if (PrefixSuffix == 2)
                                        CodeValue = Convert.ToString(Value.Substring(Length - rangeDiff, rangeDiff));
                                }
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 8:
                                CodeValue = Convert.ToString(ds.Tables[0].Rows[0]["current_semester"]);
                                break;
                            case 9:
                                Code = admissionNo;
                                int.TryParse(Convert.ToString(dr["StartNo"]), out StartNo);
                                //RunningSeries = StartNo.ToString().PadLeft(rangeDiff+1, '0');
                                RunningSeries = generateApplicationNumber(StartNo, rangeDiff);
                                //RunningSeries = StartNo; //+ 1;
                                CodeValue = RunningSeries;
                                break;
                        }
                        #endregion
                        admissionNo += CodeValue;
                        GenerationNo += Code;
                    }
                }
                else
                {
                    RunningSeries = Convert.ToString(autoGenDS.Tables[0].Rows[0]["RunningSeries"]);
                    GenerationNum = Convert.ToString(autoGenDS.Tables[0].Rows[0]["GenerationNumber"]);
                    int Runlen = 0;
                    int RunningSerial = 0;
                    int.TryParse(Convert.ToString(autoGenDS.Tables[0].Rows[0]["RunningSeriesLength"]), out Runlen);
                    int.TryParse(RunningSeries, out RunningSerial);
                    RunningSeries = generateApplicationNumber(RunningSerial, Runlen);
                    admissionNo = Convert.ToString(GenerationNum + RunningSeries);
                }
                query = "if exists(select BatchYear from AdmissionNoGenerationCode where NumberType='" + NumberGenerationType + "' and BatchYear ='" + BatchYear + "' and Semester ='" + Semester + "'  and DegreeCode ='" + DegreeCode + "'  and SeatTypeNo ='" + SeatType + "'  and CollegeCode ='" + CollegeCode + "')update AdmissionNoGenerationCode set RunningSeries='" + RunningSeries + "' where NumberType='" + NumberGenerationType + "' and BatchYear ='" + BatchYear + "' and Semester ='" + Semester + "'  and DegreeCode ='" + DegreeCode + "'  and SeatTypeNo ='" + SeatType + "'  and CollegeCode ='" + CollegeCode + "' else insert into AdmissionNoGenerationCode(NumberType,BatchYear ,Semester ,DegreeCode ,SeatTypeNo ,CollegeCode ,StartSeries ,RunningSeries,GenerationNumber)values('" + NumberGenerationType + "','" + BatchYear + "','" + Semester + "','" + DegreeCode + "','" + SeatType + "','" + CollegeCode + "','" + StartNo + "','" + RunningSeries + "','" + GenerationNo + "')";
                dirAccess.updateData(query);
            }
            return admissionNo;
        }
        catch (Exception e)
        {
            d2.sendErrorMail(e, Convert.ToString(CollegeCode), "AdmissionNumber Genderation");
            return " ";
        }
    }
    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
    }

    public void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        gridquota_DataBound();
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        popup_alert.Visible = false;
    }
}