using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using Gios.Pdf;
using System.Text;
using System.IO;
using System.Configuration;

public partial class Fee_Structure : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    string collegecode = string.Empty;
    string collegecode2 = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    static int chosedmode = 0;
    static int personmode = 0;
    static string collegecode1 = string.Empty;
    string group_user = string.Empty;
    static byte roll = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            string PageLogOut = "";
            string sess = Convert.ToString(Session["IsLogin"]);
            PageLogOut = Convert.ToString(Session["PageLogout"]);
            if (sess == "")
            {
            }
            else
            {
                if (!Request.FilePath.Contains("FinM"))
                {
                    string strPreviousPage = "";
                    if (Request.UrlReferrer != null)
                    {
                        strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                    }
                    if (strPreviousPage == "")
                    {
                        string redrURI = ConfigurationManager.AppSettings["Finance"].Trim();
                        Response.Redirect(redrURI, false);
                        return;
                    }
                }
            }
            if (Session["collegecode"] == null)
            {
                string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
                Response.Redirect(redrURI, false);
                return;
            }
            //****************************************************//
            collegecode = Convert.ToString(Session["collegecode"]);
            usercode = Convert.ToString(Session["usercode"]);
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            // lbl_stream.Text = Convert.ToString(Session["streamcode"]);
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                usercode = Session["group_code"].ToString();
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                usercode = Session["usercode"].ToString();
            }
            if (!IsPostBack)
            {
                LoadFromSettings();
                setLabelText();
                bindcollege();

                if (ddl_col.Items.Count > 0)
                {
                    collegecode2 = Convert.ToString(ddl_col.SelectedItem.Value);
                    collegecode = Convert.ToString(ddl_col.SelectedItem.Value);
                    collegecode1 = Convert.ToString(ddl_col.SelectedItem.Value);
                }
                cbYearWise.Visible = yearWise();
                loadstream();
                BindBatch();
                Bindcourse();
                binddept();
                loadsem();
                bindsec();
                bindheader();
                loadseat();
                lbl_sel.Text = "Header";
                //bindledger();
                //bindgrouphdr();
                rb_stud_Changed(sender, e);
                treeledger.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
            }
            if (ddl_col.Items.Count > 0)
            {
                collegecode2 = Convert.ToString(ddl_col.SelectedItem.Value);
                collegecode = Convert.ToString(ddl_col.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }
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
                    //query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecode1 + " order by Roll_No asc";

                    query = "select top 100 Roll_No from Registration where Roll_No like '" + prefixText + "%' and college_code=" + collegecode1 + " order by Roll_No asc";

                }
                else if (chosedmode == 1)
                {
                    //query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Reg_No asc";
                    query = "select  top 100 Reg_No from Registration where Reg_No like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where Roll_admit like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";

                    //query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";

                }
                else if (chosedmode == 4)
                {
                    query = "select  top 100 Stud_Name+'-'+Roll_No+'-'+(select c.Course_Name+'-'+dept_name from Department dt,Degree d,course c where c.Course_Id=d.Course_Id and dt.Dept_Code =d.Dept_Code and d.Degree_Code=r.degree_code) as Roll_admit from Registration r where Stud_Name like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";

                    //query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by app_formno asc";
                }
            }
            else if (personmode == 1)
            {
                query = " select top 100 staff_code from staffmaster where resign<>1 and staff_code like '" + prefixText + "%' and college_code=" + collegecode1 + " order by staff_code asc";

                //staff query

            }
            else if (personmode == 2)
            {
                //Vendor query
            }
            else
            {
                //Others query
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    private bool yearWise()
    {
        bool yearwise = true;
        byte yrWs = Convert.ToByte(d2.GetFunction("select Linkvalue from New_InsSettings where LinkName='Current Sem/Year' and user_code ='" + usercode + "' -- and college_code ='" + collegecode2 + "'").Trim());
        if (yrWs == 1)
        {
            yearwise = false;
        }
        return yearwise;
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    protected void ddl_col_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadstream();
            BindBatch();
            Bindcourse();
            binddept();
            loadsem();
            bindsec();
            bindheader();
            loadseat();
            lbl_sel.Text = "Header";
            rb_stud_Changed(sender, e);
            rb_hdr.Checked = true;
            rb_grphdr.Checked = false;
            rb_ldr.Checked = false;
        }
        catch
        {

        }
    }

    protected void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            string batch = "";
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                    batch = Convert.ToString(cbl_batch.Items[i].Text);
                }
                if (cbl_batch.Items.Count == 1)
                {
                    txt_batch.Text = "Batch(" + batch + ")";
                }
                else
                {
                    txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string buildvalue = "";
            string build = "";
            string batch = "";
            cb_batch.Checked = false;
            txt_batch.Text = "--Select--";


            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    batch = Convert.ToString(cbl_batch.Items[i].Text);
                    //cb_batch.Checked = false;
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
            if (commcount > 0)
            {
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_batch.Text = "Batch(" + batch + ")";
                }
                else
                {
                    txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cb_course_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_course.Text = "--Select--";

            if (cb_course.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_course.Items.Count; i++)
                {
                    cbl_course.Items[i].Selected = true;
                }
                txt_course.Text = lbldeg.Text + "(" + (cbl_course.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_course.Items.Count; i++)
                {
                    cbl_course.Items[i].Selected = false;

                }
                txt_course.Text = "--Select--";
            }
            binddept();

        }
        catch (Exception ex)
        {
        }
    }

    protected void cbl_course_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int commcount = 0;
            cb_course.Checked = false;
            txt_course.Text = "--Select--";
            for (i = 0; i < cbl_course.Items.Count; i++)
            {
                if (cbl_course.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_course.Items.Count)
                {
                    cb_course.Checked = true;
                }
                txt_course.Text = lbldeg.Text + " (" + commcount.ToString() + ")";
            }
            binddept();
        }
        catch (Exception ex)
        {

        }
    }

    protected void cb_dept_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_dept.Text = "--Select--";
            if (cb_dept.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                }
                txt_dept.Text = lbldept.Text + "(" + (cbl_dept.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
                txt_dept.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_dept.Checked = false;
            txt_dept.Text = "--Select--";

            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_dept.Items.Count)
                {

                    cb_dept.Checked = true;
                }
                txt_dept.Text = lbldept.Text + "(" + commcount.ToString() + ")";

            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        string sem = "";
        if (cb_sem.Checked == true)
        {
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                cbl_sem.Items[i].Selected = true;
                sem = Convert.ToString(cbl_sem.Items[i].Text);
            }
            if (lbl_sem.Text == "Semester")
            {
                if (cbl_sem.Items.Count == 1)
                {
                    txt_sem.Text = "Sem(" + sem + ")";
                }
                else
                {
                    txt_sem.Text = "Sem(" + (cbl_sem.Items.Count) + ")";
                }
            }
            if (lbl_sem.Text == "Year")
            {
                if (cbl_sem.Items.Count == 1)
                {
                    txt_sem.Text = "Year(" + sem + ")";
                }
                else
                {
                    txt_sem.Text = "Year(" + (cbl_sem.Items.Count) + ")";
                }
            }
        }
        else
        {
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                cbl_sem.Items[i].Selected = false;
            }
            txt_sem.Text = "--Select--";
        }
        bindsec();
    }

    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_sem.Text = "--Select--";
        cb_sem.Checked = false;
        string sem = "";
        int commcount = 0;
        for (int i = 0; i < cbl_sem.Items.Count; i++)
        {
            if (cbl_sem.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                sem = Convert.ToString(cbl_sem.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (lbl_sem.Text == "Semester")
            {
                if (commcount == 1)
                {
                    txt_sem.Text = "Sem(" + sem + ")";
                }
                else
                {
                    txt_sem.Text = "Sem(" + commcount.ToString() + ")";
                }
            }
            if (lbl_sem.Text == "Year")
            {
                if (commcount == 1)
                {
                    txt_sem.Text = "Year(" + sem + ")";
                }
                else
                {
                    txt_sem.Text = "Year(" + commcount.ToString() + ")";
                }
            }
            if (commcount == cbl_sem.Items.Count)
            {
                cb_sem.Checked = true;
            }
        }
        bindsec();
    }

    protected void cb_stream_CheckedChanged(object sender, EventArgs e)
    {
        string stream = "";
        if (cb_stream.Checked == true)
        {
            for (int i = 0; i < cbl_stream.Items.Count; i++)
            {
                cbl_stream.Items[i].Selected = true;
                stream = Convert.ToString(cbl_stream.Items[i].Text);
            }
            if (cbl_stream.Items.Count == 1)
                txt_stream.Text = lbl_stream.Text + "(" + stream + ")";
            else
                txt_stream.Text = lbl_stream.Text + "(" + (cbl_stream.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_stream.Items.Count; i++)
            {
                cbl_stream.Items[i].Selected = false;
            }
            txt_stream.Text = "--Select--";
        }
        Bindcourse();
        binddept();
    }

    protected void cbl_stream_SelectedIndexChanged(object sender, EventArgs e)
    {
        string stream = "";
        txt_stream.Text = "--Select--";
        cb_stream.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_stream.Items.Count; i++)
        {
            if (cbl_stream.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                stream = Convert.ToString(cbl_stream.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (commcount == 1)
            {
                txt_stream.Text = lbl_stream.Text + "(" + stream + ")";
            }
            else
            {
                txt_stream.Text = lbl_stream.Text + "(" + commcount.ToString() + ")";
            }

            if (commcount == cbl_stream.Items.Count)
            {
                cb_stream.Checked = true;
            }
        }
        // loadstream();
        Bindcourse();
        binddept();
    }

    protected void rb_hdr_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lbl_sel.Text = "Header";
            bindheader();
        }
        catch
        {

        }
    }

    protected void rb_ldr_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lbl_sel.Text = "Ledger";
            bindledger();
        }
        catch
        {

        }
    }

    protected void rb_grphdr_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lbl_sel.Text = "Group Header";
            bindgrouphdr();
        }
        catch
        {

        }
    }

    protected void cb_grp_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_grp.Checked == true)
            {
                for (int i = 0; i < cbl_grp.Items.Count; i++)
                {
                    cbl_grp.Items[i].Selected = true;
                }
                if (lbl_sel.Text == "Header")
                {
                    txt_grp.Text = "Header(" + (cbl_grp.Items.Count) + ")";
                }
                if (lbl_sel.Text == "Ledger")
                {
                    txt_grp.Text = "Ledger(" + (cbl_grp.Items.Count) + ")";
                }
                if (lbl_sel.Text == "Group Header")
                {
                    txt_grp.Text = "Group Header(" + (cbl_grp.Items.Count) + ")";
                }
            }

            else
            {
                for (int i = 0; i < cbl_grp.Items.Count; i++)
                {
                    cbl_grp.Items[i].Selected = false;
                }
                txt_grp.Text = "--Select--";
            }
            //--------------------------------
            if (rb_ldr.Checked == true)
            {
                if (cb_grp.Checked == true)
                {
                    for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                    {
                        treeledger.Nodes[remv].Checked = true;
                        txt_grp.Text = "Header(" + (treeledger.Nodes.Count) + ")";
                        if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                        {
                            for (int child = 0; child < treeledger.Nodes[remv].ChildNodes.Count; child++)
                            {
                                treeledger.Nodes[remv].ChildNodes[child].Checked = true;
                            }
                        }
                    }
                }
                else
                {
                    for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                    {
                        treeledger.Nodes[remv].Checked = false;
                        txt_grp.Text = "Header(" + (treeledger.Nodes.Count) + ")";
                        if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                        {
                            for (int child = 0; child < treeledger.Nodes[remv].ChildNodes.Count; child++)
                            {
                                treeledger.Nodes[remv].ChildNodes[child].Checked = false;
                            }
                        }
                    }
                }
            }


            //-------------------------------------------------------------------------------------------

        }
        catch
        {

        }
    }

    protected void cbl_grp_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_grp.Text = "--Select--";
        cb_grp.Checked = false;
        string clg = "";
        int commcount = 0;
        for (int i = 0; i < cbl_grp.Items.Count; i++)
        {
            if (cbl_grp.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                if (clg == "")
                    clg = cbl_grp.Items[i].Value.ToString();
                else
                    clg = clg + "," + cbl_grp.Items[i].Value;
            }
        }

        for (int i = 0; i < cbl_grp.Items.Count; i++)
        {
            if (cbl_grp.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        //-------------------------------

        //--------------------------

        if (commcount > 0)
        {
            if (lbl_sel.Text == "Header")
            {
                txt_grp.Text = "Header(" + commcount.ToString() + ")";
            }
            if (lbl_sel.Text == "Ledger")
            {
                txt_grp.Text = "Ledger(" + commcount.ToString() + ")";
            }
            if (lbl_sel.Text == "Group Header")
            {
                txt_grp.Text = "Group Header(" + commcount.ToString() + ")";
            }
            if (commcount == cbl_grp.Items.Count)
            {
                cb_grp.Checked = true;
            }
        }
    }

    protected void cb_sect_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sect.Text = "--Select--";
            string sec = "";
            if (cb_sect.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sect.Items.Count; i++)
                {
                    cbl_sect.Items[i].Selected = true;
                    sec = Convert.ToString(cbl_sect.Items[i].Text);
                }
                if (cbl_sect.Items.Count == 1)
                {
                    txt_sect.Text = "Section(" + sec + ")";
                }
                else
                {
                    txt_sect.Text = "Section(" + (cbl_sect.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbl_sect.Items.Count; i++)
                {
                    cbl_sect.Items[i].Selected = false;
                }
                txt_sect.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void cbl_sect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string sec = "";
            int commcount = 0;
            txt_sect.Text = "--Select--";
            cb_sect.Checked = false;

            for (int i = 0; i < cbl_sect.Items.Count; i++)
            {
                if (cbl_sect.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    sec = Convert.ToString(cbl_sect.Items[i].Text);
                    cb_sect.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sect.Items.Count)
                {
                    cb_sect.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_sect.Text = "Section(" + sec + ")";
                }
                else
                {
                    txt_sect.Text = "Section(" + commcount.ToString() + ")";
                }
            }
        }

        catch (Exception ex)
        {

        }
    }

    protected void cb_seat_CheckedChanged(object sender, EventArgs e)
    {
        string seat = "";
        if (cb_seat.Checked == true)
        {
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                cbl_seat.Items[i].Selected = true;
                seat = Convert.ToString(cbl_seat.Items[i].Text);
            }
            if (cbl_seat.Items.Count == 1)
            {
                txt_seat.Text = "" + seat + "";
            }
            else
            {
                txt_seat.Text = "Seat(" + (cbl_seat.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                cbl_seat.Items[i].Selected = false;
            }
            txt_seat.Text = "--Select--";
        }

    }
    protected void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_seat.Text = "--Select--";
        string seat = "";
        cb_seat.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_seat.Items.Count; i++)
        {
            if (cbl_seat.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                seat = Convert.ToString(cbl_seat.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_seat.Items.Count)
            {
                cb_seat.Checked = true;
            }
            if (commcount == 1)
            {
                txt_seat.Text = "" + seat + "";
            }
            else
            {
                txt_seat.Text = "Seat(" + commcount.ToString() + ")";
            }
        }

    }
    public void loadseat()
    {

        try
        {

            cbl_seat.Items.Clear();

            string seat = "";
            string deptquery = "select distinct TextCode,TextVal from TextValTable  where TextCriteria='seat' and college_code='" + collegecode2 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_seat.DataSource = ds;
                cbl_seat.DataTextField = "TextVal";
                cbl_seat.DataValueField = "TextCode";
                cbl_seat.DataBind();

                if (cbl_seat.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_seat.Items.Count; i++)
                    {
                        cbl_seat.Items[i].Selected = true;
                        seat = Convert.ToString(cbl_seat.Items[i].Text);
                    }
                    if (cbl_seat.Items.Count == 1)
                    {
                        txt_seat.Text = "Seat(" + seat + ")";
                    }
                    else
                    {
                        txt_seat.Text = "Seat(" + cbl_seat.Items.Count + ")";
                    }
                    cb_seat.Checked = true;
                }
            }
            else
            {
                txt_seat.Text = "--Select--";

            }
        }
        catch
        {
        }

    }

    protected void Cell_Click(object sender, EventArgs e)
    {

    }

    protected void Fpspread1_render(object sendere, EventArgs e)
    {

    }

    protected void FpSpread1_ButtonCommand(object sender, EventArgs e)
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
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch { }
    }

    protected string getOrderBy()
    {
        string orderStr = string.Empty;
        try
        {
            RollAndRegSettings();
            if (roll == 0)
                orderStr = " Order by roll_no,reg_no,roll_admit ";
            else if (roll == 1)
                orderStr = " Order by roll_no,reg_no,roll_admit ";
            else if (roll == 2)
                orderStr = " Order by roll_no ";
            else if (roll == 3)
                orderStr = " Order by reg_no ";
            else if (roll == 4)
                orderStr = " Order by roll_admit ";
            else if (roll == 5)
                orderStr = " Order by roll_no,reg_no ";
            else if (roll == 6)
                orderStr = " Order by reg_no,roll_admit ";
            else if (roll == 7)
                orderStr = " Order by roll_no,roll_admit ";
        }
        catch { }
        return orderStr;
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            string headerid = "";
            string ledgerid = "";
            string degreecode = "";
            string batchyear = "";
            string deptcode = "";
            string sem = "";
            string groupheader = "";
            string seat = "";

            Hashtable hscolidx = new Hashtable();
            hscolidx.Clear();

            double total = 0.00;
            double studtot = 0.00;
            DataView dvtot = new DataView();
            DataTable dttot = new DataTable();
            Hashtable hstot = new Hashtable();
            hstot.Clear();
            //string delquery = "delete from co_mastervalues where mastercriteria='referencenumber' and collegecode='" + collegecode2 + "'";
            //d2.update_method_wo_parameter(delquery, "Text");
            #region get value
            if (rb_hdr.Checked == true)
            {
                for (int i = 0; i < cbl_grp.Items.Count; i++)
                {
                    if (cbl_grp.Items[i].Selected == true)
                    {
                        if (headerid.Trim() == "")
                        {
                            headerid = "" + cbl_grp.Items[i].Value + "";
                        }
                        else
                        {
                            headerid = headerid + "'" + "," + "'" + cbl_grp.Items[i].Value;
                        }
                    }
                }
            }

            if (rb_ldr.Checked == true)
            {
                //for (int i = 0; i < cbl_grp.Items.Count; i++)
                //{
                //    if (cbl_grp.Items[i].Selected == true)
                for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                {
                    if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int j = 0; j < treeledger.Nodes[remv].ChildNodes.Count; j++)
                        {
                            if (treeledger.Nodes[remv].ChildNodes[j].Checked == true)
                            {
                                if (ledgerid.Trim() == "")
                                {
                                    ledgerid = "" + treeledger.Nodes[remv].ChildNodes[j].Value + "";//treeledger.Nodes[remv].ChildNodes[j].Value
                                }
                                else
                                {
                                    ledgerid = ledgerid + "'" + "," + "'" + treeledger.Nodes[remv].ChildNodes[j].Value;
                                }
                            }
                        }
                    }
                }
            }

            if (rb_grphdr.Checked == true)
            {
                for (int i = 0; i < cbl_grp.Items.Count; i++)
                {
                    if (cbl_grp.Items[i].Selected == true)
                    {
                        if (groupheader.Trim() == "")
                        {
                            groupheader = "" + cbl_grp.Items[i].Value + "";
                        }
                        else
                        {
                            groupheader = groupheader + "'" + "," + "'" + cbl_grp.Items[i].Value;
                        }
                    }
                }
            }

            for (int i = 0; i < cbl_course.Items.Count; i++)
            {
                if (cbl_course.Items[i].Selected == true)
                {
                    if (degreecode.Trim() == "")
                    {
                        degreecode = "" + cbl_course.Items[i].Value + "";
                    }
                    else
                    {
                        degreecode = degreecode + "'" + "," + "'" + cbl_course.Items[i].Value;
                    }
                }
            }

            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batchyear.Trim() == "")
                    {
                        batchyear = "" + cbl_batch.Items[i].Value + "";
                    }
                    else
                    {
                        batchyear = batchyear + "'" + "," + "'" + cbl_batch.Items[i].Value;
                    }
                }
            }

            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    if (deptcode.Trim() == "")
                    {
                        deptcode = "" + cbl_dept.Items[i].Value + "";
                    }
                    else
                    {
                        deptcode = deptcode + "'" + "," + "'" + cbl_dept.Items[i].Value;
                    }
                }
            }
            string rollno = "";
            if (rbstudtype.SelectedItem.Text == "Single")
            {
                if (ddladmit.SelectedItem.Value == "0")
                {
                    rollno = txtno.Text.Trim();
                }
                if (ddladmit.SelectedItem.Value == "1")
                {
                    rollno = txtno.Text.Trim();
                }
                if (ddladmit.SelectedItem.Value == "4")
                {
                    rollno = txtno.Text.Split('-')[1];
                }
                if (ddladmit.SelectedItem.Value == "3")
                {
                    rollno = txtno.Text.Split('-')[1];
                }

            }
            if (rbstudtype.SelectedItem.Text == "Multiple")
            {
                for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                {
                    int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[i, 1].Value);
                    if (checkval == 1)
                    {


                        if (rollno == "")
                            rollno = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 2].Tag);
                        else
                            rollno = rollno + "'" + "," + "'" + Convert.ToString(Fpspread2.Sheets[0].Cells[i, 2].Tag);
                    }
                }
            }

            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (sem.Trim() == "")
                    {
                        sem = "" + cbl_sem.Items[i].Value + "";
                    }
                    else
                    {
                        sem = sem + "'" + "," + "'" + cbl_sem.Items[i].Value;
                    }
                }
            }

            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                if (cbl_seat.Items[i].Selected == true)
                {
                    if (seat.Trim() == "")
                    {
                        seat = "" + cbl_seat.Items[i].Value + "";
                    }
                    else
                    {
                        seat = seat + "'" + "," + "'" + cbl_seat.Items[i].Value;
                    }
                }
            }
            #endregion


            if (headerid.Trim() != "" || ledgerid.Trim() != "" || groupheader.Trim() != "")
            {

                Fpspread1.Sheets[0].RowHeader.Visible = false;
                if (rb_stud.Checked == true)
                {
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].ColumnCount = 12;
                }
                if (rb_dept.Checked == true)
                {
                    if (cbdeptcumul.Checked)
                    {
                        if (rb_ldr.Checked)
                            Fpspread1.Sheets[0].ColumnHeader.RowCount = 2;
                        else
                            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    }
                    else
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].ColumnCount = 8;
                }
                Fpspread1.Sheets[0].RowCount = 1;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();

                FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                chk.AutoPostBack = false;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;
                Fpspread1.Sheets[0].FrozenRowCount = 1;

                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                Dictionary<string, int> dtcolHd = new Dictionary<string, int>();
                string mulDeptStr = string.Empty;

                if (rb_stud.Checked == true)
                {
                    #region stud design
                    RollAndRegSettings();
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    Fpspread1.Sheets[0].Cells[0, 1].CellType = chkall;
                    Fpspread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[1].Visible = true;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll Number";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg Number";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[3].Width = 125;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission Number";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[4].Width = 125;
                    //155

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[5].Width = 180;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Batch";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = lbldeg.Text;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = lbldept.Text;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = lbl_sem.Text;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Section";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Reference No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[11].Width = 125;
                    spreadColumnVisible();
                    #endregion
                }
                if (rb_dept.Checked == true)
                {

                    #region dept design
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[0].Width = 50;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[1].Width = 75;
                    if (cbdeptcumul.Checked)
                        Fpspread1.Sheets[0].Columns[1].Visible = false;
                    else
                    {
                        Fpspread1.Sheets[0].Cells[0, 1].CellType = chkall;
                        Fpspread1.Sheets[0].Cells[0, 1].Value = 0;
                        Fpspread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Columns[1].Visible = true;
                    }


                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lbldeg.Text;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[2].Width = 100;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = lbldept.Text;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[3].Width = 263;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Batch Year";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[4].Width = 75;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lbl_sem.Text;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[5].Width = 75;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Seat Type";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[6].Width = 75;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Reference No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[7].Width = 75;

                    if (cbdeptcumul.Checked)
                    {
                        if (rb_ldr.Checked)
                        {
                            #region old

                            string SelectQ = "   select distinct headername,h.headerpk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode='" + ddl_col.SelectedValue + "'  and LedgerPK in('" + ledgerid + "') order by h.headerpk";
                            SelectQ += "   select headername,h.headerpk,ledgerpk,ledgername from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode='" + ddl_col.SelectedValue + "'  and LedgerPK in('" + ledgerid + "') ";
                            //and l.HeaderFK in('" + headerid + "')
                            DataSet dshd = new DataSet();
                            dshd = d2.select_method_wo_parameter(SelectQ, "Text");
                            if (cbl_grp.Items.Count > 0)
                            {
                                int hedcnt = 6;
                                bool plus = true; ;
                                // for (int hed = 0; hed < cbl_grp.Items.Count; hed++)
                                for (int hed = 0; hed < dshd.Tables[0].Rows.Count; hed++)
                                {
                                    if (plus == true)
                                    {
                                        hedcnt++;
                                        plus = false;
                                    }
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                                    int cnt = 0;
                                    dshd.Tables[1].DefaultView.RowFilter = "headerpk='" + dshd.Tables[0].Rows[hed]["headerpk"] + "'";
                                    DataView dvhed = dshd.Tables[1].DefaultView;
                                    for (int sel = 0; sel < dvhed.Count; sel++)
                                    {
                                        Fpspread1.Sheets[0].ColumnCount++;
                                        cnt++;
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dvhed[sel]["ledgername"]);
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dvhed[sel]["ledgerpk"]);
                                        dtcolHd.Add(Convert.ToString(dvhed[sel]["ledgerpk"]), Fpspread1.Sheets[0].ColumnCount - 1);
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                    if (cnt != 0)
                                    {
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].Text = Convert.ToString(dshd.Tables[0].Rows[hed]["headername"]);
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].ForeColor = ColorTranslator.FromHtml("#000000");
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].Font.Bold = true;
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, hedcnt, 1, cnt);
                                        hedcnt += cnt;
                                    }
                                    // }
                                }
                                Fpspread1.Sheets[0].ColumnCount++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Total";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - 1, 2, 1);
                            }
                            #endregion
                        }
                        else
                        {
                            for (int row = 0; row < cbl_grp.Items.Count; row++)
                            {
                                if (cbl_grp.Items[row].Selected)
                                {
                                    int conCnt = Fpspread1.Sheets[0].ColumnCount++;
                                    dtcolHd.Add(Convert.ToString(cbl_grp.Items[row].Value), conCnt);
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, conCnt].Text = cbl_grp.Items[row].Text;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, conCnt].Tag = cbl_grp.Items[row].Value;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, conCnt].Font.Bold = true;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, conCnt].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, conCnt].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, conCnt].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }

                            Fpspread1.Sheets[0].ColumnCount++;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Total";
                            //Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = cbl_grp.Items[row].Value;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    #endregion
                }
                string selq = "";
                string sql = "";
                string strOrderBy = getOrderBy();
                if (rb_stud.Checked == true)
                {
                    selq = "select r.Roll_No,r.App_No,r.Reg_No,r.roll_admit,r.Stud_Name,r.Batch_Year,c.Course_Name,dt.Dept_Name,r.Current_Semester,r.Sections,r.degree_code,r.college_code,c.type from Course c,Degree d,Department dt,Registration r where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.college_code='" + ddl_col.SelectedValue + "' and (CC=0 or cc=1) and DelFlag =0 and Exam_Flag <>'Debar'";

                    if (batchyear.Trim() != "")
                        selq = selq + " and r.Batch_Year in('" + batchyear + "')";
                    if (deptcode.Trim() != "")
                        selq = selq + "  and d.Degree_Code in('" + deptcode + "')";
                    if (txtno.Text.Trim() != "")
                    {
                        selq = selq + " and r.roll_no in('" + rollno + "')";
                    }
                    if (rbstudtype.SelectedItem.Text == "Multiple")
                    {
                        selq = selq + " and r.roll_no in('" + rollno + "')";
                    }
                    if (!string.IsNullOrEmpty(strOrderBy))
                        selq += strOrderBy;
                }

                if (rb_dept.Checked == true)
                {
                    #region dept
                    selq = "select distinct BatchYear,c.Course_Name,dt.dept_name,r.degreecode,SeatType  from FT_FeeAllotDegree r,Degree d,Department dt,Course c,registration e where r.degreecode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and BatchYear > 0";
                    if (batchyear.Trim() != "")
                    {
                        selq = selq + " and r.BatchYear in('" + batchyear + "')";
                    }
                    if (degreecode.Trim() != "")
                    {
                        selq = selq + " and c.Course_Id in('" + degreecode + "')";
                    }
                    if (deptcode.Trim() != "")
                    {
                        selq = selq + "  and r.DegreeCode in('" + deptcode + "')";
                    }
                    if (sem != "")
                    {
                        selq = selq + " and r.FeeCategory in('" + sem + "')";
                    }
                    if (deptcode.Trim() != "")
                    {
                        selq = selq + "  and r.SeatType in('" + seat + "')";
                    }
                    if (txtno.Text.Trim() != "")
                    {
                        selq = selq + " and e.roll_no in('" + rollno + "')";
                    }

                    if (rbstudtype.SelectedItem.Text == "Multiple")
                    {
                        selq = selq + " and e.roll_no in('" + rollno + "')";
                    }
                    selq = selq + " order by r.degreecode,r.BatchYear desc";
                    selq = selq + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode2 + "'";
                    selq = selq + " select TextCode,textval from textvaltable where TextCriteria='seat' and college_code='" + collegecode2 + "'";
                    if (cbdeptcumul.Checked)
                    {

                        if (rb_hdr.Checked == true)
                        {
                            selq += " select distinct f.HeaderFK,FeeCategory,sum (TotalAmount) as TotalAmount,degreeCode from FT_FeeAllotDegree f where degreecode  in('" + deptcode + "') and BatchYear in('" + batchyear + "') and seattype in('" + seat + "')  and f.FeeCategory in('" + sem + "') group by f.HeaderFK ,FeeCategory,degreeCode order by f.HeaderFK ";
                            mulDeptStr = " and Headerfk";
                        }
                        if (rb_ldr.Checked == true)
                        {
                            selq += " select distinct f.LedgerFK,FeeCategory,sum (TotalAmount) as TotalAmount,degreeCode from FT_FeeAllotDegree f where degreecode in('" + deptcode + "') and BatchYear in('" + batchyear + "') and seattype in('" + seat + "')  and f.FeeCategory in('" + sem + "')  group by f.LedgerFK ,FeeCategory,degreeCode  order by f.LedgerFK ";
                            mulDeptStr = " and Ledgerfk";
                        }

                        if (rb_grphdr.Checked)
                        {
                            string stream = string.Empty;
                            stream = d2.GetFunction("select type from course c,degree d where c.course_id=d.course_id and d.degree_code=" + deptcode + "").Trim();
                            selq += "   select fs.ChlGroupHeader,FeeCategory,sum (TotalAmount) as TotalAmount,degreeCode from FT_FeeAllotDegree f ,FS_ChlGroupHeaderSettings fs where fs.HeaderFK =f.HeaderFK and degreecode in('" + deptcode + "') and BatchYear in ('" + batchyear + "') and seattype in('" + seat + "')  and f.FeeCategory in('" + sem + "') and Stream in('" + stream + "')  group by fs.ChlGroupHeader ,FeeCategory,degreeCode  ";
                            mulDeptStr = " and ChlGroupHeader";
                        }
                    }
                    #endregion
                }
                DataView dvnew = new DataView();
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                double endtotal = 0.0;
                double grandtotal = 0.0;

                Fpspread1.SaveChanges();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (rb_stud.Checked == true)
                    {
                        #region stud
                        FarPoint.Web.Spread.TextCellType txtRoll = new FarPoint.Web.Spread.TextCellType();
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            endtotal = 0.00;
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[row + 1, 0].Text = Convert.ToString(row + 1);
                            Fpspread1.Sheets[0].Cells[row + 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 0].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].Cells[row + 1, 1].Value = 0;
                            Fpspread1.Sheets[0].Cells[row + 1, 1].CellType = chk;
                            Fpspread1.Sheets[0].Cells[row + 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 1].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].Cells[row + 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 2].CellType = txtRoll;
                            Fpspread1.Sheets[0].Cells[row + 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["type"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 2].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].Cells[row + 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 3].CellType = txtRoll;
                            Fpspread1.Sheets[0].Cells[row + 1, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 3].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].Cells[row + 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 4].CellType = txtRoll;
                            Fpspread1.Sheets[0].Cells[row + 1, 4].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 4].HorizontalAlign = HorizontalAlign.Center;


                            Fpspread1.Sheets[0].Cells[row + 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 5].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 5].HorizontalAlign = HorizontalAlign.Left;

                            Fpspread1.Sheets[0].Cells[row + 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Batch_Year"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 6].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 6].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].Cells[row + 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Course_Name"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 7].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 7].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 7].HorizontalAlign = HorizontalAlign.Left;

                            Fpspread1.Sheets[0].Cells[row + 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Name"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 8].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 8].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 8].HorizontalAlign = HorizontalAlign.Left;

                            Fpspread1.Sheets[0].Cells[row + 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 9].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 9].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 9].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].Cells[row + 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[row]["Sections"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 10].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 10].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 10].HorizontalAlign = HorizontalAlign.Center;



                            string number = d2.GetFunction("select mastervalue from co_mastervalues where mastercriteria='ReferenceNumber' and collegecode='" + collegecode2 + "'");
                            if (number != "" && number != "0")
                            {
                                string acr = d2.GetFunction("select MasterCriteriaValue2 from co_mastervalues where mastercriteria='ReferenceNumber' and collegecode='" + collegecode2 + "'");
                                string size = d2.GetFunction("select MasterCriteriaValue1 from co_mastervalues where mastercriteria='ReferenceNumber' and collegecode='" + collegecode2 + "'");
                                int x = Convert.ToInt32(number);


                                string recenoString = number.ToString();
                                int sizeval = Convert.ToInt32(size);
                                if (sizeval != recenoString.Length && sizeval > recenoString.Length)
                                {
                                    while (sizeval != recenoString.Length)
                                    {
                                        recenoString = "0" + recenoString;
                                    }
                                }
                                number = acr + recenoString;

                                Fpspread1.Sheets[0].Cells[row + 1, 11].BackColor = ColorTranslator.FromHtml("lightyellow");
                                Fpspread1.Sheets[0].Cells[row + 1, 11].Text = Convert.ToString(number);
                                Fpspread1.Sheets[0].Cells[row + 1, 11].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].Cells[row + 1, 11].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[row + 1, 11].Font.Size = FontUnit.Medium;
                                string updateReferNumber = " update co_mastervalues set MasterValue=" + recenoString + "+1 where  mastercriteria='referencenumber' and collegecode ='" + collegecode2 + "'";
                                d2.update_method_wo_parameter(updateReferNumber, "Text");
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[row + 1, 11].BackColor = ColorTranslator.FromHtml("lightyellow");
                                Fpspread1.Sheets[0].Cells[row + 1, 11].Text = "";
                                Fpspread1.Sheets[0].Cells[row + 1, 11].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].Cells[row + 1, 11].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[row + 1, 11].Font.Size = FontUnit.Medium;
                            }
                            // number = string.Empty;

                            // lstrcpt.Text = Convert.ToString(number);
                            // x = Convert.ToInt32(number);
                            //++x;

                        }
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        Fpspread1.SaveChanges();
                        Fpspread1.Width = 900;
                        Fpspread1.Height = 500;
                        div1.Visible = true;
                        Fpspread1.Visible = true;
                        lbl_err.Visible = false;
                        #endregion
                    }
                    if (rb_dept.Checked == true)
                    {
                        #region dept
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            endtotal = 0.00;
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[row + 1, 0].Text = Convert.ToString(row + 1);
                            Fpspread1.Sheets[0].Cells[row + 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 0].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].Cells[row + 1, 1].CellType = chk;
                            Fpspread1.Sheets[0].Cells[row + 1, 1].Value = 0;
                            Fpspread1.Sheets[0].Cells[row + 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 1].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread1.Sheets[0].Cells[row + 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Course_Name"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["degreecode"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 2].HorizontalAlign = HorizontalAlign.Left;

                            Fpspread1.Sheets[0].Cells[row + 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Name"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 3].HorizontalAlign = HorizontalAlign.Left;

                            Fpspread1.Sheets[0].Cells[row + 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["BatchYear"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 4].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 4].HorizontalAlign = HorizontalAlign.Center;


                            string TextName = "";
                            //DataView Dview = new DataView();
                            //if (ds.Tables[1].Rows.Count > 0)
                            //{
                            //    ds.Tables[1].DefaultView.RowFilter = "TextCode='" + Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]) + "'";
                            //    Dview = ds.Tables[1].DefaultView;
                            //    if (Dview.Count > 0)
                            //    {
                            //        TextName = Convert.ToString(Dview[0]["TextVal"]);
                            //    }
                            //}
                            Fpspread1.Sheets[0].Cells[row + 1, 5].Text = TextName;
                            // Fpspread1.Sheets[0].Cells[row + 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 5].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Columns[5].Visible = false;
                            string seatname = "";
                            DataView dvst = new DataView();
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "TextCode='" + Convert.ToString(ds.Tables[0].Rows[row]["SeatType"]) + "'";
                                dvst = ds.Tables[2].DefaultView;
                                if (dvst.Count > 0)
                                {
                                    seatname = Convert.ToString(dvst[0]["TextVal"]);
                                }
                            }
                            Fpspread1.Sheets[0].Cells[row + 1, 6].Text = seatname;
                            Fpspread1.Sheets[0].Cells[row + 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[row]["SeatType"]);
                            Fpspread1.Sheets[0].Cells[row + 1, 6].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 1, 6].HorizontalAlign = HorizontalAlign.Left;



                            string number = d2.GetFunction("select mastervalue from co_mastervalues where mastercriteria='ReferenceNumber' and collegecode='" + collegecode2 + "'");
                            if (number != "" && number != "0")
                            {
                                string acr = d2.GetFunction("select MasterCriteriaValue2 from co_mastervalues where mastercriteria='ReferenceNumber' and collegecode='" + collegecode2 + "'");
                                string size = d2.GetFunction("select MasterCriteriaValue1 from co_mastervalues where mastercriteria='ReferenceNumber' and collegecode='" + collegecode2 + "'");
                                int x = Convert.ToInt32(number);


                                string recenoString = number.ToString();
                                int sizeval = Convert.ToInt32(size);
                                if (sizeval != recenoString.Length && sizeval > recenoString.Length)
                                {
                                    while (sizeval != recenoString.Length)
                                    {
                                        recenoString = "0" + recenoString;
                                    }
                                }
                                number = acr + recenoString;

                                Fpspread1.Sheets[0].Cells[row + 1, 7].BackColor = ColorTranslator.FromHtml("lightyellow");
                                Fpspread1.Sheets[0].Cells[row + 1, 7].Text = Convert.ToString(number);
                                Fpspread1.Sheets[0].Cells[row + 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].Cells[row + 1, 7].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[row + 1, 7].Font.Size = FontUnit.Medium;
                                string updateReferNumber = " update co_mastervalues set MasterValue=" + recenoString + "+1 where  mastercriteria='referencenumber' and collegecode ='" + collegecode2 + "'";
                                d2.update_method_wo_parameter(updateReferNumber, "Text");
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[row + 1, 7].BackColor = ColorTranslator.FromHtml("lightyellow");
                                Fpspread1.Sheets[0].Cells[row + 1, 7].Text = "";
                                Fpspread1.Sheets[0].Cells[row + 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread1.Sheets[0].Cells[row + 1, 7].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[row + 1, 7].Font.Size = FontUnit.Medium;
                            }






                            if (cbdeptcumul.Checked)
                            {
                                if (ds.Tables[3].Rows.Count > 0)
                                {
                                    DataView dvhd = new DataView();
                                    double fnlamt = 0;
                                    foreach (KeyValuePair<string, int> hdValue in dtcolHd)
                                    {
                                        double Amount = 0;
                                        int colIndex = 0;
                                        string hdFk = hdValue.Key.ToString();
                                        int.TryParse(hdValue.Value.ToString(), out colIndex);
                                        ds.Tables[3].DefaultView.RowFilter = "degreeCode='" + Convert.ToString(ds.Tables[0].Rows[row]["degreecode"]) + "'  " + mulDeptStr + "='" + hdFk + "'";
                                        dvhd = ds.Tables[3].DefaultView;
                                        if (dvhd.Count > 0)
                                            double.TryParse(Convert.ToString(dvhd[0]["TotalAmount"]), out Amount);
                                        fnlamt += Amount;
                                        Fpspread1.Sheets[0].Cells[row + 1, colIndex].Text = Convert.ToString(Amount);
                                        Fpspread1.Sheets[0].Cells[row + 1, colIndex].Font.Name = "Book Antiqua";
                                        Fpspread1.Sheets[0].Cells[row + 1, colIndex].Font.Size = FontUnit.Medium;
                                        Fpspread1.Sheets[0].Cells[row + 1, colIndex].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                    Fpspread1.Sheets[0].Cells[row + 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamt);
                                    Fpspread1.Sheets[0].Cells[row + 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[row + 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[row + 1, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                }
                            }
                        }
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        Fpspread1.SaveChanges();
                        Fpspread1.Width = 712;
                        Fpspread1.Height = 400;
                        div1.Visible = true;
                        Fpspread1.Visible = true;
                        lbl_err.Visible = false;
                        if (cbdeptcumul.Checked)
                            print.Visible = true;
                        else
                            print.Visible = false;
                        #endregion
                    }
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    lbl_err.Visible = true;
                    lbl_err.Text = "No Record Found!";
                }
            }
            else
            {
                div1.Visible = false;
                Fpspread1.Visible = false;
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select Corresponding Values!";
            }
        }
        catch
        {

        }
    }

    public bool checkedOK()
    {
        bool Ok = false;
        Fpspread1.SaveChanges();
        for (int i = 1; i < Fpspread1.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(Fpspread1.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                Ok = true;
            }
        }
        return Ok;
    }

    public string semcode(string semval)
    {
        //string type = string.Empty;
        //string strtype = d2.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
        //if (strtype == "1")
        //    type = "Yearly";
        //else if (strtype == "2")
        //    type = "Term";
        //else
        //    type = "Semester";
        string year = "";
        string sem = ""; ;
        if (semval.Trim() == "1")
        {
            year = "I";
            sem = "I";
        }
        else if (semval.Trim() == "2")
        {
            year = "I";
            sem = "II";
        }
        else if (semval.Trim() == "3")
        {
            year = "II";
            sem = "III";
        }
        else if (semval.Trim() == "4")
        {
            year = "II";
            sem = "IV";
        }
        else if (semval.Trim() == "5")
        {
            year = "III";
            sem = "V";
        }
        else if (semval.Trim() == "6")
        {
            year = "III";
            sem = "VI";
        }
        else if (semval.Trim() == "7")
        {
            year = "IV";
            sem = "VII";
        }
        else if (semval.Trim() == "8")
        {
            year = "IV";
            sem = "VIII";
        }
        return year;//sem
    }

    protected void btnprint_click(object sender, EventArgs e)
    {
        Print();
    }
    private void Print()
    {
        try
        {
            if (checkedOK())
            {
                Fpspread1.SaveChanges();
                string odd_r_even = "";
                string semes = "";
                Font FontHeader = new Font("Times New Roman", 18, FontStyle.Bold);
                Font FontHeaderAf = new Font("Times New Roman", 12, FontStyle.Bold);
                Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
                Font FontMedium = new Font("Times New Roman", 12, FontStyle.Regular);
                Font FontText = new Font("Times New Roman", 14, FontStyle.Regular);
                Font Fontsmall = new Font("Times New Roman", 9, FontStyle.Regular);
                Font Fontbold1 = new Font("Times New Roman", 12, FontStyle.Bold);
                //Font Fontboldunder1 = new Font("Book Antiqua", 12, FontStyle.Underline);
                Font Fontbodybold = new Font("Times New Roman", 10, FontStyle.Bold);
                int coltop = 0;
                bool stuflag = false;
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                Gios.Pdf.PdfPage mypdfpage;

                Gios.Pdf.PdfDocument mypdf = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);


                Hashtable hschk = new Hashtable();

                #region for Student PDF
                if (rb_stud.Checked == true)
                {
                    for (int i = 1; i < Fpspread1.Rows.Count; i++)
                    {
                        byte check = Convert.ToByte(Fpspread1.Sheets[0].Cells[i, 1].Value);
                        if (check == 1)
                        {
                            #region basedata
                            mypdfpage = mydoc.NewPage();
                            coltop = 100;
                            string year = "";
                            string sem = "";
                            string AppNo = "";
                            string gender = "";
                            string finalyr = "";
                            string refno = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 11].Text);
                            string rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 2].Text);
                            string regno = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 3].Text);
                            string studname = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 5].Text);
                            string batchyear = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 6].Text);
                            string degree = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 7].Text);
                            string dept = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 8].Text);
                            string semoryear = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 9].Text);
                            string Streamcheck = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 2].Tag);
                            year = semcode(semoryear);
                            string section = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 10].Text);
                            AppNo = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 2].Tag);
                            AppNo = d2.GetFunction("select App_No from Registration where Roll_No='" + rollno + "'");
                            string sex = d2.GetFunction("select sex from applyn where app_no='" + AppNo + "'");
                            if (sex.Trim() == "0")
                                gender = "Mr";
                            else
                                gender = "Ms";

                            double durat = 0;
                            double rmnyr = 0;
                            double addyr = 0;
                            string degcode = d2.GetFunction("select degree_code from Registration where Roll_No='" + rollno + "'");
                            double.TryParse(Convert.ToString(d2.GetFunction("select Duration from degree where degree_code='" + degcode + "'")), out durat);
                            rmnyr = durat / 2;
                            addyr = Convert.ToDouble(batchyear) + rmnyr;
                            finalyr = batchyear + " - " + Convert.ToString(addyr);
                            // changed to current acadmic year
                            int curYEar = 0;
                            int.TryParse(Convert.ToString(DateTime.Now.ToString("yyyy")), out curYEar);
                            finalyr = curYEar + "-" + Convert.ToString(curYEar + 1);
                            if (txtacd.Text.Trim() != string.Empty)
                                finalyr = Convert.ToString(txtacd.Text);

                            //for new college --------------------------
                            string settingType = string.Empty;
                            settingType = "0";
                            string acdYear = string.Empty;
                            ArrayList arFeecat = new ArrayList();
                            Dictionary<string, string> CurSemDeg = new Dictionary<string, string>();
                            string commondist = "";
                            string cursem = d2.GetFunction("select current_semester from Registration where Roll_No='" + rollno + "'");
                            string course_all = d2.GetFunction("select degree_code from Registration where Roll_No='" + rollno + "'");
                            string curSem = getCurrentSemester(batchyear, course_all, ref arFeecat, commondist, ref CurSemDeg);//current semester only
                            feeCatVal(cursem, arFeecat);
                            Dictionary<string, string> htfeecat = new Dictionary<string, string>();
                            htfeecat = (Dictionary<string, string>)ViewState["feecat"];
                            //     Dictionary<string, string> htfeecat = new Dictionary<string, string>();
                            htfeecat = (Dictionary<string, string>)ViewState["feecat"];
                         
                            string feecatval = "";
                            string feecatg = "";
                            if (htfeecat.Count > 0)
                            {
                                foreach (KeyValuePair<string, string> semesval in htfeecat)
                                {
                                    if (feecatval == "")
                                        feecatval = semesval.Value.ToString();
                                    else
                                        feecatval += "'" + "," + "'" + semesval.Value.ToString();
                                }
                            }
                            string selQuery = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_BATCH_YEAR in('" + batchyear + "') and ACD_SETTING_TYPE='" + settingType + "' and ACD_FEECATEGORY in('" + feecatval + "')  order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
                            DataSet dsPrevAMount = da.select_method_wo_parameter(selQuery, "Text");
                            if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
                            {
                                acdYear = Convert.ToString(dsPrevAMount.Tables[0].Rows[0]["ACD_YEAR"]);
                            }

                            finalyr = "ACADEMIC YEAR :" + " " + acdYear;



                            //-------------------------------------------------





                            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3,affliatedby from collinfo where college_code=" + ddl_col.SelectedItem.Value + " ";

                            string collegename = "";
                            string add1 = "";
                            string add2 = "";
                            string add3 = "";
                            string univ = "";
                            string affliated = "";
                            string feedet = "";
                            ds = d2.select_method_wo_parameter(colquery, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                                affliated = Convert.ToString(ds.Tables[0].Rows[0]["affliatedby"]);
                                // univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                            }

                            #region Added by Idhris - 03-08-2016

                            int hdrCnt = 0;
                            string stream = string.Empty;
                            if (cbl_stream.Items.Count > 0)
                            {
                                for (int str = 0; str < cbl_stream.Items.Count; str++)
                                {
                                    if (cbl_stream.Items[str].Selected)
                                        if (string.IsNullOrEmpty(stream))
                                            stream = cbl_stream.Items[str].Value;
                                        else
                                            stream = "','" + cbl_stream.Items[str].Value;
                                }
                                stream = " and h.Stream in ('" + stream + "') ";
                            }
                            StringBuilder chklstIds = new StringBuilder();
                            for (int chk = 0; chk < cbl_grp.Items.Count; chk++)
                            {
                                if (cbl_grp.Items[chk].Selected)
                                {
                                    if (chklstIds.Length == 0)
                                        chklstIds.Append(cbl_grp.Items[chk].Value);
                                    else
                                        chklstIds.Append("','" + cbl_grp.Items[chk].Value);
                                    hdrCnt++;
                                }
                            }

                            if (rb_ldr.Checked == true)
                            {
                                for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                                {
                                    if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                                    {
                                        for (int j = 0; j < treeledger.Nodes[remv].ChildNodes.Count; j++)
                                        {
                                            if (treeledger.Nodes[remv].ChildNodes[j].Checked == true)
                                            {
                                                if (chklstIds.Length == 0)
                                                    chklstIds.Append(treeledger.Nodes[remv].ChildNodes[j].Value);
                                                else
                                                    chklstIds.Append("','" + treeledger.Nodes[remv].ChildNodes[j].Value);
                                                hdrCnt++;
                                            }
                                        }
                                    }
                                }
                            }
                            #region query
                            if (rb_hdr.Checked == true)
                            {
                                feedet = " select distinct HeaderFK as uid,h.HeaderName  as name  from FT_FeeAllot f,FM_HeaderMaster h where f.HeaderFK =h.HeaderPK and f.App_No ='" + AppNo + "' and HeaderFK in ('" + chklstIds.ToString() + "') ";
                                if (rbl_PayablePaid.SelectedIndex == 0)
                                {
                                    feedet = feedet + " select SUM(totalamount),HeaderFK  as uid,FeeCategory from FT_FeeAllot f,FM_HeaderMaster h where f.HeaderFK =h.HeaderPK and f.App_No ='" + AppNo + "'  and HeaderFK in ('" + chklstIds.ToString() + "')   group by HeaderFK,FeeCategory ";
                                }
                                else
                                {
                                    feedet = feedet + " select SUM(debit),HeaderFK  as uid,FeeCategory from FT_FinDailyTransaction f,FM_HeaderMaster h where f.HeaderFK =h.HeaderPK and f.App_No ='" + AppNo + "'  and HeaderFK in ('" + chklstIds.ToString() + "')   group by HeaderFK,FeeCategory ";
                                    feedet = feedet + " select distinct convert(varchar(10),transdate,103) as transdate ,FeeCategory  from FT_FinDailyTransaction f,FM_HeaderMaster h where f.HeaderFK =h.HeaderPK and f.App_No ='" + AppNo + "'  and HeaderFK in ('" + chklstIds.ToString() + "')   order by transdate ";
                                }
                            }
                            if (rb_ldr.Checked == true)
                            {
                                //    feedet = "  select distinct LedgerPK  as uid,h.LedgerName as name ,h.priority  from FT_FeeAllot f,FM_LedgerMaster h where f.LedgerFK  =h.LedgerPK and f.App_No ='" + AppNo + "' and LedgerFK in ('" + chklstIds.ToString() + "')";
                                feedet = "select distinct HeaderAcr+'-'+LedgerName as name,h.HeaderPK,l.LedgerPK as uid,l.priority  from FM_LedgerMaster l,FM_HeaderMaster h,FT_FeeAllot f where f.LedgerFK  =l.LedgerPK and f.HeaderFK=h.HeaderPK and l.HeaderFK=h.HeaderPK and l.CollegeCode=h.CollegeCode and f.App_No ='" + AppNo + "' and f.LedgerFK in ('" + chklstIds.ToString() + "')";//modified by abarna 8.12.2017
                                //order by isnull(h.priority,1000), h.ledgerName asc
                                if (rbl_PayablePaid.SelectedIndex == 0)
                                {
                                    feedet = feedet + " select SUM(totalamount),LedgerFK as uid,FeeCategory,h.priority,h.ledgerName from FT_FeeAllot f,FM_LedgerMaster h where f.LedgerFK  =h.LedgerPK and f.App_No ='" + AppNo + "' and LedgerFK in ('" + chklstIds.ToString() + "') group by LedgerFK,FeeCategory,h.priority,h.ledgerName order by isnull(h.priority,1000), h.ledgerName asc ";
                                }
                                else
                                {
                                    feedet = feedet + " select SUM(debit),LedgerFK as uid,FeeCategory from FT_FinDailyTransaction f,FM_LedgerMaster h where f.LedgerFK  =h.LedgerPK and f.App_No ='" + AppNo + "' and LedgerFK in ('" + chklstIds.ToString() + "') group by LedgerFK,FeeCategory ";
                                    feedet = feedet + " select distinct convert(varchar(10),transdate,103) as transdate ,FeeCategory from FT_FinDailyTransaction f,FM_LedgerMaster h where f.LedgerFK  =h.LedgerPK and f.App_No ='" + AppNo + "' and LedgerFK in ('" + chklstIds.ToString() + "') order by transdate ";
                                }
                            }
                            if (rb_grphdr.Checked)
                            {
                                feedet = " select distinct h.ChlGroupHeader as name, h.ChlGroupHeader as uid     from FT_FeeAllot f,FS_ChlGroupHeaderSettings h where f.HeaderFK  =h.HeaderFK and f.App_No ='" + AppNo + "' and h.ChlGroupHeader in ('" + chklstIds.ToString() + "') " + stream + " ";

                                if (rbl_PayablePaid.SelectedIndex == 0)
                                {
                                    feedet = feedet + "   select SUM(totalamount),FeeCategory, h.ChlGroupHeader  as uid   from FT_FeeAllot f,FS_ChlGroupHeaderSettings h where f.HeaderFK  =h.HeaderFK and f.App_No ='" + AppNo + "' and h.ChlGroupHeader in ('" + chklstIds.ToString() + "')  " + stream + "  group by FeeCategory, h.ChlGroupHeader ";
                                }
                                else
                                {
                                    feedet = feedet + "   select SUM(debit),FeeCategory, h.ChlGroupHeader  as uid   from FT_FinDailyTransaction f,FS_ChlGroupHeaderSettings h where f.HeaderFK  =h.HeaderFK and f.App_No ='" + AppNo + "' and h.ChlGroupHeader in ('" + chklstIds.ToString() + "')  " + stream + "  group by FeeCategory, h.ChlGroupHeader ";
                                    feedet = feedet + "   select  distinct convert(varchar(10),transdate,103) as transdate ,FeeCategory  from FT_FinDailyTransaction f,FS_ChlGroupHeaderSettings h where f.HeaderFK  =h.HeaderFK and f.App_No ='" + AppNo + "' and h.ChlGroupHeader in ('" + chklstIds.ToString() + "')  " + stream + "   order by transdate  ";
                                }
                            }
                            #endregion
                            DataSet dsValues = new DataSet();
                            dsValues = d2.select_method_wo_parameter(feedet, "Text");
                            if (dsValues.Tables.Count > 1 && dsValues.Tables[0].Rows.Count > 0 && dsValues.Tables[1].Rows.Count > 0)
                            {
                                DataTable dsOpTbl = new DataTable();

                                dsOpTbl.Columns.Add("Semester");

                                for (int hdr = 0; hdr < dsValues.Tables[0].Rows.Count; hdr++)
                                {
                                    dsOpTbl.Columns.Add(Convert.ToString(dsValues.Tables[0].Rows[hdr]["name"]));
                                }

                                //if (rb_ldr.Checked == true)//
                                //{
                                //    for (int hdr = 0; hdr < dsValues.Tables[0].Rows.Count; hdr++)
                                //    {
                                //        dsOpTbl.Columns.Add(Convert.ToString(dsValues.Tables[0].Rows[hdr]["name"]));
                                //    }
                                //}
                                dsOpTbl.Columns.Add("Total");
                                DataRow drNRow = dsOpTbl.NewRow();

                                for (int hdr = 0; hdr < dsValues.Tables[0].Rows.Count; hdr++)
                                {
                                    drNRow[hdr + 1] = Convert.ToString(dsValues.Tables[0].Rows[hdr]["uid"]);//
                                }
                                dsOpTbl.Rows.Add(drNRow);

                                if (dsOpTbl.Rows.Count > 0)
                                {
                                    for (int semest = 0; semest < cbl_sem.Items.Count; semest++)
                                    {
                                        if (!cbl_sem.Items[semest].Selected)
                                            continue;
                                        string semName = cbl_sem.Items[semest].Text;
                                        string semValue = cbl_sem.Items[semest].Value;
                                        bool addOk = false;
                                        DataRow drRow = dsOpTbl.NewRow();
                                        drRow["Semester"] = semName;
                                        double totalValue = 0;
                                        for (int hdrVal = 1; hdrVal < dsOpTbl.Columns.Count - 1; hdrVal++)
                                        {
                                            string hdrVa = Convert.ToString(dsOpTbl.Rows[0][hdrVal]);
                                            dsValues.Tables[1].DefaultView.RowFilter = " uid='" + hdrVa + "' and FeeCategory='" + semValue + "'";
                                            DataView dvAmt = dsValues.Tables[1].DefaultView;
                                            if (dvAmt.Count > 0)
                                            {
                                                double val = 0;
                                                double.TryParse(Convert.ToString(dvAmt[0][0]), out val);
                                                drRow[hdrVal] = val;
                                                totalValue += val;
                                                addOk = true;
                                            }
                                            else
                                            {
                                                drRow[hdrVal] = "0";
                                            }
                                            if (hdrVal == dsOpTbl.Columns.Count - 2)
                                                drRow[++hdrVal] = totalValue;
                                        }
                                        if (addOk)
                                            dsOpTbl.Rows.Add(drRow);
                                    }

                                    if (dsOpTbl.Rows.Count > 1)
                                    {
                                        if (ddlFormat.SelectedIndex == 0)
                                        {
                                            double grandTotal = 0;
                                            #region for StudWise PDF Generation  Format I

                                            #region RAY
                                            stuflag = true;
                                            if (cbIncHeader.Checked)
                                            {

                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo(" + collegecode + ").jpeg")))
                                                {
                                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo(" + collegecode + ").jpeg"));
                                                    mypdfpage.Add(LogoImage, 25, 25, 600);
                                                    // mypdfpage.Add(LogoImage, 20, 30, 300);

                                                }
                                                else
                                                {
                                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("../college/left_logo(" + collegecode + ").jpg"));
                                                    mypdfpage.Add(LogoImage, 25, 25, 600);
                                                }

                                                //college address

                                                PdfTextArea clgOffText = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 25, 350, 20), ContentAlignment.MiddleCenter, collegename);
                                                mypdfpage.Add(clgOffText);

                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/right_logo(" + collegecode + ").jpeg")))
                                                {
                                                    PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/right_Logo(" + collegecode + ").jpeg"));
                                                    mypdfpage.Add(LogoImage1, 500, 25, 600);
                                                    //  mypdfpage.Add(LogoImage1, 400, 30, 300);
                                                    //mypdfpage.Add(LogoImage1, 500, 40, 200);
                                                }
                                                else
                                                {
                                                    PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode + ").jpg"));
                                                    mypdfpage.Add(LogoImage1, 500, 25, 600);
                                                }

                                                //Line2                                        


                                                add1 += " " + add2;
                                                PdfTextArea addText = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 45, 350, 20), ContentAlignment.MiddleCenter, add1);
                                                mypdfpage.Add(addText);

                                                PdfTextArea addres3 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 65, 350, 20), ContentAlignment.MiddleCenter, add3);
                                                mypdfpage.Add(addres3);

                                                PdfTextArea univer = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 85, 350, 20), ContentAlignment.MiddleCenter, univ);
                                                //mypdfpage.Add(univer);//
                                                //mypdfpage.Add(univer);

                                                //   PdfTextArea addOffText = new PdfTextArea(FontHeader, Color.Black, new PdfArea(mydoc, 120, 55, 350, 20), ContentAlignment.MiddleCenter, add1);

                                                // PdfTextArea addOffText = new PdfTextArea(FontHeaderAf, Color.Black, new PdfArea(mydoc, 70, 95, 450, 20), ContentAlignment.BottomLeft, affliated);
                                                PdfTextArea addOffText = new PdfTextArea(FontHeaderAf, Color.Black, new PdfArea(mydoc, 70, 105, 450, 20), ContentAlignment.BottomLeft, affliated);
                                                mypdfpage.Add(addOffText);


                                            }

                                            //PdfTextArea ptc = new PdfTextArea(Fontbold1, Color.Black, new PdfArea(mydoc, 0, coltop, 550, 40), System.Drawing.ContentAlignment.MiddleRight, "Date: " + DateTime.Now.ToString("dd/MM/yyyy"));
                                            coltop += 18;
                                            PdfTextArea ptc1 = new PdfTextArea(Fontbold1, Color.Black, new PdfArea(mydoc, 30, coltop, 550, 40), System.Drawing.ContentAlignment.MiddleLeft, "Reference No: " + refno);
                                            mypdfpage.Add(ptc1);
                                            PdfTextArea ptc = new PdfTextArea(Fontbold1, Color.Black, new PdfArea(mydoc, 0, coltop, 550, 40), System.Drawing.ContentAlignment.MiddleRight, "Date: " + DateTime.Now.ToString("dd/MM/yyyy"));
                                            mypdfpage.Add(ptc);
                                            coltop = coltop + 50;
                                            PdfTextArea ptsadd = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 0, coltop, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, "CERTIFICATE");
                                            mypdfpage.Add(ptsadd);
                                            PdfLine certLine = new PdfLine(mydoc, new PointF((float)(mydoc.PageWidth / 2) - 60, coltop + 18), new PointF((float)(mydoc.PageWidth / 2) + 65, coltop + 18), Color.Black, 1);
                                            mypdfpage.Add(certLine);
                                            string tempStr = string.Empty;
                                            if (ddladmit.SelectedItem.Value == "1")
                                            {
                                                tempStr = "              This is to certify that " + gender + ". " + studname + "  (Register No : " + regno + " ) " + "\n" + "\n" + "is studying " +  "in " + year + " Year " + degree + "(" + dept + ")" + " during the " + " " + "academic year  " + finalyr + ".";//+ "\n" + "\n"


                                                //    tempStr = "              This is to certify that " + gender + ". " + studname + "  (Register No : " + regno + " ) " + "is studying " + "\n" + "\n" + "in " + year + " Year " + degree + "(" + dept + ")" + " during the " + " " + "academic year  " + finalyr + ".";
                                            }
                                            else
                                            {
                                                tempStr = "              This is to certify that " + gender + ". " + studname + "  (Roll No : " + rollno + " ) " + "is studying in " + year + " Year " + degree + "\n" + "\n" + "(" + dept + ")" + " during the " + " " + "academic year  " + finalyr + ".";//+ "\n" + "\n"
                                            }

                                            //string fnal = tempStr + "\n" + temp;
                                            coltop += 30;
                                            PdfTable pdftblContent = mydoc.NewTable(Fontbold1, 1, 1, 5);
                                            pdftblContent.VisibleHeaders = false;
                                            pdftblContent.SetBorders(Color.Black, 1, BorderType.None);
                                            pdftblContent.Cell(0, 0).SetContent(tempStr);
                                            pdftblContent.Cell(0, 0).SetCellPadding(5);
                                            pdftblContent.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);

                                            PdfTablePage pdftblPage = pdftblContent.CreateTablePage(new PdfArea(mydoc, 40, coltop, mydoc.PageWidth - 50, 200));
                                            mypdfpage.Add(pdftblPage);
                                            coltop += (int)pdftblPage.Area.Height;
                                            ////   finalyr
                                            string strText = string.Empty;
                                            if (rbl_PayablePaid.SelectedIndex == 0)
                                                strText = "College semester fees is as follows:-";
                                            else
                                                strText = "College semester fees is paid as follows:-";
                                            coltop = coltop + 10;
                                            PdfTextArea ptnxtcont = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 50, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, strText);
                                            mypdfpage.Add(ptnxtcont);
                                            PdfLine certLines = new PdfLine(mydoc, new PointF((float)(mydoc.PageWidth / 2) - 247, coltop + 18), new PointF((float)(mydoc.PageWidth / 2) - 70, coltop + 18), Color.Black, 1);
                                            mypdfpage.Add(certLines);

                                            int lastRow = dsOpTbl.Columns.Count + 1;//
                                            if (dsValues.Tables.Count > 2)//
                                            {
                                                lastRow += 1;//
                                            }
                                            //Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, dsOpTbl.Rows.Count, dsOpTbl.Columns.Count, 1);//
                                            Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, (lastRow), dsOpTbl.Rows.Count, 1);
                                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table.VisibleHeaders = false;
                                            lastRow -= 1;
                                            int colHeight = 0;
                                            for (int semVal = 0; semVal < dsOpTbl.Rows.Count; semVal++)
                                            {
                                                colHeight++;
                                                for (int colval = 0; colval < dsOpTbl.Columns.Count; colval++)
                                                {
                                                    if (semVal == 0)
                                                    {
                                                        //table.Cell(semVal, colval).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        //table.Cell(semVal, colval).SetCellPadding(5);
                                                        //table.Cell(semVal, colval).SetContent(dsOpTbl.Columns[colval].ColumnName);

                                                        table.Cell(colval, semVal).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        table.Cell(colval, semVal).SetCellPadding(5);
                                                        if (colval == 0)
                                                        {
                                                            table.Cell(colval, semVal).SetContent("PARTICULARS");
                                                            table.Cell(colval, semVal).SetFont(Fontbold1);//abarna
                                                        }
                                                        else
                                                            table.Cell(colval, semVal).SetContent(dsOpTbl.Columns[colval].ColumnName.ToUpper());
                                                    }
                                                    else
                                                    {
                                                        //table.Cell(semVal, colval).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        //table.Cell(semVal, colval).SetCellPadding(5);
                                                        //table.Cell(semVal, colval).SetContent(Convert.ToString(dsOpTbl.Rows[semVal][colval]));

                                                        table.Cell(colval, semVal).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table.Cell(colval, semVal).SetCellPadding(5);
                                                        table.Cell(colval, semVal).SetContent(Convert.ToString(dsOpTbl.Rows[semVal][colval]));
                                                        table.Cell(colval, semVal).SetFont(Fontbold1);//abarna
                                                        if (colval == (dsOpTbl.Columns.Count - 1))
                                                        {
                                                            double tot = 0;
                                                            double.TryParse(Convert.ToString(dsOpTbl.Rows[semVal][colval]), out tot);
                                                            grandTotal += tot;
                                                        }
                                                    }
                                                }
                                                table.Cell((lastRow), 0).SetContent("GRAND TOTAL");//
                                                table.Cell((lastRow), 0).SetContentAlignment(ContentAlignment.MiddleLeft);//
                                                table.Cell((lastRow), 0).SetFont(Fontbold1);//
                                                table.Cell((lastRow), 1).SetContent(grandTotal);//
                                                table.Cell((lastRow), 1).SetFont(Fontbold1);//
                                                table.Cell((lastRow), 1).SetContentAlignment(ContentAlignment.MiddleCenter);//
                                                table.Cell((lastRow), 1).ColSpan = (dsOpTbl.Rows.Count - 1);//
                                            }

                                            #region princisign
                                            //string coesignphtsql = string.Empty;
                                            //coesignphtsql = "select principal_sign from collinfo where college_code='" + Convert.ToString(ddl_col.SelectedValue).Trim() + "'";
                                            //MemoryStream memoryStream = new MemoryStream();
                                            //DataSet dscoesig = new DataSet();
                                            //// DataSet dsstdpho = new DataSet();
                                            //dscoesig.Clear();
                                            //dscoesig.Dispose();
                                            //dscoesig = da.select_method_wo_parameter(coesignphtsql, "Text");
                                            //if (dscoesig.Tables.Count > 0 && dscoesig.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dscoesig.Tables[0].Rows[0][0]).Trim()))
                                            //{
                                            //    byte[] file = (byte[])dscoesig.Tables[0].Rows[0][0];
                                            //    memoryStream.Write(file, 0, file.Length);
                                            //    if (file.Length > 0)
                                            //    {
                                            //        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            //        System.Drawing.Image thumb = imgx.GetThumbnailImage(250, 250, null, IntPtr.Zero);
                                            //        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + Convert.ToString(ddl_col.SelectedValue).Trim() + ".jpeg")))
                                            //        {
                                            //        }
                                            //        else
                                            //        {
                                            //            thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + Convert.ToString(ddl_col.SelectedValue).Trim() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            //        }
                                            //    }
                                            //}
                                            //if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + Convert.ToString(ddl_col.SelectedValue).Trim() + ".jpeg")))
                                            //{
                                            //    PdfImage coesiImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + Convert.ToString(ddl_col.SelectedValue).Trim() + ".jpeg"));
                                            //    mypdfpage.Add(coesiImage2, 435, 415, 200);
                                            //}
                                            //else
                                            //{
                                            //    PdfImage coesiImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                            //    mypdfpage.Add(coesiImage2, 435, 415, 200);
                                            //}
                                            #endregion

                                            coltop = coltop + 30;
                                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, coltop, 500, 1000));
                                            mypdfpage.Add(newpdftabpage);

                                            double wordtotal = grandTotal;

                                            coltop = coltop + 200;
                                            //string wrdTotal = "Rupees : " + DecimalToWords((double)wordtotal);
                                            //PdfTextArea finalcont1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                            //                               new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, wrdTotal);
                                            //mypdfpage.Add(finalcont1);

                                            PdfTextArea finalAmtw = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, " Rupees " + DecimalToWords((decimal)grandTotal) + " Only ");
                                            mypdfpage.Add(finalAmtw);

                                            if (colHeight > 5)
                                                coltop = coltop + 350;
                                            else
                                                coltop = coltop + 225;
                                            PdfTextArea finalcont = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 0, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleRight, "Principal");
                                            mypdfpage.Add(finalcont);

                                            mypdfpage.SaveToDocument();
                                            #endregion

                                            #region RAY_TRY
                                            //                                            PdfTextArea clgOffText = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 25, 350, 20), ContentAlignment.MiddleCenter, collegename);
                                            //                                                mypdfpage.Add(clgOffText);

                                            //                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                            //                                                {
                                            //                                                    PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                            //                                                    mypdfpage.Add(LogoImage1, 500, 25, 400);
                                            //                                                    //  mypdfpage.Add(LogoImage1, 400, 30, 300);
                                            //                                                    //mypdfpage.Add(LogoImage1, 500, 40, 200);
                                            //                                                }

                                            //                                                //Line2                                        


                                            //                                                add1 += " " + add2;
                                            //                                                PdfTextArea addText = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 45, 350, 20), ContentAlignment.MiddleCenter, add1);
                                            //                                                mypdfpage.Add(addText);

                                            //                                                PdfTextArea addres3 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 65, 350, 20), ContentAlignment.MiddleCenter, add3);
                                            //                                                mypdfpage.Add(addres3);

                                            //                                                PdfTextArea univer = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 85, 350, 20), ContentAlignment.MiddleCenter, univ);
                                            //                                                mypdfpage.Add(univer);

                                            //                                                //   PdfTextArea addOffText = new PdfTextArea(FontHeader, Color.Black, new PdfArea(mydoc, 120, 55, 350, 20), ContentAlignment.MiddleCenter, add1);

                                            //                                                // PdfTextArea addOffText = new PdfTextArea(FontHeaderAf, Color.Black, new PdfArea(mydoc, 70, 95, 450, 20), ContentAlignment.BottomLeft, affliated);
                                            //                                                PdfTextArea addOffText = new PdfTextArea(FontHeaderAf, Color.Black, new PdfArea(mydoc, 70, 105, 450, 20), ContentAlignment.BottomLeft, affliated);
                                            //                                                mypdfpage.Add(addOffText);


                                            //                                            //}

                                            //                                            //PdfTextArea ptc = new PdfTextArea(Fontbold1, Color.Black, new PdfArea(mydoc, 0, coltop, 550, 40), System.Drawing.ContentAlignment.MiddleRight, "Date: " + DateTime.Now.ToString("dd/MM/yyyy"));
                                            //                                            coltop += 18;
                                            //                                            PdfTextArea ptc = new PdfTextArea(Fontbold1, Color.Black, new PdfArea(mydoc, 0, coltop, 550, 40), System.Drawing.ContentAlignment.MiddleRight, "Date: " + DateTime.Now.ToString("dd/MM/yyyy"));
                                            //                                            mypdfpage.Add(ptc);
                                            //                                            coltop = coltop + 50;
                                            //                                            PdfTextArea ptsadd = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                            //                                                                           new PdfArea(mydoc, 0, coltop, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, "CERTIFICATE");
                                            //                                            mypdfpage.Add(ptsadd);
                                            //                                            PdfLine certLine = new PdfLine(mydoc, new PointF((float)(mydoc.PageWidth / 2) - 60, coltop + 18), new PointF((float)(mydoc.PageWidth / 2) + 65, coltop + 18), Color.Black, 1);
                                            //                                            mypdfpage.Add(certLine);

                                            //                                            string tempStr = "     This is to certify that " + gender + ". " + studname + "  (Register No : " + regno + " ) " + "is studying in " + year + " " + degree + " " + "(" + dept + ")   during the " + " " + "academic year  " + finalyr + ".";//+ "\n" + "\n"

                                            //                                            coltop += 30;


                                            //                                            PdfTable pdftblContent = mydoc.NewTable(Fontbold1, 1, 1, 5);
                                            //                                            pdftblContent.VisibleHeaders = false;
                                            //                                            pdftblContent.SetBorders(Color.Black, 1, BorderType.None);
                                            //                                            pdftblContent.Cell(0, 0).SetContent(tempStr);
                                            //                                            pdftblContent.Cell(0, 0).SetCellPadding(5);
                                            //                                            pdftblContent.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);

                                            //                                            PdfTablePage pdftblPage = pdftblContent.CreateTablePage(new PdfArea(mydoc, 40, coltop, mydoc.PageWidth - 50, 200));
                                            //                                            mypdfpage.Add(pdftblPage);
                                            //                                            coltop += (int)pdftblPage.Area.Height;
                                            //                                            //   finalyr
                                            //                                            string strText = string.Empty;
                                            //                                            if (rbl_PayablePaid.SelectedIndex == 0)
                                            //                                                strText = "College semester fees is as follows:-";
                                            //                                            else
                                            //                                                strText = "College semester fees is paid as follows:-";
                                            //                                            coltop = coltop + 10;
                                            //                                            PdfTextArea ptnxtcont = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                            //                                                                           new PdfArea(mydoc, 50, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, strText);
                                            //                                            mypdfpage.Add(ptnxtcont);
                                            //                                            PdfLine certLines = new PdfLine(mydoc, new PointF((float)(mydoc.PageWidth / 2) - 247, coltop + 18), new PointF((float)(mydoc.PageWidth / 2) - 70, coltop + 18), Color.Black, 1);
                                            //                                            mypdfpage.Add(certLines);

                                            //                                            Hashtable semValues = new Hashtable();
                                            //                                            semValues.Add("0 Year", "");
                                            //                                            for (int row = 1; row < dsOpTbl.Rows.Count; row++)
                                            //                                            {
                                            //                                                string semValue = Convert.ToString(dsOpTbl.Rows[row][0]);
                                            //                                                string val = returnYearforSem(semValue.Split(' ')[0]) + " Year";
                                            //                                                if (!semValues.Contains(val))
                                            //                                                {
                                            //                                                    semValues.Add(val, semValue);
                                            //                                                }
                                            //                                                else
                                            //                                                {
                                            //                                                    semValues[val] = semValues[val] + "," + semValue;
                                            //                                                }
                                            //                                            }
                                            //                                            DataTable dtYrwise = new DataTable();
                                            //                                            for (int col = 0; col < dsOpTbl.Columns.Count; col++)
                                            //                                            {
                                            //                                                if (col == 0)
                                            //                                                    dtYrwise.Columns.Add("Year");
                                            //                                                else
                                            //                                                    dtYrwise.Columns.Add(dsOpTbl.Columns[col].ColumnName);
                                            //                                            }
                                            //                                            for (int row = 0; row < semValues.Count; row++)
                                            //                                            {
                                            //                                                DataRow dr = dtYrwise.NewRow();
                                            //                                                if (row == 0)
                                            //                                                {
                                            //                                                    for (int col = 0; col < dsOpTbl.Columns.Count; col++)
                                            //                                                    {
                                            //                                                        dr[col] = Convert.ToString(dsOpTbl.Rows[row][col]);
                                            //                                                    }
                                            //                                                }
                                            //                                                else
                                            //                                                {
                                            //                                                    for (int col = 0; col < dsOpTbl.Columns.Count; col++)
                                            //                                                    {
                                            //                                                        if (col == 0)
                                            //                                                        {
                                            //                                                            dr[col] = row + " Year";
                                            //                                                        }
                                            //                                                        else
                                            //                                                        {
                                            //                                                            double res = 0;
                                            //                                                            if (semValues.Contains(row + " Year"))
                                            //                                                            {
                                            //                                                                string[] sems = semValues[(row + " Year")].ToString().Split(',');

                                            //                                                                foreach (string semI in sems)
                                            //                                                                {
                                            //                                                                    double valueop = 0;
                                            //                                                                    dsOpTbl.DefaultView.RowFilter = "Semester ='" + semI + "'";
                                            //                                                                    DataView dv = dsOpTbl.DefaultView;
                                            //                                                                    if (dv.Count > 0)
                                            //                                                                    {
                                            //                                                                        double.TryParse(Convert.ToString(dv[0][col]), out valueop);
                                            //                                                                    }
                                            //                                                                    res += valueop;
                                            //                                                                }
                                            //                                                            }

                                            //                                                            dr[col] = res;
                                            //                                                        }
                                            //                                                    }
                                            //                                                }
                                            //                                                dtYrwise.Rows.Add(dr);
                                            //                                            }
                                            //                                            dsOpTbl.Clear();
                                            //                                            dsOpTbl = dtYrwise;
                                            //                                            int lastRow = dsOpTbl.Columns.Count + 1;
                                            //                                            if (dsValues.Tables.Count > 2)
                                            //                                            {
                                            //                                                lastRow += 1;
                                            //                                            }

                                            //                                            Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, dsOpTbl.Rows.Count, dsOpTbl.Columns.Count, 1);
                                            //                                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            //                                            table.VisibleHeaders = false;
                                            //                                            #region RAY1
                                            //                                            //int colHeight = 0;
                                            //                                            //for (int semVal = 0; semVal < dsOpTbl.Rows.Count; semVal++)
                                            //                                            //{
                                            //                                            //    colHeight++;

                                            //                                            //    for (int colval = 0; colval < dsOpTbl.Columns.Count; colval++)
                                            //                                            //    {
                                            //                                            //        if (semVal == 0)
                                            //                                            //        {
                                            //                                            //            table.Cell(semVal, colval).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            //                                            //            table.Cell(semVal, colval).SetCellPadding(5);
                                            //                                            //            table.Cell(semVal, colval).SetContent(dsOpTbl.Columns[colval].ColumnName);
                                            //                                            //        }
                                            //                                            //        else
                                            //                                            //        {
                                            //                                            //            table.Cell(semVal, colval).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            //                                            //            table.Cell(semVal, colval).SetCellPadding(5);
                                            //                                            //            table.Cell(semVal, colval).SetContent(Convert.ToString(dsOpTbl.Rows[semVal][colval]));
                                            //                                            //        }
                                            //                                            //    }
                                            //                                            //}

                                            //                                            //coltop = coltop + 30;
                                            //                                            //Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, coltop, 500, 1000));
                                            //                                            //mypdfpage.Add(newpdftabpage);
                                            //                                            //if (colHeight > 5)
                                            //                                            //    coltop = coltop + 350;
                                            //                                            //else
                                            //                                            //    coltop = coltop + 225;
                                            //#endregion

                                            //                                            lastRow -= 1;
                                            //                                            for (int semVal = 0; semVal < (dsOpTbl.Rows.Count); semVal++)
                                            //                                            {
                                            //                                                if (semVal == 0)
                                            //                                                    table.Columns[semVal].SetWidth(100);
                                            //                                                else
                                            //                                                    table.Columns[semVal].SetWidth(60);
                                            //                                                for (int colval = 0; colval < dsOpTbl.Columns.Count; colval++)
                                            //                                                {
                                            //                                                    if (semVal == 0)
                                            //                                                    {
                                            //                                                        table.Cell(colval, semVal).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            //                                                        table.Cell(colval, semVal).SetCellPadding(5);
                                            //                                                        if (colval == 0)
                                            //                                                            table.Cell(colval, semVal).SetContent("PARTICULARS");
                                            //                                                        else
                                            //                                                            table.Cell(colval, semVal).SetContent(dsOpTbl.Columns[colval].ColumnName.ToUpper());

                                            //                                                    }
                                            //                                                    else
                                            //                                                    {
                                            //                                                        table.Cell(colval, semVal).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            //                                                        table.Cell(colval, semVal).SetCellPadding(5);
                                            //                                                        table.Cell(colval, semVal).SetContent(Convert.ToString(dsOpTbl.Rows[semVal][colval]));
                                            //                                                        if (colval == (dsOpTbl.Columns.Count - 1))
                                            //                                                        {
                                            //                                                            double tot = 0;
                                            //                                                            double.TryParse(Convert.ToString(dsOpTbl.Rows[semVal][colval]), out tot);
                                            //                                                            grandTotal += tot;
                                            //                                                        }
                                            //                                                    }
                                            //                                                }
                                            //                                            }

                                            //                                            if (dsValues.Tables.Count > 2)
                                            //                                            {
                                            //                                                table.Cell((lastRow - 1), 0).SetContent("Date Of Payment");
                                            //                                                table.Cell((lastRow - 1), 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            //                                                table.Cell((lastRow - 1), 0).SetFont(Fontbold1);
                                            //                                                for (int semVal = 1; semVal < (dsOpTbl.Rows.Count); semVal++)
                                            //                                                {
                                            //                                                    string dtOfPaymt = string.Empty;
                                            //                                                    string yearVal = Convert.ToString(dsOpTbl.Rows[semVal][0]);
                                            //                                                    if (semValues.Contains(yearVal))
                                            //                                                    {
                                            //                                                        string[] sems = semValues[yearVal].ToString().Split(',');
                                            //                                                        StringBuilder sbDt = new StringBuilder();
                                            //                                                        foreach (string semI in sems)
                                            //                                                        {
                                            //                                                            string feecat = d2.GetFunction("select textcode from textvaltable where textval='" + semI + "' and Textcriteria='FEECA'").Trim();
                                            //                                                            dsValues.Tables[2].DefaultView.RowFilter = " Feecategory='" + feecat + "'";
                                            //                                                            DataView dv = dsValues.Tables[2].DefaultView;
                                            //                                                            if (dv.Count > 0)
                                            //                                                            {
                                            //                                                                for (int dt = 0; dt < dv.Count; dt++)
                                            //                                                                {
                                            //                                                                    sbDt.Append(", " + Convert.ToString(dv[dt][0]));
                                            //                                                                }
                                            //                                                            }
                                            //                                                        }
                                            //                                                        if (sbDt.Length > 0)
                                            //                                                            sbDt.Remove(0, 1);
                                            //                                                        dtOfPaymt += sbDt.ToString().Trim();
                                            //                                                    }
                                            //                                                    table.Cell((lastRow - 1), semVal).SetContent(dtOfPaymt);
                                            //                                                    // table.Cell((lastRow - 1), semVal).SetFont(Fontbold1);
                                            //                                                    table.Cell((lastRow - 1), semVal).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            //                                                }
                                            //                                            }

                                            //                                            table.Cell((lastRow), 0).SetContent("GRAND TOTAL");
                                            //                                            table.Cell((lastRow), 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            //                                            table.Cell((lastRow), 0).SetFont(Fontbold1);
                                            //                                            table.Cell((lastRow), 1).SetContent(grandTotal);
                                            //                                            table.Cell((lastRow), 1).SetFont(Fontbold1);
                                            //                                            table.Cell((lastRow), 1).SetContentAlignment(ContentAlignment.MiddleRight);
                                            //                                            table.Cell((lastRow), 1).ColSpan = (dsOpTbl.Rows.Count - 1);
                                            //                                            for (int col = 0; col < dsOpTbl.Rows.Count; col++)
                                            //                                            {
                                            //                                                table.Cell(0, col).SetFont(Fontbold1);
                                            //                                                table.Cell((dsOpTbl.Columns.Count - 1), col).SetFont(Fontbold1);
                                            //                                            }
                                            //                                            int startpos = 60;
                                            //                                            if (dsOpTbl.Rows.Count > 6)
                                            //                                            {
                                            //                                                startpos = 15;
                                            //                                            }
                                            //                                            coltop = coltop + 30;
                                            //                                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, startpos, coltop, ((dsOpTbl.Rows.Count + 1) * 65) + 30, 1000));
                                            //                                            mypdfpage.Add(newpdftabpage);


                                            //                                            coltop = coltop + (int)newpdftabpage.Area.Height + 5;

                                            //                                            PdfTextArea finalcont = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                            //                                                                           new PdfArea(mydoc, 0, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleRight, "Principal");
                                            //                                            mypdfpage.Add(finalcont);

                                            //                                            mypdfpage.SaveToDocument();
                                            #endregion
                                            #endregion
                                        }
                                        else
                                        {
                                            if (cbYearWise.Visible && cbYearWise.Checked)
                                            {
                                                double grandTotal = 0;
                                                #region for StudWise PDF Generation Format I

                                                stuflag = true;
                                                if (cbIncHeader.Checked)
                                                {
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                                    {
                                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                                        // mypdfpage.Add(LogoImage, 20, 30, 300);
                                                        mypdfpage.Add(LogoImage, 25, 25, 400);
                                                    }

                                                    //college address

                                                    PdfTextArea clgOffText = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 70, 25, 500, 20), ContentAlignment.MiddleCenter, collegename + " (AUTONOMOUS)");
                                                    mypdfpage.Add(clgOffText);

                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                                    {
                                                        PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                                        //mypdfpage.Add(LogoImage1, 500, 40, 200);
                                                        mypdfpage.Add(LogoImage1, 500, 25, 400);
                                                    }

                                                    //Line2                                        


                                                    add1 += " " + add2;
                                                    PdfTextArea addText = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 45, 350, 20), ContentAlignment.MiddleCenter, add1);
                                                    mypdfpage.Add(addText);

                                                    PdfTextArea addres3 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 65, 350, 20), ContentAlignment.MiddleCenter, add3);
                                                    mypdfpage.Add(addres3);

                                                    PdfTextArea univer = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 85, 350, 20), ContentAlignment.MiddleCenter, univ);
                                                    //mypdfpage.Add(univer);

                                                }
                                                coltop = coltop + 40;
                                                PdfTextArea ptsadd = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 0, coltop, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, "TO WHOMSOEVER IT MAY CONCERN");
                                                mypdfpage.Add(ptsadd);

                                                coltop = coltop + 40;
                                                PdfTextArea ptstart = null;
                                                string ApplValue = string.Empty;
                                                if (rbl_PayablePaid.SelectedIndex == 0)
                                                {
                                                    ApplValue += "approximate";
                                                }
                                                else if (rbl_PayablePaid.SelectedIndex == 1)
                                                {
                                                    ApplValue += "";
                                                }
                                                ptstart = new PdfTextArea(FontText, System.Drawing.Color.Black,
                                                                              new PdfArea(mydoc, 50, coltop, 500, 90), System.Drawing.ContentAlignment.TopLeft, "Following are the " + ApplValue + " fees " + rbl_PayablePaid.SelectedItem.Text.ToLower() + " by " + gender + " ." + studname + "  (Register No : " + regno + " ) " + year + " " + degree + " " + "(" + dept + ") student of our College for the period of " + finalyr + ".");


                                                mypdfpage.Add(ptstart);

                                                coltop = coltop + 30;

                                                #region Yearwise From semester
                                                Hashtable semValues = new Hashtable();
                                                semValues.Add("0 Year", "");
                                                for (int row = 1; row < dsOpTbl.Rows.Count; row++)
                                                {
                                                    string semValue = Convert.ToString(dsOpTbl.Rows[row][0]);
                                                    string val = returnYearforSem(semValue.Split(' ')[0]) + " Year";
                                                    if (!semValues.Contains(val))
                                                    {
                                                        semValues.Add(val, semValue);
                                                    }
                                                    else
                                                    {
                                                        semValues[val] = semValues[val] + "," + semValue;
                                                    }
                                                }
                                                DataTable dtYrwise = new DataTable();
                                                for (int col = 0; col < dsOpTbl.Columns.Count; col++)
                                                {
                                                    if (col == 0)
                                                        dtYrwise.Columns.Add("Year");
                                                    else
                                                        dtYrwise.Columns.Add(dsOpTbl.Columns[col].ColumnName);
                                                }
                                                for (int row = 0; row < semValues.Count; row++)
                                                {
                                                    DataRow dr = dtYrwise.NewRow();
                                                    if (row == 0)
                                                    {
                                                        for (int col = 0; col < dsOpTbl.Columns.Count; col++)
                                                        {
                                                            dr[col] = Convert.ToString(dsOpTbl.Rows[row][col]);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        for (int col = 0; col < dsOpTbl.Columns.Count; col++)
                                                        {
                                                            if (col == 0)
                                                            {
                                                                dr[col] = row + " Year";
                                                            }
                                                            else
                                                            {
                                                                double res = 0;
                                                                if (semValues.Contains(row + " Year"))
                                                                {
                                                                    string[] sems = semValues[(row + " Year")].ToString().Split(',');

                                                                    foreach (string semI in sems)
                                                                    {
                                                                        double valueop = 0;
                                                                        dsOpTbl.DefaultView.RowFilter = "Semester ='" + semI + "'";
                                                                        DataView dv = dsOpTbl.DefaultView;
                                                                        if (dv.Count > 0)
                                                                        {
                                                                            double.TryParse(Convert.ToString(dv[0][col]), out valueop);
                                                                        }
                                                                        res += valueop;
                                                                    }
                                                                }

                                                                dr[col] = res;
                                                            }
                                                        }
                                                    }
                                                    dtYrwise.Rows.Add(dr);
                                                }
                                                dsOpTbl.Clear();
                                                dsOpTbl = dtYrwise;
                                                #endregion
                                                int lastRow = dsOpTbl.Columns.Count + 1;
                                                if (dsValues.Tables.Count > 2)
                                                {
                                                    lastRow += 1;
                                                }
                                                //Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, (dsOpTbl.Columns.Count + 2), dsOpTbl.Rows.Count, 1);
                                                Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, (lastRow), dsOpTbl.Rows.Count, 1);
                                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table.VisibleHeaders = false;
                                                lastRow -= 1;
                                                for (int semVal = 0; semVal < (dsOpTbl.Rows.Count); semVal++)
                                                {
                                                    if (semVal == 0)
                                                        table.Columns[semVal].SetWidth(100);
                                                    else
                                                        table.Columns[semVal].SetWidth(60);
                                                    for (int colval = 0; colval < dsOpTbl.Columns.Count; colval++)
                                                    {
                                                        if (semVal == 0)
                                                        {
                                                            table.Cell(colval, semVal).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            table.Cell(colval, semVal).SetCellPadding(5);
                                                            if (colval == 0)
                                                                table.Cell(colval, semVal).SetContent("PARTICULARS");
                                                            else
                                                                table.Cell(colval, semVal).SetContent(dsOpTbl.Columns[colval].ColumnName.ToUpper());

                                                        }
                                                        else
                                                        {
                                                            table.Cell(colval, semVal).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            table.Cell(colval, semVal).SetCellPadding(5);
                                                            table.Cell(colval, semVal).SetContent(Convert.ToString(dsOpTbl.Rows[semVal][colval]));
                                                            if (colval == (dsOpTbl.Columns.Count - 1))
                                                            {
                                                                double tot = 0;
                                                                double.TryParse(Convert.ToString(dsOpTbl.Rows[semVal][colval]), out tot);
                                                                grandTotal += tot;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (dsValues.Tables.Count > 2)
                                                {
                                                    table.Cell((lastRow - 1), 0).SetContent("Date Of Payment");
                                                    table.Cell((lastRow - 1), 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table.Cell((lastRow - 1), 0).SetFont(Fontbold1);
                                                    for (int semVal = 1; semVal < (dsOpTbl.Rows.Count); semVal++)
                                                    {
                                                        string dtOfPaymt = string.Empty;
                                                        string yearVal = Convert.ToString(dsOpTbl.Rows[semVal][0]);
                                                        if (semValues.Contains(yearVal))
                                                        {
                                                            string[] sems = semValues[yearVal].ToString().Split(',');
                                                            StringBuilder sbDt = new StringBuilder();
                                                            foreach (string semI in sems)
                                                            {
                                                                string feecat = d2.GetFunction("select textcode from textvaltable where textval='" + semI + "' and Textcriteria='FEECA'").Trim();
                                                                dsValues.Tables[2].DefaultView.RowFilter = " Feecategory='" + feecat + "'";
                                                                DataView dv = dsValues.Tables[2].DefaultView;
                                                                if (dv.Count > 0)
                                                                {
                                                                    for (int dt = 0; dt < dv.Count; dt++)
                                                                    {
                                                                        sbDt.Append(", " + Convert.ToString(dv[dt][0]));
                                                                    }
                                                                }
                                                            }
                                                            if (sbDt.Length > 0)
                                                                sbDt.Remove(0, 1);
                                                            dtOfPaymt += sbDt.ToString().Trim();
                                                        }
                                                        table.Cell((lastRow - 1), semVal).SetContent(dtOfPaymt);
                                                        // table.Cell((lastRow - 1), semVal).SetFont(Fontbold1);
                                                        table.Cell((lastRow - 1), semVal).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    }
                                                }

                                                table.Cell((lastRow), 0).SetContent("GRAND TOTAL");
                                                table.Cell((lastRow), 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table.Cell((lastRow), 0).SetFont(Fontbold1);
                                                table.Cell((lastRow), 1).SetContent(grandTotal);
                                                table.Cell((lastRow), 1).SetFont(Fontbold1);
                                                table.Cell((lastRow), 1).SetContentAlignment(ContentAlignment.MiddleRight);
                                                table.Cell((lastRow), 1).ColSpan = (dsOpTbl.Rows.Count - 1);
                                                for (int col = 0; col < dsOpTbl.Rows.Count; col++)
                                                {
                                                    table.Cell(0, col).SetFont(Fontbold1);
                                                    table.Cell((dsOpTbl.Columns.Count - 1), col).SetFont(Fontbold1);
                                                }
                                                int startpos = 60;
                                                if (dsOpTbl.Rows.Count > 6)
                                                {
                                                    startpos = 15;
                                                }
                                                coltop = coltop + 30;
                                                Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, startpos, coltop, ((dsOpTbl.Rows.Count + 1) * 65) + 30, 1000));
                                                mypdfpage.Add(newpdftabpage);


                                                coltop = coltop + (int)newpdftabpage.Area.Height + 5;
                                                //PdfTextArea finalAmt = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                //                               new PdfArea(mydoc, 100, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL   " +grandTotal );
                                                //mypdfpage.Add(finalAmt);
                                                //coltop = coltop + 20;
                                                PdfTextArea finalAmtw = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 70, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "( Rupees " + DecimalToWords((decimal)grandTotal) + " Only )");
                                                mypdfpage.Add(finalAmtw);

                                                coltop = coltop + 50;
                                                PdfTextArea finalDate = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 100, coltop, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.Date.ToString("dd/MM/yyyy"));
                                                mypdfpage.Add(finalDate);
                                                PdfTextArea finalcont = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 0, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleRight, "MANAGER");
                                                mypdfpage.Add(finalcont);

                                                mypdfpage.SaveToDocument();

                                                #endregion
                                            }
                                            else //Mcc Format II
                                            {
                                                double grandTotal = 0;
                                                #region for StudWise PDF Generation Format II

                                                stuflag = true;
                                                if (cbIncHeader.Checked)
                                                {
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                                    {
                                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                                        // mypdfpage.Add(LogoImage, 20, 30, 300);
                                                        mypdfpage.Add(LogoImage, 25, 25, 400);

                                                    }

                                                    //college address

                                                    PdfTextArea clgOffText = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 70, 25, 500, 20), ContentAlignment.MiddleCenter, collegename + " (AUTONOMOUS)");
                                                    mypdfpage.Add(clgOffText);

                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                                    {
                                                        PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                                        // mypdfpage.Add(LogoImage1, 500, 40, 200);
                                                        mypdfpage.Add(LogoImage1, 500, 25, 400);
                                                    }

                                                    //Line2                                        


                                                    add1 += " " + add2;
                                                    PdfTextArea addText = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 45, 350, 20), ContentAlignment.MiddleCenter, add1);
                                                    mypdfpage.Add(addText);

                                                    PdfTextArea addres3 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 65, 350, 20), ContentAlignment.MiddleCenter, add3);
                                                    mypdfpage.Add(addres3);

                                                    PdfTextArea univer = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydoc, 120, 85, 350, 20), ContentAlignment.MiddleCenter, univ);
                                                    //mypdfpage.Add(univer);

                                                }
                                                //coltop = coltop + 40;
                                                coltop = coltop + 70;
                                                PdfTextArea ptsadd = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 0, coltop, 600, 30), System.Drawing.ContentAlignment.MiddleCenter, "TO WHOMSOEVER IT MAY CONCERN");
                                                mypdfpage.Add(ptsadd);

                                                coltop = coltop + 40;  // Mcc Paid
                                                string ApplValue = string.Empty;
                                                if (rbl_PayablePaid.SelectedIndex == 0)
                                                {
                                                    ApplValue += "approximate";
                                                }
                                                else if (rbl_PayablePaid.SelectedIndex == 1)
                                                {
                                                    ApplValue += "";
                                                }
                                                string StreamValue = string.Empty;
                                                if (stream.Trim() != "")
                                                {
                                                    StreamValue = "[" + Streamcheck + "]";
                                                }
                                                PdfTextArea ptstart = new PdfTextArea(FontText, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 50, coltop, 500, 90), System.Drawing.ContentAlignment.TopLeft, "Following are the " + ApplValue + " fees " + rbl_PayablePaid.SelectedItem.Text.ToLower() + " by " + gender + " ." + studname + "  (Register No : " + regno + " ) " + year + " " + degree + " " + "(" + dept + ") " + StreamValue + " student of our College during the academic year " + finalyr + ".");
                                                mypdfpage.Add(ptstart);

                                                coltop = coltop + 30;
                                                int lastRow = dsOpTbl.Columns.Count + 1;
                                                if (dsValues.Tables.Count > 2)
                                                {
                                                    lastRow += 1;
                                                }
                                                //Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, (dsOpTbl.Columns.Count + 2), dsOpTbl.Rows.Count, 1);
                                                Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, (lastRow), dsOpTbl.Rows.Count, 1);
                                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table.VisibleHeaders = false;
                                                lastRow -= 1;
                                                for (int semVal = 0; semVal < (dsOpTbl.Rows.Count); semVal++)
                                                {
                                                    if (semVal == 0)
                                                        table.Columns[semVal].SetWidth(90);
                                                    else
                                                        table.Columns[semVal].SetWidth(65);
                                                    for (int colval = 0; colval < dsOpTbl.Columns.Count; colval++)
                                                    {
                                                        if (semVal == 0)
                                                        {
                                                            table.Cell(colval, semVal).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            table.Cell(colval, semVal).SetCellPadding(5);
                                                            if (colval == 0)
                                                                table.Cell(colval, semVal).SetContent("PARTICULARS");
                                                            else
                                                            {
                                                                table.Cell(colval, semVal).SetContent(dsOpTbl.Columns[colval].ColumnName.ToUpper());
                                                                table.Cell((colval), semVal).SetFont(Fontbold1);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            table.Cell(colval, semVal).SetContentAlignment(ContentAlignment.MiddleRight);
                                                            table.Cell(colval, semVal).SetCellPadding(5);
                                                            table.Cell(colval, semVal).SetContent(Convert.ToString(dsOpTbl.Rows[semVal][colval]));
                                                            table.Cell((colval), semVal).SetFont(Fontbold1);
                                                            if (colval == (dsOpTbl.Columns.Count - 1))
                                                            {
                                                                double tot = 0;
                                                                double.TryParse(Convert.ToString(dsOpTbl.Rows[semVal][colval]), out tot);
                                                                grandTotal += tot;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (dsValues.Tables.Count > 2)
                                                {
                                                    table.Cell((lastRow - 1), 0).SetContent("Date Of Payment");
                                                    table.Cell((lastRow - 1), 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table.Cell((lastRow - 1), 0).SetFont(Fontbold1);
                                                    for (int semVal = 1; semVal < (dsOpTbl.Rows.Count); semVal++)
                                                    {
                                                        string feecat = d2.GetFunction("select textcode from textvaltable where textval='" + Convert.ToString(dsOpTbl.Rows[semVal][0]) + "' and Textcriteria='FEECA'").Trim();
                                                        dsValues.Tables[2].DefaultView.RowFilter = " Feecategory='" + feecat + "'";
                                                        DataView dv = dsValues.Tables[2].DefaultView;
                                                        if (dv.Count > 0)
                                                        {
                                                            string dtOfPaymt = string.Empty;

                                                            StringBuilder sbDt = new StringBuilder();
                                                            for (int dt = 0; dt < dv.Count; dt++)
                                                            {
                                                                sbDt.Append(", " + Convert.ToString(dv[dt][0]));
                                                            }
                                                            if (sbDt.Length > 0)
                                                                sbDt.Remove(0, 1);
                                                            dtOfPaymt = sbDt.ToString().Trim();

                                                            table.Cell((lastRow - 1), semVal).SetContent(dtOfPaymt);
                                                            table.Cell((lastRow - 1), semVal).SetFont(Fontbold1);
                                                            // table.Cell((lastRow - 1), semVal).SetFont(Fontbold1);
                                                            table.Cell((lastRow - 1), semVal).SetContentAlignment(ContentAlignment.MiddleRight);
                                                        }
                                                    }
                                                }

                                                table.Cell((lastRow), 0).SetContent("GRAND TOTAL");
                                                table.Cell((lastRow), 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table.Cell((lastRow), 0).SetFont(Fontbold1);
                                                table.Cell((lastRow), 1).SetContent(grandTotal);
                                                table.Cell((lastRow), 1).SetFont(Fontbold1);
                                                table.Cell((lastRow), 1).SetContentAlignment(ContentAlignment.MiddleRight);
                                                table.Cell((lastRow), 1).ColSpan = (dsOpTbl.Rows.Count - 1);
                                                for (int col = 0; col < dsOpTbl.Rows.Count; col++)
                                                {
                                                    table.Cell(0, col).SetFont(Fontbold1);
                                                    table.Cell((dsOpTbl.Columns.Count - 1), col).SetFont(Fontbold1);
                                                }
                                                int startpos = 60;
                                                if (dsOpTbl.Rows.Count > 6)
                                                {
                                                    startpos = 15;
                                                }
                                                int totalCnt = 0;
                                                int.TryParse(Convert.ToString(dsOpTbl.Rows.Count), out totalCnt);
                                                switch (totalCnt)
                                                {
                                                    case 1:
                                                        startpos = 190;
                                                        break;
                                                    case 2:
                                                        startpos = 180;
                                                        break;
                                                    case 3:
                                                        startpos = 140;
                                                        break;
                                                    case 4:
                                                        startpos = 120;
                                                        break;
                                                    case 5:
                                                        startpos = 90;
                                                        break;
                                                    case 6:
                                                        startpos = 50;
                                                        break;
                                                    case 7:
                                                        startpos = 25;
                                                        break;
                                                    default:
                                                        startpos = 60;
                                                        break;
                                                }

                                                coltop = coltop + 30;
                                                Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, startpos, coltop, ((dsOpTbl.Rows.Count + 1) * 65) + 30, 1000));
                                                mypdfpage.Add(newpdftabpage);


                                                coltop = coltop + (int)newpdftabpage.Area.Height + 5;
                                                //PdfTextArea finalAmt = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                //                               new PdfArea(mydoc, 100, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL   " +grandTotal );
                                                //mypdfpage.Add(finalAmt);
                                                //coltop = coltop + 20;
                                                PdfTextArea finalAmtw = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 110, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, " Rupees " + DecimalToWords((decimal)grandTotal) + " Only ");
                                                mypdfpage.Add(finalAmtw);

                                                coltop = coltop + 50;
                                                PdfTextArea finalDate = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 100, coltop, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.Date.ToString("dd/MM/yyyy"));
                                                mypdfpage.Add(finalDate);
                                                PdfTextArea finalcont = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 0, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleRight, "MANAGER-FINANCE & ACCOUNTS");
                                                mypdfpage.Add(finalcont);

                                                mypdfpage.SaveToDocument();

                                                #endregion
                                            }
                                        }

                                        if (stuflag == true)
                                        {
                                            string appPath = HttpContext.Current.Server.MapPath("~");
                                            if (appPath != "")
                                            {
                                                string szPath = appPath + "/Report/";
                                                string szFile = "Format1" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                                mydoc.SaveToFile(szPath + szFile);
                                                Response.ClearHeaders();
                                                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                                Response.ContentType = "application/pdf";
                                                Response.WriteFile(szPath + szFile);
                                            }
                                        }
                                    }
                                }
                            }


                            #endregion

                            #endregion
                        }
                    }
                }
                #endregion

                #region for DeptWise PDF
                if (rb_dept.Checked == true)
                {
                    for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                    {
                        byte check = Convert.ToByte(Fpspread1.Sheets[0].Cells[i, 1].Value);
                        if (check == 1)
                        {
                            #region for Deptwise
                            string semoryear = "";
                            Gios.Pdf.PdfPage deptpdfpage;
                            deptpdfpage = mypdf.NewPage();
                            // Font Fontboldd = new Font("Book Antiqua", 15, FontStyle.Bold);
                            //  Font FontMedium = new Font("Book Antiqua", 12, FontStyle.Regular);
                            coltop = 70;
                            string deptyear = "";
                            string deptsem = "";
                            string DeptName = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 3].Text);
                            string degree = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 2].Text);
                            string degcode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 2].Tag);
                            string batchyear = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 4].Text);
                            //  string feecat = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 5].Tag);
                            //  string feecat = feecatValue(semcode);
                            string seattype = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 6].Tag);
                            string seattet = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 6].Text);
                            #endregion

                            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + ddl_col.SelectedItem.Value + " ";

                            string collegename = "";
                            string add1 = "";
                            string add2 = "";
                            string add3 = "";
                            string univ = "";
                            ds = d2.select_method_wo_parameter(colquery, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                                // univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                            }

                            #region for DeptWise PDF Generation

                            stuflag = true;

                            string feecode = "";
                            string feedet = "";



                            #region old query commented by saranya on 18/04/2018
                            //if (rb_hdr.Checked == true)
                            //{
                            //    feedet = " select distinct f.HeaderFK,FeeCategory,sum (TotalAmount) as TotalAmount from FT_FeeAllotDegree f where degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "'  group by f.HeaderFK ,FeeCategory  order by f.HeaderFK ";
                            //    //   and FeeCategory ='" + feecat + "'
                            //    feedet = feedet + " select distinct FeeCategory,TextVal,f.SeatType  from FT_FeeAllotDegree f,TextValTable T where T.TextCode =F.FeeCategory and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "'  order by FeeCategory ";

                            //    feedet = feedet + " select distinct h.HeaderName as CollName,f.HeaderFK as CollValue from FT_FeeAllotDegree f,FM_HeaderMaster h where f.HeaderFK=h.HeaderPK  and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "'  order by f.HeaderFk";

                            //}
                            //if (rb_ldr.Checked == true)
                            //{
                            //    feedet = " select distinct f.LedgerFK,FeeCategory,sum (TotalAmount) as TotalAmount from FT_FeeAllotDegree f where degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "'  group by f.LedgerFK ,FeeCategory  order by f.LedgerFK ";
                            //    feedet = feedet + " select distinct FeeCategory,TextVal,f.SeatType  from FT_FeeAllotDegree f,TextValTable T where T.TextCode =F.FeeCategory and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "'  order by FeeCategory ";
                            //    feedet = feedet + " select distinct l.LedgerName as CollName,f.LedgerFK as CollValue from FT_FeeAllotDegree f,FM_LedgerMaster l where f.LedgerFK=l.LedgerPK  and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "'  order by f.LedgerFK";
                            //}

                            //if (rb_grphdr.Checked)
                            //{
                            //    string stream = string.Empty;
                            //    stream = d2.GetFunction("select type from course c,degree d where c.course_id=d.course_id and d.degree_code=" + degcode + "").Trim();

                            //    feedet = "   select fs.ChlGroupHeader,FeeCategory,sum (TotalAmount) as TotalAmount from FT_FeeAllotDegree f ,FS_ChlGroupHeaderSettings fs where fs.HeaderFK =f.HeaderFK and degreecode ='" + degcode + "' and BatchYear in ('" + batchyear + "') and seattype='" + seattype + "' and Stream ='" + stream + "'  group by fs.ChlGroupHeader ,FeeCategory  ";

                            //    feedet = feedet + "    select distinct FeeCategory,TextVal,f.SeatType  from FT_FeeAllotDegree f,TextValTable T where T.TextCode =F.FeeCategory and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "'  order by FeeCategory  ";

                            //    feedet = feedet + " select distinct h.ChlGroupHeader as CollName, h.ChlGroupHeader as CollValue from FT_FeeAllotDegree f,FS_ChlGroupHeaderSettings h where f.HeaderFK=h.HeaderfK and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "' and Stream ='" + stream + "'  ";
                            //}
                            #endregion

                            #region New query Added by saranya on 18/04/2018

                            int hdrCnt = 0;
                            StringBuilder chklstIds = new StringBuilder();
                            for (int chk = 0; chk < cbl_grp.Items.Count; chk++)
                            {
                                if (cbl_grp.Items[chk].Selected)
                                {
                                    if (chklstIds.Length == 0)
                                        chklstIds.Append(cbl_grp.Items[chk].Value);
                                    else
                                        chklstIds.Append("','" + cbl_grp.Items[chk].Value);
                                    hdrCnt++;
                                }
                            }

                            if (rb_ldr.Checked == true)
                            {
                                for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                                {
                                    if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                                    {
                                        for (int j = 0; j < treeledger.Nodes[remv].ChildNodes.Count; j++)
                                        {
                                            if (treeledger.Nodes[remv].ChildNodes[j].Checked == true)
                                            {
                                                if (chklstIds.Length == 0)
                                                    chklstIds.Append(treeledger.Nodes[remv].ChildNodes[j].Value);
                                                else
                                                    chklstIds.Append("','" + treeledger.Nodes[remv].ChildNodes[j].Value);
                                                hdrCnt++;
                                            }
                                        }
                                    }
                                }
                            }

                            if (rb_hdr.Checked == true)
                            {
                                feedet = " select distinct f.HeaderFK,FeeCategory,sum (TotalAmount) as TotalAmount from FT_FeeAllotDegree f,FM_HeaderMaster h where f.HeaderFK =h.HeaderPK and degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "'  and HeaderFK in ('" + chklstIds.ToString() + "') group by f.HeaderFK ,FeeCategory  order by f.HeaderFK ";
                                feedet = feedet + " select distinct FeeCategory,TextVal,f.SeatType  from FT_FeeAllotDegree f,TextValTable T,FM_HeaderMaster h where T.TextCode =F.FeeCategory and f.HeaderFK =h.HeaderPK and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "' and HeaderFK in ('" + chklstIds.ToString() + "') order by FeeCategory ";

                                feedet = feedet + " select distinct h.HeaderName as CollName,f.HeaderFK as CollValue from FT_FeeAllotDegree f,FM_HeaderMaster h where f.HeaderFK=h.HeaderPK  and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "' and HeaderFK in ('" + chklstIds.ToString() + "')  order by f.HeaderFk";
                            }
                            if (rb_ldr.Checked == true)
                            {
                                feedet = " select distinct  HeaderAcr+'-'+LedgerName as name,h.HeaderPK,f.LedgerFK,FeeCategory,sum (TotalAmount) as TotalAmount from FT_FeeAllotDegree f,FM_LedgerMaster l,FM_HeaderMaster h where f.LedgerFK  =l.LedgerPK and f.HeaderFK=h.HeaderPK and l.HeaderFK=h.HeaderPK and l.CollegeCode=h.CollegeCode and degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and f.LedgerFK in ('" + chklstIds.ToString() + "') group by f.LedgerFK ,FeeCategory,h.HeaderPK,HeaderAcr,LedgerName  order by f.LedgerFK ";

                                feedet = feedet + " select distinct FeeCategory,TextVal,f.SeatType  from FT_FeeAllotDegree f,TextValTable T,FM_LedgerMaster l where T.TextCode =F.FeeCategory and  degreecode ='" + degcode + "' and  f.LedgerFK=l.LedgerPK and BatchYear ='" + batchyear + "' and seattype='" + seattype + "' and f.LedgerFK in ('" + chklstIds.ToString() + "') order by FeeCategory ";

                                feedet = feedet + " select distinct HeaderAcr+'-'+LedgerName as CollName,f.LedgerFK as CollValue from FT_FeeAllotDegree f,FM_LedgerMaster l,FM_HeaderMaster h where f.LedgerFK  =l.LedgerPK and f.HeaderFK=h.HeaderPK and l.HeaderFK=h.HeaderPK and l.CollegeCode=h.CollegeCode  and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "' and f.LedgerFK in ('" + chklstIds.ToString() + "')  order by f.LedgerFK";

                            }
                            if (rb_grphdr.Checked)
                            {
                                string stream = string.Empty;
                                stream = d2.GetFunction("select type from course c,degree d where c.course_id=d.course_id and d.degree_code=" + degcode + "").Trim();

                                feedet = "   select fs.ChlGroupHeader,FeeCategory,sum (TotalAmount) as TotalAmount from FT_FeeAllotDegree f ,FS_ChlGroupHeaderSettings fs where fs.HeaderFK =f.HeaderFK and degreecode ='" + degcode + "' and BatchYear in ('" + batchyear + "') and fs.ChlGroupHeader in ('" + chklstIds.ToString() + "') and seattype='" + seattype + "' and Stream ='" + stream + "'  group by fs.ChlGroupHeader ,FeeCategory  ";

                                feedet = feedet + "    select distinct FeeCategory,TextVal,f.SeatType  from FT_FeeAllotDegree f,TextValTable T where T.TextCode =F.FeeCategory and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "'  order by FeeCategory  ";

                                feedet = feedet + " select distinct h.ChlGroupHeader as CollName, h.ChlGroupHeader as CollValue from FT_FeeAllotDegree f,FS_ChlGroupHeaderSettings h where f.HeaderFK=h.HeaderfK and  degreecode ='" + degcode + "' and BatchYear ='" + batchyear + "' and seattype='" + seattype + "' and Stream ='" + stream + "' and h.ChlGroupHeader in ('" + chklstIds.ToString() + "') ";
                            }
                            #endregion

                            ds.Clear();
                            DataTable dtnew = new DataTable();
                            dtnew.Clear();
                            dtnew.Rows.Clear();
                            dtnew.Columns.Clear();
                            DataRow dr;
                            dr = dtnew.NewRow();
                            DataColumn dcol = new DataColumn();
                            //dcol.ColumnName = "Course";
                            //dtnew.Columns.Add(dcol);
                            //dcol = new DataColumn();
                            //dcol.ColumnName = "Year";
                            //   dtnew.Columns.Add(dcol);
                            dcol = new DataColumn();
                            dcol.ColumnName = "FeeCatagory";
                            dtnew.Columns.Add(dcol);

                            string deptname = "";
                            string yearval = "";
                            DataView dvnew = new DataView();
                            DataTable Datafilter = new DataTable();

                            ds = d2.select_method_wo_parameter(feedet, "Text");
                            if (ds.Tables.Count > 0)
                            {
                                for (int ro = 0; ro < ds.Tables[2].Rows.Count; ro++)
                                {
                                    if (!dtnew.Columns.Contains(Convert.ToString(ds.Tables[2].Rows[ro]["CollName"])))
                                    {
                                        dtnew.Columns.Add(Convert.ToString(ds.Tables[2].Rows[ro]["CollName"]));
                                    }
                                }
                                dcol = new DataColumn();
                                dcol.ColumnName = "Total";
                                dtnew.Columns.Add(dcol);
                                for (int col = 0; col < ds.Tables[1].Rows.Count; col++)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = " FeeCategory ='" + Convert.ToString(ds.Tables[1].Rows[col]["FeeCategory"]) + "'";
                                    dvnew = ds.Tables[0].DefaultView;
                                    if (dvnew.Count > 0)
                                    {
                                        double hedamt = 0;
                                        double totamt = 0;
                                        double tothedamt = 0;
                                        dr = dtnew.NewRow();
                                        Datafilter = dvnew.ToTable();
                                        DataView dvnew1 = new DataView(Datafilter);
                                        for (int ro = 0; ro < ds.Tables[2].Rows.Count; ro++)
                                        {
                                            if (rb_hdr.Checked == true)
                                            {
                                                dvnew1.RowFilter = " FeeCategory='" + Convert.ToString(ds.Tables[1].Rows[col]["FeeCategory"]) + "' and HeaderFK='" + Convert.ToString(ds.Tables[2].Rows[ro]["CollValue"]) + "'";
                                            }
                                            if (rb_ldr.Checked == true)
                                            {
                                                dvnew1.RowFilter = " FeeCategory='" + Convert.ToString(ds.Tables[1].Rows[col]["FeeCategory"]) + "' and LedgerFK='" + Convert.ToString(ds.Tables[2].Rows[ro]["CollValue"]) + "'";
                                            }
                                            if (rb_grphdr.Checked == true)
                                            {
                                                dvnew1.RowFilter = " FeeCategory='" + Convert.ToString(ds.Tables[1].Rows[col]["FeeCategory"]) + "' and ChlGroupHeader='" + Convert.ToString(ds.Tables[2].Rows[ro]["CollValue"]) + "'";
                                            }
                                            if (dvnew1.Count > 0)
                                            {
                                                semoryear = Convert.ToString(ds.Tables[1].Rows[col]["TextVal"]);
                                                dr["FeeCatagory"] = semoryear;
                                                deptname = degree + "-" + DeptName;
                                                yearval = batchyear;
                                                double.TryParse(Convert.ToString(dvnew1[0]["TotalAmount"]), out hedamt);
                                                dr[ro + 1] = Convert.ToString(hedamt);
                                                tothedamt += hedamt;
                                                hedamt = 0;
                                            }
                                        }
                                        dr["Total"] = Convert.ToString(tothedamt);
                                        dtnew.Rows.Add(dr);
                                        tothedamt = 0;
                                    }
                                }

                                Gios.Pdf.PdfTable tabdept = mypdf.NewTable(Fontsmall, dtnew.Rows.Count + 1, dtnew.Columns.Count, 1);
                                tabdept.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                tabdept.VisibleHeaders = false;
                                if (rb_ldr.Checked == true)
                                {
                                    int[] deptwid = new int[] { 190, 75, 130, 100, 100, 100, 100, 100, 100 };
                                    //  int colwidth = (int)(500 / dtnew.Columns.Count);
                                    tabdept.CellRange(0, 0, 0, dtnew.Columns.Count - 1).SetFont(Fontbold1);
                                    for (int c = 0; c < dtnew.Columns.Count; c++)
                                    {
                                        //tabdept.Columns[c].SetWidth(deptwid[c]);
                                        tabdept.Cell(0, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tabdept.Cell(0, c).SetCellPadding(5);
                                        tabdept.Cell(0, c).SetContent(dtnew.Columns[c].ColumnName);
                                        if (c != 0)
                                            tabdept.Cell(0, c).SetFont(Fontbodybold);
                                    }
                                }
                                else
                                {
                                    //int[] deptwid = new int[] { 175, 75, 100, 100, 100, 100, 100, 100 };
                                    int colwidth = (int)(500 / dtnew.Columns.Count);
                                    tabdept.CellRange(0, 0, 0, dtnew.Columns.Count - 1).SetFont(Fontbold1);
                                    for (int c = 0; c < dtnew.Columns.Count; c++)
                                    {
                                        tabdept.Columns[c].SetWidth(colwidth);
                                        tabdept.Cell(0, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tabdept.Cell(0, c).SetCellPadding(5);
                                        tabdept.Cell(0, c).SetContent(dtnew.Columns[c].ColumnName);
                                        if (c != 0)
                                            tabdept.Cell(0, c).SetFont(Fontbodybold);
                                    }
                                }

                                for (int r = 1; r < dtnew.Rows.Count + 1; r++)
                                {
                                    int ms = 0;
                                    for (int c = 0; c < dtnew.Columns.Count; c++)
                                    {
                                        int dtObtmrk, min;
                                        bool dom = int.TryParse(dtnew.Rows[r - 1][c].ToString(), out dtObtmrk);
                                        if (c == 1)
                                            tabdept.Cell(r, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        else
                                            tabdept.Cell(r, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tabdept.Cell(r, c).SetContent(dtnew.Rows[r - 1][c].ToString());
                                        tabdept.Cell(r, c).SetFont(Fontsmall);
                                    }
                                }

                                //college address
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mypdf.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    //  deptpdfpage.Add(LogoImage, 20, 30, 300);
                                    deptpdfpage.Add(LogoImage, 25, 25, 400);

                                }


                                PdfTextArea clgOffText = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mypdf, 120, 25, 350, 20), ContentAlignment.MiddleCenter, collegename);
                                deptpdfpage.Add(clgOffText);

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {
                                    PdfImage LogoImage1 = mypdf.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    //deptpdfpage.Add(LogoImage1, 500, 40, 200);

                                    deptpdfpage.Add(LogoImage1, 500, 25, 400);
                                }

                                //Line2                                        


                                add1 += " " + add2;
                                PdfTextArea addText = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mypdf, 120, 45, 350, 20), ContentAlignment.MiddleCenter, add1);
                                deptpdfpage.Add(addText);

                                PdfTextArea addres3 = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mypdf, 120, 65, 350, 20), ContentAlignment.MiddleCenter, add3);
                                deptpdfpage.Add(addres3);

                                PdfTextArea univer = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mypdf, 120, 85, 350, 20), ContentAlignment.MiddleCenter, univ);
                                deptpdfpage.Add(univer);


                                PdfTextArea year = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mypdf, 190, 120, 200, 30), System.Drawing.ContentAlignment.MiddleRight, yearval + "-Fees Structure Report");
                                deptpdfpage.Add(year);

                                PdfTextArea course = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mypdf, 170, 140, 200, 30), System.Drawing.ContentAlignment.MiddleRight, deptname + "-" + seattet);
                                deptpdfpage.Add(course);

                                coltop = coltop + 100;
                                Gios.Pdf.PdfTablePage newpdfdept = tabdept.CreateTablePage(new Gios.Pdf.PdfArea(mypdf, 70, coltop, 500, 1000));
                                deptpdfpage.Add(newpdfdept);

                                coltop = coltop + 380;
                                PdfTextArea deptfin = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mypdf, 0, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleRight, "Principal");
                                deptpdfpage.Add(deptfin);
                                deptpdfpage.SaveToDocument();

                            #endregion
                            }
                            if (stuflag == true)
                            {
                                string appPath = HttpContext.Current.Server.MapPath("~");
                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "Format1" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                    mypdf.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
                #endregion

                if (stuflag == false)
                {
                    lbl_err.Visible = true;
                    lbl_err.Text = "Please Select Correct Values";
                }
            }

        }
        catch
        {

        }
    }

    private string DecimalToWords(double p)
    {
        throw new NotImplementedException();
    }

    protected string feecatValue(string value)
    {
        string semval = "";
        string type = "";
        try
        {
            string strtype = d2.GetFunction("select LinkValue from New_InsSettings where college_code='" + Session["collegecode"].ToString() + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
            if (strtype == "1")
                type = "Yearly";
            else
                type = "Semester";

            if (type == "Yearly")
            {
                if (value == "1" || value == "2")
                    semval = "1 Year";

                else if (value == "3" || value == "4")
                    semval = "2 Year";

                else if (value == "5" || value == "6")
                    semval = "3 Year";

                else if (value == "7" || value == "8")
                    semval = "4 Year";

            }
            else if (type == "Semester")
            {
                if (value == "1")
                    semval = "1 Semester";

                else if (value == "2")
                    semval = "2 Semester";

                else if (value == "3")
                    semval = "3 Semester";

                else if (value == "4")
                    semval = "4 Semester";

                else if (value == "5")
                    semval = "5 Semester";

                else if (value == "6")
                    semval = "6 Semester";

                else if (value == "7")

                    semval = "7 Semester";

                else if (value == "8")
                    semval = "8 Semester";

                else if (value == "9")
                    semval = "9 Semester";
            }

        }
        catch { }
        return semval;
    }
    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            switch (numeral)
            {
                case "1":
                    romanLettervalue = "I";
                    break;
                case "2":
                    romanLettervalue = "II";
                    break;
                case "3":
                    romanLettervalue = "III";
                    break;
                case "4":
                    romanLettervalue = "IV";
                    break;
                case "5":
                    romanLettervalue = "V";
                    break;
                case "6":
                    romanLettervalue = "VI";
                    break;
                case "7":
                    romanLettervalue = "VII";
                    break;
                case "8":
                    romanLettervalue = "VIII";
                    break;
                case "9":
                    romanLettervalue = "IX";
                    break;
                case "10":
                    romanLettervalue = "X";
                    break;
            }
        }
        return romanLettervalue;
    }
    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
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

        words = NumberToWords(intPortion);
        if (decPortion > 0)
        {
            words += " And ";
            words += NumberToWords(decPortion);
            words += " Paise ";
        }
        return words;
    }

    public string NumberToWords(int number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " Million ";
            number %= 1000000;
        }
        if ((number / 100000) > 0)
        {
            words += NumberToWords(number / 100000) + " Lakhs ";
            number %= 100000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

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

    public void bindcollege()
    {
        // ddl_col.Items.Clear();
        //reuse.bindCollegeToDropDown(usercode, ddl_col);
        string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
        ds.Clear();
        ddl_col.Items.Clear();
        string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_col.DataSource = ds;
            ddl_col.DataTextField = "collname";
            ddl_col.DataValueField = "college_code";
            ddl_col.DataBind();
        }

    }


    public void BindBatch()
    {
        try
        {
            string batch = "";
            cbl_batch.Items.Clear();
            // hat.Clear();
            ds = d2.BindBatch();

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
            }
            if (cbl_batch.Items.Count > 0)
            {
                for (int row = 0; row < cbl_batch.Items.Count; row++)
                {
                    cbl_batch.Items[row].Selected = true;
                    cb_batch.Checked = true;
                    batch = Convert.ToString(cbl_batch.Items[row].Text);
                }
                if (cbl_batch.Items.Count == 1)
                {
                    txt_batch.Text = "Batch(" + batch + ")";
                }
                else
                {
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                }
            }

            else
            {

                txt_batch.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void Bindcourse()
    {
        try
        {
            cbl_course.Items.Clear();
            string stream = "";
            if (cbl_stream.Items.Count > 0)
            {
                for (int i = 0; i < cbl_stream.Items.Count; i++)
                {
                    if (cbl_stream.Items[i].Selected == true)
                    {
                        if (stream.Trim() == "")
                        {
                            stream = Convert.ToString(cbl_stream.Items[i].Text);
                        }
                        else
                        {
                            stream = stream + "','" + Convert.ToString(cbl_stream.Items[i].Text);
                        }
                    }
                }
            }
            string deptquery = "";
            if (stream.Trim() != "")
            {
                deptquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + collegecode2 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and type in ('" + stream + "')";
            }
            else
            {
                deptquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + collegecode2 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "')";
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_course.DataSource = ds;
                cbl_course.DataTextField = "course_name";
                cbl_course.DataValueField = "course_id";
                cbl_course.DataBind();
                if (cbl_course.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_course.Items.Count; row++)
                    {
                        cbl_course.Items[row].Selected = true;
                    }
                    cb_course.Checked = true;
                    txt_course.Text = lbldeg.Text + "(" + cbl_course.Items.Count + ")";
                }

            }
            else
            {
                cb_course.Checked = false;
                txt_course.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void binddept()
    {
        try
        {

            cbl_dept.Items.Clear();
            string build2 = "";
            if (cbl_course.Items.Count > 0)
            {
                for (int i = 0; i < cbl_course.Items.Count; i++)
                {
                    if (cbl_course.Items[i].Selected == true)
                    {
                        if (build2 == "")
                        {
                            build2 = Convert.ToString(cbl_course.Items[i].Value);
                        }
                        else
                        {
                            // build2 = build2 + "'" + "," + "'" + Convert.ToString(cbl_course.Items[i].Value);
                            build2 += "," + Convert.ToString(cbl_course.Items[i].Value);
                        }
                    }
                }
            }
            if (build2 != "")
            {
                //string deptquery = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and  department .dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + build2 + "') and degree.college_code in ('" + collegecode2 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "')";
                //ds.Clear();
                //ds = d2.select_method_wo_parameter(deptquery, "Text");
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, build2, collegecode, usercode);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_dept.Items.Count; row++)
                        {
                            cbl_dept.Items[row].Selected = true;
                        }
                        cb_dept.Checked = true;
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
                    }

                }
            }
            else
            {
                cb_dept.Checked = false;
                txt_dept.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }

    protected void loadsem()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = d2.loadFeecategory(Convert.ToString(ddl_col.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "TextVal";
                cbl_sem.DataValueField = "TextCode";
                cbl_sem.DataBind();

                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                        cbltext = Convert.ToString(cbl_sem.Items[i].Text);
                    }
                    if (cbl_sem.Items.Count == 1)
                        txt_sem.Text = "" + linkName + "(" + cbltext + ")";
                    else
                        txt_sem.Text = "" + linkName + "(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }
    }

    //protected void loadsem()
    //{
    //    try
    //    {
    //        string sem = "";
    //        string clgvalue = collegecode2;
    //        string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //        DataSet dsset = new DataSet();
    //        dsset.Clear();
    //        dsset = d2.select_method_wo_parameter(semyear, "Text");
    //        if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
    //        {
    //            string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
    //            if (value == "1")
    //            {
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(SelectQ, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                    cbl_sem.DataSource = ds;
    //                    cbl_sem.DataTextField = "TextVal";
    //                    cbl_sem.DataValueField = "TextCode";
    //                    cbl_sem.DataBind();
    //                }
    //                if (cbl_sem.Items.Count > 0)
    //                {
    //                    for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                    {
    //                        cbl_sem.Items[i].Selected = true;
    //                        sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                    }
    //                    if (cbl_sem.Items.Count == 1)
    //                    {
    //                        txt_sem.Text = "SemesterandYear(" + sem + ")";
    //                    }
    //                    else
    //                    {
    //                        txt_sem.Text = "SemesterandYear(" + cbl_sem.Items.Count + ")";
    //                    }
    //                    cb_sem.Checked = true;
    //                }

    //            }
    //            else
    //            {
    //                cbl_sem.Items.Clear();
    //                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(settingquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //                    if (linkvalue == "0")
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            cbl_sem.DataSource = ds;
    //                            cbl_sem.DataTextField = "TextVal";
    //                            cbl_sem.DataValueField = "TextCode";
    //                            cbl_sem.DataBind();
    //                        }
    //                        if (cbl_sem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                            {
    //                                cbl_sem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                            }
    //                            if (cbl_sem.Items.Count == 1)
    //                            {
    //                                txt_sem.Text = "Semester(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
    //                            }
    //                            cb_sem.Checked = true;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            // text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            cbl_sem.DataSource = ds;
    //                            cbl_sem.DataTextField = "TextVal";
    //                            cbl_sem.DataValueField = "TextCode";
    //                            cbl_sem.DataBind();
    //                        }
    //                        if (cbl_sem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                            {
    //                                cbl_sem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                            }
    //                            if (cbl_sem.Items.Count == 1)
    //                            {
    //                                txt_sem.Text = "Year(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txt_sem.Text = "Year(" + cbl_sem.Items.Count + ")";
    //                            }
    //                            cb_sem.Checked = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}

    public void bindsec()
    {
        try
        {
            cbl_sect.Items.Clear();
            txt_sect.Text = "---Select---";
            cb_sect.Checked = false;
            string build = "";
            string sec = "";
            if (cbl_sem.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_sem.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_sem.Items[i].Value);
                        }
                    }
                }
            }
            if (build != "")
            {
                ds = d2.BindSectionDetailmult(collegecode2);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sect.DataSource = ds;
                    cbl_sect.DataTextField = "sections";
                    cbl_sect.DataValueField = "sections";
                    cbl_sect.DataBind();
                    if (cbl_sect.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sect.Items.Count; row++)
                        {
                            cbl_sect.Items[row].Selected = true;
                            sec = Convert.ToString(cbl_sect.Items[row].Text);
                        }
                        if (cbl_sect.Items.Count == 1)
                        {
                            txt_sect.Text = "Section(" + sec + ")";
                        }
                        else
                        {
                            txt_sect.Text = "Section(" + cbl_sect.Items.Count + ")";
                        }
                        cb_sect.Checked = true;
                    }
                }
            }
            else
            {
                cb_sect.Checked = false;
                txt_sect.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }

    public void bindgrouphdr()
    {
        cbl_grp.Items.Clear();
        string query = " SELECT distinct G.ChlGroupHeader FROM FS_ChlGroupHeaderSettings G,FS_HeaderPrivilage P WHERE G.HeaderFK = P.HeaderFK AND P. UserCode = '" + usercode + "'  AND P.CollegeCode = " + collegecode2 + " ";

        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_grp.DataSource = ds;
            cbl_grp.DataTextField = "ChlGroupHeader";
            cbl_grp.DataValueField = "ChlGroupHeader";
            cbl_grp.DataBind();
            for (int i = 0; i < cbl_grp.Items.Count; i++)
            {
                cbl_grp.Items[i].Selected = true;
            }
            cb_grp.Checked = true;
            txt_grp.Text = "Group Header(" + (cbl_grp.Items.Count.ToString()) + ")";
        }
        else
        {
            cb_grp.Checked = false;
            txt_grp.Text = "--Select--";
        }
    }

    public void bindheader()
    {
        cbl_grp.Items.Clear();
        string query = " SELECT  HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P.UserCode = " + usercode + " AND H.CollegeCode = " + collegecode2 + "  order by len(isnull(hd_priority,10000)),hd_priority asc";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_grp.DataSource = ds;
            cbl_grp.DataTextField = "HeaderName";
            cbl_grp.DataValueField = "HeaderPK";
            cbl_grp.DataBind();
            for (int i = 0; i < cbl_grp.Items.Count; i++)
            {
                cbl_grp.Items[i].Selected = true;
            }
            cb_grp.Checked = true;
            txt_grp.Text = "Header(" + (cbl_grp.Items.Count.ToString()) + ")";
        }
        else
        {
            cb_grp.Checked = false;
            txt_grp.Text = "--Select--";
        }
    }

    public void bindledger()
    {
        //cbl_grp.Items.Clear();
        //string query = "  SELECT  LedgerPK,LedgerName,L.Priority FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode  and l.LedgerMode=0 AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode2 + "  order by len(isnull(l.priority,1000)) , l.priority asc  ";
        ////order by case when priority is null then 1 else 0 end, priority
        //ds.Clear();
        //ds = d2.select_method_wo_parameter(query, "Text");
        ////TreeNode node = new TreeNode(ds.Tables[0].Rows[i]["header_name"].ToString(), ds.Tables[0].Rows[i]["header_id"].ToString());
        string straccheadquery = "SELECT HeaderPK as header_id,HeaderName as header_name,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = '" + usercode + "' AND H.CollegeCode = '" + collegecode2 + "' order by len(isnull(hd_priority,10000)),hd_priority asc ";
        ds = d2.select_method_wo_parameter(straccheadquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                TreeNode node = new TreeNode(ds.Tables[0].Rows[i]["header_name"].ToString(), ds.Tables[0].Rows[i]["header_id"].ToString());
                string strled = "SELECT LedgerPK as fee_code,LedgerName as fee_type FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode   AND P. UserCode = '" + Session["usercode"].ToString() + "' AND L.CollegeCode = '" + collegecode2 + "'  and L.HeaderFK in('" + ds.Tables[0].Rows[i]["header_id"].ToString() + "')and LedgerMode='0'  order by isnull(l.priority,1000), l.ledgerName asc ";
                DataSet ds1 = d2.select_method_wo_parameter(strled, "Text");
                for (int ledge = 0; ledge < ds1.Tables[0].Rows.Count; ledge++)
                {
                    TreeNode subchildnode = new TreeNode(ds1.Tables[0].Rows[ledge]["fee_type"].ToString(), ds1.Tables[0].Rows[ledge]["fee_code"].ToString());
                    subchildnode.ShowCheckBox = true;
                    node.ChildNodes.Add(subchildnode);
                }
                node.ShowCheckBox = true;
                treeledger.Nodes.Add(node);
                //if (hedgId == "")
                //    hedgId = Convert.ToString(ds.Tables[0].Rows[i]["header_id"]);
                //else
                //    hedgId = hedgId + "','" + Convert.ToString(ds.Tables[0].Rows[i]["header_id"]);
                //if (cb_grp.Checked == true)
                //{
                //    for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                //    {
                //        treeledger.Nodes[remv].Checked = true;
                //        txt_grp.Text = "Header(" + (treeledger.Nodes.Count) + ")";
                //        if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                //        {
                //            for (int child = 0; child < treeledger.Nodes[remv].ChildNodes.Count; child++)
                //                treeledger.Nodes[remv].ChildNodes[child].Checked = true;
                //        }
                //    }
                //}
                txt_grp.Text = "Ledger(" + (cbl_grp.Items.Count.ToString()) + ")";
            }
        }






        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_grp.DataSource = ds;
            cbl_grp.DataTextField = "LedgerName";
            cbl_grp.DataValueField = "LedgerPK";
            cbl_grp.DataBind();
            for (int i = 0; i < cbl_grp.Items.Count; i++)
            {
                cbl_grp.Items[i].Selected = true;
            }
            cb_grp.Checked = true;
            txt_grp.Text = "Ledger(" + (cbl_grp.Items.Count.ToString()) + ")";
        }
        else
        {
            cb_grp.Checked = false;
            txt_grp.Text = "--Select--";
        }
    }

    public void loadstream()
    {
        try
        {
            string stream = "";
            cbl_stream.Items.Clear();
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + collegecode2 + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_stream.DataSource = ds;
                cbl_stream.DataTextField = "type";
                cbl_stream.DataBind();

                if (cbl_stream.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stream.Items.Count; i++)
                    {
                        cbl_stream.Items[i].Selected = true;
                        stream = Convert.ToString(cbl_stream.Items[i].Text);
                    }
                    txt_stream.Text = "Type(" + cbl_stream.Items.Count + ")";
                    cb_stream.Checked = true;
                    if (streamEnabled() == 1)
                        txt_stream.Enabled = true;
                    else
                        txt_stream.Enabled = false;
                }
            }
            else
            {
                txt_stream.Text = "--Select--";
                txt_stream.Enabled = false;
            }
        }
        catch
        {
        }

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
        lbl.Add(lbl_stream);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lbl_sem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

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

    protected void spreadColumnVisible()
    {
        try
        {
            if (roll == 0)
            {
                Fpspread1.Columns[2].Visible = true;
                Fpspread1.Columns[3].Visible = true;
                Fpspread1.Columns[4].Visible = true;
            }
            else if (roll == 1)
            {
                Fpspread1.Columns[2].Visible = true;
                Fpspread1.Columns[3].Visible = true;
                Fpspread1.Columns[4].Visible = true;
            }
            else if (roll == 2)
            {
                Fpspread1.Columns[2].Visible = true;
                Fpspread1.Columns[3].Visible = false;
                Fpspread1.Columns[4].Visible = false;

            }
            else if (roll == 3)
            {
                Fpspread1.Columns[2].Visible = false;
                Fpspread1.Columns[3].Visible = true;
                Fpspread1.Columns[4].Visible = false;
            }
            else if (roll == 4)
            {
                Fpspread1.Columns[2].Visible = false;
                Fpspread1.Columns[3].Visible = false;
                Fpspread1.Columns[4].Visible = true;
            }
            else if (roll == 5)
            {
                Fpspread1.Columns[2].Visible = true;
                Fpspread1.Columns[3].Visible = true;
                Fpspread1.Columns[4].Visible = false;
            }
            else if (roll == 6)
            {
                Fpspread1.Columns[2].Visible = false;
                Fpspread1.Columns[3].Visible = true;
                Fpspread1.Columns[4].Visible = true;
            }
            else if (roll == 7)
            {
                Fpspread1.Columns[2].Visible = true;
                Fpspread1.Columns[3].Visible = false;
                Fpspread1.Columns[4].Visible = true;
            }
        }
        catch { }
    }

    #endregion

    private double streamEnabled()
    {
        double strValue = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='JournalEnableStreamShift' and user_code ='" + usercode + "' and college_code ='" + collegecode2 + "'")), out strValue);
        return strValue;
    }


    protected void rb_stud_Changed(object sender, EventArgs e)
    {
        cbdeptcumul.Visible = false;
        cbdeptcumul.Checked = false;
        ddlFormat_Selected(sender, e);
        cbdeptcumul_changed(sender, e);
    }
    protected void rb_dept_Changed(object sender, EventArgs e)
    {
        cbdeptcumul.Visible = true;
        cbdeptcumul.Checked = false;
        ddlFormat_Selected(sender, e);
        cbdeptcumul_changed(sender, e);
    }

    protected void ddlFormat_Selected(object sender, EventArgs e)
    {

        if (rb_stud.Checked)
        {
            if (ddlFormat.SelectedIndex == 0)
            {
                cbIncHeader.Visible = true;
                cbIncHeader.Checked = false;
                //   cbIncHeader.Visible = false;
            }
            else
            {
                cbIncHeader.Visible = true;
                cbIncHeader.Checked = false;
            }
        }
        else
            cbIncHeader.Visible = false;
    }

    protected void cbdeptcumul_changed(object sender, EventArgs e)
    {
        if (cbdeptcumul.Checked)
            btnprint.Enabled = false;
        else
            btnprint.Enabled = true;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Fee_Structure Report";
            string pagename = "Fee_Structure.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
            lblvalidation1.Visible = false;
        }
        catch
        {

        }
    }
    //added by abarna 23.12.2017
    protected void ddladmit_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Error.Visible = false;
        //FpSpread1.Visible = false;
        //btnprintmaster.Visible = false;
        txtno.Text = "";
        lblnum.Text = ddladmit.SelectedItem.ToString();

        switch (Convert.ToUInt32(ddladmit.SelectedItem.Value))
        {
            case 0:
                txtno.Attributes.Add("placeholder", "Roll No");
                chosedmode = 0;
                break;
            case 1:
                txtno.Attributes.Add("placeholder", "Reg No");
                chosedmode = 1;
                break;
            case 2:
                txtno.Attributes.Add("placeholder", "Admin No");
                chosedmode = 2;
                break;
            case 3:
                txtno.Attributes.Add("placeholder", "App No");
                chosedmode = 3;
                break;
            case 4:
                txtno.Attributes.Add("placeholder", "Name");
                chosedmode = 4;
                break;
        }


    }
    public void LoadFromSettings()
    {
        try
        {
            System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("Roll No", "0");
            System.Web.UI.WebControls.ListItem lst2 = new System.Web.UI.WebControls.ListItem("Reg No", "1");
            System.Web.UI.WebControls.ListItem lst3 = new System.Web.UI.WebControls.ListItem("Admission No", "2");
            System.Web.UI.WebControls.ListItem lst4 = new System.Web.UI.WebControls.ListItem("App No", "3");
            System.Web.UI.WebControls.ListItem lst5 = new System.Web.UI.WebControls.ListItem("Name", "4");

            //Roll Number or Reg Number or Admission No or Application Number
            ddladmit.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                ddladmit.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                ddladmit.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                ddladmit.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //App Form Number - Application Number
                ddladmit.Items.Add(lst4);

            }
            if (ddladmit.Items.Count == 0)
            {
                ddladmit.Items.Add(lst1);
            }
            ddladmit.Items.Add(lst5);
            switch (Convert.ToUInt32(ddladmit.SelectedItem.Value))
            {
                case 0:
                    txtno.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txtno.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txtno.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txtno.Attributes.Add("placeholder", "App No");
                    chosedmode = 3;
                    break;
                case 4:
                    txtno.Attributes.Add("placeholder", "");
                    chosedmode = 4;
                    break;
            }
        }
        catch { }
    }
    protected void btn_roll_Click(object sender, EventArgs e)
    {


        txtno.Text = "";
        popwindow.Visible = true;
        bindType();
        bindbatch1();
        binddegree2();
        bindbranch1();
        bindsec2();
        Fpspread2.Visible = false;
        btn_studOK.Visible = false;
        btn_exitstud.Visible = false;


        lbldisp.Text = "";
        lbldisp.Visible = false;






    }
    #region roll no Lookup


    public void bindType()
    {
        try
        {
            cbl_strm.Items.Clear();
            cb_strm.Checked = false;
            txt_strm.Text = "--Select--";
            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>'' order by type asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_strm.DataSource = ds;
                cbl_strm.DataTextField = "type";
                cbl_strm.DataValueField = "type";
                cbl_strm.DataBind();
                if (cbl_strm.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_strm.Items.Count; i++)
                    {
                        cbl_strm.Items[i].Selected = true;
                    }
                    txt_strm.Text = "Stream(" + cbl_strm.Items.Count + ")";
                    cb_strm.Checked = true;
                }
                txt_strm.Enabled = true;
            }
            else
            {
                txt_strm.Enabled = false;
            }
        }
        catch
        {
        }
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
        catch
        {
        }
    }
    public void binddegree2()
    {
        try
        {
            ds.Clear();
            cbl_degree2.Items.Clear();
            string stream = "";
            if (cbl_strm.Items.Count > 0)
            {
                for (int i = 0; i < cbl_strm.Items.Count; i++)
                {
                    if (cbl_strm.Items[i].Selected == true)
                    {
                        if (stream == "")
                        {
                            stream = Convert.ToString(cbl_strm.Items[i].Value);
                        }
                        else
                        {
                            stream = stream + "'" + "," + "'" + Convert.ToString(cbl_strm.Items[i].Value);
                        }
                    }
                }
            }
            txt_degree2.Text = "--Select--";

            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            string colleges = Convert.ToString(d2.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode1;
            }
            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegecode1 + ") and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + " ";
            if (txt_strm.Enabled)
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
                    txt_degree2.Text = lbl_degree2.Text + "(" + cbl_degree2.Items.Count + ")";
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
        catch { }
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
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
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
                        txt_branch2.Text = lbl_branch2.Text + "(" + cbl_branch1.Items.Count + ")";
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
        catch (Exception ex)
        {
        }
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
        catch { }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "";
            string batch_year = Convert.ToString(ddl_batch1.SelectedItem.Text);
            string stream = Convert.ToString(getCblSelectedValue(cbl_strm));
            string degree = Convert.ToString(getCblSelectedValue(cbl_degree2));
            string branch = Convert.ToString(getCblSelectedValue(cbl_branch1));
            string sec = Convert.ToString(getCblSelectedValue(cbl_sec2));
            string PaidFilter = string.Empty;



            if (txt_rollno3.Text != "")
            {
                selectquery = "select Roll_No,Roll_Admit,smart_serial_no,Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and r.roll_no ='" + txt_rollno3.Text + "'";


            }
            else
            {

                selectquery = "select Roll_No,Roll_Admit,smart_serial_no,Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and Batch_Year =" + batch_year + " and r.degree_code in ('" + branch + "')  and isnull(r.Sections,'') in ('" + sec + "') ";

            }

            // selectquery = "select Roll_No,Roll_Admit,smart_serial_no,Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' ";         

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region design
                Fpspread2.Visible = true;
                Fpspread2.Sheets[0].RowCount = 1;
                Fpspread2.Sheets[0].ColumnCount = 0;
                Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread2.CommandBar.Visible = false;
                Fpspread2.Sheets[0].ColumnCount = 6;
                Fpspread2.Sheets[0].RowHeader.Visible = false;


                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].Columns[0].Locked = true;
                Fpspread2.Columns[0].Width = 50;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[1].Width = 80;
                Fpspread2.Sheets[0].Columns[1].Locked = false;
                Fpspread2.Sheets[0].Columns[1].Visible = false;
                Fpspread2.Sheets[0].Cells[0, 1].CellType = chkall;
                Fpspread2.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[2].Locked = true;
                Fpspread2.Columns[2].Width = 100;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[3].Locked = true;
                Fpspread2.Columns[4].Width = 100;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[4].Locked = true;
                Fpspread2.Columns[4].Width = 200;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = lbl_degree2.Text;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[5].Locked = true;
                Fpspread2.Columns[5].Width = 308;


                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtSmartno = new FarPoint.Web.Spread.TextCellType();


                Fpspread2.Sheets[0].Columns[1].Locked = false;
                Fpspread2.Sheets[0].Columns[1].Visible = true;
                Fpspread2.Sheets[0].AutoPostBack = false;
                Fpspread2.Height = 250;

                Fpspread2.Sheets[0].Columns[1].Locked = true;
                Fpspread2.Sheets[0].Columns[1].Visible = false;
                Fpspread2.Sheets[0].AutoPostBack = true;

                #endregion

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    #region values

                    Fpspread2.Sheets[0].RowCount++;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    //
                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = false;
                    Fpspread2.Sheets[0].AutoPostBack = false;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = check;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].Columns[1].Visible = true;
                    Fpspread2.Sheets[0].Columns[1].Locked = false;


                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = txtRollno;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].CellType = txtRegno;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    #endregion
                }
                Fpspread2.Visible = true;
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                Fpspread2.Sheets[0].FrozenRowCount = 1;
                //  Fpspread2.Height = 400;

                Fpspread2.SaveChanges();
                btn_studOK.Visible = true;
                btn_exitstud.Visible = true;
                lbldisp.Visible = false;
            }
            else
            {
                Fpspread2.Visible = false;
                lbl_errormsg.Visible = true;
                lbl_errormsg.Text = "No Records Found";
                btn_studOK.Visible = false;
                btn_exitstud.Visible = false;
                lbldisp.Visible = false;
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void Fpspread2staff_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //Fpspread2.Visible = true;
        try
        {
            string actrow = Fpspread2.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread2.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread2.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanReceipt"); }
    }



    public void btn_studOK_Click(object sender, EventArgs e)
    {
        try
        {
            string rollno = "";
            string activerow = "";
            string activecol = "";
            string rollval = "";
            int cnT = 0;
            if (Fpspread2.Sheets[0].RowCount != 0)
            {
                activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
                if (rbstudtype.SelectedItem.Value == "1")
                {

                    if (activerow != Convert.ToString(-1))
                    {
                        rollno = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                        txtno.Text = Convert.ToString(rollno);
                        popwindow.Visible = false;
                    }
                }
                else
                {
                    lblrolldisp.Text = "";
                    lbldisp.Text = "";
                    Fpspread2.SaveChanges();
                    for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                    {
                        int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[i, 1].Value);
                        if (checkval == 1)
                        {
                            cnT++;
                            if (rollval == "")
                                rollval = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 2].Tag);
                            else
                                rollval = rollval + "," + Convert.ToString(Fpspread2.Sheets[0].Cells[i, 2].Tag);
                        }

                        lblrolldisp.Text = rollval;
                        lbldisp.Text = Convert.ToString("You Have Selected " + cnT + " Students");
                        lbldisp.Visible = true;
                        popwindow.Visible = false;
                    }
                }

            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_exitstud_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }

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
    protected void cb_strm_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_strm, cb_strm, txt_strm, lbl_stream.Text);
        binddegree2();
        bindbranch1();
        bindsec2();
    }
    protected void cbl_strm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_strm, cb_strm, txt_strm, lbl_stream.Text);
        binddegree2();
        bindbranch1();
        bindsec2();
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
    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread2.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread2.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread2.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    #endregion
    protected void rbstudtype_Selected(object sender, EventArgs e)
    {
        if (rbstudtype.SelectedItem.Value == "1")
        {
            txtno.Enabled = true;
            ddladmit.Enabled = true;
            lbldisp.Visible = false;
            lbldisp.Text = "";
            txtno.Text = "";

        }
        else
        {

            txtno.Enabled = false;
            ddladmit.Enabled = false;
            lbldisp.Visible = false;
            lbldisp.Text = "";
            txtno.Text = "";
            //  div1.Visible = false;
        }
    }
    protected void feeCatVal(string curSem, ArrayList arFeecat)
    {
        try
        {
            string type = "";
            string feecatg = "";
            Dictionary<string, string> htsem = new Dictionary<string, string>();
            string strtype = da.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
            if (strtype == "0")
            {
                type = "Semester";
            }
            else if (strtype == "1")
            {
                type = "Yearly";
            }
            else if (strtype == "2")
            {
                type = "Term";
            }
            string cblvalue = "";
            string cbltext = "";

            #region with current semester
            for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
            {
                if (cbl_sem.Items[sem].Selected == true)
                {
                    cblvalue = Convert.ToString(cbl_sem.Items[sem].Value);
                    cbltext = Convert.ToString(cbl_sem.Items[sem].Text);
                    if (arFeecat.Contains(cblvalue))//if current semester setting available only
                    {
                        if (type == "Semester")
                        {
                            string[] feesem = cbl_sem.Items[sem].Text.Split(' ');
                            if (feesem[0] == "1" || feesem[0] == "2")
                            {
                                if (!htsem.ContainsKey("1 Year"))
                                    htsem.Add(Convert.ToString("1 Year"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["1 Year"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("1 Year");
                                    htsem.Add(Convert.ToString("1 Year"), feecatg);
                                }
                            }
                            else if (feesem[0] == "3" || feesem[0] == "4")
                            {
                                if (!htsem.ContainsKey("2 Year"))
                                    htsem.Add(Convert.ToString("2 Year"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["2 Year"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("2 Year");
                                    htsem.Add(Convert.ToString("2 Year"), feecatg);
                                }
                            }
                            else if (feesem[0] == "5" || feesem[0] == "6")
                            {
                                if (!htsem.ContainsKey("3 Year"))
                                    htsem.Add(Convert.ToString("3 Year"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["3 Year"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("3 Year");
                                    htsem.Add(Convert.ToString("3 Year"), feecatg);
                                }
                            }
                            else if (feesem[0] == "7" || feesem[0] == "8")
                            {
                                if (!htsem.ContainsKey("4 Year"))
                                    htsem.Add(Convert.ToString("4 Year"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["4 Year"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("4 Year");
                                    htsem.Add(Convert.ToString("4 Year"), feecatg);
                                };
                            }
                        }
                        else
                        {
                            if (!htsem.ContainsKey(cbltext))
                                htsem.Add(Convert.ToString(cbltext), Convert.ToString(cblvalue));
                            else
                            {
                                feecatg = Convert.ToString(htsem[cbltext]);
                                feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                htsem.Remove(cbltext);
                                htsem.Add(Convert.ToString(cbltext), feecatg);
                            }
                        }
                    }
                }
            }
            #endregion

            ViewState["feecat"] = htsem;

        }
        catch { }
    }
    protected string getCurrentSemester(string batch, string degree, ref ArrayList arFeecat, string commondist, ref Dictionary<string, string> CurSemDeg)
    {
        string curSem = string.Empty;
        try
        {

            string type = string.Empty;
            string strtype = da.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
            if (strtype == "0")
            {
                type = "Semester";
            }
            else if (strtype == "1")
            {
                type = "Yearly";
            }
            else if (strtype == "2")
            {
                type = "Term";
            }
            StringBuilder sbCurSem = new StringBuilder();
            string selQ = " select distinct current_semester,(cast(batch_year as nvarchar(10))+'-'+cast(degree_code as nvarchar(10))+'-'+cast(r.current_semester as nvarchar(10))) as batch,degree_code,batch_year from registration r where batch_year in(" + batch + ") and degree_code in(" + degree + ")";
            if (!string.IsNullOrEmpty(commondist))
                selQ += commondist;
            selQ += " order by degree_code,batch_year desc";
            DataSet dsVal = da.select_method_wo_parameter(selQ, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    string batDeg = Convert.ToString(dsVal.Tables[0].Rows[row]["batch"]);
                    sbCurSem.Append(Convert.ToString(dsVal.Tables[0].Rows[row]["current_semester"]) + "','");
                    string typeStr = string.Empty;
                    if (type == "Term")
                        typeStr = type + " " + Convert.ToString(dsVal.Tables[0].Rows[row]["current_semester"]);
                    else
                        typeStr = Convert.ToString(dsVal.Tables[0].Rows[row]["current_semester"]) + " " + type;
                    string feecode = Convert.ToString(da.GetFunction("select textcode from textvaltable where textcriteria='FEECA' and textval like '" + typeStr + "%' and college_code='" + collegecode + "'"));
                    if (!arFeecat.Contains(feecode))
                    {
                        arFeecat.Add(feecode);
                    }
                    if (!CurSemDeg.ContainsKey(batDeg))
                    {
                        CurSemDeg.Add(batDeg, feecode);
                    }
                }
                if (sbCurSem.Length > 0)
                {
                    sbCurSem.Remove(sbCurSem.Length - 3, 3);
                    curSem = Convert.ToString(sbCurSem);
                }
            }

        }
        catch
        {
            curSem = string.Empty;
            arFeecat.Clear();
        }
        return curSem;

    }
}
