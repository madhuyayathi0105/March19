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
using System.Configuration;

public partial class CoeMod_Exam_applicationServiceSettings : System.Web.UI.Page
{
    string collegecode = string.Empty;
    string clgcode = string.Empty;
    string usercollegecode = string.Empty;
    string singleuser = string.Empty;
    string groupuser = string.Empty;
    string usercode = string.Empty;

    Hashtable ht = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                usercollegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupuser = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
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
           
            if (!IsPostBack)
            {
                bindclg();
                stream();
                bindedulevel();
                binddegdetails();

            }
        }

        catch (Exception ex)
        {
        }
    }
    public void bindclg()
    {

        try
        {
            ddlclg.Items.Clear();
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
            ddlclg.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = dsprint;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                ddlclg.SelectedIndex = 0;

            }
        }


        catch
        {
        }
    }
    public void stream()
    {
        try
        {
            ddlstream.Items.Clear();
            string str = "select distinct type from course where college_code='" + Convert.ToString(ddlclg.SelectedValue) + "'";

            DataSet ds2 = da.select_method_wo_parameter(str, "text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds2;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
                ddlstream.SelectedIndex = 0;
            }
        }
        catch
        {
        }
    }
    public void bindedulevel()
    {
        try
        {
            cbl_edulev.Items.Clear();
            string stream = Convert.ToString(ddlstream.SelectedItem.Text);
            string colcod = Convert.ToString(ddlclg.SelectedValue);
            string edleve = "select distinct edu_level from course where college_code='" + colcod + "' and type='" + stream + "'";
            DataSet dsedu = da.select_method_wo_parameter(edleve, "text");
            if (dsedu.Tables.Count > 0 && dsedu.Tables[0].Rows.Count > 0)
            {
                cbl_edulev.DataSource = dsedu;
                cbl_edulev.DataTextField = "edu_level";
                cbl_edulev.DataValueField = "edu_level";
                cbl_edulev.DataBind();
                cbl_edulev.SelectedIndex = 0;
            }
            {
                for (int i = 0; i < cbl_edulev.Items.Count; i++)
                {
                    cbl_edulev.Items[i].Selected = true;
                }
                txt_edulevel.Text = "EduLevel(" + cbl_edulev.Items.Count + ")";
                cb_edulevel.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void binddegdetails()
    {
        try
        {
            cbl_degdetails.Items.Clear();
            string edu = "";
            for (int i = 0; i < cbl_edulev.Items.Count; i++)
            {
                if (cbl_edulev.Items[i].Selected == true)
                {
                    if (edu == "")
                    {
                        edu = "'" + Convert.ToString(cbl_edulev.Items[i].Text) + "'";
                    }
                    else
                    {
                        edu += ",'" + Convert.ToString(cbl_edulev.Items[i].Text) + "'";
                    }
                }

            }

            string degdet = "select distinct convert(nvarchar(max),(Convert(nvarchar,(r.Batch_Year))+'-'+convert(nvarchar,(c.Course_Name+'-'+ de.dept_acronym+'-'+Convert(nvarchar,(r.Current_Semester)))))) as ccc ,d.Degree_Code  from Registration r,Department de,course c,Degree d,collinfo ci where  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code   and r.college_code=ci.college_code  and CC=1 and r.college_code='" + Convert.ToString(ddlclg.SelectedValue) + "' and c.type='" + Convert.ToString(ddlstream.SelectedItem.Text) + "' and c.edu_level in (" + edu + ")  order by ccc";
            DataSet dsdegdet = da.select_method_wo_parameter(degdet, "text");
            if (dsdegdet.Tables.Count > 0 && dsdegdet.Tables[0].Rows.Count > 0)
            {
                cbl_degdetails.DataSource = dsdegdet;
                cbl_degdetails.DataTextField = "ccc";
                cbl_degdetails.DataValueField = "Degree_Code";
                cbl_degdetails.DataBind();
                cbl_degdetails.SelectedIndex = 0;
            }
            for (int i = 0; i < cbl_degdetails.Items.Count; i++)
            {
                cbl_degdetails.Items[i].Selected = true;
            }
            txtdegdetails.Text = "BatchInfo(" + cbl_degdetails.Items.Count + ")";
            cb_edulevel.Checked = true;

        }
        catch
        {
        }
    }
    protected void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlstream_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindedulevel();
        binddegdetails();
    }
    protected void cb_edulevel_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txt_edulevel.Text = "--Select--";
            if (cb_edulevel.Checked == true)
            {

                for (int i = 0; i < cbl_edulev.Items.Count; i++)
                {
                    cbl_edulev.Items[i].Selected = true;
                }
                txt_edulevel.Text = "Degree(" + (cbl_edulev.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_edulev.Items.Count; i++)
                {
                    cbl_edulev.Items[i].Selected = false;
                }
            }

        }
        catch
        {
        }
    }
    protected void cbl_edulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_edulevel.Checked = false;
            int commcount = 0;
            txt_edulevel.Text = "--Select--";
            for (i = 0; i < cbl_edulev.Items.Count; i++)
            {
                if (cbl_edulev.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_edulev.Items.Count)
                {
                    cb_edulevel.Checked = true;
                }
                txt_edulevel.Text = "EduLevel(" + commcount.ToString() + ")";
            }

            binddegdetails();
        }
        catch
        {
        }
    }
    protected void cb_degdetails_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txtdegdetails.Text = "--Select--";
            if (cb_degdetails.Checked == true)
            {

                for (int i = 0; i < cbl_degdetails.Items.Count; i++)
                {
                    cbl_degdetails.Items[i].Selected = true;
                }
                txtdegdetails.Text = "BatchInfo(" + (cbl_degdetails.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degdetails.Items.Count; i++)
                {
                    cbl_degdetails.Items[i].Selected = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void cbl_degdetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_degdetails.Checked = false;
            int commcount = 0;
            txtdegdetails.Text = "--Select--";
            for (i = 0; i < cbl_degdetails.Items.Count; i++)
            {
                if (cbl_degdetails.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degdetails.Items.Count)
                {
                    cb_degdetails.Checked = true;
                }
                txtdegdetails.Text = "BatchInfo(" + commcount.ToString() + ")";
            }

        }
        catch
        {
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string collcode = Convert.ToString(ddlclg.SelectedValue);
            string degdetails = string.Empty;
            bool check = false;
            for (int i = 0; i < cbl_degdetails.Items.Count; i++)
            {
                if (cbl_degdetails.Items[i].Selected == true)
                {
                    degdetails = Convert.ToString(cbl_degdetails.Items[i].Text);
                    string[] degsplit = degdetails.Split('-');
                    string sem = Convert.ToString(degsplit[3]);
                    string batch = Convert.ToString(degsplit[0]);
                    string degcode = Convert.ToString(cbl_degdetails.Items[i].Value);
                    string insertqry = "if not exists (select * from ExamapplicationService where collegeCode='" + collcode + "' and BatchYear='" + batch + "' and DegreeCode='" + degcode + "' and Semester='" + sem + "') insert into ExamapplicationService (collegeCode,BatchYear,DegreeCode,Semester)values('" + collcode + "','" + batch + "','" + degcode + "','" + sem + "')";
                    int insert = da.update_method_wo_parameter(insertqry, "text");
                    if (insert > 0)
                    {
                        check = true;
                    }
                }
            }
            if (check == true)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                divPopAlertContent.Visible = true;
                lblAlertMsg.Text = "Saved Successfully";
                return;
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                divPopAlertContent.Visible = true;
                lblAlertMsg.Text = "Not Saved";
                return;
            }
        }
        catch
        {
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            string collcode = Convert.ToString(ddlclg.SelectedValue);
            string degdetails = string.Empty;
            bool check = false;
            for (int i = 0; i < cbl_degdetails.Items.Count; i++)
            {
                if (cbl_degdetails.Items[i].Selected == true)
                {

                    degdetails = Convert.ToString(cbl_degdetails.Items[i].Text);
                    string[] degsplit = degdetails.Split('-');
                    string sem = Convert.ToString(degsplit[3]);
                    string batch = Convert.ToString(degsplit[0]);
                    string degcode = Convert.ToString(cbl_degdetails.Items[i].Value);

                    string delqry = "delete ExamapplicationService where collegeCode='" + collcode + "' and BatchYear='" + batch + "' and DegreeCode='" + degcode + "' and Semester='" + sem + "'";
                    int del = da.update_method_wo_parameter(delqry, "text");
                    if (del > 0)
                    {
                        check = true;
                    }



                }
            }
            if (check == true)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                divPopAlertContent.Visible = true;
                lblAlertMsg.Text = "Deleted Successfully";
                return;
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                divPopAlertContent.Visible = true;
                lblAlertMsg.Text = "Not Deleted";
                return;
            }

        }
        catch
        {
        }
    }
    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        divPopAlert.Visible = false;
        divPopAlertContent.Visible = false;
    }
}