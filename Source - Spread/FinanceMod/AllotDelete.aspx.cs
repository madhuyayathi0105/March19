using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Configuration;

public partial class AllotDelete : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
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
            if (!IsPostBack)
            {
                bindBtch();
            }
        }
        catch (Exception ex) { }
    }
    public void bindBtch()
    {
        try
        {
            ddlyear.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlyear.DataSource = ds;
                ddlyear.DataTextField = "batch_year";
                ddlyear.DataValueField = "batch_year";
                ddlyear.DataBind();
            }
        }
        catch { }
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        try
        {
            string batch = Convert.ToString(ddlyear.SelectedItem.Value);
            if (!string.IsNullOrEmpty(batch))
            {
                string selQ = "     select sum(totalamount) as tot,sum(paidamount) as paid,sum(balamount) as bal,feecategory,ledgerfk,f.app_no,degree_code,batch_year from ft_feeallot f ,registration r where r.app_no=f.app_no  and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0 and r.batch_year in('" + batch + "')  and r.college_code='13' and f.headerfk in ('9','11','12','13','14','15','16','17','36') and f.ledgerFK in('151','152','153','154','232','157','158','159','160','179','161','162','163','164','165','166','167','168','169','170','171','172','173','174','175','176','177','178','181','231','182','183','184','185','186','187','188','189','190','191','192','193','194','195','196','197','198','199','200','201','202','203','605','204','205','206','207','208','209','210','211','212','213','156','214','215','228','229','234','482','230','233','224','223','226','222','227','221','220','225','219','218','216','217','237','236','238','235','627') and f.app_no='13875'  group by feecategory,ledgerfk,f.app_no,degree_code,batch_year having sum(isnull(totalamount,'0')) =sum(isnull(paidamount,'0')) and sum(isnull(totalamount,'0')) =sum(isnull(balamount,'0')) and sum(isnull(totalamount,'0'))<>'0'";
                selQ += "   select sum(debit) as tot,feecategory,ledgerfk,f.app_no,degree_code,batch_year from ft_findailytransaction f,registration r where r.app_no=f.app_no  and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0 and r.batch_year in('" + batch + "')  and r.college_code='13' and f.headerfk in ('9','11','12','13','14','15','16','17','36') and f.ledgerFK in('151','152','153','154','232','157','158','159','160','179','161','162','163','164','165','166','167','168','169','170','171','172','173','174','175','176','177','178','181','231','182','183','184','185','186','187','188','189','190','191','192','193','194','195','196','197','198','199','200','201','202','203','605','204','205','206','207','208','209','210','211','212','213','156','214','215','228','229','234','482','230','233','224','223','226','222','227','221','220','225','219','218','216','217','237','236','238','235','627') and isnull(transcode,'')<>'' and isnull(iscanceled,'0')='0' and memtype='1'  and f.app_no='13875' group by feecategory,ledgerfk,f.app_no,degree_code,batch_year having sum(isnull(debit,'0'))<>'0' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    bool check = false;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "app_no='" + ds.Tables[0].Rows[row]["app_no"] + "' and feecategory='" + ds.Tables[0].Rows[row]["feecategory"] + "' and ledgerfk='" + ds.Tables[0].Rows[row]["ledgerfk"] + "' and degree_code='" + ds.Tables[0].Rows[row]["degree_code"] + "' and batch_year='" + ds.Tables[0].Rows[row]["batch_year"] + "'";
                            DataView dv = ds.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                string updQ = "   update ft_feeallot set balamount='0' where app_no='" + ds.Tables[0].Rows[row]["app_no"] + "' and feecategory='" + ds.Tables[0].Rows[row]["feecategory"] + "' and ledgerfk='" + ds.Tables[0].Rows[row]["ledgerfk"] + "'";
                                int upd = d2.update_method_wo_parameter(updQ, "Text");
                                check = true;
                            }
                        }
                    }
                    if (check)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('updated Successfully')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not updated')", true);
                    }
                }
            }
        }
        catch { }
    }
}