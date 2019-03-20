<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="Supplier_master.aspx.cs" Inherits="Supplier_master"
    EnableEventValidation="false" %>

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
            .popupheight3
            {
                height: 55em;
            }
            .email
            {
                border: 1px solid #c4c4c4;
                padding: 4px 4px 4px 4px;
                border-radius: 4px;
                -moz-border-radius: 4px;
                -webkit-border-radius: 4px;
                box-shadow: 0px 0px 8px #d9d9d9;
                -moz-box-shadow: 0px 0px 8px #d9d9d9;
                -webkit-box-shadow: 0px 0px 8px #d9d9d9;
            }
            .watermark
            {
                color: #999999;
            }
        </style>
        <script type="text/javascript">
            function valid1() {
                var idval = "";
                var empty = "";

                idval = document.getElementById("<%=txt_connam.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_connam.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_designation.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_designation.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_conmob.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_conmob.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }

            function valid2() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                idval = document.getElementById("<%=txt_vendorname1.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_vendorname1.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_street.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_street.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_city.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_city.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=ddlstatus.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    id = document.getElementById("<%=ddlstatus.ClientID %>");
                    id.style.borderColor = 'Red';
                }
                idval = document.getElementById("<%=txtconbank.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txtconbank.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txtconbankname.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txtconbankname.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
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
            function checkEmail(id) {
                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!filter.test(id.value)) {
                    id.style.borderColor = 'Red';
                    id.value = "";
                    email.focus;
                }
                else {
                    id.style.borderColor = '#c4c4c4';
                }
            }
        </script>
    </head>
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <span style="color: Green;" class="fontstyleheader">Supplier Master</span>
                <br />
                <br />
            </div>
        </center>
        <center>
            <div class="maindivstyle" style="height: 800px; width: 1000px;">
                <br />
                <div>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_vendorName" runat="server" Text="Supplier Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_vendorname" runat="server" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pvendorname" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cb_vendorname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_vendorname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_vendorname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_vendorname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pextender" runat="server" TargetControlID="txt_vendorname"
                                            PopupControlID="pvendorname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_vendortype" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_vendortype" runat="server" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true" Width="120px" Height="18px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pvendortype" runat="server" CssClass="multxtpanel" Style="width: 126px;
                                            height: 80px;">
                                            <asp:CheckBox ID="cb_vendortype" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_vendortype_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_vendortype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_vendortype_SelectedIndexChanged">
                                                <asp:ListItem Value="1">Approved</asp:ListItem>
                                                <asp:ListItem Value="2">Blocked</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_vendortype"
                                            PopupControlID="pvendortype" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_search" runat="server" Text="Search By"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox  ddlheight3" OnSelectedIndexChanged="ddl_type_SelectedIndexChanged"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="0">Item Name</asp:ListItem>
                                    <asp:ListItem Value="1">Supplier Name</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_search" Visible="false" runat="server" placeholder="Search Item Name"
                                    CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                                <asp:TextBox ID="txt_vendorname2" Visible="false" runat="server" placeholder="Search Supplier Name"
                                    CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname1" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_vendorname2"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_add" runat="server" CssClass="textbox btn2" Text="Add New" OnClick="btn_addnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <%-- <br />--%>
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <center>
                        <asp:Label ID="lbl_errormsg" runat="server" Style="color: Red;"></asp:Label></center>
                    <div>
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="889px">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="~/images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                    </div>
                    <br />
                    <center>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="890px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_column" runat="server" Font-Bold="true" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                            Visible="false" Width="111px" OnClick="lb_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tborder" Visible="false" Width="867px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="6" RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_columnorder_SelectedIndexChanged">
                                            <%--<asp:ListItem Selected="True" Value="Roll_No">Roll No</asp:ListItem>--%>
                                            <asp:ListItem Value="VendorCode" Selected="True">Supplier Code</asp:ListItem>
                                            <asp:ListItem Value="VendorCompName" Selected="True">Company Name</asp:ListItem>
                                            <asp:ListItem Value="VendorAddress" Selected="True">Street</asp:ListItem>
                                            <asp:ListItem Value="VendorCity">City</asp:ListItem>
                                            <asp:ListItem Value="VendorPin">Pincode</asp:ListItem>
                                            <asp:ListItem Value="VendorPhoneNo">Phone No</asp:ListItem>
                                            <asp:ListItem Value="VendorFaxNo">Fax No</asp:ListItem>
                                            <asp:ListItem Value="VendorEmailID">Mail Id</asp:ListItem>
                                            <asp:ListItem Value="VendorStatus">Status</asp:ListItem>
                                            <asp:ListItem Value="VendorWebsite">Website</asp:ListItem>
                                            <asp:ListItem Value="VendorDist">District</asp:ListItem>
                                            <asp:ListItem Value="VendorState">State</asp:ListItem>
                                            <asp:ListItem Value="VendorMobileNo">Mobile No</asp:ListItem>
                                            <asp:ListItem Value="VendorCSTNo">CST No</asp:ListItem>
                                            <asp:ListItem Value="VendorPANNo">PAN</asp:ListItem>
                                            <asp:ListItem Value="VendorTINNo">TIN</asp:ListItem>
                                            <asp:ListItem Value="VendorStartYear">Business Start Year</asp:ListItem>
                                            <asp:ListItem Value="VendorPayType">Payment Type</asp:ListItem>
                                            <%-- <asp:ListItem Value="VendorStatus">Status</asp:ListItem>--%>
                                            <asp:ListItem Value="VenBankName">Bank Name</asp:ListItem>
                                            <asp:ListItem Value="VenBankBranch">Bank Branch</asp:ListItem>
                                            <asp:ListItem Value="VendorBankIFSCCode">IFSC Code</asp:ListItem>
                                            <asp:ListItem Value="VendorAccNo">Bank A/C No</asp:ListItem>
                                            <asp:ListItem Value="VendorBankSWIFTCode">SWIFT Code</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                        ExpandedImage="~/images/down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <br />
                </div>
                <div id="div1" runat="server" visible="false" style="width: 950px; height: 350px;
                    box-shadow: 0px 0px 8px #999999;" class="reportdivstyle">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="900px" Height="350px" OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please enter the report name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" CssClass="textbox textbox1 txtheight5"
                        onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=". ">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn2"
                        Text="Export To Excel" Width="127px" Height="30px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Width="60px" Height="30px" CssClass="textbox btn2" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </div>
        </center>
        <center>
            <div id="poperrjs" runat="server" visible="false" class="popupstyle popupheight3">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 9px; margin-left: 437px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <center>
                    <div class="subdivstyle" style="background-color: White; height: 750px; width: 900px;">
                        <br />
                        <div>
                            <center>
                                <span style="color: Green; font-size: large;">Supplier Entry</span>
                            </center>
                        </div>
                        <br />
                        <div style="float: left; width: 900px; height: 400px;">
                            <center>
                                <table>
                                    <tr style="display: none;">
                                        <td>
                                            <asp:Label ID="lbl_Code" runat="server" Text="Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_code" CssClass="textbox textbox1 txtheight1" Width="100px" Enabled="false"
                                                runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td>
                                            <asp:Label ID="lbl_type" runat="server" Text="Type"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdb_vendor" runat="server" Text="Supplier" GroupName="same" />
                                            <asp:RadioButton ID="rdb_customer" runat="server" Visible="false" Text="Customer"
                                                GroupName="same" />
                                            <span style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_vendorname1" runat="server" Text="Supplier Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_vendorname1" CssClass="textbox textbox1 txtheight1" onfocus="return myFunction(this)"
                                                Width="300px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_vendorname1"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" .&/#">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_street" runat="server" Text="Street"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_street" CssClass="textbox textbox1 txtheight1" onfocus="return myFunction(this)"
                                                Width="150px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txt_street"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ,/-.">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_City" runat="server" Text="City"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_city" CssClass="textbox textbox1 txtheight1" onfocus="return myFunction(this)"
                                                Width="150px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_city"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ,/-.">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_pin" runat="server" Text="PinCode"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pin" CssClass="textbox textbox1 txtheight1" MaxLength="6" Width="100px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_pin"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_state" runat="server" Text="State"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_State" runat="server" CssClass="textbox textbox1 ddlheight5"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_State_Selectindexchange">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txt_state" CssClass="textbox textbox1 txtheight1" Width="75px" Visible="false"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" TargetControlID="txt_state"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_district" runat="server" Text="District"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txt_district" CssClass="textbox textbox1 txtheight1" Width="200px"
                                                runat="server" Visible="false"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_district"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:DropDownList ID="ddl_district" runat="server" CssClass="textbox textbox1 ddlheight5">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_Phone" runat="server" Text="Phone No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_phn" MaxLength="13" CssClass="textbox textbox1 txtheight1" Width="200px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_phn"
                                                FilterType="Numbers,custom" ValidChars="- ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_mobileno" runat="server" Text="Mobile No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_mainmobileno" MaxLength="10" CssClass="textbox textbox1 txtheight1"
                                                Width="200px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txt_mainmobileno"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_email" runat="server" Text="Email Id"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_email" CssClass="email textbox1 txtheight1" Width="200px" runat="server"
                                                onfocus="return myFunction(this)" onblur="return checkEmail(this)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_email"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=".@">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_web" runat="server" Text="Website"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_web" CssClass="textbox textbox1 txtheight1" Width="200px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_web"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=".@">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_cst" runat="server" Text="CST No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_cst" MaxLength="13" CssClass="textbox textbox1 txtheight1" Width="200px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_cst"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_tin" runat="server" Text="TIN No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_tin" MaxLength="20" CssClass="textbox textbox1 txtheight1" Width="200px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txt_tin"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pan" runat="server" Text="PAN No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pan" MaxLength="8" CssClass="textbox textbox1 txtheight1" Width="200px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_pan"
                                                FilterType="Numbers, UppercaseLetters, LowercaseLetters">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_fax" runat="server" Text="Fax No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtfax" MaxLength="20" CssClass="textbox textbox1 txtheight1" Width="200px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtfax"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" Text="Business Start Year"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_startyear" runat="server" CssClass="textbox textbox1 txtheight"
                                                MaxLength="4" AutoPostBack="true" OnTextChanged="txtyear_Onchange"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" TargetControlID="txt_startyear"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:TextBox ID="oldyeartxt" Visible="false" Text="1900" CssClass="textbox textbox1 txtheight"
                                                Width="75px" runat="server"></asp:TextBox>
                                            <%-- <asp:DropDownList ID="ddlbis" runat="server" CssClass="textbox textbox1" Width="100px">
                                        </asp:DropDownList>--%>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlstatus" runat="server" onfocus="return myFunction(this)"
                                                CssClass="textbox textbox1 ddlheight">
                                            </asp:DropDownList>
                                            <span style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPayment" runat="server" Text="Payment Type"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbpaymentcash" runat="server" Text="Cash" GroupName="same2" />
                                            <asp:RadioButton ID="rdbpaymentCredit" runat="server" Text="Credit" GroupName="same2" />
                                            <asp:RadioButton ID="rdbpaymentCheque" runat="server" Text="Cheque" GroupName="same2" />
                                            <span style="color: Red;">*</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblconbankname" runat="server" Text="Bank Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtconbankname" CssClass="textbox textbox1 txtheight1" Width="150px"
                                                runat="server" onfocus="return myFunction(this)"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txtconbankname"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblconbankbranch" runat="server" Text="Bank Branch"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtconbankbranch" CssClass="textbox textbox1 txtheight1" Width="150px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txtconbankbranch"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblconbank" runat="server" Text="Bank Account No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtconbank" MaxLength="16" CssClass="textbox textbox1 txtheight1"
                                                Width="150px" runat="server" onfocus="return myFunction(this)"></asp:TextBox>
                                            <span style="color: Red;">*</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txtconbank"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblconifsc" runat="server" Text="IFSC Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtconifsc" MaxLength="8" CssClass="textbox textbox1 txtheight1"
                                                Width="150px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txtconifsc"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblconswitft" runat="server" Text="SWIFT Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtconswift" MaxLength="15" CssClass="textbox textbox1 txtheight1"
                                                Width="150px" runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txtconswift"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="ChkLibrary" runat="server" AutoPostBack="true" Text="Library" OnCheckedChanged="ChkLibrary_OnCheckedChanged"
                                                Style="font-family: Book antiqua;" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                        <div style="float: left; width: 450px; height: 220px;">
                            <fieldset id="fildset" runat="server" visible="true" style="margin-top: 37px;">
                                <legend>Item Details
                                    <asp:Button ID="btncontant" runat="server" Text="?" OnClick="btnitm_click" CssClass="textbox btn" />
                                </legend>
                                <asp:Panel ID="Panelbind" runat="server" ScrollBars="Auto" Style="height: 150px;
                                    width: 400px;">
                                    <asp:GridView ID="SelectdptGrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-ForeColor="White" OnDataBound="OnDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Header">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_itemheader" runat="server" Text='<%# Eval("Item Header") %>'></asp:Label>
                                                    <asp:Label ID="lbl_itemheadercode" runat="server" Visible="false" Text='<%# Eval("Item Headercode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_itemcode" runat="server" Text='<%# Eval("Item Code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_itemname" runat="server" Text='<%# Eval("Item Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DeptName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_deptname" runat="server" Text='<%# Eval("Dept Name") %>'></asp:Label>
                                                    <asp:Label ID="lbl_deptcode" runat="server" Visible="false" Text='<%# Eval("Dept Code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Duration">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_duration" runat="server" Text='<%# Eval("Duration") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Supplied">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_supplied" runat="server" Text='<%# Eval("Supplied") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reference">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_reference" runat="server" Text='<%# Eval("Reference") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </fieldset>
                            <fieldset id="FldsetLibrary" runat="server" visible="false" style="margin-top: 37px;">
                                <legend>Library </legend>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Address2"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtAddress2" CssClass="textbox textbox1 txtheight1" Width="150px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" TargetControlID="TxtAddress2"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ,/-.">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="EmailID1"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtemailid1" CssClass="textbox textbox1 txtheight1" Width="150px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" runat="server" TargetControlID="Txtemailid1"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ,/-.">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="EmailID2"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtemailid2" CssClass="textbox textbox1 txtheight1" Width="150px"
                                                runat="server"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender32" runat="server" TargetControlID="Txtemailid2"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ,/-.">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="SupplierType"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSupplierType" runat="server" CssClass="textbox textbox1 ddlheight">
                                            <asp:ListItem Text="Agent"></asp:ListItem>
                                            <asp:ListItem Text="Local"></asp:ListItem>
                                            <asp:ListItem Text="Others"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </table>
                            </fieldset>
                        </div>
                        <div style="float: left; width: 450px; height: 220px;">
                            <fieldset style="margin-top: 37px;">
                                <legend>Contact Details
                                    <asp:Button ID="btnitm" runat="server" Text="?" OnClick="btncontact_click" CssClass="textbox btn" />
                                </legend>
                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Style="height: 150px; width: 400px;">
                                    <asp:GridView ID="ContactGrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                        HeaderStyle-ForeColor="White" OnRowDataBound="typegrid_OnRowDataBound" OnRowCommand="ContactGrid_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_name" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_designation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phone No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_phoneno" runat="server" Text='<%# Eval("Phone") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mobile No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_mobileno" runat="server" Text='<%# Eval("Mobile No") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fax No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_faxno" runat="server" Text='<%# Eval("Fax No") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_email" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="50px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </fieldset>
                        </div>
                        <br />
                        <br />
                        <center>
                            <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn2"
                                OnClientClick="return valid2()" OnClick="btn_update_Click" Visible="false" />
                            <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                                OnClientClick="return valid2()" OnClick="btn_delete_Click" Visible="false" />
                            <asp:Button ID="btn_save" runat="server" Text="Save" Visible="false" OnClick="btn_save_Click"
                                CssClass="textbox btn2" OnClientClick="return valid2()" />
                            <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click" />
                        </center>
                    </div>
                </center>
                <center>
                    <div id="popitm" runat="server" visible="false" style="height: 48em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 47px; margin-left: 457px;"
                            OnClick="imagebtnpopclose3_Click" />
                        <br />
                        <br />
                        <br />
                        <div style="background-color: White; height: 500px; width: 943px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <br />
                            <center>
                                <span style="color: Green; font-size: large;">Item Specification</span>
                            </center>
                            <br />
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_header" runat="server" Text="Header"></asp:Label>
                                        <span style="color: Red;">*</span>
                                        <fieldset>
                                            <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Style="height: 107px; width: 173px;">
                                                <asp:CheckBoxList ID="cbl_header" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_header_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_subheader" runat="server" Text="Sub Header"></asp:Label>
                                        <span style="color: Red;">*</span>
                                        <fieldset style="width: 207px;">
                                            <asp:Panel ID="P_subheader" runat="server" ScrollBars="Auto" Style="height: 107px;
                                                width: 207px;">
                                                <asp:CheckBoxList ID="cbl_subheader" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_subheader_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_item" runat="server" Text="Item"></asp:Label>
                                        <span style="color: Red;">*</span>
                                        <fieldset>
                                            <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" Style="height: 107px; width: 198px;">
                                                <asp:CheckBoxList ID="cblitem" runat="server">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_department" runat="server" Text="Department"></asp:Label>
                                        <fieldset>
                                            <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" Style="height: 109px; width: 210px;">
                                                <asp:CheckBoxList ID="cbldepartment" runat="server">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox3" runat="server" Font-Size="Medium" Text="Select All"
                                            Visible="false" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_subheader" runat="server" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cbsubheader_Change" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_conitmselect" runat="server" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cb_conitmselect_ChekedChange" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_departemt" runat="server" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="true" OnCheckedChanged="cbdepartment_Change" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_consup" runat="server" Text="Supply Duration (Days)"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_consup" CssClass="textbox textbox1 txtheight1" Width="100px"
                                            runat="server" MaxLength="2"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txt_consup"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_alreadysup" runat="server" Font-Size="Medium" Text="Already Supplied" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_reference" runat="server" Text="References"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_itmrefence" CssClass="textbox textbox1 txtheight1" Width="250px"
                                            runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txt_itmrefence"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <br />
                            <br />
                            <center>
                                <asp:Button ID="btn_save1" runat="server" Text="Save" OnClick="btn_save1_Click" CssClass="textbox btn2" />
                                <asp:Button ID="btn_exit1" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit1_Click" />
                            </center>
                        </div>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="popcon" runat="server" visible="false" class="popupstyle popupheight1">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 93px; margin-left: 343px;"
                    OnClick="imagebtnpopclose2_Click" />
                <br />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; width: 700px; height: 400px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <br />
                    <center>
                        <span style="color: Green; font-size: large;">Contact Details</span>
                    </center>
                    <br />
                    <br />
                    <center>
                        <table>
                            <tr style="display: none;">
                                <td>
                                    <asp:Label ID="lbl_contyp" runat="server" Text="Type"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddl_contyp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_contyp1_SelectedIndexChanged"
                                        CssClass="textbox cont">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="1">Others</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_contyp" CssClass="textbox textbox1" Style="color: #000066;"
                                        Width="75px" Visible="false" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" TargetControlID="txt_contyp"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_conname" runat="server" Text="Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_connam" CssClass="textbox textbox1 txtheight1" onfocus="return myFunction(this)"
                                        Width="250px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_connam"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_conph" runat="server" Text="Phone No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_conpn" MaxLength="13" CssClass="textbox textbox1 txtheight1"
                                        Width="150px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_conpn"
                                        FilterType="Numbers,custom" ValidChars="- ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_designation" runat="server" Text="Designation"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_designation" CssClass="textbox textbox1 txtheight1" onfocus="return myFunction(this)"
                                        Width="250px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_designation"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_conmob" runat="server" Text="Mobile No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_conmob" MaxLength="10" CssClass="textbox textbox1 txtheight1"
                                        onfocus="return myFunction(this)" Width="150px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txt_conmob"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_confax" runat="server" Text="Fax No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_confax" MaxLength="20" CssClass="textbox textbox1 txtheight1"
                                        Width="150px" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txt_confax"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_conemail" runat="server" Text="Email"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_conmail" CssClass="email textbox1 txtheight1" Width="150px"
                                        runat="server" onfocus="return myFunction(this)" onblur="return checkEmail(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_conmail"
                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=".@ ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <asp:Button ID="btn_congo" runat="server" Text="Save" OnClick="btn_congo_Click" CssClass="textbox btn2"
                                OnClientClick="return valid1()" />
                            <asp:Button ID="btn_conexist" runat="server" Text="Exit" CssClass="textbox btn2"
                                OnClick="btn_conexit_Click" />
                        </center>
                    </center>
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
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
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
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureno_Click" Text="no" runat="server" />
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
