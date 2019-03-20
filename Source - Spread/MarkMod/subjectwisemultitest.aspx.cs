using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Configuration;

public partial class subjectwisemultitest : System.Web.UI.Page
{

    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    ReuasableMethods rs=new  ReuasableMethods();
    DataTable dtforbindvalues = new DataTable();
    ArrayList arrliststud = new ArrayList();
    DataSet studgradeds = new DataSet();
    DataSet dsmarkss = new DataSet();
    string usercode = "";
    string collegecode = "";
    string singleuser = "";
    string group_user = "";
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();

    //added by rajasekar 08/10/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    
    ArrayList headcolspan = new ArrayList();
    int firstrowscount = 0;
    int subcount = 0;
    int testcount = 0;

    //============================//

    protected void Page_Load(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;

        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!Request.FilePath.Contains("CAMHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/MarkMod/CAMHome.aspx");
                return;
            }
        }
        if (!IsPostBack)
        {
            rbmark.Checked = true;
            bindyear();
            bindschooltype();
            bindstandard();

            bindtest();
            bindsubject();
            final.Visible = false;


            Showgrid.Visible = false;

        }
    }

    protected void go_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            hide();
            lblerrormsg.Visible = true;
            dtforbindvalues.Columns.Add("columnno");
            dtforbindvalues.Columns.Add("cretiano");
            dtforbindvalues.Columns.Add("subjectno");
            dtforbindvalues.Columns.Add("minmark");
            dtforbindvalues.Columns.Add("maxmark");
            dtforbindvalues.Columns.Add("total");

            string batchyear = "";
            string degreecode = "";
            string sem = "";
            if (dropyear.Items.Count > 0)
            {
                batchyear = dropyear.SelectedItem.Text.ToString();
            }
            else
            {
            }

            if (ddstandard.Items.Count > 0)
            {
                degreecode = ddstandard.SelectedItem.Value.ToString();
            }
            else
            {

            }

            if (ddlSemYr.Items.Count > 0)
            {
                sem = ddlSemYr.SelectedItem.Text.ToString().Trim();
            }
            else
            {

            }
            int count = 0;
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                if (cbltest.Items[i].Selected == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Test";
                txtrngfrom.Text = "";
                txtrngto.Text = "";
                hide();
                lblerrormsg.Visible = true;
                return;


            }
            else
            {
                lblerrormsg.Text = "";

            }
            count = 0;
            for (int i = 0; i < cblsub.Items.Count; i++)
            {
                if (cblsub.Items[i].Selected == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Subject";
                txtrngfrom.Text = "";
                txtrngto.Text = "";
                hide();
                lblerrormsg.Visible = true;
                return;


            }
            else
            {
                lblerrormsg.Text = "";

            }
            if ((txtrngfrom.Text != "" && txtrngto.Text == "") || (txtrngfrom.Text == "" && txtrngto.Text != ""))
            {
                lblerrormsg.Text = "Please Enter Correct Range Values";

                hide();
                txtrngfrom.Text = "";
                txtrngto.Text = "";
                lblerrormsg.Visible = true;
                return;
            }
            else
            {
                lblerrormsg.Text = "";
            }

            if (txtrngfrom.Text != "" && txtrngto.Text != "")
            {
                int fromval = Convert.ToInt32(txtrngfrom.Text.ToString());
                int toval = Convert.ToInt32(txtrngto.Text.ToString());
                if (fromval > toval)
                {
                    lblerrormsg.Text = "Please Enter Correct Range Values";

                    hide();
                    txtrngfrom.Text = "";
                    txtrngto.Text = "";
                    lblerrormsg.Visible = true;
                    return;
                }
                else
                {
                    lblerrormsg.Text = "";
                }
            }

            string rangefrom = txtrngfrom.Text;
            int countifn = 0;
            if (rangefrom.Contains('-'))
            {
                for (int i = 0; i < rangefrom.Length; i++)
                {
                    string data = rangefrom[i].ToString();
                    if (data.Contains('-'))
                    {
                        countifn++;
                    }

                }
            }
            if (countifn >= 2)
            {
                lblerrormsg.Text = "Please Enter Correct Range Values";

                hide();
                lblerrormsg.Visible = true;
                txtrngfrom.Text = "";
                txtrngto.Text = "";
                return;
            }
            else
            {
                lblerrormsg.Text = "";
            }

            

            string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
            string strorder = "";
            string strregorder = "";
            if (orderby_Setting == "")
            {
                strorder = "ORDER BY r.Roll_No";
                strregorder = "ORDER BY registration.Roll_No";
            }
            else
            {
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY r.Roll_No";
                    strregorder = "ORDER BY registration.Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY r.Reg_No";
                    strregorder = "ORDER BY registration.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strregorder = "ORDER BY registration.Stud_Name";
                    strorder = "ORDER BY r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                    strregorder = "ORDER BY registration.Roll_No,registration.Reg_No,registration.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY r.Roll_No,r.Reg_No";
                    strregorder = "ORDER BY registration.Roll_No,registration.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                    strregorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY r.Roll_No,r.Stud_Name";
                    strregorder = "ORDER BY registration.Roll_No,registration.Stud_Name";
                }
            }
            string strsec = "";
            if (ddlSec.Enabled == true)
            {
                string sections = ddlSec.SelectedItem.Text.ToString();
                if (sections.ToString().ToLower() == "all" || sections.ToString() == "" || sections.ToString() == "-1")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and LTRIM(RTRIM(ISNULL(sections,'')))='" + sections.ToString() + "'";
                }

            }
            string gender = "";
            if (ddlgender.SelectedItem.Text.ToString().Trim().ToLower() == "male")
            {
                gender = "and sex='0'";
            }
            else if (ddlgender.SelectedItem.Text.ToString().Trim().ToLower() == "female")
            {
                gender = "and sex='1'";
            }

            string dayshostr = "";
            if (ddlhost.SelectedItem.Text.ToString().Trim() == "Hostler")
            {
                dayshostr = "and registration.Stud_Type='Hostler'";
            }
            else if (ddlhost.SelectedItem.Text.ToString().Trim() == "Day Scholar")
            {
                dayshostr = "and registration.Stud_Type='Day Scholar'";
            }
            string final11 = strsec + " " + gender + " " + dayshostr + " " + strregorder;
            string strheadquery = "select distinct registration.Roll_No as 'Roll No-l',registration.Reg_No as 'Reg No-l',registration.stud_name as 'Name-l' from registration , applyn a      where a.app_no=registration.app_no and registration.degree_code='" + degreecode + "'   and registration.batch_year='" + batchyear + "'  and RollNo_Flag<>0 and cc=0      and exam_flag <> 'DEBAR' and delflag=0   " + final11 + " ";
            ds.Reset();
            ds.Dispose();
            ds = da.select_method(strheadquery, hat, "Text");
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);
                
                for (int headcol = 0; headcol < ds.Tables[0].Columns.Count + 1;headcol++ )
                    dtl.Columns.Add("", typeof(string));

                for (int rowcount1 = 0; rowcount1 < ds.Tables[0].Rows.Count; rowcount1++)
                {
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);
                }
                 firstrowscount= ds.Tables[0].Rows.Count + 2;
                
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        

                        dtl.Rows[j + 2][0] = Convert.ToString(j + 1);
                        dtl.Rows[j + 2][i + 1] = ds.Tables[0].Rows[j][i].ToString();
                    }
                }
                dtl.Columns[0].ColumnName = "S.No";
                dtl.Rows[0][0] = "S.No";

                
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                   
                    string colname = ds.Tables[0].Columns[i].ToString();
                    string[] splitcolname = colname.Split('-');
                    if (splitcolname.GetUpperBound(0) > 0)
                    {
                        string align = splitcolname[1].ToString();
                        colname = splitcolname[0].ToString();
                        if (align.Trim() == "l")
                        {
                            
                        }
                        else if (align.Trim() == "r")
                        {
                           
                        }


                        
                        dtl.Columns[i + 1].ColumnName = colname;
                        dtl.Rows[0][i + 1] = colname;
                    }
                    else
                    {
                        dtl.Columns[i + 1].ColumnName = colname;
                        dtl.Rows[0][i + 1] = colname;

                        
                    }

                }
                
            }

            Showgrid.Visible = true;
            if (dtl.Rows.Count > 2)
            {
                bindfpheader();
                if (rbmark.Checked == true)
                {
                    bindfpvalue();
                }
                else
                {
                    bindfparrearvalue();
                }
            }
            else
            {
                lblerrormsg.Text = "No Records Found";
                txtrngfrom.Text = "";
                txtrngto.Text = "";
                hide();
                lblerrormsg.Visible = true;
                return;
            }
   
            

            if (dtl.Rows.Count > 0 && dtl.Columns.Count> 4)
            {
                
                final.Visible = true;

                Showgrid.DataSource = dtl;
                Showgrid.DataBind();
                Showgrid.Visible = true;
                Showgrid.HeaderRow.Visible = false;
                int ccc = 4;
                int ii = 0;

                    for (int i = 0; i < Showgrid.Rows.Count; i++)
                    {

                        for (int j = 0; j < dtl.Columns.Count; j++)
                        {

                            if (i == 0 || i == 1)
                            {
                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                
                                if (i == 0)
                                {
                                    
                                    if (j < 4)
                                    {
                                        Showgrid.Rows[i].Cells[j].RowSpan = 2;
                                        for (int a = i; a < 1; a++)
                                            Showgrid.Rows[a + 1].Cells[j].Visible = false;
                                    }
                                    else if (j == ccc)
                                    {
                                        if (rb_testWise.Selected)
                                        {

                                            if (headcolspan.Count > ii)
                                            {

                                                
                                                    string headtext = "";
                                                    int spancount = 0;
                                                    string rrr = headcolspan[ii].ToString();
                                                    string[] splitval6 = rrr.Split('^');
                                                    if (splitval6.Length > 1)
                                                    {
                                                        headtext = splitval6[0].ToString();
                                                        spancount = Convert.ToInt32(splitval6[1].ToString());


                                                    }

                                                    
                                                    Showgrid.Rows[i].Cells[j].ColumnSpan = spancount;
                                                    for (int a = j + 1; a < j + spancount; a++)
                                                        Showgrid.Rows[i].Cells[a].Visible = false;

                                                    ccc += spancount;
                                                    ii++;
                                                

                                            }

                                        }
                                        else
                                        {
                                            if (headcolspan.Count > ii)
                                            {
                                                    Showgrid.Rows[i].Cells[j].ColumnSpan = testcount;
                                                    for (int a = j + 1; a < j + testcount; a++)
                                                        Showgrid.Rows[i].Cells[a].Visible = false;

                                                    ccc += testcount;
                                                    ii++;

                                            }
                                        }

                                        

                                    }

                                }
                            }
                            else
                            {
                                Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                if (j != 1 && j != 2 && j != 3)
                                {
                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                }
                                string rrr = Showgrid.Rows[i].Cells[j].Text;
                                string[] splitval6 = rrr.Split('$');
                                string[] splitval2 = rrr.Split('#');
                                if (splitval6.Length > 1 || splitval2.Length > 1)
                                {
                                    if (splitval6.Length > 1 && j > 3)
                                    {
                                        Showgrid.Rows[i].Cells[j].Text = splitval6[0].ToString();

                                        Showgrid.Rows[i].Cells[j].ForeColor = Color.Red;
                                        Showgrid.Rows[i].Cells[j].Font.Underline = true;
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;

                                    }
                                    else if (splitval2.Length > 1 && j > 3)
                                    {


                                        Showgrid.Rows[i].Cells[j].Text = splitval2[0].ToString();
                                        Showgrid.Rows[i].Cells[j].BackColor = Color.PaleVioletRed;


                                    }



                                }
                                else
                                    Showgrid.Rows[i].Cells[j].Text = rrr;


                                if (firstrowscount <= i && j == 0)
                                {
                                    Showgrid.Rows[i].Cells[j].ColumnSpan = 4;
                                    for (int a = 1; a < 4; a++)
                                    {
                                        Showgrid.Rows[i].Cells[a + j].Visible = false;
                                        Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                }
                            }

                        }





                    }
                
            }
            else
            {
                lblerrormsg.Text = "No Records Found";
                txtrngfrom.Text = "";
                txtrngto.Text = "";
                hide();
                lblerrormsg.Visible = true;
                return;

            }
        }
    
        catch
        {
        }
    }


    public void bindfpvalue()
    {
        try
        {
            string strsec = "";
            if (ddlSec.Enabled == true)
            {
                string sections = ddlSec.SelectedItem.Text.ToString();
                if (sections.ToString().ToLower() == "all" || sections.ToString() == "" || sections.ToString() == "-1")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and LTRIM(RTRIM(ISNULL(r.sections,'')))='" + sections.ToString() + "'";
                }

            }

            string gender = "";
            if (ddlgender.SelectedItem.Text.ToString().Trim().ToLower() == "male")
            {
                gender = "and sex='0'";
            }
            else if (ddlgender.SelectedItem.Text.ToString().Trim().ToLower() == "female")
            {
                gender = "and sex='1'";
            }

            string dayshostr = "";
            if (ddlhost.SelectedItem.Text.ToString().Trim() == "Hostler")
            {
                dayshostr = "and r.Stud_Type='Hostler'";
            }
            else if (ddlhost.SelectedItem.Text.ToString().Trim() == "Day Scholar")
            {
                dayshostr = "and r.Stud_Type='Day Scholar'";
            }
            string final11 = strsec + " " + gender + " " + dayshostr;

            int passcount = 0;
            int failcount = 0;
            int minmark = 0;
            //int mr50to60 = 0;
            //int mr60to70 = 0;
            //int mr70to80 = 0;

            int fromrange = 0;
            int torange = 0;
            Boolean norange = false;
            if (txtrngfrom.Text.ToString() != "" && txtrngto.Text.ToString() != "")
            {
                fromrange = Convert.ToInt32(txtrngfrom.Text.ToString());
                torange = Convert.ToInt32(txtrngto.Text.ToString());
                norange = true;
            }

            string roll_no = "";
            string criteriano = "";
            string batchyear = "";
            string degreecode = "";
            string sem = "";
            if (dropyear.Items.Count > 0)
            {
                batchyear = dropyear.SelectedItem.Text.ToString();
            }
            else
            {

            }

            if (ddstandard.Items.Count > 0)
            {
                degreecode = ddstandard.SelectedItem.Value.ToString();
            }
            else
            {

            }

            if (ddlSemYr.Items.Count > 0)
            {
                sem = ddlSemYr.SelectedItem.Text.ToString().Trim();
            }
            else
            {

            }

            //string strsec = "";
            //if (ddlSec.Enabled == true)
            //{
            //    string sections = ddlSec.SelectedItem.Text.ToString();
            //    if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
            //    {
            //        strsec = "";
            //    }
            //    else
            //    {
            //        strsec = " and sections='" + sections.ToString() + "'";
            //    }

            //}
            string colno = "";
            string total = string.Empty;
            string subjectno = "";
            double markget = 0;
            for (int j = 0; j < dtforbindvalues.Rows.Count; j++)
            {
                if (Int32.TryParse(dtforbindvalues.Rows[j][3].ToString(), out minmark))
                {
                    minmark = Convert.ToInt32(dtforbindvalues.Rows[j][3].ToString());
                }

                colno = dtforbindvalues.Rows[j][0].ToString();
                criteriano = dtforbindvalues.Rows[j][1].ToString();
                subjectno = dtforbindvalues.Rows[j][2].ToString();
                for (int i = 0; i < dtl.Rows.Count-2; i++)
                {

                    

                    roll_no = dtl.Rows[i+2][1].ToString();

                    //string markob = da.GetFunction("select distinct rt.marks_obtained as mark from registration r, applyn a , result rt,exam_type et, subject s where  a.app_no=r.app_no and r.sections = et.sections  and    et.subject_no=s.subject_no  and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0      and s.subject_no= '" + subjectno + "' and r.roll_no='" + roll_no + "'");
                    string markob = "select distinct rt.marks_obtained as mark from registration r, applyn a , result rt,exam_type et, subject s where  a.app_no=r.app_no  and  et.subject_no=s.subject_no  and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0      and s.subject_no= '" + subjectno + "' and r.roll_no='" + roll_no + "'";//and LTRIM(RTRIM(ISNULL(r.sections,''))) = LTRIM(RTRIM(ISNULL(et.sections,''))) 
                    //string totmark = " select distinct SUM (rt.marks_obtained )as mark from registration r, applyn a , result rt,exam_type et, subject s where  a.app_no=r.app_no  and  et.subject_no=s.subject_no  and RollNo_Flag<>0 and et.exam_code=rt.exam_code and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0      and s.subject_no='" + subjectno + "' and r.roll_no='" + roll_no + "'";

                    //DataSet totinternal = new DataSet();
                    //totinternal = da.select_method_wo_parameter(totmark, "text");


                    dsmarkss.Clear();
                    dsmarkss = da.select_method_wo_parameter(markob, "Text");
                    if (dsmarkss.Tables[0].Rows.Count > 0)
                    {
                        markob = dsmarkss.Tables[0].Rows[0][0].ToString();

                    }
                    else
                    {
                        markob = "";
                    }
                    if (markob != "" && markob != null)
                    {
                        markget = Convert.ToDouble(markob);
                        if (markget < 0)
                        {

                            if (norange == false)
                            {
                                markob = loadmarkat(markob);
                                
                                dtl.Rows[i + 2][Convert.ToInt32(colno)] = markob;
                            }
                            else
                            {
                                if (markget >= fromrange && markget <= torange)
                                {
                                    markob = loadmarkat(markob);
                                    
                                    dtl.Rows[i + 2][Convert.ToInt32(colno)] = markob;
                                }
                            }
                        }
                        else
                        {
                            if (norange == false)
                            {
                                
                                dtl.Rows[i + 2][Convert.ToInt32(colno)] = markob;
                            }
                            else
                            {
                                if (markget >= fromrange && markget <= torange)
                                {
                                    // markob = loadmarkat(markob);
                                    
                                    dtl.Rows[i + 2][Convert.ToInt32(colno)] = markob;
                                }
                            }
                        }
                    }
                    else
                    {
                        
                        dtl.Rows[i + 2][Convert.ToInt32(colno)] = " ";

                    }            
                    double marksd = 0;
                    if (double.TryParse(dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString(), out marksd))
                    {
                       
                        marksd = Convert.ToDouble(dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString());
                        if (minmark <= marksd)
                        {


                        }
                        else
                        {
                           

                            dtl.Rows[i + 2][Convert.ToInt32(colno)] = dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString() + "$Red";
                        }

                    }
                    else
                    {
                        if (dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString().ToUpper() == "EL" || dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString().ToUpper() == "NJ")
                        {

                        }
                        else
                            if (dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString().Trim() == "")
                            {
                            }
                            else
                            {
                                
                                dtl.Rows[i + 2][Convert.ToInt32(colno)] = dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString() + "#PaleVioletRed";
                        
                            }

                    }
                   
                   
                }
            }
                
                

            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Stud Pass in Each Subject";
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Stud Fail in Each Subject";
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "Range : 50 to 60";
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "Range : 61 to 70";
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "Range : 71 to 80";
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "Range : 81 to 90";
            
            for (int j = 0; j < dtforbindvalues.Rows.Count; j++)
            {
                passcount = 0;
                failcount = 0;
                if (Int32.TryParse(dtforbindvalues.Rows[j][3].ToString(), out minmark))
                {
                    minmark = Convert.ToInt32(dtforbindvalues.Rows[j][3].ToString());
                }
                colno = dtforbindvalues.Rows[j][0].ToString();
                criteriano = dtforbindvalues.Rows[j][1].ToString();
                subjectno = dtforbindvalues.Rows[j][2].ToString();
                for (int i = 0; i < dtl.Rows.Count - 8; i++)
                {
                    double marksd = 0;
                    string value2 = dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString();
                    string[] splitval6 = value2.Split('$');
                    string[] splitval2 = value2.Split('#');
                    if (splitval6.Length > 1 || splitval2.Length > 1)
                    {
                        if (splitval6.Length > 1)
                        {
                            value2 = splitval6[0].ToString();
                        }
                        else if (splitval2.Length > 1)
                        {
                            value2 = splitval2[0].ToString();

                        }


                    }
                    if (double.TryParse(value2.ToString(), out marksd))
                    {
                        
                        marksd = Convert.ToDouble(value2);

                        if (minmark <= marksd)
                        {
                            passcount++;
                        }
                        else
                        {
                            failcount++;
                        }

                    }
                    else
                    {
                        if (value2.ToString().ToUpper() == "NJ")
                        {

                        }
                        else
                            if (value2.ToString().ToUpper() == "EL")
                            {
                                passcount++;
                            }
                            else
                            {
                                failcount++;
                            }
                    }
                }

                
                dtl.Rows[dtl.Rows.Count - 6][Convert.ToInt32(colno)] = Convert.ToString(passcount);

                dtl.Rows[dtl.Rows.Count - 5][Convert.ToInt32(colno)] = Convert.ToString(failcount);

                string range1 = da.GetFunction("select distinct count(r.Roll_No) as count11 from registration r, applyn a , result rt,exam_type et, subject s where   a.app_no=r.app_no  and r.degree_code='" + degreecode + "' and    et.subject_no=s.subject_no and r.batch_year='" + batchyear + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0       and s.subject_no= '" + subjectno + "' " + final11 + " and  rt.marks_obtained between 50 and 60");//and LTRIM(RTRIM(ISNULL(r.sections,''))) = LTRIM(RTRIM(ISNULL(et.sections,'')))
                int markran = 0;
                if (Int32.TryParse(range1, out markran))
                {

                    markran = Convert.ToInt32(range1);
                    
                    dtl.Rows[dtl.Rows.Count - 4][Convert.ToInt32(colno)] = Convert.ToString(markran);
                }
                else
                {
                    
                    dtl.Rows[dtl.Rows.Count - 4][Convert.ToInt32(colno)] = "0";
                }

                range1 = da.GetFunction("select distinct count(r.Roll_No) as count11 from registration r, applyn a , result rt,exam_type et, subject s where   a.app_no=r.app_no and r.degree_code='" + degreecode + "' and    et.subject_no=s.subject_no and r.batch_year='" + batchyear + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0       and s.subject_no= '" + subjectno + "' " + final11 + " and  rt.marks_obtained between 61 and 70");//and LTRIM(RTRIM(ISNULL(r.sections,''))) = LTRIM(RTRIM(ISNULL(et.sections,''))) 
                markran = 0;
                if (Int32.TryParse(range1, out markran))
                {
                    markran = Convert.ToInt32(range1);
                    

                    dtl.Rows[dtl.Rows.Count - 3][Convert.ToInt32(colno)] = Convert.ToString(markran);

                }
                else
                {
                    
                    dtl.Rows[dtl.Rows.Count - 3][Convert.ToInt32(colno)] = "0";
                }

                range1 = da.GetFunction("select distinct count(r.Roll_No) as count11 from registration r, applyn a , result rt,exam_type et, subject s where   a.app_no=r.app_no  and r.degree_code='" + degreecode + "' and    et.subject_no=s.subject_no and r.batch_year='" + batchyear + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0       and s.subject_no= '" + subjectno + "' " + final11 + " and  rt.marks_obtained between 71 and 80");//and LTRIM(RTRIM(ISNULL(r.sections,''))) = LTRIM(RTRIM(ISNULL(et.sections,'')))
                markran = 0;
                if (Int32.TryParse(range1, out markran))
                {
                    markran = Convert.ToInt32(range1);
                    

                    dtl.Rows[dtl.Rows.Count - 2][Convert.ToInt32(colno)] = Convert.ToString(markran);

                }
                else
                {
                    

                    dtl.Rows[dtl.Rows.Count - 2][Convert.ToInt32(colno)] = "0";
                }


                range1 = da.GetFunction("select distinct count(r.Roll_No) as count11 from registration r, applyn a , result rt,exam_type et, subject s where   a.app_no=r.app_no  and r.degree_code='" + degreecode + "' and    et.subject_no=s.subject_no and r.batch_year='" + batchyear + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0       and s.subject_no= '" + subjectno + "' " + final11 + " and  rt.marks_obtained between 81 and 90");//and LTRIM(RTRIM(ISNULL(r.sections,''))) = LTRIM(RTRIM(ISNULL(et.sections,'')))
                markran = 0;
                if (Int32.TryParse(range1, out markran))
                {
                    markran = Convert.ToInt32(range1);
                    

                    dtl.Rows[dtl.Rows.Count - 1][Convert.ToInt32(colno)] = Convert.ToString(markran);

                }
                else
                {
                   

                    dtl.Rows[dtl.Rows.Count - 1][Convert.ToInt32(colno)] = "0";
                }
            }
            //added by mullai
            if(Chktotinternal.Checked==true)
            {
                int fpto = 0;
            for (int i = 0; i < cblsub.Items.Count; i++)
            {

                if (cblsub.Items[i].Selected == true)
                {
                    string subno = cblsub.Items[i].Value;
                 
                    string subjCode = da.GetFunction("select subject_code from subject where subject_no='" + subno + "'");
                    if (!string.IsNullOrEmpty(subno))
                    {
                        fpto++;
                        int colcount = dtl.Columns.Count;
                        
                        dtl.Columns.Add("", typeof(string));
                        


                        dtl.Columns[dtl.Columns.Count - 1].ColumnName= subjCode;
                        dtl.Rows[0][dtl.Columns.Count - 1] = "Internal Mark  ";
                        dtl.Rows[1][dtl.Columns.Count - 1] = subjCode;
                   
                        

                        for (int k = 0; k < dtl.Rows.Count-2; k++)
                        {
                           
                            string tot = string.Empty;
                            DataSet totint = new DataSet();
                            string roll_no1 = dtl.Rows[k + 2][1].ToString();
                            

                            string totinternal = da.GetFunction("select t.Exammark  from tbl_Cam_Calculation t,subject s,Registration r where t.roll_no='" + roll_no1 + "' and s.subject_no='" + subno + "' and  s.subject_no=t.subject_no and r.Roll_No=t.roll_no and t.Istype='Internal Marks Out of 100'");
                           
                            

                            dtl.Rows[k + 2][colcount] = totinternal;
                        }

                    }
                }

                }

            }


        }
        catch
        {
        }
    }

    public void bindfparrearvalue()
    {
        try
        {
            int outmark = 0;
            DataSet dsstudentlistarr = new DataSet();
            DataView dv_stud = new DataView();
            dsstudentlistarr.Clear();
            string strsec = "";
            if (ddlSec.Enabled == true)
            {
                string sections = ddlSec.SelectedItem.Text.ToString();
                if (sections.ToString().ToLower() == "all" || sections.ToString() == "" || sections.ToString() == "-1")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and LTRIM(RTRIM(ISNULL(r.sections,'')))='" + sections.ToString() + "'";
                }

            }

            string gender = "";
            if (ddlgender.SelectedItem.Text.ToString().Trim().ToLower() == "male")
            {
                gender = "and sex='0'";
            }
            else if (ddlgender.SelectedItem.Text.ToString().Trim().ToLower() == "female")
            {
                gender = "and sex='1'";
            }

            string dayshostr = "";
            if (ddlhost.SelectedItem.Text.ToString().Trim() == "Hostler")
            {
                dayshostr = "and r.Stud_Type='Hostler'";
            }
            else if (ddlhost.SelectedItem.Text.ToString().Trim() == "Day Scholar")
            {
                dayshostr = "and r.Stud_Type='Day Scholar'";
            }
            string final11 = strsec + " " + gender + " " + dayshostr;

            int passcount = 0;
            int failcount = 0;
            int minmark = 0;
            //int mr50to60 = 0;
            //int mr60to70 = 0;
            //int mr70to80 = 0;

            int fromrange = 0;
            int torange = 0;
            Boolean norange = false;
            if (txtrngfrom.Text.ToString() != "" && txtrngto.Text.ToString() != "")
            {
                fromrange = Convert.ToInt32(txtrngfrom.Text.ToString());
                torange = Convert.ToInt32(txtrngto.Text.ToString());
                norange = true;
            }

            string roll_no = "";
            string criteriano = "";
            string batchyear = "";
            string degreecode = "";
            string sem = "";
            if (dropyear.Items.Count > 0)
            {
                batchyear = dropyear.SelectedItem.Text.ToString();
            }
            else
            {

            }

            if (ddstandard.Items.Count > 0)
            {
                degreecode = ddstandard.SelectedItem.Value.ToString();
            }
            else
            {

            }

            if (ddlSemYr.Items.Count > 0)
            {
                sem = ddlSemYr.SelectedItem.Text.ToString().Trim();
            }
            else
            {

            }

            //string strsec = "";
            //if (ddlSec.Enabled == true)
            //{
            //    string sections = ddlSec.SelectedItem.Text.ToString();
            //    if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
            //    {
            //        strsec = "";
            //    }
            //    else
            //    {
            //        strsec = " and sections='" + sections.ToString() + "'";
            //    }

            //}
            string colno = "";
            string subjectno = "";
            double markget = 0;
            for (int j = 0; j < dtforbindvalues.Rows.Count; j++)
            {
                if (Int32.TryParse(dtforbindvalues.Rows[j][3].ToString(), out outmark))
                {
                    minmark = Convert.ToInt32(dtforbindvalues.Rows[j][3].ToString());
                }
                else
                {
                    minmark = 0;
                }

                colno = dtforbindvalues.Rows[j][0].ToString();
                criteriano = dtforbindvalues.Rows[j][1].ToString();
                subjectno = dtforbindvalues.Rows[j][2].ToString();
                string arrstudlistsql = "select distinct count(s.subject_no) as fail, r.roll_no from registration r, applyn a , result rt,exam_type et, subject s where  a.app_no=r.app_no and LTRIM(RTRIM(ISNULL(r.sections,''))) = LTRIM(RTRIM(ISNULL(et.sections,'')))  and    et.subject_no=s.subject_no   and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "'   and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0        and  rt.marks_obtained < '" + minmark + "'  and rt.marks_obtained <> -2  group by r.roll_no";
                dsstudentlistarr = da.select_method_wo_parameter(arrstudlistsql, "Text");
                int arrercount = 0;
                for (int i = 0; i < dtl.Rows.Count-2; i++)
                {

                    

                    roll_no=dtl.Rows[i + 2][1].ToString();

                    dsstudentlistarr.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_no + "'";
                    dv_stud = dsstudentlistarr.Tables[0].DefaultView;
                    int count4 = 0;
                    count4 = dv_stud.Count;

                    if (count4 > 0)
                    {
                        //arrercount = Convert.ToInt32(dv_stud[0]["fail"].ToString());
                        if (Int32.TryParse(dv_stud[0]["fail"].ToString(), out  outmark))
                        {
                            arrercount = Convert.ToInt32(dv_stud[0]["fail"].ToString());
                        }
                        else
                        {
                            arrercount = 0;
                        }
                    }
                    else
                    {
                        arrercount = 0;
                    }

                    //if (arrercount >= fromrange && arrercount <= torange)
                    //{
                    //}
                    // DataSet dsmarkss = new DataSet();

                    // string markob = da.GetFunction("select distinct rt.marks_obtained as mark from registration r, applyn a , result rt,exam_type et, subject s where  a.app_no=r.app_no and r.sections = et.sections  and    et.subject_no=s.subject_no  and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0      and s.subject_no= '" + subjectno + "' and r.roll_no='" + roll_no + "'");
                    string markob = "select distinct rt.marks_obtained as mark from registration r, applyn a , result rt,exam_type et, subject s where  a.app_no=r.app_no and LTRIM(RTRIM(ISNULL(r.sections,''))) = LTRIM(RTRIM(ISNULL(et.sections,'')))  and    et.subject_no=s.subject_no  and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0      and s.subject_no= '" + subjectno + "' and r.roll_no='" + roll_no + "'";
                    dsmarkss.Clear();
                    dsmarkss = da.select_method_wo_parameter(markob, "Text");
                    if (dsmarkss.Tables[0].Rows.Count > 0)
                    {
                        markob = dsmarkss.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        markob = "";
                    }
                    if (markob != "" && markob != null)
                    {
                        markget = Convert.ToDouble(markob);
                        if (markget < 0)
                        {

                            if (norange == false)
                            {
                                markob = loadmarkat(markob);
                                

                                dtl.Rows[i + 2][Convert.ToInt32(colno)] = markob;
                            }
                            else
                            {
                                if (arrercount >= fromrange && arrercount <= torange)
                                {
                                    markob = loadmarkat(markob);
                                    
                                    dtl.Rows[i + 2][Convert.ToInt32(colno)] = markob;
                                }
                            }
                        }
                        else
                        {
                            if (norange == false)
                            {
                                

                                dtl.Rows[i + 2][Convert.ToInt32(colno)] = markob;
                            }
                            else
                            {
                                if (arrercount >= fromrange && arrercount <= torange)
                                {
                                    // markob = loadmarkat(markob);
                                    

                                    dtl.Rows[i + 2][Convert.ToInt32(colno)] = markob;
                                }
                            }
                        }
                    }
                    else
                    {

                        

                        dtl.Rows[i + 2][Convert.ToInt32(colno)] = " ";

                    }

                    double marksd = 0;
                    if (double.TryParse(dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString(), out marksd))
                    {
                        marksd = Convert.ToDouble(dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString());
                        if (marksd == 50)
                        {
                        }
                        if (minmark <= marksd)
                        {
                            //FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(colno)].Text = "";

                        }
                        else
                        {
                            

                            dtl.Rows[i + 2][Convert.ToInt32(colno)] = dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString() + "$Red";
                        }

                    }
                    else
                    {
                        if (dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString().ToUpper() == "EL" || dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString().ToUpper() == "NJ")
                        {

                        }
                        else
                            if (dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString().Trim() == "")
                            {
                            }
                            else
                            {
                                

                                dtl.Rows[i + 2][Convert.ToInt32(colno)] = dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString() + "#PaleVioletRed";
                            }

                    }
                    //if (Chktotinternal.Checked == true)
                    //{
                    //    int subcount = 0;
                    //    string selsub;
                    //    if (Convert.ToInt32(cblsub.SelectedValue.Count()) > 0)
                    //    {
                    //        selsub = rs.getCblSelectedValue(cblsub);
                    //        //for (int k = 0; k < cblsub.Items.Count; k++)
                    //        //{
                    //        //    if (cbltest.Items[k].Selected == true)
                    //        //    {
                    //        //        subcount++;
                    //        //    }
                    //        //}
                    //        //int subcount = cblsub.SelectedValue.Count();
                    //        foreach (ListItem item in cblsub.Items)
                    //        {
                    //            if (item.Selected)
                    //            {
                    //                subcount = Items.Count;
                    //            }
                    //        }
                    //        string totmark = "select distinct SUM (rt.marks_obtained )as mark,count(s.subject_no)from registration r, applyn a , result rt,exam_type et, subject s where a.app_no=r.app_no  and  et.subject_no=s.subject_no  and RollNo_Flag<>0 and et.exam_code=rt.exam_code and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and s.subject_no in( '" + selsub + "') and r.roll_no='" + roll_no + "' group by s.subject_no";

                    //        DataSet totinternal = new DataSet();
                    //        totinternal.Clear();
                    //        totinternal = da.select_method_wo_parameter(totmark, "text");
                    //        if (totinternal.Tables[0].Rows.Count > 0)
                    //        {
                    //            totmark = totinternal.Tables[0].Rows[0][0].ToString();
                    //            int internaltotmark = Convert.ToInt32(totmark) / subcount;
                    //            string totalmark = Convert.ToString(internaltotmark);
                    //            FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(colno) + 1].Text = totalmark;
                    //        }
                    //    }
                    
                }
            }


                
            




            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Stud Pass in Each Subject";
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Stud Fail in Each Subject";
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "Range : 50 to 60";
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "Range : 61 to 70";
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "Range : 71 to 80";
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            dtl.Rows[dtl.Rows.Count - 1][0] = "Range : 81 to 90";
            
            for (int j = 0; j < dtforbindvalues.Rows.Count; j++)
            {
                passcount = 0;
                failcount = 0;
                if (Int32.TryParse(dtforbindvalues.Rows[j][3].ToString(), out outmark))
                {
                    minmark = Convert.ToInt32(dtforbindvalues.Rows[j][3].ToString());
                }
                colno = dtforbindvalues.Rows[j][0].ToString();
                criteriano = dtforbindvalues.Rows[j][1].ToString();
                subjectno = dtforbindvalues.Rows[j][2].ToString();
                for (int i = 0; i < dtl.Rows.Count - 8; i++)
                {
                    double marksd = 0;
                    string value2 = dtl.Rows[i + 2][Convert.ToInt32(colno)].ToString();
                    string[] splitval6 = value2.Split('$');
                    string[] splitval2 = value2.Split('#');
                    if (splitval6.Length > 1 || splitval2.Length > 1)
                    {
                        if (splitval6.Length > 1)
                        {
                            value2 = splitval6[0].ToString();
                        }
                        else if (splitval2.Length > 1)
                        {
                            value2 = splitval2[0].ToString();

                        }


                    }
                    if (double.TryParse(value2.ToString(), out marksd))
                    {
                        
                        marksd = Convert.ToDouble(value2);//rrrr
                        
                        if (minmark <= marksd)
                        {
                            passcount++;
                        }
                        else
                        {
                            failcount++;
                        }

                    }
                    else
                    {
                        if (value2.ToString().ToUpper() == "NJ")
                        {

                        }
                        else
                            if (value2.ToString().ToUpper() == "EL")
                            {
                                passcount++;
                            }
                            else
                            {
                                failcount++;
                            }
                    }
                }

                dtl.Rows[dtl.Rows.Count - 6][Convert.ToInt32(colno)] = Convert.ToString(passcount);

                dtl.Rows[dtl.Rows.Count - 5][Convert.ToInt32(colno)] = Convert.ToString(failcount);

               

                string range1 = da.GetFunction("select distinct count(r.Roll_No) as count11 from registration r, applyn a , result rt,exam_type et, subject s where   a.app_no=r.app_no and LTRIM(RTRIM(ISNULL(r.sections,''))) = LTRIM(RTRIM(ISNULL(et.sections,''))) and r.degree_code='" + degreecode + "' and    et.subject_no=s.subject_no and r.batch_year='" + batchyear + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0       and s.subject_no= '" + subjectno + "' " + final11 + " and  rt.marks_obtained between 50 and 60");
                int markran = 0;
                if (Int32.TryParse(range1, out markran))
                {
                    markran = Convert.ToInt32(range1);
                    
                    dtl.Rows[dtl.Rows.Count - 4][Convert.ToInt32(colno)] = Convert.ToString(markran);
                }
                else
                {
                    

                    dtl.Rows[dtl.Rows.Count - 4][Convert.ToInt32(colno)] = "0";
                }

                range1 = da.GetFunction("select distinct count(r.Roll_No) as count11 from registration r, applyn a , result rt,exam_type et, subject s where   a.app_no=r.app_no and LTRIM(RTRIM(ISNULL(r.sections,''))) = LTRIM(RTRIM(ISNULL(et.sections,''))) and r.degree_code='" + degreecode + "' and    et.subject_no=s.subject_no and r.batch_year='" + batchyear + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0       and s.subject_no= '" + subjectno + "' " + final11 + " and  rt.marks_obtained between 61 and 70");
                markran = 0;
                if (Int32.TryParse(range1, out markran))
                {
                    markran = Convert.ToInt32(range1);
                    

                    dtl.Rows[dtl.Rows.Count - 3][Convert.ToInt32(colno)] = Convert.ToString(markran);

                }
                else
                {
                    

                    dtl.Rows[dtl.Rows.Count - 3][Convert.ToInt32(colno)] = "0";
                }

                range1 = da.GetFunction("select distinct count(r.Roll_No) as count11 from registration r, applyn a , result rt,exam_type et, subject s where   a.app_no=r.app_no and LTRIM(RTRIM(ISNULL(r.sections,''))) =LTRIM(RTRIM(ISNULL(et.sections,''))) and r.degree_code='" + degreecode + "' and    et.subject_no=s.subject_no and r.batch_year='" + batchyear + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0       and s.subject_no= '" + subjectno + "' " + final11 + " and  rt.marks_obtained between 71 and 80");
                markran = 0;
                if (Int32.TryParse(range1, out markran))
                {
                    markran = Convert.ToInt32(range1);
                    

                    dtl.Rows[dtl.Rows.Count - 2][Convert.ToInt32(colno)] = Convert.ToString(markran);

                }
                else
                {
                    

                    dtl.Rows[dtl.Rows.Count - 2][Convert.ToInt32(colno)] = "0";
                }


                range1 = da.GetFunction("select distinct count(r.Roll_No) as count11 from registration r, applyn a , result rt,exam_type et, subject s where   a.app_no=r.app_no and LTRIM(RTRIM(ISNULL(r.sections,''))) = LTRIM(RTRIM(ISNULL(et.sections,''))) and r.degree_code='" + degreecode + "' and    et.subject_no=s.subject_no and r.batch_year='" + batchyear + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and    et.criteria_no ='" + criteriano + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0       and s.subject_no= '" + subjectno + "' " + final11 + " and  rt.marks_obtained between 81 and 90");
                markran = 0;
                if (Int32.TryParse(range1, out markran))
                {
                    markran = Convert.ToInt32(range1);
                    
                    dtl.Rows[dtl.Rows.Count - 1][Convert.ToInt32(colno)] = Convert.ToString(markran);
                }
                else
                {
                    
                    dtl.Rows[dtl.Rows.Count - 1][Convert.ToInt32(colno)] = "0";
                }
            }


        }
        catch
        {
        }
    }
    public string loadmarkat(string mr)
    {
        string strgetval = "";
        if (mr == "-1")
        {
            strgetval = "AAA";
        }
        else if (mr == "-2")
        {
            strgetval = "EL";
        }
        else if (mr == "-3")
        {
            strgetval = "EOD";
        }
        else if (mr == "-4")
        {
            strgetval = "ML";
        }
        else if (mr == "-5")
        {
            strgetval = "SOD";
        }
        else if (mr == "-6")
        {
            strgetval = "NSS";
        }
        else if (mr == "-7")
        {
            strgetval = "NJ";
        }
        else if (mr == "-8")
        {
            strgetval = "S";
        }
        else if (mr == "-9")
        {
            strgetval = "L";
        }
        else if (mr == "-10")
        {
            strgetval = "NCC";
        }
        else if (mr == "-11")
        {
            strgetval = "HS";
        }
        else if (mr == "-12")
        {
            strgetval = "PP";
        }
        else if (mr == "-13")
        {
            strgetval = "SYOD";
        }
        else if (mr == "-14")
        {
            strgetval = "COD";
        }
        else if (mr == "-15")
        {
            strgetval = "OOD";
        }
        else if (mr == "-16")
        {
            strgetval = "OD";
        }
        else if (mr == "-17")
        {
            strgetval = "LA";
        }
        else if (mr == "-18")
        {
            strgetval = "RAA";
        }
        return strgetval;
    }

    public void bindfpheader()
    {

        ArrayList subjectseleceted = new ArrayList();
        ArrayList testseleceted = new ArrayList();

        for (int i = 0; i < cblsub.Items.Count; i++)
        {
            if (cblsub.Items[i].Selected == true)
            {
                subjectseleceted.Add(cblsub.Items[i].Value.ToString());
            }
        }
        for (int i = 0; i < cbltest.Items.Count; i++)
        {
            if (cbltest.Items[i].Selected == true)
            {
                testseleceted.Add(cbltest.Items[i].Value.ToString());
            }
        }
        testcount=testseleceted.Count;
        subcount= subjectseleceted.Count;
        //string seltest = "";
        int startcol = 0;
        string colheader = "";  

        if (rb_testWise.Selected)
        {
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                if (cbltest.Items[i].Selected == true)
                {

                    ds.Clear();
                    ds = da.select_method_wo_parameter("select distinct  s.subject_code, s.subject_no,s.subject_name,s.acronym from exam_type e,subject s,result r  where e.subject_no=s.subject_no and e.exam_code= r.exam_code and criteria_no='" + cbltest.Items[i].Value.ToString() + "' ", "Text");

                    if (ds.Tables[0].Rows.Count > 0 && subjectseleceted.Count > 0)
                    {
                        dtl.Columns.Add(cbltest.Items[i].Text.ToString(), typeof(string));
                        dtl.Rows[0][dtl.Columns.Count - 1] = cbltest.Items[i].Text.ToString();
                        

                        startcol = dtl.Columns.Count - 1;
                        headcolspan.Add(cbltest.Items[i].Text.ToString() + "^" + subjectseleceted.Count);
                        
                        int focol = 0;
                                                    
                        for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                        {
                            colheader = colheader + " ";
                            if (subjectseleceted.Contains(ds.Tables[0].Rows[s]["subject_no"].ToString()))
                            {
                                //FpSpread1.Sheets[0].ColumnCount++;
                                 focol++;  //modified by mullai on 28/3/2018

                                 dtl.Columns[dtl.Columns.Count - 1].ColumnName = ds.Tables[0].Rows[s]["subject_code"].ToString() + colheader;
                                 dtl.Rows[1][dtl.Columns.Count - 1] = ds.Tables[0].Rows[s]["subject_code"].ToString() + colheader;
                                
                               
                                string minmarksql = d2.GetFunction(" select min_mark from  exam_type where subject_no='" + ds.Tables[0].Rows[s]["subject_no"].ToString() + "' and criteria_no='" + cbltest.Items[i].Value.ToString() + "'");
                                string maxmarksql = d2.GetFunction(" select max_mark from  exam_type where subject_no='" + ds.Tables[0].Rows[s]["subject_no"].ToString() + "' and criteria_no='" + cbltest.Items[i].Value.ToString() + "'");

                                dtforbindvalues.Rows.Add(dtl.Columns.Count - 1, cbltest.Items[i].Value.ToString(), ds.Tables[0].Rows[s]["subject_no"].ToString(), minmarksql, maxmarksql);
                          
                               

                               dtl.Columns.Add("", typeof(string));
                            }
                           
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount + 1].Text = "Total";
                        }



                        int colcount = dtl.Columns.Count;

                        
                      // FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, startcol, 1, colcount);
                        

                        //dtl.Columns[dtl.Columns.Count - 1].ColumnName.Remove(dtl.Columns.Count - 1);

                        dtl.Columns.RemoveAt(dtl.Columns.Count - 1);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < subjectseleceted.Count; i++)
            {
                // bool boolStud = false;
                ds.Clear();
                ds = da.select_method_wo_parameter("select distinct  s.subject_code, s.subject_no,s.subject_name,s.acronym from exam_type e,subject s,result r  where e.subject_no=s.subject_no and e.exam_code= r.exam_code and s.subject_no='" + subjectseleceted[i].ToString() + "' ", "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dtl.Columns.Add(ds.Tables[0].Rows[0]["subject_code"].ToString(), typeof(string));
                    dtl.Rows[0][dtl.Columns.Count - 1] = ds.Tables[0].Rows[0]["subject_code"].ToString();
                    
                    startcol = dtl.Columns.Count - 1;
                    int fpcolum = startcol;   //modified by mullai on 28/3/2018
                    headcolspan.Add(ds.Tables[0].Rows[0]["subject_code"].ToString());
                    

                    
                    int f = 0;
                    for (int j = 0; j < cbltest.Items.Count; j++)
                    {
                     
                        if (cbltest.Items[j].Selected == true)
                        {
                            colheader = colheader + " ";
                            if (j != 0)
                            {

                                
                                dtl.Columns.Add("", typeof(string));
                            }
                                

                                dtl.Columns[startcol].ColumnName = cbltest.Items[j].Text.ToString() + colheader;
                                dtl.Rows[1][startcol] = cbltest.Items[j].Text.ToString() + colheader;
                        
                            f++;
                            string minmarksql = d2.GetFunction(" select min_mark from  exam_type where subject_no='" + ds.Tables[0].Rows[0]["subject_no"].ToString() + "' and criteria_no='" + cbltest.Items[j].Value.ToString() + "'");
                            string maxmarksql = d2.GetFunction(" select max_mark from  exam_type where subject_no='" + ds.Tables[0].Rows[0]["subject_no"].ToString() + "' and criteria_no='" + cbltest.Items[j].Value.ToString() + "'");
                            dtforbindvalues.Rows.Add(startcol, cbltest.Items[j].Value.ToString(), ds.Tables[0].Rows[0]["subject_no"].ToString(), minmarksql, maxmarksql);
                            // boolStud = true;
                           
                            startcol++;
                        }

                    }
                }
            }

        }

        
       

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                lblnorec.Visible = false;

                d2.printexcelreportgrid(Showgrid, reportname);
                txtexcelname.Text = string.Empty;
            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
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
            Printcontrol.Visible = true;
            lblnorec.Text = "";


            // string date_filt = "From : " + tbstart_date.Text.ToString() + "   " + "To : " + tbend_date.Text.ToString();
            string degreeset = da.GetFunction("select (Course_Name+' - '+Acronym) as degreeset from course c, degree d where c.Course_Id=d.Course_Id and Degree_Code='" + ddstandard.SelectedItem.Value.ToString() + "'");
            degreeset = degreeset + " - " + ddlSemYr.SelectedItem.Text.ToString();
            string strsec = "";
            if (ddlSec.Enabled == true)
            {
                string sections = ddlSec.SelectedItem.Text.ToString();
                if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " - " + sections.ToString();
                }

            }
            degreeset = degreeset + strsec;

            int batchyear = Convert.ToInt32(dropyear.SelectedItem.Text.ToString());

            string date_filt = "Batch : " + batchyear;

            date_filt = date_filt + "@" + "Degree : " + degreeset;
            string degreedetails = string.Empty;

            degreedetails = "Subjectwise Multiple Test Result Report" + "@" + date_filt;
            string pagename = "subjectwisemultitest.aspx";
            //Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            string ss = null;
            Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }

    }

    public void bindschooltype()
    {
        try
        {
            ddschooltype.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_degree", hat, "sp");
            if (studgradeds.Tables[0].Rows.Count > 0)
            {
                ddschooltype.DataSource = studgradeds;
                ddschooltype.DataTextField = "course_name";
                ddschooltype.DataValueField = "course_id";
                ddschooltype.DataBind();
            }
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    public void hide()
    {
        Printcontrol.Visible = false;
        Showgrid.Visible = false;
        final.Visible = false;


    }

    protected void dropyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindschooltype();
            bindstandard();
            lblerrormsg.Text = "";
            txtrngfrom.Text = "";
            txtrngto.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {

        }
    }
    public void bindstandard()
    {
        try
        {
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddschooltype.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_branch", hat, "sp");
            if (studgradeds.Tables[0].Rows.Count > 0)
            {
                ddstandard.DataSource = studgradeds;
                ddstandard.DataTextField = "dept_name";
                ddstandard.DataValueField = "degree_code";
                ddstandard.DataBind();
                bindsem();

            }
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }
    protected void dropschooltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            bindstandard();
            lblerrormsg.Text = "";
            txtrngfrom.Text = "";
            txtrngto.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {

        }
    }
    protected void ddstandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            lblerrormsg.Text = "";
            txtrngfrom.Text = "";
            txtrngto.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Text = "";
        txtrngfrom.Text = "";
        txtrngto.Text = "";
        hide();
        lblerrormsg.Visible = true;
        return;
    }
    protected void ddlhost_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Text = "";
        txtrngfrom.Text = "";
        txtrngto.Text = "";
        hide();
        lblerrormsg.Visible = true;
        return;
    }

    protected void ddlgender_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Text = "";
        txtrngfrom.Text = "";
        txtrngto.Text = "";
        hide();
        lblerrormsg.Visible = true;
        return;
    }

    public void bindyear()
    {
        try
        {
            dropyear.Items.Clear();
            studgradeds.Clear();
            studgradeds = d2.select_method_wo_parameter("bind_batch", "sp");

            int count = studgradeds.Tables[0].Rows.Count;
            if (count > 0)
            {
                dropyear.DataSource = studgradeds;
                dropyear.DataTextField = "batch_year";
                dropyear.DataValueField = "batch_year";
                dropyear.DataBind();
                dropyear.SelectedIndex = dropyear.Items.Count - 1;
            }

        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    protected void cbltest_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //hide();
            int cout = 0;
            cbtest.Checked = false;
            txttest.Text = "--Select--";
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                if (cbltest.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                txttest.Text = "Test(" + cout + ")";
                if (cout == cbltest.Items.Count)
                {
                    cbtest.Checked = true;
                }
            }
            bindsubject();
            lblerrormsg.Text = "";
            txtrngfrom.Text = "";
            txtrngto.Text = "";
            hide();
            lblerrormsg.Visible = true;

        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }


    protected void cbtest_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            // hide();
            if (cbtest.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < cbltest.Items.Count; i++)
                {
                    cout++;
                    cbltest.Items[i].Selected = true;
                    cbtest.Checked = true;
                    txttest.Text = "Test(" + cout + ")";
                }
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cbltest.Items.Count; i++)
                {
                    cout++;
                    cbltest.Items[i].Selected = false;
                    txttest.Text = "-Select-";
                    cbtest.Checked = false;
                }
            }
            bindsubject();
            lblerrormsg.Text = "";
            txtrngfrom.Text = "";
            txtrngto.Text = "";
            hide();
            lblerrormsg.Visible = true;

        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    public void bindtest()
    { 
        cbtest.Checked = false;
        txttest.Text = "--Select--";
       string batchyear = Convert.ToString(dropyear.SelectedItem.Value).Trim();
        string degreecode = ddstandard.SelectedItem.Value.ToString();
        string sem = ddlSemYr.SelectedItem.Text.ToString().Trim();
        ds.Clear();
        string sylyear = d2.GetFunction(" select syllabus_year from syllabus_master where degree_code='" + degreecode + "' and semester ='" + sem + "' and batch_year='" + batchyear + "'");
        string strtest = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + degreecode + "' and semester ='" + sem + "' and batch_year='" + batchyear + "' and syllabus_year='" + sylyear + "' order by criteria ";

        ds = d2.select_method_wo_parameter(strtest, "Text");
        cbltest.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbltest.DataSource = ds;
            cbltest.DataTextField = "criteria";
            cbltest.DataValueField = "criteria_no";
            cbltest.DataBind();

        }
        bindsubject();
    }

    protected void cblsub_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //hide();
            int cout = 0;
            cbsub.Checked = false;
            txtsub.Text = "--Select--";
            for (int i = 0; i < cblsub.Items.Count; i++)
            {
                if (cblsub.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                txtsub.Text = "Subject(" + cout + ")";
                if (cout == cblsub.Items.Count)
                {
                    cbsub.Checked = true;
                }
            }

            lblerrormsg.Text = "";
            txtrngfrom.Text = "";
            txtrngto.Text = "";
            hide();
            lblerrormsg.Visible = true;

        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }


    protected void cbsub_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            // hide();
            if (cbsub.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < cblsub.Items.Count; i++)
                {
                    cout++;
                    cblsub.Items[i].Selected = true;
                    cbsub.Checked = true;
                    txtsub.Text = "Subject(" + cout + ")";
                }
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cblsub.Items.Count; i++)
                {
                    cout++;
                    cblsub.Items[i].Selected = false;
                    txtsub.Text = "-Select-";
                    cbsub.Checked = false;
                }
            }
            lblerrormsg.Text = "";
            txtrngfrom.Text = "";
            txtrngto.Text = "";
            hide();
            lblerrormsg.Visible = true;

        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    public void bindsubject()
    {
        cbsub.Checked = false;
        txtsub.Text = "--Select--";
        string cretianum = "";
        for (int i = 0; i < cbltest.Items.Count; i++)
        {
            if (cbltest.Items[i].Selected == true)
            {
                if (cretianum == "")
                {
                    cretianum = cbltest.Items[i].Value.ToString();
                }
                else
                {
                    cretianum = cretianum + "','" + cbltest.Items[i].Value.ToString();
                }
            }
        }
        //string batchyear = dropyear.SelectedItem.Text.ToString();
        //string degreecode = ddstandard.SelectedItem.Value.ToString();
        //string sem = ddlSemYr.SelectedItem.Text.ToString().Trim();
        //ds.Clear();
        //string sylyear = d2.GetFunction(" select syllabus_year from syllabus_master where degree_code='" + degreecode + "' and semester ='" + sem + "' and batch_year='" + batchyear + "'");
        //string strtest = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + degreecode + "' and semester ='" + sem + "' and batch_year='" + batchyear + "' and syllabus_year='" + sylyear + "' order by criteria ";
        cblsub.Items.Clear();
        if (cretianum.Trim() != "")
        {
            ds.Clear();
            ds = da.select_method_wo_parameter("select distinct   s.subject_no,s.subject_name from exam_type e,subject s,result r  where e.subject_no=s.subject_no and e.exam_code= r.exam_code and criteria_no in ('" + cretianum + "') ", "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cblsub.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblsub.DataSource = ds;
                    cblsub.DataTextField = "subject_name";
                    cblsub.DataValueField = "subject_no";
                    cblsub.DataBind();

                }
            }
        }


    }
    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindtest();
        BindSectionDetail();
        lblerrormsg.Text = "";
        txtrngfrom.Text = "";
        txtrngto.Text = "";
        hide();
        lblerrormsg.Visible = true;
        return;
    }

    public void BindSectionDetail()
    {

        string branch = ddstandard.SelectedValue.ToString();
        string batch = dropyear.SelectedValue.ToString();

        string sqlnew = "select distinct LTRIM(RTRIM(ISNULL(sections,''))) sections from registration where batch_year=" + dropyear.SelectedValue.ToString() + " and degree_code=" + ddstandard.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
        DataSet ds = new DataSet();
        ds.Clear();
        ds = d2.select_method_wo_parameter(sqlnew, "Text");

        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        ddlSec.Items.Insert(0, "All");
        //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));

        if (ds.Tables[0].Rows.Count > 0)
        {

            ddlSec.Enabled = true;

        }
        else
        {
            ddlSec.Enabled = false;

        }

    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Text = "";
        txtrngfrom.Text = "";
        txtrngto.Text = "";
        hide();
        lblerrormsg.Visible = true;
        return;
    }

    public void bindsem()
    {

        //--------------------semester load
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;

        string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddstandard.SelectedValue.ToString() + " and batch_year=" + dropyear.Text.ToString() + " and college_code=" + Session["collegecode"] + "";
        DataSet ds = new DataSet();
        ds.Clear();
        ds = d2.select_method_wo_parameter(sqlnew, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(i.ToString());
                    //ddlSemYr.Enabled = false;
                }
                else if (first_year == true && i == 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }

            }
        }
        else
        {


            sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddstandard.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "";

            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
            }


        }
        if (ddlSemYr.Items.Count > 0)
        {
            ddlSemYr.SelectedIndex = 0;

            BindSectionDetail();
            bindtest();
        }


    }

    protected void rbmark_CheckedChanged(object sender, EventArgs e)
    {
        lblrngfrom.Text = "Mark Range";
        lblerrormsg.Text = "";
        txtrngfrom.Text = "";
        txtrngto.Text = "";
        hide();
        lblerrormsg.Visible = true;
        return;
    }


    protected void rbarrear_CheckedChanged(object sender, EventArgs e)
    {
        lblrngfrom.Text = "Fail";
        lblerrormsg.Text = "";
        txtrngfrom.Text = "";
        txtrngto.Text = "";
        hide();
        lblerrormsg.Visible = true;
        return;
    }

    
    public override void VerifyRenderingInServerForm(Control control)
    { }

}