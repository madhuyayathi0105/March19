using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Configuration;

public partial class StudentMod_Referred_Entry_ForStudent : System.Web.UI.Page
{

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
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
            }//****************************************************//

            userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

        }
        catch (Exception ex) { }
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> searchConsultant(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select refer_name,IdNo from Student_Refer_Details ";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> searchAgent(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select refer_agent_name,IdNo from Student_Refer_Details";

        name = ws.Getname(query);
        return name;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dscon = new DataSet();
            DataTable data = new DataTable();
            DataRow drow;
            string seltqry = "";
            data.Columns.Add("Consultant");
            data.Columns.Add("Agent");
            data.Columns.Add("pincode");
            data.Columns.Add("Address");
            data.Columns.Add("City");
            data.Columns.Add("District");
            data.Columns.Add("State");
            data.Columns.Add("Phone");
            data.Columns.Add("Email");
            data.Columns.Add("Remark");
            data.Columns.Add("considno");
            data.Columns.Add("ReferId");
            if (txt_Consultant.Text != "")
                seltqry = "select * from Student_Refer_Details where  refer_name='" + txt_Consultant.Text + "'";
            else if (TextAgent.Text != "")
                seltqry = "select * from Student_Refer_Details where refer_agent_name='" + TextAgent.Text + "'";
            else if (txt_Consultant.Text != "" && TextAgent.Text != "")
                seltqry = "select * from Student_Refer_Details where  refer_name='" + txt_Consultant.Text + "' and refer_agent_name='" + TextAgent.Text + "'";
            else
                seltqry = "select * from Student_Refer_Details";
            dscon.Clear();
            dscon = d2.select_method_wo_parameter(seltqry, "Text");
            if (dscon.Tables.Count > 0 && dscon.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dscon.Tables[0].Rows.Count; i++)
                {
                    drow = data.NewRow();
                    drow["Consultant"] = dscon.Tables[0].Rows[i]["refer_name"];
                    drow["Agent"] = dscon.Tables[0].Rows[i]["refer_agent_name"];
                    drow["pincode"] = dscon.Tables[0].Rows[i]["agent_pincode"];
                    drow["Address"] = dscon.Tables[0].Rows[i]["agent_address"];
                    drow["City"] = dscon.Tables[0].Rows[i]["agent_city"];
                    drow["District"] = dscon.Tables[0].Rows[i]["agent_district"];
                    drow["State"] = dscon.Tables[0].Rows[i]["agent_state"];
                    drow["Phone"] = dscon.Tables[0].Rows[i]["refer_phoneno"];
                    drow["Email"] = dscon.Tables[0].Rows[i]["refer_email"];
                    drow["Remark"] = dscon.Tables[0].Rows[i]["refer_remark"];
                    drow["considno"] = dscon.Tables[0].Rows[i]["IdNo"];
                    drow["ReferId"] = dscon.Tables[0].Rows[i]["ReferenceId"];
                    data.Rows.Add(drow);
                }
                if (data.Columns.Count > 0 && data.Rows.Count > 0)
                {
                    SelectGrid.DataSource = data;
                    SelectGrid.DataBind();
                    SelectGrid.Visible = true;
                    griddiv.Visible = true;


                }
            }
            else
            {

                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
        }
        catch
        {



        }

    }

    protected void btn_Addnew_Click(object sender, EventArgs e)
    {
        try
        {
            AddpopupRefer.Visible = true;
            btn_save.Text = "Save";
            btn_Delete.Visible = false;
        }
        catch
        {


        }

    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void SelectedIndexChanged(Object sender, EventArgs e)
    {
        btn_save.Text = "Update";
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int RowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        Session["Gridcellrow"] = Convert.ToString(RowIndex);

        if (RowIndex != -1)
        {
            Label lblcon = (SelectGrid.Rows[RowIndex].FindControl("lbl_Consultant") as Label);
            Label lblidval = (SelectGrid.Rows[RowIndex].FindControl("lbl_OtherId") as Label);
            Label lblage = (SelectGrid.Rows[RowIndex].FindControl("lbl_Agent") as Label);
            Label lblpin = (SelectGrid.Rows[RowIndex].FindControl("lbl_pin") as Label);
            Label lbladd = (SelectGrid.Rows[RowIndex].FindControl("lbl_Address") as Label);
            Label lblcit = (SelectGrid.Rows[RowIndex].FindControl("lbl_City") as Label);
            Label lbldis = (SelectGrid.Rows[RowIndex].FindControl("lbl_District") as Label);
            Label lblst = (SelectGrid.Rows[RowIndex].FindControl("lbl_State") as Label);
            Label lblphno = (SelectGrid.Rows[RowIndex].FindControl("lbl_Phone") as Label);
            Label lblemid = (SelectGrid.Rows[RowIndex].FindControl("lbl_Email") as Label);
            Label lblremk = (SelectGrid.Rows[RowIndex].FindControl("lbl_Remark") as Label);
            txtConsultant.Text = lblcon.Text;
            txt_idval.Text = lblidval.Text;
            TexAgent.Text = lblage.Text;
            TextPincode.Text = lblpin.Text;
            Textadd.Text = lbladd.Text;
            TextCity.Text = lblcit.Text;
            TextDistrict.Text = lbldis.Text;
            Textstate.Text = lblst.Text;
            Textphone.Text = lblphno.Text;
            Textemail.Text = lblemid.Text;
            Textremk.Text = lblremk.Text;
            AddpopupRefer.Visible = true;
            btn_Delete.Visible = true;
        }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {

        try
        {
            //    string apiKey = "AIzaSyDnEtI****9i4FJIZ***";
            //    string locationURL = "https://maps.googleapis.com/maps/api/geocode/json?address=" + 607802 + "&key=" + apiKey;
            //    string r = new System.Net.WebClient().DownloadString(locationURL);
            //    dynamic result = JObject.Parse(r);
            //    string latlng = result.results[0].geometry.location.lat + "," + result.results[0].geometry.location.lng;

            int rowind = Convert.ToInt32(Session["Gridcellrow"]);
            string consname = txtConsultant.Text;
            string idvalue = txt_idval.Text;
            string agtname = TexAgent.Text;
            string pincode = TextPincode.Text;
            string address = Textadd.Text;
            string city = TextCity.Text;
            string dist = TextDistrict.Text;
            string state = Textstate.Text;
            string phone = Textphone.Text;
            string email = Textemail.Text;
            string remrk = Textremk.Text;
            if (!string.IsNullOrEmpty(consname) && !string.IsNullOrEmpty(agtname) && !string.IsNullOrEmpty(pincode) && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(dist) && !string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(email))
            {
                if (btn_save.Text.ToUpper() == "SAVE")
                {
                    string insqry = "if not exists(select * from Student_Refer_Details where  refer_name='" + consname + "' and refer_agent_name='" + agtname + "' and agent_pincode='" + pincode + "' and ReferenceId='" + idvalue + "') insert into Student_Refer_Details(refer_name,refer_agent_name,agent_pincode,agent_address,agent_city,agent_district,agent_state,refer_phoneno,refer_email,refer_remark,ReferenceId) Values('" + consname + "','" + agtname + "','" + pincode + "','" + address + "','" + city + "','" + dist + "','" + state + "','" + phone + "','" + email + "','" + remrk + "','" + idvalue + "') else update Student_Refer_Details set  agent_address='" + address + "',agent_city='" + city + "',agent_district='" + dist + "',agent_state='" + state + "',refer_phoneno='" + phone + "',refer_email='" + email + "',refer_remark='" + remrk + "' , ReferenceId='" + idvalue + "' where refer_name='" + consname + "' and refer_agent_name='" + agtname + "' and agent_pincode='" + pincode + "'";
                    int ins = d2.update_method_wo_parameter(insqry, "Text");
                    if (ins > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully!')", true);
                        clear();
                    }
                }
                if (rowind >= 0 && btn_save.Text.ToUpper() == "UPDATE")
                {
                    Label lblremk = (SelectGrid.Rows[rowind].FindControl("lbl_idno") as Label);
                    string idno = lblremk.Text;
                    string insqry = "update Student_Refer_Details set  agent_address='" + address + "',agent_city='" + city + "',agent_district='" + dist + "',agent_state='" + state + "',refer_phoneno='" + phone + "',refer_email='" + email + "',refer_remark='" + remrk + "',refer_name='" + consname + "',refer_agent_name='" + agtname + "',agent_pincode='" + pincode + "' , ReferenceId='" + idvalue + "' where IdNo='" + idno + "'";
                    int ins = d2.update_method_wo_parameter(insqry, "Text");
                    if (ins > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Updated Successfully!')", true);
                        clear();
                    }

                }
                btngo_Click(sender, e);
            }



        }
        catch
        {


        }


    }


    protected void btn_Delete_Click(object sender, EventArgs e)
    {

        try
        {
            int rowind = Convert.ToInt32(Session["Gridcellrow"]);
            string consname = txtConsultant.Text;
            string agtname = TexAgent.Text;
            string pincode = TextPincode.Text;
            if (rowind >= 0)
            {
                Label lblremk = (SelectGrid.Rows[rowind].FindControl("lbl_idno") as Label);
                string idno = lblremk.Text;
                string insqry = "Delete from Student_Refer_Details where IdNo='" + idno + "'";
                int ins = d2.update_method_wo_parameter(insqry, "Text");
                if (ins > 0)
                {

                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully!')", true);
                    clear();

                }
                btngo_Click(sender, e);
            }
        }
        catch
        {


        }


    }

    public void clear()
    {
        AddpopupRefer.Visible = false;
        txtConsultant.Text = "";
        txt_idval.Text = "";
        TexAgent.Text = "";
        TextPincode.Text = "";
        Textadd.Text = "";
        TextCity.Text = "";
        TextDistrict.Text = "";
        Textstate.Text = "";
        Textphone.Text = "";
        Textemail.Text = "";
        Textremk.Text = "";
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        clear();
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        AddpopupRefer.Visible = false;
        txtConsultant.Text = "";
        txt_idval.Text = "";
        TexAgent.Text = "";
        TextPincode.Text = "";
        Textadd.Text = "";
        TextCity.Text = "";
        TextDistrict.Text = "";
        Textstate.Text = "";
        Textphone.Text = "";
        Textemail.Text = "";
        Textremk.Text = "";
    }


    //protected void FindCoordinates(object sender, EventArgs e)
    //{
    //    string url = "http://maps.google.com/maps/api/geocode/xml?address=" + txtLocation.Text + "&sensor=false";
    //    WebRequest request = WebRequest.Create(url);
    //    using (WebResponse response = (HttpWebResponse)request.GetResponse())
    //    {
    //        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
    //        {
    //            DataSet dsResult = new DataSet();
    //            dsResult.ReadXml(reader);
    //            DataTable dtCoordinates = new DataTable();
    //            dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
    //                    new DataColumn("Address", typeof(string)),
    //                    new DataColumn("Latitude",typeof(string)),
    //                    new DataColumn("Longitude",typeof(string)) });
    //            foreach (DataRow row in dsResult.Tables["result"].Rows)
    //            {
    //                string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
    //                DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
    //                dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
    //            }
    //            //GridView1.DataSource = dtCoordinates;
    //            //GridView1.DataBind();
    //        }
    //    }
    //}
}