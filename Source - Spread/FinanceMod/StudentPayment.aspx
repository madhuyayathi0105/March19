<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentPayment.aspx.cs" Inherits="StudentPayment" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=grid_Details.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 0; i < (tbl.rows.length - 1); i++) {
                var chkSelectid = document.getElementById('MainContent_grid_Details_cb_selectLedger_' + i.toString());
                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }
            checkpaidamount();
        }
        function checkpaidamount() {
            var totoldAmt = document.getElementById("<%=txt_totamt.ClientID %>");
            var txtoldPaid = document.getElementById("<%=txt_paidamt.ClientID %>");
            var txtbalanceamount = document.getElementById("<%=txt_balamt.ClientID %>");
            var txttobePaidOld = document.getElementById("<%=txttobepaid.ClientID %>");

            var oldTotAmt = parseFloat(totoldAmt.value);
            var oldPaidAmt = parseFloat(txtoldPaid.value);
            var oldBalamt = parseFloat(txtbalanceamount.value);
            //var oldToBePaidAmt = parseFloat(txttobePaid.value);

            var tbl = document.getElementById("<%=grid_Details.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");
            var totalToBePaid = 0;
            var totalPaid = 0;
            for (var i = 0; i < (gridViewControls.length - 1); i++) {
                var chkSelectid = document.getElementById('MainContent_grid_Details_cb_selectLedger_' + i.toString());

                if (chkSelectid != null) {
                    var txttobePaid = document.getElementById('MainContent_grid_Details_txt_tobepaid_amt_' + i.toString());
                    var txtbal = document.getElementById('MainContent_grid_Details_txt_bal_amt_' + i.toString());
                    var txttotal = document.getElementById('MainContent_grid_Details_txt_tot_amt_' + i.toString());
                    var txtpaid = document.getElementById('MainContent_grid_Details_txt_paid_amt_' + i.toString());
                    var tobePaidAmt = parseFloat(txttobePaid.value);
                    var balAmt = parseFloat(txtbal.value);
                    var paidAmt = parseFloat(txtpaid.value);
                    var totAmt = parseFloat(txttotal.value);
                    totalPaid += paidAmt;
                    if (chkSelectid.checked == true) {
                        if (tobePaidAmt != 0) {
                            txttobePaid.value = tobePaidAmt.toString();
                            totalToBePaid += tobePaidAmt;
                        }
                        else {
                            txttobePaid.value = balAmt.toString();
                            totalToBePaid += balAmt;
                        }
                        txtbal.value = 0;
                    }
                    else {
                        txtbal.value = (totAmt - paidAmt).toString();
                        txttobePaid.value = 0;
                        totalToBePaid = 0;
                    }
                }
            }
            //txttobePaid.value = totalToBePaid.toString();
            txtoldPaid.value = (totalPaid + totalToBePaid).toString();
            txtbalanceamount.value = (oldTotAmt - (totalPaid + totalToBePaid)).toString();
            txttobePaidOld.value = totalToBePaid.toString();
        }
        function checkToBeamount() {
            var totoldAmt = document.getElementById("<%=txt_totamt.ClientID %>");
            var txtoldPaid = document.getElementById("<%=txt_paidamt.ClientID %>");
            var txtbalanceamount = document.getElementById("<%=txt_balamt.ClientID %>");
            var txttobePaidOld = document.getElementById("<%=txttobepaid.ClientID %>");

            var oldTotAmt = parseFloat(totoldAmt.value);
            var oldPaidAmt = parseFloat(txtoldPaid.value);
            var oldBalamt = parseFloat(txtbalanceamount.value);
            // var oldToBePaidAmt = parseFloat(txttobePaid.value);

            var tbl = document.getElementById("<%=grid_Details.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");
            var totalToBePaid = 0;
            var totalPaid = 0;
            for (var i = 0; i < (gridViewControls.length - 1); i++) {
                var chkSelectid = document.getElementById('MainContent_grid_Details_cb_selectLedger_' + i.toString());

                var txttobePaid = document.getElementById('MainContent_grid_Details_txt_tobepaid_amt_' + i.toString());
                if (txttobePaid != null) {
                    var txtbal = document.getElementById('MainContent_grid_Details_txt_bal_amt_' + i.toString());
                    var txttotal = document.getElementById('MainContent_grid_Details_txt_tot_amt_' + i.toString());
                    var txtpaid = document.getElementById('MainContent_grid_Details_txt_paid_amt_' + i.toString());
                    var tobePaidAmt = parseFloat(txttobePaid.value);
                    var balAmt = parseFloat(txtbal.value);
                    var paidAmt = parseFloat(txtpaid.value);
                    var totAmt = parseFloat(txttotal.value);
                    totalPaid += paidAmt;
                    if (tobePaidAmt != 0) {
                        var temp = paidAmt + tobePaidAmt;
                        if (totAmt >= temp) {
                            txttobePaid.value = tobePaidAmt.toString();
                            txtbal.value = (totAmt - (paidAmt + tobePaidAmt)).toString();
                            totalToBePaid += tobePaidAmt;
                            chkSelectid.checked = true;
                        }
                        else {
                            txttobePaid.value = 0;
                            txtbal.value = (totAmt - paidAmt).toString();
                            chkSelectid.checked = false;
                        }
                    }
                    else {
                        chkSelectid.checked = false;
                        txtbal.value = (totAmt - paidAmt).toString();
                        txttobePaid.value = 0;
                    }
                }
            }
            //document.getElementById("<%=txttobepaid.ClientID %>").value = totalToBePaid.toString();
            txtoldPaid.value = (totalPaid + totalToBePaid).toString();
            txtbalanceamount.value = (oldTotAmt - (totalPaid + totalToBePaid)).toString();
            txttobePaidOld.value = totalToBePaid.toString();
        }
        function PrintDiv() {
            var panel = document.getElementById("<%=Div3.ClientID %>");
            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('<style> .classRegular { font-family:Arial; font-size:9px; } .classBold10 { font-family:Arial; font-size:11px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:13px; font-weight:bold;} .classBold { font-family:Arial; font-size:9px; font-weight:bold;} .classReg12 {   font-size:14px; } </style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
        function rb_ccdc_Onchange(mode) {

            if (mode == "cash") {
                document.getElementById("<%=tdChQDD.ClientID %>").style.display = "none";
                document.getElementById("<%=tdCard.ClientID %>").style.display = "none";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "none";
            }
            else if (mode == "cheque") {
                document.getElementById("<%=tdChQDD.ClientID %>").style.display = "block";
                document.getElementById("<%=tdCard.ClientID %>").style.display = "none";

                document.getElementById("<%=div_cheque.ClientID %>").style.display = "block";
                document.getElementById("<%=lbl_chqno.ClientID %>").style.display = "block";
                document.getElementById("<%=txt_chqno.ClientID %>").style.display = "block";

                document.getElementById("<%=div_card.ClientID %>").style.display = "none";
                document.getElementById("<%=lbl_ddno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_ddno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_ddnar.ClientID %>").style.display = "none";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "none";

            }
            else if (mode == "dd") {

                document.getElementById("<%=tdChQDD.ClientID %>").style.display = "block";
                document.getElementById("<%=tdCard.ClientID %>").style.display = "none";

                document.getElementById("<%=div_cheque.ClientID %>").style.display = "block";
                document.getElementById("<%=lbl_ddno.ClientID %>").style.display = "block";
                document.getElementById("<%=txt_ddno.ClientID %>").style.display = "block";
                document.getElementById("<%=txt_ddnar.ClientID %>").style.display = "block";

                document.getElementById("<%=div_card.ClientID %>").style.display = "none";
                document.getElementById("<%=lbl_chqno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_chqno.ClientID %>").style.display = "none";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "none";
            }
            else if (mode == "card") {
                document.getElementById("<%=tdChQDD.ClientID %>").style.display = "none";
                document.getElementById("<%=tdCard.ClientID %>").style.display = "block";

                document.getElementById("<%=div_cheque.ClientID %>").style.display = "none";
                document.getElementById("<%=div_card.ClientID %>").style.display = "block";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "none";
            }
            else if (mode == "NEFT") {
                document.getElementById("<%=tdChQDD.ClientID %>").style.display = "none";
                document.getElementById("<%=tdCard.ClientID %>").style.display = "none";

                document.getElementById("<%=div_cheque.ClientID %>").style.display = "none";
                document.getElementById("<%=div_card.ClientID %>").style.display = "none";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "block";

            }
        }
        function otherBank(itemid) {
            var txtid = document.getElementById("<%=ddlotherBank.ClientID %>");
            var ddlid = itemid.value;
            if (ddlid.trim().toUpperCase() == "OTHERS") {
                txtid.style.display = "block";
            } else {
                txtid.style.display = "none";
            }
        }
        function otherCardType(itemid) {
            var txtid = document.getElementById("<%=txtCardType.ClientID %>");
            var ddlid = itemid.value;
            if (ddlid.trim().toUpperCase() == "OTHERS") {
                txtid.style.display = "block";
            } else {
                txtid.style.display = "none";
            }
        }
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Transfer/Refund</span></div>
        </center>
    </div>
    <div style="width: 1000px; height: auto;">
        <center>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="College"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_indexChanged"
                            CssClass="textbox ddlstyle ddlheight3" Width="193px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblPaymode" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblPaymode_OnSelectedIndexChanged">
                            <asp:ListItem Text="Excess" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Advance"></asp:ListItem>
                            <asp:ListItem Text="Refund"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <%--Added by saranya on 5April2018--%>
                    <td id="refundStudOrStaff" runat="server" visible="false">
                        <fieldset id="StudOrStaff" style="height: 12px; width: 50px;">
                            <asp:RadioButtonList ID="rbl_Refund" runat="server" Height="10px" Width="150px" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rblRefund_OnSelectedIndexChanged">
                                <%--OnSelectedIndexChanged="rbl_rollnoNewForRefund_OnSelectedIndexChanged"--%>
                                <asp:ListItem Selected="True">Student</asp:ListItem>
                                <asp:ListItem>Staff</asp:ListItem>
                            </asp:RadioButtonList>
                        </fieldset>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_date" runat="server" CssClass="textbox txtheight" Height="15px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_rcptno" runat="server" CssClass="textbox txtheight" Height="15px"
                            Width="126px" Style="text-align: right;" Enabled="false" BackColor="#81F7D8"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </center>
        <div>
            <table id="rcptsngle" runat="server" visible="false">
                <tr>
                    <td>
                        <asp:TextBox ID="txt_rollno" runat="server" placeholder="Roll No" CssClass="textbox  txtheight2"
                            OnTextChanged="txt_rollno_Changed" AutoPostBack="true"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderroll" runat="server" TargetControlID="txt_rollno"
                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                        </asp:FilteredTextBoxExtender>
                        <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                        <asp:Button ID="btn_roll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                            OnClick="btn_roll_Click" />
                        <asp:Label ID="lblStudStatus" runat="server" Text="" Visible="false" CssClass="textbox btn1 textbox1"
                            Style="color: Green; font-weight: bold;"></asp:Label>
                        <br />
                        <asp:TextBox ID="txt_name" runat="server" placeholder="Name" CssClass="textbox txtheight2"
                            Width="300px" OnTextChanged="txt_name_Changed" AutoPostBack="true"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetName" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_dept" runat="server" placeholder="Department" CssClass="textbox txtheight2"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPpanel_sem" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight2 txtboxBg" ReadOnly="true"
                                                placeholder="Semester/Year" onfocus="return myFunction1(this)"></asp:TextBox>
                                            <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_sem_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupce_sem" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="panel_sem" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" Text="Go" runat="server" CssClass="textbox btn1 textbox1"
                                        OnClick="btn_search_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_SeatType" runat="server" placeholder="Seat Type" CssClass="textbox txtheight2"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_FatherName" runat="server" placeholder="Father Name" CssClass="textbox txtheight2"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <%--Added by saranya on 06/04/2018--%>
            <table id="StaffRefund" runat="server" visible="false">
                <tr>
                    <td id="td_RefundstaffId" colspan="6" runat="server">
                        <asp:TextBox ID="txt_staffid" runat="server" placeholder="Staff Id" CssClass="textbox  txtheight2"
                            AutoPostBack="true" OnTextChanged="txtstaffid_Changed"></asp:TextBox><%--OnTextChanged="txtroll_staff_Changed"--%>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_staffid"
                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                        </asp:FilteredTextBoxExtender>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffid"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                        <asp:Button ID="btn_staffLook" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                            OnClick="btn_staffLook_Click" />
                    </td>
                </tr>
                <tr>
                    <td id="td_RefundstaffName" colspan="6" runat="server">
                        <asp:TextBox ID="txt_staffName" runat="server" placeholder="Name" CssClass="textbox txtheight2"
                            Width="300px" AutoPostBack="true"></asp:TextBox>
                        <%--OnTextChanged="txtname_staff_Changed"--%>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffName"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                    </td>
                </tr>
                <tr>
                    <td id="td_RefundstaffCode" colspan="6" runat="server">
                        <asp:TextBox ID="txt_staffDept" runat="server" placeholder="Department" CssClass="textbox txtheight2"
                            ReadOnly="true"></asp:TextBox>
                        <asp:Button ID="btnStaffgo" Text="Go" runat="server" CssClass="textbox btn1 textbox1"
                            OnClick="btn_Staffsearch_Click" />
                    </td>
                </tr>
            </table>
            <%-----------------------------------%>
            <table>
                <tr>
                    <td id="balRow" runat="server">
                        <asp:Label ID="lbl_totamt" runat="server" Text="Total"></asp:Label>
                        <asp:TextBox ID="txt_totamt" placeholder="0.00" runat="server" onblur="checkFloatValue(this);"
                            CssClass="textbox txtheight" Style="text-align: right; width: 45px; height: 15px;"
                            BackColor="#81F7D8"></asp:TextBox>
                        <asp:Label ID="lbl_paidamt" runat="server" Text="Paid"></asp:Label>
                        <asp:TextBox ID="txt_paidamt" runat="server" placeholder="0.00" onblur="checkFloatValue(this);"
                            CssClass="textbox txtheight" Style="text-align: right; width: 45px; height: 15px;"
                            BackColor="#81F7D8"></asp:TextBox>
                        <asp:Label ID="lbl_balamt" runat="server" Text="Balance"></asp:Label>
                        <asp:TextBox ID="txt_balamt" runat="server" placeholder="0.00" onblur="checkFloatValue(this);"
                            CssClass="textbox txtheight" Style="text-align: right; width: 45px; height: 15px;"
                            BackColor="#81F7D8"></asp:TextBox>
                        <asp:Label ID="Label1" runat="server" Text="Tobepaid"></asp:Label>
                        <asp:TextBox ID="txttobepaid" runat="server" placeholder="0.00" onblur="checkFloatValue(this);"
                            CssClass="textbox txtheight" Style="text-align: right; width: 45px; height: 15px;"
                            BackColor="#81F7D8"></asp:TextBox>
                        <asp:Button ID="btnSave" Text="Save" runat="server" Visible="false" CssClass="textbox btn1 textbox1"
                            OnClick="btnSave_Click" Style="background-color: Green; color: White;" />
                    </td>
                </tr>
                <tr>
                    <td id="checCashDD" runat="server" visible="true">
                        <asp:RadioButton ID="rb_cash" runat="server" RepeatDirection="Horizontal" GroupName="same3"
                            Text="Cash" Checked="true" onchange="rb_ccdc_Onchange('cash')"></asp:RadioButton>
                        <%--OnCheckedChanged="rb_cash_CheckedChanged" AutoPostBack="true" --%>
                        <asp:RadioButton ID="rb_cheque" runat="server" RepeatDirection="Horizontal" GroupName="same3"
                            Text="Cheque" onchange="rb_ccdc_Onchange('cheque')"></asp:RadioButton><%-- OnCheckedChanged="rb_cheque_CheckedChanged"  AutoPostBack="true" --%>
                        <asp:RadioButton ID="rb_dd" runat="server" RepeatDirection="Horizontal" GroupName="same3"
                            Text="DD" onchange="rb_ccdc_Onchange('dd')"></asp:RadioButton>
                        <%--OnCheckedChanged="rb_dd_CheckedChanged" AutoPostBack="true"--%>
                        <asp:RadioButton ID="rb_card" runat="server" RepeatDirection="Horizontal" GroupName="same3"
                            Text="Card" onchange="rb_ccdc_Onchange('card')"></asp:RadioButton>
                        <asp:RadioButton ID="rb_NEFT" runat="server" RepeatDirection="Horizontal" GroupName="same3"
                            Text="Neft" onchange="rb_ccdc_Onchange('NEFT')"></asp:RadioButton><%--added by abarna 7.5.2018--%>
                        <%--OnCheckedChanged="rb_card_CheckedChanged" AutoPostBack="true"--%>
                    </td>
                </tr>
                <tr id="tdChQDD" runat="server" style="display: none;">
                    <td>
                        <table id="div_ch1" runat="server" style="padding-left: 10px; width: 980px;">
                            <tr id="div_cheque" runat="server">
                                <td>
                                    <span class="challanLabel">
                                        <p>
                                            Bank</p>
                                    </span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_bkname" runat="server" CssClass="textbox ddlheight2" onchange="return otherBank(this);">
                                    </asp:DropDownList>
                                </td>
                                <%--  <td>
                                    <asp:DropDownList ID="ddlotherBank" runat="server"  CssClass="textbox  ddlheight5"
                                        Width="150px">
                                    </asp:DropDownList>
                                </td>--%>
                                <td>
                                    <%--  <asp:TextBox ID="txt_other" runat="server" CssClass="textbox txtheight2" onfocus="return myFunction(this)"
                                        Placeholder="Other Bank" Style="display: none;"></asp:TextBox>--%>
                                    <asp:DropDownList ID="ddlotherBank" runat="server" CssClass="textbox  ddlheight5"
                                        Width="150px">
                                    </asp:DropDownList>
                                    <%--  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom"
                                        ValidChars=" " TargetControlID="ddlotherBank">
                                    </asp:FilteredTextBoxExtender>--%>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_branch" runat="server" Placeholder="Branch" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_chqno" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_chqno" runat="server" Placeholder="Cheque No" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_ddno" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_ddno" runat="server" Placeholder="DD No" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="challanLabel">
                                        <p>
                                            Date</p>
                                    </span>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_date1" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_date1" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_ddnar" runat="server" Placeholder="Narration" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="tdCard" runat="server" style="display: none;">
                    <td>
                        <table>
                            <tr id="div_card" runat="server">
                                <td>
                                    <asp:Label ID="lblCardName" runat="server" Text="Card Type" Style="float: left; padding: 2px;
                                        padding-top: 5px;"></asp:Label>
                                    <asp:DropDownList ID="ddlCardType" runat="server" CssClass="textbox ddlheight2" onchange="return otherCardType(this);"
                                        Style="float: left;">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtCardType" runat="server" MaxLength="40" CssClass="textbox txtheight2"
                                        onfocus="return myFunction(this)" Placeholder="Other Cards" Style="display: none;
                                        float: left;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom"
                                        ValidChars=" " TargetControlID="txtCardType">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lblLast4No" runat="server" Text="Card's Last Four Digits" Style="float: left;
                                        padding: 2px; padding-top: 5px;"></asp:Label>
                                    <asp:TextBox ID="txtLast4No" runat="server" Placeholder="XXXX" CssClass="textbox txtheight"
                                        MaxLength="4" onblur="if(this.value.length!=4)this.value='';" Width="35px" Style="float: left;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="fceCardTxt" runat="server" FilterType="Numbers"
                                        TargetControlID="txtLast4No">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="div_NEFT" runat="server" style="display: none;">
                    <td>
                        <asp:Label ID="lbl_NEFTNO" runat="server" Text="NEFT NO"></asp:Label>
                        <asp:TextBox ID="txt_NEFTNO" runat="server" Placeholder="NEFT NO" CssClass="textbox txtheight2"></asp:TextBox>
                    </td>
                    <%--added by abarna--%>
                </tr>
                <tr>
                    <td>
                        <span style="padding-right: 100px;">
                            <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" onchange="return SelLedgers();" />
                        </span>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <div id="div_grid" runat="server" style="width: 980px; height: 310px; overflow: auto;
                            background-color: white; border-radius: 10px;">
                            <asp:GridView ID="grid_Details" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                OnRowDataBound="grid_Details_OnRowDataBound" OnDataBound="grid_Details_DataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                            <asp:Label ID="lblappNo" runat="server" Visible="false" Text='<%#Eval("app_no") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cb_selectLedger" runat="server" onchange="return checkpaidamount();">
                                            </asp:CheckBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Semester/ Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_textCode" runat="server" Text='<%#Eval("TextCode") %>' Visible="false"
                                                    Width="80px"></asp:Label>
                                                <asp:Label ID="lbl_textval" runat="server" Text='<%#Eval("Textval") %>' Width="80px"></asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_hdrName" runat="server" Text='<%#Eval("Header_Name") %>' Width="150px"></asp:Label>
                                            <asp:Label ID="lbl_hdrid" runat="server" Text='<%#Eval("Header_Id") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fee Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_feetype" runat="server" Text='<%#Eval("Fee_Type") %>' Width="150px"></asp:Label>
                                            <asp:Label ID="lbl_feecode" runat="server" Text='<%#Eval("Fee_Code") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_tot_amt" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("Total") %>'
                                                    Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:Label ID="lbl_finyear" runat="server" Text='<%#Eval("finyearfk") %>' Visible="false"></asp:Label>
                                            <asp:FilteredTextBoxExtender ID="filterextender3" runat="server" TargetControlID="txt_tot_amt"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_paid_amt" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("PaidAmt") %>'
                                                    Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextender4" runat="server" TargetControlID="txt_paid_amt"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_bal_amt" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("BalAmt") %>'
                                                    Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextender5" runat="server" TargetControlID="txt_bal_amt"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Be Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_tobepaid_amt" runat="server" placeholder="0.00" CssClass="  textbox txtheight"
                                                    Style="text-align: right;" Text='<%#Eval("ToBePaid") %>' onchange="return checkToBeamount();"
                                                    Height="15px" Width="70px"></asp:TextBox></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%--  ******popup window******--%>
        <center>
            <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1 ">
                <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 460px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 500px; width: 950px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Select The Student</span></div>
                    </center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_batch1" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_batch1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_stream" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_strm" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_strm_OnIndexChange">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree2" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">--Select--</asp:TextBox>
                                        <asp:Panel ID="pdegree" runat="server" Width="150px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_degree2" runat="server" OnCheckedChanged="cb_degree2_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_degree2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree2_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_degree2"
                                            PopupControlID="pdegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_branch2" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_branch1" runat="server" OnCheckedChanged="cb_branch1_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_branch2"
                                            PopupControlID="pbranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <span class="challanLabel">
                                    <p>
                                        Section</p>
                                </span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8sec" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sec2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1 txtheight">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlsec2" runat="server" Width="120px" Height="80px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_sec2" runat="server" OnCheckedChanged="cb_sec2_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_sec2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec2_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_sec2"
                                            PopupControlID="pnlsec2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_rollno3" runat="server" Text="Roll No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rollno3" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                    Height="20px" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_rollno3"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno3"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" Text="Go" OnClick="btn_go_Click" CssClass="textbox btn1 textbox1"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr runat="server" id="trFuParNot" visible="false">
                            <td colspan="5">
                            </td>
                            <td colspan="8" style="text-color: white; text-align: right;">
                                <asp:CheckBox ID="cbFirstGrad" runat="server" BackColor="#EE9090" Checked="true"
                                    Text="First Graduate" />
                                <asp:CheckBox ID="cbFpaid" runat="server" BackColor="#90EE90" Checked="true" Text="Fully Paid" /><asp:CheckBox
                                    ID="cbPpaid" runat="server" BackColor="#FFB6C1" Checked="true" Text="Partially Paid" />
                                <asp:CheckBox ID="cbNpaid" runat="server" BackColor="White" Checked="true" Text="Not Paid" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <asp:Label ID="lbl_errormsg" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                    <div>
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" ShowHeaderSelection="false"
                            BorderWidth="0px" Width="830px" Style="overflow: auto; height: 250px; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            OnUpdateCommand="Fpspread1_Command">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btn_studOK" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                                OnClick="btn_studOK_Click" />
                            <asp:Button ID="btn_exitstud" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                                OnClick="btn_exitstud_Click" />
                        </div>
                    </center>
                </div>
            </div>
            <div style="height: 1px; width: 1px; overflow: auto;">
                <div id="Div3" runat="server" style="height: 710px; width: 1344px;" visible="false">
                </div>
            </div>
        </center>
    </div>
    <%--Staff Lookup --Added by saranya on 7/4/2018--%>
    <center>
        <div id="div_staffLook" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 310px;"
                OnClick="btn_exitstaff_Click" />
            <br />
            <br />
            <div style="background-color: White; height: 400px; width: 650px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Select The Staff</span></div>
                </center>
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <span class="challanLabel">
                                <p>
                                    Search By</p>
                            </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsearch1" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlsearch1_OnSelectedIndexChanged">
                                <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsearch1" runat="server" Visible="false" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                            <asp:TextBox ID="txtsearch1c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1c"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Button ID="btn_go2Staff" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                OnClick="btn_go2Staff_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="lbl_errormsgstaff" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
                <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" ShowHeaderSelection="false"
                    BorderWidth="0px" Style="width: 620px; height: 230px; auto; border: 0px solid #999999;
                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                    OnUpdateCommand="Fpspread2staff_Command">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Blue" SelectionPolicy="Single">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <center>
                    <div>
                        <asp:Button ID="btn_staffOK" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                            OnClick="btn_staffOK_Click" />
                        <asp:Button ID="btn_exitstaff" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                            OnClick="btn_exitstaff_Click" />
                    </div>
                </center>
            </div>
        </div>
    </center>
</asp:Content>
