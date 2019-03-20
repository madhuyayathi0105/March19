using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Text;

public partial class UserCreation : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    const int addtokey = 17;
    const int basekey = 43;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        errmsg.Visible = false;
        lblperrmsg.Visible = false;
        errstaff.Visible = false;
        lblgrouperr.Visible = false;
        if (!IsPostBack)
        {

            string un = "", pw = "";
            if (Session["UserName"] != "" && Session["UserName"] != null)
                un = Session["UserName"].ToString();
            if (Session["password"] != "" && Session["password"] != null)
                pw = Session["password"].ToString();
            if (un != "Palpap Admin" || pw.Trim() == "")
            {
                Response.Redirect("~/Default_login.aspx");
            }

            ddlcollege.Items.Clear();
            DataSet ds = d2.select_method_wo_parameter("Select collname,college_code from collinfo order by college_code", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
            Puser.Visible = false;
            PnStaff.Visible = false;
            PnGroup.Visible = false;
            lblstaffname.Visible = false;
            txtstaffname.Visible = false;
            loadgroup();
        }
    }
    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex)
        {
            errmsg.Text = Convert.ToString(ex);
            errmsg.Visible = true;
        }
    }
    public void loadgroup()
    {
        try
        {
            txtgroup.Text = "---Select---";
            chkgroup.Checked = false;
            chklsgroup.Items.Clear();

            string strgpqueru = "select * from GroupMaster order by groupname";
            DataSet ds = d2.select_method_wo_parameter(strgpqueru, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsgroup.DataSource = ds;
                chklsgroup.DataValueField = "group_code";
                chklsgroup.DataTextField = "groupname";
                chklsgroup.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = Convert.ToString(ex);
            errmsg.Visible = true;
        }
    }
    protected void chkgroup_ChekedChange(object sender, EventArgs e)
    {
        if (chkgroup.Checked == true)
        {
            for (int i = 0; i < chklsgroup.Items.Count; i++)
            {
                chklsgroup.Items[i].Selected = true;
            }
            txtgroup.Text = "Group (" + chklsgroup.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsgroup.Items.Count; i++)
            {
                chklsgroup.Items[i].Selected = false;
            }
            txtgroup.Text = "--Select--";
        }
    }
    protected void chklsgroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
        txtgroup.Text = "---Select---";
        chkgroup.Checked = false;
        for (int i = 0; i < chklsgroup.Items.Count; i++)
        {
            if (chklsgroup.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtgroup.Text = "Group (" + commcount.ToString() + ")";
            if (chklsgroup.Items.Count == commcount)
            {
                chkgroup.Checked = true;
            }
        }

    }

    public void loaduser()
    {
        try
        {
            Fpuser.Sheets[0].ColumnCount = 4;
            Fpuser.Sheets[0].RowCount = 0;
            Fpuser.SheetCorner.ColumnCount = 0;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpuser.Sheets[0].Columns[0].Width = 50;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 1].Text = "User Name";
            Fpuser.Sheets[0].Columns[1].Width = 200;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Description";
            Fpuser.Sheets[0].Columns[2].Width = 200;

            Fpuser.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            Fpuser.Sheets[0].Columns[3].Width = 50;
            Fpuser.CommandBar.Visible = false;

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            string chkgroup = " and (isnull(group_code,'')='' or group_code='0' or group_code='-1')";
            if (chkincludegroup.Checked == true)
            {
                chkgroup = "";
            }

            string strusename = "";
            if (txtusersearch.Text.ToString().Trim() != "")
            {
                strusename = " and user_id like '" + txtusersearch.Text.ToString() + "%'";
            }

            string strquery = "select User_code,user_id,Description from usermaster where college_code='" + ddlcollege.SelectedValue.ToString() + "' " + chkgroup + " " + strusename + " order by user_id";
            DataSet ds = d2.select_method_wo_parameter(strquery, "Text");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Fpuser.Sheets[0].RowCount++;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].Text = Fpuser.Sheets[0].RowCount.ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].CellType = txt;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["user_id"].ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].CellType = txt;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Description"].ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[0].Rows[i]["User_code"].ToString();
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].CellType = chk;
                Fpuser.Sheets[0].Cells[Fpuser.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
            }
            Fpuser.Width = 518;
            Fpuser.Sheets[0].PageSize = Fpuser.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblperrmsg.Visible = true;
            lblperrmsg.Text = ex.ToString();
        }
    }
    protected void chkincludegroup_Checked(object sender, EventArgs e)
    {
        loaduser();
    }
    protected void btnuse_Click(object sender, EventArgs e)
    {
        try
        {
            Puser.Visible = true;
            chkincludegroup.Checked = false;
            txtusersearch.Text = "";
            loaduser();
        }
        catch (Exception ex)
        {
            lblperrmsg.Visible = true;
            lblperrmsg.Text = ex.ToString();
        }
    }
    protected void btnuserok_Click(object sender, EventArgs e)
    {
        try
        {
            lblstaffcodehide.Text = "";
            Fpuser.SaveChanges();
            chkgroup.Checked = false;
            txtgroup.Text = "---Select---";
            for (int g = 0; g < chklsgroup.Items.Count; g++)
            {
                chklsgroup.Items[g].Selected = false;
            }

            for (int r = 0; r < Fpuser.Sheets[0].Rows.Count; r++)
            {
                int isval = Convert.ToInt32(Fpuser.Sheets[0].Cells[r, 3].Value);
                if (isval == 1)
                {
                    string strquery = "select  User_id,Full_Name,Description,group_code,SingleUser,is_staff,staff_code,college_code,phone_no,fin_user,otpconfirm,counter_no,counter_name from UserMaster where User_id='" + Fpuser.Sheets[0].Cells[r, 1].Text.ToString() + "'";
                    DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string uname = ds.Tables[0].Rows[0]["user_id"].ToString();
                        string funame = ds.Tables[0].Rows[0]["full_name"].ToString();
                        string descript = ds.Tables[0].Rows[0]["Description"].ToString();
                        string groupode = ds.Tables[0].Rows[0]["group_code"].ToString();
                        string singuser = ds.Tables[0].Rows[0]["SingleUser"].ToString();
                        string staffuser = ds.Tables[0].Rows[0]["is_staff"].ToString();
                        string staffcode = ds.Tables[0].Rows[0]["staff_code"].ToString();
                        string collode = ds.Tables[0].Rows[0]["college_code"].ToString();
                        string phno = ds.Tables[0].Rows[0]["phone_no"].ToString();
                        string finuser = ds.Tables[0].Rows[0]["fin_user"].ToString();
                        string countno = ds.Tables[0].Rows[0]["counter_no"].ToString();
                        string countname = ds.Tables[0].Rows[0]["counter_name"].ToString();
                        string otpuser = ds.Tables[0].Rows[0]["otpconfirm"].ToString();
                        txtusername.Text = uname;
                        txtfullname.Text = funame;
                        txtdescription.Text = descript;
                        txtphone.Text = phno;
                        txtcounter.Text = countno;
                        txtcouname.Text = countname;

                        txtstaffname.Visible = false;
                        lblstaffname.Visible = false;
                        chkstaff.Checked = false;
                        txtstaffname.Text = "";
                        if (staffuser.Trim() == "1" || staffuser.Trim().ToLower() == "true")
                        {
                            chkstaff.Checked = true;
                            txtstaffname.Visible = true;
                            lblstaffname.Visible = true;
                            lblstaffcodehide.Text = staffcode;
                            string staffname = d2.GetFunction("Select staff_name from staffmaster where staff_code='" + staffcode + "'");
                            txtstaffname.Text = staffname;
                        }

                        chkfin.Checked = false;
                        if (finuser.Trim() == "1" || finuser.Trim().ToLower() == "true")
                        {
                            chkfin.Checked = true;
                        }

                        ChkOtpConfirm.Checked = false;//abarna
                        if (otpuser.Trim() == "1" || otpuser.Trim().ToLower() == "true")
                        {
                            ChkOtpConfirm.Checked = true;
                        }

                        chkgroupuser.Checked = false;
                        if (singuser.Trim() == "0" || singuser.Trim().ToLower() == "false")
                        {
                            chkgroupuser.Checked = true;

                            int commcount = 0;
                            Hashtable hatgroup = new Hashtable();

                            string[] spg = groupode.Split(';');
                            for (int gc = 0; gc <= spg.GetUpperBound(0); gc++)
                            {
                                if (!hatgroup.Contains(spg[gc].ToString()))
                                {
                                    hatgroup.Add(spg[gc].ToString(), spg[gc].ToString());
                                }
                            }

                            for (int g = 0; g < chklsgroup.Items.Count; g++)
                            {
                                string gcod = chklsgroup.Items[g].Value.ToString();
                                if (hatgroup.Contains(gcod))
                                {
                                    chklsgroup.Items[g].Selected = true;
                                    commcount = commcount + 1;
                                }
                                else
                                {
                                    chklsgroup.Items[g].Selected = false;
                                }
                            }

                            if (commcount > 0)
                            {
                                txtgroup.Text = "Group (" + commcount.ToString() + ")";
                                if (chklsgroup.Items.Count == commcount)
                                {
                                    chkgroup.Checked = true;
                                }
                            }
                        }

                        Puser.Visible = false;

                    }
                }

            }
        }
        catch (Exception ex)
        {
            lblperrmsg.Visible = true;
            lblperrmsg.Text = ex.ToString();
        }
    }
    protected void btnuseexit_Click(object sender, EventArgs e)
    {
        try
        {
            Puser.Visible = false;
        }
        catch (Exception ex)
        {
            lblperrmsg.Visible = true;
            lblperrmsg.Text = ex.ToString();
        }
    }

    protected void btnaddstaff_Click(object sender, EventArgs e)
    {
        loadstaffdep();
        loadfsstaff();
        PnStaff.Visible = true;
    }

    public void loadstaffdep()
    {
        try
        {
            string strquery = "select distinct dept_name,dept_code from hrdept_master where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
            ddldepratstaff.DataSource = ds;
            ddldepratstaff.DataTextField = "dept_name";
            ddldepratstaff.DataValueField = "dept_code";
            ddldepratstaff.DataBind();
            ddldepratstaff.Items.Insert(0, "All");
        }
        catch (Exception ex)
        {
            errstaff.Visible = true;
            errstaff.Text = ex.ToString();
        }
    }

    protected void loadfsstaff()
    {
        try
        {
            string sql = "select sm.staff_name,st.staff_code from staffmaster sm,stafftrans st,hrdept_master h where sm.staff_code=st.staff_code and st.dept_code=h.dept_code and sm.college_code=h.college_code and sm.college_code='" + ddlcollege.SelectedValue + "'";
            if (ddldepratstaff.SelectedItem.ToString() != "All")
            {
                sql = sql + " and st.dept_code='" + ddldepratstaff.SelectedValue.ToString() + "'";
            }
            if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = sql + " and sm.staff_name like '" + txt_search.Text.ToString() + "%'";
                }
                else
                {
                    sql = sql + " and sm.staff_code like '" + txt_search.Text.ToString() + "%'";
                }
            }
            DataSet dsbindspread = d2.select_method_wo_parameter(sql, "Text");
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();

            fsstaff.Sheets[0].RowCount = 0;
            fsstaff.CommandBar.Visible = false;
            fsstaff.SheetCorner.ColumnCount = 0;
            if (dsbindspread.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Name";
                fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Code";
                fsstaff.Sheets[0].ColumnHeader.Columns[3].Label = "Select";

                fsstaff.Sheets[0].Columns[0].Width = 50;
                fsstaff.Sheets[0].Columns[1].Width = 210;
                fsstaff.Sheets[0].Columns[2].Width = 350;
                fsstaff.Sheets[0].Columns[3].Width = 64;

                fsstaff.Sheets[0].ColumnCount = 4;
                fsstaff.Width = 690;
                fsstaff.Visible = true;
                FarPoint.Web.Spread.TextCellType btva = new FarPoint.Web.Spread.TextCellType();
                for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();

                    fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
                    fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = name;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Locked = true;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].CellType = btva;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].CellType = btva;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = code;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Locked = true;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].CellType = chkcell1;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    fsstaff.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                    chkcell1.AutoPostBack = true;
                }
            }
            else
            {
                fsstaff.Visible = false;
                errstaff.Visible = true;
                errstaff.Text = "No Records Found";
            }
            fsstaff.Sheets[0].AutoPostBack = false;
            fsstaff.Sheets[0].PageSize = fsstaff.Sheets[0].RowCount;
            fsstaff.SaveChanges();
        }
        catch (Exception ex)
        {
            errstaff.Visible = true;
            errstaff.Text = ex.ToString();
        }
    }
    protected void ddlsatffcoll_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff();
        loadstaffdep();
    }
    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }
    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }
    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }
    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean stafflag = false;
            fsstaff.SaveChanges();
            for (int r = 0; r < fsstaff.Sheets[0].Rows.Count; r++)
            {
                int isval = Convert.ToInt32(fsstaff.Sheets[0].Cells[r, 3].Value);
                if (isval == 1)
                {
                    stafflag = true;
                    txtstaffname.Text = fsstaff.Sheets[0].Cells[r, 2].Text.ToString();
                    lblstaffcodehide.Text = fsstaff.Sheets[0].Cells[r, 1].Text.ToString();
                    r = fsstaff.Sheets[0].Rows.Count;
                    PnStaff.Visible = false;
                }
            }
            if (stafflag == false)
            {
                errstaff.Visible = true;
                errstaff.Text = "Please Select The Staff and Then Proceed";
            }
        }
        catch (Exception ex)
        {
            errstaff.Visible = true;
            errstaff.Text = ex.ToString();
        }
    }
    protected void exitpop_Click(object sender, EventArgs e)
    {
        PnStaff.Visible = false;
    }
    public void clear()
    {
        txtusername.Text = "";
        txtfullname.Text = "";
        txtdescription.Text = "";
        txtpassword.Text = "";
        txtconpassword.Text = "";
        txtcounter.Text = "";
        txtcouname.Text = "";
        txtphone.Text = "";
        chkfin.Checked = false;
        ChkOtpConfirm.Checked = false;//abarna
        chkstaff.Checked = false;
        txtstaffname.Text = "";
        txtstaffname.Visible = false;
        lblstaffname.Visible = false;
        chkinvenuser.Checked = false;
        txtgroup.Text = "---Select---";
        chkgroupuser.Checked = false;
        lblstaffcodehide.Text = "";
        for (int i = 0; i < chklsgroup.Items.Count; i++)
        {
            chklsgroup.Items[i].Selected = false;
        }
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = Convert.ToString(ex);
            errmsg.Visible = true;
        }
    }
    public void saveuser()
    {
        string username = txtusername.Text.ToString().Trim();
        if (username.Trim() == "")
        {
            errmsg.Text = "Please Enter The User Name and Then Proceed";
            errmsg.Visible = true;
            return;
        }

        string fullname = txtfullname.Text.ToString().Trim();
        if (fullname.Trim() == "")
        {
            errmsg.Text = "Please Enter The Full Name and Then Proceed";
            errmsg.Visible = true;
            return;
        }

        string descript = txtdescription.Text.ToString().Trim();
        if (descript.Trim() == "")
        {
            errmsg.Text = "Please Enter The Description Name and Then Proceed";
            errmsg.Visible = true;
            return;
        }

        if (username.Trim().ToLower() == "palpap admin")
        {
            if (txtpassword.Text.Trim() == "" || txtconpassword.Text.Trim() == "")
            {
                errmsg.Text = "Please Enter The Password";
                errmsg.Visible = true;
                return;

            }
            else if (txtpassword.Text.ToString() != txtconpassword.Text.ToString())
            {
                errmsg.Text = "Passwords does not match";
                errmsg.Visible = true;
                return;
            }
        }

        string password = "";
        if (txtpassword.Text != "")
        {
            password = encryptdata(txtpassword.Text);

            if (txtpassword.Text.ToString() != txtconpassword.Text.ToString())
            {
                errmsg.Text = "Please Enter The Description Name and Then Proceed";
                errmsg.Visible = true;
                return;
            }
        }
        

        string phonenum = txtphone.Text.ToString().Trim();
        string counternum = txtcounter.Text.ToString().Trim();
        string countername = txtcouname.Text.ToString().Trim();
        string finuser = "0";
        if (chkfin.Checked == true)
        {
            finuser = "1";
        }

        string otpCfm = "0";//abarna
        if (ChkOtpConfirm.Checked == true)
        {
            otpCfm = "1";
        }

        string isstaff = "0";
        string stafcode = "";
        if (chkstaff.Checked == true)
        {
            isstaff = "1";
            stafcode = lblstaffcodehide.Text.ToString();
        }



        string singleuser = "1";
        string groupcode = "";
        if (chkgroupuser.Checked == true)
        {
            singleuser = "0";
            for (int g = 0; g < chklsgroup.Items.Count; g++)
            {
                if (chklsgroup.Items[g].Selected == true)
                {
                    if (groupcode == "")
                    {
                        groupcode = chklsgroup.Items[g].Value.ToString();
                    }
                    else
                    {
                        groupcode = groupcode + ";" + chklsgroup.Items[g].Value.ToString();
                    }
                }
            }
            if (groupcode == "")
            {
                errmsg.Text = "Please Enter The Group and Then Proceed";
                errmsg.Visible = true;
                return;
            }
        }
        //if (username.Trim().ToLower() == "palpap admin")
        //{
        //    string alrExist = d2.GetFunction("select * from UserMaster where User_id='palpap admin'").Trim();
        //    if (alrExist == "0" || string.IsNullOrEmpty(alrExist))
        //        alrExist = string.Empty;

        //    if (!string.IsNullOrEmpty(alrExist))
        //    {
        //        errmsg.Text = "User already exists";
        //        errmsg.Visible = true;
        //        return;
        //    }
        //}
        string insupdatequery = " if not exists (select * from UserMaster where User_id='" + username + "') insert into UserMaster (User_id,PassWord,Full_Name,Description,group_code,SingleUser,is_staff,staff_code,college_code,phone_no,fin_user,counter_no,counter_name,otpconfirm)";
        insupdatequery = insupdatequery + " values ('" + username + "','" + password + "','" + fullname + "','" + descript + "','" + groupcode + "','" + singleuser + "','" + isstaff + "','" + stafcode + "','" + ddlcollege.SelectedValue.ToString() + "','" + phonenum + "','" + finuser + "','" + counternum + "','" + countername + "','" + otpCfm  +"')";
        insupdatequery = insupdatequery + " else update UserMaster set PassWord='" + password + "',Full_Name='" + fullname + "',Description='" + descript + "',group_code='" + groupcode + "',SingleUser='" + singleuser + "',is_staff='" + isstaff + "',staff_code='" + stafcode + "',college_code='" + ddlcollege.SelectedValue.ToString() + "',phone_no='" + phonenum + "',fin_user='" + finuser + "',otpconfirm='" + otpCfm + "',counter_no='" + counternum + "',counter_name='" + countername + "' where User_id='" + username + "'";
        int insupdval = d2.update_method_wo_parameter(insupdatequery, "Text");
        clear();
        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('User Created Successfully')", true);
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            saveuser();
        }
        catch (Exception ex)
        {
            errmsg.Text = Convert.ToString(ex);
            errmsg.Visible = true;
        }
    }
    protected void btnrestpass_Click(object sender, EventArgs e)
    {
        try
        {
            saveuser();
        }
        catch (Exception ex)
        {
            errmsg.Text = Convert.ToString(ex);
            errmsg.Visible = true;
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            string username = txtusername.Text.ToString().Trim();
            if (username.Trim() == "")
            {
                errmsg.Text = "Please Enter The User Name and Then Proceed";
                errmsg.Visible = true;
                return;
            }
            string insupdatequery = "delete from UserMaster where User_id='" + username + "'";
            int insupdval = d2.update_method_wo_parameter(insupdatequery, "Text");
            clear();
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('User Deleted Successfully')", true);
        }
        catch (Exception ex)
        {
            errmsg.Text = Convert.ToString(ex);
            errmsg.Visible = true;
        }
    }
    protected void chkstaff_CheckedChnage(object sender, EventArgs e)
    {
        try
        {
            txtstaffname.Text = "";
            lblstaffcodehide.Text = "";
            if (chkstaff.Checked == true)
            {
                txtstaffname.Visible = true;
                lblstaffname.Visible = true;
            }
            else
            {
                txtstaffname.Visible = false;
                lblstaffname.Visible = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = Convert.ToString(ex);
            errmsg.Visible = true;
        }
    }
    public string encryptdata(string text)
    {

        int counter;
        int daykey;
        string retdata = "";
        string encdata = "";
        System.Text.Encoding asc = System.Text.Encoding.ASCII;

        if (text == "")
        {
            encdata = "";
        }
        daykey = Generatekey();
        retdata = Convert.ToChar((daykey.ToString()).Length).ToString().Trim();
        retdata = retdata + encryptkey(daykey.ToString());

        for (counter = 0; counter < text.Length; counter++)
        {
            byte[] tbyte;
            string midsub = text.Substring(counter, 1);
            tbyte = asc.GetBytes(midsub);
            long tempbyte = tbyte[0];
            long temp = (tempbyte + daykey) % 256;

            string data = Encoding.Default.GetString(new[] { (byte)temp });

            retdata = retdata + data;

        }
        encdata = retdata;
        return encdata;
    }
    public int Generatekey()
    {

        int millisecond;
        millisecond = Convert.ToInt32((0) % (100));

        int generatekey = millisecond + addtokey;
        return (generatekey);
    }
    public string encryptkey(string key)
    {

        int counter;
        string newkey = "";
        System.Text.Encoding asc = System.Text.Encoding.ASCII;
        for (counter = 0; counter < key.Length; counter++)
        {
            byte[] tbyte;

            string s = key.Substring(counter, 1);
            tbyte = asc.GetBytes(s);
            long tempbyte = tbyte[0];

            string temp = Convert.ToChar(tempbyte + basekey).ToString();

            newkey = newkey + temp;
        }

        return (newkey);
    }

    public void loadbasegroup()
    {
        try
        {
            ddlgroupna.Items.Clear();
            string strgpqueru = "select college_code,collname from collinfo order by college_code asc";
            DataSet ds = d2.select_method_wo_parameter(strgpqueru, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlgroupna.DataSource = ds;
                ddlgroupna.DataValueField = "college_code";
                ddlgroupna.DataTextField = "collname";
                ddlgroupna.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblgrouperr.Visible = true;
            lblgrouperr.Text = ex.ToString();
        }
    }
    protected void ddlgroupna_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtgroupname.Text = "";
            txtgroupdesc.Text = "";
            DataSet dsgroup = d2.select_method_wo_parameter("select * from groupmaster", "Text");// where group_code='" + ddlgroupna.SelectedValue.ToString() + "
            if (dsgroup.Tables[0].Rows.Count > 0)
            {
                txtgroupname.Text = dsgroup.Tables[0].Rows[0]["groupName"].ToString();
                txtgroupdesc.Text = dsgroup.Tables[0].Rows[0]["description"].ToString();
            }
        }
        catch (Exception ex)
        {
            lblgrouperr.Text = Convert.ToString(ex);
            lblgrouperr.Visible = true;
        }
    }
    protected void btngroupcreate_Click(object sender, EventArgs e)
    {
        try
        {
            loadbasegroup();
            ddlgroupna_SelectedIndexChanged(sender, e);
            PnGroup.Visible = true;
        }
        catch (Exception ex)
        {
            errmsg.Text = Convert.ToString(ex);
            errmsg.Visible = true;
        }
    }
    protected void btngroupsave_Click(object sender, EventArgs e)
    {
        try
        {
            string grname = txtgroupname.Text.ToString().Trim();
            string grdesc = txtgroupdesc.Text.ToString().Trim();

            if (grname.Trim() == "")
            {
                lblgrouperr.Visible = true;
                lblgrouperr.Text = "Please Enter the Group Name and then Proeed";
                return;
            }

            if (grdesc.Trim() == "")
            {
                lblgrouperr.Visible = true;
                lblgrouperr.Text = "Please Enter the Group Description and then Proeed";
                return;
            }
            string strgroupval = "";
            int insval = 0;
            if (chkgropedit.Checked == true)
            {
                strgroupval = "update groupmaster set description='" + grdesc + "',groupName='" + grname + "' where group_code='" + ddlgroupna.SelectedValue.ToString() + "' ";
                insval = d2.update_method_wo_parameter(strgroupval, "text");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Gropu Updated Successfully')", true);
            }
            else
            {
                strgroupval = "if exists(select * from groupmaster where groupName='" + grname + "' ) update groupmaster set description='" + grdesc + "' where groupName='" + grname + "' else insert into groupmaster (groupName,description) values('" + grname + "','" + grdesc + "')";
                insval = d2.update_method_wo_parameter(strgroupval, "text");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Group Created Successfully')", true);
            }
            loadbasegroup();
            loadgroup();
        }
        catch (Exception ex)
        {
            lblgrouperr.Text = Convert.ToString(ex);
            lblgrouperr.Visible = true;
        }
    }
    protected void btngroupdelete_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgroup = d2.select_method_wo_parameter("select * from UserMaster where group_code like '%" + ddlgroupna.SelectedValue.ToString() + "%'", "Text");
            if (dsgroup.Tables[0].Rows.Count > 0)
            {
                lblgrouperr.Text = "Please Clear All the Users and Privilages under this Group before Deleting";
                lblgrouperr.Visible = true;
            }
            else
            {
                string strgroupval = "Delete from groupmaster where group_code='" + ddlgroupna.SelectedValue.ToString() + "'";
                int insval = d2.update_method_wo_parameter(strgroupval, "text");
                loadbasegroup();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Gropu Delete Successfully')", true);
            }
            loadgroup();
        }
        catch (Exception ex)
        {
            lblgrouperr.Text = Convert.ToString(ex);
            lblgrouperr.Visible = true;
        }
    }
    protected void btngroupexit_Click(object sender, EventArgs e)
    {
        try
        {
            PnGroup.Visible = false;
        }
        catch (Exception ex)
        {
            lblgrouperr.Text = Convert.ToString(ex);
            lblgrouperr.Visible = true;
        }
    }
}