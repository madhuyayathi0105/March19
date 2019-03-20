using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class ChallanNoUpdate : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    int i = 0;
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static int personmode = 0;
    static int chosedmode = 0;
    static byte roll = 0;
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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    collegecode1 = ddl_collegename.SelectedItem.Value.ToString();
                }
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                loadsetting();
                RollAndRegSettings();
                btnsave.Visible = true;
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



    #region college
    public void loadcollege()
    {
        try
        {
            ddl_collegename.Items.Clear();
            ds.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        { }
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = ddl_collegename.SelectedItem.Value.ToString();
            }
        }
        catch
        {
        }
    }
    #endregion

    #region Auto search roll no and name
    //roll no
    public void loadsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");

            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
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
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
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
            txt_roll.Text = "";
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("Placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("Placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("Placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("Placeholder", "App No");
                    chosedmode = 2;
                    break;
            }
        }
        catch { }
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
                    query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' order by Roll_No";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' order by Reg_No";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' order by Roll_admit";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' order by app_formno";
                }
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    //name
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetName(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            WebService ws = new WebService();
            string qAdd = string.Empty;
            if (chosedmode == 0)
            {
                qAdd = "r.Roll_No,r.Roll_No ";
            }
            else if (chosedmode == 1)
            {
                qAdd = "r.Reg_No,r.Reg_No ";
            }
            else if (chosedmode == 2)
            {
                qAdd = "r.Roll_admit,r.Roll_admit ";
            }
            else
            {
                qAdd = "a.app_formno,a.app_formno ";
            }

            string query = "select  top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+" + qAdd + " from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%' and r.college_code=" + collegecode1 + "";

            Hashtable studhash = ws.Getnamevalue(query);
            if (studhash.Count > 0)
            {
                foreach (DictionaryEntry p in studhash)
                {
                    string studname = Convert.ToString(p.Key);
                    name.Add(studname);
                }
            }
            return name;
        }
        catch { return name; }
    }

    protected void txt_roll_Changed(object sender, EventArgs e)
    {
        try
        {
            string rollno = Convert.ToString(txt_roll.Text.Trim());
            if (rollno != "")
            {
                string query = "select   a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+ r.roll_no as Name from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR'  and r.college_code=" + collegecode1 + "";

                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    query = query + " and r.Roll_no='" + rollno + "' ";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    query = query + " and r.Reg_No='" + rollno + "'";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    query = query + " and r.Roll_Admit='" + rollno + "' ";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                {
                    query = query + " and a.app_formno='" + rollno + "' ";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txt_name.Text = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);
                }
                else
                {
                    txt_name.Text = "";
                }
            }
            else
            {
                txt_name.Text = "";
            }
        }
        catch { }
    }
    protected void txt_name_Changed(object sender, EventArgs e)
    {
        try
        {
            string roll_no = Convert.ToString(txt_name.Text);
            if (roll_no != "")
            {
                try
                {
                    string rollno = roll_no.Split('-')[4];
                    roll_no = rollno;
                    txt_roll.Text = roll_no;
                }
                catch { roll_no = ""; }
            }
            else
            {
                txt_roll.Text = "";
            }

        }
        catch { }
    }

    #endregion

    #region button go and save

    protected DataSet loadDatasetValues()
    {
        DataSet dsload = new DataSet();
        try
        {
            string fromdate = Convert.ToString(txt_fromdate.Text);
            string todate = Convert.ToString(txt_todate.Text);
            string appno = "";
            string rolno = "";
            string SelectQ = "";
            string studname = "";
            string rollno = Convert.ToString(txt_roll.Text);
            string name = Convert.ToString(txt_name.Text);
            if (name != "")
            {
                try
                {
                    studname = name.Split('-')[0];
                    rolno = name.Split('-')[4];
                }
                catch
                {
                    studname = "";
                    rolno = "";
                }
            }
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
            //appno
            if (rollno != "" || rolno != "")
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    appno = d2.GetFunction(" select App_No from Registration where Roll_No='" + rollno + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    appno = d2.GetFunction(" select App_No from Registration where Reg_no='" + rollno + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    appno = d2.GetFunction(" select App_No from Registration where Roll_admit='" + rollno + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                {
                    appno = d2.GetFunction(" select app_no from applyn where app_formno='" + rollno + "'");
                }
            }
            if (appno != "")
            {
                SelectQ = "select ChallanNo,CONVERT(varchar(10),ChallanDate,103) as ChallanDate,r.Roll_No,r.Reg_No,r.roll_admit,r.Stud_Name,SUM(TakenAmt)as totalamt,r.App_No from FT_ChallanDet c,Registration r where c.App_No=r.App_No and r.App_No='" + appno + "'  group by ChallanNo,ChallanDate,r.Roll_No,r.Reg_No,r.Stud_Name,r.App_No,r.roll_admit order by ChallanNo";
                // and ChallanDate between '" + fromdate + "' and '" + todate + "'
            }
            else
            {
                SelectQ = "select ChallanNo,CONVERT(varchar(10),ChallanDate,103) as ChallanDate,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name,SUM(TakenAmt)as totalamt,r.App_No from FT_ChallanDet c,Registration r where c.App_No=r.App_No and ChallanDate between '" + fromdate + "' and '" + todate + "' group by ChallanNo,ChallanDate,r.Roll_No,r.Reg_No,r.Stud_Name,r.App_No,r.roll_admit order by ChallanNo";
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ds = loadDatasetValues();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                LoadSpreadvalue();
            }
            else
            {
                divspread.Visible = false;
                FpSpreadbase.Visible = false;
                btnsave.Visible = false;
                print.Visible = false;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                txt_roll.Text = "";
                txt_name.Text = "";
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Record Found";
            }

        }
        catch { }
    }

    protected void LoadSpreadvalue()
    {
        try
        {
            #region design
            RollAndRegSettings();
            FpSpreadbase.Sheets[0].RowCount = 0;
            FpSpreadbase.Sheets[0].ColumnCount = 0;
            FpSpreadbase.CommandBar.Visible = false;
            FpSpreadbase.Sheets[0].AutoPostBack = false;
            FpSpreadbase.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpreadbase.Sheets[0].RowHeader.Visible = false;
            FpSpreadbase.Sheets[0].ColumnCount = 9;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            //  FarPoint.Web.Spread.DoubleCellType chaltxt = new FarPoint.Web.Spread.DoubleCellType();
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Challan No";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;


            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Date";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;


            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;


            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Admission No";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;



            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Name";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Amount";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Right;

            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Modify ChallanNo";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadbase.Sheets[0].Columns[0].Locked = true;
            FpSpreadbase.Sheets[0].Columns[1].Locked = true;
            FpSpreadbase.Sheets[0].Columns[2].Locked = true;
            FpSpreadbase.Sheets[0].Columns[3].Locked = true;
            FpSpreadbase.Sheets[0].Columns[4].Locked = true;
            FpSpreadbase.Sheets[0].Columns[5].Locked = true;
            FpSpreadbase.Sheets[0].Columns[6].Locked = true;
            FpSpreadbase.Sheets[0].Columns[7].Locked = true;
            FpSpreadbase.Sheets[0].Columns[0].Width = 40;
            FpSpreadbase.Sheets[0].Columns[1].Width = 112;
            FpSpreadbase.Sheets[0].Columns[2].Width = 73;
            FpSpreadbase.Sheets[0].Columns[3].Width = 124;
            FpSpreadbase.Sheets[0].Columns[4].Width = 124;
            FpSpreadbase.Sheets[0].Columns[5].Width = 124;
            FpSpreadbase.Sheets[0].Columns[6].Width = 205;
            FpSpreadbase.Sheets[0].Columns[7].Width = 108;
            FpSpreadbase.Sheets[0].Columns[8].Width = 144;
            //if (roll == 0)
            //{
            //    FpSpreadbase.Sheets[0].Columns[3].Visible = true;
            //    FpSpreadbase.Sheets[0].Columns[4].Visible = true;
            //    FpSpreadbase.Width = 950;
            //}
            //else if (roll == 1)
            //{
            //    FpSpreadbase.Sheets[0].Columns[3].Visible = true;
            //    FpSpreadbase.Sheets[0].Columns[4].Visible = true;
            //    FpSpreadbase.Width = 950;
            //    //FpSpreadbase.Width = 807px;
            //}
            //else if (roll == 2)
            //{
            //    FpSpreadbase.Sheets[0].Columns[3].Visible = true;
            //    FpSpreadbase.Sheets[0].Columns[4].Visible = false;
            //    FpSpreadbase.Width = 825;
            //}
            //else if (roll == 3)
            //{
            //    FpSpreadbase.Sheets[0].Columns[3].Visible = false;
            //    FpSpreadbase.Sheets[0].Columns[4].Visible = true;
            //    FpSpreadbase.Width = 825;
            //}
            spreadColumnVisible();
            #endregion

            #region value
            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                FpSpreadbase.Sheets[0].RowCount++;
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[sel]["ChallanNo"]);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["App_no"]);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[sel]["ChallanDate"]);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sel]["Roll_No"]);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[sel]["Reg_No"]);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[sel]["roll_admit"]);

                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[sel]["Stud_Name"]);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[sel]["totalamt"]);
                FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 8].Text = "";


            }
            FpSpreadbase.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            #endregion

            #region visible

            FpSpreadbase.Sheets[0].PageSize = FpSpreadbase.Sheets[0].RowCount;
            FpSpreadbase.SaveChanges();
            divspread.Visible = true;
            FpSpreadbase.Visible = true;
            FpSpreadbase.Height = 450;
            print.Visible = true;
            btnsave.Visible = true;
            FpSpreadbase.ShowHeaderSelection = false;
            #endregion

        }
        catch { }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            bool update = false;
            FpSpreadbase.SaveChanges();
            for (int sel = 0; sel < FpSpreadbase.Sheets[0].Rows.Count; sel++)
            {
                string altchlno = Convert.ToString(FpSpreadbase.Sheets[0].Cells[sel, 8].Text);
                if (altchlno != "")
                {
                    string challno = Convert.ToString(FpSpreadbase.Sheets[0].Cells[sel, 1].Text);
                    string appno = Convert.ToString(FpSpreadbase.Sheets[0].Cells[sel, 1].Tag);
                    string date = Convert.ToString(FpSpreadbase.Sheets[0].Cells[sel, 2].Text);
                    string[] frdate = date.Split('/');
                    if (frdate.Length == 3)
                    {
                        date = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                    }
                    if (altchlno != "" && appno != "" && date != "")
                    {
                        string UpdateQ = " update FT_ChallanDet set ChallanNo='" + altchlno + "' where App_no='" + appno + "' and  ChallanDate='" + date + "' and ChallanNo='" + challno + "'";
                        int upd = d2.update_method_wo_parameter(UpdateQ, "Text");
                        update = true;
                    }
                }
            }
            if (update == true)
            {
                btnsearch_Click(sender, e);
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Saved Successfully";
                FpSpreadbase.Visible = false;
                btnsave.Visible = false;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Enter Any one Challan No";
            }
        }
        catch { }
    }
    #endregion

    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
        txt_roll.Text = "";
        txt_name.Text = "";
        imgdiv2.Visible = false;

    }

    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {

                d2.printexcelreport(FpSpreadbase, reportname);
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
            degreedetails = "Challan No Report";
            pagename = "ChallanNoChange.aspx";
            Printcontrolhed.loadspreaddetails(FpSpreadbase, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    #endregion

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
        lbl.Add(lbl_collegename);
        fields.Add(0);

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
                FpSpreadbase.Columns[3].Visible = true;
                FpSpreadbase.Columns[4].Visible = true;
                FpSpreadbase.Columns[5].Visible = true;
            }
            else if (roll == 1)
            {
                FpSpreadbase.Columns[3].Visible = true;
                FpSpreadbase.Columns[4].Visible = true;
                FpSpreadbase.Columns[5].Visible = true;
            }
            else if (roll == 2)
            {
                FpSpreadbase.Columns[3].Visible = true;
                FpSpreadbase.Columns[4].Visible = false;
                FpSpreadbase.Columns[5].Visible = false;

            }
            else if (roll == 3)
            {
                FpSpreadbase.Columns[3].Visible = false;
                FpSpreadbase.Columns[4].Visible = true;
                FpSpreadbase.Columns[5].Visible = false;
            }
            else if (roll == 4)
            {
                FpSpreadbase.Columns[3].Visible = false;
                FpSpreadbase.Columns[4].Visible = false;
                FpSpreadbase.Columns[5].Visible = true;
            }
            else if (roll == 5)
            {
                FpSpreadbase.Columns[3].Visible = true;
                FpSpreadbase.Columns[4].Visible = true;
                FpSpreadbase.Columns[5].Visible = false;
            }
            else if (roll == 6)
            {
                FpSpreadbase.Columns[3].Visible = false;
                FpSpreadbase.Columns[4].Visible = true;
                FpSpreadbase.Columns[5].Visible = true;
            }
            else if (roll == 7)
            {
                FpSpreadbase.Columns[3].Visible = true;
                FpSpreadbase.Columns[4].Visible = false;
                FpSpreadbase.Columns[5].Visible = true;
            }
        }
        catch { }
    }

    #endregion

    // last modified sudhagar 25.11.2016
}