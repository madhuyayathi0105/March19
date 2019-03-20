using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.IO;
using System.Drawing;
public partial class LetterDocumentInward : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string date = DateTime.Now.ToString("dd/MM/yyyy");
    int type;
    static string filepk = "";
    string code = "";
    string code1 = "";
    string codestaff1 = "";
    string codestaff2 = "";
    string staff_code = "";
    string staff_code1 = "";

    string staff_indi_code = "";
    string staff_indi_code1 = "";
    string parent_dept_code = "";
    string parent_dept_code1 = "";
    string parent_indi_code = "";
    string parent_indi_code1 = "";
    string company_dept_code = "";
    string company_dept_code1 = "";
    string company_indi_code = "";
    string company_indi_code1 = "";
    string individual_dept_code = "";
    string individual_dept_code1 = "";
    string individual_staff_code = "";
    string individual_staff_code1 = "";
    string mag_dept_code = "";
    string mag_dept_code1 = "";
    string mag_staff_code = "";
    string mag_staff_code1 = "";
    string other_dept_code = "";
    string other_dept_code1 = "";
    string other_staff_code = "";
    string other_staff_code1 = "";
    string parent_stud_code = "";
    string uploadvalue = "";
    string uploadtime = "";
    string mtype = "";
    string mcode = "";
    string mstaffcode = "";
    string mcode_par = "";
    string mcode_comp = "";
    string mcode_indi = "";
    string mcode_mag = "";
    string mcode_othr = "";
    static string oldcurrentcheck = "";
    int value;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    static Hashtable newhash = new Hashtable();
    static Hashtable newhashtbl = new Hashtable();
    static Hashtable studhash = new Hashtable();
    static Hashtable depthash = new Hashtable();
    static Hashtable staffhash = new Hashtable();
    Hashtable ht = new Hashtable();
    Hashtable hat = new Hashtable();
    Boolean fpcellclick = false;
    static string hdsave = "";
    static string ledsave = "";
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());   

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
        collegecode1 = Session["collegecode"].ToString();
        if (!IsPostBack)
        {
            BindCollege();
            loaddoc();
            loaddelivery();
            doc();
            fileuploads();
            delivery();
            dept();
            staffcodevalue();
            access();
            access1();
            bankbind();
            div_college.Attributes.Add("style", "display:none");
            //  div_cheque.Attributes.Add("style", "display:block");
            //txt_time.Text = DateTime.Now.ToLongTimeString();
            //txt_time.Attributes.Add("readonly", "readonly");
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_date.Attributes.Add("readonly", "readonly");
            txt_staff_chequedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_staff_chequedate.Attributes.Add("readonly", "readonly");
            txt_staff_dddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_staff_dddate.Attributes.Add("readonly", "readonly");
            txt_par_chequedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_par_chequedate.Attributes.Add("readonly", "readonly");
            txt_pardddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_pardddate.Attributes.Add("readonly", "readonly");
            txt_comp_chqdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_comp_chqdate.Attributes.Add("readonly", "readonly");
            txt_comp_dddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_comp_dddate.Attributes.Add("readonly", "readonly");
            txt_indi_chqdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_indi_chqdate.Attributes.Add("readonly", "readonly");
            txt_indidddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_indidddate.Attributes.Add("readonly", "readonly");
            txt_mag_chqdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_mag_chqdate.Attributes.Add("readonly", "readonly");
            txt_magdd_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_magdd_date.Attributes.Add("readonly", "readonly");
            txt_othr_chqdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_othr_chqdate.Attributes.Add("readonly", "readonly");
            txt_othr_dddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_othr_dddate.Attributes.Add("readonly", "readonly");
            oldcurrentcheck = "";
            generateReceiptNo();
            generateReceiptNo1();
            //generateReceiptNo2();
            //generateReceiptNo3();
            //generateReceiptNo4();
            txt_chequedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_chequedate.Attributes.Add("readonly", "readonly");
            txt_dddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_dddate.Attributes.Add("readonly", "readonly");
            txt_stf_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_stf_date.Attributes.Add("readonly", "readonly");
            //  txt_stf_time.Text = DateTime.Now.ToLongTimeString();
            txt_pardate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_pardate.Attributes.Add("readonly", "readonly");
            // txt_partime.Text = DateTime.Now.ToLongTimeString();
            // txt_partime.Attributes.Add("readonly", "readonly");
            txt_com_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_com_date.Attributes.Add("readonly", "readonly");
            // txt_com_time.Text = DateTime.Now.ToLongTimeString();
            //txt_com_time.Attributes.Add("readonly", "readonly");
            txt_ind_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_ind_date.Attributes.Add("readonly", "readonly");
            // txt_ind_time.Text = DateTime.Now.ToLongTimeString();
            // txt_ind_time.Attributes.Add("readonly", "readonly");
            txt_othrdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_othrdate.Attributes.Add("readonly", "readonly");
            // txt_othrtime.Text = DateTime.Now.ToLongTimeString();
            // txt_othrtime.Attributes.Add("readonly", "readonly");
            txt_magzdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_magzdate.Attributes.Add("readonly", "readonly");
            // txt_magztime.Text = DateTime.Now.ToLongTimeString();
            // txt_magztime.Attributes.Add("readonly", "readonly");
            rdo_current.Checked = true;
            rdo_parent_current.Checked = true;
            rb_in.Checked = true;
            cb_dept.Checked = true;
            div_dept.Attributes.Add("style", "display:block");
            cb_stf_dept.Checked = true;
            div_stf_dept.Attributes.Add("style", "display:block");
            cb_par_dept.Checked = true;
            div_parnt_dept.Attributes.Add("style", "display:block");
            cb_com_dept.Checked = true;
            div_comp_dept.Attributes.Add("style", "display:block");
            cb_ind_dept.Checked = true;
            div_indiv_dept.Attributes.Add("style", "display:block");
            cb_magz_dept.Checked = true;
            div_magz_dept.Attributes.Add("style", "display:block");
            cb_othr_dept.Checked = true;
            div_other_dept.Attributes.Add("style", "display:block");
            // imgdiv2.Attributes.Add("style", "display:none");
            imgdiv2.Visible = false;
            txt_batch.Attributes.Add("readonly", "readonly");
            txt_degree.Attributes.Add("readonly", "readonly");
            txt_department.Attributes.Add("readonly", "readonly");
            txt_sem.Attributes.Add("readonly", "readonly");
            txt_studtype.Attributes.Add("readonly", "readonly");
            txt_addrs.Attributes.Add("readonly", "readonly");
            txt_street.Attributes.Add("readonly", "readonly");
            txt_city.Attributes.Add("readonly", "readonly");
            txt_state.Attributes.Add("readonly", "readonly");
            txt_pho.Attributes.Add("readonly", "readonly");
            txt_mailid.Attributes.Add("readonly", "readonly");
            txt_section.Attributes.Add("readonly", "readonly");
            txt_dept.Attributes.Add("readonly", "readonly");
            txt_desigtn.Attributes.Add("readonly", "readonly");
            txt_stafftype.Attributes.Add("readonly", "readonly");
            txt_stf_add.Attributes.Add("readonly", "readonly");
            txt_stf_str.Attributes.Add("readonly", "readonly");
            txt_stf_country.Attributes.Add("readonly", "readonly");
            txt_stf_state.Attributes.Add("readonly", "readonly");
            txt_stf_ph.Attributes.Add("readonly", "readonly");
            txt_stf_mail.Attributes.Add("readonly", "readonly");
            txt_stf_city.Attributes.Add("readonly", "readonly");
            txt_stf_date.Attributes.Add("readonly", "readonly");
            //txt_stf_time.Attributes.Add("readonly", "readonly");
            txt_addr.Attributes.Add("readonly", "readonly");
            txt_street2.Attributes.Add("readonly", "readonly");
            txt_par_city.Attributes.Add("readonly", "readonly");
            txt_par_state.Attributes.Add("readonly", "readonly");
            txt_par_country.Attributes.Add("readonly", "readonly");
            txt_par_phno.Attributes.Add("readonly", "readonly");
            txt_par_mail.Attributes.Add("readonly", "readonly");
            //txt_des.Attributes.Add("readonly", "readonly");
            //txt_com_add.Attributes.Add("readonly", "readonly");
            //txt_com_str.Attributes.Add("readonly", "readonly");
            //txt_com_city.Attributes.Add("readonly", "readonly");
            //txt_com_state.Attributes.Add("readonly", "readonly");
            //txt_com_county.Attributes.Add("readonly", "readonly");
            //txt_com_ph.Attributes.Add("readonly", "readonly");
            //txt_com_mail.Attributes.Add("readonly", "readonly");
            ddl_staff_bank.Items.Insert(ddl_staff_bank.Items.Count, "Others");
            txt_bank_comp.Items.Insert(txt_bank_comp.Items.Count, "Others");
            ddl_bank_stud.Items.Insert(ddl_bank_stud.Items.Count, "Others");
            ddl_othr_bank.Items.Insert(ddl_othr_bank.Items.Count, "Others");
            DataSet dsprint = new DataSet();
            string group_code = "", columnfield = "";
            //Bind College=====================================================
            group_code = Session["group_code"].ToString();
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
            dsprint = d2.select_method("bind_college", hat, "sp");

            ddlcollegenew.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {

                ddlcollegenew.DataSource = dsprint;
                ddlcollegenew.DataTextField = "collname";
                ddlcollegenew.DataValueField = "college_code";
                ddlcollegenew.DataBind();
                ddlcollegenew_SelectedIndexChanged(sender, e);
            }

            bindcourse();
            bindBranch();
            bindBatch1();
            ddlheader.Items.Clear();
            ddlheader.Items.Add("---Select---");
            ddlheader.Items.Add("Roll No");
            ddlheader.Items.Add("Reg No");
            ddlheader.Items.Add("Name");
            ddloperator.Items.Clear();
            ddloperator.Items.Add("---Select---");
            ddloperator.Items.Add("Like");
            ddloperator.Items.Add("Starts With");
            ddloperator.Items.Add("Ends With");

            studhash.Clear();
            newhash.Clear();
            newhashtbl.Clear();
            lbl_er.Visible = false;
            loadhour();
            loadsecond();
            loadminits();
            timevalue();
        }
    }

    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    public void loadhour()
    {
        try
        {
            ddl_hour.Items.Clear();
            ddl_stfhr.Items.Clear();
            ddl_parhr.Items.Clear();
            ddl_comhr.Items.Clear();
            ddl_indhr.Items.Clear();
            ddl_maghr.Items.Clear();
            ddl_othrhr.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                ddl_hour.Items.Add(Convert.ToString(i));
                ddl_stfhr.Items.Add(Convert.ToString(i));
                ddl_parhr.Items.Add(Convert.ToString(i));
                ddl_comhr.Items.Add(Convert.ToString(i));
                ddl_indhr.Items.Add(Convert.ToString(i));
                ddl_maghr.Items.Add(Convert.ToString(i));
                ddl_othrhr.Items.Add(Convert.ToString(i));
                ddl_hour.SelectedIndex = ddl_hour.Items.Count - 1;
                ddl_stfhr.SelectedIndex = ddl_hour.Items.Count - 1;
                ddl_parhr.SelectedIndex = ddl_hour.Items.Count - 1;
                ddl_comhr.SelectedIndex = ddl_hour.Items.Count - 1;
                ddl_indhr.SelectedIndex = ddl_hour.Items.Count - 1;
                ddl_maghr.SelectedIndex = ddl_hour.Items.Count - 1;
                ddl_othrhr.SelectedIndex = ddl_hour.Items.Count - 1;
            }
        }
        catch
        {
        }
    }

    public void loadsecond()
    {
        ddl_seconds.Items.Clear();
        for (int i = 0; i <= 60; i++)
        {
            string value = Convert.ToString(i);
            if (value.Length == 1)
            {
                value = "0" + "" + value;
            }
            ddl_seconds.Items.Add(Convert.ToString(value));
        }
    }

    public void loadminits()
    {
        ddl_minits.Items.Clear();
        ddl_stfm.Items.Clear();
        ddl_parm.Items.Clear();
        ddl_commi.Items.Clear();
        ddlindmin.Items.Clear();
        ddl_magmin.Items.Clear();
        ddl_othrmin.Items.Clear();
        for (int i = 0; i <= 59; i++)
        {
            string value = Convert.ToString(i);
            if (value.Length == 1)
            {
                value = "0" + "" + value;
            }
            ddl_minits.Items.Add(Convert.ToString(value));
            ddl_stfm.Items.Add(Convert.ToString(value));
            ddl_parm.Items.Add(Convert.ToString(value));
            ddl_commi.Items.Add(Convert.ToString(value));
            ddlindmin.Items.Add(Convert.ToString(value));
            ddl_magmin.Items.Add(Convert.ToString(value));
            ddl_othrmin.Items.Add(Convert.ToString(value));
        }
    }

    public void imgbtn_stud_Click(object sender, EventArgs e)
    {
        // generateReceiptNo1();
    }

    public void imgbtn_staff_Click(object sender, EventArgs e)
    {
        //  generateReceiptNo1();
    }

    public void imgbtn_parents_Click(object sender, EventArgs e)
    {
    }

    public void imgbtn_company_Click(object sender, EventArgs e)
    {
        // generateReceiptNo1();
    }

    public void imgbtn_individual_Click(object sender, EventArgs e)
    {
    }

    public void imgbtn_magazine_Click(object sender, EventArgs e)
    {
    }

    public void imgbtn_others_Click(object sender, EventArgs e)//delsiref
    {
        //spread.Visible = false;
        //Fpspread9.Visible = false;
        //ScriptManager.RegisterStartupScript(this, GetType(), "imgbtn_others", "otherscheck();", true);
        //  generateReceiptNo1();
    }

    public void txt_time_TextChanged(object sender, EventArgs e)
    {
    }

    public void cb_dept_CheckedChanged(object sender, EventArgs e)
    {

    }

    public void cb_indi_CheckedChanged(object sender, EventArgs e)
    {
    }

    public void ddl_docpriv_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            spread.Visible = false;
            Fpspread9.Visible = false;
            string doc_prty = "";
            string delivery_mode = "";
            dept();
            staffcodevalue();
            // timevalue();
            div_stud.Attributes.Add("style", "display:block");
            div_staff.Attributes.Add("style", "display:none");
            div_company.Attributes.Add("style", "display:none");
            div_individu.Attributes.Add("style", "display:none");
            div_magazine.Attributes.Add("style", "display:none");
            div_others.Attributes.Add("style", "display:none");
            div_parent.Attributes.Add("style", "display:none");
            //div_dept.Attributes.Add("style", "display:block");
            //div_indiv.Attributes.Add("style", "display:block");
            // txt_docpriv.Attributes.Add("style", "display:block");
            // txt_delivery.Attributes.Add("style", "display:block");
            if (ddl_docpriv.SelectedItem.Value != "Select")
            {
                if (ddl_docpriv.SelectedItem.Value != "Others")
                {
                    doc_prty = Convert.ToString(ddl_docpriv.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_docpriv.Text);
                    doc_prty = subjectcodenew("TAPri", doc_prty1);
                }
            }
            if (ddl_delivery.SelectedItem.Value != "Select")
            {
                if (ddl_delivery.SelectedItem.Value != "Others")
                {
                    delivery_mode = Convert.ToString(ddl_delivery.SelectedItem.Value);
                }
                else
                {
                    string delivery_mode1 = Convert.ToString(txt_delivery.Text);
                    delivery_mode = subjectcodenew("bankN", delivery_mode1);
                }
            }

            if (ddl_stud_doc.SelectedItem.Value != "Select")
            {
                if (ddl_stud_doc.SelectedItem.Value != "Others")
                {
                    doc_prty = Convert.ToString(ddl_stud_doc.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_stud_doc_others.Text);
                    doc_prty = subjectcodenew("TAPri", doc_prty1);
                }
            }

            if (ddl_stud_delivery.SelectedItem.Value != "Select")
            {
                if (ddl_stud_delivery.SelectedItem.Value != "Others")
                {
                    delivery_mode = Convert.ToString(ddl_stud_delivery.SelectedItem.Value);
                }
                else
                {
                    string delivery_mode1 = Convert.ToString(txt_stud_delivery.Text);
                    delivery_mode = subjectcodenew("bankN", delivery_mode1);
                }
            }

            //if (ddl_staff_bank.SelectedItem.Value != "Select")
            //{

            //    if (ddl_staff_bank.SelectedItem.Value != "Others")
            //    {
            //        delivery_mode = Convert.ToString(ddl_staff_bank.SelectedItem.Value);
            //    }
            //    else
            //    {
            //       string delivery_mode1 = Convert.ToString(txt_bank.Text);
            //        delivery_mode = subjectcodenew("TASta", delivery_mode1);
            //    }


            //}    
            bool rcptCheck = false;
            if (rb_in.Checked == true)
            {
                type = 0;
            }
            else
            {
                type = 1;
            }
            int menutype = 1;
            string[] ay = txt_date.Text.Split('/');
            string date = ay[1].ToString() + "/" + ay[0].ToString() + "/" + ay[2].ToString();
            if (cb_dept.Checked == true && cb_individual.Checked == true)
            {
                value = 3;
                div_dept.Attributes.Add("style", "display:block");
                div_indiv.Attributes.Add("style", "display:block");
            }
            else if (cb_individual.Checked == true)
            {
                value = 2;
                div_indiv.Attributes.Add("style", "display:block");
            }
            else if (cb_dept.Checked == true)
            {
                value = 1;
                div_dept.Attributes.Add("style", "display:block");
            }
            string department_code1 = Convert.ToString(txt_dept_to.Text);
            string department_code2 = Convert.ToString(txt_to1.Text);
            string department_code3 = Convert.ToString(txt_dept_cc.Text);
            string department_code4 = Convert.ToString(txt_cc1.Text);
            string staff_code1 = Convert.ToString(txt_indiv.Text);
            string[] split = staff_code1.Split('-');
            string namesplit = split[0];
            string staff_code2 = Convert.ToString(txt_indiv1.Text);
            string[] split1 = staff_code2.Split('-');
            string namesplit1 = split1[0];
            string staff_code3 = Convert.ToString(txt_indiv_cc.Text);
            string[] split2 = staff_code3.Split('-');
            string namesplit2 = split2[0];
            string staff_code4 = Convert.ToString(txt_cc2.Text);
            string[] split3 = staff_code4.Split('-');
            string namesplit3 = split3[0];
            string deptcode1 = Convert.ToString(depthash[Convert.ToString(department_code1)]);
            if (deptcode1.Trim() != "")
            {
                if (code == "")
                {
                    code = deptcode1;
                }
                else
                {
                    code = code + "," + deptcode1;
                }
            }
            string deptcode2 = Convert.ToString(depthash[Convert.ToString(department_code2)]);
            if (deptcode2.Trim() != "")
            {
                if (code == "")
                {
                    code = deptcode2;
                }
                else
                {
                    code = code + "," + deptcode2;
                }
            }
            string deptcode3 = Convert.ToString(depthash[Convert.ToString(department_code3)]);
            if (deptcode3.Trim() != "")
            {
                if (code1 == "")
                {
                    code1 = deptcode3;
                }
                else
                {
                    code1 = code1 + "," + deptcode3;
                }
            }
            string deptcode4 = Convert.ToString(depthash[Convert.ToString(department_code4)]);
            if (deptcode4.Trim() != "")
            {
                if (code == "")
                {
                    code1 = deptcode4;
                }
                else
                {
                    code1 = code1 + "," + deptcode4;
                }
            }
            string staffcode11 = Convert.ToString(staffhash[Convert.ToString(namesplit)]);
            if (staffcode11.Trim() != "")
            {
                if (codestaff1 == "")
                {
                    codestaff1 = staffcode11;
                }
                else
                {
                    codestaff1 = codestaff1 + "," + staffcode11;
                }
            }
            string staffcode22 = Convert.ToString(staffhash[Convert.ToString(namesplit1)]);
            if (staffcode22.Trim() != "")
            {
                if (codestaff1 == "")
                {
                    codestaff1 = staffcode22;
                }
                else
                {
                    codestaff1 = codestaff1 + "," + staffcode22;
                }
            }
            string staffcode33 = Convert.ToString(staffhash[Convert.ToString(namesplit2)]);
            if (staffcode33.Trim() != "")
            {
                if (codestaff2 == "")
                {
                    codestaff2 = staffcode33;
                }
                else
                {
                    codestaff2 = codestaff2 + "," + staffcode33;
                }
            }
            string staffcode44 = Convert.ToString(staffhash[Convert.ToString(namesplit3)]);
            if (staffcode44.Trim() != "")
            {
                if (codestaff2 == "")
                {
                    codestaff2 = staffcode44;
                }
                else
                {
                    codestaff2 = codestaff2 + "," + staffcode44;
                }
            }
            //added by kowshika
            string tapalcode = "";
            if (txt_rcptno.Text.Trim() != "")
            {

                tapalcode = txt_rcptno.Text;

            }
            DateTime dt = new DateTime();
            string rollno = "";
            string appno = "";
            if (rdo_old.Checked == true)
            {
                rollno = Convert.ToString(txt_roll.Text);
                string appno1 = "select app_no , roll_no from Registration where Reg_No ='" + rollno + "'";
                DataSet dne = d2.select_method_wo_parameter(appno1, "Text");
                if (dne.Tables[0].Rows.Count > 0)
                {
                    appno = Convert.ToString(dne.Tables[0].Rows[0]["app_no"]);
                    rollno = Convert.ToString(dne.Tables[0].Rows[0]["roll_no"]);
                }
            }
            else
            {
                rollno = Convert.ToString(txt_roll.Text);
                appno = d2.GetFunction("select app_no  from Registration where roll_no ='" + rollno + "'");
            }
            uploadvalue = appno;
            // string subject = Convert.ToString(txt_subject.Text);
            // string refno = Convert.ToString(ttx_refno.Text);
            string subject = Convert.ToString(txt_stud_sub.Text);
            string refno = Convert.ToString(txt_stud_ref.Text);

            string getday = dt.ToString("MM/dd/yyyy");
            string hr = Convert.ToString(ddl_othrhr.SelectedItem.Text);
            string min = Convert.ToString(ddl_othrmin.SelectedItem.Text);
            string day = Convert.ToString(ddl_othram.SelectedItem.Text);
            string time = hr + ":" + min + ":" + day;
            string chequeckddname = "";
            string chequeckddno = "";
            string chequeckddamt = "";
            string[] ay1;
            string date1 = "";
            string paymenttype = "";
            string payval = "";
            string bank = "";
            string b = "";
            int amount = 0;
            if (cb_paymentstud.Checked == true)
            {
                payval = "1";
                bank = Convert.ToString(ddl_stud_bank.SelectedItem.Value);
                b = Convert.ToString(txt_bankbranch.Text);
                if (rdo_cheque.Checked == true)
                {
                    paymenttype = "0";
                    chequeckddname = Convert.ToString(txt_chequename.Text);
                    chequeckddno = Convert.ToString(txt_chequenumb.Text);
                    chequeckddamt = Convert.ToString(txt_chequeamt.Text);
                    ay1 = txt_chequedate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
                else
                {
                    paymenttype = "1";
                    chequeckddname = Convert.ToString(txt_ddname.Text);
                    chequeckddno = Convert.ToString(txt_ddno.Text);
                    chequeckddamt = Convert.ToString(txt_ddamt.Text);
                    ay1 = txt_dddate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
            }
            else
            {
                payval = "0";
            }

            if (chk_stud_charges.Checked == true)
            {
                payval = "1";
                bank = Convert.ToString(ddl_bank_stud.SelectedItem.Value);
                b = Convert.ToString(txt_stud_bankbranch.Text);
                if (rdo_stud_cheque.Checked == true)
                {
                    paymenttype = "0";
                    chequeckddname = Convert.ToString(txt_stud_chqname.Text);
                    chequeckddno = Convert.ToString(txt_stud_chqno.Text);
                    chequeckddamt = Convert.ToString(txt_stud_chqamt.Text);

                    int.TryParse(Convert.ToString(chequeckddamt), out amount);

                    ay1 = txt_stud_chqdate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
                else
                {
                    paymenttype = "1";
                    chequeckddname = Convert.ToString(txt_stud_chqname.Text);
                    chequeckddno = Convert.ToString(txt_stud_chqno.Text);
                    chequeckddamt = Convert.ToString(txt_stud_chqamt.Text);
                    ay1 = txt_stud_chqdate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
            }
            else
            {
                payval = "0";
            }

            // uploadtime = time;
            mtype = "1";
            mcode = uploadvalue;
            //string dept_code = d2.GetFunction("select Dept_Code as DeptName from Department where Dept_name='" + txt_to1.Text + "'");
            if (cb_dept.Checked == false && cb_individual.Checked == false)
            {
                //lbl_er.Visible = true;
                lblalerterr.Text = "Kindly select department and individual fields or Anyone";
                imgdiv2.Visible = true;
            }
            else
            {
                //if (code == "")
                //{
                //    lblalerterr.Text = "Kindly select correct  name";
                //    imgdiv2.Visible = true;
                //}
                //else
                //{
                if ((ddl_stud_doc.SelectedItem.Value == "Others" && txt_stud_doc_others.Text.Trim() != "") || (ddl_stud_doc.SelectedItem.Value != "Others"))
                {
                    if ((ddl_stud_delivery.SelectedItem.Value == "Others" && txt_stud_delivery.Text.Trim() != "") || (ddl_stud_delivery.SelectedItem.Value != "Others"))
                    {
                        if (txt_roll.Text != "" && txt_name.Text != "")
                        {
                            string check = "";
                            if (rdo_old.Checked == true)
                            {
                                check = d2.GetFunction("select r.Reg_No,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and CC=1 and DelFlag =0 and Exam_Flag <>'DEBAR'  and r.Reg_No='" + txt_roll.Text + "'");
                            }
                            else
                            {
                                check = d2.GetFunction("select  r.Reg_No from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.Roll_no='" + txt_roll.Text + "'");
                            }
                            if (check != "0")
                            {
                                lbl_er.Visible = false;
                                string query = "";
                                if (cb_paymentstud.Checked == true)
                                {
                                    query = "insert into TapalInward (MemType,App_no,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,type,ViewStatus,SendNote,ispayment,DDNo,DDName,DDDate,Paymentamount,PaymentType,BankCode,BranchName,usercode,TapalCode)values('" + menutype + "','" + appno + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + code + "','" + code1 + "','" + codestaff1 + "','" + codestaff2 + "','" + type + "','0','0','" + payval + "','" + chequeckddno + "','" + chequeckddname + "','" + date1 + "','" + chequeckddamt + "','" + paymenttype + "','" + bank + "','" + b + "','" + usercode + "','" + tapalcode + "')";//delsi2602
                                    //Modified by kowshi 7.5.2018
                                    d2.update_method_wo_parameter(query, "Text");
                                }
                                else
                                {
                                    query = "insert into TapalInward (MemType,App_no,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,type,ViewStatus,SendNote,ispayment,usercode,TapalCode)values('" + menutype + "','" + appno + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + code + "','" + code1 + "','" + codestaff1 + "','" + codestaff2 + "','" + type + "','0','0','" + payval + "','" + usercode + "','" + tapalcode + "')";
                                    d2.update_method_wo_parameter(query, "Text");

                                }
                                filepk = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                                upload();
                                uploadstud();
                                string rep_no = generateReceiptNo();
                                string charge = Convert.ToString(txt_stud_charges.Text);
                                if (charge != "")
                                {
                                    if (hdsave != "" && ledsave != "")
                                    {
                                        string finYearid = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + collegecode1 + "");
                                        string ap_no = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                                        string charges_query = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,TransType,FinYearFK) values('" + date + "','" + time + "','" + rep_no + "','7','" + ap_no + "','" + hdsave + "','" + ledsave + "','0','" + charge + "','0','1','2','" + finYearid + "')";
                                        int save1 = d2.update_method_wo_parameter(charges_query, "Text");
                                        #region Update Receipt No
                                        string lastRecptNo = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                                        lastRecptNo = rep_no.Replace(lastRecptNo, "");
                                        string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
                                        d2.update_method_wo_parameter(updateRecpt, "Text");
                                        #endregion
                                        if (save1 != 0)
                                        {
                                            imgdiv2.Visible = true;
                                            lblalerterr.Text = "Saved Successfully";

                                        }
                                    }
                                    else
                                    {
                                        imgdiv2.Visible = true;
                                        lblalerterr.Text = "Kindly Select Header & Ledger";
                                    }
                                }
                                else
                                {
                                    imgdiv2.Visible = true;
                                    lblalerterr.Text = "Saved Successfully";
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lblalerterr.Text = "Kindly Select Correct Roll No Or Reg No";
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lblalerterr.Text = "Kindly Select Correct Roll No Or Reg No";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "Fill The Delivery Mode";
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Fill The Document Priority";
                }
            }
            loaddoc();
            loaddelivery();
            delivery();
            doc();
            rb_out.Checked = false;
            rb_in.Checked = true;
            rdo_current.Checked = true;
            rdo_old.Checked = false;
            txt_chargestud.Text = "";
            // ddl_docpriv.Items.Insert(0, new ListItem("Select", "0"));
            //imgdiv2.Attributes.Add("style", "display:block");
            td_stud.BgColor = "#c4c4c4";
            td_indi.BgColor = "white";
            td_mag.BgColor = "white";
            td_staff.BgColor = "white";
            td_othr.BgColor = "white";
            td_comp.BgColor = "white";
            td_par.BgColor = "white";
            cb_individual.Checked = false;
            div_indiv.Attributes.Add("style", "display:none");
            div_college.Attributes.Add("style", "display:block");
        }
        catch
        {
        }
    }

    public string subjectcode(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode1 + " and TextVal='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
            }
            else
            {
                string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode1 + " and TextVal='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                    }
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }

    public string subjectcodenew(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }

    protected void rdo_challan_CheckedChanged(object sender, EventArgs e)
    {


    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        if (oldcurrentcheck == "O")
        {
            query = "select Reg_no from Registration where CC=1 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
        }
        else
        {
            query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
        }
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getreg(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        if (oldcurrentcheck == "O")
        {
            query = "select Reg_no from Registration where CC=1 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_no like '" + prefixText + "%'";
        }
        else
        {
            query = "select Reg_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_no like '" + prefixText + "%'";
        }
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";
        studhash = ws.Getnamevalue(query);
        if (studhash.Count > 0)
        {
            foreach (DictionaryEntry p in studhash)
            {
                string studname = Convert.ToString(p.Key);
                name.Add(studname);
            }
        }
        // name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getdept(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        newhash.Clear();
        string query = "select Dept_Name as DeptName,Dept_Code from Department where Dept_Name like '" + prefixText + "%'";
        //newhash = ws.Getnamevalue(query);
        if (newhash.Count > 0)
        {
            foreach (DictionaryEntry p in newhash)
            {
                string deptname = Convert.ToString(p.Key);
                name.Add(deptname);
            }
        }
        name = ws.Getname(query);
        return name;
    }

    [WebMethod]
    public static Student[] getData(string Roll_No)
    {
        string data = string.Empty;
        List<Student> details = new List<Student>();
        try
        {
            DataSet ds1 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string query = "";
            if (oldcurrentcheck == "O")
            {
                query = "select r.Reg_No, a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and CC=1 and DelFlag =0 and Exam_Flag <>'DEBAR'  and r.Roll_no='" + Roll_No + "'";
            }
            else
            {
                query = "select  r.Reg_No, a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.Roll_no='" + Roll_No + "'";
            }
            //string query = "select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,a.parent_name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.roll_no='" + Roll_No + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    Student s = new Student();
                    if (oldcurrentcheck == "O")
                    {
                        s.RollNo = ds1.Tables[0].Rows[i]["Reg_No"].ToString();
                    }
                    else
                    {
                        s.RollNo = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                    }
                    //s.Name = ds1.Tables[0].Rows[i]["Stud_Name"].ToString();
                    s.Name = ds1.Tables[0].Rows[i]["stud_name"].ToString();
                    //+"-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    s.Batchyear = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                    s.Degree = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                    s.Department = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    s.section = ds1.Tables[0].Rows[i]["Sections"].ToString();
                    s.Semester = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                    s.StudentType = ds1.Tables[0].Rows[i]["Stud_Type"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["parent_addressP"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["Streetp"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["Cityp"].ToString();
                    s.Pin = ds1.Tables[0].Rows[i]["parent_pincodec"].ToString();
                    s.State = ds1.Tables[0].Rows[i]["State"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["Student_Mobile"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["StuPer_Id"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }

    [WebMethod]
    public static Student[] getDatareg(string Reg_No)//barath 19.04.17
    {
        string data = string.Empty;
        List<Student> details = new List<Student>();
        try
        {
            DataSet ds1 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string query = "";
            if (oldcurrentcheck == "O")
            {
                query = "select r.Reg_No, a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and CC=1 and DelFlag =0 and Exam_Flag <>'DEBAR'  and r.Reg_No='" + Reg_No + "'";
            }
            else
            {
                query = "select  r.Reg_No, a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.Reg_No='" + Reg_No + "'";
            }
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)//delsi2602
                {
                    Student s = new Student();
                    if (oldcurrentcheck == "O")
                    {
                        s.RollNo = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                    }
                    else
                    {
                        s.RollNo = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                    }
                    //s.Name = ds1.Tables[0].Rows[i]["Stud_Name"].ToString();
                    s.Name = ds1.Tables[0].Rows[i]["stud_name"].ToString();
                    //+"-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    s.Batchyear = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();

                    s.Degree = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                    s.Department = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    s.section = ds1.Tables[0].Rows[i]["Sections"].ToString();
                    s.Semester = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                    s.StudentType = ds1.Tables[0].Rows[i]["Stud_Type"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["parent_addressP"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["Streetp"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["Cityp"].ToString();
                    s.Pin = ds1.Tables[0].Rows[i]["parent_pincodec"].ToString();
                    s.State = ds1.Tables[0].Rows[i]["State"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["Student_Mobile"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["StuPer_Id"].ToString();
                    details.Add(s);
                }

            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }

    [WebMethod]
    public static string getDatanewcheck(string Roll_No)
    {
        string data = "";
        try
        {
            if (Roll_No == "C")
            {
                oldcurrentcheck = "C";
            }
            else
            {
                oldcurrentcheck = "O";
            }
        }
        catch
        {
        }
        return data;
    }

    [WebMethod]
    public static Student[] getData1(string Name)
    {
        string data = string.Empty;
        List<Student> details = new List<Student>();
        try
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string[] split = Name.Split('-');
            string namesplit = split[0];
            //  string query = "select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.parent_name from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code   and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name='" + namesplit + "'";
            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_pincodec,a.parent_addressP,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and a.stud_name='" + namesplit + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    //ds1 = d21.select_method_wo_parameter(query, "Text");
                    //if (ds1.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    //    {
                    Student s = new Student();
                    s.RollNo = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                    //s.Name = ds1.Tables[0].Rows[i]["Stud_Name"].ToString();
                    s.Name = ds1.Tables[0].Rows[i]["stud_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    s.Batchyear = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                    s.Degree = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                    s.Department = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    s.section = ds1.Tables[0].Rows[i]["Sections"].ToString();
                    s.Semester = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                    s.StudentType = ds1.Tables[0].Rows[i]["Stud_Type"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["parent_addressP"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["Streetp"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["Cityp"].ToString();
                    s.Pin = ds1.Tables[0].Rows[i]["parent_pincodec"].ToString();
                    s.State = ds1.Tables[0].Rows[i]["State"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["Student_Mobile"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["StuPer_Id"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public string RollNo { get; set; }
        public string Batchyear { get; set; }
        public string Degree { get; set; }
        public string Department { get; set; }
        public string section { get; set; }
        public string Semester { get; set; }
        public string StudentType { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string MailId { get; set; }
    }
    public void btn_clear_Click(object sender, EventArgs e)
    {
        div_stud.Attributes.Add("style", "display:block");
        div_dept.Attributes.Add("style", "display:block");
        div_indiv.Attributes.Add("style", "display:block");
        txt_docpriv.Attributes.Add("style", "display:block");
        txt_delivery.Attributes.Add("style", "display:block");
    }

    public void btn_browse_Click(object sender, EventArgs e)
    {
    }

    public void btn_more_Click(object sender, EventArgs e)
    {
        div_stud.Attributes.Add("style", "display:block");
        div_dept.Attributes.Add("style", "display:block");
        div_indiv.Attributes.Add("style", "display:block");
        txt_docpriv.Attributes.Add("style", "display:block");
        txt_delivery.Attributes.Add("style", "display:block");
    }

    public void btn_exit_Click(object sender, EventArgs e)
    {
        div_stud.Attributes.Add("style", "display:block");
        div_dept.Attributes.Add("style", "display:block");
        div_indiv.Attributes.Add("style", "display:block");
        txt_docpriv.Attributes.Add("style", "display:block");
        txt_delivery.Attributes.Add("style", "display:block");
    }

    public void ddl_stf_delivry_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_stf_docpri_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void btn_stf_more_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = true;
    }

    public void btn_par_more_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = true;
    }

    public void ddl_par_delivry_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_par_doc_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void btn_com_more_Click(object sender, EventArgs e)
    {
        //popwindow1.Visible = true;
    }

    public void ddl_com_delivry_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_com_doc_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void btn_magz_more_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = true;
    }

    public void ddl_magz_delivry_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_magz_doc_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void btn_othr_more_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = true;
    }

    public void ddl_othr_delivry_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_othr_doc_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void btn_ind_more_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = true;
    }

    public void ddl_ind_delivry_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_ind_doc_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void loaddoc()
    {
        ddl_docpriv.Items.Clear();
        ds.Tables.Clear();
        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='TAPri'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_docpriv.DataSource = ds;
            ddl_docpriv.DataTextField = "MasterValue";
            ddl_docpriv.DataValueField = "MasterCode";
            ddl_docpriv.DataBind();
            ddl_docpriv.Items.Insert(0, new ListItem("Select", "0"));
            ddl_stf_docpri.DataSource = ds;
            ddl_stf_docpri.DataTextField = "MasterValue";
            ddl_stf_docpri.DataValueField = "MasterCode";
            ddl_stf_docpri.DataBind();
            ddl_stf_docpri.Items.Insert(0, new ListItem("Select", "0"));
            ddl_par_doc.DataSource = ds;
            ddl_par_doc.DataTextField = "MasterValue";
            ddl_par_doc.DataValueField = "MasterCode";
            ddl_par_doc.DataBind();
            ddl_par_doc.Items.Insert(0, new ListItem("Select", "0"));
            ddl_com_doc.DataSource = ds;
            ddl_com_doc.DataTextField = "MasterValue";
            ddl_com_doc.DataValueField = "MasterCode";
            ddl_com_doc.DataBind();
            ddl_com_doc.Items.Insert(0, new ListItem("Select", "0"));
            ddl_magz_doc.DataSource = ds;
            ddl_magz_doc.DataTextField = "MasterValue";
            ddl_magz_doc.DataValueField = "MasterCode";
            ddl_magz_doc.DataBind();
            ddl_magz_doc.Items.Insert(0, new ListItem("Select", "0"));
            ddl_othr_doc.DataSource = ds;
            ddl_othr_doc.DataTextField = "MasterValue";
            ddl_othr_doc.DataValueField = "MasterCode";
            ddl_othr_doc.DataBind();
            ddl_othr_doc.Items.Insert(0, new ListItem("Select", "0"));
            ddl_ind_doc.DataSource = ds;
            ddl_ind_doc.DataTextField = "MasterValue";
            ddl_ind_doc.DataValueField = "MasterCode";
            ddl_ind_doc.DataBind();
            ddl_ind_doc.Items.Insert(0, new ListItem("Select", "0"));
            ddl_stud_doc.DataSource = ds;
            ddl_stud_doc.DataTextField = "MasterValue";
            ddl_stud_doc.DataValueField = "MasterCode";
            ddl_stud_doc.DataBind();
            ddl_stud_doc.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_docpriv.Items.Insert(0, new ListItem("Select", "0"));
            ddl_stf_docpri.Items.Insert(0, new ListItem("Select", "0"));
            ddl_par_doc.Items.Insert(0, new ListItem("Select", "0"));
            ddl_com_doc.Items.Insert(0, new ListItem("Select", "0"));
            ddl_magz_doc.Items.Insert(0, new ListItem("Select", "0"));
            ddl_othr_doc.Items.Insert(0, new ListItem("Select", "0"));
            ddl_ind_doc.Items.Insert(0, new ListItem("Select", "0"));
            ddl_stud_doc.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    public void loaddelivery()
    {
        ddl_delivery.Items.Clear();
        ds.Tables.Clear();
        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='TASta'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_delivery.DataSource = ds;
            ddl_delivery.DataTextField = "MasterValue";
            ddl_delivery.DataValueField = "MasterCode";
            ddl_delivery.DataBind();
            ddl_delivery.Items.Insert(0, new ListItem("Select", "0"));
            ddl_stf_delivry.DataSource = ds;
            ddl_stf_delivry.DataTextField = "MasterValue";
            ddl_stf_delivry.DataValueField = "MasterCode";
            ddl_stf_delivry.DataBind();
            ddl_stf_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_par_delivry.DataSource = ds;
            ddl_par_delivry.DataTextField = "MasterValue";
            ddl_par_delivry.DataValueField = "MasterCode";
            ddl_par_delivry.DataBind();
            ddl_par_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_com_delivry.DataSource = ds;
            ddl_com_delivry.DataTextField = "MasterValue";
            ddl_com_delivry.DataValueField = "MasterCode";
            ddl_com_delivry.DataBind();
            ddl_com_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_othr_delivry.DataSource = ds;
            ddl_othr_delivry.DataTextField = "MasterValue";
            ddl_othr_delivry.DataValueField = "MasterCode";
            ddl_othr_delivry.DataBind();
            ddl_othr_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_magz_delivry.DataSource = ds;
            ddl_magz_delivry.DataTextField = "MasterValue";
            ddl_magz_delivry.DataValueField = "MasterCode";
            ddl_magz_delivry.DataBind();
            ddl_magz_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_ind_delivry.DataSource = ds;
            ddl_ind_delivry.DataTextField = "MasterValue";
            ddl_ind_delivry.DataValueField = "MasterCode";
            ddl_ind_delivry.DataBind();
            ddl_ind_delivry.Items.Insert(0, new ListItem("Select", "0"));

            ddl_stud_delivery.DataSource = ds;
            ddl_stud_delivery.DataTextField = "MasterValue";
            ddl_stud_delivery.DataValueField = "MasterCode";
            ddl_stud_delivery.DataBind();
            ddl_stud_delivery.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_delivery.Items.Insert(0, new ListItem("Select", "0"));
            ddl_stf_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_par_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_com_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_othr_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_magz_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_ind_delivry.Items.Insert(0, new ListItem("Select", "0"));
            ddl_stud_delivery.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    public void doc()//delsi
    {
        try
        {
            ddl_docpriv.Items.Insert(ddl_docpriv.Items.Count, "Others");
            ddl_stf_docpri.Items.Insert(ddl_stf_docpri.Items.Count, "Others");
            ddl_par_doc.Items.Insert(ddl_par_doc.Items.Count, "Others");
            ddl_com_doc.Items.Insert(ddl_com_doc.Items.Count, "Others");
            ddl_magz_doc.Items.Insert(ddl_magz_doc.Items.Count, "Others");
            ddl_ind_doc.Items.Insert(ddl_ind_doc.Items.Count, "Others");
            ddl_othr_doc.Items.Insert(ddl_othr_doc.Items.Count, "Others");
            ddl_stud_doc.Items.Insert(ddl_stud_doc.Items.Count, "Others");


        }
        catch
        {
        }
    }

    public void delivery()
    {
        try
        {
            // ddl_delivery.Items.Insert(ddl_delivery.Items.Count, "Select");
            ddl_delivery.Items.Insert(ddl_delivery.Items.Count, "Others");
            ddl_stf_delivry.Items.Insert(ddl_stf_delivry.Items.Count, "Others");
            ddl_par_delivry.Items.Insert(ddl_par_delivry.Items.Count, "Others");
            ddl_com_delivry.Items.Insert(ddl_com_delivry.Items.Count, "Others");
            ddl_othr_delivry.Items.Insert(ddl_othr_delivry.Items.Count, "Others");
            ddl_magz_delivry.Items.Insert(ddl_magz_delivry.Items.Count, "Others");
            ddl_ind_delivry.Items.Insert(ddl_ind_delivry.Items.Count, "Others");
            ddl_stud_delivery.Items.Insert(ddl_stud_delivery.Items.Count, "Others");
        }
        catch
        {
        }
    }

    public void btn_add_Click(object sender, EventArgs e)
    {
    }

    public void btn_add3_Click(object sender, EventArgs e)
    {
    }

    public void btn_add1_Click(object sender, EventArgs e)
    {
    }

    public void btn_add2_Click(object sender, EventArgs e)
    {
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaff(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        //query = "select distinct s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name+'-'+ s.staff_code, s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code  and s.staff_name like '%" + prefixText + "%'";

        query = "select distinct s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name+'-'+ s.staff_code, s.staff_code from staffmaster s,hrdept_master hr,desig_master dm,stafftrans st where st.staff_code=s.staff_code and st.dept_code =hr.dept_code and st.desig_code=dm.desig_code and st.latestrec=1 and s.settled=0 and s.resign=0 and s.staff_name like '%" + prefixText + "%'";
        newhashtbl = ws.Getnamevalue(query);
        if (newhashtbl.Count > 0)
        {
            foreach (DictionaryEntry p in newhashtbl)
            {
                string staffname = Convert.ToString(p.Key);
                name.Add(staffname);
            }
        }
        // name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffname(string prefixText)//delsi
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        if (oldcurrentcheck == "O")
        {
            query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=1 and resign =1 and s.staff_name like '%" + prefixText + "%'";
        }
        else
        {
            query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '%" + prefixText + "%'";
        }
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffcode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code like '" + prefixText + "%'";
        // string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    [WebMethod]
    public static Staff[] getstaffdetail(string Name)
    {
        string data = string.Empty;
        List<Staff> details = new List<Staff>();
        try
        {
            DataSet ds1 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string[] split = Name.Split('-');
            string namesplit = split[0];
            string query = "";
            if (oldcurrentcheck == "O")
            {
                query = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and  settled=1 and resign =1 and dm.desig_code=sa.desig_code and s.staff_name='" + namesplit + "'";
            }
            else
            {
                query = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and settled=0 and resign =0 and dm.desig_code=sa.desig_code and s.staff_name='" + namesplit + "'";
            }
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    Staff s = new Staff();
                    s.Name = ds1.Tables[0].Rows[i]["staff_name"].ToString();
                    //    + "-" + ds1.Tables[0].Rows[i]["desig_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["dept_name"].ToString();
                    s.Code = ds1.Tables[0].Rows[i]["staff_Code"].ToString();
                    s.Department = ds1.Tables[0].Rows[i]["dept_name"].ToString();
                    s.Designation = ds1.Tables[0].Rows[i]["desig_name"].ToString();
                    s.StaffType = ds1.Tables[0].Rows[i]["staffcategory"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["comm_address"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["comm_address1"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["ccity"].ToString();
                    s.Pin = ds1.Tables[0].Rows[i]["com_pincode"].ToString();
                    s.State = ds1.Tables[0].Rows[i]["cstate"].ToString();
                    //s.Country = ds1.Tables[0].Rows[i][""].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["com_mobileno"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["email"].ToString();
                    details.Add(s);
                }
            }
            else
            {
                Staff s = new Staff();
                s.clr = "1";
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }

    [WebMethod]
    public static Staff[] getstaffcodedetail(string Name)
    {
        string data = string.Empty;
        List<Staff> details = new List<Staff>();
        try
        {
            DataSet ds1 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string[] split = Name.Split('-');
            string namesplit = split[0];
            string query = "";
            if (oldcurrentcheck == "O")
            {
                query = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and  settled=1 and resign =1 and dm.desig_code=sa.desig_code and s.staff_Code='" + namesplit + "'";
            }
            else
            {
                query = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and settled=0 and resign =0 and dm.desig_code=sa.desig_code and s.staff_Code='" + namesplit + "'";
            }
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    Staff s = new Staff();
                    s.Name = ds1.Tables[0].Rows[i]["staff_name"].ToString();
                    //    + "-" + ds1.Tables[0].Rows[i]["desig_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["dept_name"].ToString();
                    s.Code = ds1.Tables[0].Rows[i]["staff_Code"].ToString();
                    s.Department = ds1.Tables[0].Rows[i]["dept_name"].ToString();
                    s.Designation = ds1.Tables[0].Rows[i]["desig_name"].ToString();
                    s.StaffType = ds1.Tables[0].Rows[i]["staffcategory"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["comm_address"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["comm_address1"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["ccity"].ToString();
                    s.Pin = ds1.Tables[0].Rows[i]["com_pincode"].ToString();
                    s.State = ds1.Tables[0].Rows[i]["cstate"].ToString();
                    //s.Country = ds1.Tables[0].Rows[i][""].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["com_mobileno"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["email"].ToString();
                    details.Add(s);
                }
            }
            else
            {
                Staff s = new Staff();
                s.clr = "1";
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }

    [WebMethod]
    public static Staff[] getstaffdetail1(string staffcode)
    {
        string data = string.Empty;
        List<Staff> details = new List<Staff>();
        try
        {
            DataSet ds1 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string query = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_Code='" + staffcode + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    Staff s = new Staff();
                    s.Name = ds1.Tables[0].Rows[i]["staff_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["desig_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["dept_name"].ToString();
                    s.Code = ds1.Tables[0].Rows[i]["staff_Code"].ToString();
                    s.Department = ds1.Tables[0].Rows[i]["dept_name"].ToString();
                    s.Designation = ds1.Tables[0].Rows[i]["desig_name"].ToString();
                    s.StaffType = ds1.Tables[0].Rows[i]["staffcategory"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["comm_address"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["comm_address1"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["ccity"].ToString();
                    s.Pin = ds1.Tables[0].Rows[i]["ccity"].ToString();
                    s.State = ds1.Tables[0].Rows[i]["cstate"].ToString();
                    //s.Country = ds1.Tables[0].Rows[i][""].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["com_mobileno"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["email"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }

    public class Staff
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string StaffType { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string MailId { get; set; }
        public string clr { get; set; }
    }
    public void btnstaff_save_Click(object sender, EventArgs e)
    {
        try
        {
            spread.Visible = false;
            Fpspread9.Visible = false;
            string bank_name = "";
            string doc_prty = "";
            string delivery_mode = "";
            dept();
            staffcodevalue();
            div_staff.Attributes.Add("style", "display:block");
            div_stud.Attributes.Add("style", "display:none");
            div_parent.Attributes.Add("style", "display:none");
            div_indiv.Attributes.Add("style", "display:none");
            div_others.Attributes.Add("style", "display:none");
            div_magazine.Attributes.Add("style", "display:none");
            div_company.Attributes.Add("style", "display:none");
            div_college.Attributes.Add("style", "display:block");
            if (ddl_stf_docpri.SelectedItem.Value != "Select")
            {
                if (ddl_stf_docpri.SelectedItem.Value != "Others")
                {
                    doc_prty = Convert.ToString(ddl_stf_docpri.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_doc.Text);
                    doc_prty = subjectcodenew("TAPri", doc_prty1);
                }
            }
            if (ddl_stf_delivry.SelectedItem.Value != "Select")
            {
                if (ddl_stf_delivry.SelectedItem.Value != "Others")
                {
                    delivery_mode = Convert.ToString(ddl_stf_delivry.SelectedItem.Value);
                }
                else
                {
                    string delivery_mode1 = Convert.ToString(txt_deliv.Text);
                    delivery_mode = subjectcodenew("TASta", delivery_mode1);
                }
            }
            if (ddl_staff_bank.SelectedItem.Value != "Select")
            {

                if (ddl_staff_bank.SelectedItem.Value != "Others")
                {
                    bank_name = Convert.ToString(ddl_staff_bank.SelectedItem.Value);
                }
                else
                {
                    string bank_name1 = Convert.ToString(txt_bank.Text);
                    bank_name = subjectcodenew("bankN", bank_name1);
                }




            }
            if (rb_stf_in.Checked == true)
            {
                type = 0;
            }
            else
            {
                type = 1;
            }
            int menutype = 2;
            string[] ay = txt_stf_date.Text.Split('/');
            string date = ay[1].ToString() + "/" + ay[0].ToString() + "/" + ay[2].ToString();
            if (cb_stf_dept.Checked == true && cb_stf_indi.Checked == true)
            {
                value = 3;
                div_stf_dept.Attributes.Add("style", "display:block");
                div_stf_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_stf_indi.Checked == true)
            {
                value = 2;
                div_stf_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_stf_dept.Checked == true)
            {
                value = 1;
                div_stf_dept.Attributes.Add("style", "display:block");
            }
            string department_code1 = Convert.ToString(txt_stf_depto.Text);
            string department_code2 = Convert.ToString(txt_stf_depto1.Text);
            string department_code3 = Convert.ToString(txt_stf_cc.Text);
            string department_code4 = Convert.ToString(txt_stf_cc1.Text);
            string staff_code01 = Convert.ToString(txt_stf_indito.Text);
            string[] split11 = staff_code01.Split('-');
            string namesplit = split11[0];
            string staff_code02 = Convert.ToString(txt_stf_indito1.Text);
            string[] split1 = staff_code02.Split('-');
            string namesplit1 = split1[0];
            string staff_code03 = Convert.ToString(txt_stf_indicc.Text);
            string[] split2 = staff_code03.Split('-');
            string namesplit2 = split2[0];
            string staff_code04 = Convert.ToString(txt_stf_indicc1.Text);
            string[] split3 = staff_code04.Split('-');
            string namesplit3 = split3[0];
            string deptcode1 = Convert.ToString(depthash[Convert.ToString(department_code1)]);

            if (deptcode1.Trim() != "")
            {
                if (staff_code == "")
                {
                    staff_code = deptcode1;
                }
                else
                {
                    staff_code = staff_code + "," + deptcode1;
                }
            }
            string deptcode2 = Convert.ToString(depthash[Convert.ToString(department_code2)]);
            if (deptcode2.Trim() != "")
            {
                if (staff_code == "")
                {
                    staff_code = deptcode2;
                }
                else
                {
                    staff_code = staff_code + "," + deptcode2;
                }
            }
            string deptcode3 = Convert.ToString(depthash[Convert.ToString(department_code3)]);
            if (deptcode3.Trim() != "")
            {
                if (staff_code1 == "")
                {
                    staff_code1 = deptcode3;
                }
                else
                {
                    staff_code1 = staff_code1 + "," + deptcode3;
                }
            }
            string deptcode4 = Convert.ToString(depthash[Convert.ToString(department_code4)]);
            if (deptcode4.Trim() != "")
            {
                if (staff_code1 == "")
                {
                    staff_code1 = deptcode4;
                }
                else
                {
                    staff_code1 = staff_code1 + "," + deptcode4;
                }
            }
            string staffcode11 = Convert.ToString(staffhash[Convert.ToString(namesplit)]);
            if (staffcode11.Trim() != "")
            {
                if (staff_indi_code == "")
                {
                    staff_indi_code = staffcode11;
                }
                else
                {
                    staff_indi_code = staff_indi_code + "," + staffcode11;
                }
            }
            string staffcode22 = Convert.ToString(staffhash[Convert.ToString(namesplit1)]);
            if (staffcode22.Trim() != "")
            {
                if (staff_indi_code == "")
                {
                    staff_indi_code = staffcode22;
                }
                else
                {
                    staff_indi_code = staff_indi_code + "," + staffcode22;
                }
            }
            string staffcode33 = Convert.ToString(staffhash[Convert.ToString(namesplit2)]);
            if (staffcode33.Trim() != "")
            {
                if (staff_indi_code1 == "")
                {
                    staff_indi_code1 = staffcode33;
                }
                else
                {
                    staff_indi_code1 = staff_indi_code1 + "," + staffcode33;
                }
            }
            string staffcode44 = Convert.ToString(staffhash[Convert.ToString(namesplit3)]);
            if (staffcode44.Trim() != "")
            {
                if (staff_indi_code1 == "")
                {
                    staff_indi_code1 = staffcode44;
                }
                else
                {
                    staff_indi_code1 = staff_indi_code1 + "," + staffcode44;
                }
            }
            //added by kowshika
            string tapalcode2 = "";
            if (txt_rcptno.Text.Trim() != "")
            {

                tapalcode2 = txt_rcptno.Text;

            }
            DateTime dt = new DateTime();
            string getday = dt.ToString("MM/dd/yyyy");
            string staff = Convert.ToString(txt_name1.Text);
            string[] split = staff.Split('-');
            string namesplit11 = split[0];
            string subject = Convert.ToString(txt_stf_subj.Text);
            string refno = Convert.ToString(txt_stf_refno.Text);
            string hr = Convert.ToString(ddl_stfhr.SelectedItem.Text);
            string min = Convert.ToString(ddl_stfm.SelectedItem.Text);
            string day = Convert.ToString(ddl_stfam.SelectedItem.Text);
            string chequeckddname = "";
            string chequeckddno = "";
            string chequeckddamt = "";
            string[] ay1;
            string date1 = "";
            string paymenttype = "";
            string payval = "";
            string bank = "";
            string bbranch = "";
            int amount = 0;
            if (cb_paymentstaff.Checked == true)
            {
                payval = "1";
                bank = Convert.ToString(ddl_staff_bank.SelectedItem.Value);
                bbranch = Convert.ToString(txt_staff_bankbarnch.Text);
                if (rdb_staff_cheque.Checked == true)
                {
                    paymenttype = "0";
                    chequeckddname = Convert.ToString(txt_staff_chequename.Text);
                    chequeckddno = Convert.ToString(txt_staff_cheque.Text);
                    chequeckddamt = Convert.ToString(txt_staff_chequeamt.Text);
                    int.TryParse(Convert.ToString(chequeckddamt), out amount);
                    ay1 = txt_staff_chequedate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
                else
                {
                    paymenttype = "1";
                    chequeckddname = Convert.ToString(txt_staffddname.Text);
                    chequeckddno = Convert.ToString(txt_staff_ddno.Text);
                    chequeckddamt = Convert.ToString(txt_staff_ddamt.Text);
                    ay1 = txt_staff_dddate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
            }
            else
            {
                payval = "0";
            }
            string time = hr + ":" + min + ":" + day;
            string staffcode = d2.GetFunction("select staff_code from staffmaster where staff_name ='" + namesplit11 + "'");
            mstaffcode = staffcode;
            if (cb_stf_dept.Checked == false && cb_stf_indi.Checked == false)
            {
                //imgdiv2.Visible = true;
                //lblalerterr.Text = "Kindly select department and individual fields or Anyone";
                //// div_stf_dept.Visible = false;
                //cb_stf_dept.Checked = true;
            }
            else
            {
                string check = "";
                string nn = Convert.ToString(txt_name1.Text);
                if (rb_old.Checked == true)
                {
                    check = d2.GetFunction("select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=1 and resign =1 and s.staff_name='" + nn + "'");
                }
                else
                {
                    check = d2.GetFunction("select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name = '" + nn + "'");
                }
                if ((ddl_stf_docpri.SelectedItem.Value == "Others" && txt_doc.Text.Trim() != "") || (ddl_stf_docpri.SelectedItem.Value != "Others"))
                {
                    if ((ddl_stf_delivry.SelectedItem.Value == "Others" && txt_deliv.Text.Trim() != "") || (ddl_stf_delivry.SelectedItem.Value != "Others"))
                    {
                        if (check != "0")
                        {
                            string query = "";
                            if (cb_paymentstaff.Checked == true)
                            {
                                query = "insert into TapalInward (MemType,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,type,staff_code,ViewStatus,SendNote,ispayment,DDNo,DDName,DDDate,Paymentamount,PaymentType,BankCode,BranchName,usercode,TapalCode)values('" + menutype + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + staff_code + "','" + staff_code1 + "','" + staff_indi_code + "','" + staff_indi_code1 + "','" + type + "','" + staffcode + "','0','0','" + payval + "','" + chequeckddno + "','" + chequeckddname + "','" + date1 + "','" + amount + "','" + paymenttype + "','" + bank_name + "','" + bbranch + "','" + usercode + "','" + tapalcode2 + "')";//delsis2602
                                //modofied by kowshika 08.05.2018
                                d2.update_method_wo_parameter(query, "Text");
                            }
                            else
                            {
                                query = "insert into TapalInward (MemType,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,type,staff_code,ViewStatus,SendNote,ispayment,usercode,TapalCode)values('" + menutype + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + staff_code + "','" + staff_code1 + "','" + staff_indi_code + "','" + staff_indi_code1 + "','" + type + "','" + staffcode + "','0','0','" + payval + "','" + usercode + "','" + tapalcode2 + "')";
                                d2.update_method_wo_parameter(query, "Text");
                            }
                            filepk = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                            upload_staffDoc();
                            string rep_no = generateReceiptNo();
                            string charge = Convert.ToString(txt_chargesstaff.Text);
                            if (charge != "")
                            {
                                access();
                                if (hdsave != "" && ledsave != "")
                                {
                                    string finYearid = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + collegecode1 + "");
                                    string ap_no = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                                    string charges_query = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,TransType,FinYearFK) values('" + date + "','" + time + "','" + rep_no + "','7','" + ap_no + "','" + hdsave + "','" + ledsave + "','0','" + charge + "','0','1','2','" + finYearid + "')";
                                    d2.update_method_wo_parameter(charges_query, "Text");
                                    #region Update Receipt No
                                    string lastRecptNo = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                                    lastRecptNo = rep_no.Replace(lastRecptNo, "");
                                    string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
                                    int save = d2.update_method_wo_parameter(updateRecpt, "Text");
                                    #endregion
                                    if (save != 0)
                                    {
                                        imgdiv2.Visible = true;
                                        lblalerterr.Text = "Saved Successfully";
                                    }
                                }
                                else
                                {
                                    imgdiv2.Visible = true;
                                    lblalerterr.Text = "Kindly Select Header & Ledger";
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lblalerterr.Text = "Kindly Select The Correct Staff Name";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "Fill The Delivery Mode";
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Fill The Document Priority";
                }
                //}/// div_stf_dept.Visible = true;
            }
            loaddoc();
            loaddelivery();
            doc();
            delivery();
            rb_stf_out.Checked = false;
            rb_stf_in.Checked = true;
            timevalue();
            td_staff.BgColor = "#c4c4c4";
            td_indi.BgColor = "white";
            td_mag.BgColor = "white";
            td_stud.BgColor = "white";
            td_othr.BgColor = "white";
            td_comp.BgColor = "white";
            td_par.BgColor = "white";
            txt_chargesstaff.Text = "";
            cb_stf_indi.Checked = false;
            div_stf_indi.Attributes.Add("style", "display:none");
        }
        catch
        {
        }
    }

    public void btnstaff_clear_Click(object sender, EventArgs e)
    {
        div_staff.Attributes.Add("style", "display:block");
    }

    public void btnparent_save_Click(object sender, EventArgs e)
    {
        try
        {
            string doc_prty = "";
            string delivery_mode = "";
            dept();
            staffcodevalue();
            div_college.Attributes.Add("style", "display:block");
            div_parent.Attributes.Add("style", "display:block");
            div_indiv.Attributes.Add("style", "display:none");
            div_others.Attributes.Add("style", "display:none");
            div_magazine.Attributes.Add("style", "display:none");
            div_company.Attributes.Add("style", "display:none");
            div_stud.Attributes.Add("style", "display:none");
            div_staff.Attributes.Add("style", "display:none");
            //  txt_par_doc1.Attributes.Add("style", "display:block");
            // txt_par_deliv1.Attributes.Add("style", "display:block");
            if (ddl_par_doc.SelectedItem.Value != "Select")
            {
                if (ddl_par_doc.SelectedItem.Value != "Others")
                {
                    doc_prty = Convert.ToString(ddl_par_doc.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_par_doc1.Text);
                    doc_prty = subjectcodenew("TAPri", doc_prty1);
                }
            }
            if (ddl_par_delivry.SelectedItem.Value != "Select")
            {
                if (ddl_par_delivry.SelectedItem.Value != "Others")
                {
                    delivery_mode = Convert.ToString(ddl_par_delivry.SelectedItem.Value);
                }
                else
                {
                    string delivery_mode1 = Convert.ToString(txt_par_deliv1.Text);
                    delivery_mode = subjectcodenew("TASta", delivery_mode1);
                }
            }
            if (rb_par_in.Checked == true)
            {
                type = 0;
            }
            else
            {
                type = 1;
            }
            int menutype = 3;
            string[] ay = txt_pardate.Text.Split('/');
            string date = ay[1].ToString() + "/" + ay[0].ToString() + "/" + ay[2].ToString();
            if (cb_par_dept.Checked == true && cb_par_indi.Checked == true && cb_par_stud.Checked == true)
            {
                value = 3;
                div_parnt_dept.Attributes.Add("style", "display:block");
                div_parnt_indi.Attributes.Add("style", "display:block");
                div_par_stud.Attributes.Add("style", "display:block");
            }
            else if (cb_par_indi.Checked == true)
            {
                value = 2;
                div_parnt_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_par_dept.Checked == true)
            {
                value = 1;
                div_parnt_dept.Attributes.Add("style", "display:block");
            }
            string department_code1 = Convert.ToString(txt_par_depto.Text);
            string department_code2 = Convert.ToString(txt_par_depto1.Text);
            string department_code3 = Convert.ToString(txt_par_cc.Text);
            string department_code4 = Convert.ToString(txt_par_cc1.Text);
            string staff_code1 = Convert.ToString(txt_par_indi.Text);
            string[] split = staff_code1.Split('-');
            string namesplit = split[0];
            string staff_code2 = Convert.ToString(txt_par_indi1.Text);
            string[] split1 = staff_code2.Split('-');
            string namesplit1 = split1[0];
            string staff_code3 = Convert.ToString(txt_par_cc2.Text);
            string[] split2 = staff_code3.Split('-');
            string namesplit2 = split2[0];
            string staff_code4 = Convert.ToString(txt_par_cc3.Text);
            string[] split3 = staff_code4.Split('-');
            string namesplit3 = split3[0];
            string stud_code = Convert.ToString(txt_par_studto.Text);
            string studecode1 = Convert.ToString(txt_par_studto1.Text);
            string deptcode1 = Convert.ToString(depthash[Convert.ToString(department_code1)]);
            if (deptcode1.Trim() != "")
            {
                if (parent_dept_code == "")
                {
                    parent_dept_code = deptcode1;
                }
                else
                {
                    parent_dept_code = parent_dept_code + "," + deptcode1;
                }
            }
            string deptcode2 = Convert.ToString(depthash[Convert.ToString(department_code2)]);
            if (deptcode2.Trim() != "")
            {
                if (parent_dept_code == "")
                {
                    parent_dept_code = deptcode2;
                }
                else
                {
                    parent_dept_code = parent_dept_code + "," + deptcode2;
                }
            }
            string deptcode3 = Convert.ToString(depthash[Convert.ToString(department_code3)]);
            if (deptcode3.Trim() != "")
            {
                if (parent_dept_code1 == "")
                {
                    parent_dept_code1 = deptcode3;
                }
                else
                {
                    parent_dept_code1 = parent_dept_code1 + "," + deptcode3;
                }
            }
            string deptcode4 = Convert.ToString(depthash[Convert.ToString(department_code4)]);
            if (deptcode4.Trim() != "")
            {
                if (parent_dept_code1 == "")
                {
                    parent_dept_code1 = deptcode4;
                }
                else
                {
                    parent_dept_code1 = parent_dept_code1 + "," + deptcode4;
                }
            }
            string staffcode11 = Convert.ToString(staffhash[Convert.ToString(namesplit)]);
            if (staffcode11.Trim() != "")
            {
                if (parent_indi_code == "")
                {
                    parent_indi_code = staffcode11;
                }
                else
                {
                    parent_indi_code = parent_indi_code + "," + staffcode11;
                }
            }
            string staffcode22 = Convert.ToString(staffhash[Convert.ToString(namesplit1)]);
            if (staffcode22.Trim() != "")
            {
                if (parent_indi_code == "")
                {
                    parent_indi_code = staffcode22;
                }
                else
                {
                    parent_indi_code = parent_indi_code + "," + staffcode22;
                }
            }
            string staffcode33 = Convert.ToString(staffhash[Convert.ToString(namesplit2)]);
            if (staffcode33.Trim() != "")
            {
                if (parent_indi_code1 == "")
                {
                    parent_indi_code1 = staffcode33;
                }
                else
                {
                    parent_indi_code1 = parent_indi_code1 + "," + staffcode33;
                }
            }
            string staffcode44 = Convert.ToString(staffhash[Convert.ToString(namesplit3)]);
            if (staffcode44.Trim() != "")
            {
                if (parent_indi_code1 == "")
                {
                    parent_indi_code1 = staffcode44;
                }
                else
                {
                    parent_indi_code1 = parent_indi_code1 + "," + staffcode44;
                }
            }
            string studentcode = Convert.ToString(studhash[Convert.ToString(stud_code)]);
            if (studentcode.Trim() != "")
            {
                if (parent_stud_code == "")
                {
                    parent_stud_code = studentcode;
                }
                else
                {
                    parent_stud_code = parent_stud_code + "," + studentcode;
                }
            }
            string studentcode1 = Convert.ToString(studhash[Convert.ToString(studecode1)]);
            if (studentcode1.Trim() != "")
            {
                if (parent_stud_code == "")
                {
                    parent_stud_code = studentcode1;
                }
                else
                {
                    parent_stud_code = parent_stud_code + "," + studentcode1;
                }
            }
            DateTime dt = new DateTime();
            string rollno = "";
            string subject = Convert.ToString(txt_par_sub.Text);
            string refno = Convert.ToString(txt_par_refno.Text);
            string getday = dt.ToString("MM/dd/yyyy");
            string hr = Convert.ToString(ddl_parhr.SelectedItem.Text);
            string min = Convert.ToString(ddl_parm.SelectedItem.Text);
            string day = Convert.ToString(ddl_param.SelectedItem.Text);
            string time = hr + ":" + min + ":" + day;
            string appno = "";
            string chequeckddname = "";
            string chequeckddno = "";
            string chequeckddamt = "";
            string[] ay1;
            string date1 = "";
            string paymenttype = "";
            string payval = "";
            string bank = "";
            string bbranch = "";
            if (cb_paymentpar.Checked == true)
            {
                bank = Convert.ToString(ddl_par_bank.SelectedItem.Value);
                bbranch = Convert.ToString(txt_par_bankbanch.Text);
                payval = "1";
                if (rdo_par_cheque.Checked == true)
                {
                    paymenttype = "0";
                    chequeckddname = Convert.ToString(txt_par_chq_name.Text);
                    chequeckddno = Convert.ToString(txt_par_chq_no.Text);
                    chequeckddamt = Convert.ToString(txt_par_chq_amt.Text);
                    ay1 = txt_par_chequedate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
                else
                {
                    paymenttype = "1";
                    chequeckddname = Convert.ToString(txt_par_ddname.Text);
                    chequeckddno = Convert.ToString(txt_parddno.Text);
                    chequeckddamt = Convert.ToString(txt_par_ddamt.Text);
                    ay1 = txt_pardddate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
            }
            else
            {
                payval = "0";
            }
            if (rdo_parent_old.Checked == true)
            {
                rollno = Convert.ToString(txt_rollno.Text);
                string appno1 = "select app_no , roll_no from Registration where Reg_No ='" + rollno + "'";
                DataSet dne = d2.select_method_wo_parameter(appno1, "Text");
                if (dne.Tables[0].Rows.Count > 0)
                {
                    appno = Convert.ToString(dne.Tables[0].Rows[0]["app_no"]);
                    rollno = Convert.ToString(dne.Tables[0].Rows[0]["roll_no"]);
                }
            }
            else
            {
                rollno = Convert.ToString(txt_rollno.Text);
                appno = d2.GetFunction("select app_no  from Registration where roll_no ='" + rollno + "'");
            }
            mcode_par = appno;
            if (cb_par_dept.Checked == false && cb_par_indi.Checked == false && cb_par_stud.Checked == false)
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = " Kindly select department and individual fields or Anyone";
            }
            else
            {
                if ((ddl_par_doc.SelectedItem.Value == "Others" && txt_par_doc1.Text.Trim() != "") || (ddl_par_doc.SelectedItem.Value != "Others"))
                {
                    if ((ddl_par_delivry.SelectedItem.Value == "Others" && txt_par_deliv1.Text.Trim() != "") || (ddl_par_delivry.SelectedItem.Value != "Others"))
                    {
                        string check = "";
                        if (rdo_parent_old.Checked == true)
                        {
                            check = d2.GetFunction("select r.Reg_No,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and CC=1 and DelFlag =0 and Exam_Flag <>'DEBAR'  and r.Reg_No='" + txt_rollno.Text + "'");
                        }
                        else
                        {
                            check = d2.GetFunction("select  r.Reg_No from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.Roll_no='" + txt_rollno.Text + "'");
                        }
                        if (check != "0")
                        {
                            string query = "";
                            if (cb_paymentpar.Checked == true)
                            {
                                query = "insert into TapalInward (MemType,App_no,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,tostudentno,type,ViewStatus,SendNote,ispayment,DDNo,DDName,DDDate,Paymentamount,PaymentType,BankCode,BranchName)values('" + menutype + "','" + appno + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + parent_dept_code + "','" + parent_dept_code1 + "','" + parent_indi_code + "','" + parent_indi_code1 + "','" + parent_stud_code + "','" + type + "','0','0','" + payval + "','" + chequeckddno + "','" + chequeckddname + "','" + date1 + "','" + chequeckddamt + "','" + paymenttype + "','" + bank + "','" + bbranch + "')";
                                d2.update_method_wo_parameter(query, "Text");
                            }
                            else
                            {
                                query = "insert into TapalInward (MemType,App_no,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,tostudentno,type,ViewStatus,SendNote,ispayment)values('" + menutype + "','" + appno + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + parent_dept_code + "','" + parent_dept_code1 + "','" + parent_indi_code + "','" + parent_indi_code1 + "','" + parent_stud_code + "','" + type + "','0','0','" + payval + "')";
                                d2.update_method_wo_parameter(query, "Text");
                            }
                            filepk = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                            upload_par();
                            string rep_no = generateReceiptNo();
                            string charge = Convert.ToString(txt_chargespar.Text);
                            if (charge != "")
                            {
                                if (hdsave != "" & ledsave != "")
                                {
                                    string finYearid = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + collegecode1 + "");
                                    string ap_no = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                                    string charges_query = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,TransType,FinYearFK) values('" + date + "','" + time + "','" + rep_no + "','7','" + ap_no + "','" + hdsave + "','" + ledsave + "','0','" + charge + "','0','1','2','" + finYearid + "')";
                                    d2.update_method_wo_parameter(charges_query, "Text");
                                    #region Update Receipt No
                                    string lastRecptNo = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                                    lastRecptNo = rep_no.Replace(lastRecptNo, "");
                                    string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
                                    int save = d2.update_method_wo_parameter(updateRecpt, "Text");
                                    #endregion
                                    if (save != 0)
                                    {
                                        imgdiv2.Visible = true;
                                        lblalerterr.Text = "Saved Successfully";
                                    }
                                }
                                else
                                {
                                    imgdiv2.Visible = true;
                                    lblalerterr.Text = "Kindly Select Header & Ledger";
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lblalerterr.Text = "Kindly Select The Correct Student Roll No/Reg No";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "Fill The Delivery Mode";
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Fill The Document Priority";
                }
                //}
            }
            loaddelivery();
            loaddoc();
            doc();
            delivery();
            rb_par_out.Checked = false;
            rb_par_in.Checked = true;
            timevalue();
            td_par.BgColor = "#c4c4c4";
            td_indi.BgColor = "white";
            td_mag.BgColor = "white";
            td_staff.BgColor = "white";
            td_stud.BgColor = "white";
            td_othr.BgColor = "white";
            td_comp.BgColor = "white";
            txt_chargespar.Text = "";
            cb_par_stud.Checked = false;
            cb_par_indi.Checked = false;
            div_parnt_indi.Attributes.Add("style", "display:none");
            div_par_stud.Attributes.Add("style", "display:none");
        }
        catch
        {
        }
    }

    public void btnparentclear_Click(object sender, EventArgs e)
    {
        loaddelivery();
        loaddoc();
        doc();
        delivery();
    }
    [WebMethod]
    public static parent[] parentdetail(string Name)
    {
        string data = string.Empty;
        List<parent> details = new List<parent>();
        try
        {
            DataSet ds1 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string[] split = Name.Split('-');
            string namesplit = split[0];
            string query = "select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester, a.parent_addressC,a.parentF_Mobile,a.parentM_Mobile,a.emailM,a.parent_name, r.Sections ,r.Batch_Year,a.parent_addressP,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and a.stud_name='" + namesplit + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    parent s = new parent();
                    s.RollNo = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                    s.Name = ds1.Tables[0].Rows[i]["Stud_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    s.Parent = ds1.Tables[0].Rows[i]["parent_name"].ToString();
                    s.Mob = ds1.Tables[0].Rows[i]["parentF_Mobile"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["parent_addressP"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["Streetp"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["Cityp"].ToString();
                    s.State = ds1.Tables[0].Rows[i]["State"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["parentM_Mobile"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["emailM"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    [WebMethod]
    public static parent[] parentdetail1(string Roll_No)
    {
        string data = string.Empty;
        List<parent> details = new List<parent>();
        try
        {
            DataSet ds1 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string query = "";
            if (oldcurrentcheck == "O")
            {
                query = "select a.stud_name,r.Reg_No, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester, a.parent_addressC,a.parentF_Mobile,a.parentM_Mobile,a.emailM,a.parent_name, r.Sections ,r.Batch_Year,a.parent_addressP,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and r.Reg_No='" + Roll_No + "'";
            }
            else
            {
                query = "select a.stud_name,r.Reg_No, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester, a.parent_addressC,a.parentF_Mobile,a.parentM_Mobile,a.emailM,a.parent_name, r.Sections ,r.Batch_Year,a.parent_addressP,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and r.Roll_no='" + Roll_No + "'";
            }
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    parent s = new parent();
                    if (oldcurrentcheck == "O")
                    {
                        s.RollNo = ds1.Tables[0].Rows[i]["Reg_No"].ToString();
                    }
                    else
                    {
                        s.RollNo = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                    }
                    s.Name = ds1.Tables[0].Rows[i]["Stud_Name"].ToString();
                    //+ "-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    s.Parent = ds1.Tables[0].Rows[i]["parent_name"].ToString();
                    s.Mob = ds1.Tables[0].Rows[i]["parentF_Mobile"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["parent_addressP"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["Streetp"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["Cityp"].ToString();
                    s.State = ds1.Tables[0].Rows[i]["State"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["parentM_Mobile"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["emailM"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    [WebMethod]
    public static parent[] parentdetailname(string ParentName)
    {
        string data = string.Empty;
        List<parent> details = new List<parent>();
        try
        {
            DataSet ds1 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string query = "select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester, a.parent_addressC,a.parentF_Mobile,a.parentM_Mobile,a.emailM,a.parent_name, r.Sections ,r.Batch_Year,a.parent_addressP,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and a.parent_name='" + ParentName + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    parent s = new parent();
                    s.RollNo = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                    s.Name = ds1.Tables[0].Rows[i]["Stud_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    s.Parent = ds1.Tables[0].Rows[i]["parent_name"].ToString();
                    s.Mob = ds1.Tables[0].Rows[i]["parentF_Mobile"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["parent_addressP"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["Streetp"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["Cityp"].ToString();
                    s.State = ds1.Tables[0].Rows[i]["State"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["parentM_Mobile"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["emailM"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    [WebMethod]
    public static parent[] parentdetailmob(string Parentmob)
    {
        string data = string.Empty;
        List<parent> details = new List<parent>();
        try
        {
            DataSet ds1 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string query = "select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester, a.parent_addressC,a.parentF_Mobile,a.parentM_Mobile,a.emailM,a.parent_name, r.Sections ,r.Batch_Year,a.parent_addressP,Streetp,parent_pincodec,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and a.parentF_Mobile='" + Parentmob + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    parent s = new parent();
                    s.RollNo = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                    s.Name = ds1.Tables[0].Rows[i]["Stud_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    s.Parent = ds1.Tables[0].Rows[i]["parent_name"].ToString();
                    s.Mob = ds1.Tables[0].Rows[i]["parentF_Mobile"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["parent_addressP"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["Streetp"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["Cityp"].ToString();
                    s.Pin = ds1.Tables[0].Rows[i]["parent_pincodec"].ToString();
                    s.State = ds1.Tables[0].Rows[i]["State"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["parentM_Mobile"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["emailM"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    public class parent
    {
        public string Name { get; set; }
        public string RollNo { get; set; }
        public string Parent { get; set; }
        public string Address { get; set; }
        public string Mob { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string MailId { get; set; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getparentmob(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct parentF_Mobile from applyn where parentF_Mobile !='' and parentF_Mobile like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getparentname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select parent_name from applyn where parent_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    public void btncompany_save_Click(object sender, EventArgs e)
    {
        try
        {
            spread.Visible = false;
            Fpspread9.Visible = false;
            string bank_name = "";
            string doc_prty = "";
            string delivery_mode = "";
            dept();
            staffcodevalue();
            div_comp_dept.Attributes.Add("style", "display:block");
            div_college.Attributes.Add("style", "display:block");
            div_company.Attributes.Add("style", "display:block");
            div_individu.Attributes.Add("style", "display:none");
            div_others.Attributes.Add("style", "display:none");
            div_stud.Attributes.Add("style", "display:none");
            div_staff.Attributes.Add("style", "display:none");
            div_magazine.Attributes.Add("style", "display:none");
            div_parent.Attributes.Add("style", "display:none");
            if (ddl_com_doc.SelectedItem.Value != "Select")
            {
                if (ddl_com_doc.SelectedItem.Value != "Others")
                {
                    doc_prty = Convert.ToString(ddl_com_doc.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_com_doc1.Text);
                    doc_prty = subjectcodenew("TAPri", doc_prty1);
                }
            }
            if (ddl_com_delivry.SelectedItem.Value != "Select")
            {
                if (ddl_com_delivry.SelectedItem.Value != "Others")
                {
                    delivery_mode = Convert.ToString(ddl_com_delivry.SelectedItem.Value);
                }
                else
                {
                    string delivery_mode1 = Convert.ToString(txt_com_deliv1.Text);
                    delivery_mode = subjectcodenew("TASta", delivery_mode1);
                }
            }

            if (txt_bank_comp.SelectedItem.Value != "Select")
            {

                if (txt_bank_comp.SelectedItem.Value != "Others")
                {
                    bank_name = Convert.ToString(txt_bank_comp.SelectedItem.Value);
                }
                else
                {
                    string bank_name1 = Convert.ToString(txt_bank_company.Text);
                    bank_name = subjectcodenew("bankN", bank_name1);
                }




            }


            comp_save();
            if (rb_comp_in.Checked == true)
            {
                type = 0;
            }
            else
            {
                type = 1;
            }
            int menutype = 4;
            string[] ay = txt_com_date.Text.Split('/');
            string date = ay[1].ToString() + "/" + ay[0].ToString() + "/" + ay[2].ToString();
            if (cb_com_dept.Checked == true && cb_com_indi.Checked == true)
            {
                value = 3;
                div_comp_dept.Attributes.Add("style", "display:block");
                div_comp_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_com_indi.Checked == true)
            {
                value = 2;
                div_comp_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_com_dept.Checked == true)
            {
                value = 1;
                div_comp_dept.Attributes.Add("style", "display:block");
            }
            string department_code1 = Convert.ToString(txt_comdepto.Text);
            string department_code2 = Convert.ToString(txt_comdepto1.Text);
            string department_code3 = Convert.ToString(txt_comcc.Text);
            string department_code4 = Convert.ToString(txt_comcc2.Text);
            string staff_code1 = Convert.ToString(txt_com_ind.Text);
            string[] split = staff_code1.Split('-');
            string namesplit = split[0];
            string staff_code2 = Convert.ToString(txt_com_ind1.Text);
            string[] split1 = staff_code2.Split('-');
            string namesplit1 = split1[0];
            string staff_code3 = Convert.ToString(txt_com_cc.Text);
            string[] split2 = staff_code3.Split('-');
            string namesplit2 = split2[0];
            string staff_code4 = Convert.ToString(txt_com_cc1.Text);
            string[] split3 = staff_code4.Split('-');
            string namesplit3 = split3[0];
            string deptcode1 = Convert.ToString(depthash[Convert.ToString(department_code1)]);
            if (deptcode1.Trim() != "")
            {
                if (company_dept_code == "")
                {
                    company_dept_code = deptcode1;
                }
                else
                {
                    company_dept_code = company_dept_code + "," + deptcode1;
                }
            }
            string deptcode2 = Convert.ToString(depthash[Convert.ToString(department_code2)]);
            if (deptcode2.Trim() != "")
            {
                if (company_dept_code == "")
                {
                    company_dept_code = deptcode2;
                }
                else
                {
                    company_dept_code = company_dept_code + "," + deptcode2;
                }
            }
            string deptcode3 = Convert.ToString(depthash[Convert.ToString(department_code3)]);
            if (deptcode3.Trim() != "")
            {
                if (company_dept_code1 == "")
                {
                    company_dept_code1 = deptcode3;
                }
                else
                {
                    company_dept_code1 = company_dept_code1 + "," + deptcode3;
                }
            }
            string deptcode4 = Convert.ToString(depthash[Convert.ToString(department_code4)]);
            if (deptcode4.Trim() != "")
            {
                if (company_dept_code1 == "")
                {
                    company_dept_code1 = deptcode4;
                }
                else
                {
                    company_dept_code1 = company_dept_code1 + "," + deptcode4;
                }
            }
            string staffcode11 = Convert.ToString(staffhash[Convert.ToString(namesplit)]);
            if (staffcode11.Trim() != "")
            {
                if (company_indi_code == "")
                {
                    company_indi_code = staffcode11;
                }
                else
                {
                    company_indi_code = company_indi_code + "," + staffcode11;
                }
            }
            string staffcode22 = Convert.ToString(staffhash[Convert.ToString(namesplit1)]);
            if (staffcode22.Trim() != "")
            {
                if (company_indi_code == "")
                {
                    company_indi_code = staffcode22;
                }
                else
                {
                    company_indi_code = company_indi_code + "," + staffcode22;
                }
            }
            string staffcode33 = Convert.ToString(staffhash[Convert.ToString(namesplit2)]);
            if (staffcode33.Trim() != "")
            {
                if (company_indi_code1 == "")
                {
                    company_indi_code1 = staffcode33;
                }
                else
                {
                    company_indi_code1 = company_indi_code1 + "," + staffcode33;
                }
            }
            string staffcode44 = Convert.ToString(staffhash[Convert.ToString(namesplit3)]);
            if (staffcode44.Trim() != "")
            {
                if (company_indi_code1 == "")
                {
                    company_indi_code1 = staffcode44;
                }
                else
                {
                    company_indi_code1 = company_indi_code1 + "," + staffcode44;
                }
            }
            //added by kowshika
            string tapalcode3 = "";
            if (txt_rcptno.Text.Trim() != "")
            {

                tapalcode3 = txt_rcptno.Text;

            }

            DateTime dt = new DateTime();
            string subject = Convert.ToString(txt_com_sub.Text);
            string refno = Convert.ToString(txt_com_refno.Text);
            string companyname = Convert.ToString(txt_cname.Text);
            string addrs = Convert.ToString(txt_com_add.Text);
            string street = Convert.ToString(txt_com_str.Text);
            string city = Convert.ToString(txt_com_city.Text);
            string state = Convert.ToString(txt_com_state.Text);
            string country = Convert.ToString(txt_com_county.Text);
            string phn_no = Convert.ToString(txt_com_ph.Text);
            string mail = Convert.ToString(txt_com_mail.Text);
            string appno = d2.GetFunction("select VendorContactPK from IM_VendorContactMaster where VenContactName='" + txt_pername.Text + "'");
            string getday = dt.ToString("MM/dd/yyyy");
            string hr = Convert.ToString(ddl_comhr.SelectedItem.Text);
            string min = Convert.ToString(ddl_commi.SelectedItem.Text);
            string day = Convert.ToString(ddl_comam.SelectedItem.Text);
            string chequeckddname = "";
            string chequeckddno = "";
            string chequeckddamt = "";
            string[] ay1;
            string date1 = "";
            string paymenttype = "";
            string payval = "";
            string bank = "";
            string bbranch = "";
            int amount = 0;
            if (cb_paymentcomp.Checked == true)
            {
                payval = "1";
                bank = Convert.ToString(txt_bank_comp.SelectedItem.Value);
                bbranch = Convert.ToString(txt_comp_bankbranch.Text);
                if (rdo_comp_cheque.Checked == true)
                {
                    paymenttype = "0";
                    chequeckddname = Convert.ToString(txt_comp_chqname.Text);
                    chequeckddno = Convert.ToString(txt_comp_chqno.Text);
                    chequeckddamt = Convert.ToString(txt_comp_chqamt.Text);

                    int.TryParse(Convert.ToString(chequeckddamt), out amount);

                    ay1 = txt_comp_chqdate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
                else
                {
                    paymenttype = "1";
                    chequeckddname = Convert.ToString(txt_compddname.Text);
                    chequeckddno = Convert.ToString(txt_compddno.Text);
                    chequeckddamt = Convert.ToString(txt_comp_ddamt.Text);
                    ay1 = txt_comp_dddate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
            }
            else
            {
                payval = "0";
            }
            string time = hr + ":" + min + ":" + day;
            mcode_comp = companyname;
            if (cb_com_dept.Checked == false && cb_com_indi.Checked == false)
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = " Kindly select department and individual fields or Anyone";
            }
            else
            {
                if ((ddl_com_doc.SelectedItem.Value == "Others" && txt_com_doc1.Text != "") || (ddl_com_doc.SelectedItem.Value != "Others"))
                {
                    if ((ddl_com_delivry.SelectedItem.Value == "Others" && txt_com_deliv1.Text != "") || (ddl_com_delivry.SelectedItem.Value != "Others"))
                    {
                        string query = "";
                        if (cb_paymentcomp.Checked == true)
                        {
                            query = "insert into TapalInward (MemType,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,FromName,FromStreet,FromCity,FromState,FromPhoneNo,FromMailID,type,ViewStatus,SendNote,App_No,ispayment,DDNo,DDName,DDDate,Paymentamount,PaymentType,BankCode,BranchName,usercode,TapalCode)values('" + menutype + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + company_dept_code + "','" + company_dept_code1 + "','" + company_indi_code + "','" + company_indi_code1 + "','" + companyname + "','" + street + "','" + city + "','" + state + "','" + phn_no + "','" + mail + "','" + type + "','0','0','" + appno + "','" + payval + "','" + chequeckddno + "','" + chequeckddname + "','" + date1 + "','" + amount + "','" + paymenttype + "','" + bank_name + "','" + bbranch + "','" + usercode + "','" + tapalcode3 + "')";
                            d2.update_method_wo_parameter(query, "Text");//modified by kowshika
                        }
                        else
                        {
                            query = "insert into TapalInward (MemType,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,FromName,FromStreet,FromCity,FromState,FromPhoneNo,FromMailID,type,ViewStatus,SendNote,App_No,ispayment,usercode,TapalCode)values('" + menutype + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + company_dept_code + "','" + company_dept_code1 + "','" + company_indi_code + "','" + company_indi_code1 + "','" + companyname + "','" + street + "','" + city + "','" + state + "','" + phn_no + "','" + mail + "','" + type + "','0','0','" + appno + "','" + payval + "','" + usercode + "' ,'" + tapalcode3 + "')";
                            d2.update_method_wo_parameter(query, "Text");
                        }
                        filepk = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                        // upload_comp();
                        uploadnew_comp();
                        string rep_no = generateReceiptNo();
                        string charge = Convert.ToString(txt_chargecomp.Text);
                        if (charge != "")
                        {
                            if (hdsave != "" && ledsave != "")
                            {
                                string finYearid = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + collegecode1 + "");
                                string ap_no = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                                string charges_query = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,TransType,FinYearFK) values('" + date + "','" + time + "','" + rep_no + "','7','" + ap_no + "','" + hdsave + "','" + ledsave + "','0','" + charge + "','0','1','2','" + finYearid + "')";
                                d2.update_method_wo_parameter(charges_query, "Text");
                                #region Update Receipt No
                                string lastRecptNo = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                                lastRecptNo = rep_no.Replace(lastRecptNo, "");
                                string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
                                int save = d2.update_method_wo_parameter(updateRecpt, "Text");
                                #endregion
                                if (save != 0)
                                {
                                    imgdiv2.Visible = true;
                                    lblalerterr.Text = "Saved Successfully";
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lblalerterr.Text = "Kindly Select Header & Ledger";
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lblalerterr.Text = "Saved Successfully";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "Fill The Delivery Mode";
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Fill The Document Priority";
                }
                //}
            }
            loaddoc();
            loaddelivery();
            doc();
            delivery();
            rb_comp_out.Checked = false;
            rb_comp_in.Checked = true;
            timevalue();
            td_comp.BgColor = "#c4c4c4";
            td_indi.BgColor = "white";
            td_mag.BgColor = "white";
            td_par.BgColor = "white";
            td_staff.BgColor = "white";
            td_stud.BgColor = "white";
            td_othr.BgColor = "white";
            txt_com_ind.Text = "";
            txt_com_cc.Text = "";
            txt_com_ind1.Text = "";
            txt_com_cc1.Text = "";
            txt_chargecomp.Text = "";
            cb_com_indi.Checked = false;
            div_comp_indi.Attributes.Add("style", "display:none");
        }
        catch
        {
        }
    }

    public void comp_save()
    {
        string VendorCode = "";
        string statecode = "";
        string countrycode = "";
        string state = Convert.ToString(txt_com_state.Text);
        statecode = subjectcode("state", state);
        string country = Convert.ToString(txt_com_county.Text);
        countrycode = subjectcode("coun", country);
        string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=4 and VendorCompName like '" + txt_cname.Text + "%'");
        if (VenCode != "" && VenCode != null && VenCode != "0")
        {
            VendorCode = VenCode;
        }
        else
        {
            VendorCodeGen();
            VendorCode = Session["VendorCode"].ToString();
            string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + txt_cname.Text + "' and VendorType='4') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + txt_cname.Text + "',VendorType='4',VendorAddress='" + txt_com_add.Text + "',VendorStreet='" + txt_com_str.Text + "',VendorCity='" + txt_com_city.Text + "',VendorCountry='" + countrycode + "',VendorPin='" + txt_com_pin.Text + "', VendorState='" + statecode + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + txt_cname.Text + "' and VendorType='4' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorState,VendorCountry) values('" + VendorCode + "','" + txt_cname.Text + "','4','" + txt_com_add.Text + "','" + txt_com_str.Text + "','" + txt_com_city.Text + "','" + txt_com_pin.Text + "','" + statecode + "','" + countrycode + "')";
            int vc = d2.update_method_wo_parameter(venmst, "TEXT");
        }
        Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
        string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + txt_pername.Text + "' and VenContactDesig='" + txt_des.Text + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + txt_pername.Text + "',VenContactDesig='" + txt_des.Text + "',VendorPhoneNo='" + txt_com_ph.Text + "',VendorEmail='" + txt_com_mail.Text + "' where VendorFK='" + VendorFK + "' and VenContactName='" + txt_pername.Text + "' and VenContactDesig='" + txt_des.Text + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VenContactDesig,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + txt_pername.Text + "','" + txt_des.Text + "','" + txt_com_ph.Text + "','" + txt_com_mail.Text + "')";
        int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");
    }

    public void btncompany_clear_Click(object sender, EventArgs e)
    {
        div_company.Attributes.Add("style", "display:block");
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getcompname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where VendorType=4 and VendorCompName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getcompnameper(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select VenContactName from IM_VendorContactMaster where VenContactName like '" + prefixText + "%'";
        string query = "select VenContactName from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK AND VendorType='4' and VenContactName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [WebMethod]
    public static company[] comapanydetail(string Name)
    {
        string data = string.Empty;
        List<company> details = new List<company>();
        try
        {
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string vendorpk = d21.GetFunction("select VendorPK from CO_VendorMaster where VendorCompName like '" + Name + "%'");
            string query = "select v.VendorCode,v.VendorCompName,v.VendorCountry,v.VendorAddress,v.VendorCity,v.VendorStreet,v.VendorDist,v.VendorState,v.VendorPin,vc.VenContactName,vc.VenContactDept,vc.VenContactDesig,vc.VendorPhoneNo,vc.VendorExtNo,vc.VendorMobileNo,vc.VendorEmail from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and v.VendorPK='" + vendorpk + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    company s = new company();
                    string statecode = ds1.Tables[0].Rows[i]["VendorState"].ToString();
                    string countrycode = ds1.Tables[0].Rows[i]["VendorCountry"].ToString();
                    string statename = d21.GetFunction("select textval from TextValTable where  TextCriteria='state' and TextCode='" + statecode + "' ");
                    string countryname = d21.GetFunction("select textval from TextValTable where  TextCriteria='coun' and TextCode='" + countrycode + "' ");
                    s.Name = ds1.Tables[0].Rows[i]["VendorCompName"].ToString();
                    s.Per_Name = ds1.Tables[0].Rows[i]["VenContactName"].ToString();
                    s.Designation = ds1.Tables[0].Rows[i]["VenContactDesig"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["VendorStreet"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["VendorCity"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["VendorAddress"].ToString();
                    s.State = statename;
                    s.Country = countryname;
                    s.Pin = ds1.Tables[0].Rows[i]["VendorPin"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["VendorPhoneNo"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["VendorEmail"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    [WebMethod]
    public static company[] comapanydetailperson(string Name)
    {
        string data = string.Empty;
        List<company> details = new List<company>();
        try
        {
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string vendorpk = d21.GetFunction(" select VendorFK from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK AND VendorType='4' and VenContactName like '" + Name + "%'");
            string query = "select v.VendorCode,v.VendorCompName,v.VendorCountry,v.VendorAddress,v.VendorCity,v.VendorStreet,v.VendorDist,v.VendorState,v.VendorPin,vc.VenContactName,vc.VenContactDept,vc.VenContactDesig,vc.VendorPhoneNo,vc.VendorExtNo,vc.VendorMobileNo,vc.VendorEmail from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and v.VendorPK='" + vendorpk + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    company s = new company();
                    string statecode = ds1.Tables[0].Rows[i]["VendorState"].ToString();
                    string countrycode = ds1.Tables[0].Rows[i]["VendorCountry"].ToString();
                    string statename = d21.GetFunction("select textval from TextValTable where  TextCriteria='state' and TextCode='" + statecode + "' ");
                    string countryname = d21.GetFunction("select textval from TextValTable where  TextCriteria='coun' and TextCode='" + countrycode + "' ");
                    s.Name = ds1.Tables[0].Rows[i]["VendorCompName"].ToString();
                    s.Per_Name = ds1.Tables[0].Rows[i]["VenContactName"].ToString();
                    s.Designation = ds1.Tables[0].Rows[i]["VenContactDesig"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["VendorStreet"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["VendorCity"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["VendorAddress"].ToString();
                    s.State = statename;
                    s.Country = countryname;
                    s.Pin = ds1.Tables[0].Rows[i]["VendorPin"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["VendorPhoneNo"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["VendorEmail"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    public class company
    {
        public string Name { get; set; }
        public string Per_Name { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string MailId { get; set; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getindiname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where VendorType=5 and VendorCompName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getmagazinename(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where VendorType=6 and VendorCompName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getothername(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where VendorType=7 and VendorCompName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getotherpername(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorName from CO_VendorMaster where VendorType=7 and VendorName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    public void btnother_save_Click(object sender, EventArgs e)
    {
        try
        {
            spread.Visible = false;
            Fpspread9.Visible = false;
            string bank_name = "";
            string doc_prty = "";
            string delivery_mode = "";
            dept();
            staffcodevalue();
            div_college.Attributes.Add("style", "display:block");
            div_others.Attributes.Add("style", "display:block");
            div_company.Attributes.Add("style", "display:none");
            div_staff.Attributes.Add("style", "display:none");
            div_stud.Attributes.Add("style", "display:none");
            div_parent.Attributes.Add("style", "display:none");
            div_individu.Attributes.Add("style", "display:none");
            div_magazine.Attributes.Add("style", "display:none");
            if (ddl_othr_doc.SelectedItem.Value != "Select")
            {
                if (ddl_othr_doc.SelectedItem.Value != "Others")
                {
                    doc_prty = Convert.ToString(ddl_othr_doc.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_othr_doc.Text);
                    doc_prty = subjectcodenew("TAPri", doc_prty1);
                }
            }
            if (ddl_othr_delivry.SelectedItem.Value != "Select")
            {
                if (ddl_othr_delivry.SelectedItem.Value != "Others")
                {
                    delivery_mode = Convert.ToString(ddl_othr_delivry.SelectedItem.Value);
                }
                else
                {
                    string delivery_mode1 = Convert.ToString(txt_othr_delivry.Text);
                    delivery_mode = subjectcodenew("TASta", delivery_mode1);
                }
            }



            if (ddl_othr_bank.SelectedItem.Value != "Select")
            {

                if (ddl_othr_bank.SelectedItem.Value != "Others")
                {
                    bank_name = Convert.ToString(ddl_othr_bank.SelectedItem.Value);
                }
                else
                {
                    string bank_name1 = Convert.ToString(txt_others_bank.Text);
                    bank_name = subjectcodenew("bankN", bank_name1);
                }




            }

            if (rb_othr_in.Checked == true)
            {
                type = 0;
            }
            else
            {
                type = 1;
            }
            int menutype = 7;
            string[] ay = txt_othrdate.Text.Split('/');
            string date = ay[1].ToString() + "/" + ay[0].ToString() + "/" + ay[2].ToString();
            if (cb_othr_dept.Checked == true && cb_othr_indi.Checked == true)
            {
                value = 3;
                div_other_dept.Attributes.Add("style", "display:block");
                div_other_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_othr_indi.Checked == true)
            {
                value = 2;
                div_other_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_othr_dept.Checked == true)
            {
                value = 1;
                div_other_dept.Attributes.Add("style", "display:block");
            }
            string department_code1 = Convert.ToString(txt_othr_depto.Text);
            string department_code2 = Convert.ToString(txt_othr_depto1.Text);
            string department_code3 = Convert.ToString(txt_othr_cc.Text);
            string department_code4 = Convert.ToString(txt_othr_cc1.Text);
            string staff_code1 = Convert.ToString(txt_othr_indi.Text);
            string[] split = staff_code1.Split('-');
            string namesplit = split[0];
            string staff_code2 = Convert.ToString(txt_othr_indi1.Text);
            string[] split1 = staff_code2.Split('-');
            string namesplit1 = split1[0];
            string staff_code3 = Convert.ToString(txt_othr_cc2.Text);
            string[] split2 = staff_code3.Split('-');
            string namesplit2 = split2[0];
            string staff_code4 = Convert.ToString(txt_othr_cc3.Text);
            string[] split3 = staff_code4.Split('-');
            string namesplit3 = split3[0];
            string deptcode1 = Convert.ToString(depthash[Convert.ToString(department_code1)]);
            if (deptcode1.Trim() != "")
            {
                if (other_dept_code == "")
                {
                    other_dept_code = deptcode1;
                }
                else
                {
                    other_dept_code = other_dept_code + "," + deptcode1;
                }
            }
            string deptcode2 = Convert.ToString(depthash[Convert.ToString(department_code2)]);
            if (deptcode2.Trim() != "")
            {
                if (other_dept_code == "")
                {
                    other_dept_code = deptcode2;
                }
                else
                {
                    other_dept_code = other_dept_code + "," + deptcode2;
                }
            }
            string deptcode3 = Convert.ToString(depthash[Convert.ToString(department_code3)]);
            if (deptcode3.Trim() != "")
            {
                if (other_dept_code1 == "")
                {
                    other_dept_code1 = deptcode3;
                }
                else
                {
                    other_dept_code1 = other_dept_code1 + "," + deptcode3;
                }
            }
            string deptcode4 = Convert.ToString(depthash[Convert.ToString(department_code4)]);
            if (deptcode4.Trim() != "")
            {
                if (other_dept_code1 == "")
                {
                    other_dept_code1 = deptcode4;
                }
                else
                {
                    other_dept_code1 = other_dept_code1 + "," + deptcode4;
                }
            }
            string staffcode11 = Convert.ToString(staffhash[Convert.ToString(namesplit)]);
            if (staffcode11.Trim() != "")
            {
                if (other_staff_code == "")
                {
                    other_staff_code = staffcode11;
                }
                else
                {
                    other_staff_code = other_staff_code + "," + staffcode11;
                }
            }
            string staffcode22 = Convert.ToString(staffhash[Convert.ToString(namesplit1)]);
            if (staffcode22.Trim() != "")
            {
                if (other_staff_code == "")
                {
                    other_staff_code = staffcode22;
                }
                else
                {
                    other_staff_code = other_staff_code + "," + staffcode22;
                }
            }
            string staffcode33 = Convert.ToString(staffhash[Convert.ToString(namesplit2)]);
            if (staffcode33.Trim() != "")
            {
                if (other_staff_code1 == "")
                {
                    other_staff_code1 = staffcode33;
                }
                else
                {
                    other_staff_code1 = other_staff_code1 + "," + staffcode33;
                }
            }
            string staffcode44 = Convert.ToString(staffhash[Convert.ToString(namesplit3)]);
            if (staffcode44.Trim() != "")
            {
                if (other_staff_code1 == "")
                {
                    other_staff_code1 = staffcode44;
                }
                else
                {
                    other_staff_code1 = other_staff_code1 + "," + staffcode44;
                }
            }
            //added by kowshika
            string tapalcode4 = "";
            if (txt_rcptno.Text.Trim() != "")
            {

                tapalcode4 = txt_rcptno.Text;

            }


            DateTime dt = new DateTime();
            string subject = Convert.ToString(txt_othr_sub.Text);
            string refno = Convert.ToString(txt_othr_refno.Text);
            string name = Convert.ToString(txt_othr_name.Text);
            string addr = Convert.ToString(txt_othr_add.Text);
            string street = Convert.ToString(txt_othr_str.Text);
            string city = Convert.ToString(txt_othr_city.Text);
            string state = Convert.ToString(txt_othr_state.Text);
            string country = Convert.ToString(txt_othr_county.Text);
            string phn_no = Convert.ToString(txt_othr_ph.Text);
            string mail = Convert.ToString(txt_othr_mail.Text);
            string getday = dt.ToString("MM/dd/yyyy");
            string hr = Convert.ToString(ddl_othrhr.SelectedItem.Text);
            string min = Convert.ToString(ddl_othrmin.SelectedItem.Text);
            string day = Convert.ToString(ddl_othram.SelectedItem.Text);
            string chequeckddname = "";
            string chequeckddno = "";
            string chequeckddamt = "";
            string[] ay1;
            string date1 = "";
            string paymenttype = "";
            string payval = "";
            string bank = "";
            string bbranch = "";
            int amount = 0;
            if (cb_paymentothr.Checked == true)
            {
                bank = Convert.ToString(ddl_othr_bank.SelectedItem.Value);
                bbranch = Convert.ToString(txt_othr_banknranch.Text);
                payval = "1";
                if (rdo_othr_cheque.Checked == true)
                {
                    paymenttype = "0";
                    chequeckddname = Convert.ToString(txt_othr_chqname.Text);
                    chequeckddno = Convert.ToString(txt_othr_chqno.Text);
                    chequeckddamt = Convert.ToString(txt_othr_chqamt.Text);
                    int.TryParse(Convert.ToString(chequeckddamt), out amount);
                    ay1 = txt_othr_chqdate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();

                }
                else
                {
                    paymenttype = "1";
                    chequeckddname = Convert.ToString(txt_othr_ddname.Text);
                    chequeckddno = Convert.ToString(txt_othr_ddno.Text);
                    chequeckddamt = Convert.ToString(txt_othr_ddamt.Text);
                    ay1 = txt_othr_dddate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
            }
            else
            {
                payval = "0";
            }
            string time = hr + ":" + min + ":" + day;
            string appno = "";
            mcode_othr = name;
            if (cb_othr_dept.Checked == false && cb_othr_indi.Checked == false)
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = " Kindly select department and individual fields or Anyone";
            }
            else
            {
                if ((ddl_othr_doc.SelectedItem.Value == "Others" && txt_othr_doc.Text != "") || (ddl_othr_doc.SelectedItem.Value != "Others"))
                {
                    if ((ddl_othr_delivry.SelectedItem.Value == "Others" && txt_othr_delivry.Text != "") || (ddl_othr_delivry.SelectedItem.Value != "Others"))
                    {
                        others_save();
                        uploadvalue = Convert.ToString(txt_othr_name.Text);
                        string query = "";
                        if (cb_paymentothr.Checked == true)
                        {
                            query = "insert into TapalInward (MemType,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,FromName,FromAdd1,FromCity,FromState,FromCountry,FromPhoneNo,FromMailID,FromStreet,type,ViewStatus,SendNote,ispayment,DDNo,DDName,DDDate,Paymentamount,PaymentType,BankCode,BranchName,usercode,TapalCode)values('" + menutype + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + other_dept_code + "','" + other_dept_code1 + "','" + other_staff_code + "','" + other_staff_code1 + "','" + name + "','" + addr + "','" + city + "','" + state + "','" + country + "','" + phn_no + "','" + mail + "','" + street + "','" + type + "','0','0','" + payval + "','" + chequeckddno + "','" + chequeckddname + "','" + date1 + "','" + amount + "','" + paymenttype + "','" + bank_name + "','" + bbranch + "','" + usercode + "' ,'" + tapalcode4 + "')";
                            d2.update_method_wo_parameter(query, "Text"); //modified by kowshika
                        }
                        else
                        {
                            query = "insert into TapalInward (MemType,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,FromName,FromAdd1,FromCity,FromState,FromCountry,FromPhoneNo,FromMailID,FromStreet,type,ViewStatus,SendNote,ispayment,usercode,TapalCode)values('" + menutype + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + other_dept_code + "','" + other_dept_code1 + "','" + other_staff_code + "','" + other_staff_code1 + "','" + name + "','" + addr + "','" + city + "','" + state + "','" + country + "','" + phn_no + "','" + mail + "','" + street + "','" + type + "','0','0','" + payval + "','" + usercode + "' ,'" + tapalcode4 + "')";
                            d2.update_method_wo_parameter(query, "Text");
                        }
                        filepk = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                        upload_othr();
                        string rep_no = generateReceiptNo();
                        string charge = Convert.ToString(txt_othersothr.Text);
                        if (charge != "")
                        {
                            if (hdsave != "" && ledsave != "")
                            {
                                string finYearid = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + collegecode1 + "");
                                string ap_no = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                                string charges_query = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,TransType,FinYearFK) values('" + date + "','" + time + "','" + rep_no + "','7','" + ap_no + "','" + hdsave + "','" + ledsave + "','0','" + charge + "','0','1','2','" + finYearid + "')";
                                d2.update_method_wo_parameter(charges_query, "Text");
                                #region Update Receipt No
                                string lastRecptNo = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                                lastRecptNo = rep_no.Replace(lastRecptNo, "");
                                string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
                                int save = d2.update_method_wo_parameter(updateRecpt, "Text");
                                #endregion
                                if (save != 0)
                                {
                                    imgdiv2.Visible = true;
                                    lblalerterr.Text = "Saved Successfully";
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lblalerterr.Text = "Kindly Select Ledger & Header";
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lblalerterr.Text = "Saved Successfully";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "Fill The Delivery Mode";
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Fill The Document Priority";
                }
                //}
            }
            loaddelivery();
            loaddoc();
            doc();
            delivery();
            rb_othr_out.Checked = false;
            rb_othr_in.Checked = true;
            timevalue();
            td_othr.BgColor = "#c4c4c4";
            td_indi.BgColor = "white";
            td_mag.BgColor = "white";
            td_comp.BgColor = "white";
            td_par.BgColor = "white";
            td_staff.BgColor = "white";
            td_stud.BgColor = "white";
            txt_othersothr.Text = "";
            cb_othr_indi.Checked = false;
            div_other_indi.Attributes.Add("style", "display:none");
        }
        catch
        {
        }
    }
    public void others_save()
    {
        string VendorCode = "";
        string statecode = "";
        string countrycode = "";
        string state = Convert.ToString(txt_othr_state.Text);
        statecode = subjectcode("state", state);
        string country = Convert.ToString(txt_othr_county.Text);
        countrycode = subjectcode("coun", country);
        string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=7 and VendorCompName like '" + txt_othr_name.Text + "%'");
        if (VenCode != "" && VenCode != null && VenCode != "0")
        {
            VendorCode = VenCode;
        }
        else
        {
            VendorCodeGen();
            VendorCode = Session["VendorCode"].ToString();
            string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + txt_othr_name.Text + "' and VendorType='7') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + txt_othr_name.Text + "',VendorType='7',VendorAddress='" + txt_othr_add.Text + "',VendorStreet='" + txt_othr_str.Text + "',VendorCity='" + txt_othr_city.Text + "',VendorPin='" + txt_othr_pin.Text + "',VendorName='" + txt_othr_pname.Text + "',VendorPhoneNo='" + txt_othr_ph.Text + "',VendorEmailID='" + txt_othr_mail.Text + "',VendorCountry='" + countrycode + "', VendorState='" + statecode + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + txt_othr_name.Text + "' and VendorType='7' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorState,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry) values('" + VendorCode + "','" + txt_othr_name.Text + "','7','" + txt_othr_add.Text + "','" + txt_othr_str.Text + "','" + txt_othr_city.Text + "','" + txt_othr_pin.Text + "','" + statecode + "','" + txt_othr_pname.Text + "','" + txt_othr_ph.Text + "','" + txt_othr_mail.Text + "','" + countrycode + "')";
            int vc = d2.update_method_wo_parameter(venmst, "TEXT");
        }
    }
    [WebMethod]
    public static otherss[] othersdetail(string Name)
    {
        string data = string.Empty;
        List<otherss> details = new List<otherss>();
        try
        {
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string vendorpk = d21.GetFunction("select VendorPK from CO_VendorMaster where VendorCompName like '" + Name + "%'");
            string query = "select VendorCompName,VendorAddress,VendorStreet,VendorCountry,VendorCity,VendorState,VendorPin,VendorState,VendorName,VendorEmailID,VendorPhoneNo from CO_VendorMaster where VendorPK='" + vendorpk + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    otherss s = new otherss();
                    string statecode = ds1.Tables[0].Rows[i]["VendorState"].ToString();
                    string countrycode = ds1.Tables[0].Rows[i]["VendorCountry"].ToString();
                    string statename = d21.GetFunction("select textval from TextValTable where  TextCriteria='state' and TextCode='" + statecode + "' ");
                    string countryname = d21.GetFunction("select textval from TextValTable where  TextCriteria='coun' and TextCode='" + countrycode + "' ");
                    s.Name = ds1.Tables[0].Rows[i]["VendorCompName"].ToString();
                    s.Per_Name = ds1.Tables[0].Rows[i]["VendorName"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["VendorAddress"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["VendorStreet"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["VendorCity"].ToString();
                    s.State = statename;
                    s.Country = countryname;
                    s.Pin = ds1.Tables[0].Rows[i]["VendorPin"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["VendorPhoneNo"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["VendorEmailID"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    [WebMethod]
    public static otherss[] othersdetailname(string Name)
    {
        string data = string.Empty;
        List<otherss> details = new List<otherss>();
        try
        {
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string vendorpk = d21.GetFunction("select VendorPK from CO_VendorMaster where VendorName like '" + Name + "%'");
            string query = "select VendorCompName,VendorAddress,VendorCountry,VendorStreet,VendorCity,VendorState,VendorPin,VendorState,VendorName,VendorEmailID,VendorPhoneNo from CO_VendorMaster where VendorPK='" + vendorpk + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    otherss s = new otherss();
                    string statecode = ds1.Tables[0].Rows[i]["VendorState"].ToString();
                    string countrycode = ds1.Tables[0].Rows[i]["VendorCountry"].ToString();
                    string statename = d21.GetFunction("select textval from TextValTable where  TextCriteria='state' and TextCode='" + statecode + "' ");
                    string countryname = d21.GetFunction("select textval from TextValTable where  TextCriteria='coun' and TextCode='" + countrycode + "' ");
                    s.Name = ds1.Tables[0].Rows[i]["VendorCompName"].ToString();
                    s.Per_Name = ds1.Tables[0].Rows[i]["VendorName"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["VendorAddress"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["VendorStreet"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["VendorCity"].ToString();
                    s.State = statename;
                    s.Country = countryname;
                    s.Pin = ds1.Tables[0].Rows[i]["VendorPin"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["VendorPhoneNo"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["VendorEmailID"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    public class otherss
    {
        public string Name { get; set; }
        public string Per_Name { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string MailId { get; set; }
    }
    public void btnother_clear_Click(object sender, EventArgs e)
    {
    }

    public void btnindi_save_Click(object sender, EventArgs e)
    {
        try
        {
            string doc_prty = "";
            string delivery_mode = "";
            dept();
            staffcodevalue();
            div_college.Attributes.Add("style", "display:block");
            div_individu.Attributes.Add("style", "display:block");
            div_stud.Attributes.Add("style", "display:none");
            div_staff.Attributes.Add("style", "display:none");
            div_company.Attributes.Add("style", "display:none");
            div_parent.Attributes.Add("style", "display:none");
            div_magazine.Attributes.Add("style", "display:none");
            div_others.Attributes.Add("style", "display:none");
            if (ddl_ind_doc.SelectedItem.Value != "Select")
            {
                if (ddl_ind_doc.SelectedItem.Value != "Others")
                {
                    doc_prty = Convert.ToString(ddl_ind_doc.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_ind_doc1.Text);
                    doc_prty = subjectcodenew("TAPri", doc_prty1);
                }
            }
            if (ddl_ind_delivry.SelectedItem.Value != "Select")
            {
                if (ddl_ind_delivry.SelectedItem.Value != "Others")
                {
                    delivery_mode = Convert.ToString(ddl_ind_delivry.SelectedItem.Value);
                }
                else
                {
                    string delivery_mode1 = Convert.ToString(txt_ind_deliv1.Text);
                    delivery_mode = subjectcodenew("TASta", delivery_mode1);
                }
            }
            int menutype = 5;
            if (rb_indiv_in.Checked == true)
            {
                type = 0;
            }
            else
            {
                type = 1;
            }
            string[] ay = txt_ind_date.Text.Split('/');
            string date = ay[1].ToString() + "/" + ay[0].ToString() + "/" + ay[2].ToString();
            if (cb_ind_dept.Checked == true && cb_ind_indi.Checked == true)
            {
                value = 3;
                div_indiv_dept.Attributes.Add("style", "display:block");
                div_indiv_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_ind_indi.Checked == true)
            {
                value = 2;
                div_indiv_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_ind_dept.Checked == true)
            {
                value = 1;
                div_indiv_dept.Attributes.Add("style", "display:block");
            }
            string department_code1 = Convert.ToString(txt_ind_depto.Text);
            string department_code2 = Convert.ToString(txt_ind_depto1.Text);
            string department_code3 = Convert.ToString(txt_ind_cc.Text);
            string department_code4 = Convert.ToString(txt_ind_cc1.Text);
            string staff_code1 = Convert.ToString(txt_ind_indivto.Text);
            string[] split = staff_code1.Split('-');
            string namesplit = split[0];
            string staff_code2 = Convert.ToString(txt_ind_indiv1.Text);
            string[] split1 = staff_code2.Split('-');
            string namesplit1 = split1[0];
            string staff_code3 = Convert.ToString(txt_ind_indivcc.Text);
            string[] split2 = staff_code3.Split('-');
            string namesplit2 = split2[0];
            string staff_code4 = Convert.ToString(txt_ind_indivcc1.Text);
            string[] split3 = staff_code4.Split('-');
            string namesplit3 = split3[0];
            string deptcode1 = Convert.ToString(depthash[Convert.ToString(department_code1)]);
            if (deptcode1.Trim() != "")
            {
                if (individual_dept_code == "")
                {
                    individual_dept_code = deptcode1;
                }
                else
                {
                    individual_dept_code = individual_dept_code + "," + deptcode1;
                }
            }
            string deptcode2 = Convert.ToString(depthash[Convert.ToString(department_code2)]);
            if (deptcode2.Trim() != "")
            {
                if (individual_dept_code == "")
                {
                    individual_dept_code = deptcode2;
                }
                else
                {
                    individual_dept_code = individual_dept_code + "," + deptcode2;
                }
            }
            string deptcode3 = Convert.ToString(depthash[Convert.ToString(department_code3)]);
            if (deptcode3.Trim() != "")
            {
                if (individual_dept_code1 == "")
                {
                    individual_dept_code1 = deptcode3;
                }
                else
                {
                    individual_dept_code1 = individual_dept_code1 + "," + deptcode3;
                }
            }
            string deptcode4 = Convert.ToString(depthash[Convert.ToString(department_code4)]);
            if (deptcode4.Trim() != "")
            {
                if (individual_dept_code1 == "")
                {
                    individual_dept_code1 = deptcode4;
                }
                else
                {
                    individual_dept_code1 = individual_dept_code1 + "," + deptcode4;
                }
            }
            string staffcode11 = Convert.ToString(staffhash[Convert.ToString(namesplit)]);
            if (staffcode11.Trim() != "")
            {
                if (individual_staff_code == "")
                {
                    individual_staff_code = staffcode11;
                }
                else
                {
                    individual_staff_code = individual_staff_code + "," + staffcode11;
                }
            }
            string staffcode22 = Convert.ToString(staffhash[Convert.ToString(namesplit1)]);
            if (staffcode22.Trim() != "")
            {
                if (individual_staff_code == "")
                {
                    individual_staff_code = staffcode22;
                }
                else
                {
                    individual_staff_code = individual_staff_code + "," + staffcode22;
                }
            }
            string staffcode33 = Convert.ToString(staffhash[Convert.ToString(namesplit2)]);
            if (staffcode33.Trim() != "")
            {
                if (individual_staff_code1 == "")
                {
                    individual_staff_code1 = staffcode33;
                }
                else
                {
                    individual_staff_code1 = individual_staff_code1 + "," + staffcode33;
                }
            }
            string staffcode44 = Convert.ToString(staffhash[Convert.ToString(namesplit3)]);
            if (staffcode44.Trim() != "")
            {
                if (individual_staff_code1 == "")
                {
                    individual_staff_code1 = staffcode44;
                }
                else
                {
                    individual_staff_code1 = individual_staff_code1 + "," + staffcode44;
                }
            }
            DateTime dt = new DateTime();
            string subject = Convert.ToString(txt_ind_sub.Text);
            string refno = Convert.ToString(txt_ind_refno.Text);
            string getday = dt.ToString("MM/dd/yyyy");
            string hr = Convert.ToString(ddl_indhr.SelectedItem.Text);
            string min = Convert.ToString(ddlindmin.SelectedItem.Text);
            string day = Convert.ToString(ddlinam.SelectedItem.Text);
            string time = hr + ":" + min + ":" + day;
            string companyname = Convert.ToString(txt_ind_cname.Text);
            string addrs = Convert.ToString(txt_ind_add.Text);
            string street = Convert.ToString(txt_ind_str.Text);
            string city = Convert.ToString(txt_ind_city.Text);
            string state = Convert.ToString(txt_ind_state.Text);
            string country = Convert.ToString(txt_ind_county.Text);
            string phn_no = Convert.ToString(txt_ind_ph.Text);
            string mail = Convert.ToString(txt_ind_mail.Text);
            string appno = "";
            string chequeckddname = "";
            string chequeckddno = "";
            string chequeckddamt = "";
            string[] ay1;
            string date1 = "";
            string paymenttype = "";
            string payval = "";
            string bank = "";
            string bbranch = "";
            if (cb_paymentindi.Checked == true)
            {
                payval = "1";
                bank = Convert.ToString(dd_indi_bank.SelectedItem.Value);
                bbranch = Convert.ToString(txt_indi_bankbranch.Text);
                if (rdb_indi_cheque.Checked == true)
                {
                    paymenttype = "0";
                    chequeckddname = Convert.ToString(txt_indi_chqname.Text);
                    chequeckddno = Convert.ToString(txt_indi_chqno.Text);
                    chequeckddamt = Convert.ToString(txt_indi_chqamt.Text);
                    ay1 = txt_indi_chqdate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
                else
                {
                    paymenttype = "1";
                    chequeckddname = Convert.ToString(txt_comddname.Text);
                    chequeckddno = Convert.ToString(txt_indiddno.Text);
                    chequeckddamt = Convert.ToString(txt_indi_dd_amt.Text);
                    ay1 = txt_indidddate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
            }
            else
            {
                payval = "0";
            }
            string name = Convert.ToString(txt_ind_cname.Text);
            mcode_indi = name;
            if (cb_ind_dept.Checked == false && cb_ind_indi.Checked == false)
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = " Kindly select department and individual fields or Anyone";
            }
            else
            {
                if ((ddl_ind_doc.SelectedItem.Value == "Others" && txt_ind_doc1.Text != "") || (ddl_ind_doc.SelectedItem.Value != "Others"))
                {
                    if ((ddl_ind_delivry.SelectedItem.Value == "Others" && txt_ind_deliv1.Text != "") || (ddl_ind_delivry.SelectedItem.Value != "Others"))
                    {
                        string query = "";
                        indi_save();
                        if (cb_paymentindi.Checked == true)
                        {
                            query = "insert into TapalInward (MemType,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,FromName,FromAdd1,FromCity,FromState,FromCountry,FromPhoneNo,FromMailID,FromStreet,type,ViewStatus,SendNote,ispayment,DDNo,DDName,DDDate,Paymentamount,PaymentType,BankCode,BranchName)values('" + menutype + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + individual_dept_code + "','" + individual_dept_code1 + "','" + individual_staff_code + "','" + individual_staff_code1 + "','" + companyname + "','" + addrs + "','" + city + "','" + state + "','" + country + "','" + phn_no + "','" + mail + "','" + street + "','" + type + "','0','0','" + payval + "','" + chequeckddno + "','" + chequeckddname + "','" + date1 + "','" + chequeckddamt + "','" + paymenttype + "','" + bank + "','" + bbranch + "')";
                            d2.update_method_wo_parameter(query, "Text");
                        }
                        else
                        {
                            query = "insert into TapalInward (MemType,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,FromName,FromAdd1,FromCity,FromState,FromCountry,FromPhoneNo,FromMailID,FromStreet,type,ViewStatus,SendNote,ispayment)values('" + menutype + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + individual_dept_code + "','" + individual_dept_code1 + "','" + individual_staff_code + "','" + individual_staff_code1 + "','" + companyname + "','" + addrs + "','" + city + "','" + state + "','" + country + "','" + phn_no + "','" + mail + "','" + street + "','" + type + "','0','0','" + payval + "')";
                            d2.update_method_wo_parameter(query, "Text");
                        }
                        filepk = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                        uploadvalue = Convert.ToString(txt_ind_cname.Text);
                        upload_indi();
                        string rep_no = generateReceiptNo();
                        string charge = Convert.ToString(txt_chargesindi.Text);
                        if (charge != "")
                        {
                            if (hdsave != "" && ledsave != "")
                            {
                                string finYearid = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + collegecode1 + "");
                                string ap_no = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                                string charges_query = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,TransType,FinYearFK) values('" + date + "','" + time + "','" + rep_no + "','7','" + ap_no + "','" + hdsave + "','" + ledsave + "','0','" + charge + "','0','1','2','" + finYearid + "')";
                                d2.update_method_wo_parameter(charges_query, "Text");
                                #region Update Receipt No
                                string lastRecptNo = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                                lastRecptNo = rep_no.Replace(lastRecptNo, "");
                                string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
                                int save = d2.update_method_wo_parameter(updateRecpt, "Text");
                                #endregion
                                if (save != 0)
                                {
                                    imgdiv2.Visible = true;
                                    lblalerterr.Text = "Saved Successfully";
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lblalerterr.Text = "Kindly Select Header & Ledger";
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lblalerterr.Text = "Saved Successfully";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "Fill The Delivery Mode";
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Fill The Document Priority";
                }
                //}
            }
            loaddoc();
            loaddelivery();
            doc();
            delivery();
            rb_indiv_out.Checked = false;
            rb_indiv_in.Checked = true;
            timevalue();
            td_indi.BgColor = "#c4c4c4";
            td_mag.BgColor = "white";
            td_othr.BgColor = "white";
            td_comp.BgColor = "white";
            td_par.BgColor = "white";
            td_staff.BgColor = "white";
            td_stud.BgColor = "white";
            cb_ind_indi.Checked = false;
            div_indiv_indi.Attributes.Add("style", "display:none");
            txt_chargesindi.Text = "";
        }
        catch
        {
        }
    }

    public void indi_save()
    {
        string VendorCode = "";
        string statecode = "";
        string countrycode = "";
        string state = Convert.ToString(txt_ind_state.Text);
        statecode = subjectcode("state", state);
        string country = Convert.ToString(txt_ind_county.Text);
        countrycode = subjectcode("coun", country);
        string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=5 and VendorCompName like '" + txt_ind_cname.Text + "%'");
        if (VenCode != "" && VenCode != null && VenCode != "0")
        {
            VendorCode = VenCode;
        }
        else
        {
            VendorCodeGen();
            VendorCode = Session["VendorCode"].ToString();
            string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + txt_ind_cname.Text + "' and VendorType='5') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + txt_ind_cname.Text + "',VendorType='5',VendorAddress='" + txt_com_add.Text + "',VendorStreet='" + txt_com_str.Text + "',VendorCity='" + txt_com_city.Text + "',VendorPin='" + txt_com_pin.Text + "',VendorName='" + txt_ind_pname.Text + "',VendorPhoneNo='" + txt_ind_ph.Text + "',VendorEmailID='" + txt_ind_mail.Text + "',VendorCountry='" + countrycode + "', VendorState='" + statecode + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + txt_ind_cname.Text + "' and VendorType='5' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorState,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry) values('" + VendorCode + "','" + txt_ind_cname.Text + "','5','" + txt_ind_add.Text + "','" + txt_ind_str.Text + "','" + txt_ind_city.Text + "','" + txt_ind_pin.Text + "','" + statecode + "','" + txt_ind_pname.Text + "','" + txt_ind_ph.Text + "','" + txt_ind_mail.Text + "','" + countrycode + "')";
            int vc = d2.update_method_wo_parameter(venmst, "TEXT");
        }
        Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
        string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + txt_ind_pname.Text + "' and VenContactDesig='" + txt_ind_des.Text + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + txt_ind_pname.Text + "',VenContactDesig='" + txt_ind_des.Text + "',VendorPhoneNo='" + txt_ind_ph.Text + "',VendorEmail='" + txt_ind_mail.Text + "' where VendorFK='" + VendorFK + "' and VenContactName='" + txt_ind_pname.Text + "' and VenContactDesig='" + txt_ind_des.Text + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VenContactDesig,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + txt_ind_pname.Text + "','" + txt_ind_des.Text + "','" + txt_ind_ph.Text + "','" + txt_ind_mail.Text + "')";
        int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");
    }

    [WebMethod]
    public static individual[] individualdetail(string Name)
    {
        string data = string.Empty;
        List<individual> details = new List<individual>();
        try
        {
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string vendorpk = d21.GetFunction("select VendorPK from CO_VendorMaster where VendorCompName like '" + Name + "%'");
            // string query = "select v.VendorCode,v.VendorCompName,v.VendorState,v.VendorCountry,v.VendorAddress,v.VendorCity,v.VendorStreet,v.VendorDist,v.VendorState,v.VendorPin,vc.VenContactName,vc.VenContactDept,vc.VenContactDesig,vc.VendorPhoneNo,vc.VendorExtNo,vc.VendorMobileNo,vc.VendorEmail from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and v.VendorPK='" + vendorpk + "'";
            string query = "select VendorCode,VendorCompName,VendorName,VenContactDesig ,VendorState,c.VendorPhoneNo,VendorCountry,VendorEmailID,VendorAddress,VendorCity,VendorStreet,VendorDist,VendorState,VendorPin from CO_VendorMaster c,IM_VendorContactMaster i where VendorPK='" + vendorpk + "' and VendorPK=VendorFK ";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    individual s = new individual();
                    string statecode = ds1.Tables[0].Rows[i]["VendorState"].ToString();
                    string countrycode = ds1.Tables[0].Rows[i]["VendorCountry"].ToString();
                    string statename = d21.GetFunction("select textval from TextValTable where  TextCriteria='state' and TextCode='" + statecode + "' ");
                    string countryname = d21.GetFunction("select textval from TextValTable where  TextCriteria='coun' and TextCode='" + countrycode + "' ");
                    s.Name = ds1.Tables[0].Rows[i]["VendorCompName"].ToString();
                    s.Per_Name = ds1.Tables[0].Rows[i]["VendorName"].ToString();
                    s.Designation = ds1.Tables[0].Rows[i]["VenContactDesig"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["VendorAddress"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["VendorStreet"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["VendorCity"].ToString();
                    s.State = statename;
                    s.Country = countryname;
                    s.Pin = ds1.Tables[0].Rows[i]["VendorPin"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["VendorPhoneNo"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["VendorEmailID"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    public class individual
    {
        public string Name { get; set; }
        public string Per_Name { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string MailId { get; set; }
    }
    public void btnindi_clear_Click(object sender, EventArgs e)
    {
    }

    public void btnmag_save_Click(object sender, EventArgs e)
    {
        try
        {
            td_mag.BgColor = "#c4c4c4";
            string doc_prty = "";
            string delivery_mode = "";
            dept();
            staffcodevalue();
            div_college.Attributes.Add("style", "display:block");
            div_magazine.Attributes.Add("style", "display:block");
            div_company.Attributes.Add("style", "display:none");
            div_staff.Attributes.Add("style", "display:none");
            div_stud.Attributes.Add("style", "display:none");
            div_parent.Attributes.Add("style", "display:none");
            div_individu.Attributes.Add("style", "display:none");
            div_others.Attributes.Add("style", "display:none");
            if (ddl_magz_doc.SelectedItem.Value != "Select")
            {
                if (ddl_magz_doc.SelectedItem.Value != "Others")
                {
                    doc_prty = Convert.ToString(ddl_magz_doc.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_magz_doc.Text);
                    doc_prty = subjectcodenew("TAPri", doc_prty1);
                }
            }
            if (ddl_magz_delivry.SelectedItem.Value != "Select")
            {
                if (ddl_magz_delivry.SelectedItem.Value != "Others")
                {
                    delivery_mode = Convert.ToString(ddl_magz_delivry.SelectedItem.Value);
                }
                else
                {
                    string delivery_mode1 = Convert.ToString(txt_magz_deliv1.Text);
                    delivery_mode = subjectcodenew("TASta", delivery_mode1);
                }
            }
            if (rb_magz_in.Checked == true)
            {
                type = 0;
            }
            else
            {
                type = 1;
            }
            int menutype = 6;
            string[] ay = txt_magzdate.Text.Split('/');
            string date = ay[1].ToString() + "/" + ay[0].ToString() + "/" + ay[2].ToString();
            if (cb_magz_dept.Checked == true && cb_magz_indi.Checked == true)
            {
                value = 3;
                div_magz_dept.Attributes.Add("style", "display:block");
                div_magz_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_magz_indi.Checked == true)
            {
                value = 2;
                div_magz_indi.Attributes.Add("style", "display:block");
            }
            else if (cb_magz_dept.Checked == true)
            {
                value = 1;
                div_magz_dept.Attributes.Add("style", "display:block");
            }
            string department_code1 = Convert.ToString(txt_magz_depto.Text);
            string department_code2 = Convert.ToString(txt_magz_depto1.Text);
            string department_code3 = Convert.ToString(txt_magz_cc.Text);
            string department_code4 = Convert.ToString(txt_magz_cc1.Text);
            string staff_code1 = Convert.ToString(txt_magz_indi.Text);
            string[] split = staff_code1.Split('-');
            string namesplit = split[0];
            string staff_code2 = Convert.ToString(txt_magz_indi1.Text);
            string[] split1 = staff_code2.Split('-');
            string namesplit1 = split1[0];
            string staff_code3 = Convert.ToString(txt_magz_cc2.Text);
            string[] split2 = staff_code3.Split('-');
            string namesplit2 = split2[0];
            string staff_code4 = Convert.ToString(txt_magz_cc3.Text);
            string[] split3 = staff_code4.Split('-');
            string namesplit3 = split3[0];
            string deptcode1 = Convert.ToString(depthash[Convert.ToString(department_code1)]);
            if (deptcode1.Trim() != "")
            {
                if (mag_dept_code == "")
                {
                    mag_dept_code = deptcode1;
                }
                else
                {
                    mag_dept_code = mag_dept_code + "," + deptcode1;
                }
            }
            string deptcode2 = Convert.ToString(depthash[Convert.ToString(department_code2)]);
            if (deptcode2.Trim() != "")
            {
                if (mag_dept_code == "")
                {
                    mag_dept_code = deptcode2;
                }
                else
                {
                    mag_dept_code = mag_dept_code + "," + deptcode2;
                }
            }
            string deptcode3 = Convert.ToString(depthash[Convert.ToString(department_code3)]);
            if (deptcode3.Trim() != "")
            {
                if (mag_dept_code1 == "")
                {
                    mag_dept_code1 = deptcode3;
                }
                else
                {
                    mag_dept_code1 = mag_dept_code1 + "," + deptcode3;
                }
            }
            string deptcode4 = Convert.ToString(depthash[Convert.ToString(department_code4)]);
            if (deptcode4.Trim() != "")
            {
                if (mag_dept_code1 == "")
                {
                    mag_dept_code1 = deptcode4;
                }
                else
                {
                    mag_dept_code1 = mag_dept_code1 + "," + deptcode4;
                }
            }
            string staffcode11 = Convert.ToString(staffhash[Convert.ToString(namesplit)]);
            if (staffcode11.Trim() != "")
            {
                if (mag_staff_code == "")
                {
                    mag_staff_code = staffcode11;
                }
                else
                {
                    mag_staff_code = mag_staff_code + "," + staffcode11;
                }
            }
            string staffcode22 = Convert.ToString(staffhash[Convert.ToString(namesplit1)]);
            if (staffcode22.Trim() != "")
            {
                if (mag_staff_code == "")
                {
                    mag_staff_code = staffcode22;
                }
                else
                {
                    mag_staff_code = mag_staff_code + "," + staffcode22;
                }
            }
            string staffcode33 = Convert.ToString(staffhash[Convert.ToString(namesplit2)]);
            if (staffcode33.Trim() != "")
            {
                if (mag_staff_code1 == "")
                {
                    mag_staff_code1 = staffcode33;
                }
                else
                {
                    mag_staff_code1 = mag_staff_code1 + "," + staffcode33;
                }
            }
            string staffcode44 = Convert.ToString(staffhash[Convert.ToString(namesplit3)]);
            if (staffcode44.Trim() != "")
            {
                if (mag_staff_code1 == "")
                {
                    mag_staff_code1 = staffcode44;
                }
                else
                {
                    mag_staff_code1 = mag_staff_code1 + "," + staffcode44;
                }
            }
            DateTime dt = new DateTime();
            string subject = Convert.ToString(txt_magz_sub.Text);
            string refno = Convert.ToString(txt_magz_refno.Text);
            string mag_name = Convert.ToString(txt_magzname.Text);
            string mag_type = Convert.ToString(txt_magztype.Text);
            string addr = Convert.ToString(txt_magz_add.Text);
            string street = Convert.ToString(txt_magz_str.Text);
            string city = Convert.ToString(txt_magz_city.Text);
            string state = Convert.ToString(txt_magz_state.Text);
            string country = Convert.ToString(txt_magz_county.Text);
            string phn_no = Convert.ToString(txt_magz_ph.Text);
            string mail = Convert.ToString(txt_magz_mail.Text);
            string getday = dt.ToString("MM/dd/yyyy");
            string hr = Convert.ToString(ddl_maghr.SelectedItem.Text);
            string min = Convert.ToString(ddl_magmin.SelectedItem.Text);
            string day = Convert.ToString(ddl_magam.SelectedItem.Text);
            string time = hr + ":" + min + ":" + day;
            string appno = "";
            string chequeckddname = "";
            string chequeckddno = "";
            string chequeckddamt = "";
            string[] ay1;
            string date1 = "";
            string paymenttype = "";
            string payval = "";
            string bank = "";
            string bbranch = "";
            if (cb_paymentmag.Checked == true)
            {
                payval = "1";
                bank = Convert.ToString(ddl_magbank.SelectedItem.Value);
                bbranch = Convert.ToString(txt_magbankbrabch.Text);
                if (rdb_mag_chwque.Checked == true)
                {
                    paymenttype = "0";
                    chequeckddname = Convert.ToString(txt_magchqname.Text);
                    chequeckddno = Convert.ToString(txt_magchno.Text);
                    chequeckddamt = Convert.ToString(txt_mag_chqamt.Text);
                    ay1 = txt_mag_chqdate.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
                else
                {
                    paymenttype = "1";
                    chequeckddname = Convert.ToString(txt_magddname.Text);
                    chequeckddno = Convert.ToString(txt_magddno.Text);
                    chequeckddamt = Convert.ToString(txt_magddamt.Text);
                    ay1 = txt_magdd_date.Text.Split('/');
                    date1 = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                }
            }
            else
            {
                payval = "0";
            }
            mcode_mag = mag_name;
            if (cb_magz_dept.Checked == false && cb_magz_indi.Checked == false)
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = " Kindly select department and individual fields or Anyone";
            }
            else
            {
                if ((ddl_magz_doc.SelectedItem.Value == "Others" && txt_magz_doc.Text != "") || (ddl_magz_doc.SelectedItem.Value != "Others"))
                {
                    if ((ddl_magz_delivry.SelectedItem.Value == "Others" && txt_magz_deliv1.Text != "") || (ddl_magz_delivry.SelectedItem.Value != "Others"))
                    {
                        mag_save();
                        string query = "";
                        uploadvalue = Convert.ToString(txt_magzname.Text);
                        if (cb_paymentmag.Checked == true)
                        {
                            query = "insert into TapalInward (MemType,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,MagazineName,MagazineType,FromAdd1,FromCity,FromState,FromCountry,FromPhoneNo,FromMailID,FromStreet,type,ViewStatus,SendNote,ispayment,DDNo,DDName,DDDate,Paymentamount,PaymentType,BankCode,BranchName)values('" + menutype + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + mag_dept_code + "','" + mag_dept_code1 + "','" + mag_staff_code + "','" + mag_staff_code1 + "','" + mag_name + "','" + mag_type + "','" + addr + "','" + city + "','" + state + "','" + country + "','" + phn_no + "','" + mail + "','" + street + "','" + type + "','0','0','" + payval + "','" + chequeckddno + "','" + chequeckddname + "','" + date1 + "','" + chequeckddamt + "','" + paymenttype + "','" + bank + "','" + bbranch + "')";
                            d2.update_method_wo_parameter(query, "Text");
                        }
                        else
                        {
                            query = "insert into TapalInward (MemType,MoveTo,TapalSubject,Priority,RefNo,TapalType,TapalInwardDate,TapalInwardTime,ToDeptCode,ToCCDeptCode,individual_code,ccindividual_code,MagazineName,MagazineType,FromAdd1,FromCity,FromState,FromCountry,FromPhoneNo,FromMailID,FromStreet,type,ViewStatus,SendNote,ispayment)values('" + menutype + "','" + value + "','" + subject + "','" + doc_prty + "','" + refno + "','" + delivery_mode + "','" + date + "','" + time + "','" + mag_dept_code + "','" + mag_dept_code1 + "','" + mag_staff_code + "','" + mag_staff_code1 + "','" + mag_name + "','" + mag_type + "','" + addr + "','" + city + "','" + state + "','" + country + "','" + phn_no + "','" + mail + "','" + street + "','" + type + "','0','0','" + payval + "')";
                            d2.update_method_wo_parameter(query, "Text");
                        }
                        filepk = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                        upload_mag();
                        string rep_no = generateReceiptNo();
                        string charge = Convert.ToString(TextBox1.Text);
                        if (charge != "")
                        {
                            if (hdsave != "" && ledsave != "")
                            {
                                string finYearid = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + collegecode1 + "");
                                string ap_no = d2.GetFunction("select TapalMovementsID from TapalInward where TapalMovementsID=((select max(TapalMovementsID) from TapalInward))");
                                string charges_query = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,TransType,FinYearFK) values('" + date + "','" + time + "','" + rep_no + "','7','" + ap_no + "','" + hdsave + "','" + ledsave + "','0','" + charge + "','0','1','2','" + finYearid + "')";
                                d2.update_method_wo_parameter(charges_query, "Text");
                                #region Update Receipt No
                                string lastRecptNo = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                                lastRecptNo = rep_no.Replace(lastRecptNo, "");
                                string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
                                int save = d2.update_method_wo_parameter(updateRecpt, "Text");
                                #endregion
                                if (save != 0)
                                {
                                    imgdiv2.Visible = true;
                                    lblalerterr.Text = "Saved Successfully";
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lblalerterr.Text = "Kindly Select Header & Ledger";
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lblalerterr.Text = "Saved Successfully";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "Fill The Delivery Mode";
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Fill The Document Priority";
                }
                //}
            }
            loaddelivery();
            loaddoc();
            doc();
            delivery();
            rb_magz_out.Checked = false;
            rb_magz_in.Checked = true;
            timevalue();
            td_mag.BgColor = "#c4c4c4";
            td_indi.BgColor = "white";
            td_othr.BgColor = "white";
            td_comp.BgColor = "white";
            td_par.BgColor = "white";
            td_staff.BgColor = "white";
            td_stud.BgColor = "white";
            TextBox1.Text = "";
            cb_magz_indi.Checked = false;
            div_magz_indi.Attributes.Add("style", "display:none");
        }
        catch
        {
        }
    }

    public void mag_save()
    {
        string VendorCode = "";
        string statecode = "";
        string countrycode = "";
        string state = Convert.ToString(txt_magz_state.Text);
        statecode = subjectcode("state", state);
        string country = Convert.ToString(txt_magz_county.Text);
        countrycode = subjectcode("coun", country);
        string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=6 and VendorCompName like '" + txt_magzname.Text + "%'");
        if (VenCode != "" && VenCode != null && VenCode != "0")
        {
            VendorCode = VenCode;
        }
        else
        {
            VendorCodeGen();
            VendorCode = Session["VendorCode"].ToString();
            string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + txt_magzname.Text + "' and VendorType='6') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + txt_magzname.Text + "',VendorType='6',VendorAddress='" + txt_magz_add.Text + "',VendorStreet='" + txt_magz_str.Text + "',VendorCity='" + txt_magz_city.Text + "',VendorPhoneNo='" + txt_magz_ph.Text + "',VendorEmailID='" + txt_magz_mail.Text + "',VendorPin='" + txt_pin.Text + "',TypeofMagazine='" + txt_magztype.Text + "',VendorCountry='" + countrycode + "', VendorState='" + statecode + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + txt_magzname.Text + "' and VendorType='6' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorState,TypeofMagazine,VendorPhoneNo,VendorEmailID,VendorCountry) values('" + VendorCode + "','" + txt_magzname.Text + "','6','" + txt_magz_add.Text + "','" + txt_magz_str.Text + "','" + txt_magz_city.Text + "','" + txt_pin.Text + "','" + statecode + "','" + txt_magztype.Text + "','" + txt_magz_ph.Text + "','" + txt_magz_mail.Text + "','" + countrycode + "')";
            int vc = d2.update_method_wo_parameter(venmst, "TEXT");
        }
    }
    [WebMethod]
    public static magazines[] magazinedetail(string Name)
    {
        string data = string.Empty;
        List<magazines> details = new List<magazines>();
        try
        {
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DAccess2 d21 = new DAccess2();
            Hashtable hat = new Hashtable();
            string vendorpk = d21.GetFunction("select VendorPK from CO_VendorMaster where VendorCompName like '" + Name + "%'");
            string query = "select VendorCompName,VendorAddress,VendorStreet,VendorCountry,VendorCity,VendorState,VendorPin,VendorState,TypeofMagazine,VendorEmailID,VendorPhoneNo from CO_VendorMaster where VendorPK='" + vendorpk + "'";
            ds1 = d21.select_method_wo_parameter(query, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    magazines s = new magazines();
                    string statecode = ds1.Tables[0].Rows[i]["VendorState"].ToString();
                    string countcode = ds1.Tables[0].Rows[i]["VendorCountry"].ToString();
                    string statename = d21.GetFunction("select textval from TextValTable where  TextCriteria='state' and TextCode='" + statecode + "' ");
                    string countryname = d21.GetFunction("select textval from TextValTable where  TextCriteria='coun' and TextCode='" + countcode + "' ");
                    s.Name = ds1.Tables[0].Rows[i]["VendorCompName"].ToString();
                    s.Per_Name = ds1.Tables[0].Rows[i]["TypeofMagazine"].ToString();
                    s.Address = ds1.Tables[0].Rows[i]["VendorAddress"].ToString();
                    s.Street = ds1.Tables[0].Rows[i]["VendorStreet"].ToString();
                    s.City = ds1.Tables[0].Rows[i]["VendorCity"].ToString();
                    s.State = statename;
                    s.Country = countryname;
                    s.Pin = ds1.Tables[0].Rows[i]["VendorPin"].ToString();
                    s.Phone = ds1.Tables[0].Rows[i]["VendorPhoneNo"].ToString();
                    s.MailId = ds1.Tables[0].Rows[i]["VendorEmailID"].ToString();
                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    public class magazines
    {
        public string Name { get; set; }
        public string Per_Name { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string MailId { get; set; }
    }
    public void btnmag_clear_Click(object sender, EventArgs e)
    {
    }
    [WebMethod]
    public static string CheckRollNo(string Roll_No)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Roll_No != "")
            {
                string query = "";
                if (oldcurrentcheck == "O")
                {
                    query = "select Roll_No from Registration where  DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No = '" + Roll_No + "'";//cc=1 and barath 19.04.17 cc=0 and
                }
                else
                {
                    query = "select Roll_No from Registration where  DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No = '" + Roll_No + "'";
                }
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "0";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    [WebMethod]
    public static string CheckRegNodet(string Roll_No)//barath 19.04.17
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Roll_No != "")
            {
                string query = "";
                if (oldcurrentcheck == "O")
                {
                    query = "select Roll_No from Registration where  DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No = '" + Roll_No + "'";//cc=1 and barath 19.04.17 cc=0 and
                }
                else
                {
                    query = "select Roll_No from Registration where  DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No = '" + Roll_No + "'";
                }
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "0";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    [WebMethod]
    public static string Checkcompany(string Name)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Name != "")
            {
                string vendorpk = dd.GetFunction("select VendorFK from IM_VendorContactMaster where VenContactName like '" + Name + "%'");
                string query = "select v.VendorCode,v.VendorCompName,v.VendorAddress,v.VendorCity,v.VendorStreet,v.VendorDist,v.VendorState,v.VendorPin,vc.VenContactName,vc.VenContactDept,vc.VenContactDesig,vc.VendorPhoneNo,vc.VendorExtNo,vc.VendorMobileNo,vc.VendorEmail from IM_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and v.VendorPK='" + vendorpk + "'";
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "0";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        cb_dept.Checked = true;
        cb_stf_dept.Checked = true;
        cb_par_dept.Checked = true;
        cb_com_dept.Checked = true;
        cb_ind_dept.Checked = true;
        cb_magz_dept.Checked = true;
        cb_othr_dept.Checked = true;

        txt_batch.Text = "";
        txt_degree.Text = "";
        txt_department.Text = "";
        txt_section.Text = "";
        txt_sem.Text = "";
        txt_stud_country.Text = "";
        txt_addrs.Text = "";
        txt_street.Text = "";
        txt_city.Text = "";
        txt_pin_stud.Text = "";
        txt_state.Text = "";
        txt_pho.Text = "";
        txt_mailid.Text = "";
        txt_studtype.Text = "";
        txt_dept_to.Text = "";
        txt_to1.Text = "";
        txt_dept_cc.Text = "";
        txt_cc1.Text = "";
        txt_indiv.Text = "";
        txt_indiv1.Text = "";
        txt_indiv_cc.Text = "";
        txt_cc2.Text = "";
        txt_subject.Text = "";
        txt_docpriv.Text = "";
        txt_delivery.Text = "";
        txt_bank.Text = "";
        ttx_refno.Text = "";
        txt_chargestud.Text = "";
        txt_chequename.Text = "";
        txt_chequenumb.Text = "";
        txt_chequeamt.Text = "";
        txt_ddname.Text = "";
        txt_ddno.Text = "";
        txt_ddamt.Text = "";
        txt_stud_sub.Text = "";
    }

    public void upload()
    {
        try
        {
            //bool savnotsflag = false;
            //if (fileupload.HasFile)
            //{
            //    if (fileupload.FileName.EndsWith(".jpg") || fileupload.FileName.EndsWith(".gif") || fileupload.FileName.EndsWith(".png") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".doc") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".docx") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".document") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".xlsx") || fileupload.FileName.EndsWith(".pdf") || fileupload.FileName.EndsWith(".ppt") || fileupload.FileName.EndsWith(".pptx"))
            //    {
            //        string fileName = Path.GetFileName(fileupload.PostedFile.FileName);
            //        string fileExtension = Path.GetExtension(fileupload.PostedFile.FileName);
            //        string documentType = string.Empty;
            //        switch (fileExtension)
            //        {
            //            case ".pdf":
            //                documentType = "application/pdf";
            //                break;
            //            case ".xls":
            //                documentType = "application/vnd.ms-excel";
            //                break;
            //            case ".xlsx":
            //                documentType = "application/vnd.ms-excel";
            //                break;
            //            case ".doc":
            //                documentType = "application/vnd.ms-word";
            //                break;
            //            case ".docx":
            //                documentType = "application/vnd.ms-word";
            //                break;
            //            case ".gif":
            //                documentType = "image/gif";
            //                break;
            //            case ".png":
            //                documentType = "image/png";
            //                break;
            //            case ".jpg":
            //                documentType = "image/jpg";
            //                break;
            //            case ".ppt":
            //                documentType = "application/vnd.ms-ppt";
            //                break;
            //            case ".pptx":
            //                documentType = "application/vnd.ms-pptx";
            //                break;
            //            case ".txt":
            //                documentType = "application/txt";
            //                break;
            //        }
            //        int fileSize = fileupload.PostedFile.ContentLength;
            //        //Create array and read the file into it
            //        byte[] documentBinary = new byte[fileSize];
            //        fileupload.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
            //        string date = DateTime.Now.ToString("MM/dd/yyyy");
            //        SqlCommand cmdnotes = new SqlCommand();
            //        // string fileid = uploadvalue + "@" + uploadtime;
            //        string fileid = uploadvalue;
            //        string tapalfilefk = filepk;
            //        //string fileid =  + "@" +  ;
            //        cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
            //        cmdnotes.CommandType = CommandType.Text;
            //        cmdnotes.Connection = ssql;
            //        SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
            //        DocName.Value = fileName.ToString();
            //        cmdnotes.Parameters.Add(DocName);
            //        SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
            //        Type.Value = documentType.ToString();
            //        cmdnotes.Parameters.Add(Type);
            //        SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
            //        uploadedDocument.Value = documentBinary;
            //        cmdnotes.Parameters.Add(uploadedDocument);
            //        SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
            //        uploadedDate.Value = date;
            //        cmdnotes.Parameters.Add(uploadedDate);
            //        SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
            //        uploaded_id.Value = fileid;
            //        cmdnotes.Parameters.Add(uploaded_id);
            //        SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
            //        uploaded_mtype.Value = mtype;
            //        cmdnotes.Parameters.Add(uploaded_mtype);
            //        SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
            //        uploaded_mcode.Value = mcode;
            //        cmdnotes.Parameters.Add(uploaded_mcode);
            //        SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
            //        uploaded_mPK.Value = tapalfilefk;
            //        cmdnotes.Parameters.Add(uploaded_mPK);
            //        ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
            //        ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
            //        ssql.Close();
            //        ssql.Open();
            //        int result = cmdnotes.ExecuteNonQuery();
            //        if (result > 0)
            //        {
            //            savnotsflag = true;
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
        }
    }

    public void upload_staff()
    {
        try
        {
            //int upload_memty_staff = 2;
            //bool savnotsflag = false;
            //if (file_uplod1.HasFile)
            //{
            //    if (FileUpload1.FileName.EndsWith(".jpg") || FileUpload1.FileName.EndsWith(".gif") || FileUpload1.FileName.EndsWith(".png") || FileUpload1.FileName.EndsWith(".txt") || FileUpload1.FileName.EndsWith(".doc") || FileUpload1.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".docx") || FileUpload1.FileName.EndsWith(".txt") || FileUpload1.FileName.EndsWith(".document") || FileUpload1.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".xlsx") || FileUpload1.FileName.EndsWith(".pdf") || FileUpload1.FileName.EndsWith(".ppt") || FileUpload1.FileName.EndsWith(".pptx"))
            //    {
            //        string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            //        string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
            //        string documentType = string.Empty;
            //        switch (fileExtension)
            //        {
            //            case ".pdf":
            //                documentType = "application/pdf";
            //                break;
            //            case ".xls":
            //                documentType = "application/vnd.ms-excel";
            //                break;
            //            case ".xlsx":
            //                documentType = "application/vnd.ms-excel";
            //                break;
            //            case ".doc":
            //                documentType = "application/vnd.ms-word";
            //                break;
            //            case ".docx":
            //                documentType = "application/vnd.ms-word";
            //                break;
            //            case ".gif":
            //                documentType = "image/gif";
            //                break;
            //            case ".png":
            //                documentType = "image/png";
            //                break;
            //            case ".jpg":
            //                documentType = "image/jpg";
            //                break;
            //            case ".ppt":
            //                documentType = "application/vnd.ms-ppt";
            //                break;
            //            case ".pptx":
            //                documentType = "application/vnd.ms-pptx";
            //                break;
            //            case ".txt":
            //                documentType = "application/txt";
            //                break;
            //        }
            //        int fileSize = FileUpload1.PostedFile.ContentLength;
            //        //Create array and read the file into it
            //        byte[] documentBinary = new byte[fileSize];
            //        FileUpload1.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
            //        string date = DateTime.Now.ToString("MM/dd/yyyy");
            //        SqlCommand cmdnotes = new SqlCommand();
            //        // string fileid = uploadvalue + "@" + uploadtime;
            //        string fileid = uploadvalue;
            //        string tapalfilefk = filepk;
            //        //string fileid =  + "@" +  ;
            //        cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
            //        cmdnotes.CommandType = CommandType.Text;
            //        cmdnotes.Connection = ssql;
            //        SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
            //        DocName.Value = fileName.ToString();
            //        cmdnotes.Parameters.Add(DocName);
            //        SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
            //        Type.Value = documentType.ToString();
            //        cmdnotes.Parameters.Add(Type);
            //        SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
            //        uploadedDocument.Value = documentBinary;
            //        cmdnotes.Parameters.Add(uploadedDocument);
            //        SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
            //        uploadedDate.Value = date;
            //        cmdnotes.Parameters.Add(uploadedDate);
            //        SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
            //        uploaded_id.Value = fileid;
            //        cmdnotes.Parameters.Add(uploaded_id);
            //        SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
            //        uploaded_mtype.Value = upload_memty_staff;
            //        cmdnotes.Parameters.Add(uploaded_mtype);
            //        SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
            //        uploaded_mcode.Value = mstaffcode;
            //        cmdnotes.Parameters.Add(uploaded_mcode);
            //        SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
            //        uploaded_mPK.Value = tapalfilefk;
            //        cmdnotes.Parameters.Add(uploaded_mPK);
            //        ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
            //        ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
            //        ssql.Close();
            //        ssql.Open();
            //        int result = cmdnotes.ExecuteNonQuery();
            //        if (result > 0)
            //        {
            //            savnotsflag = true;
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
        }
    }

    public void upload_par()
    {
        try
        {
            int upload_memty_staff = 3;
            bool savnotsflag = false;
            if (FileUpload_parnt.HasFile)
            {
                if (FileUpload_parnt.FileName.EndsWith(".jpg") || FileUpload_parnt.FileName.EndsWith(".gif") || FileUpload_parnt.FileName.EndsWith(".png") || FileUpload_parnt.FileName.EndsWith(".txt") || FileUpload_parnt.FileName.EndsWith(".doc") || FileUpload_parnt.FileName.EndsWith(".xls") || FileUpload_parnt.FileName.EndsWith(".docx") || FileUpload_parnt.FileName.EndsWith(".txt") || FileUpload_parnt.FileName.EndsWith(".document") || FileUpload_parnt.FileName.EndsWith(".xls") || FileUpload_parnt.FileName.EndsWith(".xlsx") || FileUpload_parnt.FileName.EndsWith(".pdf") || FileUpload_parnt.FileName.EndsWith(".ppt") || FileUpload_parnt.FileName.EndsWith(".pptx"))
                {
                    string fileName = Path.GetFileName(FileUpload_parnt.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(FileUpload_parnt.PostedFile.FileName);
                    string documentType = string.Empty;
                    switch (fileExtension)
                    {
                        case ".pdf":
                            documentType = "application/pdf";
                            break;
                        case ".xls":
                            documentType = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            documentType = "application/vnd.ms-excel";
                            break;
                        case ".doc":
                            documentType = "application/vnd.ms-word";
                            break;
                        case ".docx":
                            documentType = "application/vnd.ms-word";
                            break;
                        case ".gif":
                            documentType = "image/gif";
                            break;
                        case ".png":
                            documentType = "image/png";
                            break;
                        case ".jpg":
                            documentType = "image/jpg";
                            break;
                        case ".ppt":
                            documentType = "application/vnd.ms-ppt";
                            break;
                        case ".pptx":
                            documentType = "application/vnd.ms-pptx";
                            break;
                        case ".txt":
                            documentType = "application/txt";
                            break;
                    }
                    int fileSize = FileUpload_parnt.PostedFile.ContentLength;
                    //Create array and read the file into it
                    byte[] documentBinary = new byte[fileSize];
                    FileUpload_parnt.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                    string date = DateTime.Now.ToString("MM/dd/yyyy");
                    SqlCommand cmdnotes = new SqlCommand();
                    // string fileid = uploadvalue + "@" + uploadtime;
                    string fileid = uploadvalue;
                    string tapalfilefk = filepk;
                    //string fileid =  + "@" +  ;
                    cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;
                    SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                    DocName.Value = fileName.ToString();
                    cmdnotes.Parameters.Add(DocName);
                    SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                    Type.Value = documentType.ToString();
                    cmdnotes.Parameters.Add(Type);
                    SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                    uploadedDocument.Value = documentBinary;
                    cmdnotes.Parameters.Add(uploadedDocument);
                    SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                    uploadedDate.Value = date;
                    cmdnotes.Parameters.Add(uploadedDate);
                    SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
                    uploaded_id.Value = fileid;
                    cmdnotes.Parameters.Add(uploaded_id);
                    SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
                    uploaded_mtype.Value = upload_memty_staff;
                    cmdnotes.Parameters.Add(uploaded_mtype);
                    SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
                    uploaded_mcode.Value = mcode_par;
                    cmdnotes.Parameters.Add(uploaded_mcode);
                    SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
                    uploaded_mPK.Value = tapalfilefk;
                    cmdnotes.Parameters.Add(uploaded_mPK);
                    ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
                    ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        savnotsflag = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void upload_comp()//delsis
    {
        try
        {
            //int upload_memty_staff = 4;
            //bool savnotsflag = false;
            //if (FileUpload_com.HasFile)
            //{
            //    if (FileUpload_com.FileName.EndsWith(".jpg") || FileUpload_com.FileName.EndsWith(".gif") || FileUpload_com.FileName.EndsWith(".png") || FileUpload_com.FileName.EndsWith(".txt") || FileUpload_com.FileName.EndsWith(".doc") || FileUpload_com.FileName.EndsWith(".xls") || FileUpload_com.FileName.EndsWith(".docx") || FileUpload_com.FileName.EndsWith(".txt") || FileUpload_com.FileName.EndsWith(".document") || FileUpload_com.FileName.EndsWith(".xls") || FileUpload_com.FileName.EndsWith(".xlsx") || FileUpload_com.FileName.EndsWith(".pdf") || FileUpload_com.FileName.EndsWith(".ppt") || FileUpload_com.FileName.EndsWith(".pptx"))
            //    {
            //        string fileName = Path.GetFileName(FileUpload_com.PostedFile.FileName);
            //        string fileExtension = Path.GetExtension(FileUpload_com.PostedFile.FileName);
            //        string documentType = string.Empty;
            //        switch (fileExtension)
            //        {
            //            case ".pdf":
            //                documentType = "application/pdf";
            //                break;
            //            case ".xls":
            //                documentType = "application/vnd.ms-excel";
            //                break;
            //            case ".xlsx":
            //                documentType = "application/vnd.ms-excel";
            //                break;
            //            case ".doc":
            //                documentType = "application/vnd.ms-word";
            //                break;
            //            case ".docx":
            //                documentType = "application/vnd.ms-word";
            //                break;
            //            case ".gif":
            //                documentType = "image/gif";
            //                break;
            //            case ".png":
            //                documentType = "image/png";
            //                break;
            //            case ".jpg":
            //                documentType = "image/jpg";
            //                break;
            //            case ".ppt":
            //                documentType = "application/vnd.ms-ppt";
            //                break;
            //            case ".pptx":
            //                documentType = "application/vnd.ms-pptx";
            //                break;
            //            case ".txt":
            //                documentType = "application/txt";
            //                break;
            //        }
            //        int fileSize = FileUpload_com.PostedFile.ContentLength;
            //        //Create array and read the file into it
            //        byte[] documentBinary = new byte[fileSize];
            //        FileUpload_com.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
            //        string date = DateTime.Now.ToString("MM/dd/yyyy");
            //        SqlCommand cmdnotes = new SqlCommand();
            //        // string fileid = uploadvalue + "@" + uploadtime;
            //        string fileid = uploadvalue;
            //        string tapalfilefk = filepk;
            //        //string fileid =  + "@" +  ;
            //        cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
            //        cmdnotes.CommandType = CommandType.Text;
            //        cmdnotes.Connection = ssql;
            //        SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
            //        DocName.Value = fileName.ToString();
            //        cmdnotes.Parameters.Add(DocName);
            //        SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
            //        Type.Value = documentType.ToString();
            //        cmdnotes.Parameters.Add(Type);
            //        SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
            //        uploadedDocument.Value = documentBinary;
            //        cmdnotes.Parameters.Add(uploadedDocument);
            //        SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
            //        uploadedDate.Value = date;
            //        cmdnotes.Parameters.Add(uploadedDate);
            //        SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
            //        uploaded_id.Value = fileid;
            //        cmdnotes.Parameters.Add(uploaded_id);
            //        SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
            //        uploaded_mtype.Value = upload_memty_staff;
            //        cmdnotes.Parameters.Add(uploaded_mtype);
            //        SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
            //        uploaded_mcode.Value = mcode_comp;
            //        cmdnotes.Parameters.Add(uploaded_mcode);
            //        SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
            //        uploaded_mPK.Value = tapalfilefk;
            //        cmdnotes.Parameters.Add(uploaded_mPK);
            //        ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
            //        ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
            //        ssql.Close();
            //        ssql.Open();
            //        int result = cmdnotes.ExecuteNonQuery();
            //        if (result > 0)
            //        {
            //            savnotsflag = true;
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
        }
    }

    public void upload_indi()
    {
        try
        {
            int upload_memty_staff = 5;
            bool savnotsflag = false;
            if (FileUpload_ind.HasFile)
            {
                if (FileUpload_ind.FileName.EndsWith(".jpg") || FileUpload_ind.FileName.EndsWith(".gif") || FileUpload_ind.FileName.EndsWith(".png") || FileUpload_ind.FileName.EndsWith(".txt") || FileUpload_ind.FileName.EndsWith(".doc") || FileUpload_ind.FileName.EndsWith(".xls") || FileUpload_ind.FileName.EndsWith(".docx") || FileUpload_ind.FileName.EndsWith(".txt") || FileUpload_ind.FileName.EndsWith(".document") || FileUpload_ind.FileName.EndsWith(".xls") || FileUpload_ind.FileName.EndsWith(".xlsx") || FileUpload_ind.FileName.EndsWith(".pdf") || FileUpload_ind.FileName.EndsWith(".ppt") || FileUpload_ind.FileName.EndsWith(".pptx"))
                {
                    string fileName = Path.GetFileName(FileUpload_ind.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(FileUpload_ind.PostedFile.FileName);
                    string documentType = string.Empty;
                    switch (fileExtension)
                    {
                        case ".pdf":
                            documentType = "application/pdf";
                            break;
                        case ".xls":
                            documentType = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            documentType = "application/vnd.ms-excel";
                            break;
                        case ".doc":
                            documentType = "application/vnd.ms-word";
                            break;
                        case ".docx":
                            documentType = "application/vnd.ms-word";
                            break;
                        case ".gif":
                            documentType = "image/gif";
                            break;
                        case ".png":
                            documentType = "image/png";
                            break;
                        case ".jpg":
                            documentType = "image/jpg";
                            break;
                        case ".ppt":
                            documentType = "application/vnd.ms-ppt";
                            break;
                        case ".pptx":
                            documentType = "application/vnd.ms-pptx";
                            break;
                        case ".txt":
                            documentType = "application/txt";
                            break;
                    }
                    int fileSize = FileUpload_ind.PostedFile.ContentLength;
                    //Create array and read the file into it
                    byte[] documentBinary = new byte[fileSize];
                    FileUpload_ind.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                    string date = DateTime.Now.ToString("MM/dd/yyyy");
                    SqlCommand cmdnotes = new SqlCommand();
                    // string fileid = uploadvalue + "@" + uploadtime;
                    string fileid = uploadvalue;
                    string tapalfilefk = filepk;
                    //string fileid =  + "@" +  ;
                    cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;
                    SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                    DocName.Value = fileName.ToString();
                    cmdnotes.Parameters.Add(DocName);
                    SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                    Type.Value = documentType.ToString();
                    cmdnotes.Parameters.Add(Type);
                    SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                    uploadedDocument.Value = documentBinary;
                    cmdnotes.Parameters.Add(uploadedDocument);
                    SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                    uploadedDate.Value = date;
                    cmdnotes.Parameters.Add(uploadedDate);
                    SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
                    uploaded_id.Value = fileid;
                    cmdnotes.Parameters.Add(uploaded_id);
                    SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
                    uploaded_mtype.Value = upload_memty_staff;
                    cmdnotes.Parameters.Add(uploaded_mtype);
                    SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
                    uploaded_mcode.Value = mcode_indi;
                    cmdnotes.Parameters.Add(uploaded_mcode);
                    SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
                    uploaded_mPK.Value = tapalfilefk;
                    cmdnotes.Parameters.Add(uploaded_mPK);
                    ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
                    ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        savnotsflag = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void upload_mag()
    {
        try
        {
            int upload_memty_staff = 6;
            bool savnotsflag = false;
            if (FileUpload_magz.HasFile)
            {
                if (FileUpload_magz.FileName.EndsWith(".jpg") || FileUpload_magz.FileName.EndsWith(".gif") || FileUpload_magz.FileName.EndsWith(".png") || FileUpload_magz.FileName.EndsWith(".txt") || FileUpload_magz.FileName.EndsWith(".doc") || FileUpload_magz.FileName.EndsWith(".xls") || FileUpload_magz.FileName.EndsWith(".docx") || FileUpload_magz.FileName.EndsWith(".txt") || FileUpload_magz.FileName.EndsWith(".document") || FileUpload_magz.FileName.EndsWith(".xls") || FileUpload_magz.FileName.EndsWith(".xlsx") || FileUpload_magz.FileName.EndsWith(".pdf") || FileUpload_magz.FileName.EndsWith(".ppt") || FileUpload_magz.FileName.EndsWith(".pptx"))
                {
                    string fileName = Path.GetFileName(FileUpload_magz.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(FileUpload_magz.PostedFile.FileName);
                    string documentType = string.Empty;
                    switch (fileExtension)
                    {
                        case ".pdf":
                            documentType = "application/pdf";
                            break;
                        case ".xls":
                            documentType = "application/vnd.ms-excel";
                            break;
                        case ".xlsx":
                            documentType = "application/vnd.ms-excel";
                            break;
                        case ".doc":
                            documentType = "application/vnd.ms-word";
                            break;
                        case ".docx":
                            documentType = "application/vnd.ms-word";
                            break;
                        case ".gif":
                            documentType = "image/gif";
                            break;
                        case ".png":
                            documentType = "image/png";
                            break;
                        case ".jpg":
                            documentType = "image/jpg";
                            break;
                        case ".ppt":
                            documentType = "application/vnd.ms-ppt";
                            break;
                        case ".pptx":
                            documentType = "application/vnd.ms-pptx";
                            break;
                        case ".txt":
                            documentType = "application/txt";
                            break;
                    }
                    int fileSize = FileUpload_magz.PostedFile.ContentLength;
                    //Create array and read the file into it
                    byte[] documentBinary = new byte[fileSize];
                    FileUpload_magz.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                    string date = DateTime.Now.ToString("MM/dd/yyyy");
                    SqlCommand cmdnotes = new SqlCommand();
                    // string fileid = uploadvalue + "@" + uploadtime;
                    string fileid = uploadvalue;
                    string tapalfilefk = filepk;
                    //string fileid =  + "@" +  ;
                    cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;
                    SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                    DocName.Value = fileName.ToString();
                    cmdnotes.Parameters.Add(DocName);
                    SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                    Type.Value = documentType.ToString();
                    cmdnotes.Parameters.Add(Type);
                    SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                    uploadedDocument.Value = documentBinary;
                    cmdnotes.Parameters.Add(uploadedDocument);
                    SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                    uploadedDate.Value = date;
                    cmdnotes.Parameters.Add(uploadedDate);
                    SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
                    uploaded_id.Value = fileid;
                    cmdnotes.Parameters.Add(uploaded_id);
                    SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
                    uploaded_mtype.Value = upload_memty_staff;
                    cmdnotes.Parameters.Add(uploaded_mtype);
                    SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
                    uploaded_mcode.Value = mcode_mag;
                    cmdnotes.Parameters.Add(uploaded_mcode);
                    SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
                    uploaded_mPK.Value = tapalfilefk;
                    cmdnotes.Parameters.Add(uploaded_mPK);
                    ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
                    ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        savnotsflag = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    //public void upload_othr()
    //{
    //    try
    //    {
    //        int upload_memty_staff = 7;
    //        bool savnotsflag = false;
    //        if (FileUpload_others.HasFile)
    //        {
    //            if (FileUpload_others.FileName.EndsWith(".jpg") || FileUpload_others.FileName.EndsWith(".gif") || FileUpload_others.FileName.EndsWith(".png") || FileUpload_others.FileName.EndsWith(".txt") || FileUpload_others.FileName.EndsWith(".doc") || FileUpload_others.FileName.EndsWith(".xls") || FileUpload_others.FileName.EndsWith(".docx") || FileUpload_others.FileName.EndsWith(".txt") || FileUpload_others.FileName.EndsWith(".document") || FileUpload_others.FileName.EndsWith(".xls") || FileUpload_others.FileName.EndsWith(".xlsx") || FileUpload_others.FileName.EndsWith(".pdf") || FileUpload_others.FileName.EndsWith(".ppt") || FileUpload_others.FileName.EndsWith(".pptx"))
    //            {
    //                string fileName = Path.GetFileName(FileUpload_others.PostedFile.FileName);
    //                string fileExtension = Path.GetExtension(FileUpload_others.PostedFile.FileName);
    //                string documentType = string.Empty;
    //                switch (fileExtension)
    //                {
    //                    case ".pdf":
    //                        documentType = "application/pdf";
    //                        break;
    //                    case ".xls":
    //                        documentType = "application/vnd.ms-excel";
    //                        break;
    //                    case ".xlsx":
    //                        documentType = "application/vnd.ms-excel";
    //                        break;
    //                    case ".doc":
    //                        documentType = "application/vnd.ms-word";
    //                        break;
    //                    case ".docx":
    //                        documentType = "application/vnd.ms-word";
    //                        break;
    //                    case ".gif":
    //                        documentType = "image/gif";
    //                        break;
    //                    case ".png":
    //                        documentType = "image/png";
    //                        break;
    //                    case ".jpg":
    //                        documentType = "image/jpg";
    //                        break;
    //                    case ".ppt":
    //                        documentType = "application/vnd.ms-ppt";
    //                        break;
    //                    case ".pptx":
    //                        documentType = "application/vnd.ms-pptx";
    //                        break;
    //                    case ".txt":
    //                        documentType = "application/txt";
    //                        break;
    //                }
    //                int fileSize = FileUpload_others.PostedFile.ContentLength;
    //                //Create array and read the file into it
    //                byte[] documentBinary = new byte[fileSize];
    //                FileUpload_others.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
    //                string date = DateTime.Now.ToString("MM/dd/yyyy");
    //                SqlCommand cmdnotes = new SqlCommand();
    //                // string fileid = uploadvalue + "@" + uploadtime;
    //                string fileid = uploadvalue;
    //                string tapalfilefk = filepk;
    //                //string fileid =  + "@" +  ;
    //                cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
    //                cmdnotes.CommandType = CommandType.Text;
    //                cmdnotes.Connection = ssql;
    //                SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
    //                DocName.Value = fileName.ToString();
    //                cmdnotes.Parameters.Add(DocName);
    //                SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
    //                Type.Value = documentType.ToString();
    //                cmdnotes.Parameters.Add(Type);
    //                SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
    //                uploadedDocument.Value = documentBinary;
    //                cmdnotes.Parameters.Add(uploadedDocument);
    //                SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
    //                uploadedDate.Value = date;
    //                cmdnotes.Parameters.Add(uploadedDate);
    //                SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
    //                uploaded_id.Value = fileid;
    //                cmdnotes.Parameters.Add(uploaded_id);
    //                SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
    //                uploaded_mtype.Value = upload_memty_staff;
    //                cmdnotes.Parameters.Add(uploaded_mtype);
    //                SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
    //                uploaded_mcode.Value = mcode_othr;
    //                cmdnotes.Parameters.Add(uploaded_mcode);
    //                SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
    //                uploaded_mPK.Value = tapalfilefk;
    //                cmdnotes.Parameters.Add(uploaded_mPK);
    //                ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
    //                ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
    //                ssql.Close();
    //                ssql.Open();
    //                int result = cmdnotes.ExecuteNonQuery();
    //                if (result > 0)
    //                {
    //                    savnotsflag = true;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    public void dept()
    {
        string query = "select Dept_Name as DeptName,Dept_Code from Department ";
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string dd = Convert.ToString(ds.Tables[0].Rows[i]["DeptName"]);
                string ddd = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Code"]);
                if (!depthash.Contains(Convert.ToString(dd)))
                {
                    depthash.Add(Convert.ToString(dd), Convert.ToString(ddd));
                }
            }
        }
    }

    public void staffcodevalue()
    {
        string query = "select staff_code,staff_name from staffmaster";
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string dd = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                string ddd = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                if (!staffhash.Contains(Convert.ToString(ddd)))
                {
                    staffhash.Add(Convert.ToString(ddd), Convert.ToString(dd));
                }
            }
        }
    }

    public void txt_datestud_changed(object sender, EventArgs e)
    {
        try
        {
            div_stud.Attributes.Add("style", "display:block");
            if (txt_date.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_date.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = date.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (dt1 < dt)
                {
                    txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        catch
        {
        }
    }

    public void txt_stf_date_changed(object sender, EventArgs e)
    {
        try
        {
            //div_staff.Attributes.Add("style", "display:block");
            if (txt_stf_date.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_stf_date.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = date.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (dt1 < dt)
                {
                    txt_stf_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        catch
        {
        }
    }

    public void txt_pardate_changed(object sender, EventArgs e)
    {
        try
        {
            // div_parent.Attributes.Add("style", "display:block");
            if (txt_pardate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_pardate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = date.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (dt1 < dt)
                {
                    txt_pardate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        catch
        {
        }
    }

    public void txt_com_date_changed(object sender, EventArgs e)
    {
        try
        {
            // div_company.Attributes.Add("style", "display:block");
            if (txt_com_date.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_com_date.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = date.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (dt1 < dt)
                {
                    txt_com_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        catch
        {
        }
    }

    public void txt_ind_date_changed(object sender, EventArgs e)
    {
        try
        {
            // div_individu.Attributes.Add("style", "display:block");
            if (txt_ind_date.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_ind_date.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = date.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (dt1 < dt)
                {
                    txt_ind_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        catch
        {
        }
    }

    public void txt_magzdate_changed(object sender, EventArgs e)
    {
        try
        {
            //  div_magazine.Attributes.Add("style", "display:block");
            if (txt_magzdate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_magzdate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = date.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (dt1 < dt)
                {
                    txt_magzdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        catch
        {
        }
    }

    public void txt_othrdate_changed(object sender, EventArgs e)
    {
        try
        {
            //  div_others.Attributes.Add("style", "display:block");
            if (txt_othrdate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_othrdate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = date.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (dt1 < dt)
                {
                    txt_othrdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        catch
        {
        }
    }

    public void VendorCodeGen()
    {
        try
        {
            string newitemcode = "";
            string VendorCode = "";
            string selectquery = "select Vendor_Acr,Vendor_StNo,Vendor_Size from InvCode_Settings where Latestrec=1";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["Vendor_Acr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["Vendor_StNo"]);
                if (itemacronym.Trim() != "" && itemstarno.Trim() != "")
                {
                    selectquery = "select distinct top (1)  VendorCode  from CO_VendorMaster where VendorCode like '" + Convert.ToString(itemacronym) + "%' order by VendorCode desc";
                    //select distinct top (1)  RequestCode  from RQ_Requisition where RequestCode like '" + Convert.ToString(itemacronym) + "%' order by RequestCode desc";
                    //select distinct top (1) item_code  from item_master where item_code like '" + Convert.ToString(itemacronym) + "%' order by item_code desc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["VendorCode"]);
                        string itemacr = Convert.ToString(itemacronym);
                        int len = itemacr.Length;
                        itemcode = itemcode.Remove(0, len);
                        int len1 = Convert.ToString(itemcode).Length;
                        string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                        len = Convert.ToString(newnumber).Length;
                        len1 = len1 - len;
                        if (len1 == 2)
                        {
                            newitemcode = "00" + newnumber;
                        }
                        else if (len1 == 1)
                        {
                            newitemcode = "0" + newnumber;
                        }
                        else if (len1 == 4)
                        {
                            newitemcode = "0000" + newnumber;
                        }
                        else if (len1 == 3)
                        {
                            newitemcode = "000" + newnumber;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(newnumber);
                        }
                        if (newitemcode.Trim() != "")
                        {
                            newitemcode = itemacr + "" + newitemcode;
                        }
                    }
                    else
                    {
                        newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(itemstarno);
                    }
                    Session["VendorCode"] = newitemcode;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstate(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct top (50)TEXTVAL from TextValTable where TextCriteria ='state' and college_code=13 and TEXTVAL like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getcountry(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select TEXTVAL from TextValTable where TextCriteria ='coun' and TEXTVAL like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    public void timevalue()
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        string hrr = "";
        //string time =Convert.ToString(txt_viewtime.Text);
        string[] ay = time.Split(':');
        string val_hr = ay[0].ToString();
        int hr = Convert.ToInt16(val_hr);
        if (val_hr == "01")
        {
            hrr = "1";
        }
        else if (val_hr == "02")
        {
            hrr = "2";
        }
        else if (val_hr == "03")
        {
            hrr = "3";
        }
        else if (val_hr == "04")
        {
            hrr = "4";
        }
        else if (val_hr == "05")
        {
            hrr = "5";
        }
        else if (val_hr == "06")
        {
            hrr = "6";
        }
        else if (val_hr == "07")
        {
            hrr = "7";
        }
        else if (val_hr == "08")
        {
            hrr = "8";
        }
        else if (val_hr == "09")
        {
            hrr = "9";
        }
        else if (val_hr == "13")
        {
            hrr = "1";
        }
        else if (val_hr == "14")
        {
            hrr = "2";
        }
        else if (val_hr == "15")
        {
            hrr = "3";
        }
        else if (val_hr == "16")
        {
            hrr = "4";
        }
        else if (val_hr == "17")
        {
            hrr = "5";
        }
        else if (val_hr == "18")
        {
            hrr = "6";
        }
        else if (val_hr == "19")
        {
            hrr = "7";
        }
        else if (val_hr == "20")
        {
            hrr = "8";
        }
        else if (val_hr == "21")
        {
            hrr = "9";
        }
        else if (val_hr == "22")
        {
            hrr = "10";
        }
        else if (val_hr == "23")
        {
            hrr = "11";
        }
        else if (val_hr == "24")
        {
            hrr = "12";
        }
        if (val_hr == "10" || val_hr == "11" || val_hr == "12")
        {
            ddl_hour.Text = val_hr;
            ddl_minits.Text = ay[1].ToString();
            ddl_stfhr.Text = val_hr;
            ddl_stfm.Text = ay[1].ToString();
            ddl_parhr.Text = val_hr;
            ddl_parm.Text = ay[1].ToString();
            ddl_comhr.Text = val_hr;
            ddl_commi.Text = ay[1].ToString();
            ddl_indhr.Text = val_hr;
            ddlindmin.Text = ay[1].ToString();
            ddl_maghr.Text = val_hr;
            ddl_magmin.Text = ay[1].ToString();
            ddl_othrhr.Text = val_hr;
            ddl_othrmin.Text = ay[1].ToString();
        }
        else
        {
            ddl_hour.Text = hrr;
            ddl_minits.Text = ay[1].ToString();
            ddl_stfhr.Text = hrr;
            ddl_stfm.Text = ay[1].ToString();
            ddl_parhr.Text = hrr;
            ddl_parm.Text = ay[1].ToString();
            ddl_comhr.Text = hrr;
            ddl_commi.Text = ay[1].ToString();
            ddl_indhr.Text = hrr;
            ddlindmin.Text = ay[1].ToString();
            ddl_maghr.Text = hrr;
            ddl_magmin.Text = ay[1].ToString();
            ddl_othrhr.Text = hrr;
            ddl_othrmin.Text = ay[1].ToString();
        }
        if (val_hr == "12" || val_hr == "13" || val_hr == "14" || val_hr == "15" || val_hr == "16" || val_hr == "17" || val_hr == "18" || val_hr == "19" || val_hr == "20" || val_hr == "21" || val_hr == "22" || val_hr == "23" || val_hr == "24")
        {
            //if (hr >= 12)
            //{
            ddl_timeformate.Text = "PM";
            ddl_stfam.Text = "PM";
            ddl_param.Text = "PM";
            ddl_comam.Text = "PM";
            ddlinam.Text = "PM";
            ddl_magam.Text = "PM";
            ddl_othram.Text = "PM";
        }
        else
        {
            ddl_timeformate.Text = "AM";
            ddl_stfam.Text = "AM";
            ddl_param.Text = "AM";
            ddl_comam.Text = "AM";
            ddlinam.Text = "AM";
            ddl_magam.Text = "AM";
            ddl_othram.Text = "AM";
            //}
        }
    }
    [WebMethod]
    public static string Checkdept(string Roll_No)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Roll_No != "")
            {
                string query = "";
                query = "select Dept_Name as DeptName,Dept_Code from Department where Dept_Name = '" + Roll_No + "'";
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    [WebMethod]
    public static string Checkindividual(string Roll_No)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Roll_No != "")
            {
                string[] split1 = Roll_No.Split('-');
                string namesplit1 = split1[0];
                string query = "";
                query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and s.staff_name='" + namesplit1 + "'";
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    public void ddl_chrgstudhr_SelectedIndexChanged(object sender, EventArgs e)
    {
        div_stud.Attributes.Add("style", "display:block");
    }

    public void ddl_chrgstudled_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_chargesstaffhd_SelectedIndexChanged(object sender, EventArgs e)
    {
        div_stud.Attributes.Add("style", "display:none");
        div_staff.Attributes.Add("style", "display:block");
        div_company.Attributes.Add("style", "display:none");
        div_indiv.Attributes.Add("style", "display:none");
        div_magazine.Attributes.Add("style", "display:none");
        div_others.Attributes.Add("style", "display:none");
        div_parent.Attributes.Add("style", "display:none");
    }

    public void ddl_chargestaffled_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_chrgparhd_SelectedIndexChanged(object sender, EventArgs e)
    {
        div_stud.Attributes.Add("style", "display:none");
        div_staff.Attributes.Add("style", "display:none");
        div_company.Attributes.Add("style", "display:none");
        div_indiv.Attributes.Add("style", "display:none");
        div_magazine.Attributes.Add("style", "display:none");
        div_others.Attributes.Add("style", "display:none");
        div_parent.Attributes.Add("style", "display:block");
    }

    public void ddl_chrgparled_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_chargescomphd_SelectedIndexChanged(object sender, EventArgs e)
    {
        div_stud.Attributes.Add("style", "display:none");
        div_staff.Attributes.Add("style", "display:none");
        div_company.Attributes.Add("style", "display:block");
        div_indiv.Attributes.Add("style", "display:none");
        div_magazine.Attributes.Add("style", "display:none");
        div_others.Attributes.Add("style", "display:none");
        div_parent.Attributes.Add("style", "display:none");
    }

    public void ddl_chargescompld_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_chargesindihd_SelectedIndexChanged(object sender, EventArgs e)
    {
        div_stud.Attributes.Add("style", "display:none");
        div_staff.Attributes.Add("style", "display:none");
        div_company.Attributes.Add("style", "display:none");
        div_indiv.Attributes.Add("style", "display:block");
        div_magazine.Attributes.Add("style", "display:none");
        div_others.Attributes.Add("style", "display:none");
        div_parent.Attributes.Add("style", "display:none");
    }

    public void ddl_chargeindiled_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_chargesmadled_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void ddl_chargesmaghd_SelectedIndexChanged(object sender, EventArgs e)
    {
        div_stud.Attributes.Add("style", "display:none");
        div_staff.Attributes.Add("style", "display:none");
        div_company.Attributes.Add("style", "display:none");
        div_indiv.Attributes.Add("style", "display:none");
        div_magazine.Attributes.Add("style", "display:block");
        div_others.Attributes.Add("style", "display:none");
        div_parent.Attributes.Add("style", "display:none");
    }

    public void ddl_chargesothrhdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        div_stud.Attributes.Add("style", "display:none");
        div_staff.Attributes.Add("style", "display:none");
        div_company.Attributes.Add("style", "display:none");
        div_indiv.Attributes.Add("style", "display:none");
        div_magazine.Attributes.Add("style", "display:none");
        div_others.Attributes.Add("style", "display:block");
        div_parent.Attributes.Add("style", "display:none");
    }

    public void ddl_chargeothrled_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public string generateReceiptNo()
    {
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;
            string finYearid = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + collegecode1 + "");
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }
                string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                recacr = acronymquery;
                int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)"));
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
        catch { return recno; }
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
        catch (Exception ex)
        {
        }
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
        catch (Exception ex) { }
    }

    #endregion

    // added by kowshika(30.04.2018)
    #region codegenerate

    public void generateReceiptNo1()
    {
        string newitemcode = "";
        try
        {
            string dep = string.Empty;
            if (ddl_college.Items.Count > 0)
                collegecode = Convert.ToString(ddl_college.SelectedValue);
            usercode = Session["usercode"].ToString();


            string user = d2.GetFunction("select staff_code from UserMaster where User_code='" + usercode + "'");
            string deptcode = d2.GetFunction("select dept_code from stafftrans where staff_code='" + user + "'");
            string selectquery = "select InwardAcr,InwardSize,InwardStNo from InwardCodeSettings where  DeptCode='" + deptcode + "' order by FromDate desc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string iacronym = Convert.ToString(ds.Tables[0].Rows[0]["InwardAcr"]);
                string istarno = Convert.ToString(ds.Tables[0].Rows[0]["InwardStNo"]);
                string isize = Convert.ToString(ds.Tables[0].Rows[0]["InwardSize"]);
                if (iacronym.Trim() != "" && istarno.Trim() != "")
                {
                    //dep = d2.GetFunction("select Dept_Code from Department where  college_code='" + collegecode + "'");
                    selectquery = "select distinct isnull(TapalCode,'0') as TapalCode from TapalInward  where TapalCode like '" + Convert.ToString(iacronym) + "%' and ToDeptCode ='" + deptcode + "'  order by TapalCode desc";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        string tcode = Convert.ToString(ds.Tables[0].Rows[0]["TapalCode"]);
                        if (tcode != "0")
                        {
                            string itemacr = Convert.ToString(iacronym);
                            int len = itemacr.Length;
                            tcode = tcode.Remove(0, len);
                            int len1 = Convert.ToString(tcode).Length;
                            string newnumber = Convert.ToString((Convert.ToInt32(tcode) + 1));
                            len = Convert.ToString(newnumber).Length;
                            len1 = len1 - len;
                            if (len1 == 2)
                            {
                                newitemcode = "00" + newnumber;
                            }
                            else if (len1 == 1)
                            {
                                newitemcode = "0" + newnumber;
                            }
                            else

                                if (len1 == 3)
                                {
                                    newitemcode = "000" + newnumber;
                                }
                                else if (len1 == 4)
                                {
                                    newitemcode = "0000" + newnumber;
                                }
                                else if (len1 == 5)
                                {
                                    newitemcode = "00000" + newnumber;
                                }
                                else if (len1 == 6)
                                {
                                    newitemcode = "000000" + newnumber;
                                }
                                else
                                {
                                    newitemcode = Convert.ToString(newnumber);
                                }
                            if (newitemcode.Trim() != "")
                            {
                                newitemcode = itemacr + "" + newitemcode;
                            }

                            txt_rcptno.Text = Convert.ToString(newitemcode);
                        }
                        else
                        {
                            string itemacr = Convert.ToString(istarno);
                            int len = itemacr.Length;
                            string items = Convert.ToString(istarno);
                            int len1 = Convert.ToInt32(items);
                            int size = len1 - len;
                            if (size == 2)
                            {
                                newitemcode = "00" + istarno;
                            }
                            else if (size == 1)
                            {
                                newitemcode = "0" + istarno;
                            }
                            else if (size == 3)
                            {
                                newitemcode = "000" + istarno;
                            }
                            else if (size == 4)
                            {
                                newitemcode = "0000" + istarno;
                            }
                            else if (size == 5)
                            {
                                newitemcode = "00000" + istarno;
                            }
                            else if (size == 6)
                            {
                                newitemcode = "000000" + istarno;
                            }
                            else
                            {
                                newitemcode = Convert.ToString(istarno);
                            }
                            newitemcode = Convert.ToString(iacronym) + "" + Convert.ToString(newitemcode);
                            txt_rcptno.Text = Convert.ToString(newitemcode);
                        }

                    }
                    else
                    {
                        string itemacr = Convert.ToString(istarno);
                        int len = itemacr.Length;
                        string items = Convert.ToString(istarno);
                        int len1 = Convert.ToInt32(items);
                        int size = len1 - len;
                        if (size == 2)
                        {
                            newitemcode = "00" + istarno;
                        }
                        else if (size == 1)
                        {
                            newitemcode = "0" + istarno;
                        }
                        else if (size == 3)
                        {
                            newitemcode = "000" + istarno;
                        }
                        else if (size == 4)
                        {
                            newitemcode = "0000" + istarno;
                        }
                        else if (size == 5)
                        {
                            newitemcode = "00000" + istarno;
                        }
                        else if (size == 6)
                        {
                            newitemcode = "000000" + istarno;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(istarno);
                        }
                        newitemcode = Convert.ToString(iacronym) + "" + Convert.ToString(newitemcode);
                        txt_rcptno.Text = Convert.ToString(newitemcode);
                    }

                }
            }

        }

        catch { }

    }

    //public void generateReceiptNo2()
    //{
    //    string newitemcode2 = "";
    //    try
    //    {
    //        string dep2 = string.Empty;
    //        if (ddl_college.Items.Count > 0)
    //            collegecode = Convert.ToString(ddl_college.SelectedValue);

    //        usercode = Session["usercode"].ToString();
    //        string user2 = d2.GetFunction("select staff_code from UserMaster where User_code='" + usercode + "'");
    //        string deptcode2 = d2.GetFunction("select dept_code from stafftrans where staff_code='" + user2 + "'");


    //        string selectquery2 = "select InwardAcr,InwardSize,InwardStNo from InwardCodeSettings where DeptCode='" + deptcode2 + "' order by FromDate desc";
    //        ds = d2.select_method_wo_parameter(selectquery2, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            string iacronym2 = Convert.ToString(ds.Tables[0].Rows[0]["InwardAcr"]);
    //            string istarno2 = Convert.ToString(ds.Tables[0].Rows[0]["InwardStNo"]);
    //            string isize2 = Convert.ToString(ds.Tables[0].Rows[0]["InwardSize"]);
    //            if (iacronym2.Trim() != "" && istarno2.Trim() != "")
    //            {
    //                // dep2 = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + Depar2 + "' and college_code='" + collegecode + "'");
    //                selectquery2 = "select distinct isnull(TapalCode,'0') as TapalCode from TapalInward  where TapalCode like '" + Convert.ToString(iacronym2) + "%' and ToDeptCode ='" + deptcode2 + "'  order by TapalCode desc";

    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(selectquery2, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string tcode2 = Convert.ToString(ds.Tables[0].Rows[0]["TapalCode"]);
    //                    if (tcode2 != "0")
    //                    {
    //                        string itemacr2 = Convert.ToString(iacronym2);
    //                        int le = itemacr2.Length;
    //                        tcode2 = tcode2.Remove(0, le);
    //                        int ln1 = Convert.ToString(tcode2).Length;
    //                        string newnumber2 = Convert.ToString((Convert.ToInt32(tcode2) + 1));
    //                        le = Convert.ToString(newnumber2).Length;
    //                        ln1 = ln1 - le;
    //                        if (ln1 == 2)
    //                        {
    //                            newitemcode2 = "00" + newnumber2;
    //                        }
    //                        else if (ln1 == 1)
    //                        {
    //                            newitemcode2 = "0" + newnumber2;
    //                        }
    //                        else

    //                            if (ln1 == 3)
    //                            {
    //                                newitemcode2 = "000" + newnumber2;
    //                            }
    //                            else if (ln1 == 4)
    //                            {
    //                                newitemcode2 = "0000" + newnumber2;
    //                            }
    //                            else if (ln1 == 5)
    //                            {
    //                                newitemcode2 = "00000" + newnumber2;
    //                            }
    //                            else if (ln1 == 6)
    //                            {
    //                                newitemcode2 = "000000" + newnumber2;
    //                            }
    //                            else
    //                            {
    //                                newitemcode2 = Convert.ToString(newnumber2);
    //                            }
    //                        if (newitemcode2.Trim() != "")
    //                        {
    //                            newitemcode2 = itemacr2 + "" + newitemcode2;
    //                        }

    //                        txt_rcptno.Text = Convert.ToString(newitemcode2);

    //                    }
    //                    else
    //                    {
    //                        string itemacr2 = Convert.ToString(istarno2);
    //                        int len2 = itemacr2.Length;
    //                        string items2 = Convert.ToString(istarno2);
    //                        int le1 = Convert.ToInt32(items2);
    //                        int size2 = le1 - len2;
    //                        if (size2 == 2)
    //                        {
    //                            newitemcode2 = "00" + istarno2;
    //                        }
    //                        else if (size2 == 1)
    //                        {
    //                            newitemcode2 = "0" + istarno2;
    //                        }
    //                        else if (size2 == 3)
    //                        {
    //                            newitemcode2 = "000" + istarno2;
    //                        }
    //                        else if (size2 == 4)
    //                        {
    //                            newitemcode2 = "0000" + istarno2;
    //                        }
    //                        else if (size2 == 5)
    //                        {
    //                            newitemcode2 = "00000" + istarno2;
    //                        }
    //                        else if (size2 == 6)
    //                        {
    //                            newitemcode2 = "000000" + istarno2;
    //                        }
    //                        else
    //                        {
    //                            newitemcode2 = Convert.ToString(istarno2);
    //                        }
    //                        newitemcode2 = Convert.ToString(iacronym2) + "" + Convert.ToString(newitemcode2);
    //                        txt_rcptno.Text = Convert.ToString(newitemcode2);
    //                    }
    //                }
    //                else
    //                {
    //                    string itemacr2 = Convert.ToString(istarno2);
    //                    int len2 = itemacr2.Length;
    //                    string items2 = Convert.ToString(istarno2);
    //                    int le1 = Convert.ToInt32(items2);
    //                    int size2 = le1 - len2;
    //                    if (size2 == 2)
    //                    {
    //                        newitemcode2 = "00" + istarno2;
    //                    }
    //                    else if (size2 == 1)
    //                    {
    //                        newitemcode2 = "0" + istarno2;
    //                    }
    //                    else if (size2 == 3)
    //                    {
    //                        newitemcode2 = "000" + istarno2;
    //                    }
    //                    else if (size2 == 4)
    //                    {
    //                        newitemcode2 = "0000" + istarno2;
    //                    }
    //                    else if (size2 == 5)
    //                    {
    //                        newitemcode2 = "00000" + istarno2;
    //                    }
    //                    else if (size2 == 6)
    //                    {
    //                        newitemcode2 = "000000" + istarno2;
    //                    }
    //                    else
    //                    {
    //                        newitemcode2 = Convert.ToString(istarno2);
    //                    }
    //                    newitemcode2 = Convert.ToString(iacronym2) + "" + Convert.ToString(newitemcode2);
    //                    txt_rcptno.Text = Convert.ToString(newitemcode2);
    //                }
    //            }
    //        }

    //    }

    //    catch { }

    //}

    //public void generateReceiptNo3()
    //{
    //    string newitemcode3 = "";
    //    try
    //    {
    //        string dep3 = string.Empty;
    //        if (ddl_college.Items.Count > 0)
    //            collegecode = Convert.ToString(ddl_college.SelectedValue);
    //        usercode = Session["usercode"].ToString();
    //        string user3 = d2.GetFunction("select staff_code from UserMaster where User_code='" + usercode + "'");
    //        string deptcode3 = d2.GetFunction("select dept_code from stafftrans where staff_code='" + user3 + "'");

    //        string dept_code3 = d2.GetFunction("select  Dept_Code from Department where Dept_Name='" + deptcode3 + "'");
    //        string selectquery3 = "select InwardAcr,InwardSize,InwardStNo from InwardCodeSettings where DeptCode='" + deptcode3 + "' order by FromDate desc";
    //        ds = d2.select_method_wo_parameter(selectquery3, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            string iacronym3 = Convert.ToString(ds.Tables[0].Rows[0]["InwardAcr"]);
    //            string istarno3 = Convert.ToString(ds.Tables[0].Rows[0]["InwardStNo"]);
    //            string isize3 = Convert.ToString(ds.Tables[0].Rows[0]["InwardSize"]);
    //            {
    //                // dep3 = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + deptcode3 + "' and college_code='" + collegecode + "'");
    //                selectquery3 = "select distinct isnull(TapalCode,'0') as TapalCode from TapalInward  where TapalCode like '" + Convert.ToString(iacronym3) + "%' and ToDeptCode ='" + deptcode3 + "'  order by TapalCode desc";

    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(selectquery3, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string tcode3 = Convert.ToString(ds.Tables[0].Rows[0]["TapalCode"]);
    //                    if (tcode3 != "0")
    //                    {
    //                        string itemacr3 = Convert.ToString(iacronym3);
    //                        int le2 = itemacr3.Length;
    //                        tcode3 = tcode3.Remove(0, le2);
    //                        int ln3 = Convert.ToString(tcode3).Length;
    //                        string newnumber3 = Convert.ToString((Convert.ToInt32(tcode3) + 1));
    //                        le2 = Convert.ToString(newnumber3).Length;
    //                        ln3 = ln3 - le2;
    //                        if (ln3 == 2)
    //                        {
    //                            newitemcode3 = "00" + newnumber3;
    //                        }
    //                        else if (ln3 == 1)
    //                        {
    //                            newitemcode3 = "0" + newnumber3;
    //                        }
    //                        else

    //                            if (ln3 == 3)
    //                            {
    //                                newitemcode3 = "000" + newnumber3;
    //                            }
    //                            else if (ln3 == 4)
    //                            {
    //                                newitemcode3 = "0000" + newnumber3;
    //                            }
    //                            else if (ln3 == 5)
    //                            {
    //                                newitemcode3 = "00000" + newnumber3;
    //                            }
    //                            else if (ln3 == 6)
    //                            {
    //                                newitemcode3 = "000000" + newnumber3;
    //                            }
    //                            else
    //                            {
    //                                newitemcode3 = Convert.ToString(newnumber3);
    //                            }
    //                        if (newitemcode3.Trim() != "")
    //                        {
    //                            newitemcode3 = itemacr3 + "" + newitemcode3;
    //                        }

    //                        txt_rcptno.Text = Convert.ToString(newitemcode3);

    //                    }
    //                    else
    //                    {
    //                        string itemacr3 = Convert.ToString(istarno3);
    //                        int len2 = itemacr3.Length;
    //                        string items3 = Convert.ToString(istarno3);
    //                        int le1 = Convert.ToInt32(items3);
    //                        int size3 = le1 - len2;
    //                        if (size3 == 2)
    //                        {
    //                            newitemcode3 = "00" + istarno3;
    //                        }
    //                        else if (size3 == 1)
    //                        {
    //                            newitemcode3 = "0" + istarno3;
    //                        }
    //                        else if (size3 == 3)
    //                        {
    //                            newitemcode3 = "000" + istarno3;
    //                        }
    //                        else if (size3 == 4)
    //                        {
    //                            newitemcode3 = "0000" + istarno3;
    //                        }
    //                        else if (size3 == 5)
    //                        {
    //                            newitemcode3 = "00000" + istarno3;
    //                        }
    //                        else if (size3 == 6)
    //                        {
    //                            newitemcode3 = "000000" + istarno3;
    //                        }
    //                        else
    //                        {
    //                            newitemcode3 = Convert.ToString(istarno3);
    //                        }
    //                        newitemcode3 = Convert.ToString(iacronym3) + "" + Convert.ToString(newitemcode3);
    //                        txt_rcptno.Text = Convert.ToString(newitemcode3);
    //                    }
    //                }
    //                else
    //                {
    //                    string itemacr3 = Convert.ToString(istarno3);
    //                    int len2 = itemacr3.Length;
    //                    string items3 = Convert.ToString(istarno3);
    //                    int le1 = Convert.ToInt32(items3);
    //                    int size3 = le1 - len2;
    //                    if (size3 == 2)
    //                    {
    //                        newitemcode3 = "00" + istarno3;
    //                    }
    //                    else if (size3 == 1)
    //                    {
    //                        newitemcode3 = "0" + istarno3;
    //                    }
    //                    else if (size3 == 3)
    //                    {
    //                        newitemcode3 = "000" + istarno3;
    //                    }
    //                    else if (size3 == 4)
    //                    {
    //                        newitemcode3 = "0000" + istarno3;
    //                    }
    //                    else if (size3 == 5)
    //                    {
    //                        newitemcode3 = "00000" + istarno3;
    //                    }
    //                    else if (size3 == 6)
    //                    {
    //                        newitemcode3 = "000000" + istarno3;
    //                    }
    //                    else
    //                    {
    //                        newitemcode3 = Convert.ToString(istarno3);
    //                    }
    //                    newitemcode3 = Convert.ToString(iacronym3) + "" + Convert.ToString(newitemcode3);
    //                    txt_rcptno.Text = Convert.ToString(newitemcode3);
    //                }
    //            }
    //        }

    //    }

    //    catch { }

    //}

    //public void generateReceiptNo4()
    //{
    //    string newitemcode4 = "";
    //    try
    //    {
    //        string dep4 = string.Empty;
    //        if (ddl_college.Items.Count > 0)
    //            collegecode = Convert.ToString(ddl_college.SelectedValue);
    //        usercode = Session["usercode"].ToString();
    //        string user4 = d2.GetFunction("select staff_code from UserMaster where User_code='" + usercode + "'");
    //        string deptcode4 = d2.GetFunction("select dept_code from stafftrans where staff_code='" + user4 + "'");

    //        string dept_code4 = d2.GetFunction("select  Dept_Code from Department where Dept_Name='" + deptcode4 + "'");
    //        string selectquery4 = "select InwardAcr,InwardSize,InwardStNo from InwardCodeSettings where DeptCode='" + deptcode4 + "' order by FromDate desc";
    //        ds = d2.select_method_wo_parameter(selectquery4, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            string iacronym4 = Convert.ToString(ds.Tables[0].Rows[0]["InwardAcr"]);
    //            string istarno4 = Convert.ToString(ds.Tables[0].Rows[0]["InwardStNo"]);
    //            string isize4 = Convert.ToString(ds.Tables[0].Rows[0]["InwardSize"]);
    //            {
    //                //dep4 = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + Depar4 + "' and college_code='" + collegecode + "'");
    //                selectquery4 = "select distinct isnull(TapalCode,'0') as TapalCode from TapalInward  where TapalCode like '" + Convert.ToString(iacronym4) + "%' and ToDeptCode ='" + deptcode4 + "'  order by TapalCode desc";

    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(selectquery4, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string tcode4 = Convert.ToString(ds.Tables[0].Rows[0]["TapalCode"]);
    //                    if (tcode4 != "0")
    //                    {
    //                        string itemacr4 = Convert.ToString(iacronym4);
    //                        int le2 = itemacr4.Length;
    //                        tcode4 = tcode4.Remove(0, le2);
    //                        int ln4 = Convert.ToString(tcode4).Length;
    //                        string newnumber4 = Convert.ToString((Convert.ToInt32(tcode4) + 1));
    //                        le2 = Convert.ToString(newnumber4).Length;
    //                        ln4 = ln4 - le2;
    //                        if (ln4 == 2)
    //                        {
    //                            newitemcode4 = "00" + newnumber4;
    //                        }
    //                        else if (ln4 == 1)
    //                        {
    //                            newitemcode4 = "0" + newnumber4;
    //                        }
    //                        else

    //                            if (ln4 == 3)
    //                            {
    //                                newitemcode4 = "000" + newnumber4;
    //                            }
    //                            else if (ln4 == 4)
    //                            {
    //                                newitemcode4 = "0000" + newnumber4;
    //                            }
    //                            else if (ln4 == 5)
    //                            {
    //                                newitemcode4 = "00000" + newnumber4;
    //                            }
    //                            else if (ln4 == 6)
    //                            {
    //                                newitemcode4 = "000000" + newnumber4;
    //                            }
    //                            else
    //                            {
    //                                newitemcode4 = Convert.ToString(newnumber4);
    //                            }
    //                        if (newitemcode4.Trim() != "")
    //                        {
    //                            newitemcode4 = itemacr4 + "" + newitemcode4;
    //                        }

    //                        txt_rcptno.Text = Convert.ToString(newitemcode4);

    //                    }
    //                    else
    //                    {
    //                        string itemacr4 = Convert.ToString(istarno4);
    //                        int len2 = itemacr4.Length;
    //                        string items3 = Convert.ToString(istarno4);
    //                        int le1 = Convert.ToInt32(items3);
    //                        int size4 = le1 - len2;
    //                        if (size4 == 2)
    //                        {
    //                            newitemcode4 = "00" + istarno4;
    //                        }
    //                        else if (size4 == 1)
    //                        {
    //                            newitemcode4 = "0" + istarno4;
    //                        }
    //                        else if (size4 == 3)
    //                        {
    //                            newitemcode4 = "000" + istarno4;
    //                        }
    //                        else if (size4 == 4)
    //                        {
    //                            newitemcode4 = "0000" + istarno4;
    //                        }
    //                        else if (size4 == 5)
    //                        {
    //                            newitemcode4 = "00000" + istarno4;
    //                        }
    //                        else if (size4 == 6)
    //                        {
    //                            newitemcode4 = "000000" + istarno4;
    //                        }
    //                        else
    //                        {
    //                            newitemcode4 = Convert.ToString(istarno4);
    //                        }
    //                        newitemcode4 = Convert.ToString(iacronym4) + "" + Convert.ToString(newitemcode4);
    //                        txt_rcptno.Text = Convert.ToString(newitemcode4);
    //                    }
    //                }
    //                else
    //                {
    //                    string itemacr4 = Convert.ToString(istarno4);
    //                    int len2 = itemacr4.Length;
    //                    string items3 = Convert.ToString(istarno4);
    //                    int le1 = Convert.ToInt32(items3);
    //                    int size4 = le1 - len2;
    //                    if (size4 == 2)
    //                    {
    //                        newitemcode4 = "00" + istarno4;
    //                    }
    //                    else if (size4 == 1)
    //                    {
    //                        newitemcode4 = "0" + istarno4;
    //                    }
    //                    else if (size4 == 3)
    //                    {
    //                        newitemcode4 = "000" + istarno4;
    //                    }
    //                    else if (size4 == 4)
    //                    {
    //                        newitemcode4 = "0000" + istarno4;
    //                    }
    //                    else if (size4 == 5)
    //                    {
    //                        newitemcode4 = "00000" + istarno4;
    //                    }
    //                    else if (size4 == 6)
    //                    {
    //                        newitemcode4 = "000000" + istarno4;
    //                    }
    //                    else
    //                    {
    //                        newitemcode4 = Convert.ToString(istarno4);
    //                    }
    //                    newitemcode4 = Convert.ToString(iacronym4) + "" + Convert.ToString(newitemcode4);
    //                    txt_rcptno.Text = Convert.ToString(newitemcode4);
    //                }
    //            }
    //        }

    //    }

    //    catch { }

    //}
    #endregion

    public void access()
    {
        try
        {
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='Letter HeaderLedger Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='Letter HeaderLedger Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    hdsave = split[0];
                    ledsave = split[1];
                }
            }
        }
        catch
        {
        }
    }

    public void access1()
    {
        try
        {
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='Letter Tap Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='Letter Tap Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    string len = split.Length.ToString();
                    if (len == "1")
                    {
                        values = val;
                        if (val == "1")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                        }
                        if (val == "2")
                        {
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                        }
                        else
                        {
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                        }
                        if (val == "3")
                        {
                            imgbtn_parents.Visible = true;
                            lbl_parents.Visible = true;
                        }
                        else
                        {
                            imgbtn_parents.Visible = false;
                            lbl_parents.Visible = false;
                        }
                        if (val == "4")
                        {
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                        }
                        else
                        {
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                        }
                        if (val == "5")
                        {
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                        }
                        else
                        {
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                        }
                        if (val == "6")
                        {
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                        }
                        else
                        {
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                        }
                        if (val == "7")
                        {
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                    }
                    //******************** length 2**************
                    if (len == "2")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        if (val == "0")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                        if (sp1 == "1" || sp2 == "1")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                        }
                        if (sp1 == "2" || sp2 == "2")
                        {
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                        }
                        else
                        {
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3")
                        {
                            imgbtn_parents.Visible = true;
                            lbl_parents.Visible = true;
                        }
                        else
                        {
                            imgbtn_parents.Visible = false;
                            lbl_parents.Visible = false;
                        }
                        if (sp1 == "4" || sp2 == "4")
                        {
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                        }
                        else
                        {
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                        }
                        if (sp1 == "5" || sp2 == "5")
                        {
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                        }
                        else
                        {
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                        }
                        if (sp1 == "6" || sp2 == "6")
                        {
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                        }
                        else
                        {
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                        }
                        if (sp1 == "7" || sp2 == "7")
                        {
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                    }
                    //  *************************** length 3*****************
                    else if (len == "3")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        if (val == "0")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                            txt_indiv.Visible = false;
                            lbl_indiv.Visible = false;
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2")
                        {
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                        }
                        else
                        {
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3")
                        {
                            imgbtn_parents.Visible = true;
                            lbl_parents.Visible = true;
                        }
                        else
                        {
                            imgbtn_parents.Visible = false;
                            lbl_parents.Visible = false;
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4")
                        {
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                        }
                        else
                        {
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5")
                        {
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                        }
                        else
                        {
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6")
                        {
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                        }
                        else
                        {
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                        }
                        if (sp1 == "7" || sp2 == "7" || sp3 == "7")
                        {
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                    }
                    // *********************** length 4*****************
                    else if (len == "4")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        string sp4 = (split[3]);
                        if (val == "0")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1" || sp4 == "1")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2" || sp4 == "2")
                        {
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                        }
                        else
                        {
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3" || sp4 == "3")
                        {
                            imgbtn_parents.Visible = true;
                            lbl_parents.Visible = true;
                        }
                        else
                        {
                            imgbtn_parents.Visible = false;
                            lbl_parents.Visible = false;
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4" || sp4 == "4")
                        {
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                        }
                        else
                        {
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5" || sp4 == "5")
                        {
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                        }
                        else
                        {
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6" || sp4 == "6")
                        {
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                        }
                        else
                        {
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                        }
                        if (sp1 == "7" || sp2 == "7" || sp3 == "7" || sp4 == "7")
                        {
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                    }
                    else if (len == "5")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        string sp4 = (split[3]);
                        string sp5 = (split[4]);
                        if (val == "0")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1" || sp4 == "1" || sp5 == "1")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2" || sp4 == "2" || sp5 == "2")
                        {
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                        }
                        else
                        {
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3" || sp4 == "3" || sp5 == "3")
                        {
                            imgbtn_parents.Visible = true;
                            lbl_parents.Visible = true;
                        }
                        else
                        {
                            imgbtn_parents.Visible = false;
                            lbl_parents.Visible = false;
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4" || sp4 == "4" || sp5 == "4")
                        {
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                        }
                        else
                        {
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5" || sp4 == "5" || sp5 == "5")
                        {
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                        }
                        else
                        {
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6" || sp4 == "6" || sp5 == "6")
                        {
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                        }
                        else
                        {
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                        }
                        if (sp1 == "7" || sp2 == "7" || sp3 == "7" || sp4 == "7" || sp5 == "7")
                        {
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                    }
                    else if (len == "6")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        string sp4 = (split[3]);
                        string sp5 = (split[4]);
                        string sp6 = (split[5]);
                        if (val == "0")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1" || sp4 == "1" || sp5 == "1" || sp6 == "1")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2" || sp4 == "2" || sp5 == "2" || sp6 == "2")
                        {
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                        }
                        else
                        {
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3" || sp4 == "3" || sp5 == "3" || sp6 == "3")
                        {
                            imgbtn_parents.Visible = true;
                            lbl_parents.Visible = true;
                        }
                        else
                        {
                            imgbtn_parents.Visible = false;
                            lbl_parents.Visible = false;
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4" || sp4 == "4" || sp5 == "4" || sp6 == "4")
                        {
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                        }
                        else
                        {
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5" || sp4 == "5" || sp5 == "5" || sp6 == "5")
                        {
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                        }
                        else
                        {
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6" || sp4 == "6" || sp5 == "6" || sp6 == "6")
                        {
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                        }
                        else
                        {
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                        }
                        if (sp1 == "7" || sp2 == "7" || sp3 == "7" || sp4 == "7" || sp5 == "7" || sp6 == "7")
                        {
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                    }
                    else if (len == "7")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        string sp4 = (split[3]);
                        string sp5 = (split[4]);
                        string sp6 = (split[5]);
                        string sp7 = (split[6]);
                        if (val == "0")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1" || sp4 == "1" || sp5 == "1" || sp6 == "1" || sp7 == "1")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2" || sp4 == "2" || sp5 == "2" || sp6 == "2" || sp7 == "2")
                        {
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                        }
                        else
                        {
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3" || sp4 == "3" || sp5 == "3" || sp6 == "3" || sp7 == "3")
                        {
                            imgbtn_parents.Visible = true;
                            lbl_parents.Visible = true;
                        }
                        else
                        {
                            imgbtn_parents.Visible = false;
                            lbl_parents.Visible = false;
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4" || sp4 == "4" || sp5 == "4" || sp6 == "4" || sp7 == "4")
                        {
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                        }
                        else
                        {
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5" || sp4 == "5" || sp5 == "5" || sp6 == "5" || sp7 == "5")
                        {
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                        }
                        else
                        {
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6" || sp4 == "6" || sp5 == "6" || sp6 == "6" || sp7 == "6")
                        {
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                        }
                        else
                        {
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                        }
                        if (sp1 == "7" || sp2 == "7" || sp3 == "7" || sp4 == "7" || sp5 == "7" || sp6 == "7" || sp7 == "7")
                        {
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                    }
                    else if (len == "8")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        string sp4 = (split[3]);
                        string sp5 = (split[4]);
                        string sp6 = (split[5]);
                        string sp7 = (split[6]);
                        string sp8 = (split[7]);
                        if (val == "0")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1" || sp4 == "1" || sp5 == "1" || sp6 == "1" || sp7 == "1" || sp8 == "1")
                        {
                            imgbtn_stud.Visible = true;
                            lbl_student.Visible = true;
                        }
                        else
                        {
                            imgbtn_stud.Visible = false;
                            lbl_student.Visible = false;
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2" || sp4 == "2" || sp5 == "2" || sp6 == "2" || sp7 == "2" || sp8 == "2")
                        {
                            imgbtn_staff.Visible = true;
                            lbl_staff.Visible = true;
                        }
                        else
                        {
                            imgbtn_staff.Visible = false;
                            lbl_staff.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3" || sp4 == "3" || sp5 == "3" || sp6 == "3" || sp7 == "3" || sp8 == "3")
                        {
                            imgbtn_parents.Visible = true;
                            lbl_parents.Visible = true;
                        }
                        else
                        {
                            imgbtn_parents.Visible = false;
                            lbl_parents.Visible = false;
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4" || sp4 == "4" || sp5 == "4" || sp6 == "4" || sp7 == "4" || sp8 == "4")
                        {
                            imgbtn_company.Visible = true;
                            lbl_company.Visible = true;
                        }
                        else
                        {
                            imgbtn_company.Visible = false;
                            lbl_company.Visible = false;
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5" || sp4 == "5" || sp5 == "5" || sp6 == "5" || sp7 == "5" || sp8 == "5")
                        {
                            imgbtn_individual.Visible = true;
                            lbl_individual.Visible = true;
                        }
                        else
                        {
                            imgbtn_individual.Visible = false;
                            lbl_individual.Visible = false;
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6" || sp4 == "6" || sp5 == "6" || sp6 == "6" || sp7 == "6" || sp8 == "6")
                        {
                            imgbtn_magazine.Visible = true;
                            lbl_magazine.Visible = true;
                        }
                        else
                        {
                            imgbtn_magazine.Visible = false;
                            lbl_magazine.Visible = false;
                        }
                        if (sp1 == "7" || sp2 == "7" || sp3 == "7" || sp4 == "7" || sp5 == "7" || sp6 == "7" || sp7 == "7" || sp8 == "7")
                        {
                            imgbtn_others.Visible = true;
                            lbl_others.Visible = true;
                        }
                        else
                        {
                            imgbtn_others.Visible = false;
                            lbl_others.Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    [WebMethod]
    public static string Checkstfname(string Roll_No)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Roll_No != "")
            {
                string query = "";
                if (oldcurrentcheck == "O")
                {
                    query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=1 and resign =1 and s.staff_name like '" + Roll_No + "%'";
                }
                else
                {
                    query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + Roll_No + "%'";
                }
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "0";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    [WebMethod]
    public static string checkstaffcodechk(string Roll_No)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Roll_No != "")
            {
                string query = "";
                if (oldcurrentcheck == "O")
                    query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=1 and resign =1 and s.staff_code like '" + Roll_No + "%'";
                else
                    query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code like '" + Roll_No + "%'";
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count == 0)
                    flage = true;

                if (flage == true)
                    returnValue = "0";
                else
                    returnValue = "1";
            }
            else
                returnValue = "0";
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    public void BindCollege()
    {
        string srisql = "select collname,college_code from collinfo";
        ds.Clear();
        ds = d2.select_method_wo_parameter(srisql, "Text");
        ddl_college.DataSource = ds;
        ddl_college.DataTextField = "collname";
        ddl_college.DataValueField = "college_code";
        ddl_college.DataBind();
    }

    public void bankbind()//delsi
    {
        try
        {
            ddl_stud_bank.Items.Clear();
            //string query = "select BankName,BankCode,BankPK from FM_FinBankMaster where CollegeCode='" + collegecode1 + "'";
            //ddl_othr_bank.DataSource = ds;
            //ddl_othr_bank.DataTextField = "BankName";
            //ddl_othr_bank.DataValueField = "BankPK";
            //ddl_othr_bank.DataBind();

            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='bankN'";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_stud_bank.DataSource = ds;
                ddl_stud_bank.DataTextField = "MasterValue";
                ddl_stud_bank.DataValueField = "MasterCode";
                ddl_stud_bank.DataBind();
                ddl_stud_bank.Items.Insert(0, new ListItem("Select", "0"));
                ddl_staff_bank.DataSource = ds;
                ddl_staff_bank.DataTextField = "MasterValue";
                ddl_staff_bank.DataValueField = "MasterCode";
                ddl_staff_bank.DataBind();

                ddl_staff_bank.Items.Insert(0, new ListItem("Select", "0"));
                ddl_par_bank.DataSource = ds;
                ddl_par_bank.DataTextField = "MasterValue";
                ddl_par_bank.DataValueField = "MasterCode";
                ddl_par_bank.DataBind();
                ddl_par_bank.Items.Insert(0, new ListItem("Select", "0"));
                txt_bank_comp.DataSource = ds;
                txt_bank_comp.DataTextField = "MasterValue";
                txt_bank_comp.DataValueField = "MasterCode";
                txt_bank_comp.DataBind();
                txt_bank_comp.Items.Insert(0, new ListItem("Select", "0"));

                ddl_bank_stud.DataSource = ds;
                ddl_bank_stud.DataTextField = "MasterValue";
                ddl_bank_stud.DataValueField = "MasterCode";
                ddl_bank_stud.DataBind();
                ddl_bank_stud.Items.Insert(0, new ListItem("Select", "0"));

                dd_indi_bank.DataSource = ds;
                dd_indi_bank.DataTextField = "MasterValue";
                dd_indi_bank.DataValueField = "MasterCode";
                dd_indi_bank.DataBind();
                dd_indi_bank.Items.Insert(0, new ListItem("Select", "0"));
                ddl_magbank.DataSource = ds;
                ddl_magbank.DataTextField = "MasterValue";
                ddl_magbank.DataValueField = "MasterCode";
                ddl_magbank.DataBind();
                ddl_magbank.Items.Insert(0, new ListItem("Select", "0"));
                ddl_othr_bank.DataSource = ds;
                ddl_othr_bank.DataTextField = "MasterValue";
                ddl_othr_bank.DataValueField = "MasterCode";
                ddl_othr_bank.DataBind();
                ddl_othr_bank.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        {
        }
    }

    public void btnerrclose1_Click(object sender, EventArgs e)
    {
        imgdiv4.Visible = false;
        panel_erroralert1.Visible = false;
        //loaddesc();
        //loaddesc1();
    }

    //public void rdb_stfin_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (rb_stf_in.Checked == true)
    //    {
    //        lbl_stf_delivry.Text = "Received Mode";
    //    }
    //}
    //public void rdb_stfout_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (rb_stf_out.Checked == true)
    //    {
    //        lbl_stf_delivry.Text = "Delivery Mode";
    //    }
    //}

    public void fileuploads()//delsi1901
    {

        try
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("Files");

            dr = dt.NewRow();
            dr["Files"] = 1;
            dt.Rows.Add(dr);

            Certificate_Grid.DataSource = dt;
            Certificate_Grid.DataBind();
            ViewState["CurrentTable"] = dt;

            staffupload_Grid.DataSource = dt;
            staffupload_Grid.DataBind();
            ViewState["CurrentTable"] = dt;

            Grid_others.DataSource = dt;
            Grid_others.DataBind();
            ViewState["CurrentTable"] = dt;
            grid_stude_attach.DataSource = dt;
            grid_stude_attach.DataBind();
            ViewState["CurrentTable"] = dt;


        }
        catch (Exception ex)
        {
        }


    }

    public void uploadnew_comp()//delsis
    {
        try
        {
            int upload_memty_staff = 4;
            bool savnotsflag = false;
            //  GridView Certificate_dt = (GridView)Session["Certificate_Grid"];
            for (int i = 0; i < Certificate_Grid.Rows.Count; i++)
            {
                FileUpload fileupload = (FileUpload)Certificate_Grid.Rows[i].FindControl("file_uplod");
                Label cert_id = (Label)Certificate_Grid.Rows[i].FindControl("lbl_certificate");
                string certificateid = Convert.ToString(cert_id.Text);
                if (fileupload.HasFile)
                {
                    if (fileupload.FileName.EndsWith(".jpg") || fileupload.FileName.EndsWith(".gif") || fileupload.FileName.EndsWith(".png") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".doc") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".docx") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".document") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".xlsx") || fileupload.FileName.EndsWith(".pdf") || fileupload.FileName.EndsWith(".ppt") || fileupload.FileName.EndsWith(".pptx"))
                    {
                        string fileName = Path.GetFileName(fileupload.PostedFile.FileName);
                        string fileExtension = Path.GetExtension(fileupload.PostedFile.FileName);
                        string documentType = string.Empty;
                        switch (fileExtension)
                        {
                            case ".pdf":
                                documentType = "application/pdf";
                                break;
                            case ".xls":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".xlsx":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".doc":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".docx":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".gif":
                                documentType = "image/gif";
                                break;
                            case ".png":
                                documentType = "image/png";
                                break;
                            case ".jpg":
                                documentType = "image/jpg";
                                break;
                            case ".ppt":
                                documentType = "application/vnd.ms-ppt";
                                break;
                            case ".pptx":
                                documentType = "application/vnd.ms-pptx";
                                break;
                            case ".txt":
                                documentType = "application/txt";
                                break;
                        }


                        int fileSize = fileupload.PostedFile.ContentLength;
                        //Create array and read the file into it
                        byte[] documentBinary = new byte[fileSize];
                        fileupload.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        string date = DateTime.Now.ToString("MM/dd/yyyy");
                        SqlCommand cmdnotes = new SqlCommand();
                        // string fileid = uploadvalue + "@" + uploadtime;
                        string fileid = uploadvalue;
                        string tapalfilefk = filepk;
                        //string fileid =  + "@" +  ;
                        cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
                        cmdnotes.CommandType = CommandType.Text;
                        cmdnotes.Connection = ssql;
                        SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                        DocName.Value = fileName.ToString();
                        cmdnotes.Parameters.Add(DocName);
                        SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                        Type.Value = documentType.ToString();
                        cmdnotes.Parameters.Add(Type);
                        SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                        uploadedDocument.Value = documentBinary;
                        cmdnotes.Parameters.Add(uploadedDocument);
                        SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                        uploadedDate.Value = date;
                        cmdnotes.Parameters.Add(uploadedDate);
                        SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
                        uploaded_id.Value = fileid;
                        cmdnotes.Parameters.Add(uploaded_id);
                        SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
                        uploaded_mtype.Value = upload_memty_staff;
                        cmdnotes.Parameters.Add(uploaded_mtype);
                        SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
                        uploaded_mcode.Value = mcode;
                        cmdnotes.Parameters.Add(uploaded_mcode);
                        SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
                        uploaded_mPK.Value = tapalfilefk;
                        cmdnotes.Parameters.Add(uploaded_mPK);
                        ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
                        ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
                        ssql.Close();
                        ssql.Open();
                        int result = cmdnotes.ExecuteNonQuery();
                        if (result > 0)
                        {
                            savnotsflag = true;
                        }



                        // cert.Add("@appno", appno);
                        //   cert.Add("@CertificateName", certificateid);
                        //  int dummy = d2.insert_method("sp_student_Certificat_detailsinsert", cert, "sp");
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void upload_staffDoc()//delsis
    {
        try
        {

            int upload_memty_staff = 2;
            bool savnotsflag = false;
            //  GridView Certificate_dt = (GridView)Session["Certificate_Grid"];
            for (int i = 0; i < staffupload_Grid.Rows.Count; i++)
            {
                FileUpload fileupload = (FileUpload)staffupload_Grid.Rows[i].FindControl("file_uplod1");
                Label cert_id = (Label)staffupload_Grid.Rows[i].FindControl("lbl_certificate1");
                string certificateid = Convert.ToString(cert_id.Text);
                if (fileupload.HasFile)
                {
                    if (fileupload.FileName.EndsWith(".jpg") || fileupload.FileName.EndsWith(".gif") || fileupload.FileName.EndsWith(".png") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".doc") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".docx") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".document") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".xlsx") || fileupload.FileName.EndsWith(".pdf") || fileupload.FileName.EndsWith(".ppt") || fileupload.FileName.EndsWith(".pptx"))
                    {
                        string fileName = Path.GetFileName(fileupload.PostedFile.FileName);
                        string fileExtension = Path.GetExtension(fileupload.PostedFile.FileName);
                        string documentType = string.Empty;
                        switch (fileExtension)
                        {
                            case ".pdf":
                                documentType = "application/pdf";
                                break;
                            case ".xls":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".xlsx":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".doc":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".docx":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".gif":
                                documentType = "image/gif";
                                break;
                            case ".png":
                                documentType = "image/png";
                                break;
                            case ".jpg":
                                documentType = "image/jpg";
                                break;
                            case ".ppt":
                                documentType = "application/vnd.ms-ppt";
                                break;
                            case ".pptx":
                                documentType = "application/vnd.ms-pptx";
                                break;
                            case ".txt":
                                documentType = "application/txt";
                                break;
                        }


                        int fileSize = fileupload.PostedFile.ContentLength;
                        //Create array and read the file into it
                        byte[] documentBinary = new byte[fileSize];
                        fileupload.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        string date = DateTime.Now.ToString("MM/dd/yyyy");
                        SqlCommand cmdnotes = new SqlCommand();
                        // string fileid = uploadvalue + "@" + uploadtime;
                        string fileid = uploadvalue;
                        string tapalfilefk = filepk;
                        //string fileid =  + "@" +  ;
                        cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
                        cmdnotes.CommandType = CommandType.Text;
                        cmdnotes.Connection = ssql;
                        SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                        DocName.Value = fileName.ToString();
                        cmdnotes.Parameters.Add(DocName);
                        SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                        Type.Value = documentType.ToString();
                        cmdnotes.Parameters.Add(Type);
                        SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                        uploadedDocument.Value = documentBinary;
                        cmdnotes.Parameters.Add(uploadedDocument);
                        SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                        uploadedDate.Value = date;
                        cmdnotes.Parameters.Add(uploadedDate);
                        SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
                        uploaded_id.Value = fileid;
                        cmdnotes.Parameters.Add(uploaded_id);
                        SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
                        uploaded_mtype.Value = upload_memty_staff;
                        cmdnotes.Parameters.Add(uploaded_mtype);
                        SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
                        uploaded_mcode.Value = mstaffcode;
                        cmdnotes.Parameters.Add(uploaded_mcode);
                        SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
                        uploaded_mPK.Value = tapalfilefk;
                        cmdnotes.Parameters.Add(uploaded_mPK);
                        ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
                        ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
                        ssql.Close();
                        ssql.Open();
                        int result = cmdnotes.ExecuteNonQuery();
                        if (result > 0)
                        {
                            savnotsflag = true;
                        }



                        // cert.Add("@appno", appno);
                        //   cert.Add("@CertificateName", certificateid);
                        //  int dummy = d2.insert_method("sp_student_Certificat_detailsinsert", cert, "sp");
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void uploadstud()
    {
        try
        {


            int upload_memty_staff = 1;
            bool savnotsflag = false;
            //  GridView Certificate_dt = (GridView)Session["Certificate_Grid"];
            for (int i = 0; i < grid_stude_attach.Rows.Count; i++)
            {
                FileUpload fileupload = (FileUpload)grid_stude_attach.Rows[i].FindControl("file_uplodeStud");
                Label cert_id = (Label)grid_stude_attach.Rows[i].FindControl("lbl_stud_upload");
                string certificateid = Convert.ToString(cert_id.Text);
                if (fileupload.HasFile)
                {
                    if (fileupload.FileName.EndsWith(".jpg") || fileupload.FileName.EndsWith(".gif") || fileupload.FileName.EndsWith(".png") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".doc") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".docx") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".document") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".xlsx") || fileupload.FileName.EndsWith(".pdf") || fileupload.FileName.EndsWith(".ppt") || fileupload.FileName.EndsWith(".pptx"))
                    {
                        string fileName = Path.GetFileName(fileupload.PostedFile.FileName);
                        string fileExtension = Path.GetExtension(fileupload.PostedFile.FileName);
                        string documentType = string.Empty;
                        switch (fileExtension)
                        {
                            case ".pdf":
                                documentType = "application/pdf";
                                break;
                            case ".xls":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".xlsx":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".doc":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".docx":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".gif":
                                documentType = "image/gif";
                                break;
                            case ".png":
                                documentType = "image/png";
                                break;
                            case ".jpg":
                                documentType = "image/jpg";
                                break;
                            case ".ppt":
                                documentType = "application/vnd.ms-ppt";
                                break;
                            case ".pptx":
                                documentType = "application/vnd.ms-pptx";
                                break;
                            case ".txt":
                                documentType = "application/txt";
                                break;
                        }


                        int fileSize = fileupload.PostedFile.ContentLength;
                        //Create array and read the file into it
                        byte[] documentBinary = new byte[fileSize];
                        fileupload.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        string date = DateTime.Now.ToString("MM/dd/yyyy");
                        SqlCommand cmdnotes = new SqlCommand();
                        // string fileid = uploadvalue + "@" + uploadtime;
                        string fileid = uploadvalue;
                        string tapalfilefk = filepk;
                        //string fileid =  + "@" +  ;
                        cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
                        cmdnotes.CommandType = CommandType.Text;
                        cmdnotes.Connection = ssql;
                        SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                        DocName.Value = fileName.ToString();
                        cmdnotes.Parameters.Add(DocName);
                        SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                        Type.Value = documentType.ToString();
                        cmdnotes.Parameters.Add(Type);
                        SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                        uploadedDocument.Value = documentBinary;
                        cmdnotes.Parameters.Add(uploadedDocument);
                        SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                        uploadedDate.Value = date;
                        cmdnotes.Parameters.Add(uploadedDate);
                        SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
                        uploaded_id.Value = fileid;
                        cmdnotes.Parameters.Add(uploaded_id);
                        SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
                        uploaded_mtype.Value = upload_memty_staff;
                        cmdnotes.Parameters.Add(uploaded_mtype);
                        SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
                        uploaded_mcode.Value = mstaffcode;
                        cmdnotes.Parameters.Add(uploaded_mcode);
                        SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
                        uploaded_mPK.Value = tapalfilefk;
                        cmdnotes.Parameters.Add(uploaded_mPK);
                        ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
                        ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
                        ssql.Close();
                        ssql.Open();
                        int result = cmdnotes.ExecuteNonQuery();
                        if (result > 0)
                        {
                            savnotsflag = true;
                        }



                        // cert.Add("@appno", appno);
                        //   cert.Add("@CertificateName", certificateid);
                        //  int dummy = d2.insert_method("sp_student_Certificat_detailsinsert", cert, "sp");
                    }
                }
            }

        }
        catch
        {

        }

    }

    public void upload_othr()
    {

        try
        {

            int upload_memty_staff = 7;
            bool savnotsflag = false;
            //  GridView Certificate_dt = (GridView)Session["Certificate_Grid"];
            for (int i = 0; i < Grid_others.Rows.Count; i++)
            {
                FileUpload fileupload = (FileUpload)Grid_others.Rows[i].FindControl("file_uplod3");
                Label cert_id = (Label)Grid_others.Rows[i].FindControl("lbl_certificate3");
                string certificateid = Convert.ToString(cert_id.Text);
                if (fileupload.HasFile)
                {
                    if (fileupload.FileName.EndsWith(".jpg") || fileupload.FileName.EndsWith(".gif") || fileupload.FileName.EndsWith(".png") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".doc") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".docx") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".document") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".xlsx") || fileupload.FileName.EndsWith(".pdf") || fileupload.FileName.EndsWith(".ppt") || fileupload.FileName.EndsWith(".pptx"))
                    {
                        string fileName = Path.GetFileName(fileupload.PostedFile.FileName);
                        string fileExtension = Path.GetExtension(fileupload.PostedFile.FileName);
                        string documentType = string.Empty;
                        switch (fileExtension)
                        {
                            case ".pdf":
                                documentType = "application/pdf";
                                break;
                            case ".xls":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".xlsx":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".doc":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".docx":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".gif":
                                documentType = "image/gif";
                                break;
                            case ".png":
                                documentType = "image/png";
                                break;
                            case ".jpg":
                                documentType = "image/jpg";
                                break;
                            case ".ppt":
                                documentType = "application/vnd.ms-ppt";
                                break;
                            case ".pptx":
                                documentType = "application/vnd.ms-pptx";
                                break;
                            case ".txt":
                                documentType = "application/txt";
                                break;
                        }


                        int fileSize = fileupload.PostedFile.ContentLength;
                        //Create array and read the file into it
                        byte[] documentBinary = new byte[fileSize];
                        fileupload.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        string date = DateTime.Now.ToString("MM/dd/yyyy");
                        SqlCommand cmdnotes = new SqlCommand();
                        // string fileid = uploadvalue + "@" + uploadtime;
                        string fileid = uploadvalue;
                        string tapalfilefk = filepk;
                        //string fileid =  + "@" +  ;
                        cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata,date,fileid,Memtype,MemCode,TapalInwardFk)" + " VALUES (@DocName,@Type,@DocData,@date,@fileid,@mtype,@mcode,@tapalfilefk)";
                        cmdnotes.CommandType = CommandType.Text;
                        cmdnotes.Connection = ssql;
                        SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                        DocName.Value = fileName.ToString();
                        cmdnotes.Parameters.Add(DocName);
                        SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                        Type.Value = documentType.ToString();
                        cmdnotes.Parameters.Add(Type);
                        SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                        uploadedDocument.Value = documentBinary;
                        cmdnotes.Parameters.Add(uploadedDocument);
                        SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                        uploadedDate.Value = date;
                        cmdnotes.Parameters.Add(uploadedDate);
                        SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.NVarChar, 500);
                        uploaded_id.Value = fileid;
                        cmdnotes.Parameters.Add(uploaded_id);
                        SqlParameter uploaded_mtype = new SqlParameter("@mtype", SqlDbType.TinyInt, 500);
                        uploaded_mtype.Value = upload_memty_staff;
                        cmdnotes.Parameters.Add(uploaded_mtype);
                        SqlParameter uploaded_mcode = new SqlParameter("@mcode", SqlDbType.NVarChar, 500);
                        uploaded_mcode.Value = mstaffcode;
                        cmdnotes.Parameters.Add(uploaded_mcode);
                        SqlParameter uploaded_mPK = new SqlParameter("@tapalfilefk", SqlDbType.BigInt, 500);
                        uploaded_mPK.Value = tapalfilefk;
                        cmdnotes.Parameters.Add(uploaded_mPK);
                        ////string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
                        ////insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
                        ssql.Close();
                        ssql.Open();
                        int result = cmdnotes.ExecuteNonQuery();
                        if (result > 0)
                        {
                            savnotsflag = true;
                        }



                        // cert.Add("@appno", appno);
                        //   cert.Add("@CertificateName", certificateid);
                        //  int dummy = d2.insert_method("sp_student_Certificat_detailsinsert", cert, "sp");
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }


    }

    protected void btnaddrows_Click(object sender, EventArgs e)
    {
        try
        {

            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {

                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {

                        Label lblsNo = (Label)Certificate_Grid.Rows[rowIndex].Cells[1].FindControl("lbl_certificate");


                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["Files"] = i + 1;

                        rowIndex++;

                    }

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable"] = dtCurrentTable;

                    Certificate_Grid.DataSource = dtCurrentTable;

                    Certificate_Grid.DataBind();


                }

            }
            ScriptManager.RegisterStartupScript(this, GetType(), "imgbtn_company", "companycheck();", true);

        }

        catch (Exception ex)
        {


        }


    }

    protected void btnaddrows1_Click(object sender, EventArgs e)
    {
        try
        {

            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {

                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {

                        Label lblsNo = (Label)staffupload_Grid.Rows[rowIndex].Cells[1].FindControl("lbl_certificate1");


                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["Files"] = i + 1;

                        rowIndex++;

                    }

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable"] = dtCurrentTable;

                    staffupload_Grid.DataSource = dtCurrentTable;

                    staffupload_Grid.DataBind();


                }

            }
            ScriptManager.RegisterStartupScript(this, GetType(), "imgbtn_staff", "staffcheck();", true);

        }

        catch (Exception ex)
        {


        }


    }

    protected void btnaddrows2_Click(object sender, EventArgs e)
    {
        try
        {

            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {

                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {

                        Label lblsNo = (Label)Grid_others.Rows[rowIndex].Cells[1].FindControl("lbl_certificate3");


                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["Files"] = i + 1;

                        rowIndex++;

                    }

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable"] = dtCurrentTable;

                    Grid_others.DataSource = dtCurrentTable;

                    Grid_others.DataBind();


                }

            }
            ScriptManager.RegisterStartupScript(this, GetType(), "imgbtn_others", "otherscheck();", true);

        }

        catch (Exception ex)
        {


        }


    }

    protected void btnaddrows1_studClick(object sender, EventArgs e)
    {

        try
        {

            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {

                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {

                        Label lblsNo = (Label)grid_stude_attach.Rows[rowIndex].Cells[1].FindControl("lbl_stud_upload");


                        drCurrentRow = dtCurrentTable.NewRow();

                        drCurrentRow["Files"] = i + 1;

                        rowIndex++;

                    }

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable"] = dtCurrentTable;

                    grid_stude_attach.DataSource = dtCurrentTable;

                    grid_stude_attach.DataBind();


                }

            }
            ScriptManager.RegisterStartupScript(this, GetType(), "imgbtn_stud", "studentcheck();", true);

        }

        catch (Exception ex)
        {


        }

    }

    protected void enqbtn_Click(object sender, EventArgs e)//delsi2202
    {
        Panellookup1.Visible = true;
        ddlheader.Items.Clear();
        ddlheader.Items.Add("---Select---");
        ddlheader.Items.Add("Roll No");
        ddlheader.Items.Add("Reg No");
        ddlheader.Items.Add("Name");
        ddloperator.Items.Clear();
        ddloperator.Items.Add("---Select---");
        ddloperator.Items.Add("Like");
        ddloperator.Items.Add("Starts With");
        ddloperator.Items.Add("Ends With");

        ScriptManager.RegisterStartupScript(this, GetType(), "imgbtn_stud", "studentcheck();", true);
    }

    protected void btncloselook1_Click(object sender, EventArgs e)
    {
        Panellookup1.Visible = false;
        ScriptManager.RegisterStartupScript(this, GetType(), "imgbtn_stud", "studentcheck();", true);
    }

    protected void ddlcollegenew_SelectedIndexChanged(object sender, EventArgs e)
    {
        loaddetails();
    }

    public void loaddetails()
    {
        bindBatch1();
        bindcourse();

        if (ddlDegree.Items.Count > 0)
        {
            bindBranch();
        }
    }

    public void bindBatch1()
    {


        int year2;
        year2 = Convert.ToInt16(DateTime.Today.Year);
        ddlbatch.Items.Clear();
        for (int l = 0; l <= 10; l++)
        {
            ddlbatch.Items.Add(Convert.ToString(year2 - l));
        }

    }

    public void bindcourse()
    {
        string usercode = Session["usercode"].ToString();
        DAccess2 da1 = new DAccess2();
        DataSet ds1 = new DataSet();
        ht.Clear();
        string strisstaff = Session["Staff_Code"].ToString();
        ddlDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        ht.Clear();
        ht.Add("single_user", singleuser);
        ht.Add("group_code", group_user);
        if (strisstaff.ToLower().Trim() == "")
        {
            ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
        }
        else
        {
            ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
        }

        ht.Add("user_code", usercode);
        ds1 = da1.select_method("bind_degree", ht, "sp");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            ddlDegree.Enabled = true;
            ddlDegree.Items.Clear();
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                ddlDegree.Items.Insert(i, new System.Web.UI.WebControls.ListItem(Convert.ToString(ds1.Tables[0].Rows[i]["course_name"]), Convert.ToString(ds1.Tables[0].Rows[i]["course_id"])));
            }
        }
        else
        {
            ddlDegree.Enabled = false;

        }
    }

    public void bindBranch()
    {
        try
        {
            DAccess2 da1 = new DAccess2();
            DataSet ds1 = new DataSet();
            string strisstaff = Session["Staff_Code"].ToString();
            ddlBranch1.Items.Clear();
            ht.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ht.Add("single_user", singleuser);
            ht.Add("group_code", group_user);

            ht.Add("course_id", ddlDegree.SelectedValue);
            if (strisstaff.ToLower().Trim() == "")
            {
                ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
            }
            else
            {
                ht.Add("college_code", ddlcollegenew.SelectedValue.ToString());
            }
            ht.Add("user_code", usercode);
            ds1 = da1.select_method("bind_branch", ht, "sp");
            if (ds1.Tables.Count > 0)
            {

                ddlBranch1.DataSource = ds1;
                ddlBranch1.DataTextField = "Acronym";
                ddlBranch1.DataValueField = "degree_code";
                ddlBranch1.DataBind();
            }


        }
        catch
        {
        }

    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindcourse();
        bindBranch();

    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBranch();
    }

    protected void ddlBranch1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnlookupgo1_Click(object sender, EventArgs e)
    {
        FpSpread1.Visible = true;
        StudentLookup1();

    }

    public void StudentLookup1()
    {
        try
        {
            DataSet dsload = new DataSet();
            string serach_Crita = "";
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            // ddlstatus.Enabled = true;
            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            Studentinfo();
            if (ddlbatch.Items.Count > 0)
            {
                if (ddlDegree.Items.Count > 0)
                {
                    if (ddlBranch1.Items.Count > 0)
                    {
                        if (ddlheader.SelectedIndex == 1)
                        {
                            if (ddloperator.SelectedIndex == 1)
                            {
                                serach_Crita = " and Roll_No like '%" + tbvalue.Text.Trim() + "%' ";
                            }
                            else if (ddloperator.SelectedIndex == 2)
                            {
                                serach_Crita = " and Roll_No like '" + tbvalue.Text.Trim() + "%' ";

                            }
                            else if (ddloperator.SelectedIndex == 3)
                            {
                                serach_Crita = " and Roll_No like '%" + tbvalue.Text.Trim() + "' ";
                            }
                        }
                        if (ddlheader.SelectedIndex == 2)
                        {
                            if (ddloperator.SelectedIndex == 1)
                            {
                                serach_Crita = " and Reg_No like '%" + tbvalue.Text.Trim() + "%' ";

                            }
                            else if (ddloperator.SelectedIndex == 2)
                            {
                                serach_Crita = " and Reg_No like '" + tbvalue.Text.Trim() + "%' ";

                            }
                            else if (ddloperator.SelectedIndex == 3)
                            {
                                serach_Crita = " and Reg_No like '%" + tbvalue.Text.Trim() + "' ";
                            }

                        }

                        if (ddlheader.SelectedIndex == 3)
                        {
                            if (ddloperator.SelectedIndex == 1)
                            {
                                serach_Crita = " and Stud_Name like '%" + tbvalue.Text.Trim() + "%' ";

                            }
                            else if (ddloperator.SelectedIndex == 2)
                            {
                                serach_Crita = " and Stud_Name like '" + tbvalue.Text.Trim() + "%' ";
                            }
                            else if (ddloperator.SelectedIndex == 3)
                            {
                                serach_Crita = " and Stud_Name like '%" + tbvalue.Text.Trim() + "' ";
                            }
                        }


                        string Branch_Code;
                        //string Degree;
                        Branch_Code = ddlBranch1.SelectedValue.ToString(); //GetFunction("select degree_Code from degree where acronym = '" + ddlBranch1.SelectedItem.Text.ToString() + "'");

                        string sqlcmd = "select distinct Roll_No,Stud_Name,degree_code,Reg_No ,app_no,college_code from  registration where degree_code='" + ddlBranch1.SelectedValue.ToString() + "' and college_code = '" + ddlcollegenew.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' " + serach_Crita + " and cc=0 and exam_flag<>'debar' and delflag=0 and ((isnull(Bus_RouteID,'')='' and isnull(Boarding,'')='' and isnull(VehID,'')='') or iscanceledstage='1')";
                        //and (Bus_RouteID is null Or Boarding is null Or VehID is null or Bus_RouteID='' or Boarding='' or VehID='')";

                        dsload = d2.select_method_wo_parameter(sqlcmd, "Text");
                        if (dsload.Tables[0].Rows.Count > 0)
                        {
                            for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                            {
                                ++FpSpread1.Sheets[0].RowCount;
                                //Added by Srinath 12/12/2014
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = txt;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ddlDegree.SelectedItem.Text.ToString() + "-" + ddlBranch1.SelectedItem.Text.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ddlDegree.SelectedItem.Text.ToString() + "-" + ddlBranch1.SelectedItem.Text.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = dsload.Tables[0].Rows[loop]["Roll_No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dsload.Tables[0].Rows[loop]["app_no"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsload.Tables[0].Rows[loop]["Reg_No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsload.Tables[0].Rows[loop]["Stud_Name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = dsload.Tables[0].Rows[loop]["college_code"].ToString();

                            }
                            //FpSpread1.Sheets[0].PageSize = FpSpread1.Rows.Count;
                            FpSpread1.SaveChanges();
                            FpSpread1.Visible = true;
                            lblerrefp1.Visible = false;
                            tbvalue.Text = "";
                            tbvalue.Enabled = true;
                            ddloperator.Enabled = true;
                        }
                        else
                        {
                            lblerrefp1.Visible = true;
                            lblerrefp1.Text = "No Record(s) Found";
                            tbvalue.Text = "";
                            tbvalue.Enabled = false;
                            ddloperator.Enabled = false;
                            ddlheader.ClearSelection();
                            ddloperator.ClearSelection();
                            btnlookupgo1.Enabled = true;
                        }

                    }
                    FpSpread1.Sheets[0].PageSize = 12;
                    FpSpread1.TitleInfo.Height = 30;
                    if (FpSpread1.Sheets[0].RowCount > 10)
                    {
                        FpSpread1.Height = 390;
                    }
                    else
                    {
                        FpSpread1.Height = (FpSpread1.Sheets[0].RowCount * 25) + 140;
                    }

                }
            }
        }
        catch (Exception ex)
        {

            d2.sendErrorMail(ex, collegecode1, "LetterDocumentInward");
        }
    }

    protected void ddlheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlheader.SelectedItem.Text != "---Select---")
        {
            ddloperator.Enabled = true;
            btnlookupgo1.Enabled = true;

        }
        else
        {
            ddloperator.Enabled = false;
            tbvalue.Enabled = false;
        }
    }

    protected void ddloperator_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddloperator.SelectedItem.Text != "---Select---")
        {
            tbvalue.Enabled = true;
            btnlookupgo1.Enabled = true;
        }
        else
        {
            tbvalue.Enabled = false;
            btnlookupgo1.Enabled = false;
        }

    }

    protected void tbvalue_TextChanged(object sender, EventArgs e)
    {
        StudentLookup1();
    }

    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        fpcellclick = true;
    }

    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (fpcellclick == true)
            {
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.SaveChanges();
                if (fpcellclick == true)
                {
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
                        //  Panellookup.Visible = false;
                        string RollNo = "";
                        string studname = "";
                        string Dept = "";
                        string appno = "";
                        string clgcode = "";
                        string reg_no = "";
                        RollNo = FpSpread1.Sheets[0].Cells[ar, 1].Text.ToString();
                        appno = FpSpread1.Sheets[0].Cells[ar, 1].Tag.ToString();
                        reg_no = FpSpread1.Sheets[0].Cells[ar, 2].Text.ToString();

                        studname = FpSpread1.Sheets[0].Cells[ar, 3].Text.ToString();
                        Dept = FpSpread1.Sheets[0].Cells[ar, 0].Text.ToString();
                        clgcode = FpSpread1.Sheets[0].Cells[ar, 3].Note.ToString();
                        txt_roll.Text = RollNo.ToString();
                        txt_reg.Text = Convert.ToString(reg_no);
                        txt_name.Text = studname.ToString();
                        txt_department.Text = Dept.ToString();
                        Session["studstaffcollegecode"] = Convert.ToString(ddlcollegenew.SelectedValue);
                        //  photo.ImageUrl = "Handler/Handler3.ashx?id=" + appno.ToString();
                        // photo.Visible = true;
                        ViewState["Clgcode"] = clgcode;
                        bindmethods(RollNo);
                        txt_roll_Ontextchanged(sender, e);


                    }

                    fpcellclick = false;
                    Panellookup1.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "imgbtn_stud", "studentcheck();", true);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "imgbtn_stud", "get(" + txt_roll.Text + ");", true);
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "LetterDocumentInward");

        }
    }

    public void bindmethods(string roll)
    {
        try
        {
            DataSet dsval = new DataSet();
            string query = "select a.stud_name,r.Reg_No, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester, a.parent_addressC,a.parentF_Mobile,a.parentM_Mobile,a.emailM,a.parent_name, r.Sections ,r.Batch_Year,a.parent_addressP,Streetp,Cityp,parent_pincodec,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and r.Roll_no='" + roll + "'";
            dsval = d2.select_method_wo_parameter(query, "Text");
            if (dsval.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsval.Tables[0].Rows.Count; i++)
                {

                    txt_batch.Text = dsval.Tables[0].Rows[i]["Batch_Year"].ToString();
                    txt_sem.Text = dsval.Tables[0].Rows[i]["Current_Semester"].ToString();
                    txt_section.Text = dsval.Tables[0].Rows[i]["Sections"].ToString();
                    txt_addrs.Text = dsval.Tables[0].Rows[i]["parent_addressP"].ToString();
                    txt_street.Text = dsval.Tables[0].Rows[i]["Streetp"].ToString();
                    txt_city.Text = dsval.Tables[0].Rows[i]["Cityp"].ToString();
                    txt_pin_stud.Text = dsval.Tables[0].Rows[i]["parent_pincodec"].ToString();
                    txt_state.Text = dsval.Tables[0].Rows[i]["State"].ToString();
                    txt_pho.Text = dsval.Tables[0].Rows[i]["parentM_Mobile"].ToString();
                    txt_mailid.Text = dsval.Tables[0].Rows[i]["emailM"].ToString();

                }
            }
        }
        catch (Exception ex)
        {
        }

    }

    protected void txt_roll_Ontextchanged(object sende, EventArgs e)
    {

    }

    public void Studentinfo()
    {
        FpSpread1.Sheets[0].PageSize = 5;
        FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.NextPrev;
        FpSpread1.Pager.Align = HorizontalAlign.Right;
        FpSpread1.Pager.Font.Bold = true;
        // FpSpread1.Pager.ForeColor = Color.DarkGreen;
        // FpSpread1.Pager.BackColor = Color.Beige;
        // FpSpread1.Pager.BackColor = Color.AliceBlue;
        FpSpread1.Pager.PageCount = 5;
        FpSpread1.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
        FpSpread1.ActiveSheetView.SheetCorner.DefaultStyle.Font.Bold = true;
        FpSpread1.ActiveSheetView.DefaultRowHeight = 25;
        FpSpread1.ActiveSheetView.Rows.Default.Font.Name = "MS Sans Serif";
        FpSpread1.ActiveSheetView.Rows.Default.Font.Size = FontUnit.Small;
        FpSpread1.ActiveSheetView.Rows.Default.Font.Bold = false;
        FpSpread1.ActiveSheetView.Columns.Default.Font.Bold = false;
        FpSpread1.ActiveSheetView.Columns.Default.Font.Size = FontUnit.Small;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "MS Sans Serif";
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
        FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
        FpSpread1.Sheets[0].ColumnCount = 4;
        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Degree";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
        FpSpread1.Sheets[0].Columns[2].CellType = tt;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "StudentName";
        FpSpread1.Sheets[0].Columns[0].Width = 200;
        FpSpread1.Sheets[0].Columns[0].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Width = 100;
        FpSpread1.Sheets[0].Columns[2].Width = 100;
        FpSpread1.Sheets[0].Columns[3].Width = 200;
        FpSpread1.Width = 650;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.CommandBar.Visible = false;

    }

    public void imgbtn_entryReport_Click(object sender, EventArgs e)//delsi2402
    {

        entrySearch();
        // spread.Visible = true;
        // Fpspread9.Visible = true;

    }

    public void entrySearch()
    {
        try
        {
            string[] ay = txt_fromdate.Text.Split('/');
            string[] ay1 = txt_todate.Text.Split('/');
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                dt = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
                dt1 = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);

                Fpspread9.Sheets[0].Visible = true;
                Fpspread9.Sheets[0].RowHeader.Visible = false;
                Fpspread9.CommandBar.Visible = false;
                Fpspread9.Sheets[0].AutoPostBack = false;
                //Fpspread9.Sheets[0].RowCount = 0;
                Fpspread9.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread9.Sheets[0].ColumnCount = 7;
                FarPoint.Web.Spread.StyleInfo darkstyle111 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle111.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle111.ForeColor = Color.Black;
                darkstyle111.HorizontalAlign = HorizontalAlign.Center;
                Fpspread9.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle111;

                DataSet ds = new DataSet();
                string query = "select distinct ResponseStaff,CONVERT(VARCHAR(11),ActionDate,103) as ActionDate,fd.ActionStatus,ForwardPerson,agreestatus,t.BankCode,staff_code,ReqAppStatus,BranchName,DDNo,PaymentType,DDName,CONVERT(VARCHAR(11),DDDate,103) as DDDate,Priority,Paymentamount,ActionDesc,ViewStatus,App_No,MoveTo,case when MemType=1 then 'Student' when MemType=2 then 'Staff' when MemType=3 then 'Parent' when MemType=4 then 'Company' when MemType=5 then 'Individual' when MemType=6 then 'Magazine' when MemType=7 then 'Others'  end as MemType,ToDeptCode,TapalMovementsID,ToCCDeptCode,CONVERT(VARCHAR(11),TapalInwardTime,108) as TapalInwardTime,fromname,App_No,MagazineName,CONVERT(VARCHAR(11),TapalInwardDate,103) as TapalInwardDate,RefNo,TapalSubject,type,Priority,TapalType,isnull(t.ReqApproveStage,0)ReqApproveStage ,isnull(t.ReqAppStatus,0)ReqAppStatus , isnull(t.ReqApprovedStaffAppNo,0)ReqApprovedStaffAppNo from RQ_RequestHierarchy rh, TapalInward t left join letterForwardDetails fd on fd.tapalmovementid=t.tapalmovementsid where type='0'and TapalInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and usercode='" + usercode + "'";
                ds = d2.select_method_wo_parameter(query, "text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    spread.Visible = true;

                    Fpspread9.Visible = true;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Name";
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Date";//delsi2302
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Time";//delsi2302
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread9.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Types";//delsi2302
                    //Fpspread9.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    //Fpspread9.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    //Fpspread9.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    //Fpspread9.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject";//delsi2302
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Action staus";//delsi2302
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Received Person Name";//delsi2302

                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread9.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread9.Sheets[0].RowCount = 0;
                    int count = 0;
                    string getappno = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread9.Sheets[0].RowCount++;
                        count++;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["TapalMovementsID"]);
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                        string fromname = Convert.ToString(ds.Tables[0].Rows[i]["FromName"]);
                        string app = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                        string stf_code = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                        string magname = Convert.ToString(ds.Tables[0].Rows[i]["MagazineName"]);

                        if (fromname != "")
                        {

                            getappno = fromname;
                        }
                        else if (stf_code != "")
                        {

                            getappno = getstaffname(stf_code);
                        }
                        else if (app != "")
                        {
                            string stud_name = d2.GetFunction("select Stud_Name from Registration where app_no='" + app + "'");
                            getappno = stud_name;
                        }
                        else if (fromname == "" && app == "" && stf_code == "")
                        {
                            getappno = magname;
                        }
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 1].Text = getappno;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 1].Column.Width = 200;
                        string tapalinwarddate = Convert.ToString(ds.Tables[0].Rows[i]["TapalInwardDate"]);
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 2].Text = tapalinwarddate;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 2].Column.Width = 150;

                        string tapalinwardtime = Convert.ToString(ds.Tables[0].Rows[i]["TapalInwardTime"]);
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 3].Text = tapalinwardtime;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 3].Column.Width = 100;

                        string subject = Convert.ToString(ds.Tables[0].Rows[i]["TapalSubject"]);
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 4].Text = subject;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 4].Column.Width = 100;

                        //string subject = Convert.ToString(ds.Tables[0].Rows[i]["type"]);
                        //Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 4].Text = typess;
                        //Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        //Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        //Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        //Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 4].Column.Width = 100;


                        string actionstatuss = Convert.ToString(ds.Tables[0].Rows[i]["ActionStatus"]);
                        if (actionstatuss == "0" || actionstatuss == "")
                        {
                            actionstatuss = "Pending";
                        }
                        if (actionstatuss == "1")
                        {
                            actionstatuss = "Completed";
                        }
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 5].Text = actionstatuss;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 5].Column.Width = 100;

                        string receivd_personname = Convert.ToString(ds.Tables[0].Rows[i]["ForwardPerson"]);
                        string receiver = string.Empty;
                        if (receivd_personname != "")
                        {

                            receiver = getstaffname(receivd_personname);
                        }


                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 6].Text = receiver;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread9.Sheets[0].Cells[Fpspread9.Sheets[0].RowCount - 1, 6].Column.Width = 200;

                    }
                    Fpspread9.Sheets[0].PageSize = Fpspread9.Sheets[0].RowCount;
                    print.Visible = true;
                    Fpspread9.Width = 900;
                    Fpspread9.Height = 300;

                    Fpspread9.Visible = true;


                }
                else
                {
                    Fpspread9.Visible = false;
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Record Found\");", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Enter From Date and To Date\");", true);

            }


            //}


        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "LetterDocumentInward");
        }


    }

    public string getstaffname(string staff)
    {
        string codestaff = d2.GetFunction("select staff_name  from staffmaster where staff_code ='" + staff + "'");
        return codestaff;
    }

    public void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {

                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    // lbl_erroralert.Visible = false;
                }
                else
                {
                    //lbl_erroralert.Visible = true;
                    //lbl_erroralert.Text = "Enter FromDate less than or equal to the ToDate";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "LetterDocumentInward");
        }
    }

    public void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_todate.Text != "" && txt_fromdate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = date.Split('/');
                dt2 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt2 < dt1)
                {

                    txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                else
                {
                }

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "LetterDocumentInward");
        }
    }

    public void btnerr_Click(object sender, object e)
    {
        //errordiv.Visible = false;
        //lbl_error.Visible = false;
        //lbl_error.Text = "";

    }

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Letter Entry/Exit Report " + '@';
            pagename = "LetterDocumentInward.aspx";
            Printcontrolhed.loadspreaddetails(Fpspread9, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    #endregion
}