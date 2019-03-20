using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;

public partial class StudentMod_StudentMentorReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string StaffCode = string.Empty;
    bool Cellclick = false;
    int ACTROW = 0;
    ReuasableMethods rs = new ReuasableMethods();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();


    public void Page_Load(object sender, EventArgs e)
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
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            StaffCode = Session["Staff_Code"].ToString();
            lbl_validation.Text = "";
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!IsPostBack)
            {
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string Master = "select * from Master_Settings where settings in('Roll No','Register No','Admission No') " + grouporusercode + "";
                DataSet ds = d2.select_method_wo_parameter(Master, "Text");
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Admissionflag"] = "1";
                        }
                    }
                }
                mentorStudentDetails();
                loadsetting();
                TxtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                //ddl_searchtype.Items.Add(new System.Web.UI.WebControls.ListItem("Roll No", "0"));
                //ddl_searchtype.Items.Add(new System.Web.UI.WebControls.ListItem("Reg No", "1"));
                //ddl_searchtype.Items.Add(new System.Web.UI.WebControls.ListItem("Admission No", "2"));
                //ddl_searchtype.Items.Add(new System.Web.UI.WebControls.ListItem("App No", "3"));
                //txt_searchappno.Attributes.Add("placeholder", "Roll No");
            }
        }
        catch (Exception ex) { }
    }

    protected void ddl_searchtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        loadsearchtype();
    }

    protected void loadsearchtype()
    {
        switch (Convert.ToUInt32(ddl_searchtype.SelectedItem.Value))
        {
            case 0:
                txt_searchappno.Attributes.Add("placeholder", "Roll No");
                break;
            case 1:
                txt_searchappno.Attributes.Add("placeholder", "Reg No");
                break;
            case 2:
                txt_searchappno.Attributes.Add("placeholder", "Admission No");
                break;
            case 3:
                txt_searchappno.Attributes.Add("placeholder", "App No");
                break;

        }
    }

    public void loadsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");
            //ListItem lst5 = new ListItem("Smartcard No", "4");
            //Roll Number or Reg Number or Admission No or Application Number
            ddl_searchtype.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + Session["collegecode"].ToString() + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Roll No
                ddl_searchtype.Items.Add(list1);
            }
            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + Session["collegecode"].ToString() + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                ddl_searchtype.Items.Add(list2);
            }
            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + Session["collegecode"].ToString() + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                ddl_searchtype.Items.Add(list3);
            }
            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' and college_code ='" + Session["collegecode"].ToString() + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                ddl_searchtype.Items.Add(list4);
            }
            //insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptSmartNo' and user_code ='" + usercode + "' and college_code in(" + Session["collegecode"].ToString() + ") ";
            //save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            //if (save1 == 1)
            //{
            //    //Smartcard No - smart_serial_no
            //    ddl_searchtype.Items.Add(lst5);
            //}
            int smartDisp = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' and college_code in (" + Session["collegecode"].ToString() + ")").Trim());
            if (ddl_searchtype.Items.Count == 0)
            {
                ddl_searchtype.Items.Add(list1);
            }
            switch (Convert.ToUInt32(ddl_searchtype.SelectedItem.Value))
            {
                case 0:
                case1:
                    txt_searchappno.Attributes.Add("placeholder", "Roll No");
                    // txt_roll.Text = "Roll No";

                    break;
                case 1:
                case2:
                    txt_searchappno.Attributes.Add("placeholder", "Reg No");
                    // txt_roll.Text = "Reg No";

                    break;
                case 2:
                case3:
                    txt_searchappno.Attributes.Add("placeholder", "Admin No");
                    // txt_roll.Text = "Admin No";

                    break;
                case 3:
                case4:
                    txt_searchappno.Attributes.Add("placeholder", "App No");
                    // txt_roll.Text = "App No";

                    break;
                case 4:
                    txt_searchappno.Attributes.Add("placeholder", "Smartcard No");
                    //txt_roll.Text = "SmartCard No";

                    switch (smartDisp)
                    {
                        case 0:
                            goto case1;
                        case 1:
                            goto case2;
                        case 2:
                            goto case3;
                        case 3:
                            goto case4;
                    }
                    break;
            }

        }
        catch (Exception ex) { }
    }

    public string Encrypt(string message)
    {
        UTF8Encoding textConverter = new UTF8Encoding();
        RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
        //Convert the data to a byte array.
        byte[] toEncrypt = textConverter.GetBytes(message);
        //Get an encryptor.
        ICryptoTransform encryptor = rc2CSP.CreateEncryptor(ScrambleKey, ScrambleIV);
        //Encrypt the data.
        MemoryStream msEncrypt = new MemoryStream();
        CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        //Write all data to the crypto stream and flush it.
        // Encode length as first 4 bytes
        byte[] length = new byte[4];
        length[0] = (byte)(message.Length & 0xFF);
        length[1] = (byte)((message.Length >> 8) & 0xFF);
        length[2] = (byte)((message.Length >> 16) & 0xFF);
        length[3] = (byte)((message.Length >> 24) & 0xFF);
        csEncrypt.Write(length, 0, 4);
        csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
        csEncrypt.FlushFinalBlock();
        //Get encrypted array of bytes.
        byte[] encrypted = msEncrypt.ToArray();
        // Convert to Base64 string
        string b64 = Convert.ToBase64String(encrypted);
        // Protect against URLEncode/Decode problem
        string b64mod = b64.Replace('+', '@');
        // Return a URL encoded string
        return HttpUtility.UrlEncode(b64mod);
    }

    public string Decrypt(string scrambledMessage)
    {
        UTF8Encoding textConverter = new UTF8Encoding();
        RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
        // URL decode , replace and convert from Base64
        string b64mod = HttpUtility.UrlDecode(scrambledMessage);
        // Replace '@' back to '+' (avoid URLDecode problem)
        string b64 = b64mod.Replace('@', '+');
        // Base64 decode
        byte[] encrypted = Convert.FromBase64String(b64);
        //Get a decryptor that uses the same key and IV as the encryptor.
        ICryptoTransform decryptor = rc2CSP.CreateDecryptor(ScrambleKey, ScrambleIV);
        //Now decrypt the previously encrypted message using the decryptor
        // obtained in the above step.
        MemoryStream msDecrypt = new MemoryStream(encrypted);
        CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        byte[] fromEncrypt = new byte[encrypted.Length - 4];
        //Read the data out of the crypto stream.
        byte[] length = new byte[4];
        csDecrypt.Read(length, 0, 4);
        csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
        int len = (int)length[0] | (length[1] << 8) | (length[2] << 16) | (length[3] << 24);
        //Convert the byte array back into a string.
        return textConverter.GetString(fromEncrypt).Substring(0, len);
    }

    public byte[] ScrambleKey
    {
        set
        {
            byte[] key = value;
            if (null == key)
            {
                // Use existing key if non provided
                key = ScrambleKey;
            }
            Session["ScrambleKey"] = key;
        }
        get
        {
            byte[] key = (byte[])Session["ScrambleKey"];
            if (null == key)
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.GenerateKey();
                key = rc2.Key;
                Session["ScrambleKey"] = key;
            }
            return key;
        }
    }

    public byte[] ScrambleIV
    {
        set
        {
            byte[] key = value;
            if (null == key)
            {
                key = ScrambleIV;
            }
            Session["ScrambleIV"] = key;
        }
        get
        {
            byte[] key = (byte[])Session["ScrambleIV"];
            if (null == key)
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.GenerateIV();
                key = rc2.IV;
                Session["ScrambleIV"] = key;
            }
            return key;
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getappfrom(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //"Roll No", "0"
        //"Reg No", "1"
        //"Admission No", "2"
        //"App No", "3"
        int SEARCHTYPE = 0;
        int.TryParse(contextKey, out SEARCHTYPE);
        string type = "";
        switch (SEARCHTYPE)
        {
            case 0:
                type = "r.roll_no";
                break;
            case 1:
                type = "r.reg_no";
                break;
            case 2:
                type = "r.Roll_Admit";
                break;
            case 3:
                type = "r.app_no";
                break;
        }
        string query = " select " + type + "  from applyn a,Registration r,CO_StudentTutor ct where ct.app_no=r.app_no and ct.app_no=a.app_no and a.app_no=r.App_No  and " + type + " like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    protected void btn_Search_OnClick(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            lbl_error.Visible = false;
            string type = Convert.ToString(ddl_searchtype.SelectedValue);
            mentorStudentDetails(type);
        }
        catch (Exception ex)
        {
            lbl_error.Text = ex.ToString();
            lbl_error.Visible = true;
        }
    }

    protected void mentorStudentDetails(string searchType = null)
    {
        try
        {
            Hashtable ParameterValuehash = new Hashtable();
            string searchValue = txt_searchappno.Text;
            if (!string.IsNullOrEmpty(searchType) && !string.IsNullOrEmpty(searchValue))
            {
                ParameterValuehash.Add("@SearchType", searchType);
                ParameterValuehash.Add("@SearchValue", searchValue);
            }
            ParameterValuehash.Add("@StaffCode", StaffCode);
            ds = d2.select_method("StudentMentorReport", ParameterValuehash, "sp");
            DataTable dtStudentMentor = new DataTable();
            DataRow drow;
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                dtStudentMentor.Columns.Add("SNo", typeof(string));
                dtStudentMentor.Columns.Add("Roll No", typeof(string));
                dtStudentMentor.Columns.Add("App No", typeof(string));
                dtStudentMentor.Columns.Add("Reg No", typeof(string));
                dtStudentMentor.Columns.Add("Admission No", typeof(string));
                dtStudentMentor.Columns.Add("Student Name", typeof(string));
                dtStudentMentor.Columns.Add("Degree", typeof(string));
                dtStudentMentor.Columns.Add("Staff Name", typeof(string));
                dtStudentMentor.Columns.Add("Staff Code", typeof(string));
                dtStudentMentor.Columns.Add("Counselling", typeof(string));

                drow = dtStudentMentor.NewRow();
                drow["SNo"] = "SNo";
                drow["Roll No"] = "Roll No";
                drow["App No"] = "App No";
                drow["Reg No"] = "Reg No";
                drow["Admission No"] = "Admission No";
                drow["Student Name"] = "Student Name";
                drow["Degree"] = "Degree";
                drow["Staff Name"] = "Staff Name";
                drow["Staff Code"] = "Staff Code";
                drow["Counselling"] = "Counselling";
                dtStudentMentor.Rows.Add(drow);
                int sno = 0;
                if (!string.IsNullOrEmpty(txtstaff.Text))
                {
                    ds.Tables[0].DefaultView.RowFilter = "Staff_Name like '" + txtstaff.Text + "%'";
                    DataView dvstaff = ds.Tables[0].DefaultView;
                    if (dvstaff.Count > 0)
                    {
                        for (int m = 0; m < dvstaff.Count; m++)
                        {
                            sno++;
                            drow = dtStudentMentor.NewRow();
                            drow["SNo"] = Convert.ToString(sno);
                            drow["Roll No"] = Convert.ToString(dvstaff[m]["Roll_No"]);
                            drow["App No"] = Convert.ToString(dvstaff[m]["app_no"]);
                            drow["Reg No"] = Convert.ToString(dvstaff[m]["Reg_No"]);
                            drow["Admission No"] = Convert.ToString(dvstaff[m]["roll_admit"]);
                            drow["Student Name"] = Convert.ToString(dvstaff[m]["Stud_Name"]);
                            drow["Degree"] = Convert.ToString(dvstaff[m]["Degree"]);
                            drow["Staff Name"] = Convert.ToString(dvstaff[m]["Staff_Name"]);
                            drow["Staff Code"] = Convert.ToString(dvstaff[m]["Staff_code"]);
                            drow["Counselling"] = "Counselling";
                            dtStudentMentor.Rows.Add(drow);
                        }
                    }
                    else
                    {
                        divStudentMentor.Visible = false;
                        GrdStudentMentor.Visible = false;
                        rptprint.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Founds";
                        lbl_rptname.Visible = false;
                        txt_excelname.Visible = false;
                        btn_excel.Visible = false;
                        btn_printmaster.Visible = false;
                        btnPrint.Visible = false;
                    }
                }

                else
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sno++;
                        drow = dtStudentMentor.NewRow();
                        drow["SNo"] = Convert.ToString(sno);
                        drow["Roll No"] = Convert.ToString(dr["Roll_No"]);
                        drow["App No"] = Convert.ToString(dr["app_no"]);
                        drow["Reg No"] = Convert.ToString(dr["Reg_No"]);
                        drow["Admission No"] = Convert.ToString(dr["roll_admit"]);
                        drow["Student Name"] = Convert.ToString(dr["Stud_Name"]);
                        drow["Degree"] = Convert.ToString(dr["Degree"]);
                        drow["Staff Name"] = Convert.ToString(dr["Staff_Name"]);
                        drow["Staff Code"] = Convert.ToString(dr["Staff_code"]);
                        drow["Counselling"] = "Counselling";
                        dtStudentMentor.Rows.Add(drow);
                    }
                }
                //divStudentMentor.Visible = true;
                GrdStudentMentor.DataSource = dtStudentMentor;
                GrdStudentMentor.DataBind();
                RowHead(GrdStudentMentor);
                GrdStudentMentor.Visible = true;
                rptprint.Visible = true;

                for (int l = 0; l < GrdStudentMentor.Rows.Count; l++)
                {
                    foreach (GridViewRow row in GrdStudentMentor.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            GrdStudentMentor.Rows[l].Cells[1].Width = 80;
                            GrdStudentMentor.Rows[l].Cells[2].Width = 80;
                            GrdStudentMentor.Rows[l].Cells[9].ForeColor = Color.Green;
                        }
                    }
                }


            }
            else
            {
                divStudentMentor.Visible = true;
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Founds";
                GrdStudentMentor.Visible = false;
                rptprint.Visible = false;
                lbl_rptname.Visible = false;
                txt_excelname.Visible = false;
                btn_excel.Visible = false;
                btn_printmaster.Visible = false;
                btnPrint.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void RowHead(GridView GrdStudentMentor)
    {
        for (int head = 0; head < 1; head++)
        {
            GrdStudentMentor.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            GrdStudentMentor.Rows[head].Font.Bold = true;
            GrdStudentMentor.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    #region Added by saranya on 9/10/2018 for Counselling

    protected void GrdStudentMentor_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Convert.ToString(Session["Rollflag"]) == "1")
                e.Row.Cells[1].Visible = true;
            else
                e.Row.Cells[1].Visible = false;
            if (Convert.ToString(Session["Regflag"]) == "1")
                e.Row.Cells[3].Visible = true;
            else
                e.Row.Cells[3].Visible = false;
            if (Convert.ToString(Session["Admissionflag"]) == "1")
                e.Row.Cells[4].Visible = true;
            else
                e.Row.Cells[4].Visible = false;
            e.Row.Cells[2].Visible = false;

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToString(Session["Rollflag"]) == "1")
                e.Row.Cells[1].Visible = true;
            else
                e.Row.Cells[1].Visible = false;
            if (Convert.ToString(Session["Regflag"]) == "1")
                e.Row.Cells[3].Visible = true;
            else
                e.Row.Cells[3].Visible = false;
            if (Convert.ToString(Session["Admissionflag"]) == "1")
                e.Row.Cells[4].Visible = true;
            else
                e.Row.Cells[4].Visible = false;
            e.Row.Cells[2].Visible = false;
        }
    }

    protected void GrdStudentMentor_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenFieldStudentMentor.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void GrdStudentMentor_SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.HiddenFieldStudentMentor.Value);
        string ColumnValue = Convert.ToString(GrdStudentMentor.Rows[rowIndex].Cells[selectedCellIndex].Text);
        if (rowIndex != -1)
        {
            if (ColumnValue == "Counselling")
            {
                string appno = Convert.ToString(GrdStudentMentor.Rows[rowIndex].Cells[2].Text);
                string staffName = Convert.ToString(GrdStudentMentor.Rows[rowIndex].Cells[7].Text);
                string StudentName = Convert.ToString(GrdStudentMentor.Rows[rowIndex].Cells[5].Text);
                Session["appno"] = appno;
                Session["staffName"] = staffName;
                Session["StudentName"] = StudentName;
                CounsellingPopup.Visible = true;
                loaddesc1();
                CounsellingGo(sender, e);
            }
            else
            {
                Session["appno"] = "";
                string appno = Convert.ToString(GrdStudentMentor.Rows[rowIndex].Cells[2].Text);
                Session["appno"] = appno;
                Session["studentmentor"] = "studentmentor";
                Response.Redirect("../IndReport.aspx?app=" + Encrypt(appno) + "&Type=Admin");
            }
        }
    }

    private string AesEncrypt(string password)
    {
        string strmsg = string.Empty;
        byte[] encode = new byte[password.Length];
        encode = Encoding.UTF8.GetBytes(password);
        strmsg = Convert.ToBase64String(encode);
        return strmsg;
    }

    public void TxtDate_OnTextChanged(object sender, EventArgs e)
    {
        DateTime DtCurrentDate = DateTime.Now;
        string datetime = TxtDate.Text;
        string[] splitDt = datetime.Split('/');
        datetime = splitDt[1] + "/" + splitDt[0] + "/" + splitDt[2];
        DateTime Dt = Convert.ToDateTime(datetime);
        if (Dt > DtCurrentDate)
        {
            imgdivMessage.Visible = true;
            lbl_erroralert.Text = "Date should not be greater than the current date";
            TxtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    protected void btnAddDesc_OnClick(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addDesc.Visible = true;
        lbl_addDesc.Text = "Add Description";
        lblerror.Visible = false;
    }

    protected void btn_addDesc_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_addDesc.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_addDesc.Text + "' and MasterCriteria ='Counselling Description' and CollegeCode ='" + collegecode1 + "') update CO_MasterValues set MasterValue ='" + txt_addDesc.Text + "' where MasterValue ='" + txt_addDesc.Text + "' and MasterCriteria ='Counselling Description' and CollegeCode ='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_addDesc.Text + "','Counselling Description','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdivMessage.Visible = true;
                    lbl_erroralert.Text = "Saved sucessfully";
                    txt_addDesc.Text = "";
                    plusdiv.Visible = false;
                    panel_addDesc.Visible = false;
                }
                loaddesc1();
            }
            else
            {
                imgdivMessage.Visible = true;
                lbl_erroralert.Text = "Enter the description";
            }
        }
        catch
        {
        }
    }

    public void loaddesc1()
    {
        try
        {
            ddl_Description.Items.Clear();
            string query = " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='Counselling Description' and CollegeCode ='" + collegecode1 + "' order by MasterValue asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_Description.DataSource = ds;
                    ddl_Description.DataTextField = "MasterValue";
                    ddl_Description.DataValueField = "MasterCode";
                    ddl_Description.DataBind();
                }
            }
        }
        catch { }
    }

    protected void btn_exitDesc_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addDesc.Visible = false;
        loaddesc1();
    }

    protected void btnDeleteDesc_OnClick(object sender, EventArgs e)
    {
        if (ddl_Description.Items.Count > 0)
        {
            surediv.Visible = true;
        }
        else
        {
            imgdivMessage.Visible = true;
            lbl_erroralert.Text = "No Description Selected";
        }
    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = false;
            if (ddl_Description.Items.Count > 0)
            {

                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_Description.SelectedItem.Value.ToString() + "' and MasterCriteria ='Counselling Description' and CollegeCode='" + collegecode1 + "' ";
                int delete = d2.update_method_wo_parameter(sql, "TEXT");
                if (delete != 0)
                {
                    imgdivMessage.Visible = true;
                    lbl_erroralert.Text = "Deleted Sucessfully";
                }
                else
                {
                    imgdivMessage.Visible = true;
                    lbl_erroralert.Text = "Not deleted";
                }
                loaddesc1();
            }

            else
            {
                imgdivMessage.Visible = true;
                lbl_erroralert.Text = "No Description Selected";
            }
        }
        catch { }
    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
    }

    protected void btnerrexit_Click(object sender, EventArgs e)
    {
        imgdivMessage.Visible = false;
    }

    protected void btn_popclose_Click(object sender, EventArgs e)
    {
        CounsellingPopup.Visible = false;
        Session["staffName"] = "";
        Session["appno"] = "";
    }

    protected void BtnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string Counsdate = Convert.ToString(TxtDate.Text);
            string[] splitDt = Counsdate.Split('/');
            Counsdate = splitDt[1] + "/" + splitDt[0] + "/" + splitDt[2];
            string Desc = Convert.ToString(ddl_Description.SelectedItem.Text);
            string CounseGiven = TxtCounseGiven.Text;
            string greivance = TxtGrievance.Text;
            string Staff_Name = Convert.ToString(Session["staffName"]);
            string StudentAppNo = Convert.ToString(Session["appno"]);

            string InsQ = "if exists(select * from StudentCounsellingDetails where appNo='" + StudentAppNo + "' and counselling_date='" + Counsdate + "' and Counselling_Description='" + Desc + "' and counselling_given='" + CounseGiven + "' and Grievance='" + greivance + "' and CounsellorName='" + Staff_Name + "') update StudentCounsellingDetails set counselling_date='" + Counsdate + "',Counselling_Description='" + Desc + "',counselling_given='" + CounseGiven + "',Grievance='" + greivance + "',CounsellorName='" + Staff_Name + "' where appNo='" + StudentAppNo + "' and Counselling_Description='" + Desc + "' and counselling_given='" + CounseGiven + "' and Grievance='" + greivance + "' and CounsellorName='" + Staff_Name + "' else insert into StudentCounsellingDetails(appNo ,counselling_date,Counselling_Description,counselling_given ,Grievance,CounsellorName) values('" + StudentAppNo + "','" + Counsdate + "','" + Desc + "','" + CounseGiven + "','" + greivance + "','" + Staff_Name + "')";
            int upd = d2.update_method_wo_parameter(InsQ, "Text");
            if (upd > 0)
            {
                imgdivMessage.Visible = true;
                lbl_erroralert.Text = "Saved Successfully";
                TxtGrievance.Text = "";
                TxtCounseGiven.Text = "";
                CounsellingGo(sender, e);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void CounsellingGo(object sender, EventArgs e)
    {
        try
        {
            TxtGrievance.Text = "";
            TxtCounseGiven.Text = "";
            string Staff_Name = Convert.ToString(Session["staffName"]);
            string StudentAppNo = Convert.ToString(Session["appno"]);
            string studentName = Convert.ToString(Session["StudentName"]);
            string selQry = "select * from StudentCounsellingDetails where appNo='" + StudentAppNo + "'";
            ds = d2.select_method_wo_parameter(selQry, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtCounselling = new DataTable();
                DataRow drow;

                dtCounselling.Columns.Add("Student Name", typeof(string));
                dtCounselling.Columns.Add("Counselling Date", typeof(string));
                dtCounselling.Columns.Add("Description", typeof(string));
                dtCounselling.Columns.Add("Counselling Given", typeof(string));
                dtCounselling.Columns.Add("Greivance", typeof(string));
                dtCounselling.Columns.Add("Counsellor Name", typeof(string));

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string Counsdate = Convert.ToString(ds.Tables[0].Rows[row]["counselling_date"]);
                    string[] splitDt = Counsdate.Split('/');
                    Counsdate = splitDt[1] + "/" + splitDt[0] + "/" + splitDt[2];
                    drow = dtCounselling.NewRow();
                    drow["Student Name"] = studentName;
                    drow["Counselling Date"] = Convert.ToString(Counsdate.Split(' ')[0]);
                    drow["Description"] = Convert.ToString(ds.Tables[0].Rows[row]["Counselling_Description"]).Trim();
                    drow["Counselling Given"] = Convert.ToString(ds.Tables[0].Rows[row]["counselling_given"]).Trim();
                    drow["Greivance"] = Convert.ToString(ds.Tables[0].Rows[row]["Grievance"]).Trim();
                    drow["Counsellor Name"] = Convert.ToString(ds.Tables[0].Rows[row]["CounsellorName"]).Trim();
                    dtCounselling.Rows.Add(drow);
                }
                divCounsellingReport.Visible = true;
                grdcounselling.DataSource = dtCounselling;
                grdcounselling.DataBind();
                grdcounselling.Visible = true;
                BtnSave.Enabled = true;
                BtnUpdate.Enabled = false;
                BtnDelete.Enabled = false;
                BtncounsellingPrint.Visible = true;
            }
            else
            {
                divCounsellingReport.Visible = false;
                BtncounsellingPrint.Visible = false;
                grdcounselling.Visible = false;
                BtnSave.Enabled = true;
                BtnUpdate.Enabled = false;
                BtnDelete.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdcounselling_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void grdcounselling_SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        if (rowIndex != -1)
        {
            string counsDt = Convert.ToString(grdcounselling.Rows[rowIndex].Cells[2].Text);
            string counsDesc = Convert.ToString(grdcounselling.Rows[rowIndex].Cells[3].Text);
            string counsGiven = Convert.ToString(grdcounselling.Rows[rowIndex].Cells[4].Text);
            string Grievance = Convert.ToString(grdcounselling.Rows[rowIndex].Cells[5].Text);
            Session["counsellingGiven"] = counsGiven;
            Session["counsellingGrievance"] = Grievance;

            TxtDate.Text = counsDt;
            TxtCounseGiven.Text = counsGiven;
            TxtGrievance.Text = Grievance;
            ddl_Description.ClearSelection();
            // ddl_Description.Items.FindByText(counsDesc).Selected = true;
            BtnSave.Enabled = false;
            BtnUpdate.Enabled = true;
            BtnDelete.Enabled = true;
        }
    }

    protected void BtnUpdate_OnClick(object sender, EventArgs e)
    {
        try
        {

            string Counsdate = Convert.ToString(TxtDate.Text);
            string[] splitDt = Counsdate.Split('/');
            Counsdate = splitDt[1] + "/" + splitDt[0] + "/" + splitDt[2];
            string Desc = Convert.ToString(ddl_Description.SelectedItem.Text);
            string CounseGiven = TxtCounseGiven.Text;
            string greivance = TxtGrievance.Text;
            string Staff_Name = Convert.ToString(Session["staffName"]);
            string StudentAppNo = Convert.ToString(Session["appno"]);
            string counsellingGiven = Convert.ToString(Session["counsellingGiven"]);
            string counsellingGrievance = Convert.ToString(Session["counsellingGrievance"]);

            string Updateqry = "update StudentCounsellingDetails set counselling_date='" + Counsdate + "',Counselling_Description='" + Desc + "',counselling_given='" + CounseGiven + "',Grievance='" + greivance + "',CounsellorName='" + Staff_Name + "' where appNo='" + StudentAppNo + "' and Counselling_Description='" + Desc + "' and counselling_given='" + counsellingGiven + "' and Grievance='" + counsellingGrievance + "' and CounsellorName='" + Staff_Name + "'";
            int upd = d2.update_method_wo_parameter(Updateqry, "Text");
            if (upd > 0)
            {
                imgdivMessage.Visible = true;
                lbl_erroralert.Text = "Updated Successfully";
                TxtGrievance.Text = "";
                TxtCounseGiven.Text = "";
                CounsellingGo(sender, e);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnDelete_OnClick(object sender, EventArgs e)
    {
        SureDivCounselling.Visible = true;
    }

    protected void btnSureYesDelCouns_Click(object sender, EventArgs e)
    {
        try
        {
            string counsellingGiven = Convert.ToString(Session["counsellingGiven"]);
            string counsellingGrievance = Convert.ToString(Session["counsellingGrievance"]);
            SureDivCounselling.Visible = false;
            string Counsdate = Convert.ToString(TxtDate.Text);
            string[] splitDt = Counsdate.Split('/');
            Counsdate = splitDt[1] + "/" + splitDt[0] + "/" + splitDt[2];
            string Desc = Convert.ToString(ddl_Description.SelectedItem.Text);
            string Staff_Name = Convert.ToString(Session["staffName"]);
            string StudentAppNo = Convert.ToString(Session["appno"]);

            string sql = "delete from StudentCounsellingDetails where appNo='" + StudentAppNo + "' and Counselling_Description='" + Desc + "' and counselling_given='" + counsellingGiven + "' and Grievance='" + counsellingGrievance + "' and CounsellorName='" + Staff_Name + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {
                imgdivMessage.Visible = true;
                lbl_erroralert.Text = "Deleted Sucessfully";
                CounsellingGo(sender, e);
                TxtGrievance.Text = "";
                TxtCounseGiven.Text = "";
            }
            else
            {
                imgdivMessage.Visible = true;
                lbl_erroralert.Text = "Not deleted";
            }
        }
        catch { }
    }

    protected void btnSureNoDelCouns_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
    }

    #endregion

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                DataTable dtexport = new DataTable();
                DataRow drexport;
                string testname = string.Empty;
                dtexport.Columns.Add("Roll No", typeof(string));
                dtexport.Columns.Add("Reg No", typeof(string));
                dtexport.Columns.Add("Student Name", typeof(string));
                dtexport.Columns.Add("Degree", typeof(string));
                dtexport.Columns.Add("Staff Name", typeof(string));
                dtexport.Columns.Add("Staff Code", typeof(string));
                DataSet dsstud = new DataSet();
                foreach (GridViewRow grow in GrdStudentMentor.Rows)
                {
                    drexport = dtexport.NewRow();
                    drexport["Roll No"] = Convert.ToString(grow.Cells[1].Text);
                    drexport["Reg No"] = Convert.ToString(grow.Cells[3].Text);
                    drexport["Student Name"] = Convert.ToString(grow.Cells[5].Text);
                    drexport["Degree"] = Convert.ToString(grow.Cells[6].Text);
                    drexport["Staff Name"] = Convert.ToString(grow.Cells[7].Text);
                    drexport["Staff Code"] = Convert.ToString(grow.Cells[8].Text);

                    dtexport.Rows.Add(drexport);
                }
                if (dtexport.Columns.Count > 0)
                {
                    ExportTable(dtexport, "Student Mentor Report");
                }
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch
        {
        }
    }

    private void ExportTable(DataTable dtt, string filename)
    {
        Response.ClearContent();
        Response.Buffer = true;
        string headername = Convert.ToString(filename + DateTime.Now.ToString("dd/MM/yyyy-hh:ss") + ".xls");
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", headername));
        Response.ContentType = "application/ms-excel";
        DataTable dt = dtt;
        string str = string.Empty;
        foreach (DataColumn dtcol in dt.Columns)
        {
            Response.Write(str + dtcol.ColumnName);
            str = "\t";
        }
        str = string.Empty;
        Response.Write("\n");
        foreach (DataRow dr in dt.Rows)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                string value = Convert.ToString(dr[j]);
                if (value.EndsWith("\r\n"))
                {
                    string[] values = value.Split('\r');
                    string val = values[0];
                    Response.Write(str + val);
                    str = "\t";
                }
                else
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
            }
            str = "\r\n";
        }
        System.Web.HttpContext.Current.Response.Flush();
        Response.End();
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Student Mentor Report";
            string pagename = "StudentMentorReport.aspx";
            Printcontrol.loadspreaddetails(GrdStudentMentor, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }



    // by kowshi(07.01.2018)
    #region staff
    protected void ddlstaff_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }
    # region Getrno
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();
            {
                string txtval = string.Empty;

                query = "select distinct sm.staff_name from staffmaster sm where sm.Staff_Name like '" + prefixText + "%' ";
            }
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    # endregion
    #endregion

}