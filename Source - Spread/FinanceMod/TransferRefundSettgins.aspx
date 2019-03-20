<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TransferRefundSettgins.aspx.cs" Inherits="TransferRefundSettgins" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="Shortcut Icon" href="college/Left_Logo.jpeg" />
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
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
        .maindivstyle
        {
            border: 1px solid #999999;
            background-color: #F0F0F0;
            box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
            -moz-box-shadow: 0px 0px 10px #999999;
            -webkit-box-shadow: 0px 0px 10px #999999;
            border: 3px solid #D9D9D9;
            border-radius: 15px;
        }
        .subdivstyle
        {
            border: 5px solid #0CA6CA;
            border-top: 30px solid #0CA6CA;
            border-radius: 10px;
        }
        .maindivstyle1
        {
            border: 1px solid #999999;
            border-radius: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#<%=btntransind.ClientID %>').click(function () {
                    var lblOldPaid = $('#<%=Label3.ClientID %>').text();
                    var lblNewPaid = $('#<%=Label6.ClientID %>').text();

                    var oldAmt = lblOldPaid.split('.')[1];
                    var NewAmt = lblNewPaid.split('.')[1];
                    var strValue = "";
                    var value = "";
                    var journal = document.getElementById("<%=rb_Journal.ClientID %>");

                    if (!journal.checked) {//using transfer only
                        if (oldAmt == 0 && NewAmt == 0)
                            strValue = "0";
                        if (oldAmt != 0 && NewAmt == 0)
                            strValue = "1";
                        if (oldAmt != 0 && NewAmt != 0)
                            strValue = "2";
                        if (strValue != "") {
                            if (strValue == "0")
                                var value = confirm("Do you want continue without paid amount?");
                            if (strValue == "1")
                                var value = confirm("Do you want continue without using paid amount?");
                            if (strValue == "2")
                                var value = confirm("Do you want continue?");
                            if (!value)
                                return false;
                        }
                    }
                    else {
                        lblNewPaid = $('#<%=Label9.ClientID %>').text();
                        NewAmt = lblNewPaid.split('.')[1];
                        if (oldAmt != 0 && NewAmt != 0) {
                            if (parseFloat(oldAmt) >= parseFloat(NewAmt)) {
                                var value = confirm("Do you want continue?");
                                if (!value)
                                    return false;
                            }
                            else {
                                alert("Please Enter The Equal Paid Amount!");
                                return false;
                            }
                        }
                        else {
                            alert("Please Enter Paid Amount");
                            return false;
                        }
                    }
                });
            });
        </script>
        <script type="text/javascript">

            function OpenConfirmDialog() {
                if (confirm('Do you want continue?')) {
                    //True .. do something
                }
                else {
                    //False .. do something
                }
            }


            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
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

            function checkrefundamount() {
                var totrefund = 0.00;
                var totalgetvalue = 0.00;
                var txttotalrefamt = document.getElementById("<%=txt_AmtPerc.ClientID %>");
                var ddlPercAmt = document.getElementById("<%=ddl_AmtPerc.ClientID %>");


                var tbl = document.getElementById("<%=gridView3.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");

                for (var i = 0; i < (gridViewControls.length); i++) {

                    var lbltotal = document.getElementById('MainContent_gridView3_lbl_totamt_' + i.toString());
                    var lblbal = document.getElementById('MainContent_gridView3_lbl_bal_' + i.toString());
                    var lblpaid = document.getElementById('MainContent_gridView3_lbl_paid_' + i.toString());
                    var lblAlrefund = document.getElementById('MainContent_gridView3_lbl_AlRefunded_' + i.toString());
                    var txtrefund = document.getElementById('MainContent_gridView3_txt_refund_' + i.toString());
                    var txtrefundbal = document.getElementById('MainContent_gridView3_txt_refundbal_' + i.toString());
                    if (lblpaid != null && txtrefund != null) {
                        if (lblpaid.innerHTML != "" && txtrefund.value != "") {

                            if ((parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) >= parseFloat(txtrefund.value)) {
                                totrefund += parseFloat(txtrefund.value);

                                if ((parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) >= parseFloat(txtrefund.value)) {
                                    txtrefundbal.value = (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) - parseFloat(txtrefund.value);
                                    totalgetvalue += parseFloat(txtrefundbal.value);
                                }
                                else {
                                    txtrefundbal.value = "0";
                                }
                            }
                            else {

                                txtrefundbal.value = parseFloat(txtrefund.value) - (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML));
                                totalgetvalue += parseFloat(txtrefundbal.value);
                                txtrefund.value = "0";

                            }
                        }
                        else {
                            txtrefundbal.value = "0";
                        }

                    }

                }

                txttotalrefamt.value = totrefund.toString();
                document.getElementById("<%=txt_reamt.ClientID %>").value = totalgetvalue;

            }


            function refundamt() {
                var totrefund = parseFloat(document.getElementById("<%=txt_AmtPerc.ClientID %>").value);

                var txttotalrefamt = document.getElementById("<%=txt_AmtPerc.ClientID %>");
                var ddlPercAmt = document.getElementById("<%=ddl_AmtPerc.ClientID %>");

                var tbl = document.getElementById("<%=gridView3.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");
                var totalgetvalue = 0;
                for (var i = 0; i < (gridViewControls.length); i++) {

                    var lbltotal = document.getElementById('MainContent_gridView3_lbl_totamt_' + i.toString());
                    var lblbal = document.getElementById('MainContent_gridView3_lbl_bal_' + i.toString());
                    var lblpaid = document.getElementById('MainContent_gridView3_lbl_paid_' + i.toString());
                    var lblAlrefund = document.getElementById('MainContent_gridView3_lbl_AlRefunded_' + i.toString());
                    var txtrefund = document.getElementById('MainContent_gridView3_txt_refund_' + i.toString());
                    var txtrefundbal = document.getElementById('MainContent_gridView3_txt_refundbal_' + i.toString());

                    if (lblpaid != null && txtrefund != null) {
                        if (lblpaid.innerHTML != "") {
                            if (ddlPercAmt.value == "Amount") {

                                if ((parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) <= totrefund) {
                                    totrefund -= (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML));
                                    txtrefund.value = (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)).toString();
                                    if ((parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) >= parseFloat(txtrefund.value)) {

                                        txtrefundbal.value = (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) - parseFloat(txtrefund.value);
                                        totalgetvalue += parseFloat(txtrefundbal.value);
                                    }



                                }
                                else {
                                    txtrefund.value = totrefund.toString();
                                    txtrefundbal.value = parseFloat(txtrefund.value) - (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML));
                                    totalgetvalue += parseFloat(txtrefundbal.value);
                                    totrefund = 0;
                                }

                            }
                            else {
                                if (totrefund >= 0 && totrefund < 101) {
                                    txtrefund.value = (((parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) * totrefund) / 100).toString();
                                    txtrefundbal.value = (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) - parseFloat(txtrefund.value);
                                    totalgetvalue += parseFloat(txtrefundbal.value);
                                }
                                else {
                                    // totalgetvalue += parseFloat(txtrefundbal.value);
                                    txtrefundbal.value = "0";
                                    txtrefund.value = "0";
                                }
                            }
                        }
                    }
                }
                document.getElementById("<%=txt_reamt.ClientID %>").value = totalgetvalue;

            }
        </script>
        <script type="text/javascript">
            function checkTobepaidamount() {
                var journal = document.getElementById("<%=rb_Journal.ClientID %>");
                var transFer = document.getElementById("<%=rb_transfer.ClientID %>");
                var selectedText = "";
                if (journal.checked) {
                    var skillsSelect = document.getElementById("<%=ddlJournalType.ClientID %>");
                    selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
                }
                var journal = document.getElementById("<%=rb_Journal.ClientID %>");
                var transFer = document.getElementById("<%=rb_transfer.ClientID %>");
                var lblPaid = document.getElementById("<%=Label6.ClientID %>");
                var totalamt = document.getElementById("<%=Label8.ClientID %>");
                var lblnewPaid = document.getElementById("<%=Label9.ClientID %>");
                var lblnewhidd = document.getElementById("<%=hiddnewPaid.ClientID %>");


                //                lblPaid.textContent = "";
                //                totalamt.textContent = "";
                //                lblnewPaid.textContent = "";
                var excesTotAmt = 0;
                var totalPaidAmt = 0;
                var totalBalAmt = 0;
                var newPaidAmt = 0;
                var paidGrid = document.getElementById("<%=gridView5.ClientID %>");
                var gridcontrolsDetails = paidGrid.getElementsByTagName("input");
                for (var row = 0; row < (gridcontrolsDetails.length); row++) {
                    var lblhiddenFld = document.getElementById('MainContent_gridView5_hdfTobePaid_' + row.toString());
                    var lblTotal = document.getElementById('MainContent_gridView5_lbl_totamt_' + row.toString());
                    var txtPaid = document.getElementById('MainContent_gridView5_txt_paid_' + row.toString());
                    var txtTobePaid = document.getElementById('MainContent_gridView5_txt_tobePaid_' + row.toString());
                    var txtBal = document.getElementById('MainContent_gridView5_txt_bal_' + row.toString());
                    var txtExcess = document.getElementById('MainContent_gridView5_txt_exGrid2_' + row.toString());

                    if (lblTotal != null && txtPaid != null && txtTobePaid != null && txtTobePaid != "") {
                        var totalAmt = parseFloat(lblTotal.innerHTML);
                        var paidAmt = parseFloat(txtPaid.value);
                        var tobePaidAmt = parseFloat(txtTobePaid.value);
                        var balAmt = parseFloat(txtBal.value);
                        var hiddenToBeAmt = parseFloat(lblhiddenFld.value);
                        var excessAmount = parseFloat(txtExcess.value);
                        if (transFer.checked || (journal.checked && selectedText == "Already Paid Amount")) {
                            if (tobePaidAmt != null && tobePaidAmt != 0) {
                                //bal amount
                                var tempBalAmt = (balAmt + hiddenToBeAmt) - excessAmount;
                                txtBal.value = tempBalAmt.toString();
                                //paidamount
                                var tempPaidamount = (paidAmt + excessAmount) - hiddenToBeAmt;
                                txtPaid.value = tempPaidamount.toString();

                                paidAmt = parseFloat(txtPaid.value);
                                balAmt = parseFloat(txtBal.value);

                                var tempPaid = paidAmt + tobePaidAmt; //paid and tobe paidamt
                                if (totalAmt >= tempPaid) {
                                    txtPaid.value = tempPaid.toString();
                                    var tempBalAmt = totalAmt - tempPaid;
                                    txtBal.value = tempBalAmt.toString();
                                    totalPaidAmt += tempPaid;
                                    totalBalAmt += tempBalAmt;
                                    txtTobePaid.value = tobePaidAmt;
                                    lblhiddenFld.value = tobePaidAmt;
                                    newPaidAmt += tobePaidAmt;
                                    txtExcess.value = 0;
                                    excesTotAmt += 0;
                                }
                                else {
                                    var tempEx = tempPaid - totalAmt;
                                    txtPaid.value = totalAmt.toString();
                                    var tempBalAmt = 0;
                                    txtBal.value = tempBalAmt.toString();
                                    totalPaidAmt += totalAmt;
                                    totalBalAmt += 0;
                                    txtTobePaid.value = tobePaidAmt;
                                    lblhiddenFld.value = tobePaidAmt;
                                    txtExcess.value = tempEx.toString();
                                    excesTotAmt += tempEx;
                                }
                            }
                            else {
                                //bal amot
                                var excessAmount = parseFloat(txtExcess.value);
                                var tempBalAmt = (balAmt + hiddenToBeAmt) - excessAmount;
                                totalBalAmt += parseFloat(tempBalAmt);
                                txtBal.value = tempBalAmt.toString();
                                txtExcess.value = 0;
                                //paid amount
                                var tempPaidamount = (paidAmt + excessAmount) - hiddenToBeAmt;
                                totalPaidAmt += tempPaidamount;
                                txtPaid.value = tempPaidamount.toString();
                                lblhiddenFld.value = 0;
                                excesTotAmt += 0;
                            }
                        }
                        else {
                            if (tobePaidAmt != null && tobePaidAmt != 0) {//journal advance and excess amount
                                //bal amount
                                var tempBalAmt = balAmt + hiddenToBeAmt;
                                txtBal.value = tempBalAmt.toString();
                                //paidamount
                                var tempPaidamount = paidAmt - hiddenToBeAmt;
                                txtPaid.value = tempPaidamount.toString();

                                paidAmt = parseFloat(txtPaid.value);
                                balAmt = parseFloat(txtBal.value);

                                var tempPaid = paidAmt + tobePaidAmt; //paid and tobe paidamt
                                if (totalAmt >= tempPaid) {
                                    txtPaid.value = tempPaid.toString();
                                    var tempBalAmt = totalAmt - tempPaid;
                                    txtBal.value = tempBalAmt.toString();
                                    totalPaidAmt += tempPaid;
                                    totalBalAmt += tempBalAmt;
                                    txtTobePaid.value = tobePaidAmt;
                                    lblhiddenFld.value = tobePaidAmt;
                                    newPaidAmt += tobePaidAmt;
                                }
                                else {
                                    totalPaidAmt += tempPaidamount;
                                    totalBalAmt += tempBalAmt;
                                    txtTobePaid.value = 0;
                                    lblhiddenFld.value = 0;
                                    newPaidAmt += tobePaidAmt;
                                }
                            }
                            else {
                                //bal amot
                                var tempBalAmt = balAmt + hiddenToBeAmt;
                                totalBalAmt += tempBalAmt;
                                txtBal.value = tempBalAmt.toString();
                                //paid amount
                                var tempPaidamount = paidAmt - hiddenToBeAmt;
                                totalPaidAmt += tempPaidamount;
                                txtPaid.value = tempPaidamount.toString();
                                lblhiddenFld.value = 0;
                            }
                        }
                    }
                }
                lblPaid.textContent = "Rs." + totalPaidAmt.toString();
                totalamt.textContent = "Rs." + totalBalAmt.toString();
                if (journal.checked) {
                    lblnewPaid.textContent = "Rs." + newPaidAmt.toString();
                    lblnewhidd.value = newPaidAmt.toString();
                }
                else {
                    lblnewPaid.textContent = "Rs." + excesTotAmt.toString();
                    lblnewhidd.value = excesTotAmt.toString();
                }
            }
            function Paidamount() {

                var journal = document.getElementById("<%=rb_Journal.ClientID %>");
                var transFer = document.getElementById("<%=rb_transfer.ClientID %>");
                var lblPaid = document.getElementById("<%=Label3.ClientID %>");
                var totalPaidAmt = 0;
                var paidGrid = document.getElementById("<%=gridView4.ClientID %>");
                var selectedText = "";
                if (journal.checked) {
                    var skillsSelect = document.getElementById("<%=ddlJournalType.ClientID %>");
                    selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
                }
                var gridcontrolsDetails = paidGrid.getElementsByTagName("input");
                if (transFer.checked || (journal.checked && selectedText != "Already Paid Amount")) {//transfer only                    
                    for (var row = 0; row < (gridcontrolsDetails.length - 1); row++) {
                        var chkSelectid = document.getElementById('MainContent_gridView4_cbsel_' + row.toString());
                        if (chkSelectid.checked) {
                            var lblTotal = "";
                            if (journal.checked)
                                lblTotal = document.getElementById('MainContent_gridView4_lbl_bal_' + row.toString());
                            else
                                lblTotal = document.getElementById('MainContent_gridView4_lbl_paid_' + row.toString());
                            if (lblTotal != null) {
                                var balamt = parseFloat(lblTotal.innerHTML);
                                totalPaidAmt += balamt;
                            }
                        }
                    }
                    lblPaid.innerHTML = "Rs." + totalPaidAmt.toString();
                }
                else {//journal                    
                    for (var row = 0; row < (gridcontrolsDetails.length - 1); row++) {
                        var chkSelectid = document.getElementById('MainContent_gridView4_cbsel_' + row.toString());
                        if (chkSelectid.checked) {
                            var lblTotal = "";
                            lblTotal = document.getElementById('MainContent_gridView4_lbl_bal_' + row.toString());
                            var hdFk = document.getElementById('MainContent_gridView4_lbl_hdr_' + row.toString());
                            var ldFK = document.getElementById('MainContent_gridView4_lbl_lgr_' + row.toString());
                            var feecat = document.getElementById('MainContent_gridView4_lbl_yearsem_' + row.toString());
                            var rightGrid = document.getElementById("<%=gridView5.ClientID %>");
                            var gridRightDet = rightGrid.getElementsByTagName("input");
                            for (var nRow = 0; nRow < (gridRightDet.length); nRow++) {
                                var NewhdFk = document.getElementById('MainContent_gridView5_lbl_hdr_' + nRow.toString());
                                var NewldFK = document.getElementById('MainContent_gridView5_lbl_lgr_' + nRow.toString());
                                var Newfeecat = document.getElementById('MainContent_gridView5_lbl_yearsem_' + nRow.toString());
                                var oldPaidAmt = document.getElementById('MainContent_gridView5_txt_paid_' + nRow.toString());
                                var tobepaid = document.getElementById('MainContent_gridView5_txt_tobePaid_' + nRow.toString());
                                //var txtNarration = document.getElementById('MainContent_gridView5_txt_narration_' + row.toString());
                                //tobepaid.disabled = false;
                                if (oldPaidAmt != null && oldPaidAmt != "") {
                                    var oldPaid = parseFloat(oldPaidAmt.value);
                                    if (oldPaidAmt != "0" && oldPaidAmt != "0.00") {
                                        if (hdFk.innerHTML == NewhdFk.innerHTML && ldFK.innerHTML == NewldFK.innerHTML && feecat.innerHTML == Newfeecat.innerHTML) {
                                            tobepaid.disabled = true;
                                        }
                                    }
                                }
                            }
                            if (lblTotal != null) {
                                var balamt = parseFloat(lblTotal.innerHTML);
                                totalPaidAmt += balamt;
                            }
                        }

                    }
                    lblPaid.innerHTML = "Rs." + totalPaidAmt.toString();
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Discontinue/Prolong Absent</span></div>
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
                                CssClass="textbox ddlstyle ddlheight3" Width="300px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table class="maintablestyle" style="width: 1010px; text-align: left;">
                    <tr>
                        <td>
                            <fieldset style="height: 12px; width: 612px;">
                                <asp:RadioButton ID="rb_transfer" runat="server" Text="Transfer" GroupName="s1" OnCheckedChanged="rb_transfer_Change"
                                    AutoPostBack="true" />
                                <asp:RadioButton ID="rb_discont" runat="server" Text="Discontinue" GroupName="s1"
                                    OnCheckedChanged="rb_discont_Change" AutoPostBack="true" />
                                <asp:RadioButton ID="rb_refund" runat="server" Text="Refund" GroupName="s1" OnCheckedChanged="rb_refund_Change"
                                    AutoPostBack="true" />
                                <asp:RadioButton ID="rb_Journal" runat="server" Text="Journal" GroupName="s1" OnCheckedChanged="rb_Journal_Change"
                                    AutoPostBack="true" />
                                <asp:RadioButton ID="rb_ProlongAbsent" runat="server" Text="ProlongAbsent" GroupName="s1"
                                    OnCheckedChanged="rb_ProlongAbsent_Change" AutoPostBack="true" />
                                <asp:RadioButton ID="rb_canceltranshostel" runat="server" Text="Cancel Transport/Hostel"
                                    GroupName="s1" OnCheckedChanged="rb_refund_Change" AutoPostBack="true" />
                            </fieldset>
                        </td>
                        <td id="ftype" runat="server" visible="false">
                            <div id="journal" runat="server">
                                <fieldset id="ftype1" style="height: 12px; width: 450px;">
                                    <asp:RadioButtonList ID="rbl_rollnoNew" runat="server" Height="10px" Width="300px"
                                        RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbl_rollnoNew_OnSelectedIndexChanged">
                                        <asp:ListItem Selected="True">Student</asp:ListItem>
                                        <asp:ListItem>Staff</asp:ListItem>
                                        <asp:ListItem>Vendor</asp:ListItem>
                                        <asp:ListItem>Others</asp:ListItem>
                                    </asp:RadioButtonList>
                                </fieldset>
                            </div>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbdisWithoutFees" Visible="false" runat="server" Text="Without Fees" />
                        </td>
                        <td id="tdJournalType" runat="server" visible="false">
                            <fieldset id="Fieldset1" runat="server" style="height: 20px; width: 165px;">
                                <asp:DropDownList ID="ddlJournalType" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="166px" AutoPostBack="true" OnSelectedIndexChanged="ddlJournalType_indexChanged">
                                    <asp:ListItem Text="Advanc/Transfer Amount"></asp:ListItem>
                                    <asp:ListItem Text="Already Paid Amount"></asp:ListItem>
                                    <asp:ListItem Text="Excess Amount"></asp:ListItem>
                                    <asp:ListItem Text="Adjust Scholarship Amount"></asp:ListItem>
                                </asp:DropDownList>
                                <%--AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_indexChanged"--%>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset id="fldapplied" runat="server" visible="false" style="height: 12px; width: 300px;">
                                <asp:RadioButtonList ID="rbl_AdmitTransfer" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="rbl_AdmitTransfer_OnSelectedIndexChanged">
                                    <asp:ListItem Selected="True">Multiple College Applied</asp:ListItem>
                                    <asp:ListItem>Not Applied</asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset id="fldrefund" runat="server" visible="false" style="height: 12px; width: 195px;">
                                <asp:RadioButtonList ID="rbl_EnrollRefund" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="rbl_EnrollRefund_OnSelectedIndexChanged">
                                    <asp:ListItem Selected="True">Enrolled</asp:ListItem>
                                    <asp:ListItem>Not Enrolled</asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset id="fldadm" runat="server" visible="false" style="height: 12px; width: 186px;">
                                <asp:Label ID="lbldrop" runat="server" Text="Include"></asp:Label>
                                <asp:DropDownList ID="ddladmis" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddladmis_Selected">
                                    <asp:ListItem Text="Before Admission" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="After Admission" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </fieldset>
                        </td>
                        <%--Added by saranya on 5April2018--%>
                        <td id="refundStudOrStaff" runat="server" visible="false">
                            <div id="refund" runat="server">
                                <fieldset id="StudOrStaff" style="height: 12px; width: 150px;">
                                    <asp:RadioButtonList ID="rbl_rollnoNewForRefund" runat="server" Height="10px" Width="200px"
                                        RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbl_rollNoForRefund_OnSelectedIndexChanged">
                                        <%--OnSelectedIndexChanged="rbl_rollnoNewForRefund_OnSelectedIndexChanged"--%>
                                        <asp:ListItem Selected="True">Student</asp:ListItem>
                                        <asp:ListItem>Staff</asp:ListItem>
                                    </asp:RadioButtonList>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                </table>
            </center>
            <%--transfer filter details--%>
            <center>
                <div id="divTransfer" runat="server" visible="false">
                    <div style="width: 900px">
                        <div id="div1" style="float: left">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_roll" runat="server" CssClass="textbox txtheight4 textbox1"
                                                OnTextChanged="txt_roll_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_roll"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:TextBox ID="txt_reg" runat="server" Style="display: none" CssClass="textbox txtheight4 textbox1"
                                                onchange="return checkrno(this.value)" onkeyup="return checkrno(this.value)"
                                                onblur="return get(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                            <span style="color: Red;">*</span> <span id="rnomsg"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name
                                        </td>
                                        <td>
                                            <%--commented by saranya on 27/03/2018--%>
                                            <%--<asp:TextBox ID="txt_name" runat="server" CssClass="textbox txtheight6 textbox1"
                                                onblur="getname(this.value)"></asp:TextBox>
                                                 <asp:FilteredTextBoxExtender ID="ftext_name" runat="server" TargetControlID="txt_name"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>--%>
                                            <asp:TextBox ID="txt_name" runat="server" CssClass="textbox txtheight6 textbox1"
                                                OnTextChanged="txt_name_Changed" AutoPostBack="true"></asp:TextBox><%--Added by saranya on 27/03/2018--%>
                                            <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_name" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Date of Transfer
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Updp_date" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_date" runat="server" CssClass="textbox txtheight2 textbox2"
                                                        Width="100px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                        <div style="float: left; margin-left: 180px;">
                            <asp:Image ID="image2" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 110px;
                                width: 100px;" />
                        </div>
                    </div>
                    <%--student details div--%>
                    <div style="width: 900px">
                        <div id="div2" style="float: left;">
                            <fieldset style="height: 296px; width: 332px; border: 1px solid #999999;">
                                <legend>From</legend>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_colg" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lbltempfstclg" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_str1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_strm" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Batch
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_batch" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="txtheight txtcaps">
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
                                            <asp:Label ID="lbltempfstdeg" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Section
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_sec" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Seat Type
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_seattype" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <div style="float: left; margin-left: 144px;">
                            <fieldset id="todivnotAdmit" runat="server" visible="false" style="height: 315px;
                                width: 250px; border: 1px solid #999999;">
                                <legend>To</legend>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclgs" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_colg" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_colg_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_str2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_strm" runat="server" CssClass="textbox ddlheight4" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_strm_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Batch
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldegs" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_degree" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_degree_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldepts" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_dept" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_dept_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsems" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_sem" runat="server" CssClass="textbox ddlheight1" Width="80px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_sem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Section
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_sec" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_sec_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Seat Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_seattype" runat="server" CssClass="textbox ddlheight1"
                                                Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddl_seattype_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Button ID="Button1" runat="server" Text="Generate" OnClick="btn_generate" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Roll No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_roll_no" runat="server" CssClass="txtheight2 txtcaps" MaxLength="25"
                                                OnTextChanged="txt_roll_noNotApp_TextChanged" AutoPostBack="true">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_roll_no"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset id="todivAdmit" runat="server" visible="false" style="height: 315px; width: 250px;
                                border: 1px solid #999999;">
                                <legend>To</legend>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlclgapplied" runat="server" CssClass="textbox ddlheight5"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlclgapplied_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblrollno1" runat="server" Text="ApplicationNo"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_roll1" runat="server" CssClass="textbox txtheight4 textbox1"
                                                OnTextChanged="txt_roll1_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_roll1"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender21" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetAppFormno" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclgss" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_colg1" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lbltempsndclg" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_str3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_strm1" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Batch
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_batch1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lbldegss" runat="server" Text="Degree"></asp:Label>
                                            <asp:TextBox ID="txt_degree1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lblDegCode" runat="server" Visible="false" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- <td>
                                            <asp:Label ID="lbldegss" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_degree1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lblDegCode" runat="server" Visible="false" Text=""></asp:Label>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldeptss" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_dept1" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lbltempsnddeg" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsemss" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_sem1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                            Section
                                            <asp:TextBox ID="txt_sec1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Seat Type
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_seat_type1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                            <asp:Button ID="ButtonGenerate" runat="server" Text="Generate" OnClick="btn_generate" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Roll No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_roll_no1" runat="server" CssClass="txtheight2 txtcaps" MaxLength="25"
                                                OnTextChanged="txt_roll_noApp_TextChanged" AutoPostBack="true">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="fteTxtR1" runat="server" TargetControlID="txt_roll_no1"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </div>
                </div>
                <div>
                    <center>
                        <asp:LinkButton ID="lnkindivmap" runat="server" Style="margin-top: 438px; left: 432px;
                            position: absolute;" Enabled="false" Visible="false" Text="Transfer Mapping"
                            OnClick="lnkindivmap_Click"></asp:LinkButton>
                    </center>
                </div>
            </center>
            <%--refund filter details--%>
            <center>
                <div id="div_refund" runat="server" visible="false">
                    <div id="div_refundStudent" runat="server" visible="false" style="width: 900px;">
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="rbl_rerollno" runat="server" CssClass="textbox  ddlheight"
                                            AutoPostBack="true" OnSelectedIndexChanged="rbl_rerollno_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="Rerollno" runat="server" visible="false">
                                        <asp:TextBox ID="txt_rerollno" runat="server" AutoPostBack="true" CssClass="textbox txtheight4 textbox1"
                                            OnTextChanged="txt_rerollno_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_rerollno"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rerollno"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="TextBox2" runat="server" Style="display: none" CssClass="textbox txtheight4 textbox1"
                                            onchange="return checkrno(this.value)" onkeyup="return checkrno(this.value)"
                                            onblur="return get(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                        <span style="color: Red;">*</span> <span id="Span1"></span>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblDate" runat="server" Text="Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_rdate" runat="server" CssClass="textbox txtheight textbox2"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_rdate" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td rowspan="5">
                                        <asp:Image ID="image3" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 130px;
                                            width: 100px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblReName" runat="server" Text="Name"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <%--commented by saranya on 27/03/2018--%>
                                        <%--<asp:TextBox ID="txt_rename" runat="server" CssClass="textbox txtheight6 textbox1"
                                            onblur="getname(this.value)"></asp:TextBox>
                                       
                                         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_rename"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                        </asp:FilteredTextBoxExtender>--%>
                                        <%--Added by saranya on 27/03/2018--%>
                                        <asp:TextBox ID="txt_rename" runat="server" CssClass="textbox txtheight6 textbox1"
                                            OnTextChanged="txt_rename_Changed" AutoPostBack="true"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rename"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcoll" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_recolg" runat="server" CssClass="txtheight6 txtcaps">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblRebatch" runat="server" Text="Batch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_rebatch" runat="server" CssClass="txtheight txtcaps">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_str4" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_restrm" runat="server" CssClass="txtheight1 txtcaps">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldegre" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_redegree" runat="server" CssClass="txtheight txtcaps">
                                        </asp:TextBox>
                                        &nbsp;
                                        <asp:Label ID="lbldeptms" runat="server" Text="Department"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txt_redept" runat="server" CssClass="txtheight4 txtcaps">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsemests" runat="server" Text="Semester"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_resem" runat="server" CssClass="txtheight txtcaps">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReSection" runat="server" Text="Section"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_resec" runat="server" CssClass="txtheight txtcaps">
                                        </asp:TextBox>
                                    </td>
                                    <%--<td>
                                        <asp:Label ID="lbladvance" runat="server" Visible="false" Text="Advance"></asp:Label>
                                        <asp:DropDownList ID="ddl_AmtPerc" runat="server" Visible="false" CssClass="textbox ddlheight"
                                            BackColor="#81F7D8" AutoPostBack="true" OnSelectedIndexChanged="ddl_AmtPerc_OnSelectedIndexChanged">
                                            <asp:ListItem Selected="True">Amount</asp:ListItem>
                                            <asp:ListItem>Percent</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txt_AmtPerc" runat="server" BackColor="#81F7D8" CssClass="txtheight textbox"
                                            placeholder="0.00" Style="text-align: right" onchange="return refundamt()">
                                        </asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender44" runat="server" TargetControlID="txt_AmtPerc"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:CheckBox ID="chk_refCommon" runat="server" Visible="false" AutoPostBack="true"
                                            OnCheckedChanged="chk_refCommon_OnCheckedChanged" Text="Common" />
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td id="transport" colspan="6" visible="false" runat="server">
                                        <asp:Label ID="routeid" runat="server" Text="Route ID"></asp:Label>
                                        <asp:TextBox ID="routetxt" runat="server" CssClass="txtheight txtcaps">
                                        </asp:TextBox>
                                        <asp:Label ID="Vehicleid" runat="server" Text="Vehicle ID"></asp:Label>
                                        <asp:TextBox ID="vehicletxt" runat="server" CssClass="txtheight txtcaps">
                                        </asp:TextBox>
                                        <asp:Label ID="Stage" runat="server" Text="Stage" Style="width: 121px;"></asp:Label>
                                        <asp:TextBox ID="stagetxt" runat="server" CssClass="txtheight txtcaps">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="hostels" colspan="6" visible="false" runat="server">
                                        <asp:Label ID="hostel" runat="server" Text="Hostel Name"></asp:Label>
                                        <asp:TextBox ID="txt_hostel" runat="server" CssClass="txtheight txtcaps" Style="width: 107px;">
                                        </asp:TextBox>
                                        <asp:Label ID="building" runat="server" Text="Buliding Name"></asp:Label>
                                        <asp:TextBox ID="txt_build" runat="server" CssClass="txtheight txtcaps" Style="width: 107px;">
                                        </asp:TextBox>
                                        <%--<asp:Label ID="Roomtype" runat="server" Text="Room Type"></asp:Label>
                                        <asp:TextBox ID="txt_roomtype" runat="server" CssClass="txtheight txtcaps">
                                        </asp:TextBox>--%>
                                        <asp:Label ID="Roomname" runat="server" Text="Room Name"></asp:Label>
                                        <asp:TextBox ID="txt_roomname" runat="server" CssClass="txtheight txtcaps">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <%--saranya--%>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblRefund_staffid" runat="server" Text="Staff Id"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRefund_staffid" runat="server" placeholder="Staff Id" CssClass="textbox  txtheight2"
                                            AutoPostBack="true" OnTextChanged="txtRefund_staffid_Changed"></asp:TextBox><%--OnTextChanged="txtroll_staff_Changed"--%>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtRefund_staffid"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtRefund_staffid"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblRefund_staffName" runat="server" Text="Staff Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRefund_staffName" runat="server" placeholder="Name" CssClass="textbox txtheight2"
                                            Width="300px" AutoPostBack="true"></asp:TextBox>
                                        <%--OnTextChanged="txtname_staff_Changed"--%>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtRefund_staffName"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblRefund_staffCode" runat="server" Text="Department"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRefund_staffDept" runat="server" placeholder="Department" CssClass="textbox txtheight2"
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <%----------------------------------------------------------%>
                                <tr>
                                    <td colspan="8" align="right">
                                        <asp:Label ID="lbladvance" runat="server" Visible="false" Text="Advance"></asp:Label>
                                        <asp:DropDownList ID="ddl_AmtPerc" runat="server" Visible="false" CssClass="textbox ddlheight"
                                            BackColor="#81F7D8" AutoPostBack="true" OnSelectedIndexChanged="ddl_AmtPerc_OnSelectedIndexChanged">
                                            <asp:ListItem Selected="True">Amount</asp:ListItem>
                                            <asp:ListItem>Percent</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txt_AmtPerc" runat="server" BackColor="#81F7D8" CssClass="txtheight textbox"
                                            placeholder="0.00" Style="text-align: right" onchange="return refundamt()">
                                        </asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender44" runat="server" TargetControlID="txt_AmtPerc"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:CheckBox ID="chk_refCommon" runat="server" Visible="false" AutoPostBack="true"
                                            OnCheckedChanged="chk_refCommon_OnCheckedChanged" Text="Common" />
                                    </td>
                                </tr>
                                <%-- save button add by poomalar--%>
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:Button ID="btnsavePro" runat="server" CssClass="textbox btn btn2" Text="Save"
                                            OnClick="btn_save_Click" Font-Bold="True" BorderStyle="Solid" BackColor="#0CA6CA" />
                                    </td>
                                </tr>
                            </table>
                            <table id="tbljournal" runat="server" visible="false">
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:LinkButton ID="lnkJournal" runat="server" Text="Journal Mapping" OnClick="lnkJournal_Click"></asp:LinkButton>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <%--*******2nd div*****--%>
                    <div style="float: left; width: 900px;">
                        <center>
                            <div id="divref" runat="server" style="border-radius: 10px; border: 0px solid Gray;
                                width: 900px; height: 200px; overflow: auto;">
                                <div style="height: 170px; overflow: auto;">
                                    <asp:GridView ID="gridView3" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                        OnRowDataBound="gridView3_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("Sno") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Year/Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_yearsem" runat="server" Text='<%#Eval("YearSem") %>'></asp:Label>
                                                        <asp:Label ID="lbl_feecat" runat="server" Visible="false" Text='<%#Eval("FeeCategory") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_hdrid" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_lgr" runat="server" Text='<%#Eval("Ledger") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_feeamt" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                                    <asp:Label ID="lbl_finyear" runat="server" Visible="false" Text='<%#Eval("finyear") %>'></asp:Label><%--abarna--%>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_totamt" runat="server" Text='<%#Eval("FeeAmt") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Concess" runat="server" Text='<%#Eval("Concession") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Already Refunded" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_AlRefunded" runat="server" Text='<%#Eval("RefundAmt") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_paid" runat="server" Text='<%#Eval("Paid") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_bal" runat="server" Text='<%#Eval("Balance") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Refund Taken" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_refund" runat="server" placeholder="0.00" onchange="return checkrefundamount()"
                                                        CssClass="  textbox txtheight" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterextender2re2" runat="server" TargetControlID="txt_refund"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Refund" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_refundbal" runat="server" placeholder="0.00" CssClass=" textbox txtheight"
                                                        Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderrebal" runat="server" TargetControlID="txt_refundbal"
                                                        FilterType="Custom,Numbers" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div>
                                    <table id="tblgrid3" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                Total :
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_grid3_tot" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Paid :
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_grid3_paid" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Balance :
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_grid3_bal" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td style="text-align: right;">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkbtn_viewhistory" runat="server" Visible="false" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Blue" CausesValidation="False"
                                                    OnClick="btnHistory_Click">Fees Paid History</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                            <table id="tbltrans" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_hed" runat="server" Visible="false" Text="Header"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_refheader" Visible="false" runat="server" CssClass="textbox ddlheight4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_refheader_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_led" runat="server" Visible="false" Text="Ledger"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_refledger" Visible="false" runat="server" CssClass="textbox ddlheight4">
                                        </asp:DropDownList>
                                    </td>
                                    <%--  discontinue added by abarna 22.02.2018--%>
                                    <td>
                                        <asp:Label ID="DiscontinueReason" runat="server" Text="Discontinue Reason"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="reasondis" runat="server" CssClass="textbox txtheight6 textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="reasondis"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <%----------------------------------------------------%>
                                    <td colspan="3" id="tdRefund" runat="server" visible="false">
                                        Refund Amount
                                        <%-- </td>
                                    <td>--%>
                                        <asp:TextBox ID="txt_reamt" runat="server" CssClass="txtheight txtcaps" placeholder="0.00"
                                            BackColor="#81F7D8" Style="text-align: right">
                                        </asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filterextenderre3" runat="server" TargetControlID="txt_reamt"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                        <%-- </td>
                                    <td>--%>
                                        <asp:Button ID="btn_refund" runat="server" CssClass="textbox btn btn2" Text="Refund"
                                            OnClick="btn_refund_Click" Style="background-color: #0CA6CA; border-style: Solid;
                                            font-weight: bold;" />
                                        <%--OnClick="btn_cancel_Click"--%>
                                        <asp:Button ID="btn_cancel" runat="server" CssClass="textbox btn btn2" Text="Cancel"
                                            OnClick="btn_cancel_Click" Style="background-color: #0CA6CA; border-style: Solid;
                                            font-weight: bold;" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </div>
            </center>
            <%--reuse rollno Confirmation --%>
            <center>
                <div id="divReuseRoll" runat="server" visible="false" style="height: 100%; z-index: 10000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div9" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label7" runat="server" Text="Do You Want To Reuse Roll Number?" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnReuseYes" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btnReuseYes_Click" Text="yes" runat="server" />
                                                <asp:Button ID="btnReuseNo" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btnReuseNo_Click" Text="no" runat="server" />
                                                <asp:Button ID="btcancel" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btcancel_Click" Text="cancel" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <%--transfer mapping div--%>
            <center>
                <div id="divindi" runat="server" visible="false" style="height: 38em; z-index: 1000;
                    width: 100%; background-color: rgba(54,25,25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="divind" runat="server" class="table" style="background-color: White; height: 614px;
                            width: 1000px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 5px;
                            border-radius: 10px;">
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                                width: 30px; position: absolute; margin-top: -30px; margin-left: -46px;" OnClick="ImageButton2_Click" />
                            <div style="float: left; width: 1000px;">
                                <span style="font-family: Book Antiqua; font-weight: bold; color: Green; font-size: large;">
                                    Individual Mapping </span>
                                <br />
                                <br />
                                <%--  <asp:UpdatePanel ID="uppop" runat="server">
                                <ContentTemplate>--%>
                                <div>
                                    <table id="tblSem" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="incPaid" runat="server" Text="Show Paid Details Only" AutoPostBack="true"
                                                    OnCheckedChanged="incPaid_Changed" />
                                            </td>
                                            <td>
                                                &nbsp; &nbsp; &nbsp;
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="incSem" runat="server" Text="" AutoPostBack="true" OnCheckedChanged="incSem_Changed" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label14" runat="server" Text="Semester"></asp:Label>
                                            </td>
                                            <td>
                                                <%--  <asp:UpdatePanel ID="Updp_sem" runat="server">
                                                <ContentTemplate>--%>
                                                <asp:TextBox ID="txtsem" runat="server" Enabled="false" Style="height: 20px; width: 124px;"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                    height: auto;">
                                                    <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsem"
                                                    PopupControlID="panel_sem" Position="Bottom">
                                                </asp:PopupControlExtender>
                                                <%-- </ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label25" runat="server" Text="Other ReceiptCode" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="transCodetext" runat="server" CssClass="txtheight txtcaps" Visible="false"
                                                    AutoPostBack="true" OnTextChanged="txt_recch_Changed">
                                                </asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="getReceiptno" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="transCodetext"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:CheckBox ID="inclAddAmt" runat="server" Text="Add Fees" AutoPostBack="true"
                                                    OnCheckedChanged="inclAddAmt_Changed" />
                                            </td>
                                            <td id="tdaddamt" runat="server" visible="false">
                                                <fieldset style="height: 40px; width: auto;">
                                                    <legend>Additional amount to destination</legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                Header
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlhedind" runat="server" CssClass="textbox ddlheight4" AutoPostBack="true"
                                                                    Width="141px" OnSelectedIndexChanged="ddlhedind_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                Ledger
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlledind" runat="server" CssClass="textbox ddlheight4" Width="141px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                Semester
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlsem" runat="server" CssClass="textbox ddlheight4" Width="106px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                Amount
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtamtind" runat="server" placeholder="0.00" BackColor="#81F7D8"
                                                                    CssClass="txtheight txtcaps" Style="text-align: right;">
                                                                </asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtamtind"
                                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnAddFees" runat="server" Enabled="true" CssClass="textbox btn btn2"
                                                                    Text="Add" OnClick="btnAddFees_Click" />
                                                                <%--OnClientClick="return confirm('do you want journal print if you are not selected then go to select it');"--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="div5" runat="server" style="float: left; overflow: auto; border-radius: 10px;
                                    border: 1px solid Gray; width: 460px; height: 350px; padding-left: 10px;">
                                    <%-- <div style="height: 400px; overflow: auto;">--%>
                                    <asp:GridView ID="gridView4" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                        OnRowDataBound="gridView4_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("Sno") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:CheckBox ID="cbsel" runat="server" onchange="return Paidamount();" />
                                                        <asp:Label ID="lblpaymode" runat="server" Visible="false" Text='<%#Eval("paymode") %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Year/Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_yearsem" runat="server" Text='<%#Eval("YearSem") %>'></asp:Label>
                                                        <asp:Label ID="lbl_feecat" runat="server" Visible="false" Text='<%#Eval("FeeCategory") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_hdrid" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_lgr" runat="server" Text='<%#Eval("Ledger") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_feeamt" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_totamt" runat="server" Text='<%#Eval("FeeAmt") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Concess" runat="server" Text='<%#Eval("Concession") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_paid" runat="server" Text='<%#Eval("Paid") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_bal" runat="server" Text='<%#Eval("Balance") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Narration" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Narration" Text='<%#Eval("Narration") %>' runat="server" placeholder="Narration"
                                                        CssClass="  textbox txtheight" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterextendereex22" runat="server" TargetControlID="txt_Narration"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div id="divtblOne" runat="server" style="float: left; top: 482px; position: absolute;">
                                    <table id="Table1" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                Total :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Paid :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Balance :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="savebutton" runat="server" Enabled="true" CssClass="textbox btn btn2"
                                                    Text="UPDATE" OnClick="btnSaveStud_Click" />
                                                <%--OnClientClick="return confirm('do you want journal print if you are not selected then go to select it');"--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="div4" runat="server" style="float: left; overflow: auto; border-radius: 10px;
                                    border: 1px solid Gray; width: 460px; height: 350px; padding-left: 10px;">
                                    <%-- <div style="height: 400px; overflow: auto;">--%>
                                    <asp:GridView ID="othervendor" runat="server" AutoGenerateColumns="false" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("Sno") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:CheckBox ID="cbsel" runat="server" onchange="return Paidamount();" />
                                                        <asp:Label ID="lblpaymode" runat="server" Visible="false" Text='<%#Eval("paymode") %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vendor Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_vendorname" runat="server" Text='<%#Eval("Vendorname") %>' CssClass="  textbox txtheight"
                                                        Height="15px" Width="80px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterVendor1" runat="server" TargetControlID="txt_vendorname"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="., ">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vendor Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_vendorcode" runat="server" Text='<%#Eval("VendorCode") %>' CssClass="  textbox txtheight"
                                                        Height="15px" Width="80px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterVendor2" runat="server" TargetControlID="txt_vendorcode"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Company Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_companyname" runat="server" Text='<%#Eval("Companyname") %>'
                                                        CssClass="  textbox txtheight" Height="15px" Width="80px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterVendor3" runat="server" TargetControlID="txt_companyname"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mobile Number" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_mobno" runat="server" Text='<%#Eval("mobile") %>' CssClass="  textbox txtheight"
                                                        Height="15px" Width="80px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterVendor4" runat="server" TargetControlID="txt_mobno"
                                                        FilterType="numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_addresss" runat="server" Text='<%#Eval("address1") %>' CssClass="  textbox txtheight"
                                                        Height="15px" Width="80px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterVendor5" runat="server" TargetControlID="txt_addresss"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_hdrid" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_lgr" runat="server" Text='<%#Eval("Ledger") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transcode" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_feeamt" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transdate" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_totamt" runat="server" Text='<%#Eval("FeeAmt") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <%--    <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Concess" runat="server" Text='<%#Eval("Concession") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_paid" runat="server" Text='<%#Eval("Debit") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_bal" runat="server" Text='<%#Eval("Balance") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Narration" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_narration" runat="server" Text='<%#Eval("Narration") %>' CssClass="  textbox txtheight"
                                                        Height="15px" Width="80px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterVendor6" runat="server" TargetControlID="txt_narration"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="., ">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div id="div6" runat="server" style="float: left; top: 482px; position: absolute;">
                                        <table id="Table3" runat="server" visible="false">
                                            <tr>
                                                <td>
                                                    Total :
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label15" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    Paid :
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label16" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    Balance :
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label17" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="vendorothersave" runat="server" Enabled="true" CssClass="textbox btn btn2"
                                                        Text="UPDATE" OnClick="vendorothersave_Click" />
                                                    <%--OnClientClick="return confirm('do you want journal print if you are not selected then go to select it');"--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <%--</div>--%>
                                <%-- <span style="float: left; display: block;">&nbsp;&nbsp;&nbsp;&nbsp;</span>--%>
                                <div runat="server" id="gd5" style="float: right; overflow: auto; border-radius: 10px;
                                    border: 1px solid Gray; width: 450px; height: 350px; padding-right: 30px;">
                                    <%-- <div style="height: 400px; overflow: auto;">--%>
                                    <asp:GridView ID="gridView5" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                        OnRowDataBound="gridView5_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("Sno") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select" Visible="true" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:CheckBox ID="cblsell" runat="server" />
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Year/Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_yearsem" runat="server" Text='<%#Eval("YearSem") %>'></asp:Label>
                                                        <asp:Label ID="lbl_feecat" runat="server" Visible="false" Text='<%#Eval("FeeCategory") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_hdrid" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_lgr" runat="server" Text='<%#Eval("Ledger") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_feeamt" runat="server" Text='<%#Eval("FeeAmt") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_totamt" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Concess" runat="server" Text='<%#Eval("Concession") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_paid" runat="server" placeholder="0.00" Text='<%#Eval("Paid") %>'
                                                        CssClass="  textbox txtheight" Height="15px" Width="60px" onchange="return checkpaidamount()"
                                                        Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterextender21" runat="server" TargetControlID="txt_paid"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="To Be Paid" Visible="true" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfTobePaid" runat="server" Value='<%# Bind("hiddenTempAmt") %>' />
                                                    <asp:TextBox ID="txt_tobePaid" runat="server" placeholder="0.00" Text='<%#Eval("tobePaid") %>'
                                                        CssClass="  textbox txtheight" Height="15px" Width="60px" onchange="return checkTobepaidamount()"
                                                        Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterextenders21" runat="server" TargetControlID="txt_tobePaid"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_bal" runat="server" placeholder="0.00" Text='<%#Eval("Balance") %>'
                                                        onchange="return checkpaidamount()" ReadOnly="true" CssClass="  textbox txtheight"
                                                        Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterextender22" runat="server" TargetControlID="txt_bal"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Excess" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_exGrid2" runat="server" Text="0" placeholder="0.00" onchange="return checkpaidamount()"
                                                        ReadOnly="true" CssClass="  textbox txtheight" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterextendereex22" runat="server" TargetControlID="txt_exGrid2"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div style="float: right; padding-right: 30px;">
                                    <table id="Table4" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                Total :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label18" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Paid :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label19" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Balance :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label20" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label21" runat="server" Text=" Excess/Advance :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label22" runat="server" ForeColor="Red"></asp:Label>
                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" id="td1" runat="server" visible="false">
                                                UnMatchedExcess Amt:
                                            </td>
                                            <td>
                                                <asp:Label ID="Label23" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="staffadd" runat="server" visible="True" style="float: right; overflow: auto;
                                    border-radius: 10px; border: 1px solid Gray; width: 450px; height: 350px; padding-right: 30px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAddRow" runat="server" Text="Add New" OnClick="btnaddgrid_Click" /><%-- OnClick="btnAddRow_Click"--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                    OnRowDataBound="gridView1_OnRowDataBound" OnDataBound="Marksgrid_pg_DataBound"
                                                    OnRowCommand="gridView1_OnRowCommand" Width="100px" Visible="true">
                                                    <%--OnDataBound="gdSetting_OnDataBound"--%>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sno" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:Label ID="lblsno" runat="server" Text='<%#Eval("Sno") %>'></asp:Label>
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Header Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:DropDownList ID="ddl_headername" runat="server" CssClass="textbox ddlheight3"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_headername_SelectedIndexChanged"
                                                                        Width="110px" Visible="true">
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
                                                        <%--<asp:TemplateField HeaderText="Feecategory" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                        HeaderStyle-Width="">
                                                        <ItemTemplate>
                                                            <center>
                                                                <asp:DropDownList ID="ddlFeecat" runat="server" CssClass="textbox1 ddlheight1">
                                                                </asp:DropDownList>
                                                            </center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
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
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="float: right; padding-right: 30px;">
                                    <table id="Table2" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                Total :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Paid :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Balance :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbladvancetxt" runat="server" Text=" Excess/Advance :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" ForeColor="Red"></asp:Label>
                                                <asp:HiddenField ID="hiddnewPaid" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" id="tdunex" runat="server" visible="false">
                                                UnMatchedExcess Amt:
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtnaration" runat="server" Placeholder="Narration" Width="500px"></asp:TextBox>
                                    </td>
                                    <td colspan="4">
                                        <asp:Button ID="btnadjust" runat="server" CssClass="textbox btn btn2" Visible="false"
                                            Text="Mapping" OnClick="btnadjust_Click" Font-Bold="true" />
                                        <asp:Button ID="btnmapreset" runat="server" CssClass="textbox btn btn2" Text="Reset"
                                            OnClick="btnmapreset_Click" Font-Bold="true" />
                                        <asp:CheckBox ID="incJournal" runat="server" Checked="true" Text="Journal Print" />
                                    </td>
                                </tr>
                            </table>
                            <table id="tblJournalSet" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <fieldset style="width: 344px; height: 39px;">
                                            <legend>Journal Fees Settings</legend>
                                            <asp:Label ID="lblhd" runat="server" Text="Header"></asp:Label>
                                            <asp:DropDownList ID="ddlMainJrHed" Enabled="false" runat="server" CssClass="textbox ddlheight2">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblld" runat="server" Text="Ledger"></asp:Label>
                                            <asp:DropDownList ID="ddlMainJrLed" Enabled="false" runat="server" CssClass="textbox ddlheight2">
                                            </asp:DropDownList>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btntransind" runat="server" Enabled="true" CssClass="textbox btn btn2"
                                            Text="Transfer" OnClick="btntransind_Click" />
                                        <%--OnClientClick="return confirm('do you want journal print if you are not selected then go to select it');"--%>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnupdatestaff" runat="server" Enabled="true" CssClass="textbox btn btn2"
                                            Text="Save" OnClick="btnupdatestaff_Click" />
                                        <%--OnClientClick="return confirm('do you want journal print if you are not selected then go to select it');"--%>
                                    </td>
                                </tr>
                            </table>
                            <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                    </center>
                </div>
            </center>
        </div>
        <%--yes or no--%>
        <center>
            <div id="div7" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div10" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 287px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label11" runat="server" Text="Do You Want Transfer This Amount" Style="color: Green;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Label ID="Label12" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Label ID="Label24" runat="server" Style="color: Green;" Font-Bold="true" Font-Size="Medium"
                                                Text="Excess/Advance:"></asp:Label>
                                            <%-- </td>
                                    <td>--%>
                                            <asp:Label ID="Label24ex" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Button ID="buttonok" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="buttonok_Click" Text="OK" runat="server" />
                                            <asp:Button ID="btncancel" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btncancel_Click" Text="Cancel" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="Div3" runat="server" style="height: 710px; width: 1344px;" visible="false">
            </div>
        </div>
        <center>
            <div id="div_rcptSngleStaff" runat="server">
                <table id="rcptSngleStaff" runat="server" visible="false">
                    <tr>
                        <td>
                            <asp:Label ID="StaffID" runat="server" Text="Staff Id"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtroll_staff" runat="server" placeholder="Staff Id" CssClass="textbox  txtheight2"
                                AutoPostBack="true" OnTextChanged="txtroll_staff_Changed"></asp:TextBox><%--OnTextChanged="txtroll_staff_Changed"--%>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtroll_staff"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroll_staff"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="StaffName" runat="server" Text="Staff Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtname_staff" runat="server" placeholder="Name" CssClass="textbox txtheight2"
                                Width="300px" AutoPostBack="true"></asp:TextBox>
                            <%--OnTextChanged="txtname_staff_Changed"--%>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtname_staff"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="StaffDepartment" runat="server" Text="Staff Code"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDept_staff" runat="server" placeholder="Department" CssClass="textbox txtheight2"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table id="tbljournalStaff" runat="server" visible="false">
                    <tr>
                        <td colspan="2">
                            <center>
                                <asp:LinkButton ID="staffjournal" runat="server" Text="Journal Mapping" OnClick="staffjournal_Click"></asp:LinkButton>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <center>
            <div id="otherdiv" runat="server">
                <table id="rcptSngleOthers" runat="server" visible="false">
                    <tr>
                        <td>
                            <asp:Label ID="nameothers" runat="server" Text="Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtroll_other" runat="server" placeholder="Name" CssClass="textbox  txtheight2"
                                OnTextChanged="txtroll_other_Changed" AutoPostBack="true"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetOthername" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroll_other"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Companyname" runat="server" Text="Company Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtname_other" runat="server" placeholder="Company Name" CssClass="textbox txtheight2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Mobile" runat="server" Text="Mobile"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_otherMobile" runat="server" placeholder="Mobile" CssClass="textbox txtheight2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="address1" runat="server" Text="address1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAdd1_Other" runat="server" placeholder="Address 1" CssClass="textbox txtheight2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="address2" runat="server" Text="address1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAdd2_Other" runat="server" placeholder="Address 2" CssClass="textbox txtheight2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="transcode" runat="server" Text="Transcode"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_transcode" runat="server" placeholder="Transcode" CssClass="textbox txtheight2"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table id="lnkothersjournalmaptable" runat="server" visible="false">
                    <tr>
                        <td colspan="2">
                            <center>
                                <asp:LinkButton ID="lnkothersjournalmap" runat="server" Text="Journal Mapping" OnClick="Othersjournal_Click"></asp:LinkButton>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <center>
            <div>
                <table id="rcptSngleVendor" runat="server" visible="false">
                    <tr>
                        <td>
                            <asp:Label ID="vendorname" runat="server" Text="Vendor"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtroll_vendor" runat="server" placeholder="Vendor" CssClass="textbox  txtheight2"
                                OnTextChanged="txtroll_vendor_Changed" AutoPostBack="true"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetVendorno" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroll_vendor"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Contactperson" runat="server" Text="Contact Person"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtname_vendor" runat="server" placeholder="Contact Person" CssClass="textbox txtheight2"
                                OnTextChanged="txtname_vendor_Changed" AutoPostBack="true"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetVendorName" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtname_vendor"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Designation" runat="server" Text="Designation"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDept_vendor" runat="server" placeholder="Designation" CssClass="textbox txtheight2"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table id="lnkvendorjournalmaptable" runat="server" visible="false">
                    <tr>
                        <td colspan="2">
                            <center>
                                <asp:LinkButton ID="lnkvendorjournalmap" runat="server" Text="Journal Mapping" OnClick="Vendor_Click"></asp:LinkButton>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <center>
            <div id="imgAlert" runat="server" visible="false" style="height: 1000px; z-index: 10000;
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
                                            <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
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
            <div id="Div8" runat="server" visible="false" style="height: 100em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div11" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <span class="challanLabel" style="color: Red; font-weight: bold; font-size: Medium;">
                                            <p>
                                                Do You Want To Continue For Payment</p>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="Button2" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                                OnClientClick="return DisplayLoadingDiv();" OnClick="btn_sureyesRcpt_Click" Text="yes"
                                                runat="server" />
                                            <asp:Button ID="Button3" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                                OnClick="btn_surenoRcpt_Click" Text="no" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </body>
</asp:Content>
