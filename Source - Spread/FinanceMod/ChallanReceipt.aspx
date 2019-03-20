<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ChallanReceipt.aspx.cs" Inherits="ChallanReceipt" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Receipt / Challan</title>
    <link rel="Shortcut Icon" href="../college/Left_Logo.jpeg" />
    <link href="../Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .txtboxBg
        {
            background: #FFF url(../images/dropdown.png) no-repeat;
            background-position: 106px 5px !important;
        }
        .challanLabel
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--09-01-2017--%>
    <script type="text/javascript">



        $(document).ready(function () {
            //            $("#MainContent_div_cheque").hide('fast');
            //            $("#MainContent_div_card").hide('fast');

            //            document.getElementById("<%=div_cheque.ClientID %>").style.display = "none";
            //            document.getElementById("<%=div_card.ClientID %>").style.display = "none";
        }
     );
        function rb_ccdc_Onchange(mode) {

            if (mode == "cash") {
                //            $("#MainContent_div_cheque").hide('fast');
                //            $("#MainContent_div_card").hide('fast');
                //            $("#MainContent_btnGridPop").show('fast');

                document.getElementById("<%=div_cheque.ClientID %>").style.display = "none";
                document.getElementById("<%=div_card.ClientID %>").style.display = "none";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "none"; //added by abarna 29.01.2018

                document.getElementById("<%=lbl_challan.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_challan.ClientID %>").style.display = "none";

            }
            else if (mode == "cheque") {
                //            $("#MainContent_div_cheque").show('fast');
                //            $("#MainContent_lbl_chqno").show('fast');
                //            $("#MainContent_txt_chqno").show('fast');

                //            $("#MainContent_div_card").hide('fast');
                //            $("#MainContent_lbl_ddno").hide('fast');
                //            $("#MainContent_txt_ddno").hide('fast');
                //            $("#MainContent_btnGridPop").hide('fast');
                //            $("#MainContent_txt_ddnar").hide('fast');

                document.getElementById("<%=div_cheque.ClientID %>").style.display = "block";
                document.getElementById("<%=lbl_chqno.ClientID %>").style.display = "block";
                document.getElementById("<%=txt_chqno.ClientID %>").style.display = "block";

                document.getElementById("<%=div_card.ClientID %>").style.display = "none";
                document.getElementById("<%=lbl_ddno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_ddno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_ddnar.ClientID %>").style.display = "none";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "none";

                document.getElementById("<%=lbl_challan.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_challan.ClientID %>").style.display = "none";
            }
            else if (mode == "dd") {
                //            $("#MainContent_div_cheque").show('fast');
                //            $("#MainContent_lbl_ddno").show('fast');
                //            $("#MainContent_txt_ddno").show('fast');

                //            $("#MainContent_div_card").hide('fast');
                //            $("#MainContent_lbl_chqno").hide('fast');
                //            $("#MainContent_txt_chqno").hide('fast');
                //            $("#MainContent_btnGridPop").hide('fast');
                //            $("#MainContent_txt_ddnar").hide('fast');


                document.getElementById("<%=div_cheque.ClientID %>").style.display = "block";
                document.getElementById("<%=lbl_ddno.ClientID %>").style.display = "block";
                document.getElementById("<%=txt_ddno.ClientID %>").style.display = "block";
                document.getElementById("<%=txt_ddnar.ClientID %>").style.display = "block";

                document.getElementById("<%=div_card.ClientID %>").style.display = "none";
                document.getElementById("<%=lbl_chqno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_chqno.ClientID %>").style.display = "none";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "none";

                document.getElementById("<%=lbl_challan.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_challan.ClientID %>").style.display = "none";

            }
            else if (mode == "card") {
                //            $("#MainContent_div_cheque").hide('fast');
                //            $("#MainContent_btnGridPop").hide('fast');
                //            $("#MainContent_div_card").show('fast');

                document.getElementById("<%=div_cheque.ClientID %>").style.display = "none";
                document.getElementById("<%=div_card.ClientID %>").style.display = "block";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "none";

                document.getElementById("<%=lbl_challan.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_challan.ClientID %>").style.display = "none";
            }
            //            added by abarna 29.01.2018

            else if (mode == "NEFT") {
                //            $("#MainContent_div_cheque").hide('fast');
                //            $("#MainContent_btnGridPop").hide('fast');
                //            $("#MainContent_div_card").show('fast');

                document.getElementById("<%=div_cheque.ClientID %>").style.display = "none";
                document.getElementById("<%=div_card.ClientID %>").style.display = "none";
                document.getElementById("<%=lbl_ddno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_ddno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_ddnar.ClientID %>").style.display = "none";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "block";

                document.getElementById("<%=lbl_challan.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_challan.ClientID %>").style.display = "none";
            }
            else if (mode == "Challan")//added by abarna 07.05.2018
            {
                document.getElementById("<%=div_cheque.ClientID %>").style.display = "block";

                document.getElementById("<%=lbl_challan.ClientID %>").style.display = "block";
                document.getElementById("<%=txt_challan.ClientID %>").style.display = "block";

                document.getElementById("<%=div_card.ClientID %>").style.display = "none";
                document.getElementById("<%=lbl_ddno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_ddno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_ddnar.ClientID %>").style.display = "none";
                document.getElementById("<%=div_NEFT.ClientID %>").style.display = "none";
                document.getElementById("<%=lbl_chqno.ClientID %>").style.display = "none";
                document.getElementById("<%=txt_chqno.ClientID %>").style.display = "none";
            }
        }

    </script>
    <script type="text/javascript">
        function DisplayLoadingDiv() {
            document.getElementById("<%=divImageLoading.ClientID %>").style.display = "block";
        }
        function HideLoadingDiv() {
            document.getElementById("<%=divImageLoading.ClientID %>").style.display = "none";
        }
    </script>
    <script type="text/javascript">
        window.onload = window.history.forward(0);
        function otherBank(itemid) {
            var txtid = document.getElementById("<%=txt_other.ClientID %>");
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
        function otherCardTypeMd(itemid) {
            var txtid = document.getElementById("<%=txtCardTypeMd.ClientID %>");
            var ddlid = itemid.value;
            if (ddlid.trim().toUpperCase() == "OTHERS") {
                txtid.style.display = "block";
            } else {
                txtid.style.display = "none";
            }
        }

        function QuantityChange() {


            var lblAmt = document.getElementById("<%=lblInvTotAmount.ClientID %>");
            var totAmt = 0;
            var amt = 0.0;

            var tbl = document.getElementById("<%=gridInv.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 0; i < (gridViewControls.length); i++) {


                var txtQty = document.getElementById('MainContent_gridInv_txtQtyInv_' + i.toString());
                var txtRPU = document.getElementById('MainContent_gridInv_lbl_RPUInv_' + i.toString());
                var txtAmt = document.getElementById('MainContent_gridInv_txtamtInv_' + i.toString());


                var QtyVal = 0.0;
                var RPUVal = 0.0;

                if (txtQty.value.trim() != "") {
                    QtyVal = parseFloat(txtQty.value);
                }
                if (txtRPU.innerHTML.trim() != "") {
                    RPUVal = parseFloat(txtRPU.innerHTML);
                }
                txtAmt.value = (QtyVal * RPUVal).toString();

                totAmt += QtyVal * RPUVal;

            }
            lblAmt.innerHTML = "Total Amount Rs." + totAmt + " /-";
        }

        function excessValueEnable() {
        }

        function cb_selectHeadAllChange() {
            var txtname = "";
            var radiolist = document.getElementById('<%=rbl_headerselect.ClientID %>');
            var radio = radiolist.getElementsByTagName("input");
            for (var x = 0; x < radio.length; x++) {
                if (radio[x].checked) {
                    txtname = radio[x].value;
                }
            }
            var txt = document.getElementById('<%=txt_grpheader.ClientID%>');
            var checkid = document.getElementById("<%=cb_selectHeadAll.ClientID %>");
            var cblid = document.getElementById("<%=cbl_grpheader.ClientID %>");
            var options = cblid.getElementsByTagName('input');
            if (checkid.checked == true) {
                for (var i = 0; i < options.length; i++) {
                    options[i].checked = true;
                }
                txt.value = txtname + "(" + options.length + ")";

            } else {
                for (var i = 0; i < options.length; i++) {
                    options[i].checked = false;
                }
                txt.value = txtname + "(" + 0 + ")";
            }
            var recpt = document.getElementById('<%=txt_rcptno.ClientID %>');
            recpt.value = "";
        }
        function cb_selectsingleHeadAllChange() {
            var txtname = "";
            var radiolist = document.getElementById('<%=rbl_headerselect.ClientID %>');
            var radio = radiolist.getElementsByTagName("input");
            for (var x = 0; x < radio.length; x++) {
                if (radio[x].checked) {
                    txtname = radio[x].value;
                }
            }
            var cnt = 0;
            var txt = document.getElementById('<%=txt_grpheader.ClientID%>');
            var cball = document.getElementById('<%=cb_selectHeadAll.ClientID%>');
            var cbsingel = document.getElementById('<%=cbl_grpheader.ClientID%>');
            var cbinput = cbsingel.getElementsByTagName("input");
            if (!cball.checked) {
                for (var i = 0; i < cbinput.length; i++) {
                    if (cbinput[i].checked == true) {
                        cnt++;
                    }
                }
                if (cnt == cbinput.length) {
                    cball.checked = true;
                    txt.value = txtname + "(" + cnt + ")";
                }
                else {
                    txt.value = txtname + "(" + cnt + ")";
                    cball.checked = false;
                }
            } else {
                for (var i = 0; i < cbinput.length; i++) {
                    if (cbinput[i].checked == true) {
                        cnt++;
                    }
                }
                if (cnt == cbinput.length) {
                    cball.checked = true;
                    txt.value = txtname + "(" + cnt + ")";
                }
                else {
                    txt.value = txtname + "(" + cnt + ")";
                    cball.checked = false;
                }
            }
            var recpt = document.getElementById('<%=txt_rcptno.ClientID %>');
            recpt.value = "";
        }
        function checkvalue() {//change by abarna
            var operandleft = [2000, 1000, 500, 200, 100, 50, 20, 10, 5, 2, 1, 0];
            var operandright = 0;
            var totNum = 0;
            var totval = 0;
            var tbl = document.getElementById("<%=grid_Cash.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");
            var len = tbl.rows.length;
            var index = 1;
            var arrayindex = 0;
            for (var i = 0; i < (gridViewControls.length); i = i + 2) {

                if (gridViewControls[i].name.indexOf("txtRs") > 1) {

                    if (gridViewControls[i].value == "") {
                        operandright = 0;
                        totNum = totNum + 0;
                        if (i == 22) {//change by abarna
                            // gridViewControls[index].value = totNum;
                        } else {
                            gridViewControls[i].value = "0";
                        }
                        totval = totval + 0;
                    } else {
                        totNum = totNum + parseFloat(gridViewControls[i].value);
                        operandright = parseFloat(operandleft[arrayindex]) * parseFloat(gridViewControls[i].value);
                        totval = totval + operandright;
                    }
                }
                if (gridViewControls[index].name.indexOf("txtSum") > 1) {
                    gridViewControls[index].value = operandright;
                    if (i == 22) {
                        gridViewControls[index].value = totval;

                        var txtreturn = document.getElementById("<%=txt_return.ClientID %>");
                        var txttotaltobepaid = document.getElementById("<%=Txt_amt.ClientID %>");
                        if (txttotaltobepaid != null) {
                            txtreturn.value = (totval - parseInt(txttotaltobepaid.value)).toString();
                        } else {

                            txtreturn.value = "";
                        }
                    }
                }
                index = index + 2;
                arrayindex++;
            }
        }
        //return Amount Denomination Written by abarna
        function checkvaluereturn() {//change by abarna
            var operandleft = [2000, 1000, 500, 200, 100, 50, 20, 10, 5, 2, 1, 0];
            var operandright = 0;
            var totNum = 0;
            var totval = 0;
            var tbl = document.getElementById("<%=grid_CashReturn.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");
            var len = tbl.rows.length;
            var index = 1;
            var arrayindex = 0;
            for (var i = 0; i < (gridViewControls.length); i = i + 2) {

                if (gridViewControls[i].name.indexOf("txtRs") > 1) {

                    if (gridViewControls[i].value == "") {
                        operandright = 0;
                        totNum = totNum + 0;
                        if (i == 22) {//change by abarna
                            // gridViewControls[index].value = totNum;
                        } else {
                            gridViewControls[i].value = "0";
                        }
                        totval = totval + 0;
                    } else {
                        totNum = totNum + parseFloat(gridViewControls[i].value);
                        operandright = parseFloat(operandleft[arrayindex]) * parseFloat(gridViewControls[i].value);
                        totval = totval + operandright;
                    }
                }
                if (gridViewControls[index].name.indexOf("txtSum") > 1) {
                    gridViewControls[index].value = operandright;
                    if (i == 22) {
                        gridViewControls[index].value = totval;

                        var txtreturn = document.getElementById("<%=txtreturnamtval.ClientID %>");

                        var txttotaltobepaid = document.getElementById("<%=hid.ClientID %>").value;
                        // var txttotaltobepaid = document.getElementById("<%=txt_return.ClientID %>");
                        //  alert(txttotaltobepaid);
                        if (txttotaltobepaid != null) {
                            // alert(totval);
                            txtreturn.value = (totval - txttotaltobepaid).toString();
                            //  alert(txtreturn.value);
                        } else {

                            txtreturn.value = "";
                        }
                    }
                }
                index = index + 2;
                arrayindex++;
            }
        }
        //------------------------------
        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=grid_Details.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 0; i < (tbl.rows.length - 1); i++) {
                var chkSelectid = document.getElementById('MainContent_grid_Details_cb_selectLedger_' + i.toString());
                if (chkSelAll.checked == false && chkSelectid.disabled == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }
            checkpaidamount1();
        }

        function checkpaidamount1() {
            var curToPayAmt = document.getElementById("<%=lbl_CurPay.ClientID %>");
            var curToPayAmtVal = 0;

            var partAmt = document.getElementById("<%=Txt_amt.ClientID %>");
            partAmt.value = "";


            var tottobepaidval = 0.00;
            var totbalamt = 0.00;
            var txttotaltobepaid = document.getElementById("<%=Txt_amt.ClientID %>");
            var txtbalanceamount = document.getElementById("<%=txt_balamt.ClientID %>");

            var tbl = document.getElementById("<%=grid_Details.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 0; i < (gridViewControls.length); i++) {
                var chkSelectid = document.getElementById('MainContent_grid_Details_cb_selectLedger_' + i.toString());

                var txt = document.getElementById('MainContent_grid_Details_txt_tobepaid_amt_' + i.toString());
                var txtbal = document.getElementById('MainContent_grid_Details_txt_bal_amt_' + i.toString());
                var txttotal = document.getElementById('MainContent_grid_Details_txt_tot_amt_' + i.toString());
                var txtpaid = document.getElementById('MainContent_grid_Details_txt_paid_amt_' + i.toString());
                var txtexgridamt = document.getElementById('MainContent_grid_Details_txt_gridexcess_amt_' + i.toString());
                var txtscholamt = document.getElementById('MainContent_grid_Details_txt_scholar_amt_' + i.toString());
                var txtcautamt = document.getElementById('MainContent_grid_Details_txt_deposit_amt_' + i.toString());

                var curExcess = 0.00;
                var schlAmt = 0.00;
                var curCautAmt = 0.00;


                if (txt != null && txtbal != null && txttotal != null && txtpaid != null) {
                    //checkFloatValue(txt);
                    if (chkSelectid.checked == false) {
                        txt.value = "";
                    }

                    else {
                        if (txt.value == "" || txt.value == "0" || txt.value == "0.00") {
                            txt.value = (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt)).toString();
                        }
                    }
                    if ((parseFloat(txt.value) <= (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt)))) {

                        tottobepaidval = tottobepaidval + parseFloat(txt.value);
                        totbalamt += (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt)) - parseFloat(txt.value);
                        txttotaltobepaid.value = tottobepaidval.toString();
                        txtbalanceamount.value = totbalamt.toString();
                        txtbal.value = (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt)) - parseFloat(txt.value);
                    } else {

                        totbalamt += (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt));

                        //txttotaltobepaid.value = tottobepaidval.toString();//Commented by saranya on 24march2018
                        txttotaltobepaid.value = tottobepaidval.toString() == "0" ? "" : tottobepaidval.toString();
                        txtbalanceamount.value = totbalamt.toString();
                        txtbal.value = (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt));
                        txt.value = "0.00";
                    }
                    if (chkSelectid.checked) {
                        curToPayAmtVal += parseFloat(txt.value) + curExcess + schlAmt + curCautAmt;
                    }
                }
            }

            curToPayAmt.innerHTML = "Total Amount To Be Paid : Rs." + curToPayAmtVal.toString() + " /-";
            checkvalue();
        }

        function checkpaidamount() {
            var curToPayAmt = document.getElementById("<%=lbl_CurPay.ClientID %>");
            var curToPayAmtVal = 0;

            var partAmt = document.getElementById("<%=Txt_amt.ClientID %>");
            partAmt.value = "";


            var tottobepaidval = 0.00;
            var totbalamt = 0.00;
            var txttotaltobepaid = document.getElementById("<%=Txt_amt.ClientID %>");
            var txtbalanceamount = document.getElementById("<%=txt_balamt.ClientID %>");

            var tbl = document.getElementById("<%=grid_Details.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 0; i < (gridViewControls.length); i++) {
                var chkSelectid = document.getElementById('MainContent_grid_Details_cb_selectLedger_' + i.toString());

                var txt = document.getElementById('MainContent_grid_Details_txt_tobepaid_amt_' + i.toString());
                var txtbal = document.getElementById('MainContent_grid_Details_txt_bal_amt_' + i.toString());
                var txttotal = document.getElementById('MainContent_grid_Details_txt_tot_amt_' + i.toString());
                var txtpaid = document.getElementById('MainContent_grid_Details_txt_paid_amt_' + i.toString());
                var txtexgridamt = document.getElementById('MainContent_grid_Details_txt_gridexcess_amt_' + i.toString());
                var txtscholamt = document.getElementById('MainContent_grid_Details_txt_scholar_amt_' + i.toString());
                var txtcautamt = document.getElementById('MainContent_grid_Details_txt_deposit_amt_' + i.toString());


                var curExcess = 0.00;
                var schlAmt = 0.00;
                var curCautAmt = 0.00;



                if (txt != null && txtbal != null && txttotal != null && txtpaid != null) {
                    //checkFloatValues(txt);
                    //                    if (chkSelectid.checked == false) {
                    //                        txt.value = "0.00";
                    //                    } else {
                    //                    if (txt.value == "" && txt.value != "0") {
                    //                        txt.value = (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt)).toString();
                    //                    }
                    //                    }
                    if ((parseFloat(txt.value) <= (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt)))) {

                        tottobepaidval = tottobepaidval + parseFloat(txt.value);
                        totbalamt += (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt)) - parseFloat(txt.value);
                        txttotaltobepaid.value = tottobepaidval.toString();
                        txtbalanceamount.value = totbalamt.toString();
                        txtbal.value = (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt)) - parseFloat(txt.value);
                    } else {

                        totbalamt += (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt));
                        txttotaltobepaid.value = tottobepaidval.toString();
                        txtbalanceamount.value = totbalamt.toString();
                        txtbal.value = (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt));
                        txt.value = "0.00";
                    }
                    if (parseFloat(txt.value) > 0) {
                        chkSelectid.checked = true;
                    } else {
                        chkSelectid.checked = false;
                    }
                    if (chkSelectid.checked) {
                        curToPayAmtVal += parseFloat(txt.value) + curExcess + schlAmt + curCautAmt;
                    }
                }
            }

            curToPayAmt.innerHTML = "Total Amount To Be Paid : Rs." + curToPayAmtVal.toString() + " /-";
            checkvalue();
        }
        function getTotalExcess() {
            var tbl = document.getElementById("<%=gridExcess.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");
            var total = 0;
            var lastrow = gridViewControls.length - 1;
            for (var i = 0; i < (gridViewControls.length); i++) {

                var txt = document.getElementById('gridExcess_txt_ExAmtpop_' + i.toString());
                if (txt != null && lastrow != i) {
                    if (parseFloat(txt.value) != "NaN" && parseFloat(txt.value) != "")
                        total = total + parseFloat(txt.value)
                }
                else {
                    txt.value = total.toString();
                }
            }
        }

        function checkFloatValue(el) {
            var ex = /^[0-9]+\.?[0-9]*$/;
            if (ex.test(el.value) == false) {
                el.value = "";
            }
        }
        function checkIntValue(el) {
            var ex = /^[0-9]*$/;
            if (ex.test(el.value) == false) {
                el.value = "0";
            }
        }
        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            //            var printWindow = window.open('', '', 'height=816,width=980');//Aruna 06/07/2018
            var printWindow = window.open('', '', 'height:auto,width=980');
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
    </script>
    <%--Scheme Admission--%>
    <script type="text/javascript">
        function getapplNo(txtapplSchemeno) {
            $.ajax({
                type: "POST",
                url: "ChallanReceipt.aspx/applicationNo",
                data: '{applno: "' + txtapplSchemeno + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: Success,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function Success(response) {
            var mesg1 = document.getElementById('<%=lblerrScheme.ClientID %>');
            switch (response.d) {
                case "0":
                    mesg1.style.color = "green";
                    mesg1.innerHTML = "Not Exist";
                    break;
                case "1":
                    mesg1.style.color = "Red";
                    document.getElementById('<%=txtadmnoScheme.ClientID %>').value = "";
                    mesg1.innerHTML = "Exist";
                    break;
                case "2":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Enter Admission No";
                    break;
                case "error":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Error Occurred";
                    break;
            }
        }

        function getTempReceiptNo(txttempNo) {
            $.ajax({
                type: "POST",
                url: "ChallanReceipt.aspx/CheckReceiptNo",
                data: '{tempRcptNo:"' + txttempNo + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: Success,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function Success(response) {
            var errmsg = document.getElementById('<%=lblrcptalert.ClientID %>');
            switch (response.d) {
                case "0":
                    errmsg.style.color = "green";
                    errmsg.innerHTML = "Not Exist!";
                    break;
                case "1":
                    errmsg.style.color = "red";
                    errmsg.innerHTML = "Already Exist!";
                    document.getElementById('<%=txttemprcpt.ClientID %>').value = "";
                    break;

            }
        }
    </script>
    <%--Scheme Admission Ends--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green;">Receipt / Challan </span>
        </div>
    </center>
    <%--  Design Started By Idhris 12/10/2015 --%>
    <center>
        <div class="maindivstyle" style="width: 980px; overflow: auto;">
            <input type="hidden" runat="server" id="hid" />
            <center>
                <%--Row0 --%>
                <div style="padding-left: 10px; padding-top: 5px; clear: both;">
                    <div>
                        <%--<table  style="border-radius: 10px; background-color: White; height: 25px; float: left;
                            border-style: solid; border-width: 1px;">--%>
                        <table class="maintablestyle" style="float: left; height: 25px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight2"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexchange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbl_rollnoNew" runat="server" Height="10px" Width="300px"
                                        RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbl_rollnoNew_OnSelectedIndexChanged">
                                        <asp:ListItem Selected="True">Student</asp:ListItem>
                                        <asp:ListItem>Staff</asp:ListItem>
                                        <asp:ListItem>Vendor</asp:ListItem>
                                        <asp:ListItem>Others</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td id="studentSelect" runat="server" visible="false">
                                    <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                        OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <%-- <table style="background-color: White; height: 25px; float: left; margin-left: 20px;
                            border-radius: 10px; border-style: solid; border-width: 1px;">--%>
                        <table class="maintablestyle" style="height: 25px; float: left; margin-left: 20px;">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdo_receipt" runat="server" RepeatDirection="Horizontal" GroupName="same"
                                        OnCheckedChanged="rdo_receipt_CheckedChanged" AutoPostBack="true" Text="Receipt"
                                        Checked="true" Width="80px"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdo_challan" runat="server" RepeatDirection="Horizontal" GroupName="same"
                                        OnCheckedChanged="rdo_challan_CheckedChanged" AutoPostBack="true" Text="Challan"
                                        Width="80px"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_date" runat="server" CssClass="textbox txtheight" Height="15px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_rcptno" runat="server" CssClass="textbox txtheight" Height="15px"
                                        Style="text-align: right;" Enabled="false" BackColor="#81F7D8"></asp:TextBox>
                                    <asp:Label ID="lblstaticrollno" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblaccid" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lstrcpt" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lbltype" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div id="div_rcpt" runat="server" visible="true">
                                        <asp:CheckBox ID="chk_rcptMulmode" runat="server" Text="Multiple Paymode" Checked="false" />
                                        <asp:CheckBox ID="chl_MulRcpt" runat="server" Text="Multiple Receipt" Checked="false"
                                            AutoPostBack="true" OnCheckedChanged="chl_MulRcpt_OnCheckedChanged" />
                                    </div>
                                    <div id="div_challan" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rdo_sngle" runat="server" RepeatDirection="Horizontal" GroupName="s1"
                                                        OnCheckedChanged="rdo_sngle_CheckedChanged" AutoPostBack="true" Text="Single"
                                                        Checked="true"></asp:RadioButton>
                                                    <asp:RadioButton ID="rdo_multi" runat="server" RepeatDirection="Horizontal" GroupName="s1"
                                                        OnCheckedChanged="rdo_multi_CheckedChanged" AutoPostBack="true" Text="Multiple">
                                                    </asp:RadioButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </div>
                <%--Row1 --%>
                <div style="width: 950px;">
                    <div id="div_formultich" runat="server" style="float: left;">
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <table id="rcptsngle" runat="server">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txt_Smartno" runat="server" placeholder="Smartcard No" CssClass="textbox  txtheight2"
                                                        Visible="false" OnTextChanged="txt_Smartno_Changed" TextMode="Password" AutoPostBack="true"></asp:TextBox>
                                                    <asp:TextBox ID="txt_rollno" runat="server" placeholder="Roll No" CssClass="textbox  txtheight2"
                                                        OnTextChanged="txt_rollno_Changed" AutoPostBack="true"></asp:TextBox>
                                                    <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderroll" runat="server" TargetControlID="txt_rollno"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">--%>
                                                    <%--</asp:FilteredTextBoxExtender>--%>
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
                                                    <asp:Button ID="btnClear" runat="server" CssClass="textbox btn1 textbox1" Text="Clear"
                                                        OnClick="btnClear_Click" Style="color: Red; font-weight: bold;" />
                                                    <asp:LinkButton ID="btndetails" runat="server" Visible="false" CssClass="textbox btn1 textbox1"
                                                        Text="Details" Style="color: Red; font-weight: bold;" OnClick="btndetails_Click" />
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
                                                                <asp:DropDownList ID="ddl_sem" runat="server" CssClass="textbox  ddlheight2" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddl_sem_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
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
                                                                <%--abarna--%>
                                                                <asp:TextBox ID="cursem" runat="server" placeholder="Current Semester" CssClass="textbox txtheight2"
                                                                    ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="popup" runat="server" visible="false" style="height: 355em; z-index: 1000;
                                        width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                                        left: 0;">
                                        <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                                            width: 30px; position: absolute; margin-top: 65px; margin-left: 880px;" OnClick="imagebtnpopclose1_Click" />
                                        <center>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <div class="popsty" style="background-color: White; height: auto; width: 450px; border: 5px solid #0CA6CA;
                                                border-top: 5px solid #0CA6CA; border-radius: 10px; margin-top: -8px">
                                                <fieldset style="height: auto;">
                                                    <legend class="fontstyleheader" style="color: Green;">Student Details</legend>
                                                    <br />
                                                    <br />
                                                    <center>
                                                        <table border="1" cellpadding="5" cellspacing="5">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblName" runat="server" Text="Student Name"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblNameval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblDob" runat="server" Text="Student DOB"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDobval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblfname" runat="server" Text="Father Name"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblfnameval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblfatheroccu" runat="server" Text="Father Occupation"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblfatheroccuval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblmname" runat="server" Text="Mother Name"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblmnameval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblmotheroccu" runat="server" Text="Mother Occupation"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblmotheroccuval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblAddressval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblReligion" runat="server" Text="Religion"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblReligionval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblcommunity" runat="server" Text="Community"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblcommunityval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFphno" runat="server" Text="Father MobileNo"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFphnoval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblMphno" runat="server" Text="Mother MobileNo"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMphnoval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblStudPhno" runat="server" Text="Student MobileNo"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblStudPhnoval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblbatchyear" runat="server" Text="Batch Year"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblbatchyearval" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </center>
                                                </fieldset>
                                        </center>
                                    </div>
            </center>
        </div>
        <div>
            <table id="rcptSngleStaff" runat="server" visible="false">
                <tr>
                    <td>
                        <asp:TextBox ID="txtroll_staff" runat="server" placeholder="Staff Id" CssClass="textbox  txtheight2"
                            OnTextChanged="txtroll_staff_Changed" AutoPostBack="true"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtroll_staff"
                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                        </asp:FilteredTextBoxExtender>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroll_staff"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                        <asp:Button ID="btn_staffLook" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                            OnClick="btn_staffLook_Click" />
                        <asp:Button ID="Button4" runat="server" CssClass="textbox btn1 textbox1" Text="Clear"
                            OnClick="btnClear_Click" Style="color: Red; font-weight: bold;" />
                        <br>
                        <asp:TextBox ID="txtname_staff" runat="server" placeholder="Name" CssClass="textbox txtheight2"
                            Width="300px" OnTextChanged="txtname_staff_Changed" AutoPostBack="true"></asp:TextBox>
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
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtDept_staff" runat="server" placeholder="Department" CssClass="textbox txtheight2"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSem_staff" runat="server" CssClass="textbox  ddlheight2"
                                        AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlSem_staff_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:UpdatePanel ID="UpdatePanel8" Visible="false" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtSem_staff" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                placeholder="Semester/Year" onfocus="return myFunction1(this)"></asp:TextBox>
                                            <asp:Panel ID="panel5" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cbSem_staff" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbSem_staff_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblSem_staff" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSem_staff_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtSem_staff"
                                                PopupControlID="panel5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btnGO_staff" Text="Go" runat="server" CssClass="textbox btn1 textbox1"
                                        OnClick="btnGO_staff_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table id="rcptSngleVendor" runat="server" visible="false">
                <tr>
                    <td>
                        <asp:TextBox ID="txtroll_vendor" runat="server" placeholder="Vendor" CssClass="textbox  txtheight2"
                            OnTextChanged="txtroll_vendor_Changed" AutoPostBack="true"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetVendorno" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroll_vendor"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                        <asp:TextBox ID="txtname_vendor" runat="server" placeholder="Contact Person" CssClass="textbox txtheight2"
                            OnTextChanged="txtname_vendor_Changed" AutoPostBack="true"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetVendorName" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtname_vendor"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                        <asp:Button ID="btn_vendorLook" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                            OnClick="btn_vendorLook_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtDept_vendor" runat="server" placeholder="Designation" CssClass="textbox txtheight2"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSem_vendor" runat="server" CssClass="textbox  ddlheight2"
                                        AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlSem_vendor_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:UpdatePanel ID="UpdatePanel10" Visible="false" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtSem_vendor" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                placeholder="Semester/Year" onfocus="return myFunction1(this)"></asp:TextBox>
                                            <asp:Panel ID="panel6" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cbSem_vendor" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbSem_vendor_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblSem_vendor" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSem_vendor_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtSem_vendor"
                                                PopupControlID="panel6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btnGO_vendor" Text="Go" runat="server" CssClass="textbox btn1 textbox1"
                                        OnClick="btnGO_vendor_Click" />
                                    <asp:Button ID="Button6" runat="server" CssClass="textbox btn1 textbox1" Text="Clear"
                                        OnClick="btnClear_Click" Style="color: Red; font-weight: bold;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table id="rcptSngleOthers" runat="server" visible="false">
                <tr>
                    <td>
                        <asp:TextBox ID="txtroll_other" runat="server" placeholder="Name" CssClass="textbox  txtheight2"
                            OnTextChanged="txtroll_other_Changed" AutoPostBack="true"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetOthername" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroll_other"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="panelbackground">
                        </asp:AutoCompleteExtender>
                        <asp:TextBox ID="txtname_other" runat="server" placeholder="Company Name" CssClass="textbox txtheight2"></asp:TextBox>
                        <asp:TextBox ID="txt_otherMobile" runat="server" placeholder="Mobile" CssClass="textbox txtheight2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtAdd1_Other" runat="server" placeholder="Address 1" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdd2_Other" runat="server" placeholder="Address 2" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSem_other" runat="server" CssClass="textbox  ddlheight2"
                                        AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlSem_other_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:UpdatePanel ID="UpdatePanel11" Visible="false" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtSem_other" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                placeholder="Semester/Year" onfocus="return myFunction1(this)"></asp:TextBox>
                                            <asp:Panel ID="panel7" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cbSem_other" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbSem_other_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblSem_other" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSem_other_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtSem_other"
                                                PopupControlID="panel7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btnGO_other" Text="Go" runat="server" CssClass="textbox btn1 textbox1"
                                        OnClick="btnGO_other_Click" />
                                    <asp:Button ID="Button5" runat="server" CssClass="textbox btn1 textbox1" Text="Clear"
                                        OnClick="btnClear_Click" Style="color: Red; font-weight: bold;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divMulReceipt" runat="server" visible="false" style="float: left; width: 400px;">
            <table>
                <tr>
                    <td>
                        <span class="challanLabel">
                            <p>
                                Total No.of Students</p>
                        </span>
                        <asp:TextBox ID="txt_tostudentsrcpt" runat="server" CssClass="textbox txtheight"
                            MaxLength="8" Style="text-align: right;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_tostudentsrcpt"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnrcptRoll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                            OnClick="btn_roll_Click" />
                        <asp:Button ID="Button2" Visible="false" Text="Search" runat="server" CssClass="textbox btn2"
                            OnClick="btn_search_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="challanLabel">
                            <p>
                                Semester/Year</p>
                        </span>
                        <asp:DropDownList ID="ddl_semrcpt" runat="server" CssClass="textbox  ddlheight1"
                            AutoPostBack="true" OnSelectedIndexChanged="ddl_semrcpt_IndexChange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="challanLabel">
                            <p>
                                Amount</p>
                        </span>
                        <asp:TextBox ID="txtMultTotal" runat="server" CssClass=" textbox txtheight" Width="70px"
                            BackColor="#E49B2C" placeholder="0.00" Text="0.00" />
                    </td>
                </tr>
            </table>
        </div>
        </td> </tr>
        <tr>
            <td id="checCashDD" runat="server" visible="false">
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
                <%-- added by abarna 29.01.2018--%>
                <asp:RadioButton ID="rb_NEFT" runat="server" RepeatDirection="Horizontal" GroupName="same3"
                    Text="NEFT" onchange="rb_ccdc_Onchange('NEFT')"></asp:RadioButton>
                <asp:RadioButton ID="rb_Challan" runat="server" RepeatDirection="Horizontal" GroupName="same3"
                    Text="Challan" onchange="rb_ccdc_Onchange('Challan')" OnCheckedChanged="rb_Challan_CheckedChanged"
                    AutoPostBack="true"></asp:RadioButton><%--added by abarna 7.5.2018--%>
                <%--OnCheckedChanged="rb_card_CheckedChanged" AutoPostBack="true"--%>
                <%--  &nbsp;&nbsp;--%>
                <asp:Label ID="lbltxtamt" runat="server" Text="Pay"></asp:Label>
                <asp:TextBox ID="Txt_amt" runat="server" placeholder="0.00" onblur="checkFloatValue(this);"
                    CssClass="textbox txtheight" Style="text-align: right; width: 45px; height: 15px;"
                    BackColor="#E49B2C" MaxLength="15">
                </asp:TextBox>
                <asp:DropDownList ID="ddl_AmtPerc" runat="server" CssClass="textbox ddlheight1" BackColor="#81F7D8"
                    Style="width: 55px;">
                    <asp:ListItem Selected="True">Amt</asp:ListItem>
                    <asp:ListItem>Per</asp:ListItem>
                </asp:DropDownList>
                <%--AutoPostBack="true" OnSelectedIndexChanged="ddl_AmtPerc_OnSelectedIndexChanged"--%>
            </td>
        </tr>
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
                <asp:Label ID="lbl_examt" runat="server" Text="Excess"></asp:Label>
                <asp:TextBox ID="txt_examt" runat="server" placeholder="0.00" onblur="checkFloatValue(this);"
                    CssClass="textbox txtheight" Style="text-align: right; width: 45px; height: 15px;"
                    BackColor="#81F7D8"></asp:TextBox>
                <asp:Button ID="btnExc" runat="server" OnClick="btnExc_Click" Text="?" OnClientClick="return CheckExcessReqmts();"
                    CssClass="textbox btn2 textbox1 " Width="30px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblchalan" runat="server" Text="Challan Taken"></asp:Label>
                <asp:TextBox ID="txt_chltaken" runat="server" placeholder="0.00" onblur="checkFloatValue(this);"
                    CssClass="textbox txtheight" Style="text-align: right; width: 45px; height: 15px;"
                    BackColor="#81F7D8"></asp:TextBox>
                <asp:Label ID="lblDemand" runat="server" Visible="false" Text="Demand"></asp:Label>
                <asp:TextBox ID="txttotbalAmt" runat="server" ReadOnly="true" Visible="false" placeholder="Demand"
                    onblur="checkFloatValue(this);" CssClass="textbox txtheight" Style="text-align: right;
                    width: 58px; height: 15px;" BackColor="#81F7D8"></asp:TextBox>
                <%--  <fieldset id="fldfnlyr" runat="server" style="height: 20px; width: 150px;" visible="false">
                    <table>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtfyear" Style="height: 20px; width: 180px;" CssClass="Dropdown_Txt_Box"
                                            runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                        <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px">
                                            <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                                AutoPostBack="True">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txtfyear"
                                            PopupControlID="Pfyear" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </fieldset>--%>
                <asp:CheckBox ID="cb_exfees" runat="server" Text="Use Excess" AutoPostBack="true"
                    Visible="false" OnCheckedChanged="cb_exfees_OnCheckedChanged" />
                <asp:Label ID="lb_exfeesamount" runat="server" Text="Excess"></asp:Label>
                <asp:TextBox ID="txt_ExcAmt" runat="server" CssClass=" textbox txtheight" Width="45px"
                    placeholder="0.00" Text="0.00" BackColor="#81F7D8" />
                <asp:CheckBox ID="cb_CautionDep" runat="server" Text="Use Deposit" AutoPostBack="true"
                    OnCheckedChanged="cb_CautionDep_OnCheckedChanged" Visible="false" />
                <asp:TextBox ID="txt_CautionAmt" runat="server" CssClass=" textbox txtheight" Width="45px"
                    BackColor="#81F7D8" placeholder="0.00" Text="0.00" Visible="false" />
            </td>
        </tr>
        </table> </div>
        <fieldset id="fldfnlyr" runat="server" style="height: 20px; width: 150px;" visible="false">
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblfyear" Visible="true" Text="FinanceYear" Width="85px"></asp:Label>
                    </td>
                    <td id="tdfyear" runat="server" visible="true">
                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtfyear" Style="height: 20px; width: 180px;" CssClass="Dropdown_Txt_Box"
                                    runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px">
                                    <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                        AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txtfyear"
                                    PopupControlID="Pfyear" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div id="div_multich" runat="server" visible="false" style="float: left; width: 400px;">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Total No.of Students"></asp:Label>
                        <asp:TextBox ID="txt_totnoofstudents" runat="server" CssClass="textbox txtheight"
                            MaxLength="8" Style="text-align: right;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="filterextendertot" runat="server" TargetControlID="txt_totnoofstudents"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btn_select" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                            OnClick="btn_roll_Click" />
                        <asp:Button ID="btnSearch3" Visible="false" Text="Search" runat="server" CssClass="textbox btn2"
                            OnClick="btn_search_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="challanLabel">
                            <p>
                                Semester/Year</p>
                        </span>
                        <asp:DropDownList ID="ddl_semMultiple" runat="server" CssClass="textbox  ddlheight2">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <%--Row1 Right div --%>
        <div id="div_HeadSelect" runat="server" style="float: left; padding-bottom: 2px;
            width: 430px;">
            <table>
                <tr>
                    <td style="width: 400px; height: 20px;">
                        <div style="float: left;">
                            <asp:CheckBox ID="cb_selcthd" runat="server" Text="Select Header" OnCheckedChanged="cb_selcthd_CheckedChanged"
                                AutoPostBack="true" Height="20px" />
                        </div>
                        <div id="div_fs" runat="server" style="float: left;" visible="false">
                            <asp:RadioButtonList ID="rbl_headerselect" runat="server" RepeatDirection="Horizontal"
                                Style="border-radius: 10px;" Height="20px" AutoPostBack="true" OnSelectedIndexChanged="rbl_headerselect_OnSelectedIndexChanged">
                                <asp:ListItem Selected="True">Group Header</asp:ListItem>
                                <asp:ListItem>Header</asp:ListItem>
                                <asp:ListItem>Ledger</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
            </table>
            <div>
                <div>
                    <fieldset id="div_grphdr" visible="false" runat="server" style="height: 85px; width: 350px;
                        float: left;">
                        <table style="float: left;">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_grpheader" runat="server" CssClass="textbox txtheight4" ReadOnly="true"
                                        Width="330px"></asp:TextBox>
                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Width="340px" Height="200px"
                                        Style="text-align: left;">
                                        <asp:CheckBox ID="cb_selectHeadAll" runat="server" Text="Select All" OnCheckedChanged="cb_selectHeadAll_OnCheckedChanged"
                                            AutoPostBack="True" Height="15px" onchange="return cb_selectHeadAllChange()" />
                                        <asp:CheckBoxList ID="cbl_grpheader" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_grpheader_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_grpheader"
                                        PopupControlID="panel_dept" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_grpHdrSrch" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                        Width="330px" Placeholder="Search" OnTextChanged="txt_grpHdrSrch_TextChanged"
                                        AutoPostBack="true" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_grpHdrSrch"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetGrpHeader" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_grpHdrSrch"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkOpenSchemeSettings" runat="server" Text="Scheme" OnClick="lnkOpenSchemeSettings_OnClick"
                                        Visible="false" Style="font-family: 'Book Antiqua'; font-weight: bold; font-size: 16px;
                                        color: Green; background-color: white; padding: 2px;"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div style="float: left;">
                    <asp:Image ID="img_stud" runat="server" Style="height: 100px; width: 80px;" Visible="false" /></div>
            </div>
        </div>
        </div>
        <%-- Row3 --%>
        <div style="padding: 10px 0px 10px 0px;">
            <table id="clgbank" runat="server" style="padding-left: 10px; width: 980px;">
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lbl_cbname" runat="server" Text="CollegeBank"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddl_collegebank" runat="server" CssClass="textbox ddlheight4">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnGridPop" runat="server" OnClick="btnGridPop_Click" Text="Denomination"
                            CssClass="textbox btn2 textbox1" Width="90px" Style="float: left;" />
                        <asp:Button ID="btnAddFee" runat="server" OnClick="btnAddFee_Click" Text="AddFee"
                            CssClass="textbox btn2 textbox1" Width="50px" Style="float: left;" />
                        <asp:Button ID="btn_History" runat="server" OnClick="btnHistory_Click" Text="History"
                            CssClass="textbox btn2 textbox1" Width="50px" Style="float: left;" />
                    </td>
                    <%--<td colspan="2">
                                <asp:Label ID="lbl_remark" runat="server" Text="Remarks"></asp:Label>
                                <asp:TextBox ID="txt_remark" runat="server" CssClass="textbox txtheight5" Width="190px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderrem" runat="server" TargetControlID="txt_remark"
                                    FilterType="UppercaseLetters, LowercaseLetters,Numbers, Custom" ValidChars=" /.()-,">
                                </asp:FilteredTextBoxExtender>
                               
                            </td>--%>
                    <td>
                    </td>
                    <td>
                        <asp:TextBox ID="txttemprcpt" runat="server" CssClass="textbox txtheight5" Visible="false"
                            placeholder="Receipt No" Width="110px" onblur="return getTempReceiptNo(this.value)"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txttemprcpt"
                            FilterType="UppercaseLetters, LowercaseLetters,Numbers, Custom" ValidChars=" . /">
                        </asp:FilteredTextBoxExtender>
                        <asp:Label ID="lblrcptalert" runat="server"></asp:Label>
                        <%--  <asp:TextBox ID="txt_exfees" Visible="false" runat="server" CssClass="textbox txtheight"
                                    Style="text-align: right; width: 45px; height: 15px;" BackColor="#81F7D8"></asp:TextBox>--%>
                        <%--  <asp:TextBox ID="txt_exfees" runat="server" CssClass="textbox txtheight" Style="text-align: right;
                                    display: none;"></asp:TextBox>--%>
                        <%--  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderexfee" runat="server" TargetControlID="txt_exfees"
                                    FilterType="Numbers,CUstom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>--%>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_govt" runat="server" Text="Scholarship" AutoPostBack="true"
                                                Width="100px" OnCheckedChanged="cb_govt_SelectedIndexChanged" />
                                            <asp:TextBox ID="txt_GvtTot" runat="server" CssClass=" textbox txtheight" Width="60px"
                                                BackColor="#81F7D8" placeholder="0.00" Text="0.00" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSchlReason" Visible="false" runat="server" CssClass="textbox txtheight2"
                                                Width="60px" ReadOnly="true" placeholder="Type" onfocus="return myFunction1(this)"></asp:TextBox>
                                            <asp:Panel ID="pnlMulReason" Visible="false" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cbSchlReason" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cbSchlReason_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblSchlReason" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSchlReason_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtSchlReason"
                                                PopupControlID="pnlMulReason" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lbl_remark" runat="server" Text="Remarks"></asp:Label>
                        <asp:TextBox ID="txt_remark" runat="server" CssClass="textbox txtheight5" Width="550px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderrem" runat="server" TargetControlID="txt_remark"
                            FilterType="UppercaseLetters, LowercaseLetters,Numbers, Custom" ValidChars=" /.()-,">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
            </table>
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
                    <td>
                        <asp:TextBox ID="txt_other" runat="server" CssClass="textbox txtheight2" onfocus="return myFunction(this)"
                            Placeholder="Other Bank" Style="display: none;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom"
                            ValidChars=" " TargetControlID="txt_other">
                        </asp:FilteredTextBoxExtender>
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
                        <asp:Label ID="lbl_challan" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_challan" runat="server" AutoPostBack="true" Placeholder="Challan No"
                            OnTextChanged="txt_challan_TextChanged" CssClass="textbox txtheight2"></asp:TextBox>
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
                        <asp:Label ID="lbl_chltakn" runat="server" Text="Challan Taken"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_chltakn" runat="server" CssClass="textbox txtheight" Style="text-align: right;
                            width: 45px; height: 15px;" BackColor="#81F7D8"></asp:TextBox>
                    </td>
                </tr>
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
                <tr id="div_NEFT">
                    <td>
                        <asp:Label ID="lbl_NEFTNO" runat="server" Text="NEFT NO"></asp:Label>
                        <asp:TextBox ID="txt_NEFTNO" runat="server" Placeholder="NEFT NO" CssClass="textbox txtheight2"></asp:TextBox>
                    </td>
                    <%--added by abarna--%>
                </tr>
            </table>
        </div>
        <%--Row2 --%>
        <div style="width: 980px; height: 350px; overflow: auto; background-color: white;
            border-radius: 10px;">
            <table style="padding-left: 50px; float: left; clear: both; width: 950px; height: 35px;">
                <tr>
                    <td>
                        <span style="padding-right: 100px;">
                            <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" onchange="return SelLedgers();" />
                        </span><span style="padding-left: 50px;">
                            <asp:Label ID="lbl_CurPay" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="" ForeColor="Red"></asp:Label></span>
                    </td>
                    <td style="text-align: right;">
                        <asp:Button ID="btn_save" BackColor="#8199FD" Text="Save" runat="server" CssClass="textbox btn2 textbox1"
                            OnClick="btn_save_Click" Font-Names=" Book Antiqua" Font-Size="Medium" />
                        <asp:Button ID="btn_print" BackColor="#8199FD" Text="Print" runat="server" CssClass="textbox btn2 textbox1"
                            OnClick="btn_print_Click" Font-Names=" Book Antiqua" Font-Size="Medium" />
                        <asp:Button ID="BtnExit" Text="Exit" BackColor="#8199FD" runat="server" CssClass="textbox btn1 textbox1"
                            Visible="false" OnClick="BtnExit_Click" Font-Names=" Book Antiqua" Font-Size="Medium" />
                    </td>
                </tr>
            </table>
            <div id="div_grid" runat="server" style="width: 980px; height: 310px; overflow: auto;
                background-color: white; border-radius: 10px;">
                <asp:GridView ID="grid_Details" runat="server" AutoGenerateColumns="false" GridLines="Both"
                    OnRowDataBound="grid_Details_OnRowDataBound" OnDataBound="grid_Details_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                <asp:Label ID="lbl_Finyearfk" runat="server" Text='<%#Eval("finyearfk") %>' Visible="false"
                                    Width="80px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cb_selectLedger" runat="server" onchange="return checkpaidamount1();">
                                </asp:CheckBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="cb_selectgrid" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                            Visible="false" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lbl_roll_no" runat="server" Text='<%#Eval("Roll_No") %>' Width="80px"></asp:Label>
                                </center>
                                <center>
                                    <asp:Label ID="lbl_reg_no" runat="server" Text='<%#Eval("Reg_No") %>' Visible="false"></asp:Label>
                                </center>
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Student Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                            Visible="false" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Label ID="lbl_stud_name" runat="server" Text='<%#Eval("Stud_Name") %>' Width="150px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Degree" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                            Visible="false" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Label ID="lbl_degree" runat="server" Text='<%#Eval("Degree") %>' Width="150px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
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
                                <asp:Label ID="lbl_chltkn" runat="server" Text='<%#Eval("ChlTaken") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lbl_monwiseMon" runat="server" Text='<%#Eval("MonwiseMon") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lbl_monwiseYea" runat="server" Text='<%#Eval("MonwiseYear") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lbl_FeeallotPk" runat="server" Text='<%#Eval("FeeallotPk") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:TextBox ID="txt_fee_amt" runat="server" CssClass="  textbox txtheight" Width="80px"
                                        Text='<%#Eval("Fee_Amount") %>' Height="15px" ReadOnly="true" Style="text-align: right;"></asp:TextBox></center>
                                <asp:FilteredTextBoxExtender ID="filterextender1" runat="server" TargetControlID="txt_fee_amt"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Deduction" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:TextBox ID="txt_deduct_amt" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("Deduct") %>'
                                        Height="15px" Width="60px" ReadOnly="true" Style="text-align: right;"></asp:TextBox></center>
                                <asp:FilteredTextBoxExtender ID="filterextender2" runat="server" TargetControlID="txt_deduct_amt"
                                    FilterType="Numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:TextBox ID="txt_tot_amt" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("Total") %>'
                                        Height="15px" Width="60px" ReadOnly="true" Style="text-align: right;"></asp:TextBox></center>
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
                                        Style="text-align: right;" Text='<%#Eval("ToBePaid") %>' onfocus="if(document.getElementById('rdo_challan').checked == false)this.value='';return checkpaidamount();"
                                        onchange="return checkpaidamount();" onblur="if(this.value=='')this.value='';return checkpaidamount();"
                                        Height="15px" Width="70px"></asp:TextBox></center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excess" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:TextBox ID="txt_gridexcess_amt" runat="server" onblur="checkFloatValue(this);"
                                        CssClass="  textbox txtheight" Style="text-align: right;" Text='<%#Eval("Monthly") %>'
                                        ReadOnly="true" Height="15px" Width="60px"></asp:TextBox></center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scholarship" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:TextBox ID="txt_scholar_amt" runat="server" onblur="checkFloatValue(this);"
                                        CssClass="  textbox txtheight" Style="text-align: right;" Text='<%#Eval("Scholar") %>'
                                        ReadOnly="true" Height="15px" Width="60px"></asp:TextBox></center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Caution Deposit" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:TextBox ID="txt_deposit_amt" runat="server" onblur="checkFloatValue(this);"
                                        CssClass="  textbox txtheight" Style="text-align: right;" Text='<%#Eval("CautionDep") %>'
                                        ReadOnly="true" Height="15px" Width="60px"></asp:TextBox></center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Financial Year" Visible="true" HeaderStyle-BackColor="#0CA6CA"
                            HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lblfinfk" runat="server" Text='<%#Eval("finyearfk") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblfinyear" runat="server" Text='<%#Eval("finyear") %>' Width="75px"></asp:Label>
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:TemplateField HeaderText="MonthlyAmount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                Visible="false">
                                <ItemTemplate>
                                    <center>
                                        <asp:TextBox ID="txt_monthly_amt" runat="server" CssClass="  textbox txtheight" Style="text-align: right;"
                                            Text='<%#Eval("Monthly") %>' ReadOnly="true" Height="15px" Width="60px"></asp:TextBox></center>
                                    <asp:FilteredTextBoxExtender ID="filterextendermonthly" runat="server" TargetControlID="txt_monthly_amt"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </center>
    </div> </center>
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
                            <%--<asp:UpdatePanel ID="Updp_strm" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_strm" runat="server" CssClass="textbox txtheight" ReadOnly="true"
                                        onfocus="return myFunction1(this)"></asp:TextBox>
                                    <asp:Panel ID="panel_strm" runat="server" CssClass="multxtpanel multxtpanleheight"
                                        Width="150px">
                                        <asp:CheckBox ID="cb_strm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_strm_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_strm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_strm_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="Popupce_strm" runat="server" TargetControlID="txt_strm"
                                        PopupControlID="panel_strm" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
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
                            <asp:CheckBox ID="cbFirstGrad" runat="server" BackColor="#EE9090" Text="First Graduate" />
                            <asp:CheckBox ID="cbFpaid" runat="server" BackColor="#90EE90" Text="Fully Paid" /><asp:CheckBox
                                ID="cbPpaid" runat="server" BackColor="#FFB6C1" Text="Partially Paid" />
                            <asp:CheckBox ID="cbNpaid" runat="server" BackColor="White" Checked="false" Text="Not Paid" />
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
    </center>
    <center>
        <div id="div_cash" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 75px; margin-left: 130px;"
                OnClick="imagebtnpopGridclosecross_Click" />
            <br />
            <br />
            <br />
            <br />
            <div style="background-color: White; height: 584px; width: 300px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Denomination</span></div>
                </center>
                <br />
                <center>
                    <div id="divcash" runat="server" style="width: 300px; height: 250px; float: left;">
                        <asp:GridView ID="grid_Cash" runat="server" AutoGenerateColumns="false" GridLines="Both"
                            Width="200px" Height="250" OnDataBound="gridCash_OnDataBound" OnRowDataBound="gridCash_OnRowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Rs" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_rs" runat="server" Width="60px" Text='<%#Eval("Rupees") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nos" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:TextBox ID="txtRs" runat="server" CssClass="  textbox txtheight1" MaxLength="8"
                                                Style="text-align: right;" onblur="return checkvalue()"></asp:TextBox></center>
                                        <asp:FilteredTextBoxExtender ID="filterextenderNo" runat="server" TargetControlID="txtRs"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:TextBox ID="txtSum" runat="server" CssClass="  textbox txtheight1" Style="text-align: right;"></asp:TextBox></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="padding-top: 5px; text-align: center;">
                            <span class="challanLabel">
                                <p>
                                    Return Amount</p>
                            </span>
                            <asp:TextBox ID="txt_return" runat="server" CssClass="textbox txtheight2" Style="text-align: right;"></asp:TextBox>
                            <asp:Button ID="btnDenOk" runat="server" OnClick="imagebtnpopGridclose_Click" Text="OK"
                                CssClass="textbox btn1 textbox1" />
                        </div>
                    </div>
                </center>
                <center>
                    <div id="returnCash" runat="server" visible="false" style="width: 300px; height: 250px;
                        float: left;">
                        <asp:GridView ID="grid_CashReturn" runat="server" AutoGenerateColumns="false" GridLines="Both"
                            Width="200px" Height="250" OnDataBound="grid_CashReturn_OnDataBound" OnRowDataBound="grid_CashReturn_OnRowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Rs" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_rs" runat="server" Width="60px" Text='<%#Eval("Rupees") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nos" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:TextBox ID="txtRs" runat="server" CssClass="  textbox txtheight1" MaxLength="8"
                                                Style="text-align: right;" onblur="return checkvaluereturn()"></asp:TextBox></center>
                                        <asp:FilteredTextBoxExtender ID="filterextenderNo" runat="server" TargetControlID="txtRs"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:TextBox ID="txtSum" runat="server" CssClass="  textbox txtheight1" Style="text-align: right;"></asp:TextBox></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="padding-top: 5px; text-align: center;">
                            <span class="challanLabel">
                                <p>
                                    Return Amount</p>
                            </span>
                            <asp:TextBox ID="txtreturnamtval" runat="server" CssClass="textbox txtheight2" Style="text-align: right;"></asp:TextBox>
                            <asp:Button ID="btndenominationRetok" runat="server" OnClick="imagebtnpopGridclosereturn_Click"
                                Text="OK" CssClass="textbox btn1 textbox1" />
                        </div>
                    </div>
                </center>
            </div>
        </div>
    </center>
    <%-- Pop Add Header Ledger New --%>
    <center>
        <div id="div_HeaderLed" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 75px; margin-left: 450px;"
                OnClick="imagebtnpopLedgerclose_Click" />
            <br />
            <br />
            <br />
            <br />
            <div style="background-color: White; height: 500px; width: 950px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Additional Fee</span></div>
                </center>
                <table>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblCommInv" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rblCommInv_Indexchanged">
                                <asp:ListItem Selected="True">Common</asp:ListItem>
                                <asp:ListItem>Inventory</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlCommonLedger" runat="server">
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_HeaderPop" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">Header</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_HeaderPop" runat="server" OnCheckedChanged="cb_HeaderPop_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_HeaderPop" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_HeaderPop_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_HeaderPop"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_Ledgerpop" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight">Ledger</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_ledgerpop" runat="server" OnCheckedChanged="cb_ledgerpop_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_ledgerpop" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_ledgerpop_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_Ledgerpop"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbsem" runat="server" Visible="false"></asp:Label>
                                <asp:UpdatePanel ID="UpdatePanel9" Visible="false" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_Addfeesem" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            placeholder="Semester/Year" onfocus="return myFunction1(this)"></asp:TextBox>
                                        <asp:Panel ID="panel_semaddfee" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_semAddFee" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_semAddFee_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_semAddFee" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_semAddFee_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_Addfeesem"
                                            PopupControlID="panel_semaddfee" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <span class="challanLabel">
                                    <p>
                                        Search By</p>
                                </span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_ledgeSearch" runat="server" CssClass="textbox ddlheight1"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_ledgeSearch_Change" Width="73px">
                                    <asp:ListItem>Header</asp:ListItem>
                                    <asp:ListItem>Ledger</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td id="tdsemadd" runat="server" visible="false">
                                <asp:UpdatePanel ID="UpdatePanels6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtsemadd" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            placeholder="Semester/Year" onfocus="myFunction(this)" Width="75px"></asp:TextBox>
                                        <asp:Panel ID="panel8" runat="server" CssClass="multxtpanel" Height="150px" Width="150px">
                                            <asp:CheckBox ID="cbsemadd" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbsemadd_CheckedChanged" />
                                            <%--AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged"--%>
                                            <asp:CheckBoxList ID="cblsemadd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblsemadd_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                            <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged"--%>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txtsemadd"
                                            PopupControlID="panel8" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="ledgeUp" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_ledgeSearch" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                            Height="20px" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_ledgeSearch"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" &%-+/().,*@">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetLegerName" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ledgeSearch"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <%--    <td runat="server" id="addfeefin" visible="false">
                                <%--  <asp:Label runat="server" ID="lblfyears" Visible="true" Text="FinanceYear" Width="85px"></asp:Label>--%>
                            <%--   <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtfyears" Style="height: 20px; width: 180px;" runat="server" ReadOnly="true"
                                        CssClass="textbox txtheight2" Width="145px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Pfyears" runat="server" CssClass="multxtpanel" Width="178px">
                                        <asp:CheckBox ID="chkfyears" runat="server" Text="Select All" OnCheckedChanged="chkfyears_changed"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklsfyears" runat="server" OnSelectedIndexChanged="chklsfyears_selected"
                                            AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender15" runat="server" TargetControlID="txtfyears"
                                        PopupControlID="Pfyears" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                            <%--</td>--%>
                            <%--    ------------------------------%>
                            <%--   //--------------------add fee financial year added by abarna 10.02.2018----------%>
                            <td runat="server" id="addfeefin" visible="false">
                                <asp:Label runat="server" ID="Label3" Text="FinanceYear" Width="85px" Visible="false"></asp:Label>
                            </td>
                            <td runat="server" id="addfeefindropdown" visible="false">
                                <asp:DropDownList ID="ddlfinyear" runat="server" CssClass="textbox textbox1 ddlheight2"
                                    Style="width: 130px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="cb_addExcessFee" runat="server" Text="Excess" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStudRcpt" runat="server" CssClass="textbox ddlheight1" Style="display: none;
                                    width: 130px;">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btn_ledgesearch" Text="Go" OnClick="btn_ledgesearch_Click" Font-Size="Medium"
                                    Font-Names="Book Antiqua" CssClass="textbox btn1 textbox1" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <center>
                        <br />
                        <div id="div2" runat="server" style="width: 670px; height: 270px; overflow: auto;">
                            <asp:GridView ID="grid_HeaderLedger" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                Width="650px" OnDataBound="grid_HeaderLedger_OnDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_serial1" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_headeridpop" runat="server" Visible="false" Text='<%#Eval("HeaderPK") %>'></asp:Label>
                                            <asp:Label ID="lbl_headpop" runat="server" Text='<%#Eval("HeaderName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ledgeridpop" runat="server" Visible="false" Text='<%#Eval("LedgerPK") %>'></asp:Label>
                                            <asp:Label ID="lbl_legerpop" runat="server" Text='<%#Eval("LedgerName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txt_NewLedger" runat="server" onblur="checkFloatValue(this);" CssClass="  textbox txtheight"
                                                    Style="text-align: right;"></asp:TextBox></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="padding-top: 5px; text-align: center;">
                            <asp:Button ID="btn_ledgersave" runat="server" OnClick="btnpopLedgersave_Click" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Save" CssClass="textbox btn2 textbox1" />
                            <asp:Button ID="btn_ledgerExit" runat="server" OnClick="imagebtnpopLedgerclose_Click"
                                Font-Names="Book Antiqua" Text="Exit" CssClass="textbox btn2 textbox1" Font-Size="Medium" />
                        </div>
                    </center>
                </asp:Panel>
                <asp:Panel ID="pnlInventLedger" runat="server" Visible="false">
                    <table class="maintablestyle">
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbldeptstore" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rbldeptstore_Indexchanged">
                                    <asp:ListItem Selected="True">Department</asp:ListItem>
                                    <asp:ListItem>Store</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="challanLabel">
                                    <p>
                                        Ledger</p>
                                </span>
                                <asp:DropDownList ID="ddlLedgerInv" runat="server" CssClass="textbox ddlheight2"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlLedgerInv_Indexchange">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <span class="challanLabel" runat="server" id="Department">
                                    <p>
                                        Department</p>
                                </span><span class="challanLabel1" id="store" runat="server" visible="false">
                                    <p>
                                        Store</p>
                                </span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updStoreInv" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlStoreInv" runat="server" CssClass="textbox ddlheight2" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlStoreInv_Indexchange">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="upStore" runat="server" Visible="false">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlstroe" runat="server" CssClass="textbox ddlheight2" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlstroe_Indexchange">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%--<asp:UpdatePanel ID="updDeptInv" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtDeptInv" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            placeholder="Department" onfocus="return myFunction1(this)"></asp:TextBox>
                                        <asp:Panel ID="pnlDeptInv" runat="server" CssClass="multxtpanel" Height="200px">
                                            <asp:CheckBox ID="cbDeptInv" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbDeptInv_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblDeptInv" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblDeptInv_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceDeptInv" runat="server" TargetControlID="txtDeptInv"
                                            PopupControlID="pnlDeptInv" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upItemHcInv" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtItemHcInv" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight"
                                            Width="100px">Item Header</asp:TextBox>
                                        <asp:Panel ID="pnlItemHcInv" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cbItemHcInv" runat="server" OnCheckedChanged="cbItemHcInv_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblItemHcInv" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblItemHcInv_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceItemHcInv" runat="server" TargetControlID="txtItemHcInv"
                                            PopupControlID="pnlItemHcInv" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upItemSHcInv" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtItemSHcInv" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight"
                                            Width="120px">Item Sub Header</asp:TextBox>
                                        <asp:Panel ID="pnlItemSHcInv" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cbItemSHcInv" runat="server" OnCheckedChanged="cbItemSHcInv_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblItemSHcInv" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblItemSHcInv_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceItemSHcInv" runat="server" TargetControlID="txtItemSHcInv"
                                            PopupControlID="pnlItemSHcInv" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updItemCdInv" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtItemCdInv" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight"
                                            Width="80px">Item Code</asp:TextBox>
                                        <asp:Panel ID="pnlItemCdInv" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cbItemCdInv" runat="server" OnCheckedChanged="cbItemCdInv_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblItemCdInv" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblItemCdInv_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceItemCdInv" runat="server" TargetControlID="txtItemCdInv"
                                            PopupControlID="pnlItemCdInv" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upItemNmInv" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtItemNmInv" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight"
                                            Width="80px">Item Name</asp:TextBox>
                                        <asp:Panel ID="pnlItemNmInv" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cbItemNmInv" runat="server" OnCheckedChanged="cbItemNmInv_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblItemNmInv" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblItemNmInv_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceItemNmInv" runat="server" TargetControlID="txtItemNmInv"
                                            PopupControlID="pnlItemNmInv" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btnItemInvSearch" Text="Add" OnClick="btnItemInvSearch_Click" Font-Size="Medium"
                                    Font-Names="Book Antiqua" CssClass="textbox btn1 textbox1" Width="60px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <span class="challanLabel">
                                    <p>
                                        Search By</p>
                                </span>
                                <asp:DropDownList ID="ddlInvSearchHdr" runat="server" CssClass="textbox ddlheight1"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlInvSearchHdr_Indexchange">
                                    <asp:ListItem Selected="True">Item Code</asp:ListItem>
                                    <asp:ListItem>Item Name</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtInvSrchCode" runat="server" CssClass="textbox txtheight2" MaxLength="20"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtenderSrcCode" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetItemCode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtInvSrchCode"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtInvSrchCode"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:TextBox ID="txtInvSrchName" runat="server" CssClass="textbox txtheight2" MaxLength="45"
                                    Visible="false"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtenderSrcName" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetItemName" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtInvSrchName"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtInvSrchName"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <br />
                        <div id="divGridInv" runat="server" style="width: 940px; height: 260px; overflow: auto;">
                            <asp:GridView ID="gridInv" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                OnDataBound="gridInv_DataBound" Width="915px">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_serial1" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSel" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_itemledgeidInv" runat="server" Visible="false" Text='<%#Eval("ledgeId") %>'></asp:Label>
                                            <asp:Label ID="lbl_itemledgeInv" runat="server" Text='<%#Eval("ledge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_itemdeptidInv" runat="server" Visible="false" Text='<%#Eval("DeptId") %>'></asp:Label>
                                            <asp:Label ID="lbl_itemdeptInv" runat="server" Text='<%#Eval("Dept") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_itemhcidInv" runat="server" Visible="false" Text='<%#Eval("ItemhcId") %>'></asp:Label>
                                            <asp:Label ID="lbl_itemhcInv" runat="server" Text='<%#Eval("Itemhc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_itemnameinv" runat="server" Text='<%#Eval("ItemName") %>'></asp:Label>
                                            <asp:Label ID="lbl_itemShcInv" runat="server" Visible="false" Text='<%#Eval("ItemSHc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_itemnoidInv" runat="server" Visible="false" Text='<%#Eval("ItemPk") %>'></asp:Label>
                                            <asp:Label ID="lbl_itemnoInv" runat="server" Text='<%#Eval("ItemCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Available" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_AvlInv" runat="server" Text='<%#Eval("Available") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Measure" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_Msrnv" runat="server" Text='<%#Eval("Measure") %>'></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate (PerUnit)" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_RPUInv" runat="server" Text='<%#Eval("IssuedRPU") %>'></asp:Label>
                                            <%--<asp:TextBox ID="txtRPUInv" runat="server" CssClass="textbox txtheight" ReadOnly="true"
                                                    Text='<%#Eval("IssuedRPU") %>' Style="text-align: right;" Width="60px"></asp:TextBox>--%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txtQtyInv" runat="server" CssClass="textbox txtheight" Style="text-align: right;"
                                                    Width="45px" onchange="return QuantityChange()" placeholder="0.00"></asp:TextBox></center>
                                            <asp:FilteredTextBoxExtender ID="filterextender2re2" runat="server" TargetControlID="txtQtyInv"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                            <%--  OnTextChanged="Quantity_OnTextChanged"   Text='<%#Eval("Quanti") %>' Text='<%#Eval("Amount") %>'--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txtamtInv" runat="server" CssClass="textbox txtheight" Style="text-align: right;"
                                                    placeholder="0.00"></asp:TextBox></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <span style="width: 100%; text-align: right; padding-right: 25px; padding-left: 700px;">
                            <asp:Label ID="lblInvTotAmount" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:Label></span>
                        <div style="padding-top: 5px; text-align: center;">
                            <asp:Button ID="btnInvSave" runat="server" OnClick="btnInvSave_Click" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Save" CssClass="textbox btn2 textbox1" />
                            <asp:Button ID="btnInvDelete" runat="server" OnClick="btnInvDelete_Click" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Delete" CssClass="textbox btn2 textbox1" />
                            <asp:Button ID="btnInvExit" runat="server" OnClick="imagebtnpopLedgerclose_Click"
                                Font-Names="Book Antiqua" Text="Exit" CssClass="textbox btn2 textbox1" Font-Size="Medium" />
                        </div>
                    </center>
                </asp:Panel>
            </div>
        </div>
    </center>
    <%-- Popup  History--%>
    <center>
        <div id="div_History" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 75px; margin-left: 440px;"
                OnClick="imagebtnpopHistclose_Click" />
            <br />
            <br />
            <br />
            <br />
            <div style="background-color: White; height: 500px; width: 920px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Fee Status Report</span></div>
                </center>
                <br />
                <table class="maintablestyle" style="width: 300px; text-align: left;">
                    <tr>
                        <td>
                            <span class="challanLabel">
                                <p>
                                    Header</p>
                            </span>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtheadr3" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight1">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cbheadr3" runat="server" OnCheckedChanged="cbheadr3_ChekedChange"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cblheadr3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheadr3_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtheadr3"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <span class="challanLabel">
                                <p>
                                    Ledger</p>
                            </span>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtlgr3" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                        Width="120px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel4" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cblgr3" runat="server" OnCheckedChanged="cblgr3_ChekedChange" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbllgr3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_lgr3_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtlgr3"
                                        PopupControlID="Panel4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btnhisgo" Text="Go" OnClick="btnhisgo_Click" Font-Size="Medium" Font-Names="Book Antiqua"
                                CssClass="textbox btn1 textbox1" runat="server" />
                        </td>
                    </tr>
                </table>
                <center>
                    <br />
                    <div id="div3" runat="server" style="width: 900px; height: 320px; overflow: auto;">
                        <asp:GridView ID="gridHist" runat="server" AutoGenerateColumns="true" GridLines="Both"
                            Width="830px" OnDataBound="grid_HeaderLedger_OnDataBound" HeaderStyle-BackColor="#0CA6CA"
                            HeaderStyle-HorizontalAlign="Center">
                        </asp:GridView>
                    </div>
                    <div style="padding-top: 5px; text-align: center;">
                        <%-- <asp:Button ID="Button2" runat="server" OnClick="btnpopLedgersave_Click"  Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Text="Save"
                                CssClass="textbox btn2" />--%>
                    </div>
                </center>
            </div>
        </div>
    </center>
    <%-- Popup  Excess--%>
    <center>
        <div id="divExcess" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="imgExcess" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 75px; margin-left: 380px;"
                OnClick="imgExcessclose_Click" />
            <br />
            <br />
            <br />
            <br />
            <div style="background-color: White; height: 550px; width: 800px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Excess Fees</span></div>
                </center>
                <br />
                <div style="text-align: left; padding-left: 50px;">
                    <asp:Label ID="lbl_Exfee" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </div>
                <center>
                    <table>
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
                                            <%-- <td>
                                                Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtamtind" runat="server" placeholder="0.00" BackColor="#81F7D8"
                                                    CssClass="txtheight txtcaps" Style="text-align: right;">
                                                </asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtamtind"
                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>--%>
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
                    <br />
                    <asp:CheckBox ID="cbAdvance" runat="server" Text="Include Advance" />
                    <%-- added finyear for excess adjust--%>
                    <asp:DropDownList ID="finexcess" runat="server" CssClass="textbox textbox1 ddlheight2"
                        Style="width: 130px;">
                    </asp:DropDownList>
                    <br />
                    <div id="div5" runat="server" style="width: 800px; height: 320px; overflow: auto;">
                        <asp:GridView ID="gridExcess" runat="server" AutoGenerateColumns="false" GridLines="Both"
                            Width="780px" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                            OnDataBound="gridExcess_OnDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_serialEx" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_excesspop" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Category" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl_categoryidEx" runat="server" Visible="false" Text='<%#Eval("Category") %>'></asp:Label>
                                            <asp:Label ID="lbl_categoryEx" runat="server" Text='<%#Eval("SemYear") %>'></asp:Label></center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_headeridEx" runat="server" Visible="false" Text='<%#Eval("HeaderCode") %>'></asp:Label>
                                        <asp:Label ID="lbl_headpEx" runat="server" Text='<%#Eval("Header") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ledgeridEx" runat="server" Visible="false" Text='<%#Eval("LedgerCode") %>'></asp:Label>
                                        <asp:Label ID="lbl_ledgerEx" runat="server" Text='<%#Eval("Ledger") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Excess/Advance Amount" HeaderStyle-BackColor="#0CA6CA"
                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                    <ItemTemplate>
                                        <center>
                                            <asp:TextBox ID="txt_ExAmtpop" runat="server" onblur="checkFloatValue(this);" CssClass="  textbox txtheight1"
                                                Style="text-align: right;"></asp:TextBox></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div style="padding-top: 5px; text-align: center;">
                        <asp:Button ID="btnExcessSave" runat="server" OnClick="btnExcessOk_Click" Font-Names="Book Antiqua"
                            Text="OK" CssClass="textbox btn2 textbox1" />
                        <asp:Button ID="btnExExit" runat="server" OnClick="imgExcessclose_Click" Font-Names="Book Antiqua"
                            Text="Exit" CssClass="textbox btn2 textbox1" />
                    </div>
                </center>
            </div>
        </div>
    </center>
    <%--Excess Confirmation --%>
    <center>
        <div id="surediv" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div1" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <span class="challanLabel" style="color: Red; font-weight: bold; font-size: medium;">
                                        <p>
                                            Do you Want To Save Excess Fees</p>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_yes" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClientClick="return DisplayLoadingDiv();" OnClick="btn_sureyes_Click" Text="yes"
                                            runat="server" />
                                        <asp:Button ID="btn_no" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
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
    <%--Repeat Receipt --%>
    <center>
        <div id="divRecptRpt" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div6" runat="server" class="table" style="background-color: White; height: 350px;
                    width: 600px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 20px;
                    border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Multiple Mode</span></div>
                    </center>
                    <center>
                    </center>
                    <table style="height: 250px;">
                        <tr>
                            <td colspan="2">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lbl_mdamt" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rblCashChkDd" runat="server" RepeatDirection="Horizontal"
                                    OnSelectedIndexChanged="rblCashChkDd_OnSelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Selected="True" Value="1">Cash</asp:ListItem>
                                    <asp:ListItem Value="2">Cheque</asp:ListItem>
                                    <asp:ListItem Value="3">DD</asp:ListItem>
                                    <asp:ListItem Value="6">Card</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                Amount
                                <asp:TextBox ID="txt_mdAddAmtpop" runat="server" Text="0.00" onblur="checkFloatValue(this);"
                                    CssClass="  textbox txtheight" Height="15px" Style="text-align: right;"></asp:TextBox>
                                <asp:Button ID="btnAddNewRow" runat="server" OnClick="btnAddNewRow_onClick" CssClass="textbox textbox1 btn2"
                                    Text="Add" />
                            </td>
                        </tr>
                        <tr runat="server" id="trBankDetailsmd" visible="false">
                            <td>
                                Bank Details
                                <asp:Button ID="btnplus1" runat="server" Text="+" CssClass="textbox btn textbox1"
                                    Font-Bold="true" OnClick="btnplus1_OnClick" />
                                <asp:DropDownList ID="ddlmdBankAdd" runat="server" CssClass="  textbox  ddlheight1">
                                </asp:DropDownList>
                                <asp:Button ID="btnminus1" runat="server" Text="-" CssClass="textbox btn textbox1"
                                    Font-Bold="true" OnClick="btnminus1_OnClick" />
                                <asp:TextBox ID="txt_datemd" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_datemd" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_mdBranchAdd" runat="server" CssClass="  textbox txtheight" Height="15px"
                                    Placeholder="Branch"></asp:TextBox>
                                <asp:TextBox ID="txt_mdChequeNoAdd" runat="server" CssClass="  textbox txtheight1"
                                    Height="15px" Placeholder="Cheque/DD No"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trCardDetailsmd" visible="false">
                            <td colspan="2">
                                <span class="challanLabel" style="float: left; padding: 2px; padding-top: 5px;">
                                    <p>
                                        Card Type</p>
                                </span>
                                <asp:DropDownList ID="ddlCardTypeMd" runat="server" CssClass="textbox ddlheight2"
                                    onchange="return otherCardTypeMd(this);" Style="float: left;">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtCardTypeMd" runat="server" MaxLength="40" CssClass="textbox txtheight2"
                                    onfocus="return myFunction(this)" Placeholder="Other Cards" Style="display: none;
                                    float: left;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom"
                                    ValidChars=" " TargetControlID="txtCardTypeMd">
                                </asp:FilteredTextBoxExtender>
                                <span class="challanLabel" style="float: left; padding: 2px; padding-top: 5px;">
                                    <p>
                                        Card's Last Four Digits</p>
                                </span>
                                <asp:TextBox ID="txtLast4NoMd" runat="server" Placeholder="XXXX" CssClass="textbox txtheight"
                                    MaxLength="4" onblur="if(this.value.length!=4)this.value='';" Width="35px" Style="float: left;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterType="Numbers"
                                    TargetControlID="txtLast4NoMd">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <center>
                                    <div style="width: 560px; height: 150px; overflow: auto;">
                                        <asp:GridView ID="gridMultimode" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                            Width="460px" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_serialEx" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cb_mdpop" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Mode" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_modetypeid" runat="server" Visible="false" Text='<%#Eval("Modevalue") %>'></asp:Label>
                                                        <asp:Label ID="lbl_modetype" runat="server" Text='<%#Eval("Mode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_mddate" runat="server" Text='<%#Eval("DDDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bank" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="120px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <%-- <asp:DropDownList ID="ddlmdBank" runat="server" CssClass="  textbox  ddlheight1">
                                                            </asp:DropDownList>--%>
                                                            <asp:Label ID="lbl_mdbankid" runat="server" Visible="false" Text='<%#Eval("bankid") %>'></asp:Label>
                                                            <asp:TextBox ID="txt_mdBank" runat="server" CssClass="  textbox txtheight" Height="15px"
                                                                ReadOnly="true" Text='<%#Eval("bank") %>'></asp:TextBox>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Branch" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="120px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:TextBox ID="txt_mdbranchpop" runat="server" CssClass="  textbox txtheight" ReadOnly="true"
                                                                Text='<%#Eval("branch") %>' Height="15px"></asp:TextBox></center>
                                                        <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1mdbr" runat="server" TargetControlID="txt_mdbranchpop"
                                                            FilterType="numbers,custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cheque/DD No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="120px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:TextBox ID="txt_mdchkddnopop" runat="server" CssClass="  textbox txtheight"
                                                                Height="15px" ReadOnly="true" Text='<%#Eval("chkddno") %>'></asp:TextBox></center>
                                                        <%--<asp:FilteredTextBoxExtender ID="FTExtender1mdchkddno" runat="server" TargetControlID="txt_mdchkddnopop"
                                                            FilterType="numbers,custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="120px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:TextBox ID="txt_mdAmtpop" runat="server" onblur="checkFloatValue(this);" CssClass="  textbox txtheight"
                                                                Height="15px" Text='<%#Eval("Amount") %>' Style="text-align: right;"></asp:TextBox></center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <center>
                                    <asp:Button ID="btnyesrcpt" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                        width: 65px;" OnClick="btnyesrcpt_Click" Text="OK" runat="server" />
                                    <asp:Button ID="btnCLearrcpt" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                        width: 65px;" OnClick="btnCLearrcpt_Click" Text="Clear" runat="server" />
                                    <asp:Button ID="btnnorcpt" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                        width: 65px;" OnClick="btnnorcpt_Click" Text="Exit" runat="server" />
                                </center>
                            </td>
                        </tr>
                    </table>
            </center>
        </div>
    </center>
    </div> </center>
    <%--Staff Lookup --%>
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
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                            <asp:TextBox ID="txtsearch1c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
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
    <%--Vendor Lookup --%>
    <center>
        <div id="div_vendorLook" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton6" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 310px;"
                OnClick="btn_exitvendor_Click" />
            <br />
            <br />
            <div style="background-color: White; height: 400px; width: 650px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Select The Vendor</span></div>
                </center>
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Search By"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsearch2" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlsearch2_OnSelectedIndexChanged">
                                <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span class="challanLabel">
                                <p>
                                </p>
                            </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsearch2" runat="server" Visible="false" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetVendorName1" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch2"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                            <asp:TextBox ID="txtsearch2c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetVendorno1" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch2c"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Button ID="btn_goVendor" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                OnClick="btn_goVendor_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="lbl_errormsgvendor" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
                <FarPoint:FpSpread ID="Fpspread3" runat="server" Visible="false" ShowHeaderSelection="false"
                    BorderWidth="0px" Style="width: 620px; height: 230px; auto; border: 0px solid #999999;
                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                    OnUpdateCommand="Fpspread3vendor_Command">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <center>
                    <div>
                        <asp:Button ID="btnvendor_ok" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                            OnClick="btnvendor_ok_Click" />
                        <asp:Button ID="btnExit_vendor" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                            OnClick="btn_exitvendor_Click" />
                    </div>
                </center>
            </div>
        </div>
    </center>
    <%--Receipt Confirmation --%>
    <center>
        <div id="Div4" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div7" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <span class="challanLabel" style="color: Red; font-weight: bold; font-size: Medium;">
                                        <p>
                                            Do You Want To Save Receipt?</p>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Button1" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
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
    <%--Inventory Confirmation --%>
    <center>
        <div id="divInvConfirm" runat="server" visible="false" style="height: 100em; z-index: 10000;
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
                                    <span class="challanLabel" style="color: Red; font-weight: bold; font-size: medium;">
                                        <p>
                                            Do You Want To Save Fees?</p>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnSaveInv" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btnSaveInv_Click" Text="yes" runat="server" />
                                        <asp:Button ID="btnSaveNoInv" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btnSaveNoInv_Click" Text="no" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- ***********Add Bank*******--%>
    <div id="imgdiv3" runat="server" visible="false" style="height: 100em; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                margin-top: 200px; border-radius: 10px;">
                <table>
                    <tr>
                        <td align="center">
                            <span class="challanLabel" style="font-size: large; color: Green; font-weight: bold;">
                                <p>
                                    Add New Bank</p>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:TextBox ID="txt_description11" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                                margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_adddesc1" runat="server" Text="Add" CssClass="textbox btn1 textbox1"
                                OnClick="btndescpopadd_Click" />
                            <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" CssClass="textbox btn1 textbox1"
                                OnClick="btndescpopexit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
    </div>
    <%-- Pop Alert--%>
    <center>
        <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 100000;
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
                                        <asp:Button ID="btn_alertclose" CssClass=" textbox btn1 textbox1" Style="height: 28px;
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
    <%-- Loading Image--%>
    <center>
        <div id="divImageLoading" runat="server" style="height: 100%; z-index: 100000; width: 100%;
            background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
            display: none;">
            <center>
                <img src="../images/loader.gif" style="margin-top: 320px; height: 50px; border-radius: 10px;" />
                <br />
                <span style="font-family: Book Antiqua; font-size: Medium; font-weight: bold; color: Black;">
                    Processing Please Wait...</span>
            </center>
        </div>
    </center>
    <%-- New College Print div--%>
    <div style="height: 1px; width: 1px; overflow: auto;">
        <%-- <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false"> Aruna 06/07/2018--%>
        <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
        </div>
    </div>
    <%--Scheme Admission --%>
    <center>
        <div id="divSchemeSettings" runat="server" visible="false" class="popupstyle popupheight1 "
            style="width: 100%; margin-top: 10px;">
            <asp:ImageButton ID="imgSchemeSettingsClose" runat="server" Width="40px" Height="40px"
                ImageUrl="~/images/close.png" Style="height: 30px; width: 30px; position: absolute;
                margin-left: 450px;" OnClick="imgSchemeSettingsClose_Click" />
            <div>
                <center>
                    <div id="maindiv" runat="server" class="maindivstyle" style="width: 950px; height: 450px;">
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Receipt Amount Adjust</span></div>
                        </center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblclgScheme" Text="College" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollegeScheme" Height="25px" runat="server" CssClass="textbox3 textbox1"
                                        Width="190px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblappnoScheme" runat="server" Text="Application No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtapplScheme" runat="server" CssClass="txtheight3 txtcaps" OnTextChanged="txtapplScheme_Changed"
                                        AutoPostBack="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="tddet" runat="server" visible="false">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblstudnameScheme" runat="server" Text="Student Name:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbstudnameScheme" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="lbappnoScheme" runat="server" Visible="false" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbtypeScheme" runat="server" Text="School Type:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbscltypeScheme" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblstandScheme" runat="server" Text="Standard:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbstandScheme" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="lbldegreeScheme" runat="server" Visible="false" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblyearScheme" runat="server" Text="Year:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbyearScheme" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="lblclgSchemecode" runat="server" Visible="false" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lbl_reasScheme" runat="server" Text="Scheme Type"></asp:Label>
                                                <asp:Button ID="btn_plusScheme" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                                <asp:DropDownList ID="ddl_reasonScheme" runat="server" CssClass="textbox3 textbox1"
                                                    onfocus="return myFunction(this)">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_minusScheme" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbamtScheme" runat="server" Text="Amount"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtamountScheme" runat="server" CssClass="textbox textbox1" MaxLength="9"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftTxt" runat="server" FilterType="Numbers,Custom"
                                                    ValidChars="." TargetControlID="txtamountScheme">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbincadmisScheme" runat="server" Text="Admission No" AutoPostBack="true"
                                                    OnCheckedChanged="cbincamdis_Changed" />
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtadmnoScheme" runat="server" Enabled="false" CssClass="textbox textbox1"
                                                    onkeypress="display(this)" onblur="return getapplNo(this.value)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom"
                                                    ValidChars="." TargetControlID="txtadmnoScheme">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                                <asp:Label ID="lblerrScheme" runat="server"></asp:Label>
                                            </td>
                                            <br />
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btnadmitScheme" runat="server" Text="Admit" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox btn2" OnClick="btnadmitScheme_Click" />
                                                <%--  </td>
                                    <td>--%>
                                                <asp:Button ID="btnclearScheme" runat="server" Text="Reset" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox btn2" OnClick="btnclearScheme_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                <center>
                    <div id="plusdivScheme" runat="server" visible="false" class="popupstyle popupheight1">
                        <center>
                            <div id="panel_addgroupScheme" runat="server" visible="false" class="table" style="background-color: White;
                                height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <table style="line-height: 30px">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_addgroupScheme" runat="server" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="txt_addgroupScheme" runat="server" Width="200px" CssClass="textbox textbox1"
                                                onkeypress="display1()"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="line-height: 35px">
                                            <asp:Button ID="btn_addgroup1Scheme" runat="server" Text="Add" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Height="32px" Width="60px" OnClick="btn_addgroup_Click" />
                                            <asp:Button ID="btn_exitgroup1Scheme" runat="server" Text="Exit" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Height="32px" Width="60px" OnClick="btn_exitaddgroup_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblerrSchemeor" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                </center>
                <div id="alertdelScheme" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="alertdelSchemeete" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_delScheme" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_delScheme" Visible="false" CssClass="textbox textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_delScheme_Click" Text="Ok" runat="server" />
                                                <asp:Button ID="btn_okScheme" Visible="false" CssClass="textbox textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_okScheme_Click" Text="Cancel" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </div>
            <center>
                <div id="imgdiv2Scheme" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2Scheme" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alertScheme" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorcloseScheme" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_errorcloseScheme_Click" Text="ok" runat="server" />
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
    </center>
    <%--Scheme Admission Ends--%>
</asp:Content>
