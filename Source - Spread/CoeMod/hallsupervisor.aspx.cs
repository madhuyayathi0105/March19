using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class hallsupervisor : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable ht = new Hashtable();
    Boolean cellclick = false;

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
            lblmessage1.Visible = false;
            DropDownList2.Visible = false;
            if (!IsPostBack)
            {
                btndelete.Enabled = false;
                lblerror.Visible = false;
                lblerror1.Visible = false;
                fpcammarkstaff.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                //year();
                //month();
                //month1();
                ddltypeadd.Items.Clear();
                ddltypeview.Items.Clear();
                string strtypequery = "select distinct type from course order by type";
                DataSet dstype = da.select_method_wo_parameter(strtypequery, "text");
                if (dstype.Tables.Count > 0 && dstype.Tables[0].Rows.Count > 0)
                {
                    ddltypeadd.DataSource = dstype;
                    ddltypeadd.DataTextField = "type";
                    ddltypeadd.DataBind();

                    ddltypeview.DataSource = dstype;
                    ddltypeview.DataTextField = "type";
                    ddltypeview.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddltypeadd_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddltypeview_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnnew_click(object sender, EventArgs e)
    {
        try
        {
            ddlfromexp.Enabled = true;
            ddltoexp.Enabled = true;
            ddltypeadd.Enabled = true;
            lblerror.Visible = false;
            AddPageModify.Text = "Add";
            ddlfromexp.SelectedIndex = 0;
            ddltoexp.SelectedIndex = 0;
            txtSupervisor.Text = string.Empty;
            ddlsession.SelectedIndex = 0;
            ddltypeadd.SelectedIndex = 0;
            btnsave.Text = "Save";
            lblerror1.Visible = false;
            btndelete.Enabled = false;
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        spread1();
    }

    protected void btnsave_click(object sender, EventArgs e)
    {
        string session = string.Empty;
        if (ddlsession.Text == "0")
        {
            session = "F.N";
        }
        else if (ddlsession.Text == "1")
        {
            session = "A.N";
        }
        else if (ddlsession.Text == "2")
        {
            session = "F.N/A.N";
        }
        if (txtSupervisor.Text != "")
        {
            if ( Convert.ToInt32(ddlfromexp.Text) <= Convert.ToInt32(ddltoexp.Text) )
            {
                string typval = string.Empty;
                string strtype = string.Empty;
                string typeval = string.Empty;
                if (ddltypeadd.Enabled == true && ddltypeadd.Items.Count > 0)
                {
                    typval = " and type='" + ddltypeadd.SelectedItem.ToString() + "'";
                    strtype = "Type";
                    typeval = ddltypeadd.SelectedItem.ToString();
                }
                if (btnsave.Text == "Save")
                {
                    //string sqlquery = "select * from hallsupervision where month=" + ddlmonth.SelectedValue + " and type=" + ddltypeadd.SelectedValue + " and((expfrom<=" + ddlfromexp.SelectedValue + " and  expto>=" + ddlfromexp.SelectedValue + " )or (expfrom<=" + ddltoexp.SelectedValue + " and expto>=" + ddltoexp.SelectedValue + ")) and  ( session='" + session + "') and month='" + ddlmonth.SelectedValue + "' and type='" + ddltypeadd.SelectedValue + "'";
                    string sqlquery = "select * from hallsupervision where ((expfrom<=" + ddlfromexp.SelectedValue + " and  expto>=" + ddlfromexp.SelectedValue + " )or (expfrom<=" + ddltoexp.SelectedValue + " and expto>=" + ddltoexp.SelectedValue + ")) and  ( session='" + session + "') " + typval + "";
                    ds = da.select_method_wo_parameter(sqlquery, "text");
                }
                else if (btnsave.Text == "Update")
                {
                    ds.Tables.Add("0");
                    ds.Tables[0].Columns.Add("aasdasd", typeof(string));
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Already Exp From And To Has Been Selected";
                }
                else
                {
                    int save = 0;
                    //if (btnsave.Text == "Save")
                    //{
                    //string sqlquery1 = "if exists(select * from hallsupervision where expfrom=" + ddlfromexp.Text + " and expto=" + ddltoexp.Text + " and max_superivison='" + txtSupervisor.Text + "' and session='" + session + "' and month='" + ddlmonth.Text + "' and type='" + ddltypeadd.SelectedItem + "' ) update hallsupervision set max_superivison='" + txtSupervisor.Text + "',session='" + session + "',month='" + ddlmonth.Text + "',year='" + ddltypeadd.SelectedItem + "' where expfrom=" + ddlfromexp.Text + " and expto=" + ddltoexp.Text + " else insert into hallsupervision (expfrom,expto,max_superivison,session,month,year) values('" + ddlfromexp.Text + "','" + ddltoexp.Text + "','" + txtSupervisor.Text + "','" + session + "','" + ddlmonth.Text + "','" + ddltypeadd.SelectedItem + "')";
                    string sqlquery1 = "if exists(select * from hallsupervision where expfrom=" + ddlfromexp.Text + " and expto=" + ddltoexp.Text + " and session='" + session + "' " + typval + " ) update hallsupervision set max_superivison='" + txtSupervisor.Text + "',session='" + session + "' where expfrom=" + ddlfromexp.Text + " and expto=" + ddltoexp.Text + " " + typval + " else insert into hallsupervision (expfrom,expto,max_superivison,session,Type) values('" + ddlfromexp.Text + "','" + ddltoexp.Text + "','" + txtSupervisor.Text + "','" + session + "','" + typeval + "')";
                    save = da.insert_method(sqlquery1, ht, "Text");
                    //}
                    //else if (btnsave.Text == "Update")
                    //{
                    //    string sqlquery1 = "if exists(select * from hallsupervision where expfrom=" + ddlfromexp.Text + " and expto=" + ddltoexp.Text + "  and session='" + session + "' and month='" + ddlmonth.Text + "' and type='" + ddltypeadd.SelectedItem + "') update hallsupervision set max_superivison='" + txtSupervisor.Text + "',session='" + session + "'  where expfrom=" + ddlfromexp.Text + " and expto=" + ddltoexp.Text + "  and session='" + session + "' and month='" + ddlmonth.Text + "' and type='" + ddltypeadd.SelectedItem + "'   else insert into hallsupervision (expfrom,expto,max_superivison,session,month,year) values('" + ddlfromexp.Text + "','" + ddltoexp.Text + "','" + txtSupervisor.Text + "','" + session + "','" + ddlmonth.Text + "','" + ddltypeadd.SelectedItem + "')";
                    //    save = da.insert_method(sqlquery1, ht, "Text");
                    //}
                    if (save != 0)
                    {
                        if (btnsave.Text == "Save")
                        {
                            lblerror.Visible = false;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Saved Successfully')", true);
                            btnnew_click(sender, e);
                            spread1();
                        }
                    }
                    if (save != 0)
                    {
                        if (btnsave.Text == "Update")
                        {
                            lblerror.Visible = false;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Updated Successfully')", true);
                            btnnew.Visible = true;
                            ddlfromexp.Enabled = true;
                            ddltoexp.Enabled = true;
                            ddltypeadd.Enabled = true;
                            lblerror.Visible = false;
                            AddPageModify.Text = "Add";
                            ddlfromexp.SelectedIndex = 0;
                            ddltoexp.SelectedIndex = 0;
                            txtSupervisor.Text = string.Empty;
                            ddlsession.SelectedIndex = 0;
                            ddltypeadd.SelectedIndex = 0;
                            btnsave.Text = "Save";
                            lblerror1.Visible = false;
                            btndelete.Enabled = false;
                            spread1();
                        }
                    }
                    else
                    {
                        btnnew.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Already Data Has Been Inserted";
                    }
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "From Experience Should Not Be Greater Than To Experience";
            }
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Please Enter Max Supervision No";
        }
    }

    protected void btndelete_click(object sender, EventArgs e)
    {
        string session = string.Empty;
        if (ddlsession.Text == "0")
        {
            session = "F.N";
        }
        else if (ddlsession.Text == "1")
        {
            session = "A.N";
        }
        else if (ddlsession.Text == "2")
        {
            session = "F.N/A.N";
        }
        string sqlquery = "Delete from hallsupervision where max_superivison='" + txtSupervisor.Text + "'and session='" + session + "' and type='" + ddltypeadd.SelectedItem + "' and  expfrom=" + ddlfromexp.Text + " and expto=" + ddltoexp.Text + "";
        int save = da.update_method_wo_parameter(sqlquery, "Text");
        if (save != 0)
        {
            lblerror.Visible = false;
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            btnnew.Visible = true;
            ddlfromexp.Enabled = true;
            ddltoexp.Enabled = true;
            ddltypeadd.Enabled = true;
            lblerror.Visible = false;
            AddPageModify.Text = "Add";
            ddlfromexp.SelectedIndex = 0;
            ddltoexp.SelectedIndex = 0;
            txtSupervisor.Text = string.Empty;
            ddlsession.SelectedIndex = 0;
            ddltypeadd.SelectedIndex = 0;
            btnsave.Text = "Save";
            lblerror1.Visible = false;
            btndelete.Enabled = false;
            spread1();
        }
    }

    protected void Btnedit_click(object sender, EventArgs e)
    {
        Response.Redirect("CoeHome.aspx");
    }

    protected void spread1()
    {
        try
        {
            lblerror1.Visible = false;
            txtexcelname.Text = string.Empty;
            fpcammarkstaff.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnxl.Visible = true;
            btnprintmaster.Visible = true;
            ds.Clear();
            fpcammarkstaff.Sheets[0].RowCount = 0;
            fpcammarkstaff.Sheets[0].ColumnCount = 5;
            fpcammarkstaff.Sheets[0].RowHeader.Visible = false;
            fpcammarkstaff.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            fpcammarkstaff.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpcammarkstaff.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpcammarkstaff.Sheets[0].RowHeader.Width = 50;
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = Color.Black;
            style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpcammarkstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpcammarkstaff.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpcammarkstaff.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpcammarkstaff.Sheets[0].AllowTableCorner = true;
            fpcammarkstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            fpcammarkstaff.Sheets[0].AllowTableCorner = true;
            fpcammarkstaff.Sheets[0].AllowTableCorner = true;
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Exp From";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Exp To";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Max.No";
            fpcammarkstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Session";
            fpcammarkstaff.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpcammarkstaff.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpcammarkstaff.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpcammarkstaff.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            fpcammarkstaff.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            fpcammarkstaff.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            // string strquery = "Select * from hallsupervision where expfrom>='" + DropDownList4.Text + "' and expto<='" + DropDownList5.Text + "' and month='" + DropDownList2.SelectedValue + "' and type='" + ddltypeview.SelectedItem + "'";
            string typval = string.Empty;
            if (ddltypeview.Enabled == true && ddltypeview.Items.Count > 0)
            {
                typval = " and type='" + ddltypeview.SelectedItem.ToString() + "'";
                ddltypeadd.SelectedValue = ddltypeview.SelectedValue;
                ddltypeadd.Enabled = false;
            }
            string strquery = "Select * from hallsupervision where expfrom>='" + DropDownList4.Text + "' and expto<='" + DropDownList5.Text + "' " + typval + " order by expfrom ";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int cn = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    fpcammarkstaff.Sheets[0].RowCount++;
                    cn++;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 0].Text = cn.ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["expfrom"].ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["expto"].ToString(); ;
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["max_superivison"].ToString();
                    fpcammarkstaff.Sheets[0].Cells[fpcammarkstaff.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["session"].ToString();
                }
                fpcammarkstaff.Sheets[0].Columns[0].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[1].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[2].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[3].Locked = true;
                fpcammarkstaff.Sheets[0].Columns[4].Locked = true;
            }
            else
            {
                lblerror1.Visible = true;
                lblerror1.Text = "No Records Found";
                btnprintmaster.Visible = false;
                fpcammarkstaff.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
            }
            fpcammarkstaff.Sheets[0].PageSize = fpcammarkstaff.Sheets[0].RowCount;
            fpcammarkstaff.SaveChanges();
        }
        catch (Exception ex)
        {
        }
    }

    public void year()
    {
        ds.Clear();
        ddltypeadd.Items.Clear();
        ddltypeview.Items.Clear();
        ds = da.Examyear();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddltypeadd.DataSource = ds;
            ddltypeadd.DataTextField = "Exam_year";
            ddltypeadd.DataValueField = "Exam_year";
            ddltypeadd.DataBind();
            ddltypeadd.SelectedIndex = ddltypeview.Items.Count - 1;

            ddltypeview.DataSource = ds;
            ddltypeview.DataTextField = "Exam_year";
            ddltypeview.DataValueField = "Exam_year";
            ddltypeview.DataBind();
            ddltypeview.SelectedIndex = ddltypeview.Items.Count - 1;
        }
    }

    protected void fpcammarkstaff_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellclick = true;
        Accordion1.SelectedIndex = 1;
    }

    protected void fpcammarkstaff_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                AddPageModify.Text = "Modify";
                string activerow = string.Empty;
                string activecol = string.Empty;
                activerow = fpcammarkstaff.ActiveSheetView.ActiveRow.ToString();
                activecol = fpcammarkstaff.ActiveSheetView.ActiveColumn.ToString();
                lblerror.Visible = false;
                Accordion1.SelectedIndex = 2;
                ddlfromexp.Enabled = true;
                ddltoexp.Enabled = true;
                btndelete.Enabled = true;
                //   ddlsession.Enabled = false;
                btnsave.Text = "Update";
                for (int i = 0; i < fpcammarkstaff.Sheets[0].RowCount; i++)
                {
                    if (i == Convert.ToInt32(activerow))
                    {
                        fpcammarkstaff.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                        fpcammarkstaff.Sheets[0].SelectionBackColor = Color.IndianRed;
                        fpcammarkstaff.Sheets[0].SelectionForeColor = Color.White;
                    }
                    else
                    {
                        fpcammarkstaff.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                }
                //string month = DropDownList2.Text;
                //string year = ddltypeview.Text;
                string expfrom = fpcammarkstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                string expto = fpcammarkstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                string maxno = fpcammarkstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                string session = fpcammarkstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                if (session == "F.N/A.N")
                {
                    ddlsession.Text = "2";
                }
                else if (session == "F.N")
                {
                    ddlsession.Text = "0";
                }
                else if (session == "A.N")
                {
                    ddlsession.Text = "1";
                }
                ddlfromexp.Text = expfrom;
                ddltoexp.Text = expto;
                //ddlmonth.Text = month;
                //ddltypeadd.Text = year;
                txtSupervisor.Text = maxno;

                btnnew.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Hall Supervision " + '@' + "Date :" + DateTime.Now.ToString();
            string pagename = "hallsupervisor.aspx";
            Printcontrol.loadspreaddetails(fpcammarkstaff, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexcelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreport(fpcammarkstaff, report);
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

}