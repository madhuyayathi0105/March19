<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="inv_inward.aspx.cs" Inherits="inv_inward" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            waer .btn
            {
                width: 40px;
                height: 30px;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            function valid() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                id = document.getElementById("<%=ddl_hostel3.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "SELECT") {
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_transferqty.ClientID %>").value;
                if (id.trim() == "") {
                    empty = "E";
                    id = document.getElementById("<%=txt_transferqty.ClientID %>");
                    id.style.borderColor = 'Red';
                }
                if (empty.trim() != "") {

                    return false;
                }
                else {
                    return true;
                }
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function check(id) {
                var firstvalue = id.value;
                var secondvalue = document.getElementById("<%=txt_totalQunatity.ClientID %>").value;
                if (parseInt(secondvalue) >= parseInt(firstvalue)) {
                }
                else {
                    document.getElementById("<%=txt_transferqty.ClientID %>").style.borderColor = 'Red';
                    document.getElementById("<%=txt_transferqty.ClientID %>").value = "";
                }
            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function size() {
                var cbdirect = document.getElementById("<%=cb_direct.ClientID %>");
                var x = document.getElementById("<%=mdiv.ClientID %>");
                if (cbdirect.checked == true) {
                    x.style.width = "750px";
                }
            }

            function cal() {
                var quanity = document.getElementById("<%=txtpop1qnty.ClientID %>").value;
                var rateperunit = document.getElementById("<%=txtpop1rateunit.ClientID %>").value;
                var discount = document.getElementById("<%=txtpop1dia.ClientID %>").value;
                var tax = document.getElementById("<%=txtpop1tax.ClientID %>").value;
                var etax = document.getElementById("<%=txtpop1exetax.ClientID %>").value;
                var totalcost = document.getElementById("<%=txtpop1totalcost.ClientID %>");
                var othercharge = document.getElementById("<%=txtpop1otherchar.ClientID %>").value;
                var idnew = document.getElementById('<%=cbdis.ClientID %>');
                var totalvalue = 0;
                if (quanity.trim() != "" && rateperunit.trim() != "") {
                    totalvalue = parseFloat(parseFloat(quanity) * parseFloat(rateperunit));
                }
                if (idnew.checked) {
                    if (discount.trim() != "") {
                        totalvalue = totalvalue - parseFloat(discount);
                    }
                }
                else {
                    if (discount.trim() != "") {
                        var dis = parseFloat(totalvalue / 100) * parseFloat(discount);
                        totalvalue = totalvalue - parseFloat(dis);
                    }
                }
                if (tax.trim() != "") {
                    var t = parseFloat(totalvalue / 100) * parseFloat(tax);
                    totalvalue = totalvalue + parseFloat(t);
                }
                if (etax.trim() != "") {
                    var t = parseFloat(totalvalue / 100) * parseFloat(etax);
                    totalvalue = totalvalue + parseFloat(t);
                }
                if (othercharge.trim() != "") {
                    totalvalue = totalvalue + parseFloat(othercharge);
                }
                if (totalvalue != 0) {
                    totalcost.value = parseFloat(totalvalue.toFixed(2));
                }
                else {
                    totalcost.value = "";
                }
            }
            function change() {
                var idnew = document.getElementById('<%=cbdis.ClientID %>');
                if (idnew.checked) {
                    document.getElementById('<%=lblpop1dis.ClientID %>').innerHTML = "Discount(Amt)";
                    document.getElementById('<%=txtpop1dia.ClientID %>').value = "";
                }
                else {
                    document.getElementById('<%=lblpop1dis.ClientID %>').innerHTML = "Discount(%)";
                    document.getElementById('<%=txtpop1dia.ClientID %>').value = "";
                }
            }
               
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <asp:Label ID="lbl_header" runat="server" Style="color: green;" Text="Inward Entry"
                    CssClass="fontstyleheader"></asp:Label>
                <br />
                <br />
            </center>
        </div>
        <center>
            <div style="height: 600px; width: 1000px;">
                <%--class="maindivstyle"--%>
                <br />
                <div id="mdiv" runat="server" class="maintablestyle" style="width: 935px;">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="RadioButton_Inv" runat="server" Text="Inventory" GroupName="r"
                                    Visible="true" Checked="true" AutoPostBack="true" OnCheckedChanged="RadioButton_Inv_Checeked" />
                            </td>
                            <td>
                                <asp:RadioButton ID="RadioButton_lib" runat="server" Text="Library" Visible="true"
                                    GroupName="r" AutoPostBack="true" OnCheckedChanged="RadioButton_lib_Checeked" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_vendor" runat="server" Text="Vendor"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_vendor" runat="server" CssClass="textbox textbox1 txtheight3">--Select--</asp:TextBox>
                                        <asp:Panel ID="p2" runat="server" CssClass="multxtpanel" Style="height: 250px; width: 150px;">
                                            <asp:CheckBox ID="cb_vendor" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_vendor_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_vendor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cbl_vendor_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_vendor"
                                            PopupControlID="p2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_ords" runat="server" Text="Orders"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_ords" runat="server" CssClass="textbox textbox1 txtheight3">--Select--</asp:TextBox>
                                        <asp:Panel ID="pords" runat="server" CssClass="multxtpanel" Style="height: 250px;
                                            width: 150px;">
                                            <asp:CheckBox ID="cb_ords" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_ords_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_ords" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_ords_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_ords"
                                            PopupControlID="pords" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_itm" runat="server" Text="Items"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_item" runat="server" CssClass="textbox textbox1 txtheight3">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="height: 250px;
                                            width: 150px;">
                                            <asp:CheckBox ID="cb_item" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_item_CheckedChange" />
                                            <asp:CheckBoxList ID="Cbl_item" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cbl_item_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_item"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_fromdate" runat="server" Text="From Date" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" runat="server" Width="80px" CssClass="textbox textbox1"
                                    Visible="false"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy">
                                    <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" runat="server" Text="To Date" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" Width="80px" CssClass="textbox textbox1"
                                    Visible="false"></asp:TextBox>
                                <asp:CalendarExtender ID="caltodate" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy">
                                    <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdb_yettorec" runat="server" Text="Yet to Received" GroupName="grdo"
                                    Visible="false" AutoPostBack="true" OnCheckedChanged="yettoreceived" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_received" runat="server" Text="Received" Visible="false"
                                    GroupName="grdo" AutoPostBack="true" OnCheckedChanged="receiced_check" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_reject" runat="server" Text="Rejected" GroupName="grdo"
                                    Visible="false" AutoPostBack="true" OnCheckedChanged="reject" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cb_direct" runat="server" Style="margin-left: 10px;" Text="Direct Inward"
                                    AutoPostBack="true" OnCheckedChanged="cb_direct_CheckedChange" />
                                <asp:RadioButton ID="rdb_dirstore" runat="server" Text="Store" GroupName="grdo" Visible="true"
                                    AutoPostBack="true" OnCheckedChanged="rdb_dirstore_Click" Checked="true" />
                                <asp:RadioButton ID="rdb_dirmess" runat="server" Text="Mess" Visible="true" GroupName="grdo"
                                    AutoPostBack="true" OnCheckedChanged="rdb_dirmess_Check" />
                                <asp:RadioButton ID="rdb_dirdept" runat="server" Text="Department" GroupName="grdo"
                                    Visible="true" AutoPostBack="true" OnCheckedChanged="rdb_dirdept_Click" />
                                <asp:RadioButton ID="rdb_dirbook" runat="server" Text="Book" GroupName="grdo" Visible="true"
                                    AutoPostBack="true" OnCheckedChanged="rdb_dirbook_Click" />
                            </td>
                            <td>
                                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="store" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_dirstorebase" runat="server" CssClass="textbox textbox1 txtheight3">--Select--</asp:TextBox>
                                                <asp:Panel ID="dirp1" runat="server" CssClass="multxtpanel" Style="height: 250px;
                                                    width: 150px;">
                                                    <asp:CheckBox ID="cb_dirstorebase" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_dirstore_CheckedChange" />
                                                    <asp:CheckBoxList ID="cbl_dirstorebase" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_dirstore_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupContro" runat="server" TargetControlID="txt_dirstorebase"
                                                    PopupControlID="dirp1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>
                                    <asp:View ID="View1" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_dirmessbase" runat="server" CssClass="textbox textbox1 txtheight3">--Select--</asp:TextBox>
                                                <asp:Panel ID="dirp2" runat="server" CssClass="multxtpanel" Style="height: 250px;
                                                    width: 150px;">
                                                    <asp:CheckBox ID="cb_dirmessbase" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_dirmessbase_CheckedChange" />
                                                    <asp:CheckBoxList ID="cbl_dirmessbase" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_dirmessbase_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="sd" runat="server" TargetControlID="txt_dirmessbase"
                                                    PopupControlID="dirp2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>
                                    <asp:View ID="View2" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_dirdepartbase" runat="server" CssClass="textbox textbox1 txtheight3">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 250px;
                                                    width: 180px;">
                                                    <asp:CheckBox ID="cb_dirdepbase" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_dirdepbase_CheckedChange" />
                                                    <asp:CheckBoxList ID="cbl_dirdepbase" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_dirdepbase_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_dirdepartbase"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>
                                    <asp:View ID="View3" runat="server">
                                    </asp:View>
                                </asp:MultiView>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" Style="margin-left: 10px;" Text="Go" CssClass="textbox btn1"
                                    OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_add" runat="server" Text="Add New" CssClass="textbox btn2" OnClick="btn_add_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <div>
                    <asp:Label ID="lbl_error" ForeColor="Red" runat="server" Visible="false"></asp:Label>
                </div>
                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Height="350px" OnUpdateCommand="FpSpread1_Command" Style="background-color: White;
                    border-radius: 10px;" class="spreadborder" ShowHeaderSelection="false" OnCellClick="FpSpread1Cell_Click"
                    OnPreRender="Fpspread1_render">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label><%--Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"--%>
                    <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label><%--Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"--%>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" CssClass="textbox textbox1"
                        onkeypress="display()"></asp:TextBox><%--font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"--%>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn2"
                        Text="Export To Excel" Width="127px" /><%--Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" --%>
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        CssClass="textbox btn2" /><%--Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true"--%>
                    <asp:Button ID="btn_yettoreceived" runat="server" Visible="false" Text="Received"
                        OnClick="btn_yettoreceived_Click" CssClass="textbox btn2" /><%-- Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" --%>
                    <asp:Button ID="btnissue" runat="server" Visible="false" Text="Issue" OnClick="Issue_Click"
                        CssClass="textbox btn2" />
                    <%--Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true"--%>
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 40px; width: 40px; height: 30px; width: 30px; position: absolute;
                    margin-top: 68px; margin-left: 438px;" OnClick="imgbtn_closepopclose2_Click" />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 590px; width: 920px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <div>
                        <center>
                            <asp:Label ID="lbl_header3" runat="server" Style="font-size: large; color: Green;"
                                Text="Inward Entry"></asp:Label>
                        </center>
                    </div>
                    <br />
                    <center>
                        <div id="div4" runat="server" visible="false">
                            <table class="maintablestyle" style="width: 900px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_vendor1" runat="server" Text="Vendor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_vendor1" Width="250px" Height="30px" OnSelectedIndexChanged="ddlvendorselect"
                                            AutoPostBack="true" runat="server" CssClass=" textbox1">
                                        </asp:DropDownList>
                                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_vendor1" runat="server" CssClass="textbox textbox1 txtheight4">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 250px;
                                            position: absolute; width: 250px;">
                                            <asp:CheckBox ID="cb_vendor1" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_vendor1_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_vendor1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_vendor1_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_vendor1"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_orders" runat="server" Visible="false" Text="Orders"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_orders" runat="server" Visible="false" CssClass="textbox textbox1 txtheight4">--Select--</asp:TextBox>
                                                <asp:Panel ID="porders" runat="server" CssClass="multxtpanel" Style="height: 250px;
                                                    width: 200px;">
                                                    <asp:CheckBox ID="cb_orders" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_orders_CheckedChange" />
                                                    <asp:CheckBoxList ID="cbl_orders" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_orders_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_orders"
                                                    PopupControlID="porders" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_item1" runat="server" Text="Items"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upp2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_item1" runat="server" CssClass="textbox textbox1 txtheight4">--Select--</asp:TextBox>
                                                <asp:Panel ID="p1" runat="server" CssClass="multxtpanel" Style="height: 250px; width: 200px;">
                                                    <asp:CheckBox ID="cb_items1" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_items1_CheckedChange" />
                                                    <asp:CheckBoxList ID="cbl_items1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_items1_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_item1"
                                                    PopupControlID="p1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_fromdate1" runat="server" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_fromdate1" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate1" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_todate1" runat="server" Style="margin-left: -195px;" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_todate1" runat="server" Width="80px" Style="margin-left: -186px;"
                                            CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate1" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go1" runat="server" Text="Go" Style="margin-left: -280px;" CssClass="textbox btn1"
                                            OnClick="btn_go1_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <center>
                        <div id="dirdiv" runat="server" visible="false">
                            <table class="maintablestyle" style="width: 550px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_vendor2" runat="server" Text="Vendor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_vendor2" Width="200px" Height="30px" OnSelectedIndexChanged="ddl_vendor2_OnSelectedIndexChanged"
                                            AutoPostBack="true" runat="server" Style="margin-left: 0px;" CssClass=" textbox1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_item2" runat="server" Style="margin-left: 0px;" Text="Items"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_item2" runat="server" Style="margin-left: 0px;" CssClass="textbox textbox1 txtheight4">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="height: 250px;
                                                    width: 185px;">
                                                    <asp:CheckBox ID="cb_dir" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_dir_CheckedChange" />
                                                    <asp:CheckBoxList ID="cbl_dir" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_dir_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_item2"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <%--<td> delsi
                                        <asp:Label ID="lbl_fd" runat="server" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_fd" runat="server" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_fd" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                    <%--    </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_td" runat="server" Style="margin-left: 0px;" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_td" runat="server" Width="80px" Style="margin-left: 0px;" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_td" runat="server"
                                            Format="dd/MM/yyyy">--%>
                                    <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                    <%-- </asp:CalendarExtender>
                                    </td>--%>
                                    <td>
                                        <asp:Button ID="btn_dir" runat="server" Text="Go" Style="margin-left: 0px;" CssClass="textbox btn1"
                                            OnClick="btn_dir_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="lbl_error1" ForeColor="Red" runat="server" Visible="false"></asp:Label>
                        </div>
                    </center>
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Style="box-shadow: 0px 0px 8px #999999;
                            width: 890px; height: 300px; overflow: auto; background-color: White; border-radius: 10px;"
                            OnUpdateCommand="FpSpread2_Command" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <center>
                        <div style="width: 890px; height: 300px; overflow: auto; background-color: White;
                            box-shadow: 0px 0px 8px #999999; border-radius: 10px;" id="dircectinward" runat="server"
                            visible="false">
                            <FarPoint:FpSpread ID="FpSpread4" runat="server" Visible="false" BorderColor="Black"
                                BorderStyle="Solid" ShowHeaderSelection="false" BorderWidth="1px" OnCellClick="FpSpread4Cell_Click"
                                OnPreRender="Fpspread4_render">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    <br />
                    <div>
                        <table cellpadding="2px">
                            <tr>
                                <%--<td>
                                    <asp:RadioButton ID="rdb_store" Text="Store" Visible="false" runat="server" AutoPostBack="true"
                                        GroupName="co" OnCheckedChanged="rdb_store_Click" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_hostel" Visible="false" Text="Mess Name" runat="server"
                                        AutoPostBack="true" GroupName="co" OnCheckedChanged="rdb_Hostel_name" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_dept" Visible="false" Text="Department" runat="server" AutoPostBack="true"
                                        GroupName="co" OnCheckedChanged="rdb_dept_Click" />
                                </td>
                                --%><td>
                                    <asp:Label ID="lbl_storename" Visible="false" Text="Store Name" runat="server"></asp:Label>
                                    <asp:Label ID="lbl_messname" Visible="false" Text="Mess Name" runat="server"></asp:Label>
                                    <asp:Label ID="lbl_dept" Visible="false" Text="Department" runat="server"></asp:Label>
                                    <asp:Label ID="lbl_lib" Visible="false" Text="Library" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_storename" Width="140px" Visible="false" runat="server"
                                        CssClass="textbox1 ddlstyle ddlheight3">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_Hostelname" Width="140px" Visible="false" runat="server"
                                        CssClass="textbox1 ddlstyle ddlheight3">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_deptname" Visible="false" runat="server" CssClass="textbox1 ddlstyle ddlheight4">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_Libname" Visible="false" runat="server" CssClass="textbox1 ddlstyle ddlheight4">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_billdno" Text="Bill Number" Visible="false" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_dbillno" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_billddate" Text="Bill Date" Visible="false" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_BilldDate" runat="server" Visible="false" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender7" TargetControlID="txt_BilldDate" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%--24.03.16--%>
                    <div id="divbtns" visible="false" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_staff" runat="server" Width="100px" Text="Staff Name"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_staff" TextMode="SingleLine" onfocus="return myFunction(this)"
                                        runat="server" Height="20px" CssClass="textbox textbox1 txtheight4" OnTextChanged="checkstaffname"
                                        AutoPostBack="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_staff"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .@">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staff"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:Button ID="btn_staff" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_staff_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_receive" runat="server" Text="Received" CssClass="textbox btn2"
                                        OnClick="btn_receive_Click" />
                                    <asp:Button ID="btn_reject1" runat="server" Text="Reject" CssClass="textbox btn2"
                                        OnClick="btn_reject1_Click" />
                                    <asp:Button ID="btn_wait1" runat="server" Text="Waiting" Visible="false" CssClass="textbox btn2"
                                        OnClick="btn_wait1_Click" />
                                    <asp:Button ID="btn_exit1" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit1_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="directinwardpop" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 74px; margin-left: 311px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 420px; width: 657px;" class="subdivstyle">
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblpop1qnty" Text="Quantity" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpop1qnty" runat="server" CssClass="textbox textbox1 txtheight"
                                    MaxLength="6" onfocus="return myFunction1(this)" onchange="return cal()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtpop1qnty"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                                <span style="color: Red">*</span>
                            </td>
                            <td>
                                <asp:Label ID="lblpop1rateunit" Text="Rate Per Unit" runat="server"></asp:Label>
                            </td>
                            <td style="width: 157px;">
                                <asp:TextBox ID="txtpop1rateunit" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight"
                                    onfocus="return myFunction1(this)" onchange="return cal()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtpop1rateunit"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                                <span style="color: Red">*</span>
                                <asp:Label ID="Label1" Text="Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_date" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_date" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbdis" runat="server" onchange="return change()" />
                                <asp:Label ID="lblpop1dis" Text="Discount(Amt)" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpop1dia" runat="server" CssClass="textbox textbox1 txtheight"
                                    onchange="return cal()" MaxLength="6"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtpop1dia"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblpop1disamt" Text="Discount(%)" Visible="false" runat="server"></asp:Label>
                                <asp:Label ID="lbl_sailingprize" Text="Sailing Price" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpop1dis" runat="server" Visible="false" MaxLength="6" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtpop1dis"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                                <asp:TextBox ID="txt_sailingprice" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_sailingprice"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblpop1tax" Text="Tax(%)" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpop1tax" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight"
                                    onchange="return cal()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtpop1tax"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblpop1exetax" Text="Exercise tax(%)" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpop1exetax" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight"
                                    onchange="return cal()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txtpop1exetax"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblpop1educess" Text="Education Cess" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpop1educess" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtpop1educess"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblpop1higher" Text="Higher Education Cess" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpop1higher" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtpop1higher"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblpop1otherchar" Text="Other Charges" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpop1otherchar" runat="server" MaxLength="6" onchange="return cal()"
                                    CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtpop1otherchar"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblpop1des" Text="Description" runat="server"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtpop1des" runat="server" CssClass="textbox textbox1" Width="200px"
                                    Height="40px" TextMode="MultiLine"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txtpop1des"
                                    FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_brandname" Visible="false" Text="Brand Name" runat="server"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddl_brandname" Visible="false" Width="200px" CssClass="textbox textbox1 ddlheight4"
                                    runat="server" onfocus="return myFunction(this)" onchange="change3(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_brandname" Visible="false" runat="server" Width="200px" onfocus="return myFunction(this)"
                                    CssClass="textbox textbox1 txtheight" Style="width: 200px; display: none;"></asp:TextBox>
                            </td>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txt_brandname"
                                FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars=" ">
                            </asp:FilteredTextBoxExtender>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblpop1totalcost" Text="Total Cost" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpop1totalcost" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_upbillno" Text="Bill Number" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_upbillno" runat="server" Visible="false" MaxLength="15" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_upbillno"
                                    FilterType="Numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="lbl_upbilldate" Text="Bill Date" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_upbilldate" runat="server" Visible="false" Width="80px" CssClass="textbox textbox1"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender8" TargetControlID="txt_upbilldate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_upstaff" runat="server" Width="100px" Text="Staff Name"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txt_upstaff" TextMode="SingleLine" runat="server" Height="20px"
                                    CssClass="textbox textbox1 txtheight4" OnTextChanged="checkstaffname1" AutoPostBack="true"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_upstaff"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .@">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_upstaff"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:Button ID="btnupQ1" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_staff_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <asp:Button ID="btn_directpo" Text="Ok" runat="server" CssClass="textbox btn2" OnClick="btn_directpo_Click" />
                        <asp:Button ID="btnpop1exit" Text="Exit" runat="server" CssClass="textbox btn2" OnClick="btnpop1exit_click" />
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="popupsscode1" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 436px;"
                    OnClick="imagebtnpopclose4_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 650px; width: 900px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_selectstaffcode" runat="server" Style="font-size: large; color: #0AA7B3;"
                            Text="Select the Staff Name"></asp:Label>
                    </center>
                    <br />
                    <div>
                        <center>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_college" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                            CssClass="textbox1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_department" runat="server" Text="Department"></asp:Label>
                                        <asp:DropDownList ID="ddl_department" Width="180px" Height="30px" runat="server"
                                            AutoPostBack="true" CssClass=" textbox1" OnSelectedIndexChanged="department_selectedindex_change">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_searchby" runat="server" Text="Search By"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_searchby" Width="250px" Height="30px" runat="server" CssClass=" textbox1">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_searchby" Visible="true" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btn_go2" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go2_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br>
                            <center>
                                <asp:Label ID="err" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                            </center>
                            <div style="width: 689px;">
                                <p align="right">
                                    <asp:Label ID="lbl_errorsearch" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                </p>
                                <p align="right">
                                    <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                                        ForeColor="Red"></asp:Label>
                                </p>
                                <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                    height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                    box-shadow: 0px 0px 8px #999999;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0099CC">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_save1" Visible="false" runat="server" CssClass="textbox btn2"
                                        Text="Save" OnClick="btn_save1_Click" />
                                    <asp:Button ID="btn_exit2" Visible="false" runat="server" CssClass="textbox btn2"
                                        Text="Exit" OnClick="btn_exit2_Click" />
                                </div>
                            </center>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="Div1" runat="server" visible="false" class="popupstyle popupheight1">
                <asp:ImageButton ID="imgbtn_close" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 70px; margin-left: 254px;"
                    OnClick="imgbtn_closepopclose_Click" />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 630px; width: 800px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <div>
                        <center>
                            <asp:Label ID="Label2" runat="server" Style="font-size: large; color: Green;" Text="Transfer to Hostel"></asp:Label>
                        </center>
                    </div>
                    <br />
                    <center>
                        <table style="border: 2px solid #F0F0F0; border-radius: 10px; background-color: #F0F0F0;
                            width: 600px; height: 50px; display: none; box-shadow: 0px 0px 8px #999999;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostelname" runat="server" Text="Hostel"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostel1" runat="server" CssClass="textbox textbox1">--Select--</asp:TextBox>
                                            <asp:Panel ID="panelhostel" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                                BorderColor="Black" BorderStyle="Solid" Style="overflow: auto; height: 250px;
                                                width: 200px;">
                                                <asp:CheckBox ID="cb_hostel1" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_hostel1_CheckedChange" />
                                                <asp:CheckBoxList ID="cbl_hostel1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostel1_SelectIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_hostel1"
                                                PopupControlID="panelhostel" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_transfergo" runat="server" Text="Go" CssClass="textbox btn" OnClick="btn_transfergo_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="lbl_error2" ForeColor="Red" runat="server" Visible="false"></asp:Label>
                        </div>
                    </center>
                    <center>
                        <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Style="overflow: auto; height: 350px; background-color: White;
                            border-radius: 10px;" OnCellClick="Cell_Click" OnPreRender="FpSpread3_Render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                    <div id="div2" visible="false" runat="server">
                        <asp:Button ID="btn_transfer" runat="server" Text="Transfer" CssClass="textbox btn2"
                            OnClick="btn_transfer_Click" />
                        <asp:Button ID="btn_transferexit" runat="server" Text="Exit" CssClass="textbox btn2"
                            OnClick="btn_transferexit_Click" />
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow3" runat="server" visible="false">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 70px; margin-left: 254px;"
                    OnClick="imgbtn_closepopclose1_Click" />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 350px; width: 800px; border: 3px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_hostel3" Text="Hostel" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_hostel3" runat="server" onfocus="return myFunction(this)"
                                    CssClass=" textbox1">
                                </asp:DropDownList>
                                <span style="color: Red">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_totalqty" Text="Total Quantity" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_totalQunatity" runat="server" Enabled="false" CssClass="textbox textbox1"
                                    Width="80px" onfocus="return myFunction1(this)"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_totalQunatity"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_transferqut" Text="Transfer Quantity" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_transferqty" runat="server" onfocus="return myFunction(this)"
                                    CssClass="textbox textbox1" onblur="return check(this)" Width="80px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_transferqty"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_date" Text="Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_transferdate" runat="server" CssClass="textbox textbox1" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="calfromdate" TargetControlID="txt_transferdate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="div3" visible="false" runat="server">
                        <asp:Button ID="btn_newadd3" runat="server" Text="Add" Visible="false" CssClass="textbox btn2"
                            OnClick="btn_newadd3_Click" OnClientClick="return valid()" />
                        <asp:Button ID="btn_exit3" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit3_Click" />
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn2 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
