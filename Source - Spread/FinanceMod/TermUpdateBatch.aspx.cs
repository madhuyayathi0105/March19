using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Text;
using System.Configuration;

public partial class TermUpdateBatch : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods reuse = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
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
                loadcollege();
                bindBtch();
            }
            if (ddlclg.Items.Count > 0)
                collegecode = Convert.ToString(ddlclg.SelectedItem.Value);
        }
        catch (Exception ex) { }
    }

    public void loadcollege()
    {
        ddlclg.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlclg);
    }
    public void bindBtch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch { }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            bool boolCheck = false;
            string[] strTerm = new string[0];
            ArrayList arTerm = new ArrayList();
            StringBuilder sbTerm = new StringBuilder();
            string batch = string.Empty;
            if (ddlclg.Items.Count > 0)
                collegecode = Convert.ToString(ddlclg.SelectedItem.Value);
            if (ddlbatch.Items.Count > 0)
                batch = Convert.ToString(ddlbatch.SelectedItem.Value);
            Dictionary<string, string> dtTerm = loadTerm(collegecode);
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && dtTerm.Count > 0)
            {
                foreach (KeyValuePair<string, string> termVal in dtTerm)
                {
                    string termStr = Convert.ToString(termVal.Key);
                    string termCode = Convert.ToString(termVal.Value);
                    string selQ = " select distinct feecategory,textcode,textval,finyearfk from ft_feeallot f,textvaltable t,registration r where f.app_no=r.app_no and f.feecategory=t.textcode and t.textcriteria='FEECA' and t.college_code=r.college_code and r.college_code='" + collegecode + "' and r.batch_year='2016' and r.cc=0 and r.delflag=0 and exam_flag<>'Debar' and t.textval like'%" + termStr + "%' and t.textval<>'" + termStr + "'";
                    DataSet dsalt = new DataSet();
                    dsalt = d2.select_method_wo_parameter(selQ, "Text");
                    if (dsalt.Tables.Count > 0 && dsalt.Tables[0].Rows.Count > 0)
                    {
                        string fnlTerm = string.Empty;
                        for (int row = 0; row < dsalt.Tables[0].Rows.Count; row++)
                        {
                            string tempTerm = Convert.ToString(dsalt.Tables[0].Rows[row]["textcode"]);
                            if (!arTerm.Contains(tempTerm))
                            {
                                // sbTerm.Append(tempTerm + "',");
                                Array.Resize(ref strTerm, strTerm.Length + 1);
                                strTerm[strTerm.Length - 1] = tempTerm;
                                arTerm.Add(tempTerm);
                            }
                        }
                        //if (sbTerm.Length > 0)
                        //    sbTerm.Remove(sbTerm.Length - 1,1);
                        if (strTerm.Length > 0)
                            fnlTerm = string.Join("','", strTerm);
                        string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
                        string newfinfk = featureFinyear(Convert.ToString(DateTime.Now.Year));
                        if (!string.IsNullOrEmpty(fnlTerm) && !string.IsNullOrEmpty(finYearid) && finYearid != "0" && !string.IsNullOrEmpty(newfinfk) && newfinfk != "0")
                        {
                          boolCheck= updateAllot(fnlTerm, termCode, finYearid, newfinfk);
                        }
                        
                    }
                }
            }
            if (boolCheck)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('updated Successfully')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not updated')", true);
            }
        }
        catch { }
    }

    protected bool updateAllot(string oldtextcode, string newtextcode, string oldfinfk, string newfinfk)
    {
        bool boolCheck = false;
        try
        {
            string updQ = "update ft_feeallot set finyearfk='" + newfinfk + "',feecategory='" + newtextcode + "' where feecategory in('" + oldtextcode + "') and finyearfk='" + oldfinfk + "'";
            int upd = d2.update_method_wo_parameter(updQ, "Text");
            if (upd > 0)
            {
                string updTransQ = " update ft_findailytransaction set actualfinyearfk='" + newfinfk + "',feecategory='" + newtextcode + "' where feecategory in('" + oldtextcode + "') and finyearfk='" + oldfinfk + "'";
                int updtrs = d2.update_method_wo_parameter(updQ, "Text");
                boolCheck = true;
            }
        }
        catch { }
        return boolCheck;
    }

    protected string featureFinyear(string curyear)
    {
        string year = d2.GetFunction(" select finyearpk from fm_finyearmaster where collegecode='23' and datepart(year,finyearstart)='" + curyear + "'");
        return year;
    }

    protected Dictionary<string, string> loadTerm(string collegecode)
    {
        Dictionary<string, string> dtTerm = new Dictionary<string, string>();
        try
        {
            string selQ = " select  distinct  textval,textcode,len(isnull(textval,1000)) from textvaltable t where college_code='" + collegecode + "' and textcriteria='FEECA' and( textval like'Term 1%' or textval like'Term 2%' or textval like'Term 3%' or textval like'Term 4%' or textval like'Term 5%' or textval like'Term 6%') order by len(isnull(textval,1000)),textval asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    dtTerm.Add(Convert.ToString(ds.Tables[0].Rows[row]["textval"]), Convert.ToString(ds.Tables[0].Rows[row]["textcode"]));
                }
            }
        }
        catch { dtTerm.Clear(); }
        return dtTerm;
    }

    protected void btnmode_Click(object sender, EventArgs e)
    {
        bool boolCheck = false;
        string year = Convert.ToString(DateTime.Now.Year);
        string update = "  update registration set mode='1' where atepart(year,adm_date)<'" + year + "'";
        int upd = d2.update_method_wo_parameter(update, "Text");
        if (upd > 0)
        {
            update = "  update registration set mode='3' where atepart(year,adm_date)='" + year + "'";
            upd = d2.update_method_wo_parameter(update, "Text");
            boolCheck = true;
        }
        if (boolCheck)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('updated Successfully')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not updated')", true);
        }
    }
}