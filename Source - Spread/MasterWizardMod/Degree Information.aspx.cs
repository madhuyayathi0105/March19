using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using InsproDataAccess;
using System.Collections.Generic;

public partial class Degree_Information : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string grouporusercode = string.Empty;
    Hashtable has = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("MasterWizardMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MasterWizardMod/MasterWizardMenuIndex.aspx");
                    return;
                }
            }
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                bindcollege();
                bindbatch();
                binddegree();
                bindbranch();

            }
        }
        catch
        {
        }
    }

    protected void bindcollege()
    {
        ddlcollege.Items.Clear();
        string grporusercode = string.Empty;
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grporusercode = " and user_code=" + Session["usercode"].ToString().Trim() + "";
        }


        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where cp.college_code=cf.college_code " + grporusercode + "";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
    }

    public void bindbatch()
    {
        ddlbatch.Items.Clear();
        ds = d2.select_method_wo_parameter("bind_batch", "sp");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataTextField = "batch_year";
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();
            ddlbatch.Items.Insert(0, " ");
        }
        int count1 = ds.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            ddlbatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
    }

    public void binddegree()
    {
        ddldegree.Items.Clear();
        string collgCode = string.Empty;
        if (ddlcollege.SelectedValue != string.Empty)
        {
            collgCode = ddlcollege.SelectedValue;
        }
        //usercode = Session["usercode"].ToString();
        //collegecode = Session["collegecode"].ToString();
        //singleuser = Session["single_user"].ToString();
        //group_user = Session["group_code"].ToString();
        //if (group_user.Contains(';'))
        //{
        //    string[] group_semi = group_user.Split(';');
        //    group_user = group_semi[0].ToString();
        //}
        //has.Clear();
        //has.Add("single_user", singleuser);
        //has.Add("group_code", group_user);
        //has.Add("college_code", collegecode);
        //has.Add("user_code", usercode);
        //ds = d2.select_method("bind_degree", has, "sp");

        string degQry = " select Course_Name,Course_Id from course where college_code='" + collgCode + "'";
        DataSet ds = d2.select_method_wo_parameter(degQry, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataTextField = "Course_Name";
            ddldegree.DataValueField = "Course_Id";
            ddldegree.DataBind();

            DegddlCourse.DataSource = ds;
            DegddlCourse.DataTextField = "Course_Name";
            DegddlCourse.DataValueField = "Course_Id";
            DegddlCourse.DataBind();
        }
    }

    public void bindbranch()
    {
        ddlbranch.Items.Clear();
        string collgCode = string.Empty;
        if (ddlcollege.SelectedValue != string.Empty)
        {
            collgCode = ddlcollege.SelectedValue;
        }
        //has.Clear();
        //usercode = Session["usercode"].ToString();
        //collegecode = Session["collegecode"].ToString();
        //singleuser = Session["single_user"].ToString();
        //group_user = Session["group_code"].ToString();
        //if (group_user.Contains(';'))
        //{
        //    string[] group_semi = group_user.Split(';');
        //    group_user = group_semi[0].ToString();
        //}
        //has.Add("single_user", singleuser);
        //has.Add("group_code", group_user);
        //has.Add("course_id", ddldegree.SelectedValue);
        //has.Add("college_code", collegecode);
        //has.Add("user_code", usercode);
        //ds = d2.select_method("bind_branch", has, "sp");

        string deptQry = "select Dept_Name,Dept_Code from Department where college_code='" + collgCode + "'";
        DataSet ds = d2.select_method_wo_parameter(deptQry, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "Dept_Name";
            ddlbranch.DataValueField = "Dept_Code";
            ddlbranch.DataBind();

            DegddlBranch.DataSource = ds;
            DegddlBranch.DataTextField = "Dept_Name";
            DegddlBranch.DataValueField = "Dept_Code";
            DegddlBranch.DataBind();
        }
    }

 

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        errmsg.Text = string.Empty;
        gviewDegree.Visible = false;
        bindbatch();
        binddegree();
        bindbranch();
      
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        //gview.Visible = false;
        //lblrptname.Visible = false;
        //txtexcelname.Visible = false;
        //btnxl.Visible = false;
        //btnprintmaster.Visible = false;
        //NEWPrintMater1.Visible = false;
        //errlbl.Visible = false;
        errmsg.Visible = false;
        errmsg.Text = string.Empty;
        gviewDegree.Visible = false;
        binddegree();
        bindbranch();
      
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        //gview.Visible = false;
        //lblrptname.Visible = false;
        //txtexcelname.Visible = false;
        //btnxl.Visible = false;
        //btnprintmaster.Visible = false;
        //NEWPrintMater1.Visible = false;
        //errlbl.Visible = false;
        errmsg.Visible = false;
        errmsg.Text = string.Empty;
        gviewDegree.Visible = false;
        bindbranch();
      
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //gview.Visible = false;
        //lblrptname.Visible = false;
        //txtexcelname.Visible = false;
        //btnxl.Visible = false;
        //btnprintmaster.Visible = false;
        //NEWPrintMater1.Visible = false;
        //errlbl.Visible = false;
        errmsg.Visible = false;
        errmsg.Text = string.Empty;
        gviewDegree.Visible = false;
     
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Btn_AddNewRow.Enabled = false;
        gview.DataSource = null;
        gview.DataBind();
        importRadio.Enabled = false;
        entryRadio.Enabled = false;
        txtSeatSurrend.Text = string.Empty;
        txtSeatSurrend.Enabled = false;
        btnPopDeleteDeg.Visible = false;
        divEnterDegreeDetails.Visible = true;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string collegeCode = string.Empty;
            string batchCode = string.Empty;
            string DegreeCode = string.Empty;
            string branchCode = string.Empty;
            string semVal = string.Empty;
            string DegreeVal = string.Empty;
            DataTable Dtable = new DataTable();
            DataRow dr;

            if (ddlcollege.SelectedValue != string.Empty)
            {
                collegeCode = ddlcollege.SelectedValue;
            }
            else
            {
                errmsg.Text = "Please Choose College";
                errmsg.Visible = true;
                return;
            }
           
            if (ddldegree.SelectedValue != string.Empty)
            {
                DegreeCode = ddldegree.SelectedValue;
                DegreeVal = ddldegree.SelectedItem.Text;
            }
            else
            {
                errmsg.Text = "Please Choose Degree";
                errmsg.Visible = true;
                return;
            }
            if (ddlbranch.SelectedValue != string.Empty)
            {
                branchCode = ddlbranch.SelectedValue;
            }
            else
            {
                errmsg.Text = "Please Choose Branch";
                errmsg.Visible = true;
                return;
            }
            string qry = string.Empty;
            if (ddlbatch.SelectedIndex != 0)
            {
                qry = "select distinct d.*,dp.dept_name from degree d,Department dp,NDegree nd where d.Dept_Code=dp.Dept_Code and d.course_id='" + DegreeCode + "' and d.college_code='" + collegeCode + "' and dp.dept_code='" + ddlbranch.SelectedValue.ToString() + "' and nd.Degree_code=d.Degree_Code and nd.batch_year='" + ddlbatch.SelectedItem.Text.ToString() + "'";
            }
            else
            {

                qry = "select * from degree d,Department dp where d.Dept_Code=dp.Dept_Code and d.course_id='" + DegreeCode + "' and d.college_code='" + collegeCode + "' and dp.dept_code='" + ddlbranch.SelectedValue.ToString() + "'";
            }
            DataSet ds_select = d2.select_method_wo_parameter(qry, "Type");

            if (ds_select.Tables.Count > 0 && ds_select.Tables[0].Rows.Count > 0)
            {
                Dtable.Columns.Add("Noofsec");
                Dtable.Columns.Add("Noofsubdiv");
                Dtable.Columns.Add("Degregcode");
                Dtable.Columns.Add("Startyear");
                Dtable.Columns.Add("Regulation");
                Dtable.Columns.Add("IsSurren");
                Dtable.Columns.Add("AccStatus");
                Dtable.Columns.Add("TxtSurren");
                Dtable.Columns.Add("Txtgrade");
                Dtable.Columns.Add("Code");
                Dtable.Columns.Add("Name");
                Dtable.Columns.Add("System");
                Dtable.Columns.Add("Duration");
                Dtable.Columns.Add("Acronym");
                Dtable.Columns.Add("Noofseat");
                Dtable.Columns.Add("Firstsem");
                Dtable.Columns.Add("batchyrnew");

                for (int row = 0; row < ds_select.Tables[0].Rows.Count; row++)
                {
                    string selectNqry = "select distinct * from Ndegree where degree_code='" + Convert.ToString(ds_select.Tables[0].Rows[row]["Degree_Code"]) + "' and college_code='" + Convert.ToString(ds_select.Tables[0].Rows[row]["college_code"]) + "'";
                    DataSet dsNdeg = d2.select_method_wo_parameter(selectNqry, "Type");

                    string gradeVal = string.Empty;
                    string value = string.Empty;
                    dr = Dtable.NewRow();
                    dr["Noofsec"] = Convert.ToString(ds_select.Tables[0].Rows[row]["NoofSections"]);
                    dr["Noofsubdiv"] = Convert.ToString(ds_select.Tables[0].Rows[row]["Noofsubdiv"]);
                    dr["Degregcode"] = Convert.ToString(ds_select.Tables[0].Rows[row]["RegCode"]).Trim();
                    dr["Startyear"] = Convert.ToString(ds_select.Tables[0].Rows[row]["Start_Year"]);
                    dr["Regulation"] = Convert.ToString(ds_select.Tables[0].Rows[row]["Regulation"]);
                    dr["IsSurren"] = Convert.ToString(ds_select.Tables[0].Rows[row]["IsSurendered"]);
                    dr["AccStatus"] = Convert.ToString(ds_select.Tables[0].Rows[row]["Acc_Status"]);
                    dr["TxtSurren"] = Convert.ToString(ds_select.Tables[0].Rows[row]["SurPerc"]);
                    dr["Code"] = Convert.ToString(ds_select.Tables[0].Rows[row]["Degree_Code"]);
                    dr["Name"] = DegreeVal + "-" + Convert.ToString(ds_select.Tables[0].Rows[row]["dept_Name"]);
                    dr["System"] = Convert.ToString(ds_select.Tables[0].Rows[row]["Exam_System"]);
                    dr["Duration"] = Convert.ToString(ds_select.Tables[0].Rows[row]["Duration"]);
                    dr["Acronym"] = Convert.ToString(ds_select.Tables[0].Rows[row]["Acronym"]);
                    dr["Noofseat"] = Convert.ToString(ds_select.Tables[0].Rows[row]["No_Of_seats"]);
                    dr["batchyrnew"] = Convert.ToString(dsNdeg.Tables[0].Rows[row]["batch_year"]);
                    if (Convert.ToInt16(ds_select.Tables[0].Rows[row]["First_Year_Nonsemester"]) == 0)
                    {
                        value = "False";
                    }
                    else
                    {
                        value = "True";
                    }
                    dr["Firstsem"] = value;
                    if (Convert.ToInt16(dsNdeg.Tables[0].Rows[0]["Grade_System"]) == 0)
                    {
                        gradeVal = "False";
                    }
                    else
                    {
                        gradeVal = "True";
                    }
                    dr["Txtgrade"] = gradeVal;
                    Dtable.Rows.Add(dr);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "customScript", "<script>alert('No Records');</script>", false);
            
                gviewDegree.Visible = false;
                
            }
            gviewDegree.DataSource = Dtable;
            gviewDegree.DataBind();
            gviewDegree.Visible = true;
        }
        catch
        {
        }
    }

    protected void gviewDegree_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void gviewDegree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divEnterDegreeDetails.Visible = true;
            btnPopDeleteDeg.Visible = true;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            gviewDegree.Visible = true;
            string colgcode = string.Empty;
            if (Convert.ToString(rowIndex) != "" && Convert.ToString(selectedCellIndex) != "1")
            {
                if (!string.IsNullOrEmpty(ddlcollege.SelectedValue))
                {
                    colgcode = ddlcollege.SelectedValue;
                }
                btnPopSaveDeg.Visible = false;
                btnPopUpdateDeg.Visible = true;
                txtdegbatch.Text = (gviewDegree.Rows[rowIndex].FindControl("lblbatchyrnew") as Label).Text;
                string name = (gviewDegree.Rows[rowIndex].FindControl("lblname") as Label).Text;
                string[] spl = name.Split('-');
                DegddlCourse.SelectedIndex = DegddlCourse.Items.IndexOf(DegddlCourse.Items.FindByText(spl[0]));
                DegddlCourse.Enabled = false;
                DegddlBranch.SelectedIndex = DegddlBranch.Items.IndexOf(DegddlBranch.Items.FindByText(spl[1]));
                DegddlBranch.Enabled = false;
                txtAcronym.Text = (gviewDegree.Rows[rowIndex].FindControl("lblacr") as Label).Text;
                txtYerIntro.Text = (gviewDegree.Rows[rowIndex].FindControl("lblstrtyer") as Label).Text;
                txtregcode.Text = (gviewDegree.Rows[rowIndex].FindControl("lbldegcod") as Label).Text;
                txtNoSeats.Text = (gviewDegree.Rows[rowIndex].FindControl("lblnoofseat") as Label).Text;
                txtNoSeaction.Text = (gviewDegree.Rows[rowIndex].FindControl("lblsec") as Label).Text;
                txtSubDiv.Text = (gviewDegree.Rows[rowIndex].FindControl("lblsubdiv") as Label).Text;
                txtReg.Text = (gviewDegree.Rows[rowIndex].FindControl("lblreg") as Label).Text;
                txtduration.Text = (gviewDegree.Rows[rowIndex].FindControl("lbldur") as Label).Text;
                txtAccredition.Text = (gviewDegree.Rows[rowIndex].FindControl("lblaccstatus") as Label).Text;
                txtSeatSurrend.Text = (gviewDegree.Rows[rowIndex].FindControl("lbltxtsurren") as Label).Text;
                string chkSurren = (gviewDegree.Rows[rowIndex].FindControl("lblsurren") as Label).Text;
                string frstYer = (gviewDegree.Rows[rowIndex].FindControl("lblsem") as Label).Text;
                string gradesys = (gviewDegree.Rows[rowIndex].FindControl("lblgradesys") as Label).Text;
                lbldegCode.Text = (gviewDegree.Rows[rowIndex].FindControl("lblcode") as Label).Text;
             
                if (chkSurren == "False")
                {
                    chkSurrendseat.Checked = false;
                    txtSeatSurrend.Enabled = false;
                }
                else
                {
                    chkSurrendseat.Checked = true;
                    txtSeatSurrend.Enabled = true;
                }
                if (frstYer == "True")
                {
                    chkNonsem.Checked = true;
                }
                else
                {
                    chkNonsem.Checked = false;
                }
                if (gradesys == "False")
                {
                    chkGrade.Checked = false;
                    importRadio.Checked = false;
                    entryRadio.Checked = false;
                    importRadio.Enabled = false;
                    entryRadio.Enabled = false;
                    Btn_AddNewRow.Enabled = false;

                }
                else
                {
                    chkGrade.Checked = true;
                    string Ndegqry = "select * from grade_master where College_Code='" + colgcode + "' and Degree_Code='" + lbldegCode.Text + "'";
                    DataSet dsgrade = d2.select_method_wo_parameter(Ndegqry, "Text");
                    DataTable dsgradeset = new DataTable();
                    DataRow dr;
                    if (dsgrade.Tables.Count > 0 && dsgrade.Tables[0].Rows.Count > 0)
                    {
                        Btn_AddNewRow.Enabled = true;
                        dsgradeset.Columns.Add("frmMark");
                        dsgradeset.Columns.Add("toMark");
                        dsgradeset.Columns.Add("markGrade");
                        dsgradeset.Columns.Add("gradePoint");

                        for (int ro = 0; ro < dsgrade.Tables[0].Rows.Count; ro++)
                        {
                            dr = dsgradeset.NewRow();
                            dr["frmMark"] = Convert.ToString(dsgrade.Tables[0].Rows[ro]["Frange"]);
                            dr["toMark"] = Convert.ToString(dsgrade.Tables[0].Rows[ro]["Trange"]);
                            dr["markGrade"] = Convert.ToString(dsgrade.Tables[0].Rows[ro]["Mark_Grade"]);
                            dr["gradePoint"] = Convert.ToString(dsgrade.Tables[0].Rows[ro]["Credit_Points"]);
                            dsgradeset.Rows.Add(dr);
                        }
                        gview.DataSource = dsgradeset;
                        gview.DataBind();
                        gview.Visible = true;
                    }
                }
            }
        }
        catch
        {
        }
    }

    #region ADD_DEGREE

    protected void Surrendseat_Onclick(object sender, EventArgs e)
    {
        if (chkSurrendseat.Checked)
        {
            txtSeatSurrend.Enabled = true;
            txtSeatSurrend.Text = "0";
        }
        else
        {
            txtSeatSurrend.Text = string.Empty;
            txtSeatSurrend.Enabled = false;
        }
    }

    protected void chkGrade_Onclick(object sender, EventArgs e)
    {
        if (chkGrade.Checked)
        {
            importRadio.Enabled = true;
            entryRadio.Enabled = true;
            Btn_AddNewRow.Enabled = true;
        }
        else
        {
            importRadio.Enabled = false;
            entryRadio.Enabled = false;
            //importRadio.Checked = false;
            //entryRadio.Checked = false;
            Btn_AddNewRow.Enabled = false;
        }
    }

    protected void btnPopSaveDeg_Click(object sender, EventArgs e)
    {
        try
        {
            semRadio.Checked = true;
            lblPopDegErr.Text = "";
            lblPopDegErr.Visible = false;
            string courseId = string.Empty;
            string branchCode = string.Empty;
            string firstYerNonSem = string.Empty;
            string collegeCode = string.Empty;
            string isSurrendSeat = string.Empty;
            string surrendSeat = string.Empty;
            string semeRadio = string.Empty;
            string gradeSys = string.Empty;
            string degCode = string.Empty;
            int cont = 0;

            if (txtdegbatch.Text == string.Empty)
            {
                lblPopDegErr.Text = "Batch Year Should Not Be Empty";
                lblPopDegErr.Visible = true;
                return;
            }
            if (txtAcronym.Text == string.Empty)
            {
                lblPopDegErr.Text = "Acronym Should Not Be Empty";
                lblPopDegErr.Visible = true;
                return;
            }
            if (txtregcode.Text == string.Empty)
            {
                lblPopDegErr.Text = "Register Code Should Not Be Empty";
                lblPopDegErr.Visible = true;
                return;
            }
            if (txtNoSeats.Text == string.Empty)
            {
                lblPopDegErr.Text = "No Of Seats Should Not Be Empty";
                lblPopDegErr.Visible = true;
                return;
            }
            if (txtduration.Text == string.Empty)
            {
                lblPopDegErr.Text = "Duration Should Not Be Empty";
                lblPopDegErr.Visible = true;
                return;
            }
            if (txtYerIntro.Text == string.Empty)
            {
                lblPopDegErr.Text = "Year Of Introducation Should Not Be Empty";
                lblPopDegErr.Visible = true;
                return;
            }

            if (DegddlCourse.SelectedValue != string.Empty)
            {
                courseId = DegddlCourse.SelectedValue;
            }
            if (DegddlBranch.SelectedValue != string.Empty)
            {
                branchCode = DegddlBranch.SelectedValue;
                string s = DegddlBranch.SelectedItem.Text;
            }
            if (ddlcollege.SelectedValue != string.Empty)
            {
                collegeCode = ddlcollege.SelectedValue;
            }
            if (chkNonsem.Checked)
            {
                firstYerNonSem = "1";
            }
            else
            {
                firstYerNonSem = "0";
            }
            if (chkSurrendseat.Checked)
            {
                isSurrendSeat = "1";
                surrendSeat = txtSeatSurrend.Text;
            }
            else
            {
                isSurrendSeat = "0";
                surrendSeat = "0";
            }
            if (semRadio.Checked)
            {
                semeRadio = "Semester";
            }
            if (chkGrade.Checked)
            {
                gradeSys = "1";
            }
            else
            {
                gradeSys = "0";
            }

            string deg_Qry = "insert into Degree (Course_Id,Dept_Code,Exam_System,Duration,First_Year_Nonsemester,Acronym,No_Of_seats,college_code,auto_increment,Start_Year,Acc_Status,IsSurendered,SurPerc,NoofSections,RegCode,Noofsubdiv,Regulation) values('" + courseId + "','" + branchCode + "','" + semeRadio + "','" + txtduration.Text + "','" + firstYerNonSem + "','" + txtAcronym.Text + "','" + txtNoSeats.Text + "','" + collegeCode + "','0','" + txtYerIntro.Text + "','" + txtAccredition.Text + "','" + isSurrendSeat + "','" + surrendSeat + "','" + txtNoSeaction.Text + "','" + txtregcode.Text + "','" + txtSubDiv.Text + "','" + txtReg.Text + "')";
            int ds_deg = d2.update_method_wo_parameter(deg_Qry, "Text");

            if (ds_deg > 0)
            {
                string selectDeg = "select * from degree where Course_Id='" + courseId + "' and Dept_Code='" + branchCode + "' order by Degree_Code desc";
                DataSet ds_gegsel = d2.select_method_wo_parameter(selectDeg, "Text");
                degCode = Convert.ToString(ds_gegsel.Tables[0].Rows[0]["Degree_Code"]);
                lbldegCode.Text = Convert.ToString(ds_gegsel.Tables[0].Rows[0]["Degree_Code"]);
                string nDeg_Qry = "insert into Ndegree (Degree_code,Exam_system,First_Year_Nonsemester,batch_year,college_code,Nseats,Nsections,NDurations,Grade_System,surperc,acc_status,Nsubdiv,Regulation) values('" + degCode + "','Semester','" + firstYerNonSem + "','" + txtdegbatch.Text + "','" + collegeCode + "','" + txtNoSeats.Text + "','" + txtNoSeaction.Text + "','" + txtduration.Text + "','" + gradeSys + "','" + surrendSeat + "','" + txtAccredition.Text + "','" + txtSubDiv.Text + "','" + txtReg.Text + "')";
                int ds_nDeg = d2.update_method_wo_parameter(nDeg_Qry, "Text");
                cont++;
            }

            if (chkGrade.Checked)
            {
                string clasify = string.Empty;

                for (int row = 0; row < gview.Rows.Count; row++)
                {
                    string markGrade = (gview.Rows[row].FindControl("txtmarkgrade") as TextBox).Text;
                    string fRange = (gview.Rows[row].FindControl("txtfrmmark") as TextBox).Text;
                    string tRange = (gview.Rows[row].FindControl("txttomark") as TextBox).Text;
                    string creditPoint = (gview.Rows[row].FindControl("txtgradePoint") as TextBox).Text;


                    string gradeMas = "insert into Grade_Master (Mark_Grade,Frange,Trange,Degree_Code,College_Code,Credit_Points,batch_year,classify)values('" + markGrade + "','" + fRange + "','" + tRange + "','" + degCode + "','" + collegeCode + "','" + creditPoint + "','" + txtdegbatch.Text + "','" + clasify + "')";
                    int ds_grade = d2.update_method_wo_parameter(gradeMas, "Type");
                }
            }

            if (cont > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "customScript", "<script>alert('Saved Successfully');</script>", false);
                btnPopDeleteDeg.Visible = false;
                gviewDegree.Visible = false;
                clear();
            }
        }
        catch
        {
        }
    }

    protected void btnPopDeleteDeg_Click(object sender, EventArgs e)
    {
        string collCode = string.Empty;
        string degCod = lbldegCode.Text;
        int cont = 0;

        if (ddlcollege.SelectedValue != string.Empty)
        {
            collCode = ddlcollege.SelectedValue;
        }
        string deldeg = "delete from Degree where college_code='" + collCode + "' and Degree_Code='" + degCod + "'";
        int dele = d2.update_method_wo_parameter(deldeg, "Text");

        if (dele > 0)
        {
            string delNdeg = "delete from Ndegree where Degree_code='" + degCod + "'";
            int Ndele = d2.update_method_wo_parameter(delNdeg, "Text");

            string delGrade = "delete from grade_master where College_Code='" + collCode + "' and Degree_Code='" + degCod + "'";
            int grad = d2.update_method_wo_parameter(delGrade, "Text");
            cont++;
        }

        if (cont > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "customScript", "<script>alert('Deleted Successfully');</script>", false);
            btnPopSaveDeg.Visible = true;
            btnPopUpdateDeg.Visible = false;
            gviewDegree.Visible = false;
            clear();
        }
    }

    protected void btnPopUpdateDeg_Click(object sender, EventArgs e)
    {
        semRadio.Checked = true;
        lblPopDegErr.Text = "";
        lblPopDegErr.Visible = false;
        string courseId = string.Empty;
        string branchCode = string.Empty;
        string firstYerNonSem = string.Empty;
        string collegeCode = string.Empty;
        string isSurrendSeat = string.Empty;
        string surrendSeat = string.Empty;
        string semeRadio = string.Empty;
        string gradeSys = string.Empty;
        string degCode = lbldegCode.Text;
        int cont = 0;

        if (txtAcronym.Text == string.Empty)
        {
            lblPopDegErr.Text = "Acronym Should Not Be Empty";
            lblPopDegErr.Visible = true;
            return;
        }
        if (txtregcode.Text == string.Empty)
        {
            lblPopDegErr.Text = "Register Code Should Not Be Empty";
            lblPopDegErr.Visible = true;
            return;
        }
        if (txtNoSeats.Text == string.Empty)
        {
            lblPopDegErr.Text = "No Of Seats Should Not Be Empty";
            lblPopDegErr.Visible = true;
            return;
        }
        if (txtduration.Text == string.Empty)
        {
            lblPopDegErr.Text = "Duration Should Not Be Empty";
            lblPopDegErr.Visible = true;
            return;
        }
        if (txtYerIntro.Text == string.Empty)
        {
            lblPopDegErr.Text = "Year Of Introducation Should Not Be Empty";
            lblPopDegErr.Visible = true;
            return;
        }
        if (ddlcollege.SelectedValue != string.Empty)
        {
            collegeCode = ddlcollege.SelectedValue;
        }
        if (chkNonsem.Checked)
        {
            firstYerNonSem = "1";
        }
        else
        {
            firstYerNonSem = "0";
        }
        if (chkSurrendseat.Checked)
        {
            isSurrendSeat = "1";
            surrendSeat = txtSeatSurrend.Text;
        }
        else
        {
            isSurrendSeat = "0";
            surrendSeat = "0";
        }
        if (semRadio.Checked)
        {
            semeRadio = "Semester";
        }
        if (chkGrade.Checked)
        {
            gradeSys = "1";
        }
        else
        {
            gradeSys = "0";
        }

        string updateDeg = "update Degree set Duration='" + txtduration.Text + "',First_Year_Nonsemester='" + firstYerNonSem + "',Acronym='" + txtAcronym.Text + "',No_Of_seats='" + txtNoSeats.Text + "',Start_Year='" + txtYerIntro.Text + "',Acc_Status='" + txtAccredition.Text + "',IsSurendered='" + isSurrendSeat + "',SurPerc='" + surrendSeat + "',NoofSections='" + txtNoSeaction.Text + "',RegCode='" + txtregcode.Text + "',Noofsubdiv='" + txtSubDiv.Text + "',Regulation='" + txtReg.Text + "' where Degree_Code='" + degCode + "' and college_code='" + collegeCode + "'";
        int UpdteDeg = d2.update_method_wo_parameter(updateDeg, "Text");

        if (UpdteDeg > 0)
        {
            string updateNdeg = "update Ndegree set First_Year_Nonsemester='" + firstYerNonSem + "',batch_year='" + txtdegbatch.Text + "',Nseats='" + txtNoSeats.Text + "',Nsections='" + txtNoSeaction.Text + "',NDurations=' " + txtduration.Text + "',Grade_System='" + gradeSys + "',surperc='" + surrendSeat + "',acc_status='" + txtAccredition.Text + "',Nsubdiv='" + txtSubDiv.Text + "',Regulation='" + txtReg.Text + "' where Degree_code='" + degCode + "' and college_code='" + collegeCode + "'";
            int UpdteNdeg = d2.update_method_wo_parameter(updateNdeg, "Text");

            string qry = "delete from grade_master where College_Code='" + collegeCode + "' and Degree_Code='" + degCode + "'";
            int grad = d2.update_method_wo_parameter(qry, "Text");

            if (chkGrade.Checked)
            {
                string clasify = string.Empty;

                for (int row = 0; row < gview.Rows.Count; row++)
                {
                    string markGrade = (gview.Rows[row].FindControl("txtmarkgrade") as TextBox).Text;
                    string fRange = (gview.Rows[row].FindControl("txtfrmmark") as TextBox).Text;
                    string tRange = (gview.Rows[row].FindControl("txttomark") as TextBox).Text;
                    string creditPoint = (gview.Rows[row].FindControl("txtgradePoint") as TextBox).Text;

                    string gradeMas = "insert into Grade_Master (Mark_Grade,Frange,Trange,Degree_Code,College_Code,Credit_Points,batch_year,classify)values('" + markGrade + "','" + fRange + "','" + tRange + "','" + degCode + "','" + collegeCode + "','" + creditPoint + "','" + txtdegbatch.Text + "','" + clasify + "')";
                    int ds_grade = d2.update_method_wo_parameter(gradeMas, "Type");
                }
            }

            if (UpdteNdeg > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "customScript", "<script>alert('Updated Successfully');</script>", false);
                btnPopSaveDeg.Visible = true;
                btnPopUpdateDeg.Visible = false;
                gviewDegree.Visible = false;
                clear();
            }
        }

    }

    protected void btnPopExitDeg_Click(object sender, EventArgs e)
    {
        clear();
        divEnterDegreeDetails.Visible = false;
        btnPopUpdateDeg.Visible = false;
        btnPopSaveDeg.Visible = true;
        entryRadio.Enabled = false;
        entryRadio.Checked = false;
        importRadio.Checked = false;
        importRadio.Enabled = false;
        Btn_AddNewRow.Enabled = false;
    }

    protected void Btn_AddNewRow_Click(object sender, EventArgs e)
    {
        DataTable dt_newrow = new DataTable();
        DataRow dr;
        dt_newrow.Columns.Add("frmMark");
        dt_newrow.Columns.Add("toMark");
        dt_newrow.Columns.Add("markGrade");
        dt_newrow.Columns.Add("gradePoint");

        if (ddl_addrow.SelectedValue == "NewRow")
        {
            for (int i = 0; i < gview.Rows.Count; i++)
            {
                dr = dt_newrow.NewRow();

                string frm_Mark = (gview.Rows[i].FindControl("txtfrmmark") as TextBox).Text;
                dr["frmMark"] = frm_Mark;

                string to_Mark = (gview.Rows[i].FindControl("txttomark") as TextBox).Text;
                dr["toMark"] = to_Mark;

                string mark_Grade = (gview.Rows[i].FindControl("txtmarkgrade") as TextBox).Text;
                dr["markGrade"] = mark_Grade;

                string grade_point = (gview.Rows[i].FindControl("txtgradePoint") as TextBox).Text;
                dr["gradePoint"] = grade_point;

                dt_newrow.Rows.Add(dr);
            }
            ViewState["CurrentTable"] = dt_newrow;

            dr = dt_newrow.NewRow();

            dr["frmMark"] = string.Empty;
            dr["toMark"] = string.Empty;
            dr["markGrade"] = string.Empty;
            dr["gradePoint"] = string.Empty;

            dt_newrow.Rows.Add(dr);
        }

        gview.DataSource = dt_newrow;
        gview.DataBind();
        gview.Visible = true;
    }

    protected void Btn_btndeleteGrade_Click(object sender, EventArgs e)
    {
        try
        {
            string collCode = string.Empty;
            string degCod = lbldegCode.Text;
            lblPopDegErr.Visible = false;

            if (ddlcollege.SelectedValue != string.Empty)
            {
                collCode = ddlcollege.SelectedValue;
            }
            if (gview.Rows.Count > 0)
            {
                string delNdeg = "delete from Ndegree where Degree_code='" + degCod + "'";
                int Ndele = d2.update_method_wo_parameter(delNdeg, "Text");

                string delGrade = "delete from grade_master where College_Code='" + collCode + "' and Degree_Code='" + degCod + "'";
                int grad = d2.update_method_wo_parameter(delGrade, "Text");

                if (grad > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "customScript", "<script>alert('Grade Deleted');</script>", false);
                    gview.DataSource = null;
                    gview.DataBind();
                }
            }
            else
            {
                lblPopDegErr.Text = "No Grade To Delete";
                lblPopDegErr.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void clear()
    {
        txtdegbatch.Text = string.Empty;
        lblPopDegErr.Text = string.Empty;
        lbldegCode.Text = string.Empty;
        lblPopDegErr.Visible = false;
        chkGrade.Checked = false;
        importRadio.Checked = false;
        entryRadio.Checked = false;
        DegddlCourse.ClearSelection();
        DegddlBranch.ClearSelection();
        DegddlCourse.Enabled = true;
        DegddlBranch.Enabled = true;
        txtAcronym.Text = string.Empty;
        txtregcode.Text = string.Empty;
        txtNoSeats.Text = string.Empty;
        txtNoSeaction.Text = string.Empty;
        txtYerIntro.Text = string.Empty;
        txtSubDiv.Text = string.Empty;
        txtReg.Text = string.Empty;
        txtduration.Text = string.Empty;
        txtAccredition.Text = string.Empty;
        txtSeatSurrend.Text = string.Empty;
        chkNonsem.Checked = false;
        chkSurrendseat.Checked = false;
        txtSeatSurrend.Enabled = false;
        btnPopDeleteDeg.Visible = false;
        Btn_AddNewRow.Enabled = false;
        gview.DataSource = null;
        gview.DataBind();
    }

    #endregion

}