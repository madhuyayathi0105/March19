using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class SupplierTableChange : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_update_click(object sender, EventArgs e)
    {
        string sql = "select * from supplier_details";
        ds = d2.select_method_wo_parameter(sql, "text");
        if (ds.Tables.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string supplier_code = Convert.ToString(ds.Tables[0].Rows[i]["supplier_code"]);
                string supplier_name = Convert.ToString(ds.Tables[0].Rows[i]["supplier_name"]);
                string doorst_no = Convert.ToString(ds.Tables[0].Rows[i]["doorst_no"]);
                string city = Convert.ToString(ds.Tables[0].Rows[i]["city"]);
                string pin = Convert.ToString(ds.Tables[0].Rows[i]["pin"]);
                string phone_no = Convert.ToString(ds.Tables[0].Rows[i]["phone_no"]);
                string fax_no = Convert.ToString(ds.Tables[0].Rows[i]["fax_no"]);
                string email = Convert.ToString(ds.Tables[0].Rows[i]["email"]);
                string EmailID1 = Convert.ToString(ds.Tables[0].Rows[i]["EmailID1"]);
                string EmailID2 = Convert.ToString(ds.Tables[0].Rows[i]["EmailID2"]);
                string website = Convert.ToString(ds.Tables[0].Rows[i]["website"]);
                string Address2 = Convert.ToString(ds.Tables[0].Rows[i]["Address2"]);
                string District = Convert.ToString(ds.Tables[0].Rows[i]["District"]);
                string SupplierType = Convert.ToString(ds.Tables[0].Rows[i]["SupplierType"]);

                string insertqry = "if exists(select VendorCode from CO_VendorMaster where VendorCode='" + supplier_code + "' ) update CO_VendorMaster set VendorCode= '" + supplier_code + "',VendorCompName='" + supplier_name + "' where  VendorCode='" + supplier_code + "' else insert into CO_VendorMaster (VendorCode,VendorCompName,VendorAddress,VendorPin,VendorPhoneNo,VendorFaxNo,VendorEmailID,VendorType,VendorTINNo,VendorCSTNo,VendorWebsite,VendorCity,VendorDist,VendorState,VendorStartYear,VendorPANNo,VendorPayType,VendorStatus,VendorMobileNo,SupplierType,EmailID1,EmailID2,Address2,LibraryFlag)values('" + supplier_code + "','" + supplier_name + "','" + doorst_no + "','" + pin + "','','" + fax_no + "','" + email + "','1','','','" + website + "','" + city + "','" + District + "','','','','','','" + phone_no + "','" + SupplierType + "','" + EmailID1 + "','" + EmailID2 + "','" + Address2 + "','1')";
                int Up = d2.update_method_wo_parameter(insertqry, "Text");
                if (Up > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Update Successfully')", true);
                }
            }
        }
    }
}