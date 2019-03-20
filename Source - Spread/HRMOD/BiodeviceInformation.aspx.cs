using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using Gios.Pdf;
using System.Text.RegularExpressions;

public partial class BiodeviceInformation : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sql = "";
    int count = 0;
    Boolean cellclick = false;

    DataView dv = new DataView();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();

    Hashtable hat = new Hashtable();

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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.CommandBar.Visible = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Font.Bold = true;
                darkstyle.Border.BorderSize = 0;
                darkstyle.HorizontalAlign = HorizontalAlign.Center;
                darkstyle.VerticalAlign = VerticalAlign.Middle;
                darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FarPoint.Web.Spread.StyleInfo darkstyle1 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle1.Font.Name = "Book Antiqua";
                darkstyle1.Font.Size = FontUnit.Medium;

                FpSpread1.Sheets[0].DefaultStyle = darkstyle1;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 9;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Device Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "IP Address";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Port Number";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Machine Number";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Device For";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Device Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Device Colour";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Hostel / College";

                showdata.Visible = false;
                ddlstudtype.Items.Add(new ListItem("Hostel", "1"));
                ddlstudtype.Items.Add(new ListItem("College", "2"));
                ddlstudtype.Items.Add(new ListItem("Gatepass", "3"));//magesh 23.7.18
                ddlstudtype.Items.Add(new ListItem("Mess", "4"));//saranyadevi 28.2.2019
                ddlstudtype.Visible = false;
                radbtnfinger.Checked = true;
                radbtnbw.Checked = true;
                radbtnataff.Checked = true;
                bindcollege();
                binddevicename();
                collegebind();
            }
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }
    }


    public void binddevicename()
    {
        string collegecode = Convert.ToString(ddlcollege.SelectedValue);
        sql = " select * from DeviceInfo where College_Code='" + collegecode + "' ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(sql, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            chkldn.DataSource = ds;
            chkldn.DataTextField = "DeviceName";
            chkldn.DataValueField = "DeviceName";
            chkldn.DataBind();
            for (int i = 0; i < chkldn.Items.Count; i++)
            {
                chkldn.Items[i].Selected = true;
                if (chkldn.Items[i].Selected == true)
                {
                    count += 1;
                }
                if (chkldn.Items.Count == count)
                {
                    chkdn.Checked = true;
                }
            }
            if (chkdn.Checked == true)
            {
                for (int i = 0; i < chkldn.Items.Count; i++)
                {
                    chkldn.Items[i].Selected = true;
                    txtdn.Text = "Device(" + (chkldn.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chkldn.Items.Count; i++)
                {
                    chkldn.Items[i].Selected = false;
                    txtdn.Text = "---Select---";
                }
            }
        }
    }

    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception e) { }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        popuperrdiv.Visible = true;
        btnSave.Text = "Save";
        Saveclear();
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        binddevicename();
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        errmsg.Visible = false;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblDevid.Text = "";
            string dname = "";
            string data = "";
            string devtype = "";
            string devtypenew = "";
            string devforhostel = "";
            string devcolor = "";
            string getcolor = "";
            string getdevfor = "";
            errmsg.Text = "";
            showdata.Visible = false;
            string collegecode = Convert.ToString(ddlcollege.SelectedValue);
            for (int i = 0; i < chkldn.Items.Count; i++)
            {
                if (chkldn.Items[i].Selected == true)
                {
                    if (dname.Trim() == "")
                    {
                        dname = chkldn.Items[i].Text.ToString();
                    }
                    else
                    {
                        dname = dname + "','" + chkldn.Items[i].Text.ToString();
                    }
                }
            }
            if (dname.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select Atleast One Device Name";
                showdata.Visible = false;
                return;
            }
            sql = " select * from DeviceInfo where College_Code='" + collegecode + "' and DeviceName in ('" + dname + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Sheets[0].RowCount = 0;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["DeviceName"].ToString();
                    FpSpread1.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["IPAdd"].ToString();
                    FpSpread1.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["PortNo"].ToString();
                    FpSpread1.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["MachineNo"].ToString();
                    data = ds.Tables[0].Rows[i]["DeviceFor"].ToString();
                    if (data == "1")
                    {
                        data = "Student";
                    }
                    else
                    {
                        data = "Staff";
                    }

                    FpSpread1.Sheets[0].Cells[i, 5].Text = data;
                    devtype = Convert.ToString(ds.Tables[0].Rows[i]["DeviceMethod"]);
                    if (devtype.Trim() == "1")
                    {
                        devtypenew = "Finger";
                    }
                    else if (devtype.Trim() == "2")
                    {
                        devtypenew = "Face";
                    }
                    else if (devtype.Trim() == "3")
                    {
                        devtypenew = "Finger & Face";
                    }
                    else if (devtype.Trim() == "4")
                    {
                        devtypenew = "RFID";
                    }
                    FpSpread1.Sheets[0].Cells[i, 6].Text = devtypenew;
                    getdevfor = Convert.ToString(ds.Tables[0].Rows[i]["DeviceForHostel"]);
                    if (data == "Student")
                    {
                        if (getdevfor.Trim() == "1")
                        {
                            devforhostel = "Hostel";
                        }
                        else if (getdevfor.Trim() == "2")
                        {
                            devforhostel = "College";
                        }
                        //Added by Saranyadevi 27.7.2018
                        else if (getdevfor.Trim() == "3")
                        {
                            devforhostel = "Gatepass";
                        }
                        FpSpread1.Sheets[0].Cells[i, 8].Text = devforhostel;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[i, 8].Text = "";
                    }
                    getcolor = Convert.ToString(ds.Tables[0].Rows[i]["DeviceType"]);
                    if (getcolor.Trim() == "0")
                    {
                        FpSpread1.Sheets[0].Cells[i, 7].Text = "Black & White";
                    }
                    else if (getcolor.Trim() == "1")
                    {
                        FpSpread1.Sheets[0].Cells[i, 7].Text = "Color";
                    }
                }
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                errmsg.Visible = false;
                rptprint.Visible = true;
                showdata.Visible = true;
            }
            else
            {
                FpSpread1.Visible = false;
                rptprint.Visible = false;
                showdata.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found!";
            }
        }
        catch { }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int insertp = 0;
        string collegecode = Convert.ToString(ddlcollege.SelectedValue);
        sql = "delete from DeviceInfo where DeviceName='" + txtDevname.Text.ToString() + "'  and College_Code='" + collegecode + "' ";
        //hat.Clear();
        //insertp = da.insert_method(sql, hat, "Text");

        insertp = da.update_method_wo_parameter(sql, "Text");
        if (insertp > 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Deleted Successfully";
            Saveclear();
            binddevicename();
            btngo_Click(sender, e);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sql = "";
        string devicename = "";
        string ipadd = "";
        string machineno = "";
        string portno = "";
        string devtype = "";
        string devcolor = "";
        string devfor = "";
        string devforhostel = "";
        int insertp = 0;
        string collcode = Convert.ToString(ddlcollege.SelectedValue);

        devicename = Convert.ToString(txtDevname.Text);
        ipadd = Convert.ToString(txtIpaddress.Text);
        machineno = Convert.ToString(txtMachno.Text);
        portno = Convert.ToString(txtPortno.Text);

        #region Device Type

        if (radbtnfinger.Checked == true)
        {
            devtype = "1";
        }
        else if (radbtnface.Checked == true)
        {
            devtype = "2";
        }
        else if (radbtnfingerface.Checked == true)
        {
            devtype = "3";
        }
        else if (radbtnrfid.Checked == true)
        {
            devtype = "4";
        }

        #endregion

        #region Device Color

        if (radbtnbw.Checked == true)
        {
            devcolor = "0";
        }
        else if (radbtn.Checked == true)
        {
            devcolor = "1";
        }

        #endregion

        #region Device For

        if (radbtnstudent.Checked == true)
        {
            devfor = "1";
            devforhostel = Convert.ToString(ddlstudtype.SelectedValue);
        }
        if (radbtnataff.Checked == true)
        {
            devfor = "2";
            devforhostel = "0";
        }

        #endregion

        if (lblDevid.Text == "")
        {
            string selexist = "select * from DeviceInfo where DeviceName='" + devicename + "' and College_Code='" + collcode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selexist, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Device Name Already Exist!";
                return;
            }
        }

        sql = "if  exists (select * from DeviceInfo where DeviceName='" + devicename + "' and College_Code='" + collcode + "') update DeviceInfo set IPAdd='" + ipadd + "',PortNo='" + portno + "',MachineNo='" + machineno + "',DeviceFor='" + devfor + "',DeviceType='" + devcolor + "',DeviceMethod='" + devtype + "',DeviceTo='0',DeviceForHostel='" + devforhostel + "'  where DeviceName='" + devicename + "' and College_Code='" + collcode + "' else insert into DeviceInfo (DeviceName,IPAdd,PortNo,MachineNo,DeviceFor,College_Code,DeviceType,DeviceMethod,DeviceTo,DeviceForHostel) values ('" + devicename + "','" + ipadd + "','" + portno + "','" + machineno + "','" + devfor + "','" + collcode + "','" + devcolor + "','" + devtype + "','0','" + devforhostel + "')";

        //hat.Clear();
        //insertp = da.insert_method(sql, hat, "Text");
        insertp = da.update_method_wo_parameter(sql, "Text");
        if (lblDevid.Text != "")
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Updated Successfully";
            Saveclear();
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Saved Successfully";
            Saveclear();
        }
        binddevicename();
        btngo_Click(sender, e);
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        popuperrdiv.Visible = false;
    }

    protected void radbtnstudent_CheckedChanged(object sender, EventArgs e)
    {
        if (radbtnstudent.Checked == true)
        {
            ddlstudtype.Visible = true;
        }
        else
        {
            ddlstudtype.Visible = false;
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void chkdn_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdn.Checked == true)
            {
                for (int i = 0; i < chkldn.Items.Count; i++)
                {
                    chkldn.Items[i].Selected = true;
                }
                txtdn.Text = "Device(" + (chkldn.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chkldn.Items.Count; i++)
                {
                    chkldn.Items[i].Selected = false;
                }
                txtdn.Text = "---Select---";
            }
        }
        catch (Exception ex) { }
    }

    protected void chkldn_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txtdn.Text = "--Select--";
            chkdn.Checked = false;
            for (int i = 0; i < chkldn.Items.Count; i++)
            {
                if (chkldn.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdn.Text = "Device(" + commcount.ToString() + ")";
                if (commcount == chkldn.Items.Count)
                {
                    chkdn.Checked = true;
                }
            }
        }
        catch (Exception ex) { }
    }

    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        cellclick = true;
        FpSpread1.SaveChanges();
    }

    protected void FpSpread1_PreRender(object sender, EventArgs e)
    {
        if (cellclick == true)
        {
            string data = "";
            string devforhostel = "";
            popuperrdiv.Visible = true;
            btnSave.Text = "Update";
            string activerow = "";
            string activecol = "";
            activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            int ar;
            int ac;
            ar = Convert.ToInt32(activerow.ToString());
            ac = Convert.ToInt32(activecol.ToString());
            if (ar != -1)
            {
                lblDevid.Text = "data";
                data = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, 1].Text);
                if (data.Trim() != "")
                {
                    txtDevname.Text = data;
                }
                data = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, 2].Text);
                if (data.Trim() != "")
                {
                    txtIpaddress.Text = data;
                }
                data = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, 3].Text);
                if (data.Trim() != "")
                {
                    txtPortno.Text = data;
                }
                data = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, 4].Text);
                if (data.Trim() != "")
                {
                    txtMachno.Text = data;
                }
                data = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, 5].Text);
                if (data.Trim().ToUpper() == "STUDENT")
                {
                    radbtnstudent.Checked = true;
                    radbtnataff.Checked = false;
                    if (radbtnstudent.Checked == true)
                    {
                        devforhostel = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, 8].Text);
                        if (devforhostel.Trim() != "")
                        {
                            ddlstudtype.Visible = true;
                            if (devforhostel.Trim().ToUpper() == "HOSTEL")
                            {
                                ddlstudtype.SelectedIndex = 0;
                            }
                            else if (devforhostel.Trim().ToUpper() == "COLLEGE")
                            {
                                ddlstudtype.SelectedIndex = 1;
                            }
                            else//magesh 23.7.18
                            {
                                ddlstudtype.SelectedIndex = 2;
                            }
                        }
                    }
                }
                else if (data.Trim().ToUpper() == "STAFF")
                {
                    radbtnataff.Checked = true;
                    radbtnstudent.Checked = false;
                    ddlstudtype.Visible = false;
                }
                data = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, 6].Text);
                if (data.Trim().ToUpper() == "FINGER")
                {
                    radbtnfinger.Checked = true;
                    radbtnface.Checked = false;
                    radbtnfingerface.Checked = false;
                    radbtnrfid.Checked = false;
                }
                else if (data.Trim().ToUpper() == "FACE")
                {
                    radbtnface.Checked = true;
                    radbtnfinger.Checked = false;
                    radbtnfingerface.Checked = false;
                    radbtnrfid.Checked = false;
                }
                else if (data.Trim().ToUpper() == "FINGER & FACE")
                {
                    radbtnfingerface.Checked = true;
                    radbtnface.Checked = false;
                    radbtnfinger.Checked = false;
                    radbtnrfid.Checked = false;
                }
                else if (data.Trim().ToUpper() == "RFID")
                {
                    radbtnrfid.Checked = true;
                    radbtnfingerface.Checked = false;
                    radbtnface.Checked = false;
                    radbtnfinger.Checked = false;
                }
                data = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, 7].Text);
                if (data.Trim().ToUpper() == "BLACK & WHITE")
                {
                    radbtnbw.Checked = true;
                    radbtn.Checked = false;
                }
                else if (data.Trim().ToUpper() == "COLOR")
                {
                    radbtn.Checked = true;
                    radbtnbw.Checked = false;
                }
            }
        }
    }

    public void Saveclear()
    {
        txtDevname.Text = "";
        txtIpaddress.Text = "";
        txtMachno.Text = "";
        radbtnfinger.Checked = true;
        radbtnface.Checked = false;
        radbtnfingerface.Checked = false;
        radbtnrfid.Checked = false;
        radbtnbw.Checked = true;
        radbtn.Checked = false;
        radbtnataff.Checked = true;
        radbtnstudent.Checked = false;
        ddlstudtype.Visible = false;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            d2.printexcelreport(FpSpread1, reportname);
        }
        catch { }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Device Information";
            string pagename = "BiodeviceInformation.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch { }
    }

    protected void LinkButton_Click(object sender, EventArgs e)
    {
        try
        {
            Div2.Visible = true;
        }
        catch
        {
        }
    }
    protected void btn_errorclose_Click1(object sender, EventArgs e)
    {
        Div2.Visible = false;
    }
    protected void btn_Save_Click1(object sender, EventArgs e)
    {
         string typ=string.Empty;
          
             if (chkStu.Items.Count > 0)
        {
            for (int i = 0; i < chkStu.Items.Count; i++)
            {
                if (chkStu.Items[i].Selected == true)
                {
                    if (typ == "")
                    {
                        typ = "" + chkStu.Items[i].Value + "";
                    }
                    else
                    {
                        typ = typ + "" + "," + "" + chkStu.Items[i].Value + "";
                    }
                }

            }
        }
             int ins = 0;
         for (int i = 0; i < cblcollege.Items.Count; i++)
            {
                if (cblcollege.Items[i].Selected == true)
                {
                    string smsins = "if exists(select * from New_InsSettings where LinkName ='Gatepass Mobile Rights' and  college_code='" + cblcollege.Items[i].Value + "') update New_InsSettings set linkvalue ='" + typ + "' where  LinkName ='Gatepass Mobile Rights' and college_code='" + cblcollege.Items[i].Value + "' and user_code='" + usercode + "' else insert into New_InsSettings (LinkName,college_code,linkvalue,user_code) values ('Gatepass Mobile Rights','" + cblcollege.Items[i].Value + "','" + typ + "','" + usercode + "')";
                     ins=  da.update_method_wo_parameter(smsins, "Text");
       
                }
         }
         if (ins != 0)
         {
             ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Save Sucessfully')", true);
         }
    }
   protected void collegebind()
{
    try
{
     string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            cblcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblcollege.Enabled = true;
                cblcollege.DataSource = ds;
                cblcollege.DataTextField = "collname";
                cblcollege.DataValueField = "college_code";
                cblcollege.DataBind();
            }
        
}
    catch
{
}
}
   protected void cblcollege_ChekedChange(object sender, EventArgs e)
   {
       string typ=string.Empty;
        if (cblcollege.Items.Count > 0)
        {
            for (int i = 0; i < cblcollege.Items.Count; i++)
            {
                if (cblcollege.Items[i].Selected == true)
                {
                    if (typ == "")
                    {
                        typ = "" + cblcollege.Items[i].Value + "";
                    }
                    else
                    {
                        typ = typ + "" + "," + "" + cblcollege.Items[i].Value + "";
                    }
                }

            }
        }
        string colleg = " select * from New_InsSettings where LinkName ='Gatepass Mobile Rights' and  college_code in('" + typ + "')";
   }
}
