using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;

public partial class Collinfo : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();
    DAccess2 da = new DAccess2();
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    static bool chancellorflag = false;
    static bool vicechancellorflag = false;
    static bool crresflag = false;
    static bool secrflag = false;
    static bool principalflag = false;
    static bool viceprincipalflag = false;

    protected void Page_Load(object sender, EventArgs e)
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
        if (Session["collegecode"] == null)
        {
            txtUniver.Text = string.Empty;
            txtColStYr.Text = string.Empty;

            txt_paddress.Text = string.Empty;
            txtaddress2.Text = string.Empty;
            txtaddress3.Text = string.Empty;
            txtaddress4.Text = string.Empty;
            //  txtstate.Text = state;
            txtpincode.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtfaxno.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtwebsite.Text = string.Empty;
            txtacronym.Text = string.Empty;
            txtclgcode.Text = string.Empty;
            txtchancellor.Text = string.Empty;
            txtvicechancellor.Text = string.Empty;
            txtcorres.Text = string.Empty;
            txtsecretary.Text = string.Empty;
            txtprincipal.Text = string.Empty;
            txtviceprincipal.Text = string.Empty;
            txtcoe.Text = string.Empty;
            txtmassmail.Text = string.Empty;
            txtmassmailpwd.Text = string.Empty;
            txtmassmailpwd.TextMode = TextBoxMode.Password;
            txtmassmailpwd.Attributes["value"] = string.Empty;

            string val = string.Empty;
            string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            Response.Redirect(redrURI, false);
            return;
        }
        //****************************************************//
       
        if (rbcategory.Items[0].Selected == true)
        {
            txtcoe.Visible = true;
            txtchancellor.Enabled = true;
            txtvicechancellor.Enabled = true;
            lblCOE.Visible = true;
            btnAdd.Enabled = true;
            btnvicechan.Enabled = true;
        }
        else if (rbcategory.Items[1].Selected == true)
        {
            txtcoe.Visible = false;
            txtchancellor.Enabled = false;
            txtvicechancellor.Enabled = false;
            lblCOE.Visible = false;
            btnAdd.Enabled = false;
            btnvicechan.Enabled = false;
        }

        if (!IsPostBack)
        {
            string usercode = Session["usercode"].ToString();
            string qry3 = "select USER_ID from UserMaster where  User_code='" + usercode + "'";
            DataSet dslogin = da.select_method_wo_parameter(qry3, "text");
            if (dslogin.Tables.Count > 0 && dslogin.Tables[0].Rows.Count > 0)
            {
                string adminname = Convert.ToString(dslogin.Tables[0].Rows[0]["USER_ID"]);
                if (adminname.Trim().ToLower() == "palpap admin")
                {
                    txtUniver.Text = string.Empty;
                    txtColStYr.Text = string.Empty;

                    txt_paddress.Text = string.Empty;
                    txtaddress2.Text = string.Empty;
                    txtaddress3.Text = string.Empty;
                    txtaddress4.Text = string.Empty;
                    //  txtstate.Text = state;
                    txtpincode.Text = string.Empty;
                    txtphone.Text = string.Empty;
                    txtfaxno.Text = string.Empty;
                    txtemail.Text = string.Empty;
                    txtwebsite.Text = string.Empty;
                    txtacronym.Text = string.Empty;
                    txtclgcode.Text = string.Empty;
                    txtchancellor.Text = string.Empty;
                    txtvicechancellor.Text = string.Empty;
                    txtcorres.Text = string.Empty;
                    txtsecretary.Text = string.Empty;
                    txtprincipal.Text = string.Empty;
                    txtviceprincipal.Text = string.Empty;
                    txtcoe.Text = string.Empty;
                    txtmassmail.Text = string.Empty;
                    txtmassmailpwd.Text = string.Empty;
                    txtmassmailpwd.TextMode = TextBoxMode.Password;
                    txtmassmailpwd.Attributes["value"] = string.Empty;
            
                }
                else
                {
                    bindclg();
                    loadingfunct();
                }
            }


        }

       
    }


    #region

    public void bindclg()
    {
        try
        {

            ddlcollege1.Items.Clear();
            string columnfield = string.Empty;
            string group_user = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = da.select_method("bind_college", ht, "sp");
            ddlcollege1.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcollege1.DataSource = dsprint;
                ddlcollege1.DataTextField = "collname";
                ddlcollege1.DataValueField = "college_code";
                ddlcollege1.DataBind();
                //ddlcollege1.Items.Add(" ");
                //  ddlcollege1.SelectedIndex = 0;
                // ddlcollege1.Items.Add(0, " ");
            }

        }
        catch
        {
        }
    }

    public void bindstate()
    {
        string sta = "select state from collinfo where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
        DataSet ds1 = new DataSet();
        ds1 = da.select_method_wo_parameter(sta, "text");
        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
        {
            ddlstate.DataSource = ds1;
            ddlstate.DataTextField = "state";
            ddlstate.DataValueField = "state";
            ddlstate.DataBind();
        }
    }


    #endregion


    public void loadingfunct()
    {
        try
        {
            bindstate();

            ddlstate.Attributes.Add("onfocus", "frelig1()");
            string clgname = Convert.ToString(ddlcollege1.SelectedItem.Text);
            if (!string.IsNullOrEmpty(clgname))
            {
                txtCollName.Text = clgname;
                txtCollName.Enabled = false;
            }
            else
            {
                txtCollName.Text = string.Empty;
                txtCollName.Enabled = true;
            }
            string colinf = "select * from collinfo where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
            DataSet ds1 = new DataSet();
            ds1 = da.select_method_wo_parameter(colinf, "text");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                string univ = Convert.ToString(ds1.Tables[0].Rows[0]["university"]);
                string startyr = Convert.ToString(ds1.Tables[0].Rows[0]["startedyear"]);
                string category = Convert.ToString(ds1.Tables[0].Rows[0]["category"]);
                string address = Convert.ToString(ds1.Tables[0].Rows[0]["address1"]);
                string address1 = Convert.ToString(ds1.Tables[0].Rows[0]["address2"]);
                string address2 = Convert.ToString(ds1.Tables[0].Rows[0]["address3"]);
                string district = Convert.ToString(ds1.Tables[0].Rows[0]["district"]);
                string state = Convert.ToString(ds1.Tables[0].Rows[0]["state"]);
                string pincode = Convert.ToString(ds1.Tables[0].Rows[0]["pincode"]);
                string phoneno = Convert.ToString(ds1.Tables[0].Rows[0]["phoneno"]);
                string fax = Convert.ToString(ds1.Tables[0].Rows[0]["faxno"]);
                string email = Convert.ToString(ds1.Tables[0].Rows[0]["email"]);
                string website = Convert.ToString(ds1.Tables[0].Rows[0]["website"]);
                string clgcode = Convert.ToString(ds1.Tables[0].Rows[0]["college_code"]);
                string acr = Convert.ToString(ds1.Tables[0].Rows[0]["acr"]);
                string principal = Convert.ToString(ds1.Tables[0].Rows[0]["principal"]);
                string viceprincipal = Convert.ToString(ds1.Tables[0].Rows[0]["viceprincipal"]);
                string corres = Convert.ToString(ds1.Tables[0].Rows[0]["corres"]);
                string chairman = Convert.ToString(ds1.Tables[0].Rows[0]["chairman"]);
                string chancellor = Convert.ToString(ds1.Tables[0].Rows[0]["chancellor"]);
                string vicechancellor = Convert.ToString(ds1.Tables[0].Rows[0]["vicechancellor"]);
                string coe = Convert.ToString(ds1.Tables[0].Rows[0]["coe"]);
                string massmail = Convert.ToString(ds1.Tables[0].Rows[0]["Massemail"]);
                string massmailpwd = Convert.ToString(ds1.Tables[0].Rows[0]["Masspwd"]);
                string affilated = Convert.ToString(ds1.Tables[0].Rows[0]["affliatedby"]);
                string extnum = Convert.ToString(ds1.Tables[0].Rows[0]["EstNo"]);
                string[] estsplit = null;
                if (extnum.Contains('-'))
                {
                    estsplit = extnum.Split('-');
                    for (int m = 0; m < estsplit.Length; m++)
                    {
                        if (m == 0)
                            txtest1.Text = Convert.ToString(estsplit[m]);
                        if (m == 1)
                            txtest2.Text = Convert.ToString(estsplit[m]);
                        if (m == 2)
                            txtest3.Text = Convert.ToString(estsplit[m]);

                    }
                }

                txtUniver.Text = univ;
                txtColStYr.Text = startyr;
                if (category == "Autonomous")
                    rbcategory.Items[0].Selected = true;
                // rbautonomus.Checked = true;
                else if (category == "Affilated")
                    rbcategory.Items[1].Selected = true;
                // rbaffilated.Checked = true;
                txt_paddress.Text = address;
                txtaddress2.Text = address1;
                txtaddress3.Text = address2;
                txtaddress4.Text = district;
                //  txtstate.Text = state;
                txtpincode.Text = pincode;
                txtphone.Text = phoneno;
                txtfaxno.Text = fax;
                txtemail.Text = email;
                txtwebsite.Text = website;
                txtacronym.Text = acr;
                txtclgcode.Text = clgcode;
                txtchancellor.Text = chancellor;
                txtchancellor.Enabled = false;
                txtvicechancellor.Text = vicechancellor;
                txtvicechancellor.Enabled = false;
                txtcorres.Text = corres;
                txtcorres.Enabled = false;
                txtsecretary.Text = chairman;
                txtsecretary.Enabled = false;
                txtprincipal.Text = principal;
                txtprincipal.Enabled = false;
                txtviceprincipal.Text = viceprincipal;
                txtviceprincipal.Enabled = false;
                txtcoe.Text = coe;
                txtmassmail.Text = massmail;
                txtmassmailpwd.Text = massmailpwd;
                txtmassmailpwd.TextMode = TextBoxMode.Password;
                txtmassmailpwd.Attributes["value"] = massmailpwd;
                ddlstate.Items.FindByText(state).Selected = true;
                if (TextBox1.Enabled == false)
                {
                    TextBox1.ForeColor = Color.AntiqueWhite;
                }

                DataTable dtCollInfi = new DataTable();
                dtCollInfi.Columns.Add("Affiliatedby");
                dtCollInfi.Columns.Add("AffiliatedYR");

                DataRow drNEw = null;
                int countaff = 0;
                gridAffiliation.Visible = true;

                if (affilated.Contains('\\'))
                {
                    string[] spt = affilated.Split('\\');

                    for (int i = 0; i < spt.Count(); i++)
                    {

                        drNEw = dtCollInfi.NewRow();
                        countaff++;
                        string affby = Convert.ToString(spt[i]);

                        string[] split = affby.Split(',');
                        string affilate1 = string.Empty;
                        string affilate2 = string.Empty;
                        if (split.Count() > 1)
                            affilate2 = Convert.ToString(split[1]);

                        affilate1 = Convert.ToString(split[0]);

                        drNEw["Affiliatedby"] = affilate1;
                        drNEw["AffiliatedYR"] = affilate2;
                        dtCollInfi.Rows.Add(drNEw);
                    }

                }
                else
                {

                    drNEw = dtCollInfi.NewRow();
                    countaff++;
                    string affby = Convert.ToString(affilated);
                    string[] split = affby.Split(',');
                    string affilate1 = string.Empty;
                    string affilate2 = string.Empty;
                    if (split.Count() > 1)
                        affilate2 = Convert.ToString(split[1]);

                    affilate1 = Convert.ToString(split[0]);

                    drNEw["Affiliatedby"] = affilate1;
                    drNEw["AffiliatedYR"] = affilate2;
                    dtCollInfi.Rows.Add(drNEw);
                }
                if (dtCollInfi.Rows.Count > 0)
                {
                    gridAffiliation.DataSource = dtCollInfi;
                    gridAffiliation.DataBind();
                }
                TextBox1.Text = Convert.ToString(countaff);
                TextBox1.Enabled = false;

            }
            else
            {

                txtUniver.Text = string.Empty;
                txtColStYr.Text = string.Empty;

                txt_paddress.Text = string.Empty;
                txtaddress2.Text = string.Empty;
                txtaddress3.Text = string.Empty;
                txtaddress4.Text = string.Empty;
                //  txtstate.Text = state;
                txtpincode.Text = string.Empty;
                txtphone.Text = string.Empty;
                txtfaxno.Text = string.Empty;
                txtemail.Text = string.Empty;
                txtwebsite.Text = string.Empty;
                txtacronym.Text = string.Empty;
                txtclgcode.Text = string.Empty;
                txtchancellor.Text = string.Empty;
                txtvicechancellor.Text = string.Empty;
                txtcorres.Text = string.Empty;
                txtsecretary.Text = string.Empty;
                txtprincipal.Text = string.Empty;
                txtviceprincipal.Text = string.Empty;
                txtcoe.Text = string.Empty;
                txtmassmail.Text = string.Empty;
                txtmassmailpwd.Text = string.Empty;
                txtmassmailpwd.TextMode = TextBoxMode.Password;
                txtmassmailpwd.Attributes["value"] = string.Empty;

                string val = string.Empty;
            }
        }
        catch
        {
        }

    }

    protected void TextBox1_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string val = TextBox1.Text;
            DataTable dtCollInfi = new DataTable();

            DataRow drNEw = null;

            dtCollInfi.Columns.Add("Affiliatedby");
            dtCollInfi.Columns.Add("AffiliatedYR");
            for (int i = 0; i < Convert.ToInt32(val); i++)
            {

                drNEw = dtCollInfi.NewRow();

                dtCollInfi.Rows.Add(drNEw);
            }
            gridAffiliation.DataSource = dtCollInfi;
            gridAffiliation.DataBind();
        }
        catch
        {
        }
    }
    protected void btn_pls_pubpl_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "State";
            txt_addgroup.Attributes.Add("placeholder", "");
            //  txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex)
        {

        }


    }

    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            string group = Convert.ToString(txt_addgroup.Text);

            if (!string.IsNullOrEmpty(group))
            {

                if (lbl_addgroup.Text.Trim() == "State")
                {
                    int j = ddlstate.Items.Count;
                    ddlstate.Items.Insert(j, group);
                }

                plusdiv.Visible = false;
            }
            else
            {
                plusdiv.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Please Enter the " + lbl_addgroup.Text + "";
            }

        }

        catch (Exception ex) { }

    }
    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }

    protected void btnaltok_Click(object sender, EventArgs e)
    {
        div5.Visible = false;
        div4.Visible = false;
    }

    protected void btn_min_pubpl_Click(object sender, EventArgs e)
    {
        // string sta = ddlstate.SelectedItem.Text;
    }

    protected void ddlddlstate_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlcollege1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcollege1.SelectedItem.Text.ToString() == " ")
        {

            txtUniver.Text = string.Empty;
            txtColStYr.Text = string.Empty;
            txtCollName.Text = string.Empty;
            txt_paddress.Text = string.Empty;
            txtaddress2.Text = string.Empty;
            txtaddress3.Text = string.Empty;
            txtaddress4.Text = string.Empty;
            //  txtstate.Text = state;
            txtpincode.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtfaxno.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtwebsite.Text = string.Empty;
            txtacronym.Text = string.Empty;
            txtclgcode.Text = string.Empty;
            txtchancellor.Text = string.Empty;
            txtvicechancellor.Text = string.Empty;
            txtcorres.Text = string.Empty;
            txtsecretary.Text = string.Empty;
            txtprincipal.Text = string.Empty;
            txtviceprincipal.Text = string.Empty;
            txtcoe.Text = string.Empty;
            txtmassmail.Text = string.Empty;
            txtmassmailpwd.Text = string.Empty;
            //txtmassmailpwd.TextMode = TextBoxMode.Password;
            //txtmassmailpwd.Attributes["value"] = string.Empty;
            TextBox1.Enabled = true;
            TextBox1.Text = string.Empty;
            txtCollName.Enabled = true;
            ddlstate.Items[0].Text = "";
            gridAffiliation.Visible = false;
            txtmassmail.Text = string.Empty;
            txtmassmailpwd.Text = string.Empty;

            string val = string.Empty;
        }
        else
        {
            loadingfunct();
        }
    }
    protected void lnkbtsignature_OnClick(object sender, EventArgs e)
    {
        panel7.Visible = true;
    }

    protected void btndelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            string clgname = txtCollName.Text;
            string category = string.Empty;
            //if (rbaffilated.Checked == true)
            //    category = rbaffilated.Text;
            //else if (rbautonomus.Checked == true)
            //    category = rbautonomus.Text;

            string clgcode = txtclgcode.Text;


            string qry = "delete collinfo where college_code='" + clgcode + "'";// and category='" + category + "'";
            int deleteqry = da.update_method_wo_parameter(qry, "text");

            if (deleteqry > 0)
            {
                div4.Visible = true;
                div5.Visible = true;
                Label7.Visible = true;
                Label7.Text = "Deleted Successfully";
            }
            else
            {
                div4.Visible = true;
                div5.Visible = true;
                Label7.Visible = true;
                Label7.Text = "Not Deleted";
            }
        }
        catch
        {
        }
    }
    protected void Btnsave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string clgname = txtCollName.Text;
            string category = string.Empty;
            //if (rbaffilated.Checked == true)
            //    category = rbaffilated.Text;
            //else if (rbautonomus.Checked == true)
            //    category = rbautonomus.Text;
            if (rbcategory.Items[0].Selected == true)
                category = rbcategory.Items[0].Text;
            else if (rbcategory.Items[1].Selected == true)
                category = rbcategory.Items[1].Text;

            string univ = txtUniver.Text;
            string address1 = txt_paddress.Text;
            string address2 = txtaddress2.Text;
            string address3 = txtaddress3.Text;

            string phno = txtphone.Text;
            string corres = txtcorres.Text;
            string principal = txtprincipal.Text;
            string viceprinci = txtviceprincipal.Text;
            string startedyr = txtColStYr.Text;
            string faxno = txtfaxno.Text;
            string email = txtemail.Text;
            string website = txtwebsite.Text;
            string chairman = txtsecretary.Text;
            string clgcode = txtclgcode.Text;
            string acr = txtacronym.Text;
            string estno = string.Empty;
            string estno1 = txtest1.Text;
            string estno2 = txtest2.Text;
            string estno3 = txtest3.Text;
            if (!string.IsNullOrEmpty(estno1))
                estno = estno1;
            if (!string.IsNullOrEmpty(estno2))
                estno = estno + "-" + estno2;
            if (!string.IsNullOrEmpty(estno3))
                estno = estno + "-" + estno3;

            string coll_arc = txtacronym.Text;
            string chancellor = txtchancellor.Text;
            string vicechancellor = txtvicechancellor.Text;
            string district = txtaddress4.Text;
            string state = ddlstate.SelectedItem.Text.ToString();
            string pincode = txtpincode.Text;
            string coe = txtcoe.Text;
            string massemail = txtmassmail.Text;
            string massmailpwd = txtmassmailpwd.Text;
            string commonclgname = txtcommonclgname.Text;
            string affilated = string.Empty;
            string affilat = string.Empty;
            foreach (GridViewRow gr in gridAffiliation.Rows)
            {

                TextBox affilate1 = gridAffiliation.Rows[0].FindControl("txtAffiliation") as TextBox;
                string affi1 = affilate1.Text;
                TextBox affilate2 = gridAffiliation.Rows[0].FindControl("txtaffYr") as TextBox;
                string affi2 = affilate2.Text;
                if (string.IsNullOrEmpty(affilat))
                    affilat = affi1 + "," + affi2;
                else
                    affilat = affilat + "\\" + affi1 + "," + affi2;

            }


            string qry = " if exists(select * from collinfo where college_code='" + clgcode + "')update collinfo set collname='" + clgname + "',category='" + category + "',university='" + univ + "',address1='" + address1 + "',address2='" + address2 + "',address3='" + address3 + "',phoneno='" + phno + "',corres='" + corres + "',principal='" + principal + "',viceprincipal='" + viceprinci + "',startedyear='" + startedyr + "',faxno='" + faxno + "',email='" + faxno + "',website='" + website + "',chairman='" + chairman + "',acr='" + acr + "',EstNo='" + estno + "',affliatedby='" + affilat + "',Coll_acronymn='" + coll_arc + "',Chancellor='" + chancellor + "',ViceChancellor='" + vicechancellor + "',district='" + district + "',state='" + state + "',pincode='" + pincode + "',COE='" + coe + "',Massemail='" + massemail + "',Masspwd='" + massmailpwd + "',com_name='" + commonclgname + "' where college_code='" + clgcode + "' else insert into collinfo(collname,category,university,address1,address2,address3,phoneno,corres,principal,viceprincipal,startedyear,faxno,email,website,chairman,college_code,acr,EstNo,affliatedby,Coll_acronymn,Chancellor,ViceChancellor,district,state,pincode,COE,Massemail,Masspwd,com_name) values('" + clgname + "','" + category + "','" + univ + "','" + address1 + "','" + address2 + "','" + address3 + "','" + phno + "','" + corres + "','" + principal + "','" + viceprinci + "','" + startedyr + "','" + faxno + "','" + faxno + "','" + website + "','" + chairman + "','" + clgcode + "','" + acr + "','" + estno + "','" + affilat + "','" + coll_arc + "','" + chancellor + "','" + vicechancellor + "','" + district + "','" + state + "','" + pincode + "','" + coe + "','" + massemail + "','" + massmailpwd + "','" + commonclgname + "')";

            int insertqry = da.update_method_wo_parameter(qry, "text");


            if (!string.IsNullOrEmpty(clgcode))
            {
                if (ViewState["leftlogo"] != "0" && ViewState["size"] != "")
                {
                    byte[] photoid = (byte[])(ViewState["leftlogo"]);
                    int size = Convert.ToInt32(ViewState["size"]);

                    PhotoUpload("logo1", clgcode, size, photoid);

                }
                if (ViewState["rightlogo"] != "0" && ViewState["rsize"] != "")
                {
                    byte[] photoid = (byte[])(ViewState["rightlogo"]);
                    int size = Convert.ToInt32(ViewState["rsize"]);

                    PhotoUpload("logo2", clgcode, size, photoid);

                }
                if (ViewState["collegephoto"] != "0" && ViewState["ssize"] != "")
                {
                    byte[] photoid = (byte[])(ViewState["collegephoto"]);
                    int size = Convert.ToInt32(ViewState["ssize"]);

                    PhotoUpload("photo", clgcode, size, photoid);

                }
                if (ViewState["coesign"] != "0" && ViewState["sizec"] != "")
                {
                    byte[] photoid = (byte[])(ViewState["coesign"]);
                    int size = Convert.ToInt32(ViewState["sizec"]);

                    PhotoUpload("coe_signature", clgcode, size, photoid);

                }
                if (ViewState["principalsign"] != "0" && ViewState["sizep"] != "")
                {
                    byte[] photoid = (byte[])(ViewState["principalsign"]);
                    int size = Convert.ToInt32(ViewState["sizep"]);

                    PhotoUpload("principal_sign", clgcode, size, photoid);

                }
                loadimage(clgcode);
            }



            if (insertqry > 0)
            {
                div4.Visible = true;
                div5.Visible = true;
                Label7.Visible = true;
                Label7.Text = "Saved Successfully";
            }
            else
            {
                div4.Visible = true;
                div5.Visible = true;
                Label7.Visible = true;
                Label7.Text = "Not Saved";
            }
        }
        catch
        {
        }
    }


    public void loadimage(string clgcode)
    {
        try
        {
            string query = "select logo1,logo2,photo from collinfo where college_code='" + clgcode + "'";
            ds.Dispose();
            ds.Reset();
            ds = da.select_method_wo_parameter(query, "Text");
            imgstudp.ImageUrl = null;
            imgfatp.ImageUrl = null;
            imgmotp.ImageUrl = null;
            //  imggurp.ImageUrl = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["photo"] != null)
                {
                    imgfatp.Visible = true;
                    imgfatp.ImageUrl = "~/Handler/Handler.ashx?id=" + clgcode;
                }
                else
                {
                    imgfatp.Visible = false;
                }
                if (ds.Tables[0].Rows[0]["r_photo"] != null)
                {
                    imgmotp.Visible = true;
                    imgmotp.ImageUrl = "~/Handler/Rightlogo.ashx?id=" + clgcode;
                }
                else
                {
                    imgmotp.Visible = false;
                }
                if (ds.Tables[0].Rows[0]["l_photo"] != null)
                {
                    imgstudp.Visible = true;
                    imgstudp.ImageUrl = "~/Handler/Leftlogo.ashx?id=" + clgcode;
                }
                else
                {
                    imgstudp.Visible = false;
                }
                if (ds.Tables[0].Rows[0]["coe_sign"] != null)
                {
                    imgcoesign.Visible = true;
                    imgcoesign.ImageUrl = "~/Handler/CoeHandler.ashx?id=" + clgcode;
                }
                else
                {
                    imgcoesign.Visible = false;
                }
                if (ds.Tables[0].Rows[0]["principal_sign"] != null)
                {
                    imgprincisign.Visible = true;
                    imgprincisign.ImageUrl = "~/Handler/Principalsign.ashx?id=" + clgcode;
                }
                else
                {
                    imgprincisign.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {
            //  d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentNewApplicationReport");
        }
    }

    protected void btntrustinform_OnClick(object sender, EventArgs e)
    {
    }
    protected void btnlogo_OnClick(object sender, EventArgs e)
    {
        panelphoto.Visible = true;


    }


    protected int PhotoUpload(string ColumnName, string college_code, int FileSize, byte[] DocDocument)
    {
        int Result = 0;
        try
        {
            if (ColumnName.Trim() != "" && college_code.Trim() != "" && FileSize != 0)
            {
                string InsPhoto = "if exists (select " + ColumnName + " from collinfo where college_code=@college_code) update collinfo set " + ColumnName + "=@photoid where college_code=@college_code else insert into collinfo (college_code," + ColumnName + ") values(@college_code,@photoid)";
                SqlCommand cmd = new SqlCommand(InsPhoto, ssql);
                SqlParameter uploadedsubject_name = new SqlParameter("@college_code", SqlDbType.Int, 50);
                uploadedsubject_name.Value = college_code;
                cmd.Parameters.Add(uploadedsubject_name);
                uploadedsubject_name = new SqlParameter("@photoid", SqlDbType.Binary, FileSize);
                uploadedsubject_name.Value = DocDocument;
                cmd.Parameters.Add(uploadedsubject_name);
                ssql.Close();
                ssql.Open();
                Result = cmd.ExecuteNonQuery();
                ssql.Close();
            }
        }
        catch (Exception ex)
        {

        }
        return Result;
    }

   

    protected void rbcategory_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
        if (rbcategory.Items[0].Selected == true)
        {
            txtcoe.Visible = true;
            txtchancellor.Enabled = true;
            txtvicechancellor.Enabled = true;
            lblCOE.Visible = true;
            btnAdd.Enabled = true;
            btnvicechan.Enabled = true;
            
        }
        else if (rbcategory.Items[1].Selected == true)
        {
            txtcoe.Visible = false;
            txtchancellor.Enabled = false;
            txtvicechancellor.Enabled = false;
            lblCOE.Visible = false;
            btnAdd.Enabled = false;
            btnvicechan.Enabled = false;
        }

    }


    #region logosettings

    public void Btnsaveleftlogo_Click(object sender, EventArgs e)
    {

        if (fulstudp.HasFile)
        {
            if (fulstudp.FileName.EndsWith(".jpg") || fulstudp.FileName.EndsWith(".jpeg") || fulstudp.FileName.EndsWith(".JPG") || fulstudp.FileName.EndsWith(".gif") || fulstudp.FileName.EndsWith(".png"))
            {
                Session["Image"] = fulstudp.PostedFile;
                int fileSize = fulstudp.PostedFile.ContentLength;
                ViewState["size"] = fileSize;
                byte[] documentBinary = new byte[fileSize];
                ViewState["leftlogo"] = documentBinary;
                fulstudp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                // imgstudp.Visible = true;
                string base64String = Convert.ToBase64String(documentBinary, 0, documentBinary.Length);
                imgstudp.ImageUrl = "data:image/;base64," + base64String;

            }

        }

    }

    public void Btnsavclgphoto_Click(object sender, EventArgs e)
    {
        if (fulfatp.HasFile)
        {
            if (fulfatp.FileName.EndsWith(".jpg") || fulfatp.FileName.EndsWith(".jpeg") || fulfatp.FileName.EndsWith(".JPG") || fulfatp.FileName.EndsWith(".gif") || fulfatp.FileName.EndsWith(".png"))
            {
                int fileSize = fulfatp.PostedFile.ContentLength;
                ViewState["ssize"] = fileSize;
                byte[] documentBinary = new byte[fileSize];
                ViewState["collegephoto"] = documentBinary;
                fulfatp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

                string base64String = Convert.ToBase64String(documentBinary, 0, documentBinary.Length);
                imgfatp.ImageUrl = "data:image/;base64," + base64String;

            }
        }

    }
    public void btnrightlogo_Click(object sender, EventArgs e)
    {
        if (fulmp.HasFile)
        {
            if (fulmp.FileName.EndsWith(".jpg") || fulmp.FileName.EndsWith(".jpeg") || fulmp.FileName.EndsWith(".JPG") || fulmp.FileName.EndsWith(".gif") || fulmp.FileName.EndsWith(".png"))
            {
                int fileSize = fulmp.PostedFile.ContentLength;
                ViewState["rsize"] = fileSize;
                byte[] documentBinary = new byte[fileSize];
                ViewState["rightlogo"] = documentBinary;
                fulmp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

                string base64String = Convert.ToBase64String(documentBinary, 0, documentBinary.Length);
                imgmotp.ImageUrl = "data:image/;base64," + base64String;
            }
        }

    }

    public void btnrmvleftlogo_Click(object sender, EventArgs e)
    {
        try
        {
            string logo1 = "select logo1 from collinfo where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
            DataSet ds3 = new DataSet();
            ds3 = da.select_method_wo_parameter(logo1, "text");
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                string leftlogo = Convert.ToString(ds3.Tables[0].Rows[0]["logo1"]);
                if (!string.IsNullOrEmpty(leftlogo))
                {
                    string upd = "update collinfo set logo1='' where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
                    int val = da.update_method_wo_parameter(upd, "text");
                }

            }
        }
        catch (Exception ex)
        {


        }

    }

    public void btnstuph_Click(object sender, EventArgs e)
    {
        panelphoto.Visible = false;

    }

    public void btnexit_Click(object sender, EventArgs e)
    {
        panelphoto.Visible = false;
    }
    public void btnrmvrightlogo_Click(object sender, EventArgs e)
    {
        try
        {
            string logo2 = "select logo2 from collinfo where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
            DataSet ds3 = new DataSet();
            ds3 = da.select_method_wo_parameter(logo2, "text");
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                string rightlogo = Convert.ToString(ds3.Tables[0].Rows[0]["logo2"]);
                if (!string.IsNullOrEmpty(rightlogo))
                {
                    string upd = "update collinfo set logo2='' where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
                    int val = da.update_method_wo_parameter(upd, "text");
                }

            }
        }
        catch (Exception ex)
        {


        }
    }
    public void btnrmvclgphoto_Click(object sender, EventArgs e)
    {
        try
        {
            string photo = "select photo from collinfo where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
            DataSet ds3 = new DataSet();
            ds3 = da.select_method_wo_parameter(photo, "text");
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                string photo1 = Convert.ToString(ds3.Tables[0].Rows[0]["photo"]);
                if (!string.IsNullOrEmpty(photo1))
                {
                    string upd = "update collinfo set photo='' where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
                    int val = da.update_method_wo_parameter(upd, "text");
                }

            }
        }
        catch (Exception ex)
        {


        }
    }

    #endregion

    #region Staff LookUp

    protected void btnadd_OnClick(object sender, EventArgs e)
    {
        loadstaffdep();
        loadfsstaff();
        panel6.Visible = true;
        chancellorflag = true;
        vicechancellorflag = false;
        crresflag = false;
        secrflag = false;
        principalflag = false;
        viceprincipalflag = false;
    }

    protected void btnvicechan_OnClick(object sender, EventArgs e)
    {
        loadstaffdep();
        loadfsstaff();
        panel6.Visible = true;
        chancellorflag = false;
        vicechancellorflag = true;
        crresflag = false;
        secrflag = false;
        principalflag = false;
        viceprincipalflag = false;
    }


    protected void btnsecretary_OnClick(object sender, EventArgs e)
    {
        loadstaffdep();
        loadfsstaff();
        panel6.Visible = true;
        chancellorflag = false;
        vicechancellorflag = false;
        crresflag = false;
        secrflag = true;
        principalflag = false;
        viceprincipalflag = false;
    }

    protected void btnviceprin_OnClick(object sender, EventArgs e)
    {
        loadstaffdep();
        loadfsstaff();
        panel6.Visible = true;
        chancellorflag = false;
        vicechancellorflag = false;
        crresflag = false;
        secrflag = false;
        principalflag = false;
        viceprincipalflag = true;
    }
    protected void btnprincipal_OnClick(object sender, EventArgs e)
    {
        loadstaffdep();
        loadfsstaff();
        panel6.Visible = true;
        chancellorflag = false;
        vicechancellorflag = false;
        crresflag = false;
        secrflag = false;
        principalflag = true;
        viceprincipalflag = false;
    }



    protected void btnchairman_OnClick(object sender, EventArgs e)
    {
        loadstaffdep();
        loadfsstaff();
        panel6.Visible = true;
        chancellorflag = false;
        vicechancellorflag = false;
        crresflag = true;
        secrflag = false;
        principalflag = false;
        viceprincipalflag = false;
    }

    protected void loadfsstaff()
    {
        staffok.Visible = false;
        string sql = "";
        DataTable dtable1 = new DataTable();
        DataRow dtrow2;
        if (ddlstafftype.SelectedIndex != 0)
        {
            if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "select s.staff_name,dm.desig_name,st.stftype from staffmaster s,StaffCategorizer sc,stafftrans st,desig_master dm where s.staff_code=st.staff_code and s.college_code='" + ddlcollege1.SelectedValue.ToString() + "' and s.college_code=sc.college_code and st.desig_code=dm.desig_code and dm.collegeCode=s.college_code and dm.collegeCode=sc.college_code and st.category_code=sc.category_code and st.latestrec=1 and s.resign=0 and s.settled=0 and st.stftype='" + ddlstafftype.SelectedItem.Text.ToString() + "' and s.staff_name like '" + txt_search.Text + "%'";
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "select s.staff_name,dm.desig_name,st.stftype from staffmaster s,StaffCategorizer sc,stafftrans st,desig_master dm where s.staff_code=st.staff_code and s.college_code='" + ddlcollege1.SelectedValue.ToString() + "' and s.college_code=sc.college_code and st.desig_code=dm.desig_code and dm.collegeCode=s.college_code and dm.collegeCode=sc.college_code and st.category_code=sc.category_code and st.latestrec=1 and s.resign=0 and s.settled=0 and st.stftype='" + ddlstafftype.SelectedItem.Text.ToString() + "' and s.staff_code like '" + txt_search.Text + "%'";
                }
            }
            else
            {

                sql = "select s.staff_name,dm.desig_name,st.stftype from staffmaster s,StaffCategorizer sc,stafftrans st,desig_master dm where s.staff_code=st.staff_code and s.college_code='" + ddlcollege1.SelectedValue.ToString() + "' and s.college_code=sc.college_code and st.desig_code=dm.desig_code and dm.collegeCode=s.college_code and dm.collegeCode=sc.college_code and st.category_code=sc.category_code and st.latestrec=1 and s.resign=0 and s.settled=0 and st.stftype='" + ddlstafftype.SelectedItem.Text.ToString() + "'";
            }
        }
        else if (txt_search.Text != "")
        {
            if (ddlstaff.SelectedIndex == 0)
            {
                sql = "select s.staff_name,dm.desig_name,st.stftype from staffmaster s,StaffCategorizer sc,stafftrans st,desig_master dm where s.staff_code=st.staff_code and s.college_code='" + ddlcollege1.SelectedValue.ToString() + "' and s.college_code=sc.college_code and st.desig_code=dm.desig_code and dm.collegeCode=s.college_code and dm.collegeCode=sc.college_code and st.category_code=sc.category_code and st.latestrec=1 and s.resign=0 and s.settled=0  and s.staff_name like '" + txt_search.Text + "%'";
            }
            else if (ddlstaff.SelectedIndex == 1)
            {
                sql = "select s.staff_name,dm.desig_name,st.stftype from staffmaster s,StaffCategorizer sc,stafftrans st,desig_master dm where s.staff_code=st.staff_code and s.college_code='" + ddlcollege1.SelectedValue.ToString() + "' and s.college_code=sc.college_code and st.desig_code=dm.desig_code and dm.collegeCode=s.college_code and dm.collegeCode=sc.college_code and st.category_code=sc.category_code and st.latestrec=1 and s.resign=0 and s.settled=0 and s.staff_code like '" + txt_search.Text + "%'";
            }

        }
        else
        {
            if (ddlstafftype.SelectedValue.ToString() == "All")
            {

                sql = "select s.staff_name,dm.desig_name,st.stftype from staffmaster s,StaffCategorizer sc,stafftrans st,desig_master dm where s.staff_code=st.staff_code and s.college_code='" + ddlcollege1.SelectedValue.ToString() + "' and s.college_code=sc.college_code and st.desig_code=dm.desig_code and dm.collegeCode=s.college_code and dm.collegeCode=sc.college_code and st.category_code=sc.category_code and st.latestrec=1 and s.resign=0 and s.settled=0 ";
            }
        }

        DataSet dsbindspread = new DataSet();
        dsbindspread = da.select_method_wo_parameter(sql, "Text");

        if (dsbindspread.Tables[0].Rows.Count > 0)
        {
            int sno = 0;

            dtable1.Columns.Add("Staff_Name");
            dtable1.Columns.Add("designation");
            dtable1.Columns.Add("type");
            for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
            {
                sno++;
                string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                string desig = dsbindspread.Tables[0].Rows[rolcount]["desig_name"].ToString();
                string type = dsbindspread.Tables[0].Rows[rolcount]["stftype"].ToString();

                dtrow2 = dtable1.NewRow();
                dtrow2["Staff_Name"] = name;
                dtrow2["designation"] = desig;
                dtrow2["type"] = type;
                dtable1.Rows.Add(dtrow2);
            }


            gviewstaff.DataSource = dtable1;
            gviewstaff.DataBind();
            gviewstaff.Visible = true;
            Labelstaffalert.Visible = false;
            staffok.Visible = true;

        }
        else
        {
            gviewstaff.Visible = false;
            staffok.Visible = false;
            Labelstaffalert.Visible = true;

        }
    }


    public void loadstaffdep()
    {

        string cmd = "select distinct stftype from stafftrans where latestrec=1";

        DataSet ds1 = new DataSet();
        ds1 = da.select_method_wo_parameter(cmd, "Text");

        ddlstafftype.DataSource = ds1;
        ddlstafftype.DataTextField = "stftype";
        ddlstafftype.DataValueField = "stftype";
        ddlstafftype.DataBind();
        ddlstafftype.Items.Insert(0, "All");


    }
    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }

    protected void btn_staffok_Click(object sender, EventArgs e)
    {
        try
        {


            if (Session["Gridcellrowstaff"] != "" && Session["Gridcellrowstaff"] != null)
            {
                int RowIndex = Convert.ToInt32(Session["Gridcellrowstaff"]);
                if (RowIndex != -1)
                {
                    // Label lblstaffcode = (gviewstaff.Rows[RowIndex].FindControl("lblstaff") as Label);
                    Label lblstaff = (gviewstaff.Rows[RowIndex].FindControl("lblname") as Label);
                    string staffname = lblstaff.Text;
                    if (chancellorflag == true)
                    {
                        txtchancellor.Text = staffname;
                    }
                    if (vicechancellorflag == true)
                        txtvicechancellor.Text = staffname;
                    if (crresflag == true)
                        txtcorres.Text = staffname;
                    if (secrflag == true)
                        txtsecretary.Text = staffname;
                    if (principalflag == true)
                        txtprincipal.Text = staffname;
                    if (viceprincipalflag == true)
                        txtviceprincipal.Text = staffname;


                    panel6.Visible = false;


                }
            }
            else
            {

                Label3.Visible = true;
                Label3.Text = "Please Select Atleast One Staff!";

            }
        }
        catch
        {

        }
    }

    protected void ddlstafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff();
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
                   , HiddenField1.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }
    protected void SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int RowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.HiddenField1.Value);
        Session["Gridcellrowstaff"] = Convert.ToString(RowIndex);


    }
    protected void btn_staffexit_Click(object sender, EventArgs e)
    {
        panel6.Visible = false;
    }

    #endregion

    #region signature settings

    protected void btnsignexit_Click(object sender, EventArgs e)
    {
        panel7.Visible = false;
    }

    protected void btnsignsave_Click(object sendr, EventArgs e)
    {
        panel7.Visible = false;
    }


    protected void btnprinciremove_Click(object sender, EventArgs e)
    {
        try
        {
            string logo1 = "select principal_sign from collinfo where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
            DataSet ds3 = new DataSet();
            ds3 = da.select_method_wo_parameter(logo1, "text");
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                string leftlogo = Convert.ToString(ds3.Tables[0].Rows[0]["principal_sign"]);
                if (!string.IsNullOrEmpty(leftlogo))
                {
                    string upd = "update collinfo set principal_sign='' where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
                    int val = da.update_method_wo_parameter(upd, "text");
                }

            }
        }
        catch (Exception ex)
        {


        }
    }

    protected void btnprincisign_save_Click(object sendr, EventArgs e)
    {
        if (fupprincisign.HasFile)
        {
            if (fupprincisign.FileName.EndsWith(".jpg") || fupprincisign.FileName.EndsWith(".jpeg") || fupprincisign.FileName.EndsWith(".JPG") || fupprincisign.FileName.EndsWith(".gif") || fupprincisign.FileName.EndsWith(".png"))
            {
                Session["Image"] = fupprincisign.PostedFile;
                int fileSize = fupprincisign.PostedFile.ContentLength;
                ViewState["sizep"] = fileSize;
                byte[] documentBinary = new byte[fileSize];
                ViewState["principalsign"] = documentBinary;
                fupprincisign.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                // imgstudp.Visible = true;
                string base64String = Convert.ToBase64String(documentBinary, 0, documentBinary.Length);
                imgprincisign.ImageUrl = "data:image/;base64," + base64String;

            }

        }
    }

    protected void btncoeremove_Click(object sender, EventArgs e)
    {
        try
        {
            string logo1 = "select coe_signature from collinfo where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
            DataSet ds3 = new DataSet();
            ds3 = da.select_method_wo_parameter(logo1, "text");
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                string leftlogo = Convert.ToString(ds3.Tables[0].Rows[0]["coe_signature"]);
                if (!string.IsNullOrEmpty(leftlogo))
                {
                    string upd = "update collinfo set coe_signature='' where college_code='" + ddlcollege1.SelectedValue.ToString() + "'";
                    int val = da.update_method_wo_parameter(upd, "text");
                }

            }
        }
        catch (Exception ex)
        {


        }
    }

    protected void btncoesign_Click(object sendr, EventArgs e)
    {
        if (flupcoe.HasFile)
        {
            if (flupcoe.FileName.EndsWith(".jpg") || flupcoe.FileName.EndsWith(".jpeg") || flupcoe.FileName.EndsWith(".JPG") || flupcoe.FileName.EndsWith(".gif") || flupcoe.FileName.EndsWith(".png"))
            {
                Session["Image"] = flupcoe.PostedFile;
                int fileSize = flupcoe.PostedFile.ContentLength;
                ViewState["sizec"] = fileSize;
                byte[] documentBinary = new byte[fileSize];
                ViewState["coesign"] = documentBinary;
                flupcoe.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                // imgstudp.Visible = true;
                string base64String = Convert.ToBase64String(documentBinary, 0, documentBinary.Length);
                imgcoesign.ImageUrl = "data:image/;base64," + base64String;

            }

        }
    }

    #endregion





    protected void imagebtnpopclose5_Click(object sender, EventArgs e)
    {
        panel6.Visible = false;

    }

    protected void btnaddnew_OnClick(object sender, EventArgs e)
    {
    }
    protected void btnminus_OnClick(object sedner, EventArgs e)
    {
    }
}