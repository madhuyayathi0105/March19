using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Configuration;

public partial class PostMetricScholarship : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static ArrayList arroll = new ArrayList();
    static int personmode = 0;
    static int chosedmode = 0;
    static byte roll = 0;
    static string colgCode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                    colgCode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }

                loadsetting();
                headerload();
                ledgerload();
                bindsem();
                bindaddreason();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");

                txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtfrom.Attributes.Add("readonly", "readonly");

                txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtto.Attributes.Add("readonly", "readonly");

                txtsclNo.Attributes.Add("readonly", "readonly");
                txtsclNo.Text = generateScholarShipNo();
                RollAndRegSettings();
                rblMode_Selected(sender, e);
                arroll.Clear();
                loaddesc1();//Added by saranya on 2/8/2018
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                colgCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }

    #region roll,reg,admission setting
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }

    #endregion

    #region college

    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ddlcollege.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        { }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcollege.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            colgCode = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
        headerload();
        bindsem();
        gdstuddet.DataSource = null;
        gdstuddet.DataBind();
        gdstuddet.Visible = false;
        arroll.Clear();
        loaddesc1();
    }
    #endregion

    #region auto search
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();
            if (personmode == 0)
            {
                //student query
                if (chosedmode == 0)
                {
                    query = "select top 100 Roll_No from Registration where   Roll_No like '" + prefixText + "%' and college_code='" + colgCode + "' order by Roll_No asc ";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where   Reg_No like '" + prefixText + "%' and college_code='" + colgCode + "' order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where   Roll_admit like '" + prefixText + "%' and college_code='" + colgCode + "' order by Roll_admit asc";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code='" + colgCode + "' order by app_formno asc";
                }
            }
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    public void loadsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");

            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedItem.Value + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedItem.Value + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedItem.Value + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedItem.Value + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list4);
            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(list1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_rollno.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_rollno.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_rollno.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_rollno.Attributes.Add("placeholder", "App No");
                    chosedmode = 3;
                    break;
            }



        }
        catch { }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //stud
            txt_rollno.Text = "";

            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_rollno.Attributes.Add("Placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_rollno.Attributes.Add("Placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_rollno.Attributes.Add("Placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_rollno.Attributes.Add("Placeholder", "App No");
                    chosedmode = 2;
                    break;
            }
        }
        catch { }
    }

    protected void txt_rollno_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable tempdt = new DataTable();
            DataTable dt = new DataTable();

            tempdt.Columns.Add("SNo");
            tempdt.Columns.Add("appno");
            tempdt.Columns.Add("rollno");
            tempdt.Columns.Add("regno");
            tempdt.Columns.Add("addno");
            tempdt.Columns.Add("Name");
            tempdt.Columns.Add("degree");
            DataRow dr;
            DataRow drrows;
            string rollno = "";
            if (lbldisp.Text.Trim() != "")
                rollno = Convert.ToString(lbldisp.Text);
            else
                rollno = Convert.ToString(txt_rollno.Text);

            if (!string.IsNullOrEmpty(rollno))
            {
                string[] splVal = rollno.Split(',');
                foreach (string splroll in splVal)
                {
                    string appno = getAppno(splroll);
                    if (appno != "0")
                    {
                        #region
                        if (!arroll.Contains(appno))
                        {
                            #region old date
                            if (gdstuddet.Rows.Count > 0)
                            {
                                foreach (GridViewRow item in gdstuddet.Rows)
                                {
                                    Label lblappno = (Label)item.FindControl("lblappno");
                                    Label lblname = (Label)item.FindControl("lblname");
                                    Label lblroll = (Label)item.FindControl("lblroll");
                                    Label lblreg = (Label)item.FindControl("lblreg");
                                    Label lbladd = (Label)item.FindControl("lbladd");
                                    Label lbldeg = (Label)item.FindControl("lbldeg");
                                    if (!arroll.Contains(lblappno.Text.Trim()))
                                    {
                                        dr = tempdt.NewRow();
                                        dr["SNo"] = Convert.ToString(tempdt.Rows.Count + 1);
                                        dr["appno"] = Convert.ToString(lblappno.Text);
                                        dr["rollno"] = Convert.ToString(lblroll.Text);
                                        dr["regno"] = Convert.ToString(lblreg.Text);
                                        dr["addno"] = Convert.ToString(lbladd.Text);
                                        dr["Name"] = Convert.ToString(lblname.Text);
                                        dr["degree"] = Convert.ToString(lbldeg.Text);
                                        tempdt.Rows.Add(dr);
                                    }
                                }
                            }
                            #endregion
                            string selQ = " select r.app_no, stud_name,roll_no,reg_no,roll_admit,r.degree_code,(c.course_name+'-'+dt.dept_name) as dept_name from registration r,degree d,course c,department dt where app_no='" + appno + "' and d.degree_code=r.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code='" + ddlcollege.SelectedItem.Value + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selQ, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                dr = tempdt.NewRow();
                                dr["SNo"] = Convert.ToString(tempdt.Rows.Count + 1);
                                dr["appno"] = Convert.ToString(ds.Tables[0].Rows[0]["app_no"]);
                                dr["rollno"] = Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]);
                                dr["regno"] = Convert.ToString(ds.Tables[0].Rows[0]["reg_no"]);
                                dr["addno"] = Convert.ToString(ds.Tables[0].Rows[0]["roll_admit"]);
                                dr["Name"] = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]);
                                dr["degree"] = Convert.ToString(ds.Tables[0].Rows[0]["dept_name"]);
                                tempdt.Rows.Add(dr);
                            }
                            gdstuddet.DataSource = tempdt;
                            gdstuddet.DataBind();
                            gdstuddet.Visible = true;
                            txt_rollno.Text = "";
                            arroll.Add(appno);
                            RollAndRegSettings();
                            gridColumnsVisible();
                        }
                        else
                        {
                            txt_rollno.Text = "";
                            imgdiv2.Visible = true;
                            lbl_alert.Text = rbl_rollno.SelectedItem.Text + " Already Added";
                        }
                        #endregion
                    }
                    else
                    {
                        txt_rollno.Text = "";
                        imgdiv2.Visible = true;
                        lbl_alert.Text = rbl_rollno.SelectedItem.Text + " Not Valid";
                    }
                }
            }
            else
            {
                txt_rollno.Text = "";
                imgdiv2.Visible = true;
                lbl_alert.Text = rbl_rollno.SelectedItem.Text + " Not Valid";
            }
        }
        catch { }
    }

    protected string getAppno(string rollno)
    {
        string appno = string.Empty;
        try
        {
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                appno = d2.GetFunction(" select App_No from Registration where Roll_No='" + rollno + "'");

            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                appno = d2.GetFunction(" select App_No from Registration where reg_no='" + rollno + "'");

            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                appno = d2.GetFunction(" select App_No from Registration where Roll_admit='" + rollno + "'");

            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                appno = d2.GetFunction(" select app_no from applyn where app_formno='" + rollno + "'");

        }
        catch { }
        return appno;
    }

    protected DataTable bindData(DataTable dt, DataTable dtold)
    {
        try
        {
            dt.Columns.Add("SNo");
            dt.Columns.Add("Name");
            dt.Columns.Add("Roll No");
            dt.Columns.Add("Reg No");
            dt.Columns.Add("Admission No");
            dt.Columns.Add("Degree");
            DataRow dr;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

            }

        }
        catch { }
        return dt;
    }

    #endregion

    #region headerandledger
    public void headerload()
    {
        try
        {
            ddlheader.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode + "  order by len(isnull(hd_priority,10000)),hd_priority asc";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlheader.DataSource = ds;
                ddlheader.DataTextField = "HeaderName";
                ddlheader.DataValueField = "HeaderPK";
                ddlheader.DataBind();
                // ddlheader.Items.Insert(0, new ListItem("Select", "0"));
                ledgerload();
            }
        }
        catch
        {
        }
    }
    public void ledgerload()
    {
        try
        {
            ddlledger.Items.Clear();
            string header = string.Empty;
            if (ddlheader.Items.Count > 0)
                header = Convert.ToString(ddlheader.SelectedItem.Value);

            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode + "  and L.HeaderFK in('" + header + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlledger.DataSource = ds;
                ddlledger.DataTextField = "LedgerName";
                ddlledger.DataValueField = "LedgerPK";
                ddlledger.DataBind();
                // ddlledger.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        {
        }
    }
    public void ddlheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        ledgerload();
    }

    public void ddlledger_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    #endregion

    protected void gridColumnsVisible()
    {
        try
        {
            int a = gdstuddet.Columns.Count;

            for (int i = 0; i < gdstuddet.Rows.Count; i++)
            {
                if (roll == 0)
                {
                    gdstuddet.HeaderRow.Cells[1].Visible = true;
                    gdstuddet.HeaderRow.Cells[2].Visible = true;
                    gdstuddet.HeaderRow.Cells[3].Visible = true;
                    gdstuddet.Rows[i].Cells[1].Visible = true;
                    gdstuddet.Rows[i].Cells[2].Visible = true;
                    gdstuddet.Rows[i].Cells[3].Visible = true;
                }
                else if (roll == 1)
                {
                    gdstuddet.HeaderRow.Cells[1].Visible = true;
                    gdstuddet.HeaderRow.Cells[2].Visible = true;
                    gdstuddet.HeaderRow.Cells[3].Visible = true;
                    gdstuddet.Rows[i].Cells[1].Visible = true;
                    gdstuddet.Rows[i].Cells[2].Visible = true;
                    gdstuddet.Rows[i].Cells[3].Visible = true;
                }
                else if (roll == 2)
                {
                    gdstuddet.HeaderRow.Cells[1].Visible = true;
                    gdstuddet.HeaderRow.Cells[2].Visible = false;
                    gdstuddet.HeaderRow.Cells[3].Visible = false;
                    gdstuddet.Rows[i].Cells[1].Visible = true;
                    gdstuddet.Rows[i].Cells[2].Visible = false;
                    gdstuddet.Rows[i].Cells[3].Visible = false;
                }
                else if (roll == 3)
                {
                    gdstuddet.HeaderRow.Cells[1].Visible = false;
                    gdstuddet.HeaderRow.Cells[2].Visible = true;
                    gdstuddet.HeaderRow.Cells[3].Visible = false;
                    gdstuddet.Rows[i].Cells[1].Visible = false;
                    gdstuddet.Rows[i].Cells[2].Visible = true;
                    gdstuddet.Rows[i].Cells[3].Visible = false;
                }
                else if (roll == 4)
                {
                    gdstuddet.HeaderRow.Cells[1].Visible = false;
                    gdstuddet.HeaderRow.Cells[2].Visible = false;
                    gdstuddet.HeaderRow.Cells[3].Visible = true;
                    gdstuddet.Rows[i].Cells[1].Visible = false;
                    gdstuddet.Rows[i].Cells[2].Visible = false;
                    gdstuddet.Rows[i].Cells[3].Visible = true;
                }
                else if (roll == 5)
                {
                    gdstuddet.HeaderRow.Cells[1].Visible = true;
                    gdstuddet.HeaderRow.Cells[2].Visible = true;
                    gdstuddet.HeaderRow.Cells[3].Visible = false;
                    gdstuddet.Rows[i].Cells[1].Visible = true;
                    gdstuddet.Rows[i].Cells[2].Visible = true;
                    gdstuddet.Rows[i].Cells[3].Visible = false;
                }
                else if (roll == 6)
                {
                    gdstuddet.HeaderRow.Cells[1].Visible = false;
                    gdstuddet.HeaderRow.Cells[2].Visible = true;
                    gdstuddet.HeaderRow.Cells[3].Visible = true;
                    gdstuddet.Rows[i].Cells[1].Visible = false;
                    gdstuddet.Rows[i].Cells[2].Visible = true;
                    gdstuddet.Rows[i].Cells[3].Visible = true;
                }
                else if (roll == 7)
                {
                    gdstuddet.HeaderRow.Cells[1].Visible = true;
                    gdstuddet.HeaderRow.Cells[2].Visible = false;
                    gdstuddet.HeaderRow.Cells[3].Visible = true;
                    gdstuddet.Rows[i].Cells[1].Visible = true;
                    gdstuddet.Rows[i].Cells[2].Visible = false;
                    gdstuddet.Rows[i].Cells[3].Visible = true;
                }
            }

        }
        catch { }
    }

    protected void bindsem()
    {
        try
        {
            ddlfeecat.Items.Clear();
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = d2.loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlfeecat.DataSource = ds;
                ddlfeecat.DataTextField = "TextVal";
                ddlfeecat.DataValueField = "TextCode";
                ddlfeecat.DataBind();
                ddlfeecat.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch { }
    }

    protected void bindaddreason()
    {
        try
        {
            //ddlreason.Items.Clear();
            //ds.Clear();
            //string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + collegecode + "'";
            //ds = d2.select_method_wo_parameter(sql, "TEXT");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlreason.DataSource = ds;
            //    ddlreason.DataTextField = "TextVal";
            //    ddlreason.DataValueField = "TextCode";
            //    ddlreason.DataBind();
            //    ddlreason.Items.Insert(0, new ListItem("Select", "0"));
            //    ddlreason.Items.Insert(ddlreason.Items.Count, "Others");
            //}
            //else
            //{
            //    ddlreason.Items.Insert(0, new ListItem("Select", "0"));
            //}
        }
        catch
        { }
    }

    protected void ddlreason_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            RollAndRegSettings();
            bool save = false;
            string header = string.Empty;
            string ledger = string.Empty;
            string feecat = string.Empty;
            string reason = string.Empty;
            string date = Convert.ToString(txt_fromdate.Text);
            string[] frdate = date.Split('/');
            if (frdate.Length == 3)
                date = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            if (ddlheader.Items.Count > 0)
                header = Convert.ToString(ddlheader.SelectedItem.Value);
            if (ddlledger.Items.Count > 0)
                ledger = Convert.ToString(ddlledger.SelectedItem.Value);
            if (ddlfeecat.Items.Count > 0)
                feecat = Convert.ToString(ddlfeecat.SelectedItem.Value);

            //if (ddlreason.SelectedItem.Value != "Select")
            //{
            //    if (ddlreason.SelectedItem.Value != "Others")
            //        reason = Convert.ToString(ddlreason.SelectedItem.Value);
            //    else
            //    {
            //        string txtreason = Convert.ToString(txt_reason.Text);
            //        reason = getDeduction(txtreason, collegecode);
            //    }
            //}
            collegecode = Convert.ToString(ddlcollege.SelectedValue);
            string fincyr = d2.getCurrentFinanceYear(usercode, collegecode);
            reason = Convert.ToString(txt_reason.Text);
            double Amt = 0;
            double bal = 0;
            double.TryParse(Convert.ToString(txtamt.Text), out Amt);
            string Rcptno = Convert.ToString(txtsclNo.Text);
            string schShipType = Convert.ToString(ddl_MulSclReason.SelectedItem.Text);
            string NeftNo = Convert.ToString(TxtNeftNo.Text);
            if (validate(header, ledger, feecat, reason, Amt, fincyr))
            {
                if (!string.IsNullOrEmpty(Rcptno) && Rcptno != "0")
                {
                    foreach (GridViewRow row in gdstuddet.Rows)
                    {
                        Label apptxt = (Label)row.FindControl("lblappno");
                        string appno = Convert.ToString(apptxt.Text);
                        string balAmount = d2.GetFunction("select (isnull(TotalAmount,0)-isnull(Paidamount,0)) as BalAmount from ft_feeallot where app_no='" + appno + "' and headerfk='" + header + "' and ledgerfk='" + ledger + "' and feecategory='" + feecat + "'");
                        double.TryParse(Convert.ToString(balAmount), out bal);
                        if (bal >= Amt)
                        {
                            if (!string.IsNullOrEmpty(appno))
                            {
                                string InsQ = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected,CollectedDate,IsDeposited,DepositedDate,DDNo,ScholarshipType) VALUES('" + date + "','" + DateTime.Now.ToLongTimeString() + "','" + Rcptno + "', '1', " + appno + ", " + ledger + ", " + header + ", " + feecat + ", '0', '" + Amt + "','7', '1', '0', 0, '" + reason + "', '0', '0', '0', 0, " + usercode + ", " + fincyr + ",'5','1','','1','','" + NeftNo + "','" + schShipType + "')";
                                InsQ += " update ft_feeallot set  Paidamount=isnull(Paidamount,'0')+'" + Amt + "',balAmount=isnull(BalAmount,'0') -'" + Amt + "' from ft_feeallot where app_no='" + appno + "' and headerfk='" + header + "' and ledgerfk='" + ledger + "' and feecategory='" + feecat + "'";
                                int updCnt = d2.update_method_wo_parameter(InsQ, "Text");
                                if (updCnt > 0)
                                {
                                    if (Convert.ToInt32(Session["isHeaderwise"]) == 0 || Convert.ToInt32(Session["isHeaderwise"]) == 2)
                                    {
                                        Rcptno = Rcptno.Remove(0, Convert.ToString(Session["acronym"]).Length);
                                        string updateRecpt = " update FM_FinCodeSettings set ScholarshipStNo=" + Rcptno + "+1 where collegecode =" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                                        d2.update_method_wo_parameter(updateRecpt, "Text");
                                        txtsclNo.Text = generateScholarShipNo();
                                        Rcptno = Convert.ToString(txtsclNo.Text);
                                    }
                                }
                                save = true;

                            }
                        }
                    }
                    if (save == true)
                    {
                        //if (Convert.ToInt32(Session["isHeaderwise"]) == 0 || Convert.ToInt32(Session["isHeaderwise"]) == 2)
                        //{
                        //    Rcptno = Rcptno.Remove(0, Convert.ToString(Session["acronym"]).Length);
                        //    string updateRecpt = " update FM_FinCodeSettings set ScholarshipStNo=" + Rcptno + "+1 where collegecode =" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + collegecode + ")";
                        //    d2.update_method_wo_parameter(updateRecpt, "Text");
                        //    txtsclNo.Text = generateScholarShipNo();
                        //}

                        Clear();
                        gdstuddet.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                    }
                    else
                    {
                        Clear();
                        gdstuddet.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please Enter Valid Roll/Reg No";
                    }
                }
                else
                {
                    gdstuddet.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Scholarship Number Not Generated";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter All The Details";
            }

        }
        catch { }
    }

    protected void gdstuddet_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
        }
    }

    protected bool validate(string header, string ledger, string feecat, string reason, double Amt, string fincyr)
    {
        bool check = false;
        try
        {
            if (!string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(ledger) && !string.IsNullOrEmpty(feecat) && !string.IsNullOrEmpty(reason) && Amt != 0 && fincyr != "0")
            {
                check = true;
            }
        }
        catch { }
        return check;
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void btnremove_Click(object sender, EventArgs e)
    {
        try
        {
            int rowCnt = rowIndxClicked();

            DataTable tempdt = new DataTable();
            DataTable dt = new DataTable();
            tempdt.Columns.Add("SNo");
            tempdt.Columns.Add("appno");
            tempdt.Columns.Add("Name");
            tempdt.Columns.Add("rollno");
            tempdt.Columns.Add("regno");
            tempdt.Columns.Add("addno");
            tempdt.Columns.Add("degree");
            if (gdstuddet.Rows.Count > 0)
            {
                for (int i = 0; i < gdstuddet.Rows.Count; i++)
                {
                    if (i != rowCnt)
                    {
                        Label lblappno = (Label)gdstuddet.Rows[i].FindControl("lblappno");
                        Label lblname = (Label)gdstuddet.Rows[i].FindControl("lblname");
                        Label lblroll = (Label)gdstuddet.Rows[i].FindControl("lblroll");
                        Label lblreg = (Label)gdstuddet.Rows[i].FindControl("lblreg");
                        Label lbladd = (Label)gdstuddet.Rows[i].FindControl("lbladd");
                        Label lbldeg = (Label)gdstuddet.Rows[i].FindControl("lbldeg");
                        DataRow dr = tempdt.NewRow();
                        dr["SNo"] = Convert.ToString(tempdt.Rows.Count + 1);
                        dr["appno"] = Convert.ToString(lblappno.Text);
                        dr["Name"] = Convert.ToString(lblname.Text);
                        dr["rollno"] = Convert.ToString(lblroll.Text);
                        dr["regno"] = Convert.ToString(lblreg.Text);
                        dr["addno"] = Convert.ToString(lbladd.Text);
                        dr["degree"] = Convert.ToString(lbldeg.Text);
                        tempdt.Rows.Add(dr);
                    }
                    else
                    {
                        Label lblappno = (Label)gdstuddet.Rows[i].FindControl("lblappno");
                        arroll.Remove(lblappno.Text);
                    }
                }
                gdstuddet.DataSource = tempdt;
                gdstuddet.DataBind();
                gdstuddet.Visible = true;
                RollAndRegSettings();
                gridColumnsVisible();
            }

        }
        catch { }
    }

    protected void Clear()
    {
        txtamt.Text = "";
        txt_rollno.Text = "";
        txt_reason.Text = "";
        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        headerload();
        ledgerload();
        bindsem();
        bindaddreason();
        loadsetting();
        arroll.Clear();
        ds.Clear();
        gdstuddet.DataSource = ds;
        gdstuddet.DataBind();
        //gdstuddet.DataSource = null;

    }

    protected string getDeduction(string text, string collegecode)
    {
        string deductcode = string.Empty;
        try
        {

            string sql = "if exists ( select * from TextValTable where TextVal ='" + text + "' and TextCriteria ='DedRe' and college_code ='" + collegecode + "') update TextValTable set TextVal ='" + text + "' where TextVal ='" + text + "' and TextCriteria ='DedRe' and college_code ='" + collegecode + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + text + "','DedRe','" + collegecode + "')";
            int insert = d2.update_method_wo_parameter(sql, "Text");
            if (insert == 1)
            {
                deductcode = d2.GetFunction("select textcode from TextValTable where TextVal ='" + text + "' and TextCriteria ='DedRe' and college_code ='" + collegecode + "')");
            }
        }
        catch { deductcode = string.Empty; }
        return deductcode;
    }

    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }

    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[3].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }

        return rownumber;
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();

        lbl.Add(lblclg);
        //lbl.Add(Label3);
        lbl.Add(Label3);
        fields.Add(0);
        //fields.Add(2);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    //added by sudhagar 19.09.2017
    protected void btnReport_Click(object sender, EventArgs e)
    {
        try
        {
            gdReport.Visible = false;
            string fromdate = txtfrom.Text;
            string todate = txtto.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            }

            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("Sno");
            dtReport.Columns.Add("Roll No");
            dtReport.Columns.Add("Reg No");
            dtReport.Columns.Add("Admission No");
            dtReport.Columns.Add("Name");
            dtReport.Columns.Add("Department");
            dtReport.Columns.Add("Header");
            dtReport.Columns.Add("Ledger");
            dtReport.Columns.Add("Paymode");
            dtReport.Columns.Add("Neft No");
            dtReport.Columns.Add("Scholarship Type");
            dtReport.Columns.Add("Receipt No");
            dtReport.Columns.Add("Receipt Date");           
            dtReport.Columns.Add("Amount");
            DataRow drRow;
            string selQ = "";
            string schChecked = txt_Scholarship.Text;
            if (schChecked != "--Select--")
            {
                string scholarShipType = Convert.ToString(getCblSelectedText(cbl_Scholarship));
                selQ = " select roll_no[Roll No],reg_no[Reg No],roll_admit[Admission No],stud_name[Name],(select distinct c.course_name+'-'+dt.dept_name from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=r.degree_code)[Department],h.headername[Header],ledgername[Ledger],convert(varchar(10),transdate,103) [Receipt Date],transcode[Receipt No],sum(debit)[Amount],PayMode[Paymode],DDNo[Neft No],ScholarshipType[Scholarship Type] from ft_findailytransaction f,registration r,fm_headermaster h,fm_ledgermaster l  where f.app_no=r.app_no and h.headerpk=l.headerfk and f.headerfk=h.headerpk and f.headerfk=l.headerfk and f.ledgerfk=l.ledgerpk  and transdate between '" + fromdate + "' and '" + todate + "' and receipttype='5' and ScholarshipType in('" + scholarShipType + "')  group by roll_no,reg_no,roll_admit,stud_name,degree_code,h.headername,ledgername,transdate,transcode,PayMode,DDNo,ScholarshipType";//and f.app_no='9911'
            }
            else
            {
                selQ = " select roll_no[Roll No],reg_no[Reg No],roll_admit[Admission No],stud_name[Name],(select distinct c.course_name+'-'+dt.dept_name from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=r.degree_code)[Department],h.headername[Header],ledgername[Ledger],convert(varchar(10),transdate,103) [Receipt Date],transcode[Receipt No],sum(debit)[Amount],PayMode[Paymode],DDNo[Neft No],ScholarshipType[Scholarship Type] from ft_findailytransaction f,registration r,fm_headermaster h,fm_ledgermaster l  where f.app_no=r.app_no and h.headerpk=l.headerfk and f.headerfk=h.headerpk and f.headerfk=l.headerfk and f.ledgerfk=l.ledgerpk  and transdate between '" + fromdate + "' and '" + todate + "' and receipttype='5' group by roll_no,reg_no,roll_admit,stud_name,degree_code,h.headername,ledgername,transdate,transcode,PayMode,DDNo,ScholarshipType";
            }
            DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                int rowCnt = 0;
                double total = 0;
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    string paymodeVal = Convert.ToString(dsVal.Tables[0].Rows[row]["Paymode"]);
                    string Paymode = "";
                    if (paymodeVal == "7")
                    {
                        Paymode = "Neft";
                    }
                    drRow = dtReport.NewRow();
                    drRow["Sno"] = Convert.ToString(++rowCnt);
                    drRow["Roll No"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Roll No"]);
                    drRow["Reg No"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Reg No"]);
                    drRow["Admission No"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Admission No"]);
                    drRow["Name"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Name"]);
                    drRow["Department"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Department"]);
                    drRow["Header"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Header"]);
                    drRow["Ledger"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Ledger"]);
                    drRow["Paymode"] = Convert.ToString(Paymode);
                    drRow["Neft No"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Neft No"]);
                    drRow["Scholarship Type"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Scholarship Type"]);
                    drRow["Receipt No"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Receipt No"]);
                    drRow["Receipt Date"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Receipt Date"]);
                    //double amt = 0;
                    double.TryParse(Convert.ToString(dsVal.Tables[0].Rows[row]["Amount"]), out total);
                    drRow["Amount"] = Convert.ToString(total);
                    dtReport.Rows.Add(drRow);
                }
            }
            if (dtReport.Rows.Count > 0)
            {
                gdReport.DataSource = dtReport;
                gdReport.DataBind();
                gdReport.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
        }
        catch { }
    }

    #region scholarship no generate

    public string generateScholarShipNo()
    {
        string collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
        int isHeaderwise = 0;
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
            Session["isHeaderwise"] = isHeaderwise;
        }
        catch { isHeaderwise = 0; }
        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 5)
                return string.Empty;
        }
        catch { return string.Empty; }
        return getCommonScholarShipNo(collegecode1);
    }

    private string getCommonScholarShipNo(string collegecode1)
    {
        string recno = string.Empty;
        //lblaccid.Text = "";
        //lstrcpt.Text = "";
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            //   lblaccid.Text = accountid;
            string secondreciptqurey = "SELECT ScholarshipStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables.Count > 0 && dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }
                string acronymquery = d2.GetFunction("SELECT ScholarshipAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;
                Session["acronym"] = recacr;
                int size = Convert.ToInt32(d2.GetFunction("SELECT  ScholarshipSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));
                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }

    #endregion

    protected void rblMode_Selected(object sender, EventArgs e)
    {
        tdentry.Visible = false;
        tdno.Visible = false;
        tdReport.Visible = true;
        gdReport.Visible = false;
        lbl_Scholarship.Visible = true;
        txt_Scholarship.Visible = true;
        Panel4.Visible = true;
        if (rblMode.SelectedIndex == 0)
        {
            tdentry.Visible = true;
            tdno.Visible = true;
            tdReport.Visible = false;
            lbl_Scholarship.Visible = false;
            txt_Scholarship.Visible = false;
            Panel4.Visible = false;
        }
    }

    //popup
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }

    protected void ddl_strm_OnIndexChange(object sender, EventArgs e)
    {
        binddegree2();
        bindbranch1();
        bindsec2();
    }

    public void bindType()
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = ddlcollege.SelectedItem.Value.ToString();
            }
            ddl_strm.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm.DataSource = ds;
                ddl_strm.DataTextField = "type";
                ddl_strm.DataValueField = "type";
                ddl_strm.DataBind();
            }
            if (ddl_strm.Items.Count > 0)
            {
                if (streamEnabled() == 1)
                    ddl_strm.Enabled = true;
                else
                    ddl_strm.Enabled = false;
            }
            else
                ddl_strm.Enabled = false;
        }
        catch
        { }
    }

    public void bindbatch1()
    {
        try
        {
            ddl_batch1.Items.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            ds = d2.select_method_wo_parameter(sqlyear, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch1.DataSource = ds;
                ddl_batch1.DataTextField = "batch_year";
                ddl_batch1.DataValueField = "batch_year";
                ddl_batch1.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    public void binddegree2()
    {
        try
        {
            ds.Clear();
            cbl_degree2.Items.Clear();
            string stream = "";
            stream = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : "";
            txt_degree2.Text = "--Select--";

            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            string colleges = Convert.ToString(d2.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode;
            }
            //string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegecode1 + ") and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + " ";
            string query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegecode + ") ";
            if (ddl_strm.Enabled)//if (txt_strm.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree2.DataSource = ds;
                cbl_degree2.DataTextField = "course_name";
                cbl_degree2.DataValueField = "course_id";
                cbl_degree2.DataBind();
                if (cbl_degree2.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree2.Items.Count; i++)
                    {
                        cbl_degree2.Items[i].Selected = true;
                    }
                    txt_degree2.Text = "Degree(" + cbl_degree2.Items.Count + ")";
                    cb_degree2.Checked = true;
                }
                else
                {
                    txt_degree2.Text = "--Select--";
                }
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }

        }
        catch (Exception ex) { }
    }

    public void bindbranch1()
    {
        try
        {
            cbl_branch1.Items.Clear();

            string branch = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        branch = branch + "'" + "," + "" + "'" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                }
            }
            string commname = "";
            if (branch != "")
            {
                //commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') ";
            }
            else
            {
                //commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code ";
            }
            if (branch.Trim() != "")
            {
                ds = d2.select_method_wo_parameter(commname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();



                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch1.Items.Count; i++)
                        {
                            cbl_branch1.Items[i].Selected = true;
                        }
                        txt_branch2.Text = "Branch(" + cbl_branch1.Items.Count + ")";
                        cb_branch1.Checked = true;
                    }
                }
                else
                {
                    txt_branch2.Text = "--Select--";
                }
            }
            else
            {
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex) { }
    }

    public void bindsec2()
    {
        try
        {
            cbl_sec2.Items.Clear();
            txt_sec2.Text = "--Select--";
            ListItem item = new ListItem("Empty", " ");
            if (ddl_batch1.Items.Count > 0)
            {
                string strbatch = Convert.ToString(ddl_batch1.SelectedItem.Value);
                string branch = "";
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    if (cbl_branch1.Items[i].Selected == true)
                    {
                        if (branch == "")
                        {
                            branch = "" + cbl_branch1.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            branch = branch + "" + "," + "" + "" + cbl_branch1.Items[i].Value.ToString() + "";
                        }
                    }
                }
                if (branch != "")
                {
                    DataSet dsSec = d2.BindSectionDetail(strbatch, branch);
                    if (dsSec.Tables.Count > 0)
                    {
                        if (dsSec.Tables[0].Rows.Count > 0)
                        {
                            cbl_sec2.DataSource = dsSec;
                            cbl_sec2.DataTextField = "sections";
                            cbl_sec2.DataValueField = "sections";
                            cbl_sec2.DataBind();


                        }
                    }
                    cbl_sec2.Items.Insert(0, item);
                    for (int i = 0; i < cbl_sec2.Items.Count; i++)
                    {
                        cbl_sec2.Items[i].Selected = true;
                    }
                    cb_sec2.Checked = true;
                    txt_sec2.Text = "Section(" + cbl_sec2.Items.Count + ")";

                }
            }


        }
        catch (Exception ex) { }
    }

    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddlcollege.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");
        bindsec2();
    }

    protected void cb_branch1_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");
        bindsec2();
    }

    protected void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        bindbranch1();
        bindsec2();
    }

    protected void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        bindbranch1();
        bindsec2();
    }

    protected void cb_sec2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_sec2, cb_sec2, txt_sec2, "Section");
    }

    protected void cbl_sec2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_sec2, cb_sec2, txt_sec2, "Section");
    }

    private double streamEnabled()
    {
        double strValue = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='JournalEnableStreamShift' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'")), out strValue);
        return strValue;
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

    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }

    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }

    protected void btn_roll_Click(object sender, EventArgs e)
    {
        popwindow.Visible = true;
        bindType();
        bindbatch1();
        binddegree2();
        bindbranch1();
        bindsec2();
        txt_rollno3.Text = "";
        //btn_go_Click(sender, e);
        btn_studOK.Visible = false;

        btn_exitstud.Visible = false;
        Fpspread1.Visible = false;
        lbl_errormsg.Visible = false;
    }

    protected void btn_studOK_Click(object sender, EventArgs e)
    {
        try
        {
            if (Fpspread1.Sheets[0].RowCount > 0)
            {
                Fpspread1.SaveChanges();
                string rollno = "";
                string rolladmit = "";
                string degreename1 = "";
                string name1 = "";
                string degreecode1 = "";
                string regno1 = "";
                string smartno = string.Empty;

                string actrow = Fpspread1.Sheets[0].ActiveRow.ToString();
                string actcol = Fpspread1.Sheets[0].ActiveColumn.ToString();
                StringBuilder sbRoll = new StringBuilder();
                if (actrow != "-1")
                {
                    for (int row = 1; row < Fpspread1.Sheets[0].RowCount; row++)
                    {
                        int value = 0;
                        int.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[row, 1].Value), out value);
                        if (value == 1)
                        {
                            rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 3].Text);
                            rolladmit = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 2].Text);
                            degreename1 = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 7].Text);
                            degreecode1 = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 7].Tag);
                            name1 = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 6].Text);
                            regno1 = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 4].Text);
                            smartno = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 5].Text);
                            sbRoll.Append(rollno + ",");
                            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                            {
                                //roll no
                                //  rollno = rollno;
                            }
                            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                            {
                                //reg no
                                // rollno = regno1;
                            }
                            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                            {
                                //Admin no
                                // rollno = rolladmit;
                            }
                        }
                    }

                }
                Fpspread1.Sheets[0].ActiveRow = -1;
                Fpspread1.Sheets[0].ActiveColumn = -1;
                Fpspread1.SaveChanges();
                txt_rollno.Text = Convert.ToString(rollno);
                //txt_rollno_Changed(sender, e);
                // Session["degreecodenew"] = Convert.ToString(degreecode1);
                //lbldisp.Text = "You have selected " + rowCnt + " " + selName + "";
                if (sbRoll.Length > 0)
                    sbRoll.Remove(sbRoll.Length - 1, 1);
                lbldisp.Text = string.Empty;
                lbldisp.Visible = false;
                lbldisp.Text = Convert.ToString(sbRoll);
                popwindow.Visible = false;
                txt_rollno_OnTextChanged(sender, e);
            }
        }
        catch (Exception ex) { }
    }

    protected void btn_exitstud_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }

    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread1.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "";
            Fpspread1.SaveChanges();
            string feecat = string.Empty;
            string ddlstream = Convert.ToString(ddl_strm.SelectedItem.Value);
            string batch = Convert.ToString(ddl_batch1.SelectedItem.Value);
            string degree = Convert.ToString(getCblSelectedValue(cbl_degree2));
            string branch = Convert.ToString(getCblSelectedValue(cbl_branch1));
            string sec = Convert.ToString(getCblSelectedValue(cbl_sec2));
            string selqry = " select r.app_no,r.Roll_No,r.Reg_No,r.roll_admit,r.Stud_Name,a.app_formno,r.batch_year,r.Current_Semester,r.sections,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,smart_serial_no from applyn a,Registration r,Degree d,Department dt,Course c where a.app_no =r.App_No and  r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and CC=0 and DelFlag=0 and Exam_Flag<>'debar'  and d.Degree_Code in ('" + branch + "') and r.Batch_Year='" + batch + "'  ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Fpspread1.Sheets[0].RowCount = 1;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = 8;

                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;


                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = " S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Columns[0].Width = 50;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 80;
                Fpspread1.Sheets[0].Columns[1].Locked = false;

                Fpspread1.Sheets[0].Cells[0, 1].CellType = chkall;
                Fpspread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Admission No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Columns[2].Locked = true;
                Fpspread1.Columns[2].Width = 130;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[3].Locked = true;
                Fpspread1.Columns[3].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[4].Locked = true;
                Fpspread1.Columns[4].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Smartcard No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[5].Locked = true;
                Fpspread1.Columns[5].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[6].Locked = true;
                Fpspread1.Columns[6].Width = 200;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Degree";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[7].Locked = true;
                Fpspread1.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Columns[7].Width = 270;

                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtSmartno = new FarPoint.Web.Spread.TextCellType();

                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    //roll no
                    Fpspread1.Sheets[0].Columns[3].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    //reg no
                    Fpspread1.Sheets[0].Columns[4].Visible = true;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    //Admin no
                    Fpspread1.Sheets[0].Columns[2].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                {
                    //Smartcard no
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    //if (smartDisp == 0)
                    //    Fpspread1.Sheets[0].Columns[3].Visible = true;
                    //else if (smartDisp == 1)
                    //    Fpspread1.Sheets[0].Columns[4].Visible = true;
                    //else if (smartDisp == 2 || smartDisp == 3)
                    //    Fpspread1.Sheets[0].Columns[2].Visible = true;
                }
                else
                {
                    //App no
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "App No";
                    Fpspread1.Sheets[0].Columns[2].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {

                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    //
                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = false;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = check;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    //
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].CellType = txtRollAd;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_Admit"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = txtRollno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = txtRegno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].CellType = txtSmartno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["smart_serial_no"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Degree"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                }
                Fpspread1.Visible = true;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                Fpspread1.Sheets[0].FrozenRowCount = 1;

                Fpspread1.SaveChanges();

                btn_studOK.Visible = true;
                btn_exitstud.Visible = true;
            }
            else
            {
                Fpspread1.Visible = false;
                lbl_errormsg.Visible = true;
                lbl_errormsg.Text = "No Records Found";
                btn_studOK.Visible = false;
                btn_exitstud.Visible = false;
            }

        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode, "ChallanReceipt");
        }
    }

    protected void gdReport_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //// percentage column visible true or false
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            #region
            if (roll == 0)
            {
                 e.Row.Cells[1].Visible = true;
                 e.Row.Cells[2].Visible = true;
                 e.Row.Cells[3].Visible = true;
            }
            else if (roll == 1)
            {
                 e.Row.Cells[1].Visible = true;
                 e.Row.Cells[2].Visible = true;
                 e.Row.Cells[3].Visible = true;
            }
            else if (roll == 2)
            {
                 e.Row.Cells[1].Visible = true;
                 e.Row.Cells[2].Visible = false;
                 e.Row.Cells[3].Visible = false;

            }
            else if (roll == 3)
            {
                 e.Row.Cells[1].Visible = false;
                 e.Row.Cells[2].Visible = true;
                 e.Row.Cells[3].Visible = false;
            }
            else if (roll == 4)
            {
                 e.Row.Cells[1].Visible = false;
                 e.Row.Cells[2].Visible = false;
                 e.Row.Cells[3].Visible = true;
            }
            else if (roll == 5)
            {
                 e.Row.Cells[1].Visible = true;
                 e.Row.Cells[2].Visible = true;
                 e.Row.Cells[3].Visible = false;
            }
            else if (roll == 6)
            {
                 e.Row.Cells[1].Visible = false;
                 e.Row.Cells[2].Visible = true;
                 e.Row.Cells[3].Visible = true;
            }
            else if (roll == 7)
            {
                 e.Row.Cells[1].Visible = true;
                 e.Row.Cells[2].Visible = false;
                 e.Row.Cells[3].Visible = true;
            }
            #endregion
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            #region
            if (roll == 0)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;
            }
            else if (roll == 1)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;
            }
            else if (roll == 2)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;

            }
            else if (roll == 3)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;
            }
            else if (roll == 4)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = true;
            }
            else if (roll == 5)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;
            }
            else if (roll == 6)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;
            }
            else if (roll == 7)
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = true;
            }
            #endregion
        }
    }

    #region Added by saranya on 2/8/2018   

    public void cb_Scholarship_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string SchlType = "";
            int cout = 0;
            txt_Scholarship.Text = "--Select--";
            if (cb_Scholarship.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_Scholarship.Items.Count; i++)
                {
                    cbl_Scholarship.Items[i].Selected = true;
                    SchlType = Convert.ToString(cbl_Scholarship.Items[i].Text);
                }
                if (cbl_Scholarship.Items.Count == 1)
                {
                    txt_Scholarship.Text = "" + SchlType + "";
                }
                else
                {
                    txt_Scholarship.Text = lbl_Scholarship.Text + "(" + (cbl_Scholarship.Items.Count) + ")";
                }
                // txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_Scholarship.Items.Count; i++)
                {
                    cbl_Scholarship.Items[i].Selected = false;
                }
                txt_Scholarship.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void cbl_Scholarship_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_Scholarship.Checked = false;
            txt_Scholarship.Text = "--Select--";
            string SchlType = "";
            for (int i = 0; i < cbl_Scholarship.Items.Count; i++)
            {
                if (cbl_Scholarship.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    SchlType = Convert.ToString(cbl_Scholarship.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_Scholarship.Items.Count)
                {

                    cb_Scholarship.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_Scholarship.Text = "" + SchlType + "";
                }
                else
                {
                    txt_Scholarship.Text = lbl_Scholarship.Text + "(" + commcount.ToString() + ")";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnplusMulSclReason_OnClick(object sender, EventArgs e)
    {
        imgdiv3.Visible = true;
        panel_description.Visible = true;
    }

    protected void btnminusMulSclReason_OnClick(object sender, EventArgs e)
    {
        if (ddl_MulSclReason.Items.Count > 0)
        {
            surediv.Visible = true;
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_erroralert.Text = "No Scholarship Type Selected";
        }
    }

    protected void btndescpopadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_description11.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','SchlolarshipReason','" + collegecode + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Saved sucessfully";
                    txt_description11.Text = "";
                    imgdiv3.Visible = false;
                    panel_description.Visible = false;
                }
                loaddesc1();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Enter the description";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btndescpopexit_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        panel_description.Visible = false;
        loaddesc1();
    }

    public void loaddesc1()
    {
        try
        {
            ddl_MulSclReason.Items.Clear();
            string query = " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode + "' order by MasterValue asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_MulSclReason.DataSource = ds;
                    ddl_MulSclReason.DataTextField = "MasterValue";
                    ddl_MulSclReason.DataValueField = "MasterCode";
                    ddl_MulSclReason.DataBind();

                    cbl_Scholarship.DataSource = ds;
                    cbl_Scholarship.DataTextField = "MasterValue";
                    cbl_Scholarship.DataValueField = "MasterCode";
                    cbl_Scholarship.DataBind();
                }
            }
        }
        catch { }
    }

    protected void btnerrexit_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = false;
            if (ddl_MulSclReason.Items.Count > 0)
            {

                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_MulSclReason.SelectedItem.Value.ToString() + "' and MasterCriteria ='SchlolarshipReason' and CollegeCode='" + collegecode + "' ";
                int delete = d2.update_method_wo_parameter(sql, "TEXT");
                if (delete != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Deleted Sucessfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Not deleted";
                }
                loaddesc1();
            }

            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "No Scholarship Type Selected";
            }
        }
        catch { }
    }

    #endregion
}