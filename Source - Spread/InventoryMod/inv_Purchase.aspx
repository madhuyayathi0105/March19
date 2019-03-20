<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="inv_Purchase.aspx.cs" Inherits="inv_Purchase" %>

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
    </head>
    <body>

        <script type="text/javascript">
            function Test() {
                var ordercode = document.getElementById("<%=txtordercode.ClientID %>").value;
                var orderdate = document.getElementById("<%=txtorderdate.ClientID %>").value;
                var vendorname = document.getElementById("<%=txtvendorname.ClientID %>").value;
                var totalcost = document.getElementById("<%=txttotalcost.ClientID %>").value;
                var empty = "";
                if (ordercode.trim() == "") {
                    ordercode = document.getElementById("<%=txtordercode.ClientID %>");
                    ordercode.style.borderColor = 'Red';
                    empty = "E";
                }
                if (orderdate.trim() == "") {
                    orderdate = document.getElementById("<%=txtorderdate.ClientID %>");
                    orderdate.style.borderColor = 'Red';
                    empty = "E";
                }
                if (vendorname.trim() == "") {
                    vendorname = document.getElementById("<%=txtvendorname.ClientID %>");
                    vendorname.style.borderColor = 'Red';
                    empty = "E";
                }
                if (totalcost.trim() == "") {
                    totalcost = document.getElementById("<%=txttotalcost.ClientID %>");
                    totalcost.style.borderColor = 'Red';
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
            /*calculation*/
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

            function cal1() {

                var discount = document.getElementById("<%=txtper.ClientID %>").value;

                var tax = document.getElementById("<%=txttax.ClientID %>").value;

                var othercharges = document.getElementById("<%=txtothercharges.ClientID %>").value;

                var round = document.getElementById("<%=txtround.ClientID %>").value;

                var Advancepayment = document.getElementById("<%=txtadpay.ClientID %>").value;

                var dummyno = document.getElementById("<%=txtdummyno.ClientID %>").value;

                var totalcost = document.getElementById("<%=txttotalcost.ClientID %>").value;

                var totalcost1 = document.getElementById("<%=txttotalcost.ClientID %>");

                var balcost = document.getElementById("<%=txtbalcost.ClientID %>");

                var idnew = document.getElementById('<%=cbxamount.ClientID %>');

                var idnew1 = document.getElementById('<%=chkround.ClientID %>');

                var totalvalue = 0;



                //                if (totalcost.trim() != "") {
                //                    totalvalue = parseFloat(parseFloat(totalcost));
                //                  }

                if (dummyno.trim() != "") {
                    totalvalue = parsefloat(parsefloat(dummyno));
                }

                if (idnew.checked) {
                    if (discount.trim() != "") {
                        totalvalue = parseFloat(totalcost);
                        totalvalue = totalvalue - parseFloat(discount);

                    }
                }
                else {
                    if (discount.trim() != "") {
                        var dis = parseFloat(totalvalue / 100) * parseFloat(discount);
                        totalvalue = totalvalue - parseFloat(dis);
                    }
                }

                if (idnew1.checked) {
                    if (round.trim() != "") {
                        totalvalue = totalvalue + parseFloat(round);
                    }
                }
                else {
                    if (round.trim() != "") {
                        totalvalue = totalvalue - parseFloat(round);
                    }
                }

                if (tax.trim() != "") {
                    var t = parseFloat(totalvalue / 100) * parseFloat(tax);
                    totalvalue = totalvalue + parseFloat(t);
                }
                if (othercharges.trim() != "") {
                    totalvalue = totalvalue + parseFloat(othercharges);
                }
                if (totalvalue != 0) {
                    totalcost1.value = parseFloat(totalvalue.toFixed(2));
                    balcost.value = parseFloat(totalvalue.toFixed(2))
                }
                else {
                    totalcost1.value = "";
                }
                if (Advancepayment.trim() != "") {
                    balcost.value = parseFloat(parseFloat(totalvalue) - parseFloat(Advancepayment)).toFixed(2);
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

            function change1() {
                var idnew = document.getElementById('<%=cbxamount.ClientID %>');
                if (idnew.checked) {
                    document.getElementById('<%=lbldiscount.ClientID %>').innerHTML = "Discount(Amt)";
                }
                else {
                    document.getElementById('<%=lbldiscount.ClientID %>').innerHTML = "Discount(%)";
                }
            }

            function change2() {
                var idnew = document.getElementById('<%=chkround.ClientID %>');
                if (idnew.checked) {
                    document.getElementById('<%=lbl.ClientID %>').innerHTML = "(+)";
                }
                else {
                    document.getElementById('<%=lbl.ClientID %>').innerHTML = "(-)";
                }
            }

            function change3(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_brandname.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_brandname.ClientID %>");
                    idval.style.display = "none";
                }
            }
        </script>

        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">Purchase Order Entry</span>
            <br />
            <br />
        </center>
        <center>
            <div class="maindivstyle" style="height: 595px; width: 1000px;">
                <center>
                    <center>
                        <div>
                            <%--<div id="popwindow2" runat="server" visible="false" class="popupstyle popupheight">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 31px; margin-left: 475px;"
                        OnClick="imagebtnpopclose_Click" />--%>
                            <%--  <div style="background-color: White; height: 593px; width: 971px;" class="subdivstyle">--%>
                            <br />
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="Rad_item" AutoPostBack="true" runat="server" Checked="true"
                                                Text="Item" GroupName="d" OnCheckedChanged="Rad_item_Click" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="Rad_book" AutoPostBack="true" runat="server" Text="Book" GroupName="d"
                                                OnCheckedChanged="Rad_book_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="box-shadow: 0px 0px 8px #999999; border-radius: 5px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblordercode" Text="Order Code" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtordercode" runat="server" Enabled="false" CssClass="textbox textbox1 txtheight "
                                                onfocus="return myFunction(this)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblpurchaseorderdate" Text="Purchase Order Date" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtorderdate" runat="server" CssClass="textbox textbox1 txtheight"
                                                onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:CalendarExtender ID="calorderdate" TargetControlID="txtorderdate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldeliverrydate" Text="Delivery Date" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_deliverydate" runat="server" CssClass="textbox textbox1 txtheight"
                                                Width="80px" onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_deliverydate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                            <span style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblvendorname" Text="" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtvendorname" AutoPostBack="true" runat="server" Width="410px"
                                                Height="20px" CssClass="textbox textbox1" OnTextChanged="txtvendorname_base_onchange"
                                                onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getname1" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtvendorname"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="txtsearchpan">
                                            </asp:AutoCompleteExtender>
                                            <asp:FilteredTextBoxExtender ID="filtertextbox" runat="server" TargetControlID="txtvendorname"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <%--invitem--%>
                                            <asp:TextBox ID="txt_invitem" AutoPostBack="true" runat="server" Width="410px" Height="20px"
                                                CssClass="textbox textbox1" OnTextChanged="txt_invitem_base_onchange"></asp:TextBox>
                                            <%-- <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="inv_itemname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_invitem"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>--%>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txt_invitem"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <%--invitem end--%>
                                            <%--request& purchase--%>
                                            <asp:TextBox ID="txt_requestcode" AutoPostBack="true" placeholder="Please Select Request Code"
                                                runat="server" Width="410px" Height="20px" CssClass="textbox textbox1" OnTextChanged="txt_requestcode_base_onchange"></asp:TextBox>
                                            <%-- <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="requestpo" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_requestcode"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>--%>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_requestcode"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <%--request & end--%>
                                            <%--appvendor--%>
                                            <asp:TextBox ID="txt_appven" AutoPostBack="true" runat="server" Width="410px" Height="20px"
                                                CssClass="textbox textbox1" OnTextChanged="txt_appven_base_onchange"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="app_vendorsearch" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_appven"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="txtsearchpan">
                                            </asp:AutoCompleteExtender>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txt_requestcode"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <%--end appvendor--%>
                                            <asp:Button ID="btnadd" Text="?" runat="server" CssClass="textbox btn" Visible="true"
                                                OnClick="btnadd_Click" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblduedate" Text="Due Date" runat="server"></asp:Label>
                                            <asp:CheckBox ID="chkboxdue" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxduedate_Click" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtduedate" runat="server" CssClass="textbox textbox1 txtheight"
                                                onfocus="return myFunction(this)"></asp:TextBox>
                                            <asp:CalendarExtender ID="calduedate" TargetControlID="txtduedate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%-- CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_baseGo" Text="Go" Visible="true" runat="server" CssClass="textbox btn1"
                                                OnClick="btn_basego_click" />
                                            <asp:Button ID="btnselect" Text="Select" Visible="false" runat="server" CssClass="textbox btn2"
                                                OnClick="btnselect_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Label ID="lbl_baseerror" Text="" runat="server" Visible="false" ForeColor="red" />
                            </center>
                            <br />
                            <center>
                                <div class="spreadborder" style="height: 300px; width: 960px; background-color: White;">
                                    <FarPoint:FpSpread ID="Fpspread3" runat="server" Visible="true" BorderStyle="NotSet"
                                        BorderWidth="0px" Width="960px" OnCellClick="Cell_Click" OnPreRender="Fpspread3_render">
                                        <sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="LightBlue">
                                            </FarPoint:SheetView>
                                        </sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbxamount" onchange="return change1()" runat="server" Font-Size="Small" />
                                        <asp:Label ID="lbldiscount" Text="Discount(%)" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtper" runat="server" CssClass="textbox textbox1 txtheight" onchange="return cal1()"
                                            MaxLength="6"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filtertextbox1" runat="server" TargetControlID="txtper"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltax" Text="Tax(%)" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttax" runat="server" CssClass="textbox textbox1 txtheight" onblur="return cal1()"
                                            MaxLength="6"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txttax"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblothercharges" Text="Other Charges" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtothercharges" runat="server" CssClass="textbox textbox1 txtheight"
                                            onblur="return cal1()" MaxLength="6"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtothercharges"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldes" Text="Description" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdes" runat="server" CssClass="textbox textbox1" Width="232px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txtdes"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbltotalcost" Text="Total Cost" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttotalcost" runat="server" CssClass="textbox textbox1 txtheight"
                                            MaxLength="8" onfocus="return myFunction(this)"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txttotalcost"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red">*</span>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkround" Text="Round" onchange="return change2()" runat="server" />
                                        <asp:Label ID="lbl" Text="(-)" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtround" runat="server" CssClass="textbox textbox1 txtheight" onblur="return cal1()"
                                            MaxLength="6"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtround"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAdpay" Text="Advance Payment" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtadpay" runat="server" CssClass="textbox textbox1 txtheight" onblur="return cal1()"
                                            MaxLength="6"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtadpay"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbalcost" Text="Balance Cost" runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtbalcost" runat="server" Enabled="false" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                        <asp:TextBox ID="txtdummyno" runat="server" Style="display: none;" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtbalcost"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Label ID="lblpage" Text="Page No" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtpageno" runat="server" CssClass="textbox textbox1 txtheight"
                                            MaxLength="6"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtpageno"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkhostel" Text="For Hostel" Visible="false" runat="server" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlhostel" runat="server" Visible="false" CssClass="textbox textbox1 ddlheight3">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="textbox btn2" OnClick="btnsave_Click"
                                            OnClientClick="return Test()" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnUpdate" Text="Update" runat="server" Visible="false" CssClass="textbox btn2" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnexit" Visible="false" Text="Exit" runat="server" CssClass="textbox btn2"
                                            OnClick="btnexit_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Button_Backpage" Visible="false" Text="<- Back" BackColor="Green"
                                            runat="server" CssClass="textbox btn2" OnClick="Button_Backpage_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
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
                                                <FarPoint:FpSpread ID="FpSpread1" Visible="false" runat="server" Width="570px" Height="348px"
                                                    OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_render">
                                                    <sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </sheets>
                                                </FarPoint:FpSpread>
                                            </div>
                                        </center>
                                    </div>
                                </div>
                            </div>
                        </center>
                        <%--</div>--%>
                        <div>
                        </div>
                        <center>
                            <div id="popwindow1" runat="server" visible="false" class="popupstyle popupheight">
                                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                    Style="height: 30px; width: 30px; position: absolute; margin-top: 70px; margin-left: 313px;"
                                    OnClick="imagebtnpopclose2_Click" />
                                <br />
                                <br />
                                <br />
                                <br />
                                <div style="background-color: White; height: 435px; width: 650px;" class="subdivstyle">
                                    <br />
                                    <span style="color: #008000; font-size: large;">Item for Purchase</span>
                                    <br />
                                    <table style="margin-left: 10px; position: absolute;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblven" Text="Vendor" Style="top: 15px; left: 10px; position: absolute;"
                                                    runat="server">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="Upp6" runat="server">
                                                    <contenttemplate>
                                                        <asp:TextBox ID="txtvendor" runat="server" Style="top: 10px; left: 65px; position: absolute;"
                                                            CssClass="textbox textbox1 txtheight4">--Select--</asp:TextBox>
                                                        <asp:Panel ID="p6" runat="server" CssClass="multxtpanel" Style="height: 250px; width: 250px;">
                                                            <asp:CheckBox ID="Chkven" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="Chksechosname" />
                                                            <asp:CheckBoxList ID="Cblven" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cblsechosname">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="Pop6" runat="server" TargetControlID="txtvendor" PopupControlID="p6"
                                                            Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </contenttemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblitm" Text="Item Name" Width="100px" Style="top: 15px; left: 266px;
                                                    position: absolute;" runat="server">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upp7" runat="server">
                                                    <contenttemplate>
                                                        <asp:TextBox ID="txtitmname" runat="server" CssClass="textbox textbox1 txtheight4"
                                                            Style="top: 10px; left: 350px; position: absolute;">--Select--</asp:TextBox>
                                                        <asp:Panel ID="p7" runat="server" CssClass="multxtpanel" Style="width: 150px;" Height="200px">
                                                            <asp:CheckBox ID="Chkitm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbitem_change" />
                                                            <asp:CheckBoxList ID="Cblitm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cblitmname">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="Pop7" runat="server" TargetControlID="txtitmname" PopupControlID="p7"
                                                            Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </contenttemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="btngo_vendoritem" runat="server" Text="Go" Style="top: 10px; left: 535px;
                                                    position: absolute;" CssClass="textbox1 textbox btn1" OnClick="btngo_vendoritem_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                    <br />
                                    <div>
                                        <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderColor="Black"
                                            BorderStyle="Solid" BorderWidth="1px" Height="270px" Width="520px" class="spreadborder"
                                            OnUpdateCommand="Fpspread2_Command">
                                            <sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                    <div>
                                        <br />
                                        <asp:Button ID="btnpurchase" Text="Purchase" runat="server" CssClass="textbox btn2"
                                            OnClick="btnpurchase_Click" />
                                        <asp:Button ID="btnpopwin1exit" Text="Exit" runat="server" CssClass="textbox btn2"
                                            OnClick="btnpop1winexit_Click" />
                                    </div>
                                </div>
                            </div>
                        </center>
                        <center>
                            <div id="popbtntypediv" runat="server" visible="false" class="popupstyle popupheight">
                                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                    Style="height: 30px; width: 30px; position: absolute; margin-top: 71px; margin-left: 303px;"
                                    OnClick="imagebtnpopclose1_Click" />
                                <br />
                                <br />
                                <br />
                                <br />
                                <div style="background-color: White; height: 334px; width: 665px;" class="subdivstyle">
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
                                            <td>
                                                <asp:TextBox ID="txtpop1rateunit" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight"
                                                    onfocus="return myFunction1(this)" onchange="return cal()"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtpop1rateunit"
                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red">*</span>
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
                                                <asp:Label ID="lblpop1dis" Text="Discount(Amt)" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpop1dia" runat="server" CssClass="textbox textbox1 txtheight"
                                                    onchange="return cal()" MaxLength="6"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtpop1dia"
                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblpop1disamt" Text="Discount(%)" Visible="false" runat="server"></asp:Label>
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
                        </center>
                        <%--individual item for vendor--%>
                        <div id="pop_individualitem" runat="server" visible="false" class="popupstyle popupheight">
                            <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 433px;"
                                OnClick="imagebtnpopclose4_Click" />
                            <br />
                            <div class="subdivstyle" style="background-color: White; height: 578px; width: 900px;">
                                <br />
                                <div>
                                    <asp:Label ID="lbl_selectitem3" runat="server" Style="font-size: large; color: Green;"
                                        Text="Select the Item" Font-Bold="true"></asp:Label>
                                </div>
                                <br />
                                <%-- <asp:UpdatePanel ID="upp4" runat="server">
                                <ContentTemplate>--%>
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_itemtype3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_requestcode" Visible="false" runat="server" CssClass="textbox1 ddlheight2"
                                                OnSelectedIndexChanged="ddl_requestcode_selectedindexchange" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <contenttemplate>
                                                    <asp:TextBox ID="txt_reqcode" Visible="false" runat="server" CssClass="textbox" ReadOnly="true"
                                                        Width="106px" Height="20px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel1" runat="server" Visible="false" CssClass="multxtpanel" Style="height: 200px;
                                                        width: 150px;">
                                                        <asp:CheckBox ID="cb_request" runat="server" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cb_request_checkchange" />
                                                        <asp:CheckBoxList ID="cbl_request" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_request_selectedindexchange">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_reqcode"
                                                        PopupControlID="Panel1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </contenttemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_2itemname" runat="server" Visible="false" Text="Item Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Upp5" runat="server">
                                                <contenttemplate>
                                                    <asp:TextBox ID="txt_ind_item" Visible="false" runat="server" CssClass="textbox"
                                                        ReadOnly="true" Width="106px" Height="20px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="p51" runat="server" Visible="false" CssClass="multxtpanel" Style="height: 200px;
                                                        width: 150px;">
                                                        <asp:CheckBox ID="cb_invitem" runat="server" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cb_invitem_checkchange" />
                                                        <asp:CheckBoxList ID="cb1_invitem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_invitem_selectedindexchange">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupExt51" runat="server" TargetControlID="txt_ind_item"
                                                        PopupControlID="p51" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </contenttemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_searchitem" runat="server" Text="Search Item Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_search_itemname" Visible="false" 
                                                runat="server" CssClass="textbox textbox1" Height="20px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="inv_itemname" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search_itemname"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="txtsearchpan">
                                            </asp:AutoCompleteExtender>
                                          
                                            <asp:TextBox ID="txt_reqitemsearch" Visible="false" placeholder="Search Item Name"
                                                runat="server" CssClass="textbox textbox1" Height="20px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="requestsearchitem" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_reqitemsearch"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="txtsearchpan">
                                            </asp:AutoCompleteExtender>
                                            <asp:TextBox ID="txt_appsearchpop" Visible="false" 
                                                runat="server" CssClass="textbox textbox1" Height="20px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="appvenitems" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_appsearchpop"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="txtsearchpan">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_invitem" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btn_goinvitem_click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <center>
                                    <asp:Label ID="lbl_inverror" Visible="false" runat="server" ForeColor="red"></asp:Label>
                                </center>
                                <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                                <br />
                                <div id="div2" runat="server" visible="false" style="width: 850px; height: 362px;
                                    background-color: White;" class="spreadborder">
                                    <div style="width: 550px; float: left;">
                                        <br />
                                        <asp:DataList ID="gvdatass" runat="server" Font-Size="Medium" RepeatColumns="4" Width="500px"
                                            ForeColor="#333333">
                                            <AlternatingItemStyle BackColor="White" />
                                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                            <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="CheckBox2" AutoPostBack="true" OnCheckedChanged="selectedmenuchk"
                                                                runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_itemname" ForeColor="Green" runat="server" Text='<%# Eval("itemname") %>'></asp:Label>
                                                            <asp:Label ID="lbl_itemcode" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("itemcode") %>'></asp:Label>
                                                            <asp:Label ID="lbl_itempk" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("itempk") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                        </asp:DataList>
                                    </div>
                                    <div style="width: 200px; float: right;">
                                        <br />
                                        <asp:GridView ID="selectitemgrid" runat="server" HeaderStyle-BackColor="#0CA6CA"
                                            AutoGenerateColumns="false" HeaderStyle-ForeColor="White">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="snogv" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="itemnamegv" runat="server" Text='<%# Eval("Item Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderWidth="1px" Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Code" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="itemcodegv" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'> </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />--%>
                                                <asp:TemplateField HeaderText="Item Headername" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_headername" ForeColor="Green" Visible="false" runat="server" Text='<%# Eval("Item PK") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <br />
                                <center>
                                    <asp:Button ID="btn_selecteditem" Visible="false" CssClass="textbox btn2" OnClick="btn_selecteditem_Click"
                                        Text="Ok" runat="server" />
                                    <asp:Button ID="btn_exit2" Visible="false" CssClass="textbox btn2" OnClick="btn_exit2_Click"
                                        Text="Exit" runat="server" />
                                </center>
                            </div>
                        </div>
                        <br />
                        <center>
                            <div id="selectvendor_div" runat="server" visible="false" class="popupstyle popupheight">
                                <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                    Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 433px;"
                                    OnClick="imagebtnpopclose4_Click" />
                                <br />
                                <div class="subdivstyle" style="background-color: White; height: 578px; width: 900px;">
                                    <br />
                                    <div>
                                        <asp:Label ID="Label1" runat="server" Style="font-size: large; color: Green;" Text="Select the Vendor"
                                            Font-Bold="true"></asp:Label>
                                        <br />
                                        <br />
                                        <div>
                                            <FarPoint:FpSpread ID="Fpspread4" runat="server" Visible="false" BorderColor="Black"
                                                BorderStyle="Solid" BorderWidth="1px" Height="350px" Width="620px" class="spreadborder">
                                                <sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </sheets>
                                            </FarPoint:FpSpread>
                                        </div>
                                        <br />
                                        <br />
                                        <center>
                                            <div>
                                                <asp:Button ID="btn_vendorselect" Visible="false" CssClass="textbox btn2" OnClick="btn_vendorselect_Click"
                                                    Text="Ok" runat="server" />
                                                <asp:Button ID="btn_vendorExit" Visible="false" CssClass="textbox btn2" OnClick="btn_vendorExit_Click"
                                                    Text="Exit" runat="server" />
                                            </div>
                                        </center>
                                    </div>
                                    <br />
                                </div>
                            </div>
                        </center>
                    </center>
                </center>
            </div>
            <%--end individual item--%>
            <%--request and purchaseorder--%>
            <center>
                <div id="reqpurchaseorder_div" runat="server" visible="false" class="popupstyle popupheight">
                    <asp:ImageButton ID="ImageButton6" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 433px;"
                        OnClick="imagebtnpopclose5_Click" />
                    <br />
                    <div class="subdivstyle" style="background-color: White; height: 578px; width: 900px;">
                        <br />
                        <div>
                            <asp:Label ID="Label2" runat="server" Style="font-size: large; color: Green;" Text="Select the Vendor"
                                Font-Bold="true"></asp:Label>
                            <br />
                            <br />
                            <div>
                                <FarPoint:FpSpread ID="Fpspread5" runat="server" Visible="false" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" Height="350px" Width="620px" class="spreadborder">
                                    <sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </sheets>
                                </FarPoint:FpSpread>
                            </div>
                            <br />
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="Button1" Visible="false" CssClass="textbox btn2" OnClick="btn_vendorselect_Click"
                                        Text="Ok" runat="server" />
                                    <asp:Button ID="Button2" Visible="false" CssClass="textbox btn2" OnClick="btn_vendorExit_Click"
                                        Text="Exit" runat="server" />
                                </div>
                            </center>
                        </div>
                        <br />
                    </div>
                </div>
            </center>
            <%--end request purchase--%>
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
        </form>
    </body>
    </html>
</asp:Content>
