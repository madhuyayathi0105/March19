<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="NewChallanReceipt.aspx.cs" Inherits="NewChallanReceipt"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Receipt</title>
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


            }
            else if (mode == "card") {
                //            $("#MainContent_div_cheque").hide('fast');
                //            $("#MainContent_btnGridPop").hide('fast');
                //            $("#MainContent_div_card").show('fast');

                document.getElementById("<%=div_cheque.ClientID %>").style.display = "none";
                document.getElementById("<%=div_card.ClientID %>").style.display = "block";
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

        function QuantityChange(qty) {
            checkIntValue(qty);

            var lblAmt = document.getElementById("<%=lblInvTotAmount.ClientID %>");
            var totAmt = 0;

            var tbl = document.getElementById("<%=gridInv.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 0; i < (tbl.rows.length - 1); i++) {
                var txtQty = document.getElementById('gridInv_txtQtyInv_' + i.toString());
                var txtRPU = document.getElementById('gridInv_lbl_RPUInv_' + i.toString());
                var txtAmt = document.getElementById('gridInv_txtamtInv_' + i.toString());


                var QtyVal = 0;
                var RPUVal = 0;

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
        function checkvalue() {
            var operandleft = [1000, 500, 100, 50, 20, 10, 5, 2, 1, 0];
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
                        if (i == 18) {
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
                    if (i == 18) {
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
                    checkFloatValue(txt);
                    if (chkSelectid.checked == false) {
                        txt.value = "0.00";
                    } else {
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
                        txttotaltobepaid.value = tottobepaidval.toString();
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
                    checkFloatValue(txt);
                    //                    if (chkSelectid.checked == false) {
                    //                        txt.value = "0.00";
                    //                    } else {
                    if (txt.value == "") {
                        txt.value = (parseFloat(txttotal.value) - (parseFloat(txtpaid.value) + curExcess + schlAmt + curCautAmt)).toString();
                    }
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
    </script>
    <%--Scheme Admission Ends--%>
    <%--added by sudhagar fine--%>
    <script type="text/javascript">
        function fineInclude() {
            var cbfineId = document.getElementById('<%=cbfine.ClientID %>');
            var txtfineId = document.getElementById('<%=txtfine.ClientID %>');
            var ddlfineId = document.getElementById('<%=ddlfinefee.ClientID %>');
            if (cbfineId.checked) {
                txtfineId.style.display = "block";
                ddlfineId.style.display = "block";
            }
            else {
                txtfineId.style.display = "none";
                ddlfineId.style.display = "none";
            }
        }

        function viewHistroy() {
            var divid = document.getElementById('<%=div_History.ClientID %>');
            var tbl = document.getElementById("<%=gridHist.ClientID %>");
            if (tbl != null) {
                var gridViewControls = tbl.getElementsByTagName("input");
                if ((tbl.rows.length - 1) > 0) {
                    divid.style.display = "block";
                    return false;
                }
                else {
                    alert("No History");
                    return false;
                }
            }
            else {
                alert("No History");
                return false;
            }
        }
        function hideDiv() {
            var divid = document.getElementById('<%=div_History.ClientID %>');
            divid.style.display = "none";
            return false;
        }

        function addFeesDiv() {
            var grid = document.getElementById('<%=grid_HeaderLedger.ClientID %>');
            var txtval = document.getElementById('<%=txt_ledgeSearch.ClientID %>');
            var btnsave = document.getElementById('<%=btn_ledgersave.ClientID %>');
            var btnexit = document.getElementById('<%=btn_ledgerExit.ClientID %>');
            if (grid != null)
                grid.style.display = "none";
            if (txtval != null)
                txtval.value = "";
            if (btnsave != null)
                btnsave.style.display = "none";
            if (btnexit != null)
                btnexit.style.display = "none";
            var modeName = "";
            var mode = document.getElementById('<%=rbl_rollnoNew.ClientID %>');
            var modetag = mode.getElementsByTagName("input");
            for (var i = 0; i < modetag.length; i++) {
                if (modetag[i].checked)
                    modeName = modetag[i].value;
            }
            var divid = document.getElementById('<%=div_HeaderLed.ClientID %>');
            var roll = "";
            var name = "";
            var dept = "";
            var boolflYr = false;
            if (modeName == "Student") {
                var roll = document.getElementById('<%=txt_rollno.ClientID %>').value;
                var name = document.getElementById('<%=txt_name.ClientID %>').value;
                var dept = document.getElementById('<%=txt_dept.ClientID %>').value;
                var count = 0;
                var strText = "";
                var cbflnyr = document.getElementById('<%=chkfyear.ClientID %>');
                var cblflnyr = document.getElementById('<%=chklsfyear.ClientID %>');
                var tagName = cblflnyr.getElementsByTagName("input");
                var tagNameStr = cblflnyr.getElementsByTagName("label");
                for (var i = 0; i < tagName.length; i++) {
                    if (tagName[i].checked) {
                        count += 1;
                        strText = tagNameStr[i].innerText;
                    }
                }
                if (roll == null || roll == "" || name == null || name == "" || dept == null || dept == "") {
                    alert("Enter The " + modeName + " Details");
                    return false;
                }
                if (count == 1) {
                    boolflYr = true;
                    alert("You Selected This " + strText + " Financial Year!");
                }
                else {
                    alert("Please Select Any One Financial Year!");
                    return false;
                }
            }
            else if (modeName == "Staff") {
                var roll = document.getElementById('<%=txtroll_staff.ClientID %>').value;
                var name = document.getElementById('<%=txtname_staff.ClientID %>').value;
                var dept = document.getElementById('<%=txtDept_staff.ClientID %>').value;
                boolflYr = true;
            }
            else if (modeName == "Venodr") {
                var roll = document.getElementById('<%=txtroll_vendor.ClientID %>').value;
                var name = document.getElementById('<%=txtname_vendor.ClientID %>').value;
                var dept = document.getElementById('<%=txtDept_vendor.ClientID %>').value;
                boolflYr = true;
            }
            else if (modeName == "Others") {
                var roll = document.getElementById('<%=txtroll_other.ClientID %>').value;
                // var name = document.getElementById('<%=txtname_other.ClientID %>').value;
                // var dept = document.getElementById('<%=txt_otherMobile.ClientID %>').value;
                boolflYr = true;
            }

            if (roll != null && roll != "" && name != null && name != "" && dept != null && dept != "" && modeName != "Others" && boolflYr) {
                divid.style.display = "block";
                return false;
            }
            else if (modeName == "Others" && roll != null && roll != "" && boolflYr) {
                divid.style.display = "block";
                return false;
            }
            else {
                alert("Enter The " + modeName + " Details");
                return false;
            }
        }




        function hideFeeDiv() {
            var divid = document.getElementById('<%=div_HeaderLed.ClientID %>');
            divid.style.display = "none";
            return false;
        }
        function selectAll() {
            var txt = document.getElementById('<%=txt_sem.ClientID%>');
            var cball = document.getElementById('<%=cb_sem.ClientID%>');
            var cbsingel = document.getElementById('<%=cbl_sem.ClientID%>');
            var cbinput = cbsingel.getElementsByTagName("input");
            if (cball.checked) {
                for (var i = 0; i < cbinput.length; i++) {
                    cbinput[i].checked = true;
                }
                txt.value = "Term(" + cbinput.length + ")";
            }
            else {
                for (var i = 0; i < cbinput.length; i++) {
                    cbinput[i].checked = false;
                }
                txt.value = "Term()";
            }
        }
        function selectSingle() {
            var cnt = 0;
            var txt = document.getElementById('<%=txt_sem.ClientID%>');
            var cball = document.getElementById('<%=cb_sem.ClientID%>');
            var cbsingel = document.getElementById('<%=cbl_sem.ClientID%>');
            var cbinput = cbsingel.getElementsByTagName("input");
            if (!cball.checked) {
                for (var i = 0; i < cbinput.length; i++) {
                    if (cbinput[i].checked == true) {
                        cnt++;
                    }
                }
                if (cnt == cbinput.length) {
                    cball.checked = true;
                    txt.value = "Term(" + cnt + ")";
                }
                else {
                    txt.value = "Term(" + cnt + ")";
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
                    txt.value = "Term(" + cnt + ")";
                }
                else {
                    txt.value = "Term(" + cnt + ")";
                    cball.checked = false;
                }
            }
        }
        function checkPayAmount(id) {
            var txt = document.getElementById('<%=Txt_amt.ClientID%>');
            var txtpay = id.value;
            var txtbal = document.getElementById('<%=txt_balamt.ClientID%>').value;
            if (txtpay.trim() != "" && txtpay.trim() != "0.00" && txtbal.trim() != "" && txtbal.trim() != "0.00") {
                var payamt = parseFloat(txtpay);
                var balamt = parseFloat(txtbal);
                if (payamt > balamt) {
                    txt.value = "";
                }
            }
        }
        function validateGo() {
            var checkgo = true;
            var roll = document.getElementById('<%=txt_rollno.ClientID %>').value;
            var name = document.getElementById('<%=txt_name.ClientID %>').value;
            var dept = document.getElementById('<%=txt_dept.ClientID %>').value;
            var totamt = document.getElementById('<%=txt_totamt.ClientID %>').value;
            var Payamt = document.getElementById('<%=Txt_amt.ClientID%>').value;
            var fine = document.getElementById('<%=cbfine.ClientID%>');
            var stfine = document.getElementById('<%=cbl_sem.ClientID%>');
            var header = document.getElementById('<%=cbl_grpheader.ClientID%>');
            var feecat = GetSelectedItem(stfine)
            var headerid = GetSelectedItem(header)

            var boolfine = false;
            var chfee = false;
            if (fine.checked) {
                var fineamt = document.getElementById('<%=txtfine.ClientID %>').value;
                var finefeecat = document.getElementById('<%=ddlfinefee.ClientID %>').value;
                var stud = GetSelectedItemValue(stfine);
                chfee = validateFineFeecat(finefeecat, stud);
                if (fineamt.trim() != "" && fineamt.trim() != null && finefeecat.trim() != "" && finefeecat.trim() != null && chfee)
                    boolfine = true;
                else
                    boolfine = false;
            }
            else
                boolfine = true;

            var cheque = document.getElementById('<%=rb_cheque.ClientID%>');
            var dd = document.getElementById('<%=rb_dd.ClientID%>');
            var card = document.getElementById('<%=rb_card.ClientID%>');
            var mode = false;
            if (cheque.checked || dd.checked) {
                var chqno = "";
                var bk = document.getElementById('<%=ddl_bkname.ClientID %>').value;
                var branch = document.getElementById('<%=txt_branch.ClientID %>').value;
                if (cheque.checked)
                    chqno = document.getElementById('<%=txt_chqno.ClientID%>').value;
                else
                    chqno = document.getElementById('<%=txt_ddno.ClientID%>').value;
                if (bk.trim() != "" && bk.trim() != null && branch.trim() != "" && branch.trim() != null && chqno.trim() != "" && chqno.trim() != null)
                    mode = true;
                else
                    mode = false;
            }
            else if (card.checked) {
                var cardtype = document.getElementById('<%=ddlCardType.ClientID %>').value;
                var code = document.getElementById('<%=txtLast4No.ClientID %>').value;
                if (cardtype.trim() != "" && cardtype.trim() != null && code.trim() != "" && code.trim() != null)
                    mode = true;
                else
                    mode = false;
            }
            else
                mode = true;

            if (roll == null || roll == "") {
                document.getElementById('<%=txt_rollno.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (name == null || name == "") {
                document.getElementById('<%=txt_name.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (dept == null || dept == "") {
                document.getElementById('<%=txt_dept.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            //            if (totamt == null || totamt == "" || totamt == "0") {
            //                document.getElementById('<%=txt_totamt.ClientID %>').style.borderColor = 'Red';
            //                checkgo = false;
            //            }
            if (Payamt == null || Payamt == "" || Payamt == "0") {
                //                document.getElementById('<%=Txt_amt.ClientID %>').style.borderColor = 'Red';
                //                checkgo = false;
            }
            if (!boolfine) {
                if (fineamt.trim() == "" || fineamt.trim() == null) {
                    document.getElementById('<%=txtfine.ClientID %>').style.borderColor = 'Red';
                }
                if (finefeecat.trim() == "" || finefeecat.trim() == null || !chfee) {
                    document.getElementById('<%=ddlfinefee.ClientID %>').style.borderColor = 'Red';
                }
                checkgo = false;
            }
            if (!feecat) {
                document.getElementById('<%=txt_sem.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (!headerid) {
                document.getElementById('<%=txt_grpheader.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (!mode) {
                if (cheque.checked || dd.checked) {
                    document.getElementById('<%=ddl_bkname.ClientID %>').style.borderColor = 'Red';
                    document.getElementById('<%=txt_branch.ClientID %>').style.borderColor = 'Red';
                    if (cheque.checked)
                        document.getElementById('<%=txt_chqno.ClientID%>').style.borderColor = 'Red';
                    else
                        document.getElementById('<%=txt_ddno.ClientID%>').style.borderColor = 'Red';
                    checkgo = false;
                }
                else {
                    document.getElementById('<%=ddlCardType.ClientID %>').style.borderColor = 'Red';
                    document.getElementById('<%=txtLast4No.ClientID %>').style.borderColor = 'Red';
                    checkgo = false;
                }
            }

            if (!checkgo) {
                return false;
            }
        }

        function validateFineFeecat(feecat, studfeecat) {
            var boolcheck = false;
            var strfee = studfeecat.split(",");
            for (var i = 0; i < strfee.length; i++) {
                if (strfee[i] == feecat) {
                    boolcheck = true;
                }
            }
            return boolcheck;
        }


        function validateSave() {
            var reciptno = document.getElementById('<%=txt_rcptno.ClientID %>').value;
            if (reciptno.trim() == "" || reciptno.trim() == null) {
                alert('Receipt No Not Generated');
                return false;
            }
            var checkgo = true;
            var roll = document.getElementById('<%=txt_rollno.ClientID %>').value;
            var name = document.getElementById('<%=txt_name.ClientID %>').value;
            var dept = document.getElementById('<%=txt_dept.ClientID %>').value;
            var totamt = document.getElementById('<%=txt_totamt.ClientID %>').value;
            var Payamt = document.getElementById('<%=Txt_amt.ClientID%>').value;
            var fine = document.getElementById('<%=cbfine.ClientID%>');
            var stfine = document.getElementById('<%=cbl_sem.ClientID%>');
            var header = document.getElementById('<%=cbl_grpheader.ClientID%>');
            var feecat = GetSelectedItem(stfine)
            var headerid = GetSelectedItem(header)
            var boolfine = false;
            var chfee = false;
            if (fine.checked) {
                var fineamt = document.getElementById('<%=txtfine.ClientID %>').value;
                var finefeecat = document.getElementById('<%=ddlfinefee.ClientID %>').value;
                var stud = GetSelectedItemValue(stfine);
                chfee = validateFineFeecat(finefeecat, stud);
                if (fineamt.trim() != "" && fineamt.trim() != null && finefeecat.trim() != "" && finefeecat.trim() != null && chfee)
                    boolfine = true;
                else
                    boolfine = false;
            }
            else
                boolfine = true;


            var cheque = document.getElementById('<%=rb_cheque.ClientID%>');
            var dd = document.getElementById('<%=rb_dd.ClientID%>');
            var card = document.getElementById('<%=rb_card.ClientID%>');

            var mode = false;
            if (cheque.checked || dd.checked) {
                var chqno = "";
                var bk = document.getElementById('<%=ddl_bkname.ClientID %>').value;
                var branch = document.getElementById('<%=txt_branch.ClientID %>').value;
                if (cheque.checked)
                    chqno = document.getElementById('<%=txt_chqno.ClientID%>').value;
                else
                    chqno = document.getElementById('<%=txt_ddno.ClientID%>').value;
                if (bk.trim() != "" && bk.trim() != null && branch.trim() != "" && branch.trim() != null && chqno.trim() != "" && chqno.trim() != null)
                    mode = true;
                else
                    mode = false;
            }
            else if (card.checked) {
                var cardtype = document.getElementById('<%=ddlCardType.ClientID %>').value;
                var code = document.getElementById('<%=txtLast4No.ClientID %>').value;
                if (cardtype.trim() != "" && cardtype.trim() != null && code.trim() != "" && code.trim() != null)
                    mode = true;
                else
                    mode = false;
            }
            else
                mode = true;

            if (roll == null || roll == "") {
                document.getElementById('<%=txt_rollno.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (name == null || name == "") {
                document.getElementById('<%=txt_name.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (dept == null || dept == "") {
                document.getElementById('<%=txt_dept.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (totamt == null || totamt == "" || totamt == "0") {
                document.getElementById('<%=txt_totamt.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (Payamt == null || Payamt == "" || Payamt == "0") {
                document.getElementById('<%=Txt_amt.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (!boolfine) {
                if (fineamt.trim() == "" || fineamt.trim() == null) {
                    document.getElementById('<%=txtfine.ClientID %>').style.borderColor = 'Red';
                }
                if (finefeecat.trim() == "" || finefeecat.trim() == null || !chfee) {
                    document.getElementById('<%=ddlfinefee.ClientID %>').style.borderColor = 'Red';
                }
                checkgo = false;
            }
            if (!feecat) {
                document.getElementById('<%=txt_sem.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (!headerid) {
                document.getElementById('<%=txt_grpheader.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (reciptno == null || reciptno == "") {
                document.getElementById('<%=txt_rcptno.ClientID %>').style.borderColor = 'Red';
                checkgo = false;
            }
            if (!mode) {
                if (cheque.checked || dd.checked) {
                    document.getElementById('<%=ddl_bkname.ClientID %>').style.borderColor = 'Red';
                    document.getElementById('<%=txt_branch.ClientID %>').style.borderColor = 'Red';
                    if (cheque.checked)
                        document.getElementById('<%=txt_chqno.ClientID%>').style.borderColor = 'Red';
                    else
                        document.getElementById('<%=txt_ddno.ClientID%>').style.borderColor = 'Red';
                    checkgo = false;
                }
                else {
                    document.getElementById('<%=ddlCardType.ClientID %>').style.borderColor = 'Red';
                    document.getElementById('<%=txtLast4No.ClientID %>').style.borderColor = 'Red';
                    checkgo = false;
                }
            }
            if (!checkgo) {
                return false;
            }
            //            else
            //                return confirm('Are Sure Want To Continue');
        }

        function GetSelectedItem(cblid) {
            var selbool = false;
            var selectedValue = "";
            var CHK = cblid;
            var checkbox = CHK.getElementsByTagName("input");
            for (var i = 0; i < checkbox.length; i++) {
                if (checkbox[i].checked) {
                    if (selectedValue == "")
                        selectedValue = checkbox[i].value;
                    else
                        selectedValue = selectedValue + "," + checkbox[i].value;
                }
            }
            if (selectedValue != "")
                selbool = true;
            return selbool;
        }

        function GetSelectedItemValue(cblid) {
            var selectedValue = "";
            var CHK = cblid;
            var checkbox = CHK.getElementsByTagName("input");
            for (var i = 0; i < checkbox.length; i++) {
                if (checkbox[i].checked) {
                    if (selectedValue == "")
                        selectedValue = checkbox[i].value;
                    else
                        selectedValue = selectedValue + "," + checkbox[i].value;
                }
            }
            return selectedValue;
        }

        function validateMultiMode() {
            var txtname = "";
            var radiolist = document.getElementById('<%=rblCashChkDd.ClientID %>');
            var radio = radiolist.getElementsByTagName("input");
            for (var x = 0; x < radio.length; x++) {
                if (radio[x].checked) {
                    txtname = radio[x].value;
                }
            }
            var boolAdd = true;
            var Amt = document.getElementById('<%=txt_mdAddAmtpop.ClientID %>').value;
            if (txtname.trim() == "1") {
                if (Amt == null || Amt == "") {
                    document.getElementById('<%=txt_mdAddAmtpop.ClientID %>').style.borderColor = 'Red';
                    boolAdd = false;
                }
            }
            else if (txtname.trim() == "2" || txtname.trim() == "3") {
                var branch = document.getElementById('<%=txt_mdBranchAdd.ClientID %>').value;
                var no = document.getElementById('<%=txt_mdChequeNoAdd.ClientID %>').value;
                if (branch == null || branch == "") {
                    document.getElementById('<%=txt_mdBranchAdd.ClientID %>').style.borderColor = 'Red';
                    boolAdd = false;
                }
                if (no == null || no == "") {
                    document.getElementById('<%=txt_mdChequeNoAdd.ClientID %>').style.borderColor = 'Red';
                    boolAdd = false;
                }
                if (Amt == null || Amt == "") {
                    document.getElementById('<%=txt_mdAddAmtpop.ClientID %>').style.borderColor = 'Red';
                    boolAdd = false;
                }
            }
            else if (txtname.trim() == "6") {
                var branch = document.getElementById('<%=ddlCardTypeMd.ClientID %>').value;
                var no = document.getElementById('<%=txtLast4NoMd.ClientID %>').value;
                if (branch == null || branch == "" || branch == "Select") {
                    document.getElementById('<%=ddlCardTypeMd.ClientID %>').style.borderColor = 'Red';
                    boolAdd = false;
                }
                if (no == null || no == "") {
                    document.getElementById('<%=txtLast4NoMd.ClientID %>').style.borderColor = 'Red';
                    boolAdd = false;
                }
                if (Amt == null || Amt == "") {
                    document.getElementById('<%=txt_mdAddAmtpop.ClientID %>').style.borderColor = 'Red';
                    boolAdd = false;
                }
            }
            if (!boolAdd) {
                return false;
            }
        }
        function ModeChanged() {
            var txtname = "";
            var radiolist = document.getElementById('<%=rblCashChkDd.ClientID %>');
            var radio = radiolist.getElementsByTagName("input");
            for (var x = 0; x < radio.length; x++) {
                if (radio[x].checked) {
                    txtname = radio[x].value;
                }
            }
            var bank = document.getElementById('<%=trBankDetailsmd.ClientID %>');
            var card = document.getElementById('<%=trCardDetailsmd.ClientID %>');
            bank.style.display = "none";
            card.style.display = "none";
            if (txtname.trim() == "1") {
                bank.style.display = "none";
                card.style.display = "none";
                return false;
            }
            if (txtname.trim() == "2" || txtname.trim() == "3") {
                bank.style.display = "block";
                card.style.display = "none";
                return false;
            }
            if (txtname.trim() == "6") {
                bank.style.display = "none";
                card.style.display = "block";
                return false;
            }
        }

        function hideMultimodeDive() {
            var divid = document.getElementById('<%=divRecptRpt.ClientID %>');
            var bank = document.getElementById('<%=trBankDetailsmd.ClientID %>');
            var card = document.getElementById('<%=trCardDetailsmd.ClientID %>');
            divid.style.display = "none";
            bank.style.display = "none";
            card.style.display = "none";
            return false;
        }
        function alertdivhide() {
            var divid = document.getElementById('<%=imgAlert.ClientID %>');
            divid.style.display = "none";
            return false;
        }
        function viewSchemeAdmission() {
            var applid = document.getElementById('<%=radApplNo.ClientID %>');
            var admiss = document.getElementById('<%=radAdmNo.ClientID %>');
            document.getElementById('<%=txtapplScheme.ClientID %>').value = "";
            document.getElementById('<%=lbstudnameScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lbappnoScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lbscltypeScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lbstandScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lbldegreeScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lbyearScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lblclgSchemecode.ClientID %>').innerHTML = "";
            document.getElementById('<%=ddl_reasonScheme.ClientID %>').value = "--Select--";
            document.getElementById('<%= txtamountScheme.ClientID %>').value = "";
            if (applid.checked) {
                document.getElementById('<%= txtadmnoScheme.ClientID %>').value = "";
                document.getElementById('<%=cbincadmisScheme.ClientID %>').checked = false;
            }
            applid.checked = true;
            var divid = document.getElementById('<%=tddet.ClientID %>');
            divid.style.display = "none";
            var maindiv = document.getElementById('<%=divSchemeSettings.ClientID %>');
            maindiv.style.display = "block";
            return false;
        }
        function hideSchemediv() {
            var maindiv = document.getElementById('<%=divSchemeSettings.ClientID %>');
            maindiv.style.display = "none";
            return false;
        }
        function clearScheme() {
            document.getElementById('<%=txtapplScheme.ClientID %>').value = "";
            document.getElementById('<%=lbstudnameScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lbappnoScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lbscltypeScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lbstandScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lbldegreeScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lbyearScheme.ClientID %>').innerHTML = "";
            document.getElementById('<%=lblclgSchemecode.ClientID %>').innerHTML = "";
            document.getElementById('<%=ddl_reasonScheme.ClientID %>').value = "--Select--";
            document.getElementById('<%= txtamountScheme.ClientID %>').value = "";
            document.getElementById('<%= txtadmnoScheme.ClientID %>').value = "";
            document.getElementById('<%=cbincadmisScheme.ClientID %>').checked = false;
            var divid = document.getElementById('<%=tddet.ClientID %>');
            divid.style.display = "none";
            return false;
        }

        function myFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }


        function getTempReceiptNo(txttempNo) {
            $.ajax({
                type: "POST",
                url: "NewChallanReceipt.aspx/CheckReceiptNo",
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green;">Receipt </span>
        </div>
    </center>
    <%--  Design Started By Idhris 12/10/2015 --%>
    <center>
        <div class="maindivstyle" style="width: 980px; overflow: auto;">
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
                                    <asp:TextBox ID="txt_date" runat="server" AutoPostBack="true" OnTextChanged="txt_date_Changed"
                                        CssClass="textbox txtheight" Height="15px"></asp:TextBox>
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
                                                                    OnTextChanged="txt_rollno_Changed" AutoPostBack="true" onfocus="myFunction(this)"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderroll" runat="server" TargetControlID="txt_rollno"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <%--<asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>--%>
                                                                <%--mobile no search--%>
                                                                <asp:TextBox ID="txt_mblno" runat="server" placeholder="Mobile No" CssClass="textbox  txtheight2"
                                                                    OnTextChanged="txt_mblno_Changed" AutoPostBack="true"></asp:TextBox>
                                                                <%--OnTextChanged="txt_mblno_Changed" AutoPostBack="true"--%>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_mblno"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="- ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender14" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetMblno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_mblno"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:Button ID="btn_roll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                                                    OnClick="btn_roll_Click" />
                                                                <br />
                                                                <asp:TextBox ID="txt_name" runat="server" placeholder="Name" CssClass="textbox txtheight2"
                                                                    Width="300px" OnTextChanged="txt_name_Changed" AutoPostBack="true" onfocus="myFunction(this)"></asp:TextBox>
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
                                                                                onfocus="myFunction(this)" AutoPostBack="true" OnTextChanged="txt_dept_Changed"></asp:TextBox>
                                                                            <asp:Label ID="lblTempDeg" runat="server" Visible="false"></asp:Label>
                                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender15" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetDepartment" MinimumPrefixLength="0" CompletionInterval="100"
                                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_dept"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="panelbackground">
                                                                            </asp:AutoCompleteExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddl_sem" runat="server" CssClass="textbox  ddlheight2" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="ddl_sem_OnSelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txtsec" runat="server" placeholder="Section" CssClass="textbox txtheight2"
                                                                                onfocus="myFunction(this)" AutoPostBack="true" OnTextChanged="txtsec_Changed"></asp:TextBox>
                                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender16" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="GetSections" MinimumPrefixLength="0" CompletionInterval="100"
                                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsec"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="panelbackground">
                                                                            </asp:AutoCompleteExtender>
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
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <%--staff--%>
                                                <div>
                                                    <table id="rcptSngleStaff" runat="server" visible="false">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtroll_staff" runat="server" placeholder="Staff Id" CssClass="textbox  txtheight2"
                                                                    OnTextChanged="txtroll_staff_Changed" AutoPostBack="true"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtroll_staff"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="/ ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroll_staff"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:Button ID="btn_staffLook" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                                                    OnClick="btn_staffLook_Click" />
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
                                                                                        placeholder="Semester/Year"></asp:TextBox>
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
                                                        <tr>
                                                            <td>
                                                                <br />
                                                                <br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <%--vendor--%>
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
                                                                                        placeholder="Semester/Year"></asp:TextBox>
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
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <br />
                                                                <br />
                                                                <br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <%-- other--%>
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
                                                                                        placeholder="Semester/Year"></asp:TextBox>
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
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <br />
                                                                <br />
                                                                <br />
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
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <table>
                                                    <tr>
                                                        <td id="balRow" runat="server">
                                                            <asp:Label ID="lbl_totamt" runat="server" Text="Total"></asp:Label>
                                                            <asp:TextBox ID="txt_totamt" placeholder="0.00" runat="server" onblur="checkFloatValue(this);"
                                                                CssClass="textbox txtheight" Style="text-align: right; width: 45px; height: 15px;"
                                                                BackColor="#81F7D8" onfocus="myFunction(this)"></asp:TextBox>
                                                            <asp:Label ID="lbl_paidamt" runat="server" Text="Paid"></asp:Label>
                                                            <asp:TextBox ID="txt_paidamt" runat="server" placeholder="0.00" onblur="checkFloatValue(this);"
                                                                CssClass="textbox txtheight" Style="text-align: right; width: 45px; height: 15px;"
                                                                BackColor="#81F7D8"></asp:TextBox>
                                                            <asp:Label ID="lbl_balamt" runat="server" Text="Balance"></asp:Label>
                                                            <asp:TextBox ID="txt_balamt" runat="server" placeholder="0.00" onblur="checkFloatValue(this);"
                                                                CssClass="textbox txtheight" Style="text-align: right; width: 45px; height: 15px;"
                                                                BackColor="#81F7D8"></asp:TextBox>
                                                        </td>
                                                        <td id="tdstudsem" runat="server" visible="true">
                                                            <asp:UpdatePanel ID="UPpanel_sem" runat="server">
                                                                <ContentTemplate>
                                                                    <div style="position: relative;">
                                                                        <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight2 txtboxBg" ReadOnly="true"
                                                                            placeholder="Semester/Year" onfocus="myFunction(this)"></asp:TextBox>
                                                                        <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel">
                                                                            <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="Select All" onchange=" selectAll(this);" />
                                                                            <%--AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged"--%>
                                                                            <asp:CheckBoxList ID="cbl_sem" runat="server" onchange=" selectSingle(this);">
                                                                            </asp:CheckBoxList>
                                                                            <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged"--%>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="popupce_sem" runat="server" TargetControlID="txt_sem"
                                                                            PopupControlID="panel_sem" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltxtamt" runat="server" Text="Pay"></asp:Label>
                                                            <asp:TextBox ID="Txt_amt" runat="server" placeholder="0.00" onblur="checkFloatValue(this);"
                                                                onchange="checkPayAmount(this);" CssClass="textbox txtheight" Style="text-align: right;
                                                                width: 90px; height: 15px;" MaxLength="15" onfocus="myFunction(this)">
                                                            </asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="Txt_amt"
                                                                FilterType="Numbers">
                                                            </asp:FilteredTextBoxExtender>
                                                            <%--BackColor="#E49B2C"--%>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="btn_History" runat="server" Visible="false" OnClientClick="return viewHistroy()"
                                                                Text="History" Style="float: left; font-weight: bold; font-size: 14px;"></asp:LinkButton>
                                                            <%--OnClick="btnHistory_Click"--%>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="btnAddFee" runat="server" OnClientClick="return addFeesDiv();"
                                                                Text="AddFee" Style="float: left; font-weight: bold; font-size: 14px;"></asp:LinkButton>
                                                            <%--OnClick="btnAddFee_Click"--%>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkOpenSchemeSettings" runat="server" Text="Scheme" OnClientClick="return viewSchemeAdmission();"
                                                                Visible="false" Style="font-family: 'Book Antiqua'; font-weight: bold; font-size: 16px;
                                                                color: Green; background-color: white; padding: 2px;"></asp:LinkButton>
                                                        </td>
                                                        <td colspan="2">
                                                            <fieldset>
                                                                <asp:Label ID="Label3" runat="server" Text="Collected Amount:" Style="font-size: 13px;
                                                                    font-weight: bold;"></asp:Label>
                                                                <asp:Label ID="lblcollect" runat="server" Style="color: Green; font-size: 18px; font-weight: bold;"></asp:Label>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table>
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
                                                                <asp:RadioButton ID="rb_NEFT" runat="server" RepeatDirection="Horizontal" GroupName="same3"
                                                                Text="NEFT" onchange="rb_NEFT_Onchange('NEFT')"></asp:RadioButton>
                                                                     <asp:RadioButton ID="rb_Challan" runat="server" RepeatDirection="Horizontal" GroupName="same3"
                                                                Text="Challan" onchange="rb_ccdc_Onchange('card')"></asp:RadioButton>
                                                            <%--OnCheckedChanged="rb_card_CheckedChanged" AutoPostBack="true"--%>
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lbl_remark" runat="server" Text="Remarks"></asp:Label>
                                                            <asp:TextBox ID="txt_remark" runat="server" CssClass="textbox txtheight5"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderrem" runat="server" TargetControlID="txt_remark"
                                                                FilterType="UppercaseLetters, LowercaseLetters,Numbers, Custom" ValidChars=" /\._!@$*()~#%^-+">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:TextBox ID="txttemprcpt" runat="server" CssClass="textbox txtheight5" Visible="false"
                                                                placeholder="Receipt No" Width="110px" onblur="return getTempReceiptNo(this.value)"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txttemprcpt"
                                                                FilterType="UppercaseLetters, LowercaseLetters,Numbers, Custom" ValidChars=" . /">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:Label ID="lblrcptalert" runat="server"></asp:Label>
                                                            <%--<asp:TextBox ID="txtcollect" runat="server" Enabled="false" CssClass="textbox txtheight5"
                                                                Width="100px"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtcollect"
                                                                FilterType="UppercaseLetters, LowercaseLetters,Numbers, Custom" ValidChars=" /\._!@$*()~#%^-+">
                                                            </asp:FilteredTextBoxExtender>--%>
                                                            <asp:Button ID="btnSearch" Text="Go" runat="server" CssClass="textbox btn1 textbox1"
                                                                OnClientClick="return validateGo();" OnClick="btn_search_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblfyear" Visible="true" Text="FinanceYear" Width="85px"></asp:Label>
                                                        </td>
                                                        <td id="tdfyear" runat="server" visible="true">
                                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
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
                                                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtfyear"
                                                                        PopupControlID="Pfyear" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
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
                    <div id="div_HeadSelect" runat="server" style="left: 50%; padding-bottom: 2px; top: 25%;
                        position: absolute; width: 480px;">
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
                                <fieldset id="div_grphdr" visible="false" runat="server" style="height: 26px; width: 350px;
                                    float: left;">
                                    <table style="float: left;">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_grpheader" runat="server" CssClass="textbox txtheight4 txtboxBg"
                                                            ReadOnly="true" Width="330px" onfocus="myFunction(this)"></asp:TextBox>
                                                        <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Width="340px" Style="text-align: left;
                                                            height: 158px;">
                                                            <asp:CheckBox ID="cb_selectHeadAll" runat="server" Text="Select All" Height="15px"
                                                                onchange="return cb_selectHeadAllChange()" />
                                                            <%--OnCheckedChanged="cb_selectHeadAll_OnCheckedChanged" AutoPostBack="True"--%>
                                                            <asp:CheckBoxList ID="cbl_grpheader" runat="server" onchange="return cb_selectsingleHeadAllChange()">
                                                            </asp:CheckBoxList>
                                                            <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_grpheader_SelectedIndexChanged"--%>
                                                        </asp:Panel>
                                                        <%--<asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_grpheader"
                                                            PopupControlID="panel_dept" Position="Bottom">
                                                        </asp:PopupControlExtender>--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdser" runat="server" visible="false">
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
                                            <%--OnClick="lnkOpenSchemeSettings_OnClick"--%>
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
                    <table id="div_ch1" runat="server" style="padding-left: 10px; width: 980px;">
                        <tr id="div_cheque" runat="server">
                            <td>
                                <span class="challanLabel">
                                    <p>
                                        Bank</p>
                                </span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_bkname" runat="server" CssClass="textbox ddlheight2" onchange="return otherBank(this);"
                                    onfocus="myFunction(this)">
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
                                <asp:TextBox ID="txt_branch" runat="server" Placeholder="Branch" CssClass="textbox txtheight2"
                                    onfocus="myFunction(this)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_chqno" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_chqno" runat="server" Placeholder="Cheque No" CssClass="textbox txtheight2"
                                    onfocus="myFunction(this)"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbl_ddno" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_ddno" runat="server" Placeholder="DD No" CssClass="textbox txtheight2"
                                    onfocus="myFunction(this)"></asp:TextBox>
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
                        <tr id="div_card" runat="server">
                            <td>
                                <asp:Label ID="lblCardName" runat="server" Text="Card Type" Style="float: left; padding: 2px;
                                    padding-top: 5px;"></asp:Label>
                                <asp:DropDownList ID="ddlCardType" runat="server" CssClass="textbox ddlheight2" onchange="return otherCardType(this);"
                                    Style="float: left;" onfocus="myFunction(this)">
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
                                    MaxLength="4" onblur="if(this.value.length!=4)this.value='';" Width="35px" Style="float: left;"
                                    onfocus="myFunction(this)"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="fceCardTxt" runat="server" FilterType="Numbers"
                                    TargetControlID="txtLast4No">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <table style="padding-left: 10px; width: 980px;">
                        <tr>
                            <td>
                                <%-- <asp:Button ID="btnAddFee" runat="server" OnClick="btnAddFee_Click" Text="AddFee"
                                    CssClass="textbox btn2 textbox1" Width="50px" Style="float: left;" />--%>
                                <asp:CheckBox ID="cbfine" runat="server" Enabled="false" Text="" onchange="return fineInclude()"
                                    Style="float: left;" />
                                <asp:TextBox ID="txtfine" runat="server" CssClass="textbox txtheight2" Style="display: none;
                                    float: left;" placeholder="Amount" onblur="checkFloatValue(this);" onfocus="myFunction(this)"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" FilterType="Numbers"
                                    TargetControlID="txtfine">
                                </asp:FilteredTextBoxExtender>
                                <asp:DropDownList ID="ddlfinefee" runat="server" CssClass="textbox ddlheight5" Style="display: none;
                                    float: left;" Width="120px" onfocus="myFunction(this)">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--Row2 --%>
                <div id="divgrid" runat="server" visible="false" style="width: 980px; height: 350px;
                    overflow: auto; background-color: white; border-radius: 10px;">
                    <table style="padding-left: 50px; float: left; clear: both; width: 950px; height: 35px;">
                        <tr>
                            <td>
                                <span style="padding-right: 100px;">
                                    <asp:CheckBox ID="chkGridSelectAll" runat="server" Checked="true" Text="SelectAll"
                                        onchange="return SelLedgers();" /><%--onchange="return SelLedgers();"--%>
                                </span><span style="padding-left: 50px;">
                                    <%--AutoPostBack="true" OnCheckedChanged="chkGridSelectAll_Changed"--%>
                                    <asp:Label ID="lbl_CurPay" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="" ForeColor="Red"></asp:Label></span>
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btn_save" BackColor="#8199FD" Text="Save" runat="server" CssClass="textbox btn2 textbox1"
                                    OnClientClick="return  validateSave()" OnClick="btn_save_Click" Font-Names=" Book Antiqua"
                                    Font-Size="Medium" />
                                <asp:Button ID="btn_print" BackColor="#8199FD" Text="Print" runat="server" CssClass="textbox btn2 textbox1"
                                    OnClick="btn_print_Click" Font-Names=" Book Antiqua" Font-Size="Medium" />
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
                                <asp:TemplateField HeaderText="Term" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl_textCode" runat="server" Text='<%#Eval("TextCode") %>' Visible="false"
                                                Width="50px"></asp:Label>
                                            <asp:Label ID="lbl_textval" runat="server" Text='<%#Eval("Textval") %>' Width="50px"></asp:Label></center>
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
                                                Height="15px" Width="55px" ReadOnly="true" Style="text-align: right;"></asp:TextBox></center>
                                        <asp:FilteredTextBoxExtender ID="filterextender3" runat="server" TargetControlID="txt_tot_amt"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:TextBox ID="txt_paid_amt" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("PaidAmt") %>'
                                                Height="15px" Width="50px" Style="text-align: right;"></asp:TextBox></center>
                                        <asp:FilteredTextBoxExtender ID="filterextender4" runat="server" TargetControlID="txt_paid_amt"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:TextBox ID="txt_bal_amt" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("BalAmt") %>'
                                                Height="15px" Width="55px" Style="text-align: right;"></asp:TextBox></center>
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
                                                onchange="return checkpaidamount();" onblur="if(this.value=='')this.value='0.00';return checkpaidamount();"
                                                Height="15px" Width="70px"></asp:TextBox></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Excess" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:TextBox ID="txt_gridexcess_amt" runat="server" onblur="checkFloatValue(this);"
                                                CssClass="  textbox txtheight" Style="text-align: right;" Text='<%#Eval("Monthly") %>'
                                                ReadOnly="true" Height="15px" Width="60px"></asp:TextBox></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Scholarship" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:TextBox ID="txt_scholar_amt" runat="server" onblur="checkFloatValue(this);"
                                                CssClass="  textbox txtheight" Style="text-align: right;" Text='<%#Eval("Scholar") %>'
                                                ReadOnly="true" Height="15px" Width="60px"></asp:TextBox></center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Caution Deposit" Visible="false" HeaderStyle-BackColor="#0CA6CA"
                                    HeaderStyle-HorizontalAlign="Center">
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
        </div>
    </center>
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
                                         ></asp:TextBox>
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
                        BorderWidth="0px" Width="670px" Style="overflow: auto; height: 250px; border: 0px solid #999999;
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
                OnClick="imagebtnpopGridclose_Click" />
            <br />
            <br />
            <br />
            <br />
            <div style="background-color: White; height: 500px; width: 300px; border: 5px solid #0CA6CA;
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
            </div>
        </div>
    </center>
    <%-- Pop Add Header Ledger New --%>
    <center>
        <div id="div_HeaderLed" runat="server" style="display: none;" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton2" runat="server" OnClientClick="return hideFeeDiv();"
                Width="40px" Height="40px" ImageUrl="~/images/close.png" Style="height: 30px;
                width: 30px; position: absolute; margin-top: 75px; margin-left: 450px;" />
            <%--OnClick="imagebtnpopLedgerclose_Click"--%>
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
                                            placeholder="Semester/Year"></asp:TextBox>
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
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_ledgeSearch_Change">
                                    <asp:ListItem>Header</asp:ListItem>
                                    <asp:ListItem>Ledger</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td id="tdsemadd" runat="server" visible="false">
                                <asp:UpdatePanel ID="UpdatePanels6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtsemadd" runat="server" CssClass="textbox txtheight2 txtboxBg"
                                            ReadOnly="true" placeholder="Semester/Year" onfocus="myFunction(this)"></asp:TextBox>
                                        <asp:Panel ID="panel3" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cbsemadd" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbsemadd_CheckedChanged" />
                                            <%--AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged"--%>
                                            <asp:CheckBoxList ID="cblsemadd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblsemadd_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                            <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged"--%>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtsemadd"
                                            PopupControlID="panel3" Position="Bottom">
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
                                Width="650px" Border="0" OnDataBound="grid_HeaderLedger_OnDataBound">
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
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btn_ledgersave" runat="server" Style="display: none;" OnClick="btnpopLedgersave_Click"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Save" CssClass="textbox btn2 textbox1" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_ledgerExit" runat="server" Style="display: none;" OnClientClick="return hideFeeDiv();"
                                                OnClick="imagebtnpopLedgerclose_Click" Font-Names="Book Antiqua" Text="Exit"
                                                CssClass="textbox btn2 textbox1" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </asp:Panel>
                <asp:Panel ID="pnlInventLedger" runat="server" Visible="false">
                    <table class="maintablestyle">
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
                                <span class="challanLabel">
                                    <p>
                                        Department</p>
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
                                <%--<asp:UpdatePanel ID="updDeptInv" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtDeptInv" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            placeholder="Department"  ></asp:TextBox>
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
                                                <asp:TextBox ID="txtQtyInv" runat="server" onblur="return QuantityChange(this);"
                                                    Text='<%#Eval("Quanti") %>' CssClass="textbox txtheight" Style="text-align: right;"
                                                    Width="45px"></asp:TextBox></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <center>
                                                <asp:TextBox ID="txtamtInv" runat="server" CssClass="textbox txtheight" Style="text-align: right;"
                                                    Text='<%#Eval("Amount") %>'></asp:TextBox></center>
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
        <div id="div_History" runat="server" class="popupstyle popupheight1 " style="display: none;">
            <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 75px; margin-left: 440px;"
                OnClientClick="return hideDiv();" />
            <%--OnClick="imagebtnpopHistclose_Click"--%>
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
    <%--Repeat Receipt --%>
    <center>
        <div id="divRecptRpt" runat="server" style="height: 60em; z-index: 1000; width: 100%;
            background-color: rgba(54, 25, 25, .2); position: absolute; top: 15%; left: 0px;
            display: none;">
            <center>
                <div id="Div6" runat="server" class="table" style="background-color: White; height: 450px;
                    width: 900px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 20px;
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
                            <td colspan="4">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lbl_mdamt" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblCashChkDd" runat="server" RepeatDirection="Horizontal"
                                    onchange="return ModeChanged();">
                                    <asp:ListItem Selected="True" Value="1">Cash</asp:ListItem>
                                    <asp:ListItem Value="2">Cheque</asp:ListItem>
                                    <asp:ListItem Value="3">DD</asp:ListItem>
                                    <asp:ListItem Value="6">Card</asp:ListItem>
                                </asp:RadioButtonList>
                                <%--OnSelectedIndexChanged="rblCashChkDd_OnSelectedIndexChanged" AutoPostBack="true"--%>
                            </td>
                            <td colspan="2">
                                Amount
                                <asp:TextBox ID="txt_mdAddAmtpop" runat="server" Text="0.00" onblur="checkFloatValue(this);"
                                    CssClass="  textbox txtheight" Height="15px" Style="text-align: right;" onfocus="myFunction(this)"></asp:TextBox>
                                <asp:Button ID="btnAddNewRow" runat="server" OnClick="btnAddNewRow_onClick" CssClass="textbox textbox1 btn2"
                                    Text="Add" OnClientClick="return validateMultiMode();" />
                            </td>
                        </tr>
                        <tr id="trBankDetailsmd" runat="server" style="display: none;">
                            <td colspan="4">
                                <table>
                                    <tr>
                                        <td>
                                            Bank Details
                                            <asp:Button ID="btnplus1" runat="server" Text="+" CssClass="textbox btn textbox1"
                                                Font-Bold="true" OnClick="btnplus1_OnClick" />
                                            <asp:DropDownList ID="ddlmdBankAdd" runat="server" CssClass="  textbox  ddlheight1">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnminus1" runat="server" Text="-" CssClass="textbox btn textbox1"
                                                Font-Bold="true" OnClick="btnminus1_OnClick" />
                                            <asp:TextBox ID="txt_datemd" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_datemd" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_mdBranchAdd" runat="server" CssClass="  textbox txtheight" Height="15px"
                                                Placeholder="Branch" onfocus="myFunction(this)"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_mdChequeNoAdd" runat="server" CssClass="  textbox txtheight1"
                                                Height="15px" Placeholder="Cheque/DD No" onfocus="myFunction(this)"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server" id="trCardDetailsmd" style="display: none;">
                            <td colspan="4">
                                <table>
                                    <tr>
                                        <td>
                                            <span class="challanLabel" style="float: left; padding: 2px; padding-top: 5px;">
                                                <p>
                                                    Card Type</p>
                                            </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCardTypeMd" runat="server" CssClass="textbox ddlheight2"
                                                onchange="return otherCardTypeMd(this);" Style="float: left;">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtCardTypeMd" runat="server" MaxLength="40" CssClass="textbox txtheight2"
                                                onfocus="return myFunction(this)" Placeholder="Other Cards" Style="display: none;
                                                float: left;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Numbers, Custom"
                                                ValidChars=" " TargetControlID="txtCardTypeMd">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <span class="challanLabel" style="float: left; padding: 2px; padding-top: 5px;">
                                                <p>
                                                    Card's Last Four Digits</p>
                                            </span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLast4NoMd" runat="server" Placeholder="XXXX" CssClass="textbox txtheight"
                                                MaxLength="4" onblur="if(this.value.length!=4)this.value='';" Width="35px" Style="float: left;"
                                                onfocus="myFunction(this)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterType="Numbers"
                                                TargetControlID="txtLast4NoMd">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
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
                            <td colspan="4">
                                <center>
                                    <asp:Button ID="btnyesrcpt" Visible="false" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                        width: 65px;" OnClick="btnyesrcpt_Click" Text="OK" runat="server" />
                                    <asp:Button ID="btnCLearrcpt" Visible="false" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                        width: 65px;" OnClick="btnCLearrcpt_Click" Text="Clear" runat="server" />
                                    <asp:Button ID="btnnorcpt" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                        width: 65px;" OnClientClick="return hideMultimodeDive();" OnClick="btnnorcpt_Click"
                                        Text="Exit" runat="server" />
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
                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
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
        <div id="Div4" runat="server" visible="false" style="height: 60em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 8%;
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
        <div id="imgAlert" runat="server" style="height: 60em; z-index: 100000; width: 100%;
            background-color: rgba(54, 25, 25, .2); position: absolute; top: 13%; left: 0px;
            display: none;">
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
                                            width: 65px;" OnClientClick="return alertdivhide();" OnClick="btn_alertclose_Click"
                                            Text="ok" runat="server" />
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
        <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
        </div>
    </div>
    <%--Scheme Admission --%>
    <center>
        <div id="divSchemeSettings" runat="server" class="popupstyle popupheight1 " style="width: 100%;
            margin-top: 4%; height: 60em; display: none;">
            <asp:ImageButton ID="imgSchemeSettingsClose" runat="server" Width="40px" Height="40px"
                ImageUrl="~/images/close.png" Style="height: 30px; width: 30px; position: absolute;
                margin-left: 450px;" OnClientClick="return hideSchemediv();" OnClick="imgSchemeSettingsClose_Click" />
            <div>
                <center>
                    <div id="maindiv" runat="server" class="maindivstyle" style="width: 950px; height: 510px;">
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Scheme Admission</span></div>
                        </center>
                        <br />
                        <fieldset id="fldRad" style="width: 450px; border: 1px solid #999999; background-color: #F0F0F0;
                            box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999; -webkit-box-shadow: 0px 0px 10px #999999;
                            border: 3px solid #D9D9D9; border-radius: 15px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="radApplNo" runat="server" Checked="true" Text="Application No"
                                            OnCheckedChanged="radApplNo_Change" AutoPostBack="true" GroupName="RadAppAdmNo" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="radAdmNo" runat="server" Text="Admission No" OnCheckedChanged="radAdmNo_Change"
                                            AutoPostBack="true" GroupName="RadAppAdmNo" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbduepaid" runat="server" Checked="false" Enabled="true" Text="Include Due and Paid" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <br />
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
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td colspan="4" id="tddet" runat="server" style="display: none;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblstudnameScheme" runat="server" Text="Student Name:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbstudnameScheme" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="lbappnoScheme" runat="server" Style="display: none;" Text=""></asp:Label>
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
                                                <asp:Label ID="lbldegreeScheme" runat="server" Style="display: none;" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblyearScheme" runat="server" Text="Year:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbyearScheme" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="lblclgSchemecode" runat="server" Style="display: none;" Text=""></asp:Label>
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
                                        <tr id="trAdm" runat="server" visible="true">
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
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbldt" runat="server" Text="Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_schmdate" runat="server" Style="height: 20px; width: px;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_schmdate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblgrp" runat="server" Text="Group"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlgroup" runat="server" CssClass="textbox3 textbox1" onfocus="return myFunction(this)">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btnadmitScheme" runat="server" Text="Admit" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox btn2" OnClick="btnadmitScheme_Click" />
                                                <asp:Button ID="btnclearScheme" runat="server" Text="Reset" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox btn2" OnClientClick="return clearScheme();" />
                                                <%--OnClick="btnclearScheme_Click"--%>
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
