using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Text.RegularExpressions;
using System.Text;
using InsproDataAccess;
using System.Globalization;
using wc = System.Web.UI.WebControls;
using System.Configuration;
public partial class MarkrangeAnalysis : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet nofar = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable addtotalhash = new Hashtable();
    Hashtable hat1 = new Hashtable();
    Hashtable totalmode = new Hashtable();
    Hashtable newhash = new Hashtable();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    ReuasableMethods rs = new ReuasableMethods();
    static byte roll = 0;
    bool Cellclick = false;

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
                BindCollege();
                bindbatch();
                GridBind();
            }
        }
        catch (Exception ex) { }
    }

    void BindCollege()
    {
        try
        {
            ds.Clear();
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }
            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
        catch
        {
        }
    }
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        GridBind();

        Fpspread1.SaveChanges();
        Fpspread1.Visible = false;
    }

    public void cb_batch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
            if (cb_batch.Checked == true)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cb_batch.Checked == true)
                    {
                        cbl_batch.Items[i].Selected = true;
                        txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
                        build1 = cbl_batch.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                    txt_batch.Text = "--Select--";
                }
            }
            // bindsem();
            // BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_batch.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_batch.Text = "--Select--";
                    build = cbl_batch.Items[i].Value.ToString();
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
            if (seatcount == cbl_batch.Items.Count)
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
                cb_batch.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_batch.Text = "--Select--";
                cb_batch.Text = "--Select--";
            }
            else
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
            }
            // bindsem();
            // BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }


    public void bindbatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();

                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[0].Selected = true;
                    }

                    txt_batch.Text = "Batch(" + 1 + ")";

                }
                else
                {
                    txt_batch.Text = "--Select--";
                    cb_batch.Checked = false;
                }
            }
        }
        catch
        {
        }
    }
    public void ddl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
    }


    protected void txttosecamnt_change(object sender, EventArgs e)
    {
        try
        {
            if (Session["entryit"] == null)
            {
                string getfrstamnt = "";
                string getsecamnt = "";
                if (txtfrmsecamnt.Text == "")
                {
                    txttosecamnt.Text = "";
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Enter From Amount!";

                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("sno");
                    dt.Columns.Add("itfrmamnt");
                    dt.Columns.Add("ittoamnt");
                    dt.Columns.Add("itCalculationPK");

                    DataTable dnew = new DataTable();
                    dnew = (DataTable)Session["dtitset"];
                    DataRow dr;
                    if (dnew != null)
                    {
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                dr[0] = Convert.ToString(ro + 1);
                                for (int col = 1; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (int newro = 0; newro < dt.Rows.Count; newro++)
                        {
                            for (int col = 0; col < dt.Columns.Count; col++)
                            {
                                getfrstamnt = Convert.ToString(Convert.ToString(dt.Rows[newro][1]).Trim());
                                getsecamnt = Convert.ToString(Convert.ToString(dt.Rows[newro][2]).Trim());
                                if ((Convert.ToDouble(txtfrmsecamnt.Text) >= Convert.ToDouble(getfrstamnt) && Convert.ToDouble(txttosecamnt.Text) <= Convert.ToDouble(getsecamnt)) || (Convert.ToDouble(txtfrmsecamnt.Text) <= Convert.ToDouble(getsecamnt) && Convert.ToDouble(txttosecamnt.Text) >= Convert.ToDouble(getfrstamnt)))
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";

                                    return;
                                }
                                if (Convert.ToDouble(getfrstamnt) == Convert.ToDouble(txtfrmsecamnt.Text))
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";

                                    return;
                                }
                                if (Convert.ToDouble(getsecamnt) == Convert.ToDouble(txttosecamnt.Text))
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";

                                    return;
                                }
                            }
                        }
                    }

                }
            }
            if (Session["entryit"] == "1")
            {
                if (txtfrmsecamnt.Text.Trim() != "" && txttosecamnt.Text.Trim() != "")
                {
                    string amntfrmto = "";
                    string getfrstamnt = "";
                    string getsecamnt = "";
                    string frmamntlimit = Convert.ToString(ViewState["frmamnt"]);
                    string toamntlimit = Convert.ToString(ViewState["toamnt"]);


                    DataTable dt = new DataTable();
                    dt.Columns.Add("sno");
                    dt.Columns.Add("itfrmamnt");
                    dt.Columns.Add("ittoamnt");

                    DataRow dr;

                    if (Session["dtitset"] != null)
                    {
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["dtitset"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                dr[0] = Convert.ToString(ro + 1);
                                for (int col = 1; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (int newro = 0; newro < dt.Rows.Count; newro++)
                            {
                                if (Convert.ToString(dt.Rows[newro][1]).Trim() == Convert.ToString(frmamntlimit).Trim())
                                {
                                    dt.Rows.Remove(dt.Rows[newro]);
                                }
                            }
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dt.Rows.Count; ro++)
                            {
                                for (int co = 0; co < dt.Columns.Count; co++)
                                {
                                    getfrstamnt = Convert.ToString(Convert.ToString(dt.Rows[ro][1]).Trim());
                                    getsecamnt = Convert.ToString(Convert.ToString(dt.Rows[ro][2]).Trim());
                                    if ((Convert.ToDouble(getfrstamnt) <= Convert.ToDouble(txtfrmsecamnt.Text.Trim()) && Convert.ToDouble(getsecamnt) >= Convert.ToDouble(txttosecamnt.Text.Trim())) || (Convert.ToDouble(txtfrmsecamnt.Text) <= Convert.ToDouble(getsecamnt) && Convert.ToDouble(txttosecamnt.Text) >= Convert.ToDouble(getfrstamnt)))
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "Amount Range Already Exists!";
                                        txtfrmsecamnt.Text = "";
                                        txttosecamnt.Text = "";

                                        return;
                                    }
                                    else if (Convert.ToDouble(getfrstamnt) == Convert.ToDouble(txtfrmsecamnt.Text.Trim()))
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "Amount Range Already Exists!";
                                        txtfrmsecamnt.Text = "";
                                        txttosecamnt.Text = "";

                                        return;
                                    }
                                    else if (Convert.ToDouble(getsecamnt) == Convert.ToDouble(txttosecamnt.Text.Trim()))
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "Amount Range Already Exists!";
                                        txtfrmsecamnt.Text = "";
                                        txttosecamnt.Text = "";

                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                Session["entryit"] = null;
            }
        }
        catch { }
    }

    protected void btnadditset_click(object sender, EventArgs e)
    {

        int savecount = 0;
        string frmamntlimit = string.Empty;
        string toamntlimit = string.Empty;

        string itCalPk = string.Empty;

        try
        {
            if (txtfrmsecamnt.Text.Trim() != "" && txttosecamnt.Text.Trim() != "")
            {
                string getfrstamnt = "";
                string getsecamnt = "";
                // string amntfrmto = "";
                string getgender = "";
                int rocount = 0;
                frmamntlimit = Convert.ToString(txtfrmsecamnt.Text);
                toamntlimit = Convert.ToString(txttosecamnt.Text);

                divgrditset.Visible = true;
                grditset.Visible = true;

                if (string.IsNullOrEmpty(itCalPk))
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("sno");
                    dt.Columns.Add("itfrmamnt");
                    dt.Columns.Add("ittoamnt");
                    dt.Columns.Add("itCalculationPK");
                    DataRow dr;

                    if (Session["dtitset"] != null)
                    {
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["dtitset"];
                        int dnewColCount = (dnew.Columns.Count);
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                dr[0] = Convert.ToString(ro + 1);
                                for (int col = 1; col < dnewColCount; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dt.Rows.Count; ro++)
                            {
                                for (int co = 0; co < dt.Columns.Count; co++)
                                {
                                    getfrstamnt = Convert.ToString(Convert.ToString(dt.Rows[ro][1]).Trim());
                                    getsecamnt = Convert.ToString(Convert.ToString(dt.Rows[ro][2]).Trim());

                                    if ((Convert.ToDouble(getfrstamnt) <= Convert.ToDouble(txtfrmsecamnt.Text.Trim()) && Convert.ToDouble(getsecamnt) >= Convert.ToDouble(txttosecamnt.Text.Trim())) || (Convert.ToDouble(txtfrmsecamnt.Text) <= Convert.ToDouble(getsecamnt) && Convert.ToDouble(txttosecamnt.Text) >= Convert.ToDouble(getfrstamnt)))
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "Amount Range Already Exists!";
                                        txtfrmsecamnt.Text = "";
                                        txttosecamnt.Text = "";

                                        return;
                                    }
                                    else if (Convert.ToDouble(getfrstamnt) == Convert.ToDouble(txtfrmsecamnt.Text.Trim()))
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "Amount Range Already Exists!";
                                        txtfrmsecamnt.Text = "";
                                        txttosecamnt.Text = "";


                                        return;
                                    }
                                    else if (Convert.ToDouble(getsecamnt) == Convert.ToDouble(txttosecamnt.Text.Trim()))
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "Amount Range Already Exists!";
                                        txtfrmsecamnt.Text = "";
                                        txttosecamnt.Text = "";


                                        return;
                                    }
                                    else
                                    {
                                        rocount++;
                                    }
                                }
                            }
                        }

                        if (rocount == (dt.Rows.Count * dt.Columns.Count))
                        {
                            dr = dt.NewRow();
                            dr["sno"] = Convert.ToString(dt.Rows.Count + 1);
                            dr["itfrmamnt"] = Convert.ToString(frmamntlimit);
                            dr["ittoamnt"] = Convert.ToString(toamntlimit);

                            //   dr["itCalculationPK"] = Convert.ToString(gender);
                            dt.Rows.Add(dr);
                            Session["dtitset"] = dt;
                        }
                    }
                    else
                    {
                        dr = dt.NewRow();
                        dr["sno"] = Convert.ToString("1");
                        dr["itfrmamnt"] = Convert.ToString(frmamntlimit);
                        dr["ittoamnt"] = Convert.ToString(toamntlimit);

                        dt.Rows.Add(dr);
                        Session["dtitset"] = dt;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grditset.DataSource = dt;
                        grditset.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grditset.DataBind();
                        txtfrmsecamnt.Text = "";
                        txttosecamnt.Text = "";


                        grditset.Columns[0].HeaderStyle.Width = 75;
                        grditset.Columns[0].ItemStyle.Width = 75;
                        grditset.Columns[1].HeaderStyle.Width = 125;
                        grditset.Columns[1].ItemStyle.Width = 125;
                        grditset.Columns[2].HeaderStyle.Width = 125;
                        grditset.Columns[2].ItemStyle.Width = 125;
                        grditset.Columns[3].HeaderStyle.Width = 100;
                        grditset.Columns[3].ItemStyle.Width = 100;
                        grditset.Columns[4].HeaderStyle.Width = 125;
                        grditset.Columns[4].ItemStyle.Width = 125;

                    }
                    else
                    {
                        grditset.DataSource = dt;
                        grditset.DataBind();
                    }
                    if (savecount > 0)
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Added Successfully!";
                        grditset.Visible = true;
                        divgrditset.Visible = true;
                        txtfrmsecamnt.Text = "";
                        txttosecamnt.Text = "";

                    }


                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Fill All the Values!";
                    }
                }
                else
                {

                    //   string query = "update HR_ITCalculationSettings set FromRange=" + frmamntlimit + ",ToRange=" + toamntlimit + ",Mode=" + mode + ",Amount='" + amntfrmto.Replace("%", "") + "',collegeCode=" + ddlcollege.SelectedValue + ",sex='" + gender + "' where ITCalculationPK=" + itCalPk;
                    //   int updatedRowCount = d2.update_method_wo_parameter(query, "Text");
                    //    lbl_alert.Text = "Updated Successfully!";
                    //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                    //GridBind();
                    txtfrmsecamnt.Text = "";
                    txttosecamnt.Text = "";
                    //   txtuptosec.Text = "";
                    //  txtItCalPk.Text = "";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Enter the Values!";

            }

        }
        catch { }




    }

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        try
        {
            imgAlert.Visible = false;
            //  divgrditset.Visible = true;
            // grditset.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }


    public void GridBind()
    {
        string selitcal = "select * from MarkRange where collegeCode='" + ddlcollege.SelectedValue + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selitcal, "Text");
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    divgrditset.Visible = true;
                    grditset.Visible = true;
                    bindgrditset();
                }
                else
                {
                    Session["dtitset"] = null;
                    divgrditset.Visible = false;
                    grditset.Visible = false;
                    Fpspread1.Visible = false;

                }
            }
            else
            {
                Session["dtitset"] = null;
                divgrditset.Visible = false;
                grditset.Visible = false;
                Fpspread1.Visible = false;
                bindgrditset();

            }
        }
    }



    public void bindgrditset()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("sno");
            dt.Columns.Add("itfrmamnt");
            dt.Columns.Add("ittoamnt");
            dt.Columns.Add("itCalculationPK");

            string selitset = "select * from MarkRange where collegeCode='" + ddlcollege.SelectedValue + "' order by From_Range";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selitset, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        dr["sno"] = Convert.ToString(i + 1);
                        dr["itfrmamnt"] = Convert.ToString(ds.Tables[0].Rows[i]["From_Range"]);
                        dr["ittoamnt"] = Convert.ToString(ds.Tables[0].Rows[i]["To_Range"]);
                        dr["itCalculationPK"] = Convert.ToString(ds.Tables[0].Rows[i]["markPK"]);
                        dt.Rows.Add(dr);
                    }
                    Session["dtitset"] = dt;
                }
            }
            if (dt.Rows.Count > 0)
            {
                grditset.DataSource = dt;
                grditset.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grditset.DataBind();
                grditset.Visible = true;
                divgrditset.Visible = true;
                //Fpspread1.Visible = false;
            }
            else
            {
                grditset.Visible = false;
                divgrditset.Visible = false;
                Fpspread1.Visible = false;


            }
            grditset.Columns[0].HeaderStyle.Width = 75;
            grditset.Columns[0].ItemStyle.Width = 75;
            grditset.Columns[1].HeaderStyle.Width = 125;
            grditset.Columns[1].ItemStyle.Width = 125;
            grditset.Columns[2].HeaderStyle.Width = 125;
            grditset.Columns[2].ItemStyle.Width = 125;
            grditset.Columns[3].HeaderStyle.Width = 100;
            grditset.Columns[3].ItemStyle.Width = 100;

        }
        catch { }
    }

    protected void btn_del_Click(object sender, EventArgs e)
    {
        int rowindex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
        try
        {
            if (grditset.Rows.Count > 0)
            {
                string itCalPk = ((Label)grditset.Rows[rowindex].FindControl("lbl_itCalPk")).Text;
                string delQuery = "Delete  from markrange where markPK=" + itCalPk;
                int delcount = d2.update_method_wo_parameter(delQuery, "Text");
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);

            }
            GridBind();
        }
        catch { }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {

        // int rowindex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
        try
        {
            //if (grditset.Rows.Count > 0)
            //{
            //    string frmamntlimit = ((Label)grditset.Rows[rowindex].FindControl("lbl_frmamnt")).Text;
            //    string toamntlimit = ((Label)grditset.Rows[rowindex].FindControl("lbl_toamnt")).Text;

            //    string itCalPk = ((Label)grditset.Rows[rowindex].FindControl("lbl_itCalPk")).Text;
            //    txtfrmsecamnt.Text = frmamntlimit;
            //    txttosecamnt.Text = toamntlimit;

            //  //  txtItCalPk.Text = itCalPk;
            //    // btnadditset_click(sender, e);
            //}
            //GridBind();
        }
        catch { }

    }

    protected void grditset_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string itfrmrange = "";
            string ittorange = "";

            Session["entryit"] = "1";

            for (int rem = 0; rem < grditset.Rows.Count; rem++)
            {
                grditset.Rows[rem].BackColor = Color.White;
            }

            int idx = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Index")
            {
                divgrditset.Visible = true;
                grditset.Visible = true;

                itfrmrange = (grditset.Rows[idx].FindControl("lbl_frmamnt") as Label).Text;
                txtfrmsecamnt.Text = itfrmrange;
                ViewState["frmamnt"] = Convert.ToString(itfrmrange);
                ittorange = (grditset.Rows[idx].FindControl("lbl_toamnt") as Label).Text;
                txttosecamnt.Text = Convert.ToString(ittorange);
                ViewState["toamnt"] = Convert.ToString(ittorange);
                //itCalPk = (grditset.Rows[idx].FindControl("itCalculationPK") as Label).Text;
                // txtItCalPk.Text = itCalPk;
                //ViewState["itCalPk"] = Convert.ToString(itCalPk);
                grditset.Rows[idx].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }

    protected void grditset_rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
            // e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
            //e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
            //e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);

            Label lblPK = (Label)e.Row.Cells[0].FindControl("lbl_itCalPk");
            Button btnDel = (Button)e.Row.Cells[3].FindControl("btn_del");
            //   Button btnUpd = (Button)e.Row.Cells[4].FindControl("btn_update");
            btnDel.Enabled = true;
            //btnUpd.Enabled = true;
            if (lblPK.Text.Trim() == string.Empty || lblPK.Text.Trim() == "0")
            {
                btnDel.Enabled = false;
                // btnUpd.Enabled = false;
            }
            grditset.Visible = true;
        }


    }

    protected void btnsaveallitset_Click(object sender, EventArgs e)
    {
        try
        {

            string frmrange = "";
            string torange = "";
            string addbatch = string.Empty;
            string itCalPk = "";
            int insertcount = 0;
            string delq = "delete from MarkRange where collegeCode='" + ddlcollege.SelectedValue + "'";
            int delcount = d2.update_method_wo_parameter(delq, "Text");

            DataSet deptrightsds = new DataSet();
            string rights = string.Empty;
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
            string degreeVal = string.Empty;
            string deptrightsQry = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedValue + "'  and user_code='" + usercode + "'";

            deptrightsds = d2.select_method_wo_parameter(deptrightsQry, "text");
            if (deptrightsds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < deptrightsds.Tables[0].Rows.Count; i++)
                {
                    string getdegreeCode = Convert.ToString(deptrightsds.Tables[0].Rows[i]["degree_code"]);
                    if (degreeVal == "")
                    {
                        degreeVal = getdegreeCode;
                    }
                    else
                    {

                        degreeVal = degreeVal + "','" + getdegreeCode;
                    }

                }
            }
            addbatch = rs.GetSelectedItemsValueAsString(cbl_batch);
            DataSet courseds = new DataSet();
            string course_code = string.Empty;
            string courseQuery = " select TextCode,TextVal  from textvaltable where TextCriteria='cours' and college_code='" + ddlcollege.SelectedValue + "' and textval like '%hsc%' or textval like '%higher%' or textval like '%plus%'";
            courseds = d2.select_method_wo_parameter(courseQuery, "text");
            if (courseds.Tables[0].Rows.Count > 0)
            {
                for (int cou = 0; cou < courseds.Tables[0].Rows.Count; cou++)
                {
                    string getcourseCode = Convert.ToString(courseds.Tables[0].Rows[cou]["TextCode"]);
                    if (course_code == "")
                    {
                        course_code = getcourseCode;
                    }
                    else
                    {

                        course_code = course_code + "','" + getcourseCode;
                    }

                }

            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Higher Secondary Students Not Registered!";
                return;
            }

            if (grditset.Rows.Count > 0)
            {
                for (int ro = 0; ro < grditset.Rows.Count; ro++)
                {
                    Label lblfrmrange = (Label)grditset.Rows[ro].FindControl("lbl_frmamnt");
                    Label lbltorange = (Label)grditset.Rows[ro].FindControl("lbl_toamnt");

                    Label lblitCalPk = (Label)grditset.Rows[ro].FindControl("lbl_itCalPk");
                    frmrange = Convert.ToString(lblfrmrange.Text);
                    torange = Convert.ToString(lbltorange.Text);

                    itCalPk = Convert.ToString(lblitCalPk.Text);

                    string insertq = "  if exists(select * from MarkRange where markPK='" + itCalPk + "') update MarkRange set From_Range='" + frmrange + "',To_Range='" + torange + "',collegecode='" + ddlcollege.SelectedValue + "' where markPK='" + itCalPk + "' else insert into MarkRange (From_Range,To_Range,collegeCode) values ('" + frmrange + "','" + torange + "','" + ddlcollege.SelectedValue + "')";
                    insertcount = d2.update_method_wo_parameter(insertq, "Text");
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Enter The Values!";
                return;

            }
            if (insertcount == 1)
            {

                ds.Clear();
                string query = "select * from MarkRange where collegeCode='" + ddlcollege.SelectedValue + "'";
                ds = d2.select_method_wo_parameter(query, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataSet appliedds = new DataSet();
                    appliedds.Clear();
                    appliedds.Reset();
                    string checkapplied_admitted = string.Empty;
                    if (ddl_status.SelectedItem.Value == "0")
                    {

                        string query1 = "select COUNT(a.app_no) as TotalCount,a.degree_code,a.Batch_Year,C.Course_Name,c.Course_Id ,Dt.Dept_Name   from degree d,Department dt,Course C ,applyn a,stud_prev_details sp Where a.app_no=sp.app_no and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.degree_code in('" + degreeVal + "')and a.Batch_Year in('" + addbatch + "')  and a.college_code='" + ddlcollege.SelectedValue + "' and course_code in('" + course_code + "') and a.college_code=d.college_code  and isconfirm='1' group by a.degree_code,a.Batch_Year,c.Course_Id ,C.Course_Name,Dt.Dept_Name order by a.Batch_Year desc,c.Course_Id";
                        appliedds = d2.select_method_wo_parameter(query1, "text");
                        checkapplied_admitted = ddl_status.SelectedItem.Text;

                    }
                    if (ddl_status.SelectedItem.Value == "1")
                    {

                        string query1 = "  select COUNT(a.app_no)as TotalCount,r.degree_code,r.Batch_Year,C.Course_Name,c.Course_Id ,Dt.Dept_Name from applyn a,Registration r, degree d,Department dt,Course C,Stud_prev_details pv where a.app_no=pv.app_no and r.App_No=pv.app_no and a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + degreeVal + "')and r.Batch_Year in('" + addbatch + "') and r.college_code='" + ddlcollege.SelectedValue + "' and course_code in('" + course_code + "') and admission_status='1' and DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0) group by r.degree_code ,r.Batch_Year,C.Course_Name,c.Course_Id ,Dt.Dept_Name order by r.Batch_Year desc,c.Course_Id";
                        appliedds = d2.select_method_wo_parameter(query1, "text");
                        checkapplied_admitted = ddl_status.SelectedItem.Text;
                    }

                    if (appliedds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Visible = true;
                        Fpspread1.Sheets[0].Visible = true;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = true;
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].ColumnCount = 5;
                        FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle2.ForeColor = Color.Black;
                        darkstyle2.HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;

                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Branch";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total" + " " + checkapplied_admitted;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                string fromrange = Convert.ToString(ds.Tables[0].Rows[j]["From_Range"]);
                                string to_range = Convert.ToString(ds.Tables[0].Rows[j]["To_Range"]);
                                Fpspread1.Sheets[0].ColumnCount++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = fromrange + "-" + to_range;

                            }
                            if (chkNotEntered.Checked == true)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Not Entered";

                            }
                        }
                        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                        style2.Font.Size = 14;
                        style2.Font.Name = "Book Antiqua";
                        style2.Font.Bold = true;
                        style2.HorizontalAlign = HorizontalAlign.Center;
                        style2.ForeColor = Color.Black;
                        style2.BackColor = Color.AliceBlue;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        Fpspread1.Sheets[0].RowCount = 0;
                        int getcc = 0;
                        int count = 0;
                        int overallCount = 0;
                        for (int rowbind = 0; rowbind < appliedds.Tables[0].Rows.Count; rowbind++)
                        {
                            string batchYear = string.Empty;
                            string course = string.Empty;
                            string dept_code = string.Empty;
                            string total_count = string.Empty;
                            Fpspread1.Sheets[0].RowCount++;
                            count++;
                            batchYear = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["Batch_Year"]);
                            course = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["Course_Id"]);
                            dept_code = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["degree_code"]);
                            total_count = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["TotalCount"]);
                            overallCount = overallCount + Convert.ToInt32(total_count);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Locked = true;

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["Batch_Year"]);

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Locked = true;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["Batch_Year"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["Course_Name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["Course_Id"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Locked = true;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["Dept_Name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["degree_code"]);

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["TotalCount"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(appliedds.Tables[0].Rows[rowbind]["TotalCount"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            string totcount1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag);
                            int totalcnt = Convert.ToInt32(totcount1);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                getcc = 4;
                                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                {
                                    getcc++;
                                    string fromrange = Convert.ToString(ds.Tables[0].Rows[j]["From_Range"]);
                                    string to_range = Convert.ToString(ds.Tables[0].Rows[j]["To_Range"]);
                                    string totcount = string.Empty;
                                    if (ddl_status.SelectedItem.Value == "0")
                                    {

                                        totcount = d2.GetFunction("select COUNT(a.app_no) as TotalCount  from degree d,Department dt,Course C ,applyn a,stud_prev_details sp Where a.app_no=sp.app_no and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.degree_code in('" + dept_code + "')and a.Batch_Year in('" + batchYear + "')  and a.college_code='" + ddlcollege.SelectedItem.Value + "' and course_code in('" + course_code + "') and securedmark between '" + fromrange + "' and '" + to_range + "'  and a.college_code=d.college_code  and isconfirm='1' group by a.degree_code,a.Batch_Year,c.Course_Id ,C.Course_Name,Dt.Dept_Name order by c.Course_Id");
                                    }
                                    if (ddl_status.SelectedItem.Value == "1")
                                    {

                                        totcount = d2.GetFunction("select COUNT(a.app_no)as TotalCount from applyn a,Registration r, degree d,Department dt,Course C,Stud_prev_details pv where a.app_no=pv.app_no and r.App_No=pv.app_no and a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + dept_code + "')and r.Batch_Year in('" + batchYear + "') and r.college_code='" + ddlcollege.SelectedValue + "' and course_code in('" + course_code + "') and securedmark between '" + fromrange + "' and '" + to_range + "' and admission_status='1' and DelFlag='0' and Exam_Flag='OK' and (cc='False' or cc=0) group by r.degree_code ,r.Batch_Year,C.Course_Name,c.Course_Id ,Dt.Dept_Name order by r.Batch_Year desc,c.Course_Id");


                                    }

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, getcc].Text = totcount;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, getcc].Tag = totcount;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, getcc].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, getcc].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, getcc].Font.Size = FontUnit.Medium;

                                    if (!totalmode.Contains(getcc))
                                    {
                                        totalmode.Add(getcc, Convert.ToInt32(totcount));


                                    }
                                    else
                                    {
                                        int getval = Convert.ToInt32(totalmode[getcc]);
                                        int chkmod = getval + Convert.ToInt32(totcount);
                                        totalmode[getcc] = chkmod;


                                    }

                                    if (chkNotEntered.Checked == true)
                                    {
                                        if (totalcnt != 0)
                                        {
                                            string count1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, getcc].Tag);
                                            totalcnt = totalcnt - Convert.ToInt32(count1);

                                        }
                                    }
                                }
                            }
                            //Added By Saranyadevi 11.7.2018
                            if (chkNotEntered.Checked == true)
                            {
                                getcc++;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, getcc].Text = Convert.ToString(totalcnt);
                                // Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, getcc].Tag = totalcnt;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, getcc].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, getcc].Font.Name = "Book Antiqua";

                                if (!totalmode.Contains(getcc))
                                {
                                    totalmode.Add(getcc, Convert.ToInt32(totalcnt));


                                }
                                else
                                {
                                    int getval = Convert.ToInt32(totalmode[getcc]);
                                    int chkmod = getval + Convert.ToInt32(totalcnt);
                                    totalmode[getcc] = chkmod;


                                }
                            }

                        }


                        Fpspread1.Sheets[0].RowCount++;

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].Text = "Total";
                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].Tag = "Total";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].BackColor = ColorTranslator.FromHtml("#80EDED");
                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].Font.Bold = true;
                        Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 3);
                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].ForeColor = Color.Maroon;
                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 4].Text = Convert.ToString(overallCount);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 4].ForeColor = ColorTranslator.FromHtml("#107532");

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 4].Font.Bold = true;

                        Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), 4].BackColor = ColorTranslator.FromHtml("#80EDED");

                        foreach (DictionaryEntry entry in totalmode)
                        {
                            int col = Convert.ToInt32(entry.Key);
                            string getval = Convert.ToString(entry.Value);

                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].Text = Convert.ToString(getval);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].ForeColor = ColorTranslator.FromHtml("#107532");

                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].Font.Bold = true;

                            Fpspread1.Sheets[0].Cells[(Fpspread1.Sheets[0].RowCount - 1), col].BackColor = ColorTranslator.FromHtml("#80EDED");
                        }

                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "No Records Found!";
                        return;

                    }

                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Width = 900;
                    Fpspread1.Height = 420;
                    Fpspread1.Visible = true;
                    //  rptprint.Visible = true;
                    Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    //  Fpspread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);

                }


            }
        }
        catch (Exception ex)
        {


        }
    }

    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx");
        }
    }
    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtfrmsecamnt.Text = "";
        txttosecamnt.Text = "";
        ds.Clear();
        int delcount = 0;
        string query = "delete MarkRange where collegeCode='" + ddlcollege.SelectedValue + "'";

        delcount = d2.update_method_wo_parameter(query, "Text");
        if (delcount == 1)
        {
            divgrditset.Visible = false;
            Fpspread1.Visible = false;

        }


    }

    public void imgbtn_all_Click(object sender, EventArgs e)
    {
        //poppernew.Visible = true;
        //load();
        //lb_column1.Items.Clear();
    }




}