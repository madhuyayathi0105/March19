using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using wc = System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BalAccess;
using DalConnection;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;
using Gios.Pdf;
using System.IO;
using InsproDataAccess;
using System.Text;
using System.Net;
using System.Reflection;

public partial class LibraryMod_Library_Notification : System.Web.UI.Page
{
    #region initialization

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    public SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    DataTable dtCommon = new DataTable();
    DataSet dssmsrpt = new DataSet();
    SqlCommand cmd = new SqlCommand();
    static Hashtable Has_Stage = new Hashtable();
    ReuasableMethods ru = new ReuasableMethods();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    DAccess2 dacces2 = new DAccess2();
    DataTable libnitifi = new DataTable();
    DataRow boknitification;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
    string strcmdretrivesmsreport = "";
    bool cellflag = false;



    public void Connection()
    {
        con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
        con.Open();
    }
    DAccess2 d2 = new DAccess2();
    #endregion

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
           
                userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
          
            if (!Page.IsPostBack)
            {
                txtstartdate.Attributes.Add("Readonly", "Readonly");
                txtenddate.Attributes.Add("Readonly", "Readonly");
                txtstartdate.Text = DateTime.Today.ToString("d/MM/yyyy");
                txtenddate.Text = DateTime.Today.ToString("d/MM/yyyy");
                grdLibNotification.Visible = false;
            }
        }
        catch (Exception ex) { }
    }

    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        try
        {
            libnitifi.Columns.Add("Sender Name", typeof(string));
            libnitifi.Columns.Add("Date", typeof(string));
            libnitifi.Columns.Add("Time", typeof(string));
            libnitifi.Columns.Add("Subject", typeof(string));


            string startdate = "";
            string enddate = "";

            string date = txtstartdate.Text;
            string[] splitdate = date.Split(new char[] { '/' });
            startdate = splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString();
            DateTime stdate = Convert.ToDateTime(startdate);

            string date1 = txtenddate.Text;
            string[] splitdate1 = date1.Split(new char[] { '/' });
            enddate = splitdate1[1].ToString() + "/" + splitdate1[0].ToString() + "/" + splitdate1[2].ToString();
            DateTime eddate = Convert.ToDateTime(enddate);

            strcmdretrivesmsreport = "select distinct sender_id,convert(varchar(10),notification_date,103) date, RIGHT(CONVERT(VARCHAR, notification_time, 100),7) as Time,CONVERT(VARCHAR, notification_time, 108) as Time1, subject,status,notification_date,notification_time from  tbl_lib_notification where  convert(varchar(10),notification_date,101) between  cast('" + startdate + "' as datetime) and cast('" + enddate + "' as datetime) order by notification_date desc,notification_time desc";

            dssmsrpt = d2.select_method_wo_parameter(strcmdretrivesmsreport, "text");
            int sno = 0;
            int sr = 0;
            int rowHeight = 0;

            if (dssmsrpt.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dssmsrpt.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    boknitification = libnitifi.NewRow();
                    string sender1 = dssmsrpt.Tables[0].Rows[i]["sender_id"].ToString();
                    string ndate = dssmsrpt.Tables[0].Rows[i]["date"].ToString();
                    string subject = dssmsrpt.Tables[0].Rows[i]["subject"].ToString();
                    string time = dssmsrpt.Tables[0].Rows[i]["Time"].ToString();
                    string notetime = dssmsrpt.Tables[0].Rows[i]["Time1"].ToString();

                    boknitification["Sender Name"] = sender1;
                    boknitification["Date"] = ndate;
                    boknitification["Time"] = time;
                    boknitification["Subject"] = subject;
                    libnitifi.Rows.Add(boknitification);
                }
                grdLibNotification.DataSource = libnitifi;
                grdLibNotification.DataBind();
                grdLibNotification.Visible = true;
                for (int l = 0; l < grdLibNotification.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdLibNotification.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdLibNotification.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdLibNotification.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            grdLibNotification.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
            else
            {
                grdLibNotification.Visible = false;
                norecordlbl.Visible = true;
                norecordlbl.Text = "No Records Found";

            }



        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Notification");
        }
    }

    protected void grdLibNotification_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void grdLibNotification_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            btndelete.Enabled = true;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string date = Convert.ToString(grdLibNotification.Rows[rowIndex].Cells[2].Text);
            string time = Convert.ToString(grdLibNotification.Rows[rowIndex].Cells[3].Text);
            string sub = Convert.ToString(grdLibNotification.Rows[rowIndex].Cells[4].Text);


            strcmdretrivesmsreport = "select * from  tbl_lib_notification where subject='" + sub + "' order by notification_date desc,notification_time desc";

            dssmsrpt = d2.select_method_wo_parameter(strcmdretrivesmsreport, "text");
            if (dssmsrpt.Tables[0].Rows.Count > 0)
            {

                divTarvellerEntryDetails.Visible = true;
                btnsend.Text = "Update";
                txtsubject.Text = dssmsrpt.Tables[0].Rows[0]["subject"].ToString();
                txtnotification.Text = dssmsrpt.Tables[0].Rows[0]["notification"].ToString();
                Session["subject"] = txtsubject.Text;
                Session["notification"] = txtnotification.Text;

            }

        }


        catch
        {
        }
    }

    protected void grdLibNotification_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdLibNotification.PageIndex = e.NewPageIndex;
        btnMainGo_Click(sender, e);
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            string deleteQ;
            deleteQ = " delete from tbl_lib_notification where subject='" + txtsubject.Text + "' and notification='" + txtnotification.Text + "'";
            int delete = dacces2.update_method_wo_parameter(deleteQ, "Text");
            if (delete == 1)
            {               
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Deleted Successfully')", true);
                btnMainGo_Click(sender, e);
                txtnotification.Text = "";
                txtsubject.Text = "";
                btnsend.Text = "Send";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Notification");
        }

    }

    protected void btnsend_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtnotification.Text != "")
            {
                string viewer = "", notificationdate = "", subject = "", notifiaction = "", filetype = "", isstaff = "", islibrary = "";
                string file_extension = "", file_type = "";
                int fileSize = 0;
                byte[] documentBinary = new byte[0];
                byte[] attchementfile = new byte[0];
                int attachfile = 0;
                string attchefileexten = "", attachfiletype = "";
                Boolean atchflag = false;
                string filename = "";
                errnote.Visible = false;
                if (fudattachemnts.HasFile)
                {
                    if (fudattachemnts.FileName.EndsWith(".txt") || fudattachemnts.FileName.EndsWith(".pdf") || fudattachemnts.FileName.EndsWith(".doc") || fudattachemnts.FileName.EndsWith(".xls") || fudattachemnts.FileName.EndsWith(".xlsx") || fudattachemnts.FileName.EndsWith(".docx"))
                    {
                        atchflag = true;
                        attachfile = fudattachemnts.PostedFile.ContentLength;
                        attchementfile = new byte[attachfile];
                        fudattachemnts.PostedFile.InputStream.Read(attchementfile, 0, attachfile);
                        filename = fudattachemnts.PostedFile.FileName;
                        attchefileexten = Path.GetExtension(fudattachemnts.PostedFile.FileName);
                        attachfiletype = Get_file_format(attchefileexten);
                    }
                    else
                    {
                        errnote.Visible = true;
                        errnote.Text = "Please Select File Formate Like (.txt,.pdf,.doc,.xls,.xlsx,.docx)";
                        return;
                    }
                }

                Boolean fle = false;
                if (fudfile.HasFile)
                {
                    if (fudfile.FileName.EndsWith(".jpg") || fudfile.FileName.EndsWith(".jpeg") || fudfile.FileName.EndsWith(".JPG") || fudfile.FileName.EndsWith(".gif") || fudfile.FileName.EndsWith(".png"))
                    {
                        fle = true;
                        fileSize = fudfile.PostedFile.ContentLength;
                        documentBinary = new byte[fileSize];
                        fudfile.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        file_extension = Path.GetExtension(fudfile.PostedFile.FileName);
                        file_type = Get_file_format(file_extension);
                    }
                    else
                    {
                        errnote.Visible = true;
                        errnote.Text = "Please Select Image Formate Like (.jpg,.peg,.JPG,.gif,.png)";
                        return;
                    }
                }
                Boolean saveflag = false;

                #region Save

                string senderid = "", senderstaff = "0", descrip = "";
                string staffcode = Session["Staff_Code"].ToString();
                ds.Reset();
                ds.Dispose();
                string strquery = "";
                if (staffcode != "" && staffcode != null)
                {
                    senderstaff = "1";
                    strquery = "select Staff_name,dm.desig_name,hm.dept_name from staffmaster sm,stafftrans st,Desig_Master dm,HRDept_Master hm where sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and st.dept_code=hm.dept_code and sm.staff_code='" + staffcode + "'";
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        senderid = ds.Tables[0].Rows[0]["Staff_name"].ToString();
                        descrip = ds.Tables[0].Rows[0]["desig_name"].ToString() + " - " + ds.Tables[0].Rows[0]["dept_name"].ToString();
                    }
                }
                else
                {
                    if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                    {
                        group_user = Session["group_code"].ToString();
                        if (group_user.Contains(';'))
                        {
                            string[] group_semi = group_user.Split(';');
                            group_user = group_semi[0].ToString();
                        }
                        strquery = "select full_name,description from usermaster where group_code='" + group_user + "'";
                    }
                    else
                    {
                        strquery = "select full_name,description from usermaster where user_code='" + Session["UserCode"].ToString() + "' ";
                    }
                    usercode = Session["usercode"].ToString();
                    group_user = Session["group_code"].ToString();
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        senderid = ds.Tables[0].Rows[0]["full_name"].ToString();
                        descrip = ds.Tables[0].Rows[0]["description"].ToString();
                    }
                }
                saveflag = false;

                string query = "";
                string subj = "";
                string notifi = "";
                if (btnsend.Text != "Send")
                {
                    subj = Session["subject"].ToString();
                    notifi = Session["notification"].ToString();
                }

                if (fle == false && atchflag == false)
                {
                    if (btnsend.Text == "Send")
                    {
                        //query = "insert into tbl_lib_notification(notification_date,notification_time,subject,notification,status,staff_code,sender_id,Sender_Description,sender_staff)";
                        //query = query + "  values(@notification_date,@notification_time,@subject,@notification,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff)";
                        query = "if exists(select * from  tbl_lib_notification where notification_date=@notification_date and subject=@subject and notification=@notification)update tbl_lib_notification set notification_date=@notification_date , subject=@subject , notification=@notification where notification_date=@notification_date and subject=@subject and notification=@notification else insert into tbl_lib_notification(notification_date,notification_time,subject,notification,status,staff_code,sender_id,Sender_Description,sender_staff)  values(@notification_date,@notification_time,@subject,@notification,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff)";

                    }
                    else
                    {
                        query = "update tbl_lib_notification set subject=@subject,notification=@notification,status=@status where subject='" + subj + "' and notification='" + notifi + "'";

                    }
                }
                else if (fle == true && atchflag == false)
                {
                    if (btnsend.Text == "Send")
                    {
                        //query = "insert into tbl_lib_notification(notification_date,notification_time,subject,notification,filetype,fileupload,status,staff_code,sender_id,Sender_Description,sender_staff)";
                        //query = query + "  values(@notification_date,@notification_time,@subject,@notification,@filetype,@fileupload,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff)";

                        query = "if exists(select * from  tbl_lib_notification where notification_date=@notification_date and subject=@subject and notification=@notification  and filetype=@filetype)update tbl_lib_notification set notification_date=@notification_date , subject=@subject , notification=@notification , filetype=@filetype , fileupload=@fileupload where notification_date=@notification_date and subject=@subject and notification=@notification and filetype=@filetype and fileupload=@fileupload else insert into tbl_lib_notification(notification_date,notification_time,subject,notification,filetype,fileupload,status,staff_code,sender_id,Sender_Description,sender_staff)  values(@notification_date,@notification_time,@subject,@notification,@filetype,@fileupload,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff)";
                    }
                    else
                    {
                        query = "update tbl_lib_notification set subject=@subject,notification=@notification,filetype=@filetype,fileupload=@fileupload,status=@status where subject='" + subj + "' and notification='" + notifi + "'";

                    }
                }
                else if (fle == false && atchflag == true)
                {
                    if (btnsend.Text == "Send")
                    {
                        //query = "insert into tbl_lib_notification(notification_date,notification_time,subject,notification,status,staff_code,sender_id,Sender_Description,sender_staff,attche_filetype,attache_file,filename)";
                        //query = query + "  values(@notification_date,@notification_time,@subject,@notification,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff,@attche_filetype,@attache_file,@filename)";
                        query = "if exists(select * from  tbl_lib_notification where notification_date=@notification_date and subject=@subject and notification=@notification  and attche_filetype=@attche_filetype)update tbl_lib_notification set notification_date=@notification_date , subject=@subject , notification=@notification , attche_filetype=@attche_filetype , attache_file=@attache_file where notification_date=@notification_date and subject=@subject and notification=@notification and attche_filetype=@attche_filetype and attache_file=@attache_file else insert into tbl_lib_notification(notification_date,notification_time,subject,notification,status,staff_code,sender_id,Sender_Description,sender_staff,attche_filetype,attache_file,filename)   values     (@notification_date,@notification_time,@subject,@notification,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff,@attche_filetype,@attache_file,@filename)";
                    }
                    else
                    {
                        query = "update tbl_lib_notification set subject=@subject,notification=@notification,status=@status,attche_filetype=@attche_filetype,attache_file=@attache_file,filename=@filename where subject='" + subj + "' and notification='" + notifi + "'";

                    }
                }
                else if (fle == true && atchflag == true)
                {
                    if (btnsend.Text == "Send")
                    {
                        //query = "insert into tbl_lib_notification(notification_date,notification_time,subject,notification,status,staff_code,sender_id,Sender_Description,sender_staff,attche_filetype,attache_file,filetype,fileupload,filename)";
                        //query = query + "  values(@notification_date,@notification_time,@subject,@notification,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff,@attche_filetype,@attache_file,@filetype,@fileupload,@filename)";
                        query = "if exists(select * from  tbl_lib_notification where notification_date=@notification_date and subject=@subject and notification=@notification  and filetype=@filetype and fileupload=@fileupload and attche_filetype=@attche_filetype and attache_file=@attache_file )update tbl_lib_notification set notification_date=@notification_date , subject=@subject , notification=@notification , filetype=@filetype , fileupload=@fileupload , attche_filetype=@attche_filetype , attache_file=@attache_file where notification_date=@notification_date and subject=@subject and notification=@notification and filetype=@filetype and fileupload=@fileupload and attche_filetype=@attche_filetype and attache_file=@attache_file else insert into tbl_lib_notification(notification_date,notification_time,subject,notification,status,staff_code,sender_id,Sender_Description,sender_staff,attche_filetype,attache_file,filename)   values     (@notification_date,@notification_time,@subject,@notification,@status,@staff_code,@sender_id,@Sender_Description,@sender_staff,@attche_filetype,@attache_file,@filename)";
                    }
                    else
                    {
                        query = "update tbl_lib_notification set subject=@subject,notification=@notification,status=@status,attche_filetype=@attche_filetype,attache_file=@attache_file,filename=@filename,filetype=@filetype,fileupload=@fileupload where subject='" + subj + "' and notification='" + notifi + "'";

                    }

                }
                SqlCommand cmd = new SqlCommand(query, mysql);
                SqlParameter uploadedsubject_name = new SqlParameter();

                if (atchflag == true)
                {
                    uploadedsubject_name = new SqlParameter("@attche_filetype", SqlDbType.VarChar, 50);
                    uploadedsubject_name.Value = attachfiletype;
                    cmd.Parameters.Add(uploadedsubject_name);
                    uploadedsubject_name = new SqlParameter("@attache_file", SqlDbType.Binary, attachfile);
                    uploadedsubject_name.Value = attchementfile;
                    cmd.Parameters.Add(uploadedsubject_name);
                    uploadedsubject_name = new SqlParameter("@filename", SqlDbType.VarChar, 200);
                    uploadedsubject_name.Value = filename;
                    cmd.Parameters.Add(uploadedsubject_name);
                }
                uploadedsubject_name = new SqlParameter("@staff_code", SqlDbType.VarChar, 50);
                uploadedsubject_name.Value = staffcode;
                cmd.Parameters.Add(uploadedsubject_name);
                uploadedsubject_name = new SqlParameter("@sender_id", SqlDbType.VarChar, 50);
                uploadedsubject_name.Value = senderid;
                cmd.Parameters.Add(uploadedsubject_name);
                uploadedsubject_name = new SqlParameter("@Sender_Description", SqlDbType.VarChar, 50);
                uploadedsubject_name.Value = descrip;
                cmd.Parameters.Add(uploadedsubject_name);
                uploadedsubject_name = new SqlParameter("@sender_staff", SqlDbType.VarChar, 50);
                uploadedsubject_name.Value = senderstaff;
                cmd.Parameters.Add(uploadedsubject_name);

                string dtdate = DateTime.Now.ToString("MM/dd/yyyy");
                uploadedsubject_name = new SqlParameter("@notification_date", SqlDbType.VarChar, 50);
                uploadedsubject_name.Value = dtdate;
                cmd.Parameters.Add(uploadedsubject_name);
                string dttime = DateTime.Now.ToLongTimeString();
                uploadedsubject_name = new SqlParameter("@notification_time", SqlDbType.VarChar, 50);
                uploadedsubject_name.Value = dttime;
                cmd.Parameters.Add(uploadedsubject_name);
                subject = txtsubject.Text.ToString().Trim();
                if (subject == "" || subject == null)
                {
                    errnote.Visible = true;
                    errnote.Text = "Please Enter Subject";
                    return;
                }
                uploadedsubject_name = new SqlParameter("@subject", SqlDbType.NVarChar, 200);
                uploadedsubject_name.Value = subject;
                cmd.Parameters.Add(uploadedsubject_name);
                notifiaction = txtnotification.Text.ToString().Trim();
                if (notifiaction == null || notifiaction == "")
                {
                    errnote.Visible = true;
                    errnote.Text = "Please Enter Notification";
                    return;
                }
                if (notifiaction.Length > 8999)
                {
                    errnote.Visible = true;
                    errnote.Text = "Please Enter Notification Less than 9000 Character";
                    return;
                }
                uploadedsubject_name = new SqlParameter("@notification", SqlDbType.NVarChar, 1000);
                uploadedsubject_name.Value = notifiaction;
                cmd.Parameters.Add(uploadedsubject_name);

                string staus = "0";
                uploadedsubject_name = new SqlParameter("@status", SqlDbType.Int);
                uploadedsubject_name.Value = staus;
                cmd.Parameters.Add(uploadedsubject_name);
                if (fle == true)
                {
                    uploadedsubject_name = new SqlParameter("@filetype", SqlDbType.VarChar, 50);
                    uploadedsubject_name.Value = file_type;
                    cmd.Parameters.Add(uploadedsubject_name);
                    uploadedsubject_name = new SqlParameter("@fileupload", SqlDbType.Binary, fileSize);
                    uploadedsubject_name.Value = documentBinary;
                    cmd.Parameters.Add(uploadedsubject_name);
                }
                mysql.Close();
                mysql.Open();
                cmd.ExecuteNonQuery();
                mysql.Close();
                saveflag = true;
                if (saveflag == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Send Successfully')", true);
                    txtnotification.Text = "";
                    txtsubject.Text = "";
                    btnsend.Text = "Send";
                }

                // btnMainGo_Click(sender, e);
                #endregion
            }
            else
            {
                errnote.Visible = true;
                errnote.Text = "txtnotification is empty";
            }



        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Notification");
        }
    }

    public string Get_file_format(string file_extension)
    {
        try
        {
            string file_type = "";
            switch (file_extension)
            {
                case ".pdf":
                    file_type = "application/pdf";
                    break;
                case ".txt":
                    file_type = "application/notepad";
                    break;
                case ".xls":
                    file_type = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    file_type = "application/vnd.ms-excel";
                    break;
                case ".doc":
                    file_type = "application/vnd.ms-word";
                    break;
                case ".docx":
                    file_type = "application/vnd.ms-word";
                    break;
                case ".gif":
                    file_type = "image/gif";
                    break;
                case ".png":
                    file_type = "image/png";
                    break;
                case ".jpg":
                    file_type = "image/jpg";
                    break;
                case ".jpeg":
                    file_type = "image/jpeg";
                    break;
            }
            return file_type;
        }
        catch
        {
            return null;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {

            btnsend.Text = "Send";
            btndelete.Enabled = false;
            divTarvellerEntryDetails.Visible = true;
            txtnotification.Text = "";
            txtsubject.Text = "";

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Notification");
        }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            btnsend.Text = "Send";
            btndelete.Enabled = false;
            txtnotification.Text = "";
            txtsubject.Text = "";
            divTarvellerEntryDetails.Visible = false;
            btnMainGo_Click(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Notification");
        }
    }

    //protected void btnxl_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        string strexcelname = "";

    //        strexcelname = txtexcelname.Text;

    //        if (strexcelname != "")
    //        {
    //            lblnorec.Visible = false;

    //            d2.printexcelreport(FpSpread1, strexcelname);
    //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('saved successfully')", true);
    //        }
    //        else
    //        {
    //            lblnorec.Text = "Please enter your Report Name";
    //            lblnorec.Visible = true;
    //        }

    //        txtexcelname.Text = "";
    //        txtexcelname.Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblnorec.Text = ex.ToString();
    //    }
    //}

    //protected void btnprintmaster_Click(Object sender, EventArgs e)
    //{

    //    string degreedetails = "Library Notification" + '@' + "Date :" + txtstartdate.Text.ToString() + " To " + txtenddate.Text.ToString();
    //    string pagename = "Library_Notification.aspx";
    //    Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
    //    Printcontrol.Visible = true;
    //}



}