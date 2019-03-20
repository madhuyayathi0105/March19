<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="Inv_Transfer.aspx.cs" Inherits="Inv_Transfer" %>

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
            .maindivstylesize
            {
                height: 550px;
                width: 1000px;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <script type="text/javascript">
            function valid() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                id = document.getElementById("<%=ddl_hostel1.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "SELECT") {
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=ddl_popstore.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "SELECT") {
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=ddl_transdept.ClientID %>");
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

            function calculation() {
                var total = document.getElementById("<%=txt_totalQunatity.ClientID %>").value;
                var transqty = document.getElementById("<%=txt_transferqty.ClientID %>").value;
                var cal = "c";
                document.getElementById("<%=lblgreater.ClientID %>").innerHTML = parseFloat(total) >= parseFloat(transqty);

                if (cal.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function display1() {
                document.getElementById('<%=lblgreater.ClientID %>').innerHTML = "";
            }
        </script>
        <%--document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            document.getElementById("<%=txt_total1.ClientID %>").value = parseFloat(parseFloat(openstack) * parseFloat(rateperunit));
        document.getElementById("<%=lblgreater.ClientID %>").innerHTML = "Total Quantity Greater than Transquantity";
        --%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green">Stock Transfer</span></div>
                    </center>
                    <br />
                    <div class="maindivstyle maindivstylesize">
                        <br />
                        <center>
                            <div>
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_fromdate" runat="server" OnTextChanged="txt_fromdate_TextChanged"
                                                AutoPostBack="true" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight1"
                                                OnTextChanged="txt_todate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:CalendarExtender ID="caltodate" TargetControlID="txt_todate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            Search By
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_option" runat="server" CssClass="textbox1 ddlheight1" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_option_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Mess Name" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Store Name" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Department" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_hostelname" runat="server" Visible="false" Text="Mess Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upp1" Visible="false" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_hosname" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                    <asp:Panel ID="p1" runat="server" CssClass="multxtpanel" Height="150px" Width="160px">
                                                        <asp:CheckBox ID="cb_hos" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_hostel_CheckedChange" />
                                                        <asp:CheckBoxList ID="cbl_hos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostel_SelectedIndexChange">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_hosname"
                                                        PopupControlID="p1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblstorename" runat="server" Visible="false" Text="Store Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_basestore" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="150px">
                                                        <asp:CheckBox ID="cb_mainstore" runat="server" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cb_mainstore_CheckedChange" />
                                                        <asp:CheckBoxList ID="cbl_mainstore" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_mainstore_SelectedIndexChange">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_basestore"
                                                        PopupControlID="Panel1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_degree" Visible="false" Text="Department" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Upp4" Visible="false" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3">-- Select--</asp:TextBox>
                                                    <asp:Panel ID="p3" runat="server" CssClass="multxtpanel" Width="250px" Height="180px">
                                                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cb_degree_checkedchange" />
                                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_degree"
                                                        PopupControlID="p3" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_addnew" Text="Add New" runat="server" CssClass="textbox btn2"
                                                OnClick="btn_addnew_Click" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <%--  <div style="text-align: left; text-indent: 150px; font-size: medium;">--%>
                                <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                <br />
                                <%--<center>--%>
                                <div id="spreaddiv1" runat="server" visible="false" style="width: 824px; height: 372px;"
                                    class="spreadborder">
                                    <br />
                                    <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Width="776px" Height="320px">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                                <%--   </center>--%>
                                <br />
                                <%-- <div id="rptprint" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                    Visible="false" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                <br />
                                <asp:Label ID="lblrptname" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Width="180px" Height="20px" onkeypress="display()"
                                    CssClass="textbox textbox1"></asp:TextBox>
                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                                    Width="127px" CssClass="textbox btn1" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    CssClass="textbox btn1"  Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </div>--%>
                                <div id="rptprint" runat="server" visible="false">
                                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                        Width="180px" onkeypress="display()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                        Text="Export To Excel" Width="127px" />
                                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                        CssClass="textbox btn1" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                </div>
                                <br />
                                <%-- </div>--%>
                            </div>
                        </center>
                    </div>
                </div>
            </center>
            <center>
                <div id="popwindow" runat="server" visible="false" style="height: 45em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 530px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 570px; width: 1090px; border: 3px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <span style="color: #008080; font-size: x-large;">Stock Transfer</span>
                        <br />
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="Radio_ManTrans" AutoPostBack="true" Checked="true" runat="server"
                                            Text="Transfer" GroupName="ff" OnCheckedChanged="Radio_ManTrans_Click" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="Radio_ReqTrans" AutoPostBack="true" runat="server" Text="Request Transfer"
                                            GroupName="ff" OnCheckedChanged="Radio_ReqTrans_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <center>
                            <table id="Trans" runat="server" visible="true" class="maintablestyle" style="width: 960px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbltranstype" runat="server" Text="Tranfer Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_transtype" CssClass="textbox textbox1 ddlheight3" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_transtype_Selected_indexchange">
                                            <asp:ListItem Value="0">Store to Mess</asp:ListItem>
                                            <asp:ListItem Value="1">Mess to Mess</asp:ListItem>
                                            <asp:ListItem Value="2">Store to Store</asp:ListItem>
                                            <asp:ListItem Value="3">Store to Department</asp:ListItem>
                                            <asp:ListItem Value="4">Department to Department</asp:ListItem>
                                            <asp:ListItem Value="5">Department to Store</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="display: none;">
                                        <asp:RadioButton ID="rdb_showall" AutoPostBack="true" runat="server" Text="Show All"
                                            GroupName="da" OnCheckedChanged="rdb_showall_Click" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="rdb_datewise" AutoPostBack="true" runat="server" Text="Datewise"
                                            OnCheckedChanged="rdb_datewise_Click" />
                                        <asp:RadioButton ID="rdo_acodomicdept" Visible="false" AutoPostBack="true" runat="server"
                                            Text="Academic" GroupName="s2dep" OnCheckedChanged="rdo_acodomicdept_oncheckedchange" />
                                        <asp:RadioButton ID="rdo_nonacodept" Visible="false" AutoPostBack="true" runat="server"
                                            Text="Non Non-Academic" GroupName="s2dep" OnCheckedChanged="rdo_nonacodept_oncheckedchange" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdb_hostohos" AutoPostBack="true" runat="server" Text="Mess To Mess"
                                            GroupName="da" OnCheckedChanged="rdb_hostohos_Click" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblpopfromdate" runat="server" Text="From Date"></asp:Label>
                                        <asp:Label ID="lbl_hostelname3" runat="server" Text="Mess Name"></asp:Label>
                                        <asp:Label ID="lbl_storename" Visible="false" runat="server" Text=" Store Name"></asp:Label>
                                        <asp:Label ID="lbl_depttxt" Visible="false" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_hostelname3" CssClass="textbox textbox1 ddlheight3" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_hostel_Selected_indexChange">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_storename" Visible="false" CssClass="textbox textbox1 ddlheight3"
                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_storename_Selected_indexChange">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_acadamic" Visible="false" CssClass="textbox textbox1 ddlheight5"
                                            runat="server" OnSelectedIndexChanged="ddl_acadamic_selected_indexchange">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtpopfrom" runat="server" AutoPostBack="true" Width="80px" CssClass="textbox textbox1 txtheight1"
                                            ForeColor="Black" OnTextChanged="txtfrom_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtpopfrom" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%-- CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_storesearch" Text="Search by" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lbl_to" runat="server" Text="To Date"></asp:Label>
                                        <asp:TextBox ID="txt_to1" runat="server" AutoPostBack="true" Width="80px" ForeColor="Black"
                                            CssClass="textbox textbox1 txtheight1" OnTextChanged="txtto_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_to1" runat="server"
                                            Format="dd/MM/yyyy">
                                            <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_storetostore" runat="server" placeholder="Search store name"
                                            CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_storetostore"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=". ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="storename" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_storetostore"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txt_storetostore">
                                        </asp:AutoCompleteExtender>
                                        <asp:Label ID="lbl_itemsearch" runat="server" Text="Item Name"></asp:Label>
                                        <asp:TextBox ID="txt_searchitem" placeholder="Search Item Name" CssClass="textbox textbox1"
                                            runat="server"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchitem"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go1" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btn_transfergo_Click" />
                                    </td>
                                </tr>
                                <%-- <tr>
                                <td>
                                    <asp:Label ID="lbl_itemname1" Text="Item Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_item" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Itemname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_item"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>                                
                            </tr> --%>
                            </table>
                            <table id="Reqtrans" runat="server" visible="false" class="maintablestyle" style="width: 700px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Tranfer Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_transtype1" CssClass="textbox textbox1 ddlheight3" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_transtype1_Selected_indexchange">
                                            <asp:ListItem Value="0">Store</asp:ListItem>
                                            <asp:ListItem Value="1">Mess</asp:ListItem>
                                            <asp:ListItem Value="2">Department</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Respective Staff"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_Staff" CssClass="textbox textbox1 ddlheight3" runat="server"
                                            AutoPostBack="true" Width="220px" OnSelectedIndexChanged="ddl_Staff_Selected_indexchange">
                                        </asp:DropDownList>
                                    </td>
                                  <%--  <td>
                                        <asp:CheckBox ID="chk_date" AutoPostBack="true" runat="server" Text="Datewise" OnCheckedChanged="chk_date_Click" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="From Date"></asp:Label>
                                        <asp:TextBox ID="TextBox1frm" runat="server" AutoPostBack="true" Width="80px" CssClass="textbox textbox1 txtheight1"
                                            ForeColor="Black" OnTextChanged="txtfrom_TextChanged1" Enabled="false"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="TextBox1frm" runat="server"
                                            Format="dd/MM/yyyy">
                                            
                                       </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="To Date"></asp:Label>
                                        <asp:TextBox ID="TextBox2to" runat="server" AutoPostBack="true" Width="80px" ForeColor="Black"
                                            CssClass="textbox textbox1 txtheight1" Enabled="false" OnTextChanged="txtto_TextChanged1"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="TextBox2to" runat="server"
                                            Format="dd/MM/yyyy"> 
                                        </asp:CalendarExtender>
                                    </td>--%>
                                    <td>
                                        <asp:Button ID="btn_transgo" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btn_transgo_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="lbl_error1" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            <div id="spreaddiv" runat="server" style="width: 919px; height: 350px;" class="spreadborder">
                                <FarPoint:FpSpread ID="FpSpread1" runat="server" Width="900px" Height="350px" OnCellClick="Cell_Click"
                                    OnPreRender="FpSpread1_Render">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                            <br />
                            <asp:Button ID="btn_Transfer" runat="server" Text="Transfer" Visible="true" CssClass="textbox btn2"
                                OnClick="btn_Transfer_Click" />
                            <asp:Button ID="btn_exit1" Text="Exit" runat="server" CssClass="textbox btn2" OnClick="btn_exit1_Click" />
                        </center>
                    </div>
                </div>
            </center>
            <center>
                <div id="popwindow3" runat="server" visible="false" style="height: 40em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 93px; margin-left: 242px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 324px; width: 508px; border: 3px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_fromhostel" Text="From Mess" Visible="false" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="bhosname" Text="" ForeColor="#0ca6ca" Visible="false" Height="23px"
                                        runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostel1" Text="" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_hostel1" Width="180px" runat="server" AutoPostBack="true"
                                        onfocus="return myFunction(this)" CssClass="textbox ddlstyle" Height="30px">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_popstore" runat="server" AutoPostBack="true" onfocus="return myFunction(this)"
                                        CssClass="textbox ddlstyle ddlheight3">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddl_transdept" runat="server" AutoPostBack="true" Width="180px"
                                        Height="30px" onfocus="return myFunction(this)" CssClass="textbox ddlstyle">
                                    </asp:DropDownList>
                                    <span style="color: Red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_itemname" Text="Item Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_itemname" runat="server" CssClass="textbox textbox1 txtheight2"
                                        onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_itemname"
                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=". ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_itemmeasure" Text="Item Measure" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_itemmeasure" runat="server" CssClass="textbox textbox1 txtheight2"
                                        onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_itemmeasure"
                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=". ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_totalqty" Text="Total Quantity" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_totalQunatity" runat="server" Enabled="false" CssClass="textbox textbox1 txtheight1"
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
                                        CssClass="textbox textbox1 txtheight1" onkeypress="display1()" Width="80px" OnTextChanged="transchange"
                                        AutoPostBack="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_transferqty"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lblgreater" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_date" Text="Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_transferdate" runat="server" CssClass="textbox textbox1 txtheight1"
                                        Width="80px"></asp:TextBox>
                                    <asp:CalendarExtender ID="calfromdate" TargetControlID="txt_transferdate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="div3" visible="false" runat="server">
                            <asp:Button ID="btn_newadd" runat="server" Text="Add" Visible="false" CssClass="textbox btn2"
                                OnClick="btn_newadd_Click" OnClientClick="return valid()" />
                            <asp:Button ID="btn_ex" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_ex_Click" />
                        </div>
                    </div>
                </div>
            </center>
            <center>
                <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="lbl_alerterror" Visible="false" runat="server" Text="" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass=" textbox btn2 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />
                                                <%-- <asp:ImageButton ID="btn_errorclose" Style="height: 40px; width: 40px;" OnClick="btn_errorclose_Click"
                                                ImageUrl="~/images/okimg.jpg" runat="server" />--%>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
