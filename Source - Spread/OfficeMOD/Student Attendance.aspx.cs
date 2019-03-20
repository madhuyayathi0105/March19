using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;

public partial class Student_Attendance : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        //string PageLogOut = "";
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("OffM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
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

        if (!Request.FilePath.Contains("OfficeHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            bindCompanyname();
            bindbatch();
            bindedu();
            gview.Visible = false;
            btnsave.Visible = false;
        }
    }
    public void bindCompanyname()
    {
        try
        {
            gview.Visible = false;
            btnsave.Visible = false;
            ds.Clear();
            drpcompany.Items.Clear();
            string itemname = "select distinct CompanyPK, CompName from CompanyMaster  order by CompanyPK";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                drpcompany.DataSource = ds;
                drpcompany.DataTextField = "CompName";
                drpcompany.DataValueField = "CompanyPK";
                drpcompany.DataBind();


            }
            bindedu();
            bindposition();
        }
        catch
        {
        }
    }
    public void binddate()
    {
        try
        {
            gview.Visible = false;
            btnsave.Visible = false;
            ddldate.Items.Clear();
                        string position = string.Empty;
            string posi = string.Empty;
            if (cbldes.Items.Count > 0)
            {

                for (int i = 0; i < cbldes.Items.Count; i++)
                {
                    if (cbldes.Items[i].Selected == true)
                    {
                        posi = cbldes.Items[i].Value.ToString();
                        if (position == "")
                        {
                            position = posi;
                        }
                        else
                        {
                            position = position + "'" + "," + "'" + posi;
                        }
                    }
                }
            }
            if (position != "")
            {
                string datebind = "select convert(varchar, interviewdate, 103) as interviewdate  from Company_datails where CompanyFK='" + Convert.ToString(drpcompany.SelectedValue) + "' and  composition in(select  MasterCode from CO_MasterValues where MasterCriteria ='Company Position' ) and composition in('" + position + "')";
                DataSet dsdate = new DataSet();
                dsdate = d2.select_method_wo_parameter(datebind, "text");
                if (dsdate.Tables[0].Rows.Count > 0)
                {
                    ddldate.DataSource = dsdate;
                    ddldate.DataTextField = "interviewdate";
                    ddldate.DataValueField = "interviewdate";
                    ddldate.DataBind();
                }
            }
        }
        catch
        {
        }

    }
    public void drpcompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindedu();
        bindposition();
        gview.Visible = false;
        btnsave.Visible = false;
    }
    public void bindbatch()
    {
        try
        {
            gview.Visible = false;
            btnsave.Visible = false;
            cbl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                //ddl_batch1.SelectedIndex = 3;
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[0].Selected = true;
                    }
                    // txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    txt_batch.Text = "Batch(" + 1 + ")";
                    //cb_batch.Checked = true;
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
    //public void interviewround()
    //{
    //    try
    //    {
    //        btnsave.Visible = false;
    //        gview.Visible = false;
    //        ds.Clear();
    //        Cblround.Items.Clear();
    //        string itemname = d2.GetFunction("select max (LinkValue) from New_InsSettings where LinkName ='Interview Rounds'");
    //        //ds.Clear();
    //        //ds = d2.select_method_wo_parameter(itemname, "Text");
    //        if (itemname != "" & itemname != "0") //if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < Convert.ToInt64(itemname); i++)
    //            {
    //                Cblround.Items.Insert(i, Convert.ToString(i + 1));
    //            }
    //            //Cblround.DataSource = ds;
    //            //Cblround.DataTextField = "LinkValue";
    //            //Cblround.DataValueField = "LinkValue";
    //            //Cblround.DataBind();
    //            if (Cblround.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_batch.Items.Count; i++)
    //                {
    //                    Cblround.Items[0].Selected = true;
    //                }
    //                // txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
    //                txtround.Text = "No of Round(" + 1 + ")";
    //                //cb_batch.Checked = true;
    //            }
    //            else
    //            {
    //                txtround.Text = "--Select--";
    //                Cbround.Checked = false;
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    public void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            btnsave.Visible = false;
            gview.Visible = false;
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

        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            btnsave.Visible = false;
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

        }
        catch (Exception ex)
        {
        }
    }

    //public void cb_round_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        btnsave.Visible = false;
    //        gview.Visible = false;
    //        string buildvalue1 = string.Empty;
    //        string build1 = string.Empty;
    //        if (Cbround.Checked == true)
    //        {
    //            for (int i = 0; i < Cblround.Items.Count; i++)
    //            {
    //                if (Cbround.Checked == true)
    //                {
    //                    Cblround.Items[i].Selected = true;
    //                    txtround.Text = "Batch(" + (Cblround.Items.Count) + ")";
    //                    build1 = Cblround.Items[i].Value.ToString();
    //                    if (buildvalue1 == "")
    //                    {
    //                        buildvalue1 = build1;
    //                    }
    //                    else
    //                    {
    //                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < Cblround.Items.Count; i++)
    //            {
    //                Cblround.Items[i].Selected = false;
    //                txtround.Text = "--Select--";
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //public void cbl_round_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        btnsave.Visible = false;
    //        gview.Visible = false;
    //        int seatcount = 0;
    //        Cbround.Checked = false;
    //        string buildvalue = string.Empty;
    //        string build = string.Empty;
    //        for (int i = 0; i < Cblround.Items.Count; i++)
    //        {
    //            if (Cblround.Items[i].Selected == true)
    //            {
    //                seatcount = seatcount + 1;
    //                txtround.Text = "--Select--";
    //                build = Cblround.Items[i].Value.ToString();
    //                if (buildvalue == "")
    //                {
    //                    buildvalue = build;
    //                }
    //                else
    //                {
    //                    buildvalue = buildvalue + "'" + "," + "'" + build;
    //                }
    //            }
    //        }
    //        if (seatcount == Cblround.Items.Count)
    //        {
    //            txtround.Text = "Batch(" + seatcount.ToString() + ")";
    //            Cbround.Checked = true;
    //        }
    //        else if (seatcount == 0)
    //        {
    //            txtround.Text = "--Select--";
    //            Cbround.Text = "--Select--";
    //        }
    //        else
    //        {
    //            txtround.Text = "Batch(" + seatcount.ToString() + ")";
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            #region datatable
            DataRow drrow = null;
            DataTable dtTTDisp = new DataTable();

            dtTTDisp.Columns.Add("App_no");
            dtTTDisp.Columns.Add("SNo.");
            dtTTDisp.Columns.Add("Roll No");
            dtTTDisp.Columns.Add("Reg No");
            dtTTDisp.Columns.Add("Student Name");
            dtTTDisp.Columns.Add("Batch");
            dtTTDisp.Columns.Add("Section");
            dtTTDisp.Columns.Add("Semester");
            dtTTDisp.Columns.Add("Applied Post");
            int y = dtTTDisp.Columns.Count;
            drrow = dtTTDisp.NewRow();
            drrow["App_no"] = "App_no";
            drrow["SNo."] = "SNo.";
            drrow["Student Name"] = "Student Name";
            drrow["Roll No"] = "Roll No";
            drrow["Reg No"] = "Reg No";
            drrow["Batch"] = "Batch";
            drrow["Semester"] = "Semester";
            drrow["Section"] = "Section";
            drrow["Applied Post"] = "Applied Post";
            dtTTDisp.Rows.Add(drrow);
            drrow = dtTTDisp.NewRow();
            drrow["App_no"] = "App_no";
            drrow["SNo."] = "SNo.";
            drrow["Student Name"] = "Student Name";
            drrow["Roll No"] = "Roll No";
            drrow["Reg No"] = "Reg No";
            drrow["Batch"] = "Batch";
            drrow["Semester"] = "Semester";
            drrow["Section"] = "Section";
            drrow["Applied Post"] = "Applied Post";
            dtTTDisp.Rows.Add(drrow);
            dtTTDisp.Columns.Add("Present", System.Type.GetType("System.Boolean"));
            #endregion
            string Batch_tagvalue = string.Empty;
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    string addbatch1 = cbl_batch.Items[i].Value.ToString();
                    if (Batch_tagvalue == "")
                    {
                        Batch_tagvalue = addbatch1;
                    }
                    else
                    {
                        Batch_tagvalue = Batch_tagvalue + "'" + "," + "'" + addbatch1;
                    }
                }
            }
            string branch = string.Empty;
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                if (cbldepartment.Items[i].Selected == true)
                {
                    string branch1 = cbldepartment.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = branch1;
                    }
                    else
                    {
                        branch = branch + "'" + "," + "'" + branch1;
                    }
                }
            }
            string position = string.Empty;
            string posi = string.Empty;
            if (cbldes.Items.Count > 0)
            {

                for (int i = 0; i < cbldes.Items.Count; i++)
                {
                    if (cbldes.Items[i].Selected == true)
                    {
                        posi = cbldes.Items[i].Value.ToString();
                        if (position == "")
                        {
                            position = posi;
                        }
                        else
                        {
                            position = position + "'" + "," + "'" + posi;
                        }
                    }
                }
            }
            string dates = string.Empty;
            dates = Convert.ToString(ddldate.SelectedValue);
            string[] spl = dates.Split('/');
            int getdate = 0;
            int.TryParse(spl[0], out getdate);
            if (getdate < 10)
            {
                String startOfString = spl[0].Remove(0, 1);
                spl[0] = startOfString;
            }
            if (Batch_tagvalue != "" && Convert.ToString(drpcompany.SelectedValue) != "" && branch != "" && Convert.ToString(ddldate.SelectedValue) != "" && position!="")
            {
                string qury = "select r.Stud_Name,r.batch_year,r.Roll_No,r.Reg_No,r.App_No,Dept_Name,r.degree_code,c.course_name,r.Current_Semester,r.Sections, co.MasterValue as appposted from  Company_StudentRegistration cr, Registration r,degree d,Department dt,Course C,  CompanyMaster cm, Company_datails cd,CO_MasterValues co where r.App_No=cr.APP_No and cr.CompanyFK=cm.CompanyPK and cm.CompanyPK=cd.CompanyFK and cm.CompanyPK='" + Convert.ToString(drpcompany.SelectedValue) + "' and r.Batch_Year in('" + Batch_tagvalue + "') and cd.composition=cr.composition and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and cd.Companydetailspk=cr.CompanydetailsFk and convert(varchar,cd.interviewdate,103)='" + Convert.ToString(ddldate.SelectedValue) + "' and cd.interviewdate=cr.interviewdate and r.degree_code in('" + branch + "') and MasterCode=cd.composition and MasterCriteria ='Company Position' and MasterCode in('" + position + "') and cr.shortlist_flag='1' order by r.Roll_No,r.batch_year,r.degree_code";

                qury = qury + " select * from Cm_Attendance where  CompanyFK='" + Convert.ToString(drpcompany.SelectedValue) + "' and AttnMonth='" + Convert.ToString(spl[1]) + "' and AttnYear='" + Convert.ToString(spl[2]) + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(qury, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record";
                }
                else
                {
                    int cun = 0;
                    drrow = dtTTDisp.NewRow();
                    drrow["App_no"] = "";
                    drrow["SNo."] = "";
                    drrow["Student Name"] = "";
                    drrow["Roll No"] = "";
                    drrow["Reg No"] = "";
                    drrow["Batch"] = "";
                    drrow["Semester"] = "";
                    drrow["Section"] = "";
                    drrow["Applied Post"] = "";
          
                    //drrow["App_no"] = "App_no";
                    //drrow["SNo."] = "SNo.";
                    //drrow["Student Name"] = "Student Name";
                    //drrow["Roll No"] = "Roll No";
                    //drrow["Reg No"] = "Reg No";
                    //drrow["Batch"] = "Batch";
                    //drrow["Semester"] = "Semester";
                    //drrow["Section"] = "Section";
                    //drrow["Applied Post"] = "Applied Post";
                  dtTTDisp.Rows.Add(drrow);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cun++;
                        drrow = dtTTDisp.NewRow();
                        if (i == 0)
                        {
                            drrow["SNo."] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            
                            drrow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            drrow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            drrow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            drrow["Batch"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            drrow["Semester"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            drrow["Section"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            drrow["Applied Post"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            dtTTDisp.Rows.Add(drrow);
                        }
                        else
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i - 1]["Dept_Name"]) != Convert.ToString(ds.Tables[0].Rows[i - 1]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i - 1]["Dept_Name"]))
                            {
                                drrow = dtTTDisp.NewRow();
                                drrow["SNo."] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                               
                                drrow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drrow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drrow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drrow["Batch"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drrow["Semester"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drrow["Section"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                drrow["Applied Post"] = Convert.ToString(ds.Tables[0].Rows[i]["course_name"]) + '-' + Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                                dtTTDisp.Rows.Add(drrow);
                            }
                        }
                        drrow = dtTTDisp.NewRow();
                        drrow["App_no"] = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                        drrow["SNo."] = cun;
                        drrow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                        drrow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                        drrow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                        drrow["Batch"] = Convert.ToString(ds.Tables[0].Rows[i]["Batch_year"]);
                        drrow["Semester"] = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                        drrow["Section"] = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                        if (Convert.ToString(ds.Tables[0].Rows[i]["Sections"])=="")
                        drrow["Section"] = "-";
                        drrow["Applied Post"] = Convert.ToString(ds.Tables[0].Rows[i]["appposted"]);
                        ds.Tables[1].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "'";
                        DataView dvStudentAttendance = ds.Tables[1].DefaultView;
                        dtTTDisp.Rows.Add(drrow);
                        if (dvStudentAttendance.Count > 0)
                        {

                            dtTTDisp.Rows[dtTTDisp.Rows.Count - 1]["present"] = true;
                            string colname = "D" + Convert.ToString(spl[0]);
                            if (Convert.ToString(dvStudentAttendance[0][colname]) == "1")
                            {

                                dtTTDisp.Rows[dtTTDisp.Rows.Count - 1]["present"] = true;
                            }
                            else if (Convert.ToString(dvStudentAttendance[0][colname]) == "2")
                            {
                                dtTTDisp.Rows[dtTTDisp.Rows.Count - 1]["present"] = false;
                            }




                        }


                    }
                    if (dtTTDisp.Rows.Count > 1)
                    {



                        gview.DataSource = dtTTDisp;
                        gview.DataBind();
                        gview.Visible = true;

                        btnsave.Visible = true;
                        for (int i = 2; i < gview.Rows.Count; i++)
                        {
                            int row = 0;
                            for (int c = 0; c < gview.Rows[0].Cells.Count; c++)
                            {

                                string val = string.Empty;
                                if (gview.Rows[0].Cells[c].Text == "")
                                {
                                    if (row == 0)
                                        row = c;
                                    int a = c - row;
                                    if (a < 10)
                                    {


                                        val = "0" + a + "";
                                    }
                                    else
                                    {
                                        val = Convert.ToString(c);
                                    }
                                    string chkname = "ctl" + val + "";
                                    CheckBox stud_rollno = (gview.Rows[i].Cells[c].FindControl(chkname)) as CheckBox;

                                    stud_rollno.Enabled = true;
                                }
                            }
                        }
                        int cblcun = 0;
                        if (gview.Rows.Count > 0)
                        {


                            int a = 0;

                            cblcun++;
                            for (int i = 0; i < gview.Rows[1].Cells.Count; i++)
                            {
                                string colname = gview.Rows[0].Cells[i].Text;
                                if (gview.Rows[0].Cells[i].Text == "")
                                {
                                    gview.Rows[0].Cells[i].Text = "Attendance";
                                    gview.Rows[0].Cells[i].Width = 50;
                                }
                                if (gview.Rows[1].Cells[i].Text == "")
                                {
                                    if (a != 1)
                                    {
                                        a = 1;

                                        gview.Rows[1].Cells[i].Text = "Present";

                                        i = gview.Rows[1].Cells.Count;
                                    }

                                }
                            }
                        }





                        #region span
                        for (int i = gview.Rows.Count - 1; i >= 1; i--)
                        {
                            GridViewRow row = gview.Rows[i];
                            GridViewRow previousRow = gview.Rows[i - 1];
                            for (int j = 0; j < row.Cells.Count - 1; j++)
                            {


                                string date = row.Cells[j].Text;
                                string predate = previousRow.Cells[j].Text;


                                if (date == predate)
                                {
                                    if (previousRow.Cells[j].RowSpan == 0)
                                    {
                                        if (row.Cells[j].RowSpan == 0)
                                        {
                                            previousRow.Cells[j].RowSpan += 2;
                                        }
                                        else
                                        {
                                            previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                        }
                                        row.Cells[j].Visible = false;
                                    }
                                }


                            }

                            row.Cells[0].Visible = false;
                            gview.Rows[0].Cells[0].Visible = false;

                        }
                        RowHead(gview);


                       


                        #endregion span

                        for (int m = gview.Rows.Count - 1; m >= 2; m--)
                        {

                            GridViewRow rows = gview.Rows[m];
                            GridViewRow previousRows = gview.Rows[m];
                            GridViewRow previousRowss = gview.Rows[m];
                            gview.Rows[m].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                            gview.Rows[m].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                            gview.Rows[m].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                            gview.Rows[m].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            gview.Rows[m].Cells[9].HorizontalAlign = HorizontalAlign.Center;
                            string cellte = gview.Rows[m].Cells[1].Text;
                            if (!Convert.ToString(cellte).All(char.IsNumber))
                            {
                               
                                if (m != 2)
                                {
                                    gview.Rows[m].Cells[1].ColumnSpan = gview.Rows[m].Cells.Count;

                                    gview.Rows[m].Cells[1].ColumnSpan = gview.Rows[m].Cells.Count;
                                    for (int j = 2; j < gview.Rows[m].Cells.Count; j++)
                                    {
                                        gview.Rows[m].Cells[0].Visible = false;
                                        gview.Rows[m].Cells[j].Visible = false;
                                    }
                                }
                                if (m == 2)
                                {
                                    gview.Rows[m].Cells[8].ColumnSpan = gview.Rows[m].Cells.Count-2;

                                    gview.Rows[m].Cells[8].ColumnSpan = gview.Rows[m].Cells.Count-2;
                                    for (int j = 2; j < gview.Rows[m].Cells.Count; j++)
                                    {
                                        gview.Rows[m].Cells[0].Visible = false;
                                        gview.Rows[m].Cells[j].Visible = false;
                                    }
                                    gview.Rows[m].Cells[1].Visible = false;
                                    gview.Rows[m].Cells[9].Visible = true;
                                    gview.Rows[m].Cells[8].Visible = true;
                                    gview.Rows[m].Cells[9].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }






                        }
                    }
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select All Feild";
            }

        }
        catch
        {
        }
    }
    protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 2; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }
    public void btnsave_Click(object sender, EventArgs e)
    {
        int vcm = 0;
        string colmname = string.Empty;
        string compk = string.Empty;
        string applpost = string.Empty;
        string app_no = string.Empty;
        string date = string.Empty;
        date = Convert.ToString(ddldate.SelectedValue);
            
         
        
        string[] spl = date.Split('/');
        date = Convert.ToString(Convert.ToString(spl[1]).Trim() + "/" + Convert.ToString(spl[0]).Trim() + "/" + Convert.ToString(spl[2]).Trim());
        int getdate = 0;
    int.TryParse(spl[0],out getdate);
    if (getdate < 10)
    {
        String startOfString = spl[0].Remove(0, 1);
        spl[0] = startOfString;
    }
       // date = Convert.ToDateTime(spl[0]).ToString("d");
        if (gview.Rows.Count > 0)
        {
            for (int i = 3; i < gview.Rows.Count; i++)
            {
                string insertcol = string.Empty;
                string insertcolval = string.Empty;
                string insertcolupdate = string.Empty;
                string insertcolvalupdate = string.Empty;
                int row = 0;

                for (int m = 9; m < gview.HeaderRow.Cells.Count; m++)
                {


                    string abcol = gview.Rows[1].Cells[m].Text;
                    app_no = gview.Rows[i].Cells[0].Text;
                    applpost = gview.Rows[i].Cells[9].Text;
                    colmname = "D" + Convert.ToString(spl[0]);
                    compk = Convert.ToString(drpcompany.SelectedValue);
                    if (app_no != "&nbsp;")
                    {
                        string val = string.Empty;
                        if (row == 0)
                            row = m;
                        int a = m - row;
                        if (a < 10)
                        {


                            val = "0" + a + "";
                        }
                        else
                        {
                            val = Convert.ToString(m);
                        }
                        if (m == 9)
                        {
                            string chkname = "ctl" + val + "";
                            CheckBox stud_rollno = (gview.Rows[i].Cells[m].FindControl(chkname)) as CheckBox;

                            if (stud_rollno.Checked)
                            {
                                if (insertcol == "")
                                {
                                    insertcolupdate = colmname + '=' + "1";
                                    insertcol = colmname;
                                    insertcolval = "1";
                                }

                                else
                                {
                                    insertcolupdate = insertcolupdate + ',' + colmname + '=' + "1";
                                    insertcol = insertcol + ',' + colmname;
                                    insertcolval = insertcolval + ',' + "1";
                                }
                            }

                            if (!stud_rollno.Checked)
                            {
                                if (insertcol == "")
                                {
                                    insertcolupdate = colmname + '=' + "2";
                                    insertcol = colmname;
                                    insertcolval = "2";
                                }

                                else
                                {
                                    insertcolupdate = insertcolupdate + ',' + colmname + '=' + "2";
                                    insertcol = insertcol + ',' + colmname;
                                    insertcolval = insertcolval + ',' + "2";
                                }
                            }
                        }
                        
                    }

                    string stu_selecton = string.Empty;
                    if (insertcol != "")
                    {
                        stu_selecton = "if exists(select * from Cm_Attendance where app_no='" + app_no + "' and CompanyFK='" + compk + "' and AttnMonth='" + Convert.ToString(spl[1]) + "' and AttnYear='" + Convert.ToString(spl[2]) + "') update Cm_Attendance set " + insertcolupdate + " where app_no='" + app_no + "' and CompanyFK='" + compk + "' and AttnMonth='" + Convert.ToString(spl[1]) + "' and AttnYear='" + Convert.ToString(spl[2]) + "' else  insert into Cm_Attendance (APP_No,AttnYear,AttnMonth,CompanyFK," + insertcol + ") values('" + app_no + "','" + Convert.ToString(spl[2]) + "','" + Convert.ToString(spl[1]) + "','" + compk + "'," + insertcolval + ")";
                        vcm = d2.update_method_wo_parameter(stu_selecton, "TEXT");
                    }

                }
            }
            if (vcm != 0)
            {
                Page.MaintainScrollPositionOnPostBack = true;
                imgdiv3.Visible = true;
                Label1.Text = "Saved Successfully";
               
            }

        }
    }

    public void binddegree()
    {
        try
        {
            gview.Visible = false;
            btnsave.Visible = false;
            cb_degree.Checked = false;
            string typ = "";
            if (cblcourse.Items.Count > 0)
            {
                for (int i = 0; i < cblcourse.Items.Count; i++)
                {
                    if (cblcourse.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cblcourse.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cblcourse.Items[i].Value + "";
                        }
                    }

                }
            }
            if (typ != "")
            {
                string deptquery = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + collegecode1 + "' and Edu_Level in('" + typ + "') ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldegree.DataSource = ds;
                    cbldegree.DataTextField = "Course_Name";
                    cbldegree.DataValueField = "Course_Id";
                    cbldegree.DataBind();
                }
                if (cbldegree.Items.Count > 0)
                {
                    string deu = "select distinct degree from IM_CompanyDept where CompanyFK='" + Convert.ToString(drpcompany.SelectedValue) + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deu, "Text");
                    int cun = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                        {
                            for (int i = 0; i < cbldegree.Items.Count; i++)
                            {

                                if (Convert.ToString(ds.Tables[0].Rows[m]["degree"]) == cbldegree.Items[i].Value)
                                {
                                    cun++;
                                    cbldegree.Items[i].Enabled = true;
                                    cbldegree.Items[i].Selected = true;
                                }
                                else
                                {
                                    if (cbldegree.Items[i].Selected != true)
                                        cbldegree.Items[i].Enabled = false;
                                }
                            }
                        }
                        txtdegree.Text = "Degree(" + cun + ")";
                    }
                    else
                    {
                        for (int i = 0; i < cbldegree.Items.Count; i++)
                        {
                            cbldegree.Items[i].Enabled = false;
                        }
                        txtdegree.Text = "--Select--";
                    }

                }
            }
            binddepartment();
        }
        catch
        {
        }
    }

    public void binddepartment()
    {
        try
        {
            gview.Visible = false;
            btnsave.Visible = false;
            cb_departemt.Checked = false;
            string typ = "";
            if (cbldegree.Items.Count > 0)
            {
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {
                    if (cbldegree.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbldegree.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cbldegree.Items[i].Value + "";
                        }
                    }

                }
            }
            if (typ != "")
            {
                string deptquery = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + typ + "') and  degree.college_code='" + collegecode1 + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldepartment.DataSource = ds;
                    cbldepartment.DataTextField = "dept_name";
                    cbldepartment.DataValueField = "degree_code";
                    cbldepartment.DataBind();
                }
                if (cbldepartment.Items.Count > 0)
                {
                    string deu = "select distinct deptcode from IM_CompanyDept where CompanyFK='" + Convert.ToString(drpcompany.SelectedValue) + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(deu, "Text");
                    int cun = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                        {
                            for (int i = 0; i < cbldepartment.Items.Count; i++)
                            {

                                if (Convert.ToString(ds.Tables[0].Rows[m]["deptcode"]) == cbldepartment.Items[i].Value)
                                {
                                    cun++;
                                    cbldepartment.Items[i].Enabled = true;
                                    cbldepartment.Items[i].Selected = true;
                                }
                                else
                                {
                                    if (cbldepartment.Items[i].Selected != true)
                                        cbldepartment.Items[i].Enabled = false;
                                }
                            }
                        }
                        txtdept.Text = "Branch(" + cun + ")";
                    }
                    else
                    {
                        for (int i = 0; i < cbldepartment.Items.Count; i++)
                        {
                            cbldepartment.Items[i].Enabled = false;
                        }
                        txtdept.Text = "--Select--";
                    }

                }

            }
        }
        catch
        {
        }
    }
    public void bindedu()
    {
        try
        {
            btnsave.Visible = false;
            gview.Visible = false;
            string deptquery = " select distinct course.Edu_Level from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblcourse.DataSource = ds;
                cblcourse.DataTextField = "Edu_Level";
                cblcourse.DataValueField = "Edu_Level";
                cblcourse.DataBind();
            }
            if (cblcourse.Items.Count > 0)
            {
                string deu = "select distinct course from IM_CompanyDept where CompanyFK='" + Convert.ToString(drpcompany.SelectedValue) + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deu, "Text");
                int cun = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                    {
                        for (int i = 0; i < cblcourse.Items.Count; i++)
                        {

                            if (Convert.ToString(ds.Tables[0].Rows[m]["course"]) == cblcourse.Items[i].Value)
                            {
                                cun++;
                                cblcourse.Items[i].Enabled = true;
                                cblcourse.Items[i].Selected = true;
                            }
                            else
                            {
                                if (cblcourse.Items[i].Selected != true)
                                {
                                    cblcourse.Items[i].Enabled = false;
                                }
                            }
                        }
                    }
                    txtcourse.Text = "course(" + cun + ")";
                }
                else
                {
                    for (int i = 0; i < cblcourse.Items.Count; i++)
                    {
                        cblcourse.Items[i].Enabled = false;
                    }
                    txtcourse.Text = "--Select--";
                }

            }

            binddegree();
        }
        catch
        {
        }

    }
    protected void cb_degree_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            btnsave.Visible = false;
            gview.Visible = false;
            if (cbldegree.Items.Count > 0)
            {
                int cun = 0;
                if (cb_degree.Checked == true)
                {
                    for (int i = 0; i < cbldegree.Items.Count; i++)
                    {
                        if (cbldegree.Items[i].Enabled == true)
                        {
                            cbldegree.Items[i].Selected = true;
                            cun++;
                        }
                    
                    }
                }
                else
                {
                    for (int i = 0; i < cbldegree.Items.Count; i++)
                    {
                        if (cbldegree.Items[i].Enabled == true)
                            cbldegree.Items[i].Selected = false;
                    }
                }
                txtdegree.Text = "Degree(" + cun + ")";
            }

            binddepartment();

        }
        catch
        {
        }
    }
    protected void cb_course_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            btnsave.Visible = false;
            gview.Visible = false;
            if (cblcourse.Items.Count > 0)
            {
                int cun = 0;
                if (cb_course.Checked == true)
                {
                    for (int i = 0; i < cblcourse.Items.Count; i++)
                    {
                        if (cblcourse.Items[i].Enabled == true)
                        {
                            cblcourse.Items[i].Selected = true;
                            cun++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cblcourse.Items.Count; i++)
                    {
                        if (cblcourse.Items[i].Enabled == true)
                            cblcourse.Items[i].Selected = false;
                    }
                }
                txtcourse.Text = "course(" + cun + ")";
            }

            binddegree();

        }
        catch
        {
        }
    }
    protected void cbdepartment_Change(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            btnsave.Visible = false;
            if (cbldepartment.Items.Count > 0)
            {
                int cun = 0;
                if (cb_departemt.Checked == true)
                {
                    for (int i = 0; i < cbldepartment.Items.Count; i++)
                    {
                        if (cbldepartment.Items[i].Enabled == true)
                        {
                            cbldepartment.Items[i].Selected = true;
                            cun++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cbldepartment.Items.Count; i++)
                    {
                        if (cbldepartment.Items[i].Enabled == true)
                            cbldepartment.Items[i].Selected = false;
                    }
                }
                txtdept.Text = "Branch(" + cun + ")";
            }
        }
        catch
        {
        }
    }
    protected void cblcourse_ChekedChange(object sender, EventArgs e)
    {
        if (cblcourse.Items.Count > 0)
        {
            int cun = 0;
                for (int i = 0; i < cblcourse.Items.Count; i++)
                {
                    if (cblcourse.Items[i].Selected == true)
                    {
                        cun++;
                    }
                }
            
            txtcourse.Text = "course(" + cun + ")";
        }
        binddegree();
        btnsave.Visible = false;
        gview.Visible = false;
    }
    protected void cbldegree_ChekedChange(object sender, EventArgs e)
    {

        if (cbldegree.Items.Count > 0)
        {
            int cun = 0;
            for (int i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    cun++;
                }
            }
            txtdegree.Text = "Degree(" + cun + ")";
        }
        binddepartment();
        gview.Visible = false;
        btnsave.Visible = false;
    }
    protected void cbldepartment_ChekedChange(object sender, EventArgs e)
    {
        if (cbldepartment.Items.Count > 0)
        {
            int cun = 0;
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                if (cbldepartment.Items[i].Selected == true)
                {
                    cun++;
                }
            }
            txtdept.Text = "Branch(" + cun + ")";
        }

        gview.Visible = false;
        btnsave.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        gview.Visible = false;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        btn_go_Click(sender, e);
    }
    protected void gview_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
          
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string activerow = Convert.ToString(rowIndex);
            string activecol = Convert.ToString(selectedCellIndex); int rows = 0;
            int.TryParse(activerow,out rows);
                int col = 0;
                int.TryParse(activecol, out col);
                if (gview.Rows.Count > 0)
                {


                    int a = 0;

                  
                    for (int i = 9; i < gview.Rows[1].Cells.Count; i++)
                    {
                        string colname = gview.Rows[0].Cells[i].Text;
                       
                            gview.Rows[0].Cells[i].Text = "Attendance";
                       
                       
                            if (a != 1)
                            {
                                a = 1;

                                gview.Rows[1].Cells[i].Text = "Present";
                                i = gview.Rows[1].Cells.Count;
                            }

                        
                    }
                }
                if (col == 9 && rows == 2)
                {
                    int row = 0;
                    for (int i = 3; i < gview.Rows.Count; i++)
                    {

                        for (int m = 9; m < gview.HeaderRow.Cells.Count; m++)
                        {


                            string abcol = gview.Rows[1].Cells[m].Text;
                            string app_no = gview.Rows[i].Cells[0].Text;

                            if (app_no != "&nbsp;")
                            {
                                string val = string.Empty;
                                if (row == 0)
                                    row = m;
                                int a = m - row;
                                if (a < 10)
                                {


                                    val = "0" + a + "";
                                }
                                else
                                {
                                    val = Convert.ToString(m);
                                }
                                if (m == 9)
                                {
                                    string chkname = "ctl" + val + "";
                                    CheckBox selectall = (gview.Rows[2].Cells[m].FindControl("ctl00")) as CheckBox;
                                    if (selectall.Checked == true)
                                    {
                                        CheckBox stud_rollno = (gview.Rows[i].Cells[m].FindControl(chkname)) as CheckBox;
                                        stud_rollno.Checked = true;
                                    }
                                    else
                                    {
                                        CheckBox stud_rollno = (gview.Rows[i].Cells[m].FindControl(chkname)) as CheckBox;
                                        stud_rollno.Checked = false;
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
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
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

    public void bindposition()
    {
        try
        {
           
                ds.Clear();
                cbldes.Items.Clear();
                string itemname = "select distinct MasterCode, MasterValue from CO_MasterValues where MasterCriteria ='Company Position' and MasterCode in(select composition from Company_datails where CompanyFK in('" + Convert.ToString(drpcompany.SelectedValue) + "')) order by MasterCode";
                ds.Clear();
                ds = d2.select_method_wo_parameter(itemname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldes.DataSource = ds;
                    cbldes.DataTextField = "MasterValue";
                    cbldes.DataValueField = "MasterCode";
                    cbldes.DataBind();


                }
                binddate();
        }
        catch
        {
        }
    }
    public void chkdes_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
            if (chkdes.Checked == true)
            {
                for (int i = 0; i < cbldes.Items.Count; i++)
                {
                    if (chkdes.Checked == true)
                    {
                        cbldes.Items[i].Selected = true;
                        txtdes.Text = "Description(" + (cbl_batch.Items.Count) + ")";
                        build1 = cbldes.Items[i].Value.ToString();
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
                    cbldes.Items[i].Selected = false;
                    txtdes.Text = "--Select--";
                }
            }
            binddate();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbldes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            int seatcount = 0;
            chkdes.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < cbldes.Items.Count; i++)
            {
                if (cbldes.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txtdes.Text = "--Select--";
                    build = cbldes.Items[i].Value.ToString();
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
            if (seatcount == cbldes.Items.Count)
            {
                txtdes.Text = "Description(" + seatcount.ToString() + ")";
                chkdes.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtdes.Text = "--Select--";
                chkdes.Text = "--Select--";
            }
            else
            {
                txtdes.Text = "Description(" + seatcount.ToString() + ")";
            }
            binddate();
        }
        catch (Exception ex)
        {
        }
    }
}