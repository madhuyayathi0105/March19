<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FinanceUniversalReport.aspx.cs" Inherits="FinanceUniversalReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <%--    <script src="../FinanceModScripts/FinanceUniversalScript.js" type="text/javascript"></script>--%>
    <style>
        .backColor
        {
            border-color: Red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            //batch
            function CheckBoxListSelectBatch(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_batch.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }
            //degree
            function CheckBoxListSelectDegree(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_degree.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //department
            function CheckBoxListSelectDept(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_dept.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //section
            function CheckBoxListSelectSec(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_sect.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //student mode
            function CheckBoxListSelectStudMode(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_type.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //seat type
            function CheckBoxListSelectSeatType(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_seat.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }
            //sub quota
            function CheckBoxListSelectQuota(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_quota.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //student category
            function CheckBoxListSelectStudCatg(cbControl) {
                var chkBoxList = document.getElementById('<%=cblinclude.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //student type
            function CheckBoxListSelectStudType(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_stutype.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //religion
            function CheckBoxListSelectReg(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_religion.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //community
            function CheckBoxListSelectComm(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_community.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //sem
            function CheckBoxListSelectSem(cbControl) {
                var chkBoxList = document.getElementById('<%=cbl_sem.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //Header
            function CheckBoxListSelectHed(cbControl) {
                var chkBoxList = document.getElementById('<%=chkl_studhed.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //Ledger
            function CheckBoxListSelectLed(cbControl) {
                var chkBoxList = document.getElementById('<%=chkl_studled.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //financial year
            function CheckBoxListSelectFnl(cbControl) {
                var chkBoxList = document.getElementById('<%=chklsfyear.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //Route
            function CheckBoxListSelectRoute(cbControl) {
                var chkBoxList = document.getElementById('<%=cblroute.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }
            //Vehicle
            function CheckBoxListSelectVehicle(cbControl) {
                var chkBoxList = document.getElementById('<%=cblvechile.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //stage
            function CheckBoxListSelectStage(cbControl) {
                var chkBoxList = document.getElementById('<%=cblstage.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //hostel name
            function CheckBoxListSelectHstlName(cbControl) {
                var chkBoxList = document.getElementById('<%=cblhstlname.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //building name
            function CheckBoxListSelectBuildingName(cbControl) {
                var chkBoxList = document.getElementById('<%=cblbuilding.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //room Type 
            function CheckBoxListSelectRoomType(cbControl) {
                var chkBoxList = document.getElementById('<%=cblroomtype.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //room Type 
            function CheckBoxListSelectRoomName(cbControl) {
                var chkBoxList = document.getElementById('<%=cblrommName.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //Paymode
            function CheckBoxListSelectPayMode(cbControl) {
                var chkBoxList = document.getElementById('<%=chkl_paid.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }

            //gender
            function CheckBoxListSelectGender(cbControl) {
                var chkBoxList = document.getElementById('<%=cblgender.ClientID %>');
                var chkBoxCount = chkBoxList.getElementsByTagName("input");
                if (cbControl.checked == true) {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = true;
                    }
                }
                else {
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }
                }
            }
                
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#<%=btnAdd.ClientID %>').click(function () {
                    $('#<%=divaddtype.ClientID %>').show();
                    $('#<%=txtdesc.ClientID %>').val('');
                    return false;
                });
                $('#<%=btnDel.ClientID %>').click(function () {
                    var rptText = $('#<%=ddlreport.ClientID %>').find('option:selected').text();
                    if (rptText.trim() != null && rptText != "Select") {
                        var msg = confirm("Are you sure you want to delete this report type?");
                        if (msg)
                            return true;
                        else
                            return false;
                    }
                    else {
                        alert("Please select any one report type!");
                        return false;
                    }
                });

                $('#<%=btnexittype.ClientID %>').click(function () {
                    $('#<%=divaddtype.ClientID %>').hide();
                    return false;
                });

                $('#<%=btnaddtype.ClientID %>').click(function () {
                    var txtval = $('#<%=txtdesc.ClientID %>').val();
                    if (txtval == null || txtval == "") {
                        alert("Please enter the report type!");
                        return false;
                    }
                });

                $('#<%=btnclear.ClientID %>').click(function () {
                    $('#<%=txtcolorder.ClientID %>').val('');
                    $('#<%=txtallot.ClientID %>').val('');
                    $("[id*=cblcolumnorder]").removeAttr('checked');
                    $("[id*=cblcolumnorderAlt]").removeAttr('checked');
                    $('#<%=cb_column.ClientID %>').removeAttr('checked');
                    return false;
                });

                $('#<%=imgcolumn.ClientID %>').click(function () {
                    $('#<%=divcolorder.ClientID %>').hide();
                    return false;
                });
                $('#<%=btngo.ClientID %>').click(function () {
                    var rptText = $('#<%=ddlMainreport.ClientID %>').find('option:selected').text();
                    if (rptText.trim() == null || rptText == "Select") {
                        alert("Please select any one report type!");
                        return false;
                    }
                });

                $('#<%=btncolorderOK.ClientID %>').click(function () {
                    var rptText = $('#<%=ddlreport.ClientID %>').find('option:selected').text();
                    var textval = $('#<%=txtcolorder.ClientID %>').val();
                    var textvalAllot = $('#<%=txtallot.ClientID %>').val();
                    if (rptText.trim() == null || rptText == "Select") {
                        alert("Please select any one report type!");
                        return false;
                    }
                    $('#<%=txtcolorder.ClientID %>').removeClass("backColor");
                    $('#<%=txtallot.ClientID %>').removeClass("backColor");
                    if ((textval == "" || textval == null) || (textvalAllot == "" || textvalAllot == null)) {
                        if (textval == "" || textval == null) {
                            $('#<%=txtcolorder.ClientID %>').addClass("backColor");
                        }
                        if (textvalAllot == "" || textvalAllot == null) {
                            $('#<%=txtallot.ClientID %>').addClass("backColor");
                        }
                        alert("Please Select columns!");
                        return false;
                    }
                });
                //include hostel
                $('#<%=cbIncHstl.ClientID %>').change(function () {
                    $('#<%=cbIncTrans.ClientID %>').removeAttr('checked');
                    $('#<%=fldTrans.ClientID %>').hide();
                    if ($(this).is(':checked')) {
                        $('#<%=fldHstl.ClientID %>').show();
                        $('#<%=fldTransClick.ClientID %>').removeClass("backColor");
                    }
                    else {
                        $('#<%=fldHstl.ClientID %>').hide();
                        $('#<%=fldTransClick.ClientID %>').removeClass("backColor");
                    }
                    return false;
                });
                //include student details
                $('#<%=cbIncStud.ClientID %>').change(function () {
                    if ($(this).is(':checked')) {
                        $("[id*=cblinclude]").removeAttr('checked');
                        $('#<%=cbinclude.ClientID %>').removeAttr('checked');
                        $('#<%=fldstud.ClientID %>').show();
                        $('#<%=sub_quotafield.ClientID %>').show();

                    }
                    else {
                        $('#<%=fldstud.ClientID %>').hide();
                        $('#<%=sub_quotafield.ClientID %>').hide();
                    }
                    return false;
                });
                //include Transport Details
                $('#<%=cbIncTrans.ClientID %>').change(function () {
                    $('#<%=cbIncHstl.ClientID %>').removeAttr('checked');
                    $('#<%=fldHstl.ClientID %>').hide();
                    if ($(this).is(':checked')) {
                        $('#<%=fldTrans.ClientID %>').show();
                        //                        $('#<%=fldTransClick.ClientID %>').addClass("backColor");
                    }
                    else {
                        $('#<%=fldTrans.ClientID %>').hide();
                        //                        $('#<%=fldTransClick.ClientID %>').removeClass("backColor");
                    }
                    return false;
                });
                $('#<%=btnExcel.ClientID %>').click(function () {
                    var txtval = $('#<%=txtexcelname.ClientID %>').val();
                    if (txtval == null || txtval == "") {
                        $('#<%=lblvalidation1.ClientID %>').show();
                        $('#<%=lblvalidation1.ClientID %>').text("Please Enter Your  Report Name");
                        return false;
                    }
                });

                //range 31.07.2017 by sudhagar
                $('#<%=cbRange.ClientID %>').change(function () {
                    $('#<%=txtFromRange.ClientID %>').val('');
                    $('#<%=txtToRange.ClientID %>').val('');
                    if ($(this).is(':checked')) {
                        $('#<%=txtFromRange.ClientID %>').removeAttr('disabled');
                        $('#<%=txtToRange.ClientID %>').removeAttr('disabled');
                    }
                    else {
                        $('#<%=txtFromRange.ClientID %>').attr('disabled', 'disabled');
                        $('#<%=txtToRange.ClientID %>').attr('disabled', 'disabled');
                    }
                });
            });
            function columnOrderCbl() {
                $('#<%=txtcolorder.ClientID %>').removeClass("backColor");
                var txtval = document.getElementById('<%=txtcolorder.ClientID%>');
                txtval.value = "";
                var getval = "";
                var cball = document.getElementById('<%=cb_column.ClientID%>');
                var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
                var tagname = cblall.getElementsByTagName("input");
                var tagnamestr = cblall.getElementsByTagName("label");
                if (cball.checked == true) {
                    for (var i = 0; i < tagname.length; i++) {
                        tagname[i].checked = true;
                        if (getval == "")
                            getval = tagnamestr[i].innerHTML; //+ "(" + (i + 1) + ")"
                        else
                            getval += ", " + tagnamestr[i].innerHTML; //+ "(" + (i + 1) + ")"
                    }
                }
                else {
                    for (var i = 0; i < tagname.length; i++) {
                        tagname[i].checked = false;
                    }
                    getval = "";
                    oldval = "";
                }
                if (getval != "") {
                    txtval.value = getval.toString();
                }
            }

            function columnOrderCb() {
                var txtval = document.getElementById('<%=txtcolorder.ClientID%>');
                var oldval = txtval.value.toString();
                txtval.value = "";
                var newval = "";
                var getval = "";
                var count = 0;
                var cball = document.getElementById('<%=cb_column.ClientID%>');
                var cblall = document.getElementById('<%=cblcolumnorder.ClientID%>');
                var tagname = cblall.getElementsByTagName("input");
                var tagnamestr = cblall.getElementsByTagName("label");
                for (var i = 0; i < tagname.length; i++) {
                    if (tagname[i].checked == true) {
                        count += 1;
                        getval = tagnamestr[i].innerHTML; //current checked val
                        if (oldval != null && oldval != "") {
                            var result = oldval.includes(getval);
                            if (!result) {
                                oldval += "," + getval;
                            }
                        }
                        else {
                            oldval = getval;
                        }
                    }
                }
                if (tagname.length == count) {
                    cball.checked = true;
                }
                else {
                    cball.checked = false;
                }
                if (oldval != "") {
                    txtval.value = oldval.toString();
                }
            }
            //allot only
            function columnOrderCbAlt() {
                $('#<%=txtallot.ClientID %>').removeClass("backColor");
                var txtval = document.getElementById('<%=txtallot.ClientID%>');
                var oldval = txtval.value.toString();
                txtval.value = "";
                var newval = "";
                var getval = "";
                var count = 0;
                //  var cball = document.getElementById('<%=cb_column.ClientID%>');
                var cblall = document.getElementById('<%=cblcolumnorderAlt.ClientID%>');
                var tagname = cblall.getElementsByTagName("input");
                var tagnamestr = cblall.getElementsByTagName("label");
                for (var i = 0; i < tagname.length; i++) {
                    if (tagname[i].checked == true) {
                        count += 1;
                        getval = tagnamestr[i].innerHTML; //current checked val
                        if (oldval != null && oldval != "") {
                            var result = oldval.includes(getval);
                            if (!result) {
                                oldval += "," + getval;
                            }
                        }
                        else {
                            oldval = getval;
                        }

                    }
                }
                if (oldval != "") {
                    txtval.value = oldval.toString();
                }
            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <script type="text/javascript">
            function checkDate() {
                var fromDate = "";
                var toDate = "";
                var date = ""
                var date1 = ""
                var month = "";
                var month1 = "";
                var year = "";
                var year1 = "";
                var empty = "";
                fromDate = document.getElementById('<%=txt_fromdate.ClientID%>').value;
                toDate = document.getElementById('<%=txt_todate.ClientID%>').value;

                date = fromDate.substring(0, 2);
                month = fromDate.substring(3, 5);
                year = fromDate.substring(6, 10);

                date1 = toDate.substring(0, 2);
                month1 = toDate.substring(3, 5);
                year1 = toDate.substring(6, 10);
                var today = new Date();
                //  var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth() + 1;
                var yyyy = today.getFullYear();
                if (dd < 10) { dd = '0' + dd }
                if (mm < 10) { mm = '0' + mm }
                var today = dd + '/' + mm + '/' + yyyy;

                if (year == year1) {
                    if (month == month1) {
                        if (date == date1) {
                            empty = "";
                        }
                        else if (date < date1) {
                            empty = "";
                        }
                        else {
                            empty = "e";
                        }
                    }
                    else if (month < month1) {
                        empty = "";
                    }
                    else if (month > month1) {
                        empty = "e";
                    }
                }
                else if (year < year1) {
                    empty = "";
                }
                else if (year > year1) {
                    empty = "e";
                }
                if (empty != "") {
                    document.getElementById('<%=txt_fromdate.ClientID%>').value = today;
                    document.getElementById('<%=txt_todate.ClientID%>').value = today;
                    alert("To date should be greater than from date ");
                    return false;
                }
            }
            $(document).ready(function () {
                $('#<%=btnExcel.ClientID%>').click(function () {
                    var excelName = $('#<%=txtexcelname.ClientID%>').val();
                    if (excelName == null || excelName == "") {
                        $('#<%=lblvalidation1.ClientID%>').show();
                        return false;
                    }
                    else {
                        $('#<%=lblvalidation1.ClientID%>').hide();
                    }
                });

                $('#<%=txtexcelname.ClientID %>').keypress(function () {
                    $('#<%=lblvalidation1.ClientID %>').hide();
                });
               
                    return false;
                });

            });

            function cbdateChange() {
                var from = document.getElementById('<%=txt_fromdate.ClientID %>');
                var to = document.getElementById('<%=txt_todate.ClientID %>');
                var cbdt = document.getElementById('<%=cbdate.ClientID %>');
                if (cbdt.checked) {
                    from.disabled = false;
                    to.disabled = false;
                }
                else {
                    from.disabled = true;
                    to.disabled = true;
                }
            }


           
    </script>
        <div>
            <center>
                <div>
                    <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;">Finance
                        Universal Reports</span>
                </div>
            </center>
        </div>
        <center>
            <table style="background-color: rgb(204,204,204); color: Black; font-weight: normal;">
                <%--style="background-color: rgb(255, 0, 0);"--%>
                <tr>
                    <td colspan="6">
                        <fieldset style="width: 500px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblclg" Text="College" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcollegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddlcollegename_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Text="Batch"></asp:Label>
                                        <fieldset style="height: 100px; width: 90px;">
                                            <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cb_batch" runat="server" Text="All" onclick="CheckBoxListSelectBatch(this);" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                                <%--javascript: CheckBoxListSelect ('<%= cb_batch.ClientID %>',false)--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                        <fieldset>
                                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Style="height: 109px; width: 200px;">
                                                <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectDegree(this);" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                        <fieldset style="height: 109px; width: 270px;">
                                            <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectDept(this);" />
                                                <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text=" Section"></asp:Label>
                                        <fieldset style="height: 109px; width: 124px;">
                                            <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cb_sect" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectSec(this);" />
                                                <asp:CheckBoxList ID="cbl_sect" runat="server">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_sect_OnSelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <table>
                            <tr>
                                <td>
                                    <fieldset style="height: 15px; width: 260px;">
                                        <asp:RadioButtonList ID="rblsemType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblsemType_Selected"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Yearwise" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Alloted Feecategory" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 15px; width: 143px;">
                                        <asp:RadioButtonList ID="rbFeesType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbFeesType_Selected"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Header" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Ledger" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 15px; width: 119px;">
                                        <asp:CheckBox ID="cbIncStud" runat="server" Text="Student Details" Checked="false" />
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset id="fldTransClick" runat="server" style="height: 15px; width: 85px;">
                                        <asp:CheckBox ID="cbIncTrans" runat="server" Text="Transport" Checked="false" />
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 15px; width: 64px;">
                                        <asp:CheckBox ID="cbIncHstl" runat="server" Text="Hostel" Checked="false" />
                                    </fieldset>
                                </td>
                                <%--abarna 16.5.18--%>
                                <td rowspan="3" id="sub_quotafield" runat="server">
                                    <asp:Label ID="lbl_subquota" runat="server" Text="Quota/Category"></asp:Label>
                                    <fieldset style="height: 80px; width: 163px;">
                                        <asp:Panel ID="Panel24" runat="server" ScrollBars="Auto" Style="height: 80px;">
                                            <asp:CheckBox ID="cb_quota" runat="server" Text="All" onclick="CheckBoxListSelectQuota(this);" />
                                            <asp:CheckBoxList ID="cbl_quota" runat="server">
                                            </asp:CheckBoxList>
                                            <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged"--%>
                                        </asp:Panel>
                                    </fieldset>
                                </td>
                                <%-- <td colspan="3">
                                    <fieldset style="height: 15px; width: 193px;">
                                        <asp:CheckBox ID="cbAcdYear" runat="server" Text="Academic Year" />
                                        <asp:DropDownList ID="ddlAcademic" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="75px">
                                        </asp:DropDownList>
                                    </fieldset>
                                </td>--%>
                                <%--<td>
                                    <asp:Button ID="btncutof" BackColor="White" runat="server" Style="background: transparent;
                                        border: none !important; font-size: 0;" OnClick="cutofcalculation_Click" />
                                </td>--%>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <fieldset style="height: 23px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbAcdYear" runat="server" Text="" />
                                                    <asp:DropDownList ID="ddlAcademic" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="102px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblTypeNew" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Academic Year" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Odd"></asp:ListItem>
                                                        <asp:ListItem Text="Even"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="cbdate" runat="server" /><%--onclick="return cbdateChange();"--%>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 67px;"
                                                        onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 67px;" onchange="return checkDate()"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <fieldset id="fldstud" runat="server" style="display: none;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_type" runat="server" Text="Mode"></asp:Label>
                                        <fieldset style="height: 109px; width: 104px;">
                                            <asp:Panel ID="Panel11" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cb_type" runat="server" Text="All" onclick="CheckBoxListSelectStudMode(this);" />
                                                <asp:CheckBoxList ID="cbl_type" runat="server">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_type_SelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_seat" runat="server" Text="SeatType"></asp:Label>
                                        <fieldset style="height: 109px; width: 111px;">
                                            <asp:Panel ID="Panel10" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cb_seat" runat="server" Text="All" onclick="CheckBoxListSelectSeatType(this);" />
                                                <asp:CheckBoxList ID="cbl_seat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="checkdicon" runat="server" Text="Student Catagory" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Style="width: 200px;" />
                                        <fieldset style="height: 109px; width: 178px;">
                                            <asp:Panel ID="Panel9" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cbinclude" runat="server" Text="All" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    onclick="CheckBoxListSelectStudCatg(this);" />
                                                <asp:CheckBoxList ID="cblinclude" runat="server" Font-Size="Medium" Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                                <%--OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged" AutoPostBack="True"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_stutype" runat="server" Text="Student Type"></asp:Label>
                                        <fieldset style="height: 109px; width: 103px;">
                                            <asp:Panel ID="Panel12" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cb_stutype" runat="server" Text="All" onclick="CheckBoxListSelectStudType(this);" />
                                                <asp:CheckBoxList ID="cbl_stutype" runat="server">
                                                    <asp:ListItem>Day Scholar</asp:ListItem>
                                                    <asp:ListItem>Hostler</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_stutype_SelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Religion" runat="server" Text="Religion"></asp:Label>
                                        <fieldset style="height: 109px; width: 119px;">
                                            <asp:Panel ID="Panel16" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cb_religion" runat="server" Text="All" onclick="CheckBoxListSelectReg(this);" />
                                                <asp:CheckBoxList ID="cbl_religion" runat="server">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_religion_SelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Community" runat="server" Text="Community"></asp:Label>
                                        <fieldset style="height: 109px; width: 99px;">
                                            <asp:Panel ID="Panel17" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cb_community" runat="server" Text="All" onclick="CheckBoxListSelectComm(this);" />
                                                <asp:CheckBoxList ID="cbl_community" runat="server">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_community_SelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Gender"></asp:Label>
                                        <fieldset style="height: 109px; width: 71px;">
                                            <asp:Panel ID="Panel23" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cbgender" runat="server" Text="All" onclick="CheckBoxListSelectGender(this);" />
                                                <asp:CheckBoxList ID="cblgender" runat="server">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_community_SelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <fieldset>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                        <fieldset style="height: 109px; width: 145px;">
                                            <asp:Panel ID="Panel8" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="All" onclick="CheckBoxListSelectSem(this);" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblheader" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                                        <fieldset style="height: 109px; width: 200px;">
                                            <asp:Panel ID="Panel5" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="chk_studhed" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectHed(this);" />
                                                <asp:CheckBoxList ID="chkl_studhed" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studhed_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblledger" runat="server" Text="Ledger"></asp:Label>
                                        <fieldset>
                                            <asp:Panel ID="Panel6" runat="server" ScrollBars="Auto" Style="height: 109px; width: 200px;">
                                                <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectLed(this);" />
                                                <asp:CheckBoxList ID="chkl_studled" runat="server">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="PayMode"></asp:Label>
                                        <fieldset style="height: 109px; width: 118px;">
                                            <asp:Panel ID="Panel22" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="chk_paid" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectPayMode(this);" />
                                                <asp:CheckBoxList ID="chkl_paid" runat="server" OnSelectedIndexChanged="chkl_paid_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="True"--%>
                                                <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_community_SelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                                        <fieldset style="height: 109px; width: 177px;">
                                            <asp:Panel ID="Panel7" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="chkfyear" runat="server" Text="All" onclick="CheckBoxListSelectFnl(this);" />
                                                <asp:CheckBoxList ID="chklsfyear" runat="server">
                                                </asp:CheckBoxList>
                                                <%--OnSelectedIndexChanged="chklsfyear_selected"  AutoPostBack="True"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <fieldset id="fldTrans" runat="server" style="display: none;">
                            <%--class="backColor"--%>
                            <table>
                                <tr>
                                    <td>
                                        <span style="font-family: Book Antiqua;">Route ID</span>
                                        <fieldset style="height: 109px; width: 150px;">
                                            <asp:Panel ID="Panel15" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cbroute" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectRoute(this);" />
                                                <asp:CheckBoxList ID="cblroute" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblroute_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <span style="font-family: Book Antiqua;">Vechile ID</span>
                                        <fieldset style="height: 109px; width: 150px;">
                                            <asp:Panel ID="Panel13" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cbvechile" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectVehicle(this);" />
                                                <asp:CheckBoxList ID="cblvechile" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblvechile_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <span style="font-family: Book Antiqua;">Stage</span>
                                        <fieldset style="height: 109px; width: 221px;">
                                            <asp:Panel ID="Panel14" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                <asp:CheckBox ID="cbstage" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectStage(this);" />
                                                <asp:CheckBoxList ID="cblstage" runat="server">
                                                </asp:CheckBoxList>
                                                <%--AutoPostBack="true" OnSelectedIndexChanged="cblstage_OnSelectedIndexChanged"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <fieldset id="fldHstl" runat="server" style="display: none;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblhtslname" Text="Hostel" runat="server"></asp:Label>
                                        <fieldset>
                                            <asp:Panel ID="Panel18" runat="server" ScrollBars="Auto" Style="height: 109px; width: 200px;">
                                                <asp:CheckBox ID="cbhstlname" runat="server" Text="All" onclick="CheckBoxListSelectHstlName(this);" />
                                                <asp:CheckBoxList ID="cblhstlname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblhstlname_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" Text="Building" runat="server"></asp:Label>
                                        <fieldset>
                                            <asp:Panel ID="Panel20" runat="server" ScrollBars="Auto" Style="height: 109px; width: 200px;">
                                                <asp:CheckBox ID="cbbuilding" runat="server" Text="All" onclick="CheckBoxListSelectBuildingName(this);" />
                                                <asp:CheckBoxList ID="cblbuilding" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbbuilding_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblroomtype" Text="Room Type" runat="server"></asp:Label>
                                        <fieldset>
                                            <asp:Panel ID="Panel19" runat="server" ScrollBars="Auto" Style="height: 109px; width: 200px;">
                                                <asp:CheckBox ID="cbroomtype" runat="server" Text="All" onclick="CheckBoxListSelectRoomType(this);" />
                                                <asp:CheckBoxList ID="cblroomtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblroomtype_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" Text="Room Name" runat="server"></asp:Label>
                                        <fieldset>
                                            <asp:Panel ID="Panel21" runat="server" ScrollBars="Auto" Style="height: 109px; width: 200px;">
                                                <asp:CheckBox ID="cbrommName" runat="server" Text="All" onclick="CheckBoxListSelectRoomName(this);" />
                                                <asp:CheckBoxList ID="cblrommName" runat="server" AutoPostBack="true">
                                                </asp:CheckBoxList>
                                                <%--OnSelectedIndexChanged="cblrommName_SelectedIndexChange"--%>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <table>
                            <tr>
                                <td>
                                    <fieldset style="height: 15px; width: 180px;">
                                        <asp:RadioButtonList ID="rblrptType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblrptType_Selected"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Detailed" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Cumulative" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </fieldset>
                                </td>
                                <td>
                                    <asp:Label ID="lblrpt" runat="server" Text="Report"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlMainreport" runat="server" CssClass="textbox textbox1 ddlheight4"
                                        Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlMainreport_Selected">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <span>Order By</span>
                                    <asp:DropDownList ID="ddlordBy" runat="server" CssClass="textbox textbox1 ddlheight4"
                                        Width="80px">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="3" id="tdPaid" runat="server" visible="false">
                                    <fieldset style="height: 15px; width: 273px;">
                                        <asp:CheckBox ID="cbPaymode" runat="server" Text="Paymode" />
                                        <asp:CheckBox ID="cbtotPaid" runat="server" Text="Total Paid" />
                                        <asp:CheckBox ID="cbtotBal" runat="server" Text="Total Balance" />
                                    </fieldset>
                                </td>
                                <td colspan="6" id="tdRange" runat="server" visible="false">
                                    <fieldset style="height: 21px; width: 361px;">
                                        <asp:CheckBox ID="cbRange" runat="server" Text="Range" />
                                        <asp:DropDownList ID="ddlRange" runat="server" CssClass="textbox textbox1 ddlheight4"
                                            Width="100px">
                                        </asp:DropDownList>
                                        <span>From</span>
                                        <asp:TextBox ID="txtFromRange" runat="server" Enabled="false" Style="height: 20px;
                                            width: 60px;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtFromRange"
                                            FilterType="Numbers" ValidChars="." InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                        <span>To</span>
                                        <asp:TextBox ID="txtToRange" runat="server" Enabled="false" Style="height: 20px;
                                            width: 60px;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtToRange"
                                            FilterType="Numbers" ValidChars="." InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                    </fieldset>
                                </td>
                                <td>
                                    <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="lnkcolorder" runat="server" Width="30px" Height="30px" Text="All"
                                        ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="lnkcolorder_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                            BorderWidth="0px" Width="980px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            class="spreadborder">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                </tr>
                <tr>
                    <td>
                        <center>
                            <div id="print" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Visible="false" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel" runat="server" Visible="false" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                                    Height="32px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmasterhed" runat="server" Visible="false" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Print Setting" OnClick="btnprintmaster_Click" Height="32px"
                                    Style="margin-top: 10px;" CssClass="textbox textbox1" Width="100px" />
                                <%--added by deepali 01.11.2017--%>
                                <asp:Button ID="btn_print" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Print" OnClick="btn_print_Click" Height="32px" Style="margin-top: 10px;"
                                    CssClass="textbox textbox1" Width="60px" />
                                <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                            </div>
                        </center>
                    </td>
                </tr>
            </table>
        </center>
        <%--column order--%>
        <center>
            <div id="divcolorder" runat="server" style="height: 100%; display: none; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <asp:ImageButton ID="imgcolumn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 83px; margin-left: 403px;" />
                <%--   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>--%>
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 580px;
                        width: 850px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
                        border-radius: 10px;">
                        <center>
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblcolr" runat="server" Text="Column Order" Style="font-family: Book Antiqua;
                                            font-size: 20px; font-weight: bold; color: Green;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblrptype" Text="Report Type" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAdd" runat="server" Text="+" CssClass="textbox textbox1 btn1" /><%--OnClick="btnAdd_OnClick"--%>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlreport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged"
                                                            CssClass="textbox textbox1 ddlheight4">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDel" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                                            OnClick="btnDel_OnClick" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" onchange="return columnOrderCbl()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtcolorder" runat="server" Columns="20" Style="height: 70px; width: 800px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="800px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                            RepeatDirection="Horizontal" onclick="return columnOrderCb()">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtallot" runat="server" Columns="20" Style="height: 70px; width: 800px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorderAlt" runat="server" Height="43px" Width="800px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                            RepeatDirection="Horizontal" onclick="return columnOrderCbAlt()">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <center>
                                            <asp:Button ID="btncolorderOK" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btncolorderOK_Click" Text="OK" runat="server" />
                                            <asp:Button ID="btnclear" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                Text="Clear" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
                <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
            </div>
        </center>
        <%--report type name enter text box--%>
        <center>
            <div id="divaddtype" runat="server" style="height: 100%; z-index: 10000; width: 100%;
                background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
                display: none;">
                <center>
                    <div id="panel_description11" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbldesc" runat="server" Text="Description" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txtdesc" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                        margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnaddtype" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btnaddtype_Click" />
                                    <asp:Button ID="btnexittype" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox btn1" /><%--OnClick="btnexittype_Click"--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </center>
    </body>
</asp:Content>
