using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Drawing;


public partial class manpower_Alter : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    static string clgcode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable ht = new Hashtable();
    Hashtable hat = new Hashtable();
    bool check = false;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            clgcode = collegecode1;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                //fairpoint();
                binddepartment();
                binddesignation();
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                btn_go_Click(sender, e);
            }
            lbl_norec.Visible = false;
            Fpspread1.SaveChanges();
        }
        catch (Exception ex) { }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        string collcode = Convert.ToString(ddlclg.SelectedItem.Value);
        try
        {
            int count = 0;
            for (int i = 0; i < cbl_dptname.Items.Count; i++)
            {
                if (cbl_dptname.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                lbl_alert.Text = "Please Select Atleast One Department";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                hide();
                return;
            }
            string deptcod = "";
            if (cbl_dptname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_dptname.Items.Count; i++)
                {
                    if (cbl_dptname.Items[i].Selected == true)
                    {
                        if (deptcod == "")
                        {
                            deptcod = Convert.ToString(cbl_dptname.Items[i].Value);
                        }
                        else
                        {
                            deptcod = deptcod + "','" + Convert.ToString(cbl_dptname.Items[i].Value);
                        }
                    }
                }
            }

            count = 0;
            string sql = "";
            DataView dvfil = new DataView();
            Fpspread1.Sheets[0].Rows.Count = 0;
            Fpspread1.Sheets[0].Columns.Count = 6;
            Fpspread1.Height = 340;
            Fpspread1.Width = 800;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Fpspread1.Sheets[0].ColumnHeader.Columns[0].Width = 75;
            Fpspread1.Sheets[0].ColumnHeader.Columns[1].Width = 310;
            Fpspread1.Sheets[0].ColumnHeader.Columns[2].Width = 100;
            Fpspread1.Sheets[0].ColumnHeader.Columns[3].Width = 100;
            Fpspread1.Sheets[0].ColumnHeader.Columns[4].Width = 100;
            Fpspread1.Sheets[0].ColumnHeader.Columns[5].Width = 100;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Designation";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Available Staff";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "No.of Req Staff";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "No.of Vacancy";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "No.of Additional Req Staff";

            FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
            intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            intgrcel.MinimumValue = 0;
            intgrcel.ErrorMessage = "Enter valid Required Staff";
            string desigcod = d2.GetFunction(" Select desig_code from desig_master where desig_name='" + txt_search.Text.ToString() + "' and collegeCode='" + collcode + "'");
            string selquery = "";

            selquery = "select t.desig_code,t.dept_code,count(s.staff_code)as count from staffmaster s,stafftrans  t where s.staff_code =t.staff_code and t.latestrec ='1' and resign=0 and settled =0 and isnull(Discontinue,'0') ='0'  and s.college_code ='" + collcode + "' group by t.desig_code,t.dept_code";
            selquery = selquery + " select DeptCode,DesigCode,No_ofPersons from VacancyMaster where Collegecode ='" + collcode + "'";
            DataSet dsnew = new DataSet();
            dsnew.Clear();
            dsnew = d2.select_method_wo_parameter(selquery, "Text");
            int sno = 1;
            int rowcount = 0;
            if (cbl_dptname.Items.Count > 0 && txt_dptname.Text.Trim() != "--Select--")
            {
                for (int st = 0; st < cbl_dptname.Items.Count; st++)
                {
                    if (cbl_dptname.Items[st].Selected == true)
                    {
                        rowcount = 0;
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cbl_dptname.Items[st].Text);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 6);

                        string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbl_dptname.Items[st].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbl_dptname.Items[st].Value) + "%') or (dept_code like '%" + Convert.ToString(cbl_dptname.Items[st].Value) + "') or (dept_code='" + Convert.ToString(cbl_dptname.Items[st].Value) + "'))";

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                            {
                                if (txt_search.Text.Trim() != "")
                                {
                                    if (Convert.ToString(ds.Tables[0].Rows[ik]["desig_code"]) == desigcod)
                                    {
                                        rowcount++;
                                        Fpspread1.Sheets[0].RowCount++;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(txt_search.Text.Trim());
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(desigcod);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(cbl_dptname.Items[st].Value);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                        if (dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                                        {
                                            DataView dvnew = new DataView();
                                            dsnew.Tables[0].DefaultView.RowFilter = " dept_code='" + Convert.ToString(cbl_dptname.Items[st].Value) + "' and desig_code='" + desigcod + "'";
                                            dvnew = dsnew.Tables[0].DefaultView;
                                            if (dvnew.Count > 0)
                                            {
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvnew[0]["count"]);
                                            }
                                            else
                                            {
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = "-";
                                            }
                                        }
                                        else
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = "-";
                                        }
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                        if (dsnew.Tables.Count > 0 && dsnew.Tables[1].Rows.Count > 0)
                                        {
                                            DataView dvmynew = new DataView();
                                            dsnew.Tables[1].DefaultView.RowFilter = " DeptCode='" + Convert.ToString(cbl_dptname.Items[st].Value) + "' and DesigCode='" + desigcod + "'";
                                            dvmynew = dsnew.Tables[1].DefaultView;
                                            if (dvmynew.Count > 0)
                                            {
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvmynew[0]["No_ofPersons"]);
                                            }
                                            else
                                            {
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = "-";
                                            }
                                        }
                                        else
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = "-";
                                        }
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                        Double availcount = 0;
                                        Double noofreq = 0;
                                        Double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text), out availcount);
                                        Double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text), out noofreq);

                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(noofreq - availcount);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].CellType = intgrcel;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                    }
                                }
                                else
                                {
                                    for (int jk = 0; jk < cbl_desname.Items.Count; jk++)
                                    {
                                        if (cbl_desname.Items[jk].Selected == true)
                                        {
                                            if (Convert.ToString(ds.Tables[0].Rows[ik]["desig_code"]) == Convert.ToString(cbl_desname.Items[jk].Value))
                                            {
                                                rowcount++;
                                                Fpspread1.Sheets[0].RowCount++;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cbl_desname.Items[jk].Text);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(cbl_desname.Items[jk].Value);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(cbl_dptname.Items[st].Value);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                if (dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                                                {
                                                    DataView dvnew = new DataView();
                                                    dsnew.Tables[0].DefaultView.RowFilter = " dept_code='" + Convert.ToString(cbl_dptname.Items[st].Value) + "' and desig_code='" + Convert.ToString(cbl_desname.Items[jk].Value) + "'";
                                                    dvnew = dsnew.Tables[0].DefaultView;
                                                    if (dvnew.Count > 0)
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvnew[0]["count"]);
                                                    }
                                                    else
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = "-";
                                                    }
                                                }
                                                else
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = "-";
                                                }
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                                if (dsnew.Tables.Count > 0 && dsnew.Tables[1].Rows.Count > 0)
                                                {
                                                    DataView dvmynew = new DataView();
                                                    dsnew.Tables[1].DefaultView.RowFilter = " DeptCode='" + Convert.ToString(cbl_dptname.Items[st].Value) + "' and DesigCode='" + Convert.ToString(cbl_desname.Items[jk].Value) + "'";
                                                    dvmynew = dsnew.Tables[1].DefaultView;
                                                    if (dvmynew.Count > 0)
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvmynew[0]["No_ofPersons"]);
                                                    }
                                                    else
                                                    {
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = "-";
                                                    }
                                                }
                                                else
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = "-";
                                                }
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                Double availcount = 0;
                                                Double noofreq = 0;
                                                Double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text), out availcount);
                                                Double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text), out noofreq);

                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(noofreq - availcount);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].CellType = intgrcel;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                            }
                                        }
                                    }
                                }
                            }
                            if (rowcount == 0)
                            {
                                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Remove();
                            }
                        }
                        else
                        {
                            Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Remove();
                        }
                    }
                }
            }

            for (int ii = 0; ii < Fpspread1.Sheets[0].Columns.Count; ii++)
            {
                Fpspread1.Sheets[0].ColumnHeader.Columns[ii].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Columns[ii].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Columns[ii].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Columns[ii].Font.Size = FontUnit.Medium;
            }
            for (int kl = 0; kl < Fpspread1.Sheets[0].ColumnCount - 1; kl++)
            {
                Fpspread1.Sheets[0].Columns[kl].Locked = true;
            }
            if (Fpspread1.Sheets[0].RowCount > 1)
            {
                Fpspread1.Visible = true;
                rptprint.Visible = true;
                div1.Visible = true;

                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.SaveChanges();
                txt_search.Text = "";
            }
            else
            {
                txt_search.Text = "";
                lbl_alert.Text = "No Records Found";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                hide();
            }
            //}
            //else
            //{
            //    txt_search.Text = "";
            //    lbl_alert.Text = "No Records Found";
            //    lbl_alert.Visible = true;
            //    imgdiv2.Visible = true;
            //    hide();
            //}
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "manpower_Alter.aspx");
        }
    }

    public void bindcollege()
    {
        string selqry = "select collname,college_code from collinfo";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selqry, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlclg.DataSource = ds;
            ddlclg.DataTextField = "collname";
            ddlclg.DataValueField = "college_code";
            ddlclg.DataBind();
        }
    }

    public void binddepartment()
    {
        cbl_dptname.Items.Clear();
        string collcode = Convert.ToString(ddlclg.SelectedValue);
        string selqry = "select Dept_Code,Dept_Name FROM hrdept_master where college_code='" + collcode + "' order by Dept_Name";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selqry, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_dptname.DataSource = ds;
            cbl_dptname.DataTextField = "Dept_Name";
            cbl_dptname.DataValueField = "Dept_Code";
            cbl_dptname.DataBind();

            if (cbl_dptname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_dptname.Items.Count; i++)
                {
                    cbl_dptname.Items[i].Selected = true;
                }
                txt_dptname.Text = "Department Name(" + cbl_dptname.Items.Count + ")";
                cb_dptname.Checked = true;
            }
        }
        else
        {
            txt_dptname.Text = "--Select--";
            cb_dptname.Checked = false;
        }
        binddesignation();
    }

    public void binddesignation()
    {
        cbl_desname.Items.Clear();
        Dictionary<string, string> dicgetcode = new Dictionary<string, string>();
        dicgetcode.Clear();
        Dictionary<string, string> dicdescode = new Dictionary<string, string>();
        dicdescode.Clear();
        string collcode = Convert.ToString(ddlclg.SelectedValue);
        if (cbl_dptname.Items.Count > 0)
        {
            for (int ik = 0; ik < cbl_dptname.Items.Count; ik++)
            {
                if (cbl_dptname.Items[ik].Selected == true)
                {
                    if (!dicgetcode.ContainsKey(Convert.ToString(cbl_dptname.Items[ik].Value)))
                    {
                        string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbl_dptname.Items[ik].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbl_dptname.Items[ik].Value) + "%') or (dept_code like '%" + Convert.ToString(cbl_dptname.Items[ik].Value) + "') or (dept_code='" + Convert.ToString(cbl_dptname.Items[ik].Value) + "'))";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int jk = 0; jk < ds.Tables[0].Rows.Count; jk++)
                            {
                                if (!dicdescode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])))
                                {
                                    cbl_desname.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])));
                                    dicdescode.Add(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]));
                                }
                            }
                        }
                        dicgetcode.Add(Convert.ToString(cbl_dptname.Items[ik].Value), Convert.ToString(cbl_dptname.Items[ik].Text));
                    }
                }
            }
        }

        if (cbl_desname.Items.Count > 0)
        {
            for (int i = 0; i < cbl_desname.Items.Count; i++)
            {
                cbl_desname.Items[i].Selected = true;
            }
            txt_desname.Text = "Designation Name(" + cbl_desname.Items.Count + ")";
            cb_desname.Checked = true;
        }
        else
        {
            txt_desname.Text = "--Select--";
            cb_desname.Checked = false;
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select desig_name from desig_master WHERE desig_name like '" + prefixText + "%' and collegeCode='" + clgcode + "'";
        name = ws.Getname(query);
        return name;
    }

    protected void ddldptnameonselected(object sender, EventArgs e)
    {

    }

    protected void cb_dptname_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_dptname, cbl_dptname, txt_dptname, "Department Name");
        binddesignation();
    }

    protected void cbl_dptname_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_dptname, cbl_dptname, txt_dptname, "Department Name");
        binddesignation();
    }

    protected void cb_desname_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_desname, cbl_desname, txt_desname, "Designation Name");
    }

    protected void cbl_desname_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_desname, cbl_desname, txt_desname, "Designation Name");
    }

    protected void ddlclgonselected(object sender, EventArgs e)
    {
        clgcode = Convert.ToString(ddlclg.SelectedValue);
        binddepartment();
        binddesignation();
        btn_go_Click(sender, e);
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        string dptname = "Man power";
        string pagename = "manpower_Alter.aspx";
        Printcontrol.loadspreaddetails(Fpspread1, pagename, dptname);
        Printcontrol.Visible = true;
    }

    public void btnsave_Click(object sender, EventArgs e)
    {
        string collcode = Convert.ToString(ddlclg.SelectedValue);
        try
        {
            string sqldeptcode = "";
            int totalpersons = 0;
            int save = 0;
            Fpspread1.SaveChanges();
            for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
            {
                if (Fpspread1.Sheets[0].Cells[i, 0].Text.Trim() != "")
                {
                    string ReqStaff = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 5].Text);
                    if (ReqStaff.Trim() != "")
                    {
                        string sqldesigncode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 1].Tag);
                        sqldeptcode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 1].Note);
                        totalpersons = Convert.ToInt32(ReqStaff);

                        string noofpersons = d2.GetFunction("select No_ofPersons from VacancyMaster where DeptCode='" + sqldeptcode + "' and DesigCode='" + sqldesigncode + "' and Collegecode='" + collcode + "'");

                        int persons = 0;
                        if (Int32.TryParse(noofpersons, out persons))
                        {
                            persons = Convert.ToInt32(noofpersons);
                        }
                        else
                        {
                            persons = 0;
                        }
                        totalpersons = totalpersons + persons;

                        string insertempprsql = "if exists (select * from VacancyMaster where DeptCode='" + sqldeptcode + "' and DesigCode='" + sqldesigncode + "' and Collegecode='" + collcode + "') update VacancyMaster set No_ofPersons='" + totalpersons + "' where DeptCode='" + sqldeptcode + "' and DesigCode='" + sqldesigncode + "' and Collegecode='" + collcode + "' else INSERT INTO VacancyMaster (DeptCode,DesigCode,No_ofPersons,Collegecode) VALUES('" + sqldeptcode + "','" + sqldesigncode + "','" + totalpersons + "','" + collcode + "')";
                        d2.insert_method(insertempprsql, ht, "Text");
                        save++;
                    }
                    else
                    {
                        Fpspread1.SaveChanges();
                        lbl_alert.Text = "Please Add Required Staff";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                    }
                }
            }
            if (save > 0)
            {
                lbl_alert.Text = "Saved  Successfully";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
            }
            btn_go_Click(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "manpower_Alter.aspx");
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void hide()
    {
        lbl_norec.Visible = false;
        Printcontrol.Visible = false;
        div1.Visible = false;
        rptprint.Visible = false;
    }

    public void show()
    {
        div1.Visible = true;
        rptprint.Visible = true;
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                {
                    chkchange.Checked = true;
                }
            }
        }
        catch { }
    }
}