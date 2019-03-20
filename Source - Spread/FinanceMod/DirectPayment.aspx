<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DirectPayment.aspx.cs" Inherits="DirectPayment"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .maindivstylesize
        {
            height: 900px;
            width: 1000px;
        }
        .lbl
        {
            text-align: center;
        }
        .container
        {
            width: 100%;
        }
        .col1
        {
            float: left;
            width: 50%;
        }
        .col2
        {
            float: right;
            width: 50%;
        }
        .table2
        {
            border: 1px solid #0CA6CA;
            border-radius: 10px;
            background-color: #0CA6CA;
            box-shadow: 0px 0px 8px #7bc1f7;
        }
        .tabeltd
        {
            background-color: #79BD9A;
            text-decoration: none;
            color: white;
        }
        .autocomplete_highlightedListItem
        {
            background-color: #EEEE89;
            color: black;
            padding: 1px;
            width: 241px;
        }
        .txtcaps
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">

            function PrintDiv() {
                var panel = document.getElementById("<%=contentDiv.ClientID %>");
                var printWindow = window.open('', '', 'height=816,width=980');
                printWindow.document.write('<html><head>');
                printWindow.document.write('<style>body, html {margin:0;padding:0;height:100%;} .classRegular { font-family:Arial; font-size:10px; } .classBold10 { font-family:Arial; font-size:12px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:14px; font-weight:bold;} .classBold { font-family:Arial; font-size:10px; font-weight:bold;} .classReg12 {   font-size:14px; } </style>');
                printWindow.document.write('</head><body >');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                }, 500);
                return false;
            }
            function addmarks(id) {
                var finalvalue = 0;

                var txtid = document.getElementById("<%=txt_venpayamt.ClientID %>").value;
                if (txtid.trim() != "") {
                    txtid = document.getElementById("<%=txt_venpayamt.ClientID%>").value;
                    var varn = document.getElementById("<%=gridView1.ClientID %>");
                    var gridViewControls = varn.getElementsByTagName("input");
                    var len = varn.rows.length;
                    for (var i = 0; i < gridViewControls.length; i++) {
                        if (gridViewControls[i].name.indexOf("txtpaymt") > 1) {
                            if (gridViewControls[i].value != "") {
                                finalvalue = parseInt(finalvalue) + parseInt(gridViewControls[i].value);
                                if (parseInt(finalvalue) > parseInt(txtid)) {
                                    gridViewControls[i].value = "";
                                }
                            }
                        }

                    }

                }
                else {
                    var varn = document.getElementById("<%=gridView1.ClientID %>");
                    var gridViewControls = varn.getElementsByTagName("input");
                    var len = varn.rows.length;
                    for (var i = 0; i < gridViewControls.length; i++) {
                        if (gridViewControls[i].name.indexOf("txtpaymt") > 1) {
                            if (gridViewControls[i].value != "") {
                                gridViewControls[i].value = "";
                            }
                        }

                    }
                }

            }


            function addmarkss(id) {
                var finalvalue = 0;

                var txtid = document.getElementById("<%=txt_lbl_vencurbal.ClientID %>").value;
                if (txtid.trim() != "") {
                    txtid = document.getElementById("<%=txt_lbl_vencurbal.ClientID%>").value;
                    var varn = document.getElementById("<%=gridView2.ClientID %>");
                    var gridViewControls = varn.getElementsByTagName("input");
                    var len = varn.rows.length;
                    for (var i = 0; i < gridViewControls.length; i++) {
                        if (gridViewControls[i].name.indexOf("txtamt") > 1) {
                            if (gridViewControls[i].value != "") {
                                finalvalue = parseInt(finalvalue) + parseInt(gridViewControls[i].value);
                                if (parseInt(finalvalue) > parseInt(txtid)) {
                                    gridViewControls[i].value = "";
                                }
                            }
                        }

                    }

                }
                else {
                    var varn = document.getElementById("<%=gridView1.ClientID %>");
                    var gridViewControls = varn.getElementsByTagName("input");
                    var len = varn.rows.length;
                    for (var i = 0; i < gridViewControls.length; i++) {
                        if (gridViewControls[i].name.indexOf("txtpaymt") > 1) {
                            if (gridViewControls[i].value != "") {
                                gridViewControls[i].value = "";
                            }
                        }

                    }
                }

            }
            function Textamtchange() {
                var FnlAmt = "";
                var cash = document.getElementById('<%=rb_cash.ClientID %>')
                var bank = document.getElementById('<%=rb_bank.ClientID %>');

                var totamt = document.getElementById('<%=txt_ventotpayamt.ClientID %>').value;
                var payamt = document.getElementById('<%=txt_venpayamt.ClientID %>').value;
                if (cash.checked == true)
                    FnlAmt = document.getElementById('<%=txtpayamt.ClientID %>').value;
                else
                    FnlAmt = document.getElementById('<%=txtavailAmt.ClientID %>').value;

                var curbal = 0;
                var labl = "";
                if (parseFloat(totamt) != "" && parseFloat(payamt) != "" && parseFloat(FnlAmt) != "") {
                    if (parseFloat(totamt) >= parseFloat(payamt) && parseFloat(FnlAmt) >= parseFloat(payamt)) {
                        curbal = parseFloat(totamt) - parseFloat(payamt);
                        document.getElementById('<%=txt_lbl_vencurbal.ClientID%>').value = parseFloat(curbal);
                        document.getElementById('<%=txt_amt.ClientID%>').value = parseFloat(payamt);
                        document.getElementById('<%=lbl_er.ClientID%>').innerHTML = "";

                    }
                    else {
                        document.getElementById('<%=txt_venpayamt.ClientID %>').value = "";
                        document.getElementById('<%=txt_lbl_vencurbal.ClientID%>').value = "";
                        document.getElementById('<%=txt_amt.ClientID%>').value = "";
                    }

                }
                else {
                    curbal = "";
                    document.getElementById('<%=txt_lbl_vencurbal.ClientID%>').value = "";
                    document.getElementById('<%=txt_venpayamt.ClientID %>').value = "";
                }

            }
            function checkFloatValue(el) {
                var ex = /^[0-9]+\.?[0-9]*$/;
                if (ex.test(el.value) == false) {
                    el.value = "0.00";
                }
            }

            function checkDate() {
                var EnteredDate = document.getElementById('<%=txt_vdate.ClientID %>').value;
                var date = EnteredDate.substring(0, 2);
                var month = EnteredDate.substring(3, 5);
                var year = EnteredDate.substring(6, 10);
                var myDate = new Date(year, month - 1, date);
                var today = new Date();
                var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
                var empty = "";
                if (myDate > today) {
                    alert("Voucher date is greater than today's date ");
                    document.getElementById('<%=txt_vdate.ClientID %>').value = currentDate;
                    empty = "e";
                    return false;
                }
                //            if (myDate < today) {
                //                alert("Voucher date is Less than today's date ");
                //                document.getElementById('<%=txt_vdate.ClientID %>').value = currentDate;
                //                empty = "e";
                //                return false;
                //            }
                if (empty == "") {
                    return true;
                }
            }

            function checkInstallDate() {

                var bank = document.getElementById('<%=rb_bank.ClientID %>');
                if (bank.checked == true) {
                    var cheqno = document.getElementById('<%=txt_chqno.ClientID %>').value;
                    if (cheqno == "" || cheqno == null) {
                        alert("Please Enter Cheque No");
                        return false;
                    }
                }
                var install = document.getElementById('<%=chkinstall.ClientID%>');
                if (install.checked == true) {
                    var a = true;
                    var empty = "";
                    var grid = document.getElementById('<%=gridView2.ClientID%>');
                    var gdViewCont = grid.getElementsByTagName("input");
                    var gdLen = grid.rows.length;
                    for (var k = 0; k < gdViewCont.length; k++) {
                        if (gdViewCont[k].name.indexOf("txtdate") > 1) {

                            var EnteredDate = gdViewCont[k].value;
                            var date = EnteredDate.substring(0, 2);
                            var month = EnteredDate.substring(3, 5);
                            var year = EnteredDate.substring(6, 10);
                            var myDate = new Date(year, month - 1, date);
                            var today = new Date();
                            var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
                            var curdate = today.getDate();
                            var curmonth = (today.getMonth() + 1);
                            var curyear = today.getFullYear();

                            if (year == curyear) {
                                if (month == curmonth) {
                                    if (date == curdate) {
                                    }
                                    else if (date > curdate) {
                                    }
                                    else {
                                        a = false;
                                    }
                                }
                                else if (month > curmonth) {
                                }
                                else {
                                    a = false;
                                }
                            }
                            else if (year > curyear) {
                            }
                            else if (year < curyear) {
                            }
                        }
                    }
                    if (!a) {
                        alert("Installment date is greater than today's date ");
                        a = true;
                        return false;
                    }
                }
            }
            function checkVendorAmt() {
                var empty = "";
                var gdVendor = document.getElementById("<%=gridven.ClientID%>");
                var gdVendorControls = gdVendor.getElementsByTagName("input");
                var gdvenleng = gdVendor.rows.length;
                for (var j = 0; j < (gdVendorControls.length); j++) {
                    var totamt = document.getElementById('gridven_txttotamt_' + j.toString());
                    var tobepaid = document.getElementById('gridven_txttobepaid_' + j.toString());
                    if (totamt != null && tobepaid != null) {
                        var totalamt = parseFloat(totamt.value);
                        var tobepaidamt = parseFloat(tobepaid.value);
                        if (parseFloat(totalamt) >= parseFloat(tobepaidamt)) {

                        }
                        else {
                            empty = "e";
                        }
                    }
                }
                if (empty != "") {
                    alert("Please Enter the correct Amount");
                    return false;
                }
                else {
                    return true;
                }
            }
            function bankAmount() {
                var payamount = document.getElementById('<%=txt_venpayamt.ClientID%>').value;
                var bankamt = document.getElementById('<%=txt_amt.ClientID%>').value;
                if (payamount != "" && bankamt != "") {
                    if (parseFloat(payamount) == parseFloat(bankamt)) {
                        return true;

                    }
                    else {
                        document.getElementById('<%=txt_amt.ClientID%>').value = "";
                        alert("Enter the epual payamount");
                        return false;
                    }
                }
            }

            function chequeNo() {
                var bank = document.getElementById('<%=rb_bank.ClientID %>');
                if (bank.checked == true) {
                    var cheqno = document.getElementById('<%=txt_chqno.ClientID %>').value;
                    if (cheqno == "" || cheqno == null) {
                        alert("Please Enter Cheque No");
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Direct Payment</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 1000px; height: 690px;">
                <div>
                    <fieldset style="float: left; width: 182px; height: 20px;">
                        <asp:RadioButton ID="rb_cash" runat="server" Text="Cash" GroupName="s1" OnCheckedChanged="rb_cash_Change"
                            AutoPostBack="true" />
                        <asp:RadioButton ID="rb_bank" runat="server" Text="Bank" GroupName="s1" OnCheckedChanged="rb_bank_Change"
                            AutoPostBack="true" />
                        <asp:RadioButton ID="rbonline" runat="server" Text="Online" GroupName="s1" OnCheckedChanged="rbonline_Change"
                            AutoPostBack="true" />
                    </fieldset>
                    <fieldset style="float: left; width: 52px; height: 20px; margin-left: -3px;">
                        <asp:RadioButton ID="rb_pety" runat="server" AutoPostBack="true" GroupName="p" OnCheckedChanged="rb_pety_OnCheckedChanged"
                            Text="Pety" />
                        <asp:RadioButton ID="rb_hand" runat="server" Visible="false" AutoPostBack="true"
                            GroupName="p" OnCheckedChanged="rb_hand_OnCheckedChanged" Text="Hand" />
                    </fieldset>
                    <fieldset style="width: 282px; height: 20px; float: left; margin-left: -3px;">
                        <asp:RadioButton ID="rb_stud" runat="server" Text="Student" GroupName="s2" Checked="true"
                            AutoPostBack="true" OnCheckedChanged="rb_stud_Change" Visible="false" />
                        <asp:RadioButton ID="rb_staff" runat="server" Text="Staff" GroupName="s2" AutoPostBack="true"
                            OnCheckedChanged="rb_staff_Change" Visible="false" />
                        <asp:RadioButton ID="rb_vendor" runat="server" Text="Company" GroupName="s2" AutoPostBack="true"
                            OnCheckedChanged="rb_vendor_Change" Visible="false" />
                        <asp:RadioButton ID="rb_others" runat="server" Text="Others" GroupName="s2" AutoPostBack="true"
                            OnCheckedChanged="rb_others_Change" Visible="false" />
                    </fieldset>
                    <fieldset style="float: left; height: 20px; width: 386px; margin-left: -3px;">
                        <div id="vouch" runat="server" style="margin-left: 10px; margin-top: -6px;">
                            <table>
                                <tr>
                                    <td>
                                        Voucher Date
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_vdate" runat="server" AutoPostBack="true" onchange="return checkDate()"
                                                    CssClass="textbox" Width="79px"></asp:TextBox>
                                                <%--onchange="return checkDate()"--%>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_vdate" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        Voucher No
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox9" runat="server" CssClass="txtheight1" Width="80px"> </asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
                <div>
                    <fieldset style="margin-left: -678px; margin-top: -7px; height: 24px; width: 200px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_clg" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="234px" OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="fldpay" runat="server" visible="false" style="margin-left: 257px; margin-top: -47px;
                        height: 24px; width: 600px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblpayledg" runat="server" Text="Payment Ledger"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlapyledg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="250px" OnSelectedIndexChanged="ddlapyledg_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblamt" runat="server" Text="Amount"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpayamt" runat="server" CssClass="txtheight3 txtcaps" Width="70px"
                                        Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <%-- ****main div******--%>
                <center>
                    <div id="div_student" runat="server" style="width: 1000px; height: 300px;">
                        <div id="divstudcommon" runat="server" style="width: 460px; float: left; border: 1px solid;">
                            <%--  <tr>
                        <%----td1----%>
                            <div id="stud" runat="server" style="width: 450px; height: 300px; float: left;">
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_rollno" runat="server" CssClass="txtheight3 txtcaps" OnTextChanged="txt_rollno_OnTextChanged"
                                                AutoPostBack="true"></asp:TextBox>
                                            <asp:Button ID="btn_stud" Text="?" runat="server" OnClick="btn_stud_Click" CssClass="textbox btn" />
                                            <asp:CheckBox ID="chk_insexcess" runat="server" Text="Include Excess" AutoPostBack="true"
                                                OnCheckedChanged="chk_insexcess_Changed" Style="top: 269px; left: 500px; position: absolute;" />
                                            <asp:CheckBox ID="chk_deposite" runat="server" Text="Include Deposit" AutoPostBack="true"
                                                OnCheckedChanged="chk_deposite_Changed" Style="top: 290px; left: 500px; position: absolute;" />
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txt_rollno"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_name" runat="server" CssClass="txtheight5 txtcaps" OnTextChanged="txt_name_OnTextChanged"
                                                AutoPostBack="true">
                                            </asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txt_name"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .-@,">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Batch
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_batch" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_degr" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_dept" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="txtheight3" Style="width: 50px;">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Section
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_sec" runat="server" CssClass="txtheight3" Style="width: 50px;">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div id="divimbg" runat="server">
                                    <asp:ImageButton ID="imb_studpht" runat="server" Visible="false" Style="margin-left: 70px;
                                        margin-top: -61px; height: 76px; width: 109px;" OnClick="imb_studpht_Click" />
                                </div>
                            </div>
                            <%-- -----staff-----%>
                            <div id="staff" runat="server" visible="false" style="width: 450px; height: 300px;
                                float: left;">
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            Staff Code
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox2" runat="server" CssClass="txtheight3 txtcaps" OnTextChanged="TextBox2_OnTextChanged"
                                                AutoPostBack="true">
                                            </asp:TextBox>
                                            <asp:Button ID="btn_staff" Text="?" runat="server" OnClick="btn_staff_Click" CssClass="textbox btn" />
                                            <asp:CheckBox ID="cbStaff" runat="server" Visible="false" Text="Already Paid" AutoPostBack="true"
                                                OnCheckedChanged="cbStaff_Changed" />
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox2"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" TargetControlID="TextBox2"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .-@,">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Staff Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox3" runat="server" CssClass="txtheight5 txtcaps" OnTextChanged="TextBox3_OnTextChanged"
                                                AutoPostBack="true">
                                            </asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox3"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" TargetControlID="TextBox3"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .-@,">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Department
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox4" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="TextBox4"
                                                FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars="  ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Designation
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox5" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBox5"
                                                FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars="  ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Type
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stftype" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_stftype"
                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Catagory
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stfcat" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_stfcat"
                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Join Date
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_stfjn" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_stfjn"
                                                FilterType="Custom,Numbers" ValidChars="/ ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                                <div id="div1" runat="server">
                                    <asp:ImageButton ID="ImageButton3" runat="server" Visible="false" Style="margin-left: 252px;
                                        margin-top: -76px; height: 76px; width: 109px;" />
                                </div>
                            </div>
                            <%-- -----vendor-------%>
                            <div id="divvendor" runat="server" visible="false" style="width: 450px; height: 300px;
                                float: left;">
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <%-- Vendor Name--%>
                                            <asp:Label ID="Label3" runat="server" Text="Company Name" Style="margin-right: 0px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_venname" runat="server" CssClass="txtheight5 txtcaps" OnTextChanged="txt_venname_OnTextChanged"
                                                AutoPostBack="true">
                                            </asp:TextBox>
                                            <%-- <asp:Button ID="btn_itemvendor" Text="?" runat="server" OnClick="btn_vendor_Click" CssClass="textbox btn" style="margin-left:50px; margin-top:50px;" />--%>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getname3" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_venname"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txt_venname"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .-@,">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_itemvendor" Text="?" runat="server" OnClick="btn_vendor_Click"
                                                CssClass="textbox btn" Style="margin-left: 1px; margin-top: -4px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Contact Person
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ccont" runat="server" OnTextChanged="txt_ccont_OnTextChanged"
                                                AutoPostBack="true" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getname2" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ccont"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txt_ccont"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .-@,">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Designation
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_cdesi" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txt_cdesi"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Address
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_cadd" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_cadd"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="Order ID" Style="margin-right: 10px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox12" runat="server" AutoPostBack="true" OnTextChanged="TextBox12_OnTextChanged"
                                                CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                            <asp:Button ID="btnorder" Text="?" runat="server" OnClick="btnorderr_Click" CssClass="textbox btn" />
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender29TextBox12" runat="server"
                                                TargetControlID="TextBox12" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"
                                                ValidChars=" .">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%-------others---------%>
                            <div id="others" runat="server" visible="false" style="width: 450px; height: 300px;
                                float: left;">
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox6" runat="server" OnTextChanged="TextBox6_OnTextChanged"
                                                AutoPostBack="true" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="TextBox6"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" -.">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getothername" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox6"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Company Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_othcname" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox><span style="color: Red;">*</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txt_othcname"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td>
                                            Designation
                                        </td>--%>
                                        <td>
                                            <asp:TextBox ID="TextBox7" Visible="false" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Address
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_othadd" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txt_othadd"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Mobile No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_mblno" MaxLength="10" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_mblno"
                                                FilterType="Numbers,Custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_header" runat="server" Text="Type of Identification"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                                Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" Height="33px"
                                                Width="35px" />
                                            <asp:DropDownList ID="ddl_group" runat="server" Height="35px" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                            </asp:DropDownList>
                                            <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                Height="33px" Width="35px" Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Identification No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox8" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="TextBox8"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .,@">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="seconvdiv" runat="server" style="width: 540px; height: 310px; overflow:auto;
                                                        float: left; border: 1px solid; margin-top: -302px; margin-left: 462px;">
                            <div id="divstudtotpay" runat="server" visible="false" style="width: 540px; height: 300px;">
                                <asp:Label ID="lbl_er" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_ventotpayamt" runat="server" Visible="false" Text="Total Pay amount"></asp:Label>
                                            <asp:TextBox ID="txt_ventotpayamt" runat="server" Visible="false" onblur="return checkFloatValue(this)"
                                                CssClass="txtheight3 txtcaps" Width="50px">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_ventotpayamt"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:Label ID="lbl_venpayamt" runat="server" Visible="false" onblur="return checkFloatValue(this)"
                                                Text="Pay amount"></asp:Label>
                                            <asp:TextBox ID="txt_venpayamt" runat="server" Visible="false" CssClass="txtheight3 txtcaps"
                                                Width="50px" AutoPostBack="true" onchange="return Textamtchange()">
                                           <%--  OnTextChanged="txt_venpayamt_OnTextChanged">--%>
                                          <%--  onblur="return checkFloatValue(this)"--%>                                         
                                          
                                           
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_venpayamt"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:Label ID="lbl_vencurbal" runat="server" Visible="false" Text="Current Balance"></asp:Label>
                                            <asp:TextBox ID="txt_lbl_vencurbal" runat="server" Visible="false" CssClass="txtheight3 txtcaps"
                                                Width="50px">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_lbl_vencurbal"
                                                FilterType="Numbers,Custom" ValidChars=" .">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--   added by abarna 09.12.2017--%>
                                        <td colspan="3">
                                            <fieldset style="width: 450px" id="fieldset1" runat="server">
                                                <legend><b>JOURNAL</b></legend>
                                                <table>
                                                    <tr>
                                                        <%--<td>
                                                            <asp:Label ID="Label4" Text=" AMOUNT" runat="server"></asp:Label>
                                                        </td>--%>
                                                        <td>
                                                            <asp:RadioButton ID="rb_percentage" runat="server" Text="Percentage" GroupName="dts"
                                                                OnSelectedIndexChanged="rb_percentage_OnSelectedIndexChanged" AutoPostBack="true"
                                                                Checked="true" />
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rb_amount" runat="server" Text="Amount" GroupName="dts" AutoPostBack="true" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_dtsamt" runat="server" CssClass="textbox txtheight" Style="text-align: right;
                                                                width: 65px; height: 15px;" BackColor="#81F7D8"></asp:TextBox>
                                                            <asp:Button ID="btnadd" Text="Add" CssClass="textbox textbox1" Height="32px" Width="50px"
                                                                runat="server" Style="width: 50px; margin-left: 6px; margin-top: -72px;" OnClick="btnaddgriddts_Click" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <div id="div3" runat="server">
                                                            <asp:GridView ID="gridView3" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                                OnRowDataBound="gridView3_OnRowDataBound" OnDataBound="Marksgrid_pg1_DataBound"
                                                                Width="100px">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                                        HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Label ID="lbl_rs" runat="server" Width="60px" Text='<%#Eval("Sno") %>'></asp:Label>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Credit Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:DropDownList ID="ddl_credit" runat="server" Height="35px" CssClass="textbox textbox1 ddlstyle ddlheight4"
                                                                                    AutoPostBack="true">
                                                                                </asp:DropDownList>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Debit Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:DropDownList ID="ddl_debit" runat="server" Height="35px" CssClass="textbox textbox1 ddlstyle ddlheight4"
                                                                                    AutoPostBack="true">
                                                                                </asp:DropDownList>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Payment" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:TextBox ID="txtpaymt" runat="server" onblur="return addmarks(this)" CssClass="  textbox txtheight1"
                                                                                    Height="17px" Width="90px"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtpaymt"
                                                                                    FilterType="Numbers,Custom" ValidChars=" .">
                                                                                </asp:FilteredTextBoxExtender>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="div_cash" visible="false" runat="server">
                                                <%--style="margin-right: -303px">--%>
                                                <%--style="width: 373px; height:97px; margin-left: 33px; margin-top: 33px;  overflow: auto; ackground-color: White; border-radius: 10px;">--%>
                                                <div id="grid1" runat="server" style="width: 500px; height: 158px; overflow: auto;
                                                    background-color: White; border-radius: 0px;">
                                                    <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                        OnRowDataBound="gridView1_OnRowDataBound" OnDataBound="Marksgrid_pg_DataBound"
                                                        OnRowCommand="gridView1_OnRowCommand" Width="100px" align="left">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                                HeaderStyle-Width="100px">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lbl_rs" runat="server" Width="60px" Text='<%#Eval("Sno") %>'></asp:Label>
                                                                    </center>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Header Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:DropDownList ID="ddl_headername" runat="server" CssClass="textbox ddlheight3"
                                                                            AutoPostBack="true" Width="110px" Visible="true">
                                                                        </asp:DropDownList>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ledger Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:DropDownList ID="ddl_ledgername" runat="server" CssClass="textbox ddlheight3"
                                                                            AutoPostBack="true" Width="110px" Visible="true">
                                                                        </asp:DropDownList>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Payment" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:TextBox ID="txtpaymt" runat="server" onblur="return addmarks(this)" CssClass="  textbox txtheight1"
                                                                            Height="17px" Width="90px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtpaymt"
                                                                            FilterType="Numbers,Custom" ValidChars=" .">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="paidDiv" visible="false" runat="server">
                                                <%--style="margin-right: -303px">--%>
                                                <%--style="width: 373px; height:97px; margin-left: 33px; margin-top: 33px;  overflow: auto; ackground-color: White; border-radius: 10px;">--%>
                                                <div id="Div4" runat="server" style="width: 410px; height: 158px; overflow: auto;
                                                    ackground-color: White; border-radius: 0px;">
                                                    <asp:GridView ID="gdPaid" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                        OnRowDataBound="gdPaid_OnRowDataBound">
                                                        <%-- OnRowDataBound="gridView1_OnRowDataBound" OnDataBound="Marksgrid_pg_DataBound"
                                                        OnRowCommand="gridView1_OnRowCommand"--%>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                                HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lbl_sno" runat="server" Width="60px" Text='<%#Eval("Sno") %>'></asp:Label>
                                                                    </center>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Header Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lblhdName" runat="server" Width="60px" Text='<%#Eval("hdName") %>'></asp:Label>
                                                                        <asp:Label ID="lblhdFk" runat="server" Width="60px" Text='<%#Eval("hdFK") %>' Visible="false"></asp:Label>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ledger Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lblldName" runat="server" Width="60px" Text='<%#Eval("ldName") %>'></asp:Label>
                                                                        <asp:Label ID="lblldFk" runat="server" Width="60px" Text='<%#Eval("ldFK") %>' Visible="false"></asp:Label>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Payment" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:TextBox ID="txtAmt" runat="server" Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                                        <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtAmt"
                                                                            FilterType="Numbers,Custom" ValidChars=" .">
                                                                        </asp:FilteredTextBoxExtender>--%>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divven" runat="server" visible="false" style="width: 540px; height: 287px;
                                                overflow: auto; ackground-color: White; border-radius: 0px; border: 1px solid;">
                                                <div id="divendgrid" runat="server" style="width: 524px; height: 169px; overflow: auto;
                                                    ackground-color: White; border-radius: 0px;">
                                                    <asp:GridView ID="gridven" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                        OnDataBound="gridven_OnDataBound" OnRowDataBound="gridven_OnRowDataBound" OnRowCommand="gridven_OnRowCommand">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'></asp:Label>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:DropDownList ID="ddlheader" runat="server" Width="100px" AutoPostBack="true"
                                                                            CssClass="textbox ddlheight3">
                                                                        </asp:DropDownList>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:DropDownList ID="ddlledger" runat="server" Width="100px" AutoPostBack="true"
                                                                            CssClass="textbox ddlheight3">
                                                                        </asp:DropDownList>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Total Allot" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:TextBox ID="txttotamt" runat="server" CssClass="  textbox txtheight1" Height="17px"
                                                                            Width="56px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1txttotamt" runat="server"
                                                                            TargetControlID="txttotamt" FilterType="Numbers,Custom" ValidChars=". ">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Paid Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:TextBox ID="txtpaidamt" runat="server" CssClass="  textbox txtheight1" Height="17px"
                                                                            Width="56px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1txtpaidamt" runat="server"
                                                                            TargetControlID="txtpaidamt" FilterType="Numbers,Custom" ValidChars=". ">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:TextBox ID="txtbal" runat="server" CssClass="  textbox txtheight1" Height="17px"
                                                                            Width="56px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1Balance" runat="server"
                                                                            TargetControlID="txtbal" FilterType="Numbers,Custom" ValidChars=". ">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="TobePaid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:TextBox ID="txttobepaid" runat="server" onchange="return checkVendorAmt()" CssClass="  textbox txtheight1"
                                                                            Height="17px" Width="56px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1txttobepaid" runat="server"
                                                                            TargetControlID="txttobepaid" FilterType="Numbers,Custom" ValidChars=". ">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Order Id" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:TextBox ID="txtorderid" runat="server" CssClass="  textbox txtheight1" Height="17px"
                                                                            Width="56px"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1txtorderid" runat="server"
                                                                            TargetControlID="txtorderid" FilterType="Numbers" ValidChars=". ">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <br />
                                                <center>
                                                    <asp:Button ID="btn_history" runat="server" Text="History" CssClass="textbox textbox1"
                                                        Style="height: 30px; width: 56px;" OnClick="btn_history_OnClick" />
                                                </center>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="grid1btn" runat="server" visible="false" style="width: 50px; margin-left: 384px;
                                    margin-top: -189px;">
                                    <asp:Button ID="btnaddgrid" Text="Add" CssClass="textbox textbox1" Height="32px"
                                        Width="50px" runat="server" Style="width: 50px; margin-left: 6px; margin-top: -72px;"
                                        OnClick="btnaddgrid_Click" />
                                </div>
                            </div>
                            <div id="divnar" runat="server" style="margin-left: 10px; margin-top: -65px; position: absolute;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" Text="Narration" runat="server" Visible="true"></asp:Label>
                                            <asp:TextBox ID="txt_narr" runat="server" Width="347px" CssClass="txtheight3" Visible="true"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendernarr" runat="server" TargetControlID="txt_narr"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=".,$*#/&-+(){}[]\ ">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:Button ID="btnpayment" runat="server" Text="Payment" OnClientClick="return checkInstallDate()"
                                                OnClick="btnpayment_Click" CssClass="textbox textbox1" Height="32px" Width="60px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cbClgFormat" runat="server" Visible="true" Checked="true" Text="Include Fourteen Format" />
                                            <asp:CheckBox ID="cbledgdet" runat="server" Visible="false" Text="Include Ledger Detail" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <%--totalpayamount--%>
                        <div id="bankandpaytot" runat="server" style="width: 460px; height: 300px; margin-top: 302px;
                            margin-left: 1px; border: 1px solid; position: absolute;">
                            <br />
                            <div id="bank" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            Account No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_acno" runat="server" CssClass="txtheight5 txtcaps" OnTextChanged="txt_acno_OnTextChanged"
                                                AutoPostBack="true">
                                            </asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getname4" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_acno"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Available Amt
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtavailAmt" Enabled="false" Width="100px" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Account Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_acname" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txt_acname"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Bank Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_bnkname" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_bnkname"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Branch
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_branch" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderbranch" runat="server" TargetControlID="txt_branch"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Cheque Date
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_cal" runat="server" CssClass="textbox txtheight2 textbox2" Width="100px"
                                                        ReadOnly="true"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtenderche" runat="server" TargetControlID="txt_cal"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Amount
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_amt" runat="server" CssClass="txtheight3 txtcaps" onblur="return checkFloatValue(this)"
                                                onchange="return bankAmount()">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxt_amt" runat="server" TargetControlID="txt_amt"
                                                FilterType="Numbers,Custom" ValidChars=" .">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Cheque No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_chqno" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox><span style="color: Red;">*</span>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_chqno"
                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <br />
                        <div id="instal" runat="server" visible="true" style="width: 540px; height: 300px;
                            float: left; border: 1px solid; margin-left: 462px; margin-top: 0px;">
                            <div id="divinst" runat="server" style="float: left;">
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            If Installment
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkinstall" runat="server" AutoPostBack="true" OnCheckedChanged="chkinstall_OnCheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_instal" Text="No of Installment" runat="server" Visible="false"></asp:Label>
                                            <asp:TextBox ID="TextBox1" runat="server" Width="30px" CssClass="txtheight3 txtcaps"
                                                OnTextChanged="TextBox1_OnTextChanged" AutoPostBack="true" Visible="false" MaxLength="1">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="TextBox1"
                                                FilterType="Numbers" ValidChars="">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <center>
                                <div id="divbank" runat="server" visible="false">
                                    <div style="width: 399px; overflow: auto; background-color: White; border-radius: 0px;
                                        height: 200px;">
                                        <br />
                                        <asp:GridView ID="gridView2" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                            OnDataBound="bankgrid_pg_DataBound" Width="350px">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_rs" runat="server" Width="90px" Text='<%#Eval("Sno") %>'></asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtdate" runat="server" CssClass=" textbox" Height="17px" Width="130px"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="CalendarExtenderdt" runat="server" TargetControlID="txtdate"
                                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                    </asp:CalendarExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:TextBox ID="txtamt" runat="server" CssClass="  textbox" Height="17px" Width="130px"
                                                                onblur="return addmarkss(this)"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxt" runat="server" TargetControlID="txtamt"
                                                                FilterType="Numbers" ValidChars="">
                                                            </asp:FilteredTextBoxExtender>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </center>
                        </div>
                    </div>
            </div>
        </center>
        <center>
            <div id="popupstud" runat="server" class="popupstyle" visible="false" style="height: 50em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0;">
                <asp:ImageButton ID="imgbtn2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 410px;"
                    OnClick="imagebtnpop2close_Click" />
                <br />
                <div style="background-color: White; height: 547px; width: 840px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Select the Student</span></div>
                        <br />
                    </center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_pop2collgname" Text="College Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2collgname" runat="server" CssClass="textbox ddlheight5 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_pop2collgname_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2batchyr" Text="Batch Year" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2batchyear" runat="server" CssClass="textbox textbox1"
                                    Style="height: 30px; width: 120px;" AutoPostBack="true" onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Stream
                            </td>
                            <td>
                                <asp:TextBox ID="txt_str" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="pnl_str" runat="server" CssClass="multxtpanel " Style="width: 121px;
                                    height: 100px;">
                                    <asp:CheckBox ID="chk_str" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="chk_str_OnCheckedChanged" />
                                    <asp:CheckBoxList ID="chkl_str" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkl_str_OnSelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_str"
                                    PopupControlID="pnl_str" Position="Bottom">
                                </asp:PopupControlExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_pop2degre" Text="Degree" runat="server" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2degre" runat="server" CssClass="textbox ddlheight2 textbox1"
                                    OnSelectedIndexChanged="ddl_pop2degre_SelectedIndexChanged" AutoPostBack="true"
                                    onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2branch" Text="Branch" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2branch" runat="server" CssClass="textbox textbox1"
                                    Style="height: 30px; width: 135px;" AutoPostBack="true" OnSelectedIndexChanged="ddl_pop2branch_OnSelectedIndexChanged"
                                    onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Semester
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsem1" runat="server" AutoPostBack="True" CssClass="ddlheight1 textbox textbox1"
                                    OnSelectedIndexChanged="ddlsem1_selectedchanged" Width="76px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Section
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_sect" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sect" runat="server" CssClass="textbox textbox1" Style="height: 20px;
                                            width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_sect" runat="server" CssClass="multxtpanel" Style="width: 100px;
                                            height: 100px;">
                                            <asp:CheckBox ID="cb_sect" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_sect_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_sect" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sect_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtendersec" runat="server" TargetControlID="txt_sect"
                                            PopupControlID="panel_sect" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2sex" Text="Sex" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2sex" runat="server" CssClass="textbox ddlheight2 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2studenttype" Text="Student Type" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2studenttype" runat="server" CssClass="textbox textbox1 ddlheight2"
                                    AutoPostBack="true" onfocus="return myFunction1(this)">
                                    <asp:ListItem Value="Hostler','Day Scholar">Both</asp:ListItem>
                                    <asp:ListItem Value="Hostler">Hostler</asp:ListItem>
                                    <asp:ListItem Value="Day Scholar">Day Scholar</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btn_popupstud" Text="Go" CssClass="textbox btn1" runat="server" OnClick="btn_popupstud_Click" />
                            </td>
                        </tr>
                    </table>
                    <center>
                        <br />
                        <div>
                            <asp:Label ID="lblpop2error" runat="server" ForeColor="Red" Visible="false">
                            </asp:Label>
                        </div>
                    </center>
                    <br />
                    <center>
                        <div id="div2" runat="server" visible="false" style="width: 691px; height: 261px;
                            box-shadow: 0px 0px 8px #999999;" class="reportdivstyle">
                            <FarPoint:FpSpread ID="fproll" runat="server" Visible="false" Style="overflow: auto;
                                border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                class="reportdivstyle">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <%-- </div>--%>
                        </div>
                    </center>
                    <br />
                    <asp:Button ID="btn_popupstud_save" Text="Ok" runat="server" CssClass="textbox btn2"
                        Visible="false" OnClick="btn_popupstud_save_Click" />
                    <asp:Button ID="btn_popupstud_exit" Text="Exit" runat="server" CssClass="textbox btn2"
                        Visible="false" OnClick="btn_popupstud_exit_Click" />
                </div>
            </div>
        </center>
        <%-- <staffcodediv>--%>
        <center>
            <div id="popupstaff" runat="server" visible="false" class="popupstyle popupheight"
                style="background-color: rgba(54, 25, 25, .2);">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 436px;"
                    OnClick="imagebtnpopsscode_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 515px; width: 900px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_selectstaffcode" runat="server" Style="font-size: large; color: Green;"
                            Text="Select the Staff Name"></asp:Label>
                    </center>
                    <br />
                    <div>
                        <center>
                            <table class="maintablestyle" style="width: 66%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_college2" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_college2" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                            CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_department3" runat="server" Text="Department"></asp:Label>
                                        <asp:DropDownList ID="ddl_department3" Width="180px" Height="30px" runat="server"
                                            AutoPostBack="true" CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_searchby" runat="server" Text="Search By"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_searchby" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_searchby_SelectedIndexChanged" CssClass="textbox textbox1">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                            <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_searchby" Visible="true" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_searchby"
                                            FilterType="uppercaseletters,lowercaseletters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname1" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_wardencode" Visible="false" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_wardencode"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btn_popupstaff" runat="server" CssClass="textbox btn1" Text="Go"
                                            OnClick="btn_popupstaff_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <p>
                                    <asp:Label ID="lbl_errorsearch" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                </p>
                                <p>
                                    <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                                        ForeColor="Red"></asp:Label>
                                </p>
                                <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Style="width: 723px;
                                    height: 261px; overflow: auto; border: 0px solid #999999; border-radius: 5px;
                                    background-color: White; box-shadow: 0px 0px 8px #999999;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_popupstaff_save" runat="server" CssClass="textbox textbox1" Style="height: 28px;
                                        width: 65px;" Text="Save" Visible="false" OnClick="btn_popupstaff_save_Click" />
                                    <asp:Button ID="btn_popupstaff_exit" runat="server" CssClass="textbox textbox1" Style="height: 28px;
                                        width: 65px;" Text="Exit" OnClick="btn_popupstaff_exit_Click" Visible="false" />
                                </div>
                            </center>
                        </center>
                        <center>
                            <asp:Label ID="err" ForeColor="Red" Visible="false" runat="server"></asp:Label></center>
                    </div>
                </div>
            </div>
        </center>
        <%-- <vendor>--%>
        <center>
            <div id="popupvender" runat="server" visible="false" class="popupstyle popupheight"
                style="background-color: rgba(54, 25, 25, .2);">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 9px; margin-left: 360px;"
                    OnClick="ImageButton2_Click" />
                <br />
                <div style="background-color: White; height: 489px; width: 754px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="Labelvensel" runat="server" class="fontstyleheader" Style="font-size: large;
                            color: #0AA7B3;" Text="Select the Vender Name"></asp:Label>
                    </center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_vendorName" runat="server" Text="Vendor Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_vencomname" runat="server" CssClass="textbox textbox1" Style="height: 20px;
                                            width: 170px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel1" runat="server" CssClass="multxtpanel" Style="width: 188px;
                                            height: 100px;">
                                            <asp:CheckBox ID="chk_vencomname" runat="server" Width="170px" Text="Select All"
                                                AutoPostBack="True" OnCheckedChanged="chk_vencomname_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="chkl_vencomname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_vencomname_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_vencomname"
                                            PopupControlID="panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btn_vendor_go" runat="server" Width="100px" CssClass="textbox btn1"
                                    Text="Go" OnClick="btn_vendor_go_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                            Font-Size="Medium"></asp:Label>
                        <center>
                            <asp:Label ID="lbl_errormsg" runat="server" Style="color: Red;"></asp:Label></center>
                    </div>
                    <br />
                    <br />
                    <div id="divFpsvender" runat="server" visible="false" style="width: 550px; height: 250px;
                        box-shadow: 0px 0px 8px #999999;" class="reportdivstyle">
                        <FarPoint:FpSpread ID="Fpsvender" runat="server" Style="overflow: auto; height: 247px;
                            border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            class="reportdivstyle">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <div>
                        <asp:Button ID="btn_vendor_save" runat="server" CssClass="textbox btn2" Text="Save"
                            Visible="false" OnClick="btn_vendor_save_Click" />
                        <asp:Button ID="btn_vendor_exit" runat="server" CssClass="textbox btn2" Text="Exit"
                            OnClick="btn_vendor_exit_Click" Visible="false" />
                    </div>
                    <center>
                        <asp:Label ID="Labelven" ForeColor="Red" Visible="false" runat="server"></asp:Label></center>
                </div>
            </div>
        </center>
        <center>
            <div id="popuporderid" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="imgbtn3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 86px; margin-left: 240px;"
                    OnClick="imagebtnorder_Click" />
                <br />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 400px; width: 500px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <center>
                        <br />
                        <asp:Label ID="lbl_selectbuild" runat="server" Style="font-size: large; color: #790D03;"
                            Text="Select Order"></asp:Label>
                        <br />
                        <br />
                    </center>
                    <div>
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_order" runat="server" Text="Order Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_order" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                            Width="200px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txt_order"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go_order" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_order_Click" />
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                            </table>
                            <center>
                                <div>
                                    <p>
                                        <asp:Label ID="lbl_error3" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </p>
                                    <FarPoint:FpSpread ID="fpbuild" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                        height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                        box-shadow: 0px 0px 8px #999999;">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                                <br />
                                <br />
                                <center>
                                    <div>
                                        <asp:Button ID="btn_order_save" runat="server" Visible="false" CssClass="textbox btn2"
                                            Text="Ok" OnClick="btn_order_save_Click" />
                                        <asp:Button ID="btn_order_exit" runat="server" Visible="false" CssClass="textbox btn2"
                                            Text="Exit" OnClick="btn_order_exit_Click" />
                                    </div>
                                </center>
                            </center>
                        </center>
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
        <%---------end of popup--------%>
        <div id="imgdiv5" runat="server" visible="false" style="height: 56em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="panel_header" runat="server" visible="false" class="table" style="background-color: White;
                    height: auto; width: 367px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_header1" runat="server" Style="color: Green; font: bold;" Text="Identification"></asp:Label>
                            </td>
                            <br />
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_header" runat="server" Height="25px" onfocus=" return display(this)"
                                    Style="text-transform: capitalize;" CssClass="textbox textbox1" Width="232px"></asp:TextBox>
                            </td>
                        </tr>
                        <br />
                        <tr>
                            <td align="center">
                                <br />
                                <asp:Button ID="btn_addheader" runat="server" Visible="true" CssClass="textbox btn2"
                                    Text="Add" OnClientClick="return checkadd()" OnClick="btn_addheader_Click" />
                                <asp:Button ID="btn_exitheader" runat="server" Visible="true" CssClass="textbox btn2"
                                    Text="Exit" OnClick="btn_exitheader_Click" />
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
        <center>
            <div id="history" runat="server" visible="false" style="height: 56em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="hist" runat="server" style="background-color: White; overflow: auto; height: 405px;
                        width: 656px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px;
                        border-radius: 10px;">
                        <asp:Label ID="Label2" runat="server" Text="History" Style="color: Green; font-size: 25px;"></asp:Label>
                        <div id="divgr" runat="server" style="height: 300px; width: 656px; overflow: auto;">
                            <center>
                                <asp:GridView ID="gridhty" runat="server" AutoGenerateColumns="false" GridLines="Both">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Order Id" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txtorderid" runat="server"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtorderid" runat="server"
                                                        FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers" ValidChars=" "
                                                        TargetControlID="txtorderid">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:UpdatePanel ID="UpdatePanel5hty" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txthtydate" runat="server" CssClass=" textbox" Height="17px" Width="130px"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtenderdtt" runat="server" TargetControlID="txthtydate"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Voucher No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txtvocuherno" runat="server" CssClass="  textbox txtheight1" Height="17px"
                                                        Width="56px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderVoucherNo" runat="server"
                                                        FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers" ValidChars=" "
                                                        TargetControlID="txtvocuherno">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Mode" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txtmode" runat="server" CssClass="  textbox txtheight1" Height="17px"
                                                        Width="56px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderMode" runat="server" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers"
                                                        ValidChars=". " TargetControlID="txtmode">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Debit" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txtdebit" runat="server" CssClass="  textbox txtheight1" Height="17px"
                                                        Width="56px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderDebit" runat="server" FilterType="Numbers,Custom"
                                                        ValidChars=". " TargetControlID="txtdebit">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Credit" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txtcredit" runat="server" CssClass="  textbox txtheight1" Height="17px"
                                                        Width="56px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderCredit" runat="server" FilterType="Numbers,Custom"
                                                        ValidChars=". " TargetControlID="txtcredit">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </center>
                        </div>
                    </div>
                </center>
                <center>
                    <div>
                        <asp:Button ID="btnhtok" runat="server" Text="Ok" Style="margin-left: -41px; margin-top: -55px;
                            position: absolute;" CssClass="textbox textbox1" Height="32px" Width="60px" OnClick="btnhtok_OnClick" />
                    </div>
                </center>
            </div>
        </center>
        <%-- New Print div--%>
        <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
            </div>
        </div>
    </body>
    </html>
</asp:Content>
