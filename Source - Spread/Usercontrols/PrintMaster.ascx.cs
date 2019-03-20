using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using Gios.Pdf;

public partial class Usercontrols_PrintMaster : System.Web.UI.UserControl
{
    string collegecode = "";

    #region "Veriables"

    static Hashtable hat_print = new Hashtable();
    static TreeNode node;
    static TreeNode childnode;
    public static FpSpread fpspreadsample;
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    int column_header_row_count_orgi = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        errmsg.Visible = false;
        collegecode = Session["collegecode"].ToString();
        if (!IsPostBack)
        {
            if (Session["column_header_row_count"].ToString() != "" && Session["column_header_row_count"] != null)
            {
                column_header_row_count_orgi = Convert.ToInt16(Session["column_header_row_count"]);
            }
            FpFooter.CommandBar.Visible = false;
            FpFooter.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpFooter.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpFooter.Enabled = false;
            chkSetCommPrint.Checked = false;
            btnok.Enabled = false;
            FpFooter.Sheets[0].RowCount = 1;
            FpFooter.Sheets[0].ColumnCount = 0;
            treeview_spreadfields.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
            FpFooter.Height = 100;
            FpFooter.Width = 600;
            setstyle();
        }
    }

    public void setstyle()
    {
        try
        {
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["user_code"].ToString().Trim() + "";
            }
            string selectvalue = da.GetFunction("select value  from Master_Settings where settings ='Print Edit Option' and " + grouporusercode + " ");
            //if (selectvalue.Trim() == "0")  Aruna 24nov2017
            //{
                maindiv.Visible = true;
            //}
            //else
            //{
                //maindiv.Visible = false;
            //}
        }
        catch
        {

        }
    }

    protected void ddladd_SelectedIndexChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        FpFooter.Sheets[0].RowCount = 0;
        FpFooter.Sheets[0].ColumnCount = 0;
        FpFooter.Enabled = true;
        btnok.Enabled = true;
        int column = 0;
        int row = 0;
        if (ddladd.SelectedItem.ToString() == "Header")
        {

            if (txtcolumn.Text.ToString().Trim() != "0" && txtcolumn.Text.ToString().Trim() != "")
            {
                column = Convert.ToInt32(txtcolumn.Text.ToString().Trim());
            }
            if (txtrow.Text.ToString().Trim() != "" && txtrow.Text.ToString().Trim() != "0")
            {
                row = Convert.ToInt32(txtrow.Text.ToString().Trim());
            }
            FpFooter.Enabled = true;
            FpFooter.Sheets[0].RowCount = Convert.ToInt32(row);
            FpFooter.Sheets[0].ColumnCount = Convert.ToInt32(column);
            FpFooter.Sheets[0].ColumnHeader.RowCount = 1;
            FpFooter.Enabled = true;
            btnok.Enabled = true;
            string headertext = da.GetFunction("select Header from tbl_print_master_settings  where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
            if (headertext.Trim() != "" && headertext.Trim() != "0")
            {
                string[] spitrow = headertext.Split('^');
                if (row == 0)
                {
                    row = spitrow.GetUpperBound(0) + 1;
                }
                for (int g = 0; g < row; g++)
                {
                    if (g <= spitrow.GetUpperBound(0))
                    {
                        string[] spitcolumn = spitrow[g].Split('!');
                        if (column == 0)
                        {
                            column = spitcolumn.GetUpperBound(0) + 1;
                        }
                        FpFooter.Sheets[0].RowCount = Convert.ToInt32(row);
                        FpFooter.Sheets[0].ColumnCount = Convert.ToInt32(column);
                        for (int k = 0; k < column; k++)
                        {
                            if (k <= spitcolumn.GetUpperBound(0))
                            {
                                FpFooter.Sheets[0].Cells[g, k].Text = spitcolumn[k].ToString();
                            }
                        }
                    }
                }
                txtcolumn.Text = column.ToString();
                txtrow.Text = row.ToString();
            }
            else
            {
                if (txtrow.Text.ToString().Trim() == "" || txtrow.Text.ToString().Trim() == "0" || txtcolumn.Text.ToString().Trim() == "0" || txtcolumn.Text.ToString().Trim() == "")
                {
                    FpFooter.Sheets[0].RowCount = 0;
                    FpFooter.Sheets[0].ColumnCount = 0;
                    errmsg.Text = "Please Enter  Value";
                }

            }
        }
        else if (ddladd.SelectedItem.ToString() == "Footer")
        {

            if (txtcolumn.Text.ToString().Trim() != "0" && txtcolumn.Text.ToString().Trim() != "")
            {
                column = Convert.ToInt32(txtcolumn.Text.ToString().Trim());
            }
            if (txtrow.Text.ToString().Trim() != "" && txtrow.Text.ToString().Trim() != "0")
            {
                row = Convert.ToInt32(txtrow.Text.ToString().Trim());
            }
            FpFooter.Enabled = true;
            FpFooter.Sheets[0].RowCount = Convert.ToInt32(row);
            FpFooter.Sheets[0].ColumnCount = Convert.ToInt32(column);
            FpFooter.Sheets[0].ColumnHeader.RowCount = 1;
            FpFooter.Enabled = true;
            btnok.Enabled = true;
            string headertext = da.GetFunction("select Footer from tbl_print_master_settings  where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
            if (headertext.Trim() != "" && headertext.Trim() != "0")
            {
                string[] spitrow = headertext.Split('^');
                if (row == 0)
                {
                    row = spitrow.GetUpperBound(0) + 1;
                }
                for (int g = 0; g < row; g++)
                {
                    if (g <= spitrow.GetUpperBound(0))
                    {
                        string[] spitcolumn = spitrow[g].Split('!');
                        if (column == 0)
                        {
                            column = spitcolumn.GetUpperBound(0) + 1;
                        }
                        FpFooter.Sheets[0].RowCount = Convert.ToInt32(row);
                        FpFooter.Sheets[0].ColumnCount = Convert.ToInt32(column);
                        for (int k = 0; k < column; k++)
                        {
                            if (k <= spitcolumn.GetUpperBound(0))
                            {
                                FpFooter.Sheets[0].Cells[g, k].Text = spitcolumn[k].ToString();
                            }
                        }
                    }
                }
                txtcolumn.Text = column.ToString();
                txtrow.Text = row.ToString();
            }
            else
            {
                if (txtrow.Text.ToString().Trim() == "" || txtrow.Text.ToString().Trim() == "0" || txtcolumn.Text.ToString().Trim() == "0" || txtcolumn.Text.ToString().Trim() == "")
                {
                    FpFooter.Sheets[0].RowCount = 0;
                    FpFooter.Sheets[0].ColumnCount = 0;
                    errmsg.Text = "Please Enter  Value";
                }
            }
        }
        else if (ddladd.SelectedItem.ToString() == "ISO Code")
        {
            txtcolumn.Text = "1";
            if (txtrow.Text.ToString().Trim() != "" && txtrow.Text.ToString().Trim() != "0")
            {
                row = Convert.ToInt32(txtrow.Text.ToString().Trim());
            }
            FpFooter.Enabled = true;
            FpFooter.Sheets[0].RowCount = Convert.ToInt32(row);
            FpFooter.Sheets[0].ColumnCount = 1;
            FpFooter.Sheets[0].ColumnHeader.RowCount = 1;
            FpFooter.Enabled = true;
            btnok.Enabled = true;
            string headertext = da.GetFunction("select Isocode from tbl_print_master_settings  where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
            if (headertext.Trim() != "" && headertext.Trim() != "0")
            {
                string[] spitrow = headertext.Split('^');
                if (row == 0)
                {
                    row = spitrow.GetUpperBound(0) + 1;
                }
                FpFooter.Sheets[0].RowCount = Convert.ToInt32(row);
                FpFooter.Sheets[0].ColumnCount = 1;
                for (int k = 0; k < row; k++)
                {
                    if (k <= spitrow.GetUpperBound(0))
                    {
                        FpFooter.Sheets[0].Cells[k, 0].Text = spitrow[k].ToString();
                    }
                }
                txtcolumn.Text = "1";
                txtrow.Text = row.ToString();
            }
            else
            {
                if (txtrow.Text.ToString().Trim() == "" || txtrow.Text.ToString().Trim() == "0")
                {
                    FpFooter.Sheets[0].RowCount = 0;
                    FpFooter.Sheets[0].ColumnCount = 0;
                    errmsg.Text = "Please Enter Row Value";
                }
            }
        }

        FpFooter.SaveChanges();
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        FpFooter.SaveChanges();
        string header = "";
        if (txtcolumn.Text != "" && txtrow.Text != "" && txtcolumn.Text.Trim() != "0" && txtrow.Text.Trim() != "0")
        {
            for (int r = 0; r < FpFooter.Sheets[0].RowCount; r++)
            {
                if (header != "")
                {
                    header = header + "^";
                }
                for (int c = 0; c < FpFooter.Sheets[0].ColumnCount; c++)
                {
                    if (c == 0)
                    {
                        header = header + FpFooter.Sheets[0].Cells[r, c].Text;
                    }
                    else
                    {
                        header = header + '!' + FpFooter.Sheets[0].Cells[r, c].Text;
                    }
                }
            }

            string setting = ddladd.SelectedItem.ToString();
            ds.Dispose();
            ds.Reset();
            string query = "Select * from tbl_print_master_settings where  page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
            ds = da.select_method(query, hat_print, "Text");

            if (setting == "Header")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string updatequery = "update tbl_print_master_settings set header='" + header + "' where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
                    int q = da.update_method_wo_parameter(updatequery, "Text");
                }
                else
                {
                    string pagesettings = "insert into tbl_print_master_settings (Page_Name,header,usercode) values ('" + Session["Pagename"] + "','" + header + "','" + Convert.ToString(Session["user_code"]) + "')";
                    int p = da.insert_method(pagesettings, hat_print, "Text");
                }
            }
            else if (setting == "Footer")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string updatequery = "update tbl_print_master_settings set Footer='" + header + "' where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
                    int q = da.update_method_wo_parameter(updatequery, "Text");
                }
                else
                {
                    string pagesettings = "insert into tbl_print_master_settings (Page_Name,Footer,usercode) values ('" + Session["Pagename"] + "','" + header + "', '" + Convert.ToString(Session["user_code"]) + "')";
                    int p = da.insert_method(pagesettings, hat_print, "Text");
                }
            }
            else if (setting == "ISO Code")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string updatequery = "update tbl_print_master_settings set ISOCODE='" + header + "' where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
                    int q = da.update_method_wo_parameter(updatequery, "Text");
                }
                else
                {
                    string pagesettings = "insert into tbl_print_master_settings (Page_Name,ISOCODE,usercode) values ('" + Session["Pagename"] + "','" + header + "' ,'" + Convert.ToString(Session["user_code"]) + "')";
                    int p = da.insert_method(pagesettings, hat_print, "Text");
                }
            }
            FpFooter.Enabled = false;
            btnok.Enabled = false;
        }
        else
        {
            errmsg.Visible = true;
            errmsg.Text = "Please Enter Value";
        }
        FpFooter.Sheets[0].RowCount = 0;
        FpFooter.Sheets[0].ColumnCount = 0;
    }

    protected void btnclose_Click(object sender, EventArgs e)
    {
        #region printlock

        string printAvailability = "update TextValTable set TextVal='0' where TextCriteria='prtlk'";
        int printAvailabilityfun = da.update_method_wo_parameter(printAvailability, "text");

        #endregion
        pnlforlistbox.Visible = false;
    }


    #endregion

    public void loadspreaddetails(FpSpread FpSpread1, string pagename, string degreedetails, byte print = 0, string userCode = null)
    {

       

        setstyle();
        chkcolour.Checked = true;
        column_header_row_count_orgi = Convert.ToInt16(Session["column_header_row_count"]);
        errmsg.Visible = false;
        pnlforlistbox.Visible = (print != 0) ? false : true;

        string isColor = da.GetFunction("select isColor from tbl_print_master_settings where page_Name='" + pagename + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");

        Session["Degree"] = degreedetails;
       // radioheader.Items[0].Selected = true;
       // radiofooter.Items[0].Selected = true;
        Session["Pagename"] = pagename;
        FpFooter.Enabled = false;
        btnok.Enabled = false;
        treeview_spreadfields.Nodes.Clear();
        fpspreadsample = (FpSpread)FpSpread1;
        fpspreadsample = FpSpread1;
        Chk_sel.Checked = false;
        Session["user_code"] = "";
        if (!string.IsNullOrEmpty(userCode))
            Session["user_code"] = userCode;
        int total_clmn_count = fpspreadsample.Sheets[0].ColumnCount;
        int headrow = fpspreadsample.Sheets[0].ColumnHeader.RowCount;
        int spancolumn = 0;
        int spanrow = 0;
        string Columnname = "";
        string columnval = "";
        int srtcnt = 0;
        TreeNode subnode;
        TreeNode subchildnode;
        node = null;
        childnode = null;
        for (srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        {
            if (fpspreadsample.Sheets[0].Columns[srtcnt].Visible == true)
            {
                if (fpspreadsample.Sheets[0].ColumnHeader.Cells[0, srtcnt].Text != "")
                {
                    spancolumn = Convert.ToInt32(fpspreadsample.Sheets[0].ColumnHeader.Cells[0, srtcnt].ColumnSpan);
                    spanrow = Convert.ToInt32(fpspreadsample.Sheets[0].ColumnHeader.Cells[0, srtcnt].RowSpan);
                    Columnname = fpspreadsample.Sheets[0].ColumnHeader.Cells[0, srtcnt].Text.ToString();
                    string[] sp = Columnname.Split('&');
                    if (sp.GetUpperBound(0) > 0)
                    {
                        string gv = "";
                        for (int s = 0; s <= sp.GetUpperBound(0); s++)
                        {
                            if (gv == "")
                            {
                                gv = sp[s].ToString();
                            }
                            else
                            {
                                gv = gv + " and " + sp[s].ToString();
                            }
                        }
                        Columnname = gv;
                    }
                    columnval = srtcnt.ToString();
                    node = new TreeNode(Columnname, columnval);
                    for (int spancol = srtcnt; spancol <= srtcnt + spancolumn - 1; spancol++)
                    {

                        if (spancol < total_clmn_count)
                        {
                            if (headrow > spanrow)
                            {
                                for (int row = 1; row < headrow; row++)
                                {
                                    if (fpspreadsample.Sheets[0].Columns[spancol].Visible == true)
                                    {
                                        if (fpspreadsample.Sheets[0].ColumnHeader.Cells[row, spancol].Text != "")
                                        {
                                            string rowColumnname = fpspreadsample.Sheets[0].ColumnHeader.Cells[row, spancol].Text.ToString();
                                            string rowcolumnval = spancol.ToString();
                                            int spanrowcol = Convert.ToInt32(fpspreadsample.Sheets[0].ColumnHeader.Cells[row, spancol].ColumnSpan);
                                            int spanrowrow = Convert.ToInt32(fpspreadsample.Sheets[0].ColumnHeader.Cells[row, spancol].RowSpan);
                                            childnode = new TreeNode(rowColumnname, rowcolumnval);
                                            childnode.ShowCheckBox = true;

                                            node.ChildNodes.Add(childnode);
                                            for (int subcol = spancol; subcol <= spancol + spanrowcol - 1; subcol++)
                                            {
                                                if (spanrowcol >= 1)
                                                {
                                                    for (int subrow = row + 1; subrow < headrow; subrow++)
                                                    {
                                                        if (fpspreadsample.Sheets[0].Columns[subcol].Visible == true)
                                                        {
                                                            if (fpspreadsample.Sheets[0].ColumnHeader.Cells[subrow, subcol].Text != "")
                                                            {
                                                                string subrowColumnname = fpspreadsample.Sheets[0].ColumnHeader.Cells[subrow, subcol].Text.ToString();
                                                                string subrowcolumnval = subcol.ToString();
                                                                int subspanrowcol = Convert.ToInt32(fpspreadsample.Sheets[0].ColumnHeader.Cells[subrow, subcol].ColumnSpan);
                                                                int subspanrowrow = Convert.ToInt32(fpspreadsample.Sheets[0].ColumnHeader.Cells[subrow, subcol].RowSpan);
                                                                subnode = new TreeNode(subrowColumnname, subrowcolumnval);
                                                                subnode.ShowCheckBox = true;
                                                                childnode.ChildNodes.Add(subnode);
                                                                subcol = subcol + subspanrowcol - 1;
                                                                for (int subspancol = srtcnt; subspancol <= subcol + subspanrowcol - 1; subspancol++)
                                                                {

                                                                    if (subspanrowrow > 1)
                                                                    {
                                                                        for (int subchirow = subrow + 1; subchirow < headrow; subchirow++)
                                                                        {
                                                                            if (fpspreadsample.Sheets[0].Columns[subspancol].Visible == true)
                                                                            {
                                                                                if (fpspreadsample.Sheets[0].ColumnHeader.Cells[subchirow, subspancol].Text != "")
                                                                                {
                                                                                    string childsubrowColumnname = fpspreadsample.Sheets[0].ColumnHeader.Cells[subchirow, subspancol].Text.ToString();
                                                                                    string childsubrowcolumnval = spancol.ToString();
                                                                                    int childsubspanrowcol = Convert.ToInt32(fpspreadsample.Sheets[0].ColumnHeader.Cells[subchirow, subspancol].ColumnSpan);
                                                                                    int childsubspanrowrow = Convert.ToInt32(fpspreadsample.Sheets[0].ColumnHeader.Cells[subchirow, subspancol].RowSpan);
                                                                                    subchildnode = new TreeNode(childsubrowColumnname, childsubrowcolumnval);
                                                                                    subchildnode.ShowCheckBox = true;
                                                                                    subnode.ChildNodes.Add(subchildnode);
                                                                                    subspancol = subspancol + childsubspanrowcol - 1;
                                                                                }
                                                                            }
                                                                            subspancol = subspancol + subspanrowcol - 1;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                            row = 2;
                                            spancol = spancol + spanrowcol - 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    node.ShowCheckBox = true;
                    treeview_spreadfields.Nodes.Add(node);
                }
                if (spancolumn > 1)
                {
                    srtcnt = srtcnt + spancolumn - 1;
                }
            }
        }

        string selectedPrintFields = da.GetFunction("select print_fields from tbl_print_master_settings where page_Name='" + pagename + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
        string[] splitselectedPrintFields = selectedPrintFields.Split('#');
        Chk_sel.Checked = false;
        if (splitselectedPrintFields.Length > 0)
        {
            int seleCount = 0;
            int selTotCount = 0;
            for (int parenT = 0; parenT < treeview_spreadfields.Nodes.Count; parenT++)
            {
                selTotCount++;
                int childRowCount = 0;
                int newChildRowCount = 0;
                int a = 0;
                for (int val = 0; val < splitselectedPrintFields.Length; val++)
                {
                    string printField = Convert.ToString(splitselectedPrintFields[val]);
                    if (printField == treeview_spreadfields.Nodes[parenT].Text)
                    {
                        treeview_spreadfields.Nodes[parenT].Checked = true;
                        seleCount++;
                    }
                    else if (printField.Contains("^"))
                    {
                        if (printField.Split('^')[0] == treeview_spreadfields.Nodes[parenT].Text)
                        {

                            int totChildCount = 0;
                            int.TryParse(Convert.ToString(treeview_spreadfields.Nodes[parenT].ChildNodes.Count), out totChildCount);
                            for (int childRow = 0; childRow < treeview_spreadfields.Nodes[parenT].ChildNodes.Count; childRow++)
                            {
                                if (printField.Contains("@"))
                                {
                                    string strChild = printField.Split('@')[0];
                                    if (strChild.Split('^')[1] == treeview_spreadfields.Nodes[parenT].ChildNodes[childRow].Text)
                                    {
                                        int totNewChildCount = 0;
                                        int.TryParse(Convert.ToString(treeview_spreadfields.Nodes[parenT].ChildNodes[childRow].ChildNodes.Count), out totNewChildCount);
                                        for (int newChildRow = 0; newChildRow < treeview_spreadfields.Nodes[parenT].ChildNodes[childRow].ChildNodes.Count; newChildRow++)
                                        {
                                            if (printField.Split('@')[1] == treeview_spreadfields.Nodes[parenT].ChildNodes[childRow].ChildNodes[newChildRow].Text)
                                            {
                                                treeview_spreadfields.Nodes[parenT].ChildNodes[childRow].ChildNodes[newChildRow].Checked = true;
                                                newChildRowCount++;

                                            }
                                        }
                                        if (totNewChildCount == newChildRowCount)
                                        {
                                            treeview_spreadfields.Nodes[parenT].ChildNodes[childRow].Checked = true;
                                            childRowCount++;
                                        }
                                    }
                                }
                                else
                                {
                                    if (printField.Split('^')[1] == treeview_spreadfields.Nodes[parenT].ChildNodes[childRow].Text)
                                    {
                                        treeview_spreadfields.Nodes[parenT].ChildNodes[childRow].Checked = true;
                                        childRowCount++;
                                    }
                                }
                                //if (printField.Split('^')[1] == treeview_spreadfields.Nodes[parenT].ChildNodes[childRow].Text)
                                //{
                                //    treeview_spreadfields.Nodes[parenT].ChildNodes[childRow].Checked = true;
                                //    childRowCount++;
                                //}
                            }
                            if (totChildCount == childRowCount)
                            {
                                treeview_spreadfields.Nodes[parenT].Checked = true;
                                seleCount++;
                            }
                        }

                    }
                }

            }
            if (selTotCount == seleCount)
                Chk_sel.Checked = true;

        }



        string collegedetails = da.GetFunction("select college_details from tbl_print_master_settings where page_Name='" + pagename + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
        string[] spiltcollegedetails = collegedetails.Split('#');
        for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
        {
            string collinfo = spiltcollegedetails[i].ToString();
            if (collinfo == "College Name")
            {
                chkcollege.Items[0].Selected = true;

            }
            else if (collinfo == "University")
            {
                chkcollege.Items[1].Selected = true;
            }
            else if (collinfo == "Affliated By")
            {
                chkcollege.Items[2].Selected = true;
            }
            else if (collinfo == "Address")
            {
                chkcollege.Items[3].Selected = true;
            }
            else if (collinfo == "City")
            {
                chkcollege.Items[4].Selected = true;
            }
            else if (collinfo == "District & State & Pincode")
            {
                chkcollege.Items[5].Selected = true;
            }
            else if (collinfo == "Phone No & Fax")
            {
                chkcollege.Items[6].Selected = true;
            }
            else if (collinfo == "Email & Web Site")
            {
                chkcollege.Items[7].Selected = true;
            }
            else if (collinfo == "Right Logo")
            {
                chkcollege.Items[8].Selected = true;
            }
            else if (collinfo == "Left Logo")
            {
                chkcollege.Items[9].Selected = true;
            }
            else if (collinfo == "Signature")
            {
                chkcollege.Items[10].Selected = true;
            }
        }
        string headerlevel = da.GetFunction("select header_level from tbl_print_master_settings where page_Name='" + pagename + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
        if (headerlevel != "" && headerlevel != "0")
        {
            if (headerlevel == "All Pages")
            {
                radioheader.Items[0].Selected = true;
            }
            else if (headerlevel == "First Page")
            {
                radioheader.Items[1].Selected = true;
            }
            else if (headerlevel == "No Header")
            {
                radioheader.Items[2].Selected = true;
            }
        }
        else
        {
            radioheader.Items[0].Selected = true;
        }
        string footerlevel = da.GetFunction("select footer_level from tbl_print_master_settings where page_Name='" + pagename + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
        if (footerlevel != "" && footerlevel != "0")
        {
            if (footerlevel == "All Pages")
            {
                radiofooter.Items[0].Selected = true;
            }
            else if (footerlevel == "Last Page")
            {
                radiofooter.Items[1].Selected = true;
            }
            else if (footerlevel == "No Footer")
            {
                radiofooter.Items[2].Selected = true;
            }
        }
        else
        {
            radiofooter.Items[0].Selected = true;
        }
        ddlsection.SelectedIndex = 0;
        string noofrowperpage = "select with_out_header_no_row_pages,Head_Style,page_settings from tbl_print_master_settings where page_Name='" + pagename + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
        DataSet dscaculate = da.select_method_wo_parameter(noofrowperpage, "Text");
        if (dscaculate.Tables[0].Rows.Count > 0)
        {
            string styleva = dscaculate.Tables[0].Rows[0]["Head_Style"].ToString();
            if (styleva != null && styleva.Trim() != "" && styleva.Trim() != "0")
            {
                string[] stylespilt = styleva.Trim().Split(',');
                if (stylespilt.GetUpperBound(0) == 1)
                {
                    string fontname = stylespilt[0].ToString();
                    string fontsize = stylespilt[1].ToString();
                    ddlfont.Text = fontname;
                    ddlsize.Text = fontsize;
                }
            }
            noofrowperpage = dscaculate.Tables[0].Rows[0]["with_out_header_no_row_pages"].ToString();
            if (noofrowperpage != "" && noofrowperpage != "0" && noofrowperpage != null)
            {
                txtnofrow.Text = noofrowperpage;
            }
            else
            {
                if (ddlorientation.Text == "Portrait")
                {
                    txtnofrow.Text = "30";
                }
                else
                {
                    txtnofrow.Text = "25";
                }
            }
            string strsettingval = dscaculate.Tables[0].Rows[0]["page_settings"].ToString();
            chkfitpaper.Checked = false;
            txtpading.Text = "";
            if (strsettingval.Trim() != "" && strsettingval != "0")
            {
                string[] stap = strsettingval.Split('@');
                if (stap.GetUpperBound(0) == 1)
                {
                    string setpadsize = stap[0].ToString();
                    if (setpadsize.Trim() != "")
                    {
                        txtpading.Text = setpadsize;
                    }
                    string setfitpa = stap[1].ToString();
                    if (setfitpa.Trim() != "" && setfitpa.Trim() != "0")
                    {
                        chkfitpaper.Checked = true;
                    }
                }
            }
        }
        //btnprint_Click(new object(), new EventArgs());
        if (print != 0)
        {
            chkcolour.Checked = (!string.IsNullOrEmpty(isColor) && (isColor.Trim() == "1" || isColor.Trim().ToLower() == "true")) ? true : false;
           btnprint_Click(new object(), new EventArgs());
        }
        else 
        {
            chkcolour.Checked = (!string.IsNullOrEmpty(isColor) && (isColor.Trim() == "1" || isColor.Trim().ToLower() == "true")) ? true : false;
        }
       
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {

        try
        {

            #region printlock 
            string statusofPrintAvailability = da.GetFunction("select distinct TextVal from TextValTable where TextCriteria='prtlk'");
            if (!String.IsNullOrEmpty(statusofPrintAvailability) && statusofPrintAvailability == "1")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Try Again Later";
                return;
            }

            string updateqry = "update TextValTable set TextVal='1' where TextCriteria='prtlk'";
            int res = da.update_method_wo_parameter(updateqry, "text");

            #endregion


            string selectedPrintfields = "";
            string printField = "";
            string DegreeDetails = "";
            int printrow = 0;
            int startrowfp = 0;
            errmsg.Visible = false;
            string Headername = "";
            string Columname = "";
            int columncount = 0;
            DataSet dssign = new DataSet();
            DataSet MyDs = new DataSet();
            DAccess2 d2 = new DAccess2();

            Boolean fistrowselect = false;
            Boolean secondrowselect = false;
            Boolean thirdrowselect = false;
            Gios.Pdf.PdfTablePage newpdftabpage;

            

            for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
            {
                string columnvalue = "";
                if (treeview_spreadfields.Nodes[remv].Checked == true)
                {
                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                        {
                            if (treeview_spreadfields.Nodes[remv].ChildNodes[child].Text != "")
                            {
                                fistrowselect = true;
                                Columname = treeview_spreadfields.Nodes[remv].Text + "^" + treeview_spreadfields.Nodes[remv].ChildNodes[child].Text;
                                columnvalue = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value;
                                printField = treeview_spreadfields.Nodes[remv].Text + "^" + treeview_spreadfields.Nodes[remv].ChildNodes[child].Text;
                                if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes.Count > 0)
                                {
                                    for (int chl1 = 0; chl1 < treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes.Count; chl1++)
                                    {
                                        if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Text != "")
                                        {
                                            secondrowselect = true;
                                            string thirdrow = Columname + '#' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Text;
                                            columnvalue = treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Value;
                                            string firstPrintSubChild = printField + '@' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Text;
                                            if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes.Count > 0)
                                            {
                                                for (int chl2 = 0; chl2 < treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes.Count; chl2++)
                                                {
                                                    thirdrowselect = true;
                                                    Columname = thirdrow + '~' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Text;
                                                    columnvalue = treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Value;
                                                    printField = firstPrintSubChild + '~' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Text;
                                                    if (Headername == "")
                                                    {
                                                        columncount++;
                                                        Headername = Columname + '&' + columnvalue;
                                                    }
                                                    else
                                                    {
                                                        columncount++;
                                                        Headername = Headername + '@' + Columname + '&' + columnvalue;
                                                    }

                                                    if (selectedPrintfields == "")
                                                    {

                                                        selectedPrintfields = printField;
                                                    }
                                                    else
                                                    {

                                                        selectedPrintfields = selectedPrintfields + '#' + printField;
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                thirdrowselect = true;
                                                if (Headername == "")
                                                {
                                                    columncount++;
                                                    Headername = thirdrow + '&' + columnvalue;
                                                }
                                                else
                                                {
                                                    columncount++;
                                                    Headername = Headername + '@' + thirdrow + '&' + columnvalue;
                                                }

                                                if (selectedPrintfields == "")
                                                {

                                                    selectedPrintfields = firstPrintSubChild;
                                                }
                                                else
                                                {

                                                    selectedPrintfields = selectedPrintfields + '#' + firstPrintSubChild;
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    secondrowselect = true;
                                    if (Headername == "")
                                    {
                                        columncount++;
                                        Headername = Columname + '&' + columnvalue;
                                    }
                                    else
                                    {
                                        columncount++;
                                        Headername = Headername + '@' + Columname + '&' + columnvalue;
                                    }

                                    if (selectedPrintfields == "")
                                    {

                                        selectedPrintfields = printField;
                                    }
                                    else
                                    {

                                        selectedPrintfields = selectedPrintfields + '#' + printField;
                                    }


                                }

                            }
                        }
                    }
                    else
                    {
                        fistrowselect = true;
                        Columname = treeview_spreadfields.Nodes[remv].Text;
                        printField = treeview_spreadfields.Nodes[remv].Text;
                        columnvalue = treeview_spreadfields.Nodes[remv].Value;
                        if (Headername == "")
                        {
                            columncount++;
                            Headername = Columname + '&' + columnvalue;
                        }
                        else
                        {
                            columncount++;
                            Headername = Headername + '@' + Columname + '&' + columnvalue;
                        }

                        if (selectedPrintfields == "")
                        {

                            selectedPrintfields = printField;
                        }
                        else
                        {

                            selectedPrintfields = selectedPrintfields + '#' + printField;
                        }

                    }

                }
                else
                {

                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                        {
                            if (treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked == true)
                            {
                                secondrowselect = true;
                                Columname = treeview_spreadfields.Nodes[remv].Text + "^" + treeview_spreadfields.Nodes[remv].ChildNodes[child].Text;
                                columnvalue = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value;

                                printField = treeview_spreadfields.Nodes[remv].Text + "^" + treeview_spreadfields.Nodes[remv].ChildNodes[child].Text;

                                if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes.Count > 0)
                                {
                                    for (int chl1 = 0; chl1 < treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes.Count; chl1++)
                                    {
                                        if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Checked == true)
                                        {
                                            thirdrowselect = true;
                                            string secondcolumn = Columname + '#' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Text;
                                            columnvalue = treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Value;

                                            string firstPrintSubChild = printField + '@' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Text;

                                            if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes.Count > 0)
                                            {
                                                for (int chl2 = 0; chl2 < treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes.Count; chl2++)
                                                {
                                                    if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Checked == true)
                                                    {
                                                        string thirdcolum = secondcolumn + '~' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Text;
                                                        columnvalue = treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Value;

                                                        string secondPrintSubChild = firstPrintSubChild + '~' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Text;
                                                        if (Headername == "")
                                                        {
                                                            columncount++;
                                                            Headername = thirdcolum + '&' + columnvalue;
                                                        }
                                                        else
                                                        {
                                                            columncount++;
                                                            Headername = Headername + '@' + thirdcolum + '&' + columnvalue;
                                                        }

                                                        if (selectedPrintfields == "")
                                                        {

                                                            selectedPrintfields = secondPrintSubChild;
                                                        }
                                                        else
                                                        {

                                                            selectedPrintfields = selectedPrintfields + '#' + secondPrintSubChild;
                                                        }



                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Headername == "")
                                                {
                                                    columncount++;
                                                    Headername = secondcolumn + '&' + columnvalue;
                                                }
                                                else
                                                {
                                                    columncount++;
                                                    Headername = Headername + '@' + secondcolumn + '&' + columnvalue;
                                                }

                                                if (selectedPrintfields == "")
                                                {

                                                    selectedPrintfields = firstPrintSubChild;
                                                }
                                                else
                                                {

                                                    selectedPrintfields = selectedPrintfields + '#' + firstPrintSubChild;
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Headername == "")
                                    {
                                        columncount++;
                                        Headername = Columname + '&' + columnvalue;
                                    }
                                    else
                                    {
                                        columncount++;
                                        Headername = Headername + '@' + Columname + '&' + columnvalue;
                                    }

                                    if (selectedPrintfields == "")
                                    {

                                        selectedPrintfields = printField;
                                    }
                                    else
                                    {

                                        selectedPrintfields = selectedPrintfields + '#' + printField;
                                    }
                                }

                            }
                            else
                            {
                                if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes.Count > 0)
                                {
                                    Columname = treeview_spreadfields.Nodes[remv].Text + "^" + treeview_spreadfields.Nodes[remv].ChildNodes[child].Text;
                                    printField = treeview_spreadfields.Nodes[remv].Text + "^" + treeview_spreadfields.Nodes[remv].ChildNodes[child].Text;
                                    for (int chl1 = 0; chl1 < treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes.Count; chl1++)
                                    {
                                        if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Checked == true)
                                        {
                                            thirdrowselect = true;
                                            string thirdcolumn = Columname + '#' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Text;
                                            columnvalue = treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Value;

                                            string firstPrintSubChild = printField + '@' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Text;
                                            if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes.Count > 0)
                                            {
                                                for (int chl2 = 0; chl2 < treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes.Count; chl2++)
                                                {
                                                    if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Checked == true)
                                                    {
                                                        thirdcolumn = Columname + '~' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Text;
                                                        columnvalue = treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Value;
                                                        string secondPrintSubChild = printField + '~' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Text;
                                                        if (Headername == "")
                                                        {
                                                            columncount++;
                                                            Headername = thirdcolumn + '&' + columnvalue;
                                                        }
                                                        else
                                                        {
                                                            columncount++;
                                                            Headername = Headername + '@' + thirdcolumn + '&' + columnvalue;
                                                        }

                                                        if (selectedPrintfields == "")
                                                        {

                                                            selectedPrintfields = secondPrintSubChild;
                                                        }
                                                        else
                                                        {

                                                            selectedPrintfields = selectedPrintfields + '#' + secondPrintSubChild;
                                                        }


                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Headername == "")
                                                {
                                                    columncount++;
                                                    Headername = thirdcolumn + '&' + columnvalue;
                                                }
                                                else
                                                {
                                                    columncount++;
                                                    Headername = Headername + '@' + thirdcolumn + '&' + columnvalue;
                                                }

                                                if (selectedPrintfields == "")
                                                {

                                                    selectedPrintfields = firstPrintSubChild;
                                                }
                                                else
                                                {

                                                    selectedPrintfields = selectedPrintfields + '#' + firstPrintSubChild;
                                                }


                                            }
                                        }
                                        else
                                        {
                                            if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes.Count > 0)
                                            {
                                                for (int chl2 = 0; chl2 < treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes.Count; chl2++)
                                                {
                                                    Columname = treeview_spreadfields.Nodes[remv].Text + "^" + treeview_spreadfields.Nodes[remv].ChildNodes[child].Text + '~' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Text;
                                                    printField = treeview_spreadfields.Nodes[remv].Text + "^" + treeview_spreadfields.Nodes[remv].ChildNodes[child].Text + '@' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].Text;
                                                    if (treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Checked == true)
                                                    {
                                                        Columname = Columname + '~' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Text;
                                                        columnvalue = treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Value;
                                                        printField = printField + '~' + treeview_spreadfields.Nodes[remv].ChildNodes[child].ChildNodes[chl1].ChildNodes[chl2].Text;
                                                        if (Headername == "")
                                                        {
                                                            columncount++;
                                                            Headername = Columname + '&' + columnvalue;
                                                        }
                                                        else
                                                        {
                                                            columncount++;
                                                            Headername = Headername + '@' + Columname + '&' + columnvalue;
                                                        }

                                                        if (selectedPrintfields == "")
                                                        {

                                                            selectedPrintfields = printField;
                                                        }
                                                        else
                                                        {

                                                            selectedPrintfields = selectedPrintfields + '#' + printField;
                                                        }


                                                        Columname = "";
                                                        printField = "";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            if (treeview_spreadfields.Nodes.Count == 0)
            {
                if (fpspreadsample.Sheets[0].ColumnHeaderVisible == false)
                {
                    Headername = "&0";
                }
            }
            if (Headername != "")
            {
                column_header_row_count_orgi = Convert.ToInt16(Session["column_header_row_count"]);
                if (fistrowselect == true)
                {
                    column_header_row_count_orgi = 1;
                }
                if (secondrowselect == true)
                {
                    column_header_row_count_orgi = 2;
                }
                if (thirdrowselect == true)
                {
                    column_header_row_count_orgi = 3;
                }
                string tempvalue = "";
                int tempspan = 0;
                int[] tablewidth = new int[columncount];
                Boolean headflag = true;
                Boolean footflag = false;
                string collegedetails = "";
                //string selectedPrintFields = "";
                Boolean pagesizeflag = false;
                Hashtable hatspancol = new Hashtable();
                if (ddlorientation.SelectedItem.Text == "Landscape")
                {
                    pagesizeflag = true;
                }
                if (radioheader.SelectedItem.ToString() == "No Header")
                {
                    headflag = false;
                }
                if (radiofooter.SelectedItem.ToString().Trim() == "All Pages")
                {
                    footflag = true;
                }
                string strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                ds = da.select_method(strquery, hat_print, "Text");
                string strpagesize = ddlpagesize.SelectedItem.ToString();
                int headalign = 800;
                int pagecount = Convert.ToInt32(fpspreadsample.Sheets[0].RowCount) / 50;
                int repage = Convert.ToInt32(fpspreadsample.Sheets[0].RowCount) % 50;
                int nopages = pagecount;
                int nexthead = 0;
                int fontsize = 0;
                Gios.Pdf.PdfDocument mydoc;
                Font Fonthead;
                Font FontBodyhead;
                Font FontBody;
                Font Fonttablehead;
                int collnamesize = 0;
                Boolean space = false;
                string collfontname = "Book Antiqua";
                int isox = 580;

                string padingleg = txtpading.Text.ToString();
                Double padval = 0;
                string pagesetting = "";
                if (padingleg.Trim() != "")
                {
                    padval = Convert.ToDouble(padingleg);
                    pagesetting = padingleg;
                }
                pagesetting = padingleg + "@0";
                if (chkfitpaper.Checked == true)
                {
                    pagesetting = padingleg + "@1";
                }

                if (strpagesize == "A3")
                {

                    if (pagesizeflag == true)
                    {
                        headalign = 1200;
                        mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(16.5, 11.7));
                        Fonthead = new Font("Book Antiqua", 6, FontStyle.Bold);
                        FontBody = new Font("Book Antiqua", 5, FontStyle.Regular);
                        FontBodyhead = new Font("Book Antiqua", 5, FontStyle.Bold);
                        Fonttablehead = new Font("Book Antiqua", 5, FontStyle.Bold);
                        nexthead = 10;
                        fontsize = 5;
                        isox = 880;
                        collnamesize = 12;
                    }
                    else
                    {
                        headalign = 1700;
                        mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(60, 40));
                        Fonthead = new Font("Book Antiqua", 7, FontStyle.Bold);
                        FontBody = new Font("Book Antiqua", 6, FontStyle.Regular);
                        FontBodyhead = new Font("Book Antiqua", 6, FontStyle.Bold);
                        Fonttablehead = new Font("Book Antiqua", 6, FontStyle.Bold);
                        nexthead = 10;
                        fontsize = 6;
                        isox = 1300;
                        collnamesize = 14;
                    }
                }
                else if (strpagesize == "A4")
                {
                    headalign = 800;
                    isox = 580;
                    if (pagesizeflag == true)
                    {
                        mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
                        Fonthead = new Font("Book Antiqua", 7, FontStyle.Bold);
                        FontBody = new Font("Book Antiqua", 5, FontStyle.Regular);
                        FontBodyhead = new Font("Book Antiqua", 5, FontStyle.Bold);
                        Fonttablehead = new Font("Book Antiqua", 5, FontStyle.Bold);
                        nexthead = 10;
                        fontsize = 5;
                        collnamesize = 14;
                    }
                    else
                    {
                        mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
                        Fonthead = new Font("Book Antiqua", 10, FontStyle.Bold);
                        FontBody = new Font("Book Antiqua", 8, FontStyle.Regular);
                        FontBodyhead = new Font("Book Antiqua", 8, FontStyle.Bold);
                        Fonttablehead = new Font("Book Antiqua", 8, FontStyle.Bold);
                        nexthead = 10;
                        fontsize = 8;
                        collnamesize = 20;
                    }
                }
                else
                {
                    if (pagesizeflag == true)
                    {
                        mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(20, 30));
                        Fonthead = new Font("Book Antiqua", 11, FontStyle.Bold);
                        FontBody = new Font("Book Antiqua", 9, FontStyle.Regular);
                        FontBodyhead = new Font("Book Antiqua", 9, FontStyle.Bold);
                        Fonttablehead = new Font("Book Antiqua", 9, FontStyle.Bold);
                        nexthead = 10;
                        fontsize = 9;
                        collnamesize = 22;
                    }
                    else
                    {
                        mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 20));
                        Fonthead = new Font("Book Antiqua", 18, FontStyle.Bold);
                        FontBody = new Font("Book Antiqua", 16, FontStyle.Regular);
                        FontBodyhead = new Font("Book Antiqua", 16, FontStyle.Bold);
                        Fonttablehead = new Font("Book Antiqua", 16, FontStyle.Bold);
                        nexthead = 15;
                        fontsize = 16;
                        collnamesize = 36;
                    }
                }
                int noofrowsperpage = 0;
                string noofrow = da.GetFunction("select with_out_header_no_row_pages from tbl_print_master_settings where page_Name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
                if (noofrow != "" && noofrow != "0" && noofrow != null)
                {
                    noofrowsperpage = Convert.ToInt32(noofrow);
                }
                else
                {
                    if (txtnofrow.Text != "" && txtnofrow.Text != "0" && txtnofrow.Text != null)
                    {
                        noofrowsperpage = Convert.ToInt32(txtnofrow.Text);
                    }
                    else
                    {
                        if (ddlorientation.Text == "Portrait")
                        {
                            noofrowsperpage = 45;
                        }
                        else
                        {
                            noofrowsperpage = 25;
                        }
                    }
                }

                DataSet dsstyle = da.select_method("select Head_Style,Body_Style,Foot_Style from tbl_print_master_settings where Page_Name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'", hat_print, "Text");
                if (dsstyle.Tables[0].Rows.Count > 0)
                {
                    if (dsstyle.Tables[0].Rows[0]["Head_Style"].ToString().Trim() != "" && dsstyle.Tables[0].Rows[0]["Head_Style"].ToString().Trim() != null)
                    {
                        string[] stylespilt = dsstyle.Tables[0].Rows[0]["Head_Style"].ToString().Trim().Split(',');
                        if (stylespilt.GetUpperBound(0) == 1)
                        {
                            Fonthead = new Font(stylespilt[0], Convert.ToInt32(stylespilt[1]), FontStyle.Bold);
                            nexthead = Convert.ToInt32(stylespilt[1]);
                            collnamesize = nexthead * 2;
                            collfontname = stylespilt[0];
                        }
                    }
                    if (dsstyle.Tables[0].Rows[0]["Body_Style"].ToString().Trim() != "" && dsstyle.Tables[0].Rows[0]["Body_Style"].ToString().Trim() != null)
                    {
                        string[] stylespilt = dsstyle.Tables[0].Rows[0]["Body_Style"].ToString().Trim().Split(',');
                        if (stylespilt.GetUpperBound(0) == 1)
                        {
                            FontBody = new Font(stylespilt[0], Convert.ToInt32(stylespilt[1]), FontStyle.Regular);
                            Fonttablehead = new Font(stylespilt[0], Convert.ToInt32(stylespilt[1]), FontStyle.Bold);
                            fontsize = Convert.ToInt32(stylespilt[1]);

                        }
                    }
                    if (dsstyle.Tables[0].Rows[0]["Foot_Style"].ToString().Trim() != "" && dsstyle.Tables[0].Rows[0]["Foot_Style"].ToString().Trim() != null)
                    {
                        string[] stylespilt = dsstyle.Tables[0].Rows[0]["Foot_Style"].ToString().Trim().Split(',');
                        if (stylespilt.GetUpperBound(0) == 1)
                        {
                            FontBodyhead = new Font(stylespilt[0], Convert.ToInt32(stylespilt[1]), FontStyle.Bold);
                        }
                    }
                }

                if (repage > 0)
                {
                    nopages++;
                }
                if (nopages > 0)
                {
                    int value = 0;
                    int page = 0;
                    int totalrow = 0;
                    int visiblerow = 0;
                    for (int vr = 0; vr < fpspreadsample.Sheets[0].RowCount; vr++)
                    {
                        if (fpspreadsample.Sheets[0].Rows[vr].Visible == true)
                        {
                            visiblerow++;
                        }
                    }
                    string isiso = da.GetFunction("select isocode from tbl_print_master_settings where page_name='cumreport.aspx' and usercode='" + Convert.ToString(Session["user_code"]) + "'");

                    int srno = 0;
                    int norow = 0;
                    for (int row = 0; row < nopages; row++)
                    {

                        if (row > 150)
                        {
                            row = nopages + nopages;
                            nopages = 0;
                        }
                        if (headflag == true)
                        {
                            string noofrow1 = da.GetFunction("select with_header_no_row_pages from tbl_print_master_settings where page_Name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
                            if (noofrow1 != "" && noofrow1 != "0" && noofrow1 != null)
                            {
                                noofrowsperpage = Convert.ToInt32(noofrow1);
                            }
                            page = page + noofrowsperpage;
                            value = page - noofrowsperpage;

                        }
                        else
                        {
                            string noofrow1 = da.GetFunction("select with_out_header_no_row_pages from tbl_print_master_settings where page_Name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
                            if (noofrow1 != "" && noofrow1 != "0" && noofrow1 != null)
                            {
                                noofrowsperpage = Convert.ToInt32(noofrow1);
                            }
                            page = page + noofrowsperpage;
                            value = page - noofrowsperpage;
                        }
                        //if (visiblerow < page)
                        //{
                        //    page = visiblerow;
                        //}
                        if (value < fpspreadsample.Sheets[0].RowCount)
                        {
                            int width = 0;
                            int collheader = 0;
                            if (radiofooter.SelectedItem.ToString() == "Last Page")
                            {
                                if (row == nopages - 1)
                                {
                                    footflag = true;
                                }
                            }
                            if (page == fpspreadsample.Sheets[0].RowCount - 1)
                            {
                                if (value >= visiblerow)
                                {
                                    row = nopages + nopages;
                                }
                            }

                            int coltop = 0;
                            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();


                            if (headflag == true)
                            {
                                if (chkSetCommPrint.Checked == true)
                                {
                                    MyDs.Clear();
                                    Gios.Pdf.PdfTable Mytable;
                                    Gios.Pdf.PdfTablePage pdftablePge;
                                    Font collnamehaed = new Font("Book Antiqua", 14, FontStyle.Regular);
                                    string ModName = Convert.ToString(Session["Module"]);
                                    string CollCode = Convert.ToString(Session["collegecode"]);
                                    int FontSize = 0;
                                    string Is_Bold = "0";
                                    string HeaderName = "";
                                    bool HdrChk = false;
                                    string isLeftLogo = "false";
                                    string isRightLogo = "false";
                                    int PdfHgt = 0;

                                    string selQ = "select * from Col_Hdr_Settings where Mod_Name='" + ModName + "' and college_code='" + CollCode + "'";
                                    try
                                    {
                                        MyDs = d2.select_method_wo_parameter(selQ, "Text");
                                        if (MyDs.Tables.Count > 0 && MyDs.Tables[0].Rows.Count > 0)
                                        {
                                            Mytable = mydoc.NewTable(collnamehaed, MyDs.Tables[0].Rows.Count, 1, 3);
                                            for (int mycol = 0; mycol < MyDs.Tables[0].Rows.Count; mycol++)
                                            {
                                                //if (mycol == 0)
                                                //    coltop = coltop + 20;
                                                //else
                                                //    coltop = coltop + nexthead;
                                                Int32.TryParse(Convert.ToString(MyDs.Tables[0].Rows[mycol]["Hdr_Font_Size"]), out FontSize);
                                                Is_Bold = Convert.ToString(MyDs.Tables[0].Rows[mycol]["Is_Bold"]);
                                                HeaderName = Convert.ToString(MyDs.Tables[0].Rows[mycol]["Hdr_Name"]);
                                                if (Is_Bold.Trim().ToLower() == "true" || Is_Bold.Trim() == "1")
                                                    collnamehaed = new Font("Book Antiqua", FontSize, FontStyle.Bold);
                                                else
                                                    collnamehaed = new Font("Book Antiqua", FontSize, FontStyle.Regular);
                                                if (HdrChk == false)
                                                {
                                                    isLeftLogo = Convert.ToString(MyDs.Tables[0].Rows[mycol]["Is_LeftLogo"]);
                                                    isRightLogo = Convert.ToString(MyDs.Tables[0].Rows[mycol]["Is_RightLogo"]);
                                                    HdrChk = true;
                                                }
                                                Mytable.Cell(mycol, 0).SetContent(HeaderName);
                                                Mytable.Cell(mycol, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                Mytable.Cell(mycol, 0).SetFont(collnamehaed);
                                                PdfHgt += 50;
                                                //PdfTextArea pts = new PdfTextArea(collnamehaed, System.Drawing.Color.Black,
                                                //               new PdfArea(mydoc, 0, coltop, mydoc.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, HeaderName);
                                                //mypdfpage.Add(pts);
                                            }
                                            coltop = coltop + nexthead;
                                            pdftablePge = Mytable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 0, coltop, mydoc.PageWidth, PdfHgt));
                                            mypdfpage.Add(pdftablePge);
                                            coltop = coltop + Convert.ToInt32(pdftablePge.Area.Height);
                                            if (isLeftLogo.Trim().ToLower() == "true")
                                            {
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg")))
                                                {
                                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg"));
                                                    if (strpagesize == "A3")
                                                        mypdfpage.Add(LogoImage, 25, 25, 500);
                                                    else
                                                        mypdfpage.Add(LogoImage, 25, 25, 400);
                                                }
                                            }
                                            if (isRightLogo.Trim().ToLower() == "true")
                                            {
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg")))
                                                {
                                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg"));
                                                    if (strpagesize == "A3")
                                                    {
                                                        if (pagesizeflag == true)
                                                            mypdfpage.Add(LogoImage, 1100, 25, 500);
                                                        else
                                                            mypdfpage.Add(LogoImage, 1600, 25, 500);
                                                    }
                                                    else
                                                    {
                                                        if (isiso.Trim() != "" && isiso.Trim() != "0" && isiso != null)
                                                            mypdfpage.Add(LogoImage, 630, 25, 400);
                                                        else
                                                            mypdfpage.Add(LogoImage, 720, 25, 400);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch { }
                                }

                                else  //Add here
                                {
                                    if (chkcollegeheader.Checked == false)
                                    {
                                        for (int parent = 0; parent < chkcollege.Items.Count; parent++)
                                        {
                                            if (chkcollege.Items[parent].Selected == true)
                                            {
                                                string Collvalue = "";
                                                string collinfo = chkcollege.Items[parent].Value;
                                                if (collinfo == "College Name")
                                                {

                                                    if (isiso.Trim() != "" && isiso.Trim() != "0" && isiso != null)
                                                    {
                                                        coltop = coltop + nexthead + 10;
                                                    }
                                                    else
                                                    {
                                                        coltop = coltop + nexthead;
                                                    }
                                                    Collvalue = ds.Tables[0].Rows[0]["collname"].ToString();
                                                    Font collnamehaed = new Font(collfontname, collnamesize, FontStyle.Bold);
                                                    PdfTextArea pts = new PdfTextArea(collnamehaed, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 0, coltop, headalign, 50), System.Drawing.ContentAlignment.MiddleCenter, Collvalue);
                                                    space = true;
                                                    mypdfpage.Add(pts);
                                                    collheader = collheader + 1;

                                                }
                                                else if (collinfo == "University")
                                                {
                                                    if (space == true)
                                                    {
                                                        coltop = coltop + nexthead * 2;
                                                        space = false;
                                                    }
                                                    else
                                                    {
                                                        coltop = coltop + nexthead;
                                                    }
                                                    string address1 = ds.Tables[0].Rows[0]["university"].ToString();
                                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                                    {
                                                        Collvalue = address1;
                                                    }

                                                    PdfTextArea pts = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 0, coltop, headalign, 50), System.Drawing.ContentAlignment.MiddleCenter, Collvalue);
                                                    mypdfpage.Add(pts);
                                                    collheader = collheader + 1;
                                                }
                                                else if (collinfo == "Affliated By")
                                                {
                                                    if (space == true)
                                                    {
                                                        coltop = coltop + nexthead * 2;
                                                        space = false;
                                                    }
                                                    else
                                                    {
                                                        coltop = coltop + nexthead;
                                                    }
                                                    string address1 = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                                                    string[] spat = address1.Split(',');
                                                    string srtaff = "";
                                                    if (spat.GetUpperBound(0) > 0)
                                                    {
                                                        for (int caf = 0; caf < spat.GetUpperBound(0); caf++)
                                                        {
                                                            string getaffval = spat[caf].ToString();
                                                            if (getaffval.Trim() != "")
                                                            {
                                                                if (srtaff == "")
                                                                {
                                                                    srtaff = getaffval;
                                                                }
                                                                else
                                                                {
                                                                    srtaff = srtaff + "," + getaffval;
                                                                }
                                                            }
                                                        }
                                                        address1 = srtaff;
                                                    }
                                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                                    {
                                                        Collvalue = address1;
                                                    }

                                                    PdfTextArea pts = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 0, coltop, headalign, 50), System.Drawing.ContentAlignment.MiddleCenter, Collvalue);
                                                    mypdfpage.Add(pts);
                                                    collheader = collheader + 1;
                                                }
                                                else if (collinfo == "Address")
                                                {
                                                    if (space == true)
                                                    {
                                                        coltop = coltop + nexthead * 2;
                                                        space = false;
                                                    }
                                                    else
                                                    {
                                                        coltop = coltop + nexthead;
                                                    }
                                                    string address1 = ds.Tables[0].Rows[0]["Address1"].ToString();
                                                    string address2 = ds.Tables[0].Rows[0]["Address2"].ToString();
                                                    string address3 = ds.Tables[0].Rows[0]["Address3"].ToString();
                                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                                    {
                                                        Collvalue = address1;
                                                    }
                                                    if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                                    {
                                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                                        {
                                                            Collvalue = Collvalue + ',' + address2;
                                                        }
                                                        else
                                                        {
                                                            Collvalue = address2;
                                                        }
                                                    }
                                                    if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                                    {
                                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                                        {
                                                            Collvalue = Collvalue + ',' + address3;
                                                        }
                                                        else
                                                        {
                                                            Collvalue = address3;
                                                        }
                                                    }

                                                    PdfTextArea pts = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 0, coltop, headalign, 50), System.Drawing.ContentAlignment.MiddleCenter, Collvalue);
                                                    mypdfpage.Add(pts);
                                                    collheader = collheader + 1;
                                                }
                                                else if (collinfo == "City")
                                                {
                                                    if (space == true)
                                                    {
                                                        coltop = coltop + nexthead * 2;
                                                        space = false;
                                                    }
                                                    else
                                                    {
                                                        coltop = coltop + nexthead;
                                                    }
                                                    string address1 = ds.Tables[0].Rows[0]["Address3"].ToString();
                                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                                    {
                                                        Collvalue = address1;
                                                    }

                                                    PdfTextArea pts = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 0, coltop, headalign, 50), System.Drawing.ContentAlignment.MiddleCenter, Collvalue);
                                                    mypdfpage.Add(pts);
                                                    collheader = collheader + 1;
                                                }
                                                else if (collinfo == "District & State & Pincode")
                                                {
                                                    if (space == true)
                                                    {
                                                        coltop = coltop + nexthead * 2;
                                                        space = false;
                                                    }
                                                    else
                                                    {
                                                        coltop = coltop + nexthead;
                                                    }
                                                    // Collvalue = ds.Tables[0].Rows[0]["district"].ToString() + " , " + ds.Tables[0].Rows[0]["State"].ToString() + " , " + ds.Tables[0].Rows[0]["Pincode"].ToString();
                                                    string district = ds.Tables[0].Rows[0]["district"].ToString();
                                                    string state = ds.Tables[0].Rows[0]["State"].ToString();
                                                    string pincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                                                    if (district.Trim() != "" && district != null && district.Length > 1)
                                                    {
                                                        Collvalue = district;
                                                    }
                                                    if (state.Trim() != "" && state != null && state.Length > 1)
                                                    {
                                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                                        {
                                                            Collvalue = Collvalue + ',' + state;
                                                        }
                                                        else
                                                        {
                                                            Collvalue = state;
                                                        }
                                                    }
                                                    if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                                    {
                                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                                        {
                                                            Collvalue = Collvalue + '-' + pincode;
                                                        }
                                                        else
                                                        {
                                                            Collvalue = pincode;
                                                        }
                                                    }
                                                    PdfTextArea pts = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 0, coltop, headalign, 50), System.Drawing.ContentAlignment.MiddleCenter, Collvalue);
                                                    mypdfpage.Add(pts);
                                                    collheader = collheader + 1;
                                                }

                                                else if (collinfo == "Phone No & Fax")
                                                {
                                                    if (space == true)
                                                    {
                                                        coltop = coltop + nexthead * 2;
                                                        space = false;
                                                    }
                                                    else
                                                    {
                                                        coltop = coltop + nexthead;
                                                    }
                                                    //Collvalue = "Phone : " + ds.Tables[0].Rows[0]["Phoneno"].ToString() + " , Fax :" + ds.Tables[0].Rows[0]["Faxno"].ToString();
                                                    string phone = ds.Tables[0].Rows[0]["Phoneno"].ToString();
                                                    string fax = ds.Tables[0].Rows[0]["Faxno"].ToString();
                                                    if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                                    {
                                                        Collvalue = "Phone :" + phone;
                                                    }
                                                    if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                                    {
                                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                                        {
                                                            Collvalue = Collvalue + " , Fax : " + fax;
                                                        }
                                                        else
                                                        {
                                                            Collvalue = "Fax :" + fax;
                                                        }
                                                    }

                                                    PdfTextArea pts = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 0, coltop, headalign, 50), System.Drawing.ContentAlignment.MiddleCenter, Collvalue);
                                                    mypdfpage.Add(pts);
                                                    collheader = collheader + 1;
                                                }
                                                else if (collinfo == "Email & Web Site")
                                                {
                                                    if (space == true)
                                                    {
                                                        coltop = coltop + nexthead * 2;
                                                        space = false;
                                                    }
                                                    else
                                                    {
                                                        coltop = coltop + nexthead;
                                                    }
                                                    string email = ds.Tables[0].Rows[0]["Email"].ToString();
                                                    string website = ds.Tables[0].Rows[0]["Website"].ToString();
                                                    if (email.Trim() != "" && email != null && email.Length > 1)
                                                    {
                                                        Collvalue = "Email :" + email;
                                                    }
                                                    if (website.Trim() != "" && website != null && website.Length > 1)
                                                    {
                                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                                        {
                                                            Collvalue = Collvalue + " , Web Site : " + website;
                                                        }
                                                        else
                                                        {
                                                            Collvalue = "Web Site :" + website;
                                                        }
                                                    }
                                                    //Collvalue = "Email : " + ds.Tables[0].Rows[0]["Email"].ToString() + " , Web Site : " + ds.Tables[0].Rows[0]["Website"].ToString();
                                                    PdfTextArea pts = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 0, coltop, headalign, 50), System.Drawing.ContentAlignment.MiddleCenter, Collvalue);
                                                    mypdfpage.Add(pts);
                                                    collheader = collheader + 1;
                                                }
                                                else if (collinfo == "Left Logo")
                                                {
                                                    if (coltop < 60)
                                                    {
                                                        coltop = 60;
                                                    }
                                                    //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"))) Aruna 19jun2018
                                                    //{
                                                    //    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg")))
                                                    {

                                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg"));//Aruna 19jun2018
                                                        if (strpagesize == "A3")
                                                        {
                                                            mypdfpage.Add(LogoImage, 25, 25, 500);
                                                        }
                                                        else
                                                        {
                                                            mypdfpage.Add(LogoImage, 25, 25, 400);
                                                        }
                                                    }
                                                    else if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpg")))
                                                    {

                                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpg"));//Aruna 19jun2018
                                                        if (strpagesize == "A3")
                                                        {
                                                            mypdfpage.Add(LogoImage, 25, 25, 500);
                                                        }
                                                        else
                                                        {
                                                            mypdfpage.Add(LogoImage, 25, 25, 400);
                                                        }
                                                    }
                                                    if (collheader < 6)
                                                    {
                                                        collheader = 6;
                                                    }
                                                }
                                                else if (collinfo == "Right Logo")
                                                {
                                                    if (coltop < 60)
                                                    {
                                                        coltop = 60;
                                                    }
                                                    //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"))) Aruna 19jun2018
                                                    //{
                                                    //    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));

                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg")))
                                                    {

                                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg")); //Aruna 19jun2018

                                                        if (strpagesize == "A3")
                                                        {
                                                            if (pagesizeflag == true)
                                                                mypdfpage.Add(LogoImage, 1100, 25, 500);
                                                            else
                                                                mypdfpage.Add(LogoImage, 1600, 25, 500);
                                                        }
                                                        else
                                                        {
                                                            if (isiso.Trim() != "" && isiso.Trim() != "0" && isiso != null)
                                                            {
                                                                mypdfpage.Add(LogoImage, 630, 25, 400);
                                                            }
                                                            else
                                                            {
                                                                mypdfpage.Add(LogoImage, 720, 25, 400);
                                                            }
                                                        }
                                                    }
                                                    else if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpg")))
                                                    {

                                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpg")); //Aruna 19jun2018

                                                        if (strpagesize == "A3")
                                                        {
                                                            if (pagesizeflag == true)
                                                                mypdfpage.Add(LogoImage, 1100, 25, 500);
                                                            else
                                                                mypdfpage.Add(LogoImage, 1600, 25, 500);
                                                        }
                                                        else
                                                        {
                                                            if (isiso.Trim() != "" && isiso.Trim() != "0" && isiso != null)
                                                            {
                                                                mypdfpage.Add(LogoImage, 630, 25, 400);
                                                            }
                                                            else
                                                            {
                                                                mypdfpage.Add(LogoImage, 720, 25, 400);
                                                            }
                                                        }
                                                    }
                                                    if (collheader < 6)
                                                    {
                                                        collheader = 6;
                                                    }
                                                }
                                                if (row == 0)
                                                {
                                                    if (collegedetails == "")
                                                    {
                                                        collegedetails = collinfo;
                                                    }
                                                    else
                                                    {
                                                        collegedetails = collegedetails + '#' + collinfo;
                                                    }
                                                }
                                            }
                                        }

                                        if (collheader > 0)
                                        {
                                            Double nrc = (collheader * 3) / 2;
                                            collheader = Convert.ToInt32(Math.Round(nrc, 2, MidpointRounding.AwayFromZero));
                                        }




                                    }


                                    else
                                    {
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                                        {
                                            DataSet dsstuphoto = da.select_method_wo_parameter("select fileupload from tbl_notification where viewrs='Printmaster' and College_Code='" + Session["collegecode"].ToString() + "'", "Text");
                                            if (dsstuphoto.Tables[0].Rows.Count > 0)
                                            {
                                                if (dsstuphoto.Tables[0].Rows[0]["fileupload"] != null && dsstuphoto.Tables[0].Rows[0]["fileupload"].ToString().Trim() != "")
                                                {
                                                    byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["fileupload"];
                                                    MemoryStream memoryStream = new MemoryStream();
                                                    memoryStream.Write(file, 0, file.Length);
                                                    if (file.Length > 0)
                                                    {
                                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(2630, 270, null, IntPtr.Zero);
                                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                                                        {
                                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/printheader" + Session["collegecode"].ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                        }
                                                    }
                                                    memoryStream.Dispose();
                                                    memoryStream.Close();
                                                }
                                            }
                                        }
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                                        {
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/printheader" + Session["collegecode"].ToString() + ".jpeg"));

                                            if (strpagesize == "A3")
                                            {
                                                if (pagesizeflag == true)
                                                {
                                                    mypdfpage.Add(LogoImage, 5, 5, 161);
                                                    coltop = coltop + (nexthead * 9);
                                                }
                                                else
                                                {
                                                    mypdfpage.Add(LogoImage, 5, 5, 112);
                                                    coltop = coltop + (nexthead * 14);
                                                }
                                            }
                                            else
                                            {
                                                if (pagesizeflag == true)
                                                {
                                                    mypdfpage.Add(LogoImage, 5, 5, 227);
                                                }
                                                else
                                                {
                                                    mypdfpage.Add(LogoImage, 5, 5, 225);
                                                }
                                                coltop = coltop + (nexthead * 6);
                                            }

                                        }
                                    }
                                    //added by deepali

                                    for (int parent = 0; parent < treeview_spreadfields.Nodes.Count; parent++)
                                    {
                                        //if (treeview_spreadfields.Nodes[parent].Checked == true)
                                        //{
                                        //   // string printvalue = "";
                                        //    string printFieldsinfo = treeview_spreadfields.Nodes[parent].Text;

                                        //    if (selectedPrintFields== "")
                                        //    {
                                        //        selectedPrintFields= printFieldsinfo;
                                        //    }
                                        //    else
                                        //    {
                                        //        selectedPrintFields = selectedPrintFields + '#' + printFieldsinfo;
                                        //    }

                                        //}



                                    }





                                }


                                int xpos = 500;
                                if (strpagesize == "A3")
                                {
                                    xpos = headalign - 400;
                                }
                                string getdegreedetails = "";
                                string degreedetails = Session["Degree"].ToString();
                                if (degreedetails.Trim() != "" && degreedetails != null)
                                {
                                    string[] spiltdegree = degreedetails.Split('@');
                                    for (int de = 0; de <= spiltdegree.GetUpperBound(0); de++)
                                    {
                                        if (getdegreedetails == "")
                                        {
                                            string[] getdegree = spiltdegree[de].Split(':');
                                            if (getdegree.GetUpperBound(0) >= 1)
                                            {
                                                string[] spitdetails = getdegree[1].Split('-');
                                                if (spitdetails.GetUpperBound(0) >= 3)
                                                {
                                                    for (int di = 0; di <= spitdetails.GetUpperBound(0); di++)
                                                    {
                                                        if (spitdetails[di].ToString().ToLower().Trim() != "sem" && spitdetails[di].ToString().ToLower().Trim() != "sec")
                                                        {
                                                            if (getdegreedetails == "")
                                                            {
                                                                getdegreedetails = spitdetails[di].ToString();
                                                            }
                                                            else
                                                            {
                                                                getdegreedetails = getdegreedetails + ',' + spitdetails[di].ToString();
                                                            }
                                                        }
                                                    }
                                                    DegreeDetails = getdegreedetails;
                                                }
                                            }
                                        }

                                        if (de == 0)
                                        {
                                            string[] spmulhead = spiltdegree[de].ToString().Split('$');
                                            for (int mh = 0; mh <= spmulhead.GetUpperBound(0); mh++)
                                            {
                                                collheader = collheader + 2;
                                                coltop = coltop + nexthead * 2;
                                                PdfTextArea ptdegree = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                     new PdfArea(mydoc, 0, coltop, headalign, 50), System.Drawing.ContentAlignment.MiddleCenter, spmulhead[mh].ToString());
                                                mypdfpage.Add(ptdegree);
                                            }
                                        }
                                        else
                                        {
                                            //if (de % 2 == 0)
                                            //{

                                            //PdfTextArea ptdegree = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                            //   new PdfArea(mydoc, 300, coltop, xpos, 50), System.Drawing.ContentAlignment.MiddleRight, spiltdegree[de].ToString());
                                            //mypdfpage.Add(ptdegree);
                                            //}
                                            //else
                                            //{
                                            collheader = collheader + 2;
                                            coltop = coltop + nexthead + 10;
                                            PdfTextArea ptdegree = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                 new PdfArea(mydoc, 20, coltop, headalign, 50), System.Drawing.ContentAlignment.MiddleLeft, spiltdegree[de].ToString());
                                            mypdfpage.Add(ptdegree);
                                            //}
                                        }
                                    }

                                }
                            }
                            if (visiblerow - norow >= noofrowsperpage)
                            {
                                totalrow = noofrowsperpage + column_header_row_count_orgi;
                            }
                            else
                            {
                                totalrow = (visiblerow - norow) + column_header_row_count_orgi;
                            }
                            //if (fpspreadsample.Sheets[0].RowCount > page)
                            //{
                            //    totalrow = page - value + column_header_row_count_orgi;

                            //}
                            //else
                            //{
                            //    if (fpspreadsample.Sheets[0].RowCount > value)
                            //    {
                            //        totalrow = fpspreadsample.Sheets[0].RowCount - value + column_header_row_count_orgi;
                            //    }
                            //    else
                            //    {
                            //        totalrow = fpspreadsample.Sheets[0].RowCount + column_header_row_count_orgi;
                            //    }
                            //}
                            if (treeview_spreadfields.Nodes.Count == 0)
                            {
                                if (fpspreadsample.Sheets[0].ColumnHeaderVisible == false)
                                {
                                    for (int def = 1; def < fpspreadsample.Sheets[0].ColumnCount; def++)
                                    {
                                        if (fpspreadsample.Sheets[0].Columns[def].Visible == true)
                                        {
                                            if (fpspreadsample.Sheets[0].ColumnHeader.Cells[0, def].Text == "")
                                            {
                                                Headername = Headername + "@&" + def + "";
                                            }
                                        }
                                    }
                                }
                            }
                            string[] spilthead = Headername.Split('@');
                            string[] spiltvalue;
                            int column_header_row_count = 1;
                            column_header_row_count = column_header_row_count_orgi;
                            Boolean incrow = false;
                            int colummerge = 0;
                            try
                            {
                                for (int i = 0; i < (spilthead.GetUpperBound(0)) + 1; i++)
                                {
                                    string[] spitcolumnvallue = spilthead[i].Split('&');
                                    int column = Convert.ToInt32(spitcolumnvallue[spitcolumnvallue.GetUpperBound(0)]);
                                    int lastrow = 0;
                                    if (fpspreadsample.Sheets[0].RowCount > 1)
                                    {
                                        if ((page) < fpspreadsample.Sheets[0].RowCount)
                                        {
                                            lastrow = page - 1;
                                        }
                                        else
                                        {
                                            lastrow = fpspreadsample.Sheets[0].RowCount - 1;
                                        }
                                        int colmerg = spitcolumnvallue.GetUpperBound(0);
                                        if (colmerg >= 0)
                                        {
                                            int mergecolumn = Convert.ToInt32(fpspreadsample.Sheets[0].GetColumnMerge(Convert.ToInt32(spitcolumnvallue[colmerg])));
                                            if (mergecolumn >= 1)
                                            {
                                                colummerge++;
                                                string lastval = fpspreadsample.Sheets[0].Cells[lastrow - 1, (Convert.ToInt32(spitcolumnvallue[colmerg]))].Text.ToString();
                                                string lastpreval = fpspreadsample.Sheets[0].Cells[lastrow, (Convert.ToInt32(spitcolumnvallue[colmerg]))].Text.ToString();
                                                if (lastval == lastpreval)
                                                {
                                                    if (incrow == false)
                                                    {
                                                        totalrow++;
                                                        incrow = true;
                                                    }
                                                    // i = spilthead.GetUpperBound(0) + 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                            incrow = false;
                            if (colummerge > 0)
                            {
                                if (colummerge == (spilthead.GetUpperBound(0)) + 1)
                                {
                                    incrow = true;
                                }
                            }
                            Gios.Pdf.PdfTable table;
                            if (chksno.Checked == false)
                            {
                                if (incrow == false)
                                {
                                    table = mydoc.NewTable(FontBody, totalrow, (Convert.ToInt32(spilthead.GetUpperBound(0)) + 1), column_header_row_count);
                                }
                                else
                                {
                                    table = mydoc.NewTable(FontBody, totalrow, (Convert.ToInt32(spilthead.GetUpperBound(0)) + 2), column_header_row_count);
                                }
                            }
                            else
                            {
                                if (incrow == false)
                                {
                                    table = mydoc.NewTable(FontBody, totalrow, (Convert.ToInt32(spilthead.GetUpperBound(0)) + 2), column_header_row_count);
                                }
                                else
                                {
                                    table = mydoc.NewTable(FontBody, totalrow, (Convert.ToInt32(spilthead.GetUpperBound(0)) + 3), column_header_row_count);
                                }
                            }

                            if (chktblfalse.Checked == false)
                            {
                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            }
                            else
                            {
                                table.SetBorders(Color.Black, 1, BorderType.Bounds);
                            }
                            //table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.CellRange(0, 0, 0, spilthead.GetUpperBound(0)).SetFont(Fonthead);
                            table.VisibleHeaders = false;
                            string tempheader = "";
                            string temphead = "";
                            int spancount = 0;
                            int thirdrowspan = 0;
                            int secondrowspan = 0;
                            int spanheadcolu = 0;
                            if (chkcolour.Checked == true)
                            {
                                for (int hlc = 0; hlc < column_header_row_count; hlc++)
                                {

                                    table.Rows[hlc].SetColors(Color.Black, Color.AliceBlue);
                                }
                            }

                            Boolean tablegflag = false;
                            for (int head = 0; head <= spilthead.GetUpperBound(0); head++)
                            {
                                int tablecolumn = head;
                                if (chksno.Checked == true)
                                {
                                    table.Cell(0, 0).SetContent("S.No");
                                    table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, 0).SetFont(Fonttablehead);
                                    if (column_header_row_count > 1)
                                    {
                                        foreach (PdfCell pc in table.CellRange(0, 0, 0, 0).Cells)
                                        {
                                            pc.RowSpan = column_header_row_count;
                                        }
                                    }
                                    if (chkcolour.Checked == true)
                                    {
                                        table.Rows[0].SetColors(Color.Black, Color.AliceBlue);
                                    }
                                    tablecolumn = head + 1;
                                }
                                System.Drawing.Color colr;
                                int leng = 0;
                                int testlen = 0;
                                string headcolum = spilthead[head].ToString();
                                string[] spitsubcolumn = headcolum.Split('^');
                                string subcoulmn = "";
                                if (spitsubcolumn.GetUpperBound(0) > 0)
                                {
                                    headcolum = spitsubcolumn[0].ToString();
                                    subcoulmn = spitsubcolumn[1].ToString();
                                    spiltvalue = subcoulmn.Split('&');
                                }
                                else
                                {
                                    spiltvalue = headcolum.Split('&');
                                }

                                if (subcoulmn == "")
                                {
                                    table.Cell(0, tablecolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, tablecolumn).SetFont(Fonttablehead);

                                    if (column_header_row_count > 1)
                                    {
                                        foreach (PdfCell pc in table.CellRange(0, tablecolumn, 0, tablecolumn).Cells)
                                        {
                                            pc.RowSpan = column_header_row_count;
                                        }
                                    }
                                    table.Cell(0, tablecolumn).SetContent(spiltvalue[0]);
                                    table.Cell(0, tablecolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    if (chkcolour.Checked == true)
                                    {
                                        table.Cell(0, tablecolumn).SetColors(Color.Black, Color.AliceBlue);
                                    }
                                }
                                else
                                {
                                    string[] spiltthird = subcoulmn.Split('#');
                                    if (spiltthird.GetUpperBound(0) > 0)
                                    {
                                        string thirdhead = spiltthird[0];
                                        spiltvalue = spiltthird[1].Split('&');
                                        if (tempheader != headcolum)
                                        {
                                            tempheader = headcolum;
                                            table.Cell(0, tablecolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, tablecolumn).SetFont(Fonttablehead);
                                            table.Cell(0, tablecolumn).SetContent(headcolum);
                                            if (chkcolour.Checked == true)
                                            {
                                                table.Cell(0, head).SetColors(Color.Black, Color.AliceBlue);
                                            }
                                            spancount = 1;
                                            spanheadcolu = tablecolumn;
                                            secondrowspan = tablecolumn;
                                            if (thirdhead != temphead)
                                            {
                                                temphead = thirdhead;
                                                table.Cell(1, tablecolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, tablecolumn).SetFont(Fonttablehead);
                                                table.Cell(1, tablecolumn).SetContent(thirdhead);
                                                if (chkcolour.Checked == true)
                                                {
                                                    table.Cell(1, tablecolumn).SetColors(Color.Black, Color.AliceBlue);
                                                }
                                                spanheadcolu = tablecolumn;
                                                thirdrowspan = 1;
                                            }
                                            else
                                            {
                                                thirdrowspan++;
                                                foreach (PdfCell pr in table.CellRange(1, spanheadcolu, 1, spanheadcolu).Cells)
                                                {
                                                    pr.ColSpan = thirdrowspan;
                                                }
                                                table.Cell(0, (tablecolumn - spancount + 1)).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }
                                        }
                                        else
                                        {
                                            spancount++;
                                            foreach (PdfCell pr in table.CellRange(0, secondrowspan, 0, secondrowspan).Cells)
                                            {
                                                pr.ColSpan = spancount;
                                            }
                                            table.Cell(0, secondrowspan).SetContentAlignment(ContentAlignment.MiddleCenter);

                                            if (thirdhead != temphead)
                                            {
                                                temphead = thirdhead;
                                                table.Cell(1, tablecolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, tablecolumn).SetFont(Fonttablehead);
                                                table.Cell(1, tablecolumn).SetContent(thirdhead);

                                                if (chkcolour.Checked == true)
                                                {
                                                    table.Cell(1, tablecolumn).SetColors(Color.Black, Color.AliceBlue);
                                                }
                                                spanheadcolu = head;
                                                thirdrowspan = 1;
                                            }
                                            else
                                            {
                                                thirdrowspan++;
                                                foreach (PdfCell pr in table.CellRange(1, spanheadcolu, 1, spanheadcolu).Cells)
                                                {
                                                    pr.ColSpan = thirdrowspan;
                                                }
                                                table.Cell(0, spanheadcolu).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }
                                        }
                                        table.Cell(2, tablecolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(2, tablecolumn).SetFont(Fonttablehead);
                                        table.Cell(2, tablecolumn).SetContent(spiltvalue[0]);
                                        if (chkcolour.Checked == true)
                                        {
                                            table.Cell(2, tablecolumn).SetColors(Color.Black, Color.AliceBlue);
                                        }
                                    }
                                    else
                                    {
                                        if (tempheader != headcolum)
                                        {
                                            tempheader = headcolum;
                                            table.Cell(0, tablecolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, tablecolumn).SetFont(Fonttablehead);
                                            table.Cell(0, tablecolumn).SetContent(headcolum);
                                            if (chkcolour.Checked == true)
                                            {
                                                table.Cell(0, tablecolumn).SetColors(Color.Black, Color.AliceBlue);
                                            }
                                            spancount = 1;

                                            secondrowspan = tablecolumn;
                                        }
                                        else
                                        {
                                            spancount++;
                                            foreach (PdfCell pr in table.CellRange(0, secondrowspan, 0, secondrowspan).Cells)
                                            {
                                                pr.ColSpan = spancount;
                                            }
                                            table.Cell(0, secondrowspan).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        }
                                        table.Cell(1, tablecolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(1, tablecolumn).SetFont(Fonttablehead);
                                        table.Cell(1, tablecolumn).SetContent(spiltvalue[0]);
                                        if (chkcolour.Checked == true)
                                        {
                                            table.Cell(1, tablecolumn).SetColors(Color.Black, Color.AliceBlue);
                                        }
                                    }
                                }
                                string headvalue = spiltvalue[0].ToString();
                                string[] spheadva = headvalue.Split(' ');
                                for (int sph = 0; sph <= spheadva.GetUpperBound(0); sph++)
                                {
                                    testlen = Convert.ToInt32(spheadva[sph].Length);
                                    if (leng < testlen)
                                    {
                                        leng = testlen;
                                    }
                                }
                                int headcolspan = fpspreadsample.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(spiltvalue[1])].ColumnSpan;
                                int column = Convert.ToInt32(spiltvalue[1]);
                                string rowvalue = "";
                                int spanrow = 0;
                                int val = column_header_row_count_orgi - 1;
                                string alignment = fpspreadsample.Sheets[0].Columns[Convert.ToInt32(spiltvalue[1])].HorizontalAlign.ToString();

                                if (page == value)
                                {
                                    value = value++;
                                }
                                for (int rows = value; rows < page; rows++)
                                {
                                    Boolean alignmentcell = false;
                                    if (rows < fpspreadsample.Sheets[0].RowCount)
                                    {
                                        if (fpspreadsample.Sheets[0].Rows[rows].Visible == true)
                                        {
                                            if (head == 0)
                                            {
                                                norow++;
                                            }
                                            tablegflag = true;
                                            val++;
                                            if (chksno.Checked == true)
                                            {
                                                if (head == 0)
                                                {
                                                    srno++;
                                                    table.Cell(val, 0).SetContent(srno.ToString());
                                                    table.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                            }
                                            rowvalue = fpspreadsample.Sheets[0].Cells[rows, column].Text;
                                            string checklength = rowvalue.Trim();
                                            string[] splen = checklength.Split(' ');
                                            for (int sps = 0; sps <= splen.GetUpperBound(0); sps++)
                                            {
                                                if (testlen < splen[sps].ToString().Length)
                                                {
                                                    testlen = Convert.ToInt32(splen[sps].ToString().Length);
                                                }
                                            }
                                            string setspace = "";
                                            if (rowvalue.Trim() != "" && rowvalue != null)
                                            {

                                                if (chkcolour.Checked == true)
                                                {
                                                    colr = fpspreadsample.Sheets[0].Cells[rows, column].BackColor;
                                                    System.Drawing.Color colr1 = fpspreadsample.Sheets[0].Cells[rows, column].ForeColor;
                                                    if (colr.Name.Trim().ToLower() != "black" && colr.Name.Trim().ToLower() != "0")
                                                    {
                                                        table.Cell(val, tablecolumn).SetColors(Color.Black, colr);
                                                    }
                                                    if (colr1.Name.Trim().ToLower() == "white")
                                                    {
                                                        table.Cell(val, tablecolumn).SetColors(Color.White, Color.White);
                                                    }
                                                }
                                                string var = "";
                                                setspace = "";
                                                string[] spiltrowvalu = rowvalue.Split(';');
                                                if (spiltrowvalu.GetUpperBound(0) > 0)
                                                {
                                                    for (int sp = 0; sp <= spiltrowvalu.GetUpperBound(0); sp++)
                                                    {
                                                        if (setspace == "")
                                                        {
                                                            setspace = spiltrowvalu[sp].ToString();
                                                            var = "";
                                                            string[] spitspaceva = spiltrowvalu[sp].Split('-');
                                                            for (int spt = 0; spt < spitspaceva.GetUpperBound(0); spt++)
                                                            {
                                                                if (var == "")
                                                                {

                                                                    var = spitspaceva[spt].ToString();
                                                                }
                                                                else
                                                                {
                                                                    var = var + "- " + spitspaceva[spt].ToString();
                                                                }
                                                                testlen = Convert.ToInt32(spitspaceva[spt].Length);
                                                                if (leng < testlen)
                                                                {
                                                                    leng = testlen;
                                                                }
                                                            }
                                                            setspace = var;
                                                        }
                                                        else
                                                        {
                                                            var = "";
                                                            string[] spitspaceva = spiltrowvalu[sp].Split('-');
                                                            for (int spt = 0; spt < spitspaceva.GetUpperBound(0); spt++)
                                                            {
                                                                if (var == "")
                                                                {
                                                                    var = spitspaceva[spt].ToString();
                                                                }
                                                                else
                                                                {
                                                                    var = var + "- " + spitspaceva[spt].ToString();
                                                                }
                                                                testlen = Convert.ToInt32(spitspaceva[spt].Length);
                                                                if (leng < testlen)
                                                                {
                                                                    leng = testlen;
                                                                }
                                                            }
                                                            setspace = setspace + "; " + var;
                                                            if (var == "")
                                                            {
                                                                setspace = setspace + "; " + spiltrowvalu[sp].ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    string[] spiltrow = rowvalue.Split('-');
                                                    if (spiltrow.GetUpperBound(0) > 3)
                                                    {
                                                        for (int sp = 0; sp <= spiltrow.GetUpperBound(0); sp++)
                                                        {
                                                            if (setspace == "")
                                                            {
                                                                setspace = spiltrow[sp];
                                                            }
                                                            else
                                                            {
                                                                setspace = setspace + " - " + spiltrow[sp];
                                                            }
                                                            testlen = Convert.ToInt32(spiltrow[sp].Length);
                                                            if (leng < testlen)
                                                            {
                                                                leng = testlen;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (setspace != "")
                                            {
                                                rowvalue = setspace;
                                            }
                                            if (leng < testlen)
                                            {
                                                leng = testlen;
                                            }
                                            alignment = fpspreadsample.Sheets[0].Cells[rows, column].HorizontalAlign.ToString();
                                            if (alignment == "NotSet")
                                            {
                                                alignment = fpspreadsample.Sheets[0].Columns[Convert.ToInt32(spiltvalue[1])].HorizontalAlign.ToString();
                                            }
                                            int mergecolumn = Convert.ToInt32(fpspreadsample.Sheets[0].GetColumnMerge(column));
                                            if (mergecolumn >= 1)
                                            {
                                                if (rows == value)
                                                {
                                                    tempvalue = rowvalue;
                                                    tempspan = 1;
                                                    spanrow = val;
                                                }
                                                else
                                                {
                                                    if (val == column_header_row_count_orgi)
                                                    {
                                                        tempspan = 1;
                                                    }
                                                    if (tempvalue != rowvalue)
                                                    {
                                                        tempvalue = rowvalue;
                                                        tempspan = 1;
                                                        spanrow = val;
                                                    }
                                                    else
                                                    {
                                                        tempspan++;
                                                        if (spanrow + tempspan >= totalrow)
                                                        {
                                                            tempspan = totalrow - spanrow;
                                                        }
                                                        if (totalrow > spanrow + tempspan)
                                                        {
                                                            foreach (PdfCell pc in table.CellRange(spanrow, tablecolumn, spanrow, tablecolumn).Cells)
                                                            {
                                                                pc.RowSpan = tempspan;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            int colspan = fpspreadsample.Sheets[0].Cells[rows, column].ColumnSpan;
                                            if (colspan > 1)
                                            {
                                                if (!hatspancol.Contains(rows))
                                                {
                                                    string values = tablecolumn.ToString() + ',' + colspan.ToString();
                                                    hatspancol.Add(rows, tablecolumn);
                                                    alignment = fpspreadsample.Sheets[0].Cells[rows, column].HorizontalAlign.ToString();
                                                    if (alignment == "NotSet")
                                                    {
                                                        alignment = fpspreadsample.Sheets[0].Columns[Convert.ToInt32(spiltvalue[1])].HorizontalAlign.ToString();
                                                    }
                                                }
                                            }
                                            if (hatspancol.Contains(rows))
                                            {
                                                if (rowvalue.Trim() == "" || rowvalue == null && colspan > 1)
                                                {
                                                    string startrow = GetCorrespondingKey(rows, hatspancol).ToString();
                                                    string[] spilt = startrow.Split(',');
                                                    int spanning = tablecolumn - Convert.ToInt32(spilt[0]) + 1;
                                                    if (spilt.GetUpperBound(0) >= 1)
                                                    {
                                                        if (spanning <= Convert.ToInt32(spilt[1]))
                                                        {
                                                            foreach (PdfCell pr in table.CellRange(val, Convert.ToInt32(spilt[0]), val, Convert.ToInt32(spilt[0])).Cells)
                                                            {
                                                                pr.ColSpan = spanning;
                                                            }
                                                        }
                                                        // table.Cell(val, Convert.ToInt32(spilt[0])).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        alignmentcell = true;
                                                    }
                                                }
                                                else
                                                {
                                                    colspan = fpspreadsample.Sheets[0].Cells[rows, column].ColumnSpan;
                                                    string values = tablecolumn.ToString() + ',' + colspan.ToString();
                                                    hatspancol[rows] = values;
                                                }
                                            }
                                            if (fpspreadsample.Sheets[0].Cells[rows, column].Font.Bold == true)
                                            {
                                                table.Cell(val, tablecolumn).SetFont(Fonttablehead);
                                            }
                                            else
                                            {
                                                table.Cell(val, tablecolumn).SetFont(FontBody);
                                            }
                                            table.Cell(val, tablecolumn).SetContent(rowvalue);
                                         
                                           
                                            if (padingleg.Trim() != "")
                                            {
                                                table.Cell(val, tablecolumn).SetCellPadding(padval);
                                            }

                                            if (alignmentcell == false)
                                            {
                                                if (alignment == "Center")
                                                {
                                                    table.Cell(val, tablecolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                                else if (alignment == "Right")
                                                {
                                                    table.Cell(val, tablecolumn).SetContentAlignment(ContentAlignment.MiddleRight);
                                                }
                                                else
                                                {
                                                    table.Cell(val, tablecolumn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (head == 0)
                                            {
                                                if (fpspreadsample.Sheets[0].RowCount > page + 1)
                                                {
                                                    page++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        rows = page;
                                    }
                                }
                                table.Columns[tablecolumn].SetWidth(leng * fontsize);
                                width = width + (leng * fontsize);
                                if (chksno.Checked == true)
                                {
                                    width = width + (3 * fontsize);
                                    table.Columns[0].SetWidth((3 * fontsize));
                                }
                                //    if (spiltvalue[0].ToLower().Trim().ToLower() == "name" || spiltvalue[0].Trim().ToLower() == "student name" || spiltvalue[0].Trim().ToLower() == "staff name")
                                //    {

                                //        table.Columns[tablecolumn].SetWidth(leng + (11 * fontsize));
                                //        width = width + leng + (11 * fontsize);
                                //    }
                                if (spiltvalue[0].Trim().ToLower() == "s.no" || spiltvalue[0].Trim().ToLower() == "sno" || spiltvalue[0].Trim().ToLower() == "s no" || spiltvalue[0] == "sr.no")
                                {
                                    table.Columns[tablecolumn].SetWidth((3 * fontsize));
                                    width = width + (3 * fontsize);
                                }
                                //    else if (spiltvalue[0].Trim().ToString() == "No. Appeared" || spiltvalue[0].Trim().ToString() == "No. passed")
                                //    {
                                //        table.Columns[tablecolumn].SetWidth((4 * fontsize));
                                //        width = width + (4 * fontsize);
                                //    }
                                //    else
                                //    {
                                //        if (leng < 6)
                                //        {
                                //            table.Columns[tablecolumn].SetWidth(leng + (5 * fontsize));
                                //            width = width + leng + (5 * fontsize);
                                //        }
                                //        else if (leng < 11)
                                //        {
                                //            table.Columns[tablecolumn].SetWidth(leng + (6 * fontsize));
                                //            width = width + leng + (6 * fontsize);
                                //        }
                                //        else if (leng < 16)
                                //        {
                                //            table.Columns[tablecolumn].SetWidth(leng + (8 * fontsize));
                                //            width = width + leng + (8 * fontsize);
                                //        }
                                //        else if (leng < 21)
                                //        {
                                //            table.Columns[tablecolumn].SetWidth(leng + (9 * fontsize));
                                //            width = width + leng + (9 * fontsize);
                                //        }
                                //        else
                                //        {
                                //            table.Columns[tablecolumn].SetWidth(leng + (10 * fontsize));
                                //            width = width + leng + (10 * fontsize);
                                //        }
                                //    }
                            }
                            if (incrow == true)
                            {
                                table.Columns[(spilthead.GetUpperBound(0) + 1)].SetWidth(1);
                                for (int dumrow = 0; dumrow < totalrow; dumrow++)
                                {
                                    table.Cell(dumrow, (spilthead.GetUpperBound(0) + 1)).SetColors(Color.White, Color.White);
                                }
                            }
                            //if (strpagesize == "A3")
                            //{
                            //    if (pagesizeflag == false)
                            //    {
                            //        int fontlevel = 20 - fontsize;
                            //        fontlevel = fontlevel * 20;
                            //        int availwidth = 1700 + fontlevel;
                            //        if (width > availwidth)
                            //        {
                            //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Exceed The Page Limits!')", true);
                            //            return;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        int fontlevel = 20 - fontsize;
                            //        fontlevel = fontlevel * 15;
                            //        int availwidth = 1000 - fontlevel;
                            //        if (width > availwidth)
                            //        {
                            //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Exceed The Page Limits!')", true);
                            //            return;
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    if (pagesizeflag == false)
                            //    {
                            //        int fontlevel = 20 - fontsize;
                            //        fontlevel = fontlevel * 15;
                            //        int availwidth = 650 + fontlevel;
                            //        if (width > availwidth)
                            //        {
                            //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Exceed The Page Limits!')", true);
                            //            return;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        int fontlevel = 20 - fontsize;
                            //        fontlevel = fontlevel * 15;
                            //        int availwidth = 725 + fontlevel;
                            //        if (width > availwidth)
                            //        {
                            //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Exceed The Page Limits!')", true);
                            //            return;
                            //        }

                            //    }
                            //}
                            if (page < fpspreadsample.Sheets[0].RowCount)
                            {
                                if (row == nopages - 1)
                                {
                                    nopages++;
                                    if (radiofooter.SelectedItem.ToString() == "Last Page")
                                    {
                                        footflag = false;
                                    }
                                }
                            }
                            else
                            {
                                if (radiofooter.SelectedItem.ToString() == "Last Page")
                                {
                                    footflag = true;
                                }

                            }
                            if (tablegflag == true)
                            {
                                if (headflag == true)
                                {
                                    coltop = coltop + 10;
                                    string headercolumn = da.GetFunction("Select header from tbl_print_master_settings where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
                                    if (headercolumn != "" && headercolumn != "0")
                                    {
                                        string[] spiltheadcolumn = headercolumn.Split('^');

                                        for (int co = 0; co <= spiltheadcolumn.GetUpperBound(0); co++)
                                        {
                                            coltop = coltop + nexthead;
                                            int left = 10;
                                            string[] spiltcolvalue = spiltheadcolumn[co].Split('!');
                                            Double leftvalue = 1000 / Convert.ToInt32(spiltcolvalue.GetUpperBound(0) + 2);
                                            leftvalue = Math.Round(leftvalue, 0);
                                            if (spiltcolvalue.GetUpperBound(0) == 0)
                                            {
                                                string strhead = spiltcolvalue[0].ToString();
                                                PdfTextArea pthead = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, strhead);
                                                mypdfpage.Add(pthead);
                                            }
                                            else
                                            {
                                                for (int re = 0; re <= spiltcolvalue.GetUpperBound(0); re++)
                                                {
                                                    if (re > 0)
                                                    {
                                                        left = left + Convert.ToInt32(leftvalue);
                                                    }

                                                    string strhead = spiltcolvalue[re].ToString();
                                                    PdfTextArea pthead = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, left, coltop, leftvalue, 50), System.Drawing.ContentAlignment.MiddleCenter, strhead);
                                                    mypdfpage.Add(pthead);
                                                }
                                            }
                                        }
                                        coltop = coltop + nexthead + 10;
                                    }
                                    int isoy = 0;
                                    string isocodecoulmn = da.GetFunction("Select isocode from tbl_print_master_settings where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
                                    if (isocodecoulmn != "" && isocodecoulmn != "0")
                                    {
                                        string[] spiltisocolumn = isocodecoulmn.Split('^');

                                        for (int co = 0; co <= spiltisocolumn.GetUpperBound(0); co++)
                                        {
                                            string[] spiltisocolvalue = spiltisocolumn[co].Split('!');
                                            if (spiltisocolvalue.GetUpperBound(0) == 0)
                                            {
                                                if (co > 0)
                                                {
                                                    isoy = isoy + nexthead;
                                                }
                                                string strhead = spiltisocolvalue[0].ToString();
                                                if (isiso.Trim() != "" && isiso.Trim() != "0" && isiso != null)
                                                {
                                                    PdfTextArea pthead = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, (isox + 60), isoy, 150, 50), System.Drawing.ContentAlignment.MiddleRight, strhead);
                                                    mypdfpage.Add(pthead);
                                                }
                                                else
                                                {
                                                    PdfTextArea pthead = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, isox, isoy, 150, 50), System.Drawing.ContentAlignment.MiddleRight, strhead);
                                                    mypdfpage.Add(pthead);
                                                }
                                            }
                                        }
                                    }
                                    if (isoy > coltop)
                                    {
                                        coltop = isoy;
                                    }
                                    coltop = coltop + (3 * nexthead);

                                    if (strpagesize == "A3")
                                    {
                                        if (pagesizeflag == false)
                                        {
                                            if (width > 1670 || chkfitpaper.Checked == true)
                                            {
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 10, coltop, 1670, 251561165));
                                            }
                                            else
                                            {
                                                Double leftarrange = Math.Round(Convert.ToDouble((1670 - width) / 2), 0);
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, leftarrange, coltop, width, 1100));
                                            }
                                            mypdfpage.Add(newpdftabpage);
                                        }
                                        else
                                        {
                                            if (width > 1150 || chkfitpaper.Checked == true)
                                            {
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 10, coltop, 1150, 1500));
                                            }
                                            else
                                            {
                                                Double leftarrange = Math.Round(Convert.ToDouble((1150 - width) / 2), 0);
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, leftarrange, coltop, width, 1500));
                                            }
                                            mypdfpage.Add(newpdftabpage);
                                        }
                                    }
                                    else
                                    {
                                        if (width > 825 || chkfitpaper.Checked == true)
                                        {
                                            newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 10, coltop, 825, 1000));
                                        }
                                        else
                                        {
                                            Double leftarrange = Math.Round(Convert.ToDouble((825 - width) / 2), 0);
                                            newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, leftarrange, coltop, width, 1000));
                                        }
                                        mypdfpage.Add(newpdftabpage);
                                    }
                                }
                                else
                                {
                                    if (strpagesize == "A3")
                                    {
                                        if (pagesizeflag == false)
                                        {
                                            if (width > 1670 || chkfitpaper.Checked == true)
                                            {
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 10, 60, 1670, 1100));
                                            }
                                            else
                                            {
                                                Double leftarrange = Math.Round(Convert.ToDouble((1670 - width) / 2), 0);
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, leftarrange, 60, width, 1100));
                                            }
                                            mypdfpage.Add(newpdftabpage);
                                        }
                                        else
                                        {
                                            if (width > 1150 || chkfitpaper.Checked == true)
                                            {
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 10, 60, 1150, 1500));
                                            }
                                            else
                                            {
                                                Double leftarrange = Math.Round(Convert.ToDouble((1150 - width) / 2), 0);
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, leftarrange, 60, width, 1500));
                                            }
                                            mypdfpage.Add(newpdftabpage);
                                        }

                                    }
                                    else
                                    {
                                        if (pagesizeflag == false)
                                        {
                                            if (width > 825 || chkfitpaper.Checked == true)
                                            {
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 10, 75, 825, 1000));
                                            }
                                            else
                                            {
                                                Double leftarrange = Math.Round(Convert.ToDouble((825 - width) / 2), 0);
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, leftarrange, 75, width, 1000));
                                            }
                                            mypdfpage.Add(newpdftabpage);
                                        }
                                        else
                                        {
                                            if (width > 825 || chkfitpaper.Checked == true)
                                            {
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 10, 25, 825, 1000));
                                            }
                                            else
                                            {
                                                Double leftarrange = Math.Round(Convert.ToDouble((825 - width) / 2), 0);
                                                newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, leftarrange, 75, width, 1000));
                                            }
                                            mypdfpage.Add(newpdftabpage);
                                        }
                                    }
                                }

                                Double getheigh = newpdftabpage.Area.Height;
                                getheigh = Math.Round(getheigh, 0);
                                string[] spitgetdegree;
                                if (footflag == true)
                                {
                                    string sign = "";
                                    string Batch = "";
                                    string degree = "";
                                    string sem = "";
                                    string section = "";
                                    string branch = "";
                                    int signtop = coltop + 30;
                                    int imsize = 0;
                                    Double leftvalue = 0;
                                    int left = 50;
                                    int imaleft = 0;
                                    MemoryStream memoryStream = new MemoryStream();
                                    SqlCommand cmd = new SqlCommand();
                                    string footercolumns = da.GetFunction("Select footer from tbl_print_master_settings where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'");
                                    if (footercolumns.Trim() != "" && footercolumns != "0" && footercolumns != null)
                                    {
                                        string[] spiltfootcolumn = footercolumns.Split('^');
                                        if (chkcollege.Items[10].Selected == true)
                                        {
                                            if (spiltfootcolumn.GetUpperBound(0) > 0)
                                            {
                                                if (strpagesize == "A3")
                                                {
                                                    if (pagesizeflag == false)
                                                    {
                                                        // coltop = 850;
                                                        imsize = 1200;
                                                    }
                                                    else
                                                    {
                                                        // coltop = 600;
                                                        imsize = 1200;
                                                    }
                                                }
                                                else
                                                {
                                                    if (pagesizeflag == false)
                                                    {
                                                        //  coltop = 850;
                                                        imsize = 450;
                                                    }
                                                    else
                                                    {
                                                        // coltop = 370;
                                                        imsize = 1000;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (strpagesize == "A3")
                                                {
                                                    if (pagesizeflag == false)
                                                    {
                                                        //  coltop = 940;
                                                        imsize = 1200;
                                                    }
                                                    else
                                                    {
                                                        // coltop = 680;
                                                        imsize = 1200;
                                                    }
                                                }
                                                else
                                                {
                                                    if (pagesizeflag == false)
                                                    {
                                                        // coltop = 910;
                                                        imsize = 450;
                                                    }
                                                    else
                                                    {
                                                        // coltop = 430;
                                                        imsize = 1000;
                                                    }
                                                }
                                            }
                                        }
                                        int footnexthead = nexthead * 3;
                                        coltop = Convert.ToInt32(getheigh) + footnexthead;
                                        for (int co = 0; co <= spiltfootcolumn.GetUpperBound(0); co++)
                                        {

                                            string[] spiltfootcolvalue = spiltfootcolumn[co].Split('!');
                                            if (strpagesize == "A3")
                                            {
                                                // footnexthead = footnexthead + footnexthead;
                                                coltop = coltop + footnexthead;
                                                left = 50;
                                                if (pagesizeflag == true)
                                                {
                                                    if (spiltfootcolvalue.GetUpperBound(0) > 1)
                                                    {
                                                        leftvalue = 1200 / Convert.ToInt32(spiltfootcolvalue.GetUpperBound(0) + 1);
                                                    }
                                                    else
                                                    {
                                                        leftvalue = 900;
                                                    }
                                                }
                                                else
                                                {
                                                    if (spiltfootcolvalue.GetUpperBound(0) > 1)
                                                    {
                                                        leftvalue = 1600 / Convert.ToInt32(spiltfootcolvalue.GetUpperBound(0) + 1);
                                                    }
                                                    else
                                                    {
                                                        leftvalue = 1300;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (pagesizeflag == true)
                                                {
                                                    left = 50;
                                                }
                                                else
                                                {
                                                    left = 25;
                                                }
                                                if (spiltfootcolvalue.GetUpperBound(0) > 1)
                                                {
                                                    leftvalue = 850 / Convert.ToInt32(spiltfootcolvalue.GetUpperBound(0) + 1);
                                                }
                                                else
                                                {
                                                    leftvalue = 600;
                                                }
                                                coltop = coltop + footnexthead;
                                            }
                                            if (co == 0)
                                            {
                                                coltop = coltop + (footnexthead * 6);
                                            }
                                            leftvalue = Math.Round(leftvalue, 0);
                                            left = Convert.ToInt32(leftvalue);
                                            if (spiltfootcolvalue.GetUpperBound(0) == 0)
                                            {
                                                if (strpagesize != "A3")
                                                {
                                                    footnexthead = footnexthead + footnexthead;
                                                }
                                                coltop = Convert.ToInt32(coltop) + footnexthead + footnexthead;
                                                string strhead = spiltfootcolvalue[0].ToString();
                                                if (strpagesize != "A3")
                                                {

                                                    if (pagesizeflag == true)
                                                    {
                                                        signtop = coltop;
                                                        imaleft = 400;
                                                    }
                                                    else
                                                    {
                                                        signtop = coltop;
                                                        imaleft = 370;
                                                    }
                                                }
                                                else
                                                {
                                                    signtop = coltop;
                                                    if (pagesizeflag == true)
                                                    {
                                                        imaleft = 550;

                                                    }
                                                    else
                                                    {
                                                        imaleft = 800;
                                                    }
                                                }
                                                Boolean imagsetflag = false;
                                                if (chkcollege.Items[10].Selected == true)
                                                {
                                                    try
                                                    {

                                                        string[] spitfoot = strhead.Split(' ');
                                                        for (int fo = 0; fo <= spitfoot.GetUpperBound(0); fo++)
                                                        {
                                                            string test = spitfoot[fo].ToString();
                                                            try
                                                            {
                                                                if (test.ToLower().Trim() == "hod" || test.ToLower().Trim() == "head")
                                                                {
                                                                    if (degree.Trim() == "" || degree == null || degree == "0")
                                                                    {
                                                                        if (DegreeDetails != null && DegreeDetails.Trim() != "")
                                                                        {
                                                                            spitgetdegree = DegreeDetails.Split(',');
                                                                            if (spitgetdegree.GetUpperBound(0) >= 3)
                                                                            {
                                                                                Batch = spitgetdegree[0].ToString();
                                                                                degree = spitgetdegree[1].ToString();
                                                                                branch = spitgetdegree[2].ToString();
                                                                                sem = spitgetdegree[3].ToString();
                                                                                degree = da.GetFunction("select d.Degree_Code from Degree d,Department de,course  c where d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=de.college_code and c.college_code=de.college_code and c.Course_Name='" + spitgetdegree[1].ToString() + "' and de.Dept_Name='" + spitgetdegree[2].ToString() + "'");
                                                                            }
                                                                            if (spitgetdegree.GetUpperBound(0) >= 4)
                                                                            {
                                                                                section = " and Sections='" + spitgetdegree[4].ToString() + "'";
                                                                            }
                                                                            else
                                                                            {
                                                                                section = "";
                                                                            }
                                                                        }
                                                                    }
                                                                    if (degree.Trim() != "" && degree != null && degree != "0")
                                                                    {
                                                                        sign = da.GetFunction("select staff_code from staffmaster s,Department de,Degree d where de.Head_Of_Dept=s.staff_code and d.Dept_Code=de.Dept_Code and d.Degree_Code=" + degree + "");
                                                                        if (sign.Trim() != "" && sign != null && sign != "0")
                                                                        {
                                                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                dssign.Dispose();
                                                                                dssign.Reset();
                                                                                dssign = da.select_method("select staffsign from staffphoto where staff_code='" + sign + "' and staffsign is not null ", hat_print, "Text");
                                                                                if (dssign.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                                    {
                                                                                        byte[] file = (byte[])dssign.Tables[0].Rows[0]["staffsign"];
                                                                                        memoryStream.Write(file, 0, file.Length);
                                                                                        if (file.Length > 0)
                                                                                        {
                                                                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                                        }
                                                                                        memoryStream.Dispose();
                                                                                        memoryStream.Close();
                                                                                    }
                                                                                }
                                                                            }
                                                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                imagsetflag = true;
                                                                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                                                                mypdfpage.Add(LogoImage, imaleft, signtop, imsize);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch
                                                            {
                                                            }
                                                            try
                                                            {
                                                                if (test.ToLower().Trim() == "principal" || test.ToLower().Trim() == "correspond" || test.ToLower().Trim() == "corresponded")
                                                                {
                                                                    sign = "principal" + Session["collegecode"] + "";
                                                                    if (sign.Trim() != "" && sign != null && sign != "0")
                                                                    {
                                                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                        {
                                                                            dssign.Dispose();
                                                                            dssign.Reset();
                                                                            dssign = da.select_method("select principal_sign from collinfo where college_code='" + Session["collegecode"] + "' and principal_sign is not null", hat_print, "Text");
                                                                            if (dssign.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                byte[] file = (byte[])dssign.Tables[0].Rows[0]["principal_sign"];
                                                                                memoryStream.Write(file, 0, file.Length);
                                                                                if (file.Length > 0)
                                                                                {
                                                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                                }
                                                                                memoryStream.Dispose();
                                                                                memoryStream.Close();
                                                                            }
                                                                        }
                                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                        {
                                                                            imagsetflag = true;
                                                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                                                            mypdfpage.Add(LogoImage, imaleft, signtop, imsize);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch
                                                            {
                                                            }
                                                            try
                                                            {
                                                                if (test.ToLower().Trim() == "dean")
                                                                {
                                                                    if (degree.Trim() == "" || degree == null || degree == "0")
                                                                    {
                                                                        if (DegreeDetails != null && DegreeDetails.Trim() != "")
                                                                        {
                                                                            spitgetdegree = DegreeDetails.Split(',');
                                                                            if (spitgetdegree.GetUpperBound(0) >= 3)
                                                                            {
                                                                                Batch = spitgetdegree[0].ToString();
                                                                                degree = spitgetdegree[1].ToString();
                                                                                branch = spitgetdegree[2].ToString();
                                                                                sem = spitgetdegree[3].ToString();
                                                                                degree = da.GetFunction("select d.Degree_Code from Degree d,Department de,course  c where d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=de.college_code and c.college_code=de.college_code and c.Course_Name='" + spitgetdegree[1].ToString() + "' and de.Dept_Name='" + spitgetdegree[2].ToString() + "'");
                                                                            }
                                                                            if (spitgetdegree.GetUpperBound(0) >= 4)
                                                                            {
                                                                                section = " and Sections='" + spitgetdegree[4].ToString() + "'";
                                                                            }
                                                                            else
                                                                            {
                                                                                section = "";
                                                                            }
                                                                        }
                                                                    }
                                                                    if (degree.Trim() != "" && degree != null && degree != "0")
                                                                    {
                                                                        sign = da.GetFunction("select staff_code from staffmaster s,Department de,Degree d where de.dean_name=s.staff_code and d.Dept_Code=de.Dept_Code and d.Degree_Code=" + degree + "");
                                                                        if (sign.Trim() != "" && sign != null && sign != "0")
                                                                        {
                                                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                dssign.Dispose();
                                                                                dssign.Reset();
                                                                                dssign = da.select_method("select staffsign from staffphoto where staff_code='" + sign + "' and staffsign is not null ", hat_print, "Text");
                                                                                if (dssign.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                                    {
                                                                                        byte[] file = (byte[])dssign.Tables[0].Rows[0]["staffsign"];
                                                                                        memoryStream.Write(file, 0, file.Length);
                                                                                        if (file.Length > 0)
                                                                                        {
                                                                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                                        }
                                                                                        memoryStream.Dispose();
                                                                                        memoryStream.Close();
                                                                                    }
                                                                                }
                                                                            }
                                                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                imagsetflag = true;
                                                                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                                                                mypdfpage.Add(LogoImage, imaleft, signtop, imsize);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch
                                                            {
                                                            }
                                                            if (test.ToLower().Trim() == "Secretary")
                                                            {

                                                            }
                                                            try
                                                            {
                                                                if (test.ToLower().Trim() == "coe")
                                                                {
                                                                    sign = "coe" + Session["collegecode"] + "";
                                                                    if (sign.Trim() != "" && sign != null && sign != "0")
                                                                    {
                                                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                        {
                                                                            dssign.Dispose();
                                                                            dssign.Reset();
                                                                            dssign = da.select_method("select coe_signature from collinfo  where college_code='" + Session["collegecode"] + "' and coe_signature is not null", hat_print, "Text");
                                                                            if (dssign.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                byte[] file = (byte[])dssign.Tables[0].Rows[0]["coe_signature"];
                                                                                memoryStream.Write(file, 0, file.Length);
                                                                                if (file.Length > 0)
                                                                                {
                                                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                                                                }
                                                                                memoryStream.Dispose();
                                                                                memoryStream.Close();
                                                                            }
                                                                        }
                                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                        {
                                                                            imagsetflag = true;
                                                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                                                            mypdfpage.Add(LogoImage, imaleft, signtop, imsize);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch
                                                            {
                                                            }
                                                            try
                                                            {
                                                                if (test.ToLower().Trim() == "class")
                                                                {
                                                                    if (degree.Trim() == "" || degree == null || degree == "0")
                                                                    {
                                                                        if (DegreeDetails != null && DegreeDetails.Trim() != "")
                                                                        {
                                                                            spitgetdegree = DegreeDetails.Split(',');
                                                                            if (spitgetdegree.GetUpperBound(0) >= 3)
                                                                            {
                                                                                Batch = spitgetdegree[0].ToString();
                                                                                branch = spitgetdegree[2].ToString();
                                                                                sem = spitgetdegree[3].ToString();
                                                                                degree = da.GetFunction("select d.Degree_Code from Degree d,Department de,course  c where d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=de.college_code and c.college_code=de.college_code and c.Course_Name='" + spitgetdegree[1].ToString() + "' and de.Dept_Name='" + spitgetdegree[2].ToString() + "'");
                                                                            }
                                                                            if (spitgetdegree.GetUpperBound(0) >= 4)
                                                                            {
                                                                                section = " and Sections='" + spitgetdegree[4].ToString() + "'";
                                                                            }
                                                                            else
                                                                            {
                                                                                section = "";
                                                                            }
                                                                        }
                                                                    }
                                                                    if (degree.Trim() != "" && degree != null && degree != "0")
                                                                    {

                                                                        sign = da.GetFunction("select class_advisor from Semester_Schedule where degree_code=" + degree + " and batch_year=" + Batch + " and semester=" + sem + " " + section + " and LastRec=1");
                                                                        if (sign.Trim() != "" && sign != null && sign != "0")
                                                                        {
                                                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                dssign.Dispose();
                                                                                dssign.Reset();
                                                                                dssign = da.select_method("select staffsign from staffphoto where staff_code='" + sign + "' and staffsign is not null", hat_print, "Text");
                                                                                if (dssign.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    byte[] file = (byte[])dssign.Tables[0].Rows[0]["staffsign"];
                                                                                    memoryStream.Write(file, 0, file.Length);
                                                                                    if (file.Length > 0)
                                                                                    {
                                                                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                                    }
                                                                                    memoryStream.Dispose();
                                                                                    memoryStream.Close();
                                                                                }
                                                                            }
                                                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                imagsetflag = true;
                                                                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                                                                mypdfpage.Add(LogoImage, imaleft, signtop, imsize);

                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch
                                                            {
                                                            }
                                                        }
                                                        if (imagsetflag == true)
                                                        {
                                                            if (strpagesize == "A4" && pagesizeflag == false)
                                                            {
                                                                coltop = signtop + (5 * nexthead);
                                                            }
                                                            else
                                                            {
                                                                coltop = signtop + nexthead;
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                                if (strpagesize != "A3")
                                                {
                                                    PdfTextArea pthead = new PdfTextArea(FontBodyhead, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, strhead);
                                                    mypdfpage.Add(pthead);
                                                }
                                                else
                                                {
                                                    if (pagesizeflag == true)
                                                    {
                                                        PdfTextArea pthead = new PdfTextArea(FontBodyhead, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 0, coltop, 1150, 50), System.Drawing.ContentAlignment.MiddleCenter, strhead);
                                                        mypdfpage.Add(pthead);
                                                    }
                                                    else
                                                    {
                                                        PdfTextArea pthead = new PdfTextArea(FontBodyhead, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 0, coltop, 1600, 50), System.Drawing.ContentAlignment.MiddleCenter, strhead);
                                                        mypdfpage.Add(pthead);
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                for (int re = 0; re <= spiltfootcolvalue.GetUpperBound(0); re++)
                                                {
                                                    //if (chkcollege.Items[7].Selected == true)
                                                    //{
                                                    if (re > 0)
                                                    {
                                                        left = left + Convert.ToInt32(leftvalue);
                                                        imaleft = left;
                                                        //if (strpagesize == "A3")
                                                        //{
                                                        //    if (pagesizeflag == true)
                                                        //    {
                                                        //        //imaleft = left + 140;
                                                        //        imaleft = left + (220 - (spiltfootcolvalue.GetUpperBound(0) - 1) * 40);
                                                        //        if (spiltfootcolvalue.GetUpperBound(0) - 1 == 0)
                                                        //        {
                                                        //            imaleft = imaleft + 20;
                                                        //        }
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        //imaleft = left + 240;
                                                        //        imaleft = left + (300 - (spiltfootcolvalue.GetUpperBound(0) - 1) * 50);
                                                        //        if (spiltfootcolvalue.GetUpperBound(0) - 1 == 0)
                                                        //        {
                                                        //            imaleft = imaleft + 20;
                                                        //        }
                                                        //    }
                                                        //}
                                                        //else
                                                        //{
                                                        //    if (pagesizeflag == true)
                                                        //    {
                                                        //        //  imaleft = left + 95 + (spiltfootcolvalue.GetUpperBound(0) * 2);
                                                        //        imaleft = left + (140 - (spiltfootcolvalue.GetUpperBound(0) - 1) * 40);
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        imaleft = left + 135;
                                                        //    }
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        left = 25;
                                                        imaleft = left;
                                                        if (strpagesize == "A3")
                                                        {
                                                            if (pagesizeflag == true)
                                                            {

                                                                //imaleft = left + (220 - (spiltfootcolvalue.GetUpperBound(0) - 1) * 40);
                                                                //if (spiltfootcolvalue.GetUpperBound(0) - 1 == 0)
                                                                //{
                                                                //    imaleft = imaleft + 20;
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                // imaleft = left + 240;
                                                                //imaleft = left + (300 - (spiltfootcolvalue.GetUpperBound(0) - 1) * 50);
                                                                //if (spiltfootcolvalue.GetUpperBound(0) - 1 == 0)
                                                                //{
                                                                //    imaleft = imaleft + 20;
                                                                //}
                                                            }
                                                            if (chkcollege.Items[10].Selected == true)
                                                            {
                                                                signtop = coltop + 10;
                                                                coltop = coltop + 55;
                                                            }

                                                        }
                                                        else
                                                        {
                                                            if (chkcollege.Items[10].Selected == true)
                                                            {
                                                                if (pagesizeflag == true)
                                                                {
                                                                    // imaleft = left + (140 - (spiltfootcolvalue.GetUpperBound(0) - 1) * 40);
                                                                    signtop = coltop + 10;
                                                                    coltop = coltop + 45;

                                                                }
                                                                else
                                                                {
                                                                    //imaleft = left + (140 - (spiltfootcolvalue.GetUpperBound(0) - 1) * 40);
                                                                    signtop = coltop + 10;
                                                                    coltop = coltop + 60;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //}
                                                    string strhead = spiltfootcolvalue[re].ToString();
                                                    string[] spitfoot = strhead.Split(' ');
                                                    try
                                                    {
                                                        if (chkcollege.Items[10].Selected == true)
                                                        {
                                                            for (int fo = 0; fo <= spitfoot.GetUpperBound(0); fo++)
                                                            {
                                                                string test = spitfoot[fo].ToString();
                                                                if (test.ToLower().Trim() == "hod" || test.ToLower().Trim() == "head")
                                                                {
                                                                    if (degree.Trim() == "" || degree == null || degree == "0")
                                                                    {
                                                                        if (DegreeDetails != null && DegreeDetails.Trim() != "")
                                                                        {
                                                                            spitgetdegree = DegreeDetails.Split(',');
                                                                            if (spitgetdegree.GetUpperBound(0) >= 3)
                                                                            {
                                                                                Batch = spitgetdegree[0].ToString();
                                                                                degree = spitgetdegree[1].ToString();
                                                                                branch = spitgetdegree[2].ToString();
                                                                                sem = spitgetdegree[3].ToString();
                                                                                degree = da.GetFunction("select d.Degree_Code from Degree d,Department de,course  c where d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=de.college_code and c.college_code=de.college_code and c.Course_Name='" + spitgetdegree[1].ToString() + "' and de.Dept_Name='" + spitgetdegree[2].ToString() + "'");
                                                                            }
                                                                            if (spitgetdegree.GetUpperBound(0) >= 4)
                                                                            {
                                                                                section = " and Sections='" + spitgetdegree[4].ToString() + "'";
                                                                            }
                                                                            else
                                                                            {
                                                                                section = "";
                                                                            }
                                                                        }
                                                                    }
                                                                    if (degree.Trim() != "" && degree != null && degree != "0")
                                                                    {
                                                                        sign = da.GetFunction("select staff_code from staffmaster s,Department de,Degree d where de.Head_Of_Dept=s.staff_code and d.Dept_Code=de.Dept_Code and d.Degree_Code=" + degree + "");
                                                                        if (sign.Trim() != "" && sign != null && sign != "0")
                                                                        {
                                                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                dssign.Dispose();
                                                                                dssign.Reset();
                                                                                dssign = da.select_method("select staffsign from staffphoto where staff_code='" + sign + "' and staffsign is not null ", hat_print, "Text");
                                                                                if (dssign.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                                    {
                                                                                        byte[] file = (byte[])dssign.Tables[0].Rows[0]["staffsign"];
                                                                                        memoryStream.Write(file, 0, file.Length);
                                                                                        if (file.Length > 0)
                                                                                        {
                                                                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                                        }
                                                                                        memoryStream.Dispose();
                                                                                        memoryStream.Close();
                                                                                    }
                                                                                }
                                                                            }
                                                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                                                                mypdfpage.Add(LogoImage, imaleft, signtop, imsize);
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                if (test.ToLower().Trim() == "principal" || test.ToLower().Trim() == "correspond" || test.ToLower().Trim() == "corresponded")
                                                                {
                                                                    sign = "principal" + Session["collegecode"] + "";
                                                                    if (sign.Trim() != "" && sign != null && sign != "0")
                                                                    {
                                                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                        {
                                                                            dssign.Dispose();
                                                                            dssign.Reset();
                                                                            dssign = da.select_method("select principal_sign from collinfo where college_code='" + Session["collegecode"] + "' and principal_sign is not null", hat_print, "Text");
                                                                            if (dssign.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                byte[] file = (byte[])dssign.Tables[0].Rows[0]["principal_sign"];
                                                                                memoryStream.Write(file, 0, file.Length);
                                                                                if (file.Length > 0)
                                                                                {
                                                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                                                                }
                                                                                memoryStream.Dispose();
                                                                                memoryStream.Close();
                                                                            }
                                                                        }
                                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                        {
                                                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                                                            mypdfpage.Add(LogoImage, imaleft, signtop, imsize);
                                                                        }
                                                                    }
                                                                }

                                                                if (test.ToLower().Trim() == "dean")
                                                                {
                                                                    if (degree.Trim() == "" || degree == null || degree == "0")
                                                                    {
                                                                        if (DegreeDetails != null && DegreeDetails.Trim() != "")
                                                                        {
                                                                            spitgetdegree = DegreeDetails.Split(',');
                                                                            if (spitgetdegree.GetUpperBound(0) >= 3)
                                                                            {
                                                                                Batch = spitgetdegree[0].ToString();
                                                                                degree = spitgetdegree[1].ToString();
                                                                                branch = spitgetdegree[2].ToString();
                                                                                sem = spitgetdegree[3].ToString();
                                                                                degree = da.GetFunction("select d.Degree_Code from Degree d,Department de,course  c where d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=de.college_code and c.college_code=de.college_code and c.Course_Name='" + spitgetdegree[1].ToString() + "' and de.Dept_Name='" + spitgetdegree[2].ToString() + "'");
                                                                            }
                                                                            if (spitgetdegree.GetUpperBound(0) >= 4)
                                                                            {
                                                                                section = " and Sections='" + spitgetdegree[4].ToString() + "'";
                                                                            }
                                                                            else
                                                                            {
                                                                                section = "";
                                                                            }
                                                                        }
                                                                    }
                                                                    if (degree.Trim() != "" && degree != null && degree != "0")
                                                                    {
                                                                        sign = da.GetFunction("select staff_code from staffmaster s,Department de,Degree d where de.dean_name=s.staff_code and d.Dept_Code=de.Dept_Code and d.Degree_Code=" + degree + "");
                                                                        if (sign.Trim() != "" && sign != null && sign != "0")
                                                                        {
                                                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                dssign.Dispose();
                                                                                dssign.Reset();
                                                                                dssign = da.select_method("select staffsign from staffphoto where staff_code='" + sign + "' and staffsign is not null ", hat_print, "Text");
                                                                                if (dssign.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                                    {
                                                                                        byte[] file = (byte[])dssign.Tables[0].Rows[0]["staffsign"];
                                                                                        memoryStream.Write(file, 0, file.Length);
                                                                                        if (file.Length > 0)
                                                                                        {
                                                                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                                        }
                                                                                        memoryStream.Dispose();
                                                                                        memoryStream.Close();
                                                                                    }
                                                                                }
                                                                            }
                                                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                                                                mypdfpage.Add(LogoImage, imaleft, signtop, imsize);
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                if (test.ToLower().Trim() == "coe")
                                                                {
                                                                    sign = "coe" + Session["collegecode"] + "";
                                                                    if (sign.Trim() != "" && sign != null && sign != "0")
                                                                    {
                                                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                        {
                                                                            dssign.Dispose();
                                                                            dssign.Reset();
                                                                            dssign = da.select_method("select coe_signature from collinfo  where college_code='" + Session["collegecode"] + "' and coe_signature is not null", hat_print, "Text");
                                                                            if (dssign.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                byte[] file = (byte[])dssign.Tables[0].Rows[0]["coe_signature"];
                                                                                memoryStream.Write(file, 0, file.Length);
                                                                                if (file.Length > 0)
                                                                                {
                                                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                                                                }
                                                                                memoryStream.Dispose();
                                                                                memoryStream.Close();
                                                                            }
                                                                        }
                                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                        {
                                                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                                                            mypdfpage.Add(LogoImage, imaleft, signtop, imsize);
                                                                        }
                                                                    }
                                                                }

                                                                if (test.ToLower().Trim() == "class")
                                                                {
                                                                    if (degree.Trim() == "" || degree == null || degree == "0")
                                                                    {
                                                                        if (DegreeDetails != null && DegreeDetails.Trim() != "")
                                                                        {
                                                                            spitgetdegree = DegreeDetails.Split(',');
                                                                            if (spitgetdegree.GetUpperBound(0) >= 3)
                                                                            {
                                                                                Batch = spitgetdegree[0].ToString();
                                                                                branch = spitgetdegree[2].ToString();
                                                                                sem = spitgetdegree[3].ToString();
                                                                                degree = da.GetFunction("select d.Degree_Code from Degree d,Department de,course  c where d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=de.college_code and c.college_code=de.college_code and c.Course_Name='" + spitgetdegree[1].ToString() + "' and de.Dept_Name='" + spitgetdegree[2].ToString() + "'");
                                                                            }
                                                                            if (spitgetdegree.GetUpperBound(0) >= 4)
                                                                            {
                                                                                section = " and Sections='" + spitgetdegree[4].ToString() + "'";
                                                                            }
                                                                            else
                                                                            {
                                                                                section = "";
                                                                            }
                                                                        }
                                                                    }
                                                                    if (degree.Trim() != "" && degree != null && degree != "0")
                                                                    {

                                                                        sign = da.GetFunction("select class_advisor from Semester_Schedule where degree_code=" + degree + " and batch_year=" + Batch + " and semester=" + sem + " " + section + " and LastRec=1");
                                                                        if (sign.Trim() != "" && sign != null && sign != "0")
                                                                        {
                                                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                dssign.Dispose();
                                                                                dssign.Reset();
                                                                                dssign = da.select_method("select staffsign from staffphoto where staff_code='" + sign + "' and staffsign is not null", hat_print, "Text");
                                                                                if (dssign.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    byte[] file = (byte[])dssign.Tables[0].Rows[0]["staffsign"];
                                                                                    memoryStream.Write(file, 0, file.Length);
                                                                                    if (file.Length > 0)
                                                                                    {
                                                                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                                    }
                                                                                    memoryStream.Dispose();
                                                                                    memoryStream.Close();
                                                                                }
                                                                            }
                                                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                                                                            {
                                                                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                                                                mypdfpage.Add(LogoImage, imaleft, signtop, imsize);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                    }
                                                    PdfTextArea pthead;
                                                    if (re == 0)
                                                    {
                                                        pthead = new PdfTextArea(FontBodyhead, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, left, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, strhead);
                                                    }
                                                    else
                                                    {
                                                        pthead = new PdfTextArea(FontBodyhead, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, left, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, strhead);
                                                    }
                                                    mypdfpage.Add(pthead);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                row = nopages + nopages;
                            }
                            if (radioheader.SelectedItem.ToString() == "First Page")
                            {
                                headflag = false;
                            }
                            mypdfpage.SaveToDocument();
                        }
                        else
                        {
                            row = nopages;
                        }
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    Response.Buffer = true;
                    Response.Clear();
                    string szPath = appPath + "/Report/";
                    string szFile = "PrintReport" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                    FileInfo fiPath = new FileInfo(szPath + szFile);
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);//jairam

                }


                string query = "if exists(Select * from tbl_print_master_settings where  page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "')";
                query = query + " update tbl_print_master_settings set page_settings='" + pagesetting + "',college_details='" + collegedetails + "',print_fields='" + selectedPrintfields + "',isColor='" + chkcolour.Checked + "' where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
                query = query + " else insert into tbl_print_master_settings (Page_Name,college_details,page_settings,print_fields,usercode,isColor) values ('" + Session["Pagename"] + "','" + collegedetails + "','" + pagesetting + "','" + selectedPrintfields + "', '" + Convert.ToString(Session["user_code"]) + "','" + chkcolour.Checked + "')";
                int p = da.insert_method(query, hat_print, "Text");

                string headerlevel = radioheader.SelectedItem.Value.ToString();
                string footerlevel = radiofooter.SelectedItem.ToString();
                string updatequery = "update tbl_print_master_settings set header_level='" + headerlevel + "',footer_level='" + footerlevel + "' where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "' ";
                int q = da.update_method_wo_parameter(updatequery, "Text");
                if (txtnofrow.Text != "0" && txtnofrow.Text != "" && txtnofrow.Text != null)
                {
                    string straddrow = "update tbl_print_master_settings set with_out_header_no_row_pages='" + txtnofrow.Text + "' where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
                    int b = da.update_method_wo_parameter(straddrow, "Text");
                }

                #region printlock

                string printAvailability = "update TextValTable set TextVal='0' where TextCriteria='prtlk'";
                int printAvailabilityfun = da.update_method_wo_parameter(printAvailability, "text");

                #endregion
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select  Fields for Print";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strstylequery = "";
        if (ddlsection.Text == "Footer")
        {
            strstylequery = "select Foot_Style from tbl_print_master_settings where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
        }
        else if (ddlsection.Text == "Body")
        {
            strstylequery = "select Body_Style from tbl_print_master_settings where page_name='" + Session["Pagename"].ToString() + "'  and usercode='" + Convert.ToString(Session["user_code"]) + "'";
        }
        else
        {
            strstylequery = "select Head_Style from tbl_print_master_settings where page_name='" + Session["Pagename"].ToString() + "'  and usercode='" + Convert.ToString(Session["user_code"]) + "'";
        }

        string getstyle = da.GetFunction(strstylequery);
        if (getstyle.Trim() != null && getstyle.Trim() != "" && getstyle.Trim() != "0")
        {
            string[] spstyle = getstyle.Split(',');
            if (spstyle.GetUpperBound(0) == 1)
            {
                string fontname = spstyle[0].ToString();
                string fontsize = spstyle[1].ToString();
                ddlfont.Text = fontname;
                ddlsize.Text = fontsize;
            }
        }

    }

    protected void Chk_sel_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_sel.Checked == true)
        {
            if (treeview_spreadfields.Nodes.Count > 0)
            {
                for (int nodecount = 0; nodecount < treeview_spreadfields.Nodes.Count; nodecount++)
                {
                    treeview_spreadfields.Nodes[nodecount].Checked = true;
                    if (treeview_spreadfields.Nodes[nodecount].ChildNodes.Count > 0)
                    {
                        for (int chnodecount = 0; chnodecount < treeview_spreadfields.Nodes[nodecount].ChildNodes.Count; chnodecount++)
                        {
                            treeview_spreadfields.Nodes[nodecount].ChildNodes[chnodecount].Checked = true;
                            if (treeview_spreadfields.Nodes[nodecount].ChildNodes[chnodecount].ChildNodes.Count > 0)
                            {
                                for (int subChnodecount = 0; subChnodecount < treeview_spreadfields.Nodes[nodecount].ChildNodes[chnodecount].ChildNodes.Count; subChnodecount++)
                                {
                                    treeview_spreadfields.Nodes[nodecount].ChildNodes[chnodecount].ChildNodes[subChnodecount].Checked = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (treeview_spreadfields.Nodes.Count > 0)
            {
                for (int nodecount = 0; nodecount < treeview_spreadfields.Nodes.Count; nodecount++)
                {
                    treeview_spreadfields.Nodes[nodecount].Checked = false;
                    if (treeview_spreadfields.Nodes[nodecount].ChildNodes.Count > 0)
                    {
                        for (int chnodecount = 0; chnodecount < treeview_spreadfields.Nodes[nodecount].ChildNodes.Count; chnodecount++)
                        {
                            treeview_spreadfields.Nodes[nodecount].ChildNodes[chnodecount].Checked = false;
                            if (treeview_spreadfields.Nodes[nodecount].ChildNodes[chnodecount].ChildNodes.Count > 0)
                            {
                                for (int subChnodecount = 0; subChnodecount < treeview_spreadfields.Nodes[nodecount].ChildNodes[chnodecount].ChildNodes.Count; subChnodecount++)
                                {
                                    treeview_spreadfields.Nodes[nodecount].ChildNodes[chnodecount].ChildNodes[subChnodecount].Checked = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    protected void Chk_sell_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_sell.Checked == true)
        {
            for (int parent = 0; parent < chkcollege.Items.Count; parent++)
            {
                chkcollege.Items[parent].Selected = true;
            }
        }
        else
        {
            for (int parent = 0; parent < chkcollege.Items.Count; parent++)
            {
                chkcollege.Items[parent].Selected = false;
            }
        }
    }

    protected void chkcollegeheader_CheckedChanged(object sender, EventArgs e)
    {
        Chk_sell.Checked = false;
        if (chkcollegeheader.Checked == true)
        {
            Chk_sell.Enabled = false;
        }
        else
        {
            Chk_sell.Enabled = true;
        }
    }

    protected void btnset_Click(object sender, EventArgs e)
    {
        string section = ddlsection.SelectedItem.ToString().Trim();
        string Style = ddlfont.SelectedItem.ToString() + ',' + ddlsize.SelectedItem.ToString();
        ds.Dispose();
        ds.Reset();
        string query = "Select * from tbl_print_master_settings where  page_name='" + Session["Pagename"].ToString() + "'  and usercode='" + Convert.ToString(Session["user_code"]) + "'";
        ds = da.select_method(query, hat_print, "Text");
        if (section == "Header")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                string updatequery = "update tbl_print_master_settings set Head_Style='" + Style + "' where page_name='" + Session["Pagename"].ToString() + "'  and usercode='" + Convert.ToString(Session["user_code"]) + "'";
                int q = da.update_method_wo_parameter(updatequery, "Text");

            }
            else
            {
                string insert = "insert into tbl_print_master_settings (page_name,Head_Style,usercode) values ('" + Session["Pagename"].ToString() + "','" + Style + "' ,'" + Convert.ToString(Session["user_code"]) + "')";
                int q = da.update_method_wo_parameter(insert, "Text");

            }
        }
        else if (section == "Body")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                string updatequery = "update tbl_print_master_settings set Body_Style='" + Style + "' where page_name='" + Session["Pagename"].ToString() + "'  and usercode='" + Convert.ToString(Session["user_code"]) + "'";
                int q = da.update_method_wo_parameter(updatequery, "Text");
            }
            else
            {
                string insert = "insert into tbl_print_master_settings (page_name,Body_Style,usercode) values ('" + Session["Pagename"].ToString() + "','" + Style + "' ,'" + Convert.ToString(Session["user_code"]) + "')";
                int q = da.update_method_wo_parameter(insert, "Text");
            }
        }
        else if (section == "Footer")
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                string updatequery = "update tbl_print_master_settings set Foot_Style='" + Style + "' where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
                int q = da.update_method_wo_parameter(updatequery, "Text");
            }
            else
            {
                string insert = "insert into tbl_print_master_settings (page_name,Foot_Style,usercode) values ('" + Session["Pagename"].ToString() + "','" + Style + "','" + Convert.ToString(Session["user_code"]) + "')";
                int q = da.update_method_wo_parameter(insert, "Text");
            }
            if (txtnofrow.Text != "0" && txtnofrow.Text != null && txtnofrow.Text != "")
            {

            }
        }


    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }

        return null;
    }

    protected void btnrow_Click(object sender, EventArgs e)
    {
        if (txtnofrow.Text != "0" && txtnofrow.Text != "" && txtnofrow.Text != null)
        {
            errmsg.Visible = false;
            ds.Dispose();
            ds.Reset();
            string query = "Select * from tbl_print_master_settings where  page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
            ds = da.select_method(query, hat_print, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (txtnofrow.Text != "0" && txtnofrow.Text != "" && txtnofrow.Text != null)
                {
                    if (ddlheader.Text == "With Header")
                    {
                        string straddrow = "update tbl_print_master_settings set with_header_no_row_pages='" + txtnofrow.Text + "' where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
                        int q = da.update_method_wo_parameter(straddrow, "Text");
                    }
                    else if (ddlheader.Text == "With out Header")
                    {
                        string straddrow = "update tbl_print_master_settings set with_out_header_no_row_pages='" + txtnofrow.Text + "' where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
                        int q = da.update_method_wo_parameter(straddrow, "Text");
                    }
                }
            }
            else
            {
                if (ddlheader.Text == "With Header")
                {
                    string straddrow = "insert into tbl_print_master_settings (with_header_no_row_pages,Page_Name,usercode) values ('" + txtnofrow.Text + "','" + Session["Pagename"].ToString() + "', '" + Convert.ToString(Session["user_code"]) + "')";
                    int q = da.update_method_wo_parameter(straddrow, "Text");
                }
                else if (ddlheader.Text == "With out Header")
                {
                    string straddrow = "insert into tbl_print_master_settings (with_out_header_no_row_pages,Page_Name,usercode) values ('" + txtnofrow.Text + "','" + Session["Pagename"].ToString() + "', '" + Convert.ToString(Session["user_code"]) + "')";
                    int q = da.update_method_wo_parameter(straddrow, "Text");
                }
            }
        }
        else
        {
            errmsg.Visible = true;
            errmsg.Text = "Please Enter No Of Rows Per Page";
        }
    }

    protected void btnimagesave_Click(object sender, EventArgs e)
    {
        string viewer = "", file_type = "", file_extension = "";
        int fileSize = 0;
        byte[] documentBinary = new byte[0];
        string filename = "";
        if (Fpimage.HasFile)
        {
            if (Fpimage.FileName.EndsWith(".jpg") || Fpimage.FileName.EndsWith(".jpeg") || Fpimage.FileName.EndsWith(".JPG") || Fpimage.FileName.EndsWith(".gif") || Fpimage.FileName.EndsWith(".png"))
            {
                fileSize = Fpimage.PostedFile.ContentLength;
                documentBinary = new byte[fileSize];
                Fpimage.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

                file_extension = Path.GetExtension(Fpimage.PostedFile.FileName);
                file_type = Get_file_format(file_extension);

                string strquery = "if exists(select * from tbl_notification where viewrs=@viewrs and College_Code=@College_Code)";
                strquery = strquery + " update tbl_notification set filetype=@filetype,fileupload=@fileupload where viewrs=@viewrs and College_Code=@College_Code";
                strquery = strquery + " else insert into tbl_notification(viewrs,filetype,fileupload,College_Code) values(@viewrs,@filetype,@fileupload,@College_Code)";

                Hashtable hati = new Hashtable();
                hati.Add("viewrs", "Printmaster");
                hati.Add("College_Code", Session["collegecode"].ToString());
                hati.Add("filetype", file_type);
                hati.Add("fileupload", documentBinary);
                int a = da.insert_method(strquery, hati, "Text");
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select Image Formate Like (.jpg,.peg,.JPG,.gif,.png)";
                return;

            }
        }
    }

    public string Get_file_format(string file_extension)
    {
        try
        {
            string file_type = "";
            switch (file_extension)
            {

                case ".pdf":
                    file_type = "application/pdf";
                    break;

                case ".txt":
                    file_type = "application/notepad";
                    break;

                case ".xls":
                    file_type = "application/vnd.ms-excel";
                    break;

                case ".xlsx":
                    file_type = "application/vnd.ms-excel";
                    break;

                case ".doc":
                    file_type = "application/vnd.ms-word";
                    break;

                case ".docx":
                    file_type = "application/vnd.ms-word";
                    break;

                case ".gif":
                    file_type = "image/gif";
                    break;

                case ".png":
                    file_type = "image/png";
                    break;

                case ".jpg":
                    file_type = "image/jpg";
                    break;

                case ".jpeg":
                    file_type = "image/jpeg";
                    break;

            }
            return file_type;
        }
        catch
        {
            return null;
        }
    }

    protected void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            string delquery = "delete from tbl_print_master_settings where page_name='" + Session["Pagename"].ToString() + "' and usercode='" + Convert.ToString(Session["user_code"]) + "'";
            int del = da.update_method_wo_parameter(delquery, "Text");
            ddlsection.SelectedIndex = 0;
            ddlheader.SelectedIndex = 0;
            ddlorientation.SelectedIndex = 0;
            ddlpagesize.SelectedIndex = 0;
            ddlsize.SelectedIndex = 0;
            ddlfont.SelectedIndex = 0;
            ddlsection.SelectedIndex = 0;
            txtnofrow.Text = "";
            txtcolumn.Text = "";
            txtrow.Text = "";
            Chk_sel.Checked = false;
            Chk_sell.Checked = false;
            FpFooter.Enabled = true;
            FpFooter.Sheets[0].RowCount = 0;
            FpFooter.Sheets[0].ColumnCount = 0;
        }
        catch
        {
        }
    }
    //create table tbl_print_master_settings (Page_Name nvarchar(100),page_settings nvarchar(50),college_details varchar(1000),
    //header_level nvarchar(100),footer_level nvarchar(100),header nvarchar(1000),footer nvarchar(1000),isocode nvarchar(1000)
    //,Head_Style nvarchar(150),Body_Style nvarchar(150),Foot_Style nvarchar(150))
    // alter table tbl_print_master_settings add with_out_header_no_row_pages int 
    //alter table tbl_print_master_settings add with_header_no_row_pages int

}