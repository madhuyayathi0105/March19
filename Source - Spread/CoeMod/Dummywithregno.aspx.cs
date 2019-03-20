using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

//using BalAccess;
//using DalConnection;
//using System.Windows.Forms;
using System.Data.SqlClient;

public partial class Dummywithregno : System.Web.UI.Page
{
    SqlCommand cmd;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con4 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con5 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    string batchyear = "";
    string degree_code = "";
    string cur_sem = "";
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    protected void Page_Load(object sender, EventArgs e)
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
        if (!Page.IsPostBack)
        {
            try
            {
                sprdViewdummy.Visible = false;
                lbltotstud.Visible = false;
                lblviewstud.Visible = false;
                lblremainstud.Visible = false;
                lblremainstudvies.Visible = false;
                Radioserial.Checked = true;
                ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));



                int year1;
                year1 = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 10; l++)
                {

                    ddlYear.Items.Add(Convert.ToString(year1 - l));

                }
                ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                sprdViewdummy.Sheets[0].RowHeader.Visible = false;
                sprdViewdummy.Sheets[0].ColumnCount = 4;
                sprdViewdummy.Sheets[0].AutoPostBack = true;
                sprdViewdummy.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                sprdViewdummy.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                sprdViewdummy.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                sprdViewdummy.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                sprdViewdummy.Sheets[0].ColumnHeader.RowCount = 2;
                sprdViewdummy.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 4);
                sprdViewdummy.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Reg No With Dummy No";
                sprdViewdummy.Sheets[0].ColumnHeader.Cells[1, 0].Text = "S.No";
                sprdViewdummy.Sheets[0].ColumnHeader.Cells[1, 1].Text = "Reg No";
                sprdViewdummy.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Dummy No";
                sprdViewdummy.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Status";

            }
            catch(Exception ex)
            {

            }
        }


    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmag.Visible = false;
        sprdViewdummy.Visible = false;
        lbltotstud.Visible = false;
        lblviewstud.Visible = false;
        lblremainstud.Visible = false;
        lblremainstudvies.Visible = false;
        ddldate.Items.Clear();

        string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date from exmtt_det as exdt,exmtt as exm where exm.exam_code=exdt.exam_code and exm.exam_month=" + ddlMonth.SelectedIndex.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + " order by exam_date";
        SqlDataAdapter da1 = new SqlDataAdapter(getexamdate, con);
        DataSet ds1 = new DataSet();
        da1.Fill(ds1);
        con.Close();
        con.Open();

        if (ds1.Tables[0].Rows.Count > 0)
        {
            ddldate.DataSource = ds1;
            ddldate.DataValueField = "Exam_date";
            ddldate.DataBind();
            ddldate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmag.Visible = false;
        sprdViewdummy.Visible = false;
        lbltotstud.Visible = false;
        lblviewstud.Visible = false;
        ddldate.Items.Clear();
        string getexamdate = "select distinct convert(varchar(10),exdt.Exam_date,105) as exam_date from exmtt_det as exdt,exmtt as exm where exm.exam_code=exdt.exam_code and exm.exam_month=" + ddlMonth.SelectedIndex.ToString() + " and exm.exam_year=" + ddlYear.SelectedValue.ToString() + " order by exam_date";
        SqlDataAdapter da1 = new SqlDataAdapter(getexamdate, con);
        DataSet ds1 = new DataSet();
        da1.Fill(ds1);
        con.Close();
        con.Open();

        if (ds1.Tables[0].Rows.Count > 0)
        {

            ddldate.DataSource = ds1;
            ddldate.DataValueField = "Exam_date";
            ddldate.DataBind();
            ddldate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));

        }
    }
    protected void ddlexamtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmag.Visible = false;
        sprdViewdummy.Visible = false;
        lbltotstud.Visible = false;
        lblviewstud.Visible = false;
        lblremainstud.Visible = false;
        lblremainstudvies.Visible = false;
    }
    protected void ddldate_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmag.Visible = false;
        sprdViewdummy.Visible = false;
        lbltotstud.Visible = false;
        lblviewstud.Visible = false;
        lblremainstud.Visible = false;
        lblremainstudvies.Visible = false;
        ddlsubject.Items.Clear();
        string subnoquery = "select distinct s.subject_Name as SubjectName ,s.Subject_code as subjectcode from subject s,exmtt e,exmtt_det ex,sub_sem where sub_sem.subtype_no=s.subtype_no  and s.subject_no=ex.subject_no and ex.coll_code=" + Session["collegecode"].ToString() + " and ex.exam_Date=convert(datetime,'" + ddldate.SelectedValue.ToString() + "',103)and ex.exam_code=e.exam_code and e.Exam_Month=" + ddlMonth.SelectedValue.ToString() + " and e.Exam_Year=" + ddlYear.SelectedValue.ToString() + " and e.exam_type='Univ'";
        SqlDataAdapter dasubnoquery = new SqlDataAdapter(subnoquery, con);
        DataSet dssubnoquery = new DataSet();
        dasubnoquery.Fill(dssubnoquery);
        con.Close();
        con.Open();

        if (dssubnoquery.Tables[0].Rows.Count > 0)
        {
            ddlsubject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
            int i1 = 1;
            for (int i = 0; i < dssubnoquery.Tables[0].Rows.Count; i++)
            {
                ddlsubject.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dssubnoquery.Tables[0].Rows[i]["SubjectName"].ToString() + "", "" + dssubnoquery.Tables[0].Rows[i]["subjectcode"].ToString() + ""));
                i1++;
            }
        }
    }
    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmag.Visible = false;
        sprdViewdummy.Visible = false;
        string studcount = "select batch_year,degree_code,current_semester,r.roll_no,s.subject_code from registration r,subjectchooser sc,subject s where s.subtype_no=sc.subtype_no and s.subject_no=sc.subject_no and r.roll_no=sc.roll_no and sc.subject_no in(select distinct s.subject_no from subject s,exmtt e,exmtt_det ex where s.subject_name='" + ddlsubject.SelectedItem.Text + "' and e.exam_code=ex.exam_code and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and s.subject_no=ex.subject_no and ex.exam_date=convert(datetime,'" + ddldate.SelectedValue.ToString() + "',105)) and cc=0 and delflag=0 and exam_flag <> 'DEBAR'";
        SqlDataAdapter dastudcount = new SqlDataAdapter(studcount, con);
        DataSet dsstudcount = new DataSet();
        dastudcount.Fill(dsstudcount);
        con.Close();
        con.Open();
        if (dsstudcount.Tables[0].Rows.Count > 0)
        {
            lbltotstud.Visible = true;
            lblviewstud.Visible = true;
            lblviewstud.Text = Convert.ToString(dsstudcount.Tables[0].Rows.Count);
        }
        string studdummycount = "select count(roll_no) as totdummy from dummynumber where  exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_year=" + ddlYear.SelectedValue.ToString() + " and exam_date='" + ddldate.SelectedValue.ToString() + "' and subject_no in(select distinct s.subject_no from subject s,exmtt e,exmtt_det ex where s.subject_name='" + ddlsubject.SelectedItem.Text + "' and e.exam_code=ex.exam_code and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and s.subject_no=ex.subject_no and ex.exam_date=convert(datetime,'" + ddldate.SelectedValue.ToString() + "',105))";
        SqlDataAdapter dastuddummycount = new SqlDataAdapter(studdummycount, con);
        DataSet dsstuddummycount = new DataSet();
        dastuddummycount.Fill(dsstuddummycount);
        con.Close();
        con.Open();
        if (dsstuddummycount.Tables[0].Rows.Count > 0)
        {
            lblremainstud.Visible = true;
            lblremainstudvies.Visible = true;
            lblremainstudvies.Text = dsstuddummycount.Tables[0].Rows[0]["totdummy"].ToString();
        }
    }
    protected void txtreg_TextChanged(object sender, EventArgs e)
    {
        lblerrmag.Visible = false;
        if (ddlMonth.SelectedValue != "0" && ddlYear.SelectedValue != "0" && ddldate.SelectedValue != "0" && ddlsubject.SelectedValue != "0")
        {
            string regno = txtreg.Text;
            string checkreg = "select batch_year,degree_code,current_semester,r.roll_no,s.subject_code from registration r,subjectchooser sc,subject s where s.subtype_no=sc.subtype_no and s.subject_no=sc.subject_no and r.roll_no=sc.roll_no and sc.subject_no in(select distinct s.subject_no from subject s,exmtt e,exmtt_det ex where s.subject_name='" + ddlsubject.SelectedItem.Text + "' and e.exam_code=ex.exam_code and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and s.subject_no=ex.subject_no and ex.exam_date=convert(datetime,'" + ddldate.SelectedValue.ToString() + "',105)) and r.reg_no='" + regno + "'and cc=0 and delflag=0 and exam_flag <> 'DEBAR'";
            SqlDataAdapter dacheckreg = new SqlDataAdapter(checkreg, con);
            DataSet dscheckreg = new DataSet();
            dacheckreg.Fill(dscheckreg);
            con.Close();
            con.Open();
            string rollno = "";
            string subject_code = "";
            if (dscheckreg.Tables[0].Rows.Count > 0)
            {
                batchyear = dscheckreg.Tables[0].Rows[0]["batch_year"].ToString();
                degree_code = dscheckreg.Tables[0].Rows[0]["degree_code"].ToString();
                cur_sem = dscheckreg.Tables[0].Rows[0]["current_semester"].ToString();
                rollno = dscheckreg.Tables[0].Rows[0]["roll_no"].ToString();
                subject_code = dscheckreg.Tables[0].Rows[0]["subject_code"].ToString();
                string checkalrdyreg = "select * from dummynumber where batch=" + batchyear + " and degreecode=" + degree_code + " and semester=" + cur_sem + " and regno='" + regno + "' and dummy_type=1 and subject_no in(select distinct s.subject_no from subject s,exmtt e,exmtt_det ex where s.subject_name='" + ddlsubject.SelectedItem.Text + "' and e.exam_code=ex.exam_code and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and s.subject_no=ex.subject_no and ex.exam_date=convert(datetime,'" + ddldate.SelectedValue.ToString() + "',105)) and exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_year=" + ddlYear.SelectedValue.ToString() + " and exam_date='" + ddldate.SelectedValue.ToString() + "'";
                SqlDataAdapter dacheckalrdyreg = new SqlDataAdapter(checkalrdyreg, con1);
                DataSet dscheckalrdyreg = new DataSet();
                dacheckalrdyreg.Fill(dscheckalrdyreg);
                con1.Close();
                con1.Open();
                if (dscheckalrdyreg.Tables[0].Rows.Count > 0)
                {
                    txtreg.Text = "";
                    lblerrmag.Text = "Register Number Already Assigned";
                    lblerrmag.Visible = true;
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Register Number Already Assigned')", true);
                }

                Session["batchyear"] = batchyear;
                Session["degreecode"] = degree_code;
                Session["cursem"] = cur_sem;
                Session["roll_no"] = rollno;
                Session["subcode"] = subject_code;

            }
            else
            {
                txtreg.Text = "";
                lblerrmag.Text = "Register Number Not Found";
                lblerrmag.Visible = true;
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Register Number Not Found')", true);
            }
        }
        else
        {
            lblerrmag.Text = "Select the Above Options";
            lblerrmag.Visible = true;
        }
    }
    protected void txtdummy_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrmag.Visible = false;
            int sno = 0;

            int dummytype = 0;
            if (Radiorandom.Checked == true)
            {
                dummytype = 0;
            }
            else if (Radioserial.Checked == true)
            {

                dummytype = 1;
            }
            if (Session["batchyear"] != "" && Session["degreecode"] != "" && Session["batchyear"] != " " && Session["degreecode"] != " " && txtreg.Text != "")
            {
                string checkdummyalrdy = "select * from dummynumber where dummy_no=" + txtdummy.Text + "";
                SqlDataAdapter dacheckdummyalrdy = new SqlDataAdapter(checkdummyalrdy, con);
                DataSet dscheckdummyalrdy = new DataSet();
                con.Close();
                con.Open();
                dacheckdummyalrdy.Fill(dscheckdummyalrdy);
                if (dscheckdummyalrdy.Tables[0].Rows.Count > 0)
                {
                    txtdummy.Text = "";
                    lblerrmag.Text = "Dummy Number Already Registered";
                    lblerrmag.Visible = true;
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Dummy Number Already Registered')", true);
                }
                else
                {
                    string getdummy = "select * from dummynumbernew where batch=" + Session["batchyear"] + " and degreecode=" + Session["degreecode"] + " and subject_no in(select distinct s.subject_no from subject s,exmtt e,exmtt_det ex where s.subject_name='" + ddlsubject.SelectedItem.Text + "' and e.exam_code=ex.exam_code and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and s.subject_no=ex.subject_no and ex.exam_date=convert(datetime,'" + ddldate.SelectedValue.ToString() + "',105))" + " and semester=" + Session["cursem"] + " and dummy_no=" + txtdummy.Text + " and dummy_type=" + dummytype + " and exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_year=" + ddlYear.SelectedValue.ToString() + " and exam_date='" + ddldate.SelectedValue.ToString() + "'";
                    SqlDataAdapter dagetdummy = new SqlDataAdapter(getdummy, con);
                    DataSet dsgetdummy = new DataSet();
                    con.Close();
                    con.Open();
                    dagetdummy.Fill(dsgetdummy);
                    string exammonth = ddlMonth.SelectedValue.ToString();
                    string examyear = ddlYear.SelectedValue.ToString();
                    string examdate = ddldate.SelectedValue.ToString();
                    string rollno = Session["roll_no"].ToString();
                    string regno = txtreg.Text;
                    if (dsgetdummy.Tables[0].Rows.Count > 0)
                    {
                        string subjectcode = dsgetdummy.Tables[0].Rows[0]["subject"].ToString();
                        string subjectno = dsgetdummy.Tables[0].Rows[0]["subject_no"].ToString();
                        string dummyno = txtdummy.Text;
                        string selectdummy = "select * from dummynumber where batch=" + Session["batchyear"] + " and degreecode=" + Session["degreecode"] + "  and subject_no=" + subjectno + " and semester=" + Session["cursem"] + " and roll_no='" + rollno + "'and dummy_type=" + dummytype + " and exam_month=" + exammonth + " and exam_year=" + examyear + " and exam_date='" + examdate + "'";
                        SqlCommand selectdummycmd = new SqlCommand(selectdummy, con3);
                        con3.Close();
                        con3.Open();
                        SqlDataReader selectdummyreader;
                        selectdummyreader = selectdummycmd.ExecuteReader();
                        if (selectdummyreader.HasRows)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Dummy Number Already Assigned to this Reg No Do you want to Replace that')", true);
                            string updatedummy = "update dummynumber set batch=" + Session["batchyear"] + ",degreecode=" + Session["degreecode"] + ",subject='" + Session["subcode"] + "',subject_no=" + subjectno + ",semester=" + Session["cursem"] + ",roll_no='" + rollno + "',regno='" + regno + "',dummy_no=" + dummyno + ",dummy_type=" + dummytype + ", exam_month=" + exammonth + ", exam_year=" + examyear + ",exam_date='" + examdate + "' where batch=" + Session["batchyear"] + " and semester=" + Session["cursem"] + " and degreecode=" + Session["degreecode"] + " and roll_no='" + rollno + "'and subject='" + subjectcode + "' and subject_no=" + subjectno + " and dummy_type=" + dummytype + "and exam_month=" + exammonth + " and exam_year=" + examyear + " and exam_date='" + examdate + "'";
                            SqlCommand updatedummycmd = new SqlCommand(updatedummy, con3);
                            con3.Close();
                            con3.Open();
                            updatedummycmd.ExecuteNonQuery();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Dummy Number Assigned Successfully')", true);
                            txtdummy.Text = "";
                            txtreg.Text = "";
                        }
                        else
                        {

                            string insertdummy = "insert into dummynumber values(" + Session["batchyear"] + "," + Session["degreecode"] + ",'" + subjectcode + "'," + Session["cursem"] + ",'" + rollno + "','" + regno + "'," + dummyno + "," + dummytype + "," + subjectno + "," + exammonth + "," + examyear + ",'" + examdate + "')";
                            SqlCommand createdummycmd = new SqlCommand(insertdummy, con3);
                            con3.Close();
                            con3.Open();
                            createdummycmd.ExecuteNonQuery();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Dummy Number Assigned Successfully')", true);
                            txtdummy.Text = "";
                            txtreg.Text = "";
                        }
                        string selectreg = "select r.roll_no,isnull(r.reg_no,' ') as reg_no from registration r,subjectchooser sc,subject s where s.subtype_no=sc.subtype_no and s.subject_no=sc.subject_no and r.roll_no=sc.roll_no and sc.subject_no in(select distinct s.subject_no from subject s,exmtt e,exmtt_det ex where s.subject_name='" + ddlsubject.SelectedItem.Text + "' and e.exam_code=ex.exam_code and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and s.subject_no=ex.subject_no and ex.exam_date=convert(datetime,'" + ddldate.SelectedValue.ToString() + "',105)) and cc=0 and delflag=0 and exam_flag <> 'DEBAR'";
                        SqlDataAdapter daselectreg = new SqlDataAdapter(selectreg, con4);
                        DataSet dsselectreg = new DataSet();
                        daselectreg.Fill(dsselectreg);
                        con4.Close();
                        con4.Open();
                        if (dsselectreg.Tables[0].Rows.Count > 0)
                        {
                            sprdViewdummy.Visible = true;
                            sprdViewdummy.Sheets[0].RowCount = 0;
                            for (int totreg = 0; totreg < dsselectreg.Tables[0].Rows.Count; totreg++)
                            {
                                sno++;
                                string dummynum = "";
                                string status = "";
                                string regnum = dsselectreg.Tables[0].Rows[totreg]["reg_no"].ToString();
                                if (regnum != "" && regnum != " ")
                                {
                                    string binddummysprd = "select regno,dummy_no from dummynumber where exam_month=" + exammonth + " and exam_year=" + examyear + " and exam_date='" + examdate + "' and subject_no=" + subjectno + " and regno='" + regnum + "'";
                                    SqlDataAdapter dabinddummysprd = new SqlDataAdapter(binddummysprd, con5);
                                    DataSet dsbinddummysprd = new DataSet();
                                    con5.Close();
                                    con5.Open();
                                    dabinddummysprd.Fill(dsbinddummysprd);
                                    if (dsbinddummysprd.Tables[0].Rows.Count > 0)
                                    {
                                        status = "Generated";
                                        dummynum = dsbinddummysprd.Tables[0].Rows[0]["dummy_no"].ToString();
                                    }
                                    else
                                    {
                                        status = "Not";
                                    }
                                }
                                else
                                {
                                    status = "Not";
                                }
                                sprdViewdummy.Sheets[0].RowCount = sprdViewdummy.Sheets[0].RowCount + 1;
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 1].Text = regnum;
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 2].Text = "*" + dummynum + "*";
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 2].Font.Name = "IDAutomationHC39M";
                                if (status == "Not")
                                {
                                    sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 2].Text = "";
                                }
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 3].Text = status;
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                sprdViewdummy.Sheets[0].Cells[sprdViewdummy.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            }
                            int rowcount = sprdViewdummy.Sheets[0].RowCount;
                            sprdViewdummy.Height = (rowcount * 50) + 20;
                            sprdViewdummy.Sheets[0].PageSize = (rowcount * 50) + 20;
                        }

                    }
                    else
                    {
                        txtdummy.Text = "";
                        lblerrmag.Text = "Dummy Number Not Found";
                        lblerrmag.Visible = true;
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Dummy Number Not Found')", true);
                    }

                }

                //======label text
                string studcount = "select batch_year,degree_code,current_semester,r.roll_no,s.subject_code from registration r,subjectchooser sc,subject s where s.subtype_no=sc.subtype_no and s.subject_no=sc.subject_no and r.roll_no=sc.roll_no and sc.subject_no in(select distinct s.subject_no from subject s,exmtt e,exmtt_det ex where s.subject_name='" + ddlsubject.SelectedItem.Text + "' and e.exam_code=ex.exam_code and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and s.subject_no=ex.subject_no and ex.exam_date=convert(datetime,'" + ddldate.SelectedValue.ToString() + "',105)) and cc=0 and delflag=0 and exam_flag <> 'DEBAR'";
                SqlDataAdapter dastudcount = new SqlDataAdapter(studcount, con);
                DataSet dsstudcount = new DataSet();
                dastudcount.Fill(dsstudcount);
                con.Close();
                con.Open();
                if (dsstudcount.Tables[0].Rows.Count > 0)
                {
                    lbltotstud.Visible = true;
                    lblviewstud.Visible = true;
                    lblviewstud.Text = Convert.ToString(dsstudcount.Tables[0].Rows.Count);
                }
                string studdummycount = "select count(roll_no) as totdummy from dummynumber where  exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_year=" + ddlYear.SelectedValue.ToString() + " and exam_date='" + ddldate.SelectedValue.ToString() + "' and subject_no in(select distinct s.subject_no from subject s,exmtt e,exmtt_det ex where s.subject_name='" + ddlsubject.SelectedItem.Text + "' and e.exam_code=ex.exam_code and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and s.subject_no=ex.subject_no and ex.exam_date=convert(datetime,'" + ddldate.SelectedValue.ToString() + "',105))";
                SqlDataAdapter dastuddummycount = new SqlDataAdapter(studdummycount, con);
                DataSet dsstuddummycount = new DataSet();
                dastuddummycount.Fill(dsstuddummycount);
                con.Close();
                con.Open();
                if (dsstuddummycount.Tables[0].Rows.Count > 0)
                {
                    lblremainstud.Visible = true;
                    lblremainstudvies.Visible = true;
                    lblremainstudvies.Text = dsstuddummycount.Tables[0].Rows[0]["totdummy"].ToString();
                }

            }
        }
        catch (Exception e1)
        {
            lblerrmag.Text = e1.Message;
        }
    }

}