using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

public partial class LibraryMod_NonMemberEntry : System.Web.UI.Page
{
    string collegecode = string.Empty;
    string clg_code = string.Empty;
    string usercollegecode = string.Empty;
    string singleuser = string.Empty;
    string groupuser = string.Empty;
    string usercode = string.Empty;
    string user_name = string.Empty;
    string user_id = string.Empty;
    string reg_date = string.Empty;
    string memtype = string.Empty;
    string depart = string.Empty;
    string designation = string.Empty;
    string gender = string.Empty;
    string qry = string.Empty;
    string useid = string.Empty;
    string name = string.Empty;
    string peradd = string.Empty;
    string perpin = string.Empty;
    string perphone = string.Empty;
    string peremail = string.Empty;
    string tempadd = string.Empty;
    string temppin = string.Empty;
    string tempphone = string.Empty;
    string tempemail = string.Empty;
    string regdate = string.Empty;
    string member_type = string.Empty;
    string deptartment = string.Empty;
    string desig = string.Empty;
    string dateofbirth = string.Empty;
    string gend = string.Empty;
    string NoofCards = string.Empty;
    string collcode = string.Empty;
    Boolean Cellclick = false;
    DataTable nonmem = new DataTable();
    Hashtable ht = new Hashtable();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    DataRow dr;
    public SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    public SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    bool flag_true = false;
    static int searchby = 0;
    static string searchclgcode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("LibraryHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/LibraryMod/LibraryHome.aspx");
                    return;
                }
            }
          
                usercollegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupuser = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
           
            if (!IsPostBack)
            {
                bindclg();
                bindsearchby();
                txt_fromdate1.Attributes.Add("readonly", "readonly");
                txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate1.Attributes.Add("readonly", "readonly");
                txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtclsdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            bindsearchby();
        }
        catch
        {
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearch(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();
        if (searchby == 1)
        {
            query = "SELECT DISTINCT  TOP  100 name FROM user_master where name Like '" + prefixText + "%'  AND college_code='" + searchclgcode + "' order by name";
        }
        else if (searchby == 2)
        {
            query = "SELECT DISTINCT  TOP  100 user_id FROM user_master where user_id Like '" + prefixText + "%'  AND college_code='" + searchclgcode + "' order by user_id";
        }
        else if (searchby == 5)
        {
            query = "SELECT DISTINCT  TOP  100 department FROM user_master where department Like '" + prefixText + "%'  AND college_code='" + searchclgcode + "' order by department";
        }
        else if (searchby == 6)
        {
            query = "SELECT DISTINCT  TOP  100 desig_name FROM user_master where desig_name Like '" + prefixText + "%'  AND college_code='" + searchclgcode + "' order by desig_name";
        }

        values = ws.Getname(query);
        return values;
    }

    public void bindclg()
    {
        try
        {
            ddlclg.Items.Clear();
            string columnfiel = string.Empty;
            string group_user = ((Session["group_user"] != null) ? Convert.ToString(Session["group_user"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "True" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE")
            {
                columnfiel = " and group_user ='" + group_user + "'";

            }
            else if (Session["group_user"] != null)
            {
                columnfiel = " and user_code='" + Convert.ToString(Session["user_code"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfiel));
            DataSet biclg = da.select_method("bind_college", ht, "sp");
            ddlclg.Items.Clear();
            if (biclg.Tables.Count > 0 && biclg.Tables[0].Rows.Count > 0)
            {
                clg_code = Convert.ToString(biclg.Tables[0].Rows[0]["college_code"]);
                ddlclg.DataSource = biclg;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                ddlclg.SelectedIndex = 0;


                searchclgcode = Convert.ToString(ddlclg.SelectedValue);
            }

        }

        catch
        {
        }

    }

    public void bindsearchby()
    {
        try
        {
            if (ddlserach.SelectedIndex == 1 || ddlserach.SelectedIndex == 2 || ddlserach.SelectedIndex == 5 || ddlserach.SelectedIndex == 6)
            {
                txtusernam.Visible = true;
                txt_fromdate1.Visible = false;
                txt_todate1.Visible = false;
                ddlgender.Visible = false;
                ddlmemtype.Visible = false;
                lbl_todate.Visible = false;
                lblfrom.Visible = false;
            }
            else if (ddlserach.SelectedIndex == 3)
            {
                txt_fromdate1.Visible = true;
                txt_todate1.Visible = true;
                txtusernam.Visible = false;
                ddlmemtype.Visible = false;
                ddlgender.Visible = false;
                lbl_todate.Visible = true;
                lblfrom.Visible = true;

            }
            else if (ddlserach.SelectedIndex == 4)
            {
                ddlmemtype.Visible = true;
                ddlgender.Visible = false;
                txtusernam.Visible = false;
                txt_todate1.Visible = false;
                txt_fromdate1.Visible = false;
                lbl_todate.Visible = false;
                lblfrom.Visible = false;
            }
            else if (ddlserach.SelectedIndex == 7)
            {
                ddlmemtype.Visible = false;
                ddlgender.Visible = true;
                txt_fromdate1.Visible = false;
                txt_todate1.Visible = false;
                txtusernam.Visible = false;
                lbl_todate.Visible = false;
                lblfrom.Visible = false;
            }
            else if (ddlserach.SelectedIndex == 0)
            {
                ddlmemtype.Visible = false;
                ddlgender.Visible = false;
                txt_fromdate1.Visible = false;
                txt_todate1.Visible = false;
                txtusernam.Visible = false;
                lbl_todate.Visible = false;
                lblfrom.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void ddlcollege_selectedindexchange(object sender, EventArgs e)
    {

        searchclgcode = Convert.ToString(ddlclg.SelectedValue);
    }

    protected void ddlsearch_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            txtusernam.Text = "";
            searchby = ddlserach.SelectedIndex;

        }
        catch
        {

        }
    }

    protected void ddlgender_selectedindexchange(object sender, EventArgs e)
    {
    }

    protected void ddlmemtype_selectedindexchange(object sender, EventArgs e)
    {
    }

    protected void grdNonMem_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdNonMem.PageIndex = e.NewPageIndex;
        btn_go_click(sender, e);
    }

    protected void btn_go_click(object sender, EventArgs e)
    {
        try
        {
            string genderno = string.Empty;
            string staffno = string.Empty;
            string qry1 = string.Empty;
            if (ddlclg.Items.Count > 0)
            {
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collcode))
                        {
                            collcode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collcode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (ddlserach.SelectedIndex == 7)
            {
                if (ddlgender.SelectedIndex == 1)
                {
                    genderno = "0";
                }
                else if (ddlgender.SelectedIndex == 2)
                {
                    genderno = "1";
                }
                qry1 = "and gender='" + genderno + "'";

            }
            else if (ddlserach.SelectedIndex == 1)
            {
                qry1 = " and name='" + txtusernam.Text + "'";

            }
            else if (ddlserach.SelectedIndex == 2)
            {
                qry1 = " and  user_id='" + txtusernam.Text + "'";

            }
            else if (ddlserach.SelectedIndex == 3)
            {
                string fromdate = txt_fromdate1.Text;
                string todate = txt_todate1.Text;

                dt = Convert.ToDateTime(fromdate);
                fromdate = dt.ToString("yyyy-MM-dd");


                dt1 = Convert.ToDateTime(todate);
                todate = dt1.ToString("yyyy-MM-dd");

                qry1 = " and date_of_reg between '" + fromdate + "' and '" + todate + "'";
            }
            else if (ddlserach.SelectedIndex == 4)
            {
                if (ddlmemtype.SelectedIndex == 1)
                {
                    staffno = "0";
                }
                else if (ddlmemtype.SelectedIndex == 2)
                {
                    staffno = "1";
                }
                qry1 = " and gender='" + staffno + "'";
            }
            else if (ddlserach.SelectedIndex == 5)
            {
                qry1 = " and department='" + Convert.ToString(ddldept1.Text) + "' ";
            }
            else if (ddlserach.SelectedIndex == 6)
            {
                qry1 = "and desig_name='" + txtusernam.Text + "'";
            }

            qry = "select * from user_master where college_code=" + collcode + " " + qry1 + "";
            ds1 = da.select_method_wo_parameter(qry, "text");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                nonmem.Columns.Add("SNo", typeof(string));
                nonmem.Columns.Add("User ID", typeof(string));
                nonmem.Columns.Add("Name", typeof(string));
                nonmem.Columns.Add("Permanent Address", typeof(string));
                nonmem.Columns.Add("Permanent Pincode", typeof(string));
                nonmem.Columns.Add("Permanent Phone", typeof(string));
                nonmem.Columns.Add("Permanent Email", typeof(string));
                nonmem.Columns.Add("Temporary Address", typeof(string));
                nonmem.Columns.Add("Temporary Pincode", typeof(string));
                nonmem.Columns.Add("Temporary Phone", typeof(string));
                nonmem.Columns.Add("Temporary Email", typeof(string));
                nonmem.Columns.Add("Registration Date", typeof(string));
                nonmem.Columns.Add("Member Type", typeof(string));
                nonmem.Columns.Add("Department", typeof(string));
                nonmem.Columns.Add("Designation", typeof(string));
                nonmem.Columns.Add("Date Of Birth", typeof(string));
                nonmem.Columns.Add("Gender", typeof(string));
                nonmem.Columns.Add("No Of Cards", typeof(string));

            

                int sno = 0;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    dr = nonmem.NewRow();
                    useid = Convert.ToString(ds1.Tables[0].Rows[i]["user_id"]);
                    name = Convert.ToString(ds1.Tables[0].Rows[i]["name"]);
                    peradd = Convert.ToString(ds1.Tables[0].Rows[i]["perm_addr"]);
                    perpin = Convert.ToString(ds1.Tables[0].Rows[i]["perm_pin"]);
                    perphone = Convert.ToString(ds1.Tables[0].Rows[i]["perm_phone"]);
                    peremail = Convert.ToString(ds1.Tables[0].Rows[i]["perm_email"]);
                    tempadd = Convert.ToString(ds1.Tables[0].Rows[i]["temp_addr"]);
                    temppin = Convert.ToString(ds1.Tables[0].Rows[i]["temp_pin"]);
                    tempphone = Convert.ToString(ds1.Tables[0].Rows[i]["temp_phone"]);
                    tempemail = Convert.ToString(ds1.Tables[0].Rows[i]["temp_email"]);
                    regdate = Convert.ToString(ds1.Tables[0].Rows[i]["date_of_reg"]);
                    string[] dtregdate = regdate.Split('/');
                    if (dtregdate.Length == 3)
                        regdate = dtregdate[1].ToString() + "/" + dtregdate[0].ToString() + "/" + dtregdate[2].ToString();
                    member_type = Convert.ToString(ds1.Tables[0].Rows[i]["memberType"]);


                    depart = Convert.ToString(ds1.Tables[0].Rows[i]["department"]);
                    desig = Convert.ToString(ds1.Tables[0].Rows[i]["desig_name"]);
                    dateofbirth = Convert.ToString(ds1.Tables[0].Rows[i]["date_of_birth"]);
                    string[] dtDob = dateofbirth.Split('/');
                    if (dtDob.Length == 3)
                        dateofbirth = dtDob[1].ToString() + "/" + dtDob[0].ToString() + "/" + dtDob[2].ToString();

                    gend = Convert.ToString(ds1.Tables[0].Rows[i]["gender"]);
                    if (gend == "1")
                    {
                        gend = "Male";
                    }
                    else
                    {
                        gend = "Female";
                    }
                    NoofCards = Convert.ToString(ds1.Tables[0].Rows[i]["no_of_cards"]);
                    dr["SNo"] = Convert.ToString(sno);
                    dr["User ID"] = useid;
                    dr["Name"] = name;
                    dr["Permanent Address"] = peradd;
                    dr["Permanent Pincode"] = perpin;
                    dr["Permanent Phone"] = perphone;
                    dr["Permanent Email"] = peremail;
                    dr["Temporary Address"] = tempadd;
                    dr["Temporary Pincode"] = temppin;
                    dr["Temporary Phone"] = tempphone;
                    dr["Temporary Email"] = tempemail;
                    dr["Registration Date"] = regdate.Split(' ')[0];
                    dr["Member Type"] = member_type;
                    dr["Department"] = depart;
                    dr["Designation"] = desig;
                    dr["Date Of Birth"] = dateofbirth.Split(' ')[0];
                    dr["Gender"] = gend;
                    dr["No Of Cards"] = NoofCards;
                    nonmem.Rows.Add(dr);
                }
                chkGridSelectAll.Visible = true;
                grdNonMem.DataSource = nonmem;
                grdNonMem.DataBind();
                grdNonMem.Visible = true;
                div_report.Visible = true;
               
            }
            if (ds1.Tables[0].Rows.Count > 0)
            {
                divtable.Visible = true;
                btndelete.Visible = true;

            }
            else
            {
                chkGridSelectAll.Visible = false;
                grdNonMem.Visible = false;
                divtable.Visible = false;
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
            }

        }
        catch
        {
        }
    }

   

    protected void grdNonMem_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdNonMem_RowDataBound(object sender, GridViewRowEventArgs e)
    {



        if (e.Row.RowIndex == 0)
        {
            e.Row.Cells[0].Text = "Select";
        }
    }

    protected void grdNonMem_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            bindept();
            btnsave.Visible = false;
            btnupdate.Visible = true;
            divaddnew.Visible = true;
            divaddnew1.Visible = true;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            if (Convert.ToString(rowIndex) != "-1" && Convert.ToString(selectedRow) != "")
            {
                string userid = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[2].Text);
                string name = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[3].Text);
                string peraddress = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[4].Text);
                if (peraddress == "&nbsp;")
                {
                    peraddress = "";
                }
                string perpincode = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[5].Text);
                if (perpincode == "&nbsp;")
                {
                    perpincode = "";
                }
                string perphone = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[6].Text);
                if (perphone == "&nbsp;")
                {
                    perphone = "";
                }
                string peremail = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[7].Text);
                if (peremail == "&nbsp;")
                {
                    peremail = "";
                }
                string temaddress = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[8].Text);
                if (temaddress == "&nbsp;")
                {
                    temaddress = "";
                }
                string tempincode = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[9].Text);
                if (tempincode == "&nbsp;")
                {
                    tempincode = "";
                }
                string temphone = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[10].Text);
                if (temphone == "&nbsp;")
                {
                    temphone = "";
                }
                string tememail = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[11].Text);
                if (tememail == "&nbsp;")
                {
                    tememail = "";
                }
                string registerdate = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[12].Text);
                string memtype = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[13].Text);
                if (memtype == "&nbsp;")
                {
                    memtype = "";
                }
                string dept = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[14].Text);
                string design = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[15].Text);
                if (design == "&nbsp;")
                {
                    design = "";
                }
                string dob = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[16].Text);
                string noofcards = Convert.ToString(grdNonMem.Rows[rowIndex].Cells[18].Text);

                txtuserid.Text = userid;
                txtname.Text = name;
                txtperadd.Text = peraddress;
                txtpin.Text = perpincode;
                txtphone.Text = perphone;
                txtemail.Text = peremail;
                ddldept1.Text = dept;
                txtdesig.Text = design;
                txtnoc.Text = noofcards;
                txtdob.Text = dob;
                txttempadd.Text = temaddress;
                txtpincode.Text = tempincode;
                txtphno.Text = temphone;
                txtemailid.Text = tememail;
                txtdoreg.Text = registerdate;

                string sql = "select no_of_days,fine from lib_master where code='" + userid + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtdue.Text = Convert.ToString(ds.Tables[0].Rows[0]["no_of_days"]);
                    txtfine.Text = Convert.ToString(ds.Tables[0].Rows[0]["fine"]);
                }
                if (memtype.ToLower() == "student")
                {
                    imgstudp.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + userid + " ";
                }
                if (memtype.ToLower() == "staff")
                {
                    imgstudp.ImageUrl = "~/Handler/staffphoto.ashx?Staff_code=" + userid + " ";
                }
                if (memtype.ToLower() == "visitor")
                {
                    imgstudp.ImageUrl = "~/Handler/VisitorPhoto.ashx?VisitorID=" + userid + " ";
                }
            }
        }

        catch
        {
        }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            int insertqry;
            string gen = string.Empty;
            string actinact = string.Empty;
            string memtyn = string.Empty;
            string genderno = string.Empty;
            string staffno = string.Empty;
            string qry1 = string.Empty;
            string memberType = string.Empty;
            btnupdate.Visible = true;
            if (ddlclg.Items.Count > 0)
            {
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collcode))
                        {
                            collcode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collcode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (ddlserach.SelectedIndex == 7)
            {
                if (ddlgender.SelectedIndex == 1)
                {
                    genderno = "0";
                }
                else if (ddlgender.SelectedIndex == 2)
                {
                    genderno = "1";
                }
                qry1 = "and gender='" + genderno + "'";

            }
            else if (ddlserach.SelectedIndex == 1)
            {
                qry1 = " and name='" + txtusernam.Text + "'";

            }
            else if (ddlserach.SelectedIndex == 2)
            {
                qry1 = " and  user_id='" + txtusernam.Text + "'";

            }
            else if (ddlserach.SelectedIndex == 3)
            {

                string fromdate = txt_fromdate1.Text;
                string todate = txt_todate1.Text;

                dt = Convert.ToDateTime(fromdate);
                fromdate = dt.ToString("yyyy-MM-dd");


                dt1 = Convert.ToDateTime(todate);
                todate = dt1.ToString("yyyy-MM-dd");

                qry1 = " and date_of_reg between '" + fromdate + "' and '" + todate + "'";
            }
            else if (ddlserach.SelectedIndex == 4)
            {
                if (ddlmemtype.SelectedIndex == 1)
                {
                    staffno = "0";
                }
                else if (ddlmemtype.SelectedIndex == 2)
                {
                    staffno = "1";
                }
                qry1 = " and gender='" + staffno + "'";
            }
            else if (ddlserach.SelectedIndex == 5)
            {
                qry1 = " and department='" + Convert.ToString(ddldept1.Text) + "' ";
            }
            else if (ddlserach.SelectedIndex == 6)
            {
                qry1 = "and desig_name='" + txtusernam.Text + "'";
            }
            if (rblStatus.SelectedIndex == 0)
            {
                gen = "0";
            }
            else
            {
                gen = "1";
            }
            if (cbstatus.Checked == true)
            {
                actinact = "1";
            }
            else
            {
                actinact = "0";
            }
            if (rblmemty.SelectedIndex == 0)
            {
                memtyn = "0";
                memberType = "student";
            }
            else if (rblmemty.SelectedIndex == 1)
            {
                memtyn = "1";
                memberType = "staff";
            }
            else
            {
                memtyn = "2";
                memberType = "visitor";
            }
            string dateofbirth = string.Empty;
            dateofbirth = txtdob.Text;
            string dob = string.Empty;
            string[] dtDob = dateofbirth.Split('/');
            if (dtDob.Length == 3)
                dob = dtDob[1].ToString() + "/" + dtDob[0].ToString() + "/" + dtDob[2].ToString();

            string dateofreg = string.Empty;
            dateofreg = txtdoreg.Text;
            string dor = string.Empty;
            string[] dtdor = dateofreg.Split('/');
            if (dtdor.Length == 3)
                dor = dtdor[1].ToString() + "/" + dtdor[0].ToString() + "/" + dtdor[2].ToString();

            // dt1 = Convert.ToDateTime(dateofreg); 
            string closedate = string.Empty;
            closedate = txtclsdate.Text;
            string[] dtclose = closedate.Split('/');
            if (dtclose.Length == 3)
                closedate = dtclose[1].ToString() + "/" + dtclose[0].ToString() + "/" + dtclose[2].ToString();



            if (dateofbirth != "" && dateofreg != "")
            {
                //string[] dob = dateofbirth.Split('-');
                //string[] dor = dateofreg.Split('-');
                if (dob == dor)
                {
                    if (Convert.ToDateTime(dob) >= Convert.ToDateTime(dor))
                    {
                        divPopupAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        divAlertContent.Visible = true;
                        lblAlertMsg.Text = "Date of Registration should be greater than Date of Birth";
                        return;
                    }
                }
                else
                {
                    if (Convert.ToDateTime(dob) >= Convert.ToDateTime(dor))
                    {
                        divPopupAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        divAlertContent.Visible = true;
                        lblAlertMsg.Text = "Date of Registration Year should be greater than Date of Birth Year";
                        return;
                    }

                }
            }
            string insertqry1 = "update user_master set   name= '" + txtname.Text + "' , perm_addr='" + txtperadd.Text + "' , perm_pin='" + txtpin.Text + "' , perm_phone='" + txtphone.Text + "' , perm_email='" + txtemail.Text + "' , temp_addr='" + txttempadd.Text + "' , temp_pin='" + txtpincode.Text + "' , temp_phone='" + txtphno.Text + "' , temp_email='" + txtemailid.Text + "' , date_of_reg='" + dor + "' , is_staff='" + memtyn + "' , desig_name= '" + txtdesig.Text + "' , no_of_cards='" + txtnoc.Text + "' , date_of_birth='" + dob + "' , gender='" + gen + "' ,  department= '" + ddldept1.SelectedItem.ToString() + "' , college_code=" + collcode + " , Status= '" + actinact + "' , CloseDate='" + closedate + "',membertype ='" + memberType + "' where user_id='" + txtuserid.Text + "'  ";//update user_master set  (user_id,name,perm_addr,perm_pin,perm_phone,perm_email,temp_addr,temp_pin,temp_phone,temp_email,date_of_reg,is_staff,desig_name,no_of_cards,date_of_birth,gender,department,college_code,Status,CloseDate) values('a11','aparna','','','','','','','','','06/07/2018','2','staff','2','02/02/1999','1','COMPUTER SCIENCE AND ENGINEERING','13','1','07/10/2018') else update user_master set   name= 'aparna' , perm_addr='' , perm_pin='' , perm_phone='' , perm_email='' , temp_addr='' , temp_pin='' , temp_phone='' , temp_email='' , date_of_reg='06/07/2018' , is_staff='2' , desig_name= 'staff' , no_of_cards='2' , date_of_birth='02/02/1999' , gender='1' ,  department= 'COMPUTER SCIENCE AND ENGINEERING' , college_code='13' , Status= '1' , CloseDate='07/10/2018' where user_id='a11'  
            insertqry = da.update_method_wo_parameter(insertqry1, "text");

            string selQry = "SELECT COUNT(*) FROM TokenDetails WHERE Roll_No ='" + txtuserid.Text + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";

            string MaxCard = da.GetFunction(selQry);
            int StrMaxCard = Convert.ToInt32(MaxCard);
            int NoOfCards = Convert.ToInt32(txtnoc.Text);
            string StrTokNo = string.Empty;
            string userID = Convert.ToString(txtuserid.Text);
            string insertQry = "";
            int insert = 0;
            for (int k = StrMaxCard + 1; k <= (StrMaxCard + NoOfCards); k++)
            {
                StrTokNo = userID + "." + k;
                selQry = "Select token_no from tokendetails where Roll_No ='" + userID + "' AND token_no='" + StrTokNo + "' AND Is_Staff ='" + memtyn + "' ";
                ds.Clear();
                ds = da.select_method_wo_parameter(selQry, "Text");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    string Time = DateTime.Now.ToString("HH:MM:ss tt");
                    string Date = DateTime.Now.ToString("MM/dd/yyy");
                    insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time,is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + userID + "','" + txtname.Text + "','" + memtyn + "','" + ddldept1.SelectedItem.ToString() + "','" + Date + "', '" + Time + "','0','All','All',0,'','All','All','All')";
                    insert = da.update_method_wo_parameter(insertQry, "Text");
                }
            }
            if (insertqry == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Text = "Records Not Saved";
                grdNonMem.Visible = false;
                divaddnew.Visible = false;
                divaddnew1.Visible = false;
                itemsclear();
                //div_report.Visible = false;

            }
            else
            {
                if (ViewState["studentimage"] != "0" && ViewState["size"] != "")
                {
                    byte[] photoid = (byte[])(ViewState["studentimage"]);
                    int size = Convert.ToInt32(ViewState["size"]);
                    photosave(txtuserid.Text, size, photoid);

                }
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Updated Successfully";
                grdNonMem.Visible = false;
                divaddnew.Visible = false;
                divaddnew1.Visible = false;
                itemsclear();
                //div_report.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void btn_print_click(object sender, EventArgs e)
    {

    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        lblAlertMsg.Visible = false;
        divPopupAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    protected void btn_add_click(object sender, EventArgs e)
    {
        divaddnew.Visible = true;
        divaddnew1.Visible = true;
        bindept();
        txtclsdate.Attributes.Add("readonly", "readonly");
        txtclsdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        txtdob.Attributes.Add("readonly", "readonly");
        txtdob.Text = DateTime.Now.ToString("dd/MM/yyyy");

        txtdoreg.Attributes.Add("readonly", "readonly");
        txtdoreg.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    protected void ddldept1_selectedindexchange(object sender, EventArgs e)
    {
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            int insertqry;
            string gen = string.Empty;
            string actinact = string.Empty;
            string memtyn = string.Empty;
            string membertype = "";
            if (ddlclg.Items.Count > 0)
            {
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collcode))
                        {
                            collcode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collcode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (txtuserid.Text == "")
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The User ID";
                btnPopAlertClose.Visible = true;
                return;
            }
            if (txtuserid.Text == "")
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The User ID";
                btnPopAlertClose.Visible = true;
                return;
            }
            if (txtname.Text == "")
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The User Name";
                btnPopAlertClose.Visible = true;
                return;
            }
            if (txtnoc.Text == "")
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The Number Of Cards";
                btnPopAlertClose.Visible = true;
                return;
            }
            if (txtdue.Text == "")
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The Due Days";
                btnPopAlertClose.Visible = true;
                return;
            }
            if (txtfine.Text == "")
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter The Fine Amount";
                btnPopAlertClose.Visible = true;
                return;
            }
            if (rblStatus.SelectedIndex == 0)
            {
                gen = "0";
            }
            else
            {
                gen = "1";
            }
            if (cbstatus.Checked == true)
            {
                actinact = "1";
            }
            else
            {
                actinact = "0";
            }
            if (rblmemty.SelectedIndex == 0)
            {
                memtyn = "0";
                membertype = "student";
            }
            else if (rblmemty.SelectedIndex == 1)
            {
                memtyn = "1";
                membertype = "staff";
            }
            else
            {
                memtyn = "2";
                membertype = "visitor";
            }
            //string dateofbirth = string.Empty;
            //dateofbirth = txtdob.Text;
            //DateTime dt = new DateTime();
            //dt = Convert.ToDateTime(dateofbirth);
            //dateofbirth = dt.ToString("yyyy-MM-dd");
            //string dateofreg = string.Empty;
            //dateofreg = txtdoreg.Text;
            //DateTime dt1 = new DateTime();
            //dt1 = Convert.ToDateTime(dateofreg);
            //dateofreg = dt1.ToString("yyyy-MM-dd");
            //string closedate = string.Empty;
            //closedate = txtclsdate.Text;
            //DateTime dt2 = new DateTime();
            //dt2 = Convert.ToDateTime(closedate);
            //closedate = dt2.ToString("yyyy-MM-dd");

            string dateofbirth = string.Empty;
            dateofbirth = txtdob.Text;
            string dob = string.Empty;
            string[] dtDob = dateofbirth.Split('/');
            if (dtDob.Length == 3)
                dob = dtDob[1].ToString() + "/" + dtDob[0].ToString() + "/" + dtDob[2].ToString();

            string dateofreg = string.Empty;
            dateofreg = txtdoreg.Text;
            string dor = string.Empty;
            string[] dtdor = dateofreg.Split('/');
            if (dtdor.Length == 3)
                dor = dtdor[1].ToString() + "/" + dtdor[0].ToString() + "/" + dtdor[2].ToString();

            string closedate = string.Empty;
            closedate = txtclsdate.Text;
            string[] dtclose = closedate.Split('/');
            if (dtclose.Length == 3)
                closedate = dtclose[1].ToString() + "/" + dtclose[0].ToString() + "/" + dtclose[2].ToString();

            if (dateofbirth != "" && dateofreg != "")
            {
                //string[] dob = dateofbirth.Split('-');
                //string[] dor = dateofreg.Split('-');
                if (dob == dor)
                {
                    if (Convert.ToDateTime(dob) >= Convert.ToDateTime(dor))
                    {
                        divPopupAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        divAlertContent.Visible = true;
                        lblAlertMsg.Text = "Date of Registration should be greater than Date of Birth";
                        return;
                    }
                }
                else
                {
                    if (Convert.ToDateTime(dob) >= Convert.ToDateTime(dor))
                    {
                        divPopupAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        divAlertContent.Visible = true;
                        lblAlertMsg.Text = "Date of Registration Year should be greater than Date of Birth Year";
                        return;
                    }
                }
            }

            string insertqry1 = "if not exists (select * from user_master  where user_id='" + txtuserid.Text + "' and name= '" + txtname.Text + "' and perm_addr='" + txtperadd.Text + "' and perm_pin='" + txtpin.Text + "' and perm_phone='" + txtphone.Text + "' and perm_email='" + txtemail.Text + "' and temp_addr='" + txttempadd.Text + "' and temp_pin='" + txtpincode.Text + "' and temp_phone='" + txtphno.Text + "' and temp_email='" + txtemailid.Text + "' and date_of_reg='" + dor + "' and is_staff='" + memtyn + "' and desig_name= '" + txtdesig.Text + "' and no_of_cards='" + txtnoc.Text + "' and date_of_birth='" + dob + "' and gender='" + gen + "' and  department= '" + ddldept1.SelectedItem.ToString() + "' and college_code=" + collcode + " and Status= '" + actinact + "' and CloseDate='" + closedate + "') insert into user_master (user_id,name,perm_addr,perm_pin,perm_phone,perm_email,temp_addr,temp_pin,temp_phone,temp_email,date_of_reg,is_staff,desig_name,no_of_cards,date_of_birth,gender,department,college_code,Status,CloseDate,membertype) values('" + txtuserid.Text + "','" + txtname.Text + "','" + txtperadd.Text + "','" + txtpin.Text + "','" + txtphone.Text + "','" + txtemail.Text + "','" + txttempadd.Text + "','" + txtpincode.Text + "','" + txtphno.Text + "','" + txtemailid.Text + "','" + dor + "','" + memtyn + "','" + txtdesig.Text + "','" + txtnoc.Text + "','" + dob + "','" + gen + "','" + ddldept1.SelectedItem.ToString() + "'," + collcode + ",'" + actinact + "','" + closedate + "','" + membertype + "') else update user_master set   name= '" + txtname.Text + "' , perm_addr='" + txtperadd.Text + "' , perm_pin='" + txtpin.Text + "' , perm_phone='" + txtphone.Text + "' , perm_email='" + txtemail.Text + "' , temp_addr='" + txttempadd.Text + "' , temp_pin='" + txtpincode.Text + "' , temp_phone='" + txtphno.Text + "' , temp_email='" + txtemailid.Text + "' , date_of_reg='" + dor + "' , is_staff='" + memtyn + "' , desig_name= '" + txtdesig.Text + "' , no_of_cards='" + txtnoc.Text + "' , date_of_birth='" + dob + "' , gender='" + gen + "' ,  department= '" + ddldept1.SelectedItem.ToString() + "' , college_code=" + collcode + " , Status= '" + actinact + "' , CloseDate='" + closedate + "',membertype='" + membertype + "' where user_id='" + txtuserid.Text + "'  ";
            insertqry = da.update_method_wo_parameter(insertqry1, "text");

            string insertQry = "";
            int insert = 0;
            string selQry = "SELECT * FROM Lib_Master WHERE Code ='" + ddldept1.SelectedItem.ToString() + "' AND Is_Staff = '" + memtyn + "' ";
            ds.Clear();
            ds = da.select_method_wo_parameter(selQry, "Text");
            if (ds.Tables[0].Rows.Count == 0)
            {
                selQry = "SELECT * FROM user_master WHERE user_id='" + txtuserid.Text + "' and department ='" + ddldept1.SelectedItem.ToString() + "' ";
                ds1.Clear();
                ds1 = da.select_method_wo_parameter(selQry, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    insertQry = "INSERT INTO Lib_Master(code,code_descp,batch_year,no_of_token,no_of_days,fine,is_staff,OverNightFine, category, studcategory,Book_Type,IndCategory,Ref_NoofDays,TransLibCode,FineType,CardCat) VALUES('" + txtuserid.Text + "','" + ddldept1.SelectedItem.ToString() + "','0'," + txtnoc.Text + "," + txtdue.Text + "," + txtfine.Text + ",'" + memtyn + "',0,'All','All','All',0,0,'All',0,'All') ";

                    insert = da.update_method_wo_parameter(insertQry, "TEXT");

                    selQry = "SELECT COUNT(*) FROM TokenDetails WHERE Roll_No ='" + txtuserid.Text + "' AND ISNULL(Category,'All') = 'All' AND ISNULL(StudCategory,'All') = 'All' AND IndCategory = 0 AND Is_Staff = 1 ";

                    string MaxCard = da.GetFunction(selQry);
                    int StrMaxCard = Convert.ToInt32(MaxCard);
                    int NoOfCards = Convert.ToInt32(txtnoc.Text);
                    string StrTokNo = string.Empty;
                    string userID = Convert.ToString(txtuserid.Text);
                    for (int k = StrMaxCard + 1; k <= (StrMaxCard + NoOfCards); k++)
                    {
                        StrTokNo = userID + "." + k;
                        selQry = "Select token_no from tokendetails where Roll_No ='" + userID + "' AND token_no='" + StrTokNo + "' AND Is_Staff ='" + memtyn + "' ";
                        ds.Clear();
                        ds = da.select_method_wo_parameter(selQry, "Text");
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            string Time = DateTime.Now.ToString("HH:MM:ss tt");
                            string Date = DateTime.Now.ToString("MM/dd/yyy");
                            insertQry = "insert into tokendetails(token_no,roll_no,stud_name,is_staff,dept_name,access_date,access_time,is_locked,category,studcategory,indcategory,Renew_Days,TransLibCode,Book_Type,CardCat) values('" + StrTokNo + "','" + userID + "','" + txtname.Text + "','" + memtyn + "','" + ddldept1.SelectedItem.ToString() + "','" + Date + "', '" + Time + "','0','All','All',0,'','All','All','All')";
                            insert = da.update_method_wo_parameter(insertQry, "Text");
                        }
                    }
                }
            }
            if (insertqry == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Text = "Records Not Saved";
                grdNonMem.Visible = false;
                divaddnew.Visible = false;
                divaddnew1.Visible = false;
                itemsclear();
                //div_report.Visible = false;
            }
            else
            {
                if (ViewState["studentimage"] != "0" && ViewState["size"] != "")
                {
                    byte[] photoid = (byte[])(ViewState["studentimage"]);
                    int size = Convert.ToInt32(ViewState["size"]);
                    photosave(txtuserid.Text, size, photoid);

                }
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Saved Successfully";
                grdNonMem.Visible = false;
                divaddnew.Visible = false;
                divaddnew1.Visible = false;
                itemsclear();
                //div_report.Visible = false;
            }

        }
        catch
        {
        }



    }

    protected void btn_delete_click(object sender, EventArgs e)
    {
        try
        {
            div1.Visible = true;
            div2.Visible = true;
            lbldeletealter.Visible = true;
            lbldeletealter.Text = "Are you sure to delete?";
            btnyes.Visible = true;
            btnNo.Visible = true;



        }

        catch
        {
        }
    }

    protected void btnPopAlertyes_Click(object sender, EventArgs e)
    {
        try
        {

            string userid = string.Empty;
            int del = 0;
            string delqry = string.Empty;
            //var grid = (GridView)sender;
            //GridViewRow selectedRow = grid.SelectedRow;
            //int rowIndex = grid.SelectedIndex;
            //int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            int selected1 = 0;


            foreach (GridViewRow row in grdNonMem.Rows)
            {
                CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                if (!cbsel.Checked)
                    continue;


                user_id = Convert.ToString(row.Cells[2].Text);
                delqry = "DELETE FROM user_master WHERE user_id='" + user_id + "'";
                del = da.update_method_wo_parameter(delqry, "text");

            }
            if (del > 0)
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Deleted Successfully";
                grdNonMem.Visible = false;
                divaddnew.Visible = false;
                divaddnew1.Visible = false;
                div1.Visible = false;
                div2.Visible = false;
                //div_report.Visible = false;
            }
            else
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Not Deleted";
                grdNonMem.Visible = false;
                divaddnew.Visible = false;
                divaddnew1.Visible = false;
                div1.Visible = false;
                div2.Visible = false;
                //div_report.Visible = false;
            }

        }

        catch
        {
        }
    }

    protected void btnPopAlertNo_Click(object sender, EventArgs e)
    {
        div1.Visible = false;
        div2.Visible = false;
        grdNonMem.Visible = false;
        divaddnew.Visible = false;
        divaddnew1.Visible = false;
    }

    public void itemsclear()
    {
        txtuserid.Text = string.Empty;
        txtname.Text = string.Empty;
        txtperadd.Text = string.Empty;
        txtpin.Text = string.Empty;
        txtphone.Text = string.Empty;
        txtemail.Text = string.Empty;
        txttempadd.Text = string.Empty;
        txtpincode.Text = string.Empty;
        txtphno.Text = string.Empty;
        txtdoreg.Text = string.Empty;
        txtdesig.Text = string.Empty;
        txtnoc.Text = string.Empty;
        txtdob.Text = string.Empty;
        collcode = string.Empty;
        txtclsdate.Text = string.Empty;
        imgstudp.ImageUrl = null;



    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        divaddnew.Visible = false;
        divaddnew1.Visible = false;
        itemsclear();
        //div_report.Visible = false;


    }

    protected void cbstatus_OnCheckedChanged(object sender, EventArgs e)
    {
    }

    protected void btn_move_Click(object sender, EventArgs e)
    {
        movetotemp();

    }

    public void movetotemp()
    {
        try
        {
            string peradd1 = txtperadd.Text;
            string perpincod = txtpin.Text;
            string phno = txtphone.Text;
            string email = txtemail.Text;

            txttempadd.Text = peradd1;
            txtpincode.Text = perpincod;
            txtphno.Text = phno;
            txtemailid.Text = email;
        }
        catch
        {
        }

    }

    public void bindept()
    {
        try
        {
            if (ddlclg.Items.Count > 0)
            {
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collcode))
                        {
                            collcode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collcode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            string dep = ddldept1.SelectedValue.ToString();
            ds.Clear();
            if (!string.IsNullOrEmpty(collcode))
            {

                string dep1 = "select Dept_Name from Department where college_code=" + collcode + " ";
                ds = da.select_method_wo_parameter(dep1, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldept1.DataSource = ds;
                ddldept1.DataTextField = "Dept_Name";
                ddldept1.DataValueField = "Dept_Name";
                ddldept1.DataBind();

            }
        }
        catch
        {
        }
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Non Member Entry";
            string pagename = "NonMemberEntry.aspx";
            string ss=null;
            Printcontrolhed2.loadspreaddetails(grdNonMem, pagename, attendance, 0, ss,"1");
            Printcontrolhed2.Visible = true;
        }
        catch { }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(grdNonMem, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();

        }
        catch
        {

        }

    }

    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
            if (txt_excelname.Text == "")
            {
                lbl_norec.Visible = true;
            }
            else
            {
                lbl_norec.Visible = false;
            }
        }
        catch { }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void BtnsaveStud_Click(object sender, EventArgs e)
    {
        if (fulstudp.HasFile)
        {
            if (fulstudp.FileName.EndsWith(".jpg") || fulstudp.FileName.EndsWith(".jpeg") || fulstudp.FileName.EndsWith(".JPG") || fulstudp.FileName.EndsWith(".gif") || fulstudp.FileName.EndsWith(".png") || fulstudp.FileName.EndsWith(".gif") || fulstudp.FileName.EndsWith(".bmp"))
            {
                Session["Image"] = fulstudp.PostedFile;
                int fileSize = fulstudp.PostedFile.ContentLength;
                ViewState["size"] = fileSize;
                byte[] documentBinary = new byte[fileSize];
                ViewState["studentimage"] = documentBinary;
                fulstudp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                string base64String = Convert.ToBase64String(documentBinary, 0, documentBinary.Length);
                imgstudp.ImageUrl = "data:image/;base64," + base64String;
            }

        }
    }

    protected int photosave(string userid, int FileSize, byte[] DocDocument)
    {
        int Result = 0;
        try
        {
            string InsPhoto = string.Empty;
            if (txtuserid.Text != "" && FileSize != 0)
            {
                if (rblmemty.SelectedIndex == 1)
                {
                    InsPhoto = "if exists(select staff_code,photo from StaffPhoto where staff_code='" + txtuserid.Text + "')update StaffPhoto set photo=@photoid where staff_code='" + txtuserid.Text + "' else insert into StaffPhoto(staff_code,photo) values ('" + txtuserid.Text + "',@photoid)";
                }
                else if (rblmemty.SelectedIndex == 0)
                {
                    InsPhoto = "if exists(select app_no,photo from StdPhoto where app_no='" + txtuserid.Text + "')update StdPhoto set photo=@photoid where app_no='" + txtuserid.Text + "' else insert into StdPhoto(app_no,photo) values ('" + txtuserid.Text + "',@photoid)";
                }
                else if (rblmemty.SelectedIndex == 2)
                {
                    InsPhoto = "if exists(select VisitorID,VisitorPhoto from VisitorsPhoto WHERE VisitorID='" + txtuserid.Text + "')update VisitorsPhoto set VisitorPhoto=@photoid where VisitorID='" + txtuserid.Text + "' else insert into VisitorsPhoto(VisitorID,VisitorPhoto) values ('" + txtuserid.Text + "',@photoid)";
                }
                SqlCommand cmd = new SqlCommand(InsPhoto, ssql);
                SqlParameter uploadedsubject_name = new SqlParameter("@photoid", SqlDbType.Binary, FileSize);
                uploadedsubject_name.Value = DocDocument;
                cmd.Parameters.Add(uploadedsubject_name);
                ssql.Close();
                ssql.Open();
                Result = cmd.ExecuteNonQuery();
                ssql.Close();
            }
        }
        catch
        {
        }
        return Result;
    }
   
}