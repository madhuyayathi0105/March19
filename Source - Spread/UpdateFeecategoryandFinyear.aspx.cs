using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class UpdateFeecategoryandFinyear : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCollege();
            BindFinYear();
        }
        ErrorLbl.Visible = false;
    }
    public void BindCollege()
    {
        ddlCollege.Items.Clear();
        ds = d2.BindCollege();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlCollege.DataSource = ds;
            ddlCollege.DataTextField = "collname";
            ddlCollege.DataValueField = "college_code";
            ddlCollege.DataBind();
        }

    }
    public void BindFinYear()
    {
        ddlFinyear.Items.Clear();
        ds = d2.select_method_wo_parameter("select FinyearPK,convert(varchar, cast(Year(FinYearStart)as varchar)+' - '+cast( Year(FinYearEnd) as varchar)) as FinayearName from Fm_finyearMAster where collegeCode ='" + ddlCollege.SelectedValue + "'", "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlFinyear.DataSource = ds;
            ddlFinyear.DataTextField = "FinayearName";
            ddlFinyear.DataValueField = "FinyearPK";
            ddlFinyear.DataBind();
        }

    }

    protected void ddlcollegeChange_NEW(object sender, EventArgs e)
    {
        BindFinYear();
    }
    protected void btnFeeCodeClike(object sender, EventArgs e)
    {
        UpdateFeecode();
    }
    public void UpdateFeecode()
    {
        try
        {
            string Msg = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                string Query = "select * from TextValtable where college_code='" + ddlCollege.SelectedValue + "' and TextCriteria='FEECA' and TextVal like '%Term%'";
                Query += " select * from TextValtable where college_code='" + ddlCollege.SelectedValue + "' and TextCriteria='FEECA' and (TextVal like 'Term 1%' or TextVal like 'Term 2%' or TextVal like 'Term 3%' or TextVal like 'Term 4%')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(Query, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int Trow = 0; Trow < ds.Tables[1].Rows.Count; Trow++)
                    {
                        StringBuilder FTextCode = new StringBuilder();
                        ds.Tables[0].DefaultView.RowFilter = "TextVal like '%" + Convert.ToString(ds.Tables[1].Rows[Trow]["TextVal"]) + "%' and TextCode<>'" + Convert.ToString(ds.Tables[1].Rows[Trow]["TextCode"]) + "'";
                        DataTable DtTrem = ds.Tables[0].DefaultView.ToTable();
                        if (DtTrem.Rows.Count > 0)
                        {
                            for (int InTerm = 0; InTerm < DtTrem.Rows.Count; InTerm++)
                            {
                                FTextCode.Append(DtTrem.Rows[InTerm]["TextCode"] + ",");
                            }
                            if (FTextCode.Length > 0)
                            {
                                FTextCode = FTextCode.Remove(FTextCode.Length - 1, 1);
                            }
                        }
                        if (FTextCode.Length > 0)
                        {
                            string UpQuery = "update Ft_Feeallot set FinyearFk= '" + ddlFinyear.SelectedValue + "' ,Feecategory='" + Convert.ToString(ds.Tables[1].Rows[Trow]["TextCode"]) + "' where Feecategory in (" + FTextCode.ToString() + ")";
                            UpQuery += " update ft_findailytransaction set ActualFinYearFk= '" + ddlFinyear.SelectedValue + "' ,Feecategory='" + Convert.ToString(ds.Tables[1].Rows[Trow]["TextCode"]) + "' where Feecategory in (" + FTextCode.ToString() + ")";

                            int Up = d2.update_method_wo_parameter(UpQuery, "Text");
                            if (Up > 0)
                            {
                                Msg = "Updated Sucessfully";
                            }
                        }
                    }
                }
                if (Msg.Trim() == "")
                {
                    Msg = "Not Update";
                }
                ErrorLbl.Text = Msg;
                ErrorLbl.Visible = true;
            }
        }
        catch
        {
        }
}
}