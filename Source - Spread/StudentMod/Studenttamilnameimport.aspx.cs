using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class StudentMod_Studenttamilnameimport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    string q1 = string.Empty;
    int i = 0;
    string error1 = string.Empty;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    
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
                setLabelText();
                bindcollege();
                txt_startyear.Text = Convert.ToString(System.DateTime.Now.ToString("yyyy"));
                txt_endyear.Text = Convert.ToString(System.DateTime.Now.ToString("yyyy"));
            }
        }
        catch (Exception ex) { }
    }

    public void bindcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollegebaseonrights(Session["usercode"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = Convert.ToString(ex);
        }
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lbl_degree);
        lbl.Add(lbl_branch);
        lbl.Add(lbl_sem);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_startyear.Text.Trim() != "" && txt_endyear.Text.Trim() != "")
            {
                string startyear = txt_startyear.Text;
                string endyear = txt_endyear.Text;
                int txtstartyearmark = Convert.ToInt32(txt_startyear.Text);
                int txtendyearmark = Convert.ToInt32(txt_endyear.Text);

                if (Convert.ToInt32(txt_startyear.Text) <= Convert.ToInt32(txt_endyear.Text))
                {
                    string rights = string.Empty;
                    string usercode = Session["usercode"].ToString();
                    string singleuser = Session["single_user"].ToString();
                    string group_user = Session["group_code"].ToString();
                    if (group_user.Contains(";"))
                    {
                        string[] group_semi = group_user.Split(';');
                        group_user = group_semi[0].ToString();
                    }
                    if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
                    {
                        rights = "and group_code='" + group_user + "'";
                    }
                    else
                    {
                        rights = " and user_code='" + usercode + "'";
                    }

                    string getquery = " select c.college_code,c.Edu_Level,c.Course_Name,de.Dept_Name,de.dept_acronym,d.Course_Id,d.Dept_Code,d.Degree_Code from Degree d,course c,Department de, DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and c.college_code=d.college_code and d.college_code=de.college_code and c.college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' " + rights + " order by c.college_code asc,c.Priority asc,d.Degree_Code asc ";

                    getquery += " SELECT  (SELECT TOP 1 batch_year FROM tbl_attendance_rights where batch_year between '" + startyear + "' and '" + endyear + "' ORDER BY batch_year asc) startyear, (SELECT TOP 1 batch_year as s FROM tbl_attendance_rights where batch_year between '" + startyear + "' and '" + endyear + "' ORDER BY batch_year DESC)as endyear";


                    ds.Clear();
                    ds = d2.select_method_wo_parameter(getquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            string S = Convert.ToString(ds.Tables[1].Rows[0]["startyear"]);
                            string E = Convert.ToString(ds.Tables[1].Rows[0]["endyear"]);
                            if (S.Trim() != "")
                            {
                                txtstartyearmark = Convert.ToInt32(S);
                            }
                            if (E.Trim() != "")
                            {
                                txtendyearmark = Convert.ToInt32(E);
                            }
                            if (txtstartyearmark != 0 && txtendyearmark != 0)
                            {
                                string edulevel = string.Empty;
                                string degree = string.Empty;
                                string dept = string.Empty;
                                string depyacronym = string.Empty;
                                string degreecode = string.Empty;
                                string semester = string.Empty;
                                DataTable dt = new DataTable();
                                dt.Columns.Add("S.No");
                                dt.Columns.Add("BatchYear");
                                dt.Columns.Add("EduLevel");
                                dt.Columns.Add("Degree");
                                dt.Columns.Add("Department");
                                dt.Columns.Add("degreecode");

                                for (int j = txtstartyearmark; j <= txtendyearmark; j++)
                                {
                                    string degcheck = string.Empty;
                                    int spancount1 = 0;
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        edulevel = Convert.ToString(ds.Tables[0].Rows[i]["Edu_Level"]);
                                        degree = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                                        dept = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                        depyacronym = Convert.ToString(ds.Tables[0].Rows[i]["dept_acronym"]);
                                        degreecode = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);
                                        if (degreecode == degcheck)
                                        {
                                            ViewState["rowcount"] = spancount1 + 1;
                                        }
                                        else
                                        {
                                            spancount1 = 1;
                                            ViewState["rowcount"] = spancount1 + 1;
                                            degcheck = degreecode;
                                        }
                                        DataRow dr;
                                        dr = dt.NewRow();
                                        dr[1] = j;
                                        dr[2] = edulevel;
                                        dr[3] = degree;
                                        dr[4] = depyacronym;
                                        dr[5] = degreecode;
                                        dt.Rows.Add(dr);
                                    }
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    importgrid.DataSource = dt;
                                    importgrid.DataBind();
                                    importgrid.Visible = true;
                                    lbl_error.Visible = false;
                                }
                                importgrid.Visible = true;
                            }
                            else
                            {
                                importgrid.Visible = false;
                                lbl_alerterror.Text = " Please Set Batch Year Rights ";
                                alertmessage.Visible = true;
                            }
                        }
                        else
                        {
                            importgrid.Visible = false;
                            lbl_alerterror.Text = " Please Set Batch Year Rights ";
                            alertmessage.Visible = true;
                        }
                    }
                    else
                    {
                        importgrid.Visible = false;
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "No Record(s) Found.";
                        alertmessage.Visible = true;
                    }
                }
            }
            else
            {
                lbl_error.Text = "Please Enter Start Year and End Year";
                lbl_error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = Convert.ToString(ex);
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        Browsefile_div.Visible = false;
    }

    public void btn_Exit_Click1(object sender, EventArgs e)
    {
        cannot_insert_div.Visible = false;
    }

    protected void btn_upload_click(object sender, EventArgs e)
    {
        try
        {
            string error = ""; bool check = false;
            using (Stream stream = this.FileUpload1.FileContent as Stream)
            {
                string extension = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (extension.Trim() != "")
                {
                    string moduletype = Convert.ToString(ViewState["moduletype"]);
                    string path = Server.MapPath("~/Importfiles/" + System.IO.Path.GetFileName(FileUpload1.FileName));
                    FileUpload1.SaveAs(path);
                    ds1.Clear();
                    ds1 = Excelconvertdataset(path); string tamilname = ""; bool errormessage = false; string errorformat = "";
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (System.IO.Path.GetExtension(FileUpload1.FileName) == ".xls")
                        {
                            if (Convert.ToString(ViewState["degreecode"]) != "" && Convert.ToString(ViewState["degreecode"]) != null && Convert.ToString(ViewState["batchyear"]) != "" && Convert.ToString(ViewState["batchyear"]) != null)
                            {
                                q1 = " select App_No,Reg_No from Registration where degree_code='" + Convert.ToString(ViewState["degreecode"]) + "' and Batch_Year='" + Convert.ToString(ViewState["batchyear"]) + "' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "' ";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(q1, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (i = 0; i < ds1.Tables[0].Rows.Count; i++)
                                    {
                                        error1 = "";
                                        if (Convert.ToString(ds1.Tables[0].Rows[i]["Reg No"]).Trim() != "" && Convert.ToString(ds1.Tables[0].Rows[i]["TamilName"]).Trim() != "")
                                        {
                                            ds.Tables[0].DefaultView.RowFilter = "Reg_No='" + Convert.ToString(ds1.Tables[0].Rows[i]["Reg No"]).Trim() + "'";
                                            DataView dv1 = ds.Tables[0].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string App_No = Convert.ToString(dv1[0]["App_No"]);
                                                tamilname = Convert.ToString(ds1.Tables[0].Rows[i]["TamilName"]).Trim();
                                                tamilname = tamilname.Replace("'", "''");
                                                if (App_No.Trim() != "" && App_No.Trim() != "0")
                                                {
                                                    string sql = "update applyn set stud_nametamil=N'" + tamilname + "' where App_No='" + App_No + "'";
                                                    int a = d2.update_method_wo_parameter(sql, "Text");
                                                    if (a != 0)
                                                    {
                                                        check = true;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {

                                            if (Convert.ToString(ds1.Tables[0].Rows[i]["Reg No"]).Trim() == "")
                                            {
                                                errorformat = " Reg no is Empty";
                                            }
                                            if (Convert.ToString(ds1.Tables[0].Rows[i]["TamilName"]).Trim() == "")
                                            {
                                                errorformat = " Tamilname is Empty";
                                            }
                                            if (errorformat.Trim() != "")
                                            {
                                                error1 = "Row no(" + (Convert.ToInt32(i) + Convert.ToInt32(1)) + ") " + errorformat + "\r\n";
                                            }
                                            if (error1.Trim() != "")
                                            {
                                                if (error == "")
                                                {
                                                    error = error1;
                                                }
                                                else
                                                {
                                                    error = error + error1;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Browsefile_div.Visible = false;
                                    lbl_alerterror.Visible = true;
                                    lbl_alerterror.Text = "Please Check the Reg No";
                                    alertmessage.Visible = true;
                                }
                            }
                            else
                            {
                                Browsefile_div.Visible = false;
                                lbl_alerterror.Visible = true;
                                lbl_alerterror.Text = "Please Click Correct " + lbl_degree.Text + " ";
                                alertmessage.Visible = true;
                            }
                        }
                        else
                        {
                            Browsefile_div.Visible = false;
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "Please Import Only xls Format";
                            alertmessage.Visible = true;
                        }
                    }
                    else
                    {
                        Browsefile_div.Visible = true;
                        pnl2.Visible = true;
                        alertmessage.Visible = true;
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Excel does not having any data.";
                    }
                }
                if (check == true)
                {
                    if (error.Trim() != "")
                    {
                        Browsefile_div.Visible = false;
                        cannot_insert_div.Visible = true;
                        lbl_cannotsave.Visible = true;
                        lbl_cannotsave.Text = "Should Not Save Row No";
                        lbl_cannotinsert.Visible = true;
                        lbl_cannotinsert.Text = error;
                        pnl2.Visible = true;
                        alertmessage.Visible = true;
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Uploaded Successfully";
                    }
                    else
                    {
                        Browsefile_div.Visible = false;
                        pnl2.Visible = true;
                        alertmessage.Visible = true;
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Uploaded Successfully";
                    }
                }
                else
                {
                    if (error.Trim() != "")
                    {
                        cannot_insert_div.Visible = true;
                        lbl_cannotsave.Visible = true;
                        lbl_cannotsave.Text = "Should Not Save Row No ";
                        lbl_cannotinsert.Visible = true;
                        lbl_cannotinsert.Text = error;
                        Browsefile_div.Visible = false;
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void btn_download_click(object sender, EventArgs e)
    {

    }

    protected void importgrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int row = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Download")
            {
                string degreecode = ((importgrid.Rows[row].FindControl("lbl_degreecode") as Label).Text);
                string batchyear = ((importgrid.Rows[row].FindControl("lblbtch") as Label).Text);

                if (degreecode.Trim() != "0" && degreecode.Trim() != "" && batchyear.Trim() != "" && batchyear.Trim() != "0")
                {
                    string xlname = "studenttamilnameimport";
                    DataTable gendratetable = new DataTable();

                    q1 = " select Reg_No as [Reg No],Stud_Name as [Student Name],'' TamilName from Registration where degree_code='" + degreecode + "' and Batch_Year='" + batchyear + "' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'  ";
                    //q1 = " select Reg_No as [Reg No],r.Stud_Name as [Student Name],stud_nametamil as [TamilName] from Registration r,applyn a where r.App_No=a.app_no and r.degree_code='" + degreecode + "' and r.Batch_Year='" + batchyear + "' and r.college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'   ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "Text");
                    if (ds.Tables.Count > 0)
                    {
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    gendratetable = ds.Tables[0];
                        //}
                        gendratetable = ds.Tables[0];
                    }
                    xlname = "Update_" + xlname;
                    ExportTable1(gendratetable, xlname);
                }
            }
            else if (e.CommandName == "Upload")
            {
                lbl_alert.Visible = false;
                ViewState["degreecode"] = null;
                ViewState["batchyear"] = null;

                ViewState["degreecode"] = ((importgrid.Rows[row].FindControl("lbl_degreecode") as Label).Text);
                ViewState["batchyear"] = ((importgrid.Rows[row].FindControl("lblbtch") as Label).Text);
                Browsefile_div.Visible = true;

            }
            else if (e.CommandName == "Help")
            {

            }
        }
        catch { }
    }

    public static DataSet Excelconvertdataset(string path)
    {
        DataSet ds3 = new DataSet();
        string StrSheetName = "";

        string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';";
        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
        try
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            if (excelConnection.State == ConnectionState.Closed)
                excelConnection.Open();
            DataTable dtSheets = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dtSheets != null && dtSheets.Rows.Count > 0)
            {
                StrSheetName = dtSheets.Rows[0].ItemArray[2].ToString();

            }
            if (!string.IsNullOrEmpty(StrSheetName))
            {
                OleDbCommand cmd = new OleDbCommand("Select * from [" + StrSheetName + "]", excelConnection);
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(ds3, "excelData");
            }
        }
        catch
        {
        }
        finally
        {
            if (excelConnection.State != ConnectionState.Closed)
                excelConnection.Close();
        }
        return ds3;
    }

    private void ExportTable1(DataTable dtt, string filename)
    {
        Response.ClearContent();
        Response.Buffer = true; string staffname = "";
        string headername = Convert.ToString(filename.Trim() + ".xls");
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", headername));
        Response.ContentType = "application/ms-excel";
        DataTable dt = dtt;
        string str = string.Empty;
        foreach (DataColumn dtcol in dt.Columns)
        {
            Response.Write(str + dtcol.ColumnName);
            str = "\t";
        }
        Response.Write("\n"); str = "";
        foreach (DataRow dr in dt.Rows)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                staffname = Convert.ToString(dr[j]).Trim();
                Response.Write(str + staffname);
                str = "\t";
            }
            str = "\r\n";
        }
        System.Web.HttpContext.Current.Response.Flush();
        Response.End();
    }

    protected void importgrid_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[3].Text = lbl_degree.Text;
            e.Row.Cells[4].Text = lbl_branch.Text;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.importgrid, "Download$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.importgrid, "Upload$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.importgrid, "Help$" + e.Row.RowIndex);
        }
    }

    protected void importgrid_DataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = importgrid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = importgrid.Rows[i];
                GridViewRow previousRow = importgrid.Rows[i - 1];
                for (int j = 1; j <= 4; j++)
                {
                    Label lnlname = (Label)row.FindControl("lbl_degreecode");
                    Label lnlname1 = (Label)previousRow.FindControl("lbl_degreecode");
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += Convert.ToInt32(ViewState["rowcount"]);
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

}