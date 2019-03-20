using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using Gios.Pdf;
using System.IO;
using System.Data.SqlClient;
using InsproDataAccess;
using System.Net.Mail;
using System.Net;
using System.Configuration;
public partial class StudAdmissionSelection : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    InsproDirectAccess DirAccess = new InsproDirectAccess();
    AdmissionNumberAndApplicationNumberGeneration autoGenDS = new AdmissionNumberAndApplicationNumberGeneration();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    bool boolfeeupdate = false;
    static bool usBasedRights = false;
    static ArrayList colord = new ArrayList();
    static byte roll = 0;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"]);
    static string admisionvalue = string.Empty;
    static bool recptset = false;
    static bool editableRights = false;
    static string formatevalue = string.Empty;
    static byte autoByte = 0;
    string paavaiNewApplcationNO = string.Empty;

    static string loadval = string.Empty;
    static string colval = string.Empty;
    static string printval = string.Empty;
    static string loadval1 = string.Empty;
    static string colval1 = string.Empty;
    static string savecolumnoder = string.Empty;

    Hashtable htprintVal = new Hashtable();
    Hashtable htColVal = new Hashtable();

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Session.Remove("grid");
        if (Session["grid"] != null)
        {
            gridstud.DataSource = Session["grid"];
            gridstud.DataBind();
            // Session.Remove("grid");
        }
        else
        {
            gridstud.DataSource = null;
            gridstud.DataBind();
        }
    }
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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                applicationPdfFormateRights();
                btnFeeUpdate.Visible = false;
                lblstudmsg.Visible = false;
                bindCollege();
                if (cblclg.Items.Count > 0)
                    collegecode = Convert.ToString(getCblSelectedValue(cblclg));
                bindBtch();
                bindedu();
                binddeg();
                binddept();
                //  loadfinanceyear();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                btnrejectreason.Attributes.Add("onfocus", "flg1()");
                //columnType();
                columnordertype();
                string user = "";
                if (group_user.Trim() != "" && group_user.Trim() != "0")
                    user = " and  group_code ='" + Session["group_code"].ToString() + "'";
                else
                    user = " and usercode='" + Session["usercode"].ToString() + "'";
                string viewformat = d2.GetFunction(" select value from Master_Settings where settings='Application view format' " + user + "");
                ViewState["applicationviewformatset"] = viewformat;
                Bindstage();
                htColVal.Clear();
                htprintVal.Clear();
            }
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            // divcolorder.Attributes.Add("Style", "display:none;");
            if (boolfeeupdate)
                panel4.Attributes.Add("Style", "display:block;");
            else
                panel4.Attributes.Add("Style", "display:none;");
            popSendSms.Attributes.Add("Style", "display:none;");
            panel2.Attributes.Add("Style", "display:none;");
            clgHeader_tbl.Visible = false;
        }
        catch (Exception ex) { }
    }
    #region college
    protected void bindCollege()
    {
        cblclg.Items.Clear();
        cbclg.Checked = false;
        txtclg.Text = "--Select--";
        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblclg.DataSource = ds;
            cblclg.DataTextField = "collname";
            cblclg.DataValueField = "college_code";
            cblclg.DataBind();
            if (cblclg.Items.Count > 0)
            {
                for (int row = 0; row < cblclg.Items.Count; row++)
                {
                    cblclg.Items[row].Selected = true;
                }
                cbclg.Checked = true;
                txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
            }
        }
    }
    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        binddeg();
        binddept();
        //columnType();
        if (cbdegree.Checked)//include degree option
        {
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
        }
        else
        {
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
        }
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        binddeg();
        binddept();
        //columnType();
        if (cbdegree.Checked)//include degree option
        {
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
        }
        else
        {
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
        }
    }
    #endregion
    #region batch
    public void bindBtch()
    {
        try
        {
            ddl_batch.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
        }
        catch { }
    }
    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindedu();
        binddeg();
        binddept();
        if (cbdegree.Checked)//include degree option
        {
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
        }
        else
        {
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
        }
    }
    #endregion
    #region edulevel
    public void bindedu()
    {
        try
        {
            ddledu.Items.Clear();
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            if (!string.IsNullOrEmpty(collegecode) && collegecode != "0")
            {
                string selQ = " select distinct Edu_Level from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code in('" + collegecode + "') and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' order by Edu_Level desc";
                ds = d2.select_method_wo_parameter(selQ, "Text");
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddledu.DataSource = ds;
                    ddledu.DataTextField = "Edu_Level";
                    ddledu.DataValueField = "Edu_Level";
                    ddledu.DataBind();
                }
                else
                    ddledu.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        { }
    }
    public void ddledu_SelectedIndexchange(object sender, EventArgs e)
    {
        binddeg();
        binddept();
        if (cbdegree.Checked)//include degree option
        {
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
        }
        else
        {
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
        }
    }
    #endregion
    #region degree
    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            cbl_degree.Items.Clear();
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string eduLevel = string.Empty;
            if (ddledu.Items.Count > 0 && ddledu.SelectedItem.Text != "--Select--")
                eduLevel = Convert.ToString(ddledu.SelectedItem.Value);
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code in('" + collegecode + "') ";
            if (!string.IsNullOrEmpty(eduLevel))
                selqry += " and Edu_Level in('" + eduLevel + "')";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
        binddept();
        if (cbdegree.Checked)
        {
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
        }
        else
        {
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
        }
        // cb_degree.Attributes.Add("onclick", "return radioChange();");
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
        binddept();
        if (cbdegree.Checked)
        {
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
        }
        else
        {
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
        }
        //cbl_degree.Attributes.Add("onclick", "return radioChange();");
    }
    #endregion
    #region dept
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string batch = Convert.ToString(ddl_batch.SelectedValue);
            string degree = Convert.ToString(getCblSelectedValue(cbl_degree));
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            if (batch != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiples(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }
        }
        catch { }
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
        if (cbdegree.Checked)//include degree option
        {
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
        }
        else
        {
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
        }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
        if (cbdegree.Checked)
        {
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
        }
        else
        {
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
        }
    }
    #endregion
    protected void rdbtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        gridstud.Visible = false;
        buttonview.Visible = false;
        btngo_Click(sender, e);
        if (rdbtype.SelectedIndex == 0)
        {
            //btngo_Click(sender, e);
            autoByte = 0;
        }
        else if (rdbtype.SelectedIndex == 1)
        {
            autoByte = 1;
        }
        else
        {
            autoByte = 2;
        }
    }
    protected void buttonHide()
    {
        btnshortstud.Visible = false;
        btnadmitstud.Visible = false;
        btncallltrstud.Visible = false;
        btnleftstud.Visible = false;
        btnrejstud.Visible = false;
        txt_studName.Text = string.Empty;
        txt_studApplNo.Text = string.Empty;
        txt_studMblno.Text = string.Empty;
        if (rdbtype.SelectedIndex == 0)
        {
            btnshortstud.Visible = true;
            btnadmitstud.Visible = true;
        }
        else if (rdbtype.SelectedIndex == 1)
        {
            btnadmitstud.Visible = true;
            btncallltrstud.Visible = true;
            btnrejstud.Visible = true;
        }
        else
        {
            btnleftstud.Visible = true;//paavai no need
            // btnleftstud.Visible = false;
            btnrejstud.Visible = true;
        }
        buttonview.Visible = true;
    }
    protected DataSet loadDetails(string selectCol)
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            UserbasedRights();
            string batch = string.Empty;
            string degree = string.Empty;
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            if (ddl_batch.Items.Count > 0)
                batch = Convert.ToString(ddl_batch.SelectedValue);
            if (cbl_degree.Items.Count > 0)
                degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string strReg = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            #endregion
            ////string selCol = " distinct(a.app_formno),''sno,''sel,''stview," + selectCol + ",a.app_no,a.college_code,a.degree_code";
            //  string GrpselCol = "paymode," + groupStr + ",f.app_no";

            string selCol = " ''sno,''sel,''stview," + selectCol + ",a.app_no,a.college_code,a.degree_code";
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch))
            {
                #region Query
                string SelQ = string.Empty;
                string strCondition = string.Empty;
                string strDate = string.Empty;
                string studSearch = string.Empty;
                if (txt_studName.Text != "")
                    studSearch = " and a.stud_name like'" + txt_studName.Text + "%'";
                else if (txt_studApplNo.Text != "")
                    studSearch = " and a.app_formno='" + txt_studApplNo.Text + "'";
                else if (txt_studMblno.Text != "")
                    studSearch = " and a.Student_Mobile='" + txt_studMblno.Text + "'";
                SelQ = "";
                if (rdbtype.SelectedIndex != 2)
                {
                    if (rdbtype.SelectedIndex == 0)
                    {
                        strCondition = " and isnull(selection_status,'0')='0' and isnull(admission_status,'0')='0'";
                        strDate = " and date_applied between '" + fromdate + "' and '" + todate + "'";
                    }
                    else
                    {
                        strCondition = " and isnull(selection_status,'0')='1' and isnull(admission_status,'0')='0'";
                        strDate = " and AdmitedDate between '" + fromdate + "' and '" + todate + "'";
                    }
                    //SelQ = " select " + selCol + " from applyn a,stud_prev_details st where a.app_no=st.app_no  and isconfirm='1' " + strCondition + "";

                    //SelQ = " select " + selCol + " from applyn a,stud_prev_details st where  isconfirm='1' " + strCondition + "";
                    SelQ = " select " + selCol + " from applyn a,TextValTable t where a.seattype=t.TextCode and  isconfirm='1' " + strCondition + "";
                    if (cbdegree.Checked)
                        SelQ += " and a.degree_code in('" + degree + "')";
                    if (!string.IsNullOrEmpty(studSearch))
                        SelQ += studSearch;
                    else
                        SelQ += " and batch_year='" + batch + "' and a.college_code in ('" + collegecode + "')  and date_applied between '" + fromdate + "' and '" + todate + "' order by a.college_code desc";
                }
                else
                {
                    SelQ = " select distinct " + selCol + " from applyn a,registration r where r.app_no=a.app_no and isconfirm='1' and isnull(selection_status,'0')='1' and isnull(admission_status,'0')='1' ";//r.app_no=st.app_no and a.app_no=st.app_no and 
                    //SelQ = " select " + selCol + " from applyn a,stud_prev_details st,registration r where isconfirm='1' and isnull(selection_status,'0')='1' and isnull(admission_status,'0')='1' ";
                    if (cbdegree.Checked)
                        SelQ += " and a.degree_code in('" + degree + "')";
                    if (!string.IsNullOrEmpty(studSearch))
                        SelQ += studSearch;
                    else
                        SelQ += "and a.batch_year='" + batch + "' and a.college_code in ('" + collegecode + "')   and AdmitedDate between '" + fromdate + "' and '" + todate + "' order by a.college_code desc";
                }
                //  SelQ = " select a.stud_name,a.dob,a.app_formno,(select c.course_name+''+dt.dept_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( a.degree_code,0)) as degree_code,(select c.course_name+''+dt.dept_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( a.Alternativedegree_code,0)) as degree_code,(select TextVal from TextValtable where TExtCode=isnull(a.religion,0)) as religion,(select TextVal from TextValtable where TExtCode=isnull(a.community,0)) as community,a.student_Mobile,a.stuPer_id,(select Coll_acronymn from collinfo where college_code =isnull( a.college_code,0))as college_code,st.percentage,st.Cut_Of_Mark from applyn a,stud_prev_details st where a.app_no=st.app_no and batch_year='" + batch + "' and college_code in ('" + collegecode + "') and a.degree_code in('" + degree + "') and isconfirm='1' and isnull(selection_status,'0')='0' and isnull(admission_status,'0')='0' order by cut_of_Mark desc";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(SelQ, "Text");
                #endregion
            }
        }
        catch { }
        return dsload;
    }
    protected void getStudentDetails(DataSet ds)
    {
        try
        {
            DataSet dss = new DataSet();
            string selColumn = string.Empty;

            string linkname1 = Convert.ToString(ddlMainreport.SelectedItem.Text);
            string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code='" + cblclg.SelectedItem.Value + "' and user_code='" + usercode + "' ";
            dss.Clear();
            dss = d2.select_method_wo_parameter(selcol1, "Text");
            if (dss.Tables.Count > 0)
            {
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int c = 0; c < dss.Tables[0].Rows.Count; c++)
                    {
                        string value = Convert.ToString(dss.Tables[0].Rows[c]["LinkValue"]);
                        if (value != "")
                        {
                            string[] valuesplit = value.Split(',');
                            if (valuesplit.Length > 0)
                            {
                                for (int k = 0; k < valuesplit.Length; k++)
                                {
                                    colval = Convert.ToString(valuesplit[k]);
                                    string colName = loadtext(1);
                                    string oldName = Convert.ToString(dss.Tables[0].Columns[c].ColumnName);
                                    ds.Tables[0].Columns[k + 3].ColumnName = colName;
                                    colval = Convert.ToString(valuesplit[k]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Div2.Visible = true;
                    LblAlertMsg.Text = "No Records Found";
                    return;
                }
            }
            else
            {
                Div2.Visible = true;
                LblAlertMsg.Text = "Set Column Order";
            }

            //Hashtable htRealName = htcolumnHeaderValue(); htColVal
            //for (int row = 0; row < ds.Tables[0].Columns.Count - 4; row++)
            //{
            //    string oldName = Convert.ToString(ds.Tables[0].Columns[row].ColumnName);
            //    string viewcolName = Convert.ToString(htColVal[oldName.Trim()]);
            //    ds.Tables[0].Columns[row].ColumnName = viewcolName;
            //}
            if (ds.Tables[0].Rows.Count > 0)
            {
                gridstud.DataSource = ds;
                gridstud.DataBind();
                gridstud.Visible = true;
                Session["grid"] = ds;
                pnlContents.Visible = true;
                buttonHide();
                printCollegeDet();
                // gridAlignment();
            }
        }
        catch { }
    }
    protected void gridstud_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox cbAll = new CheckBox();
            e.Row.Cells[1].Controls.Add(cbAll);
            // e.Row.CssClass = "header";
            // cbAll.AutoPostBack = true;
            cbAll.Attributes.Add("onchange", "return OnGridHeaderSelected();");
            e.Row.Cells[2].Text = "View";
            int NumCells = e.Row.Cells.Count;
            for (int i = 0; i < NumCells - 1; i++)
            {
                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
            }
            //visible last 3 columns
            e.Row.Cells[e.Row.Cells.Count - 3].Visible = false;
            e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //  e.Row.Cells[0].Text = Convert.ToString(e.Row.Cells.Count);
            e.Row.Cells[0].Text = "" + ((((GridView)sender).PageIndex * ((GridView)sender).PageSize) + (e.Row.RowIndex + 1));
            CheckBox cb = new CheckBox();
            cb.ID = "cb";
            e.Row.Cells[1].Controls.Add(cb);
            //view button add
            Button btnview = new Button();
            btnview.ID = "btnview";
            btnview.Text = "View";
            btnview.Click += new EventHandler(btnview_Click);
            e.Row.Cells[2].Controls.Add(btnview);
            //visible last 3 columns
            e.Row.Cells[e.Row.Cells.Count - 3].Visible = false;
            e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }
    protected void gridstud_OnDataBound(object sender, EventArgs e)
    {
        for (int i = gridstud.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = gridstud.Rows[i];
            for (int j = 0; j <= gridstud.Columns.Count; j++)
            {
                string strtext = gridstud.Columns[j].HeaderText;
                if (strtext == "SNo")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                if (strtext == "Select")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                if (strtext == "Student Name")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Left;
                if (strtext == "DOB")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                if (strtext == "Application ID")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                if (strtext == "Application Date")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                if (strtext == "Department")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Left;
                if (strtext == "Alternative Course")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Left;
                if (strtext == "Religion")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Left;
                if (strtext == "Community")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Left;
                if (strtext == "Mobile No")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                if (strtext == "Email Id")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Left;
                if (strtext == "Institute Name")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Left;
                if (strtext == "Percentage")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                if (strtext == "Cut of Mark")
                    row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
    protected void gridAlignment()
    {
        //foreach (GridViewRow row in gridstud.Rows)
        //{
        //    string strtext = gridstud.Rows[row].Cells[0].Text;
        //    foreach (TableCell cell in row.Cells)
        //    {
        //        cell.Attributes.CssStyle["text-align"] = "center";
        //    }
        //}
    }
    protected Hashtable getDeptName()
    {
        Hashtable htdtName = new Hashtable();
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string SelQ = " select distinct d.degree_code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from degree d,department dt,course c where c.course_id=d.course_id and d.dept_code=dt.dept_code and d.college_code in('" + collegecode + "')";
            DataSet dsdeg = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsdeg.Tables.Count > 0 && dsdeg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsdeg.Tables[0].Rows.Count; row++)
                {
                    if (!htdtName.ContainsKey(Convert.ToString(dsdeg.Tables[0].Rows[row]["degree_code"])))
                        htdtName.Add(Convert.ToString(dsdeg.Tables[0].Rows[row]["degree_code"]), Convert.ToString(dsdeg.Tables[0].Rows[row]["degreename"]));
                }
            }
        }
        catch { }
        return htdtName;
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        string groupStr = string.Empty;
        string selColumn = loadlcolumns();

        ds.Clear();
        ds = loadDetails(selColumn);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            getStudentDetails(ds);
        }
        else
        {
            gridstud.Visible = false;
            buttonview.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        }
    }
    protected void UserbasedRights()
    {
        string userrht = d2.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and user_code='" + usercode + "'");
        if (userrht == "1")
            usBasedRights = true;
        else
            usBasedRights = false;
    }
    #region Common Checkbox and Checkboxlist Event
    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private string getCblSelectedTextwithout(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("," + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }
    #endregion

    #region colorder Commented by saranya
    //protected void lnkcolorder_Click(object sender, EventArgs e)
    //{
    //    txtcolorder.Text = string.Empty;
    //    loadcolumnorder();
    //    columnType();
    //    // loadcolumns();
    //    divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
    //    //divcolorder.Visible = true;
    //}
    //public void loadcolumnorder()
    //{
    //    cblcolumnorder.Items.Clear();
    //    cblcolumnorder.Items.Add(new ListItem("Student Name", "1"));
    //    cblcolumnorder.Items.Add(new ListItem("DOB", "2"));
    //    cblcolumnorder.Items.Add(new ListItem("Application ID", "3"));
    //    cblcolumnorder.Items.Add(new ListItem("Application Date", "4"));
    //    cblcolumnorder.Items.Add(new ListItem("Department", "5"));
    //    cblcolumnorder.Items.Add(new ListItem("Alternative Course", "6"));
    //    cblcolumnorder.Items.Add(new ListItem("Religion", "7"));
    //    cblcolumnorder.Items.Add(new ListItem("Community", "8"));
    //    cblcolumnorder.Items.Add(new ListItem("Mobile No", "9"));
    //    cblcolumnorder.Items.Add(new ListItem("Email Id", "10"));
    //    cblcolumnorder.Items.Add(new ListItem("Institute Name", "11"));
    //    //cblcolumnorder.Items.Add(new ListItem("Percentage", "12"));
    //    //cblcolumnorder.Items.Add(new ListItem("Cut of Mark", "13"));
    //    cblcolumnorder.Items.Add(new ListItem("Father Name", "14"));
    //    cblcolumnorder.Items.Add(new ListItem("Father Mobile No", "15"));
    //    cblcolumnorder.Items.Add(new ListItem("Semester", "16"));
    //    cblcolumnorder.Items.Add(new ListItem("Batch Year", "17"));
    //    cblcolumnorder.Items.Add(new ListItem("seattype", "18"));
    //    if (rdbtype.SelectedItem.Value == "2")
    //    {
    //        cblcolumnorder.Items.Add(new ListItem("Admission date", "19"));//abarna
    //    }
    //}
    //protected Hashtable htcolumnValue()
    //{
    //    Hashtable htcol = new Hashtable();
    //    try
    //    {
    //        htcol.Add("Student Name", "a.stud_name");
    //        htcol.Add("DOB", "convert(varchar(10),a.dob,103) as dob");
    //        htcol.Add("Application ID", "a.app_formno");
    //        htcol.Add("Application Date", "convert(varchar(10),a.date_applied,103) as date_applied");
    //        htcol.Add("Department", "(select c.course_name+'-'+dt.dept_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( a.degree_code,0)) as degree_code");
    //        htcol.Add("Alternative Course", "(select c.course_name+'-'+dt.dept_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( a.Alternativedegree_code,0)) as alterdegree_code");
    //        htcol.Add("Religion", "(select TextVal from TextValtable where TExtCode=isnull(a.religion,0)) as Religion");
    //        htcol.Add("Community", "(select TextVal from TextValtable where TExtCode=isnull(a.community,0)) as Community");
    //        htcol.Add("Mobile No", "a.student_Mobile");
    //        htcol.Add("Email Id", "a.stuPer_id");
    //        htcol.Add("Institute Name", "(select Coll_acronymn from collinfo where college_code =isnull( a.college_code,0))as collegecode");

    //        htcol.Add("Father Name", "a.parent_name");
    //        htcol.Add("Father Mobile No", "a.parentf_mobile");
    //        htcol.Add("Semester", "isnull(a.current_semester,0) as current_semester");//delsi2702
    //        htcol.Add("Batch Year", "a.batch_year");
    //        htcol.Add("seattype", "(select TextVal from TextValtable where TExtCode=isnull(a.seattype,0)) as SeatType ");
    //        if (rdbtype.SelectedItem.Value == "2")
    //        {
    //            htcol.Add("Admission date", "convert(varchar(10),r.adm_date,103) as admissiondate");//abarna
    //        }
    //        //htcol.Add("Referred By", "a.batch_year");
    //        //
    //        //htcol.Add("Cut of Mark", "st.Cut_Of_Mark");
    //        //htcol.Add("Percentage", "st.percentage");//krishhna kumar.r 09.05.2018
    //        //htcol.Add("Cut of Mark", "st.Cut_Of_Mark");krishhna kumar.r 09.05.2018
    //    }
    //    catch { }
    //    return htcol;
    //}
    //protected Hashtable htcolumnHeaderValue()
    //{
    //    Hashtable htcol = new Hashtable();
    //    try
    //    {
    //        htcol.Add("sno", "SNo");
    //        htcol.Add("sel", "Select");
    //        htcol.Add("stview", "View");
    //        htcol.Add("stud_name", "Student Name");
    //        htcol.Add("dob", "DOB");
    //        htcol.Add("app_formno", "Application ID");
    //        htcol.Add("date_applied", "Application Date");
    //        htcol.Add("degree_code", "Department");
    //        htcol.Add("alterdegree_code", "Alternative Course");
    //        htcol.Add("Religion", "Religion");
    //        htcol.Add("Community", "Community");
    //        htcol.Add("student_Mobile", "Mobile No");
    //        htcol.Add("stuPer_id", "Email Id");
    //        htcol.Add("collegecode", "Institute Name");
    //        //htcol.Add("percentage", "Percentage");krishhna kumar.r
    //        //htcol.Add("Cut_Of_Mark", "Cut of Mark");krishhna kumar.r
    //        htcol.Add("roll_admit", "Admission No");
    //        htcol.Add("parent_name", "Father Name");
    //        htcol.Add("parentf_mobile", "Father Mobile No");
    //        htcol.Add("current_semester", "Semester");
    //        htcol.Add("batch_year", "Batch Year");
    //        htcol.Add("seattype", "seattype");//krishhna kumar.r
    //        htcol.Add("SeatType", "SeatType");//krishhna kumar.r
    //        if (rdbtype.SelectedItem.Value == "2")
    //        {
    //            htcol.Add("adm_date", "Admission date");//abarna
    //        }

    //    }
    //    catch { }
    //    return htcol;
    //}
    //protected void btncolorderOK_Click(object sender, EventArgs e)
    //{
    //    // loadcolumns();
    //    divcolorder.Visible = true;
    //    if (getsaveColumnOrder())
    //    {
    //        divcolorder.Attributes.Add("Style", "display:none;");
    //    }
    //}
    //protected bool getsaveColumnOrder()
    //{
    //    bool boolSave = false;
    //    try
    //    {
    //        string strText = string.Empty;
    //        if (cblcolumnorder.Items.Count > 0)
    //            strText = Convert.ToString(getCblSelectedTextwithout(cblcolumnorder));
    //        if (!string.IsNullOrEmpty(strText))
    //            strText = Convert.ToString(txtcolorder.Text);
    //        string Usercollegecode = string.Empty;
    //        if (Session["collegecode"] != null)
    //            Usercollegecode = Convert.ToString(Session["collegecode"]);
    //        string linkName = string.Empty;
    //        if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
    //            linkName = Convert.ToString(ddlreport.SelectedItem.Text);
    //        if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0" && !string.IsNullOrEmpty(strText))
    //        {
    //            string SelQ = " if exists (select * from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "')update New_InsSettings set linkvalue='" + strText + "' where  LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "' else insert into New_InsSettings(LinkName,linkvalue,user_code,college_code) values('" + linkName + "','" + strText + "','" + usercode + "','" + Usercollegecode + "')";
    //            int insQ = d2.update_method_wo_parameter(SelQ, "Text");
    //            boolSave = true;
    //        }
    //        if (!boolSave)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please select corresponding values!')", true);
    //        }
    //    }
    //    catch { }
    //    return boolSave;
    //}
    //public bool columncount()
    //{
    //    bool colorder = false;
    //    try
    //    {
    //        for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //        {
    //            if (cblcolumnorder.Items[i].Selected == true)
    //            {
    //                colorder = true;
    //            }
    //        }
    //    }
    //    catch { }
    //    return colorder;
    //}
    //public void loadcolumns()
    //{
    //    try
    //    {
    //        string linkname = "DFCR column order settings";
    //        string columnvalue = "";
    //        int clsupdate = 0;
    //        DataSet dscol = new DataSet();
    //        string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
    //        dscol.Clear();
    //        dscol = d2.select_method_wo_parameter(selcol, "Text");
    //        if (columncount() == true)
    //        {
    //            if (cblcolumnorder.Items.Count > 0)
    //            {
    //                colord.Clear();
    //                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //                {
    //                    if (cblcolumnorder.Items[i].Selected == true)
    //                    {
    //                        colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
    //                        if (columnvalue == "")
    //                            columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
    //                        else
    //                            columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
    //                    }
    //                }
    //            }
    //        }
    //        else if (dscol.Tables.Count > 0)
    //        {
    //            if (dscol.Tables[0].Rows.Count > 0)
    //            {
    //                colord.Clear();
    //                for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
    //                {
    //                    string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
    //                    string[] valuesplit = value.Split(',');
    //                    if (valuesplit.Length > 0)
    //                    {
    //                        for (int k = 0; k < valuesplit.Length; k++)
    //                        {
    //                            colord.Add(Convert.ToString(valuesplit[k]));
    //                            if (columnvalue == "")
    //                                columnvalue = Convert.ToString(valuesplit[k]);
    //                            else
    //                                columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            colord.Clear();
    //            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //            {
    //                cblcolumnorder.Items[i].Selected = true;
    //                colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
    //                if (columnvalue == "")
    //                    columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
    //                else
    //                    columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
    //            }
    //        }
    //        if (columnvalue != "" && columnvalue != null)
    //        {
    //            string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode + "' else insert into New_InsSettings (LinkName,LinkValue,usercode,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode + "')";
    //            clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
    //        }
    //        if (clsupdate == 1)
    //        {
    //            string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
    //            DataSet dscolor = new DataSet();
    //            dscolor.Clear();
    //            dscolor = d2.select_method_wo_parameter(sel, "Text");
    //            if (dscolor.Tables.Count > 0)
    //            {
    //                int count = 0;
    //                if (dscolor.Tables[0].Rows.Count > 0)
    //                {
    //                    string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
    //                    string[] value1 = value.Split(',');
    //                    if (value1.Length > 0)
    //                    {
    //                        for (int i = 0; i < value1.Length; i++)
    //                        {
    //                            string val = value1[i].ToString();
    //                            for (int k = 0; k < cblcolumnorder.Items.Count; k++)
    //                            {
    //                                if (val == cblcolumnorder.Items[k].Value)
    //                                {
    //                                    cblcolumnorder.Items[k].Selected = true;
    //                                    count++;
    //                                }
    //                                if (count == cblcolumnorder.Items.Count)
    //                                    cb_column.Checked = true;
    //                                else
    //                                    cb_column.Checked = false;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}

    ////protected void btnAdd_OnClick(object sender, EventArgs e)
    ////{
    ////}
    //protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
    //    selectReportType();
    //}
    //protected void btnDel_OnClick(object sender, EventArgs e)
    //{
    //    deleteReportType();
    //}
    ////type save
    //protected void btnaddtype_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string Usercollegecode = string.Empty;
    //        if (Session["collegecode"] != null)
    //            Usercollegecode = Convert.ToString(Session["collegecode"]);
    //        string strDesc = Convert.ToString(txtdesc.Text);
    //        if (!string.IsNullOrEmpty(strDesc) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
    //        {
    //            string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + strDesc + "' and MasterCriteria ='AdmissionReportDetails' and CollegeCode ='" + Usercollegecode + "') update CO_MasterValues set MasterValue ='" + strDesc + "' where MasterValue ='" + strDesc + "' and MasterCriteria ='AdmissionReportDetails' and CollegeCode ='" + Usercollegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + strDesc + "','AdmissionReportDetails','" + Usercollegecode + "')";
    //            int insert = d2.update_method_wo_parameter(sql, "Text");
    //            if (insert > 0)
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true); txtdesc.Text = string.Empty;
    //            }
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter report type')", true);
    //        }
    //        columnType();
    //        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
    //    }
    //    catch { }
    //}
    //public void columnType()
    //{
    //    string Usercollegecode = string.Empty;
    //    if (Session["collegecode"] != null)
    //        Usercollegecode = Convert.ToString(Session["collegecode"]);
    //    ddlreport.Items.Clear();
    //    ddlMainreport.Items.Clear();
    //    if (!string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
    //    {
    //        string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='AdmissionReportDetails' and CollegeCode='" + Usercollegecode + "'";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(query, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlreport.DataSource = ds;
    //            ddlreport.DataTextField = "MasterValue";
    //            ddlreport.DataValueField = "MasterCode";
    //            ddlreport.DataBind();
    //            ddlreport.Items.Insert(0, new ListItem("Select", "0"));
    //            //main search filter
    //            ddlMainreport.DataSource = ds;
    //            ddlMainreport.DataTextField = "MasterValue";
    //            ddlMainreport.DataValueField = "MasterCode";
    //            ddlMainreport.DataBind();
    //            // ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
    //        }
    //        else
    //        {
    //            ddlreport.Items.Insert(0, new ListItem("Select", "0"));
    //            ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
    //        }
    //    }
    //}
    //protected void selectReportType()
    //{
    //    try
    //    {
    //        bool boolcheck = false;
    //        string getName = string.Empty;
    //        txtcolorder.Text = string.Empty;
    //        string strText = string.Empty;
    //        string Usercollegecode = string.Empty;
    //        if (Session["collegecode"] != null)
    //            Usercollegecode = Convert.ToString(Session["collegecode"]);
    //        string linkName = string.Empty;
    //        if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
    //            linkName = Convert.ToString(ddlreport.SelectedItem.Text);
    //        if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
    //        {
    //            getName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' ");
    //            if (!string.IsNullOrEmpty(getName) && getName != "0")
    //            {
    //                string[] splName = getName.Split(',');
    //                if (splName.Length > 0)
    //                {
    //                    for (int sprow = 0; sprow < splName.Length; sprow++)
    //                    {
    //                        for (int flt = 0; flt < cblcolumnorder.Items.Count; flt++)
    //                        {
    //                            if (splName[sprow].Trim() == cblcolumnorder.Items[flt].Text.Trim())
    //                            {
    //                                cblcolumnorder.Items[flt].Selected = true;
    //                                boolcheck = true;
    //                                // strText += cblcolumnorder.Items[flt].Text;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            txtcolorder.Text = string.Empty;
    //            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //            {
    //                cblcolumnorder.Items[i].Selected = false;
    //            }
    //            cb_column.Checked = false;
    //        }
    //        if (boolcheck)
    //        {
    //            txtcolorder.Text = getName;
    //        }
    //    }
    //    catch { }
    //}
    //protected void deleteReportType()
    //{
    //    int delMQ = 0;
    //    string Usercollegecode = string.Empty;
    //    if (Session["collegecode"] != null)
    //        Usercollegecode = Convert.ToString(Session["collegecode"]);
    //    string linkName = string.Empty;
    //    if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
    //        linkName = Convert.ToString(ddlreport.SelectedItem.Text);
    //    if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
    //    {
    //        int delQ = 0;
    //        int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "'", "Text")), out delQ);
    //        int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete  from CO_MasterValues where MasterCriteria='AdmissionReportDetails' and mastervalue='" + linkName + "'  and collegecode='" + Usercollegecode + "'", "Text")), out delMQ);
    //    }
    //    if (delMQ > 0)
    //    {
    //        txtcolorder.Text = string.Empty;
    //        for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //        {
    //            cblcolumnorder.Items[i].Selected = false;
    //        }
    //        cb_column.Checked = false;
    //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
    //    }
    //    else
    //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Failed')", true);
    //    columnType();
    //    divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
    //}
    #endregion


    protected string getheadername()
    {
        string selQ = string.Empty;
        try
        {
            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(Session["collegecode"]);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
            }
        }
        catch { }
        return selQ;
    }
    //protected string getSelectedColumn()
    //{
    //    string val = string.Empty;
    //    try
    //    {
    //        StringBuilder strCol = new StringBuilder();
    //        StringBuilder grpstrCol = new StringBuilder();
    //        Hashtable htcolumn = htcolumnValue();
    //        string Usercollegecode = string.Empty;
    //        if (Session["collegecode"] != null)
    //            Usercollegecode = Convert.ToString(Session["collegecode"]);
    //        string linkName = string.Empty;
    //        if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
    //            linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
    //        if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
    //        {
    //            string selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
    //            if (!string.IsNullOrEmpty(selQ) && selQ != "0")
    //            {
    //                string[] splVal = selQ.Split(',');
    //                if (splVal.Length > 0)
    //                {
    //                    for (int row = 0; row < splVal.Length; row++)
    //                    {
    //                        string tempSel = Convert.ToString(htcolumn[splVal[row].Trim()]);
    //                        if (rdbtype.Items.Count > 0 && rdbtype.SelectedIndex == 2 && tempSel.Trim() == "a.app_formno")
    //                            tempSel = "r.roll_admit";
    //                        strCol.Append(tempSel + ",");
    //                        //if (tempSel != "sum(debit) as debit" && tempSel != "sum(credit) as credit")
    //                        //{
    //                        //    if (tempSel == "convert(varchar(10),transdate,103)as transdate")
    //                        //        tempSel = "transdate";
    //                        //    grpstrCol.Append(tempSel + ",");
    //                        //}
    //                    }
    //                }
    //            }
    //            if (strCol.Length > 0)//&& grpstrCol.Length > 0
    //            {
    //                strCol.Remove(strCol.Length - 1, 1);
    //                val = Convert.ToString(strCol);
    //                //grpstrCol.Remove(grpstrCol.Length - 1, 1);
    //                //groupStr = Convert.ToString(grpstrCol);
    //            }
    //        }
    //    }
    //    catch { }
    //    return val;
    //}
    //student view list 15.05.2017
    protected void btnview_Click(object sender, EventArgs e)
    {
        try
        {
            string app_no = string.Empty;
            string type = string.Empty;
            string grduation = string.Empty;
            string course = string.Empty;
            string edulevel = string.Empty;
            int rowIndex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
            string collegecode = string.Empty;
            string degreeCode = string.Empty;
            app_no = gridstud.Rows[rowIndex].Cells[gridstud.Rows[rowIndex].Cells.Count - 3].Text;
            collegecode = gridstud.Rows[rowIndex].Cells[gridstud.Rows[rowIndex].Cells.Count - 2].Text;
            degreeCode = gridstud.Rows[rowIndex].Cells[gridstud.Rows[rowIndex].Cells.Count - 1].Text;

            //barath 19.05.17 
            SettingReceipt();
            ddlAdmissionStudType_IndexChange(sender, e);
            if (rdbtype.SelectedIndex == 0 || rdbtype.SelectedIndex == 1)
                cbpersonal.Checked = true;
            cbpersonal_Changed(sender, e);
            if (rdbtype.SelectedIndex == 2)
                bindFeeLedgerGrid(app_no, collegecode, degreeCode);
            if (isFinanceLink())
                lbRcpt.Visible = true;
            else
                lbRcpt.Visible = false;
            getdeptDetails(collegecode, degreeCode, ref  type, ref  grduation, ref  course, ref  edulevel);
            Session["pdfapp_no"] = Convert.ToString(app_no);
            Session["studclgcode"] = collegecode;
            Session["studdegcode"] = degreeCode;

            #region Certificate view details
            string certificate = " select Certificate_Name as [Certificate Name],FileName from Stud_Certificate_Det where  app_no='" + app_no + "'";
            certificate += "  select 'Community Certificate' as [Community Certificate]  ,'Community Certificate' as FileName, communitycertificate  from StdPhoto where app_no='" + app_no + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(certificate, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                certificate_grid.DataSource = ds.Tables[0];
                certificate_grid.DataBind();
                certificate_detdownload.Visible = true;
            }
            else
            {
                certificate_grid.DataBind();
                certificate_detdownload.Visible = false;
            }
            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                communitity_grid.DataSource = ds.Tables[1];
                communitity_grid.DataBind();
                certificate_detdownload.Visible = true;
            }
            else
            {
                communitity_grid.DataBind();
                certificate_detdownload.Visible = false;
            }
            #endregion

            if (Convert.ToString(ViewState["applicationviewformatset"]) == "0")
            {
                if (edulevel.ToString().ToUpper() == "PG")
                {
                    pgdiv_verification.Visible = true;
                    ugdiv_verification.Visible = false;
                }
                else if (edulevel.ToString().ToUpper() == "UG")
                {
                    pgdiv_verification.Visible = false;
                    ugdiv_verification.Visible = true;
                }
                #region Commonformat
                //string query = "select app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,a.degree_code,batch_year,a.college_code,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet,a.degree_code,c.Course_Name ,Alternativedegree_code,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.subcaste) and TextCriteria='scast') SubCaste,case when Dalits='1' then 'Yes' when Dalits='0' then 'No' end Dalits,Parish_name,dt.Dept_Name  from applyn a,degree d,Department dt,Course c where  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.degree_code=d.Degree_Code and  a.app_no='" + app_no + "'";

                //krishhna kumar.r
                string query = "select app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.Cityc) and TextCriteria='city')as Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.Cityc) and TextCriteria='city')as cityp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,a.degree_code,batch_year,a.college_code,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet,a.degree_code,c.Course_Name ,Alternativedegree_code,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.subcaste) and TextCriteria='scast') SubCaste,case when Dalits='1' then 'Yes' when Dalits='0' then 'No' end Dalits,Parish_name,dt.Dept_Name  from applyn a,degree d,Department dt,Course c where  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.degree_code=d.Degree_Code and  a.app_no='" + app_no + "'";//,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.seattype) and TextCriteria='seat') as seattype

                query = query + " select course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark,tancetmark_year,Cut_Of_Mark,PCM_Percentage from Stud_prev_details where app_no ='" + app_no + "'";
                //query = query + " select Certificate_Name as [Certificate Name],FileName from Stud_Certificate_Det where  app_no='" + app_no + "'";
                //query = query + "  select 'Community Certificate' as [Community Certificate]  ,'Community Certificate' as FileName, communitycertificate  from StdPhoto where app_no='" + app_no + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblCurSemDet.Text = Convert.ToString(ds.Tables[0].Rows[0]["current_semester"]);//barath 18.05.17
                    lblBatchDet.Text = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                    txt_OldBatch.Text = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                    Session["OldDegCode"] = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]);
                    Session["OldSeatType"] = Convert.ToString(ds.Tables[0].Rows[0]["seattype"]);
                    DataTable DegName = DirAccess.selectDataTable("select c.Course_Name+ '-' + dt.Dept_Name as Dept_Name,(select TextVal from TextValTable where TextCode=seattype) as SeatType from applyn a,Degree d,Department dt,Course c where c.Course_Id=d.Course_Id and a.degree_code=d.Degree_Code and dt.Dept_Code=d.Dept_Code and a.degree_code='" + Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]) + "' and app_no='" + app_no + "'");
                    if (DegName.Rows.Count > 0)
                    {
                        txt_OldDegree.Text = Convert.ToString(DegName.Rows[0]["Dept_Name"]).Trim();
                        txt_OldSeattype.Text = Convert.ToString(DegName.Rows[0]["seattype"]).Trim();
                    }
                    txt_OldApplNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["app_formno"]);
                    stud_img.ImageUrl = "~/Handler/Handler3.ashx?id=" + app_no;
                    college_span.InnerHtml = ":  " + Convert.ToString(type);
                    degree_Span.InnerHtml = ":  " + Convert.ToString(edulevel);
                    graduation_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);
                    //Convert.ToString(d2.GetFunction("select c.Course_Name from degree d,Department dt,Course c  where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  degree_code=" + Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]) + ""));
                    //course_span.InnerHtml = ":  " + Convert.ToString(d2.GetFunction("select dt.Dept_Name from degree d,Department dt,Course c  where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  degree_code=" + Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]) + ""));//barath 19.05.17
                    course_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);
                    string alternatedegree = "0";
                    if (Convert.ToString(ds.Tables[0].Rows[0]["Alternativedegree_code"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["Alternativedegree_code"]).Trim() != "0")
                        alternatedegree = Convert.ToString(d2.GetFunction("select dt.Dept_Name from degree d,Department dt,Course c  where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  degree_code=" + Convert.ToString(ds.Tables[0].Rows[0]["Alternativedegree_code"]) + ""));
                    course_span2.InnerHtml = alternatedegree == "0" ? " :  - " : ":  " + alternatedegree;
                    applicantname_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]);
                    applicantno_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["app_formno"]);
                    dob_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["dob"]);
                    if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "0")
                    {
                        gender_span.InnerHtml = ":  Male";
                    }
                    else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "1")
                    {
                        gender_span.InnerHtml = ":  Female";
                    }
                    else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "2")
                    {
                        gender_span.InnerHtml = ":  Transgender";
                    }
                    parent_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]);
                    string occupation = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["parent_occu"]));
                    occupation_span.InnerHtml = ":  " + occupation.ToString();
                    string mothertonge = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["mother_tongue"]));
                    mothertongue_span.InnerHtml = ":  " + Convert.ToString(mothertonge);
                    string relisgion = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["religion"]));
                    string subcaste = Convert.ToString(ds.Tables[0].Rows[0]["SubCaste"]);
                    string Dalits = Convert.ToString(ds.Tables[0].Rows[0]["Dalits"]);
                    string Parish_name = Convert.ToString(ds.Tables[0].Rows[0]["Parish_name"]);
                    string subcasteval = "";
                    if (subcaste.ToUpper() == "ROMAN CATHOLIC")
                    {
                        subcasteval = " Dalits :" + Dalits + " <br/></t> Name of the Parish : " + Parish_name;
                    }
                    if (subcasteval.Trim() != "")
                        relisgion = relisgion + " " + subcasteval;

                    religion_span.InnerHtml = ":  " + Convert.ToString(relisgion);
                    string city = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["citizen"]));
                    nationality_span.InnerHtml = ":  " + Convert.ToString(city);
                    string coummnity = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["community"]));
                    commuity_span.InnerHtml = ":  " + Convert.ToString(coummnity);
                    if (Convert.ToString(ds.Tables[0].Rows[0]["caste"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["caste"]) != "0")
                    {
                        string scas = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["caste"]));
                        Caste_span.InnerHtml = ":  " + Convert.ToString(scas);
                    }
                    else
                    {
                        Caste_span.InnerHtml = ":  -";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["TamilOrginFromAndaman"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["TamilOrginFromAndaman"]) != "False")
                    {
                        tamilorigin_span.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        tamilorigin_span.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]) != "False")
                    {
                        Ex_service_span.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        Ex_service_span.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["isdisable"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["isdisable"]) != "False")
                    {
                        Differentlyable_Span.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        Differentlyable_Span.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["first_graduate"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["first_graduate"]) != "False")
                    {
                        first_generation_Span.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        first_generation_Span.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["CampusReq"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["CampusReq"]) != "False")
                    {
                        residancerequired_span.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        residancerequired_span.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["DistinctSport"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["DistinctSport"]) != "0")
                    {
                        string disy = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["DistinctSport"]));
                        sport_span.InnerHtml = ":  " + Convert.ToString(disy);
                    }
                    else
                    {
                        sport_span.InnerHtml = ":  -";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["co_curricular"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["co_curricular"]) != "0")
                    {
                        string cocour = subjectcode(Convert.ToString(ds.Tables[0].Rows[0]["co_curricular"]));
                        Co_Curricular_span.InnerHtml = ":  " + Convert.ToString(cocour);
                    }
                    else
                    {
                        Co_Curricular_span.InnerHtml = ":  -";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["ncccadet"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["ncccadet"]) != "False")
                    {
                        ncccadetspan.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        ncccadetspan.InnerHtml = ":  No";
                    }
                    if (Convert.ToString(ds.Tables[1].Rows[0]["Vocational_stream"]).Trim() != "" && Convert.ToString(ds.Tables[1].Rows[0]["Vocational_stream"]) != "False")
                    {
                        Vocationalspan.InnerHtml = ":  Yes";
                    }
                    else
                    {
                        Vocationalspan.InnerHtml = ":  No";
                    }
                    caddressline1_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_addressC"]);
                    string address = Convert.ToString(ds.Tables[0].Rows[0]["Streetc"]);
                    if (ds.Tables[0].Rows[0]["Streetc"].ToString().Trim() != "")
                    {
                        string[] split = address.Split('/');
                        if (split.Length > 1)
                        {
                            if (Convert.ToString(split[0]).Trim() != "")
                            {
                                Addressline2_span.InnerHtml = ":  " + Convert.ToString(split[0]);
                            }
                            else
                            {
                                Addressline2_span.InnerHtml = ":  -";
                            }
                            if (Convert.ToString(split[1]).Trim() != "")
                            {
                                Addressline3_span.InnerHtml = ":  " + Convert.ToString(split[1]);
                            }
                            else
                            {
                                Addressline3_span.InnerHtml = ":  -";
                            }
                        }
                        else
                        {
                            Addressline2_span.InnerHtml = ":  " + Convert.ToString(split[0]);
                        }
                    }
                    else
                    {
                        Addressline2_span.InnerHtml = ":  -";
                        Addressline3_span.InnerHtml = ":  -";
                    }
                    if (ds.Tables[0].Rows[0]["Cityc"].ToString().Trim() != "")
                    {
                        city_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Cityc"]);
                    }


                    else
                    {
                        city_span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["parent_statec"].ToString().Trim() != "")
                    {
                        string state = subjectcode(ds.Tables[0].Rows[0]["parent_statec"].ToString());
                        state_span.InnerHtml = ":  " + Convert.ToString(state);
                    }
                    else
                    {
                        state_span.InnerHtml = ":  -";
                    }
                    if (ds.Tables[0].Rows[0]["Countryc"].ToString().Trim() != "")
                    {
                        string country = subjectcode(ds.Tables[0].Rows[0]["Countryc"].ToString());
                        Country_span.InnerHtml = ":  " + Convert.ToString(country);
                    }
                    else
                    {
                        Country_span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["parent_pincodec"].ToString().Trim() != "")
                    {
                        Postelcode_Span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodec"]);
                    }
                    else
                    {
                        Postelcode_Span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["Student_Mobile"].ToString().Trim() != "")
                    {
                        Mobilenumber_Span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]);
                    }
                    else
                    {
                        Mobilenumber_Span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["alter_mobileno"].ToString().Trim() != "")
                    {
                        Alternatephone_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["alter_mobileno"]);
                    }
                    else
                    {
                        Alternatephone_span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["StuPer_Id"].ToString().Trim() != "")
                    {
                        emailid_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]);
                    }
                    else
                    {
                        emailid_span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["parent_phnoc"].ToString().Trim() != "")
                    {
                        std_ist_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_phnoc"]);
                    }
                    else
                    {
                        std_ist_span.InnerHtml = "-";
                    }
                    // permnant
                    paddressline1_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]);
                    if (ds.Tables[0].Rows[0]["Streetp"].ToString().Trim() != "")
                    {
                        string streat = Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]);
                        if (streat.Trim() != "")
                        {
                            string[] splitstreat = streat.Split('/');
                            if (splitstreat.Length > 1)
                            {
                                if (Convert.ToString(splitstreat[0]).Trim() != "")
                                {
                                    paddressline2_span.InnerHtml = ":  " + Convert.ToString(splitstreat[0]);
                                }
                                else
                                {
                                    paddressline2_span.InnerHtml = ":  -";
                                }
                                if (Convert.ToString(splitstreat[0]).Trim() != "")
                                {
                                    paddressline3_span.InnerHtml = ":  " + Convert.ToString(splitstreat[1]);
                                }
                                else
                                {
                                    paddressline3_span.InnerHtml = ":  -";
                                }
                            }
                            else
                            {
                                paddressline2_span.InnerHtml = ":  " + Convert.ToString(splitstreat[0]);
                            }
                        }
                        paddressline2_span.InnerHtml = ":  -";
                        paddressline3_span.InnerHtml = ":  -";
                    }
                    else
                    {
                        paddressline2_span.InnerHtml = ":  -";
                        paddressline3_span.InnerHtml = ":  -";
                    }
                    if (ds.Tables[0].Rows[0]["Cityp"].ToString().Trim() != "")
                    {
                        pcity_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Cityp"]);
                    }
                    else
                    {
                        pcity_span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["parent_statep"].ToString().Trim() != "")
                    {
                        string state = subjectcode(ds.Tables[0].Rows[0]["parent_statep"].ToString());
                        pstate_span.InnerHtml = ":  " + Convert.ToString(state);
                    }
                    else
                    {
                        pstate_span.InnerHtml = ":  -";
                    }
                    if (ds.Tables[0].Rows[0]["Countryp"].ToString().Trim() != "")
                    {
                        string country = subjectcode(ds.Tables[0].Rows[0]["Countryp"].ToString());
                        pcountry_span.InnerHtml = ":  " + Convert.ToString(country);
                    }
                    else
                    {
                        pcountry_span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["parent_pincodep"].ToString().Trim() != "")
                    {
                        ppostelcode_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]);
                    }
                    else
                    {
                        ppostelcode_span.InnerHtml = "-";
                    }
                    if (ds.Tables[0].Rows[0]["parent_phnop"].ToString().Trim() != "")
                    {
                        pstdisd_span.InnerHtml = ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]);
                    }
                    else
                    {
                        pstdisd_span.InnerHtml = "-";
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (ddledu.SelectedItem.Text == "UG")
                    {
                        #region Ug

                        ugtotaldiv.Visible = true;
                        pgtotaldiv.Visible = false;
                        string courseentronumber = Convert.ToString(ds.Tables[1].Rows[0]["course_entno"]);
                        string coursecode = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);
                        string university_code = Convert.ToString(ds.Tables[1].Rows[0]["university_code"]);
                        string institutename = Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]);
                        string percentage = Convert.ToString(ds.Tables[1].Rows[0]["percentage"]);
                        string institueaddress = Convert.ToString(ds.Tables[1].Rows[0]["instaddress"]);
                        string medium = Convert.ToString(ds.Tables[1].Rows[0]["medium"]);
                        string part1language = Convert.ToString(ds.Tables[1].Rows[0]["Part1Language"]);
                        string part2language = Convert.ToString(ds.Tables[1].Rows[0]["Part2Language"]);
                        string isgrade = Convert.ToString(ds.Tables[1].Rows[0]["isgrade"]);
                        string university_state = Convert.ToString(ds.Tables[1].Rows[0]["uni_state"]);
                        // string part1language = Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]);
                        string cutoffmark = Convert.ToString(ds.Tables[1].Rows[0]["Cut_Of_Mark"]);
                        string pcmmark = Convert.ToString(ds.Tables[1].Rows[0]["PCM_Percentage"]);
                        if (coursecode.Trim() != "")
                        {
                            string course1 = subjectcode(coursecode);
                            qualifyingexam_span.InnerHtml = ":  " + Convert.ToString(course1);
                        }
                        else
                        {
                            qualifyingexam_span.InnerHtml = ":  -";
                        }
                        if (institutename.Trim() != "")
                        {
                            Nameofschool_span.InnerHtml = ":  " + Convert.ToString(institutename);
                        }
                        else
                        {
                            Nameofschool_span.InnerHtml = "";
                        }
                        if (institueaddress.Trim() != "")
                        {
                            locationofschool_Span.InnerHtml = ":  " + Convert.ToString(institueaddress);
                        }
                        else
                        {
                            locationofschool_Span.InnerHtml = "";
                        }
                        if (medium.Trim() != "")
                        {
                            string m = subjectcode(medium);
                            mediumofstudy_span.InnerHtml = ":  " + Convert.ToString(m);
                        }
                        else
                        {
                            mediumofstudy_span.InnerHtml = ":  -";
                        }
                        if (university_code.Trim() != "")
                        {
                            string univ = subjectcode(university_code);
                            qualifyingboard_span.InnerHtml = ":  " + Convert.ToString(univ);
                        }
                        else
                        {
                            qualifyingboard_span.InnerHtml = ":  -";
                        }
                        if (isgrade.Trim() != "")
                        {
                            if (isgrade == "True")
                            {
                                marksgrade_span.InnerHtml = ":  Grade";
                            }
                            else
                            {
                                marksgrade_span.InnerHtml = ":  Marks";
                            }
                        }
                        if (pcmmark.Trim() != "")
                        {
                            pcm_mark.InnerHtml = ":" + Convert.ToString(pcmmark);
                        }
                        cutoffmark_span.InnerHtml = ": " + Convert.ToString(cutoffmark) == "" ? " - " : ": " + Convert.ToString(cutoffmark);
                        string markquery = "select psubjectno,registerno,acual_marks,max_marks,noofattempt,pass_month,pass_year,semyear ,grade from perv_marks_history  where course_entno ='" + courseentronumber + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(markquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable data = new DataTable();
                            DataRow dr = null;
                            Hashtable hash = new Hashtable();
                            data.Columns.Add("Language", typeof(string));
                            data.Columns.Add("Subject", typeof(string));
                            data.Columns.Add("Marks Obtained", typeof(string));
                            data.Columns.Add("Month", typeof(string));
                            data.Columns.Add("Year", typeof(string));
                            data.Columns.Add("Register No / Roll No", typeof(string));
                            data.Columns.Add("No of Attempts", typeof(string));
                            data.Columns.Add("Maximum Marks", typeof(string));
                            hash.Add(0, "Language1");
                            hash.Add(1, "Language2");
                            hash.Add(2, " Subject1");
                            hash.Add(3, " Subject2");
                            hash.Add(4, " Subject3");
                            hash.Add(5, " Subject4");
                            hash.Add(6, " Subject5");
                            hash.Add(7, " Subject6");
                            hash.Add(8, " Subject7");
                            hash.Add(9, " Subject8");
                            hash.Add(10, " Subject9");
                            hash.Add(11, " Subject10");
                            hash.Add(12, " Subject11");
                            int totalmark = 0;
                            int maxtotal = 0;
                            for (int mark = 0; mark < ds.Tables[0].Rows.Count; mark++)
                            {
                                string subjectno = Convert.ToString(ds.Tables[0].Rows[mark]["psubjectno"]);
                                string actualmark = "";
                                if (isgrade == "True")
                                {
                                    actualmark = Convert.ToString(ds.Tables[0].Rows[mark]["grade"]);
                                }
                                else
                                {
                                    actualmark = Convert.ToString(ds.Tables[0].Rows[mark]["acual_marks"]);
                                }
                                string month = Convert.ToString(ds.Tables[0].Rows[mark]["pass_month"]);
                                string year = Convert.ToString(ds.Tables[0].Rows[mark]["pass_year"]);
                                string regno = Convert.ToString(ds.Tables[0].Rows[mark]["registerno"]);
                                string noofattenm = Convert.ToString(ds.Tables[0].Rows[mark]["noofattempt"]);
                                string maxmark = Convert.ToString(ds.Tables[0].Rows[mark]["max_marks"]);
                                dr = data.NewRow();
                                string lang = Convert.ToString(hash[mark]);
                                dr[0] = Convert.ToString(lang);
                                string sub = subjectcode(subjectno);
                                dr[1] = Convert.ToString(sub);
                                dr[2] = Convert.ToString(actualmark);
                                dr[3] = Convert.ToString(month);
                                dr[4] = Convert.ToString(year);
                                dr[5] = Convert.ToString(regno);
                                dr[6] = Convert.ToString(noofattenm);
                                dr[7] = Convert.ToString(maxmark);
                                data.Rows.Add(dr);
                                if (isgrade != "True")
                                {
                                    totalmark = totalmark + Convert.ToInt32(actualmark);
                                    maxtotal = maxtotal + Convert.ToInt32(maxmark);
                                }
                            }
                            if (isgrade != "True")
                            {
                                total_marks_secured.InnerHtml = ":  " + Convert.ToString(totalmark);
                                maximum_marks.InnerHtml = ":  " + Convert.ToString(maxtotal);
                                percentage_span.InnerHtml = ":  " + percentage;
                            }
                            VerificationGridug.DataSource = data;
                            VerificationGridug.DataBind();
                            if (VerificationGridug.Rows.Count > 0)
                            {
                                for (int v = 0; v < VerificationGridug.Rows.Count; v++)
                                {
                                    VerificationGridug.Rows[v].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                                    VerificationGridug.Rows[v].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                                    VerificationGridug.Rows[v].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                    VerificationGridug.Rows[v].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                    VerificationGridug.Rows[v].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                                    VerificationGridug.Rows[v].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        #endregion
                    }
                    else if (ddledu.SelectedItem.Text == "PG")
                    {
                        #region Pg

                        ugtotaldiv.Visible = false;
                        pgtotaldiv.Visible = true;
                        string courseentronumber = Convert.ToString(ds.Tables[1].Rows[0]["course_entno"]);
                        string coursecode = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);
                        string university_code = Convert.ToString(ds.Tables[1].Rows[0]["university_code"]);
                        string institutename = Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]);
                        string percentage = Convert.ToString(ds.Tables[1].Rows[0]["percentage"]);
                        string institueaddress = Convert.ToString(ds.Tables[1].Rows[0]["instaddress"]);
                        string medium = Convert.ToString(ds.Tables[1].Rows[0]["medium"]);
                        string part1language = Convert.ToString(ds.Tables[1].Rows[0]["Part1Language"]);
                        string part2language = Convert.ToString(ds.Tables[1].Rows[0]["Part2Language"]);
                        string isgrade = Convert.ToString(ds.Tables[1].Rows[0]["isgrade"]);
                        string university_state = Convert.ToString(ds.Tables[1].Rows[0]["uni_state"]);
                        string typeofsubject = Convert.ToString(ds.Tables[1].Rows[0]["type_major"]);
                        string typeofsemester = Convert.ToString(ds.Tables[1].Rows[0]["type_semester"]);
                        string regno = Convert.ToString(ds.Tables[1].Rows[0]["registration_no"]);
                        string major = Convert.ToString(ds.Tables[1].Rows[0]["branch_code"]);
                        string majorpercentage = Convert.ToString(ds.Tables[1].Rows[0]["major_percent"]);
                        string majorallidepercentage = Convert.ToString(ds.Tables[1].Rows[0]["majorallied_percent"]);
                        string Tancet = Convert.ToString(ds.Tables[1].Rows[0]["tancet_mark"]);
                        string Tancetyear = Convert.ToString(ds.Tables[1].Rows[0]["tancetmark_year"]);
                        string cutoffmark = Convert.ToString(ds.Tables[1].Rows[0]["Cut_Of_Mark"]);
                        percentagemajorspan.InnerHtml = ":  " + Convert.ToString(percentage);
                        majorsubjectspan.InnerHtml = ":  " + Convert.ToString(majorpercentage);
                        alliedmajorspan.InnerHtml = ":  " + Convert.ToString(majorallidepercentage);
                        if (coursecode.Trim() != "")
                        {
                            string course1 = subjectcode(coursecode);
                            ugqualifyingexam_span.InnerHtml = ":  " + Convert.ToString(course1);
                        }
                        else
                        {
                            ugqualifyingexam_span.InnerHtml = ":  -";
                        }
                        if (institutename.Trim() != "")
                        {
                            nameofcollege_Sapn.InnerHtml = ":  " + Convert.ToString(institutename);
                        }
                        else
                        {
                            nameofcollege_Sapn.InnerHtml = "";
                        }
                        if (institueaddress.Trim() != "")
                        {
                            locationofcollege_sapn.InnerHtml = ":  " + Convert.ToString(institueaddress);
                        }
                        else
                        {
                            locationofcollege_sapn.InnerHtml = "";
                        }
                        if (major.Trim() != "")
                        {
                            string major1 = subjectcode(major);
                            major_span.InnerHtml = ":  " + Convert.ToString(major1);
                        }
                        else
                        {
                            major_span.InnerHtml = "";
                        }
                        if (typeofsubject.Trim() != "")
                        {
                            if (typeofsubject == "1")
                            {
                                typeofsubject = "Single";
                            }
                            else if (typeofsubject == "2")
                            {
                                typeofsubject = "Double";
                            }
                            else if (typeofsubject == "3")
                            {
                                typeofsubject = "Triple";
                            }
                            typeofmajor_span.InnerHtml = ":  " + Convert.ToString(typeofsubject);
                        }
                        if (typeofsemester.Trim() != "")
                        {
                            if (typeofsemester == "True")
                            {
                                typeofsemester = "Semester";
                            }
                            else
                            {
                                typeofsemester = "Non Semester";
                            }
                            typeofsemester_span.InnerHtml = ":  " + Convert.ToString(typeofsemester);
                        }
                        if (medium.Trim() != "")
                        {
                            string lang = subjectcode(medium);
                            mediumofstudy_spanug.InnerHtml = ":  " + Convert.ToString(lang);
                        }
                        if (isgrade.Trim() != "")
                        {
                            if (isgrade == "True")
                            {
                                marksorgradeug_span.InnerHtml = ":  Grade";
                            }
                            else
                            {
                                marksorgradeug_span.InnerHtml = ":  Marks";
                            }
                        }
                        //if (isgrade.Trim() != "")
                        //{
                        //    marksorgradeug_span.InnerHtml = ":  " + Convert.ToString(isgrade);
                        //}
                        if (regno.Trim() != "")
                        {
                            reg_no_span.InnerHtml = ":  " + Convert.ToString(regno);
                        }
                        cutoffmarkpg.InnerHtml = ": " + Convert.ToString(cutoffmark) == "" ? " - " : ": " + Convert.ToString(cutoffmark);
                        if (type == "MCA")
                        {
                            tnspan.Visible = true;
                            Tancetspan.Visible = true;
                            Tancetspan.InnerHtml = ": " + Convert.ToString(Tancet) + " - " + Convert.ToString(Tancetyear);
                        }
                        string pgquery = "select psubjectno,subject_typeno,acual_marks,max_marks,pass_month,pass_year,semyear ,grade from perv_marks_history where course_entno ='" + courseentronumber + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(pgquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable data = new DataTable();
                            DataRow dr = null;
                            Hashtable hash = new Hashtable();
                            data.Columns.Add("S.No", typeof(string));
                            //  data.Columns.Add("Sem/Year", typeof(string));
                            data.Columns.Add("Subject", typeof(string));
                            data.Columns.Add("Subject type", typeof(string));
                            data.Columns.Add("Marks", typeof(string));
                            data.Columns.Add("Month", typeof(string));
                            data.Columns.Add("Year", typeof(string));
                            data.Columns.Add("Maximum Marks", typeof(string));
                            int sno = 0;
                            for (int pg = 0; pg < ds.Tables[0].Rows.Count; pg++)
                            {
                                sno++;
                                string subjectno = Convert.ToString(ds.Tables[0].Rows[pg]["psubjectno"]);
                                string subjecttypeno = Convert.ToString(ds.Tables[0].Rows[pg]["subject_typeno"]);
                                string actualmark = "";
                                if (isgrade == "True")
                                {
                                    actualmark = Convert.ToString(ds.Tables[0].Rows[pg]["grade"]);
                                }
                                else
                                {
                                    actualmark = Convert.ToString(ds.Tables[0].Rows[pg]["acual_marks"]);
                                }
                                string month = Convert.ToString(ds.Tables[0].Rows[pg]["pass_month"]);
                                string year = Convert.ToString(ds.Tables[0].Rows[pg]["pass_year"]);
                                // string noofattenm = Convert.ToString(ds.Tables[0].Rows[pg]["noofattempt"]);
                                string maxmark = Convert.ToString(ds.Tables[0].Rows[pg]["max_marks"]);
                                dr = data.NewRow();
                                dr[0] = Convert.ToString(sno);
                                string subject = subjectcode(subjectno);
                                dr[1] = Convert.ToString(subject);
                                string typesub = subjectcode(subjecttypeno);
                                dr[2] = Convert.ToString(typesub);
                                dr[3] = Convert.ToString(actualmark);
                                dr[4] = Convert.ToString(month);
                                dr[5] = Convert.ToString(year);
                                dr[6] = Convert.ToString(maxmark);
                                data.Rows.Add(dr);
                            }
                            Verificationgridpg.DataSource = data;
                            Verificationgridpg.DataBind();
                            if (VerificationGridug.Rows.Count > 0)
                            {
                                for (int v = 0; v < Verificationgridpg.Rows.Count; v++)
                                {
                                    Verificationgridpg.Rows[v].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                                    Verificationgridpg.Rows[v].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                    Verificationgridpg.Rows[v].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                    Verificationgridpg.Rows[v].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion
            }
            else if (Convert.ToString(ViewState["applicationviewformatset"]) == "1")
            {
                licetapplicationprint(app_no);//barath 22.05.17

            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbDegreeUpdate_Changed(object sender, EventArgs e)
    {
        string collegecode = string.Empty;
        if (Session["studclgcode"] != null)
            collegecode = Convert.ToString(Session["studclgcode"]);
        if (cbDegreeUpdate.Checked == true)
        {
            panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
            divDegreeDetails.Visible = true;
            UpdBindBatch();
            UpdBindDegree(collegecode);
            UpdBindSeatType(collegecode);
        }
        else
        {
            panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
            divDegreeDetails.Visible = false;
        }
    }
    //print pdf
    protected void Button6_Clcik(object sender, EventArgs e)
    {
        if (Convert.ToString(ViewState["applicationviewformatset"]) == "0")
        {
            string collegecode = string.Empty;
            if (Session["studclgcode"] != null)
                collegecode = Convert.ToString(Session["studclgcode"]);
            string degreecode = string.Empty;
            if (Session["studclgcode"] != null)
                degreecode = Convert.ToString(Session["studdegcode"]);
            string degreeText = string.Empty;
            string deptText = string.Empty;
            string eduLevel = string.Empty;
            string type = string.Empty;
            getdeptDetails(collegecode, degreecode, ref  type, ref  degreeText, ref  deptText, ref  eduLevel);
            pdf(collegecode, degreeText, deptText);
        }
        else if (Convert.ToString(ViewState["applicationviewformatset"]) == "1")
        {
            clgHeader_tbl.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", " licetPrintdiv();", true);
        }
    }
    public void pdf(string college_code, string degreeText, string deptText)
    {
        try
        {
            Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypage = mydoc.NewPage();
            Gios.Pdf.PdfPage mypage1 = mydoc.NewPage();
            Gios.Pdf.PdfPage mypage2 = mydoc.NewPage();
            bool dummyflage = false;
            if (File.Exists(HttpContext.Current.Server.MapPath("~/image/logo.jpg")))//Aruna
            {
                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/image/logo.jpg"));
                mypage.Add(LogoImage, 20, 20, 200);
            }
            //if (File.Exists(HttpContext.Current.Server.MapPath("~/image/logo1.jpg")))//Aruna
            //{
            //    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/image/logo1.jpg"));
            //    mypage.Add(LogoImage, 500, 20, 200);
            //}
            string collquery = "";
            collquery = "select collname,category,university,address1,address2,address3,phoneno,faxno,email,website,district,state,pincode  from collinfo where college_Code=" + college_code + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(collquery, "Text");
            string collegename = "";
            string collegeaddress = "";
            string collegedistrict = "";
            string phonenumber = "";
            string fax = "";
            string email = "";
            string website = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                collegeaddress = Convert.ToString(ds.Tables[0].Rows[0]["address1"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address2"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                collegedistrict = Convert.ToString(ds.Tables[0].Rows[0]["district"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["state"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
                phonenumber = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                fax = Convert.ToString(ds.Tables[0].Rows[0]["faxno"]); ;
                email = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
            }
            PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 10, 10, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
            mypage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 130, 25, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegeaddress);
            mypage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 130, 35, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegedistrict);
            mypage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 130, 45, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phone No: " + phonenumber + ", Fax:" + fax);
            mypage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 130, 55, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, email);
            mypage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 130, 65, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, website);
            mypage.Add(ptc);


            //krishhna kumar.r 


            //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + Convert.ToString(Session["pdfapp_no"]) + ".jpeg")))//Aruna
            //{
            //    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + Convert.ToString(Session["pdfapp_no"]) + ".jpeg"));
            //    mypage.Add(LogoImage, 500, 20, 200);
            //}

            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + Convert.ToString(Session["pdfapp_no"]) + ".jpeg")))//Aruna
            {
                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + Convert.ToString(Session["pdfapp_no"]) + ".jpeg"));
                mypage.Add(LogoImage, 450, 20, 200);
            }




            int y = 60;
            int line1 = 40;
            int line2 = 300;
            //string query = "select app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu, (select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.mother_tongue)) mother_tongue, (select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.religion) and TextCriteria='relig') religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,degree_code,batch_year,college_code,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet,Alternativedegree_code,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.subcaste) and TextCriteria='scast') SubCaste,case when Dalits='1' then 'Yes' when Dalits='0' then 'No' end Dalits,Parish_name,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.bldgrp) and TextCriteria='bgrou') bloodgroup  from applyn a where a.app_no='" + Convert.ToString(Session["pdfapp_no"]) + "'";
            //query = query + " select course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark,Cut_Of_Mark from Stud_prev_details where app_no ='" + Convert.ToString(Session["pdfapp_no"]) + "'";

            ////krishhna kumar.r

            string query = "select app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu, (select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.mother_tongue)) mother_tongue, (select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.religion) and TextCriteria='relig') religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.Cityc) and TextCriteria='city')as Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.Cityc) and TextCriteria='city')as cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,degree_code,batch_year,college_code,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet,Alternativedegree_code,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.subcaste) and TextCriteria='scast') SubCaste,case when Dalits='1' then 'Yes' when Dalits='0' then 'No' end Dalits,Parish_name,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.bldgrp) and TextCriteria='bgrou') bloodgroup  from applyn a where a.app_no='" + Convert.ToString(Session["pdfapp_no"]) + "'";
            query = query + " select course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark,Cut_Of_Mark,PCM_Percentage from Stud_prev_details where app_no ='" + Convert.ToString(Session["pdfapp_no"]) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 50, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Course Details");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Stream");
                mypage.Add(ptc);
                string stream = "";
                //if (ddltype.SelectedItem.Text != "--Select--")
                //{
                //    stream = Convert.ToString(ddltype.SelectedItem.Text);
                //}
                //else
                //{
                //    stream = "";
                //}

                string Cut_Of_Mark = Convert.ToString(ds.Tables[1].Rows[0]["Cut_Of_Mark"]);
                string pcmmark = Convert.ToString(ds.Tables[1].Rows[0]["PCM_Percentage"]);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + stream);
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Graduation");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ddledu.SelectedItem.Text));
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 110, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 110, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + degreeText);
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Choice I");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 130, 300, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + deptText);
                mypage.Add(ptc);
                y += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Choice II");
                mypage.Add(ptc);

                string choiseII = Convert.ToString(d2.GetFunction("select dt.Dept_Name from degree d,Department dt,Course c  where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  degree_code=" + Convert.ToString(ds.Tables[0].Rows[0]["Alternativedegree_code"]) + ""));
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 130, 300, 30), System.Drawing.ContentAlignment.MiddleLeft, choiseII == "0" ? ":  -" : ":  " + choiseII);
                mypage.Add(ptc);






                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, line1, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Application No");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["app_formno"]));
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Applicant Name");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, line2, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                mypage.Add(ptc);
                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                                new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Applicant Last  Name");
                //mypage.Add(ptc);
                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                              new PdfArea(mydoc, line2, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Session["lastname"]));
                //mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date of Birth");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line2, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["dob"]));
                mypage.Add(ptc);
                string gender = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
                if (gender == "0")
                {
                    gender = "Male";
                }
                else if (gender == "1")
                {
                    gender = "Female";
                }
                else if (gender == "2")
                {
                    gender = "Transgender";
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Gender");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(gender));
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Parent's Name/Guardian Name");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]));
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Relationship");
                mypage.Add(ptc);





                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, line2, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Relationship"]));
                mypage.Add(ptc);






                //krishhna kumar.r


                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, line1, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Blood Group:");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["bloodgroup"]));
                mypage.Add(ptc);



                string occupation = Convert.ToString(ds.Tables[0].Rows[0]["parent_occu"]);
                if (occupation.Trim() != "")
                {
                    occupation = subjectcode(occupation);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 290, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Occupation");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 290, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Session["occupation"]));
                mypage.Add(ptc);
                string mothertounge = Convert.ToString(ds.Tables[0].Rows[0]["mother_tongue"]);

                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Mother Tounge ");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(mothertounge));
                mypage.Add(ptc);
                string Religion = Convert.ToString(ds.Tables[0].Rows[0]["religion"]);

                string subcaste = Convert.ToString(ds.Tables[0].Rows[0]["SubCaste"]);
                string Dalits = Convert.ToString(ds.Tables[0].Rows[0]["Dalits"]);
                string Parish_name = Convert.ToString(ds.Tables[0].Rows[0]["Parish_name"]);
                string subcasteval = "";
                if (subcaste.ToUpper() == "ROMAN CATHOLIC")
                {
                    subcasteval = " Dalits :" + Dalits + " Parish Name: " + Parish_name;
                }
                if (subcasteval.Trim() != "")
                    Religion = Religion + " " + subcasteval;

                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, line1, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Religion");
                mypage.Add(ptc);

                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 330, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Religion));
                mypage.Add(ptc);
                string Nationality = Convert.ToString(ds.Tables[0].Rows[0]["citizen"]);
                if (Nationality.Trim() != "")
                {
                    Nationality = subjectcode(Nationality);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Nationality");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(Nationality));
                mypage.Add(ptc);
                string coummunity = Convert.ToString(ds.Tables[0].Rows[0]["community"]);
                if (coummunity.Trim() != "")
                {
                    coummunity = subjectcode(coummunity);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Coummunity(Foriegn Students Select OC)");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, line2, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(coummunity));
                mypage.Add(ptc);
                string caste = Convert.ToString(ds.Tables[0].Rows[0]["caste"]);
                if (caste.Trim() != "")
                {
                    caste = subjectcode(caste);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Caste");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, line2, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(caste));
                mypage.Add(ptc);
                string subreligion = Convert.ToString(ds.Tables[0].Rows[0]["caste"]);
                if (subreligion.Trim() != "")
                {
                    subreligion = subjectcode(subreligion);
                }
                int col = y + 390;
                if (Convert.ToString(subreligion).ToUpper() == "PROTESTANT")
                {
                    string missionarychild = Convert.ToString(ds.Tables[0].Rows[0]["MissionaryChild"]);
                    if (missionarychild == "0" || missionarychild == "False")
                    {
                        missionarychild = "No";
                    }
                    else
                    {
                        missionarychild = "Yes";
                    }
                    col += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are You a missionary child ?");
                    mypage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(missionarychild));
                    mypage.Add(ptc);
                }
                string tamilorgion = Convert.ToString(ds.Tables[0].Rows[0]["TamilOrginFromAndaman"]);
                if (tamilorgion.Trim() == "0" || tamilorgion.Trim() == "False")
                {
                    tamilorgion = "No";
                }
                else
                {
                    tamilorgion = "Yes";
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are You of Tamil Origin From Andaman and Nicobar Islands ?");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(tamilorgion));
                mypage.Add(ptc);
                string xserviceman = Convert.ToString(ds.Tables[0].Rows[0]["IsExService"]);
                if (xserviceman.Trim() == "0" || xserviceman.Trim() == "False")
                {
                    xserviceman = "No";
                }
                else
                {
                    xserviceman = "Yes";
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are You a Child of an Ex-serviceman of Tamil Nadu origin ?");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(xserviceman));
                mypage.Add(ptc);
                string differentlyabled = Convert.ToString(ds.Tables[0].Rows[0]["isdisable"]);
                if (differentlyabled.Trim() == "0" || differentlyabled.Trim() == "False")
                {
                    differentlyabled = "No";
                }
                else
                {
                    differentlyabled = "Yes";
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are you a Differently abled");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(differentlyabled));
                mypage.Add(ptc);
                string firstgeneration = Convert.ToString(ds.Tables[0].Rows[0]["first_graduate"]);
                if (firstgeneration.Trim() == "0" || firstgeneration.Trim() == "False")
                {
                    firstgeneration = "No";
                }
                else
                {
                    firstgeneration = "Yes";
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are you a first genaration learner ?");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(firstgeneration));
                mypage.Add(ptc);
                string oncampus = Convert.ToString(ds.Tables[0].Rows[0]["CampusReq"]);
                if (oncampus.Trim() == "0" || oncampus.Trim() == "False")
                {
                    oncampus = "No";
                }
                else
                {
                    oncampus = "Yes";
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Is Residence on Campus Required ?");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(oncampus));
                mypage.Add(ptc);
                string sports = Convert.ToString(ds.Tables[0].Rows[0]["DistinctSport"]);
                if (sports.Trim() != "")
                {
                    sports = subjectcode(sports);
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Distinction in Sports");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(sports));
                mypage.Add(ptc);
                string cocucuricular = Convert.ToString(ds.Tables[0].Rows[0]["co_curricular"]);
                if (cocucuricular.Trim() != "")
                {
                    cocucuricular = subjectcode(cocucuricular);
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, line1, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "Extra Curricular Activites/Co-Curricular Activites");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, line2, col, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(cocucuricular));
                mypage.Add(ptc);
                col += 20;
                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Communication Address");
                mypage.Add(ptc);
                string addressline1 = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressC"]);
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line1");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addressline1));
                mypage.Add(ptc);
                string addressline2 = Convert.ToString(ds.Tables[0].Rows[0]["Streetc"]);
                string addressline3 = "";
                if (addressline2.Contains('/') == true)
                {
                    string[] splitaddress = addressline2.Split('/');
                    if (splitaddress.Length > 1)
                    {
                        addressline2 = Convert.ToString(splitaddress[0]);
                        addressline3 = Convert.ToString(splitaddress[1]);
                    }
                    else
                    {
                        addressline2 = Convert.ToString(splitaddress[0]);
                    }
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line2");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addressline2));
                mypage.Add(ptc);
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line3");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addressline3));
                mypage.Add(ptc);
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "City");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Cityc"]));
                mypage.Add(ptc);
                string pstate = Convert.ToString(ds.Tables[0].Rows[0]["parent_statec"]);
                if (pstate.Trim() != "")
                {
                    pstate = subjectcode(pstate);
                }
                col += 20;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "State");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(pstate));
                mypage.Add(ptc);
                col += 20;
                string country = Convert.ToString(ds.Tables[0].Rows[0]["Countryc"]);
                if (country.Trim() != "")
                {
                    country = subjectcode(country);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Country");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(country));
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, col + 20, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "PIN code");
                mypage.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, col + 20, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodec"]));
                mypage.Add(ptc);
                y = 40;
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, line1, y + 30, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 30, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, line1, y + 50, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Alternate Number");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 50, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["alter_mobileno"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Email ID");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["StuPer_Id"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Phone Number With STD Code");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 90, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_phnoc"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 110, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Permanent Address");
                mypage1.Add(ptc);
                string addresslinec1 = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line1");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addresslinec1));
                mypage1.Add(ptc);
                string addresslinec2 = Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]);
                string addresslinec3 = "";
                if (addressline2.Contains('/') == true)
                {
                    string[] splitaddress = addressline2.Split('/');
                    if (splitaddress.Length > 1)
                    {
                        addresslinec2 = Convert.ToString(splitaddress[0]);
                        addresslinec3 = Convert.ToString(splitaddress[1]);
                    }
                    else
                    {
                        addresslinec2 = Convert.ToString(splitaddress[0]);
                    }
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line2");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 150, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addresslinec2));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Address Line3");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 170, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(addresslinec3));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "City");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["cityp"]));
                mypage1.Add(ptc);
                string cstate = Convert.ToString(ds.Tables[0].Rows[0]["parent_statep"]);
                if (cstate.Trim() != "")
                {
                    cstate = subjectcode(cstate);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "State");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 210, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(cstate));
                mypage1.Add(ptc);
                string ccournty = Convert.ToString(ds.Tables[0].Rows[0]["Countryp"]);
                if (ccournty.Trim() != "")
                {
                    ccournty = subjectcode(ccournty);
                }
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Country");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 230, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ccournty));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "PIN code");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 250, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Phone Number With STD Code");
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, line2, y + 270, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]));
                mypage1.Add(ptc);
                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 290, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Academic Details");
                mypage1.Add(ptc);



                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, 40, y + 700, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "i.	All the original certificates should be submitted to the college immediately after receiving from the school.  Sufficient number of copies of the certificates may be taken before submitting the same. ");
                mypage1.Add(ptc);


                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, 40, y + 740, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "ii.	I, hereby, confirm you that as I have joined in management quota, I will not attend the Tamilnadu Engineering Admission Counselling 2018. ");
                mypage1.Add(ptc);

                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                              new PdfArea(mydoc, 40, y + 780, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Parent ");
                mypage1.Add(ptc);


                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                              new PdfArea(mydoc, 40, y + 780, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "Signature of the Student ");
                mypage1.Add(ptc);


                if (ddledu.SelectedItem.Text.ToUpper() == "UG")
                {
                    string qualifyingexam = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);
                    if (qualifyingexam.Trim() != "")
                    {
                        qualifyingexam = subjectcode(qualifyingexam);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Qualifying Examination Passed");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(qualifyingexam));
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name of School");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]));
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Location of School");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["instaddress"]));
                    mypage1.Add(ptc);
                    string mediumofstudy = Convert.ToString(ds.Tables[1].Rows[0]["medium"]);
                    if (mediumofstudy.Trim() != "")
                    {
                        mediumofstudy = subjectcode(mediumofstudy);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Medium of Study of Qualifying Examination");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(mediumofstudy));
                    mypage1.Add(ptc);
                    string qulifyboard = Convert.ToString(ds.Tables[1].Rows[0]["university_code"]);
                    if (qulifyboard.Trim() != "")
                    {
                        qulifyboard = subjectcode(qulifyboard);
                    }
                    string qulifystate = Convert.ToString(ds.Tables[1].Rows[0]["uni_state"]);
                    if (qulifystate.Trim() != "")
                    {
                        qulifystate = subjectcode(qulifystate);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Qualifying Board & State");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(qulifyboard) + " " + Convert.ToString(qulifystate));
                    mypage1.Add(ptc);
                    string vocationalstream = Convert.ToString(ds.Tables[1].Rows[0]["Vocational_stream"]);
                    if (vocationalstream.Trim() == "0" || vocationalstream.Trim() == "False")
                    {
                        vocationalstream = "No";
                    }
                    else
                    {
                        vocationalstream = "Yes";
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Are you Vocational stream");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(vocationalstream));
                    mypage1.Add(ptc);
                    string markgrade = Convert.ToString(ds.Tables[1].Rows[0]["isgrade"]);
                    if (markgrade.Trim() == "False")
                    {
                        markgrade = "Mark";
                    }
                    else
                    {
                        markgrade = "Grade";
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Marks/Grade");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(markgrade));
                    mypage1.Add(ptc);
                    string percentage = Convert.ToString(ds.Tables[1].Rows[0]["percentage"]);
                    int totalmark = 0;
                    int maxtotal = 0;
                    DataTable data = new DataTable();
                    DataRow dr = null;
                    Hashtable hash = new Hashtable();
                    string markquery = "select psubjectno,registerno,acual_marks,grade,max_marks,noofattempt,pass_month,pass_year from perv_marks_history  where course_entno ='" + Convert.ToString(ds.Tables[1].Rows[0]["course_entno"]) + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(markquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        data.Columns.Add("Language", typeof(string));
                        data.Columns.Add("Subject", typeof(string));
                        data.Columns.Add("Marks Obtained", typeof(string));
                        data.Columns.Add("Month", typeof(string));
                        data.Columns.Add("Year", typeof(string));
                        data.Columns.Add("Register No / Roll No", typeof(string));
                        data.Columns.Add("No of Attempts", typeof(string));
                        data.Columns.Add("Maximum Marks", typeof(string));
                        hash.Add(0, "Language1");
                        hash.Add(1, "Language2");
                        hash.Add(2, " Subject1");
                        hash.Add(3, " Subject2");
                        hash.Add(4, " Subject3");
                        hash.Add(5, " Subject4");
                        hash.Add(6, " Subject5");
                        hash.Add(7, " Subject6");
                        hash.Add(8, " Subject7");
                        hash.Add(9, " Subject8");
                        hash.Add(10, " Subject9");
                        hash.Add(11, " Subject10");
                        hash.Add(12, " Subject11");
                        for (int mark = 0; mark < ds.Tables[0].Rows.Count; mark++)
                        {
                            string subjectno = Convert.ToString(ds.Tables[0].Rows[mark]["psubjectno"]);
                            string actualmark = "";
                            if (markgrade.Trim() == "Mark")
                            {
                                actualmark = Convert.ToString(ds.Tables[0].Rows[mark]["acual_marks"]);
                            }
                            if (markgrade.Trim() == "Grade")
                            {
                                actualmark = Convert.ToString(ds.Tables[0].Rows[mark]["grade"]);
                            }
                            string month = Convert.ToString(ds.Tables[0].Rows[mark]["pass_month"]);
                            string year = Convert.ToString(ds.Tables[0].Rows[mark]["pass_year"]);
                            string regno = Convert.ToString(ds.Tables[0].Rows[mark]["registerno"]);
                            string noofattenm = Convert.ToString(ds.Tables[0].Rows[mark]["noofattempt"]);
                            string maxmark = Convert.ToString(ds.Tables[0].Rows[mark]["max_marks"]);
                            dr = data.NewRow();
                            string lang = Convert.ToString(hash[mark]);
                            dr[0] = Convert.ToString(lang);
                            string sub = subjectcode(subjectno);
                            dr[1] = Convert.ToString(sub);
                            dr[2] = Convert.ToString(actualmark);
                            dr[3] = Convert.ToString(month);
                            dr[4] = Convert.ToString(year);
                            dr[5] = Convert.ToString(regno);
                            dr[6] = Convert.ToString(noofattenm);
                            dr[7] = Convert.ToString(maxmark);
                            data.Rows.Add(dr);
                            if (markgrade.Trim() != "Grade")
                            {
                                totalmark = totalmark + Convert.ToInt32(actualmark);
                                maxtotal = maxtotal + Convert.ToInt32(maxmark);
                            }
                        }
                        //////////////// zzz
                        int count = 0;
                        count = data.Rows.Count;
                        Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2.VisibleHeaders = false;
                        table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table2.Columns[0].SetWidth(100);
                        table2.Columns[1].SetWidth(100);
                        table2.Columns[2].SetWidth(100);
                        table2.Columns[3].SetWidth(100);
                        table2.Columns[4].SetWidth(100);
                        table2.Columns[5].SetWidth(100);
                        table2.Columns[6].SetWidth(100);
                        table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                        table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 0).SetContent("Subjects");
                        if (markgrade.Trim() == "Mark")
                        {
                            table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 1).SetContent("Mark");
                        }
                        if (markgrade.Trim() == "Grade")
                        {
                            table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 1).SetContent("Grade");
                        }
                        table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 2).SetContent("Month");
                        table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 3).SetContent("Year");
                        table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 4).SetContent("Register No");
                        table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 5).SetContent("No.of Attempts");
                        table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 6).SetContent("Maximun Marks");
                        for (int add = 0; add < data.Rows.Count; add++)
                        {
                            table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(add + 1, 0).SetContent(Convert.ToString(data.Rows[add]["Subject"]));
                            table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 1).SetContent(Convert.ToString(data.Rows[add]["Marks Obtained"]));
                            table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 2).SetContent(Convert.ToString(data.Rows[add]["Month"]));
                            // Month.First().ToString().ToUpper() + Month.Substring(1)
                            table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 3).SetContent(Convert.ToString(data.Rows[add]["Year"]));
                            table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 4).SetContent(Convert.ToString(data.Rows[add]["Register No / Roll No"]));
                            table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 5).SetContent(Convert.ToString(data.Rows[add]["No of Attempts"]));
                            table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 6).SetContent(Convert.ToString(data.Rows[add]["Maximum Marks"]));
                        }
                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 550, 550, 550));
                        mypage1.Add(myprov_pdfpage1);
                        if (Convert.ToString(markgrade).Trim() == "Mark")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 40, y + 650, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total Marks Obtained :  " + Convert.ToString(totalmark));
                            mypage1.Add(ptc);
                            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                        new PdfArea(mydoc, 250, y + 650, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Maximum Marks :  " + Convert.ToString(maxtotal));
                            //mypage1.Add(ptc);
                            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                        new PdfArea(mydoc, 480, y + 650, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Percentage :  " + Convert.ToString(percentage));
                            //mypage1.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 480, y + 650, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Pcm Mark :  " + Convert.ToString(pcmmark));
                            mypage1.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 40, y + 680, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Cut Off Mark :  " + Convert.ToString(Cut_Of_Mark));
                            mypage1.Add(ptc);

                        }
                    }
                }



                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                new PdfArea(mydoc, 40, y + 800, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "i.	All the original certificates should be submitted to the college immediately after receiving from the school.  Sufficient number of copies of the certificates may be taken before submitting the same. ");
                //mypage1.Add(ptc);


                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                                new PdfArea(mydoc, line1, 860, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "ii.	I, hereby, confirm you that as I have joined in management quota, I will not attend the Tamilnadu Engineering Admission Counselling 2018. ");
                //mypage1.Add(ptc);

                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                              new PdfArea(mydoc, line1, 880, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Parent " );
                //mypage1.Add(ptc);


                //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                //                              new PdfArea(mydoc, line1, 880, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Student " );
                //mypage2.Add(ptc);




                if (ddledu.SelectedItem.Text.ToUpper() == "PG")
                {
                    string qualifyingexam = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);
                    if (qualifyingexam.Trim() != "")
                    {
                        qualifyingexam = subjectcode(qualifyingexam);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Qualifying Examination Passed");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 310, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(qualifyingexam));
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name of the College");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 330, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["Institute_name"]));
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Location of the College");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 350, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["instaddress"]));
                    mypage1.Add(ptc);
                    string branchcode = Convert.ToString(ds.Tables[1].Rows[0]["course_code"]);
                    if (branchcode.Trim() != "")
                    {
                        branchcode = subjectcode(branchcode);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Mention Major");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 370, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(branchcode));
                    mypage1.Add(ptc);
                    string typeofmajor = Convert.ToString(ds.Tables[1].Rows[0]["type_major"]);
                    if (typeofmajor.Trim() == "1")
                    {
                        typeofmajor = "Single";
                    }
                    else if (typeofmajor.Trim() == "2")
                    {
                        typeofmajor = "Double";
                    }
                    else if (typeofmajor.Trim() == "3")
                    {
                        typeofmajor = "Triple";
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, line1, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Type of Major");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 390, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(typeofmajor));
                    mypage1.Add(ptc);
                    string typeofsemester = Convert.ToString(ds.Tables[1].Rows[0]["type_semester"]);
                    if (typeofsemester.Trim() == "True")
                    {
                        typeofsemester = "Semester";
                    }
                    else
                    {
                        typeofsemester = "Non Semester";
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, line1, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Type of Semester");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 410, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(typeofsemester));
                    mypage1.Add(ptc);
                    string mediumofstudy = Convert.ToString(ds.Tables[1].Rows[0]["medium"]);
                    if (mediumofstudy.Trim() != "")
                    {
                        mediumofstudy = subjectcode(mediumofstudy);
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, line1, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Medium of Study at UG level");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 430, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(mediumofstudy));
                    mypage1.Add(ptc);
                    string markgrade = Convert.ToString(ds.Tables[1].Rows[0]["isgrade"]);
                    if (markgrade.Trim() == "False")
                    {
                        markgrade = "Mark";
                    }
                    else
                    {
                        markgrade = "Grade";
                    }
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 450, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Marks/Grade");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 450, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(markgrade));
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, line1, y + 470, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Registration No as Mentioned on your Mark Sheet");
                    mypage1.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, line2, y + 470, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, ":  " + Convert.ToString(ds.Tables[1].Rows[0]["registration_no"]));
                    mypage1.Add(ptc);
                    string majorpercentage = Convert.ToString(ds.Tables[1].Rows[0]["major_percent"]);
                    string majoralliedpercentage = Convert.ToString(ds.Tables[1].Rows[0]["majorallied_percent"]);
                    string majoralliedpracticalspercentage = Convert.ToString(ds.Tables[1].Rows[0]["percentage"]);
                    DataTable data = new DataTable();
                    DataRow dr = null;
                    Hashtable hash = new Hashtable();
                    int count = 0;
                    string pgquery = "select psubjectno,subject_typeno,acual_marks,max_marks,pass_month,pass_year,semyear ,grade  from perv_marks_history where course_entno ='" + Convert.ToString(ds.Tables[1].Rows[0]["course_entno"]) + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(pgquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        data.Columns.Add("Sem", typeof(string));
                        //  data.Columns.Add("Sem/Year", typeof(string));
                        data.Columns.Add("Subject", typeof(string));
                        data.Columns.Add("Subject type", typeof(string));
                        data.Columns.Add("Marks", typeof(string));
                        data.Columns.Add("Month", typeof(string));
                        data.Columns.Add("Year", typeof(string));
                        data.Columns.Add("Maximum Marks", typeof(string));
                        int sno = 0;
                        for (int pg = 0; pg < ds.Tables[0].Rows.Count; pg++)
                        {
                            sno++;
                            string semyear = Convert.ToString(ds.Tables[0].Rows[pg]["semyear"]);
                            string subjectno = Convert.ToString(ds.Tables[0].Rows[pg]["psubjectno"]);
                            string subjecttypeno = Convert.ToString(ds.Tables[0].Rows[pg]["subject_typeno"]);
                            string actualmark = "";
                            if (markgrade.Trim() == "Mark")
                            {
                                actualmark = Convert.ToString(ds.Tables[0].Rows[pg]["acual_marks"]);
                            }
                            else if (markgrade.Trim() == "Grade")
                            {
                                actualmark = Convert.ToString(ds.Tables[0].Rows[pg]["grade"]);
                            }
                            string month = Convert.ToString(ds.Tables[0].Rows[pg]["pass_month"]);
                            string year = Convert.ToString(ds.Tables[0].Rows[pg]["pass_year"]);
                            // string noofattenm = Convert.ToString(ds.Tables[0].Rows[pg]["noofattempt"]);
                            string maxmark = Convert.ToString(ds.Tables[0].Rows[pg]["max_marks"]);
                            dr = data.NewRow();
                            dr[0] = Convert.ToString(semyear);
                            string subject = subjectcode(subjectno);
                            dr[1] = Convert.ToString(subject);
                            string typesub = subjectcode(subjecttypeno);
                            dr[2] = Convert.ToString(typesub);
                            dr[3] = Convert.ToString(actualmark);
                            dr[4] = Convert.ToString(month);
                            dr[5] = Convert.ToString(year);
                            dr[6] = Convert.ToString(maxmark);
                            data.Rows.Add(dr);
                        }
                    }
                    count = data.Rows.Count;
                    if (count < 8)
                    {
                        Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2.VisibleHeaders = false;
                        table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table2.Columns[0].SetWidth(100);
                        table2.Columns[1].SetWidth(100);
                        table2.Columns[2].SetWidth(100);
                        table2.Columns[3].SetWidth(100);
                        table2.Columns[4].SetWidth(100);
                        table2.Columns[5].SetWidth(100);
                        table2.Columns[6].SetWidth(100);
                        table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                        table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 0).SetContent("Sem/Year");
                        table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 1).SetContent("Subject");
                        table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 2).SetContent("Type of Subject");
                        if (markgrade.Trim() == "Mark")
                        {
                            table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 3).SetContent("Mark");
                        }
                        if (markgrade.Trim() == "Grade")
                        {
                            table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 3).SetContent("Grade");
                        }
                        table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 4).SetContent("Month");
                        table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 5).SetContent("Year");
                        table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 6).SetContent("Maximun Marks");
                        for (int add = 0; add < data.Rows.Count; add++)
                        {
                            table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(add + 1, 0).SetContent(Convert.ToString(data.Rows[add]["Sem"]));
                            table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 1).SetContent(Convert.ToString(Convert.ToString(data.Rows[add]["Subject"])));
                            table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 2).SetContent(Convert.ToString(data.Rows[add]["Subject type"]));
                            // Month.First().ToString().ToUpper() + Month.Substring(1)
                            table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 3).SetContent(Convert.ToString(data.Rows[add]["Marks"]));
                            table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 4).SetContent(Convert.ToString(data.Rows[add]["Month"]));
                            table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 5).SetContent(Convert.ToString(data.Rows[add]["Year"]));
                            table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 6).SetContent(Convert.ToString(data.Rows[add]["Maximum Marks"]));
                        }
                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 600, 550, 550));
                        mypage1.Add(myprov_pdfpage1);
                        if (markgrade.Trim() == "Mark")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, line1, 750, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage of marks in all subjects (Language/major/Allied/Ancillary/Elective inclusive of Theory and Practical  : " + Convert.ToString(majoralliedpracticalspercentage) + "");
                            mypage1.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, line1, 770, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total % of Marks in Major subjects alone (Including theory & Practicals)  : " + Convert.ToString(majorpercentage) + "");
                            mypage1.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, 790, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals  : " + Convert.ToString(majoralliedpercentage) + "");
                            mypage1.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, line1, 820, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Cut Off Mark :  " + Convert.ToString(Cut_Of_Mark));
                            mypage1.Add(ptc);
                        }
                    }
                    else
                    {
                        dummyflage = true;
                        Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2 = mydoc.NewTable(Fontsmall, count + 1, 7, 1);
                        table2.VisibleHeaders = false;
                        table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table2.Columns[0].SetWidth(100);
                        table2.Columns[1].SetWidth(100);
                        table2.Columns[2].SetWidth(100);
                        table2.Columns[3].SetWidth(100);
                        table2.Columns[4].SetWidth(100);
                        table2.Columns[5].SetWidth(100);
                        table2.Columns[6].SetWidth(100);
                        table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                        table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 0).SetContent("Sem/Year");
                        table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 1).SetContent("Subject");
                        table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 2).SetContent("Type of Subject");
                        if (markgrade.Trim() == "Mark")
                        {
                            table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 3).SetContent("Mark");
                        }
                        if (markgrade.Trim() == "Grade")
                        {
                            table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 3).SetContent("Grade");
                        }
                        table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 4).SetContent("Month");
                        table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 5).SetContent("Year");
                        table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 6).SetContent("Maximun Marks");
                        for (int add = 0; add < data.Rows.Count; add++)
                        {
                            table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(add + 1, 0).SetContent(Convert.ToString(data.Rows[add]["Sem"]));
                            table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 1).SetContent(Convert.ToString(Convert.ToString(data.Rows[add]["Subject"])));
                            table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 2).SetContent(Convert.ToString(data.Rows[add]["Subject type"]));
                            // Month.First().ToString().ToUpper() + Month.Substring(1)
                            table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 3).SetContent(Convert.ToString(data.Rows[add]["Marks"]));
                            table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 4).SetContent(Convert.ToString(data.Rows[add]["Month"]));
                            table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 5).SetContent(Convert.ToString(data.Rows[add]["Year"]));
                            table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(add + 1, 6).SetContent(Convert.ToString(data.Rows[add]["Maximum Marks"]));
                        }
                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 40, 550, 700));
                        mypage2.Add(myprov_pdfpage1);
                        if (markgrade.Trim() == "Mark")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, line1, 750, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage of marks in all subjects (Language/major/Allied/Ancillary/Elective inclusive of Theory and Practical  : " + Convert.ToString(majoralliedpracticalspercentage) + "");
                            mypage2.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, line1, 770, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total % of Marks in Major subjects alone (Including theory & Practicals)  : " + Convert.ToString(majorpercentage) + "");
                            mypage2.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, 790, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals  : " + Convert.ToString(majoralliedpercentage) + "");
                            mypage2.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, line1, 820, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Cut Off Mark :  " + Convert.ToString(Cut_Of_Mark));
                            mypage2.Add(ptc);
                        }
                    }
                }



                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                              new PdfArea(mydoc, line1, 840, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "i.	All the original certificates should be submitted to the college immediately after receiving from the school.  Sufficient number of copies of the certificates may be taken before submitting the same. ");
                mypage1.Add(ptc);


                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, line1, 860, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "ii.	I, hereby, confirm you that as I have joined in management quota, I will not attend the Tamilnadu Engineering Admission Counselling 2018. ");
                mypage1.Add(ptc);

                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                              new PdfArea(mydoc, line1, 880, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Parent ");
                mypage1.Add(ptc);

                mypage1.Add(ptc);
                mypage.SaveToDocument();
                mypage1.SaveToDocument();
                if (dummyflage == true)
                {
                    mypage2.SaveToDocument();
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Application.pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }

            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
            //                                    new PdfArea(mydoc, line1, 840, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "i.	All the original certificates should be submitted to the college immediately after receiving from the school.  Sufficient number of copies of the certificates may be taken before submitting the same. " );
            //mypage1.Add(ptc);


            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
            //                                new PdfArea(mydoc, line1, 860, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "ii.	I, hereby, confirm you that as I have joined in management quota, I will not attend the Tamilnadu Engineering Admission Counselling 2018. " );
            //mypage1.Add(ptc);

            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
            //                              new PdfArea(mydoc, line1, 880, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Parent " );
            //mypage1.Add(ptc);


            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
            //                              new PdfArea(mydoc, line1, 880, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Student " );
            //mypage2.Add(ptc);

        }



        catch
        { }
    }
    protected void cbpersonal_Changed(object sender, EventArgs e)
    {
        if (cbpersonal.Checked == true)
        {
            panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
            Button6.Visible = true;
            if (Convert.ToString(ViewState["applicationviewformatset"]) == "0")
            {
                coursedetails.Visible = true;
                ugtotaldiv.Visible = true;
                Academicinfo.Visible = true;
            }
            else if (Convert.ToString(ViewState["applicationviewformatset"]) == "1")
            {
                coursedetails.Visible = false;
                ugtotaldiv.Visible = false;
                Academicinfo.Visible = false;
                licet_print_div.Visible = true;
            }
        }
        else
        {
            panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
            coursedetails.Visible = false;
            ugtotaldiv.Visible = false;
            Academicinfo.Visible = false;
            Button6.Visible = false;
            licet_print_div.Visible = false;
        }
    }

    protected void btnUpdDegDet_Click(object sender, EventArgs e)
    {
        try
        {
            int MyCount = 0;
            string headerfk = "";
            string leadgerfk = "";
            double feeamount = 0;
            double deduct = 0;
            string deductrea = "";
            double totalamount = 0;
            string refund = "";
            string feecatg = "";
            double finamount = 0;
            string paymode = "";
            ListItem MyList = new ListItem();
            string textcode = string.Empty;
            string collegecode = string.Empty;
            if (Session["studclgcode"] != null)
                collegecode = Convert.ToString(Session["studclgcode"]);
            //   activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            // activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
            string OldBatch = Convert.ToString(txt_OldBatch.Text);
            string OldAppFormNo = Convert.ToString(txt_OldApplNo.Text);
            string OldDegCode = Convert.ToString(Session["OldDegCode"]);
            string OldSeatType = Convert.ToString(Session["OldSeatType"]);
            string DegCode = Convert.ToString(upd_ddlDegree.SelectedItem.Value);
            string SeatType = Convert.ToString(upd_ddlSeatType.SelectedItem.Value);
            string Batch_Year = Convert.ToString(upd_ddlBatch.SelectedItem.Text);
            string app_no = string.Empty;
            if (Session["pdfapp_no"] != null)
                app_no = Convert.ToString(Session["pdfapp_no"]);
            //= Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
            string getfinid = d2.getCurrentFinanceYear(usercode, collegecode);
            string includeMulsem = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IncludeMultipleTermSettings' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");//26.01.18 usercode change user_code
            if (includeMulsem == "1")
            {
                string MulsemCode = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='SelectedMultipleFeecategoryCode' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");//26.01.18 usercode change user_code
                textcode = MulsemCode != "0" ? MulsemCode : "0";
            }
            if (textcode == "0" || string.IsNullOrEmpty(textcode))
            {
                string sem = getFeecategory(app_no);
                MyList = getFeecategoryNEW(sem, collegecode);
                textcode = MyList.Value;
            }
            string InsQ = "";
            if (chkFeesUpd.Checked == true)
            {
                InsQ = "update applyn set degree_code ='" + DegCode + "' ,batch_year ='" + Batch_Year + "' , seattype='" + SeatType + "' where app_no='" + app_no + "' and app_formno='" + OldAppFormNo + "' and batch_year ='" + OldBatch + "' and degree_code ='" + OldDegCode + "' and seattype ='" + OldSeatType + "'";
                int UpdCount = d2.update_method_wo_parameter(InsQ, "Text");
                if (UpdCount > 0)
                    MyCount++;
                if (textcode != "0" && getfinid != "" && getfinid != "0")
                {
                    string[] splcode = textcode.Split(',');
                    for (int row = 0; row < splcode.Length; row++)
                    {
                        textcode = Convert.ToString(splcode[row]);
                        string qur = "select LedgerFK,HeaderFK,PayMode,FeeAmount,deductAmout,DeductReason,TotalAmount,RefundAmount,FeeCategory,FineAmount from FT_FeeAllotDegree where DegreeCode='" + DegCode + "' and BatchYear ='" + Batch_Year + "' and SeatType ='" + SeatType + "' and FeeCategory ='" + textcode + "' and FinYearFK ='" + getfinid + "'";
                        ds = d2.select_method_wo_parameter(qur, "text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {
                                headerfk = Convert.ToString(ds.Tables[0].Rows[k]["HeaderFK"]);
                                leadgerfk = Convert.ToString(ds.Tables[0].Rows[k]["LedgerFK"]).Trim();
                                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["FeeAmount"]), out feeamount);
                                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["deductAmout"]), out deduct);
                                deductrea = Convert.ToString(ds.Tables[0].Rows[k]["DeductReason"]);
                                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["TotalAmount"]), out totalamount);
                                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["FineAmount"]), out finamount);
                                refund = Convert.ToString(ds.Tables[0].Rows[k]["RefundAmount"]);
                                feecatg = Convert.ToString(ds.Tables[0].Rows[k]["FeeCategory"]);
                                paymode = Convert.ToString(ds.Tables[0].Rows[k]["PayMode"]);
                                string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + textcode + "')  and App_No in('" + app_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeamount + "',PaidAmount='0' ,DeductAmout='" + deduct + "',DeductReason='" + deductrea + "',FromGovtAmt='0',TotalAmount='" + totalamount + "',RefundAmount='" + refund + "',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='" + paymode + "',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='" + finamount + "',BalAmount='" + totalamount + "' where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "') and App_No in('" + app_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no + ",'" + leadgerfk + "','" + headerfk + "','" + feeamount + "','" + deduct + "'," + deductrea + ",'0','" + totalamount + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + totalamount + "','" + getfinid + "')";
                                int a = d2.update_method_wo_parameter(insupdquery, "text");
                                if (a > 0)
                                    MyCount++;
                            }
                        }
                    }
                }
            }
            else
            {
                InsQ = "update applyn set degree_code ='" + DegCode + "' ,batch_year ='" + Batch_Year + "' , seattype='" + SeatType + "' where app_no='" + app_no + "' and app_formno='" + OldAppFormNo + "' and batch_year ='" + OldBatch + "' and degree_code ='" + OldDegCode + "' and seattype ='" + OldSeatType + "'";
                int UpdCount = d2.update_method_wo_parameter(InsQ, "Text");
                if (UpdCount > 0)
                    MyCount++;
            }
            if (MyCount > 0)
            {
                //poperrjs.Visible = true;
                //  errorspan.InnerHtml = "Degree Details Updated Successfully!";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Degree Details Updated Successfully!')", true);
                Session["OldDegCode"] = "";
                Session["OldSeatType"] = "";
            }
        }
        catch { }
    }
    private void UpdBindBatch()
    {
        upd_ddlBatch.Items.Clear();
        ds = d2.BindBatch();
        if (ds.Tables[0].Rows.Count > 0)
        {
            upd_ddlBatch.DataSource = ds;
            upd_ddlBatch.DataTextField = "batch_year";
            upd_ddlBatch.DataValueField = "batch_year";
            upd_ddlBatch.DataBind();
        }
    }
    private void UpdBindDegree(string collegecode)
    {
        try
        {
            string query = "";
            string edulvl = "";
            upd_ddlDegree.Items.Clear();
            if (ddledu.SelectedItem.Text == "--Select--")
            {
                query = "select distinct d.Course_Id,c.Course_Name+'-'+dt.Dept_Name as coursename,d.degree_code from Degree d,course c,department dt where dt.Dept_Code=d.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + collegecode + "'";
            }
            else
            {
                edulvl = Convert.ToString(ddledu.SelectedItem.Value);
                query = "select distinct d.Course_Id,c.Course_Name+'-'+dt.Dept_Name as coursename,d.degree_code from Degree d,course c,department dt where dt.Dept_Code=d.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + collegecode + "' and Edu_Level in('" + edulvl + "')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                upd_ddlDegree.DataSource = ds;
                upd_ddlDegree.DataTextField = "coursename";
                upd_ddlDegree.DataValueField = "degree_code";
                upd_ddlDegree.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    private void UpdBindSeatType(string collegecode)
    {
        try
        {
            upd_ddlSeatType.Items.Clear();
            string deptquery = "select distinct TextVal,TextCode  from TextValTable where TextCriteria ='Seat' and college_code=" + collegecode + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                upd_ddlSeatType.DataSource = ds;
                upd_ddlSeatType.DataTextField = "TextVal";
                upd_ddlSeatType.DataValueField = "TextCode";
                upd_ddlSeatType.DataBind();
            }
        }
        catch { }
    }
    public string subjectcode(string textcri)
    {
        string subjec_no = "";
        try
        {
            string select_subno = Convert.ToString(d2.GetFunction("select TextVal from textvaltable where TextCode ='" + textcri + "' and college_code ='" + Session["collegecode"].ToString() + "' "));
            if (!string.IsNullOrEmpty(select_subno) && select_subno != "0")
                subjec_no = select_subno;
        }
        catch
        { }
        return subjec_no;
    }
    private ListItem getFeecategoryNEW(string Sem, string college_code)
    {
        //if (ddl_collegename.Items.Count > 0)
        //{
        //    college_code = Convert.ToString(ddl_collegename.SelectedItem.Value);
        //}
        //if (imgdiv2.Visible && chkIsColDegChange.Checked)
        //{
        //    college_code = Convert.ToString(ddlColChangeDeg.SelectedItem.Value);
        //}
        string val = string.Empty;
        string code = string.Empty;
        ListItem feeCategory = new ListItem();
        string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + college_code + "'");
        DataSet dsFeecat = new DataSet();
        if (linkvalue == "0")
        {
            dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + Sem + " Semester' and college_code=" + college_code + "", "Text");
        }
        else if (linkvalue == "1")
        {
            string year = newfunction(Sem);
            dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + year + " Year' and college_code=" + college_code + "", "Text");
        }
        else if (linkvalue == "2")
        {
            //string term = newfunction(Sem);
            //dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = 'Term " + term + "' and college_code=" + college_code + "", "Text");
            string term = newfunction(Sem);
            dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + Sem + " Semester' or textval = 'Term " + term + "' and college_code=" + college_code + "", "Text");
        }
        if (linkvalue == "1")
        {
            if (dsFeecat.Tables.Count > 0 && dsFeecat.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFeecat.Tables[0].Rows.Count; i++)
                {
                    string feeval = Convert.ToString(dsFeecat.Tables[0].Rows[i]["textval"]);
                    string feecode = Convert.ToString(dsFeecat.Tables[0].Rows[i]["TextCode"]);
                    if (val == "")
                        val = feeval;
                    else
                        val += "'" + "," + "'" + feeval;
                    if (code == "")
                        code = feecode;
                    else
                        code += "'" + "," + "'" + feecode;

                }
                feeCategory.Text = val;
                feeCategory.Value = code;
            }
            else
            {
                feeCategory.Text = " ";
                feeCategory.Value = "-1";
            }

        }
        else
        {
            if (dsFeecat.Tables.Count > 0 && dsFeecat.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFeecat.Tables[0].Rows.Count; i++)
                {
                    string feeval = Convert.ToString(dsFeecat.Tables[0].Rows[i]["textval"]);
                    string feecode = Convert.ToString(dsFeecat.Tables[0].Rows[i]["TextCode"]);
                    if (val == "")
                        val = feeval;
                    else
                        val += "'" + "," + "'" + feeval;
                    if (code == "")
                        code = feecode;
                    else
                        code += "'" + "," + "'" + feecode;

                }
                feeCategory.Text = val;
                feeCategory.Value = code;
                //feeCategory.Text = Convert.ToString(dsFeecat.Tables[0].Rows[0]["textval"]);
                //feeCategory.Value = Convert.ToString(dsFeecat.Tables[0].Rows[0]["TextCode"]);
            }
            else
            {
                feeCategory.Text = " ";
                feeCategory.Value = "-1";
            }
        }
        return feeCategory;
    }

    public ListItem loadFeecategory(string collegecode, string usercode, ref string linkName)
    {
        ListItem dsset = new ListItem();
        try
        {

            //string linkName = string.Empty;
            string linkValue = string.Empty;
            string SelectQ = string.Empty;
            string strVal = string.Empty;
            linkValue = "select distinct LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code in('" + collegecode + "') Order By LinkValue";
            DataSet dsVal = d2.select_method_wo_parameter(linkValue, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    double linkVal = 0;
                    double.TryParse(Convert.ToString(dsVal.Tables[0].Rows[row]["LinkValue"]), out linkVal);
                    switch (Convert.ToString(linkVal))
                    {
                        case "0":
                            strVal = " textval like '%Semester'";
                            linkName = "Semester";
                            break;
                        case "1":
                            if (strVal == string.Empty)
                            {
                                strVal = " textval like '%Year'";
                                linkName = "Year";
                            }

                            else
                            {
                                strVal += " or textval like '%Year'";
                                linkName = "Year";
                            }
                            break;
                        case "2":
                            if (strVal == string.Empty)
                            {
                                strVal = "  textval like  '%Term%'";
                                linkName = "Term";
                            }
                            else
                            {
                                strVal += " or textval like '%Term%'";
                                linkName = "Term";
                            }
                            break;
                        default:
                            strVal = " textval like '%Semester'";
                            linkName = "Semester";
                            break;
                    }
                }
            }
            SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (" + strVal + ") and textval not like '-1%' and college_code in('" + collegecode + "') order by len(textval),textval asc";
            DataSet dsFeecat = d2.select_method_wo_parameter(SelectQ, "Text");
            string sem = string.Empty;
            string val = string.Empty;
            if (dsFeecat.Tables.Count > 0 && dsFeecat.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dsFeecat.Tables[0].Rows.Count; j++)
                {
                    string texval = Convert.ToString(dsFeecat.Tables[0].Rows[j]["textval"]);
                    string textc = Convert.ToString(dsFeecat.Tables[0].Rows[j]["TextCode"]);
                    if (sem == "")
                        sem = texval;
                    else
                        sem = sem + "','" + texval;
                    if (val == "")
                        val = textc;
                    else
                        val = val + "','" + textc;

                }
                dsset.Text = sem;
                dsset.Value = val;
            }
            else
            {
                dsset.Text = " ";
                dsset.Value = "-1";
            }
            return dsset;

        }
        catch { }
        return dsset;

    }//added by abarna
    public string newfunction(string val)
    {
        string value = "";
        if (val.Trim() == "1" || val.Trim() == "2")
        {
            value = "1";
        }
        if (val.Trim() == "3" || val.Trim() == "4")
        {
            value = "2";
        }
        if (val.Trim() == "5" || val.Trim() == "6")
        {
            value = "3";
        }
        if (val.Trim() == "7" || val.Trim() == "8")
        {
            value = "4";
        }
        if (val.Trim() == "9" || val.Trim() == "10")
        {
            value = "5";
        }
        return value;
    }
    protected void cbCounselling_CheckedChange(object sender, EventArgs e)
    {
        txtCounsellingNo.Text = "";
        txtCounsellingDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (cbCounselling.Checked == false)
        {
            txtCounsellingNo.Enabled = false;
            txtCounsellingDt.Enabled = false;
        }
        else
        {
            txtCounsellingNo.Enabled = true;
            txtCounsellingDt.Enabled = true;
        }
        ViewUpdateDiv();
    }
    public void ViewUpdateDiv()
    {
        //photo_div.Visible = true;
        if (cbpersonal.Checked == true)
        {
            panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
            coursedetails.Visible = true;
            ugtotaldiv.Visible = true;
            Academicinfo.Visible = true;
            Button6.Visible = true;
        }
        else
        {
            panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
            coursedetails.Visible = false;
            ugtotaldiv.Visible = false;
            Academicinfo.Visible = false;
            Button6.Visible = false;
        }
    }
    protected void cbSame_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cbSame.Checked == true)
            {
                txt_rollno.Text = Convert.ToString(txt_AdmissionNo.Text);
                txt_rollno.Enabled = false;
            }
            else
            {
                txt_rollno.Text = "";
                txt_rollno.Enabled = true;
            }
            if (cbpersonal.Checked == true)
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = true;
                ugtotaldiv.Visible = true;
                Academicinfo.Visible = true;
                Button6.Visible = true;
            }
            else
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = false;
                ugtotaldiv.Visible = false;
                Academicinfo.Visible = false;
                Button6.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void ddlAdmissionStudType_IndexChange(object sender, EventArgs e)
    {
        cb_IncDayscMess.Checked = false;
        cb_IncDayscMess.Visible = false;
        if (ddlAdmissionStudType.SelectedIndex == 0)
        {
            cb_IncDayscMess.Visible = true;
            //rbldayScTrans.Visible = true;
            transport_div.Visible = true;
            rbldayScTrans_IndexChange(sender, e);
            //lblBoardPnt.Visible = false;
            //txtBoardPnt.Visible = false;
            Hostel_div.Visible = false;
        }
        else
        {
            transport_div.Visible = false;
            Hostel_div.Visible = true;
            ddlHosHostel_IndexChange(sender, e);
        }
        ViewUpdateDiv();
    }
    protected void rbldayScTrans_IndexChange(object sender, EventArgs e)
    {
        // lblBoardPnt.Text = "";
        lblBoardPnt.Visible = false;
        //txtBoardPnt.Text = "";
        ddl_boarding.Visible = false;
        if (rbldayScTrans.SelectedIndex == 1)
        {
            // lblBoardPnt.Text = "Boarding";
            lblBoardPnt.Visible = true;
            // txtBoardPnt.Text = "";
            ddl_boarding.Visible = true;
        }
        ViewUpdateDiv();
    }
    protected void ddlHosHostel_IndexChange(object sender, EventArgs e)
    {
        bindroomtype();
        ViewUpdateDiv();
    }
    protected void Link_Photo(object sender, EventArgs e)
    {
        try
        {
            photo_div.Visible = true;
            if (cbpersonal.Checked == true)
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = true;
                ugtotaldiv.Visible = true;
                Academicinfo.Visible = true;
                Button6.Visible = true;
            }
            else
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = false;
                ugtotaldiv.Visible = false;
                Academicinfo.Visible = false;
                Button6.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void btnUpdateInformation_Click(object sender, EventArgs e)
    {
        try
        {
            //code added by Idhris 28-05-2016
            string counsellingNo = string.Empty;
            DateTime dtCounselling = DateTime.Now.Date;
            if (cbCounselling.Checked)
            {
                counsellingNo = txtCounsellingNo.Text;
                string[] counsDtAr = txtCounsellingDt.Text.Split('/');
                if (counsDtAr.Length == 3)
                    dtCounselling = Convert.ToDateTime(counsDtAr[1] + "/" + counsDtAr[0] + "/" + counsDtAr[2]);
            }
            string daySchHost = ddlAdmissionStudType.SelectedIndex.ToString();//0-Day 1 - Hostel
            string hostelNo = string.Empty;
            string roomNo = string.Empty;
            string boarding = string.Empty;
            string transOwnIns = string.Empty;
            string collegecode = string.Empty;
            if (Session["studclgcode"] != null)
                collegecode = Convert.ToString(Session["studclgcode"]);
            string getfinid = d2.getCurrentFinanceYear(usercode, collegecode);
            if (ddlAdmissionStudType.SelectedIndex == 1)
            {
                hostelNo = ddlHosHostel.SelectedValue;
                roomNo = ddlHosRoom.SelectedValue;
                string Hostelfee = d2.GetFunction("select value from Master_Settings where settings ='HostelFeeAllot' and usercode ='" + usercode + "'");
                if (Hostelfee == "1")
                {
                    HostelRegistration(hostelNo, roomNo, collegecode, Convert.ToString(Session["pdfapp_no"]), "", "", usercode, "", "", true);
                    #region old
                    //string val = "";
                    //string header = d2.GetFunction("select hosteladmfeeheaderfk from HM_HostelMaster where hostelmasterpk='" + hostelNo + "'");
                    //string ledger = d2.GetFunction("select hosteladmfeeledgerfk from HM_HostelMaster where hostelmasterpk='" + hostelNo + "'");
                    //string roomcost = d2.GetFunction("select Room_Cost from RoomCost_Master where college_code='" + collegecode + "' and Room_Type='" + roomNo + "'");
                    //string renttype = d2.GetFunction("select Rent_Type from RoomCost_Master where college_code='" + collegecode + "' and Room_Type='" + roomNo + "'");
                    //if (renttype == "2")
                    //{
                    //    val = "1 Year";
                    //}
                    //else
                    //{
                    //    val = "1 Semester";
                    //}
                    //string textcode = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + val + "' and college_code='" + collegecode + "'");
                    //if (textcode != "" && textcode != "0")
                    //{
                    //    if (header != "0" && ledger != "0" && roomcost != "0")
                    //    {
                    //        string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "')  and App_No in('" + Session["pdfapp_no"] + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + roomcost + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + roomcost + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + roomcost + "' where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "') and App_No in('" + Session["pdfapp_no"] + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + Session["pdfapp_no"] + ",'" + ledger + "','" + header + "','" + roomcost + "','0','0','0','" + roomcost + "','0','0','','0','" + textcode + "','','0','','0','0','" + roomcost + "','" + getfinid + "')";
                    //        int a = d2.update_method_wo_parameter(insupdquery, "text");
                    //    }
                    //    else
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Kindly Allot The Fees Or Hostel Header and Ledger')", true);
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Kindly Allot The Fees')", true);
                    //    return;
                    //}
                    #endregion
                }
            }
            else
            {
                #region Include Day scholar mess fees -- 23-07-2016

                if (cb_IncDayscMess.Checked)
                {
                    string type = "";
                    string header = "";
                    string ledger = "";
                    string cost = "";
                    string value = lblCurSemDet.Text.Trim();
                    #region Sem Year Feecat
                    string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'").Trim();
                    if (linkvalue == "0")
                    {
                        value += " Semester";
                    }
                    else
                    {
                        //value = returnYearforSem(value) + " Year";
                    }
                    string textcode = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + value + "' and college_code='" + collegecode + "'").Trim();
                    #endregion
                    string ledheadamt = d2.GetFunction("select LinkValue from InsSettings where LinkName='DayScholarStudentMessSetting'  and college_code ='" + collegecode + "'");
                    string[] spl = ledheadamt.Split(',');
                    if (spl.Length == 3)
                    {
                        header = spl[0];
                        ledger = spl[1];
                        cost = spl[2];
                        if (textcode != "" && textcode != "0" && header != "0" && ledger != "0" && cost != "0")
                        {
                            string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "')  and App_No in('" + Session["pdfapp_no"] + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + cost + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + cost + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + cost + "' where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "') and App_No in('" + Session["pdfapp_no"] + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + Session["pdfapp_no"] + ",'" + ledger + "','" + header + "','" + cost + "','0','0','0','" + cost + "','0','0','','0','" + textcode + "','','0','','0','0','" + cost + "','" + getfinid + "')";
                            int a = d2.update_method_wo_parameter(insupdquery, "text");
                        }
                    }
                }
                #endregion
                if (rbldayScTrans.SelectedItem.Value == "1")
                {
                    #region Transport Allot
                    bool insert = false;
                    string set = " select value from Master_Settings where settings in('TransportFeeAllot') and usercode ='" + usercode + "'";
                    set += " select LinkValue from New_InsSettings where LinkName='TransportLedgerValue' and user_code ='" + usercode + "' and college_code='" + collegecode + "'";
                    set += " select value  from Master_Settings where settings ='TransportFeeAllotmentSettings' and usercode ='" + usercode + "'";
                    DataSet setting = d2.select_method_wo_parameter(set, "text");
                    if (getfinid.Trim() != "" && getfinid.Trim() != "0")
                    {
                        if (setting.Tables[0].Rows.Count > 0 && setting.Tables[1].Rows.Count > 0 && setting.Tables[2].Rows.Count > 0 && setting.Tables != null)
                        {
                            string transfee = Convert.ToString(setting.Tables[0].Rows[0]["value"]);
                            string[] valtranc = transfee.Split('/');
                            if (valtranc[0] == "1")
                            {
                                string cost = ""; string header = "";
                                string value = ""; string ledger = ""; string type = "";
                                if (setting.Tables[1].Rows.Count > 0)
                                {
                                    string ledhead = Convert.ToString(setting.Tables[1].Rows[0]["LinkValue"]);
                                    string[] spl = ledhead.Split(',');
                                    header = spl[0];
                                    ledger = spl[1];
                                    boarding = ddl_boarding.SelectedItem.Value;
                                    if (setting.Tables[2].Rows.Count > 0)
                                    {
                                        string Transportsettings = Convert.ToString(setting.Tables[2].Rows[0]["value"]);
                                        if (Transportsettings.Trim() != "" && Transportsettings.Trim() != "0")
                                        {
                                            string[] transtype = Transportsettings.Split('-');
                                            string paytype = "";
                                            if (transtype[0] == "1")
                                                paytype = " and payType ='Semester'";
                                            else if (transtype[0] == "2")
                                                paytype = " and payType ='Yearly'";
                                            else
                                                paytype = " and payType ='Monthly'";
                                            string values = d2.GetFunction(" select convert(varchar, isnull(cost,0))+'$'+convert(varchar,isnull(payType,0)) from feeinfo where StrtPlace ='" + boarding + "' " + paytype + " ");
                                            string[] costandpaytype = values.Split('$');
                                            if (costandpaytype.Length == 2)
                                            {
                                                cost = Convert.ToString(costandpaytype[0]);
                                                value = Convert.ToString(costandpaytype[1]);
                                                type = "Semester";
                                            }
                                            string val = "";
                                            if (value == "Yearly")
                                            {
                                                val = "1 Year";
                                            }
                                            else if (value == "Semester")
                                            {
                                                val = "1 Semester";
                                            }
                                            else if (value == "Monthly")
                                            {
                                                string settingquery = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                                                if (settingquery.Trim() != "")
                                                {
                                                    if (settingquery == "0")
                                                    {
                                                        val = "1 Semester";
                                                    }
                                                    else if (settingquery == "1")
                                                    {
                                                        val = "1 Year";
                                                    }
                                                }
                                            }
                                            if (val.Trim() != "")
                                            {
                                                string textcode = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + val + "' and college_code='" + collegecode + "'");
                                                if (transtype[0] != "3")
                                                {
                                                    #region year and semesterwise
                                                    if (textcode != "" && textcode != "0")
                                                    {
                                                        if (header != "0" && ledger != "0" && cost != "0")
                                                        {
                                                            string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "')  and App_No in('" + Convert.ToString(Session["pdfapp_no"]) + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + cost + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + cost + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + cost + "' where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "') and App_No in('" + Convert.ToString(Session["pdfapp_no"]) + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout, DeductReason,FromGovtAmt, TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + Convert.ToString(Session["pdfapp_no"]) + ",'" + ledger + "','" + header + "','" + cost + "','0','0','0','" + cost + "','0','0','','0','" + textcode + "','','0','','0','0','" + cost + "','" + getfinid + "')";
                                                            insupdquery += "update registration set Boarding='" + boarding + "' where app_no='" + Convert.ToString(Session["pdfapp_no"]) + "'";
                                                            int u = d2.update_method_wo_parameter(insupdquery, "text");
                                                            if (u != 0)
                                                                insert = true;
                                                        }
                                                        else
                                                        {
                                                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Kindly Allot The Fees", true);
                                                            return;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Kindly Allot The Feecatagory", true);
                                                        return;
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Monthwise
                                                    double calcost = 0;
                                                    string mnthamt = "";
                                                    string[] yearcal = transtype[1].Split(';');
                                                    string[] monthcal = yearcal[0].Split(',');
                                                    for (int u = 0; u < monthcal.Length; u++)
                                                    {
                                                        string year = yearcal[1];
                                                        if (mnthamt == "")
                                                            mnthamt = monthcal[u] + ":" + year + ":" + cost;
                                                        else
                                                            mnthamt = mnthamt + "," + monthcal[u] + ":" + year + ":" + cost;
                                                        calcost = calcost + Convert.ToDouble(cost);
                                                    }
                                                    string querystu1 = " if exists (select * from FT_FeeAllot where App_No ='" + Convert.ToString(Session["pdfapp_no"]) + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "' ) update FT_FeeAllot set FeeAmount='" + calcost + "',TotalAmount ='" + calcost + "' ,BalAmount ='" + calcost + "', FeeAmountMonthly='" + mnthamt + "'  where App_No ='" + Convert.ToString(Session["pdfapp_no"]) + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "'  else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt,FeeAmountMonthly)  values ('" + Convert.ToString(Session["pdfapp_no"]) + "','" + ledger + "','" + header + "','" + getfinid + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + calcost + "','" + textcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "',0,0,'" + calcost + "','" + calcost + "','1','1',0,0,'" + mnthamt + "')";
                                                    querystu1 += "update registration set Boarding='" + boarding + "' where app_no='" + Convert.ToString(Session["pdfapp_no"]) + "'";
                                                    int uh = d2.update_method_wo_parameter(querystu1, "text");
                                                    if (uh != 0)
                                                        insert = true;
                                                    string allotpk = d2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + Convert.ToString(Session["pdfapp_no"]) + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "'");
                                                    if (allotpk != "")
                                                    {
                                                        for (int u = 0; u < monthcal.Length; u++)
                                                        {
                                                            string year = yearcal[1];
                                                            string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + monthcal[u] + "' and AllotYear='" + year + "' and FinYearFK='" + getfinid + "')update FT_FeeallotMonthly set AllotAmount=AllotAmount+'" + cost + "',BalAmount=BalAmount+'" + cost + "' where FeeAllotPK='" + allotpk + "' and AllotMonth='" + monthcal[u] + "' and AllotYear='" + year + "' and FinYearFK='" + getfinid + "' else insert into FT_FeeallotMonthly (FeeAllotPK,AllotMonth,AllotYear,AllotAmount,FinYearFK,BalAmount) values('" + allotpk + "','" + monthcal[u] + "','" + year + "','" + cost + "','" + getfinid + "','" + cost + "')";
                                                            int ins = d2.update_method_wo_parameter(InsertQ, "Text");
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                string appnumb = Convert.ToString(Convert.ToString(Session["pdfapp_no"]));
                                                travelAllotment(appnumb, type, collegecode);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Please set Boarding point fees settings", true);
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Please set Transport Fee Allotment settings", true);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Please set Transport FeeAllot settings", true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Please set Financial year settings", true);
                    }
                    #endregion
                    #region transportOld
                    /*
                    string type = "";
                    string header = "";
                    string ledger = "";
                    boarding = Convert.ToString(ddl_boarding.SelectedItem.Value).Trim();
                    //boarding = d2.GetFunction("select distinct Stage_id  from Stage_Master where Stage_Name='" + txtBoardPnt.Text + "' ");
                    transOwnIns = rbldayScTrans.SelectedIndex.ToString();
                    string transfee = d2.GetFunction("select value from Master_Settings where settings ='TransportFeeAllot' and usercode ='" + usercode + "'");
                    string[] valtranc = transfee.Split('/');
                    if (valtranc[0] == "1")
                    {
                        string cost = "";
                        string value = "";
                        string ledhead = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='TransportLedgerValue' and user_code ='" + usercode + "'");
                        string[] spl = ledhead.Split(',');
                        header = spl[0];
                        ledger = spl[1];
                        string Transportsettings = d2.GetFunction("select value  from Master_Settings where settings ='TransportFeeAllotmentSettings' and usercode ='" + usercode + "'");
                        if (Transportsettings.Trim() != "" && Transportsettings.Trim() != "0")
                        {
                            string[] transtype = Transportsettings.Split('-');
                            if (transtype[0] == "1")  // Modify by jairam 22-07-2016 ------ For New Transport Allotment 
                            {
                                //cost = d2.GetFunction("select cost from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Semester' and Fee_Code ='" + ledger + "' ");
                                //value = d2.GetFunction("select payType from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Semester' and Fee_Code ='" + ledger + "'");
                                cost = d2.GetFunction("select cost from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Semester' ");
                                value = d2.GetFunction("select payType from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Semester'");
                                type = "Semester";
                            }
                            else if (transtype[0] == "2")
                            {
                                //cost = d2.GetFunction("select cost from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Yearly' and Fee_Code ='" + ledger + "' ");
                                //value = d2.GetFunction("select payType from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Yearly' and Fee_Code ='" + ledger + "'");
                                cost = d2.GetFunction("select cost from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Yearly'");
                                value = d2.GetFunction("select payType from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Yearly'");
                                type = "Yearly";
                            }
                            else
                            {
                                //cost = d2.GetFunction("select cost from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Monthly' and Fee_Code ='" + ledger + "' ");
                                //value = d2.GetFunction("select payType from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Monthly' and Fee_Code ='" + ledger + "'");
                                cost = d2.GetFunction("select cost from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Monthly'");
                                value = d2.GetFunction("select payType from feeinfo where StrtPlace ='" + boarding + "'  and payType ='Monthly'");
                                type = "Monthly";
                            }
                            string val = "";
                            if (value == "Yearly")
                            {
                                // string year = returnYearforSem(Convert.ToString(lblCurSemDet.Text));
                                val = "" + year + " Year";
                            }
                            else if (value == "Semester")
                            {
                                val = "" + lblCurSemDet.Text + " Semester";
                            }
                            else if (value == "Monthly")
                            {
                                string settingquery = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and usercode ='" + usercode + "' and college_code ='" + collegecode + "'");
                                if (settingquery.Trim() != "")
                                {
                                    if (settingquery == "0")
                                    {
                                        val = "" + lblCurSemDet.Text + " Semester";
                                    }
                                    else if (settingquery == "1")
                                    {
                                        // string year = returnYearforSem(Convert.ToString(lblCurSemDet.Text));
                                        val = "" + year + " Year";
                                        //val = "" + lblCurSemDet.Text + " Year";
                                    }
                                }
                            }
                            string textcode = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + val + "' and college_code='" + collegecode + "'");
                            if (transtype[0] != "3")
                            {
                                if (textcode != "" && textcode != "0")
                                {
                                    if (header != "0" && ledger != "0" && cost != "0")
                                    {
                                        string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "')  and App_No in('" + Session["pdfapp_no"] + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + cost + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + cost + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + cost + "' where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + textcode + "') and App_No in('" + Session["pdfapp_no"] + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + Session["pdfapp_no"] + ",'" + ledger + "','" + header + "','" + cost + "','0','0','0','" + cost + "','0','0','','0','" + textcode + "','','0','','0','0','" + cost + "','" + getfinid + "')";
                                        int a = d2.update_method_wo_parameter(insupdquery, "text");
                                        string querystu = "update registration set Boarding='" + boarding + "' where app_no='" + Session["pdfapp_no"] + "'";
                                        int u = d2.update_method_wo_parameter(querystu, "text");
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Kindly Allot The Fees')", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Kindly Allot The Fees')", true);
                                    return;
                                }
                            }
                            else
                            {
                                double calcost = 0;
                                string mnthamt = "";
                                string[] yearcal = transtype[1].Split(';');
                                string[] monthcal = yearcal[0].Split(',');
                                for (int u = 0; u < monthcal.Length; u++)
                                {
                                    year = yearcal[1];
                                    if (mnthamt == "")
                                    {
                                        mnthamt = monthcal[u] + ":" + year + ":" + cost;
                                    }
                                    else
                                    {
                                        mnthamt = mnthamt + "," + monthcal[u] + ":" + year + ":" + cost;
                                    }
                                    calcost = calcost + Convert.ToDouble(cost);
                                }
                                string querystu1 = " if exists (select * from FT_FeeAllot where App_No ='" + Session["pdfapp_no"] + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "' ) update FT_FeeAllot set FeeAmount='" + calcost + "',TotalAmount ='" + calcost + "' ,BalAmount ='" + calcost + "', FeeAmountMonthly='" + mnthamt + "'  where App_No ='" + Session["pdfapp_no"] + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "'  else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt,FeeAmountMonthly)  values ('" + Session["pdfapp_no"] + "','" + ledger + "','" + header + "','" + getfinid + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + calcost + "','" + textcode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "',0,0,'" + calcost + "','" + calcost + "','1','1',0,0,'" + mnthamt + "')";
                                int iii = d2.update_method_wo_parameter(querystu1, "Text");
                                string querystu = "update registration set Boarding='" + boarding + "' where app_no='" + Session["pdfapp_no"] + "'";
                                int uh = d2.update_method_wo_parameter(querystu, "text");
                                string allotpk = d2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + Session["pdfapp_no"] + "' and LedgerFK='" + ledger + "' and HeaderFK='" + header + "' and FeeCategory ='" + textcode + "'");
                                if (allotpk != "")
                                {
                                    for (int u = 0; u < monthcal.Length; u++)
                                    {
                                        year = yearcal[1];
                                        string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + monthcal[u] + "' and AllotYear='" + year + "' and FinYearFK='" + getfinid + "')update FT_FeeallotMonthly set AllotAmount=AllotAmount+'" + cost + "',BalAmount=BalAmount+'" + cost + "' where FeeAllotPK='" + allotpk + "' and AllotMonth='" + monthcal[u] + "' and AllotYear='" + year + "' and FinYearFK='" + getfinid + "' else insert into FT_FeeallotMonthly (FeeAllotPK,AllotMonth,AllotYear,AllotAmount,FinYearFK,BalAmount) values('" + allotpk + "','" + monthcal[u] + "','" + year + "','" + cost + "','" + getfinid + "','" + cost + "')";
                                        int ins = d2.update_method_wo_parameter(InsertQ, "Text");
                                    }
                                }
                            }
                            string appnumb = Convert.ToString(Session["pdfapp_no"]);
                            travelAllotment(appnumb, type, collegecode);
                        }
                    }*/
                    #endregion
                }
            }
            if (cbpersonal.Checked == true)
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = true;
                ugtotaldiv.Visible = true;
                Academicinfo.Visible = true;
                Button6.Visible = true;
            }
            else
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = false;
                ugtotaldiv.Visible = false;
                Academicinfo.Visible = false;
                Button6.Visible = false;
            }
            int count = 0;
            string date = Convert.ToString(txt_AdmissionDate.Text);
            string[] splitdate = date.Split('/');
            DateTime newdate = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            string roll_no = Convert.ToString(txt_rollno.Text);
            string mode = "";
            if (rblModeDet.SelectedItem.Value == "1")
            {
                mode = "1";
            }
            else if (rblModeDet.SelectedItem.Value == "2")
            {
                mode = "2";
            }
            else
            {
                mode = "3";
            }
            string queury = " update Registration set Roll_Admit ='" + txt_AdmissionNo.Text + "' ,Adm_Date ='" + newdate.ToString("MM/dd/yyyy") + "',Stud_Type ='" + ddlAdmissionStudType.SelectedItem.Text + "',mode='" + mode + "' where App_No ='" + Session["pdfapp_no"] + "'";//,Roll_No ='" + roll_no + "'
            count = d2.update_method_wo_parameter(queury, "Text");
            if (count != 0)
            {
                string img = Convert.ToString(ViewState["stfimg"]);
                if (img.Trim() != "")
                {
                    FileStream fs = new FileStream(Server.MapPath("~/Student Photo/") + img,
              FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string stfphoto = Path.GetFileName("~/Student Photo/img");
                    string stdphotopath = Server.MapPath("~/Staff Photo/") + fileuploadbrowse.FileName;
                    string stdphotoext = System.IO.Path.GetExtension(fileuploadbrowse.FileName);
                    string insphoto = "if exists (select photo from StdPhoto where app_no='" + Session["pdfapp_no"] + "')";
                    insphoto = insphoto + " update StdPhoto set photo=@photo where app_no='" + Session["pdfapp_no"] + "'";
                    insphoto = insphoto + " else insert into StdPhoto (app_no,photo) values('" + Session["pdfapp_no"] + "',@photo)";
                    SqlCommand cmd = new SqlCommand(insphoto, con);
                    SqlParameter uploadedsubject_name = new SqlParameter("@photo", SqlDbType.Binary, bytes.Length);
                    uploadedsubject_name.Value = bytes;
                    cmd.Parameters.Add(uploadedsubject_name);
                    br.Close();
                    fs.Close();
                    con.Close();
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                // errorspan.InnerHtml = "Updated Successfully";
                // poperrjs.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "studadmissionselection");
        }
    }
    protected void btn_photoupload_OnClick(object sender, EventArgs e)
    {
        try
        {
            bool upflage = false;
            ViewState["stfimg"] = null;
            //string stfphoto = "";           
            //MemoryStream memoryStream = new MemoryStream();
            if (fileuploadbrowse.HasFile)
            {
                if (fileuploadbrowse.FileName.EndsWith(".jpg") || fileuploadbrowse.FileName.EndsWith(".JPG"))
                {
                    string stdphotopath = Server.MapPath("~/Student Photo/") + fileuploadbrowse.FileName;
                    string stdphotoext = System.IO.Path.GetExtension(fileuploadbrowse.FileName);
                    fileuploadbrowse.SaveAs(stdphotopath);
                    //string insphoto = "if exists (select photo from staffphoto where staff_code='" + scode + "')";
                    //insphoto = insphoto + " update staffphoto set photo=@photo where staff_code='" + scode + "'";
                    //insphoto = insphoto + " else insert into staffphoto (staff_code,photo) values('" + scode + "',@photo)";
                    // int fileSize = fileuploadbrowse.PostedFile.ContentLength;
                    // byte[] documentBinary = new byte[fileSize];
                    //// memoryStream.Read(documentBinary, 0, documentBinary.Length);
                    // memoryStream.Write(documentBinary, 0, documentBinary.Length);
                    // if (documentBinary.Length > 0)
                    // {
                    //     System.Drawing.Image imgx = System.Drawing.Image.FromStream((Stream)memoryStream, true, true);
                    //     System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                    //     thumb.Save(HttpContext.Current.Server.MapPath("~/Staff Photo/" + fileuploadbrowse.FileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                    // }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/Student Photo/" + fileuploadbrowse.FileName)))
                    {
                        StudentImage.ImageUrl = "~/Student Photo/" + fileuploadbrowse.FileName;
                    }
                    ViewState["stfimg"] = fileuploadbrowse.FileName;
                    //fileuploadbrowse.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                    //SqlCommand cmd = new SqlCommand(insphoto, con);
                    //SqlParameter uploadedsubject_name = new SqlParameter("@photo", SqlDbType.Binary, fileSize);
                    //uploadedsubject_name.Value = documentBinary;
                    //cmd.Parameters.Add(uploadedsubject_name);
                    //con.Close();
                    //con.Open();
                    //cmd.ExecuteNonQuery();
                    //con.Close();
                    //stf_img.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + scode;
                }
            }
            if (cbpersonal.Checked == true)
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = true;
                ugtotaldiv.Visible = true;
                Academicinfo.Visible = true;
                Button6.Visible = true;
            }
            else
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = false;
                ugtotaldiv.Visible = false;
                Academicinfo.Visible = false;
                Button6.Visible = false;
            }
            photo_div.Visible = false;
        }
        catch
        {
        }
    }
    protected void btn_uploadclose_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (cbpersonal.Checked == true)
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = true;
                ugtotaldiv.Visible = true;
                Academicinfo.Visible = true;
                Button6.Visible = true;
            }
            else
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = false;
                ugtotaldiv.Visible = false;
                Academicinfo.Visible = false;
                Button6.Visible = false;
            }
            photo_div.Visible = false;
        }
        catch
        {
        }
    }
    protected void btnFeeUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string collegecode = string.Empty;
            if (Session["studclgcode"] != null)
                collegecode = Convert.ToString(Session["studclgcode"]);
            string appNo = string.Empty;
            if (Session["pdfapp_no"] != null)
                appNo = Convert.ToString(Session["pdfapp_no"]);
            string degreecode = string.Empty;
            if (Session["studdegcode"] != null)
                degreecode = Convert.ToString(Session["studdegcode"]);

            string sem = getFeecategory(appNo);
            ListItem feecat = getFeecategoryNEW(sem, collegecode);
            string getfinid = d2.getCurrentFinanceYear(usercode, collegecode);
            string app_no = lblAppnoFee.Text;
            bool check = false; bool boolsave = false;
            if (app_no == string.Empty || app_no == "0")
                app_no = appNo;
            for (int gRow = 0; gRow < gridFeeDet.Rows.Count; gRow++)
            {
                Label hdrid = (Label)gridFeeDet.Rows[gRow].FindControl("lblAdmHeaderId");
                Label lgrid = (Label)gridFeeDet.Rows[gRow].FindControl("lblAdmLedgerId");
                TextBox feeamt = (TextBox)gridFeeDet.Rows[gRow].FindControl("txtAdmFeeAllot");
                TextBox dedamt = (TextBox)gridFeeDet.Rows[gRow].FindControl("txtAdmDeduc");
                Label dedrea = (Label)gridFeeDet.Rows[gRow].FindControl("lblAdmDedRes");
                Label totamt = (Label)gridFeeDet.Rows[gRow].FindControl("lblAdmFeeTotal");
                Label finamt = (Label)gridFeeDet.Rows[gRow].FindControl("lblAdmFine");
                Label paymo = (Label)gridFeeDet.Rows[gRow].FindControl("lblAdmPaymode");
                Label refamt = (Label)gridFeeDet.Rows[gRow].FindControl("lblAdmRefund");
                string headerfk = hdrid.Text;
                string leadgerfk = lgrid.Text;
                string feeamount = feeamt.Text;
                string deduct = dedamt.Text;
                string deductrea = dedrea.Text;
                string totalamount = totamt.Text;
                try
                {
                    totalamount = (Convert.ToDouble(feeamount) - Convert.ToDouble(deduct)).ToString();
                }
                catch { }
                string finamount = finamt.Text;
                string refund = refamt.Text;
                string feecatg = feecat.Value;
                string textcode = feecatg;
                string paymode = paymo.Text;
                check = false;
                if (ShowAllCb.Checked == true)
                {
                    if (totalamount.Trim() != "0" && feeamount.Trim() != "0")
                    {
                        check = true;
                    }
                }
                else
                {
                    check = true;
                }
                if (check == true)
                {
                    string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + textcode + "')  and App_No in('" + app_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeamount + "',DeductAmout='" + deduct + "',DeductReason='" + deductrea + "',FromGovtAmt='0',TotalAmount='" + totalamount + "',RefundAmount='" + refund + "',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='" + paymode + "',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='" + finamount + "',BalAmount=" + totalamount + " - isnull(PaidAmount,0) where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "') and App_No in('" + app_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no + ",'" + leadgerfk + "','" + headerfk + "','" + feeamount + "','" + deduct + "'," + deductrea + ",'0','" + totalamount + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + totalamount + "','" + getfinid + "')";
                    int res = d2.update_method_wo_parameter(insupdquery, "text");
                    string upChallanQ = "if exists (select ChallanNo  from ft_challandet where isnull(isconfirmed,0)=0 and feecategory=" + feecatg + " and app_no=" + app_no + " and Headerfk=" + headerfk + " and Ledgerfk=" + leadgerfk + " and Finyearfk=" + getfinid + ") update ft_challandet set TakenAmt=" + totalamount + ",FeeAmount=" + totalamount + "  where isnull(isconfirmed,0)=0 and feecategory=" + feecatg + " and app_no=" + app_no + " and Headerfk=" + headerfk + " and Ledgerfk=" + leadgerfk + " and Finyearfk=" + getfinid + " ";
                    int res2 = d2.update_method_wo_parameter(upChallanQ, "Text");
                    boolsave = true;
                }
            }
            string dayscholormess = "";
            if (cb_IncDayscMess.Checked == true)
            {
                dayscholormess = "1";
            }
            else
            {
                dayscholormess = "0";
            }
            string updatedayscholormess = " update Registration set IsdayscholorMess='" + dayscholormess + "' where App_No='" + app_no + "'";
            int reg3 = d2.update_method_wo_parameter(updatedayscholormess, "Text");

            if (boolsave)
            {
                boolfeeupdate = true; bindFeeLedgerGrid(appNo, collegecode, degreecode);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
            }
            else
            {
                boolfeeupdate = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Not Updated')", true);
            }
            panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
        }
        catch { }
    }
    protected void ShowAllCb_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            bindFeeLedgerGrid(Convert.ToString(Session["pdfapp_no"]), Convert.ToString(Session["studclgcode"]), Convert.ToString(Session["studdegcode"]));
            if (cbpersonal.Checked == true)
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = true;
                ugtotaldiv.Visible = true;
                Academicinfo.Visible = true;
                Button6.Visible = true;
            }
            else
            {
                panel4.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 500%; left: -8px; position: absolute; top: -9px; width: 101%; display: block;");
                coursedetails.Visible = false;
                ugtotaldiv.Visible = false;
                Academicinfo.Visible = false;
                Button6.Visible = false;
            }
        }
        catch
        {
        }
    }
    private void bindFeeLedgerGrid(string appNo, string collegecode, string degCode)
    {
        try
        {
            double allot = 0;
            double conses = 0;
            double total = 0;
            gridFeeDet.DataSource = null;
            gridFeeDet.DataBind();
            btnFeeUpdate.Visible = false;
            lblstudmsg.Visible = false;
            AddtionalInformationDiv.Visible = false;
            lblAppnoFee.Text = string.Empty;
            ShowAllCb.Visible = false;

            btnFeeUpdate.Visible = false;//barath 20.05.17
            divstuddt.Visible = false;
            trtotal.Visible = false;
            //string degCode = returnStudDeg(appNo);
            string MemType = d2.GetFunction("select case when sex='0' then '1' when sex ='1' then '2' end  from applyn where app_no ='" + appNo + "'");
            loadHostel(MemType);
            loadHostelRoom();
            string mode = d2.GetFunction("select mode from Registration where app_no='" + appNo + "'");
            if (string.IsNullOrEmpty(mode))
                mode = "1";
            if (mode == "1")
            {
                rblModeDet.SelectedIndex = rblModeDet.Items.IndexOf(rblModeDet.Items.FindByValue("1"));
            }
            else if (mode == "2")
            {
                rblModeDet.SelectedIndex = rblModeDet.Items.IndexOf(rblModeDet.Items.FindByValue("2"));
            }
            else if (mode == "3")
            {
                rblModeDet.SelectedIndex = rblModeDet.Items.IndexOf(rblModeDet.Items.FindByValue("3"));
            }
            //string collegecode = string.Empty;
            //if (Session["studclgcode"] != null)
            // collegecode = Convert.ToString(Session["studclgcode"]);
            //int rowIndex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
            //string degreeCode = string.Empty;
            //app_no = gridstud.Rows[rowIndex].Cells[gridstud.Rows[rowIndex].Cells.Count - 3].Text;
            //collegecode = gridstud.Rows[rowIndex].Cells[gridstud.Rows[rowIndex].Cells.Count - 2].Text;
            //degreeCode = gridstud.Rows[rowIndex].Cells[gridstud.Rows[rowIndex].Cells.Count - 1].Text;


            string sem = getFeecategory(appNo);
            ListItem feecat = getFeecategoryNEW(sem, collegecode);


            string finYearId = d2.getCurrentFinanceYear(usercode, collegecode);
            //string 
            string ledgerQ = "select ledgername,LedgerFK,a.HeaderFK,isnull(FeeAmount,0) as AllotAmount,isnull(DeductAmout,0) as Deduction,DeductReason,isnull(TotalAmount,0) as TotalAmount,isnull(RefundAmount,0) as RefundAmount,PayMode,isnull(FineAmount,0) as FineAmount from ft_feeallot a,fm_ledgermaster l where l.ledgerPk=a.ledgerfk and a.Headerfk=l.Headerfk and  app_no =" + appNo + " and FeeCategory ='" + feecat.Value + "' and FinYearFK ='" + finYearId + "'  and l.CollegeCode='" + collegecode + "'";
            if (ShowAllCb.Checked == true)
            {
                ledgerQ = ledgerQ + " union select ledgername,LedgerpK,HeaderFK,0 FeeAmount,0 deductAmout,0 DeductReason,0 TotalAmount,0 RefundAmount,0 PayMode,0 fineAmount  from FM_LedgerMaster where LedgerPK not in (select ledgerFk from ft_feeallot where app_no =" + appNo + " and FeeCategory ='" + feecat.Value + "' and FinYearFK ='" + finYearId + "' and CollegeCode='" + collegecode + "') and CollegeCode='" + collegecode + "'";
            }
            DataSet dsLedger = new DataSet();
            dsLedger = d2.select_method_wo_parameter(ledgerQ, "Text");
            if (dsLedger.Tables.Count == 0 || dsLedger.Tables[0].Rows.Count == 0)
            {
                string seattype = d2.GetFunction("select seattype from applyn where app_no =" + appNo + "").Trim();
                ledgerQ = "select ledgername,d.LedgerFk,isnull(FeeAmount,0) as AllotAmount,isnull(DeductAmout,0) as Deduction,isnull(TotalAmount,0) as TotalAmount,PayMode,d.HeaderFK,DeductReason,isnull(FineAmount,0) as FineAmount,isnull(RefundAmount,0) as RefundAmount from FT_FeeAllotDegree d,Fm_ledgermaster l where d.BatchYear ='" + Convert.ToString(ddl_batch.SelectedItem.Value) + "' and d.FeeCategory ='" + feecat.Value + "' and d.FinYearFK ='" + finYearId + "' and d.LedgerFk=l.ledgerpk  and DegreeCode='" + degCode + "'  and d.seattype='" + seattype + "' and l.CollegeCode='" + collegecode + "' ";
                if (ShowAllCb.Checked == true)
                {
                    ledgerQ = ledgerQ + " union select ledgername,LedgerpK,HeaderFK,0 FeeAmount,0 deductAmout,0 DeductReason,0 TotalAmount,0 RefundAmount,0 PayMode,0 fineAmount  from FM_LedgerMaster where LedgerPK not in (select ledgerFk from FT_FeeAllotDegree where BatchYear ='" + Convert.ToString(ddl_batch.SelectedItem.Value) + "' and DegreeCode='" + degCode + "' and seattype='" + seattype + "' and FeeCategory ='" + feecat.Value + "' and FinYearFK ='" + finYearId + "' and l.CollegeCode='" + collegecode + "' )";
                }
                dsLedger.Clear();
                dsLedger = d2.select_method_wo_parameter(ledgerQ, "Text");
            }
            if (dsLedger.Tables.Count > 0 && dsLedger.Tables[0].Rows.Count > 0)
            {
                DataTable dtledger = new DataTable();
                dtledger.Columns.Add("AdmLedger");
                dtledger.Columns.Add("AdmLedgerId");
                dtledger.Columns.Add("AdmHeaderId");
                dtledger.Columns.Add("AdmPaymode");
                dtledger.Columns.Add("AdmDedRes");
                dtledger.Columns.Add("AdmFine");
                dtledger.Columns.Add("AdmRefund");
                dtledger.Columns.Add("FeeAlloted");
                dtledger.Columns.Add("Deduction");
                dtledger.Columns.Add("TotalAmt");
                for (int ledgeCnt = 0; ledgeCnt < dsLedger.Tables[0].Rows.Count; ledgeCnt++)
                {
                    DataRow drLedger = dtledger.NewRow();
                    double feeAllot = 0;
                    double.TryParse(Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["AllotAmount"]), out feeAllot);
                    allot += feeAllot;
                    double dedAmt = 0;
                    double.TryParse(Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["Deduction"]), out dedAmt);
                    conses += dedAmt;
                    double feeTotal = 0;
                    double.TryParse(Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["TotalAmount"]), out feeTotal);
                    total += feeTotal;
                    drLedger["AdmLedger"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["ledgername"]);
                    drLedger["AdmLedgerId"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["LedgerFk"]);
                    drLedger["AdmHeaderId"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["HeaderFK"]);
                    drLedger["AdmPaymode"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["PayMode"]);
                    drLedger["AdmDedRes"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["DeductReason"]);
                    drLedger["AdmFine"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["FineAmount"]);
                    drLedger["AdmRefund"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["RefundAmount"]);
                    drLedger["FeeAlloted"] = feeAllot;
                    drLedger["Deduction"] = dedAmt;
                    drLedger["TotalAmt"] = feeTotal;
                    dtledger.Rows.Add(drLedger);
                }
                if (dtledger.Rows.Count > 0)
                {
                    gridFeeDet.DataSource = dtledger;
                    gridFeeDet.DataBind();
                    lblaltamt.Text = Convert.ToString(allot);
                    lblconsamt.Text = Convert.ToString(conses);
                    lbltotamt.Text = Convert.ToString(total);
                    btnFeeUpdate.Visible = true;
                    divstuddt.Visible = true;
                    trtotal.Visible = true;
                    ShowAllCb.Visible = true;
                    lblstudmsg.Visible = true;
                    lblAppnoFee.Text = appNo;
                    AddtionalInformationDiv.Visible = false;
                }
            }
            string hostname = "";
            string roomtype = "";
            if (AdmConfFormat() == 0)
            {
                AddtionalInformationDiv.Visible = true;
                StudentImage.ImageUrl = "Handler3.ashx?id=" + Session["pdfapp_no"];
                string selectquery = " select Boarding,Roll_Admit,convert(varchar(10), Adm_Date,103)as Adm_Date,Roll_No,Stud_Type from Registration where App_No ='" + appNo + "'";//+ Session["pdfapp_no"] +//Modified
                DataSet dne = new DataSet();
                dne.Clear();
                dne = d2.select_method_wo_parameter(selectquery, "Text");
                if (dne.Tables[0].Rows.Count > 0)
                {
                    admissionNoGeneration();
                    if (admisionvalue == "1")
                    {
                        txt_AdmissionNo.Enabled = false;
                    }
                    else
                    {
                        txt_AdmissionNo.Enabled = true;
                    }
                    string boarding = d2.GetFunction("select stage_id from stage_master where Stage_id='" + Convert.ToString(dne.Tables[0].Rows[0]["Boarding"]) + "'");
                    if (boarding != "0")
                    {
                        ddl_boarding.SelectedIndex = ddl_boarding.Items.IndexOf(ddl_boarding.Items.FindByValue(boarding));
                        ddl_boarding.Visible = true;
                        rbldayScTrans.SelectedIndex = rbldayScTrans.Items.IndexOf(rbldayScTrans.Items.FindByValue("1"));
                        //txtBoardPnt.Text = boarding;
                        //rbldayScTrans.SelectedIndex = rbldayScTrans.Items.IndexOf(rbldayScTrans.Items.FindByValue("1"));
                        //txtBoardPnt.Visible = true;
                    }
                    else
                    {
                        rbldayScTrans.SelectedIndex = rbldayScTrans.Items.IndexOf(rbldayScTrans.Items.FindByValue("0"));
                        ddl_boarding.Visible = false;
                        //txtBoardPnt.Visible = false; txtBoardPnt.Text = "";
                    }
                    txt_AdmissionNo.Text = Convert.ToString(dne.Tables[0].Rows[0]["Roll_Admit"]);
                    txt_AdmissionDate.Text = Convert.ToString(dne.Tables[0].Rows[0]["Adm_Date"]);
                    txt_rollno.Text = Convert.ToString(dne.Tables[0].Rows[0]["Roll_No"]);
                    ddlAdmissionStudType.SelectedIndex = ddlAdmissionStudType.Items.IndexOf(ddlAdmissionStudType.Items.FindByText(Convert.ToString(dne.Tables[0].Rows[0]["Stud_Type"])));
                    if (ddlAdmissionStudType.SelectedItem.Text == "Hostler")
                    {
                        hostname = "select hostelmasterfk,RoomfK from HT_HostelRegistration where app_no='" + Session["pdfapp_no"] + "'";
                        DataSet ds1 = d2.select_method_wo_parameter(hostname, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            ddlHosHostel.SelectedIndex = ddlHosHostel.Items.IndexOf(ddlHosHostel.Items.FindByValue(Convert.ToString(ds1.Tables[0].Rows[0]["hostelmasterfk"])));
                            loadHostelRoom();
                            roomtype = d2.GetFunction("select room_type from Room_Detail r, HT_HostelRegistration h where h.RoomfK='" + Convert.ToString(ds1.Tables[0].Rows[0]["RoomfK"]) + "' and h.roomfk=r.roompk");
                            if (ddlHosRoom.Items.Count > 0)
                            {
                                ddlHosRoom.SelectedIndex = ddlHosRoom.Items.IndexOf(ddlHosRoom.Items.FindByText(Convert.ToString(roomtype)));
                            }
                            // ddlHosRoom.SelectedItem.Text = roomtype;
                        }
                    }
                }
                else
                {
                    txt_AdmissionNo.Text = "";
                    txt_AdmissionDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt_rollno.Text = "";
                    ddlAdmissionStudType.SelectedIndex = 0;
                    rbldayScTrans.SelectedIndex = 0;
                    rblModeDet.SelectedIndex = 0;
                }
            }
            else if (AdmConfFormat() == 1)//&& DropDownList2.SelectedItem.Value == "3"/mo
            {
                AddtionalInformationDiv.Visible = true;
                StudentImage.ImageUrl = "Handler3.ashx?id=" + Session["pdfapp_no"];
                string selectquery = " select Boarding,Roll_Admit,convert(varchar(10), Adm_Date,103)as Adm_Date,Roll_No,Stud_Type from Registration where App_No ='" + Session["pdfapp_no"] + "'";
                DataSet dne = new DataSet();
                dne.Clear();
                dne = d2.select_method_wo_parameter(selectquery, "Text");
                if (dne.Tables[0].Rows.Count > 0)
                {
                    admissionNoGeneration();
                    if (admisionvalue == "1")
                    {
                        txt_AdmissionNo.Enabled = false;
                    }
                    else
                    {
                        txt_AdmissionNo.Enabled = true;
                    }
                    string boarding = d2.GetFunction("select stage_id from stage_master where Stage_id='" + Convert.ToString(dne.Tables[0].Rows[0]["Boarding"]) + "'");
                    if (boarding != "0")
                    {
                        ddl_boarding.SelectedIndex = ddl_boarding.Items.IndexOf(ddl_boarding.Items.FindByValue(boarding));
                        ddl_boarding.Visible = true;
                        rbldayScTrans.SelectedIndex = rbldayScTrans.Items.IndexOf(rbldayScTrans.Items.FindByValue("1"));
                        //txtBoardPnt.Text = boarding;
                        //rbldayScTrans.SelectedIndex = rbldayScTrans.Items.IndexOf(rbldayScTrans.Items.FindByValue("1"));
                        //txtBoardPnt.Visible = true;
                    }
                    else
                    {
                        ddl_boarding.Visible = false;
                        rbldayScTrans.SelectedIndex = rbldayScTrans.Items.IndexOf(rbldayScTrans.Items.FindByValue("0"));
                    }
                    txt_AdmissionNo.Text = Convert.ToString(dne.Tables[0].Rows[0]["Roll_Admit"]);
                    txt_AdmissionDate.Text = Convert.ToString(dne.Tables[0].Rows[0]["Adm_Date"]);
                    txt_rollno.Text = Convert.ToString(dne.Tables[0].Rows[0]["Roll_No"]);
                    ddlAdmissionStudType.SelectedIndex = ddlAdmissionStudType.Items.IndexOf(ddlAdmissionStudType.Items.FindByText(Convert.ToString(dne.Tables[0].Rows[0]["Stud_Type"])));
                    if (ddlAdmissionStudType.SelectedItem.Text == "Hostler")
                    {
                        hostname = "select hostelmasterfk,RoomfK from HT_HostelRegistration where app_no='" + Session["pdfapp_no"] + "'";
                        DataSet ds1 = d2.select_method_wo_parameter(hostname, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            ddlHosHostel.SelectedIndex = ddlHosHostel.Items.IndexOf(ddlHosHostel.Items.FindByValue(Convert.ToString(ds1.Tables[0].Rows[0]["hostelmasterfk"])));
                            loadHostelRoom();
                            roomtype = d2.GetFunction("select room_type from Room_Detail r, HT_HostelRegistration h where h.RoomfK='" + Convert.ToString(ds1.Tables[0].Rows[0]["RoomfK"]) + "' and h.roomfk=r.roompk");
                            ddlHosRoom.SelectedIndex = ddlHosRoom.Items.IndexOf(ddlHosRoom.Items.FindByValue(Convert.ToString(roomtype)));
                        }
                    }
                }
                else
                {
                    txt_AdmissionNo.Text = "";
                    txt_AdmissionDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt_rollno.Text = "";
                    ddlAdmissionStudType.SelectedIndex = 0;
                    rbldayScTrans.SelectedIndex = 0;
                    rblModeDet.SelectedIndex = 0;
                }
            }
            if (ddlAdmissionStudType.SelectedIndex == 0)
            {
                transport_div.Visible = true;
                rbldayScTrans.Visible = true;
                // lblBoardPnt.Text = "";
                lblBoardPnt.Visible = false;
                //txtBoardPnt.Text = "";
                ddl_boarding.Visible = false;
                if (rbldayScTrans.SelectedIndex == 1)
                {
                    // lblBoardPnt.Text = "Boarding";
                    lblBoardPnt.Visible = true;
                    // txtBoardPnt.Text = "";
                    ddl_boarding.Visible = true;
                }
                Hostel_div.Visible = false;
            }
            else
            {
                transport_div.Visible = false;
                Hostel_div.Visible = true;
                // ddlHosHostel_IndexChange(sender, e);
            }
        }
        catch { }
    }
    protected void admissionNoGeneration()
    {
        try
        {
            string value = d2.GetFunction("select value from Master_Settings where settings ='Admission No Rights' and usercode ='" + usercode + "'");
            if (value == "1")
                admisionvalue = "1";
            else
                admisionvalue = "0";
        }
        catch { }
    }
    private byte AdmConfFormat()
    {
        string collegecode = string.Empty;
        if (Session["studclgcode"] != null)
            collegecode = Convert.ToString(Session["studclgcode"]);
        //Format value 0 - Admit, Format value 1 - Wait to Admit
        byte format = 0;
        string AdmConQ = "select LinkValue from New_InsSettings where LinkName='AdmissionConfirmSetting' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
        format = Convert.ToByte(d2.GetFunction(AdmConQ).Trim());
        return format;
    }
    private string returnStudDeg(string appNo)
    {
        string deg = "0";
        string degQ = string.Empty;
        string collegecode = string.Empty;
        if (Session["studclgcode"] != null)
            collegecode = Convert.ToString(Session["studclgcode"]);
        if (true)//cbAltCourse.Checked
        {
            degQ = "select degree_code from applyn where college_code=" + collegecode + " and app_no =" + appNo + "";
        }
        else
        {
            degQ = "select degree_code from applyn where college_code=" + collegecode + " and app_no =" + appNo + "";
        }
        deg = d2.GetFunction(degQ).Trim();
        return deg;
    }
    protected void gridFeeDet_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (recptset == true)
            {
                TextBox txtallot = (TextBox)e.Row.Cells[2].FindControl("txtAdmFeeAllot");
                TextBox txtConces = (TextBox)e.Row.Cells[3].FindControl("txtAdmDeduc");
                txtallot.Attributes.Add("readonly", "readonly");
                txtConces.Attributes.Add("readonly", "readonly");
            }
            else
            {
                TextBox txtallot = (TextBox)e.Row.Cells[2].FindControl("txtAdmFeeAllot");
                TextBox txtConces = (TextBox)e.Row.Cells[3].FindControl("txtAdmDeduc");
                txtallot.Attributes.Remove("readonly");
                txtConces.Attributes.Remove("readonly");
            }
        }
    }
    protected void SettingReceipt()
    {
        try
        {
            if (checkadmitSetting() == 1)
            {
                if (checkSchoolSetting() == 0)
                {
                    if (feesStructureSetting() == 1)
                    {
                        recptset = true;
                    }
                    else
                        recptset = false;
                }
                else
                    recptset = false;
            }
            else
                recptset = false;
        }
        catch { }
    }
    private double feesStructureSetting()
    {
        double getVal = 0;
        try
        {
            double.TryParse(Convert.ToString(d2.GetFunction("select linkvalue from New_InsSettings where LinkName='AdmissionConfirmFeesStructureSetting' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'")), out getVal);
            //if (getVal == 0)
            //{
            //    btn_ch_gen.Visible = true;
            //    btnconform.Visible = true;
            //    btnconformrecpt.Visible = false;
            //}
            //else
            //{
            //    btn_ch_gen.Visible = false;
            //    btnconform.Visible = false;
            //    if (DropDownList2.SelectedItem.Text == "Fee Paid")
            //        btnconformrecpt.Visible = true;
            //}
        }
        catch { }
        return getVal;
    }
    private double checkSchoolSetting()
    {
        double getVal = 0;
        try
        {
            double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and user_code='" + usercode + "'")), out getVal);
        }
        catch { }
        return getVal;
    }
    private double checkadmitSetting()
    {
        double getVal = 0;
        try
        {
            double.TryParse(Convert.ToString(d2.GetFunction(" select LinkValue from New_InsSettings where LinkName='AdmissionConfirmSetting' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'")), out getVal);
        }
        catch { }
        return getVal;
    }
    //sudhagar 16.05.2017
    protected void getdeptDetails(string clgcode, string degcode, ref string type, ref string degName, ref string deptName, ref string eduLevel)
    {
        try
        {
            string selQ = "  select distinct edu_level,c.course_name,dt.dept_name,degree_code,type from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code='" + clgcode + "' and d.degree_code='" + degcode + "'";
            DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                type = Convert.ToString(dsval.Tables[0].Rows[0]["type"]);
                degName = Convert.ToString(dsval.Tables[0].Rows[0]["course_name"]);
                deptName = Convert.ToString(dsval.Tables[0].Rows[0]["dept_name"]);
                eduLevel = Convert.ToString(dsval.Tables[0].Rows[0]["edu_level"]);
            }
        }
        catch { }
    }
    protected void btnpdfstud_Click(object sender, EventArgs e)
    {
        applicationPdfFormateRights();
        if (formatevalue == "0")
        {
            pdfapplication();
        }
    }
    protected void btnsmsstud_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> dtstud = new Dictionary<string, string>();
        if (getSmsStud(out dtstud))
        {
            //popSendSms.Visible = true;
            popSendSms.Attributes.Add("Style", "height: 100em; z-index: 1000; width: 100%;position: absolute; top: 0; left: 0;display:block;");
            txt_SmsMsgPop.Text = string.Empty;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Students Selected')", true);
            //errorspan.InnerHtml = "No Students Selected";
            //poperrjs.Visible = true;
        }
    }
    protected bool getSmsStud(out Dictionary<string, string> dtstud)
    {
        dtstud = new Dictionary<string, string>();
        bool Ok = false;
        try
        {
            if (rdbtype.SelectedIndex == 0)
            {
                foreach (GridViewRow gdrow in gridstud.Rows)
                {
                    CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                    if (cb.Checked)
                    {
                        Ok = true;
                        dtstud.Add(Convert.ToString(gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text), Convert.ToString(gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text));
                    }
                }
            }
            else if (rdbtype.SelectedIndex == 1)
            {
                foreach (GridViewRow gdrow in gridstud.Rows)
                {
                    CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                    if (cb.Checked)
                    {
                        Ok = true;
                        dtstud.Add(Convert.ToString(gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text), Convert.ToString(gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text));
                    }
                }
            }
            else if (rdbtype.SelectedIndex == 2)
            {
                foreach (GridViewRow gdrow in gridstud.Rows)
                {
                    CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                    if (cb.Checked)
                    {
                        Ok = true;
                        dtstud.Add(Convert.ToString(gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text), Convert.ToString(gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text));
                    }
                }
            }
        }
        catch { }
        return Ok;
    }
    //shortlist
    protected void btnshortstud_Click(object sender, EventArgs e)
    {
        try
        {
            bool boolcheck = false;
            foreach (GridViewRow gdrow in gridstud.Rows)
            {
                CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                if (cb.Checked)
                {
                    string appNo = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text;
                    string collegecode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text;
                    string degreeCode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 1].Text;
                    string approve = " update applyn set selection_status='1', AdmitedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no ='" + appNo + "'";
                    int a = d2.update_method_wo_parameter(approve, "Text");
                    boolcheck = true;
                }
            }
            if (boolcheck)
            {
                btngo_Click(sender, e);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Shortlisted Successfully')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Any one Category')", true);
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnadmitstud_Click(object sender, EventArgs e)
    {
        // getAdmited();
        //Load ChangeCollege and Degree Details -- Code added by Idhris 29-04-2017
        string collegcode = string.Empty;
        string degCode = string.Empty;
        string appNo = string.Empty;
        txt_remarks.Text = "";
        if (getAdmited(ref collegcode, ref degCode, ref appNo) == 1)
        {
            spstudDet.Visible = true;
            spstudDet.InnerHtml = getStudDet(collegcode, degCode, appNo);//student Details shows in admit div
            LoadDegChangeDetails();
            string StudentSeattype = d2.GetFunction("select seattype from applyn where app_no='" + appNo + "'");
            ddl_seattype.SelectedIndex = ddl_seattype.Items.IndexOf(ddl_seattype.Items.FindByValue(Convert.ToString(StudentSeattype)));
            bool checK = false;
            imgdiv2.Visible = true;
            checkEditablerights();
            if (editableRights)
            {
                tdconces.Visible = true;
                bindConcessionReason();
            }
            else
                tdconces.Visible = false;
            bindAdmLedgerGrid();
            string isgeneral = IsGeneralFeeAllot();
            if (isgeneral.Trim() == "1")
                remark_div.Visible = true;
            else
                remark_div.Visible = false;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Any One Student!')", true);
        }
        //checK = true;
        //if (!checK)
        //{
        //    //errorspan.InnerHtml = "Please Select Any One Student";
        //    //poperrjs.Visible = true;
        //}
    }
    protected string getStudDet(string clgcode, string degcode, string appNo)
    {
        string strName = string.Empty;
        try
        {
            strName = Convert.ToString(d2.GetFunction("  select distinct (r.stud_name+'--'+c.course_name+'-'+dt.dept_name+'--'+cl.collname) as name  from degree d,course c,department dt,collinfo cl,applyn r where r.degree_code=d.degree_code and r.college_code=d.college_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code=cl.college_code and r.college_code=cl.college_code and d.college_code='" + clgcode + "' and d.degree_code='" + degcode + "' and r.app_no='" + appNo + "'"));
        }
        catch { }
        return strName;
    }
    protected int getAdmited(ref string collegecode, ref string degreeCode, ref string appNo)
    {
        int totCnt = 0;
        try
        {
            bool boolCheck = false;
            foreach (GridViewRow gdrow in gridstud.Rows)
            {
                CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                if (cb.Checked && !boolCheck)
                {
                    appNo = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text;
                    collegecode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text;
                    degreeCode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 1].Text;
                    Session["stdadmitappno"] = appNo;
                    Session["stdadmitdegcode"] = degreeCode;
                    Session["stdadmitclgcode"] = collegecode;
                    boolCheck = true;
                    totCnt++;
                }
            }
        }
        catch { }
        return totCnt;
    }
    //Added by Idhris 29-04-2017
    private void LoadDegChangeDetails()
    {
        chkIsColDegChange.Text = "Is Change " + lbldept.Text;
        lblColChangeDeg.Text = lblclg.Text;
        lblEdulevChangeDeg.Text = lbledu.Text;
        lblDegChangeDeg.Text = lbldeg.Text;
        lblDeptChangeDeg.Text = lbldept.Text;
        changeDegCollegeLoad();
        bindseattype();
        bindConcessionReason();
        changeDegBatchLoad();
        changeDegEdulevLoad();
        changeDegDegreeLoad();
        changeDegDeptLoad();
    }
    private void changeDegCollegeLoad()
    {
        try
        {
            ds.Clear();
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
            Hashtable hat = new Hashtable();
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlColChangeDeg.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlColChangeDeg.DataSource = ds;
                ddlColChangeDeg.DataTextField = "collname";
                ddlColChangeDeg.DataValueField = "college_code";
                ddlColChangeDeg.DataBind();
            }
        }
        catch
        {
        }
    }
    private void changeDegBatchLoad()
    {
        try
        {
            ddlBatChangeDeg.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatChangeDeg.DataSource = ds;
                ddlBatChangeDeg.DataTextField = "batch_year";
                ddlBatChangeDeg.DataValueField = "batch_year";
                ddlBatChangeDeg.DataBind();
            }
        }
        catch { }
    }
    private void changeDegEdulevLoad()
    {
        try
        {
            ddlEdulevChangeDeg.Items.Clear();
            ds = d2.select_method_wo_parameter("select distinct Edu_Level from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + ddlColChangeDeg.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' and degree.college_code='" + ddlColChangeDeg.SelectedItem.Value + "'  order by Edu_Level desc", "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlEdulevChangeDeg.DataSource = ds;
                ddlEdulevChangeDeg.DataTextField = "Edu_Level";
                ddlEdulevChangeDeg.DataValueField = "Edu_Level";
                ddlEdulevChangeDeg.DataBind();
            }
            else
            {
                ddlEdulevChangeDeg.Items.Insert(0, "--Select--");
            }
        }
        catch
        {
        }
    }
    private void changeDegDegreeLoad()
    {
        try
        {
            string query = "";
            string edulvl = "";
            if (ddlEdulevChangeDeg.SelectedItem.Text == "--Select--")
            {
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlColChangeDeg.SelectedItem.Value + "'";
            }
            else
            {
                edulvl = Convert.ToString(ddlEdulevChangeDeg.SelectedItem.Value);
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlColChangeDeg.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlDegChangeDeg.DataSource = ds;
                ddlDegChangeDeg.DataTextField = "course_name";
                ddlDegChangeDeg.DataValueField = "course_id";
                ddlDegChangeDeg.DataBind();
            }
        }
        catch
        {
        }
    }
    private void changeDegDeptLoad()
    {
        try
        {
            ddlDeptChangeDeg.Items.Clear();
            string deg = ddlDegChangeDeg.SelectedValue;
            if (deg != "--Select--" && deg != null && ddlDegChangeDeg.SelectedItem.Text != "All")
            {
                ds = d2.select_method_wo_parameter("select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + deg + "') and degree.college_code='" + ddlColChangeDeg.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "'", "Text");
            }
            else
            {
                ds = d2.select_method_wo_parameter("select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlColChangeDeg.SelectedItem.Value + "' and user_code='" + usercode + "'", "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlDeptChangeDeg.DataSource = ds;
                ddlDeptChangeDeg.DataTextField = "dept_name";
                ddlDeptChangeDeg.DataValueField = "degree_code";
                ddlDeptChangeDeg.DataBind();
            }
            else
            {
                ddlDeptChangeDeg.Items.Insert(0, "--Select--");
            }
        }
        catch
        {
        }
    }
    public void chkIsColDegChange_CheckChange(object sender, EventArgs e)
    {
        bindseattype();
        bindConcessionReason();
        loadcommunity();//ABARNA
    }
    public void ddlColChangeDeg_SelectedIndexchange(object sender, EventArgs e)
    {
        bindseattype();
        bindConcessionReason();
        changeDegEdulevLoad();
        changeDegDegreeLoad();
        changeDegDeptLoad();
        ddl_seattype_IndexChange(sender, e);
    }
    protected void ddlEdulevChangeDeg_SelectedIndexchange(object sender, EventArgs e)
    {
        changeDegDegreeLoad();
        changeDegDeptLoad();
    }
    protected void ddlDegChangeDeg_SelectedIndexchange(object sender, EventArgs e)
    {
        changeDegDeptLoad();
    }
    //Ended by Idhris 29-04-2017
    public void bindseattype()
    {
        try
        {
            string collegeCode = string.Empty;
            string degCode = string.Empty;
            string appNo = string.Empty;
            int studCnt = getAdmited(ref  collegeCode, ref  degCode, ref  appNo);
            if (imgdiv2.Visible && chkIsColDegChange.Checked)
            {
                collegeCode = ddlColChangeDeg.SelectedValue;
            }
            ddl_seattype.Items.Clear();
            if (studCnt == 1)//student count check
            {
                string seat = "";
                string deptquery = "select distinct TextVal,TextCode  from TextValTable where TextCriteria ='Seat' and college_code=" + collegeCode + "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_seattype.DataSource = ds;
                    ddl_seattype.DataTextField = "TextVal";
                    ddl_seattype.DataValueField = "TextCode";
                    ddl_seattype.DataBind();
                }
            }
        }
        catch
        {
        }
    }
    protected void bindConcessionReason()
    {
        try
        {
            // string collegeCode = college_code;
            string collegeCode = string.Empty;
            string degCode = string.Empty;
            string appNo = string.Empty;
            int studCnt = getAdmited(ref  collegeCode, ref  degCode, ref  appNo);
            if (imgdiv2.Visible && chkIsColDegChange.Checked)
            {
                collegeCode = ddlColChangeDeg.SelectedValue;
            }
            ddlconces.Items.Clear();
            if (studCnt == 1)//student count check
            {
                ds.Clear();
                string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + collegeCode + "'";
                ds = d2.select_method_wo_parameter(sql, "TEXT");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlconces.DataSource = ds;
                    ddlconces.DataTextField = "TextVal";
                    ddlconces.DataValueField = "TextCode";
                    ddlconces.DataBind();
                    ddlconces.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    ddlconces.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch
        { }
    }
    protected void ddl_seattype_IndexChange(object sender, EventArgs e)
    {
        bindAdmLedgerGrid();
    }
    private void bindAdmLedgerGrid()
    {
        try
        {
            SettingReceipt();
            //btnconformrecpt.Visible = false;
            txt_gridAdmLedgeTot.Visible = false;
            gridAdmLedge.DataSource = null;
            gridAdmLedge.DataBind();
            pnl2.Style.Add("height", "250px");
            if (ShowFeeStruct() == 1)
            {
                pnl2.Style.Add("height", "550px");
                string seattype = string.Empty;
                if (ddl_seattype.Items.Count > 0)
                {
                    seattype = " and d.seattype='" + ddl_seattype.SelectedValue + "'";
                }
                string degCode = "-1";
                if (Session["stdadmitdegcode"] != null)
                    degCode = Convert.ToString(Session["stdadmitdegcode"]);
                //if (ddldept.Items.Count > 0)
                //{
                //    degCode = ddldept.SelectedValue;
                //}
                string collegeCode = string.Empty;
                if (Session["stdadmitclgcode"] != null)
                    collegeCode = Convert.ToString(Session["stdadmitclgcode"]);
                //if (txt_searchappno.Text.Trim() != string.Empty || txt_searchmobno.Text.Trim() != string.Empty || txt_searchstudname.Text.Trim() != string.Empty)
                //{
                //    string app_no = string.Empty;
                //    try
                //    {
                //        if (TabContainer1.ActiveTabIndex == 0)
                //        {
                //            app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[1, 0].Tag).Trim();
                //        }
                //        else
                //            if (TabContainer1.ActiveTabIndex == 1)
                //            {
                //                app_no = Convert.ToString(FpSpread1.Sheets[0].Cells[1, 0].Tag).Trim();
                //            }
                //        if (app_no != string.Empty && app_no != null && app_no != "0")
                //        {
                //            degCode = d2.GetFunction("select degree_code from applyn where app_no='" + app_no + "'");
                //        }
                //    }
                //    catch { }
                //}
                string batchYr = ddl_batch.SelectedValue;
                //ddl_collegename.SelectedValue;
                if (imgdiv2.Visible && chkIsColDegChange.Checked)
                {
                    collegeCode = ddlColChangeDeg.SelectedValue;
                    degCode = ddlDeptChangeDeg.SelectedValue;
                    batchYr = ddlBatChangeDeg.SelectedValue;
                }
                string appNo = string.Empty;
                if (Session["stdadmitappno"] != null)
                    appNo = Convert.ToString(Session["stdadmitappno"]);
                string sem = getFeecategory(appNo);
                string community = d2.GetFunction("select community from applyn where  app_no='" + appNo + "'");//modified
                //Convert.ToString(ddl_sem.SelectedItem.Value);
                ListItem feecat = getFeecategoryNEW(sem, collegeCode);
                string linkName = string.Empty;
                //ListItem feecat = loadFeecategory(collegecode, usercode, ref linkName);
                string finYearId = d2.getCurrentFinanceYear(usercode, collegeCode);
                string comm = d2.GetFunction("select communitycode from ft_feeallotdegree where seattype=" + ddl_seattype.SelectedValue + " and degreecode='" + degCode + "' and FeeCategory in('" + feecat.Value + "') and FinYearFK ='" + finYearId + "' and BatchYear ='" + Convert.ToString(batchYr) + "' and communitycode='" + community + "'");//MODIFIED


                string ledgerQ = string.Empty;


                if (community == comm)//MODIFIED
                {
                    ledgerQ = "select distinct d.LedgerFk,ledgername,isnull(FeeAmount,0) as AllotAmount,isnull(DeductAmout,0) as Deduction,isnull(TotalAmount,0) as TotalAmount,PayMode,d.HeaderFK,DeductReason,isnull(FineAmount,0) as FineAmount,isnull(RefundAmount,0) as RefundAmount from FT_FeeAllotDegree d,Fm_ledgermaster l where d.BatchYear ='" + Convert.ToString(batchYr) + "' and d.FeeCategory in('" + feecat.Value + "') and d.FinYearFK ='" + finYearId + "' and d.LedgerFk=l.ledgerpk  and DegreeCode='" + degCode + "' " + seattype + " and d.communitycode='" + community + "'  and isnull(isHostelFees,0)<>1";
                }
                else
                {
                    ledgerQ = "select distinct d.LedgerFk,ledgername,isnull(FeeAmount,0) as AllotAmount,isnull(DeductAmout,0) as Deduction,isnull(TotalAmount,0) as TotalAmount,PayMode,d.HeaderFK,DeductReason,isnull(FineAmount,0) as FineAmount,isnull(RefundAmount,0) as RefundAmount from FT_FeeAllotDegree d,Fm_ledgermaster l where d.BatchYear ='" + Convert.ToString(batchYr) + "' and d.FeeCategory in('" + feecat.Value + "') and d.FinYearFK ='" + finYearId + "' and d.LedgerFk=l.ledgerpk  and DegreeCode='" + degCode + "' " + seattype + "   and isnull(isHostelFees,0)<>1";
                }
                DataSet dsLedger = new DataSet();
                dsLedger = d2.select_method_wo_parameter(ledgerQ, "Text");
                if (dsLedger.Tables.Count > 0 && dsLedger.Tables[0].Rows.Count > 0)
                {
                    DataTable dtledger = new DataTable();
                    dtledger.Columns.Add("AdmLedger");
                    dtledger.Columns.Add("AdmLedgerId");
                    dtledger.Columns.Add("AdmHeaderId");
                    dtledger.Columns.Add("AdmPaymode");
                    dtledger.Columns.Add("AdmDedRes");
                    dtledger.Columns.Add("AdmFine");
                    dtledger.Columns.Add("AdmRefund");
                    dtledger.Columns.Add("FeeAlloted");
                    dtledger.Columns.Add("Deduction");
                    dtledger.Columns.Add("TotalAmt");
                    double ovTotalAmt = 0;
                    for (int ledgeCnt = 0; ledgeCnt < dsLedger.Tables[0].Rows.Count; ledgeCnt++)
                    {
                        DataRow drLedger = dtledger.NewRow();
                        double feeAllot = 0;
                        double.TryParse(Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["AllotAmount"]), out feeAllot);
                        double dedAmt = 0;
                        double.TryParse(Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["Deduction"]), out dedAmt);
                        double feeTotal = 0;
                        double.TryParse(Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["TotalAmount"]), out feeTotal);
                        ovTotalAmt += feeTotal;
                        drLedger["AdmLedger"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["ledgername"]);
                        drLedger["AdmLedgerId"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["LedgerFk"]);
                        drLedger["AdmHeaderId"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["HeaderFK"]);
                        drLedger["AdmPaymode"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["PayMode"]);
                        drLedger["AdmDedRes"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["DeductReason"]);
                        drLedger["AdmFine"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["FineAmount"]);
                        drLedger["AdmRefund"] = Convert.ToString(dsLedger.Tables[0].Rows[ledgeCnt]["RefundAmount"]);
                        drLedger["FeeAlloted"] = feeAllot;
                        drLedger["Deduction"] = dedAmt;
                        drLedger["TotalAmt"] = feeTotal;
                        dtledger.Rows.Add(drLedger);
                    }
                    if (dtledger.Rows.Count > 0)
                    {
                        gridAdmLedge.DataSource = dtledger;
                        gridAdmLedge.DataBind();
                        txt_gridAdmLedgeTot.Visible = true;
                        txt_gridAdmLedgeTot.Text = "Total Amount : Rs." + ovTotalAmt + " /-";
                    }
                }
            }
        }
        catch { }
    }
    protected void gridAdmLedge_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (editableRights)
                {
                    TextBox txtallot = (TextBox)e.Row.Cells[1].FindControl("txtAdmFeeAllot");
                    TextBox txtconces = (TextBox)e.Row.Cells[2].FindControl("txtAdmDeduc");
                    txtallot.Attributes.Add("readonly", "readonly");
                    txtconces.Attributes.Add("readonly", "readonly");
                }
                else
                {
                    TextBox txtallot = (TextBox)e.Row.Cells[1].FindControl("txtAdmFeeAllot");
                    TextBox txtconces = (TextBox)e.Row.Cells[2].FindControl("txtAdmDeduc");
                    txtallot.Attributes.Remove("readonly");
                    txtconces.Attributes.Remove("readonly");
                }
                if (recptset == true)
                {
                    TextBox txtallot = (TextBox)e.Row.Cells[1].FindControl("txtAdmFeeAllot");
                    TextBox txtconces = (TextBox)e.Row.Cells[2].FindControl("txtAdmDeduc");
                    txtallot.Attributes.Add("readonly", "readonly");
                    txtconces.Attributes.Add("readonly", "readonly");
                }
                //else
                //{
                //    TextBox txtallot = (TextBox)e.Row.Cells[1].FindControl("txtAdmFeeAllot");
                //    TextBox txtconces = (TextBox)e.Row.Cells[2].FindControl("txtAdmDeduc");
                //    txtallot.Attributes.Remove("readonly");
                //    txtconces.Attributes.Remove("readonly");
                //}
            }
        }
        catch { }
    }
    protected void checkEditablerights()
    {
        double rightsVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction(" select LinkValue from New_InsSettings where LinkName='AdmissionFeeEditable' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'")), out rightsVal);
        editableRights = rightsVal == 0 ? true : false;
        //return rightsVal;
    }
    private byte ShowFeeStruct()
    {
        //Format value 0 - Dont show, Format value 1 - Show Ledgers in grid
        byte format = 0;
        string ShowLedQ = "select LinkValue from New_InsSettings where LinkName='AdmissionShowFeeStructure' and user_code ='" + usercode + "' ";
        format = Convert.ToByte(d2.GetFunction(ShowLedQ).Trim());
        return format;
    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        bool changeDegree = true;
        if (chkIsColDegChange.Checked)
        {
            changeDegree = ddlBatChangeDeg.Items.Count > 0 ? true : false;
            if (changeDegree)
                changeDegree = ddlDeptChangeDeg.Items.Count > 0 ? ddlDeptChangeDeg.SelectedIndex >= 0 ? true : false : false;
        }
        if (changeDegree)
        {
            if (ShowFeeStruct() == 0)
            {
                if ((rdbtype.SelectedIndex == 0) || (rdbtype.SelectedIndex == 1))
                {
                    admitsave(false);
                    btngo_Click(sender, e);
                }
                imgdiv2.Visible = false;
                // string errMsg = errorspan.InnerHtml;
                //  Button1_Click(sender, e);
                chkIsColDegChange.Checked = false;
                //errorspan.InnerHtml = errMsg;
            }
            else
            {
                if (gridAdmLedge.Rows.Count > 0)
                {
                    if ((rdbtype.SelectedIndex == 0) || (rdbtype.SelectedIndex == 1))
                    {
                        admitsave(true);
                        btngo_Click(sender, e);
                    }
                    imgdiv2.Visible = false;
                    // string errMsg = errorspan.InnerHtml;
                    // Button1_Click(sender, e);
                    chkIsColDegChange.Checked = false;
                    // errorspan.InnerHtml = errMsg;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Fees Available')", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('New " + lblDeptChangeDeg.Text + " and Batch are needed')", true);
        }
    }
    //alter by sudhagar 21.12.2016
    public void admitsave(bool showFee)
    {
        int isval1 = 0;
        int fllg = 0;
        int k = 0;
        string app_no = "";
        string collegeCode = string.Empty;
        string degreecode = "";
        string seattype = "";
        string headerfk = "";
        string leadgerfk = "";
        double feeamount = 0;
        double deduct = 0;
        string deductrea = "";
        double totalamount = 0;
        string refund = "";
        string feecatg = "";
        double finamount = 0;
        string paymode = "";
        string Generalfeeallot = "";
        bool blAppNo = false;
        ArrayList alAppNo = new ArrayList();
        string textcode = string.Empty;
        ListItem feecat = new ListItem();
        string batchyear = Convert.ToString(ddl_batch.SelectedItem.Value);
        bool checkflage = false;
        //  DateTime applycurrentdate = new DateTime();
        string applycurrentdate = DateTime.Now.ToString("MM/dd/yyyy");
        //  applycurrentdate.ToString("MM/dd/yyyy");
        //= DateTime.Now.ToString("MM/dd/yyyy");
        if (Session["stdadmitdegcode"] != null)
            degreecode = Convert.ToString(Session["stdadmitdegcode"]);
        if (Session["stdadmitclgcode"] != null)
            collegeCode = Convert.ToString(Session["stdadmitclgcode"]);
        // collegeCode = ddl_collegename.SelectedItem.Value;
        seattype = Convert.ToString(ddl_seattype.SelectedItem.Value);
        string comm = Convert.ToString(ddl_community.SelectedItem.Value);//abarna
        // degreecode = Convert.ToString(ddldept.SelectedItem.Value);
        batchyear = Convert.ToString(ddl_batch.SelectedItem.Value);
        #region For new degree and apply change
        if (imgdiv2.Visible && chkIsColDegChange.Checked)
        {
            collegeCode = ddlColChangeDeg.SelectedValue.Trim();
            degreecode = ddlDeptChangeDeg.SelectedValue.Trim();
            batchyear = ddlBatChangeDeg.SelectedValue.Trim();
        }
        #endregion
        string appNo = string.Empty;
        if (Session["stdadmitappno"] != null)
            appNo = Convert.ToString(Session["stdadmitappno"]);
        string sem = getFeecategory(appNo);
        string cursem = sem != "0" ? sem : "1";
        string getfinid = d2.getCurrentFinanceYear(usercode, Convert.ToString(collegeCode));
        Generalfeeallot = d2.GetFunction("select value from Master_Settings where settings ='GeneralFeeAllot' and usercode ='" + usercode + "'");
        string includeMulsem = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IncludeMultipleTermSettings'  and college_code ='" + collegeCode + "'"); //and user_code ='" + user_code + "' // Modify by jairam 09-05-2017 //barath added 10.01.18
        if (includeMulsem == "1")
        {
            string MulsemCode = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='SelectedMultipleFeecategoryCode' and college_code ='" + collegeCode + "'"); //and user_code ='" + user_code + "' Modify by jairam 09-05-2017 //barath added 10.01.18
            textcode = MulsemCode != "0" ? MulsemCode : "0";
        }
        if (textcode == "0" || string.IsNullOrEmpty(textcode))
        {
            feecat = getFeecategoryNEW(sem, collegeCode);
            textcode = feecat.Value;
        }
        bool RegnoShowFlag = false;
        string[] splcode = null;
        if (textcode != "0" && getfinid != "" && getfinid != "0")
        {
            if (textcode.Contains("','"))
                splcode = textcode.Split(new string[] { "','" }, StringSplitOptions.None);
            else if (textcode.Contains(','))
                splcode = textcode.Split(',');
            else
            {
                splcode = new string[1];
                splcode[0] = textcode;
            }
            for (int row = 0; row < splcode.Length; row++)
            {
                if (showFee && RegnoShowFlag)
                {
                    showFee = false;
                }
                textcode = Convert.ToString(splcode[row]);
                string checkfee = "select LedgerFK,HeaderFK,PayMode,FeeAmount,deductAmout,DeductReason,TotalAmount,RefundAmount,FeeCategory,FineAmount from FT_FeeAllotDegree where DegreeCode='" + degreecode + "' and BatchYear ='" + batchyear + "' and SeatType ='" + seattype + "' and FeeCategory ='" + textcode + "' and FinYearFK ='" + getfinid + "'  and isnull(isHostelFees,0)<>1";
                ds = d2.select_method_wo_parameter(checkfee, "text");
                if (Generalfeeallot == "1" && ds.Tables[0].Rows.Count == 0 && textcode != "-1")
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Allot The General Fee')", true);
                }
                else
                {
                    #region
                    bool validate = false;
                    bool cheCkSave = false;
                    //concession setting
                    double consAmt = 0;
                    string consReason = string.Empty;
                    int type = 0;
                    Hashtable conHeaderfk = new Hashtable();
                    Hashtable conLedgerfk = new Hashtable();
                    string conDeductRes = string.Empty;
                    checkEditablerights();
                    if (editableRights)
                    {
                        if (rbtype.SelectedIndex == 0)
                            type = 0;
                        else
                            type = 1;
                        if (ddlconces.Items.Count > 0 && ddlconces.SelectedItem.Text.Trim() != "Select")
                        {
                            consReason = Convert.ToString(ddlconces.SelectedItem.Value);
                            deductionAmount(degreecode, consReason, textcode, type, ref conHeaderfk, ref conLedgerfk);
                            validate = conLedgerfk.Count != 0 ? true : false;
                        }
                        else
                        {
                            validate = true;
                            consAmt = 0;
                            consReason = "0";
                        }
                    }
                    else
                        validate = true;
                    if (validate)
                    {
                        string IsGeneralFeeAllot = d2.GetFunction("select value from Master_Settings where settings='GeneralFeeAllot' and usercode='" + usercode + "'");
                        if (rdbtype.SelectedIndex == 0)
                        {
                            #region 1
                            if (checkflage == false)
                            {
                                int a = 0;
                                foreach (GridViewRow gdrow in gridstud.Rows)
                                {
                                    CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                                    if (cb.Checked)
                                    {
                                        //for (int i = 1; i < FpSpread3.Sheets[0].Rows.Count; i++)
                                        //{
                                        //    int.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[i, 1].Value), out isval1);
                                        //    if (isval1 == 1)
                                        //    {
                                        //string approve = "";
                                        // app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Tag);
                                        app_no = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text;
                                        if (!chkIsColDegChange.Checked)
                                        {
                                            batchyear = d2.GetFunction("select batch_year from applyn where app_no='" + app_no + "'").Trim();
                                        }
                                        // degreecode = d2.GetFunction("select degree_code  from applyn where app_no ='" + app_no + "'");
                                        string approve = " update applyn set seattype='" + seattype + "',allotcomm='" + comm + "' where app_no ='" + app_no + "'";//community addedhere for alloted
                                        a = d2.update_method_wo_parameter(approve, "text");
                                        if (!chkIsColDegChange.Checked)
                                        {
                                            //degreecode = returnStudDeg(Convert.ToString(FpSpread3.Sheets[0].Cells[i, 0].Tag));
                                            degreecode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 1].Text;
                                        }
                                        seattype = d2.GetFunction("select seattype from applyn where app_no ='" + app_no + "'");
                                        if (getfinid.Trim() != "" && getfinid.Trim() != "0" && seattype.Trim() != "" && seattype.Trim() != "0")
                                        {
                                            //if (!showFee)  // Note:::::: barath 10.01.18
                                            //{
                                            if (IsGeneralFeeAllot.Trim() == "1")
                                            {
                                                generalFeeallot(degreecode, seattype, batchyear, getfinid, app_no, conHeaderfk, conLedgerfk, ref  cheCkSave, ref  fllg, type, consReason, textcode);
                                            }
                                            else
                                            {
                                                cheCkSave = true;
                                                fllg = 1;
                                            }
                                            //}
                                            if (showFee)//  else barath 10.01.18
                                            {
                                                #region grid
                                                for (int gRow = 0; gRow < gridAdmLedge.Rows.Count; gRow++)
                                                {
                                                    Label hdrid = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmHeaderId");
                                                    Label lgrid = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmLedgerId");
                                                    TextBox feeamt = (TextBox)gridAdmLedge.Rows[gRow].FindControl("txtAdmFeeAllot");
                                                    TextBox dedamt = (TextBox)gridAdmLedge.Rows[gRow].FindControl("txtAdmDeduc");
                                                    Label dedrea = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmDedRes");
                                                    Label totamt = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmFeeTotal");
                                                    Label finamt = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmFine");
                                                    Label paymo = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmPaymode");
                                                    Label refamt = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmRefund");
                                                    headerfk = hdrid.Text;
                                                    leadgerfk = lgrid.Text;
                                                    double.TryParse(Convert.ToString(feeamt.Text), out feeamount);
                                                    double.TryParse(Convert.ToString(dedamt.Text), out deduct);
                                                    double.TryParse(Convert.ToString(totamt.Text), out totalamount);
                                                    deductrea = dedrea.Text;
                                                    totalamount = feeamount - deduct;
                                                    if (conHeaderfk.ContainsKey(Convert.ToString(headerfk)) && conLedgerfk.ContainsKey(Convert.ToString(leadgerfk)) && totalamount != 0)
                                                    {
                                                        double.TryParse(Convert.ToString(conLedgerfk[leadgerfk]), out consAmt);
                                                        getInsertValues(type, ref totalamount, ref consAmt, ref deduct);
                                                        deductrea = consReason;
                                                    }
                                                    else
                                                        getInsertValues(type, ref totalamount, ref consAmt, ref deduct);
                                                    double.TryParse(Convert.ToString(finamt.Text), out finamount);
                                                    refund = refamt.Text;
                                                    // feecatg = feecat.Value;
                                                    feecatg = textcode;
                                                    paymode = paymo.Text;

                                                    //barath 20.05.17
                                                    string remarks = "";
                                                    if (ddlconces.SelectedIndex != 0)
                                                    {
                                                        if (IsGeneralFeeAllot.Trim() == "1" && consAmt != 0)
                                                            remarks = txt_remarks.Text;
                                                    }
                                                    else
                                                        remarks = txt_remarks.Text;
                                                    string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + textcode + "')  and App_No in('" + app_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeamount + "',PaidAmount='0' ,DeductAmout='" + deduct + "',DeductReason='" + deductrea + "',FromGovtAmt='0',TotalAmount='" + totalamount + "',RefundAmount='" + refund + "',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='" + paymode + "',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='" + finamount + "',BalAmount='" + totalamount + "',Remarks='" + remarks + "' where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "') and App_No in('" + app_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK,Remarks) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no + ",'" + leadgerfk + "','" + headerfk + "','" + feeamount + "','" + deduct + "'," + deductrea + ",'0','" + totalamount + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + totalamount + "','" + getfinid + "','" + remarks + "')";
                                                    a = d2.update_method_wo_parameter(insupdquery, "text");
                                                    cheCkSave = true;
                                                    fllg = 1;
                                                }
                                                #endregion
                                            }
                                            if (!alAppNo.Contains(app_no) && cheCkSave)
                                            {
                                                alAppNo.Add(app_no);
                                                blAppNo = true;
                                            }
                                            //Rajkumar 7/3/2017
                                            if (blAppNo && cheCkSave)
                                            {
                                                string stapp = " update applyn set seattype='" + seattype + "',AdmitedDate='" + applycurrentdate + "',college_code='" + collegeCode + "',degree_code='" + degreecode + "',batch_year='" + batchyear + "' where app_no ='" + app_no + "'";
                                                int ast = d2.update_method_wo_parameter(stapp, "text");
                                            }
                                            //=================
                                            #region registration
                                            if (AdmConfFormat() == 0 && blAppNo && cheCkSave)
                                            {
                                                if (!RegnoShowFlag)
                                                {
                                                    admissionNumGeneration(app_no, seattype, degreecode, batchyear, cursem, ref blAppNo, Convert.ToDateTime(applycurrentdate), collegeCode);
                                                    // RegnoShowFlag = true;//delsi 
                                                }
                                            }
                                            if (blAppNo && cheCkSave)
                                            {
                                                string stapp = " update applyn set  Admission_Status='1',selection_status='1' where app_no ='" + app_no + "'";
                                                int ast = d2.update_method_wo_parameter(stapp, "text");
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                RegnoShowFlag = true;//delsi 
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Any one Category')", true);
                            }
                            #endregion
                        }
                        else if (rdbtype.SelectedIndex == 1)
                        {
                            #region 2
                            int a = 0;
                            if (checkflage == false)
                            {
                                foreach (GridViewRow gdrow in gridstud.Rows)
                                {
                                    CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                                    if (cb.Checked)
                                    {
                                        //for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                                        //{
                                        //    isval1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value);
                                        //    if (isval1 == 1)
                                        //    {
                                        string approve = "";
                                        // app_no = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag);
                                        app_no = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text;
                                        if (!chkIsColDegChange.Checked)
                                        {
                                            batchyear = d2.GetFunction("select batch_year from applyn where app_no='" + app_no + "'").Trim();
                                            // degreecode = returnStudDeg(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag));
                                            degreecode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 1].Text;
                                        }
                                        approve = " update applyn set seattype='" + seattype + "',allotcomm='" + comm + "' where app_no ='" + app_no + "'";//community alloted for here
                                        a = d2.update_method_wo_parameter(approve, "text");
                                        seattype = d2.GetFunction("select seattype  from applyn where app_no ='" + app_no + "'");
                                        if (getfinid.Trim() != "" && getfinid.Trim() != "0" && seattype.Trim() != "" && seattype.Trim() != "0")
                                        {
                                            if (!showFee)
                                            {
                                                if (IsGeneralFeeAllot.Trim() == "1")
                                                {
                                                    generalFeeallot(degreecode, seattype, batchyear, getfinid, app_no, conHeaderfk, conLedgerfk, ref  cheCkSave, ref  fllg, type, consReason, textcode);
                                                }
                                                else
                                                {
                                                    cheCkSave = true;
                                                    fllg = 1;
                                                }
                                            }
                                            else
                                            {
                                                #region grid
                                                for (int gRow = 0; gRow < gridAdmLedge.Rows.Count; gRow++)
                                                {
                                                    Label hdrid = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmHeaderId");
                                                    Label lgrid = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmLedgerId");
                                                    TextBox feeamt = (TextBox)gridAdmLedge.Rows[gRow].FindControl("txtAdmFeeAllot");
                                                    TextBox dedamt = (TextBox)gridAdmLedge.Rows[gRow].FindControl("txtAdmDeduc");
                                                    Label dedrea = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmDedRes");
                                                    Label totamt = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmFeeTotal");
                                                    Label finamt = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmFine");
                                                    Label paymo = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmPaymode");
                                                    Label refamt = (Label)gridAdmLedge.Rows[gRow].FindControl("lblAdmRefund");
                                                    headerfk = hdrid.Text;
                                                    leadgerfk = lgrid.Text;
                                                    deductrea = dedrea.Text;
                                                    double.TryParse(Convert.ToString(feeamt.Text), out feeamount);
                                                    double.TryParse(Convert.ToString(dedamt.Text), out deduct);
                                                    double.TryParse(Convert.ToString(totamt.Text), out totalamount);
                                                    totalamount = feeamount - deduct;
                                                    if (conHeaderfk.ContainsKey(Convert.ToString(headerfk)) && conLedgerfk.ContainsKey(Convert.ToString(leadgerfk)) && totalamount != 0)
                                                    {
                                                        double.TryParse(Convert.ToString(conLedgerfk[leadgerfk]), out consAmt);
                                                        getInsertValues(type, ref totalamount, ref consAmt, ref deduct);
                                                        deductrea = consReason;
                                                    }
                                                    else
                                                        getInsertValues(type, ref totalamount, ref consAmt, ref deduct);
                                                    double.TryParse(Convert.ToString(finamt.Text), out finamount);
                                                    refund = refamt.Text;
                                                    feecatg = textcode;
                                                    paymode = paymo.Text;
                                                    string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + textcode + "')  and App_No in('" + app_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeamount + "',PaidAmount='0' ,DeductAmout='" + deduct + "',DeductReason='" + deductrea + "',FromGovtAmt='0',TotalAmount='" + totalamount + "',RefundAmount='" + refund + "',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='" + paymode + "',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='" + finamount + "',BalAmount='" + totalamount + "' where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "') and App_No in('" + app_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no + ",'" + leadgerfk + "','" + headerfk + "','" + feeamount + "','" + deduct + "'," + deductrea + ",'0','" + totalamount + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + totalamount + "','" + getfinid + "')";
                                                    a = d2.update_method_wo_parameter(insupdquery, "text");
                                                    cheCkSave = true;
                                                    fllg = 1;
                                                }
                                                #endregion
                                            }
                                            if (!alAppNo.Contains(app_no) && cheCkSave)
                                            {
                                                alAppNo.Add(app_no);
                                                blAppNo = true;
                                            }
                                            #region registration
                                            if (AdmConfFormat() == 0 && blAppNo && cheCkSave)//delsi
                                            {
                                                //Admit
                                                if (!RegnoShowFlag)
                                                {

                                                    admissionNumGeneration(app_no, seattype, degreecode, batchyear, cursem, ref blAppNo, Convert.ToDateTime(applycurrentdate), collegeCode);
                                                    RegnoShowFlag = true;
                                                }
                                            }
                                            if (blAppNo && cheCkSave)
                                            {
                                                string stapp = " update applyn set Admission_Status='1',selection_status='1',seattype='" + seattype + "',AdmitedDate='" + applycurrentdate + "',college_code='" + collegeCode + "',degree_code='" + degreecode + "',batch_year='" + batchyear + "' where app_no ='" + app_no + "'";
                                                int ast = d2.update_method_wo_parameter(stapp, "text");
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Any one Category')", true);
                            }
                            #endregion
                        }
                    }
                    else
                        fllg = 2;
                    #endregion
                }
            }
            if (fllg == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Fees Available Please Set General FeeAllot')", true);
            }
            else if (fllg == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Admitted Successfully')", true);
            }
            else if (fllg == 2)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Set Concession Settings')", true);
            }
            else if (fllg == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Choose Atleast One Student And Than Proceed')", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Financial Year')", true);
        }
        txt_remarks.Text = "";
    }
    protected void generalFeeallot(string degreecode, string seattype, string batchyear, string getfinid, string app_no, Hashtable conHeaderfk, Hashtable conLedgerfk, ref bool cheCkSave, ref int fllg, int type, string consReason, string textcode)
    {
        #region general
        string headerfk = "";
        string leadgerfk = "";
        double feeamount = 0;
        double deduct = 0;
        string deductrea = "";
        double totalamount = 0;
        string refund = "";
        string feecatg = "";
        double finamount = 0;
        string paymode = "";
        double consAmt = 0;
        string qur = "select LedgerFK,HeaderFK,PayMode,FeeAmount,deductAmout,DeductReason,TotalAmount,RefundAmount,FeeCategory,FineAmount,communitycode,iscommunity from FT_FeeAllotDegree where DegreeCode='" + degreecode + "' and BatchYear ='" + batchyear + "' and SeatType ='" + seattype + "' and FeeCategory ='" + textcode + "' and FinYearFK ='" + getfinid + "'  and isnull(isHostelFees,0)<>1";
        qur += " select community from applyn where app_no='" + app_no + "'";
        ds = d2.select_method_wo_parameter(qur, "text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                headerfk = Convert.ToString(ds.Tables[0].Rows[k]["HeaderFK"]);
                leadgerfk = Convert.ToString(ds.Tables[0].Rows[k]["LedgerFK"]).Trim();
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["FeeAmount"]), out feeamount);
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["deductAmout"]), out deduct);
                deductrea = Convert.ToString(ds.Tables[0].Rows[k]["DeductReason"]);
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["TotalAmount"]), out totalamount);
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["FineAmount"]), out finamount);
                refund = Convert.ToString(ds.Tables[0].Rows[k]["RefundAmount"]);
                feecatg = Convert.ToString(ds.Tables[0].Rows[k]["FeeCategory"]);
                paymode = Convert.ToString(ds.Tables[0].Rows[k]["PayMode"]);
                consAmt = 0;

                //10.01.18 barath communitywise Fees Allot
                string communitycode = Convert.ToString(ds.Tables[0].Rows[k]["communitycode"]);
                string IsCommunity = Convert.ToString(ds.Tables[0].Rows[k]["iscommunity"]);
                bool CommunityBool = false;
                if (IsCommunity == "1" || IsCommunity == "True")
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        string CommunityCode = Convert.ToString(ds.Tables[1].Rows[0]["community"]);
                        if (CommunityCode == communitycode)
                            CommunityBool = true;
                        else
                            CommunityBool = false;
                    }
                }
                else
                    CommunityBool = true;
                if (CommunityBool)//barath 10.01.18 communitywise Fees Allot
                {
                    if (conHeaderfk.ContainsKey(Convert.ToString(headerfk)) && conLedgerfk.ContainsKey(Convert.ToString(leadgerfk)) && totalamount != 0)
                    {
                        double.TryParse(Convert.ToString(conLedgerfk[leadgerfk]), out consAmt);
                        getInsertValues(type, ref totalamount, ref consAmt, ref deduct);
                        deductrea = consReason;
                    }
                    else
                        getInsertValues(type, ref totalamount, ref consAmt, ref deduct);
                    string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + textcode + "')  and App_No in('" + app_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeamount + "',PaidAmount='0' ,DeductAmout='" + deduct + "',DeductReason='" + deductrea + "',FromGovtAmt='0',TotalAmount='" + totalamount + "',RefundAmount='" + refund + "',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='" + paymode + "',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='" + finamount + "',BalAmount='" + totalamount + "' where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "') and App_No in('" + app_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no + ",'" + leadgerfk + "','" + headerfk + "','" + feeamount + "','" + deduct + "'," + deductrea + ",'0','" + totalamount + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + totalamount + "','" + getfinid + "')";
                    int a = d2.update_method_wo_parameter(insupdquery, "text");
                    cheCkSave = true;
                    fllg = 1;
                }
            }
        }
        #endregion
    }
    protected void getInsertValues(int type, ref double totalamount, ref double consAmt, ref double deduct)
    {
        try
        {
            if (type == 0)
            {
                totalamount = totalamount - consAmt;
                deduct += consAmt;
            }
            else
            {
                double percent = 0;
                percent = Math.Round((totalamount / 100) * consAmt);
                totalamount = totalamount - percent;
                deduct += percent;
            }
        }
        catch { }
    }
    protected void deductionAmount(string degreeCode, string deductRes, string feeCode, int type, ref Hashtable conHeaderfk, ref Hashtable conLedgerfk)
    {
        double amtORper = 0;
        try
        {
            string SelQ = " select consper,consamt,headerfk,ledgerfk,fee_category,consdesc from FM_ConcessionRefundSettings where degree_code='" + degreeCode + "' and  consdesc='" + deductRes + "' and fee_category='" + feeCode + "' and RefMode='1' ";
            DataSet dsl = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsl.Tables.Count > 0 && dsl.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsl.Tables[0].Rows.Count; row++)
                {
                    string conHeaderfk1 = Convert.ToString(dsl.Tables[0].Rows[row]["headerfk"]);
                    string conLedgerfk1 = Convert.ToString(dsl.Tables[0].Rows[row]["ledgerfk"]);
                    if (type == 0)
                        double.TryParse(Convert.ToString(dsl.Tables[0].Rows[row]["consamt"]), out amtORper);
                    else
                        double.TryParse(Convert.ToString(dsl.Tables[0].Rows[row]["consper"]), out amtORper);
                    if (!conLedgerfk.Contains(conLedgerfk1))
                        conLedgerfk.Add(Convert.ToString(conLedgerfk1), amtORper);
                    if (!conHeaderfk.Contains(conHeaderfk1))
                        conHeaderfk.Add(Convert.ToString(conHeaderfk1), amtORper);
                }
            }
        }
        catch { }
    }
    protected void admissionNumGeneration(string app_no, string seattype, string degreecode, string batchyear, string cursem, ref bool blAppNo, DateTime applycurrentdate, string collegeCode)
    {
        try
        {
            //Admit
            string rolladmit = "";
            string approve = "";
            string stud_name = string.Empty;
            string app_fromno = string.Empty;
            string batchYr = string.Empty;
            string Mode = string.Empty;
            string eduleve = string.Empty;
            string sem = string.Empty;
            admissionNoGeneration();
            string selQ = "select seattype,stud_name,app_formno,batch_year,mode,(select Edu_Level from course c,Degree d where d.Course_Id=c.Course_Id and a.degree_code=d.Degree_Code) as Edulevel,current_semester from applyn a where app_no ='" + app_no + "'";
            DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                seattype = Convert.ToString(dsval.Tables[0].Rows[0]["seattype"]);
                stud_name = Convert.ToString(dsval.Tables[0].Rows[0]["stud_name"]);
                app_fromno = Convert.ToString(dsval.Tables[0].Rows[0]["app_formno"]);
                batchYr = Convert.ToString(dsval.Tables[0].Rows[0]["batch_year"]);
                Mode = Convert.ToString(dsval.Tables[0].Rows[0]["mode"]);
                eduleve = Convert.ToString(dsval.Tables[0].Rows[0]["Edulevel"]);
                sem = Convert.ToString(dsval.Tables[0].Rows[0]["current_semester"]);
            }
            if (string.IsNullOrEmpty(Mode))
                Mode = "1";
            int format = 0;
            string paavaiNewApplcationNO = string.Empty;
            if (admisionvalue == "1")//Barath add 09.01.18
            {
                paavaiNewApplcationNO = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='Common Application Number Settings' and  college_code  in('" + collegeCode + "') ");//barath 10.01.18//user_code ='" + usercode + "' and 01/02/2018 barath
                if (string.IsNullOrEmpty(paavaiNewApplcationNO) || paavaiNewApplcationNO == "0")
                    rolladmit = generateApplNo(collegeCode, Convert.ToInt32(degreecode), eduleve, Mode, seattype, batchYr, out format);//genearateAdmissionNo(collegeCode, degreecode, batchYr);
                else
                    rolladmit = autoGenDS.AdmissionNoAndApplicationNumberGeneration(0, appno: app_no, Mode: Mode, DegreeCode: degreecode, CollegeCode: collegeCode, SeatType: seattype, BatchYear: batchYr, Semester: sem);//barath 24.01.18
            }
            else
                rolladmit = app_fromno;
            if (rolladmit.Trim() == "0" || string.IsNullOrEmpty(rolladmit))
                rolladmit = app_fromno;
            string regEntry = "  if exists(select * from Registration where App_No='" + app_no + "')  delete from Registration where App_No='" + app_no + "' insert into Registration (App_No,Adm_Date,Roll_Admit,Roll_No,RollNo_Flag,Reg_No,Stud_Name,Batch_Year,degree_code,college_code,CC,DelFlag,Exam_Flag,Current_Semester,mode,entryusercode)values('" + app_no + "','" + System.DateTime.Now.ToString("yyy/MM/dd") + "','" + rolladmit + "','" + rolladmit + "','1','" + rolladmit + "','" + stud_name + "','" + batchyear + "','" + degreecode + "','" + collegeCode + "','0','0','OK','" + cursem + "','" + Mode + "','" + usercode + "')";
            int s = d2.update_method_wo_parameter(regEntry, "Text");
            if (string.IsNullOrEmpty(paavaiNewApplcationNO) || paavaiNewApplcationNO == "0")
                UpdateApplNo(collegeCode, Convert.ToInt32(degreecode), eduleve, Mode, seattype, batchYr, format);//Barath add 09.01.18
            blAppNo = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, Convert.ToString(collegeCode), "studAdmissionSelection"); }
    }
    public void btn_popclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void cbAdmLedgeFee_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbAdmLedgeFee.Checked == true)
            {
                string Getvalue = d2.GetFunction("select value from Master_Settings where settings='LedgerSettingValue' and usercode ='" + usercode + "'");
                if (Getvalue.Trim() != "0" && Getvalue.Trim() != "")
                {
                    string[] splitnew = Getvalue.Split('/');
                    if (splitnew.Length > 0)
                    {
                        for (int row = 0; row <= splitnew.GetUpperBound(0); row++)
                        {
                            if (row == 0)
                            {
                                string[] splitSecond = splitnew[row].Split(':');
                                if (splitSecond.Length > 0)
                                {
                                    ddlAdmLedge1.SelectedIndex = ddlAdmLedge1.Items.IndexOf(ddlAdmLedge1.Items.FindByValue(splitSecond[0]));
                                    txtAdmledge1Amt.Text = Convert.ToString(splitSecond[1]);
                                }
                            }
                            if (row == 1)
                            {
                                string[] splitSecond = splitnew[row].Split(':');
                                if (splitSecond.Length > 0)
                                {
                                    ddlAdmLedge2.SelectedIndex = ddlAdmLedge2.Items.IndexOf(ddlAdmLedge2.Items.FindByValue(splitSecond[0]));
                                    txtAdmledge2Amt.Text = Convert.ToString(splitSecond[1]);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                txtAdmledge1Amt.Text = string.Empty;
                txtAdmledge2Amt.Text = string.Empty;
                loadAdmLedger1();
                loadAdmLedger2();
            }
        }
        catch
        {
        }
    }
    private void loadAdmLedger1()
    {
        ListItem feecat = getFeecategory();
        ddlAdmLedge1.Items.Clear();
        try
        {
            string query = "  SELECT  LedgerPK,LedgerName,L.Priority FROM FM_LedgerMaster L,Ft_feeallotdegree fd WHERE  l.LedgerMode=0 and fd.Ledgerfk=L.LedgerPk  and fd.Headerfk=l.headerfk  and fd.BatchYear= " + Convert.ToString(ddl_batch.SelectedItem.Value) + "  and fd.feecategory= " + feecat.Value + "   AND L.CollegeCode = " + collegecode + " order by case when priority is null then 1 else 0 end, priority ";//--and fd.DegreeCode=" + ddldept.SelectedValue + "
            DataSet dsLedger = d2.select_method_wo_parameter(query, "Text");
            if (dsLedger.Tables.Count > 0 && dsLedger.Tables[0].Rows.Count > 0)
            {
                ddlAdmLedge1.DataSource = dsLedger;
                ddlAdmLedge1.DataTextField = "LedgerName";
                ddlAdmLedge1.DataValueField = "LedgerPK";
                ddlAdmLedge1.DataBind();
            }
        }
        catch { }
        ddlAdmLedge1.Items.Insert(0, "Select");
    }
    private void loadAdmLedger2()
    {
        ListItem feecat = getFeecategory();
        ddlAdmLedge2.Items.Clear();
        try
        {
            string query = "   SELECT  LedgerPK,LedgerName,L.Priority FROM FM_LedgerMaster L,Ft_feeallotdegree fd WHERE  l.LedgerMode=0 and fd.Ledgerfk=L.LedgerPk  and fd.Headerfk=l.headerfk  and fd.BatchYear= " + Convert.ToString(ddl_batch.SelectedItem.Value) + " and fd.feecategory= " + feecat.Value + "  AND L.CollegeCode = " + collegecode + " order by case when priority is null then 1 else 0 end, priority ";//--and fd.DegreeCode=" + ddldept.SelectedValue + "
            DataSet dsLedger = d2.select_method_wo_parameter(query, "Text");
            if (dsLedger.Tables.Count > 0 && dsLedger.Tables[0].Rows.Count > 0)
            {
                ddlAdmLedge2.DataSource = dsLedger;
                ddlAdmLedge2.DataTextField = "LedgerName";
                ddlAdmLedge2.DataValueField = "LedgerPK";
                ddlAdmLedge2.DataBind();
            }
        }
        catch { }
        ddlAdmLedge2.Items.Insert(0, "Select");
    }
    private ListItem getFeecategory()
    {
        //if (ddl_collegename.Items.Count > 0)
        //{
        //    college_code = Convert.ToString(ddl_collegename.SelectedItem.Value);
        //}
        ListItem feeCategory = new ListItem();
        string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
        DataSet dsFeecat = new DataSet();
        if (linkvalue == "0")
        {
            dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '1 Semester' and college_code=" + collegecode + "", "Text");
        }
        else
        {
            dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '1 Year' and college_code=" + collegecode + "", "Text");
        }
        if (dsFeecat.Tables.Count > 0 && dsFeecat.Tables[0].Rows.Count > 0)
        {
            feeCategory.Text = Convert.ToString(dsFeecat.Tables[0].Rows[0]["textval"]);
            feeCategory.Value = Convert.ToString(dsFeecat.Tables[0].Rows[0]["TextCode"]);
        }
        else
        {
            feeCategory.Text = " ";
            feeCategory.Value = "-1";
        }
        return feeCategory;
    }
    protected void btnfeesave_Click(object sender, EventArgs e)
    {
        try
        {
            string firstLedgercode = "";
            string firstLedgerAmount = "";
            string firstLedgercode1 = "";
            string firstLedgerAmount1 = "";
            string concat = "";
            if (ddlAdmLedge1.SelectedItem.Text != "Select")
            {
                firstLedgercode = Convert.ToString(ddlAdmLedge1.SelectedItem.Value);
                firstLedgerAmount = Convert.ToString(txtAdmledge1Amt.Text);
                concat = firstLedgercode + ":" + firstLedgerAmount;
            }
            if (ddlAdmLedge2.SelectedItem.Text != "Select")
            {
                firstLedgercode1 = Convert.ToString(ddlAdmLedge2.SelectedItem.Value);
                firstLedgerAmount1 = Convert.ToString(txtAdmledge2Amt.Text);
                concat = concat + "/" + firstLedgercode1 + ":" + firstLedgerAmount1;
            }
            string insertupdaquery = "if exists (select * from Master_Settings where settings='LedgerSettingValue' and usercode ='" + usercode + "') update Master_Settings set value ='" + concat + "' where settings='LedgerSettingValue' and usercode ='" + usercode + "' else insert into Master_Settings (usercode,settings,value) values ('" + usercode + "','LedgerSettingValue','" + concat + "')";
            int insetquery = d2.update_method_wo_parameter(insertupdaquery, "Text");
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            //errorspan.InnerHtml = "Saved Successfully";
            //poperrjs.Visible = true;
        }
        catch
        {
        }
    }
    protected void ddlAdmLedge1_IndexChanged(object sender, EventArgs e)
    {
        if (ddlAdmLedge1.Items.Count > 0)
        {
            txtAdmledge1Amt.Text = retLedgeAmount(ddlAdmLedge1.SelectedValue).ToString();
        }
    }
    protected void ddlAdmLedge2_IndexChanged(object sender, EventArgs e)
    {
        if (ddlAdmLedge2.Items.Count > 0)
        {
            txtAdmledge2Amt.Text = retLedgeAmount(ddlAdmLedge2.SelectedValue).ToString();
        }
    }
    private double retLedgeAmount(string ledgerId)
    {
        double amount = 0;
        try
        {
            // string degCOde = returnStudDeg(appNo);
            string collegecode = string.Empty;
            ListItem feecat = getFeecategory();
            string finYearId = d2.getCurrentFinanceYear(usercode, collegecode);
            string amtQ = "select isnull(TotalAmount,0) from FT_FeeAllotDegree where BatchYear ='" + Convert.ToString(ddl_batch.SelectedItem.Value) + "' and SeatType ='" + ddl_seattype.SelectedValue + "' and FeeCategory ='" + feecat.Value + "' and FinYearFK ='" + finYearId + "' and LedgerFk='" + ledgerId + "' -- and DegreeCode='degCOde '  ";
            double.TryParse(d2.GetFunction(amtQ).Trim(), out amount);
        }
        catch { }
        return amount;
    }
    protected string getFeecategory(string appNo)
    {
        string feeCat = string.Empty;
        try
        {
            feeCat = Convert.ToString(d2.GetFunction("select distinct current_semester from applyn where app_no='" + appNo + "'"));
        }
        catch { }
        return feeCat;
    }
    private string generateApplNo(string collegecode, int degreecode, string edulevel, string mode, string seattype, string batchyear, out int format)
    {
        string applNo = string.Empty;
        format = 0;
        try
        {
            ApplicationNumberGeneration appGen = new ApplicationNumberGeneration();
            int codeCheck = 0;
            string query = "select LinkValue from New_InsSettings where LinkName='CollegewiseAdmissionNoRights' and user_code ='" + usercode + "' "; //and college_code ='" + collegecode + "'
            codeCheck = DirAccess.selectScalarInt(query);
            if (codeCheck > 0)
            {
                applNo = appGen.getApplicationNumber(collegecode, batchyear, 1);
                format = 1;
            }
            else
            {
                query = "select LinkValue from New_InsSettings where LinkName='EdulevelAdmissionNoRights' and user_code ='" + usercode + "' ";//and college_code ='" + collegecode + "'
                codeCheck = DirAccess.selectScalarInt(query);
                if (codeCheck > 0)
                {
                    applNo = appGen.getApplicationNumber(collegecode, edulevel, batchyear, 1);
                    format = 2;
                }
                else
                {
                    query = "select LinkValue from New_InsSettings where LinkName='DegreeSeatModewiseAdmissionNoRights' and user_code ='" + usercode + "' ";//and college_code ='" + collegecode + "'
                    codeCheck = DirAccess.selectScalarInt(query);
                    if (codeCheck > 0)
                    {
                        applNo = appGen.getApplicationNumber(collegecode, batchyear, degreecode.ToString(), mode, seattype, 1);
                        format = 3;
                    }
                    else
                    {
                        applNo = appGen.getApplicationNumber(collegecode, batchyear, degreecode, 1);
                        format = 0;
                    }
                }
            }
        }
        catch { applNo = string.Empty; }
        return applNo;
    }
    private bool UpdateApplNo(string collegecode, int degreecode, string edulevel, string mode, string seattype, string batchyear, int format)
    {
        bool update = false;
        try
        {
            ApplicationNumberGeneration appGen = new ApplicationNumberGeneration();
            int codeCheck = 0;
            string query = "select LinkValue from New_InsSettings where LinkName='CollegewiseAdmissionNoRights' and user_code ='" + usercode + "' "; //and college_code ='" + collegecode + "'
            codeCheck = DirAccess.selectScalarInt(query);
            if (codeCheck > 0)
            {
                update = appGen.updateApplicationNumber(collegecode, batchyear, 1);
            }
            else
            {
                query = "select LinkValue from New_InsSettings where LinkName='EdulevelAdmissionNoRights' and user_code ='" + usercode + "'"; // and college_code ='" + collegecode + "'
                codeCheck = DirAccess.selectScalarInt(query);
                if (codeCheck > 0)
                {
                    update = appGen.updateApplicationNumber(collegecode, edulevel, batchyear, 1);
                }
                else
                {
                    query = "select LinkValue from New_InsSettings where LinkName='DegreeSeatModewiseAdmissionNoRights' and user_code ='" + usercode + "' "; //and college_code ='" + collegecode + "'
                    codeCheck = DirAccess.selectScalarInt(query);
                    if (codeCheck > 0)
                    {
                        update = appGen.updateApplicationNumber(collegecode, batchyear, degreecode.ToString(), mode, seattype, 1);
                    }
                    else
                    {
                        update = appGen.updateApplicationNumber(collegecode, batchyear, degreecode, 1);
                    }
                }
            }
        }
        catch { update = false; }
        return update;
    }
    protected void applicationPdfFormateRights()
    {
        try
        {
            string value = d2.GetFunction("select value from Master_Settings where settings ='Application Pdf Format Setting' and usercode ='" + usercode + "'");
            if (value == "0")
                formatevalue = "0";
            else
                formatevalue = "";
        }
        catch { }
    }
    public void pdfapplication()
    {
        try
        {
            string checkvalue = "";
            DAccess2 da = new DAccess2();
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            //Gios.Pdf.PdfDocument mydocument = null;
            // mydocument.PageCount = 0;
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Gios.Pdf.PdfPage mypdfpage1 = mydocument.NewPage();
            Font header = new Font("Arial", 15, FontStyle.Bold);
            Font header1 = new Font("Arial", 14, FontStyle.Bold);
            Font Fonthead = new Font("Arial", 12, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
            Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontsmall = new Font("Arial", 9, FontStyle.Regular);
            Font FontsmallBold = new Font("Arial", 10, FontStyle.Bold);
            Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            string spread = "";
            foreach (GridViewRow gdrow in gridstud.Rows)
            {
                CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                if (cb.Checked)
                {
                    //for (int i = 0; i < FpSpread3.Sheets[0].RowCount; i++)
                    //{
                    //    checkvalue = Convert.ToString(FpSpread3.Sheets[0].Cells[i, 1].Value);
                    //    if (checkvalue == "1")
                    //    {
                    mypdfpage = mydocument.NewPage();
                    mypdfpage1 = mydocument.NewPage();
                    // string app_no = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(i), 0].Tag);
                    string app_no = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text;
                    ;
                    string collegecode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text;
                    Session["pdfapp_no"] = Convert.ToString(app_no);
                    string strquery = "Select * from collinfo where college_code='" + collegecode + "'";
                    DataSet ds = da.select_method_wo_parameter(strquery, "Text");
                    string university = "";
                    string collname = "";
                    string address1 = "";
                    string address2 = "";
                    string address3 = "";
                    string pincode = "";
                    string affliated = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        collname = ds.Tables[0].Rows[0]["collname"].ToString();
                        address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                        address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                        address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                        pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                        affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                    }
                    string query = "select IsExService,parentF_Mobile,Degree_Code,bldgrp,parent_income,emailp,mother,motherocc,mIncome,parentM_Mobile,emailM,guardian_name,guardian_mobile,emailg,aadharno,place_birth,app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,degree_code,batch_year,college_code,SubCaste,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet from applyn a where a.app_no='" + Convert.ToString(Session["pdfapp_no"]) + "'";
                    query = query + " select instaddress,course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark from Stud_prev_details where app_no ='" + Convert.ToString(Session["pdfapp_no"]) + "' ";
                    query = query + " select * from perv_marks_history ";
                    DataSet ds1 = d2.select_method_wo_parameter(query, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        int left1 = 1;
                        int left2 = 225;
                        int left4 = 470;
                        string[] split = collname.Split('(');
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg"));
                            mypdfpage.Add(LogoImage, 20, 40, 250);
                        }
                        int coltop = 15;
                        PdfTextArea ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Application No:  " + Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Registration No:" + Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(fontitalic, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(To be allotted by the College Office)");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, -40, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]));
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 80, coltop - 2, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("(Autonomous)"));
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, -20, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleCenter, address1 + " , " + address2 + " , " + address3 + " - " + pincode + ".  INDIA");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, -20, coltop - 20, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affliated);
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "APPLICATION FOR ADMISSION");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "U.G.COURSES - (2016-19)");
                        mypdfpage.Add(ptc);
                        string Timing = "";
                        if (Convert.ToString(Session["college_Code"]) == "13")
                        {
                            Timing = "(SHIFT - I : 8.30 AM - 1.30 PM)";
                        }
                        if (Convert.ToString(Session["college_Code"]) == "14")
                        {
                            Timing = "(SHIFT - II : 2.15 PM - 6.40 PM)";
                        }
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Timing);
                        mypdfpage.Add(ptc);
                        ////////photo/////////
                        string imgPhoto = string.Empty;
                        string appformno = Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpg")))
                        {
                            imgPhoto = HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpg");
                        }
                        if (imgPhoto.Trim() == string.Empty)
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left2, 40, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "Affix");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left2, 50, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "Passport size");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left2, 60, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "photograph");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                            //{
                            try
                            {
                                PdfImage studimg = mydocument.NewImage(imgPhoto);
                                mypdfpage.Add(studimg, 460, 50, 250);
                            }
                            catch { }
                            //}
                        }
                        coltop = coltop + 40;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "[Please read the Prospectus carefully before filling up the application form. Use CAPITAL LETTERS only]");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        left1 = 15;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "COURSE APPLIED FOR");
                        mypdfpage.Add(ptc);
                        string courseid = d2.GetFunction("select c.Course_Name from Degree d,course c where Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "' and d.Course_Id=c.Course_Id");
                        string deptname = d2.GetFunction("select Dept_Name from Degree d,Department dd where Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "' and d.Dept_Code=dd.Dept_Code");
                        left1 = 140;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + courseid + "-" + deptname + "");
                        mypdfpage.Add(ptc);
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                                     new PdfArea(mydocument, left1 - 90, coltop + 30, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(Session["gradutation"]) + "-" + Convert.ToString(Session["course"]) + "");
                        //mypdfpage.Add(ptc);
                        //ptc = new PdfTextArea(fontitalic, System.Drawing.Color.Black,
                        //                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "* Subject to approval of affiliation from the University of Madras");
                        //mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop = coltop + 25;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop + 15, 600, 50), System.Drawing.ContentAlignment.TopLeft, "PART -I LANGUAGE");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 50;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "For office use:");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 30;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Admitted in   : _________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 295, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "on  ");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 310, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " _____________________");
                        mypdfpage.Add(ptc);
                        left4 = 475;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left4, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "AT / BT /NME");
                        mypdfpage.Add(ptc);
                        left1 = 20;
                        coltop = coltop + 32;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop - 5, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Allied - 1         : ____________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 275, coltop - 5, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Allied - 2     : ________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 65, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Applicant's Name (In English)");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 200, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "  " + ds1.Tables[0].Rows[0]["stud_name"] + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Applicant's Name (In Tamil)");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Address for Communication");
                        mypdfpage.Add(ptc);
                        left1 = 350;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " Permanent Address");
                        mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string address = "";
                        address = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressP"]) + "," + Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]);
                        string address_value = "";
                        address_value = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressC"]) + "," + Convert.ToString(ds1.Tables[0].Rows[0]["Streetc"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(address) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 280, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(address_value) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string addressfist = "";
                        addressfist = Convert.ToString(ds1.Tables[0].Rows[0]["cityp"]);
                        string addressfist1 = "";
                        addressfist1 = Convert.ToString(ds1.Tables[0].Rows[0]["Cityc"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressfist) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 280, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressfist1) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string addressscond = "";
                        addressscond = Convert.ToString(ds1.Tables[0].Rows[0]["parent_statep"]);
                        string addressscond1 = "";
                        addressscond1 = Convert.ToString(ds1.Tables[0].Rows[0]["parent_statec"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressscond) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 280, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressscond1) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Pincode:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 100, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_pincodep"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 - 5, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Pincode:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 + 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_pincodec"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 14;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "E-mail:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, left1 + 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["StuPer_Id"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 - 5, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mobile No:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 300 + 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["Student_Mobile"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Nationality ");
                        mypdfpage.Add(ptc);
                        string nationality = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["citizen"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + nationality + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Birth");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Aadhar Card No.");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(ds1.Tables[0].Rows[0]["aadharno"]));
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["dob"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Place of Birth");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["place_birth"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Religion & Community");
                        mypdfpage.Add(ptc);
                        string relig = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["religion"]));
                        string comm = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["community"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + relig + " & " + comm + "      (Attach photocopy)");
                        mypdfpage.Add(ptc);
                        string caste = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["caste"]));
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                               new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Caste");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + caste + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Blood Group");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mother Tongue");
                        mypdfpage.Add(ptc);
                        string mothertong = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["mother_tongue"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + mothertong + "");
                        mypdfpage.Add(ptc);
                        string bldgrp = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 30;
                        if (Convert.ToString(Session["co_curricular"]) != "-")
                        {
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Distinction / Participation in Sports / Athletics / NCC / NSS ");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Distinction / Participation in Sports / Athletics / NCC / NSS : " + Convert.ToString(Session["co_curricular"]) + " ( bring relevant documents at the time of Admission)");
                            mypdfpage.Add(ptc);
                        }
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Whether differently-abled      :");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["isdisable"]) == "1")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + "YES" + " / If yes, bring relevant documents at the time of Admission");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + "No" + "");
                            mypdfpage.Add(ptc);
                        }
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Whether son of Ex-serviceman :");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["IsExService"]) == "1")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + "Yes" + " / If yes, bring relevant documents at the time of Admission");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + "No" + "");
                            mypdfpage.Add(ptc);
                        }
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "PARTICULARS OF THE PARENTS/GUARDIAN ");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name (in English)");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_name"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name (in Tamil)");
                        mypdfpage.Add(ptc);
                        string occcp = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_occu"]));
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Occupation");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + occcp + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 340, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Annual Income");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 410, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        string income = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_income"]));
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 410, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + income + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Contact No.");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parentF_Mobile"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Email ID");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["emailp"]) + "");
                        mypdfpage.Add(ptc);
                        ////////////////////////////////page2///////////////////////////////////////
                        coltop = 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mother's Name");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 115, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________________________________________________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 115, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["mother"]) + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Occupation");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 115, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "___________________________________________");
                        mypdfpage1.Add(ptc);
                        string moth_occ = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["motherocc"]));
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 115, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + moth_occ + "");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 333, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Annual Income ");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, 405, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________");
                        mypdfpage1.Add(ptc);
                        string moth_income = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["mIncome"]));
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 405, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + moth_income + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Contact No.");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 115, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 115, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parentM_Mobile"]) + "");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "E-mail ID");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 325, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 325, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["emailM"]) + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Guardian's Name (if living with guardian)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                               new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "____________________________________________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 225, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["guardian_name"]) + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Contact No.");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 115, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 115, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["guardian_mobile"]) + "");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "E-mail ID");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 330, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 330, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["emailg"]) + "");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "PARTICULARS OF PREVIOUS ACADEMIC RECORD");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 45;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Qualifying exam passed");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(Session["qualifyingexam"]) + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 20;
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                          new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the Board");
                        //mypdfpage1.Add(ptc);
                        //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                        //                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(Session["bordoruniversity"]) + "");
                        //mypdfpage1.Add(ptc);
                        //coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Institution last attended");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[1].Rows[0]["Institute_name"]) + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(With Address & Contact Nos)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[1].Rows[0]["instaddress"]) + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Language studied in X-Std");
                        mypdfpage1.Add(ptc);
                        string medium = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["medium"]));
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " " + medium + " ");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 300, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Language studied in XII-Std");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 300 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + medium + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "EXTRACT OF THE MARK STATEMENT(S) OF THE QUALIFYING EXAMINATION PASSED ");
                        mypdfpage1.Add(ptc);
                        ////// table////////
                        string subjectname = "";
                        string finalmarkandgrade = "";
                        string subjectwisemark = "";
                        string Month = "";
                        string year1 = "";
                        string regno = "";
                        string nofoattempts = "";
                        string max_mark = "";
                        int maxtotal = 0;
                        int mintotal = 0;
                        string grade = "";
                        DataView dv = new DataView();
                        int count = 0;
                        ds1.Tables[2].DefaultView.RowFilter = " course_entno='" + Convert.ToString(ds1.Tables[1].Rows[0]["course_entno"]) + "' ";
                        dv = ds1.Tables[2].DefaultView;
                        if (dv.Count > 0)
                        {
                            for (int u = 0; u < dv.Count; u++)
                            {
                                count++;
                                grade = Convert.ToString(dv[u]["grade"]);
                                if (grade != "")
                                {
                                    finalmarkandgrade = Convert.ToString(dv[u]["grade"]);
                                }
                                else
                                {
                                    finalmarkandgrade = Convert.ToString(dv[u]["acual_marks"]);
                                }
                                subjectname = Convert.ToString(dv[u]["psubjectno"]);
                                Month = Convert.ToString(dv[u]["pass_month"]);
                                year1 = Convert.ToString(dv[u]["pass_year"]);
                                regno = Convert.ToString(dv[u]["registerno"]);
                                nofoattempts = Convert.ToString(dv[u]["noofattempt"]);
                                max_mark = Convert.ToString(dv[u]["max_marks"]);
                                if (subjectname.Trim() != "")
                                {
                                    if (subjectwisemark == "")
                                    {
                                        subjectwisemark = subjectname + "-" + finalmarkandgrade + "-" + Month + "-" + year1 + "-" + regno + "-" + nofoattempts + "-" + max_mark;
                                    }
                                    else
                                    {
                                        subjectwisemark = subjectwisemark + "/" + subjectname + "-" + finalmarkandgrade + "-" + Month + "-" + year1 + "-" + regno + "-" + nofoattempts + "-" + max_mark;
                                    }
                                    if (maxtotal == 0)
                                    {
                                        maxtotal = Convert.ToInt32(max_mark);
                                    }
                                    else
                                    {
                                        maxtotal = maxtotal + Convert.ToInt32(max_mark);
                                    }
                                    if (mintotal == 0)
                                    {
                                        mintotal = Convert.ToInt32(finalmarkandgrade);
                                    }
                                    else
                                    {
                                        mintotal = mintotal + Convert.ToInt32(finalmarkandgrade);
                                    }
                                }
                            }
                        }
                        string[] splittablevlaue;
                        if (subjectwisemark.Trim() != "")
                        {
                            Session["subjectwisemark"] = subjectwisemark.ToString();
                        }
                        Gios.Pdf.PdfTable table2 = mydocument.NewTable(Fontsmall, count + 1 + 1, 7, 1);
                        table2 = mydocument.NewTable(Fontsmall, count + 1 + 1, 7, 1);
                        table2.VisibleHeaders = false;
                        table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table2.Columns[0].SetWidth(100);
                        table2.Columns[1].SetWidth(100);
                        table2.Columns[2].SetWidth(100);
                        table2.Columns[3].SetWidth(100);
                        table2.Columns[4].SetWidth(100);
                        table2.Columns[5].SetWidth(100);
                        table2.Columns[6].SetWidth(100);
                        table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                        table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 0).SetContent("Subjects");
                        table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 1).SetContent("Register No");
                        if (grade == "")
                        {
                            table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 2).SetContent("Mark");
                        }
                        else
                        {
                            table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 2).SetContent("Grade");
                        }
                        table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 3).SetContent("Maximum Marks");
                        table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 4).SetContent("Month");
                        table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 5).SetContent("Year");
                        table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 6).SetContent("No.of Attempts");
                        int count_value = 0;
                        string tablevalue1 = Convert.ToString(Session["subjectwisemark"]);
                        if (tablevalue1.Trim() != "")
                        {
                            splittablevlaue = tablevalue1.Split('/');
                            if (splittablevlaue.Length > 0)
                            {
                                for (int add = 0; add <= splittablevlaue.GetUpperBound(0); add++)
                                {
                                    count_value++;
                                    string[] firstvalue = splittablevlaue[add].Split('-');
                                    if (firstvalue.Length > 0)
                                    {
                                        subjectname = Convert.ToString(firstvalue[0]);
                                        string subjectname1 = "";
                                        string selectquery = "select Textval from textvaltable where TextCode='" + subjectname + "' and college_code ='" + collegecode + "'";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            subjectname1 = Convert.ToString(ds.Tables[0].Rows[0]["Textval"]);
                                        }
                                        table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table2.Cell(add + 1, 0).SetContent(subjectname1);
                                        table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 1).SetContent(Convert.ToString(firstvalue[4]));
                                        table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 2).SetContent(Convert.ToString(firstvalue[1]));
                                        table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 3).SetContent(Convert.ToString(firstvalue[6]));
                                        table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 4).SetContent(Convert.ToString(firstvalue[2].First().ToString().ToUpper() + firstvalue[2].Substring(1)));
                                        // Month.First().ToString().ToUpper() + Month.Substring(1)
                                        table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 5).SetContent(Convert.ToString(firstvalue[3]));
                                        table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 6).SetContent(Convert.ToString(firstvalue[5]));
                                    }
                                }
                                table2.Cell(count_value + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                table2.Cell(count_value + 1, 0).SetContent("Total Marks Secured");
                                foreach (PdfCell pr in table2.CellRange(count_value + 1, 0, count_value + 1, 0).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                table2.Cell(count_value + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(count_value + 1, 2).SetContent("" + mintotal + "");
                                table2.Cell(count_value + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(count_value + 1, 3).SetContent("" + maxtotal + "");
                                foreach (PdfCell pr in table2.CellRange(count_value + 1, 4, count_value + 1, 4).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                            }
                        }
                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, coltop + 30, 550, 550));
                        mypdfpage1.Add(myprov_pdfpage1);
                        /////////////////////////////bottom////////////////////////
                        //coltop = coltop + 200;
                        //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                        //                                   new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        //mypdfpage1.Add(ptc);
                        //coltop = coltop + 10;
                        //ptc = new PdfTextArea(tamil, System.Drawing.Color.Black,
                        //                                   new PdfArea(mydocument, left1 + 25, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        //mypdfpage1.Add(ptc);
                        ////???? ??????????????? ????????? ????????? ??????????, ????????. ???? ?????????????? ??????????????? ?????? ????????????? ???????????? ????????. ??????????? ????????? ???????????? ?????? ????????? ????? ????????. ????????, ??????? ????????? ????????? ??????????????? ??????????????.
                        coltop = coltop + 200;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "List of enclosures :");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(i)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(ii)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(iii)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 415, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(iv)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(v)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(vi)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 420, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Declaration:");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1 + 55, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "I declare that the particulars furnished above are true and correct. I submit that i will abide by the rules and");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " regulations of the college, and will not take part in any activity prejudical to the interest of the college.");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(tamil, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1 + 25, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        mypdfpage1.Add(ptc);
                        //???? ??????????????? ????????? ????????? ??????????, ????????. ???? ?????????????? ??????????????? ?????? ????????????? ???????????? ????????. ??????????? ????????? ???????????? ?????? ????????? ????? ????????. ????????, ??????? ????????? ????????? ??????????????? ??????????????.
                        coltop = coltop + 40;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 375, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the Parent/Guardian");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the Student");
                        mypdfpage1.Add(ptc);
                        bool falge = false;
                        if (falge == false)
                        {
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "----------------------------------------------------------------FOR OFFICE USE ONLY------------------------------------------------------------");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Interviewed on");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Admitted in");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "by");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "____________________________");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 375, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(Staff No:");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 425, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "____________________)");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 55;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 420, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Principal");
                            mypdfpage1.Add(ptc);
                        }
                        coltop = coltop + 60;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Place :");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date :");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                        mypdfpage1.Add(ptc);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 25, 18, 400);
                        }
                        /////////////////2ND header/////////////
                        PdfArea pa12 = new PdfArea(mydocument, 110, 40, 344, 120);
                        PdfRectangle pr12 = new PdfRectangle(mydocument, pa12, Color.Black);
                        mypdfpage.Add(pr12);
                        /////////////////////right photo//////////////////
                        PdfArea pa4 = new PdfArea(mydocument, 454, 40, 120, 120);
                        PdfRectangle pr4 = new PdfRectangle(mydocument, pa4, Color.Black);
                        mypdfpage.Add(pr4);
                        /////////////////1st header/////////////
                        PdfArea pa5 = new PdfArea(mydocument, 110, 40, 344, 60);
                        PdfRectangle pr5 = new PdfRectangle(mydocument, pa5, Color.Black);
                        mypdfpage.Add(pr5);
                        /////////////////page//////////////
                        PdfArea pa1 = new PdfArea(mydocument, 14, 12, 560, 825);
                        PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                        mypdfpage.Add(pr3);
                        mypdfpage1.Add(pr3);
                        //////////////////////////for office/////////////////////
                        PdfArea pa13 = new PdfArea(mydocument, 14, 280, 540, 60);
                        PdfRectangle pr13 = new PdfRectangle(mydocument, pa13, Color.Black);
                        mypdfpage.Add(pr13);
                        //////////////////addressleft/////////////
                        PdfArea pa9 = new PdfArea(mydocument, 14, 380, 280, 95);
                        PdfRectangle pr9 = new PdfRectangle(mydocument, pa9, Color.Black);
                        mypdfpage.Add(pr9);
                        ////////////////addressright/////////////
                        PdfArea pa90 = new PdfArea(mydocument, 294.5, 380, 274, 95);
                        PdfRectangle pr90 = new PdfRectangle(mydocument, pa90, Color.Black);
                        mypdfpage.Add(pr90);
                        ////////////////////email\\\\\\\\\\\\\\\\\\\\\\\
                        //PdfArea pa91 = new PdfArea(mydocument, 14, 520, 555, 30);
                        //PdfRectangle pr91 = new PdfRectangle(mydocument, pa91, Color.Black);
                        //mypdfpage.Add(pr91);
                        mypdfpage.SaveToDocument();
                        mypdfpage1.SaveToDocument();
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "ApplicationForm" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
                Response.End();
            }
            else
            {
            }
        }
        catch
        {
        }
    }
    protected void btnSendSmsPop_click(object sender, EventArgs e)
    {
        // poperrjs.Visible = true;
        if (txt_SmsMsgPop.Text.Trim() != string.Empty)
        {
            //List<string> appNoList = new List<string>();
            //checkedOKSpread(out appNoList);
            Dictionary<string, string> dtstud = new Dictionary<string, string>();
            getSmsStud(out  dtstud);
            int okcnt = 0;
            int errcnt = 0;
            foreach (KeyValuePair<string, string> dtValue in dtstud)
            {
                string appNo = Convert.ToString(dtValue.Key);
                string clgcode = Convert.ToString(dtValue.Value);
                string mobile = d2.GetFunction("select student_mobile from applyn where app_no=" + appNo + "").Trim();
                if (mobile != "0")
                {
                    //sendsmsnew(mobile, appno, 1);                  
                    string Msg = txt_SmsMsgPop.Text.Trim();
                    string user_id = "";
                    string ssr = "select * from Track_Value where college_code='" + clgcode + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(ssr, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                    }
                    mobile = "0";
                    int d = d2.send_sms(user_id, clgcode, usercode, mobile, Msg, "0");
                    okcnt++;
                }
                else
                {
                    errcnt++;
                }
            }
            btngo_Click(sender, e);
            popSendSms.Attributes.Add("Style", "display:none;");
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('<u>Sms Details</u> <br><br>Sent :'" + okcnt + "'. Not Sent : '" + errcnt + "'')", true);
            // errorspan.InnerHtml = "<u>Sms Details</u> <br><br>Sent : " + okcnt + ". Not Sent : " + errcnt;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter Message')", true);
            //errorspan.InnerHtml = "Please Enter Message";
        }
    }
    protected void btnClosePop_click(object sender, EventArgs e)
    {
        popSendSms.Attributes.Add("Style", "display:none;");
    }
    //17.05.2017   
    protected void Okay_clcik(object sender, EventArgs e)
    {
        // List<string> appnolist = new List<string>();
        Dictionary<string, string> dtstud = new Dictionary<string, string>();
        if (getSmsStud(out  dtstud))
        {
            if (callLetterFormat() == 0)
            {
                loadprint();
            }
            else if (callLetterFormat() == 1)
            {
                loadNewCallLetter(dtstud);
            }
            Div3.Visible = false;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Student Selected!')", true);
        }
    }
    protected void Cancel_clcik(object sender, EventArgs e)
    {
        Div3.Visible = false;
    }
    public void btncallltrstud_Click(object sender, EventArgs e)
    {
        Txt_callDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        Txt_callDate.Attributes.Add("readonly", "readonly");
        txtPrepDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtPrepDate.Attributes.Add("readonly", "readonly");
        Div3.Visible = true;
        ddlIntMin.Items.Clear();
        for (int i = 0; i < 60; i++)
        {
            string iv = i.ToString();
            if (i.ToString().Length == 1)
                iv = "0" + i;
            ListItem lst = new ListItem(iv, i.ToString());
            ddlIntMin.Items.Add(lst);
        }
        ddlIntMin.SelectedIndex = 0;
        ddlIntHr.SelectedIndex = 0;
        ddlIntMed.SelectedIndex = 0;
        txtVenue.Text = "";
        txtddAmount.Text = "0";
        //For different formats
        Label10.Visible = false;
        Label6.Text = "Date";
        txtPrepDate.Visible = false;
        Label13.Visible = false;
        ddlIntHr.Visible = false;
        ddlIntMed.Visible = false;
        ddlIntMin.Visible = false;
        Label11.Visible = false;
        txtVenue.Visible = false;
        Label12.Visible = false;
        txtddAmount.Visible = false;
        if (callLetterFormat() == 0)
        {
            //Jamal
            Label6.Text = "Date";
            txtVenue.Text = "-";
        }
        else if (callLetterFormat() == 1)
        {
            //New College
            Label10.Visible = true;
            Label6.Text = "Interview Date";
            txtPrepDate.Visible = true;
            Label13.Visible = true;
            ddlIntHr.Visible = true;
            ddlIntMed.Visible = true;
            ddlIntMin.Visible = true;
            Label11.Visible = true;
            txtVenue.Visible = true;
            Label12.Visible = true;
            txtddAmount.Visible = true;
        }
    }
    private byte callLetterFormat()
    {
        //value 0 - Jamal, value 1 - New College
        byte format = 0;
        string callLetterQ = "select LinkValue from New_InsSettings where LinkName='AdmissionCallLetterSetting' and user_code ='" + usercode + "' ";
        format = Convert.ToByte(d2.GetFunction(callLetterQ).Trim());
        return format;
    }
    public void loadprint()
    {
        try
        {
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4); ;
            Font font16R = new Font("Arial", 16, FontStyle.Regular);
            Font font16B = new Font("Arial", 16, FontStyle.Bold);
            Font font12R = new Font("Arial", 12, FontStyle.Regular);
            Font font12R_Ti = new Font("Times New Roman", 12, FontStyle.Regular);
            Font font12B = new Font("Arial", 12, FontStyle.Bold);
            Font font14R = new Font("Arial", 14, FontStyle.Regular);
            Font font14B = new Font("Arial", 14, FontStyle.Bold);
            Boolean saveflag = false;
            //string sign = "principal" + ddlcollege.SelectedValue.ToString() + "";
            DataSet d_value = new DataSet();
            //string strquery = "select * from collinfo where college_code='" + college_code + "'";
            //ds.Dispose();
            //ds.Reset();
            //ds = d2.select_method_wo_parameter(strquery, "Text");
            string Collegename = "";
            string aff = "";
            string address = "";
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
            //    aff = "(Affiliated to " + ds.Tables[0].Rows[0]["university"].ToString() + ")";
            //    //address = ds.Tables[0].Rows[0]["address1"].ToString() + " , " + ds.Tables[0].Rows[0]["district"].ToString() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
            //    address = ds.Tables[0].Rows[0]["district"].ToString().ToUpper() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
            //}
            string deptText = string.Empty;
            foreach (GridViewRow gdrow in gridstud.Rows)
            {
                CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                if (cb.Checked)
                {
                    //  FpSpread1.SaveChanges();
                    //for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                    //{
                    //    int isval = 0;
                    //    isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value);
                    //    if (isval == 1)
                    //    {
                    string rollnonew = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text;
                    string collgcode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text;
                    string degreecode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 1].Text;
                    string strquery = "select * from collinfo where college_code='" + collgcode + "'";
                    ds.Dispose();
                    ds.Reset();
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                        aff = "(Affiliated to " + ds.Tables[0].Rows[0]["university"].ToString() + ")";
                        //address = ds.Tables[0].Rows[0]["address1"].ToString() + " , " + ds.Tables[0].Rows[0]["district"].ToString() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
                        address = ds.Tables[0].Rows[0]["district"].ToString().ToUpper() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
                    }
                    try
                    {
                        saveflag = true;
                        // string rollnonew = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString(); //app_formno
                        string type = string.Empty;
                        string degreeText = string.Empty;
                        string eduLevel = string.Empty;
                        deptText = string.Empty;
                        getdeptDetails(collgcode, degreecode, ref  type, ref  degreeText, ref  deptText, ref  eduLevel);
                        string name = d2.GetFunction("select stud_name from applyn where app_no ='" + rollnonew + "'");
                        string rollno = d2.GetFunction("select app_formno from applyn where app_no ='" + rollnonew + "'");
                        string deprt = deptText;
                        string course = degreeText;
                        //string deprt = Convert.ToString(ddldept.SelectedItem.Text);
                        //string course = Convert.ToString(ddldegree.SelectedItem.Text);
                        string degDep = course + " (" + deprt + ")";
                        Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                        Gios.Pdf.PdfDocument mydocnew = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                        Gios.Pdf.PdfPage mypdfpage1 = mydocnew.NewPage();
                        int ik = 1;
                        DateTime dt_date = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                        string updatequery = "update applyn set admitcard_date ='" + dt_date.ToString("MM/dd/yyyy") + "' where app_no ='" + rollnonew + "'";
                        int d = d2.update_method_wo_parameter(updatequery, "Text");
                        while (ik <= 2)
                        {
                            dt_date = dt_date.AddDays(1);
                            if (dt_date.ToString("dddd") == "Sunday")
                            {
                                dt_date = dt_date.AddDays(1);
                            }
                            ik++;
                        }
                        string sign = "principal" + collgcode + "";
                        string mail_id = "";
                        string stud_phoneno = "";
                        string mailidquery = "select StuPer_Id,Student_Mobile  from applyn where app_formno ='" + rollno + "'";
                        d_value.Clear();
                        d_value = d2.select_method_wo_parameter(mailidquery, "Text");
                        if (d_value.Tables[0].Rows.Count > 0)
                        {
                            mail_id = Convert.ToString(d_value.Tables[0].Rows[0]["StuPer_Id"]);
                            stud_phoneno = Convert.ToString(d_value.Tables[0].Rows[0]["Student_Mobile"]);
                        }
                        //string upadte = "update applyn set enroll_date='" + dten + "',feedate='" + dtfee + "',Is_Enroll='1' where app_formno='" + rollno + "'";
                        //int a = d2.update_method_wo_parameter(upadte, "Text");
                        int xvlaue = 40;
                        #region doc1
                        PdfArea tete = new PdfArea(mydoc, 10, 10, 570, 820);
                        PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                        mypdfpage.Add(pr1);
                        PdfTextArea ptc = new PdfTextArea(font16B, System.Drawing.Color.Black,
                                                                      new PdfArea(mydoc, 80, 20, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, Collegename);
                        mypdfpage.Add(ptc);
                        PdfTextArea ptcpot = new PdfTextArea(font12B, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 80, 40, 500, 20), System.Drawing.ContentAlignment.MiddleCenter, "College with Potential for Excellence");
                        mypdfpage.Add(ptcpot);
                        PdfTextArea ptcAcc = new PdfTextArea(font12R, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, 80, 60, 500, 20), System.Drawing.ContentAlignment.MiddleCenter, "Accredited with \"A\" Grade by NAAC - CGPA 3.6 out of 4.0");
                        mypdfpage.Add(ptcAcc);
                        PdfTextArea ptc02 = new PdfTextArea(font12R, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, 80, 80, 500, 20), System.Drawing.ContentAlignment.MiddleCenter, aff);
                        mypdfpage.Add(ptc02);
                        PdfTextArea ptc01 = new PdfTextArea(font14B, System.Drawing.Color.Black,
                                                                      new PdfArea(mydoc, 80, 100, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, address);
                        mypdfpage.Add(ptc01);
                        PdfTextArea ptcdt = new PdfTextArea(font12R, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 180, 130, 380, 20), System.Drawing.ContentAlignment.MiddleRight, "Date : " + DateTime.Now.Date.ToString("dd/MM/yyyy"));
                        mypdfpage.Add(ptcdt);
                        PdfTextArea ptc0265 = new PdfTextArea(font16B, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 100, 150, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "ADMISSION LETTER");
                        mypdfpage.Add(ptc0265);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 25, 25, 300);
                        }
                        PdfTextArea ptcappNo = new PdfTextArea(font14B, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, xvlaue, 170, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Application No : " + rollno.ToString());
                        mypdfpage.Add(ptcappNo);
                        PdfTextArea ptcappName = new PdfTextArea(font14B, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, xvlaue, 190, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name : " + name.ToString());
                        mypdfpage.Add(ptcappName);
                        PdfTextArea ptcMsg1 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, xvlaue + 20, 230, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "       We are happy to inform you that you have been provisionally selected for admission");
                        mypdfpage.Add(ptcMsg1);
                        PdfTextArea ptcMsg11 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, xvlaue + 20, 250, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "into the " + degDep + " course. You should appear before the Principal along");
                        mypdfpage.Add(ptcMsg11);
                        PdfTextArea ptcMsg12 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, xvlaue + 20, 270, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "with your parent on or before " + Txt_callDate.Text + "  with this letter, all the certificates (in original)");
                        mypdfpage.Add(ptcMsg12);
                        PdfTextArea ptcMsg13 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, xvlaue + 20, 290, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "mentioned in the prospectus and three (one stamp size and two passport size) photographs. ");
                        mypdfpage.Add(ptcMsg13);
                        //PdfTextArea ptcMsg14 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                        //                                         new PdfArea(mydoc, xvlaue, 310, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "");
                        //mypdfpage.Add(ptcMsg14);
                        PdfTextArea ptcMsg2 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, xvlaue + 20, 340, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "       You should, on being selected, remit the prescribed fees on the same day,  else you ");
                        mypdfpage.Add(ptcMsg2);
                        PdfTextArea ptcMsg21 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, xvlaue + 20, 360, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "forefeit your seat.");
                        mypdfpage.Add(ptcMsg21);
                        PdfTextArea ptcPrin = new PdfTextArea(font14B, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, xvlaue, 420, 500, 20), System.Drawing.ContentAlignment.MiddleRight, "PRINCIPAL");
                        mypdfpage.Add(ptcPrin);
                        #endregion
                        #region doc2
                        PdfArea ntete = new PdfArea(mydocnew, 10, 10, 570, 820);
                        PdfRectangle npr1 = new PdfRectangle(mydocnew, ntete, Color.Black);
                        mypdfpage1.Add(npr1);
                        PdfTextArea nptc = new PdfTextArea(font16B, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocnew, 80, 20, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, Collegename);
                        mypdfpage1.Add(nptc);
                        PdfTextArea nptcpot = new PdfTextArea(font12B, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocnew, 80, 40, 500, 20), System.Drawing.ContentAlignment.MiddleCenter, "College with Potential for Excellence");
                        mypdfpage1.Add(nptcpot);
                        PdfTextArea nptcAcc = new PdfTextArea(font12R, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocnew, 80, 60, 500, 20), System.Drawing.ContentAlignment.MiddleCenter, "Accredited with \"A\" Grade by NAAC - CGPA 3.6 out of 4.0");
                        mypdfpage1.Add(nptcAcc);
                        PdfTextArea nptc02 = new PdfTextArea(font12R, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocnew, 80, 80, 500, 20), System.Drawing.ContentAlignment.MiddleCenter, aff);
                        mypdfpage1.Add(nptc02);
                        PdfTextArea nptc01 = new PdfTextArea(font14B, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocnew, 80, 100, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, address);
                        mypdfpage1.Add(nptc01);
                        PdfTextArea nptcdt = new PdfTextArea(font12R, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocnew, 180, 130, 380, 20), System.Drawing.ContentAlignment.MiddleRight, "Date : " + DateTime.Now.Date.ToString("dd/MM/yyyy"));
                        mypdfpage1.Add(nptcdt);
                        PdfTextArea nptc0265 = new PdfTextArea(font16B, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocnew, 100, 150, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "ADMISSION LETTER");
                        mypdfpage1.Add(nptc0265);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                        {
                            PdfImage LogoImage = mydocnew.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage1.Add(LogoImage, 25, 25, 300);
                        }
                        PdfTextArea nptcappNo = new PdfTextArea(font14B, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocnew, xvlaue, 170, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Application No : " + rollno.ToString());
                        mypdfpage1.Add(nptcappNo);
                        PdfTextArea nptcappName = new PdfTextArea(font14B, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocnew, xvlaue, 190, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name : " + name.ToString());
                        mypdfpage1.Add(nptcappName);
                        PdfTextArea nptcMsg1 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocnew, xvlaue + 20, 230, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "       We are happy to inform you that you have been provisionally selected for admission");
                        mypdfpage1.Add(nptcMsg1);
                        PdfTextArea nptcMsg11 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocnew, xvlaue + 20, 250, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "into the " + degDep + " course. You should appear before the Principal along");
                        mypdfpage1.Add(nptcMsg11);
                        PdfTextArea nptcMsg12 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocnew, xvlaue + 20, 270, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "with your parent on or before " + Txt_callDate.Text + "  with this letter, all the certificates (in original)");
                        mypdfpage1.Add(nptcMsg12);
                        PdfTextArea nptcMsg13 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocnew, xvlaue + 20, 290, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "mentioned in the prospectus and three (one stamp size and two passport size) photographs. ");
                        mypdfpage1.Add(nptcMsg13);
                        //PdfTextArea ptcMsg14 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                        //                                         new PdfArea(mydocnew, xvlaue, 310, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "");
                        //mypdfpage1.Add(ptcMsg14);
                        PdfTextArea nptcMsg2 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocnew, xvlaue + 20, 340, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "       You should, on being selected, remit the prescribed fees on the same day,  else you ");
                        mypdfpage1.Add(nptcMsg2);
                        PdfTextArea nptcMsg21 = new PdfTextArea(font12R_Ti, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocnew, xvlaue + 20, 360, 500, 20), System.Drawing.ContentAlignment.MiddleLeft, "forefeit your seat.");
                        mypdfpage1.Add(nptcMsg21);
                        PdfTextArea nptcPrin = new PdfTextArea(font14B, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocnew, xvlaue, 420, 500, 20), System.Drawing.ContentAlignment.MiddleRight, "PRINCIPAL");
                        mypdfpage1.Add(nptcPrin);
                        #endregion
                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            ds.Dispose();
                            ds.Reset();
                            ds = d2.select_method_wo_parameter("select principal_sign from collinfo where college_code='" + Session["collegecode"] + "' and principal_sign is not null", "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])ds.Tables[0].Rows[0]["principal_sign"];
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
                        mypdfpage.SaveToDocument();
                        // mypdfpage1 = mypdfpage.CreateCopy();
                        mypdfpage1.SaveToDocument();
                        string appPath = HttpContext.Current.Server.MapPath("~");
                        if (appPath != "")
                        {
                            Response.Buffer = true;
                            Response.Clear();
                            string szPath = appPath + "/Report/";
                            string szFile = "" + rollno + ".pdf";
                            mydocnew.SaveToFile(szPath + szFile);
                        }
                        Div3.Visible = false;
                        //sendmail(mail_id, name, rollno, Collegename, new StringBuilder().Append("<br>Thank You</br>"));
                        //sendsms(stud_phoneno, rollno);
                        string Msg = "";
                        string getgroup = d2.GetFunction("select c.Course_Name+'('+dt.dept_acronym+')'  from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_formno ='" + rollno + "'");
                        string tot = sumttoal(rollno, collgcode);
                        Msg = " Application No : " + rollno + ". You are provisionally selected for " + getgroup + ". Meet the Principal with your Parent on or before " + Convert.ToString(Txt_callDate.Text) + " with original certificates. Remit the College Fees Rs." + tot + " ";
                        if (d2.GetFunction("select COLLNAME from collinfo where college_code='" + collgcode + "'").Trim().ToUpper().Contains("JAMAL MOHAMED"))
                        {
                            Msg += " and the Hostel Fees Rs.19350 on the same day. - PRINCIPAL, JMC";
                        }
                        else
                        {
                            Msg = "Application No:" + rollno + " You are provisionally selected for " + getgroup + ". Meet the Principal with your Parent at " + ddlIntHr.SelectedItem.Text + ":" + ddlIntMin.SelectedItem.Text + " " + ddlIntMed.SelectedItem.Text + " on " + Convert.ToString(Txt_callDate.Text) + " with original certificates and DD for Rs." + Convert.ToString(txtddAmount.Text) + "(Fee) ";
                        }
                        string user_id = "";
                        string ssr = "select * from Track_Value where college_code='" + Convert.ToString(collgcode) + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(ssr, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                        }
                        int d1 = d2.send_sms(user_id, collgcode, usercode, stud_phoneno, Msg, "0");
                    }
                    catch
                    {
                    }
                }
            }
            // FpSpread4.SaveChanges();
            if (saveflag == true)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    Response.Buffer = true;
                    Response.Clear();
                    string szPath = appPath + "/Report/";
                    string szFile = "" + deptText + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    //Response.ClearHeaders();
                    //Response.ClearHeaders();
                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    //Response.ContentType = "application/pdf";
                    //Response.WriteFile(szPath + szFile);
                    Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Call Letter Generated')", true);
                    //errorspan.InnerHtml = "Call Letter Generated";
                    //poperrjs.Visible = true;
                }
            }
        }
        catch
        {
        }
    }
    public string sumttoal(string applno, string collgcode)
    {
        string total = "";
        string textcode = "";
        // string link = "select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code='" + ddl_collegename.SelectedItem.Value + "'";
        string getfinid = d2.getCurrentFinanceYear(usercode, collgcode);
        string seattype = d2.GetFunction("select TextCode from TextValTable where TextCriteria ='Seat' and college_code  ='" + collgcode + "'");
        if (getfinid.Trim() != "" && getfinid.Trim() != "0" && seattype.Trim() != "" && seattype.Trim() != "0")
        {
            string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collgcode + "'");
            if (linkvalue == "0")
            {
                textcode = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '1 Semester' and textval not like '-1%' and college_code ='" + collgcode + "' order by textval asc");
            }
            else
            {
                textcode = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '1 Year' and textval not like '-1%' and college_code ='" + collgcode + "'");
            }
            string GetDegreeCodeQuery = "";
            //if (cbAltCourse.Checked == true)
            //{
            //    GetDegreeCodeQuery = d2.GetFunction("select  Alternativedegree_code  from applyn where app_formno ='" + applno + "'");
            //}
            //else
            //{
            GetDegreeCodeQuery = d2.GetFunction("select Degree_code from applyn where app_formno ='" + applno + "'");
            // }
            string qur = d2.GetFunction("select SUM(TotalAmount) from FT_FeeAllotDegree where DegreeCode='" + GetDegreeCodeQuery + "' and BatchYear ='" + Convert.ToString(ddl_batch.SelectedItem.Value) + "' and SeatType ='" + seattype + "' and FeeCategory ='" + textcode + "' and FinYearFK ='" + getfinid + "'");
            if (qur.Trim() != "0" && qur.Trim() != "")
            {
                total = qur.ToString();
            }
        }
        return total;
    }
    private void loadNewCallLetter(Dictionary<string, string> appNoList)
    {
        try
        {
            //string colQ = "select * from collinfo where college_code='" + college_code + "'";
            //DataSet dsCol = new DataSet();
            //dsCol = d2.select_method_wo_parameter(colQ, "Text");
            string collegeName = string.Empty;
            string collegeCateg = string.Empty;
            string collegeAff = string.Empty;
            string collegeAdd = string.Empty;
            string collegePhone = string.Empty;
            string collegeFax = string.Empty;
            string collegeWeb = string.Empty;
            string collegeEmai = string.Empty;
            string collegePin = string.Empty;
            string City = string.Empty;
            //if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
            //{
            //    collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            //    City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            //    collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            //    collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            //    collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            //    collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            //    collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            //    collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            //    collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            //    collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
            //}
            string prepDate = txtPrepDate.Text;//Prepared date
            string intDate = Txt_callDate.Text;//Interview date
            string intTime = ddlIntHr.SelectedItem.Text + ":" + ddlIntMin.SelectedItem.Text + " " + ddlIntMed.SelectedItem.Text;
            string[] prepDateAr = txtPrepDate.Text.Split('/');
            DateTime prepDateDt = Convert.ToDateTime(prepDateAr[1] + "/" + prepDateAr[0] + "/" + prepDateAr[2]);
            string[] intDateAr = Txt_callDate.Text.Split('/');
            DateTime intDateDt = Convert.ToDateTime(intDateAr[1] + "/" + intDateAr[0] + "/" + intDateAr[2]);
            string intDay = intDateDt.DayOfWeek.ToString();
            decimal ddAmt = 0;
            decimal.TryParse(txtddAmount.Text, out ddAmt);
            string ddAmtStr = DecimalToWords(ddAmt);
            string venue = txtVenue.Text;
            contentDiv.InnerHtml = "";
            foreach (KeyValuePair<string, string> appValue in appNoList)
            {
                string appNo = Convert.ToString(appValue.Key);
                string collegcode = Convert.ToString(appValue.Value);
                string colQ = "select * from collinfo where college_code='" + collegcode + "'";
                DataSet dsCol = new DataSet();
                dsCol = d2.select_method_wo_parameter(colQ, "Text");
                if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
                {
                    collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
                    City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
                    collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
                    collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
                    collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
                    collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
                    collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
                    collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
                    collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
                    collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
                }
                string queryNewvalue = "select stud_name,app_formno,parent_addressP,Streetp,cityp,parent_pincodep from applyn where app_no ='" + appNo + "'";
                string Namevalue = "";
                string Addressvalue = "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(queryNewvalue, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]).Trim() != "")
                    {
                        Namevalue = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]) + " (Appl. ID :" + Convert.ToString(ds.Tables[0].Rows[0]["app_formno"]) + ")";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]).Trim() != "")
                    {
                        Addressvalue = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]);
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]).Trim() != "")
                    {
                        Addressvalue = Addressvalue + ", " + Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]);
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["cityp"]).Trim() != "")
                    {
                        Addressvalue = Addressvalue + ", " + Convert.ToString(ds.Tables[0].Rows[0]["cityp"]);
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]).Trim() != "")
                    {
                        Addressvalue = Addressvalue + " - " + Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]);
                    }
                }
                string degCode = returnStudDeg(appNo);
                string course = d2.GetFunction("select type+' '+course_name+'('+Dept_name+')' from course c,Degree d,Department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code='" + degCode + "'").Trim();
                if (course == "0")
                {
                    course = string.Empty;
                }
                StringBuilder sbHtml = new StringBuilder();
                sbHtml.Append("<div style='padding-left:5px;height: 900px; width:650px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 650px; padding-left:15px; font-family:Times New Roman; font-size:16px;'><tr><td rowspan='5'><img src='" + "college/Left_Logo.jpeg" + "' style='height:80px; width:80px;'/></td><td colspan='7' style='align:center'>" + collegeName + " " + collegeCateg + "</td></tr><tr><td colspan='7' style='align:center'>" + collegeAff + "</td></tr><tr><td colspan='7' style='align:center'>" + collegeAdd + "</td></tr><tr><td colspan='7' style='align:center'>" + collegePhone + " " + collegeFax + "</td></tr><tr><td colspan='7' style='align:center'>" + collegeWeb + " " + collegeEmai + "</td></tr><tr><td colspan='8'><hr style='height:2px; width:650px;'></td></tr></table>");
                string[] splitconlname = collegeName.Split('(');
                collegeName = splitconlname[0];
                string mphilPgAppend = "<tr><td colspan='8'>•	10th Std Mark Statement & HSC/Equivalent Course Mark Statement along with 3 photocopies of each. (Provisional Certificate in the case of  March " + DateTime.Now.Year + " candidates). </td></tr>";
                string pgchk = d2.GetFunction("select Upper(c.EDU_LEVEL) from degree d, course c,department dt,applyn a where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=a.degree_code and a.app_no=" + appNo + "").Trim().ToUpper();
                if (pgchk.Trim().ToUpper() == "PG")
                {
                    mphilPgAppend = "<tr><td colspan='8'>•	10th Std Mark Statement , HSC/Equivalent Course Mark Statement, UG Mark Statement of Provisional Certificate along with 3 photocopies of each.(Provisional Certificate in the case of March " + DateTime.Now.Year + " candidates)</td></tr>";
                }
                string MphilChk = d2.GetFunction("select Upper(c.EDU_LEVEL) from degree d, course c,department dt,applyn a where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=a.degree_code and a.app_no=" + appNo + "").Trim().ToUpper();
                if (MphilChk.Trim().ToUpper() == "M.PHIL" || MphilChk.Trim().ToUpper() == "M.PHIL." || MphilChk.Trim().ToUpper() == "MPHIL" || MphilChk.Trim().ToUpper() == "MPHIL.")
                {
                    mphilPgAppend = "<tr><td colspan='8'>•	10th Std Mark Statement , HSC/Equivalent Course Mark Statement, UG Mark Statement of Provisional Certificate , PG mark statement along with 3 photocopies of each.(Provisional Certificate in the case of March " + DateTime.Now.Year + " candidates)</td></tr>";
                }
                sbHtml.Append("<table cellpadding='2' cellspacing='0' style=' width: 650px; padding-left:15px;font-family:Times New Roman; font-size:14px;text-align:justify;'><tr><td colspan='8' style='text-align:center;' ><span style='height:30px; width:230px; border:1px solid black; font-size:16px; font-weight:bold;'>INTERVIEW LETTER</span></td></tr><tr><td colspan='8' style='text-align:right;'><span style='font-size:16px;font-weight:bold;'>" + prepDate + "</span></td></tr><tr><td colspan='8' style='align:left;'>To</td></tr><tr><td colspan='8' style='align:right;'>" + Namevalue + ",</td></tr><tr><td colspan='8' style='align:right;'>" + Addressvalue + ",</td></tr><tr><td colspan='8' style='align:right;'>Dear candidate,</td></tr><tr><td colspan='8' style='align:left; text-indent:20px;'><p>You have been provisionally selected for admission to <span style='font-size:16px;font-weight:bold;'>" + course + "</span></p></td></tr><tr><td colspan='8' style='align:left;'><b>*<u> Date & Time of Interview:</u></b> <span>" + intDay + ", " + intDate + " at " + intTime + "</span></td></tr><tr><td colspan='8' style='align:left;'><b>*<u>  Venue:</u></b> <span>" + venue.ToUpper() + "</span></td></tr><tr><td colspan='8' style='align:left;'><u><b>Documents to be produced at the time of interview:</b></u></td></tr><tr><td colspan='8' style='align:left;'><u><b># For Submission: </b></u></td></tr><tr><td colspan='8'><span style='font-weight:bold;'>* A crossed DD for Rs." + ddAmt + " (Rupees " + ddAmtStr + " only) drawn in favour of \" <span style='text-transform:capitalize;'>" + collegeName + "," + City + " " + collegePin + "\"</span>.</span></td></tr><tr><td colspan='8'><span style='font-weight:bold;font-family:courier new; font-size:11px;'>[The Demand Draft can be purchased from any Nationalised Bank. The candidate need not wait till the Interview Date to purchase the draft. It is advised to purchase the Demand Draft two or three days ahead of the interview date to have the DD ready for submission.]</span></td></tr>" + mphilPgAppend + "<tr><td colspan='8'>•	Transfer Certificate (Original plus 3 photocopies).</td></tr><tr><td colspan='8'>•	Three <b>recently taken</b> Passport Size Photographs (Size: 3.5 cm x 4.5 cm) – with <b>Sky Blue Background.</b></td></tr><tr><td colspan='8'>•	<u>Soft Copy of the Photograph (as mentioned above - in JPEG Format) saved in a CD. </u></td></tr><tr><td colspan='8'>•	 Eligibility Certificate from the University of Madras (applicable for candidates who have qualified from other State Boards/Universities).</td></tr><tr><td colspan='8'>•	Photocopies of all the documents required to be produced for <b>verification (see below)</b>.</td></tr><tr><td colspan='8' style='align:left;'><u><b># For Verification: </b></u></td></tr><tr><td colspan='8' style='align:left;'>•	Aadhar Card.</td></tr><tr><td colspan='8' style='align:left;'>•	Any other valid Identity Card for Address Proof.</td></tr><tr><td colspan='8'>•	Birth Certificate.</td></tr><tr><td>•	Community Certificate.</td></tr><tr><td colspan='8'>•	Documents in support of Distinction/participation in Sports/Athletics/NCC/NSS.</td></tr><tr><td colspan='8'>•	Differently-abled/Sons of Ex-Servicemen shall submit relevant documents. </td></tr><tr><td colspan='8'>-	Failing to turn up on the above mentioned Time & Date with necessary documents and Demand Draft (as mentioned above) would imply forfeiture of the seat allotted to you.</td></tr><tr><td colspan='8'>-	Fees once paid will not be refunded.</td></tr><tr><td colspan='8' style='font-weight:bold;font-family:courier new; font-size:11px;text-align:justify;'> -	மேற்குறிப்பிடப்பட்ட நேர்காணலுக்கு உரிய நாள் மற்றும் நேரத்தில் உரிய சான்றிதழ்களுடனும் கல்விக்கட்டணத் தொகைக்கான வரைவோலையுடனும் வரத்தவறும் மாணவர்கள் தங்களுக்கு ஒதுக்கப்பட்ட இடத்தை இழந்து விடுவர். அந்த இடம் தகுதியுள்ள வேறு ஒரு மாணவருக்கு உடனே வழங்கப்படும்.</td></tr><tr><td colspan='8' style='font-weight:bold;font-family:courier new; font-size:11px; text-align:justify;'>-	செலுத்தப்பட்ட கல்விக் கட்டணத் தொகையினைத் திருப்பித்தர இயலாது.</td></tr><tr><td colspan='8' style='text-align:right; font-weight:bold;font-size:14px;'>Principal</td></tr></table></div>");
                contentDiv.InnerHtml += sbHtml.ToString();
                string name = d2.GetFunction("select stud_name from applyn where app_no ='" + appNo + "'");
                string rollno = d2.GetFunction("select app_formno from applyn where app_no ='" + appNo + "'");
                string mail_id = d2.GetFunction("select StuPer_Id from applyn where app_no ='" + appNo + "'");
                string stud_phoneno = d2.GetFunction("select Student_Mobile from applyn where app_no ='" + appNo + "'");
                sendmail(mail_id, name, rollno, collegeName, sbHtml, collegcode);
                //sendsms(stud_phoneno, rollno);
                string Msg = "";
                string getgroup = d2.GetFunction("select c.Course_Name+'('+dt.dept_acronym+')'  from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_formno ='" + rollno + "'");
                string tot = sumttoal(rollno, collegcode);
                Msg = " Application No : " + rollno + ". You are provisionally selected for " + getgroup + ". Meet the Principal with your Parent on or before " + Convert.ToString(Txt_callDate.Text) + " with original certificates. Remit the College Fees Rs." + tot + " ";
                if (d2.GetFunction("select COLLNAME from collinfo where college_code='" + collegcode + "'").Trim().ToUpper().Contains("JAMAL MOHAMED"))
                {
                    Msg += " and the Hostel Fees Rs.19350 on the same day. - PRINCIPAL, JMC";
                }
                else
                {
                    Msg = "Application No:" + rollno + " You are provisionally selected for " + getgroup + ". Meet the Principal with your Parent at " + ddlIntHr.SelectedItem.Text + ":" + ddlIntMin.SelectedItem.Text + " " + ddlIntMed.SelectedItem.Text + " on " + Convert.ToString(Txt_callDate.Text) + " with original certificates and DD for Rs." + Convert.ToString(txtddAmount.Text) + "(Fee) ";
                }
                string user_id = "";
                string ssr = "select * from Track_Value where college_code='" + collegcode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(ssr, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                }
                int d1 = d2.send_sms(user_id, collegcode, usercode, stud_phoneno, Msg, "0");
            }
            contentDiv.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Call Letter Generated')", true);
            //  errorspan.InnerHtml = "Call Letter Generated";
            // poperrjs.Visible = true;
        }
        catch { }
    }
    public string DecimalToWords(decimal number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "Minus " + DecimalToWords(Math.Abs(number));
        string words = "";
        int intPortion = (int)number;
        decimal fraction = (number - intPortion) * 100;
        int decPortion = (int)fraction;
        words = ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
        if (decPortion > 0)
        {
            words += " And ";
            words += ConvertNumbertoWords(decPortion);//NumberToWords(intPortion)
            words += " Paise ";
        }
        return words;
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " Lakhs";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + "  Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    public void sendmail(string mail, string name, string app, string collegename, StringBuilder mailMessage, string collegcode)
    {
        try
        {
            string send_mail = "";
            string send_pw = "";
            string to_mail = Convert.ToString(mail);
            string subtext = collegename + " Admission-Regarding";
            string strstuname = Convert.ToString(name);
            string strquery = "select massemail,masspwd from collinfo where college_code = " + collegcode + " ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                send_mail = Convert.ToString(ds.Tables[0].Rows[0]["massemail"]);
                send_pw = Convert.ToString(ds.Tables[0].Rows[0]["masspwd"]);
            }
            if (send_mail.Trim() != "" && send_pw.Trim() != "" && to_mail.Trim() != "")
            {
                SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mailmsg = new MailMessage();
                MailAddress mfrom = new MailAddress(send_mail);
                mailmsg.From = mfrom;
                mailmsg.To.Add(to_mail);
                mailmsg.Subject = subtext;
                mailmsg.IsBodyHtml = true;
                // mailmsg.Body = "Hi";
                //mailmsg.Body = mailmsg.Body + strstuname;
                //mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                mailmsg.Body = mailMessage.ToString();
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = app + ".pdf";
                    string attachementpath = szPath + szFile;
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/Report/" + szFile + "")))
                    {
                        Attachment data = new Attachment(attachementpath);
                        mailmsg.Attachments.Add(data);
                    }
                }
                Mail.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                Mail.UseDefaultCredentials = false;
                Mail.Credentials = credentials;
                Mail.Send(mailmsg);
            }
        }
        catch
        {
        }
    }
    //reject and left   
    protected void btnrejstud_Click(object sender, EventArgs e)
    {
        try
        {
            bool boolCheck = false;
            foreach (GridViewRow gdrow in gridstud.Rows)
            {
                CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                if (cb.Checked)
                {
                    string appNo = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text;
                    collegecode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text;
                    string degreeCode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 1].Text;
                    //Session["stdadmitappno"] = appNo;
                    //Session["stdadmitdegcode"] = degreeCode;
                    Session["stdrejclgcode"] = collegecode;
                    boolCheck = true;
                }
            }
            if (boolCheck)
            {
                // panel2.Visible = true;
                panel2.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px; position: absolute; top: 33px; width: 101%;display:block;");
                panel9.Visible = false;
                panel11.Visible = true;
                Panel10.Visible = false;
                reason1();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Choose Atleast One Student And Than Proceed')", true);
            }
        }
        catch (Exception ex)
        { }
    }
    public void reason1()
    {
        try
        {
            string collegecode = string.Empty;
            if (Session["stdrejclgcode"] != null)
                collegecode = Convert.ToString(Session["stdrejclgcode"]);
            btnrejectreason.Items.Clear();
            ds = d2.select_method_wo_parameter("select distinct TextCode,TextVal from textvaltable where TextCriteria = 'reres' and college_code = '" + collegecode + "'", "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                btnrejectreason.DataSource = ds;
                btnrejectreason.DataTextField = "TextVal";
                btnrejectreason.DataValueField = "TextCode";
                btnrejectreason.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void reason()
    {
        try
        {
            string collegecode = string.Empty;
            if (Session["stdrejclgcode"] != null)
                collegecode = Convert.ToString(Session["stdrejclgcode"]);
            ddlreason.Items.Clear();
            ds = d2.select_method_wo_parameter("select distinct TextCode,TextVal from textvaltable where TextCriteria = 'adres' and college_code = '" + collegecode + "'", "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlreason.DataSource = ds;
                ddlreason.DataTextField = "TextVal";
                ddlreason.DataValueField = "TextCode";
                ddlreason.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnreason_click(object sender, EventArgs e)
    {
        // panel2.Visible = false;
        panel2.Attributes.Add("Style", "display:none;");
    }
    protected void btnadd_Click(object sender, EventArgs e)
    {
        reason();
        Panel10.Visible = true;
    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {
            string collegecode = string.Empty;
            if (Session["stdrejclgcode"] != null)
                collegecode = Convert.ToString(Session["stdrejclgcode"]);
            string add = "delete from textvaltable where TextCode='" + ddlreason.SelectedValue + "'and TextCriteria='adres' and college_code='" + collegecode + "' ";
            int a = d2.update_method_wo_parameter(add, "text");
            reason();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnadd1_Click(object sender, EventArgs e)
    {
        try
        {
            string collegecode = string.Empty;
            if (Session["stdrejclgcode"] != null)
                collegecode = Convert.ToString(Session["stdrejclgcode"]);
            string add = " if exists(select * from textvaltable where TextVal='" + txtadd.Text + "' and TextCriteria='adres'and college_code='" + collegecode + "' ) update textvaltable set TextVal='" + txtadd.Text + "',TextCriteria='adres',college_code='" + collegecode + "' where TextVal='" + txtadd.Text + "' and TextCriteria='adres'and college_code='" + collegecode + "' else insert into textvaltable (TextVal,TextCriteria,college_code) values ('" + txtadd.Text + "','adres','" + collegecode + "')";
            int a = d2.update_method_wo_parameter(add, "text");
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Added Successfully')", true);
            reason();
            txtadd.Text = "";
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnrjt_click(object sender, EventArgs e)
    {
        // panel2.Visible = false;
        panel2.Attributes.Add("Style", "display:none;");
        TextBox1.Text = "";
    }
    protected void btnexit1_Click(object sender, EventArgs e)
    {
        Panel10.Visible = false;
        txtadd.Text = "";
        reason();
    }
    protected void btnaddrejt_Click(object sender, EventArgs e)
    {
        reason1();
        Panel12.Visible = true;
        panel2.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px; position: absolute; top: 33px; width: 101%;display:block;");
    }
    protected void btnminusrejt_Click(object sender, EventArgs e)
    {
        try
        {
            string collegecode = string.Empty;
            if (Session["stdrejclgcode"] != null)
                collegecode = Convert.ToString(Session["stdrejclgcode"]);
            string add = "delete from textvaltable where TextCode='" + btnrejectreason.SelectedValue + "'and TextCriteria='reres' and college_code='" + collegecode + "' ";
            int a = d2.update_method_wo_parameter(add, "text");
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            reason1();
            panel2.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px; position: absolute; top: 33px; width: 101%;display:block;");
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnrct_Click(object sender, EventArgs e)
    {
        try
        {
            bool boolCheck = false;
            if (rdbtype.SelectedIndex == 1)
            {
                foreach (GridViewRow gdrow in gridstud.Rows)
                {
                    CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                    if (cb.Checked)
                    {
                        boolCheck = true;
                        string appNo = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text;
                        string collegecode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text;
                        string sel = "select * from textvaltable where TextVal='" + btnrejectreason.SelectedItem.Text + "' and TextCriteria='reres'and college_code='" + collegecode + "'";
                        ds = d2.select_method_wo_parameter(sel, "text");
                        string textcode = d2.GetFunction("select textcode from textvaltable where TextCriteria='reres'and college_code='" + collegecode + "' and TextVal='" + btnrejectreason.SelectedItem.Text + "'");//added by abarna 10.01.2019
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string approve = "update selectcriteria set isapprove='0',isview='0',usercode='" + usercode + "', select_date='" + System.DateTime.Now.ToString("yyy/MM/dd") + "',textcode='" + ds.Tables[0].Rows[0]["TextCode"].ToString() + "' where app_no='" + appNo + "' and degree_code='" + returnStudDeg(appNo) + "' and college_code='" + collegecode + "' ";
                            approve = approve + "update applyn set admission_status='0',selection_status='0',reject_flag='1',reject_reason='" + textcode + "' where app_no ='" + appNo + "'";//added by abarna 10.01.2019

                            int a = d2.update_method_wo_parameter(approve, "text");
                        }
                    }
                }
            }
            if (rdbtype.SelectedIndex == 2)
            {
                foreach (GridViewRow gdrow in gridstud.Rows)
                {
                    CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                    if (cb.Checked)
                    {
                        boolCheck = true;
                        string appNo = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text;
                        string collegecode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text;
                        string sel = "select * from textvaltable where TextVal='" + btnrejectreason.SelectedItem.Text + "' and TextCriteria='reres'and college_code='" + collegecode + "'";
                        ds = d2.select_method_wo_parameter(sel, "text");
                        string textcode = d2.GetFunction("select textcode from textvaltable where TextCriteria='reres'and college_code='" + collegecode + "' and TextVal='" + btnrejectreason.SelectedItem.Text + "'");//added by abarna 10.01.2019
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string approve = "update selectcriteria set isapprove='0',isview='0',usercode='" + usercode + "', select_date='" + System.DateTime.Now.ToString("yyy/MM/dd") + "',textcode='" + ds.Tables[0].Rows[0]["TextCode"].ToString() + "' where app_no='" + appNo + "' and degree_code='" + returnStudDeg(appNo) + "' and college_code='" + collegecode + "' ";
                            approve = approve + "update applyn set admission_status='0',selection_status='0' where app_no ='" + appNo + "'";
                            approve = approve + " delete from FT_FeeAllot where App_No ='" + appNo + "'";
                            approve = approve + " if exists(select * from ft_findailytransaction where App_No ='" + appNo + "') delete from ft_findailytransaction where App_No ='" + appNo + "'";
                            approve = approve + "update applyn set admission_status='0',selection_status='0',reject_flag='1',reject_reason='" + textcode + "' where app_no ='" + appNo + "'";//added by abarna 10.01.2019
                            int a = d2.update_method_wo_parameter(approve, "text");
                        }
                    }
                }
            }
            if (boolCheck)
            {
                btngo_Click(sender, e);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Reject Successfully')", true);
                // panel2.Visible = false;
                panel2.Attributes.Add("Style", "display:none;");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Choose Atleast One Student And Than Proceed')", true);
                // panel2.Visible = false;
                panel2.Attributes.Add("Style", "display:none;");
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnadd1ret_Click(object sender, EventArgs e)
    {
        try
        {
            string collegecode = string.Empty;
            if (Session["stdrejclgcode"] != null)
                collegecode = Convert.ToString(Session["stdrejclgcode"]);
            string add = " if exists(select * from textvaltable where TextVal='" + TextBox1.Text + "' and TextCriteria='reres'and college_code='" + collegecode + "' ) update textvaltable set TextVal='" + TextBox1.Text + "',TextCriteria='reres',college_code='" + collegecode + "' where TextVal='" + TextBox1.Text + "' and TextCriteria='reres'and college_code='" + collegecode + "' else insert into textvaltable (TextVal,TextCriteria,college_code) values ('" + TextBox1.Text + "','reres','" + collegecode + "')";
            int a = d2.update_method_wo_parameter(add, "text");
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Added Successfully')", true);
            reason1();
            txtadd.Text = "";
            Panel12.Visible = false;
            panel2.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px; position: absolute; top: 33px; width: 101%;display:block;");
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnexit1rejt_Click(object sender, EventArgs e)
    {
        Panel12.Visible = false;
        TextBox1.Text = "";
        panel2.Attributes.Add("Style", "background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83); border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px; position: absolute; top: 33px; width: 101%;display:block;");
    }
    //left
    protected void btnleftstud_Click(object sender, EventArgs e)
    {
        try
        {
            bool boolCheck = false;
            foreach (GridViewRow gdrow in gridstud.Rows)
            {
                CheckBox cb = (CheckBox)gdrow.FindControl("cb");
                if (cb.Checked)
                {
                    string appNo = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 3].Text;
                    collegecode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 2].Text;
                    string degreeCode = gdrow.Cells[gridstud.Rows[gdrow.RowIndex].Cells.Count - 1].Text;
                    Session["studclgcode"] = collegecode;
                    string UpdQ = string.Empty;
                    if (AdmConfFormat() == 1)
                    {
                        UpdQ = "update applyn set Admission_Status='2',AdmitedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appNo + "'";
                    }
                    else
                    {
                        UpdQ = "update Registration set DelFlag=2 where App_No='" + appNo + "'";
                        UpdQ = UpdQ + " update applyn set Admission_Status='2',AdmitedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appNo + "'";
                    }
                    int upd = d2.update_method_wo_parameter(UpdQ, "Text");
                    boolCheck = true;
                }
            }
            if (boolCheck)
            {

                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Lefted Successfully')", true);
                btngo_Click(sender, e);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Any Any One Students')", true);
            }
        }
        catch { }
    }
    //student auto search
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = string.Empty;
        if (autoByte == 0)
            query = "select stud_name from applyn where isconfirm ='1' and isnull(selection_status,'0')='0' and isnull(admission_status,'0')='0' and stud_name like '" + prefixText + "%'";
        else if (autoByte == 1)
            query = "select stud_name from applyn where isconfirm ='1' and isnull(selection_status,'0')='1' and isnull(admission_status,'0')='0' and stud_name like '" + prefixText + "%'";
        else if (autoByte == 2)
            query = "select stud_name from applyn where isconfirm ='1' and isnull(selection_status,'0')='1' and isnull(admission_status,'0')='1' and stud_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getmob(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = string.Empty;
        if (autoByte == 0)
            query = "select Student_Mobile from applyn where isconfirm ='1' and isnull(selection_status,'0')='0' and isnull(admission_status,'0')='0' and Student_Mobile like '" + prefixText + "%'";
        else if (autoByte == 1)
            query = "select Student_Mobile from applyn where isconfirm ='1' and isnull(selection_status,'0')='1' and isnull(admission_status,'0')='0' and Student_Mobile like '" + prefixText + "%'";
        else if (autoByte == 2)
            query = "select Student_Mobile from applyn where isconfirm ='1' and isnull(selection_status,'0')='1' and isnull(admission_status,'0')='1' and Student_Mobile like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getappfrom(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = string.Empty;
        if (autoByte == 0)
            query = "select app_formno from applyn where isconfirm ='1' and isnull(selection_status,'0')='0' and isnull(admission_status,'0')='0' and app_formno like '" + prefixText + "%'";
        else if (autoByte == 1)
            query = "select app_formno from applyn where isconfirm ='1' and isnull(selection_status,'0')='1' and isnull(admission_status,'0')='0' and app_formno like '" + prefixText + "%'";
        else if (autoByte == 2)
            query = "select app_formno from applyn where isconfirm ='1' and isnull(selection_status,'0')='1' and isnull(admission_status,'0')='1' and app_formno like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    public void txt_studName_TextChanged(object sender, EventArgs e)
    {
        if (txt_studName.Text != "")
        {
            txt_studMblno.Text = "";
            txt_studApplNo.Text = "";
            btngo_Click(sender, e);
        }
    }
    public void txt_studApplNo_TextChanged(object sender, EventArgs e)
    {
        if (txt_studApplNo.Text != "")
        {
            txt_studMblno.Text = "";
            txt_studName.Text = "";
            btngo_Click(sender, e);
        }
    }
    public void txt_studMblno_TextChanged(object sender, EventArgs e)
    {
        if (txt_studMblno.Text != "")
        {
            txt_studApplNo.Text = "";
            txt_studName.Text = "";
            btngo_Click(sender, e);
        }
    }
    protected void printCollegeDet()
    {
        try
        {
            string collegecode = string.Empty;
            string colgcode = Convert.ToString(getCblSelectedValue(cblclg));
            if (!colgcode.Contains(','))
                collegecode = colgcode;
            else
            {
                bool boolCheck = false;
                for (int row = 0; row < cblclg.Items.Count; row++)
                {
                    if (cblclg.Items[row].Selected && !boolCheck)
                        collegecode = Convert.ToString(cblclg.Items[row].Value);
                }
            }
            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode + " ";
            string academicyear = d2.GetFunctionv("select value from master_settings where settings='Academic year'");
            academicyear = academicyear.Trim().Trim(',').Replace(",", "-");
            string collegename = "";
            string add1 = "";
            string add2 = "";
            string add3 = "";
            string univ = "";
            string feedet = "";
            ds = d2.select_method_wo_parameter(colquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                add1 += " " + add2;
                spCollege.InnerText = collegename;
                spAffBy.InnerText = add1;
                spController.InnerText = add3;
                spSeating.InnerText = univ;
                // spDateSession.InnerText = "PRE-PRIMARY COMPARTMENT";
                sprptnamedt.InnerText = "STUDENTS REPORT--" + academicyear + "";
                spdate.InnerText = DateTime.Now.ToString("dd.MM.yyyy");
                //spdate.InnerText = "STUDENTS ATTENDANCE CONSOLIDATION--" + academicyear + "";
            }
        }
        catch { }
    }
    protected void certificate_grid_SelectedIndexChanged(object sender, EventArgs e)
    {
        string query = " select Certificate_Name as [Certificate Name],FileName,Filetype,Attach_Doc from Stud_Certificate_Det where  app_no='" + Convert.ToString(Session["pdfapp_no"]) + "' and Certificate_Name ='" + Convert.ToString(certificate_grid.SelectedRow.Cells[1].Text) + "' ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            string filename = "";
            string filetype = "";
            filename = Convert.ToString(ds.Tables[0].Rows[0]["FileName"]).Replace(" ", "").Trim();
            filetype = Convert.ToString(ds.Tables[0].Rows[0]["Filetype"]);
            if (filetype == "application/pdf")
                filename = filename + ".pdf";
            else if (filetype == "image/jpg")
                filename = filename + ".jpg";
            if (filename.Trim() != "")
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = filetype;
                Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite((byte[])ds.Tables[0].Rows[0]["Attach_Doc"]);
                Response.End();
            }
        }
    }
    protected void communitity_grid_SelectedIndexChanged(object sender, EventArgs e)
    {
        string query = " select communitycertificate  from StdPhoto where app_no='" + Convert.ToString(Session["pdfapp_no"]) + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            string filename = "communitycertificate";
            string filetype = "application/pdf";
            //filename = Convert.ToString(ds.Tables[0].Rows[0]["FileName"]).Replace(" ", "").Trim();
            //filetype = Convert.ToString(ds.Tables[0].Rows[0]["Filetype"]);
            if (filetype == "application/pdf")
                filename = filename + ".pdf";
            else if (filetype == "image/jpg")
                filename = filename + ".jpg";
            if (filename.Trim() != "")
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = filetype;
                Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite((byte[])ds.Tables[0].Rows[0]["communitycertificate"]);
                Response.End();
            }
        }
    }
    //19.05.17 barath
    private bool isFinanceLink()
    {
        bool format = false;
        string ShowFinQ = "select LinkValue from New_InsSettings where LinkName='IncludeFinanceLinkInAdmission' and user_code ='" + usercode + "' ";
        format = Convert.ToByte(d2.GetFunction(ShowFinQ).Trim()) == 1 ? true : false;
        return format;
    }
    protected string IsGeneralFeeAllot()
    {
        string formatevalue = string.Empty;
        formatevalue = d2.GetFunction("select value from Master_Settings where settings='GeneralFeeAllot' and usercode='" + usercode + "'");
        return formatevalue;
    }
    protected void verification_Databoud(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        catch
        {
        }
    }
    protected void verification_pgDataboud(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        catch
        {
        }
    }
    public void travelAllotment(string appnumber, string type, string collegecode)
    {
        try
        {
            string sqlcmd = "";
            string routeid = "";
            string Dep_Time = "";
            string Arr_Time = "";
            string addrouteid = "";
            string conc = "";
            double duration = 0;
            string routid_dur = "";
            string Stage_id = "";
            string Veh_ID = "";
            string veh_ids = "";
            ArrayList route = new ArrayList();
            sqlcmd = " (select distinct v.Veh_ID,r.Route_ID,s.Stage_Name,Stage_id,Arr_Time,Dep_Time,Stages,TotalNo_Seat,nofstudents,nofStaffs from vehicle_master v,routemaster r,stage_master s";
            sqlcmd = sqlcmd + " where v.veh_id=r.veh_id and v.route=r.route_id and convert(varchar(50),s.Stage_id)=(r.Stage_Name)";
            sqlcmd = sqlcmd + " and college_code like'%" + collegecode + "%' and s.stage_id='" + Convert.ToString(ddl_boarding.SelectedItem.Value) + "' and sess='M')";
            sqlcmd = sqlcmd + " UNION ";
            sqlcmd = sqlcmd + " (select distinct v.Veh_ID,r.Route_ID,s.Stage_Name,Stage_id,Arr_Time,Dep_Time,Stages,TotalNo_Seat,nofstudents,nofStaffs from vehicle_master v,routemaster r,stage_master s";
            sqlcmd = sqlcmd + " where v.veh_id=r.veh_id and v.route=r.route_id and convert(varchar(50),s.Stage_id)=(r.Stage_Name)";
            sqlcmd = sqlcmd + " and (college_code is null or college_code='' or college_code not like'%" + collegecode + "%') and s.stage_name='" + Convert.ToString(ddl_boarding.SelectedItem.Value) + "' and sess='M')";
            ds = d2.select_method_wo_parameter(sqlcmd, "Text");
            Dictionary<int, double> routee = new Dictionary<int, double>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                //if (count > 1)
                //{
                for (int y = 0; y < ds.Tables[0].Rows.Count; y++)
                {
                    routeid = Convert.ToString(ds.Tables[0].Rows[y]["Route_ID"]);
                    Dep_Time = d2.GetFunction("select Arr_Time  from routemaster where Route_ID='" + routeid + "' and sess='M' and (Dep_Time like 'Hal%')");
                    Arr_Time = d2.GetFunction("select Dep_Time  from routemaster where Route_ID='" + routeid + "' and sess='M' and (Arr_Time like 'Ha%')");
                    duration = Convert.ToDouble(Dep_Time) - Convert.ToDouble(Arr_Time);
                    Stage_id = Convert.ToString(ds.Tables[0].Rows[y]["Stage_id"]);
                    Veh_ID = Convert.ToString(ds.Tables[0].Rows[y]["Veh_ID"]);
                    if (addrouteid == "")
                    {
                        addrouteid = Convert.ToString(duration);
                        routid_dur = routeid;
                        veh_ids = Veh_ID;
                    }
                    else
                    {
                        if (Convert.ToDouble(addrouteid) > duration)
                        {
                            addrouteid = Convert.ToString(duration);
                            routid_dur = routeid;
                        }
                    }
                }
                string querystu;
                querystu = "update registration set Bus_RouteID='" + routid_dur + "',Boarding='" + Stage_id + "',VehID='" + veh_ids + "',Trans_PayType='" + type + "',Traveller_Date = '" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appnumber + "'";
                int u = d2.update_method_wo_parameter(querystu, "text");
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, ddl_collegename.SelectedItem.Value, "Commom_Selection_Process");
        }
    }
    protected void licetapplicationprint(string app_no)//barath 18.05.17
    {
        #region query
        //string query = "   select dt.Dept_Name,ci.address1,ci.address2,ci.address3,ci.pincode,ci.affliatedby,ci.district,ci.state,ci.pincode,a.mode,a.LastTCNo,a.college_code,ci.collname,ci.phoneno,ci.faxno,ci.email,ci.website,c.course_name,c.edu_level,type, a.app_no, case when isnull(TamilOrginFromAndaman,0)='0' then 'No' when isnull(TamilOrginFromAndaman,0)='1' then 'Yes' end TamilOrginFromAndaman, case when isnull(IsExService,0)='0' then 'No' when isnull(IsExService,0)='1' then 'Yes' end  IsExService,handy,visualhandy,case when isnull(first_graduate,0)='0' then 'No' when isnull(first_graduate,0)='1' then 'Yes' end  first_graduate, case when isnull(CampusReq,0)='0' then 'No' when isnull(CampusReq,0)='1' then 'Yes' end CampusReq,DistinctSport,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(co_curricular,0))) = convert(varchar,(T.TextCode))) co_curricular,parent_phnoc,parent_nametamil,emailM,parentM_Mobile,alter_mobileno,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(motherocc,0))) = convert(varchar,T.TextCode)) motherocc,emailg,guardian_mobile,guardian_name,emailp,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(mIncome,0))) = convert(varchar,( T.TextCode))) mIncome,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(parent_income,0))) = convert(varchar,(T.TextCode))) parent_income,mother,parentF_Mobile,place_birth,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(partI_Language,0))) = convert(varchar,(T.TextCode))) partI_Language, (Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(bldgrp,0))) = convert(varchar,(T.TextCode)) and TextCriteria='bgrou') bldgrp,Aadharcard_no, Aadhaar_Enroll_No,ElectionID_No, PanCard_No,Fax_No_Per,(select dt.Dept_Name from  degree d,Department dt,Course C where  d.Degree_Code =a.Alternativedegree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id)Alternativedegree_code,app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name, case when sex='0' then 'Male' when sex='1' then 'Female' when sex='2' then 'Transgender' end sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,(Select TextVal FROM TextValTable T WHERE convert(varchar,(parent_occu)) = convert(varchar,(isnull(T.TextCode,0)))) parent_occu, (Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(mother_tongue,0))) = convert(varchar,(T.TextCode)))mother_tongue,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(religion,0))) = convert(varchar,(T.TextCode))) religion,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(citizen,0))) = convert(varchar,(T.TextCode))) citizen, (Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(community,0))) = convert(varchar,(T.TextCode))) community,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(caste,0))) = convert(varchar,(T.TextCode)) and TextCriteria='caste' ) caste,parent_addressC,Streetc,Cityc,TalukC,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(parent_statec,0))) = convert(varchar,(T.TextCode))) parent_statec,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(Countryc,0))) = convert(varchar,(T.TextCode)))Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc, alter_mobileno,parent_addressP,Streetp,cityp,TalukP, (Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(parent_statep,0))) = convert(varchar,(T.TextCode)))parent_statep,(Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(Countryp,0))) = convert(varchar,(T.TextCode)))   Countryp,parent_pincodep,parent_phnop,a.degree_code,batch_year,a.college_code, SubCaste, case when isnull(isdisable,0)='0' then 'No' when isnull(isdisable,0)='1' then 'Yes' end isdisable ,isdisabledisc, islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet ,C.Course_Name,c.Course_Id ,Dt.Dept_Name,CONVERT(varchar(10), fatherdob,103) as  fatherdob, CONVERT(varchar(10), motherdob,103) as  motherdob, ExsRank,ExsNumber,ReserveCategory,EconBackword,(Select TextVal FROM TextValTable T WHERE isnull(SecondLang,0) = T.TextCode) SecondLang , (Select TextVal FROM TextValTable T WHERE convert(varchar,(isnull(ThirdLang,0))) = convert(varchar,(T.TextCode)))   ThirdLang,ExSPlace,tutionfee_waiver,isdonar,mar_status,mQualification,fqualification,idmark,Insurance_Amount,Insurance_Nominee,Ins_Nomin_Age,parentoldstud,Driving_details,IsDrivingLic,Convert(varchar(10),DrivLic_Issue_Date,103) as DrivLic_Issue_Date, IsInsurance,Insurance_InsBy,Insurance_NominRelation,a.mode, a.spouse_name,InternalPercentage, ExternalPercentage, Cut_Of_Mark,PCM_Percentage,Xmedium,totalmark,securedmark,PassYear,PassMonth,course_entno,(Select TextVal FROM TextValTable T WHERE s.course_code = T.TextCode) course_code,(Select TextVal FROM TextValTable T WHERE university_code = T.TextCode)university_code,Vocational_stream,Institute_name, percentage,instaddress,Sch_Clg_Type,(Select TextVal FROM TextValTable T WHERE medium = T.TextCode) medium,(Select TextVal FROM TextValTable T WHERE isnull(s.branch_code,0) = T.TextCode )branch_code ,(Select TextVal FROM TextValTable T WHERE isnull(s.Part1Language,0) = T.TextCode )Part1Language,(Select TextVal FROM TextValTable T WHERE isnull(s.Part2Language,0) = T.TextCode )Part2Language,isgrade,uni_state,registration_no,s.PCM_Percentage, type_semester,majorallied_percent,major_percent,type_major,tancet_mark,tancetmark_year,isnull(markPriority,1) as markPriority,s.insstate_code,s.branch_code ,s.course_entno,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.subcaste) and TextCriteria='scast') SubCaste,case when Dalits='1' then 'Yes' when Dalits='0' then 'No' end Dalits,Parish_name,(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.seattype) and TextCriteria='seat')seattypeval from applyn a right join  Stud_prev_details s on a.app_no=s.app_no ,degree d,Department dt,Course C,collinfo ci  where d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and ci.college_code=c.college_code and isnull(IsConfirm,0)='1'  and a.app_no='" + app_no + "'";
        //det_ds = d2.select_method_wo_parameter(query, "text");
        #endregion
        DataSet det_ds = new DataSet();
        Hashtable studentdetails = new Hashtable();
        studentdetails.Add("@app_no", app_no);
        det_ds = d2.select_method("studentalldetails", studentdetails, "sp");
        if (det_ds.Tables.Count > 0 && det_ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in det_ds.Tables[0].Rows)
            {
                applicationno_span1.InnerHtml = ":" + Convert.ToString(dr["app_formno"]);
                string Educationlevel = Convert.ToString(dr["edu_level"]);
                ViewState["Educationlevel"] = Convert.ToString(dr["edu_level"]);
                string course_entno = Convert.ToString(dr["course_entno"]);
                string mode = Convert.ToString(dr["mode"]);
                stud_printimg.ImageUrl = "~/Handler/Handler3.ashx?id=" + app_no;
                clglogoleft.ImageUrl = "~/Handler/leftlogo.ashx?id=" + Convert.ToString(dr["college_code"]);
                lblCurSemDet.Text = Convert.ToString(dr["current_semester"]);
                lblBatchDet.Text = Convert.ToString(dr["batch_year"]);
                txt_OldBatch.Text = Convert.ToString(dr["batch_year"]);
                Session["OldDegCode"] = Convert.ToString(dr["degree_code"]);
                Session["OldSeatType"] = Convert.ToString(dr["seattype"]);

                txt_OldDegree.Text = Convert.ToString(dr["course_name"]) + '-' + Convert.ToString(dr["Dept_Name"]);
                txt_OldSeattype.Text = Convert.ToString(dr["seattypeval"]);
                txt_OldApplNo.Text = Convert.ToString(dr["app_formno"]);

                #region collegeDetails
                collegename_span1.InnerHtml = Convert.ToString(dr["collname"]);
                collegename_span2.InnerHtml = Convert.ToString(dr["address1"]) + "," + Convert.ToString(dr["address2"]) + "," + Convert.ToString(dr["address3"]);
                collegename_span3.InnerHtml = Convert.ToString(dr["district"]) + "," + Convert.ToString(dr["state"]) + "," + Convert.ToString(dr["pincode"]);
                clgphfax_span4.InnerHtml = " Phone No: " + Convert.ToString(dr["phoneno"]) + ", Fax No: " + Convert.ToString(dr["faxno"]);
                clgemail_span5.InnerHtml = " Email ID:" + Convert.ToString(dr["email"]);
                clsgwebsite_span6.InnerHtml = " Website :" + Convert.ToString(dr["website"]);
                #endregion

                #region Course Details
                Institution_Name_span1.InnerHtml = ":" + Convert.ToString(dr["collname"]);
                Graduation_span1.InnerHtml = ":" + Convert.ToString(dr["edu_level"]);
                Degree_span1.InnerHtml = ":" + Convert.ToString(dr["course_name"]);
                CourseI.InnerHtml = ":" + Convert.ToString(dr["Dept_Name"]);
                choiseII.InnerHtml = ":" + Convert.ToString(dr["Alternativedegree_code"]);
                Seattypev.InnerHtml = ":" + Convert.ToString(dr["seattypeval"]);//added
                Datev.InnerHtml = ":" + Convert.ToString(dr["date_applied"]);
                string dateadmit = d2.GetFunction("select convert(nvarchar(15),Adm_Date,103) from registration where app_no='" + app_no + "'");
                if (dateadmit != "" && dateadmit != "0")
                {
                    AdmDateV.InnerHtml = ":" + Convert.ToString(dateadmit);
                }
                else
                {
                    AdmDateV.InnerHtml = ":";
                }
                #endregion

                #region Acadamic Details

                if (Educationlevel.ToUpper() == "UG")
                {
                    if (mode == "1")
                    {
                        qualifyingexam_span1.InnerHtml = ":" + Convert.ToString(dr["course_code"]);
                        Nameofschool_span1.InnerHtml = ":" + Convert.ToString(dr["Institute_name"]);
                        languagestudidespan.InnerHtml = ":" + Convert.ToString(dr["medium"]);
                        qualifyingboard_span.InnerHtml = ":" + Convert.ToString(dr["university_code"]);
                        locationofschool_Span1.InnerHtml = ":" + Convert.ToString(dr["instaddress"]);

                        ug_totalmark_span1.InnerHtml = ":" + Convert.ToString(dr["securedmark"]);
                        Pcmmarkvalue_span.InnerHtml = ":" + Convert.ToString(dr["PCM_Percentage"]);
                        cutoffmark_span1.InnerHtml = ":" + Convert.ToString(dr["Cut_Of_Mark"]) == "" ? " - " : ": " + Convert.ToString(dr["Cut_Of_Mark"]);
                        ugtotaltable.Visible = true; pgtotaltable.Visible = false;
                    }
                    else
                    {
                        qualifyingexam_span2.InnerHtml = ":" + Convert.ToString(dr["course_code"]);
                        Nameofschool_span2.InnerHtml = ":" + Convert.ToString(dr["Institute_name"]);
                        collegelocation_span2.InnerHtml = ":" + Convert.ToString(dr["instaddress"]);
                        majorspan2.InnerHtml = ":" + Convert.ToString(dr["Alternativedegree_code"]);
                        typeofsemester.InnerHtml = ":" + Convert.ToString(dr["type_semester"]);
                        mediumofstudyug.InnerHtml = ":" + Convert.ToString(dr["medium"]);
                        markorgrade_span.InnerHtml = ":" + Convert.ToString(dr["isgrade"]);
                        reg_no_span2.InnerHtml = ":" + Convert.ToString(dr["registration_no"]);

                        percentagemajorspan.InnerHtml = ":  " + Convert.ToString(dr["percentage"]);
                        majorsubjectspan.InnerHtml = ":  " + Convert.ToString(dr["major_percent"]);
                        alliedmajorspan.InnerHtml = ":  " + Convert.ToString(dr["majorallied_percent"]);
                        pgtotaltable.Visible = true; ugtotaltable.Visible = false;
                    }
                    bindmark(course_entno, mode);
                }
                #endregion

                #region Personal details
                Applicantname_span1.InnerHtml = ":" + Convert.ToString(dr["stud_name"]);
                dob_span1.InnerHtml = ":" + Convert.ToString(dr["dob"]);
                placeofbirthspan.InnerHtml = ":" + Convert.ToString(dr["place_birth"]);
                sex_span1.InnerHtml = ":" + Convert.ToString(dr["sex"]);
                mothertongue_span1.InnerHtml = ":" + Convert.ToString(dr["mother_tongue"]);
                string Religion = Convert.ToString(dr["religion"]);
                string subcaste = Convert.ToString(dr["SubCaste"]);
                string Dalits = Convert.ToString(dr["Dalits"]);
                string Parish_name = Convert.ToString(dr["Parish_name"]);
                string subcasteval = "";
                if (subcaste.ToUpper() == "ROMAN CATHOLIC")
                    subcasteval = " Dalits :" + Dalits + " Parish Name: " + Parish_name;
                if (subcasteval.Trim() != "")
                    Religion = Religion + " " + subcasteval;
                Religion_span1.InnerHtml = ":" + Convert.ToString(Religion);
                Nationality_span1.InnerHtml = ":" + Convert.ToString(dr["citizen"]);
                Community_span1.InnerHtml = ":" + Convert.ToString(dr["community"]);
                Caste_span1.InnerHtml = ":" + Convert.ToString(dr["caste"]);
                bloodgroupspan.InnerHtml = ":" + Convert.ToString(dr["bldgrp"]);
                Aadharspan.InnerHtml = ":" + Convert.ToString(dr["Aadharcard_no"]);
                ishostelreq_span.InnerHtml = ":" + Convert.ToString(dr["CampusReq"]);
                extracurricular_span.InnerHtml = ":" + Convert.ToString(dr["co_curricular"]);
                //father details
                fathername_span1.InnerHtml = ":" + Convert.ToString(dr["parent_name"]);
                foccup.InnerHtml = ":" + Convert.ToString(dr["parent_occu"]);
                fannualincomespan.InnerHtml = ":" + Convert.ToString(dr["parent_income"]);
                fathercontactnospan.InnerHtml = ":" + Convert.ToString(dr["parentF_Mobile"]);
                fatheremailidspan.InnerHtml = ":" + Convert.ToString(dr["emailp"]);
                //mother details
                mothernamespan.InnerHtml = ":" + Convert.ToString(dr["mother"]);
                motheroccupationspan.InnerHtml = ":" + Convert.ToString(dr["motherocc"]);
                motherannualincomespan.InnerHtml = ":" + Convert.ToString(dr["mIncome"]);
                mothercontactnospan.InnerHtml = ":" + Convert.ToString(dr["parentM_Mobile"]);
                motheremailspan.InnerHtml = ":" + Convert.ToString(dr["emailM"]);
                //guardian details
                guardiannamepspan.InnerHtml = ":" + Convert.ToString(dr["guardian_name"]);
                guardiancontactnospan.InnerHtml = ":" + Convert.ToString(dr["guardian_mobile"]);
                guardinaemailspan.InnerHtml = ":" + Convert.ToString(dr["emailg"]);
                //communication address

                caddress1_span1.InnerHtml = ":" + Convert.ToString(dr["parent_addressC"]);
                caddress2_span1.InnerHtml = ":" + Convert.ToString(dr["Streetc"]);
                //caddress3_span1.InnerHtml = ":" + Convert.ToString(dr["Alternativedegree_code"]);
                ccity_span1.InnerHtml = ":" + Convert.ToString(dr["cityc"]);
                cState_span1.InnerHtml = ":" + Convert.ToString(dr["parent_statec"]);
                Country_span1.InnerHtml = ":" + Convert.ToString(dr["Countryc"]);
                Postelcode_Span1.InnerHtml = ":" + Convert.ToString(dr["parent_pincodec"]);
                Mobilenumber_Span1.InnerHtml = ":" + Convert.ToString(dr["Student_Mobile"]);
                Alternatephone_span1.InnerHtml = ":" + Convert.ToString(dr["alter_mobileno"]);
                emailid_span1.InnerHtml = ":" + Convert.ToString(dr["StuPer_Id"]);
                std_ist_span1.InnerHtml = ":" + Convert.ToString(dr["parent_phnoc"]);//commented
                //Permanent Address
                paddressline1_span1.InnerHtml = ":" + Convert.ToString(dr["parent_addressP"]);
                paddressline2_span1.InnerHtml = ":" + Convert.ToString(dr["Streetp"]);
                //paddressline3_span1.InnerHtml = ":" + Convert.ToString(dr["Alternativedegree_code"]);
                pcity_span1.InnerHtml = ":" + Convert.ToString(dr["cityp"]);
                pstate_span1.InnerHtml = ":" + Convert.ToString(dr["parent_statep"]);
                pcountry_span1.InnerHtml = ":" + Convert.ToString(dr["Countryp"]);
                ppostelcode_span1.InnerHtml = ":" + Convert.ToString(dr["parent_pincodep"]);
                pstdisd_span1.InnerHtml = ":" + Convert.ToString(dr["parent_phnop"]);
                studsignature.InnerHtml = ":";
                string referby = d2.GetFunction("select direct_refer from applyn where app_no='" + app_no + "'");
                if (referby == "0")
                {
                    Refer.InnerHtml = ": Direct";

                }
                else if (referby == "1")
                {
                    string staffcode = d2.GetFunction("select refer_stcode from applyn where app_no='" + app_no + "'");
                    string appl_no = d2.GetFunction("select appl_no  from staffmaster where staff_code='" + staffcode + "'");
                    string department = d2.GetFunction("select dept_name from staff_appl_master where appl_no='" + appl_no + "'");
                    string staffname = d2.GetFunction("select staff_name  from staffmaster where staff_code='" + staffcode + "'");
                    string college = d2.GetFunction("select college_code from staff_appl_master where appl_no='" + appl_no + "'");
                    string contact = d2.GetFunction("select per_phone from staff_appl_master where appl_no='" + appl_no + "'");
                    Refer.InnerHtml = ":Staff";
                    Refernamet.InnerHtml = "Staff Name";
                    Refernamev.InnerHtml = ":" + Convert.ToString(staffname);
                    Refercodet.InnerHtml = "Staff Code";
                    Refercodev.InnerHtml = ":" + Convert.ToString(staffcode);

                    departt.InnerHtml = "Department";
                    departv.InnerHtml = ":" + Convert.ToString(department);
                    colleget.InnerHtml = "College";

                    collegev.InnerHtml = ":" + d2.GetFunction("select collname from collinfo where college_code='" + college + "'");
                    contactt.InnerHtml = "Contact No";
                    Contactv.InnerHtml = ":" + Convert.ToString(contact);

                    Signaturet.InnerHtml = "Signature Of Staff";
                    Signaturev.InnerHtml = ":";

                }
                else if (referby == "2")
                {
                    Refer.InnerHtml = ": Student";
                    string rollno = d2.GetFunction("select roll_no from registration where app_no='" + app_no + "'");

                    string department = d2.GetFunction("select degree_code from applyn where app_no='" + app_no + "'");
                    string studname = d2.GetFunction("select stud_name  from applyn where app_no='" + app_no + "'");
                    string college = d2.GetFunction("select college_code from applyn where app_no='" + app_no + "'");
                    string contact = d2.GetFunction("select Student_Mobile from applyn where app_no='" + app_no + "'");
                    Refer.InnerHtml = ":Staff";
                    Refernamet.InnerHtml = "Staff Name";
                    Refernamev.InnerHtml = ":" + Convert.ToString(studname);
                    Refercodet.InnerHtml = "Staff Code";
                    Refercodev.InnerHtml = ":" + Convert.ToString(rollno);

                    departt.InnerHtml = "Department";
                    departv.InnerHtml = ":" + d2.GetFunction("select dept_name from department where dept_code='" + department + "'");
                    colleget.InnerHtml = "College";

                    collegev.InnerHtml = ":" + d2.GetFunction("select collname from collinfo where college_code='" + college + "'");
                    contactt.InnerHtml = "Contact No";
                    Contactv.InnerHtml = ":" + Convert.ToString(contact);

                    Signaturet.InnerHtml = "Signature Of Student";
                    Signaturev.InnerHtml = ":";

                }
                else if (referby == "3")
                {
                    Refer.InnerHtml = ": Consultant";

                    string referno = d2.GetFunction("select refer_name from applyn where app_no='" + app_no + "'");

                    string department = d2.GetFunction("select refer_name from Student_Refer_Details where idno='" + referno + "'");
                    string studname = d2.GetFunction("select refer_agent_name  from Student_Refer_Details where idno='" + referno + "'");
                    string contact = d2.GetFunction("select refer_phoneno from Student_Refer_Details where idno='" + referno + "'");

                    //  Refer.InnerHtml = ":ConsultancyName";
                    Refernamet.InnerHtml = "Consultancy Name";
                    Refernamev.InnerHtml = ":" + Convert.ToString(department);
                    Refercodet.InnerHtml = "Agent Name";
                    Refercodev.InnerHtml = ":" + Convert.ToString(studname);


                    contactt.InnerHtml = "Contact No";
                    Contactv.InnerHtml = ":" + Convert.ToString(contact);

                    Signaturet.InnerHtml = "Signature Of Consultant";
                    Signaturev.InnerHtml = ":";
                }
                #endregion
            }
        }
    }
    protected void bindmark(string course_entno, string Mode)
    {
        DataSet mark_det = new DataSet();
        string markquery = " select (select textval from textvaltable where convert(varchar,psubjectno)=convert(varchar,textcode))Subject,(select textval from textvaltable where convert(varchar,subject_typeno)=convert(varchar,textcode))subject_typeno,registerno as [Register No],acual_marks as Marks,max_marks as [Maximum Marks],pass_month as Month,pass_year as Year,noofattempt as [No.of Attempts] from perv_marks_history where course_entno='" + course_entno + "' order by indMrkNo";
        mark_det = d2.select_method_wo_parameter(markquery, "text");
        if (ViewState["Educationlevel"].ToString().ToUpper() == "UG")
        {
            if (Mode == "1")
            {
                regular_div.Visible = true;
                lateral_div.Visible = false;
                VerificationGridug1.DataSource = mark_det.Tables[0];
                VerificationGridug1.DataBind();
                if (VerificationGridug1.Rows.Count > 0)
                {
                    if (VerificationGridug1.HeaderRow.Cells.Count > 0)
                        VerificationGridug1.HeaderRow.Cells[1].Visible = false;
                    for (int i = 0; i < VerificationGridug1.Rows.Count; i++)
                    {
                        VerificationGridug1.Rows[i].Cells[1].Visible = false;
                    }
                }
            }
            else
            {
                regular_div.Visible = false;
                lateral_div.Visible = true;
                VerificationGridug1.DataSource = mark_det.Tables[0];
                VerificationGridug1.DataBind();
            }
        }

    }
    public void loadHostelRoom()
    {
        try
        {
            ddlHosRoom.Items.Clear();
            if (ddl_roomtype.Items.Count > 0)
            {
                string Q = "select Room_Name+'('+convert(varchar,isnull(students_allowed,0)) +'-'+ convert(varchar,isnull(Avl_Student,0))+')' as Room_Name,roompk from room_detail where  Room_type='" + Convert.ToString(ddl_roomtype.SelectedItem.Value) + "' and ISNULL(students_allowed,0)<>ISNULL(avl_student,0) and Room_type<>'' order by LEN(room_name) asc";
                DataSet dsHost = new DataSet();
                dsHost = d2.select_method_wo_parameter(Q, "Text");
                if (dsHost.Tables.Count > 0 && dsHost.Tables[0].Rows.Count > 0)
                {
                    ddlHosRoom.DataSource = dsHost.Tables[0];
                    ddlHosRoom.DataTextField = "Room_Name";
                    ddlHosRoom.DataValueField = "roompk";
                    ddlHosRoom.DataBind();
                }
            }
        }
        catch { }
        ListItem ls = new ListItem("Select", "-1");
        ddlHosRoom.Items.Insert(0, ls);
    }
    public void bindroomtype()
    {
        ddl_roomtype.Items.Clear();
        if (ddlHosHostel.Items.Count > 0)
        {
            string HostelQuery = "select HostelBuildingFK  from HM_HostelMaster where HostelMasterPK ='" + Convert.ToString(ddlHosHostel.SelectedValue) + "'";
            DataSet dsHostPk = new DataSet();
            dsHostPk = d2.select_method_wo_parameter(HostelQuery, "Text");
            if (dsHostPk.Tables.Count > 0 && dsHostPk.Tables[0].Rows.Count > 0)
            {
                string Q = "select distinct Room_type from Room_Detail r,Building_Master b where r.Building_Name =b.Building_Name and b.Code in (" + Convert.ToString(dsHostPk.Tables[0].Rows[0][0]) + ") and Room_type<>''";
                DataSet dsHost = new DataSet();
                dsHost = d2.select_method_wo_parameter(Q, "Text");
                if (dsHost.Tables.Count > 0 && dsHost.Tables[0].Rows.Count > 0)
                {
                    ddl_roomtype.DataSource = dsHost;
                    ddl_roomtype.DataTextField = "Room_type";
                    ddl_roomtype.DataValueField = "Room_type";
                    ddl_roomtype.DataBind();
                }
            }
            ddl_roomtype.Items.Insert(0, "Select");
        }
    }
    public void loadHostel(string Mem)
    {
        try
        {
            ddlHosHostel.Items.Clear();
            string Q = "select HostelName,HostelMasterPK  from HM_HostelMaster where HostelType in (" + Mem + ",0) ";
            DataSet dsHost = new DataSet();
            dsHost = d2.select_method_wo_parameter(Q, "Text");
            if (dsHost.Tables.Count > 0 && dsHost.Tables[0].Rows.Count > 0)
            {
                ddlHosHostel.DataSource = dsHost;
                ddlHosHostel.DataTextField = "HostelName";
                ddlHosHostel.DataValueField = "HostelMasterPK";
                ddlHosHostel.DataBind();
            }
        }
        catch { }
        ListItem ls = new ListItem("Select", "-1");
        ddlHosHostel.Items.Insert(0, ls);
    }

    //public void cb_comm_checkedchange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_comm.Checked == true)
    //        {
    //            for (int i = 0; i < cbl_comm.Items.Count; i++)
    //            {
    //                cbl_comm.Items[i].Selected = true;
    //            }
    //            txt_comm.Text = "Community(" + (cbl_comm.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cbl_comm.Items.Count; i++)
    //            {
    //                cbl_comm.Items[i].Selected = false;
    //            }
    //            txt_comm.Text = "--Select--";
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    //public void cbl_comm_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int commcount = 0;
    //        txt_comm.Text = "--Select--";
    //        cb_comm.Checked = false;
    //        for (int i = 0; i < cbl_comm.Items.Count; i++)
    //        {
    //            if (cbl_comm.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //            }
    //        }
    //        if (commcount == cbl_comm.Items.Count)
    //        {
    //            txt_comm.Text = "Community(" + commcount.ToString() + ")";
    //            cb_comm.Checked = true;
    //        }
    //        else if (commcount == 0)
    //        {
    //            txt_comm.Text = "--Select--";
    //        }
    //        else
    //        {
    //            txt_comm.Text = "Community(" + commcount.ToString() + ")";
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    public void loadcommunity()
    {
        try
        {
            string comm = "";
            //  string selq = "select TextCode,textval from textvaltable where TextCriteria like '%comm%' and college_code='" + ddlcollege.SelectedItem.Value + "' and textval<>''and textval<>'-' and TextCriteria2='comm1' order by textval ";
            string selq = " select distinct TextCode,textval from textvaltable t,applyn a where t.TextCode=a.community and TextCriteria like '%comm%' and a.college_code='" + ddlColChangeDeg.SelectedItem.Value + "' and textval<>''and textval<>'-'  order by textval ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_community.DataSource = ds;
                    ddl_community.DataTextField = "TextVal";
                    ddl_community.DataValueField = "TextCode";
                    ddl_community.DataBind();
                }
            }
        }



        catch
        {
        }
    }
    private void Bindstage()
    {
        ds.Clear(); ddl_boarding.Items.Clear();
        if (ddl_boarding.SelectedValue != "Select")
        {
            string roomquery = "select Stage_id,Stage_Name from Stage_Master order by Stage_Name ";
            ds = d2.select_method_wo_parameter(roomquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_boarding.DataSource = ds.Tables[0];
                ddl_boarding.DataTextField = "Stage_Name";
                ddl_boarding.DataValueField = "Stage_id";
                ddl_boarding.DataBind();
            }
        }
    }
    protected void ddl_roomtype_selectedindexchanged(object sendder, EventArgs e)
    {
        loadHostelRoom();
        ViewUpdateDiv();
    }
    /// <summary>
    /// Hostel Registeration add by Barath 29.06.17
    /// </summary>
    /// <param name="HostelFk"></param>
    /// <param name="RoomFk"></param>
    /// <param name="Collegecode"></param>
    /// <param name="AppNo"></param>
    /// <param name="buildingFk"></param>
    /// <param name="FloorFk"></param>
    /// <param name="usercode"></param>
    /// <param name="admit_date"></param>
    /// <param name="FinyearFk"></param>
    /// <param name="Feeallot"></param>
    protected void HostelRegistration(string HostelFk, string RoomFk, string Collegecode, string AppNo, string buildingFk = null, string FloorFk = null, string usercode = null, string admit_date = null, string FinyearFk = null, bool Feeallot = false)
    {
        #region Hostel
        if (HostelFk != "0")
        {
            if (RoomFk != "0")
            {
                double Studentallowed = 0; double Studentavilable = 0;
                string q1 = " select b.Building_Name,b.Code,f.Floor_Name,f.Floorpk,rd.Room_Name,rd.RoomPK,rd.Room_type,isnull(rd.students_allowed,0)students_allowed,ISNULL(rd.Avl_Student,0)Avl_Student from Room_Detail rd,Floor_Master f,Building_Master b where b.Building_Name=f.Building_Name and b.Building_Name=rd.Building_Name and rd.Building_Name=f.Building_Name and rd.Floor_Name=f.Floor_Name and rd.RoomPK='" + RoomFk + "' ";
                q1 += " select convert(varchar, hosteladmfeeheaderfk)+'$'+CONVERT(varchar, hosteladmfeeledgerfk)headerandledger from HM_HostelMaster where hostelmasterpk='" + HostelFk + "' ";
                q1 += " select convert(varchar(18), Room_Cost)+'$'+CONVERT(varchar(10), Rent_Type)Roomcostandrenttype from RoomCost_Master where college_code='" + Collegecode + "' and Room_Type='" + Convert.ToString(ddl_roomtype.SelectedItem.Text) + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(buildingFk))
                            buildingFk = Convert.ToString(ds.Tables[0].Rows[0]["Code"]);
                        if (string.IsNullOrEmpty(FloorFk))
                            FloorFk = Convert.ToString(ds.Tables[0].Rows[0]["Floorpk"]);
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["students_allowed"]), out Studentallowed);
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["Avl_Student"]), out Studentavilable);
                    }
                    if (FloorFk.Trim() != "" && RoomFk.Trim() != "" && buildingFk != "")
                    {
                        #region Insert
                        if (string.IsNullOrEmpty(admit_date))
                            admit_date = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                        int h = 0;
                        if (Studentallowed >= Studentavilable && Studentallowed != Studentavilable)
                        {
                            string hostelquery = " if not exists(select app_no from HT_HostelRegistration where app_no='" + AppNo + "')  update Room_Detail set Avl_Student= isnull(Avl_Student,0) + 1 where RoomPK='" + RoomFk + "'";
                            hostelquery += " if not exists(select app_no from HT_HostelRegistration where app_no='" + AppNo + "') insert into HT_HostelRegistration(MemType,APP_No,HostelAdmDate,BuildingFK, FloorFK,RoomFK,StudMessType,IsDiscontinued, DiscontinueDate, HostelMasterFK,collegecode)values(1,'" + AppNo + "','" + admit_date + "','" + buildingFk + "','" + FloorFk + "','" + RoomFk + "','0','0','','" + HostelFk + "','" + Collegecode + "')";
                            hostelquery += " update Registration set Stud_Type='Hostler' where App_No='" + AppNo + "'";
                            h = d2.update_method_wo_parameter(hostelquery, "Text");
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Room Filled Please Select Another Room Name", true);

                        #endregion
                        if (h != 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Saved Successfully", true);
                            loadHostelRoom();
                            ddlHosRoom.SelectedIndex = ddlHosRoom.Items.IndexOf(ddlHosRoom.Items.FindByValue(RoomFk));
                        }
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Room Details Missing", true);
                    if (Feeallot == true)
                    {
                        #region Hostel Feeallot
                        string Hostelfee = d2.GetFunction("select value from Master_Settings where settings ='HostelFeeAllot' and usercode ='" + usercode + "'");
                        if (Hostelfee == "1")
                        {
                            string Hostelheader = "";
                            string Hostelledger = "";
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                string[] headerandledger = Convert.ToString(ds.Tables[1].Rows[0]["headerandledger"]).Split('$');
                                if (headerandledger.Length == 2)
                                {
                                    Hostelheader = Convert.ToString(headerandledger[0]);
                                    Hostelledger = Convert.ToString(headerandledger[1]);
                                }
                            }
                            string roomcost = ""; string renttype = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                string[] Roomcostandrenttype = Convert.ToString(ds.Tables[2].Rows[0]
    ["Roomcostandrenttype"]).Split('$');
                                if (Roomcostandrenttype.Length == 2)
                                {
                                    roomcost = Convert.ToString(Roomcostandrenttype[0]);
                                    renttype = Convert.ToString(Roomcostandrenttype[1]);
                                }
                            }
                            string val = "";
                            if (renttype == "2")
                                val = "1 Year";
                            else
                                val = "1 Semester";
                            string catagory = d2.GetFunction("select TextCode from textvaltable where TextCriteria='FEECA' and textval='" + val + "' and college_code='" + Collegecode + "'");
                            if (FinyearFk == null)
                                FinyearFk = d2.getCurrentFinanceYear(usercode, Collegecode);
                            if (FinyearFk.Trim() == "" || FinyearFk.Trim() == "0")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Set Financial year settings", true);
                                return;
                            }
                            if (catagory != "" && catagory != "0")
                            {
                                if (Hostelheader != "0" && Hostelledger != "0" && roomcost != "0" && roomcost != "")
                                {
                                    string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + Hostelledger + "') and HeaderFK in('" + Hostelheader + "') and FeeCategory in('" + catagory + "')  and App_No in('" + AppNo + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + roomcost + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + roomcost + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + roomcost + "' where LedgerFK in('" + Hostelledger + "') and HeaderFK in('" + Hostelheader + "') and FeeCategory in('" + catagory + "') and App_No in('" + AppNo + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount, DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + AppNo + ",'" + Hostelledger + "','" + Hostelheader + "','" + roomcost + "','0','0','0','" + roomcost + "','0','0','','0','" + catagory + "','','0','','0','0','" + roomcost + "','" + FinyearFk + "')";
                                    int a = d2.update_method_wo_parameter(insupdquery, "text");
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Kindly Allot The Fees Or Hostel Header and Ledger", true);
                                    return;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Kindly Set Fee catagory", true);
                                return;
                            }
                        }
                        #endregion
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Please Run I Patch", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Please select one room name", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "Please select Hostel Name", true);

        #endregion
    }

    #region New columnOrder Added by saranya on 10/7/2018

    public void lnkcolorder_Click(object sender, EventArgs e)
    {
        poppernew.Visible = true;
        ddl_coltypeadd.SelectedIndex = 0;
        load();
        lb_column1.Items.Clear();
    }

    public void load()
    {
        lb_selectcolumn.Items.Clear();
        lb_selectcolumn.Items.Add(new ListItem("Student Name", "54"));
        lb_selectcolumn.Items.Add(new ListItem("Roll No", "55"));
        lb_selectcolumn.Items.Add(new ListItem("Reg No", "57"));
        lb_selectcolumn.Items.Add(new ListItem("Admission No", "58"));
        lb_selectcolumn.Items.Add(new ListItem("Application No", "59"));
        lb_selectcolumn.Items.Add(new ListItem("Applied Date", "81"));
        lb_selectcolumn.Items.Add(new ListItem("Batch", "3"));
        lb_selectcolumn.Items.Add(new ListItem(lbldeg.Text, "1"));
        lb_selectcolumn.Items.Add(new ListItem(lbldept.Text, "2"));
        lb_selectcolumn.Items.Add(new ListItem("Semester", "4"));
        lb_selectcolumn.Items.Add(new ListItem("Section", "60"));
        lb_selectcolumn.Items.Add(new ListItem("SeatType", "16"));
        lb_selectcolumn.Items.Add(new ListItem("Student Type", "63"));
        lb_selectcolumn.Items.Add(new ListItem("HostelName", "34"));
        //30.07.16
        lb_selectcolumn.Items.Add(new ListItem("Mode", "43"));
        lb_selectcolumn.Items.Add(new ListItem("Boarding", "122"));
        lb_selectcolumn.Items.Add(new ListItem("Vehicle Id", "123"));
        lb_selectcolumn.Items.Add(new ListItem("Gender", "61"));
        lb_selectcolumn.Items.Add(new ListItem("DOB", "6"));
        lb_selectcolumn.Items.Add(new ListItem("Blood Group", "62"));
        lb_selectcolumn.Items.Add(new ListItem("Father Name", "5"));
        lb_selectcolumn.Items.Add(new ListItem("Father Income", "84"));
        lb_selectcolumn.Items.Add(new ListItem("Father Occupation", "7"));
        lb_selectcolumn.Items.Add(new ListItem("Father Mob No", "85"));
        lb_selectcolumn.Items.Add(new ListItem("Father Email Id", "86"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Name", "87"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Income", "88"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Occupation", "96"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Mob No", "89"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Email Id", "90"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Name", "91"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Email Id", "92"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Mob No", "93"));
        lb_selectcolumn.Items.Add(new ListItem("Place Of Birth", "94"));
        lb_selectcolumn.Items.Add(new ListItem("Adhaar Card No", "95"));
        lb_selectcolumn.Items.Add(new ListItem("Voter ID", "35"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Tongue", "8"));
        lb_selectcolumn.Items.Add(new ListItem("Religion", "9"));
        lb_selectcolumn.Items.Add(new ListItem("Community", "11"));
        lb_selectcolumn.Items.Add(new ListItem("Caste", "12"));
        lb_selectcolumn.Items.Add(new ListItem("Sub Caste", "83"));
        lb_selectcolumn.Items.Add(new ListItem("Citizen", "10"));
        lb_selectcolumn.Items.Add(new ListItem("TamilOrginFromAndaman", "13"));
        lb_selectcolumn.Items.Add(new ListItem("Ex-serviceman", "64"));
        lb_selectcolumn.Items.Add(new ListItem("Rank", "74"));
        lb_selectcolumn.Items.Add(new ListItem("Place", "75"));
        lb_selectcolumn.Items.Add(new ListItem("Number", "76"));
        lb_selectcolumn.Items.Add(new ListItem("IsDisable", "53"));
        lb_selectcolumn.Items.Add(new ListItem("VisualHandy", "14"));
        lb_selectcolumn.Items.Add(new ListItem("Residency", "48"));
        lb_selectcolumn.Items.Add(new ListItem("Physically challange", "49"));
        lb_selectcolumn.Items.Add(new ListItem("Learning Disability", "51"));
        lb_selectcolumn.Items.Add(new ListItem("Other Disability", "52"));
        lb_selectcolumn.Items.Add(new ListItem("Sports", "50"));
        lb_selectcolumn.Items.Add(new ListItem("First Graduate", "15"));
        lb_selectcolumn.Items.Add(new ListItem("MissionaryChild", "26"));
        lb_selectcolumn.Items.Add(new ListItem("missionarydisc", "27"));
        lb_selectcolumn.Items.Add(new ListItem("Hostel accommodation", "65"));
        lb_selectcolumn.Items.Add(new ListItem("Blood Donor", "66"));
        lb_selectcolumn.Items.Add(new ListItem("Reserved Caste", "67"));
        lb_selectcolumn.Items.Add(new ListItem("Economic Backward", "68"));
        lb_selectcolumn.Items.Add(new ListItem("Parents Old Student", "69"));
        lb_selectcolumn.Items.Add(new ListItem("Driving License", "70"));
        lb_selectcolumn.Items.Add(new ListItem("License No", "71"));
        lb_selectcolumn.Items.Add(new ListItem("Tuition Fee Waiver", "72"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance", "73"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance Amount", "77"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance InsBy", "78"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance Nominee", "79"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance NominRelation", "80"));
        lb_selectcolumn.Items.Add(new ListItem("Address", "18"));
        lb_selectcolumn.Items.Add(new ListItem("Street", "19"));
        lb_selectcolumn.Items.Add(new ListItem("City", "20"));
        lb_selectcolumn.Items.Add(new ListItem("State", "21"));
        lb_selectcolumn.Items.Add(new ListItem("Country", "22"));
        lb_selectcolumn.Items.Add(new ListItem("PinCode", "24"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Address", "108"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Street", "109"));
        lb_selectcolumn.Items.Add(new ListItem("Communication City", "110"));
        lb_selectcolumn.Items.Add(new ListItem("Communication State", "111"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Country", "112"));
        lb_selectcolumn.Items.Add(new ListItem("Communication PinCode", "113"));
        lb_selectcolumn.Items.Add(new ListItem("Student Mobile", "23"));
        lb_selectcolumn.Items.Add(new ListItem("Alternate Mob No", "82"));
        lb_selectcolumn.Items.Add(new ListItem("Student EmailId", "56"));
        lb_selectcolumn.Items.Add(new ListItem("Parent Phone No", "25"));
        lb_selectcolumn.Items.Add(new ListItem("Curricular", "17"));
        lb_selectcolumn.Items.Add(new ListItem("Institute Name", "28"));
        lb_selectcolumn.Items.Add(new ListItem("Institute Address", "97"));
        lb_selectcolumn.Items.Add(new ListItem("X Medium", "98"));
        lb_selectcolumn.Items.Add(new ListItem("X11 Medium", "99"));
        lb_selectcolumn.Items.Add(new ListItem("Part1 Language", "29"));
        lb_selectcolumn.Items.Add(new ListItem("Part2 Language", "30"));
        lb_selectcolumn.Items.Add(new ListItem("Percentage", "100"));
        lb_selectcolumn.Items.Add(new ListItem("Secured Mark", "101"));
        lb_selectcolumn.Items.Add(new ListItem("Total Mark", "102"));
        lb_selectcolumn.Items.Add(new ListItem("Pass Month", "103"));
        lb_selectcolumn.Items.Add(new ListItem("Pass Year", "104"));
        lb_selectcolumn.Items.Add(new ListItem("Vocational Stream", "105"));
        lb_selectcolumn.Items.Add(new ListItem("Mark Priority", "106"));
        lb_selectcolumn.Items.Add(new ListItem("Cut Of Mark", "107"));
        lb_selectcolumn.Items.Add(new ListItem("University Name", "31"));
        lb_selectcolumn.Items.Add(new ListItem("State", "40"));
        lb_selectcolumn.Items.Add(new ListItem("Last TC No", "32"));
        lb_selectcolumn.Items.Add(new ListItem("Last TC Date", "33"));//delsii
        //lb_selectcolumn.Items.Add(new ListItem("12th MS", "34"));
        //lb_selectcolumn.Items.Add(new ListItem("Community Certificate No", "35"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Provisional No", "36"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Consolidate", "35"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Degree", "38"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma- No of Semester", "39"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Provisional No", "40"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Consolidate", "41"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Degree", "42"));
        //lb_selectcolumn.Items.Add(new ListItem("UG- No of Semester", "43"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Provisional No", "44"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Consolidate", "45"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Degree", "46"));
        //lb_selectcolumn.Items.Add(new ListItem("PG- No of Semester", "47"));
        lb_selectcolumn.Items.Add(new ListItem("A/C No", "114"));
        lb_selectcolumn.Items.Add(new ListItem("DebitCard No", "115"));
        lb_selectcolumn.Items.Add(new ListItem("IFSCCode", "116"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Name", "117"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Branch", "118"));
        lb_selectcolumn.Items.Add(new ListItem("Relative Name", "119"));
        lb_selectcolumn.Items.Add(new ListItem("RelationShip", "120"));
        lb_selectcolumn.Items.Add(new ListItem("Student/Staff", "121"));
        lb_selectcolumn.Items.Add(new ListItem("Admission Date", "36"));
        lb_selectcolumn.Items.Add(new ListItem("Enrollment Date", "37"));
        lb_selectcolumn.Items.Add(new ListItem("Join Date", "38"));
        lb_selectcolumn.Items.Add(new ListItem("CGPA", "125"));
        lb_selectcolumn.Items.Add(new ListItem("No Of Arrear", "126"));
        lb_selectcolumn.Items.Add(new ListItem("Refered By", "127"));
        lb_selectcolumn.Items.Add(new ListItem("Alternative Course", "128"));

        string query = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + cblclg.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int y = 0; y < ds.Tables[0].Rows.Count; y++)
            {
                lb_selectcolumn.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(ds.Tables[0].Rows[y]["MasterValue"]), Convert.ToString(ds.Tables[0].Rows[y]["MasterCode"])));
            }
        }
    }

    public void loadvalue()
    {
        if (colval == "1")
        {
            loadval = "Course_Name";
        }
        if (colval == "2")
        {
            loadval = "Dept_Name";
        }
        if (colval == "3")
        {
            loadval = "Batch_Year";
        }
        if (colval == "4")
        {
            loadval = "Current_Semester";
        }
        if (colval == "5")
        {
            loadval = "parent_name";
        }
        if (colval == "6")
        {
            loadval = "dob";
        }
        if (colval == "7")
        {
            loadval = "parent_occu";
        }
        if (colval == "8")
        {
            loadval = "mother_tongue";
        }
        if (colval == "9")
        {
            loadval = "religion";
        }
        if (colval == "10")
        {
            loadval = "citizen";
        }
        if (colval == "11")
        {
            loadval = "community";
        }
        if (colval == "12")
        {
            loadval = "caste";
        }
        if (colval == "13")
        {
            loadval = "TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            loadval = "visualhandy";
        }
        if (colval == "15")
        {
            loadval = "first_graduate";
        }
        if (colval == "16")
        {
            loadval = "seattype";
        }
        if (colval == "17")
        {
            loadval = "co_curricular";
        }
        if (colval == "18")
        {
            loadval = "parent_addressP";
        }
        if (colval == "19")
        {
            loadval = "Streetp";
        }
        if (colval == "20")
        {
            loadval = "cityp";
        }
        if (colval == "21")
        {
            loadval = "parent_statep";
        }
        if (colval == "22")
        {
            loadval = "Countryp";
        }
        if (colval == "23")
        {
            loadval = "Student_Mobile";
        }
        if (colval == "24")
        {
            loadval = "parent_pincodep";
        }
        if (colval == "25")
        {
            loadval = "parent_phnop";
        }
        if (colval == "26")
        {
            loadval = "MissionaryChild";
        }
        if (colval == "27")
        {
            loadval = "missionarydisc";
        }
        if (colval == "28")
        {
            loadval = "Institute_name";
        }
        if (colval == "29")
        {
            loadval = "Part1Language";
        }
        if (colval == "30")
        {
            loadval = "Part2Language";
        }
        if (colval == "31")
        {
            loadval = "University";
        }
        if (colval == "40")
        {
            loadval = "uni_state";

        }

        if (colval == "32")
        {
            loadval = "LastTCNo";
        }
        if (colval == "33")
        {
            loadval = "LastTCDate";
        }
        if (colval == "35")
        {
            loadval = "ElectionID_No";
        }
        if (colval == "34")
        {
            loadval = "Twelth_CertNo";
        }
        if (colval == "35")
        {
            loadval = "CommunityNo";
        }
        if (colval == "36")
        {
            loadval = "DeplomProv_CertNo";
        }
        if (colval == "37")
        {
            loadval = "DeplomConsolidate_CertNo";
        }
        if (colval == "38")
        {
            loadval = "DeplomDegree_CertNo";
        }
        if (colval == "39")
        {
            loadval = "type_semester";
        }
        if (colval == "40")
        {
            loadval = "UGProv_CertNo";
        }
        if (colval == "41")
        {
            loadval = "UGConsolidate_CertNo";
        }
        if (colval == "42")
        {
            loadval = "UGDegree_CertNo";
        }
        if (colval == "43")
        {
            loadval = "type_semester";
        }
        if (colval == "44")
        {
            loadval = "PGProv_CertNo";
        }
        if (colval == "45")
        {
            loadval = "PGConsolidate_CertNo";
        }
        if (colval == "46")
        {
            loadval = "PGDegree_CertNo";
        }
        if (colval == "47")
        {
            loadval = "type_semester";
        }
        if (colval == "48")
        {
            loadval = "CampusReq";
        }
        if (colval == "49")
        {
            loadval = "handy";
        }
        if (colval == "50")
        {
            loadval = "DistinctSport";
        }
        if (colval == "51")
        {
            loadval = "islearningdis";
        }
        if (colval == "52")
        {
            loadval = "isdisabledisc";
        }
        if (colval == "53")
        {
            loadval = "isdisable";
        }
        if (colval == "54")
        {
            loadval = "stud_name";
        }
        if (colval == "55")
        {
            loadval = "Roll_no";
        }
        if (colval == "56")
        {
            loadval = "StuPer_Id";
        }
        if (colval == "57")
        {
            loadval = "reg_no";
        }
        if (colval == "58")
        {
            loadval = "roll_admit";
        }
        if (colval == "59")
        {
            loadval = "app_formno";
        }
        if (colval == "60")
        {
            loadval = "sections";
        }
        if (colval == "61")
        {
            loadval = "sex";
        }
        if (colval == "62")
        {
            loadval = "bldgrp";
        }
        if (colval == "63")
        {
            loadval = "stud_type";
        }
        if (colval == "64")
        {
            loadval = "IsExService";
        } if (colval == "65")
        {
            loadval = "CampusReq";
        }
        if (colval == "66")
        {
            loadval = "isdonar";
        }
        if (colval == "67")
        {
            loadval = "ReserveCategory";
        }
        if (colval == "68")
        {
            loadval = "EconBackword";
        }
        if (colval == "69")
        {
            loadval = "parentoldstud";
        }
        if (colval == "70")
        {
            loadval = "IsDrivingLic";
        }
        if (colval == "71")
        {
            loadval = "Driving_details";
        }
        if (colval == "72")
        {
            loadval = "tutionfee_waiver";
        }
        if (colval == "73")
        {
            loadval = "IsInsurance";
        }
        if (colval == "74")
        {
            loadval = "ExsRank";
        }
        if (colval == "75")
        {
            loadval = "ExSPlace";
        }
        if (colval == "76")
        {
            loadval = "ExsNumber";
        }
        if (colval == "77")
        {
            loadval = "Insurance_Amount";
        }
        if (colval == "78")
        {
            loadval = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            loadval = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            loadval = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            loadval = "date_applied";
        }
        if (colval == "82")
        {
            loadval = "alter_mobileno";
        }
        if (colval == "83")
        {
            loadval = "SubCaste";
        }
        if (colval == "84")
        {
            loadval = "parent_income";
        }
        if (colval == "85")
        {
            loadval = "parentF_Mobile";
        }
        if (colval == "86")
        {
            loadval = "emailp";
        }
        if (colval == "87")
        {
            loadval = "mother";
        }
        if (colval == "88")
        {
            loadval = "mIncome";
        }
        if (colval == "89")
        {
            loadval = "parentM_Mobile";
        }
        if (colval == "90")
        {
            loadval = "emailM";
        }
        if (colval == "91")
        {
            loadval = "guardian_name";
        }
        if (colval == "92")
        {
            loadval = "guardian_mobile";
        }
        if (colval == "93")
        {
            loadval = "emailg";
        }
        if (colval == "94")
        {
            loadval = "place_birth";
        }
        if (colval == "95")
        {
            loadval = "Aadharcard_no";
        }
        if (colval == "96")
        {
            loadval = "motherocc";
        }
        if (colval == "97")
        {
            loadval = "instaddress";
        }
        if (colval == "98")
        {
            loadval = "Xmedium";
        }
        if (colval == "99")
        {
            loadval = "medium";
        }
        if (colval == "100")
        {
            loadval = "percentage";
        }
        if (colval == "101")
        {
            loadval = "securedmark";
        }
        if (colval == "102")
        {
            loadval = "totalmark";
        }
        if (colval == "103")
        {
            loadval = "passmonth";
        }
        if (colval == "104")
        {
            loadval = "passyear";
        }
        if (colval == "105")
        {
            loadval = "Vocational_stream";
        }
        if (colval == "106")
        {
            loadval = "markPriority";
        }
        if (colval == "107")
        {
            loadval = "Cut_Of_Mark";
        }
        if (colval == "108")
        {
            loadval = "parent_addressc";
        }
        if (colval == "109")
        {
            loadval = "Streetc";
        }
        if (colval == "110")
        {
            loadval = "cityc";
        }
        if (colval == "111")
        {
            loadval = "parent_statec";
        }
        if (colval == "112")
        {
            loadval = "Countryc";
        }
        if (colval == "113")
        {
            loadval = "parent_pincodec";
        }
        if (colval == "114")
        {
            loadval = "AccNo";
        }
        if (colval == "115")
        {
            loadval = "DebitCardNo";
        }
        if (colval == "116")
        {
            loadval = "IFSCCode";
        }
        if (colval == "117")
        {
            loadval = "BankName";
        }
        if (colval == "118")
        {
            loadval = "Branch";
        }
        if (colval == "119")
        {
            loadval = "name_roll";
        }
        if (colval == "120")
        {
            loadval = "relationship";
        }
        if (colval == "121")
        {
            loadval = "isstaff";
        }
        if (colval == "122")
        {
            loadval = "Boarding";
        }
        if (colval == "123")
        {
            loadval = "vehid";
        }
        if (colval == "36")//delsii
        {
            loadval = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "37")
        {
            loadval = "CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date";
        }
        if (colval == "38")//delsii
        {
            loadval = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "125")
        {
            loadval = "CGPA";
        }
        if (colval == "126")
        {
            loadval = "No_of_arrear";
        }
        if (Convert.ToInt32(colval) > 123)
        {
            loadval = d2.GetFunction("select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + cblclg.SelectedItem.Value + "' and MasterCode='" + colval + "' ");
        }
    }

    protected string loadlcolumns()
    {
        string val = string.Empty;
        string selQry = "";
        try
        {
            string linkname = Convert.ToString(ddlMainreport.SelectedItem.Text);
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code in('" + cblclg.SelectedItem.Value + "') ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {

                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colval = Convert.ToString(valuesplit[k]);
                                string SelcolVal = loadtext(0);
                                if (selQry == "")
                                    selQry = SelcolVal;
                                else
                                    selQry = selQry + "," + SelcolVal;
                                lb_column1.Items.Add(new ListItem(loadval, colval));
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "StudentStrengthStatusReport.aspx"); }
        return selQry;
    }

    public void savecolumnorder()
    {
        string columnvalue = string.Empty;
        DataSet dscol = new DataSet();
        string linkname = Convert.ToString(ddl_coltypeadd.SelectedItem.Text);
        string val = string.Empty;
        for (int j = 0; j < lb_column1.Items.Count; j++)
        {
            val = lb_column1.Items[j].Value;
            if (columnvalue == "")
            {
                columnvalue = val;
            }
            else
            {
                columnvalue = columnvalue + ',' + val;
            }
        }
        int clsupdate = 0;
        for (int i = 0; i < cblclg.Items.Count; i++)
        {
            if (cblclg.Items[i].Selected == true)
            {
                string colcode = Convert.ToString(cblclg.Items[i].Value);
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code='" + colcode + "' and user_code='" + usercode + "'  ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "'  and college_code='" + colcode + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + colcode + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
        }
        if (clsupdate > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
    }

    public string loadtext(int i)
    {

        string val = string.Empty;
        if (colval == "1")
        {
            loadval = "Degree";
            printval = "(select c.course_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( a.degree_code,0)) as CourseName";
        }
        if (colval == "2")
        {
            loadval = "Department";
            printval = "(select dt.dept_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( a.degree_code,0)) as Department";
        }
        if (colval == "3")
        {
            loadval = "Batch";
            if (rbtype.SelectedIndex == 0)
            {
                printval = "a.Batch_Year";
            }
            else
            {
                printval = "r.Batch_Year";
            }
        }
        if (colval == "4")
        {
            loadval = "Semester";
            printval = "a.Current_Semester";
        }
        if (colval == "5")
        {
            loadval = "Father Name";
            printval = "parent_name";
        }
        if (colval == "6")
        {
            loadval = "DOB";
            //   printval = "dob";
            printval = "convert(varchar(10),dob,103)";//change by abarna
        }
        if (colval == "7")
        {
            loadval = "Parent Occupation";
            printval = "parent_occu";
        }
        if (colval == "8")
        {
            loadval = "Mother Tongue";
            printval = "mother_tongue";
        }
        if (colval == "9")
        {
            loadval = "Religion";
            printval = "religion";
        }
        if (colval == "10")
        {
            loadval = "Citizen";
            printval = "citizen";
        }
        if (colval == "11")
        {
            loadval = "Community";
            printval = "(select TextVal from TextValtable where TExtCode=isnull(a.community,0)) as Community";
        }
        if (colval == "12")
        {
            loadval = "Caste";
            printval = "caste";
        }
        if (colval == "13")
        {
            loadval = "TamilOrginFromAndaman";
            printval = "TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            loadval = "VisualHandy";
            printval = "visualhandy";
        }
        if (colval == "15")
        {
            loadval = "First Graduate";
            printval = "first_graduate";
        }
        if (colval == "16")
        {
            loadval = "SeatType";
            printval = "(select TextVal from TextValtable where TExtCode=isnull(a.seattype,0)) as SeatType ";
        }
        if (colval == "17")
        {
            loadval = "Curricular";
            printval = "co_curricular";
        }
        if (colval == "18")
        {
            loadval = "Address";
            printval = "parent_addressP";
        }
        if (colval == "19")
        {
            loadval = "Street";
            printval = "Streetp";
        }
        if (colval == "20")
        {
            loadval = "City";
            printval = "(select textval from textvaltable where CONVERT(varchar,TextCode)=CONVERT(varchar, a.Cityp) and TextCriteria='city')as cityp";//abarna change on 15.08.2018 for Displaying City Name
            //   printval = "cityp";

        }
        if (colval == "21")
        {
            loadval = "State";
            printval = "parent_statep";
        }
        if (colval == "22")
        {
            loadval = "Country";
            printval = "Countryp";
        }
        if (colval == "23")
        {
            loadval = "Student Mobile";
            printval = "Student_Mobile";
        }
        if (colval == "24")
        {
            loadval = "PinCode";
            printval = "parent_pincodep";
        }
        if (colval == "25")
        {
            loadval = "Parent Phone No";
            printval = "parent_phnop";
        }
        if (colval == "26")
        {
            loadval = "MissionaryChild";
            printval = "MissionaryChild";
        }
        if (colval == "27")
        {
            loadval = "missionarydisc";
            printval = "missionarydisc";
        }
        if (colval == "28")
        {
            loadval = "Institute Name";
            printval = "(select Coll_acronymn from collinfo where college_code =isnull( a.college_code,0)) as Institute_name";
        }
        if (colval == "29")
        {
            loadval = "Part1 Language";
            printval = "Part1Language";
        }
        if (colval == "30")
        {
            loadval = "Part2 Language";
            printval = "Part2Language";
        }
        if (colval == "31")
        {
            loadval = "University Name";
            printval = "University";
        }
        if (colval == "40")
        {
            loadval = "State";
            printval = "uni_state";

        }
        if (colval == "32")
        {
            loadval = "LastTC No";
            printval = "LastTCNo";
        }
        if (colval == "33")
        {
            loadval = "LastTC Date";
            printval = "LastTCDate";
        }
        if (colval == "34")
        {
            loadval = "HostelName";
            printval = "HostelName";
        }
        if (colval == "35")
        {
            loadval = "Voter ID";
            printval = "ElectionID_No";
        }
        if (colval == "36")
        {
            loadval = "Diploma-Provisional No";
        }
        if (colval == "37")
        {
            loadval = "Diploma-Consolidate";
        }
        if (colval == "38")
        {
            loadval = "Diploma-Degree";
        }
        if (colval == "39")
        {
            loadval = "Diploma- No of Semester";
        }
        if (colval == "40")
        {
            loadval = "UG-Provisional No";
        }
        if (colval == "41")
        {
            loadval = "UG-Consolidate";
        }
        if (colval == "42")
        {
            loadval = "UG-Degree";
        }
        if (colval == "43")
        {
            loadval = "UG- No of Semester";
        }
        if (colval == "44")
        {
            loadval = "PG-Provisional No";
        }
        if (colval == "45")
        {
            loadval = "PG-Consolidate";
        }
        if (colval == "46")
        {
            loadval = "PG-Degree";
        }
        if (colval == "47")
        {
            loadval = "PG- No of Semester";
        }
        if (colval == "48")
        {
            loadval = "Residency";
            printval = "CampusReq";
        }
        if (colval == "49")
        {
            loadval = "Physically challange";
            printval = "handy";
        }
        if (colval == "50")
        {
            printval = "DistinctSport";
            loadval = "Sports";
        }
        if (colval == "51")
        {
            printval = "islearningdis";
            loadval = "Learning Disability";
        }
        if (colval == "52")
        {
            printval = "isdisabledisc";
            loadval = "Other Disability";
        }
        if (colval == "53")
        {
            loadval = "IsDisable";
            printval = "isdisable";
        }
        if (colval == "54")
        {
            loadval = "Student Name";
            printval = "a.stud_name";
        }
        if (colval == "55")
        {
            loadval = "Roll No";
            printval = "Roll_no";
        }
        if (colval == "56")
        {
            loadval = "Student EmailId";
            printval = "StuPer_Id";
        }
        if (colval == "57")
        {
            loadval = "Reg No";
            printval = "reg_no";
        }
        if (colval == "58")
        {
            loadval = "Admission No";
            printval = "roll_admit";
        }
        if (colval == "59")
        {
            loadval = "Application No";
            printval = "app_formno";
        }
        if (colval == "60")
        {
            loadval = "Section";
            printval = "sections";
        }
        if (colval == "61")
        {
            loadval = "Gender";
            printval = "case when sex='0' then 'Male' when sex='1' then 'Female' end sex";

        }
        if (colval == "62")
        {
            loadval = "Blood Group";
            printval = "bldgrp";
        }
        if (colval == "63")
        {
            loadval = "Student Type";
            printval = "stud_type";
        }
        if (colval == "64")
        {
            loadval = "Ex-serviceman";
            printval = "IsExService";
        }
        if (colval == "65")
        {
            loadval = "Hostel accommodation";
            printval = "CampusReq";
        }
        if (colval == "66")
        {
            loadval = "Blood Donor";
            printval = "isdonar";
        }
        if (colval == "67")
        {
            loadval = "Reserved Caste";
            printval = "ReserveCategory";
        }
        if (colval == "68")
        {
            loadval = "Economic Backward";
            printval = "EconBackword";
        }
        if (colval == "69")
        {
            loadval = "Parents Old Student";
            printval = "parentoldstud";
        }
        if (colval == "70")
        {
            loadval = "Driving License";
            printval = "IsDrivingLic";
        }
        if (colval == "71")
        {
            loadval = "License No";
            printval = "Driving_details";
        }
        if (colval == "72")
        {
            loadval = "Tuition Fee Waiver";
            printval = "tutionfee_waiver";
        }
        if (colval == "73")
        {
            loadval = "Insurance";
            printval = "IsInsurance";
        }
        if (colval == "74")
        {
            loadval = "Rank";
            printval = "ExsRank";
        }
        if (colval == "75")
        {
            loadval = "Place";
            printval = "ExSPlace";
        }
        if (colval == "76")
        {
            loadval = "Number";
            printval = "ExsNumber";
        }
        if (colval == "77")
        {
            loadval = "Insurance Amount";
            printval = "Insurance_Amount";
        }
        if (colval == "78")
        {
            loadval = "Insurance InsBy";
            printval = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            loadval = "Insurance Nominee";
            printval = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            loadval = "Insurance NominRelation";
            printval = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            loadval = "Applied Date";
            // printval = "date_applied";
            printval = "convert(varchar(10),date_applied,103)";//change by abarna for displaying date only
        }
        if (colval == "82")
        {
            loadval = "Alternate Mob No";
            printval = "alter_mobileno";
        }
        if (colval == "83")
        {
            loadval = "Sub Caste";
            printval = "SubCaste";
        }
        if (colval == "84")
        {
            loadval = "Father Income";
            printval = "parent_income";
        }
        if (colval == "85")
        {
            loadval = "Father Mob No";
            printval = "parentF_Mobile";
        }
        if (colval == "86")
        {
            loadval = "Father EmailId";
            printval = "emailp";
        }
        if (colval == "87")
        {
            loadval = "Mother";
            printval = "mother";
        }
        if (colval == "88")
        {
            loadval = "Mother Income";
            printval = "mIncome";
        }
        if (colval == "89")
        {
            loadval = "Mother Mob No";
            printval = "parentM_Mobile";
        }
        if (colval == "90")
        {
            loadval = "Mother EmailId";
            printval = "emailM";
        }
        if (colval == "91")
        {
            loadval = "Guardian Name";
            printval = "guardian_name";
        }
        if (colval == "92")
        {
            loadval = "Guardian Mob No";
            printval = "guardian_mobile";
        }
        if (colval == "93")
        {
            loadval = "Guardian Email Id";
            printval = "emailg";
        }
        if (colval == "94")
        {
            loadval = "Place Of Birth";
            printval = "place_birth";
        }
        if (colval == "95")
        {
            loadval = "Adhaar Card No";
            printval = "Aadharcard_no";
        }
        if (colval == "96")
        {
            loadval = "Mother Occupation";
            printval = "motherocc";
        }
        if (colval == "97")
        {
            loadval = "Institution Address";
            printval = "instaddress";
        }
        if (colval == "98")
        {
            loadval = "X medium";
            printval = "Xmedium";
        }
        if (colval == "99")
        {
            loadval = "X11 Medium";
            printval = "medium";
        }
        if (colval == "100")
        {
            loadval = "Percentage";
            printval = "percentage";
        }
        if (colval == "101")
        {
            loadval = "Secured Mark";
            printval = "securedmark";
        }
        if (colval == "102")
        {
            printval = "totalmark";
            loadval = "Total Mark";
        }
        if (colval == "103")
        {
            loadval = "Pass Month";
            printval = "passmonth";
        }
        if (colval == "104")
        {
            loadval = "Pass Year";
            printval = "passyear";
        }
        if (colval == "105")
        {
            loadval = "Vocational Stream";
            printval = "Vocational_stream";
        }
        if (colval == "106")
        {
            loadval = "Mark Priority";
            printval = "markPriority";
        }
        if (colval == "107")
        {
            loadval = "Cut Of Mark";
            printval = "Cut_Of_Mark";
        }
        if (colval == "108")
        {
            loadval = "Communication Address";
            printval = "parent_addressc";
        }
        if (colval == "109")
        {
            loadval = "Communication Street";
            printval = "Streetc";
        }
        if (colval == "110")
        {
            loadval = "Communication City";
            printval = "cityc";
        }
        if (colval == "111")
        {
            loadval = "Communication State";
            printval = "parent_statec";
        }
        if (colval == "112")
        {
            loadval = "Communication Country";
            printval = "Countryc";
        }
        if (colval == "113")
        {
            printval = "parent_pincodec";
            loadval = "Communication PinCode";
        }
        if (colval == "114")
        {
            loadval = "A/C No";
            printval = "AccNo";
        }
        if (colval == "115")
        {
            printval = "DebitCardNo";
            loadval = "DebitCard No";
        }
        if (colval == "116")
        {
            loadval = "IFSCCode";
            printval = "IFSCCode";
        }
        if (colval == "117")
        {
            loadval = "Bank Name";
            printval = "BankName";
        }
        if (colval == "118")
        {
            printval = "Branch";
            loadval = "Branch";
        }
        if (colval == "119")
        {
            printval = "name_roll";
            loadval = "Relation Name";
        }
        if (colval == "120")
        {
            printval = "relationship";
            loadval = "Relationship";
        }
        if (colval == "121")
        {
            printval = "isstaff";
            loadval = "Staff/Student";
        }
        if (colval == "122")
        {
            printval = "Boarding";
            loadval = "Boarding";
        }
        if (colval == "123")
        {
            printval = "vehid";
            loadval = "Vehicle Id";
        }
        if (colval == "43")
        {
            printval = "case when Mode='1' then 'Regular' when Mode='2' then 'Transfer' when Mode='3' then 'Lateral' when Mode='4' then 'Irregular' end Mode";
            loadval = "Mode";
        }
        if (colval == "36")
        {
            // printval = "Adm_Date";
            loadval = "Admission Date";
            printval = "convert(varchar(10),Adm_Date,103)";
        }
        if (colval == "37")
        {
            printval = "enrollment_confirm_date";
            loadval = "Enrollment Date";
        }
        if (colval == "38")
        {
            printval = "Adm_Date";
            loadval = "Join Date";
        }
        if (colval == "125")
        {
            printval = "CGPA";
            loadval = "CGPA";
        }
        if (colval == "126")
        {
            printval = "noofarrear";
            loadval = "No of arrear";
        }
        if (colval == "127")//added
        {
            printval = "referby";
            loadval = "Refered By";
        }
        if (colval == "128")
        {
            printval = "(select c.course_name+'-'+dt.dept_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( a.Alternativedegree_code,0)) as alterdegree_code";
            loadval = "Alternative Course";
        }
        //if (Convert.ToInt32(colval) > 127)
        //{
        //    loadval = d2.GetFunction("select distinct MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + cblclg.SelectedItem.Value + "' and MasterCode='" + colval + "' ");
        //    printval = d2.GetFunction("select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + cblclg.SelectedItem.Value + "' and MasterCode='" + colval + "' ");
        //}


        if (i == 0)
            val = printval;
        else if (i == 1)
            val = loadval;
        return val;
    }

    protected void btnMvOneRt_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lb_selectcolumn.Items.Count > 0 && lb_selectcolumn.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_column1.Items.Count; j++)
                {
                    if (lb_column1.Items[j].Value == lb_selectcolumn.SelectedItem.Value)
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    ListItem lst = new ListItem(lb_selectcolumn.SelectedItem.Text, lb_selectcolumn.SelectedItem.Value);
                    lb_column1.Items.Add(lst);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "StudentStrengthStatusReport.aspx"); }
    }

    protected void btnMvTwoRt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_column1.Items.Clear();
            if (lb_selectcolumn.Items.Count > 0)
            {
                for (int j = 0; j < lb_selectcolumn.Items.Count; j++)
                {
                    lb_column1.Items.Add(new ListItem(lb_selectcolumn.Items[j].Text.ToString(), lb_selectcolumn.Items[j].Value.ToString()));
                }
            }
            lb_selectcolumn.Items.Clear();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "StudentStrengthStatusReport.aspx"); }
    }

    protected void btnMvOneLt_Click(object sender, EventArgs e)
    {
        try
        {
            if (lb_column1.Items.Count > 0 && lb_column1.SelectedItem.Value != "")
            {
                lb_column1.Items.RemoveAt(lb_column1.SelectedIndex);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "StudentStrengthStatusReport.aspx"); }
    }

    protected void btnMvTwoLt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_column1.Items.Clear();
            load();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "StudentStrengthStatusReport.aspx"); }
    }

    protected void btnok_click(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedItem.Text != "Select")
        {
            if (lb_column1.Items.Count > 0)
            {
                poppernew.Visible = false;
                savecolumnorder();
                if (savecolumnoder == "")
                {
                    //fpspread1go1();
                }
                else
                {
                    //if (rdb_cumm.Checked == true)
                    //{
                    //    go();
                    //}
                    //else
                    //{
                    //    fpspread1go1();
                    //}
                    savecolumnoder = string.Empty;
                }
                lblalerterr.Visible = false;
            }
            else
            {
                lblalerterr.Visible = true;
                lblalerterr.Text = "Please select atleast one colunm then proceed!";
            }
        }
        else
        {
            Div2.Visible = true;
            lbl_alert.Text = "Please Select Report Type";
        }
    }

    protected void btnclose_click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }

    public void columnordertype()
    {
        ddlMainreport.Items.Clear();
        ddl_coltypeadd.Items.Clear();
        string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudentAdmission' and CollegeCode in('" + cblclg.SelectedItem.Value + "')";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlMainreport.DataSource = ds;
            ddlMainreport.DataTextField = "MasterValue";
            ddlMainreport.DataValueField = "MasterCode";
            ddlMainreport.DataBind();
            ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
            ddl_coltypeadd.DataSource = ds;
            ddl_coltypeadd.DataTextField = "MasterValue";
            ddl_coltypeadd.DataValueField = "MasterCode";
            ddl_coltypeadd.DataBind();
            ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
            ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    public void btn_addtype_OnClick(object sender, EventArgs e)
    {
        imgdiv33.Visible = true;
        panel_description11.Visible = true;
    }

    public void btn_deltype_OnClick(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedIndex == -1)
        {

            Div2.Visible = true;
            LblAlertMsg.Text = "No records found";
        }
        else if (ddl_coltypeadd.SelectedIndex == 0)
        {
            Div2.Visible = true;
            LblAlertMsg.Text = "Select any record";
        }
        else if (ddl_coltypeadd.SelectedIndex != 0)
        {
            string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='StudentAdmission' and CollegeCode in('" + cblclg.SelectedItem.Value + "') ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {
                Div2.Visible = true;
                LblAlertMsg.Text = "Deleted Sucessfully";
            }
            else
            {
                Div2.Visible = true;
                LblAlertMsg.Text = "No records found";
            }
            columnordertype();
        }
        else
        {
            Div2.Visible = true;
            lbl_alert.Text = "No records found";
        }
    }

    public void ddl_coltypeadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        viewcolumorder();
    }

    public void viewcolumorder()
    {
        try
        {
            lb_column1.Items.Clear();
            if (ddl_coltypeadd.SelectedItem.Text != "Select")
            {
                string q = "select LinkValue from New_InsSettings where LinkName='" + ddl_coltypeadd.SelectedItem.Text + "' and college_code in('" + cblclg.SelectedItem.Value + "') and user_code='" + usercode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string vall = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                    string[] sp = vall.Split(',');
                    for (int y = 0; y < sp.Length; y++)
                    {
                        colval = sp[y];
                        loadtext(1);
                        lb_column1.Items.Add(new System.Web.UI.WebControls.ListItem(loadval, Convert.ToString(sp[y])));
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void btndescpopadd_Click(object sender, EventArgs e)
    {
        if (txt_description11.Text != "")
        {
            int insert = 0;
            for (int i = 0; i < cblclg.Items.Count; i++)
            {
                if (cblclg.Items[i].Selected == true)
                {
                    string colcode = Convert.ToString(cblclg.Items[i].Value);
                    string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='StudentAdmission' and CollegeCode ='" + colcode + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='StudentAdmission' and CollegeCode ='" + colcode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','StudentAdmission','" + colcode + "')";
                    insert = d2.update_method_wo_parameter(sql, "TEXT");
                }
            }
            if (insert != 0)
            {
                Div2.Visible = true;
                LblAlertMsg.Text = "Added sucessfully";
                txt_description11.Text = string.Empty;
                imgdiv33.Visible = false;
            }
        }
        else
        {
            Div2.Visible = true;
            pnl2.Visible = true;
            LblAlertMsg.Text = "Enter the description";
        }
        columnordertype();
    }

    public void btndescpopexit_Click(object sender, EventArgs e)
    {
        panel_description11.Visible = false;
        imgdiv33.Visible = false;
    }

    public void btn_errorcloseAlert_Click(object sender, EventArgs e)
    {
        Div2.Visible = false;
    }

    #endregion
}