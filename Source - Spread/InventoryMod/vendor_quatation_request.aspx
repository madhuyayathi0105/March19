<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="vendor_quatation_request.aspx.cs" Inherits="vendor_quatation_request" %>

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
            function display2() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function display() {
                document.getElementById('<%=lbl_error.ClientID %>').innerHTML = "";
            }
            function display1() {
                document.getElementById('<%=lbl_error.ClientID %>').innerHTML = "";
            }
            function Test1() {
                var quantity = document.getElementById("<%=txtpop1qnty.ClientID %>").value;
                var rateperunit = document.getElementById("<%=txtpop1rateunit.ClientID %>").value;

                if (quantity.trim() == "") {
                    quantity = document.getElementById("<%=txtpop1qnty.ClientID %>");
                    quantity.style.borderColor = 'Red';
                    empty = "E";

                }
                if (rateperunit.trim() == "") {
                    rateperunit = document.getElementById("<%=txtpop1rateunit.ClientID %>");
                    rateperunit.style.borderColor = 'Red';
                    empty = "E";

                }

                if (empty.trim() != "") {

                    return false;
                }
                else {
                    return true;
                }
            }
            function myFunction1(y) {
                y.style.borderColor = "#c4c4c4";
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
            //calculation
            function cal() {
                var quanity = document.getElementById("<%=txtpop1qnty.ClientID %>").value;
                var rateperunit = document.getElementById("<%=txtpop1rateunit.ClientID %>").value;
                var discount = document.getElementById("<%=txtpop1dia.ClientID %>").value;
                var tax = document.getElementById("<%=txtpop1tax.ClientID %>").value;
                var etax = document.getElementById("<%=txtpop1exetax.ClientID %>").value;
                var totalcost = document.getElementById("<%=txtpop1totalcost.ClientID %>");
                var othercharge = document.getElementById("<%=txtpop1otherchar.ClientID %>").value;
                var educess = document.getElementById("<%=txtpop1educess.ClientID %>").value;
                var highedu = document.getElementById("<%=txtpop1higher.ClientID %>").value;
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
                if (educess.trim() != "") {
                    totalvalue = totalvalue + parseFloat(educess);
                }
                if (highedu.trim() != "") {
                    totalvalue = totalvalue + parseFloat(highedu);
                }
                if (totalvalue != 0) {
                    totalcost.value = parseFloat(totalvalue.toFixed(2));
                }
                else {
                    totalcost.value = "";
                }
            }

        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Supplier Quotation For Request</span>
                    </div>
                    <br />
                </center>
            </div>
            <center>
                <div class="maindivstyle maindivstylesize">
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="Rad_item" AutoPostBack="true" runat="server" Text="Item" GroupName="d"
                                    OnCheckedChanged="Rad_item_Click" />
                            </td>
                            <td>
                                <asp:RadioButton ID="Rad_book" AutoPostBack="true" runat="server" Text="Book" GroupName="d"
                                    OnCheckedChanged="Rad_book_Click" />
                            </td>
                        </tr>
                    </table>
                    <table class="maintablestyle" style="width: 714px">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_vendor" runat="server" Text="Supplier Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_vendorname" runat="server" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" CssClass="multxtpanel" runat="server" Style="height: 200px;
                                            width: 200px; position: absolute;">
                                            <asp:CheckBox ID="cb_vendor" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_vendor_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_vendor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_vendor_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_vendorname"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblordercode" runat="server" Text="Supplier Quotation Code"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_basereqcode" runat="server" Width="131px" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" CssClass="multxtpanel" runat="server" Style="height: 150px;
                                            width: 150px; position: absolute;">
                                            <asp:CheckBox ID="cb_quocode" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_quocode_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_quocode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_quocode_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_basereqcode"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btn_basego" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_basego_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_baseaddnew" Text="Add new" runat="server" CssClass="textbox btn2"
                                    OnClick="btn_baseaddnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="lbl_base_error" Visible="false" runat="server" ForeColor="red" Text=""></asp:Label>
                        </div>
                    </center>
                    <div id="spreaddiv1" runat="server" visible="false" style="width: 975px; height: 372px;"
                        class="spreadborder">
                        <br />
                        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Width="968px" Height="350px" OnCellClick="Cell_Click" OnPreRender="FpSpread2_render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <center>
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <br />
                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Width="180px" Height="20px" onkeypress="display2()"
                                CssClass="textbox textbox1"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                                Width="127px" CssClass="textbox btn1" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                CssClass="textbox btn1" Width="60px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="popwindow2" runat="server" visible="false" class="popupstyle popupheight">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 31px; margin-left: 475px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 637px; width: 971px;" class="subdivstyle">
                        <br />
                        <span class="fontstyleheader" style="color: Green;">Vendor Quotation Entry</span>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="Radioitem" AutoPostBack="true" runat="server" Text="Item" GroupName="c"
                                        OnCheckedChanged="Radioitem_Click" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="RadioBook" AutoPostBack="true" runat="server" Text="Book" GroupName="c"
                                        OnCheckedChanged="RadioBook_Click" />
                                </td>
                            </tr>
                        </table>
                        <table class="maintable1 table">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_reqcode" runat="server" Text="Quotation Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_reqcode" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                        Format="dd/MM/yyyy">
                                        <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                    <asp:CalendarExtender ID="caltodate" TargetControlID="txt_todate" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_received" runat="server" Visible="false" Text="Received" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_notreceived" runat="server" Visible="false" Text="Not Received" />
                                </td>
                            </tr>
                            <tr>
                           <%-- Cmd Saran--%>
                               <%-- <td>
                                    <asp:Label ID="lbl_requestcode" runat="server" Text="Request Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_requestcode" runat="server" CssClass="textbox1 ddlheight3"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_requestcode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>--%>
                                <td>
                                    <asp:Label ID="lbl_vendorreq" runat="server" Text="Vendor Request Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_vendorreqcode" runat="server" CssClass="textbox1 ddlheight3"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_vendorreqcode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_vend" runat="server" Text="Vendor Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_venname" runat="server" placeholder="Vendor Name" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_search" Visible="false" runat="server" Text="Request ID"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_search" Visible="false" runat="server" placeholder="Search Request ID"
                                        CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="venreqcode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                    <asp:FilteredTextBoxExtender ID="ff" runat="server" TargetControlID="txt_search"
                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars="">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_selectvendor" Visible="false" runat="server" Text="Select Vendor"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <%--  <asp:DropDownList ID="ddl_vendor" runat="server" CssClass="textbox textbox1 ddlheight5">
                            </asp:DropDownList>--%>
                                    <asp:TextBox ID="txt_searchvendor" Visible="false" runat="server" CssClass="textbox textbox1 txtheight5"
                                        AutoPostBack="true" placeholder="Search Vendor Name" OnTextChanged="txt_searchvendor_txt_change"
                                        onkeypress="display1()"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getname1" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchvendor"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                    <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_searchvendor"
                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                            </asp:FilteredTextBoxExtender>--%>
                                    <asp:Button ID="btn_vendorqmark" Text="?" Visible="false" runat="server" CssClass="textbox btn"
                                        OnClick="btn_vendorqmark_Click" />
                                    <%-- <span  style="color: Red;">*</span>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_venquano" runat="server" Text="Vendor Quotation No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_venquano" runat="server" placeholder="Quotation No" CssClass="textbox textbox1 txtheight2"></asp:TextBox><%--onfocus="return color(this)"--%>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_venquano"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,custom" ValidChars="">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_duedate" runat="server" Text="Vendor Due Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_venduedate" runat="server" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_venduedate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <div>
                                <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </div>
                        </center>
                        <center>
                            <%--  <div id="spreaddiv" runat="server" visible="false" style="width: 960px; height: 350px;"
                            class="spreadborder">--%>
                            <FarPoint:FpSpread ID="FpSpread1" Visible="false" runat="server" Width="940px" Height="350px"
                                OnButtonCommand="btnType_Click" CssClass="spreadborder">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <%-- </div>--%>
                        </center>
                        <br />
                        <div>
                            <asp:Button ID="btnmain_save" Text="Save" Visible="false" runat="server" CssClass="textbox btn2"
                                OnClick="btnmainsave_Click" /><%--OnClientClick="return savefun()"--%>
                            <asp:Button ID="btn_delete" Text="Delete" Visible="false" runat="server" CssClass="textbox btn2"
                                OnClick="btn_deletevendorequest" />
                            <asp:Button ID="btn_exit" Text="Exit" Visible="false" runat="server" CssClass="textbox btn2"
                                OnClick="btn_exit_Click" />
                        </div>
                        <%-- </div>--%>
                    </div>
                </div>
            </center>
            <br />
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
            <center>
                <div>
                    <asp:GridView ID="gd_cost" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                        HeaderStyle-ForeColor="White">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost Per Quantity">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_gd_cost" runat="server" Text='<%# Eval("Cost") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Cast">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_gd_tocost" runat="server" Text='<%# Eval("Total Cost") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Button ID="btn_save" runat="server" Visible="false" Text="Save" CssClass="textbox btn1" />
                </div>
            </center>
            <%--  <center>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_payment" runat="server" Text="Payment "></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cb_initial" runat="server" Text="Initial" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cb_bal" runat="server" Text="Balance " />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="GridView1" runat="server">
                </asp:GridView>
                <br />
                <asp:Button ID="Button1" runat="server" CssClass="textbox btn1" />
            </div>
        </center>--%>
            <center>
                <div id="pop_vendor" runat="server" visible="false" style="height: 48em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 47px; margin-left: 405px;"
                        OnClick="imagebtnpopclose3_Click" />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 490px; width: 830px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <span style="color: Green; font-size: large;">Select the vendor</span>
                        </center>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_popsearchvendor" runat="server" Text="Select Vendor"></asp:Label>
                                </td>
                                <td>
                                    <%-- <asp:DropDownList ID="ddl_vendor" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>--%>
                                    <asp:TextBox ID="txt_popsearchvendor" runat="server" CssClass="textbox  txtheight5"
                                        AutoPostBack="true" OnTextChanged="txt_popsearchvendor_txt_change" onkeypress="display()"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getname1" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_popsearchvendor"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                    <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_popsearchvendor"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" >
                                </asp:FilteredTextBoxExtender>--%>
                                    <asp:Button ID="btn_popgo" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_popgo_Click" />
                                </td>
                            </tr>
                        </table>
                        <center>
                            <div>
                                <asp:Label ID="lbl_error2" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </center>
                        <br />
                        <div>
                            <center>
                                <div id="vendorsearch_div" runat="server" visible="false" style="width: 590px; height: 350px;"
                                    class="spreadborder">
                                    <FarPoint:FpSpread ID="FpSpread3" Visible="false" runat="server" Width="570px" Height="348px"
                                        OnCellClick="FpSpread3_CellClick" OnPreRender="FpSpread3_render">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </center>
                        </div>
                    </div>
                </div>
            </center>
            <center>
                <div id="popbtntypediv" runat="server" visible="false" class="popupstyle popupheight">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 71px; margin-left: 286px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="background-color: White; height: 340px; width: 650px;" class="subdivstyle">
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblpop1qnty" Text="Quantity" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpop1qnty" runat="server" CssClass="textbox textbox1 txtheight"
                                        MaxLength="6" Enabled="false" onfocus="return myFunction1(this)" onchange="return cal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtpop1qnty"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop1rateunit" Text="Rate Per Unit" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpop1rateunit" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight"
                                        onfocus="return myFunction1(this)" onchange="return cal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtpop1rateunit"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red">*</span>
                                    <%--</td>
                            <td>--%>
                                    <asp:Label ID="lbl_date" Text="Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_date" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_date" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbdis" runat="server" onchange="return change()" />
                                    <asp:Label ID="lblpop1dis" Text="Discount(%)" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpop1dia" runat="server" CssClass="textbox textbox1 txtheight"
                                        onchange="return cal()" MaxLength="6"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtpop1dia"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop1disamt" Text="Discount(Amt)" Visible="false" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpop1dis" runat="server" Visible="false" MaxLength="6" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtpop1dis"
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
                                    <asp:TextBox ID="txtpop1educess" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight"
                                        onchange="return cal()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtpop1educess"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblpop1higher" Text="Higher Education Cess" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpop1higher" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight"
                                        onchange="return cal()"></asp:TextBox>
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
                                <%-- <td>
                                <asp:Label ID="lblpop1dep" Text="Hostel Name" runat="server"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlpop1dep" runat="server" CssClass="textbox textbox1 ddlheight3"
                                    onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>--%>
                            </tr>
                        </table>
                        <br />
                        <div>
                            <asp:Button ID="btnpop1ok" Text="Ok" runat="server" CssClass="textbox btn2" OnClientClick="return Test1()"
                                OnClick="btnpopbtntypeok_Click" />
                            <asp:Button ID="btnpop1exit" Text="Exit" runat="server" CssClass="textbox btn2" OnClick="btnpop1Exit_Click" />
                        </div>
                    </div>
                </div>
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
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
